using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChilliCream.Tracing.Generator.ProjectSystem.CSharp;
using FluentAssertions;
using Xunit;

namespace ChilliCream.Tracing.Generator.ProjectSystem.Tests
{
    public class CSharpClassicProjectSystemTests
        : ProjectSystemTestBase
    {
        protected override string ValidProject => TestProjects.ValidClassicProject;

        protected override IProjectSystem ProjectSystem { get; } = new CSharpClassicProjectSystem();
    }

    public abstract class ProjectSystemTestBase
        : IDisposable
    {
        private readonly List<string> _createdDirectories = new List<string>();

        protected abstract IProjectSystem ProjectSystem { get; }
        protected abstract string ValidProject { get; }


        [Fact]
        public async Task OpenProject()
        {
            // arrange
            string tempDirectory = ExtractTestFiles(ValidProject);
            string projectFile = Path.Combine(tempDirectory, "ClassLibrary1", "ClassLibrary1.csproj");

            Directory.CreateDirectory(tempDirectory);
            ZipUtils.Extract(TestProjects.ValidClassicProject, tempDirectory);

            // act
            Project project = await ProjectSystem.OpenAsync(projectFile);

            // assert
            project.Documents.Should().HaveCount(2);
            project.Documents.Any(t => t.Name == "IManyArgumentsEventSource.cs").Should().BeTrue();
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
