using Thor.Generator.Tasks;

namespace Thor.Generator.CLI
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
