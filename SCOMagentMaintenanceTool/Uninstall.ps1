Param(
	[Parameter(Mandatory=$False)][switch]$EnableWindowsPowerButton,
	[Parameter(Mandatory=$False)][string]$InstallDirectory = "C:\Program Files\SCOMagentMaintenanceTool"
)
If (-NOT ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]"Administrator"))
{
    Write-Warning "You do not have Administrator rights to run this script!`nPlease re-run this script as an Administrator!"
    Break
}

Get-ChildItem $InstallDirectory | Remove-Item -Confirm:$False
Get-ChildItem "$env:PUBLIC\SCOMagentMaintenanceTool.config" | Remove-Item -Confirm:$False
Get-ChildItem "$env:ALLUSERSPROFILE\Microsoft\Windows\Start Menu\Programs\StartUp\SCOMagentMaintenanceTool.lnk" | Remove-Item -Confirm:$False

# Enable Windows power button (use only if was disabled on installation)
If ($EnableWindowsPowerButton) {
	New-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\Explorer" -Name "NoClose" -Value 0 -Force
}
