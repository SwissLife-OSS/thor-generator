using System;
using System.Collections.Generic;
using System.Linq;

namespace ChilliCream.Tracing.Generator.Templates
{
    internal class AttributeModel
    {
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
