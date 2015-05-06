namespace GrinGlobal.Core.TestHarness {
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chkSybase = new System.Windows.Forms.CheckBox();
            this.chkOracle = new System.Windows.Forms.CheckBox();
            this.chkSQLServer = new System.Windows.Forms.CheckBox();
            this.dgvSybase = new System.Windows.Forms.DataGridView();
            this.dgvOracle = new System.Windows.Forms.DataGridView();
            this.dgvSQLServer = new System.Windows.Forms.DataGridView();
            this.chkSQLite = new System.Windows.Forms.CheckBox();
            this.chkPostgreSQL = new System.Windows.Forms.CheckBox();
            this.chkMySQL = new System.Windows.Forms.CheckBox();
            this.dgvSQLite = new System.Windows.Forms.DataGridView();
            this.dgvPostgreSQL = new System.Windows.Forms.DataGridView();
            this.dgvMySQL = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSybase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOracle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSQLServer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSQLite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPostgreSQL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMySQL)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Fill";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(396, 10);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(219, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 4;
            this.txtName.Text = "John Q. Public";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(499, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Location = new System.Drawing.Point(12, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(702, 392);
            this.panel1.TabIndex = 23;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chkSybase);
            this.splitContainer1.Panel1.Controls.Add(this.chkOracle);
            this.splitContainer1.Panel1.Controls.Add(this.chkSQLServer);
            this.splitContainer1.Panel1.Controls.Add(this.dgvSybase);
            this.splitContainer1.Panel1.Controls.Add(this.dgvOracle);
            this.splitContainer1.Panel1.Controls.Add(this.dgvSQLServer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chkSQLite);
            this.splitContainer1.Panel2.Controls.Add(this.chkPostgreSQL);
            this.splitContainer1.Panel2.Controls.Add(this.chkMySQL);
            this.splitContainer1.Panel2.Controls.Add(this.dgvSQLite);
            this.splitContainer1.Panel2.Controls.Add(this.dgvPostgreSQL);
            this.splitContainer1.Panel2.Controls.Add(this.dgvMySQL);
            this.splitContainer1.Size = new System.Drawing.Size(702, 392);
            this.splitContainer1.SplitterDistance = 335;
            this.splitContainer1.TabIndex = 0;
            // 
            // chkSybase
            // 
            this.chkSybase.AutoSize = true;
            this.chkSybase.Location = new System.Drawing.Point(3, 256);
            this.chkSybase.Name = "chkSybase";
            this.chkSybase.Size = new System.Drawing.Size(61, 17);
            this.chkSybase.TabIndex = 28;
            this.chkSybase.Text = "Sybase";
            this.chkSybase.UseVisualStyleBackColor = true;
            // 
            // chkOracle
            // 
            this.chkOracle.AutoSize = true;
            this.chkOracle.Location = new System.Drawing.Point(3, 134);
            this.chkOracle.Name = "chkOracle";
            this.chkOracle.Size = new System.Drawing.Size(57, 17);
            this.chkOracle.TabIndex = 27;
            this.chkOracle.Text = "Oracle";
            this.chkOracle.UseVisualStyleBackColor = true;
            // 
            // chkSQLServer
            // 
            this.chkSQLServer.AutoSize = true;
            this.chkSQLServer.Location = new System.Drawing.Point(3, 5);
            this.chkSQLServer.Name = "chkSQLServer";
            this.chkSQLServer.Size = new System.Drawing.Size(81, 17);
            this.chkSQLServer.TabIndex = 26;
            this.chkSQLServer.Text = "SQL Server";
            this.chkSQLServer.UseVisualStyleBackColor = true;
            // 
            // dgvSybase
            // 
            this.dgvSybase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSybase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSybase.Location = new System.Drawing.Point(3, 279);
            this.dgvSybase.Name = "dgvSybase";
            this.dgvSybase.Size = new System.Drawing.Size(327, 108);
            this.dgvSybase.TabIndex = 25;
            // 
            // dgvOracle
            // 
            this.dgvOracle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOracle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOracle.Location = new System.Drawing.Point(3, 157);
            this.dgvOracle.Name = "dgvOracle";
            this.dgvOracle.Size = new System.Drawing.Size(327, 93);
            this.dgvOracle.TabIndex = 24;
            // 
            // dgvSQLServer
            // 
            this.dgvSQLServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSQLServer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSQLServer.Location = new System.Drawing.Point(3, 25);
            this.dgvSQLServer.Name = "dgvSQLServer";
            this.dgvSQLServer.Size = new System.Drawing.Size(327, 106);
            this.dgvSQLServer.TabIndex = 23;
            // 
            // chkSQLite
            // 
            this.chkSQLite.AutoSize = true;
            this.chkSQLite.Location = new System.Drawing.Point(2, 256);
            this.chkSQLite.Name = "chkSQLite";
            this.chkSQLite.Size = new System.Drawing.Size(58, 17);
            this.chkSQLite.TabIndex = 27;
            this.chkSQLite.Text = "SQLite";
            this.chkSQLite.UseVisualStyleBackColor = true;
            // 
            // chkPostgreSQL
            // 
            this.chkPostgreSQL.AutoSize = true;
            this.chkPostgreSQL.Location = new System.Drawing.Point(2, 137);
            this.chkPostgreSQL.Name = "chkPostgreSQL";
            this.chkPostgreSQL.Size = new System.Drawing.Size(83, 17);
            this.chkPostgreSQL.TabIndex = 26;
            this.chkPostgreSQL.Text = "PostgreSQL";
            this.chkPostgreSQL.UseVisualStyleBackColor = true;
            // 
            // chkMySQL
            // 
            this.chkMySQL.AutoSize = true;
            this.chkMySQL.Location = new System.Drawing.Point(3, 5);
            this.chkMySQL.Name = "chkMySQL";
            this.chkMySQL.Size = new System.Drawing.Size(61, 17);
            this.chkMySQL.TabIndex = 25;
            this.chkMySQL.Text = "MySQL";
            this.chkMySQL.UseVisualStyleBackColor = true;
            // 
            // dgvSQLite
            // 
            this.dgvSQLite.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSQLite.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSQLite.Location = new System.Drawing.Point(2, 279);
            this.dgvSQLite.Name = "dgvSQLite";
            this.dgvSQLite.Size = new System.Drawing.Size(358, 108);
            this.dgvSQLite.TabIndex = 24;
            // 
            // dgvPostgreSQL
            // 
            this.dgvPostgreSQL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPostgreSQL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPostgreSQL.Location = new System.Drawing.Point(3, 157);
            this.dgvPostgreSQL.Name = "dgvPostgreSQL";
            this.dgvPostgreSQL.Size = new System.Drawing.Size(358, 93);
            this.dgvPostgreSQL.TabIndex = 23;
            // 
            // dgvMySQL
            // 
            this.dgvMySQL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMySQL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMySQL.Location = new System.Drawing.Point(2, 25);
            this.dgvMySQL.Name = "dgvMySQL";
            this.dgvMySQL.Size = new System.Drawing.Size(359, 106);
            this.dgvMySQL.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(141, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Author Name:";
            // 
            // txtAge
            // 
            this.txtAge.Location = new System.Drawing.Point(360, 12);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(30, 20);
            this.txtAge.TabIndex = 25;
            this.txtAge.Text = "40";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(325, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Age:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 442);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAge);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "DataManager Test Harness";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSybase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOracle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSQLServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSQLite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPostgreSQL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMySQL)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox chkSybase;
        private System.Windows.Forms.CheckBox chkOracle;
        private System.Windows.Forms.CheckBox chkSQLServer;
        private System.Windows.Forms.DataGridView dgvSybase;
        private System.Windows.Forms.DataGridView dgvOracle;
        private System.Windows.Forms.DataGridView dgvSQLServer;
        private System.Windows.Forms.CheckBox chkSQLite;
        private System.Windows.Forms.CheckBox chkPostgreSQL;
        private System.Windows.Forms.CheckBox chkMySQL;
        private System.Windows.Forms.DataGridView dgvSQLite;
        private System.Windows.Forms.DataGridView dgvPostgreSQL;
        private System.Windows.Forms.DataGridView dgvMySQL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.Label label2;
	}
}

