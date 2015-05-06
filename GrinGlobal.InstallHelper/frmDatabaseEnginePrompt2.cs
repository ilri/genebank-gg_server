using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Net;

namespace GrinGlobal.InstallHelper {
    public partial class frmDatabaseEnginePrompt2 : Form {
        public frmDatabaseEnginePrompt2() {
            InitializeComponent();
            AllowRemote = true;
        }

        void openURL(string url, bool closeWindow) {
            Process.Start(url);
            Thread.Sleep(1000);
            if (closeWindow) {
                DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        public bool ClientMode { get; set; }

        public bool AllowRemote { get; set; }

        public DialogResult ShowDialog(IWin32Window owner, bool clientMode) {
            ClientMode = clientMode;
            return ShowDialog(owner);
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e) {
            string engineName = "";
            string instanceName = "";
            string location = "local";
            int port = 0;
            bool useWindowsAuth = false;

            if (rdoRemote.Checked) {
                // use defaults
                location = "remote";
            } else if (rdoOverride.Checked) {
                // don't know, default to 'unknown' until they override it later
                engineName = "unknown";
                instanceName = "";
                port = 0;
                location = "unknown";
            } else {
                location = "local";
                if (rdoMySql.Checked) {
                    engineName = "mysql";
                    instanceName = "";
                    port = 3306;
                } else if (rdoSqlServer.Checked) {
                    engineName = "sqlserver";
                    instanceName = "SQLExpress";
                    port = 0;
                    useWindowsAuth = true;
                } else if (rdoSqlite.Checked) {
                    engineName = "sqlite";
                    instanceName = "";
                    port = 0;
                } else if (rdoOracle.Checked) {
                    engineName = "oracle";
                    instanceName = "";
                    port = 1521;
                } else if (rdoPostgreSQL.Checked) {
                    engineName = "postgresql";
                    instanceName = "";
                    port = 5432;
                }
            }

            Utility.SaveDatabaseWindowsAuth(useWindowsAuth);
            Utility.SaveDatabaseEngine(engineName);
            Utility.SaveDatabaseLocation(location);
            Utility.SaveDatabaseInstanceName(instanceName);
            Utility.SaveDatabasePort(port);

            // NOTE: switching to 'remote' defaults it to an invalid server name
            Utility.SaveDatabaseServer(rdoRemote.Checked ? "-" : "localhost");
            DialogResult = DialogResult.OK;
            Close();
        }

        private void lnkSqlServer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            openURL("http://www.microsoft.com/web/gallery/install.aspx?appsxml=&appid=SQLExpressTools;SQLExpressTools", true);
        }

        private void lnkMySQL_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e) {
            openURL("http://dev.mysql.com/downloads/mysql/", true);

        }

        private void lnkOracle_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e) {
            openURL("http://www.oracle.com/technology/software/products/database/xe/htdocs/102xewinsoft.html", true);

        }

        private void lnkPostgreSQL_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e) {
            openURL("http://www.postgresql.org/download/windows", true);
        }

        private void frmDatabaseEnginePrompt_Load(object sender, EventArgs e) {
            
            rdoMySql.Enabled = Utility.IsMySQLInstalled;
            lnkMySQL.Enabled = !rdoMySql.Enabled;
            
            rdoOracle.Enabled = Utility.IsOracleExpressInstalled;
            lnkOracle.Enabled = !rdoOracle.Enabled;

            rdoSqlServer.Enabled = Utility.IsSqlServer2008Installed;
            lnkSqlServer.Enabled = !rdoSqlServer.Enabled;

            rdoPostgreSQL.Enabled = Utility.IsPostgreSQLInstalled;
            lnkPostgreSQL.Enabled = !rdoPostgreSQL.Enabled;


            if (Utility.IsAnyDatabaseEngineInstalled) {
                // only auto-select one based on registry settings if one is already installed locally
                var engineName = Utility.GetDatabaseEngine(null, null, false, "???");
                switch (engineName.ToLower().Trim()) {
                    case "mysql":
                        rdoMySql.Checked = true;
                        break;
                    case "sqlserver":
                        rdoSqlServer.Checked = true;
                        break;
                    case "oracle":
                        rdoOracle.Checked = true;
                        break;
                    case "postgresql":
                        rdoPostgreSQL.Checked = true;
                        break;
                    case "sqlite":
                        rdoSqlite.Checked = true;
                        break;
                }
            }


            var location = Utility.GetDatabaseLocation(null, null, "local");
            switch (location.ToLower()) {
                case "remote":
                    rdoRemote.Checked = true;
                    break;
                case "unknown":
                    rdoOverride.Checked = true;
                    break;
                case "local":
                default:
                    rdoLocal.Checked = true;
                    break;
            }


            //var serverName = Utility.GetDatabaseServerName(null, null, "localhost");

            //if (!rdoOverride.Checked) {
            //    if (!String.IsNullOrEmpty(serverName)) {
            //        switch (serverName.ToLower()) {
            //            case "localhost":
            //            case "127.0.0.1":
            //            case "(local)":
            //            case ".":
            //                rdoLocal.Checked = true;
            //                break;
            //            default:
            //                if (Dns.GetHostName().ToLower() == serverName.ToLower()) {
            //                    // assume local if dns name matches server name reg key
            //                    rdoLocal.Checked = true;
            //                } else {
            //                    if (ClientMode) {
            //                        rdoLocal.Checked = true;
            //                    } else {
            //                        // otherwise assume remote
            //                        rdoRemote.Checked = true;
            //                    }
            //                }
            //                break;

            //        }
            //    }
            //}

            if (ClientMode) {
                lblInfo.Text = "GRIN-Global Curator Tool requires a local database to cache data.";
            } else {
                lblInfo.Text = "GRIN-Global requires a database to work properly.";
            }
            rdoSqlite.Visible = ClientMode;
            lnkSQLite.Visible = ClientMode;


            syncGUI();
        }

        private void syncGUI(){

            lblLocal.Visible = rdoLocal.Checked;
            gbLocal.Visible = rdoLocal.Checked;

            rdoRemote.Enabled = !ClientMode && AllowRemote;
            lblRemote.Visible = rdoRemote.Checked;

            btnOK.Enabled =
                (rdoLocal.Checked
                    && (rdoMySql.Checked || rdoSqlServer.Checked || rdoPostgreSQL.Checked || rdoOracle.Checked || rdoSqlite.Checked)
                )
                || rdoRemote.Checked
                || rdoOverride.Checked;
        }

        private void lnkPostgreSQLInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            openURL("http://www.postgresql.org/about/", false);

        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            openURL("http://www.microsoft.com/sqlserver/2008/en/us/overview.aspx", false);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            openURL("http://www.mysql.com/about/", false);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            openURL("http://www.oracle.com/technology/products/database/xe/index.html", false);
        }

        private void rdoSqlServer_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void rdoMySql_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void rdoOracle_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void rdoPostgreSQL_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void rdoRemote_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void rdoLocal_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void lnkSQLite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            openURL("http://sqlite.org/about.html", false);
        }

        private void rdoSqlite_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void rdoOverride_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }
    }
}
