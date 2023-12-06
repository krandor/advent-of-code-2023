namespace Fertilizer.DTO;

using System.Collections.Generic;

public class SeedListDto
{
    public List<(long min, long max)> SeedRanges { get; set; } = new List<(long min, long max)>();
}