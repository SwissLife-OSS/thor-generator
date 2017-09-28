using System;
using System.IO;
using ChilliCream.Tracing.Generator.ProjectSystem;

namespace ChilliCream.Tracing.Generator
{
    public class TemplateStorage
    {
        private static readonly string _mustache = ".mustache";
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

            _defaultTemplateDirectory = Path.Combine(applicationDirectory, "Resources");
            _customTemplateDirectory = Path.Combine(applicationDirectory, "Templates");
        }


        public string GetTemplate(Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                    string templateFile = Path.Combine(_defaultTemplateDirectory, language + _mustache);
                    return File.ReadAllText(templateFile);

                default:
                    throw new NotSupportedException($"{language} is not supported as template language.");
            }
        }

        public string GetCustomTemplate(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            string templateFile = Path.Combine(_customTemplateDirectory, name + _mustache);

            if (!File.Exists(templateFile))
            {
                throw new FileNotFoundException($"The template {name} could not be found.", templateFile);
            }

            return File.ReadAllText(templateFile);
        }

        public void SaveCustomTemplate(string name, string content)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!Directory.Exists(_customTemplateDirectory))
            {
                Directory.CreateDirectory(_customTemplateDirectory);
            }

            string templateFile = Path.Combine(_customTemplateDirectory, name + _mustache);
            File.WriteAllText(templateFile, content);
        }

        public bool CustomTemplateExists(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return File.Exists(Path.Combine(_customTemplateDirectory, name + _mustache));
        }
    }
}
