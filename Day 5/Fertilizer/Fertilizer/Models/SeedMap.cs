namespace Fertilizer.Models;

using System.Collections.Generic;

public class SeedMap
{
    public List<DependencyMap> Dependencies { get; set; } = new List<DependencyMap>();

    public long Seed { get; set; }
}