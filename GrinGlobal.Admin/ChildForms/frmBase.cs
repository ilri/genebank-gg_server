using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using GrinGlobal.Business;
using System.IO;
using System.Reflection;
using GrinGlobal.Interface.Dataviews;
using System.Diagnostics;
using GrinGlobal.Business.SqlParser;
using GrinGlobal.InstallHelper;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmBase : Form {
        public frmBase() {
            InitializeComponent();  tellBaseComponents(components);
            _id = -1;
            try {

                //var fis = new List<FormInfo>();
                //// fill info from the main form...
                //var mi = new FormInfo { AppName = "Admin Tool", FormName = this.Name, FormText = this.Text };
                //var subcomps = frmBase.ListComponents(this.components, null, this);
                //frmBase.FillComponentInfo(mi, subcomps, langs);
                //fis.Add(mi);

                var comps = ListComponents(components, _childComponents, this);
                GuiManager.LoadResources(MainFormResourceTable(false), "AdminTool", this, comps);
            } catch {
            }
        }

        private IContainer _childComponents;
        protected void tellBaseComponents(IContainer components) {
            _childComponents = components;

            // HACK: ListView doesn't fill in the Name property appropriately, so we do that here
            foreach (Control c in GuiManager.TraverseDescendentControls(this)){
                var lv = c as ListView;
                if (lv != null) {
                    fixColumnNames(lv);
                }

            }
        }

        internal void fillFormInfo(FormInfo fi, List<LangInfo> langs) {

            var comps = ListComponents(components, this._childComponents, this);
            FillComponentInfo(fi, comps, langs);
        }

        internal static List<Component> ListComponents(IContainer components, IContainer childComponents, Control ctl) {
            var comps = new List<Component>();

            if (components != null) {
                foreach (Component c in components.Components) {
                    comps.Add(c);
                }
            }

            if (childComponents != null) {
                foreach (Component c in childComponents.Components) {
                    comps.Add(c);
                }
            }

            if (ctl != null) {
                foreach (Control c in ctl.Controls) {
                    comps.Add(c);
                }
            }
            return comps;
        }

        internal static void FillComponentInfo(FormInfo fi, List<Component> comps, List<LangInfo> langs){
            foreach (Component c in comps) {
                if (c is SplitContainer) {
                    var sc = c as SplitContainer;

                    var subcomps = ListComponents(null, null, sc.Panel1);
                    FillComponentInfo(fi, subcomps, langs);

                    subcomps = ListComponents(null, null, sc.Panel2);
                    FillComponentInfo(fi, subcomps, langs);

                } else if (c is TextBox || c is Label || c is GroupBox || c is LinkLabel || c is Panel || c is RadioButton || c is CheckBox || c is Button || c is SplitterPanel) {
                    var newLangs = LangInfo.Clone(langs);
                    foreach (LangInfo li in newLangs) {
                        if (li.Name == "English") {
                            li.Text = ((Control)c).Text;
                            li.TooltipText = ((Control)c).Text;
                        }
                    }
                    fi.Components.Add(new ComponentInfo { Name = ((Control)c).Name, Languages = newLangs });

                    if (c is Panel || c is SplitterPanel || c is GroupBox) {
                        var subcomps = ListComponents(null, null, (Control)c);
                        FillComponentInfo(fi, subcomps, langs);
                    }

                } else if (c is ContextMenuStrip) {
                    var c2 = (ContextMenuStrip)c;
                    foreach (ToolStripItem tsmi in c2.Items) {
                        if (tsmi.Name.StartsWith("toolStripSeparator")) {
                            // skip separators, nothing to do for them
                        } else {
                            var newLangs = LangInfo.Clone(langs);
                            foreach (LangInfo li in newLangs) {
                                if (li.Name == "English") {
                                    li.Text = tsmi.Text;
                                    li.TooltipText = tsmi.Text;
                                }
                            }
                            fi.Components.Add(new ComponentInfo { Name = tsmi.Name, Languages = newLangs });
                            var subcomps = ListComponents(tsmi.Container, null, null);
                            FillComponentInfo(fi, subcomps, langs);
                        }
                    }

                    //} else if (c is MenuStrip) {
                    //    var c2 = (MenuStrip)c;
                    //    foreach (ToolStripMenuItem tsmi in c2.Items) {
                    //        var newLangs = LangInfo.Clone(langs);
                    //        foreach (LangInfo li in newLangs) {
                    //            if (li.Name == "English") {
                    //                li.Text = tsmi.Text;
                    //                li.TooltipText = tsmi.Text;
                    //            }
                    //        }
                    //        fi.Components.Add(new ComponentInfo { Name = tsmi.Name, Languages = newLangs });

                    //        //fillFormInfo(fi, langs, f, tsmi.Container);
                    //        // fillFormInfo(fi, langs, f, tsi.);
                    //    }
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

                    //} else if (c is ToolStrip) {
                    //    var c2 = (ToolStrip)c;
                    //    foreach (ToolStripItem tsi in c2.Items) {
                    //        var newLangs = LangInfo.Clone(langs);
                    //        foreach (LangInfo li in newLangs) {
                    //            if (li.Name == "English") {
                    //                li.Text = tsi.Text;
                    //                li.TooltipText = tsi.Text;
                    //            }
                    //        }
                    //        fi.Components.Add(new ComponentInfo { Name = tsi.Name, Languages = newLangs });
                    //        // fillFormInfo(fi, langs, f, tsi.);
                    //    }

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

                            fi.Components.Add(new ComponentInfo { Name = ((Control)c).Name, Languages = newLangs, Offset = i });
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

                            fi.Components.Add(new ComponentInfo { Name = ((Control)c).Name, Languages = newLangs, Offset = i });
                        }
                    }
                } else if (c is ListView) {

                    var lv = (ListView)c;

                    // HACK: ListView doesn't fill in the Name property appropriately, so we do that here
                    fixColumnNames(lv);

                    for (var i = 0; i < lv.Columns.Count; i++) {
                        var col = lv.Columns[i];
                        var colName = col.Name;
                        var newLangs = LangInfo.Clone(langs);
                        foreach (LangInfo li in newLangs) {
                            if (li.Name == "English") {
                                li.Text = col.Text;
                                li.TooltipText = ""; //col.To

                                if (colName == "") {
                                    colName = "col" + col.Text;
                                }
                            }
                        }
                        fi.Components.Add(new ComponentInfo { Name = colName, Languages = newLangs, Offset = i });
                    }

                } else if (c is DataGridView) {

                    var dgv = (DataGridView)c;
                    for (var i = 0; i < dgv.Columns.Count; i++) {
                        var col = dgv.Columns[i];
                        var newLangs = LangInfo.Clone(langs);
                        foreach (LangInfo li in newLangs) {
                            if (li.Name == "English") {
                                li.Text = col.HeaderText;
                                li.TooltipText = col.ToolTipText;
                            }
                        }
                        fi.Components.Add(new ComponentInfo { Name = col.Name, Languages = newLangs, Offset = i });
                    }
                } else if (c is TabControl) {

                    var tc = (TabControl)c;
                    foreach (TabPage tp in tc.TabPages) {
                        var newLangs = LangInfo.Clone(langs);
                        foreach (LangInfo li in newLangs) {
                            if (li.Name == "English") {
                                li.Text = tp.Text;
                                li.TooltipText = tp.ToolTipText;
                            }
                        }
                        fi.Components.Add(new ComponentInfo { Name = tp.Name, Languages = newLangs });

                        var subcomps = ListComponents(null, null, tp);
                        FillComponentInfo(fi, subcomps, langs);

                    }

                } else if (c is TabPage) {
                    Debug.WriteLine("todo3...");
                } else if (c is TreeView) {

                    // we go at most a depth of 2...
                    var tv = c as TreeView;
                    foreach (TreeNode nd in tv.Nodes) {

                        tv.SelectedNode = nd;
                        Application.DoEvents();

                        if (!nd.Name.Contains("_")) {
                            // nodes with "_" in name are computed values
                            fillTreeNodeInfo(fi, nd, langs);
                        }

                        if (nd.Nodes.Count > 0) {
                            // inspect only the first server's nodes
                            var nd2 = nd.Nodes[0];
                            tv.SelectedNode = nd2;
                            Application.DoEvents();

                            if (!nd2.Name.Contains("_")) {
                                // nodes with "_" in name are computed values
                                fillTreeNodeInfo(fi, nd2, langs);
                            }


                            foreach (TreeNode nd3 in nd2.Nodes) {
                                try {
                                    //tv.SelectedNode = nd3;
                                    //Application.DoEvents();

                                    if (!nd3.Name.Contains("_")) {
                                        // nodes with "_" in name are computed values
                                        fillTreeNodeInfo(fi, nd3, langs);
                                    }
                                } catch { }
                            }

                        }
                    }


                } else if (c is ToolStrip) {
                    var ts = c as ToolStrip;

                    foreach (ToolStripItem tsi in ts.Items) {
                        var newLangs = LangInfo.Clone(langs);
                        foreach (LangInfo li in newLangs) {
                            if (li.Name == "English") {
                                li.Text = tsi.Text;
                                li.TooltipText = tsi.Text;
                            }
                        }
                        fi.Components.Add(new ComponentInfo { Name = tsi.Name, Languages = newLangs });
                        var subcomps = ListComponents(tsi.Container, null, null);
                        FillComponentInfo(fi, subcomps, langs);
                    }

                } else if (c is SplitContainer
                    || c is ToolTip
                    || c is Splitter
                    || c is HScrollBar
                    || c is VScrollBar
                    || c is ProgressBar
                    || c is MdiClient
                    || c is ImageList
                    || c is Timer) {
                    // nothing to do here, text was empty or this control simply has nothing language-specific to display.
                    // of course, we will still check the child controls though.
                } else {
                    throw new InvalidOperationException(getDisplayMember("FillComponentInfo", "Type {0} not mapped in fillFormInfo.", c.GetType().ToString()));
                }

            }
        }

        /// Fixes column names. Requires all ColumnHeaders to have members (set GenerateMember to <c>true</c>). 
        /// </summary> 
        /// <param name="list">The ListView.</param> 
        private static void fixColumnNames(ListView list) {
            List<ColumnHeader> columns = new List<ColumnHeader>();
            foreach (ColumnHeader column in list.Columns) {
                if (string.IsNullOrEmpty(column.Name)) {
                    columns.Add(column);
                }
            }

            if (columns.Count == 0) {
                // no need to fix names 
                return;
            }

            Control parent = list.Parent;

            while (parent != null) {
                FieldInfo listInfo = parent.GetType().GetField(list.Name, BindingFlags.NonPublic | BindingFlags.Instance);

                if (listInfo != null) {
                    // found a member with given name, let's check if it points to the same object 
                    if (object.ReferenceEquals(listInfo.GetValue(parent), list)) {
                        // yes, this member points to our object 
                        break;
                    }
                }

                parent = parent.Parent;
            }

            if (parent != null) {
                foreach (ColumnHeader column in columns) {
                    FieldInfo columnInfo = null;

                    // for all fields 
                    foreach (FieldInfo field in parent.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)) {
                        // ...find the one pointing to column we're looking for 
                        if ((field.FieldType == column.GetType()) && (object.ReferenceEquals(field.GetValue(parent), column))) {
                            // same type, same value -> yes, this is the member we're looking for 
                            columnInfo = field;
                            break;
                        }
                    }

                    if (columnInfo != null) {
                        try {
                            column.Name = columnInfo.Name;
                        } catch {
                            // No idea why, but we don't want any exceptions... 
                        }
                    } else {
                        // no member found 
                    }
                }
            } else {
                // no parent found 
            }
        } 

        private static void fillTreeNodeInfo(FormInfo fi, TreeNode nd, List<LangInfo> langs) {
            var newLangs = LangInfo.Clone(langs);
            foreach (LangInfo li in newLangs) {
                if (li.Name == "English") {
                    li.Text = nd.Text;
                    li.TooltipText = nd.ToolTipText;
                }
            }
            fi.Components.Add(new ComponentInfo { Name = nd.Name, Languages = newLangs });
        }

        /// <summary>
        /// Initialized by the MdiParent.  Allows a form to call into the webservice/database with a single interface.
        /// </summary>
        public AdminHostProxy AdminProxy;

        /// <summary>
        /// DataTable containing data represented by this child form.  Not applicable to forms representing leaf nodes in the MdiParent treeview.
        /// </summary>
        public DataTable DataTable;

        /// <summary>
        /// ColumnName of the column in the DataTable which contains the primary key for a given row
        /// </summary>
        public string IDColumn;
        /// <summary>
        /// ColumnName of the column in the DataTable the MdiParent is to display in its treeview
        /// </summary>
        public string TextColumn;

        /// <summary>
        /// ColumnName of the column in the DataTable the MdiParent is to use to break up the list into subnodes in the MdiParent treeview
        /// </summary>
        public string CategoryColumn;

        private int _id;
        /// <summary>
        /// Gets or sets the ID for this form.
        /// </summary>
        public int ID {
            get {
                return _id;
            }
            set {
                _id = value;
            }
        }

        public void RefreshData(int id) {
            try {
                _id = id;
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(getDisplayMember("RefreshData{failed}", "Error refreshing form: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Called from the MdiParent whenever data needs to be refreshed.  Is called immediately after Show()-ing the form.
        /// </summary>
        public virtual void RefreshData() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows child form to remember which tags were selected before it refreshes.
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        protected List<object> rememberSelectedTags(ListView lv) {
            var ret = new List<object>();
            foreach (ListViewItem lvi in lv.SelectedItems) {
                ret.Add(lvi.Tag);
            }
            return ret;
        }

        protected void selectRememberedTags(ListView lv, List<object> tags) {
            var first = -1;
            for(var i=0;i<lv.Items.Count;i++){
                var lvi = lv.Items[i];
                if (tags.Contains(lvi.Tag)) {
                    lvi.Selected = true;
                    if (first == -1) {
                        first = i;
                    }
                }
            }

            if (first > -1) {
                lv.EnsureVisible(first);
            }
        }

        /// <summary>
        /// Parameters specify values MdiParent needs to know about to update its GUI and create proper links into child form.
        /// </summary>
        /// <param name="dataTable">DataTable containing data represented by this child form.  Not applicable to forms representing leaf nodes in the MdiParent treeview.</param>
        /// <param name="textColumn">ColumnName of the column in the DataTable the MdiParent is to display in its treeview</param>
        /// <param name="idColumn">ColumnName of the column in the DataTable which contains the primary key for a given row</param>
        protected void initHooksForMdiParent(DataTable dataTable, string textColumn, string idColumn) {
            initHooksForMdiParent(dataTable, textColumn, idColumn, null);
        }

        /// <summary>
        /// Parameters specify values MdiParent needs to know about to update its GUI and create proper links into child form.
        /// </summary>
        /// <param name="dataTable">DataTable containing data represented by this child form.  Not applicable to forms representing leaf nodes in the MdiParent treeview.</param>
        /// <param name="textColumn">ColumnName of the column in the DataTable the MdiParent is to display in its treeview</param>
        /// <param name="idColumn">ColumnName of the column in the DataTable which contains the primary key for a given row</param>
        /// <param name="categoryColumn">ColumnName of the column in the DataTable which the MdiParent uses to break up the list into sub-nodes.  i.e. think listing dataviews by category.</param>
        protected void initHooksForMdiParent(DataTable dataTable, string textColumn, string idColumn, string categoryColumn) {
            DataTable = dataTable;
            TextColumn = textColumn;
            IDColumn = idColumn;
            CategoryColumn = categoryColumn;
        }

        protected bool MainFormIsOracleConnection() {
            return mdiParent.Current.IsOracleConnection();
        }

        protected void MainFormRefreshData() {
            mdiParent.Current.RefreshData();
        }

        protected void MainFormUpdateStatus(string text, bool flash) {
            mdiParent.Current.UpdateStatus(text, flash);
        }

        protected void MainFormNotifyIndexCompleted() {
            mdiParent.Current.NotifyWhenIndexCompleted();
        }

        protected void MainFormReportError(string text) {
            mdiParent.Current.ReportError(text);
        }

        protected string MainFormCurrentNodeText(string defaultValue) {
            return mdiParent.Current.CurrentNodeText(defaultValue);
        }

        protected TreeNode MainFormCurrentNode() {
            return mdiParent.Current.CurrentNode();
        }

        protected void MainFormSelectPreviousNode() {
            mdiParent.Current.SelectPreviousTreeViewNode();
        }

        protected void MainFormSaveConnections(ConnectionInfo ci) {
            mdiParent.Current.SaveSettings(ci);
        }

        protected void MainFormSelectParentTreeNode() {
            mdiParent.Current.SelectParentTreeViewNode();
        }

        protected void MainFormSelectChildTreeNode(string tagValue) {
            mdiParent.Current.SelectChildTreeViewNode(tagValue);
        }

        protected void MainFormSelectDescendentTreeNode(string tagValue) {
            mdiParent.Current.SelectDescendentTreeViewNode(tagValue);
        }

        protected void MainFormSelectDescendentTreeNode(string tagValue, TreeNode fromNode) {
            mdiParent.Current.SelectDescendentTreeViewNode(tagValue, fromNode);
        }

        protected bool MainFormIsCurrentNodeDescendentOf(string tagValue) {
            return mdiParent.Current.IsCurrentNodeDescendentOf(tagValue);
        }

        protected void MainFormSelectConnection(ConnectionInfo conn) {
            mdiParent.Current.SelectConnection(conn);
        }

        protected void MainFormPopupNewItemForm(frmBase form) {
            mdiParent.Current.PopupNewItemForm(form);
        }

        protected DialogResult MainFormPopupForm(frmBase form, Form owner, bool resetID) {
            return mdiParent.Current.PopupForm(form, owner, resetID);
        }

        protected void MainFormSelectCousinTreeNode(string parentNodeName, string childTag) {
            mdiParent.Current.SelectCousinTreeNode(parentNodeName, childTag);
        }

        protected void MainFormSelectSiblingTreeNode(string siblingTag) {
            mdiParent.Current.SelectSiblingTreeNode(siblingTag);
        }

        protected TreeNode MainFormGetUncleTreeNode(string parentNodeName) {
            return mdiParent.Current.GetUncleTreeNode(parentNodeName);
        }

        protected TreeNode MainFormGetAncestorTreeNode(string ancestorNodeName) {
            return mdiParent.Current.GetAncestorTreeNode(ancestorNodeName);
        }

        protected DataTable MainFormResourceTable(bool forceRefresh) {
            return mdiParent.Current.GetResourceTable(forceRefresh);
        }

        protected void addPermItem(ListViewItem lvi, string value) {
            var subItem = lvi.SubItems.Add(value);
            if (value.StartsWith("A")) {
            } else if (value.StartsWith("D")) {
                subItem.ForeColor = Color.Red;
            } else if (value.StartsWith("I")) {
                subItem.ForeColor = Color.Gray;
            }
        }

        protected void MakeListViewSortable(ListView lv) {
            lv.EnableColumnSorting();
            //lv.ListViewItemSorter = new ListViewColumnSorter();
            //lv.ColumnClick += new ColumnClickEventHandler(lv_ColumnClick);
            //lv.KeyUp += new KeyEventHandler(lv_KeyUp);
        }

        //void lv_KeyUp(object sender, KeyEventArgs e) {
        //    if ((e.Modifiers & Keys.Control) == Keys.Control && e.KeyCode == Keys.A) {
        //        var lv = (ListView)sender;
        //        foreach (ListViewItem lvi in lv.Items) {
        //            lvi.Selected = true;
        //        }
        //    }
        //}

        //void lv_ColumnClick(object sender, ColumnClickEventArgs e) {
        //    var lvcs = ((ListView)sender).ListViewItemSorter as ListViewColumnSorter;
        //    // Determine if clicked column is already the column that is being sorted.
        //    if (e.Column == lvcs.SortColumn) {
        //        // Reverse the current sort direction for this column.
        //        if (lvcs.Order == SortOrder.Ascending) {
        //            lvcs.Order = SortOrder.Descending;
        //        } else {
        //            lvcs.Order = SortOrder.Ascending;
        //        }
        //    } else {
        //        // Set the column number that is to be sorted; default to ascending.
        //        lvcs.SortColumn = e.Column;
        //        lvcs.Order = SortOrder.Ascending;
        //    }

        //    // Perform the sort with these new sort options.
        //    ((ListView)sender).Sort();
        //}

        private TabControl _primaryTabControl;
        protected TabControl PrimaryTabControl {
            get { return _primaryTabControl; }
            set { _primaryTabControl = value; }
        }

        private TabControl _secondaryTabControl;
        protected TabControl SecondaryTabControl {
            get { return _secondaryTabControl; }
            set { _secondaryTabControl = value; }
        }

        public void SyncTab(frmBase otherForm) {
            if (otherForm != null) {
                // don't sync form tabs that are from different forms
                if (otherForm.GetType() == this.GetType()) {
                    // don't sync tabs if there are none specified for syncing
                    if (otherForm.PrimaryTabControl != null && _primaryTabControl != null) {
                        _primaryTabControl.SelectedIndex = otherForm.PrimaryTabControl.SelectedIndex;
                    }

                    if (otherForm.SecondaryTabControl != null && _secondaryTabControl != null) {
                        _secondaryTabControl.SelectedIndex = otherForm.SecondaryTabControl.SelectedIndex;
                    }
                }
            }
        }

        private bool _syncing;
        protected bool IsSyncing {
            get {
                return _syncing;
            }
        }
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


        protected void initAssemblyDropDown(ComboBox ddl, string folderPath, string defaultText, Type[] mustImplementTheseTypes) {

            ddl.Items.Clear();
            if (!String.IsNullOrEmpty(defaultText)){
                ddl.Items.Add(defaultText);
            }
            if (String.IsNullOrEmpty(folderPath)){
                return;
            }

            var folder = Toolkit.ResolveDirectoryPath(folderPath, false);
            if (Directory.Exists(folder)) {

                var files = Directory.GetFiles(folder, "*.dll", SearchOption.TopDirectoryOnly);
                if (mustImplementTheseTypes == null || mustImplementTheseTypes.Length == 0) {
                    foreach (var f in files) {
                        ddl.Items.Add(Path.GetFileName(f));
                    }
                } else {
                    foreach (var f in files) {
                        // we explicitly exclude the business dll as it should never be configurable.
                        if (!f.ToLower().EndsWith("gringlobal.business.dll")) {
                            var asm = Assembly.LoadFrom(f);
                            try {
                                var types = asm.GetTypes();
                                var found = false;
                                foreach (var t in types) {
                                    var ifs = t.GetInterfaces();
                                    foreach (var i in ifs) {
                                        foreach (var mii in mustImplementTheseTypes) {
                                            if (mii == i) {
                                                found = true;
                                                break;
                                            }
                                        }
                                        if (found) {
                                            break;
                                        }
                                    }
                                    if (found) {
                                        break;
                                    }
                                }
                                if (found) {
                                    ddl.Items.Add(Path.GetFileName(f));
                                }
                            } catch (ReflectionTypeLoadException rtle) {
                                // bad dll, ignore.
                                Debug.WriteLine("Could not load dll: " + rtle.Message);
                            }
                        }
                    }
                }
            }


        }

        protected int getDataviewID(string dataviewName) {
            var ds = AdminProxy.GetDataViewDefinition(dataviewName);
            var dt = ds.Tables["sys_dataview"];
            if (dt.Rows.Count > 0) {
                return Toolkit.ToInt32(dt.Rows[0]["sys_dataview_id"], -1);
            } else {
                return -1;
            }
        }

        protected int getTableID(string tableName) {
            if (tableName.Contains(" (")) {
                tableName = Toolkit.Cut(tableName, 0, tableName.IndexOf(" ("));
            }
            var ds = AdminProxy.ListTables(tableName);
            var dt = ds.Tables["list_tables"];
            if (dt.Rows.Count > 0) {
                return Toolkit.ToInt32(dt.Rows[0]["sys_table_id"], -1);
            } else {
                return -1;
            }
        }

        protected void initDataViewDropDown(ComboBox ddl, bool valueMemberIsID, string defaultText, int showOnlyThisDataViewID) {
            initDataViewDropDown(ddl, valueMemberIsID, defaultText, showOnlyThisDataViewID, null, null);
        }

        protected void initDataViewDropDown(ComboBox ddl, bool valueMemberIsID, string defaultText, int showOnlyThisDataViewID, string categoryName, string databaseArea) {

            if (valueMemberIsID) {
                ddl.ValueMember = "sys_dataview_id";
            } else {
                ddl.ValueMember = "dataview_name";
            }
            ddl.DisplayMember = "dataview_name";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["dataview_list_dataviews" + showOnlyThisDataViewID + categoryName + "-" + databaseArea] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListDataViews(showOnlyThisDataViewID, categoryName, false, databaseArea, false).Tables["list_dataviews"];
                if (!String.IsNullOrEmpty(defaultText)) {
                    var drView = dt.NewRow();
                    if (valueMemberIsID) {
                        drView[ddl.ValueMember] = -1;
                    } else {
                        drView[ddl.ValueMember] = defaultText;
                    }
                    drView[ddl.DisplayMember] = defaultText;
                    dt.Rows.InsertAt(drView, 0);
                }

                cm["dataview_list_dataviews" + showOnlyThisDataViewID + categoryName + "-" + databaseArea] = dt;
            }
            ddl.DataSource = dt;

        }

        protected void initDataViewDropDown(DataGridViewComboBoxColumn col, bool valueMemberIsID, string defaultText, int showOnlyThisDataViewID) {

            if (valueMemberIsID) {
                col.ValueMember = "sys_dataview_id";
            } else {
                col.ValueMember = "dataview_name";
            }
            col.DisplayMember = "dataview_name";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["dataview_list_dataviews" + showOnlyThisDataViewID] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListDataViews(showOnlyThisDataViewID, null, false, null, false).Tables["list_dataviews"];
                if (!String.IsNullOrEmpty(defaultText)) {
                    var drView = dt.NewRow();
                    if (valueMemberIsID) {
                        drView[col.ValueMember] = -1;
                    } else {
                        drView[col.ValueMember] = defaultText;
                    }
                    drView[col.DisplayMember] = defaultText;
                    dt.Rows.InsertAt(drView, 0);
                }

                cm["dataview_list_dataviews" + showOnlyThisDataViewID] = dt;
            }

            col.DataSource = dt;

        }

        protected void initDataViewLookupDropDown(DataGridViewComboBoxColumn col, string defaultText) {

            col.ValueMember = "dataview_name";
            col.DisplayMember = "dataview_name";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["dataview_list_lookup_dataviews"] as DataTable;
            if (dt == null) {
                //try {
                //    dt = AdminProxy.GetDataViewDefinition("get_lookup_table_list").Tables["sys_dataview"];
                //} catch {
                //    dt = null;
                //}
                if (dt == null || dt.Rows.Count == 0) {
                    // dataview lookup definition does not exist or there was an error retrieving it.
                    // just call the ListDataViews method and rename the first two fields.
                    dt = AdminProxy.ListLookupDataViews().Tables["list_lookup_dataviews"];
                } else {
                    dt = AdminProxy.GetData("get_lookup_table_list", null).Tables["get_lookup_table_list"];
                }
                dt.DefaultView.Sort = "dataview_name asc";
                if (!String.IsNullOrEmpty(defaultText)) {
                    var drView = dt.NewRow();
                    drView[col.ValueMember] = defaultText;
                    drView[col.DisplayMember] = defaultText;
                    dt.Rows.InsertAt(drView, 0);
                }
                cm["dataview_list_lookup_dataviews"] = dt;
            }

            col.DataSource = dt;

        }

        protected void initDataViewFieldDropDown(ComboBox ddl, int dataviewID, bool valueMemberIsID, string defaultText) {

            if (valueMemberIsID) {
                ddl.ValueMember = "sys_dataview_field_id";
            } else {
                ddl.ValueMember = "dataview_field_name";
            }
            ddl.DisplayMember = "dataview_field_name";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["dataview_list_dataview_fields" + dataviewID] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListDataViewFields(dataviewID, -1).Tables["list_dataview_fields"];

                var drView = dt.NewRow();
                if (valueMemberIsID) {
                    drView[ddl.ValueMember] = -1;
                } else {
                    drView[ddl.ValueMember] = defaultText;
                }
                drView[ddl.DisplayMember] = defaultText;
                dt.Rows.InsertAt(drView, 0);

                cm["dataview_list_dataview_fields" + dataviewID] = dt;
            
            }


            ddl.DataSource = dt;

        }


        protected void initDataViewCategoryDropDown(ComboBox cbo) {

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["dataview_list_dataview_categories"] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListDataViewCategories().Tables["list_dataview_categories"];
                cm["dataview_list_dataview_categories"] = dt;
            }

            cbo.Items.Clear();
            foreach (DataRow dr in dt.Rows) {
                cbo.Items.Add(dr["category_code"].ToString());
            }
        }

        protected void initDataViewDatabaseAreaDropDown(ComboBox cbo) {

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["dataview_list_dataview_database_areas"] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListDataViewDatabaseAreas().Tables["list_dataview_database_areas"];
                cm["dataview_list_dataview_database_areas"] = dt;
            }

            cbo.Items.Clear();
            foreach (DataRow dr in dt.Rows) {
                cbo.Items.Add(dr["database_area_code"].ToString());
            }
        }

        protected void initDatabaseDropDown(ComboBox cbo) {

            var dt = new DataTable();
            dt.Columns.Add("value_member", typeof(string));
            dt.Columns.Add("display_member", typeof(string));

            var pcodes = Utility.ListProviderCodes();
            var pnames = Utility.ListProviderNames();
            for (var i = 0; i < pcodes.Length; i++) {
                dt.Rows.Add(pcodes[i], pnames[i]);
            }

            cbo.ValueMember = "value_member";
            cbo.DisplayMember = "display_member";
            cbo.DataSource = dt;

        }

        protected void initTableDropDown(ComboBox ddl, bool valueMemberIsID, string defaultText, int fromDataviewID, int showOnlyThisTableID) {

            if (valueMemberIsID) {
                ddl.ValueMember = "sys_table_id";
            } else {
                ddl.ValueMember = "table_name";
            }
            ddl.DisplayMember = "table_name";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["table_list_tables_by_dataview" + showOnlyThisTableID] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListTables(fromDataviewID, showOnlyThisTableID).Tables["list_tables"];
                if (!String.IsNullOrEmpty(defaultText)) {
                    var drView = dt.NewRow();
                    if (valueMemberIsID) {
                        drView[ddl.ValueMember] = -1;
                    } else {
                        drView[ddl.ValueMember] = defaultText;
                    }
                    drView[ddl.DisplayMember] = defaultText;
                    dt.Rows.InsertAt(drView, 0);
                }
                cm["table_list_tables_by_dataview" + showOnlyThisTableID] = dt;
            }

            ddl.DataSource = dt;

        }


        protected void initTableDropDown(ComboBox ddl, bool valueMemberIsID, string defaultText, int showOnlyThisTableID) {

            if (valueMemberIsID) {
                ddl.ValueMember = "sys_table_id";
            } else {
                ddl.ValueMember = "table_name";
            }
            ddl.DisplayMember = "table_name";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["table_list_tables" + showOnlyThisTableID] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListTables(showOnlyThisTableID).Tables["list_tables"];
                if (!String.IsNullOrEmpty(defaultText)) {
                    var drView = dt.NewRow();
                    if (valueMemberIsID) {
                        drView[ddl.ValueMember] = -1;
                    } else {
                        drView[ddl.ValueMember] = defaultText;
                    }
                    drView[ddl.DisplayMember] = defaultText;
                    dt.Rows.InsertAt(drView, 0);
                }
                cm["table_list_tables" + showOnlyThisTableID] = dt;
            }

            ddl.DataSource = dt;

        }

        protected void initTableDropDown(DataGridViewComboBoxColumn col, bool valueMemberIsID, string defaultText, int showOnlyThisTableID) {

            if (valueMemberIsID) {
                col.ValueMember = "sys_table_id";
            } else {
                col.ValueMember = "table_name";
            }
            col.DisplayMember = "table_name";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["table_list_tables" + showOnlyThisTableID] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListTables(showOnlyThisTableID).Tables["list_tables"];
                if (!String.IsNullOrEmpty(defaultText)) {
                    var drView = dt.NewRow();
                    if (valueMemberIsID) {
                        drView[col.ValueMember] = -1;
                    } else {
                        drView[col.ValueMember] = defaultText;
                    }
                    drView[col.DisplayMember] = defaultText;
                    dt.Rows.InsertAt(drView, 0);
                }
                cm["table_list_tables" + showOnlyThisTableID] = dt;
            }

            col.DataSource = dt;

        }

        protected void initGuiHintDropdown(DataGridViewComboBoxCell cell, string selectedValue) {

            var dt = new DataTable();
            dt.Columns.Add("value_member", typeof(string));
            dt.Columns.Add("display_member", typeof(string));
            dt.Rows.Add("", "(None)");
            dt.Rows.Add("TOGGLE_CONTROL", "Checkbox");
            dt.Rows.Add("DATE_CONTROL", "Date / Time Control");
            dt.Rows.Add("TEXT_CONTROL", "Textbox (free-form)");
            dt.Rows.Add("LARGE_SINGLE_SELECT_CONTROL", "Lookup Picker");
            dt.Rows.Add("SMALL_SINGLE_SELECT_CONTROL", "Drop Down");
            dt.Rows.Add("INTEGER_CONTROL", "Textbox (integers only)");
            dt.Rows.Add("DECIMAL_CONTROL", "Textbox (decimals only)");

            cell.DataSource = dt;

            cell.ValueMember = "value_member";
            cell.DisplayMember = "display_member";


            cell.Value = selectedValue;

        }


        protected void initTableDropDown(DataGridViewComboBoxColumn col, bool valueMemberIsID, string defaultText, List<ITable> tables) {

            if (valueMemberIsID) {
                col.ValueMember = "sys_table_id";
            } else {
                col.ValueMember = "table_name";
            }
            col.DisplayMember = "table_name";

            // just get the DataTable definition from the db..
            var dt = AdminProxy.ListTables(int.MaxValue).Tables["list_tables"];

            // then fill it with our data
            if (!String.IsNullOrEmpty(defaultText)) {
                var drView = dt.NewRow();
                if (valueMemberIsID) {
                    drView[col.ValueMember] = -1;
                } else {
                    drView[col.ValueMember] = defaultText;
                }
                drView[col.DisplayMember] = defaultText;
                dt.Rows.InsertAt(drView, 0);
            }

            var names = new List<string>();
            foreach (var t in tables) {
                if (!names.Contains(t.TableName + " (" + t.AliasName + ")")) {
                    dt.Rows.Add(t.TableID, t.TableName + " (" + t.AliasName + ")");
                    names.Add(t.TableName + " (" + t.AliasName + ")");
                }
            }

            col.DataSource = dt;

        }

        protected DataTable initTableFieldDropDown(ComboBox ddl, int tableID, bool valueMemberIsID, string defaultText) {

            if (valueMemberIsID) {
                ddl.ValueMember = "sys_table_field_id";
            } else {
                ddl.ValueMember = "field_name";
            }
            ddl.DisplayMember = "field_name";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["table_list_table_fields" + tableID] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListTableFields(tableID, -1, false).Tables["list_table_fields"];
                var drView = dt.NewRow();
                if (valueMemberIsID) {
                    drView[ddl.ValueMember] = -1;
                } else {
                    drView[ddl.ValueMember] = defaultText;
                }
                drView[ddl.DisplayMember] = defaultText;
                dt.Rows.InsertAt(drView, 0);

                cm["table_list_table_fields" + tableID] = dt;
            }

            ddl.DataSource = dt;

            return dt;
        }


        protected void selectItemByValue(ComboBox ddl, int value) {
            var dt = ddl.DataSource as DataTable;
            for (var i = 0; i < dt.Rows.Count; i++) {
                var dr = dt.Rows[i];
                if (value == Toolkit.ToInt32(dr[ddl.ValueMember], -1)) {
                    ddl.SelectedIndex = i;
                    return;
                }
            }
            ddl.SelectedIndex = 0;
        }


        protected DataTable initTableFieldDropDown(ComboBox ddl, int tableID, bool valueMemberIsID, string defaultText, bool onlyPrimaryKeyFields, bool onlyForiegnKeyFields) {

            if (valueMemberIsID) {
                ddl.ValueMember = "sys_table_field_id";
            } else {
                ddl.ValueMember = "field_name";
            }
            ddl.DisplayMember = "field_name";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            var cacheKey = "table_list_table_fields" + (onlyPrimaryKeyFields ? "PK" : "") + (onlyForiegnKeyFields ? "FK" : "") + tableID;
            DataTable dt = cm[cacheKey] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListTableFields(tableID, -1, onlyPrimaryKeyFields, onlyForiegnKeyFields).Tables["list_table_fields"];
                var drView = dt.NewRow();
                if (valueMemberIsID) {
                    drView[ddl.ValueMember] = -1;
                } else {
                    drView[ddl.ValueMember] = defaultText;
                }
                drView[ddl.DisplayMember] = defaultText;
                dt.Rows.InsertAt(drView, 0);

                cm[cacheKey] = dt;
            }

            ddl.DataSource = dt;

            return dt;
        }



        protected DataTable initTableFieldDropDown(DataGridViewComboBoxCell cell, int tableID, bool valueMemberIsID, string defaultText) {

            if (valueMemberIsID) {
                cell.ValueMember = "sys_table_field_id";
            } else {
                cell.ValueMember = "field_name";
            }
            cell.DisplayMember = "field_name";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["table_list_table_fields" + tableID] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListTableFields(tableID, -1, false).Tables["list_table_fields"];
                var drView = dt.NewRow();
                if (valueMemberIsID) {
                    drView[cell.ValueMember] = -1;
                } else {
                    drView[cell.ValueMember] = defaultText;
                }
                drView[cell.DisplayMember] = defaultText;
                dt.Rows.InsertAt(drView, 0);

                cm["table_list_table_fields" + tableID] = dt;
            }

            cell.DataSource = dt;

            return dt;
        }

        protected DataTable initTableFieldDropDown(DataGridViewComboBoxColumn col, int tableID, bool valueMemberIsID, string defaultText) {

            if (valueMemberIsID) {
                col.ValueMember = "sys_table_field_id";
            } else {
                col.ValueMember = "field_name";
            }
            col.DisplayMember = "field_name";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["table_list_table_fields" + tableID] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListTableFields(tableID, -1, false).Tables["list_table_fields"];
                var drView = dt.NewRow();
                if (valueMemberIsID) {
                    drView[col.ValueMember] = -1;
                } else {
                    drView[col.ValueMember] = defaultText;
                }
                drView[col.DisplayMember] = defaultText;
                dt.Rows.InsertAt(drView, 0);

                cm["table_list_table_fields" + tableID] = dt;
            }

            col.DataSource = dt;

            return dt;
        }



        protected void initParentTableDropDown(ComboBox ddl, bool valueMemberIsID, string defaultText, int dataviewID, int tableID, int parentTableFieldID) {

            if (valueMemberIsID) {
                ddl.ValueMember = "sys_table_id";
            } else {
                ddl.ValueMember = "table_name";
            }
            ddl.DisplayMember = "table_name";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["table_list_related_tables" + dataviewID + "_" + tableID + "_" + parentTableFieldID] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListRelatedTables(dataviewID, tableID, parentTableFieldID, new string[]{"PARENT", "OWNER_PARENT"}).Tables["list_related_tables"];
                if (!String.IsNullOrEmpty(defaultText)) {
                    var drView = dt.NewRow();
                    if (valueMemberIsID) {
                        drView[ddl.ValueMember] = -1;
                    } else {
                        drView[ddl.ValueMember] = defaultText;
                    }
                    drView[ddl.DisplayMember] = defaultText;
                    dt.Rows.InsertAt(drView, 0);
                }
                cm["table_list_related_tables" + dataviewID + "_" + tableID + "_" + parentTableFieldID] = dt;
            }

            ddl.DataSource = dt;

        }

        protected void initSiteDropDown(ComboBox ddl, bool valueMemberIsID, string defaultText) {

            if (valueMemberIsID) {
                // ddl.ValueMember = "site_short_name";
                ddl.ValueMember = "site_id";
            } else {
                ddl.ValueMember = "display_text";
            }
            ddl.DisplayMember = "display_text";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["table_list_sites0"] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListSites(null).Tables["list_sites"];

                if (!String.IsNullOrEmpty(defaultText)) {
                    var drView = dt.NewRow();
                    if (valueMemberIsID) {
                        drView[ddl.ValueMember] = -1;
                    } else {
                        drView[ddl.ValueMember] = defaultText;
                    }
                    drView[ddl.DisplayMember] = defaultText;
                    dt.Rows.InsertAt(drView, 0);
                }
                cm["table_list_sites0"] = dt;
            }

            ddl.DataSource = dt;

        }

        protected DataTable initCodeValueDropDown(ComboBox ddl, string groupName, string defaultText) {

            ddl.ValueMember = "value_member";
            ddl.DisplayMember = "display_member";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["table_list_code_values_" + groupName] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListCodeValues(groupName, null, null).Tables["list_code_values"];

                if (!String.IsNullOrEmpty(defaultText)) {
                    var drView = dt.NewRow();
                    drView[ddl.ValueMember] = "";
                    drView[ddl.DisplayMember] = defaultText;
                    dt.Rows.InsertAt(drView, 0);
                }
                cm["table_list_code_values_" + groupName] = dt;
            }

            ddl.DataSource = dt;

            return dt;

        }


        protected DataTable initLanguageDropDown(ComboBox ddl, bool valueMemberIsID, string defaultText) {

            if (valueMemberIsID) {
                ddl.ValueMember = "sys_lang_id";
            } else {
                ddl.ValueMember = "title";
            }
            ddl.DisplayMember = "title";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["table_list_languages-1"] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListLanguages(-1).Tables["list_languages"];

                if (!String.IsNullOrEmpty(defaultText)) {
                    var drView = dt.NewRow();
                    if (valueMemberIsID) {
                        drView[ddl.ValueMember] = -1;
                    } else {
                        drView[ddl.ValueMember] = defaultText;
                    }
                    drView[ddl.DisplayMember] = defaultText;
                    dt.Rows.InsertAt(drView, 0);
                }
                cm["table_list_languages-1"] = dt;
            }

            ddl.DataSource = dt;

            return dt;

        }

        private bool sameTableName(ITable tbl, string tableName) {
            return String.Compare(tbl.AliasName, tableName, true) == 0
                || String.Compare(tbl.TableName, tableName, true) == 0
                || String.Compare(tbl.TableID.ToString(), tableName, true) == 0;
        }

        protected frmSplash _splash;
        protected void updateSplashText(string text) {
            if (_splash != null) {
                _splash.ChangeText(text);
            }
        }
        protected List<ITable> initTableFieldTreeView(TreeView tv, bool orderByDependencies) {

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);

            tv.ShowNodeToolTips = true;


            List<ITable> tables = cm["table_list_tables"] as List<ITable>;
            if (tables == null) {
                tables = new List<ITable>();
                updateSplashText("Listing all tables...");
                var dt = AdminProxy.ListTables(-1).Tables["list_tables"];
                foreach (DataRow dr in dt.Rows) {
                    var tableName = dr["table_name"].ToString();
                    updateSplashText("Loading definition for table '" + tableName + "' ...");
                    var fmt = AdminProxy.MapTable(tableName);
                    if (fmt != null) {
                        tables.Add(fmt);
                    }
                }
                cm["table_list_tables"] = tables;
            }

            tv.Nodes.Clear();

            var cacheRoot = cm["table_root_source_node-" + orderByDependencies] as TreeNode;
            if (cacheRoot != null)
            {
                foreach (TreeNode tn in cacheRoot.Nodes)
                {
                    tv.Nodes.Add(tn.Clone() as TreeNode);
                }
            }
            else
            {


                if (!orderByDependencies)
                {

                    updateSplashText("Filling Source Tables Name treeview...");

                    foreach (var t in tables)
                    {
                        var tableNode = new TreeNode(t.TableName, 0, 0);
                        tableNode.Name = "Table|" + t.TableName;
                        tableNode.Tag = t;
                        tableNode.ToolTipText = t.TableName + "\r\n" + (String.IsNullOrEmpty(t.UniqueKeyFields) ? "(no unique key)" : "unique key fields:\r\n" + t.UniqueKeyFields);
                        tv.Nodes.Add(tableNode);

                        foreach (var f in t.Mappings)
                        {
                            var fieldNode = new TreeNode(f.TableFieldName, 1, 1);
                            fieldNode.Name = "Field|" + f.TableFieldName;
                            fieldNode.Tag = f;
                            fieldNode.ToolTipText = f.FriendlyFieldName ?? f.TableFieldName;

                            if (!f.IsNullable)
                            {
                                // show as required
                                fieldNode.ImageIndex = fieldNode.SelectedImageIndex = 2;
                                fieldNode.ToolTipText += " (required)";
                            }

                            var maxChars = "";
                            if (f.DataTypeString == "STRING")
                            {
                                if (f.MaximumLength < 0)
                                {
                                    maxChars = " - unlimited characters";
                                }
                                else
                                {
                                    maxChars = " - " + f.MaximumLength + " characters";
                                }
                            }

                            fieldNode.ToolTipText += "\r\n" + f.DataTypeString + maxChars;

                            //var friendlyName = f.TableFieldName;
                            //if (f.DataviewFriendlyFieldNames.TryGetValue(AdminProxy.LanguageID, out friendlyName)) {
                            //    if (!String.IsNullOrEmpty(friendlyName)) {
                            //        fieldNode.ToolTipText = friendlyName;
                            //    }
                            //}
                            tableNode.Nodes.Add(fieldNode);
                        }
                    }
                }
                else
                {

                    // pull all relationships
                    List<Join> joins = cm["table_list_relationships"] as List<Join>;
                    if (joins == null)
                    {
                        joins = new List<Join>();
                        var dt = cm["table_list_table_relationships"] as DataTable;
                        if (dt == null)
                        {
                            updateSplashText("Determining table relationships...");
                            dt = AdminProxy.ListTableRelationships(-1, -1).Tables["list_table_relationships"];
                            cm["table_list_table_relationships"] = dt;
                        }
                        // now we need to keep a list of those that are roots (no parents) and those that are not.
                        // assume all are roots until proven otherwise.
                        var possibleRoots = new List<ITable>();
                        var children = new List<ITable>();

                        var auditFields = new string[] { "created_by", "modified_by", "owned_by" };
                        foreach (DataRow dr in dt.Rows)
                        {

                            var tableName = dr["table_name"].ToString();
                            var otherTableName = dr["table_name2"].ToString();
                            var relationship = dr["relationship_type_tag"].ToString();
                            var fromField = dr["field_name"].ToString();

                            // ignore audit fields
                            if (!auditFields.Contains(fromField.ToLower()))
                            {
                                // add each one to the nodes collection
                                var tbl = tables.Find(t => { return sameTableName(t, tableName); });
                                if (tbl != null)
                                {
                                    if (relationship.ToLower() == "self")
                                    {
                                        tbl.ExtendedProperties["self_referential"] = true;
                                    }
                                    else if (relationship.ToLower() == "parent"
                                        || relationship.ToLower() == "owner_parent")
                                    {

                                        object parents = null;
                                        if (!tbl.ExtendedProperties.TryGetValue("parents", out parents))
                                        {
                                            tbl.ExtendedProperties.Add("parents", new List<string> { otherTableName });
                                        }
                                        else
                                        {
                                            ((List<string>)parents).Add(otherTableName);
                                        }

                                        if (!children.Contains(tbl))
                                        {
                                            children.Add(tbl);
                                        }
                                    }
                                }


                                // add each one to the roots collection
                                tbl = tables.Find(t => { return sameTableName(t, otherTableName); });
                                if (tbl != null)
                                {
                                    if (!possibleRoots.Contains(tbl))
                                    {
                                        possibleRoots.Add(tbl);
                                    }
                                }
                            }
                        }

                        var forcedRoots = new string[] { "taxonomy_family", "accession", "crop" };

                        for (var i = 0; i < possibleRoots.Count; i++)
                        {
                            var root = possibleRoots[i];
                            if (children.Contains(root))
                            {
                                if (!forcedRoots.Contains(root.TableName.ToLower()))
                                {
                                    // this item is not a root because it exists in the children list
                                    // or we have explicitly said for it to be a root
                                    possibleRoots.RemoveAt(i);
                                    i--;
                                }
                            }
                        }

                        // we get here, possible roots consists of only those that actually are roots.
                        updateSplashText("Filling Source Tables Hierarchy treeview...");
                        foreach (var root in possibleRoots)
                        {
                            addChildren(tv, null, root, children);
                        }
                    }


                }

                cacheRoot = new TreeNode();
                foreach (TreeNode tn in tv.Nodes)
                {
                    cacheRoot.Nodes.Add(tn.Clone() as TreeNode);
                }

                cm["table_root_source_node-" + orderByDependencies] = cacheRoot;
            }

            return tables;

        }

        private void addChildren(TreeView tv, TreeNode parentNode, ITable table, List<ITable> allChildren){
            var tn = new TreeNode(table.TableName, 0, 0);
            tn.Name = "Table|" + table.TableName;
            tn.ToolTipText = table.TableName;
            tn.ToolTipText = table.TableName + "\r\n" + (String.IsNullOrEmpty(table.UniqueKeyFields) ? "(no unique key)" : "unique key fields:\r\n" + table.UniqueKeyFields);
            tn.Tag = table;
            //object selfReferential = null;
            //if (table.ExtendedProperties.TryGetValue("self_referential", out selfReferential)){
            //    if ((bool)selfReferential){
            //    }
            //}

            // add folder for fields
            var fldr = new TreeNode("<Fields>", 3, 3);
            fldr.Name = "Folder|" + table.TableName;
            fldr.ToolTipText = "Fields for " + table.TableName;
            fldr.Tag = table;
            tn.Nodes.Add(fldr);

            // add all fields
            foreach (var f in table.Mappings) {
                var fieldNode = new TreeNode(f.TableFieldName, 1, 1);

                fieldNode.Name = "Field|" + f.TableFieldName;
                fieldNode.Tag = f;
                fieldNode.ToolTipText = f.FriendlyFieldName ?? f.TableFieldName;

                if (!f.IsNullable) {
                    // show as required
                    fieldNode.ImageIndex = fieldNode.SelectedImageIndex = 2;
                    fieldNode.ToolTipText += " (required)";
                }


                var maxChars = "";
                if (f.DataTypeString == "STRING") {
                    if (f.MaximumLength < 0) {
                        maxChars = " - unlimited characters";
                    } else {
                        maxChars = " - " + f.MaximumLength + " characters";
                    }
                }

                fieldNode.ToolTipText += "\r\n" + f.DataTypeString + maxChars;

                //var friendlyName = f.TableFieldName;
                //if (f.DataviewFriendlyFieldNames.TryGetValue(AdminProxy.LanguageID, out friendlyName)) {
                //    if (!String.IsNullOrEmpty(friendlyName)) {
                //        fieldNode.ToolTipText = friendlyName;
                //    }
                //}
                fldr.Nodes.Add(fieldNode);
            }


            // add all child table(s)
            var currentChildren = allChildren.FindAll(n => {
                    var parents = n.ExtendedProperties["parents"] as List<string>;
                    foreach (var p in parents) {
                        if (p.ToLower() == table.TableName.ToLower()) {
                            return true;
                        }
                    }
                    return false;
                });

                if (parentNode == null) {
                    tv.Nodes.Add(tn);
                } else {
                    parentNode.Nodes.Add(tn);
                }
                foreach (var child in currentChildren) {
                    // first, make sure it's not in the parent chain already...
                    var alreadyAnAncestor = false;
                    var nd = parentNode;
                    while (nd != null) {
                        var pt = nd.Tag as ITable;
                        if (pt != null) {
                            if (pt.TableName.ToLower() == table.TableName.ToLower()) {
                                alreadyAnAncestor = true;
                                break;
                            }
                        }
                        nd = nd.Parent;
                    }



                    if (!alreadyAnAncestor) {
                        if (child.TableName.ToLower() != table.TableName.ToLower()) {
                            addChildren(tv, tn, child, allChildren);
                        }
                    }
                }

        }


        protected DataTable initCodeGroupDropDown(ComboBox ddl, string defaultText) {

            ddl.ValueMember = "group_name";
            ddl.DisplayMember = "group_name";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["table_list_code_groups0"] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListCodeGroups(null).Tables["list_code_groups"];

                if (!String.IsNullOrEmpty(defaultText)) {
                    var drView = dt.NewRow();
                    drView[ddl.ValueMember] = defaultText;
                    drView[ddl.DisplayMember] = defaultText;
                    dt.Rows.InsertAt(drView, 0);
                }
                cm["table_list_code_groups0"] = dt;
            }

            ddl.DataSource = dt;
            return dt;

        }

        protected void initCodeGroupDropDown(DataGridViewComboBoxColumn ddl, bool valueMemberIsID, string defaultText) {

            if (valueMemberIsID) {
                ddl.ValueMember = "group_name";
            } else {
                ddl.ValueMember = "group_name";
            }
            ddl.DisplayMember = "group_name";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["table_list_code_groups0"] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListCodeGroups(null).Tables["list_code_groups"];

                if (!String.IsNullOrEmpty(defaultText)) {
                    var drView = dt.NewRow();
                    if (valueMemberIsID) {
                        drView[ddl.ValueMember] = "";
                    } else {
                        drView[ddl.ValueMember] = defaultText;
                    }
                    drView[ddl.DisplayMember] = defaultText;
                    dt.Rows.InsertAt(drView, 0);
                }
                cm["table_list_code_groups0"] = dt;
            }

            ddl.DataSource = dt;

        }

        protected void initGeographyDropDown(ComboBox ddl, bool valueMemberIsID, string defaultText) {

            if (valueMemberIsID) {
                ddl.ValueMember = "geography_id";
            } else {
                ddl.ValueMember = "geography_id";
            }
            ddl.DisplayMember = "geography_id";

            CacheManager cm = CacheManager.Get(AdminProxy.Connection.ServerName);
            DataTable dt = cm["table_list_geographies-1"] as DataTable;
            if (dt == null) {
                dt = AdminProxy.ListGeographies(-1).Tables["list_geographies"];

                if (!String.IsNullOrEmpty(defaultText)) {
                    var drView = dt.NewRow();
                    if (valueMemberIsID) {
                        drView[ddl.ValueMember] = -1;
                    } else {
                        drView[ddl.ValueMember] = defaultText;
                    }
                    drView[ddl.DisplayMember] = defaultText;
                    dt.Rows.InsertAt(drView, 0);
                }
                cm["table_list_geographies-1"] = dt;
            }

            ddl.DataSource = dt;

        }


        protected List<string> SPECIAL_COMMAND_VALUES = new List<string>(new string[] { "__CURRENTCOOPERATORID__", "__CURRENTLANGUAGEID__" });
        protected List<string> SPECIAL_COMMAND_VERBIAGE = new List<string>(new string[] { "Cooperator ID for Current User", "Language ID for Current User" });

        protected string getVerbiageForSpecialCommand(string specialCommand) {
            int offset = SPECIAL_COMMAND_VALUES.IndexOf(specialCommand);
            if (offset < 0) {
                return specialCommand;
            } else {
                return SPECIAL_COMMAND_VERBIAGE[offset];
            }
        }

        protected string getSpecialCommandFromVerbiage(string verbiage) {
            int offset = SPECIAL_COMMAND_VERBIAGE.IndexOf(verbiage);
            if (offset < 0) {
                return verbiage;
            } else {
                return SPECIAL_COMMAND_VALUES[offset];
            }
        }

        /// <summary>
        /// Creates a DragDropObject and fills it appropriately assuming the tree node of the current form is the proper node to associate it with, then calls DoDragDrop on the given sender object
        /// </summary>
        /// <param name="sender"></param>
        protected void startDrag(object sender) {
            startDrag(sender, MainFormCurrentNode());
        }

        /// <summary>
        /// Creates a DragDropObject and fills it appropriately given the parent/child tree node names, then calls DoDragDrop on the given sender object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="parentNodeName"></param>
        /// <param name="childTag"></param>
        protected void startDrag(object sender, string parentNodeName) {
            var nd = MainFormGetUncleTreeNode(parentNodeName);
            startDrag(sender, nd);
        }

        /// <summary>
        /// Creates a DragDropObject and fills it appropriately given the tree node, then calls DoDragDrop on the given sender object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="node"></param>
        protected void startDrag(object sender, TreeNode node) {
            if (sender is ListView) {
                ((ListView)sender).DoDragDrop(new DragDropObject((ListView)sender, node), DragDropEffects.Copy);
            } else if (sender is TreeView){
                ((TreeView)sender).DoDragDrop(new DragDropObject(node), DragDropEffects.Copy);
            } else {
                MessageBox.Show(getDisplayMember("startDrag{badtype}", "startDrag not defined for sender of type {0}.  Edit the frmBase.startDrag() method to include it!", sender.GetType().ToString()));
            }
        }

        private void frmBase_Load(object sender, EventArgs e) {
        }

        /// <summary>
        /// If value is db representation, outputs the formatted representation.  If value is formatted representation, outputs the db representation.  i.e. "AUTO_ASSIGN_CREATE" -> "Auto-filled Created By", "Editable Data" -> "DATA"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string convertFieldPurpose(string value) {
            switch (value) {
                    // db values
                case "AUTO_DATE_CREATE":
                    return "Auto-filled Created Date";
                case "AUTO_ASSIGN_CREATE":
                    return "Auto-filled Created By";
                case "AUTO_DATE_MODIFY":
                    return "Auto-filled Modified Date";
                case "AUTO_ASSIGN_MODIFY":
                    return "Auto-filled Modified By";
                case "AUTO_DATE_OWN":
                    return "Auto-filled Owned Date";
                case "AUTO_ASSIGN_OWN":
                    return "Auto-filled Owned By";
                case "DATA":
                    return "Editable Data";
                case "PRIMARY_KEY":
                    return "Primary Key";

                    // formatted values
                case "Auto-filled Created Date":
                    return "AUTO_DATE_CREATE";
                case "Auto-filled Created By":
                    return "AUTO_ASSIGN_CREATE";
                case "Auto-filled Modified Date":
                    return "AUTO_DATE_MODIFY";
                case "Auto-filled Modified By":
                    return "AUTO_ASSIGN_MODIFY";
                case "Auto-filled Owned Date":
                    return "AUTO_DATE_OWN";
                case "Auto-filled Owned By":
                    return "AUTO_ASSIGN_OWN";
                case "Editable Data":
                    return "DATA";
                case "Primary Key":
                    return "PRIMARY_KEY";
                default:
                    return value;
            
            }
        }

        protected string convertFieldType(string value) {
            switch (value) {
                    // db values
                case "DATETIME":
                    return "Date / Time";
                case "INTEGER":
                    return "Integer";
                case "DECIMAL":
                    return "Decimal";
                case "STRING":
                    return "Text";
                case "LONG":
                    return "Large Integer";

                    // formatted values
                case "Date / Time":
                    return "DATETIME";
                case "Integer":
                    return "INTEGER";
                case "Large Integer":
                    return "LONG";
                case "Decimal":
                    return "DECIMAL";
                case "Text":
                    return "STRING";
                default:
                    return value;
            }
        }

        protected string convertFieldGuiHint(string value) {
            switch (value) {
                    // db values
                case "TOGGLE_CONTROL":
                    return "Checkbox";
                case "DATE_CONTROL":
                    return "Date / Time Control";
                case "TEXT_CONTROL":
                    return "Textbox (free-form)";
                case "LARGE_SINGLE_SELECT_CONTROL":
                    return "Lookup Picker";
                case "SMALL_SINGLE_SELECT_CONTROL":
                    return "Drop Down";
                case "INTEGER_CONTROL":
                    return "Textbox (integers only)";
                case "DECIMAL_CONTROL":
                    return "Textbox (decimals only)";

                    // formatted values
                case "Checkbox":
                    return "TOGGLE_CONTROL";
                case "Date / Time Control":
                    return "DATE_CONTROL";
                case "Textbox (free-form)":
                    return "TEXT_CONTROL";
                case "Lookup Picker":
                    return "LARGE_SINGLE_SELECT_CONTROL";
                case "Drop Down":
                    return "SMALL_SINGLE_SELECT_CONTROL";
                case "Textbox (integers only)":
                    return "INTEGER_CONTROL";
                case "Textbox (decimals only)":
                    return "DECIMAL_CONTROL";

                default:
                    return value;
            }
        }


        private bool _forceDirty;
        /// <summary>
        /// Returns true if any checkbox, radio button, textbox, or combobox has changed values since the last call to MarkClean()
        /// </summary>
        /// <returns></returns>
        protected bool IsDirty() {
            if (_forceDirty){
                return true;
            }
            if (_originalValues != null) {
                foreach(Control c in GuiManager.TraverseDescendentControls(this)){
                    if (IsControlDirty(c)) {
                        return true;
                    }
                }
            }
            return false;
        }

        protected bool IsControlDirty(Control c) {
            // clean=true is a backdoor for a control to be excluded from dirtiness (i.e. want to ignore a control's value when performing a dirty check)
            if (!(c.Tag + string.Empty).Contains("clean=true")) {
                string origValue = null;
                if (_originalValues.TryGetValue(c.Name, out origValue)) {
                    var curValue = getControlValue(c);
                    if (curValue != null) {
                        if (origValue != curValue) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Forces IsDirty() to return true regardless of control values.  Can be reset only via MarkClean().
        /// </summary>
        protected void MarkDirty() {
            _forceDirty = true;
        }


        protected void CancelAndClose() {
            if (IsDirty()) {
                if (MessageBox.Show(this, getDisplayMember("CancelAndClose{body}", "Your changes have not been saved and will be lost.\nDo you want to continue?"), 
                    getDisplayMember("CancelAndClose{title}", "Ignore Changes?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No){
                    return;
                }
            }

            DialogResult = DialogResult.Cancel;
            Close();

        }


        private Dictionary<string, string> _originalValues;

        /// <summary>
        /// Remembers current values of checkboxes, radio buttons, textboxes, and comboboxes on the form so they can be compared value when IsDirty() is called
        /// </summary>
        protected void MarkClean() {
            _forceDirty = false;
            _originalValues = new Dictionary<string, string>();
            foreach (Control c in GuiManager.TraverseDescendentControls(this)) {
                var value = getControlValue(c);
                if (value != null && !String.IsNullOrEmpty(c.Name)) {
                    _originalValues.Add(c.Name, value);
                }
            }
            if (AcceptButton != null) {
                ((Button)AcceptButton).Enabled = false;
            }
        }

        private string getControlValue(Control c) {
            if (c is RadioButton) {
                return ((RadioButton)c).Checked.ToString();
            } else if (c is CheckBox) {
                return ((CheckBox)c).Checked.ToString();
            } else if (c is Button || c is Label) {
                // buttons and labels are ignored
                return null;
            } else if (c is DataGridView){
                // datagridview. concatenate all cell values into a big string.
                var sb = new StringBuilder();
                var dgv = (DataGridView)c;
                foreach (DataGridViewRow dgvr in dgv.Rows) {
                    foreach (DataGridViewCell cell in dgvr.Cells) {
                        sb.Append(cell.Value ?? string.Empty).Append("-|-");
                    }
                    sb.AppendLine("=|=");
                }
                return sb.ToString();
            } else if (c is ComboBox){
                return (c as ComboBox).Text;
            } else if (c is ListBox){
                var sb = new StringBuilder();
                var lb = c as ListBox;
                foreach (object item in lb.Items) {
                    sb.Append(item).Append("-|-");
                }
                return sb.ToString();
            } else {
                // combobox / textbox / button / etc. just use text.
                return c.Text;
            }
        }

        protected void selectAllIfNeeded(TextBox tb, KeyEventArgs kea) {
            if (((Control.ModifierKeys & Keys.Control) == Keys.Control) && kea.KeyCode == Keys.A) {
                tb.SelectAll();
                kea.SuppressKeyPress = true;
            }
        }

        internal DragDropObject getDragDropObject(DragEventArgs e) {
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent(typeof(DragDropObject))) {
                DragDropObject tgt = e.Data.GetData(typeof(DragDropObject)) as DragDropObject;
                return tgt;
            }
            return null;
        }

        protected bool showCanDrop(DragEventArgs e, params string[] validFromCategoryNames){
            var ddo = getDragDropObject(e);
            if (ddo != null) {
                foreach (var v in validFromCategoryNames) {
                    if (ddo.FromCategoryName == v) {
                        e.Effect = DragDropEffects.Copy;
                        return true;
                    }
                }
            }
            return false;
        }

        internal virtual void FullScreenToggled(bool fullScreenEnabled) {
            // nothing to do yet, let derived forms handle it
        }

        protected void notDone() {
            MessageBox.Show(this, getDisplayMember("notDone{body}", "Not implemented yet"), getDisplayMember("notDone{title}", "Not Implemented Yet"));
        }

        public delegate void BackgroundWorkerCallback(BackgroundWorker worker);
        public delegate void BackgroundWorkerProgress(BackgroundWorker worker, ProgressChangedEventArgs e);
        public delegate void BackgroundWorkerCompleted(BackgroundWorker worker, RunWorkerCompletedEventArgs e);

        protected BackgroundWorker processInBackground(BackgroundWorkerCallback callback, BackgroundWorkerProgress progress, BackgroundWorkerCompleted completed) {
            var bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            if (callback != null) {
                bw.DoWork += new DoWorkEventHandler((sender, e) => {
                    callback(bw);
                });
                if (progress != null) {
                    bw.ProgressChanged += new ProgressChangedEventHandler((sender, e) => {
                        progress(bw, e);
                    });
                }
                if (completed != null) {
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler((sender, e) => {
                        completed(bw, e);
                    });
                }
                bw.RunWorkerAsync();
            }
            return bw;
        }

        protected void CheckDirty() {
            if (this.AcceptButton != null) {
                var btn = AcceptButton as Button;
                btn.Enabled = IsDirty();
            }
        }

        protected List<ConnectionInfo> MainFormListAllConnections() {
            return mdiParent.Current.ListAllConnections(false);
        }

        protected bool MainFormDeleteConnection(ConnectionInfo ci, bool prompt) {
            return mdiParent.Current.DeleteConnection(ci, null, prompt);
        }

        protected bool MainFormPromptForNewConnection(){
            return mdiParent.Current.PromptForNewConnection();
        }

        protected void ExportListView(ListView lv) {
            using (new AutoCursor(this)) {
                // var tempFolder = Utility.GetTempDirectory(5);
                //var tempFile = Toolkit.ResolveFilePath(tempFolder + Path.GetTempFileName() @"\list_export.csv", true);
                var tempFile = Toolkit.ResolveFilePath(Path.GetTempFileName().Replace(".tmp", ".csv"), true);
                var names = new List<string>();
                var results = new List<string>();

                foreach (ColumnHeader ch in lv.Columns) {
                    names.Add(ch.Text);
                }
                using (var sw = new StreamWriter(new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.Write))) {
                    Toolkit.OutputCSVHeader(names.ToArray(), sw);

                    foreach (ListViewItem lvi in lv.Items) {
                        results = new List<string>();
                        for (var i = 0; i < lvi.SubItems.Count; i++) {
                            results.Add(lvi.SubItems[i].Text);
                        }
                        Toolkit.OutputCSV(results.ToArray(), sw, false);
                    }
                }
                Process.Start(tempFile);
            }
        }

        /// <summary>
        /// If the combobox is databound, matches on the ValueMember.  If not, matches on the actual display text.
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected int getSelectedIndex(ComboBox ddl, string value) {
            var lowValue = "" + value.ToLower();
            var dt = ddl.DataSource as DataTable;
            if (dt == null) {
                for (var i = 0; i < ddl.Items.Count; i++) {
                    var itemValue = ddl.Items[i].ToString().ToLower();
                    if (itemValue == lowValue) {
                        return i;
                    }
                }
            } else {
                for (var i = 0; i < dt.Rows.Count; i++) {
                    if (dt.Rows[i][ddl.ValueMember].ToString().ToLower() == lowValue) {
                        return i;
                    }
                }
            }
            return -1;
        }

        protected object getSelectedValue(ComboBox ddl) {
            var ret = ddl.SelectedValue;
            if (ret != null) {
                if (ret is DataRowView) {
                    var dt = ddl.DataSource as DataTable;
                    if (dt != null) {
                        var row = ((DataRowView)ddl.SelectedValue).Row;
                        ret = row[ddl.ValueMember];
                    }
                }
            }
            return ret;
        }

        protected void showSplash(string text) {
            hideSplash();
            _splash = new frmSplash(text, false, this);
            _splash.DisableOwner = true;
        }

        protected void hideSplash() {
            if (_splash != null) {
                _splash.Close();
                _splash = null;
            }
        }

        private void frmBase_KeyPress(object sender, KeyPressEventArgs e) {
        }

        private void frmBase_KeyDown(object sender, KeyEventArgs e) {
        }

        private void frmBase_KeyUp(object sender, KeyEventArgs e) {
            Debug.WriteLine(e.Control + " & " + e.KeyCode.ToString());
            if (e.Control && e.KeyCode == Keys.S) {
                if (this.AcceptButton != null) {
                    this.AcceptButton.PerformClick();
                }
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmBase", resourceName, null, defaultValue, substitutes);
        }
    }
}
