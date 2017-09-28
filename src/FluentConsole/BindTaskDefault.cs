using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ChilliCream.FluentConsole
{
    internal sealed class BindTaskDefault<TTask>
        : IBindTaskDefault<TTask>
        where TTask : class, ICommandLineTask
    {
        private readonly ICollection<TaskDefinition> _tasks;
        private readonly TaskDefinition _task;

        public BindTaskDefault(ICollection<TaskDefinition> tasks, TaskDefinition task)
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

        public IBindArgument<TTask> Argument<TProperty>(Expression<Func<TTask, TProperty>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            PropertyInfo propertyInfo = TaskUtils.GetPropertyInfo(property);
            return new BindArgument<TTask>(_task, propertyInfo);
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
