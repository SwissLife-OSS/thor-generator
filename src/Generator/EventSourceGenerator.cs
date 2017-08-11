﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChilliCream.Logging.Generator.Models;
using ChilliCream.Logging.Generator.Resources;
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
            EventSourceModel eventSourceModel = CreateGeneratorModel();
            return Render.StringToString(_templateCode, eventSourceModel, renderContextBehaviour: new RenderContextBehaviour { HtmlEncoder = t => t });
        }

        private EventSourceModel CreateGeneratorModel()
        {
            EventSourceModel eventSourceModel = new EventSourceModel
            {
                Name = _eventSourceDefinition.ClassName,
                Namespace = _eventSourceDefinition.Namespace,
                AttributeArgumentSyntax = CreateAttributeArgumentSyntax()
            };

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
            int c = 97;
            int i = 2;
            bool isFollowing = false;

            foreach (WriteMethod writeMethod in GetWriteMethods())
            {
                if (!writeMethod.Equals(_defaultWriteMethod))
                {
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

        // TODO : Add type handlers
        private string GetWriteMethodParameterType(string type)
        {
            return "string";
        }

        private void AddTypeDetails(EventParameterModel eventParameterModel)
        {
            eventParameterModel.IsString = true;
            eventParameterModel.Size = "((b.Length + 1) * 2)";
            eventParameterModel.Operator = null;
        }
    }
}
