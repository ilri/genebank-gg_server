namespace GrinGlobal.DatabaseCopier {
	partial class frmWizardBase {
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
			this.btnNext = new System.Windows.Forms.Button();
			this.btnPrevious = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnFinish = new System.Windows.Forms.Button();
			this.gbMain = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// btnNext
			// 
			this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNext.Location = new System.Drawing.Point(409, 315);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(83, 23);
			this.btnNext.TabIndex = 0;
			this.btnNext.Text = "&Next >";
			this.btnNext.UseVisualStyleBackColor = true;
			// 
			// btnPrevious
			// 
			this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPrevious.Location = new System.Drawing.Point(322, 315);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(81, 23);
			this.btnPrevious.TabIndex = 1;
			this.btnPrevious.Text = "< &Previous";
			this.btnPrevious.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(235, 315);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(81, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnFinish
			// 
			this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFinish.Location = new System.Drawing.Point(498, 315);
			this.btnFinish.Name = "btnFinish";
			this.btnFinish.Size = new System.Drawing.Size(83, 23);
			this.btnFinish.TabIndex = 3;
			this.btnFinish.Text = "&Finish";
			this.btnFinish.UseVisualStyleBackColor = true;
			// 
			// gbMain
			// 
			this.gbMain.Location = new System.Drawing.Point(13, 13);
			this.gbMain.Name = "gbMain";
			this.gbMain.Size = new System.Drawing.Size(568, 296);
			this.gbMain.TabIndex = 4;
			this.gbMain.TabStop = false;
			this.gbMain.Text = "Main Wizard";
			// 
			// frmWizardBase
			// 
			this.AcceptButton = this.btnNext;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(593, 350);
			this.Controls.Add(this.gbMain);
			this.Controls.Add(this.btnFinish);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnPrevious);
			this.Controls.Add(this.btnNext);
			this.Name = "frmWizardBase";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Wizard";
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.Button btnNext;
		public System.Windows.Forms.Button btnPrevious;
		public System.Windows.Forms.Button btnCancel;
		public System.Windows.Forms.Button btnFinish;
		public System.Windows.Forms.GroupBox gbMain;

	}
}