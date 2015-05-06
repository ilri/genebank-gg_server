namespace GrinGlobal.Admin.PopupForms {
    partial class frmVerifyDataviews {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVerifyDataviews));
            this.btnDone = new System.Windows.Forms.Button();
            this.dgvDataviews = new System.Windows.Forms.DataGridView();
            this.colDataview = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction = new System.Windows.Forms.DataGridViewLinkColumn();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.chkUnescapeWhere = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataviews)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDone
            // 
            this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDone.Location = new System.Drawing.Point(513, 265);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 1;
            this.btnDone.Text = "&Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // dgvDataviews
            // 
            this.dgvDataviews.AllowUserToAddRows = false;
            this.dgvDataviews.AllowUserToDeleteRows = false;
            this.dgvDataviews.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDataviews.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataviews.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDataview,
            this.colStatus,
            this.colAction});
            this.dgvDataviews.Location = new System.Drawing.Point(12, 12);
            this.dgvDataviews.Name = "dgvDataviews";
            this.dgvDataviews.ReadOnly = true;
            this.dgvDataviews.Size = new System.Drawing.Size(576, 247);
            this.dgvDataviews.TabIndex = 2;
            this.dgvDataviews.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDataviews_CellClick);
            this.dgvDataviews.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDataviews_CellContentClick);
            // 
            // colDataview
            // 
            this.colDataview.HeaderText = "Dataview";
            this.colDataview.Name = "colDataview";
            this.colDataview.ReadOnly = true;
            this.colDataview.Width = 200;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 250;
            // 
            // colAction
            // 
            this.colAction.HeaderText = "Action";
            this.colAction.Name = "colAction";
            this.colAction.ReadOnly = true;
            this.colAction.Width = 60;
            // 
            // btnVerify
            // 
            this.btnVerify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVerify.Location = new System.Drawing.Point(432, 265);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(75, 23);
            this.btnVerify.TabIndex = 3;
            this.btnVerify.Text = "&Verify";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(13, 266);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export Results";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // chkUnescapeWhere
            // 
            this.chkUnescapeWhere.AutoSize = true;
            this.chkUnescapeWhere.Location = new System.Drawing.Point(104, 270);
            this.chkUnescapeWhere.Name = "chkUnescapeWhere";
            this.chkUnescapeWhere.Size = new System.Drawing.Size(210, 17);
            this.chkUnescapeWhere.TabIndex = 5;
            this.chkUnescapeWhere.Text = "Unescape \'WHERE\' clause as needed";
            this.chkUnescapeWhere.UseVisualStyleBackColor = true;
            this.chkUnescapeWhere.Visible = false;
            // 
            // frmVerifyDataviews
            // 
            this.AcceptButton = this.btnVerify;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(600, 300);
            this.ControlBox = true;
            this.Controls.Add(this.chkUnescapeWhere);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.dgvDataviews);
            this.Controls.Add(this.btnDone);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.Name = "frmVerifyDataviews";
            this.Text = "Verify Dataviews";
            this.Activated += new System.EventHandler(this.frmVerifyDataviews_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataviews)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.DataGridView dgvDataviews;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataview;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewLinkColumn colAction;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.CheckBox chkUnescapeWhere;
    }
}
