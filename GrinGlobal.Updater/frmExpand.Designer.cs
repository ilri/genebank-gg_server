namespace GrinGlobal.Updater {
    partial class frmExpand {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExpand));
            this.rdoExpand = new System.Windows.Forms.RadioButton();
            this.rdoExplorer = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtExpandTo = new System.Windows.Forms.TextBox();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.chkLaunchSetup = new System.Windows.Forms.CheckBox();
            this.chkEmptyDirectory = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // rdoExpand
            // 
            this.rdoExpand.AutoSize = true;
            this.rdoExpand.Checked = true;
            this.rdoExpand.Location = new System.Drawing.Point(12, 27);
            this.rdoExpand.Name = "rdoExpand";
            this.rdoExpand.Size = new System.Drawing.Size(120, 17);
            this.rdoExpand.TabIndex = 0;
            this.rdoExpand.TabStop = true;
            this.rdoExpand.Text = "&Expand contents to:";
            this.rdoExpand.UseVisualStyleBackColor = true;
            this.rdoExpand.CheckedChanged += new System.EventHandler(this.rdoExpand_CheckedChanged);
            // 
            // rdoExplorer
            // 
            this.rdoExplorer.AutoSize = true;
            this.rdoExplorer.Location = new System.Drawing.Point(12, 113);
            this.rdoExplorer.Name = "rdoExplorer";
            this.rdoExplorer.Size = new System.Drawing.Size(104, 17);
            this.rdoExplorer.TabIndex = 1;
            this.rdoExplorer.Text = "&Show in Explorer";
            this.rdoExplorer.UseVisualStyleBackColor = true;
            this.rdoExplorer.CheckedChanged += new System.EventHandler(this.rdoExplorer_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(377, 134);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtExpandTo
            // 
            this.txtExpandTo.Location = new System.Drawing.Point(31, 50);
            this.txtExpandTo.Name = "txtExpandTo";
            this.txtExpandTo.Size = new System.Drawing.Size(384, 20);
            this.txtExpandTo.TabIndex = 3;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(421, 50);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(31, 23);
            this.btnSelectFolder.TabIndex = 4;
            this.btnSelectFolder.Text = "...";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // chkLaunchSetup
            // 
            this.chkLaunchSetup.AutoSize = true;
            this.chkLaunchSetup.Location = new System.Drawing.Point(255, 76);
            this.chkLaunchSetup.Name = "chkLaunchSetup";
            this.chkLaunchSetup.Size = new System.Drawing.Size(160, 17);
            this.chkLaunchSetup.TabIndex = 5;
            this.chkLaunchSetup.Text = "&Launch Setup.bat if possible";
            this.chkLaunchSetup.UseVisualStyleBackColor = true;
            this.chkLaunchSetup.Visible = false;
            // 
            // chkEmptyDirectory
            // 
            this.chkEmptyDirectory.AutoSize = true;
            this.chkEmptyDirectory.Checked = true;
            this.chkEmptyDirectory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEmptyDirectory.Location = new System.Drawing.Point(31, 76);
            this.chkEmptyDirectory.Name = "chkEmptyDirectory";
            this.chkEmptyDirectory.Size = new System.Drawing.Size(183, 17);
            this.chkEmptyDirectory.TabIndex = 6;
            this.chkEmptyDirectory.Text = "Empty directory before expanding";
            this.chkEmptyDirectory.UseVisualStyleBackColor = true;
            // 
            // frmExpand
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 169);
            this.ControlBox = false;
            this.Controls.Add(this.chkEmptyDirectory);
            this.Controls.Add(this.chkLaunchSetup);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.txtExpandTo);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.rdoExplorer);
            this.Controls.Add(this.rdoExpand);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExpand";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select File Action";
            this.Load += new System.EventHandler(this.frmExpand_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        public System.Windows.Forms.RadioButton rdoExpand;
        public System.Windows.Forms.RadioButton rdoExplorer;
        public System.Windows.Forms.TextBox txtExpandTo;
        public System.Windows.Forms.CheckBox chkLaunchSetup;
        public System.Windows.Forms.CheckBox chkEmptyDirectory;
    }
}