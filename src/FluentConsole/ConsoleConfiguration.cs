using System.Collections.Generic;

namespace ChilliCream.FluentConsole
{
    public class ConsoleConfiguration
    {
        private readonly List<TaskDefinition> _taskDefinitions = new List<TaskDefinition>();

        public IBindTask<TTask> Bind<TTask>()
            where TTask : class, ICommandLineTask
        {
            return new BindTask<TTask>(_taskDefinitions);
        }

        public ConsoleRuntime CreateRuntime()
        {
            return new ConsoleRuntime(ClassicConsole.Default, _taskDefinitions);
        }

        public ConsoleRuntime CreateRuntime(IConsole console)
        {
            return new ConsoleRuntime(console, _taskDefinitions);
        }
    }
}
