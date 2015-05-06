namespace GrinGlobal.Core.TestHarness {
	partial class frmCrypto {
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
			this.button2 = new System.Windows.Forms.Button();
			this.txtPlain = new System.Windows.Forms.TextBox();
			this.txtCipher = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(159, 134);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Encrypt";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(286, 134);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Decrypt";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// txtPlain
			// 
			this.txtPlain.Location = new System.Drawing.Point(13, 13);
			this.txtPlain.Multiline = true;
			this.txtPlain.Name = "txtPlain";
			this.txtPlain.Size = new System.Drawing.Size(498, 115);
			this.txtPlain.TabIndex = 2;
			// 
			// txtCipher
			// 
			this.txtCipher.Location = new System.Drawing.Point(12, 163);
			this.txtCipher.Multiline = true;
			this.txtCipher.Name = "txtCipher";
			this.txtCipher.Size = new System.Drawing.Size(498, 115);
			this.txtCipher.TabIndex = 3;
			// 
			// frmCrypto
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(523, 291);
			this.Controls.Add(this.txtCipher);
			this.Controls.Add(this.txtPlain);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "frmCrypto";
			this.Text = "frmCrypto";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox txtPlain;
		private System.Windows.Forms.TextBox txtCipher;
	}
}