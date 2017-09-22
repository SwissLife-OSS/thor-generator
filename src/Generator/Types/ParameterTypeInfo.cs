using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

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

        public bool IsString { get; internal set; }

        public string Operator { get; internal set; }

        public bool IsType(string typeName)
        {
            if (typeName == null)
            {
                return false;
            }
            return _names.Contains(typeName, StringComparer.Ordinal);
        }
    }

    internal static class ParameterTypeInfo
    {
        private static readonly IParameterTypeInfo[] _parameterTypeInfos
            = new IParameterTypeInfo[]
        {
            new ParameterTypeInfo<string>("string", "((b.Length + 1) * 2)")
            {
                IsString = true,
                Operator = null
            },

            new ParameterTypeInfo<long>("long", sizeof(long)),
            new ParameterTypeInfo<int>("int", sizeof(int)),
            new ParameterTypeInfo<short>("short", sizeof(short)),

            new ParameterTypeInfo<ulong>("ulong", sizeof(ulong)),
            new ParameterTypeInfo<uint>("uint", sizeof(uint)),
            new ParameterTypeInfo<ushort>("ushort", sizeof(ushort)),

            new ParameterTypeInfo<float>("float", sizeof(float)),
            new ParameterTypeInfo<double>("double", sizeof(double)),

            new ParameterTypeInfo<char>("char", sizeof(char)),
            new ParameterTypeInfo<byte>("byte", sizeof(byte)),
            new ParameterTypeInfo<sbyte>("sbyte", sizeof(sbyte)),
            new ParameterTypeInfo<decimal>("decimal", sizeof(decimal)),
            new ParameterTypeInfo<bool>("bool", sizeof(bool)),

            new ParameterTypeInfo<Guid>(Marshal.SizeOf(typeof(Guid)))
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
