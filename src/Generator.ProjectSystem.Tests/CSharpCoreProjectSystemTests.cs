using System;
using ChilliCream.Tracing.Generator.ProjectSystem.CSharp;

namespace ChilliCream.Tracing.Generator.ProjectSystem.Tests
{
    public class CSharpCoreProjectSystemTests
       : ProjectSystemTestBase
    {
        protected override IProjectSystem ProjectSystem { get; } = new CSharpCoreProjectSystem();

        protected override string ValidProject => TestProjects.ValidCoreProject;
        protected override int ValidProjectInitialFiles => 1;

        protected override IProjectId InvalidProjectId => new CSharpDirectoryProjectId(Guid.NewGuid().ToString());

        protected override IProjectId CreateValidProjectId(string randomString)
        {
            return new CSharpCoreProjectId(randomString);
        }
    }
}
