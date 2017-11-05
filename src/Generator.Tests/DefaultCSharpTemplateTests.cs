using System;
using ChilliCream.Tracing.Generator.Templates;
using Xunit;
using ChilliCream.Tracing.Generator.ProjectSystem;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ChilliCream.Tracing.Generator
{
    public class DefaultCSharpTemplateTests
    {
        [Fact]
        public void RetrieveTemplate() 
        {
            // arrange
            TemplateStorage templateStorage = new TemplateStorage();

            // act
            Template template = templateStorage.GetTemplate(Language.CSharp);

            // assert
            template.Should().NotBeNull();
        }

        [Fact]
        public void GenerateEventSource()
        {
            // arrange
            TemplateStorage templateStorage = new TemplateStorage();
            Template template = templateStorage.GetTemplate(Language.CSharp);

            EventSourceModel eventSourceModel = new EventSourceModel();
            EventModel eventModel = new EventModel();
            eventSourceModel.Events.Add(eventModel);

            // act
            EventSourceTemplateEngine templateEngine = new EventSourceTemplateEngine(template);
            string eventSourceCode = templateEngine.Generate(eventSourceModel);
        
            // assert
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(eventSourceCode);

            EventSourceVisitor eventSourceVisitor = new EventSourceVisitor();
            eventSourceVisitor.Visit(syntaxTree.GetRoot());
        }


    }
}
