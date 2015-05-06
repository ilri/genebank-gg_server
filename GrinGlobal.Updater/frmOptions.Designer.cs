namespace GrinGlobal.Updater {
    partial class frmOptions {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.chkAutoCheckUpdaterVersion = new System.Windows.Forms.CheckBox();
            this.chkUseDefaultProxy = new System.Windows.Forms.CheckBox();
            this.gbCustomProxy = new System.Windows.Forms.GroupBox();
            this.chkProxySuppress417Errors = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProxyPort = new System.Windows.Forms.TextBox();
            this.lblProxyDomain = new System.Windows.Forms.Label();
            this.txtProxyDomain = new System.Windows.Forms.TextBox();
            this.chkProxyBypassOnLocal = new System.Windows.Forms.CheckBox();
            this.chkUseDefaultProxyCredentials = new System.Windows.Forms.CheckBox();
            this.lblProxyPassword = new System.Windows.Forms.Label();
            this.txtProxyPassword = new System.Windows.Forms.TextBox();
            this.lblProxyUsername = new System.Windows.Forms.Label();
            this.txtProxyUsername = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtProxyServerName = new System.Windows.Forms.TextBox();
            this.chkHttpProxy = new System.Windows.Forms.CheckBox();
            this.chkAutoFormatUrl = new System.Windows.Forms.CheckBox();
            this.tpDownloadCache = new System.Windows.Forms.TabPage();
            this.lnkOpenFolder = new System.Windows.Forms.LinkLabel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colChecked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFileName = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colFileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFullPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpSystemInfo = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSystemInfo = new System.Windows.Forms.TextBox();
            this.tpInstallerCD = new System.Windows.Forms.TabPage();
            this.rdoEverything = new System.Windows.Forms.RadioButton();
            this.rdoGGOnly = new System.Windows.Forms.RadioButton();
            this.rdoPrerequisites = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDownloadInstallerCD = new System.Windows.Forms.Button();
            this.tpMirrorServer = new System.Windows.Forms.TabPage();
            this.txtMirrorCD = new System.Windows.Forms.TextBox();
            this.chkMirrorCD = new System.Windows.Forms.CheckBox();
            this.txtMirrorServer = new System.Windows.Forms.TextBox();
            this.chkIsMirrorServer = new System.Windows.Forms.CheckBox();
            this.btnDone = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.gbCustomProxy.SuspendLayout();
            this.tpDownloadCache.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tpSystemInfo.SuspendLayout();
            this.tpInstallerCD.SuspendLayout();
            this.tpMirrorServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpGeneral);
            this.tabControl1.Controls.Add(this.tpDownloadCache);
            this.tabControl1.Controls.Add(this.tpSystemInfo);
            this.tabControl1.Controls.Add(this.tpInstallerCD);
            this.tabControl1.Controls.Add(this.tpMirrorServer);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(582, 342);
            this.tabControl1.TabIndex = 0;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.btnTestConnection);
            this.tpGeneral.Controls.Add(this.chkAutoCheckUpdaterVersion);
            this.tpGeneral.Controls.Add(this.chkUseDefaultProxy);
            this.tpGeneral.Controls.Add(this.gbCustomProxy);
            this.tpGeneral.Controls.Add(this.chkHttpProxy);
            this.tpGeneral.Controls.Add(this.chkAutoFormatUrl);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(574, 316);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(42, 287);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(148, 23);
            this.btnTestConnection.TabIndex = 13;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestProxy_Click);
            // 
            // chkAutoCheckUpdaterVersion
            // 
            this.chkAutoCheckUpdaterVersion.AutoSize = true;
            this.chkAutoCheckUpdaterVersion.Checked = true;
            this.chkAutoCheckUpdaterVersion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoCheckUpdaterVersion.Location = new System.Drawing.Point(24, 41);
            this.chkAutoCheckUpdaterVersion.Name = "chkAutoCheckUpdaterVersion";
            this.chkAutoCheckUpdaterVersion.Size = new System.Drawing.Size(260, 17);
            this.chkAutoCheckUpdaterVersion.TabIndex = 7;
            this.chkAutoCheckUpdaterVersion.Text = "Automaticallly check for a new version of Updater";
            this.chkAutoCheckUpdaterVersion.UseVisualStyleBackColor = true;
            // 
            // chkUseDefaultProxy
            // 
            this.chkUseDefaultProxy.AutoSize = true;
            this.chkUseDefaultProxy.Location = new System.Drawing.Point(42, 87);
            this.chkUseDefaultProxy.Name = "chkUseDefaultProxy";
            this.chkUseDefaultProxy.Size = new System.Drawing.Size(192, 17);
            this.chkUseDefaultProxy.TabIndex = 0;
            this.chkUseDefaultProxy.Text = "Use Internet Explorer proxy settings";
            this.chkUseDefaultProxy.UseVisualStyleBackColor = true;
            this.chkUseDefaultProxy.CheckedChanged += new System.EventHandler(this.chkUseDefaultProxy_CheckedChanged);
            // 
            // gbCustomProxy
            // 
            this.gbCustomProxy.Controls.Add(this.chkProxySuppress417Errors);
            this.gbCustomProxy.Controls.Add(this.label1);
            this.gbCustomProxy.Controls.Add(this.txtProxyPort);
            this.gbCustomProxy.Controls.Add(this.lblProxyDomain);
            this.gbCustomProxy.Controls.Add(this.txtProxyDomain);
            this.gbCustomProxy.Controls.Add(this.chkProxyBypassOnLocal);
            this.gbCustomProxy.Controls.Add(this.chkUseDefaultProxyCredentials);
            this.gbCustomProxy.Controls.Add(this.lblProxyPassword);
            this.gbCustomProxy.Controls.Add(this.txtProxyPassword);
            this.gbCustomProxy.Controls.Add(this.lblProxyUsername);
            this.gbCustomProxy.Controls.Add(this.txtProxyUsername);
            this.gbCustomProxy.Controls.Add(this.label4);
            this.gbCustomProxy.Controls.Add(this.txtProxyServerName);
            this.gbCustomProxy.Location = new System.Drawing.Point(42, 108);
            this.gbCustomProxy.Name = "gbCustomProxy";
            this.gbCustomProxy.Size = new System.Drawing.Size(497, 170);
            this.gbCustomProxy.TabIndex = 6;
            this.gbCustomProxy.TabStop = false;
            this.gbCustomProxy.Text = "Custom Proxy";
            // 
            // chkProxySuppress417Errors
            // 
            this.chkProxySuppress417Errors.AutoSize = true;
            this.chkProxySuppress417Errors.Location = new System.Drawing.Point(257, 39);
            this.chkProxySuppress417Errors.Name = "chkProxySuppress417Errors";
            this.chkProxySuppress417Errors.Size = new System.Drawing.Size(139, 17);
            this.chkProxySuppress417Errors.TabIndex = 12;
            this.chkProxySuppress417Errors.Text = "Ignore HTTP 417 Errors";
            this.chkProxySuppress417Errors.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(364, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Port:";
            // 
            // txtProxyPort
            // 
            this.txtProxyPort.Location = new System.Drawing.Point(399, 13);
            this.txtProxyPort.Name = "txtProxyPort";
            this.txtProxyPort.Size = new System.Drawing.Size(58, 20);
            this.txtProxyPort.TabIndex = 10;
            // 
            // lblProxyDomain
            // 
            this.lblProxyDomain.AutoSize = true;
            this.lblProxyDomain.Location = new System.Drawing.Point(55, 142);
            this.lblProxyDomain.Name = "lblProxyDomain";
            this.lblProxyDomain.Size = new System.Drawing.Size(46, 13);
            this.lblProxyDomain.TabIndex = 9;
            this.lblProxyDomain.Text = "Domain:";
            // 
            // txtProxyDomain
            // 
            this.txtProxyDomain.Location = new System.Drawing.Point(107, 139);
            this.txtProxyDomain.Name = "txtProxyDomain";
            this.txtProxyDomain.Size = new System.Drawing.Size(163, 20);
            this.txtProxyDomain.TabIndex = 8;
            // 
            // chkProxyBypassOnLocal
            // 
            this.chkProxyBypassOnLocal.AutoSize = true;
            this.chkProxyBypassOnLocal.Location = new System.Drawing.Point(107, 39);
            this.chkProxyBypassOnLocal.Name = "chkProxyBypassOnLocal";
            this.chkProxyBypassOnLocal.Size = new System.Drawing.Size(100, 17);
            this.chkProxyBypassOnLocal.TabIndex = 7;
            this.chkProxyBypassOnLocal.Text = "Bypass on local";
            this.chkProxyBypassOnLocal.UseVisualStyleBackColor = true;
            // 
            // chkUseDefaultProxyCredentials
            // 
            this.chkUseDefaultProxyCredentials.AutoSize = true;
            this.chkUseDefaultProxyCredentials.Location = new System.Drawing.Point(107, 62);
            this.chkUseDefaultProxyCredentials.Name = "chkUseDefaultProxyCredentials";
            this.chkUseDefaultProxyCredentials.Size = new System.Drawing.Size(147, 17);
            this.chkUseDefaultProxyCredentials.TabIndex = 6;
            this.chkUseDefaultProxyCredentials.Text = "Use Windows Credentials";
            this.chkUseDefaultProxyCredentials.UseVisualStyleBackColor = true;
            this.chkUseDefaultProxyCredentials.CheckedChanged += new System.EventHandler(this.chkUseDefaultProxyCredentials_CheckedChanged);
            // 
            // lblProxyPassword
            // 
            this.lblProxyPassword.AutoSize = true;
            this.lblProxyPassword.Location = new System.Drawing.Point(45, 116);
            this.lblProxyPassword.Name = "lblProxyPassword";
            this.lblProxyPassword.Size = new System.Drawing.Size(56, 13);
            this.lblProxyPassword.TabIndex = 5;
            this.lblProxyPassword.Text = "Password:";
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.Location = new System.Drawing.Point(107, 113);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.Size = new System.Drawing.Size(163, 20);
            this.txtProxyPassword.TabIndex = 4;
            // 
            // lblProxyUsername
            // 
            this.lblProxyUsername.AutoSize = true;
            this.lblProxyUsername.Location = new System.Drawing.Point(43, 89);
            this.lblProxyUsername.Name = "lblProxyUsername";
            this.lblProxyUsername.Size = new System.Drawing.Size(58, 13);
            this.lblProxyUsername.TabIndex = 3;
            this.lblProxyUsername.Text = "Username:";
            // 
            // txtProxyUsername
            // 
            this.txtProxyUsername.Location = new System.Drawing.Point(107, 86);
            this.txtProxyUsername.Name = "txtProxyUsername";
            this.txtProxyUsername.Size = new System.Drawing.Size(163, 20);
            this.txtProxyUsername.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Server Name:";
            // 
            // txtProxyServerName
            // 
            this.txtProxyServerName.Location = new System.Drawing.Point(107, 13);
            this.txtProxyServerName.Name = "txtProxyServerName";
            this.txtProxyServerName.Size = new System.Drawing.Size(230, 20);
            this.txtProxyServerName.TabIndex = 0;
            // 
            // chkHttpProxy
            // 
            this.chkHttpProxy.AutoSize = true;
            this.chkHttpProxy.Location = new System.Drawing.Point(24, 64);
            this.chkHttpProxy.Name = "chkHttpProxy";
            this.chkHttpProxy.Size = new System.Drawing.Size(106, 17);
            this.chkHttpProxy.TabIndex = 5;
            this.chkHttpProxy.Text = "Use HTTP Proxy";
            this.chkHttpProxy.UseVisualStyleBackColor = true;
            this.chkHttpProxy.CheckedChanged += new System.EventHandler(this.chkHttpProxy_CheckedChanged);
            // 
            // chkAutoFormatUrl
            // 
            this.chkAutoFormatUrl.AutoSize = true;
            this.chkAutoFormatUrl.Checked = true;
            this.chkAutoFormatUrl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoFormatUrl.Location = new System.Drawing.Point(24, 18);
            this.chkAutoFormatUrl.Name = "chkAutoFormatUrl";
            this.chkAutoFormatUrl.Size = new System.Drawing.Size(200, 17);
            this.chkAutoFormatUrl.TabIndex = 0;
            this.chkAutoFormatUrl.Text = "Automaticallly format URL as needed";
            this.chkAutoFormatUrl.UseVisualStyleBackColor = true;
            // 
            // tpDownloadCache
            // 
            this.tpDownloadCache.Controls.Add(this.lnkOpenFolder);
            this.tpDownloadCache.Controls.Add(this.btnDelete);
            this.tpDownloadCache.Controls.Add(this.dataGridView1);
            this.tpDownloadCache.Location = new System.Drawing.Point(4, 22);
            this.tpDownloadCache.Name = "tpDownloadCache";
            this.tpDownloadCache.Padding = new System.Windows.Forms.Padding(3);
            this.tpDownloadCache.Size = new System.Drawing.Size(574, 316);
            this.tpDownloadCache.TabIndex = 1;
            this.tpDownloadCache.Text = "Download Cache";
            this.tpDownloadCache.UseVisualStyleBackColor = true;
            // 
            // lnkOpenFolder
            // 
            this.lnkOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkOpenFolder.Location = new System.Drawing.Point(131, 260);
            this.lnkOpenFolder.Name = "lnkOpenFolder";
            this.lnkOpenFolder.Size = new System.Drawing.Size(437, 13);
            this.lnkOpenFolder.TabIndex = 3;
            this.lnkOpenFolder.TabStop = true;
            this.lnkOpenFolder.Text = "Open Download Cache Folder In Windows Explorer";
            this.lnkOpenFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOpenFolder_LinkClicked);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(6, 255);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(119, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "&Delete Selected";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChecked,
            this.colFileName,
            this.colFileSize,
            this.colFullPath});
            this.dataGridView1.Location = new System.Drawing.Point(6, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(562, 243);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // colChecked
            // 
            this.colChecked.FalseValue = "False";
            this.colChecked.HeaderText = "";
            this.colChecked.Name = "colChecked";
            this.colChecked.TrueValue = "True";
            this.colChecked.Width = 40;
            // 
            // colFileName
            // 
            this.colFileName.HeaderText = "File Name";
            this.colFileName.Name = "colFileName";
            this.colFileName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFileName.Width = 250;
            // 
            // colFileSize
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colFileSize.DefaultCellStyle = dataGridViewCellStyle2;
            this.colFileSize.HeaderText = "Size (MB)";
            this.colFileSize.Name = "colFileSize";
            this.colFileSize.Width = 80;
            // 
            // colFullPath
            // 
            this.colFullPath.HeaderText = "Full Path";
            this.colFullPath.Name = "colFullPath";
            this.colFullPath.Visible = false;
            // 
            // tpSystemInfo
            // 
            this.tpSystemInfo.Controls.Add(this.label2);
            this.tpSystemInfo.Controls.Add(this.txtSystemInfo);
            this.tpSystemInfo.Location = new System.Drawing.Point(4, 22);
            this.tpSystemInfo.Name = "tpSystemInfo";
            this.tpSystemInfo.Size = new System.Drawing.Size(574, 316);
            this.tpSystemInfo.TabIndex = 3;
            this.tpSystemInfo.Text = "System Info";
            this.tpSystemInfo.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(299, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "GRIN-Global Updater detects the following about your system:";
            // 
            // txtSystemInfo
            // 
            this.txtSystemInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSystemInfo.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemInfo.Location = new System.Drawing.Point(4, 33);
            this.txtSystemInfo.Multiline = true;
            this.txtSystemInfo.Name = "txtSystemInfo";
            this.txtSystemInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSystemInfo.Size = new System.Drawing.Size(567, 248);
            this.txtSystemInfo.TabIndex = 1;
            // 
            // tpInstallerCD
            // 
            this.tpInstallerCD.Controls.Add(this.rdoEverything);
            this.tpInstallerCD.Controls.Add(this.rdoGGOnly);
            this.tpInstallerCD.Controls.Add(this.rdoPrerequisites);
            this.tpInstallerCD.Controls.Add(this.label3);
            this.tpInstallerCD.Controls.Add(this.btnDownloadInstallerCD);
            this.tpInstallerCD.Location = new System.Drawing.Point(4, 22);
            this.tpInstallerCD.Name = "tpInstallerCD";
            this.tpInstallerCD.Size = new System.Drawing.Size(574, 316);
            this.tpInstallerCD.TabIndex = 4;
            this.tpInstallerCD.Text = "Installer CD";
            this.tpInstallerCD.UseVisualStyleBackColor = true;
            // 
            // rdoEverything
            // 
            this.rdoEverything.AutoSize = true;
            this.rdoEverything.Location = new System.Drawing.Point(13, 196);
            this.rdoEverything.Name = "rdoEverything";
            this.rdoEverything.Size = new System.Drawing.Size(363, 17);
            this.rdoEverything.TabIndex = 7;
            this.rdoEverything.Text = "Download Everything Required By GRIN-Global (approximately 930 MB)";
            this.rdoEverything.UseVisualStyleBackColor = true;
            // 
            // rdoGGOnly
            // 
            this.rdoGGOnly.AutoSize = true;
            this.rdoGGOnly.Checked = true;
            this.rdoGGOnly.Location = new System.Drawing.Point(13, 173);
            this.rdoGGOnly.Name = "rdoGGOnly";
            this.rdoGGOnly.Size = new System.Drawing.Size(336, 17);
            this.rdoGGOnly.TabIndex = 6;
            this.rdoGGOnly.TabStop = true;
            this.rdoGGOnly.Text = "Download GRIN-Global Software Only CD (approximately 230 MB)";
            this.rdoGGOnly.UseVisualStyleBackColor = true;
            // 
            // rdoPrerequisites
            // 
            this.rdoPrerequisites.AutoSize = true;
            this.rdoPrerequisites.Location = new System.Drawing.Point(13, 150);
            this.rdoPrerequisites.Name = "rdoPrerequisites";
            this.rdoPrerequisites.Size = new System.Drawing.Size(331, 17);
            this.rdoPrerequisites.TabIndex = 5;
            this.rdoPrerequisites.Text = "Download Prerequisite Software Only CD (approximately 700 MB)";
            this.rdoPrerequisites.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(557, 137);
            this.label3.TabIndex = 4;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // btnDownloadInstallerCD
            // 
            this.btnDownloadInstallerCD.Location = new System.Drawing.Point(13, 236);
            this.btnDownloadInstallerCD.Name = "btnDownloadInstallerCD";
            this.btnDownloadInstallerCD.Size = new System.Drawing.Size(133, 23);
            this.btnDownloadInstallerCD.TabIndex = 3;
            this.btnDownloadInstallerCD.Text = "Download &CD";
            this.btnDownloadInstallerCD.UseVisualStyleBackColor = true;
            this.btnDownloadInstallerCD.Click += new System.EventHandler(this.btnDownloadInstallerCD_Click);
            // 
            // tpMirrorServer
            // 
            this.tpMirrorServer.Controls.Add(this.txtMirrorCD);
            this.tpMirrorServer.Controls.Add(this.chkMirrorCD);
            this.tpMirrorServer.Controls.Add(this.txtMirrorServer);
            this.tpMirrorServer.Controls.Add(this.chkIsMirrorServer);
            this.tpMirrorServer.Location = new System.Drawing.Point(4, 22);
            this.tpMirrorServer.Name = "tpMirrorServer";
            this.tpMirrorServer.Padding = new System.Windows.Forms.Padding(3);
            this.tpMirrorServer.Size = new System.Drawing.Size(574, 316);
            this.tpMirrorServer.TabIndex = 5;
            this.tpMirrorServer.Text = "Mirror Server";
            this.tpMirrorServer.UseVisualStyleBackColor = true;
            // 
            // txtMirrorCD
            // 
            this.txtMirrorCD.Location = new System.Drawing.Point(45, 216);
            this.txtMirrorCD.Multiline = true;
            this.txtMirrorCD.Name = "txtMirrorCD";
            this.txtMirrorCD.ReadOnly = true;
            this.txtMirrorCD.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMirrorCD.Size = new System.Drawing.Size(506, 94);
            this.txtMirrorCD.TabIndex = 8;
            this.txtMirrorCD.Text = resources.GetString("txtMirrorCD.Text");
            // 
            // chkMirrorCD
            // 
            this.chkMirrorCD.AutoSize = true;
            this.chkMirrorCD.Location = new System.Drawing.Point(45, 193);
            this.chkMirrorCD.Name = "chkMirrorCD";
            this.chkMirrorCD.Size = new System.Drawing.Size(133, 17);
            this.chkMirrorCD.TabIndex = 7;
            this.chkMirrorCD.Text = "Mirror GRIN-Global CD";
            this.chkMirrorCD.UseVisualStyleBackColor = true;
            this.chkMirrorCD.CheckedChanged += new System.EventHandler(this.chkMirrorCD_CheckedChanged);
            // 
            // txtMirrorServer
            // 
            this.txtMirrorServer.Location = new System.Drawing.Point(17, 46);
            this.txtMirrorServer.Multiline = true;
            this.txtMirrorServer.Name = "txtMirrorServer";
            this.txtMirrorServer.ReadOnly = true;
            this.txtMirrorServer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMirrorServer.Size = new System.Drawing.Size(534, 141);
            this.txtMirrorServer.TabIndex = 6;
            this.txtMirrorServer.Text = resources.GetString("txtMirrorServer.Text");
            // 
            // chkIsMirrorServer
            // 
            this.chkIsMirrorServer.AutoSize = true;
            this.chkIsMirrorServer.Location = new System.Drawing.Point(17, 23);
            this.chkIsMirrorServer.Name = "chkIsMirrorServer";
            this.chkIsMirrorServer.Size = new System.Drawing.Size(130, 17);
            this.chkIsMirrorServer.TabIndex = 5;
            this.chkIsMirrorServer.Text = "This Is A Mirror Server";
            this.chkIsMirrorServer.UseVisualStyleBackColor = true;
            this.chkIsMirrorServer.CheckedChanged += new System.EventHandler(this.chkIsMirrorServer_CheckedChanged);
            // 
            // btnDone
            // 
            this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDone.Location = new System.Drawing.Point(520, 361);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 1;
            this.btnDone.Text = "&Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // frmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 396);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmOptions_KeyUp);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmOptions_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            this.gbCustomProxy.ResumeLayout(false);
            this.gbCustomProxy.PerformLayout();
            this.tpDownloadCache.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tpSystemInfo.ResumeLayout(false);
            this.tpSystemInfo.PerformLayout();
            this.tpInstallerCD.ResumeLayout(false);
            this.tpInstallerCD.PerformLayout();
            this.tpMirrorServer.ResumeLayout(false);
            this.tpMirrorServer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpDownloadCache;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox chkAutoFormatUrl;
        private System.Windows.Forms.TabPage tpSystemInfo;
        private System.Windows.Forms.TextBox txtSystemInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lnkOpenFolder;
        private System.Windows.Forms.TabPage tpInstallerCD;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDownloadInstallerCD;
        private System.Windows.Forms.CheckBox chkUseDefaultProxy;
        private System.Windows.Forms.GroupBox gbCustomProxy;
        private System.Windows.Forms.CheckBox chkHttpProxy;
        private System.Windows.Forms.CheckBox chkUseDefaultProxyCredentials;
        private System.Windows.Forms.Label lblProxyPassword;
        private System.Windows.Forms.TextBox txtProxyPassword;
        private System.Windows.Forms.Label lblProxyUsername;
        private System.Windows.Forms.TextBox txtProxyUsername;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtProxyServerName;
        private System.Windows.Forms.CheckBox chkProxyBypassOnLocal;
        private System.Windows.Forms.Label lblProxyDomain;
        private System.Windows.Forms.TextBox txtProxyDomain;
        private System.Windows.Forms.RadioButton rdoEverything;
        private System.Windows.Forms.RadioButton rdoGGOnly;
        private System.Windows.Forms.RadioButton rdoPrerequisites;
        private System.Windows.Forms.CheckBox chkAutoCheckUpdaterVersion;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChecked;
        private System.Windows.Forms.DataGridViewLinkColumn colFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFullPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProxyPort;
        private System.Windows.Forms.CheckBox chkProxySuppress417Errors;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.TabPage tpMirrorServer;
        private System.Windows.Forms.TextBox txtMirrorServer;
        private System.Windows.Forms.CheckBox chkIsMirrorServer;
        private System.Windows.Forms.CheckBox chkMirrorCD;
        private System.Windows.Forms.TextBox txtMirrorCD;
    }
}