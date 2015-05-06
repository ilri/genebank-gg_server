namespace GrinGlobal.Admin.ChildForms {
    partial class frmDataviews {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataviews));
            this.lvDataviews = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colTitle = new System.Windows.Forms.ColumnHeader();
            this.colCategoryName = new System.Windows.Forms.ColumnHeader();
            this.colDatabaseArea = new System.Windows.Forms.ColumnHeader();
            this.colLastTouched = new System.Windows.Forms.ColumnHeader();
            this.colDescription = new System.Windows.Forms.ColumnHeader();
            this.cmDataview = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiImport = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiExport = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCopyTo = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiVerify = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiDataviewExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.sfdExport = new System.Windows.Forms.SaveFileDialog();
            this.ofdImport = new System.Windows.Forms.OpenFileDialog();
            this.cmiDataviewShowDependencies = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDataview.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvDataviews
            // 
            this.lvDataviews.AllowDrop = true;
            this.lvDataviews.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colTitle,
            this.colCategoryName,
            this.colDatabaseArea,
            this.colLastTouched,
            this.colDescription});
            this.lvDataviews.ContextMenuStrip = this.cmDataview;
            this.lvDataviews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDataviews.FullRowSelect = true;
            this.lvDataviews.HideSelection = false;
            this.lvDataviews.Location = new System.Drawing.Point(0, 0);
            this.lvDataviews.Name = "lvDataviews";
            this.lvDataviews.Size = new System.Drawing.Size(674, 333);
            this.lvDataviews.SmallImageList = this.imageList1;
            this.lvDataviews.TabIndex = 2;
            this.lvDataviews.UseCompatibleStateImageBehavior = false;
            this.lvDataviews.View = System.Windows.Forms.View.Details;
            this.lvDataviews.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvDataviews_MouseDoubleClick);
            this.lvDataviews.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvDataviews_DragDrop);
            this.lvDataviews.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvDataviews_MouseDown);
            this.lvDataviews.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvDataviews_DragEnter);
            this.lvDataviews.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvDataviews_KeyPress);
            this.lvDataviews.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvDataviews_KeyUp);
            this.lvDataviews.DragOver += new System.Windows.Forms.DragEventHandler(this.lvDataviews_DragOver);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 150;
            // 
            // colTitle
            // 
            this.colTitle.Text = "Title";
            this.colTitle.Width = 250;
            // 
            // colCategoryName
            // 
            this.colCategoryName.Text = "Category";
            this.colCategoryName.Width = 80;
            // 
            // colDatabaseArea
            // 
            this.colDatabaseArea.Text = "Database Area";
            this.colDatabaseArea.Width = 100;
            // 
            // colLastTouched
            // 
            this.colLastTouched.Text = "Last Touched";
            this.colLastTouched.Width = 130;
            // 
            // colDescription
            // 
            this.colDescription.Text = "Description";
            this.colDescription.Width = 250;
            // 
            // cmDataview
            // 
            this.cmDataview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiNew,
            this.toolStripSeparator2,
            this.cmiImport,
            this.cmiExport,
            this.cmiCopyTo,
            this.cmiVerify,
            this.cmiRename,
            this.cmiDataviewShowDependencies,
            this.toolStripSeparator1,
            this.cmiDelete,
            this.toolStripSeparator18,
            this.cmiDataviewExportList,
            this.cmiRefresh,
            this.cmiProperties});
            this.cmDataview.Name = "ctxMenuNodeUser";
            this.cmDataview.Size = new System.Drawing.Size(190, 286);
            this.cmDataview.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuNodePermission_Opening);
            // 
            // cmiNew
            // 
            this.cmiNew.Name = "cmiNew";
            this.cmiNew.Size = new System.Drawing.Size(189, 22);
            this.cmiNew.Text = "&New Dataview...";
            this.cmiNew.Click += new System.EventHandler(this.cmiNew_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(186, 6);
            // 
            // cmiImport
            // 
            this.cmiImport.Name = "cmiImport";
            this.cmiImport.Size = new System.Drawing.Size(189, 22);
            this.cmiImport.Text = "&Import...";
            this.cmiImport.Click += new System.EventHandler(this.cmiImport_Click);
            // 
            // cmiExport
            // 
            this.cmiExport.Name = "cmiExport";
            this.cmiExport.Size = new System.Drawing.Size(189, 22);
            this.cmiExport.Text = "&Export...";
            this.cmiExport.Click += new System.EventHandler(this.cmiExport_Click);
            // 
            // cmiCopyTo
            // 
            this.cmiCopyTo.Name = "cmiCopyTo";
            this.cmiCopyTo.Size = new System.Drawing.Size(189, 22);
            this.cmiCopyTo.Text = "&Copy To...";
            this.cmiCopyTo.Click += new System.EventHandler(this.cmiCopyTo_Click);
            // 
            // cmiVerify
            // 
            this.cmiVerify.Name = "cmiVerify";
            this.cmiVerify.Size = new System.Drawing.Size(189, 22);
            this.cmiVerify.Text = "&Verify...";
            this.cmiVerify.Click += new System.EventHandler(this.cmiVerify_Click);
            // 
            // cmiRename
            // 
            this.cmiRename.Name = "cmiRename";
            this.cmiRename.Size = new System.Drawing.Size(189, 22);
            this.cmiRename.Text = "Re&name...";
            this.cmiRename.Click += new System.EventHandler(this.cmiDataviewRename_Click);
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
            this.cmiDelete.Click += new System.EventHandler(this.deletePermissionTemplateMenuItem_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(186, 6);
            // 
            // cmiDataviewExportList
            // 
            this.cmiDataviewExportList.Name = "cmiDataviewExportList";
            this.cmiDataviewExportList.Size = new System.Drawing.Size(189, 22);
            this.cmiDataviewExportList.Text = "E&xport List";
            this.cmiDataviewExportList.Click += new System.EventHandler(this.cmiDataviewExportList_Click);
            // 
            // cmiRefresh
            // 
            this.cmiRefresh.Name = "cmiRefresh";
            this.cmiRefresh.Size = new System.Drawing.Size(189, 22);
            this.cmiRefresh.Text = "&Refresh";
            this.cmiRefresh.Click += new System.EventHandler(this.refreshPermissionTemplateMenuItem_Click);
            // 
            // cmiProperties
            // 
            this.cmiProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiProperties.Name = "cmiProperties";
            this.cmiProperties.Size = new System.Drawing.Size(189, 22);
            this.cmiProperties.Text = "&Properties";
            this.cmiProperties.Click += new System.EventHandler(this.defaultPermissionTemplateMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "dataviews.ico");
            // 
            // sfdExport
            // 
            this.sfdExport.Filter = "Dataview Files (*.dataview, *.dat)|*.dataview;*.dat|All Files (*.*)|*.*";
            this.sfdExport.Title = "Save Dataview Export As...";
            // 
            // ofdImport
            // 
            this.ofdImport.DefaultExt = "dat";
            this.ofdImport.Filter = "Dataview Files (*.dataview, *.dat)|*.dataview;*.dat|All Files (*.*)|*.*";
            this.ofdImport.Multiselect = true;
            this.ofdImport.SupportMultiDottedExtensions = true;
            // 
            // cmiDataviewShowDependencies
            // 
            this.cmiDataviewShowDependencies.Name = "cmiDataviewShowDependencies";
            this.cmiDataviewShowDependencies.Size = new System.Drawing.Size(189, 22);
            this.cmiDataviewShowDependencies.Text = "&Show Dependencies...";
            this.cmiDataviewShowDependencies.Click += new System.EventHandler(this.cmiDataviewShowDependencies_Click);
            // 
            // frmDataviews
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(674, 333);
            this.Controls.Add(this.lvDataviews);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDataviews";
            this.Text = "Dataviews";
            this.Load += new System.EventHandler(this.frmDataviews_Load);
            this.cmDataview.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvDataviews;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ContextMenuStrip cmDataview;
        private System.Windows.Forms.ToolStripMenuItem cmiNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem cmiRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiProperties;
        private System.Windows.Forms.ColumnHeader colDescription;
        private System.Windows.Forms.ColumnHeader colLastTouched;
        private System.Windows.Forms.ColumnHeader colTitle;
        private System.Windows.Forms.ToolStripMenuItem cmiExport;
        private System.Windows.Forms.SaveFileDialog sfdExport;
        private System.Windows.Forms.ToolStripMenuItem cmiImport;
        private System.Windows.Forms.OpenFileDialog ofdImport;
        private System.Windows.Forms.ToolStripMenuItem cmiCopyTo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmiVerify;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem cmiRename;
        private System.Windows.Forms.ColumnHeader colDatabaseArea;
        private System.Windows.Forms.ColumnHeader colCategoryName;
        private System.Windows.Forms.ToolStripMenuItem cmiDataviewExportList;
        private System.Windows.Forms.ToolStripMenuItem cmiDataviewShowDependencies;
    }
}
