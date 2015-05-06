using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Threading;
using GrinGlobal.Core;
using System.DirectoryServices;
using GrinGlobal.InstallHelper;

namespace GrinGlobal.ConfigurationWizard {
    public partial class frmAllInOne : Form {
        public frmAllInOne() {
            InitializeComponent();
        }

        private enum ActionImage : int {
            Blank = 0,
            NoAction = 1,
            Install = 2,
            Uninstall = 3,
            MixedAction = 4
        }

        private DatabaseEngineUtil _dbEngineUtil;

        private string getEngineName() {
            if (rdoMySQL.Checked) {
                return "mysql";
            } else if (rdoOracle.Checked) {
                return "oracle";
            } else if (rdoPostgreSQL.Checked) {
                return "postgresql";
            } else if (rdoSqlServer.Checked) {
                return "sqlserver";
            } else {
                return "sqlserver";
            }
        }

        private delegate bool _boolMethodInvoker();
        private delegate void _voidMethodInvoker();

        private void tvOptions_AfterSelect(object sender, TreeViewEventArgs e) {
            switch (e.Node.Name){

                case "ndRoot":
                    lblTitle.Text = "GRIN-Global Setup";
                    lblBody.Text = "This allows you to configure your local installation of GRIN-Global.\nUse the items on the left to customize your installation.";
                    selectPanel(pnlRoot);
                    // we do BeginInvoke here to asynchronously set the focus.
                    // see http://dis4ea.blogspot.com/2006/01/treeview-afterselect-and-changing.html
                    txtInstallDir.BeginInvoke(new _boolMethodInvoker(txtInstallDir.Focus));
                    break;
                case "ndServer":
                    lblTitle.Text = "Server Setup";
                    lblBody.Text = "Use this to configure the Server installation of the GRIN-Global Application Suite.";
                    lblDefaultOptions.Text = @"The Server install consists of the following:

* Database Engine 
    A relational database management system, such as PostgreSQL, SQL Server Express, Oracle Express, or MySQL.  The current version only supports SQL Server, though future versions will support all those listed.

* Database
    The data store for GRIN-Global data

* Web Application
    Provides web services for clients to consume

* Search Engine
    Allows fast searching of entire database

The Server install may require the installation of Microsoft's IIS web server.
" +
  (isIISInstalled ? "It is already installed on your machine." : "You may be prompted for your Windows CD.");

                    selectPanel(pnlDefault);
                    break;
                case "ndDatabaseEngine":
                    lblTitle.Text = "Database Engine Setup";
                    lblBody.Text = "GRIN-Global requires a third-party database engine to host its data.\nOne of the following must be selected.";

                    refreshDatabaseEngineUtil();

                    selectPanel(pnlDatabaseEngine);
                    break;
                case "ndDatabaseTool":
                    lblTitle.Text = "Database Engine Tools Setup";
                    lblBody.Text = "GRIN-Global does not require database engine tools.  However, if you wish to view or manipulate data without using the GRIN-Global applications, these tools will allow you to do so.";

                    lblDefaultOptions.Text = "This will install the graphical user interface (GUI) database engine tools created by the database engine vendor.\r\n\r\nThe command line tools are always installed regardless of this option.";
                    selectPanel(pnlDefault);

                    break;
                case "ndDatabase":
                    lblTitle.Text = "Database Setup";
                    lblBody.Text = "This installs the actual database schema and data into " + _dbEngineUtil.FriendlyName;
                    lblDatabaseDescription.Text = @"To install or uninstall the GRIN-Global database, the " + _dbEngineUtil.SuperUserName + " password for " + _dbEngineUtil.FriendlyName + " is required.";

                    if (!isSelectedDatabaseEngineInstalled) {
                        lblDatabaseDescription.Text += "\n\nSince " + _dbEngineUtil.FriendlyName + " is not yet installed, the password you provide here will be the " + _dbEngineUtil.SuperUserName + " password.";
                    }


                    lblSuperUserPassword.Text = _dbEngineUtil.SuperUserName + " password:";

                    // make sure root password works
                    btnTestLogin.Enabled = lblPasswordMismatch.Text.Length == 0 && txtPassword.Text.Length > 0 && isSelectedDatabaseEngineInstalled && !isUsingWindowsAuthentication;

                    selectPanel(pnlDatabase);
                    txtPassword.BeginInvoke(new _boolMethodInvoker(txtPassword.Focus));
                    break;
                case "ndWebApplication":
                    lblTitle.Text = "Web Application Setup";
                    lblBody.Text = "This installs the required web application and web services.\nIt requires the Microsoft web server, IIS, to function properly.";
                    selectPanel(pnlWebApplication);
                    break;
                case "ndSearchEngine":
                    lblTitle.Text = "Search Engine Setup";
                    lblBody.Text = "This installs the search engine required to perform searches on GRIN-Global data.";
                    lblDefaultOptions.Text = "The Search Engine allows fast access to any word that occurs in the entire database.";
                    selectPanel(pnlDefault);
                    break;
                case "ndClient":
                    lblTitle.Text = "Client Setup";
                    lblBody.Text = "Use this to configure the Client installation of the GRIN-Global Application Suite.";
                    lblDefaultOptions.Text = @"The Client install consists of the following:

* Curator Tool
    A spreadsheet-like interface for accessing, editing, and uploading GRIN-Global data

The Client install requires SQL Server 2008 Express.";
                    selectPanel(pnlDefault);
                    break;
                case "ndCuratorTool":
                    lblTitle.Text = "Curator Tool Setup";
                    lblBody.Text = "This installs the front-end applications used by curators.";
                    if (isSqlServer2008ExpressInstalled) {
                        lblSqlServer.Text = "SQL Server 2008 Express is already installed";
                    } else {
                        lblSqlServer.Text = "SQL Server Express 2008 will be installed";
                    }
                    selectPanel(pnlCuratorTool);
                    break;
                case "ndUpdateWindows":
                    lblTitle.Text = "Windows Update";
                    lblBody.Text = "This updates Windows with the latest security patches and bug fixes.";
                    lblDefaultOptions.Text = "The GRIN-Global Application Suite relies upon software bundled with Windows. The version of this software on your computer or distributed with GRIN-Global may be out of date.\n\nRunning Windows Update ensures the latest patches have been applied to your system.";
                    selectPanel(pnlDefault);
                    break;
                default:
                    break;
            }

            refreshActions();

        }

        private void selectPanel(Panel p) {
            foreach (Control ctl in gbPanel.Controls) {
                ctl.Visible = false;
            }
            if (p != null) {
                p.Visible = true;
            }
        }

        private void frmAllInOne_Load(object sender, EventArgs e) {

            this.Text = Toolkit.GetApplicationVersion("GRIN-Global Configuration Wizard");

            _installedApps = Utility.ListInstalledApplications();

            txtSqlServerInstanceName.Text = Utility.GetDatabaseInstanceName(null, null);

            chkUseWindowsLogon.Checked = Utility.GetDatabaseWindowsAuth(null, null);

            // default to using windows logon for sql server installs
            if (!chkUseWindowsLogon.Checked && getEngineName() == "sqlserver" && !isDatabaseInstalled) {
                chkUseWindowsLogon.Checked = true;
            }

            refreshDatabaseEngineUtil();


            lblPasswordMismatch.Text = badPasswordReason();

            this.Width = 620;
            this.MinimumSize = new Size(620, 460);
            this.Height = 460;

            // since we messed with the height/width, we need to manually recenter the form
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;

            foreach (Control ctl in gbPanel.Controls) {
                ctl.Dock = DockStyle.Fill;
                ctl.Visible = false;
                ctl.Top = 0;
                ctl.Left = 0;
            }

            // TODO: re-enable windows option???
            getNode("ndUpdateWindows", true).Remove();

            bool newInstall = isNewInstall;

            checkNode("ndDatabaseEngine", isSelectedDatabaseEngineInstalled, newInstall);
            checkNode("ndDatabaseTool", isSelectedDatabaseToolInstalled, newInstall);
            checkNode("ndDatabase", isDatabaseInstalled, newInstall);
            checkNode("ndWebApplication", isWebApplicationInstalled, newInstall);
            checkNode("ndSearchEngine", isSearchEngineInstalled, newInstall);
            checkNode("ndCuratorTool", isCuratorToolInstalled, newInstall);



            rdoSqlServer.Checked = true;

            rdoMySQL.Enabled = false;
            rdoPostgreSQL.Enabled = false;
            rdoOracle.Enabled = false;

            // look for presence of non-SQL Server installers.  if it's there, enable the corresponding radio button(s).
            string binDir = Utility.GetBinDirectory(null, null);
            if (!String.IsNullOrEmpty(binDir)) {
                if (Directory.Exists(binDir)) {
                    foreach (string f in Directory.GetFiles(binDir)) {
                        if (f.ToLower().Contains("mysql")) {
                            rdoMySQL.Enabled = true;
                        } else if (f.ToLower().Contains("pgsql") || f.ToLower().Contains("postgres")) {
                            rdoPostgreSQL.Enabled = true;
                        } else if (f.ToLower().Contains("oracle")) {
                            rdoOracle.Enabled = true;
                        }
                    }
                }
            }

            //// if a database engine is already installed, enable its radio button
            //if (isMySQL51Installed) {
            //    rdoMySQL.Enabled = true;
            //}
            //if (isOracle10gExpressInstalled) {
            //    rdoOracle.Enabled = true;
            //}
            //if (isPostgreSQL83Installed) {
            //    rdoPostgreSQL.Enabled = true;
            //}



            // pull registry key to see which engine was selected for GRIN-Global (if any)
            string engine = Utility.GetDatabaseEngine(null, null, false);
            if (!String.IsNullOrEmpty(engine)) {
                switch (engine) {
                    case "mysql":
                        rdoMySQL.Checked = true;
                        break;
                    case "oracle":
                        rdoPostgreSQL.Checked = true;
                        break;
                    case "postgresql":
                        rdoPostgreSQL.Checked = true;
                        break;
                    case "sqlserver":
                    default:
                        rdoSqlServer.Checked = true;
                        break;
                }
            } else {
                //if (isOracle10gExpressInstalled) {
                //    rdoOracle.Checked = true;
                //}
                //if (isPostgreSQL83Installed) {
                //    rdoPostgreSQL.Checked = true;
                //}
                //if (isSqlServer2008ExpressInstalled) {
                //    rdoSqlServer.Checked = true;
                //}
                //if (isMySQL51Installed) {
                //    rdoMySQL.Checked = true;
                //}

            }



            tvOptions.ExpandAll();

            tvOptions.SelectedNode = null;
            tvOptions.SelectedNode = tvOptions.Nodes[0];

        }

        private TreeNode getNode(string name, bool returnDefaultIfNotFound){
            TreeNode[] nodes = tvOptions.Nodes.Find(name, true);
            if (nodes.Length > 0) {
                return nodes[0];
            }
            if (returnDefaultIfNotFound) {
                return new TreeNode(name);
            } else {
                return null;
            }
        }

        private void checkNode(string name, bool option, bool forceOption) {
            TreeNode node = getNode(name, false);
            if (node != null){
                node.Checked = option || forceOption;
            }
        }

        private void displayNode(string name) {
            TreeNode nd = getNode(name, false);
            if (nd != null) {
                tvOptions.SelectedNode = nd;
                nd.EnsureVisible();
            }
        }

        private void refreshDatabaseEngineUtil() {
            _dbEngineUtil = null;

            _dbEngineUtil = DatabaseEngineUtil.CreateInstance(getEngineName(), Utility.GetTargetDirectory(null, null, "ConfigurationWizard") + @"\gguac.exe", txtSqlServerInstanceName.Text);
            if (!isSelectedDatabaseEngineInstalled) {
                lblDatabaseEngine.Text = _dbEngineUtil.FriendlyName + " will be installed.";
            } else {
                lblDatabaseEngine.Text = _dbEngineUtil.FriendlyName + " is already installed.";
            }
        }

        private List<ActionDetail> _actions;

        private void refreshActions() {



            // only show 'Use Windows Authentication' if SQL Server is the selected database engine
            chkUseWindowsLogon.Visible = getEngineName() == "sqlserver";


            _actions = new List<ActionDetail>();

            lbActions.Items.Clear();

//            string options = @" /L+*v c:\gginstaller_msilog.txt "; // /quiet
            string options = @" /passive  /L """ + (txtInstallDir.Text + @"\msilog.txt"" ").Replace(@"\\", @"\");

            string baseInstallDir = txtInstallDir.Text.Replace(@"""", @"\""");

            TreeNode ndDatabaseEngine = getNode("ndDatabaseEngine", true);
            ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.NoAction;
            TreeNode ndDatabaseTool = getNode("ndDatabaseTool", true);
            ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.NoAction;
            TreeNode ndDatabase = getNode("ndDatabase", true);
            ndDatabase.ImageIndex = ndDatabase.SelectedImageIndex = (int)ActionImage.NoAction;
            TreeNode ndWebApplication = getNode("ndWebApplication", true);
            ndWebApplication.ImageIndex = ndWebApplication.SelectedImageIndex = (int)ActionImage.NoAction;
            TreeNode ndSearchEngine = getNode("ndSearchEngine", true);
            ndSearchEngine.ImageIndex = ndSearchEngine.SelectedImageIndex = (int)ActionImage.NoAction;
            TreeNode ndCuratorTool = getNode("ndCuratorTool", true);
            ndCuratorTool.ImageIndex = ndCuratorTool.SelectedImageIndex = (int)ActionImage.NoAction;

            //TreeNode ndRoot = getNode("ndRoot", true);
            //ndRoot.ImageIndex = ndRoot.SelectedImageIndex = (int)ActionImage.NoAction;

            TreeNode ndServer = getNode("ndServer", true);
            ndServer.ImageIndex = ndServer.SelectedImageIndex = (int)ActionImage.NoAction;

            TreeNode ndClient = getNode("ndClient", true);
            ndClient.ImageIndex = ndClient.SelectedImageIndex = (int)ActionImage.NoAction;



//            lblSqlServerInstanceName.Enabled = txtSqlServerInstanceName.Enabled = rdoSqlServer.Checked;



            // sql server installer is ... not straightforward.  engine + tools are bundled together, and extracting it twice is really slow.
            // so we combine them into one action when we can. (i.e. either both are installing or both are uninstalling, or there's only one action to do)

            string sqlServerInstallFeatures = "";
            string sqlServerUninstallFeatures = "";
            string sqlServerInstallDisplayText = "";
            string sqlServerUninstallDisplayText = "";

            string sqlServerInstallSecurityMode = @"SECURITYMODE=""SQL""";

            if (ndDatabaseEngine.Checked) {
                if (ndDatabaseTool.Checked) {

                    // they want both engine + tools.

                    if (isSqlServer2008ExpressInstalled) {
                        if (isSqlServerExpressToolsInstalled) {

                            // both already installed
                            // nothing to do
                        } else {
                            // engine already installed, tools are not
                            sqlServerInstallFeatures = "SSMS";
                            sqlServerInstallDisplayText = "Install SQL Server Tools *";

                            // blank out securitymode, vista doesn't like this specified when just Tools are being installed
                            sqlServerInstallSecurityMode = "";
                        }
                    } else {
                        if (isSqlServerExpressToolsInstalled) {
                            // tools already installed, engine is not
                            sqlServerInstallFeatures = "SQLEngine";
                            sqlServerInstallDisplayText = "Install SQL Server Engine *";
                        } else {
                            // neither engine nor tools are installed
                            sqlServerInstallFeatures = "SQLEngine,SSMS";
                            sqlServerInstallDisplayText = "Install SQL Server Engine + Tools *";
                        }
                    }


                } else {

                    // they want engine installed, not tools

                    if (isSqlServer2008ExpressInstalled) {
                        if (isSqlServerExpressToolsInstalled) {
                            // both already installed

                            sqlServerUninstallFeatures = "SSMS";
                            sqlServerUninstallDisplayText = "Uninstall SQL Server Tools *";

                            // blank out securitymode, vista doesn't like this specified when just Tools are being installed
                            sqlServerInstallSecurityMode = "";

                        } else {
                            // engine already installed, tools are not
                            // nothing to do
                        }
                    } else {
                        if (isSqlServerExpressToolsInstalled) {
                            // tools already installed, engine is not
                            sqlServerUninstallFeatures = "SSMS";
                            sqlServerUninstallDisplayText = "Uninstall SQL Server Tools *";
                            sqlServerInstallFeatures = "SQLEngine";
                            sqlServerInstallDisplayText = "Install SQL Server Engine *";
                        } else {
                            // neither engine nor tools are installed
                            sqlServerInstallFeatures = "SQLEngine";
                            sqlServerInstallDisplayText = "Install SQL Server Engine *";
                        }
                    }

                }
            } else {
                if (ndDatabaseTool.Checked) {

                    // they want just tools, not engine

                    if (isSqlServer2008ExpressInstalled) {

                        // they don't want the engine...

                        if (isSqlServerExpressToolsInstalled) {
                            // both already installed
                            sqlServerUninstallFeatures = "SQLEngine";
                            sqlServerUninstallDisplayText = "Uninstall SQL Server Engine *";
                        } else {
                            // engine already installed, tools are not
                            sqlServerUninstallFeatures = "SQLEngine";
                            sqlServerUninstallDisplayText = "Uninstall SQL Server Engine *";
                            sqlServerInstallFeatures = "SSMS";
                            sqlServerInstallDisplayText = "Install SQL Server Tools *";

                            // blank out securitymode, vista doesn't like this specified when just Tools are being installed
                            sqlServerInstallSecurityMode = "";
                        }
                    } else {


                        if (isSqlServerExpressToolsInstalled) {
                            // tools already installed, engine is not
                            // nothing to do.
                        } else {
                            // neither engine nor tools are installed
                            sqlServerInstallFeatures = "SSMS";
                            sqlServerInstallDisplayText = "Install SQL Server Tools *";

                            // blank out securitymode, vista doesn't like this specified when just Tools are being installed
                            sqlServerInstallSecurityMode = "";
                        }
                    }


                } else {

                    // they want neither engine nor tools

                    if (isSqlServer2008ExpressInstalled) {
                        if (isSqlServerExpressToolsInstalled) {
                            // both already installed
                            sqlServerUninstallFeatures = "SQLEngine,SSMS";
                            sqlServerUninstallDisplayText = "Uninstall SQL Server Engine + Tools *";

                        } else {
                            // engine already installed, tools are not
                            sqlServerUninstallFeatures = "SQLEngine";
                            sqlServerUninstallDisplayText = "Uninstall SQL Server Engine *";
                        }
                    } else {
                        if (isSqlServerExpressToolsInstalled) {
                            // tools already installed, engine is not
                            sqlServerUninstallFeatures = "SSMS";
                            sqlServerUninstallDisplayText = "Uninstall SQL Server Tools *";
                        } else {
                            // neither engine nor tools are installed
                            // nothing to do
                        }
                    }

                }
            }

            string sqlServerSAPassword = (String.IsNullOrEmpty(sqlServerInstallSecurityMode) ? "" : @" /SAPWD=""__PASSWORD__"" ");

            string sqlServerDisplayText = sqlServerInstallDisplayText + (sqlServerUninstallDisplayText.Length > 0 ? ", " + sqlServerUninstallDisplayText : "");


            Dictionary<string, string> sqlServerConfigDictionary = new Dictionary<string,string>();
            sqlServerConfigDictionary.Add("__INSTALLFEATURES__", sqlServerInstallFeatures);
            sqlServerConfigDictionary.Add("__UNINSTALLFEATURES__", sqlServerUninstallFeatures);
            sqlServerConfigDictionary.Add("__INSTALLSECURITYMODE__", sqlServerInstallSecurityMode);
            sqlServerConfigDictionary.Add("__INSTANCENAME__", txtSqlServerInstanceName.Text); // TODO: GrinGlobal???
            sqlServerConfigDictionary.Add("__SECURITYMODE__", (String.IsNullOrEmpty(sqlServerSAPassword) ? "" : @"SECURITYMODE=""SQL"""));

            if (ndDatabaseEngine.Checked){
                if (rdoMySQL.Checked){
                    if (!isMySQL51Installed) {
                        _actions.Add(
                            new ActionDetail {
                                FriendlyName = "Install MySQL",
                                PrimaryExe = "cmd.exe",
                                PrimaryArgs = "/c start /wait " + Utility.GetSystem32Directory() + @"msiexec.exe " + options + @" /i ""__BINDIR__\mysql-essential-5_1_34-win32.msi""",
                                PrimaryWorkingDirectory = baseInstallDir
                            });
                            
                        lbActions.Items.Add("Install MySQL *");
                        ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.Install;
                    }
                    if (ndDatabaseTool.Checked && !isMySQLToolsInstalled) {
                        _actions.Add(
                            new ActionDetail {
                                FriendlyName = "Install MySQL Tools",
                                PrimaryExe = "cmd.exe",
                                PrimaryArgs = "/c start /wait " + Utility.GetSystem32Directory() + @"msiexec.exe " + options + @" /i ""__BINDIR__\mysql-gui-tools-5_0-r17-win32.msi""",
                                PrimaryWorkingDirectory = baseInstallDir 
                            });
                        
                        lbActions.Items.Add("Install MySQL Tools");
                        ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.Install;
                    }
                } else if (rdoSqlServer.Checked){

                    if (isSqlServer2008ExpressInstalled) {

                        // sql server is already installed, engine node is checked

                        if (ndDatabaseTool.Checked) {
                            if (isSqlServerExpressToolsInstalled) {

                                // both are already installed. nothing to do.
                                ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.NoAction;
                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.NoAction;

                            } else {

                                if (!Utility.IsPowershellInstalled) {
                                    if (Utility.IsWindowsXP) {
                                        _actions.Add(new ActionDetail {
                                            FriendlyName = "Install Powershell (for SQL Server)",
                                            PrimaryExe = "cmd.exe",
                                            PrimaryArgs = @"/c start /wait update.exe " + options.Replace(" /L ", " /log:"),
                                            PrimaryWorkingDirectory = @"__TEMPBINDIR__\update\",
                                            TempExe = @"""__BINDIR__\WindowsXP-KB926139-v2-x86-ENU.exe""",
                                            TempArgs = @" /passive /x:""__TEMPBINDIR__"""
                                        });

                                        lbActions.Items.Add("Install XP Powershell (for SQL Server)");

                                    } else if (Utility.IsWindowsVista) {
                                        _actions.Add(new ActionDetail {
                                            FriendlyName = "Install Powershell (for SQL Server)",
                                            PrimaryExe = "cmd.exe",
                                            PrimaryArgs = @"/c start /wait pkgmgr.exe /ip /m:__TEMPBINDIR__\Windows6.0-KB928439-x86.cab ",
                                            PrimaryWorkingDirectory = @"__TEMPBINDIR__\",
                                            TempExe = "expand.exe",
                                            TempArgs = @" ""__BINDIR__\Windows6_0-KB928439-x86.msu"" -F:* __TEMPBINDIR__"
                                        });

                                        lbActions.Items.Add("Install Vista Powershell (for SQL Server)");

                                    }
                                }


                                // engine is installed, tools are not.  they're saying they want the tools added.
                                _actions.Add(
                                    new ActionDetail {
                                        FriendlyName = sqlServerInstallDisplayText.Replace(" *", ""),
                                        PrimaryExe = @"""__TEMPBINDIR__\setup.exe""",
                                        PrimaryArgs = @" /CONFIGURATIONFILE=""__TEMPBINDIR__\sqlserverconfig.ini"" " + sqlServerSAPassword,
                                        PrimaryWorkingDirectory = @"__TEMPBINDIR__",
                                        TempExe = @"""__BINDIR__\SQLEXPRWT_x86_ENU.exe""", 
                                        TempArgs = @" /passive /x:""__TEMPBINDIR__""",
                                        SpecialCommand = "sqlserver_install_tools",
                                        ConfigurationDictionary = sqlServerConfigDictionary
                                    });
                                lbActions.Items.Add(sqlServerInstallDisplayText);

                                ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.NoAction;
                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.Install;

                            }
                        } else {
                            if (isSqlServerExpressToolsInstalled) {

                                // engine is installed, tools are too.  they're saying they want the tools uninstalled.



                                //ActionDetail newAction = new ActionDetail {
                                //    FriendlyName = sqlServerUninstallDisplayText.Replace(" *", ""),
                                //    PrimaryExe = @"""__TEMPBINDIR__\setup.exe""", 
                                //    PrimaryArgs = @" /CONFIGURATIONFILE=""__TEMPBINDIR__\sqlserverconfig.ini"" ",
                                //    PrimaryWorkingDirectory = @"__TEMPBINDIR__", 
                                //    TempExe = @"""__BINDIR__\SQLEXPRWT_x86_ENU.exe""", 
                                //    TempArgs = @" /passive /x:""__TEMPBINDIR__""",
                                //    SpecialCommand = "sqlserver_uninstall_tools",
                                //    ConfigurationDictionary = sqlServerConfigDictionary
                                //};

                                ActionDetail newAction = null;
                                string sqlServerToolsUninstall = Utility.GetUninstallCommand("Microsoft SQL Server 2008 Management Studio", null);
                                if (String.IsNullOrEmpty(sqlServerToolsUninstall)) {
                                    newAction = new ActionDetail {
                                        FriendlyName = sqlServerUninstallDisplayText.Replace(" *", ""),
                                        PrimaryExe = @"""__TEMPBINDIR__\setup.exe""",
                                        PrimaryArgs = @" /CONFIGURATIONFILE=""__TEMPBINDIR__\sqlserverconfig.ini"" ",
                                        PrimaryWorkingDirectory = @"__TEMPBINDIR__",
                                        TempExe = @"""__BINDIR__\SQLEXPRWT_x86_ENU.exe""",
                                        TempArgs = @" /passive /x:""__TEMPBINDIR__""",
                                        SpecialCommand = "sqlserver_uninstall_tools",
                                        ConfigurationDictionary = sqlServerConfigDictionary
                                    };
                                } else {
                                    newAction = new ActionDetail {
                                        FriendlyName = sqlServerUninstallDisplayText.Replace(" *", ""),
                                        PrimaryExe = "cmd.exe",
                                        PrimaryArgs = " /c start /wait " + sqlServerToolsUninstall.Replace("/passive", options) + " ",
                                        PrimaryWorkingDirectory = Utility.GetSystem32Directory(),
                                    };
                                }



                                if (lbActions.Items.Count > 0 && lbActions.Items[lbActions.Items.Count - 1].ToString().StartsWith("Uninstall")) {
                                    _actions.Insert(_actions.Count - 1, newAction);
                                    lbActions.Items.Insert(lbActions.Items.Count - 1, sqlServerUninstallDisplayText);
                                } else {
                                    _actions.Add(newAction);
                                    lbActions.Items.Add(sqlServerUninstallDisplayText);
                                }

                                ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.NoAction;
                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.Uninstall;


                            } else {
                                // engine is installed, tools are not.  they want exactly that.  nothing to do.
                                ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.NoAction;
                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.NoAction;

                            }
                        }


                    } else {

                        // sql server is not installed yet, they want it installed

                        // add powershell if it's not already there (different installers for XP and Vista)
                        if (!Utility.IsPowershellInstalled) {
                            if (Utility.IsWindowsXP) {
                                _actions.Add(new ActionDetail {
                                    FriendlyName = "Install Powershell (for SQL Server)",
                                    PrimaryExe = "cmd.exe",
                                    PrimaryArgs = @"/c start /wait update.exe " + options.Replace(" /L ", " /log:"),
                                    PrimaryWorkingDirectory = @"__TEMPBINDIR__\update\",
                                    TempExe = @"""__BINDIR__\WindowsXP-KB926139-v2-x86-ENU.exe""",
                                    TempArgs = @" /passive /x:""__TEMPBINDIR__"""
                                });

                                lbActions.Items.Add("Install XP Powershell (for SQL Server)");

                            } else if (Utility.IsWindowsVista) {
                                _actions.Add(new ActionDetail {
                                    FriendlyName = "Install Powershell (for SQL Server)",
                                    PrimaryExe = "cmd.exe",
                                    PrimaryArgs = @"/c start /wait pkgmgr.exe /ip /m:__TEMPBINDIR__\Windows6.0-KB928439-x86.cab ",
                                    PrimaryWorkingDirectory = @"__TEMPBINDIR__\",
                                    TempExe = "expand.exe",
                                    TempArgs = @" ""__BINDIR__\Windows6_0-KB928439-x86.msu"" -F:* __TEMPBINDIR__"
                                });

                                lbActions.Items.Add("Install Vista Powershell (for SQL Server)");

                            }
                        }


                        if (ndDatabaseTool.Checked) {
                            if (isSqlServerExpressToolsInstalled) {

                                // engine is not installed, tools are installed.  they want both installed. (add the engine)
                                _actions.Add(
                                    new ActionDetail {
                                        FriendlyName = sqlServerInstallDisplayText.Replace(" *", ""), 
                                        PrimaryExe = @"""__TEMPBINDIR__\setup.exe""",
                                        PrimaryArgs = @" /CONFIGURATIONFILE=""__TEMPBINDIR__\sqlserverconfig.ini"" " + sqlServerSAPassword,
                                        PrimaryWorkingDirectory = @"__TEMPBINDIR__", 
                                        TempExe = @"""__BINDIR__\SQLEXPRWT_x86_ENU.exe""", 
                                        TempArgs = @" /passive /x:""__TEMPBINDIR__""",
                                        SpecialCommand = "sqlserver_install_engine",
                                    ConfigurationDictionary = sqlServerConfigDictionary
                                    });
                                lbActions.Items.Add(sqlServerInstallDisplayText);

                                ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.Install;
                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.NoAction;

                            } else {

                                // engine is not installed, tools are not either.  they want both installed.

                                _actions.Add(
                                    new ActionDetail {
                                        FriendlyName = sqlServerInstallDisplayText.Replace(" *", ""), 
                                        PrimaryExe = @"""__TEMPBINDIR__\setup.exe""",
                                        PrimaryArgs = @" /CONFIGURATIONFILE=""__TEMPBINDIR__\sqlserverconfig.ini"" " + sqlServerSAPassword,
                                        PrimaryWorkingDirectory = @"__TEMPBINDIR__",
                                        TempExe = @"""__BINDIR__\SQLEXPRWT_x86_ENU.exe""", 
                                        TempArgs = @" /passive /x:""__TEMPBINDIR__""" ,
                                        SpecialCommand = "sqlserver_install_engine_tools",
                                        ConfigurationDictionary = sqlServerConfigDictionary
                                    });
                                lbActions.Items.Add(sqlServerInstallDisplayText);

                                ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.Install;
                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.Install;

                            }
                        } else {
                            if (isSqlServerExpressToolsInstalled) {

                                // engine is not installed, tools are.  they want engine installed but not the tools.

                                // install engine.
                                _actions.Add(
                                    new ActionDetail {
                                        FriendlyName = sqlServerInstallDisplayText.Replace(" *", ""), 
                                        PrimaryExe = @"""__TEMPBINDIR__\setup.exe""",
                                        PrimaryArgs = @" /CONFIGURATIONFILE=""__TEMPBINDIR__\sqlserverconfig.ini"" " + sqlServerSAPassword,
                                        PrimaryWorkingDirectory = @"__TEMPBINDIR__",
                                        TempExe = @"""__BINDIR__\SQLEXPRWT_x86_ENU.exe""", 
                                        TempArgs = @" /passive /x:""__TEMPBINDIR__""",
                                        SpecialCommand = "sqlserver_install_engine",
                                        ConfigurationDictionary = sqlServerConfigDictionary

                                    });

                                lbActions.Items.Add(sqlServerInstallDisplayText);

                                ndDatabaseEngine.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.Install;

                                // uninstall tools.
                                ActionDetail newAction = null;
                                string sqlServerToolsUninstall = Utility.GetUninstallCommand("Microsoft SQL Server 2008 Management Studio", null);
                                if (String.IsNullOrEmpty(sqlServerToolsUninstall)) {
                                    newAction = new ActionDetail {
                                        FriendlyName = sqlServerUninstallDisplayText.Replace(" *", ""),
                                        PrimaryExe = @"""__TEMPBINDIR__\setup.exe""",
                                        PrimaryArgs = @" /CONFIGURATIONFILE=""__TEMPBINDIR__\sqlserverconfig.ini"" ",
                                        PrimaryWorkingDirectory = @"__TEMPBINDIR__",
                                        TempExe = @"""__BINDIR__\SQLEXPRWT_x86_ENU.exe""",
                                        TempArgs = @" /passive /x:""__TEMPBINDIR__""",
                                        SpecialCommand = "sqlserver_uninstall_tools",
                                        ConfigurationDictionary = sqlServerConfigDictionary
                                    };
                                } else {
                                    newAction = new ActionDetail {
                                        FriendlyName = sqlServerUninstallDisplayText.Replace(" *", ""),
                                        PrimaryExe = "cmd.exe",
                                        PrimaryArgs = " /c start /wait " + sqlServerToolsUninstall.Replace("/passive", options) + " ",
                                        PrimaryWorkingDirectory = Utility.GetSystem32Directory(),
                                    };
                                }

                                if (lbActions.Items.Count > 0 && lbActions.Items[lbActions.Items.Count - 1].ToString().StartsWith("Uninstall")) {
                                    _actions.Insert(_actions.Count - 1, newAction);
                                    lbActions.Items.Insert(lbActions.Items.Count - 1, sqlServerUninstallDisplayText);
                                } else {
                                    _actions.Add(newAction);
                                    lbActions.Items.Add(sqlServerUninstallDisplayText);
                                }
                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.Uninstall;

                            } else {

                                // engine is not installed, tools are not installed.  they want engine installed.

                                // for info on parameters: http://msdn.microsoft.com/en-us/library/ms144259.aspx
                                _actions.Add(new ActionDetail {
                                    FriendlyName = sqlServerInstallDisplayText.Replace(" *", ""), 
                                    PrimaryExe = @"""__TEMPBINDIR__\setup.exe""",
                                    PrimaryArgs = @" /CONFIGURATIONFILE=""__TEMPBINDIR__\sqlserverconfig.ini"" " + sqlServerSAPassword,
                                    PrimaryWorkingDirectory = @"__TEMPBINDIR__", 
                                    TempExe = @"""__BINDIR__\SQLEXPRWT_x86_ENU.exe""",
                                    TempArgs = @" /passive /x:""__TEMPBINDIR__""" ,
                                    SpecialCommand = "sqlserver_install_engine",
                                    ConfigurationDictionary = sqlServerConfigDictionary
                                });
                                
                                lbActions.Items.Add(sqlServerInstallDisplayText);

                                ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.Install;

                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.NoAction;

                            }
                        }

                    }

                } else if (rdoOracle.Checked && !isOracle10gExpressInstalled) {
                    // TODO: get proper msi for sql server 2008 express
                    _actions.Add(new ActionDetail {
                        FriendlyName = "Install Oracle", 
                        PrimaryExe = "echo.exe",
                        PrimaryArgs = options + @" What to do now???", 
                        PrimaryWorkingDirectory = baseInstallDir
                    });
                        
                    lbActions.Items.Add("Install Oracle *");
                    ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.Install;
                } else if (rdoPostgreSQL.Checked && !isPostgreSQL83Installed) {
                    // TODO: get proper msi for sql server 2008 express
                    _actions.Add(new ActionDetail {
                        FriendlyName = "Install PostgreSQL",
                        PrimaryExe = "echo.exe",
                        PrimaryArgs = options + @" What to do now???",
                        PrimaryWorkingDirectory = baseInstallDir
                    });

                    lbActions.Items.Add("Install PostgreSQL *");
                    ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.Install;
                }
            } else {
                if (rdoMySQL.Checked){
                    if (isMySQL51Installed) {
                        ActionDetail newAction = new ActionDetail {
                            FriendlyName = "Uninstall MySQL",
                            PrimaryExe = "cmd.exe",
                            PrimaryArgs = "/c start /wait " + Utility.GetSystem32Directory() + @"msiexec.exe " + options + @"  /x ""__BINDIR__\mysql-essential-5_1_34-win32.msi""", 
                            PrimaryWorkingDirectory = baseInstallDir
                        };
                        if (lbActions.Items.Count > 0 && lbActions.Items[lbActions.Items.Count - 1].ToString().StartsWith("Uninstall")) {
                            _actions.Insert(_actions.Count - 1, newAction);
                            lbActions.Items.Insert(lbActions.Items.Count - 1, "Uninstall MySQL *");
                        } else {
                            _actions.Add(newAction);
                            lbActions.Items.Add("Uninstall MySQL *");
                        }
                        ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.Uninstall;
                    }
                    if (!ndDatabaseTool.Checked && isMySQLToolsInstalled) {
                        _actions.Add(new ActionDetail {
                            FriendlyName = "Uninstall MySQL Tools", 
                            PrimaryExe = "cmd.exe",
                            PrimaryArgs = "/c start /wait " + Utility.GetSystem32Directory() + @"msiexec.exe " + options + @" /x ""__BINDIR__\mysql-gui-tools-5_0-r17-win32.msi""", 
                            PrimaryWorkingDirectory = baseInstallDir
                        });
                        lbActions.Items.Add("Uninstall MySQL Tools");
                        ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.Uninstall;
                    }
                } else if (rdoSqlServer.Checked){


                    // engine node is NOT checked

                    if (isSqlServer2008ExpressInstalled) {

                        // engine node is NOT checked, sql server is installed already


                        ActionDetail newAction = null;
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("InstanceId", "MSSQL10." + txtSqlServerInstanceName.Text);
                        string sqlServerEngineUninstall = Utility.GetUninstallCommand("Microsoft SQL Server 2008 Database Engine Services", dic);
                        if (String.IsNullOrEmpty(sqlServerEngineUninstall)) {
                            // for info on parameters: http://msdn.microsoft.com/en-us/library/ms144259.aspx
                            newAction = new ActionDetail {
                                FriendlyName = sqlServerUninstallDisplayText.Replace(" *", ""),
                                PrimaryExe = @"""__TEMPBINDIR__\setup.exe""",
                                PrimaryArgs = @" /CONFIGURATIONFILE=""__TEMPBINDIR__\sqlserverconfig.ini"" ",
                                PrimaryWorkingDirectory = @"__TEMPBINDIR__",
                                TempExe = @"""__BINDIR__\SQLEXPRWT_x86_ENU.exe""",
                                TempArgs = @" /passive /x:""__TEMPBINDIR__""",
                                SpecialCommand = "sqlserver_uninstall_engine",
                                ConfigurationDictionary = sqlServerConfigDictionary
                            };
                        } else {
                            newAction = new ActionDetail {
                                FriendlyName = sqlServerUninstallDisplayText.Replace(" *", ""),
                                PrimaryExe = "cmd.exe",
                                PrimaryArgs = " /c start /wait " + sqlServerEngineUninstall.Replace("/passive", options) + " ",
                                PrimaryWorkingDirectory = Utility.GetSystem32Directory(),
                            };
                        }




                        if (lbActions.Items.Count > 0 && lbActions.Items[lbActions.Items.Count - 1].ToString().StartsWith("Uninstall")) {
                            _actions.Insert(_actions.Count - 1, newAction);
                            lbActions.Items.Insert(lbActions.Items.Count - 1, sqlServerUninstallDisplayText);
                        } else {
                            _actions.Add(newAction);
                            lbActions.Items.Add(sqlServerUninstallDisplayText);
                        }
                        ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.Uninstall;

                        if (isSqlServerExpressToolsInstalled) {
                            if (ndDatabaseTool.Checked) {
                                // sql server is installed, tools are installed, they want engine + tools. nothing to do.
                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.NoAction;
                            } else {
                                // sql server is installed, tools are installed, they want engine but not tools.  uninstall tools.

                                ActionDetail newAction2 = null;
                                string sqlServerToolsUninstall = Utility.GetUninstallCommand("Microsoft SQL Server 2008 Management Studio", null);
                                if (String.IsNullOrEmpty(sqlServerToolsUninstall)) {
                                    newAction2 = new ActionDetail {
                                        FriendlyName = sqlServerUninstallDisplayText.Replace(" *", ""),
                                        PrimaryExe = @"""__TEMPBINDIR__\setup.exe""",
                                        PrimaryArgs = @" /CONFIGURATIONFILE=""__TEMPBINDIR__\sqlserverconfig.ini"" ",
                                        PrimaryWorkingDirectory = @"__TEMPBINDIR__",
                                        TempExe = @"""__BINDIR__\SQLEXPRWT_x86_ENU.exe""",
                                        TempArgs = @" /passive /x:""__TEMPBINDIR__""",
                                        SpecialCommand = "sqlserver_uninstall_tools",
                                        ConfigurationDictionary = sqlServerConfigDictionary
                                    };
                                } else {
                                    newAction2 = new ActionDetail {
                                        FriendlyName = sqlServerUninstallDisplayText.Replace(" *", ""),
                                        PrimaryExe = "cmd.exe",
                                        PrimaryArgs = " /c start /wait " + sqlServerToolsUninstall.Replace("/passive", options) + " "
                                    };
                                }



                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.Uninstall;
                            }
                        } else {

                            // sql server is installed, tools are not

                            if (ndDatabaseTool.Checked) {

                                if (!Utility.IsPowershellInstalled) {
                                    if (Utility.IsWindowsXP) {
                                        _actions.Add(new ActionDetail {
                                            FriendlyName = "Install Powershell (for SQL Server)",
                                            PrimaryExe = "cmd.exe",
                                            PrimaryArgs = @"/c start /wait update.exe " + options.Replace(" /L ", " /log:"),
                                            PrimaryWorkingDirectory = @"__TEMPBINDIR__\update\",
                                            TempExe = @"""__BINDIR__\WindowsXP-KB926139-v2-x86-ENU.exe""",
                                            TempArgs = @" /passive /x:""__TEMPBINDIR__"""
                                        });

                                        lbActions.Items.Add("Install XP Powershell (for SQL Server)");

                                    } else if (Utility.IsWindowsVista) {
                                        _actions.Add(new ActionDetail {
                                            FriendlyName = "Install Powershell (for SQL Server)",
                                            PrimaryExe = "cmd.exe",
                                            PrimaryArgs = @"/c start /wait pkgmgr.exe /ip /m:__TEMPBINDIR__\Windows6.0-KB928439-x86.cab ",
                                            PrimaryWorkingDirectory = @"__TEMPBINDIR__\",
                                            TempExe = "expand.exe",
                                            TempArgs = @" ""__BINDIR__\Windows6_0-KB928439-x86.msu"" -F:* __TEMPBINDIR__"
                                        });

                                        lbActions.Items.Add("Install Vista Powershell (for SQL Server)");

                                    }
                                }



                                // sql server is installed, tools are NOT installed, they want engine and tools.
                                _actions.Add(new ActionDetail { 
                                    FriendlyName = sqlServerInstallDisplayText.Replace(" *", ""),
                                    PrimaryExe = @"""__TEMPBINDIR__\setup.exe""",
                                    PrimaryArgs = @" /CONFIGURATIONFILE=""__TEMPBINDIR__\sqlserverconfig.ini"" " + sqlServerSAPassword,
                                    PrimaryWorkingDirectory = @"__TEMPBINDIR__",
                                    TempExe = @"""__BINDIR__\SQLEXPRWT_x86_ENU.exe""",
                                    TempArgs = @" /passive /x:""__TEMPBINDIR__""",
                                    SpecialCommand = "sqlserver_install_tools",
                                    ConfigurationDictionary = sqlServerConfigDictionary
                                });
                                lbActions.Items.Add(sqlServerInstallDisplayText);
                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.Install;
                            } else {
                                // sql server is installed, tools are NOT installed, they want engine but not tools. nothing to do.
                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.NoAction;
                            }
                        }


                    } else {

                        // sql server is NOT installed, they are NOT asking for engine

                        ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.NoAction;

                        if (isSqlServerExpressToolsInstalled) {
                            if (ndDatabaseTool.Checked) {
                                // engine NOT installed, tools installed, they want just tools. nothing to do.
                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.NoAction;
                            } else {

                                // engine NOT installed, tools installed, they want neither. uninstall tools.

                                // for info on parameters: http://msdn.microsoft.com/en-us/library/ms144259.aspx
                                ActionDetail newAction = null;
                                string sqlServerToolsUninstall = Utility.GetUninstallCommand("Microsoft SQL Server 2008 Management Studio", null);
                                if (String.IsNullOrEmpty(sqlServerToolsUninstall)) {
                                    newAction = new ActionDetail {
                                        FriendlyName = sqlServerUninstallDisplayText.Replace(" *", ""),
                                        PrimaryExe = @"""__TEMPBINDIR__\setup.exe""",
                                        PrimaryArgs = @" /CONFIGURATIONFILE=""__TEMPBINDIR__\sqlserverconfig.ini"" ",
                                        PrimaryWorkingDirectory = @"__TEMPBINDIR__",
                                        TempExe = @"""__BINDIR__\SQLEXPRWT_x86_ENU.exe""",
                                        TempArgs = @" /passive /x:""__TEMPBINDIR__""",
                                        SpecialCommand = "sqlserver_uninstall_tools",
                                        ConfigurationDictionary = sqlServerConfigDictionary
                                    };
                                } else {
                                    newAction = new ActionDetail {
                                        FriendlyName = sqlServerUninstallDisplayText.Replace(" *", ""),
                                        PrimaryExe = "cmd.exe",
                                        PrimaryArgs = " /c start /wait " + sqlServerToolsUninstall.Replace("/passive", options) + " ",
                                        PrimaryWorkingDirectory = Utility.GetSystem32Directory(),
                                    };
                                }

                                if (lbActions.Items.Count > 0 && lbActions.Items[lbActions.Items.Count - 1].ToString().StartsWith("Uninstall")) {
                                    _actions.Insert(_actions.Count - 1, newAction);
                                    lbActions.Items.Insert(lbActions.Items.Count - 1, sqlServerUninstallDisplayText);
                                } else {
                                    _actions.Add(newAction);
                                    lbActions.Items.Add(sqlServerUninstallDisplayText);
                                }

                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.Uninstall;


                            }
                        } else {
                            if (ndDatabaseTool.Checked) {

                                if (!Utility.IsPowershellInstalled) {
                                    if (Utility.IsWindowsXP) {
                                        _actions.Add(new ActionDetail {
                                            FriendlyName = "Install Powershell (for SQL Server)",
                                            PrimaryExe = "cmd.exe",
                                            PrimaryArgs = @"/c start /wait update.exe " + options.Replace(" /L ", " /log:"),
                                            PrimaryWorkingDirectory = @"__TEMPBINDIR__\update\",
                                            TempExe = @"""__BINDIR__\WindowsXP-KB926139-v2-x86-ENU.exe""",
                                            TempArgs = @" /passive /x:""__TEMPBINDIR__"""
                                        });

                                        lbActions.Items.Add("Install XP Powershell (for SQL Server)");

                                    } else if (Utility.IsWindowsVista) {
                                        _actions.Add(new ActionDetail {
                                            FriendlyName = "Install Powershell (for SQL Server)",
                                            PrimaryExe = "cmd.exe",
                                            PrimaryArgs = @"/c start /wait pkgmgr.exe /ip /m:__TEMPBINDIR__\Windows6.0-KB928439-x86.cab ",
                                            PrimaryWorkingDirectory = @"__TEMPBINDIR__\",
                                            TempExe = "expand.exe",
                                            TempArgs = @" ""__BINDIR__\Windows6_0-KB928439-x86.msu"" -F:* __TEMPBINDIR__"
                                        });

                                        lbActions.Items.Add("Install Vista Powershell (for SQL Server)");

                                    }
                                }


                                // engine NOT instlled, tools NOT installed, asking for tools install.
                                _actions.Add(new ActionDetail {
                                    FriendlyName = sqlServerInstallDisplayText.Replace(" *", ""),
                                    PrimaryExe = @"""__TEMPBINDIR__\setup.exe""",
                                    PrimaryArgs = @" /CONFIGURATIONFILE=""__TEMPBINDIR__\sqlserverconfig.ini"" " + sqlServerSAPassword,
                                    PrimaryWorkingDirectory = @"__TEMPBINDIR__",
                                    TempExe = @"""__BINDIR__\SQLEXPRWT_x86_ENU.exe""",
                                    TempArgs = @" /passive /x:""__TEMPBINDIR__""",
                                    SpecialCommand = "sqlserver_install_tools",
                                    ConfigurationDictionary = sqlServerConfigDictionary
                                });
                                lbActions.Items.Add(sqlServerInstallDisplayText);

                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.Install;

                            } else {
                                // engine NOT installed, tools NOT installed, asking for no tools. nothing to do.
                                ndDatabaseTool.ImageIndex = ndDatabaseTool.SelectedImageIndex = (int)ActionImage.NoAction;
                            }
                        }

                    }



                } else if (rdoOracle.Checked && isOracle10gExpressInstalled) {
                    // TODO: get proper msi for oracle
                    ActionDetail newAction = new ActionDetail {
                        FriendlyName = "Uninstall Oracle",
                        PrimaryExe = @"echo.exe",
                        PrimaryArgs = @" What to do now??? ",
                        PrimaryWorkingDirectory = baseInstallDir };
                    if (lbActions.Items.Count > 0 && lbActions.Items[lbActions.Items.Count-1].ToString().StartsWith("Uninstall")) {
                        _actions.Insert(_actions.Count - 1, newAction);
                        lbActions.Items.Insert(lbActions.Items.Count - 1, "Uninstall Oracle *");
                    } else {
                        _actions.Add(newAction);
                        lbActions.Items.Add("Uninstall Oracle *");
                    }
                    ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.Uninstall;
                } else if (rdoPostgreSQL.Checked && isPostgreSQL83Installed) {
                    // TODO: get proper msi for PostgreSQL
                    ActionDetail newAction = new ActionDetail {
                        FriendlyName = "Uninstall PostgreSQL",
                        PrimaryExe = @"echo.exe",
                        PrimaryArgs = @" What to do now??? ",
                        PrimaryWorkingDirectory = baseInstallDir };
                    if (lbActions.Items.Count > 0 && lbActions.Items[lbActions.Items.Count-1].ToString().StartsWith("Uninstall")) {
                        _actions.Insert(_actions.Count - 1, newAction);
                        lbActions.Items.Insert(lbActions.Items.Count - 1, "Uninstall PostgreSQL *");
                    } else {
                        _actions.Add(newAction);
                        lbActions.Items.Add("Uninstall PostgreSQL *");
                    }
                    ndDatabaseEngine.ImageIndex = ndDatabaseEngine.SelectedImageIndex = (int)ActionImage.Uninstall;
                }
            }

            if (ndDatabase.Checked) {
                if (!isDatabaseInstalled) {
                    _actions.Add(new ActionDetail {
                        FriendlyName = "Install Database",
                        PrimaryExe = "cmd.exe",
                        PrimaryArgs = "/c start /wait " + Utility.GetSystem32Directory() + @"msiexec.exe " + options + @" /i ""__BINDIR__\GrinGlobal_Database_Setup.msi"" " + _dbEngineUtil.InstallParameter + @" PASSWORD=""__PASSWORD__"" TARGETDIR=""" + baseInstallDir + @"\GRIN-Global Database\"" INSTANCENAME=""" + txtSqlServerInstanceName.Text + @""" WINDOWSAUTH=""" + (chkUseWindowsLogon.Checked ? "1" : "0") + @"""",
                        PrimaryWorkingDirectory = baseInstallDir + @"\GRIN-Global Database\"
                    });
                    lbActions.Items.Add("Install Database *");
                    ndDatabase.ImageIndex = ndDatabase.SelectedImageIndex = (int)ActionImage.Install;
                }
            } else if (isDatabaseInstalled) {

                string args = "/c start /wait " + Utility.GetSystem32Directory() + @"msiexec.exe " + options + @"  /x ""__BINDIR__\GrinGlobal_Database_Setup.msi"" " + _dbEngineUtil.InstallParameter + @" PASSWORD=""__PASSWORD__"" TARGETDIR=""" + baseInstallDir + @"\GRIN-Global Database\"" ";
                string uninstallString = Utility.GetUninstallCommand("GRIN-Global Database", null);
                if (!String.IsNullOrEmpty(uninstallString)) {
                    args = "/c start /wait " + uninstallString.Replace("/passive", options + " " + _dbEngineUtil.InstallParameter + " ") + " ";
                }

                ActionDetail newAction = new ActionDetail {
                    FriendlyName = "Uninstall Database",
                    PrimaryExe = "cmd.exe",
                    PrimaryArgs = args,
                    PrimaryWorkingDirectory = baseInstallDir + @"\GRIN-Global Database\"
                };
                if (lbActions.Items.Count > 0 && lbActions.Items[lbActions.Items.Count-1].ToString().StartsWith("Uninstall")) {
                    _actions.Insert(_actions.Count - 1, newAction);
                    lbActions.Items.Insert(lbActions.Items.Count - 1, "Uninstall Database *");
                } else {
                    _actions.Add(newAction);
                    lbActions.Items.Add("Uninstall Database *");
                }
                ndDatabase.ImageIndex = ndDatabase.SelectedImageIndex = (int)ActionImage.Uninstall;
            }

            //if (!isDatabaseEngineInstalled) {
            //    lblDatabasePassword.Text = "The password you provide will be ";
            //} else {
            //    lblDatabasePassword.Text = "";
            //}


            if (ndWebApplication.Checked) {
                if (!isIISInstalled) {
                    _actions.Add(new ActionDetail {
                        FriendlyName = "Install IIS",
                        SpecialCommand = "iis_install"
                    });
                    lbActions.Items.Add("Install IIS");
                    lblIIS.Text = "IIS will be installed because it is required by the Web Application.\n\nYou may be prompted to insert your Windows XP CD.";
                }
                if (!isWebApplicationInstalled) {
                    _actions.Add(new ActionDetail {
                        FriendlyName = "Install Web Application",
                        PrimaryExe = "cmd.exe",
                        PrimaryArgs = "/c start /wait " + Utility.GetSystem32Directory() + @"msiexec.exe " + options + @" /i ""__BINDIR__\GrinGlobal_Web_Setup.msi"" TARGETVDIR=gringlobal ",
                        PrimaryWorkingDirectory = baseInstallDir
                    });
                    lbActions.Items.Add("Install Web Application");
                    ndWebApplication.ImageIndex = ndWebApplication.SelectedImageIndex = (int)ActionImage.Install;
                }
            } else if (isWebApplicationInstalled) {


                string args = "/c start /wait " + Utility.GetSystem32Directory() + @"msiexec.exe " + options + @"  /x ""__BINDIR__\GrinGlobal_Web_Setup.msi"" TARGETVDIR=gringlobal ";
                string uninstallString = Utility.GetUninstallCommand("GRIN-Global Web Application", null);
                if (!String.IsNullOrEmpty(uninstallString)) {
                    args = "/c start /wait " + uninstallString.Replace("/passive", options) + " ";
                }

                ActionDetail newAction = new ActionDetail {
                    FriendlyName = "Uninstall Web Application",
                    PrimaryExe = "cmd.exe",
                    PrimaryArgs = args,
                    PrimaryWorkingDirectory = baseInstallDir
                };
                if (lbActions.Items.Count > 0 && lbActions.Items[lbActions.Items.Count-1].ToString().StartsWith("Uninstall")) {
                    _actions.Insert(_actions.Count - 1, newAction);
                    lbActions.Items.Insert(lbActions.Items.Count - 1, "Uninstall Web Application");
                } else {
                    _actions.Add(newAction);
                    lbActions.Items.Add("Uninstall Web Application");
                }
                ndWebApplication.ImageIndex = ndWebApplication.SelectedImageIndex = (int)ActionImage.Uninstall;
            }

            if (isIISInstalled) {
                lblIIS.Text = "IIS is already installed.";
            } else {
                if (ndWebApplication.Checked) {
                    lblIIS.Text = "IIS is not installed.\nYou may be prompted for your Windows CD during installation.";
                } else {
                    lblIIS.Text = "If you choose to install the Web Application, you may be prompted for your Windows CD during installation.";
                }
            }

            if (ndSearchEngine.Checked) {
                if (!isSearchEngineInstalled) {
                    _actions.Add(new ActionDetail {
                        FriendlyName = "Install Search Engine",
                        PrimaryExe = "cmd.exe",
                        PrimaryArgs = "/c start /wait " + Utility.GetSystem32Directory() + @"msiexec.exe " + options + @" /i ""__BINDIR__\GrinGlobal_Search_Engine_Setup.msi"" TARGETDIR=""" + baseInstallDir + @"\GRIN-Global Search Engine\"" ",
                        PrimaryWorkingDirectory = baseInstallDir + @"\GRIN-Global Search Engine\"
                    });
                    lbActions.Items.Add("Install Search Engine");
                    ndSearchEngine.ImageIndex = ndSearchEngine.SelectedImageIndex = (int)ActionImage.Install;
                }
            } else if (isSearchEngineInstalled) {

                string args = "/c start /wait " + Utility.GetSystem32Directory() + @"msiexec.exe " + options + @" /x ""__BINDIR__\GrinGlobal_Search_Engine_Setup.msi"" TARGETDIR=""" + baseInstallDir + @"\GRIN-Global Search Engine\"" ";
                string uninstallString = Utility.GetUninstallCommand("GRIN-Global Search Engine", null);
                if (!String.IsNullOrEmpty(uninstallString)) {
                    args = "/c start /wait " + uninstallString.Replace("/passive", options) + " ";
                }

                ActionDetail newAction = new ActionDetail {
                    FriendlyName = "Uninstall Search Engine",
                    PrimaryExe = "cmd.exe",
                    PrimaryArgs = args,
                    PrimaryWorkingDirectory = baseInstallDir + @"\GRIN-Global Search Engine\"
                };
                if (lbActions.Items.Count > 0 && lbActions.Items[lbActions.Items.Count-1].ToString().StartsWith("Uninstall")) {
                    _actions.Insert(_actions.Count - 1, newAction);
                    lbActions.Items.Insert(lbActions.Items.Count - 1, "Uninstall Search Engine");
                } else {
                    _actions.Add(newAction);
                    lbActions.Items.Add("Uninstall Search Engine");
                }
                ndSearchEngine.ImageIndex = ndSearchEngine.SelectedImageIndex = (int)ActionImage.Uninstall;
            }

            if (ndCuratorTool.Checked) {
                if (!isCuratorToolInstalled) {

                    if (!rdoSqlServer.Checked || !ndDatabaseEngine.Checked) {

                        if (!Utility.IsPowershellInstalled) {
                            if (Utility.IsWindowsXP) {
                                _actions.Add(new ActionDetail {
                                    FriendlyName = "Install Powershell (for SQL Server)",
                                    PrimaryExe = "cmd.exe",
                                    PrimaryArgs = @"/c start /wait update.exe " + options.Replace(" /L ", " /log:"),
                                    PrimaryWorkingDirectory = @"__TEMPBINDIR__\update\",
                                    TempExe = @"""__BINDIR__\WindowsXP-KB926139-v2-x86-ENU.exe""",
                                    TempArgs = @" /passive /x:""__TEMPBINDIR__"""
                                });
                                lbActions.Items.Add("Install XP Powershell (for SQL Server)");
                            } else if (Utility.IsWindowsVista) {
                                _actions.Add(new ActionDetail {
                                    FriendlyName = "Install Powershell (for SQL Server)",
                                    PrimaryExe = "cmd.exe",
                                    PrimaryArgs = @"/c start /wait pkgmgr.exe /ip /m:__TEMPBINDIR__\Windows6.0-KB928439-x86.cab ",
                                    PrimaryWorkingDirectory = @"__TEMPBINDIR__\",
                                    TempExe = "expand.exe",
                                    TempArgs = @" ""__BINDIR__\Windows6_0-KB928439-x86.msu"" -F:* __TEMPBINDIR__"
                                });
                                lbActions.Items.Add("Install Vista Powershell (for SQL Server)");
                            }
                        }

                        // for info on parameters: http://msdn.microsoft.com/en-us/library/ms144259.aspx
                        if (!isSqlServer2008ExpressInstalled){
                            _actions.Add(new ActionDetail {
                                FriendlyName = "Install SQL Server",
                                PrimaryExe = @"""__TEMPBINDIR__\setup.exe""",
                                PrimaryArgs = @" /CONFIGURATIONFILE=""__TEMPBINDIR__\sqlserverconfig.ini"" " + sqlServerSAPassword,
                                PrimaryWorkingDirectory = @"__TEMPBINDIR__",
                                TempExe = @"""__BINDIR__\SQLEXPRWT_x86_ENU.exe""",
                                TempArgs = @" /passive /x:""__TEMPBINDIR__""",
                                SpecialCommand = "sqlserver_install_engine",
                                ConfigurationDictionary = sqlServerConfigDictionary
                            });
                            lbActions.Items.Add("Install SQL Server (for Curator Tool) *");
                        }

                    }

                    _actions.Add(new ActionDetail {
                        FriendlyName = "Install Curator Tool",
                        PrimaryExe  = "cmd.exe",
                        PrimaryArgs = "/c start /wait " + Utility.GetSystem32Directory() + @"msiexec.exe " + options + @" /i ""__BINDIR__\GrinGlobal_Client_Installer.msi"" LIMITUI=1 ALLUSERS=1 TARGETDIR=""" + baseInstallDir + @"\GRIN-Global Curator Tool\"" ",
                        PrimaryWorkingDirectory = baseInstallDir + @"\GRIN-Global Curator Tool\"
                    });
                    lbActions.Items.Add("Install Curator Tool");

                    ndCuratorTool.ImageIndex = ndCuratorTool.SelectedImageIndex = (int)ActionImage.Install;
                    ndClient.ImageIndex = ndClient.SelectedImageIndex = ndCuratorTool.ImageIndex;
                }
            } else if (isCuratorToolInstalled) {

                string args = "/c start /wait " + Utility.GetSystem32Directory() + @"msiexec.exe " + options + @" /x ""__BINDIR__\GrinGlobal_Client_Installer.msi"" LIMITUI=1 ALLUSERS=1 TARGETDIR=""" + baseInstallDir + @"\GRIN-Global Curator Tool\"" ";
                string uninstallString = Utility.GetUninstallCommand("GRIN-Global Curator Tool", null);
                if (!String.IsNullOrEmpty(uninstallString)) {
                    args = "/c start /wait " + uninstallString.Replace("/passive", options) + " ";
//                    MessageBox.Show("uninstall for curator tool: cmd.exe " + args);
                }


                ActionDetail newAction = new ActionDetail {
                    FriendlyName = "Uninstall Curator Tool",
                    PrimaryExe = "cmd.exe",
                    PrimaryArgs = args,
                    PrimaryWorkingDirectory = baseInstallDir + @"\GRIN-Global Curator Tool\"
                };
                if (lbActions.Items.Count > 0 && lbActions.Items[lbActions.Items.Count - 1].ToString().StartsWith("Uninstall")) {
                    _actions.Insert(_actions.Count - 1, newAction);
                    lbActions.Items.Insert(lbActions.Items.Count - 1, "Uninstall Curator Tool");
                } else {
                    _actions.Add(newAction);
                    lbActions.Items.Add("Uninstall Curator Tool");
                }
                ndCuratorTool.ImageIndex = ndCuratorTool.SelectedImageIndex = (int)ActionImage.Uninstall;
                ndClient.ImageIndex = ndClient.SelectedImageIndex = ndCuratorTool.ImageIndex;
            }



            // determine server image to show...
            ActionImage serverImage = ActionImage.Blank;
            int installCount = 0;
            int uninstallCount = 0;
            foreach (TreeNode nd in ndServer.Nodes) {
                if (nd.ImageIndex == (int)ActionImage.Install) {
                    installCount++;
                } else if (nd.ImageIndex == (int)ActionImage.Uninstall) {
                    uninstallCount++;
                }
            }

            if (installCount == 0 && uninstallCount == 0) {
                // no installs or uninstalls to do.
                ndServer.ImageIndex = ndServer.SelectedImageIndex = (int)ActionImage.NoAction;
            } else {
                if (installCount == ndServer.Nodes.Count){
                    // all are installs
                    ndServer.ImageIndex = (int)ActionImage.Install;
                } else if (uninstallCount == ndServer.Nodes.Count){
                    // all are uninstalls
                    ndServer.ImageIndex = ndServer.SelectedImageIndex = (int)ActionImage.Uninstall;
                } else {
                    // some are installs, some are uninstalls or no action.
                    ndServer.ImageIndex = ndServer.SelectedImageIndex = (int)ActionImage.MixedAction;
                }
            }



            //if (getNode("ndUpdateWindows", true).Checked) {
            //    _actions.Add(new ActionDetail {FriendlyName = "Run Windows Update", SpecialCommand = @"windows_update" });
            //    lbActions.Items.Add("Run Windows Update");
            //}


            if (ndDatabaseEngine.Checked) {
                statServerDatabaseEngine.Text = "Server - " + _dbEngineUtil.FriendlyName;
            } else {
                statServerDatabaseEngine.Text = "Server - n/a";
            }

            if (ndCuratorTool.Checked) {
                statClientDatabaseEngine.Text = "Client - SQL Server 2008 Express";
            } else {
                statClientDatabaseEngine.Text = "Client - n/a";
            }

            refreshPasswordText();

            btnContinue.Enabled = lbActions.Items.Count > 0;

            TreeNode ndRoot = getNode("ndRoot", true);
            if (ndClient.ImageIndex == ndServer.ImageIndex) {
                ndRoot.ImageIndex = ndRoot.SelectedImageIndex = ndServer.ImageIndex;
            } else {
                ndRoot.ImageIndex = ndRoot.SelectedImageIndex = (int)ActionImage.MixedAction;
            }


        }

        private void refreshPasswordText() {
            lblPasswordRequired.Text = "";
            foreach (string item in lbActions.Items) {
                if (item.Contains("*")) {
                    if (isUsingWindowsAuthentication) {
                        lblPasswordRequired.Text = "* - Current windows logon will be used for " + _dbEngineUtil.FriendlyName;
                    } else {
                        lblPasswordRequired.Text = "* - Requires " + _dbEngineUtil.SuperUserName + " password for " + _dbEngineUtil.FriendlyName;
                    }
                    break;
                }
            }

        }

        private void syncTreeViewState(TreeNode node) {
            if (_adjustingNodeChecks == 0) {
                refreshActions();
                checkChildren(node);
                checkAncestors(node);
                refreshActions();
            }
            if (node.Parent == null) {
                // special case, sync tree view state on first child of root (if any) to make sure
                // image index is correct
                if (node.Nodes.Count > 0) {
                    syncTreeViewState(node.Nodes[0]);
                }
            }
        }

        private void tvOptions_AfterCheck(object sender, TreeViewEventArgs e) {

            if (rdoSqlServer.Checked){
                if (!e.Node.Checked && e.Node.ImageIndex == (int)ActionImage.NoAction){
                    if (e.Node.Name == "ndDatabaseEngine" || e.Node.Name == "ndDatabaseTool"){
                        DialogResult dr;
                        if (Utility.IsWindowsXP){
                            dr = MessageBox.Show("This can only be done through the Add/Remove Programs control panel.\nWould you like to go there now?", "Open Control Panel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (dr == DialogResult.Yes) {
                                Process.Start("control.exe", " appwiz.cpl ");
                            }
                        } else {
                            dr = MessageBox.Show("This can only be done through the Programs and Features control panel.\nWould you like to go there now?", "Open Control Panel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (dr == DialogResult.Yes) {
                                Process.Start((Utility.GetTargetDirectory(null, null, "ConfigurationWizard") + @"\gguac.exe").Replace(@"\\", @"\"), " /hide control.exe appwiz.cpl ");
                            }
                        }
                        e.Node.Checked = true;
                    }
                }
            }

            syncTreeViewState(e.Node);
        }

        private void checkChildren(TreeNode nd) {
            try {
                _adjustingNodeChecks++;
                foreach (TreeNode child in nd.Nodes) {
                    child.Checked = nd.Checked;
                    checkChildren(child);
                }
            } finally {
                _adjustingNodeChecks--;
            }
        }


        int _adjustingNodeChecks = 0;
        private void checkAncestors(TreeNode nd) {
            if (nd.Parent == null){
                return;
            }
            try {
                _adjustingNodeChecks++;
                bool allChecked = true;
                bool oneChecked = false;
                bool allUnchecked = true;
                bool oneUnchecked = false;

                int firstChildImageIndex = (nd.Parent.Nodes.Count > 0 ? nd.Parent.Nodes[0].ImageIndex : 0);
                bool differentImages = false;

                foreach (TreeNode sibling in nd.Parent.Nodes) {
                    oneChecked = oneChecked || sibling.Checked;
                    allChecked = allChecked && sibling.Checked;

                    oneUnchecked = oneUnchecked || !sibling.Checked;
                    allUnchecked = allUnchecked && !sibling.Checked;

                    differentImages = differentImages || firstChildImageIndex != sibling.ImageIndex;

                }

                if (allUnchecked && nd.Name != "ndDatabaseTool") {
                    // uncheck parent, bubble
                    nd.Parent.Checked = false;
                }

                if (oneChecked) {
                    // check parent, bubble
                    nd.Parent.Checked = true;
                }

                if (differentImages) {
                    nd.Parent.ImageIndex = nd.Parent.SelectedImageIndex = (int)ActionImage.MixedAction;
                } else {
                    nd.Parent.ImageIndex = nd.Parent.SelectedImageIndex = firstChildImageIndex;
                }
                checkAncestors(nd.Parent);


            } finally {
                _adjustingNodeChecks--;
            }



        }

        private bool isUsingWindowsAuthentication {
            get {
                return getEngineName().ToLower() == "sqlserver" && chkUseWindowsLogon.Checked;
            }
        }

        private bool isSelectedDatabaseToolInstalled {
            get {
                switch (getEngineName().ToLower()) {
                    case "mysql":
                        return isMySQLToolsInstalled;
                    case "oracle":
                        return isOracleExpressToolsInstalled;
                    case "sqlserver":
                        return isSqlServerExpressToolsInstalled;
                    case "postgresql":
                        return isPostgreSQLToolsInstalled;
                    default:
                        return false;
                }
            }
        }

        private bool isSelectedDatabaseEngineInstalled {
            get {
                switch(getEngineName().ToLower()){
                    case "mysql":
                        return isMySQL51Installed;
                    case "oracle":
                        return isOracle10gExpressInstalled;
                    case "sqlserver":
                        return isSqlServer2008ExpressInstalled;
                    case "postgresql":
                        return isPostgreSQL83Installed;
                    default:
                        return false;
                }
            }
        }

        private bool isAnyDatabaseEngineInstalled {
            get {
                return isMySQL51Installed || isSqlServer2008ExpressInstalled || isOracle10gExpressInstalled || isPostgreSQL83Installed;
            }
        }

        private bool isMySQLToolsInstalled {
            get {
                try {
                    string toolsDir = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\MySQL AB\MySQL Tools for 5.0", "Location", "");
                    if (String.IsNullOrEmpty(toolsDir)) {
                        return false;
                    } else {
                        return File.Exists((toolsDir + @"\MySQLAdministrator.exe").Replace(@"\\", @"\"));
                    }
                } catch {
                    return false;
                }
            }
        }

        private bool isMySQL51Installed {
            get {
                try {
                    string baseDir = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\MySQL AB\MySQL Server 5.1", "Location", "");
                    if (String.IsNullOrEmpty(baseDir)) {
                        return false;
                    } else {
                        return File.Exists((baseDir + @"\bin\mysqld.exe").Replace(@"\\", @"\"));
                    }
                } catch {
                    return false;
                }
            }
        }

        private bool isSqlServerExpressToolsInstalled {
            get {
                string val = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\100\Tools\Setup", "FeatureList", "") as string;
                return (String.IsNullOrEmpty(val) ? false : val.ToLower().Contains("ssms"));
            }
        }

        private bool isSqlServer2008ExpressInstalled {
            get {
                try {
                    string curVer = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\" + txtSqlServerInstanceName.Text + @"\MSSQLServer\CurrentVersion", "CurrentVersion", "");
                    return !String.IsNullOrEmpty(curVer) && curVer.StartsWith("10.");
                } catch {
                    return false;
                }
            }
        }
        private bool isOracleExpressToolsInstalled {
            get {
                return false;
            }
        }

        private bool isOracle10gExpressInstalled {
            get {
                try {
                    string curVer = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\ORACLE\KEY_XE", "VERSION", "") as string;
                    return !String.IsNullOrEmpty(curVer) && curVer.StartsWith("10.");
                } catch {
                    return false;
                }
            }
        }

        private bool isPostgreSQLToolsInstalled {
            get {
                return false;
            }
        }

        private bool isPostgreSQL83Installed {
            get {
                try {
                    string productCode = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\PostgreSQL\Services\pgsql-8.3", "Product Code", "") as string;
                    return !String.IsNullOrEmpty(productCode);
                } catch {
                    return false;
                }
            }
        }

        private bool isIISInstalled {
            get {
                try {
                    int majorVersion = (int)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\InetStp", "MajorVersion", 0);
                    // even if IIS is installed, we have to know if the w3svc is installed as well...
                    string w3svc = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W3SVC", "ImagePath", "");

                    return majorVersion >= 5 && !String.IsNullOrEmpty(w3svc);
                } catch {
                    return false;
                }

            }
        }


        private List<string> _installedApps;



        private bool isDatabaseInstalled {
            get {
                return _installedApps.Contains("GRIN-Global Database");
            }
        }

        private bool isCuratorToolInstalled {
            get {
                return _installedApps.Contains("GRIN-Global Curator Tool");
            }
        }

        private bool isWebApplicationInstalled {
            get {
                return _installedApps.Contains("GRIN-Global Web Application");
            }
        }

        private bool isSearchEngineInstalled {
            get {
                return _installedApps.Contains("GRIN-Global Search Engine");
            }
        }

        private bool isNewInstall {
            get {
                return !isCuratorToolInstalled && !isDatabaseInstalled && !isWebApplicationInstalled && !isSearchEngineInstalled;
            }
        }

        private string badPasswordReason() {
            string reason = "";

            if (isUsingWindowsAuthentication) {
                if (!isSqlServer2008ExpressInstalled) {
                    reason = "Random sa password will be generated";
                }
            } else if (txtPassword.Text.Length == 0) {
                reason = "Password not specified";
            } else if (txtPassword.Text.Contains('"')) {
                reason = "Password cannot contain a double quote (\")";
            } else if (txtPassword.Text.Length < 4) {
                reason = "Password must be at least 4 characters";
            } else if (txtPassword.Text != txtPasswordRepeat.Text) {
                reason = "Passwords do not match";
            }
            return reason;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e) {
            lblPasswordMismatch.Text = badPasswordReason();
            btnTestLogin.Enabled = lblPasswordMismatch.Text.Length == 0 && txtPassword.Text.Length > 0 && isSelectedDatabaseEngineInstalled && !isUsingWindowsAuthentication;
        }

        private void txtPasswordRepeat_TextChanged(object sender, EventArgs e) {
            lblPasswordMismatch.Text = badPasswordReason();
            btnTestLogin.Enabled = lblPasswordMismatch.Text.Length == 0 && txtPassword.Text.Length > 0 && isSelectedDatabaseEngineInstalled && !isUsingWindowsAuthentication;
        }

        private void btnChooseDirectory_Click(object sender, EventArgs e) {
            folderBrowserDialog1.Description = "Choose GRIN-Global Installation Directory";
            folderBrowserDialog1.ShowNewFolderButton = true;
            folderBrowserDialog1.SelectedPath = txtInstallDir.Text;
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.OK) {
                txtInstallDir.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private string verifyPassword() {
            // if TestLogin returns null / zero-length string, login succeeded.
            try {
                Cursor = Cursors.WaitCursor;
                return _dbEngineUtil.TestLogin(txtPassword.Text);
            } finally {
                Cursor = Cursors.Default;
            }
        }

        private void btnContinue_Click(object sender, EventArgs e) {

            refreshActions();

            // verify
            if (txtInstallDir.Text.Trim() == "") {
                MessageBox.Show("You must specify a valid path for the installation directory.");
                displayNode("ndRoot");
                txtInstallDir.Focus();
                return;
            }

            bool installingDatabase = false;
            bool uninstallingDatabase = false;
            bool uninstallingDatabaseEngine = false;
            bool databaseEngineWillExistAfterExit = isSelectedDatabaseEngineInstalled;
            bool databaseWillExistAfterExit = isDatabaseInstalled;
            foreach (object o in lbActions.Items) {
                string act = o.ToString().ToLower();

                if (isSelectedDatabaseEngineInstalled){
                    if (act.StartsWith("uninstall sql server engine") || act.StartsWith("uninstall mysql") || act.StartsWith("uninstall oracle") || act.StartsWith("uninstall postgres")){
                        databaseEngineWillExistAfterExit = false;
                        uninstallingDatabaseEngine = true;
                    }
                } else {
                    if (act.StartsWith("install sql server engine") || act.StartsWith("install mysql") || act.StartsWith("install oracle") || act.StartsWith("install postgres")) {
                        databaseEngineWillExistAfterExit = true;
                    }
                }

                if (act.StartsWith("install database")) {
                    databaseWillExistAfterExit = true;
                    installingDatabase = true;
                } else if (act.StartsWith("uninstall database")){
                    uninstallingDatabase = true;
                }

                if (act.Contains("database") || act.Contains("sql") || act.Contains("express")) {
                    // make sure root password is filled in and correct

                    if (isUsingWindowsAuthentication) {
                        // they want to use windows authentication instead of specifying an SA password.
                        // skip password checks. (note this only applies to SQL Server)
                    } else {

                        if (txtPassword.Text.Trim() == "") {
                            MessageBox.Show("You must specify a password for the " + _dbEngineUtil.SuperUserName + " database user to continue.");
                            displayNode("ndDatabase");
                            txtPassword.Focus();
                            return;
                        }
                        if (lblPasswordMismatch.Text.Length > 0) {
                            MessageBox.Show("The " + _dbEngineUtil.SuperUserName + " database password does not meet the minimum requirements.");
                            displayNode("ndDatabase");
                            txtPassword.Focus();
                            return;
                        } else if (btnTestLogin.Enabled) {
                            if (!String.IsNullOrEmpty(verifyPassword())) {
                                MessageBox.Show("You must specify the correct password for the " + _dbEngineUtil.SuperUserName + " database user to continue.");
                                displayNode("ndDatabase");
                                txtPassword.Focus();
                                return;
                            }
                        }
                    }
                }

            }






            if (!databaseEngineWillExistAfterExit){

                if (databaseWillExistAfterExit) {

                    if (uninstallingDatabaseEngine) {
                        if (isDatabaseInstalled) {
                            if (!uninstallingDatabase) {
                                if (MessageBox.Show(this, "Uninstalling " + _dbEngineUtil.FriendlyName + " will also remove the GRIN-Global database.\nAll your existing data will be lost.\nThis cannot be undone.\n\nDo you wish to continue?", "Delete data?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
                                    // mark the database as being removed as well
                                    getNode("ndDatabase", true).Checked = false;
                                    this.BeginInvoke(new _voidMethodInvoker(btnContinue.PerformClick));
                                }
                                // yes or no, bail.  if they said yes, this method is immediately called again after the db node is unchecked
                                return;
                            }
                        } else if (installingDatabase) {
                            // db doesn't currently exist, they want to add it AND remove db engine. can't do that.
                            MessageBox.Show(this, "You cannot uninstall " + _dbEngineUtil.FriendlyName + " and then install the GRIN-Global database.", "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        } else {
                            // no db, they don't want a db, and they're uninstalling the engine. ok to continue.
                        }
                    }
                }

            } else if (uninstallingDatabase) {
                if (MessageBox.Show(this, "Uninstalling the GRIN-Global database will cause all your existing data to be lost.\nThis cannot be undone.\n\nDo you wish to continue?", "Delete Data?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No) {
                    return;
                }
            } 



            // write all the pertinent paths to the registry so the other installers
            // can just blindly look for them.
//            Utility.SetTargetDirectory(txtInstallDir.Text);


            // proceed
            frmProgress fp = new frmProgress();
            fp.ActionsToDo = _actions;
            fp.DatabaseInstanceName = txtSqlServerInstanceName.Text;
            fp.DatabaseWindowsAuth = isUsingWindowsAuthentication;
            if (isUsingWindowsAuthentication) {
                // generate a random password for the SA account if we're installing sql server
                if (!isSqlServer2008ExpressInstalled) {
                    fp.DatabasePassword = Toolkit.Cut(Toolkit.ToBase64(DateTime.Now.Ticks.ToString().PadLeft(10, '0')), -10, 10);
                } else {
                    fp.DatabasePassword = ""; 
                }
            } else {
                fp.DatabasePassword = txtPassword.Text;
            }
            fp.TargetDirectory = txtInstallDir.Text;

            // pass the engine string name here so that form is force to re-get the DatabaseEngineUtil object after settings have changed
            fp.DatabaseEngine = getEngineName();
            DialogResult dr = fp.ShowDialog(this);
            if (dr == DialogResult.Retry) {
                // something is requiring a reboot.
                // remember the checked nodes to file, add a RunOnce to the registry, and reboot.
                saveGUIState();
                Utility.AddToRunOnce("GrinGlobal.ConfigurationWizard", Application.ExecutablePath);
                if (MessageBox.Show("A reboot is required to finish configuration.  Do you wish to reboot now?", "Reboot Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    // reboot -- we're running as privileged on Vista so this should work fine.
                    Process.Start("shutdown.exe", @" /r /f /t 0 /d p:4:2 /c ""GRIN-Global Configuration Wizard installed an application which requires a reboot""");
                }
                this.Close();
                return;
            }
            //if (dr == DialogResult.OK) {
            //    this.Close();
            //} else if (dr == DialogResult.Cancel) {
            //}

            _installedApps = Utility.ListInstalledApplications();

            refreshActions();
            refreshDatabaseEngineUtil();

        }


        private void listCheckedNodes(TreeNode parent, List<string> nodeNames) {
            foreach (TreeNode child in parent.Nodes) {
                if (child.Checked) {
                    nodeNames.Add(child.Name);
                }

                listCheckedNodes(child, nodeNames);
            }
        }

        private bool readGUIState() {
            string fileName = Application.ExecutablePath + ".reboot";
            if (File.Exists(fileName)) {
                string[] lines = File.ReadAllLines(fileName);
                foreach (string line in lines) {
                    if (!String.IsNullOrEmpty(line)) {
                        if (line.StartsWith(";") || line.StartsWith("#") || line.StartsWith("//") || line.Trim().Length == 0 || line.IndexOf("=") == -1) {
                            // comment / empty / invalid line. ignore.
                        } else {
                            string keyword = Toolkit.Cut(line, 0, line.IndexOf("="));
                            string value = Toolkit.Cut(line, keyword.Length + 1);

                            switch (keyword.ToLower()) {
                                case "checkednodes":
                                    string[] nodes = value.Split(',');
                                    foreach (string node in nodes) {
                                        getNode(node, true).Checked = true;
                                    }
                                    break;
                                case "engine":
                                    switch (value.ToLower()) {
                                        case "sqlserver":
                                            rdoSqlServer.Checked = true;
                                            break;
                                        case "postgresql":
                                            rdoPostgreSQL.Checked = true;
                                            break;
                                        case "mysql":
                                            rdoMySQL.Checked = true;
                                            break;
                                        case "oracle":
                                            rdoOracle.Checked = true;
                                            break;
                                    }
                                    break;
                                case "instancename":
                                    txtSqlServerInstanceName.Text = value;
                                    break;
                                case "superuserpassword":
                                    string pw = Crypto.DecryptText(value);
                                    txtPassword.Text = pw;
                                    txtPasswordRepeat.Text = pw;
                                    break;
                                case "targetdirectory":
                                    txtInstallDir.Text = value;
                                    break;
                            }

                        }
                    }
                }
                refreshDatabaseEngineUtil();
                refreshActions();
                refreshPasswordText();
                if (File.Exists(fileName)) {
                    File.Delete(fileName);
                }
                return true;
            } else {
                return false;
            }
        }

        private void saveGUIState() {


            StringBuilder sb = new StringBuilder();

            // remember which nodes are checked
            List<string> nodeNames = new List<string>();
            foreach(TreeNode child in tvOptions.Nodes){
                listCheckedNodes(child, nodeNames);
            }
            string nodes = String.Join(",", nodeNames.ToArray());
            sb.Append("CheckedNodes=").AppendLine(nodes);

            // remember engine
            sb.Append("Engine=").AppendLine(_dbEngineUtil.EngineName);

            // remember instance name
            sb.Append("InstanceName=").AppendLine(txtSqlServerInstanceName.Text);

            // remember password
            sb.Append("SuperUserPassword=").AppendLine(Crypto.EncryptText(txtPassword.Text));

            // remember target directory
            sb.Append("TargetDirectory=").AppendLine(txtInstallDir.Text);

            string fileName = Application.ExecutablePath + ".reboot";
            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }
            File.WriteAllText(fileName, sb.ToString());

        }

        private void btnTestLogin_Click(object sender, EventArgs e) {
            string response = verifyPassword();
            if (String.IsNullOrEmpty(response)) {
                MessageBox.Show("Password is valid.");
            } else {
                MessageBox.Show("Invalid password. Cause:\n\n" + response);
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                btnTestLogin.PerformClick();
                e.Handled = true;
            }
        }

        private void txtPasswordRepeat_Enter(object sender, EventArgs e) {
            txtPasswordRepeat.SelectionStart = 0;
            txtPasswordRepeat.SelectionLength = txtPasswordRepeat.Text.Length;
        }

        private void txtPassword_Enter(object sender, EventArgs e) {
            txtPassword.SelectionStart = 0;
            txtPassword.SelectionLength = txtPassword.Text.Length;
        }

        private void txtPasswordRepeat_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                btnTestLogin.PerformClick();
                e.Handled = true;
            }

        }

        private void rdoOracle_CheckedChanged(object sender, EventArgs e) {
            refreshActions();
            refreshDatabaseEngineUtil();
        }

        private void rdoMySQL_CheckedChanged(object sender, EventArgs e) {
            refreshActions();
            refreshDatabaseEngineUtil();
        }

        private void rdoSqlServer_CheckedChanged(object sender, EventArgs e) {
            refreshActions();
            refreshDatabaseEngineUtil();
        }

        private void rdoPostgreSQL_CheckedChanged(object sender, EventArgs e) {
            refreshActions();
            refreshDatabaseEngineUtil();
        }


        private string getWebConfigFilePath(string webApplicationName) {
            DirectoryEntry root = new DirectoryEntry(
            "IIS://localhost/W3SVC/1/Root");
            //"user",
            //"password");

            System.DirectoryServices.PropertyCollection properties = root.Properties;
            string path = (properties["path"] == null ? null : properties["path"].Value) as string;

            if (!String.IsNullOrEmpty(path)) {
                path = path + (@"\" + webApplicationName + @"\web.config");
            }

            return path;
        }

        private void button1_Click(object sender, EventArgs e) {

            //if (Debugger.IsAttached) {
            //    ShellUtil.Unzip(@"\projects\gringlobal\documentation\gringlobal_install\data\gringlobal_data_small.zip", @"C:\temp\gginst\gringlobal_data_small\");
            //} else {
            //    ShellUtil.Unzip(@"\gringlobal_install\data\gringlobal_data_small.zip", @"C:\temp\gginst\gringlobal_data_small\");
            //}

            //MessageBox.Show("Unzip completed.");

            //MessageBox.Show(getWebConfigFilePath("gringlobal"));
            return;

            //using (DataManager dm = DataManager.Create(DatabaseEngineUtil.CreateInstance("sqlserver").GetDataConnectionSpec("gringlobal", "test1", " =;'"))) {
            //    dm.DataConnectionSpec.CommandTimeout = 3600;
            //    DataTable dt = dm.Read("sp_databases");
            //}
        }

        private void chkUseWindowsLogon_CheckedChanged(object sender, EventArgs e) {


            txtPassword.Enabled = chkUseWindowsLogon.Visible && !chkUseWindowsLogon.Checked;
            txtPasswordRepeat.Enabled = chkUseWindowsLogon.Visible && !chkUseWindowsLogon.Checked;

            lblPasswordMismatch.Text = badPasswordReason();
            if (isUsingWindowsAuthentication){
                lblPasswordMismatch.ForeColor = Color.Black;
            } else {
                txtPassword.Focus();
                lblPasswordMismatch.ForeColor = Color.Red;
            }

            btnTestLogin.Enabled = lblPasswordMismatch.Text.Length == 0 && txtPassword.Text.Length > 0 && isSelectedDatabaseEngineInstalled && !isUsingWindowsAuthentication;

            refreshPasswordText();
        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void lblPasswordMismatch_Click(object sender, EventArgs e) {

        }

        private void label3_Click(object sender, EventArgs e) {

        }

        private void lblSuperUserPassword_Click(object sender, EventArgs e) {

        }

        private void frmAllInOne_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.F5) {
                _installedApps = Utility.ListInstalledApplications();
                refreshActions();
                refreshDatabaseEngineUtil();
            }
        }

        private bool _hasBeenActivated;
        private void frmAllInOne_Activated(object sender, EventArgs e) {
            if (!_hasBeenActivated) {
                _hasBeenActivated = true;

                Toolkit.ActivateApplication(this.Handle);

                if (readGUIState()) {
                    // we just rebooted. auto-show the progress dialog (readGUIState() auto-sets the gui to appropriate state)
                    btnContinue.PerformClick();
                } else {
                    // nothing to do. wait for user input.
                }

            }
        }

        private void mnuExit_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void mnuAbout_Click(object sender, EventArgs e) {
            MessageBox.Show("GRIN-Global Configuration Wizard\n" + Toolkit.GetApplicationVersion());
        }


    }
}
