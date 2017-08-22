using System;
using ChilliCream.Logging.Generator;

namespace Generator.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Class1 c = new Class1(@"C:\Work\EventSourceDemo\EventSourceDemo\EventSourceDemo.sln");
            c.Foo().Wait();
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
