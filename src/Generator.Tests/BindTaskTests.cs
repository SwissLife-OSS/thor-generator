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

    }
}
