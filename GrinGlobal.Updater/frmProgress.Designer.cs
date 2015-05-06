namespace GrinGlobal.Updater {
    partial class frmProgress {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProgress));
            this.pbFile = new System.Windows.Forms.ProgressBar();
            this.pbOverallDownload = new System.Windows.Forms.ProgressBar();
            this.lblCurrentAction = new System.Windows.Forms.Label();
            this.lblOverallDownload = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblOverallInstall = new System.Windows.Forms.Label();
            this.pbOverallInstall = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblURL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pbFile
            // 
            this.pbFile.Location = new System.Drawing.Point(13, 55);
            this.pbFile.Name = "pbFile";
            this.pbFile.Size = new System.Drawing.Size(458, 23);
            this.pbFile.TabIndex = 0;
            // 
            // pbOverallDownload
            // 
            this.pbOverallDownload.Location = new System.Drawing.Point(13, 111);
            this.pbOverallDownload.Name = "pbOverallDownload";
            this.pbOverallDownload.Size = new System.Drawing.Size(458, 23);
            this.pbOverallDownload.TabIndex = 1;
            // 
            // lblCurrentAction
            // 
            this.lblCurrentAction.AutoSize = true;
            this.lblCurrentAction.Location = new System.Drawing.Point(13, 39);
            this.lblCurrentAction.Name = "lblCurrentAction";
            this.lblCurrentAction.Size = new System.Drawing.Size(61, 13);
            this.lblCurrentAction.TabIndex = 2;
            this.lblCurrentAction.Text = "Initializing...";
            // 
            // lblOverallDownload
            // 
            this.lblOverallDownload.AutoSize = true;
            this.lblOverallDownload.Location = new System.Drawing.Point(12, 95);
            this.lblOverallDownload.Name = "lblOverallDownload";
            this.lblOverallDownload.Size = new System.Drawing.Size(91, 13);
            this.lblOverallDownload.TabIndex = 3;
            this.lblOverallDownload.Text = "0.0% downloaded";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(12, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(458, 23);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Initializing...";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOverallInstall
            // 
            this.lblOverallInstall.AutoSize = true;
            this.lblOverallInstall.Location = new System.Drawing.Point(11, 148);
            this.lblOverallInstall.Name = "lblOverallInstall";
            this.lblOverallInstall.Size = new System.Drawing.Size(71, 13);
            this.lblOverallInstall.TabIndex = 6;
            this.lblOverallInstall.Text = "0.0% installed";
            // 
            // pbOverallInstall
            // 
            this.pbOverallInstall.Location = new System.Drawing.Point(12, 164);
            this.pbOverallInstall.Name = "pbOverallInstall";
            this.pbOverallInstall.Size = new System.Drawing.Size(458, 23);
            this.pbOverallInstall.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(203, 193);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblURL
            // 
            this.lblURL.Location = new System.Drawing.Point(16, 228);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(454, 23);
            this.lblURL.TabIndex = 8;
            this.lblURL.Text = "-";
            this.lblURL.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // frmProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 253);
            this.Controls.Add(this.lblURL);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblOverallInstall);
            this.Controls.Add(this.pbOverallInstall);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblOverallDownload);
            this.Controls.Add(this.lblCurrentAction);
            this.Controls.Add(this.pbOverallDownload);
            this.Controls.Add(this.pbFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Downloading...";
            this.Load += new System.EventHandler(this.frmProgress_Load);
            this.Activated += new System.EventHandler(this.frmProgress_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbFile;
        private System.Windows.Forms.ProgressBar pbOverallDownload;
        private System.Windows.Forms.Label lblCurrentAction;
        private System.Windows.Forms.Label lblOverallDownload;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblOverallInstall;
        private System.Windows.Forms.ProgressBar pbOverallInstall;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblURL;


    }
}