namespace GrinGlobal.DatabaseCopier {
	partial class frmCopier2 {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCopier2));
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbActionWriteSchema = new System.Windows.Forms.GroupBox();
            this.chkWriteSchemaCreateSelfReferentialConstraints = new System.Windows.Forms.CheckBox();
            this.chkWriteSchemaCreateForeignConstraints = new System.Windows.Forms.CheckBox();
            this.chkWriteSchemaCreateIndexes = new System.Windows.Forms.CheckBox();
            this.chkWriteSchemaCreateTables = new System.Windows.Forms.CheckBox();
            this.btnActionWriteSchemaGo = new System.Windows.Forms.Button();
            this.rbActionWriteSchema = new System.Windows.Forms.RadioButton();
            this.rbActionWriteData = new System.Windows.Forms.RadioButton();
            this.rbActionScript = new System.Windows.Forms.RadioButton();
            this.rbActionGenerateSearch = new System.Windows.Forms.RadioButton();
            this.gbActionWriteData = new System.Windows.Forms.GroupBox();
            this.rbActionWriteMappingData = new System.Windows.Forms.RadioButton();
            this.rbActionWriteDatabaseData = new System.Windows.Forms.RadioButton();
            this.btnActionWriteDataGo = new System.Windows.Forms.Button();
            this.gbActionSearch = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboActionGenerateSearch = new System.Windows.Forms.ComboBox();
            this.btnActionSearchGo = new System.Windows.Forms.Button();
            this.gbActionScript = new System.Windows.Forms.GroupBox();
            this.chkScriptUseMyISAMDuringMigration = new System.Windows.Forms.CheckBox();
            this.btnActionScriptGo = new System.Windows.Forms.Button();
            this.txtToMigrateToDatabaseName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtToMigrateFromDatabaseName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkScriptDropMigrationTables = new System.Windows.Forms.CheckBox();
            this.chkScriptMigrationInserts = new System.Windows.Forms.CheckBox();
            this.chkScriptCreateMigrationTables = new System.Windows.Forms.CheckBox();
            this.chkScriptCreateConstraints = new System.Windows.Forms.CheckBox();
            this.chkScriptCreateIndexes = new System.Windows.Forms.CheckBox();
            this.chkScriptCreateTables = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnPopupTableList = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.gbFromFile.SuspendLayout();
            this.gbFromDatabase.SuspendLayout();
            this.status.SuspendLayout();
            this.gbTo.SuspendLayout();
            this.gbToFile.SuspendLayout();
            this.gbToDatabase.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbActionWriteSchema.SuspendLayout();
            this.gbActionWriteData.SuspendLayout();
            this.gbActionSearch.SuspendLayout();
            this.gbActionScript.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkSelectAllTables
            // 
            this.chkSelectAllTables.AutoSize = true;
            this.chkSelectAllTables.Location = new System.Drawing.Point(19, 117);
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
            this.btnListTables.Location = new System.Drawing.Point(545, 115);
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
            this.lvTableList.Location = new System.Drawing.Point(12, 144);
            this.lvTableList.Name = "lvTableList";
            this.lvTableList.Size = new System.Drawing.Size(624, 146);
            this.lvTableList.TabIndex = 32;
            this.lvTableList.UseCompatibleStateImageBehavior = false;
            this.lvTableList.View = System.Windows.Forms.View.List;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.gbFromFile);
            this.groupBox3.Controls.Add(this.gbFromDatabase);
            this.groupBox3.Controls.Add(this.rbFromIsXml);
            this.groupBox3.Controls.Add(this.rbFromIsCSV);
            this.groupBox3.Controls.Add(this.rbFromIsDatabase);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(624, 99);
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
            this.gbFromFile.Location = new System.Drawing.Point(88, 70);
            this.gbFromFile.Name = "gbFromFile";
            this.gbFromFile.Size = new System.Drawing.Size(530, 68);
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
            this.txtFromFileName.Size = new System.Drawing.Size(484, 20);
            this.txtFromFileName.TabIndex = 19;
            this.txtFromFileName.Text = "D:\\test_db_creator";
            this.txtFromFileName.TextChanged += new System.EventHandler(this.txtFromFileName_TextChanged);
            // 
            // btnFromFilenamePrompt
            // 
            this.btnFromFilenamePrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFromFilenamePrompt.Location = new System.Drawing.Point(496, 16);
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
            this.gbFromDatabase.Controls.Add(this.rbFromDatabasePostgreSQL);
            this.gbFromDatabase.Controls.Add(this.txtFromDatabaseName);
            this.gbFromDatabase.Controls.Add(this.label14);
            this.gbFromDatabase.Controls.Add(this.cboFromConnectionString);
            this.gbFromDatabase.Controls.Add(this.rbFromDatabaseOracle);
            this.gbFromDatabase.Controls.Add(this.rbFromDatabaseSQLServer);
            this.gbFromDatabase.Controls.Add(this.rbFromDatabaseMySQL);
            this.gbFromDatabase.Controls.Add(this.label13);
            this.gbFromDatabase.Location = new System.Drawing.Point(88, 20);
            this.gbFromDatabase.Name = "gbFromDatabase";
            this.gbFromDatabase.Size = new System.Drawing.Size(530, 68);
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
            this.txtFromDatabaseName.Location = new System.Drawing.Point(346, 16);
            this.txtFromDatabaseName.Name = "txtFromDatabaseName";
            this.txtFromDatabaseName.Size = new System.Drawing.Size(80, 20);
            this.txtFromDatabaseName.TabIndex = 35;
            this.txtFromDatabaseName.Text = "gringlobal";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(286, 19);
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
            this.cboFromConnectionString.Size = new System.Drawing.Size(518, 21);
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
            this.label13.Location = new System.Drawing.Point(433, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(91, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "Connection String";
            // 
            // rbFromIsXml
            // 
            this.rbFromIsXml.AutoSize = true;
            this.rbFromIsXml.Location = new System.Drawing.Point(7, 66);
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
            this.rbFromIsCSV.Location = new System.Drawing.Point(7, 43);
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
            this.status.Size = new System.Drawing.Size(648, 22);
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
            this.gbTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTo.Controls.Add(this.gbToFile);
            this.gbTo.Controls.Add(this.gbToDatabase);
            this.gbTo.Controls.Add(this.rbToIsXml);
            this.gbTo.Controls.Add(this.rbToIsDatabase);
            this.gbTo.Location = new System.Drawing.Point(13, 296);
            this.gbTo.Name = "gbTo";
            this.gbTo.Size = new System.Drawing.Size(624, 99);
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
            this.gbToFile.Location = new System.Drawing.Point(88, 70);
            this.gbToFile.Name = "gbToFile";
            this.gbToFile.Size = new System.Drawing.Size(530, 68);
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
            this.txtToFolderName.Size = new System.Drawing.Size(484, 20);
            this.txtToFolderName.TabIndex = 19;
            this.txtToFolderName.Text = "D:\\test_db_creator\\";
            // 
            // btnToFolderNamePrompt
            // 
            this.btnToFolderNamePrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToFolderNamePrompt.Location = new System.Drawing.Point(496, 16);
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
            this.gbToDatabase.Controls.Add(this.rbToDatabasePostgreSQL);
            this.gbToDatabase.Controls.Add(this.txtToDatabaseName);
            this.gbToDatabase.Controls.Add(this.label1);
            this.gbToDatabase.Controls.Add(this.cboToConnectionString);
            this.gbToDatabase.Controls.Add(this.rbToDatabaseOracle);
            this.gbToDatabase.Controls.Add(this.rbToDatabaseSQLServer);
            this.gbToDatabase.Controls.Add(this.rbToDatabaseMySQL);
            this.gbToDatabase.Controls.Add(this.label2);
            this.gbToDatabase.Location = new System.Drawing.Point(88, 20);
            this.gbToDatabase.Name = "gbToDatabase";
            this.gbToDatabase.Size = new System.Drawing.Size(530, 68);
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
            this.txtToDatabaseName.Location = new System.Drawing.Point(344, 16);
            this.txtToDatabaseName.Name = "txtToDatabaseName";
            this.txtToDatabaseName.Size = new System.Drawing.Size(81, 20);
            this.txtToDatabaseName.TabIndex = 35;
            this.txtToDatabaseName.Text = "gringlobal";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(285, 19);
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
            this.cboToConnectionString.Size = new System.Drawing.Size(518, 21);
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
            this.label2.Location = new System.Drawing.Point(433, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Connection String";
            // 
            // rbToIsXml
            // 
            this.rbToIsXml.AutoSize = true;
            this.rbToIsXml.Location = new System.Drawing.Point(7, 43);
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
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gbActionWriteSchema);
            this.groupBox1.Controls.Add(this.rbActionWriteSchema);
            this.groupBox1.Controls.Add(this.rbActionWriteData);
            this.groupBox1.Controls.Add(this.rbActionScript);
            this.groupBox1.Controls.Add(this.rbActionGenerateSearch);
            this.groupBox1.Controls.Add(this.gbActionWriteData);
            this.groupBox1.Controls.Add(this.gbActionSearch);
            this.groupBox1.Controls.Add(this.gbActionScript);
            this.groupBox1.Location = new System.Drawing.Point(13, 402);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(623, 100);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Action:";
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
            this.gbActionWriteSchema.Location = new System.Drawing.Point(70, 12);
            this.gbActionWriteSchema.Name = "gbActionWriteSchema";
            this.gbActionWriteSchema.Size = new System.Drawing.Size(494, 88);
            this.gbActionWriteSchema.TabIndex = 6;
            this.gbActionWriteSchema.TabStop = false;
            this.gbActionWriteSchema.Text = "Schema Options";
            // 
            // chkWriteSchemaCreateSelfReferentialConstraints
            // 
            this.chkWriteSchemaCreateSelfReferentialConstraints.AutoSize = true;
            this.chkWriteSchemaCreateSelfReferentialConstraints.Checked = true;
            this.chkWriteSchemaCreateSelfReferentialConstraints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteSchemaCreateSelfReferentialConstraints.Location = new System.Drawing.Point(184, 59);
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
            this.btnActionWriteSchemaGo.Location = new System.Drawing.Point(433, 59);
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
            this.rbActionWriteSchema.Location = new System.Drawing.Point(6, 32);
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
            this.rbActionWriteData.Location = new System.Drawing.Point(6, 52);
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
            this.rbActionScript.Location = new System.Drawing.Point(6, 12);
            this.rbActionScript.Name = "rbActionScript";
            this.rbActionScript.Size = new System.Drawing.Size(104, 17);
            this.rbActionScript.TabIndex = 0;
            this.rbActionScript.TabStop = true;
            this.rbActionScript.Text = "Generate Scripts";
            this.rbActionScript.UseVisualStyleBackColor = true;
            this.rbActionScript.CheckedChanged += new System.EventHandler(this.rbActionScript_CheckedChanged);
            // 
            // rbActionGenerateSearch
            // 
            this.rbActionGenerateSearch.AutoSize = true;
            this.rbActionGenerateSearch.Location = new System.Drawing.Point(6, 71);
            this.rbActionGenerateSearch.Name = "rbActionGenerateSearch";
            this.rbActionGenerateSearch.Size = new System.Drawing.Size(106, 17);
            this.rbActionGenerateSearch.TabIndex = 7;
            this.rbActionGenerateSearch.Text = "Generate Search";
            this.rbActionGenerateSearch.UseVisualStyleBackColor = true;
            this.rbActionGenerateSearch.CheckedChanged += new System.EventHandler(this.rbActionGenerateSearch_CheckedChanged);
            // 
            // gbActionWriteData
            // 
            this.gbActionWriteData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbActionWriteData.Controls.Add(this.rbActionWriteMappingData);
            this.gbActionWriteData.Controls.Add(this.rbActionWriteDatabaseData);
            this.gbActionWriteData.Controls.Add(this.btnActionWriteDataGo);
            this.gbActionWriteData.Location = new System.Drawing.Point(64, 32);
            this.gbActionWriteData.Name = "gbActionWriteData";
            this.gbActionWriteData.Size = new System.Drawing.Size(494, 88);
            this.gbActionWriteData.TabIndex = 5;
            this.gbActionWriteData.TabStop = false;
            this.gbActionWriteData.Text = "Data Options";
            // 
            // rbActionWriteMappingData
            // 
            this.rbActionWriteMappingData.AutoSize = true;
            this.rbActionWriteMappingData.Location = new System.Drawing.Point(127, 23);
            this.rbActionWriteMappingData.Name = "rbActionWriteMappingData";
            this.rbActionWriteMappingData.Size = new System.Drawing.Size(141, 17);
            this.rbActionWriteMappingData.TabIndex = 2;
            this.rbActionWriteMappingData.Text = "Dataview Mapping Data";
            this.rbActionWriteMappingData.UseVisualStyleBackColor = true;
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
            this.btnActionWriteDataGo.Location = new System.Drawing.Point(433, 59);
            this.btnActionWriteDataGo.Name = "btnActionWriteDataGo";
            this.btnActionWriteDataGo.Size = new System.Drawing.Size(55, 23);
            this.btnActionWriteDataGo.TabIndex = 0;
            this.btnActionWriteDataGo.Text = "Go";
            this.btnActionWriteDataGo.UseVisualStyleBackColor = true;
            this.btnActionWriteDataGo.Click += new System.EventHandler(this.btnActionWriteDataGo_Click);
            // 
            // gbActionSearch
            // 
            this.gbActionSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbActionSearch.Controls.Add(this.label5);
            this.gbActionSearch.Controls.Add(this.cboActionGenerateSearch);
            this.gbActionSearch.Controls.Add(this.btnActionSearchGo);
            this.gbActionSearch.Location = new System.Drawing.Point(64, 6);
            this.gbActionSearch.Name = "gbActionSearch";
            this.gbActionSearch.Size = new System.Drawing.Size(494, 88);
            this.gbActionSearch.TabIndex = 8;
            this.gbActionSearch.TabStop = false;
            this.gbActionSearch.Text = "Search Options";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(439, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Generation of Search SQL requires a MySQL database.  Specify the connection strin" +
                "g here:";
            // 
            // cboActionGenerateSearch
            // 
            this.cboActionGenerateSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboActionGenerateSearch.FormattingEnabled = true;
            this.cboActionGenerateSearch.Items.AddRange(new object[] {
            "----- Local -----",
            "Data Source=localhost;Database=gringlobal2;user id=root;password=passw0rd!",
            "----- DEV -----",
            "Data Source=mw25pi-grin-t1.visitor.iastate.edu;Database=dev2;user id=root;passwor" +
                "d=passw0rd!",
            "Data Source=mw25pi-grin-t1.visitor.iastate.edu, 1435;Database=devtestsql;User Id=" +
                "test_user;password=passw0rd!",
            "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(PORT=1521)(HOST=129.186.187.178)" +
                ")(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=DEVTESTORA;password=passw0rd!",
            "----- BETA -----",
            "Data Source=mw25pi-grin-t1.visitor.iastate.edu;Database=dev3;user id=root;passwor" +
                "d=passw0rd!",
            "----- PRODUCTION -----",
            "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.100.146.29)(PORT=1521))" +
                "(CONNECT_DATA=(SID=NPGS)));User Id=nc7bw;Password=sunpasswd",
            "Driver={Microsoft ODBC for Oracle};Server=npgs.ars-grin.gov;Uid=nc7bw;Pwd=sunpass" +
                "wd"});
            this.cboActionGenerateSearch.Location = new System.Drawing.Point(6, 34);
            this.cboActionGenerateSearch.Name = "cboActionGenerateSearch";
            this.cboActionGenerateSearch.Size = new System.Drawing.Size(482, 21);
            this.cboActionGenerateSearch.TabIndex = 24;
            // 
            // btnActionSearchGo
            // 
            this.btnActionSearchGo.Location = new System.Drawing.Point(433, 59);
            this.btnActionSearchGo.Name = "btnActionSearchGo";
            this.btnActionSearchGo.Size = new System.Drawing.Size(55, 23);
            this.btnActionSearchGo.TabIndex = 0;
            this.btnActionSearchGo.Text = "Go";
            this.btnActionSearchGo.UseVisualStyleBackColor = true;
            this.btnActionSearchGo.Click += new System.EventHandler(this.btnActionSearchGo_Click);
            // 
            // gbActionScript
            // 
            this.gbActionScript.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbActionScript.Controls.Add(this.chkScriptUseMyISAMDuringMigration);
            this.gbActionScript.Controls.Add(this.btnActionScriptGo);
            this.gbActionScript.Controls.Add(this.txtToMigrateToDatabaseName);
            this.gbActionScript.Controls.Add(this.label4);
            this.gbActionScript.Controls.Add(this.txtToMigrateFromDatabaseName);
            this.gbActionScript.Controls.Add(this.label3);
            this.gbActionScript.Controls.Add(this.chkScriptDropMigrationTables);
            this.gbActionScript.Controls.Add(this.chkScriptMigrationInserts);
            this.gbActionScript.Controls.Add(this.chkScriptCreateMigrationTables);
            this.gbActionScript.Controls.Add(this.chkScriptCreateConstraints);
            this.gbActionScript.Controls.Add(this.chkScriptCreateIndexes);
            this.gbActionScript.Controls.Add(this.chkScriptCreateTables);
            this.gbActionScript.Location = new System.Drawing.Point(117, 9);
            this.gbActionScript.Name = "gbActionScript";
            this.gbActionScript.Size = new System.Drawing.Size(494, 85);
            this.gbActionScript.TabIndex = 3;
            this.gbActionScript.TabStop = false;
            this.gbActionScript.Text = "Script Options";
            // 
            // chkScriptUseMyISAMDuringMigration
            // 
            this.chkScriptUseMyISAMDuringMigration.AutoSize = true;
            this.chkScriptUseMyISAMDuringMigration.Checked = true;
            this.chkScriptUseMyISAMDuringMigration.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScriptUseMyISAMDuringMigration.Location = new System.Drawing.Point(259, 58);
            this.chkScriptUseMyISAMDuringMigration.Name = "chkScriptUseMyISAMDuringMigration";
            this.chkScriptUseMyISAMDuringMigration.Size = new System.Drawing.Size(168, 17);
            this.chkScriptUseMyISAMDuringMigration.TabIndex = 13;
            this.chkScriptUseMyISAMDuringMigration.Text = "Use MyISAM During Migration";
            this.chkScriptUseMyISAMDuringMigration.UseVisualStyleBackColor = true;
            this.chkScriptUseMyISAMDuringMigration.CheckedChanged += new System.EventHandler(this.chkScriptUseMyISAMEngine_CheckedChanged);
            // 
            // btnActionScriptGo
            // 
            this.btnActionScriptGo.Location = new System.Drawing.Point(433, 56);
            this.btnActionScriptGo.Name = "btnActionScriptGo";
            this.btnActionScriptGo.Size = new System.Drawing.Size(55, 23);
            this.btnActionScriptGo.TabIndex = 12;
            this.btnActionScriptGo.Text = "Go";
            this.btnActionScriptGo.UseVisualStyleBackColor = true;
            this.btnActionScriptGo.Click += new System.EventHandler(this.btnActionScriptGo_Click);
            // 
            // txtToMigrateToDatabaseName
            // 
            this.txtToMigrateToDatabaseName.Location = new System.Drawing.Point(388, 34);
            this.txtToMigrateToDatabaseName.Name = "txtToMigrateToDatabaseName";
            this.txtToMigrateToDatabaseName.Size = new System.Drawing.Size(100, 20);
            this.txtToMigrateToDatabaseName.TabIndex = 10;
            this.txtToMigrateToDatabaseName.Text = "gringlobal";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(270, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Migrate to db name:";
            // 
            // txtToMigrateFromDatabaseName
            // 
            this.txtToMigrateFromDatabaseName.Location = new System.Drawing.Point(388, 12);
            this.txtToMigrateFromDatabaseName.Name = "txtToMigrateFromDatabaseName";
            this.txtToMigrateFromDatabaseName.Size = new System.Drawing.Size(100, 20);
            this.txtToMigrateFromDatabaseName.TabIndex = 8;
            this.txtToMigrateFromDatabaseName.Text = "gringlobal";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(270, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Migrate from db name:";
            // 
            // chkScriptDropMigrationTables
            // 
            this.chkScriptDropMigrationTables.AutoSize = true;
            this.chkScriptDropMigrationTables.Checked = true;
            this.chkScriptDropMigrationTables.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScriptDropMigrationTables.Location = new System.Drawing.Point(126, 57);
            this.chkScriptDropMigrationTables.Name = "chkScriptDropMigrationTables";
            this.chkScriptDropMigrationTables.Size = new System.Drawing.Size(130, 17);
            this.chkScriptDropMigrationTables.TabIndex = 6;
            this.chkScriptDropMigrationTables.Text = "Drop Migration Tables";
            this.chkScriptDropMigrationTables.UseVisualStyleBackColor = true;
            // 
            // chkScriptMigrationInserts
            // 
            this.chkScriptMigrationInserts.AutoSize = true;
            this.chkScriptMigrationInserts.Checked = true;
            this.chkScriptMigrationInserts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScriptMigrationInserts.Location = new System.Drawing.Point(126, 39);
            this.chkScriptMigrationInserts.Name = "chkScriptMigrationInserts";
            this.chkScriptMigrationInserts.Size = new System.Drawing.Size(103, 17);
            this.chkScriptMigrationInserts.TabIndex = 4;
            this.chkScriptMigrationInserts.Text = "Migration Inserts";
            this.chkScriptMigrationInserts.UseVisualStyleBackColor = true;
            // 
            // chkScriptCreateMigrationTables
            // 
            this.chkScriptCreateMigrationTables.AutoSize = true;
            this.chkScriptCreateMigrationTables.Checked = true;
            this.chkScriptCreateMigrationTables.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScriptCreateMigrationTables.Location = new System.Drawing.Point(126, 20);
            this.chkScriptCreateMigrationTables.Name = "chkScriptCreateMigrationTables";
            this.chkScriptCreateMigrationTables.Size = new System.Drawing.Size(138, 17);
            this.chkScriptCreateMigrationTables.TabIndex = 3;
            this.chkScriptCreateMigrationTables.Text = "Create Migration Tables";
            this.chkScriptCreateMigrationTables.UseVisualStyleBackColor = true;
            // 
            // chkScriptCreateConstraints
            // 
            this.chkScriptCreateConstraints.AutoSize = true;
            this.chkScriptCreateConstraints.Checked = true;
            this.chkScriptCreateConstraints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScriptCreateConstraints.Location = new System.Drawing.Point(7, 58);
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
            this.chkScriptCreateIndexes.Location = new System.Drawing.Point(7, 39);
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
            this.btnPopupTableList.Location = new System.Drawing.Point(419, 115);
            this.btnPopupTableList.Name = "btnPopupTableList";
            this.btnPopupTableList.Size = new System.Drawing.Size(118, 23);
            this.btnPopupTableList.TabIndex = 39;
            this.btnPopupTableList.Text = "Popup Table List";
            this.btnPopupTableList.UseVisualStyleBackColor = true;
            this.btnPopupTableList.Click += new System.EventHandler(this.btnPopupTableList_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(196, 118);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 40;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmCopier2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 530);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnPopupTableList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbTo);
            this.Controls.Add(this.status);
            this.Controls.Add(this.chkSelectAllTables);
            this.Controls.Add(this.btnListTables);
            this.Controls.Add(this.lvTableList);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCopier2";
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbActionWriteSchema.ResumeLayout(false);
            this.gbActionWriteSchema.PerformLayout();
            this.gbActionWriteData.ResumeLayout(false);
            this.gbActionWriteData.PerformLayout();
            this.gbActionSearch.ResumeLayout(false);
            this.gbActionSearch.PerformLayout();
            this.gbActionScript.ResumeLayout(false);
            this.gbActionScript.PerformLayout();
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
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox gbActionScript;
		private System.Windows.Forms.RadioButton rbActionWriteSchema;
		private System.Windows.Forms.RadioButton rbActionWriteData;
		private System.Windows.Forms.RadioButton rbActionScript;
		private System.Windows.Forms.CheckBox chkScriptMigrationInserts;
		private System.Windows.Forms.CheckBox chkScriptCreateMigrationTables;
		private System.Windows.Forms.CheckBox chkScriptCreateConstraints;
		private System.Windows.Forms.CheckBox chkScriptCreateIndexes;
		private System.Windows.Forms.CheckBox chkScriptCreateTables;
		private System.Windows.Forms.CheckBox chkScriptDropMigrationTables;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ToolStripProgressBar statusProgress;
		private System.Windows.Forms.ToolStripStatusLabel statusText;
		private System.Windows.Forms.GroupBox gbActionWriteData;
		private System.Windows.Forms.Button btnActionWriteDataGo;
		private System.Windows.Forms.GroupBox gbActionWriteSchema;
		private System.Windows.Forms.Button btnActionWriteSchemaGo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtToMigrateToDatabaseName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtToMigrateFromDatabaseName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnActionScriptGo;
		private System.Windows.Forms.CheckBox chkWriteSchemaCreateForeignConstraints;
		private System.Windows.Forms.CheckBox chkWriteSchemaCreateIndexes;
		private System.Windows.Forms.CheckBox chkWriteSchemaCreateTables;
		private System.Windows.Forms.CheckBox chkWriteSchemaCreateSelfReferentialConstraints;
		private System.Windows.Forms.CheckBox chkScriptUseMyISAMDuringMigration;
		private System.Windows.Forms.RadioButton rbActionGenerateSearch;
		private System.Windows.Forms.GroupBox gbActionSearch;
		private System.Windows.Forms.Button btnActionSearchGo;
		private System.Windows.Forms.Button btnPopupTableList;
		private System.Windows.Forms.RadioButton rbActionWriteMappingData;
		private System.Windows.Forms.RadioButton rbActionWriteDatabaseData;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cboActionGenerateSearch;
        private System.Windows.Forms.RadioButton rbFromDatabasePostgreSQL;
        private System.Windows.Forms.RadioButton rbToDatabasePostgreSQL;
        private System.Windows.Forms.Button button1;
	}
}