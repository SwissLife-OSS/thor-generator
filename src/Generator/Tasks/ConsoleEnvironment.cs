using System;
using System.Collections.Generic;
using System.IO;

namespace ChilliCream.Tracing.Generator.Tasks
{
    internal static class ConsoleEnvironment
    {
        public static readonly string ConsoleDirectory
            = Path.GetDirectoryName(typeof(ConsoleEnvironment).Assembly.Location);

        public static readonly string TemplateDirectory
            = Path.Combine(ConsoleDirectory, "templates");
    }
}
