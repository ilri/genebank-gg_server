namespace GrinGlobal.InstallHelper {
    partial class frmDatabaseLoginPrompt {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDatabaseLoginPrompt));
            this.chkWindowsAuthentication = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ddlEngine = new System.Windows.Forms.ComboBox();
            this.lblServerName = new System.Windows.Forms.Label();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.txtSID = new System.Windows.Forms.TextBox();
            this.lblSID = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtConnectionstring = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnServerName = new System.Windows.Forms.Button();
            this.lblWarning = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lnkMixedMode = new System.Windows.Forms.LinkLabel();
            this.lblAutoRetrying = new System.Windows.Forms.Label();
            this.btnSkip = new System.Windows.Forms.Button();
            this.lblPasswordRequired = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkWindowsAuthentication
            // 
            this.chkWindowsAuthentication.AutoSize = true;
            this.chkWindowsAuthentication.Location = new System.Drawing.Point(110, 211);
            this.chkWindowsAuthentication.Name = "chkWindowsAuthentication";
            this.chkWindowsAuthentication.Size = new System.Drawing.Size(163, 17);
            this.chkWindowsAuthentication.TabIndex = 4;
            this.chkWindowsAuthentication.Text = "Use Windows Authentication";
            this.chkWindowsAuthentication.UseVisualStyleBackColor = true;
            this.chkWindowsAuthentication.CheckedChanged += new System.EventHandler(this.chkWindowsAuthentication_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(233, 341);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(110, 286);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(117, 23);
            this.btnTestConnection.TabIndex = 7;
            this.btnTestConnection.Text = "&Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(110, 260);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(227, 20);
            this.txtPassword.TabIndex = 6;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(18, 263);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(86, 13);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Engine:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ddlEngine
            // 
            this.ddlEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlEngine.FormattingEnabled = true;
            this.ddlEngine.Items.AddRange(new object[] {
            "SQL Server 2008 or later"});
            this.ddlEngine.Location = new System.Drawing.Point(110, 59);
            this.ddlEngine.Name = "ddlEngine";
            this.ddlEngine.Size = new System.Drawing.Size(227, 21);
            this.ddlEngine.TabIndex = 0;
            this.ddlEngine.SelectedIndexChanged += new System.EventHandler(this.ddlEngine_SelectedIndexChanged);
            // 
            // lblServerName
            // 
            this.lblServerName.Location = new System.Drawing.Point(15, 89);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(89, 13);
            this.lblServerName.TabIndex = 13;
            this.lblServerName.Text = "Server:";
            this.lblServerName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(110, 86);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(227, 20);
            this.txtServerName.TabIndex = 1;
            this.txtServerName.Text = "localhost";
            this.txtServerName.TextChanged += new System.EventHandler(this.txtServerName_TextChanged);
            // 
            // txtSID
            // 
            this.txtSID.Location = new System.Drawing.Point(111, 141);
            this.txtSID.Name = "txtSID";
            this.txtSID.Size = new System.Drawing.Size(227, 20);
            this.txtSID.TabIndex = 3;
            this.txtSID.Text = "*INVALID*";
            this.txtSID.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lblSID
            // 
            this.lblSID.Location = new System.Drawing.Point(15, 141);
            this.lblSID.Name = "lblSID";
            this.lblSID.Size = new System.Drawing.Size(90, 13);
            this.lblSID.TabIndex = 15;
            this.lblSID.Text = "SID:";
            this.lblSID.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(110, 112);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(227, 20);
            this.txtPort.TabIndex = 2;
            this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(15, 115);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(89, 13);
            this.lblPort.TabIndex = 17;
            this.lblPort.Text = "Port:";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(110, 234);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(227, 20);
            this.txtUsername.TabIndex = 5;
            // 
            // lblUsername
            // 
            this.lblUsername.Location = new System.Drawing.Point(18, 237);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(86, 13);
            this.lblUsername.TabIndex = 19;
            this.lblUsername.Text = "User:";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtConnectionstring
            // 
            this.txtConnectionstring.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionstring.Location = new System.Drawing.Point(110, 315);
            this.txtConnectionstring.Name = "txtConnectionstring";
            this.txtConnectionstring.Size = new System.Drawing.Size(322, 20);
            this.txtConnectionstring.TabIndex = 21;
            this.txtConnectionstring.Enter += new System.EventHandler(this.txtConnectionstring_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 318);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Connection string:";
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(110, 341);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(117, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "&Save and Continue";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Location = new System.Drawing.Point(12, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(419, 43);
            this.lblTitle.TabIndex = 23;
            this.lblTitle.Text = "You must specify the database connection information for configuration to continu" +
                "e.\r\n";
            // 
            // btnServerName
            // 
            this.btnServerName.Location = new System.Drawing.Point(343, 86);
            this.btnServerName.Name = "btnServerName";
            this.btnServerName.Size = new System.Drawing.Size(39, 23);
            this.btnServerName.TabIndex = 26;
            this.btnServerName.Text = "...";
            this.btnServerName.UseVisualStyleBackColor = true;
            this.btnServerName.Click += new System.EventHandler(this.btnServerName_Click);
            // 
            // lblWarning
            // 
            this.lblWarning.Location = new System.Drawing.Point(108, 168);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(291, 40);
            this.lblWarning.TabIndex = 27;
            this.lblWarning.Text = "* GRIN-Global has not been fully regression tested against this database engine a" +
                "nd may exhibit problems.";
            this.lblWarning.Visible = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lnkMixedMode
            // 
            this.lnkMixedMode.AutoSize = true;
            this.lnkMixedMode.Location = new System.Drawing.Point(340, 263);
            this.lnkMixedMode.Name = "lnkMixedMode";
            this.lnkMixedMode.Size = new System.Drawing.Size(136, 13);
            this.lnkMixedMode.TabIndex = 28;
            this.lnkMixedMode.TabStop = true;
            this.lnkMixedMode.Text = "Enable SQL Server Login...";
            this.lnkMixedMode.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMixedMode_LinkClicked);
            // 
            // lblAutoRetrying
            // 
            this.lblAutoRetrying.AutoSize = true;
            this.lblAutoRetrying.Location = new System.Drawing.Point(233, 291);
            this.lblAutoRetrying.Name = "lblAutoRetrying";
            this.lblAutoRetrying.Size = new System.Drawing.Size(75, 13);
            this.lblAutoRetrying.TabIndex = 29;
            this.lblAutoRetrying.Text = "Auto retrying...";
            this.lblAutoRetrying.Visible = false;
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point(294, 341);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(138, 23);
            this.btnSkip.TabIndex = 30;
            this.btnSkip.Text = "Skip Database Actions";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // lblPasswordRequired
            // 
            this.lblPasswordRequired.AutoSize = true;
            this.lblPasswordRequired.Location = new System.Drawing.Point(340, 291);
            this.lblPasswordRequired.Name = "lblPasswordRequired";
            this.lblPasswordRequired.Size = new System.Drawing.Size(157, 13);
            this.lblPasswordRequired.TabIndex = 31;
            this.lblPasswordRequired.Text = "Non-empty password is required";
            // 
            // frmDatabaseLoginPrompt
            // 
            this.AcceptButton = this.btnTestConnection;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(443, 379);
            this.Controls.Add(this.lblPasswordRequired);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.lblAutoRetrying);
            this.Controls.Add(this.lnkMixedMode);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.btnServerName);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtConnectionstring);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txtSID);
            this.Controls.Add(this.lblSID);
            this.Controls.Add(this.txtServerName);
            this.Controls.Add(this.lblServerName);
            this.Controls.Add(this.ddlEngine);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkWindowsAuthentication);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDatabaseLoginPrompt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GRIN-Global Database Connection";
            this.Load += new System.EventHandler(this.frmDatabaseLoginPrompt_Load);
            this.Activated += new System.EventHandler(this.frmDatabaseLoginPrompt_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDatabaseLoginPrompt_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.Label lblSID;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.CheckBox chkWindowsAuthentication;
        public System.Windows.Forms.TextBox txtPassword;
        public System.Windows.Forms.ComboBox ddlEngine;
        public System.Windows.Forms.TextBox txtServerName;
        public System.Windows.Forms.TextBox txtSID;
        public System.Windows.Forms.TextBox txtPort;
        public System.Windows.Forms.TextBox txtUsername;
        public System.Windows.Forms.TextBox txtConnectionstring;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnServerName;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.LinkLabel lnkMixedMode;
        private System.Windows.Forms.Label lblAutoRetrying;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.Label lblPasswordRequired;
    }
}