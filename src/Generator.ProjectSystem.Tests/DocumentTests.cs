using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChilliCream.Logging.Generator;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace Generator.ProjectSystem.Tests
{
    public class DocumentTests
    {
        [Fact]
        public async Task DocumentWithStaticContent()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            string content = Guid.NewGuid().ToString();

            // act
            Document document = Document.Create(content, name);
            string retrievedContent = await document.GetContentAsync();

            // assert
            document.Id.Should().NotBeNull();
            document.Id.ToString().Should().Be(name);
            document.Name.Should().Be(name);
            document.Folders.Should().BeEmpty();
            retrievedContent.Should().Be(content);
        }

        [Fact]
        public async Task DocumentWithStaticContentWithEnumerable()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            string content = Guid.NewGuid().ToString();

            // act
            Document document = Document.Create(content, name, Enumerable.Empty<string>());
            string retrievedContent = await document.GetContentAsync();

            // assert
            document.Name.Should().Be(name);
            document.Folders.Should().BeEmpty();
            retrievedContent.Should().Be(content);
        }

        [Fact]
        public void DocumentWithStaticContentArgumentValidation()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            string content = Guid.NewGuid().ToString();

            // act
            Action a = () => Document.Create((string)null, name);
            Action b = () => Document.Create(content, null);
            Action c = () => Document.Create(content, name, (string[])null);

            Action d = () => Document.Create((string)null, name, Enumerable.Empty<string>());
            Action e = () => Document.Create(content, null, Enumerable.Empty<string>());
            Action f = () => Document.Create(content, name, (IEnumerable<string>)null);

            Action g = () => Document.Create(content, name, Array.Empty<string>());
            Action h = () => Document.Create(content, name, Enumerable.Empty<string>());
            Action i = () => Document.Create(content, name, Guid.NewGuid().ToString());
            Action j = () => Document.Create(content, name, (IEnumerable<string>)new string[] { "a" });

            // assert
            a.ShouldThrow<ArgumentNullException>("a");
            b.ShouldThrow<ArgumentNullException>("b");
            c.ShouldThrow<ArgumentNullException>("c");
            d.ShouldThrow<ArgumentNullException>("d");
            e.ShouldThrow<ArgumentNullException>("e");
            f.ShouldThrow<ArgumentNullException>("f");
            g.ShouldNotThrow("g");
            h.ShouldNotThrow("h");
            i.ShouldNotThrow("i");
            j.ShouldNotThrow("j");
        }

        [Fact]
        public async Task DocumentWithAsyncDelegateContent()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            string content = Guid.NewGuid().ToString();
            Task<string> task = Task.FromResult(content);

            // act
            Document document = Document.Create(c => task, name);
            string retrievedContent = await document.GetContentAsync();

            // assert
            document.Id.Should().NotBeNull();
            document.Id.ToString().Should().Be(name);
            document.Name.Should().Be(name);
            document.Folders.Should().BeEmpty();
            retrievedContent.Should().Be(content);
        }

        [Fact]
        public async Task DocumentWithAsyncDelegateContentWithEnumerable()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            string content = Guid.NewGuid().ToString();
            Task<string> task = Task.FromResult(content);

            // act
            Document document = Document.Create(c => task, name, Enumerable.Empty<string>());
            string retrievedContent = await document.GetContentAsync();

            // assert
            document.Name.Should().Be(name);
            document.Folders.Should().BeEmpty();
            retrievedContent.Should().Be(content);
        }

        [Fact]
        public void DocumentWithAsyncDelegateContentArgumentValidation()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            string content = Guid.NewGuid().ToString();
            Task<string> task = Task.FromResult(content);

            // act
            Action a = () => Document.Create((Func<CancellationToken, Task<string>>)null, name);
            Action b = () => Document.Create(ct => task, null);
            Action c = () => Document.Create(ct => task, name, (string[])null);

            Action d = () => Document.Create((Func<CancellationToken, Task<string>>)null, name, Enumerable.Empty<string>());
            Action e = () => Document.Create(ct => task, null, Enumerable.Empty<string>());
            Action f = () => Document.Create(ct => task, name, (IEnumerable<string>)null);

            Action g = () => Document.Create(ct => task, name, Array.Empty<string>());
            Action h = () => Document.Create(ct => task, name, Enumerable.Empty<string>());
            Action i = () => Document.Create(ct => task, name, Guid.NewGuid().ToString());
            Action j = () => Document.Create(ct => task, name, (IEnumerable<string>)new string[] { "a" });

            // assert
            a.ShouldThrow<ArgumentNullException>("a");
            b.ShouldThrow<ArgumentNullException>("b");
            c.ShouldThrow<ArgumentNullException>("c");
            d.ShouldThrow<ArgumentNullException>("d");
            e.ShouldThrow<ArgumentNullException>("e");
            f.ShouldThrow<ArgumentNullException>("f");
            g.ShouldNotThrow("g");
            h.ShouldNotThrow("h");
            i.ShouldNotThrow("i");
            j.ShouldNotThrow("j");
        }

        [Fact]
        public async Task DocumentWithDelegateContent()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            string content = Guid.NewGuid().ToString();
            Func<string> func = () => content;

            // act
            Document document = Document.Create(func, name);
            string retrievedContent = await document.GetContentAsync();

            // assert
            document.Id.Should().NotBeNull();
            document.Id.ToString().Should().Be(name);
            document.Name.Should().Be(name);
            document.Folders.Should().BeEmpty();
            retrievedContent.Should().Be(content);
        }

        [Fact]
        public async Task DocumentWithDelegateContentWithEnumerable()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            string content = Guid.NewGuid().ToString();
            Func<string> func = () => content;

            // act
            Document document = Document.Create(func, name, Enumerable.Empty<string>());
            string retrievedContent = await document.GetContentAsync();

            // assert
            document.Name.Should().Be(name);
            document.Folders.Should().BeEmpty();
            retrievedContent.Should().Be(content);
        }

        [Fact]
        public void DocumentWithDelegateContentArgumentValidation()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            string content = Guid.NewGuid().ToString();
            Func<string> func = () => content;

            // act
            Action a = () => Document.Create((Func<string>)null, name);
            Action b = () => Document.Create(func, null);
            Action c = () => Document.Create(func, name, (string[])null);

            Action d = () => Document.Create((Func<string>)null, name, Enumerable.Empty<string>());
            Action e = () => Document.Create(func, null, Enumerable.Empty<string>());
            Action f = () => Document.Create(func, name, (IEnumerable<string>)null);

            Action g = () => Document.Create(func, name, Array.Empty<string>());
            Action h = () => Document.Create(func, name, Enumerable.Empty<string>());
            Action i = () => Document.Create(func, name, Guid.NewGuid().ToString());
            Action j = () => Document.Create(func, name, (IEnumerable<string>)new string[] { "a" });

            // assert
            a.ShouldThrow<ArgumentNullException>("a");
            b.ShouldThrow<ArgumentNullException>("b");
            c.ShouldThrow<ArgumentNullException>("c");
            d.ShouldThrow<ArgumentNullException>("d");
            e.ShouldThrow<ArgumentNullException>("e");
            f.ShouldThrow<ArgumentNullException>("f");
            g.ShouldNotThrow("g");
            h.ShouldNotThrow("h");
            i.ShouldNotThrow("i");
            j.ShouldNotThrow("j");
        }

        [Fact]
        public void DocumentEquality()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            Document a = Document.Create(Guid.NewGuid().ToString(), name);
            Document b = Document.Create(Guid.NewGuid().ToString(), name);
            Document c = Document.Create(Guid.NewGuid().ToString(), name, Guid.NewGuid().ToString());
            Document d = Document.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Document e = a;

            // act
            bool resulta = a.Equals(b);
            bool resultb = a.Equals(c);
            bool resultc = a.Equals(d);
            bool resultd = a.Equals(a);
            bool resulte = a == b;
            bool resultf = a != b;
            bool resultg = a == e;
            bool resulth = a != e;
            bool resulti = a == null;
            bool resultj = a != null;
            bool resultk = a.Equals(null);

            // assert
            resulta.Should().BeTrue("(a) a and b have the same document id");
            resultb.Should().BeFalse("(b) a and c have the same name but are located in different folders");
            resultc.Should().BeFalse("(c) a and d have a completely different document id");
            resultd.Should().BeTrue("(d) a is reference equal to itself");
            resulte.Should().BeFalse("(e) a and b share the same document id but are not the same instance");
            resultf.Should().BeTrue("(f) a and b share the same document id but are not the same instance");
            resultg.Should().BeTrue("(g) a and e are pointing to the same reference");
            resulth.Should().BeFalse("(h) a and e are pointing to the same reference");
            resulti.Should().BeFalse("(i) a is not null.");
            resultj.Should().BeTrue("(j) a is not null.");
            resultk.Should().BeFalse("(k) a is not null.");
        }

        [Fact]
        public void DocumentObjectEquality()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            Document a = Document.Create(Guid.NewGuid().ToString(), name);
            Document b = Document.Create(Guid.NewGuid().ToString(), name);
            Document c = Document.Create(Guid.NewGuid().ToString(), name, Guid.NewGuid().ToString());
            Document d = Document.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            // act
            bool resulta = a.Equals((object)b);
            bool resultb = a.Equals((object)c);
            bool resultc = a.Equals((object)d);
            bool resultd = a.Equals((object)a);
            bool resulte = a.Equals((object)null);

            // assert
            resulta.Should().BeTrue("(a) a and b have the same document id");
            resultb.Should().BeFalse("(b) a and c have the same name but are located in different folders");
            resultc.Should().BeFalse("(c) a and d have a completely different document id");
            resultd.Should().BeTrue("(d) a is reference equal to itself");
            resulte.Should().BeFalse("(e) a is not null.");
        }

        [Fact]
        public void DocumentToString()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            string content = Guid.NewGuid().ToString();

            // act
            Document document = Document.Create(content, name);

            // assert
            document.ToString().Should().Be(document.Id.ToString());
        }

        [Fact]
        public void DocumentGetHashCode()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            Document a = Document.Create(Guid.NewGuid().ToString(), name);
            Document b = Document.Create(Guid.NewGuid().ToString(), name);
            Document c = Document.Create(Guid.NewGuid().ToString(), name, Guid.NewGuid().ToString());
            Document d = Document.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            // act
            int resulta = a.GetHashCode();
            int resultb = b.GetHashCode();
            int resultc = c.GetHashCode();
            int resultd = d.GetHashCode();

            // assert
            resulta.Should().Be(resultb, "a and b have the same document id");
            resultc.Should().NotBe(resulta, "c and a have different document ids");
            resultd.Should().NotBe(resulta, "d and a have different document ids");
            resultd.Should().NotBe(resultc, "d and c have different document ids");
        }

        [Fact]
        public async Task DocumentGetSyntaxRoot()
        {
            // arrange
            string name = Guid.NewGuid().ToString();
            string content = "class Foo { }";

            // act
            Document document = Document.Create(content, name);
            Microsoft.CodeAnalysis.SyntaxNode root = await document.GetSyntaxRootAsync();

            // assert
            root.Should().NotBeNull();
            (root is CompilationUnitSyntax).Should().BeTrue("the root should be a CompilationUnitSyntax");
            ClassDeclarationSyntax classDeclaration = ((CompilationUnitSyntax)root).Members.OfType<ClassDeclarationSyntax>().FirstOrDefault();
            classDeclaration.Should().NotBeNull("the document should contain a class declaration");
            classDeclaration.Identifier.ValueText.Should().Be("Foo", "the class name should be foo");
        }

        [Fact]
        public void DocumentCreateFilePath()
        {
            // arrange
            string root = Path.GetTempPath();
            string name = "a.cs";
            string folder = "b";
            string expectedFilePath = Path.Combine(root, folder, name);

            // act
            Document document = Document.Create(Guid.NewGuid().ToString(), name, folder);
            string filePath = document.CreateFilePath(root);

            // assert
            filePath.Should().Be(expectedFilePath);
        }

        [Fact]
        public void DocumentCreateFilePathArgumentValidation()
        {
            // arrange
            string root = Path.GetTempPath();
            string name = "a.cs";
            string folder = "b";
            string expectedFilePath = Path.Combine(root, folder, name);

            // act
            Document document = Document.Create(Guid.NewGuid().ToString(), name, folder);
            Action a = () => document.CreateFilePath(root);
            Action b = () => document.CreateFilePath(null);
            Action c = () => DocumentExtensions.CreateFilePath(null, root);

            // assert
            a.ShouldNotThrow();
            b.ShouldThrow<ArgumentNullException>();
            c.ShouldThrow<ArgumentNullException>();
        }
    }
}
