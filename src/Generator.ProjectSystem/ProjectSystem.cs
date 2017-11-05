using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Thor.Generator.ProjectSystem
{
    /// <summary>
    /// Provides a base implementation for a prject system.
    /// </summary>
    /// <typeparam name="TProjectId">The type of the t project identifier.</typeparam>
    /// <seealso cref="Thor.Generator.ProjectSystem.IProjectSystem" />
    public abstract class ProjectSystem<TProjectId>
        : IProjectSystem
        where TProjectId : class, IProjectId
    {
        /// <summary>
        /// Determines whether this <see cref="IProjectSystem" />
        /// can handle the specified project file or directory
        /// with code files.
        /// </summary>
        /// <param name="projectFileOrDirectoryName">Name of the project file or directory.</param>
        /// <returns><c>true</c> if this <see cref="IProjectSystem" /> can handle
        /// the specified project file or directory with code files;
        /// otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="projectFileOrDirectoryName"/> is <c>null</c>.
        /// </exception>
        public abstract bool CanHandle(string projectFileOrDirectoryName);

        /// <summary>
        /// Determines whether this <see cref="IProjectSystem" />
        /// can handle a project with the specified project identifier.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns><c>true</c> if this <see cref="IProjectSystem" /> can handle
        /// a project with the specified project identifier;
        /// otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="projectId"/> is <c>null</c>.
        /// </exception>
        public bool CanHandle(IProjectId projectId)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            return projectId.GetType() == typeof(TProjectId);
        }

        /// <summary>
        /// Creates a project object from the given project file or directory of code files.
        /// </summary>
        /// <param name="projectFileOrDirectoryName">Name of the project file or directory.</param>
        /// <returns>Returns a project object from the given project file or directory of code files.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="projectFileOrDirectoryName"/> is <c>null</c>.
        /// </exception>
        public Project Open(string projectFileOrDirectoryName)
        {
            if (projectFileOrDirectoryName == null)
            {
                throw new ArgumentNullException(projectFileOrDirectoryName);
            }

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

        /// <summary>
        /// Commits changes made to the project object to the project file or directory of code files.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="project"/> is <c>null</c>.
        /// </exception>
        public void CommitChanges(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            string projectFileOrDirectoryName = GetFileOrDirectoryName(project.Id);
            string projectRootDirectory = GetProjectDirectory(projectFileOrDirectoryName);
            HashSet<string> projectFiles = new HashSet<string>(
                GetProjectFiles(projectFileOrDirectoryName));

            CommitChanges(project, projectFileOrDirectoryName,
                projectRootDirectory, projectFiles);
        }

        protected virtual void CommitChanges(Project project, string projectFileOrDirectoryName,
            string projectRootDirectory, HashSet<string> projectFiles)
        {
            foreach (DocumentId updatedDocumentId in project.UpdatedDocumets)
            {
                Document updatedDocument = project.GetDocument(updatedDocumentId);
                string documentFileName = updatedDocument.CreateFilePath(projectRootDirectory);

                string directory = Path.GetDirectoryName(documentFileName);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.WriteAllText(documentFileName, updatedDocument.GetContent());
            }
        }

        /// <summary>
        /// Creates a project identifier for the specified <paramref name="projectFileOrDirectoryName"/>.
        /// </summary>
        /// <param name="projectFileOrDirectoryName">Name of the project file or directory.</param>
        /// <returns>
        /// Returns a project identifier for the specified <paramref name="projectFileOrDirectoryName"/>.
        /// </returns>
        protected abstract IProjectId CreateProjectId(string projectFileOrDirectoryName);

        /// <summary>
        /// Gets the project directory.
        /// </summary>
        /// <param name="projectFileOrDirectoryName">Name of the project file or directory.</param>
        /// <returns>System.String.</returns>
        protected abstract string GetProjectDirectory(string projectFileOrDirectoryName);

        /// <summary>
        /// Gets the project files.
        /// </summary>
        /// <param name="projectFileOrDirectoryName">Name of the project file or directory.</param>
        /// <returns>
        /// Returns the files of the project.
        /// </returns>
        protected abstract IEnumerable<string> GetProjectFiles(string projectFileOrDirectoryName);

        protected abstract string GetFileOrDirectoryName(IProjectId projectId);
    }
}
