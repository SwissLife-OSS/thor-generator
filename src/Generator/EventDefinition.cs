using Microsoft.Diagnostics.Tracing;
using System.Collections.Generic;

namespace ChilliCream.Tracing.Generator
{
    public class EventDefinition
    {
        public string Name { get; set; }
        public int EventId { get; set; }
        public EventLevel Level { get; set; }
        public EventKeywords Keywords { get; set; }
        public EventOpcode Opcode { get; set; }
        public EventTask Task { get; set; }
        public EventChannel Channel { get; set; }
        public byte Version { get; set; }
        public string Message { get; set; }
        public EventTags Tags { get; set; }
        public EventActivityOptions ActivityOptions { get; set; }
        public string AttributeSyntax { get; set; }
        public List<EventArgumentDefinition> Arguments { get; } = new List<EventArgumentDefinition>();
    }
}