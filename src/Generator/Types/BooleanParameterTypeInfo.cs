namespace ChilliCream.Logging.Generator.Types
{
    internal class BooleanParameterTypeInfo
      : ParameterTypeInfo<bool>
    {
        public BooleanParameterTypeInfo()
            : base("bool", sizeof(bool))
        {
        }
    }
}
