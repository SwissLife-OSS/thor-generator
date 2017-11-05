using System;
using Thor.Generator.ProjectSystem;
using Thor.Generator.Templates;

namespace Thor.Generator
{
    public class EventSourceGenerator
    {
        private readonly Project _source;
        private readonly Project _target;
        private readonly EventSourceResolver _eventSourceResolver;
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
            _eventSourceResolver = new EventSourceResolver(source, template);
            _templateEngine = new EventSourceTemplateEngine(template);
        }

        public void Generate()
        {
            foreach (EventSourceFile eventSourceFile in _eventSourceResolver.FindEventSourceDefinitions())
            {
                string eventSource = _templateEngine.Generate(eventSourceFile.Model);
                _target.UpdateDocument(eventSource, eventSourceFile.Model.FileName, eventSourceFile.Document.Folders);
            }
        }
    }
}
