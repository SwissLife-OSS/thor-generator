using System;
using ChilliCream.FluentConsole;
using ChilliCream.Tracing.Generator.ProjectSystem;
using ChilliCream.Tracing.Generator.Properties;

namespace ChilliCream.Tracing.Generator.Tasks
{
    /// <summary>
    /// This command line task generates event sources based on projects provided.
    /// </summary>
    /// <seealso cref="ChilliCream.Tracing.Generator.Tasks.EventSourceGeneratorTaskBase" />
    /// <seealso cref="ChilliCream.FluentConsole.ICommandLineTask" />
    public class ProjectEventSourceGeneratorTask
        : EventSourceGeneratorTaskBase
        , ICommandLineTask
    {
        private readonly TemplateStorage _templateStorage;
        private readonly IConsole _console;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectEventSourceGeneratorTask"/> class.
        /// </summary>
        /// <param name="console">The console.</param>
        public ProjectEventSourceGeneratorTask(IConsole console)
            : base(console, new TemplateStorage())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectEventSourceGeneratorTask"/> class.
        /// </summary>
        /// <param name="console">The console interface.</param>
        /// <param name="templateStorage">The template storage.</param>
        public ProjectEventSourceGeneratorTask(IConsole console, TemplateStorage templateStorage)
            : base(console, templateStorage)
        { }

        /// <summary>
        /// Gets or sets the source project. 
        /// The source project contains the event source 
        /// interfaces that shall be used as blue prints.
        /// </summary>
        /// <value>The source project.</value>
        public string SourceProject { get; set; }

        /// <summary>
        /// Gets or sets the target project.
        /// The generated event sources will be 
        /// stored into the target project.
        /// </summary>
        /// <value>The target project.</value>
        public string TargetProject { get; set; }

        /// <summary>
        /// Executes this command line task.
        /// </summary>
        public int Execute()
        {
            if (string.IsNullOrEmpty(SourceProject))
            {
                Console.Error(Resources.NoSourceProject);
                return CustomErrorCodes.MissingArgument;
            }

            if (string.IsNullOrEmpty(TargetProject))
            {
                TargetProject = SourceProject;
            }

            // ensure that we have rooted file or directory paths
            SourceProject = _console.GetFullPath(SourceProject);
            TargetProject = _console.GetFullPath(TargetProject);

            if (Project.TryParse(SourceProject, out Project source)
                && Project.TryParse(TargetProject, out Project target))
            {
                GenerateEventSources(source, target);
                target.CommitChanges();
            }

            return CommandLineResults.OK;
        }

    }
}
