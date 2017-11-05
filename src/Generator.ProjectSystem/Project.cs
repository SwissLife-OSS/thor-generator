using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Thor.Generator.ProjectSystem.CSharp;

namespace Thor.Generator.ProjectSystem
{
    /// <summary>
    /// Represents a .net project.
    /// </summary>
    public sealed class Project
    {
        private readonly HashSet<DocumentId> _updatedDocuments = new HashSet<DocumentId>();
        private ImmutableDictionary<DocumentId, Document> _documents;

        private Project(IProjectId projectId, IEnumerable<Document> documents)
        {
            Id = projectId;
            _documents = documents.Distinct().ToImmutableDictionary(t => t.Id);
            Documents = new ReadOnlyDocumentCollection(_documents);
        }

        /// <summary>
        /// Gets the project identifier.
        /// </summary>
        /// <value>The project identifier.</value>
        public IProjectId Id { get; }

        /// <summary>
        /// Gets the documents of this project.
        /// </summary>
        /// <value>The documents.</value>
        public IReadOnlyCollection<Document> Documents { get; private set; }

        /// <summary>
        /// Gets the identifiers of the added and updated documets.
        /// </summary>
        /// <value>The identifiers of the added abd updated documets.</value>
        public IReadOnlyCollection<DocumentId> UpdatedDocumets => _updatedDocuments;

        /// <summary>
        /// Gets a document by its <paramref name="documentId"/>.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <returns>
        /// Returns a document that is associated with the specified <paramref name="documentId"/>.
        /// </returns>
        public Document GetDocument(DocumentId documentId)
        {
            if (documentId == null)
            {
                throw new ArgumentNullException(nameof(documentId));
            }

            if (_documents.ContainsKey(documentId))
            {
                return _documents[documentId];
            }
            return null;
        }

        /// <summary>
        /// Adds or replaces a document of this project.
        /// If there is already a project in the specified
        /// project folder with the specified file name the
        /// document will be replaced; otherwise a new document
        /// will be added.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="name">Name of the file.</param>
        /// <param name="folders">The folders.</param>
        /// <returns>Returns a new document object.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="content" /> is <c>null</c>
        /// or
        /// <paramref name="content" /> is <see cref="string.Empty" />
        /// or
        /// <paramref name="content" /> consists of a whitespace
        /// or
        /// <paramref name="name" /> is <c>null</c>
        /// or
        /// <paramref name="name" /> is <see cref="string.Empty" />
        /// or
        /// <paramref name="name" /> consists of a whitespace
        /// or
        /// <paramref name="folders" /> is <c>null</c>.
        /// </exception>
        public Document UpdateDocument(string content, string name, IEnumerable<string> folders)
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

            Document document = Document.Create(content, name, folders);

            // check if an update of the document is necessary
            if (_documents.TryGetValue(document.Id, out Document currentVersion))
            {
                string currentContent = currentVersion.GetContent();
                if(currentContent.Equals(content, StringComparison.Ordinal))
                {
                    return currentVersion;
                }
            }

            // todo: remove and than add due to equality. this issue will be fixed later.
            _documents = _documents.Remove(document.Id).Add(document.Id, document);
            _updatedDocuments.Add(document.Id);
            Documents = new ReadOnlyDocumentCollection(_documents);

            return document;
        }

        /// <summary>
        /// Commits changes to the file system.
        /// </summary>
        public void CommitChanges()
        {
            CSharpProjectSystems.TryCommitChanges(this);
        }

        #region Project Factory

        /// <summary>
        /// Creates a new project object.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="documents">The project documents.</param>
        /// <returns>Returns a new project object.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="projectId"/> is <c>null</c>
        /// or
        /// <paramref name="documents"/> is <c>null</c>.
        /// </exception>
        public static Project Create(IProjectId projectId, IEnumerable<Document> documents)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            return new Project(projectId, documents);
        }

        public static bool TryParse(string fileOrDirectoryName, out Project project)
        {
            if (string.IsNullOrEmpty(fileOrDirectoryName))
            {
                throw new ArgumentNullException(nameof(fileOrDirectoryName));
            }

            return CSharpProjectSystems.TryOpenProject(fileOrDirectoryName, out project);
        }

        #endregion

        #region Nested Types

        private class ReadOnlyDocumentCollection
            : IReadOnlyCollection<Document>
        {
            private readonly ImmutableDictionary<DocumentId, Document> _documents;

            public ReadOnlyDocumentCollection(ImmutableDictionary<DocumentId, Document> documents)
            {
                _documents = documents;
            }

            public int Count => _documents.Count();

            public IEnumerator<Document> GetEnumerator()
            {
                return _documents.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        #endregion
    }
}
