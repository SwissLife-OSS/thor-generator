namespace ChilliCream.Logging.Generator.Types
{
    internal class Int64ParameterTypeInfo
       : ParameterTypeInfo<long>
    {
        public Int64ParameterTypeInfo()
            : base("long", sizeof(long))
        {
        }
    }
}
