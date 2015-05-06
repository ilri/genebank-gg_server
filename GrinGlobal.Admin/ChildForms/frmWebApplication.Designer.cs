namespace GrinGlobal.Admin.ChildForms {
    partial class frmWebApplication {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWebApplication));
            this.lv = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colValue = new System.Windows.Forms.ColumnHeader();
            this.cmAppSettings = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiAppSettingsNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiAppSettingsDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiAppSettingsRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiAppSettingsProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.gbAppSettings = new System.Windows.Forms.GroupBox();
            this.gbConnectionStrings = new System.Windows.Forms.GroupBox();
            this.lvConnectionStrings = new System.Windows.Forms.ListView();
            this.colConnName = new System.Windows.Forms.ColumnHeader();
            this.colConnProvider = new System.Windows.Forms.ColumnHeader();
            this.colConnValue = new System.Windows.Forms.ColumnHeader();
            this.cmConnectionStrings = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiConnectionStringsNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiConnectionStringsDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiConnectionStringsRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiConnectionStringsProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClearAllCaches = new System.Windows.Forms.Button();
            this.cmiAppSettingsExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiConnectionStringsExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmAppSettings.SuspendLayout();
            this.gbAppSettings.SuspendLayout();
            this.gbConnectionStrings.SuspendLayout();
            this.cmConnectionStrings.SuspendLayout();
            this.SuspendLayout();
            // 
            // lv
            // 
            this.lv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colValue});
            this.lv.ContextMenuStrip = this.cmAppSettings;
            this.lv.FullRowSelect = true;
            this.lv.HideSelection = false;
            this.lv.Location = new System.Drawing.Point(6, 19);
            this.lv.Name = "lv";
            this.lv.ShowItemToolTips = true;
            this.lv.Size = new System.Drawing.Size(598, 165);
            this.lv.SmallImageList = this.imageList1;
            this.lv.TabIndex = 1;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            this.lv.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_MouseDoubleClick);
            this.lv.SelectedIndexChanged += new System.EventHandler(this.lv_SelectedIndexChanged_1);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 200;
            // 
            // colValue
            // 
            this.colValue.Text = "Value";
            this.colValue.Width = 300;
            // 
            // cmAppSettings
            // 
            this.cmAppSettings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiAppSettingsNew,
            this.toolStripSeparator1,
            this.cmiAppSettingsDelete,
            this.toolStripSeparator18,
            this.cmiAppSettingsExportList,
            this.cmiAppSettingsRefresh,
            this.cmiAppSettingsProperties});
            this.cmAppSettings.Name = "ctxMenuNodeUser";
            this.cmAppSettings.Size = new System.Drawing.Size(212, 126);
            this.cmAppSettings.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuNodePermission_Opening);
            // 
            // cmiAppSettingsNew
            // 
            this.cmiAppSettingsNew.Name = "cmiAppSettingsNew";
            this.cmiAppSettingsNew.Size = new System.Drawing.Size(211, 22);
            this.cmiAppSettingsNew.Text = "&New Application Setting...";
            this.cmiAppSettingsNew.Click += new System.EventHandler(this.newMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(208, 6);
            // 
            // cmiAppSettingsDelete
            // 
            this.cmiAppSettingsDelete.Name = "cmiAppSettingsDelete";
            this.cmiAppSettingsDelete.Size = new System.Drawing.Size(211, 22);
            this.cmiAppSettingsDelete.Text = "&Delete";
            this.cmiAppSettingsDelete.Click += new System.EventHandler(this.deletePermissionMenuItem_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(208, 6);
            // 
            // cmiAppSettingsRefresh
            // 
            this.cmiAppSettingsRefresh.Name = "cmiAppSettingsRefresh";
            this.cmiAppSettingsRefresh.Size = new System.Drawing.Size(211, 22);
            this.cmiAppSettingsRefresh.Text = "&Refresh";
            this.cmiAppSettingsRefresh.Click += new System.EventHandler(this.refreshPermissionMenuItem_Click);
            // 
            // cmiAppSettingsProperties
            // 
            this.cmiAppSettingsProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiAppSettingsProperties.Name = "cmiAppSettingsProperties";
            this.cmiAppSettingsProperties.Size = new System.Drawing.Size(211, 22);
            this.cmiAppSettingsProperties.Text = "&Properties";
            this.cmiAppSettingsProperties.Click += new System.EventHandler(this.defaultPermissionMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "webappsetting.ico");
            this.imageList1.Images.SetKeyName(1, "connectionstring.ico");
            // 
            // gbAppSettings
            // 
            this.gbAppSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAppSettings.Controls.Add(this.lv);
            this.gbAppSettings.Location = new System.Drawing.Point(12, 12);
            this.gbAppSettings.Name = "gbAppSettings";
            this.gbAppSettings.Size = new System.Drawing.Size(610, 190);
            this.gbAppSettings.TabIndex = 3;
            this.gbAppSettings.TabStop = false;
            this.gbAppSettings.Text = "Application Settings";
            // 
            // gbConnectionStrings
            // 
            this.gbConnectionStrings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbConnectionStrings.Controls.Add(this.lvConnectionStrings);
            this.gbConnectionStrings.Location = new System.Drawing.Point(12, 208);
            this.gbConnectionStrings.Name = "gbConnectionStrings";
            this.gbConnectionStrings.Size = new System.Drawing.Size(610, 126);
            this.gbConnectionStrings.TabIndex = 4;
            this.gbConnectionStrings.TabStop = false;
            this.gbConnectionStrings.Text = "Connection Strings";
            // 
            // lvConnectionStrings
            // 
            this.lvConnectionStrings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvConnectionStrings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colConnName,
            this.colConnProvider,
            this.colConnValue});
            this.lvConnectionStrings.ContextMenuStrip = this.cmConnectionStrings;
            this.lvConnectionStrings.FullRowSelect = true;
            this.lvConnectionStrings.HideSelection = false;
            this.lvConnectionStrings.Location = new System.Drawing.Point(6, 19);
            this.lvConnectionStrings.Name = "lvConnectionStrings";
            this.lvConnectionStrings.ShowItemToolTips = true;
            this.lvConnectionStrings.Size = new System.Drawing.Size(598, 101);
            this.lvConnectionStrings.SmallImageList = this.imageList1;
            this.lvConnectionStrings.TabIndex = 1;
            this.lvConnectionStrings.UseCompatibleStateImageBehavior = false;
            this.lvConnectionStrings.View = System.Windows.Forms.View.Details;
            this.lvConnectionStrings.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvConnectionStrings_MouseDoubleClick);
            // 
            // colConnName
            // 
            this.colConnName.Text = "Name";
            this.colConnName.Width = 200;
            // 
            // colConnProvider
            // 
            this.colConnProvider.Text = "Provider";
            this.colConnProvider.Width = 80;
            // 
            // colConnValue
            // 
            this.colConnValue.Text = "Connection String";
            this.colConnValue.Width = 300;
            // 
            // cmConnectionStrings
            // 
            this.cmConnectionStrings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiConnectionStringsNew,
            this.toolStripSeparator2,
            this.cmiConnectionStringsDelete,
            this.toolStripSeparator3,
            this.cmiConnectionStringsExportList,
            this.cmiConnectionStringsRefresh,
            this.cmiConnectionStringsProperties});
            this.cmConnectionStrings.Name = "ctxMenuNodeUser";
            this.cmConnectionStrings.Size = new System.Drawing.Size(207, 148);
            // 
            // cmiConnectionStringsNew
            // 
            this.cmiConnectionStringsNew.Name = "cmiConnectionStringsNew";
            this.cmiConnectionStringsNew.Size = new System.Drawing.Size(206, 22);
            this.cmiConnectionStringsNew.Text = "&New Connection String...";
            this.cmiConnectionStringsNew.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(203, 6);
            // 
            // cmiConnectionStringsDelete
            // 
            this.cmiConnectionStringsDelete.Name = "cmiConnectionStringsDelete";
            this.cmiConnectionStringsDelete.Size = new System.Drawing.Size(206, 22);
            this.cmiConnectionStringsDelete.Text = "&Delete";
            this.cmiConnectionStringsDelete.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(203, 6);
            // 
            // cmiConnectionStringsRefresh
            // 
            this.cmiConnectionStringsRefresh.Name = "cmiConnectionStringsRefresh";
            this.cmiConnectionStringsRefresh.Size = new System.Drawing.Size(206, 22);
            this.cmiConnectionStringsRefresh.Text = "&Refresh";
            this.cmiConnectionStringsRefresh.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // cmiConnectionStringsProperties
            // 
            this.cmiConnectionStringsProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiConnectionStringsProperties.Name = "cmiConnectionStringsProperties";
            this.cmiConnectionStringsProperties.Size = new System.Drawing.Size(206, 22);
            this.cmiConnectionStringsProperties.Text = "&Properties";
            this.cmiConnectionStringsProperties.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // btnClearAllCaches
            // 
            this.btnClearAllCaches.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearAllCaches.Location = new System.Drawing.Point(12, 340);
            this.btnClearAllCaches.Name = "btnClearAllCaches";
            this.btnClearAllCaches.Size = new System.Drawing.Size(109, 23);
            this.btnClearAllCaches.TabIndex = 5;
            this.btnClearAllCaches.Text = "Clear All Caches";
            this.btnClearAllCaches.UseVisualStyleBackColor = true;
            this.btnClearAllCaches.Click += new System.EventHandler(this.btnClearAllCaches_Click);
            // 
            // cmiAppSettingsExportList
            // 
            this.cmiAppSettingsExportList.Name = "cmiAppSettingsExportList";
            this.cmiAppSettingsExportList.Size = new System.Drawing.Size(211, 22);
            this.cmiAppSettingsExportList.Text = "E&xport List";
            this.cmiAppSettingsExportList.Click += new System.EventHandler(this.cmiAppSettingsExportList_Click);
            // 
            // cmiConnectionStringsExportList
            // 
            this.cmiConnectionStringsExportList.Name = "cmiConnectionStringsExportList";
            this.cmiConnectionStringsExportList.Size = new System.Drawing.Size(206, 22);
            this.cmiConnectionStringsExportList.Text = "E&xport List";
            this.cmiConnectionStringsExportList.Click += new System.EventHandler(this.cmiConnectionStringsExportList_Click);
            // 
            // frmWebApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(634, 375);
            this.Controls.Add(this.btnClearAllCaches);
            this.Controls.Add(this.gbAppSettings);
            this.Controls.Add(this.gbConnectionStrings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmWebApplication";
            this.Text = "Web Application";
            this.Load += new System.EventHandler(this.frmTableMappings_Load);
            this.cmAppSettings.ResumeLayout(false);
            this.gbAppSettings.ResumeLayout(false);
            this.gbConnectionStrings.ResumeLayout(false);
            this.cmConnectionStrings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lv;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ContextMenuStrip cmAppSettings;
        private System.Windows.Forms.ToolStripMenuItem cmiAppSettingsDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem cmiAppSettingsRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiAppSettingsProperties;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.ToolStripMenuItem cmiAppSettingsNew;
        private System.Windows.Forms.GroupBox gbAppSettings;
        private System.Windows.Forms.GroupBox gbConnectionStrings;
        private System.Windows.Forms.ListView lvConnectionStrings;
        private System.Windows.Forms.ColumnHeader colConnName;
        private System.Windows.Forms.ColumnHeader colConnValue;
        private System.Windows.Forms.ColumnHeader colConnProvider;
        private System.Windows.Forms.Button btnClearAllCaches;
        private System.Windows.Forms.ContextMenuStrip cmConnectionStrings;
        private System.Windows.Forms.ToolStripMenuItem cmiConnectionStringsNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmiConnectionStringsDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cmiConnectionStringsRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiConnectionStringsProperties;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem cmiAppSettingsExportList;
        private System.Windows.Forms.ToolStripMenuItem cmiConnectionStringsExportList;
    }
}
