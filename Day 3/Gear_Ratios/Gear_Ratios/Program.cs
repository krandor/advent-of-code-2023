namespace Cube_Conundrum
{
    using System;
    using System.IO.Abstractions;
    using System.Linq.Expressions;

    internal class Program
    {
        private static List<char> symbolChars = new List<char>
        {
            '!',
            '@',
            '#',
            '$',
            '%',
            '^',
            '&',
            '*',
            '(',
            ')',
            '-',
            '_',
            '=',
            '+',
            '/',
            '?',
            '>',
            '<',
            ',',
        };

        private static (List<FoundNumber>, List<FoundSymbol>) FindNumbersAndSymbols(string line, int lineIndex)
        {
            var foundNumbers = new List<FoundNumber>();
            var foundSymbols = new List<FoundSymbol>();

            var numberString = string.Empty;
            for (var i = 0; i < line.Length; i++)
            {
                var character = line[i];
                if (!char.IsDigit(character))
                {
                    if (!string.IsNullOrEmpty(numberString))
                    {
                        foundNumbers.Add(new FoundNumber()
                        {
                            Value = int.Parse(numberString),
                            EndIndex = i - 1,
                            StartIndex = i - numberString.Length,
                            LineIndex = lineIndex,
                        });

                        numberString = string.Empty;
                    }

                    if (SymbolFound(character))
                    {
                        foundSymbols.Add(new FoundSymbol()
                        {
                            Index = i,
                            LineIndex = lineIndex,
                            Value = character,
                        });
                    }
                }
                else
                {
                    numberString += character;

                    if (i + 1 == line.Length) // last character in line
                    {
                        foundNumbers.Add(new FoundNumber()
                        {
                            Value = int.Parse(numberString),
                            EndIndex = i,
                            StartIndex = i - numberString.Length + 1,
                            LineIndex = lineIndex,
                        });

                        numberString = string.Empty;
                    }
                }
            }

            return (foundNumbers, foundSymbols);
        }

        private static List<FoundNumber> FindSymbols(string currentLine, string lineAbove, string lineBelow, List<FoundNumber> foundNumbersList)
        {
            var numbersNearSymbols = new List<FoundNumber>();

            foreach (var number in foundNumbersList)
            {
                var startIndex = number.StartIndex > 0 ? number.StartIndex - 1 : 0;
                var endIndex = number.EndIndex < currentLine.Length - 1 ? number.EndIndex + 1 : currentLine.Length - 1;

                // check current line front
                if (SymbolFound(currentLine[startIndex]))
                {
                    numbersNearSymbols.Add(number);
                    continue;
                }

                // check currentt line back
                if (SymbolFound(currentLine[endIndex]))
                {
                    numbersNearSymbols.Add(number);
                    continue;
                }

                var symbolFound = false;

                // check line above front to back
                if (!string.IsNullOrEmpty(lineAbove))
                {
                    symbolFound = SymbolFoundInLine(lineAbove, numbersNearSymbols, number, startIndex, endIndex);
                }

                if (!symbolFound)
                {
                    // check line below front to back
                    if (!string.IsNullOrEmpty(lineBelow))
                    {
                        SymbolFoundInLine(lineBelow, numbersNearSymbols, number, startIndex, endIndex);
                    }
                }
            }

            return numbersNearSymbols;
        }

        private static async Task Main(string[] args)
        {
            var fileSystem = new FileSystem();
            var lines = await fileSystem.File.ReadAllLinesAsync("input.txt");

            var partsSum = 0;

            var allNumbers = new List<FoundNumber>();
            var allSymbols = new List<FoundSymbol>();

            for (var i = 0; i < lines.Length; i++)
            {
                var currentLine = lines[i];

                // get numbers
                var foundNumbersAndSymbols = FindNumbersAndSymbols(currentLine, i);

                allNumbers.AddRange(foundNumbersAndSymbols.Item1);
                allSymbols.AddRange(foundNumbersAndSymbols.Item2);
            }

            // Find numbers near *
            foreach (var symbol in allSymbols)
            {
                if (symbol.Value != '*')
                {
                    continue;
                }

                var indexAbove = symbol.LineIndex == 0 ? 0 : symbol.LineIndex - 1;
                var indexBelow = symbol.LineIndex == lines.Length - 1 ? symbol.LineIndex : symbol.LineIndex + 1;
                var currentIndex = symbol.LineIndex;

                var adjacentParts = allNumbers.Where(n =>
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

        private static bool SymbolFound(char character)
        {
            if (symbolChars.Contains(character))
            {
                return true;
            }

            return false;
        }

        private static bool SymbolFoundInLine(string lineAbove, List<FoundNumber> numbersNearSymbols, FoundNumber number, int startIndex, int endIndex)
        {
            var symbolFound = false;

            for (var i = startIndex; i <= endIndex; i++)
            {
                if (SymbolFound(lineAbove[i]))
                {
                    numbersNearSymbols.Add(number);
                    symbolFound = true;
                    break;
                }
            }

            return symbolFound;
        }

        private class FoundNumber
        {
            public int EndIndex { get; set; }

            public int LineIndex { get; set; }

            public int StartIndex { get; set; }

            public int Value { get; set; }
        }

        private class FoundSymbol
        {
            public int Index { get; set; }

            public int LineIndex { get; set; }

            public char Value { get; set; }
        }
    }
}