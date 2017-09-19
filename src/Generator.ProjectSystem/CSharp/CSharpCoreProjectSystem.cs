using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ChilliCream.Tracing.Generator.ProjectSystem.CSharp
{
    public class CSharpCoreProjectSystem
        : CSharpDirectoryProjectSystem
    {
        public override bool CanHandle(string projectFileOrDirectoryName)
        {
            if (File.Exists(projectFileOrDirectoryName))
            {
                try
                {
                    XDocument document = XDocument.Load(projectFileOrDirectoryName, LoadOptions.None);
                    XElement element = document.Elements().FirstOrDefault();
                    return element != null
                        && element.Name == "Project"
                        && element.Attribute("Sdk")?.Value == "Microsoft.NET.Sdk";
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public override bool CanHandle(IProjectId projectId)
        {
            return base.CanHandle(projectId);
        }

        protected override IProjectId CreateProjectId(string projectFileOrDirectoryName)
        {
            return base.CreateProjectId(projectFileOrDirectoryName);
        }
    }
}
