using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thor.Generator.Templates;
using Xunit;

namespace Thor.Generator
{
    public class MessageParserTests
    {
        [Fact]
        public void FindPlaceholders()
        {
            // arrange
            string message = "{foo:c} {foo:c:sjkfj_fjd:shjhda} {foo} {a}{b} " +
                "{{escaped}} {{{not-escaped}}}";

            // act
            Placeholder[] placeholders =
                MessageParser.FindPlaceholders(message).ToArray();

            // assert
            Assert.Collection(placeholders,
                t =>
                {
                    Assert.Equal(0, t.Start);
                    Assert.Equal(6, t.End);
                    Assert.Equal("foo", t.Name);
                    Assert.Equal("c", t.Format);
                },
                t =>
                {
                    Assert.Equal(8, t.Start);
                    Assert.Equal(31, t.End);
                    Assert.Equal("foo", t.Name);
                    Assert.Equal("c:sjkfj_fjd:shjhda", t.Format);
                },
                t =>
                {
                    Assert.Equal(33, t.Start);
                    Assert.Equal(37, t.End);
                    Assert.Equal("foo", t.Name);
                    Assert.Null(t.Format);
                },
                t =>
                {
                    Assert.Equal(39, t.Start);
                    Assert.Equal(41, t.End);
                    Assert.Equal("a", t.Name);
                    Assert.Null(t.Format);
                },
                t =>
                {
                    Assert.Equal(42, t.Start);
                    Assert.Equal(44, t.End);
                    Assert.Equal("b", t.Name);
                    Assert.Null(t.Format);
                },
                t =>
                {
                    Assert.Equal(60, t.Start);
                    Assert.Equal(72, t.End);
                    Assert.Equal("not-escaped", t.Name);
                    Assert.Null(t.Format);
                });
        }

        [Fact]
        public void ReplacePlaceholders()
        {
            // arrange
            string message =
                "{foo:c} {foo:c:sjkfj_fjd:shjhda} {foo} {a}{b} " +
                "{{z}} {{{z}}}";

            Placeholder[] placeholders =
                MessageParser.FindPlaceholders(message).ToArray();

            // act
            int i = 0;
            string newMessage = MessageParser.ReplacePlaceholders(
                message,
                placeholders,
                p => p.ToString(i++));

            // assert
            string expectedMessage =
                "{0:c} {1:c:sjkfj_fjd:shjhda} {2} {3}{4} " +
                "{{z}} {{{5}}}";
            Assert.Equal(expectedMessage, newMessage);
        }

        [Fact]
        public void Placeholder_ToString_NoFormat()
        {
            // arrange
            Placeholder placeholder = new Placeholder(0, 0, "foo");

            // act
            string result = placeholder.ToString();

            // assert
            Assert.Equal("{foo}", result);
        }

        [Fact]
        public void Placeholder_ToString_WithFormat()
        {
            // arrange
            Placeholder placeholder = new Placeholder(0, 0, "foo", "format");

            // act
            string result = placeholder.ToString();

            // assert
            Assert.Equal("{foo:format}", result);
        }

        [Fact]
        public void Placeholder_ToStringIndex_NoFormat()
        {
            // arrange
            Placeholder placeholder = new Placeholder(0, 0, "foo");

            // act
            string result = placeholder.ToString(1);

            // assert
            Assert.Equal("{1}", result);
        }

        [Fact]
        public void Placeholder_ToStringIndex_WithFormat()
        {
            // arrange
            Placeholder placeholder = new Placeholder(0, 0, "foo", "format");

            // act
            string result = placeholder.ToString(1);

            // assert
            Assert.Equal("{1:format}", result);
        }
    }
}