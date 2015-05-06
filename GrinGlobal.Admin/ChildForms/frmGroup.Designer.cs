namespace GrinGlobal.Admin.ChildForms {
    partial class frmGroup {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGroup));
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lvPermissions = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colResource = new System.Windows.Forms.ColumnHeader();
            this.colPermEnabled = new System.Windows.Forms.ColumnHeader();
            this.colCreate = new System.Windows.Forms.ColumnHeader();
            this.colRead = new System.Windows.Forms.ColumnHeader();
            this.colUpdate = new System.Windows.Forms.ColumnHeader();
            this.colDelete = new System.Windows.Forms.ColumnHeader();
            this.colRestriction = new System.Windows.Forms.ColumnHeader();
            this.cmPermissions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiPermissionAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiPermissionRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiPermissionsExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiPermissionProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tcPermsUsers = new System.Windows.Forms.TabControl();
            this.tpPermissions = new System.Windows.Forms.TabPage();
            this.btnAddPermission = new System.Windows.Forms.Button();
            this.tpUsers = new System.Windows.Forms.TabPage();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.lvUsers = new System.Windows.Forms.ListView();
            this.colUserName = new System.Windows.Forms.ColumnHeader();
            this.colFullName = new System.Windows.Forms.ColumnHeader();
            this.colEnabled = new System.Windows.Forms.ColumnHeader();
            this.cmUsers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiUserAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiUserRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiUsersExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiUserProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.txtTag = new System.Windows.Forms.TextBox();
            this.lblTag = new System.Windows.Forms.Label();
            this.cmPermissions.SuspendLayout();
            this.tcPermsUsers.SuspendLayout();
            this.tpPermissions.SuspendLayout();
            this.tpUsers.SuspendLayout();
            this.cmUsers.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(121, 13);
            this.txtName.MaxLength = 500;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(575, 20);
            this.txtName.TabIndex = 0;
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
            this.btnSave.Location = new System.Drawing.Point(540, 361);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(621, 361);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            this.colPermEnabled,
            this.colCreate,
            this.colRead,
            this.colUpdate,
            this.colDelete,
            this.colRestriction});
            this.lvPermissions.ContextMenuStrip = this.cmPermissions;
            this.lvPermissions.FullRowSelect = true;
            this.lvPermissions.HideSelection = false;
            this.lvPermissions.Location = new System.Drawing.Point(3, 3);
            this.lvPermissions.Name = "lvPermissions";
            this.lvPermissions.Size = new System.Drawing.Size(671, 174);
            this.lvPermissions.SmallImageList = this.imageList1;
            this.lvPermissions.TabIndex = 4;
            this.lvPermissions.UseCompatibleStateImageBehavior = false;
            this.lvPermissions.View = System.Windows.Forms.View.Details;
            this.lvPermissions.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvPermissions_MouseDoubleClick);
            this.lvPermissions.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvPermissions_DragDrop);
            this.lvPermissions.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvPermissions_DragEnter);
            this.lvPermissions.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvPermissions_KeyPress);
            this.lvPermissions.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvPermissions_KeyUp);
            this.lvPermissions.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvPermissions_ItemDrag);
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
            // colPermEnabled
            // 
            this.colPermEnabled.Text = "Enabled?";
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
            this.cmiPermissionAdd,
            this.cmiPermissionRemove,
            this.toolStripSeparator2,
            this.cmiPermissionsExportList,
            this.cmiPermissionProperties});
            this.cmPermissions.Name = "ctxMenuPermissions";
            this.cmPermissions.Size = new System.Drawing.Size(142, 98);
            this.cmPermissions.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuPermissions_Opening);
            // 
            // cmiPermissionAdd
            // 
            this.cmiPermissionAdd.Name = "cmiPermissionAdd";
            this.cmiPermissionAdd.Size = new System.Drawing.Size(141, 22);
            this.cmiPermissionAdd.Text = "&Add...";
            this.cmiPermissionAdd.Click += new System.EventHandler(this.addPermissionToolStripItem_Click);
            // 
            // cmiPermissionRemove
            // 
            this.cmiPermissionRemove.Name = "cmiPermissionRemove";
            this.cmiPermissionRemove.Size = new System.Drawing.Size(141, 22);
            this.cmiPermissionRemove.Text = "&Remove";
            this.cmiPermissionRemove.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(138, 6);
            // 
            // cmiPermissionsExportList
            // 
            this.cmiPermissionsExportList.Name = "cmiPermissionsExportList";
            this.cmiPermissionsExportList.Size = new System.Drawing.Size(141, 22);
            this.cmiPermissionsExportList.Text = "E&xport List";
            this.cmiPermissionsExportList.Click += new System.EventHandler(this.cmiPermissionsExportList_Click);
            // 
            // cmiPermissionProperties
            // 
            this.cmiPermissionProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiPermissionProperties.Name = "cmiPermissionProperties";
            this.cmiPermissionProperties.Size = new System.Drawing.Size(141, 22);
            this.cmiPermissionProperties.Text = "&Properties...";
            this.cmiPermissionProperties.Click += new System.EventHandler(this.editPermissionToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "permissions.ico");
            this.imageList1.Images.SetKeyName(1, "users.ico");
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(121, 73);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(575, 38);
            this.txtDescription.TabIndex = 2;
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(12, 73);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(97, 13);
            this.lblDescription.TabIndex = 58;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tcPermsUsers
            // 
            this.tcPermsUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcPermsUsers.Controls.Add(this.tpPermissions);
            this.tcPermsUsers.Controls.Add(this.tpUsers);
            this.tcPermsUsers.Location = new System.Drawing.Point(12, 117);
            this.tcPermsUsers.Name = "tcPermsUsers";
            this.tcPermsUsers.SelectedIndex = 0;
            this.tcPermsUsers.Size = new System.Drawing.Size(684, 238);
            this.tcPermsUsers.TabIndex = 3;
            // 
            // tpPermissions
            // 
            this.tpPermissions.Controls.Add(this.btnAddPermission);
            this.tpPermissions.Controls.Add(this.lvPermissions);
            this.tpPermissions.Location = new System.Drawing.Point(4, 22);
            this.tpPermissions.Name = "tpPermissions";
            this.tpPermissions.Padding = new System.Windows.Forms.Padding(3);
            this.tpPermissions.Size = new System.Drawing.Size(676, 212);
            this.tpPermissions.TabIndex = 0;
            this.tpPermissions.Text = "Permissions";
            this.tpPermissions.UseVisualStyleBackColor = true;
            // 
            // btnAddPermission
            // 
            this.btnAddPermission.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPermission.Location = new System.Drawing.Point(595, 183);
            this.btnAddPermission.Name = "btnAddPermission";
            this.btnAddPermission.Size = new System.Drawing.Size(75, 23);
            this.btnAddPermission.TabIndex = 5;
            this.btnAddPermission.Text = "Add...";
            this.btnAddPermission.UseVisualStyleBackColor = true;
            this.btnAddPermission.Click += new System.EventHandler(this.btnAddPermission_Click_1);
            // 
            // tpUsers
            // 
            this.tpUsers.Controls.Add(this.btnAddUser);
            this.tpUsers.Controls.Add(this.lvUsers);
            this.tpUsers.Location = new System.Drawing.Point(4, 22);
            this.tpUsers.Name = "tpUsers";
            this.tpUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tpUsers.Size = new System.Drawing.Size(676, 212);
            this.tpUsers.TabIndex = 1;
            this.tpUsers.Text = "Users";
            this.tpUsers.UseVisualStyleBackColor = true;
            // 
            // btnAddUser
            // 
            this.btnAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddUser.Location = new System.Drawing.Point(595, 183);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(75, 23);
            this.btnAddUser.TabIndex = 6;
            this.btnAddUser.Text = "Add...";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // lvUsers
            // 
            this.lvUsers.AllowDrop = true;
            this.lvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colUserName,
            this.colFullName,
            this.colEnabled});
            this.lvUsers.ContextMenuStrip = this.cmUsers;
            this.lvUsers.FullRowSelect = true;
            this.lvUsers.HideSelection = false;
            this.lvUsers.Location = new System.Drawing.Point(3, 3);
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.Size = new System.Drawing.Size(670, 174);
            this.lvUsers.SmallImageList = this.imageList1;
            this.lvUsers.TabIndex = 5;
            this.lvUsers.UseCompatibleStateImageBehavior = false;
            this.lvUsers.View = System.Windows.Forms.View.Details;
            this.lvUsers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvUsers_MouseDoubleClick);
            this.lvUsers.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvUsers_DragDrop);
            this.lvUsers.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvUsers_DragEnter);
            this.lvUsers.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvUsers_KeyUp);
            this.lvUsers.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvUsers_ItemDrag);
            // 
            // colUserName
            // 
            this.colUserName.Text = "User Name";
            this.colUserName.Width = 100;
            // 
            // colFullName
            // 
            this.colFullName.Text = "Full Name";
            this.colFullName.Width = 150;
            // 
            // colEnabled
            // 
            this.colEnabled.Text = "Enabled?";
            // 
            // cmUsers
            // 
            this.cmUsers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiUserAdd,
            this.cmiUserRemove,
            this.toolStripSeparator1,
            this.cmiUsersExportList,
            this.cmiUserProperties});
            this.cmUsers.Name = "ctxMenuPermissions";
            this.cmUsers.Size = new System.Drawing.Size(142, 98);
            this.cmUsers.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuUsers_Opening);
            // 
            // cmiUserAdd
            // 
            this.cmiUserAdd.Name = "cmiUserAdd";
            this.cmiUserAdd.Size = new System.Drawing.Size(141, 22);
            this.cmiUserAdd.Text = "&Add...";
            this.cmiUserAdd.Click += new System.EventHandler(this.addUserMenuItem_Click);
            // 
            // cmiUserRemove
            // 
            this.cmiUserRemove.Name = "cmiUserRemove";
            this.cmiUserRemove.Size = new System.Drawing.Size(141, 22);
            this.cmiUserRemove.Text = "&Remove";
            this.cmiUserRemove.Click += new System.EventHandler(this.removeUserMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(138, 6);
            // 
            // cmiUsersExportList
            // 
            this.cmiUsersExportList.Name = "cmiUsersExportList";
            this.cmiUsersExportList.Size = new System.Drawing.Size(141, 22);
            this.cmiUsersExportList.Text = "E&xport List";
            this.cmiUsersExportList.Click += new System.EventHandler(this.cmiUsersExportList_Click);
            // 
            // cmiUserProperties
            // 
            this.cmiUserProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiUserProperties.Name = "cmiUserProperties";
            this.cmiUserProperties.Size = new System.Drawing.Size(141, 22);
            this.cmiUserProperties.Text = "&Properties...";
            this.cmiUserProperties.Click += new System.EventHandler(this.defaultUserMenuItem_Click);
            // 
            // txtTag
            // 
            this.txtTag.Location = new System.Drawing.Point(121, 39);
            this.txtTag.MaxLength = 1000;
            this.txtTag.Name = "txtTag";
            this.txtTag.Size = new System.Drawing.Size(575, 20);
            this.txtTag.TabIndex = 1;
            this.txtTag.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
            // 
            // lblTag
            // 
            this.lblTag.Location = new System.Drawing.Point(12, 42);
            this.lblTag.Name = "lblTag";
            this.lblTag.Size = new System.Drawing.Size(97, 13);
            this.lblTag.TabIndex = 61;
            this.lblTag.Text = "Tag:";
            this.lblTag.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmGroup
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(708, 396);
            this.Controls.Add(this.txtTag);
            this.Controls.Add(this.lblTag);
            this.Controls.Add(this.tcPermsUsers);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmGroup";
            this.Text = "Group";
            this.Load += new System.EventHandler(this.frmPermissionTemplate_Load);
            this.cmPermissions.ResumeLayout(false);
            this.tcPermsUsers.ResumeLayout(false);
            this.tpPermissions.ResumeLayout(false);
            this.tpUsers.ResumeLayout(false);
            this.cmUsers.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView lvPermissions;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colResource;
        private System.Windows.Forms.ColumnHeader colCreate;
        private System.Windows.Forms.ColumnHeader colRead;
        private System.Windows.Forms.ColumnHeader colUpdate;
        private System.Windows.Forms.ColumnHeader colDelete;
        private System.Windows.Forms.ColumnHeader colRestriction;
        private System.Windows.Forms.ContextMenuStrip cmPermissions;
        private System.Windows.Forms.ToolStripMenuItem cmiPermissionAdd;
        private System.Windows.Forms.ToolStripMenuItem cmiPermissionRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmiPermissionProperties;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TabControl tcPermsUsers;
        private System.Windows.Forms.TabPage tpPermissions;
        private System.Windows.Forms.TabPage tpUsers;
        private System.Windows.Forms.ListView lvUsers;
        private System.Windows.Forms.ColumnHeader colUserName;
        private System.Windows.Forms.ColumnHeader colFullName;
        private System.Windows.Forms.ColumnHeader colEnabled;
        private System.Windows.Forms.ContextMenuStrip cmUsers;
        private System.Windows.Forms.ToolStripMenuItem cmiUserAdd;
        private System.Windows.Forms.ToolStripMenuItem cmiUserRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmiUserProperties;
        private System.Windows.Forms.TextBox txtTag;
        private System.Windows.Forms.Label lblTag;
        private System.Windows.Forms.Button btnAddPermission;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem cmiPermissionsExportList;
        private System.Windows.Forms.ToolStripMenuItem cmiUsersExportList;
        private System.Windows.Forms.ColumnHeader colPermEnabled;

    }
}
