namespace Gear_Ratios.InputProcessors;

using System.Threading.Tasks;
using Gear_Ratios.Models;

public interface IInputProcessor
{
    Task<ProcessResult> Process(string[] input);
}