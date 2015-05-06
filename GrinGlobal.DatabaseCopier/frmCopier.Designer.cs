namespace GrinGlobal.DatabaseCopier {
	partial class frmCopier {
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
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.lbProgress = new System.Windows.Forms.ListBox();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.tc = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.txtFromDatabaseOwner = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.btnPreviewList = new System.Windows.Forms.Button();
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnPreviewFrom = new System.Windows.Forms.Button();
			this.chkFromList = new System.Windows.Forms.CheckBox();
			this.btnListTables = new System.Windows.Forms.Button();
			this.lvFromTables = new System.Windows.Forms.ListView();
			this.chkCompressOnCopy = new System.Windows.Forms.CheckBox();
			this.btnFolderTo = new System.Windows.Forms.Button();
			this.btnCopyDataFrom = new System.Windows.Forms.Button();
			this.btnCopySchemaFrom = new System.Windows.Forms.Button();
			this.gbFrom = new System.Windows.Forms.GroupBox();
			this.cboConnectionStringFrom = new System.Windows.Forms.ComboBox();
			this.btnCreateFromScratch = new System.Windows.Forms.Button();
			this.rbFromOracle = new System.Windows.Forms.RadioButton();
			this.rbFromSqlServer = new System.Windows.Forms.RadioButton();
			this.rbFromMySql = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtDestinationFolder = new System.Windows.Forms.TextBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.btnCopySelfConstraints = new System.Windows.Forms.Button();
			this.btnCopyIndexesAndConstraintsToDatabase = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.btnPreviewTo = new System.Windows.Forms.Button();
			this.chkToList = new System.Windows.Forms.CheckBox();
			this.lvTablesTo = new System.Windows.Forms.ListView();
			this.btnRefreshListTo = new System.Windows.Forms.Button();
			this.btnCopyDataTo = new System.Windows.Forms.Button();
			this.btnCopySchemaTo = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkCaseSensitiveTo = new System.Windows.Forms.CheckBox();
			this.rbToOracle = new System.Windows.Forms.RadioButton();
			this.rbToSqlServer = new System.Windows.Forms.RadioButton();
			this.rbToMySql = new System.Windows.Forms.RadioButton();
			this.txtConnectionStringTo = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtSourceFolder = new System.Windows.Forms.TextBox();
			this.btnFolderFrom = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.ddlImportToTable = new System.Windows.Forms.ComboBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnImportToViewTables = new System.Windows.Forms.Button();
			this.rbImportOracle = new System.Windows.Forms.RadioButton();
			this.rbImportSqlServer = new System.Windows.Forms.RadioButton();
			this.rbImportMySql = new System.Windows.Forms.RadioButton();
			this.txtImportToConnectionString = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.btnImportData = new System.Windows.Forms.Button();
			this.dgvImportTo = new System.Windows.Forms.DataGridView();
			this.btnLoadImport = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.txtImportFromFile = new System.Windows.Forms.TextBox();
			this.btnFileImport = new System.Windows.Forms.Button();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.btnCopyFromDBOracleProductionWebService = new System.Windows.Forms.Button();
			this.chkFromCSVCreateMapTables = new System.Windows.Forms.CheckBox();
			this.btnFromCSVPreviewSelfConstraints = new System.Windows.Forms.Button();
			this.label12 = new System.Windows.Forms.Label();
			this.txtFromCSVToDBName = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.txtFromCSVFromDBName = new System.Windows.Forms.TextBox();
			this.btnFromCSVMigrationInserts = new System.Windows.Forms.Button();
			this.btnFromCsvPreviewSchema = new System.Windows.Forms.Button();
			this.btnFromCsvCopySchemaToFile = new System.Windows.Forms.Button();
			this.chkFromCSVSelectAll = new System.Windows.Forms.CheckBox();
			this.lvFromCsvTableList = new System.Windows.Forms.ListView();
			this.btnFromCsvFolderPrompt = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.txtFromCsvToFolder = new System.Windows.Forms.TextBox();
			this.btnFromCSVListTables = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.txtCSVFromFile = new System.Windows.Forms.TextBox();
			this.btnCSVFromOpen = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.lblStatus = new System.Windows.Forms.Label();
			this.lblTotalTime = new System.Windows.Forms.Label();
			this.lblTotalProgress = new System.Windows.Forms.Label();
			this.btnFromCSV = new System.Windows.Forms.Button();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.gbSourceIsDatabase = new System.Windows.Forms.GroupBox();
			this.cboSourceConnectionString = new System.Windows.Forms.ComboBox();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.label13 = new System.Windows.Forms.Label();
			this.gbSourceIsFile = new System.Windows.Forms.GroupBox();
			this.txtSourceFileName = new System.Windows.Forms.TextBox();
			this.btnSourceFilenamePrompt = new System.Windows.Forms.Button();
			this.rbSourceIsDatabase = new System.Windows.Forms.RadioButton();
			this.rbSourceIsCSV = new System.Windows.Forms.RadioButton();
			this.rbSourceIsFolder = new System.Windows.Forms.RadioButton();
			this.txtSourceDatabaseName = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.chkSelectAllTables = new System.Windows.Forms.CheckBox();
			this.btnFillTableList = new System.Windows.Forms.Button();
			this.lvTableList = new System.Windows.Forms.ListView();
			this.tc.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.gbFrom.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvImportTo)).BeginInit();
			this.tabPage4.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.gbSourceIsDatabase.SuspendLayout();
			this.gbSourceIsFile.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbProgress
			// 
			this.lbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbProgress.FormattingEnabled = true;
			this.lbProgress.Location = new System.Drawing.Point(12, 382);
			this.lbProgress.Name = "lbProgress";
			this.lbProgress.Size = new System.Drawing.Size(700, 95);
			this.lbProgress.TabIndex = 2;
			this.lbProgress.DoubleClick += new System.EventHandler(this.lbProgress_DoubleClick);
			// 
			// tc
			// 
			this.tc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tc.Controls.Add(this.tabPage1);
			this.tc.Controls.Add(this.tabPage2);
			this.tc.Controls.Add(this.tabPage3);
			this.tc.Controls.Add(this.tabPage4);
			this.tc.Controls.Add(this.tabPage5);
			this.tc.Location = new System.Drawing.Point(12, 11);
			this.tc.Name = "tc";
			this.tc.SelectedIndex = 0;
			this.tc.Size = new System.Drawing.Size(700, 365);
			this.tc.TabIndex = 5;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.txtFromDatabaseOwner);
			this.tabPage1.Controls.Add(this.label8);
			this.tabPage1.Controls.Add(this.btnPreviewList);
			this.tabPage1.Controls.Add(this.btnSelect);
			this.tabPage1.Controls.Add(this.btnPreviewFrom);
			this.tabPage1.Controls.Add(this.chkFromList);
			this.tabPage1.Controls.Add(this.btnListTables);
			this.tabPage1.Controls.Add(this.lvFromTables);
			this.tabPage1.Controls.Add(this.chkCompressOnCopy);
			this.tabPage1.Controls.Add(this.btnFolderTo);
			this.tabPage1.Controls.Add(this.btnCopyDataFrom);
			this.tabPage1.Controls.Add(this.btnCopySchemaFrom);
			this.tabPage1.Controls.Add(this.gbFrom);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.txtDestinationFolder);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(692, 339);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Copy From Database";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// txtFromDatabaseOwner
			// 
			this.txtFromDatabaseOwner.Location = new System.Drawing.Point(304, 107);
			this.txtFromDatabaseOwner.Name = "txtFromDatabaseOwner";
			this.txtFromDatabaseOwner.Size = new System.Drawing.Size(126, 20);
			this.txtFromDatabaseOwner.TabIndex = 32;
			this.txtFromDatabaseOwner.Text = "PROD";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(107, 109);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(191, 13);
			this.label8.TabIndex = 31;
			this.label8.Text = "Database name or owner (Oracle only):";
			// 
			// btnPreviewList
			// 
			this.btnPreviewList.Location = new System.Drawing.Point(596, 105);
			this.btnPreviewList.Name = "btnPreviewList";
			this.btnPreviewList.Size = new System.Drawing.Size(75, 23);
			this.btnPreviewList.TabIndex = 30;
			this.btnPreviewList.Text = "Preview List";
			this.btnPreviewList.UseVisualStyleBackColor = true;
			this.btnPreviewList.Click += new System.EventHandler(this.btnPreviewList_Click);
			// 
			// btnSelect
			// 
			this.btnSelect.Location = new System.Drawing.Point(253, 304);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(75, 23);
			this.btnSelect.TabIndex = 29;
			this.btnSelect.Text = "Select";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// btnPreviewFrom
			// 
			this.btnPreviewFrom.Enabled = false;
			this.btnPreviewFrom.Location = new System.Drawing.Point(143, 304);
			this.btnPreviewFrom.Name = "btnPreviewFrom";
			this.btnPreviewFrom.Size = new System.Drawing.Size(104, 23);
			this.btnPreviewFrom.TabIndex = 28;
			this.btnPreviewFrom.Text = "Preview Schema";
			this.btnPreviewFrom.UseVisualStyleBackColor = true;
			this.btnPreviewFrom.Click += new System.EventHandler(this.btnPreviewFrom_Click);
			// 
			// chkFromList
			// 
			this.chkFromList.AutoSize = true;
			this.chkFromList.Location = new System.Drawing.Point(9, 109);
			this.chkFromList.Name = "chkFromList";
			this.chkFromList.Size = new System.Drawing.Size(70, 17);
			this.chkFromList.TabIndex = 27;
			this.chkFromList.Text = "Select All";
			this.chkFromList.UseVisualStyleBackColor = true;
			this.chkFromList.CheckedChanged += new System.EventHandler(this.chkFromList_CheckedChanged);
			// 
			// btnListTables
			// 
			this.btnListTables.Location = new System.Drawing.Point(495, 105);
			this.btnListTables.Name = "btnListTables";
			this.btnListTables.Size = new System.Drawing.Size(75, 23);
			this.btnListTables.TabIndex = 26;
			this.btnListTables.Text = "List Tables";
			this.btnListTables.UseVisualStyleBackColor = true;
			this.btnListTables.Click += new System.EventHandler(this.btnListTables_Click);
			// 
			// lvFromTables
			// 
			this.lvFromTables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvFromTables.CheckBoxes = true;
			this.lvFromTables.Location = new System.Drawing.Point(9, 136);
			this.lvFromTables.Name = "lvFromTables";
			this.lvFromTables.Size = new System.Drawing.Size(669, 113);
			this.lvFromTables.TabIndex = 25;
			this.lvFromTables.UseCompatibleStateImageBehavior = false;
			this.lvFromTables.View = System.Windows.Forms.View.List;
			// 
			// chkCompressOnCopy
			// 
			this.chkCompressOnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkCompressOnCopy.AutoSize = true;
			this.chkCompressOnCopy.Enabled = false;
			this.chkCompressOnCopy.Location = new System.Drawing.Point(451, 308);
			this.chkCompressOnCopy.Name = "chkCompressOnCopy";
			this.chkCompressOnCopy.Size = new System.Drawing.Size(98, 17);
			this.chkCompressOnCopy.TabIndex = 24;
			this.chkCompressOnCopy.Text = "Compress Data";
			this.chkCompressOnCopy.UseVisualStyleBackColor = true;
			// 
			// btnFolderTo
			// 
			this.btnFolderTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFolderTo.Location = new System.Drawing.Point(652, 266);
			this.btnFolderTo.Name = "btnFolderTo";
			this.btnFolderTo.Size = new System.Drawing.Size(26, 23);
			this.btnFolderTo.TabIndex = 14;
			this.btnFolderTo.Text = "...";
			this.btnFolderTo.UseVisualStyleBackColor = true;
			this.btnFolderTo.Click += new System.EventHandler(this.btnFolderTo_Click);
			// 
			// btnCopyDataFrom
			// 
			this.btnCopyDataFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCopyDataFrom.Enabled = false;
			this.btnCopyDataFrom.Location = new System.Drawing.Point(562, 304);
			this.btnCopyDataFrom.Name = "btnCopyDataFrom";
			this.btnCopyDataFrom.Size = new System.Drawing.Size(116, 23);
			this.btnCopyDataFrom.TabIndex = 13;
			this.btnCopyDataFrom.Text = "Copy &Data To Files";
			this.btnCopyDataFrom.UseVisualStyleBackColor = true;
			this.btnCopyDataFrom.Click += new System.EventHandler(this.btnCopyDataFrom_Click);
			// 
			// btnCopySchemaFrom
			// 
			this.btnCopySchemaFrom.Enabled = false;
			this.btnCopySchemaFrom.Location = new System.Drawing.Point(9, 304);
			this.btnCopySchemaFrom.Name = "btnCopySchemaFrom";
			this.btnCopySchemaFrom.Size = new System.Drawing.Size(128, 23);
			this.btnCopySchemaFrom.TabIndex = 12;
			this.btnCopySchemaFrom.Text = "Copy &Schema To File";
			this.btnCopySchemaFrom.UseVisualStyleBackColor = true;
			this.btnCopySchemaFrom.Click += new System.EventHandler(this.btnCopySchemaFrom_Click);
			// 
			// gbFrom
			// 
			this.gbFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbFrom.Controls.Add(this.cboConnectionStringFrom);
			this.gbFrom.Controls.Add(this.btnCreateFromScratch);
			this.gbFrom.Controls.Add(this.rbFromOracle);
			this.gbFrom.Controls.Add(this.rbFromSqlServer);
			this.gbFrom.Controls.Add(this.rbFromMySql);
			this.gbFrom.Controls.Add(this.label2);
			this.gbFrom.Location = new System.Drawing.Point(9, 13);
			this.gbFrom.Name = "gbFrom";
			this.gbFrom.Size = new System.Drawing.Size(669, 86);
			this.gbFrom.TabIndex = 11;
			this.gbFrom.TabStop = false;
			this.gbFrom.Text = "From Database";
			// 
			// cboConnectionStringFrom
			// 
			this.cboConnectionStringFrom.FormattingEnabled = true;
			this.cboConnectionStringFrom.Items.AddRange(new object[] {
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
			this.cboConnectionStringFrom.Location = new System.Drawing.Point(6, 54);
			this.cboConnectionStringFrom.Name = "cboConnectionStringFrom";
			this.cboConnectionStringFrom.Size = new System.Drawing.Size(656, 21);
			this.cboConnectionStringFrom.TabIndex = 18;
			// 
			// btnCreateFromScratch
			// 
			this.btnCreateFromScratch.Location = new System.Drawing.Point(516, 14);
			this.btnCreateFromScratch.Name = "btnCreateFromScratch";
			this.btnCreateFromScratch.Size = new System.Drawing.Size(146, 28);
			this.btnCreateFromScratch.TabIndex = 17;
			this.btnCreateFromScratch.Text = "Test Create From Scratch";
			this.btnCreateFromScratch.UseVisualStyleBackColor = true;
			this.btnCreateFromScratch.Click += new System.EventHandler(this.btnCreateFromScratch_Click);
			// 
			// rbFromOracle
			// 
			this.rbFromOracle.AutoSize = true;
			this.rbFromOracle.Location = new System.Drawing.Point(200, 20);
			this.rbFromOracle.Name = "rbFromOracle";
			this.rbFromOracle.Size = new System.Drawing.Size(56, 17);
			this.rbFromOracle.TabIndex = 16;
			this.rbFromOracle.Text = "Oracle";
			this.rbFromOracle.UseVisualStyleBackColor = true;
			// 
			// rbFromSqlServer
			// 
			this.rbFromSqlServer.AutoSize = true;
			this.rbFromSqlServer.Location = new System.Drawing.Point(98, 20);
			this.rbFromSqlServer.Name = "rbFromSqlServer";
			this.rbFromSqlServer.Size = new System.Drawing.Size(74, 17);
			this.rbFromSqlServer.TabIndex = 15;
			this.rbFromSqlServer.Text = "Sql Server";
			this.rbFromSqlServer.UseVisualStyleBackColor = true;
			// 
			// rbFromMySql
			// 
			this.rbFromMySql.AutoSize = true;
			this.rbFromMySql.Checked = true;
			this.rbFromMySql.Location = new System.Drawing.Point(9, 20);
			this.rbFromMySql.Name = "rbFromMySql";
			this.rbFromMySql.Size = new System.Drawing.Size(54, 17);
			this.rbFromMySql.TabIndex = 14;
			this.rbFromMySql.TabStop = true;
			this.rbFromMySql.Text = "MySql";
			this.rbFromMySql.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 38);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(91, 13);
			this.label2.TabIndex = 11;
			this.label2.Text = "Connection String";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(15, 252);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "To Folder:";
			// 
			// txtDestinationFolder
			// 
			this.txtDestinationFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtDestinationFolder.Location = new System.Drawing.Point(18, 268);
			this.txtDestinationFolder.Name = "txtDestinationFolder";
			this.txtDestinationFolder.Size = new System.Drawing.Size(628, 20);
			this.txtDestinationFolder.TabIndex = 5;
			this.txtDestinationFolder.Text = "D:\\test_db_creator";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.btnCopySelfConstraints);
			this.tabPage2.Controls.Add(this.btnCopyIndexesAndConstraintsToDatabase);
			this.tabPage2.Controls.Add(this.button1);
			this.tabPage2.Controls.Add(this.btnPreviewTo);
			this.tabPage2.Controls.Add(this.chkToList);
			this.tabPage2.Controls.Add(this.lvTablesTo);
			this.tabPage2.Controls.Add(this.btnRefreshListTo);
			this.tabPage2.Controls.Add(this.btnCopyDataTo);
			this.tabPage2.Controls.Add(this.btnCopySchemaTo);
			this.tabPage2.Controls.Add(this.groupBox1);
			this.tabPage2.Controls.Add(this.txtSourceFolder);
			this.tabPage2.Controls.Add(this.btnFolderFrom);
			this.tabPage2.Controls.Add(this.label6);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(692, 339);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Copy To Database";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// btnCopySelfConstraints
			// 
			this.btnCopySelfConstraints.Location = new System.Drawing.Point(564, 310);
			this.btnCopySelfConstraints.Name = "btnCopySelfConstraints";
			this.btnCopySelfConstraints.Size = new System.Drawing.Size(120, 23);
			this.btnCopySelfConstraints.TabIndex = 29;
			this.btnCopySelfConstraints.Text = "Copy Self &Constraints";
			this.btnCopySelfConstraints.UseVisualStyleBackColor = true;
			this.btnCopySelfConstraints.Click += new System.EventHandler(this.btnCopySelfConstraints_Click);
			// 
			// btnCopyIndexesAndConstraintsToDatabase
			// 
			this.btnCopyIndexesAndConstraintsToDatabase.Location = new System.Drawing.Point(270, 310);
			this.btnCopyIndexesAndConstraintsToDatabase.Name = "btnCopyIndexesAndConstraintsToDatabase";
			this.btnCopyIndexesAndConstraintsToDatabase.Size = new System.Drawing.Size(172, 23);
			this.btnCopyIndexesAndConstraintsToDatabase.TabIndex = 28;
			this.btnCopyIndexesAndConstraintsToDatabase.Text = "Copy &Indexes && Most Constraints";
			this.btnCopyIndexesAndConstraintsToDatabase.UseVisualStyleBackColor = true;
			this.btnCopyIndexesAndConstraintsToDatabase.Click += new System.EventHandler(this.btnCopyIndexesAndConstraintsToDatabase_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(505, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 27;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Visible = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// btnPreviewTo
			// 
			this.btnPreviewTo.Enabled = false;
			this.btnPreviewTo.Location = new System.Drawing.Point(9, 310);
			this.btnPreviewTo.Name = "btnPreviewTo";
			this.btnPreviewTo.Size = new System.Drawing.Size(97, 23);
			this.btnPreviewTo.TabIndex = 26;
			this.btnPreviewTo.Text = "&Preview Schema";
			this.btnPreviewTo.UseVisualStyleBackColor = true;
			this.btnPreviewTo.Click += new System.EventHandler(this.btnPreviewTo_Click);
			// 
			// chkToList
			// 
			this.chkToList.AutoSize = true;
			this.chkToList.Location = new System.Drawing.Point(9, 66);
			this.chkToList.Name = "chkToList";
			this.chkToList.Size = new System.Drawing.Size(70, 17);
			this.chkToList.TabIndex = 25;
			this.chkToList.Text = "Select All";
			this.chkToList.UseVisualStyleBackColor = true;
			this.chkToList.CheckedChanged += new System.EventHandler(this.chkToList_CheckedChanged);
			// 
			// lvTablesTo
			// 
			this.lvTablesTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvTablesTo.CheckBoxes = true;
			this.lvTablesTo.Location = new System.Drawing.Point(9, 85);
			this.lvTablesTo.Name = "lvTablesTo";
			this.lvTablesTo.Size = new System.Drawing.Size(674, 108);
			this.lvTablesTo.TabIndex = 24;
			this.lvTablesTo.UseCompatibleStateImageBehavior = false;
			this.lvTablesTo.View = System.Windows.Forms.View.List;
			// 
			// btnRefreshListTo
			// 
			this.btnRefreshListTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefreshListTo.Location = new System.Drawing.Point(594, 57);
			this.btnRefreshListTo.Name = "btnRefreshListTo";
			this.btnRefreshListTo.Size = new System.Drawing.Size(87, 23);
			this.btnRefreshListTo.TabIndex = 23;
			this.btnRefreshListTo.Text = "Refresh List";
			this.btnRefreshListTo.UseVisualStyleBackColor = true;
			this.btnRefreshListTo.Click += new System.EventHandler(this.btnRefreshListTo_Click);
			// 
			// btnCopyDataTo
			// 
			this.btnCopyDataTo.Location = new System.Drawing.Point(447, 310);
			this.btnCopyDataTo.Name = "btnCopyDataTo";
			this.btnCopyDataTo.Size = new System.Drawing.Size(112, 23);
			this.btnCopyDataTo.TabIndex = 22;
			this.btnCopyDataTo.Text = "Copy &Data To DB";
			this.btnCopyDataTo.UseVisualStyleBackColor = true;
			this.btnCopyDataTo.Click += new System.EventHandler(this.btnCopyDataTo_Click);
			// 
			// btnCopySchemaTo
			// 
			this.btnCopySchemaTo.Location = new System.Drawing.Point(112, 310);
			this.btnCopySchemaTo.Name = "btnCopySchemaTo";
			this.btnCopySchemaTo.Size = new System.Drawing.Size(152, 23);
			this.btnCopySchemaTo.TabIndex = 21;
			this.btnCopySchemaTo.Text = "Copy Table &Shema To DB";
			this.btnCopySchemaTo.UseVisualStyleBackColor = true;
			this.btnCopySchemaTo.Click += new System.EventHandler(this.btnCopySchemaTo_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.chkCaseSensitiveTo);
			this.groupBox1.Controls.Add(this.rbToOracle);
			this.groupBox1.Controls.Add(this.rbToSqlServer);
			this.groupBox1.Controls.Add(this.rbToMySql);
			this.groupBox1.Controls.Add(this.txtConnectionStringTo);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Location = new System.Drawing.Point(9, 199);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(674, 105);
			this.groupBox1.TabIndex = 20;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "To Database";
			// 
			// chkCaseSensitiveTo
			// 
			this.chkCaseSensitiveTo.AutoSize = true;
			this.chkCaseSensitiveTo.Location = new System.Drawing.Point(435, 19);
			this.chkCaseSensitiveTo.Name = "chkCaseSensitiveTo";
			this.chkCaseSensitiveTo.Size = new System.Drawing.Size(161, 17);
			this.chkCaseSensitiveTo.TabIndex = 17;
			this.chkCaseSensitiveTo.Text = "Use Case-Sensitive Collation";
			this.chkCaseSensitiveTo.UseVisualStyleBackColor = true;
			// 
			// rbToOracle
			// 
			this.rbToOracle.AutoSize = true;
			this.rbToOracle.Location = new System.Drawing.Point(200, 20);
			this.rbToOracle.Name = "rbToOracle";
			this.rbToOracle.Size = new System.Drawing.Size(56, 17);
			this.rbToOracle.TabIndex = 16;
			this.rbToOracle.Text = "Oracle";
			this.rbToOracle.UseVisualStyleBackColor = true;
			// 
			// rbToSqlServer
			// 
			this.rbToSqlServer.AutoSize = true;
			this.rbToSqlServer.Checked = true;
			this.rbToSqlServer.Location = new System.Drawing.Point(98, 20);
			this.rbToSqlServer.Name = "rbToSqlServer";
			this.rbToSqlServer.Size = new System.Drawing.Size(74, 17);
			this.rbToSqlServer.TabIndex = 15;
			this.rbToSqlServer.TabStop = true;
			this.rbToSqlServer.Text = "Sql Server";
			this.rbToSqlServer.UseVisualStyleBackColor = true;
			// 
			// rbToMySql
			// 
			this.rbToMySql.AutoSize = true;
			this.rbToMySql.Location = new System.Drawing.Point(9, 20);
			this.rbToMySql.Name = "rbToMySql";
			this.rbToMySql.Size = new System.Drawing.Size(54, 17);
			this.rbToMySql.TabIndex = 14;
			this.rbToMySql.Text = "MySql";
			this.rbToMySql.UseVisualStyleBackColor = true;
			// 
			// txtConnectionStringTo
			// 
			this.txtConnectionStringTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtConnectionStringTo.Location = new System.Drawing.Point(6, 68);
			this.txtConnectionStringTo.Name = "txtConnectionStringTo";
			this.txtConnectionStringTo.Size = new System.Drawing.Size(662, 20);
			this.txtConnectionStringTo.TabIndex = 12;
			this.txtConnectionStringTo.Text = "Data Source=localhost;Initial Catalog=prod;user id=sa;password=passw0rd!";
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 52);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(91, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Connection String";
			// 
			// txtSourceFolder
			// 
			this.txtSourceFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSourceFolder.Location = new System.Drawing.Point(9, 31);
			this.txtSourceFolder.Name = "txtSourceFolder";
			this.txtSourceFolder.Size = new System.Drawing.Size(640, 20);
			this.txtSourceFolder.TabIndex = 17;
			this.txtSourceFolder.Text = "D:\\test_db_creator";
			// 
			// btnFolderFrom
			// 
			this.btnFolderFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFolderFrom.Location = new System.Drawing.Point(655, 31);
			this.btnFolderFrom.Name = "btnFolderFrom";
			this.btnFolderFrom.Size = new System.Drawing.Size(28, 23);
			this.btnFolderFrom.TabIndex = 18;
			this.btnFolderFrom.Text = "...";
			this.btnFolderFrom.UseVisualStyleBackColor = true;
			this.btnFolderFrom.Click += new System.EventHandler(this.btnFolderFrom_Click);
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 15);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(65, 13);
			this.label6.TabIndex = 19;
			this.label6.Text = "From Folder:";
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.ddlImportToTable);
			this.tabPage3.Controls.Add(this.groupBox2);
			this.tabPage3.Controls.Add(this.btnImportData);
			this.tabPage3.Controls.Add(this.dgvImportTo);
			this.tabPage3.Controls.Add(this.btnLoadImport);
			this.tabPage3.Controls.Add(this.label7);
			this.tabPage3.Controls.Add(this.txtImportFromFile);
			this.tabPage3.Controls.Add(this.btnFileImport);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(692, 339);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Import Data From CSV";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// ddlImportToTable
			// 
			this.ddlImportToTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlImportToTable.FormattingEnabled = true;
			this.ddlImportToTable.Location = new System.Drawing.Point(320, 310);
			this.ddlImportToTable.Name = "ddlImportToTable";
			this.ddlImportToTable.Size = new System.Drawing.Size(196, 21);
			this.ddlImportToTable.TabIndex = 22;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.btnImportToViewTables);
			this.groupBox2.Controls.Add(this.rbImportOracle);
			this.groupBox2.Controls.Add(this.rbImportSqlServer);
			this.groupBox2.Controls.Add(this.rbImportMySql);
			this.groupBox2.Controls.Add(this.txtImportToConnectionString);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Location = new System.Drawing.Point(6, 199);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(601, 105);
			this.groupBox2.TabIndex = 21;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "To Database";
			// 
			// btnImportToViewTables
			// 
			this.btnImportToViewTables.Location = new System.Drawing.Point(516, 68);
			this.btnImportToViewTables.Name = "btnImportToViewTables";
			this.btnImportToViewTables.Size = new System.Drawing.Size(75, 23);
			this.btnImportToViewTables.TabIndex = 19;
			this.btnImportToViewTables.Text = "View Tables";
			this.btnImportToViewTables.UseVisualStyleBackColor = true;
			this.btnImportToViewTables.Click += new System.EventHandler(this.btnImportToViewTables_Click);
			// 
			// rbImportOracle
			// 
			this.rbImportOracle.AutoSize = true;
			this.rbImportOracle.Location = new System.Drawing.Point(200, 20);
			this.rbImportOracle.Name = "rbImportOracle";
			this.rbImportOracle.Size = new System.Drawing.Size(56, 17);
			this.rbImportOracle.TabIndex = 16;
			this.rbImportOracle.Text = "Oracle";
			this.rbImportOracle.UseVisualStyleBackColor = true;
			// 
			// rbImportSqlServer
			// 
			this.rbImportSqlServer.AutoSize = true;
			this.rbImportSqlServer.Checked = true;
			this.rbImportSqlServer.Location = new System.Drawing.Point(98, 20);
			this.rbImportSqlServer.Name = "rbImportSqlServer";
			this.rbImportSqlServer.Size = new System.Drawing.Size(74, 17);
			this.rbImportSqlServer.TabIndex = 15;
			this.rbImportSqlServer.TabStop = true;
			this.rbImportSqlServer.Text = "Sql Server";
			this.rbImportSqlServer.UseVisualStyleBackColor = true;
			// 
			// rbImportMySql
			// 
			this.rbImportMySql.AutoSize = true;
			this.rbImportMySql.Location = new System.Drawing.Point(9, 20);
			this.rbImportMySql.Name = "rbImportMySql";
			this.rbImportMySql.Size = new System.Drawing.Size(54, 17);
			this.rbImportMySql.TabIndex = 14;
			this.rbImportMySql.Text = "MySql";
			this.rbImportMySql.UseVisualStyleBackColor = true;
			// 
			// txtImportToConnectionString
			// 
			this.txtImportToConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtImportToConnectionString.Location = new System.Drawing.Point(6, 68);
			this.txtImportToConnectionString.Name = "txtImportToConnectionString";
			this.txtImportToConnectionString.Size = new System.Drawing.Size(505, 20);
			this.txtImportToConnectionString.TabIndex = 12;
			this.txtImportToConnectionString.Text = "Data Source=localhost;Initial Catalog=prod;user id=sa;password=passw0rd!";
			// 
			// label9
			// 
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(6, 52);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(91, 13);
			this.label9.TabIndex = 11;
			this.label9.Text = "Connection String";
			// 
			// btnImportData
			// 
			this.btnImportData.Location = new System.Drawing.Point(522, 310);
			this.btnImportData.Name = "btnImportData";
			this.btnImportData.Size = new System.Drawing.Size(75, 23);
			this.btnImportData.TabIndex = 5;
			this.btnImportData.Text = "Import Data";
			this.btnImportData.UseVisualStyleBackColor = true;
			this.btnImportData.Click += new System.EventHandler(this.btnImportData_Click);
			// 
			// dgvImportTo
			// 
			this.dgvImportTo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvImportTo.Location = new System.Drawing.Point(3, 91);
			this.dgvImportTo.Name = "dgvImportTo";
			this.dgvImportTo.Size = new System.Drawing.Size(604, 102);
			this.dgvImportTo.TabIndex = 4;
			// 
			// btnLoadImport
			// 
			this.btnLoadImport.Location = new System.Drawing.Point(532, 63);
			this.btnLoadImport.Name = "btnLoadImport";
			this.btnLoadImport.Size = new System.Drawing.Size(75, 23);
			this.btnLoadImport.TabIndex = 3;
			this.btnLoadImport.Text = "Load";
			this.btnLoadImport.UseVisualStyleBackColor = true;
			this.btnLoadImport.Click += new System.EventHandler(this.btnLoadImport_Click);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(7, 17);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(84, 13);
			this.label7.TabIndex = 2;
			this.label7.Text = "Import From File:";
			// 
			// txtImportFromFile
			// 
			this.txtImportFromFile.Location = new System.Drawing.Point(6, 36);
			this.txtImportFromFile.Name = "txtImportFromFile";
			this.txtImportFromFile.Size = new System.Drawing.Size(566, 20);
			this.txtImportFromFile.TabIndex = 1;
			// 
			// btnFileImport
			// 
			this.btnFileImport.Location = new System.Drawing.Point(578, 34);
			this.btnFileImport.Name = "btnFileImport";
			this.btnFileImport.Size = new System.Drawing.Size(30, 23);
			this.btnFileImport.TabIndex = 0;
			this.btnFileImport.Text = "...";
			this.btnFileImport.UseVisualStyleBackColor = true;
			this.btnFileImport.Click += new System.EventHandler(this.btnFileImport_Click);
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.btnFromCSV);
			this.tabPage4.Controls.Add(this.btnCopyFromDBOracleProductionWebService);
			this.tabPage4.Controls.Add(this.chkFromCSVCreateMapTables);
			this.tabPage4.Controls.Add(this.btnFromCSVPreviewSelfConstraints);
			this.tabPage4.Controls.Add(this.label12);
			this.tabPage4.Controls.Add(this.txtFromCSVToDBName);
			this.tabPage4.Controls.Add(this.label11);
			this.tabPage4.Controls.Add(this.label10);
			this.tabPage4.Controls.Add(this.txtFromCSVFromDBName);
			this.tabPage4.Controls.Add(this.btnFromCSVMigrationInserts);
			this.tabPage4.Controls.Add(this.btnFromCsvPreviewSchema);
			this.tabPage4.Controls.Add(this.btnFromCsvCopySchemaToFile);
			this.tabPage4.Controls.Add(this.chkFromCSVSelectAll);
			this.tabPage4.Controls.Add(this.lvFromCsvTableList);
			this.tabPage4.Controls.Add(this.btnFromCsvFolderPrompt);
			this.tabPage4.Controls.Add(this.label4);
			this.tabPage4.Controls.Add(this.txtFromCsvToFolder);
			this.tabPage4.Controls.Add(this.btnFromCSVListTables);
			this.tabPage4.Controls.Add(this.label3);
			this.tabPage4.Controls.Add(this.txtCSVFromFile);
			this.tabPage4.Controls.Add(this.btnCSVFromOpen);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(692, 339);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Copy From CSV";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// btnCopyFromDBOracleProductionWebService
			// 
			this.btnCopyFromDBOracleProductionWebService.Location = new System.Drawing.Point(525, 281);
			this.btnCopyFromDBOracleProductionWebService.Name = "btnCopyFromDBOracleProductionWebService";
			this.btnCopyFromDBOracleProductionWebService.Size = new System.Drawing.Size(164, 23);
			this.btnCopyFromDBOracleProductionWebService.TabIndex = 43;
			this.btnCopyFromDBOracleProductionWebService.Text = "Copy From Oracle Production";
			this.btnCopyFromDBOracleProductionWebService.UseVisualStyleBackColor = true;
			this.btnCopyFromDBOracleProductionWebService.Click += new System.EventHandler(this.btnCopyFromDBOracleProductionWebService_Click);
			// 
			// chkFromCSVCreateMapTables
			// 
			this.chkFromCSVCreateMapTables.AutoSize = true;
			this.chkFromCSVCreateMapTables.Checked = true;
			this.chkFromCSVCreateMapTables.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFromCSVCreateMapTables.Location = new System.Drawing.Point(12, 317);
			this.chkFromCSVCreateMapTables.Name = "chkFromCSVCreateMapTables";
			this.chkFromCSVCreateMapTables.Size = new System.Drawing.Size(116, 17);
			this.chkFromCSVCreateMapTables.TabIndex = 42;
			this.chkFromCSVCreateMapTables.Text = "Create Map Tables";
			this.chkFromCSVCreateMapTables.UseVisualStyleBackColor = true;
			// 
			// btnFromCSVPreviewSelfConstraints
			// 
			this.btnFromCSVPreviewSelfConstraints.Enabled = false;
			this.btnFromCSVPreviewSelfConstraints.Location = new System.Drawing.Point(507, 312);
			this.btnFromCSVPreviewSelfConstraints.Name = "btnFromCSVPreviewSelfConstraints";
			this.btnFromCSVPreviewSelfConstraints.Size = new System.Drawing.Size(127, 23);
			this.btnFromCSVPreviewSelfConstraints.TabIndex = 41;
			this.btnFromCSVPreviewSelfConstraints.Text = "Preview Constraints";
			this.btnFromCSVPreviewSelfConstraints.UseVisualStyleBackColor = true;
			this.btnFromCSVPreviewSelfConstraints.Click += new System.EventHandler(this.btnFromCSVPreviewSelfConstraints_Click);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(18, 296);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(261, 13);
			this.label12.TabIndex = 40;
			this.label12.Text = "(Uses connectionstring from Copy From Database tab)";
			// 
			// txtFromCSVToDBName
			// 
			this.txtFromCSVToDBName.Location = new System.Drawing.Point(425, 288);
			this.txtFromCSVToDBName.Name = "txtFromCSVToDBName";
			this.txtFromCSVToDBName.Size = new System.Drawing.Size(100, 20);
			this.txtFromCSVToDBName.TabIndex = 39;
			this.txtFromCSVToDBName.Text = "dev3";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(347, 291);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(72, 13);
			this.label11.TabIndex = 38;
			this.label11.Text = "To DB Name:";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(337, 265);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(82, 13);
			this.label10.TabIndex = 37;
			this.label10.Text = "From DB Name:";
			// 
			// txtFromCSVFromDBName
			// 
			this.txtFromCSVFromDBName.Location = new System.Drawing.Point(425, 262);
			this.txtFromCSVFromDBName.Name = "txtFromCSVFromDBName";
			this.txtFromCSVFromDBName.Size = new System.Drawing.Size(100, 20);
			this.txtFromCSVFromDBName.TabIndex = 36;
			this.txtFromCSVFromDBName.Text = "dev2";
			// 
			// btnFromCSVMigrationInserts
			// 
			this.btnFromCSVMigrationInserts.Enabled = false;
			this.btnFromCSVMigrationInserts.Location = new System.Drawing.Point(350, 313);
			this.btnFromCSVMigrationInserts.Name = "btnFromCSVMigrationInserts";
			this.btnFromCSVMigrationInserts.Size = new System.Drawing.Size(151, 23);
			this.btnFromCSVMigrationInserts.TabIndex = 35;
			this.btnFromCSVMigrationInserts.Text = "Preview Migration Inserts";
			this.btnFromCSVMigrationInserts.UseVisualStyleBackColor = true;
			this.btnFromCSVMigrationInserts.Click += new System.EventHandler(this.btnFromCSVMigrationInserts_Click);
			// 
			// btnFromCsvPreviewSchema
			// 
			this.btnFromCsvPreviewSchema.Enabled = false;
			this.btnFromCsvPreviewSchema.Location = new System.Drawing.Point(129, 313);
			this.btnFromCsvPreviewSchema.Name = "btnFromCsvPreviewSchema";
			this.btnFromCsvPreviewSchema.Size = new System.Drawing.Size(103, 23);
			this.btnFromCsvPreviewSchema.TabIndex = 34;
			this.btnFromCsvPreviewSchema.Text = "Preview Tables";
			this.btnFromCsvPreviewSchema.UseVisualStyleBackColor = true;
			this.btnFromCsvPreviewSchema.Click += new System.EventHandler(this.btnFromCsvPreviewSchema_Click);
			// 
			// btnFromCsvCopySchemaToFile
			// 
			this.btnFromCsvCopySchemaToFile.Enabled = false;
			this.btnFromCsvCopySchemaToFile.Location = new System.Drawing.Point(12, 265);
			this.btnFromCsvCopySchemaToFile.Name = "btnFromCsvCopySchemaToFile";
			this.btnFromCsvCopySchemaToFile.Size = new System.Drawing.Size(128, 23);
			this.btnFromCsvCopySchemaToFile.TabIndex = 33;
			this.btnFromCsvCopySchemaToFile.Text = "Copy &Schema To File";
			this.btnFromCsvCopySchemaToFile.UseVisualStyleBackColor = true;
			this.btnFromCsvCopySchemaToFile.Click += new System.EventHandler(this.btnFromCsvCopySchemaToFile_Click);
			// 
			// chkFromCSVSelectAll
			// 
			this.chkFromCSVSelectAll.AutoSize = true;
			this.chkFromCSVSelectAll.Location = new System.Drawing.Point(12, 79);
			this.chkFromCSVSelectAll.Name = "chkFromCSVSelectAll";
			this.chkFromCSVSelectAll.Size = new System.Drawing.Size(70, 17);
			this.chkFromCSVSelectAll.TabIndex = 32;
			this.chkFromCSVSelectAll.Text = "Select All";
			this.chkFromCSVSelectAll.UseVisualStyleBackColor = true;
			this.chkFromCSVSelectAll.CheckedChanged += new System.EventHandler(this.chkFromCSVSelectAll_CheckedChanged);
			// 
			// lvFromCsvTableList
			// 
			this.lvFromCsvTableList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvFromCsvTableList.CheckBoxes = true;
			this.lvFromCsvTableList.Location = new System.Drawing.Point(12, 106);
			this.lvFromCsvTableList.Name = "lvFromCsvTableList";
			this.lvFromCsvTableList.Size = new System.Drawing.Size(669, 113);
			this.lvFromCsvTableList.TabIndex = 31;
			this.lvFromCsvTableList.UseCompatibleStateImageBehavior = false;
			this.lvFromCsvTableList.View = System.Windows.Forms.View.List;
			// 
			// btnFromCsvFolderPrompt
			// 
			this.btnFromCsvFolderPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFromCsvFolderPrompt.Location = new System.Drawing.Point(655, 236);
			this.btnFromCsvFolderPrompt.Name = "btnFromCsvFolderPrompt";
			this.btnFromCsvFolderPrompt.Size = new System.Drawing.Size(26, 23);
			this.btnFromCsvFolderPrompt.TabIndex = 30;
			this.btnFromCsvFolderPrompt.Text = "...";
			this.btnFromCsvFolderPrompt.UseVisualStyleBackColor = true;
			this.btnFromCsvFolderPrompt.Click += new System.EventHandler(this.btnFromCsvFolderPrompt_Click);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(18, 222);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(55, 13);
			this.label4.TabIndex = 29;
			this.label4.Text = "To Folder:";
			// 
			// txtFromCsvToFolder
			// 
			this.txtFromCsvToFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFromCsvToFolder.Location = new System.Drawing.Point(21, 238);
			this.txtFromCsvToFolder.Name = "txtFromCsvToFolder";
			this.txtFromCsvToFolder.Size = new System.Drawing.Size(628, 20);
			this.txtFromCsvToFolder.TabIndex = 28;
			this.txtFromCsvToFolder.Text = "D:\\test_db_creator";
			// 
			// btnFromCSVListTables
			// 
			this.btnFromCSVListTables.Location = new System.Drawing.Point(600, 67);
			this.btnFromCSVListTables.Name = "btnFromCSVListTables";
			this.btnFromCSVListTables.Size = new System.Drawing.Size(75, 23);
			this.btnFromCSVListTables.TabIndex = 3;
			this.btnFromCSVListTables.Text = "List Tables";
			this.btnFromCSVListTables.UseVisualStyleBackColor = true;
			this.btnFromCSVListTables.Click += new System.EventHandler(this.btnFromCSVListTables_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(14, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(126, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Copy Schema From CSV:";
			// 
			// txtCSVFromFile
			// 
			this.txtCSVFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCSVFromFile.Location = new System.Drawing.Point(14, 29);
			this.txtCSVFromFile.Name = "txtCSVFromFile";
			this.txtCSVFromFile.Size = new System.Drawing.Size(626, 20);
			this.txtCSVFromFile.TabIndex = 1;
			this.txtCSVFromFile.Text = "D:\\projects\\GrinGlobal\\db_scripts\\new_db_schema.csv";
			// 
			// btnCSVFromOpen
			// 
			this.btnCSVFromOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCSVFromOpen.Location = new System.Drawing.Point(646, 27);
			this.btnCSVFromOpen.Name = "btnCSVFromOpen";
			this.btnCSVFromOpen.Size = new System.Drawing.Size(29, 23);
			this.btnCSVFromOpen.TabIndex = 0;
			this.btnCSVFromOpen.Text = "...";
			this.btnCSVFromOpen.UseVisualStyleBackColor = true;
			this.btnCSVFromOpen.Click += new System.EventHandler(this.btnCSVFromOpen_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// lblStatus
			// 
			this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(12, 490);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(10, 13);
			this.lblStatus.TabIndex = 6;
			this.lblStatus.Text = "-";
			// 
			// lblTotalTime
			// 
			this.lblTotalTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTotalTime.Location = new System.Drawing.Point(558, 490);
			this.lblTotalTime.Name = "lblTotalTime";
			this.lblTotalTime.Size = new System.Drawing.Size(155, 16);
			this.lblTotalTime.TabIndex = 7;
			this.lblTotalTime.Text = ":";
			this.lblTotalTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblTotalProgress
			// 
			this.lblTotalProgress.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lblTotalProgress.Location = new System.Drawing.Point(271, 490);
			this.lblTotalProgress.Name = "lblTotalProgress";
			this.lblTotalProgress.Size = new System.Drawing.Size(175, 23);
			this.lblTotalProgress.TabIndex = 8;
			this.lblTotalProgress.Text = "...";
			this.lblTotalProgress.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// btnFromCSV
			// 
			this.btnFromCSV.Enabled = false;
			this.btnFromCSV.Location = new System.Drawing.Point(238, 313);
			this.btnFromCSV.Name = "btnFromCSV";
			this.btnFromCSV.Size = new System.Drawing.Size(103, 23);
			this.btnFromCSV.TabIndex = 44;
			this.btnFromCSV.Text = "Preview Indexes";
			this.btnFromCSV.UseVisualStyleBackColor = true;
			this.btnFromCSV.Click += new System.EventHandler(this.button2_Click);
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add(this.chkSelectAllTables);
			this.tabPage5.Controls.Add(this.btnFillTableList);
			this.tabPage5.Controls.Add(this.lvTableList);
			this.tabPage5.Controls.Add(this.groupBox3);
			this.tabPage5.Location = new System.Drawing.Point(4, 22);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Size = new System.Drawing.Size(692, 339);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "New";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.rbSourceIsFolder);
			this.groupBox3.Controls.Add(this.rbSourceIsCSV);
			this.groupBox3.Controls.Add(this.rbSourceIsDatabase);
			this.groupBox3.Controls.Add(this.gbSourceIsDatabase);
			this.groupBox3.Location = new System.Drawing.Point(3, 3);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(669, 99);
			this.groupBox3.TabIndex = 12;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Load Schema / Data From:";
			// 
			// gbSourceIsDatabase
			// 
			this.gbSourceIsDatabase.Controls.Add(this.txtSourceDatabaseName);
			this.gbSourceIsDatabase.Controls.Add(this.gbSourceIsFile);
			this.gbSourceIsDatabase.Controls.Add(this.label14);
			this.gbSourceIsDatabase.Controls.Add(this.cboSourceConnectionString);
			this.gbSourceIsDatabase.Controls.Add(this.radioButton1);
			this.gbSourceIsDatabase.Controls.Add(this.radioButton2);
			this.gbSourceIsDatabase.Controls.Add(this.radioButton3);
			this.gbSourceIsDatabase.Controls.Add(this.label13);
			this.gbSourceIsDatabase.Location = new System.Drawing.Point(88, 19);
			this.gbSourceIsDatabase.Name = "gbSourceIsDatabase";
			this.gbSourceIsDatabase.Size = new System.Drawing.Size(581, 68);
			this.gbSourceIsDatabase.TabIndex = 19;
			this.gbSourceIsDatabase.TabStop = false;
			this.gbSourceIsDatabase.Text = "Database";
			// 
			// cboSourceConnectionString
			// 
			this.cboSourceConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cboSourceConnectionString.FormattingEnabled = true;
			this.cboSourceConnectionString.Items.AddRange(new object[] {
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
			this.cboSourceConnectionString.Location = new System.Drawing.Point(6, 42);
			this.cboSourceConnectionString.Name = "cboSourceConnectionString";
			this.cboSourceConnectionString.Size = new System.Drawing.Size(569, 21);
			this.cboSourceConnectionString.TabIndex = 23;
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Location = new System.Drawing.Point(144, 19);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(56, 17);
			this.radioButton1.TabIndex = 22;
			this.radioButton1.Text = "Oracle";
			this.radioButton1.UseVisualStyleBackColor = true;
			// 
			// radioButton2
			// 
			this.radioButton2.AutoSize = true;
			this.radioButton2.Location = new System.Drawing.Point(66, 19);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(74, 17);
			this.radioButton2.TabIndex = 21;
			this.radioButton2.Text = "Sql Server";
			this.radioButton2.UseVisualStyleBackColor = true;
			// 
			// radioButton3
			// 
			this.radioButton3.AutoSize = true;
			this.radioButton3.Checked = true;
			this.radioButton3.Location = new System.Drawing.Point(6, 19);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(54, 17);
			this.radioButton3.TabIndex = 20;
			this.radioButton3.TabStop = true;
			this.radioButton3.Text = "MySql";
			this.radioButton3.UseVisualStyleBackColor = true;
			// 
			// label13
			// 
			this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(484, 21);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(91, 13);
			this.label13.TabIndex = 19;
			this.label13.Text = "Connection String";
			// 
			// gbSourceIsFile
			// 
			this.gbSourceIsFile.Controls.Add(this.txtSourceFileName);
			this.gbSourceIsFile.Controls.Add(this.btnSourceFilenamePrompt);
			this.gbSourceIsFile.Location = new System.Drawing.Point(48, 1);
			this.gbSourceIsFile.Name = "gbSourceIsFile";
			this.gbSourceIsFile.Size = new System.Drawing.Size(575, 68);
			this.gbSourceIsFile.TabIndex = 20;
			this.gbSourceIsFile.TabStop = false;
			this.gbSourceIsFile.Text = "File:";
			// 
			// txtSourceFileName
			// 
			this.txtSourceFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSourceFileName.Location = new System.Drawing.Point(6, 19);
			this.txtSourceFileName.Name = "txtSourceFileName";
			this.txtSourceFileName.Size = new System.Drawing.Size(529, 20);
			this.txtSourceFileName.TabIndex = 19;
			this.txtSourceFileName.Text = "D:\\test_db_creator";
			// 
			// btnSourceFilenamePrompt
			// 
			this.btnSourceFilenamePrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSourceFilenamePrompt.Location = new System.Drawing.Point(541, 16);
			this.btnSourceFilenamePrompt.Name = "btnSourceFilenamePrompt";
			this.btnSourceFilenamePrompt.Size = new System.Drawing.Size(28, 23);
			this.btnSourceFilenamePrompt.TabIndex = 20;
			this.btnSourceFilenamePrompt.Text = "...";
			this.btnSourceFilenamePrompt.UseVisualStyleBackColor = true;
			// 
			// rbSourceIsDatabase
			// 
			this.rbSourceIsDatabase.AutoSize = true;
			this.rbSourceIsDatabase.Checked = true;
			this.rbSourceIsDatabase.Location = new System.Drawing.Point(7, 20);
			this.rbSourceIsDatabase.Name = "rbSourceIsDatabase";
			this.rbSourceIsDatabase.Size = new System.Drawing.Size(71, 17);
			this.rbSourceIsDatabase.TabIndex = 21;
			this.rbSourceIsDatabase.TabStop = true;
			this.rbSourceIsDatabase.Text = "Database";
			this.rbSourceIsDatabase.UseVisualStyleBackColor = true;
			this.rbSourceIsDatabase.CheckedChanged += new System.EventHandler(this.rbSourceIsDatabase_CheckedChanged);
			// 
			// rbSourceIsCSV
			// 
			this.rbSourceIsCSV.AutoSize = true;
			this.rbSourceIsCSV.Location = new System.Drawing.Point(7, 47);
			this.rbSourceIsCSV.Name = "rbSourceIsCSV";
			this.rbSourceIsCSV.Size = new System.Drawing.Size(65, 17);
			this.rbSourceIsCSV.TabIndex = 22;
			this.rbSourceIsCSV.Text = "CSV File";
			this.rbSourceIsCSV.UseVisualStyleBackColor = true;
			this.rbSourceIsCSV.CheckedChanged += new System.EventHandler(this.rbSourceIsCSV_CheckedChanged);
			// 
			// rbSourceIsFolder
			// 
			this.rbSourceIsFolder.AutoSize = true;
			this.rbSourceIsFolder.Location = new System.Drawing.Point(7, 70);
			this.rbSourceIsFolder.Name = "rbSourceIsFolder";
			this.rbSourceIsFolder.Size = new System.Drawing.Size(66, 17);
			this.rbSourceIsFolder.TabIndex = 23;
			this.rbSourceIsFolder.TabStop = true;
			this.rbSourceIsFolder.Text = "XML File";
			this.rbSourceIsFolder.UseVisualStyleBackColor = true;
			this.rbSourceIsFolder.CheckedChanged += new System.EventHandler(this.rbSourceIsFolder_CheckedChanged);
			// 
			// txtSourceDatabaseName
			// 
			this.txtSourceDatabaseName.Location = new System.Drawing.Point(345, 19);
			this.txtSourceDatabaseName.Name = "txtSourceDatabaseName";
			this.txtSourceDatabaseName.Size = new System.Drawing.Size(81, 20);
			this.txtSourceDatabaseName.TabIndex = 34;
			this.txtSourceDatabaseName.Text = "PROD";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(226, 13);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(113, 26);
			this.label14.TabIndex = 33;
			this.label14.Text = "Database name\r\nor owner (Oracle only):";
			// 
			// chkSelectAllTables
			// 
			this.chkSelectAllTables.AutoSize = true;
			this.chkSelectAllTables.Location = new System.Drawing.Point(10, 108);
			this.chkSelectAllTables.Name = "chkSelectAllTables";
			this.chkSelectAllTables.Size = new System.Drawing.Size(70, 17);
			this.chkSelectAllTables.TabIndex = 30;
			this.chkSelectAllTables.Text = "Select All";
			this.chkSelectAllTables.UseVisualStyleBackColor = true;
			// 
			// btnFillTableList
			// 
			this.btnFillTableList.Location = new System.Drawing.Point(604, 106);
			this.btnFillTableList.Name = "btnFillTableList";
			this.btnFillTableList.Size = new System.Drawing.Size(75, 23);
			this.btnFillTableList.TabIndex = 29;
			this.btnFillTableList.Text = "List Tables";
			this.btnFillTableList.UseVisualStyleBackColor = true;
			this.btnFillTableList.Click += new System.EventHandler(this.btnFillTableList_Click);
			// 
			// lvTableList
			// 
			this.lvTableList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvTableList.CheckBoxes = true;
			this.lvTableList.Location = new System.Drawing.Point(10, 135);
			this.lvTableList.Name = "lvTableList";
			this.lvTableList.Size = new System.Drawing.Size(669, 113);
			this.lvTableList.TabIndex = 28;
			this.lvTableList.UseCompatibleStateImageBehavior = false;
			this.lvTableList.View = System.Windows.Forms.View.List;
			// 
			// frmCopier
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(725, 515);
			this.Controls.Add(this.lblTotalProgress);
			this.Controls.Add(this.lblTotalTime);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.tc);
			this.Controls.Add(this.lbProgress);
			this.Name = "frmCopier";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Database Copier";
			this.Load += new System.EventHandler(this.frmCopier_Load);
			this.tc.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.gbFrom.ResumeLayout(false);
			this.gbFrom.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvImportTo)).EndInit();
			this.tabPage4.ResumeLayout(false);
			this.tabPage4.PerformLayout();
			this.tabPage5.ResumeLayout(false);
			this.tabPage5.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.gbSourceIsDatabase.ResumeLayout(false);
			this.gbSourceIsDatabase.PerformLayout();
			this.gbSourceIsFile.ResumeLayout(false);
			this.gbSourceIsFile.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.ListBox lbProgress;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.TabControl tc;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtDestinationFolder;
		private System.Windows.Forms.Button btnCopySchemaFrom;
		private System.Windows.Forms.GroupBox gbFrom;
		private System.Windows.Forms.RadioButton rbFromOracle;
		private System.Windows.Forms.RadioButton rbFromSqlServer;
		private System.Windows.Forms.RadioButton rbFromMySql;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnCopyDataFrom;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Button btnCopyDataTo;
		private System.Windows.Forms.Button btnCopySchemaTo;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbToOracle;
		private System.Windows.Forms.RadioButton rbToSqlServer;
		private System.Windows.Forms.RadioButton rbToMySql;
		private System.Windows.Forms.TextBox txtConnectionStringTo;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtSourceFolder;
		private System.Windows.Forms.Button btnFolderFrom;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnFolderTo;
		private System.Windows.Forms.CheckBox chkCompressOnCopy;
		private System.Windows.Forms.ListView lvFromTables;
		private System.Windows.Forms.CheckBox chkFromList;
		private System.Windows.Forms.Button btnListTables;
		private System.Windows.Forms.Button btnRefreshListTo;
		private System.Windows.Forms.ListView lvTablesTo;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton rbImportOracle;
		private System.Windows.Forms.RadioButton rbImportSqlServer;
		private System.Windows.Forms.RadioButton rbImportMySql;
		private System.Windows.Forms.TextBox txtImportToConnectionString;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button btnImportData;
		private System.Windows.Forms.DataGridView dgvImportTo;
		private System.Windows.Forms.Button btnLoadImport;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtImportFromFile;
		private System.Windows.Forms.Button btnFileImport;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ComboBox ddlImportToTable;
		private System.Windows.Forms.Button btnImportToViewTables;
		private System.Windows.Forms.CheckBox chkToList;
		private System.Windows.Forms.Button btnPreviewFrom;
		private System.Windows.Forms.Button btnPreviewTo;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox chkCaseSensitiveTo;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnCopyIndexesAndConstraintsToDatabase;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Label lblTotalTime;
		private System.Windows.Forms.Label lblTotalProgress;
		private System.Windows.Forms.Button btnCopySelfConstraints;
		private System.Windows.Forms.Button btnPreviewList;
		private System.Windows.Forms.Button btnCreateFromScratch;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.CheckBox chkFromCSVSelectAll;
		private System.Windows.Forms.ListView lvFromCsvTableList;
		private System.Windows.Forms.Button btnFromCsvFolderPrompt;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtFromCsvToFolder;
		private System.Windows.Forms.Button btnFromCSVListTables;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtCSVFromFile;
		private System.Windows.Forms.Button btnCSVFromOpen;
		private System.Windows.Forms.Button btnFromCsvPreviewSchema;
		private System.Windows.Forms.Button btnFromCsvCopySchemaToFile;
		private System.Windows.Forms.ComboBox cboConnectionStringFrom;
		private System.Windows.Forms.TextBox txtFromDatabaseOwner;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtFromCSVToDBName;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtFromCSVFromDBName;
		private System.Windows.Forms.Button btnFromCSVMigrationInserts;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Button btnFromCSVPreviewSelfConstraints;
		private System.Windows.Forms.CheckBox chkFromCSVCreateMapTables;
		private System.Windows.Forms.Button btnCopyFromDBOracleProductionWebService;
		private System.Windows.Forms.Button btnFromCSV;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton rbSourceIsFolder;
		private System.Windows.Forms.RadioButton rbSourceIsCSV;
		private System.Windows.Forms.RadioButton rbSourceIsDatabase;
		private System.Windows.Forms.GroupBox gbSourceIsFile;
		private System.Windows.Forms.TextBox txtSourceFileName;
		private System.Windows.Forms.Button btnSourceFilenamePrompt;
		private System.Windows.Forms.GroupBox gbSourceIsDatabase;
		private System.Windows.Forms.ComboBox cboSourceConnectionString;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox txtSourceDatabaseName;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.CheckBox chkSelectAllTables;
		private System.Windows.Forms.Button btnFillTableList;
		private System.Windows.Forms.ListView lvTableList;
	}
}