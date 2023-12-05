namespace Scratchcards.InputProcessors;

using System.Threading.Tasks;
using Scratchcards.Models;

public class InputProcessor : IInputProcessor
{
    public Task<ProcessResults> Process(string[] input)
    {
        var processResult = new ProcessResults();

        for (var i = 0; i < input.Length; i++)
        {
            var card = new Card();
            var currentLine = input[i];

            // get card data
            var cardParts = currentLine.Split(':');
            card.CardId = cardParts[0];

            var numberParts = cardParts[1].Split("|", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var winningNumbers = numberParts[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var playerNumbers = numberParts[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries); ;

            card.WinningNumbers.AddRange(winningNumbers.Select(n => int.Parse(n)));
            card.PlayerNumbers.AddRange(playerNumbers.Select(n => int.Parse(n)));

            processResult.Cards.Add(card);
        }

        return Task.FromResult(processResult);
    }
}