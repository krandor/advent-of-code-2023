namespace Scratchcards.InputProcessors;

using System.Threading.Tasks;
using Scratchcards.Models;

public interface IInputProcessor
{
    Task<ProcessResults> Process(string[] input);
}