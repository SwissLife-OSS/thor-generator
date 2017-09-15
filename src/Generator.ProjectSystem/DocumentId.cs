using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace ChilliCream.Logging.Generator
{
    public class DocumentId
    {
        private const string _separator = "/";
        private string _internalId;

        public DocumentId(string name, IEnumerable<string> folders)
        {
            _internalId = folders.Any() ? string.Join(_separator, folders) + "\\" + name : name;
        }

        public bool Equals(DocumentId other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(other, this))
            {
                return true;
            }

            return other._internalId.Equals(_internalId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            return Equals(obj as Document);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _internalId.GetHashCode() * 397;
            }
        }

        public override string ToString()
        {
            return _internalId;
        }
    }
}
