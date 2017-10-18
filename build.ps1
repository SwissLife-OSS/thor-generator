param([switch]$DisableBuild, [switch]$RunTests)

if($DisableBuild -eq $false)
{
    dotnet restore src
    msbuild src
}

if($RunTests)
{
    dotnet test src/FluentConsole.Tests
    dotnet test src/Generator.ProjectSystem.Tests
    dotnet test src/Generator.Tests
}