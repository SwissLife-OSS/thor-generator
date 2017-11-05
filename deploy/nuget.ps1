param([string]$Version, [string]$ApiKey)

$packageDir = Join-Path -Path $PSScriptRoot -ChildPath "nuget"
$nupkg = Join-Path -Path $packageDir -ChildPath "esgen.$Version.nupkg"

Push-Location
Set-Location $packageDir

.\nuget.exe pack -Version $Version
.\nuget.exe push $nupkg -source https://api.nuget.org/v3/index.json -apiKey $ApiKey

Set-Location Pop-Location