using System;
using System.Collections.Generic;

namespace ChilliCream.FluentConsole
{
    internal sealed class ResolvedTaskDefinitions
    {
        public ResolvedTaskDefinitions(IReadOnlyList<TaskDefinition> taskDefinitions)
        {
            if (taskDefinitions == null)
            {
                throw new ArgumentNullException(nameof(taskDefinitions));
            }

            TaskDefinitions = taskDefinitions;
        }

        public ResolvedTaskDefinitions(IReadOnlyList<TaskDefinition> taskDefinitions, int argumentCount)
        {
            if (taskDefinitions == null)
            {
                throw new ArgumentNullException(nameof(taskDefinitions));
            }

            if (argumentCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(argumentCount));
            }

            TaskDefinitions = taskDefinitions;
            ArgumentCount = argumentCount;
        }

        public IReadOnlyList<TaskDefinition> TaskDefinitions { get; set; }
        public int ArgumentCount { get; set; }
    }
}
