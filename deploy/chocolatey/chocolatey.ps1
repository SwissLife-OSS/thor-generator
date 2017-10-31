param([string]$Version, [string]$ApiKey)

$nuspec = Join-Path -Path $PSScriptRoot -ChildPath "esgen.nuspec"
$nupkg = Join-Path -Path $PSScriptRoot -ChildPath "esgen.$Version.nupkg"
$installScript = Join-Path -Path $PSScriptRoot -ChildPath "tools/chocolatey.ps1"

$installScriptContent = [System.IO.File]::ReadAllText($installScript)
$installScriptContent = $installScriptContent.Replace("{version}", $Version)
[System.IO.File]::WriteAllText($installScript, $installScriptContent)

cpack $nuspec --version $Version --outdir $PSScriptRoot
cpush $nupkg --apikey $ApiKey