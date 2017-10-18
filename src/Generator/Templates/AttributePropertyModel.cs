namespace ChilliCream.Tracing.Generator.Templates
{
    /// <summary>
    /// Represents an attribute property or constructor argument template model.
    /// </summary>
    public class AttributePropertyModel
    {
        /// <summary>
        /// Gets or sets the name of the attribute model.
        /// </summary>
        /// <value>The propetrty name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value syntax of the attribute property.
        /// The syntax will include all necassary quotes etc. so 
        /// that the template logic does not have to deal with 
        /// those cases.
        /// </summary>
        /// <value>The property value syntax.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property is the first property in the property collection.
        /// </summary>
        /// <value><c>true</c> if this property is the first property  in the property collection; otherwise, <c>false</c>.</value>
        public bool IsFirst { get; set; }

        /// <summary>
        /// Gets a value indicating whether this property has name.
        /// </summary>
        /// <value><c>true</c> if this property has name; otherwise, <c>false</c>.</value>
        public bool HasName => !string.IsNullOrEmpty(Name);
    }
}
