namespace GrinGlobal.Admin.ChildForms {
    partial class frmSearchEngineIndexes {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchEngineIndexes));
            this.lvIndexes = new System.Windows.Forms.ListView();
            this.colIndexName = new System.Windows.Forms.ColumnHeader();
            this.colEnabled = new System.Windows.Forms.ColumnHeader();
            this.colResolvers = new System.Windows.Forms.ColumnHeader();
            this.colValid = new System.Windows.Forms.ColumnHeader();
            this.colLastRebuild = new System.Windows.Forms.ColumnHeader();
            this.cmIndex = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiIndexNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiIndexEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiIndexDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiIndexDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiIndexRebuild = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiIndexDefragment = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiIndexReload = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiIndexVerify = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiIndexRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiIndexProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblPendingRealtimeUpdates = new System.Windows.Forms.Label();
            this.lblPendingRealtimeUpdatesCount = new System.Windows.Forms.Label();
            this.lblClientCount = new System.Windows.Forms.Label();
            this.lblConnectedClients = new System.Windows.Forms.Label();
            this.lblPendingIndexCreations = new System.Windows.Forms.Label();
            this.lblProcessingIndexes = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tcIndexes = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.btnTest = new System.Windows.Forms.Button();
            this.lblNotRegressionTested = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbGlobalSettings = new System.Windows.Forms.GroupBox();
            this.gbSeparators = new System.Windows.Forms.GroupBox();
            this.btnRemoveSeparator = new System.Windows.Forms.Button();
            this.btnAddSeparator = new System.Windows.Forms.Button();
            this.lbSeparators = new System.Windows.Forms.ListBox();
            this.btnDatabasePrompt = new System.Windows.Forms.Button();
            this.gbStopWords = new System.Windows.Forms.GroupBox();
            this.btnRemoveStopWord = new System.Windows.Forms.Button();
            this.btnAddStopWord = new System.Windows.Forms.Button();
            this.lbStopWords = new System.Windows.Forms.ListBox();
            this.ddlProvider = new System.Windows.Forms.ComboBox();
            this.lblDatabaseProvider = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.lblDatabaseConnectionString = new System.Windows.Forms.Label();
            this.txtTermCacheMinimum = new System.Windows.Forms.TextBox();
            this.lblTermCacheMinimum = new System.Windows.Forms.Label();
            this.chkTermCachingEnabled = new System.Windows.Forms.CheckBox();
            this.tpIndexes = new System.Windows.Forms.TabPage();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.tpStatus = new System.Windows.Forms.TabPage();
            this.tpLog = new System.Windows.Forms.TabPage();
            this.btnRefreshLog = new System.Windows.Forms.Button();
            this.btnShowInTextEditor = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lvLog = new System.Windows.Forms.ListView();
            this.colDate = new System.Windows.Forms.ColumnHeader();
            this.colType = new System.Windows.Forms.ColumnHeader();
            this.colMessage = new System.Windows.Forms.ColumnHeader();
            this.cmLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiLogRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiIndexRebuildAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cmIndex.SuspendLayout();
            this.tcIndexes.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.gbGlobalSettings.SuspendLayout();
            this.gbSeparators.SuspendLayout();
            this.gbStopWords.SuspendLayout();
            this.tpIndexes.SuspendLayout();
            this.tpStatus.SuspendLayout();
            this.tpLog.SuspendLayout();
            this.cmLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvIndexes
            // 
            this.lvIndexes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvIndexes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIndexName,
            this.colEnabled,
            this.colResolvers,
            this.colValid,
            this.colLastRebuild});
            this.lvIndexes.ContextMenuStrip = this.cmIndex;
            this.lvIndexes.FullRowSelect = true;
            this.lvIndexes.HideSelection = false;
            this.lvIndexes.Location = new System.Drawing.Point(6, 6);
            this.lvIndexes.Name = "lvIndexes";
            this.lvIndexes.ShowItemToolTips = true;
            this.lvIndexes.Size = new System.Drawing.Size(482, 331);
            this.lvIndexes.SmallImageList = this.imageList1;
            this.lvIndexes.TabIndex = 1;
            this.lvIndexes.UseCompatibleStateImageBehavior = false;
            this.lvIndexes.View = System.Windows.Forms.View.Details;
            this.lvIndexes.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvIndexes_MouseDoubleClick);
            this.lvIndexes.SelectedIndexChanged += new System.EventHandler(this.lvIndexes_SelectedIndexChanged);
            this.lvIndexes.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvIndexes_KeyUp);
            this.lvIndexes.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvIndexes_ItemDrag);
            // 
            // colIndexName
            // 
            this.colIndexName.Text = "Index";
            this.colIndexName.Width = 110;
            // 
            // colEnabled
            // 
            this.colEnabled.Text = "Enabled?";
            // 
            // colResolvers
            // 
            this.colResolvers.Text = "Resolvers";
            this.colResolvers.Width = 250;
            // 
            // colValid
            // 
            this.colValid.Text = "Valid?";
            // 
            // colLastRebuild
            // 
            this.colLastRebuild.Text = "Last Rebuild";
            this.colLastRebuild.Width = 130;
            // 
            // cmIndex
            // 
            this.cmIndex.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiIndexNew,
            this.toolStripSeparator1,
            this.cmiIndexEnable,
            this.cmiIndexDisable,
            this.cmiIndexDelete,
            this.toolStripSeparator18,
            this.cmiIndexRebuildAll,
            this.cmiIndexRebuild,
            this.cmiIndexDefragment,
            this.toolStripSeparator3,
            this.cmiIndexReload,
            this.cmiIndexVerify,
            this.toolStripSeparator2,
            this.cmiIndexRefresh,
            this.cmiIndexProperties});
            this.cmIndex.Name = "ctxMenuNodeUser";
            this.cmIndex.Size = new System.Drawing.Size(153, 292);
            this.cmIndex.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuNodeIndex_Opening);
            // 
            // cmiIndexNew
            // 
            this.cmiIndexNew.Enabled = false;
            this.cmiIndexNew.Name = "cmiIndexNew";
            this.cmiIndexNew.Size = new System.Drawing.Size(152, 22);
            this.cmiIndexNew.Text = "&New Index...";
            this.cmiIndexNew.Click += new System.EventHandler(this.newIndexMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // cmiIndexEnable
            // 
            this.cmiIndexEnable.Name = "cmiIndexEnable";
            this.cmiIndexEnable.Size = new System.Drawing.Size(152, 22);
            this.cmiIndexEnable.Text = "&Enable";
            this.cmiIndexEnable.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // cmiIndexDisable
            // 
            this.cmiIndexDisable.Name = "cmiIndexDisable";
            this.cmiIndexDisable.Size = new System.Drawing.Size(152, 22);
            this.cmiIndexDisable.Text = "Disable";
            this.cmiIndexDisable.Click += new System.EventHandler(this.disableIndexMenuItem_Click);
            // 
            // cmiIndexDelete
            // 
            this.cmiIndexDelete.Name = "cmiIndexDelete";
            this.cmiIndexDelete.Size = new System.Drawing.Size(152, 22);
            this.cmiIndexDelete.Text = "&Delete";
            this.cmiIndexDelete.Click += new System.EventHandler(this.deleteIndexMenuItem_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(149, 6);
            // 
            // cmiIndexRebuild
            // 
            this.cmiIndexRebuild.Name = "cmiIndexRebuild";
            this.cmiIndexRebuild.Size = new System.Drawing.Size(152, 22);
            this.cmiIndexRebuild.Text = "Re&build";
            this.cmiIndexRebuild.Click += new System.EventHandler(this.toolStripMenuItem1_Click_1);
            // 
            // cmiIndexDefragment
            // 
            this.cmiIndexDefragment.Name = "cmiIndexDefragment";
            this.cmiIndexDefragment.Size = new System.Drawing.Size(152, 22);
            this.cmiIndexDefragment.Text = "De&fragment";
            this.cmiIndexDefragment.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
            // 
            // cmiIndexReload
            // 
            this.cmiIndexReload.Name = "cmiIndexReload";
            this.cmiIndexReload.Size = new System.Drawing.Size(152, 22);
            this.cmiIndexReload.Text = "Re&load";
            this.cmiIndexReload.Click += new System.EventHandler(this.cmiIndexReload_Click);
            // 
            // cmiIndexVerify
            // 
            this.cmiIndexVerify.Name = "cmiIndexVerify";
            this.cmiIndexVerify.Size = new System.Drawing.Size(152, 22);
            this.cmiIndexVerify.Text = "&Verify";
            this.cmiIndexVerify.Click += new System.EventHandler(this.cmiIndexVerify_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // cmiIndexRefresh
            // 
            this.cmiIndexRefresh.Name = "cmiIndexRefresh";
            this.cmiIndexRefresh.Size = new System.Drawing.Size(152, 22);
            this.cmiIndexRefresh.Text = "&Refresh";
            this.cmiIndexRefresh.Click += new System.EventHandler(this.refreshIndexMenuItem_Click);
            // 
            // cmiIndexProperties
            // 
            this.cmiIndexProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cmiIndexProperties.Name = "cmiIndexProperties";
            this.cmiIndexProperties.Size = new System.Drawing.Size(152, 22);
            this.cmiIndexProperties.Text = "&Properties";
            this.cmiIndexProperties.Click += new System.EventHandler(this.defaultIndexMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "GG_search.ico");
            // 
            // lblPendingRealtimeUpdates
            // 
            this.lblPendingRealtimeUpdates.Location = new System.Drawing.Point(16, 29);
            this.lblPendingRealtimeUpdates.Name = "lblPendingRealtimeUpdates";
            this.lblPendingRealtimeUpdates.Size = new System.Drawing.Size(152, 13);
            this.lblPendingRealtimeUpdates.TabIndex = 2;
            this.lblPendingRealtimeUpdates.Text = "Pending Realtime Updates:";
            this.lblPendingRealtimeUpdates.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblPendingRealtimeUpdatesCount
            // 
            this.lblPendingRealtimeUpdatesCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPendingRealtimeUpdatesCount.Location = new System.Drawing.Point(174, 24);
            this.lblPendingRealtimeUpdatesCount.Name = "lblPendingRealtimeUpdatesCount";
            this.lblPendingRealtimeUpdatesCount.Size = new System.Drawing.Size(81, 23);
            this.lblPendingRealtimeUpdatesCount.TabIndex = 3;
            this.lblPendingRealtimeUpdatesCount.Text = "0";
            this.lblPendingRealtimeUpdatesCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblClientCount
            // 
            this.lblClientCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClientCount.Location = new System.Drawing.Point(174, 109);
            this.lblClientCount.Name = "lblClientCount";
            this.lblClientCount.Size = new System.Drawing.Size(81, 23);
            this.lblClientCount.TabIndex = 5;
            this.lblClientCount.Text = "0";
            this.lblClientCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblClientCount.Visible = false;
            this.lblClientCount.Click += new System.EventHandler(this.lblClientCount_Click);
            // 
            // lblConnectedClients
            // 
            this.lblConnectedClients.Location = new System.Drawing.Point(19, 114);
            this.lblConnectedClients.Name = "lblConnectedClients";
            this.lblConnectedClients.Size = new System.Drawing.Size(149, 13);
            this.lblConnectedClients.TabIndex = 4;
            this.lblConnectedClients.Text = "Connected Clients:";
            this.lblConnectedClients.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblConnectedClients.Visible = false;
            this.lblConnectedClients.Click += new System.EventHandler(this.label3_Click);
            // 
            // lblPendingIndexCreations
            // 
            this.lblPendingIndexCreations.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPendingIndexCreations.Location = new System.Drawing.Point(174, 63);
            this.lblPendingIndexCreations.Name = "lblPendingIndexCreations";
            this.lblPendingIndexCreations.Size = new System.Drawing.Size(81, 23);
            this.lblPendingIndexCreations.TabIndex = 7;
            this.lblPendingIndexCreations.Text = "0";
            this.lblPendingIndexCreations.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProcessingIndexes
            // 
            this.lblProcessingIndexes.Location = new System.Drawing.Point(16, 68);
            this.lblProcessingIndexes.Name = "lblProcessingIndexes";
            this.lblProcessingIndexes.Size = new System.Drawing.Size(152, 13);
            this.lblProcessingIndexes.TabIndex = 6;
            this.lblProcessingIndexes.Text = "Processing Indexes:";
            this.lblProcessingIndexes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(16, 336);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(81, 23);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tcIndexes
            // 
            this.tcIndexes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcIndexes.Controls.Add(this.tpGeneral);
            this.tcIndexes.Controls.Add(this.tpIndexes);
            this.tcIndexes.Controls.Add(this.tpStatus);
            this.tcIndexes.Controls.Add(this.tpLog);
            this.tcIndexes.Location = new System.Drawing.Point(13, 13);
            this.tcIndexes.Name = "tcIndexes";
            this.tcIndexes.SelectedIndex = 0;
            this.tcIndexes.Size = new System.Drawing.Size(502, 398);
            this.tcIndexes.TabIndex = 12;
            this.tcIndexes.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.btnTest);
            this.tpGeneral.Controls.Add(this.lblNotRegressionTested);
            this.tpGeneral.Controls.Add(this.btnSave);
            this.tpGeneral.Controls.Add(this.gbGlobalSettings);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(494, 372);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTest.Location = new System.Drawing.Point(197, 343);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(177, 23);
            this.btnTest.TabIndex = 9;
            this.btnTest.Text = "&Test Import Search Dataviews";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // lblNotRegressionTested
            // 
            this.lblNotRegressionTested.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNotRegressionTested.AutoSize = true;
            this.lblNotRegressionTested.ForeColor = System.Drawing.Color.Blue;
            this.lblNotRegressionTested.Location = new System.Drawing.Point(13, 348);
            this.lblNotRegressionTested.Name = "lblNotRegressionTested";
            this.lblNotRegressionTested.Size = new System.Drawing.Size(141, 13);
            this.lblNotRegressionTested.TabIndex = 8;
            this.lblNotRegressionTested.Text = "* - Not fully regression tested";
            this.lblNotRegressionTested.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(413, 343);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbGlobalSettings
            // 
            this.gbGlobalSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbGlobalSettings.Controls.Add(this.gbSeparators);
            this.gbGlobalSettings.Controls.Add(this.btnDatabasePrompt);
            this.gbGlobalSettings.Controls.Add(this.gbStopWords);
            this.gbGlobalSettings.Controls.Add(this.ddlProvider);
            this.gbGlobalSettings.Controls.Add(this.lblDatabaseProvider);
            this.gbGlobalSettings.Controls.Add(this.txtConnectionString);
            this.gbGlobalSettings.Controls.Add(this.lblDatabaseConnectionString);
            this.gbGlobalSettings.Controls.Add(this.txtTermCacheMinimum);
            this.gbGlobalSettings.Controls.Add(this.lblTermCacheMinimum);
            this.gbGlobalSettings.Controls.Add(this.chkTermCachingEnabled);
            this.gbGlobalSettings.Location = new System.Drawing.Point(6, 6);
            this.gbGlobalSettings.Name = "gbGlobalSettings";
            this.gbGlobalSettings.Size = new System.Drawing.Size(482, 331);
            this.gbGlobalSettings.TabIndex = 3;
            this.gbGlobalSettings.TabStop = false;
            this.gbGlobalSettings.Text = "Global Settings";
            // 
            // gbSeparators
            // 
            this.gbSeparators.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gbSeparators.Controls.Add(this.btnRemoveSeparator);
            this.gbSeparators.Controls.Add(this.btnAddSeparator);
            this.gbSeparators.Controls.Add(this.lbSeparators);
            this.gbSeparators.Location = new System.Drawing.Point(10, 112);
            this.gbSeparators.Name = "gbSeparators";
            this.gbSeparators.Size = new System.Drawing.Size(192, 213);
            this.gbSeparators.TabIndex = 3;
            this.gbSeparators.TabStop = false;
            this.gbSeparators.Text = "Separators";
            // 
            // btnRemoveSeparator
            // 
            this.btnRemoveSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveSeparator.Location = new System.Drawing.Point(111, 183);
            this.btnRemoveSeparator.Name = "btnRemoveSeparator";
            this.btnRemoveSeparator.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveSeparator.TabIndex = 2;
            this.btnRemoveSeparator.Text = "Remove";
            this.btnRemoveSeparator.UseVisualStyleBackColor = true;
            this.btnRemoveSeparator.Click += new System.EventHandler(this.btnRemoveSeparator_Click);
            // 
            // btnAddSeparator
            // 
            this.btnAddSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddSeparator.Location = new System.Drawing.Point(7, 184);
            this.btnAddSeparator.Name = "btnAddSeparator";
            this.btnAddSeparator.Size = new System.Drawing.Size(75, 23);
            this.btnAddSeparator.TabIndex = 1;
            this.btnAddSeparator.Text = "Add...";
            this.btnAddSeparator.UseVisualStyleBackColor = true;
            this.btnAddSeparator.Click += new System.EventHandler(this.btnAddSeparator_Click);
            // 
            // lbSeparators
            // 
            this.lbSeparators.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSeparators.FormattingEnabled = true;
            this.lbSeparators.Location = new System.Drawing.Point(7, 20);
            this.lbSeparators.Name = "lbSeparators";
            this.lbSeparators.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbSeparators.Size = new System.Drawing.Size(179, 160);
            this.lbSeparators.TabIndex = 0;
            // 
            // btnDatabasePrompt
            // 
            this.btnDatabasePrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDatabasePrompt.Location = new System.Drawing.Point(445, 83);
            this.btnDatabasePrompt.Name = "btnDatabasePrompt";
            this.btnDatabasePrompt.Size = new System.Drawing.Size(32, 23);
            this.btnDatabasePrompt.TabIndex = 7;
            this.btnDatabasePrompt.Text = "...";
            this.btnDatabasePrompt.UseVisualStyleBackColor = true;
            this.btnDatabasePrompt.Click += new System.EventHandler(this.btnDatabasePrompt_Click);
            // 
            // gbStopWords
            // 
            this.gbStopWords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gbStopWords.Controls.Add(this.btnRemoveStopWord);
            this.gbStopWords.Controls.Add(this.btnAddStopWord);
            this.gbStopWords.Controls.Add(this.lbStopWords);
            this.gbStopWords.Location = new System.Drawing.Point(208, 112);
            this.gbStopWords.Name = "gbStopWords";
            this.gbStopWords.Size = new System.Drawing.Size(261, 213);
            this.gbStopWords.TabIndex = 2;
            this.gbStopWords.TabStop = false;
            this.gbStopWords.Text = "Stop Words";
            // 
            // btnRemoveStopWord
            // 
            this.btnRemoveStopWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveStopWord.Location = new System.Drawing.Point(180, 185);
            this.btnRemoveStopWord.Name = "btnRemoveStopWord";
            this.btnRemoveStopWord.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveStopWord.TabIndex = 2;
            this.btnRemoveStopWord.Text = "Remove";
            this.btnRemoveStopWord.UseVisualStyleBackColor = true;
            this.btnRemoveStopWord.Click += new System.EventHandler(this.btnRemoveStopWord_Click);
            // 
            // btnAddStopWord
            // 
            this.btnAddStopWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddStopWord.Location = new System.Drawing.Point(7, 184);
            this.btnAddStopWord.Name = "btnAddStopWord";
            this.btnAddStopWord.Size = new System.Drawing.Size(75, 23);
            this.btnAddStopWord.TabIndex = 1;
            this.btnAddStopWord.Text = "Add...";
            this.btnAddStopWord.UseVisualStyleBackColor = true;
            this.btnAddStopWord.Click += new System.EventHandler(this.btnAddStopWord_Click);
            // 
            // lbStopWords
            // 
            this.lbStopWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStopWords.FormattingEnabled = true;
            this.lbStopWords.Location = new System.Drawing.Point(7, 20);
            this.lbStopWords.Name = "lbStopWords";
            this.lbStopWords.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbStopWords.Size = new System.Drawing.Size(248, 160);
            this.lbStopWords.TabIndex = 0;
            // 
            // ddlProvider
            // 
            this.ddlProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlProvider.FormattingEnabled = true;
            this.ddlProvider.Items.AddRange(new object[] {
            "SQL Server 2008 or later",
            "MySQL 5.1 or later *",
            "PostgreSQL 8.3 or later *",
            "Oracle XE 10g or later *"});
            this.ddlProvider.Location = new System.Drawing.Point(11, 85);
            this.ddlProvider.Name = "ddlProvider";
            this.ddlProvider.Size = new System.Drawing.Size(159, 21);
            this.ddlProvider.TabIndex = 6;
            this.ddlProvider.SelectedIndexChanged += new System.EventHandler(this.ddlProvider_SelectedIndexChanged);
            // 
            // lblDatabaseProvider
            // 
            this.lblDatabaseProvider.AutoSize = true;
            this.lblDatabaseProvider.Location = new System.Drawing.Point(8, 69);
            this.lblDatabaseProvider.Name = "lblDatabaseProvider";
            this.lblDatabaseProvider.Size = new System.Drawing.Size(98, 13);
            this.lblDatabaseProvider.TabIndex = 5;
            this.lblDatabaseProvider.Text = "Database Provider:";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(176, 85);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(263, 20);
            this.txtConnectionString.TabIndex = 4;
            this.txtConnectionString.TextChanged += new System.EventHandler(this.txtConnectionString_TextChanged);
            // 
            // lblDatabaseConnectionString
            // 
            this.lblDatabaseConnectionString.AutoSize = true;
            this.lblDatabaseConnectionString.Location = new System.Drawing.Point(173, 69);
            this.lblDatabaseConnectionString.Name = "lblDatabaseConnectionString";
            this.lblDatabaseConnectionString.Size = new System.Drawing.Size(143, 13);
            this.lblDatabaseConnectionString.TabIndex = 3;
            this.lblDatabaseConnectionString.Text = "Database Connection String:";
            // 
            // txtTermCacheMinimum
            // 
            this.txtTermCacheMinimum.Location = new System.Drawing.Point(339, 37);
            this.txtTermCacheMinimum.Name = "txtTermCacheMinimum";
            this.txtTermCacheMinimum.Size = new System.Drawing.Size(100, 20);
            this.txtTermCacheMinimum.TabIndex = 2;
            this.txtTermCacheMinimum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTermCacheMinimum.TextChanged += new System.EventHandler(this.txtTermCacheMinimum_TextChanged);
            // 
            // lblTermCacheMinimum
            // 
            this.lblTermCacheMinimum.Location = new System.Drawing.Point(10, 40);
            this.lblTermCacheMinimum.Name = "lblTermCacheMinimum";
            this.lblTermCacheMinimum.Size = new System.Drawing.Size(323, 13);
            this.lblTermCacheMinimum.TabIndex = 1;
            this.lblTermCacheMinimum.Text = "Cache a term only if number of hits returned exceeds:";
            this.lblTermCacheMinimum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkTermCachingEnabled
            // 
            this.chkTermCachingEnabled.AutoSize = true;
            this.chkTermCachingEnabled.Location = new System.Drawing.Point(10, 20);
            this.chkTermCachingEnabled.Name = "chkTermCachingEnabled";
            this.chkTermCachingEnabled.Size = new System.Drawing.Size(400, 17);
            this.chkTermCachingEnabled.TabIndex = 0;
            this.chkTermCachingEnabled.Text = "Enable Term Caching  (\'Term\' is defined as any word for which a user searches)";
            this.chkTermCachingEnabled.UseVisualStyleBackColor = true;
            this.chkTermCachingEnabled.CheckedChanged += new System.EventHandler(this.chkTermCachingEnabled_CheckedChanged);
            // 
            // tpIndexes
            // 
            this.tpIndexes.Controls.Add(this.btnAddNew);
            this.tpIndexes.Controls.Add(this.lvIndexes);
            this.tpIndexes.Location = new System.Drawing.Point(4, 22);
            this.tpIndexes.Name = "tpIndexes";
            this.tpIndexes.Padding = new System.Windows.Forms.Padding(3);
            this.tpIndexes.Size = new System.Drawing.Size(494, 372);
            this.tpIndexes.TabIndex = 1;
            this.tpIndexes.Text = "Indexes";
            this.tpIndexes.UseVisualStyleBackColor = true;
            // 
            // btnAddNew
            // 
            this.btnAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNew.Location = new System.Drawing.Point(413, 343);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(75, 23);
            this.btnAddNew.TabIndex = 2;
            this.btnAddNew.Text = "&Add New...";
            this.btnAddNew.UseVisualStyleBackColor = true;
            // 
            // tpStatus
            // 
            this.tpStatus.Controls.Add(this.lblPendingRealtimeUpdates);
            this.tpStatus.Controls.Add(this.lblPendingRealtimeUpdatesCount);
            this.tpStatus.Controls.Add(this.lblProcessingIndexes);
            this.tpStatus.Controls.Add(this.lblPendingIndexCreations);
            this.tpStatus.Controls.Add(this.lblClientCount);
            this.tpStatus.Controls.Add(this.lblConnectedClients);
            this.tpStatus.Controls.Add(this.btnRefresh);
            this.tpStatus.Location = new System.Drawing.Point(4, 22);
            this.tpStatus.Name = "tpStatus";
            this.tpStatus.Size = new System.Drawing.Size(494, 372);
            this.tpStatus.TabIndex = 2;
            this.tpStatus.Text = "Status";
            this.tpStatus.UseVisualStyleBackColor = true;
            // 
            // tpLog
            // 
            this.tpLog.Controls.Add(this.btnRefreshLog);
            this.tpLog.Controls.Add(this.btnShowInTextEditor);
            this.tpLog.Controls.Add(this.txtMessage);
            this.tpLog.Controls.Add(this.lvLog);
            this.tpLog.Location = new System.Drawing.Point(4, 22);
            this.tpLog.Name = "tpLog";
            this.tpLog.Size = new System.Drawing.Size(494, 372);
            this.tpLog.TabIndex = 3;
            this.tpLog.Text = "Log";
            this.tpLog.UseVisualStyleBackColor = true;
            // 
            // btnRefreshLog
            // 
            this.btnRefreshLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshLog.Location = new System.Drawing.Point(18, 343);
            this.btnRefreshLog.Name = "btnRefreshLog";
            this.btnRefreshLog.Size = new System.Drawing.Size(81, 23);
            this.btnRefreshLog.TabIndex = 10;
            this.btnRefreshLog.Text = "&Refresh";
            this.btnRefreshLog.UseVisualStyleBackColor = true;
            this.btnRefreshLog.Click += new System.EventHandler(this.btnRefreshLog_Click);
            // 
            // btnShowInTextEditor
            // 
            this.btnShowInTextEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowInTextEditor.Location = new System.Drawing.Point(118, 343);
            this.btnShowInTextEditor.Name = "btnShowInTextEditor";
            this.btnShowInTextEditor.Size = new System.Drawing.Size(134, 23);
            this.btnShowInTextEditor.TabIndex = 6;
            this.btnShowInTextEditor.Text = "&Show in Text Editor...";
            this.btnShowInTextEditor.UseVisualStyleBackColor = true;
            this.btnShowInTextEditor.Click += new System.EventHandler(this.btnShowInTextEditor_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(3, 232);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMessage.Size = new System.Drawing.Size(488, 105);
            this.txtMessage.TabIndex = 5;
            this.txtMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyDown);
            // 
            // lvLog
            // 
            this.lvLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDate,
            this.colType,
            this.colMessage});
            this.lvLog.ContextMenuStrip = this.cmLog;
            this.lvLog.FullRowSelect = true;
            this.lvLog.HideSelection = false;
            this.lvLog.Location = new System.Drawing.Point(3, 3);
            this.lvLog.MultiSelect = false;
            this.lvLog.Name = "lvLog";
            this.lvLog.ShowItemToolTips = true;
            this.lvLog.Size = new System.Drawing.Size(488, 223);
            this.lvLog.TabIndex = 4;
            this.lvLog.UseCompatibleStateImageBehavior = false;
            this.lvLog.View = System.Windows.Forms.View.Details;
            this.lvLog.SelectedIndexChanged += new System.EventHandler(this.lvLog_SelectedIndexChanged);
            // 
            // colDate
            // 
            this.colDate.Text = "Date";
            this.colDate.Width = 160;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            this.colType.Width = 100;
            // 
            // colMessage
            // 
            this.colMessage.Text = "Message";
            this.colMessage.Width = 400;
            // 
            // cmLog
            // 
            this.cmLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiLogRefresh});
            this.cmLog.Name = "ctxMenuNodeUser";
            this.cmLog.Size = new System.Drawing.Size(114, 26);
            // 
            // cmiLogRefresh
            // 
            this.cmiLogRefresh.Name = "cmiLogRefresh";
            this.cmiLogRefresh.Size = new System.Drawing.Size(113, 22);
            this.cmiLogRefresh.Text = "&Refresh";
            this.cmiLogRefresh.Click += new System.EventHandler(this.refreshLogMenuItem_Click);
            // 
            // cmiIndexRebuildAll
            // 
            this.cmiIndexRebuildAll.Name = "cmiIndexRebuildAll";
            this.cmiIndexRebuildAll.Size = new System.Drawing.Size(152, 22);
            this.cmiIndexRebuildAll.Text = "Rebuild &All Enabled";
            this.cmiIndexRebuildAll.Click += new System.EventHandler(this.cmiIndexRebuildAll_Click);
            // 
            // frmSearchEngineIndexes
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(527, 423);
            this.Controls.Add(this.tcIndexes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSearchEngineIndexes";
            this.Text = "Search Engine";
            this.Load += new System.EventHandler(this.frmSearchEngineIndexes_Load);
            this.cmIndex.ResumeLayout(false);
            this.tcIndexes.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            this.gbGlobalSettings.ResumeLayout(false);
            this.gbGlobalSettings.PerformLayout();
            this.gbSeparators.ResumeLayout(false);
            this.gbStopWords.ResumeLayout(false);
            this.tpIndexes.ResumeLayout(false);
            this.tpStatus.ResumeLayout(false);
            this.tpLog.ResumeLayout(false);
            this.tpLog.PerformLayout();
            this.cmLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvIndexes;
        private System.Windows.Forms.ColumnHeader colIndexName;
        private System.Windows.Forms.ColumnHeader colEnabled;
        private System.Windows.Forms.ContextMenuStrip cmIndex;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexEnable;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexDisable;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexProperties;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ColumnHeader colResolvers;
        private System.Windows.Forms.Label lblPendingRealtimeUpdates;
        private System.Windows.Forms.Label lblPendingRealtimeUpdatesCount;
        private System.Windows.Forms.Label lblClientCount;
        private System.Windows.Forms.Label lblConnectedClients;
        private System.Windows.Forms.Label lblPendingIndexCreations;
        private System.Windows.Forms.Label lblProcessingIndexes;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexRebuild;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexDefragment;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ColumnHeader colValid;
        private System.Windows.Forms.TabControl tcIndexes;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpIndexes;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.TabPage tpStatus;
        private System.Windows.Forms.TabPage tpLog;
        private System.Windows.Forms.Button btnShowInTextEditor;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.ListView lvLog;
        private System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.ColumnHeader colLastRebuild;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gbGlobalSettings;
        private System.Windows.Forms.Label lblTermCacheMinimum;
        private System.Windows.Forms.CheckBox chkTermCachingEnabled;
        private System.Windows.Forms.TextBox txtTermCacheMinimum;
        private System.Windows.Forms.ComboBox ddlProvider;
        private System.Windows.Forms.Label lblDatabaseProvider;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label lblDatabaseConnectionString;
        private System.Windows.Forms.Button btnDatabasePrompt;
        private System.Windows.Forms.Label lblNotRegressionTested;
        private System.Windows.Forms.ContextMenuStrip cmLog;
        private System.Windows.Forms.ToolStripMenuItem cmiLogRefresh;
        private System.Windows.Forms.GroupBox gbStopWords;
        private System.Windows.Forms.ListBox lbStopWords;
        private System.Windows.Forms.Button btnRemoveStopWord;
        private System.Windows.Forms.Button btnAddStopWord;
        private System.Windows.Forms.GroupBox gbSeparators;
        private System.Windows.Forms.Button btnRemoveSeparator;
        private System.Windows.Forms.Button btnAddSeparator;
        private System.Windows.Forms.ListBox lbSeparators;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnRefreshLog;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexVerify;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexReload;
        private System.Windows.Forms.ToolStripMenuItem cmiIndexRebuildAll;
    }
}
