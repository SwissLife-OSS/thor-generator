using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Thor.Generator.ProjectSystem
{
    /// <summary>
    /// This class represents a source code document of a project.
    /// </summary>
    /// <seealso cref="IEquatable{Document}" />
    public sealed class Document
        : IEquatable<Document>
    {
        private Func<string> _readDocument;

        private Document(string name, IEnumerable<string> folders,
            Func<string> readDocument)
        {
            Name = name;
            Folders = folders.Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();
            _readDocument = readDocument;
            Id = new DocumentId(name, folders);
        }

        /// <summary>
        /// Gets the <see cref="Document"/> identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public DocumentId Id { get; }

        /// <summary>
        /// Gets the <see cref="Document"/> file name.
        /// </summary>
        /// <value>The file name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the document folder stack.
        /// </summary>
        /// <value>The folders.</value>
        public IReadOnlyList<string> Folders { get; }

        /// <summary>
        /// Gets the document content.
        /// </summary>
        /// <returns>
        /// Returns a <see cref="String"/> representing the document content.
        /// </returns>
        public string GetContent()
        {
            return _readDocument();
        }

        /// <summary>
        /// Gets the root node of the syntax tree.
        /// </summary>
        /// <returns>
        /// Returns the root node of the syntax tree.
        /// </returns>
        public SyntaxNode GetSyntaxRoot()
        {
            string sourceText = GetContent();
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceText);
            return syntaxTree.GetRoot();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
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

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            return Equals(obj as Document);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Returns <see cref="Id"/> <see cref="System.String" /> repesantion.
        /// </summary>
        /// <returns>The <see cref="Id"/> <see cref="System.String" /> repesantion.</returns>
        public override string ToString()
        {
            return Id.ToString();
        }

        #region Document Factories

        /// <summary>
        /// Creates a new document object with a static <paramref name="content"/>.
        /// </summary>
        /// <param name="content">The document content.</param>
        /// <param name="name">The document name.</param>
        /// <param name="folders">The document folder.</param>
        /// <returns>Returns a new document object with a static content.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="content"/> is <c>null</c>
        /// or
        /// <paramref name="content"/> is <see cref="string.Empty"/>
        /// or
        /// <paramref name="content"/> is whitespace
        /// or
        /// <paramref name="name"/> is <c>null</c>
        /// or
        /// <paramref name="name"/> is <see cref="string.Empty"/>
        /// or
        /// <paramref name="name"/> is whitespace
        /// or
        /// <paramref name="folders"/> is <c>null</c>.
        /// </exception>
        public static Document Create(string content, string name, params string[] folders)
        {
            return Create(content, name, (IEnumerable<string>)folders);
        }

        /// <summary>
        /// Creates a new document object with a static <paramref name="content"/>.
        /// </summary>
        /// <param name="content">The document content.</param>
        /// <param name="name">The document name.</param>
        /// <param name="folders">The document folder.</param>
        /// <returns>Returns a new document object with a static content.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="content"/> is <c>null</c>
        /// or
        /// <paramref name="content"/> is <see cref="string.Empty"/>
        /// or
        /// <paramref name="content"/> is whitespace
        /// or
        /// <paramref name="name"/> is <c>null</c>
        /// or
        /// <paramref name="name"/> is <see cref="string.Empty"/>
        /// or
        /// <paramref name="name"/> is whitespace
        /// or
        /// <paramref name="folders"/> is <c>null</c>.
        /// </exception>
        public static Document Create(string content, string name, IEnumerable<string> folders)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (folders == null)
            {
                throw new ArgumentNullException(nameof(folders));
            }

            return new Document(name, folders, () => content);
        }

        /// <summary>
        /// Creates a new document object that specifies a delegate to load the content of the document.
        /// </summary>
        /// <param name="readDocument">The delegate to load the content of the document.</param>
        /// <param name="name">The document name.</param>
        /// <param name="folders">The document folder.</param>
        /// <returns>Returns a new document object with a static content.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="readDocument"/> is <c>null</c>
        /// or
        /// <paramref name="name"/> is <c>null</c>
        /// or
        /// <paramref name="name"/> is <see cref="string.Empty"/>
        /// or
        /// <paramref name="name"/> is whitespace
        /// or
        /// <paramref name="folders"/> is <c>null</c>.
        /// </exception>
        public static Document Create(Func<string> readDocument, string name, params string[] folders)
        {
            return Create(readDocument, name, (IEnumerable<string>)folders);
        }

        /// <summary>
        /// Creates a new document object that specifies a delegate to load the content of the document.
        /// </summary>
        /// <param name="readDocument">The delegate to load the content of the document.</param>
        /// <param name="name">The document name.</param>
        /// <param name="folders">The document folder.</param>
        /// <returns>Returns a new document object with a static content.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="readDocument"/> is <c>null</c>
        /// or
        /// <paramref name="name"/> is <c>null</c>
        /// or
        /// <paramref name="name"/> is <see cref="string.Empty"/>
        /// or
        /// <paramref name="name"/> is whitespace
        /// or
        /// <paramref name="folders"/> is <c>null</c>.
        /// </exception>
        public static Document Create(Func<string> readDocument, string name, IEnumerable<string> folders)
        {
            if (readDocument == null)
            {
                throw new ArgumentNullException(nameof(readDocument));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (folders == null)
            {
                throw new ArgumentNullException(nameof(folders));
            }

            return new Document(name, folders, readDocument);
        }

        #endregion
    }
}
