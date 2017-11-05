using System;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;

namespace Thor.Generator.Types
{
    internal static class ParameterTypeLookup
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

        private static ImmutableDictionary<string, IParameterTypeInfo> _typeLookup;

        private static void Initialize()
        {
            if (_typeLookup == null)
            {
                ImmutableDictionary<string, IParameterTypeInfo> typeLookup = ImmutableDictionary<string, IParameterTypeInfo>.Empty;

                foreach (IParameterTypeInfo typeInfo in _parameterTypeInfos)
                {
                    foreach (string name in typeInfo.GetNames())
                    {
                        typeLookup = typeLookup.SetItem(name, typeInfo);
                    }
                }

                _typeLookup = typeLookup;
            }
        }

        public static bool TryGet(string typeName,
            out IParameterTypeInfo parameterTypeInfo)
        {
            Initialize();

            if (typeName != null && _typeLookup.TryGetValue(typeName.Trim(),
                out parameterTypeInfo))
            {
                return true;
            }

            parameterTypeInfo = null;
            return false;
        }
    }
}
