namespace GrinGlobal.Admin.ChildForms {
    partial class frmFiles {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFiles));
            this.cmFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.lv = new System.Windows.Forms.ListView();
            this.colDisplayName = new System.Windows.Forms.ColumnHeader();
            this.colFileName = new System.Windows.Forms.ColumnHeader();
            this.colVersion = new System.Windows.Forms.ColumnHeader();
            this.colSize = new System.Windows.Forms.ColumnHeader();
            this.colEnabled = new System.Windows.Forms.ColumnHeader();
            this.colVirtualFilePath = new System.Windows.Forms.ColumnHeader();
            this.colLastTouched = new System.Windows.Forms.ColumnHeader();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAddToGroup = new System.Windows.Forms.Button();
            this.btnNewFile = new System.Windows.Forms.Button();
            this.cmFilesExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmFiles
            // 
            this.cmFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiNew,
            this.toolStripSeparator1,
            this.cmiEnable,
            this.cmiDisable,
            this.cmiDelete,
            this.toolStripSeparator2,
            this.cmFilesExportList,
            this.cmiRefresh,
            this.cmiProperties});
            this.cmFiles.Name = "ctxMenuNodeUser";
            this.cmFiles.Size = new System.Drawing.Size(153, 192);
            this.cmFiles.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuFieldMapping_Opening);
            // 
            // cmiNew
            // 
            this.cmiNew.Name = "cmiNew";
            this.cmiNew.Size = new System.Drawing.Size(152, 22);
            this.cmiNew.Text = "&New File...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // cmiEnable
            // 
            this.cmiEnable.Name = "cmiEnable";
            this.cmiEnable.Size = new System.Drawing.Size(152, 22);
            this.cmiEnable.Text = "&Enable";
            // 
            // cmiDisable
            // 
            this.cmiDisable.Name = "cmiDisable";
            this.cmiDisable.Size = new System.Drawing.Size(152, 22);
            this.cmiDisable.Text = "Disable";
            this.cmiDisable.Click += new System.EventHandler(this.disableMenuItem_Click_1);
            // 
            // cmiDelete
            // 
            this.cmiDelete.Name = "cmiDelete";
            this.cmiDelete.Size = new System.Drawing.Size(152, 22);
            this.cmiDelete.Text = "&Delete";
            this.cmiDelete.Click += new System.EventHandler(this.deleteMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // cmiRefresh
            // 
            this.cmiRefresh.Name = "cmiRefresh";
            this.cmiRefresh.Size = new System.Drawing.Size(152, 22);
            this.cmiRefresh.Text = "&Refresh";
            this.cmiRefresh.Click += new System.EventHandler(this.refreshPermissionMenuItem_Click);
            // 
            // cmiProperties
            // 
            this.cmiProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiProperties.Name = "cmiProperties";
            this.cmiProperties.Size = new System.Drawing.Size(152, 22);
            this.cmiProperties.Text = "&Properties";
            this.cmiProperties.Click += new System.EventHandler(this.defaultPermissionMenuItem_Click);
            // 
            // lv
            // 
            this.lv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDisplayName,
            this.colFileName,
            this.colVersion,
            this.colSize,
            this.colEnabled,
            this.colVirtualFilePath,
            this.colLastTouched});
            this.lv.ContextMenuStrip = this.cmFiles;
            this.lv.FullRowSelect = true;
            this.lv.Location = new System.Drawing.Point(-1, -1);
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(634, 258);
            this.lv.TabIndex = 59;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            this.lv.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_MouseDoubleClick_1);
            this.lv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lv_KeyPress_1);
            this.lv.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lv_KeyUp_1);
            this.lv.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lv_ItemDrag);
            // 
            // colDisplayName
            // 
            this.colDisplayName.Text = "Display Name";
            this.colDisplayName.Width = 123;
            // 
            // colFileName
            // 
            this.colFileName.Text = "File Name";
            this.colFileName.Width = 96;
            // 
            // colVersion
            // 
            this.colVersion.Text = "Version";
            this.colVersion.Width = 74;
            // 
            // colSize
            // 
            this.colSize.Text = "Size (MB)";
            this.colSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colSize.Width = 65;
            // 
            // colEnabled
            // 
            this.colEnabled.Text = "Enabled?";
            this.colEnabled.Width = 66;
            // 
            // colVirtualFilePath
            // 
            this.colVirtualFilePath.Text = "Virtual Path";
            this.colVirtualFilePath.Width = 121;
            // 
            // colLastTouched
            // 
            this.colLastTouched.Text = "Last Touched";
            this.colLastTouched.Width = 116;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(545, 263);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 61;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_2);
            // 
            // btnAddToGroup
            // 
            this.btnAddToGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddToGroup.Location = new System.Drawing.Point(436, 263);
            this.btnAddToGroup.Name = "btnAddToGroup";
            this.btnAddToGroup.Size = new System.Drawing.Size(103, 23);
            this.btnAddToGroup.TabIndex = 60;
            this.btnAddToGroup.Text = "&Add To Group";
            this.btnAddToGroup.UseVisualStyleBackColor = true;
            this.btnAddToGroup.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnNewFile
            // 
            this.btnNewFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNewFile.Location = new System.Drawing.Point(12, 263);
            this.btnNewFile.Name = "btnNewFile";
            this.btnNewFile.Size = new System.Drawing.Size(86, 23);
            this.btnNewFile.TabIndex = 62;
            this.btnNewFile.Text = "New File...";
            this.btnNewFile.UseVisualStyleBackColor = true;
            this.btnNewFile.Click += new System.EventHandler(this.btnNewFile_Click);
            // 
            // cmFilesExportList
            // 
            this.cmFilesExportList.Name = "cmFilesExportList";
            this.cmFilesExportList.Size = new System.Drawing.Size(152, 22);
            this.cmFilesExportList.Text = "E&xport List";
            this.cmFilesExportList.Click += new System.EventHandler(this.cmFilesExportList_Click);
            // 
            // frmFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(632, 298);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNewFile);
            this.Controls.Add(this.btnAddToGroup);
            this.Controls.Add(this.lv);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmFiles";
            this.Text = "Files";
            this.Load += new System.EventHandler(this.frmFileGroup_Load);
            this.cmFiles.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmFiles;
        private System.Windows.Forms.ToolStripMenuItem cmiNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmiRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiProperties;
        private System.Windows.Forms.ListView lv;
        private System.Windows.Forms.ColumnHeader colDisplayName;
        private System.Windows.Forms.ColumnHeader colFileName;
        private System.Windows.Forms.ColumnHeader colVersion;
        private System.Windows.Forms.ColumnHeader colSize;
        private System.Windows.Forms.ColumnHeader colVirtualFilePath;
        private System.Windows.Forms.ColumnHeader colLastTouched;
        private System.Windows.Forms.ColumnHeader colEnabled;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAddToGroup;
        private System.Windows.Forms.Button btnNewFile;
        private System.Windows.Forms.ToolStripMenuItem cmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmiEnable;
        private System.Windows.Forms.ToolStripMenuItem cmiDisable;
        private System.Windows.Forms.ToolStripMenuItem cmFilesExportList;
    }
}
