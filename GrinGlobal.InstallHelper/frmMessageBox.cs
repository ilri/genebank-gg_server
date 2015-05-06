using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrinGlobal.InstallHelper {
    public partial class frmMessageBox : Form {
        public frmMessageBox() {
            InitializeComponent();
        }

        private void btnYes_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void btnNo_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.No;
            Close();
        }

        private void frmMessageBox_Load(object sender, EventArgs e) {
            txtMessage.SelectionStart = 0;
            txtMessage.SelectionLength = 0;
            if (this.Owner != null) {
                this.Icon = this.Owner.Icon;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //MessageBox.Show(this, Utility.GetApplicationVersion("GRIN-Global Updater"), "GRIN-Global Updater");
            new frmDisclaimer().ShowDialog(this);
        }
    }
}
