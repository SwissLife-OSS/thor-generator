using System.Collections.Generic;
using System.Linq;

namespace ChilliCream.Tracing.Generator.Templates
{
    internal class EventSourceModel
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public AttributeModel Attribute { get; set; }
        public List<EventModel> Events { get; set; } = new List<EventModel>();
        public List<WriteCoreModel> WriteMethods { get; set; } = new List<WriteCoreModel>();
    }

    internal class EventSourceModel2
    {
        public string Name { get; set; }
        public string Namespace { get; set; }

        public string InterfaceName { get; set; }

        public AttributeModel2 Attribute { get; set; }
        public List<EventModel2> Events { get; set; } = new List<EventModel2>();
        public List<WriteCoreModel> WriteMethods { get; set; } = new List<WriteCoreModel>();
    }

    internal class AttributeModel2
    {
        public string Name { get; set; }
        public bool HasProperties => Properties != null && Properties.Any();
        public List<AttributePropertyModel> Properties { get; set; } = new List<AttributePropertyModel>();
    }

    /// <summary>
    /// Represents an attribute property template model.
    /// </summary>
    internal class AttributePropertyModel
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
        
        public bool HasName { get; set; }
    }

    internal class EventModel2
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public AttributeModel2 Attribute { get; set; }

        public List<EventParameterModel2> Parameters { get; set; } = new List<EventParameterModel2>();
    }

    internal class EventParameterModel2
    {
        public string Name { get; set; }
        public string Type { get; set; }
        
        public bool IsFirst { get; set; }
    }

    internal class WriteCoreModel2
    {
        public int ParametersCount { get; set; }
        public List<EventParameterModel> Parameters { get; set; } = new List<EventParameterModel>();
    }

    internal class WriteMethodParameterModel
    {
        public string Name { get; set; }
        public string Size { get; set; }
       
        public string Operator { get; set; }
        public int? Position { get; set; }
        public string Type { get; set; }

        public bool IsFirst { get; set; }
        public bool IsString { get; set; }
    }
}
