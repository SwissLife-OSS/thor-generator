# EventSourceGenerator

**An *ETW* EventSource generator build on *.Net Core 2.0***

Microsoft's Event Tracing for Windows is a powerfull tracing framwork that offers minimal overhead and structured log payloads.

The problem in writing event sources is often that you have to work with unsafe code and that if you get anything wrong in your event source it won't log at all. This behaviour is a feature of ETW, the application shall not be disrupted by faulty event sources, so your event source won't log but it also won't throw exceptions that crash your application.

The other problem with writing event sources is that one has to invest a lot of time into writing repetitive code "just" to have some logging in an application. It is not seldom that teams do not want to invest that time and opt for a simpler string based logging solution.

The EventSourceGenerator (ESGen) wants to solf these problems by generating the nescesarry event source code.

Event sources will be specified by writing an interface that describes the logging events (no other dsl needed). The event source generator will inspect those interfaces and generate the event source code for you.

The event source templates can be amended to fit your needs and your aesthetic point of view concerning the generated code.

At the moment we offer two built-in templates to generate event sources in c#.

## Get EventSourceGenerator

### Build Integration

We provide a nuget package that will integrate esgen with your project rather than relying on an installation. This will make it easy for developers who want to clone, build and get going rather than installing prerequisites.

1. Install the ```esgen``` nuget package to the projects that contain event source interfaces.

```powershell
Install-Package esgen
```

2. With the classic .net MSBuild projects we will inject esgen into your project file and run esgen for this project after every build. With the new MSBuild projects used for .net core our package will be located in the global package cache. You can then either integrate our MSBuild task into your projects or use the esgen console from your build scripts.

### Windows

If you opt to install esgen on windows you can use chocolatey.

```powershell
choco install esgen
```

Chocolatey will add esgen to your path variable so that you can use it in any console you like.

 
### macOS

If you want to install esgen on macOS you can use brew.

```bash
brew install esgen
```

#### Install with brew

## How to get started

Open a project where you want to add your event sources and add an interface that declares the structure of the event source you want to create. Annotate your event source interface with a ```EventSourceDefinitionAttribute```. You can either use the attribute located in our [tracing core](https://www.nuget.org/packages/ChilliCream.Tracing.Abstractions/1.0.0) or create your own.

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

If you are using the msbuild integration of esgen just compile your project; otherwise, open a terminal window and switch to your solution location and run the following command.

```cmd
esgen
```

Or run ```esgen -r``` if you want esgen to search recursively for any solution.

For a more detailed help that shows all the scenarious visit our [documentation](https://github.com/ChilliCream/EventSourceGenerator-docs/blob/master/README.md).


## Building the Repository

| Windows               | macOS                 |
| --------------------- | --------------------- |
| [Instructions](https://github.com/ChilliCream/EventSourceGenerator-docs/blob/master/build/windows.md)      | [Instructions](https://github.com/ChilliCream/EventSourceGenerator-docs/blob/master/build/macos.md)      |

### Build status of master branch

| AppVeyor (Windows)    | Code Coverage         |
| --------------------- | --------------------- |
| [![AppVeyor branch](https://img.shields.io/appveyor/ci/rstaib/EventSourceGenerator/master.svg)](https://ci.appveyor.com/project/rstaib/eventsourcegenerator) | [![Coveralls](https://img.shields.io/coveralls/ChilliCream/EventSourceGenerator.svg)](https://coveralls.io/github/ChilliCream/EventSourceGenerator?branch=master) |

## Legal and Licensing

The EventSourceGenerator is licensed under the [MIT](LICENSE) license.