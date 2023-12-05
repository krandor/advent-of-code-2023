namespace Gear_Ratios.InputProcessors;

using System.Collections.Generic;
using System.Threading.Tasks;
using Cube_Conundrum;
using Gear_Ratios.Models;

public class InputProcessor : IInputProcessor
{
    private static Dictionary<string, char> numberWords = new Dictionary<string, char>()
        {
            { "zero", '0' },
            { "one", '1' },
            { "two", '2' },
            { "three", '3' },
            { "four", '4' },
            { "five", '5' },
            { "six", '6' },
            { "seven", '7' },
            { "eight", '8' },
            { "nine", '9' },
        };

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

    public Task<ProcessResult> Process(string[] input)
    {
        var processResult = new ProcessResult();

        for (var i = 0; i < input.Length; i++)
        {
            var currentLine = input[i];

            // get numbers
            var foundNumbersAndSymbols = FindNumbersAndSymbols(currentLine, i);

            processResult.FoundNumbers.AddRange(foundNumbersAndSymbols.Item1);
            processResult.FoundSymbols.AddRange(foundNumbersAndSymbols.Item2);
            processResult.FoundNumberWords.AddRange(foundNumbersAndSymbols.Item3);
        }

        return Task.FromResult(processResult);
    }

    private static (List<FoundNumber>, List<FoundSymbol>, List<FoundNumberWord>) FindNumbersAndSymbols(string line, int lineIndex)
    {
        var foundNumbers = new List<FoundNumber>();
        var foundSymbols = new List<FoundSymbol>();
        var foundNumberWords = new List<FoundNumberWord>();

        var numberString = string.Empty;
        var numberWordString = string.Empty;

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
                else
                {
                    numberWordString += character;

                    foreach (var key in numberWords.Keys)
                    {
                        if (numberWordString.Contains(key, StringComparison.InvariantCultureIgnoreCase))
                        {
                            // if the collected characters contain a numberWord, take it's value and add it to the numbers "string" and break out of this loop
                            foundNumberWords.Add(new FoundNumberWord()
                            {
                                ValueString = $"{key}",
                                Value = int.Parse($"{numberWords[key]}"),
                                LineIndex = lineIndex,
                                StartIndex = i - key.Length,
                                EndIndex = i - 1,
                            });

                            numberWordString = string.Empty;
                            numberWordString += character;
                            break;
                        }
                    }
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

        return (foundNumbers, foundSymbols, foundNumberWords);
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
}