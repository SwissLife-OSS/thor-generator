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
    public class EventSourceBuilder
        : IDisposable
    {
        private readonly MSBuildWorkspace _workspace;
        private Solution _solution;

        public EventSourceBuilder(string solutionFile)
        {
            if (string.IsNullOrWhiteSpace(solutionFile))
            {
                throw new ArgumentNullException(nameof(solutionFile));
            }

            SolutionFile = solutionFile;
            _workspace = MSBuildWorkspace.Create();
        }

        public string SolutionFile { get; }

        public async Task BuildAsync()
        {
            if (_solution == null)
            {
                _solution = await _workspace.OpenSolutionAsync(SolutionFile)
                    .ConfigureAwait(false);
            }

            foreach (Project project in _solution.Projects.ToArray())
            {
                ICollection<EventSourceFile> eventSourceFiles = await GetEventSourceDefinitionsAsync(project);
                _solution = CreateEventSources(project, eventSourceFiles);
            }

            _workspace.TryApplyChanges(_solution);
        }

        private Solution CreateEventSources(Project project, IEnumerable<EventSourceFile> eventSourceFiles)
        {
            Project p = project;

            foreach (EventSourceFile eventSourceFile in eventSourceFiles)
            {
                string folderPath = string.Join("\\", eventSourceFile.Folders);
                string documentName = eventSourceFile.Definition.ClassName + ".cs";
                SourceText source = GenerateEventSource(eventSourceFile.Definition);

                Document document = p.Documents.FirstOrDefault
                    (t => string.Join("\\", t.Folders).Equals(folderPath) && t.Name == documentName);

                if (document == null)
                {
                    p = p.AddDocument(documentName, source, folders: eventSourceFile.Folders).Project;
                }
                else
                {
                    p = document.WithText(source).Project;
                }
            }

            return p.Solution;
        }

        private SourceText GenerateEventSource(EventSourceDefinition definition)
        {
            EventSourceGenerator eventSourceGenerator = new EventSourceGenerator(definition);
            string code = eventSourceGenerator.CreateEventSource();
            return SourceText.From(code);
        }



        private async Task<ICollection<EventSourceFile>> GetEventSourceDefinitionsAsync(Project project)
        {
            List<EventSourceFile> eventSourceFiles = new List<EventSourceFile>();
            foreach (Document document in project.Documents)
            {
                SyntaxNode node = await document.GetSyntaxRootAsync().ConfigureAwait(false);
                EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
                visitor.Visit(node);

                if (visitor.EventSourceDefinition != null)
                {
                    eventSourceFiles.Add(new EventSourceFile(
                        document.Folders.ToArray(), visitor.EventSourceDefinition));
                }
            }
            return eventSourceFiles;
        }

        public void Dispose()
        {
            _workspace.Dispose();
        }

        #region Nested Types

        private class EventSourceFile
        {
            public EventSourceFile(string[] folders, EventSourceDefinition definition)
            {
                if (folders == null)
                {
                    throw new ArgumentNullException(nameof(folders));
                }

                if (definition == null)
                {
                    throw new ArgumentNullException(nameof(definition));
                }

                Folders = folders;
                Definition = definition;
            }

            public string[] Folders { get; }
            public EventSourceDefinition Definition { get; }
        }

        #endregion
    }
}
