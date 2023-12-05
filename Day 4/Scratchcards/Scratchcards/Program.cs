namespace Scratchcards;

using Scratchcards.InputProcessors;
using Scratchcards.Models;
using System;
using System.IO.Abstractions;

internal partial class Program
{
    private static void AddCardsToStack(Dictionary<string, List<Card>> cardStacks, KeyValuePair<string, List<Card>> kvp)
    {
        if (cardStacks.ContainsKey(kvp.Key))
        {
            cardStacks[kvp.Key].AddRange(kvp.Value);
        }
        else
        {
            cardStacks.Add(kvp.Key, kvp.Value);
        }
    }

    private static void AddCardToStack(Dictionary<string, List<Card>> cardStacks, Card matchedCard)
    {
        if (cardStacks.ContainsKey(matchedCard.CardId))
        {
            cardStacks[matchedCard.CardId].Add(matchedCard);
        }
        else
        {
            cardStacks.Add(matchedCard.CardId, new List<Card> { matchedCard });
        }
    }

    private static async Task Main(string[] args)
    {
        var fileSystem = new FileSystem();
        var lines = await fileSystem.File.ReadAllLinesAsync("input.txt");

        var inputProcessor = new InputProcessor();

        var processResults = await inputProcessor.Process(lines);

        var cardStacks = ProcessCards(processResults.Cards, 0, processResults.Cards.Count);

        Console.WriteLine($"Parts Sum: {cardStacks.SelectMany(c => c.Value).Count()}");
    }

    private static Dictionary<string, List<Card>> ProcessCard(List<Card> cards, int startIndex)
    {
        var card = cards[startIndex];
        var cardStacks = new Dictionary<string, List<Card>>();

        var matches = card.PlayerNumbers.Intersect(card.WinningNumbers);
        var matchCount = matches.Count();
        var matchedCards = cards.Skip(startIndex + 1).Take(matchCount);

        foreach (var matchedCard in matchedCards)
        {
            AddCardToStack(cardStacks, matchedCard);
        }

        if (matches.Any())
        {
            if (startIndex + 1 < cards.Count)
            {
                for (var n = startIndex + 1; n <= startIndex + matchCount; n++)
                {
                    var stacks = ProcessCard(cards, n);
                    foreach (var kvp in stacks)
                    {
                        AddCardsToStack(cardStacks, kvp);
                    }
                }
            }
        }

        return cardStacks;
    }

    private static Dictionary<string, List<Card>> ProcessCards(List<Card> cards, int startIndex, int count)
    {
        var cardStacks = new Dictionary<string, List<Card>>();

        for (var i = startIndex; i < count; i++)
        {
            AddCardToStack(cardStacks, cards[i]);
            var stacks = ProcessCard(cards, i);
            foreach (var kvp in stacks)
            {
                AddCardsToStack(cardStacks, kvp);
            }
        }

        return cardStacks;
    }
}