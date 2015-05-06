namespace GrinGlobal.Admin.ChildForms {
    partial class frmPermissions {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPermissions));
            this.lvPerms = new System.Windows.Forms.ListView();
            this.colPermissionName = new System.Windows.Forms.ColumnHeader();
            this.colResource = new System.Windows.Forms.ColumnHeader();
            this.colEnabled = new System.Windows.Forms.ColumnHeader();
            this.colCreate = new System.Windows.Forms.ColumnHeader();
            this.colRead = new System.Windows.Forms.ColumnHeader();
            this.colUpdate = new System.Windows.Forms.ColumnHeader();
            this.colDelete = new System.Windows.Forms.ColumnHeader();
            this.colRestrictTo = new System.Windows.Forms.ColumnHeader();
            this.cmPermission = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmiPermissionExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmPermission.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvPerms
            // 
            this.lvPerms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPerms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPermissionName,
            this.colResource,
            this.colEnabled,
            this.colCreate,
            this.colRead,
            this.colUpdate,
            this.colDelete,
            this.colRestrictTo});
            this.lvPerms.ContextMenuStrip = this.cmPermission;
            this.lvPerms.FullRowSelect = true;
            this.lvPerms.HideSelection = false;
            this.lvPerms.Location = new System.Drawing.Point(0, 0);
            this.lvPerms.Name = "lvPerms";
            this.lvPerms.Size = new System.Drawing.Size(634, 221);
            this.lvPerms.SmallImageList = this.imageList1;
            this.lvPerms.TabIndex = 1;
            this.lvPerms.UseCompatibleStateImageBehavior = false;
            this.lvPerms.View = System.Windows.Forms.View.Details;
            this.lvPerms.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvPerms_MouseDoubleClick);
            this.lvPerms.SelectedIndexChanged += new System.EventHandler(this.lvPerms_SelectedIndexChanged);
            this.lvPerms.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvPerms_KeyPress);
            this.lvPerms.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvPerms_KeyUp);
            this.lvPerms.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvPerms_ItemDrag);
            // 
            // colPermissionName
            // 
            this.colPermissionName.Text = "Permission";
            this.colPermissionName.Width = 110;
            // 
            // colResource
            // 
            this.colResource.Text = "Resource";
            this.colResource.Width = 120;
            // 
            // colEnabled
            // 
            this.colEnabled.Text = "Enabled?";
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
            // colRestrictTo
            // 
            this.colRestrictTo.Text = "Restrict To";
            this.colRestrictTo.Width = 150;
            // 
            // cmPermission
            // 
            this.cmPermission.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiNew,
            this.toolStripSeparator1,
            this.cmiEnable,
            this.cmiDisable,
            this.cmiDelete,
            this.toolStripSeparator18,
            this.cmiPermissionExportList,
            this.cmiRefresh,
            this.cmiProperties});
            this.cmPermission.Name = "ctxMenuNodeUser";
            this.cmPermission.Size = new System.Drawing.Size(169, 192);
            this.cmPermission.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuNodePermission_Opening);
            // 
            // cmiNew
            // 
            this.cmiNew.Name = "cmiNew";
            this.cmiNew.Size = new System.Drawing.Size(168, 22);
            this.cmiNew.Text = "&New Permission...";
            this.cmiNew.Click += new System.EventHandler(this.newPermissionMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(165, 6);
            // 
            // cmiEnable
            // 
            this.cmiEnable.Name = "cmiEnable";
            this.cmiEnable.Size = new System.Drawing.Size(168, 22);
            this.cmiEnable.Text = "&Enable";
            this.cmiEnable.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // cmiDisable
            // 
            this.cmiDisable.Name = "cmiDisable";
            this.cmiDisable.Size = new System.Drawing.Size(168, 22);
            this.cmiDisable.Text = "Disable";
            this.cmiDisable.Click += new System.EventHandler(this.disablePermissionMenuItem_Click);
            // 
            // cmiDelete
            // 
            this.cmiDelete.Name = "cmiDelete";
            this.cmiDelete.Size = new System.Drawing.Size(168, 22);
            this.cmiDelete.Text = "&Delete";
            this.cmiDelete.Click += new System.EventHandler(this.deletePermissionMenuItem_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(165, 6);
            // 
            // cmiRefresh
            // 
            this.cmiRefresh.Name = "cmiRefresh";
            this.cmiRefresh.Size = new System.Drawing.Size(168, 22);
            this.cmiRefresh.Text = "&Refresh";
            this.cmiRefresh.Click += new System.EventHandler(this.refreshPermissionMenuItem_Click);
            // 
            // cmiProperties
            // 
            this.cmiProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiProperties.Name = "cmiProperties";
            this.cmiProperties.Size = new System.Drawing.Size(168, 22);
            this.cmiProperties.Text = "&Properties";
            this.cmiProperties.Click += new System.EventHandler(this.defaultPermissionMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "permissions.ico");
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(466, 229);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&Add";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(547, 229);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmiPermissionExportList
            // 
            this.cmiPermissionExportList.Name = "cmiPermissionExportList";
            this.cmiPermissionExportList.Size = new System.Drawing.Size(168, 22);
            this.cmiPermissionExportList.Text = "E&xport List";
            this.cmiPermissionExportList.Click += new System.EventHandler(this.cmiPermissionExportList_Click);
            // 
            // frmPermissions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(634, 264);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lvPerms);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPermissions";
            this.Text = "Permissions";
            this.Load += new System.EventHandler(this.frmPermissions_Load);
            this.cmPermission.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvPerms;
        private System.Windows.Forms.ColumnHeader colPermissionName;
        private System.Windows.Forms.ColumnHeader colResource;
        private System.Windows.Forms.ColumnHeader colCreate;
        private System.Windows.Forms.ColumnHeader colRead;
        private System.Windows.Forms.ColumnHeader colUpdate;
        private System.Windows.Forms.ColumnHeader colDelete;
        private System.Windows.Forms.ColumnHeader colEnabled;
        private System.Windows.Forms.ContextMenuStrip cmPermission;
        private System.Windows.Forms.ToolStripMenuItem cmiEnable;
        private System.Windows.Forms.ToolStripMenuItem cmiDisable;
        private System.Windows.Forms.ToolStripMenuItem cmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem cmiRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiProperties;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolStripMenuItem cmiNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ColumnHeader colRestrictTo;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem cmiPermissionExportList;
    }
}
