namespace GrinGlobal.Search.Engine.Service {
    partial class frmInstallIndexes {
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Accession Index");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Cooperator Index");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Inventory Index");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Observation Index");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Taxonomy Index");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInstallIndexes));
            this.btnContinue = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rdoCreateLocally = new System.Windows.Forms.RadioButton();
            this.rdoDownloadFromServer = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinue.Location = new System.Drawing.Point(366, 278);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(75, 23);
            this.btnContinue.TabIndex = 5;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.CheckBoxes = true;
            this.treeView1.Location = new System.Drawing.Point(32, 126);
            this.treeView1.Name = "treeView1";
            treeNode1.Checked = true;
            treeNode1.Name = "Node2";
            treeNode1.Tag = "Accession Search";
            treeNode1.Text = "Accession Index";
            treeNode2.Checked = true;
            treeNode2.Name = "Node3";
            treeNode2.Tag = "Cooperator Search";
            treeNode2.Text = "Cooperator Index";
            treeNode3.Checked = true;
            treeNode3.Name = "Node0";
            treeNode3.Tag = "Inventory Search";
            treeNode3.Text = "Inventory Index";
            treeNode4.Checked = true;
            treeNode4.Name = "Node3";
            treeNode4.Tag = "Observation Search";
            treeNode4.Text = "Observation Index";
            treeNode5.Checked = true;
            treeNode5.Name = "Node1";
            treeNode5.Tag = "Taxonomy Search";
            treeNode5.Text = "Taxonomy Index";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            this.treeView1.Size = new System.Drawing.Size(191, 102);
            this.treeView1.TabIndex = 6;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDescription.Location = new System.Drawing.Point(229, 126);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(288, 102);
            this.lblDescription.TabIndex = 8;
            this.lblDescription.Text = "label1";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(447, 278);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Select Indexes to Download:";
            // 
            // rdoCreateLocally
            // 
            this.rdoCreateLocally.AutoSize = true;
            this.rdoCreateLocally.Location = new System.Drawing.Point(12, 243);
            this.rdoCreateLocally.Name = "rdoCreateLocally";
            this.rdoCreateLocally.Size = new System.Drawing.Size(323, 17);
            this.rdoCreateLocally.TabIndex = 11;
            this.rdoCreateLocally.Text = "Create Index Files Locally -- may take up to 3 hours to complete";
            this.rdoCreateLocally.UseVisualStyleBackColor = true;
            this.rdoCreateLocally.CheckedChanged += new System.EventHandler(this.rdoCreateLocally_CheckedChanged);
            // 
            // rdoDownloadFromServer
            // 
            this.rdoDownloadFromServer.AutoSize = true;
            this.rdoDownloadFromServer.Checked = true;
            this.rdoDownloadFromServer.Location = new System.Drawing.Point(12, 79);
            this.rdoDownloadFromServer.Name = "rdoDownloadFromServer";
            this.rdoDownloadFromServer.Size = new System.Drawing.Size(429, 17);
            this.rdoDownloadFromServer.TabIndex = 12;
            this.rdoDownloadFromServer.TabStop = true;
            this.rdoDownloadFromServer.Text = "Download Prebuilt Index Files From Server -- about 175 MB download for all index " +
                "files";
            this.rdoDownloadFromServer.UseVisualStyleBackColor = true;
            this.rdoDownloadFromServer.CheckedChanged += new System.EventHandler(this.rdoDownloadFromServer_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(15, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(507, 48);
            this.label2.TabIndex = 13;
            this.label2.Text = "The GRIN-Global Search Engine requires index files to operate properly.  \r\n\r\nThe " +
                "index files for the example data are fairly large, so only choose those indexes " +
                "you want to download.";
            // 
            // frmInstallIndexes
            // 
            this.AcceptButton = this.btnContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(534, 310);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rdoDownloadFromServer);
            this.Controls.Add(this.rdoCreateLocally);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmInstallIndexes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GRIN-Global Search Engine Index Download";
            this.Load += new System.EventHandler(this.frmInstallData_Load);
            this.Activated += new System.EventHandler(this.frmInstallIndexes_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmInstallData_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdoDownloadFromServer;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.RadioButton rdoCreateLocally;
    }
}