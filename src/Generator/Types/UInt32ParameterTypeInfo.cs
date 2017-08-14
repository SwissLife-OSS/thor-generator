namespace ChilliCream.Logging.Generator.Types
{
    internal class UInt32ParameterTypeInfo
       : ParameterTypeInfo<uint>
    {
        public UInt32ParameterTypeInfo()
            : base("uint", sizeof(uint))
        {
        }
    }
}
