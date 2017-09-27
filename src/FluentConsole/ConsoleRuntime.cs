using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChilliCream.FluentConsole
{
    public class ConsoleRuntime
    {
        private readonly List<TaskDefinition> _tasks = new List<TaskDefinition>();
        private readonly TaskDefinitionResolver _resolver;

        public ConsoleRuntime()
        {
            _resolver = new TaskDefinitionResolver(_tasks);
        }

        public IBindTask<TTask> Bind<TTask>()
            where TTask : class, ITask
        {
            return new BindTask<TTask>(_tasks);
        }

        public void Run(IConsole console, string[] args)
        {
            Argument[] arguments = ArgumentParser.Parse(args).ToArray();
            if (arguments.Any())
            {
                if (_resolver.TryResolve(arguments, out ResolvedTaskDefinitions resolvedTaskDefinitions))
                {
                    TaskFactory taskFactory = new TaskFactory(
                        resolvedTaskDefinitions.TaskDefinitions,
                        arguments.Skip(resolvedTaskDefinitions.ArgumentCount));
                    if (taskFactory.TryCreate(console, out ITask task))
                    {
                        task.Execute();
                    }
                }
            }
        }
    }
}
