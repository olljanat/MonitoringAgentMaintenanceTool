Param(
	[Parameter(Mandatory=$False)][string]$InstallDirectory = "C:\Program Files\SCOMagentMaintenanceTool",
	[Parameter(Mandatory=$False)][switch]$EnableAutoStart,
	[Parameter(Mandatory=$False)][switch]$DisableWindowsPowerButton
)
If (-NOT ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]"Administrator"))
{
    Write-Warning "You do not have Administrator rights to run this script!`nPlease re-run this script as an Administrator!"
    Break
}

$scriptPath = Split-Path -parent $MyInvocation.MyCommand.Definition

If (-NOT (Test-Path ($InstallDirectory))) {
	New-Item -ItemType Directory -Path $InstallDirectory
}

# Install/update
Copy-Item -Path "$scriptPath\SCOMagentMaintenanceTool.exe" -Destination $InstallDirectory -Force
Copy-Item -Path "$scriptPath\SCOMagentMaintenanceTool.exe.config" -Destination $InstallDirectory -Force
Copy-Item -Path "$scriptPath\SCOMagentMaintenanceTool.config" -Destination $env:PUBLIC -Force

## Auto start on login
If ($EnableAutoStart) {
	$TargetFile = $InstallDirectory + "\SCOMagentMaintenanceTool.exe"
	$ShortcutFile = $env:ALLUSERSPROFILE + "\Microsoft\Windows\Start Menu\Programs\StartUp\SCOMagentMaintenanceTool.lnk"
	$WScriptShell = New-Object -ComObject WScript.Shell
	$Shortcut = $WScriptShell.CreateShortcut($ShortcutFile)
	$Shortcut.TargetPath = $TargetFile
	$Shortcut.Save()
}

# Disable Windows power button so users must use this tool for reboot (NOTE! shutdown.exe still works)
If ($DisableWindowsPowerButton) {
	New-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\Explorer" -Name "NoClose" -Value 1 -Force
}