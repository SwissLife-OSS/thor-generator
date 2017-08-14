using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using ChilliCream.Logging.Generator.EventSourceDefinitions;
using ChilliCream.Logging.Generator.Analyzer;


namespace ChilliCream.Logging.Generator
{
    public class EventSourceGeneratorTests
    {
        [Fact]
        public void GenerateEventSource()
        {
            // arrange
            SyntaxTree tree = CSharpSyntaxTree.ParseText(EventSourceSamples.SimpleEventSource);
            SyntaxNode simpleEventSource = tree.GetRoot();

            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(simpleEventSource);

            // act
            EventSourceGenerator generator = new EventSourceGenerator(visitor.EventSourceDefinition);
            string eventSourceCode = generator.CreateEventSource();

            // assert

        }
    }
}
