using System;

namespace Thor.Generator.ProjectSystem.CSharp
{
    /// <summary>
    /// Represents a project identifier for <see cref="CSharpCoreProjectSystem"/> projects.
    /// </summary>
    /// <seealso cref="Thor.Generator.ProjectSystem.ProjectId" />
    public sealed class CSharpCoreProjectId
        : ProjectId
    {
        private string _fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpCoreProjectId"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/> is <c>null</c>.
        /// </exception>
        public CSharpCoreProjectId(string fileName)
            : base(fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Gets the project file name.
        /// </summary>
        /// <value>The project file name.</value>
        public string FileName => _fileName;
    }
}
