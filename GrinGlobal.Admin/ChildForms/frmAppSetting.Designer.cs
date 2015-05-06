namespace GrinGlobal.Admin.ChildForms {
    partial class frmAppSetting {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAppSetting));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblValue = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.lblProvider = new System.Windows.Forms.Label();
            this.ddlProvider = new System.Windows.Forms.ComboBox();
            this.btnConnectionStringCreator = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(352, 182);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(433, 182);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(26, 29);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(482, 20);
            this.txtName.TabIndex = 2;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(23, 13);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name:";
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(23, 59);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(37, 13);
            this.lblValue.TabIndex = 5;
            this.lblValue.Text = "Value:";
            // 
            // txtValue
            // 
            this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtValue.Location = new System.Drawing.Point(26, 75);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(482, 20);
            this.txtValue.TabIndex = 4;
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // lblProvider
            // 
            this.lblProvider.AutoSize = true;
            this.lblProvider.Location = new System.Drawing.Point(23, 101);
            this.lblProvider.Name = "lblProvider";
            this.lblProvider.Size = new System.Drawing.Size(49, 13);
            this.lblProvider.TabIndex = 7;
            this.lblProvider.Text = "Provider:";
            // 
            // ddlProvider
            // 
            this.ddlProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlProvider.FormattingEnabled = true;
            this.ddlProvider.Items.AddRange(new object[] {
            "sqlserver",
            "mysql",
            "postgresql",
            "oracle"});
            this.ddlProvider.Location = new System.Drawing.Point(26, 117);
            this.ddlProvider.Name = "ddlProvider";
            this.ddlProvider.Size = new System.Drawing.Size(157, 21);
            this.ddlProvider.TabIndex = 8;
            this.ddlProvider.SelectedIndexChanged += new System.EventHandler(this.ddlProvider_SelectedIndexChanged);
            // 
            // btnConnectionStringCreator
            // 
            this.btnConnectionStringCreator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnectionStringCreator.Location = new System.Drawing.Point(329, 117);
            this.btnConnectionStringCreator.Name = "btnConnectionStringCreator";
            this.btnConnectionStringCreator.Size = new System.Drawing.Size(179, 23);
            this.btnConnectionStringCreator.TabIndex = 9;
            this.btnConnectionStringCreator.Text = "Connection String Creator...";
            this.btnConnectionStringCreator.UseVisualStyleBackColor = true;
            this.btnConnectionStringCreator.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmAppSetting
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(520, 217);
            this.Controls.Add(this.btnConnectionStringCreator);
            this.Controls.Add(this.ddlProvider);
            this.Controls.Add(this.lblProvider);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAppSetting";
            this.Text = "Setting Editor";
            this.Load += new System.EventHandler(this.frmSetting_Load);
            this.Activated += new System.EventHandler(this.frmSetting_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSetting_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSetting_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label lblProvider;
        private System.Windows.Forms.ComboBox ddlProvider;
        private System.Windows.Forms.Button btnConnectionStringCreator;
    }
}
