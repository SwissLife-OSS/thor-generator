using System.Collections.Generic;

namespace ChilliCream.Logging.Generator
{
    public interface ISolution
    {
        IReadOnlyCollection<Project> Projects { get; }
    }
}
