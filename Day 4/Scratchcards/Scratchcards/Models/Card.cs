namespace Scratchcards.Models;

using System.Collections.Generic;

public class Card
{
    public string CardId { get; set; } = string.Empty;

    public List<int> PlayerNumbers { get; set; } = new List<int>();

    public List<int> WinningNumbers { get; set; } = new List<int>();
}