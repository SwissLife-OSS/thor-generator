using System.Collections;
using System.Collections.Generic;

namespace ChilliCream.Logging.Generator
{
    public class EventSourceDefinition
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string LocalizationResources { get; set; }
        public List<EventDefinition> Events { get; } = new List<EventDefinition>();
    }
}