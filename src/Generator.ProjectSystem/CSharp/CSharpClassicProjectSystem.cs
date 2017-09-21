using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            "{FECA14C5-1B06-4400-8131-3ECE5BD78381}"
        };

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
            ProjectRootElement projectElement = ProjectRootElement.Open(projectFileOrDirectoryName);
            foreach (string include in projectElement.Items
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

        protected override string GetFileOrDirectoryName(IProjectId projectId)
        {
            return ((CSharpClassicProjectId)projectId).FileName;
        }

        public override bool CanHandle(string projectFileOrDirectoryName)
        {
            if (string.IsNullOrEmpty(projectFileOrDirectoryName))
            {
                throw new ArgumentNullException(nameof(projectFileOrDirectoryName));
            }

            if (File.Exists(projectFileOrDirectoryName))
            {
                ProjectRootElement project = ProjectRootElement.Open(projectFileOrDirectoryName);
                ProjectPropertyElement property = project.Properties.FirstOrDefault(p => p.Name == "ProjectGuid");
                return property != null && property.Value != null && _projectGuids.Contains(property.Value);
            }
            return false;
        }

        protected override void CommitChanges(Project project, string projectFileOrDirectoryName,
            string projectRootDirectory, HashSet<string> projectFiles)
        {
            ProjectRootElement projectElement = ProjectRootElement.Open(projectFileOrDirectoryName);

            foreach (DocumentId updatedDocumentId in project.UpdatedDocumets)
            {
                Document updatedDocument = project.GetDocument(updatedDocumentId);
                string documentFileName = updatedDocument.CreateFilePath(projectRootDirectory);

                // update project file
                if (!projectFiles.Contains(documentFileName))
                {
                    string relativePath = updatedDocument.Folders.Any()
                        ? string.Join("\\", updatedDocument.Folders) +
                            "\\" + updatedDocument.Name
                        : updatedDocument.Name;
                    projectElement.AddItem("Compile", relativePath);
                }
            }

            base.CommitChanges(project, projectFileOrDirectoryName,
                projectRootDirectory, projectFiles);

            projectElement.Save();
        }
    }
}
