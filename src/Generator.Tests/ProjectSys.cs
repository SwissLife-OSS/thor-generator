using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Construction;
using Xunit;

namespace ChilliCream.Logging.Generator
{
    public class ProjectSys
    {
        private string s = @"C:\Work\EventSourceGenerator\src\Generator\Generator.csproj";
        private string b = @"C:\Work\BusinessProcess-Engine\Engine\EngineConsole\EngineConsole.csproj";

        [Fact]
        public void Test()
        {
            ProjectRootElement p = ProjectRootElement.Open(s);
            ProjectRootElement pb = ProjectRootElement.Open(b);


        }
    }
}
