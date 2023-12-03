namespace Cube_Conundrum
{
    using System.IO.Abstractions;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var fileSystem = new FileSystem();
            var lines = await fileSystem.File.ReadAllLinesAsync("input.txt");
            var gameIdScore = 0;

            var allowedColors = new List<string> { "red", "green", "blue" };

            foreach (var line in lines)
            {
                var cubeMaximums = new Dictionary<string, int>();

                var gameData = line.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var gameRound = gameData[0];
                var gameRoundData = gameRound.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var gameId = int.Parse(gameRoundData[1]);
                var gameSets = gameData[1].Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var gamePower = 1;
                foreach (var gameSet in gameSets)
                {
                    var cubeDataItems = gameSet.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    foreach (var cubeDataItem in cubeDataItems)
                    {
                        var cubeData = cubeDataItem.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                        var cubeCount = int.Parse(cubeData[0]);
                        var cubeColor = cubeData[1];

                        if (cubeMaximums.ContainsKey(cubeColor))
                        {
                            if (cubeCount > cubeMaximums[cubeColor])
                            {
                                cubeMaximums[cubeColor] = cubeCount;
                            }
                        }
                        else
                        {
                            cubeMaximums.Add(cubeColor, cubeCount);
                        }
                    }
                }

                foreach (var allowedColor in allowedColors)
                {
                    if (cubeMaximums.ContainsKey(allowedColor))
                    {
                        gamePower *= cubeMaximums[allowedColor];
                    }
                }

                gameIdScore += gamePower;

                Console.WriteLine($"{gameRound} - {gameSets.Length} sets. Game Power: {gamePower} Current Game Score: {gameIdScore}");
            }
        }
    }
}