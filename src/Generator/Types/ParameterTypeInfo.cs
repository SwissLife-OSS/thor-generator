using System;
using System.Collections.Generic;
using System.Linq;

namespace ChilliCream.Logging.Generator.Types
{
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
            new Int16ParameterTypeInfo(),
            new UInt64ParameterTypeInfo(),
            new UInt32ParameterTypeInfo(),
            new UInt16ParameterTypeInfo(),
            new DecimalParameterTypeInfo(),
            new DoubleParameterTypeInfo(),
            new BooleanParameterTypeInfo()
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
