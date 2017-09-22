using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChilliCream.Logging.Generator.Types;
using ChilliCream.Tracing.Generator.Analyzer;
using ChilliCream.Tracing.Generator.Models;
using ChilliCream.Tracing.Generator.Resources;
using Nustache.Core;

namespace ChilliCream.Logging.Generator
{
    public class EventSourceGenerator
    {
        private static readonly WriteMethod _defaultWriteMethod = new WriteMethod(new[] { "string" });
        private readonly EventSourceDefinition _eventSourceDefinition;
        private readonly string _templateCode;

        public EventSourceGenerator(EventSourceDefinition eventSourceDefinition)
        {
            if (eventSourceDefinition == null)
            {
                throw new ArgumentNullException(nameof(eventSourceDefinition));
            }

            _eventSourceDefinition = eventSourceDefinition;
            _templateCode = Encoding.UTF8.GetString(Templates.EventSourceBase);
        }

        public string CreateEventSource()
        {
            // generate event source
            EventSourceModel eventSourceModel = CreateGeneratorModel();
            string sourceCode = Render.StringToString(_templateCode, eventSourceModel,
                renderContextBehaviour: new RenderContextBehaviour { HtmlEncoder = t => t });

            // reformat methods
            //SyntaxNode syntaxRoot = CSharpSyntaxTree.ParseText(sourceCode).GetRoot();
            //FormatMethodsRewriter rewriter = new FormatMethodsRewriter();
            //syntaxRoot = rewriter.Visit(syntaxRoot);
            //return syntaxRoot.ToString();
            return sourceCode;
        }

        private EventSourceModel CreateGeneratorModel()
        {
            EventSourceModel eventSourceModel = new EventSourceModel
            {
                Name = _eventSourceDefinition.ClassName,
                Namespace = _eventSourceDefinition.Namespace
            };

            string attributeArgumentSyntax = CreateAttributeArgumentSyntax();
            if (attributeArgumentSyntax != null)
            {
                eventSourceModel.Attribute = new AttributeModel
                {
                    ArgumentSyntax = attributeArgumentSyntax
                };
            }

            AddEvents(eventSourceModel);
            AddWriteMethods(eventSourceModel);

            return eventSourceModel;
        }

        private string CreateAttributeArgumentSyntax()
        {
            StringBuilder argumentSyntax = new StringBuilder();
            if (_eventSourceDefinition.Name != null)
            {
                argumentSyntax.Append($"Name =\"{_eventSourceDefinition.Name}\"");
            }

            if (_eventSourceDefinition.Guid != null)
            {
                if (argumentSyntax.Length > 0)
                {
                    argumentSyntax.Append(", ");
                }
                argumentSyntax.Append($"Guid =\"{_eventSourceDefinition.Guid}\"");
            }

            if (_eventSourceDefinition.LocalizationResources != null)
            {
                if (argumentSyntax.Length > 0)
                {
                    argumentSyntax.Append(", ");
                }
                argumentSyntax.Append($"LocalizationResources =\"{_eventSourceDefinition.LocalizationResources}\"");
            }
            return argumentSyntax.ToString();
        }

        private void AddEvents(EventSourceModel eventSourceModel)
        {
            foreach (EventDefinition eventDefinition in _eventSourceDefinition.Events)
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

        private void AddWriteMethods(EventSourceModel eventSourceModel)
        {
            foreach (WriteMethod writeMethod in GetWriteMethods())
            {
                if (!writeMethod.Equals(_defaultWriteMethod))
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

        private IEnumerable<WriteMethod> GetWriteMethods()
        {
            HashSet<WriteMethod> hashSet = new HashSet<WriteMethod>();
            foreach (EventDefinition eventDefinition in _eventSourceDefinition.Events)
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
            if (!ParameterTypeInfo.TryGet(typeName, out typeInfo))
            {
                throw new ArgumentException("The specified type is not allowed.", nameof(typeName));
            }
            return typeInfo.Name;
        }

        private void AddTypeDetails(EventParameterModel eventParameterModel)
        {
            IParameterTypeInfo typeInfo;
            if (!ParameterTypeInfo.TryGet(eventParameterModel.Type, out typeInfo))
            {
                throw new ArgumentException("The specified type is not allowed.", nameof(eventParameterModel));
            }

            eventParameterModel.IsString = typeInfo.IsString;
            eventParameterModel.Size = typeInfo.Size;
            eventParameterModel.Operator = typeInfo.Operator;
        }
    }
}
