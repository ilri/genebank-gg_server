namespace GrinGlobal.InstallHelper {
    partial class frmDatabaseEnginePrompt2 {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDatabaseEnginePrompt2));
            this.rdoLocal = new System.Windows.Forms.RadioButton();
            this.rdoRemote = new System.Windows.Forms.RadioButton();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblLocal = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbLocal = new System.Windows.Forms.GroupBox();
            this.lnkSQLite = new System.Windows.Forms.LinkLabel();
            this.rdoSqlite = new System.Windows.Forms.RadioButton();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lnkPostgreSQLInfo = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.rdoPostgreSQL = new System.Windows.Forms.RadioButton();
            this.rdoOracle = new System.Windows.Forms.RadioButton();
            this.rdoMySql = new System.Windows.Forms.RadioButton();
            this.lnkPostgreSQL = new System.Windows.Forms.LinkLabel();
            this.lnkOracle = new System.Windows.Forms.LinkLabel();
            this.lnkMySQL = new System.Windows.Forms.LinkLabel();
            this.lnkSqlServer = new System.Windows.Forms.LinkLabel();
            this.rdoSqlServer = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.lblRemote = new System.Windows.Forms.Label();
            this.rdoOverride = new System.Windows.Forms.RadioButton();
            this.gbLocal.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdoLocal
            // 
            this.rdoLocal.AutoSize = true;
            this.rdoLocal.Checked = true;
            this.rdoLocal.Location = new System.Drawing.Point(32, 54);
            this.rdoLocal.Name = "rdoLocal";
            this.rdoLocal.Size = new System.Drawing.Size(216, 17);
            this.rdoLocal.TabIndex = 3;
            this.rdoLocal.TabStop = true;
            this.rdoLocal.Text = "Use a database hosted on this computer";
            this.rdoLocal.UseVisualStyleBackColor = true;
            this.rdoLocal.CheckedChanged += new System.EventHandler(this.rdoLocal_CheckedChanged);
            // 
            // rdoRemote
            // 
            this.rdoRemote.AutoSize = true;
            this.rdoRemote.Location = new System.Drawing.Point(32, 287);
            this.rdoRemote.Name = "rdoRemote";
            this.rdoRemote.Size = new System.Drawing.Size(247, 17);
            this.rdoRemote.TabIndex = 4;
            this.rdoRemote.Text = "Use a database hosted on a different computer";
            this.rdoRemote.UseVisualStyleBackColor = true;
            this.rdoRemote.CheckedChanged += new System.EventHandler(this.rdoRemote_CheckedChanged);
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(29, 18);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(346, 33);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "GRIN-Global requires a database to operate properly.";
            // 
            // lblLocal
            // 
            this.lblLocal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLocal.Location = new System.Drawing.Point(52, 74);
            this.lblLocal.Name = "lblLocal";
            this.lblLocal.Size = new System.Drawing.Size(323, 34);
            this.lblLocal.TabIndex = 6;
            this.lblLocal.Text = "The following database engines are compatible with GRIN-Global.  All are free of " +
                "charge from their respective vendors.\r\n";
            this.lblLocal.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(259, 409);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(340, 409);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbLocal
            // 
            this.gbLocal.Controls.Add(this.lnkSQLite);
            this.gbLocal.Controls.Add(this.rdoSqlite);
            this.gbLocal.Controls.Add(this.linkLabel3);
            this.gbLocal.Controls.Add(this.linkLabel2);
            this.gbLocal.Controls.Add(this.linkLabel1);
            this.gbLocal.Controls.Add(this.lnkPostgreSQLInfo);
            this.gbLocal.Controls.Add(this.label2);
            this.gbLocal.Controls.Add(this.rdoPostgreSQL);
            this.gbLocal.Controls.Add(this.rdoOracle);
            this.gbLocal.Controls.Add(this.rdoMySql);
            this.gbLocal.Controls.Add(this.lnkPostgreSQL);
            this.gbLocal.Controls.Add(this.lnkOracle);
            this.gbLocal.Controls.Add(this.lnkMySQL);
            this.gbLocal.Controls.Add(this.lnkSqlServer);
            this.gbLocal.Controls.Add(this.rdoSqlServer);
            this.gbLocal.Location = new System.Drawing.Point(55, 111);
            this.gbLocal.Name = "gbLocal";
            this.gbLocal.Size = new System.Drawing.Size(320, 170);
            this.gbLocal.TabIndex = 12;
            this.gbLocal.TabStop = false;
            this.gbLocal.Text = "Select Database Engine";
            this.gbLocal.Visible = false;
            // 
            // lnkSQLite
            // 
            this.lnkSQLite.AutoSize = true;
            this.lnkSQLite.Location = new System.Drawing.Point(226, 114);
            this.lnkSQLite.Name = "lnkSQLite";
            this.lnkSQLite.Size = new System.Drawing.Size(86, 13);
            this.lnkSQLite.TabIndex = 22;
            this.lnkSQLite.TabStop = true;
            this.lnkSQLite.Text = "More Information";
            this.lnkSQLite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSQLite_LinkClicked);
            // 
            // rdoSqlite
            // 
            this.rdoSqlite.AutoSize = true;
            this.rdoSqlite.Location = new System.Drawing.Point(12, 112);
            this.rdoSqlite.Name = "rdoSqlite";
            this.rdoSqlite.Size = new System.Drawing.Size(122, 17);
            this.rdoSqlite.TabIndex = 21;
            this.rdoSqlite.TabStop = true;
            this.rdoSqlite.Text = "SQLite 3 (included) *";
            this.rdoSqlite.UseVisualStyleBackColor = true;
            this.rdoSqlite.CheckedChanged += new System.EventHandler(this.rdoSqlite_CheckedChanged);
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(226, 21);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(86, 13);
            this.linkLabel3.TabIndex = 20;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "More Information";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(226, 45);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(86, 13);
            this.linkLabel2.TabIndex = 19;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "More Information";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(226, 70);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(86, 13);
            this.linkLabel1.TabIndex = 18;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "More Information";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // lnkPostgreSQLInfo
            // 
            this.lnkPostgreSQLInfo.AutoSize = true;
            this.lnkPostgreSQLInfo.Location = new System.Drawing.Point(226, 91);
            this.lnkPostgreSQLInfo.Name = "lnkPostgreSQLInfo";
            this.lnkPostgreSQLInfo.Size = new System.Drawing.Size(86, 13);
            this.lnkPostgreSQLInfo.TabIndex = 17;
            this.lnkPostgreSQLInfo.TabStop = true;
            this.lnkPostgreSQLInfo.Text = "More Information";
            this.lnkPostgreSQLInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPostgreSQLInfo_LinkClicked);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(29, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(283, 34);
            this.label2.TabIndex = 16;
            this.label2.Text = "* GRIN-Global has not been fully regression tested with this database engine and " +
                "may exhibit problems";
            // 
            // rdoPostgreSQL
            // 
            this.rdoPostgreSQL.AutoSize = true;
            this.rdoPostgreSQL.Location = new System.Drawing.Point(12, 89);
            this.rdoPostgreSQL.Name = "rdoPostgreSQL";
            this.rdoPostgreSQL.Size = new System.Drawing.Size(142, 17);
            this.rdoPostgreSQL.TabIndex = 15;
            this.rdoPostgreSQL.TabStop = true;
            this.rdoPostgreSQL.Text = "PostgreSQL 8.3 or later *";
            this.rdoPostgreSQL.UseVisualStyleBackColor = true;
            this.rdoPostgreSQL.CheckedChanged += new System.EventHandler(this.rdoPostgreSQL_CheckedChanged);
            // 
            // rdoOracle
            // 
            this.rdoOracle.AutoSize = true;
            this.rdoOracle.Location = new System.Drawing.Point(12, 66);
            this.rdoOracle.Name = "rdoOracle";
            this.rdoOracle.Size = new System.Drawing.Size(136, 17);
            this.rdoOracle.TabIndex = 14;
            this.rdoOracle.TabStop = true;
            this.rdoOracle.Text = "Oracle XE 10g or later *";
            this.rdoOracle.UseVisualStyleBackColor = true;
            this.rdoOracle.CheckedChanged += new System.EventHandler(this.rdoOracle_CheckedChanged);
            // 
            // rdoMySql
            // 
            this.rdoMySql.AutoSize = true;
            this.rdoMySql.Location = new System.Drawing.Point(12, 43);
            this.rdoMySql.Name = "rdoMySql";
            this.rdoMySql.Size = new System.Drawing.Size(120, 17);
            this.rdoMySql.TabIndex = 13;
            this.rdoMySql.TabStop = true;
            this.rdoMySql.Text = "MySQL 5.1 or later *";
            this.rdoMySql.UseVisualStyleBackColor = true;
            this.rdoMySql.CheckedChanged += new System.EventHandler(this.rdoMySql_CheckedChanged);
            // 
            // lnkPostgreSQL
            // 
            this.lnkPostgreSQL.AutoSize = true;
            this.lnkPostgreSQL.Location = new System.Drawing.Point(165, 91);
            this.lnkPostgreSQL.Name = "lnkPostgreSQL";
            this.lnkPostgreSQL.Size = new System.Drawing.Size(55, 13);
            this.lnkPostgreSQL.TabIndex = 12;
            this.lnkPostgreSQL.TabStop = true;
            this.lnkPostgreSQL.Text = "Download";
            this.lnkPostgreSQL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPostgreSQL_LinkClicked_1);
            // 
            // lnkOracle
            // 
            this.lnkOracle.AutoSize = true;
            this.lnkOracle.Location = new System.Drawing.Point(165, 68);
            this.lnkOracle.Name = "lnkOracle";
            this.lnkOracle.Size = new System.Drawing.Size(55, 13);
            this.lnkOracle.TabIndex = 11;
            this.lnkOracle.TabStop = true;
            this.lnkOracle.Text = "Download";
            this.lnkOracle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOracle_LinkClicked_1);
            // 
            // lnkMySQL
            // 
            this.lnkMySQL.AutoSize = true;
            this.lnkMySQL.Location = new System.Drawing.Point(165, 45);
            this.lnkMySQL.Name = "lnkMySQL";
            this.lnkMySQL.Size = new System.Drawing.Size(55, 13);
            this.lnkMySQL.TabIndex = 10;
            this.lnkMySQL.TabStop = true;
            this.lnkMySQL.Text = "Download";
            this.lnkMySQL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMySQL_LinkClicked_1);
            // 
            // lnkSqlServer
            // 
            this.lnkSqlServer.AutoSize = true;
            this.lnkSqlServer.Location = new System.Drawing.Point(165, 21);
            this.lnkSqlServer.Name = "lnkSqlServer";
            this.lnkSqlServer.Size = new System.Drawing.Size(55, 13);
            this.lnkSqlServer.TabIndex = 2;
            this.lnkSqlServer.TabStop = true;
            this.lnkSqlServer.Text = "Download";
            this.lnkSqlServer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSqlServer_LinkClicked);
            // 
            // rdoSqlServer
            // 
            this.rdoSqlServer.AutoSize = true;
            this.rdoSqlServer.Location = new System.Drawing.Point(12, 19);
            this.rdoSqlServer.Name = "rdoSqlServer";
            this.rdoSqlServer.Size = new System.Drawing.Size(142, 17);
            this.rdoSqlServer.TabIndex = 0;
            this.rdoSqlServer.TabStop = true;
            this.rdoSqlServer.Text = "SQL Server 2008 or later";
            this.rdoSqlServer.UseVisualStyleBackColor = true;
            this.rdoSqlServer.CheckedChanged += new System.EventHandler(this.rdoSqlServer_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 383);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(304, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "You will be prompted later for additional connection information.";
            // 
            // lblRemote
            // 
            this.lblRemote.Location = new System.Drawing.Point(52, 307);
            this.lblRemote.Name = "lblRemote";
            this.lblRemote.Size = new System.Drawing.Size(323, 33);
            this.lblRemote.TabIndex = 14;
            this.lblRemote.Text = "Your network and remote server must be configured properly.\r\nSee your database en" +
                "gine\'s documentation for details.";
            this.lblRemote.Visible = false;
            // 
            // rdoOverride
            // 
            this.rdoOverride.Location = new System.Drawing.Point(32, 343);
            this.rdoOverride.Name = "rdoOverride";
            this.rdoOverride.Size = new System.Drawing.Size(343, 17);
            this.rdoOverride.TabIndex = 15;
            this.rdoOverride.Text = "My database engine is already installed but is not detected properly";
            this.rdoOverride.UseVisualStyleBackColor = true;
            this.rdoOverride.CheckedChanged += new System.EventHandler(this.rdoOverride_CheckedChanged);
            // 
            // frmDatabaseEnginePrompt2
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(427, 444);
            this.Controls.Add(this.rdoOverride);
            this.Controls.Add(this.lblRemote);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.gbLocal);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblLocal);
            this.Controls.Add(this.rdoRemote);
            this.Controls.Add(this.rdoLocal);
            this.Controls.Add(this.lblInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDatabaseEnginePrompt2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Specify Database Engine for GRIN-Global";
            this.Load += new System.EventHandler(this.frmDatabaseEnginePrompt_Load);
            this.gbLocal.ResumeLayout(false);
            this.gbLocal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoLocal;
        private System.Windows.Forms.RadioButton rdoRemote;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblLocal;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbLocal;
        private System.Windows.Forms.LinkLabel lnkSqlServer;
        private System.Windows.Forms.RadioButton rdoSqlServer;
        private System.Windows.Forms.LinkLabel lnkPostgreSQL;
        private System.Windows.Forms.LinkLabel lnkOracle;
        private System.Windows.Forms.LinkLabel lnkMySQL;
        private System.Windows.Forms.RadioButton rdoMySql;
        private System.Windows.Forms.RadioButton rdoOracle;
        private System.Windows.Forms.RadioButton rdoPostgreSQL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lnkPostgreSQLInfo;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel lnkSQLite;
        private System.Windows.Forms.RadioButton rdoSqlite;
        private System.Windows.Forms.Label lblRemote;
        private System.Windows.Forms.RadioButton rdoOverride;
    }
}