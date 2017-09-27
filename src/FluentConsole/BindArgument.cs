using System;
using System.Linq;
using System.Reflection;

namespace ChilliCream.FluentConsole
{
    internal sealed class BindArgument<TTask>
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
            if (string.IsNullOrEmpty(_argument.Name)
                && !_argument.Position.HasValue)
            {
                throw new InvalidOperationException("You have to specify a position or argument name first.");
            }

            return new BindAdditionalArgument<TTask>(_task);
        }

        public IBindArgument<TTask> Mandatory()
        {
            _argument.IsMandatory = true;
            return this;
        }

        public IBindArgument<TTask> Position(int position)
        {
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position), position, "The position cannot be below zero.");
            }

            if (_task.PositionalArguments.Where(t => t != _argument)
                .Any(t => t.Position == position))
            {
                throw new ArgumentException("There is already an argument defined at this position.", nameof(position));
            }

            _argument.Position = position;

            if (!_task.PositionalArguments.Contains(_argument))
            {
                _task.PositionalArguments.Add(_argument);
            }

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

            if (key < 97 || key > 122)
            {
                throw new ArgumentException("The key must be between a and z.", nameof(name));
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
