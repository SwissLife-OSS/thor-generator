using System;
using System.IO;
using ChilliCream.FluentConsole;
using ChilliCream.Tracing.Generator.ProjectSystem;

namespace ChilliCream.Tracing.Generator.Tasks
{
    public sealed class ExportTemplate
        : ITask
    {
        private readonly IConsole _console;
        private readonly string _templateDirectory;

        public ExportTemplate(IConsole console)
            : this(console, ConsoleEnvironment.TemplateDirectory)
        { }

        public ExportTemplate(IConsole console, string templateDirectory)
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
        public Language Language { get; set; } = Language.CSharp;
        public string Name { get; set; }

        public void Execute()
        {
            FileName = _console.GetFullPath(FileName);

            string templateContent = TemplateResolver.Resolve(_templateDirectory, Name, Language);
            if (!string.IsNullOrEmpty(templateContent))
            {
                string exportDirectory = Path.GetDirectoryName(FileName);
                if (!Directory.Exists(exportDirectory))
                {
                    Directory.CreateDirectory(exportDirectory);
                }
                File.WriteAllText(FileName, exportDirectory);
            }
        }
    }
}
