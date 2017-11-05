# Thor Generator

[![GitHub release](https://img.shields.io/github/release/chillicream/thor-generator.svg)](https://github.com/ChilliCream/thor-generator/releases) [![NuGet Package](https://img.shields.io/nuget/v/Thor.Generator.svg)](https://www.nuget.org/packages/Thor.Generator/) [![Chocolatey Package](https://img.shields.io/chocolatey/v/Thor.Generator.svg)](https://chocolatey.org/packages/Thor.Generator/) [![License](https://img.shields.io/github/license/ChilliCream/thor-generator.svg)](https://github.com/ChilliCream/thor-generator/releases) [![Build](https://img.shields.io/appveyor/ci/rstaib/thor-generator/master.svg)](https://ci.appveyor.com/project/rstaib/thor-generator) [![Tests](https://img.shields.io/appveyor/tests/rstaib/thor-generator/master.svg)](https://ci.appveyor.com/project/rstaib/thor-generator) [![Coveralls](https://img.shields.io/coveralls/ChilliCream/thor-generator.svg)](https://coveralls.io/github/ChilliCream/thor-generator?branch=master)

*Thor Generator (ThorGen) is a generator for ETW (Event Tracing for Windows) event sources which helps avoid frequent mistakes and and saves time.*

Microsoft's Event Tracing for Windows is a powerfull tracing framwork that offers minimal overhead and structured payloads.

The problem with writing event sources is often that you have to work with unsafe code and that if you get anything wrong in your event source it won't write events at all. This behaviour is a feature of ETW, applications shall not be disrupted by faulty event sources, so your event source won't log but it also won't throw exceptions that crash your application.

The other problem with writing event sources is that one has to invest a lot of time into writing repetitive code "just" to have some "logging" in an application. It is not seldom that teams do not want to invest that time and opt for a simpler string based logging solution waiving all the benefits of ETW.

The ThorGen wants to solve these problems by generating the nescesarry event source code and letting developers focus on designing their tracing events around their business logic. ThorGen makes ETW easy to use and fast to implement.

Event sources will be specified by writing interfaces that define the trace events and their payloads (no other DSL  needed, no context switch). The event source generator will inspect those interfaces and generate the necessary event source code.

The event source templates can be amended to fit your needs and your aesthetic point of view concerning the generated code.

At the moment we offer two built-in templates to generate event sources in c#.

## Get Thor Generator

### Build Integration

We provide a nuget package that will integrate ThorGen with your project rather than relying on an installation. This will make it easy for developers who want to clone, build and get going rather than installing prerequisites.

1. Install the ```Thor.Generator``` nuget package to the projects that contain event source interfaces.

```powershell
Install-Package Thor.Generator
```

2. With classic .net projects we will inject ThorGen into your project file and run ThorGen for this project before every build. With the new MSBuild projects used for .net core our package will be located in the global package cache. You can then either integrate our MSBuild task into your projects or use the esgen console from your build scripts.

We have a walk through for both scenarious [here](https://github.com/ChilliCream/thor-generator-docs/blob/master/README.md).

### Windows

If you opt to install esgen on windows you can use chocolatey.

```powershell
choco install Thor.Generator
```

Chocolatey will add ThorGen to your path variable so that you can use it in any console you like.

### macOS

**Note: We are still working on our brew formula.**

If you want to install ThorGen on macOS you can use brew.

```bash
brew install Thor.Generator
```

## How to get started

Open a project where you want to add your event sources and add an interface that declares the structure of the event source you want to create. Annotate your event source interface with a ```EventSourceDefinitionAttribute```. You can either use the attribute located in our [tracing core](https://www.nuget.org/packages/Thor.Abstractions) or create your own.

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

If you are using the msbuild integration of ThorGen just compile your project; otherwise, open a terminal window and switch to your solution location and run the following command.

```cmd
thorgen
```

Or run ```thorgen -r``` if you want esgen to search recursively for any solution.

For a more detailed help that shows all the scenarious visit our [documentation](https://github.com/ChilliCream/thor-generator-docs/blob/master/README.md).


## Building the Repository

| Windows               | macOS                 |
| --------------------- | --------------------- |
| [Instructions](https://github.com/ChilliCream/thor-generator-docs/blob/master/build/windows.md)      | [Instructions](https://github.com/ChilliCream/thor-generator-docs/blob/master/build/macos.md)      |

### Build status of master branch

| Platform               | Build               | Tests                 | Code Coverage         |
| --------------------- | --------------------- | --------------------- | --------------------- |
| Windows | [![AppVeyor branch](https://img.shields.io/appveyor/ci/rstaib/thor-generator/master.svg)](https://ci.appveyor.com/project/rstaib/thor-generator) | [![Tests](https://img.shields.io/appveyor/tests/rstaib/thor-generator/master.svg)](https://ci.appveyor.com/project/rstaib/thor-generator) | [![Coveralls](https://img.shields.io/coveralls/ChilliCream/thor-generator.svg)](https://coveralls.io/github/ChilliCream/thor-generator?branch=master) |

## Legal and Licensing

The Thor Generator (ThorGen) is licensed under the [MIT](LICENSE) license.