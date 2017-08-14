namespace ChilliCream.Logging.Generator.Types
{
    internal class Int32ParameterTypeInfo
        : ParameterTypeInfo<int>
    {
        public Int32ParameterTypeInfo()
            : base("int", sizeof(int))
        {
        }
    }
}
