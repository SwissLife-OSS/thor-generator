//using System;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.CSharp.Syntax;

//namespace ChilliCream.Logging.Generator
//{
//    public class EventSourceRewriter
//        : CSharpSyntaxRewriter
//    {
//        public EventSourceRewriter(EventSourceDefinition eventSourceDefinition)
//        {
//            if (eventSourceDefinition == null)
//            {
//                throw new ArgumentNullException(nameof(eventSourceDefinition));
//            }

//            EventSourceDefinition = eventSourceDefinition;
//        }

//        public EventSourceDefinition EventSourceDefinition { get; set; }

//        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
//        {            
//            return base.VisitClassDeclaration(node.WithIdentifier(SyntaxToken("")));
//        }

//    }
//}
