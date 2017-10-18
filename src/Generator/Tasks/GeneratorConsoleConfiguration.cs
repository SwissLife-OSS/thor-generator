using ChilliCream.FluentConsole;

namespace ChilliCream.Tracing.Generator.Tasks
{
    /// <summary>
    /// This configuration provides command line tasks for a event source generator cli.
    /// </summary>
    /// <seealso cref="ChilliCream.FluentConsole.ConsoleConfiguration" />
    public class GeneratorConsoleConfiguration
        : ConsoleConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorConsoleConfiguration"/> class.
        /// </summary>
        public GeneratorConsoleConfiguration()
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
                .WithName("template");

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
                .WithName("template");

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
