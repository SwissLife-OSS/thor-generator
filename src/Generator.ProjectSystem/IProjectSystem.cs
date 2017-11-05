using System;

namespace Thor.Generator.ProjectSystem
{
    /// <summary>
    /// A prject system provides functionality to load projects or commit changes back to the project files.
    /// </summary>
    public interface IProjectSystem
    {
        /// <summary>
        /// Determines whether this <see cref="IProjectSystem"/> 
        /// can handle the specified project file or directory 
        /// with code files.
        /// </summary>
        /// <param name="projectFileOrDirectoryName">
        /// Name of the project file or directory.
        /// </param>
        /// <returns>
        /// <c>true</c> if this <see cref="IProjectSystem"/>  can handle 
        /// the specified project file or directory with code files; 
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="projectFileOrDirectoryName"/> is <c>null</c>.
        /// </exception>
        bool CanHandle(string projectFileOrDirectoryName);

        /// <summary>
        /// Determines whether this <see cref="IProjectSystem"/> 
        /// can handle a project with the specified project identifier.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>
        /// <c>true</c> if this <see cref="IProjectSystem"/> can handle 
        /// a project with the specified project identifier; 
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="projectId"/> is <c>null</c>.
        /// </exception>
        bool CanHandle(IProjectId projectId);

        /// <summary>
        /// Creates a project object from the given project file or directory of code files.
        /// </summary>
        /// <param name="projectFileOrDirectoryName">
        /// Name of the project file or directory.
        /// </param>
        /// <returns>
        /// Returns a project object from the given project file or directory of code files.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="projectFileOrDirectoryName"/> is <c>null</c>.
        /// </exception>
        Project Open(string projectFileOrDirectoryName);

        /// <summary>
        /// Commits changes made to the project object to the project file or directory of code files.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="project"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="project"/> is <c>null</c>.
        /// </exception>
        void CommitChanges(Project project);
    }
}
