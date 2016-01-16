using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Net;
using System.Net.NetworkInformation;
using System.Globalization;
using System.Data.SqlClient;

namespace SCOMagentMaintenanceTool
{
    public partial class Form1 : Form
    {
        // General strings
        public string strRestartInfoText = "Restart button will be activated after you enable maintenance mode";
        public string strRegistryKey = "SCOMagentMaintenanceTool";
        public string strMaintenanceStatusValue = "MaintenanceStatus";
        public string strMaintenanceUntilValue = "strMaintenanceUntil";
        public string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbx_Reason_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbx_Comment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_Enable_Click(object sender, EventArgs e)
        {
            EnableMaintenanceMode(((KeyValuePair<int, string>)this.cbx_Duration.SelectedItem).Key);
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            UpdateMaintenanceMode(((KeyValuePair<int, string>)this.cbx_Duration.SelectedItem).Key);
        }

        private void btn_Disable_Click(object sender, EventArgs e)
        {
            DisableMaintenanceMode();
        }

        private void btn_Restart_Click(object sender, EventArgs e)
        {
            var confirmRestart = System.Windows.Forms.MessageBox.Show("Are you sure that you want restart this server?", "Confirm restart", MessageBoxButtons.YesNo);
            if (confirmRestart == DialogResult.Yes)
            {
                System.Windows.Forms.MessageBox.Show("Not implemented yet");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width;
            this.Top = Screen.PrimaryScreen.Bounds.Height - this.Height;

            // DEBUG mode
            if (Settings.DebugMode == true)
            {
                this.ClientSize = new System.Drawing.Size(1024, 768);
            }
            else
            {
                this.ClientSize = new System.Drawing.Size(289, 406);
                this.groupBox_DEBUG.Visible = false;
            }

            // Update maintenance status
            UpdateMaintenanceStatus(2,"");

            // Fill duration combobox
            Dictionary<int, string> comboSourceDuration = new Dictionary<int, string>();
            comboSourceDuration.Add(30, "30 minutes");
            comboSourceDuration.Add(60, "1 hour");
            comboSourceDuration.Add(120, "2 hours");
            comboSourceDuration.Add(180, "3 hours");
            comboSourceDuration.Add(240, "4 hours");
            comboSourceDuration.Add(300, "5 hours");
            comboSourceDuration.Add(360, "6 hours");
            comboSourceDuration.Add(420, "7 hours");
            comboSourceDuration.Add(480, "8 hours");
            this.cbx_Duration.DataSource = new BindingSource(comboSourceDuration, null);
            this.cbx_Duration.DisplayMember = "Value";
            this.cbx_Duration.ValueMember = "Key";
            this.cbx_Duration.SelectedIndex = 0;

            // Fill Reason combobox
            this.cbx_Reason.Items.Add("Other (Planned)"); // 0
            this.cbx_Reason.Items.Add("Other (Unplanned)"); // 1
            this.cbx_Reason.Items.Add("Hardware: Maintenance (Planned)"); // 2
            this.cbx_Reason.Items.Add("Hardware: Maintenance (Unplanned)"); // 3
            this.cbx_Reason.Items.Add("Hardware: Installation (Planned)"); // 4
            this.cbx_Reason.Items.Add("Hardware: Installation (Unplanned)"); // 5
            this.cbx_Reason.Items.Add("Operating System: Reconfiguration (Planned)"); // 6
            this.cbx_Reason.Items.Add("Operating System: Reconfiguration (Unplanned)"); // 7
            this.cbx_Reason.Items.Add("Application: Maintenance (Planned)"); // 8
            this.cbx_Reason.Items.Add("Application: Maintenance (Unplanned)"); // 9
            this.cbx_Reason.Items.Add("Application: Installation (Planned)"); // 10
            this.cbx_Reason.Items.Add("Application: Unresponsive"); // 11
            this.cbx_Reason.Items.Add("Application: Unstable"); // 12
            this.cbx_Reason.Items.Add("Security Issue"); // 13
            this.cbx_Reason.Items.Add("Loss of network connectivity (Unplanned)"); // 14
            this.cbx_Reason.SelectedIndex = 0;

            lbl_SCOMconnectInfo.Text = "";
        }

        public void UpdateMaintenanceStatus(int Status, string strMaintenanceUntil)
        {
            RegistryKey RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);

            if (RegistryKey.OpenSubKey(strRegistryKey) == null)
                RegistryKey.CreateSubKey(strRegistryKey);
            RegistryKey = RegistryKey.OpenSubKey(strRegistryKey, true);

            // Status = 2 means that we will just read current value instead of updating it
            if (Status == 2) {
                if (RegistryKey.GetValue(strMaintenanceStatusValue) == null)
                    RegistryKey.SetValue(strMaintenanceStatusValue, 0);
                if (RegistryKey.GetValue(strMaintenanceUntilValue) == null)
                    RegistryKey.SetValue(strMaintenanceUntilValue, "");

                // If maintenance until value is already in past
                strMaintenanceUntil = RegistryKey.GetValue(strMaintenanceUntilValue).ToString();
                if (strMaintenanceUntil != "") {
                    DateTime currentTime = DateTime.Now;
                    DateTime MaintenanceUntil = DateTime.Parse(strMaintenanceUntil);
                    if (currentTime >= MaintenanceUntil)
                    {
                        RegistryKey.SetValue(strMaintenanceStatusValue, 0);
                        RegistryKey.SetValue(strMaintenanceUntilValue, "");
                    }
                }
            }
            else
            {
                RegistryKey.SetValue(strMaintenanceStatusValue, Status);
            }


            object boolMaintenanceStatus = RegistryKey.GetValue(strMaintenanceStatusValue);
            if ((int)boolMaintenanceStatus == 1)
            {
                this.label_MaintenanceStatus.Text = "Enabled";
                this.btn_Restart.Enabled = true;
                this.lbl_restart_info.Text = "";
                this.label_until.Text = "until:";
                this.btn_Disable.Enabled = true;

                // Modify "Enable" button to "Update" button
                this.btn_Enable.Text = "Update";
                this.cbx_Reason.Enabled = false;
                this.txt_Comment.Enabled = false;
                this.btn_Enable.Click -= new System.EventHandler(this.btn_Enable_Click);
                this.btn_Enable.Click += new System.EventHandler(this.btn_Update_Click);

                if (strMaintenanceUntil != "")
                    RegistryKey.SetValue(strMaintenanceUntilValue, strMaintenanceUntil);

                if (RegistryKey.GetValue(strMaintenanceUntilValue) == null)
                    RegistryKey.SetValue(strMaintenanceUntilValue, "");

                strMaintenanceUntil = RegistryKey.GetValue(strMaintenanceUntilValue).ToString();
                this.label_until_value.Text = strMaintenanceUntil;
            }
            else
            {
                this.label_MaintenanceStatus.Text = "Disabled";
                this.btn_Restart.Enabled = false;
                this.lbl_restart_info.Text = strRestartInfoText;
                this.label_until.Text = "";
                this.label_until_value.Text = "";
                this.btn_Disable.Enabled = false;

                // Modify "Update" button to "Enable" button
                this.btn_Enable.Text = "Enable";
                this.cbx_Reason.Enabled = true;
                this.txt_Comment.Enabled = true;
                this.btn_Enable.Click -= new System.EventHandler(this.btn_Update_Click);
                this.btn_Enable.Click += new System.EventHandler(this.btn_Enable_Click);

                RegistryKey.SetValue(strMaintenanceUntilValue, "");
            }
        }

        public void EnableMaintenanceMode(int DurationMin)
        {
            this.lbl_SCOMconnectInfo.Text = "Connecting to SCOM database, please wait...";
            DateTime currentTime = DateTime.Now;
            string strMaintenanceUntil = currentTime.AddMinutes(DurationMin).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            string strSQLquery = @"
DECLARE @BaseManagedTypeID VARCHAR(50)
DECLARE @BaseManagedEntityId VARCHAR(50)
SELECT @BaseManagedTypeID = [BaseManagedTypeID] FROM [dbo].[ManagedType] WHERE [TypeName] = 'Microsoft.Windows.Server.Computer'
SELECT @BaseManagedEntityId = [BaseManagedEntityId] FROM [dbo].[BaseManagedEntity] WHERE [Name] = '" + GetFQDN() + @"' AND [BaseManagedTypeID] = @BaseManagedTypeID

DECLARE @dt_start DateTime, @dt_end DateTime
SET @dt_start = GETUTCDATE()
SELECT @dt_end = DATEADD(Hour, DATEDIFF(Hour, GETDATE(), GETUTCDATE()), CAST('" + strMaintenanceUntil + @"' AS datetime))

EXEC p_MaintenanceModeStart
@BaseManagedEntityID = @BaseManagedEntityId,
@ScheduledEndTime = @dt_end ,
@ReasonCode = " + this.cbx_Reason.SelectedIndex + @",
@Comments = N'" + this.txt_Comment.Text + @"',
@User = N'" + userName + @"',
@Recursive = 1,
@StartTime = @dt_start

";

            if (Settings.DebugMode == true)
                this.txt_DEBUG.Text = "Executing SQL query:" + strSQLquery;

            bool returnCode;
            if (Settings.DebugMode == false)
                returnCode = ExecuteSQLquery(strSQLquery, Settings.SCOMdbConnectionString);
            else
                returnCode = true;

            if (returnCode == true) {
                this.lbl_SCOMconnectInfo.Text = "Maintenance mode enabled";
                UpdateMaintenanceStatus(1, strMaintenanceUntil);
            }
        }

        public void UpdateMaintenanceMode(int DurationMin)
        {
            this.lbl_SCOMconnectInfo.Text = "Connecting to SCOM database, please wait...";
            DateTime currentTime = DateTime.Now;
            string strMaintenanceUntil = currentTime.AddMinutes(DurationMin).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            string strSQLquery = @"
DECLARE @BaseManagedTypeID VARCHAR(50)
DECLARE @BaseManagedEntityId VARCHAR(50)
SELECT @BaseManagedTypeID = [BaseManagedTypeID] FROM [dbo].[ManagedType] WHERE [TypeName] = 'Microsoft.Windows.Server.Computer'
SELECT @BaseManagedEntityId = [BaseManagedEntityId] FROM [dbo].[BaseManagedEntity] WHERE [Name] = '" + GetFQDN() + @"' AND [BaseManagedTypeID] = @BaseManagedTypeID

DECLARE @dt_start DateTime, @dt_end DateTime
SET @dt_start = GETUTCDATE()
SELECT @dt_end = DATEADD(Hour, DATEDIFF(Hour, GETDATE(), GETUTCDATE()), CAST('" + strMaintenanceUntil + @"' AS datetime))

EXEC p_MaintenanceModeUpdate
@BaseManagedEntityID = @BaseManagedEntityId,
@ScheduledEndTime = @dt_end ,
@ReasonCode = " + this.cbx_Reason.SelectedIndex + @",
@Comments = N'" + this.txt_Comment.Text + @"',
@User = N'" + userName + @"',
@Recursive = 1

";

            if (Settings.DebugMode == true)
                this.txt_DEBUG.Text = "Executing SQL query:" + strSQLquery;

            bool returnCode;
            if (Settings.DebugMode == false)
                returnCode = ExecuteSQLquery(strSQLquery, Settings.SCOMdbConnectionString);
            else
                returnCode = true;

            if (returnCode == true)
            {
                this.lbl_SCOMconnectInfo.Text = "Maintenance mode updated";
                UpdateMaintenanceStatus(1, strMaintenanceUntil);
            }
        }

        public void DisableMaintenanceMode()
        {
            this.lbl_SCOMconnectInfo.Text = "Connecting to SCOM database, please wait...";

            string strSQLquery = @"
DECLARE @BaseManagedTypeID VARCHAR(50)
DECLARE @BaseManagedEntityId VARCHAR(50)
SELECT @BaseManagedTypeID = [BaseManagedTypeID] FROM [dbo].[ManagedType] WHERE [TypeName] = 'Microsoft.Windows.Server.Computer'
SELECT @BaseManagedEntityId = [BaseManagedEntityId] FROM [dbo].[BaseManagedEntity] WHERE [Name] = '" + GetFQDN() + @"' AND [BaseManagedTypeID] = @BaseManagedTypeID

DECLARE @dt_end DateTime
SET @dt_end = GETUTCDATE()

EXEC p_MaintenanceModeStop
@BaseManagedEntityID = @BaseManagedEntityId,
@User = N'" + userName + @"',
@Recursive = 1,
@EndTime = @dt_end

";

            if (Settings.DebugMode == true)
                this.txt_DEBUG.Text = "Executing SQL query:" + strSQLquery;

            bool returnCode;
            if (Settings.DebugMode == false)
                returnCode = ExecuteSQLquery(strSQLquery, Settings.SCOMdbConnectionString);
            else
                returnCode = true;

            if (returnCode == true)
            {
                this.lbl_SCOMconnectInfo.Text = "Maintenance mode disabled";
                UpdateMaintenanceStatus(0, "");
            }
        }

        public static string GetFQDN()
        {
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();

            domainName = "." + domainName;
            if (!hostName.EndsWith(domainName))
            {
                hostName += domainName;
            }

            return hostName;
        }

        public bool ExecuteSQLquery(string queryString,
            string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try {
                    command.Connection.Open();
                }
                catch (Exception e)
                {
                    lbl_SCOMconnectInfo.Text += "Connection to SCOM database failed. Check you connection string.";
                    lbl_SCOMconnectInfo.ForeColor = System.Drawing.Color.Red;

                    if (Settings.DebugMode == true)
                        txt_DEBUG.Text += "ERROR: " + e.Message;

                    return false;
                }

                try
                {
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
                catch (Exception e)
                {
                    lbl_SCOMconnectInfo.Text += "Executing SQL query failed. You probably don't have enough rights to SCOM database.";
                    lbl_SCOMconnectInfo.ForeColor = System.Drawing.Color.Red;

                    if (Settings.DebugMode == true)
                        txt_DEBUG.Text += "ERROR: " + e.Message;

                    return false;
                }
            }
            return true;
        }

        public void RestartServer()
        {
            string strCmdText;
            strCmdText = "/r /t 0 /c " + this.txt_Comment + " /d ";
            System.Diagnostics.Process.Start("C:\\Windows\\System32\\shutdown.exe", strCmdText);
        }
    }
}
