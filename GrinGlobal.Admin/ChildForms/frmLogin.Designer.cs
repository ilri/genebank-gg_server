namespace GrinGlobal.Admin.ChildForms {
    partial class frmLogin {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rdoDatabase = new System.Windows.Forms.RadioButton();
            this.rdoWebService = new System.Windows.Forms.RadioButton();
            this.gbWebService = new System.Windows.Forms.GroupBox();
            this.txtGrinGlobalUrl = new System.Windows.Forms.TextBox();
            this.gbDatabaseConnection = new System.Windows.Forms.GroupBox();
            this.txtSID = new System.Windows.Forms.TextBox();
            this.lblSID = new System.Windows.Forms.Label();
            this.txtDatabaseEngineServerName = new System.Windows.Forms.TextBox();
            this.btnDatabaseEngineTestConnection = new System.Windows.Forms.Button();
            this.chkDatabaseEngineRememberPassword = new System.Windows.Forms.CheckBox();
            this.txtDatabaseEnginePassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtDatabaseEngineUserName = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.chkWindowsAuthentication = new System.Windows.Forms.CheckBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.ddlDatabaseEngine = new System.Windows.Forms.ComboBox();
            this.lblEngine = new System.Windows.Forms.Label();
            this.chkGrinGlobalRememberPassword = new System.Windows.Forms.CheckBox();
            this.txtGrinGlobalPassword = new System.Windows.Forms.TextBox();
            this.lblGrinGlobalPassword = new System.Windows.Forms.Label();
            this.txtGrinGlobalUserName = new System.Windows.Forms.TextBox();
            this.lblGrinGlobalUserName = new System.Windows.Forms.Label();
            this.lblDatabaseTitle = new System.Windows.Forms.Label();
            this.lblLoginTitle = new System.Windows.Forms.Label();
            this.gbGrinGlobalLogin = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpDatabase = new System.Windows.Forms.TabPage();
            this.tpWebApp = new System.Windows.Forms.TabPage();
            this.btnLocateWebConfig = new System.Windows.Forms.Button();
            this.txtWebAppPhysicalPath = new System.Windows.Forms.TextBox();
            this.lblWebConfigPath = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.gbWebService.SuspendLayout();
            this.gbDatabaseConnection.SuspendLayout();
            this.gbGrinGlobalLogin.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpDatabase.SuspendLayout();
            this.tpWebApp.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(107, 94);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 11;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(188, 94);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rdoDatabase
            // 
            this.rdoDatabase.AutoSize = true;
            this.rdoDatabase.Checked = true;
            this.rdoDatabase.Location = new System.Drawing.Point(25, 407);
            this.rdoDatabase.Name = "rdoDatabase";
            this.rdoDatabase.Size = new System.Drawing.Size(168, 17);
            this.rdoDatabase.TabIndex = 12;
            this.rdoDatabase.TabStop = true;
            this.rdoDatabase.Text = "Connect Directly To Database";
            this.rdoDatabase.UseVisualStyleBackColor = true;
            this.rdoDatabase.Visible = false;
            this.rdoDatabase.CheckedChanged += new System.EventHandler(this.rdoDatabase_CheckedChanged);
            // 
            // rdoWebService
            // 
            this.rdoWebService.AutoSize = true;
            this.rdoWebService.Location = new System.Drawing.Point(25, 430);
            this.rdoWebService.Name = "rdoWebService";
            this.rdoWebService.Size = new System.Drawing.Size(146, 17);
            this.rdoWebService.TabIndex = 13;
            this.rdoWebService.Text = "Connect To Web Service";
            this.rdoWebService.UseVisualStyleBackColor = true;
            this.rdoWebService.Visible = false;
            // 
            // gbWebService
            // 
            this.gbWebService.Controls.Add(this.txtGrinGlobalUrl);
            this.gbWebService.Enabled = false;
            this.gbWebService.Location = new System.Drawing.Point(25, 453);
            this.gbWebService.Name = "gbWebService";
            this.gbWebService.Size = new System.Drawing.Size(426, 53);
            this.gbWebService.TabIndex = 16;
            this.gbWebService.TabStop = false;
            this.gbWebService.Visible = false;
            // 
            // txtGrinGlobalUrl
            // 
            this.txtGrinGlobalUrl.Location = new System.Drawing.Point(44, 19);
            this.txtGrinGlobalUrl.Name = "txtGrinGlobalUrl";
            this.txtGrinGlobalUrl.Size = new System.Drawing.Size(376, 20);
            this.txtGrinGlobalUrl.TabIndex = 17;
            // 
            // gbDatabaseConnection
            // 
            this.gbDatabaseConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDatabaseConnection.Controls.Add(this.txtSID);
            this.gbDatabaseConnection.Controls.Add(this.lblSID);
            this.gbDatabaseConnection.Controls.Add(this.txtDatabaseEngineServerName);
            this.gbDatabaseConnection.Controls.Add(this.btnDatabaseEngineTestConnection);
            this.gbDatabaseConnection.Controls.Add(this.chkDatabaseEngineRememberPassword);
            this.gbDatabaseConnection.Controls.Add(this.txtDatabaseEnginePassword);
            this.gbDatabaseConnection.Controls.Add(this.lblPassword);
            this.gbDatabaseConnection.Controls.Add(this.txtDatabaseEngineUserName);
            this.gbDatabaseConnection.Controls.Add(this.lblUsername);
            this.gbDatabaseConnection.Controls.Add(this.chkWindowsAuthentication);
            this.gbDatabaseConnection.Controls.Add(this.lblServer);
            this.gbDatabaseConnection.Controls.Add(this.ddlDatabaseEngine);
            this.gbDatabaseConnection.Controls.Add(this.lblEngine);
            this.gbDatabaseConnection.Location = new System.Drawing.Point(4, 8);
            this.gbDatabaseConnection.Name = "gbDatabaseConnection";
            this.gbDatabaseConnection.Size = new System.Drawing.Size(331, 228);
            this.gbDatabaseConnection.TabIndex = 17;
            this.gbDatabaseConnection.TabStop = false;
            this.gbDatabaseConnection.Text = "Database Connection";
            // 
            // txtSID
            // 
            this.txtSID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSID.Location = new System.Drawing.Point(111, 71);
            this.txtSID.Name = "txtSID";
            this.txtSID.Size = new System.Drawing.Size(204, 20);
            this.txtSID.TabIndex = 2;
            // 
            // lblSID
            // 
            this.lblSID.Location = new System.Drawing.Point(15, 74);
            this.lblSID.Name = "lblSID";
            this.lblSID.Size = new System.Drawing.Size(89, 13);
            this.lblSID.TabIndex = 21;
            this.lblSID.Text = "SID:";
            this.lblSID.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtDatabaseEngineServerName
            // 
            this.txtDatabaseEngineServerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatabaseEngineServerName.Location = new System.Drawing.Point(111, 44);
            this.txtDatabaseEngineServerName.Name = "txtDatabaseEngineServerName";
            this.txtDatabaseEngineServerName.Size = new System.Drawing.Size(204, 20);
            this.txtDatabaseEngineServerName.TabIndex = 1;
            // 
            // btnDatabaseEngineTestConnection
            // 
            this.btnDatabaseEngineTestConnection.Location = new System.Drawing.Point(110, 195);
            this.btnDatabaseEngineTestConnection.Name = "btnDatabaseEngineTestConnection";
            this.btnDatabaseEngineTestConnection.Size = new System.Drawing.Size(117, 23);
            this.btnDatabaseEngineTestConnection.TabIndex = 7;
            this.btnDatabaseEngineTestConnection.Text = "Test Connection";
            this.btnDatabaseEngineTestConnection.UseVisualStyleBackColor = true;
            this.btnDatabaseEngineTestConnection.Click += new System.EventHandler(this.btnDatabaseEngineTestConnection_Click);
            // 
            // chkDatabaseEngineRememberPassword
            // 
            this.chkDatabaseEngineRememberPassword.AutoSize = true;
            this.chkDatabaseEngineRememberPassword.Location = new System.Drawing.Point(110, 172);
            this.chkDatabaseEngineRememberPassword.Name = "chkDatabaseEngineRememberPassword";
            this.chkDatabaseEngineRememberPassword.Size = new System.Drawing.Size(126, 17);
            this.chkDatabaseEngineRememberPassword.TabIndex = 6;
            this.chkDatabaseEngineRememberPassword.Text = "Remember Password";
            this.chkDatabaseEngineRememberPassword.UseVisualStyleBackColor = true;
            // 
            // txtDatabaseEnginePassword
            // 
            this.txtDatabaseEnginePassword.Location = new System.Drawing.Point(110, 146);
            this.txtDatabaseEnginePassword.Name = "txtDatabaseEnginePassword";
            this.txtDatabaseEnginePassword.PasswordChar = '*';
            this.txtDatabaseEnginePassword.Size = new System.Drawing.Size(204, 20);
            this.txtDatabaseEnginePassword.TabIndex = 5;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(15, 149);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(89, 13);
            this.lblPassword.TabIndex = 17;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtDatabaseEngineUserName
            // 
            this.txtDatabaseEngineUserName.Location = new System.Drawing.Point(110, 120);
            this.txtDatabaseEngineUserName.Name = "txtDatabaseEngineUserName";
            this.txtDatabaseEngineUserName.Size = new System.Drawing.Size(204, 20);
            this.txtDatabaseEngineUserName.TabIndex = 4;
            // 
            // lblUsername
            // 
            this.lblUsername.Location = new System.Drawing.Point(15, 123);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(89, 13);
            this.lblUsername.TabIndex = 15;
            this.lblUsername.Text = "User Name:";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkWindowsAuthentication
            // 
            this.chkWindowsAuthentication.AutoSize = true;
            this.chkWindowsAuthentication.Checked = true;
            this.chkWindowsAuthentication.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWindowsAuthentication.Location = new System.Drawing.Point(110, 97);
            this.chkWindowsAuthentication.Name = "chkWindowsAuthentication";
            this.chkWindowsAuthentication.Size = new System.Drawing.Size(163, 17);
            this.chkWindowsAuthentication.TabIndex = 3;
            this.chkWindowsAuthentication.Text = "Use Windows Authentication";
            this.chkWindowsAuthentication.UseVisualStyleBackColor = true;
            this.chkWindowsAuthentication.CheckedChanged += new System.EventHandler(this.chkWindowsAuthentication_CheckedChanged_1);
            // 
            // lblServer
            // 
            this.lblServer.Location = new System.Drawing.Point(15, 47);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(89, 13);
            this.lblServer.TabIndex = 12;
            this.lblServer.Text = "Server:";
            this.lblServer.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ddlDatabaseEngine
            // 
            this.ddlDatabaseEngine.DisplayMember = "DisplayMember";
            this.ddlDatabaseEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDatabaseEngine.FormattingEnabled = true;
            this.ddlDatabaseEngine.Items.AddRange(new object[] {
            "SQL Server 2008 or later",
            "MySQL 5.1 or later",
            "PostgreSQL 8.3 or later",
            "Oracle XE 10g or later"});
            this.ddlDatabaseEngine.Location = new System.Drawing.Point(110, 16);
            this.ddlDatabaseEngine.Name = "ddlDatabaseEngine";
            this.ddlDatabaseEngine.Size = new System.Drawing.Size(204, 21);
            this.ddlDatabaseEngine.TabIndex = 0;
            this.ddlDatabaseEngine.ValueMember = "ValueMember";
            this.ddlDatabaseEngine.SelectedIndexChanged += new System.EventHandler(this.ddlDatabaseEngine_SelectedIndexChanged_1);
            // 
            // lblEngine
            // 
            this.lblEngine.Location = new System.Drawing.Point(12, 19);
            this.lblEngine.Name = "lblEngine";
            this.lblEngine.Size = new System.Drawing.Size(92, 13);
            this.lblEngine.TabIndex = 10;
            this.lblEngine.Text = "Engine:";
            this.lblEngine.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkGrinGlobalRememberPassword
            // 
            this.chkGrinGlobalRememberPassword.AutoSize = true;
            this.chkGrinGlobalRememberPassword.Location = new System.Drawing.Point(107, 71);
            this.chkGrinGlobalRememberPassword.Name = "chkGrinGlobalRememberPassword";
            this.chkGrinGlobalRememberPassword.Size = new System.Drawing.Size(126, 17);
            this.chkGrinGlobalRememberPassword.TabIndex = 10;
            this.chkGrinGlobalRememberPassword.Text = "Remember Password";
            this.chkGrinGlobalRememberPassword.UseVisualStyleBackColor = true;
            // 
            // txtGrinGlobalPassword
            // 
            this.txtGrinGlobalPassword.Location = new System.Drawing.Point(107, 45);
            this.txtGrinGlobalPassword.Name = "txtGrinGlobalPassword";
            this.txtGrinGlobalPassword.PasswordChar = '*';
            this.txtGrinGlobalPassword.Size = new System.Drawing.Size(209, 20);
            this.txtGrinGlobalPassword.TabIndex = 9;
            // 
            // lblGrinGlobalPassword
            // 
            this.lblGrinGlobalPassword.Location = new System.Drawing.Point(9, 48);
            this.lblGrinGlobalPassword.Name = "lblGrinGlobalPassword";
            this.lblGrinGlobalPassword.Size = new System.Drawing.Size(92, 13);
            this.lblGrinGlobalPassword.TabIndex = 22;
            this.lblGrinGlobalPassword.Text = "Password:";
            this.lblGrinGlobalPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtGrinGlobalUserName
            // 
            this.txtGrinGlobalUserName.Location = new System.Drawing.Point(107, 19);
            this.txtGrinGlobalUserName.Name = "txtGrinGlobalUserName";
            this.txtGrinGlobalUserName.Size = new System.Drawing.Size(209, 20);
            this.txtGrinGlobalUserName.TabIndex = 8;
            // 
            // lblGrinGlobalUserName
            // 
            this.lblGrinGlobalUserName.Location = new System.Drawing.Point(12, 22);
            this.lblGrinGlobalUserName.Name = "lblGrinGlobalUserName";
            this.lblGrinGlobalUserName.Size = new System.Drawing.Size(89, 13);
            this.lblGrinGlobalUserName.TabIndex = 20;
            this.lblGrinGlobalUserName.Text = "User Name:";
            this.lblGrinGlobalUserName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblDatabaseTitle
            // 
            this.lblDatabaseTitle.AutoSize = true;
            this.lblDatabaseTitle.Location = new System.Drawing.Point(22, 9);
            this.lblDatabaseTitle.Name = "lblDatabaseTitle";
            this.lblDatabaseTitle.Size = new System.Drawing.Size(327, 13);
            this.lblDatabaseTitle.TabIndex = 25;
            this.lblDatabaseTitle.Text = "To administer GRIN-Global, a valid database connection is required.";
            // 
            // lblLoginTitle
            // 
            this.lblLoginTitle.AutoSize = true;
            this.lblLoginTitle.Location = new System.Drawing.Point(22, 251);
            this.lblLoginTitle.Name = "lblLoginTitle";
            this.lblLoginTitle.Size = new System.Drawing.Size(330, 13);
            this.lblLoginTitle.TabIndex = 26;
            this.lblLoginTitle.Text = "Also, a GRIN-Global user with administrative rights must be specified.";
            // 
            // gbGrinGlobalLogin
            // 
            this.gbGrinGlobalLogin.Controls.Add(this.txtGrinGlobalUserName);
            this.gbGrinGlobalLogin.Controls.Add(this.btnConnect);
            this.gbGrinGlobalLogin.Controls.Add(this.btnCancel);
            this.gbGrinGlobalLogin.Controls.Add(this.chkGrinGlobalRememberPassword);
            this.gbGrinGlobalLogin.Controls.Add(this.lblGrinGlobalUserName);
            this.gbGrinGlobalLogin.Controls.Add(this.txtGrinGlobalPassword);
            this.gbGrinGlobalLogin.Controls.Add(this.lblGrinGlobalPassword);
            this.gbGrinGlobalLogin.Location = new System.Drawing.Point(6, 242);
            this.gbGrinGlobalLogin.Name = "gbGrinGlobalLogin";
            this.gbGrinGlobalLogin.Size = new System.Drawing.Size(327, 131);
            this.gbGrinGlobalLogin.TabIndex = 27;
            this.gbGrinGlobalLogin.TabStop = false;
            this.gbGrinGlobalLogin.Text = "GRIN-Global Login";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpDatabase);
            this.tabControl1.Controls.Add(this.tpWebApp);
            this.tabControl1.Location = new System.Drawing.Point(7, 29);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(346, 415);
            this.tabControl1.TabIndex = 100;
            // 
            // tpDatabase
            // 
            this.tpDatabase.Controls.Add(this.gbDatabaseConnection);
            this.tpDatabase.Controls.Add(this.gbGrinGlobalLogin);
            this.tpDatabase.Location = new System.Drawing.Point(4, 22);
            this.tpDatabase.Name = "tpDatabase";
            this.tpDatabase.Padding = new System.Windows.Forms.Padding(3);
            this.tpDatabase.Size = new System.Drawing.Size(338, 389);
            this.tpDatabase.TabIndex = 0;
            this.tpDatabase.Text = "Database";
            this.tpDatabase.UseVisualStyleBackColor = true;
            // 
            // tpWebApp
            // 
            this.tpWebApp.Controls.Add(this.btnLocateWebConfig);
            this.tpWebApp.Controls.Add(this.txtWebAppPhysicalPath);
            this.tpWebApp.Controls.Add(this.lblWebConfigPath);
            this.tpWebApp.Location = new System.Drawing.Point(4, 22);
            this.tpWebApp.Name = "tpWebApp";
            this.tpWebApp.Size = new System.Drawing.Size(338, 389);
            this.tpWebApp.TabIndex = 2;
            this.tpWebApp.Text = "Web Application";
            this.tpWebApp.UseVisualStyleBackColor = true;
            // 
            // btnLocateWebConfig
            // 
            this.btnLocateWebConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLocateWebConfig.Location = new System.Drawing.Point(288, 38);
            this.btnLocateWebConfig.Name = "btnLocateWebConfig";
            this.btnLocateWebConfig.Size = new System.Drawing.Size(31, 23);
            this.btnLocateWebConfig.TabIndex = 19;
            this.btnLocateWebConfig.Text = "...";
            this.btnLocateWebConfig.UseVisualStyleBackColor = true;
            this.btnLocateWebConfig.Click += new System.EventHandler(this.btnLocateWebConfig_Click);
            // 
            // txtWebAppPhysicalPath
            // 
            this.txtWebAppPhysicalPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWebAppPhysicalPath.Location = new System.Drawing.Point(14, 41);
            this.txtWebAppPhysicalPath.Name = "txtWebAppPhysicalPath";
            this.txtWebAppPhysicalPath.Size = new System.Drawing.Size(268, 20);
            this.txtWebAppPhysicalPath.TabIndex = 18;
            // 
            // lblWebConfigPath
            // 
            this.lblWebConfigPath.AutoSize = true;
            this.lblWebConfigPath.Location = new System.Drawing.Point(11, 25);
            this.lblWebConfigPath.Name = "lblWebConfigPath";
            this.lblWebConfigPath.Size = new System.Drawing.Size(240, 13);
            this.lblWebConfigPath.TabIndex = 7;
            this.lblWebConfigPath.Text = "Directory containing Web.config for GRIN-Global:";
            // 
            // frmLogin
            // 
            this.AcceptButton = this.btnConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(363, 456);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblLoginTitle);
            this.Controls.Add(this.lblDatabaseTitle);
            this.Controls.Add(this.gbWebService);
            this.Controls.Add(this.rdoWebService);
            this.Controls.Add(this.rdoDatabase);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLogin";
            this.ShowInTaskbar = false;
            this.Text = "Connect to Server";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.gbWebService.ResumeLayout(false);
            this.gbWebService.PerformLayout();
            this.gbDatabaseConnection.ResumeLayout(false);
            this.gbDatabaseConnection.PerformLayout();
            this.gbGrinGlobalLogin.ResumeLayout(false);
            this.gbGrinGlobalLogin.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpDatabase.ResumeLayout(false);
            this.tpWebApp.ResumeLayout(false);
            this.tpWebApp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rdoDatabase;
        private System.Windows.Forms.RadioButton rdoWebService;
        private System.Windows.Forms.GroupBox gbWebService;
        private System.Windows.Forms.GroupBox gbDatabaseConnection;
        private System.Windows.Forms.CheckBox chkDatabaseEngineRememberPassword;
        private System.Windows.Forms.TextBox txtDatabaseEnginePassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtDatabaseEngineUserName;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.CheckBox chkWindowsAuthentication;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.ComboBox ddlDatabaseEngine;
        private System.Windows.Forms.Label lblEngine;
        private System.Windows.Forms.CheckBox chkGrinGlobalRememberPassword;
        private System.Windows.Forms.TextBox txtGrinGlobalPassword;
        private System.Windows.Forms.Label lblGrinGlobalPassword;
        private System.Windows.Forms.TextBox txtGrinGlobalUserName;
        private System.Windows.Forms.Label lblGrinGlobalUserName;
        private System.Windows.Forms.Button btnDatabaseEngineTestConnection;
        private System.Windows.Forms.TextBox txtGrinGlobalUrl;
        private System.Windows.Forms.TextBox txtDatabaseEngineServerName;
        private System.Windows.Forms.Label lblDatabaseTitle;
        private System.Windows.Forms.Label lblLoginTitle;
        private System.Windows.Forms.GroupBox gbGrinGlobalLogin;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpDatabase;
        private System.Windows.Forms.TabPage tpWebApp;
        private System.Windows.Forms.Button btnLocateWebConfig;
        private System.Windows.Forms.TextBox txtWebAppPhysicalPath;
        private System.Windows.Forms.Label lblWebConfigPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtSID;
        private System.Windows.Forms.Label lblSID;
    }
}