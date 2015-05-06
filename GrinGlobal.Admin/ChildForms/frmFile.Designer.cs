namespace GrinGlobal.Admin.ChildForms {
    partial class frmFile {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFile));
            this.btnSave = new System.Windows.Forms.Button();
            this.lblFileName = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.txtPhysicalPath = new System.Windows.Forms.TextBox();
            this.lblPhysicalPath = new System.Windows.Forms.Label();
            this.txtVirtualPath = new System.Windows.Forms.TextBox();
            this.lblVirtualPath = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.txtFileSize = new System.Windows.Forms.TextBox();
            this.lblFileSize = new System.Windows.Forms.Label();
            this.txtFileVersion = new System.Windows.Forms.TextBox();
            this.lblFileVersion = new System.Windows.Forms.Label();
            this.ofdPhysicalPath = new System.Windows.Forms.OpenFileDialog();
            this.chkAvailable = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(181, 258);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(13, 94);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(57, 13);
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "File Name:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(262, 258);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileName.Location = new System.Drawing.Point(12, 111);
            this.txtFileName.MaxLength = 255;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(325, 20);
            this.txtFileName.TabIndex = 3;
            this.txtFileName.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
            // 
            // txtPhysicalPath
            // 
            this.txtPhysicalPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPhysicalPath.Location = new System.Drawing.Point(12, 31);
            this.txtPhysicalPath.Name = "txtPhysicalPath";
            this.txtPhysicalPath.ReadOnly = true;
            this.txtPhysicalPath.Size = new System.Drawing.Size(293, 20);
            this.txtPhysicalPath.TabIndex = 5;
            this.txtPhysicalPath.TextChanged += new System.EventHandler(this.txtPhysicalPath_TextChanged);
            // 
            // lblPhysicalPath
            // 
            this.lblPhysicalPath.AutoSize = true;
            this.lblPhysicalPath.Location = new System.Drawing.Point(13, 15);
            this.lblPhysicalPath.Name = "lblPhysicalPath";
            this.lblPhysicalPath.Size = new System.Drawing.Size(74, 13);
            this.lblPhysicalPath.TabIndex = 4;
            this.lblPhysicalPath.Text = "Physical Path:";
            // 
            // txtVirtualPath
            // 
            this.txtVirtualPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVirtualPath.Location = new System.Drawing.Point(12, 196);
            this.txtVirtualPath.MaxLength = 255;
            this.txtVirtualPath.Name = "txtVirtualPath";
            this.txtVirtualPath.Size = new System.Drawing.Size(325, 20);
            this.txtVirtualPath.TabIndex = 7;
            this.txtVirtualPath.TextChanged += new System.EventHandler(this.txtVirtualPath_TextChanged);
            // 
            // lblVirtualPath
            // 
            this.lblVirtualPath.AutoSize = true;
            this.lblVirtualPath.Location = new System.Drawing.Point(14, 179);
            this.lblVirtualPath.Name = "lblVirtualPath";
            this.lblVirtualPath.Size = new System.Drawing.Size(64, 13);
            this.lblVirtualPath.TabIndex = 6;
            this.lblVirtualPath.Text = "Virtual Path:";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDisplayName.Location = new System.Drawing.Point(12, 71);
            this.txtDisplayName.MaxLength = 255;
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(325, 20);
            this.txtDisplayName.TabIndex = 9;
            this.txtDisplayName.TextChanged += new System.EventHandler(this.txtDisplayName_TextChanged);
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.AutoSize = true;
            this.lblDisplayName.Location = new System.Drawing.Point(13, 54);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(75, 13);
            this.lblDisplayName.TabIndex = 8;
            this.lblDisplayName.Text = "Display Name:";
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(312, 29);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(26, 23);
            this.btnOpenFile.TabIndex = 10;
            this.btnOpenFile.Text = "...";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // txtFileSize
            // 
            this.txtFileSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileSize.Location = new System.Drawing.Point(12, 151);
            this.txtFileSize.Name = "txtFileSize";
            this.txtFileSize.ReadOnly = true;
            this.txtFileSize.Size = new System.Drawing.Size(155, 20);
            this.txtFileSize.TabIndex = 12;
            this.txtFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFileSize
            // 
            this.lblFileSize.AutoSize = true;
            this.lblFileSize.Location = new System.Drawing.Point(13, 134);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.Size = new System.Drawing.Size(74, 13);
            this.lblFileSize.TabIndex = 11;
            this.lblFileSize.Text = "File Size (MB):";
            // 
            // txtFileVersion
            // 
            this.txtFileVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileVersion.Location = new System.Drawing.Point(187, 151);
            this.txtFileVersion.Name = "txtFileVersion";
            this.txtFileVersion.ReadOnly = true;
            this.txtFileVersion.Size = new System.Drawing.Size(151, 20);
            this.txtFileVersion.TabIndex = 14;
            this.txtFileVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFileVersion
            // 
            this.lblFileVersion.AutoSize = true;
            this.lblFileVersion.Location = new System.Drawing.Point(184, 135);
            this.lblFileVersion.Name = "lblFileVersion";
            this.lblFileVersion.Size = new System.Drawing.Size(64, 13);
            this.lblFileVersion.TabIndex = 13;
            this.lblFileVersion.Text = "File Version:";
            // 
            // ofdPhysicalPath
            // 
            this.ofdPhysicalPath.FileName = "openFileDialog1";
            this.ofdPhysicalPath.Multiselect = true;
            this.ofdPhysicalPath.Title = "Select File";
            // 
            // chkAvailable
            // 
            this.chkAvailable.AutoSize = true;
            this.chkAvailable.Location = new System.Drawing.Point(13, 223);
            this.chkAvailable.Name = "chkAvailable";
            this.chkAvailable.Size = new System.Drawing.Size(135, 17);
            this.chkAvailable.TabIndex = 15;
            this.chkAvailable.Text = "Available for Download";
            this.chkAvailable.UseVisualStyleBackColor = true;
            this.chkAvailable.CheckedChanged += new System.EventHandler(this.chkAvailable_CheckedChanged);
            // 
            // frmFile
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(349, 293);
            this.Controls.Add(this.chkAvailable);
            this.Controls.Add(this.txtFileVersion);
            this.Controls.Add(this.lblFileVersion);
            this.Controls.Add(this.txtFileSize);
            this.Controls.Add(this.lblFileSize);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.txtDisplayName);
            this.Controls.Add(this.lblDisplayName);
            this.Controls.Add(this.txtVirtualPath);
            this.Controls.Add(this.lblVirtualPath);
            this.Controls.Add(this.txtPhysicalPath);
            this.Controls.Add(this.lblPhysicalPath);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.btnSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmFile";
            this.Text = "File";
            this.Load += new System.EventHandler(this.frmFile_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.TextBox txtPhysicalPath;
        private System.Windows.Forms.Label lblPhysicalPath;
        private System.Windows.Forms.TextBox txtVirtualPath;
        private System.Windows.Forms.Label lblVirtualPath;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TextBox txtFileSize;
        private System.Windows.Forms.Label lblFileSize;
        private System.Windows.Forms.TextBox txtFileVersion;
        private System.Windows.Forms.Label lblFileVersion;
        private System.Windows.Forms.OpenFileDialog ofdPhysicalPath;
        private System.Windows.Forms.CheckBox chkAvailable;
    }
}
