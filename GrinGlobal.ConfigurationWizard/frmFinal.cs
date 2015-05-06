using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GrinGlobal.ConfigurationWizard {
    public partial class frmFinal : GrinGlobal.ConfigurationWizard.frmMain {
        public frmFinal() {
            InitializeComponent();
        }

        private void frmFinal_Load(object sender, EventArgs e) {
            setButtons();
        }

        private void setButtons() {
            syncButtons(true, true, false, true);
        }
    }
}
