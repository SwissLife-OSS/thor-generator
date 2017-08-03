using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChilliCream.Tracing.Generator.Tests
{
    public class FooTests
    {
        [Fact]
        public async Task Tests()
        {
            const string code = @"[EventSourceDefinition(Name = ""Mock"")] 
                                   public interface IMockEventSource 
                                    { 
[Event(1, 
            Level = EventLevel.Informational, 
            Message = ""Host { 2} created."", 
            Version = 2)]
        void Simple(string name);
        }";

            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            SyntaxNode root = tree.GetRoot();

            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(root);
        }

    }
}
