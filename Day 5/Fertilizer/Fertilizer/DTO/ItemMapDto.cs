namespace Fertilizer.DTO;

public class ItemMapDto
{
    public long Destination { get; set; }

    public string From { get; set; } = string.Empty;

    public long Range { get; set; }

    public long Source { get; set; }

    public string To { get; set; } = string.Empty;
}