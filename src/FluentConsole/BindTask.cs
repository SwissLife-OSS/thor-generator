using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ChilliCream.FluentConsole
{
    internal sealed class BindTask<TTask>
        : IBindTask<TTask>
        where TTask : class, ITask
    {
        private readonly ICollection<TaskDefinition> _tasks;
        private readonly TaskDefinition _task;

        public BindTask(ICollection<TaskDefinition> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException(nameof(tasks));
            }

            _tasks = tasks;
            _task = new TaskDefinition
            {
                TaskType = typeof(TTask)
            };

            _tasks.Add(_task);
        }

        public IBindTaskDefault<TTask> AsDefault()
        {
            if (_tasks.Any(t => t.IsDefault))
            {
                throw new InvalidOperationException("There is already a default task.");
            }

            _task.IsDefault = true;
            return new BindTaskDefault<TTask>(_tasks, _task);
        }

        public IBindTaskName<TTask> WithName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            _task.Key = name;
            _task.Name = new[] { name };
            return new BindTaskName<TTask>(_tasks, _task);
        }

        public IBindTaskName<TTask> WithName(params string[] name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            _task.Key = string.Join("-", name);
            _task.Name = name;
            return new BindTaskName<TTask>(_tasks, _task);
        }
    }
}
