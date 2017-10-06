using System;
using System.Collections.Generic;
using System.Linq;

namespace ChilliCream.Tracing.Generator.Templates
{
    /// <summary>
    /// Represents an attribute template model
    /// </summary>
    public class AttributeModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeModel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is <c>null</c>
        /// or
        /// <paramref name="name"/> is <see cref="string.Empty"/>
        /// </exception>
        public AttributeModel(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

        public string Name { get; }
        public bool HasProperties => Properties != null && Properties.Any();
        public List<AttributePropertyModel> Properties { get; } = new List<AttributePropertyModel>();
    }
}
