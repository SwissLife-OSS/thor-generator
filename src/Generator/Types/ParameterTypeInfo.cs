using System;
using System.Collections.Generic;

namespace ChilliCream.Tracing.Generator.Types
{
    internal class ParameterTypeInfo<T>
        : IParameterTypeInfo
    {
        private readonly List<string> _names;

        public ParameterTypeInfo(int size)
        {
            _names = new List<string>
            {
                typeof(T).Name,
                typeof(T).FullName
            };

            Name = typeof(T).Name;
            Size = size.ToString();
            Operator = "&";
            IsString = false;
        }

        public ParameterTypeInfo(string alias, int size)
            : this(alias, size.ToString())
        {
        }

        public ParameterTypeInfo(string alias, string size)
        {
            if (alias == null)
            {
                throw new ArgumentNullException(nameof(alias));
            }

            if (size == null)
            {
                throw new ArgumentNullException(nameof(size));
            }

            _names = new List<string>
            {
                alias,
                typeof(T).Name,
                typeof(T).FullName
            };

            Name = alias;
            Size = size;
            Operator = "&";
            IsString = false;
        }

        public string Name { get; }

        public string Size { get; }

        public bool IsString { get; internal set; }

        public string Operator { get; internal set; }

        public IEnumerable<string> GetNames()
        {
            return _names;
        }
    }
}
