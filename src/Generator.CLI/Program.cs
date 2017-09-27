using ChilliCream.FluentConsole;
using ChilliCream.Tracing.Generator.Tasks;

namespace Generator.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            GeneratorConsoleConfiguration consoleConfiguration 
                = new GeneratorConsoleConfiguration();

            consoleConfiguration
                .CreateRuntime()
                .Run(args);
        }
    }
}
