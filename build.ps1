param([switch]$DisableBuild, [switch]$RunTests, [switch]$EnableCoverage)

if($DisableBuild -eq $false)
{
    dotnet restore src
    msbuild src
}

if($RunTests)
{
    # Test
    $serachDirs = [System.IO.Path]::Combine($PSScriptRoot, "src", "*", "bin",  "Debug", "netcoreapp2.0")
    $testAssemblies = $null
    
    Get-ChildItem -Path $serachDirs -Include *.Tests.dll -Recurse | %{ $testAssemblies += $_.FullName + " " }

    if (!!$testAssemblies) # Has test assemblies
    {    
        $userDirectory = $env:USERPROFILE
        if($IsMacOS) 
        {
            $userDirectory = $env:HOME
        }        
        
        if ($EnableCoverage)
        {
            # Test & Code Coverage
            $nugetPackages = [System.IO.Path]::Combine($userDirectory, ".nuget", "packages")
            
            $openCover = [System.IO.Path]::Combine($nugetPackages, "OpenCover", "*", "tools",  "OpenCover.Console.exe")
            $openCover = Resolve-Path $openCover

            $coveralls = [System.IO.Path]::Combine($nugetPackages, "coveralls.io", "*", "tools",  "coveralls.net.exe")
            $coveralls = Resolve-Path $coveralls

            $openCoverFormat = "{0} -register:user -target:`"dotnet`" -targetargs:`"test {1} --no-build`" -searchdirs:`"{2}`" -oldstyle -output:coverage.xml -skipautoprops -returntargetcode -filter:`"+[*Tracing]*`""        
            Write-Host ([string]::Format($openCoverFormat, $openCover, $testAssemblies, $serachDirs))

            Invoke-Expression ($openCover + ' -register:user -target:"dotnet" -targetargs:"test ' + $testAssemblies + ' --no-build" -searchdirs:"' + $serachDirs + '" -oldstyle -output:coverage.xml -skipautoprops -returntargetcode -filter:"+[*Tracing]*"')
            Invoke-Expression ($coveralls + ' --opencover coverage.xml')
        }
        else
        {
            # Test
            & $vstest $testAssemblies $vstestFramework $vstestLogger
        }
    }
}