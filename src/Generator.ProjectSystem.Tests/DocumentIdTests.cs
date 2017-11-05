using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Thor.Generator.ProjectSystem.Tests
{
    public class DocumentIdTests
    {
        [Fact]
        public void DocumentIdArgumentVariants()
        {
            // act
            Action a = () => new DocumentId(null, Array.Empty<string>());
            Action b = () => new DocumentId("a", Array.Empty<string>());
            Action c = () => new DocumentId("a", new string[] { "b" });
            Action d = () => new DocumentId("a", null);

            // assert
            a.ShouldThrow<ArgumentNullException>();
            b.ShouldNotThrow();
            c.ShouldNotThrow();
            d.ShouldNotThrow();
        }

        [Fact]
        public void DocumentIdToString()
        {
            // arrange
            DocumentId documentId = new DocumentId("foo.cs", new string[] { "a", "b" });

            // act
            string result = documentId.ToString();

            // assert
            string expectedString = string.Concat("a", Path.DirectorySeparatorChar,
                "b", Path.DirectorySeparatorChar, "foo.cs");
            result.Should().Be(expectedString);
        }


        [InlineData("a,b,f.cs", "A,b,f.cs", false)]
        [InlineData("a,b,f.cs", "c,d,f.cs", false)]
        [InlineData("a,b,f.cs", "a,b,f.cs", true)]
        [Theory]
        public void DocumentIdEquality(string a, string b, bool expectedResult)
        {
            // arrange
            DocumentId ida = DocumentIdFromString(a, ',');
            DocumentId idb = DocumentIdFromString(b, ',');

            // act
            bool resulta = ida.Equals(idb);
            bool resultb = ida.Equals(ida);
            bool resultc = ida.Equals(null);

            // assert
            resulta.Should().Be(expectedResult);
            resultb.Should().BeTrue();
            resultc.Should().BeFalse();
        }

        [InlineData("a,b,f.cs", "A,b,f.cs", false)]
        [InlineData("a,b,f.cs", "c,d,f.cs", false)]
        [InlineData("a,b,f.cs", "a,b,f.cs", true)]
        [Theory]
        public void DocumentIdObjectEquality(string a, string b, bool expectedResult)
        {
            // arrange
            DocumentId ida = DocumentIdFromString(a, ',');
            DocumentId idb = DocumentIdFromString(b, ',');

            // act
            bool resulta = ida.Equals((object)idb);
            bool resultb = ida.Equals((object)ida);
            bool resultc = ida.Equals((object)null);

            // assert
            resulta.Should().Be(expectedResult);
            resultb.Should().BeTrue();
            resultc.Should().BeFalse();
        }

        [InlineData("a,b,f.cs", "A,b,f.cs", false)]
        [InlineData("a,b,f.cs", "c,d,f.cs", false)]
        [InlineData("a,b,f.cs", "a,b,f.cs", true)]
        [Theory]
        public void DocumentIdEqualsOperator(string a, string b, bool expectedResult)
        {
            // arrange
            DocumentId ida = DocumentIdFromString(a, ',');
            DocumentId idb = DocumentIdFromString(b, ',');
            DocumentId idc = ida;

            // act
            bool resulta = ida == idb;
            bool resultb = null == ida;
            bool resultc = ida == null;
            bool resultd = ida == idc;

            // assert
            resulta.Should().Be(expectedResult);
            resultb.Should().BeFalse();
            resultc.Should().BeFalse();
        }

        [InlineData("a,b,f.cs", "A,b,f.cs", true)]
        [InlineData("a,b,f.cs", "c,d,f.cs", true)]
        [InlineData("a,b,f.cs", "a,b,f.cs", false)]
        [Theory]
        public void DocumentIdNotEqualsOperator(string a, string b, bool expectedResult)
        {
            // arrange
            DocumentId ida = DocumentIdFromString(a, ',');
            DocumentId idb = DocumentIdFromString(b, ',');

            // act
            bool result = ida != idb;

            // assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void DocumentIdGetHashCode()
        {
            // arrange
            DocumentId a1 = DocumentIdFromString("a,b,f.cs", ',');
            DocumentId a2 = DocumentIdFromString("a,b,f.cs", ',');
            DocumentId b = DocumentIdFromString("c,d,e.cs", ',');

            // act
            int hasha1 = a1.GetHashCode();
            int hasha2 = a2.GetHashCode();
            int hashb = b.GetHashCode();

            // assert
            hasha1.Should().Be(hasha2);
            hasha1.Should().NotBe(hashb);
        }


        private DocumentId DocumentIdFromString(string s, char seperator)
        {
            string[] parts = s.Split(seperator);
            return new DocumentId(parts.Last(), parts.Take(parts.Length - 1));
        }
    }
}
