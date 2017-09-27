using System;
using System.IO;
using System.Linq;
using ChilliCream.FluentConsole;
using ChilliCream.Tracing.Generator.ProjectSystem;

namespace ChilliCream.Tracing.Generator.Tasks
{
    public sealed class CreateSolutionEventSources
        : ITask
    {
        private string _templateDirectory;

        public CreateSolutionEventSources()
            : this(ConsoleEnvironment.TemplateDirectory)
        { }

        public CreateSolutionEventSources(string templateDirectory)
        {
            if (string.IsNullOrEmpty(templateDirectory))
            {
                throw new ArgumentNullException(nameof(templateDirectory));
            }
            _templateDirectory = templateDirectory;
        }

        public string FileOrDirectoryName { get; set; }
        public Language Language { get; set; } = Language.CSharp;
        public string TemplateName { get; set; }
        public bool Recursive { get; set; }

        public void Execute()
        {
            if (Directory.Exists(FileOrDirectoryName))
            {
                string[] solutionFileNames = Directory.GetFiles(FileOrDirectoryName, "*.sln",
                    Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                foreach (string solutionFileName in solutionFileNames)
                {
                    ProcessSolution(Solution.Create(solutionFileName));
                }
            }
            else if (File.Exists(FileOrDirectoryName))
            {
                ProcessSolution(Solution.Create(FileOrDirectoryName));
            }
        }

        private void ProcessSolution(Solution solution)
        {
            string templateContent = TemplateResolver.Resolve(_templateDirectory, TemplateName, Language);

            foreach (Project project in solution.Projects)
            {
                EventSourceBuilder eventSourceBuilder = new EventSourceBuilder(project, project, templateContent);
                eventSourceBuilder.Build();
            }

            if (solution.Projects.Any(t => t.UpdatedDocumets.Any()))
            {
                solution.CommitChanges();
            }
        }
    }
}
