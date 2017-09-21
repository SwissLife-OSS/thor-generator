using System;
using ChilliCream.Tracing.Generator.ProjectSystem.CSharp;

namespace ChilliCream.Tracing.Generator.ProjectSystem.Tests
{
    public class CSharpClassicProjectSystemTests
        : ProjectSystemTestBase
    {
        protected override IProjectSystem ProjectSystem { get; } = new CSharpClassicProjectSystem();

        protected override string ValidProject => TestProjects.ValidClassicProject;
        protected override int ValidProjectInitialFiles => 2;

        protected override IProjectId InvalidProjectId => new CSharpDirectoryProjectId(Guid.NewGuid().ToString());

        protected override IProjectId CreateValidProjectId(string randomString)
        {
            return new CSharpClassicProjectId(randomString);
        }
    }
}
