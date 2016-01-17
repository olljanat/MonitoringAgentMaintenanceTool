using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SCOMagentMaintenanceTool
{
    public partial class Form1 : Form
    {
        // General strings/settings
        public int intFormWidth;
        public int intFormWidthDebugMode = 1024;
        public int intFormWidthNormalMode = 288;
        public string strRestartInfoText = "Restart button will be activated after you enable maintenance mode";
        public string strMaintenanceStatusValue = "MaintenanceStatus";
        public string strMaintenanceUntilValue = "MaintenanceUntil";
        public string strPlannedMaintenanceValue = "PlannedMaintenance";
        public string strMaintenanceReasonValue = "MaintenananceReason";
        public string strMaintenanceCommentValue = "MaintenananceComment";
        public string strMaintenanceEnabledByValue = "MaintenanceEnabledBy";
        public string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        public string SCOMdbConnectionString;
        public bool DebugMode = false;
        public bool DemoMode = false;
        public string TempConfigFile = System.Environment.GetEnvironmentVariable("public") + "\\SCOMagentMaintenanceTool.config";
        public ExeConfigurationFileMap TempConfigMap = new ExeConfigurationFileMap();
        public Configuration TempConfig;

        public Form1()
        {

            if (ConfigurationManager.AppSettings["DebugMode"] == "true")
                intFormWidth = intFormWidthDebugMode;
            else
                intFormWidth = intFormWidthNormalMode;
            InitializeComponent();
            if (ConfigurationManager.AppSettings["LocationTopRight"] == "true")
            {
                this.StartPosition = FormStartPosition.Manual;

                foreach (var scrn in Screen.AllScreens)
                {
                    if (scrn.Bounds.Contains(this.Location))
                    {
                        this.Location = new Point(scrn.Bounds.Right - intFormWidth, scrn.Bounds.Top);
                        return;
                    }
                }
            }
        }

        // Disable close button
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                if (ConfigurationManager.AppSettings["DisableCloseButton"] == "true")
                    myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                else
                    myCp.ClassStyle = myCp.ClassStyle;
                return myCp;
            }
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
                RestartServer();
            }
        }

        private void checkBox_PlannedMaintenance_CheckedChanged(object sender, EventArgs e)
        {
            updateReasonCombobox();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Read settings
            SCOMdbConnectionString = ConfigurationManager.AppSettings["SCOMdbConnectionString"];
            DebugMode = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugMode"]);
            DemoMode = Convert.ToBoolean(ConfigurationManager.AppSettings["DemoMode"]);

            // Load temporary settings
            TempConfigMap.ExeConfigFilename = TempConfigFile;
            TempConfig = ConfigurationManager.OpenMappedExeConfiguration(TempConfigMap, ConfigurationUserLevel.None);

            lbl_SCOMconnectInfo.Text = "";

            // DEBUG mode
            if (ConfigurationManager.AppSettings["DebugMode"] == "true")
            {
                this.ClientSize = new System.Drawing.Size(1024, 768);
            }
            else
            {
                this.ClientSize = new System.Drawing.Size(288, 430);
                this.groupBox_DEBUG.Visible = false;
            }

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

            // Fill reason combobox
            updateReasonCombobox();

            // Update maintenance status
            GetMaintenanceStatus();
        }

        public void updateReasonCombobox()
        {
            Dictionary<int, string> comboSourceReason = new Dictionary<int, string>();
            if (((CheckBox)this.checkBox_PlannedMaintenance).CheckState == CheckState.Checked)
            {
                comboSourceReason.Add(0, "Other (Planned)");
                comboSourceReason.Add(2, "Hardware: Maintenance (Planned)");
                comboSourceReason.Add(4, "Hardware: Installation (Planned)");
                comboSourceReason.Add(6, "Operating System: Reconfiguration (Planned)");
                comboSourceReason.Add(8, "Application: Maintenance (Planned)");
                comboSourceReason.Add(10, "Application: Installation (Planned)");
            }
            else
            {
                comboSourceReason.Add(1, "Other (Unplanned)");
                comboSourceReason.Add(3, "Hardware: Maintenance (Unplanned)");
                comboSourceReason.Add(5, "Hardware: Installation (Unplanned)");
                comboSourceReason.Add(7, "Operating System: Reconfiguration (Unplanned)");
                comboSourceReason.Add(9, "Application: Maintenance (Unplanned)");
                comboSourceReason.Add(11, "Application: Unresponsive");
                comboSourceReason.Add(12, "Application:  Unstable");
                comboSourceReason.Add(13, "Security Issue");

                // Disabled because it is not possible to use this tool without network connectivity
                // comboSourceReason.Add(14, "Loss of network connectivity (Unplanned)");
            }

            this.cbx_Reason.DataSource = new BindingSource(comboSourceReason, null);
            this.cbx_Reason.DisplayMember = "Value";
            this.cbx_Reason.ValueMember = "Key";
            this.cbx_Reason.SelectedIndex = 0;
        }

        public void GetMaintenanceStatus()
        {
            if (!(System.IO.File.Exists(TempConfigFile)))
            {
                System.Windows.Forms.MessageBox.Show("Temporary configuration file: " + TempConfigFile + " missing");
                System.Environment.Exit(1);
            }

            bool boolMaintenanceStatus = Convert.ToBoolean(TempConfig.AppSettings.Settings["MaintenanceStatus"].Value);
            string strMaintenanceUntil = TempConfig.AppSettings.Settings["MaintenanceUntil"].Value;
            int intPlannedMaintenanceSelection, intPlannedMaintenanceReason;
            int.TryParse(TempConfig.AppSettings.Settings["PlannedMaintenance"].Value, out intPlannedMaintenanceSelection);
            int.TryParse(TempConfig.AppSettings.Settings["MaintenananceReason"].Value, out intPlannedMaintenanceReason);
            string strMaintenanceComment = TempConfig.AppSettings.Settings["MaintenananceComment"].Value;
            string strMaintenanceEnabledBy = TempConfig.AppSettings.Settings["MaintenanceEnabledBy"].Value;

            DateTime currentTime = DateTime.Now;
            DateTime MaintenanceUntil = currentTime;
            if (strMaintenanceUntil != "")
                MaintenanceUntil = DateTime.Parse(strMaintenanceUntil);

            // If maintenance mode is already ended
            if ((boolMaintenanceStatus == true) && (strMaintenanceUntil != "") && (currentTime >= MaintenanceUntil))
            {
                TempConfig.AppSettings.Settings["MaintenanceStatus"].Value = "false";
                TempConfig.AppSettings.Settings["MaintenanceUntil"].Value = "";
                TempConfig.AppSettings.Settings["PlannedMaintenance"].Value = "0";
                TempConfig.AppSettings.Settings["MaintenananceReason"].Value = "";
                TempConfig.AppSettings.Settings["MaintenananceComment"].Value = "";
                TempConfig.AppSettings.Settings["MaintenanceEnabledBy"].Value = "";
                TempConfig.Save(ConfigurationSaveMode.Modified);
            }
            else if (boolMaintenanceStatus == true)
            {
                this.label_MaintenanceStatus.Text = "Enabled";
                this.btn_Restart.Enabled = true;
                this.lbl_restart_info.Text = "";
                this.label_until.Text = "until:";
                this.btn_Disable.Enabled = true;
                this.checkBox_PlannedMaintenance.Enabled = false;
                this.label_until_value.Text = strMaintenanceUntil;
                if (intPlannedMaintenanceSelection == 1)
                    this.checkBox_PlannedMaintenance.CheckState = CheckState.Checked;
                else
                    this.checkBox_PlannedMaintenance.CheckState = CheckState.Unchecked;
                this.cbx_Reason.SelectedIndex = intPlannedMaintenanceReason;
                this.txt_Comment.Text = strMaintenanceComment;
                this.lbl_SCOMconnectInfo.Text = "Maintenance mode enabled by: " + strMaintenanceEnabledBy;

                // Modify "Enable" button to "Update" button
                this.btn_Enable.Text = "Update";
                this.cbx_Reason.Enabled = false;
                this.txt_Comment.Enabled = false;
                this.btn_Enable.Click -= new System.EventHandler(this.btn_Enable_Click);
                this.btn_Enable.Click += new System.EventHandler(this.btn_Update_Click);
            }
            else
            {
                this.label_MaintenanceStatus.Text = "Disabled";
                this.btn_Restart.Enabled = false;
                this.lbl_restart_info.Text = strRestartInfoText;
                this.label_until.Text = "";
                this.label_until_value.Text = "";
                this.btn_Disable.Enabled = false;
                this.checkBox_PlannedMaintenance.Enabled = true;

                // Modify "Update" button to "Enable" button
                this.btn_Enable.Text = "Enable";
                this.cbx_Reason.Enabled = true;
                this.txt_Comment.Enabled = true;
                this.btn_Enable.Click -= new System.EventHandler(this.btn_Update_Click);
                this.btn_Enable.Click += new System.EventHandler(this.btn_Enable_Click);
            }
        }

        public void UpdateMaintenanceStatus(int Status, string strMaintenanceUntil)
        {
            if (Status == 1)
            {
                // new code
                TempConfig.AppSettings.Settings["MaintenanceStatus"].Value = "true";
                TempConfig.AppSettings.Settings["MaintenanceUntil"].Value = strMaintenanceUntil;
                if (((CheckBox)this.checkBox_PlannedMaintenance).CheckState == CheckState.Checked)
                    TempConfig.AppSettings.Settings["PlannedMaintenance"].Value = "1";
                else
                    TempConfig.AppSettings.Settings["PlannedMaintenance"].Value = "0";
                TempConfig.AppSettings.Settings["MaintenananceReason"].Value = this.cbx_Reason.SelectedIndex.ToString();
                TempConfig.AppSettings.Settings["MaintenananceComment"].Value = this.txt_Comment.Text;
                TempConfig.AppSettings.Settings["MaintenanceEnabledBy"].Value = userName;
                TempConfig.Save(ConfigurationSaveMode.Modified);
            }
            else
            {
                TempConfig.AppSettings.Settings["MaintenanceStatus"].Value = "false";
                TempConfig.AppSettings.Settings["MaintenanceUntil"].Value = "";
                TempConfig.AppSettings.Settings["PlannedMaintenance"].Value = "0";
                TempConfig.AppSettings.Settings["MaintenananceReason"].Value = "";
                TempConfig.AppSettings.Settings["MaintenananceComment"].Value = "";
                TempConfig.AppSettings.Settings["MaintenanceEnabledBy"].Value = "";
                TempConfig.Save(ConfigurationSaveMode.Modified);
            }

            GetMaintenanceStatus();
        }

        public void EnableMaintenanceMode(int DurationMin)
        {
            this.lbl_SCOMconnectInfo.Text = "Connecting to SCOM database, please wait...";
            DateTime currentTime = DateTime.Now;
            string strMaintenanceUntil = currentTime.AddMinutes(DurationMin).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            int intReasonCode = ((KeyValuePair<int, string>)this.cbx_Reason.SelectedItem).Key;

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
@ReasonCode = " + intReasonCode + @",
@Comments = N'" + this.txt_Comment.Text + @"',
@User = N'" + userName + @"',
@Recursive = 1,
@StartTime = @dt_start

";

            if (DebugMode == true)
                this.txt_DEBUG.Text = "Executing SQL query:" + strSQLquery;

            bool returnCode;
            if (DemoMode == false)
                returnCode = ExecuteSQLquery(strSQLquery, SCOMdbConnectionString);
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
            int intReasonCode = ((KeyValuePair<int, string>)this.cbx_Reason.SelectedItem).Key;

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
@ReasonCode = " + intReasonCode + @",
@Comments = N'" + this.txt_Comment.Text + @"',
@User = N'" + userName + @"',
@Recursive = 1

";

            if (DebugMode == true)
                this.txt_DEBUG.Text = "Executing SQL query:" + strSQLquery;

            bool returnCode;
            if (DemoMode == false)
                returnCode = ExecuteSQLquery(strSQLquery, SCOMdbConnectionString);
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

            if (DebugMode == true)
                this.txt_DEBUG.Text = "Executing SQL query:" + strSQLquery;

            bool returnCode;
            if (DemoMode == false)
                returnCode = ExecuteSQLquery(strSQLquery, SCOMdbConnectionString);
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

                    if (DebugMode == true)
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

                    if (DebugMode == true)
                        txt_DEBUG.Text += "ERROR: " + e.Message;

                    return false;
                }
            }
            return true;
        }

        public void RestartServer()
        {
            int intReasonCode = ((KeyValuePair<int, string>)this.cbx_Reason.SelectedItem).Key;

            // Converting SCOM reason code to shutdown.exe format
            string strShutdownReason;
            switch (intReasonCode)
            {
                case 0:
                    strShutdownReason = "p:0:0";
                    break;
                case 2:
                    strShutdownReason = "p:1:1";
                    break;
                case 3:
                    strShutdownReason = "u:1:1";
                    break;
                case 4:
                    strShutdownReason = "p:1:2";
                    break;
                case 5:
                    strShutdownReason = "u:1:2";
                    break;
                case 6:
                    strShutdownReason = "p:2:4";
                    break;
                case 7:
                    strShutdownReason = "u:2:4";
                    break;
                case 8:
                    strShutdownReason = "p:4:1";
                    break;
                case 9:
                    strShutdownReason = "u:4:1";
                    break;
                case 10:
                    strShutdownReason = "p:4:2";
                    break;
                case 11:
                    strShutdownReason = "u:4:5";
                    break;
                case 12:
                    strShutdownReason = "u:4:6";
                    break;
                case 13:
                    strShutdownReason = "u:5:19";
                    break;
                case 14:
                    strShutdownReason = "u:5:20";
                    break;
                default:
                    strShutdownReason = "u:0:0";
                    break;
            }

            string strCmdText;
            strCmdText = "/r /t 0 /c \"" + this.txt_Comment.Text + "\" /d "+ strShutdownReason;

            if (DebugMode == true)
                this.txt_DEBUG.Text = strCmdText;

            if (DemoMode == false)
                System.Diagnostics.Process.Start("C:\\Windows\\System32\\shutdown.exe", strCmdText);
        }
    }
}
