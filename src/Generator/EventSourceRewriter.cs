using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ChilliCream.Logging.Generator
{
    public class EventSourceRewriter
        : CSharpSyntaxRewriter
    {
        public EventSourceRewriter(EventSourceDefinition eventSourceDefinition)
        {
            if (eventSourceDefinition == null)
            {
                throw new ArgumentNullException(nameof(eventSourceDefinition));
            }

            EventSourceDefinition = eventSourceDefinition;
        }

        public EventSourceDefinition EventSourceDefinition { get; set; }


        public override SyntaxNode VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            NamespaceDeclarationSyntax namespaceDeclaration = node
                .WithName(IdentifierName(EventSourceDefinition.Namespace))
                .WithTriviaFrom(node.Name);

            return base.VisitNamespaceDeclaration(namespaceDeclaration);
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            ClassDeclarationSyntax classDeclaration = RenameClass(node);
            classDeclaration = AddEventSourceAttribute(classDeclaration);
            classDeclaration = classDeclaration.WithMembers(CreateWriteMethods());
            return base.VisitClassDeclaration(classDeclaration);
        }

        private ClassDeclarationSyntax RenameClass(ClassDeclarationSyntax classDeclaration)
        {
            SyntaxToken newIdentifier = Identifier(EventSourceDefinition.ClassName)
                .WithTriviaFrom(classDeclaration.Identifier);
            return classDeclaration.WithIdentifier(newIdentifier);
        }

        private ClassDeclarationSyntax AddEventSourceAttribute(ClassDeclarationSyntax classDeclaration)
        {
            SyntaxList<AttributeListSyntax> attributeLists =
                classDeclaration.AttributeLists.Add(
                    AttributeList(CreateEventSourceAttribute()));

            return classDeclaration.WithoutTrivia()
                .WithAttributeLists(attributeLists)
                .WithTriviaFrom(classDeclaration);
        }

        private SeparatedSyntaxList<AttributeSyntax> CreateEventSourceAttribute()
        {
            SeparatedSyntaxList<AttributeSyntax> attributeList =
                new SeparatedSyntaxList<AttributeSyntax>();
            attributeList = attributeList.Add(RewriteEventSourceAttribute(
                Attribute(IdentifierName("EventSource"))));
            return attributeList;
        }

        private AttributeSyntax RewriteEventSourceAttribute(AttributeSyntax attribute)
        {
            SeparatedSyntaxList<AttributeArgumentSyntax> arguments =
                new SeparatedSyntaxList<AttributeArgumentSyntax>();

            if (EventSourceDefinition.Name != null)
            {
                arguments = arguments.Add(
                    AttributeArgument(
                        ParseExpression($"Name = \"{EventSourceDefinition.Name}\"")));
            }

            if (EventSourceDefinition.Guid != null)
            {
                arguments = arguments.Add(
                    AttributeArgument(
                        ParseExpression($"Guid = \"${EventSourceDefinition.Guid}\"")));
            }

            if (EventSourceDefinition.LocalizationResources != null)
            {
                arguments = arguments.Add(
                    AttributeArgument(
                        ParseExpression($"LocalizationResources = \"${EventSourceDefinition.LocalizationResources}\"")));
            }

            return attribute.WithArgumentList(AttributeArgumentList(arguments));
        }

        private SyntaxList<MemberDeclarationSyntax> CreateWriteMethods()
        {
            SyntaxList<MemberDeclarationSyntax> members = new SyntaxList<MemberDeclarationSyntax>();
            foreach (WriteMethod method in GetWriteMethods())
            {
                MethodDeclarationSyntax methodDeclaration = MethodDeclaration(IdentifierName("void "), "WriteCore");

                ParameterListSyntax parameters = methodDeclaration.ParameterList
                    .AddParameters(CreateWriteMethodParameters(method.ParameterTypes).ToArray());

                AttributeListSyntax attributeList = AttributeList(
                    SeparatedList(new[] { Attribute(ParseName("NonEvent")) }));

                methodDeclaration = methodDeclaration
                    .WithParameterList(parameters)
                    .AddModifiers(Identifier("private "))
                    .WithAttributeLists(List(new[] { attributeList }));

                members = members.Add(methodDeclaration);
            }
            return members;
        }


        private IEnumerable<ParameterSyntax> CreateWriteMethodParameters(IEnumerable<string> parameterTypes)
        {
            int i = 97;

            foreach (string type in parameterTypes)
            {
                ParameterSyntax parameter = Parameter(Identifier(((char)i).ToString()));
                parameter = parameter.WithType(IdentifierName(type + " "));
                yield return parameter;
                i++;
            }
        }


        private IEnumerable<WriteMethod> GetWriteMethods()
        {
            HashSet<WriteMethod> hashSet = new HashSet<WriteMethod>();
            foreach (EventDefinition eventDefinition in EventSourceDefinition.Events)
            {
                hashSet.Add(new WriteMethod(eventDefinition.Arguments.Select(t => t.Type)));

            }
            return hashSet;
        }
    }
}