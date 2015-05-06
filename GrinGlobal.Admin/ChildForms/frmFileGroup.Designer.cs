namespace GrinGlobal.Admin.ChildForms {
    partial class frmFileGroup {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFileGroup));
            this.cmFileGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiAddFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiFileGroupExportList = new System.Windows.Forms.ToolStripMenuItem();
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
            this.gbFiles = new System.Windows.Forms.GroupBox();
            this.btnAddFile = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.cmFileGroup.SuspendLayout();
            this.gbFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmFileGroup
            // 
            this.cmFileGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiAddFile,
            this.toolStripSeparator1,
            this.cmiRemove,
            this.cmiEnable,
            this.cmiDisable,
            this.toolStripSeparator18,
            this.cmiFileGroupExportList,
            this.cmiRefresh,
            this.cmiProperties});
            this.cmFileGroup.Name = "ctxMenuNodeUser";
            this.cmFileGroup.Size = new System.Drawing.Size(133, 170);
            this.cmFileGroup.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuFieldMapping_Opening);
            // 
            // cmiAddFile
            // 
            this.cmiAddFile.Name = "cmiAddFile";
            this.cmiAddFile.Size = new System.Drawing.Size(132, 22);
            this.cmiAddFile.Text = "&Add File...";
            this.cmiAddFile.Click += new System.EventHandler(this.newMenuItem_Click_1);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(129, 6);
            // 
            // cmiRemove
            // 
            this.cmiRemove.Name = "cmiRemove";
            this.cmiRemove.Size = new System.Drawing.Size(132, 22);
            this.cmiRemove.Text = "&Remove";
            this.cmiRemove.Click += new System.EventHandler(this.removeMenuItem_Click);
            // 
            // cmiEnable
            // 
            this.cmiEnable.Name = "cmiEnable";
            this.cmiEnable.Size = new System.Drawing.Size(132, 22);
            this.cmiEnable.Text = "&Enable";
            this.cmiEnable.Click += new System.EventHandler(this.enableMenuItem_Click);
            // 
            // cmiDisable
            // 
            this.cmiDisable.Name = "cmiDisable";
            this.cmiDisable.Size = new System.Drawing.Size(132, 22);
            this.cmiDisable.Text = "&Disable";
            this.cmiDisable.Click += new System.EventHandler(this.disableMenuItem_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(129, 6);
            // 
            // cmiFileGroupExportList
            // 
            this.cmiFileGroupExportList.Name = "cmiFileGroupExportList";
            this.cmiFileGroupExportList.Size = new System.Drawing.Size(132, 22);
            this.cmiFileGroupExportList.Text = "E&xport List";
            this.cmiFileGroupExportList.Click += new System.EventHandler(this.cmiFileGroupExportList_Click);
            // 
            // cmiRefresh
            // 
            this.cmiRefresh.Name = "cmiRefresh";
            this.cmiRefresh.Size = new System.Drawing.Size(132, 22);
            this.cmiRefresh.Text = "&Refresh";
            this.cmiRefresh.Click += new System.EventHandler(this.refreshPermissionMenuItem_Click);
            // 
            // cmiProperties
            // 
            this.cmiProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiProperties.Name = "cmiProperties";
            this.cmiProperties.Size = new System.Drawing.Size(132, 22);
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
            this.lv.ContextMenuStrip = this.cmFileGroup;
            this.lv.FullRowSelect = true;
            this.lv.Location = new System.Drawing.Point(6, 19);
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(596, 217);
            this.lv.TabIndex = 59;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            this.lv.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_MouseDoubleClick_1);
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
            // gbFiles
            // 
            this.gbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFiles.Controls.Add(this.btnAddFile);
            this.gbFiles.Controls.Add(this.lv);
            this.gbFiles.Location = new System.Drawing.Point(12, 88);
            this.gbFiles.Name = "gbFiles";
            this.gbFiles.Size = new System.Drawing.Size(608, 271);
            this.gbFiles.TabIndex = 60;
            this.gbFiles.TabStop = false;
            this.gbFiles.Text = "Files";
            // 
            // btnAddFile
            // 
            this.btnAddFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFile.Location = new System.Drawing.Point(527, 242);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(75, 23);
            this.btnAddFile.TabIndex = 60;
            this.btnAddFile.Text = "&Add...";
            this.btnAddFile.UseVisualStyleBackColor = true;
            this.btnAddFile.Click += new System.EventHandler(this.btnAddFile_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(545, 365);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 61;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_2);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(464, 365);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 62;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_2);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(115, 12);
            this.txtName.MaxLength = 100;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(212, 20);
            this.txtName.TabIndex = 63;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(18, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(91, 13);
            this.lblName.TabIndex = 64;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblVersion
            // 
            this.lblVersion.Location = new System.Drawing.Point(12, 41);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(97, 13);
            this.lblVersion.TabIndex = 66;
            this.lblVersion.Text = "Version:";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(115, 38);
            this.txtVersion.MaxLength = 50;
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(212, 20);
            this.txtVersion.TabIndex = 65;
            this.txtVersion.TextChanged += new System.EventHandler(this.txtVersion_TextChanged);
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Location = new System.Drawing.Point(115, 64);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(71, 17);
            this.chkEnabled.TabIndex = 67;
            this.chkEnabled.Text = "Enabled?";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // frmFileGroup
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(632, 400);
            this.Controls.Add(this.chkEnabled);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.txtVersion);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.gbFiles);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmFileGroup";
            this.Text = "File Group Detail";
            this.Load += new System.EventHandler(this.frmFileGroup_Load);
            this.cmFileGroup.ResumeLayout(false);
            this.gbFiles.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmFileGroup;
        private System.Windows.Forms.ToolStripMenuItem cmiAddFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmiRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
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
        private System.Windows.Forms.ToolStripMenuItem cmiEnable;
        private System.Windows.Forms.ToolStripMenuItem cmiDisable;
        private System.Windows.Forms.GroupBox gbFiles;
        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.ToolStripMenuItem cmiFileGroupExportList;
    }
}
