using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChilliCream.Tracing.Generator.Tasks;
using FluentAssertions;
using Xunit;

namespace ChilliCream.Tracing.Generator
{
    public class BindArgumentTests
    {
        [Fact]
        public void WithName()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());
            string name = Guid.NewGuid().ToString();

            // act
            bindArgument.WithName(name);

            // assert
            task.Arguments.ContainsKey(name).Should().BeTrue();
        }

        [Fact]
        public void WithNameArgumentValidation()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());
            string name = Guid.NewGuid().ToString();

            // act
            Action a = () => bindArgument.WithName(null);
            Action b = () => bindArgument.WithName(string.Empty);

            // assert
            a.ShouldThrow<ArgumentNullException>();
            b.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ReplaceName()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());
            string name = Guid.NewGuid().ToString();

            // act
            bindArgument.WithName(Guid.NewGuid().ToString()).WithName(name);

            // assert
            task.Arguments.Should().HaveCount(1);
            task.Arguments.ContainsKey(name).Should().BeTrue();
        }

        [Fact]
        public void WithNameAndKey()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());
            string name = Guid.NewGuid().ToString();
            char key = 'h';

            // act
            bindArgument.WithName(name, key);

            // assert
            task.Arguments.ContainsKey(name).Should().BeTrue();
            task.Arguments.ContainsKey(key.ToString()).Should().BeTrue();
            task.Arguments.Should().HaveCount(2);
        }

        [Fact]
        public void WithNameAndKeyArgumentValidation()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());
            string name = Guid.NewGuid().ToString();
            char key = 'h';

            // act
            Action a = () => bindArgument.WithName(name, ' ');
            Action b = () => bindArgument.WithName(name, '1');
            Action c = () => bindArgument.WithName(null, key);
            Action d = () => bindArgument.WithName(string.Empty, key);

            // assert
            a.ShouldThrow<ArgumentException>();
            b.ShouldThrow<ArgumentException>();
            c.ShouldThrow<ArgumentNullException>();
            d.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void WithNameAndKeyKeyValidation()
        {
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());
            string name = Guid.NewGuid().ToString();

            for (int i = 97; i < 123; i++)
            {
                Action a = () => bindArgument.WithName(name, (char)i);
                a.ShouldNotThrow();
            }
        }

        [InlineData(20)]
        [InlineData(10)]
        [InlineData(3)]
        [InlineData(1)]
        [InlineData(0)]
        [Theory]
        public void Position(int position)
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());

            // act
            bindArgument.Position(position);

            // assert
            task.PositionalArguments
                .Should().HaveCount(1);
            task.PositionalArguments
                .Any(t => t.Position == position)
                .Should().BeTrue();
            task.Arguments.Should().BeEmpty();
        }

        [Fact]
        public void PositionArgumentValidation()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());

            // act
            Action a = () => bindArgument.Position(-1);

            // assert
            a.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void DuplicatePosition()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());
            bindArgument.Position(1);

            bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());

            // act
            Action a = () => bindArgument.Position(1);

            // assert
            a.ShouldThrow<InvalidOperationException>();
        }
    }
}
