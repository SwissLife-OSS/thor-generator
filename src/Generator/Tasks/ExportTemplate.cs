using System;
using System.IO;
using ChilliCream.FluentConsole;
using ChilliCream.Tracing.Generator.ProjectSystem;

namespace ChilliCream.Tracing.Generator.Tasks
{
    public sealed class ExportTemplate
        : ITask
    {
        private string _templateDirectory;

        public ExportTemplate()
            : this(ConsoleEnvironment.TemplateDirectory)
        { }

        public ExportTemplate(string templateDirectory)
        {
            if (string.IsNullOrEmpty(templateDirectory))
            {
                throw new ArgumentNullException(nameof(templateDirectory));
            }
            _templateDirectory = templateDirectory;
        }

        public string FileName { get; set; }
        public Language Language { get; set; } = Language.CSharp;
        public string Name { get; set; }

        public void Execute()
        {
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
