namespace GrinGlobal.Admin.PopupForms {
    partial class frmOptions {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkDataviewAutoSynchronize = new System.Windows.Forms.CheckBox();
            this.tc = new System.Windows.Forms.TabControl();
            this.tpDataviews = new System.Windows.Forms.TabPage();
            this.chkDataviewShowAllEngines = new System.Windows.Forms.CheckBox();
            this.tpWebApplication = new System.Windows.Forms.TabPage();
            this.tc.SuspendLayout();
            this.tpDataviews.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(298, 229);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(379, 229);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkDataviewAutoSynchronize
            // 
            this.chkDataviewAutoSynchronize.AutoSize = true;
            this.chkDataviewAutoSynchronize.Location = new System.Drawing.Point(16, 17);
            this.chkDataviewAutoSynchronize.Name = "chkDataviewAutoSynchronize";
            this.chkDataviewAutoSynchronize.Size = new System.Drawing.Size(161, 17);
            this.chkDataviewAutoSynchronize.TabIndex = 3;
            this.chkDataviewAutoSynchronize.Text = "Auto Synchronize By Default";
            this.chkDataviewAutoSynchronize.UseVisualStyleBackColor = true;
            // 
            // tc
            // 
            this.tc.Controls.Add(this.tpDataviews);
            this.tc.Controls.Add(this.tpWebApplication);
            this.tc.Location = new System.Drawing.Point(12, 12);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(442, 210);
            this.tc.TabIndex = 4;
            // 
            // tpDataviews
            // 
            this.tpDataviews.Controls.Add(this.chkDataviewShowAllEngines);
            this.tpDataviews.Controls.Add(this.chkDataviewAutoSynchronize);
            this.tpDataviews.Location = new System.Drawing.Point(4, 22);
            this.tpDataviews.Name = "tpDataviews";
            this.tpDataviews.Padding = new System.Windows.Forms.Padding(3);
            this.tpDataviews.Size = new System.Drawing.Size(434, 184);
            this.tpDataviews.TabIndex = 1;
            this.tpDataviews.Text = "Dataviews";
            this.tpDataviews.UseVisualStyleBackColor = true;
            // 
            // chkDataviewShowAllEngines
            // 
            this.chkDataviewShowAllEngines.AutoSize = true;
            this.chkDataviewShowAllEngines.Checked = true;
            this.chkDataviewShowAllEngines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDataviewShowAllEngines.Location = new System.Drawing.Point(16, 49);
            this.chkDataviewShowAllEngines.Name = "chkDataviewShowAllEngines";
            this.chkDataviewShowAllEngines.Size = new System.Drawing.Size(196, 17);
            this.chkDataviewShowAllEngines.TabIndex = 4;
            this.chkDataviewShowAllEngines.Text = "Show SQL for All Database Engines";
            this.chkDataviewShowAllEngines.UseVisualStyleBackColor = true;
            // 
            // tpWebApplication
            // 
            this.tpWebApplication.Location = new System.Drawing.Point(4, 22);
            this.tpWebApplication.Name = "tpWebApplication";
            this.tpWebApplication.Padding = new System.Windows.Forms.Padding(3);
            this.tpWebApplication.Size = new System.Drawing.Size(434, 184);
            this.tpWebApplication.TabIndex = 3;
            this.tpWebApplication.Text = "Web Application";
            this.tpWebApplication.UseVisualStyleBackColor = true;
            // 
            // frmOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(466, 264);
            this.Controls.Add(this.tc);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmOptions";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.tc.ResumeLayout(false);
            this.tpDataviews.ResumeLayout(false);
            this.tpDataviews.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkDataviewAutoSynchronize;
        private System.Windows.Forms.TabControl tc;
        private System.Windows.Forms.TabPage tpDataviews;
        private System.Windows.Forms.CheckBox chkDataviewShowAllEngines;
        private System.Windows.Forms.TabPage tpWebApplication;
    }
}