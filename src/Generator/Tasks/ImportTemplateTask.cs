using System;
using System.IO;
using ChilliCream.FluentConsole;
using ChilliCream.Tracing.Generator.Properties;
using ChilliCream.Tracing.Generator.Templates;

namespace ChilliCream.Tracing.Generator.Tasks
{
    /// <summary>
    /// This command line task imports custom event source template into the event source generator.
    /// </summary>
    /// <seealso cref="ChilliCream.FluentConsole.ICommandLineTask" />
    public class ImportTemplateTask
        : ICommandLineTask
    {
        private readonly TemplateStorage _templateStorage;
        private readonly IConsole _console;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportTemplateTask"/> class.
        /// </summary>
        /// <param name="console">The console.</param>
        public ImportTemplateTask(IConsole console)
            : this(console, new TemplateStorage())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportTemplateTask"/> class.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="templateStorage">The template storage.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="console"/> is <c>null</c>.
        /// or
        /// <paramref name="templateStorage"/> is <c>null</c>.
        /// </exception>
        public ImportTemplateTask(IConsole console, TemplateStorage templateStorage)
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
        /// Gets or sets the name of the template file that shall be imported.
        /// </summary>
        /// <value>The name of the template file.</value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name that the imported template shall have.
        /// </summary>
        /// <value>The template name.</value>
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

            if (string.IsNullOrEmpty(Name))
            {
                _console.Error(Resources.NoTemplateName);
                return CustomErrorCodes.MissingArgument;
            }

            // ensure that we have rooted file or directory paths
            FileName = _console.GetFullPath(FileName);

            if (!File.Exists(FileName))
            {
                _console.Error(string.Format(Resources.TemplateFileNotExists, FileName));
                return CustomErrorCodes.FileNotExists;
            }

            Template template = Template.FromFile(FileName);
            _templateStorage.SaveCustomTemplate(template);

            return CommandLineResults.OK;
        }
    }
}
