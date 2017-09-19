using System;

namespace ChilliCream.Tracing.Generator.ProjectSystem.CSharp
{
    public sealed class CSharpDirectoryProjectId
        : IProjectId
    {
        private string _directoryName;

        public CSharpDirectoryProjectId(string directoryName)
        {
            if (directoryName == null)
            {
                throw new ArgumentNullException(nameof(directoryName));
            }
            _directoryName = directoryName;
        }

        public bool Equals(IProjectId other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return (other is CSharpDirectoryProjectId p
                && p._directoryName.Equals(_directoryName));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return Equals(obj as IProjectId);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return _directoryName;
        }
    }
}
