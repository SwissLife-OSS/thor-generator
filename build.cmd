dotnet restore src
dotnet build src -c debug

dotnet test src/Generator.ProjectSystem.Tests --no-build --no-restore --logger:trx
dotnet test src/Generator.Tests --no-build --no-restore --logger:trx
dotnet test src/FluentConsole.Tests --no-build --no-restore --logger:trx

dotnet build src/Generator.CLI -c release -r win10-x64
dotnet publish src/Generator.CLI -c release -r win10-x64 -f netcoreapp2.0
dotnet build src/Generator.CLI -c release -r osx-x64
dotnet publish src/Generator.CLI -c release -r  osx-x64 -f netcoreapp2.0
