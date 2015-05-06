using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GrinGlobal.ConfigurationWizard {
    public partial class frmInstallType : GrinGlobal.ConfigurationWizard.frmMain {
        public frmInstallType() {
            InitializeComponent();
        }

        private void frmInstallType_Load(object sender, EventArgs e) {
            setButtons();
        }

        private void setButtons() {
            syncButtons(true, true, chkServerApps.Checked || chkClientApps.Checked, false);
        }

        private void chkServerApps_CheckedChanged(object sender, EventArgs e) {
            setButtons();
        }

        private void chkClientApps_CheckedChanged(object sender, EventArgs e) {
            setButtons();
        }

        private void btnNext_Click(object sender, EventArgs e) {

        }

    }
}
