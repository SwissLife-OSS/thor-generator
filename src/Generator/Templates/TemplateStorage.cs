using System;
using System.IO;
using Thor.Generator.ProjectSystem;

namespace Thor.Generator.Templates
{
    /// <summary>
    /// This class provides access to the event source templates.
    /// </summary>
    public class TemplateStorage
    {
        private static readonly string _applicationDirectory = Path.GetDirectoryName(typeof(TemplateStorage).Assembly.Location);

        private readonly string _defaultTemplateDirectory;
        private readonly string _customTemplateDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateStorage"/> class.
        /// </summary>
        public TemplateStorage()
            : this(_applicationDirectory)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateStorage"/> class.
        /// </summary>
        /// <param name="applicationDirectory">The application directory.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationDirectory"/> is <c>null</c>
        /// or
        /// <paramref name="applicationDirectory"/> is <see cref="string.Empty"/>.
        /// </exception>
        public TemplateStorage(string applicationDirectory)
        {
            if (string.IsNullOrEmpty(applicationDirectory))
            {
                throw new ArgumentNullException(nameof(applicationDirectory));
            }

            _customTemplateDirectory = Path.Combine(applicationDirectory, "Templates");
            _defaultTemplateDirectory = Path.Combine(_customTemplateDirectory, "Defaults");
        }

        /// <summary>
        /// Get the default template for the specified code <paramref name="language"/>.
        /// </summary>
        /// <param name="language">The code language.</param>
        /// <returns>
        /// Returns the default language for the specified <paramref name="language"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// The specified language is not supported yet.
        /// </exception>
        public Template GetTemplate(Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                    string templateFile = Path.Combine(_defaultTemplateDirectory, language + Template.TemplateExtension);
                    return Template.FromFile(templateFile);

                default:
                    throw new NotSupportedException($"{language} is not supported as template language.");
            }
        }

        /// <summary>
        /// Gets custom template by it's <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Returns the custom template with the specified <paramref name="name"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// A template with the specified <paramref name="name"/> does not exist.
        /// </exception>
        public Template GetCustomTemplate(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            string templateFile = Path.Combine(_customTemplateDirectory, name + Template.TemplateExtension);

            if (!File.Exists(templateFile))
            {
                throw new FileNotFoundException($"The template {name} could not be found.", templateFile);
            }

            return Template.FromFile(templateFile);
        }

        /// <summary>
        /// Persists the specified custom <paramref name="template"/> within the <see cref="TemplateStorage"/>.
        /// </summary>
        /// <param name="template">
        /// The template that shall be persisted.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="template"/> is <c>null</c>.
        /// </exception>
        public void SaveCustomTemplate(Template template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            template.Save(_customTemplateDirectory);
        }


        /// <summary>
        /// Checks if a template with the specified <paramref name="name"/> exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if a template with the specified <paramref name="name"/> exists, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is <c>null</c>.
        /// </exception>
        public bool CustomTemplateExists(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return File.Exists(Path.Combine(_customTemplateDirectory, name + Template.TemplateExtension));
        }
    }
}
