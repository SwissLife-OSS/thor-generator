using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;
using ChilliCream.Tracing.Generator.Templates;
using ChilliCream.Tracing.Generator.Properties;
using Microsoft.CodeAnalysis;
using FluentAssertions;
using System.Linq;
using ChilliCream.Tracing.Generator.ProjectSystem;

namespace ChilliCream.Tracing.Generator
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

            eventSourceVisitor.Classes.Should().HaveCount(1);
            eventSourceVisitor.Classes.First().Should()
                .Be(eventSourceDefinitionVisitor.EventSource.Name);

            eventSourceVisitor.MethodSignatures[0].Should()
                .Be("One(Guid messageId, Guid correlationId, string messageType, string from, string to)");
            eventSourceVisitor.MethodSignatures[1].Should()
                .Be("Two(Guid messageId, Guid correlationId, string messageType, string from, string to)");
            eventSourceVisitor.MethodSignatures[2].Should()
                .Be("WriteCore(int eventId, int applicationId, Guid activityId, Guid a, Guid b, string c, string d, string e)");
            eventSourceVisitor.MethodSignatures[3].Should()
                .Be("SetToEmptyIfNull(string value)");
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
    }

    public class EventSourceVisitor
        : CSharpSyntaxWalker
    {
        public IList<string> Classes { get; } = new List<string>();
        public IList<string> MethodSignatures { get; } = new List<string>();

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            Classes.Add(node.Identifier.Text);
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

            base.VisitMethodDeclaration(node);
        }
    }
}
