namespace GrinGlobal.Import {
    partial class frmSourceDataDetails {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSourceDataDetails));
            this.lblDetails = new System.Windows.Forms.Label();
            this.dgvExample = new System.Windows.Forms.DataGridView();
            this.btnOK = new System.Windows.Forms.Button();
            this.lvTables = new System.Windows.Forms.ListView();
            this.colItemsColors = new System.Windows.Forms.ColumnHeader();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExample)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDetails
            // 
            this.lblDetails.Location = new System.Drawing.Point(13, 13);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(599, 65);
            this.lblDetails.TabIndex = 0;
            this.lblDetails.Text = resources.GetString("lblDetails.Text");
            // 
            // dgvExample
            // 
            this.dgvExample.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExample.Location = new System.Drawing.Point(12, 82);
            this.dgvExample.Name = "dgvExample";
            this.dgvExample.Size = new System.Drawing.Size(600, 130);
            this.dgvExample.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(537, 297);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lvTables
            // 
            this.lvTables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colItemsColors});
            this.lvTables.Location = new System.Drawing.Point(16, 219);
            this.lvTables.Name = "lvTables";
            this.lvTables.Size = new System.Drawing.Size(296, 97);
            this.lvTables.TabIndex = 3;
            this.lvTables.UseCompatibleStateImageBehavior = false;
            this.lvTables.View = System.Windows.Forms.View.Details;
            // 
            // colItemsColors
            // 
            this.colItemsColors.Text = "Color => Database Table Legend";
            this.colItemsColors.Width = 300;
            // 
            // frmSourceDataDetails
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 332);
            this.Controls.Add(this.lvTables);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgvExample);
            this.Controls.Add(this.lblDetails);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSourceDataDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Source Data Details";
            this.Load += new System.EventHandler(this.frmSourceDataDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExample)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Button btnOK;
        public System.Windows.Forms.DataGridView dgvExample;
        private System.Windows.Forms.ListView lvTables;
        private System.Windows.Forms.ColumnHeader colItemsColors;
    }
}