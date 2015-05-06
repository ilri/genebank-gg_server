using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core.DataManagers;
using GrinGlobal.Core;

namespace GrinGlobal.InstallHelper {
    public partial class frmEnableSqlServerLogin : Form {
        public frmEnableSqlServerLogin() {
            InitializeComponent();
        }

        public SqlServerEngineUtil SqlUtil;

        private void txtPassword_TextChanged(object sender, EventArgs e) {
            btnEnableLogin.Enabled = txtPassword.Text.Length > 0;
        }

        private void btnEnableLogin_Click(object sender, EventArgs e) {

            var splash = new frmSplash();
            try {
                splash.Show("Enabling SQL Server Login Authentication...", false, this);
                var useWinAuth = SqlUtil.UseWindowsAuthentication;
                try {
                    SqlUtil.UseWindowsAuthentication = true;
                    SqlUtil.EnableMixedModeIfNeeded(SqlUtil.InstanceName);

                    splash.ChangeText(getDisplayMember("enableLogin{setting}", "Setting the {0} password...", "SA"));
                    SqlUtil.ChangePassword(null, "master", "sa", "", txtPassword.Text);

                    splash.ChangeText(getDisplayMember("enableLogin{enabling}", "Enabling {0} user...", "SA"));
                    SqlUtil.EnableUser(null, "master", "sa");

                    splash.ChangeText(getDisplayMember("enableLogin{stopping}", "Stopping SQL Server..."));
                    SqlUtil.StopService();
                    splash.ChangeText(getDisplayMember("enableLogin{starting}", "Starting SQL Server..."));
                    SqlUtil.StartService();

                    splash.Close();
                    MessageBox.Show(this, getDisplayMember("enableLogin{success_body}", "SQL Server Login is now enabled."), 
                        getDisplayMember("enableLogin{success_title}", "Success"));
                } catch (Exception ex) {
                    splash.Close();
                    MessageBox.Show(this, 
                        getDisplayMember("enableLogin{failed_body}", "Error enabling SQL Server login: {0}", ex.Message), 
                        getDisplayMember("enableLogin{failed_title}", "Error"));
                    txtPassword.Focus();
                    return;
                } finally {
                    SqlUtil.UseWindowsAuthentication = useWinAuth;
                }
            } finally {
                splash.Close();
            }


            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "InstallHelper", "frmEnableSqlServerLogin", resourceName, null, defaultValue, substitutes);
        }

    }
}
