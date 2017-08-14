namespace ChilliCream.Logging.Generator.Types
{
    internal class DecimalParameterTypeInfo
      : ParameterTypeInfo<decimal>
    {
        public DecimalParameterTypeInfo()
            : base("decimal", sizeof(decimal))
        {
        }
    }
}
