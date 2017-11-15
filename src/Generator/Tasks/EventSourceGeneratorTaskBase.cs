using System;
using ChilliCream.FluentConsole;
using Thor.Generator.ProjectSystem;
using Thor.Generator.Templates;

namespace Thor.Generator.Tasks
{
    /// <summary>
    /// This class provides the basic functionality to retireve 
    /// templates and generate event sources from event source interfaces.
    /// </summary>
    public class EventSourceGeneratorTaskBase
    {
        private readonly TemplateStorage _templateStorage;
        private readonly IConsole _console;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSourceGeneratorTaskBase"/> class.
        /// </summary>
        /// <param name="console">The console interface.</param>
        /// <param name="templateStorage">The template storage.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="console"/> is <c>null</c>.
        /// or
        /// <paramref name="templateStorage"/> is <c>null</c>.
        /// </exception>
        protected EventSourceGeneratorTaskBase(IConsole console, TemplateStorage templateStorage)
        {
            if (console == null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (templateStorage == null)
            {
                throw new ArgumentNullException(nameof(templateStorage));
            }

            _console = console;
            _templateStorage = templateStorage;
        }

        /// <summary>
        /// This property provides access to the console interface.
        /// </summary>
        /// <value>The console interface.</value>
        protected IConsole Console => _console;

        /// <summary>
        /// Gets or sets the language of the default template 
        /// which will be used to generate event sources if 
        /// no custom template was provided.
        /// </summary>
        /// <value>The default template language.</value>
        public Language Language { get; set; } = Language.CSharp;

        /// <summary>
        /// Gets or sets the name of a custom 
        /// template that shall be used to generate event sources.
        /// </summary>
        /// <value>The name of the template.</value>
        public string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets a custom template file 
        /// that shall be used to generate event sources.
        /// </summary>
        public string TemplateFile { get; set; }

        /// <summary>
        /// Analyzes the given <paramref name="project"/> and 
        /// generates the event source into the same project. 
        /// </summary>
        /// <param name="project">The project.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="project"/> is <c>null</c>.
        /// </exception>
        protected void GenerateEventSources(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            GenerateEventSources(project, project);
        }

        /// <summary>
        /// Analyzes the given <paramref name="source"/>-project and 
        /// generates the event sources into the <paramref name="target"/>-project.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <c>null</c>
        /// or
        /// <paramref name="target"/> is <c>null</c>.
        /// </exception>
        protected void GenerateEventSources(Project source, Project target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (source == target)
            {
                _console.WriteLine($"Processing {source.Id} ...");
            }
            else
            {
                _console.WriteLine($"Processing {source.Id} -> {target.Id}  ...");
            }

            EventSourceGenerator eventSourceBuilder =
                new EventSourceGenerator(source, target, GetTemplate());
            eventSourceBuilder.Generate();
        }

        /// <summary>
        /// Gets the template that shall be used to generate event sources.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing a mustache template.
        /// </returns>
        protected Template GetTemplate()
        {
            if(!string.IsNullOrEmpty(TemplateFile))
            {
                return Template.FromFile(TemplateFile);
            }

            if (!string.IsNullOrEmpty(TemplateName))
            {
                return _templateStorage.GetCustomTemplate(TemplateName);
            }

            return _templateStorage.GetTemplate(Language);
        }
    }
}
