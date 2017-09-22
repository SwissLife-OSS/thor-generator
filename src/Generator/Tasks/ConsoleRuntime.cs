using System;
using System.Collections.Generic;
using System.Linq;

namespace ChilliCream.Tracing.Generator.Tasks
{
    public class ConsoleRuntime
    {
        private readonly List<TaskDefinition> _tasks = new List<TaskDefinition>();

        public IBindTask<TTask> Bind<TTask>()
            where TTask : class, ITask
        {
            return new BindTask<TTask>(_tasks);
        }

        public void Run(IConsole console, string[] args)
        {
            Argument[] arguments = ArgumentParser.Parser(args).ToArray();
            if (arguments.Any())
            {
                TaskDefinition taskDefinition = _tasks.FirstOrDefault(t => t.IsDefault);
                bool skip = false;

                if (string.IsNullOrEmpty(arguments[0].Name))
                {
                    TaskDefinition temp = _tasks.FirstOrDefault(t =>
                       string.Equals(arguments[0].Value, t.Name,
                       System.StringComparison.OrdinalIgnoreCase));
                    taskDefinition = temp ?? taskDefinition;
                    skip = true;
                }

                if (taskDefinition != null)
                {
                    if (skip)
                    {
                        arguments = arguments.Skip(1).ToArray();
                        for (int i = 0; i < arguments.Length; i++)
                        {
                            arguments[i].Position = i;
                        }
                    }

                    ITask task = (ITask)taskDefinition.TaskType.Assembly
                        .CreateInstance(taskDefinition.TaskType.FullName);

                    foreach (Argument argument in arguments)
                    {
                        ArgumentDefinition argumentDefinition = null;
                        if (string.IsNullOrEmpty(argument.Name))
                        {
                            argumentDefinition = taskDefinition.Arguments.Values
                                .Concat(taskDefinition.PositionalArguments)
                                .FirstOrDefault(t => t.Position == argument.Position);
                        }
                        else
                        {
                            argumentDefinition = taskDefinition.Arguments.Values
                                .FirstOrDefault(t => t.Name.Equals(argument.Name,
                                    StringComparison.OrdinalIgnoreCase));
                        }

                        if (argumentDefinition == null)
                        {
                            throw new InvalidOperationException("Could not resolve argument.");
                        }

                        if (string.IsNullOrEmpty(argument.Value))
                        {
                            argumentDefinition.Property.SetValue(task, argument.IsSelected);
                        }
                        else
                        {
                            argumentDefinition.Property.SetValue(task, argument.Value);
                        }
                    }

                    task.Execute();
                }
            }
        }
    }
}
