using System;
using System.Text;
using System.Threading.Tasks;

namespace ChilliCream.Logging.Generator
{
    public interface IProjectSystem
    {
        string Type { get; }

        string Language { get; }

        Task<ISolution> OpenSolutionAsync(string fileName);

        Task<IProject> OpenProjectAsync(string fileName);
    }
}
