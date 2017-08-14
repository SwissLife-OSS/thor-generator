using System.Collections;
using System.Collections.Generic;

namespace ChilliCream.Logging.Generator.Analyzer
{
    public class EventSourceDefinition
    {
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string LocalizationResources { get; set; }
        public List<EventDefinition> Events { get; } = new List<EventDefinition>();
    }
}