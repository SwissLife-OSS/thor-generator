using System;
using System.Diagnostics.Tracing;
using Thor.Core.Abstractions;
using Xunit;

namespace Thor.Generator.Analyzer;

public class UnitTest1
{

    [Fact]
    public void Test1()
    {

    }
}

[EventSourceDefinition(Name = "Foo")]
public interface IMessageEventSource
{
    [Event(1,
        Level = EventLevel.Verbose,
        Message = "Sent message {correlationId}/{messageType} to {to}.",
        Version = 1)]
    void MessageSent(Guid messageId, Guid correlationId, string messageType, string from, string to);
}

