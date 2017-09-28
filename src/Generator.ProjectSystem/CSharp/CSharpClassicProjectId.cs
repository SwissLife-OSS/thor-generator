using System;

namespace ChilliCream.Tracing.Generator.ProjectSystem.CSharp
{
    /// <summary>
    /// Represents a project identifier for <see cref="CSharpClassicProjectSystem"/> projects.
    /// </summary>
    /// <seealso cref="ChilliCream.Tracing.Generator.ProjectSystem.ProjectId" />
    public sealed class CSharpClassicProjectId
        : ProjectId
    {
        private string _fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpClassicProjectId"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/> is <c>null</c>.
        /// </exception>
        public CSharpClassicProjectId(string fileName)
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
