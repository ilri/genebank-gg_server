namespace GrinGlobal.Admin.PopupForms {
    partial class frmTableRelationship {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTableRelationship));
            this.lblChildTable = new System.Windows.Forms.Label();
            this.lblChildField = new System.Windows.Forms.Label();
            this.ddlChildField = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gvChild = new System.Windows.Forms.GroupBox();
            this.chkChildOnlyForeignKeys = new System.Windows.Forms.CheckBox();
            this.txtChildTableName = new System.Windows.Forms.TextBox();
            this.gbParent = new System.Windows.Forms.GroupBox();
            this.chkParentOnlyPrimaryKeys = new System.Windows.Forms.CheckBox();
            this.lblParentTable = new System.Windows.Forms.Label();
            this.lblParentField = new System.Windows.Forms.Label();
            this.ddlParentTable = new System.Windows.Forms.ComboBox();
            this.ddlParentField = new System.Windows.Forms.ComboBox();
            this.chkOwner = new System.Windows.Forms.CheckBox();
            this.gvChild.SuspendLayout();
            this.gbParent.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblChildTable
            // 
            this.lblChildTable.Location = new System.Drawing.Point(12, 22);
            this.lblChildTable.Name = "lblChildTable";
            this.lblChildTable.Size = new System.Drawing.Size(110, 13);
            this.lblChildTable.TabIndex = 0;
            this.lblChildTable.Text = "Table:";
            this.lblChildTable.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblChildField
            // 
            this.lblChildField.Location = new System.Drawing.Point(12, 49);
            this.lblChildField.Name = "lblChildField";
            this.lblChildField.Size = new System.Drawing.Size(110, 13);
            this.lblChildField.TabIndex = 2;
            this.lblChildField.Text = "Field:";
            this.lblChildField.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ddlChildField
            // 
            this.ddlChildField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlChildField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlChildField.FormattingEnabled = true;
            this.ddlChildField.Location = new System.Drawing.Point(128, 46);
            this.ddlChildField.Name = "ddlChildField";
            this.ddlChildField.Size = new System.Drawing.Size(213, 21);
            this.ddlChildField.TabIndex = 3;
            this.ddlChildField.SelectedIndexChanged += new System.EventHandler(this.ddlField_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(199, 271);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(280, 271);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gvChild
            // 
            this.gvChild.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gvChild.Controls.Add(this.chkChildOnlyForeignKeys);
            this.gvChild.Controls.Add(this.txtChildTableName);
            this.gvChild.Controls.Add(this.lblChildTable);
            this.gvChild.Controls.Add(this.lblChildField);
            this.gvChild.Controls.Add(this.ddlChildField);
            this.gvChild.Location = new System.Drawing.Point(9, 12);
            this.gvChild.Name = "gvChild";
            this.gvChild.Size = new System.Drawing.Size(347, 97);
            this.gvChild.TabIndex = 12;
            this.gvChild.TabStop = false;
            this.gvChild.Text = "Child";
            // 
            // chkChildOnlyForeignKeys
            // 
            this.chkChildOnlyForeignKeys.AutoSize = true;
            this.chkChildOnlyForeignKeys.Checked = true;
            this.chkChildOnlyForeignKeys.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChildOnlyForeignKeys.Location = new System.Drawing.Point(127, 74);
            this.chkChildOnlyForeignKeys.Name = "chkChildOnlyForeignKeys";
            this.chkChildOnlyForeignKeys.Size = new System.Drawing.Size(166, 17);
            this.chkChildOnlyForeignKeys.TabIndex = 5;
            this.chkChildOnlyForeignKeys.Text = "Show Only Foreign Key Fields";
            this.chkChildOnlyForeignKeys.UseVisualStyleBackColor = true;
            this.chkChildOnlyForeignKeys.CheckedChanged += new System.EventHandler(this.chkFromOnlyKeys_CheckedChanged);
            // 
            // txtChildTableName
            // 
            this.txtChildTableName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChildTableName.Location = new System.Drawing.Point(129, 20);
            this.txtChildTableName.Name = "txtChildTableName";
            this.txtChildTableName.ReadOnly = true;
            this.txtChildTableName.Size = new System.Drawing.Size(211, 20);
            this.txtChildTableName.TabIndex = 4;
            // 
            // gbParent
            // 
            this.gbParent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbParent.Controls.Add(this.chkParentOnlyPrimaryKeys);
            this.gbParent.Controls.Add(this.lblParentTable);
            this.gbParent.Controls.Add(this.lblParentField);
            this.gbParent.Controls.Add(this.ddlParentTable);
            this.gbParent.Controls.Add(this.ddlParentField);
            this.gbParent.Location = new System.Drawing.Point(9, 115);
            this.gbParent.Name = "gbParent";
            this.gbParent.Size = new System.Drawing.Size(347, 100);
            this.gbParent.TabIndex = 13;
            this.gbParent.TabStop = false;
            this.gbParent.Text = "Parent";
            // 
            // chkParentOnlyPrimaryKeys
            // 
            this.chkParentOnlyPrimaryKeys.AutoSize = true;
            this.chkParentOnlyPrimaryKeys.Checked = true;
            this.chkParentOnlyPrimaryKeys.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkParentOnlyPrimaryKeys.Location = new System.Drawing.Point(128, 73);
            this.chkParentOnlyPrimaryKeys.Name = "chkParentOnlyPrimaryKeys";
            this.chkParentOnlyPrimaryKeys.Size = new System.Drawing.Size(165, 17);
            this.chkParentOnlyPrimaryKeys.TabIndex = 6;
            this.chkParentOnlyPrimaryKeys.Text = "Show Only Primary Key Fields";
            this.chkParentOnlyPrimaryKeys.UseVisualStyleBackColor = true;
            this.chkParentOnlyPrimaryKeys.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lblParentTable
            // 
            this.lblParentTable.Location = new System.Drawing.Point(12, 22);
            this.lblParentTable.Name = "lblParentTable";
            this.lblParentTable.Size = new System.Drawing.Size(110, 13);
            this.lblParentTable.TabIndex = 0;
            this.lblParentTable.Text = "Table:";
            this.lblParentTable.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblParentField
            // 
            this.lblParentField.Location = new System.Drawing.Point(12, 49);
            this.lblParentField.Name = "lblParentField";
            this.lblParentField.Size = new System.Drawing.Size(110, 13);
            this.lblParentField.TabIndex = 2;
            this.lblParentField.Text = "Field:";
            this.lblParentField.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ddlParentTable
            // 
            this.ddlParentTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlParentTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlParentTable.FormattingEnabled = true;
            this.ddlParentTable.Location = new System.Drawing.Point(128, 19);
            this.ddlParentTable.Name = "ddlParentTable";
            this.ddlParentTable.Size = new System.Drawing.Size(213, 21);
            this.ddlParentTable.TabIndex = 1;
            this.ddlParentTable.SelectedIndexChanged += new System.EventHandler(this.ddlToTable_SelectedIndexChanged);
            // 
            // ddlParentField
            // 
            this.ddlParentField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlParentField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlParentField.FormattingEnabled = true;
            this.ddlParentField.Location = new System.Drawing.Point(128, 46);
            this.ddlParentField.Name = "ddlParentField";
            this.ddlParentField.Size = new System.Drawing.Size(213, 21);
            this.ddlParentField.TabIndex = 3;
            this.ddlParentField.SelectedIndexChanged += new System.EventHandler(this.ddlToField_SelectedIndexChanged);
            // 
            // chkOwner
            // 
            this.chkOwner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkOwner.Location = new System.Drawing.Point(83, 230);
            this.chkOwner.Name = "chkOwner";
            this.chkOwner.Size = new System.Drawing.Size(219, 24);
            this.chkOwner.TabIndex = 14;
            this.chkOwner.Text = "Parent and child data have same owner";
            this.chkOwner.UseVisualStyleBackColor = true;
            this.chkOwner.CheckedChanged += new System.EventHandler(this.chkOwner_CheckedChanged);
            // 
            // frmTableRelationship
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(367, 306);
            this.ControlBox = true;
            this.Controls.Add(this.chkOwner);
            this.Controls.Add(this.gbParent);
            this.Controls.Add(this.gvChild);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTableRelationship";
            this.Text = "Specify Relationship";
            this.Load += new System.EventHandler(this.frmChooseTableField_Load);
            this.gvChild.ResumeLayout(false);
            this.gvChild.PerformLayout();
            this.gbParent.ResumeLayout(false);
            this.gbParent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblChildTable;
        private System.Windows.Forms.Label lblChildField;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.ComboBox ddlChildField;
        private System.Windows.Forms.GroupBox gvChild;
        private System.Windows.Forms.GroupBox gbParent;
        private System.Windows.Forms.Label lblParentTable;
        private System.Windows.Forms.Label lblParentField;
        public System.Windows.Forms.ComboBox ddlParentTable;
        public System.Windows.Forms.ComboBox ddlParentField;
        private System.Windows.Forms.TextBox txtChildTableName;
        private System.Windows.Forms.CheckBox chkChildOnlyForeignKeys;
        private System.Windows.Forms.CheckBox chkParentOnlyPrimaryKeys;
        private System.Windows.Forms.CheckBox chkOwner;
    }
}