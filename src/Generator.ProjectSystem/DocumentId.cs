using System.Collections.Generic;
using System.IO;
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
            _internalId = folders.Any() ? Path.Combine(Path.Combine(folders.ToArray()), name) : name;
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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return _internalId;
        }
    }
}
