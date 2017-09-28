using System;

namespace ChilliCream.Tracing.Generator.ProjectSystem.CSharp
{
    public sealed class CSharpDirectoryProjectId
        : ProjectId
    {
        private string _directoryName;

        public CSharpDirectoryProjectId(string directoryName)
            : base(directoryName)
        {
            _directoryName = directoryName;
        }

        public string DirectoryName => _directoryName;
    }
}
