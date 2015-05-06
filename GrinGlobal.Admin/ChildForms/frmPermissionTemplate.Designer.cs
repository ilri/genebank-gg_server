namespace GrinGlobal.Admin.ChildForms {
    partial class frmPermissionTemplate {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPermissionTemplate));
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbAssignedPermissions = new System.Windows.Forms.GroupBox();
            this.btnAddPermission = new System.Windows.Forms.Button();
            this.lvPermissions = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colResource = new System.Windows.Forms.ColumnHeader();
            this.colCreate = new System.Windows.Forms.ColumnHeader();
            this.colRead = new System.Windows.Forms.ColumnHeader();
            this.colUpdate = new System.Windows.Forms.ColumnHeader();
            this.colDelete = new System.Windows.Forms.ColumnHeader();
            this.colRestriction = new System.Windows.Forms.ColumnHeader();
            this.cmPermissions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiProperites = new System.Windows.Forms.ToolStripMenuItem();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.cmiPermissionsExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.gbAssignedPermissions.SuspendLayout();
            this.cmPermissions.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(121, 13);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(575, 20);
            this.txtName.TabIndex = 52;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(12, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(97, 13);
            this.lblName.TabIndex = 53;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(540, 263);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 54;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(621, 263);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 55;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbAssignedPermissions
            // 
            this.gbAssignedPermissions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAssignedPermissions.Controls.Add(this.btnAddPermission);
            this.gbAssignedPermissions.Controls.Add(this.lvPermissions);
            this.gbAssignedPermissions.Location = new System.Drawing.Point(12, 83);
            this.gbAssignedPermissions.Name = "gbAssignedPermissions";
            this.gbAssignedPermissions.Size = new System.Drawing.Size(684, 174);
            this.gbAssignedPermissions.TabIndex = 56;
            this.gbAssignedPermissions.TabStop = false;
            this.gbAssignedPermissions.Text = "Assigned Permissions";
            // 
            // btnAddPermission
            // 
            this.btnAddPermission.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPermission.Location = new System.Drawing.Point(601, 145);
            this.btnAddPermission.Name = "btnAddPermission";
            this.btnAddPermission.Size = new System.Drawing.Size(77, 23);
            this.btnAddPermission.TabIndex = 22;
            this.btnAddPermission.Text = "Add...";
            this.btnAddPermission.UseVisualStyleBackColor = true;
            this.btnAddPermission.Click += new System.EventHandler(this.btnAddPermission_Click);
            // 
            // lvPermissions
            // 
            this.lvPermissions.AllowDrop = true;
            this.lvPermissions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPermissions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colResource,
            this.colCreate,
            this.colRead,
            this.colUpdate,
            this.colDelete,
            this.colRestriction});
            this.lvPermissions.ContextMenuStrip = this.cmPermissions;
            this.lvPermissions.FullRowSelect = true;
            this.lvPermissions.HideSelection = false;
            this.lvPermissions.Location = new System.Drawing.Point(7, 20);
            this.lvPermissions.Name = "lvPermissions";
            this.lvPermissions.Size = new System.Drawing.Size(671, 120);
            this.lvPermissions.TabIndex = 21;
            this.lvPermissions.UseCompatibleStateImageBehavior = false;
            this.lvPermissions.View = System.Windows.Forms.View.Details;
            this.lvPermissions.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvPermissions_MouseDoubleClick);
            this.lvPermissions.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvPermissions_DragDrop);
            this.lvPermissions.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvPermissions_DragEnter);
            this.lvPermissions.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvPermissions_KeyPress);
            this.lvPermissions.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvPermissions_KeyUp);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 90;
            // 
            // colResource
            // 
            this.colResource.Text = "Resource";
            this.colResource.Width = 130;
            // 
            // colCreate
            // 
            this.colCreate.Text = "Create";
            // 
            // colRead
            // 
            this.colRead.Text = "Read";
            // 
            // colUpdate
            // 
            this.colUpdate.Text = "Update";
            // 
            // colDelete
            // 
            this.colDelete.Text = "Delete";
            // 
            // colRestriction
            // 
            this.colRestriction.Text = "Restricted To";
            this.colRestriction.Width = 200;
            // 
            // cmPermissions
            // 
            this.cmPermissions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiAdd,
            this.cmiRemove,
            this.toolStripSeparator2,
            this.cmiPermissionsExportList,
            this.cmiProperites});
            this.cmPermissions.Name = "ctxMenuPermissions";
            this.cmPermissions.Size = new System.Drawing.Size(153, 120);
            // 
            // cmiAdd
            // 
            this.cmiAdd.Name = "cmiAdd";
            this.cmiAdd.Size = new System.Drawing.Size(152, 22);
            this.cmiAdd.Text = "&Add...";
            this.cmiAdd.Click += new System.EventHandler(this.addPermissionToolStripItem_Click);
            // 
            // cmiRemove
            // 
            this.cmiRemove.Name = "cmiRemove";
            this.cmiRemove.Size = new System.Drawing.Size(152, 22);
            this.cmiRemove.Text = "&Remove";
            this.cmiRemove.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // cmiProperites
            // 
            this.cmiProperites.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiProperites.Name = "cmiProperites";
            this.cmiProperites.Size = new System.Drawing.Size(152, 22);
            this.cmiProperites.Text = "&Properties...";
            this.cmiProperites.Click += new System.EventHandler(this.editPermissionToolStripMenuItem_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(121, 39);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(575, 38);
            this.txtDescription.TabIndex = 57;
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(12, 42);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(97, 13);
            this.lblDescription.TabIndex = 58;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmiPermissionsExportList
            // 
            this.cmiPermissionsExportList.Name = "cmiPermissionsExportList";
            this.cmiPermissionsExportList.Size = new System.Drawing.Size(152, 22);
            this.cmiPermissionsExportList.Text = "E&xport List";
            this.cmiPermissionsExportList.Click += new System.EventHandler(this.cmiPermissionsExportList_Click);
            // 
            // frmPermissionTemplate
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(708, 298);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.gbAssignedPermissions);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPermissionTemplate";
            this.Text = "Permission Template";
            this.Load += new System.EventHandler(this.frmPermissionTemplate_Load);
            this.gbAssignedPermissions.ResumeLayout(false);
            this.cmPermissions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbAssignedPermissions;
        private System.Windows.Forms.Button btnAddPermission;
        private System.Windows.Forms.ListView lvPermissions;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colResource;
        private System.Windows.Forms.ColumnHeader colCreate;
        private System.Windows.Forms.ColumnHeader colRead;
        private System.Windows.Forms.ColumnHeader colUpdate;
        private System.Windows.Forms.ColumnHeader colDelete;
        private System.Windows.Forms.ColumnHeader colRestriction;
        private System.Windows.Forms.ContextMenuStrip cmPermissions;
        private System.Windows.Forms.ToolStripMenuItem cmiAdd;
        private System.Windows.Forms.ToolStripMenuItem cmiRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmiProperites;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.ToolStripMenuItem cmiPermissionsExportList;

    }
}
