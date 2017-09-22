using System.Reflection;

namespace ChilliCream.Tracing.Generator.Tasks
{
    internal sealed class ArgumentDefinition
    {
        public PropertyInfo Property { get; set; }
        public string Name { get; set; }
        public char? Key { get; set; }
        public int? Position { get; set; }
        public bool IsSelected { get; set; }
        public string Value { get; set; }
        public bool IsMandatory { get; set; }
    }
}
