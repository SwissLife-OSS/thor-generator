using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Construction;

namespace ChilliCream.Tracing.Generator.ProjectSystem.CSharp
{
    /// <summary>
    /// This project system provides functionality to load and alter clasic csharp projects.
    /// </summary>
    /// <seealso cref="ProjectSystem{CSharpClassicProjectId}" />
    public class CSharpClassicProjectSystem
        : ProjectSystem<CSharpClassicProjectId>
    {
        private static readonly HashSet<string> _projectGuids = new HashSet<string>
        {
            "{0BB0B76D-F56E-4EF3-B502-0AFDB8413EEF}"
        };

        public override bool CanHandle(string projectFileOrDirectoryName)
        {
            if (File.Exists(projectFileOrDirectoryName))
            {
                ProjectRootElement project = ProjectRootElement.Open(projectFileOrDirectoryName);
                ProjectPropertyElement property = project.Properties.FirstOrDefault(p => p.Name == "ProjectGuid");
                return (property != null
                    && property.Value != null
                    && _projectGuids.Contains(property.Value));
            }
            return false;
        }

        public override void CommitChanges(Project project)
        {
            throw new NotImplementedException();
        }

        protected override IProjectId CreateProjectId(string projectFileOrDirectoryName)
        {
            return new CSharpClassicProjectId(projectFileOrDirectoryName);
        }

        protected override string GetProjectDirectory(string projectFileOrDirectoryName)
        {
            return Path.GetDirectoryName(projectFileOrDirectoryName);
        }

        protected override IEnumerable<string> GetProjectFiles(string projectFileOrDirectoryName)
        {
            string projectDirectory = GetProjectDirectory(projectFileOrDirectoryName);
            ProjectRootElement project = ProjectRootElement.Open(projectFileOrDirectoryName);
            foreach (string include in project.Items
                .Where(t => t.ItemType == "Compile")
                .Select(t => t.Include).Distinct())
            {
                string fileName = Path.Combine(projectDirectory, include);
                if (File.Exists(fileName) && Path.GetExtension(fileName) == ".cs")
                {
                    yield return fileName;
                }
            }
        }
    }
}
