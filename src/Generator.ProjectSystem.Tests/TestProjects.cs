using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChilliCream.Tracing.Generator.ProjectSystem.Tests
{
    public static class TestProjects
    {
        private static readonly string _resources = "resources";
        private static string GetAssemblyDirectory()
            => Path.GetDirectoryName(typeof(TestProjects).Assembly.Location);

        private static string GetResourceDirectory()
        {
            string path = Path.Combine(GetAssemblyDirectory(),
                "..", "..", "..", "..", "..");
            path = Path.GetFullPath(path);

            if (Directory.Exists(Path.Combine(path, _resources)))
            {
                return Path.Combine(path, _resources);
            }

            path = Path.Combine(path, "..", "..", "..", "..");
            path = Path.GetFullPath(path);

            if (Directory.Exists(Path.Combine(path, _resources)))
            {
                return Path.Combine(path, _resources);
            }

            throw new DirectoryNotFoundException("Could not find the resource directory. ");
        }
        public static readonly string ValidClassicProject = Path.Combine(GetResourceDirectory(), "ValidClassicProject.zip");
        public static readonly string ValidCoreProject = Path.Combine(GetResourceDirectory(), "ValidCoreProject.zip");
        public static readonly string ValidDirectoryProject = Path.Combine(GetResourceDirectory(), "ValidDirectoryProject.zip");
    }
}
