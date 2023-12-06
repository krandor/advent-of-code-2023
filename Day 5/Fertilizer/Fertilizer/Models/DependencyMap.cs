namespace Fertilizer.Models;

public class DependencyMap
{
    public List<DependencyMap> Dependencies { get; set; } = new List<DependencyMap>();

    public string Type { get; set; } = string.Empty;

    public long TypeId { get; set; }
}