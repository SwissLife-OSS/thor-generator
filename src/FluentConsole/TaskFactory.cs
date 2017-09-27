using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChilliCream.FluentConsole
{
    internal sealed class TaskFactory
    {
        private readonly TaskDefinition[] _taskDefinitions;
        private readonly Argument[] _arguments;

        public TaskFactory(IEnumerable<TaskDefinition> taskDefinitions, IEnumerable<Argument> arguments)
        {
            if (taskDefinitions == null)
            {
                throw new ArgumentNullException(nameof(taskDefinitions));
            }

            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            _taskDefinitions = taskDefinitions.ToArray();
            _arguments = arguments.ToArray();
        }

        public bool TryCreate(IConsole console, out ITask task)
        {
            foreach (TaskDefinition taskDefinition in _taskDefinitions)
            {
                if (TryResolveCompatibleTaskDefinition(taskDefinition, out Action<ITask> initializeTaskProperties)
                    && TryCreateTaskInstance(console, taskDefinition.TaskType, out task))
                {
                    initializeTaskProperties(task);
                    return true;
                }
            }

            task = null;
            return false;
        }

        private bool TryResolveCompatibleTaskDefinition(TaskDefinition taskDefinition, out Action<ITask> initializeTaskProperties)
        {
            HashSet<ArgumentDefinition> providedArguments = new HashSet<ArgumentDefinition>();
            List<Action<ITask>> taskPropertyInitializers = new List<Action<ITask>>();

            foreach (Argument argument in _arguments)
            {
                ArgumentDefinition argumentDefinition = null;
                if (string.IsNullOrEmpty(argument.Name))
                {
                    argumentDefinition = taskDefinition.PositionalArguments
                        .FirstOrDefault(t => t.Position == argument.Position);
                }
                else
                {
                    taskDefinition.Arguments.TryGetValue(argument.Name, out argumentDefinition);
                }

                if (argumentDefinition == null)
                {
                    initializeTaskProperties = null;
                    return false;
                }

                providedArguments.Add(argumentDefinition);

                if (string.IsNullOrEmpty(argument.Value))
                {
                    taskPropertyInitializers.Add(t => argumentDefinition.Property.SetValue(t, argument.IsSelected));
                }
                else
                {
                    taskPropertyInitializers.Add(t => argumentDefinition.Property.SetValue(t, argument.Value));
                }
            }

            if (taskDefinition.AllArguments.Except(providedArguments).Any())
            {
                initializeTaskProperties = null;
                return false;
            }

            initializeTaskProperties = t =>
            {
                foreach (Action<ITask> action in taskPropertyInitializers)
                {
                    action(t);
                }
            };
            return true;
        }

        private bool TryCreateTaskInstance(IConsole console, Type taskType, out ITask task)
        {
            task = null;

            ConstructorInfo constructor = taskType.GetConstructors().FirstOrDefault(t =>
            {
                ParameterInfo[] parameters = t.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType == typeof(IConsole))
                {
                    return true;
                }
                return false;
            });

            if (constructor != null)
            {
                task = (ITask)constructor.Invoke(new object[] { console });
            }

            if (task == null)
            {
                constructor = taskType.GetConstructors().FirstOrDefault(t => t.GetParameters().Length == 0);
                task = (ITask)constructor.Invoke(Array.Empty<object>());
            }

            return task != null;
        }


        private class CompatibleTaskDefinition
        {
            public TaskDefinition TaskDefinition { get; set; }
            public Dictionary<ArgumentDefinition, Argument> Arguments { get; set; }
        }

    }
}
