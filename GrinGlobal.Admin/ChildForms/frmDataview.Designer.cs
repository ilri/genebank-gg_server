namespace GrinGlobal.Admin.ChildForms {
    partial class frmDataview {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataview));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.spcSql = new System.Windows.Forms.SplitContainer();
            this.pnlSource = new System.Windows.Forms.Panel();
            this.lblTables = new System.Windows.Forms.Label();
            this.tcTables = new System.Windows.Forms.TabControl();
            this.tpTablesByName = new System.Windows.Forms.TabPage();
            this.tvByName = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tpTablesByHierarchy = new System.Windows.Forms.TabPage();
            this.tvByRelationship = new System.Windows.Forms.TreeView();
            this.lblDropAs = new System.Windows.Forms.Label();
            this.chkAutoSynchronize = new System.Windows.Forms.CheckBox();
            this.tcSql = new System.Windows.Forms.TabControl();
            this.tpSqlServer = new System.Windows.Forms.TabPage();
            this.txtSQLServer = new System.Windows.Forms.TextBox();
            this.tpPostgreSQL = new System.Windows.Forms.TabPage();
            this.txtPostgreSQL = new System.Windows.Forms.TextBox();
            this.tpMySQL = new System.Windows.Forms.TabPage();
            this.txtMySQL = new System.Windows.Forms.TextBox();
            this.tpOracle = new System.Windows.Forms.TabPage();
            this.txtOracle = new System.Windows.Forms.TextBox();
            this.txtDataviewName = new System.Windows.Forms.TextBox();
            this.lblDataviewName = new System.Windows.Forms.Label();
            this.lbSuggest = new System.Windows.Forms.ListBox();
            this.lblParameters = new System.Windows.Forms.Label();
            this.dgvParameters = new System.Windows.Forms.DataGridView();
            this.colParameterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParameterType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.spcFields = new System.Windows.Forms.SplitContainer();
            this.lblError = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tcBottom = new System.Windows.Forms.TabControl();
            this.tpFields = new System.Windows.Forms.TabPage();
            this.dgvFields = new System.Windows.Forms.DataGridView();
            this.colFieldDataviewName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFieldTableName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colFieldFieldName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colFieldRequired = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFieldVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFieldReadOnly = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFieldPrimaryKey = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFieldTransform = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFieldGuiHint = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colFieldGroupName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colFieldForeignKey = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cmFields = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiFieldsMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiFieldsMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiFieldsDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tpParameters = new System.Windows.Forms.TabPage();
            this.tpProperties = new System.Windows.Forms.TabPage();
            this.pnlProperties = new System.Windows.Forms.Panel();
            this.txtConfigurationOptions = new System.Windows.Forms.TextBox();
            this.lblConfigurationOptions = new System.Windows.Forms.Label();
            this.chkIsWebVisible = new System.Windows.Forms.CheckBox();
            this.chkIsUserVisible = new System.Windows.Forms.CheckBox();
            this.gbFlagsAndSettings = new System.Windows.Forms.GroupBox();
            this.txtDatabaseAreaOrder = new System.Windows.Forms.TextBox();
            this.lblDatabaseAreaOrder = new System.Windows.Forms.Label();
            this.lnkHelpCategorizing = new System.Windows.Forms.LinkLabel();
            this.ddlCategoryCode = new System.Windows.Forms.ComboBox();
            this.lblCategoryName = new System.Windows.Forms.Label();
            this.ddlDatabaseAreaCode = new System.Windows.Forms.ComboBox();
            this.lblDatabaseArea = new System.Windows.Forms.Label();
            this.chkIsReadOnlyOnInsert = new System.Windows.Forms.CheckBox();
            this.chkIsReadOnly = new System.Windows.Forms.CheckBox();
            this.chkIsSystem = new System.Windows.Forms.CheckBox();
            this.gbTransform = new System.Windows.Forms.GroupBox();
            this.chkIsTransform = new System.Windows.Forms.CheckBox();
            this.ddlFieldCaptionSource = new System.Windows.Forms.ComboBox();
            this.lblFieldCaptionSource = new System.Windows.Forms.Label();
            this.ddlFieldValueSource = new System.Windows.Forms.ComboBox();
            this.lblFieldValueSource = new System.Windows.Forms.Label();
            this.ddlFieldNameSource = new System.Windows.Forms.ComboBox();
            this.lblFieldNameSource = new System.Windows.Forms.Label();
            this.tpTitles = new System.Windows.Forms.TabPage();
            this.dgvTitles = new System.Windows.Forms.DataGridView();
            this.colPropertiesLanguage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPropertiesTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPropertiesDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpPreview = new System.Windows.Forms.TabPage();
            this.lblPreviewCount = new System.Windows.Forms.Label();
            this.dgvPreview = new System.Windows.Forms.DataGridView();
            this.btnGo = new System.Windows.Forms.Button();
            this.ddlViewInLanguage = new System.Windows.Forms.ComboBox();
            this.lblViewInLanguage = new System.Windows.Forms.Label();
            this.txtLimitRows = new System.Windows.Forms.TextBox();
            this.chkLimitRows = new System.Windows.Forms.CheckBox();
            this.tmrQueryText = new System.Windows.Forms.Timer(this.components);
            this.pnlSQL = new System.Windows.Forms.Panel();
            this.spcSql.Panel1.SuspendLayout();
            this.spcSql.Panel2.SuspendLayout();
            this.spcSql.SuspendLayout();
            this.pnlSource.SuspendLayout();
            this.tcTables.SuspendLayout();
            this.tpTablesByName.SuspendLayout();
            this.tpTablesByHierarchy.SuspendLayout();
            this.tcSql.SuspendLayout();
            this.tpSqlServer.SuspendLayout();
            this.tpPostgreSQL.SuspendLayout();
            this.tpMySQL.SuspendLayout();
            this.tpOracle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParameters)).BeginInit();
            this.spcFields.Panel1.SuspendLayout();
            this.spcFields.Panel2.SuspendLayout();
            this.spcFields.SuspendLayout();
            this.tcBottom.SuspendLayout();
            this.tpFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFields)).BeginInit();
            this.cmFields.SuspendLayout();
            this.tpParameters.SuspendLayout();
            this.tpProperties.SuspendLayout();
            this.pnlProperties.SuspendLayout();
            this.gbFlagsAndSettings.SuspendLayout();
            this.gbTransform.SuspendLayout();
            this.tpTitles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTitles)).BeginInit();
            this.tpPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).BeginInit();
            this.pnlSQL.SuspendLayout();
            this.SuspendLayout();
            // 
            // spcSql
            // 
            this.spcSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcSql.Location = new System.Drawing.Point(0, 0);
            this.spcSql.Name = "spcSql";
            // 
            // spcSql.Panel1
            // 
            this.spcSql.Panel1.Controls.Add(this.pnlSource);
            // 
            // spcSql.Panel2
            // 
            this.spcSql.Panel2.Controls.Add(this.pnlSQL);
            this.spcSql.Size = new System.Drawing.Size(771, 222);
            this.spcSql.SplitterDistance = 181;
            this.spcSql.TabIndex = 0;
            // 
            // pnlSource
            // 
            this.pnlSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSource.Controls.Add(this.lblTables);
            this.pnlSource.Controls.Add(this.tcTables);
            this.pnlSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSource.Location = new System.Drawing.Point(0, 0);
            this.pnlSource.Name = "pnlSource";
            this.pnlSource.Size = new System.Drawing.Size(181, 222);
            this.pnlSource.TabIndex = 3;
            // 
            // lblTables
            // 
            this.lblTables.AutoSize = true;
            this.lblTables.Location = new System.Drawing.Point(3, 4);
            this.lblTables.Name = "lblTables";
            this.lblTables.Size = new System.Drawing.Size(127, 13);
            this.lblTables.TabIndex = 1;
            this.lblTables.Text = "Source Tables and Fields";
            // 
            // tcTables
            // 
            this.tcTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcTables.Controls.Add(this.tpTablesByName);
            this.tcTables.Controls.Add(this.tpTablesByHierarchy);
            this.tcTables.Location = new System.Drawing.Point(3, 23);
            this.tcTables.Name = "tcTables";
            this.tcTables.SelectedIndex = 0;
            this.tcTables.Size = new System.Drawing.Size(173, 194);
            this.tcTables.TabIndex = 2;
            // 
            // tpTablesByName
            // 
            this.tpTablesByName.Controls.Add(this.tvByName);
            this.tpTablesByName.Location = new System.Drawing.Point(4, 22);
            this.tpTablesByName.Name = "tpTablesByName";
            this.tpTablesByName.Padding = new System.Windows.Forms.Padding(3);
            this.tpTablesByName.Size = new System.Drawing.Size(165, 168);
            this.tpTablesByName.TabIndex = 0;
            this.tpTablesByName.Text = "Name";
            this.tpTablesByName.UseVisualStyleBackColor = true;
            // 
            // tvByName
            // 
            this.tvByName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvByName.HideSelection = false;
            this.tvByName.HotTracking = true;
            this.tvByName.ImageIndex = 0;
            this.tvByName.ImageList = this.imageList1;
            this.tvByName.Location = new System.Drawing.Point(3, 3);
            this.tvByName.Name = "tvByName";
            this.tvByName.PathSeparator = "/";
            this.tvByName.SelectedImageIndex = 0;
            this.tvByName.ShowNodeToolTips = true;
            this.tvByName.Size = new System.Drawing.Size(159, 162);
            this.tvByName.TabIndex = 0;
            this.tvByName.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvByName_ItemDrag);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "table.ico");
            this.imageList1.Images.SetKeyName(1, "table-field.ico");
            this.imageList1.Images.SetKeyName(2, "table-field-req.ico");
            this.imageList1.Images.SetKeyName(3, "folder.png");
            // 
            // tpTablesByHierarchy
            // 
            this.tpTablesByHierarchy.Controls.Add(this.tvByRelationship);
            this.tpTablesByHierarchy.Location = new System.Drawing.Point(4, 22);
            this.tpTablesByHierarchy.Name = "tpTablesByHierarchy";
            this.tpTablesByHierarchy.Padding = new System.Windows.Forms.Padding(3);
            this.tpTablesByHierarchy.Size = new System.Drawing.Size(165, 168);
            this.tpTablesByHierarchy.TabIndex = 1;
            this.tpTablesByHierarchy.Text = "Hierarchy";
            this.tpTablesByHierarchy.UseVisualStyleBackColor = true;
            // 
            // tvByRelationship
            // 
            this.tvByRelationship.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvByRelationship.HideSelection = false;
            this.tvByRelationship.HotTracking = true;
            this.tvByRelationship.ImageIndex = 0;
            this.tvByRelationship.ImageList = this.imageList1;
            this.tvByRelationship.Location = new System.Drawing.Point(3, 3);
            this.tvByRelationship.Name = "tvByRelationship";
            this.tvByRelationship.PathSeparator = "/";
            this.tvByRelationship.SelectedImageIndex = 0;
            this.tvByRelationship.ShowNodeToolTips = true;
            this.tvByRelationship.Size = new System.Drawing.Size(159, 162);
            this.tvByRelationship.TabIndex = 1;
            this.tvByRelationship.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvByRelationship_ItemDrag);
            // 
            // lblDropAs
            // 
            this.lblDropAs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDropAs.ForeColor = System.Drawing.Color.Blue;
            this.lblDropAs.Location = new System.Drawing.Point(3, 3);
            this.lblDropAs.Name = "lblDropAs";
            this.lblDropAs.Size = new System.Drawing.Size(439, 19);
            this.lblDropAs.TabIndex = 5;
            this.lblDropAs.Text = "Show \'Drop As\' text here when dragging and dropping";
            // 
            // chkAutoSynchronize
            // 
            this.chkAutoSynchronize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAutoSynchronize.Checked = true;
            this.chkAutoSynchronize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoSynchronize.Location = new System.Drawing.Point(448, 7);
            this.chkAutoSynchronize.Name = "chkAutoSynchronize";
            this.chkAutoSynchronize.Size = new System.Drawing.Size(130, 17);
            this.chkAutoSynchronize.TabIndex = 4;
            this.chkAutoSynchronize.Text = "Auto Synchronize";
            this.chkAutoSynchronize.UseVisualStyleBackColor = true;
            this.chkAutoSynchronize.CheckedChanged += new System.EventHandler(this.chkAutoSynchronizeGridviews_CheckedChanged);
            // 
            // tcSql
            // 
            this.tcSql.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcSql.Controls.Add(this.tpSqlServer);
            this.tcSql.Controls.Add(this.tpPostgreSQL);
            this.tcSql.Controls.Add(this.tpMySQL);
            this.tcSql.Controls.Add(this.tpOracle);
            this.tcSql.Location = new System.Drawing.Point(0, 30);
            this.tcSql.Name = "tcSql";
            this.tcSql.SelectedIndex = 0;
            this.tcSql.Size = new System.Drawing.Size(587, 192);
            this.tcSql.TabIndex = 0;
            // 
            // tpSqlServer
            // 
            this.tpSqlServer.Controls.Add(this.txtSQLServer);
            this.tpSqlServer.Location = new System.Drawing.Point(4, 22);
            this.tpSqlServer.Name = "tpSqlServer";
            this.tpSqlServer.Size = new System.Drawing.Size(579, 166);
            this.tpSqlServer.TabIndex = 0;
            this.tpSqlServer.Text = "SQL Server";
            this.tpSqlServer.UseVisualStyleBackColor = true;
            // 
            // txtSQLServer
            // 
            this.txtSQLServer.AcceptsReturn = true;
            this.txtSQLServer.AcceptsTab = true;
            this.txtSQLServer.AllowDrop = true;
            this.txtSQLServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSQLServer.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSQLServer.HideSelection = false;
            this.txtSQLServer.Location = new System.Drawing.Point(0, 0);
            this.txtSQLServer.Multiline = true;
            this.txtSQLServer.Name = "txtSQLServer";
            this.txtSQLServer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSQLServer.Size = new System.Drawing.Size(579, 166);
            this.txtSQLServer.TabIndex = 1;
            this.txtSQLServer.WordWrap = false;
            this.txtSQLServer.TextChanged += new System.EventHandler(this.sqlQuery_TextChanged);
            this.txtSQLServer.DragDrop += new System.Windows.Forms.DragEventHandler(this.sqlQuery_DragDrop);
            this.txtSQLServer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.sqlQuery_MouseMove);
            this.txtSQLServer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sqlQuery_KeyDown);
            this.txtSQLServer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.sqlQuery_KeyUp);
            this.txtSQLServer.DragLeave += new System.EventHandler(this.sqlQuery_DragLeave);
            this.txtSQLServer.DragEnter += new System.Windows.Forms.DragEventHandler(this.sqlQuery_DragEnter);
            this.txtSQLServer.DragOver += new System.Windows.Forms.DragEventHandler(this.sqlQuery_DragOver);
            // 
            // tpPostgreSQL
            // 
            this.tpPostgreSQL.Controls.Add(this.txtPostgreSQL);
            this.tpPostgreSQL.Location = new System.Drawing.Point(4, 22);
            this.tpPostgreSQL.Name = "tpPostgreSQL";
            this.tpPostgreSQL.Size = new System.Drawing.Size(572, 162);
            this.tpPostgreSQL.TabIndex = 2;
            this.tpPostgreSQL.Text = "PostgreSQL";
            this.tpPostgreSQL.UseVisualStyleBackColor = true;
            // 
            // txtPostgreSQL
            // 
            this.txtPostgreSQL.AcceptsReturn = true;
            this.txtPostgreSQL.AcceptsTab = true;
            this.txtPostgreSQL.AllowDrop = true;
            this.txtPostgreSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPostgreSQL.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.txtPostgreSQL.HideSelection = false;
            this.txtPostgreSQL.Location = new System.Drawing.Point(0, 0);
            this.txtPostgreSQL.Multiline = true;
            this.txtPostgreSQL.Name = "txtPostgreSQL";
            this.txtPostgreSQL.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPostgreSQL.Size = new System.Drawing.Size(572, 162);
            this.txtPostgreSQL.TabIndex = 1;
            this.txtPostgreSQL.WordWrap = false;
            this.txtPostgreSQL.TextChanged += new System.EventHandler(this.sqlQuery_TextChanged);
            this.txtPostgreSQL.DragDrop += new System.Windows.Forms.DragEventHandler(this.sqlQuery_DragDrop);
            this.txtPostgreSQL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.sqlQuery_MouseMove);
            this.txtPostgreSQL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sqlQuery_KeyDown);
            this.txtPostgreSQL.KeyUp += new System.Windows.Forms.KeyEventHandler(this.sqlQuery_KeyUp);
            this.txtPostgreSQL.DragLeave += new System.EventHandler(this.sqlQuery_DragLeave);
            this.txtPostgreSQL.DragEnter += new System.Windows.Forms.DragEventHandler(this.sqlQuery_DragEnter);
            this.txtPostgreSQL.DragOver += new System.Windows.Forms.DragEventHandler(this.sqlQuery_DragOver);
            // 
            // tpMySQL
            // 
            this.tpMySQL.Controls.Add(this.txtMySQL);
            this.tpMySQL.Location = new System.Drawing.Point(4, 22);
            this.tpMySQL.Name = "tpMySQL";
            this.tpMySQL.Size = new System.Drawing.Size(572, 162);
            this.tpMySQL.TabIndex = 1;
            this.tpMySQL.Text = "MySQL";
            this.tpMySQL.UseVisualStyleBackColor = true;
            // 
            // txtMySQL
            // 
            this.txtMySQL.AcceptsReturn = true;
            this.txtMySQL.AcceptsTab = true;
            this.txtMySQL.AllowDrop = true;
            this.txtMySQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMySQL.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.txtMySQL.HideSelection = false;
            this.txtMySQL.Location = new System.Drawing.Point(0, 0);
            this.txtMySQL.Multiline = true;
            this.txtMySQL.Name = "txtMySQL";
            this.txtMySQL.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMySQL.Size = new System.Drawing.Size(572, 162);
            this.txtMySQL.TabIndex = 1;
            this.txtMySQL.WordWrap = false;
            this.txtMySQL.TextChanged += new System.EventHandler(this.sqlQuery_TextChanged);
            this.txtMySQL.DragDrop += new System.Windows.Forms.DragEventHandler(this.sqlQuery_DragDrop);
            this.txtMySQL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.sqlQuery_MouseMove);
            this.txtMySQL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sqlQuery_KeyDown);
            this.txtMySQL.KeyUp += new System.Windows.Forms.KeyEventHandler(this.sqlQuery_KeyUp);
            this.txtMySQL.DragLeave += new System.EventHandler(this.sqlQuery_DragLeave);
            this.txtMySQL.DragEnter += new System.Windows.Forms.DragEventHandler(this.sqlQuery_DragEnter);
            this.txtMySQL.DragOver += new System.Windows.Forms.DragEventHandler(this.sqlQuery_DragOver);
            // 
            // tpOracle
            // 
            this.tpOracle.Controls.Add(this.txtOracle);
            this.tpOracle.Location = new System.Drawing.Point(4, 22);
            this.tpOracle.Name = "tpOracle";
            this.tpOracle.Size = new System.Drawing.Size(572, 162);
            this.tpOracle.TabIndex = 3;
            this.tpOracle.Text = "Oracle";
            this.tpOracle.UseVisualStyleBackColor = true;
            // 
            // txtOracle
            // 
            this.txtOracle.AcceptsReturn = true;
            this.txtOracle.AcceptsTab = true;
            this.txtOracle.AllowDrop = true;
            this.txtOracle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOracle.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.txtOracle.HideSelection = false;
            this.txtOracle.Location = new System.Drawing.Point(0, 0);
            this.txtOracle.Multiline = true;
            this.txtOracle.Name = "txtOracle";
            this.txtOracle.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOracle.Size = new System.Drawing.Size(572, 162);
            this.txtOracle.TabIndex = 0;
            this.txtOracle.WordWrap = false;
            this.txtOracle.TextChanged += new System.EventHandler(this.sqlQuery_TextChanged);
            this.txtOracle.DragDrop += new System.Windows.Forms.DragEventHandler(this.sqlQuery_DragDrop);
            this.txtOracle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.sqlQuery_MouseMove);
            this.txtOracle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sqlQuery_KeyDown);
            this.txtOracle.KeyUp += new System.Windows.Forms.KeyEventHandler(this.sqlQuery_KeyUp);
            this.txtOracle.DragLeave += new System.EventHandler(this.sqlQuery_DragLeave);
            this.txtOracle.DragEnter += new System.Windows.Forms.DragEventHandler(this.sqlQuery_DragEnter);
            this.txtOracle.DragOver += new System.Windows.Forms.DragEventHandler(this.sqlQuery_DragOver);
            // 
            // txtDataviewName
            // 
            this.txtDataviewName.Location = new System.Drawing.Point(169, 11);
            this.txtDataviewName.MaxLength = 100;
            this.txtDataviewName.Name = "txtDataviewName";
            this.txtDataviewName.Size = new System.Drawing.Size(322, 20);
            this.txtDataviewName.TabIndex = 3;
            this.txtDataviewName.TextChanged += new System.EventHandler(this.txtDataviewName_TextChanged);
            this.txtDataviewName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDataviewName_KeyDown);
            // 
            // lblDataviewName
            // 
            this.lblDataviewName.Location = new System.Drawing.Point(9, 15);
            this.lblDataviewName.Name = "lblDataviewName";
            this.lblDataviewName.Size = new System.Drawing.Size(154, 13);
            this.lblDataviewName.TabIndex = 2;
            this.lblDataviewName.Text = "Dataview Name:";
            this.lblDataviewName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbSuggest
            // 
            this.lbSuggest.FormattingEnabled = true;
            this.lbSuggest.Location = new System.Drawing.Point(367, 0);
            this.lbSuggest.Name = "lbSuggest";
            this.lbSuggest.Size = new System.Drawing.Size(199, 95);
            this.lbSuggest.TabIndex = 2;
            this.lbSuggest.Visible = false;
            // 
            // lblParameters
            // 
            this.lblParameters.AutoSize = true;
            this.lblParameters.Location = new System.Drawing.Point(3, 13);
            this.lblParameters.Name = "lblParameters";
            this.lblParameters.Size = new System.Drawing.Size(60, 13);
            this.lblParameters.TabIndex = 1;
            this.lblParameters.Text = "Parameters";
            // 
            // dgvParameters
            // 
            this.dgvParameters.AllowUserToAddRows = false;
            this.dgvParameters.AllowUserToDeleteRows = false;
            this.dgvParameters.AllowUserToResizeRows = false;
            this.dgvParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colParameterName,
            this.colParameterType});
            this.dgvParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvParameters.Location = new System.Drawing.Point(3, 3);
            this.dgvParameters.Name = "dgvParameters";
            this.dgvParameters.Size = new System.Drawing.Size(754, 203);
            this.dgvParameters.TabIndex = 0;
            this.dgvParameters.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvParameters_CellValidating);
            this.dgvParameters.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvParameters_CellEndEdit);
            // 
            // colParameterName
            // 
            this.colParameterName.HeaderText = "Name";
            this.colParameterName.Name = "colParameterName";
            this.colParameterName.ReadOnly = true;
            this.colParameterName.Width = 200;
            // 
            // colParameterType
            // 
            this.colParameterType.HeaderText = "Type";
            this.colParameterType.Name = "colParameterType";
            this.colParameterType.Width = 200;
            // 
            // spcFields
            // 
            this.spcFields.AllowDrop = true;
            this.spcFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcFields.Location = new System.Drawing.Point(0, 0);
            this.spcFields.Name = "spcFields";
            this.spcFields.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcFields.Panel1
            // 
            this.spcFields.Panel1.Controls.Add(this.spcSql);
            // 
            // spcFields.Panel2
            // 
            this.spcFields.Panel2.AllowDrop = true;
            this.spcFields.Panel2.Controls.Add(this.lblError);
            this.spcFields.Panel2.Controls.Add(this.btnSave);
            this.spcFields.Panel2.Controls.Add(this.btnCancel);
            this.spcFields.Panel2.Controls.Add(this.tcBottom);
            this.spcFields.Size = new System.Drawing.Size(771, 506);
            this.spcFields.SplitterDistance = 222;
            this.spcFields.TabIndex = 0;
            // 
            // lblError
            // 
            this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(7, 242);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(589, 29);
            this.lblError.TabIndex = 4;
            this.lblError.Text = "Error info goes here";
            this.lblError.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSave.Location = new System.Drawing.Point(602, 245);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(683, 245);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tcBottom
            // 
            this.tcBottom.AllowDrop = true;
            this.tcBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcBottom.Controls.Add(this.tpFields);
            this.tcBottom.Controls.Add(this.tpParameters);
            this.tcBottom.Controls.Add(this.tpProperties);
            this.tcBottom.Controls.Add(this.tpTitles);
            this.tcBottom.Controls.Add(this.tpPreview);
            this.tcBottom.Location = new System.Drawing.Point(3, 4);
            this.tcBottom.Name = "tcBottom";
            this.tcBottom.SelectedIndex = 0;
            this.tcBottom.Size = new System.Drawing.Size(768, 235);
            this.tcBottom.TabIndex = 1;
            // 
            // tpFields
            // 
            this.tpFields.AllowDrop = true;
            this.tpFields.Controls.Add(this.dgvFields);
            this.tpFields.Location = new System.Drawing.Point(4, 22);
            this.tpFields.Name = "tpFields";
            this.tpFields.Padding = new System.Windows.Forms.Padding(3);
            this.tpFields.Size = new System.Drawing.Size(760, 209);
            this.tpFields.TabIndex = 0;
            this.tpFields.Text = "Fields";
            this.tpFields.UseVisualStyleBackColor = true;
            // 
            // dgvFields
            // 
            this.dgvFields.AllowDrop = true;
            this.dgvFields.AllowUserToAddRows = false;
            this.dgvFields.AllowUserToDeleteRows = false;
            this.dgvFields.AllowUserToResizeRows = false;
            this.dgvFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFieldDataviewName,
            this.colFieldTableName,
            this.colFieldFieldName,
            this.colFieldRequired,
            this.colFieldVisible,
            this.colFieldReadOnly,
            this.colFieldPrimaryKey,
            this.colFieldTransform,
            this.colFieldGuiHint,
            this.colFieldGroupName,
            this.colFieldForeignKey});
            this.dgvFields.ContextMenuStrip = this.cmFields;
            this.dgvFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFields.Location = new System.Drawing.Point(3, 3);
            this.dgvFields.Name = "dgvFields";
            this.dgvFields.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvFields.Size = new System.Drawing.Size(754, 203);
            this.dgvFields.TabIndex = 0;
            this.dgvFields.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvFields_MouseDown);
            this.dgvFields.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvFields_CellBeginEdit);
            this.dgvFields.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvFields_MouseMove);
            this.dgvFields.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvFields_ColumnHeaderMouseClick);
            this.dgvFields.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvFields_DragOver);
            this.dgvFields.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvFields_CellFormatting);
            this.dgvFields.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvFields_CellValidating);
            this.dgvFields.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFields_CellEndEdit);
            this.dgvFields.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvFields_CellPainting);
            this.dgvFields.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvFields_EditingControlShowing);
            this.dgvFields.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.dgvFields_CellToolTipTextNeeded);
            this.dgvFields.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvFields_DataError);
            this.dgvFields.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvFields_KeyDown);
            this.dgvFields.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvFields_RowsRemoved);
            this.dgvFields.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvFields_KeyPress);
            this.dgvFields.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvFields_KeyUp);
            this.dgvFields.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvFields_DragDrop);
            // 
            // colFieldDataviewName
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colFieldDataviewName.DefaultCellStyle = dataGridViewCellStyle5;
            this.colFieldDataviewName.Frozen = true;
            this.colFieldDataviewName.HeaderText = "Name";
            this.colFieldDataviewName.Name = "colFieldDataviewName";
            this.colFieldDataviewName.ReadOnly = true;
            this.colFieldDataviewName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colFieldDataviewName.Width = 120;
            // 
            // colFieldTableName
            // 
            this.colFieldTableName.HeaderText = "Table";
            this.colFieldTableName.Name = "colFieldTableName";
            this.colFieldTableName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFieldTableName.Width = 150;
            // 
            // colFieldFieldName
            // 
            this.colFieldFieldName.HeaderText = "Field";
            this.colFieldFieldName.Name = "colFieldFieldName";
            this.colFieldFieldName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFieldFieldName.Width = 150;
            // 
            // colFieldRequired
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle6.NullValue = false;
            this.colFieldRequired.DefaultCellStyle = dataGridViewCellStyle6;
            this.colFieldRequired.HeaderText = "Required?";
            this.colFieldRequired.Name = "colFieldRequired";
            this.colFieldRequired.ReadOnly = true;
            this.colFieldRequired.Width = 60;
            // 
            // colFieldVisible
            // 
            this.colFieldVisible.HeaderText = "Visible In CT?";
            this.colFieldVisible.Name = "colFieldVisible";
            this.colFieldVisible.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFieldVisible.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colFieldVisible.Width = 60;
            // 
            // colFieldReadOnly
            // 
            this.colFieldReadOnly.HeaderText = "Read Only?";
            this.colFieldReadOnly.Name = "colFieldReadOnly";
            this.colFieldReadOnly.Width = 60;
            // 
            // colFieldPrimaryKey
            // 
            this.colFieldPrimaryKey.HeaderText = "Primary Key?";
            this.colFieldPrimaryKey.Name = "colFieldPrimaryKey";
            this.colFieldPrimaryKey.Width = 60;
            // 
            // colFieldTransform
            // 
            this.colFieldTransform.HeaderText = "Transform?";
            this.colFieldTransform.Name = "colFieldTransform";
            this.colFieldTransform.Width = 70;
            // 
            // colFieldGuiHint
            // 
            this.colFieldGuiHint.HeaderText = "User Interface";
            this.colFieldGuiHint.Items.AddRange(new object[] {
            "Checkbox",
            "Date / Time Control",
            "Textbox (free-form)",
            "Lookup Picker",
            "Drop Down",
            "Textbox (integers only)",
            "Textbox (decimals only)"});
            this.colFieldGuiHint.Name = "colFieldGuiHint";
            this.colFieldGuiHint.Width = 120;
            // 
            // colFieldGroupName
            // 
            this.colFieldGroupName.HeaderText = "Drop Down Source";
            this.colFieldGroupName.Name = "colFieldGroupName";
            this.colFieldGroupName.Width = 150;
            // 
            // colFieldForeignKey
            // 
            this.colFieldForeignKey.HeaderText = "Lookup Picker Source";
            this.colFieldForeignKey.Name = "colFieldForeignKey";
            this.colFieldForeignKey.Width = 150;
            // 
            // cmFields
            // 
            this.cmFields.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiFieldsMoveUp,
            this.cmiFieldsMoveDown,
            this.toolStripSeparator1,
            this.cmiFieldsDelete});
            this.cmFields.Name = "cmFields";
            this.cmFields.Size = new System.Drawing.Size(139, 76);
            this.cmFields.Opening += new System.ComponentModel.CancelEventHandler(this.cmFields_Opening);
            // 
            // cmiFieldsMoveUp
            // 
            this.cmiFieldsMoveUp.Name = "cmiFieldsMoveUp";
            this.cmiFieldsMoveUp.Size = new System.Drawing.Size(138, 22);
            this.cmiFieldsMoveUp.Text = "Move &Up";
            this.cmiFieldsMoveUp.Click += new System.EventHandler(this.cmiFieldsMoveUp_Click);
            // 
            // cmiFieldsMoveDown
            // 
            this.cmiFieldsMoveDown.Name = "cmiFieldsMoveDown";
            this.cmiFieldsMoveDown.Size = new System.Drawing.Size(138, 22);
            this.cmiFieldsMoveDown.Text = "&Move Down";
            this.cmiFieldsMoveDown.Click += new System.EventHandler(this.cmiFieldsMoveDown_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(135, 6);
            // 
            // cmiFieldsDelete
            // 
            this.cmiFieldsDelete.Name = "cmiFieldsDelete";
            this.cmiFieldsDelete.Size = new System.Drawing.Size(138, 22);
            this.cmiFieldsDelete.Text = "&Delete";
            this.cmiFieldsDelete.Click += new System.EventHandler(this.cmiFieldsDelete_Click);
            // 
            // tpParameters
            // 
            this.tpParameters.Controls.Add(this.dgvParameters);
            this.tpParameters.Location = new System.Drawing.Point(4, 22);
            this.tpParameters.Name = "tpParameters";
            this.tpParameters.Padding = new System.Windows.Forms.Padding(3);
            this.tpParameters.Size = new System.Drawing.Size(760, 209);
            this.tpParameters.TabIndex = 2;
            this.tpParameters.Text = "Parameters";
            this.tpParameters.UseVisualStyleBackColor = true;
            // 
            // tpProperties
            // 
            this.tpProperties.Controls.Add(this.pnlProperties);
            this.tpProperties.Location = new System.Drawing.Point(4, 22);
            this.tpProperties.Name = "tpProperties";
            this.tpProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tpProperties.Size = new System.Drawing.Size(760, 209);
            this.tpProperties.TabIndex = 3;
            this.tpProperties.Text = "Properties";
            this.tpProperties.UseVisualStyleBackColor = true;
            // 
            // pnlProperties
            // 
            this.pnlProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProperties.AutoScroll = true;
            this.pnlProperties.Controls.Add(this.txtConfigurationOptions);
            this.pnlProperties.Controls.Add(this.lblConfigurationOptions);
            this.pnlProperties.Controls.Add(this.chkIsWebVisible);
            this.pnlProperties.Controls.Add(this.chkIsUserVisible);
            this.pnlProperties.Controls.Add(this.gbFlagsAndSettings);
            this.pnlProperties.Controls.Add(this.txtDataviewName);
            this.pnlProperties.Controls.Add(this.chkIsSystem);
            this.pnlProperties.Controls.Add(this.gbTransform);
            this.pnlProperties.Controls.Add(this.lblDataviewName);
            this.pnlProperties.Location = new System.Drawing.Point(3, 6);
            this.pnlProperties.Name = "pnlProperties";
            this.pnlProperties.Size = new System.Drawing.Size(754, 197);
            this.pnlProperties.TabIndex = 7;
            // 
            // txtConfigurationOptions
            // 
            this.txtConfigurationOptions.Location = new System.Drawing.Point(246, 186);
            this.txtConfigurationOptions.Name = "txtConfigurationOptions";
            this.txtConfigurationOptions.Size = new System.Drawing.Size(391, 20);
            this.txtConfigurationOptions.TabIndex = 6;
            this.txtConfigurationOptions.TextChanged += new System.EventHandler(this.txtConfigurationOptions_TextChanged);
            // 
            // lblConfigurationOptions
            // 
            this.lblConfigurationOptions.AutoSize = true;
            this.lblConfigurationOptions.Location = new System.Drawing.Point(128, 186);
            this.lblConfigurationOptions.Name = "lblConfigurationOptions";
            this.lblConfigurationOptions.Size = new System.Drawing.Size(111, 13);
            this.lblConfigurationOptions.TabIndex = 5;
            this.lblConfigurationOptions.Text = "Configuration Options:";
            // 
            // chkIsWebVisible
            // 
            this.chkIsWebVisible.AutoSize = true;
            this.chkIsWebVisible.Location = new System.Drawing.Point(643, 100);
            this.chkIsWebVisible.Name = "chkIsWebVisible";
            this.chkIsWebVisible.Size = new System.Drawing.Size(132, 17);
            this.chkIsWebVisible.TabIndex = 4;
            this.chkIsWebVisible.Text = "Is Visible by Web User";
            this.chkIsWebVisible.UseVisualStyleBackColor = true;
            this.chkIsWebVisible.Visible = false;
            // 
            // chkIsUserVisible
            // 
            this.chkIsUserVisible.AutoSize = true;
            this.chkIsUserVisible.Location = new System.Drawing.Point(643, 77);
            this.chkIsUserVisible.Name = "chkIsUserVisible";
            this.chkIsUserVisible.Size = new System.Drawing.Size(167, 17);
            this.chkIsUserVisible.TabIndex = 2;
            this.chkIsUserVisible.Text = "Is Visible by Curator Tool User";
            this.chkIsUserVisible.UseVisualStyleBackColor = true;
            this.chkIsUserVisible.Visible = false;
            this.chkIsUserVisible.CheckedChanged += new System.EventHandler(this.chkIsVisible_CheckedChanged);
            // 
            // gbFlagsAndSettings
            // 
            this.gbFlagsAndSettings.Controls.Add(this.txtDatabaseAreaOrder);
            this.gbFlagsAndSettings.Controls.Add(this.lblDatabaseAreaOrder);
            this.gbFlagsAndSettings.Controls.Add(this.lnkHelpCategorizing);
            this.gbFlagsAndSettings.Controls.Add(this.ddlCategoryCode);
            this.gbFlagsAndSettings.Controls.Add(this.lblCategoryName);
            this.gbFlagsAndSettings.Controls.Add(this.ddlDatabaseAreaCode);
            this.gbFlagsAndSettings.Controls.Add(this.lblDatabaseArea);
            this.gbFlagsAndSettings.Controls.Add(this.chkIsReadOnlyOnInsert);
            this.gbFlagsAndSettings.Controls.Add(this.chkIsReadOnly);
            this.gbFlagsAndSettings.Location = new System.Drawing.Point(2, 37);
            this.gbFlagsAndSettings.Name = "gbFlagsAndSettings";
            this.gbFlagsAndSettings.Size = new System.Drawing.Size(303, 139);
            this.gbFlagsAndSettings.TabIndex = 2;
            this.gbFlagsAndSettings.TabStop = false;
            this.gbFlagsAndSettings.Text = "Flags and Settings";
            // 
            // txtDatabaseAreaOrder
            // 
            this.txtDatabaseAreaOrder.Location = new System.Drawing.Point(115, 113);
            this.txtDatabaseAreaOrder.Name = "txtDatabaseAreaOrder";
            this.txtDatabaseAreaOrder.Size = new System.Drawing.Size(100, 20);
            this.txtDatabaseAreaOrder.TabIndex = 3;
            this.txtDatabaseAreaOrder.TextChanged += new System.EventHandler(this.txtCategoryOrder_TextChanged);
            // 
            // lblDatabaseAreaOrder
            // 
            this.lblDatabaseAreaOrder.Location = new System.Drawing.Point(8, 117);
            this.lblDatabaseAreaOrder.Name = "lblDatabaseAreaOrder";
            this.lblDatabaseAreaOrder.Size = new System.Drawing.Size(101, 16);
            this.lblDatabaseAreaOrder.TabIndex = 2;
            this.lblDatabaseAreaOrder.Text = "Order:";
            this.lblDatabaseAreaOrder.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lnkHelpCategorizing
            // 
            this.lnkHelpCategorizing.AutoSize = true;
            this.lnkHelpCategorizing.Location = new System.Drawing.Point(259, 67);
            this.lnkHelpCategorizing.Name = "lnkHelpCategorizing";
            this.lnkHelpCategorizing.Size = new System.Drawing.Size(38, 13);
            this.lnkHelpCategorizing.TabIndex = 5;
            this.lnkHelpCategorizing.TabStop = true;
            this.lnkHelpCategorizing.Text = "Help...";
            this.lnkHelpCategorizing.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpNaming_LinkClicked);
            // 
            // ddlCategoryCode
            // 
            this.ddlCategoryCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCategoryCode.FormattingEnabled = true;
            this.ddlCategoryCode.Items.AddRange(new object[] {
            "Curator Tool",
            "Import Wizard",
            "Search Engine",
            "Web Application",
            "System"});
            this.ddlCategoryCode.Location = new System.Drawing.Point(115, 63);
            this.ddlCategoryCode.MaxLength = 100;
            this.ddlCategoryCode.Name = "ddlCategoryCode";
            this.ddlCategoryCode.Size = new System.Drawing.Size(138, 21);
            this.ddlCategoryCode.TabIndex = 5;
            this.ddlCategoryCode.SelectedIndexChanged += new System.EventHandler(this.cboCategoryName_SelectedIndexChanged_1);
            this.ddlCategoryCode.TextChanged += new System.EventHandler(this.cboCategoryName_TextChanged);
            // 
            // lblCategoryName
            // 
            this.lblCategoryName.Location = new System.Drawing.Point(8, 66);
            this.lblCategoryName.Name = "lblCategoryName";
            this.lblCategoryName.Size = new System.Drawing.Size(101, 16);
            this.lblCategoryName.TabIndex = 4;
            this.lblCategoryName.Text = "Category:";
            this.lblCategoryName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ddlDatabaseAreaCode
            // 
            this.ddlDatabaseAreaCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDatabaseAreaCode.FormattingEnabled = true;
            this.ddlDatabaseAreaCode.Location = new System.Drawing.Point(115, 90);
            this.ddlDatabaseAreaCode.MaxLength = 50;
            this.ddlDatabaseAreaCode.Name = "ddlDatabaseAreaCode";
            this.ddlDatabaseAreaCode.Size = new System.Drawing.Size(138, 21);
            this.ddlDatabaseAreaCode.TabIndex = 1;
            this.ddlDatabaseAreaCode.SelectedIndexChanged += new System.EventHandler(this.cboDatabaseArea_SelectedIndexChanged);
            this.ddlDatabaseAreaCode.TextChanged += new System.EventHandler(this.cboDatabaseArea_TextChanged);
            // 
            // lblDatabaseArea
            // 
            this.lblDatabaseArea.Location = new System.Drawing.Point(8, 93);
            this.lblDatabaseArea.Name = "lblDatabaseArea";
            this.lblDatabaseArea.Size = new System.Drawing.Size(101, 16);
            this.lblDatabaseArea.TabIndex = 0;
            this.lblDatabaseArea.Text = "Database Area:";
            this.lblDatabaseArea.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkIsReadOnlyOnInsert
            // 
            this.chkIsReadOnlyOnInsert.AutoSize = true;
            this.chkIsReadOnlyOnInsert.Location = new System.Drawing.Point(115, 40);
            this.chkIsReadOnlyOnInsert.Name = "chkIsReadOnlyOnInsert";
            this.chkIsReadOnlyOnInsert.Size = new System.Drawing.Size(133, 17);
            this.chkIsReadOnlyOnInsert.TabIndex = 3;
            this.chkIsReadOnlyOnInsert.Text = "Is Read Only On Insert";
            this.chkIsReadOnlyOnInsert.UseVisualStyleBackColor = true;
            this.chkIsReadOnlyOnInsert.CheckedChanged += new System.EventHandler(this.chkIsReadOnlyOnInsert_CheckedChanged);
            // 
            // chkIsReadOnly
            // 
            this.chkIsReadOnly.AutoSize = true;
            this.chkIsReadOnly.Location = new System.Drawing.Point(115, 17);
            this.chkIsReadOnly.Name = "chkIsReadOnly";
            this.chkIsReadOnly.Size = new System.Drawing.Size(87, 17);
            this.chkIsReadOnly.TabIndex = 1;
            this.chkIsReadOnly.Text = "Is Read Only";
            this.chkIsReadOnly.UseVisualStyleBackColor = true;
            this.chkIsReadOnly.CheckedChanged += new System.EventHandler(this.chkIsReadOnly_CheckedChanged);
            // 
            // chkIsSystem
            // 
            this.chkIsSystem.AutoSize = true;
            this.chkIsSystem.Location = new System.Drawing.Point(643, 131);
            this.chkIsSystem.Name = "chkIsSystem";
            this.chkIsSystem.Size = new System.Drawing.Size(231, 17);
            this.chkIsSystem.TabIndex = 0;
            this.chkIsSystem.Text = "Is System (all users are given Read access)";
            this.chkIsSystem.UseVisualStyleBackColor = true;
            this.chkIsSystem.Visible = false;
            this.chkIsSystem.CheckedChanged += new System.EventHandler(this.chkIssys_CheckedChanged);
            // 
            // gbTransform
            // 
            this.gbTransform.Controls.Add(this.chkIsTransform);
            this.gbTransform.Controls.Add(this.ddlFieldCaptionSource);
            this.gbTransform.Controls.Add(this.lblFieldCaptionSource);
            this.gbTransform.Controls.Add(this.ddlFieldValueSource);
            this.gbTransform.Controls.Add(this.lblFieldValueSource);
            this.gbTransform.Controls.Add(this.ddlFieldNameSource);
            this.gbTransform.Controls.Add(this.lblFieldNameSource);
            this.gbTransform.Location = new System.Drawing.Point(311, 37);
            this.gbTransform.Name = "gbTransform";
            this.gbTransform.Size = new System.Drawing.Size(326, 139);
            this.gbTransform.TabIndex = 4;
            this.gbTransform.TabStop = false;
            // 
            // chkIsTransform
            // 
            this.chkIsTransform.AutoSize = true;
            this.chkIsTransform.Location = new System.Drawing.Point(13, 0);
            this.chkIsTransform.Name = "chkIsTransform";
            this.chkIsTransform.Size = new System.Drawing.Size(108, 17);
            this.chkIsTransform.TabIndex = 6;
            this.chkIsTransform.Text = "Transform Output";
            this.chkIsTransform.UseVisualStyleBackColor = true;
            this.chkIsTransform.CheckedChanged += new System.EventHandler(this.chkIsTransform_CheckedChanged);
            // 
            // ddlFieldCaptionSource
            // 
            this.ddlFieldCaptionSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFieldCaptionSource.FormattingEnabled = true;
            this.ddlFieldCaptionSource.Location = new System.Drawing.Point(127, 78);
            this.ddlFieldCaptionSource.Name = "ddlFieldCaptionSource";
            this.ddlFieldCaptionSource.Size = new System.Drawing.Size(193, 21);
            this.ddlFieldCaptionSource.TabIndex = 5;
            this.ddlFieldCaptionSource.SelectedIndexChanged += new System.EventHandler(this.ddlFieldCaptionSource_SelectedIndexChanged);
            // 
            // lblFieldCaptionSource
            // 
            this.lblFieldCaptionSource.Location = new System.Drawing.Point(6, 83);
            this.lblFieldCaptionSource.Name = "lblFieldCaptionSource";
            this.lblFieldCaptionSource.Size = new System.Drawing.Size(115, 16);
            this.lblFieldCaptionSource.TabIndex = 4;
            this.lblFieldCaptionSource.Text = "Field for Captions:";
            this.lblFieldCaptionSource.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ddlFieldValueSource
            // 
            this.ddlFieldValueSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFieldValueSource.FormattingEnabled = true;
            this.ddlFieldValueSource.Location = new System.Drawing.Point(127, 51);
            this.ddlFieldValueSource.Name = "ddlFieldValueSource";
            this.ddlFieldValueSource.Size = new System.Drawing.Size(193, 21);
            this.ddlFieldValueSource.TabIndex = 3;
            this.ddlFieldValueSource.SelectedIndexChanged += new System.EventHandler(this.ddlFieldValueSource_SelectedIndexChanged);
            // 
            // lblFieldValueSource
            // 
            this.lblFieldValueSource.Location = new System.Drawing.Point(9, 54);
            this.lblFieldValueSource.Name = "lblFieldValueSource";
            this.lblFieldValueSource.Size = new System.Drawing.Size(112, 16);
            this.lblFieldValueSource.TabIndex = 2;
            this.lblFieldValueSource.Text = "Field for Values:";
            this.lblFieldValueSource.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ddlFieldNameSource
            // 
            this.ddlFieldNameSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFieldNameSource.FormattingEnabled = true;
            this.ddlFieldNameSource.Location = new System.Drawing.Point(127, 19);
            this.ddlFieldNameSource.Name = "ddlFieldNameSource";
            this.ddlFieldNameSource.Size = new System.Drawing.Size(193, 21);
            this.ddlFieldNameSource.TabIndex = 1;
            this.ddlFieldNameSource.SelectedIndexChanged += new System.EventHandler(this.ddlFieldNameSource_SelectedIndexChanged);
            // 
            // lblFieldNameSource
            // 
            this.lblFieldNameSource.Location = new System.Drawing.Point(6, 24);
            this.lblFieldNameSource.Name = "lblFieldNameSource";
            this.lblFieldNameSource.Size = new System.Drawing.Size(115, 16);
            this.lblFieldNameSource.TabIndex = 0;
            this.lblFieldNameSource.Text = "Field for Names:";
            this.lblFieldNameSource.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tpTitles
            // 
            this.tpTitles.Controls.Add(this.dgvTitles);
            this.tpTitles.Location = new System.Drawing.Point(4, 22);
            this.tpTitles.Name = "tpTitles";
            this.tpTitles.Size = new System.Drawing.Size(760, 209);
            this.tpTitles.TabIndex = 4;
            this.tpTitles.Text = "Titles";
            this.tpTitles.UseVisualStyleBackColor = true;
            // 
            // dgvTitles
            // 
            this.dgvTitles.AllowUserToAddRows = false;
            this.dgvTitles.AllowUserToDeleteRows = false;
            this.dgvTitles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTitles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPropertiesLanguage,
            this.colPropertiesTitle,
            this.colPropertiesDescription});
            this.dgvTitles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTitles.Location = new System.Drawing.Point(0, 0);
            this.dgvTitles.Name = "dgvTitles";
            this.dgvTitles.Size = new System.Drawing.Size(760, 209);
            this.dgvTitles.TabIndex = 5;
            this.dgvTitles.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTitles_CellEndEdit);
            this.dgvTitles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTitles_KeyDown);
            // 
            // colPropertiesLanguage
            // 
            this.colPropertiesLanguage.HeaderText = "Language";
            this.colPropertiesLanguage.Name = "colPropertiesLanguage";
            this.colPropertiesLanguage.Width = 120;
            // 
            // colPropertiesTitle
            // 
            this.colPropertiesTitle.HeaderText = "Title";
            this.colPropertiesTitle.Name = "colPropertiesTitle";
            this.colPropertiesTitle.Width = 200;
            // 
            // colPropertiesDescription
            // 
            this.colPropertiesDescription.HeaderText = "Description";
            this.colPropertiesDescription.Name = "colPropertiesDescription";
            this.colPropertiesDescription.Width = 400;
            // 
            // tpPreview
            // 
            this.tpPreview.Controls.Add(this.lblPreviewCount);
            this.tpPreview.Controls.Add(this.dgvPreview);
            this.tpPreview.Controls.Add(this.btnGo);
            this.tpPreview.Controls.Add(this.ddlViewInLanguage);
            this.tpPreview.Controls.Add(this.lblViewInLanguage);
            this.tpPreview.Controls.Add(this.txtLimitRows);
            this.tpPreview.Controls.Add(this.chkLimitRows);
            this.tpPreview.Location = new System.Drawing.Point(4, 22);
            this.tpPreview.Name = "tpPreview";
            this.tpPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tpPreview.Size = new System.Drawing.Size(760, 209);
            this.tpPreview.TabIndex = 1;
            this.tpPreview.Text = "Preview";
            this.tpPreview.UseVisualStyleBackColor = true;
            // 
            // lblPreviewCount
            // 
            this.lblPreviewCount.AutoSize = true;
            this.lblPreviewCount.Location = new System.Drawing.Point(513, 9);
            this.lblPreviewCount.Name = "lblPreviewCount";
            this.lblPreviewCount.Size = new System.Drawing.Size(10, 13);
            this.lblPreviewCount.TabIndex = 6;
            this.lblPreviewCount.Text = "-";
            // 
            // dgvPreview
            // 
            this.dgvPreview.AllowUserToAddRows = false;
            this.dgvPreview.AllowUserToDeleteRows = false;
            this.dgvPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreview.Location = new System.Drawing.Point(3, 45);
            this.dgvPreview.Name = "dgvPreview";
            this.dgvPreview.ReadOnly = true;
            this.dgvPreview.RowHeadersVisible = false;
            this.dgvPreview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPreview.ShowCellErrors = false;
            this.dgvPreview.ShowCellToolTips = false;
            this.dgvPreview.ShowEditingIcon = false;
            this.dgvPreview.ShowRowErrors = false;
            this.dgvPreview.Size = new System.Drawing.Size(754, 161);
            this.dgvPreview.TabIndex = 5;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(422, 4);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 4;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // ddlViewInLanguage
            // 
            this.ddlViewInLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlViewInLanguage.FormattingEnabled = true;
            this.ddlViewInLanguage.Location = new System.Drawing.Point(245, 6);
            this.ddlViewInLanguage.Name = "ddlViewInLanguage";
            this.ddlViewInLanguage.Size = new System.Drawing.Size(171, 21);
            this.ddlViewInLanguage.TabIndex = 3;
            // 
            // lblViewInLanguage
            // 
            this.lblViewInLanguage.AutoSize = true;
            this.lblViewInLanguage.Location = new System.Drawing.Point(194, 9);
            this.lblViewInLanguage.Name = "lblViewInLanguage";
            this.lblViewInLanguage.Size = new System.Drawing.Size(45, 13);
            this.lblViewInLanguage.TabIndex = 2;
            this.lblViewInLanguage.Text = "View In:";
            // 
            // txtLimitRows
            // 
            this.txtLimitRows.Location = new System.Drawing.Point(110, 6);
            this.txtLimitRows.Name = "txtLimitRows";
            this.txtLimitRows.Size = new System.Drawing.Size(57, 20);
            this.txtLimitRows.TabIndex = 1;
            this.txtLimitRows.Text = "100";
            this.txtLimitRows.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkLimitRows
            // 
            this.chkLimitRows.AutoSize = true;
            this.chkLimitRows.Checked = true;
            this.chkLimitRows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLimitRows.Location = new System.Drawing.Point(8, 8);
            this.chkLimitRows.Name = "chkLimitRows";
            this.chkLimitRows.Size = new System.Drawing.Size(96, 17);
            this.chkLimitRows.TabIndex = 0;
            this.chkLimitRows.Text = "Limit Rows To:";
            this.chkLimitRows.UseVisualStyleBackColor = true;
            this.chkLimitRows.CheckedChanged += new System.EventHandler(this.chkLimitRows_CheckedChanged);
            // 
            // tmrQueryText
            // 
            this.tmrQueryText.Interval = 2000;
            this.tmrQueryText.Tick += new System.EventHandler(this.tmrQueryText_Tick);
            // 
            // pnlSQL
            // 
            this.pnlSQL.Controls.Add(this.lblDropAs);
            this.pnlSQL.Controls.Add(this.chkAutoSynchronize);
            this.pnlSQL.Controls.Add(this.tcSql);
            this.pnlSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSQL.Location = new System.Drawing.Point(0, 0);
            this.pnlSQL.Name = "pnlSQL";
            this.pnlSQL.Size = new System.Drawing.Size(586, 222);
            this.pnlSQL.TabIndex = 6;
            // 
            // frmDataview
            // 
            this.AcceptButton = this.btnSave;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(771, 506);
            this.Controls.Add(this.spcFields);
            this.Controls.Add(this.lbSuggest);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDataview";
            this.Text = "Dataview";
            this.Load += new System.EventHandler(this.frmDataview_Load);
            this.Activated += new System.EventHandler(this.frmDataview_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDataview_FormClosing);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.frmDataview_DragOver);
            this.spcSql.Panel1.ResumeLayout(false);
            this.spcSql.Panel2.ResumeLayout(false);
            this.spcSql.ResumeLayout(false);
            this.pnlSource.ResumeLayout(false);
            this.pnlSource.PerformLayout();
            this.tcTables.ResumeLayout(false);
            this.tpTablesByName.ResumeLayout(false);
            this.tpTablesByHierarchy.ResumeLayout(false);
            this.tcSql.ResumeLayout(false);
            this.tpSqlServer.ResumeLayout(false);
            this.tpSqlServer.PerformLayout();
            this.tpPostgreSQL.ResumeLayout(false);
            this.tpPostgreSQL.PerformLayout();
            this.tpMySQL.ResumeLayout(false);
            this.tpMySQL.PerformLayout();
            this.tpOracle.ResumeLayout(false);
            this.tpOracle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParameters)).EndInit();
            this.spcFields.Panel1.ResumeLayout(false);
            this.spcFields.Panel2.ResumeLayout(false);
            this.spcFields.ResumeLayout(false);
            this.tcBottom.ResumeLayout(false);
            this.tpFields.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFields)).EndInit();
            this.cmFields.ResumeLayout(false);
            this.tpParameters.ResumeLayout(false);
            this.tpProperties.ResumeLayout(false);
            this.pnlProperties.ResumeLayout(false);
            this.pnlProperties.PerformLayout();
            this.gbFlagsAndSettings.ResumeLayout(false);
            this.gbFlagsAndSettings.PerformLayout();
            this.gbTransform.ResumeLayout(false);
            this.gbTransform.PerformLayout();
            this.tpTitles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTitles)).EndInit();
            this.tpPreview.ResumeLayout(false);
            this.tpPreview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).EndInit();
            this.pnlSQL.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spcFields;
        private System.Windows.Forms.SplitContainer spcSql;
        private System.Windows.Forms.TabControl tcSql;
        private System.Windows.Forms.TabPage tpSqlServer;
        private System.Windows.Forms.TabPage tpPostgreSQL;
        private System.Windows.Forms.TabPage tpMySQL;
        private System.Windows.Forms.TabPage tpOracle;
        private System.Windows.Forms.TreeView tvByName;
        private System.Windows.Forms.DataGridView dgvParameters;
        private System.Windows.Forms.TabControl tcTables;
        private System.Windows.Forms.TabPage tpTablesByName;
        private System.Windows.Forms.TabPage tpTablesByHierarchy;
        private System.Windows.Forms.Label lblTables;
        private System.Windows.Forms.Label lblDataviewName;
        private System.Windows.Forms.Label lblParameters;
        private System.Windows.Forms.TreeView tvByRelationship;
        private System.Windows.Forms.TabControl tcBottom;
        private System.Windows.Forms.TabPage tpFields;
        private System.Windows.Forms.TabPage tpPreview;
        private System.Windows.Forms.DataGridView dgvFields;
        private System.Windows.Forms.TabPage tpParameters;
        private System.Windows.Forms.Label lblViewInLanguage;
        private System.Windows.Forms.TextBox txtLimitRows;
        private System.Windows.Forms.CheckBox chkLimitRows;
        private System.Windows.Forms.ComboBox ddlViewInLanguage;
        private System.Windows.Forms.DataGridView dgvPreview;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TabPage tpProperties;
        private System.Windows.Forms.CheckBox chkIsSystem;
        private System.Windows.Forms.GroupBox gbFlagsAndSettings;
        private System.Windows.Forms.CheckBox chkIsUserVisible;
        private System.Windows.Forms.CheckBox chkIsReadOnly;
        private System.Windows.Forms.TextBox txtDatabaseAreaOrder;
        private System.Windows.Forms.Label lblDatabaseAreaOrder;
        private System.Windows.Forms.Label lblDatabaseArea;
        private System.Windows.Forms.GroupBox gbTransform;
        private System.Windows.Forms.ComboBox ddlFieldNameSource;
        private System.Windows.Forms.Label lblFieldNameSource;
        private System.Windows.Forms.DataGridView dgvTitles;
        private System.Windows.Forms.ComboBox ddlFieldCaptionSource;
        private System.Windows.Forms.Label lblFieldCaptionSource;
        private System.Windows.Forms.ComboBox ddlFieldValueSource;
        private System.Windows.Forms.Label lblFieldValueSource;
        private System.Windows.Forms.CheckBox chkIsTransform;
        private System.Windows.Forms.CheckBox chkIsReadOnlyOnInsert;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtSQLServer;
        private System.Windows.Forms.TextBox txtPostgreSQL;
        private System.Windows.Forms.TextBox txtMySQL;
        private System.Windows.Forms.TextBox txtOracle;
        private System.Windows.Forms.ListBox lbSuggest;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPropertiesLanguage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPropertiesTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPropertiesDescription;
        private System.Windows.Forms.TextBox txtDataviewName;
        private System.Windows.Forms.Label lblPreviewCount;
        private System.Windows.Forms.Panel pnlProperties;
        private System.Windows.Forms.TabPage tpTitles;
        private System.Windows.Forms.CheckBox chkAutoSynchronize;
        private System.Windows.Forms.Timer tmrQueryText;
        private System.Windows.Forms.ContextMenuStrip cmFields;
        private System.Windows.Forms.ToolStripMenuItem cmiFieldsMoveUp;
        private System.Windows.Forms.ToolStripMenuItem cmiFieldsMoveDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmiFieldsDelete;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.CheckBox chkIsWebVisible;
        private System.Windows.Forms.LinkLabel lnkHelpCategorizing;
        private System.Windows.Forms.Label lblCategoryName;
        public System.Windows.Forms.ComboBox ddlDatabaseAreaCode;
        public System.Windows.Forms.ComboBox ddlCategoryCode;
        private System.Windows.Forms.Label lblDropAs;
        private System.Windows.Forms.Label lblConfigurationOptions;
        private System.Windows.Forms.TextBox txtConfigurationOptions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParameterName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colParameterType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFieldDataviewName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colFieldTableName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colFieldFieldName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFieldRequired;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFieldVisible;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFieldReadOnly;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFieldPrimaryKey;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFieldTransform;
        private System.Windows.Forms.DataGridViewComboBoxColumn colFieldGuiHint;
        private System.Windows.Forms.DataGridViewComboBoxColumn colFieldGroupName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colFieldForeignKey;
        private System.Windows.Forms.Panel pnlSource;
        private System.Windows.Forms.Panel pnlSQL;
    }
}
