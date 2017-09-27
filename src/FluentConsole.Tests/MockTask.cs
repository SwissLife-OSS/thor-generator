namespace ChilliCream.FluentConsole
{
    public class MockTask1
       : ITask
    {
        public MockTask1(IConsole console)
        {
            Console = console;
        }

        public IConsole Console { get; }

        public string FileOrDirectoryName { get; set; }
        public bool Recursive { get; set; }

        public void Execute()
        {
            if (!string.IsNullOrEmpty(FileOrDirectoryName))
            {
                Console.Write("task1: " + FileOrDirectoryName);
            }
        }
    }

    public class MockTask2
       : ITask
    {
        public MockTask2(IConsole console)
        {
            Console = console;
        }

        public IConsole Console { get; }

        public string FileOrDirectoryName { get; set; }
        public string Property1 { get; set; }
        public string Property2 { get; set; }

        public void Execute()
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
        }
    }

    public class MockTask3
        : ITask
    {
        public MockTask3(IConsole console)
        {
            Console = console;
        }

        public IConsole Console { get; }

        public void Execute()
        {
            Console.Write("task3");
        }
    }

    public class MockTask4
        : ITask
    {
        public MockTask4(IConsole console)
        {
            Console = console;
        }

        public IConsole Console { get; }

        public void Execute()
        {
            Console.Write("task4");
        }
    }
}