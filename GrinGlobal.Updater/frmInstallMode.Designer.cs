namespace GrinGlobal.Updater {
    partial class frmInstallMode {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInstallMode));
            this.rdoCustom = new System.Windows.Forms.RadioButton();
            this.rdoDefault = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTarget = new System.Windows.Forms.Label();
            this.chkPrepopulate = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rdoCustom
            // 
            this.rdoCustom.AutoSize = true;
            this.rdoCustom.Location = new System.Drawing.Point(11, 131);
            this.rdoCustom.Name = "rdoCustom";
            this.rdoCustom.Size = new System.Drawing.Size(113, 17);
            this.rdoCustom.TabIndex = 0;
            this.rdoCustom.Text = "Custom Installation";
            this.rdoCustom.UseVisualStyleBackColor = true;
            // 
            // rdoDefault
            // 
            this.rdoDefault.AutoSize = true;
            this.rdoDefault.Checked = true;
            this.rdoDefault.Location = new System.Drawing.Point(12, 12);
            this.rdoDefault.Name = "rdoDefault";
            this.rdoDefault.Size = new System.Drawing.Size(112, 17);
            this.rdoDefault.TabIndex = 1;
            this.rdoDefault.TabStop = true;
            this.rdoDefault.Text = "Default Installation";
            this.rdoDefault.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(34, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(374, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "Prompt me as little as possible.  Use all the default settings.  \r\nSoftware will " +
                "be installed to a subfolder of the following:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(34, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(374, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Prompt me to verify settings before applying them.";
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(37, 68);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(201, 13);
            this.lblTarget.TabIndex = 4;
            this.lblTarget.Text = "(Base GRIN-Global folder shows up here)";
            // 
            // chkPrepopulate
            // 
            this.chkPrepopulate.AutoSize = true;
            this.chkPrepopulate.Location = new System.Drawing.Point(37, 97);
            this.chkPrepopulate.Name = "chkPrepopulate";
            this.chkPrepopulate.Size = new System.Drawing.Size(237, 17);
            this.chkPrepopulate.TabIndex = 5;
            this.chkPrepopulate.Text = "Pre-populate with example GRIN-Global data";
            this.chkPrepopulate.UseVisualStyleBackColor = true;
            this.chkPrepopulate.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(305, 178);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(386, 178);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmInstallMode
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(473, 213);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkPrepopulate);
            this.Controls.Add(this.lblTarget);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rdoDefault);
            this.Controls.Add(this.rdoCustom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInstallMode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Install Mode";
            this.Load += new System.EventHandler(this.frmCustomInstall_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTarget;
        public System.Windows.Forms.CheckBox chkPrepopulate;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.RadioButton rdoCustom;
        public System.Windows.Forms.RadioButton rdoDefault;
    }
}