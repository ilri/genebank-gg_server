namespace GrinGlobal.Admin.ChildForms {
    partial class frmConnections {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConnections));
            this.lvConnections = new System.Windows.Forms.ListView();
            this.colServer = new System.Windows.Forms.ColumnHeader();
            this.colProvider = new System.Windows.Forms.ColumnHeader();
            this.cmConnection = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiConnectionNew = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiConnectionExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvConnections
            // 
            this.lvConnections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colServer,
            this.colProvider});
            this.lvConnections.ContextMenuStrip = this.cmConnection;
            this.lvConnections.FullRowSelect = true;
            this.lvConnections.HideSelection = false;
            this.lvConnections.Location = new System.Drawing.Point(0, 0);
            this.lvConnections.Name = "lvConnections";
            this.lvConnections.Size = new System.Drawing.Size(634, 265);
            this.lvConnections.SmallImageList = this.imageList1;
            this.lvConnections.TabIndex = 1;
            this.lvConnections.UseCompatibleStateImageBehavior = false;
            this.lvConnections.View = System.Windows.Forms.View.Details;
            this.lvConnections.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvMappings_MouseDoubleClick);
            this.lvConnections.SelectedIndexChanged += new System.EventHandler(this.lvMappings_SelectedIndexChanged_1);
            // 
            // colServer
            // 
            this.colServer.Text = "Server";
            this.colServer.Width = 200;
            // 
            // colProvider
            // 
            this.colProvider.Text = "Provider";
            this.colProvider.Width = 200;
            // 
            // cmConnection
            // 
            this.cmConnection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiConnectionNew,
            this.toolStripSeparator1,
            this.cmiDelete,
            this.toolStripSeparator18,
            this.cmiConnectionExportList,
            this.cmiRefresh,
            this.cmiProperties});
            this.cmConnection.Name = "ctxMenuNodeUser";
            this.cmConnection.Size = new System.Drawing.Size(173, 148);
            this.cmConnection.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuNodePermission_Opening);
            // 
            // cmiConnectionNew
            // 
            this.cmiConnectionNew.Name = "cmiConnectionNew";
            this.cmiConnectionNew.Size = new System.Drawing.Size(172, 22);
            this.cmiConnectionNew.Text = "&New Connection...";
            this.cmiConnectionNew.Click += new System.EventHandler(this.cmiConnectionNew_Click);
            // 
            // cmiDelete
            // 
            this.cmiDelete.Name = "cmiDelete";
            this.cmiDelete.Size = new System.Drawing.Size(172, 22);
            this.cmiDelete.Text = "&Delete";
            this.cmiDelete.Click += new System.EventHandler(this.deletePermissionMenuItem_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(169, 6);
            // 
            // cmiRefresh
            // 
            this.cmiRefresh.Name = "cmiRefresh";
            this.cmiRefresh.Size = new System.Drawing.Size(172, 22);
            this.cmiRefresh.Text = "&Refresh";
            this.cmiRefresh.Click += new System.EventHandler(this.refreshPermissionMenuItem_Click);
            // 
            // cmiProperties
            // 
            this.cmiProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiProperties.Name = "cmiProperties";
            this.cmiProperties.Size = new System.Drawing.Size(172, 22);
            this.cmiProperties.Text = "&Properties";
            this.cmiProperties.Click += new System.EventHandler(this.defaultPermissionMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "connection.ico");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // cmiConnectionExportList
            // 
            this.cmiConnectionExportList.Name = "cmiConnectionExportList";
            this.cmiConnectionExportList.Size = new System.Drawing.Size(172, 22);
            this.cmiConnectionExportList.Text = "E&xport List";
            this.cmiConnectionExportList.Click += new System.EventHandler(this.cmiConnectionExportList_Click);
            // 
            // frmConnections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(634, 264);
            this.Controls.Add(this.lvConnections);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConnections";
            this.Text = "Connections";
            this.Load += new System.EventHandler(this.frmTableMappings_Load);
            this.cmConnection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvConnections;
        private System.Windows.Forms.ColumnHeader colServer;
        private System.Windows.Forms.ContextMenuStrip cmConnection;
        private System.Windows.Forms.ToolStripMenuItem cmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem cmiRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiProperties;
        private System.Windows.Forms.ColumnHeader colProvider;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem cmiConnectionNew;
        private System.Windows.Forms.ToolStripMenuItem cmiConnectionExportList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}
