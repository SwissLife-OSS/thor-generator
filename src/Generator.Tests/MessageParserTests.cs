using System.Collections.Generic;
using System.Linq;
using Thor.Generator.Templates;
using Xunit;

namespace Thor.Generator
{
    public class MessageParserTests
    {
        [Fact]
        public void Parse()
        {
            // arrange
            string message = "{foo:c} {foo:c:sjkfj_fjd:shjhda} {foo} {a}{b} {{escaped}} {{{not-escaped}}}";

            // act
            Placeholder[] placeholders = MessageParser.FindPlaceholders(message).ToArray();

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
                    Assert.Equal(6, t.End);
                    Assert.Equal("foo", t.Name);
                    Assert.Equal("c:sjkfj_fjd:shjhda", t.Format);
                },
                t =>
                {
                    Assert.Equal(0, t.Start);
                    Assert.Equal(6, t.End);
                    Assert.Equal("foo", t.Name);
                    Assert.Null(t.Format);
                },
                t =>
                {
                    Assert.Equal(0, t.Start);
                    Assert.Equal(6, t.End);
                    Assert.Equal("a", t.Name);
                    Assert.Null(t.Format);
                },
                t =>
                {
                    Assert.Equal(0, t.Start);
                    Assert.Equal(6, t.End);
                    Assert.Equal("b", t.Name);
                    Assert.Null(t.Format);
                },
                t =>
                {
                    Assert.Equal(0, t.Start);
                    Assert.Equal(6, t.End);
                    Assert.Equal("not-escaped", t.Name);
                    Assert.Null(t.Format);
                });
        }
    }
}