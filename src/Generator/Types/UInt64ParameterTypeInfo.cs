namespace ChilliCream.Logging.Generator.Types
{
    internal class UInt64ParameterTypeInfo
       : ParameterTypeInfo<ulong>
    {
        public UInt64ParameterTypeInfo()
            : base("ulong", sizeof(ulong))
        {
        }
    }
}
