namespace GrinGlobal.Admin.PopupForms {
    partial class frmGeography {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGeography));
            this.lvGeographies = new System.Windows.Forms.ListView();
            this.colAdmin1 = new System.Windows.Forms.ColumnHeader();
            this.colCountry = new System.Windows.Forms.ColumnHeader();
            this.colSubcontinent = new System.Windows.Forms.ColumnHeader();
            this.colContinent = new System.Windows.Forms.ColumnHeader();
            this.btnGo = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.colCountryName = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lvGeographies
            // 
            this.lvGeographies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvGeographies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAdmin1,
            this.colCountry,
            this.colCountryName,
            this.colSubcontinent,
            this.colContinent});
            this.lvGeographies.FullRowSelect = true;
            this.lvGeographies.Location = new System.Drawing.Point(12, 43);
            this.lvGeographies.MultiSelect = false;
            this.lvGeographies.Name = "lvGeographies";
            this.lvGeographies.Size = new System.Drawing.Size(614, 180);
            this.lvGeographies.TabIndex = 0;
            this.lvGeographies.UseCompatibleStateImageBehavior = false;
            this.lvGeographies.View = System.Windows.Forms.View.Details;
            this.lvGeographies.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvGeographys_MouseDoubleClick);
            this.lvGeographies.SelectedIndexChanged += new System.EventHandler(this.lvGeographies_SelectedIndexChanged);
            // 
            // colAdmin1
            // 
            this.colAdmin1.Text = "Admin 1";
            this.colAdmin1.Width = 120;
            // 
            // colCountry
            // 
            this.colCountry.Text = "Country Code";
            this.colCountry.Width = 85;
            // 
            // colSubcontinent
            // 
            this.colSubcontinent.Text = "Subcontinent";
            this.colSubcontinent.Width = 120;
            // 
            // colContinent
            // 
            this.colContinent.Text = "Continent";
            this.colContinent.Width = 120;
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Enabled = false;
            this.btnGo.Location = new System.Drawing.Point(551, 14);
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
            this.txtSearch.Size = new System.Drawing.Size(532, 20);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
            this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(470, 229);
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
            this.btnCancel.Location = new System.Drawing.Point(551, 229);
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
            this.lblCount.Location = new System.Drawing.Point(12, 234);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(10, 13);
            this.lblCount.TabIndex = 6;
            this.lblCount.Text = "-";
            // 
            // colCountryName
            // 
            this.colCountryName.Text = "Country Name";
            this.colCountryName.Width = 150;
            // 
            // frmGeography
            // 
            this.AcceptButton = this.btnGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(638, 264);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.lvGeographies);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmGeography";
            this.Text = "Search For Geography";
            this.Load += new System.EventHandler(this.frmGeography_Load);
            this.Activated += new System.EventHandler(this.frmGeography_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvGeographies;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ColumnHeader colContinent;
        private System.Windows.Forms.ColumnHeader colSubcontinent;
        private System.Windows.Forms.ColumnHeader colCountry;
        private System.Windows.Forms.ColumnHeader colAdmin1;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.ColumnHeader colCountryName;
    }
}