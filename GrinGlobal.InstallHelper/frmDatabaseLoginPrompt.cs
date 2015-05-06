using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GrinGlobal.Core;
using GrinGlobal.Core.DataManagers;
using System.IO;
using System.Net;

namespace GrinGlobal.InstallHelper {
    public partial class frmDatabaseLoginPrompt : Form {
        public frmDatabaseLoginPrompt() {
            InitializeComponent();
            ddlEngine.Items.Add("MySQL 5.1 or later *");
            ddlEngine.Items.Add("PostgreSQL 8.3 or later *");
            ddlEngine.Items.Add("Oracle XE 10g or later *");
            ddlEngine.SelectedIndex = 0;
        }

        public bool ClientMode { get; set; }

        public string Password { get { return txtPassword.Text; } }
        public bool UseWindowsAuthentication {
            get {
                return _dbEngineUtil == null ? false : _dbEngineUtil.UseWindowsAuthentication;
            }
        }

        DatabaseEngineUtil _dbEngineUtil;
        /// <summary>
        /// Gets the DatabaseEngineUtil as specified by the form contents.  Will be null after form is closed if user does not click 'Save'.
        /// </summary>
        public DatabaseEngineUtil DatabaseEngineUtil { get { return _dbEngineUtil; } }

        private void textBox1_TextChanged(object sender, EventArgs e) {

        }

        private void ddlEngine_SelectedIndexChanged(object sender, EventArgs e) {
            syncGui(true);
        }

        public string EngineName {
            get {
                var s = ddlEngine.Text.ToLower();

                if (s.Contains("sql server")) {
                    return "sqlserver";
                } else if (s.Contains("mysql")) {
                    return "mysql";
                } else if (s.Contains("postgresql")) {
                    return "postgresql";
                } else if (s.Contains("oracle")) {
                    return "oracle";
                } else if (s.Contains("sqlite")){
                    return "sqlite";
                } else {
                    return "";
                }
            }
            set {
                var s = ("" + value).ToLower();
                if (s.Contains("sqlserver")) {
                    ddlEngine.SelectedIndex = 0;
                } else if (s.Contains("mysql")) {
                    ddlEngine.SelectedIndex = 1;
                } else if (s.Contains("postgresql")) {
                    ddlEngine.SelectedIndex = 2;
                } else if (s.Contains("oracle")) {
                    ddlEngine.SelectedIndex = 3;
                } else if (s.Contains("sqlite")) {
                    ddlEngine.SelectedIndex = 4;
                }

            }
        }

        private void syncGui(bool useDefaults){

            var engine = EngineName;

            lblSID.Visible = false;
            txtSID.Visible = false;

            if (engine == "sqlserver") {
                chkWindowsAuthentication.Visible = chkWindowsAuthentication.Enabled = true;
                lblPort.Visible = true;
                txtPort.Visible = true;
                if (useDefaults) {
                    txtUsername.Text = "sa";
                    if (txtPort.Text == "*INVALID*" || txtPort.Text == "5432" || txtPort.Text == "1521" || txtPort.Text == "3306") {
                        txtPort.Text = "SQLExpress";
                    }
                    chkWindowsAuthentication.Checked = true;
                }
            } else {
                chkWindowsAuthentication.Visible = chkWindowsAuthentication.Enabled = false;
                if (engine == "sqlite") {
                    lblPort.Visible = false;
                    txtPort.Visible = false;
                    btnServerName.Visible = true;
                } else {
                    lblPort.Visible = true;
                    txtPort.Visible = true;
                    btnServerName.Visible = false;
                }
            }

            btnServerName.Visible = false;

            if (engine == "mysql"){
                if (useDefaults) {
                    txtPort.Text = "3306";
                    txtUsername.Text = "root";
                }
            } else if (engine == "postgresql") {
                if (useDefaults) {
                    txtPort.Text = "5432";
                    txtUsername.Text = "postgres";
                }
            } else if (engine == "oracle") {
                txtSID.Visible = true;
                lblSID.Visible = true;
                if (useDefaults) {
                    txtPort.Text = "1521";
                    txtUsername.Text = "SYS";
                    if (txtSID.Text == "*INVALID*") {
                        txtSID.Text = "XE";
                    }
                }
            } else if (engine == "sqlite") {
                if (txtServerName.Text == "localhost") {
                    txtServerName.Text = Toolkit.ResolveFilePath("~/gringlobal_localhost.db", false);
                }
                btnServerName.Visible = true;
            }

            if (!chkWindowsAuthentication.Visible) {
                txtPassword.Enabled = txtUsername.Enabled = true;
            } else {
                txtPassword.Enabled = txtUsername.Enabled = !chkWindowsAuthentication.Checked;
            }

            if (ddlEngine.Text.EndsWith("*")) {
                lblWarning.Left = txtPort.Left;
                lblWarning.Visible = true;
            } else {
                lblWarning.Visible = false;
            }

            toggleTestConnection();

            refreshEngineUtil(false);

        }

        private void refreshEngineUtil(bool forceRefresh) {

            var gguacPath = Utility.GetPathToUACExe();
            if (!File.Exists(gguacPath)) {
                gguacPath = Toolkit.ResolveFilePath("~/gguac.exe", false);
                if (!File.Exists(gguacPath)) {
                    throw new InvalidOperationException(getDisplayMember("refreshEngineUtil{nouac}", "Could not locate the path to gguac.exe.  Can not continue."));
                }
            }

            var databaseName = "gringlobal";

            lnkMixedMode.Visible = false;
            txtPassword.Visible = true;

            if (forceRefresh) {
                _dbEngineUtil = null;
            }

            switch (EngineName) {
                case "sqlserver":
                    if (_dbEngineUtil == null || !(_dbEngineUtil is SqlServerEngineUtil)) {
                        _dbEngineUtil = DatabaseEngineUtil.CreateInstance("sqlserver", gguacPath, null);
                    }
                    var port = Toolkit.ToInt32(txtPort.Text, -1);
                    var instanceName = "";
                    if (port > -1 && port < 65536) {
                        // assume they gave us a port number
                        _dbEngineUtil.Port = port;
                    } else {
                        // assume they gave us an instance name
                        instanceName = txtPort.Text;
                        (_dbEngineUtil as SqlServerEngineUtil).InstanceName = instanceName;
                    }
                    (_dbEngineUtil as SqlServerEngineUtil).UseWindowsAuthentication = chkWindowsAuthentication.Enabled && chkWindowsAuthentication.Checked;

                    if (!chkWindowsAuthentication.Checked) {
                        if (!(_dbEngineUtil as SqlServerEngineUtil).IsMixedModeEnabled(instanceName)) {
                            lnkMixedMode.Visible = true;
                            lnkMixedMode.Left = txtPassword.Left;
                            lnkMixedMode.Top = txtPassword.Top;
                            txtPassword.Visible = false;
                        }
                    }

                    break;
                case "mysql":
                    if (_dbEngineUtil == null || !(_dbEngineUtil is MySqlEngineUtil)) {
                        _dbEngineUtil = DatabaseEngineUtil.CreateInstance("mysql", gguacPath, null);
                    }
                    break;
                case "oracle":
                    if (_dbEngineUtil == null || !(_dbEngineUtil is OracleEngineUtil)) {
                        _dbEngineUtil = DatabaseEngineUtil.CreateInstance("oracle", gguacPath, txtSID.Text);
                    }
                    break;
                case "postgresql":
                    if (_dbEngineUtil == null || !(_dbEngineUtil is PostgreSqlEngineUtil)) {
                        _dbEngineUtil = DatabaseEngineUtil.CreateInstance("postgresql", gguacPath, null);
                    }
                    break;
                case "sqlite":
                    if (_dbEngineUtil == null || !(_dbEngineUtil is SqliteEngineUtil)) {
                        _dbEngineUtil = DatabaseEngineUtil.CreateInstance("sqlite", gguacPath, null);
                        // sqlite simply points at a file, so we specify the full file path as the database name
                        databaseName = txtServerName.Text;
                    }
                    break;
                case "":
                    // ignore, happens when launched from an installer context for some reason
                    // next pass around will give us the right value so.... whatever.
                    return;
                default:
                    throw new InvalidOperationException(getDisplayMember("refreshEngineUtil{default}", "No valid database engine selected."));
            }

            _dbEngineUtil.Port = Toolkit.ToInt32(txtPort.Text, 0);
            _dbEngineUtil.ServerName = txtServerName.Text;
            _dbEngineUtil.SID = txtSID.Text;

            //            _dbEngineUtil.SuperUserName = txtUsername.Text;

            // NOTE we already set the UseWindowsAuthentication above for sql server so no special code is needed here
            // don't show the password in the connection string...
            txtConnectionstring.Text = _dbEngineUtil.GetDataConnectionSpec(databaseName, txtUsername.Text, "__PASSWORD__").ConnectionString;


            _genericConnectionString = _dbEngineUtil.GetDataConnectionSpec(databaseName, "__USERID__", "__PASSWORD__").ConnectionString;

            Application.DoEvents();
        }

        private string _genericConnectionString;

        public string GetGenericConnectionString() {
            return _genericConnectionString;
        }

        private void txtConnectionstring_Enter(object sender, EventArgs e) {

        }

        private int _installerWindowHandle;
        private bool _saveToRegistry;
        public DialogResult ShowDialog(int installerWindowHandle, DatabaseEngineUtil dbEngineUtil, bool loadFromRegistry, bool writeToRegistryOnSave, bool clientMode, string superUserPassword, bool? useWindowsAuthentication, bool enableAutoLogin, bool showSkipDatabase) {
            _installerWindowHandle = installerWindowHandle;
            return ShowDialog(null, dbEngineUtil, loadFromRegistry, writeToRegistryOnSave, clientMode, superUserPassword, useWindowsAuthentication, enableAutoLogin, showSkipDatabase);
        }

        public DialogResult ShowDialog(IWin32Window owner, bool loadFromRegistry, bool writeToRegistryOnSave, bool clientMode, string superUserPassword, bool? useWindowsAuthentication, bool enableAutoLogin, bool showSkipDatabase) {
            return ShowDialog(owner, null, loadFromRegistry, writeToRegistryOnSave, clientMode, superUserPassword, useWindowsAuthentication, enableAutoLogin, showSkipDatabase);
        }
        public DialogResult ShowDialog(IWin32Window owner, DatabaseEngineUtil dbEngineUtil, bool loadFromRegistry, bool writeToRegistryOnSave, bool clientMode, string superUserPassword,  bool? useWindowsAuthentication, bool enableAutoLogin, bool showSkipDatabase) {
            _saveToRegistry = writeToRegistryOnSave;

            btnSkip.Visible = showSkipDatabase;

            ClientMode = clientMode;
            if (dbEngineUtil == null) {
                try {
                    if (loadFromRegistry) {
                        this.loadFromRegistry();
                        refreshEngineUtil(true);
                        if (useWindowsAuthentication != null) {
                            _dbEngineUtil.UseWindowsAuthentication = (bool)useWindowsAuthentication;
                        }
                        chkWindowsAuthentication.Checked = _dbEngineUtil.UseWindowsAuthentication;
                        txtPassword.Text = superUserPassword;
                        if (enableAutoLogin) {
                            if (_dbEngineUtil.UseWindowsAuthentication || !String.IsNullOrEmpty(txtPassword.Text)) {
                                if (_dbEngineUtil.TestLogin(txtUsername.Text, txtPassword.Text) == null) {
                                    // valid login, do not prompt, just return it's valid
                                    return DialogResult.OK;
                                }
                            }
                        }
                    }
                } catch (Exception ex) {
                    MessageBox.Show(getDisplayMember("ShowDialog{failed}", "Error showing database connection prompt: {0}", ex.Message));
                    throw;
                }
            } else {
                EngineName = dbEngineUtil.EngineName;
                txtServerName.Text = dbEngineUtil.ServerName;
                txtPort.Text = dbEngineUtil.Port.ToString();
                //if (dbEngineUtil is SqlServerEngineUtil) {
                //    txtSID.Text = (dbEngineUtil as SqlServerEngineUtil).InstanceName;
                //} else {
                //    txtSID.Text = "";
                //}
                chkWindowsAuthentication.Checked = (dbEngineUtil.UseWindowsAuthentication || (useWindowsAuthentication == true)) && (dbEngineUtil is SqlServerEngineUtil);
            }

            return this.ShowDialog(owner);
        }

        private void frmDatabaseLoginPrompt_Load(object sender, EventArgs e) {
            if (ClientMode) {
                ddlEngine.Items.Add("SQLite 3 *");
            }
        }

        private void chkWindowsAuthentication_CheckedChanged(object sender, EventArgs e) {
            syncGui(false);
        }

        private void txtServerName_TextChanged(object sender, EventArgs e) {
//            syncGui(false);
        }

        private void btnLogin_Click(object sender, EventArgs e) {

            try {
                this.Cursor = Cursors.WaitCursor;
                var retries = 0;
                this.AcceptButton = btnTestConnection;

                while (retries < 2) {
                    try {
                        Application.DoEvents();
                        refreshEngineUtil(true);
                        var output = _dbEngineUtil.TestLogin(txtUsername.Text, (chkWindowsAuthentication.Checked && chkWindowsAuthentication.Enabled ? null : txtPassword.Text));
                        if (output != null) {
                            throw new InvalidOperationException(output);
                        } else {

                            if (_dbEngineUtil.UseWindowsAuthentication && Utility.IsRegisteredOnDomain() && _dbEngineUtil is SqlServerEngineUtil) {
                                // Configuration will fail if the machine is connected to a network but the domain controller cannot be reached.
//                                MessageBox.Show(this, "This machine appears to be registered to a domain.\nIf it is unable to reach the domain controller, installation will fail.\nIf this occurs, please reinstall without using Windows Authentication.", "Valid Connection - But Installation May Fail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                var resp = MessageBox.Show(this, getDisplayMember("login{domain_body}", "This computer appears to be registered to a domain.\nIt is recommended that a SQL Server Login be used in this situation.\n\nDo you want to use the 'sa' login instead?"), 
                                    getDisplayMember("login{domain_title}", "Use of SQL Server Login Recommended"), MessageBoxButtons.YesNo, MessageBoxIcon.Question); //If configuration fails with an error similar to 'cannot create table accession', please uncheck 'Use Windows Authentication', provide a valid database userid and password, then try again.", "Valid Connection - But Configuration May Fail", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                if (resp == DialogResult.Yes) {
                                    chkWindowsAuthentication.Checked = false;
                                    txtPassword.Focus();
                                    return;
                                }

                            } else {
                                MessageBox.Show(this, getDisplayMember("login{success_body}", "Successfully connected to {0}", EngineName), 
                                    getDisplayMember("login{success_title}", "Valid Connection"), MessageBoxButtons.OK, MessageBoxIcon.None);
                            }
                            btnSave.Enabled = true;
                            Application.DoEvents();
                            this.AcceptButton = btnSave;
                            btnSave.Focus();
                            retries = 1000;  // flag to jump out of loop

                        }
                    } catch (Exception ex) {
                        if (ex.Message.Contains("No process is on the other end of the pipe") && retries == 0) {
                            // SQL Server specific error that happens when connection pooling is enabled
                            // and pool hasn't been cleared since last sql server service recycling.
                            // We basically give it one auto-retry before reporting a connection error.
                            lblAutoRetrying.Visible = true;
                            //MessageBox.Show("Connection failed, auto-retrying...");
                        } else {
                            retries = 1000;  // flag to jump out of loop
                            MessageBox.Show(getDisplayMember("login{failed}", "Connection failed.  Reason:\n{0}", ex.Message));
                            btnSave.Enabled = false;
                        }
                    }
                    retries++;
                }
            } finally {
                lblAutoRetrying.Visible = false;
                this.Cursor = Cursors.Default;
                Application.DoEvents();
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {

            if (_saveToRegistry) {
                var engineName = EngineName;
                var usingWindowsAuth = chkWindowsAuthentication.Checked && chkWindowsAuthentication.Enabled;
                var serverName = txtServerName.Text;

                var instanceName = txtSID.Text;
                
                var port = Utility.ToInt32(txtPort.Text, 0);
                if (port == 0 && engineName.ToLower() == "sqlserver") {
                    instanceName = txtPort.Text;
                }

                var location = "remote";

                if (serverName.Contains("localhost") || 
                        serverName.Contains("127.0.0.1") || 
                        serverName.Contains("(local)") || 
                        serverName == "." || 
                        serverName.Contains(Dns.GetHostName().ToLower())) {
                    location = "local";
                } else {
                    var ips = Dns.GetHostAddresses(Dns.GetHostName());
                    foreach (var ip in ips) {
                        if (serverName.Contains(ip.ToString())) {
                            location = "local";
                            break;
                        }
                    }
                }

                Utility.SaveDatabaseSettings(engineName, usingWindowsAuth, serverName, port, instanceName, _genericConnectionString, location);
            }

            refreshEngineUtil(false);
            //_dbEngineUtil.EngineName = EngineName;
            //_dbEngineUtil.UseWindowsAuthentication = chkWindowsAuthentication.Checked && chkWindowsAuthentication.Enabled;
            //if (_dbEngineUtil is SqlServerEngineUtil) {
            //    ((SqlServerEngineUtil)_dbEngineUtil).InstanceName = txtInstance.Text;
            //}
            //_dbEngineUtil.ServerName = txtServerName.Text;
            //_dbEngineUtil.Port = Utility.ToInt32(txtPort.Text, 0);
            ////_dbEngineUtil.SuperUserName = txtUsername.Text;


            DialogResult = DialogResult.OK;
            Close();
        }

        bool _activated;
        private void frmDatabaseLoginPrompt_Activated(object sender, EventArgs e) {
            if (!_activated) {
                _activated = true;

                if (_dbEngineUtil != null){
                    lblTitle.Text = "You must specify the database connection information for configuration to continue.";
                } else {
                    lblTitle.Text = "You must specify the database connection information for configuration to continue.\n\nTo use the defaults, just click 'Test Connection', then 'Save and Continue'.";
                }

                lblPasswordRequired.Visible = false;
                lblPasswordRequired.Left = lblAutoRetrying.Left;

                syncGui(false);

                if (txtPassword.Enabled && txtPassword.Visible) {
                    txtPassword.Focus();
                }

                //Toolkit.ActivateApplication(this.Handle);
                //this.Focus();
                //if (_installerWindowHandle > 0) {
                //    Toolkit.MinimizeWindow(_installerWindowHandle);
                //}
            }

        }

        private void loadFromRegistry() {
            EngineName = Utility.GetDatabaseEngine(null, null, false, "sqlserver");
            txtSID.Text = Utility.GetDatabaseInstanceName(null, null, "XE");
            var winAuth = Utility.GetDatabaseWindowsAuth(null, null);
            if (!winAuth && EngineName == "sqlserver" && String.IsNullOrEmpty(Utility.GetDatabaseConnectionString(null, null, null, null))){
                // using default of sql server, as no conn string was found in reg. assume using windows auth
                chkWindowsAuthentication.Checked = true;
            } else {
                chkWindowsAuthentication.Checked = winAuth;
            }
            txtServerName.Text = Utility.GetDatabaseServerName(null, null, "localhost");
            var port = Utility.GetDatabasePort(null, null, 0);
            if (port > 0){
                txtPort.Text = port.ToString();
            }

            if (!String.IsNullOrEmpty(Utility.GetDatabaseConnectionString(null, null, null, null))) {
                lblTitle.Text = "You must specify the database connection information for configuration to continue.";
            } else {
                lblTitle.Text = "You must specify the database connection information for configuration to continue.\n\nTo use the defaults, just click 'Test Connection', then 'Save and Continue'.";
            }

        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void frmDatabaseLoginPrompt_FormClosing(object sender, FormClosingEventArgs e) {
            if (DialogResult != DialogResult.OK) {
                _dbEngineUtil = null;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e) {
            toggleTestConnection();
            AcceptButton = btnTestConnection;
        }

        private void toggleTestConnection(){
            btnTestConnection.Enabled = txtPassword.Text.Trim().Length > 0 || (chkWindowsAuthentication.Checked && chkWindowsAuthentication.Enabled);
            lblPasswordRequired.Visible = !btnTestConnection.Enabled && txtPassword.Enabled && txtPassword.Visible;
        }

        private void btnServerName_Click(object sender, EventArgs e) {
            openFileDialog1.Title = "Locate SQLite3 Database File";
            openFileDialog1.Filter = "SQLite3 databases (*.db)|*.db|All Files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK){
                txtServerName.Text = openFileDialog1.FileName;
            }
        }

        private void lnkMixedMode_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            var fenable = new frmEnableSqlServerLogin();
            fenable.SqlUtil = _dbEngineUtil as SqlServerEngineUtil;
            fenable.SqlUtil.InstanceName = txtPort.Text;
            var dr = fenable.ShowDialog(this); // MessageBox.Show(this, "SQL Server is currently configured to use only Windows Authentication.\nTo use a SQL login, Mixed Mode must be enabled.  This will require restarting the SQL Server service.\nDo you want to enable Mixed Mode?", "Enable Mixed Mode?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.OK) {
                refreshEngineUtil(false);
                txtPassword.Text = fenable.txtPassword.Text;
                syncGui(false);
                btnTestConnection.Focus();
            }
        }

        private void btnSkip_Click(object sender, EventArgs e) {
            if (MessageBox.Show(this, getDisplayMember("skipDatabase{body}", "The GRIN-Global database and its data will not be altered in any way.\nDo you want to uninstall the associated software?"), 
                getDisplayMember("skipDatabase{title}", "Uninstall Application but not Database?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes){
                DialogResult = DialogResult.Ignore;
                Close();
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "frmDatabaseLoginPrompt", resourceName, null, defaultValue, substitutes);
        }

        private void txtPort_TextChanged(object sender, EventArgs e) {

        }
    }
}
