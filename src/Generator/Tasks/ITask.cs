using System.IO;
using System.Linq;
using ChilliCream.Tracing.Generator.ProjectSystem;

namespace ChilliCream.Tracing.Generator.Tasks
{
    public interface ITask
    {
        void Execute();
    }
}
