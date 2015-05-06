namespace GrinGlobal.Admin.ChildForms {
    partial class frmPermissionTemplates {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPermissionTemplates));
            this.lvPermissionTemplates = new System.Windows.Forms.ListView();
            this.colPermissionTemplateName = new System.Windows.Forms.ColumnHeader();
            this.colPermissions = new System.Windows.Forms.ColumnHeader();
            this.cmTemplate = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cmiTemplateExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmTemplate.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvPermissionTemplates
            // 
            this.lvPermissionTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPermissionTemplates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPermissionTemplateName,
            this.colPermissions});
            this.lvPermissionTemplates.ContextMenuStrip = this.cmTemplate;
            this.lvPermissionTemplates.FullRowSelect = true;
            this.lvPermissionTemplates.HideSelection = false;
            this.lvPermissionTemplates.Location = new System.Drawing.Point(0, 0);
            this.lvPermissionTemplates.Name = "lvPermissionTemplates";
            this.lvPermissionTemplates.Size = new System.Drawing.Size(561, 190);
            this.lvPermissionTemplates.SmallImageList = this.imageList1;
            this.lvPermissionTemplates.TabIndex = 2;
            this.lvPermissionTemplates.UseCompatibleStateImageBehavior = false;
            this.lvPermissionTemplates.View = System.Windows.Forms.View.Details;
            this.lvPermissionTemplates.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvPermissionTemplates_MouseDoubleClick);
            this.lvPermissionTemplates.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvPermissionTemplates_KeyPress);
            this.lvPermissionTemplates.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvPermissionTemplates_KeyUp);
            this.lvPermissionTemplates.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvPermissionTemplates_ItemDrag);
            // 
            // colPermissionTemplateName
            // 
            this.colPermissionTemplateName.Text = "Template Name";
            this.colPermissionTemplateName.Width = 150;
            // 
            // colPermissions
            // 
            this.colPermissions.Text = "Permissions";
            this.colPermissions.Width = 400;
            // 
            // cmTemplate
            // 
            this.cmTemplate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiNew,
            this.toolStripSeparator1,
            this.cmiDelete,
            this.toolStripSeparator18,
            this.cmiTemplateExportList,
            this.cmiRefresh,
            this.cmiProperties});
            this.cmTemplate.Name = "ctxMenuNodeUser";
            this.cmTemplate.Size = new System.Drawing.Size(222, 148);
            this.cmTemplate.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuNodePermission_Opening);
            // 
            // cmiNew
            // 
            this.cmiNew.Name = "cmiNew";
            this.cmiNew.Size = new System.Drawing.Size(221, 22);
            this.cmiNew.Text = "&New Permission Template...";
            this.cmiNew.Click += new System.EventHandler(this.newPermissionTemplateMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(218, 6);
            // 
            // cmiDelete
            // 
            this.cmiDelete.Name = "cmiDelete";
            this.cmiDelete.Size = new System.Drawing.Size(221, 22);
            this.cmiDelete.Text = "&Delete";
            this.cmiDelete.Click += new System.EventHandler(this.deletePermissionTemplateMenuItem_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(218, 6);
            // 
            // cmiRefresh
            // 
            this.cmiRefresh.Name = "cmiRefresh";
            this.cmiRefresh.Size = new System.Drawing.Size(221, 22);
            this.cmiRefresh.Text = "&Refresh";
            this.cmiRefresh.Click += new System.EventHandler(this.refreshPermissionTemplateMenuItem_Click);
            // 
            // cmiProperties
            // 
            this.cmiProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiProperties.Name = "cmiProperties";
            this.cmiProperties.Size = new System.Drawing.Size(221, 22);
            this.cmiProperties.Text = "&Properties";
            this.cmiProperties.Click += new System.EventHandler(this.defaultPermissionTemplateMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "template.png");
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(474, 196);
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
            this.btnOK.Location = new System.Drawing.Point(393, 196);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "&Add";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cmiTemplateExportList
            // 
            this.cmiTemplateExportList.Name = "cmiTemplateExportList";
            this.cmiTemplateExportList.Size = new System.Drawing.Size(221, 22);
            this.cmiTemplateExportList.Text = "E&xport List";
            this.cmiTemplateExportList.Click += new System.EventHandler(this.cmiTemplateExportList_Click);
            // 
            // frmPermissionTemplates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(561, 231);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lvPermissionTemplates);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPermissionTemplates";
            this.Text = "Permission Templates";
            this.Load += new System.EventHandler(this.frmPermissionTemplates_Load);
            this.cmTemplate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvPermissionTemplates;
        private System.Windows.Forms.ColumnHeader colPermissionTemplateName;
        private System.Windows.Forms.ColumnHeader colPermissions;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ContextMenuStrip cmTemplate;
        private System.Windows.Forms.ToolStripMenuItem cmiNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem cmiRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiProperties;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem cmiTemplateExportList;
    }
}
