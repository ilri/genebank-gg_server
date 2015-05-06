namespace GrinGlobal.Admin.ChildForms
{
    partial class frmMappings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMappings));
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.clbTables = new System.Windows.Forms.CheckedListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.cmTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.cmTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(13, 25);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(71, 17);
            this.chkAll.TabIndex = 0;
            this.chkAll.Text = "Check All";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tables in Database";
            // 
            // clbTables
            // 
            this.clbTables.CheckOnClick = true;
            this.clbTables.ColumnWidth = 200;
            this.clbTables.FormattingEnabled = true;
            this.clbTables.HorizontalScrollbar = true;
            this.clbTables.Location = new System.Drawing.Point(13, 48);
            this.clbTables.MultiColumn = true;
            this.clbTables.Name = "clbTables";
            this.clbTables.Size = new System.Drawing.Size(584, 364);
            this.clbTables.TabIndex = 2;
            this.clbTables.ThreeDCheckBoxes = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(464, 432);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(144, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "&Recreate Table Mappings";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cmTable
            // 
            this.cmTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiRefresh,
            this.cmiProperties});
            this.cmTable.Name = "ctxMenuTableMappings";
            this.cmTable.Size = new System.Drawing.Size(124, 48);
            // 
            // cmiRefresh
            // 
            this.cmiRefresh.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cmiRefresh.Name = "cmiRefresh";
            this.cmiRefresh.Size = new System.Drawing.Size(123, 22);
            this.cmiRefresh.Text = "&Refresh";
            this.cmiRefresh.Click += new System.EventHandler(this.refreshTableMappingsMenuItem_Click);
            // 
            // cmiProperties
            // 
            this.cmiProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmiProperties.Name = "cmiProperties";
            this.cmiProperties.Size = new System.Drawing.Size(123, 22);
            this.cmiProperties.Text = "&Property";
            this.cmiProperties.Click += new System.EventHandler(this.defaultTableMappingsMenuItem_Click);
            // 
            // frmMappings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 467);
            this.Controls.Add(this.clbTables);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMappings";
            this.Text = "Table Mappings";
            this.Load += new System.EventHandler(this.frmMappings_Load);
            this.cmTable.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox clbTables;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ContextMenuStrip cmTable;
        private System.Windows.Forms.ToolStripMenuItem cmiRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiProperties;
    }
}