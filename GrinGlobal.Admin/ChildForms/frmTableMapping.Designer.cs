namespace GrinGlobal.Admin.ChildForms {
    partial class frmTableMapping {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTableMapping));
            this.txtDatabaseArea = new System.Windows.Forms.TextBox();
            this.lblDatabaseArea = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.lvFieldMappings = new System.Windows.Forms.ListView();
            this.colFieldName = new System.Windows.Forms.ColumnHeader();
            this.colPurpose = new System.Windows.Forms.ColumnHeader();
            this.colType = new System.Windows.Forms.ColumnHeader();
            this.colGuiHint = new System.Windows.Forms.ColumnHeader();
            this.colDefaultValue = new System.Windows.Forms.ColumnHeader();
            this.colRequired = new System.Windows.Forms.ColumnHeader();
            this.colReadOnly = new System.Windows.Forms.ColumnHeader();
            this.colFK = new System.Windows.Forms.ColumnHeader();
            this.colFKField = new System.Windows.Forms.ColumnHeader();
            this.colLookupPickerSource = new System.Windows.Forms.ColumnHeader();
            this.colDropDownSource = new System.Windows.Forms.ColumnHeader();
            this.cmField = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiFieldShowDependencies = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiFieldExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tc = new System.Windows.Forms.TabControl();
            this.tpFields = new System.Windows.Forms.TabPage();
            this.tpRelationships = new System.Windows.Forms.TabPage();
            this.lvRelationships = new System.Windows.Forms.ListView();
            this.colChildField = new System.Windows.Forms.ColumnHeader();
            this.colRelationshipType = new System.Windows.Forms.ColumnHeader();
            this.colParentField = new System.Windows.Forms.ColumnHeader();
            this.cmRelationship = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiRelationshipNew = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRelationshipAutoCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRelationshipDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRelationshipExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRelationshipRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRelationshipProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.tpIndexes = new System.Windows.Forms.TabPage();
            this.lvIndexes = new System.Windows.Forms.ListView();
            this.colIndexName = new System.Windows.Forms.ColumnHeader();
            this.colIndexUnique = new System.Windows.Forms.ColumnHeader();
            this.colIndexFields = new System.Windows.Forms.ColumnHeader();
            this.cmIndex = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiIndexInspect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiIndexDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiIndexExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiIndexRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiIndexProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.cmField.SuspendLayout();
            this.tc.SuspendLayout();
            this.tpFields.SuspendLayout();
            this.tpRelationships.SuspendLayout();
            this.cmRelationship.SuspendLayout();
            this.tpIndexes.SuspendLayout();
            this.cmIndex.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDatabaseArea
            // 
            this.txtDatabaseArea.Location = new System.Drawing.Point(131, 13);
            this.txtDatabaseArea.MaxLength = 100;
            this.txtDatabaseArea.Name = "txtDatabaseArea";
            this.txtDatabaseArea.Size = new System.Drawing.Size(198, 20);
            this.txtDatabaseArea.TabIndex = 0;
            this.txtDatabaseArea.TextChanged += new System.EventHandler(this.txtDatabaseArea_TextChanged);
            // 
            // lblDatabaseArea
            // 
            this.lblDatabaseArea.Location = new System.Drawing.Point(12, 16);
            this.lblDatabaseArea.Name = "lblDatabaseArea";
            this.lblDatabaseArea.Size = new System.Drawing.Size(113, 13);
            this.lblDatabaseArea.TabIndex = 15;
            this.lblDatabaseArea.Text = "Database Area:";
            this.lblDatabaseArea.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Location = new System.Drawing.Point(388, 16);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkEnabled.TabIndex = 52;
            this.chkEnabled.Text = "Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // lvFieldMappings
            // 
            this.lvFieldMappings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFieldMappings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFieldName,
            this.colPurpose,
            this.colType,
            this.colGuiHint,
            this.colDefaultValue,
            this.colRequired,
            this.colReadOnly,
            this.colFK,
            this.colFKField,
            this.colLookupPickerSource,
            this.colDropDownSource});
            this.lvFieldMappings.ContextMenuStrip = this.cmField;
            this.lvFieldMappings.FullRowSelect = true;
            this.lvFieldMappings.Location = new System.Drawing.Point(0, 0);
            this.lvFieldMappings.Name = "lvFieldMappings";
            this.lvFieldMappings.Size = new System.Drawing.Size(623, 294);
            this.lvFieldMappings.SmallImageList = this.imageList1;
            this.lvFieldMappings.TabIndex = 54;
            this.lvFieldMappings.UseCompatibleStateImageBehavior = false;
            this.lvFieldMappings.View = System.Windows.Forms.View.Details;
            this.lvFieldMappings.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvFieldMappings_MouseDoubleClick);
            this.lvFieldMappings.SelectedIndexChanged += new System.EventHandler(this.lvFieldMappings_SelectedIndexChanged);
            this.lvFieldMappings.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvFieldMappings_KeyPress);
            this.lvFieldMappings.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvFieldMappings_KeyUp);
            // 
            // colFieldName
            // 
            this.colFieldName.Text = "Name";
            this.colFieldName.Width = 100;
            // 
            // colPurpose
            // 
            this.colPurpose.Text = "Purpose";
            this.colPurpose.Width = 70;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            this.colType.Width = 70;
            // 
            // colGuiHint
            // 
            this.colGuiHint.Text = "User Interface";
            this.colGuiHint.Width = 100;
            // 
            // colDefaultValue
            // 
            this.colDefaultValue.Text = "Default";
            // 
            // colRequired
            // 
            this.colRequired.Text = "Required?";
            this.colRequired.Width = 70;
            // 
            // colReadOnly
            // 
            this.colReadOnly.Text = "Read Only?";
            this.colReadOnly.Width = 70;
            // 
            // colFK
            // 
            this.colFK.Text = "Foreign Key?";
            // 
            // colFKField
            // 
            this.colFKField.Text = "Foreign Key Field";
            this.colFKField.Width = 100;
            // 
            // colLookupPickerSource
            // 
            this.colLookupPickerSource.Text = "Lookup Picker Source";
            this.colLookupPickerSource.Width = 100;
            // 
            // colDropDownSource
            // 
            this.colDropDownSource.Text = "Drop Down Source";
            this.colDropDownSource.Width = 70;
            // 
            // cmField
            // 
            this.cmField.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiNew,
            this.cmiFieldShowDependencies,
            this.toolStripSeparator1,
            this.cmiDelete,
            this.toolStripSeparator18,
            this.cmiFieldExportList,
            this.cmiRefresh,
            this.cmiProperties});
            this.cmField.Name = "ctxMenuNodeUser";
            this.cmField.Size = new System.Drawing.Size(190, 148);
            this.cmField.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuFieldMapping_Opening);
            // 
            // cmiNew
            // 
            this.cmiNew.Name = "cmiNew";
            this.cmiNew.Size = new System.Drawing.Size(189, 22);
            this.cmiNew.Text = "&New Field...";
            this.cmiNew.Click += new System.EventHandler(this.newTableMappingMenuItem_Click);
            // 
            // cmiFieldShowDependencies
            // 
            this.cmiFieldShowDependencies.Name = "cmiFieldShowDependencies";
            this.cmiFieldShowDependencies.Size = new System.Drawing.Size(189, 22);
            this.cmiFieldShowDependencies.Text = "&Show Dependencies...";
            this.cmiFieldShowDependencies.Click += new System.EventHandler(this.cmiFieldShowDependencies_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(186, 6);
            // 
            // cmiDelete
            // 
            this.cmiDelete.Name = "cmiDelete";
            this.cmiDelete.Size = new System.Drawing.Size(189, 22);
            this.cmiDelete.Text = "&Delete";
            this.cmiDelete.Click += new System.EventHandler(this.deleteTableMappingsMenuItem_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(186, 6);
            // 
            // cmiFieldExportList
            // 
            this.cmiFieldExportList.Name = "cmiFieldExportList";
            this.cmiFieldExportList.Size = new System.Drawing.Size(189, 22);
            this.cmiFieldExportList.Text = "E&xport List";
            this.cmiFieldExportList.Click += new System.EventHandler(this.cmiFieldExportList_Click);
            // 
            // cmiRefresh
            // 
            this.cmiRefresh.Name = "cmiRefresh";
            this.cmiRefresh.Size = new System.Drawing.Size(189, 22);
            this.cmiRefresh.Text = "&Refresh";
            this.cmiRefresh.Click += new System.EventHandler(this.refreshPermissionMenuItem_Click);
            // 
            // cmiProperties
            // 
            this.cmiProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiProperties.Name = "cmiProperties";
            this.cmiProperties.Size = new System.Drawing.Size(189, 22);
            this.cmiProperties.Text = "&Properties";
            this.cmiProperties.Click += new System.EventHandler(this.defaultPermissionMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "table-field.ico");
            this.imageList1.Images.SetKeyName(1, "relationships.ico");
            this.imageList1.Images.SetKeyName(2, "table-index.ico");
            this.imageList1.Images.SetKeyName(3, "table-field-req.ico");
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(484, 365);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 57;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(565, 365);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 58;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // tc
            // 
            this.tc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tc.Controls.Add(this.tpFields);
            this.tc.Controls.Add(this.tpRelationships);
            this.tc.Controls.Add(this.tpIndexes);
            this.tc.Location = new System.Drawing.Point(9, 39);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(631, 320);
            this.tc.TabIndex = 59;
            // 
            // tpFields
            // 
            this.tpFields.Controls.Add(this.lvFieldMappings);
            this.tpFields.Location = new System.Drawing.Point(4, 22);
            this.tpFields.Name = "tpFields";
            this.tpFields.Padding = new System.Windows.Forms.Padding(3);
            this.tpFields.Size = new System.Drawing.Size(623, 294);
            this.tpFields.TabIndex = 0;
            this.tpFields.Text = "Fields";
            this.tpFields.UseVisualStyleBackColor = true;
            // 
            // tpRelationships
            // 
            this.tpRelationships.Controls.Add(this.lvRelationships);
            this.tpRelationships.Location = new System.Drawing.Point(4, 22);
            this.tpRelationships.Name = "tpRelationships";
            this.tpRelationships.Padding = new System.Windows.Forms.Padding(3);
            this.tpRelationships.Size = new System.Drawing.Size(623, 294);
            this.tpRelationships.TabIndex = 1;
            this.tpRelationships.Text = "Relationships";
            this.tpRelationships.UseVisualStyleBackColor = true;
            // 
            // lvRelationships
            // 
            this.lvRelationships.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvRelationships.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colChildField,
            this.colRelationshipType,
            this.colParentField});
            this.lvRelationships.ContextMenuStrip = this.cmRelationship;
            this.lvRelationships.FullRowSelect = true;
            this.lvRelationships.Location = new System.Drawing.Point(0, 0);
            this.lvRelationships.Name = "lvRelationships";
            this.lvRelationships.Size = new System.Drawing.Size(623, 294);
            this.lvRelationships.SmallImageList = this.imageList1;
            this.lvRelationships.TabIndex = 55;
            this.lvRelationships.UseCompatibleStateImageBehavior = false;
            this.lvRelationships.View = System.Windows.Forms.View.Details;
            this.lvRelationships.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvRelationships_MouseDoubleClick);
            this.lvRelationships.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvRelationships_KeyPress);
            this.lvRelationships.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvRelationships_KeyUp);
            // 
            // colChildField
            // 
            this.colChildField.Text = "Child";
            this.colChildField.Width = 200;
            // 
            // colRelationshipType
            // 
            this.colRelationshipType.Text = "Type";
            this.colRelationshipType.Width = 150;
            // 
            // colParentField
            // 
            this.colParentField.Text = "Parent";
            this.colParentField.Width = 200;
            // 
            // cmRelationship
            // 
            this.cmRelationship.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiRelationshipNew,
            this.cmiRelationshipAutoCreate,
            this.toolStripSeparator2,
            this.cmiRelationshipDelete,
            this.toolStripSeparator3,
            this.cmiRelationshipExportList,
            this.cmiRelationshipRefresh,
            this.cmiRelationshipProperties});
            this.cmRelationship.Name = "ctxMenuNodeUser";
            this.cmRelationship.Size = new System.Drawing.Size(246, 148);
            this.cmRelationship.Opening += new System.ComponentModel.CancelEventHandler(this.cmRelationship_Opening);
            // 
            // cmiRelationshipNew
            // 
            this.cmiRelationshipNew.Name = "cmiRelationshipNew";
            this.cmiRelationshipNew.Size = new System.Drawing.Size(245, 22);
            this.cmiRelationshipNew.Text = "&New Relationship...";
            this.cmiRelationshipNew.Click += new System.EventHandler(this.cmiRelationshipNew_Click);
            // 
            // cmiRelationshipAutoCreate
            // 
            this.cmiRelationshipAutoCreate.Name = "cmiRelationshipAutoCreate";
            this.cmiRelationshipAutoCreate.Size = new System.Drawing.Size(245, 22);
            this.cmiRelationshipAutoCreate.Text = "&Inspect and Create Relationships";
            this.cmiRelationshipAutoCreate.Click += new System.EventHandler(this.cmiRelationshipAutoCreate_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(242, 6);
            // 
            // cmiRelationshipDelete
            // 
            this.cmiRelationshipDelete.Name = "cmiRelationshipDelete";
            this.cmiRelationshipDelete.Size = new System.Drawing.Size(245, 22);
            this.cmiRelationshipDelete.Text = "&Delete";
            this.cmiRelationshipDelete.Click += new System.EventHandler(this.cmiRelationshipDelete_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(242, 6);
            // 
            // cmiRelationshipExportList
            // 
            this.cmiRelationshipExportList.Name = "cmiRelationshipExportList";
            this.cmiRelationshipExportList.Size = new System.Drawing.Size(245, 22);
            this.cmiRelationshipExportList.Text = "E&xport List";
            this.cmiRelationshipExportList.Click += new System.EventHandler(this.cmiRelationshipExportList_Click);
            // 
            // cmiRelationshipRefresh
            // 
            this.cmiRelationshipRefresh.Name = "cmiRelationshipRefresh";
            this.cmiRelationshipRefresh.Size = new System.Drawing.Size(245, 22);
            this.cmiRelationshipRefresh.Text = "&Refresh";
            this.cmiRelationshipRefresh.Click += new System.EventHandler(this.cmiRelationshipRefresh_Click);
            // 
            // cmiRelationshipProperties
            // 
            this.cmiRelationshipProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiRelationshipProperties.Name = "cmiRelationshipProperties";
            this.cmiRelationshipProperties.Size = new System.Drawing.Size(245, 22);
            this.cmiRelationshipProperties.Text = "&Properties";
            this.cmiRelationshipProperties.Click += new System.EventHandler(this.cmiRelationshipProperties_Click);
            // 
            // tpIndexes
            // 
            this.tpIndexes.Controls.Add(this.lvIndexes);
            this.tpIndexes.Location = new System.Drawing.Point(4, 22);
            this.tpIndexes.Name = "tpIndexes";
            this.tpIndexes.Padding = new System.Windows.Forms.Padding(3);
            this.tpIndexes.Size = new System.Drawing.Size(623, 294);
            this.tpIndexes.TabIndex = 2;
            this.tpIndexes.Text = "Indexes";
            this.tpIndexes.UseVisualStyleBackColor = true;
            // 
            // lvIndexes
            // 
            this.lvIndexes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvIndexes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIndexName,
            this.colIndexUnique,
            this.colIndexFields});
            this.lvIndexes.ContextMenuStrip = this.cmIndex;
            this.lvIndexes.FullRowSelect = true;
            this.lvIndexes.Location = new System.Drawing.Point(0, 0);
            this.lvIndexes.Name = "lvIndexes";
            this.lvIndexes.Size = new System.Drawing.Size(623, 294);
            this.lvIndexes.SmallImageList = this.imageList1;
            this.lvIndexes.TabIndex = 56;
            this.lvIndexes.UseCompatibleStateImageBehavior = false;
            this.lvIndexes.View = System.Windows.Forms.View.Details;
            // 
            // colIndexName
            // 
            this.colIndexName.Text = "Name";
            this.colIndexName.Width = 200;
            // 
            // colIndexUnique
            // 
            this.colIndexUnique.Text = "Is Unique?";
            this.colIndexUnique.Width = 80;
            // 
            // colIndexFields
            // 
            this.colIndexFields.Text = "Fields";
            this.colIndexFields.Width = 300;
            // 
            // cmIndex
            // 
            this.cmIndex.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiIndexInspect,
            this.toolStripSeparator4,
            this.cmiIndexDelete,
            this.toolStripSeparator5,
            this.cmiIndexExportList,
            this.cmiIndexRefresh,
            this.cmiIndexProperties});
            this.cmIndex.Name = "ctxMenuNodeUser";
            this.cmIndex.Size = new System.Drawing.Size(224, 126);
            this.cmIndex.Opening += new System.ComponentModel.CancelEventHandler(this.cmIndex_Opening);
            // 
            // cmiIndexInspect
            // 
            this.cmiIndexInspect.Name = "cmiIndexInspect";
            this.cmiIndexInspect.Size = new System.Drawing.Size(223, 22);
            this.cmiIndexInspect.Text = "&Inspect and Create Indexes...";
            this.cmiIndexInspect.Click += new System.EventHandler(this.cmiIndexInspect_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(220, 6);
            // 
            // cmiIndexDelete
            // 
            this.cmiIndexDelete.Name = "cmiIndexDelete";
            this.cmiIndexDelete.Size = new System.Drawing.Size(223, 22);
            this.cmiIndexDelete.Text = "&Delete";
            this.cmiIndexDelete.Click += new System.EventHandler(this.cmiIndexDelete_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(220, 6);
            // 
            // cmiIndexExportList
            // 
            this.cmiIndexExportList.Name = "cmiIndexExportList";
            this.cmiIndexExportList.Size = new System.Drawing.Size(223, 22);
            this.cmiIndexExportList.Text = "E&xport List";
            this.cmiIndexExportList.Click += new System.EventHandler(this.cmiIndexExportList_Click);
            // 
            // cmiIndexRefresh
            // 
            this.cmiIndexRefresh.Name = "cmiIndexRefresh";
            this.cmiIndexRefresh.Size = new System.Drawing.Size(223, 22);
            this.cmiIndexRefresh.Text = "&Refresh";
            this.cmiIndexRefresh.Click += new System.EventHandler(this.cmiIndexRefresh_Click);
            // 
            // cmiIndexProperties
            // 
            this.cmiIndexProperties.Enabled = false;
            this.cmiIndexProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiIndexProperties.Name = "cmiIndexProperties";
            this.cmiIndexProperties.Size = new System.Drawing.Size(223, 22);
            this.cmiIndexProperties.Text = "&Properties";
            this.cmiIndexProperties.Click += new System.EventHandler(this.cmiIndexProperties_Click);
            // 
            // frmTableMapping
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(652, 400);
            this.Controls.Add(this.tc);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkEnabled);
            this.Controls.Add(this.txtDatabaseArea);
            this.Controls.Add(this.lblDatabaseArea);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTableMapping";
            this.Text = "Table Mapping Detail";
            this.Load += new System.EventHandler(this.frmTableMapping_Load);
            this.cmField.ResumeLayout(false);
            this.tc.ResumeLayout(false);
            this.tpFields.ResumeLayout(false);
            this.tpRelationships.ResumeLayout(false);
            this.cmRelationship.ResumeLayout(false);
            this.tpIndexes.ResumeLayout(false);
            this.cmIndex.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDatabaseArea;
        private System.Windows.Forms.Label lblDatabaseArea;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.ListView lvFieldMappings;
        private System.Windows.Forms.ContextMenuStrip cmField;
        private System.Windows.Forms.ToolStripMenuItem cmiNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem cmiRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiProperties;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ColumnHeader colFieldName;
        private System.Windows.Forms.ColumnHeader colPurpose;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colDefaultValue;
        private System.Windows.Forms.ColumnHeader colFK;
        private System.Windows.Forms.ColumnHeader colRequired;
        private System.Windows.Forms.ColumnHeader colReadOnly;
        private System.Windows.Forms.ColumnHeader colDropDownSource;
        private System.Windows.Forms.ColumnHeader colFKField;
        private System.Windows.Forms.ColumnHeader colLookupPickerSource;
        private System.Windows.Forms.ColumnHeader colGuiHint;
        private System.Windows.Forms.TabControl tc;
        private System.Windows.Forms.TabPage tpFields;
        private System.Windows.Forms.TabPage tpRelationships;
        private System.Windows.Forms.ListView lvRelationships;
        private System.Windows.Forms.ColumnHeader colChildField;
        private System.Windows.Forms.ColumnHeader colRelationshipType;
        private System.Windows.Forms.ColumnHeader colParentField;
        private System.Windows.Forms.ContextMenuStrip cmRelationship;
        private System.Windows.Forms.ToolStripMenuItem cmiRelationshipNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmiRelationshipDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cmiRelationshipRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiRelationshipProperties;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem cmiRelationshipAutoCreate;
        private System.Windows.Forms.TabPage tpIndexes;
        private System.Windows.Forms.ListView lvIndexes;
        private System.Windows.Forms.ColumnHeader colIndexName;
        private System.Windows.Forms.ColumnHeader colIndexUnique;
        private System.Windows.Forms.ColumnHeader colIndexFields;
        private System.Windows.Forms.ContextMenuStrip cmIndex;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexInspect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexProperties;
        private System.Windows.Forms.ToolStripMenuItem cmiFieldExportList;
        private System.Windows.Forms.ToolStripMenuItem cmiRelationshipExportList;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexExportList;
        private System.Windows.Forms.ToolStripMenuItem cmiFieldShowDependencies;
    }
}
