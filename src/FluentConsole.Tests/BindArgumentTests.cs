using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;

namespace ChilliCream.FluentConsole
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
                    typeof(CreateSolutionEventSources).GetProperties().Last());

            // act
            Action a = () => bindArgument.Position(1);

            // assert
            a.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void Mandatory()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());
            string name = Guid.NewGuid().ToString();

            // act
            bindArgument.WithName(name).Mandatory();

            // assert
            task.Arguments.Should().HaveCount(1);
            task.Arguments[name].IsMandatory.Should().BeTrue();
        }

        [Fact]
        public void ConstructorArgumentValidation()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();

            // act
            Action a = () => new BindArgument<CreateSolutionEventSources>(task,
                typeof(CreateSolutionEventSources).GetProperties().First());
            Action b = () => new BindArgument<CreateSolutionEventSources>(null,
                typeof(CreateSolutionEventSources).GetProperties().First());
            Action c = () => new BindArgument<CreateSolutionEventSources>(task, null);

            // assert
            a.ShouldNotThrow();
            b.ShouldThrow<ArgumentNullException>();
            c.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void And()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());
            string name = Guid.NewGuid().ToString();

            // act
            bindArgument.WithName(Guid.NewGuid().ToString())
                .And()
                .Argument(t => t.Recursive)
                .WithName(name);

            // assert
            task.Arguments.Should().HaveCount(2);
            task.Arguments.ContainsKey(name).Should().BeTrue();
        }

        [Fact]
        public void AndWithPosition()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());
            string name = Guid.NewGuid().ToString();

            // act
            bindArgument.Position(0)
                .And()
                .Argument(t => t.Recursive)
                .WithName(name);

            // assert
            task.Arguments.Should().HaveCount(1);
            task.Arguments.ContainsKey(name).Should().BeTrue();
            task.PositionalArguments.Should().HaveCount(1);
            task.PositionalArguments.First()
                .Position.Should().Be(0);
            task.PositionalArguments.First()
                .Should().NotBe(task.Arguments.Values.First());
        }

        [Fact]
        public void AndWithDuplicateProperty()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperty("Recursive"));
            string name = Guid.NewGuid().ToString();

            // act
            Action a = () => bindArgument.WithName(Guid.NewGuid().ToString())
                .And()
                .Argument(t => t.Recursive)
                .WithName(name);

            // assert
            a.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void AndWithDuplicateName()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());
            string name = Guid.NewGuid().ToString();

            // act
            Action a = () => bindArgument.WithName(name)
                .And()
                .Argument(t => t.Recursive)
                .WithName(name);

            // assert
            a.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void AndWithDuplicateKey()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());

            // act
            Action a = () => bindArgument.WithName(Guid.NewGuid().ToString(), 'a')
                .And()
                .Argument(t => t.Recursive)
                .WithName(Guid.NewGuid().ToString(), 'a');

            // assert
            a.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void AndWithUnspecifiedArument()
        {
            // arrange
            TaskDefinition task = new TaskDefinition();
            BindArgument<CreateSolutionEventSources> bindArgument
                = new BindArgument<CreateSolutionEventSources>(task,
                    typeof(CreateSolutionEventSources).GetProperties().First());

            // act
            Action a = () => bindArgument
                .And()
                .Argument(t => t.Recursive);

            // assert
            a.ShouldThrow<InvalidOperationException>();
        }
    }
}
