using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ChilliCream.Tracing.Generator.Tasks
{
    internal sealed class BindAdditionalArgument<TTask>
        : IBindAdditionalArgument<TTask>
        where TTask : class, ITask
    {
        private readonly TaskDefinition _task;

        public BindAdditionalArgument(TaskDefinition task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            _task = task;
        }

        public IBindArgument<TTask> Argument<TProperty>(Expression<Func<TTask, TProperty>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            PropertyInfo propertyInfo = TaskUtils.GetPropertyInfo(property);

            if (_task.Arguments.Values.Concat(_task.PositionalArguments)
                .Any(t => t.Property == propertyInfo))
            {
                throw new ArgumentException("The specified property has already been declared.",  nameof(property));
            }

            return new BindArgument<TTask>(_task, propertyInfo);
        }
    }
}
