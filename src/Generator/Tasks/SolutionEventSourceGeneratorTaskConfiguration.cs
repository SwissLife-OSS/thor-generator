using ChilliCream.FluentConsole;

namespace Thor.Generator.Tasks
{
    public class SolutionEventSourceGeneratorTaskConfiguration
        : CommandLineTaskConfiguration
    {
        public SolutionEventSourceGeneratorTaskConfiguration() 
        {
            Bind<SolutionEventSourceGeneratorTask>()
                .AsDefault()
                .Argument(t => t.FileOrDirectoryName)
                .WithName("solution", 's')
                .Position(0)
                .And()
                .Argument(t => t.Recursive)
                .WithName("recursive", 'r')
                .And()
                .Argument(t => t.TemplateName)
                .WithName("template")
                .And()
                .Argument(t => t.TemplateFile)
                .WithName("template-file");
        }
    }
}