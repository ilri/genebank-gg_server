using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.InstallHelper;

namespace GrinGlobal.Updater {
    public partial class frmExpand : Form {
        public frmExpand() {
            InitializeComponent();
        }

        private void syncGui() {
            if (rdoExpand.Checked) {
                txtExpandTo.Enabled = true;
                btnSelectFolder.Enabled = true;
                chkLaunchSetup.Enabled = true;
                chkEmptyDirectory.Enabled = true;
            } else {
                txtExpandTo.Enabled = false;
                btnSelectFolder.Enabled = false;
                chkLaunchSetup.Enabled = false;
                chkEmptyDirectory.Enabled = false;
            }
        }

        private void rdoExpand_CheckedChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void rdoExplorer_CheckedChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void frmExpand_Load(object sender, EventArgs e) {
            if (txtExpandTo.Text == "") {
                txtExpandTo.Text = Utility.GetTempDirectory(1500);
            }
            syncGui();
        }

        private void btnOK_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e) {
            folderBrowserDialog1.Description = "Select Destination Folder";
            folderBrowserDialog1.ShowNewFolderButton = true;
            folderBrowserDialog1.SelectedPath = txtExpandTo.Text;
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog(this)) {
                txtExpandTo.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
