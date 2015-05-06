namespace GrinGlobal.Admin.ChildForms {
    partial class frmSearchEngineResolver {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchEngineResolver));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.cmField = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newRestrictionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtPrimaryKeyField = new System.Windows.Forms.TextBox();
            this.lblPrimaryKeyField = new System.Windows.Forms.Label();
            this.tcResolver = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.ddlSearchDataviews = new System.Windows.Forms.ComboBox();
            this.lblDataviewName = new System.Windows.Forms.Label();
            this.btnNewDataview = new System.Windows.Forms.Button();
            this.btnEditDataview = new System.Windows.Forms.Button();
            this.txtResolvedPrimaryKeyField = new System.Windows.Forms.TextBox();
            this.lblResolvedPrimaryKeyField = new System.Windows.Forms.Label();
            this.ddlMethod = new System.Windows.Forms.ComboBox();
            this.lblMethod = new System.Windows.Forms.Label();
            this.chkAllowRealtimeUpdates = new System.Windows.Forms.CheckBox();
            this.txtForeignKeyField = new System.Windows.Forms.TextBox();
            this.lblForeignKeyField = new System.Windows.Forms.Label();
            this.tpAdvanced = new System.Windows.Forms.TabPage();
            this.ddlCacheMode = new System.Windows.Forms.ComboBox();
            this.lblCacheMode = new System.Windows.Forms.Label();
            this.txtKeywordCacheSize = new System.Windows.Forms.TextBox();
            this.lblKeywordCacheSize = new System.Windows.Forms.Label();
            this.txtNodeCacheSize = new System.Windows.Forms.TextBox();
            this.lblNodeCacheSize = new System.Windows.Forms.Label();
            this.gbResolverGeneration = new System.Windows.Forms.GroupBox();
            this.txtEncoding = new System.Windows.Forms.TextBox();
            this.lblEncoding = new System.Windows.Forms.Label();
            this.txtFanoutSize = new System.Windows.Forms.TextBox();
            this.lblFanoutSize = new System.Windows.Forms.Label();
            this.txtAverageKeywordSize = new System.Windows.Forms.TextBox();
            this.lblAverageKeywordSize = new System.Windows.Forms.Label();
            this.cmResolver = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmParameter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.tcResolver.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpAdvanced.SuspendLayout();
            this.gbResolverGeneration.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(425, 427);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 51;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(344, 427);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 50;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(173, 22);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(200, 20);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(7, 25);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(160, 13);
            this.lblName.TabIndex = 15;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Location = new System.Drawing.Point(173, 48);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkEnabled.TabIndex = 3;
            this.chkEnabled.Text = "Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // cmField
            // 
            this.cmField.Name = "ctxMenuField";
            this.cmField.Size = new System.Drawing.Size(61, 4);
            // 
            // newRestrictionToolStripMenuItem
            // 
            this.newRestrictionToolStripMenuItem.Name = "newRestrictionToolStripMenuItem";
            this.newRestrictionToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // txtPrimaryKeyField
            // 
            this.txtPrimaryKeyField.Location = new System.Drawing.Point(173, 124);
            this.txtPrimaryKeyField.Name = "txtPrimaryKeyField";
            this.txtPrimaryKeyField.Size = new System.Drawing.Size(200, 20);
            this.txtPrimaryKeyField.TabIndex = 54;
            this.txtPrimaryKeyField.TextChanged += new System.EventHandler(this.txtPrimaryKeyField_TextChanged);
            // 
            // lblPrimaryKeyField
            // 
            this.lblPrimaryKeyField.Location = new System.Drawing.Point(10, 127);
            this.lblPrimaryKeyField.Name = "lblPrimaryKeyField";
            this.lblPrimaryKeyField.Size = new System.Drawing.Size(157, 13);
            this.lblPrimaryKeyField.TabIndex = 55;
            this.lblPrimaryKeyField.Text = "Primary Key Field:";
            this.lblPrimaryKeyField.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tcResolver
            // 
            this.tcResolver.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcResolver.Controls.Add(this.tpGeneral);
            this.tcResolver.Controls.Add(this.tpAdvanced);
            this.tcResolver.Location = new System.Drawing.Point(16, 12);
            this.tcResolver.Name = "tcResolver";
            this.tcResolver.SelectedIndex = 0;
            this.tcResolver.Size = new System.Drawing.Size(485, 409);
            this.tcResolver.TabIndex = 60;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.ddlSearchDataviews);
            this.tpGeneral.Controls.Add(this.lblDataviewName);
            this.tpGeneral.Controls.Add(this.btnNewDataview);
            this.tpGeneral.Controls.Add(this.btnEditDataview);
            this.tpGeneral.Controls.Add(this.txtResolvedPrimaryKeyField);
            this.tpGeneral.Controls.Add(this.lblResolvedPrimaryKeyField);
            this.tpGeneral.Controls.Add(this.ddlMethod);
            this.tpGeneral.Controls.Add(this.lblMethod);
            this.tpGeneral.Controls.Add(this.chkAllowRealtimeUpdates);
            this.tpGeneral.Controls.Add(this.chkEnabled);
            this.tpGeneral.Controls.Add(this.txtForeignKeyField);
            this.tpGeneral.Controls.Add(this.lblForeignKeyField);
            this.tpGeneral.Controls.Add(this.txtPrimaryKeyField);
            this.tpGeneral.Controls.Add(this.txtName);
            this.tpGeneral.Controls.Add(this.lblName);
            this.tpGeneral.Controls.Add(this.lblPrimaryKeyField);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(477, 383);
            this.tpGeneral.TabIndex = 1;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // ddlSearchDataviews
            // 
            this.ddlSearchDataviews.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlSearchDataviews.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSearchDataviews.FormattingEnabled = true;
            this.ddlSearchDataviews.Location = new System.Drawing.Point(172, 177);
            this.ddlSearchDataviews.Name = "ddlSearchDataviews";
            this.ddlSearchDataviews.Size = new System.Drawing.Size(227, 21);
            this.ddlSearchDataviews.TabIndex = 84;
            this.ddlSearchDataviews.SelectedIndexChanged += new System.EventHandler(this.ddlSearchDataviews_SelectedIndexChanged);
            // 
            // lblDataviewName
            // 
            this.lblDataviewName.Location = new System.Drawing.Point(18, 180);
            this.lblDataviewName.Name = "lblDataviewName";
            this.lblDataviewName.Size = new System.Drawing.Size(151, 13);
            this.lblDataviewName.TabIndex = 83;
            this.lblDataviewName.Text = "Dataview Name:";
            this.lblDataviewName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnNewDataview
            // 
            this.btnNewDataview.Location = new System.Drawing.Point(172, 204);
            this.btnNewDataview.Name = "btnNewDataview";
            this.btnNewDataview.Size = new System.Drawing.Size(122, 23);
            this.btnNewDataview.TabIndex = 81;
            this.btnNewDataview.Text = "&New Dataview...";
            this.btnNewDataview.UseVisualStyleBackColor = true;
            this.btnNewDataview.Click += new System.EventHandler(this.btnNewDataview_Click);
            // 
            // btnEditDataview
            // 
            this.btnEditDataview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditDataview.Location = new System.Drawing.Point(407, 177);
            this.btnEditDataview.Name = "btnEditDataview";
            this.btnEditDataview.Size = new System.Drawing.Size(59, 23);
            this.btnEditDataview.TabIndex = 80;
            this.btnEditDataview.Text = "&Edit...";
            this.btnEditDataview.UseVisualStyleBackColor = true;
            this.btnEditDataview.Click += new System.EventHandler(this.btnEditDataview_Click);
            // 
            // txtResolvedPrimaryKeyField
            // 
            this.txtResolvedPrimaryKeyField.Location = new System.Drawing.Point(173, 151);
            this.txtResolvedPrimaryKeyField.Name = "txtResolvedPrimaryKeyField";
            this.txtResolvedPrimaryKeyField.Size = new System.Drawing.Size(200, 20);
            this.txtResolvedPrimaryKeyField.TabIndex = 76;
            this.txtResolvedPrimaryKeyField.TextChanged += new System.EventHandler(this.txtResolvedPrimaryKeyField_TextChanged);
            // 
            // lblResolvedPrimaryKeyField
            // 
            this.lblResolvedPrimaryKeyField.Location = new System.Drawing.Point(7, 154);
            this.lblResolvedPrimaryKeyField.Name = "lblResolvedPrimaryKeyField";
            this.lblResolvedPrimaryKeyField.Size = new System.Drawing.Size(160, 13);
            this.lblResolvedPrimaryKeyField.TabIndex = 77;
            this.lblResolvedPrimaryKeyField.Text = "Resolved Primary Key Field:";
            this.lblResolvedPrimaryKeyField.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ddlMethod
            // 
            this.ddlMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlMethod.FormattingEnabled = true;
            this.ddlMethod.Items.AddRange(new object[] {
            "None",
            "Index\'s Primary Key",
            "Foreign Key",
            "Sql"});
            this.ddlMethod.Location = new System.Drawing.Point(173, 71);
            this.ddlMethod.Name = "ddlMethod";
            this.ddlMethod.Size = new System.Drawing.Size(121, 21);
            this.ddlMethod.TabIndex = 75;
            this.ddlMethod.SelectedIndexChanged += new System.EventHandler(this.ddlMethod_SelectedIndexChanged);
            // 
            // lblMethod
            // 
            this.lblMethod.Location = new System.Drawing.Point(7, 74);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(160, 13);
            this.lblMethod.TabIndex = 70;
            this.lblMethod.Text = "Method:";
            this.lblMethod.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkAllowRealtimeUpdates
            // 
            this.chkAllowRealtimeUpdates.AutoSize = true;
            this.chkAllowRealtimeUpdates.Location = new System.Drawing.Point(244, 48);
            this.chkAllowRealtimeUpdates.Name = "chkAllowRealtimeUpdates";
            this.chkAllowRealtimeUpdates.Size = new System.Drawing.Size(138, 17);
            this.chkAllowRealtimeUpdates.TabIndex = 69;
            this.chkAllowRealtimeUpdates.Text = "Allow Realtime Updates";
            this.chkAllowRealtimeUpdates.UseVisualStyleBackColor = true;
            this.chkAllowRealtimeUpdates.CheckedChanged += new System.EventHandler(this.chkAllowRealtimeUpdates_CheckedChanged);
            // 
            // txtForeignKeyField
            // 
            this.txtForeignKeyField.Location = new System.Drawing.Point(173, 98);
            this.txtForeignKeyField.Name = "txtForeignKeyField";
            this.txtForeignKeyField.Size = new System.Drawing.Size(200, 20);
            this.txtForeignKeyField.TabIndex = 71;
            this.txtForeignKeyField.TextChanged += new System.EventHandler(this.txtForeignKeyField_TextChanged);
            // 
            // lblForeignKeyField
            // 
            this.lblForeignKeyField.Location = new System.Drawing.Point(7, 101);
            this.lblForeignKeyField.Name = "lblForeignKeyField";
            this.lblForeignKeyField.Size = new System.Drawing.Size(160, 13);
            this.lblForeignKeyField.TabIndex = 72;
            this.lblForeignKeyField.Text = "Foreign Key Field:";
            this.lblForeignKeyField.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tpAdvanced
            // 
            this.tpAdvanced.Controls.Add(this.ddlCacheMode);
            this.tpAdvanced.Controls.Add(this.lblCacheMode);
            this.tpAdvanced.Controls.Add(this.txtKeywordCacheSize);
            this.tpAdvanced.Controls.Add(this.lblKeywordCacheSize);
            this.tpAdvanced.Controls.Add(this.txtNodeCacheSize);
            this.tpAdvanced.Controls.Add(this.lblNodeCacheSize);
            this.tpAdvanced.Controls.Add(this.gbResolverGeneration);
            this.tpAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tpAdvanced.Name = "tpAdvanced";
            this.tpAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tpAdvanced.Size = new System.Drawing.Size(477, 383);
            this.tpAdvanced.TabIndex = 2;
            this.tpAdvanced.Text = "Advanced";
            this.tpAdvanced.UseVisualStyleBackColor = true;
            // 
            // ddlCacheMode
            // 
            this.ddlCacheMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCacheMode.FormattingEnabled = true;
            this.ddlCacheMode.Items.AddRange(new object[] {
            "None",
            "Id",
            "Id And Keyword"});
            this.ddlCacheMode.Location = new System.Drawing.Point(172, 19);
            this.ddlCacheMode.Name = "ddlCacheMode";
            this.ddlCacheMode.Size = new System.Drawing.Size(121, 21);
            this.ddlCacheMode.TabIndex = 85;
            // 
            // lblCacheMode
            // 
            this.lblCacheMode.Location = new System.Drawing.Point(6, 22);
            this.lblCacheMode.Name = "lblCacheMode";
            this.lblCacheMode.Size = new System.Drawing.Size(160, 13);
            this.lblCacheMode.TabIndex = 84;
            this.lblCacheMode.Text = "Cache Mode:";
            this.lblCacheMode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtKeywordCacheSize
            // 
            this.txtKeywordCacheSize.Location = new System.Drawing.Point(172, 72);
            this.txtKeywordCacheSize.Name = "txtKeywordCacheSize";
            this.txtKeywordCacheSize.Size = new System.Drawing.Size(200, 20);
            this.txtKeywordCacheSize.TabIndex = 82;
            // 
            // lblKeywordCacheSize
            // 
            this.lblKeywordCacheSize.Location = new System.Drawing.Point(6, 75);
            this.lblKeywordCacheSize.Name = "lblKeywordCacheSize";
            this.lblKeywordCacheSize.Size = new System.Drawing.Size(160, 13);
            this.lblKeywordCacheSize.TabIndex = 83;
            this.lblKeywordCacheSize.Text = "Keyword Cache Size:";
            this.lblKeywordCacheSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtNodeCacheSize
            // 
            this.txtNodeCacheSize.Location = new System.Drawing.Point(172, 46);
            this.txtNodeCacheSize.Name = "txtNodeCacheSize";
            this.txtNodeCacheSize.Size = new System.Drawing.Size(200, 20);
            this.txtNodeCacheSize.TabIndex = 80;
            // 
            // lblNodeCacheSize
            // 
            this.lblNodeCacheSize.BackColor = System.Drawing.Color.Transparent;
            this.lblNodeCacheSize.Location = new System.Drawing.Point(6, 49);
            this.lblNodeCacheSize.Name = "lblNodeCacheSize";
            this.lblNodeCacheSize.Size = new System.Drawing.Size(160, 13);
            this.lblNodeCacheSize.TabIndex = 81;
            this.lblNodeCacheSize.Text = "Node Cache Size:";
            this.lblNodeCacheSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // gbResolverGeneration
            // 
            this.gbResolverGeneration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbResolverGeneration.Controls.Add(this.txtEncoding);
            this.gbResolverGeneration.Controls.Add(this.lblEncoding);
            this.gbResolverGeneration.Controls.Add(this.txtFanoutSize);
            this.gbResolverGeneration.Controls.Add(this.lblFanoutSize);
            this.gbResolverGeneration.Controls.Add(this.txtAverageKeywordSize);
            this.gbResolverGeneration.Controls.Add(this.lblAverageKeywordSize);
            this.gbResolverGeneration.Location = new System.Drawing.Point(6, 112);
            this.gbResolverGeneration.Name = "gbResolverGeneration";
            this.gbResolverGeneration.Size = new System.Drawing.Size(458, 127);
            this.gbResolverGeneration.TabIndex = 79;
            this.gbResolverGeneration.TabStop = false;
            this.gbResolverGeneration.Text = "During Resolver Generation...";
            // 
            // txtEncoding
            // 
            this.txtEncoding.Location = new System.Drawing.Point(166, 80);
            this.txtEncoding.Name = "txtEncoding";
            this.txtEncoding.Size = new System.Drawing.Size(200, 20);
            this.txtEncoding.TabIndex = 69;
            // 
            // lblEncoding
            // 
            this.lblEncoding.Location = new System.Drawing.Point(9, 83);
            this.lblEncoding.Name = "lblEncoding";
            this.lblEncoding.Size = new System.Drawing.Size(151, 13);
            this.lblEncoding.TabIndex = 70;
            this.lblEncoding.Text = "Encoding:";
            this.lblEncoding.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtFanoutSize
            // 
            this.txtFanoutSize.Location = new System.Drawing.Point(166, 54);
            this.txtFanoutSize.Name = "txtFanoutSize";
            this.txtFanoutSize.Size = new System.Drawing.Size(200, 20);
            this.txtFanoutSize.TabIndex = 61;
            // 
            // lblFanoutSize
            // 
            this.lblFanoutSize.Location = new System.Drawing.Point(9, 57);
            this.lblFanoutSize.Name = "lblFanoutSize";
            this.lblFanoutSize.Size = new System.Drawing.Size(151, 13);
            this.lblFanoutSize.TabIndex = 62;
            this.lblFanoutSize.Text = "Tree Fanout Size:";
            this.lblFanoutSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtAverageKeywordSize
            // 
            this.txtAverageKeywordSize.Location = new System.Drawing.Point(166, 28);
            this.txtAverageKeywordSize.Name = "txtAverageKeywordSize";
            this.txtAverageKeywordSize.Size = new System.Drawing.Size(200, 20);
            this.txtAverageKeywordSize.TabIndex = 59;
            // 
            // lblAverageKeywordSize
            // 
            this.lblAverageKeywordSize.Location = new System.Drawing.Point(6, 31);
            this.lblAverageKeywordSize.Name = "lblAverageKeywordSize";
            this.lblAverageKeywordSize.Size = new System.Drawing.Size(154, 13);
            this.lblAverageKeywordSize.TabIndex = 60;
            this.lblAverageKeywordSize.Text = "Average Keyword Size:";
            this.lblAverageKeywordSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmResolver
            // 
            this.cmResolver.Name = "ctxMenuResolver";
            this.cmResolver.Size = new System.Drawing.Size(61, 4);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 6);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 6);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(32, 19);
            // 
            // cmParameter
            // 
            this.cmParameter.Name = "ctxMenuParameter";
            this.cmParameter.Size = new System.Drawing.Size(61, 4);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(32, 19);
            // 
            // frmSearchEngineResolver
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(513, 462);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tcResolver);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSearchEngineResolver";
            this.Text = "Search Engine Resolver";
            this.Load += new System.EventHandler(this.frmSearchEngineResolver_Load);
            this.tcResolver.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            this.tpAdvanced.ResumeLayout(false);
            this.tpAdvanced.PerformLayout();
            this.gbResolverGeneration.ResumeLayout(false);
            this.gbResolverGeneration.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.ContextMenuStrip cmField;
        private System.Windows.Forms.ToolStripMenuItem newRestrictionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.TextBox txtPrimaryKeyField;
        private System.Windows.Forms.Label lblPrimaryKeyField;
        private System.Windows.Forms.TabControl tcResolver;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.ContextMenuStrip cmParameter;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ContextMenuStrip cmResolver;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.TextBox txtForeignKeyField;
        private System.Windows.Forms.Label lblForeignKeyField;
        private System.Windows.Forms.CheckBox chkAllowRealtimeUpdates;
        private System.Windows.Forms.ComboBox ddlMethod;
        private System.Windows.Forms.TextBox txtResolvedPrimaryKeyField;
        private System.Windows.Forms.Label lblResolvedPrimaryKeyField;
        private System.Windows.Forms.Button btnNewDataview;
        private System.Windows.Forms.Button btnEditDataview;
        private System.Windows.Forms.TabPage tpAdvanced;
        private System.Windows.Forms.GroupBox gbResolverGeneration;
        private System.Windows.Forms.TextBox txtEncoding;
        private System.Windows.Forms.Label lblEncoding;
        private System.Windows.Forms.TextBox txtFanoutSize;
        private System.Windows.Forms.Label lblFanoutSize;
        private System.Windows.Forms.TextBox txtAverageKeywordSize;
        private System.Windows.Forms.Label lblAverageKeywordSize;
        private System.Windows.Forms.ComboBox ddlSearchDataviews;
        private System.Windows.Forms.Label lblDataviewName;
        private System.Windows.Forms.ComboBox ddlCacheMode;
        private System.Windows.Forms.Label lblCacheMode;
        private System.Windows.Forms.TextBox txtKeywordCacheSize;
        private System.Windows.Forms.Label lblKeywordCacheSize;
        private System.Windows.Forms.TextBox txtNodeCacheSize;
        private System.Windows.Forms.Label lblNodeCacheSize;
    }
}
