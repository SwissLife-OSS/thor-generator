using System;
using System.IO;
using ChilliCream.FluentConsole;

namespace ChilliCream.Tracing.Generator.Tasks
{
    public sealed class ImportTemplate
        : ITask
    {
        private string _templateDirectory;

        public ImportTemplate()
            : this(ConsoleEnvironment.TemplateDirectory)
        { }

        public ImportTemplate(string templateDirectory)
        {
            if (string.IsNullOrEmpty(templateDirectory))
            {
                throw new ArgumentNullException(nameof(templateDirectory));
            }
            _templateDirectory = templateDirectory;
        }

        public string FileName { get; set; }
        public string Name { get; set; }

        public void Execute()
        {
            if (File.Exists(FileName))
            {
                string templateContent = File.ReadAllText(FileName);

                if (!Directory.Exists(_templateDirectory))
                {
                    Directory.CreateDirectory(_templateDirectory);
                }

                string templateFileName = TemplateResolver
                    .CreateTemplateFileName(_templateDirectory, Name);

                File.WriteAllText(templateFileName, templateContent);
            }
        }
    }
}
