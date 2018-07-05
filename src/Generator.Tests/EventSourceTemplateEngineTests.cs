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

namespace Thor.Generator
{
    public class EventSourceTemplateEngineTests
    {
        [Fact]
        public void DontGenerateCSharpEventSource()
        {
            // arrange
            TemplateStorage templateStorage = new TemplateStorage();
            Template template = templateStorage.GetTemplate(ProjectSystem.Language.CSharp);

            EventSourceDefinitionVisitor eventSourceDefinitionVisitor = new EventSourceDefinitionVisitor();
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Resources.EventSourceWithComplexType);
            eventSourceDefinitionVisitor.Visit(syntaxTree.GetRoot());

            Exception ex = null;

            // act
            EventSourceTemplateEngine templateEngine = new EventSourceTemplateEngine(template);
            try
            {
                string eventSourceCode = templateEngine.Generate(eventSourceDefinitionVisitor.EventSource);
            }
            catch (ArgumentException e)
            {
                ex = e;
            }
            // assert
            Assert.NotNull(ex);
            ex.Message.Should().Be("ComplexType parameters are not allowed by the template.");
        }

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

            eventSourceVisitor.Classes.Should().HaveCount(1);
            eventSourceVisitor.Classes.First().Should()
                .Be(eventSourceDefinitionVisitor.EventSource.Name);

            eventSourceVisitor.MethodSignatures[0].Should()
                .Be("One(Guid ex, Guid correlationId, string messageType, string from, string to)");
            eventSourceVisitor.MethodSignatures[1].Should()
                .Be("Two(Guid messageId, Guid correlationId, string messageType, string from, string to)");
            eventSourceVisitor.MethodSignatures[2].Should()
                .Be("WriteCore(int eventId, Guid a, Guid b, string c, string d, string e)");
            eventSourceVisitor.MethodSignatures[3].Should()
                .Be("SetToEmptyIfNull(string value)");

            eventSourceVisitor.ImportedNamespaces.Count.Should()
                .Be(8);
            eventSourceVisitor.ImportedNamespaces[0].Should()
                .Be("using static Newtonsoft.Json;");
            eventSourceVisitor.ImportedNamespaces[1].Should()
                .Be("using System;");
            eventSourceVisitor.ImportedNamespaces[2].Should()
                .Be("using Gen = System.Generic;");
            eventSourceVisitor.ImportedNamespaces[3].Should()
                .Be("using Io = System.IO;");
            eventSourceVisitor.ImportedNamespaces[4].Should()
                .Be("using System.Linq;");
            eventSourceVisitor.ImportedNamespaces[5].Should()
                .Be("using static System.Math;");
            eventSourceVisitor.ImportedNamespaces[6].Should()
                .Be("using System.Text;");
            eventSourceVisitor.ImportedNamespaces[7].Should()
                .Be("using Tasks = System.Threading.Tasks;");
        }

        [Fact]
        public void GenerateCsharpComplexEventSource()
        {
            // arrange
            TemplateStorage templateStorage = new TemplateStorage();
            Template template = templateStorage.GetCustomTemplate("Defaults\\CSharpWithComplex");

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

            eventSourceVisitor.Classes.Should().HaveCount(1);
            eventSourceVisitor.Classes.First().Should()
                .Be(eventSourceDefinitionVisitor.EventSource.Name);

            eventSourceVisitor.MethodSignatures[0].Should()
                .Be("One(Exception ex, Guid correlationId, string messageType, string from, string to)");
            eventSourceVisitor.MethodSignatures[1].Should()
                .Be("One(int applicationId, Guid activityId, String attachmentId, Guid correlationId, string messageType, string from, string to)");
            eventSourceVisitor.MethodSignatures[2].Should()
                .Be("Two(Guid messageId, Guid correlationId, string messageType, string from, string to)");
            eventSourceVisitor.MethodSignatures[3].Should()
                .Be("Two(int applicationId, Guid activityId, Guid messageId, Guid correlationId, string messageType, string from, string to)");
            eventSourceVisitor.MethodSignatures[4].Should()
                .Be("WriteCore(int eventId, int applicationId, Guid activityId, string a, Guid b, string c, string d, string e)");
            eventSourceVisitor.MethodSignatures[5].Should()
                .Be("WriteCore(int eventId, int applicationId, Guid activityId, Guid a, Guid b, string c, string d, string e)");

            eventSourceVisitor.ImportedNamespaces.Count.Should()
                .Be(9);
            eventSourceVisitor.ImportedNamespaces[0].Should()
                .Be("using System;");
            eventSourceVisitor.ImportedNamespaces[1].Should()
                .Be("using Gen = System.Generic;");
            eventSourceVisitor.ImportedNamespaces[2].Should()
                .Be("using System.Linq;");
            eventSourceVisitor.ImportedNamespaces[3].Should()
                .Be("using static System.Math;");
            eventSourceVisitor.ImportedNamespaces[4].Should()
                .Be("using System.Text;");
            eventSourceVisitor.ImportedNamespaces[5].Should()
                .Be("using Tasks = System.Threading.Tasks;");
            eventSourceVisitor.ImportedNamespaces[6].Should()
                .Be("using Thor.Core;");
            eventSourceVisitor.ImportedNamespaces[7].Should()
                .Be("using Thor.Core.Abstractions;");
            eventSourceVisitor.ImportedNamespaces[8].Should()
                .Be("using Thor.Core.Transmission.Abstractions;");
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

            eventSourceVisitor.Classes.Should().HaveCount(1);
            eventSourceVisitor.MethodSignatures.Should().HaveCount(1);
            eventSourceVisitor.MethodSignatures[0].Should()
                .Be("Foo()");
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

            eventSourceVisitor.Classes.Should().HaveCount(1);
            eventSourceVisitor.ClassDocumentations.Should().HaveCount(1);
            eventSourceVisitor.MethodSignatures.Should().HaveCount(1);
            eventSourceVisitor.MethodDocumentations.Should().HaveCount(1);

            eventSourceVisitor.ClassDocumentations.First().Should().Be(eventSourceModel.DocumentationXml);
            eventSourceVisitor.MethodDocumentations.First().Should().Be(eventModel.DocumentationXml);
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
