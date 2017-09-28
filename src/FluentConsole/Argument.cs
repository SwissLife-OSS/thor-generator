namespace ChilliCream.FluentConsole
{
    internal sealed class Argument
    {
        public string Name { get; set; }
        public int Position { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }

        public override string ToString()
        {
            string value = string.IsNullOrEmpty(Value)
                ? IsSelected.ToString()
                : Value;

            string name = string.IsNullOrEmpty(Name)
                ? Position.ToString()
                : $"{Position} {Name}";

            return $"{name}: {value}";
        }
    }
}
