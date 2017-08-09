using System;
using System.Collections.Generic;
using System.Linq;

namespace ChilliCream.Logging.Generator
{
    internal sealed class WriteMethod
        : IEquatable<WriteMethod>
    {
        private readonly string[] _types;
        private readonly string _key;

        public WriteMethod(IEnumerable<string> types)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            _types = types.ToArray();
            _key = string.Join("_", _types);
        }

        public IReadOnlyCollection<string> ParameterTypes => _types;

        public bool Equals(WriteMethod other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(other, this))
            {
                return true;
            }

            return _key.Equals(other._key);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as WriteMethod);
        }

        public override int GetHashCode()
        {
            return _key.GetHashCode();
        }

        public override string ToString()
        {
            return string.Join(", ", _types);
        }
    }
}