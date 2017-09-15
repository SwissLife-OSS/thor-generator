using System;
using System.IO;
using System.Linq;
using ChilliCream.Logging.Generator;
using FluentAssertions;
using Xunit;

namespace Generator.ProjectSystem.Tests
{
    public class DocumentIdTests
    {
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
            bool result = ida.Equals(idb);

            // assert
            result.Equals(expectedResult);
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

            // act
            bool result = ida == idb;

            // assert
            result.Equals(expectedResult);
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
            result.Equals(expectedResult);
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
