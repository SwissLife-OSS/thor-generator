using ChilliCream.FluentConsole;
using ChilliCream.Tracing.Generator.Tasks;

namespace Generator.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleRuntime consoleRuntime = new ConsoleRuntime();
            consoleRuntime.Bind<CreateSolutionEventSources>()
                .AsDefault()
                .Argument(t => t.FileOrDirectoryName)
                    .WithName("solution", 's')
                    .Position(0)
                .And()
                .Argument(t => t.Recursive)
                    .WithName("recursive", 'r')
                    .Position(1);

            consoleRuntime.Bind<CreateProjectEventSources>()
                .WithName("proj")
                .Argument(t => t.SourceProject)
                    .WithName("source-project", 's')
                    .Position(0)
                    .Mandatory()
                .And()
                .Argument(t => t.TargetProject)
                    .WithName("target-project", 't')
                    .Position(1);

            consoleRuntime.Run(null, args);
        }
    }
}
