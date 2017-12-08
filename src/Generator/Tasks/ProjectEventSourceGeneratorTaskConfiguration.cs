using ChilliCream.FluentConsole;

namespace Thor.Generator.Tasks
{
    public class ProjectEventSourceGeneratorTaskConfiguration
        : CommandLineTaskConfiguration
    {
        public ProjectEventSourceGeneratorTaskConfiguration() 
        {
            Bind<ProjectEventSourceGeneratorTask>()
                .AsDefault()
                .WithName("project")
                .Argument(t => t.SourceProject)
                .WithName("source", 's')
                .Position(0)
                .Mandatory()
                .And()
                .Argument(t => t.TargetProject)
                .WithName("target", 't')
                .Position(1)
                .And()
                .Argument(t => t.TemplateName)
                .WithName("template")
                .And()
                .Argument(t => t.TemplateFile)
                .WithName("template-file");
        }
    }
}
