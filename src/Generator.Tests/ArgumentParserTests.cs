using System.Linq;
using ChilliCream.Tracing.Generator.Tasks;
using FluentAssertions;
using Xunit;

namespace ChilliCream.Tracing.Generator
{
    public class ArgumentParserTests
    {
        [Fact]
        public void Parse()
        {
            // arrange
            string[] args = ArgumentParserTestConstants.DefaultArguments;

            // act
            Argument[] arguments = ArgumentParser.Parser(args).ToArray();

            // assert
            arguments[0].Name.Should().BeNullOrEmpty("argument 0 have no name");
            arguments[0].Value.Should().Be("a a");
            arguments[0].Position.Should().Be(0);
            arguments[0].IsSelected.Should().BeFalse("argument 0 should not be selected");

            arguments[1].Name.Should().BeNullOrEmpty("argument 1 have no name");
            arguments[1].Value.Should().Be("b");
            arguments[1].Position.Should().Be(1);
            arguments[1].IsSelected.Should().BeFalse("argument 1 should not be selected");

            arguments[2].Name.Should().BeNullOrEmpty("argument 2 have no name");
            arguments[2].Value.Should().Be("c");
            arguments[2].Position.Should().Be(2);
            arguments[2].IsSelected.Should().BeFalse("argument 2 should not be selected");

            arguments[3].Name.Should().BeNullOrEmpty("argument 3 have no name");
            arguments[3].Value.Should().Be("x");
            arguments[3].Position.Should().Be(3);
            arguments[3].IsSelected.Should().BeFalse("argument 3 should not be selected");

            arguments[4].Name.Should().Be("zzz");
            arguments[4].Value.Should().BeNullOrEmpty("argument 4 have no value");
            arguments[4].Position.Should().Be(4);
            arguments[4].IsSelected.Should().BeTrue("argument 4 should be selected");

            arguments[5].Name.Should().Be("x");
            arguments[5].Value.Should().Be("s");
            arguments[5].Position.Should().Be(5);
            arguments[5].IsSelected.Should().BeTrue("argument 5 should be selected");

            arguments[6].Name.Should().Be("y");
            arguments[6].Value.Should().BeNullOrEmpty("argument 6 have no value");
            arguments[6].Position.Should().Be(6);
            arguments[6].IsSelected.Should().BeFalse("argument 6 should not be selected");

            arguments[7].Name.Should().Be("aaa");
            arguments[7].Value.Should().Be("s s");
            arguments[7].Position.Should().Be(7);
            arguments[7].IsSelected.Should().BeTrue("argument 7 should be selected");

            arguments[8].Name.Should().Be("z");
            arguments[8].Value.Should().Be("s s");
            arguments[8].Position.Should().Be(8);
            arguments[8].IsSelected.Should().BeTrue("argument 8 should be selected");
        }
    }
}
