using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChilliCream.Tracing.Generator.ProjectSystem.Tests
{
    public static class TestProjects
    {
        private static string GetAssemblyDirectory()
            => Path.GetDirectoryName(typeof(TestProjects).Assembly.Location);
        private static string GetResourceDirectory()
            => Path.GetFullPath(Path.Combine(GetAssemblyDirectory(),
                "..", "..", "..", "..", "..", "resources"));

        public static readonly string ValidClassicProject = Path.Combine(GetResourceDirectory(), "ValidClassicProject.zip");
        public static readonly string ValidCoreProject = Path.Combine(GetResourceDirectory(), "ValidCoreProject.zip");
        public static readonly string ValidDirectoryProject = Path.Combine(GetResourceDirectory(), "ValidDirectoryProject.zip");
    }
}
