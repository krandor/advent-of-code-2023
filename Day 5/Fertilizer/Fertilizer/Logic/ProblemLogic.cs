namespace Fertilizer.Logic;

using Fertilizer.Models;

public class ProblemLogic : IProblemLogic
{
    public LogicResults Process(ProcessResults processResults)
    {
        var logicResults = new LogicResults();
        var smallestLocation = long.MaxValue;
        var itemMapDtos = processResults.ItemMapDtos;

        var soilMaps = processResults.ItemMapDtos.Where(m => m.To == "soil");
        var fertilizerMaps = processResults.ItemMapDtos.Where(m => m.To == "fertilizer");
        var waterMaps = processResults.ItemMapDtos.Where(m => m.To == "water");
        var lightMaps = processResults.ItemMapDtos.Where(m => m.To == "light");
        var temperatureMaps = processResults.ItemMapDtos.Where(m => m.To == "temperature");
        var humidityMaps = processResults.ItemMapDtos.Where(m => m.To == "humidity");
        var locationMaps = processResults.ItemMapDtos.Where(m => m.To == "location");

        var batch = 0;
        var batches = processResults.SeedListDto.SeedRanges.OrderByDescending(r => r.max - r.min).Select(r => new { Batch = batch++, Range = r });

        Parallel.ForEach(batches, seedBatch =>
        {
            var batch = seedBatch.Batch;
            var seedRange = seedBatch.Range;

            var localSmallestLocation = long.MaxValue;

            Console.WriteLine($"Batch: {batch} Start: {seedRange.min} End: {seedRange.max} Count: {seedRange.max - seedRange.min} Started: {DateTime.Now}");

            for (var seed = seedRange.min; seed <= seedRange.max; seed++)
            {
                // get the soil
                var itemMap = soilMaps.FirstOrDefault(m => seed >= m.Source && seed <= m.Source + m.Range);
                var soilId = seed;

                if (itemMap != null)
                {
                    soilId = (seed - itemMap.Source) + itemMap.Destination;
                }

                // get the fertilizer
                itemMap = fertilizerMaps.FirstOrDefault(m => soilId >= m.Source && soilId <= m.Source + m.Range);
                var fertilizerId = soilId;

                if (itemMap != null)
                {
                    fertilizerId = (soilId - itemMap.Source) + itemMap.Destination;
                }

                // get the water
                itemMap = waterMaps.FirstOrDefault(m => fertilizerId >= m.Source && fertilizerId <= m.Source + m.Range);
                var waterId = fertilizerId;

                if (itemMap != null)
                {
                    waterId = (fertilizerId - itemMap.Source) + itemMap.Destination;
                }

                // get the light
                itemMap = lightMaps.FirstOrDefault(m => waterId >= m.Source && waterId <= m.Source + m.Range);
                var lightId = waterId;

                if (itemMap != null)
                {
                    lightId = (waterId - itemMap.Source) + itemMap.Destination;
                }

                // get the temp
                itemMap = temperatureMaps.FirstOrDefault(m => lightId >= m.Source && lightId <= m.Source + m.Range);
                var temperatureId = lightId;
                if (itemMap != null)
                {
                    temperatureId = (lightId - itemMap.Source) + itemMap.Destination;
                }

                // get the humidity
                itemMap = humidityMaps.FirstOrDefault(m => temperatureId >= m.Source && temperatureId <= m.Source + m.Range);
                var humidityId = temperatureId;

                if (itemMap != null)
                {
                    humidityId = (temperatureId - itemMap.Source) + itemMap.Destination;
                }

                // get the location
                itemMap = locationMaps.FirstOrDefault(m => humidityId >= m.Source && humidityId <= m.Source + m.Range);
                var locationId = humidityId;

                if (itemMap != null)
                {
                    locationId = (humidityId - itemMap.Source) + itemMap.Destination;
                }

                if (locationId < localSmallestLocation)
                {
                    localSmallestLocation = locationId;
                }
            }

            if (smallestLocation > localSmallestLocation)
            {
                smallestLocation = localSmallestLocation;
            }

            Console.WriteLine($"Batch: {batch} Start: {seedRange.min} End: {seedRange.max} Local Smallest Location: {localSmallestLocation} Global Smallest Location: {smallestLocation} Ended: {DateTime.Now}");
        });

        logicResults.LowestLocation = smallestLocation;

        return logicResults;
    }
}