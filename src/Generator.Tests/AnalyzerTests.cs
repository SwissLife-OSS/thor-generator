using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using FluentAssertions;
using Xunit;
using Thor.Generator.Properties;
using Thor.Generator.Templates;
using System.IO;

namespace Thor.Generator
{
    public class EventSourceDefinitionVisitorTests
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
            visitor.EventSource.Should().NotBeNull();
            visitor.EventSource.InterfaceName.Should().Be("IMessageEventSource");
            visitor.EventSource.Namespace.Should().Be("EventSources");
            visitor.EventSource.Name.Should().Be("MessageEventSource");
            visitor.EventSource.FileName.Should().Be("MessageEventSource.cs");

            visitor.EventSource.Attribute.HasProperties.Should().BeTrue();
            visitor.EventSource.Attribute.Properties.Should().HaveCount(1);

            AttributePropertyModel attributeProperty = visitor.EventSource.Attribute.Properties.First();
            attributeProperty.Name.Should().Be("Name");
            attributeProperty.Value.Should().Be("Constants.MessageEventSourceName");
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
            visitor.EventSource.Should().NotBeNull();
            visitor.EventSource.InterfaceName.Should().Be("IMessageEventSource");
            visitor.EventSource.Namespace.Should().Be("EventSources");
            visitor.EventSource.Name.Should().Be("MessageEventSource");
            visitor.EventSource.FileName.Should().Be("MessageEventSource.cs");

            visitor.EventSource.Attribute.HasProperties.Should().BeTrue();
            visitor.EventSource.Attribute.Properties.Should().HaveCount(1);

            AttributePropertyModel attributeProperty = visitor.EventSource.Attribute.Properties.First();
            attributeProperty.Name.Should().Be("Name");
            attributeProperty.Value.Should().Be("\"Foo\"");
        }

        [Fact]
        public void GetEventAttribute()
        {
            // arrange
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Resources.EventSourceWithLiteral);

            // act
            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(syntaxTree.GetRoot());

            // assert
            visitor.EventSource.Events.Should().HaveCount(1);

            EventModel eventModel = visitor.EventSource.Events.First();
            eventModel.Name.Should().Be("MessageSent");
            eventModel.Id.Should().Be(1);

            eventModel.Attribute.HasProperties.Should().BeTrue();
            eventModel.Attribute.Properties.Should().HaveCount(4);

            eventModel.Attribute.Properties[0].Name.Should().BeNullOrEmpty();
            eventModel.Attribute.Properties[0].Value.Should().Be("1");
            eventModel.Attribute.Properties[1].Name.Should().Be("Level");
            eventModel.Attribute.Properties[1].Value.Should().Be("EventLevel.Verbose");
            eventModel.Attribute.Properties[2].Name.Should().Be("Message");
            eventModel.Attribute.Properties[2].Value.Should().Be("\"Sent message {correlationId}/{messageType} to {to}.\"");
            eventModel.Attribute.Properties[3].Name.Should().Be("Version");
            eventModel.Attribute.Properties[3].Value.Should().Be("1");
        }

        [Fact]
        public void GetEventParameters()
        {
            // arrange
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Resources.EventSourceWithLiteral);

            // act
            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(syntaxTree.GetRoot());

            // assert
            visitor.EventSource.Events.Should().HaveCount(1);

            EventModel eventModel = visitor.EventSource.Events.First();
            eventModel.Name.Should().Be("MessageSent");
            eventModel.Id.Should().Be(1);

            eventModel.HasParameters.Should().BeTrue();
            eventModel.InputParameters[0].Name.Should().Be("messageId");
            eventModel.InputParameters[0].Type.Should().Be("Guid");
            eventModel.InputParameters[1].Name.Should().Be("correlationId");
            eventModel.InputParameters[1].Type.Should().Be("Guid");
            eventModel.InputParameters[2].Name.Should().Be("messageType");
            eventModel.InputParameters[2].Type.Should().Be("string");
            eventModel.InputParameters[3].Name.Should().Be("from");
            eventModel.InputParameters[3].Type.Should().Be("string");
            eventModel.InputParameters[4].Name.Should().Be("to");
            eventModel.InputParameters[4].Type.Should().Be("string");
        }

        [Fact]
        public void ExtractDocumentationXmlFromInterface()
        {
            // arange
            string s = @"
                        /// <summary>
                        /// MyComment
                        /// Foo
                        /// </summary>
                        [EventSourceDefinition(Name = Constants.MessageEventSourceName)]
                        public interface IMessageEventSource
                        {
                            /// <summary>
                            /// Method
                            /// </summary>
                            [Event(1,
                            Level = EventLevel.Verbose,
                            Message = ""Sent message {correlationId}/{messageId} to {to}."",
                            Version = 1)]
                            void MessageSent(Guid messageId, Guid correlationId, string messageType, string from, string to);
                        }";
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(s);

            // act
            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(syntaxTree.GetRoot());


            // assert
            visitor.EventSource.DocumentationXml.Should().NotBeNullOrEmpty();

            StringReader sr = new StringReader(visitor.EventSource.DocumentationXml);
            sr.ReadLine().Trim().Should().Be("/// <summary>");
            sr.ReadLine().Trim().Should().Be("/// MyComment");
            sr.ReadLine().Trim().Should().Be("/// Foo");
            sr.ReadLine().Trim().Should().Be("/// </summary>");
        }

        [Fact]
        public void ExtractDocumentationXmlFromEvents()
        {
            // arange
            string s = @"
                        /// <summary>
                        /// MyComment
                        /// Foo
                        /// </summary>
                        [EventSourceDefinition(Name = Constants.MessageEventSourceName)]
                        public interface IMessageEventSource
                        {
                            /// <summary>
                            /// Method
                            /// </summary>
                            [Event(1,
                            Level = EventLevel.Verbose,
                            Message = ""Sent message {correlationId}/{messageId} to {to}."",
                            Version = 1)]
                            void MessageSent(Guid messageId, Guid correlationId, string messageType, string from, string to);
                        }";
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(s);

            // act
            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(syntaxTree.GetRoot());


            // assert
            string documentationXml = visitor.EventSource.Events.First().DocumentationXml;

            StringReader sr = new StringReader(documentationXml);
            sr.ReadLine().Trim().Should().Be("/// <summary>");
            sr.ReadLine().Trim().Should().Be("/// Method");
            sr.ReadLine().Trim().Should().Be("/// </summary>");
        }
    }
}
