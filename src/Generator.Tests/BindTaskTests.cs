using System;
using System.Collections.Generic;
using System.Text;
using ChilliCream.Tracing.Generator.Tasks;
using FluentAssertions;
using Xunit;

namespace ChilliCream.Tracing.Generator
{
    public class BindTaskTests
    {
        [Fact]
        public void ConstructorArgumentValidation()
        {
            // arrange
            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();

            // act
            Action a = () => new BindTask<CreateSolutionEventSources>(taskDefinitions);
            Action b = () => new BindTask<CreateSolutionEventSources>(null);

            // assert
            a.ShouldNotThrow();
            b.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void AsDefault()
        {
            // arrange
            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();
            BindTask<CreateSolutionEventSources> bindTask = new BindTask<CreateSolutionEventSources>(taskDefinitions);

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
            BindTask<CreateSolutionEventSources> bindTask = new BindTask<CreateSolutionEventSources>(taskDefinitions);
            string name = Guid.NewGuid().ToString();

            // act
            bindTask.AsDefault().WithName(name);

            // assert
            taskDefinitions.Should().HaveCount(1);
            taskDefinitions[0].IsDefault.Should().BeTrue();
            taskDefinitions[0].Name.Should().Be(name);
        }

        [Fact]
        public void AsDefaultDuplicate()
        {
            // arrange
            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();
            new BindTask<CreateSolutionEventSources>(taskDefinitions).AsDefault();
            BindTask<CreateSolutionEventSources> bindTask = new BindTask<CreateSolutionEventSources>(taskDefinitions);

            // act
            Action a = () => bindTask.AsDefault();
            Action b = () => bindTask.WithName("foo").AsDefault();

            // assert
            a.ShouldThrow<InvalidOperationException>();
            b.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void WithName()
        {
            // arrange
            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();
            BindTask<CreateSolutionEventSources> bindTask = new BindTask<CreateSolutionEventSources>(taskDefinitions);
            string name = Guid.NewGuid().ToString();

            // act
            bindTask.WithName(name);

            // assert
            taskDefinitions.Should().HaveCount(1);
            taskDefinitions[0].Name.Should().Be(name);
        }

        [Fact]
        public void WithNameAsDefault()
        {
            // arrange
            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();
            BindTask<CreateSolutionEventSources> bindTask = new BindTask<CreateSolutionEventSources>(taskDefinitions);
            string name = Guid.NewGuid().ToString();

            // act
            bindTask.WithName(name).AsDefault();

            // assert
            taskDefinitions.Should().HaveCount(1);
            taskDefinitions[0].IsDefault.Should().BeTrue();
            taskDefinitions[0].Name.Should().Be(name);
        }

        [Fact]
        public void WithNameArgumentValidation()
        {
            // arrange
            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();
            BindTask<CreateSolutionEventSources> bindTask = new BindTask<CreateSolutionEventSources>(taskDefinitions);

            // act
            Action a = () => bindTask.WithName(null);

            // assert
            a.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void WithNameDuplicateName()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();
            new BindTask<CreateSolutionEventSources>(taskDefinitions).WithName(name);
            BindTask<CreateSolutionEventSources> bindTask = new BindTask<CreateSolutionEventSources>(taskDefinitions);

            // act
            Action a = () => bindTask.WithName(name);
            Action b = () => bindTask.AsDefault().WithName(name);

            // assert
            a.ShouldThrow<ArgumentException>();
            b.ShouldThrow<ArgumentException>();
        }


    }
}
