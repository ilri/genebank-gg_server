using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.InstallHelper;
using System.IO;

namespace GrinGlobal.Updater {
    public partial class frmInstallMode : Form {
        public frmInstallMode() {
            InitializeComponent();
        }

        private bool _showPrepopulate;

        public DialogResult ShowDialog(IWin32Window owner, bool showPrepopulateCheckBox)
        {
            _showPrepopulate = showPrepopulateCheckBox;
            return ShowDialog(owner);
        }

        private void frmCustomInstall_Load(object sender, EventArgs e) {
            chkPrepopulate.Checked = _showPrepopulate;
            chkPrepopulate.Visible = _showPrepopulate;
            chkPrepopulate.Enabled = _showPrepopulate;

            var updaterFolder = Utility.GetTargetDirectory(null, null, "Updater");
            var parent = new DirectoryInfo(updaterFolder).Parent;
            if (parent != null){
                lblTarget.Text = parent.FullName;
            } else {
                lblTarget.Text = updaterFolder;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
