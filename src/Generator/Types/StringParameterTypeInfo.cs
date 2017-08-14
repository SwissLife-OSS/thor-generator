namespace ChilliCream.Logging.Generator.Types
{
    internal class StringParameterTypeInfo
        : ParameterTypeInfo<string>
    {
        public StringParameterTypeInfo()
            : base("string", "((b.Length + 1) * 2)")
        {
            IsString = true;
            Operator = null;
        }
    }
}
