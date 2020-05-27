using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Host;
using Snapshooter.Xunit;
using Thor.Analyzer;
using Thor.Generator.Properties;
using Thor.Generator.Templates;
using Xunit;

namespace Thor.Generator
{
    public class CompilationTests
    {
        [Fact]
        public void AnalyzeCsharpComplexEventSource()
        {
            // arrange
            TemplateStorage templateStorage = new TemplateStorage();
            Template template = templateStorage.GetCustomTemplate(Path.Combine("Defaults", "CSharp"));

            EventSourceDefinitionVisitor eventSourceDefinitionVisitor = new EventSourceDefinitionVisitor();
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Resources.EventSourceWithComplexTypeThatBuilds);
            eventSourceDefinitionVisitor.Visit(syntaxTree.GetRoot());


            // act
            EventSourceTemplateEngine templateEngine = new EventSourceTemplateEngine(template);
            string eventSourceCode = templateEngine.Generate(eventSourceDefinitionVisitor.EventSource);

            // assert
            eventSourceCode.MatchSnapshot();

            Report report = EventSourceReportBuilder.New()
                .AddSourceCode(Resources.EventSourceWithComplexTypeThatBuilds)
                .AddSourceCode(eventSourceCode)
                .Build("EventSources.FooEventSource");

            Assert.NotNull(report);
            Assert.False(report.HasErrors);
        }

        [Fact]
        public void AnalyzeCsharpSimpleEventSource()
        {
            // arrange
            TemplateStorage templateStorage = new TemplateStorage();
            Template template = templateStorage.GetCustomTemplate(Path.Combine("Defaults", "CSharp"));

            EventSourceDefinitionVisitor eventSourceDefinitionVisitor = new EventSourceDefinitionVisitor();
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Resources.EventSourceThatBuilds);
            eventSourceDefinitionVisitor.Visit(syntaxTree.GetRoot());

            EventSourceReportBuilder analyzer = new EventSourceReportBuilder();

            // act
            EventSourceTemplateEngine templateEngine = new EventSourceTemplateEngine(template);
            string eventSourceCode = templateEngine.Generate(eventSourceDefinitionVisitor.EventSource);

            // assert
            eventSourceCode.MatchSnapshot();

            Report report = EventSourceReportBuilder.New()
                .AddSourceCode(Resources.EventSourceThatBuilds)
                .AddSourceCode(eventSourceCode)
                .Build("EventSources.BarEventSource");

            Assert.NotNull(report);
            Assert.False(report.HasErrors);
        }
    }

    public class EventSourceReportBuilder
    {
        private readonly CSharpLanguage _sourceLanguage = new CSharpLanguage();
        private List<MetadataReference> _references = new List<MetadataReference>();
        private List<SyntaxTree> _sourceTrees = new List<SyntaxTree>();

        public static EventSourceReportBuilder New() =>
            new EventSourceReportBuilder();

        public EventSourceReportBuilder()
        {
            _references.AddRange(GetBaseAssemblies());
        }

        public EventSourceReportBuilder AddReferences(ICollection<PortableExecutableReference> references)
        {
            _references.AddRange(references);
            return this;
        }

        public EventSourceReportBuilder AddSourceCode(string source)
        {
            _sourceTrees.Add(_sourceLanguage.ParseText(source, SourceCodeKind.Regular));
            return this;
        }

        public Report Build(string className)
        {
            Assembly assembly = CreateAssemblyDefinition();

            Type type = assembly.GetType(className);
            EventSource eventSource = (EventSource)type.GetTypeInfo()
                .DeclaredConstructors
                .First(t => !t.GetParameters().Any())
                    .Invoke(Array.Empty<object>());
            return Analyze(eventSource);
        }

        private Report Analyze(EventSource eventSource)
        {
            EventSourceAnalyzer analyzer = new EventSourceAnalyzer();
            return analyzer.Inspect(eventSource);
        }

        private Assembly CreateAssemblyDefinition()
        {
            Compilation compilation = _sourceLanguage
                .CreateLibraryCompilation(
                    assemblyName: "InMemoryAssembly",
                    enableOptimisations: false)
                .AddReferences(_references)
                .AddSyntaxTrees(_sourceTrees);

            MemoryStream stream = new MemoryStream();
            EmitResult emitResult = compilation.Emit(stream);

            if (emitResult.Success)
            {
                stream.Seek(0, SeekOrigin.Begin);

                using (stream)
                {
                    byte[] data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                    return Assembly.Load(data);
                }
            }

            return null;
        }

        private List<PortableExecutableReference> GetBaseAssemblies()
        {
            string[] trustedAssembliesPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator);
            List<PortableExecutableReference> references = trustedAssembliesPaths
                .Select(p => MetadataReference.CreateFromFile(p))
                .ToList();

            references.AddRange(new[]
            {
                MetadataReference.CreateFromFile(typeof(EventSource).GetTypeInfo().Assembly.Location),
            });

            return references;
        }
    }

    public class CSharpLanguage : ILanguageService
    {
        private readonly IReadOnlyCollection<MetadataReference> _references = new[]
        {
            MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ValueTuple<>).GetTypeInfo().Assembly.Location)
        };

        private readonly LanguageVersion _maxLanguageVersion = Enum
            .GetValues(typeof(LanguageVersion))
            .Cast<LanguageVersion>()
            .Max();

        public SyntaxTree ParseText(string sourceCode, SourceCodeKind kind)
        {
            CSharpParseOptions options = new CSharpParseOptions(kind: kind, languageVersion: _maxLanguageVersion);

            // Return a syntax tree of our source code
            return CSharpSyntaxTree.ParseText(sourceCode, options);
        }


        public Compilation CreateLibraryCompilation(string assemblyName, bool enableOptimisations)
        {
            CSharpCompilationOptions options = new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: enableOptimisations ? OptimizationLevel.Release : OptimizationLevel.Debug,
                allowUnsafe: true);

            return CSharpCompilation.Create(assemblyName, options: options, references: _references);
        }
    }
}
