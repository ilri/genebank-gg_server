namespace GrinGlobal.Import {
    partial class frmChooseDelimiter {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChooseDelimiter));
            this.lv = new System.Windows.Forms.ListView();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ddlColumnSeparator = new System.Windows.Forms.ComboBox();
            this.lblColumnDelimiter = new System.Windows.Forms.Label();
            this.txtCustomColumnDelimiter = new System.Windows.Forms.TextBox();
            this.txtCustomRowDelimiter = new System.Windows.Forms.TextBox();
            this.lblRowDelimiter = new System.Windows.Forms.Label();
            this.ddlRowDelimiter = new System.Windows.Forms.ComboBox();
            this.lblSubset = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lv
            // 
            this.lv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lv.FullRowSelect = true;
            this.lv.Location = new System.Drawing.Point(1, 39);
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(662, 309);
            this.lv.TabIndex = 0;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(497, 354);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(578, 354);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ddlColumnSeparator
            // 
            this.ddlColumnSeparator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlColumnSeparator.FormattingEnabled = true;
            this.ddlColumnSeparator.Location = new System.Drawing.Point(131, 13);
            this.ddlColumnSeparator.Name = "ddlColumnSeparator";
            this.ddlColumnSeparator.Size = new System.Drawing.Size(155, 21);
            this.ddlColumnSeparator.TabIndex = 3;
            this.ddlColumnSeparator.SelectedIndexChanged += new System.EventHandler(this.ddlSeparator_SelectedIndexChanged);
            // 
            // lblColumnDelimiter
            // 
            this.lblColumnDelimiter.AutoSize = true;
            this.lblColumnDelimiter.Location = new System.Drawing.Point(12, 16);
            this.lblColumnDelimiter.Name = "lblColumnDelimiter";
            this.lblColumnDelimiter.Size = new System.Drawing.Size(88, 13);
            this.lblColumnDelimiter.TabIndex = 4;
            this.lblColumnDelimiter.Text = "Column Delimiter:";
            // 
            // txtCustomColumnDelimiter
            // 
            this.txtCustomColumnDelimiter.Location = new System.Drawing.Point(293, 13);
            this.txtCustomColumnDelimiter.MaxLength = 1;
            this.txtCustomColumnDelimiter.Name = "txtCustomColumnDelimiter";
            this.txtCustomColumnDelimiter.Size = new System.Drawing.Size(100, 20);
            this.txtCustomColumnDelimiter.TabIndex = 5;
            this.txtCustomColumnDelimiter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCustomColumnDelimiter.TextChanged += new System.EventHandler(this.txtCustomDelimiter_TextChanged);
            // 
            // txtCustomRowDelimiter
            // 
            this.txtCustomRowDelimiter.Location = new System.Drawing.Point(293, 40);
            this.txtCustomRowDelimiter.MaxLength = 1;
            this.txtCustomRowDelimiter.Name = "txtCustomRowDelimiter";
            this.txtCustomRowDelimiter.Size = new System.Drawing.Size(100, 20);
            this.txtCustomRowDelimiter.TabIndex = 8;
            this.txtCustomRowDelimiter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCustomRowDelimiter.Visible = false;
            // 
            // lblRowDelimiter
            // 
            this.lblRowDelimiter.AutoSize = true;
            this.lblRowDelimiter.Location = new System.Drawing.Point(12, 43);
            this.lblRowDelimiter.Name = "lblRowDelimiter";
            this.lblRowDelimiter.Size = new System.Drawing.Size(75, 13);
            this.lblRowDelimiter.TabIndex = 7;
            this.lblRowDelimiter.Text = "Row Delimiter:";
            this.lblRowDelimiter.Visible = false;
            // 
            // ddlRowDelimiter
            // 
            this.ddlRowDelimiter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRowDelimiter.FormattingEnabled = true;
            this.ddlRowDelimiter.Location = new System.Drawing.Point(131, 40);
            this.ddlRowDelimiter.Name = "ddlRowDelimiter";
            this.ddlRowDelimiter.Size = new System.Drawing.Size(155, 21);
            this.ddlRowDelimiter.TabIndex = 6;
            this.ddlRowDelimiter.Visible = false;
            // 
            // lblSubset
            // 
            this.lblSubset.AutoSize = true;
            this.lblSubset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubset.Location = new System.Drawing.Point(12, 359);
            this.lblSubset.Name = "lblSubset";
            this.lblSubset.Size = new System.Drawing.Size(254, 13);
            this.lblSubset.TabIndex = 9;
            this.lblSubset.Text = "* Displaying only the first 50 lines of the file";
            // 
            // frmChooseDelimiter
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(665, 389);
            this.Controls.Add(this.lblSubset);
            this.Controls.Add(this.txtCustomRowDelimiter);
            this.Controls.Add(this.lblRowDelimiter);
            this.Controls.Add(this.ddlRowDelimiter);
            this.Controls.Add(this.txtCustomColumnDelimiter);
            this.Controls.Add(this.lblColumnDelimiter);
            this.Controls.Add(this.ddlColumnSeparator);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lv);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmChooseDelimiter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Delimiter";
            this.Load += new System.EventHandler(this.frmChooseDelimiter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lv;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox ddlColumnSeparator;
        private System.Windows.Forms.Label lblColumnDelimiter;
        private System.Windows.Forms.TextBox txtCustomColumnDelimiter;
        private System.Windows.Forms.TextBox txtCustomRowDelimiter;
        private System.Windows.Forms.Label lblRowDelimiter;
        private System.Windows.Forms.ComboBox ddlRowDelimiter;
        private System.Windows.Forms.Label lblSubset;
    }
}