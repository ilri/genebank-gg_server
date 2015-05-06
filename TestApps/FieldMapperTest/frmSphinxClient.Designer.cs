namespace FieldMapperTest {
	partial class frmSphinxClient {
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
			this.button1 = new System.Windows.Forms.Button();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.lbResults = new System.Windows.Forms.ListBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkCOOP = new System.Windows.Forms.CheckBox();
			this.chkORD = new System.Windows.Forms.CheckBox();
			this.chkIV = new System.Windows.Forms.CheckBox();
			this.chkACC = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.ddlMode = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(439, 57);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Search";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// txtSearch
			// 
			this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSearch.Location = new System.Drawing.Point(81, 13);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(433, 20);
			this.txtSearch.TabIndex = 1;
			this.txtSearch.Text = "Millard";
			// 
			// lbResults
			// 
			this.lbResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbResults.FormattingEnabled = true;
			this.lbResults.Location = new System.Drawing.Point(13, 103);
			this.lbResults.Name = "lbResults";
			this.lbResults.Size = new System.Drawing.Size(501, 199);
			this.lbResults.TabIndex = 6;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkCOOP);
			this.groupBox1.Controls.Add(this.chkORD);
			this.groupBox1.Controls.Add(this.chkIV);
			this.groupBox1.Controls.Add(this.chkACC);
			this.groupBox1.Location = new System.Drawing.Point(13, 44);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(243, 42);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "In Indexes:";
			this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
			// 
			// chkCOOP
			// 
			this.chkCOOP.AutoSize = true;
			this.chkCOOP.Checked = true;
			this.chkCOOP.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCOOP.Location = new System.Drawing.Point(159, 19);
			this.chkCOOP.Name = "chkCOOP";
			this.chkCOOP.Size = new System.Drawing.Size(56, 17);
			this.chkCOOP.TabIndex = 9;
			this.chkCOOP.Text = "COOP";
			this.chkCOOP.UseVisualStyleBackColor = true;
			// 
			// chkORD
			// 
			this.chkORD.AutoSize = true;
			this.chkORD.Checked = true;
			this.chkORD.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkORD.Location = new System.Drawing.Point(103, 19);
			this.chkORD.Name = "chkORD";
			this.chkORD.Size = new System.Drawing.Size(50, 17);
			this.chkORD.TabIndex = 8;
			this.chkORD.Text = "ORD";
			this.chkORD.UseVisualStyleBackColor = true;
			// 
			// chkIV
			// 
			this.chkIV.AutoSize = true;
			this.chkIV.Checked = true;
			this.chkIV.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkIV.Location = new System.Drawing.Point(60, 19);
			this.chkIV.Name = "chkIV";
			this.chkIV.Size = new System.Drawing.Size(36, 17);
			this.chkIV.TabIndex = 7;
			this.chkIV.Text = "IV";
			this.chkIV.UseVisualStyleBackColor = true;
			// 
			// chkACC
			// 
			this.chkACC.AutoSize = true;
			this.chkACC.Checked = true;
			this.chkACC.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkACC.Location = new System.Drawing.Point(6, 19);
			this.chkACC.Name = "chkACC";
			this.chkACC.Size = new System.Drawing.Size(47, 17);
			this.chkACC.TabIndex = 6;
			this.chkACC.Text = "ACC";
			this.chkACC.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.ddlMode);
			this.groupBox2.Location = new System.Drawing.Point(262, 44);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(163, 42);
			this.groupBox2.TabIndex = 9;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Mode:";
			// 
			// ddlMode
			// 
			this.ddlMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlMode.FormattingEnabled = true;
			this.ddlMode.Items.AddRange(new object[] {
            "All",
            "Any",
            "Phrase",
            "Boolean",
            "Extended",
            "FullScan",
            "Extended2"});
			this.ddlMode.Location = new System.Drawing.Point(6, 15);
			this.ddlMode.Name = "ddlMode";
			this.ddlMode.Size = new System.Drawing.Size(145, 21);
			this.ddlMode.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 13);
			this.label1.TabIndex = 10;
			this.label1.Text = "Search For:";
			// 
			// frmSphinxClient
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(526, 310);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lbResults);
			this.Controls.Add(this.txtSearch);
			this.Controls.Add(this.button1);
			this.Name = "frmSphinxClient";
			this.Text = "frmSphinxClient";
			this.Load += new System.EventHandler(this.frmSphinxClient_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.ListBox lbResults;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkCOOP;
		private System.Windows.Forms.CheckBox chkORD;
		private System.Windows.Forms.CheckBox chkIV;
		private System.Windows.Forms.CheckBox chkACC;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox ddlMode;
		private System.Windows.Forms.Label label1;
	}
}