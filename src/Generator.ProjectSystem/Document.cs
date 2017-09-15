using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ChilliCream.Logging.Generator
{
    /// <summary>
    /// This class represents a document of a project that contains source code.
    /// </summary>
    /// <seealso cref="IEquatable{ChilliCream.Logging.Generator.Document}" />
    public class Document
        : IEquatable<Document>
    {
        private Func<CancellationToken, Task<string>> _readDocumentAsync;

        private Document(string name, IEnumerable<string> folders,
            Func<CancellationToken, Task<string>> readDocumentAsync)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (readDocumentAsync == null)
            {
                throw new ArgumentNullException(nameof(readDocumentAsync));
            }

            Name = name;
            Folders = folders?.ToArray() ?? Array.Empty<string>();
            _readDocumentAsync = readDocumentAsync;
            Id = new DocumentId(name, folders);
        }

        public DocumentId Id { get; }

        public string Name { get; }

        public string[] Folders { get; }

        public Task<string> GetContentAsync()
        {
            return GetContentAsync(CancellationToken.None);
        }

        public async Task<string> GetContentAsync(CancellationToken cancellationToken)
        {
            return await _readDocumentAsync(cancellationToken);
        }

        public Task<SyntaxNode> GetSyntaxRootAsync()
        {
            return GetSyntaxRootAsync(CancellationToken.None);
        }

        public async Task<SyntaxNode> GetSyntaxRootAsync(CancellationToken cancellationToken)
        {
            string sourceText = await _readDocumentAsync(cancellationToken);
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceText);
            return await syntaxTree.GetRootAsync();
        }

        public bool Equals(Document other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(other, this))
            {
                return true;
            }

            return other.Id.Equals(Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            return Equals(obj as Document);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        #region Document Factories

        public static Document Create(string content, string name, params string[] folders)
        {
            return Create(content, name, folders);
        }

        public static Document Create(string content, string name, IEnumerable<string> folders)
        {
            return new Document(name, folders, c => Task.FromResult(content));
        }

        public static Document Create(Func<CancellationToken, Task<string>> readDocumentAsync, string name, params string[] folders)
        {
            return new Document(name, folders, readDocumentAsync);
        }

        public static Document Create(Func<CancellationToken, Task<string>> readDocumentAsync, string name, IEnumerable<string> folders)
        {
            return new Document(name, folders, readDocumentAsync);
        }

        #endregion
    }
}
