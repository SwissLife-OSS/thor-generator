using System.Collections.Generic;

namespace Thor.Generator.Types
{
    internal interface IParameterTypeInfo
    {
        string Name { get; }
        string Size { get; }
        string Operator { get; }
        bool IsString { get; }
        IEnumerable<string> GetNames();
    }
}
