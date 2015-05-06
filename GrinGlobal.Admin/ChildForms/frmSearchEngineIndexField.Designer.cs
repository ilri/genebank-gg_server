namespace GrinGlobal.Admin.ChildForms {
    partial class frmSearchEngineIndexField {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchEngineIndexField));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkPrimaryKey = new System.Windows.Forms.CheckBox();
            this.chkStoredInIndex = new System.Windows.Forms.CheckBox();
            this.chkSearchable = new System.Windows.Forms.CheckBox();
            this.chkIsBoolean = new System.Windows.Forms.CheckBox();
            this.lblTrueValue = new System.Windows.Forms.Label();
            this.txtTrueValue = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtFormat = new System.Windows.Forms.TextBox();
            this.lblFormat = new System.Windows.Forms.Label();
            this.txtOrdinal = new System.Windows.Forms.TextBox();
            this.lblOrdinal = new System.Windows.Forms.Label();
            this.txtCalculation = new System.Windows.Forms.TextBox();
            this.lblCalculation = new System.Windows.Forms.Label();
            this.txtForeignKeyTable = new System.Windows.Forms.TextBox();
            this.lblForeignKeyTable = new System.Windows.Forms.Label();
            this.txtForeignKeyField = new System.Windows.Forms.TextBox();
            this.lblForeignKeyField = new System.Windows.Forms.Label();
            this.btnChooseForeignKey = new System.Windows.Forms.Button();
            this.btnCalculationEditor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(363, 348);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(282, 348);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkPrimaryKey
            // 
            this.chkPrimaryKey.AutoSize = true;
            this.chkPrimaryKey.Location = new System.Drawing.Point(135, 63);
            this.chkPrimaryKey.Name = "chkPrimaryKey";
            this.chkPrimaryKey.Size = new System.Drawing.Size(81, 17);
            this.chkPrimaryKey.TabIndex = 2;
            this.chkPrimaryKey.Text = "Primary Key";
            this.chkPrimaryKey.UseVisualStyleBackColor = true;
            this.chkPrimaryKey.CheckedChanged += new System.EventHandler(this.chkPrimaryKey_CheckedChanged);
            // 
            // chkStoredInIndex
            // 
            this.chkStoredInIndex.AutoSize = true;
            this.chkStoredInIndex.Location = new System.Drawing.Point(135, 138);
            this.chkStoredInIndex.Name = "chkStoredInIndex";
            this.chkStoredInIndex.Size = new System.Drawing.Size(98, 17);
            this.chkStoredInIndex.TabIndex = 3;
            this.chkStoredInIndex.Text = "Stored In Index";
            this.chkStoredInIndex.UseVisualStyleBackColor = true;
            this.chkStoredInIndex.CheckedChanged += new System.EventHandler(this.chkStoredInIndex_CheckedChanged);
            // 
            // chkSearchable
            // 
            this.chkSearchable.AutoSize = true;
            this.chkSearchable.Location = new System.Drawing.Point(135, 161);
            this.chkSearchable.Name = "chkSearchable";
            this.chkSearchable.Size = new System.Drawing.Size(80, 17);
            this.chkSearchable.TabIndex = 4;
            this.chkSearchable.Text = "Searchable";
            this.chkSearchable.UseVisualStyleBackColor = true;
            this.chkSearchable.CheckedChanged += new System.EventHandler(this.chkSearchable_CheckedChanged);
            // 
            // chkIsBoolean
            // 
            this.chkIsBoolean.AutoSize = true;
            this.chkIsBoolean.Location = new System.Drawing.Point(135, 236);
            this.chkIsBoolean.Name = "chkIsBoolean";
            this.chkIsBoolean.Size = new System.Drawing.Size(76, 17);
            this.chkIsBoolean.TabIndex = 5;
            this.chkIsBoolean.Text = "Is Boolean";
            this.chkIsBoolean.UseVisualStyleBackColor = true;
            this.chkIsBoolean.CheckedChanged += new System.EventHandler(this.chkIsBoolean_CheckedChanged);
            // 
            // lblTrueValue
            // 
            this.lblTrueValue.Location = new System.Drawing.Point(104, 259);
            this.lblTrueValue.Name = "lblTrueValue";
            this.lblTrueValue.Size = new System.Drawing.Size(95, 13);
            this.lblTrueValue.TabIndex = 6;
            this.lblTrueValue.Text = "True Value:";
            this.lblTrueValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtTrueValue
            // 
            this.txtTrueValue.Location = new System.Drawing.Point(202, 256);
            this.txtTrueValue.Name = "txtTrueValue";
            this.txtTrueValue.Size = new System.Drawing.Size(100, 20);
            this.txtTrueValue.TabIndex = 7;
            this.txtTrueValue.TextChanged += new System.EventHandler(this.txtTrueValue_TextChanged);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(135, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(204, 20);
            this.txtName.TabIndex = 11;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(12, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(117, 13);
            this.lblName.TabIndex = 10;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtFormat
            // 
            this.txtFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFormat.Location = new System.Drawing.Point(202, 184);
            this.txtFormat.Name = "txtFormat";
            this.txtFormat.Size = new System.Drawing.Size(199, 20);
            this.txtFormat.TabIndex = 13;
            this.txtFormat.TextChanged += new System.EventHandler(this.txtFormat_TextChanged);
            // 
            // lblFormat
            // 
            this.lblFormat.Location = new System.Drawing.Point(101, 187);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(95, 13);
            this.lblFormat.TabIndex = 12;
            this.lblFormat.Text = "Format:";
            this.lblFormat.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtOrdinal
            // 
            this.txtOrdinal.Location = new System.Drawing.Point(135, 38);
            this.txtOrdinal.Name = "txtOrdinal";
            this.txtOrdinal.ReadOnly = true;
            this.txtOrdinal.Size = new System.Drawing.Size(98, 20);
            this.txtOrdinal.TabIndex = 15;
            // 
            // lblOrdinal
            // 
            this.lblOrdinal.Location = new System.Drawing.Point(12, 41);
            this.lblOrdinal.Name = "lblOrdinal";
            this.lblOrdinal.Size = new System.Drawing.Size(117, 13);
            this.lblOrdinal.TabIndex = 14;
            this.lblOrdinal.Text = "Ordinal:";
            this.lblOrdinal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtCalculation
            // 
            this.txtCalculation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCalculation.Location = new System.Drawing.Point(202, 210);
            this.txtCalculation.Name = "txtCalculation";
            this.txtCalculation.Size = new System.Drawing.Size(199, 20);
            this.txtCalculation.TabIndex = 17;
            this.txtCalculation.TextChanged += new System.EventHandler(this.txtCalculation_TextChanged);
            // 
            // lblCalculation
            // 
            this.lblCalculation.Location = new System.Drawing.Point(104, 213);
            this.lblCalculation.Name = "lblCalculation";
            this.lblCalculation.Size = new System.Drawing.Size(92, 13);
            this.lblCalculation.TabIndex = 16;
            this.lblCalculation.Text = "Calculation:";
            this.lblCalculation.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtForeignKeyTable
            // 
            this.txtForeignKeyTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtForeignKeyTable.Location = new System.Drawing.Point(135, 86);
            this.txtForeignKeyTable.Name = "txtForeignKeyTable";
            this.txtForeignKeyTable.Size = new System.Drawing.Size(266, 20);
            this.txtForeignKeyTable.TabIndex = 19;
            this.txtForeignKeyTable.TextChanged += new System.EventHandler(this.txtForeignKeyTable_TextChanged);
            // 
            // lblForeignKeyTable
            // 
            this.lblForeignKeyTable.Location = new System.Drawing.Point(15, 89);
            this.lblForeignKeyTable.Name = "lblForeignKeyTable";
            this.lblForeignKeyTable.Size = new System.Drawing.Size(114, 13);
            this.lblForeignKeyTable.TabIndex = 18;
            this.lblForeignKeyTable.Text = "Foreign Key Table:";
            this.lblForeignKeyTable.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtForeignKeyField
            // 
            this.txtForeignKeyField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtForeignKeyField.Location = new System.Drawing.Point(135, 112);
            this.txtForeignKeyField.Name = "txtForeignKeyField";
            this.txtForeignKeyField.Size = new System.Drawing.Size(266, 20);
            this.txtForeignKeyField.TabIndex = 21;
            this.txtForeignKeyField.TextChanged += new System.EventHandler(this.txtForeignKeyField_TextChanged);
            // 
            // lblForeignKeyField
            // 
            this.lblForeignKeyField.Location = new System.Drawing.Point(15, 115);
            this.lblForeignKeyField.Name = "lblForeignKeyField";
            this.lblForeignKeyField.Size = new System.Drawing.Size(109, 13);
            this.lblForeignKeyField.TabIndex = 20;
            this.lblForeignKeyField.Text = "Foreign Key Field:";
            this.lblForeignKeyField.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnChooseForeignKey
            // 
            this.btnChooseForeignKey.Location = new System.Drawing.Point(407, 110);
            this.btnChooseForeignKey.Name = "btnChooseForeignKey";
            this.btnChooseForeignKey.Size = new System.Drawing.Size(31, 23);
            this.btnChooseForeignKey.TabIndex = 22;
            this.btnChooseForeignKey.Text = "...";
            this.btnChooseForeignKey.UseVisualStyleBackColor = true;
            this.btnChooseForeignKey.Click += new System.EventHandler(this.btnChooseForeignKey_Click);
            // 
            // btnCalculationEditor
            // 
            this.btnCalculationEditor.Location = new System.Drawing.Point(407, 208);
            this.btnCalculationEditor.Name = "btnCalculationEditor";
            this.btnCalculationEditor.Size = new System.Drawing.Size(31, 23);
            this.btnCalculationEditor.TabIndex = 23;
            this.btnCalculationEditor.Text = "...";
            this.btnCalculationEditor.UseVisualStyleBackColor = true;
            this.btnCalculationEditor.Click += new System.EventHandler(this.btnCalculationEditor_Click);
            // 
            // frmSearchEngineIndexField
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(450, 383);
            this.Controls.Add(this.btnCalculationEditor);
            this.Controls.Add(this.btnChooseForeignKey);
            this.Controls.Add(this.txtForeignKeyField);
            this.Controls.Add(this.lblForeignKeyField);
            this.Controls.Add(this.txtForeignKeyTable);
            this.Controls.Add(this.lblForeignKeyTable);
            this.Controls.Add(this.txtCalculation);
            this.Controls.Add(this.lblCalculation);
            this.Controls.Add(this.txtOrdinal);
            this.Controls.Add(this.lblOrdinal);
            this.Controls.Add(this.txtFormat);
            this.Controls.Add(this.lblFormat);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtTrueValue);
            this.Controls.Add(this.lblTrueValue);
            this.Controls.Add(this.chkIsBoolean);
            this.Controls.Add(this.chkSearchable);
            this.Controls.Add(this.chkStoredInIndex);
            this.Controls.Add(this.chkPrimaryKey);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSearchEngineIndexField";
            this.Text = "Search Engine Index Field";
            this.Load += new System.EventHandler(this.frmSearchEngineIndexField_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblTrueValue;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.Label lblOrdinal;
        private System.Windows.Forms.Label lblCalculation;
        private System.Windows.Forms.Label lblForeignKeyTable;
        private System.Windows.Forms.Label lblForeignKeyField;
        private System.Windows.Forms.Button btnChooseForeignKey;
        private System.Windows.Forms.Button btnCalculationEditor;
        public System.Windows.Forms.CheckBox chkPrimaryKey;
        public System.Windows.Forms.CheckBox chkStoredInIndex;
        public System.Windows.Forms.CheckBox chkSearchable;
        public System.Windows.Forms.CheckBox chkIsBoolean;
        public System.Windows.Forms.TextBox txtTrueValue;
        public System.Windows.Forms.TextBox txtName;
        public System.Windows.Forms.TextBox txtFormat;
        public System.Windows.Forms.TextBox txtOrdinal;
        public System.Windows.Forms.TextBox txtCalculation;
        public System.Windows.Forms.TextBox txtForeignKeyTable;
        public System.Windows.Forms.TextBox txtForeignKeyField;
    }
}
