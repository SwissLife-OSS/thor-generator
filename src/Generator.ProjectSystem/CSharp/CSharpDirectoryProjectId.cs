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
            if (directoryName == null)
            {
                throw new ArgumentNullException(nameof(directoryName));
            }
            _directoryName = directoryName;
        }
    }
}
