using System.Collections.Generic;

namespace ChilliCream.Tracing.Generator.Types
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
