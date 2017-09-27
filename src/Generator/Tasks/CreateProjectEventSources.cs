using ChilliCream.FluentConsole;
using ChilliCream.Tracing.Generator.ProjectSystem;

namespace ChilliCream.Tracing.Generator.Tasks
{
    public class CreateProjectEventSources
        : ITask
    {

        public string SourceProject { get; set; }
        public string TargetProject { get; set; }

        public void Execute()
        {
            if (string.IsNullOrEmpty(TargetProject))
            {
                TargetProject = SourceProject;
            }

            if (Project.TryParse(SourceProject, out Project source)
                && Project.TryParse(TargetProject, out Project target))
            {
                EventSourceBuilder eventSourceBuilder = new EventSourceBuilder(source, target);
                eventSourceBuilder.Build();

                target.CommitChanges();
            }
        }
    }
}
