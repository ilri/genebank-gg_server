using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrinGlobal.ConfigurationWizard {
    public partial class frmMain : Form {
        public frmMain() {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e) {

        }

        protected void syncButtons(bool cancelEnabled, bool previousEnabled, bool nextEnabled, bool finishEnabled){
            btnCancel.Enabled = cancelEnabled;
            btnPrevious.Enabled = previousEnabled;
            btnNext.Enabled = nextEnabled;
            btnFinish.Enabled = finishEnabled;
        }

        private void btnNext_Click(object sender, EventArgs e) {
            Program.NextStep(this);
        }

        private void btnPrevious_Click(object sender, EventArgs e) {
            Program.PreviousStep(this);
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Program.Cancel(this);
        }

        private void btnFinish_Click(object sender, EventArgs e) {
            Program.Finish(this);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = Program.Cancel(this);
        }

    }
}
