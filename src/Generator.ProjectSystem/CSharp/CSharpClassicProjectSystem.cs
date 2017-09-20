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
                return (property != null
                    && property.Value != null
                    && _projectGuids.Contains(property.Value));
            }
            return false;
        }



        public override void CommitChanges(Project project)
        {
            CSharpClassicProjectId projectId = (CSharpClassicProjectId)project.Id;
            string projectRootDirectory = Path.GetDirectoryName(projectId.FileName);
            ProjectRootElement projectElement = ProjectRootElement.Open(projectId.FileName);
            HashSet<string> projectFiles = new HashSet<string>(
                GetProjectFiles(projectId.FileName));

            foreach (DocumentId updatedDocumentId in project.UpdatedDocumets)
            {
                Document updatedDocument = project.GetDocument(updatedDocumentId);
                string documentFileName = updatedDocument.CreateFilePath(projectRootDirectory);

                // update project file
                if (!projectFiles.Contains(documentFileName))
                {
                    string relativePath = updatedDocument.Folders.Any()
                        ? string.Join("\\", updatedDocument.Folders) + "\\" + updatedDocument.Name
                        : updatedDocument.Name;
                    projectElement.AddItem("Compile", relativePath);
                }

                // update code file
                File.WriteAllText(documentFileName, updatedDocument.GetContent());
            }


            throw new ArgumentNullException();
        }
    }
}
