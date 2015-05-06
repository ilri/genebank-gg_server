namespace GrinGlobal.Search.Engine.AdminTool {
	partial class Form1 {
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
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
			this.lbKeywords = new System.Windows.Forms.ListBox();
			this.lvHits = new System.Windows.Forms.ListView();
			this.chPrimaryKeyID = new System.Windows.Forms.ColumnHeader();
			this.chFieldOffset = new System.Windows.Forms.ColumnHeader();
			this.chFieldName = new System.Windows.Forms.ColumnHeader();
			this.chWordOffset = new System.Windows.Forms.ColumnHeader();
			this.cbIndexes = new System.Windows.Forms.ComboBox();
			this.lbAccessions = new System.Windows.Forms.ListBox();
			this.lblAccessions = new System.Windows.Forms.Label();
			this.lblInventory = new System.Windows.Forms.Label();
			this.lbInventory = new System.Windows.Forms.ListBox();
			this.lblOrders = new System.Windows.Forms.Label();
			this.lbOrders = new System.Windows.Forms.ListBox();
			this.lblHits = new System.Windows.Forms.Label();
			this.lblKeywords = new System.Windows.Forms.Label();
			this.btnSearch = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.createIndexesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.loadIndexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.verifyIndexesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.btnTestCreateTree = new System.Windows.Forms.Button();
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusProgress,
            this.statusText});
			this.statusStrip1.Location = new System.Drawing.Point(0, 437);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(676, 22);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusProgress
			// 
			this.statusProgress.Name = "statusProgress";
			this.statusProgress.Size = new System.Drawing.Size(100, 16);
			// 
			// statusText
			// 
			this.statusText.Name = "statusText";
			this.statusText.Size = new System.Drawing.Size(559, 17);
			this.statusText.Spring = true;
			this.statusText.Text = "-";
			// 
			// lbKeywords
			// 
			this.lbKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lbKeywords.FormattingEnabled = true;
			this.lbKeywords.Location = new System.Drawing.Point(12, 76);
			this.lbKeywords.Name = "lbKeywords";
			this.lbKeywords.Size = new System.Drawing.Size(240, 355);
			this.lbKeywords.TabIndex = 4;
			this.lbKeywords.SelectedIndexChanged += new System.EventHandler(this.lbKeywords_SelectedIndexChanged);
			// 
			// lvHits
			// 
			this.lvHits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvHits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPrimaryKeyID,
            this.chFieldOffset,
            this.chFieldName,
            this.chWordOffset});
			this.lvHits.FullRowSelect = true;
			this.lvHits.Location = new System.Drawing.Point(258, 76);
			this.lvHits.Name = "lvHits";
			this.lvHits.Size = new System.Drawing.Size(406, 149);
			this.lvHits.TabIndex = 5;
			this.lvHits.UseCompatibleStateImageBehavior = false;
			this.lvHits.View = System.Windows.Forms.View.Details;
			this.lvHits.SelectedIndexChanged += new System.EventHandler(this.lvHits_SelectedIndexChanged);
			// 
			// chPrimaryKeyID
			// 
			this.chPrimaryKeyID.Text = "PrimaryKey ID";
			this.chPrimaryKeyID.Width = 80;
			// 
			// chFieldOffset
			// 
			this.chFieldOffset.Text = "FieldOffset";
			this.chFieldOffset.Width = 70;
			// 
			// chFieldName
			// 
			this.chFieldName.Text = "Field Name";
			this.chFieldName.Width = 140;
			// 
			// chWordOffset
			// 
			this.chWordOffset.Text = "Word Offset";
			this.chWordOffset.Width = 70;
			// 
			// cbIndexes
			// 
			this.cbIndexes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbIndexes.FormattingEnabled = true;
			this.cbIndexes.Location = new System.Drawing.Point(15, 27);
			this.cbIndexes.Name = "cbIndexes";
			this.cbIndexes.Size = new System.Drawing.Size(137, 21);
			this.cbIndexes.TabIndex = 6;
			this.cbIndexes.SelectedIndexChanged += new System.EventHandler(this.cbIndexes_SelectedIndexChanged);
			// 
			// lbAccessions
			// 
			this.lbAccessions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lbAccessions.FormattingEnabled = true;
			this.lbAccessions.Location = new System.Drawing.Point(258, 258);
			this.lbAccessions.Name = "lbAccessions";
			this.lbAccessions.Size = new System.Drawing.Size(137, 173);
			this.lbAccessions.TabIndex = 9;
			// 
			// lblAccessions
			// 
			this.lblAccessions.Location = new System.Drawing.Point(258, 228);
			this.lblAccessions.Name = "lblAccessions";
			this.lblAccessions.Size = new System.Drawing.Size(134, 27);
			this.lblAccessions.TabIndex = 10;
			this.lblAccessions.Text = "Accessions";
			this.lblAccessions.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// lblInventory
			// 
			this.lblInventory.Location = new System.Drawing.Point(398, 228);
			this.lblInventory.Name = "lblInventory";
			this.lblInventory.Size = new System.Drawing.Size(116, 27);
			this.lblInventory.TabIndex = 12;
			this.lblInventory.Text = "Inventory";
			this.lblInventory.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// lbInventory
			// 
			this.lbInventory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lbInventory.FormattingEnabled = true;
			this.lbInventory.Location = new System.Drawing.Point(401, 258);
			this.lbInventory.Name = "lbInventory";
			this.lbInventory.Size = new System.Drawing.Size(116, 173);
			this.lbInventory.TabIndex = 11;
			// 
			// lblOrders
			// 
			this.lblOrders.Location = new System.Drawing.Point(520, 228);
			this.lblOrders.Name = "lblOrders";
			this.lblOrders.Size = new System.Drawing.Size(132, 27);
			this.lblOrders.TabIndex = 14;
			this.lblOrders.Text = "Orders";
			this.lblOrders.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// lbOrders
			// 
			this.lbOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbOrders.FormattingEnabled = true;
			this.lbOrders.Location = new System.Drawing.Point(523, 258);
			this.lbOrders.Name = "lbOrders";
			this.lbOrders.Size = new System.Drawing.Size(141, 173);
			this.lbOrders.TabIndex = 13;
			// 
			// lblHits
			// 
			this.lblHits.AutoSize = true;
			this.lblHits.Location = new System.Drawing.Point(258, 60);
			this.lblHits.Name = "lblHits";
			this.lblHits.Size = new System.Drawing.Size(25, 13);
			this.lblHits.TabIndex = 15;
			this.lblHits.Text = "Hits";
			// 
			// lblKeywords
			// 
			this.lblKeywords.AutoSize = true;
			this.lblKeywords.Location = new System.Drawing.Point(12, 60);
			this.lblKeywords.Name = "lblKeywords";
			this.lblKeywords.Size = new System.Drawing.Size(53, 13);
			this.lblKeywords.TabIndex = 17;
			this.lblKeywords.Text = "Keywords";
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Location = new System.Drawing.Point(589, 27);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(75, 23);
			this.btnSearch.TabIndex = 18;
			this.btnSearch.Text = "Search...";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(676, 24);
			this.menuStrip1.TabIndex = 19;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.createIndexesToolStripMenuItem,
            this.loadIndexToolStripMenuItem,
            this.verifyIndexesMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
			this.fileToolStripMenuItem.Text = "&Indexes";
			this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 22);
			this.toolStripMenuItem2.Text = "Load &from Config...";
			this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
			// 
			// createIndexesToolStripMenuItem
			// 
			this.createIndexesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allToolStripMenuItem1});
			this.createIndexesToolStripMenuItem.Name = "createIndexesToolStripMenuItem";
			this.createIndexesToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
			this.createIndexesToolStripMenuItem.Text = "&Create";
			this.createIndexesToolStripMenuItem.Click += new System.EventHandler(this.createIndexesToolStripMenuItem_Click);
			// 
			// allToolStripMenuItem1
			// 
			this.allToolStripMenuItem1.Name = "allToolStripMenuItem1";
			this.allToolStripMenuItem1.Size = new System.Drawing.Size(96, 22);
			this.allToolStripMenuItem1.Text = "(All)";
			this.allToolStripMenuItem1.Click += new System.EventHandler(this.allToolStripMenuItem1_Click);
			// 
			// loadIndexToolStripMenuItem
			// 
			this.loadIndexToolStripMenuItem.Name = "loadIndexToolStripMenuItem";
			this.loadIndexToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
			this.loadIndexToolStripMenuItem.Text = "&Load";
			this.loadIndexToolStripMenuItem.Click += new System.EventHandler(this.loadIndexToolStripMenuItem_Click);
			// 
			// verifyIndexesMenuItem
			// 
			this.verifyIndexesMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allToolStripMenuItem});
			this.verifyIndexesMenuItem.Name = "verifyIndexesMenuItem";
			this.verifyIndexesMenuItem.Size = new System.Drawing.Size(177, 22);
			this.verifyIndexesMenuItem.Text = "&Verify";
			this.verifyIndexesMenuItem.Click += new System.EventHandler(this.verifyIndexesMenuItem_Click);
			// 
			// allToolStripMenuItem
			// 
			this.allToolStripMenuItem.Name = "allToolStripMenuItem";
			this.allToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
			this.allToolStripMenuItem.Text = "(All)";
			this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// btnTestCreateTree
			// 
			this.btnTestCreateTree.Location = new System.Drawing.Point(391, 26);
			this.btnTestCreateTree.Name = "btnTestCreateTree";
			this.btnTestCreateTree.Size = new System.Drawing.Size(112, 23);
			this.btnTestCreateTree.TabIndex = 20;
			this.btnTestCreateTree.Text = "Test Create Tree";
			this.btnTestCreateTree.UseVisualStyleBackColor = true;
			this.btnTestCreateTree.Click += new System.EventHandler(this.btnTestCreateTree_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(676, 459);
			this.Controls.Add(this.btnTestCreateTree);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.lblKeywords);
			this.Controls.Add(this.lblHits);
			this.Controls.Add(this.lblOrders);
			this.Controls.Add(this.lbOrders);
			this.Controls.Add(this.lblInventory);
			this.Controls.Add(this.lbInventory);
			this.Controls.Add(this.lblAccessions);
			this.Controls.Add(this.lbAccessions);
			this.Controls.Add(this.cbIndexes);
			this.Controls.Add(this.lvHits);
			this.Controls.Add(this.lbKeywords);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "GRIN-Global - Search Engine Admin Tool";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripProgressBar statusProgress;
		private System.Windows.Forms.ToolStripStatusLabel statusText;
		private System.Windows.Forms.ListBox lbKeywords;
		private System.Windows.Forms.ListView lvHits;
		private System.Windows.Forms.ComboBox cbIndexes;
		private System.Windows.Forms.ColumnHeader chPrimaryKeyID;
		private System.Windows.Forms.ColumnHeader chFieldOffset;
		private System.Windows.Forms.ColumnHeader chFieldName;
		private System.Windows.Forms.ColumnHeader chWordOffset;
		private System.Windows.Forms.ListBox lbAccessions;
		private System.Windows.Forms.Label lblAccessions;
		private System.Windows.Forms.Label lblInventory;
		private System.Windows.Forms.ListBox lbInventory;
		private System.Windows.Forms.Label lblOrders;
		private System.Windows.Forms.ListBox lbOrders;
		private System.Windows.Forms.Label lblHits;
		private System.Windows.Forms.Label lblKeywords;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem createIndexesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem verifyIndexesMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ToolStripMenuItem loadIndexToolStripMenuItem;
		private System.Windows.Forms.Button btnTestCreateTree;
	}
}

