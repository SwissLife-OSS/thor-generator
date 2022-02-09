using System;
using System.Collections.Generic;
using System.Linq;

namespace Thor.Generator.Templates
{
    /// <summary>
    /// Represents an event source write method. This class is used to 
    /// determine the various write method combinations that are 
    /// needed to generate an event source.
    /// </summary>
    /// <seealso cref="System.IEquatable{WriteMethod}" />
    public sealed class WriteMethod
        : IEquatable<WriteMethod>
    {
        private readonly string[] _types;
        private readonly string _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteMethod"/> class.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is <c>null</c>.
        /// </exception>
        public WriteMethod(IEnumerable<string> types)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            _types = types.ToArray();
            _key = string.Join("_", _types);
        }

        /// <summary>
        /// Gets the write method parameter types.
        /// </summary>
        /// <value>The parameter types.</value>
        public IReadOnlyCollection<string> ParameterTypes => _types;

        /// <summary>
        /// Indicates whether the current write method is equal to another write method.
        /// </summary>
        /// <param name="other">Another writemethod that shall be compared to this.</param>
        /// <returns>true if the current write method is equal to the <paramref name="other">write method</paramref>; otherwise, false.</returns>
        public bool Equals(WriteMethod other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(other, this))
            {
                return true;
            }

            return _key.Equals(other._key);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as WriteMethod);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _key.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this write method.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this write method.</returns>
        public override string ToString()
        {
            return string.Join(", ", _types);
        }
    }
}