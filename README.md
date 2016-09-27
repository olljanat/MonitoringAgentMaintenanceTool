# Monitoring Agent Maintenance Tool
This tool can be used to allow server admins to enable/disable SCOM and/or Nimsoft maintenance mode from client side.


## SCOM Specific
Tool uses SCOM databases stored procedures (p_MaintenanceModeStart/p_MaintenanceModeStop/p_MaintenanceModeUpdate) which why tool is very fast and using it can be delegated for users who don't have rights to SCOM server.

This project also contains SQL reporting server report "SCOM_ServersOnMaintenanceMode.rdl" which can be used to report which servers are on maintenance mode.

## Nimsoft Specific
Tool uses sets maintenance mode informations directly to robot.cfg -file which why tool is very fast and using it can be delegated for users who don't have rights to Nimsoft server.


## Releases
| Version | Date       | File                               | MD5 hash                         |
|---------|------------|------------------------------------|----------------------------------|
| 1.0.0.0 | 2016-01-18 | SCOMAgentMaintenanceTool_v1000.zip | 4cb4dd2146cab6463ede5023a2776d8f |

## Installation
You can find binary version from Releases folder.
Do installation using "Run as administrator.cmd" to single computer and using Install.ps1 for mass deployments.

## Configuring
* Configure SCOM database connection string to "SCOMdbConnectionString" variable on MonitoringAgentMaintenanceTool.exe.config
* Disable DebugMode after you are tested that application works on your environment
* Disable / enable SCOM and/or Nimsoft mode depending which one you need.

## SCOM database delegations
You can use this these commands to create "MonitoringAgentMaintenanceToolUser" role to SCOM database and after that you just need give that role for group where all server admins are:
* CREATE ROLE MonitoringAgentMaintenanceToolUser
* GRANT EXECUTE ON p_MaintenanceModeStart TO MonitoringAgentMaintenanceToolUser
* GRANT EXECUTE ON p_MaintenanceModeStop TO MonitoringAgentMaintenanceToolUser
* GRANT EXECUTE ON p_MaintenanceModeUpdate TO MonitoringAgentMaintenanceToolUser

You also need give db_datareader role for users.

## Nimsoft robot.cfg and service permissions delegations
Easiest way delegate robot.cfg modify permissions and "Nimsoft Robot Watcher" -service restart rights is doing it using group policy like this:
![Alt text](https://raw.githubusercontent.com/olljanat/MonitoringAgentMaintenanceTool/master/Screenshots/NimsoftGPO.png "Nimsoft right delegation group policy")
You need grant modify rights to whole "C:\Program Files\Nimsoft\robot" -folder instead of just robot.cfg -file because other why Nimsoft service restart will reset robot.cfg rights back to original ones.

Using these configurations anyone who have access to server and enable/disable Nimsoft robot maintenance mode without UAC elevation.

## Build
* Build using Visual Studio 2015 (Express for Windows Desktop is enough)


## Known issues
* Application will not notice if another user is changed maintenance status.
* Application will not change maintenance status when end time occurs.
* If client and SQL server are on different timezones you need compensate it when you choose length of maintenance mode.

## TODO
* Fix known issues.
* Add command line support.
* All ideas are welcome.

## Screenshots
### Debug mode
![Alt text](https://raw.githubusercontent.com/olljanat/MonitoringAgentMaintenanceTool/master/Screenshots/DebugMode.PNG "Debug mode")
### Normal mode
![Alt text](https://raw.githubusercontent.com/olljanat/MonitoringAgentMaintenanceTool/master/Screenshots/NormalMode_after_start.PNG "Normal mode after start")
![Alt text](https://raw.githubusercontent.com/olljanat/MonitoringAgentMaintenanceTool/master/Screenshots/NormalMode_maintenance_enabled.PNG "Normal mode maintenance enabled")
![Alt text](https://raw.githubusercontent.com/olljanat/MonitoringAgentMaintenanceTool/master/Screenshots/NormalMode_maintenance_updated.PNG "Normal mode maintenance updated")
![Alt text](https://raw.githubusercontent.com/olljanat/MonitoringAgentMaintenanceTool/master/Screenshots/NormalMode_maintenance_disabled.PNG "Normal mode maintenance disabled")
### SQL report
![Alt text](https://raw.githubusercontent.com/olljanat/MonitoringAgentMaintenanceTool/master/Screenshots/SQLreport.PNG "SQL report")