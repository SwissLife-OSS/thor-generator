using System;
using System.IO;
using ChilliCream.Logging.Generator;
using ChilliCream.Tracing.Generator.Tasks;

namespace Generator.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateSolutionEventSources t = new CreateSolutionEventSources();
            t.FileOrDirectoryName = args[0];
            t.Recursive = true;
            t.Execute();


            //DirectoryInfo directory = new DirectoryInfo(Environment.CurrentDirectory);

            //foreach (FileInfo file in directory.GetFiles("*.sln", SearchOption.AllDirectories))
            //{
            //    Console.WriteLine(file.FullName);
            //    EventSourceBuilder builder = new EventSourceBuilder(file.FullName);
            //    builder.BuildAsync().Wait();
            //}
        }
    }
}
