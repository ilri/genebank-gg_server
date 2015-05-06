using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Business;
using System.Security.Authentication;

using System.ServiceModel;
using System.Diagnostics;
using GrinGlobal.Core;

namespace GrinGlobal.Import {
    public partial class frmLogin : System.Windows.Forms.Form {

        public frmLogin() {
            InitializeComponent();
        }

        internal HostProxy HostProxy;


        private bool _syncing;
        public delegate void VoidCallback();
        /// <summary>
        /// Used to suppress GUI synchronization and event handling when an event is in the process of being handled.  (aka prevent controls from cross-firing events)
        /// </summary>
        /// <param name="callback"></param>
        protected void Sync(bool showCursor, VoidCallback callback) {
            if (!_syncing) {
                try {
                    _syncing = true;
                    if (showCursor) {
                        using (new AutoCursor(this)) {
                            callback();
                        }
                    } else {
                        callback();
                    }
                } finally {
                    _syncing = false;
                }
            }
        }

        private void frmLogin_Load(object sender, EventArgs e) {
            //ddlDatabaseEngine.Items.AddRange(new string[] { "SQL Server", "MySQL", "Oracle", "PostgreSql" });
            if (ddlDatabaseEngine.SelectedIndex < 0) {
                ddlDatabaseEngine.SelectedIndex = ddlDatabaseEngine.FindString("SQL Server");
            }
            syncGUI();
        }

        public void RefreshData() {
            Sync(false, delegate() {
                if (HostProxy != null) {
                    this.Text = HostProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
                    for (int i = 0; i < ddlDatabaseEngine.Items.Count; i++) {
                        var engine = getEngineName(ddlDatabaseEngine.Items[i] as string);
                        if (engine.ToLower() == HostProxy.Connection.DatabaseEngineProviderName) {
                            ddlDatabaseEngine.SelectedIndex = i;
                            break;
                        }
                    }

                    chkWindowsAuthentication.Checked = HostProxy.Connection.UseWindowsAuthentication;

                    txtDatabaseEngineServerName.Text = HostProxy.Connection.DatabaseEngineServerName;
                    txtDatabaseEngineUserName.Text = HostProxy.Connection.DatabaseEngineUserName;
                    //chkDatabaseEngineRememberPassword.Checked = HostProxy.Connection.DatabaseEngineRememberPassword;
                    //if (chkDatabaseEngineRememberPassword.Checked) {
                    //    txtDatabaseEnginePassword.Text = HostProxy.Connection.DatabaseEnginePassword;
                    //} else {
                        txtDatabaseEnginePassword.Text = "";
                    //}

                    rdoWebService.Checked = !String.IsNullOrEmpty(HostProxy.Connection.GrinGlobalUrl);

                    txtGrinGlobalUrl.Text = HostProxy.Connection.GrinGlobalUrl;
                    txtGrinGlobalUserName.Text = HostProxy.Connection.GrinGlobalUserName;
                    //chkGrinGlobalRememberPassword.Checked = HostProxy.Connection.GrinGlobalRememberPassword;
                    //if (chkGrinGlobalRememberPassword.Checked) {
                    //    txtGrinGlobalPassword.Text = HostProxy.Connection.GrinGlobalPassword;
                    //} else {
                        txtGrinGlobalPassword.Text = "";
                    //}
                }

                btnCancel.Visible = Modal;
                if (!Modal) {
                    btnConnect.Text = "Save";
                }
            });

        }

        private void ddlDatabaseEngine_SelectedIndexChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void chkWindowsAuthentication_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }
        private void syncGUI(){
            Sync(false, delegate() {
                gbDatabaseConnection.Enabled = rdoDatabase.Checked;
                gbWebService.Enabled = !rdoDatabase.Checked;


                if (ddlDatabaseEngine.SelectedIndex > -1) {
                    if (!ddlDatabaseEngine.SelectedItem.ToString().ToLower().Contains("sql server")) {
                        chkWindowsAuthentication.Checked = false;
                    }
                }

                chkWindowsAuthentication.Enabled = ddlDatabaseEngine.SelectedIndex == 0;

                txtDatabaseEnginePassword.Enabled =
                    lblPassword.Enabled =
                    txtDatabaseEngineUserName.Enabled =
                    lblUsername.Enabled =
                                        !chkWindowsAuthentication.Checked;

                    //chkDatabaseEngineRememberPassword.Enabled =
                    //!chkWindowsAuthentication.Checked;
            });
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private ConnectionInfo _conn;

        private string getEngineName() {
            return getEngineName(ddlDatabaseEngine.Text);
        }
        private string getEngineName(string text) {
            var txt = text.ToLower();
            if (txt.Contains("sql server")) {
                return "sqlserver";
            } else if (txt.Contains("postgre")) {
                return "postgresql";
            } else if (txt.Contains("mysql")) {
                return "mysql";
            } else if (txt.Contains("oracle")) {
                return "oracle";
            } else {
                throw new InvalidOperationException(getDisplayMember("getEngineName", "ddlDatabaseEngine contains an option that is not mapped in getEngineName(): {0}" + text));
            }
        }

        public DialogResult ShowDialog(IWin32Window owner, ConnectionInfo ci) {
            _conn = ci;

            Sync(false, delegate() {
                switch (_conn.DatabaseEngineProviderName) {
                    case "sqlserver":
                        ddlDatabaseEngine.SelectedIndex = ddlDatabaseEngine.FindString("SQL Server");
                        break;
                    case "mysql":
                        ddlDatabaseEngine.SelectedIndex = ddlDatabaseEngine.FindString("MySQL");
                        break;
                    case "postgresql":
                        ddlDatabaseEngine.SelectedIndex = ddlDatabaseEngine.FindString("Postgre");
                        break;
                    case "oracle":
                        ddlDatabaseEngine.SelectedIndex = ddlDatabaseEngine.FindString("Oracle");
                        break;
                }

                txtDatabaseEngineUserName.Text = _conn.DatabaseEngineUserName;
                txtDatabaseEnginePassword.Text = _conn.DatabaseEnginePassword;
                // chkDatabaseEngineRememberPassword.Checked = _conn.DatabaseEngineRememberPassword;
                chkWindowsAuthentication.Checked = _conn.UseWindowsAuthentication;
                txtDatabaseEngineServerName.Text = _conn.DatabaseEngineServerName;

                txtGrinGlobalUserName.Text = _conn.GrinGlobalUserName;
                txtGrinGlobalPassword.Text = _conn.GrinGlobalPassword;
                txtGrinGlobalUrl.Text = _conn.GrinGlobalUrl;
            });

            return this.ShowDialog(owner);
        }

        private void syncConnectionInfoToGUI(string loginToken) {
            if (_conn == null) {
                _conn = new ConnectionInfo();
            }
            _conn.DatabaseEngineProviderName = getEngineName();
            _conn.DatabaseEngineDatabaseName = "gringlobal";
            _conn.DatabaseEngineUserName = txtDatabaseEngineUserName.Text;
            _conn.DatabaseEnginePassword = txtDatabaseEnginePassword.Text;
            _conn.DatabaseEngineServerName = txtDatabaseEngineServerName.Text;
            _conn.UseWindowsAuthentication = chkWindowsAuthentication.Checked;
            // _conn.DatabaseEngineRememberPassword = chkDatabaseEngineRememberPassword.Checked;

            //if (rdoWebService.Checked) {
            //    _conn.GrinGlobalUrl = txtGrinGlobalUrl.Text;
            //} else {
            _conn.GrinGlobalUrl = null;
            //}
            _conn.UseWebService = false;
            _conn.GrinGlobalUserName = txtGrinGlobalUserName.Text;
            _conn.GrinGlobalPassword = txtGrinGlobalPassword.Text;
            _conn.GrinGlobalLoginToken = loginToken;
            // _conn.GrinGlobalRememberPassword = chkGrinGlobalRememberPassword.Checked;

        }

        private string autoFormatUrl(string origUrl) {
            string newUrl = origUrl;
            if (!newUrl.StartsWith("http://")) {
                newUrl = "http://" + newUrl;
            }
            if (!newUrl.Contains("adminservice.svc")) {
                if (newUrl.EndsWith("/")) {
                    newUrl += "adminservice.svc";
                } else {
                    newUrl += "/adminservice.svc";
                }
            }
            if (!newUrl.Contains("/gringlobal")) {
                newUrl = newUrl.Replace("/adminservice.svc", "/gringlobal/adminservice.svc");
            }
            return newUrl;
        }

        private void btnConnect_Click(object sender, EventArgs e) {
            try {

                if (rdoWebService.Checked) {
                    txtGrinGlobalUrl.Text = autoFormatUrl(txtGrinGlobalUrl.Text);
                } else {
                }
                syncConnectionInfoToGUI(null);
                var proxy = new HostProxy(_conn);
                var connReason = proxy.TestDatabaseConnection();
                if (!String.IsNullOrEmpty(connReason)) {
                    MessageBox.Show(this, getDisplayMember("connect{failed_body}", "Connection failed: {0}", connReason), 
                        getDisplayMember("connect{failed_title}", "Connection Failed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    string login = proxy.Login();
                    if (String.IsNullOrEmpty(login)) {
                        MessageBox.Show(this, getDisplayMember("connect{invalidlogin_body}", "Invalid username or password.\nPlease alter your credentials and try again."), 
                            getDisplayMember("connect{invalidlogin_title}", "Invalid Credentials"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } else {
                        syncConnectionInfoToGUI(login);

                        // we get here, we have a valid user w/ admin rights.
                        HostProxy = proxy;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            } catch (InvalidCredentialException ice) {
                Debug.WriteLine(ice.Message);
                MessageBox.Show(this, getDisplayMember("connect{gooddbnoadmin_body}", "Successfully connected to database engine.\n\nHowever, you must also specify a valid GRIN-Global Login to import data."),
                    getDisplayMember("connect{gooddbnoadmin_title}", "Database Engine Connection Success, Login Failure"));
                txtGrinGlobalUserName.Focus();
                txtGrinGlobalUserName.Select();
            } catch (EndpointNotFoundException epnfe){
                Debug.WriteLine(epnfe.Message);
                MessageBox.Show(this, getDisplayMember("connect{badurl_body}", "The specified url does not seem to point to a valid GRIN-Global server.  Please alter it as needed and try again."),
                    getDisplayMember("connect{badurl_title}", "Connection Failed - Invalid URL"));
                txtGrinGlobalUrl.Focus();
                txtGrinGlobalUrl.Select();
            } catch (FaultException<ExceptionDetail> feed){
                Debug.WriteLine(feed.Message);
                MessageBox.Show(this, getDisplayMember("connect{goodwebnoadmin_body}", "Successfully connected to web service.\n\nHowever, you must also specify a valid GRIN-Global Login to import data."),
                    getDisplayMember("connect{goodwebnoadmin_title}", "Web Service Connection Success, Login Failure"));
                txtGrinGlobalUserName.Focus();
                txtGrinGlobalUserName.Select();
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                MessageBox.Show(this, ex.Message, getDisplayMember("connect{failed_title}", "Connection Failed"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void rdoDatabase_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void btnDatabaseEngineTestConnection_Click(object sender, EventArgs e) {
            try {
                syncConnectionInfoToGUI(null);
                var proxy = new HostProxy(_conn);
                string failureReason = proxy.TestDatabaseConnection();
                if (!String.IsNullOrEmpty(failureReason)){
                    MessageBox.Show(this, failureReason, getDisplayMember("testDbConnection{failed_title}", "Connection Failed"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                } else {
                    string invalidLogin = proxy.Login();
                    if (!String.IsNullOrEmpty(invalidLogin)) {
                        MessageBox.Show(this, getDisplayMember("testDbConnection{success_noadmin_body}", "Successfully connected to database engine.\n\nHowever, you must also specify a valid GRIN-Global Login to import data."), 
                            getDisplayMember("testDbConnection{success_noadmin_title}", "Database Engine Connection Success"));
                        txtGrinGlobalUserName.Focus();
                        txtGrinGlobalUserName.Select();
                    } else {
                        MessageBox.Show(this, getDisplayMember("testDbConnection{success_body}", "Connection Succeeded."),
                        getDisplayMember("testDbConnection{success_title}", "Valid Connection"), MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                }
            } catch (InvalidCredentialException ice){
                Debug.WriteLine(ice.Message);
                MessageBox.Show(this, getDisplayMember("testDbConnection{noadmin_body}", "Successfully connected to database engine.\n\nHowever, you must also specify a valid GRIN-Global Login to import data."),
                    getDisplayMember("testDbConnection{noadmin_title}", "Database Engine Connection Success"));
                txtGrinGlobalUserName.Focus();
                txtGrinGlobalUserName.Select();
            } catch (Exception ex) {
                MessageBox.Show(this, getDisplayMember("testDbConnection{completefail_body}", "Unable to contact the database engine.  Please verify the connection information is correct and your credentials are valid.\n\n{0}", ex.Message),
                    getDisplayMember("testDbConnection{noadmin_title}", "Unable to Contact Database Engine"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void chkWindowsAuthentication_CheckedChanged_1(object sender, EventArgs e) {
            syncGUI();
        }

        private void ddlDatabaseEngine_SelectedIndexChanged_1(object sender, EventArgs e) {

            txtDatabaseEnginePassword.Text = "";

            var engine = getEngineName();
            switch (engine) {
                case "sqlserver":
                    txtDatabaseEngineServerName.Text = @"localhost\sqlexpress";
                    txtDatabaseEngineUserName.Text = "sa";
                    chkWindowsAuthentication.Enabled = true;
                    chkWindowsAuthentication.Checked = true;
                    btnDatabaseEngineTestConnection.Focus();
                    break;
                case "mysql":
                    txtDatabaseEngineServerName.Text = "localhost:3306";
                    txtDatabaseEngineUserName.Text = "root";
                    chkWindowsAuthentication.Checked = false;
                    txtDatabaseEnginePassword.Focus();
                    break;
                case "oracle":
                    txtDatabaseEngineServerName.Text = "localhost:1521";
                    txtDatabaseEngineUserName.Text = "gg_user";
                    txtDatabaseEnginePassword.Text = "PA55w0rd!";
                    chkWindowsAuthentication.Checked = false;
                    btnDatabaseEngineTestConnection.Focus();
                    break;
                case "postgresql":
                    txtDatabaseEngineServerName.Text = "localhost:5432";
                    txtDatabaseEngineUserName.Text = "postgres";
                    chkWindowsAuthentication.Checked = false;
                    txtDatabaseEnginePassword.Focus();
                    break;
                default:
                    txtDatabaseEngineServerName.Text = "localhost";
                    txtDatabaseEngineUserName.Focus();
                    break;
            }


            syncGUI();
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "ImportWizard", "frmLogin", resourceName, null, defaultValue, substitutes);
        }
    }
}
