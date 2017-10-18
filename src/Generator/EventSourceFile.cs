using System;
using ChilliCream.Tracing.Generator.ProjectSystem;
using ChilliCream.Tracing.Generator.Templates;

namespace ChilliCream.Tracing.Generator
{
    internal class EventSourceFile
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
}
