namespace Fertilizer;

using Fertilizer.InputProcessors;
using Fertilizer.Logic;
using System;
using System.IO.Abstractions;

internal partial class Program
{
    private static async Task Main(string[] args)
    {
        var fileSystem = new FileSystem();
        var lines = await fileSystem.File.ReadAllLinesAsync("input.txt");

        var inputProcessor = new InputProcessor();
        var processResults = await inputProcessor.Process(lines);
        var mapGroups = processResults.ItemMapDtos.GroupBy(m => $"{m.From}-to-{m.To}");

        Console.WriteLine($"Seed Count: {processResults.SeedListDto.SeedRanges.Count}");
        foreach (var group in mapGroups)
        {
            Console.WriteLine($"{group.Key}: {group.Count()}");
        }

        IProblemLogic problemLogic = new ProblemLogic();
        var logicResults = problemLogic.Process(processResults);

        Console.WriteLine($"lowest: {logicResults.LowestLocation}");

        Console.ReadLine();
    }
}