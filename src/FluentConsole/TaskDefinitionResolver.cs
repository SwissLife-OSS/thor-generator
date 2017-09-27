using System;
using System.Collections.Generic;
using System.Linq;

namespace ChilliCream.FluentConsole
{
    internal sealed class TaskDefinitionResolver
    {
        private readonly IList<TaskDefinition> _tasks;

        public TaskDefinitionResolver(IList<TaskDefinition> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException(nameof(tasks));
            }

            _tasks = tasks;
        }

        public bool TryResolve(Argument[] arguments,
           out ResolvedTaskDefinitions resolvedTaskDefinitions)
        {
            if (_tasks.Count == 0)
            {
                resolvedTaskDefinitions = null;
                return false;
            }

            if (_tasks.Count == 1 && _tasks[0].IsDefault)
            {
                resolvedTaskDefinitions = new ResolvedTaskDefinitions(new[] { _tasks[0] });
                return true;
            }

            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();
            ILookup<int, TaskDefinition> taskLookup = _tasks.ToLookup(t => t.Name.Length);
            int maxNameParts = _tasks.Select(t => t.Name?.Length ?? 0).Max();
            int maxPositionalParameters = arguments.Count(t => string.IsNullOrEmpty(t.Name));
            maxNameParts = maxPositionalParameters < maxNameParts ? maxPositionalParameters : maxNameParts;
            for (int i = maxNameParts - 1; i >= 0; i--)
            {
                if (TryCreateName(arguments, i + 1, out string name))
                {
                    foreach (TaskDefinition taskDefinition in taskLookup[i + 1])
                    {
                        if (string.Equals(taskDefinition.Key, name,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            taskDefinitions.Add(taskDefinition);
                        }
                    }

                    if (taskDefinitions.Any())
                    {
                        resolvedTaskDefinitions = new ResolvedTaskDefinitions(taskDefinitions, i + 1);
                        return true;
                    }
                }
            }

            resolvedTaskDefinitions = null;
            return false;
        }

        private bool TryCreateName(Argument[] arguments, int position, out string name)
        {
            name = string.Empty;
            for (int i = 0; i < position; i++)
            {
                if (string.IsNullOrEmpty(arguments[i].Name))
                {
                    name += string.IsNullOrEmpty(name)
                        ? arguments[i].Value
                        : "-" + arguments[i].Value;
                }
                else
                {
                    name = null;
                    return false;
                }
            }
            return true;
        }

    }
}
