namespace GrinGlobal.Admin.ChildForms {
    partial class frmDataTriggers {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataTriggers));
            this.lv = new System.Windows.Forms.ListView();
            this.colDataview = new System.Windows.Forms.ColumnHeader();
            this.colTable = new System.Windows.Forms.ColumnHeader();
            this.colAssembly = new System.Windows.Forms.ColumnHeader();
            this.colClass = new System.Windows.Forms.ColumnHeader();
            this.colDescription = new System.Windows.Forms.ColumnHeader();
            this.colEnabled = new System.Windows.Forms.ColumnHeader();
            this.colVirtualFilePath = new System.Windows.Forms.ColumnHeader();
            this.colLastTouched = new System.Windows.Forms.ColumnHeader();
            this.cmDataTriggers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiDataTriggersImport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiDataTriggersExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.cmDataTriggers.SuspendLayout();
            this.SuspendLayout();
            // 
            // lv
            // 
            this.lv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDataview,
            this.colTable,
            this.colAssembly,
            this.colClass,
            this.colDescription,
            this.colEnabled,
            this.colVirtualFilePath,
            this.colLastTouched});
            this.lv.ContextMenuStrip = this.cmDataTriggers;
            this.lv.FullRowSelect = true;
            this.lv.HideSelection = false;
            this.lv.Location = new System.Drawing.Point(0, 0);
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(634, 265);
            this.lv.SmallImageList = this.imageList1;
            this.lv.TabIndex = 1;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            this.lv.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_MouseDoubleClick_1);
            this.lv.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lv_KeyUp_1);
            // 
            // colDataview
            // 
            this.colDataview.Text = "Dataview";
            this.colDataview.Width = 100;
            // 
            // colTable
            // 
            this.colTable.Text = "Table";
            this.colTable.Width = 100;
            // 
            // colAssembly
            // 
            this.colAssembly.Text = "Assembly";
            this.colAssembly.Width = 100;
            // 
            // colClass
            // 
            this.colClass.Text = "Class";
            this.colClass.Width = 100;
            // 
            // colDescription
            // 
            this.colDescription.Text = "Description";
            this.colDescription.Width = 200;
            // 
            // colEnabled
            // 
            this.colEnabled.Text = "Enabled?";
            // 
            // colVirtualFilePath
            // 
            this.colVirtualFilePath.Text = "Virtual File Path";
            this.colVirtualFilePath.Width = 100;
            // 
            // colLastTouched
            // 
            this.colLastTouched.Text = "Last Touched";
            this.colLastTouched.Width = 150;
            // 
            // cmDataTriggers
            // 
            this.cmDataTriggers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiNew,
            this.toolStripSeparator1,
            this.cmiDataTriggersImport,
            this.toolStripSeparator2,
            this.cmiEnable,
            this.cmiDisable,
            this.cmiDelete,
            this.toolStripSeparator18,
            this.cmiDataTriggersExportList,
            this.cmiRefresh,
            this.cmiProperties});
            this.cmDataTriggers.Name = "ctxMenuNodeUser";
            this.cmDataTriggers.Size = new System.Drawing.Size(176, 198);
            this.cmDataTriggers.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuNodePermission_Opening);
            // 
            // cmiNew
            // 
            this.cmiNew.Name = "cmiNew";
            this.cmiNew.Size = new System.Drawing.Size(175, 22);
            this.cmiNew.Text = "&New Data Trigger...";
            this.cmiNew.Click += new System.EventHandler(this.toolStripMenuItem1_Click_1);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(172, 6);
            // 
            // cmiDataTriggersImport
            // 
            this.cmiDataTriggersImport.Name = "cmiDataTriggersImport";
            this.cmiDataTriggersImport.Size = new System.Drawing.Size(175, 22);
            this.cmiDataTriggersImport.Text = "&Import...";
            this.cmiDataTriggersImport.Click += new System.EventHandler(this.cmiDataTriggersImport_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(172, 6);
            // 
            // cmiEnable
            // 
            this.cmiEnable.Name = "cmiEnable";
            this.cmiEnable.Size = new System.Drawing.Size(175, 22);
            this.cmiEnable.Text = "&Enable";
            this.cmiEnable.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // cmiDisable
            // 
            this.cmiDisable.Name = "cmiDisable";
            this.cmiDisable.Size = new System.Drawing.Size(175, 22);
            this.cmiDisable.Text = "Disable";
            this.cmiDisable.Click += new System.EventHandler(this.disablePermissionMenuItem_Click);
            // 
            // cmiDelete
            // 
            this.cmiDelete.Name = "cmiDelete";
            this.cmiDelete.Size = new System.Drawing.Size(175, 22);
            this.cmiDelete.Text = "&Delete";
            this.cmiDelete.Click += new System.EventHandler(this.deletePermissionMenuItem_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(172, 6);
            // 
            // cmiDataTriggersExportList
            // 
            this.cmiDataTriggersExportList.Name = "cmiDataTriggersExportList";
            this.cmiDataTriggersExportList.Size = new System.Drawing.Size(175, 22);
            this.cmiDataTriggersExportList.Text = "E&xport List";
            this.cmiDataTriggersExportList.Click += new System.EventHandler(this.toolStripMenuItem1_Click_2);
            // 
            // cmiRefresh
            // 
            this.cmiRefresh.Name = "cmiRefresh";
            this.cmiRefresh.Size = new System.Drawing.Size(175, 22);
            this.cmiRefresh.Text = "&Refresh";
            this.cmiRefresh.Click += new System.EventHandler(this.refreshPermissionMenuItem_Click);
            // 
            // cmiProperties
            // 
            this.cmiProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiProperties.Name = "cmiProperties";
            this.cmiProperties.Size = new System.Drawing.Size(175, 22);
            this.cmiProperties.Text = "&Properties";
            this.cmiProperties.Click += new System.EventHandler(this.defaultPermissionMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "triggers.ico");
            // 
            // frmDataTriggers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(634, 264);
            this.Controls.Add(this.lv);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDataTriggers";
            this.Text = "Data Triggers";
            this.Load += new System.EventHandler(this.frmTableMappings_Load);
            this.cmDataTriggers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lv;
        private System.Windows.Forms.ColumnHeader colDataview;
        private System.Windows.Forms.ContextMenuStrip cmDataTriggers;
        private System.Windows.Forms.ToolStripMenuItem cmiEnable;
        private System.Windows.Forms.ToolStripMenuItem cmiDisable;
        private System.Windows.Forms.ToolStripMenuItem cmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem cmiRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiProperties;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ColumnHeader colEnabled;
        private System.Windows.Forms.ToolStripMenuItem cmiNew;
        private System.Windows.Forms.ColumnHeader colLastTouched;
        private System.Windows.Forms.ColumnHeader colTable;
        private System.Windows.Forms.ColumnHeader colVirtualFilePath;
        private System.Windows.Forms.ColumnHeader colAssembly;
        private System.Windows.Forms.ColumnHeader colClass;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem cmiDataTriggersExportList;
        private System.Windows.Forms.ColumnHeader colDescription;
        private System.Windows.Forms.ToolStripMenuItem cmiDataTriggersImport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}
