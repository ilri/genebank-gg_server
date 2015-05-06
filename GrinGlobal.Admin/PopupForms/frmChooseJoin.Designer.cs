namespace GrinGlobal.Admin.PopupForms {
    partial class frmChooseJoin {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChooseJoin));
            this.lvJoins = new System.Windows.Forms.ListView();
            this.colTable = new System.Windows.Forms.ColumnHeader();
            this.colCondition = new System.Windows.Forms.ColumnHeader();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblSpecify = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvJoins
            // 
            this.lvJoins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvJoins.CheckBoxes = true;
            this.lvJoins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTable,
            this.colCondition});
            this.lvJoins.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lvJoins.FullRowSelect = true;
            this.lvJoins.Location = new System.Drawing.Point(12, 36);
            this.lvJoins.MultiSelect = false;
            this.lvJoins.Name = "lvJoins";
            this.lvJoins.Size = new System.Drawing.Size(630, 187);
            this.lvJoins.TabIndex = 0;
            this.lvJoins.UseCompatibleStateImageBehavior = false;
            this.lvJoins.View = System.Windows.Forms.View.Details;
            this.lvJoins.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvJoins_MouseDoubleClick);
            this.lvJoins.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvJoins_ItemChecked);
            this.lvJoins.SelectedIndexChanged += new System.EventHandler(this.lvJoins_SelectedIndexChanged);
            // 
            // colTable
            // 
            this.colTable.Text = "Table";
            this.colTable.Width = 200;
            // 
            // colCondition
            // 
            this.colCondition.Text = "Condition";
            this.colCondition.Width = 400;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(486, 229);
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
            this.btnCancel.Location = new System.Drawing.Point(567, 229);
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
            // lblSpecify
            // 
            this.lblSpecify.AutoSize = true;
            this.lblSpecify.Location = new System.Drawing.Point(12, 13);
            this.lblSpecify.Name = "lblSpecify";
            this.lblSpecify.Size = new System.Drawing.Size(129, 13);
            this.lblSpecify.TabIndex = 7;
            this.lblSpecify.Text = "Specify join for table \'blah\'";
            // 
            // frmChooseJoin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(654, 264);
            this.Controls.Add(this.lblSpecify);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lvJoins);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmChooseJoin";
            this.Text = "Select Preferred Relationship";
            this.Load += new System.EventHandler(this.frmGeography_Load);
            this.Activated += new System.EventHandler(this.frmChooseJoin_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvJoins;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.ColumnHeader colTable;
        private System.Windows.Forms.ColumnHeader colCondition;
        private System.Windows.Forms.Label lblSpecify;
    }
}