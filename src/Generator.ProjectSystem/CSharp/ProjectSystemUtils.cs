using System.IO;
using System.Linq;

namespace ChilliCream.Tracing.Generator.ProjectSystem.CSharp
{
    internal static class ProjectSystemUtils
    {
        public static Document CreateDocument(string fileName, int rootPathLength)
        {
            string[] relativePath = fileName.Substring(rootPathLength)
                .Split(Path.DirectorySeparatorChar);
            return Document.Create(() => File.ReadAllText(fileName),
                relativePath.Last(), relativePath.Take(relativePath.Length - 1));
        }
    }
}
