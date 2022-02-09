using System;
using Nustache.Core;

namespace Thor.Generator.Templates
{
    /// <summary>
    /// The template engine can generate event sources from an <see cref="EventSourceModel"/>.
    /// </summary>
    internal class EventSourceTemplateEngine
    {
        private static readonly RenderContextBehaviour _renderContextBehaviour =
            new RenderContextBehaviour
            {
                HtmlEncoder = t => t
            };

        private readonly string _template;
        private readonly EventSourceModelPostProcessor _eventSourceModelPostProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSourceTemplateEngine"/> class.
        /// </summary>
        /// <param name="template">
        /// The event source template.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="template"/> is <c>null</c>.
        /// </exception>
        public EventSourceTemplateEngine(Template template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            _template = template.Code;
            _eventSourceModelPostProcessor = new EventSourceModelPostProcessor(template);
        }

        /// <summary>
        /// Generates the specified event source.
        /// </summary>
        /// <param name="eventSourceModel">The event source model.</param>
        /// <returns>
        /// Returns a <see cref="string"/> representing the generated event source.
        /// </returns>
        public string Generate(EventSourceModel eventSourceModel)
        {
            if (eventSourceModel == null)
            {
                throw new ArgumentNullException(nameof(eventSourceModel));
            }

            _eventSourceModelPostProcessor.Process(eventSourceModel);

            // generate event source
            return Render.StringToString(_template, eventSourceModel,
                renderContextBehaviour: _renderContextBehaviour);
        }
    }
}
