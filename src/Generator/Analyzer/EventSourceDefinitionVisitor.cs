using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics.Tracing;

namespace ChilliCream.Logging.Generator
{
    /// <summary>
    /// This visitor inspects syntax trees for event source definitions.
    /// </summary>
    /// <seealso cref="Microsoft.CodeAnalysis.CSharp.CSharpSyntaxWalker" />
    public class EventSourceDefinitionVisitor
        : CSharpSyntaxWalker
    {
        private const string _eventSourceAttributeName = "EventSourceDefinition";
        private const string _eventAttributeName = "Event";

        /// <summary>
        /// Gets the event source definition that was found 
        /// after inspecting a syntax tree. If this property is 
        /// <c>null</c> no event source was found.
        /// </summary>
        /// <value>The event source definition.</value>
        public EventSourceDefinition EventSourceDefinition { get; private set; }

        public override void VisitAttribute(AttributeSyntax node)
        {
            if (node.Parent?.Parent is InterfaceDeclarationSyntax)
            {
                EventSourceDefinition = ParseEventSourceAttribute(node);
            }

            base.VisitAttribute(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            AttributeSyntax eventAttribute = node.AttributeLists
                .SelectMany(l => l.Attributes)
                .FirstOrDefault(t => IsEventAttribute(t));

            if (EventSourceDefinition != null
                && eventAttribute != null
                && eventAttribute.ArgumentList.Arguments.Count > 0
                && TryParseEventAttribute(eventAttribute, out EventDefinition eventDefinition)
                && TryParseEventArgument(node, out List<EventArgumentDefinition> eventArgumentDefinitions))
            {
                eventDefinition.Name = node.Identifier.Text;
                eventDefinition.Arguments.AddRange(eventArgumentDefinitions);
                EventSourceDefinition.Events.Add(eventDefinition);
            }

            base.VisitMethodDeclaration(node);
        }

        private EventSourceDefinition ParseEventSourceAttribute(AttributeSyntax node)
        {
            EventSourceDefinition eventSourceDefinition = new EventSourceDefinition();

            foreach (AttributeArgumentSyntax argument in node.ArgumentList.Arguments)
            {
                string value = GetValue(argument);
                switch (GetName(argument))
                {
                    case "Guid":
                        eventSourceDefinition.Guid = value;
                        break;

                    case "Name":
                        eventSourceDefinition.Name = value;
                        break;

                    case "LocalizationResources":
                        eventSourceDefinition.LocalizationResources = value;
                        break;
                }
            }

            return eventSourceDefinition;
        }

        private bool TryParseEventAttribute(AttributeSyntax eventAttribute, out EventDefinition eventDefinition)
        {
            eventDefinition = new EventDefinition();

            AttributeArgumentSyntax eventId = eventAttribute
                .ArgumentList.Arguments.First();

            if (eventId.NameEquals != null)
            {
                eventDefinition = null;
                return false;
            }

            eventDefinition.EventId = int.Parse(GetValue(eventId));
            eventDefinition.AttributeSyntax = eventAttribute.ToString();

            foreach (AttributeArgumentSyntax argument in eventAttribute.ArgumentList.Arguments.Skip(1))
            {
                string value = GetValue(argument);
                switch (GetName(argument))
                {
                    case "Level":
                        eventDefinition.Level = (EventLevel)Enum.Parse(typeof(EventLevel), value);
                        break;

                    case "Keywords":
                        eventDefinition.Keywords = (EventKeywords)Enum.Parse(typeof(EventKeywords), value);
                        break;

                    case "Opcode":
                        eventDefinition.Opcode = (EventOpcode)Enum.Parse(typeof(EventOpcode), value);
                        break;

                    case "Task":
                        eventDefinition.Task = (EventTask)Enum.Parse(typeof(EventTask), value);
                        break;

                    case "Channel":
                        eventDefinition.Channel = (EventChannel)Enum.Parse(typeof(EventChannel), value);
                        break;

                    case "Version":
                        eventDefinition.Version = byte.Parse(value);
                        break;

                    case "Message":
                        eventDefinition.Message = value;
                        break;

                    case "Tags":
                        eventDefinition.Tags = (EventTags)Enum.Parse(typeof(EventTags), value);
                        break;

                    case "ActivityOptions":
                        eventDefinition.ActivityOptions = (EventActivityOptions)Enum.Parse(typeof(EventActivityOptions), value);
                        break;
                }
            }

            return true;
        }

        private bool TryParseEventArgument(MethodDeclarationSyntax eventDeclaration, out List<EventArgumentDefinition> eventArgument)
        {
            eventArgument = new List<EventArgumentDefinition>();

            foreach (ParameterSyntax eventParameter in eventDeclaration.ParameterList.Parameters)
            {
                eventArgument.Add(new EventArgumentDefinition
                {
                    Name = GetName(eventParameter),
                    Type = GetValue(eventParameter),
                    ParameterSyntax = eventParameter.ToString()
                });
            }

            return true;
        }

        private bool IsDefinitionAttribute(AttributeSyntax attribute)
        {
            return GetSimpleNameFromNode(attribute)
                .Identifier
                .Text
                .StartsWith(_eventSourceAttributeName);
        }

        private bool IsEventAttribute(AttributeSyntax attribute)
        {
            return GetSimpleNameFromNode(attribute)
                .Identifier
                .Text
                .StartsWith(_eventAttributeName);
        }

        private static SimpleNameSyntax GetSimpleNameFromNode(AttributeSyntax node)
        {
            return (node.Name as IdentifierNameSyntax)
                ?? (node.Name as QualifiedNameSyntax).Right
                ?? (node.Name as AliasQualifiedNameSyntax).Name;
        }

        private static string GetName(AttributeArgumentSyntax node)
        {
            return node.NameEquals.Name.Identifier.Text;
        }

        private static string GetName(ParameterSyntax node)
        {
            return node.Identifier.Text;
        }

        private static string GetValue(AttributeArgumentSyntax node)
        {
            string value = (node.Expression as LiteralExpressionSyntax)?.Token.Text.Trim('\"')
                ?? (node.Expression as MemberAccessExpressionSyntax)?.Name.Identifier.Text;
            return value;
        }

        private static string GetValue(ParameterSyntax node)
        {
            return (node.Type as PredefinedTypeSyntax)?.Keyword.Text;
        }
    }
}