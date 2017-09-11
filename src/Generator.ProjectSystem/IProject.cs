using System.Collections.Generic;

namespace ChilliCream.Logging.Generator
{
    public interface IProject
    {
        IReadOnlyCollection<IDocument> Projects { get; }

        IDocument AddDocument(string content, string fileName, IEnumerable<string> folders);
    }
}
