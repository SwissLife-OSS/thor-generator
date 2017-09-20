//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ChilliCream.Tracing.Generator.ProjectSystem.CSharp
//{
//    public class CSharpDirectoryProjectSystem
//        : IProjectSystem
//    {
//        public virtual bool CanHandle(string projectFileOrDirectoryName)
//        {
//            return Directory.Exists(projectFileOrDirectoryName);
//        }

//        public virtual bool CanHandle(IProjectId projectId)
//        {
//            return projectId is CSharpDirectoryProjectId;
//        }

//        protected virtual IProjectId CreateProjectId(string projectFileOrDirectoryName)
//        {
//            return new CSharpDirectoryProjectId(projectFileOrDirectoryName);
//        }

//        public Task<Project> OpenAsync(string projectFileOrDirectoryName)
//        {
//            int rootPathLength = projectFileOrDirectoryName.Length;
//            IProjectId projectId = CreateProjectId(projectFileOrDirectoryName);
//            HashSet<Document> documents = new HashSet<Document>();

//            foreach (string file in Directory.GetFiles(projectFileOrDirectoryName,
//                "*.cs", SearchOption.AllDirectories))
//            {
//                documents.Add(ProjectSystemUtils.CreateDocument(file, rootPathLength));
//            }

//            return Task.FromResult(Project.Create(projectId, documents));
//        }

//        public async Task CommitChangesAsync(Project project)
//        {
//            foreach (DocumentId documentId in project.UpdatedDocumets)
//            {
//                Document document = project.GetDocument(documentId);
//                string fileName = document.CreateFilePath(project.Id.ToString());
//                string content = await document.GetContentAsync();
//                File.WriteAllText(fileName, content);
//            }
//        }
//    }
//}
