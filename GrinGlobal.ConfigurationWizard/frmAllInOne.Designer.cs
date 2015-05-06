namespace GrinGlobal.ConfigurationWizard {
    partial class frmAllInOne {
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Database Engine");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Database Tools");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Database");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Web Application");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Search Engine");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Server", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Curator Tool");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Client", new System.Windows.Forms.TreeNode[] {
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("GRIN-Global Application Suite", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Windows Update");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAllInOne));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stat1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statServerDatabaseEngine = new System.Windows.Forms.ToolStripStatusLabel();
            this.stat5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statClientDatabaseEngine = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tvOptions = new System.Windows.Forms.TreeView();
            this.ilTodo = new System.Windows.Forms.ImageList(this.components);
            this.lblPasswordRequired = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbActions = new System.Windows.Forms.ListBox();
            this.btnContinue = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblBody = new System.Windows.Forms.Label();
            this.gbPanel = new System.Windows.Forms.GroupBox();
            this.pnlDatabase = new System.Windows.Forms.Panel();
            this.chkUseWindowsLogon = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnTestLogin = new System.Windows.Forms.Button();
            this.lblDatabaseDescription = new System.Windows.Forms.Label();
            this.txtPasswordRepeat = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPasswordMismatch = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSuperUserPassword = new System.Windows.Forms.Label();
            this.pnlCuratorTool = new System.Windows.Forms.Panel();
            this.lblSqlServer = new System.Windows.Forms.Label();
            this.pnlRoot = new System.Windows.Forms.Panel();
            this.btnChooseDirectory = new System.Windows.Forms.Button();
            this.txtInstallDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlDatabaseEngine = new System.Windows.Forms.Panel();
            this.lblSqlServerInstanceName = new System.Windows.Forms.Label();
            this.txtSqlServerInstanceName = new System.Windows.Forms.TextBox();
            this.lblDatabaseEngine = new System.Windows.Forms.Label();
            this.rdoPostgreSQL = new System.Windows.Forms.RadioButton();
            this.rdoOracle = new System.Windows.Forms.RadioButton();
            this.rdoSqlServer = new System.Windows.Forms.RadioButton();
            this.rdoMySQL = new System.Windows.Forms.RadioButton();
            this.pnlDefault = new System.Windows.Forms.Panel();
            this.lblDefaultOptions = new System.Windows.Forms.Label();
            this.pnlWebApplication = new System.Windows.Forms.Panel();
            this.lblIIS = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.gbPanel.SuspendLayout();
            this.pnlDatabase.SuspendLayout();
            this.pnlCuratorTool.SuspendLayout();
            this.pnlRoot.SuspendLayout();
            this.pnlDatabaseEngine.SuspendLayout();
            this.pnlDefault.SuspendLayout();
            this.pnlWebApplication.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stat1,
            this.statServerDatabaseEngine,
            this.stat5,
            this.statClientDatabaseEngine});
            this.statusStrip1.Location = new System.Drawing.Point(0, 478);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(884, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stat1
            // 
            this.stat1.Name = "stat1";
            this.stat1.Size = new System.Drawing.Size(102, 17);
            this.stat1.Text = "Database Engines:";
            // 
            // statServerDatabaseEngine
            // 
            this.statServerDatabaseEngine.Name = "statServerDatabaseEngine";
            this.statServerDatabaseEngine.Size = new System.Drawing.Size(42, 17);
            this.statServerDatabaseEngine.Text = "Server:";
            // 
            // stat5
            // 
            this.stat5.Name = "stat5";
            this.stat5.Size = new System.Drawing.Size(10, 17);
            this.stat5.Text = "|";
            // 
            // statClientDatabaseEngine
            // 
            this.statClientDatabaseEngine.Name = "statClientDatabaseEngine";
            this.statClientDatabaseEngine.Size = new System.Drawing.Size(41, 17);
            this.statClientDatabaseEngine.Text = "Client:";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnContinue);
            this.splitContainer1.Panel2.Controls.Add(this.lblTitle);
            this.splitContainer1.Panel2.Controls.Add(this.lblBody);
            this.splitContainer1.Panel2.Controls.Add(this.gbPanel);
            this.splitContainer1.Size = new System.Drawing.Size(884, 478);
            this.splitContainer1.SplitterDistance = 326;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label6);
            this.splitContainer2.Panel1.Controls.Add(this.button1);
            this.splitContainer2.Panel1.Controls.Add(this.tvOptions);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lblPasswordRequired);
            this.splitContainer2.Panel2.Controls.Add(this.label4);
            this.splitContainer2.Panel2.Controls.Add(this.lbActions);
            this.splitContainer2.Size = new System.Drawing.Size(326, 478);
            this.splitContainer2.SplitterDistance = 242;
            this.splitContainer2.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Application Selection:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(147, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tvOptions
            // 
            this.tvOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvOptions.CheckBoxes = true;
            this.tvOptions.HideSelection = false;
            this.tvOptions.ImageIndex = 0;
            this.tvOptions.ImageList = this.ilTodo;
            this.tvOptions.Location = new System.Drawing.Point(0, 25);
            this.tvOptions.Name = "tvOptions";
            treeNode1.Checked = true;
            treeNode1.Name = "ndDatabaseEngine";
            treeNode1.Text = "Database Engine";
            treeNode2.Checked = true;
            treeNode2.Name = "ndDatabaseTool";
            treeNode2.Text = "Database Tools";
            treeNode3.Checked = true;
            treeNode3.Name = "ndDatabase";
            treeNode3.Text = "Database";
            treeNode4.Checked = true;
            treeNode4.Name = "ndWebApplication";
            treeNode4.Text = "Web Application";
            treeNode5.Checked = true;
            treeNode5.Name = "ndSearchEngine";
            treeNode5.Text = "Search Engine";
            treeNode6.Checked = true;
            treeNode6.ImageIndex = -2;
            treeNode6.Name = "ndServer";
            treeNode6.Text = "Server";
            treeNode7.Checked = true;
            treeNode7.Name = "ndCuratorTool";
            treeNode7.Text = "Curator Tool";
            treeNode8.Checked = true;
            treeNode8.ImageIndex = -2;
            treeNode8.Name = "ndClient";
            treeNode8.Text = "Client";
            treeNode9.Checked = true;
            treeNode9.ImageIndex = -2;
            treeNode9.Name = "ndRoot";
            treeNode9.Text = "GRIN-Global Application Suite";
            treeNode10.Checked = true;
            treeNode10.Name = "ndUpdateWindows";
            treeNode10.Text = "Windows Update";
            this.tvOptions.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode9,
            treeNode10});
            this.tvOptions.SelectedImageIndex = 0;
            this.tvOptions.Size = new System.Drawing.Size(323, 214);
            this.tvOptions.TabIndex = 1;
            this.tvOptions.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvOptions_AfterCheck);
            this.tvOptions.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvOptions_AfterSelect);
            // 
            // ilTodo
            // 
            this.ilTodo.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTodo.ImageStream")));
            this.ilTodo.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTodo.Images.SetKeyName(0, "blank.gif");
            this.ilTodo.Images.SetKeyName(1, "no_action.gif");
            this.ilTodo.Images.SetKeyName(2, "install.gif");
            this.ilTodo.Images.SetKeyName(3, "remove.gif");
            this.ilTodo.Images.SetKeyName(4, "mixed_action.gif");
            // 
            // lblPasswordRequired
            // 
            this.lblPasswordRequired.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPasswordRequired.Location = new System.Drawing.Point(3, 191);
            this.lblPasswordRequired.Name = "lblPasswordRequired";
            this.lblPasswordRequired.Size = new System.Drawing.Size(320, 33);
            this.lblPasswordRequired.TabIndex = 5;
            this.lblPasswordRequired.Text = "* Requires database engine\'s super user password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Actions to Perform:";
            // 
            // lbActions
            // 
            this.lbActions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbActions.FormattingEnabled = true;
            this.lbActions.Location = new System.Drawing.Point(3, 21);
            this.lbActions.Name = "lbActions";
            this.lbActions.Size = new System.Drawing.Size(320, 160);
            this.lbActions.TabIndex = 1;
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinue.Location = new System.Drawing.Point(412, 447);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(130, 23);
            this.btnContinue.TabIndex = 3;
            this.btnContinue.Text = "Verify and Proceed...";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Location = new System.Drawing.Point(1, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(550, 22);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Title";
            // 
            // lblBody
            // 
            this.lblBody.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBody.Location = new System.Drawing.Point(1, 42);
            this.lblBody.Name = "lblBody";
            this.lblBody.Size = new System.Drawing.Size(550, 44);
            this.lblBody.TabIndex = 0;
            this.lblBody.Text = "Body";
            // 
            // gbPanel
            // 
            this.gbPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbPanel.Controls.Add(this.pnlDatabase);
            this.gbPanel.Controls.Add(this.pnlCuratorTool);
            this.gbPanel.Controls.Add(this.pnlRoot);
            this.gbPanel.Controls.Add(this.pnlDatabaseEngine);
            this.gbPanel.Controls.Add(this.pnlDefault);
            this.gbPanel.Controls.Add(this.pnlWebApplication);
            this.gbPanel.Location = new System.Drawing.Point(4, 89);
            this.gbPanel.Name = "gbPanel";
            this.gbPanel.Size = new System.Drawing.Size(538, 352);
            this.gbPanel.TabIndex = 2;
            this.gbPanel.TabStop = false;
            this.gbPanel.Text = "Additional Information";
            // 
            // pnlDatabase
            // 
            this.pnlDatabase.Controls.Add(this.chkUseWindowsLogon);
            this.pnlDatabase.Controls.Add(this.label2);
            this.pnlDatabase.Controls.Add(this.btnTestLogin);
            this.pnlDatabase.Controls.Add(this.lblDatabaseDescription);
            this.pnlDatabase.Controls.Add(this.txtPasswordRepeat);
            this.pnlDatabase.Controls.Add(this.txtPassword);
            this.pnlDatabase.Controls.Add(this.lblPasswordMismatch);
            this.pnlDatabase.Controls.Add(this.label3);
            this.pnlDatabase.Controls.Add(this.lblSuperUserPassword);
            this.pnlDatabase.Location = new System.Drawing.Point(227, 20);
            this.pnlDatabase.Name = "pnlDatabase";
            this.pnlDatabase.Size = new System.Drawing.Size(372, 279);
            this.pnlDatabase.TabIndex = 4;
            // 
            // chkUseWindowsLogon
            // 
            this.chkUseWindowsLogon.AutoSize = true;
            this.chkUseWindowsLogon.Checked = true;
            this.chkUseWindowsLogon.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseWindowsLogon.Location = new System.Drawing.Point(139, 99);
            this.chkUseWindowsLogon.Name = "chkUseWindowsLogon";
            this.chkUseWindowsLogon.Size = new System.Drawing.Size(163, 17);
            this.chkUseWindowsLogon.TabIndex = 17;
            this.chkUseWindowsLogon.Text = "Use Windows Authentication";
            this.chkUseWindowsLogon.UseVisualStyleBackColor = true;
            this.chkUseWindowsLogon.CheckedChanged += new System.EventHandler(this.chkUseWindowsLogon_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(35, 223);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(305, 36);
            this.label2.TabIndex = 16;
            this.label2.Text = "Passwords must be at least 4 characters in length and cannot contain a double quo" +
                "te (\")";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // btnTestLogin
            // 
            this.btnTestLogin.Enabled = false;
            this.btnTestLogin.Location = new System.Drawing.Point(141, 194);
            this.btnTestLogin.Name = "btnTestLogin";
            this.btnTestLogin.Size = new System.Drawing.Size(75, 23);
            this.btnTestLogin.TabIndex = 15;
            this.btnTestLogin.Text = "&Test Login";
            this.btnTestLogin.UseVisualStyleBackColor = true;
            this.btnTestLogin.Click += new System.EventHandler(this.btnTestLogin_Click);
            // 
            // lblDatabaseDescription
            // 
            this.lblDatabaseDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDatabaseDescription.Location = new System.Drawing.Point(32, 15);
            this.lblDatabaseDescription.Name = "lblDatabaseDescription";
            this.lblDatabaseDescription.Size = new System.Drawing.Size(308, 79);
            this.lblDatabaseDescription.TabIndex = 13;
            this.lblDatabaseDescription.Text = resources.GetString("lblDatabaseDescription.Text");
            // 
            // txtPasswordRepeat
            // 
            this.txtPasswordRepeat.Enabled = false;
            this.txtPasswordRepeat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPasswordRepeat.Location = new System.Drawing.Point(141, 148);
            this.txtPasswordRepeat.Name = "txtPasswordRepeat";
            this.txtPasswordRepeat.PasswordChar = '*';
            this.txtPasswordRepeat.Size = new System.Drawing.Size(146, 22);
            this.txtPasswordRepeat.TabIndex = 12;
            this.txtPasswordRepeat.TextChanged += new System.EventHandler(this.txtPasswordRepeat_TextChanged);
            this.txtPasswordRepeat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPasswordRepeat_KeyPress);
            this.txtPasswordRepeat.Enter += new System.EventHandler(this.txtPasswordRepeat_Enter);
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(141, 122);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(146, 22);
            this.txtPassword.TabIndex = 11;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            // 
            // lblPasswordMismatch
            // 
            this.lblPasswordMismatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPasswordMismatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPasswordMismatch.ForeColor = System.Drawing.Color.Black;
            this.lblPasswordMismatch.Location = new System.Drawing.Point(38, 173);
            this.lblPasswordMismatch.Name = "lblPasswordMismatch";
            this.lblPasswordMismatch.Size = new System.Drawing.Size(280, 18);
            this.lblPasswordMismatch.TabIndex = 10;
            this.lblPasswordMismatch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPasswordMismatch.Click += new System.EventHandler(this.lblPasswordMismatch_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Repeat password:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // lblSuperUserPassword
            // 
            this.lblSuperUserPassword.Location = new System.Drawing.Point(6, 125);
            this.lblSuperUserPassword.Name = "lblSuperUserPassword";
            this.lblSuperUserPassword.Size = new System.Drawing.Size(131, 20);
            this.lblSuperUserPassword.TabIndex = 8;
            this.lblSuperUserPassword.Text = "Administrator password:";
            this.lblSuperUserPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSuperUserPassword.Click += new System.EventHandler(this.lblSuperUserPassword_Click);
            // 
            // pnlCuratorTool
            // 
            this.pnlCuratorTool.Controls.Add(this.lblSqlServer);
            this.pnlCuratorTool.Location = new System.Drawing.Point(214, 69);
            this.pnlCuratorTool.Name = "pnlCuratorTool";
            this.pnlCuratorTool.Size = new System.Drawing.Size(200, 100);
            this.pnlCuratorTool.TabIndex = 3;
            // 
            // lblSqlServer
            // 
            this.lblSqlServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSqlServer.Location = new System.Drawing.Point(9, 22);
            this.lblSqlServer.Name = "lblSqlServer";
            this.lblSqlServer.Size = new System.Drawing.Size(182, 57);
            this.lblSqlServer.TabIndex = 1;
            this.lblSqlServer.Text = "-";
            // 
            // pnlRoot
            // 
            this.pnlRoot.Controls.Add(this.btnChooseDirectory);
            this.pnlRoot.Controls.Add(this.txtInstallDir);
            this.pnlRoot.Controls.Add(this.label1);
            this.pnlRoot.Location = new System.Drawing.Point(19, 271);
            this.pnlRoot.Name = "pnlRoot";
            this.pnlRoot.Size = new System.Drawing.Size(334, 100);
            this.pnlRoot.TabIndex = 7;
            // 
            // btnChooseDirectory
            // 
            this.btnChooseDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChooseDirectory.Enabled = false;
            this.btnChooseDirectory.Location = new System.Drawing.Point(299, 34);
            this.btnChooseDirectory.Name = "btnChooseDirectory";
            this.btnChooseDirectory.Size = new System.Drawing.Size(30, 23);
            this.btnChooseDirectory.TabIndex = 2;
            this.btnChooseDirectory.Text = "...";
            this.btnChooseDirectory.UseVisualStyleBackColor = true;
            // 
            // txtInstallDir
            // 
            this.txtInstallDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInstallDir.Enabled = false;
            this.txtInstallDir.Location = new System.Drawing.Point(14, 34);
            this.txtInstallDir.Name = "txtInstallDir";
            this.txtInstallDir.Size = new System.Drawing.Size(279, 20);
            this.txtInstallDir.TabIndex = 1;
            this.txtInstallDir.Text = "C:\\Program Files\\GRIN-Global\\";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Install To Directory:";
            // 
            // pnlDatabaseEngine
            // 
            this.pnlDatabaseEngine.Controls.Add(this.lblSqlServerInstanceName);
            this.pnlDatabaseEngine.Controls.Add(this.txtSqlServerInstanceName);
            this.pnlDatabaseEngine.Controls.Add(this.lblDatabaseEngine);
            this.pnlDatabaseEngine.Controls.Add(this.rdoPostgreSQL);
            this.pnlDatabaseEngine.Controls.Add(this.rdoOracle);
            this.pnlDatabaseEngine.Controls.Add(this.rdoSqlServer);
            this.pnlDatabaseEngine.Controls.Add(this.rdoMySQL);
            this.pnlDatabaseEngine.Location = new System.Drawing.Point(6, 19);
            this.pnlDatabaseEngine.Name = "pnlDatabaseEngine";
            this.pnlDatabaseEngine.Size = new System.Drawing.Size(292, 222);
            this.pnlDatabaseEngine.TabIndex = 0;
            // 
            // lblSqlServerInstanceName
            // 
            this.lblSqlServerInstanceName.AutoSize = true;
            this.lblSqlServerInstanceName.Enabled = false;
            this.lblSqlServerInstanceName.Location = new System.Drawing.Point(38, 63);
            this.lblSqlServerInstanceName.Name = "lblSqlServerInstanceName";
            this.lblSqlServerInstanceName.Size = new System.Drawing.Size(82, 13);
            this.lblSqlServerInstanceName.TabIndex = 12;
            this.lblSqlServerInstanceName.Text = "Instance Name:";
            // 
            // txtSqlServerInstanceName
            // 
            this.txtSqlServerInstanceName.Enabled = false;
            this.txtSqlServerInstanceName.Location = new System.Drawing.Point(126, 60);
            this.txtSqlServerInstanceName.Name = "txtSqlServerInstanceName";
            this.txtSqlServerInstanceName.Size = new System.Drawing.Size(100, 20);
            this.txtSqlServerInstanceName.TabIndex = 11;
            this.txtSqlServerInstanceName.Text = "SQLEXPRESS";
            // 
            // lblDatabaseEngine
            // 
            this.lblDatabaseEngine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDatabaseEngine.Location = new System.Drawing.Point(10, 137);
            this.lblDatabaseEngine.Name = "lblDatabaseEngine";
            this.lblDatabaseEngine.Size = new System.Drawing.Size(269, 37);
            this.lblDatabaseEngine.TabIndex = 10;
            this.lblDatabaseEngine.Text = "- SQL Server Express 2008 will be installed";
            // 
            // rdoPostgreSQL
            // 
            this.rdoPostgreSQL.AutoSize = true;
            this.rdoPostgreSQL.Enabled = false;
            this.rdoPostgreSQL.Location = new System.Drawing.Point(14, 19);
            this.rdoPostgreSQL.Name = "rdoPostgreSQL";
            this.rdoPostgreSQL.Size = new System.Drawing.Size(100, 17);
            this.rdoPostgreSQL.TabIndex = 0;
            this.rdoPostgreSQL.Text = "PostgreSQL 8.3";
            this.rdoPostgreSQL.UseVisualStyleBackColor = true;
            this.rdoPostgreSQL.CheckedChanged += new System.EventHandler(this.rdoPostgreSQL_CheckedChanged);
            // 
            // rdoOracle
            // 
            this.rdoOracle.AutoSize = true;
            this.rdoOracle.Enabled = false;
            this.rdoOracle.Location = new System.Drawing.Point(13, 89);
            this.rdoOracle.Name = "rdoOracle";
            this.rdoOracle.Size = new System.Drawing.Size(117, 17);
            this.rdoOracle.TabIndex = 2;
            this.rdoOracle.Text = "Oracle 10g Express";
            this.rdoOracle.UseVisualStyleBackColor = true;
            this.rdoOracle.CheckedChanged += new System.EventHandler(this.rdoOracle_CheckedChanged);
            // 
            // rdoSqlServer
            // 
            this.rdoSqlServer.AutoSize = true;
            this.rdoSqlServer.Checked = true;
            this.rdoSqlServer.Location = new System.Drawing.Point(14, 42);
            this.rdoSqlServer.Name = "rdoSqlServer";
            this.rdoSqlServer.Size = new System.Drawing.Size(147, 17);
            this.rdoSqlServer.TabIndex = 1;
            this.rdoSqlServer.TabStop = true;
            this.rdoSqlServer.Text = "SQL Server 2008 Express";
            this.rdoSqlServer.UseVisualStyleBackColor = true;
            this.rdoSqlServer.CheckedChanged += new System.EventHandler(this.rdoSqlServer_CheckedChanged);
            // 
            // rdoMySQL
            // 
            this.rdoMySQL.AutoSize = true;
            this.rdoMySQL.Location = new System.Drawing.Point(13, 112);
            this.rdoMySQL.Name = "rdoMySQL";
            this.rdoMySQL.Size = new System.Drawing.Size(78, 17);
            this.rdoMySQL.TabIndex = 3;
            this.rdoMySQL.Text = "MySQL 5.1";
            this.rdoMySQL.UseVisualStyleBackColor = true;
            this.rdoMySQL.CheckedChanged += new System.EventHandler(this.rdoMySQL_CheckedChanged);
            // 
            // pnlDefault
            // 
            this.pnlDefault.Controls.Add(this.lblDefaultOptions);
            this.pnlDefault.Location = new System.Drawing.Point(368, 247);
            this.pnlDefault.Name = "pnlDefault";
            this.pnlDefault.Size = new System.Drawing.Size(170, 72);
            this.pnlDefault.TabIndex = 2;
            // 
            // lblDefaultOptions
            // 
            this.lblDefaultOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDefaultOptions.Location = new System.Drawing.Point(14, 18);
            this.lblDefaultOptions.Name = "lblDefaultOptions";
            this.lblDefaultOptions.Size = new System.Drawing.Size(142, 42);
            this.lblDefaultOptions.TabIndex = 0;
            this.lblDefaultOptions.Text = "(No options)";
            // 
            // pnlWebApplication
            // 
            this.pnlWebApplication.Controls.Add(this.lblIIS);
            this.pnlWebApplication.Location = new System.Drawing.Point(191, 248);
            this.pnlWebApplication.Name = "pnlWebApplication";
            this.pnlWebApplication.Size = new System.Drawing.Size(144, 76);
            this.pnlWebApplication.TabIndex = 1;
            // 
            // lblIIS
            // 
            this.lblIIS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIIS.Location = new System.Drawing.Point(14, 18);
            this.lblIIS.Name = "lblIIS";
            this.lblIIS.Size = new System.Drawing.Size(114, 31);
            this.lblIIS.TabIndex = 0;
            this.lblIIS.Text = "TODO: Check if IIS is installed";
            // 
            // frmAllInOne
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 500);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.KeyPreview = true;
            this.Name = "frmAllInOne";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GRIN-Global Configuration Wizard";
            this.Load += new System.EventHandler(this.frmAllInOne_Load);
            this.Activated += new System.EventHandler(this.frmAllInOne_Activated);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmAllInOne_KeyPress);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.gbPanel.ResumeLayout(false);
            this.pnlDatabase.ResumeLayout(false);
            this.pnlDatabase.PerformLayout();
            this.pnlCuratorTool.ResumeLayout(false);
            this.pnlRoot.ResumeLayout(false);
            this.pnlRoot.PerformLayout();
            this.pnlDatabaseEngine.ResumeLayout(false);
            this.pnlDatabaseEngine.PerformLayout();
            this.pnlDefault.ResumeLayout(false);
            this.pnlWebApplication.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblBody;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox gbPanel;
        private System.Windows.Forms.Panel pnlDatabaseEngine;
        private System.Windows.Forms.RadioButton rdoOracle;
        private System.Windows.Forms.RadioButton rdoSqlServer;
        private System.Windows.Forms.RadioButton rdoMySQL;
        private System.Windows.Forms.Panel pnlDefault;
        private System.Windows.Forms.Label lblDefaultOptions;
        private System.Windows.Forms.Panel pnlWebApplication;
        private System.Windows.Forms.Label lblIIS;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView tvOptions;
        private System.Windows.Forms.ListBox lbActions;
        private System.Windows.Forms.Panel pnlCuratorTool;
        private System.Windows.Forms.Panel pnlDatabase;
        private System.Windows.Forms.Label lblDatabaseDescription;
        private System.Windows.Forms.TextBox txtPasswordRepeat;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPasswordMismatch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSuperUserPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.RadioButton rdoPostgreSQL;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Button btnTestLogin;
        private System.Windows.Forms.Panel pnlRoot;
        private System.Windows.Forms.Button btnChooseDirectory;
        public System.Windows.Forms.TextBox txtInstallDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDatabaseEngine;
        private System.Windows.Forms.Label lblSqlServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblPasswordRequired;
        private System.Windows.Forms.ToolStripStatusLabel stat1;
        private System.Windows.Forms.ToolStripStatusLabel statServerDatabaseEngine;
        private System.Windows.Forms.ToolStripStatusLabel stat5;
        private System.Windows.Forms.ToolStripStatusLabel statClientDatabaseEngine;
        private System.Windows.Forms.CheckBox chkUseWindowsLogon;
        private System.Windows.Forms.ImageList ilTodo;
        private System.Windows.Forms.Label lblSqlServerInstanceName;
        private System.Windows.Forms.TextBox txtSqlServerInstanceName;
    }
}