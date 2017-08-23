using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ChilliCream.Logging.Generator
{
    public class FormatMethodsRewriter
        : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            return base.VisitMethodDeclaration(node.WithLeadingTrivia().WithTrailingTrivia().NormalizeWhitespace());
        }
    }
}