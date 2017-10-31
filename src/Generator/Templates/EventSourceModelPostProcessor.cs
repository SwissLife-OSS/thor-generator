using System;
using System.Collections.Generic;
using System.Linq;
using ChilliCream.Tracing.Generator.Types;

namespace ChilliCream.Tracing.Generator.Templates
{
    /// <summary>
    /// The event source post processor will transform the <see cref="EventSourceModel"/> to satisfy the template specific demands.
    /// Especially it will calculate which write methods are needed for the specified <see cref="Template"/>.
    /// </summary>
    internal class EventSourceModelPostProcessor
    {
        private readonly Template _template;
        private HashSet<WriteMethod> _baseWriteMethods;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSourceModelPostProcessor"/> class.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="template"/> is <c>null</c>.
        /// </exception>
        public EventSourceModelPostProcessor(Template template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            _template = template;
            _baseWriteMethods = new HashSet<WriteMethod>(template.BaseWriteMethods);
        }

        /// <summary>
        /// Processes the specified event source model.
        /// </summary>
        /// <param name="eventSourceModel">The event source model.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventSourceModel"/> is <c>null</c>.
        /// </exception>
        public void Process(EventSourceModel eventSourceModel)
        {
            if (eventSourceModel == null)
            {
                throw new ArgumentNullException(nameof(eventSourceModel));
            }

            eventSourceModel.WriteMethods.Clear();
            AddWriteMethods(eventSourceModel);
        }

        private void AddWriteMethods(EventSourceModel eventSourceModel)
        {
            foreach (WriteMethod writeMethod in GetWriteMethods(eventSourceModel))
            {
                if (!_baseWriteMethods.Contains(writeMethod))
                {
                    int c = 97;
                    int i = _template.DefaultPayloads;

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
