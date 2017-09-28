using ChilliCream.Tracing.Generator.Tasks;

namespace ChilliCream.Tracing.Generator.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            GeneratorConsoleConfiguration configuration
                = new GeneratorConsoleConfiguration();

            configuration
                .CreateRuntime()
                .Run(args);
        }
    }
}
