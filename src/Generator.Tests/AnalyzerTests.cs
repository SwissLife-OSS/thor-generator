using System;
using System.Collections.Generic;
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
        public void Analyze()
        {
            // arrange
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Resources.EventSourceWithConstant);

            // act
            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(syntaxTree.GetRoot());

            // assert
            visitor.EventSourceDefinition.Should().NotBeNull();

        }

    }
}
