using ChilliCream.Tracing.Generator.EventSourceDefinitions;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChilliCream.Tracing.Generator.Tests
{
    public class EventSourceDefinitionVisitorTests
    {
        [Fact]
        public async Task InspectSimpleEventSource()
        {
            // arrange
            SyntaxTree tree = CSharpSyntaxTree.ParseText(EventSourceSamples.SimpleEventSource);
            SyntaxNode simpleEventSource = tree.GetRoot();

            // act
            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(simpleEventSource);

            // assert
            visitor.EventSourceDefinition.Should().NotBeNull();

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
    }
}
