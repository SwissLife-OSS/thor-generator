using System;
using System.Collections.Generic;
using System.Linq;
using ChilliCream.Logging.Generator.Templates;
using ChilliCream.Tracing.Generator.Types;
using Nustache.Core;

namespace ChilliCream.Tracing.Generator.Templates
{
    /// <summary>
    /// The template engine can generate event sources from an <see cref="EventSourceModel"/>.
    /// </summary>
    internal class EventSourceTemplateEngine
    {
        private HashSet<WriteMethod> _baseWriteMethods;
        private readonly string _template;
        private readonly int _defaultPayloads;

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
            _defaultPayloads = template.DefaultPayloads;
        }

        /// <summary>
        /// Generates the specified event source.
        /// </summary>
        /// <param name="eventSourceModel">The event source model.</param>
        /// <returns>
        /// Returns a <see cref="string"/> representing the generated event source.
        /// </returns>
        public string Generate(EventSourceModel eventSourceModel)
        {
            if (eventSourceModel == null)
            {
                throw new ArgumentNullException(nameof(eventSourceModel));
            }

            AddWriteMethods(eventSourceModel);

            // generate event source
            return Render.StringToString(_template, eventSourceModel,
                renderContextBehaviour: new RenderContextBehaviour { HtmlEncoder = t => t });
        }

        private void AddWriteMethods(EventSourceModel eventSourceModel)
        {
            eventSourceModel.WriteMethods.Clear();

            foreach (WriteMethod writeMethod in GetWriteMethods(eventSourceModel))
            {
                if (!_baseWriteMethods.Contains(writeMethod))
                {
                    int c = 97;
                    int i = _defaultPayloads;

                    WriteCoreModel writeCoreModel = new WriteCoreModel();
                    foreach (string type in writeMethod.ParameterTypes)
                    {
                        IParameterTypeInfo typeInfo = GetWriteMethodParameterTypeInfo(type);

                        WriteMethodParameterModel parameterModel = new WriteMethodParameterModel
                        {
                            Name = ((char)c++).ToString(),
                            Type = typeInfo.Name,
                            Position = i++,
                            IsString = typeInfo.IsString,
                            Operator = typeInfo.Operator,
                            Size = typeInfo.Size,
                            IsFirst = writeCoreModel.Parameters.Count == 0
                        };
                        writeCoreModel.Parameters.Add(parameterModel);
                    }
                    writeCoreModel.TotalParameters = i;
                    eventSourceModel.WriteMethods.Add(writeCoreModel);
                }
            }
        }

        private IEnumerable<WriteMethod> GetWriteMethods(EventSourceModel eventSourceModel)
        {
            HashSet<WriteMethod> hashSet = new HashSet<WriteMethod>();
            foreach (EventModel eventModel in eventSourceModel.Events)
            {
                IEnumerable<string> types = eventModel.Parameters
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

        private IParameterTypeInfo GetWriteMethodParameterTypeInfo(string typeName)
        {
            IParameterTypeInfo typeInfo;
            if (!ParameterTypeLookup.TryGet(typeName, out typeInfo))
            {
                throw new ArgumentException("The specified type is not allowed.", nameof(typeName));
            }
            return typeInfo;
        }
    }
}
