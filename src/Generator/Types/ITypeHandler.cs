using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChilliCream.Logging.Generator.Types
{
    internal interface IParameterTypeInfo
    {
        bool IsType(string typeName);

        string Name { get; }
        string Size { get; }
        string Operator { get; }
        bool IsString { get; }
    }

    internal class StringParameterTypeInfo
        : ParameterTypeInfo<string>
    {
        public StringParameterTypeInfo()
            : base("string", "((b.Length + 1) * 2)")
        {
            IsString = true;
            Operator = null;
        }
    }

    internal class Int64ParameterTypeInfo
       : ParameterTypeInfo<long>
    {
        public Int64ParameterTypeInfo()
            : base("long", sizeof(long))
        {
        }
    }

    internal class Int32ParameterTypeInfo
        : ParameterTypeInfo<int>
    {
        public Int32ParameterTypeInfo()
            : base("int", sizeof(int))
        {
        }
    }

    internal class Int16ParameterTypeInfo
        : ParameterTypeInfo<short>
    {
        public Int16ParameterTypeInfo()
            : base("short", sizeof(short))
        {
        }
    }

    internal class ParameterTypeInfo<T>
        : IParameterTypeInfo
    {
        private readonly HashSet<string> _names;

        public ParameterTypeInfo(int size)
        {
            _names = new HashSet<string>
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

            _names = new HashSet<string>
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

        public bool IsString { get; protected set; }

        public string Operator { get; protected set; }

        public bool IsType(string typeName)
        {
            if (typeName == null)
            {
                return false;
            }
            return _names.Contains(typeName);
        }
    }

    internal static class ParameterTypeInfo
    {
        private static readonly IParameterTypeInfo[] _parameterTypeInfos
            = new IParameterTypeInfo[]
        {
            new StringParameterTypeInfo(),
            new Int64ParameterTypeInfo(),
            new Int32ParameterTypeInfo(),
            new Int16ParameterTypeInfo()
        };

        public static bool TryGet(string typeName,
            out IParameterTypeInfo parameterTypeInfo)
        {
            if (typeName != null)
            {
                string localTypeName = typeName.Trim();
                parameterTypeInfo = _parameterTypeInfos
                    .FirstOrDefault(t => t.IsType(localTypeName));
                return parameterTypeInfo != null;
            }

            parameterTypeInfo = null;
            return false;
        }
    }
}
