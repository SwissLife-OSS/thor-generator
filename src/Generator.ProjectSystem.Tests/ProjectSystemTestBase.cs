using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Thor.Generator.ProjectSystem.Tests
{
    public abstract class ProjectSystemTestBase
        : IDisposable
    {
        private readonly List<string> _createdDirectories = new List<string>();

        protected abstract IProjectSystem ProjectSystem { get; }

        protected abstract string ValidProject { get; }
        protected abstract int ValidProjectInitialFiles { get; }
        protected abstract string ValidProjectFileName { get; }

        protected abstract IProjectId InvalidProjectId { get; }
        protected abstract IProjectId CreateValidProjectId(string randomString);

        [Fact]
        public void ProjectIdEquals()
        {
            // arrange
            IProjectId ida1 = CreateValidProjectId("a");
            IProjectId ida2 = CreateValidProjectId("a");
            IProjectId idb = CreateValidProjectId("b");

            // act
            bool a = ida1.Equals(ida1);
            bool b = ida1.Equals(ida2);
            bool c = ida1.Equals(idb);
            bool d = idb.Equals(ida1);
            bool e = idb.Equals(null);
            bool f = idb.Equals(InvalidProjectId);

            bool g = ida1.Equals((object)ida1);
            bool h = ida1.Equals((object)ida2);
            bool i = ida1.Equals((object)idb);
            bool j = idb.Equals((object)ida1);
            bool k = idb.Equals((object)null);
            bool l = idb.Equals((object)InvalidProjectId);

            // assert
            a.Should().BeTrue("a1 is equals to a1");
            b.Should().BeTrue("a1 is equals to a2");
            c.Should().BeFalse("a1 is not equals to b");
            d.Should().BeFalse("b is not equals to a1");
            e.Should().BeFalse("b is not equals to null");
            f.Should().BeFalse("b is not equals to InvalidProjectId");

            g.Should().BeTrue("(obj) a1 is equals to a1");
            h.Should().BeTrue("(obj) a1 is equals to a2");
            i.Should().BeFalse("(obj) a1 is not equals to b");
            j.Should().BeFalse("(obj) b is not equals to a1");
            k.Should().BeFalse("(obj) b is not equals to null");
            l.Should().BeFalse("b is not equals to InvalidProjectId");
        }

        [InlineData("a")]
        [InlineData("b")]
        [InlineData("c")]
        [Theory]
        public void ProjectIdToString(string key)
        {
            // arrange
            IProjectId id = CreateValidProjectId(key);

            // act
            string value = id.ToString();

            // assert
            value.Should().Be(key);
        }

        [Fact]
        public void ProjectIdGetHashCode()
        {
            // arrange
            IProjectId ida1 = CreateValidProjectId("a");
            IProjectId ida2 = CreateValidProjectId("a");
            IProjectId idb = CreateValidProjectId("b");

            // act
            int a1 = ida1.GetHashCode();
            int a2 = ida2.GetHashCode();
            int b = idb.GetHashCode();

            // assert
            a1.Should().Be(a2, "a1 hash should be equals to a2 hash");
            a1.Should().NotBe(b, "a1 hash should not be equals to b hash");
        }

        [Fact]
        public void ProjectIdConstructorArgumentValidation()
        {
            // act
            Action a = () => CreateValidProjectId(null);

            // assert
            a.ShouldThrow<ArgumentNullException>("key is null");
        }

        [Fact]
        public void CanHandleValidProject()
        {
            // arrange
            string tempDirectory = ExtractTestFiles(ValidProject);
            string projectFile = Path.Combine(tempDirectory, ValidProjectFileName);

            // act
            bool result = ProjectSystem.CanHandle(projectFile);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CanHandleWithProjectId()
        {
            // act
            bool resulta = ProjectSystem.CanHandle(CreateValidProjectId(Guid.NewGuid().ToString("N")));
            bool resultb = ProjectSystem.CanHandle(InvalidProjectId);

            // assert
            resulta.Should().BeTrue("projectId is valid");
            resultb.Should().BeFalse("projectId is invalid");
        }

        [Fact]
        public void CanHandleArgumentValidation()
        {
            // act
            Action a = () => ProjectSystem.CanHandle((IProjectId)null);
            Action b = () => ProjectSystem.CanHandle((string)null);

            // assert
            a.ShouldThrow<ArgumentNullException>();
            b.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void OpenProject()
        {
            // arrange
            string tempDirectory = ExtractTestFiles(ValidProject);
            string projectFile = Path.Combine(tempDirectory, ValidProjectFileName);

            // act
            Project project = ProjectSystem.Open(projectFile);

            // assert
            project.Documents.Should().HaveCount(ValidProjectInitialFiles);
            project.Documents.Any(t => t.Name == "IManyArgumentsEventSource.cs").Should().BeTrue();
        }

        [Fact]
        public void CommitChanges()
        {
            // arrange
            string tempDirectory = ExtractTestFiles(ValidProject);
            string projectFile = Path.Combine(tempDirectory, ValidProjectFileName);

            Project project = ProjectSystem.Open(projectFile);
            project.Documents.Should().HaveCount(ValidProjectInitialFiles);

            // act
            project.UpdateDocument("foo", "foo.cs", "a");
            ProjectSystem.CommitChanges(project);

            // assert
            Project reloadedProject = ProjectSystem.Open(projectFile);

            reloadedProject.Should().NotBe(project);
            reloadedProject.Documents.Should().HaveCount(ValidProjectInitialFiles + 1);
            reloadedProject.Documents.First(t => t.Name == "foo.cs")
                .GetContent().Should().Be("foo");
        }

        private string ExtractTestFiles(string resource)
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            _createdDirectories.Add(tempDirectory);
            Directory.CreateDirectory(tempDirectory);
            ZipUtils.Extract(resource, tempDirectory);
            return tempDirectory;
        }

        #region CleanUp

        public void Dispose()
        {
            foreach (DirectoryInfo directory in _createdDirectories.Select(t => new DirectoryInfo(t)))
            {
                if (directory.Exists)
                {
                    foreach (FileInfo file in directory.GetFiles("*.*", SearchOption.AllDirectories))
                    {
                        try
                        {
                            file.Attributes = FileAttributes.Normal;
                            file.Delete();
                        }
                        catch
                        {
                            // ignore
                        }
                    }

                    try
                    {
                        directory.Delete(true);
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }
        }

        #endregion
    }
}
