namespace ChilliCream.FluentConsole
{
    public class MockTask1
       : ICommandLineTask
    {
        public MockTask1(IConsole console)
        {
            Console = console;
        }

        public IConsole Console { get; }

        public string FileOrDirectoryName { get; set; }
        public bool Recursive { get; set; }

        public int Execute()
        {
            if (!string.IsNullOrEmpty(FileOrDirectoryName))
            {
                Console.Write("task1: " + FileOrDirectoryName);
            }
            return CommandLineResults.OK;
        }
    }

    public class MockTask2
       : ICommandLineTask
    {
        public MockTask2(IConsole console)
        {
            Console = console;
        }

        public IConsole Console { get; }

        public string FileOrDirectoryName { get; set; }
        public string Property1 { get; set; }
        public string Property2 { get; set; }

        public int Execute()
        {
            if (!string.IsNullOrEmpty(FileOrDirectoryName))
            {
                Console.Write("task2: " + FileOrDirectoryName);
            }

            if (!string.IsNullOrEmpty(Property1))
            {
                Console.Write("task2: " + Property1);
            }

            if (!string.IsNullOrEmpty(Property2))
            {
                Console.Write("task2: " + Property2);
            }

            return CommandLineResults.OK;
        }
    }

    public class MockTask3
        : ICommandLineTask
    {
        public MockTask3(IConsole console)
        {
            Console = console;
        }

        public IConsole Console { get; }

        public int Execute()
        {
            Console.Write("task3");
            return CommandLineResults.OK;
        }
    }

    public class MockTask4
        : ICommandLineTask
    {
        public MockTask4(IConsole console)
        {
            Console = console;
        }

        public IConsole Console { get; }

        public int Execute()
        {
            Console.Write("task4");
            return CommandLineResults.OK;
        }
    }
}