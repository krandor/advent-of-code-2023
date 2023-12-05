namespace Gear_Ratios.Models;

using Cube_Conundrum;

public class ProcessResult
{
    public List<FoundNumber> FoundNumbers { get; set; } = new List<FoundNumber>();

    public List<FoundNumberWord> FoundNumberWords { get; set; } = new List<FoundNumberWord>();

    public List<FoundSymbol> FoundSymbols { get; set; } = new List<FoundSymbol>();
}