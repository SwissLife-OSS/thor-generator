using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChilliCream.Tracing.Generator.ProjectSystem
{
    public abstract class ProjectSystem<TProjectId>
        : IProjectSystem
        where TProjectId : class, IProjectId
    {
        public bool CanHandle(IProjectId projectId)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            return projectId.GetType() == typeof(TProjectId);
        }

        public abstract bool CanHandle(string projectFileOrDirectoryName);

        public abstract void CommitChanges(Project project);

        public Project Open(string projectFileOrDirectoryName)
        {
            IProjectId projectId = CreateProjectId(projectFileOrDirectoryName);
            string projectDirectory = GetProjectDirectory(projectFileOrDirectoryName);
            int projectDirectoryLength = projectDirectory.Length;

            HashSet<Document> documents = new HashSet<Document>();

            foreach (string fileName in GetProjectFiles(projectFileOrDirectoryName))
            {
                string[] relativePath = fileName.Substring(projectDirectoryLength)
                    .Split(Path.DirectorySeparatorChar);
                Document document = Document.Create(() => File.ReadAllText(fileName),
                    relativePath.Last(), relativePath.Take(relativePath.Length - 1));
                documents.Add(document);
            }

            return Project.Create(projectId, documents);
        }

        protected abstract IProjectId CreateProjectId(string projectFileOrDirectoryName);
        protected abstract string GetProjectDirectory(string projectFileOrDirectoryName);
        protected abstract IEnumerable<string> GetProjectFiles(string projectFileOrDirectoryName);
    }
}
