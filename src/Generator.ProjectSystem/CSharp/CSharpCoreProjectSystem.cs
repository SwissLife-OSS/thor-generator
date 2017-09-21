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

        public override bool CanHandle(string projectFileOrDirectoryName)
        {
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

        public override void CommitChanges(Project project)
        {
            CSharpClassicProjectId projectId = (CSharpClassicProjectId)project.Id;
            string projectRootDirectory = Path.GetDirectoryName(projectId.FileName);

            foreach (DocumentId updatedDocumentId in project.UpdatedDocumets)
            {
                Document updatedDocument = project.GetDocument(updatedDocumentId);
                string documentFileName = updatedDocument.CreateFilePath(projectRootDirectory);

                // update code file
                string directory = Path.GetDirectoryName(documentFileName);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.WriteAllText(documentFileName, updatedDocument.GetContent());
            }
        }
    }
}
