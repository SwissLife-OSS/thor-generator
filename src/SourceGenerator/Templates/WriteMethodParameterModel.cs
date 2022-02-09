namespace Thor.Generator.Templates
{
    /// <summary>
    /// Represents an eventsource template model for the write method parameters.
    /// </summary>
    internal class WriteMethodParameterModel
    {
        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The parameter name.</value>
        public string Name { get; set; }

        // TODO : Make this an int!
        /// <summary>
        /// Gets or sets the size of the parameter type.
        /// Note that for string or large object the size will be zero.
        /// </summary>
        /// <value>The size.</value>
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets the type name.
        /// </summary>
        /// <value>The type name.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>The operator.</value>
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets the payload position.
        /// </summary>
        /// <value>The payload position.</value>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the first parameter in the parmeter list.
        /// </summary>
        /// <value><c>true</c> if this parmeter is the first parameter in the parameter list; otherwise, <c>false</c>.</value>
        public bool IsFirst { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this parameter is of type <see cref="string"/>.
        /// </summary>
        /// <value><c>true</c> if this parameter is of type <see cref="string"/>; otherwise, <c>false</c>.</value>
        public bool IsString { get; set; }
    }
}
