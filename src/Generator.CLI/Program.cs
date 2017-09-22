using System;
using System.IO;
using ChilliCream.Logging.Generator;
using ChilliCream.Tracing.Generator.Tasks;
using Newtonsoft.Json;

namespace Generator.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = JsonConvert.SerializeObject(args);
            //CreateSolutionEventSources t = new CreateSolutionEventSources();
            //t.FileOrDirectoryName = args[0];
            //t.Recursive = true;
            //t.Execute();

            //Task.Bind<CreateSolutionEventSources>()
            //    .AsDefault()
            //    .Argument(t => t.FileOrDirectoryName)
            //        .WithName("solution", 's')
            //        .Position(0)
            //    .And()
            //    .Argument(t => t.Recursive)
            //        .WithName("recursive", 'r')
            //        .Position(1);

            //Task.Bind<CreateSolutionEventSources>()
            //    .WithName("gen")
            //    .Argument(t => t.FileOrDirectoryName)
            //        .WithName("solution", 's')
            //        .Position(0)
            //    .And()
            //    .Argument(t => t.Recursive)
            //        .WithName("recursive", 'r')
            //        .Position(1);


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
