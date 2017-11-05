using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Thor.Generator.ProjectSystem
{
    /// <summary>
    /// Provides extensions to the <see cref="Document"/>-object.
    /// </summary>
    public static class DocumentExtensions
    {
        /// <summary>
        /// Creates a full file path for the specified <paramref name="document"/>
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="rootDirectory">The project root directory.</param>
        /// <returns>
        /// Returns a full file path for the specified <paramref name="document"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="document"/> is <c>null</c>
        /// or
        /// <paramref name="rootDirectory"/> is <c>null</c>
        /// or
        /// <paramref name="rootDirectory"/> is <see cref="string.Empty"/>
        /// or
        /// <paramref name="rootDirectory"/> is a whitespace.
        /// </exception>
        public static string CreateFilePath(this Document document, string rootDirectory)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if(string.IsNullOrWhiteSpace(rootDirectory))
            {
                throw new ArgumentNullException(nameof(rootDirectory));
            }

            return Path.Combine(rootDirectory, document.Id.ToString());
        }
    }
}
