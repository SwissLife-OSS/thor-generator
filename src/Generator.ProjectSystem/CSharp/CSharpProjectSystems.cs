using System;
using System.Collections.Generic;
using System.Linq;

namespace ChilliCream.Tracing.Generator.ProjectSystem.CSharp
{
    internal static class CSharpProjectSystems
    {
        public static readonly IEnumerable<IProjectSystem> All = new IProjectSystem[]
        {
            new CSharpClassicProjectSystem(),
            new CSharpCoreProjectSystem(),
            new CSharpDirectoryProjectSystem()
        };

        public static bool TryOpenProject(string fileOrDirectoryName,
            out Project project)
        {
            if (fileOrDirectoryName == null)
            {
                throw new ArgumentNullException(nameof(fileOrDirectoryName));
            }

            IProjectSystem projectSystem = All.FirstOrDefault(t => t.CanHandle(fileOrDirectoryName));
            project = projectSystem?.Open(fileOrDirectoryName);
            return project != null;
        }

        public static void TryCommitChanges(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            IProjectSystem projectSystem = All.FirstOrDefault(t => t.CanHandle(project.Id));
            projectSystem?.CommitChanges(project);
        }
    }
}
