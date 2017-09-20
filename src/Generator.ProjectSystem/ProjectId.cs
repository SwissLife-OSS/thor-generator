using System;

namespace ChilliCream.Tracing.Generator.ProjectSystem
{
    /// <summary>
    /// A project identifier base class that 
    /// overrides <see cref="object.GetHashCode()"/>
    /// and <see cref="object.Equals(object)"/>.
    /// </summary>
    /// <seealso cref="ChilliCream.Tracing.Generator.ProjectSystem.IProjectId" />
    public class ProjectId
        : IProjectId
    {
        private string _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectId"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <exception cref="ArgumentNullException">key</exception>
        protected ProjectId(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            _key = key;
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Boolean.</returns>
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

            return (other is ProjectId p
                && p._key.Equals(_key));
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return Equals(obj as IProjectId);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return _key.GetHashCode() * 397;
            }
        }

        /// <summary>
        /// Returns a string that represents the project identifier key.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return _key;
        }
    }

}
