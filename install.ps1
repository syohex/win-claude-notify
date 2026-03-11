$ErrorActionPreference = "Stop"

$ToolName = "win-claude-notify"
$InstallDir = Join-Path $env:USERPROFILE ".dotnet\tools"
$ProjectDir = $PSScriptRoot

Write-Host "Building $ToolName (Release)..."
dotnet publish $ProjectDir -c Release -r win-x64 --self-contained -o "$ProjectDir\publish"

Write-Host "Installing to $InstallDir..."
New-Item -ItemType Directory -Force -Path $InstallDir | Out-Null
Copy-Item "$ProjectDir\publish\$ToolName.exe" "$InstallDir\$ToolName.exe" -Force

Write-Host "Done. Installed $InstallDir\$ToolName.exe"
