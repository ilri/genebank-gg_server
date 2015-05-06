namespace GrinGlobal.Admin.ChildForms {
    partial class frmPermission {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPermission));
            this.lblDataview = new System.Windows.Forms.Label();
            this.ddlDataView = new System.Windows.Forms.ComboBox();
            this.ddlTable = new System.Windows.Forms.ComboBox();
            this.lblTable = new System.Windows.Forms.Label();
            this.ddlCreate = new System.Windows.Forms.ComboBox();
            this.lblCreate = new System.Windows.Forms.Label();
            this.ddlRead = new System.Windows.Forms.ComboBox();
            this.lblRead = new System.Windows.Forms.Label();
            this.ddlUpdate = new System.Windows.Forms.ComboBox();
            this.lblUpdate = new System.Windows.Forms.Label();
            this.ddlDelete = new System.Windows.Forms.ComboBox();
            this.lblDelete = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAddRestriction = new System.Windows.Forms.Button();
            this.lvPermFields = new System.Windows.Forms.ListView();
            this.colResourceName = new System.Windows.Forms.ColumnHeader();
            this.colFieldName = new System.Windows.Forms.ColumnHeader();
            this.colCompareOperator = new System.Windows.Forms.ColumnHeader();
            this.colValue = new System.Windows.Forms.ColumnHeader();
            this.cmPermission = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiPermissionExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.cmPermission.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDataview
            // 
            this.lblDataview.Location = new System.Drawing.Point(13, 113);
            this.lblDataview.Name = "lblDataview";
            this.lblDataview.Size = new System.Drawing.Size(112, 13);
            this.lblDataview.TabIndex = 0;
            this.lblDataview.Text = "Applies To Data View:";
            this.lblDataview.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ddlDataView
            // 
            this.ddlDataView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlDataView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataView.FormattingEnabled = true;
            this.ddlDataView.Location = new System.Drawing.Point(131, 110);
            this.ddlDataView.Name = "ddlDataView";
            this.ddlDataView.Size = new System.Drawing.Size(328, 21);
            this.ddlDataView.TabIndex = 3;
            this.ddlDataView.SelectedIndexChanged += new System.EventHandler(this.ddlDataView_SelectedIndexChanged);
            // 
            // ddlTable
            // 
            this.ddlTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTable.FormattingEnabled = true;
            this.ddlTable.Location = new System.Drawing.Point(131, 137);
            this.ddlTable.Name = "ddlTable";
            this.ddlTable.Size = new System.Drawing.Size(328, 21);
            this.ddlTable.TabIndex = 4;
            this.ddlTable.SelectedIndexChanged += new System.EventHandler(this.ddlTable_SelectedIndexChanged);
            // 
            // lblTable
            // 
            this.lblTable.Location = new System.Drawing.Point(12, 140);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(113, 13);
            this.lblTable.TabIndex = 2;
            this.lblTable.Text = "Applies To Table:";
            this.lblTable.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ddlCreate
            // 
            this.ddlCreate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCreate.FormattingEnabled = true;
            this.ddlCreate.Items.AddRange(new object[] {
            "Inherit",
            "Deny",
            "Allow"});
            this.ddlCreate.Location = new System.Drawing.Point(36, 208);
            this.ddlCreate.Name = "ddlCreate";
            this.ddlCreate.Size = new System.Drawing.Size(91, 21);
            this.ddlCreate.TabIndex = 6;
            this.ddlCreate.SelectedIndexChanged += new System.EventHandler(this.ddlCreate_SelectedIndexChanged);
            // 
            // lblCreate
            // 
            this.lblCreate.Location = new System.Drawing.Point(33, 192);
            this.lblCreate.Name = "lblCreate";
            this.lblCreate.Size = new System.Drawing.Size(91, 13);
            this.lblCreate.TabIndex = 4;
            this.lblCreate.Text = "Create:";
            // 
            // ddlRead
            // 
            this.ddlRead.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRead.FormattingEnabled = true;
            this.ddlRead.Items.AddRange(new object[] {
            "Inherit",
            "Deny",
            "Allow"});
            this.ddlRead.Location = new System.Drawing.Point(133, 208);
            this.ddlRead.Name = "ddlRead";
            this.ddlRead.Size = new System.Drawing.Size(91, 21);
            this.ddlRead.TabIndex = 7;
            this.ddlRead.SelectedIndexChanged += new System.EventHandler(this.ddlRead_SelectedIndexChanged);
            // 
            // lblRead
            // 
            this.lblRead.Location = new System.Drawing.Point(130, 192);
            this.lblRead.Name = "lblRead";
            this.lblRead.Size = new System.Drawing.Size(91, 13);
            this.lblRead.TabIndex = 6;
            this.lblRead.Text = "Read:";
            // 
            // ddlUpdate
            // 
            this.ddlUpdate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlUpdate.FormattingEnabled = true;
            this.ddlUpdate.Items.AddRange(new object[] {
            "Inherit",
            "Deny",
            "Allow"});
            this.ddlUpdate.Location = new System.Drawing.Point(230, 208);
            this.ddlUpdate.Name = "ddlUpdate";
            this.ddlUpdate.Size = new System.Drawing.Size(91, 21);
            this.ddlUpdate.TabIndex = 8;
            this.ddlUpdate.SelectedIndexChanged += new System.EventHandler(this.ddlUpdate_SelectedIndexChanged);
            // 
            // lblUpdate
            // 
            this.lblUpdate.Location = new System.Drawing.Point(227, 192);
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(91, 13);
            this.lblUpdate.TabIndex = 8;
            this.lblUpdate.Text = "Update:";
            // 
            // ddlDelete
            // 
            this.ddlDelete.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDelete.FormattingEnabled = true;
            this.ddlDelete.Items.AddRange(new object[] {
            "Inherit",
            "Deny",
            "Allow"});
            this.ddlDelete.Location = new System.Drawing.Point(327, 208);
            this.ddlDelete.Name = "ddlDelete";
            this.ddlDelete.Size = new System.Drawing.Size(91, 21);
            this.ddlDelete.TabIndex = 9;
            this.ddlDelete.SelectedIndexChanged += new System.EventHandler(this.ddlDelete_SelectedIndexChanged);
            // 
            // lblDelete
            // 
            this.lblDelete.Location = new System.Drawing.Point(324, 192);
            this.lblDelete.Name = "lblDelete";
            this.lblDelete.Size = new System.Drawing.Size(94, 13);
            this.lblDelete.TabIndex = 10;
            this.lblDelete.Text = "Delete:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(384, 411);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(303, 411);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(131, 13);
            this.txtName.MaxLength = 500;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(328, 20);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(12, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(113, 13);
            this.lblName.TabIndex = 15;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Checked = true;
            this.chkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnabled.Location = new System.Drawing.Point(131, 164);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkEnabled.TabIndex = 5;
            this.chkEnabled.Text = "Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnAddRestriction);
            this.groupBox1.Controls.Add(this.lvPermFields);
            this.groupBox1.Location = new System.Drawing.Point(12, 241);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(447, 164);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Restricted To:";
            // 
            // btnAddRestriction
            // 
            this.btnAddRestriction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRestriction.Location = new System.Drawing.Point(366, 135);
            this.btnAddRestriction.Name = "btnAddRestriction";
            this.btnAddRestriction.Size = new System.Drawing.Size(75, 23);
            this.btnAddRestriction.TabIndex = 11;
            this.btnAddRestriction.Text = "Add...";
            this.btnAddRestriction.UseVisualStyleBackColor = true;
            this.btnAddRestriction.Click += new System.EventHandler(this.btnAddRestriction_Click);
            // 
            // lvPermFields
            // 
            this.lvPermFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPermFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colResourceName,
            this.colFieldName,
            this.colCompareOperator,
            this.colValue});
            this.lvPermFields.ContextMenuStrip = this.cmPermission;
            this.lvPermFields.FullRowSelect = true;
            this.lvPermFields.HideSelection = false;
            this.lvPermFields.Location = new System.Drawing.Point(6, 19);
            this.lvPermFields.Name = "lvPermFields";
            this.lvPermFields.Size = new System.Drawing.Size(435, 112);
            this.lvPermFields.TabIndex = 10;
            this.lvPermFields.UseCompatibleStateImageBehavior = false;
            this.lvPermFields.View = System.Windows.Forms.View.Details;
            this.lvPermFields.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvPermFields_MouseDoubleClick);
            this.lvPermFields.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvPermFields_KeyUp);
            // 
            // colResourceName
            // 
            this.colResourceName.Text = "Resource";
            this.colResourceName.Width = 135;
            // 
            // colFieldName
            // 
            this.colFieldName.Text = "Field";
            this.colFieldName.Width = 100;
            // 
            // colCompareOperator
            // 
            this.colCompareOperator.Text = "Compare";
            // 
            // colValue
            // 
            this.colValue.Text = "Value";
            this.colValue.Width = 120;
            // 
            // cmPermission
            // 
            this.cmPermission.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiNew,
            this.toolStripSeparator1,
            this.cmiDelete,
            this.toolStripSeparator2,
            this.cmiPermissionExportList,
            this.cmiProperties});
            this.cmPermission.Name = "contextMenuStrip1";
            this.cmPermission.Size = new System.Drawing.Size(167, 104);
            this.cmPermission.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // cmiNew
            // 
            this.cmiNew.Name = "cmiNew";
            this.cmiNew.Size = new System.Drawing.Size(166, 22);
            this.cmiNew.Text = "New Restriction...";
            this.cmiNew.Click += new System.EventHandler(this.newRestrictionToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // cmiDelete
            // 
            this.cmiDelete.Name = "cmiDelete";
            this.cmiDelete.Size = new System.Drawing.Size(166, 22);
            this.cmiDelete.Text = "&Delete";
            this.cmiDelete.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(163, 6);
            // 
            // cmiPermissionExportList
            // 
            this.cmiPermissionExportList.Name = "cmiPermissionExportList";
            this.cmiPermissionExportList.Size = new System.Drawing.Size(166, 22);
            this.cmiPermissionExportList.Text = "E&xport List";
            this.cmiPermissionExportList.Click += new System.EventHandler(this.cmiPermissionExportList_Click);
            // 
            // cmiProperties
            // 
            this.cmiProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiProperties.Name = "cmiProperties";
            this.cmiProperties.Size = new System.Drawing.Size(166, 22);
            this.cmiProperties.Text = "&Properties";
            this.cmiProperties.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(131, 67);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(328, 37);
            this.txtDescription.TabIndex = 2;
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            this.txtDescription.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtDescription_PreviewKeyDown);
            this.txtDescription.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDescription_KeyDown);
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(12, 70);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(113, 13);
            this.lblDescription.TabIndex = 53;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtCode
            // 
            this.txtCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCode.Location = new System.Drawing.Point(131, 41);
            this.txtCode.MaxLength = 50;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(328, 20);
            this.txtCode.TabIndex = 1;
            this.txtCode.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
            // 
            // lblCode
            // 
            this.lblCode.Location = new System.Drawing.Point(14, 44);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(113, 13);
            this.lblCode.TabIndex = 55;
            this.lblCode.Text = "Tag:";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmPermission
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(472, 446);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.chkEnabled);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblDataview);
            this.Controls.Add(this.ddlUpdate);
            this.Controls.Add(this.ddlDelete);
            this.Controls.Add(this.lblDelete);
            this.Controls.Add(this.lblUpdate);
            this.Controls.Add(this.ddlTable);
            this.Controls.Add(this.ddlRead);
            this.Controls.Add(this.lblRead);
            this.Controls.Add(this.ddlCreate);
            this.Controls.Add(this.lblCreate);
            this.Controls.Add(this.ddlDataView);
            this.Controls.Add(this.lblTable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPermission";
            this.Text = "Permission";
            this.Load += new System.EventHandler(this.frmPermission_Load);
            this.groupBox1.ResumeLayout(false);
            this.cmPermission.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDataview;
        private System.Windows.Forms.ComboBox ddlDataView;
        private System.Windows.Forms.ComboBox ddlTable;
        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.ComboBox ddlCreate;
        private System.Windows.Forms.Label lblCreate;
        private System.Windows.Forms.ComboBox ddlRead;
        private System.Windows.Forms.Label lblRead;
        private System.Windows.Forms.ComboBox ddlUpdate;
        private System.Windows.Forms.Label lblUpdate;
        private System.Windows.Forms.ComboBox ddlDelete;
        private System.Windows.Forms.Label lblDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAddRestriction;
        private System.Windows.Forms.ListView lvPermFields;
        private System.Windows.Forms.ColumnHeader colResourceName;
        private System.Windows.Forms.ColumnHeader colFieldName;
        private System.Windows.Forms.ColumnHeader colCompareOperator;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.ContextMenuStrip cmPermission;
        private System.Windows.Forms.ToolStripMenuItem cmiNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmiDelete;
        private System.Windows.Forms.ToolStripMenuItem cmiProperties;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmiPermissionExportList;
    }
}
