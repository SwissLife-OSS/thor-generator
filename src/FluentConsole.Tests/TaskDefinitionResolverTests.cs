using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace ChilliCream.FluentConsole
{
    public class TaskDefinitionResolverTests
    {

        [InlineData("[\"a\", \"-f test1\"]", 3, 1)]
        [InlineData("[\"a\", \"-f test2\", \"-r\"]", 3, 1)]
        [InlineData("[\"a\", \"-f\", \"--property1 prop1\"]", 3, 1)]
        [InlineData("[\"a\", \"--property2 prop2\"]", 3, 1)]
        [InlineData("[\"a\", \"b\"]", 1, 2)]
        [InlineData("[\"a\", \"b\", \"c\"]", 1, 3)]
        [Theory]
        public void TaskVariants(string args, int expectedTaskCount, int expectedArgumentCount)
        {
            // arrange
            IList<TaskDefinition> taskDefinitions = CreateTaskDefinitions();
            Argument[] arguments = ArgumentParser
                .Parse(JsonConvert.DeserializeObject<string[]>(args))
                .ToArray();

            // act
            ResolvedTaskDefinitions resolvedTaskDefinitions;
            TaskDefinitionResolver resolver = new TaskDefinitionResolver(taskDefinitions);
            bool success = resolver.TryResolve(arguments, out resolvedTaskDefinitions);

            // assert
            success.Should().BeTrue();
            resolvedTaskDefinitions.TaskDefinitions.Should().HaveCount(expectedTaskCount);
            resolvedTaskDefinitions.ArgumentCount.Should().Be(expectedArgumentCount);
        }

        private IList<TaskDefinition> CreateTaskDefinitions()
        {
            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();

            new BindTask<MockTask1>(taskDefinitions)
                .WithName("a")
                .Argument(t => t.FileOrDirectoryName)
                .WithName("file", 'f')
                .And()
                .Argument(t => t.Recursive)
                .WithName("recursive", 'r');

            new BindTask<MockTask2>(taskDefinitions)
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

            new BindTask<MockTask2>(taskDefinitions)
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

            new BindTask<MockTask3>(taskDefinitions)
                .WithName("a", "b");

            new BindTask<MockTask4>(taskDefinitions)
                .WithName("a", "b", "c");

            return taskDefinitions;
        }
    }
}
