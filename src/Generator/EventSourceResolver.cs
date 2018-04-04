using System;
using System.Collections.Generic;
using Thor.Generator.ProjectSystem;
using Thor.Generator.Templates;

namespace Thor.Generator
{
    internal class EventSourceResolver
    {
        private readonly Project _project;
        private readonly Template _template;

        public EventSourceResolver(Project project, Template template)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            _project = project;
            _template = template;
        }

        public IEnumerable<EventSourceFile> FindEventSourceDefinitions()
        {
            foreach (Document document in _project.Documents)
            {
                EventSourceDefinitionVisitor visitor = new EventSourceDefinitionVisitor();
                visitor.Visit(document.GetSyntaxRoot());

                if (visitor.EventSource.HasEvents)
                {
                    yield return new EventSourceFile(document, visitor.EventSource);
                }
            }
        }
    }
}
