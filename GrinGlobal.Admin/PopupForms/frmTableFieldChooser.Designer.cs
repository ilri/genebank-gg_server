namespace GrinGlobal.Admin.PopupForms {
    partial class frmTableFieldChooser {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTableFieldChooser));
            this.lblTable = new System.Windows.Forms.Label();
            this.ddlTable = new System.Windows.Forms.ComboBox();
            this.lblField = new System.Windows.Forms.Label();
            this.ddlField = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTable
            // 
            this.lblTable.Location = new System.Drawing.Point(12, 19);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(53, 13);
            this.lblTable.TabIndex = 0;
            this.lblTable.Text = "Table:";
            this.lblTable.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ddlTable
            // 
            this.ddlTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTable.FormattingEnabled = true;
            this.ddlTable.Location = new System.Drawing.Point(71, 16);
            this.ddlTable.Name = "ddlTable";
            this.ddlTable.Size = new System.Drawing.Size(219, 21);
            this.ddlTable.TabIndex = 1;
            this.ddlTable.SelectedIndexChanged += new System.EventHandler(this.ddlTable_SelectedIndexChanged);
            // 
            // lblField
            // 
            this.lblField.Location = new System.Drawing.Point(6, 49);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(59, 13);
            this.lblField.TabIndex = 2;
            this.lblField.Text = "Field:";
            this.lblField.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ddlField
            // 
            this.ddlField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlField.FormattingEnabled = true;
            this.ddlField.Location = new System.Drawing.Point(71, 46);
            this.ddlField.Name = "ddlField";
            this.ddlField.Size = new System.Drawing.Size(219, 21);
            this.ddlField.TabIndex = 3;
            this.ddlField.SelectedIndexChanged += new System.EventHandler(this.ddlField_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(133, 80);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(214, 80);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmTableFieldChooser
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(301, 115);
            this.ControlBox = true;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.ddlField);
            this.Controls.Add(this.lblField);
            this.Controls.Add(this.ddlTable);
            this.Controls.Add(this.lblTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTableFieldChooser";
            this.Text = "Choose Table and Field";
            this.Load += new System.EventHandler(this.frmChooseTableField_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.Label lblField;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.ComboBox ddlTable;
        public System.Windows.Forms.ComboBox ddlField;
    }
}