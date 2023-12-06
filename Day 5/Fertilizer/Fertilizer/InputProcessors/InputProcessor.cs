namespace Fertilizer.InputProcessors;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fertilizer.DTO;
using Fertilizer.Models;

public class InputProcessor : IInputProcessor
{
    private readonly string mapRegexPattern = "(\\w+)-to-(\\w+)\\s?map:";
    private readonly string seedRegexPattern = "seeds:";

    public Task<ProcessResults> Process(string[] input)
    {
        var processResult = new ProcessResults();

        for (var i = 0; i < input.Length; i++)
        {
            var seedMatch = Regex.Match(input[i], this.seedRegexPattern);
            if (seedMatch.Success)
            {
                this.HandleSeeds(processResult, input[i]);
            }

            var mapMatch = Regex.Match(input[i], this.mapRegexPattern);
            if (mapMatch.Success)
            {
                this.HandleMap(processResult, input, i, mapMatch);
            }
        }

        return Task.FromResult(processResult);
    }

    private void HandleMap(ProcessResults processResult, string[] input, int index, Match mapMatch)
    {
        var itemMapDtos = new List<ItemMapDto>();
        var mapIndex = index + 1;

        var mapFrom = mapMatch.Groups[1].Value;
        var mapTo = mapMatch.Groups[2].Value;

        while (mapIndex < input.Length && !string.IsNullOrEmpty(input[mapIndex]) && char.IsDigit(input[mapIndex].First()))
        {
            var itemMap = new ItemMapDto()
            {
                From = mapFrom,
                To = mapTo,
            };

            var numbers = input[mapIndex].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(s =>
            {
                if (long.TryParse(s, out var mapId))
                {
                    return mapId;
                }

                return -1L;
            }).ToList();

            if (numbers.Count() == 3)
            {
                itemMap.Destination = numbers[0];
                itemMap.Source = numbers[1];
                itemMap.Range = numbers[2];
            }

            mapIndex++;

            itemMapDtos.Add(itemMap);
        }

        processResult.ItemMapDtos.AddRange(itemMapDtos);
    }

    private void HandleSeeds(ProcessResults processResult, string line)
    {
        var perPage = 2;

        var lineArray = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var numbers = lineArray[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var maxPages = numbers.Length / 2;

        for (var page = 0; page < maxPages; page++)
        {
            var range = numbers.Skip(page * perPage).Take(perPage).ToList();

            var start = long.Parse(range[0]);
            var count = long.Parse(range[1]);

            processResult.SeedListDto.SeedRanges.Add((start, start + count));
        }
    }
}