using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;
using Thor.Generator.Templates;
using Thor.Generator.Properties;
using Microsoft.CodeAnalysis;
using FluentAssertions;
using System.Linq;
using Thor.Generator.ProjectSystem;
using System.IO;
using ChilliCream.Testing;

namespace Thor.Generator
{
    public class EventSourceTemplateEngineTests
    {
        [Fact]
        public void GenerateCSharpEventSource()
        {
            // arrange
            TemplateStorage templateStorage = new TemplateStorage();
            Template template = templateStorage.GetTemplate(ProjectSystem.Language.CSharp);

            EventSourceDefinitionVisitor eventSourceDefinitionVisitor = new EventSourceDefinitionVisitor();
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Resources.EventSourceWithTwoMethods);
            eventSourceDefinitionVisitor.Visit(syntaxTree.GetRoot());

            // act
            EventSourceTemplateEngine templateEngine = new EventSourceTemplateEngine(template);
            string eventSourceCode = templateEngine.Generate(eventSourceDefinitionVisitor.EventSource);

            // assert
            syntaxTree = CSharpSyntaxTree.ParseText(eventSourceCode);

            EventSourceVisitor eventSourceVisitor = new EventSourceVisitor();
            eventSourceVisitor.Visit(syntaxTree.GetRoot());

            eventSourceVisitor.Snapshot();
        }

        [Fact]
        public void GenerateCsharpComplexEventSource()
        {
            // arrange
            TemplateStorage templateStorage = new TemplateStorage();
            Template template = templateStorage.GetCustomTemplate(Path.Combine("Defaults", "CSharp"));

            EventSourceDefinitionVisitor eventSourceDefinitionVisitor = new EventSourceDefinitionVisitor();
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Resources.EventSourceWithComplexType);
            eventSourceDefinitionVisitor.Visit(syntaxTree.GetRoot());

            // act
            EventSourceTemplateEngine templateEngine = new EventSourceTemplateEngine(template);
            string eventSourceCode = templateEngine.Generate(eventSourceDefinitionVisitor.EventSource);

            // assert
            syntaxTree = CSharpSyntaxTree.ParseText(eventSourceCode);

            EventSourceVisitor eventSourceVisitor = new EventSourceVisitor();
            eventSourceVisitor.Visit(syntaxTree.GetRoot());

            eventSourceVisitor.Snapshot();
        }

        [Fact]
        public void GenerateCsharpComplexEventSource1()
        {
            // arrange
            TemplateStorage templateStorage = new TemplateStorage();
            Template template = templateStorage.GetCustomTemplate(Path.Combine("Foo", "CSharp"));

            EventSourceDefinitionVisitor eventSourceDefinitionVisitor = new EventSourceDefinitionVisitor();
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Resources.EventSourceWithComplexType);
            eventSourceDefinitionVisitor.Visit(syntaxTree.GetRoot());

            // act
            EventSourceTemplateEngine templateEngine = new EventSourceTemplateEngine(template);
            string eventSourceCode = templateEngine.Generate(eventSourceDefinitionVisitor.EventSource);

            // assert
            syntaxTree = CSharpSyntaxTree.ParseText(eventSourceCode);

            EventSourceVisitor eventSourceVisitor = new EventSourceVisitor();
            eventSourceVisitor.Visit(syntaxTree.GetRoot());

            eventSourceVisitor.Snapshot();
        }

        [Fact]
        public void GenerateCSharpEventSourceWithEventWithoutArguments()
        {
            // arrange
            TemplateStorage templateStorage = new TemplateStorage();
            Template template = templateStorage.GetTemplate(Language.CSharp);

            EventSourceModel eventSourceModel = new EventSourceModel();
            EventModel eventModel = new EventModel();
            eventModel.Name = "Foo";
            eventSourceModel.Events.Add(eventModel);

            // act
            EventSourceTemplateEngine templateEngine = new EventSourceTemplateEngine(template);
            string eventSourceCode = templateEngine.Generate(eventSourceModel);

            // assert
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(eventSourceCode);

            EventSourceVisitor eventSourceVisitor = new EventSourceVisitor();
            eventSourceVisitor.Visit(syntaxTree.GetRoot());

            eventSourceVisitor.Snapshot();
        }

        [Fact]
        public void GenerateCSharpEventSourceWithEventWithDocumentation()
        {
            // arrange
            TemplateStorage templateStorage = new TemplateStorage();
            Template template = templateStorage.GetTemplate(Language.CSharp);

            EventSourceModel eventSourceModel = new EventSourceModel();
            eventSourceModel.DocumentationXml = "// <summary>eventsource</summary>";
            EventModel eventModel = new EventModel();
            eventModel.Name = "Foo";
            eventModel.DocumentationXml = "// <summary>event</summary>";
            eventSourceModel.Events.Add(eventModel);

            // act
            EventSourceTemplateEngine templateEngine = new EventSourceTemplateEngine(template);
            string eventSourceCode = templateEngine.Generate(eventSourceModel);

            // assert
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(eventSourceCode);

            EventSourceVisitor eventSourceVisitor = new EventSourceVisitor();
            eventSourceVisitor.Visit(syntaxTree.GetRoot());

            eventSourceVisitor.Snapshot();
        }

    }

    public class EventSourceVisitor
        : CSharpSyntaxWalker
    {
        public IList<string> Classes { get; } = new List<string>();
        public IList<string> ClassDocumentations { get; } = new List<string>();
        public IList<string> MethodSignatures { get; } = new List<string>();
        public IList<string> MethodDocumentations { get; } = new List<string>();
        public IList<string> ImportedNamespaces { get; } = new List<string>();


        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            Classes.Add(node.Identifier.Text);
            ClassDocumentations.Add(GetDocumentationXml(node));
            base.VisitClassDeclaration(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            StringBuilder methodSignature = new StringBuilder();
            methodSignature.Append(node.Identifier.Text);
            methodSignature.Append("(");

            bool isFirst = true;
            foreach (ParameterSyntax parameterSyntax in node.ParameterList.Parameters)
            {
                if (!isFirst)
                {
                    methodSignature.Append(", ");
                }

                methodSignature.Append(parameterSyntax.Type.ToString());
                methodSignature.Append(" ");
                methodSignature.Append(parameterSyntax.Identifier.Text);
                isFirst = false;
            }

            methodSignature.Append(")");
            MethodSignatures.Add(methodSignature.ToString());
            MethodDocumentations.Add(GetDocumentationXml(node));

            base.VisitMethodDeclaration(node);
        }

        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            ImportedNamespaces.Add(node.ToString());

            base.VisitUsingDirective(node);
        }

        private static string GetDocumentationXml(CSharpSyntaxNode syntaxNode)
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
