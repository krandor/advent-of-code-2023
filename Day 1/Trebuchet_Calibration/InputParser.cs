namespace AdventOfCode2023
{
    using System.Collections.Generic;
    using System.Linq;

    public class InputParser
    {
        private static Dictionary<string, char> numberWords = new Dictionary<string, char>()
        {
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

        public int Parse(string[] lines)
        {
            var total = 0;
            foreach (var line in lines)
            {
                var numbersInLine = new List<char>();
                var letterChain = string.Empty;

                // for each character in the string
                foreach (var c in line.Trim())
                {
                    // if the character is a digit, add it to the "numbers" string and iterate to next character
                    if (char.IsDigit(c))
                    {
                        numbersInLine.Add(c);
                        letterChain = string.Empty;
                        continue;
                    }

                    // if the character is not a digit add it to the temp string
                    letterChain += c;

                    // iterate over the numberWords and check if the number exists in the collected characters
                    foreach (var key in numberWords.Keys)
                    {
                        if (letterChain.Contains(key, StringComparison.InvariantCultureIgnoreCase))
                        {
                            // if the collected characters contain a numberWord, take it's value and add it to the numbers "string" and break out of this loop
                            numbersInLine.Add(numberWords[key]);
                            letterChain = string.Empty;
                            letterChain += c;
                            break;
                        }
                    }
                }

                // if this current line contains any numbers, parse them and add to the total
                if (numbersInLine.Any())
                {
                    var numberString = $"{numbersInLine.First()}{numbersInLine.Last()}";
                    if (!string.IsNullOrEmpty(numberString))
                    {
                        var parsedInt = int.Parse(numberString);
                        total += parsedInt;

                        Console.WriteLine($"{parsedInt}");
                    }
                }
            }

            return total;
        }
    }
}