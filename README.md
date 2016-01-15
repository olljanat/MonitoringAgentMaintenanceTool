# SCOM Agent Maintenance Tool
This tool can be used to allow server admins to enable/disable SCOM maintenance from any server.

Tool uses SCOM databases stored procedures (p_MaintenanceModeStart/p_MaintenanceModeStop/p_MaintenanceModeUpdate) which why tool is very fast and using it can be delegated for users who don't have rights to SCOM server.

This project also contains SQL reporting server report "SCOM_ServersOnMaintenanceMode.rdl" which can be used to report which servers are on maintenance mode.


## Configuring / build
* Configure SCOM database connection string to "SCOMdbConnectionString" variable on Settings.cs
* Build using Visual Studio 2015 (Express for Windows Desktop is enought)
* Disable DebugMode after you are tested that application works on your environment and deploy it to all servers.

## SCOM database delegations
You can use this these commands to create "SCOMagentMaintenanceToolUser" role to SCOM database and after that you just need give that role for group where all server admins are:
CREATE ROLE SCOMagentMaintenanceToolUser
GRANT EXECUTE ON p_MaintenanceModeStart TO SCOMagentMaintenanceToolUser
GRANT EXECUTE ON p_MaintenanceModeStop TO SCOMagentMaintenanceToolUser
GRANT EXECUTE ON p_MaintenanceModeUpdate TO SCOMagentMaintenanceToolUser