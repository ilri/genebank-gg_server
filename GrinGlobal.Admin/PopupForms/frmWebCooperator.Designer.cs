namespace GrinGlobal.Admin.PopupForms {
    partial class frmWebCooperator {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWebCooperator));
            this.lvWebLogins = new System.Windows.Forms.ListView();
            this.colLastName = new System.Windows.Forms.ColumnHeader();
            this.colFirstName = new System.Windows.Forms.ColumnHeader();
            this.colOrganization = new System.Windows.Forms.ColumnHeader();
            this.btnGo = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.colUserName = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lvWebLogins
            // 
            this.lvWebLogins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvWebLogins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colUserName,
            this.colLastName,
            this.colFirstName,
            this.colOrganization});
            this.lvWebLogins.FullRowSelect = true;
            this.lvWebLogins.Location = new System.Drawing.Point(12, 43);
            this.lvWebLogins.MultiSelect = false;
            this.lvWebLogins.Name = "lvWebLogins";
            this.lvWebLogins.Size = new System.Drawing.Size(516, 190);
            this.lvWebLogins.TabIndex = 0;
            this.lvWebLogins.UseCompatibleStateImageBehavior = false;
            this.lvWebLogins.View = System.Windows.Forms.View.Details;
            this.lvWebLogins.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvCooperators_MouseDoubleClick);
            // 
            // colLastName
            // 
            this.colLastName.Text = "Last";
            this.colLastName.Width = 120;
            // 
            // colFirstName
            // 
            this.colFirstName.Text = "First";
            this.colFirstName.Width = 100;
            // 
            // colOrganization
            // 
            this.colOrganization.Text = "Organization";
            this.colOrganization.Width = 150;
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Enabled = false;
            this.btnGo.Location = new System.Drawing.Point(453, 14);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "&Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(13, 14);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(434, 20);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
            this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(372, 239);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(453, 239);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(13, 239);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(10, 13);
            this.lblCount.TabIndex = 5;
            this.lblCount.Text = "-";
            // 
            // colUserName
            // 
            this.colUserName.Text = "Web User Name";
            this.colUserName.Width = 120;
            // 
            // frmWebCooperator
            // 
            this.AcceptButton = this.btnGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(540, 274);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.lvWebLogins);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmWebCooperator";
            this.Text = "Search For Web Login";
            this.Load += new System.EventHandler(this.frmCooperator_Load);
            this.Activated += new System.EventHandler(this.frmCooperator_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvWebLogins;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ColumnHeader colLastName;
        private System.Windows.Forms.ColumnHeader colFirstName;
        private System.Windows.Forms.ColumnHeader colOrganization;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.ColumnHeader colUserName;
    }
}