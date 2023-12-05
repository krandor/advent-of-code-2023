namespace Cube_Conundrum;

using Gear_Ratios.InputProcessors;
using System;
using System.IO.Abstractions;

internal partial class Program
{
    private static async Task Main(string[] args)
    {
        var fileSystem = new FileSystem();
        var lines = await fileSystem.File.ReadAllLinesAsync("test.txt");

        var inputProcessor = new InputProcessor();

        var partsSum = 0;

        var processResults = await inputProcessor.Process(lines);

        // Find numbers near *
        foreach (var symbol in processResults.FoundSymbols)
        {
            if (symbol.Value != '*')
            {
                continue;
            }

            var indexAbove = symbol.LineIndex == 0 ? 0 : symbol.LineIndex - 1;
            var indexBelow = symbol.LineIndex == lines.Length - 1 ? symbol.LineIndex : symbol.LineIndex + 1;
            var currentIndex = symbol.LineIndex;

            var adjacentParts = processResults.FoundNumbers.Where(n =>
                    (n.LineIndex == currentIndex || n.LineIndex == indexAbove || n.LineIndex == indexBelow) &&
                    symbol.Index <= n.EndIndex + 1 &&
                    symbol.Index >= n.StartIndex - 1).ToList();

            if (adjacentParts.Count() == 2)
            {
                var product = adjacentParts[0].Value * adjacentParts[1].Value;
                partsSum += product;
            }
        }

        Console.WriteLine($"Parts Sum: {partsSum}");
    }
}