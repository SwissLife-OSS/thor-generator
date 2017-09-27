using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace ChilliCream.FluentConsole
{
    public class ConsoleRuntimeTests
    {

        [InlineData("[\"a\", \"-f\", \"test1\"]", "task1: test1")]
        [InlineData("[\"a\", \"-f\", \"test2\", \"-r\"]", "task1: test2")]
        [InlineData("[\"a\", \"--property2\", \"prop2\"]", "task2: prop2")]
        [InlineData("[\"a\", \"b\"]", "task3")]
        [InlineData("[\"a\", \"b\", \"c\"]", "task4")]
        [Theory]
        public void RunTask(string arguments, string expectedResult)
        {
            // arrange
            MockConsole mockConsole = new MockConsole();
            ConsoleRuntime consoleRuntime = new ConsoleRuntime();
            BindTasks(consoleRuntime);

            string[] args = JsonConvert.DeserializeObject<string[]>(arguments);

            // act
            consoleRuntime.Run(mockConsole, args);

            // assert
            mockConsole.Messages.Should().HaveCount(1);
            mockConsole.Messages[0].Should().Be(expectedResult);
        }

        private void BindTasks(ConsoleRuntime consoleRuntime)
        {
            consoleRuntime.Bind<MockTask1>()
                .WithName("a")
                .Argument(t => t.FileOrDirectoryName)
                .WithName("file", 'f')
                .And()
                .Argument(t => t.Recursive)
                .WithName("recursive", 'r');

            consoleRuntime.Bind<MockTask2>()
                .WithName("a")
                .Argument(t => t.FileOrDirectoryName)
                .WithName("file", 'f')
                .And()
                .Argument(t => t.Property1)
                .WithName("property1", 'x')
                .Mandatory()
                .And()
                .Argument(t => t.Property2)
                .WithName("property2", 'y');

            consoleRuntime.Bind<MockTask2>()
                .WithName("a")
                .Argument(t => t.FileOrDirectoryName)
                .WithName("file", 'f')
                .And()
                .Argument(t => t.Property1)
                .WithName("property1", 'x')
                .And()
                .Argument(t => t.Property2)
                .WithName("property2", 'y')
                .Mandatory();

            consoleRuntime.Bind<MockTask3>()
                .WithName("a", "b");

            consoleRuntime.Bind<MockTask4>()
                .WithName("a", "b", "c");
        }
    }
}
