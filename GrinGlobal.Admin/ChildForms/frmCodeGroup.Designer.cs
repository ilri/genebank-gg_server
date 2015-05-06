namespace GrinGlobal.Admin.ChildForms {
    partial class frmCodeGroup {
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
            this.components = new System.ComponentModel.Container();
            this.dgvCodes = new System.Windows.Forms.DataGridView();
            this.colValueValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValueTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValueDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValueLastTouched = new GrinGlobal.Core.DataGridViewCalendarColumn();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblGroup = new System.Windows.Forms.Label();
            this.tc = new System.Windows.Forms.TabControl();
            this.tpValues = new System.Windows.Forms.TabPage();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.lvValues = new System.Windows.Forms.ListView();
            this.colValuesValue = new System.Windows.Forms.ColumnHeader();
            this.colValuesTitle = new System.Windows.Forms.ColumnHeader();
            this.colValuesDescription = new System.Windows.Forms.ColumnHeader();
            this.colValuesLastTouched = new System.Windows.Forms.ColumnHeader();
            this.cmValue = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.ddlLanguage = new System.Windows.Forms.ComboBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.tpReferencedBy = new System.Windows.Forms.TabPage();
            this.lvReferencedBy = new System.Windows.Forms.ListView();
            this.colReferencedByDataview = new System.Windows.Forms.ColumnHeader();
            this.colReferencedByTable = new System.Windows.Forms.ColumnHeader();
            this.colReferencedByField = new System.Windows.Forms.ColumnHeader();
            this.colReferencedByLastTouched = new System.Windows.Forms.ColumnHeader();
            this.cmReference = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiReferenceExportList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.txtName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCodes)).BeginInit();
            this.tc.SuspendLayout();
            this.tpValues.SuspendLayout();
            this.cmValue.SuspendLayout();
            this.tpReferencedBy.SuspendLayout();
            this.cmReference.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvCodes
            // 
            this.dgvCodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCodes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colValueValue,
            this.colValueTitle,
            this.colValueDescription,
            this.colValueLastTouched});
            this.dgvCodes.Location = new System.Drawing.Point(409, 73);
            this.dgvCodes.Name = "dgvCodes";
            this.dgvCodes.Size = new System.Drawing.Size(149, 87);
            this.dgvCodes.TabIndex = 0;
            this.dgvCodes.Visible = false;
            // 
            // colValueValue
            // 
            this.colValueValue.DataPropertyName = "value_member";
            this.colValueValue.Frozen = true;
            this.colValueValue.HeaderText = "Value";
            this.colValueValue.Name = "colValueValue";
            // 
            // colValueTitle
            // 
            this.colValueTitle.DataPropertyName = "title";
            this.colValueTitle.HeaderText = "Title";
            this.colValueTitle.Name = "colValueTitle";
            // 
            // colValueDescription
            // 
            this.colValueDescription.DataPropertyName = "description";
            this.colValueDescription.HeaderText = "Description";
            this.colValueDescription.Name = "colValueDescription";
            // 
            // colValueLastTouched
            // 
            this.colValueLastTouched.DataPropertyName = "last_touched";
            this.colValueLastTouched.HeaderText = "Last Touched";
            this.colValueLastTouched.Name = "colValueLastTouched";
            this.colValueLastTouched.ReadOnly = true;
            this.colValueLastTouched.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colValueLastTouched.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(509, 294);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(428, 294);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblGroup
            // 
            this.lblGroup.Location = new System.Drawing.Point(13, 13);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Size = new System.Drawing.Size(99, 17);
            this.lblGroup.TabIndex = 4;
            this.lblGroup.Text = "Group Name:";
            // 
            // tc
            // 
            this.tc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tc.Controls.Add(this.tpValues);
            this.tc.Controls.Add(this.tpReferencedBy);
            this.tc.Location = new System.Drawing.Point(12, 39);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(572, 249);
            this.tc.TabIndex = 6;
            // 
            // tpValues
            // 
            this.tpValues.Controls.Add(this.btnAddNew);
            this.tpValues.Controls.Add(this.lvValues);
            this.tpValues.Controls.Add(this.ddlLanguage);
            this.tpValues.Controls.Add(this.lblLanguage);
            this.tpValues.Controls.Add(this.dgvCodes);
            this.tpValues.Location = new System.Drawing.Point(4, 22);
            this.tpValues.Name = "tpValues";
            this.tpValues.Padding = new System.Windows.Forms.Padding(3);
            this.tpValues.Size = new System.Drawing.Size(564, 223);
            this.tpValues.TabIndex = 1;
            this.tpValues.Text = "Values";
            this.tpValues.UseVisualStyleBackColor = true;
            // 
            // btnAddNew
            // 
            this.btnAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNew.Location = new System.Drawing.Point(483, 193);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(75, 23);
            this.btnAddNew.TabIndex = 4;
            this.btnAddNew.Text = "&Add...";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // lvValues
            // 
            this.lvValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colValuesValue,
            this.colValuesTitle,
            this.colValuesDescription,
            this.colValuesLastTouched});
            this.lvValues.ContextMenuStrip = this.cmValue;
            this.lvValues.FullRowSelect = true;
            this.lvValues.Location = new System.Drawing.Point(3, 31);
            this.lvValues.Name = "lvValues";
            this.lvValues.Size = new System.Drawing.Size(558, 156);
            this.lvValues.TabIndex = 3;
            this.lvValues.UseCompatibleStateImageBehavior = false;
            this.lvValues.View = System.Windows.Forms.View.Details;
            this.lvValues.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvValues_MouseDoubleClick);
            this.lvValues.SelectedIndexChanged += new System.EventHandler(this.lvValues_SelectedIndexChanged);
            this.lvValues.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lvValues_KeyPress);
            this.lvValues.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvValues_KeyDown);
            // 
            // colValuesValue
            // 
            this.colValuesValue.Text = "Value";
            this.colValuesValue.Width = 100;
            // 
            // colValuesTitle
            // 
            this.colValuesTitle.Text = "Title";
            this.colValuesTitle.Width = 130;
            // 
            // colValuesDescription
            // 
            this.colValuesDescription.Text = "Description";
            this.colValuesDescription.Width = 160;
            // 
            // colValuesLastTouched
            // 
            this.colValuesLastTouched.Text = "Last Touched";
            this.colValuesLastTouched.Width = 150;
            // 
            // cmValue
            // 
            this.cmValue.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiNew,
            this.toolStripSeparator1,
            this.cmiDelete,
            this.toolStripSeparator18,
            this.cmiExportList,
            this.cmiRefresh,
            this.cmiProperties});
            this.cmValue.Name = "ctxMenuNodeUser";
            this.cmValue.Size = new System.Drawing.Size(140, 126);
            // 
            // cmiNew
            // 
            this.cmiNew.Name = "cmiNew";
            this.cmiNew.Size = new System.Drawing.Size(139, 22);
            this.cmiNew.Text = "&New Value...";
            this.cmiNew.Click += new System.EventHandler(this.cmiNew_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(136, 6);
            // 
            // cmiDelete
            // 
            this.cmiDelete.Name = "cmiDelete";
            this.cmiDelete.Size = new System.Drawing.Size(139, 22);
            this.cmiDelete.Text = "&Delete";
            this.cmiDelete.Click += new System.EventHandler(this.cmiDelete_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(136, 6);
            // 
            // cmiExportList
            // 
            this.cmiExportList.Name = "cmiExportList";
            this.cmiExportList.Size = new System.Drawing.Size(139, 22);
            this.cmiExportList.Text = "E&xport List";
            this.cmiExportList.Click += new System.EventHandler(this.cmiExportList_Click);
            // 
            // cmiRefresh
            // 
            this.cmiRefresh.Name = "cmiRefresh";
            this.cmiRefresh.Size = new System.Drawing.Size(139, 22);
            this.cmiRefresh.Text = "&Refresh";
            this.cmiRefresh.Click += new System.EventHandler(this.cmiRefresh_Click);
            // 
            // cmiProperties
            // 
            this.cmiProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiProperties.Name = "cmiProperties";
            this.cmiProperties.Size = new System.Drawing.Size(139, 22);
            this.cmiProperties.Text = "&Properties";
            this.cmiProperties.Click += new System.EventHandler(this.cmiProperties_Click);
            // 
            // ddlLanguage
            // 
            this.ddlLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLanguage.FormattingEnabled = true;
            this.ddlLanguage.Location = new System.Drawing.Point(113, 4);
            this.ddlLanguage.Name = "ddlLanguage";
            this.ddlLanguage.Size = new System.Drawing.Size(327, 21);
            this.ddlLanguage.TabIndex = 2;
            this.ddlLanguage.Tag = "clean=true";
            this.ddlLanguage.SelectedIndexChanged += new System.EventHandler(this.ddlLanguage_SelectedIndexChanged);
            // 
            // lblLanguage
            // 
            this.lblLanguage.Location = new System.Drawing.Point(7, 7);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(100, 15);
            this.lblLanguage.TabIndex = 1;
            this.lblLanguage.Text = "Language:";
            this.lblLanguage.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tpReferencedBy
            // 
            this.tpReferencedBy.Controls.Add(this.lvReferencedBy);
            this.tpReferencedBy.Location = new System.Drawing.Point(4, 22);
            this.tpReferencedBy.Name = "tpReferencedBy";
            this.tpReferencedBy.Padding = new System.Windows.Forms.Padding(3);
            this.tpReferencedBy.Size = new System.Drawing.Size(564, 223);
            this.tpReferencedBy.TabIndex = 0;
            this.tpReferencedBy.Text = "Referenced By";
            this.tpReferencedBy.UseVisualStyleBackColor = true;
            // 
            // lvReferencedBy
            // 
            this.lvReferencedBy.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colReferencedByDataview,
            this.colReferencedByTable,
            this.colReferencedByField,
            this.colReferencedByLastTouched});
            this.lvReferencedBy.ContextMenuStrip = this.cmReference;
            this.lvReferencedBy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvReferencedBy.FullRowSelect = true;
            this.lvReferencedBy.Location = new System.Drawing.Point(3, 3);
            this.lvReferencedBy.Name = "lvReferencedBy";
            this.lvReferencedBy.Size = new System.Drawing.Size(558, 217);
            this.lvReferencedBy.TabIndex = 0;
            this.lvReferencedBy.UseCompatibleStateImageBehavior = false;
            this.lvReferencedBy.View = System.Windows.Forms.View.Details;
            // 
            // colReferencedByDataview
            // 
            this.colReferencedByDataview.Text = "Dataview";
            this.colReferencedByDataview.Width = 130;
            // 
            // colReferencedByTable
            // 
            this.colReferencedByTable.Text = "Table";
            this.colReferencedByTable.Width = 130;
            // 
            // colReferencedByField
            // 
            this.colReferencedByField.Text = "Field";
            this.colReferencedByField.Width = 130;
            // 
            // colReferencedByLastTouched
            // 
            this.colReferencedByLastTouched.Text = "Last Touched";
            this.colReferencedByLastTouched.Width = 150;
            // 
            // cmReference
            // 
            this.cmReference.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiReferenceExportList,
            this.toolStripMenuItem4});
            this.cmReference.Name = "ctxMenuNodeUser";
            this.cmReference.Size = new System.Drawing.Size(129, 48);
            // 
            // cmiReferenceExportList
            // 
            this.cmiReferenceExportList.Name = "cmiReferenceExportList";
            this.cmiReferenceExportList.Size = new System.Drawing.Size(128, 22);
            this.cmiReferenceExportList.Text = "E&xport List";
            this.cmiReferenceExportList.Click += new System.EventHandler(this.cmiReferenceExportList_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(128, 22);
            this.toolStripMenuItem4.Text = "&Refresh";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(118, 10);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(459, 20);
            this.txtName.TabIndex = 7;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // frmCodeGroup
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(596, 329);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.tc);
            this.Controls.Add(this.lblGroup);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Name = "frmCodeGroup";
            this.Text = "Code Group";
            this.Load += new System.EventHandler(this.frmCodeGroup_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCodeGroup_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCodes)).EndInit();
            this.tc.ResumeLayout(false);
            this.tpValues.ResumeLayout(false);
            this.cmValue.ResumeLayout(false);
            this.tpReferencedBy.ResumeLayout(false);
            this.cmReference.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCodes;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblGroup;
        private System.Windows.Forms.TabControl tc;
        private System.Windows.Forms.TabPage tpReferencedBy;
        private System.Windows.Forms.TabPage tpValues;
        private System.Windows.Forms.ListView lvReferencedBy;
        private System.Windows.Forms.ColumnHeader colReferencedByDataview;
        private System.Windows.Forms.ColumnHeader colReferencedByTable;
        private System.Windows.Forms.ColumnHeader colReferencedByField;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox ddlLanguage;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.ColumnHeader colReferencedByLastTouched;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValueValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValueTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValueDescription;
        private GrinGlobal.Core.DataGridViewCalendarColumn colValueLastTouched;
        private System.Windows.Forms.ListView lvValues;
        private System.Windows.Forms.ColumnHeader colValuesValue;
        private System.Windows.Forms.ColumnHeader colValuesTitle;
        private System.Windows.Forms.ColumnHeader colValuesDescription;
        private System.Windows.Forms.ColumnHeader colValuesLastTouched;
        private System.Windows.Forms.ContextMenuStrip cmValue;
        private System.Windows.Forms.ToolStripMenuItem cmiNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem cmiExportList;
        private System.Windows.Forms.ToolStripMenuItem cmiRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiProperties;
        private System.Windows.Forms.ContextMenuStrip cmReference;
        private System.Windows.Forms.ToolStripMenuItem cmiReferenceExportList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.Button btnAddNew;
    }
}
