namespace Fertilizer.InputProcessors;

using System.Threading.Tasks;
using Fertilizer.Models;

public interface IInputProcessor
{
    Task<ProcessResults> Process(string[] input);
}