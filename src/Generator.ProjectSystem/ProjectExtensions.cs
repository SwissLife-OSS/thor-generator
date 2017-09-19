using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace ChilliCream.Tracing.Generator.ProjectSystem
{
    /// <summary>
    /// Project extensions.
    /// </summary>
    public static class ProjectExtensions
    {
        /// <summary>
        /// Adds or replaces a document of this project.
        /// If there is already a project in the specified
        /// project folder with the specified file name the
        /// document will be replaced; otherwise a new document
        /// will be added.
        /// </summary>
        /// <param name="project">The project that shall be extended.</param>
        /// <param name="content">The content.</param>
        /// <param name="name">Name of the file.</param>
        /// <param name="folders">The folders.</param>
        /// <returns>Returns a new document object.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="project" /> is <c>null</c>
        /// or
        /// <paramref name="content" /> is <c>null</c>
        /// or
        /// <paramref name="content" /> is <see cref="string.Empty" />
        /// or
        /// <paramref name="content" /> consists of a whitespace
        /// or
        /// <paramref name="name" /> is <c>null</c>
        /// or
        /// <paramref name="name" /> is <see cref="string.Empty" />
        /// or
        /// <paramref name="name" /> consists of a whitespace
        /// or
        /// <paramref name="folders" /> is <c>null</c>.
        /// </exception>
        public static Document UpdateDocument(this Project project, string content, string name, params string[] folders)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (folders == null)
            {
                throw new ArgumentNullException(nameof(folders));
            }

            return project.UpdateDocument(content, name, folders);
        }
    }
}
