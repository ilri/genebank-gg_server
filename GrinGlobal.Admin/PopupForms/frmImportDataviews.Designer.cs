namespace GrinGlobal.Admin.PopupForms {
    partial class frmImportDataviews {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImportDataviews));
            this.lvDataviews = new System.Windows.Forms.ListView();
            this.colDataviewName = new System.Windows.Forms.ColumnHeader();
            this.colLastTouched = new System.Windows.Forms.ColumnHeader();
            this.colCategory = new System.Windows.Forms.ColumnHeader();
            this.colDatabaseArea = new System.Windows.Forms.ColumnHeader();
            this.colTitle = new System.Windows.Forms.ColumnHeader();
            this.colDescription = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.chkSQLServer = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkOracle = new System.Windows.Forms.CheckBox();
            this.chkMySQL = new System.Windows.Forms.CheckBox();
            this.chkPostgreSQL = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.statLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbLanguage = new System.Windows.Forms.GroupBox();
            this.chkUseAsTableMappings = new System.Windows.Forms.CheckBox();
            this.chkIncludeLanguage = new System.Windows.Forms.CheckBox();
            this.lblServerName = new System.Windows.Forms.Label();
            this.gbOtherSettings = new System.Windows.Forms.GroupBox();
            this.chkFieldsAndParameters = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.gbLanguage.SuspendLayout();
            this.gbOtherSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvDataviews
            // 
            this.lvDataviews.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDataviews.CheckBoxes = true;
            this.lvDataviews.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDataviewName,
            this.colLastTouched,
            this.colCategory,
            this.colDatabaseArea,
            this.colTitle,
            this.colDescription});
            this.lvDataviews.FullRowSelect = true;
            this.lvDataviews.LabelEdit = true;
            this.lvDataviews.Location = new System.Drawing.Point(12, 35);
            this.lvDataviews.Name = "lvDataviews";
            this.lvDataviews.Size = new System.Drawing.Size(561, 171);
            this.lvDataviews.SmallImageList = this.imageList1;
            this.lvDataviews.TabIndex = 0;
            this.lvDataviews.UseCompatibleStateImageBehavior = false;
            this.lvDataviews.View = System.Windows.Forms.View.Details;
            this.lvDataviews.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvDataviews_AfterLabelEdit);
            // 
            // colDataviewName
            // 
            this.colDataviewName.Text = "Dataview";
            this.colDataviewName.Width = 300;
            // 
            // colLastTouched
            // 
            this.colLastTouched.Text = "Last Touched";
            this.colLastTouched.Width = 150;
            // 
            // colCategory
            // 
            this.colCategory.Text = "Category";
            this.colCategory.Width = 120;
            // 
            // colDatabaseArea
            // 
            this.colDatabaseArea.Text = "Database Area";
            this.colDatabaseArea.Width = 120;
            // 
            // colTitle
            // 
            this.colTitle.Text = "Title";
            this.colTitle.Width = 150;
            // 
            // colDescription
            // 
            this.colDescription.Text = "Description";
            this.colDescription.Width = 120;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "dataviews.ico");
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(498, 387);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(407, 387);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(85, 23);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "&Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(12, 12);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(70, 17);
            this.chkSelectAll.TabIndex = 4;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkShowAllTables_CheckedChanged);
            // 
            // chkSQLServer
            // 
            this.chkSQLServer.Location = new System.Drawing.Point(41, 19);
            this.chkSQLServer.Name = "chkSQLServer";
            this.chkSQLServer.Size = new System.Drawing.Size(129, 24);
            this.chkSQLServer.TabIndex = 5;
            this.chkSQLServer.Text = "SQL Server";
            this.chkSQLServer.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.chkOracle);
            this.groupBox1.Controls.Add(this.chkMySQL);
            this.groupBox1.Controls.Add(this.chkPostgreSQL);
            this.groupBox1.Controls.Add(this.chkSQLServer);
            this.groupBox1.Location = new System.Drawing.Point(12, 212);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(217, 168);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import SQL For...";
            // 
            // chkOracle
            // 
            this.chkOracle.Location = new System.Drawing.Point(41, 109);
            this.chkOracle.Name = "chkOracle";
            this.chkOracle.Size = new System.Drawing.Size(129, 24);
            this.chkOracle.TabIndex = 8;
            this.chkOracle.Text = "Oracle";
            this.chkOracle.UseVisualStyleBackColor = true;
            // 
            // chkMySQL
            // 
            this.chkMySQL.Location = new System.Drawing.Point(41, 79);
            this.chkMySQL.Name = "chkMySQL";
            this.chkMySQL.Size = new System.Drawing.Size(129, 24);
            this.chkMySQL.TabIndex = 7;
            this.chkMySQL.Text = "MySQL";
            this.chkMySQL.UseVisualStyleBackColor = true;
            // 
            // chkPostgreSQL
            // 
            this.chkPostgreSQL.Location = new System.Drawing.Point(41, 49);
            this.chkPostgreSQL.Name = "chkPostgreSQL";
            this.chkPostgreSQL.Size = new System.Drawing.Size(129, 24);
            this.chkPostgreSQL.TabIndex = 6;
            this.chkPostgreSQL.Text = "PostgreSQL";
            this.chkPostgreSQL.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statProgress,
            this.statLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 413);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(585, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statProgress
            // 
            this.statProgress.Name = "statProgress";
            this.statProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // statLabel
            // 
            this.statLabel.Name = "statLabel";
            this.statLabel.Size = new System.Drawing.Size(10, 17);
            this.statLabel.Text = " ";
            // 
            // gbLanguage
            // 
            this.gbLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLanguage.Controls.Add(this.chkUseAsTableMappings);
            this.gbLanguage.Controls.Add(this.chkIncludeLanguage);
            this.gbLanguage.Location = new System.Drawing.Point(10, 60);
            this.gbLanguage.Name = "gbLanguage";
            this.gbLanguage.Size = new System.Drawing.Size(322, 96);
            this.gbLanguage.TabIndex = 8;
            this.gbLanguage.TabStop = false;
            this.gbLanguage.Text = "Language";
            // 
            // chkUseAsTableMappings
            // 
            this.chkUseAsTableMappings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkUseAsTableMappings.Location = new System.Drawing.Point(40, 49);
            this.chkUseAsTableMappings.Name = "chkUseAsTableMappings";
            this.chkUseAsTableMappings.Size = new System.Drawing.Size(276, 34);
            this.chkUseAsTableMappings.TabIndex = 1;
            this.chkUseAsTableMappings.Text = "Use as defaults in table mappings";
            this.chkUseAsTableMappings.UseVisualStyleBackColor = true;
            // 
            // chkIncludeLanguage
            // 
            this.chkIncludeLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIncludeLanguage.Checked = true;
            this.chkIncludeLanguage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeLanguage.Location = new System.Drawing.Point(18, 19);
            this.chkIncludeLanguage.Name = "chkIncludeLanguage";
            this.chkIncludeLanguage.Size = new System.Drawing.Size(298, 34);
            this.chkIncludeLanguage.TabIndex = 0;
            this.chkIncludeLanguage.Text = "Include language-specific field names and titles";
            this.chkIncludeLanguage.UseVisualStyleBackColor = true;
            this.chkIncludeLanguage.CheckedChanged += new System.EventHandler(this.chkIncludeLanguage_CheckedChanged);
            // 
            // lblServerName
            // 
            this.lblServerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(13, 387);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(107, 13);
            this.lblServerName.TabIndex = 9;
            this.lblServerName.Text = "-- server name here --";
            // 
            // gbOtherSettings
            // 
            this.gbOtherSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbOtherSettings.Controls.Add(this.chkFieldsAndParameters);
            this.gbOtherSettings.Controls.Add(this.gbLanguage);
            this.gbOtherSettings.Location = new System.Drawing.Point(235, 212);
            this.gbOtherSettings.Name = "gbOtherSettings";
            this.gbOtherSettings.Size = new System.Drawing.Size(338, 168);
            this.gbOtherSettings.TabIndex = 12;
            this.gbOtherSettings.TabStop = false;
            this.gbOtherSettings.Text = "Other Settings";
            // 
            // chkFieldsAndParameters
            // 
            this.chkFieldsAndParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFieldsAndParameters.Checked = true;
            this.chkFieldsAndParameters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFieldsAndParameters.Location = new System.Drawing.Point(18, 19);
            this.chkFieldsAndParameters.Name = "chkFieldsAndParameters";
            this.chkFieldsAndParameters.Size = new System.Drawing.Size(314, 34);
            this.chkFieldsAndParameters.TabIndex = 0;
            this.chkFieldsAndParameters.Text = "Include Fields, Properties, and Parameters";
            this.chkFieldsAndParameters.UseVisualStyleBackColor = true;
            this.chkFieldsAndParameters.CheckedChanged += new System.EventHandler(this.chkFieldsAndParameters_CheckedChanged);
            // 
            // frmImportDataviews
            // 
            this.AcceptButton = this.btnImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(585, 435);
            this.ControlBox = true;
            this.Controls.Add(this.gbOtherSettings);
            this.Controls.Add(this.lblServerName);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lvDataviews);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = true;
            this.Name = "frmImportDataviews";
            this.Text = "Select Dataview(s) to Import";
            this.Load += new System.EventHandler(this.frmTableMappingInspectSchema_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmImportDataviews_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.gbLanguage.ResumeLayout(false);
            this.gbOtherSettings.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvDataviews;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ColumnHeader colDataviewName;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.CheckBox chkSQLServer;
        public System.Windows.Forms.CheckBox chkOracle;
        public System.Windows.Forms.CheckBox chkMySQL;
        public System.Windows.Forms.CheckBox chkPostgreSQL;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar statProgress;
        private System.Windows.Forms.ToolStripStatusLabel statLabel;
        private System.Windows.Forms.ColumnHeader colLastTouched;
        private System.Windows.Forms.ColumnHeader colCategory;
        private System.Windows.Forms.ColumnHeader colDatabaseArea;
        private System.Windows.Forms.ColumnHeader colTitle;
        private System.Windows.Forms.ColumnHeader colDescription;
        private System.Windows.Forms.GroupBox gbLanguage;
        private System.Windows.Forms.CheckBox chkUseAsTableMappings;
        private System.Windows.Forms.CheckBox chkIncludeLanguage;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.GroupBox gbOtherSettings;
        private System.Windows.Forms.CheckBox chkFieldsAndParameters;
    }
}