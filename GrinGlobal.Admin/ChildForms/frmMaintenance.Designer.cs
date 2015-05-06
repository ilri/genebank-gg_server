namespace GrinGlobal.Admin.ChildForms {
    partial class frmMaintenance {
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Import Wizard",
            "A utility for mass-loading data into the GRIN-Global database"}, 0);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Code Groups",
            "Edit the values that appear as dropdowns in the Curator Tool"}, 1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMaintenance));
            this.lv = new System.Windows.Forms.ListView();
            this.colFunction = new System.Windows.Forms.ColumnHeader();
            this.colDescription = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // lv
            // 
            this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFunction,
            this.colDescription});
            this.lv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv.FullRowSelect = true;
            listViewItem1.Tag = "ndImportWizard";
            listViewItem1.ToolTipText = "A utility to mass-load data into GRIN-Global";
            listViewItem2.Tag = "ndCodeGroups";
            this.lv.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.lv.Location = new System.Drawing.Point(0, 0);
            this.lv.MultiSelect = false;
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(460, 264);
            this.lv.SmallImageList = this.imageList1;
            this.lv.TabIndex = 4;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            this.lv.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_MouseDoubleClick);
            this.lv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lv_KeyPress);
            // 
            // colFunction
            // 
            this.colFunction.Text = "Function";
            this.colFunction.Width = 100;
            // 
            // colDescription
            // 
            this.colDescription.Text = "Description";
            this.colDescription.Width = 400;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "sysIM.ico");
            this.imageList1.Images.SetKeyName(1, "sysCG.ico");
            // 
            // frmMaintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(460, 264);
            this.Controls.Add(this.lv);
            this.Name = "frmMaintenance";
            this.Text = "Maintenance";
            this.Load += new System.EventHandler(this.frmMaintenance_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lv;
        private System.Windows.Forms.ColumnHeader colFunction;
        private System.Windows.Forms.ColumnHeader colDescription;
        private System.Windows.Forms.ImageList imageList1;
    }
}
