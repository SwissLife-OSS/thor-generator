using System;
using System.Collections.Generic;
using System.Linq;

namespace ChilliCream.FluentConsole
{
    internal sealed class TaskDefinitionResolver
    {
        private readonly IList<TaskDefinition> _taskDefinitions;

        public TaskDefinitionResolver(IList<TaskDefinition> taskDefinitions)
        {
            if (taskDefinitions == null)
            {
                throw new ArgumentNullException(nameof(taskDefinitions));
            }

            _taskDefinitions = taskDefinitions;
        }

        public bool TryResolve(Argument[] arguments,
           out ResolvedTaskDefinitions resolvedTaskDefinitions)
        {
            if (_taskDefinitions.Count == 0)
            {
                resolvedTaskDefinitions = null;
                return false;
            }

            if (_taskDefinitions.Count == 1 && _taskDefinitions[0].IsDefault)
            {
                resolvedTaskDefinitions = new ResolvedTaskDefinitions(new[] { _taskDefinitions[0] });
                return true;
            }

            List<TaskDefinition> taskDefinitions = new List<TaskDefinition>();
            ILookup<int, TaskDefinition> taskLookup = _taskDefinitions.ToLookup(t => t.Name.Length);
            int maxNameParts = _taskDefinitions.Select(t => t.Name?.Length ?? 0).Max();
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
