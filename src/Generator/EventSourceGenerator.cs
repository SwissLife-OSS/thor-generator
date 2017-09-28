using System;
using System.Collections.Generic;
using System.Linq;
using ChilliCream.Logging.Generator;
using ChilliCream.Tracing.Generator.Analyzer;
using ChilliCream.Tracing.Generator.ProjectSystem;
using ChilliCream.Tracing.Generator.Templates;

namespace ChilliCream.Tracing.Generator
{
    public class EventSourceGenerator
    {
        private readonly Project _source;
        private readonly Project _target;
        private readonly EventSourceTemplateEngine _templateEngine;

        public EventSourceGenerator(Project source, Project target, Template template)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            _source = source;
            _target = target;
            _templateEngine = new EventSourceTemplateEngine(template);
        }

        public void Generate()
        {
            foreach (EventSourceFile eventSourceFile in FindEventSourceDefinitions())
            {
                string newName = eventSourceFile.Document.Name.StartsWith("I")
                    ? eventSourceFile.Document.Name.Substring(1)
                    : string.Concat(eventSourceFile.Document.Name, "Impl");
                string eventSource = _templateEngine.Generate(eventSourceFile.Definition);
                _target.UpdateDocument(eventSource, newName, eventSourceFile.Document.Folders);
            }
        }

        private IEnumerable<EventSourceFile> FindEventSourceDefinitions()
        {
            foreach (Document document in _source.Documents)
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
