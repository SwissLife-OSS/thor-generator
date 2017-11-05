using System;

namespace Thor.Generator.Templates
{
    internal static class ModelExtensions
    {
        public static void AddProperty(this AttributeModel attributeModel, string name, string value)
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

        public static void AddProperty(this AttributeModel attributeModel, string value)
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

        public static void AddParameter(this EventModel eventModel, string name, string type)
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

            eventModel.Parameters.Add(new EventParameterModel
            {
                Name = name,
                Type = type,
                IsFirst = eventModel.Parameters.Count == 0
            });
        }
    }
}
