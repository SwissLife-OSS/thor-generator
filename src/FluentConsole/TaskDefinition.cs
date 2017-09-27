using System;
using System.Collections.Generic;
using System.Linq;

namespace ChilliCream.FluentConsole
{
    internal sealed class TaskDefinition
    {
        public Type TaskType { get; set; }
        public string Key { get; set; }
        public string[] Name { get; set; }
        public bool IsDefault { get; set; }
        public List<ArgumentDefinition> PositionalArguments { get; }
            = new List<ArgumentDefinition>();
        public Dictionary<string, ArgumentDefinition> Arguments { get; }
            = new Dictionary<string, ArgumentDefinition>(StringComparer.OrdinalIgnoreCase);
        public IEnumerable<ArgumentDefinition> AllArguments
            => Arguments.Values.Concat(PositionalArguments).Distinct();
        public IEnumerable<ArgumentDefinition> MandatoryArguments
          => AllArguments.Where(t => t.IsMandatory);
    }
}
