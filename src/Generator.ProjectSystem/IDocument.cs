using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace ChilliCream.Logging.Generator
{
    public interface IDocument
    {
        string Name { get; }

        string[] Folders { get; }

        Task<SyntaxNode> GetSyntaxRootAsync(CancellationToken cancellationToken);
    }
}
