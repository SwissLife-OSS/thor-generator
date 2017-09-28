using System;
using System.IO;
using System.Linq;
using ChilliCream.FluentConsole;
using ChilliCream.Tracing.Generator.ProjectSystem;
using ChilliCream.Tracing.Generator.Properties;

namespace ChilliCream.Tracing.Generator.Tasks
{
    /// <summary>
    /// This command line task generates event sources based on solution files provided.
    /// </summary>
    /// <seealso cref="ChilliCream.Tracing.Generator.Tasks.EventSourceGeneratorTaskBase" />
    /// <seealso cref="ChilliCream.FluentConsole.ICommandLineTask" />
    public class SolutionEventSourceGeneratorTask
        : EventSourceGeneratorTaskBase
        , ICommandLineTask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionEventSourceGeneratorTask"/> class.
        /// </summary>
        /// <param name="console">The console.</param>
        public SolutionEventSourceGeneratorTask(IConsole console)
            : base(console, new TemplateStorage())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionEventSourceGeneratorTask"/> class.
        /// </summary>
        /// <param name="console">The console interface.</param>
        /// <param name="templateStorage">The template storage.</param>
        public SolutionEventSourceGeneratorTask(IConsole console, TemplateStorage templateStorage)
            : base(console, templateStorage)
        { }

        /// <summary>
        /// Gets or sets the name of a solution file or directory that contains solution files.
        /// </summary>
        /// <value>The name of a solution file or directory.</value>
        public string FileOrDirectoryName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SolutionEventSourceGeneratorTask"/> shall recursively search for solution files.
        /// </summary>
        /// <value><c>true</c> if this task shall search for solution files recursively; otherwise, <c>false</c>.</value>
        public bool Recursive { get; set; }

        /// <summary>
        /// Executes this command line task.
        /// </summary>
        public int Execute()
        {
            if (string.IsNullOrEmpty(FileOrDirectoryName))
            {
                FileOrDirectoryName = ".";
            }

            // ensure that we have rooted file or directory paths
            FileOrDirectoryName = Console.GetFullPath(FileOrDirectoryName);

            if (Directory.Exists(FileOrDirectoryName))
            {
                // search for solutions
                string[] solutionFileNames = Directory.GetFiles(FileOrDirectoryName, "*.sln",
                    Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                foreach (string solutionFileName in solutionFileNames)
                {
                    ProcessSolution(Solution.Create(solutionFileName));
                }
                return CommandLineResults.OK;
            }
            else if (File.Exists(FileOrDirectoryName))
            {
                // file that was provided is a solution
                ProcessSolution(Solution.Create(FileOrDirectoryName));
                return CommandLineResults.OK;
            }
            else
            {
                // the specified file or directory name is invalid
                Console.Error(Resources.FileOrDirectoryNotExists);
                return CustomErrorCodes.FileNotExists;
            }
        }

        private void ProcessSolution(Solution solution)
        {
            foreach (Project project in solution.Projects)
            {
                GenerateEventSources(project);
            }

            if (solution.Projects.Any(t => t.UpdatedDocumets.Any()))
            {
                solution.CommitChanges();
            }
        }
    }
}
