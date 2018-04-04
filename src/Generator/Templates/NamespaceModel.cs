namespace Thor.Generator.Templates
{
    /// <summary>
    /// Represents a namespace declaration
    /// </summary>
    public class NamespaceModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceModel" /> class.
        /// </summary>
        /// <param name="isStatic">if set to <c>true</c> [is static].</param>
        /// <param name="alias">The alias.</param>
        /// <param name="@namespace">The ns.</param>
        public NamespaceModel(bool isStatic, string alias, string @namespace)
        {
            IsStatic = isStatic;
            Alias = alias;
            Namespace = @namespace;

            _key = $"{isStatic}-{alias??string.Empty}={@namespace}";
        }

        /// <summary>
        /// Gets a value indicating whether this instance is static.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is static; otherwise, <c>false</c>.
        /// </value>
        public bool IsStatic { get; }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias { get; }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        /// <value>
        /// The namespace.
        /// </value>
        public string Namespace { get; }

        private readonly string _key;

        /// <summary>
        /// Equalses the specified object.
        /// </summary>
        /// <param name="other">The other object.</param>
        /// <returns></returns>
        public bool Equals(NamespaceModel other)
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
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as NamespaceModel);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _key.GetHashCode();
        }
    }
}
