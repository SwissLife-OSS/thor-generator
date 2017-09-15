using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChilliCream.Logging.Generator
{
    public class DirectoryProjectHandler
        : IProjectHandler
    {
        public bool CanHandle(string projectFileOrDirectoryName)
        {
            return Directory.Exists(projectFileOrDirectoryName);
        }

        public Task<Project> OpenAsync(string projectFileOrDirectoryName)
        {
            int rootPathLength = projectFileOrDirectoryName.Length;
            DirectoryProjectId projectId = new DirectoryProjectId(projectFileOrDirectoryName);
            List<Document> documents = new List<Document>();

            foreach (string file in Directory.GetFiles(projectFileOrDirectoryName,
                "*.cs", SearchOption.AllDirectories))
            {
                string[] relativePath = file.Substring(rootPathLength).Split(Path.DirectorySeparatorChar);
                Document document = Document.Create(() => File.ReadAllText(file),
                    relativePath.Last(), relativePath.Take(relativePath.Length - 1));
            }

            return Task.FromResult(Project.Create(projectId, documents));
        }

        public async Task CommitChangesAsync(Project project)
        {
            foreach (DocumentId documentId in project.UpdatedDocumets)
            {
                Document document = project.GetDocument(documentId);
                string fileName = Path.Combine(project.Id.ToString(),
                    Path.Combine(document.Folders), document.Name);
                string content = await document.GetContentAsync();
                File.WriteAllText(fileName, content);
            }
        }
    }
}
