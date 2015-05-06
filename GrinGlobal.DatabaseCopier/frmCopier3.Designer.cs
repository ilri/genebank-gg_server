namespace GrinGlobal.DatabaseCopier {
	partial class frmCopier3 {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCopier3));
            this.chkSelectAllTables = new System.Windows.Forms.CheckBox();
            this.btnListTables = new System.Windows.Forms.Button();
            this.lvTableList = new System.Windows.Forms.ListView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gbFromFile = new System.Windows.Forms.GroupBox();
            this.txtFromFileName = new System.Windows.Forms.TextBox();
            this.btnFromFilenamePrompt = new System.Windows.Forms.Button();
            this.gbFromDatabase = new System.Windows.Forms.GroupBox();
            this.rbFromDatabasePostgreSQL = new System.Windows.Forms.RadioButton();
            this.txtFromDatabaseName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cboFromConnectionString = new System.Windows.Forms.ComboBox();
            this.rbFromDatabaseOracle = new System.Windows.Forms.RadioButton();
            this.rbFromDatabaseSQLServer = new System.Windows.Forms.RadioButton();
            this.rbFromDatabaseMySQL = new System.Windows.Forms.RadioButton();
            this.label13 = new System.Windows.Forms.Label();
            this.rbFromIsXml = new System.Windows.Forms.RadioButton();
            this.rbFromIsCSV = new System.Windows.Forms.RadioButton();
            this.rbFromIsDatabase = new System.Windows.Forms.RadioButton();
            this.status = new System.Windows.Forms.StatusStrip();
            this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbTo = new System.Windows.Forms.GroupBox();
            this.gbToFile = new System.Windows.Forms.GroupBox();
            this.txtToFolderName = new System.Windows.Forms.TextBox();
            this.btnToFolderNamePrompt = new System.Windows.Forms.Button();
            this.gbToDatabase = new System.Windows.Forms.GroupBox();
            this.rbToDatabasePostgreSQL = new System.Windows.Forms.RadioButton();
            this.txtToDatabaseName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboToConnectionString = new System.Windows.Forms.ComboBox();
            this.rbToDatabaseOracle = new System.Windows.Forms.RadioButton();
            this.rbToDatabaseSQLServer = new System.Windows.Forms.RadioButton();
            this.rbToDatabaseMySQL = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rbToIsXml = new System.Windows.Forms.RadioButton();
            this.rbToIsDatabase = new System.Windows.Forms.RadioButton();
            this.gbActionWriteSchema = new System.Windows.Forms.GroupBox();
            this.chkWriteSchemaCreateSelfReferentialConstraints = new System.Windows.Forms.CheckBox();
            this.chkWriteSchemaCreateForeignConstraints = new System.Windows.Forms.CheckBox();
            this.chkWriteSchemaCreateIndexes = new System.Windows.Forms.CheckBox();
            this.chkWriteSchemaCreateTables = new System.Windows.Forms.CheckBox();
            this.btnActionWriteSchemaGo = new System.Windows.Forms.Button();
            this.rbActionWriteSchema = new System.Windows.Forms.RadioButton();
            this.rbActionWriteData = new System.Windows.Forms.RadioButton();
            this.rbActionScript = new System.Windows.Forms.RadioButton();
            this.gbActionWriteData = new System.Windows.Forms.GroupBox();
            this.chkActionWriteDataDryRun = new System.Windows.Forms.CheckBox();
            this.rbActionWriteDatabaseData = new System.Windows.Forms.RadioButton();
            this.btnActionWriteDataGo = new System.Windows.Forms.Button();
            this.gbActionScript = new System.Windows.Forms.GroupBox();
            this.btnActionScriptGo = new System.Windows.Forms.Button();
            this.chkScriptCreateConstraints = new System.Windows.Forms.CheckBox();
            this.chkScriptCreateIndexes = new System.Windows.Forms.CheckBox();
            this.chkScriptCreateTables = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnPopupTableList = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnDestinationTestConnection = new System.Windows.Forms.Button();
            this.btnSourceTestConnection = new System.Windows.Forms.Button();
            this.chkScriptLoadData = new System.Windows.Forms.CheckBox();
            this.chkActionWriteOnlyRequiredData = new System.Windows.Forms.CheckBox();
            this.chkScriptOnlyRequiredData = new System.Windows.Forms.CheckBox();
            this.groupBox3.SuspendLayout();
            this.gbFromFile.SuspendLayout();
            this.gbFromDatabase.SuspendLayout();
            this.status.SuspendLayout();
            this.gbTo.SuspendLayout();
            this.gbToFile.SuspendLayout();
            this.gbToDatabase.SuspendLayout();
            this.gbActionWriteSchema.SuspendLayout();
            this.gbActionWriteData.SuspendLayout();
            this.gbActionScript.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkSelectAllTables
            // 
            this.chkSelectAllTables.AutoSize = true;
            this.chkSelectAllTables.Location = new System.Drawing.Point(7, 17);
            this.chkSelectAllTables.Name = "chkSelectAllTables";
            this.chkSelectAllTables.Size = new System.Drawing.Size(70, 17);
            this.chkSelectAllTables.TabIndex = 34;
            this.chkSelectAllTables.Text = "Select All";
            this.chkSelectAllTables.UseVisualStyleBackColor = true;
            this.chkSelectAllTables.Click += new System.EventHandler(this.chkSelectAllTables_Click);
            // 
            // btnListTables
            // 
            this.btnListTables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnListTables.Location = new System.Drawing.Point(527, 13);
            this.btnListTables.Name = "btnListTables";
            this.btnListTables.Size = new System.Drawing.Size(92, 23);
            this.btnListTables.TabIndex = 33;
            this.btnListTables.Text = "List Tables";
            this.btnListTables.UseVisualStyleBackColor = true;
            this.btnListTables.Click += new System.EventHandler(this.btnListTables_Click);
            // 
            // lvTableList
            // 
            this.lvTableList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvTableList.CheckBoxes = true;
            this.lvTableList.Location = new System.Drawing.Point(6, 42);
            this.lvTableList.Name = "lvTableList";
            this.lvTableList.Size = new System.Drawing.Size(613, 437);
            this.lvTableList.TabIndex = 32;
            this.lvTableList.UseCompatibleStateImageBehavior = false;
            this.lvTableList.View = System.Windows.Forms.View.List;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gbFromFile);
            this.groupBox3.Controls.Add(this.gbFromDatabase);
            this.groupBox3.Controls.Add(this.rbFromIsXml);
            this.groupBox3.Controls.Add(this.rbFromIsCSV);
            this.groupBox3.Controls.Add(this.rbFromIsDatabase);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(616, 476);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Load Schema / Data From:";
            // 
            // gbFromFile
            // 
            this.gbFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFromFile.Controls.Add(this.txtFromFileName);
            this.gbFromFile.Controls.Add(this.btnFromFilenamePrompt);
            this.gbFromFile.Location = new System.Drawing.Point(18, 209);
            this.gbFromFile.Name = "gbFromFile";
            this.gbFromFile.Size = new System.Drawing.Size(592, 68);
            this.gbFromFile.TabIndex = 24;
            this.gbFromFile.TabStop = false;
            this.gbFromFile.Text = "File:";
            // 
            // txtFromFileName
            // 
            this.txtFromFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFromFileName.Location = new System.Drawing.Point(6, 19);
            this.txtFromFileName.Name = "txtFromFileName";
            this.txtFromFileName.Size = new System.Drawing.Size(546, 20);
            this.txtFromFileName.TabIndex = 19;
            this.txtFromFileName.Text = "D:\\test_db_creator";
            this.txtFromFileName.TextChanged += new System.EventHandler(this.txtFromFileName_TextChanged);
            // 
            // btnFromFilenamePrompt
            // 
            this.btnFromFilenamePrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFromFilenamePrompt.Location = new System.Drawing.Point(558, 16);
            this.btnFromFilenamePrompt.Name = "btnFromFilenamePrompt";
            this.btnFromFilenamePrompt.Size = new System.Drawing.Size(28, 23);
            this.btnFromFilenamePrompt.TabIndex = 20;
            this.btnFromFilenamePrompt.Text = "...";
            this.btnFromFilenamePrompt.UseVisualStyleBackColor = true;
            this.btnFromFilenamePrompt.Click += new System.EventHandler(this.btnFromFilenamePrompt_Click);
            // 
            // gbFromDatabase
            // 
            this.gbFromDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFromDatabase.Controls.Add(this.btnSourceTestConnection);
            this.gbFromDatabase.Controls.Add(this.rbFromDatabasePostgreSQL);
            this.gbFromDatabase.Controls.Add(this.txtFromDatabaseName);
            this.gbFromDatabase.Controls.Add(this.label14);
            this.gbFromDatabase.Controls.Add(this.cboFromConnectionString);
            this.gbFromDatabase.Controls.Add(this.rbFromDatabaseOracle);
            this.gbFromDatabase.Controls.Add(this.rbFromDatabaseSQLServer);
            this.gbFromDatabase.Controls.Add(this.rbFromDatabaseMySQL);
            this.gbFromDatabase.Controls.Add(this.label13);
            this.gbFromDatabase.Location = new System.Drawing.Point(18, 43);
            this.gbFromDatabase.Name = "gbFromDatabase";
            this.gbFromDatabase.Size = new System.Drawing.Size(592, 98);
            this.gbFromDatabase.TabIndex = 19;
            this.gbFromDatabase.TabStop = false;
            this.gbFromDatabase.Text = "Database";
            // 
            // rbFromDatabasePostgreSQL
            // 
            this.rbFromDatabasePostgreSQL.AutoSize = true;
            this.rbFromDatabasePostgreSQL.Location = new System.Drawing.Point(90, 17);
            this.rbFromDatabasePostgreSQL.Name = "rbFromDatabasePostgreSQL";
            this.rbFromDatabasePostgreSQL.Size = new System.Drawing.Size(82, 17);
            this.rbFromDatabasePostgreSQL.TabIndex = 21;
            this.rbFromDatabasePostgreSQL.Text = "PostgreSQL";
            this.rbFromDatabasePostgreSQL.UseVisualStyleBackColor = true;
            this.rbFromDatabasePostgreSQL.CheckedChanged += new System.EventHandler(this.rbFromDatabasePostgreSQL_CheckedChanged);
            // 
            // txtFromDatabaseName
            // 
            this.txtFromDatabaseName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFromDatabaseName.Location = new System.Drawing.Point(408, 16);
            this.txtFromDatabaseName.Name = "txtFromDatabaseName";
            this.txtFromDatabaseName.Size = new System.Drawing.Size(80, 20);
            this.txtFromDatabaseName.TabIndex = 35;
            this.txtFromDatabaseName.Text = "GRINGLOBAL";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(348, 19);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(54, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "DB name:";
            // 
            // cboFromConnectionString
            // 
            this.cboFromConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFromConnectionString.FormattingEnabled = true;
            this.cboFromConnectionString.Items.AddRange(new object[] {
            "---- SQL Server ----",
            "Data Source=localhost\\sqlexpress;Database=gringlobal;user id=sa;password=",
            "---- PostgreSQL ----",
            "Server=localhost;Port=5432;Database=gringlobal;user id=postgres;password=",
            "---- Oracle ----",
            "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(PORT=1521)(HOST=127.0.0.1))(CONN" +
                "ECT_DATA=(SERVICE_NAME=XE)));User Id=SYS;password=",
            "---- MySQL ----",
            "Data Source=localhost;Database=gringlobal;user id=root;password="});
            this.cboFromConnectionString.Location = new System.Drawing.Point(6, 42);
            this.cboFromConnectionString.Name = "cboFromConnectionString";
            this.cboFromConnectionString.Size = new System.Drawing.Size(580, 21);
            this.cboFromConnectionString.TabIndex = 23;
            // 
            // rbFromDatabaseOracle
            // 
            this.rbFromDatabaseOracle.AutoSize = true;
            this.rbFromDatabaseOracle.Location = new System.Drawing.Point(178, 17);
            this.rbFromDatabaseOracle.Name = "rbFromDatabaseOracle";
            this.rbFromDatabaseOracle.Size = new System.Drawing.Size(56, 17);
            this.rbFromDatabaseOracle.TabIndex = 22;
            this.rbFromDatabaseOracle.Text = "Oracle";
            this.rbFromDatabaseOracle.UseVisualStyleBackColor = true;
            this.rbFromDatabaseOracle.CheckedChanged += new System.EventHandler(this.rbFromDatabaseOracle_CheckedChanged);
            // 
            // rbFromDatabaseSQLServer
            // 
            this.rbFromDatabaseSQLServer.AutoSize = true;
            this.rbFromDatabaseSQLServer.Checked = true;
            this.rbFromDatabaseSQLServer.Location = new System.Drawing.Point(13, 17);
            this.rbFromDatabaseSQLServer.Name = "rbFromDatabaseSQLServer";
            this.rbFromDatabaseSQLServer.Size = new System.Drawing.Size(74, 17);
            this.rbFromDatabaseSQLServer.TabIndex = 20;
            this.rbFromDatabaseSQLServer.TabStop = true;
            this.rbFromDatabaseSQLServer.Text = "Sql Server";
            this.rbFromDatabaseSQLServer.UseVisualStyleBackColor = true;
            this.rbFromDatabaseSQLServer.CheckedChanged += new System.EventHandler(this.rbFromDatabaseSQLServer_CheckedChanged);
            // 
            // rbFromDatabaseMySQL
            // 
            this.rbFromDatabaseMySQL.AutoSize = true;
            this.rbFromDatabaseMySQL.Location = new System.Drawing.Point(235, 17);
            this.rbFromDatabaseMySQL.Name = "rbFromDatabaseMySQL";
            this.rbFromDatabaseMySQL.Size = new System.Drawing.Size(54, 17);
            this.rbFromDatabaseMySQL.TabIndex = 23;
            this.rbFromDatabaseMySQL.Text = "MySql";
            this.rbFromDatabaseMySQL.UseVisualStyleBackColor = true;
            this.rbFromDatabaseMySQL.CheckedChanged += new System.EventHandler(this.rbFromDatabaseMySQL_CheckedChanged);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(495, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(91, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "Connection String";
            // 
            // rbFromIsXml
            // 
            this.rbFromIsXml.AutoSize = true;
            this.rbFromIsXml.Location = new System.Drawing.Point(7, 185);
            this.rbFromIsXml.Name = "rbFromIsXml";
            this.rbFromIsXml.Size = new System.Drawing.Size(66, 17);
            this.rbFromIsXml.TabIndex = 23;
            this.rbFromIsXml.TabStop = true;
            this.rbFromIsXml.Text = "XML File";
            this.rbFromIsXml.UseVisualStyleBackColor = true;
            this.rbFromIsXml.CheckedChanged += new System.EventHandler(this.rbFromIsXml_CheckedChanged);
            // 
            // rbFromIsCSV
            // 
            this.rbFromIsCSV.AutoSize = true;
            this.rbFromIsCSV.Location = new System.Drawing.Point(7, 162);
            this.rbFromIsCSV.Name = "rbFromIsCSV";
            this.rbFromIsCSV.Size = new System.Drawing.Size(65, 17);
            this.rbFromIsCSV.TabIndex = 22;
            this.rbFromIsCSV.Text = "CSV File";
            this.rbFromIsCSV.UseVisualStyleBackColor = true;
            this.rbFromIsCSV.CheckedChanged += new System.EventHandler(this.rbFromIsCSV_CheckedChanged);
            // 
            // rbFromIsDatabase
            // 
            this.rbFromIsDatabase.AutoSize = true;
            this.rbFromIsDatabase.Checked = true;
            this.rbFromIsDatabase.Location = new System.Drawing.Point(7, 20);
            this.rbFromIsDatabase.Name = "rbFromIsDatabase";
            this.rbFromIsDatabase.Size = new System.Drawing.Size(71, 17);
            this.rbFromIsDatabase.TabIndex = 21;
            this.rbFromIsDatabase.TabStop = true;
            this.rbFromIsDatabase.Text = "Database";
            this.rbFromIsDatabase.UseVisualStyleBackColor = true;
            this.rbFromIsDatabase.CheckedChanged += new System.EventHandler(this.rbFromIsDatabase_CheckedChanged);
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusProgress,
            this.statusText});
            this.status.Location = new System.Drawing.Point(0, 508);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(630, 22);
            this.status.TabIndex = 36;
            this.status.Text = "statusStrip1";
            // 
            // statusProgress
            // 
            this.statusProgress.Name = "statusProgress";
            this.statusProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // statusText
            // 
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(12, 17);
            this.statusText.Text = "-";
            // 
            // gbTo
            // 
            this.gbTo.Controls.Add(this.gbToFile);
            this.gbTo.Controls.Add(this.gbToDatabase);
            this.gbTo.Controls.Add(this.rbToIsXml);
            this.gbTo.Controls.Add(this.rbToIsDatabase);
            this.gbTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTo.Location = new System.Drawing.Point(3, 3);
            this.gbTo.Name = "gbTo";
            this.gbTo.Size = new System.Drawing.Size(616, 476);
            this.gbTo.TabIndex = 37;
            this.gbTo.TabStop = false;
            this.gbTo.Text = "Write Schema / Data To:";
            // 
            // gbToFile
            // 
            this.gbToFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbToFile.Controls.Add(this.txtToFolderName);
            this.gbToFile.Controls.Add(this.btnToFolderNamePrompt);
            this.gbToFile.Location = new System.Drawing.Point(23, 177);
            this.gbToFile.Name = "gbToFile";
            this.gbToFile.Size = new System.Drawing.Size(587, 68);
            this.gbToFile.TabIndex = 24;
            this.gbToFile.TabStop = false;
            this.gbToFile.Text = "Folder:";
            // 
            // txtToFolderName
            // 
            this.txtToFolderName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtToFolderName.Location = new System.Drawing.Point(6, 19);
            this.txtToFolderName.Name = "txtToFolderName";
            this.txtToFolderName.Size = new System.Drawing.Size(541, 20);
            this.txtToFolderName.TabIndex = 19;
            this.txtToFolderName.Text = "D:\\test_db_creator\\";
            // 
            // btnToFolderNamePrompt
            // 
            this.btnToFolderNamePrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToFolderNamePrompt.Location = new System.Drawing.Point(553, 16);
            this.btnToFolderNamePrompt.Name = "btnToFolderNamePrompt";
            this.btnToFolderNamePrompt.Size = new System.Drawing.Size(28, 23);
            this.btnToFolderNamePrompt.TabIndex = 20;
            this.btnToFolderNamePrompt.Text = "...";
            this.btnToFolderNamePrompt.UseVisualStyleBackColor = true;
            this.btnToFolderNamePrompt.Click += new System.EventHandler(this.btnToFolderNamePrompt_Click);
            // 
            // gbToDatabase
            // 
            this.gbToDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbToDatabase.Controls.Add(this.btnDestinationTestConnection);
            this.gbToDatabase.Controls.Add(this.rbToDatabasePostgreSQL);
            this.gbToDatabase.Controls.Add(this.txtToDatabaseName);
            this.gbToDatabase.Controls.Add(this.label1);
            this.gbToDatabase.Controls.Add(this.cboToConnectionString);
            this.gbToDatabase.Controls.Add(this.rbToDatabaseOracle);
            this.gbToDatabase.Controls.Add(this.rbToDatabaseSQLServer);
            this.gbToDatabase.Controls.Add(this.rbToDatabaseMySQL);
            this.gbToDatabase.Controls.Add(this.label2);
            this.gbToDatabase.Location = new System.Drawing.Point(23, 43);
            this.gbToDatabase.Name = "gbToDatabase";
            this.gbToDatabase.Size = new System.Drawing.Size(587, 91);
            this.gbToDatabase.TabIndex = 19;
            this.gbToDatabase.TabStop = false;
            this.gbToDatabase.Text = "Database";
            // 
            // rbToDatabasePostgreSQL
            // 
            this.rbToDatabasePostgreSQL.AutoSize = true;
            this.rbToDatabasePostgreSQL.Location = new System.Drawing.Point(89, 17);
            this.rbToDatabasePostgreSQL.Name = "rbToDatabasePostgreSQL";
            this.rbToDatabasePostgreSQL.Size = new System.Drawing.Size(82, 17);
            this.rbToDatabasePostgreSQL.TabIndex = 21;
            this.rbToDatabasePostgreSQL.Text = "PostgreSQL";
            this.rbToDatabasePostgreSQL.UseVisualStyleBackColor = true;
            this.rbToDatabasePostgreSQL.CheckedChanged += new System.EventHandler(this.rbToDatabasePostgreSQL_CheckedChanged);
            // 
            // txtToDatabaseName
            // 
            this.txtToDatabaseName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtToDatabaseName.Location = new System.Drawing.Point(401, 16);
            this.txtToDatabaseName.Name = "txtToDatabaseName";
            this.txtToDatabaseName.Size = new System.Drawing.Size(81, 20);
            this.txtToDatabaseName.TabIndex = 35;
            this.txtToDatabaseName.Text = "GRINGLOBAL";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(342, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "DB name:";
            // 
            // cboToConnectionString
            // 
            this.cboToConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboToConnectionString.FormattingEnabled = true;
            this.cboToConnectionString.Items.AddRange(new object[] {
            "---- SQL Server ----",
            "Data Source=localhost\\sqlexpress;Database=gringlobal;user id=sa;password=",
            "---- PostgreSQL ----",
            "Server=localhost;Port=5432;Database=gringlobal;user id=postgres;password=",
            "---- Oracle ----",
            "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(PORT=1521)(HOST=127.0.0.1))(CONN" +
                "ECT_DATA=(SERVICE_NAME=XE)));User Id=SYS;password=",
            "---- MySQL ----",
            "Data Source=localhost;Database=gringlobal;user id=root;password="});
            this.cboToConnectionString.Location = new System.Drawing.Point(6, 42);
            this.cboToConnectionString.Name = "cboToConnectionString";
            this.cboToConnectionString.Size = new System.Drawing.Size(575, 21);
            this.cboToConnectionString.TabIndex = 23;
            // 
            // rbToDatabaseOracle
            // 
            this.rbToDatabaseOracle.AutoSize = true;
            this.rbToDatabaseOracle.Location = new System.Drawing.Point(177, 17);
            this.rbToDatabaseOracle.Name = "rbToDatabaseOracle";
            this.rbToDatabaseOracle.Size = new System.Drawing.Size(56, 17);
            this.rbToDatabaseOracle.TabIndex = 22;
            this.rbToDatabaseOracle.Text = "Oracle";
            this.rbToDatabaseOracle.UseVisualStyleBackColor = true;
            this.rbToDatabaseOracle.CheckedChanged += new System.EventHandler(this.rbToDatabaseOracle_CheckedChanged);
            // 
            // rbToDatabaseSQLServer
            // 
            this.rbToDatabaseSQLServer.AutoSize = true;
            this.rbToDatabaseSQLServer.Checked = true;
            this.rbToDatabaseSQLServer.Location = new System.Drawing.Point(12, 17);
            this.rbToDatabaseSQLServer.Name = "rbToDatabaseSQLServer";
            this.rbToDatabaseSQLServer.Size = new System.Drawing.Size(74, 17);
            this.rbToDatabaseSQLServer.TabIndex = 20;
            this.rbToDatabaseSQLServer.TabStop = true;
            this.rbToDatabaseSQLServer.Text = "Sql Server";
            this.rbToDatabaseSQLServer.UseVisualStyleBackColor = true;
            this.rbToDatabaseSQLServer.CheckedChanged += new System.EventHandler(this.rbToDatabaseSQLServer_CheckedChanged);
            // 
            // rbToDatabaseMySQL
            // 
            this.rbToDatabaseMySQL.AutoSize = true;
            this.rbToDatabaseMySQL.Location = new System.Drawing.Point(234, 17);
            this.rbToDatabaseMySQL.Name = "rbToDatabaseMySQL";
            this.rbToDatabaseMySQL.Size = new System.Drawing.Size(54, 17);
            this.rbToDatabaseMySQL.TabIndex = 23;
            this.rbToDatabaseMySQL.Text = "MySql";
            this.rbToDatabaseMySQL.UseVisualStyleBackColor = true;
            this.rbToDatabaseMySQL.CheckedChanged += new System.EventHandler(this.rbToDatabaseMySQL_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(490, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Connection String";
            // 
            // rbToIsXml
            // 
            this.rbToIsXml.AutoSize = true;
            this.rbToIsXml.Location = new System.Drawing.Point(7, 154);
            this.rbToIsXml.Name = "rbToIsXml";
            this.rbToIsXml.Size = new System.Drawing.Size(66, 17);
            this.rbToIsXml.TabIndex = 23;
            this.rbToIsXml.TabStop = true;
            this.rbToIsXml.Text = "XML File";
            this.rbToIsXml.UseVisualStyleBackColor = true;
            this.rbToIsXml.CheckedChanged += new System.EventHandler(this.rbToIsXml_CheckedChanged);
            // 
            // rbToIsDatabase
            // 
            this.rbToIsDatabase.AutoSize = true;
            this.rbToIsDatabase.Checked = true;
            this.rbToIsDatabase.Location = new System.Drawing.Point(7, 20);
            this.rbToIsDatabase.Name = "rbToIsDatabase";
            this.rbToIsDatabase.Size = new System.Drawing.Size(71, 17);
            this.rbToIsDatabase.TabIndex = 21;
            this.rbToIsDatabase.TabStop = true;
            this.rbToIsDatabase.Text = "Database";
            this.rbToIsDatabase.UseVisualStyleBackColor = true;
            this.rbToIsDatabase.CheckedChanged += new System.EventHandler(this.rbToIsDatabase_CheckedChanged);
            // 
            // gbActionWriteSchema
            // 
            this.gbActionWriteSchema.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbActionWriteSchema.Controls.Add(this.chkWriteSchemaCreateSelfReferentialConstraints);
            this.gbActionWriteSchema.Controls.Add(this.chkWriteSchemaCreateForeignConstraints);
            this.gbActionWriteSchema.Controls.Add(this.chkWriteSchemaCreateIndexes);
            this.gbActionWriteSchema.Controls.Add(this.chkWriteSchemaCreateTables);
            this.gbActionWriteSchema.Controls.Add(this.btnActionWriteSchemaGo);
            this.gbActionWriteSchema.Location = new System.Drawing.Point(115, 145);
            this.gbActionWriteSchema.Name = "gbActionWriteSchema";
            this.gbActionWriteSchema.Size = new System.Drawing.Size(499, 108);
            this.gbActionWriteSchema.TabIndex = 6;
            this.gbActionWriteSchema.TabStop = false;
            this.gbActionWriteSchema.Text = "Schema Options";
            // 
            // chkWriteSchemaCreateSelfReferentialConstraints
            // 
            this.chkWriteSchemaCreateSelfReferentialConstraints.AutoSize = true;
            this.chkWriteSchemaCreateSelfReferentialConstraints.Checked = true;
            this.chkWriteSchemaCreateSelfReferentialConstraints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteSchemaCreateSelfReferentialConstraints.Location = new System.Drawing.Point(7, 82);
            this.chkWriteSchemaCreateSelfReferentialConstraints.Name = "chkWriteSchemaCreateSelfReferentialConstraints";
            this.chkWriteSchemaCreateSelfReferentialConstraints.Size = new System.Drawing.Size(187, 17);
            this.chkWriteSchemaCreateSelfReferentialConstraints.TabIndex = 14;
            this.chkWriteSchemaCreateSelfReferentialConstraints.Text = "Create Self-Referential Constraints";
            this.chkWriteSchemaCreateSelfReferentialConstraints.UseVisualStyleBackColor = true;
            // 
            // chkWriteSchemaCreateForeignConstraints
            // 
            this.chkWriteSchemaCreateForeignConstraints.AutoSize = true;
            this.chkWriteSchemaCreateForeignConstraints.Checked = true;
            this.chkWriteSchemaCreateForeignConstraints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteSchemaCreateForeignConstraints.Location = new System.Drawing.Point(7, 59);
            this.chkWriteSchemaCreateForeignConstraints.Name = "chkWriteSchemaCreateForeignConstraints";
            this.chkWriteSchemaCreateForeignConstraints.Size = new System.Drawing.Size(171, 17);
            this.chkWriteSchemaCreateForeignConstraints.TabIndex = 13;
            this.chkWriteSchemaCreateForeignConstraints.Text = "Create Foreign Key Constraints";
            this.chkWriteSchemaCreateForeignConstraints.UseVisualStyleBackColor = true;
            // 
            // chkWriteSchemaCreateIndexes
            // 
            this.chkWriteSchemaCreateIndexes.AutoSize = true;
            this.chkWriteSchemaCreateIndexes.Checked = true;
            this.chkWriteSchemaCreateIndexes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteSchemaCreateIndexes.Location = new System.Drawing.Point(7, 40);
            this.chkWriteSchemaCreateIndexes.Name = "chkWriteSchemaCreateIndexes";
            this.chkWriteSchemaCreateIndexes.Size = new System.Drawing.Size(97, 17);
            this.chkWriteSchemaCreateIndexes.TabIndex = 12;
            this.chkWriteSchemaCreateIndexes.Text = "Create Indexes";
            this.chkWriteSchemaCreateIndexes.UseVisualStyleBackColor = true;
            // 
            // chkWriteSchemaCreateTables
            // 
            this.chkWriteSchemaCreateTables.AutoSize = true;
            this.chkWriteSchemaCreateTables.Checked = true;
            this.chkWriteSchemaCreateTables.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteSchemaCreateTables.Location = new System.Drawing.Point(7, 20);
            this.chkWriteSchemaCreateTables.Name = "chkWriteSchemaCreateTables";
            this.chkWriteSchemaCreateTables.Size = new System.Drawing.Size(92, 17);
            this.chkWriteSchemaCreateTables.TabIndex = 11;
            this.chkWriteSchemaCreateTables.Text = "Create Tables";
            this.chkWriteSchemaCreateTables.UseVisualStyleBackColor = true;
            // 
            // btnActionWriteSchemaGo
            // 
            this.btnActionWriteSchemaGo.Location = new System.Drawing.Point(433, 76);
            this.btnActionWriteSchemaGo.Name = "btnActionWriteSchemaGo";
            this.btnActionWriteSchemaGo.Size = new System.Drawing.Size(55, 23);
            this.btnActionWriteSchemaGo.TabIndex = 0;
            this.btnActionWriteSchemaGo.Text = "Go";
            this.btnActionWriteSchemaGo.UseVisualStyleBackColor = true;
            this.btnActionWriteSchemaGo.Click += new System.EventHandler(this.btnActionWriteSchemaGo_Click);
            // 
            // rbActionWriteSchema
            // 
            this.rbActionWriteSchema.AutoSize = true;
            this.rbActionWriteSchema.Location = new System.Drawing.Point(3, 145);
            this.rbActionWriteSchema.Name = "rbActionWriteSchema";
            this.rbActionWriteSchema.Size = new System.Drawing.Size(92, 17);
            this.rbActionWriteSchema.TabIndex = 2;
            this.rbActionWriteSchema.Text = "Write Schema";
            this.rbActionWriteSchema.UseVisualStyleBackColor = true;
            this.rbActionWriteSchema.CheckedChanged += new System.EventHandler(this.rbActionWriteSchema_CheckedChanged);
            // 
            // rbActionWriteData
            // 
            this.rbActionWriteData.AutoSize = true;
            this.rbActionWriteData.Location = new System.Drawing.Point(10, 277);
            this.rbActionWriteData.Name = "rbActionWriteData";
            this.rbActionWriteData.Size = new System.Drawing.Size(76, 17);
            this.rbActionWriteData.TabIndex = 1;
            this.rbActionWriteData.Text = "Write Data";
            this.rbActionWriteData.UseVisualStyleBackColor = true;
            this.rbActionWriteData.CheckedChanged += new System.EventHandler(this.rbActionWriteData_CheckedChanged);
            // 
            // rbActionScript
            // 
            this.rbActionScript.AutoSize = true;
            this.rbActionScript.Checked = true;
            this.rbActionScript.Location = new System.Drawing.Point(5, 25);
            this.rbActionScript.Name = "rbActionScript";
            this.rbActionScript.Size = new System.Drawing.Size(104, 17);
            this.rbActionScript.TabIndex = 0;
            this.rbActionScript.TabStop = true;
            this.rbActionScript.Text = "Generate Scripts";
            this.rbActionScript.UseVisualStyleBackColor = true;
            this.rbActionScript.CheckedChanged += new System.EventHandler(this.rbActionScript_CheckedChanged);
            // 
            // gbActionWriteData
            // 
            this.gbActionWriteData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbActionWriteData.Controls.Add(this.chkActionWriteOnlyRequiredData);
            this.gbActionWriteData.Controls.Add(this.chkActionWriteDataDryRun);
            this.gbActionWriteData.Controls.Add(this.rbActionWriteDatabaseData);
            this.gbActionWriteData.Controls.Add(this.btnActionWriteDataGo);
            this.gbActionWriteData.Location = new System.Drawing.Point(115, 277);
            this.gbActionWriteData.Name = "gbActionWriteData";
            this.gbActionWriteData.Size = new System.Drawing.Size(504, 104);
            this.gbActionWriteData.TabIndex = 5;
            this.gbActionWriteData.TabStop = false;
            this.gbActionWriteData.Text = "Data Options";
            // 
            // chkActionWriteDataDryRun
            // 
            this.chkActionWriteDataDryRun.AutoSize = true;
            this.chkActionWriteDataDryRun.Checked = true;
            this.chkActionWriteDataDryRun.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActionWriteDataDryRun.Location = new System.Drawing.Point(13, 46);
            this.chkActionWriteDataDryRun.Name = "chkActionWriteDataDryRun";
            this.chkActionWriteDataDryRun.Size = new System.Drawing.Size(412, 17);
            this.chkActionWriteDataDryRun.TabIndex = 3;
            this.chkActionWriteDataDryRun.Text = "Dry Run (only create SQL files which contain the LOAD DATA INFILE statements)";
            this.chkActionWriteDataDryRun.UseVisualStyleBackColor = true;
            // 
            // rbActionWriteDatabaseData
            // 
            this.rbActionWriteDatabaseData.AutoSize = true;
            this.rbActionWriteDatabaseData.Checked = true;
            this.rbActionWriteDatabaseData.Location = new System.Drawing.Point(13, 23);
            this.rbActionWriteDatabaseData.Name = "rbActionWriteDatabaseData";
            this.rbActionWriteDatabaseData.Size = new System.Drawing.Size(97, 17);
            this.rbActionWriteDatabaseData.TabIndex = 1;
            this.rbActionWriteDatabaseData.TabStop = true;
            this.rbActionWriteDatabaseData.Text = "Database Data";
            this.rbActionWriteDatabaseData.UseVisualStyleBackColor = true;
            // 
            // btnActionWriteDataGo
            // 
            this.btnActionWriteDataGo.Location = new System.Drawing.Point(433, 69);
            this.btnActionWriteDataGo.Name = "btnActionWriteDataGo";
            this.btnActionWriteDataGo.Size = new System.Drawing.Size(55, 23);
            this.btnActionWriteDataGo.TabIndex = 0;
            this.btnActionWriteDataGo.Text = "Go";
            this.btnActionWriteDataGo.UseVisualStyleBackColor = true;
            this.btnActionWriteDataGo.Click += new System.EventHandler(this.btnActionWriteDataGo_Click);
            // 
            // gbActionScript
            // 
            this.gbActionScript.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbActionScript.Controls.Add(this.chkScriptOnlyRequiredData);
            this.gbActionScript.Controls.Add(this.chkScriptLoadData);
            this.gbActionScript.Controls.Add(this.btnActionScriptGo);
            this.gbActionScript.Controls.Add(this.chkScriptCreateConstraints);
            this.gbActionScript.Controls.Add(this.chkScriptCreateIndexes);
            this.gbActionScript.Controls.Add(this.chkScriptCreateTables);
            this.gbActionScript.Location = new System.Drawing.Point(115, 25);
            this.gbActionScript.Name = "gbActionScript";
            this.gbActionScript.Size = new System.Drawing.Size(499, 114);
            this.gbActionScript.TabIndex = 3;
            this.gbActionScript.TabStop = false;
            this.gbActionScript.Text = "Script Options";
            // 
            // btnActionScriptGo
            // 
            this.btnActionScriptGo.Location = new System.Drawing.Point(433, 82);
            this.btnActionScriptGo.Name = "btnActionScriptGo";
            this.btnActionScriptGo.Size = new System.Drawing.Size(55, 23);
            this.btnActionScriptGo.TabIndex = 12;
            this.btnActionScriptGo.Text = "Go";
            this.btnActionScriptGo.UseVisualStyleBackColor = true;
            this.btnActionScriptGo.Click += new System.EventHandler(this.btnActionScriptGo_Click);
            // 
            // chkScriptCreateConstraints
            // 
            this.chkScriptCreateConstraints.AutoSize = true;
            this.chkScriptCreateConstraints.Checked = true;
            this.chkScriptCreateConstraints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScriptCreateConstraints.Location = new System.Drawing.Point(7, 88);
            this.chkScriptCreateConstraints.Name = "chkScriptCreateConstraints";
            this.chkScriptCreateConstraints.Size = new System.Drawing.Size(112, 17);
            this.chkScriptCreateConstraints.TabIndex = 2;
            this.chkScriptCreateConstraints.Text = "Create Constraints";
            this.chkScriptCreateConstraints.UseVisualStyleBackColor = true;
            // 
            // chkScriptCreateIndexes
            // 
            this.chkScriptCreateIndexes.AutoSize = true;
            this.chkScriptCreateIndexes.Checked = true;
            this.chkScriptCreateIndexes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScriptCreateIndexes.Location = new System.Drawing.Point(7, 65);
            this.chkScriptCreateIndexes.Name = "chkScriptCreateIndexes";
            this.chkScriptCreateIndexes.Size = new System.Drawing.Size(97, 17);
            this.chkScriptCreateIndexes.TabIndex = 1;
            this.chkScriptCreateIndexes.Text = "Create Indexes";
            this.chkScriptCreateIndexes.UseVisualStyleBackColor = true;
            // 
            // chkScriptCreateTables
            // 
            this.chkScriptCreateTables.AutoSize = true;
            this.chkScriptCreateTables.Checked = true;
            this.chkScriptCreateTables.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScriptCreateTables.Location = new System.Drawing.Point(7, 19);
            this.chkScriptCreateTables.Name = "chkScriptCreateTables";
            this.chkScriptCreateTables.Size = new System.Drawing.Size(92, 17);
            this.chkScriptCreateTables.TabIndex = 0;
            this.chkScriptCreateTables.Text = "Create Tables";
            this.chkScriptCreateTables.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnPopupTableList
            // 
            this.btnPopupTableList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPopupTableList.Enabled = false;
            this.btnPopupTableList.Location = new System.Drawing.Point(403, 13);
            this.btnPopupTableList.Name = "btnPopupTableList";
            this.btnPopupTableList.Size = new System.Drawing.Size(118, 23);
            this.btnPopupTableList.TabIndex = 39;
            this.btnPopupTableList.Text = "Popup Table List";
            this.btnPopupTableList.UseVisualStyleBackColor = true;
            this.btnPopupTableList.Click += new System.EventHandler(this.btnPopupTableList_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(190, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 40;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(630, 508);
            this.tabControl1.TabIndex = 41;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(622, 482);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Source";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gbTo);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(622, 482);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Destination";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnPopupTableList);
            this.tabPage3.Controls.Add(this.button1);
            this.tabPage3.Controls.Add(this.lvTableList);
            this.tabPage3.Controls.Add(this.btnListTables);
            this.tabPage3.Controls.Add(this.chkSelectAllTables);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(622, 482);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Tables";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.rbActionWriteSchema);
            this.tabPage4.Controls.Add(this.gbActionWriteSchema);
            this.tabPage4.Controls.Add(this.rbActionWriteData);
            this.tabPage4.Controls.Add(this.rbActionScript);
            this.tabPage4.Controls.Add(this.gbActionScript);
            this.tabPage4.Controls.Add(this.gbActionWriteData);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(622, 482);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Action";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnDestinationTestConnection
            // 
            this.btnDestinationTestConnection.Location = new System.Drawing.Point(447, 62);
            this.btnDestinationTestConnection.Name = "btnDestinationTestConnection";
            this.btnDestinationTestConnection.Size = new System.Drawing.Size(134, 23);
            this.btnDestinationTestConnection.TabIndex = 36;
            this.btnDestinationTestConnection.Text = "&Test Connection";
            this.btnDestinationTestConnection.UseVisualStyleBackColor = true;
            this.btnDestinationTestConnection.Click += new System.EventHandler(this.btnDestinationTestConnection_Click);
            // 
            // btnSourceTestConnection
            // 
            this.btnSourceTestConnection.Location = new System.Drawing.Point(452, 69);
            this.btnSourceTestConnection.Name = "btnSourceTestConnection";
            this.btnSourceTestConnection.Size = new System.Drawing.Size(134, 23);
            this.btnSourceTestConnection.TabIndex = 37;
            this.btnSourceTestConnection.Text = "&Test Connection";
            this.btnSourceTestConnection.UseVisualStyleBackColor = true;
            this.btnSourceTestConnection.Click += new System.EventHandler(this.btnSourceTestConnection_Click);
            // 
            // chkScriptLoadData
            // 
            this.chkScriptLoadData.AutoSize = true;
            this.chkScriptLoadData.Checked = true;
            this.chkScriptLoadData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScriptLoadData.Location = new System.Drawing.Point(7, 42);
            this.chkScriptLoadData.Name = "chkScriptLoadData";
            this.chkScriptLoadData.Size = new System.Drawing.Size(76, 17);
            this.chkScriptLoadData.TabIndex = 14;
            this.chkScriptLoadData.Text = "Load Data";
            this.chkScriptLoadData.UseVisualStyleBackColor = true;
            this.chkScriptLoadData.CheckedChanged += new System.EventHandler(this.chkScriptLoadData_CheckedChanged);
            // 
            // chkActionWriteOnlyRequiredData
            // 
            this.chkActionWriteOnlyRequiredData.AutoSize = true;
            this.chkActionWriteOnlyRequiredData.Checked = true;
            this.chkActionWriteOnlyRequiredData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActionWriteOnlyRequiredData.Location = new System.Drawing.Point(13, 69);
            this.chkActionWriteOnlyRequiredData.Name = "chkActionWriteOnlyRequiredData";
            this.chkActionWriteOnlyRequiredData.Size = new System.Drawing.Size(119, 17);
            this.chkActionWriteOnlyRequiredData.TabIndex = 4;
            this.chkActionWriteOnlyRequiredData.Text = "Only Required Data";
            this.chkActionWriteOnlyRequiredData.UseVisualStyleBackColor = true;
            // 
            // chkScriptOnlyRequiredData
            // 
            this.chkScriptOnlyRequiredData.AutoSize = true;
            this.chkScriptOnlyRequiredData.Checked = true;
            this.chkScriptOnlyRequiredData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScriptOnlyRequiredData.Location = new System.Drawing.Point(102, 42);
            this.chkScriptOnlyRequiredData.Name = "chkScriptOnlyRequiredData";
            this.chkScriptOnlyRequiredData.Size = new System.Drawing.Size(119, 17);
            this.chkScriptOnlyRequiredData.TabIndex = 15;
            this.chkScriptOnlyRequiredData.Text = "Only Required Data";
            this.chkScriptOnlyRequiredData.UseVisualStyleBackColor = true;
            // 
            // frmCopier3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 530);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.status);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCopier3";
            this.Text = "Database Copier";
            this.Load += new System.EventHandler(this.frmCopier2_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gbFromFile.ResumeLayout(false);
            this.gbFromFile.PerformLayout();
            this.gbFromDatabase.ResumeLayout(false);
            this.gbFromDatabase.PerformLayout();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.gbTo.ResumeLayout(false);
            this.gbTo.PerformLayout();
            this.gbToFile.ResumeLayout(false);
            this.gbToFile.PerformLayout();
            this.gbToDatabase.ResumeLayout(false);
            this.gbToDatabase.PerformLayout();
            this.gbActionWriteSchema.ResumeLayout(false);
            this.gbActionWriteSchema.PerformLayout();
            this.gbActionWriteData.ResumeLayout(false);
            this.gbActionWriteData.PerformLayout();
            this.gbActionScript.ResumeLayout(false);
            this.gbActionScript.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkSelectAllTables;
		private System.Windows.Forms.Button btnListTables;
		private System.Windows.Forms.ListView lvTableList;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton rbFromIsXml;
		private System.Windows.Forms.RadioButton rbFromIsCSV;
		private System.Windows.Forms.RadioButton rbFromIsDatabase;
		private System.Windows.Forms.GroupBox gbFromDatabase;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.ComboBox cboFromConnectionString;
		private System.Windows.Forms.RadioButton rbFromDatabaseOracle;
		private System.Windows.Forms.RadioButton rbFromDatabaseSQLServer;
		private System.Windows.Forms.RadioButton rbFromDatabaseMySQL;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.GroupBox gbFromFile;
		private System.Windows.Forms.TextBox txtFromFileName;
		private System.Windows.Forms.Button btnFromFilenamePrompt;
		private System.Windows.Forms.TextBox txtFromDatabaseName;
		private System.Windows.Forms.StatusStrip status;
		private System.Windows.Forms.GroupBox gbTo;
		private System.Windows.Forms.GroupBox gbToFile;
		private System.Windows.Forms.TextBox txtToFolderName;
		private System.Windows.Forms.Button btnToFolderNamePrompt;
		private System.Windows.Forms.GroupBox gbToDatabase;
		private System.Windows.Forms.TextBox txtToDatabaseName;
		private System.Windows.Forms.ComboBox cboToConnectionString;
		private System.Windows.Forms.RadioButton rbToDatabaseOracle;
		private System.Windows.Forms.RadioButton rbToDatabaseSQLServer;
		private System.Windows.Forms.RadioButton rbToDatabaseMySQL;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton rbToIsXml;
        private System.Windows.Forms.RadioButton rbToIsDatabase;
		private System.Windows.Forms.GroupBox gbActionScript;
		private System.Windows.Forms.RadioButton rbActionWriteSchema;
		private System.Windows.Forms.RadioButton rbActionWriteData;
        private System.Windows.Forms.RadioButton rbActionScript;
		private System.Windows.Forms.CheckBox chkScriptCreateConstraints;
		private System.Windows.Forms.CheckBox chkScriptCreateIndexes;
        private System.Windows.Forms.CheckBox chkScriptCreateTables;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ToolStripProgressBar statusProgress;
		private System.Windows.Forms.ToolStripStatusLabel statusText;
		private System.Windows.Forms.GroupBox gbActionWriteData;
		private System.Windows.Forms.Button btnActionWriteDataGo;
		private System.Windows.Forms.GroupBox gbActionWriteSchema;
		private System.Windows.Forms.Button btnActionWriteSchemaGo;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnActionScriptGo;
		private System.Windows.Forms.CheckBox chkWriteSchemaCreateForeignConstraints;
		private System.Windows.Forms.CheckBox chkWriteSchemaCreateIndexes;
		private System.Windows.Forms.CheckBox chkWriteSchemaCreateTables;
        private System.Windows.Forms.CheckBox chkWriteSchemaCreateSelfReferentialConstraints;
        private System.Windows.Forms.Button btnPopupTableList;
		private System.Windows.Forms.RadioButton rbActionWriteDatabaseData;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.RadioButton rbFromDatabasePostgreSQL;
        private System.Windows.Forms.RadioButton rbToDatabasePostgreSQL;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.CheckBox chkActionWriteDataDryRun;
        private System.Windows.Forms.Button btnDestinationTestConnection;
        private System.Windows.Forms.Button btnSourceTestConnection;
        private System.Windows.Forms.CheckBox chkScriptLoadData;
        private System.Windows.Forms.CheckBox chkActionWriteOnlyRequiredData;
        private System.Windows.Forms.CheckBox chkScriptOnlyRequiredData;
	}
}