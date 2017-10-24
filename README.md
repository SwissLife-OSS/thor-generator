# EventSourceGenerator

**An *ETW* EventSource generator build on *.Net Core 2.0***

## Get EventSourceGenerator

### Windows

#### Install with choco

#### Install with NuGet

#### Download 


### macOS

#### Install with brew

## How to get started

Open a project where you want to add your event sources and add an interface that declares the structure of the event source you want to create. Annotate your event source interface with a ```EventSourceDefinitionAttribute```. You can either use the attribute located in our [tracing core](https://nuget.org) or create your own.

```csharp
using System;
using System.Diagnostics.Tracing;
using ChilliCream.Tracing;

[EventSourceDefinition("MyEventSourceName")]
public interface IMyEventSource
{
    [Event(1)]
    void SayHello(string message);
}
```

After you have added all your event source interfaces open a console window and switch to your solution location and run the following command.

```cmd
esgen
```

For a more detailed help that shows all the scenarious visit our [documentation](http://io.github.com).


## Building the Repository

| Windows               | macOS                 |
| --------------------- | --------------------- |
| [Instructions]()      | [Instructions]()      |

### Build status of master branch

| AppVeyor (Windows)    | Code Coverage         |
| --------------------- | --------------------- |
| [![AppVeyor branch](https://img.shields.io/appveyor/ci/rstaib/EventSourceGenerator/master.svg)](https://ci.appveyor.com/project/rstaib/eventsourcegenerator) | [![Coveralls](https://img.shields.io/coveralls/ChilliCream/EventSourceGenerator.svg)](https://coveralls.io/github/ChilliCream/EventSourceGenerator?branch=master) |

## Legal and Licensing

The EventSourceGenerator is licensed under the [MIT](LICENSE) license.