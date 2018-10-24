namespace Thor.Generator.Templates
{
    internal class Placeholder
    {
        public Placeholder(int start, int end, string name)
        {
            Start = start;
            End = end;
            Name = name;
        }

        public Placeholder(int start, int end, string name, string format)
        {
            Start = start;
            End = end;
            Name = name;
            Format = format;
        }

        public int Start { get; }

        public int End { get; }

        public string Name { get; }

        public string Format { get; }

        public override string ToString()
        {
            if (Format == null)
            {
                return $"{{{Name}}}";
            }
            return $"{{{Name}:{Format}}}";
        }

        public string ToString(int index)
        {
            if (Format == null)
            {
                return $"{{{index}}}";
            }
            return $"{{{index}:{Format}}}";
        }
    }
}
