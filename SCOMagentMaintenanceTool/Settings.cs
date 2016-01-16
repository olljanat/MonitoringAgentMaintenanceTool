using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCOMagentMaintenanceTool
{
    public static class Settings
    {
        public static string SCOMdbConnectionString = "Data Source=SqlServer;Initial Catalog=OperationsManager;Integrated Security=True;";
        public static bool DebugMode = true;
    }
}
