using System.Collections.Generic;
using System.Linq;

namespace ChilliCream.FluentConsole
{
    public class ConsoleRuntime
    {
        private readonly IConsole _console;
        private readonly TaskDefinitionResolver _resolver;

        internal ConsoleRuntime(IConsole console, IEnumerable<TaskDefinition> taskDefinitions)
        {
            if (console == null)
            {
                throw new System.ArgumentNullException(nameof(console));
            }

            if (taskDefinitions == null)
            {
                throw new System.ArgumentNullException(nameof(taskDefinitions));
            }

            _console = console;
            _resolver = new TaskDefinitionResolver(taskDefinitions.ToArray());
        }

        public void Run(string[] args)
        {
            CreateTask(args)?.Execute();
        }

        public ITask CreateTask(string[] args)
        {
            Argument[] arguments = ArgumentParser.Parse(args).ToArray();
            if (arguments.Any())
            {
                if (_resolver.TryResolve(arguments, out ResolvedTaskDefinitions resolvedTaskDefinitions))
                {
                    TaskFactory taskFactory = new TaskFactory(
                        resolvedTaskDefinitions.TaskDefinitions,
                        arguments.Skip(resolvedTaskDefinitions.ArgumentCount));
                    if (taskFactory.TryCreate(_console, out ITask task))
                    {
                        return task;
                    }
                }
            }
            return null;
        }
    }
}
