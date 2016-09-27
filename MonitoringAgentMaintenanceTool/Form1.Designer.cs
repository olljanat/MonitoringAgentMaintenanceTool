namespace MonitoringAgentMaintenanceTool
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btn_Enable = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label_MaintenanceStatus = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_PlannedMaintenance = new System.Windows.Forms.CheckBox();
            this.lbl_SCOMconnectInfo = new System.Windows.Forms.Label();
            this.txt_Comment = new System.Windows.Forms.TextBox();
            this.btn_Disable = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cbx_Reason = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbx_Duration = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Restart = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbl_restart_info = new System.Windows.Forms.Label();
            this.label_until = new System.Windows.Forms.Label();
            this.label_until_value = new System.Windows.Forms.Label();
            this.groupBox_DEBUG = new System.Windows.Forms.GroupBox();
            this.txt_DEBUG = new System.Windows.Forms.TextBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox_DEBUG.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Enable
            // 
            this.btn_Enable.Location = new System.Drawing.Point(9, 202);
            this.btn_Enable.Name = "btn_Enable";
            this.btn_Enable.Size = new System.Drawing.Size(75, 23);
            this.btn_Enable.TabIndex = 0;
            this.btn_Enable.Text = "Enable";
            this.btn_Enable.UseVisualStyleBackColor = true;
            this.btn_Enable.Click += new System.EventHandler(this.btn_Enable_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Maintenance mode:";
            // 
            // label_MaintenanceStatus
            // 
            this.label_MaintenanceStatus.AutoSize = true;
            this.label_MaintenanceStatus.ForeColor = System.Drawing.Color.Red;
            this.label_MaintenanceStatus.Location = new System.Drawing.Point(106, 9);
            this.label_MaintenanceStatus.Name = "label_MaintenanceStatus";
            this.label_MaintenanceStatus.Size = new System.Drawing.Size(37, 13);
            this.label_MaintenanceStatus.TabIndex = 3;
            this.label_MaintenanceStatus.Text = "Status";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_PlannedMaintenance);
            this.groupBox1.Controls.Add(this.lbl_SCOMconnectInfo);
            this.groupBox1.Controls.Add(this.txt_Comment);
            this.groupBox1.Controls.Add(this.btn_Disable);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbx_Reason);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btn_Enable);
            this.groupBox1.Controls.Add(this.cbx_Duration);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(44, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 299);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Maintenance mode";
            // 
            // checkBox_PlannedMaintenance
            // 
            this.checkBox_PlannedMaintenance.AutoSize = true;
            this.checkBox_PlannedMaintenance.Checked = true;
            this.checkBox_PlannedMaintenance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_PlannedMaintenance.Location = new System.Drawing.Point(7, 72);
            this.checkBox_PlannedMaintenance.Name = "checkBox_PlannedMaintenance";
            this.checkBox_PlannedMaintenance.Size = new System.Drawing.Size(129, 17);
            this.checkBox_PlannedMaintenance.TabIndex = 10;
            this.checkBox_PlannedMaintenance.Text = "Planned maintenance";
            this.checkBox_PlannedMaintenance.UseVisualStyleBackColor = true;
            this.checkBox_PlannedMaintenance.CheckedChanged += new System.EventHandler(this.checkBox_PlannedMaintenance_CheckedChanged);
            // 
            // lbl_SCOMconnectInfo
            // 
            this.lbl_SCOMconnectInfo.Location = new System.Drawing.Point(6, 237);
            this.lbl_SCOMconnectInfo.Name = "lbl_SCOMconnectInfo";
            this.lbl_SCOMconnectInfo.Size = new System.Drawing.Size(188, 54);
            this.lbl_SCOMconnectInfo.TabIndex = 9;
            this.lbl_SCOMconnectInfo.Text = "lbl_SCOMconnectInfo";
            // 
            // txt_Comment
            // 
            this.txt_Comment.Location = new System.Drawing.Point(9, 162);
            this.txt_Comment.Name = "txt_Comment";
            this.txt_Comment.Size = new System.Drawing.Size(100, 20);
            this.txt_Comment.TabIndex = 7;
            // 
            // btn_Disable
            // 
            this.btn_Disable.Enabled = false;
            this.btn_Disable.Location = new System.Drawing.Point(108, 202);
            this.btn_Disable.Name = "btn_Disable";
            this.btn_Disable.Size = new System.Drawing.Size(75, 23);
            this.btn_Disable.TabIndex = 6;
            this.btn_Disable.Text = "Disable";
            this.btn_Disable.UseVisualStyleBackColor = true;
            this.btn_Disable.Click += new System.EventHandler(this.btn_Disable_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Comment";
            // 
            // cbx_Reason
            // 
            this.cbx_Reason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_Reason.FormattingEnabled = true;
            this.cbx_Reason.Location = new System.Drawing.Point(9, 109);
            this.cbx_Reason.Name = "cbx_Reason";
            this.cbx_Reason.Size = new System.Drawing.Size(185, 21);
            this.cbx_Reason.TabIndex = 3;
            this.cbx_Reason.SelectedIndexChanged += new System.EventHandler(this.cbx_Reason_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Reason";
            // 
            // cbx_Duration
            // 
            this.cbx_Duration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_Duration.FormattingEnabled = true;
            this.cbx_Duration.Location = new System.Drawing.Point(9, 41);
            this.cbx_Duration.Name = "cbx_Duration";
            this.cbx_Duration.Size = new System.Drawing.Size(121, 21);
            this.cbx_Duration.TabIndex = 1;
            this.cbx_Duration.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Duration";
            // 
            // btn_Restart
            // 
            this.btn_Restart.Enabled = false;
            this.btn_Restart.Location = new System.Drawing.Point(44, 47);
            this.btn_Restart.Name = "btn_Restart";
            this.btn_Restart.Size = new System.Drawing.Size(96, 23);
            this.btn_Restart.TabIndex = 5;
            this.btn_Restart.Text = "Restart server";
            this.btn_Restart.UseVisualStyleBackColor = true;
            this.btn_Restart.Click += new System.EventHandler(this.btn_Restart_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbl_restart_info);
            this.groupBox2.Controls.Add(this.btn_Restart);
            this.groupBox2.Location = new System.Drawing.Point(44, 347);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 76);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Restart server";
            // 
            // lbl_restart_info
            // 
            this.lbl_restart_info.ForeColor = System.Drawing.Color.Red;
            this.lbl_restart_info.Location = new System.Drawing.Point(6, 16);
            this.lbl_restart_info.Name = "lbl_restart_info";
            this.lbl_restart_info.Size = new System.Drawing.Size(176, 28);
            this.lbl_restart_info.TabIndex = 6;
            this.lbl_restart_info.Text = "Restart info text";
            // 
            // label_until
            // 
            this.label_until.AutoSize = true;
            this.label_until.Location = new System.Drawing.Point(149, 9);
            this.label_until.Name = "label_until";
            this.label_until.Size = new System.Drawing.Size(29, 13);
            this.label_until.TabIndex = 7;
            this.label_until.Text = "until:";
            // 
            // label_until_value
            // 
            this.label_until_value.AutoSize = true;
            this.label_until_value.Location = new System.Drawing.Point(177, 9);
            this.label_until_value.Name = "label_until_value";
            this.label_until_value.Size = new System.Drawing.Size(106, 13);
            this.label_until_value.TabIndex = 8;
            this.label_until_value.Text = "2000-01-01 00:00:00";
            // 
            // groupBox_DEBUG
            // 
            this.groupBox_DEBUG.Controls.Add(this.txt_DEBUG);
            this.groupBox_DEBUG.Location = new System.Drawing.Point(12, 429);
            this.groupBox_DEBUG.Name = "groupBox_DEBUG";
            this.groupBox_DEBUG.Size = new System.Drawing.Size(1007, 319);
            this.groupBox_DEBUG.TabIndex = 10;
            this.groupBox_DEBUG.TabStop = false;
            this.groupBox_DEBUG.Text = "Debug information";
            // 
            // txt_DEBUG
            // 
            this.txt_DEBUG.Location = new System.Drawing.Point(7, 19);
            this.txt_DEBUG.Multiline = true;
            this.txt_DEBUG.Name = "txt_DEBUG";
            this.txt_DEBUG.ReadOnly = true;
            this.txt_DEBUG.Size = new System.Drawing.Size(993, 330);
            this.txt_DEBUG.TabIndex = 0;
            this.txt_DEBUG.Text = "DEBUG";
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "SCOM Agent Maintenance Tool";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.label_until_value);
            this.Controls.Add(this.label_until);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label_MaintenanceStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox_DEBUG);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Monitoring Agent Maintenance Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox_DEBUG.ResumeLayout(false);
            this.groupBox_DEBUG.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Enable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_MaintenanceStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbx_Duration;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbx_Reason;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_Restart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbl_restart_info;
        private System.Windows.Forms.Button btn_Disable;
        private System.Windows.Forms.Label label_until;
        private System.Windows.Forms.Label label_until_value;
        private System.Windows.Forms.TextBox txt_Comment;
        private System.Windows.Forms.Label lbl_SCOMconnectInfo;
        private System.Windows.Forms.GroupBox groupBox_DEBUG;
        private System.Windows.Forms.TextBox txt_DEBUG;
        private System.Windows.Forms.CheckBox checkBox_PlannedMaintenance;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}

