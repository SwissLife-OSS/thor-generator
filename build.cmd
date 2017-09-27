dotnet restore src
dotnet build src

dotnet test src/Generator.ProjectSystem.Tests
dotnet test src/Generator.Tests
