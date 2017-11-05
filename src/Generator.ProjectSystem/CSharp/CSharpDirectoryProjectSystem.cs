using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Thor.Generator.ProjectSystem.CSharp
{
    public class CSharpDirectoryProjectSystem
        : ProjectSystem<CSharpDirectoryProjectId>
    {
        protected override IProjectId CreateProjectId(string projectFileOrDirectoryName)
        {
            return new CSharpDirectoryProjectId(projectFileOrDirectoryName);
        }

        protected override string GetProjectDirectory(string projectFileOrDirectoryName)
        {
            return projectFileOrDirectoryName;
        }

        protected override IEnumerable<string> GetProjectFiles(string projectFileOrDirectoryName)
        {
            return Directory.GetFiles(projectFileOrDirectoryName,
                CSharpConstants.FileFilter, SearchOption.AllDirectories);
        }

        protected override string GetFileOrDirectoryName(IProjectId projectId)
        {
            return ((CSharpDirectoryProjectId)projectId).DirectoryName;
        }

        public override bool CanHandle(string projectFileOrDirectoryName)
        {
            if (string.IsNullOrEmpty(projectFileOrDirectoryName))
            {
                throw new ArgumentNullException(nameof(projectFileOrDirectoryName));
            }

            return Directory.Exists(projectFileOrDirectoryName);
        }
    }
}
