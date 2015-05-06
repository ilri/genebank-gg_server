namespace GrinGlobal.Admin.PopupForms {
    partial class frmCodeValue {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCodeValue));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.lblValue = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.chkAutoSave = new System.Windows.Forms.CheckBox();
            this.dgvLanguages = new System.Windows.Forms.DataGridView();
            this.colLanguage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDisplayText = new System.Windows.Forms.Label();
            this.lvReferencedBy = new System.Windows.Forms.ListView();
            this.colRefTable = new System.Windows.Forms.ColumnHeader();
            this.colRefField = new System.Windows.Forms.ColumnHeader();
            this.colRefCount = new System.Windows.Forms.ColumnHeader();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLanguages)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(91, 449);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(174, 449);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(34, 30);
            this.txtValue.MaxLength = 50;
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(190, 20);
            this.txtValue.TabIndex = 0;
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // lblValue
            // 
            this.lblValue.Location = new System.Drawing.Point(31, 14);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(135, 13);
            this.lblValue.TabIndex = 96;
            this.lblValue.Text = "Value:";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(398, 14);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(36, 23);
            this.btnNext.TabIndex = 103;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(356, 14);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(36, 23);
            this.btnPrevious.TabIndex = 104;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // chkAutoSave
            // 
            this.chkAutoSave.Location = new System.Drawing.Point(356, 41);
            this.chkAutoSave.Name = "chkAutoSave";
            this.chkAutoSave.Size = new System.Drawing.Size(135, 23);
            this.chkAutoSave.TabIndex = 105;
            this.chkAutoSave.Text = "Auto-save on move";
            this.chkAutoSave.UseVisualStyleBackColor = true;
            // 
            // dgvLanguages
            // 
            this.dgvLanguages.AllowUserToAddRows = false;
            this.dgvLanguages.AllowUserToDeleteRows = false;
            this.dgvLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLanguages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLanguages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLanguage,
            this.colTitle,
            this.colDescription});
            this.dgvLanguages.Location = new System.Drawing.Point(34, 81);
            this.dgvLanguages.Name = "dgvLanguages";
            this.dgvLanguages.Size = new System.Drawing.Size(497, 178);
            this.dgvLanguages.TabIndex = 106;
            this.dgvLanguages.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvLanguages_CellFormatting);
            this.dgvLanguages.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLanguages_CellEndEdit);
            this.dgvLanguages.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvLanguages_CellPainting);
            this.dgvLanguages.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvLanguages_EditingControlShowing);
            this.dgvLanguages.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.dgvLanguages_CellToolTipTextNeeded);
            // 
            // colLanguage
            // 
            this.colLanguage.DataPropertyName = "language_title";
            this.colLanguage.HeaderText = "Language";
            this.colLanguage.Name = "colLanguage";
            this.colLanguage.ReadOnly = true;
            // 
            // colTitle
            // 
            this.colTitle.DataPropertyName = "title";
            this.colTitle.HeaderText = "Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.Width = 150;
            // 
            // colDescription
            // 
            this.colDescription.DataPropertyName = "description";
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.Width = 200;
            // 
            // lblDisplayText
            // 
            this.lblDisplayText.Location = new System.Drawing.Point(31, 65);
            this.lblDisplayText.Name = "lblDisplayText";
            this.lblDisplayText.Size = new System.Drawing.Size(135, 13);
            this.lblDisplayText.TabIndex = 107;
            this.lblDisplayText.Text = "Display Text:";
            // 
            // lvReferencedBy
            // 
            this.lvReferencedBy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvReferencedBy.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colRefTable,
            this.colRefField,
            this.colRefCount});
            this.lvReferencedBy.FullRowSelect = true;
            this.lvReferencedBy.Location = new System.Drawing.Point(34, 290);
            this.lvReferencedBy.Name = "lvReferencedBy";
            this.lvReferencedBy.Size = new System.Drawing.Size(497, 153);
            this.lvReferencedBy.TabIndex = 108;
            this.lvReferencedBy.UseCompatibleStateImageBehavior = false;
            this.lvReferencedBy.View = System.Windows.Forms.View.Details;
            // 
            // colRefTable
            // 
            this.colRefTable.Text = "Table";
            this.colRefTable.Width = 130;
            // 
            // colRefField
            // 
            this.colRefField.Text = "Field";
            this.colRefField.Width = 130;
            // 
            // colRefCount
            // 
            this.colRefCount.Text = "Row Count";
            this.colRefCount.Width = 100;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(31, 274);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 109;
            this.label1.Text = "Referenced By:";
            // 
            // frmCodeValue
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(543, 482);
            this.ControlBox = true;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvReferencedBy);
            this.Controls.Add(this.lblDisplayText);
            this.Controls.Add(this.dgvLanguages);
            this.Controls.Add(this.chkAutoSave);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCodeValue";
            this.Text = "Code Value";
            this.Load += new System.EventHandler(this.frmTableMappingField_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLanguages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.CheckBox chkAutoSave;
        private System.Windows.Forms.DataGridView dgvLanguages;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLanguage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.Label lblDisplayText;
        private System.Windows.Forms.ListView lvReferencedBy;
        private System.Windows.Forms.ColumnHeader colRefTable;
        private System.Windows.Forms.ColumnHeader colRefField;
        private System.Windows.Forms.ColumnHeader colRefCount;
        private System.Windows.Forms.Label label1;
    }
}