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
                    .FirstOrDefault(t => t.Name.Equals("Message",
                        StringComparison.Ordinal));
            if (messageProperty != null)
            {
                ILookup<string, Placeholder> placeholderLookup =
                    MessageParser.FindPlaceholders(messageProperty.Value)
                        .ToLookup(t => t.Name);

                int offset = _template.DefaultPayloads;
                StringBuilder message = new StringBuilder(messageProperty.Value);

                for (int i = 0; i < eventModel.InputParameters.Count; i++)
                {
                    string name = eventModel.InputParameters[i].Name;
                    foreach (Placeholder placeholder in placeholderLookup[name])
                    {
                        int index = i + offset;
                        message.Replace(
                            placeholder.ToString(),
                            placeholder.ToString(index));
                    }
                }

                messageProperty.Value = message.ToString();
            }
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
                    Type = typeof(string).Name
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

    internal class MessageParser
    {
        public static IEnumerable<Placeholder> FindPlaceholders(string message)
        {
            int position = 0;
            while (Skip(message, '{', ref position))
            {
                int start = position;

                if (Peek(message, '{', start))
                {
                    position += 2;
                    continue;
                }
                else if (Skip(message, '}', ref position))
                {
                    yield return ParsePlaceholder(
                        start,
                        position,
                        message.Substring(
                            start + 1,
                            position - start - 1));
                }
            }
        }

        private static Placeholder ParsePlaceholder(
            int start,
            int end,
            string placeholder)
        {
            int index = 0;

            if (Skip(placeholder, ':', ref index))
            {
                return new Placeholder(
                    start,
                    end,
                    placeholder.Substring(0, index),
                    placeholder.Substring(index + 1));
            }

            return new Placeholder(start, end, placeholder);
        }

        private static bool Skip(string message, char token, ref int position)
        {
            while (position < message.Length && message[position] != token)
            {
                position++;
            }

            return (position < message.Length && message[position] == token);
        }

        private static bool Peek(string message, char token, int position)
        {
            int next = position + 1;
            return message.Length > next
                && message[next] == token;
        }
    }

    internal class Placeholder
    {
        public Placeholder(int start, int end, string name)
        {
            Start = start;
            End = end;
            Name = name;
        }

        public Placeholder(int start, int end, string name, string format)
        {
            Start = start;
            End = end;
            Name = name;
            Format = format;
        }

        public int Start { get; }

        public int End { get; }

        public string Name { get; }

        public string Format { get; }

        public override string ToString()
        {
            if (Format == null)
            {
                return $"{{{Name}}}";
            }
            return $"{{{Name}:{Format}}}";
        }

        public string ToString(int index)
        {
            if (Format == null)
            {
                return $"{{{index}}}";
            }
            return $"{{{index}:{Format}}}";
        }
    }
}
