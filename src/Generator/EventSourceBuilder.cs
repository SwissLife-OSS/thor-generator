using System;
using System.Collections.Generic;
using System.Linq;
using ChilliCream.Logging.Generator;
using ChilliCream.Tracing.Generator.Analyzer;
using ChilliCream.Tracing.Generator.ProjectSystem;

namespace ChilliCream.Tracing.Generator
{
    public class EventSourceBuilder
    {
        public EventSourceBuilder(Project source, Project target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            Source = source;
            Target = target;
        }

        public Project Source { get; }
        public Project Target { get; }

        public void Build()
        {
            foreach (EventSourceFile eventSourceFile in FindEventSourceDefinitions())
            {
                EventSourceGenerator generator = new EventSourceGenerator(eventSourceFile.Definition);

                string newName = eventSourceFile.Document.Name.StartsWith("I")
                    ? eventSourceFile.Document.Name.Substring(1)
                    : string.Concat(eventSourceFile.Document.Name, "Impl");

                Target.UpdateDocument(generator.CreateEventSource(),
                    newName, eventSourceFile.Document.Folders);
            }
        }

        private IEnumerable<EventSourceFile> FindEventSourceDefinitions()
        {
            foreach (Document document in Source.Documents)
            {
                EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
                visitor.Visit(document.GetSyntaxRoot());

                if (visitor.EventSourceDefinition != null)
                {
                    yield return new EventSourceFile(document, visitor.EventSourceDefinition);
                }
            }
        }

        #region Nested Types

        private class EventSourceFile
        {
            public EventSourceFile(Document document, EventSourceDefinition definition)
            {
                if (document == null)
                {
                    throw new ArgumentNullException(nameof(document));
                }

                if (definition == null)
                {
                    throw new ArgumentNullException(nameof(definition));
                }

                Document = document;
                Definition = definition;
            }

            public Document Document { get; }
            public EventSourceDefinition Definition { get; }
        }

        #endregion
    }
}
