namespace GrinGlobal.Admin.ChildForms {
    partial class frmTableMappings {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTableMappings));
            this.lvMappings = new System.Windows.Forms.ListView();
            this.colTableName = new System.Windows.Forms.ColumnHeader();
            this.colTableEnabled = new System.Windows.Forms.ColumnHeader();
            this.colDatabaseArea = new System.Windows.Forms.ColumnHeader();
            this.colLastTouched = new System.Windows.Forms.ColumnHeader();
            this.cmTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiInspect = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRebuild = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.cmiTableExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvMappings
            // 
            this.lvMappings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMappings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTableName,
            this.colTableEnabled,
            this.colDatabaseArea,
            this.colLastTouched});
            this.lvMappings.ContextMenuStrip = this.cmTable;
            this.lvMappings.FullRowSelect = true;
            this.lvMappings.HideSelection = false;
            this.lvMappings.Location = new System.Drawing.Point(0, 0);
            this.lvMappings.Name = "lvMappings";
            this.lvMappings.Size = new System.Drawing.Size(634, 265);
            this.lvMappings.SmallImageList = this.imageList1;
            this.lvMappings.TabIndex = 1;
            this.lvMappings.UseCompatibleStateImageBehavior = false;
            this.lvMappings.View = System.Windows.Forms.View.Details;
            this.lvMappings.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvMappings_MouseDoubleClick);
            this.lvMappings.SelectedIndexChanged += new System.EventHandler(this.lvMappings_SelectedIndexChanged_1);
            // 
            // colTableName
            // 
            this.colTableName.Text = "Table";
            this.colTableName.Width = 200;
            // 
            // colTableEnabled
            // 
            this.colTableEnabled.Text = "Enabled?";
            // 
            // colDatabaseArea
            // 
            this.colDatabaseArea.Text = "Area";
            this.colDatabaseArea.Width = 100;
            // 
            // colLastTouched
            // 
            this.colLastTouched.Text = "Last Touched";
            this.colLastTouched.Width = 150;
            // 
            // cmTable
            // 
            this.cmTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiInspect,
            this.cmiRebuild,
            this.toolStripSeparator1,
            this.cmiEnable,
            this.cmiDisable,
            this.cmiDelete,
            this.toolStripSeparator18,
            this.cmiTableExportList,
            this.cmiRefresh,
            this.cmiProperties});
            this.cmTable.Name = "ctxMenuNodeUser";
            this.cmTable.Size = new System.Drawing.Size(284, 214);
            this.cmTable.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuNodePermission_Opening);
            // 
            // cmiInspect
            // 
            this.cmiInspect.Name = "cmiInspect";
            this.cmiInspect.Size = new System.Drawing.Size(283, 22);
            this.cmiInspect.Text = "&Inspect Schema for Unmapped Tables...";
            this.cmiInspect.Click += new System.EventHandler(this.toolStripMenuItem1_Click_1);
            // 
            // cmiRebuild
            // 
            this.cmiRebuild.Name = "cmiRebuild";
            this.cmiRebuild.Size = new System.Drawing.Size(283, 22);
            this.cmiRebuild.Text = "&Remap from Schema...";
            this.cmiRebuild.Click += new System.EventHandler(this.cmiRebuild_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(280, 6);
            // 
            // cmiEnable
            // 
            this.cmiEnable.Name = "cmiEnable";
            this.cmiEnable.Size = new System.Drawing.Size(283, 22);
            this.cmiEnable.Text = "&Enable";
            this.cmiEnable.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // cmiDisable
            // 
            this.cmiDisable.Name = "cmiDisable";
            this.cmiDisable.Size = new System.Drawing.Size(283, 22);
            this.cmiDisable.Text = "Disable";
            this.cmiDisable.Click += new System.EventHandler(this.disablePermissionMenuItem_Click);
            // 
            // cmiDelete
            // 
            this.cmiDelete.Name = "cmiDelete";
            this.cmiDelete.Size = new System.Drawing.Size(283, 22);
            this.cmiDelete.Text = "&Delete";
            this.cmiDelete.Click += new System.EventHandler(this.deletePermissionMenuItem_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(280, 6);
            // 
            // cmiRefresh
            // 
            this.cmiRefresh.Name = "cmiRefresh";
            this.cmiRefresh.Size = new System.Drawing.Size(283, 22);
            this.cmiRefresh.Text = "&Refresh";
            this.cmiRefresh.Click += new System.EventHandler(this.refreshPermissionMenuItem_Click);
            // 
            // cmiProperties
            // 
            this.cmiProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiProperties.Name = "cmiProperties";
            this.cmiProperties.Size = new System.Drawing.Size(283, 22);
            this.cmiProperties.Text = "&Properties";
            this.cmiProperties.Click += new System.EventHandler(this.defaultPermissionMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "table.ico");
            // 
            // cmiTableExportList
            // 
            this.cmiTableExportList.Name = "cmiTableExportList";
            this.cmiTableExportList.Size = new System.Drawing.Size(283, 22);
            this.cmiTableExportList.Text = "E&xport List";
            this.cmiTableExportList.Click += new System.EventHandler(this.cmiTableExportList_Click);
            // 
            // frmTableMappings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(634, 264);
            this.Controls.Add(this.lvMappings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTableMappings";
            this.Text = "Table Mappings";
            this.Load += new System.EventHandler(this.frmTableMappings_Load);
            this.cmTable.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvMappings;
        private System.Windows.Forms.ColumnHeader colTableName;
        private System.Windows.Forms.ContextMenuStrip cmTable;
        private System.Windows.Forms.ToolStripMenuItem cmiEnable;
        private System.Windows.Forms.ToolStripMenuItem cmiDisable;
        private System.Windows.Forms.ToolStripMenuItem cmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem cmiRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiProperties;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ColumnHeader colTableEnabled;
        private System.Windows.Forms.ColumnHeader colDatabaseArea;
        private System.Windows.Forms.ToolStripMenuItem cmiInspect;
        private System.Windows.Forms.ColumnHeader colLastTouched;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem cmiRebuild;
        private System.Windows.Forms.ToolStripMenuItem cmiTableExportList;
    }
}
