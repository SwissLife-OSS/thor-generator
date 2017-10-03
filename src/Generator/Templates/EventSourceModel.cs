using System;
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
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }

        public string InterfaceName { get; set; }

        public AttributeModel2 Attribute { get; } = new AttributeModel2(Constants.EventSourceAttributeName);
        public List<EventModel2> Events { get; } = new List<EventModel2>();
        public List<WriteCoreModel2> WriteMethods { get; } = new List<WriteCoreModel2>();
    }

    internal static class ModelExtensions
    {
        public static void AddProperty(this AttributeModel2 attributeModel, string name, string value)
        {
            if (attributeModel == null)
            {
                throw new ArgumentNullException(nameof(attributeModel));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            attributeModel.Properties.Add(new AttributePropertyModel
            {
                Name = name,
                Value = value,
                IsFirst = attributeModel.Properties.Count == 0
            });
        }

        public static void AddProperty(this AttributeModel2 attributeModel, string value)
        {
            if (attributeModel == null)
            {
                throw new ArgumentNullException(nameof(attributeModel));
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            attributeModel.Properties.Add(new AttributePropertyModel
            {
                Value = value,
                IsFirst = attributeModel.Properties.Count == 0
            });
        }

        public static void AddParameter(this EventModel2 eventModel, string name, string type)
        {
            if (eventModel == null)
            {
                throw new ArgumentNullException(nameof(eventModel));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            eventModel.Parameters.Add(new EventParameterModel2
            {
                Name = name,
                Type = type,
                IsFirst = eventModel.Parameters.Count == 0
            });
        }
    }

    internal class AttributeModel2
    {
        public AttributeModel2(string name)
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

        /// <summary>
        /// Gets a value indicating whether this property has name.
        /// </summary>
        /// <value><c>true</c> if this property has name; otherwise, <c>false</c>.</value>
        public bool HasName => !string.IsNullOrEmpty(Name);
    }

    internal class EventModel2
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public AttributeModel2 Attribute { get; } = new AttributeModel2("Event");

        public List<EventParameterModel2> Parameters { get; } = new List<EventParameterModel2>();
    }

    internal class EventParameterModel2
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public bool IsFirst { get; set; }
    }

    internal class WriteCoreModel2
    {
        public int TotalParameters { get; set; }
        public List<WriteMethodParameterModel> Parameters { get; set; } = new List<WriteMethodParameterModel>();
    }

    internal class WriteMethodParameterModel
    {
        public string Name { get; set; }
        public string Size { get; set; }

        public string Operator { get; set; }
        public int Position { get; set; }

        public bool IsFirst { get; set; }
        public bool IsString { get; set; }
    }
}
