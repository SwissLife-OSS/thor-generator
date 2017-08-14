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

        [Fact(DisplayName = "Analyzer: Event with multiple arguments")]
        public void InspectEventWithMultipleArguments()
        {
            // arrange
            SyntaxTree tree = CSharpSyntaxTree.ParseText(EventSourceSamples.ManyArgumentsEventSource);
            SyntaxNode eventSource = tree.GetRoot();

            // act
            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(eventSource);

            // assert
            visitor.EventSourceDefinition.Should().NotBeNull();

            visitor.EventSourceDefinition.Events.Should().HaveCount(1);

            EventDefinition eventDefinition = visitor.EventSourceDefinition.Events.First();
            eventDefinition.Arguments.Should().HaveCount(11);
            eventDefinition.Arguments[0].Type.Should().Be("string");
            eventDefinition.Arguments[1].Type.Should().Be("short");
            eventDefinition.Arguments[2].Type.Should().Be("int");
            eventDefinition.Arguments[3].Type.Should().Be("long");
            eventDefinition.Arguments[4].Type.Should().Be("ushort");
            eventDefinition.Arguments[5].Type.Should().Be("unit");
            eventDefinition.Arguments[6].Type.Should().Be("ulong");
            eventDefinition.Arguments[7].Type.Should().Be("decimal");
            eventDefinition.Arguments[8].Type.Should().Be("System.Boolean");
            eventDefinition.Arguments[9].Type.Should().Be("bool");
            eventDefinition.Arguments[10].Type.Should().Be("double");
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
