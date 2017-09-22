using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ChilliCream.Tracing.Generator.Tasks
{
    internal class BindArgument<TTask>
        : IBindArgument<TTask>
        where TTask : class, ITask
    {
        private readonly TaskDefinition _task;
        private readonly ArgumentDefinition _argument;

        public BindArgument(TaskDefinition task, PropertyInfo property)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            _task = task;
            _argument = new ArgumentDefinition
            {
                Property = property
            };
        }


        public IBindAdditionalArgument<TTask> And()
        {
            throw new NotImplementedException();
        }

        public IBindArgument<TTask> Mandatory()
        {
            throw new NotImplementedException();
        }

        public IBindArgument<TTask> Position(int porition)
        {
            return this;
        }

        public IBindArgument<TTask> WithName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (_argument.Name != null)
            {
                _task.Arguments.Remove(_argument.Name);
            }

            _argument.Name = name;
            _task.Arguments.Add(name, _argument);

            return this;
        }

        public IBindArgument<TTask> WithName(string name, char key)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (_argument.Name != null)
            {
                _task.Arguments.Remove(_argument.Name);
            }

            if (_argument.Key.HasValue)
            {
                _task.Arguments.Remove(_argument.Key.Value.ToString());
            }

            _argument.Name = name;
            _argument.Key = key;

            _task.Arguments.Add(name, _argument);
            _task.Arguments.Add(key.ToString(), _argument);

            return this;
        }
    }
}
