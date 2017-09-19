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
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folders">The folders.</param>
        /// <returns>Returns a new document object.</returns>
        /// <exception cref="ArgumentException">
        /// The document content cannot be null or empty or consist only of a whitespace.
        /// or
        /// The file name cannot be null or empty or consist only of a whitespace.
        /// </exception>
        public Document UpdateDocument(string content, string fileName, IEnumerable<string> folders)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                // Todo : resources
                throw new ArgumentException("The document content cannot be null or empty or consist only of a whitespace.", nameof(content));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                // Todo : resources
                throw new ArgumentException("The file name cannot be null or empty or consist only of a whitespace.", nameof(fileName));
            }

            Document document = Document.Create(content, fileName, folders);
            _documents[document.Id] = document;
            _updatedDocuments.Add(document.Id);
            return document;
        }

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
    }
}
