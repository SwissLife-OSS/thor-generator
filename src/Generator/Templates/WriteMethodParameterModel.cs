namespace ChilliCream.Tracing.Generator.Templates
{
    internal class WriteMethodParameterModel
    {
        public string Name { get; set; }
        public string Size { get; set; }

        public string Operator { get; set; }
        public int Position { get; set; }

        public bool IsFirst { get; set; }
        public bool IsString { get; set; }
    }
}
