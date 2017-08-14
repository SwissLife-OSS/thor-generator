namespace ChilliCream.Logging.Generator.Types
{
    internal class UInt16ParameterTypeInfo
        : ParameterTypeInfo<short>
    {
        public UInt16ParameterTypeInfo()
            : base("ushort", sizeof(ushort))
        {
        }
    }
}
