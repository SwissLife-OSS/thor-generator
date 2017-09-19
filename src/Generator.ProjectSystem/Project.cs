using System;
using System.Collections.Generic;
using System.Linq;

namespace ChilliCream.Tracing.Generator.ProjectSystem
{
    public class Project
    {
        private readonly HashSet<DocumentId> _updatedDocuments = new HashSet<DocumentId>();
        private readonly Dictionary<DocumentId, Document> _documents = new Dictionary<DocumentId, Document>();

        private Project(IProjectId projectId, IEnumerable<Document> documents)
        {
            Id = projectId;
            _documents = documents.Distinct().ToDictionary(t => t.Id);
        }

        public IProjectId Id { get; }
        public IReadOnlyCollection<Document> Documents => _documents.Values;
        public IReadOnlyCollection<DocumentId> UpdatedDocumets => _updatedDocuments;

        public Document GetDocument(DocumentId documentId)
        {
            return _documents[documentId];
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
        /// <paramref name="content" /> is <see cref="string.Empty">
        /// or
        /// <paramref name="content" /> consists of a whitespace
        /// or
        /// <paramref name="name" /> is <c>null</c>
        /// or
        /// <paramref name="name" /> is <see cref="string.Empty">
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

            if(folders == null)
            {
                throw new ArgumentNullException(nameof(folders));
            }

            Document document = Document.Create(content, name, folders);
            _documents[document.Id] = document;
            _updatedDocuments.Add(document.Id);
            return document;
        }

        #region Project Factory

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

        #endregion
    }
}
