using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChilliCream.Logging.Generator.Templates;
using ChilliCream.Tracing.Generator.Analyzer;
using ChilliCream.Tracing.Generator.Types;
using Nustache.Core;

namespace ChilliCream.Tracing.Generator.Templates
{
    /// <summary>
    /// The template engine can generate event sources from an <see cref="EventSourceDefinition"/>.
    /// </summary>
    public class EventSourceTemplateEngine
    {
        private HashSet<WriteMethod> _baseWriteMethods;
        private readonly string _template;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSourceTemplateEngine"/> class.
        /// </summary>
        /// <param name="template">
        /// The event source template.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="template"/> is <c>null</c>.
        /// </exception>
        public EventSourceTemplateEngine(Template template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            _baseWriteMethods = new HashSet<WriteMethod>(template.BaseWriteMethods);
            _template = template.Code;
        }

        /// <summary>
        /// Generates the specified event source.
        /// </summary>
        /// <param name="eventSourceDefinition">The event source definition.</param>
        /// <returns>
        /// Returns a <see cref="string"/> representing the generated event source.
        /// </returns>
        public string Generate(EventSourceDefinition eventSourceDefinition)
        {
            if (eventSourceDefinition == null)
            {
                throw new ArgumentNullException(nameof(eventSourceDefinition));
            }

            // create template model
            EventSourceModel eventSourceModel = CreateGeneratorModel(eventSourceDefinition);

            // generate event source
            return Render.StringToString(_template, eventSourceModel,
                renderContextBehaviour: new RenderContextBehaviour { HtmlEncoder = t => t });
        }

        private EventSourceModel CreateGeneratorModel(EventSourceDefinition eventSourceDefinition)
        {
            EventSourceModel eventSourceModel = new EventSourceModel
            {
                Name = eventSourceDefinition.ClassName,
                Namespace = eventSourceDefinition.Namespace
            };

            string attributeArgumentSyntax = CreateAttributeArgumentSyntax(eventSourceDefinition);
            if (attributeArgumentSyntax != null)
            {
                eventSourceModel.Attribute = new AttributeModel
                {
                    ArgumentSyntax = attributeArgumentSyntax
                };
            }

            AddEvents(eventSourceDefinition, eventSourceModel);
            AddWriteMethods(eventSourceDefinition, eventSourceModel);

            return eventSourceModel;
        }

        private string CreateAttributeArgumentSyntax(EventSourceDefinition eventSourceDefinition)
        {
            StringBuilder argumentSyntax = new StringBuilder();
            if (eventSourceDefinition.Name != null)
            {
                argumentSyntax.Append($"Name =\"{eventSourceDefinition.Name}\"");
            }

            if (eventSourceDefinition.Guid != null)
            {
                if (argumentSyntax.Length > 0)
                {
                    argumentSyntax.Append(", ");
                }
                argumentSyntax.Append($"Guid =\"{eventSourceDefinition.Guid}\"");
            }

            if (eventSourceDefinition.LocalizationResources != null)
            {
                if (argumentSyntax.Length > 0)
                {
                    argumentSyntax.Append(", ");
                }
                argumentSyntax.Append($"LocalizationResources =\"{eventSourceDefinition.LocalizationResources}\"");
            }
            return argumentSyntax.ToString();
        }

        private void AddEvents(EventSourceDefinition eventSourceDefinition, EventSourceModel eventSourceModel)
        {
            foreach (EventDefinition eventDefinition in eventSourceDefinition.Events)
            {
                EventModel eventModel = new EventModel();
                eventModel.Id = eventDefinition.EventId;
                eventModel.Name = eventDefinition.Name;
                eventModel.AttributeSyntax = eventDefinition.AttributeSyntax;

                int i = 0;
                bool isFollowing = false;

                foreach (EventArgumentDefinition eventArgument in eventDefinition.Arguments)
                {
                    EventParameterModel parameterModel = new EventParameterModel
                    {
                        Position = i++,
                        Name = eventArgument.Name,
                        Type = GetWriteMethodParameterType(eventArgument.Type),
                        IsFollowing = isFollowing
                    };
                    AddTypeDetails(parameterModel);
                    eventModel.Parameters.Add(parameterModel);
                    isFollowing = true;
                }
                eventSourceModel.Events.Add(eventModel);
            }
        }

        private void AddWriteMethods(EventSourceDefinition eventSourceDefinition, EventSourceModel eventSourceModel)
        {
            foreach (WriteMethod writeMethod in GetWriteMethods(eventSourceDefinition))
            {
                if (!_baseWriteMethods.Contains(writeMethod))
                {
                    int c = 97;
                    int i = 2;
                    bool isFollowing = false;

                    WriteCoreModel writeCoreModel = new WriteCoreModel();
                    foreach (string type in writeMethod.ParameterTypes)
                    {
                        EventParameterModel parameterModel = new EventParameterModel
                        {
                            Name = ((char)c++).ToString(),
                            Position = i++,
                            Type = GetWriteMethodParameterType(type),
                            IsFollowing = isFollowing
                        };
                        AddTypeDetails(parameterModel);
                        writeCoreModel.Parameters.Add(parameterModel);
                        isFollowing = true;
                    }
                    writeCoreModel.ParametersCount = i;
                    eventSourceModel.WriteMethods.Add(writeCoreModel);
                }
            }
        }

        private IEnumerable<WriteMethod> GetWriteMethods(EventSourceDefinition eventSourceDefinition)
        {
            HashSet<WriteMethod> hashSet = new HashSet<WriteMethod>();
            foreach (EventDefinition eventDefinition in eventSourceDefinition.Events)
            {
                IEnumerable<string> types = eventDefinition.Arguments
                    .Select(t => GetWriteMethodParameterType(t.Type));
                hashSet.Add(new WriteMethod(types));
            }
            return hashSet;
        }

        private string GetWriteMethodParameterType(string typeName)
        {
            IParameterTypeInfo typeInfo;
            if (!ParameterTypeLookup.TryGet(typeName, out typeInfo))
            {
                throw new ArgumentException("The specified type is not allowed.", nameof(typeName));
            }
            return typeInfo.Name;
        }

        private void AddTypeDetails(EventParameterModel eventParameterModel)
        {
            IParameterTypeInfo typeInfo;
            if (!ParameterTypeLookup.TryGet(eventParameterModel.Type, out typeInfo))
            {
                throw new ArgumentException("The specified type is not allowed.", nameof(eventParameterModel));
            }

            eventParameterModel.IsString = typeInfo.IsString;
            eventParameterModel.Size = typeInfo.Size;
            eventParameterModel.Operator = typeInfo.Operator;
        }
    }
}
