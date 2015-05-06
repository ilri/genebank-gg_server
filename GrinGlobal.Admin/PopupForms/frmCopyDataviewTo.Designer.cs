namespace GrinGlobal.Admin.PopupForms {
    partial class frmCopyDataviewTo {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCopyDataviewTo));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtNewDataviewName = new System.Windows.Forms.TextBox();
            this.lblNewDataviewName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(358, 82);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(277, 82);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtNewDataviewName
            // 
            this.txtNewDataviewName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNewDataviewName.Location = new System.Drawing.Point(12, 44);
            this.txtNewDataviewName.MaxLength = 100;
            this.txtNewDataviewName.Name = "txtNewDataviewName";
            this.txtNewDataviewName.Size = new System.Drawing.Size(421, 20);
            this.txtNewDataviewName.TabIndex = 2;
            this.txtNewDataviewName.TextChanged += new System.EventHandler(this.txtNewDataviewName_TextChanged);
            // 
            // lblNewDataviewName
            // 
            this.lblNewDataviewName.AutoSize = true;
            this.lblNewDataviewName.Location = new System.Drawing.Point(12, 28);
            this.lblNewDataviewName.Name = "lblNewDataviewName";
            this.lblNewDataviewName.Size = new System.Drawing.Size(122, 13);
            this.lblNewDataviewName.TabIndex = 3;
            this.lblNewDataviewName.Text = "Name for new dataview:";
            // 
            // frmCopyDataviewTo
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(445, 117);
            this.Controls.Add(this.lblNewDataviewName);
            this.Controls.Add(this.txtNewDataviewName);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCopyDataviewTo";
            this.Text = "Copy Dataview To...";
            this.Load += new System.EventHandler(this.frmCopyDataviewTo_Load);
            this.Activated += new System.EventHandler(this.frmCopyDataviewTo_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblNewDataviewName;
        public System.Windows.Forms.TextBox txtNewDataviewName;
    }
}
