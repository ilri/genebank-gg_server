namespace GrinGlobal.Admin.ChildForms {
    partial class frmCodeGroups {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCodeGroups));
            this.lvGroups = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colReferences = new System.Windows.Forms.ColumnHeader();
            this.colValues = new System.Windows.Forms.ColumnHeader();
            this.colLastTouched = new System.Windows.Forms.ColumnHeader();
            this.cmGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiGroupExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cmGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvGroups
            // 
            this.lvGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colReferences,
            this.colValues,
            this.colLastTouched});
            this.lvGroups.ContextMenuStrip = this.cmGroup;
            this.lvGroups.FullRowSelect = true;
            this.lvGroups.HideSelection = false;
            this.lvGroups.Location = new System.Drawing.Point(0, 0);
            this.lvGroups.Name = "lvGroups";
            this.lvGroups.Size = new System.Drawing.Size(674, 292);
            this.lvGroups.SmallImageList = this.imageList1;
            this.lvGroups.TabIndex = 2;
            this.lvGroups.UseCompatibleStateImageBehavior = false;
            this.lvGroups.View = System.Windows.Forms.View.Details;
            this.lvGroups.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvGroups_MouseDoubleClick);
            this.lvGroups.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvGroups_KeyPress);
            this.lvGroups.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvGroups_KeyUp);
            this.lvGroups.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvGroups_ItemDrag);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 200;
            // 
            // colReferences
            // 
            this.colReferences.Text = "Referenced By Tables/Dataviews";
            this.colReferences.Width = 200;
            // 
            // colValues
            // 
            this.colValues.Text = "Values";
            // 
            // colLastTouched
            // 
            this.colLastTouched.Text = "Last Touched";
            this.colLastTouched.Width = 150;
            // 
            // cmGroup
            // 
            this.cmGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiNew,
            this.toolStripSeparator1,
            this.cmiDelete,
            this.toolStripSeparator18,
            this.cmiGroupExportList,
            this.cmiRefresh,
            this.cmiProperties});
            this.cmGroup.Name = "ctxMenuNodeUser";
            this.cmGroup.Size = new System.Drawing.Size(144, 126);
            this.cmGroup.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuNodePermission_Opening);
            // 
            // cmiNew
            // 
            this.cmiNew.Name = "cmiNew";
            this.cmiNew.Size = new System.Drawing.Size(143, 22);
            this.cmiNew.Text = "&New Group...";
            this.cmiNew.Click += new System.EventHandler(this.newPermissionTemplateMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(140, 6);
            // 
            // cmiDelete
            // 
            this.cmiDelete.Name = "cmiDelete";
            this.cmiDelete.Size = new System.Drawing.Size(143, 22);
            this.cmiDelete.Text = "&Delete";
            this.cmiDelete.Click += new System.EventHandler(this.deletePermissionTemplateMenuItem_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(140, 6);
            // 
            // cmiGroupExportList
            // 
            this.cmiGroupExportList.Name = "cmiGroupExportList";
            this.cmiGroupExportList.Size = new System.Drawing.Size(143, 22);
            this.cmiGroupExportList.Text = "E&xport List";
            this.cmiGroupExportList.Click += new System.EventHandler(this.cmiGroupExportList_Click);
            // 
            // cmiRefresh
            // 
            this.cmiRefresh.Name = "cmiRefresh";
            this.cmiRefresh.Size = new System.Drawing.Size(143, 22);
            this.cmiRefresh.Text = "&Refresh";
            this.cmiRefresh.Click += new System.EventHandler(this.refreshPermissionTemplateMenuItem_Click);
            // 
            // cmiProperties
            // 
            this.cmiProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiProperties.Name = "cmiProperties";
            this.cmiProperties.Size = new System.Drawing.Size(143, 22);
            this.cmiProperties.Text = "&Properties";
            this.cmiProperties.Click += new System.EventHandler(this.defaultPermissionTemplateMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "code-group.ico");
            this.imageList1.Images.SetKeyName(1, "sysCVE.ico");
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(587, 298);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(506, 298);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "&Add";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmCodeGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(674, 333);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lvGroups);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCodeGroups";
            this.Text = "Code Groups";
            this.Load += new System.EventHandler(this.frmGroups_Load);
            this.cmGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvGroups;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ContextMenuStrip cmGroup;
        private System.Windows.Forms.ToolStripMenuItem cmiNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem cmiRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiProperties;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem cmiGroupExportList;
        private System.Windows.Forms.ColumnHeader colValues;
        private System.Windows.Forms.ColumnHeader colReferences;
        private System.Windows.Forms.ColumnHeader colLastTouched;
    }
}
