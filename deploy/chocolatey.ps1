param([string]$Version, [string]$ApiKey)

$packageDir = Join-Path -Path $PSScriptRoot -ChildPath "chocolatey"
$nuspec = Join-Path -Path $packageDir -ChildPath "esgen.nuspec"
$nupkg = Join-Path -Path $packageDir -ChildPath "esgen.$Version.nupkg"
$installScript = Join-Path -Path $packageDir -ChildPath "tools/chocolateyInstall.ps1"

$installScriptContent = [System.IO.File]::ReadAllText($installScript)
$installScriptContent = $installScriptContent.Replace("{version}", $Version)
[System.IO.File]::WriteAllText($installScript, $installScriptContent)

Set-Location $packageDir
Write-Host "cpack $nuspec --version $Version --outdir $packageDir"
choco pack "$nuspec" --version $Version --outdir "$packageDir"
choco push $nupkg --apikey $ApiKey