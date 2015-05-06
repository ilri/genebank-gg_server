namespace GrinGlobal.CooperatorMunger {
    partial class frmCooperatorMunger {
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
            this.gbFromDatabase = new System.Windows.Forms.GroupBox();
            this.rbFromDatabasePostgreSQL = new System.Windows.Forms.RadioButton();
            this.txtFromDatabaseName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cboFromConnectionString = new System.Windows.Forms.ComboBox();
            this.rbFromDatabaseOracle = new System.Windows.Forms.RadioButton();
            this.rbFromDatabaseSQLServer = new System.Windows.Forms.RadioButton();
            this.rbFromDatabaseMySQL = new System.Windows.Forms.RadioButton();
            this.label13 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnMunge = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbFromDatabase.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFromDatabase
            // 
            this.gbFromDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFromDatabase.Controls.Add(this.rbFromDatabasePostgreSQL);
            this.gbFromDatabase.Controls.Add(this.txtFromDatabaseName);
            this.gbFromDatabase.Controls.Add(this.label14);
            this.gbFromDatabase.Controls.Add(this.cboFromConnectionString);
            this.gbFromDatabase.Controls.Add(this.rbFromDatabaseOracle);
            this.gbFromDatabase.Controls.Add(this.rbFromDatabaseSQLServer);
            this.gbFromDatabase.Controls.Add(this.rbFromDatabaseMySQL);
            this.gbFromDatabase.Controls.Add(this.label13);
            this.gbFromDatabase.Location = new System.Drawing.Point(12, 12);
            this.gbFromDatabase.Name = "gbFromDatabase";
            this.gbFromDatabase.Size = new System.Drawing.Size(592, 68);
            this.gbFromDatabase.TabIndex = 20;
            this.gbFromDatabase.TabStop = false;
            this.gbFromDatabase.Text = "Database";
            // 
            // rbFromDatabasePostgreSQL
            // 
            this.rbFromDatabasePostgreSQL.AutoSize = true;
            this.rbFromDatabasePostgreSQL.Location = new System.Drawing.Point(90, 17);
            this.rbFromDatabasePostgreSQL.Name = "rbFromDatabasePostgreSQL";
            this.rbFromDatabasePostgreSQL.Size = new System.Drawing.Size(82, 17);
            this.rbFromDatabasePostgreSQL.TabIndex = 21;
            this.rbFromDatabasePostgreSQL.Text = "PostgreSQL";
            this.rbFromDatabasePostgreSQL.UseVisualStyleBackColor = true;
            // 
            // txtFromDatabaseName
            // 
            this.txtFromDatabaseName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFromDatabaseName.Location = new System.Drawing.Point(408, 16);
            this.txtFromDatabaseName.Name = "txtFromDatabaseName";
            this.txtFromDatabaseName.Size = new System.Drawing.Size(80, 20);
            this.txtFromDatabaseName.TabIndex = 35;
            this.txtFromDatabaseName.Text = "GRINGLOBAL";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(348, 19);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(54, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "DB name:";
            // 
            // cboFromConnectionString
            // 
            this.cboFromConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFromConnectionString.FormattingEnabled = true;
            this.cboFromConnectionString.Items.AddRange(new object[] {
            "---- SQL Server ----",
            "Data Source=localhost\\sqlexpress;Database=gringlobal;user id=sa;password=",
            "---- PostgreSQL ----",
            "Server=localhost;Port=5432;Database=gringlobal;user id=postgres;password=",
            "---- Oracle ----",
            "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(PORT=1521)(HOST=127.0.0.1))(CONN" +
                "ECT_DATA=(SERVICE_NAME=XE)));User Id=SYS;password=",
            "---- MySQL ----",
            "Data Source=localhost;Database=gringlobal;user id=root;password="});
            this.cboFromConnectionString.Location = new System.Drawing.Point(6, 42);
            this.cboFromConnectionString.Name = "cboFromConnectionString";
            this.cboFromConnectionString.Size = new System.Drawing.Size(580, 21);
            this.cboFromConnectionString.TabIndex = 23;
            // 
            // rbFromDatabaseOracle
            // 
            this.rbFromDatabaseOracle.AutoSize = true;
            this.rbFromDatabaseOracle.Location = new System.Drawing.Point(178, 17);
            this.rbFromDatabaseOracle.Name = "rbFromDatabaseOracle";
            this.rbFromDatabaseOracle.Size = new System.Drawing.Size(56, 17);
            this.rbFromDatabaseOracle.TabIndex = 22;
            this.rbFromDatabaseOracle.Text = "Oracle";
            this.rbFromDatabaseOracle.UseVisualStyleBackColor = true;
            // 
            // rbFromDatabaseSQLServer
            // 
            this.rbFromDatabaseSQLServer.AutoSize = true;
            this.rbFromDatabaseSQLServer.Checked = true;
            this.rbFromDatabaseSQLServer.Location = new System.Drawing.Point(13, 17);
            this.rbFromDatabaseSQLServer.Name = "rbFromDatabaseSQLServer";
            this.rbFromDatabaseSQLServer.Size = new System.Drawing.Size(74, 17);
            this.rbFromDatabaseSQLServer.TabIndex = 20;
            this.rbFromDatabaseSQLServer.TabStop = true;
            this.rbFromDatabaseSQLServer.Text = "Sql Server";
            this.rbFromDatabaseSQLServer.UseVisualStyleBackColor = true;
            // 
            // rbFromDatabaseMySQL
            // 
            this.rbFromDatabaseMySQL.AutoSize = true;
            this.rbFromDatabaseMySQL.Location = new System.Drawing.Point(235, 17);
            this.rbFromDatabaseMySQL.Name = "rbFromDatabaseMySQL";
            this.rbFromDatabaseMySQL.Size = new System.Drawing.Size(54, 17);
            this.rbFromDatabaseMySQL.TabIndex = 23;
            this.rbFromDatabaseMySQL.Text = "MySql";
            this.rbFromDatabaseMySQL.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(495, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(91, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "Connection String";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(12, 86);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(132, 23);
            this.btnTest.TabIndex = 21;
            this.btnTest.Text = "Test Connection";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnMunge
            // 
            this.btnMunge.Enabled = false;
            this.btnMunge.Location = new System.Drawing.Point(150, 86);
            this.btnMunge.Name = "btnMunge";
            this.btnMunge.Size = new System.Drawing.Size(172, 23);
            this.btnMunge.TabIndex = 22;
            this.btnMunge.Text = "Munge Cooperator Information";
            this.btnMunge.UseVisualStyleBackColor = true;
            this.btnMunge.Click += new System.EventHandler(this.btnMunge_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(18, 116);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(581, 134);
            this.textBox1.TabIndex = 23;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(328, 86);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmCooperatorMunger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 262);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnMunge);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.gbFromDatabase);
            this.Name = "frmCooperatorMunger";
            this.Text = "Cooperator Munger";
            this.gbFromDatabase.ResumeLayout(false);
            this.gbFromDatabase.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFromDatabase;
        private System.Windows.Forms.RadioButton rbFromDatabasePostgreSQL;
        private System.Windows.Forms.TextBox txtFromDatabaseName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cboFromConnectionString;
        private System.Windows.Forms.RadioButton rbFromDatabaseOracle;
        private System.Windows.Forms.RadioButton rbFromDatabaseSQLServer;
        private System.Windows.Forms.RadioButton rbFromDatabaseMySQL;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnMunge;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnCancel;
    }
}

