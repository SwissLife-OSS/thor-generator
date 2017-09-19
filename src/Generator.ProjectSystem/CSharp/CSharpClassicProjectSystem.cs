using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Construction;

namespace ChilliCream.Tracing.Generator.ProjectSystem.CSharp
{
    public class CSharpClassicProjectSystem
        : IProjectSystem
    {
        private static readonly HashSet<string> _projectGuids = new HashSet<string>
        {
            "{0BB0B76D-F56E-4EF3-B502-0AFDB8413EEF}"
        };

        public bool CanHandle(string projectFileOrDirectoryName)
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

        public bool CanHandle(IProjectId projectId)
        {
            return projectId is CSharpClassicProjectId;
        }

        public Task CommitChangesAsync(Project project)
        {




            throw new System.NotImplementedException();
        }

        public Task<Project> OpenAsync(string projectFileOrDirectoryName)
        {
            CSharpClassicProjectId projectId = new CSharpClassicProjectId(projectFileOrDirectoryName);
            HashSet<Document> documents = new HashSet<Document>();

            string projectDirectory = Path.GetDirectoryName(projectFileOrDirectoryName);
            int rootPathLength = projectDirectory.Length;

            ProjectRootElement project = ProjectRootElement.Open(projectFileOrDirectoryName);
            foreach (string include in project.Items
                .Where(t => t.ItemType == "Compile")
                .Select(t => t.Include).Distinct())
            {
                string fileName = Path.Combine(projectDirectory, include);
                if (File.Exists(fileName) && Path.GetExtension(fileName) == ".cs")
                {
                    documents.Add(ProjectSystemUtils.CreateDocument(fileName, rootPathLength));
                }
            }

            return Task.FromResult(Project.Create(projectId, documents));
        }
    }
}
