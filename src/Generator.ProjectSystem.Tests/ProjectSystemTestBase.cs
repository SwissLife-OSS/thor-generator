using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace ChilliCream.Tracing.Generator.ProjectSystem.Tests
{
    public abstract class ProjectSystemTestBase
        : IDisposable
    {
        private readonly List<string> _createdDirectories = new List<string>();

        protected abstract IProjectSystem ProjectSystem { get; }
        protected abstract string ValidProject { get; }
        protected abstract int ValidProjectInitialFiles { get; }
        protected abstract IProjectId ValidProjectId { get; }
        protected abstract IProjectId InvalidProjectId { get; }

        [Fact]
        public void CanHandleValidProject()
        {
            // arrange
            string tempDirectory = ExtractTestFiles(ValidProject);
            string projectFile = Path.Combine(tempDirectory, "ClassLibrary1", "ClassLibrary1.csproj");

            // act
            bool result = ProjectSystem.CanHandle(projectFile);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CanHandleWithProjectId()
        {
            // act
            bool resulta = ProjectSystem.CanHandle(ValidProjectId);
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
            string projectFile = Path.Combine(tempDirectory, "ClassLibrary1", "ClassLibrary1.csproj");

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
            string projectFile = Path.Combine(tempDirectory, "ClassLibrary1", "ClassLibrary1.csproj");

            Directory.CreateDirectory(tempDirectory);
            ZipUtils.Extract(TestProjects.ValidClassicProject, tempDirectory);

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
