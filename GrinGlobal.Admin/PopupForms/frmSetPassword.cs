using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Admin.ChildForms;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmSetPassword : frmBase {
        public frmSetPassword() {
            InitializeComponent();  tellBaseComponents(components);
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e) {
            txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
        }

        public string Password;

        private void btnChange_Click(object sender, EventArgs e) {
            Password = txtPassword.Text;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Password = null;
            this.Close();
        }

        public DialogResult ShowDialog(IWin32Window parent, string username) {
            lblPassword.Text = "New password for " + username + ":";
            return this.ShowDialog(parent);
        }

        private void frmChangePassword_Load(object sender, EventArgs e) {
        }

        private void frmChangePassword_Activated(object sender, EventArgs e) {
            txtPassword.Focus();
            txtPassword.SelectAll();
        }

    }
}
