using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ChilliCream.Tracing.Generator.ProjectSystem.CSharp
{
    public class CSharpCoreProjectSystem
        : ProjectSystem<CSharpCoreProjectId>
    {
        protected override IProjectId CreateProjectId(string projectFileOrDirectoryName)
        {
            return new CSharpCoreProjectId(projectFileOrDirectoryName);
        }

        protected override string GetProjectDirectory(string projectFileOrDirectoryName)
        {
            return Path.GetDirectoryName(projectFileOrDirectoryName);
        }

        protected override IEnumerable<string> GetProjectFiles(string projectFileOrDirectoryName)
        {
            string projectDirectory = GetProjectDirectory(projectFileOrDirectoryName);
            return Directory.GetFiles(projectDirectory,
                CSharpConstants.FileFilter, SearchOption.AllDirectories);
        }

        protected override string GetFileOrDirectoryName(IProjectId projectId)
        {
            return ((CSharpCoreProjectId)projectId).FileName;
        }

        public override bool CanHandle(string projectFileOrDirectoryName)
        {
            if (string.IsNullOrEmpty(projectFileOrDirectoryName))
            {
                throw new ArgumentNullException(nameof(projectFileOrDirectoryName));
            }

            if (File.Exists(projectFileOrDirectoryName))
            {
                try
                {
                    XDocument document = XDocument.Load(projectFileOrDirectoryName, LoadOptions.None);
                    XElement element = document.Elements().FirstOrDefault();
                    return element != null
                        && element.Name == CSharpConstants.ProjectElementName
                        && element.Attribute(CSharpConstants.SdkAttributeName)?.Value == CSharpConstants.SdkAttributeValue;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
