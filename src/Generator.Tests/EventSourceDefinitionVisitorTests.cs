using System.Diagnostics.Tracing;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using FluentAssertions;
using Xunit;
using ChilliCream.Logging.Generator.EventSourceDefinitions;
using ChilliCream.Logging.Generator.Analyzer;

namespace ChilliCream.Logging.Generator
{
    [Trait("Analyzer", null)]
    public class EventSourceDefinitionVisitorTests
    {
        [Fact(DisplayName = "Analyzer: Simple Event Source")]
        public void InspectSimpleEventSource()
        {
            // arrange
            SyntaxTree tree = CSharpSyntaxTree.ParseText(EventSourceSamples.SimpleEventSource);
            SyntaxNode eventSource = tree.GetRoot();

            // act
            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(eventSource);

            // assert
            visitor.EventSourceDefinition.Should().NotBeNull();

            visitor.EventSourceDefinition.Namespace.Should().Be("FooNamespace");
            visitor.EventSourceDefinition.ClassName.Should().Be("SimpleEventSource");
            visitor.EventSourceDefinition.Name.Should().Be("Mock");
            visitor.EventSourceDefinition.Guid.Should().BeNull();
            visitor.EventSourceDefinition.LocalizationResources.Should().BeNull();

            visitor.EventSourceDefinition.Events.Should().HaveCount(1);

            EventDefinition eventDefinition = visitor.EventSourceDefinition.Events.First();
            eventDefinition.Name.Should().Be("Simple");
            eventDefinition.AttributeSyntax.Should().NotBeNullOrWhiteSpace();
            eventDefinition.EventId.Should().Be(1);
            eventDefinition.Message.Should().Be("Simple is called {0}.");
            eventDefinition.Level.Should().Be(EventLevel.Informational);
            eventDefinition.Version.Should().Be(2);

            eventDefinition.Arguments.Should().HaveCount(1);

            EventArgumentDefinition eventArgumentDefinition = eventDefinition.Arguments.First();
            eventArgumentDefinition.Name.Should().Be("name");
            eventArgumentDefinition.Type.Should().Be("string");
            eventArgumentDefinition.ParameterSyntax.Should().NotBeNullOrWhiteSpace();
        }

        [Fact(DisplayName = "Analyzer: Event Source with two Events")]
        public void InspectEventSourceWithMultipleEvents()
        {
            // arrange
            SyntaxTree tree = CSharpSyntaxTree.ParseText(EventSourceSamples.TwoEventsEventSource);
            SyntaxNode eventSource = tree.GetRoot();

            // act
            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(eventSource);

            // assert
            visitor.EventSourceDefinition.Should().NotBeNull();

            visitor.EventSourceDefinition.Events.Should().HaveCount(2);
            visitor.EventSourceDefinition.Events.Any(t => t.Name == "A").Should().BeTrue();
            visitor.EventSourceDefinition.Events.Any(t => t.Name == "B").Should().BeTrue();
        }


        [InlineData("[EventSourceDefinition(Name = \"a\")]", "a", null, null)]
        [InlineData("[EventSourceDefinition(Guid = \"b\")]", null, "b", null)]
        [InlineData("[EventSourceDefinition(LocalizationResources = \"c\")]", null, null, "c")]
        [InlineData("[EventSourceDefinition(Name = \"x\", Guid = \"y\", LocalizationResources = \"z\")]", "x", "y", "z")]
        [Theory(DisplayName = "Analyzer: Definition Attribute")]
        public void InspectDefinitionAttribute(string attribute, string expectedName,
            string expectedGuid, string expectedLocalizationResources)
        {
            // arrange
            string sourceCode = EventSourceSamples.SimpleEventSource
                .Replace("[EventSourceDefinition(Name = \"Mock\")]", attribute);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
            SyntaxNode eventSource = tree.GetRoot();

            // act
            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(eventSource);

            // assert
            visitor.EventSourceDefinition.Should().NotBeNull();
            visitor.EventSourceDefinition.Name.Should().Be(expectedName);
            visitor.EventSourceDefinition.Guid.Should().Be(expectedGuid);
            visitor.EventSourceDefinition.LocalizationResources.Should().Be(expectedLocalizationResources);
        }
    }
}
