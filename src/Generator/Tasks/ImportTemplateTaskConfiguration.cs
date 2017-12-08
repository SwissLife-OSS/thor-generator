using ChilliCream.FluentConsole;

namespace Thor.Generator.Tasks
{
    public class ImportTemplateTaskConfiguration
        : CommandLineTaskConfiguration
    {
        public ImportTemplateTaskConfiguration() 
        {
            Bind<ImportTemplateTask>()
                .WithName("templates", "import")
                .Argument(t => t.FileName)
                .WithName("file-name", 'f')
                .Position(0)
                .Mandatory()
                .And()
                .Argument(t => t.Name)
                .WithName("name", 'n');
        }
    }
}
