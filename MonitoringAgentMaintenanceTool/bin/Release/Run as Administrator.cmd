@ECHO OFF
SET /P EnableAutoStart="Do you want enable autostart? [y/n]: "
SET /P DisableWindowsPowerButton="Do you want disable Windows power button? [y/n]: "
SET /P AlwaysShowSystray="Do you want always show systray for all users [y/n]: "

SET COMMAND=%~dp0Install.ps1
IF %EnableAutoStart%==y SET COMMAND=%COMMAND% -EnableAutoStart
IF %DisableWindowsPowerButton%==y SET COMMAND=%COMMAND% -DisableWindowsPowerButton
IF %AlwaysShowSystray%==y SET COMMAND=%COMMAND% -AlwaysShowSystray

C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe -File %COMMAND%
PAUSE