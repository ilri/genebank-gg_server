namespace GrinGlobal.Admin.ChildForms {
    partial class frmFileGroups {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFileGroups));
            this.lv = new System.Windows.Forms.ListView();
            this.colGroup = new System.Windows.Forms.ColumnHeader();
            this.colVersion = new System.Windows.Forms.ColumnHeader();
            this.colEnabled = new System.Windows.Forms.ColumnHeader();
            this.colTotalSize = new System.Windows.Forms.ColumnHeader();
            this.colLastTouched = new System.Windows.Forms.ColumnHeader();
            this.cmFileGroups = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.cmiFileGroupsExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmFileGroups.SuspendLayout();
            this.SuspendLayout();
            // 
            // lv
            // 
            this.lv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colGroup,
            this.colVersion,
            this.colEnabled,
            this.colTotalSize,
            this.colLastTouched});
            this.lv.ContextMenuStrip = this.cmFileGroups;
            this.lv.FullRowSelect = true;
            this.lv.HideSelection = false;
            this.lv.Location = new System.Drawing.Point(0, 0);
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(634, 265);
            this.lv.SmallImageList = this.imageList1;
            this.lv.TabIndex = 1;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            this.lv.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_MouseDoubleClick);
            this.lv.SelectedIndexChanged += new System.EventHandler(this.lv_SelectedIndexChanged_1);
            // 
            // colGroup
            // 
            this.colGroup.Text = "Group";
            this.colGroup.Width = 200;
            // 
            // colVersion
            // 
            this.colVersion.Text = "Version";
            this.colVersion.Width = 110;
            // 
            // colEnabled
            // 
            this.colEnabled.Text = "Enabled?";
            this.colEnabled.Width = 79;
            // 
            // colTotalSize
            // 
            this.colTotalSize.Text = "Size (MB)";
            this.colTotalSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // colLastTouched
            // 
            this.colLastTouched.Text = "Last Touched";
            this.colLastTouched.Width = 150;
            // 
            // cmFileGroups
            // 
            this.cmFileGroups.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiNew,
            this.toolStripSeparator1,
            this.cmiEnable,
            this.cmiDisable,
            this.cmiDelete,
            this.toolStripSeparator18,
            this.cmiFileGroupsExportList,
            this.cmiRefresh,
            this.cmiProperties});
            this.cmFileGroups.Name = "ctxMenuNodeUser";
            this.cmFileGroups.Size = new System.Drawing.Size(165, 192);
            this.cmFileGroups.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuNodePermission_Opening);
            // 
            // cmiNew
            // 
            this.cmiNew.Name = "cmiNew";
            this.cmiNew.Size = new System.Drawing.Size(164, 22);
            this.cmiNew.Text = "&New File Group...";
            this.cmiNew.Click += new System.EventHandler(this.newMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(161, 6);
            // 
            // cmiEnable
            // 
            this.cmiEnable.Name = "cmiEnable";
            this.cmiEnable.Size = new System.Drawing.Size(164, 22);
            this.cmiEnable.Text = "&Enable";
            this.cmiEnable.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // cmiDisable
            // 
            this.cmiDisable.Name = "cmiDisable";
            this.cmiDisable.Size = new System.Drawing.Size(164, 22);
            this.cmiDisable.Text = "Disable";
            this.cmiDisable.Click += new System.EventHandler(this.disablePermissionMenuItem_Click);
            // 
            // cmiDelete
            // 
            this.cmiDelete.Name = "cmiDelete";
            this.cmiDelete.Size = new System.Drawing.Size(164, 22);
            this.cmiDelete.Text = "&Delete";
            this.cmiDelete.Click += new System.EventHandler(this.deletePermissionMenuItem_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(161, 6);
            // 
            // cmiRefresh
            // 
            this.cmiRefresh.Name = "cmiRefresh";
            this.cmiRefresh.Size = new System.Drawing.Size(164, 22);
            this.cmiRefresh.Text = "&Refresh";
            this.cmiRefresh.Click += new System.EventHandler(this.refreshPermissionMenuItem_Click);
            // 
            // cmiProperties
            // 
            this.cmiProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiProperties.Name = "cmiProperties";
            this.cmiProperties.Size = new System.Drawing.Size(164, 22);
            this.cmiProperties.Text = "&Properties";
            this.cmiProperties.Click += new System.EventHandler(this.defaultPermissionMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "filegroup.ico");
            // 
            // cmiFileGroupsExportList
            // 
            this.cmiFileGroupsExportList.Name = "cmiFileGroupsExportList";
            this.cmiFileGroupsExportList.Size = new System.Drawing.Size(164, 22);
            this.cmiFileGroupsExportList.Text = "E&xport List";
            this.cmiFileGroupsExportList.Click += new System.EventHandler(this.cmiFileGroupsExportList_Click);
            // 
            // frmFileGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(634, 264);
            this.Controls.Add(this.lv);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmFileGroups";
            this.Text = "File Groups";
            this.Load += new System.EventHandler(this.frmTableMappings_Load);
            this.cmFileGroups.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lv;
        private System.Windows.Forms.ColumnHeader colGroup;
        private System.Windows.Forms.ContextMenuStrip cmFileGroups;
        private System.Windows.Forms.ToolStripMenuItem cmiEnable;
        private System.Windows.Forms.ToolStripMenuItem cmiDisable;
        private System.Windows.Forms.ToolStripMenuItem cmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem cmiRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiProperties;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ColumnHeader colEnabled;
        private System.Windows.Forms.ColumnHeader colVersion;
        private System.Windows.Forms.ToolStripMenuItem cmiNew;
        private System.Windows.Forms.ColumnHeader colLastTouched;
        private System.Windows.Forms.ColumnHeader colTotalSize;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem cmiFileGroupsExportList;
    }
}
