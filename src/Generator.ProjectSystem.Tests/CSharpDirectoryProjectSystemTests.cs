using System;
using Thor.Generator.ProjectSystem.CSharp;

namespace Thor.Generator.ProjectSystem.Tests
{
    public class CSharpDirectoryProjectSystemTests
        : ProjectSystemTestBase
    {
        protected override IProjectSystem ProjectSystem { get; } = new CSharpDirectoryProjectSystem();

        protected override string ValidProject => TestProjects.ValidDirectoryProject;
        protected override int ValidProjectInitialFiles => 1;
        protected override string ValidProjectFileName { get; } = "ClassLibrary1";

        protected override IProjectId InvalidProjectId => new CSharpCoreProjectId(Guid.NewGuid().ToString());

        protected override IProjectId CreateValidProjectId(string randomString)
        {
            return new CSharpDirectoryProjectId(randomString);
        }
    }
}
