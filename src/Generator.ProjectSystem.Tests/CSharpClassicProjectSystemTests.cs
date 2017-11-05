using System;
using System.IO;
using Thor.Generator.ProjectSystem.CSharp;

namespace Thor.Generator.ProjectSystem.Tests
{
    public class CSharpClassicProjectSystemTests
        : ProjectSystemTestBase
    {
        protected override IProjectSystem ProjectSystem { get; } = new CSharpClassicProjectSystem();

        protected override string ValidProject => TestProjects.ValidClassicProject;
        protected override int ValidProjectInitialFiles => 2;
        protected override string ValidProjectFileName { get; } = Path.Combine("ClassLibrary1", "ClassLibrary1.csproj");

        protected override IProjectId InvalidProjectId => new CSharpDirectoryProjectId(Guid.NewGuid().ToString());

        protected override IProjectId CreateValidProjectId(string randomString)
        {
            return new CSharpClassicProjectId(randomString);
        }
    }
}
