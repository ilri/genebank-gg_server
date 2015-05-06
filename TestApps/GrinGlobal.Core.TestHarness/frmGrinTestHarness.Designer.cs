namespace GrinGlobal.Core.TestHarness {
	partial class frmGrinTestHarness {
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.dgvOracle = new System.Windows.Forms.DataGridView();
			this.label2 = new System.Windows.Forms.Label();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.dgvMySQL = new System.Windows.Forms.DataGridView();
			this.label3 = new System.Windows.Forms.Label();
			this.dgvSqlServer = new System.Windows.Forms.DataGridView();
			this.label4 = new System.Windows.Forms.Label();
			this.txtIgName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnSearch = new System.Windows.Forms.Button();
			this.chkSyncScroll = new System.Windows.Forms.CheckBox();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvOracle)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvMySQL)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvSqlServer)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(1, 34);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.dgvOracle);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(921, 420);
			this.splitContainer1.SplitterDistance = 149;
			this.splitContainer1.TabIndex = 9;
			// 
			// dgvOracle
			// 
			this.dgvOracle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dgvOracle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvOracle.Location = new System.Drawing.Point(3, 25);
			this.dgvOracle.Name = "dgvOracle";
			this.dgvOracle.ReadOnly = true;
			this.dgvOracle.Size = new System.Drawing.Size(915, 121);
			this.dgvOracle.TabIndex = 7;
			this.dgvOracle.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvOracle_Scroll);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Oracle";
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
			this.splitContainer2.Panel1.Controls.Add(this.dgvMySQL);
			this.splitContainer2.Panel1.Controls.Add(this.label3);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.dgvSqlServer);
			this.splitContainer2.Panel2.Controls.Add(this.label4);
			this.splitContainer2.Size = new System.Drawing.Size(921, 267);
			this.splitContainer2.SplitterDistance = 122;
			this.splitContainer2.TabIndex = 0;
			// 
			// dgvMySQL
			// 
			this.dgvMySQL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dgvMySQL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvMySQL.Location = new System.Drawing.Point(3, 16);
			this.dgvMySQL.Name = "dgvMySQL";
			this.dgvMySQL.ReadOnly = true;
			this.dgvMySQL.Size = new System.Drawing.Size(915, 104);
			this.dgvMySQL.TabIndex = 8;
			this.dgvMySQL.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvMySQL_Scroll);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(42, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "MySQL";
			// 
			// dgvSqlServer
			// 
			this.dgvSqlServer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dgvSqlServer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSqlServer.Location = new System.Drawing.Point(3, 15);
			this.dgvSqlServer.Name = "dgvSqlServer";
			this.dgvSqlServer.ReadOnly = true;
			this.dgvSqlServer.Size = new System.Drawing.Size(915, 126);
			this.dgvSqlServer.TabIndex = 10;
			this.dgvSqlServer.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvSqlServer_Scroll);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, -1);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(62, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "SQL Server";
			// 
			// txtIgName
			// 
			this.txtIgName.Location = new System.Drawing.Point(119, 8);
			this.txtIgName.Name = "txtIgName";
			this.txtIgName.Size = new System.Drawing.Size(166, 20);
			this.txtIgName.TabIndex = 12;
			this.txtIgName.Text = "Zea.Ames.GH.2006.SPR";
			this.txtIgName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "Search By IGNAME:";
			// 
			// btnSearch
			// 
			this.btnSearch.Location = new System.Drawing.Point(291, 9);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(75, 23);
			this.btnSearch.TabIndex = 10;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// chkSyncScroll
			// 
			this.chkSyncScroll.AutoSize = true;
			this.chkSyncScroll.Checked = true;
			this.chkSyncScroll.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSyncScroll.Location = new System.Drawing.Point(621, 14);
			this.chkSyncScroll.Name = "chkSyncScroll";
			this.chkSyncScroll.Size = new System.Drawing.Size(111, 17);
			this.chkSyncScroll.TabIndex = 13;
			this.chkSyncScroll.Text = "Synchronize Grids";
			this.chkSyncScroll.UseVisualStyleBackColor = true;
			// 
			// frmGrinTestHarness
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(924, 457);
			this.Controls.Add(this.chkSyncScroll);
			this.Controls.Add(this.txtIgName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.splitContainer1);
			this.Name = "frmGrinTestHarness";
			this.Text = "GRIN Test Harness for DataManager";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvOracle)).EndInit();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.Panel2.PerformLayout();
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvMySQL)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvSqlServer)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.DataGridView dgvOracle;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DataGridView dgvMySQL;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DataGridView dgvSqlServer;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtIgName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.CheckBox chkSyncScroll;
	}
}