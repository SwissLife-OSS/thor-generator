using ChilliCream.FluentConsole;
using Thor.Generator.Tasks;

namespace Thor.Generator.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleConfiguration configuration = new ConsoleConfiguration();

            configuration.AddTaskConfiguration<SolutionEventSourceGeneratorTaskConfiguration>();
            configuration.AddTaskConfiguration<ProjectEventSourceGeneratorTaskConfiguration>();
            configuration.AddTaskConfiguration<ExportTemplateTaskConfiguration>();
            configuration.AddTaskConfiguration<ImportTemplateTaskConfiguration>();

            configuration
                .CreateRuntime()
                .Run(args);
        }
    }
}
