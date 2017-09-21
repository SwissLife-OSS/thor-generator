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

        protected override IProjectId ValidProjectId => new CSharpClassicProjectId(Guid.NewGuid().ToString());
        protected override IProjectId InvalidProjectId => new CSharpDirectoryProjectId(Guid.NewGuid().ToString());
    }
}
