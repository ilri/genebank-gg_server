namespace GrinGlobal.Admin.PopupForms {
    partial class frmSearchEngineCalculationFormatHelper {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchEngineCalculationFormatHelper));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtFormat = new System.Windows.Forms.TextBox();
            this.rdoSortableDate = new System.Windows.Forms.RadioButton();
            this.rdoFreeFormFormat = new System.Windows.Forms.RadioButton();
            this.gbFormat = new System.Windows.Forms.GroupBox();
            this.gbCalculation = new System.Windows.Forms.GroupBox();
            this.lbFields = new System.Windows.Forms.ListBox();
            this.txtCalculationFreeForm = new System.Windows.Forms.TextBox();
            this.rdoCalcFreeForm = new System.Windows.Forms.RadioButton();
            this.rdoSqlStatementCalculation = new System.Windows.Forms.RadioButton();
            this.txtCalculationSql = new System.Windows.Forms.TextBox();
            this.rdoFieldCalculation = new System.Windows.Forms.RadioButton();
            this.gbFormat.SuspendLayout();
            this.gbCalculation.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(394, 367);
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
            this.btnOK.Location = new System.Drawing.Point(313, 367);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtFormat
            // 
            this.txtFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFormat.Location = new System.Drawing.Point(13, 42);
            this.txtFormat.Name = "txtFormat";
            this.txtFormat.Size = new System.Drawing.Size(438, 20);
            this.txtFormat.TabIndex = 2;
            this.txtFormat.TextChanged += new System.EventHandler(this.txtFormat_TextChanged);
            // 
            // rdoSortableDate
            // 
            this.rdoSortableDate.AutoSize = true;
            this.rdoSortableDate.Location = new System.Drawing.Point(13, 19);
            this.rdoSortableDate.Name = "rdoSortableDate";
            this.rdoSortableDate.Size = new System.Drawing.Size(90, 17);
            this.rdoSortableDate.TabIndex = 4;
            this.rdoSortableDate.TabStop = true;
            this.rdoSortableDate.Text = "Sortable Date";
            this.rdoSortableDate.UseVisualStyleBackColor = true;
            this.rdoSortableDate.CheckedChanged += new System.EventHandler(this.rdoSortableDate_CheckedChanged);
            // 
            // rdoFreeFormFormat
            // 
            this.rdoFreeFormFormat.AutoSize = true;
            this.rdoFreeFormFormat.Location = new System.Drawing.Point(134, 19);
            this.rdoFreeFormFormat.Name = "rdoFreeFormFormat";
            this.rdoFreeFormFormat.Size = new System.Drawing.Size(72, 17);
            this.rdoFreeFormFormat.TabIndex = 7;
            this.rdoFreeFormFormat.TabStop = true;
            this.rdoFreeFormFormat.Text = "Free Form";
            this.rdoFreeFormFormat.UseVisualStyleBackColor = true;
            this.rdoFreeFormFormat.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // gbFormat
            // 
            this.gbFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFormat.Controls.Add(this.rdoFreeFormFormat);
            this.gbFormat.Controls.Add(this.txtFormat);
            this.gbFormat.Controls.Add(this.rdoSortableDate);
            this.gbFormat.Location = new System.Drawing.Point(12, 12);
            this.gbFormat.Name = "gbFormat";
            this.gbFormat.Size = new System.Drawing.Size(459, 79);
            this.gbFormat.TabIndex = 8;
            this.gbFormat.TabStop = false;
            this.gbFormat.Text = "Format:";
            // 
            // gbCalculation
            // 
            this.gbCalculation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCalculation.Controls.Add(this.lbFields);
            this.gbCalculation.Controls.Add(this.txtCalculationFreeForm);
            this.gbCalculation.Controls.Add(this.rdoCalcFreeForm);
            this.gbCalculation.Controls.Add(this.rdoSqlStatementCalculation);
            this.gbCalculation.Controls.Add(this.txtCalculationSql);
            this.gbCalculation.Controls.Add(this.rdoFieldCalculation);
            this.gbCalculation.Location = new System.Drawing.Point(12, 97);
            this.gbCalculation.Name = "gbCalculation";
            this.gbCalculation.Size = new System.Drawing.Size(459, 264);
            this.gbCalculation.TabIndex = 9;
            this.gbCalculation.TabStop = false;
            this.gbCalculation.Text = "Calculation:";
            // 
            // lbFields
            // 
            this.lbFields.FormattingEnabled = true;
            this.lbFields.Location = new System.Drawing.Point(13, 52);
            this.lbFields.Name = "lbFields";
            this.lbFields.Size = new System.Drawing.Size(438, 173);
            this.lbFields.TabIndex = 10;
            this.lbFields.SelectedIndexChanged += new System.EventHandler(this.lbFields_SelectedIndexChanged);
            // 
            // txtCalculationFreeForm
            // 
            this.txtCalculationFreeForm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCalculationFreeForm.Location = new System.Drawing.Point(13, 238);
            this.txtCalculationFreeForm.Name = "txtCalculationFreeForm";
            this.txtCalculationFreeForm.Size = new System.Drawing.Size(438, 20);
            this.txtCalculationFreeForm.TabIndex = 9;
            this.txtCalculationFreeForm.TextChanged += new System.EventHandler(this.txtCalculationFreeForm_TextChanged);
            // 
            // rdoCalcFreeForm
            // 
            this.rdoCalcFreeForm.AutoSize = true;
            this.rdoCalcFreeForm.Location = new System.Drawing.Point(267, 19);
            this.rdoCalcFreeForm.Name = "rdoCalcFreeForm";
            this.rdoCalcFreeForm.Size = new System.Drawing.Size(72, 17);
            this.rdoCalcFreeForm.TabIndex = 8;
            this.rdoCalcFreeForm.TabStop = true;
            this.rdoCalcFreeForm.Text = "Free Form";
            this.rdoCalcFreeForm.UseVisualStyleBackColor = true;
            this.rdoCalcFreeForm.CheckedChanged += new System.EventHandler(this.rdoCalcFreeForm_CheckedChanged);
            // 
            // rdoSqlStatementCalculation
            // 
            this.rdoSqlStatementCalculation.AutoSize = true;
            this.rdoSqlStatementCalculation.Location = new System.Drawing.Point(134, 19);
            this.rdoSqlStatementCalculation.Name = "rdoSqlStatementCalculation";
            this.rdoSqlStatementCalculation.Size = new System.Drawing.Size(97, 17);
            this.rdoSqlStatementCalculation.TabIndex = 7;
            this.rdoSqlStatementCalculation.TabStop = true;
            this.rdoSqlStatementCalculation.Text = "SQL Statement";
            this.rdoSqlStatementCalculation.UseVisualStyleBackColor = true;
            this.rdoSqlStatementCalculation.CheckedChanged += new System.EventHandler(this.rdoSqlStatementCalculation_CheckedChanged);
            // 
            // txtCalculationSql
            // 
            this.txtCalculationSql.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCalculationSql.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCalculationSql.Location = new System.Drawing.Point(65, 52);
            this.txtCalculationSql.Multiline = true;
            this.txtCalculationSql.Name = "txtCalculationSql";
            this.txtCalculationSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCalculationSql.Size = new System.Drawing.Size(438, 206);
            this.txtCalculationSql.TabIndex = 2;
            this.txtCalculationSql.WordWrap = false;
            // 
            // rdoFieldCalculation
            // 
            this.rdoFieldCalculation.AutoSize = true;
            this.rdoFieldCalculation.Location = new System.Drawing.Point(13, 19);
            this.rdoFieldCalculation.Name = "rdoFieldCalculation";
            this.rdoFieldCalculation.Size = new System.Drawing.Size(58, 17);
            this.rdoFieldCalculation.TabIndex = 4;
            this.rdoFieldCalculation.TabStop = true;
            this.rdoFieldCalculation.Text = "Field(s)";
            this.rdoFieldCalculation.UseVisualStyleBackColor = true;
            this.rdoFieldCalculation.CheckedChanged += new System.EventHandler(this.rdoFieldCalculation_CheckedChanged);
            // 
            // frmSearchEngineCalculationFormatHelper
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(481, 402);
            this.Controls.Add(this.gbCalculation);
            this.Controls.Add(this.gbFormat);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSearchEngineCalculationFormatHelper";
            this.Text = "Format and Calculation Helper";
            this.Load += new System.EventHandler(this.frmSearchEngineCalculationFormatHelper_Load);
            this.gbFormat.ResumeLayout(false);
            this.gbFormat.PerformLayout();
            this.gbCalculation.ResumeLayout(false);
            this.gbCalculation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtFormat;
        private System.Windows.Forms.RadioButton rdoSortableDate;
        private System.Windows.Forms.RadioButton rdoFreeFormFormat;
        private System.Windows.Forms.GroupBox gbFormat;
        private System.Windows.Forms.GroupBox gbCalculation;
        private System.Windows.Forms.RadioButton rdoCalcFreeForm;
        private System.Windows.Forms.RadioButton rdoSqlStatementCalculation;
        private System.Windows.Forms.TextBox txtCalculationSql;
        private System.Windows.Forms.RadioButton rdoFieldCalculation;
        private System.Windows.Forms.TextBox txtCalculationFreeForm;
        private System.Windows.Forms.ListBox lbFields;
    }
}
