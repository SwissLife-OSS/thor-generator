using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;

namespace ChilliCream.Tracing.Generator.Roslyn
{
    public class GeneratorWorkspace
        : Workspace
    {
        internal GeneratorWorkspace(HostServices host)
            : base(host, nameof(GeneratorWorkspace))
        {
        }
    }
}
