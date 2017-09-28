using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;

namespace ChilliCream.FluentConsole
{
    public class BindTaskTests
    {
        [Fact]
        public void ConstructorArgumentValidation()
        {
            // arrange
            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();

            // act
            Action a = () => new BindTask<MockTask1>(taskDefinitions);
            Action b = () => new BindTask<MockTask1>(null);

            // assert
            a.ShouldNotThrow();
            b.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void AsDefault()
        {
            // arrange
            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();
            BindTask<MockTask1> bindTask = new BindTask<MockTask1>(taskDefinitions);

            // act
            bindTask.AsDefault();

            // assert
            taskDefinitions.Should().HaveCount(1);
            taskDefinitions[0].IsDefault.Should().BeTrue();
        }

        [Fact]
        public void AsDefaultWithName()
        {
            // arrange
            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();
            BindTask<MockTask1> bindTask = new BindTask<MockTask1>(taskDefinitions);
            string name = Guid.NewGuid().ToString();

            // act
            bindTask.AsDefault().WithName(name);

            // assert
            taskDefinitions.Should().HaveCount(1);
            taskDefinitions[0].IsDefault.Should().BeTrue();
            taskDefinitions[0].Name.First().Should().Be(name);
        }       

        [Fact]
        public void WithName()
        {
            // arrange
            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();
            BindTask<MockTask1> bindTask = new BindTask<MockTask1>(taskDefinitions);
            string name = Guid.NewGuid().ToString();

            // act
            bindTask.WithName(name);

            // assert
            taskDefinitions.Should().HaveCount(1);
            taskDefinitions[0].Name.First().Should().Be(name);
        }

        [Fact]
        public void WithNameAsDefault()
        {
            // arrange
            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();
            BindTask<MockTask1> bindTask = new BindTask<MockTask1>(taskDefinitions);
            string name = Guid.NewGuid().ToString();

            // act
            bindTask.WithName(name).AsDefault();

            // assert
            taskDefinitions.Should().HaveCount(1);
            taskDefinitions[0].IsDefault.Should().BeTrue();
            taskDefinitions[0].Name.First().Should().Be(name);
        }

        [Fact]
        public void WithNameArgumentValidation()
        {
            // arrange
            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();
            BindTask<MockTask1> bindTask = new BindTask<MockTask1>(taskDefinitions);

            // act
            Action a = () => bindTask.WithName((string)null);

            // assert
            a.ShouldThrow<ArgumentNullException>();
        }
    }
}
