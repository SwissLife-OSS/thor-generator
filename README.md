# EventSourceGenerator

**An *ETW* EventSource generator build on *.Net Core 2.0***

Microsoft's Event Tracing for Windows is a powerfull tracing framwork that offers minimal overhead and structured log payloads. 

The problem in writing event sources is often that you have to work with unsafe code and that if you get anything wrong in your event source it won't log at all. This behaviour is a feature of ETW, the application shall not be disrupted by a faulty logger, so your event source won't log but it also won't throw exceptions.

The other problem with writing event sources is that I don't want to spend too much time writing repetitive code just to trace my events. The way I see it is that I want to concentrate on the task at hand and design my business code and withit I want to design my tracing events with its payloads.

The best way to do this is to just describe the event sources needed by specifing an interfaces that describe the payloads. By doing this I do not have to switch another dsl like json or yaml or somthing else. I also can channel the power of the .net compile to validate my interface.

The EventSourceGenerator will take care from here by using Microsoft's Roslyn compiler to find and analyse event source interfaces in your code and generate the event source implementations by using the mustache template engine.

At the moment we offer two built-in templates to generate event sources in c#. Moreover, you can import your own mustache based templates into the EventSourceGenerator.

## Get EventSourceGenerator

### Windows

The easiest way to get start is by installing esgen with chocolatey.

```powershell
choco install esgen
```

Chocolatey will add esgen to your path variable so that you can use it in any console you like.
 
### macOS

### Build Integration

We also provide a nuget package that will integrate esgen with your project rather than relying on an installation. This will make it easy for developers who want to clone, build and get going rather than installing prerequisites.

1. Install the ```esgen``` nuget package to the projects that contain event source interfaces.

```powershell
Install-Package esgen
```

2. With the classic .net MSBuild projects we will inject esgen into your project file and run esgen for this project after every build.
With the new MSBuild projects used for .net core our package will be located in the global package cache. You can then either integrate our MSBuild task into your projects or if you use

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