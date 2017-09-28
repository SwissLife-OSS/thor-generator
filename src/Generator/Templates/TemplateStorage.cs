using System;
using System.IO;
using ChilliCream.Tracing.Generator.ProjectSystem;

namespace ChilliCream.Tracing.Generator.Templates
{
    public class TemplateStorage
    {
        private static readonly string _applicationDirectory = Path.GetDirectoryName(typeof(TemplateStorage).Assembly.Location);

        private readonly string _defaultTemplateDirectory;
        private readonly string _customTemplateDirectory;

        public TemplateStorage()
            : this(_applicationDirectory)
        { }

        public TemplateStorage(string applicationDirectory)
        {
            if (string.IsNullOrEmpty(applicationDirectory))
            {
                throw new ArgumentNullException(nameof(applicationDirectory));
            }

            _defaultTemplateDirectory = Path.Combine(applicationDirectory, "Templates", "Defaults");
            _customTemplateDirectory = Path.Combine(applicationDirectory, "Templates");
        }


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

        public void SaveCustomTemplate(Template template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            template.Save(_customTemplateDirectory);
        }

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
