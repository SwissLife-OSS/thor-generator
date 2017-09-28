using System;
using System.IO;
using ChilliCream.FluentConsole;
using ChilliCream.Tracing.Generator.ProjectSystem;
using ChilliCream.Tracing.Generator.Properties;

namespace ChilliCream.Tracing.Generator.Tasks
{
    /// <summary>
    /// This command line task exports default event source templates or custom event source template to a specified file.
    /// </summary>
    /// <seealso cref="ChilliCream.FluentConsole.ICommandLineTask" />
    public class ExportTemplateTask
        : ICommandLineTask
    {
        private readonly TemplateStorage _templateStorage;
        private readonly IConsole _console;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportTemplateTask"/> class.
        /// </summary>
        /// <param name="console">The console.</param>
        public ExportTemplateTask(IConsole console)
            : this(console, new TemplateStorage())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportTemplateTask"/> class.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="templateStorage">The template storage.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="console"/> is <c>null</c>.
        /// or
        /// <paramref name="templateStorage"/> is <c>null</c>.
        /// </exception>
        public ExportTemplateTask(IConsole console, TemplateStorage templateStorage)
        {
            if (console == null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (templateStorage == null)
            {
                throw new ArgumentNullException(nameof(templateStorage));
            }

            _console = console;
            _templateStorage = templateStorage;
        }

        /// <summary>
        /// Gets or sets the name of the file to which the specified template shall be exported.
        /// </summary>
        /// <value>The name of the template file.</value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the language of the default template 
        /// which will be used to export if 
        /// no custom template name was provided.
        /// </summary>
        /// <value>The default template language.</value>
        public Language Language { get; set; } = Language.CSharp;

        /// <summary>
        /// Gets or sets the name of a custom template that shall be exported.
        /// </summary>
        /// <value>The custom template name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Executes this command line task.
        /// </summary>
        public int Execute()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                _console.Error(Resources.NoTemplateFile);
                return CustomErrorCodes.MissingArgument;
            }

            // ensure that we have rooted file or directory paths
            FileName = _console.GetFullPath(FileName);

            if (string.IsNullOrEmpty(Name))
            {
                // export default template
                SaveTemplateToFile(_templateStorage.GetTemplate(Language));
                return CommandLineResults.OK;
            }
            else if (_templateStorage.CustomTemplateExists(Name))
            {
                // export custom template
                SaveTemplateToFile(_templateStorage.GetCustomTemplate(Name));
                return CommandLineResults.OK;
            }
            else
            {
                // could not find custom template
                _console.Error(string.Format(Resources.TemplateNotExists, Name));
                return CustomErrorCodes.TemplateNotExists;
            }
        }

        private void SaveTemplateToFile(string template)
        {
            string exportDirectory = Path.GetDirectoryName(FileName);

            // ensure file directory exists before writing file
            if (!Directory.Exists(exportDirectory))
            {
                Directory.CreateDirectory(exportDirectory);
            }

            // write template to file
            File.WriteAllText(FileName, template);
        }
    }
}
