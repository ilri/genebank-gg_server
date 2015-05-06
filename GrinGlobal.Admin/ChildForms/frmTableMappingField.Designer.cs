namespace GrinGlobal.Admin.ChildForms {
    partial class frmTableMappingField {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTableMappingField));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblField = new System.Windows.Forms.Label();
            this.gbForeignKey = new System.Windows.Forms.GroupBox();
            this.lblForeignKeyField = new System.Windows.Forms.Label();
            this.btnForeignKeyField = new System.Windows.Forms.Button();
            this.lblLookupPicker = new System.Windows.Forms.Label();
            this.ddlLookupPicker = new System.Windows.Forms.ComboBox();
            this.txtMaxLength = new System.Windows.Forms.TextBox();
            this.lblMaxOrScale = new System.Windows.Forms.Label();
            this.txtMinLength = new System.Windows.Forms.TextBox();
            this.lblMinOrPrecision = new System.Windows.Forms.Label();
            this.ddlCodeGroup = new System.Windows.Forms.ComboBox();
            this.lblCodeGroup = new System.Windows.Forms.Label();
            this.ddlGuiHint = new System.Windows.Forms.ComboBox();
            this.lblGuiHint = new System.Windows.Forms.Label();
            this.chkRequired = new System.Windows.Forms.CheckBox();
            this.txtDefaultValue = new System.Windows.Forms.TextBox();
            this.lblDefaultValue = new System.Windows.Forms.Label();
            this.ddlType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.ddlPurpose = new System.Windows.Forms.ComboBox();
            this.lblPurpose = new System.Windows.Forms.Label();
            this.chkPK = new System.Windows.Forms.CheckBox();
            this.chkForeignKey = new System.Windows.Forms.CheckBox();
            this.txtFieldName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chkReadOnly = new System.Windows.Forms.CheckBox();
            this.chkAutoIncrement = new System.Windows.Forms.CheckBox();
            this.txtScale = new System.Windows.Forms.TextBox();
            this.txtPrecision = new System.Windows.Forms.TextBox();
            this.chkDefaultIsNull = new System.Windows.Forms.CheckBox();
            this.chkUnlimitedLength = new System.Windows.Forms.CheckBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.chkAutoSave = new System.Windows.Forms.CheckBox();
            this.dgvLanguages = new System.Windows.Forms.DataGridView();
            this.colLanguage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDisplayText = new System.Windows.Forms.Label();
            this.gbForeignKey.SuspendLayout();
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
            // lblField
            // 
            this.lblField.AutoSize = true;
            this.lblField.Location = new System.Drawing.Point(41, 24);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(32, 13);
            this.lblField.TabIndex = 68;
            this.lblField.Text = "Field:";
            // 
            // gbForeignKey
            // 
            this.gbForeignKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbForeignKey.Controls.Add(this.lblForeignKeyField);
            this.gbForeignKey.Controls.Add(this.btnForeignKeyField);
            this.gbForeignKey.Controls.Add(this.lblField);
            this.gbForeignKey.Location = new System.Drawing.Point(91, 84);
            this.gbForeignKey.Name = "gbForeignKey";
            this.gbForeignKey.Size = new System.Drawing.Size(524, 57);
            this.gbForeignKey.TabIndex = 95;
            this.gbForeignKey.TabStop = false;
            this.gbForeignKey.Text = "Foreign Key Detail";
            // 
            // lblForeignKeyField
            // 
            this.lblForeignKeyField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblForeignKeyField.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblForeignKeyField.Location = new System.Drawing.Point(80, 19);
            this.lblForeignKeyField.Name = "lblForeignKeyField";
            this.lblForeignKeyField.Size = new System.Drawing.Size(402, 23);
            this.lblForeignKeyField.TabIndex = 72;
            this.lblForeignKeyField.Text = "(None)";
            this.lblForeignKeyField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblForeignKeyField.Click += new System.EventHandler(this.lblForeignKeyField_Click);
            // 
            // btnForeignKeyField
            // 
            this.btnForeignKeyField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnForeignKeyField.Location = new System.Drawing.Point(488, 19);
            this.btnForeignKeyField.Name = "btnForeignKeyField";
            this.btnForeignKeyField.Size = new System.Drawing.Size(23, 23);
            this.btnForeignKeyField.TabIndex = 3;
            this.btnForeignKeyField.Text = "...";
            this.btnForeignKeyField.UseVisualStyleBackColor = true;
            this.btnForeignKeyField.Click += new System.EventHandler(this.btnForeignKeyField_Click);
            // 
            // lblLookupPicker
            // 
            this.lblLookupPicker.Location = new System.Drawing.Point(258, 278);
            this.lblLookupPicker.Name = "lblLookupPicker";
            this.lblLookupPicker.Size = new System.Drawing.Size(220, 13);
            this.lblLookupPicker.TabIndex = 71;
            this.lblLookupPicker.Text = "Lookup Picker Source:";
            // 
            // ddlLookupPicker
            // 
            this.ddlLookupPicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlLookupPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLookupPicker.FormattingEnabled = true;
            this.ddlLookupPicker.Location = new System.Drawing.Point(378, 281);
            this.ddlLookupPicker.Name = "ddlLookupPicker";
            this.ddlLookupPicker.Size = new System.Drawing.Size(237, 21);
            this.ddlLookupPicker.TabIndex = 4;
            this.ddlLookupPicker.SelectedIndexChanged += new System.EventHandler(this.ddlForeignKeyDataview_SelectedIndexChanged);
            // 
            // txtMaxLength
            // 
            this.txtMaxLength.Location = new System.Drawing.Point(327, 197);
            this.txtMaxLength.Name = "txtMaxLength";
            this.txtMaxLength.Size = new System.Drawing.Size(58, 20);
            this.txtMaxLength.TabIndex = 9;
            this.txtMaxLength.TextChanged += new System.EventHandler(this.txtMaxOrScale_TextChanged);
            // 
            // lblMaxOrScale
            // 
            this.lblMaxOrScale.Location = new System.Drawing.Point(324, 178);
            this.lblMaxOrScale.Name = "lblMaxOrScale";
            this.lblMaxOrScale.Size = new System.Drawing.Size(74, 13);
            this.lblMaxOrScale.TabIndex = 93;
            this.lblMaxOrScale.Text = "Max Length:";
            // 
            // txtMinLength
            // 
            this.txtMinLength.Location = new System.Drawing.Point(258, 197);
            this.txtMinLength.Name = "txtMinLength";
            this.txtMinLength.Size = new System.Drawing.Size(57, 20);
            this.txtMinLength.TabIndex = 8;
            this.txtMinLength.TextChanged += new System.EventHandler(this.txtMinLength_TextChanged);
            // 
            // lblMinOrPrecision
            // 
            this.lblMinOrPrecision.Location = new System.Drawing.Point(255, 178);
            this.lblMinOrPrecision.Name = "lblMinOrPrecision";
            this.lblMinOrPrecision.Size = new System.Drawing.Size(63, 13);
            this.lblMinOrPrecision.TabIndex = 91;
            this.lblMinOrPrecision.Text = "Min Length:";
            // 
            // ddlCodeGroup
            // 
            this.ddlCodeGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlCodeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCodeGroup.FormattingEnabled = true;
            this.ddlCodeGroup.Location = new System.Drawing.Point(378, 254);
            this.ddlCodeGroup.Name = "ddlCodeGroup";
            this.ddlCodeGroup.Size = new System.Drawing.Size(237, 21);
            this.ddlCodeGroup.TabIndex = 12;
            this.ddlCodeGroup.SelectedIndexChanged += new System.EventHandler(this.ddlCodeGroup_SelectedIndexChanged);
            // 
            // lblCodeGroup
            // 
            this.lblCodeGroup.Location = new System.Drawing.Point(255, 257);
            this.lblCodeGroup.Name = "lblCodeGroup";
            this.lblCodeGroup.Size = new System.Drawing.Size(223, 13);
            this.lblCodeGroup.TabIndex = 89;
            this.lblCodeGroup.Text = "Drop Down Source:";
            this.lblCodeGroup.Click += new System.EventHandler(this.label5_Click);
            // 
            // ddlGuiHint
            // 
            this.ddlGuiHint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlGuiHint.FormattingEnabled = true;
            this.ddlGuiHint.Items.AddRange(new object[] {
            "Checkbox",
            "Date / Time Control",
            "Textbox (free-form)",
            "Textbox (integers only)",
            "Textbox (decimals only)",
            "Drop Down",
            "Lookup Picker"});
            this.ddlGuiHint.Location = new System.Drawing.Point(91, 254);
            this.ddlGuiHint.Name = "ddlGuiHint";
            this.ddlGuiHint.Size = new System.Drawing.Size(158, 21);
            this.ddlGuiHint.TabIndex = 11;
            this.ddlGuiHint.SelectedIndexChanged += new System.EventHandler(this.ddlGuiHint_SelectedIndexChanged);
            // 
            // lblGuiHint
            // 
            this.lblGuiHint.Location = new System.Drawing.Point(5, 257);
            this.lblGuiHint.Name = "lblGuiHint";
            this.lblGuiHint.Size = new System.Drawing.Size(80, 13);
            this.lblGuiHint.TabIndex = 87;
            this.lblGuiHint.Text = "User Interface:";
            this.lblGuiHint.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblGuiHint.Click += new System.EventHandler(this.label4_Click);
            // 
            // chkRequired
            // 
            this.chkRequired.AutoSize = true;
            this.chkRequired.Location = new System.Drawing.Point(91, 147);
            this.chkRequired.Name = "chkRequired";
            this.chkRequired.Size = new System.Drawing.Size(80, 17);
            this.chkRequired.TabIndex = 5;
            this.chkRequired.Text = "Is Required";
            this.chkRequired.UseVisualStyleBackColor = true;
            this.chkRequired.CheckedChanged += new System.EventHandler(this.chkNullable_CheckedChanged);
            // 
            // txtDefaultValue
            // 
            this.txtDefaultValue.Location = new System.Drawing.Point(91, 224);
            this.txtDefaultValue.Name = "txtDefaultValue";
            this.txtDefaultValue.Size = new System.Drawing.Size(158, 20);
            this.txtDefaultValue.TabIndex = 10;
            this.txtDefaultValue.TextChanged += new System.EventHandler(this.txtDefaultValue_TextChanged);
            // 
            // lblDefaultValue
            // 
            this.lblDefaultValue.Location = new System.Drawing.Point(2, 227);
            this.lblDefaultValue.Name = "lblDefaultValue";
            this.lblDefaultValue.Size = new System.Drawing.Size(83, 13);
            this.lblDefaultValue.TabIndex = 84;
            this.lblDefaultValue.Text = "Default Value:";
            this.lblDefaultValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblDefaultValue.Click += new System.EventHandler(this.label3_Click);
            // 
            // ddlType
            // 
            this.ddlType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlType.FormattingEnabled = true;
            this.ddlType.Items.AddRange(new object[] {
            "Date / Time",
            "Decimal",
            "Integer",
            "Large Integer",
            "Text"});
            this.ddlType.Location = new System.Drawing.Point(91, 197);
            this.ddlType.Name = "ddlType";
            this.ddlType.Size = new System.Drawing.Size(158, 21);
            this.ddlType.TabIndex = 7;
            this.ddlType.SelectedIndexChanged += new System.EventHandler(this.ddlType_SelectedIndexChanged);
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(51, 200);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(34, 13);
            this.lblType.TabIndex = 82;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblType.Click += new System.EventHandler(this.label2_Click);
            // 
            // ddlPurpose
            // 
            this.ddlPurpose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPurpose.FormattingEnabled = true;
            this.ddlPurpose.Items.AddRange(new object[] {
            "Auto-filled Created By",
            "Auto-filled Created Date",
            "Auto-filled Modified By",
            "Auto-filled Modified Date",
            "Auto-filled Owned By",
            "Auto-filled Owned Date",
            "Editable Data",
            "Primary Key"});
            this.ddlPurpose.Location = new System.Drawing.Point(91, 170);
            this.ddlPurpose.Name = "ddlPurpose";
            this.ddlPurpose.Size = new System.Drawing.Size(158, 21);
            this.ddlPurpose.TabIndex = 6;
            this.ddlPurpose.SelectedIndexChanged += new System.EventHandler(this.ddlPurpose_SelectedIndexChanged);
            // 
            // lblPurpose
            // 
            this.lblPurpose.Location = new System.Drawing.Point(11, 173);
            this.lblPurpose.Name = "lblPurpose";
            this.lblPurpose.Size = new System.Drawing.Size(74, 13);
            this.lblPurpose.TabIndex = 80;
            this.lblPurpose.Text = "Purpose:";
            this.lblPurpose.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPurpose.Click += new System.EventHandler(this.label1_Click);
            // 
            // chkPK
            // 
            this.chkPK.AutoSize = true;
            this.chkPK.Location = new System.Drawing.Point(91, 40);
            this.chkPK.Name = "chkPK";
            this.chkPK.Size = new System.Drawing.Size(92, 17);
            this.chkPK.TabIndex = 1;
            this.chkPK.Text = "Is Primary Key";
            this.chkPK.UseVisualStyleBackColor = true;
            this.chkPK.CheckedChanged += new System.EventHandler(this.chkPK_CheckedChanged);
            // 
            // chkForeignKey
            // 
            this.chkForeignKey.AutoSize = true;
            this.chkForeignKey.Location = new System.Drawing.Point(91, 63);
            this.chkForeignKey.Name = "chkForeignKey";
            this.chkForeignKey.Size = new System.Drawing.Size(93, 17);
            this.chkForeignKey.TabIndex = 2;
            this.chkForeignKey.Text = "Is Foreign Key";
            this.chkForeignKey.UseVisualStyleBackColor = true;
            this.chkForeignKey.CheckedChanged += new System.EventHandler(this.chkForeignKey_CheckedChanged);
            // 
            // txtFieldName
            // 
            this.txtFieldName.Location = new System.Drawing.Point(91, 14);
            this.txtFieldName.MaxLength = 50;
            this.txtFieldName.Name = "txtFieldName";
            this.txtFieldName.Size = new System.Drawing.Size(190, 20);
            this.txtFieldName.TabIndex = 0;
            this.txtFieldName.TextChanged += new System.EventHandler(this.txtFieldName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(11, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(74, 13);
            this.lblName.TabIndex = 96;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblName.Click += new System.EventHandler(this.label7_Click);
            // 
            // chkReadOnly
            // 
            this.chkReadOnly.AutoSize = true;
            this.chkReadOnly.Location = new System.Drawing.Point(194, 147);
            this.chkReadOnly.Name = "chkReadOnly";
            this.chkReadOnly.Size = new System.Drawing.Size(87, 17);
            this.chkReadOnly.TabIndex = 97;
            this.chkReadOnly.Text = "Is Read Only";
            this.chkReadOnly.UseVisualStyleBackColor = true;
            this.chkReadOnly.CheckedChanged += new System.EventHandler(this.chkReadOnly_CheckedChanged);
            // 
            // chkAutoIncrement
            // 
            this.chkAutoIncrement.AutoSize = true;
            this.chkAutoIncrement.Location = new System.Drawing.Point(194, 40);
            this.chkAutoIncrement.Name = "chkAutoIncrement";
            this.chkAutoIncrement.Size = new System.Drawing.Size(121, 17);
            this.chkAutoIncrement.TabIndex = 98;
            this.chkAutoIncrement.Text = "Is Auto Incremented";
            this.chkAutoIncrement.UseVisualStyleBackColor = true;
            this.chkAutoIncrement.CheckedChanged += new System.EventHandler(this.chkAutoIncrement_CheckedChanged);
            // 
            // txtScale
            // 
            this.txtScale.Location = new System.Drawing.Point(327, 155);
            this.txtScale.Name = "txtScale";
            this.txtScale.Size = new System.Drawing.Size(57, 20);
            this.txtScale.TabIndex = 99;
            this.txtScale.TextChanged += new System.EventHandler(this.txtScale_TextChanged);
            // 
            // txtPrecision
            // 
            this.txtPrecision.Location = new System.Drawing.Point(258, 155);
            this.txtPrecision.Name = "txtPrecision";
            this.txtPrecision.Size = new System.Drawing.Size(58, 20);
            this.txtPrecision.TabIndex = 100;
            this.txtPrecision.TextChanged += new System.EventHandler(this.txtPrecision_TextChanged);
            // 
            // chkDefaultIsNull
            // 
            this.chkDefaultIsNull.AutoSize = true;
            this.chkDefaultIsNull.Location = new System.Drawing.Point(258, 226);
            this.chkDefaultIsNull.Name = "chkDefaultIsNull";
            this.chkDefaultIsNull.Size = new System.Drawing.Size(111, 17);
            this.chkDefaultIsNull.TabIndex = 101;
            this.chkDefaultIsNull.Text = "Null Default Value";
            this.chkDefaultIsNull.UseVisualStyleBackColor = true;
            this.chkDefaultIsNull.CheckedChanged += new System.EventHandler(this.chkDefaultIsNull_CheckedChanged);
            // 
            // chkUnlimitedLength
            // 
            this.chkUnlimitedLength.AutoSize = true;
            this.chkUnlimitedLength.Location = new System.Drawing.Point(391, 199);
            this.chkUnlimitedLength.Name = "chkUnlimitedLength";
            this.chkUnlimitedLength.Size = new System.Drawing.Size(69, 17);
            this.chkUnlimitedLength.TabIndex = 102;
            this.chkUnlimitedLength.Text = "Unlimited";
            this.chkUnlimitedLength.UseVisualStyleBackColor = true;
            this.chkUnlimitedLength.CheckedChanged += new System.EventHandler(this.chkUnlimitedLength_CheckedChanged);
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
            this.dgvLanguages.Location = new System.Drawing.Point(91, 281);
            this.dgvLanguages.Name = "dgvLanguages";
            this.dgvLanguages.Size = new System.Drawing.Size(524, 162);
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
            this.lblDisplayText.Location = new System.Drawing.Point(5, 284);
            this.lblDisplayText.Name = "lblDisplayText";
            this.lblDisplayText.Size = new System.Drawing.Size(80, 13);
            this.lblDisplayText.TabIndex = 107;
            this.lblDisplayText.Text = "Display Text:";
            this.lblDisplayText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmTableMappingField
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(630, 482);
            this.ControlBox = true;
            this.Controls.Add(this.lblDisplayText);
            this.Controls.Add(this.ddlLookupPicker);
            this.Controls.Add(this.dgvLanguages);
            this.Controls.Add(this.chkAutoSave);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.chkUnlimitedLength);
            this.Controls.Add(this.chkDefaultIsNull);
            this.Controls.Add(this.txtPrecision);
            this.Controls.Add(this.txtScale);
            this.Controls.Add(this.chkAutoIncrement);
            this.Controls.Add(this.chkReadOnly);
            this.Controls.Add(this.txtFieldName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.gbForeignKey);
            this.Controls.Add(this.txtMaxLength);
            this.Controls.Add(this.lblMaxOrScale);
            this.Controls.Add(this.txtMinLength);
            this.Controls.Add(this.lblMinOrPrecision);
            this.Controls.Add(this.ddlCodeGroup);
            this.Controls.Add(this.lblCodeGroup);
            this.Controls.Add(this.ddlGuiHint);
            this.Controls.Add(this.lblGuiHint);
            this.Controls.Add(this.chkRequired);
            this.Controls.Add(this.txtDefaultValue);
            this.Controls.Add(this.lblDefaultValue);
            this.Controls.Add(this.ddlType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.ddlPurpose);
            this.Controls.Add(this.lblPurpose);
            this.Controls.Add(this.chkPK);
            this.Controls.Add(this.chkForeignKey);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblLookupPicker);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTableMappingField";
            this.Text = "Table Mapping Field Detail";
            this.Load += new System.EventHandler(this.frmTableMappingField_Load);
            this.gbForeignKey.ResumeLayout(false);
            this.gbForeignKey.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLanguages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblField;
        private System.Windows.Forms.GroupBox gbForeignKey;
        private System.Windows.Forms.Button btnForeignKeyField;
        private System.Windows.Forms.Label lblLookupPicker;
        private System.Windows.Forms.ComboBox ddlLookupPicker;
        private System.Windows.Forms.TextBox txtMaxLength;
        private System.Windows.Forms.Label lblMaxOrScale;
        private System.Windows.Forms.TextBox txtMinLength;
        private System.Windows.Forms.Label lblMinOrPrecision;
        private System.Windows.Forms.ComboBox ddlCodeGroup;
        private System.Windows.Forms.Label lblCodeGroup;
        private System.Windows.Forms.ComboBox ddlGuiHint;
        private System.Windows.Forms.Label lblGuiHint;
        private System.Windows.Forms.CheckBox chkRequired;
        private System.Windows.Forms.TextBox txtDefaultValue;
        private System.Windows.Forms.Label lblDefaultValue;
        private System.Windows.Forms.ComboBox ddlType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox ddlPurpose;
        private System.Windows.Forms.Label lblPurpose;
        private System.Windows.Forms.CheckBox chkPK;
        private System.Windows.Forms.CheckBox chkForeignKey;
        private System.Windows.Forms.TextBox txtFieldName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chkReadOnly;
        private System.Windows.Forms.CheckBox chkAutoIncrement;
        private System.Windows.Forms.Label lblForeignKeyField;
        private System.Windows.Forms.TextBox txtScale;
        private System.Windows.Forms.TextBox txtPrecision;
        private System.Windows.Forms.CheckBox chkDefaultIsNull;
        private System.Windows.Forms.CheckBox chkUnlimitedLength;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.CheckBox chkAutoSave;
        private System.Windows.Forms.DataGridView dgvLanguages;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLanguage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.Label lblDisplayText;
    }
}