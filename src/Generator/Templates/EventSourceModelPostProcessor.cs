using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Thor.Generator.Types;

namespace Thor.Generator.Templates
{
    /// <summary>
    /// The event source post processor will transform the <see cref="EventSourceModel"/> to satisfy the template specific demands.
    /// Especially it will calculate which write methods are needed for the specified <see cref="Template"/>.
    /// </summary>
    internal class EventSourceModelPostProcessor
    {
        private readonly Template _template;
        private HashSet<WriteMethod> _baseWriteMethods;
        private HashSet<NamespaceModel> _usings;
        private bool _allowComplexParameters;
        private string _eventComplexParameterName;

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
            _usings = new HashSet<NamespaceModel>(template.Usings);
            _allowComplexParameters = template.AllowComplexParameters;
            _eventComplexParameterName = template.EventComplexParameterName;
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

            // 1. Separate complextype parameters from simpletype parameters
            QualifyParameters(eventSourceModel);

            // 2. Generate the write methods with respect to the already existing write methods
            eventSourceModel.WriteMethods.Clear();
            AddWriteMethods(eventSourceModel);

            // 3. Generate the list of usings from the two input sources: Template usings and Interface (input file) usings
            MergeUsings(eventSourceModel);

            // 4. Replace named placeholders
            ReplaceMessagePlaceholdersWithIndexes(eventSourceModel);
        }

        /// <summary>
        /// Merges the usings from the Interface with the usings from the template.
        /// </summary>
        /// <param name="eventSourceModel">The event source model.</param>
        private void MergeUsings(EventSourceModel eventSourceModel)
        {
            eventSourceModel.Usings =
                eventSourceModel.Usings.Union(_usings).Distinct()
                    .OrderBy(u => u.Namespace).ToHashSet();
        }

        private void ReplaceMessagePlaceholdersWithIndexes(
            EventSourceModel eventSourceModel)
        {
            foreach (EventModel eventModel in eventSourceModel.Events)
            {
                ReplaceMessagePlaceholdersWithIndexes(eventModel);
            }
        }

        private void ReplaceMessagePlaceholdersWithIndexes(
            EventModel eventModel)
        {
            AttributePropertyModel messageProperty =
                eventModel.Attribute.Properties
                    .FirstOrDefault(t => t.Name != null
                        && t.Name.Equals("Message",
                            StringComparison.Ordinal));

            if (messageProperty != null
                && !string.IsNullOrEmpty(messageProperty.Value))
            {
                Placeholder[] placeholders =
                    MessageParser.FindPlaceholders(messageProperty.Value)
                        .ToArray();

                Dictionary<Placeholder, string> values =
                    CreatePlaceholderValues(eventModel, placeholders);

                messageProperty.Value = MessageParser.ReplacePlaceholders(
                    messageProperty.Value,
                    placeholders,
                    p => values[p]);
            }
        }

        private Dictionary<Placeholder, string> CreatePlaceholderValues(
            EventModel eventModel,
            IEnumerable<Placeholder> placeholders)
        {
            ILookup<string, Placeholder> placeholderLookup =
                    placeholders.ToLookup(t => t.Name);

            int offset = _template.DefaultPayloads;

            Dictionary<Placeholder, string> values =
                new Dictionary<Placeholder, string>();

            for (int i = 0; i < eventModel.InputParameters.Count; i++)
            {
                string name = eventModel.InputParameters[i].Name;
                int index = i + offset;

                foreach (Placeholder placeholder in placeholderLookup[name])
                {
                    values[placeholder] = placeholder.ToString(index);
                }
            }

            return values;
        }

        private void QualifyParameters(EventSourceModel eventSourceModel)
        {
            foreach (EventModel eventModel in eventSourceModel.Events)
            {
                QualifyEventParameters(eventModel);
            }

            if (!_allowComplexParameters && eventSourceModel.Events.Any(e => e.HasComplexTypeParameters))
            {
                throw new ArgumentException("ComplexType parameters are not allowed by the template.");
            }
        }

        private void QualifyEventParameters(EventModel eventModel)
        {
            foreach (EventParameterModel parameter in eventModel.InputParameters)
            {
                if (!ParameterTypeLookup.TryGet(parameter.Type, out IParameterTypeInfo typeInfo))
                {
                    eventModel.ComplexParameters.Add(parameter.Clone());
                }
                else
                {
                    eventModel.ValueParameters.Add(parameter.Clone());
                }
            }

            if (eventModel.HasComplexTypeParameters)
            {
                EventParameterModel parameter = new EventParameterModel
                {
                    Name = _eventComplexParameterName,
                    Type = "string"
                };

                eventModel.ValueParameters.Insert(0, parameter);
            }

            SetFirst(eventModel.InputParameters);
            SetFirst(eventModel.ValueParameters);
            SetFirst(eventModel.ComplexParameters);
        }

        private static void SetFirst(List<EventParameterModel> items)
        {
            if (items.Any())
            {
                items.First().IsFirst = true;
            }
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
                IEnumerable<string> types = eventModel.ValueParameters
                    .Select(t => GetWriteMethodParameterType(t.Type));
                hashSet.Add(new WriteMethod(types));
            }
            return hashSet;
        }

        private string GetWriteMethodParameterType(string typeName)
        {
            if (!ParameterTypeLookup.TryGet(typeName, out IParameterTypeInfo typeInfo))
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
