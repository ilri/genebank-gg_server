namespace GrinGlobal.Search.Engine.Tester {
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
            this.components = new System.ComponentModel.Container();
            this.btnToggle = new System.Windows.Forms.Button();
            this.btnPing = new System.Windows.Forms.Button();
            this.ddlConnectUsing = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lvLog = new System.Windows.Forms.ListView();
            this.colDate = new System.Windows.Forms.ColumnHeader();
            this.colType = new System.Windows.Forms.ColumnHeader();
            this.colMessage = new System.Windows.Forms.ColumnHeader();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chkDGVRightToLeft = new System.Windows.Forms.CheckBox();
            this.chkMirrored = new System.Windows.Forms.CheckBox();
            this.btnTest5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnToggle
            // 
            this.btnToggle.Location = new System.Drawing.Point(13, 13);
            this.btnToggle.Name = "btnToggle";
            this.btnToggle.Size = new System.Drawing.Size(106, 23);
            this.btnToggle.TabIndex = 0;
            this.btnToggle.Text = "Start Hosting";
            this.btnToggle.UseVisualStyleBackColor = true;
            this.btnToggle.Click += new System.EventHandler(this.btnToggle_Click);
            // 
            // btnPing
            // 
            this.btnPing.Location = new System.Drawing.Point(366, 13);
            this.btnPing.Name = "btnPing";
            this.btnPing.Size = new System.Drawing.Size(75, 23);
            this.btnPing.TabIndex = 1;
            this.btnPing.Text = "Ping";
            this.btnPing.UseVisualStyleBackColor = true;
            this.btnPing.Visible = false;
            this.btnPing.Click += new System.EventHandler(this.btnPing_Click);
            // 
            // ddlConnectUsing
            // 
            this.ddlConnectUsing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlConnectUsing.FormattingEnabled = true;
            this.ddlConnectUsing.Items.AddRange(new object[] {
            "http",
            "net.tcp",
            "net.pipe"});
            this.ddlConnectUsing.Location = new System.Drawing.Point(239, 12);
            this.ddlConnectUsing.Name = "ddlConnectUsing";
            this.ddlConnectUsing.Size = new System.Drawing.Size(121, 21);
            this.ddlConnectUsing.TabIndex = 2;
            this.ddlConnectUsing.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(141, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Connect using:";
            this.label1.Visible = false;
            // 
            // lvLog
            // 
            this.lvLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDate,
            this.colType,
            this.colMessage});
            this.lvLog.Location = new System.Drawing.Point(13, 43);
            this.lvLog.Name = "lvLog";
            this.lvLog.Size = new System.Drawing.Size(693, 352);
            this.lvLog.TabIndex = 4;
            this.lvLog.UseCompatibleStateImageBehavior = false;
            this.lvLog.View = System.Windows.Forms.View.Details;
            // 
            // colDate
            // 
            this.colDate.Text = "Date";
            this.colDate.Width = 130;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            this.colType.Width = 80;
            // 
            // colMessage
            // 
            this.colMessage.Text = "Message";
            this.colMessage.Width = 470;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(629, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(489, 8);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 6;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(144, 37);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(479, 206);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.Visible = false;
            // 
            // chkDGVRightToLeft
            // 
            this.chkDGVRightToLeft.AutoSize = true;
            this.chkDGVRightToLeft.Location = new System.Drawing.Point(144, 249);
            this.chkDGVRightToLeft.Name = "chkDGVRightToLeft";
            this.chkDGVRightToLeft.Size = new System.Drawing.Size(80, 17);
            this.chkDGVRightToLeft.TabIndex = 8;
            this.chkDGVRightToLeft.Text = "Right-to-left";
            this.chkDGVRightToLeft.UseVisualStyleBackColor = true;
            this.chkDGVRightToLeft.Visible = false;
            this.chkDGVRightToLeft.CheckedChanged += new System.EventHandler(this.chkDGVRightToLeft_CheckedChanged);
            // 
            // chkMirrored
            // 
            this.chkMirrored.AutoSize = true;
            this.chkMirrored.Location = new System.Drawing.Point(259, 249);
            this.chkMirrored.Name = "chkMirrored";
            this.chkMirrored.Size = new System.Drawing.Size(52, 17);
            this.chkMirrored.TabIndex = 9;
            this.chkMirrored.Text = "Mirror";
            this.chkMirrored.UseVisualStyleBackColor = true;
            this.chkMirrored.Visible = false;
            this.chkMirrored.CheckedChanged += new System.EventHandler(this.chkMirrored_CheckedChanged);
            // 
            // btnTest5
            // 
            this.btnTest5.Location = new System.Drawing.Point(509, 358);
            this.btnTest5.Name = "btnTest5";
            this.btnTest5.Size = new System.Drawing.Size(130, 23);
            this.btnTest5.TabIndex = 10;
            this.btnTest5.Text = "Create gg db";
            this.btnTest5.UseVisualStyleBackColor = true;
            this.btnTest5.Visible = false;
            this.btnTest5.Click += new System.EventHandler(this.btnTest5_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 407);
            this.Controls.Add(this.btnTest5);
            this.Controls.Add(this.chkMirrored);
            this.Controls.Add(this.chkDGVRightToLeft);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lvLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ddlConnectUsing);
            this.Controls.Add(this.btnPing);
            this.Controls.Add(this.btnToggle);
            this.Name = "Form1";
            this.Text = "GRIN-Global Search Engine Test App";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnToggle;
		private System.Windows.Forms.Button btnPing;
		private System.Windows.Forms.ComboBox ddlConnectUsing;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView lvLog;
		private System.Windows.Forms.ColumnHeader colDate;
		private System.Windows.Forms.ColumnHeader colType;
		private System.Windows.Forms.ColumnHeader colMessage;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox chkDGVRightToLeft;
        private System.Windows.Forms.CheckBox chkMirrored;
        private System.Windows.Forms.Button btnTest5;
	}
}

