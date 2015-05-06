namespace GrinGlobal.Admin.ChildForms {
    partial class frmSearchEngineIndex {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchEngineIndex));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtIndexName = new System.Windows.Forms.TextBox();
            this.lblIndexName = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.cmField = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiFieldRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiFieldProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.txtPrimaryKeyField = new System.Windows.Forms.TextBox();
            this.lblPrimaryKeyField = new System.Windows.Forms.Label();
            this.lvFields = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colStoredInIndex = new System.Windows.Forms.ColumnHeader();
            this.colSearchable = new System.Windows.Forms.ColumnHeader();
            this.colCalculation = new System.Windows.Forms.ColumnHeader();
            this.colFormat = new System.Windows.Forms.ColumnHeader();
            this.colIsBoolean = new System.Windows.Forms.ColumnHeader();
            this.colTrueValue = new System.Windows.Forms.ColumnHeader();
            this.tcIndex = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.txtKeywordCacheSize = new System.Windows.Forms.TextBox();
            this.lblKeywordCacheSize = new System.Windows.Forms.Label();
            this.txtNodeCacheSize = new System.Windows.Forms.TextBox();
            this.lblNodeCacheSize = new System.Windows.Forms.Label();
            this.tpFields = new System.Windows.Forms.TabPage();
            this.tpResolvers = new System.Windows.Forms.TabPage();
            this.lvResolvers = new System.Windows.Forms.ListView();
            this.colResolverName = new System.Windows.Forms.ColumnHeader();
            this.colResolverEnabled = new System.Windows.Forms.ColumnHeader();
            this.colResolverMethod = new System.Windows.Forms.ColumnHeader();
            this.colResolverValid = new System.Windows.Forms.ColumnHeader();
            this.cmResolver = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiResolverNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiResolverEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiResolverDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiResolverDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiResolverRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiResolverProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.tpStats = new System.Windows.Forms.TabPage();
            this.lblErrorsSinceLastRebuild = new System.Windows.Forms.Label();
            this.lblLastRebuildEndDate = new System.Windows.Forms.Label();
            this.lblLastRebuildBeginDate = new System.Windows.Forms.Label();
            this.lblLastRebuildEndDateTitle = new System.Windows.Forms.Label();
            this.txtErrorOnRebuild = new System.Windows.Forms.TextBox();
            this.lblErrorOnRebuild = new System.Windows.Forms.Label();
            this.lblErrorsSinceLastRebuildTitle = new System.Windows.Forms.Label();
            this.lblLastRebuildDateTitle = new System.Windows.Forms.Label();
            this.cmParameter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiParameterNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiParameterDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiParameterRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiParameterProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.tpAdvanced = new System.Windows.Forms.TabPage();
            this.gbIndexGeneration = new System.Windows.Forms.GroupBox();
            this.txtEncoding = new System.Windows.Forms.TextBox();
            this.lblEncoding = new System.Windows.Forms.Label();
            this.txtMaxSortSize = new System.Windows.Forms.TextBox();
            this.lblMaxSortSize = new System.Windows.Forms.Label();
            this.txtFanoutSize = new System.Windows.Forms.TextBox();
            this.lblFanoutSize = new System.Windows.Forms.Label();
            this.txtAverageKeywordSize = new System.Windows.Forms.TextBox();
            this.lblAverageKeywordSize = new System.Windows.Forms.Label();
            this.chkStripHtml = new System.Windows.Forms.CheckBox();
            this.chkIndexAllTextFields = new System.Windows.Forms.CheckBox();
            this.btnNewDataview = new System.Windows.Forms.Button();
            this.btnEditDataview = new System.Windows.Forms.Button();
            this.ddlSearchDataviews = new System.Windows.Forms.ComboBox();
            this.lblDataviewName = new System.Windows.Forms.Label();
            this.cmField.SuspendLayout();
            this.tcIndex.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpFields.SuspendLayout();
            this.tpResolvers.SuspendLayout();
            this.cmResolver.SuspendLayout();
            this.tpStats.SuspendLayout();
            this.cmParameter.SuspendLayout();
            this.tpAdvanced.SuspendLayout();
            this.gbIndexGeneration.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(493, 417);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 51;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(412, 417);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 50;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtIndexName
            // 
            this.txtIndexName.Location = new System.Drawing.Point(172, 17);
            this.txtIndexName.Name = "txtIndexName";
            this.txtIndexName.Size = new System.Drawing.Size(200, 20);
            this.txtIndexName.TabIndex = 0;
            this.txtIndexName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblIndexName
            // 
            this.lblIndexName.Location = new System.Drawing.Point(6, 17);
            this.lblIndexName.Name = "lblIndexName";
            this.lblIndexName.Size = new System.Drawing.Size(160, 13);
            this.lblIndexName.TabIndex = 15;
            this.lblIndexName.Text = "Index Name:";
            this.lblIndexName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Location = new System.Drawing.Point(172, 69);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkEnabled.TabIndex = 3;
            this.chkEnabled.Text = "Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // cmField
            // 
            this.cmField.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.cmiFieldRefresh,
            this.cmiFieldProperties});
            this.cmField.Name = "contextMenuStrip1";
            this.cmField.Size = new System.Drawing.Size(133, 54);
            this.cmField.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(129, 6);
            // 
            // cmiFieldRefresh
            // 
            this.cmiFieldRefresh.Name = "cmiFieldRefresh";
            this.cmiFieldRefresh.Size = new System.Drawing.Size(132, 22);
            this.cmiFieldRefresh.Text = "&Refresh";
            this.cmiFieldRefresh.Click += new System.EventHandler(this.cmiFieldRefresh_Click);
            // 
            // cmiFieldProperties
            // 
            this.cmiFieldProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiFieldProperties.Name = "cmiFieldProperties";
            this.cmiFieldProperties.Size = new System.Drawing.Size(132, 22);
            this.cmiFieldProperties.Text = "&Properties";
            this.cmiFieldProperties.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // txtPrimaryKeyField
            // 
            this.txtPrimaryKeyField.Location = new System.Drawing.Point(172, 43);
            this.txtPrimaryKeyField.Name = "txtPrimaryKeyField";
            this.txtPrimaryKeyField.Size = new System.Drawing.Size(200, 20);
            this.txtPrimaryKeyField.TabIndex = 54;
            this.txtPrimaryKeyField.TextChanged += new System.EventHandler(this.txtPrimaryKeyField_TextChanged);
            // 
            // lblPrimaryKeyField
            // 
            this.lblPrimaryKeyField.Location = new System.Drawing.Point(9, 46);
            this.lblPrimaryKeyField.Name = "lblPrimaryKeyField";
            this.lblPrimaryKeyField.Size = new System.Drawing.Size(157, 13);
            this.lblPrimaryKeyField.TabIndex = 55;
            this.lblPrimaryKeyField.Text = "Primary Key Field:";
            this.lblPrimaryKeyField.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lvFields
            // 
            this.lvFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colStoredInIndex,
            this.colSearchable,
            this.colCalculation,
            this.colFormat,
            this.colIsBoolean,
            this.colTrueValue});
            this.lvFields.ContextMenuStrip = this.cmField;
            this.lvFields.FullRowSelect = true;
            this.lvFields.Location = new System.Drawing.Point(5, 3);
            this.lvFields.Name = "lvFields";
            this.lvFields.Size = new System.Drawing.Size(537, 367);
            this.lvFields.TabIndex = 0;
            this.lvFields.UseCompatibleStateImageBehavior = false;
            this.lvFields.View = System.Windows.Forms.View.Details;
            this.lvFields.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvFields_MouseDoubleClick);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 100;
            // 
            // colStoredInIndex
            // 
            this.colStoredInIndex.Text = "Stored?";
            // 
            // colSearchable
            // 
            this.colSearchable.Text = "Searchable?";
            this.colSearchable.Width = 83;
            // 
            // colCalculation
            // 
            this.colCalculation.Text = "Calculation";
            this.colCalculation.Width = 115;
            // 
            // colFormat
            // 
            this.colFormat.Text = "Format";
            this.colFormat.Width = 70;
            // 
            // colIsBoolean
            // 
            this.colIsBoolean.Text = "Boolean?";
            // 
            // colTrueValue
            // 
            this.colTrueValue.Text = "True Value";
            // 
            // tcIndex
            // 
            this.tcIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcIndex.Controls.Add(this.tpGeneral);
            this.tcIndex.Controls.Add(this.tpAdvanced);
            this.tcIndex.Controls.Add(this.tpFields);
            this.tcIndex.Controls.Add(this.tpResolvers);
            this.tcIndex.Controls.Add(this.tpStats);
            this.tcIndex.Location = new System.Drawing.Point(16, 12);
            this.tcIndex.Name = "tcIndex";
            this.tcIndex.SelectedIndex = 0;
            this.tcIndex.Size = new System.Drawing.Size(553, 399);
            this.tcIndex.TabIndex = 60;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.btnNewDataview);
            this.tpGeneral.Controls.Add(this.btnEditDataview);
            this.tpGeneral.Controls.Add(this.ddlSearchDataviews);
            this.tpGeneral.Controls.Add(this.lblDataviewName);
            this.tpGeneral.Controls.Add(this.txtIndexName);
            this.tpGeneral.Controls.Add(this.txtKeywordCacheSize);
            this.tpGeneral.Controls.Add(this.chkEnabled);
            this.tpGeneral.Controls.Add(this.lblKeywordCacheSize);
            this.tpGeneral.Controls.Add(this.txtPrimaryKeyField);
            this.tpGeneral.Controls.Add(this.txtNodeCacheSize);
            this.tpGeneral.Controls.Add(this.lblNodeCacheSize);
            this.tpGeneral.Controls.Add(this.lblIndexName);
            this.tpGeneral.Controls.Add(this.lblPrimaryKeyField);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(545, 373);
            this.tpGeneral.TabIndex = 1;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // txtKeywordCacheSize
            // 
            this.txtKeywordCacheSize.Location = new System.Drawing.Point(172, 118);
            this.txtKeywordCacheSize.Name = "txtKeywordCacheSize";
            this.txtKeywordCacheSize.Size = new System.Drawing.Size(200, 20);
            this.txtKeywordCacheSize.TabIndex = 67;
            this.txtKeywordCacheSize.TextChanged += new System.EventHandler(this.txtKeywordCacheSize_TextChanged);
            // 
            // lblKeywordCacheSize
            // 
            this.lblKeywordCacheSize.Location = new System.Drawing.Point(6, 121);
            this.lblKeywordCacheSize.Name = "lblKeywordCacheSize";
            this.lblKeywordCacheSize.Size = new System.Drawing.Size(160, 13);
            this.lblKeywordCacheSize.TabIndex = 68;
            this.lblKeywordCacheSize.Text = "Keyword Cache Size:";
            this.lblKeywordCacheSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtNodeCacheSize
            // 
            this.txtNodeCacheSize.Location = new System.Drawing.Point(172, 92);
            this.txtNodeCacheSize.Name = "txtNodeCacheSize";
            this.txtNodeCacheSize.Size = new System.Drawing.Size(200, 20);
            this.txtNodeCacheSize.TabIndex = 65;
            this.txtNodeCacheSize.TextChanged += new System.EventHandler(this.txtNodeCacheSize_TextChanged);
            // 
            // lblNodeCacheSize
            // 
            this.lblNodeCacheSize.Location = new System.Drawing.Point(9, 95);
            this.lblNodeCacheSize.Name = "lblNodeCacheSize";
            this.lblNodeCacheSize.Size = new System.Drawing.Size(157, 13);
            this.lblNodeCacheSize.TabIndex = 66;
            this.lblNodeCacheSize.Text = "Node Cache Size:";
            this.lblNodeCacheSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tpFields
            // 
            this.tpFields.Controls.Add(this.lvFields);
            this.tpFields.Location = new System.Drawing.Point(4, 22);
            this.tpFields.Name = "tpFields";
            this.tpFields.Size = new System.Drawing.Size(545, 373);
            this.tpFields.TabIndex = 2;
            this.tpFields.Text = "Field Definitions";
            this.tpFields.UseVisualStyleBackColor = true;
            // 
            // tpResolvers
            // 
            this.tpResolvers.Controls.Add(this.lvResolvers);
            this.tpResolvers.Location = new System.Drawing.Point(4, 22);
            this.tpResolvers.Name = "tpResolvers";
            this.tpResolvers.Size = new System.Drawing.Size(545, 373);
            this.tpResolvers.TabIndex = 3;
            this.tpResolvers.Text = "Resolvers";
            this.tpResolvers.UseVisualStyleBackColor = true;
            // 
            // lvResolvers
            // 
            this.lvResolvers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvResolvers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colResolverName,
            this.colResolverEnabled,
            this.colResolverMethod,
            this.colResolverValid});
            this.lvResolvers.ContextMenuStrip = this.cmResolver;
            this.lvResolvers.FullRowSelect = true;
            this.lvResolvers.Location = new System.Drawing.Point(7, 17);
            this.lvResolvers.Name = "lvResolvers";
            this.lvResolvers.Size = new System.Drawing.Size(530, 338);
            this.lvResolvers.TabIndex = 1;
            this.lvResolvers.UseCompatibleStateImageBehavior = false;
            this.lvResolvers.View = System.Windows.Forms.View.Details;
            this.lvResolvers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvResolvers_MouseDoubleClick);
            // 
            // colResolverName
            // 
            this.colResolverName.Text = "Name";
            this.colResolverName.Width = 100;
            // 
            // colResolverEnabled
            // 
            this.colResolverEnabled.Text = "Enabled?";
            // 
            // colResolverMethod
            // 
            this.colResolverMethod.Text = "Method";
            this.colResolverMethod.Width = 100;
            // 
            // colResolverValid
            // 
            this.colResolverValid.Text = "Valid?";
            // 
            // cmResolver
            // 
            this.cmResolver.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiResolverNew,
            this.toolStripSeparator3,
            this.cmiResolverEnable,
            this.cmiResolverDisable,
            this.toolStripSeparator4,
            this.cmiResolverDelete,
            this.cmiResolverRefresh,
            this.cmiResolverProperties});
            this.cmResolver.Name = "contextMenuStrip1";
            this.cmResolver.Size = new System.Drawing.Size(155, 148);
            // 
            // cmiResolverNew
            // 
            this.cmiResolverNew.Name = "cmiResolverNew";
            this.cmiResolverNew.Size = new System.Drawing.Size(154, 22);
            this.cmiResolverNew.Text = "&New Resolver...";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(151, 6);
            // 
            // cmiResolverEnable
            // 
            this.cmiResolverEnable.Name = "cmiResolverEnable";
            this.cmiResolverEnable.Size = new System.Drawing.Size(154, 22);
            this.cmiResolverEnable.Text = "&Enable";
            this.cmiResolverEnable.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // cmiResolverDisable
            // 
            this.cmiResolverDisable.Name = "cmiResolverDisable";
            this.cmiResolverDisable.Size = new System.Drawing.Size(154, 22);
            this.cmiResolverDisable.Text = "Di&sable";
            this.cmiResolverDisable.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(151, 6);
            // 
            // cmiResolverDelete
            // 
            this.cmiResolverDelete.Name = "cmiResolverDelete";
            this.cmiResolverDelete.Size = new System.Drawing.Size(154, 22);
            this.cmiResolverDelete.Text = "&Delete";
            // 
            // cmiResolverRefresh
            // 
            this.cmiResolverRefresh.Name = "cmiResolverRefresh";
            this.cmiResolverRefresh.Size = new System.Drawing.Size(154, 22);
            this.cmiResolverRefresh.Text = "&Refresh";
            this.cmiResolverRefresh.Click += new System.EventHandler(this.toolStripMenuItem9_Click);
            // 
            // cmiResolverProperties
            // 
            this.cmiResolverProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiResolverProperties.Name = "cmiResolverProperties";
            this.cmiResolverProperties.Size = new System.Drawing.Size(154, 22);
            this.cmiResolverProperties.Text = "&Properties";
            this.cmiResolverProperties.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // tpStats
            // 
            this.tpStats.Controls.Add(this.lblErrorsSinceLastRebuild);
            this.tpStats.Controls.Add(this.lblLastRebuildEndDate);
            this.tpStats.Controls.Add(this.lblLastRebuildBeginDate);
            this.tpStats.Controls.Add(this.lblLastRebuildEndDateTitle);
            this.tpStats.Controls.Add(this.txtErrorOnRebuild);
            this.tpStats.Controls.Add(this.lblErrorOnRebuild);
            this.tpStats.Controls.Add(this.lblErrorsSinceLastRebuildTitle);
            this.tpStats.Controls.Add(this.lblLastRebuildDateTitle);
            this.tpStats.Location = new System.Drawing.Point(4, 22);
            this.tpStats.Name = "tpStats";
            this.tpStats.Size = new System.Drawing.Size(545, 373);
            this.tpStats.TabIndex = 4;
            this.tpStats.Text = "Stats";
            this.tpStats.UseVisualStyleBackColor = true;
            // 
            // lblErrorsSinceLastRebuild
            // 
            this.lblErrorsSinceLastRebuild.AutoSize = true;
            this.lblErrorsSinceLastRebuild.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblErrorsSinceLastRebuild.Location = new System.Drawing.Point(12, 91);
            this.lblErrorsSinceLastRebuild.Name = "lblErrorsSinceLastRebuild";
            this.lblErrorsSinceLastRebuild.Size = new System.Drawing.Size(12, 15);
            this.lblErrorsSinceLastRebuild.TabIndex = 10;
            this.lblErrorsSinceLastRebuild.Text = "-";
            // 
            // lblLastRebuildEndDate
            // 
            this.lblLastRebuildEndDate.AutoSize = true;
            this.lblLastRebuildEndDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLastRebuildEndDate.Location = new System.Drawing.Point(182, 42);
            this.lblLastRebuildEndDate.Name = "lblLastRebuildEndDate";
            this.lblLastRebuildEndDate.Size = new System.Drawing.Size(12, 15);
            this.lblLastRebuildEndDate.TabIndex = 9;
            this.lblLastRebuildEndDate.Text = "-";
            // 
            // lblLastRebuildBeginDate
            // 
            this.lblLastRebuildBeginDate.AutoSize = true;
            this.lblLastRebuildBeginDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLastRebuildBeginDate.Location = new System.Drawing.Point(12, 42);
            this.lblLastRebuildBeginDate.Name = "lblLastRebuildBeginDate";
            this.lblLastRebuildBeginDate.Size = new System.Drawing.Size(12, 15);
            this.lblLastRebuildBeginDate.TabIndex = 8;
            this.lblLastRebuildBeginDate.Text = "-";
            // 
            // lblLastRebuildEndDateTitle
            // 
            this.lblLastRebuildEndDateTitle.AutoSize = true;
            this.lblLastRebuildEndDateTitle.Location = new System.Drawing.Point(178, 20);
            this.lblLastRebuildEndDateTitle.Name = "lblLastRebuildEndDateTitle";
            this.lblLastRebuildEndDateTitle.Size = new System.Drawing.Size(91, 13);
            this.lblLastRebuildEndDateTitle.TabIndex = 7;
            this.lblLastRebuildEndDateTitle.Text = "Last Rebuild End:";
            // 
            // txtErrorOnRebuild
            // 
            this.txtErrorOnRebuild.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtErrorOnRebuild.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtErrorOnRebuild.Location = new System.Drawing.Point(12, 135);
            this.txtErrorOnRebuild.Multiline = true;
            this.txtErrorOnRebuild.Name = "txtErrorOnRebuild";
            this.txtErrorOnRebuild.ReadOnly = true;
            this.txtErrorOnRebuild.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtErrorOnRebuild.Size = new System.Drawing.Size(520, 226);
            this.txtErrorOnRebuild.TabIndex = 5;
            this.txtErrorOnRebuild.WordWrap = false;
            // 
            // lblErrorOnRebuild
            // 
            this.lblErrorOnRebuild.AutoSize = true;
            this.lblErrorOnRebuild.Location = new System.Drawing.Point(9, 119);
            this.lblErrorOnRebuild.Name = "lblErrorOnRebuild";
            this.lblErrorOnRebuild.Size = new System.Drawing.Size(86, 13);
            this.lblErrorOnRebuild.TabIndex = 4;
            this.lblErrorOnRebuild.Text = "Error on Rebuild:";
            // 
            // lblErrorsSinceLastRebuildTitle
            // 
            this.lblErrorsSinceLastRebuildTitle.AutoSize = true;
            this.lblErrorsSinceLastRebuildTitle.Location = new System.Drawing.Point(9, 69);
            this.lblErrorsSinceLastRebuildTitle.Name = "lblErrorsSinceLastRebuildTitle";
            this.lblErrorsSinceLastRebuildTitle.Size = new System.Drawing.Size(129, 13);
            this.lblErrorsSinceLastRebuildTitle.TabIndex = 2;
            this.lblErrorsSinceLastRebuildTitle.Text = "Errors Since Last Rebuild:";
            // 
            // lblLastRebuildDateTitle
            // 
            this.lblLastRebuildDateTitle.AutoSize = true;
            this.lblLastRebuildDateTitle.Location = new System.Drawing.Point(9, 20);
            this.lblLastRebuildDateTitle.Name = "lblLastRebuildDateTitle";
            this.lblLastRebuildDateTitle.Size = new System.Drawing.Size(99, 13);
            this.lblLastRebuildDateTitle.TabIndex = 0;
            this.lblLastRebuildDateTitle.Text = "Last Rebuild Begin:";
            // 
            // cmParameter
            // 
            this.cmParameter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiParameterNew,
            this.toolStripSeparator2,
            this.cmiParameterDelete,
            this.cmiParameterRefresh,
            this.cmiParameterProperties});
            this.cmParameter.Name = "contextMenuStrip1";
            this.cmParameter.Size = new System.Drawing.Size(165, 98);
            // 
            // cmiParameterNew
            // 
            this.cmiParameterNew.Name = "cmiParameterNew";
            this.cmiParameterNew.Size = new System.Drawing.Size(164, 22);
            this.cmiParameterNew.Text = "&New Parameter...";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(161, 6);
            // 
            // cmiParameterDelete
            // 
            this.cmiParameterDelete.Name = "cmiParameterDelete";
            this.cmiParameterDelete.Size = new System.Drawing.Size(164, 22);
            this.cmiParameterDelete.Text = "&Delete";
            // 
            // cmiParameterRefresh
            // 
            this.cmiParameterRefresh.Name = "cmiParameterRefresh";
            this.cmiParameterRefresh.Size = new System.Drawing.Size(164, 22);
            this.cmiParameterRefresh.Text = "&Refresh";
            this.cmiParameterRefresh.Click += new System.EventHandler(this.cmiParameterRefresh_Click);
            // 
            // cmiParameterProperties
            // 
            this.cmiParameterProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiParameterProperties.Name = "cmiParameterProperties";
            this.cmiParameterProperties.Size = new System.Drawing.Size(164, 22);
            this.cmiParameterProperties.Text = "&Properties";
            // 
            // tpAdvanced
            // 
            this.tpAdvanced.Controls.Add(this.gbIndexGeneration);
            this.tpAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tpAdvanced.Name = "tpAdvanced";
            this.tpAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tpAdvanced.Size = new System.Drawing.Size(545, 373);
            this.tpAdvanced.TabIndex = 5;
            this.tpAdvanced.Text = "Advanced";
            this.tpAdvanced.UseVisualStyleBackColor = true;
            // 
            // gbIndexGeneration
            // 
            this.gbIndexGeneration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbIndexGeneration.Controls.Add(this.txtEncoding);
            this.gbIndexGeneration.Controls.Add(this.lblEncoding);
            this.gbIndexGeneration.Controls.Add(this.txtMaxSortSize);
            this.gbIndexGeneration.Controls.Add(this.lblMaxSortSize);
            this.gbIndexGeneration.Controls.Add(this.txtFanoutSize);
            this.gbIndexGeneration.Controls.Add(this.lblFanoutSize);
            this.gbIndexGeneration.Controls.Add(this.txtAverageKeywordSize);
            this.gbIndexGeneration.Controls.Add(this.lblAverageKeywordSize);
            this.gbIndexGeneration.Controls.Add(this.chkStripHtml);
            this.gbIndexGeneration.Controls.Add(this.chkIndexAllTextFields);
            this.gbIndexGeneration.Location = new System.Drawing.Point(6, 6);
            this.gbIndexGeneration.Name = "gbIndexGeneration";
            this.gbIndexGeneration.Size = new System.Drawing.Size(533, 184);
            this.gbIndexGeneration.TabIndex = 58;
            this.gbIndexGeneration.TabStop = false;
            this.gbIndexGeneration.Text = "During Index Generation...";
            // 
            // txtEncoding
            // 
            this.txtEncoding.Location = new System.Drawing.Point(166, 143);
            this.txtEncoding.Name = "txtEncoding";
            this.txtEncoding.Size = new System.Drawing.Size(200, 20);
            this.txtEncoding.TabIndex = 69;
            // 
            // lblEncoding
            // 
            this.lblEncoding.Location = new System.Drawing.Point(15, 146);
            this.lblEncoding.Name = "lblEncoding";
            this.lblEncoding.Size = new System.Drawing.Size(145, 13);
            this.lblEncoding.TabIndex = 70;
            this.lblEncoding.Text = "Encoding:";
            this.lblEncoding.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtMaxSortSize
            // 
            this.txtMaxSortSize.Location = new System.Drawing.Point(166, 117);
            this.txtMaxSortSize.Name = "txtMaxSortSize";
            this.txtMaxSortSize.Size = new System.Drawing.Size(200, 20);
            this.txtMaxSortSize.TabIndex = 63;
            // 
            // lblMaxSortSize
            // 
            this.lblMaxSortSize.Location = new System.Drawing.Point(12, 120);
            this.lblMaxSortSize.Name = "lblMaxSortSize";
            this.lblMaxSortSize.Size = new System.Drawing.Size(148, 13);
            this.lblMaxSortSize.TabIndex = 64;
            this.lblMaxSortSize.Text = "Max. Sort Size (MB):";
            this.lblMaxSortSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtFanoutSize
            // 
            this.txtFanoutSize.Location = new System.Drawing.Point(166, 91);
            this.txtFanoutSize.Name = "txtFanoutSize";
            this.txtFanoutSize.Size = new System.Drawing.Size(200, 20);
            this.txtFanoutSize.TabIndex = 61;
            // 
            // lblFanoutSize
            // 
            this.lblFanoutSize.Location = new System.Drawing.Point(9, 94);
            this.lblFanoutSize.Name = "lblFanoutSize";
            this.lblFanoutSize.Size = new System.Drawing.Size(151, 13);
            this.lblFanoutSize.TabIndex = 62;
            this.lblFanoutSize.Text = "Tree Fanout Size:";
            this.lblFanoutSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtAverageKeywordSize
            // 
            this.txtAverageKeywordSize.Location = new System.Drawing.Point(166, 65);
            this.txtAverageKeywordSize.Name = "txtAverageKeywordSize";
            this.txtAverageKeywordSize.Size = new System.Drawing.Size(200, 20);
            this.txtAverageKeywordSize.TabIndex = 59;
            // 
            // lblAverageKeywordSize
            // 
            this.lblAverageKeywordSize.Location = new System.Drawing.Point(6, 68);
            this.lblAverageKeywordSize.Name = "lblAverageKeywordSize";
            this.lblAverageKeywordSize.Size = new System.Drawing.Size(154, 13);
            this.lblAverageKeywordSize.TabIndex = 60;
            this.lblAverageKeywordSize.Text = "Average Keyword Size:";
            this.lblAverageKeywordSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkStripHtml
            // 
            this.chkStripHtml.AutoSize = true;
            this.chkStripHtml.Location = new System.Drawing.Point(166, 42);
            this.chkStripHtml.Name = "chkStripHtml";
            this.chkStripHtml.Size = new System.Drawing.Size(80, 17);
            this.chkStripHtml.TabIndex = 58;
            this.chkStripHtml.Text = "Strip HTML";
            this.chkStripHtml.UseVisualStyleBackColor = true;
            // 
            // chkIndexAllTextFields
            // 
            this.chkIndexAllTextFields.AutoSize = true;
            this.chkIndexAllTextFields.Location = new System.Drawing.Point(166, 19);
            this.chkIndexAllTextFields.Name = "chkIndexAllTextFields";
            this.chkIndexAllTextFields.Size = new System.Drawing.Size(112, 17);
            this.chkIndexAllTextFields.TabIndex = 57;
            this.chkIndexAllTextFields.Text = "Index all text fields";
            this.chkIndexAllTextFields.UseVisualStyleBackColor = true;
            // 
            // btnNewDataview
            // 
            this.btnNewDataview.Location = new System.Drawing.Point(172, 199);
            this.btnNewDataview.Name = "btnNewDataview";
            this.btnNewDataview.Size = new System.Drawing.Size(108, 23);
            this.btnNewDataview.TabIndex = 78;
            this.btnNewDataview.Text = "New Dataview...";
            this.btnNewDataview.UseVisualStyleBackColor = true;
            // 
            // btnEditDataview
            // 
            this.btnEditDataview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditDataview.Location = new System.Drawing.Point(473, 172);
            this.btnEditDataview.Name = "btnEditDataview";
            this.btnEditDataview.Size = new System.Drawing.Size(59, 23);
            this.btnEditDataview.TabIndex = 77;
            this.btnEditDataview.Text = "Edit...";
            this.btnEditDataview.UseVisualStyleBackColor = true;
            // 
            // ddlSearchDataviews
            // 
            this.ddlSearchDataviews.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlSearchDataviews.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSearchDataviews.FormattingEnabled = true;
            this.ddlSearchDataviews.Location = new System.Drawing.Point(172, 172);
            this.ddlSearchDataviews.Name = "ddlSearchDataviews";
            this.ddlSearchDataviews.Size = new System.Drawing.Size(295, 21);
            this.ddlSearchDataviews.TabIndex = 76;
            // 
            // lblDataviewName
            // 
            this.lblDataviewName.Location = new System.Drawing.Point(18, 175);
            this.lblDataviewName.Name = "lblDataviewName";
            this.lblDataviewName.Size = new System.Drawing.Size(151, 13);
            this.lblDataviewName.TabIndex = 75;
            this.lblDataviewName.Text = "Dataview Name:";
            this.lblDataviewName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmSearchEngineIndex
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(581, 452);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tcIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSearchEngineIndex";
            this.Text = "Search Engine Index";
            this.Load += new System.EventHandler(this.frmPermission_Load);
            this.cmField.ResumeLayout(false);
            this.tcIndex.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            this.tpFields.ResumeLayout(false);
            this.tpResolvers.ResumeLayout(false);
            this.cmResolver.ResumeLayout(false);
            this.tpStats.ResumeLayout(false);
            this.tpStats.PerformLayout();
            this.cmParameter.ResumeLayout(false);
            this.tpAdvanced.ResumeLayout(false);
            this.gbIndexGeneration.ResumeLayout(false);
            this.gbIndexGeneration.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtIndexName;
        private System.Windows.Forms.Label lblIndexName;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.ContextMenuStrip cmField;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmiFieldProperties;
        private System.Windows.Forms.TextBox txtPrimaryKeyField;
        private System.Windows.Forms.Label lblPrimaryKeyField;
        private System.Windows.Forms.ListView lvFields;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colStoredInIndex;
        private System.Windows.Forms.ColumnHeader colSearchable;
        private System.Windows.Forms.ColumnHeader colIsBoolean;
        private System.Windows.Forms.ColumnHeader colCalculation;
        private System.Windows.Forms.TabControl tcIndex;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpFields;
        private System.Windows.Forms.TabPage tpResolvers;
        private System.Windows.Forms.TextBox txtKeywordCacheSize;
        private System.Windows.Forms.Label lblKeywordCacheSize;
        private System.Windows.Forms.TextBox txtNodeCacheSize;
        private System.Windows.Forms.Label lblNodeCacheSize;
        private System.Windows.Forms.ContextMenuStrip cmParameter;
        private System.Windows.Forms.ToolStripMenuItem cmiParameterNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmiParameterDelete;
        private System.Windows.Forms.ToolStripMenuItem cmiParameterProperties;
        private System.Windows.Forms.ColumnHeader colFormat;
        private System.Windows.Forms.ColumnHeader colTrueValue;
        private System.Windows.Forms.ListView lvResolvers;
        private System.Windows.Forms.ColumnHeader colResolverName;
        private System.Windows.Forms.ColumnHeader colResolverEnabled;
        private System.Windows.Forms.ColumnHeader colResolverMethod;
        private System.Windows.Forms.ContextMenuStrip cmResolver;
        private System.Windows.Forms.ToolStripMenuItem cmiResolverNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cmiResolverDelete;
        private System.Windows.Forms.ToolStripMenuItem cmiResolverProperties;
        private System.Windows.Forms.ToolStripMenuItem cmiResolverEnable;
        private System.Windows.Forms.ToolStripMenuItem cmiResolverDisable;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem cmiResolverRefresh;
        private System.Windows.Forms.ColumnHeader colResolverValid;
        private System.Windows.Forms.TabPage tpStats;
        private System.Windows.Forms.TextBox txtErrorOnRebuild;
        private System.Windows.Forms.Label lblErrorOnRebuild;
        private System.Windows.Forms.Label lblErrorsSinceLastRebuildTitle;
        private System.Windows.Forms.Label lblLastRebuildDateTitle;
        private System.Windows.Forms.Label lblLastRebuildEndDateTitle;
        private System.Windows.Forms.Label lblErrorsSinceLastRebuild;
        private System.Windows.Forms.Label lblLastRebuildEndDate;
        private System.Windows.Forms.Label lblLastRebuildBeginDate;
        private System.Windows.Forms.ToolStripMenuItem cmiFieldRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiParameterRefresh;
        private System.Windows.Forms.Button btnNewDataview;
        private System.Windows.Forms.Button btnEditDataview;
        private System.Windows.Forms.ComboBox ddlSearchDataviews;
        private System.Windows.Forms.Label lblDataviewName;
        private System.Windows.Forms.TabPage tpAdvanced;
        private System.Windows.Forms.GroupBox gbIndexGeneration;
        private System.Windows.Forms.TextBox txtEncoding;
        private System.Windows.Forms.Label lblEncoding;
        private System.Windows.Forms.TextBox txtMaxSortSize;
        private System.Windows.Forms.Label lblMaxSortSize;
        private System.Windows.Forms.TextBox txtFanoutSize;
        private System.Windows.Forms.Label lblFanoutSize;
        private System.Windows.Forms.TextBox txtAverageKeywordSize;
        private System.Windows.Forms.Label lblAverageKeywordSize;
        private System.Windows.Forms.CheckBox chkStripHtml;
        private System.Windows.Forms.CheckBox chkIndexAllTextFields;
    }
}
