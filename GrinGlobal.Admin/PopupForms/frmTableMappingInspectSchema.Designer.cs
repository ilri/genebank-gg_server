namespace GrinGlobal.Admin.PopupForms {
    partial class frmTableMappingInspectSchema {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTableMappingInspectSchema));
            this.lvMissingTableMappings = new System.Windows.Forms.ListView();
            this.colTableName = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerateDefaults = new System.Windows.Forms.Button();
            this.chkShowAllTables = new System.Windows.Forms.CheckBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lvMissingTableMappings
            // 
            this.lvMissingTableMappings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMissingTableMappings.CheckBoxes = true;
            this.lvMissingTableMappings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTableName});
            this.lvMissingTableMappings.Location = new System.Drawing.Point(12, 70);
            this.lvMissingTableMappings.Name = "lvMissingTableMappings";
            this.lvMissingTableMappings.Size = new System.Drawing.Size(390, 205);
            this.lvMissingTableMappings.SmallImageList = this.imageList1;
            this.lvMissingTableMappings.TabIndex = 0;
            this.lvMissingTableMappings.UseCompatibleStateImageBehavior = false;
            this.lvMissingTableMappings.View = System.Windows.Forms.View.Details;
            // 
            // colTableName
            // 
            this.colTableName.Text = "Table Name";
            this.colTableName.Width = 200;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "table.ico");
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(12, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(389, 33);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Inspecting database schema, please be patient...";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(327, 281);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerateDefaults
            // 
            this.btnGenerateDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateDefaults.Location = new System.Drawing.Point(154, 281);
            this.btnGenerateDefaults.Name = "btnGenerateDefaults";
            this.btnGenerateDefaults.Size = new System.Drawing.Size(167, 23);
            this.btnGenerateDefaults.TabIndex = 3;
            this.btnGenerateDefaults.Text = "Generate Default Mappings";
            this.btnGenerateDefaults.UseVisualStyleBackColor = true;
            this.btnGenerateDefaults.Click += new System.EventHandler(this.btnGenerateDefaults_Click);
            // 
            // chkShowAllTables
            // 
            this.chkShowAllTables.AutoSize = true;
            this.chkShowAllTables.Location = new System.Drawing.Point(12, 47);
            this.chkShowAllTables.Name = "chkShowAllTables";
            this.chkShowAllTables.Size = new System.Drawing.Size(102, 17);
            this.chkShowAllTables.TabIndex = 4;
            this.chkShowAllTables.Text = "Show All Tables";
            this.chkShowAllTables.UseVisualStyleBackColor = true;
            this.chkShowAllTables.CheckedChanged += new System.EventHandler(this.chkShowAllTables_CheckedChanged);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(154, 47);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(70, 17);
            this.chkSelectAll.TabIndex = 5;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // frmTableMappingInspectSchema
            // 
            this.AcceptButton = this.btnGenerateDefaults;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(414, 316);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.chkShowAllTables);
            this.Controls.Add(this.btnGenerateDefaults);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lvMissingTableMappings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTableMappingInspectSchema";
            this.Text = "Schema Inspection Results";
            this.Load += new System.EventHandler(this.frmTableMappingInspectSchema_Load);
            this.Activated += new System.EventHandler(this.frmTableMappingInspectSchema_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvMissingTableMappings;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerateDefaults;
        private System.Windows.Forms.ColumnHeader colTableName;
        private System.Windows.Forms.CheckBox chkShowAllTables;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.CheckBox chkSelectAll;
    }
}