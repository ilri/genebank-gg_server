using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using System.IO;
using GrinGlobal.Admin.ChildForms;
using GrinGlobal.Core.Xml;
using GrinGlobal.Admin.PopupForms;
using System.Diagnostics;
using GrinGlobal.Business;
using System.Net;
using System.ServiceProcess;
using GrinGlobal.InstallHelper;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Authentication;

namespace GrinGlobal.Admin {
    public partial class mdiParent : Form {




        private bool _otherAdminToolIsImporting;
        public bool TellOtherAdminToolToImport(string dvFileName) {
            try {
                _otherAdminToolIsImporting = false;
                IntPtr hwnd = WindowSearcher.SearchForWindow("WindowsForms10.Window.8.app.0.378734a", "GRIN-Global Admin", mdiParent.Current.Handle);

                MessageHelper.SendWindowsStringMessage(hwnd, this.Handle, dvFileName);
                GC.KeepAlive(this);
                return _otherAdminToolIsImporting;

            } catch {
                return false;
            }
        }

        protected override void WndProc(ref Message m) {
            if (m.Msg == MessageHelper.WM_COPYDATA) {
                try {
                    var s = MessageHelper.ReceivedWindowsStringMessage(m);
                    DataviewToImport = s;
                    //MessageBox.Show("got message to import dataview=" + DataviewToImport);
                    this.BeginInvoke((MethodInvoker)delegate() {
                        Toolkit.ActivateApplication(this.Handle);
                        autoImportDataviewIfNeeded(true);
                    });
                    MessageHelper.SendAck(m.WParam, this.Handle);
                } catch {
                }
            } else if (m.Msg == MessageHelper.WM_USER) {
                //MessageBox.Show("ack received");
                _otherAdminToolIsImporting = true;
            }
            base.WndProc(ref m);
        }

        private void autoImportDataviewIfNeeded(bool autoConnect) {

            if (CurrentConnectionInfo == null && autoConnect) {
                if (tvItems.SelectedNode == null || tvItems.SelectedNode == tvItems.Nodes[0]) {
                    // grab 'first' connection...
                    if (tvItems.Nodes[0].Nodes.Count > 0) {
                        tvItems.SelectedNode = tvItems.Nodes[0].Nodes[0];
                    } else {
                        return;
                    }
                }
            }

            if (CurrentConnectionInfo != null && !String.IsNullOrEmpty(DataviewToImport)) {
                // navigate to the dataviews node, fire an import action, giving it the file name

                SelectChildTreeViewNodeByName("ndDataviews");


                var fid = new frmImportDataviews();

                fid.ImportFiles.Add(DataviewToImport);

                if (DialogResult.OK == PopupForm(fid, this, false)) {
                    RefreshData();
                    UpdateStatus(getDisplayMember("autoImportDataviewIfNeeded{success}", "Imported {0} dataview(s)", fid.ImportedDataviews.Count.ToString()), true);
                }

            }
        }





        public static string HIVE = @"HKEY_LOCAL_MACHINE\SOFTWARE\GRIN-Global\Admin";

        private static mdiParent __instance;
        public static mdiParent Current {
            get {
                if (__instance == null) {
                    __instance = new mdiParent();
                }
                return __instance;
            }
        }

        private List<ConnectionInfo> _connections;

        //private bool _autoPopulate;
        // public bool AutoPopulate { get { return _autoPopulate; } }

//        private string _defaultServer;

        private ConnectionInfo _currentConnectionInfo;

        public ConnectionInfo CurrentConnectionInfo {
            get {
                return _currentConnectionInfo;
            }
        }

        public string DataviewToImport { get; set; }

        public string DefaultNodeToDisplay { get; set; }

        private bool refreshChildNodes() {
            if (tvItems.SelectedNode != null){
                var nd = tvItems.SelectedNode;
                nd.Nodes.Clear();
                if (nd.Name == "ndRoot") {
                    fillTreeView(true);
                    return true;
                } else {
                    if (nd.Tag is ConnectionInfo) {
                        //addDefaultNodesToServer(nd, _autoPopulate);
                        addDefaultNodesToServer(nd);
                    }
                    fireDefaultContextMenuItem(nd);
                    return true;
                }
            }
            return false;
        }

        private void refreshCurrentForm() {
            if (_previousForm != null) {
                using (new AutoCursor(this)) {
                    int id = 0;
                    if (tvItems.SelectedNode != null) {
                        id = Toolkit.ToInt32(tvItems.SelectedNode.Tag, -1);
                    }
                    _previousForm.RefreshData(id);
                }
            }
        }

        public mdiParent() {
            InitializeComponent();  // tellBaseComponents(components);
        }

        private void createNewConnection(object sender, EventArgs e) {
            PromptForNewConnection();
        }
        public bool PromptForNewConnection(){
            var ci = new ConnectionInfo {
                DatabaseEngineProviderName = "sqlserver",
                DatabaseEngineDatabaseName = "gringlobal",
                UseWindowsAuthentication = true,
                GrinGlobalUrl = "http://localhost/gringlobal/adminservice.svc",
            };
            if (promptForLoginIfNeeded(ci)) {
                SaveSettings(ci);
                addConnectionToTreeView(ci);
                if (tvItems.SelectedNode != null && tvItems.SelectedNode.Parent == null) {
                    refreshCurrentForm();
                }
                return true;
            }
            return false;
        }

        private void OpenFile(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK) {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK) {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e) {
            Debug.WriteLine("copy");
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e) {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e) {
            statusStrip.Visible = mnuViewStatusBar.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (Form childForm in MdiChildren) {
                childForm.Close();
            }
        }

        private string getConnectionFilePath() {
            return Toolkit.ResolveFilePath(@"*Application Data*\GRIN-Global\Admin\dbconn.txt", true);
        }

        private DataTable _dtLang;

        private DataTable _dtResource;
        public DataTable GetResourceTable(bool forceRefresh) {
            if (_dtResource == null || forceRefresh) {
                var proxy = GetAdminHostProxy();
                if (proxy != null) {
                    _dtResource = proxy.ListResources("AdminTool", proxy.LanguageID).Tables["list_resources"];
                }
            }
            return _dtResource;
        }

        public DataTable GetLanguageTable(bool forceRefresh) {
            if (_dtLang == null || forceRefresh) {
                var proxy = GetAdminHostProxy();
                if (proxy != null) {
                    _dtLang = proxy.ListLanguages(-1).Tables["list_languages"];
                }
            }
            return _dtLang;
        }

        public bool IsOracleConnection() {
            return _currentConnectionInfo != null && _currentConnectionInfo.DatabaseEngineProviderName.ToLower() == "oracle";
        }

        public List<ConnectionInfo> ListAllConnections(bool initFromFile) {
            var rv = new List<ConnectionInfo>();
            if (initFromFile) {
                // read from xml file
                string xml = null;
                string xmlFilePath = getConnectionFilePath();
                if (File.Exists(xmlFilePath)) {
                    xml = File.ReadAllText(xmlFilePath);

                    // HACK: support converting old format to new format
                    if (xml.StartsWith("<Connections>")) {
                        xml = "<Settings>" + xml + "</Settings>";
                    }
                } else {
                    //                    xml =
                    //@"<Settings AutoPopulate='true' DefaultServer='localhost\sqlexpress'>
                    //    <Connections>
                    //        <Server DatabaseEngineName='localhost\sqlexpress' DatabaseEngineProvider='sqlserver' DatabaseEngineDatabaseName='gringlobal' UseWindowsAuthentication='true' DatabaseEngineRememberPassword='true' DatabaseEngineUserName='' DatabaseEnginePassword='' GrinGlobalRememberPassword='true' GrinGlobalUserName='' GrinGlobalPassword='' GrinGlobalUrl='' />
                    //    </Connections>
                    //</Settings>";
                }


                if (!String.IsNullOrEmpty(xml)) {
                    Document doc = new Document();
                    doc.LoadXml(xml);
                    foreach (Node nd in doc.Root.Nodes["Connections"].Nodes) {
                        var ci = ConnectionInfo.FromXmlNode(nd);
                        if (ci != null) {
                            rv.Add(ci);
                        }
                    }
                }
            } else {
                // just use whatever is already in our list
                foreach (var ci in _connections) {
                    rv.Add(ci);
                }
            }
            return rv;
        }

        private void fillTreeView(bool initFromFile) {

            // database file has the following characteristics:
            // 1. encrypted to prevent casual browsing of file
            // 2. xml like:
            //    <connections>
            //      <Server Name="" Provider="" UseWindowsAuthentication="" RememberPassword="" UserName="" Password="" Url="" />
            //    </connections>


            _connections = new List<ConnectionInfo>();

            if (initFromFile) {
                _connections = ListAllConnections(initFromFile);
                //_autoPopulate = ("" + Toolkit.GetRegSetting(HIVE, "GeneralAutoPopulate", "false")).ToLower() == "true";
            }


            var regEngine = Utility.GetDatabaseEngine(null, null, false, "sqlserver");
            var regServer = Utility.GetDatabaseServerName(null, null, "localhost");
            var regConn = regEngine + "|" + regServer;

            var regInstance = Utility.GetDatabaseInstanceName(null, null, "");
            var regPort = Utility.GetDatabasePort(null, null, 0);

            if (String.Compare(regEngine, "sqlserver", true) == 0){
                if (!String.IsNullOrEmpty(regInstance)) {
                    regConn += @"\" + regInstance;                
                }
                if (regPort > 0) {
                    regConn += @":" + regPort;
                }
            } else {
                if (regPort > 0) {
                    regConn += @":" + regPort;
                }
            }

            var lastUsedDate = DateTime.MinValue;
            TreeNode lastUsed = null;
            foreach (var c in _connections) {
                //var comparer = c.DatabaseEngineProviderName + "|" + c.ServerName;  // servername is hostname\instance or hostname:port
                //if (String.Compare(comparer, regConn, true) == 0){
                //    defaultNode = addConnectionToTreeView(c);
                //    lastUsed = defaultNode;
                //} else {
                    if (c.LastUsed > lastUsedDate) {
                        lastUsed = addConnectionToTreeView(c);
                        lastUsedDate = c.LastUsed;
                    } else {
                        addConnectionToTreeView(c);
                    }
                //}
            }

            if (lastUsed != null) {
                //if (defaultNode == null && tvItems.Nodes.Count > 0) {
                //    defaultNode = tvItems.Nodes[0];
                //}

                tvItems.SelectedNode = lastUsed;

                if (tvItems.SelectedNode != null) {
                    tvItems.SelectedNode.Expand();
                }

               // GuiManager.LoadResources(GetResourceTable(false), "AdminTool", this, null);


            } else {


                //            if (tvItems.Nodes.Count == 0) {
                // first time in, no connections available.  auto-popup the login form.
                createNewConnection(this, null);
                //            }
            }

            // HACK: force a refresh so child form border is updated properly
            Height += 1;
            Height -= 1;

        }

        private bool _syncing;
        private void syncGUI() {
            if (!_syncing) {
                try {
                    _syncing = true;

                } finally {
                    _syncing = false;
                }
            }
        }

        private void mdiParent_Load(object sender, EventArgs e) {

            lnkLevel1.Visible = false;
            lnkLevel2.Visible = false;
            lnkLevel3.Visible = false;
            lnkLevel4.Visible = false;

            _defaultColor = statusToolStrip.BackColor;

            _statusTimer.Tick += new EventHandler(_statusTimer_Tick);

//            AdminHostProxy.Scan();

            if (Debugger.IsAttached) {
                // only show this funcitonality while debugging, no need to otherwise
                mnuToolsGenerateAppResourceData.Visible = true;
                mnuToolsTest.Visible = true;
            }
            // for now...
            toolStrip.Visible = false;

//            this.Text = Toolkit.GetInstalledVersion("GRIN-Global Admin");
            this.Text = Toolkit.GetApplicationVersion("GRIN-Global Admin");

            lnkLevel1.Click += new EventHandler(lnkBreadcrumb_Click);
            lnkLevel2.Click += new EventHandler(lnkBreadcrumb_Click);
            lnkLevel3.Click += new EventHandler(lnkBreadcrumb_Click);

        }

        void lnkBreadcrumb_Click(object sender, EventArgs e) {
            var lnk = ((LinkLabel)sender);
            if (lnk.Tag != null) {
                var nd = lnk.Tag as TreeNode;
                if (nd != null) {
                    tvItems.SelectedNode = nd;
                }
            }
        }

        private class NodeInfo {
            public string ProgrammaticName;
            public string DisplayName;
            public bool RequiresElevation;
            public List<NodeInfo> Children;
            public NodeInfo() {
                Children = new List<NodeInfo>();
            }
        }

        private NodeInfo[] DEFAULT_NODES = new NodeInfo[] {
            new NodeInfo { ProgrammaticName = "ndGroups", DisplayName = "Groups", RequiresElevation = false },
            new NodeInfo { ProgrammaticName = "ndUsers", DisplayName = "Users", RequiresElevation = false },
            new NodeInfo { ProgrammaticName = "ndPermissions", DisplayName = "Permissions", RequiresElevation = false },
            new NodeInfo { ProgrammaticName = "ndDataviews", DisplayName = "Dataviews", RequiresElevation = false },
            new NodeInfo { ProgrammaticName = "ndTableMappings", DisplayName = "Table Mappings", RequiresElevation = false },
//            new NodeInfo { ProgrammaticName = "ndFilters", DisplayName = "Filters", RequiresElevation = true },
            new NodeInfo { ProgrammaticName = "ndDataTriggers", DisplayName = "Data Triggers", RequiresElevation = true },
            new NodeInfo { ProgrammaticName = "ndMaintenance", DisplayName = "Maintenance", RequiresElevation = false, 
                    Children = new List<NodeInfo> { 
                        new NodeInfo { ProgrammaticName = "ndImportWizard", DisplayName="Import Wizard", RequiresElevation = false },
                        new NodeInfo { ProgrammaticName = "ndCodeGroups", DisplayName = "Code Groups", RequiresElevation = false }
                    }
            },
            new NodeInfo { ProgrammaticName = "ndFileGroups", DisplayName = "File Groups", RequiresElevation = true },
            //new NodeInfo { ProgrammaticName = "ndSearchEngine", DisplayName = "Search Engine", RequiresElevation = true },
            new NodeInfo { ProgrammaticName = "ndWebApplication", DisplayName = "Web Application", RequiresElevation = true },
        };

//        private string[] DEFAULT_NODES2 = new string[] {
//            "Groups",
//            "Users",
//            "Permissions",
///*            "Permission Templates", */
//            "Dataviews",
//            "Table Mappings",
//            "Filters",
//            "File Groups",
//            "Search Engine",
//            "Web Application"
//        };

        private TreeNode addRootIfNeeded(TreeView tv) {

            TreeNode root = null;
            if (tv.Nodes.Count == 0) {
                root = new TreeNode("Connections");
                root.Name = "ndRoot";
                root.ImageIndex = root.SelectedImageIndex = 26; // connections.ico
                root.Tag = null;
                tvItems.Nodes.Add(root);
            } else {
                root = tvItems.Nodes[0];
            }
            return root;
        }

        private TreeNode addConnectionToTreeView(ConnectionInfo ci) {

            var nd = new TreeNode(ci.ServerName + " - " + ci.DatabaseEngineProviderName);
            nd.Name = "ndServer_" + ci.ServerName;
            nd.ImageIndex = 2;
            nd.Tag = ci;

            addDefaultNodesToServer(nd);

            var root = addRootIfNeeded(tvItems);
            root.Nodes.Add(nd);

            return nd;

        }

        //private List<TreeNode> addDefaultNodesToServer(TreeNode nd, bool autoPopulate){
        private void addDefaultNodesToServer(TreeNode nd){

            //var autoPopulateList = new List<TreeNode>();

            foreach (NodeInfo ni in DEFAULT_NODES) {
                addChildNode(nd, ni);
            }

            //if (!autoPopulate) {
            //    autoPopulateList.Clear();
            //}

            //return autoPopulateList;

        }

        private void addChildNode(TreeNode nd, NodeInfo ni) {
            var child = new TreeNode(ni.DisplayName);
            child.Name = ni.ProgrammaticName;
            child.Tag = ni;

            child.Text = ni.DisplayName;  // TODO: look up language-specific text from app_resource table

            int index = 0;
            switch (child.Name.ToLower()) {
                case "ndgroups":
                    index = 23;
                    child.ToolTipText = "Edit groups and their associated users or permissions";
                    //autoPopulateList.Add(child);
                    break;
                case "ndusers":
                    index = 3;
                    child.ToolTipText = "Edit users and their associated permissions";
                    //autoPopulateList.Add(child);
                    break;
                case "ndpermissions":
                    index = 5;
                    child.ToolTipText = "Edit permissions to restrict access to data";
                    //autoPopulateList.Add(child);
                    break;
                case "ndpermissiontemplates":
                    index = 7;
                    child.ToolTipText = "Edit sets of commonly-used permissions for ease of administration";
                    //autoPopulateList.Add(child);
                    break;
                case "ndsearchengine":
                    index = 9;
                    child.ToolTipText = "Edit all configurable aspects of the GRIN-Global Search Engine (requires local connection)";
                    //    autoPopulateList.Add(child);
                    break;
                case "ndtablemappings":
                    index = 12;
                    //autoPopulateList.Add(child);
                    child.ToolTipText = "Table mappings are used by Dataviews to associate a Dataview field to a Table field for the purposes of showing friendly column names, allowing updates, etc.";
                    break;
                case "nddatatriggers":
                    index = 14;
                    child.ToolTipText = "Data Triggers are used by the middle tier to run custom .NET code when reading or writing data";
                    //autoPopulateList.Add(child);
                    break;
                case "ndfilters":
                    index = 14;
                    child.ToolTipText = "Filters are used by the middle tier to run custom .NET code when reading or writing data";
                    //autoPopulateList.Add(child);
                    break;
                case "ndmaintenance":
                    index = 27;
                    child.ToolTipText = "Maintain your GRIN-Global system and data";
                    break;
                case "ndfilegroups":
                    index = 16;
                    child.ToolTipText = "Define files available for downloading via Updater";
                    //autoPopulateList.Add(child);
                    break;
                case "ndwebapplication":
                    index = 18;
                    child.ToolTipText = "Edit configurable aspects of the Web Application (requires local connection)";
                    //autoPopulateList.Add(child);
                    break;
                case "nddataviews":
                    index = 20;
                    child.ToolTipText = "Edit how data is presented in GRIN-Global";
                    //autoPopulateList.Add(child);
                    break;
                case "ndimportwizard":
                    index = 28;
                    child.ToolTipText = "Import large amounts of data into GRIN-Global";
                    break;
                case "ndcodegroups":
                    index = 29;
                    child.ToolTipText = "Edit dropdown values";
                    break;
                default:
                    index = 1;
                    break;
            }
            child.ImageIndex = child.SelectedImageIndex = index;
            if (ni.Children != null) {
                foreach (NodeInfo cni in ni.Children) {
                    addChildNode(child, cni);
                }
            }
            nd.Nodes.Add(child);
        }

        private void tvItems_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                var ctxMenu = getContextMenu(e.Node);
                if (ctxMenu != null) {
                    tvItems.SelectedNode = e.Node;
                    ctxMenu.Show(tvItems, e.Location);
                }
            }
        }

        private ContextMenuStrip getContextMenu(TreeNode nd){
            switch (nd.Name) {
                case "ndRoot":
                    return cmRoot;
                case "ndGroups":
                    return cmGroups;
                case "ndUsers":
                    return cmUsers;
                case "ndPermissions":
                    return cmPermissions;
                case "ndPermissionTemplates":
                    return cmTemplates;
                case "ndDataviews":
                    return cmDataviews;
                case "ndFilters":
                    return cmFilters;
                case "ndDataTriggers":
                    return cmDataTriggers;
                case "ndFileGroups":
                    return cmFileGroups;
                case "ndMaintenance":
                    return cmMaintenance;
                case "ndTableMappings":
                    return cmTableMappings;
                case "ndSearchEngine":
                    return cmSearchEngine;
                case "ndWebApplication":
                    return cmWebApp;
                case "ndImportWizard":
                    return cmImportWizard;
                case "ndCodeGroups":
                    return cmCodeGroups;
                case "ndCodeGroup":
                    return cmCodeGroup;
                default:
                    if (nd.Name.StartsWith("ndServer")) {
                        return cmTreeView;
                    } else {

                        // we get here, we need to map the context menu dynamically.
                        if (nd.Parent == null) {
                            return null;
                        } else {
                            return getChildContextMenu(nd.Parent);
                        }
                    }
            }
        }

        private ContextMenuStrip getChildContextMenu(TreeNode parent) {
            switch (parent.Name) {
                case "ndGroups":
                    return cmGroup;
                case "ndUsers":
                    return cmUser;
                case "ndPermissions":
                    return cmPermission;
                case "ndPermissionTemplates":
                    return cmTemplate;
                case "ndDataviews":
                    return cmDataviewCategory;
                case "ndDataTriggers":
                    return cmDataTrigger;
                case "ndFilters":
                    return cmFilter;
                case "ndFileGroups":
                    return cmFileGroup;
                case "ndTableMappings":
                    return cmTableMapping;
                case "ndSearchEngine":
                    return cmSearchEngineIndex;
                case "ndWebApplication":
                    return cmWebAppSetting;
                case "ndMaintenance":
                    return cmMaintenance;
                case "ndImportWizard":
                    return cmImportWizard;
                case "ndCodeGroups":
                    return cmCodeGroup;
                case "":
                default:

                    // special case -- so far only 1 entry has more than two levels, and that's search engine / index / resolver.
                    if (parent.Parent != null && parent.Parent.Name == "ndSearchEngine") {
                        return cmSearchEngineResolver;
                    }

                    // special case -- dataviews are now categorized, so we may need to skip over the Category node
                    if (parent.Parent != null && parent.Parent.Name == "ndDataviews") {
                        return cmDataview;
                    }

                    throw new NotImplementedException(getDisplayMember("getChildContextMenu", "mdiParent.getChildContextMenu() does not contain a context menu mapping for {0}", parent.Name));
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
//            MessageBox.Show(Toolkit.GetInstalledVersion("GRIN-Global Admin"));
            var br = "---------------------------------------------------------------\r\n";
            var msg = new frmMessageBox();
            msg.btnYes.Visible = false;
            msg.Width = 480;
            msg.Height = 336;
            msg.linkLabel1.Visible = true;
            msg.btnNo.Text = "OK";
            msg.Text = "About GRIN-Global Admin Tool";
            msg.txtMessage.Font = new Font("Courier-New", 10.0f, FontStyle.Regular);
            var ci = _currentConnectionInfo;
            var info = "Connection Information\r\n" + br;
            if (ci != null) {
                info += "User: " + ci.GrinGlobalUserName;
                info += "\r\nServer: " + ci.DatabaseEngineServerName;
            } else {
                info = "\r\nn/a";
            }
            msg.txtMessage.Text = Toolkit.GetApplicationVersion("GRIN-Global Admin") + "\r\n" + br + "\r\n" + info;
            msg.ShowDialog(this);
            //MessageBox.Show(Toolkit.GetApplicationVersion("GRIN-Global Admin"));
        }


        //public void UpdateStatus(string newStatus) {
        //    UpdateStatus(newStatus, false);
        //}

        private Timer _statusTimer = new Timer();
        private Color _defaultColor;
        private TreeNode _droppedOnNode;
        private int _droppedOnNodeImageIndex;
        private const int SAVE_IMAGE_ICON_INDEX = 24;

        public void UpdateStatus(string newStatus, bool flash) {
            UpdateStatus(newStatus, flash, false, null);
        }

        private Timer _indexNotificationTimer = new Timer();

        public void NotifyWhenIndexCompleted() {
            var notify = ("" + Toolkit.GetRegSetting(HIVE, "SearchEngineNotifyOnIndexProcessingCompleted", "true")).ToLower() == "true";

            if (_indexNotificationTimer != null) {
                _indexNotificationTimer.Stop();
                _indexNotificationTimer = null;
            }

            if (notify) {
                _indexNotificationTimer = new Timer();
                _indexNotificationTimer.Interval = 5000;
                _indexNotificationTimer.Tick += new EventHandler(_indexNotificationTimer_Tick);
                _indexNotificationTimer.Start();
            }
        }

        void _indexNotificationTimer_Tick(object sender, EventArgs e) {

            try {
                var proxy = GetAdminHostProxy();
                if (proxy != null) {
                    var ds = proxy.GetSearchEngineStatus();
                    var dt = ds.Tables["status"];
                    var stillProcessing = 0;
                    if (dt.Rows.Count > 0) {
                        stillProcessing = Toolkit.ToInt32(dt.Rows[0]["processing_indexes"], 0);
                    }
                    if (stillProcessing == 0) {
                        _indexNotificationTimer.Stop();
                        MessageBox.Show(this, getDisplayMember("seindex{done_body}", "All search engine indexes have completed processing."),
                            getDisplayMember("seindex{done_title}", "Index Processing Completed"));
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(this, getDisplayMember("seindex{failed_body}", "There was an error communicating with the search engine:\r\n\r\n{0}", ex.Message), 
                    getDisplayMember("seindex{failed_title}", "Search Engine Communication Failure"));
                _indexNotificationTimer.Stop();
            }
        }

        public void UpdateStatus(string newStatus, bool flash, TreeNode droppedOnNode) {
            UpdateStatus(newStatus, flash, false, droppedOnNode);
        }

        public void ReportError(string errorText) {
            UpdateStatus(errorText, true, true, null);
        }

        public void UpdateStatus(string newStatus, bool flash, bool isError, TreeNode droppedOnNode) {
            if (!_statusTimer.Enabled) {
                statusToolStrip.Text = getDisplayMember("updateStatus{body}", "{0} at {1}", newStatus, DateTime.Now.ToString());
                if (flash) {
                    if (droppedOnNode != null) {
                        _droppedOnNodeImageIndex = droppedOnNode.ImageIndex;
                        _droppedOnNode = droppedOnNode;
                        droppedOnNode.ImageIndex = SAVE_IMAGE_ICON_INDEX;
                        tvItems.Refresh();
                    }
                    if (isError) {
                        statusToolStrip.BackColor = Color.Red;
                    } else {
                        statusToolStrip.BackColor = Color.Orange;
                    }
                    Application.DoEvents();
                    _statusTimer.Interval = 2000;
                    _statusTimer.Start();
                } else {
                    Application.DoEvents();
                }
            }
        }

        void _statusTimer_Tick(object sender, EventArgs e) {
            statusToolStrip.BackColor = _defaultColor;
            if (_droppedOnNode != null) {
                _droppedOnNode.ImageIndex = _droppedOnNodeImageIndex;
                _droppedOnNode = null;
                tvItems.Refresh();
            }
            Application.DoEvents();
            _statusTimer.Stop();
        }

        private TreeNode _previousTreeViewNode;
        private frmBase _previousForm;

        public DialogResult PopupForm(frmBase form, Form owner, bool resetID) {
            if (initForm(form, owner, null, resetID)) {
                return form.ShowDialog();
            } else {
                return DialogResult.Cancel;
            }
        }

        private bool initForm(frmBase form, Form owner, TreeNode correspondingNode, bool resetID) {
            if (_currentConnectionInfo != null) {
                try {
                    form.AdminProxy = new AdminHostProxy(_currentConnectionInfo, !(form is frmLogin));
                } catch (InvalidCredentialException ice) {
                    Debug.WriteLine(ice.Message);
                    this.BeginInvoke((MethodInvoker)(delegate() {
                        if (!promptForLoginIfNeeded(_currentConnectionInfo)) {
                            SelectConnectionNode();
                        } else {
                            RefreshData();
                        }
                    }));
                    return false;
                }
            } else {
                try {
                    var cnn = getConnectionForNode(correspondingNode);
                    if (cnn != null) {
                        form.AdminProxy = new AdminHostProxy(cnn);
                    }
                } catch (Exception ex){
                    Debug.WriteLine("Now what???");
                }
            }

            // tell the form to update its properties before showing it
            int id = (resetID ? -1 : form.ID);
            if (correspondingNode != null) {
                id = Toolkit.ToInt32(correspondingNode.Tag, -1);
            }
            if (!(owner is mdiParent)) {
                form.Owner = owner;
            }
            if (form.AdminProxy == null){
                form.RefreshData(id);
            } else if (form.AdminProxy.IsValid) {
                form.RefreshData(id);
            } else {
                // connection, if any, may have been given but it's login credentials aren't right.
            }

            return true;
        }

        private void showChildForm(frmBase form, TreeNode correspondingNode) {
            using (new AutoCursor(this)) {
                using (new FreezeWindow(this.Handle)) {

                    form.WindowState = FormWindowState.Maximized;
                    
                    // this MUST be set to sizable even though it's always maximized -- this prevents some resizing issues
                    form.FormBorderStyle = FormBorderStyle.Sizable;

                    form.ControlBox = false;
                    form.MaximizeBox = false;
                    form.MinimizeBox = false;
                    form.MdiParent = this;
                    this.Refresh();
                    Application.DoEvents();
                    if (!initForm(form, this, correspondingNode, true)) {
                        return;
                    }

                    if (_previousForm != null) {

                        //if (_previousForm.GetType() == form.GetType()) {

                        //    try {
                        //        _previousForm.ID = form.ID;
                        //        _previousForm.RefreshData();
                        //    } catch (ObjectDisposedException ode) {
                        //        // this exception is thrown when a form closes during a RefreshData in case of an invalid
                        //        // operation, such as trying to retrieve data from the local web app when it is not installed.
                        //        // just ignore and continue.
                        //    }


                        //} else {
                            try {
                                    form.SyncTab(_previousForm);
                                    form.Show();
                                    form.Dock = DockStyle.Fill;
                                    this.Refresh();
                                    Application.DoEvents();
                                    childFormRefreshed(form);
                                    _previousForm.Close();
                            }
                            catch (ObjectDisposedException ode)
                            {
                                // this exception is thrown when a form closes during a RefreshData in case of an invalid
                                // operation, such as trying to retrieve data from the local web app when it is not installed.
                                // just ignore and continue.
                                Debug.WriteLine(ode.Message);
                            }
                        //}
                    } else {
                            form.Show();
                            form.Dock = DockStyle.Fill;
                            this.Refresh();
                            Application.DoEvents();
                            childFormRefreshed(form);
                    }


                    _previousForm = form;


                    // sync the breadcrumbs

                    var ancestors = new List<TreeNode>();
                    var parent = correspondingNode;
                    while (parent != null) {
                        ancestors.Add(parent);
                        parent = parent.Parent;
                    }

                    if (ancestors.Count == 0) {
                        lnkLevel1.Visible = false;
                        lnkLevel2.Visible = false;
                        lnkLevel3.Visible = false;
                        lnkLevel4.Visible = false;
                    } else {
                        // last one is always the root, so let's just ignore it
                        ancestors.RemoveAt(ancestors.Count - 1);

                        switch (ancestors.Count) {
                            case 0:
                                lnkLevel1.Visible = false;
                                lnkLevel2.Visible = false;
                                lnkLevel3.Visible = false;
                                lnkLevel4.Visible = false;
                                break;
                            case 1:
                                lnkLevel1.Visible = true;
                                lnkLevel1.Text = ancestors[ancestors.Count - 1].Text + " > ";
                                lnkLevel1.Tag = ancestors[ancestors.Count - 1];

                                lnkLevel2.Visible = false;

                                lnkLevel3.Visible = false;

                                lnkLevel4.Visible = false;
                                break;
                            case 2:
                                lnkLevel1.Visible = true;
                                lnkLevel1.Text = ancestors[ancestors.Count - 1].Text + " > ";
                                lnkLevel1.Tag = ancestors[ancestors.Count - 1];

                                lnkLevel2.Visible = true;
                                lnkLevel2.Text = ancestors[ancestors.Count - 2].Text;
                                lnkLevel2.Tag = ancestors[ancestors.Count - 2];

                                lnkLevel3.Visible = false;

                                lnkLevel4.Visible = false;
                                break;
                            case 3:
                                lnkLevel1.Visible = true;
                                lnkLevel1.Text = ancestors[ancestors.Count - 1].Text + " > ";
                                lnkLevel1.Tag = ancestors[ancestors.Count - 1];

                                lnkLevel2.Visible = true;
                                lnkLevel2.Text = ancestors[ancestors.Count - 2].Text + " > ";
                                lnkLevel2.Tag = ancestors[ancestors.Count - 2];

                                lnkLevel3.Visible = true;
                                lnkLevel3.Text = ancestors[ancestors.Count - 3].Text;
                                lnkLevel3.Tag = ancestors[ancestors.Count - 3];

                                lnkLevel4.Visible = false;
                                break;
                            default:
                                lnkLevel1.Visible = true;
                                lnkLevel1.Text = ancestors[ancestors.Count - 1].Text + " > ";
                                lnkLevel1.Tag = ancestors[ancestors.Count - 1];

                                lnkLevel2.Visible = true;
                                lnkLevel2.Text = ancestors[ancestors.Count - 2].Text + " > ";
                                lnkLevel2.Tag = ancestors[ancestors.Count - 2];

                                lnkLevel3.Visible = true;
                                lnkLevel3.Text = ancestors[ancestors.Count - 3].Text + " > ";
                                lnkLevel3.Tag = ancestors[ancestors.Count - 3];

                                lnkLevel4.Visible = true;
                                lnkLevel4.Text = ancestors[ancestors.Count - 4].Text;
                                lnkLevel4.Tag = ancestors[ancestors.Count - 4];
                                break;
                        }

                    }

                }
            }
        }

        public void SelectParentTreeViewNode() {
            if (tvItems.SelectedNode != null && tvItems.SelectedNode.Parent != null) {
                Application.DoEvents();
                tvItems.SelectedNode = tvItems.SelectedNode.Parent;
            }
        }

        public void SelectPreviousTreeViewNode() {
            BeginInvoke((MethodInvoker)delegate () {
                if (_previousTreeViewNode != null) {
                    tvItems.SelectedNode = _previousTreeViewNode;
                }
            });
        }

        public void SelectChildTreeViewNode(string tagValue) {
            if (tvItems.SelectedNode != null) {
                foreach (TreeNode nd in tvItems.SelectedNode.Nodes) {
                    if (nd.Tag != null){
                        if (nd.Tag.ToString() == tagValue) {
                            tvItems.SelectedNode = nd;
                            break;
                        } else {
                            var ni = nd.Tag as NodeInfo;
                            if (ni != null) {
                                if (ni.ProgrammaticName == tagValue) {
                                    tvItems.SelectedNode = nd;
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool IsCurrentNodeDescendentOf(string tagValue) {
            if (tvItems.SelectedNode != null) {
                var nd = tvItems.SelectedNode;
                while (nd.Parent != null) {
                    if (("" + nd.Tag).ToString() == tagValue) {
                        return true;
                    }
                    nd = nd.Parent;
                }
            }
            return false;
        }

        public void SelectDescendentTreeViewNode(string tagValue) {
            if (tvItems.SelectedNode != null) {
                SelectDescendentTreeViewNode(tagValue, tvItems.SelectedNode);
            }
        }

        public void SelectDescendentTreeViewNode(string tagValue, TreeNode parent){
            if (parent.Tag != null && parent.Tag.ToString() == tagValue) {
                var nd2 = parent;
                tvItems.SelectedNode = nd2;
                while (nd2.Parent != null) {
                    nd2.Expand();
                    nd2 = nd2.Parent;
                }
            } else {
                foreach (TreeNode nd in parent.Nodes) {
                    SelectDescendentTreeViewNode(tagValue, nd);
                }
            }
        }

        public void SelectChildTreeViewNodeByName(string nameValue) {
            if (tvItems.SelectedNode != null) {
                foreach (TreeNode nd in tvItems.SelectedNode.Nodes) {
                    if (nd.Name != null && String.Compare(nd.Name, nameValue, true) == 0) {
                        tvItems.SelectedNode = nd;
                        break;
                    }
                }
            }
        }

        public void SelectConnection(ConnectionInfo connection) {
            if (connection != null && tvItems.Nodes.Count > 0){
                foreach(TreeNode nd in tvItems.Nodes[0].Nodes){
                    if (nd.Tag as ConnectionInfo == connection) {
                        tvItems.SelectedNode = nd;
                        break;
                    }
                }
            }
        }

        public void ClearLoginToken() {
            var nd = tvItems.SelectedNode;
            if (nd != null) {
                var connNode = getRootConnectionNode(nd);
                if (connNode != null) {
                    var ci = connNode.Tag as ConnectionInfo;
                    if (ci != null) {
                        ci.GrinGlobalLoginToken = null;
                    }
                }
            }
        }

        public void SelectConnectionNode() {
            var nd = tvItems.SelectedNode;
            if (nd != null) {
                var connNode = getRootConnectionNode(nd);
                if (connNode != null) {
                    tvItems.SelectedNode = connNode;
                }
            }
        }

        private void childFormRefreshed(frmBase f){
            if (tvItems.SelectedNode != null) {
                if (tvItems.SelectedNode.Parent != null) {
                    if (!(tvItems.SelectedNode.Tag is ConnectionInfo)) {
                        if (f.DataTable != null) {
                            tvItems.SelectedNode.Nodes.Clear();
                            // populate the treeview nodes under current one with all data in f.Data
                            if (!String.IsNullOrEmpty(f.CategoryColumn)) {
                                var dt = f.DataTable.Copy();
                                var rows = dt.Select("1=1", "category_code");
                                string prevCategory = null;
                                TreeNode ndCat = null;
                                var parentImageIndex = tvItems.SelectedNode.ImageIndex;
                                foreach (DataRow dr in rows){
                                    var category = dr[f.CategoryColumn].ToString();
                                    if (prevCategory != category) {
                                        if (ndCat != null) {
                                            tvItems.SelectedNode.Nodes.Add(ndCat);
                                        }
                                        ndCat = new TreeNode(dr[f.CategoryColumn].ToString());
                                        if (ndCat.Text.Trim() == "") {
                                            ndCat.Text = "(None)";
                                        }
                                        ndCat.Tag = dr[f.CategoryColumn].ToString();
                                        ndCat.ImageIndex = ndCat.SelectedImageIndex = parentImageIndex + 1;
                                        prevCategory = category;
                                    }
                                    var nd = new TreeNode(dr[f.TextColumn].ToString());
                                    nd.Tag = dr[f.IDColumn].ToString();
                                    nd.ImageIndex = parentImageIndex + 2;
                                    nd.SelectedImageIndex = 0;

                                    ndCat.Nodes.Add(nd);
                                }
                                if (ndCat != null) {
                                    tvItems.SelectedNode.Nodes.Add(ndCat);
                                }

                            } else {
                                foreach (DataRow dr in f.DataTable.Rows) {
                                    var nd = new TreeNode(dr[f.TextColumn].ToString());
                                    nd.Tag = dr[f.IDColumn].ToString();
                                    tvItems.SelectedNode.Nodes.Add(nd);
                                    nd.ImageIndex = nd.Parent.ImageIndex + 1;
                                    nd.SelectedImageIndex = 0;
                                }
                            }
                            UpdateStatusCount(f.DataTable.Rows.Count);
                        }
                    }
                }
            }
        }

        public void UpdateStatusCount(int count) {
            statusCount.Text = count + getDisplayMember("statusCount{items}", " items");
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e) {
            PopupNewItemForm(new frmUser());
        }

        private void newPermissionToolStripMenuItem_Click(object sender, EventArgs e) {
            PopupNewItemForm(new frmPermission());
        }

        public void SaveSettings(ConnectionInfo ci) {
            if (ci != null) {
                bool found = false;
                for(int i=0;i<_connections.Count;i++){
                    var c = _connections[i];
                    if (c.ServerName == ci.ServerName && c.GrinGlobalUserName == ci.GrinGlobalUserName) {
                        ci.LastUsed = DateTime.UtcNow;
                        _connections[i] = ci;
                        var root = addRootIfNeeded(tvItems);
                        if (root.Nodes.Count > i) {
                            root.Nodes[i].Tag = ci;
                        }
                        found = true;
                        break;
                    }
                }
                if (!found){
                    _connections.Add(ci);
                }
            }

            Document doc = new Document("Settings");
            //Toolkit.SaveRegSetting(HIVE, "GeneralAutoPopulate", _autoPopulate.ToString(), false);
            var nd = new Node("Connections");
            doc.Root.Nodes.Add(nd);
            foreach (var c in _connections) {
                nd.Nodes.Add(c.ToXmlNode());
            }
            doc.Save(getConnectionFilePath());

        }

        private bool promptForLoginIfNeeded(ConnectionInfo ci) {
            // first try to auto-login
            if (AdminHostProxy.AutoLogin(ci)) {
                SaveSettings(ci);
                return true;
            }


            if (DialogResult.OK == new frmLogin().ShowDialog(this, ci)) {
                // save connection to file, return true so caller knows connection is valid
                SaveSettings(ci);
                return true;
            }
            return false;
        }

        private bool _safeConnectionMode;
        private void tvItems_AfterSelect(object sender, TreeViewEventArgs e) {
            if (e.Node.Parent == null) {
                _currentConnectionInfo = null;
            } else {
                var ci = getConnectionForNode(e.Node);
                if (ci != null) {
                    if (_currentConnectionInfo != ci) {
                        try {
                            _currentConnectionInfo = null;
                            if (!promptForLoginIfNeeded(ci)) {
                                tvItems.SelectedNode = tvItems.Nodes[0];
                                return;
                            }
                        } catch (Exception ex) {
                            Debug.WriteLine("now what?");
                            var connNode = getRootConnectionNode(e.Node);
                            if (connNode != null) {
                                tvItems.SelectedNode = connNode;
                                Application.DoEvents();
                                fireDefaultContextMenuItem(connNode);
                                Application.DoEvents();
                                _currentConnectionInfo = ci;
                                return;
                            }
                        }
                        _currentConnectionInfo = ci;
                        var comps = new List<Component>();
                        foreach (Component c in components.Components) {
                            comps.Add(c);
                        }
                        comps.Add(this.MainMenuStrip);
                        GuiManager.LoadResources(GetResourceTable(true), "AdminTool", this, comps);
                    }
                }
            }

            if (e.Node.Tag is NodeInfo) {
                var ni = e.Node.Tag as NodeInfo;
                if (ni.RequiresElevation && !Toolkit.IsProcessElevated() && (Toolkit.IsWindows7 || Toolkit.IsWindowsVista)) {
                    // this is a node that requires administrative rights but the user does not currently have administrator rights
                    var f = new frmElevatePrompt();
                    if (DialogResult.OK == f.ShowDialog()) {
                        var gguac = Toolkit.GetPathToUACExe();
                        var adminTool = Application.ExecutablePath;
                        Toolkit.LaunchProcess(gguac, @"""" + adminTool + @""" /display " + e.Node.Name, false);
                        Application.Exit();
                        return;
                    }
                }
            }


            fireDefaultContextMenuItem(e.Node);
        }

        private bool _activated;
        private void mdiParent_Activated(object sender, EventArgs e) {
            if (!_activated) {
                // load db connections from encrypted txt file
                _activated = true;
                fillTreeView(true);

                if (!String.IsNullOrEmpty(DefaultNodeToDisplay)) {
                    if (!DefaultNodeToDisplay.ToLower().StartsWith("nd")) {
                        // all names always start with "nd", prepend it if needed
                        DefaultNodeToDisplay = "nd" + DefaultNodeToDisplay;
                    }
                    SelectChildTreeViewNodeByName(DefaultNodeToDisplay);

                } else {
                    autoImportDataviewIfNeeded(false);
                }

            }
        }

        private void tvItems_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {
        }

        private void fireDefaultContextMenuItem(TreeNode nd) {
            if (!_syncing){
                ContextMenuStrip ctxMenu = getContextMenu(nd);
                foreach (ToolStripItem tsmi in ctxMenu.Items) {
                    if (tsmi.Font.Bold) {
                        // this is the default context menu item (because it's bold), so click it
                        tsmi.PerformClick();
                        break;
                    }
                }
            }
        }

        private void fireDeleteContextMenuItem(TreeNode nd) {
            if (!_syncing) {
                ContextMenuStrip ctxMenu = getContextMenu(nd);
                foreach (ToolStripItem tsmi in ctxMenu.Items) {
                    if (tsmi.Text.Contains("Delete")) {
                        // this is the 'Delete' context menu item, so click it
                        tsmi.PerformClick();
                        break;
                    }
                }
            }
        }

        private TreeNode getRootConnectionNode(TreeNode nd) {
            while (nd != null && nd.Parent != null && !(nd.Tag is ConnectionInfo)) {
                nd = nd.Parent;
            }
            return nd;
        }

        private ConnectionInfo getConnectionForNode(TreeNode nd){
            var rootNode = getRootConnectionNode(nd);
            if (rootNode != null) {
                return rootNode.Tag as ConnectionInfo;
            } else {
                return null;
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e) {
            if (tvItems.SelectedNode != null) {
                var ci = getConnectionForNode(tvItems.SelectedNode);
                DeleteConnection(ci, tvItems.SelectedNode, true);
            }
        }

        public bool DeleteConnection(ConnectionInfo ci, TreeNode node, bool prompt){
            if (!prompt || DialogResult.Yes == MessageBox.Show(this, getDisplayMember("deleteConnection{start_body}", "Are you sure you want to delete connection {0}{1}?", ci.GrinGlobalUrl, ci.DatabaseEngineServerName),
                getDisplayMember("deleteConnection{start_title}", "Delete Connection?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                if (node == null) {
                    // find node with same connectioninfo as a tag
                    foreach (TreeNode nd in tvItems.Nodes[0].Nodes) {
                        if (nd.Tag == ci) {
                            node = nd;
                            break;
                        }
                    }
                }
                if (node != null) {
                    node.Remove();
                }
                _connections.Remove(ci);
                SaveSettings(null);
                return true;
            }
            return false;
        }

        private void notDone() {
            MessageBox.Show("not implemented yet");
        }

        //private bool _doingAutoPopulate;
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            showChildForm(new frmLogin(), tvItems.SelectedNode);

            //bool hasGrandChildren = (tvItems.SelectedNode.Nodes.Count > 0 ? tvItems.SelectedNode.Nodes[0].Nodes.Count : 0) > 0;
            //if (!hasGrandChildren) {
            //    try {
            //        if (!_doingAutoPopulate) {
            //            _doingAutoPopulate = true;
            //            //if (_autoPopulate) {
            //            //    // spin through children, populate all of them (note this only applies for root-level parents)
            //            //    var origNode = tvItems.SelectedNode;
            //            //    var nodes = new List<TreeNode>();
            //            //    foreach (TreeNode nd in tvItems.SelectedNode.Nodes) {
            //            //        nodes.Add(nd);
            //            //    }
            //            //    foreach (var nd in nodes) {
            //            //        // ignore notDone() ones for now
            //            //        if (nd.ImageIndex != 1) {
            //            //            tvItems.SelectedNode = nd;
            //            //        }
            //            //    }
            //            //    tvItems.SelectedNode = origNode;
            //            //}
            //        }
            //    } finally {
            //        _doingAutoPopulate = false;
            //    }
            //}
        }

        public void RefreshData() {
            if (!refreshChildNodes()) {
                refreshCurrentForm();
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void refreshToolStripMenuItem7_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void propertiesToolStripMenuItem8_Click(object sender, EventArgs e) {
            notDone();

        }

        private void ctxMenuNodeFilters_Opening(object sender, CancelEventArgs e) {
            
        }

        private void newFilterToolStripMenuItem_Click(object sender, EventArgs e) {
            PopupNewItemForm(new frmDataTrigger());

        }

        private void refreshToolStripMenuItem6_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void propertiesToolStripMenuItem7_Click(object sender, EventArgs e) {
            showChildForm(new frmDataTriggers(), tvItems.SelectedNode);
        }

        private void newTableMappingToolStripMenuItem_Click(object sender, EventArgs e) {
            PopupNewItemForm(new frmTableMappingInspectSchema());
        }

        private void refreshToolStripMenuItem5_Click(object sender, EventArgs e) {
            RefreshData();
            
        }

        private void propertiesToolStripMenuItem6_Click(object sender, EventArgs e) {
            showChildForm(new frmTableMappings(), tvItems.SelectedNode);
        }

        private void newDataViewToolStripMenuItem_Click(object sender, EventArgs e) {
            PopupNewItemForm(new frmDataview());
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e) {
            importDataview();
        }

        private void importDataview(){
            if (DialogResult.OK == ofdImport.ShowDialog(this)) {

                var fid = new frmImportDataviews();

                // show them a list of all the dataview names, let them select a subset
                foreach (var f in ofdImport.FileNames) {
                    fid.ImportFiles.Add(f);
                }

                if (DialogResult.OK == PopupForm(fid, this, false)) {
                    RefreshData();
                    UpdateStatus(getDisplayMember("importDataview{success}", "Imported {0} dataview(s)", fid.ImportedDataviews.Count.ToString()), true);
                }
            }
        }

        private void refreshToolStripMenuItem4_Click(object sender, EventArgs e) {
            RefreshData();
            
        }

        private void propertiesToolStripMenuItem5_Click(object sender, EventArgs e) {
            showChildForm(new frmDataviews(), tvItems.SelectedNode);

        }

        public object CurrentNodeTag() {
            if (tvItems.SelectedNode != null) {
                return tvItems.SelectedNode.Tag;
            } else {
                return null;
            }
        }
        public string CurrentNodeText(string defaultValue){
            if (tvItems.SelectedNode != null){
                return tvItems.SelectedNode.Text;
            } else {
                return defaultValue;
            }
        }

        public TreeNode CurrentNode() {
            return tvItems.SelectedNode;
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                using (new AutoCursor(this)) {
                    proxy.DisableUser(Toolkit.ToInt32(CurrentNodeTag(), -1));
                    RefreshData();
                    UpdateStatus(getDisplayMember("User {0} has been disabled.", CurrentNodeText("")), true);
                }
            }
        }

        private AdminHostProxy GetAdminHostProxy() {
            var conn = getConnectionForNode(tvItems.SelectedNode);
            if (conn != null) {
                return new AdminHostProxy(conn);
            }
            return null;
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e) {

            var f = new frmSetPassword();

            if (f.ShowDialog(this, CurrentNodeText("???")) == DialogResult.OK) {
                using (new AutoCursor(this)) {
                    var proxy = GetAdminHostProxy();
                    if (proxy != null) {
                        using (new AutoCursor(this)) {
                            proxy.ChangePassword(Toolkit.ToInt32(CurrentNodeTag(), -1), f.Password);
                            UpdateStatus(getDisplayMember("changePassword{success}", "Changed password for user {0}.", CurrentNodeText("???")), true);
                        }
                    }
                }
            }


            
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("deleteUser{start_body}", "Are you sure you want to delete {0}?", CurrentNodeText("")), 
                    getDisplayMember("deleteUser{start_title}", "Delete User?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    using (new AutoCursor(this)) {
                        proxy.DeleteUser(Toolkit.ToInt32(CurrentNodeTag(), -1));
                        UpdateStatus(getDisplayMember("deleteUser{done}", "User {0} has been deleted.", CurrentNodeText("")), true);
                    }
                }
            }

        }

        private void refreshToolStripMenuItem1_Click(object sender, EventArgs e) {
            RefreshData();
            
        }

        private void propertiesToolStripMenuItem1_Click(object sender, EventArgs e) {
            showChildForm(new frmUser(), tvItems.SelectedNode);

        }

        private void refreshToolStripMenuItem2_Click(object sender, EventArgs e) {
            RefreshData();
            
        }

        private void propertiesToolStripMenuItem3_Click(object sender, EventArgs e) {
            showChildForm(new frmPermissions(), tvItems.SelectedNode);

        }

        private void newPermissionTemplateToolStripMenuItem_Click(object sender, EventArgs e) {
            //PopupNewItemForm(new frmPermissionTemplate());
        }

        private void refreshToolStripMenuItem3_Click(object sender, EventArgs e) {
            RefreshData();

        }

        private void propertiesToolStripMenuItem4_Click(object sender, EventArgs e) {
            //showChildForm(new frmPermissionTemplates(), tvItems.SelectedNode);

        }

        private void propertiesToolStripMenuItem2_Click(object sender, EventArgs e) {
            showChildForm(new frmUsers(), tvItems.SelectedNode);

        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e) {
            frmOptions fo = new frmOptions();
            if (DialogResult.OK == fo.ShowDialog(this)) {
                // _autoPopulate = ("" + Toolkit.GetRegSetting(HIVE, "GeneralAutoPopulate", "false")).ToLower() == "true";
            }
        }

        private void tvItems_KeyPress(object sender, KeyPressEventArgs e) {
            if (tvItems.SelectedNode != null) {
                if ((Keys)e.KeyChar == Keys.Enter) {
                    fireDefaultContextMenuItem(tvItems.SelectedNode);
                    e.Handled = true;
                } else if ((Keys)e.KeyChar == Keys.Delete) {
                    fireDeleteContextMenuItem(tvItems.SelectedNode);
                    e.Handled = true;
                }
            }
        }

        private void ctxMenuTreeView_Opening(object sender, CancelEventArgs e) {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            RefreshData();

        }

        public void PopupNewItemForm(frmBase form) {

            if (initForm(form, this, null, true)) {
                if (form.ShowDialog() == DialogResult.OK) {
                    RefreshData();
                }
            }
        }

        private void ctxMenuNodeUser_Opening(object sender, CancelEventArgs e) {

        }

        private void disablePermissionMenuItem_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                using (new AutoCursor(this)) {
                    proxy.DisablePermission(Toolkit.ToInt32(CurrentNodeTag(), -1));
                    UpdateStatus(getDisplayMember("disablePermission{success}", "Permission {0} has been disabled.", CurrentNodeText("") ), true);
                }
            }

        }

        private void deletePermissionMenuItem_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {

                var id = Toolkit.ToInt32(CurrentNodeTag(), -1);

                var fmb = new frmMessageBox();
                fmb.Text = "Delete Permission(s)?";
                fmb.btnYes.Text = "&Delete";
                fmb.btnNo.Text = "&Cancel";

                fmb.txtMessage.Text = "Are you sure you want to delete the following permission(s)?\r\n" + CurrentNodeText("");
                if (DialogResult.Yes == fmb.ShowDialog(this)) {
                    try {
                        proxy.DeletePermission(id, false);
                        } catch (Exception ex) {
                            if (ex.Message.Contains("are referencing")) {
                                fmb.txtMessage.Text = ex.Message + "\r\n\r\nDo you want to remove the reference(s) and continue deleting " + CurrentNodeText("") + "?";
                                fmb.Text = "Remove References and Continue Delete?";
                                fmb.btnYes.Text = "Continue &Deleting";
                                fmb.btnNo.Text = "&Cancel";
                                if (DialogResult.Yes == fmb.ShowDialog(this)) {
                                    proxy.DeletePermission(id, true);
                                } else {
                                    // nothing to do
                                }
                            } else {
                                throw;
                            }
                        }
                    }
                    SelectParentTreeViewNode();
                    UpdateStatus(getDisplayMember("deletePermission{success}", "Permission {0} has been deleted.", CurrentNodeText("") ), true);

                //if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to delete permission " + CurrentNodeText(null) + "?", "Delete Permission?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                //    using (new AutoCursor(this)) {
                //    }
                //}
            }

        }

        private void refreshPermissionMenuItem_Click(object sender, EventArgs e) {
            refreshCurrentForm();
        }

        private void defaultPermissionMenuItem_Click(object sender, EventArgs e) {
            showChildForm(new frmPermission(), tvItems.SelectedNode);
        }

        private void tvItems_DragDrop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(typeof(DragDropObject))){
                DragDropObject ddo = e.Data.GetData(typeof(DragDropObject)) as DragDropObject;
                var nd = GuiManager.TreeViewSyncDrag((TreeView)sender, e);
                if (nd == null || nd.Parent == null || getRootConnectionNode(nd) != ddo.RootNode) {
                    // ignore anything not dropped on a node or dropped on the root node(s)
                    // also drag/drop has to occur on same server (i.e. both nodes have same root)
                } else {
                    var toCategoryName = nd.Parent.Name;
                    var id = Toolkit.ToInt32(nd.Tag, -1);

                    switch (ddo.FromCategoryName) {
                        case "ndPermissions":
                            if (toCategoryName == "ndUsers") {
                                // dragged permission to user
                                GetAdminHostProxy().AddPermissionsToUser(id, ddo.IDList);
                                UpdateStatus(getDisplayMember("itemsDragDrop{addpermtouser_success}", "Added permission(s) to user {0}", nd.Text), true, nd);
                            } else if (toCategoryName == "ndGroups") {
                                // dragged permission to group
                                GetAdminHostProxy().AddPermissionsToGroup(id, ddo.IDList);
                                UpdateStatus(getDisplayMember("itemsDragDrop{addpermtogroup_success}", "Added permission(s) to group {0}", nd.Text), true, nd);
                            } else if (toCategoryName == "ndPermissionTemplates") {
                                // dragged permission to permission template
                                GetAdminHostProxy().AddPermissionsToTemplate(id, ddo.IDList);
                                UpdateStatus(getDisplayMember("itemsDragDrop{addpermtotemplate_success}", "Added permission(s) to template {0}", nd.Text), true, nd);
                            }
                            break;
                        case "ndUsers":
                            if (toCategoryName == "ndGroups") {
                                // dragged user to group
                                GetAdminHostProxy().AddUsersToGroup(id, ddo.IDList);
                                UpdateStatus(getDisplayMember("itemsDragDrop{addusertogroup_success}", "Added user(s) to group {0}", nd.Text), true, nd);
                            }
                            break;
                        case "ndGroups":
                            // dragged group ... anything to do?  have no idea
                            break;
                        case "ndPermissionTemplates":
                            if (toCategoryName == "ndUsers") {
                                // dragged permission template to user
                                GetAdminHostProxy().ApplyPermissionTemplatesToUser(id, ddo.IDList);
                                UpdateStatus(getDisplayMember("itemsDragDrop{addpermtotemplate_success}", "Added permission(s) in template {0} to user {1}", ddo.SourceNode.Text, nd.Text), true, nd);
                            } else if (toCategoryName == "ndGroups"){
                                // dragged permission template to group
                            }
                            break;
                        default:
                            MessageBox.Show(getDisplayMember("itemDragDrop{default}", "You dropped:\n{0}\n on {1}, whose FromCategoryName is {2} and ToCategoryName is {3}", e.Data.GetData(typeof(DragDropObject)) as string, nd.Tag as string, ddo.FromCategoryName, toCategoryName));
                            break;
                    }
                }
            }
        }

        //private long _tvItemTicks;
        //private TreeNode syncDropTargetNode(DragEventArgs e) {
        //    //int x = e.X - this.Left - tvItems.Left;
        //    //int y = e.Y - this.Top - tvItems.Top - toolStrip.Height;
        //    var pt = tvItems.PointToClient(new Point(e.X, e.Y));
        //    var nd = tvItems.GetNodeAt(pt);
        //    if (_tvItemTicks == 0){
        //        _tvItemTicks = DateTime.Now.Ticks;
        //    }

        //    if (nd != null) {

        //        if (pt.X < 16) {
        //            nd.EnsureVisible();
        //        }

        //        var ts = new TimeSpan(DateTime.Now.Ticks - _tvItemTicks);
        //        if (pt.Y < tvItems.ItemHeight) {
        //            if (nd.PrevVisibleNode != null) {
        //                if (nd.PrevVisibleNode.Tag is ConnectionInfo) {
        //                    return nd;
        //                }
        //                var prev = nd.PrevVisibleNode;
        //                if (!prev.IsExpanded) {
        //                    prev.Expand();
        //                    if (prev.Nodes.Count > 0) {
        //                        nd = prev.Nodes[prev.Nodes.Count - 1];
        //                    } else {
        //                        nd = prev;
        //                    }
        //                } else {
        //                    nd = nd.PrevVisibleNode;
        //                }
        //            }
        //            nd.EnsureVisible();
        //            _tvItemTicks = DateTime.Now.Ticks;
        //        } else if (pt.Y < (tvItems.ItemHeight * 2)) {
        //            if (ts.TotalMilliseconds > 250) {
        //                if (nd.PrevVisibleNode.Tag is ConnectionInfo) {
        //                    return nd;
        //                }
        //                nd = nd.PrevVisibleNode;
        //                if (nd.PrevVisibleNode != null) {
        //                    var prev = nd.PrevVisibleNode;
        //                    if (!prev.IsExpanded) {
        //                        prev.Expand();
        //                        if (prev.Nodes.Count > 0) {
        //                            nd = prev.Nodes[prev.Nodes.Count - 1];
        //                        } else {
        //                            nd = prev;
        //                        }
        //                    } else {
        //                        nd = nd.PrevVisibleNode;
        //                    }
        //                }
        //                nd.EnsureVisible();
        //                _tvItemTicks = DateTime.Now.Ticks;
        //            }
        //        } else if (pt.Y > (tvItems.Height - tvItems.ItemHeight * 2)) {
        //            Debug.WriteLine("fast scroll down");
        //            if (nd.NextVisibleNode.Tag is ConnectionInfo) {
        //                return nd;
        //            }
        //            nd = nd.NextVisibleNode;
        //            if (nd.NextVisibleNode != null) {
        //                var next = nd.NextVisibleNode;
        //                if (!next.IsExpanded) {
        //                    next.Expand();
        //                    nd = next;
        //                } else {
        //                    nd = nd.NextVisibleNode;
        //                }
        //            }
        //            nd.EnsureVisible();
        //            _tvItemTicks = DateTime.Now.Ticks;
        //        } else if (pt.Y > tvItems.Height - tvItems.ItemHeight * 3) {
        //            Debug.WriteLine("slow scroll down");
        //            if (ts.TotalMilliseconds > 250) {
        //                if (nd.NextVisibleNode != null) {
        //                    if (nd.NextVisibleNode.Tag is ConnectionInfo) {
        //                        return nd;
        //                    }
        //                    var next = nd.NextVisibleNode;
        //                    if (!next.IsExpanded) {
        //                        next.Expand();
        //                        nd = next;
        //                    } else {
        //                        nd = nd.NextVisibleNode;
        //                    }
        //                }
        //                nd.EnsureVisible();
        //                _tvItemTicks = DateTime.Now.Ticks;
        //            }
        //        }
        //    }

        //    return nd;
        //}

        //private TreeNode getRoot(TreeNode nd) {
        //    if (nd == null || nd.Parent == null) {
        //        return nd;
        //    } else {
        //        var ndRoot = nd.Parent;
        //        while (ndRoot.Parent != null) {
        //            ndRoot = ndRoot.Parent;
        //        }
        //        return ndRoot;
        //    }
        //}

        private void tvItems_DragOver(object sender, DragEventArgs e) {

            var nd = GuiManager.TreeViewSyncDrag((TreeView)sender, e);
            if (nd != null && nd.Parent != null) {
                if (nd.Parent.Tag is ConnectionInfo){
                    nd.Expand();
                    return;
                }

                if (e.Data.GetDataPresent(typeof(DragDropObject))) {
                    DragDropObject tgt = e.Data.GetData(typeof(DragDropObject)) as DragDropObject;
                    if (tgt.RootNode == getRootConnectionNode(nd)) {

                        var toCategoryName = nd.Parent.Name;
                        var toTag = ("" + nd.Tag);

                        Debug.WriteLine("from " + tgt.FromCategoryName + "." + tgt.IDList[0] + " to " + toCategoryName + "." + toTag);

                        switch (tgt.FromCategoryName) {
                            case "ndUsers":
                                switch (toCategoryName) {
                                    case "ndGroups":
                                        e.Effect = DragDropEffects.Copy;
                                        Debug.WriteLine("COPY");
                                        break;
                                }
                                break;
                            case "ndGroups":
                                break;
                            case "ndPermissions":
                                switch (toCategoryName) {
                                    case "ndGroups":
                                    case "ndUsers":
                                    case "ndPermissionTemplates":
                                        e.Effect = DragDropEffects.Copy;
                                        Debug.WriteLine("COPY");
                                        break;
                                }
                                break;
                            case "ndPermissionTemplates":
                                switch (toCategoryName) {
                                    case "ndGroups":
                                    case "ndUsers":
                                        e.Effect = DragDropEffects.Copy;
                                        Debug.WriteLine("COPY");
                                        break;
                                }

                                break;
                            default:
                                break;
                        }

                    }
                }
            }

        }

        public void SelectCousinTreeNode(string parentName, string childTag) {
            var n = GetCousinTreeNode(parentName, childTag);
            if (n != null) {
                tvItems.SelectedNode = n;
            }
        }

        public void SelectSiblingTreeNode(string siblingTag) {
            var n = GetSiblingTreeNode(siblingTag);
            if (n != null) {
                tvItems.SelectedNode = n;
            }
        }

        public TreeNode GetSiblingTreeNode(string siblingTag) {
            var parent = tvItems.SelectedNode;
            if (parent != null) {
                foreach (TreeNode n in parent.Nodes) {
                    if ((string.Empty + n.Tag) == siblingTag) {
                        return n;
                    }
                }
            }
            return null;
        }

        public TreeNode GetCousinTreeNode(string parentName, string childTag){
            // bounce up until we find the root parent node (which has the connectioninfo object)
            var root = getRootConnectionNode(tvItems.SelectedNode);
            var parent = root.Nodes[parentName];
            if (parent != null){
                // make sure it's populated
                tvItems.SelectedNode = parent;
//                return getDescendentTreeNode(childTag, parent);
//                refreshChildNodes();
                foreach (TreeNode n in parent.Nodes) {
                    if (n.Tag.ToString() == childTag) {
                        return n;
                    }
                }
            }
            return null;
        }

        private TreeNode getDescendentTreeNode(string tag, TreeNode parent) {
            foreach (TreeNode n in parent.Nodes) {
                if (String.Compare(n.Tag.ToString(), tag, true) == 0) {
                    return n;
                } else {
                    return getDescendentTreeNode(tag, n);
                }
            }
            return null;
        }

        public TreeNode GetUncleTreeNode(string parentName) {
            // bounce up until we find the root parent node (which has the connectioninfo object)
            var root = getRootConnectionNode(tvItems.SelectedNode);
            var parent = root.Nodes[parentName];
            if (parent != null) {
                return parent;
            }
            return null;
        }

        public TreeNode GetAncestorTreeNode(string ancestorNodeName) {
            // bounce up until we find the root parent node (which has the connectioninfo object)
            var nd = tvItems.SelectedNode;
            if (String.Compare(nd.Name, ancestorNodeName, true) == 0) {
                return nd;
            }

            var root = getRootConnectionNode(nd);
            while (root != nd) {
                nd = nd.Parent;
                if (String.Compare(nd.Name, ancestorNodeName, true) == 0) {
                    return nd;
                }
            }
            return null;
        }

        private void tvItems_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                fireDeleteContextMenuItem(tvItems.SelectedNode);
                e.Handled = true;
            }

        }

        private void propertiesPermissionTemplateMenuItem_Click(object sender, EventArgs e) {
            //showChildForm(new frmPermissionTemplate(), tvItems.SelectedNode);
        }

        private void refreshPermissionTemplateMenuItem_Click(object sender, EventArgs e) {
            RefreshData();

        }

        private void deletePermissionTemplateMenuItem_Click(object sender, EventArgs e) {
            //var proxy = GetAdminHostProxy();
            //if (proxy != null) {
            //    if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to delete " + CurrentNodeText("") + "?", "Delete Template?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
            //        using (new AutoCursor(this)) {
            //            proxy.DeletePermissionTemplate(Toolkit.ToInt32(CurrentNodeTag(), -1));
            //            UpdateStatus("Permission Template " + CurrentNodeText("") + " has been deleted.", true);
            //            SelectParentTreeViewNode();
            //        }
            //    }
            //}
        }

        private void tvItems_ItemDrag(object sender, ItemDragEventArgs e) {
            var nd = e.Item as TreeNode;
            if (String.IsNullOrEmpty(nd.Name)) {
                tvItems.DoDragDrop(new DragDropObject(nd), DragDropEffects.Copy);
            }
        }

        private void refreshToolStripMenuItem8_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            PopupNewItemForm(new frmSearchEngineIndex());

        }

        private void showLocalOnlyMessage(string component) {
            MessageBox.Show(this, getDisplayMember("showLocalOnlyMessage{body}", "{0} can not be administered on a remote computer.",  component), 
                getDisplayMember("showLocalOnlyMessage{title}", "Local Connection Required"));
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e) {
            var ci = this.getConnectionForNode(tvItems.SelectedNode);
            //if (ci.IsLocal){
                showChildForm(new frmSearchEngineIndexes(), tvItems.SelectedNode);
            //} else {
            //    showLocalOnlyMessage("Search engine ");
            //}
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e) {
            RefreshData();

        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                using (new AutoCursor(this)) {
                    //proxy.DisableSearchEngineIndex(Toolkit.ToInt32(CurrentNodeTag(), -1));
                    UpdateStatus(getDisplayMember("disableSEIndex{success}", "Search Engine Index {0} has been disabled.", CurrentNodeText("")), true);
                }
            }

        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("deleteSearchEngineIndex{start_body}", "Are you sure you want to delete search engine index {0}?", CurrentNodeText(null)), 
                    getDisplayMember("deleteSearchEngineIndex{start_title}", "Delete Index?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    using (new AutoCursor(this)) {
                        proxy.DeleteSearchEngineIndex(CurrentNodeTag().ToString());
                        UpdateStatus(getDisplayMember("deleteSEIndex{success}", "Index {0} has been deleted.", CurrentNodeText("")), true);
                        SelectParentTreeViewNode();
                    }
                }
            }

        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e) {
            refreshCurrentForm();

        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e) {
            showChildForm(new frmSearchEngineIndex(), tvItems.SelectedNode);
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e) {
            showChildForm(new frmTableMapping(), tvItems.SelectedNode);
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e) {
            refreshCurrentForm();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("deleteTableMapping{start_body}", "Are you sure you want to delete table mapping {0}?", CurrentNodeText(null)), 
                    getDisplayMember("deleteTableMapping{start_title}", "Delete Table Mapping?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    using (new AutoCursor(this)) {
                        try {
                            proxy.DeleteTableMapping(Toolkit.ToInt32(CurrentNodeTag(), -1), false);
                            UpdateStatus(getDisplayMember("deleteTableMapping{success}", "Table Mapping {0} has been deleted.", CurrentNodeText("") ), true);
                            SelectParentTreeViewNode();
                        } catch (Exception ex) {
                            MessageBox.Show(this, getDisplayMember("deleteTableMapping{failed_body}", "Could not delete mapping for table '{0}': {1}",  CurrentNodeText(""), ex.Message),
                                getDisplayMember("deleteTableMapping{failed_title}", "Failed to Delete Table Mapping"));
                        }
                    }
                }
            }

        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                //if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to delete table mapping " + CurrentNodeText(null) + "?", "Delete Table Mapping?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    using (new AutoCursor(this)) {
                        proxy.DisableTableMapping(Toolkit.ToInt32(CurrentNodeTag(), -1));
                        UpdateStatus(getDisplayMember("disableTableMapping{success}", "Table Mapping {0} has been disabled.",  CurrentNodeText("") ), true);
                        SelectParentTreeViewNode();
                    }
                //}
            }
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e) {

            stopService("ggse", "GRIN-Global Search Engine");
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e) {
            startService("ggse", "GRIN-Global Search Engine");
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e) {
            stopService("ggse", "GRIN-Global Search Engine");
            startService("ggse", "GRIN-Global Search Engine");
        }

        private void stopService(string serviceName, string displayName) {
            var f = new frmSplash();
            try {
                f.Show("Stopping " + displayName + "...", false, this);
                Toolkit.StopService(serviceName);
                f.Close();
            } catch (Exception ex) {
                f.Close();
                MessageBox.Show(this, getDisplayMember("stopService{failed_body}", "Failed to stop {0}:\n{1}", displayName, ex.Message), 
                    getDisplayMember("stopService{faile_title}", "Service Stop Failed"));
            }
        }

        private void startService(string serviceName, string displayName) {
            var f = new frmSplash();
            try {
                f.Show("Starting " + displayName + "...", false, this);
                Toolkit.StartService(serviceName);
                f.Close();
            } catch (Exception ex) {
                f.Close();
                MessageBox.Show(this, getDisplayMember("startService{failed_body}", "Failed to start {0}:\n{1}", displayName, ex.Message),
                    getDisplayMember("startService{failed_title}", "Service Start Failed"));
            }

        }

        private void ctxMenuNodeSearchEngineIndexes_Opening(object sender, CancelEventArgs e) {
            var c = getConnectionForNode(tvItems.SelectedNode);
            //if (!c.IsLocal) {
            //    e.Cancel = true;
            //    showLocalOnlyMessage("Search engine ");
            //    return;
            //}

            if (!c.IsLocal) {
                cmiSearchEngineStart.Enabled = false;
                cmiSearchEngineStop.Enabled = false;
                cmiSearchEngineRestart.Enabled = false;
            } else {
                if (Toolkit.IsServiceRunning("ggse")) {
                    cmiSearchEngineStart.Enabled = false;
                    cmiSearchEngineStop.Enabled = true;
                } else {
                    cmiSearchEngineStart.Enabled = true;
                    cmiSearchEngineStop.Enabled = false;
                }
            }
        }

        private void toolStripMenuItem13_Click_1(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                using (new AutoCursor(this)) {
                    proxy.DisableTrigger(Toolkit.ToInt32(CurrentNodeTag(), -1));
                    UpdateStatus(getDisplayMember("disableTrigger{success}", "Trigger {0} has been disabled.", CurrentNodeText("")), true);
                    SelectParentTreeViewNode();
                }
            }

        }

        private void toolStripMenuItem14_Click_1(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("deleteTrigger{start_body}", "Are you sure you want to delete trigger {0}?", CurrentNodeText(null)), 
                    getDisplayMember("deleteTrigger{start_title}", "Delete Trigger?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    using (new AutoCursor(this)) {
                        proxy.DeleteTrigger(Toolkit.ToInt32(CurrentNodeTag(), -1));
                        UpdateStatus(getDisplayMember("deleteTrigger{success}", "Trigger {0} has been deleted.", CurrentNodeText("")), true);
                        SelectParentTreeViewNode();
                    }
                }
            }


        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e) {
            refreshCurrentForm();

        }

        private void toolStripMenuItem17_Click(object sender, EventArgs e) {
            showChildForm(new frmDataTrigger(), tvItems.SelectedNode);
        }

        private void toolStripMenuItem19_Click(object sender, EventArgs e) {
            refreshCurrentForm();
        }

        private void toolStripMenuItem20_Click(object sender, EventArgs e) {
            showChildForm(new frmFileGroups(), tvItems.SelectedNode);
        }

        private void ctxMenuNodeFileMappings_Opening(object sender, CancelEventArgs e) {

        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e) {
            PopupNewItemForm(new frmFileGroup());
        }

        private void toolStripMenuItem23_Click(object sender, EventArgs e) {
            refreshCurrentForm();

        }

        private void ctxMenuNodeFileGroup_Opening(object sender, CancelEventArgs e) {

        }

        private void toolStripMenuItem24_Click(object sender, EventArgs e) {
            showChildForm(new frmFileGroup(), tvItems.SelectedNode);
        }

        private void toolStripMenuItem26_Click(object sender, EventArgs e) {
            refreshCurrentForm();
        }

        private void toolStripMenuItem27_Click(object sender, EventArgs e) {
            var ci = this.getConnectionForNode(tvItems.SelectedNode);
            //if (ci.IsLocal) {
                showChildForm(new frmWebApplication(), tvItems.SelectedNode);
            //} else {
            //    showLocalOnlyMessage("Web Application");
            //}
        }

        private void toolStripMenuItem20_Click_1(object sender, EventArgs e) {
            showChildForm(new frmFileGroups(), tvItems.SelectedNode);
        }

        private void toolStripMenuItem26_Click_1(object sender, EventArgs e) {
            var splash = new frmSplash();
            try {
                splash.Show("Stopping IIS...", false, this);
                Toolkit.StopService("w3svc");
            } finally {
                splash.Close();
            }
        }

        private void toolStripMenuItem27_Click_1(object sender, EventArgs e) {
            var splash = new frmSplash();
            try {
                splash.Show("Starting IIS...", false, this);
                Toolkit.StartService("w3svc");
            } finally {
                splash.Close();
            }

        }

        private void toolStripMenuItem28_Click(object sender, EventArgs e) {
            var splash = new frmSplash();
            try {
                splash.Show(getDisplayMember("restartIIS{stopping}", "Stopping IIS..."), false, this);
                Toolkit.StopService("w3svc");
                splash.ChangeText(getDisplayMember("restartIIS{starting}", "Starting IIS..."));
                Toolkit.StartService("w3svc");
            } finally {
                splash.Close();
            }

        }

        private void toolStripMenuItem29_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void toolStripMenuItem30_Click(object sender, EventArgs e) {
            var ci = this.getConnectionForNode(tvItems.SelectedNode);
            //if (ci.IsLocal) {
                showChildForm(new frmWebApplication(), tvItems.SelectedNode);
            //} else {
            //    showLocalOnlyMessage("Web Application");
            //}
        }

        private void ctxMenuNodeWebApplication_Opening_1(object sender, CancelEventArgs e) {
            var c = getConnectionForNode(tvItems.SelectedNode);
            //if (!c.IsLocal) {
            //    e.Cancel = true;
            //    showLocalOnlyMessage("Web Application");
            //    return;
            //} else {

                var path = c.WebAppPhysicalPath;
                if (String.IsNullOrEmpty(path)) {
                    // web app is not installed locally
                    e.Cancel = true;
                    return;
                }

            //}

            if (!c.IsLocal) {
                cmiWebAppStart.Enabled = false;
                cmiWebAppRestart.Enabled = false;
                cmiWebAppStop.Enabled = false;
            } else {
                if (Toolkit.IsServiceRunning("w3svc")) {
                    cmiWebAppStart.Enabled = false;
                    cmiWebAppStop.Enabled = true;
                } else {
                    cmiWebAppStart.Enabled = true;
                    cmiWebAppStop.Enabled = false;
                }
            }
        }

        private void toolStripMenuItem27_Click_2(object sender, EventArgs e) {
            showChildForm(new frmAppSetting(), tvItems.SelectedNode);
        }

        private void toolStripMenuItem33_Click(object sender, EventArgs e) {
            showChildForm(new frmSearchEngineResolver(), tvItems.SelectedNode);
        }

        private void toolStripMenuItem22_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("deleteFileGroup{start_body}", "Are you sure you want to delete file group '{0};?\nNo files will be deleted.", CurrentNodeText("")), getDisplayMember("deleteFileGroup{start_title}", "Delete File Group?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    using (new AutoCursor(this)) {
                        proxy.DeleteFileGroup(Toolkit.ToInt32(CurrentNodeTag(), -1));
                        UpdateStatus(getDisplayMember("deleteFileGroup{success}", "File Group '{0} has been deleted.", CurrentNodeText("")), true);
                        SelectParentTreeViewNode();
                    }
                }
            }
            
        }

        private void toolStripMenuItem34_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                using (new AutoCursor(this)) {
                    proxy.EnableTrigger(Toolkit.ToInt32(CurrentNodeTag(), -1));
                    UpdateStatus(getDisplayMember("enableTrigger{success}", "Trigger {0} has been enabled.", CurrentNodeText("") ), true);
                    SelectParentTreeViewNode();
                }
            }

        }

        private void toolStripMenuItem35_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("deleteSecurityGroup{start_body}", "Are you sure you want to delete {0}?", CurrentNodeText("")), 
                    getDisplayMember("deleteSecurityGroup{start_title}", "Delete Group?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    using (new AutoCursor(this)) {
                        proxy.DeleteGroup(Toolkit.ToInt32(CurrentNodeTag(), -1));
                        UpdateStatus(getDisplayMember("deleteSecurityGroup{success}", "Group {0} has been deleted.", CurrentNodeText("") ), true);
                        SelectParentTreeViewNode();
                    }
                }
            }

        }

        private void toolStripMenuItem36_Click(object sender, EventArgs e) {
            RefreshData();

        }

        private void ctxMenuNodeGroup_Opening(object sender, CancelEventArgs e) {

        }

        private void toolStripMenuItem37_Click(object sender, EventArgs e) {
            showChildForm(new frmGroup(), tvItems.SelectedNode);

        }

        private void toolStripMenuItem38_Click(object sender, EventArgs e) {
            PopupNewItemForm(new frmGroup());

        }

        private void toolStripMenuItem39_Click(object sender, EventArgs e) {
            RefreshData();

        }

        private void toolStripMenuItem40_Click(object sender, EventArgs e) {
            showChildForm(new frmGroups(), tvItems.SelectedNode);

        }

        private void cmiDataviewDelete_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {

                var name = CurrentNodeTag().ToString();
                var fmb = new frmMessageBox();
                fmb.Text = "Delete Dataview(s)?";
                fmb.btnYes.Text = "&Delete";
                fmb.btnNo.Text = "&Cancel";
                fmb.txtMessage.Text = "Are you sure you want to delete dataview " + name + "?";

                if (DialogResult.Yes == fmb.ShowDialog(this)) {
                    using (new AutoCursor(this)) {
                        try {
                            proxy.DeleteDataViewDefinition(name, false);
                            SelectParentTreeViewNode();
                            UpdateStatus(getDisplayMember("deleteDataview{success}", "Dataview {0} has been deleted.",CurrentNodeText("")), true);
                        } catch (Exception ex) {
                            if (ex.Message.Contains("following are referencing")) {
                                fmb = new frmMessageBox();
                                fmb.Text = "Remove References and Continue Delete?";
                                fmb.btnYes.Text = "Continue &Deleting";
                                fmb.btnNo.Text = "&Cancel";
                                fmb.txtMessage.Text = ex.Message + "\r\n\r\nDo you want to remove the reference(s) and continue deleting " + name + "?";
                                if (DialogResult.Yes == fmb.ShowDialog(this)) {
                                    proxy.DeleteDataViewDefinition(name, true);
                                }
                            }
                        }
                    }
                }
            }

        }

        private void cmiDataviewRefresh_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void cmiDataviewProperties_Click(object sender, EventArgs e) {
            var f = new frmDataview();
            if (tvItems.SelectedNode.Tag != null) {
                f.DataviewName = tvItems.SelectedNode.Tag.ToString();
                showChildForm(f, tvItems.SelectedNode);
            }
        }

        private void cmDataview_Opening(object sender, CancelEventArgs e) {

        }

        private void cmiDataviewExport_Click(object sender, EventArgs e) {
            sfdExport.FileName = DateTime.Now.ToString("yyyy_MM_dd_");
            sfdExport.FileName += CurrentNodeText("");
            sfdExport.OverwritePrompt = true;
            sfdExport.SupportMultiDottedExtensions = true;

            if (DialogResult.OK == sfdExport.ShowDialog(this)) {

                var dvs = new List<string>();
                dvs.Add(CurrentNodeTag() + string.Empty);




                showSplash(getDisplayMember("export{progress}", "Exporting {0} dataviews...", dvs.Count.ToString()));
                //changeSplashText();

                var bw = new BackgroundWorker();
                //bw.WorkerReportsProgress = true;
                //bw.WorkerSupportsCancellation = true;
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

                var dic = new Dictionary<string, object>();
                dic.Add("proxy", GetAdminHostProxy());
                dic.Add("filename", sfdExport.FileName);
                dic.Add("dataviews", dvs);
                dic.Add("completedmessage", "Exported " + dvs.Count + " dataviews");
                bw.RunWorkerAsync(dic);

                //var ds = GetAdminHostProxy().ExportDataViewDefinitions(dvs);

                //BinaryFormatter bin = new BinaryFormatter();
                //using (StreamWriter sw = new StreamWriter(sfdExport.FileName)) {
                //    ds.RemotingFormat = SerializationFormat.Binary;
                //    bin.Serialize(sw.BaseStream, ds);
                //}

            }

        }

        private void mnuViewFullScreen_Click(object sender, EventArgs e) {
            if (mnuViewTheaterMode.Checked) {
                this.WindowState = FormWindowState.Normal;
                mnuViewTheaterMode.Checked = false;
            } else {
                this.WindowState = FormWindowState.Maximized;
                mnuViewTheaterMode.Checked = true;
            }

            if (ActiveForm != null) {
                if (!ActiveForm.Modal) {
                    if (ActiveForm is frmBase) {
                        var f = ((frmBase)ActiveForm);
                        f.FullScreenToggled(this.WindowState == FormWindowState.Maximized);
                    }
                    // HACK: forces child form to align properly
                    this.Height += 1;
                    this.Height -= 1;

                }
            }

        }

        private void cmiDataviewCopyTo_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            var f = new frmCopyDataviewTo();
            f.Copying = true;
            var origName = CurrentNodeText("-");
            f.OriginalDataviewName = origName;
            f.txtNewDataviewName.Text = origName;
            if (DialogResult.OK == PopupForm(f, this, false)) {
                var newName = f.txtNewDataviewName.Text.Trim();
                proxy.CopyDataViewDefinition(origName, newName);
                SelectParentTreeViewNode();
                UpdateStatus(getDisplayMember("dataviewCopyTo{success}", "Copied {0} to {1}", origName, newName), true);
            }

        }

        private void cmiDataviewVerify_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            var name = CurrentNodeText("-");

            var f = new frmVerifyDataviews();
            f.Dataviews.Add(name);
            PopupForm(f, this, false);
        }

        private void cmiDataviewsVerifyAll_Click(object sender, EventArgs e) {
            verifyDescendentDataviews();
        }

        private void verifyDescendentDataviews(){
            var proxy = GetAdminHostProxy();
            var f = new frmVerifyDataviews();
            var nd = CurrentNode();
            foreach (TreeNode tn in nd.Nodes) {
                if (nd.Name.ToLower() == "nddataviews") {
                    foreach (TreeNode gn in tn.Nodes) {
                        var tag = gn.Tag + "";
                        if (tag != string.Empty) {
                            f.Dataviews.Add(tag);
                        }
                    }
                } else {
                    var tag = tn.Tag + "";
                    if (tag != string.Empty) {
                        f.Dataviews.Add(tag);
                    }
                }
            }
            if (f.Dataviews.Count > 0) {
                PopupForm(f, this, false);
            }

        }

        private void mdiParent_SizeChanged(object sender, EventArgs e) {

        }


        private void tvItems_BeforeSelect(object sender, TreeViewCancelEventArgs e) {
            //if (_previousForm != null) {
            //    _previousForm.Close();
            //}
            //_previousTreeViewNode = e.Node;
        }

        private void cmiTableMappingsCreateRelationships_Click(object sender, EventArgs e) {
            using (new AutoCursor(this)) {
                var names = new List<string>();
                foreach (TreeNode nd in tvItems.SelectedNode.Nodes){
                    names.Add(nd.Text);
                }
                var proxy = GetAdminHostProxy();
                UpdateStatus(getDisplayMember("creatingUnmapped{start}", "Creating unmapped relationships..."), false);
                proxy.RecreateTableRelationships(names);
                RefreshData();
                UpdateStatus(getDisplayMember("creatingUnmapped{done}", "Created new relationship(s) for {0} tables.", names.Count.ToString()), true);
            }

        }

        private void cmiUserEnable_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                using (new AutoCursor(this)) {
                    proxy.EnableUser(Toolkit.ToInt32(CurrentNodeTag(), -1));
                    RefreshData();
                    UpdateStatus(getDisplayMember("enableUser{done}", "User has been enabled.",CurrentNodeText("") ), true);
                }
            }
        }

        private void cmiDataviewRename_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            var f = new frmCopyDataviewTo();
            f.Copying = false;
            var origName = CurrentNodeTag().ToString();
            f.OriginalDataviewName = origName;
            f.txtNewDataviewName.Text = origName;
            if (DialogResult.OK == PopupForm(f, this, false)) {
                var newName = f.txtNewDataviewName.Text.Trim();
                proxy.RenameDataViewDefinition(origName, newName);
                SelectParentTreeViewNode();
                UpdateStatus(getDisplayMember("dataviewRename{success}", "Renamed {0} to {1}", origName ,newName), true);
            }

        }

        private void cmiRootNew_Click(object sender, EventArgs e) {
            PromptForNewConnection();
        }

        private void cmiRootRefresh_Click(object sender, EventArgs e) {
            fillTreeView(true);
        }

        private void cmiRootProperties_Click(object sender, EventArgs e) {
            showChildForm(new frmConnections(), null);
        }

        private void cmiDataviewsExportAll_Click(object sender, EventArgs e) {
            exportDescendentDataviews();
        }

        private void exportDescendentDataviews(){

            var nd = tvItems.SelectedNode;

            sfdExport.FileName = DateTime.Now.ToString("yyyy_MM_dd_");

            if (nd.Name.ToLower() == "nddataviews") {
                sfdExport.FileName += "all";
            } else {
                sfdExport.FileName += nd.Text.Replace(" ", "_") + "_all";
            }

            sfdExport.OverwritePrompt = true;
            sfdExport.SupportMultiDottedExtensions = true;

            if (DialogResult.OK == sfdExport.ShowDialog(this)) {

                var dvs = new List<string>();
                foreach (TreeNode tn in nd.Nodes) {
                    if (nd.Name.ToLower() == "nddataviews") {
                        foreach (TreeNode gn in tn.Nodes) {
                            var tag = gn.Tag + "";
                            if (tag != string.Empty) {
                                dvs.Add(tag);
                            }
                        }
                    } else {
                        var tag = tn.Tag + "";
                        if (tag != string.Empty) {
                            dvs.Add(tag);
                        }
                    }
                }

                showSplash(getDisplayMember("export{progress}", "Exporting {0} dataviews...", dvs.Count.ToString()));
                //changeSplashText();

                var bw = new BackgroundWorker();
                //bw.WorkerReportsProgress = true;
                //bw.WorkerSupportsCancellation = true;
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

                var dic = new Dictionary<string, object>();
                dic.Add("proxy", GetAdminHostProxy());
                dic.Add("filename", sfdExport.FileName);
                dic.Add("dataviews", dvs);
                dic.Add("completedmessage", "Exported " + dvs.Count + " dataviews");
                bw.RunWorkerAsync(dic);

            }

        }

        private frmSplash _splash;
        void showSplash(string text) {
            if (_splash == null) {
                _splash = new frmSplash();
                this.Cursor = Cursors.WaitCursor;
            }
            _splash.Show(text, false, this);
            Application.DoEvents();
        }

        void changeSplashText(string text) {
            if (_splash != null) {
                _splash.ChangeText(text);
                Application.DoEvents();
            }
        }
        void hideSplash() {
            if (_splash != null) {
                _splash.Close();
                _splash = null;
                this.Cursor = Cursors.Default;
                Application.DoEvents();
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            hideSplash();
            if (e.Result != null) {
                if (e.Result is string) {
                    UpdateStatus(e.Result as string, true);
                } else if (e.Result is Exception) {
                    ReportError((e.Result as Exception).Message);
                    MessageBox.Show(this, getDisplayMember("runworker{failed_body}", "Error: {0}", (e.Result as Exception).Message),
                        getDisplayMember("runworker{failed_title}", "Error"));
                } else {
                    UpdateStatus(e.Result.ToString(), true);
                }
            }
        }

        void bw_DoWork(object sender, DoWorkEventArgs e) {
            var dic = e.Argument as Dictionary<string, object>;
            var dvs = dic["dataviews"] as List<string>;
            try {
                var filename = dic["filename"] as string;
                var proxy = dic["proxy"] as AdminHostProxy;
                var ds = proxy.ExportDataViewDefinitions(dvs);
                BinaryFormatter bin = new BinaryFormatter();
                using (StreamWriter sw = new StreamWriter(filename)) {
                    ds.RemotingFormat = SerializationFormat.Binary;
                    bin.Serialize(sw.BaseStream, ds);
                }
                if (dic.ContainsKey("completedmessage")) {
                    e.Result = dic["completedmessage"];
                } else {
                    e.Result = "Success";
                }
            } catch (Exception ex) {
                e.Result = ex;
            }

        }


        private void mnuToolsGenerateAppResourceData_Click(object sender, EventArgs e) {
            if (!Debugger.IsAttached) {
                MessageBox.Show(this, "This is useful only during debugging and should never be called outside the context of a debugger.", "Debugger Not Attached, Nothing To Do");
            } else {

                var langs = new List<LangInfo>();

                var dtLang = GetLanguageTable(true);
                foreach (DataRow drLang in dtLang.Rows) {
                    var li = new LangInfo { ID = Toolkit.ToInt32(drLang["sys_lang_id"], -1), Name = drLang["title"].ToString(), Text = "" };

                    if (li.Name.ToLower().Contains("esp")) {
                        li.DatabaseName = "spanish";
                    } else if (li.Name.ToLower().Contains("fra")) {
                        li.DatabaseName = "french";
                    } else if (li.Name.ToLower().Contains("rus") || li.Name.StartsWith("Рус")) {
                        li.DatabaseName = "russian";
                    } else if (li.Name.ToLower().Contains("por")) {
                        li.DatabaseName = "portuguese";
                    } else if (li.Name.ToLower().Contains("esk")){
                        li.DatabaseName = "czech";
                    } else if (li.Name.ToLower().Contains("english")){
                        li.DatabaseName = "english";
                    } else {
                        li.DatabaseName = "arabic";
                    }

                    langs.Add(li);
                }

                var fis = new List<FormInfo>();

                // fill info from the main form...
                var mi = new FormInfo { AppName = "AdminTool", FormName = this.Name, FormText = this.Text };
                var subcomps = frmBase.ListComponents(this.components, null, this);
                frmBase.FillComponentInfo(mi, subcomps, langs);
                fis.Add(mi);


                // reflect on all forms at the GrinGlobal.Admin namespace and below.
                // for each derivative of frmBase found, create a row for each resource in it
                var asm = Assembly.GetExecutingAssembly();
                foreach (Type type in asm.GetTypes()) {
                    if (type.IsSubclassOf(typeof(frmBase))) {
                        var f = (frmBase)Activator.CreateInstance(type);
                        var fi = new FormInfo { AppName = "AdminTool", FormName = f.Name, FormText = f.Text };
                        if (fi.FormName != fi.FormText) {
                            // add the form, and inspect it to fill the information properly
                            fis.Add(fi);

                            //var mis = type.GetMembers();
                            //foreach (MemberInfo mi in mis) {
                                
                            //}
                            f.Show();
                            f.fillFormInfo(fi, langs);
                            f.Hide();

                            // fillFormInfo(fi, langs, f, f.Container);
                        }
                    }
                }


                // fis now has all the language info from all forms in this project.
                var sb = new StringBuilder();
                // file format:
                // appname
                // form name
                // resource name
                // offset (for combobox entries)
                // English text
                // English tool tip
                // lang id
                var offset = 0;
                sb.Append("app_name\tform_name\tapp_resource_name\tsort_order");
                for(var i=0;i<langs.Count;i++){
                    var li = langs[i];
                    sb.Append("\t" + li.DatabaseName + "_text");
                    sb.Append("\t" + li.DatabaseName + "_description");
                    if (li.DatabaseName.ToLower() == "english") {
                        offset = i;
                    }
                }
                sb.AppendLine();

                foreach (FormInfo fi in fis) {
                    var s = fi.ToString(offset);
                    if (!String.IsNullOrEmpty(s)) {
                        sb.Append(s);
                    }
                }

                var fn = @"C:\temp\test.xls";
                File.WriteAllText(fn, sb.ToString());
                Process.Start(fn);
            }
        }

        //private void fillFormInfo(FormInfo fi, List<LangInfo> langs, Form f, ToolStripMenuItem tsmi) {
        //    foreach (ToolStripItem tsi in tsmi.DropDown.Items) {
        //        if (tsi is ToolStripMenuItem) {
        //            if (tsi.Text.Length > 0) {
        //                var newLangs = LangInfo.Clone(langs);
        //                foreach (LangInfo li in newLangs) {
        //                    if (li.Name == "English") {
        //                        li.Text = tsi.Text;
        //                        li.TooltipText = tsi.Text;
        //                    }
        //                }
        //                fi.Components.Add(new ComponentInfo { Name = tsi.Name, Languages = newLangs });
        //            }
        //            fillFormInfo(fi, langs, f, (ToolStripMenuItem)tsi);
        //        } else if (tsi is ToolStripSeparator){
        //        } else {
        //            throw new InvalidOperationException("no mapping defined in fillFormInfo() for " + tsi.GetType());
        //        }
        //    }
        //}

        //private void fillFormInfo(FormInfo fi, List<LangInfo> langs, Form f, MenuItem mi) {
        //    foreach (MenuItem smi in mi.MenuItems) {
        //        if (smi.Text.Length > 0) {
        //            var newLangs = LangInfo.Clone(langs);
        //            foreach (LangInfo li in newLangs) {
        //                if (li.Name == "English") {
        //                    li.Text = smi.Text;
        //                    li.TooltipText = smi.Text;
        //                }
        //            }
        //            fi.Components.Add(new ComponentInfo { Name = smi.Name, Languages = newLangs });
        //        }
        //        fillFormInfo(fi, langs, f, smi);
        //    }
        //}

        private void fillFormInfo(FormInfo fi, List<LangInfo> langs, Form f, IContainer container) {
            foreach (Component c in container.Components) {
                if (c is TextBox || c is Label || c is GroupBox || c is RadioButton || c is CheckBox || c is Button) {
                    var newLangs = LangInfo.Clone(langs);
                    foreach (LangInfo li in newLangs) {
                        if (li.Name == "English") {
                            li.Text = ((Control)c).Text;
                            li.TooltipText = ((Control)c).Text;
                        }
                    }
                    fi.Components.Add(new ComponentInfo { Name = ((Control)c).Name, Languages = newLangs });
                } else if (c is ContextMenuStrip){
                    var c2 = (ContextMenuStrip)c;
                    foreach (MenuItem mi in c2.Items) {
                        var newLangs = LangInfo.Clone(langs);
                        foreach (LangInfo li in newLangs) {
                            if (li.Name == "English") {
                                li.Text = c2.Text;
                                li.TooltipText = c2.Text;
                            }
                        }
                        fi.Components.Add(new ComponentInfo { Name = c2.Name, Languages = newLangs });
                        fillFormInfo(fi, langs, f, mi.Container);
                        // fillFormInfo(fi, langs, f, tsi.);
                    }

                } else if (c is MenuStrip) {
                    var c2 = (MenuStrip)c;
                    foreach (ToolStripMenuItem tsmi in c2.Items) {
                        var newLangs = LangInfo.Clone(langs);
                        foreach (LangInfo li in newLangs) {
                            if (li.Name == "English") {
                                li.Text = tsmi.Text;
                                li.TooltipText = tsmi.Text;
                            }
                        }
                        fi.Components.Add(new ComponentInfo { Name = tsmi.Name, Languages = newLangs });

                        fillFormInfo(fi, langs, f, tsmi.Container);
                        // fillFormInfo(fi, langs, f, tsi.);
                    }
                } else if (c is StatusStrip) {
                    //var c2 = (StatusStrip)c;
                    //foreach (Strip tsi in c2.Items) {
                    //    var newLangs = LangInfo.Clone(langs);
                    //    foreach (LangInfo li in newLangs) {
                    //        if (li.Name == "English") {
                    //            li.Text = c2.Text;
                    //            li.TooltipText = c2.Text;
                    //        }
                    //    }
                    //    fi.Controls.Add(new ControlInfo { Name = c2.Name, Languages = newLangs });
                    //    // fillFormInfo(fi, langs, f, tsi.);
                    //}

                } else if (c is ToolStrip) {
                    var c2 = (ToolStrip)c;
                    foreach (ToolStripItem tsi in c2.Items) {
                        var newLangs = LangInfo.Clone(langs);
                        foreach (LangInfo li in newLangs) {
                            if (li.Name == "English") {
                                li.Text = tsi.Text;
                                li.TooltipText = tsi.Text;
                            }
                        }
                        fi.Components.Add(new ComponentInfo { Name = tsi.Name, Languages = newLangs });
                        // fillFormInfo(fi, langs, f, tsi.);
                    }

                } else if (c is ComboBox) {
                    // add multiple rows for this control, one for each item (if any items exist)
                    var ddl = c as ComboBox;
                    if (ddl.Items.Count > 0) {
                        for (int i = 0; i < ddl.Items.Count; i++) {
                            var newLangs = LangInfo.Clone(langs);
                            foreach (LangInfo li in newLangs) {
                                if (li.Name == "English") {
                                    li.Text = ddl.Items[i].ToString();
                                    li.TooltipText = ddl.Items[i].ToString();
                                }
                            }

                            fi.Components.Add(new ComponentInfo { Name = ((Control)c).Name, Languages = newLangs });
                        }
                    }
                } else if (c is ListBox) {
                    // add multiple rows for this control, one for each item (if any items exist)
                    var lb = c as ListBox;
                    if (lb.Items.Count > 0) {
                        for (int i = 0; i < lb.Items.Count; i++) {
                            var newLangs = LangInfo.Clone(langs);
                            foreach (LangInfo li in newLangs) {
                                if (li.Name == "English") {
                                    li.Text = lb.Items[i].ToString();
                                    li.TooltipText = lb.Items[i].ToString();
                                }
                            }

                            fi.Components.Add(new ComponentInfo { Name = ((Control)c).Name, Languages = newLangs });
                        }
                    }
                } else if (c is ListView || c is DataGridView || c is SplitContainer || c is Splitter || c is Panel || c is TabControl || c is TreeView || c is TextBox || c is Label || c is HScrollBar || c is VScrollBar || c is GroupBox || c is ProgressBar || c is MdiClient) {
                    // nothing to do here, text was empty or this control simply has nothing language-specific to display.
                    // of course, we will still check the child controls though.
                } else {
                    throw new InvalidOperationException(getDisplayMember("fillFormInfo", "Type {0} not mapped in fillFormInfo.", c.GetType().ToString()));
                }

                fillFormInfo(fi, langs, f, c.Container);
            }
        }

        private void cmiDataTriggersNew_Click(object sender, EventArgs e) {
            PopupNewItemForm(new frmDataTrigger());

        }

        private void cmiDataTriggersRefresh_Click(object sender, EventArgs e) {
            RefreshData();

        }

        private void cmiDataTriggersProperties_Click(object sender, EventArgs e) {
            showChildForm(new frmDataTriggers(), tvItems.SelectedNode);

        }

        private void cmFilter_Opening(object sender, CancelEventArgs e) {

        }

        private void cmiDataTriggerRefresh_Click(object sender, EventArgs e) {
            RefreshData();

        }

        private void cmiDataTriggerProperties_Click(object sender, EventArgs e) {
            showChildForm(new frmDataTrigger(), tvItems.SelectedNode);

        }

        private void mnuToolsClearAdminCache_Click(object sender, EventArgs e) {
            if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("clearAdminCache{start_body}", "This will clear the internal cache used by the Admin Tool.\nThis may cause the Admin Tool to respond slowly for a short while, but will make sure the latest data is displayed.\nContinue?"), 
                getDisplayMember("clearAdminCache{start_title}","Clear Internal Cache?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                var proxy = GetAdminHostProxy();
                if (proxy != null) {
                    proxy.ClearCache(null);
                }
                _dtResource = null;
                _dtLang = null;
            }
        }

        private void mnuTools_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            mnuToolsClearAdminCache.Enabled = proxy != null;
        }

        private void cmiMaintenanceRefresh_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void cmiMaintenanceProperties_Click(object sender, EventArgs e) {
            showChildForm(new frmMaintenance(), tvItems.SelectedNode);
        }

        private void cmPermission_Opening(object sender, CancelEventArgs e) {

        }

        private void cmiPermissionEnable_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                using (new AutoCursor(this)) {
                    proxy.EnablePermission(Toolkit.ToInt32(CurrentNodeTag(), -1));
                    UpdateStatus(getDisplayMember("enablePermission{done}", "Permission {0} has been enabled.", CurrentNodeText("") ), true);
                }
            }

        }

        private void cmiDataviewCategoryProperties_Click(object sender, EventArgs e) {
            var f = new frmDataviews();
            f.CategoryName = tvItems.SelectedNode.Tag.ToString();
            showChildForm(f, tvItems.SelectedNode);
        }

        private void cmiDataviewCategoryRefresh_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void cmiDataviewCategoryVerifyAll_Click(object sender, EventArgs e) {
            verifyDescendentDataviews();
        }

        private void cmiDataviewCategoryExportAll_Click(object sender, EventArgs e) {
            exportDescendentDataviews();
        }

        private void cmiDataviewCategoryImport_Click(object sender, EventArgs e) {
            importDataview();
        }

        private void cmiTableMappingsDeleteOrphanedMappings_Click(object sender, EventArgs e) {
            using (new AutoCursor(this)) {
                UpdateStatus(getDisplayMember("deleteOrphanMappings{start}", "Deleting orphaned mappings..."), false);
                GetAdminHostProxy().DeleteOrphanedMappings();
                RefreshData();
                UpdateStatus(getDisplayMember("deleteOrphanMappings{done}", "Deleted orphaned mappings"), true);
            }


        }

        private void mnuToolsTest_Click(object sender, EventArgs e) {
            MessageBox.Show("My handle=" + this.Handle.ToString());
            //var p = GetAdminHostProxy();
            //var ds = p.GetData("get_accession", ":accessionid=382132;:inventoryid=;:cooperatorid=;:orderrequestid=;:cropid=;:taxonomygenusid=;:geographyid=;");
            //var dr = ds.Tables["get_accession"].Rows[0];
            //Debug.WriteLine(dr["accession_id"].ToString());
            //var ds2 = p.TransferOwnership(ds, 62243, true);
            //var ds2 = p.ResolveUniqueKeys(ds, null);

        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e) {
            var f = new frmTableMappingInspectSchema();

            f.TableNames.Add(CurrentNodeText(""));
            if (f.TableNames.Count > 0) {
                if (DialogResult.OK == PopupForm(f, this, false)) {
                }
            }

        }

        private void cmiDataTriggersImport_Click(object sender, EventArgs e) {
            if (DialogResult.OK == PopupForm(new frmImportDataTrigger(), this, false)) {
                RefreshData();
            }
        }

        private void cmiImportWizardRefresh_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void cmiImportWizardProperties_Click(object sender, EventArgs e) {
            showChildForm(new frmLaunchImportWizard(), tvItems.SelectedNode);
        }

        private void cmiCodeGroupProperties_Click(object sender, EventArgs e) {
            showChildForm(new frmCodeGroup(), tvItems.SelectedNode);
        }

        private void cmiCodeGroupRefresh_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void cmiCodeGroupsRefresh_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void cmiCodeGroupsProperties_Click(object sender, EventArgs e) {
            showChildForm(new frmCodeGroups(), tvItems.SelectedNode);
        }

        private void cmDataviews_Opening(object sender, CancelEventArgs e) {

        }

        private void cmiDataviewCategoryNew_Click(object sender, EventArgs e) {
            var fdv = new frmDataview();
            fdv.DefaultCategory = tvItems.SelectedNode.Text;
            PopupNewItemForm(fdv);

        }

        private void cmiSearchEngineRebuildAll_Click(object sender, EventArgs e) {
            var proxy = GetAdminHostProxy();
            if (proxy != null) {
                proxy.RebuildAllSearchEngineIndexes();
                UpdateStatus(getDisplayMember("searchEngineRebuild{started}", "Rebuilding all enabled search indexes..."), true);
            }
        }

        private void cmiCodeGroupNew_Click(object sender, EventArgs e) {
            PopupNewItemForm(new frmCodeGroup());
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "mdiParent", resourceName, null, defaultValue, substitutes);
        }
    }
}
