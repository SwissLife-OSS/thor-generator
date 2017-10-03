using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChilliCream.Tracing.Generator.Analyzer;
using ChilliCream.Tracing.Generator.Properties;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace ChilliCream.Tracing.Generator
{
    public class AnalyzerTests
    {

        [Fact]
        public void GetEventSourceDetailsWithNameConstant()
        {
            // arrange
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Resources.EventSourceWithConstant);

            // act
            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(syntaxTree.GetRoot());

            // assert
            //visitor.EventSourceDefinition.Should().NotBeNull();
            //visitor.EventSourceDefinition.InterfaceName.Should().Be("IMessageEventSource");
            //visitor.EventSourceDefinition.Namespace.Should().Be("EventSources");

            //visitor.EventSourceDefinition.Name.Should().Be("Constants.MessageEventSourceName");
            //visitor.EventSourceDefinition.LocalizationResources.Should().BeNull();
            //visitor.EventSourceDefinition.Guid.Should().BeNull();
        }

        [Fact]
        public void GetEventSourceDetailsWithLiteral()
        {
            // arrange
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Resources.EventSourceWithLiteral);

            // act
            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(syntaxTree.GetRoot());

            // assert
            //visitor.EventSourceDefinition.Should().NotBeNull();
            //visitor.EventSourceDefinition.InterfaceName.Should().Be("IMessageEventSource");
            //visitor.EventSourceDefinition.Namespace.Should().Be("EventSources");

            //visitor.EventSourceDefinition.Name.Should().Be("\"Foo\"");
            //visitor.EventSourceDefinition.LocalizationResources.Should().BeNull();
            //visitor.EventSourceDefinition.Guid.Should().BeNull();
        }

        [Fact]
        public void GetEventAttributes()
        {
            // arrange
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Resources.EventSourceWithLiteral);

            // act
            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(syntaxTree.GetRoot());

            // assert
            //visitor.EventSourceDefinition.Should().NotBeNull();
            //visitor.EventSourceDefinition.Events.Should().HaveCount(1);

            //EventDefinition eventDefinition = visitor.EventSourceDefinition.Events.First();
        }

    }
}
