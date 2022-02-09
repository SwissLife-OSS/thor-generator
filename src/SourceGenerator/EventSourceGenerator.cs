using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Thor.Generator.Templates;

namespace Thor.Generator;

public class EventSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<EventSourceModel> modulesAndTypes =
            context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsInterfaceWithAttribute(s),
                    transform: TryGetEventSourceFile)
                .Where(static t => t is not null)!;

        var valueProvider = context.CompilationProvider.Combine(modulesAndTypes.Collect());

        context.RegisterSourceOutput(
            valueProvider,
            static (context, source) => Execute(context, source.Item1, source.Item2));
    }

    private static bool IsInterfaceWithAttribute(SyntaxNode node)
        => node is InterfaceDeclarationSyntax m && m.AttributeLists.Count > 0;

    private static EventSourceModel? TryGetEventSourceFile(
        GeneratorSyntaxContext context,
        CancellationToken cancellationToken)
    {
        if (context.Node is InterfaceDeclarationSyntax possibleEventSource)
        {
            EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
            visitor.Visit(possibleEventSource);

            if (visitor.EventSource.IsInterface && visitor.EventSource.HasEvents)
            {
                return visitor.EventSource;
            }
        }

        return null;
    }

    private static void Execute(
        SourceProductionContext context,
        Compilation compilation,
        ImmutableArray<EventSourceModel> models)
    {
        var template = Template.FromString(
            Analyzer.Templates.DefaultTemplateInfo,
            Analyzer.Templates.DefaultTemplateBody);
        
        var templateEngine = new EventSourceTemplateEngine(template);

        foreach (var model in models)
        {
            context.AddSource(model.FileName + "g.cs", templateEngine.Generate(model));
        }
    }
}
