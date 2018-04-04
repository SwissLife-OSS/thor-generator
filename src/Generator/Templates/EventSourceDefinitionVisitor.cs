using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace Thor.Generator.Templates
{
    /// <summary>
    /// This visitor inspects syntax trees for event source definitions.
    /// </summary>
    /// <seealso cref="Microsoft.CodeAnalysis.CSharp.CSharpSyntaxWalker" />
    internal class EventSourceDefinitionVisitor
        : CSharpSyntaxWalker
    {
        private static HashSet<string> _eventSourceAttributeProperties = new HashSet<string>
        {
            "Guid",
            "Name",
            "LocalizationResources"
        };

        private static HashSet<string> _eventAttributeProperties = new HashSet<string>
        {
            "Level",
            "Keywords",
            "Opcode",
            "Task",
            "Channel",
            "Version",
            "Message",
            "Tags",
            "ActivityOptions"
        };

        private const string _eventSourceAttributeName = "EventSourceDefinition";
        private const string _eventAttributeName = "Event";

        public EventSourceModel EventSource { get; } = new EventSourceModel();

        private NamespaceDeclarationSyntax NamespaceDeclaration { get; set; }


        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            NamespaceDeclaration = node;
            base.VisitNamespaceDeclaration(node);
        }

        public override void VisitAttribute(AttributeSyntax node)
        {
            if (IsDefinitionAttribute(node)
                && node.Parent?.Parent is InterfaceDeclarationSyntax interfaceDeclaration)
            {
                ParseEventSourceAttribute(node);
                EventSource.InterfaceName = interfaceDeclaration.Identifier.Text;
                EventSource.Namespace = NamespaceDeclaration?.Name.ToString() ?? "EmptyNamespace";

                EventSource.Name = EventSource.InterfaceName.StartsWith(Constants.InterfacePrefix)
                    ? EventSource.InterfaceName.Substring(Constants.InterfacePrefix.Length)
                    : string.Concat(EventSource.InterfaceName, Constants.ClassNamePostfix);

                EventSource.FileName = EventSource.Name + ".cs";
                EventSource.DocumentationXml = GetDocumentationXml(interfaceDeclaration);
            }

            base.VisitAttribute(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (EventSource != null)
            {
                AttributeSyntax eventAttribute = node.AttributeLists
                    .SelectMany(l => l.Attributes)
                    .FirstOrDefault(t => IsEventAttribute(t));

                if (eventAttribute != null
                    && eventAttribute.ArgumentList.Arguments.Count > 0
                    && TryParseEventAttribute(eventAttribute, out EventModel eventModel))
                {
                    foreach (ParameterSyntax eventParameter in node.ParameterList.Parameters)
                    {
                        eventModel.AddParameter(GetName(eventParameter), GetValue(eventParameter));
                    }

                    eventModel.Name = node.Identifier.Text;
                    eventModel.DocumentationXml = GetDocumentationXml(node);

                    EventSource.Events.Add(eventModel);
                }
            }

            base.VisitMethodDeclaration(node);
        }

        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            var namespaceModel = new NamespaceModel(node.StaticKeyword.Value != null, node.Alias?.Name?.ToString(), node.Name.ToString());

            if (!EventSource.Usings.Contains(namespaceModel))
            {
                EventSource.Usings.Add(namespaceModel);
            }

            base.VisitUsingDirective(node);
        }

        private void ParseEventSourceAttribute(AttributeSyntax node)
        {
            foreach (AttributeArgumentSyntax argument in node.ArgumentList?.Arguments
                ?? Enumerable.Empty<AttributeArgumentSyntax>())
            {
                string name = GetName(argument);
                if (_eventSourceAttributeProperties.Contains(name))
                {
                    EventSource.Attribute.AddProperty(name,
                        GetValueSyntax(argument));
                }
            }
        }

        private bool TryParseEventAttribute(AttributeSyntax eventAttribute, out EventModel eventModel)
        {
            eventModel = new EventModel();

            AttributeArgumentSyntax eventId = eventAttribute
                .ArgumentList.Arguments.First();

            if (eventId.NameEquals != null)
            {
                eventModel = null;
                return false;
            }

            eventModel.Id = int.Parse(GetValue(eventId));
            eventModel.Attribute.AddProperty(GetValueSyntax(eventId));

            foreach (AttributeArgumentSyntax argument in eventAttribute.ArgumentList.Arguments.Skip(1))
            {
                string name = GetName(argument);
                if (_eventAttributeProperties.Contains(name))
                {
                    eventModel.Attribute.AddProperty(name, GetValueSyntax(argument));
                }
            }

            return true;
        }

        private bool IsDefinitionAttribute(AttributeSyntax attribute)
        {
            string name = GetSimpleNameFromNode(attribute)
                .Identifier
                .Text;

            if (name == _eventSourceAttributeName
                || name == _eventSourceAttributeName + "Attribute"
                || name.EndsWith("." + _eventSourceAttributeName)
                || name.EndsWith("." + _eventSourceAttributeName + "Attribute"))
            {
                return true;
            }

            return false;
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
            if (node.Expression is LiteralExpressionSyntax literalExpression)
            {
                return literalExpression.Token.Text.Trim('\"');
            }

            if (node.Expression is MemberAccessExpressionSyntax memberAccessExpression)
            {
                return memberAccessExpression.Name.Identifier.Text;
            }

            throw new InvalidOperationException("Could not parse AttributeArgumentSyntax");
        }

        private static string GetValueSyntax(AttributeArgumentSyntax node)
        {
            if (node.Expression is LiteralExpressionSyntax literalExpression)
            {
                return literalExpression.Token.Text.ToString();
            }

            if (node.Expression is MemberAccessExpressionSyntax memberAccessExpression)
            {
                return memberAccessExpression.ToString();
            }

            throw new InvalidOperationException("Could not parse AttributeArgumentSyntax");
        }

        private static string GetValue(ParameterSyntax node)
        {
            return (node.Type as PredefinedTypeSyntax)?.Keyword.Text
                ?? (node.Type as QualifiedNameSyntax)?.ToString()
                ?? (node.Type as IdentifierNameSyntax)?.ToString();
        }

        private static string GetDocumentationXml(CSharpSyntaxNode syntaxNode)
        {
            string documentationXml = ResolveDocumentationXml(syntaxNode);

            if(string.IsNullOrEmpty(documentationXml))
            {
                return null;
            }

            if(documentationXml.StartsWith("///") || documentationXml.StartsWith("//"))
            {
                return documentationXml;
            }

            return "///" + documentationXml;
        }

        private static string ResolveDocumentationXml(CSharpSyntaxNode syntaxNode)
        {
            SyntaxTrivia documentation = syntaxNode.GetLeadingTrivia()
                 .FirstOrDefault(t => t.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia);
            if (documentation.Equals(default(SyntaxTrivia)))
            {
                documentation = syntaxNode.GetLeadingTrivia()
                    .FirstOrDefault(t => t.Kind() == SyntaxKind.MultiLineDocumentationCommentTrivia);
            }

            if (!documentation.Equals(default(SyntaxTrivia)))
            {
                return documentation.ToString();
            }

            documentation = syntaxNode.GetLeadingTrivia()
                .FirstOrDefault(t => t.Kind() == SyntaxKind.SingleLineCommentTrivia);
            if (documentation.Equals(default(SyntaxTrivia)))
            {
                documentation = syntaxNode.GetLeadingTrivia()
                    .FirstOrDefault(t => t.Kind() == SyntaxKind.MultiLineCommentTrivia);
            }

            if (!documentation.Equals(default(SyntaxTrivia)))
            {
                return documentation.ToString();
            }

            return null;
        }
    }
}