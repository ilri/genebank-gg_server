using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GrinGlobal.ConfigurationWizard {
    public partial class frmSplash : GrinGlobal.ConfigurationWizard.frmMain {
        public frmSplash() {
            InitializeComponent();
        }

        private void frmSplash_Load(object sender, EventArgs e) {
            syncButtons(true, false, true, false);
        }

    }
}
