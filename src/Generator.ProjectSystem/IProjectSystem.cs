using System.Text;
using System.Threading.Tasks;

namespace ChilliCream.Tracing.Generator.ProjectSystem
{
    public interface IProjectSystem
    {
        bool CanHandle(string projectFileOrDirectoryName);

        bool CanHandle(IProjectId projectId);

        Project Open(string projectFileOrDirectoryName);

        void CommitChanges(Project project);
    }
}
