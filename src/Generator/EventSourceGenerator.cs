using System;
using System.Collections.Generic;
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
                string eventSource = _templateEngine.Generate(eventSourceFile.Model);
                _target.UpdateDocument(eventSource, eventSourceFile.Model.FileName, eventSourceFile.Document.Folders);
            }
        }

        private IEnumerable<EventSourceFile> FindEventSourceDefinitions()
        {
            foreach (Document document in _source.Documents)
            {
                EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
                visitor.Visit(document.GetSyntaxRoot());

                if (visitor.EventSource != null)
                {
                    yield return new EventSourceFile(document, visitor.EventSource);
                }
            }
        }

        #region Nested Types

        private class EventSourceFile
        {
            public EventSourceFile(Document document, EventSourceModel model)
            {
                if (document == null)
                {
                    throw new ArgumentNullException(nameof(document));
                }

                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                Document = document;
                Model = model;
            }

            public Document Document { get; }
            public EventSourceModel Model { get; }
        }

        #endregion
    }
}
