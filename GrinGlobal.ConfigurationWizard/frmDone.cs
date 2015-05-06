using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrinGlobal.ConfigurationWizard {
    public partial class frmDone : Form {
        public frmDone() {
            InitializeComponent();
        }

        public void ShowDialog(string errorMessage, string loggedText, IWin32Window owner) {
            if (String.IsNullOrEmpty(errorMessage) || errorMessage.ToLower() == "success") {
                lblError.Text = "No errors occurred during configuration.";
                lblError.ForeColor = Color.Black;
            } else {
                lblError.Text = errorMessage;
                lblError.ForeColor = Color.Red;
            }
            txtLog.Text = loggedText;
            this.ShowDialog(Owner);
        }

        private void btnDone_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
