using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCOMagentMaintenanceTool
{
    public static class Settings
    {
        // Connection string to SCOM database
        public static string SCOMdbConnectionString = "Data Source=SqlServer;Initial Catalog=OperationsManager;Integrated Security=True;";

        // Show/Hide debug section
        public static bool DebugMode = true;

        // Test UI without connecting to SCOM
        public static bool DemoMode = false;
    }
}
