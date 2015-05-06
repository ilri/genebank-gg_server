namespace GrinGlobal.Sql.TestHarness {
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
			this.button1 = new System.Windows.Forms.Button();
			this.dgView = new System.Windows.Forms.DataGridView();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnFill = new System.Windows.Forms.Button();
			this.btnFill2 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.btnLanguageToggle = new System.Windows.Forms.Button();
			this.btnSecUser = new System.Windows.Forms.Button();
			this.btnInspectSecUser = new System.Windows.Forms.Button();
			this.btnSearch = new System.Windows.Forms.Button();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.btnCreateMappings = new System.Windows.Forms.Button();
			this.btnDeleteMapping = new System.Windows.Forms.Button();
			this.txtMapping = new System.Windows.Forms.TextBox();
			this.btnInspect = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgView)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(1010, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(120, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Check Security";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// dgView
			// 
			this.dgView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dgView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgView.Location = new System.Drawing.Point(3, 45);
			this.dgView.Name = "dgView";
			this.dgView.Size = new System.Drawing.Size(1136, 245);
			this.dgView.TabIndex = 1;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(1055, 296);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "&Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnFill
			// 
			this.btnFill.Location = new System.Drawing.Point(3, 12);
			this.btnFill.Name = "btnFill";
			this.btnFill.Size = new System.Drawing.Size(75, 23);
			this.btnFill.TabIndex = 3;
			this.btnFill.Text = "&Fill";
			this.btnFill.UseVisualStyleBackColor = true;
			this.btnFill.Click += new System.EventHandler(this.btnFill_Click);
			// 
			// btnFill2
			// 
			this.btnFill2.Location = new System.Drawing.Point(84, 12);
			this.btnFill2.Name = "btnFill2";
			this.btnFill2.Size = new System.Drawing.Size(75, 23);
			this.btnFill2.TabIndex = 4;
			this.btnFill2.Text = "IV and ACC";
			this.btnFill2.UseVisualStyleBackColor = true;
			this.btnFill2.Click += new System.EventHandler(this.btnFill2_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(165, 12);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 5;
			this.button2.Text = "ViewIV";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// btnLanguageToggle
			// 
			this.btnLanguageToggle.Location = new System.Drawing.Point(345, 12);
			this.btnLanguageToggle.Name = "btnLanguageToggle";
			this.btnLanguageToggle.Size = new System.Drawing.Size(117, 23);
			this.btnLanguageToggle.TabIndex = 6;
			this.btnLanguageToggle.Text = "Toggle Language";
			this.btnLanguageToggle.UseVisualStyleBackColor = true;
			this.btnLanguageToggle.Click += new System.EventHandler(this.btnCountryToggle_Click);
			// 
			// btnSecUser
			// 
			this.btnSecUser.Location = new System.Drawing.Point(403, 295);
			this.btnSecUser.Name = "btnSecUser";
			this.btnSecUser.Size = new System.Drawing.Size(117, 23);
			this.btnSecUser.TabIndex = 7;
			this.btnSecUser.Text = "Gen SecUser";
			this.btnSecUser.UseVisualStyleBackColor = true;
			// 
			// btnInspectSecUser
			// 
			this.btnInspectSecUser.Location = new System.Drawing.Point(526, 295);
			this.btnInspectSecUser.Name = "btnInspectSecUser";
			this.btnInspectSecUser.Size = new System.Drawing.Size(117, 23);
			this.btnInspectSecUser.TabIndex = 8;
			this.btnInspectSecUser.Text = "Inspect SecUser";
			this.btnInspectSecUser.UseVisualStyleBackColor = true;
			// 
			// btnSearch
			// 
			this.btnSearch.Location = new System.Drawing.Point(294, 296);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(75, 23);
			this.btnSearch.TabIndex = 9;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// txtSearch
			// 
			this.txtSearch.Location = new System.Drawing.Point(3, 298);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(285, 20);
			this.txtSearch.TabIndex = 10;
			this.txtSearch.Text = "acid: 1000001,1000002 1000003, 1000004       1000005";
			// 
			// btnCreateMappings
			// 
			this.btnCreateMappings.Location = new System.Drawing.Point(649, 295);
			this.btnCreateMappings.Name = "btnCreateMappings";
			this.btnCreateMappings.Size = new System.Drawing.Size(117, 23);
			this.btnCreateMappings.TabIndex = 11;
			this.btnCreateMappings.Text = "Recreate Mappings";
			this.btnCreateMappings.UseVisualStyleBackColor = true;
			this.btnCreateMappings.Click += new System.EventHandler(this.btnCreateMappings_Click);
			// 
			// btnDeleteMapping
			// 
			this.btnDeleteMapping.Location = new System.Drawing.Point(789, 296);
			this.btnDeleteMapping.Name = "btnDeleteMapping";
			this.btnDeleteMapping.Size = new System.Drawing.Size(102, 23);
			this.btnDeleteMapping.TabIndex = 12;
			this.btnDeleteMapping.Text = "Delete Mapping >";
			this.btnDeleteMapping.UseVisualStyleBackColor = true;
			// 
			// txtMapping
			// 
			this.txtMapping.Location = new System.Drawing.Point(898, 298);
			this.txtMapping.Name = "txtMapping";
			this.txtMapping.Size = new System.Drawing.Size(100, 20);
			this.txtMapping.TabIndex = 13;
			this.txtMapping.Text = "";
			// 
			// btnInspect
			// 
			this.btnInspect.Location = new System.Drawing.Point(510, 12);
			this.btnInspect.Name = "btnInspect";
			this.btnInspect.Size = new System.Drawing.Size(75, 23);
			this.btnInspect.TabIndex = 14;
			this.btnInspect.Text = "Inspect DB";
			this.btnInspect.UseVisualStyleBackColor = true;
			this.btnInspect.Click += new System.EventHandler(this.btnInspect_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1142, 325);
			this.Controls.Add(this.btnInspect);
			this.Controls.Add(this.txtMapping);
			this.Controls.Add(this.btnDeleteMapping);
			this.Controls.Add(this.btnCreateMappings);
			this.Controls.Add(this.txtSearch);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.btnInspectSecUser);
			this.Controls.Add(this.btnSecUser);
			this.Controls.Add(this.btnLanguageToggle);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.btnFill2);
			this.Controls.Add(this.btnFill);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.dgView);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.dgView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.DataGridView dgView;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnFill;
		private System.Windows.Forms.Button btnFill2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button btnLanguageToggle;
		private System.Windows.Forms.Button btnSecUser;
		private System.Windows.Forms.Button btnInspectSecUser;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.Button btnCreateMappings;
		private System.Windows.Forms.Button btnDeleteMapping;
		private System.Windows.Forms.TextBox txtMapping;
		private System.Windows.Forms.Button btnInspect;
	}
}

