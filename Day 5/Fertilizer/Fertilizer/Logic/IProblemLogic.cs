namespace Fertilizer.Logic;

using Fertilizer.Models;

public interface IProblemLogic
{
    LogicResults Process(ProcessResults processResults);
}