namespace GrinGlobal.Admin.ChildForms {
    partial class frmPermissionField {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPermissionField));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.ddlCompare = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.ddlResource = new System.Windows.Forms.ComboBox();
            this.ddlResourceField = new System.Windows.Forms.ComboBox();
            this.rdoFieldValue = new System.Windows.Forms.RadioButton();
            this.rdoChildValue = new System.Windows.Forms.RadioButton();
            this.ddlParentTableField = new System.Windows.Forms.ComboBox();
            this.ddlParentTable = new System.Windows.Forms.ComboBox();
            this.ddlParentCompare = new System.Windows.Forms.ComboBox();
            this.lblParentType = new System.Windows.Forms.Label();
            this.cboValue = new System.Windows.Forms.ComboBox();
            this.cboParentValue = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(347, 250);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 51;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(266, 250);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 50;
            this.btnOK.Text = "&Save";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ddlCompare
            // 
            this.ddlCompare.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCompare.FormattingEnabled = true;
            this.ddlCompare.Items.AddRange(new object[] {
            "<",
            "<=",
            "==",
            ">",
            ">=",
            "<>",
            "like",
            "not like"});
            this.ddlCompare.Location = new System.Drawing.Point(66, 63);
            this.ddlCompare.Name = "ddlCompare";
            this.ddlCompare.Size = new System.Drawing.Size(82, 21);
            this.ddlCompare.TabIndex = 6;
            this.ddlCompare.SelectedIndexChanged += new System.EventHandler(this.ddlCompare_SelectedIndexChanged);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(151, 87);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(10, 13);
            this.lblType.TabIndex = 12;
            this.lblType.Text = "-";
            // 
            // ddlResource
            // 
            this.ddlResource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlResource.Enabled = false;
            this.ddlResource.FormattingEnabled = true;
            this.ddlResource.Location = new System.Drawing.Point(27, 36);
            this.ddlResource.Name = "ddlResource";
            this.ddlResource.Size = new System.Drawing.Size(193, 21);
            this.ddlResource.TabIndex = 53;
            this.ddlResource.SelectedIndexChanged += new System.EventHandler(this.ddlResource_SelectedIndexChanged);
            // 
            // ddlResourceField
            // 
            this.ddlResourceField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlResourceField.Enabled = false;
            this.ddlResourceField.FormattingEnabled = true;
            this.ddlResourceField.Location = new System.Drawing.Point(226, 36);
            this.ddlResourceField.Name = "ddlResourceField";
            this.ddlResourceField.Size = new System.Drawing.Size(193, 21);
            this.ddlResourceField.TabIndex = 54;
            this.ddlResourceField.SelectedIndexChanged += new System.EventHandler(this.ddlResourceField_SelectedIndexChanged);
            // 
            // rdoFieldValue
            // 
            this.rdoFieldValue.AutoSize = true;
            this.rdoFieldValue.Checked = true;
            this.rdoFieldValue.Location = new System.Drawing.Point(12, 13);
            this.rdoFieldValue.Name = "rdoFieldValue";
            this.rdoFieldValue.Size = new System.Drawing.Size(134, 17);
            this.rdoFieldValue.TabIndex = 55;
            this.rdoFieldValue.TabStop = true;
            this.rdoFieldValue.Text = "Restrict By Field Value:";
            this.rdoFieldValue.UseVisualStyleBackColor = true;
            this.rdoFieldValue.CheckedChanged += new System.EventHandler(this.rdoFieldValue_CheckedChanged);
            // 
            // rdoChildValue
            // 
            this.rdoChildValue.AutoSize = true;
            this.rdoChildValue.Location = new System.Drawing.Point(12, 129);
            this.rdoChildValue.Name = "rdoChildValue";
            this.rdoChildValue.Size = new System.Drawing.Size(175, 17);
            this.rdoChildValue.TabIndex = 56;
            this.rdoChildValue.Text = "Restrict Children By Field Value:";
            this.rdoChildValue.UseVisualStyleBackColor = true;
            this.rdoChildValue.CheckedChanged += new System.EventHandler(this.rdoResolveValue_CheckedChanged);
            // 
            // ddlParentTableField
            // 
            this.ddlParentTableField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlParentTableField.Enabled = false;
            this.ddlParentTableField.FormattingEnabled = true;
            this.ddlParentTableField.Location = new System.Drawing.Point(226, 152);
            this.ddlParentTableField.Name = "ddlParentTableField";
            this.ddlParentTableField.Size = new System.Drawing.Size(193, 21);
            this.ddlParentTableField.TabIndex = 60;
            this.ddlParentTableField.SelectedIndexChanged += new System.EventHandler(this.ddlParentTableField_SelectedIndexChanged);
            // 
            // ddlParentTable
            // 
            this.ddlParentTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlParentTable.Enabled = false;
            this.ddlParentTable.FormattingEnabled = true;
            this.ddlParentTable.Location = new System.Drawing.Point(27, 152);
            this.ddlParentTable.Name = "ddlParentTable";
            this.ddlParentTable.Size = new System.Drawing.Size(193, 21);
            this.ddlParentTable.TabIndex = 59;
            this.ddlParentTable.SelectedIndexChanged += new System.EventHandler(this.ddlParentTable_SelectedIndexChanged);
            // 
            // ddlParentCompare
            // 
            this.ddlParentCompare.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlParentCompare.FormattingEnabled = true;
            this.ddlParentCompare.Items.AddRange(new object[] {
            "<",
            "<=",
            "==",
            ">",
            ">=",
            "<>",
            "like",
            "not like"});
            this.ddlParentCompare.Location = new System.Drawing.Point(66, 179);
            this.ddlParentCompare.Name = "ddlParentCompare";
            this.ddlParentCompare.Size = new System.Drawing.Size(80, 21);
            this.ddlParentCompare.TabIndex = 57;
            this.ddlParentCompare.SelectedIndexChanged += new System.EventHandler(this.ddlParentCompare_SelectedIndexChanged);
            // 
            // lblParentType
            // 
            this.lblParentType.AutoSize = true;
            this.lblParentType.Location = new System.Drawing.Point(151, 203);
            this.lblParentType.Name = "lblParentType";
            this.lblParentType.Size = new System.Drawing.Size(10, 13);
            this.lblParentType.TabIndex = 61;
            this.lblParentType.Text = "-";
            // 
            // cboValue
            // 
            this.cboValue.FormattingEnabled = true;
            this.cboValue.Items.AddRange(new object[] {
            "Cooperator ID for Current User",
            "Language ID for Current User"});
            this.cboValue.Location = new System.Drawing.Point(154, 63);
            this.cboValue.Name = "cboValue";
            this.cboValue.Size = new System.Drawing.Size(265, 21);
            this.cboValue.TabIndex = 62;
            this.cboValue.SelectedIndexChanged += new System.EventHandler(this.cboValue_SelectedIndexChanged);
            this.cboValue.TextChanged += new System.EventHandler(this.cboValue_TextChanged);
            // 
            // cboParentValue
            // 
            this.cboParentValue.FormattingEnabled = true;
            this.cboParentValue.Items.AddRange(new object[] {
            "Cooperator ID for Current User",
            "Language ID for Current User"});
            this.cboParentValue.Location = new System.Drawing.Point(152, 179);
            this.cboParentValue.Name = "cboParentValue";
            this.cboParentValue.Size = new System.Drawing.Size(267, 21);
            this.cboParentValue.TabIndex = 63;
            this.cboParentValue.SelectedIndexChanged += new System.EventHandler(this.cboParentValue_SelectedIndexChanged);
            this.cboParentValue.TextChanged += new System.EventHandler(this.cboParentValue_TextChanged);
            // 
            // frmPermissionField
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(434, 285);
            this.Controls.Add(this.cboParentValue);
            this.Controls.Add(this.cboValue);
            this.Controls.Add(this.lblParentType);
            this.Controls.Add(this.ddlParentTableField);
            this.Controls.Add(this.ddlParentTable);
            this.Controls.Add(this.ddlParentCompare);
            this.Controls.Add(this.rdoChildValue);
            this.Controls.Add(this.rdoFieldValue);
            this.Controls.Add(this.ddlResourceField);
            this.Controls.Add(this.ddlResource);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.ddlCompare);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPermissionField";
            this.Text = "New Restriction";
            this.Load += new System.EventHandler(this.frmPermissionField_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox ddlCompare;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox ddlResource;
        private System.Windows.Forms.ComboBox ddlResourceField;
        private System.Windows.Forms.RadioButton rdoFieldValue;
        private System.Windows.Forms.RadioButton rdoChildValue;
        private System.Windows.Forms.ComboBox ddlParentTableField;
        private System.Windows.Forms.ComboBox ddlParentTable;
        private System.Windows.Forms.ComboBox ddlParentCompare;
        private System.Windows.Forms.Label lblParentType;
        private System.Windows.Forms.ComboBox cboValue;
        private System.Windows.Forms.ComboBox cboParentValue;
    }
}