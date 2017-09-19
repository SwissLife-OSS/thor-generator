using System;

namespace ChilliCream.Tracing.Generator.ProjectSystem.CSharp
{
    public sealed class CSharpClassicProjectId
        : IProjectId
    {
        private string _fileName;

        public CSharpClassicProjectId(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            _fileName = fileName;
        }

        public string FileName => _fileName;

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

            return (other is CSharpClassicProjectId p
                && p._fileName.Equals(_fileName));
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
            return _fileName;
        }
    }
}
