namespace GrinGlobal.Admin.ChildForms {
    partial class frmLaunchImportWizard {
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
            this.gbImportData = new System.Windows.Forms.GroupBox();
            this.lblImportTitle = new System.Windows.Forms.Label();
            this.btnLaunchWizard = new System.Windows.Forms.Button();
            this.gbImportData.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbImportData
            // 
            this.gbImportData.Controls.Add(this.lblImportTitle);
            this.gbImportData.Controls.Add(this.btnLaunchWizard);
            this.gbImportData.Location = new System.Drawing.Point(15, 12);
            this.gbImportData.Name = "gbImportData";
            this.gbImportData.Size = new System.Drawing.Size(257, 100);
            this.gbImportData.TabIndex = 4;
            this.gbImportData.TabStop = false;
            this.gbImportData.Text = "Import Data";
            // 
            // lblImportTitle
            // 
            this.lblImportTitle.AutoSize = true;
            this.lblImportTitle.Location = new System.Drawing.Point(19, 25);
            this.lblImportTitle.Name = "lblImportTitle";
            this.lblImportTitle.Size = new System.Drawing.Size(205, 13);
            this.lblImportTitle.TabIndex = 3;
            this.lblImportTitle.Text = "Easily import and export GRIN-Global data";
            // 
            // btnLaunchWizard
            // 
            this.btnLaunchWizard.Location = new System.Drawing.Point(22, 50);
            this.btnLaunchWizard.Name = "btnLaunchWizard";
            this.btnLaunchWizard.Size = new System.Drawing.Size(204, 23);
            this.btnLaunchWizard.TabIndex = 2;
            this.btnLaunchWizard.Text = "&Launch Import Wizard";
            this.btnLaunchWizard.UseVisualStyleBackColor = true;
            this.btnLaunchWizard.Click += new System.EventHandler(this.btnLaunchWizard_Click);
            // 
            // frmLaunchImportWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.gbImportData);
            this.Name = "frmLaunchImportWizard";
            this.Text = "Launch Import Wizard";
            this.gbImportData.ResumeLayout(false);
            this.gbImportData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbImportData;
        private System.Windows.Forms.Label lblImportTitle;
        private System.Windows.Forms.Button btnLaunchWizard;
    }
}
