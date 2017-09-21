//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ChilliCream.Logging.Generator.Analyzer;
//using ChilliCream.Tracing.Generator.ProjectSystem;

//namespace ChilliCream.Logging.Generator
//{
//    public class EventSourceBuilder
//    {
//        private Solution _solution;

//        public EventSourceBuilder(string solutionFile)
//        {
//            if (string.IsNullOrWhiteSpace(solutionFile))
//            {
//                throw new ArgumentNullException(nameof(solutionFile));
//            }

//            SolutionFile = solutionFile;
//            _solution = Solution.Create(solutionFile);
//        }

//        public string SolutionFile { get; }

//        public void Build()
//        {
//            foreach (Project project in _solution.Projects)
//            {
                


//            }

//            _solution.CommitChanges();
//        }

//        private Solution CreateEventSources(Project project, IEnumerable<EventSourceFile> eventSourceFiles)
//        {
//            Project p = project;

//            foreach (EventSourceFile eventSourceFile in eventSourceFiles)
//            {
//                string folderPath = string.Join("\\", eventSourceFile.Folders);
//                string documentName = eventSourceFile.Definition.ClassName + ".cs";
//                SourceText source = GenerateEventSource(eventSourceFile.Definition);

//                Document document = p.Documents.FirstOrDefault
//                    (t => string.Join("\\", t.Folders).Equals(folderPath) && t.Name == documentName);

//                if (document == null)
//                {
//                    p = p.AddDocument(documentName, source, folders: eventSourceFile.Folders).Project;
//                }
//                else
//                {
//                    p = document.WithText(source).Project;
//                }
//            }

//            return p.Solution;
//        }

//        private string GenerateEventSource(EventSourceDefinition definition)
//        {
//            EventSourceGenerator eventSourceGenerator = new EventSourceGenerator(definition);
//            return eventSourceGenerator.CreateEventSource();
//        }

//        private ICollection<EventSourceFile> GetEventSourceDefinitions(Project project)
//        {
//            List<EventSourceFile> eventSourceFiles = new List<EventSourceFile>();
//            foreach (Document document in project.Documents)
//            {
//                EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
//                visitor.Visit(document.GetSyntaxRoot());

//                if (visitor.EventSourceDefinition != null)
//                {
//                    eventSourceFiles.Add(new EventSourceFile(
//                        document.Folders.ToArray(), visitor.EventSourceDefinition));
//                }
//            }
//            return eventSourceFiles;
//        }

//        #region Nested Types

//        private class EventSourceFile
//        {
//            public EventSourceFile(string[] folders, EventSourceDefinition definition)
//            {
//                if (folders == null)
//                {
//                    throw new ArgumentNullException(nameof(folders));
//                }

//                if (definition == null)
//                {
//                    throw new ArgumentNullException(nameof(definition));
//                }

//                Folders = folders;
//                Definition = definition;
//            }

//            public EventSourceDefinition Definition { get; }
//            public Document DefinitionDocument { get; set; }
//        }

//        #endregion
//    }
//}
