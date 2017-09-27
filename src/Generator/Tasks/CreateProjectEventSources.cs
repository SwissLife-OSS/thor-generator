using System;
using ChilliCream.FluentConsole;
using ChilliCream.Tracing.Generator.ProjectSystem;

namespace ChilliCream.Tracing.Generator.Tasks
{
    public sealed class CreateProjectEventSources
        : ITask
    {
        private string _templateDirectory;

        public CreateProjectEventSources()
            : this(ConsoleEnvironment.TemplateDirectory)
        { }

        public CreateProjectEventSources(string templateDirectory)
        {
            if (string.IsNullOrEmpty(templateDirectory))
            {
                throw new ArgumentNullException(nameof(templateDirectory));
            }
            _templateDirectory = templateDirectory;
        }


        public string SourceProject { get; set; }
        public string TargetProject { get; set; }
        public Language Language { get; set; } = Language.CSharp;
        public string TemplateName { get; set; }

        public void Execute()
        {
            if (string.IsNullOrEmpty(TargetProject))
            {
                TargetProject = SourceProject;
            }

            if (Project.TryParse(SourceProject, out Project source)
                && Project.TryParse(TargetProject, out Project target))
            {
                string templateContent = TemplateResolver.Resolve(_templateDirectory, TemplateName, Language);
                EventSourceBuilder eventSourceBuilder = new EventSourceBuilder(source, target, templateContent);
                eventSourceBuilder.Build();

                target.CommitChanges();
            }
        }
    }
}
