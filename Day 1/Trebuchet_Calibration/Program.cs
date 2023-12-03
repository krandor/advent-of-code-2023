namespace AdventOfCode2023;

using System.IO.Abstractions;

internal class Program
{
    private static IFileSystem FileSystem;

    public static async Task Main(string[] args)
    {
        FileSystem = new FileSystem();
        var lines = await FileSystem.File.ReadAllLinesAsync("input.txt");

        var inputParser = new InputParser();

        var total = inputParser.Parse(lines);

        Console.WriteLine($"Total: {total}");
    }
}