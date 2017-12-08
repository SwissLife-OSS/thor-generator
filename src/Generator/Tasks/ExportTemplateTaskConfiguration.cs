using ChilliCream.FluentConsole;

namespace Thor.Generator.Tasks
{
    public class ExportTemplateTaskConfiguration
        : CommandLineTaskConfiguration
    {
        public ExportTemplateTaskConfiguration() 
        {
            Bind<ExportTemplateTask>()
                .WithName("templates", "export")
                .Argument(t => t.DirectoryName)
                .WithName("directory-name", 'd')
                .Position(0)
                .Mandatory()
                .And()
                .Argument(t => t.Language)
                .WithName("language", 'l')
                .And()
                .Argument(t => t.Name)
                .WithName("name", 'n');
        }
    }
}
