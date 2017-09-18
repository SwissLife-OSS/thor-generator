using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace ChilliCream.Logging.Generator
{
    /// <summary>
    /// Represents a <see cref="Project"/> unique identifier of a <see cref="Document"/>.
    /// </summary>
    public sealed class DocumentId
    {
        private const string _separator = "/";
        private string _internalId;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentId"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="folders">The folders.</param>
        public DocumentId(string name, IEnumerable<string> folders)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            _internalId = (folders?.Any() ?? false)
                ? Path.Combine(Path.Combine(folders.ToArray()), name)
                : name;
        }

        /// <summary>
        /// Determines whether the specified <see cref="DocumentId" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The other <see cref="DocumentId" /> to compare with the current <see cref="DocumentId" />.</param>
        /// <returns><c>true</c> if the specified <see cref="DocumentId" /> is equal to this instance; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            return Equals(obj as DocumentId);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return _internalId.GetHashCode() * 397;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the project relative path of the document.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents the project relative path of the document.</returns>
        public override string ToString()
        {
            return _internalId;
        }

        #region Operators

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(DocumentId x, DocumentId y)
        {
            if (ReferenceEquals(x, null))
            {
                return ReferenceEquals(y, null);
            }
            else
            {
                if (ReferenceEquals(y, null))
                {
                    return false;
                }
                return x.Equals(y);
            }
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(DocumentId x, DocumentId y)
        {
            return !(x == y);
        }

        #endregion
    }
}
