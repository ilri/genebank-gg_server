namespace GrinGlobal.ConfigurationWizard {
    partial class frmInstallType {
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
            this.chkServerApps = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkClientApps = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Size = new System.Drawing.Size(123, 13);
            this.lblTitle.Text = "Choose Installation Type";
            // 
            // chkServerApps
            // 
            this.chkServerApps.AutoSize = true;
            this.chkServerApps.Checked = true;
            this.chkServerApps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkServerApps.Location = new System.Drawing.Point(13, 44);
            this.chkServerApps.Name = "chkServerApps";
            this.chkServerApps.Size = new System.Drawing.Size(147, 17);
            this.chkServerApps.TabIndex = 4;
            this.chkServerApps.Text = "Install Server Applications";
            this.chkServerApps.UseVisualStyleBackColor = true;
            this.chkServerApps.CheckedChanged += new System.EventHandler(this.chkServerApps_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(46, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(369, 45);
            this.label1.TabIndex = 5;
            this.label1.Text = "This will allow you to install the server-side database engine, database, web app" +
                "lication, and search engine.";
            // 
            // chkClientApps
            // 
            this.chkClientApps.AutoSize = true;
            this.chkClientApps.Checked = true;
            this.chkClientApps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkClientApps.Location = new System.Drawing.Point(13, 120);
            this.chkClientApps.Name = "chkClientApps";
            this.chkClientApps.Size = new System.Drawing.Size(142, 17);
            this.chkClientApps.TabIndex = 6;
            this.chkClientApps.Text = "Install Client Applications";
            this.chkClientApps.UseVisualStyleBackColor = true;
            this.chkClientApps.CheckedChanged += new System.EventHandler(this.chkClientApps_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(46, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(369, 54);
            this.label2.TabIndex = 7;
            this.label2.Text = "This will allow you to install the Curator Tool, Search Tool, and client-side cac" +
                "hing database engine.";
            // 
            // frmInstallType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(492, 264);
            this.Controls.Add(this.chkClientApps);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkServerApps);
            this.Controls.Add(this.label1);
            this.Name = "frmInstallType";
            this.Load += new System.EventHandler(this.frmInstallType_Load);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.chkServerApps, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.chkClientApps, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.CheckBox chkServerApps;
        public System.Windows.Forms.CheckBox chkClientApps;
    }
}
