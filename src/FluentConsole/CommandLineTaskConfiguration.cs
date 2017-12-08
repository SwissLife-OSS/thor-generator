using System;
using System.Collections.Generic;

namespace ChilliCream.FluentConsole
{
    public class CommandLineTaskConfiguration
    {
        private readonly ICollection<TaskDefinition> _taskDefinitions;

        protected CommandLineTaskConfiguration()
        {
            _taskDefinitions = new List<TaskDefinition>();
        }

        internal IEnumerable<TaskDefinition> GetTaskDefinitions() => _taskDefinitions;

        internal void AddTaskDefinition(TaskDefinition taskDefinition)
        {
            if (taskDefinition == null)
            {
                throw new ArgumentNullException(nameof(taskDefinition));
            }
            _taskDefinitions.Add(taskDefinition);
        }

        public IBindTask<TTask> Bind<TTask>()
            where TTask : class, ICommandLineTask
        {
            return new BindTask<TTask>(_taskDefinitions);
        }
    }
}
