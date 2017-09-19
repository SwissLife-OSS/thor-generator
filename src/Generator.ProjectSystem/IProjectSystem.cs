using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChilliCream.Tracing.Generator.ProjectSystem
{
    public interface IProjectSystem
    {
        bool CanHandle(string projectFileOrDirectoryName);

        bool CanHandle(IProjectId projectId);

        Task<Project> OpenAsync(string projectFileOrDirectoryName);

        Task CommitChangesAsync(Project project);
    }
}
