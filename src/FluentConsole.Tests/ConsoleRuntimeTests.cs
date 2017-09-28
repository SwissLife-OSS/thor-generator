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
            MockConsole console = new MockConsole();
            ConsoleRuntime consoleRuntime = CreateRuntime(console);
            string[] args = JsonConvert.DeserializeObject<string[]>(arguments);

            // act
            consoleRuntime.Run(args);

            // assert
            console.Messages.Should().HaveCount(1);
            console.Messages[0].Should().Be(expectedResult);
        }

        private ConsoleRuntime CreateRuntime(IConsole console)
        {
            ConsoleConfiguration configuration = new ConsoleConfiguration();

            configuration.Bind<MockTask1>()
                .WithName("a")
                .Argument(t => t.FileOrDirectoryName)
                .WithName("file", 'f')
                .And()
                .Argument(t => t.Recursive)
                .WithName("recursive", 'r');

            configuration.Bind<MockTask2>()
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

            configuration.Bind<MockTask2>()
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

            configuration.Bind<MockTask3>()
                .WithName("a", "b");

            configuration.Bind<MockTask4>()
                .WithName("a", "b", "c");

            return configuration.CreateRuntime(console);
        }
    }
}
