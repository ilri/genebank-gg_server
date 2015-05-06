namespace GrinGlobal.Import {
    partial class frmImport {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImport));
            this.tc = new System.Windows.Forms.TabControl();
            this.tpDataType = new System.Windows.Forms.TabPage();
            this.btnCancel1 = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.ddlOwnedBy = new System.Windows.Forms.ComboBox();
            this.lblOwnedBy = new System.Windows.Forms.Label();
            this.chkViewAllExistingData = new System.Windows.Forms.CheckBox();
            this.ddlLanguage = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDataTypeTitle = new System.Windows.Forms.Label();
            this.ddlDataType = new System.Windows.Forms.ComboBox();
            this.lblSearchEngineLag = new System.Windows.Forms.Label();
            this.tpSource = new System.Windows.Forms.TabPage();
            this.chkDisplayOnlyErrors = new System.Windows.Forms.CheckBox();
            this.lnkNextError = new System.Windows.Forms.LinkLabel();
            this.lnkRequired = new System.Windows.Forms.LinkLabel();
            this.lblRequired = new System.Windows.Forms.Label();
            this.btnCancel4 = new System.Windows.Forms.Button();
            this.btnLoadFromFile = new System.Windows.Forms.Button();
            this.dgvSource = new System.Windows.Forms.DataGridView();
            this.cmImport = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiImportCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiImportPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiImportDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPreviousDataType = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.tpPreview = new System.Windows.Forms.TabPage();
            this.btnCancel2 = new System.Windows.Forms.Button();
            this.btnPreviousSource = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.tpResults = new System.Windows.Forms.TabPage();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.dgcResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcRowIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcTableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnFixErrors = new System.Windows.Forms.Button();
            this.lblImportErrors = new System.Windows.Forms.Label();
            this.lblImportResults = new System.Windows.Forms.Label();
            this.btnImportMore = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblServerName = new System.Windows.Forms.Label();
            this.lnkWhyNoDragDrop = new System.Windows.Forms.LinkLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusRow = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusTable = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.ofdSource = new System.Windows.Forms.OpenFileDialog();
            this.lblDataType = new System.Windows.Forms.Label();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblResults = new System.Windows.Forms.Label();
            this.tlp = new System.Windows.Forms.TableLayoutPanel();
            this.lblDataviewTitle = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tc.SuspendLayout();
            this.tpDataType.SuspendLayout();
            this.tpSource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSource)).BeginInit();
            this.cmImport.SuspendLayout();
            this.tpPreview.SuspendLayout();
            this.tpResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.tlp.SuspendLayout();
            this.SuspendLayout();
            // 
            // tc
            // 
            this.tc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tc.Controls.Add(this.tpDataType);
            this.tc.Controls.Add(this.tpSource);
            this.tc.Controls.Add(this.tpPreview);
            this.tc.Controls.Add(this.tpResults);
            this.tc.Location = new System.Drawing.Point(0, 81);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(702, 372);
            this.tc.TabIndex = 0;
            this.tc.TabStop = false;
            this.tc.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tc_Selecting);
            this.tc.SelectedIndexChanged += new System.EventHandler(this.tc_SelectedIndexChanged);
            // 
            // tpDataType
            // 
            this.tpDataType.Controls.Add(this.btnCancel1);
            this.tpDataType.Controls.Add(this.btnNext);
            this.tpDataType.Controls.Add(this.ddlOwnedBy);
            this.tpDataType.Controls.Add(this.lblOwnedBy);
            this.tpDataType.Controls.Add(this.chkViewAllExistingData);
            this.tpDataType.Controls.Add(this.ddlLanguage);
            this.tpDataType.Controls.Add(this.label1);
            this.tpDataType.Controls.Add(this.lblDataTypeTitle);
            this.tpDataType.Controls.Add(this.ddlDataType);
            this.tpDataType.Controls.Add(this.lblSearchEngineLag);
            this.tpDataType.Location = new System.Drawing.Point(4, 22);
            this.tpDataType.Name = "tpDataType";
            this.tpDataType.Padding = new System.Windows.Forms.Padding(3);
            this.tpDataType.Size = new System.Drawing.Size(694, 346);
            this.tpDataType.TabIndex = 0;
            this.tpDataType.Text = "Data Type";
            this.tpDataType.UseVisualStyleBackColor = true;
            // 
            // btnCancel1
            // 
            this.btnCancel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel1.Location = new System.Drawing.Point(8, 317);
            this.btnCancel1.Name = "btnCancel1";
            this.btnCancel1.Size = new System.Drawing.Size(75, 23);
            this.btnCancel1.TabIndex = 3;
            this.btnCancel1.Text = "&Cancel";
            this.btnCancel1.UseVisualStyleBackColor = true;
            this.btnCancel1.Click += new System.EventHandler(this.btnCancel1_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(605, 317);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(81, 23);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "&Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNextSource_Click);
            // 
            // ddlOwnedBy
            // 
            this.ddlOwnedBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlOwnedBy.FormattingEnabled = true;
            this.ddlOwnedBy.Location = new System.Drawing.Point(56, 201);
            this.ddlOwnedBy.Name = "ddlOwnedBy";
            this.ddlOwnedBy.Size = new System.Drawing.Size(215, 21);
            this.ddlOwnedBy.TabIndex = 9;
            // 
            // lblOwnedBy
            // 
            this.lblOwnedBy.AutoSize = true;
            this.lblOwnedBy.Location = new System.Drawing.Point(53, 185);
            this.lblOwnedBy.Name = "lblOwnedBy";
            this.lblOwnedBy.Size = new System.Drawing.Size(202, 13);
            this.lblOwnedBy.TabIndex = 8;
            this.lblOwnedBy.Text = "All new records created will be owned by:";
            // 
            // chkViewAllExistingData
            // 
            this.chkViewAllExistingData.AutoSize = true;
            this.chkViewAllExistingData.Location = new System.Drawing.Point(56, 245);
            this.chkViewAllExistingData.Name = "chkViewAllExistingData";
            this.chkViewAllExistingData.Size = new System.Drawing.Size(111, 17);
            this.chkViewAllExistingData.TabIndex = 6;
            this.chkViewAllExistingData.Text = "View existing data";
            this.chkViewAllExistingData.UseVisualStyleBackColor = true;
            // 
            // ddlLanguage
            // 
            this.ddlLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLanguage.FormattingEnabled = true;
            this.ddlLanguage.Location = new System.Drawing.Point(56, 141);
            this.ddlLanguage.Name = "ddlLanguage";
            this.ddlLanguage.Size = new System.Drawing.Size(215, 21);
            this.ddlLanguage.TabIndex = 5;
            this.ddlLanguage.SelectedIndexChanged += new System.EventHandler(this.ddlLanguage_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "A specific language is also required:";
            // 
            // lblDataTypeTitle
            // 
            this.lblDataTypeTitle.AutoSize = true;
            this.lblDataTypeTitle.Location = new System.Drawing.Point(53, 39);
            this.lblDataTypeTitle.Name = "lblDataTypeTitle";
            this.lblDataTypeTitle.Size = new System.Drawing.Size(248, 39);
            this.lblDataTypeTitle.TabIndex = 2;
            this.lblDataTypeTitle.Text = "You can import various types of GRIN-Global data.\r\n\r\nPlease select one from the f" +
                "ollowing list to proceed:";
            // 
            // ddlDataType
            // 
            this.ddlDataType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataType.FormattingEnabled = true;
            this.ddlDataType.Location = new System.Drawing.Point(56, 81);
            this.ddlDataType.Name = "ddlDataType";
            this.ddlDataType.Size = new System.Drawing.Size(529, 21);
            this.ddlDataType.TabIndex = 0;
            this.ddlDataType.SelectedIndexChanged += new System.EventHandler(this.ddlDataType_SelectedIndexChanged);
            // 
            // lblSearchEngineLag
            // 
            this.lblSearchEngineLag.AutoSize = true;
            this.lblSearchEngineLag.Location = new System.Drawing.Point(53, 277);
            this.lblSearchEngineLag.Name = "lblSearchEngineLag";
            this.lblSearchEngineLag.Size = new System.Drawing.Size(454, 13);
            this.lblSearchEngineLag.TabIndex = 7;
            this.lblSearchEngineLag.Text = "Note:  The Search Engine will not start indexing any new data until you close the" +
                " Import Wizard";
            // 
            // tpSource
            // 
            this.tpSource.Controls.Add(this.chkDisplayOnlyErrors);
            this.tpSource.Controls.Add(this.lnkNextError);
            this.tpSource.Controls.Add(this.lnkRequired);
            this.tpSource.Controls.Add(this.lblRequired);
            this.tpSource.Controls.Add(this.btnCancel4);
            this.tpSource.Controls.Add(this.btnLoadFromFile);
            this.tpSource.Controls.Add(this.dgvSource);
            this.tpSource.Controls.Add(this.btnPreviousDataType);
            this.tpSource.Controls.Add(this.btnImport);
            this.tpSource.Location = new System.Drawing.Point(4, 22);
            this.tpSource.Name = "tpSource";
            this.tpSource.Size = new System.Drawing.Size(694, 346);
            this.tpSource.TabIndex = 2;
            this.tpSource.Text = "Source";
            this.tpSource.UseVisualStyleBackColor = true;
            // 
            // chkDisplayOnlyErrors
            // 
            this.chkDisplayOnlyErrors.AutoSize = true;
            this.chkDisplayOnlyErrors.Location = new System.Drawing.Point(197, 8);
            this.chkDisplayOnlyErrors.Name = "chkDisplayOnlyErrors";
            this.chkDisplayOnlyErrors.Size = new System.Drawing.Size(155, 17);
            this.chkDisplayOnlyErrors.TabIndex = 11;
            this.chkDisplayOnlyErrors.Text = "Display only errors in results";
            this.chkDisplayOnlyErrors.UseVisualStyleBackColor = true;
            this.chkDisplayOnlyErrors.Visible = false;
            // 
            // lnkNextError
            // 
            this.lnkNextError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkNextError.AutoSize = true;
            this.lnkNextError.LinkColor = System.Drawing.Color.Red;
            this.lnkNextError.Location = new System.Drawing.Point(89, 322);
            this.lnkNextError.Name = "lnkNextError";
            this.lnkNextError.Size = new System.Drawing.Size(91, 13);
            this.lnkNextError.TabIndex = 10;
            this.lnkNextError.TabStop = true;
            this.lnkNextError.Text = "Jump to next error";
            this.lnkNextError.Visible = false;
            this.lnkNextError.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNextError_LinkClicked);
            // 
            // lnkRequired
            // 
            this.lnkRequired.AutoSize = true;
            this.lnkRequired.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkRequired.Location = new System.Drawing.Point(102, 8);
            this.lnkRequired.Name = "lnkRequired";
            this.lnkRequired.Size = new System.Drawing.Size(39, 13);
            this.lnkRequired.TabIndex = 9;
            this.lnkRequired.TabStop = true;
            this.lnkRequired.Text = "Details";
            this.lnkRequired.Visible = false;
            this.lnkRequired.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRequired_LinkClicked);
            // 
            // lblRequired
            // 
            this.lblRequired.AutoSize = true;
            this.lblRequired.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequired.Location = new System.Drawing.Point(8, 8);
            this.lblRequired.Name = "lblRequired";
            this.lblRequired.Size = new System.Drawing.Size(88, 13);
            this.lblRequired.TabIndex = 8;
            this.lblRequired.Text = "* - Required Field";
            // 
            // btnCancel4
            // 
            this.btnCancel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel4.Location = new System.Drawing.Point(8, 317);
            this.btnCancel4.Name = "btnCancel4";
            this.btnCancel4.Size = new System.Drawing.Size(75, 23);
            this.btnCancel4.TabIndex = 6;
            this.btnCancel4.Text = "&Cancel";
            this.btnCancel4.UseVisualStyleBackColor = true;
            this.btnCancel4.Click += new System.EventHandler(this.btnCancel4_Click);
            // 
            // btnLoadFromFile
            // 
            this.btnLoadFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadFromFile.Location = new System.Drawing.Point(560, 3);
            this.btnLoadFromFile.Name = "btnLoadFromFile";
            this.btnLoadFromFile.Size = new System.Drawing.Size(125, 23);
            this.btnLoadFromFile.TabIndex = 4;
            this.btnLoadFromFile.Text = "&Load from file...";
            this.btnLoadFromFile.UseVisualStyleBackColor = true;
            this.btnLoadFromFile.Click += new System.EventHandler(this.btnLoadFromCSV_Click);
            // 
            // dgvSource
            // 
            this.dgvSource.AllowDrop = true;
            this.dgvSource.AllowUserToResizeRows = false;
            this.dgvSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSource.ContextMenuStrip = this.cmImport;
            this.dgvSource.Location = new System.Drawing.Point(3, 32);
            this.dgvSource.Name = "dgvSource";
            this.dgvSource.Size = new System.Drawing.Size(688, 279);
            this.dgvSource.TabIndex = 3;
            this.dgvSource.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSource_CellValueChanged);
            this.dgvSource.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvSource_CellBeginEdit);
            this.dgvSource.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSource_ColumnHeaderMouseClick);
            this.dgvSource.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSource_CellMouseDown);
            this.dgvSource.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvSource_DragOver);
            this.dgvSource.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSource_CellEndEdit);
            this.dgvSource.CurrentCellChanged += new System.EventHandler(this.dgvSource_CurrentCellChanged);
            this.dgvSource.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvSource_DataError);
            this.dgvSource.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSource_KeyDown);
            this.dgvSource.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSource_CellEnter);
            this.dgvSource.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvSource_KeyPress);
            this.dgvSource.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvSource_DragDrop);
            // 
            // cmImport
            // 
            this.cmImport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiImportCopy,
            this.cmiImportPaste,
            this.toolStripSeparator1,
            this.cmiImportDelete});
            this.cmImport.Name = "cmImport";
            this.cmImport.Size = new System.Drawing.Size(108, 76);
            // 
            // cmiImportCopy
            // 
            this.cmiImportCopy.Name = "cmiImportCopy";
            this.cmiImportCopy.Size = new System.Drawing.Size(107, 22);
            this.cmiImportCopy.Text = "&Copy";
            this.cmiImportCopy.Visible = false;
            this.cmiImportCopy.Click += new System.EventHandler(this.cmiImportCopy_Click);
            // 
            // cmiImportPaste
            // 
            this.cmiImportPaste.Name = "cmiImportPaste";
            this.cmiImportPaste.Size = new System.Drawing.Size(107, 22);
            this.cmiImportPaste.Text = "&Paste";
            this.cmiImportPaste.Visible = false;
            this.cmiImportPaste.Click += new System.EventHandler(this.cmiImportPaste_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(104, 6);
            // 
            // cmiImportDelete
            // 
            this.cmiImportDelete.Name = "cmiImportDelete";
            this.cmiImportDelete.Size = new System.Drawing.Size(107, 22);
            this.cmiImportDelete.Text = "&Delete";
            this.cmiImportDelete.Click += new System.EventHandler(this.cmiImportDelete_Click);
            // 
            // btnPreviousDataType
            // 
            this.btnPreviousDataType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreviousDataType.Location = new System.Drawing.Point(519, 317);
            this.btnPreviousDataType.Name = "btnPreviousDataType";
            this.btnPreviousDataType.Size = new System.Drawing.Size(80, 23);
            this.btnPreviousDataType.TabIndex = 2;
            this.btnPreviousDataType.Text = "< &Previous";
            this.btnPreviousDataType.UseVisualStyleBackColor = true;
            this.btnPreviousDataType.Click += new System.EventHandler(this.btnPreviousDataType_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Enabled = false;
            this.btnImport.Location = new System.Drawing.Point(605, 317);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 23);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "&Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // tpPreview
            // 
            this.tpPreview.Controls.Add(this.btnCancel2);
            this.tpPreview.Controls.Add(this.btnPreviousSource);
            this.tpPreview.Controls.Add(this.btnFinish);
            this.tpPreview.Location = new System.Drawing.Point(4, 22);
            this.tpPreview.Name = "tpPreview";
            this.tpPreview.Size = new System.Drawing.Size(694, 346);
            this.tpPreview.TabIndex = 3;
            this.tpPreview.Text = "Preview";
            this.tpPreview.UseVisualStyleBackColor = true;
            // 
            // btnCancel2
            // 
            this.btnCancel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel2.Location = new System.Drawing.Point(8, 339);
            this.btnCancel2.Name = "btnCancel2";
            this.btnCancel2.Size = new System.Drawing.Size(75, 23);
            this.btnCancel2.TabIndex = 4;
            this.btnCancel2.Text = "&Cancel";
            this.btnCancel2.UseVisualStyleBackColor = true;
            this.btnCancel2.Click += new System.EventHandler(this.btnCancel2_Click);
            // 
            // btnPreviousSource
            // 
            this.btnPreviousSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreviousSource.Location = new System.Drawing.Point(525, 320);
            this.btnPreviousSource.Name = "btnPreviousSource";
            this.btnPreviousSource.Size = new System.Drawing.Size(80, 23);
            this.btnPreviousSource.TabIndex = 3;
            this.btnPreviousSource.Text = "< &Previous";
            this.btnPreviousSource.UseVisualStyleBackColor = true;
            this.btnPreviousSource.Click += new System.EventHandler(this.btnPreviousSource_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFinish.Location = new System.Drawing.Point(611, 320);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 23);
            this.btnFinish.TabIndex = 1;
            this.btnFinish.Text = "&Finish";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnNextResults_Click);
            // 
            // tpResults
            // 
            this.tpResults.Controls.Add(this.dgvResults);
            this.tpResults.Controls.Add(this.btnFixErrors);
            this.tpResults.Controls.Add(this.lblImportErrors);
            this.tpResults.Controls.Add(this.lblImportResults);
            this.tpResults.Controls.Add(this.btnImportMore);
            this.tpResults.Controls.Add(this.btnClose);
            this.tpResults.Location = new System.Drawing.Point(4, 22);
            this.tpResults.Name = "tpResults";
            this.tpResults.Size = new System.Drawing.Size(694, 346);
            this.tpResults.TabIndex = 4;
            this.tpResults.Text = "Results";
            this.tpResults.UseVisualStyleBackColor = true;
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.AllowUserToResizeRows = false;
            this.dgvResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvResults.CausesValidation = false;
            this.dgvResults.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgcResult,
            this.dgcRowIndex,
            this.dgcTableName});
            this.dgvResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvResults.Location = new System.Drawing.Point(8, 24);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResults.ShowCellToolTips = false;
            this.dgvResults.ShowEditingIcon = false;
            this.dgvResults.Size = new System.Drawing.Size(678, 287);
            this.dgvResults.TabIndex = 8;
            this.dgvResults.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvResults_RowPrePaint);
            // 
            // dgcResult
            // 
            this.dgcResult.HeaderText = "Result";
            this.dgcResult.Name = "dgcResult";
            this.dgcResult.ReadOnly = true;
            // 
            // dgcRowIndex
            // 
            this.dgcRowIndex.HeaderText = "Row Index";
            this.dgcRowIndex.Name = "dgcRowIndex";
            this.dgcRowIndex.ReadOnly = true;
            // 
            // dgcTableName
            // 
            this.dgcTableName.HeaderText = "Table Name";
            this.dgcTableName.Name = "dgcTableName";
            this.dgcTableName.ReadOnly = true;
            // 
            // btnFixErrors
            // 
            this.btnFixErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFixErrors.Location = new System.Drawing.Point(389, 317);
            this.btnFixErrors.Name = "btnFixErrors";
            this.btnFixErrors.Size = new System.Drawing.Size(87, 23);
            this.btnFixErrors.TabIndex = 7;
            this.btnFixErrors.Text = "< &Fix Errors";
            this.btnFixErrors.UseVisualStyleBackColor = true;
            this.btnFixErrors.Click += new System.EventHandler(this.btnImportRetry_Click);
            // 
            // lblImportErrors
            // 
            this.lblImportErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblImportErrors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImportErrors.ForeColor = System.Drawing.Color.Red;
            this.lblImportErrors.Location = new System.Drawing.Point(9, 317);
            this.lblImportErrors.Name = "lblImportErrors";
            this.lblImportErrors.Size = new System.Drawing.Size(371, 23);
            this.lblImportErrors.TabIndex = 6;
            this.lblImportErrors.Text = "Errors Go Here";
            this.lblImportErrors.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblImportResults
            // 
            this.lblImportResults.AutoSize = true;
            this.lblImportResults.Location = new System.Drawing.Point(9, 8);
            this.lblImportResults.Name = "lblImportResults";
            this.lblImportResults.Size = new System.Drawing.Size(74, 13);
            this.lblImportResults.TabIndex = 4;
            this.lblImportResults.Text = "Import Results";
            // 
            // btnImportMore
            // 
            this.btnImportMore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportMore.Location = new System.Drawing.Point(482, 317);
            this.btnImportMore.Name = "btnImportMore";
            this.btnImportMore.Size = new System.Drawing.Size(120, 23);
            this.btnImportMore.TabIndex = 3;
            this.btnImportMore.Text = "&Import More Data";
            this.btnImportMore.UseVisualStyleBackColor = true;
            this.btnImportMore.Click += new System.EventHandler(this.btnImportMore_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(608, 317);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "E&xit";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblServerName
            // 
            this.lblServerName.Location = new System.Drawing.Point(1, 56);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(276, 23);
            this.lblServerName.TabIndex = 10;
            this.lblServerName.Text = "-- server name here --";
            this.lblServerName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lnkWhyNoDragDrop
            // 
            this.lnkWhyNoDragDrop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkWhyNoDragDrop.AutoSize = true;
            this.lnkWhyNoDragDrop.Location = new System.Drawing.Point(509, 458);
            this.lnkWhyNoDragDrop.Name = "lnkWhyNoDragDrop";
            this.lnkWhyNoDragDrop.Size = new System.Drawing.Size(167, 13);
            this.lnkWhyNoDragDrop.TabIndex = 7;
            this.lnkWhyNoDragDrop.TabStop = true;
            this.lnkWhyNoDragDrop.Text = "Why doesn\'t drag-and-drop work?";
            this.lnkWhyNoDragDrop.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWhyNoDragDrop_LinkClicked);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusCount,
            this.toolStripStatusLabel1,
            this.statusRow,
            this.toolStripStatusLabel2,
            this.statusTable,
            this.toolStripStatusLabel3,
            this.statusText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 454);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(702, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusCount
            // 
            this.statusCount.Name = "statusCount";
            this.statusCount.Size = new System.Drawing.Size(41, 17);
            this.statusCount.Text = "0 rows";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // statusRow
            // 
            this.statusRow.Name = "statusRow";
            this.statusRow.Size = new System.Drawing.Size(12, 17);
            this.statusRow.Text = "-";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // statusTable
            // 
            this.statusTable.Name = "statusTable";
            this.statusTable.Size = new System.Drawing.Size(12, 17);
            this.statusTable.Text = "-";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel3.Text = "|";
            // 
            // statusText
            // 
            this.statusText.ForeColor = System.Drawing.Color.Red;
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(12, 17);
            this.statusText.Text = "-";
            this.statusText.MouseEnter += new System.EventHandler(this.statusText_MouseEnter);
            this.statusText.MouseLeave += new System.EventHandler(this.statusText_MouseLeave);
            this.statusText.Click += new System.EventHandler(this.statusText_Click);
            // 
            // pb
            // 
            this.pb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pb.Location = new System.Drawing.Point(0, 3);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(702, 23);
            this.pb.TabIndex = 2;
            this.pb.Value = 33;
            // 
            // ofdSource
            // 
            this.ofdSource.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            // 
            // lblDataType
            // 
            this.lblDataType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDataType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataType.Location = new System.Drawing.Point(3, 0);
            this.lblDataType.Name = "lblDataType";
            this.lblDataType.Size = new System.Drawing.Size(225, 45);
            this.lblDataType.TabIndex = 4;
            this.lblDataType.Text = "1.  Choose Type of Data";
            this.lblDataType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSource
            // 
            this.lblSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSource.Location = new System.Drawing.Point(234, 0);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(225, 45);
            this.lblSource.TabIndex = 5;
            this.lblSource.Text = "2. Specify Data";
            this.lblSource.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResults
            // 
            this.lblResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResults.Location = new System.Drawing.Point(465, 0);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(227, 45);
            this.lblResults.TabIndex = 6;
            this.lblResults.Text = "3.  View Results";
            this.lblResults.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlp
            // 
            this.tlp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tlp.ColumnCount = 3;
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlp.Controls.Add(this.lblDataType, 0, 0);
            this.tlp.Controls.Add(this.lblResults, 2, 0);
            this.tlp.Controls.Add(this.lblSource, 1, 0);
            this.tlp.Location = new System.Drawing.Point(3, 8);
            this.tlp.Name = "tlp";
            this.tlp.RowCount = 1;
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlp.Size = new System.Drawing.Size(695, 45);
            this.tlp.TabIndex = 7;
            // 
            // lblDataviewTitle
            // 
            this.lblDataviewTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDataviewTitle.Location = new System.Drawing.Point(283, 56);
            this.lblDataviewTitle.Name = "lblDataviewTitle";
            this.lblDataviewTitle.Size = new System.Drawing.Size(415, 23);
            this.lblDataviewTitle.TabIndex = 12;
            this.lblDataviewTitle.Text = "-- Select Dataview --";
            this.lblDataviewTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Result";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Row Index";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Table Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // frmImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 476);
            this.Controls.Add(this.lblServerName);
            this.Controls.Add(this.lblDataviewTitle);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.lnkWhyNoDragDrop);
            this.Controls.Add(this.tlp);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tc);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GRIN-Global Import Wizard";
            this.Load += new System.EventHandler(this.frmImport_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmImport_KeyPress);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImport_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmImport_KeyDown);
            this.tc.ResumeLayout(false);
            this.tpDataType.ResumeLayout(false);
            this.tpDataType.PerformLayout();
            this.tpSource.ResumeLayout(false);
            this.tpSource.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSource)).EndInit();
            this.cmImport.ResumeLayout(false);
            this.tpPreview.ResumeLayout(false);
            this.tpResults.ResumeLayout(false);
            this.tpResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tlp.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tc;
        private System.Windows.Forms.TabPage tpDataType;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.TabPage tpSource;
        private System.Windows.Forms.TabPage tpPreview;
        private System.Windows.Forms.TabPage tpResults;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPreviousDataType;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnPreviousSource;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Label lblDataTypeTitle;
        private System.Windows.Forms.ComboBox ddlDataType;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnLoadFromFile;
        private System.Windows.Forms.DataGridView dgvSource;
        private System.Windows.Forms.OpenFileDialog ofdSource;
        private System.Windows.Forms.Button btnCancel1;
        private System.Windows.Forms.Button btnCancel4;
        private System.Windows.Forms.Button btnCancel2;
        private System.Windows.Forms.Label lblDataType;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.Button btnImportMore;
        private System.Windows.Forms.Label lblImportResults;
        private System.Windows.Forms.Label lblImportErrors;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddlLanguage;
        private System.Windows.Forms.LinkLabel lnkWhyNoDragDrop;
        private System.Windows.Forms.TableLayoutPanel tlp;
        private System.Windows.Forms.LinkLabel lnkRequired;
        private System.Windows.Forms.Label lblRequired;
        private System.Windows.Forms.CheckBox chkViewAllExistingData;
        private System.Windows.Forms.ContextMenuStrip cmImport;
        private System.Windows.Forms.ToolStripMenuItem cmiImportDelete;
        private System.Windows.Forms.Button btnFixErrors;
        private System.Windows.Forms.LinkLabel lnkNextError;
        private System.Windows.Forms.ToolStripStatusLabel statusCount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel statusRow;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Label lblSearchEngineLag;
        private System.Windows.Forms.ToolStripMenuItem cmiImportCopy;
        private System.Windows.Forms.ToolStripMenuItem cmiImportPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel statusTable;
        private System.Windows.Forms.CheckBox chkDisplayOnlyErrors;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcRowIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcTableName;
        private System.Windows.Forms.ComboBox ddlOwnedBy;
        private System.Windows.Forms.Label lblOwnedBy;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Label lblDataviewTitle;
    }
}

