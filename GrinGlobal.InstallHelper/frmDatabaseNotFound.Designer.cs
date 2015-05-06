namespace GrinGlobal.InstallHelper {
    partial class frmDatabaseNotFound {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDatabaseNotFound));
            this.btnPrompt = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnPrompt
            // 
            this.btnPrompt.Location = new System.Drawing.Point(178, 112);
            this.btnPrompt.Name = "btnPrompt";
            this.btnPrompt.Size = new System.Drawing.Size(282, 23);
            this.btnPrompt.TabIndex = 0;
            this.btnPrompt.Text = "Prompt me for database connection information";
            this.btnPrompt.UseVisualStyleBackColor = true;
            this.btnPrompt.Click += new System.EventHandler(this.btnPrompt_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(466, 112);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Use Default";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(12, 13);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(548, 93);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.Text = "A connection to the database could not be determined automatically.\r\n\r\nPlease spe" +
                "cify one or use the default.";
            // 
            // frmDatabaseNotFound
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 147);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPrompt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDatabaseNotFound";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GRIN-Global Database Not Found";
            this.Activated += new System.EventHandler(this.frmDatabaseNotFound_Activated);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPrompt;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.Label lblMessage;
    }
}