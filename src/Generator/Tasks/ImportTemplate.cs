using System;
using System.IO;
using ChilliCream.FluentConsole;

namespace ChilliCream.Tracing.Generator.Tasks
{
    public sealed class ImportTemplate
        : ITask
    {
        private readonly IConsole _console;
        private readonly string _templateDirectory;

        public ImportTemplate(IConsole console)
            : this(console, ConsoleEnvironment.TemplateDirectory)
        { }

        public ImportTemplate(IConsole console, string templateDirectory)
        {
            if (console == null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (string.IsNullOrEmpty(templateDirectory))
            {
                throw new ArgumentNullException(nameof(templateDirectory));
            }

            _console = console;
            _templateDirectory = templateDirectory;
        }

        public string FileName { get; set; }
        public string Name { get; set; }

        public void Execute()
        {
            FileName = _console.GetFullPath(FileName);

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
