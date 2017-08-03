using System;

namespace ChilliCream.Tracing.Generator
{
    // todo : this type has to be moved into the diagnostics core
    public sealed class EventSourceDefinitionAttribute
        : Attribute
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string LocalizationResources { get; set; }
    }
}