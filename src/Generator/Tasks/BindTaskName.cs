using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ChilliCream.Tracing.Generator.Tasks
{
    internal sealed class BindTaskName<TTask>
        : IBindTaskName<TTask>
        where TTask : class, ITask
    {
        private readonly ICollection<TaskDefinition> _tasks;
        private readonly TaskDefinition _task;

        public BindTaskName(ICollection<TaskDefinition> tasks, TaskDefinition task)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException(nameof(tasks));
            }

            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            _tasks = tasks;
            _task = task;
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

        public IBindArgument<TTask> Argument<TProperty>(Expression<Func<TTask, TProperty>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            PropertyInfo propertyInfo = TaskUtils.GetPropertyInfo(property);
            return new BindArgument<TTask>(_task, propertyInfo);
        }
    }
}
