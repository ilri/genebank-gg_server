namespace GrinGlobal.Updater {
    partial class frmUpdater {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdater));
            this.cboSourceServer = new System.Windows.Forms.ComboBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.dgvServerComponents = new System.Windows.Forms.DataGridView();
            this.btnCheckForServerUpdates = new System.Windows.Forms.Button();
            this.btnDownloadServer = new System.Windows.Forms.Button();
            this.dgvClientComponents = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCheckForClientUpdates = new System.Windows.Forms.Button();
            this.btnDownloadClient = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForNewUpdaterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuToolsDownloadCD = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLocal = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lblServerRequirements = new System.Windows.Forms.Label();
            this.lblClientRequirements = new System.Windows.Forms.Label();
            this.colServerCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colServerDisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colServerInstalledVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colServerLatestVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colServerSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colServerStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colServerAction = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colClientCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colClientDisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClientInstalledVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClientLatestVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClientSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClientStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClientAction = new System.Windows.Forms.DataGridViewLinkColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServerComponents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientComponents)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboSourceServer
            // 
            this.cboSourceServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSourceServer.FormattingEnabled = true;
            this.cboSourceServer.Items.AddRange(new object[] {
            "http://sun.ars-grin.gov/~dbmuke/cgi-bin/gringlobal/1.9.4/gui.asmx"});
            this.cboSourceServer.Location = new System.Drawing.Point(137, 34);
            this.cboSourceServer.Name = "cboSourceServer";
            this.cboSourceServer.Size = new System.Drawing.Size(420, 21);
            this.cboSourceServer.TabIndex = 0;
            this.cboSourceServer.SelectedIndexChanged += new System.EventHandler(this.cboSourceServer_SelectedIndexChanged);
            // 
            // dgvServerComponents
            // 
            this.dgvServerComponents.AllowUserToAddRows = false;
            this.dgvServerComponents.AllowUserToDeleteRows = false;
            this.dgvServerComponents.AllowUserToResizeColumns = false;
            this.dgvServerComponents.AllowUserToResizeRows = false;
            this.dgvServerComponents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvServerComponents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvServerComponents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colServerCheck,
            this.colServerDisplayName,
            this.colServerInstalledVersion,
            this.colServerLatestVersion,
            this.colServerSize,
            this.colServerStatus,
            this.colServerAction});
            this.dgvServerComponents.Location = new System.Drawing.Point(13, 90);
            this.dgvServerComponents.Name = "dgvServerComponents";
            this.dgvServerComponents.RowHeadersVisible = false;
            this.dgvServerComponents.Size = new System.Drawing.Size(648, 139);
            this.dgvServerComponents.TabIndex = 5;
            this.dgvServerComponents.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvServerComponents_CellValueChanged);
            this.dgvServerComponents.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvServerComponents_CellEndEdit);
            this.dgvServerComponents.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvServerComponents_CellContentClick);
            // 
            // btnCheckForServerUpdates
            // 
            this.btnCheckForServerUpdates.Location = new System.Drawing.Point(12, 62);
            this.btnCheckForServerUpdates.Name = "btnCheckForServerUpdates";
            this.btnCheckForServerUpdates.Size = new System.Drawing.Size(167, 23);
            this.btnCheckForServerUpdates.TabIndex = 6;
            this.btnCheckForServerUpdates.Text = "Check For Server Updates";
            this.btnCheckForServerUpdates.UseVisualStyleBackColor = true;
            this.btnCheckForServerUpdates.Click += new System.EventHandler(this.btnCheckForServerUpdates_Click);
            // 
            // btnDownloadServer
            // 
            this.btnDownloadServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownloadServer.Enabled = false;
            this.btnDownloadServer.Location = new System.Drawing.Point(524, 235);
            this.btnDownloadServer.Name = "btnDownloadServer";
            this.btnDownloadServer.Size = new System.Drawing.Size(138, 23);
            this.btnDownloadServer.TabIndex = 7;
            this.btnDownloadServer.Text = "Download / Install";
            this.btnDownloadServer.UseVisualStyleBackColor = true;
            this.btnDownloadServer.Click += new System.EventHandler(this.btnDownloadServer_Click);
            // 
            // dgvClientComponents
            // 
            this.dgvClientComponents.AllowUserToAddRows = false;
            this.dgvClientComponents.AllowUserToDeleteRows = false;
            this.dgvClientComponents.AllowUserToResizeColumns = false;
            this.dgvClientComponents.AllowUserToResizeRows = false;
            this.dgvClientComponents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvClientComponents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClientComponents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colClientCheck,
            this.colClientDisplayName,
            this.colClientInstalledVersion,
            this.colClientLatestVersion,
            this.colClientSize,
            this.colClientStatus,
            this.colClientAction});
            this.dgvClientComponents.Location = new System.Drawing.Point(13, 294);
            this.dgvClientComponents.Name = "dgvClientComponents";
            this.dgvClientComponents.RowHeadersVisible = false;
            this.dgvClientComponents.Size = new System.Drawing.Size(648, 95);
            this.dgvClientComponents.TabIndex = 10;
            this.dgvClientComponents.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClientComponents_CellEndEdit);
            this.dgvClientComponents.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClientComponents_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Download From Server:";
            // 
            // btnCheckForClientUpdates
            // 
            this.btnCheckForClientUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCheckForClientUpdates.Location = new System.Drawing.Point(12, 265);
            this.btnCheckForClientUpdates.Name = "btnCheckForClientUpdates";
            this.btnCheckForClientUpdates.Size = new System.Drawing.Size(167, 23);
            this.btnCheckForClientUpdates.TabIndex = 12;
            this.btnCheckForClientUpdates.Text = "Check For Client Updates";
            this.btnCheckForClientUpdates.UseVisualStyleBackColor = true;
            this.btnCheckForClientUpdates.Click += new System.EventHandler(this.btnCheckForClientUpdates_Click);
            // 
            // btnDownloadClient
            // 
            this.btnDownloadClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownloadClient.Enabled = false;
            this.btnDownloadClient.Location = new System.Drawing.Point(524, 395);
            this.btnDownloadClient.Name = "btnDownloadClient";
            this.btnDownloadClient.Size = new System.Drawing.Size(138, 23);
            this.btnDownloadClient.TabIndex = 13;
            this.btnDownloadClient.Text = "Download / Install";
            this.btnDownloadClient.UseVisualStyleBackColor = true;
            this.btnDownloadClient.Click += new System.EventHandler(this.btnDownloadClient_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 436);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(673, 22);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusText
            // 
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(658, 17);
            this.statusText.Spring = true;
            this.statusText.Text = "-";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolStripMenuItem3,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(673, 24);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForNewUpdaterToolStripMenuItem,
            this.mnuToolsDownloadCD,
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.toolStripMenuItem2,
            this.toolStripSeparator2,
            this.optionsToolStripMenuItem});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(48, 20);
            this.toolStripMenuItem3.Text = "Tools";
            // 
            // checkForNewUpdaterToolStripMenuItem
            // 
            this.checkForNewUpdaterToolStripMenuItem.Name = "checkForNewUpdaterToolStripMenuItem";
            this.checkForNewUpdaterToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.checkForNewUpdaterToolStripMenuItem.Text = "Check For &New Updater";
            this.checkForNewUpdaterToolStripMenuItem.Click += new System.EventHandler(this.checkForNewUpdaterToolStripMenuItem_Click);
            // 
            // mnuToolsDownloadCD
            // 
            this.mnuToolsDownloadCD.Name = "mnuToolsDownloadCD";
            this.mnuToolsDownloadCD.Size = new System.Drawing.Size(236, 22);
            this.mnuToolsDownloadCD.Text = "&Download CD...";
            this.mnuToolsDownloadCD.Click += new System.EventHandler(this.mnuToolsDownloadCD_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(236, 22);
            this.toolStripMenuItem1.Text = "&Delete Cached Files";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(233, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(236, 22);
            this.toolStripMenuItem2.Text = "Database Engine Information...";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(233, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.optionsToolStripMenuItem.Text = "&Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // btnLocal
            // 
            this.btnLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLocal.Location = new System.Drawing.Point(563, 32);
            this.btnLocal.Name = "btnLocal";
            this.btnLocal.Size = new System.Drawing.Size(98, 23);
            this.btnLocal.TabIndex = 17;
            this.btnLocal.Text = "Use Offline...";
            this.btnLocal.UseVisualStyleBackColor = true;
            this.btnLocal.Click += new System.EventHandler(this.btnLocal_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lblServerRequirements
            // 
            this.lblServerRequirements.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblServerRequirements.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblServerRequirements.Location = new System.Drawing.Point(13, 232);
            this.lblServerRequirements.Name = "lblServerRequirements";
            this.lblServerRequirements.Size = new System.Drawing.Size(505, 30);
            this.lblServerRequirements.TabIndex = 18;
            this.lblServerRequirements.Text = "placeholder for lblServerRequirements";
            // 
            // lblClientRequirements
            // 
            this.lblClientRequirements.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClientRequirements.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblClientRequirements.Location = new System.Drawing.Point(13, 395);
            this.lblClientRequirements.Name = "lblClientRequirements";
            this.lblClientRequirements.Size = new System.Drawing.Size(505, 41);
            this.lblClientRequirements.TabIndex = 19;
            this.lblClientRequirements.Text = "placeholder for lblClientRequirements";
            // 
            // colServerCheck
            // 
            this.colServerCheck.FalseValue = "False";
            this.colServerCheck.HeaderText = "";
            this.colServerCheck.Name = "colServerCheck";
            this.colServerCheck.TrueValue = "True";
            this.colServerCheck.Width = 30;
            // 
            // colServerDisplayName
            // 
            this.colServerDisplayName.HeaderText = "Component";
            this.colServerDisplayName.Name = "colServerDisplayName";
            this.colServerDisplayName.ReadOnly = true;
            this.colServerDisplayName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colServerDisplayName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colServerDisplayName.Width = 160;
            // 
            // colServerInstalledVersion
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colServerInstalledVersion.DefaultCellStyle = dataGridViewCellStyle1;
            this.colServerInstalledVersion.HeaderText = "Installed Version";
            this.colServerInstalledVersion.Name = "colServerInstalledVersion";
            this.colServerInstalledVersion.ReadOnly = true;
            this.colServerInstalledVersion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colServerInstalledVersion.Width = 90;
            // 
            // colServerLatestVersion
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colServerLatestVersion.DefaultCellStyle = dataGridViewCellStyle2;
            this.colServerLatestVersion.HeaderText = "Latest Version";
            this.colServerLatestVersion.Name = "colServerLatestVersion";
            this.colServerLatestVersion.ReadOnly = true;
            this.colServerLatestVersion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colServerLatestVersion.Width = 90;
            // 
            // colServerSize
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colServerSize.DefaultCellStyle = dataGridViewCellStyle3;
            this.colServerSize.HeaderText = "Size (MB)";
            this.colServerSize.Name = "colServerSize";
            this.colServerSize.ReadOnly = true;
            this.colServerSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colServerSize.Width = 80;
            // 
            // colServerStatus
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colServerStatus.DefaultCellStyle = dataGridViewCellStyle4;
            this.colServerStatus.HeaderText = "Status";
            this.colServerStatus.Name = "colServerStatus";
            this.colServerStatus.ReadOnly = true;
            this.colServerStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colServerStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colServerStatus.Width = 90;
            // 
            // colServerAction
            // 
            this.colServerAction.HeaderText = "";
            this.colServerAction.Name = "colServerAction";
            this.colServerAction.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colServerAction.Text = "";
            this.colServerAction.TrackVisitedState = false;
            this.colServerAction.Width = 70;
            // 
            // colClientCheck
            // 
            this.colClientCheck.FalseValue = "False";
            this.colClientCheck.HeaderText = "";
            this.colClientCheck.Name = "colClientCheck";
            this.colClientCheck.TrueValue = "True";
            this.colClientCheck.Width = 30;
            // 
            // colClientDisplayName
            // 
            this.colClientDisplayName.HeaderText = "Component";
            this.colClientDisplayName.Name = "colClientDisplayName";
            this.colClientDisplayName.ReadOnly = true;
            this.colClientDisplayName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colClientDisplayName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colClientDisplayName.Width = 160;
            // 
            // colClientInstalledVersion
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colClientInstalledVersion.DefaultCellStyle = dataGridViewCellStyle5;
            this.colClientInstalledVersion.HeaderText = "Installed Version";
            this.colClientInstalledVersion.Name = "colClientInstalledVersion";
            this.colClientInstalledVersion.ReadOnly = true;
            this.colClientInstalledVersion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colClientInstalledVersion.Width = 90;
            // 
            // colClientLatestVersion
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colClientLatestVersion.DefaultCellStyle = dataGridViewCellStyle6;
            this.colClientLatestVersion.HeaderText = "Latest Version";
            this.colClientLatestVersion.Name = "colClientLatestVersion";
            this.colClientLatestVersion.ReadOnly = true;
            this.colClientLatestVersion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colClientLatestVersion.Width = 90;
            // 
            // colClientSize
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colClientSize.DefaultCellStyle = dataGridViewCellStyle7;
            this.colClientSize.HeaderText = "Size (MB)";
            this.colClientSize.Name = "colClientSize";
            this.colClientSize.ReadOnly = true;
            this.colClientSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colClientSize.Width = 80;
            // 
            // colClientStatus
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colClientStatus.DefaultCellStyle = dataGridViewCellStyle8;
            this.colClientStatus.HeaderText = "Status";
            this.colClientStatus.Name = "colClientStatus";
            this.colClientStatus.ReadOnly = true;
            this.colClientStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colClientStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colClientStatus.Width = 90;
            // 
            // colClientAction
            // 
            this.colClientAction.HeaderText = "";
            this.colClientAction.Name = "colClientAction";
            this.colClientAction.Width = 70;
            // 
            // frmUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 458);
            this.Controls.Add(this.lblClientRequirements);
            this.Controls.Add(this.lblServerRequirements);
            this.Controls.Add(this.btnLocal);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnDownloadClient);
            this.Controls.Add(this.btnCheckForClientUpdates);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvClientComponents);
            this.Controls.Add(this.btnDownloadServer);
            this.Controls.Add(this.btnCheckForServerUpdates);
            this.Controls.Add(this.dgvServerComponents);
            this.Controls.Add(this.cboSourceServer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmUpdater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GRIN-Global Updater";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Activated += new System.EventHandler(this.frmUpdater_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.dgvServerComponents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientComponents)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSourceServer;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.DataGridView dgvServerComponents;
        private System.Windows.Forms.Button btnCheckForServerUpdates;
        private System.Windows.Forms.Button btnDownloadServer;
        private System.Windows.Forms.DataGridView dgvClientComponents;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCheckForClientUpdates;
        private System.Windows.Forms.Button btnDownloadClient;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem checkForNewUpdaterToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button btnLocal;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lblServerRequirements;
        private System.Windows.Forms.Label lblClientRequirements;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuToolsDownloadCD;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colServerCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colServerDisplayName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colServerInstalledVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colServerLatestVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colServerSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colServerStatus;
        private System.Windows.Forms.DataGridViewLinkColumn colServerAction;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colClientCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClientDisplayName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClientInstalledVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClientLatestVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClientSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClientStatus;
        private System.Windows.Forms.DataGridViewLinkColumn colClientAction;
    }
}

