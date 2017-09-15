using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChilliCream.Logging.Generator
{
    public interface IProjectHandler
    {
        bool CanHandle(string projectFileOrDirectoryName);

        Task<Project> OpenAsync(string projectFileOrDirectoryName);

        Task CommitChangesAsync(string projectFileOrDirectoryName);
    }
}
