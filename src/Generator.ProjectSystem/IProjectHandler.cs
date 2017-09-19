using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChilliCream.Tracing.Generator.ProjectSystem
{
    public interface IProjectHandler
    {
        bool CanHandle(string projectFileOrDirectoryName);

        Task<Project> OpenAsync(string projectFileOrDirectoryName);

        Task CommitChangesAsync(Project project);
    }
}
