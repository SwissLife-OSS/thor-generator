param([switch]$DisableBuild, [switch]$RunTests, [switch]$EnableCoverage, [switch]$EnableSonar, [switch]$Publish)

if (!!$env:APPVEYOR_REPO_TAG_NAME)
{
    $version = $env:APPVEYOR_REPO_TAG_NAME
}
elseif(!!$env:APPVEYOR_BUILD_VERSION)
{
    $version = $env:APPVEYOR_BUILD_VERSION
}

if($EnableSonar)
{

}


if($DisableBuild -eq $false)
{
    dotnet restore src
    msbuild src
}

if($RunTests -or $EnableCoverage)
{
    # Test
    $serachDirs = [System.IO.Path]::Combine($PSScriptRoot, "src", "*", "bin",  "Debug", "netcoreapp2.0")
    $runTestsCmd = [System.Guid]::NewGuid().ToString("N") + ".cmd"
    $runTestsCmd = Join-Path -Path $env:TEMP -ChildPath $runTestsCmd
    $testAssemblies = ""
    
    Get-ChildItem ./src/*.Tests | %{ $testAssemblies += "dotnet test `"" + $_.FullName + "`" --no-build`n" }
    
    if (!!$testAssemblies) # Has test assemblies
    {    
        $userDirectory = $env:USERPROFILE
        if($IsMacOS) 
        {
            $userDirectory = $env:HOME
        }
        
        [System.IO.File]::WriteAllText($runTestsCmd, $testAssemblies)
        Write-Host $runTestsCmd

        if ($EnableCoverage)
        {
            # Test & Code Coverage
            $nugetPackages = [System.IO.Path]::Combine($userDirectory, ".nuget", "packages")
            
            $openCover = [System.IO.Path]::Combine($nugetPackages, "OpenCover", "*", "tools",  "OpenCover.Console.exe")
            $openCover = Resolve-Path $openCover

            $coveralls = [System.IO.Path]::Combine($nugetPackages, "coveralls.io", "*", "tools",  "coveralls.net.exe")
            $coveralls = Resolve-Path $coveralls

            & $openCover -register:user -target:"$runTestsCmd" -searchdirs:"$serachDirs" -oldstyle -output:coverage.xml -skipautoprops -returntargetcode -filter:"+[ChilliCream*]*"
            & $coveralls --opencover coverage.xml
        }
        else
        {
            # Test
            & $runTestsCmd
        }
    }
}

if($EnableSonar)
{

    
}

if($Publish) 
{
    $dropRootDirectory = Join-Path -Path $PSScriptRoot -ChildPath "drop"
    $win10x64 = Join-Path -Path $dropRootDirectory -ChildPath "win10-x64"
    $ubuntu1404x64 = Join-Path -Path $dropRootDirectory -ChildPath "ubuntu.14.04-x64"
    $osxx64 = Join-Path -Path $dropRootDirectory -ChildPath "osx-x64"

    dotnet publish ./src/Generator.CLI -c Release -f netcoreapp2.0 -v $version -r win10-x64 -o $win10x64
    dotnet publish ./src/Generator.CLI -c Release -f netcoreapp2.0 -v $version -r ubuntu.14.04-x64 -o $ubuntu1404x64
    dotnet publish ./src/Generator.CLI -c Release -f netcoreapp2.0 -v $version -r osx-x64 -o $osxx64
}