using Microsoft.Diagnostics.Tracing;

namespace ChilliCream.Tracing.Generator.Tests
{
    [EventSourceDefinition(Name = "Mock")]
    public interface IMockEventSource
    {
        [Event(1, 
            Level = EventLevel.Informational, 
            Message = "Host {2} created.", 
            Version = 2)]
        void Simple(string name);
    }
}
