using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChilliCream.Logging.Generator.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ChilliCream.Logging.Generator
{
    public class Class1
        : IDisposable
    {
        private readonly MSBuildWorkspace _workspace;
        private Solution _solution;

        public Class1(string solutionFile)
        {
            if (string.IsNullOrWhiteSpace(solutionFile))
            {
                throw new ArgumentNullException(nameof(solutionFile));
            }

            SolutionFile = solutionFile;
            _workspace = MSBuildWorkspace.Create();
        }

        public string SolutionFile { get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task Foo()
        {
            if (_solution == null)
            {
                _solution = await _workspace.OpenSolutionAsync(SolutionFile)
                    .ConfigureAwait(false);
            }


            int projectIndex = 0;
            while (_solution.ProjectIds.Count > projectIndex)
            {
                int documentIndex = 0;
                Project project = _solution.Projects.Skip(projectIndex).First();
                Project updatedProject = project;
                while (project.DocumentIds.Count > documentIndex)
                {
                    Document document = project.Documents.Skip(documentIndex).First();
                    SyntaxNode node = await document.GetSyntaxRootAsync();

                    EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
                    visitor.Visit(node);

                    if (visitor.EventSourceDefinition != null)
                    {
                        EventSourceGenerator eventSourceGenerator
                            = new EventSourceGenerator(visitor.EventSourceDefinition);
                        string code = eventSourceGenerator.CreateEventSource();
                        SourceText sourceText = SourceText.From(code);

                        updatedProject = updatedProject.AddDocument(visitor.EventSourceDefinition.ClassName + ".cs", sourceText).Project;
                    }
                    documentIndex++;
                }
                projectIndex++;
                _workspace.TryApplyChanges(updatedProject.Solution);
            }



        }

    }
}
