namespace Fertilizer.Logic;

using Fertilizer.DTO;
using Fertilizer.Extensions;
using Fertilizer.Models;

public class ProblemLogic : IProblemLogic
{
    public static IEnumerable<long> Int64Enumerable(long from, long to, long step)
    {
        if (step <= 0L) step = (step == 0L) ? 1L : -step;

        if (from <= to)
        {
            for (long l = from; l <= to; l += step) yield return l;
        }
        else
        {
            for (long l = from; l >= to; l -= step) yield return l;
        }
    }

    public LogicResults Process(ProcessResults processResults)
    {
        var logicResults = new LogicResults();
        var locations = new List<long>();
        var itemMapDtos = processResults.ItemMapDtos;
        Parallel.ForEach(processResults.SeedListDto.SeedRanges, seedRange =>
        {
            Parallel.ForEach(Int64Enumerable(seedRange.min, seedRange.max - seedRange.min, 1L), seed =>
            {
                var fromSeedMaps = processResults.ItemMapDtos.Where(m => m.From == "seed");
                var matchedFromSeedMaps = fromSeedMaps.Where(m => seed >= m.Source && seed <= m.Source + m.Range);

                if (!matchedFromSeedMaps.Any())
                {
                    var dependency = new DependencyMap()
                    {
                        Type = fromSeedMaps.First().To,
                        TypeId = seed,
                    };

                    var manualMapDto = new ItemMapDto()
                    {
                        From = "seed",
                        To = fromSeedMaps.First().To,
                        Source = seed,
                        Destination = seed,
                        Range = 0,
                    };

                    this.ProcessMap(dependency, manualMapDto, ref itemMapDtos);
                    AddLowestLocation(locations, dependency);
                }

                foreach (var map in matchedFromSeedMaps)
                {
                    var dependency = new DependencyMap()
                    {
                        Type = map.To,
                        TypeId = (seed - map.Source) + map.Destination,
                    };

                    this.ProcessMap(dependency, map, ref itemMapDtos);
                    AddLowestLocation(locations, dependency);
                }
            });
        });

        logicResults.LowestLocation = locations.Min();

        return logicResults;
    }

    private static void AddLowestLocation(List<long> locations, DependencyMap dependency)
    {
        var localLocations = new List<long>();
        var flattenedDependencies = dependency.Dependencies.Map(p => true, (n) => { return n.Dependencies; });
        var map = flattenedDependencies.OrderBy(d => d.TypeId).FirstOrDefault(d => d.Type.Equals("location", StringComparison.InvariantCultureIgnoreCase));

        if (map != null && (!locations.Any() || locations.Min() > map.TypeId))
        {
            locations.Add(map.TypeId);
        }
    }

    private void ProcessMap(DependencyMap dependencyMap, ItemMapDto itemMap, ref List<ItemMapDto> itemMapDtos)
    {
        var allDestinations = itemMapDtos.Where(m => m.From == itemMap.To);

        var matchedDestinations = allDestinations.Where(m => dependencyMap.TypeId >= m.Source && dependencyMap.TypeId <= m.Source + m.Range).ToList();

        if (!matchedDestinations.Any() && allDestinations.Any())
        {
            var dependency = new DependencyMap()
            {
                Type = allDestinations.First().To,
                TypeId = dependencyMap.TypeId,
            };

            var manualMapDto = new ItemMapDto()
            {
                From = allDestinations.First().From,
                To = allDestinations.First().To,
                Source = dependencyMap.TypeId,
                Destination = dependencyMap.TypeId,
                Range = 0,
            };

            this.ProcessMap(dependency, manualMapDto, ref itemMapDtos);

            dependencyMap.Dependencies.Add(dependency);
        }

        foreach (var destination in matchedDestinations)
        {
            var dependency = new DependencyMap()
            {
                Type = destination.To,
                TypeId = (dependencyMap.TypeId - destination.Source) + destination.Destination,
            };

            this.ProcessMap(dependency, destination, ref itemMapDtos);

            dependencyMap.Dependencies.Add(dependency);
        }
    }
}