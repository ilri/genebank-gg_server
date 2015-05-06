namespace GrinGlobal.Search.Engine.AdminTool {
	partial class frmSearch {
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
			this.txtAccessions = new System.Windows.Forms.TextBox();
			this.lblAccessions = new System.Windows.Forms.Label();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lblInventory = new System.Windows.Forms.Label();
			this.txtInventory = new System.Windows.Forms.TextBox();
			this.lblOrders = new System.Windows.Forms.Label();
			this.txtOrders = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.clbIndexes = new System.Windows.Forms.CheckedListBox();
			this.lblAccessionsTime = new System.Windows.Forms.Label();
			this.lblInventoryTime = new System.Windows.Forms.Label();
			this.lblOrdersTime = new System.Windows.Forms.Label();
			this.chkIgnoreCase = new System.Windows.Forms.CheckBox();
			this.chkMatchAll = new System.Windows.Forms.CheckBox();
			this.chkCheckAll = new System.Windows.Forms.CheckBox();
			this.chkUseService = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// txtAccessions
			// 
			this.txtAccessions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.txtAccessions.Location = new System.Drawing.Point(12, 225);
			this.txtAccessions.Multiline = true;
			this.txtAccessions.Name = "txtAccessions";
			this.txtAccessions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtAccessions.Size = new System.Drawing.Size(169, 236);
			this.txtAccessions.TabIndex = 0;
			this.txtAccessions.TabStop = false;
			// 
			// lblAccessions
			// 
			this.lblAccessions.AutoSize = true;
			this.lblAccessions.Location = new System.Drawing.Point(12, 209);
			this.lblAccessions.Name = "lblAccessions";
			this.lblAccessions.Size = new System.Drawing.Size(61, 13);
			this.lblAccessions.TabIndex = 1;
			this.lblAccessions.Text = "Accessions";
			// 
			// txtSearch
			// 
			this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSearch.Location = new System.Drawing.Point(76, 13);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(409, 20);
			this.txtSearch.TabIndex = 2;
			// 
			// btnGo
			// 
			this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGo.Location = new System.Drawing.Point(491, 13);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(43, 23);
			this.btnGo.TabIndex = 3;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Search For:";
			// 
			// lblInventory
			// 
			this.lblInventory.AutoSize = true;
			this.lblInventory.Location = new System.Drawing.Point(184, 209);
			this.lblInventory.Name = "lblInventory";
			this.lblInventory.Size = new System.Drawing.Size(51, 13);
			this.lblInventory.TabIndex = 6;
			this.lblInventory.Text = "Inventory";
			// 
			// txtInventory
			// 
			this.txtInventory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.txtInventory.Location = new System.Drawing.Point(187, 225);
			this.txtInventory.Multiline = true;
			this.txtInventory.Name = "txtInventory";
			this.txtInventory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtInventory.Size = new System.Drawing.Size(175, 236);
			this.txtInventory.TabIndex = 5;
			this.txtInventory.TabStop = false;
			// 
			// lblOrders
			// 
			this.lblOrders.AutoSize = true;
			this.lblOrders.Location = new System.Drawing.Point(365, 209);
			this.lblOrders.Name = "lblOrders";
			this.lblOrders.Size = new System.Drawing.Size(38, 13);
			this.lblOrders.TabIndex = 8;
			this.lblOrders.Text = "Orders";
			// 
			// txtOrders
			// 
			this.txtOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOrders.Location = new System.Drawing.Point(368, 225);
			this.txtOrders.Multiline = true;
			this.txtOrders.Name = "txtOrders";
			this.txtOrders.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtOrders.Size = new System.Drawing.Size(166, 236);
			this.txtOrders.TabIndex = 7;
			this.txtOrders.TabStop = false;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(18, 66);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "In Index(es):";
			// 
			// clbIndexes
			// 
			this.clbIndexes.CheckOnClick = true;
			this.clbIndexes.FormattingEnabled = true;
			this.clbIndexes.HorizontalScrollbar = true;
			this.clbIndexes.Location = new System.Drawing.Point(16, 82);
			this.clbIndexes.MultiColumn = true;
			this.clbIndexes.Name = "clbIndexes";
			this.clbIndexes.Size = new System.Drawing.Size(518, 109);
			this.clbIndexes.TabIndex = 11;
			// 
			// lblAccessionsTime
			// 
			this.lblAccessionsTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblAccessionsTime.AutoSize = true;
			this.lblAccessionsTime.Location = new System.Drawing.Point(13, 464);
			this.lblAccessionsTime.Name = "lblAccessionsTime";
			this.lblAccessionsTime.Size = new System.Drawing.Size(70, 13);
			this.lblAccessionsTime.TabIndex = 12;
			this.lblAccessionsTime.Text = "Search Time:";
			// 
			// lblInventoryTime
			// 
			this.lblInventoryTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblInventoryTime.AutoSize = true;
			this.lblInventoryTime.Location = new System.Drawing.Point(184, 464);
			this.lblInventoryTime.Name = "lblInventoryTime";
			this.lblInventoryTime.Size = new System.Drawing.Size(70, 13);
			this.lblInventoryTime.TabIndex = 13;
			this.lblInventoryTime.Text = "Search Time:";
			// 
			// lblOrdersTime
			// 
			this.lblOrdersTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblOrdersTime.AutoSize = true;
			this.lblOrdersTime.Location = new System.Drawing.Point(365, 464);
			this.lblOrdersTime.Name = "lblOrdersTime";
			this.lblOrdersTime.Size = new System.Drawing.Size(70, 13);
			this.lblOrdersTime.TabIndex = 14;
			this.lblOrdersTime.Text = "Search Time:";
			// 
			// chkIgnoreCase
			// 
			this.chkIgnoreCase.AutoSize = true;
			this.chkIgnoreCase.Checked = true;
			this.chkIgnoreCase.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkIgnoreCase.Location = new System.Drawing.Point(98, 40);
			this.chkIgnoreCase.Name = "chkIgnoreCase";
			this.chkIgnoreCase.Size = new System.Drawing.Size(83, 17);
			this.chkIgnoreCase.TabIndex = 15;
			this.chkIgnoreCase.Text = "Ignore Case";
			this.chkIgnoreCase.UseVisualStyleBackColor = true;
			// 
			// chkMatchAll
			// 
			this.chkMatchAll.AutoSize = true;
			this.chkMatchAll.Location = new System.Drawing.Point(188, 40);
			this.chkMatchAll.Name = "chkMatchAll";
			this.chkMatchAll.Size = new System.Drawing.Size(119, 17);
			this.chkMatchAll.TabIndex = 16;
			this.chkMatchAll.Text = "Match All Keywords";
			this.chkMatchAll.UseVisualStyleBackColor = true;
			// 
			// chkCheckAll
			// 
			this.chkCheckAll.AutoSize = true;
			this.chkCheckAll.Checked = true;
			this.chkCheckAll.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCheckAll.Location = new System.Drawing.Point(463, 62);
			this.chkCheckAll.Name = "chkCheckAll";
			this.chkCheckAll.Size = new System.Drawing.Size(71, 17);
			this.chkCheckAll.TabIndex = 17;
			this.chkCheckAll.Text = "Check All";
			this.chkCheckAll.UseVisualStyleBackColor = false;
			this.chkCheckAll.CheckedChanged += new System.EventHandler(this.chkCheckAll_CheckedChanged);
			// 
			// chkUseService
			// 
			this.chkUseService.AutoSize = true;
			this.chkUseService.Location = new System.Drawing.Point(408, 40);
			this.chkUseService.Name = "chkUseService";
			this.chkUseService.Size = new System.Drawing.Size(126, 17);
			this.chkUseService.TabIndex = 18;
			this.chkUseService.Text = "Use Installed Service";
			this.chkUseService.UseVisualStyleBackColor = true;
			// 
			// frmSearch
			// 
			this.AcceptButton = this.btnGo;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(546, 497);
			this.Controls.Add(this.chkUseService);
			this.Controls.Add(this.chkCheckAll);
			this.Controls.Add(this.chkMatchAll);
			this.Controls.Add(this.chkIgnoreCase);
			this.Controls.Add(this.lblOrdersTime);
			this.Controls.Add(this.lblInventoryTime);
			this.Controls.Add(this.lblAccessionsTime);
			this.Controls.Add(this.clbIndexes);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblOrders);
			this.Controls.Add(this.txtOrders);
			this.Controls.Add(this.lblInventory);
			this.Controls.Add(this.txtInventory);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.txtSearch);
			this.Controls.Add(this.lblAccessions);
			this.Controls.Add(this.txtAccessions);
			this.Name = "frmSearch";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "GRIN-Global - Search Query";
			this.Load += new System.EventHandler(this.frmSearch_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtAccessions;
		private System.Windows.Forms.Label lblAccessions;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblInventory;
		private System.Windows.Forms.TextBox txtInventory;
		private System.Windows.Forms.Label lblOrders;
		private System.Windows.Forms.TextBox txtOrders;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckedListBox clbIndexes;
		private System.Windows.Forms.Label lblAccessionsTime;
		private System.Windows.Forms.Label lblInventoryTime;
		private System.Windows.Forms.Label lblOrdersTime;
		private System.Windows.Forms.CheckBox chkIgnoreCase;
		private System.Windows.Forms.CheckBox chkMatchAll;
		private System.Windows.Forms.CheckBox chkCheckAll;
		private System.Windows.Forms.CheckBox chkUseService;
	}
}