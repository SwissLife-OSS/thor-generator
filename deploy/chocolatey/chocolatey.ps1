param([string]$Version, [string]$ApiKey)

$nuspec = Join-Path -Path $PSScriptRoot -ChildPath "esgen.nuspec"
$nupkg = Join-Path -Path $PSScriptRoot -ChildPath "esgen.$Version.nupkg"

cpack $nuspec --version $Version --outdir $PSScriptRoot
cpush $nupkg --apikey $ApiKey