namespace GrinGlobal.Admin.ChildForms {
    partial class frmDataTrigger {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataTrigger));
            this.lblDataview = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ddlDataview = new System.Windows.Forms.ComboBox();
            this.ddlTable = new System.Windows.Forms.ComboBox();
            this.lblTable = new System.Windows.Forms.Label();
            this.lblClass = new System.Windows.Forms.Label();
            this.ddlClass = new System.Windows.Forms.ComboBox();
            this.btnFile = new System.Windows.Forms.Button();
            this.lblAssembly = new System.Windows.Forms.Label();
            this.ofdTriggerDetail = new System.Windows.Forms.OpenFileDialog();
            this.ddlAssembly = new System.Windows.Forms.ComboBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDataview
            // 
            this.lblDataview.Location = new System.Drawing.Point(12, 42);
            this.lblDataview.Name = "lblDataview";
            this.lblDataview.Size = new System.Drawing.Size(85, 13);
            this.lblDataview.TabIndex = 15;
            this.lblDataview.Text = "Dataview:";
            this.lblDataview.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Location = new System.Drawing.Point(105, 120);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkEnabled.TabIndex = 52;
            this.chkEnabled.Text = "Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(349, 261);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 57;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(430, 261);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 58;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // ddlDataview
            // 
            this.ddlDataview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlDataview.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataview.FormattingEnabled = true;
            this.ddlDataview.Location = new System.Drawing.Point(105, 39);
            this.ddlDataview.Name = "ddlDataview";
            this.ddlDataview.Size = new System.Drawing.Size(400, 21);
            this.ddlDataview.TabIndex = 59;
            this.ddlDataview.SelectedIndexChanged += new System.EventHandler(this.ddlDataview_SelectedIndexChanged);
            // 
            // ddlTable
            // 
            this.ddlTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTable.FormattingEnabled = true;
            this.ddlTable.Location = new System.Drawing.Point(105, 66);
            this.ddlTable.Name = "ddlTable";
            this.ddlTable.Size = new System.Drawing.Size(400, 21);
            this.ddlTable.TabIndex = 61;
            this.ddlTable.SelectedIndexChanged += new System.EventHandler(this.ddlTable_SelectedIndexChanged);
            // 
            // lblTable
            // 
            this.lblTable.Location = new System.Drawing.Point(12, 69);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(85, 13);
            this.lblTable.TabIndex = 60;
            this.lblTable.Text = "Table:";
            this.lblTable.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblClass
            // 
            this.lblClass.Location = new System.Drawing.Point(15, 96);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(82, 13);
            this.lblClass.TabIndex = 62;
            this.lblClass.Text = "Class:";
            this.lblClass.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ddlClass
            // 
            this.ddlClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlClass.FormattingEnabled = true;
            this.ddlClass.Location = new System.Drawing.Point(105, 93);
            this.ddlClass.Name = "ddlClass";
            this.ddlClass.Size = new System.Drawing.Size(400, 21);
            this.ddlClass.TabIndex = 63;
            this.ddlClass.SelectedIndexChanged += new System.EventHandler(this.ddlClass_SelectedIndexChanged);
            // 
            // btnFile
            // 
            this.btnFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFile.Location = new System.Drawing.Point(376, 120);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(75, 23);
            this.btnFile.TabIndex = 64;
            this.btnFile.Text = "Add New...";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Visible = false;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // lblAssembly
            // 
            this.lblAssembly.Location = new System.Drawing.Point(12, 15);
            this.lblAssembly.Name = "lblAssembly";
            this.lblAssembly.Size = new System.Drawing.Size(90, 13);
            this.lblAssembly.TabIndex = 66;
            this.lblAssembly.Text = "Assembly:";
            this.lblAssembly.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ofdTriggerDetail
            // 
            this.ofdTriggerDetail.FileName = "openFileDialog1";
            // 
            // ddlAssembly
            // 
            this.ddlAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlAssembly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlAssembly.FormattingEnabled = true;
            this.ddlAssembly.Location = new System.Drawing.Point(105, 12);
            this.ddlAssembly.Name = "ddlAssembly";
            this.ddlAssembly.Size = new System.Drawing.Size(400, 21);
            this.ddlAssembly.TabIndex = 69;
            this.ddlAssembly.SelectedIndexChanged += new System.EventHandler(this.ddlAssembly_SelectedIndexChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(15, 166);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(82, 13);
            this.lblDescription.TabIndex = 70;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(105, 166);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(400, 89);
            this.txtDescription.TabIndex = 71;
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.Location = new System.Drawing.Point(105, 138);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.ReadOnly = true;
            this.txtTitle.Size = new System.Drawing.Size(400, 20);
            this.txtTitle.TabIndex = 73;
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(15, 141);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(82, 17);
            this.lblTitle.TabIndex = 72;
            this.lblTitle.Text = "Title:";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmDataTrigger
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(517, 296);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.ddlAssembly);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblAssembly);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.ddlClass);
            this.Controls.Add(this.lblClass);
            this.Controls.Add(this.ddlTable);
            this.Controls.Add(this.lblTable);
            this.Controls.Add(this.ddlDataview);
            this.Controls.Add(this.lblDataview);
            this.Controls.Add(this.chkEnabled);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDataTrigger";
            this.Text = "Data Trigger Detail";
            this.Load += new System.EventHandler(this.frmTableMapping_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDataview;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox ddlDataview;
        private System.Windows.Forms.ComboBox ddlTable;
        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.Label lblClass;
        private System.Windows.Forms.ComboBox ddlClass;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Label lblAssembly;
        private System.Windows.Forms.OpenFileDialog ofdTriggerDetail;
        private System.Windows.Forms.ComboBox ddlAssembly;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblTitle;
    }
}
