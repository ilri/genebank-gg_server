using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using System.Diagnostics;
using Microsoft.Win32;
using GrinGlobal.InstallHelper;

namespace GrinGlobal.DatabaseCopier {
    public partial class frmLogin : Form {
        public frmLogin() {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;

        }

        private DatabaseEngineUtil _engineUtil;


        public string SuperUserPassword;
        public bool UseWindowsAuthentication {
            get {
                return chkWindowsAuthentication.Checked && chkWindowsAuthentication.Enabled;
            }
        }
        public string InstanceName;

        private string _engine;
        public string Engine {
            get {
                return _engine;
            }
            set {
                _engine = value;

//                MessageBox.Show("engine=" + _engine + "\nIs valid?" + (DatabaseEngineUtil.IsValidEngine(_engine) ? "true" : "false"));

                _engineUtil = DatabaseEngineUtil.CreateInstance(_engine, Utility.GetTargetDirectory(null, null, "Database") + @"\gguac.exe", this.InstanceName);

                lblTitle.Text = "To create the GRIN-Global Database, the " + _engineUtil.SuperUserName + " password is required.";
                if (_engineUtil.EngineName.ToLower() == "sqlserver") {
                    lblTitle.Text += "\nAlternatively, you may authenticate using the following Windows login:\n" + Environment.UserDomainName + @"\" + Environment.UserName;
                    chkWindowsAuthentication.Checked = true;
                    chkWindowsAuthentication.Enabled = true;
                } else {
                    chkWindowsAuthentication.Checked = false;
                    chkWindowsAuthentication.Enabled = false;
                }
                lblPassword.Text = _engineUtil.SuperUserName + " password:";
                this.Text = "Enter " + _engineUtil.SuperUserName + " Password";
            }
        }

        private string testLogin(string password) {

            _engineUtil.UseWindowsAuthentication = chkWindowsAuthentication.Checked;
            string failedReason = _engineUtil.TestLogin(password);
            return failedReason;

        }
        private void btnLogin_Click(object sender, EventArgs e) {

            try {

                string reason = testLogin(txtPassword.Text);
                if (!String.IsNullOrEmpty(reason)) {
                    if (chkWindowsAuthentication.Checked) {
                        MessageBox.Show("Windows authentication failed.  Reason:\n\n" + reason + "\n\nPlease try again or authenticate by specifying the sa password.");
                    } else {
                        MessageBox.Show("The password appears to be incorrect.  Please try again.\n\n " + reason);
                    }
                } else {
                    // we get here, login suceeded
                    SuperUserPassword = txtPassword.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
            } catch (Exception ex) {
                MessageBox.Show("The password appears to be incorrect.  Please try again.\n\nError: " + ex.Message);
            }
            if (txtPassword.Text.Length > 0) {
                txtPassword.SelectionStart = 0;
                txtPassword.SelectionLength = txtPassword.Text.Length;
            }
            txtPassword.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e) {

        }

        public DialogResult ShowDialog(string engine, string instanceName, bool windowsAuth) {

            this.Engine = engine;
            
            return this.ShowDialog();
        }

        private void chkWindowsAuthentication_CheckedChanged(object sender, EventArgs e) {
            if (chkWindowsAuthentication.Checked){
                txtPassword.Enabled = false;
                lblPassword.Enabled = false;
                btnLogin.Text = "Continue";
            } else {
                txtPassword.Enabled = true;
                lblPassword.Enabled = true;
                btnLogin.Text = "Login";
            }
        }

        private void lblTitle_Click(object sender, EventArgs e) {
            MessageBox.Show(this.Engine);
        }
    }
}
