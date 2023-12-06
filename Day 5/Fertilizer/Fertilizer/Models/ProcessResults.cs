namespace Fertilizer.Models;

using System.Collections.Generic;
using Fertilizer.DTO;

public class ProcessResults
{
    public List<ItemMapDto> ItemMapDtos { get; set; } = new List<ItemMapDto>();

    public SeedListDto SeedListDto { get; set; } = new SeedListDto();
}