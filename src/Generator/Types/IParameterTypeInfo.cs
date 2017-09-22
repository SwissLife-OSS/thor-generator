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
}
