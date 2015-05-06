using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using GrinGlobal.InstallHelper;
using GrinGlobal.Admin.PopupForms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Xml.XPath;
using System.Xml;
using System.ServiceModel;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmSearchEngineIndexes : GrinGlobal.Admin.ChildForms.frmBase {
        public frmSearchEngineIndexes() {
            InitializeComponent();  tellBaseComponents(components);
            initDropDowns();
        }

        DataSet _dsInfo = null;

        private void refreshStatus() {
            using (new AutoCursor(this)) {

                bool done = false;
                DataSet ds = null;
                while (!done) {
                    done = true;
                    try {
                        ds = AdminProxy.GetSearchEngineStatus();
                    } catch (Exception ex) {
                        if (ex.Message.Contains("no endpoint listening")) {
                            var res = MessageBox.Show(this, getDisplayMember("refreshStatus{promptRestart_body}", "Could not contact the search engine service on the local machine.\n\nAttempt to restart the service and try again?"), 
                                getDisplayMember("refreshStatus{promptRestart_title}", "Restart Search Engine?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (res == DialogResult.Yes) {
                                try {
                                    Toolkit.StopService("ggse");
                                    Toolkit.StartService("ggse");
                                    done = false;
                                    continue;
                                } catch (Exception exRestart) {
                                    MessageBox.Show(this, getDisplayMember("refreshStatus{restartfailed_body}", "Failed to restart search engine service: {0}", exRestart.Message), 
                                        getDisplayMember("refreshStatus{restartfailed_title}","Search Engine Start Failed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                //} else {
                                //    this.BeginInvoke((MethodInvoker)delegate() { MainFormSelectParentTreeNode(); });
                            }
                            return;
                        } else {
                            MessageBox.Show(this, getDisplayMember("refreshStatus{cantconnect_body}", "Error connecting to search engine service: {0}", ex.Message), 
                                getDisplayMember("refreshStatus{cantconnect_title}", "Search Engine Connection Failed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //this.BeginInvoke((MethodInvoker)delegate() {MainFormSelectParentTreeNode(); });
                            return;
                        }
                    }
                }

                var dtStatus = ds.Tables["status"];
                if (dtStatus.Rows.Count > 0) {
                    var drStatus = dtStatus.Rows[0];
                    lblClientCount.Text = drStatus["connected_clients"].ToString();
                    lblPendingIndexCreations.Text = drStatus["processing_indexes"].ToString();
                    lblPendingRealtimeUpdatesCount.Text = drStatus["pending_realtime_updates"].ToString();
                }
            }
        }

        public override void RefreshData() {

            using (new AutoCursor(this)) {

                //lvIndexes.MultiSelect = !Modal;
                PrimaryTabControl = tcIndexes;

                this.Text = "Search Engine - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
                var tags = rememberSelectedTags(lvIndexes);

                // this form has multiple uses, as does the underlying ListIndexes method.
                // it can:
                // List all Indexes (0,[0])
                // List Index info for a single Index (37,[0])
                // List Indexes that are NOT in a list (0, [2,3,4,5,6])
                bool done = false;

                while (!done) {
                    done = true;
                    try {
                        _dsInfo = AdminProxy.GetSearchEngineInfoEx(false, null, null);
                    } catch (CommunicationObjectFaultedException cofe){
                        if (AdminProxy.Connection.IsLocal) {
                            var res = MessageBox.Show(this, getDisplayMember("refreshStatus{promptRestart_body}", "Could not contact the search engine service on the local machine.\n\nAttempt to restart the service and try again?"),
                                getDisplayMember("refreshStatus{promptRestart_title}", "Restart Search Engine?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (res == DialogResult.Yes) {
                                try {
                                    Toolkit.StopService("ggse");
                                    Toolkit.StartService("ggse");
                                    done = false;
                                    continue;
                                } catch (Exception exRestart) {
                                    MessageBox.Show(this, getDisplayMember("refreshStatus{restartfailed_body}", "Failed to restart search engine service: {0}", exRestart.Message),
                                        getDisplayMember("refreshStatus{restartfailed_title}", "Search Engine Start Failed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                //} else {
                                //    this.BeginInvoke((MethodInvoker)delegate() { MainFormSelectParentTreeNode(); });
                            }
                        } else {
                            MessageBox.Show(this, getDisplayMember("refreshStatus{cantconnect_body}", "Error connecting to search engine service: {0}", cofe.Message),
                                getDisplayMember("refreshStatus{cantconnect_title}", "Search Engine Connection Failed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return;
                    } catch (Exception ex) {
                        MessageBox.Show(this, getDisplayMember("refreshStatus{cantconnect_body}", "Error connecting to search engine service: {0}", ex.Message),
                            getDisplayMember("refreshStatus{cantconnect_title}", "Search Engine Connection Failed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //this.BeginInvoke((MethodInvoker)delegate() {MainFormSelectParentTreeNode(); });
                        return;
                    }
                }
                var dtIndexer = _dsInfo.Tables["indexer"];
                var dtIndexes = _dsInfo.Tables["index"];
                var dtResolvers = _dsInfo.Tables["resolver"];

                if (dtIndexer == null || dtIndexes == null || dtResolvers == null) {
                    MessageBox.Show(this, getDisplayMember("RefreshData{badprotocol_body}", "The installed version of the search engine does not support the protocol that this version of the Admin Tool expects.\nPlease install a newer version of the search engine."), 
                         getDisplayMember("RefreshData{badprotocol_title}", "Outdated Search Engine"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (dtIndexer.Rows.Count > 0) {
                    var drIndexer = dtIndexer.Rows[0];
                    chkTermCachingEnabled.Checked = drIndexer["term_caching_enabled"].ToString().Trim().ToUpper() == "TRUE";
                    txtTermCacheMinimum.Text = Toolkit.ToInt32(drIndexer["term_caching_minimum"], 1500).ToString();
                    ddlProvider.SelectedIndex = getSelectedIndex(ddlProvider, drIndexer["provider_name"].ToString());
                    txtConnectionString.Text = drIndexer["connection_string"].ToString();


                    var stopWords = drIndexer["stop_words"].ToString().Split('\t');
                    lbStopWords.Items.Clear();
                    for (var i = 0; i < stopWords.Length; i++) {
                        if (stopWords[i].Length == 1) {
                            stopWords[i] = getCharFriendlyName(stopWords[i].ToCharArray()[0]);
                        }
                    }
                    lbStopWords.Items.AddRange(stopWords);

                    var sepChars = drIndexer["separators"].ToString().ToCharArray();
                    var sepStrings = new List<string>();
                    foreach (var c in sepChars) {
                        sepStrings.Add(getCharFriendlyName(c));
                    }
                    lbSeparators.Items.Clear();
                    lbSeparators.Items.AddRange(sepStrings.ToArray());

                }




                initHooksForMdiParent(dtIndexes, "index_name", "index_name");
                lvIndexes.Items.Clear();
                foreach (DataRow dr in dtIndexes.Rows) {
                    var lvi = new ListViewItem(dr["index_name"].ToString(), 0);
                    lvi.UseItemStyleForSubItems = true;
                    lvi.SubItems.Add(Toolkit.ToBoolean(dr["enabled"], false) ? "Y" : "N");

                    var resolvers = dtResolvers.Select("index_name = '" + lvi.Text + "'");

                    var list = new List<string>();
                    foreach (var drr in resolvers) {
                        list.Add(drr["resolver_name"].ToString());
                    }
                    lvi.SubItems.Add(String.Join(", ", list.ToArray()));
                    var valid = Toolkit.ToBoolean(dr["is_valid"], false);
                    if (valid) {
                        lvi.SubItems.Add("Y");
                    } else {
                        lvi.SubItems.Add("N - " + dr["invalid_reason"]);
                        lvi.ForeColor = Color.Red;
                    }
                    var rebuildDate = dr["last_rebuild_end_date"];
                    if (rebuildDate == DBNull.Value) {
                        lvi.SubItems.Add(" n/a");
                        lvi.ToolTipText = "Has never been built";
                    } else {
                        var date = Toolkit.ToDateTime(rebuildDate, DateTime.MinValue);
                        if (date == DateTime.MinValue) {
                            var beginDateText = Toolkit.ToDateTime(dr["last_rebuild_begin_date"], DateTime.MinValue).ToLocalTime().ToString();
                            lvi.SubItems.Add("Rebuilding now, started at " + beginDateText);
                            lvi.ToolTipText = "Strill rebuilding, started at " + beginDateText;
                        } else {
                            var errorText = dr["error_on_rebuild"].ToString();
                            if (!String.IsNullOrEmpty(errorText)) {
                                // error during creation.
                                lvi.SubItems.Add("*** Error on last Rebuild at " + date.ToLocalTime().ToString() + " -- " + errorText);
                                lvi.ForeColor = Color.Red;

                                lvi.ToolTipText = errorText;
                            } else {
                                var endDateText = date.ToLocalTime().ToString();
                                lvi.SubItems.Add(endDateText);
                                lvi.ToolTipText = "Last rebuild finished at " + endDateText;
                            }
                        }
                    }

                    lvi.ImageIndex = 0;
                    lvi.Tag = dr["index_name"];
                    lvIndexes.Items.Add(lvi);
                }

                var dtStatus = _dsInfo.Tables["status"];
                if (dtStatus.Rows.Count > 0) {
                    var drStatus = dtStatus.Rows[0];
                    lblClientCount.Text = drStatus["connected_clients"].ToString();
                    lblPendingIndexCreations.Text = drStatus["processing_indexes"].ToString();
                    lblPendingRealtimeUpdatesCount.Text = drStatus["pending_realtime_updates"].ToString();
                }

                selectRememberedTags(lvIndexes, tags);

                // wipe out log so it is refreshed when they go back to that tab
                _dtLog = null;

                MarkClean();

                syncGUI();

                MainFormUpdateStatus(getDisplayMember("RefreshData[done}", "Refreshed Search Engine"), false);

                if (tcIndexes.SelectedTab == tpLog) {
                    refreshLogData();
                }
            }

        }

        private string getCharFriendlyName(char c) {
            switch (c) {
                case '\t':
                    return @"\t (Tab)";
                case '\r':
                    return @"\r (Carriage Return)";
                case '\n':
                    return @"\n (New Line)";
                case ' ':
                    return @"   (Space)";
                default:
                    return c.ToString();
            }
        }

        private void showProperties() {
            if (lvIndexes.SelectedItems.Count == 1) {
                MainFormSelectChildTreeNode(lvIndexes.SelectedItems[0].Tag.ToString());
            }
        }

        private void defaultIndexMenuItem_Click(object sender, EventArgs e) {
            showProperties();
        }

        private void refreshIndexMenuItem_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void deleteIndexMenuItem_Click(object sender, EventArgs e) {
            promptToDelete();
        }

        private void promptToDelete() {
            if (lvIndexes.SelectedItems.Count > 0) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToDelete{prompt_body}", "Are you sure you want to delete index(s)?"), 
                    getDisplayMember("promptToDelete{prompt_title}", "Delete Index(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lvIndexes.SelectedItems) {
                        AdminProxy.DeleteSearchEngineIndex(lvi.Tag + "");
                    }
                    MainFormUpdateStatus(getDisplayMember("promptToDelete{done}", "Deleted {0} index(es)", lvIndexes.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
                }
            }
        }

        private void disableIndexMenuItem_Click(object sender, EventArgs e) {
            if (lvIndexes.SelectedItems.Count > 0) {
                foreach (ListViewItem lvi in lvIndexes.SelectedItems) {
                    AdminProxy.DisableSearchEngineIndex(lvi.Tag + "");
                }
                MainFormUpdateStatus(getDisplayMember("disable{done}", "Disabled {0} index(es)", lvIndexes.SelectedItems.Count.ToString("###,##0")), true);
                RefreshData();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            if (lvIndexes.SelectedItems.Count > 0) {
                foreach (ListViewItem lvi in lvIndexes.SelectedItems) {
                    AdminProxy.EnableSearchEngineIndex(lvi.Tag + "");
                }
                MainFormUpdateStatus(getDisplayMember("enable{done}", "Enabled {0} index(es)", lvIndexes.SelectedItems.Count.ToString("###,##0")), true);
                RefreshData();
            }
        }

        private void ctxMenuNodeIndex_Opening(object sender, CancelEventArgs e) {
            cmiIndexDelete.Enabled = lvIndexes.SelectedItems.Count > 0;
            cmiIndexProperties.Enabled = lvIndexes.SelectedItems.Count == 1;

            int toEnable = 0;
            int toDisable = 0;
            foreach (ListViewItem lvi in lvIndexes.SelectedItems) {
                if (lvi.SubItems[1].Text == "Y") {
                    toDisable++;
                } else {
                    toEnable++;
                }
            }

            cmiIndexEnable.Visible = toEnable > 0;
            cmiIndexDisable.Visible = toDisable > 0;
        }

        private void lvIndexes_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void lvIndexes_MouseDoubleClick(object sender, MouseEventArgs e) {
            showProperties();
        }

        private void frmIndexes_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvIndexes);
        }

        private void initDropDowns() {
            initDatabaseDropDown(ddlProvider);
        }

        public List<int> AssignedIDs = new List<int>();
        public List<int> SelectedIDs = new List<int>();

        private void lvIndexes_ItemDrag(object sender, ItemDragEventArgs e) {
            startDrag(sender);
        }

        private void lvIndexes_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDelete();
            }
        }

        private void newIndexMenuItem_Click(object sender, EventArgs e) {
            MainFormPopupNewItemForm(new frmSearchEngineIndex());
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e) {
            if (lvIndexes.SelectedItems.Count > 0) {
                using (new AutoCursor(this)) {
                    foreach (ListViewItem lvi in lvIndexes.SelectedItems) {
                        AdminProxy.RebuildSearchEngineIndex("" + lvi.Tag);
                    }
                    MainFormUpdateStatus(getDisplayMember("rebuilding{start}", "Rebuilding {0} index(es)", lvIndexes.SelectedItems.Count.ToString("###,##0")), true);

                    // wait a second, give search engine a chance to start processing the index before refreshing the form
                    Thread.Sleep(1000);
                    RefreshData();

                    MainFormNotifyIndexCompleted();

                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            if (lvIndexes.SelectedItems.Count > 0) {
                foreach (ListViewItem lvi in lvIndexes.SelectedItems) {
                    AdminProxy.DefragSearchEngineIndex("" + lvi.Tag);
                }
                MainFormUpdateStatus(getDisplayMember("defrag{start}", "Defragmenting {0} index(es)", lvIndexes.SelectedItems.Count.ToString("###,##0")), true);
                RefreshData();
            }
    
        }

        private void frmSearchEngineIndexes_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvIndexes);
            btnTest.Visible = Debugger.IsAttached;
        }

        private StringBuilder _log;
        private DataTable _dtLog;
        private void refreshLogData() {
            lvLog.Items.Clear();
            txtMessage.Text = "";
            _log = new StringBuilder();
            if (_dtLog == null) {
                using (new AutoCursor(this)) {
                    _dtLog = AdminProxy.GetSearchEngineLog().Tables["search_engine_log"];
                }
            }

            var listItems = new List<ListViewItem>();
            foreach (DataRow dr in _dtLog.Rows) {
                var lvi = new ListViewItem((Toolkit.ToDateTime(dr["date"], DateTime.MinValue)).ToLocalTime().ToString());
                lvi.UseItemStyleForSubItems = true;

                lvi.SubItems.Add(dr["type"].ToString());
                if (lvi.SubItems[1].Text.ToLower().Trim() == "error") {
                    lvi.ForeColor = Color.Red;
                }

                lvi.SubItems.Add(dr["message"].ToString().Replace(@"\r", "").Replace(@"\n", " ").Replace(@"\t", " "));
                lvi.ToolTipText = dr["message"].ToString().Replace(@"\r", "\r").Replace(@"\n", "\n").Replace(@"\t", "\t");
                _log.AppendLine(String.Format("{0} - {1} - {2}", lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text).Replace("\r", "\\r").Replace("\n", "\\n"));
                listItems.Add(lvi);
            }
            lvLog.Items.AddRange(listItems.ToArray());
        }

        //private void btnViewLog_Click(object sender, EventArgs e) {
        //    var ds = AdminProxy.GetSearchEngineLog();
        //    var f = new frmLogViewer();
        //    f.ShowDialog(this, ds.Tables["search_engine_log"]);
        //}

        private void btnRefresh_Click(object sender, EventArgs e) {
            refreshStatus();
        }

        private void lblClientCount_Click(object sender, EventArgs e) {

        }

        private void label3_Click(object sender, EventArgs e) {

        }

        private void lvLog_SelectedIndexChanged(object sender, EventArgs e) {
            if (lvLog.SelectedItems.Count > 0) {
                txtMessage.Text = lvLog.SelectedItems[0].SubItems[2].Text;
            } else {
                txtMessage.Text = "(choose an item above to view more information about it)";
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            if (tcIndexes.SelectedTab == tpLog && _dtLog == null) {
                refreshLogData();
            }
        }

        private void btnShowInTextEditor_Click(object sender, EventArgs e) {
            var tempFile = Toolkit.ResolveFilePath(Utility.GetTempDirectory(20) + @"\search_engine_log.txt", true);
            File.WriteAllText(tempFile, _log.ToString());
            Process.Start(tempFile);
        }

        private void chkTermCachingEnabled_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void syncGUI() {

            lblTermCacheMinimum.Enabled = txtTermCacheMinimum.Enabled = chkTermCachingEnabled.Checked;

            lblNotRegressionTested.Visible = ddlProvider.Text.Contains("*");

            CheckDirty();
        }

        private void btnDatabasePrompt_Click(object sender, EventArgs e) {
            var f = new frmDatabaseLoginPrompt();
            if (DialogResult.OK == f.ShowDialog(this, null, false, false, false, null, ddlProvider.Text.ToLower().Contains("sql server"), false, false)) {
                ddlProvider.SelectedIndex = ddlProvider.FindString(f.ddlEngine.Text);
                txtTermCacheMinimum.Text = f.txtConnectionstring.Text;
            }
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e) {
            selectAllIfNeeded(sender as TextBox, e);
        }

        private void refreshLogMenuItem_Click(object sender, EventArgs e) {
            _dtLog = null;
            refreshLogData();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (IsDirty()) {
                //MessageBox.Show("TODO: save here!");

                DataRow dr = null;
                var dt = _dsInfo.Tables["indexer"];
                if (dt.Rows.Count == 0) {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                } else {
                    dr = dt.Rows[0];
                }

                dr["connection_string"] = txtConnectionString.Text;
                dr["term_caching_minimum"] = Toolkit.ToInt32(txtTermCacheMinimum.Text, 1500);
                dr["term_caching_enabled"] = chkTermCachingEnabled.Checked;
                if (ddlProvider.SelectedIndex > -1) {
                    dr["provider_name"] = ddlProvider.SelectedValue.ToString();
                }

                // separators...
                var sb = new StringBuilder();
                foreach (string val in lbSeparators.Items) {
                    sb.Append(val);
                }
                dr["separators"] = sb.ToString();

                // stop words...
                sb = new StringBuilder();
                foreach (string val in lbStopWords.Items) {
                    sb.Append(val).Append("\t");
                }
                dr["stop_words"] = sb.ToString().Trim();


                AdminProxy.SaveSearchEngineIndexer(_dsInfo);

                MainFormUpdateStatus(getDisplayMember("save{done}", "Saved search engine global settings"), true);

                // if they changed db connection, stopwords, or separators, all the enabled indexes needs to be rebuilt in case they were pulling their values from the defaults changed here.
                if (IsControlDirty(ddlProvider) || IsControlDirty(txtConnectionString) || IsControlDirty(lbSeparators) || IsControlDirty(lbStopWords)) {
                    var result = MessageBox.Show(this, getDisplayMember("save{delayed_body}", "Your changes have been saved, but they will not take effect until all enabled indexes are rebuilt.\nWould you like to rebuild them now?"), 
                        getDisplayMember("save{delayed_title}", "Save Successful - Rebuild Indexes?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes) {
                        AdminProxy.RebuildAllSearchEngineIndexes();
                    }
                }

                MarkClean();
                syncGUI();
            }
        }

        private void btnRemoveStopWord_Click(object sender, EventArgs e) {

            removeSelectedItems(lbStopWords);

        }

        private void removeSelectedItems(ListBox lb) {
            if (lb.SelectedItems.Count > 0) {
                while (lb.SelectedItems.Count > 0) {
                    lb.Items.Remove(lb.SelectedItems[0]);
                }
                syncGUI();
            }
        }

        private void btnRemoveSeparator_Click(object sender, EventArgs e) {
            removeSelectedItems(lbSeparators);
        }

        private void ddlProvider_SelectedIndexChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void btnAddSeparator_Click(object sender, EventArgs e) {
            var f = new frmAddSeparator();
            if (f.ShowDialog(this) == DialogResult.OK) {
                lbSeparators.Items.Add(f.SelectedCharacter.ToString());
                syncGUI();
            }

        }

        private void btnAddStopWord_Click(object sender, EventArgs e) {
            var f = new frmAddWord();
            if (f.ShowDialog(this) == DialogResult.OK) {
                lbStopWords.Items.Add(f.SelectedWord);
                syncGUI();
            }

        }

        private void txtConnectionString_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtTermCacheMinimum_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void btnTest_Click(object sender, EventArgs e) {

            var engines = new string[] { "sqlserver", "mysql", "oracle", "postgresql" };

            var masterConfig = @"C:\projects\gringlobal\gringlobal.search.engine.tester\gringlobal.search.config";

            var doc = new XmlDocument();
            doc.Load(masterConfig);


            foreach (var eng in engines) {

                var fp = @"C:\projects\gringlobal\gringlobal.search.engine.tester\" + eng + "_queries.xml";
                if (File.Exists(fp)) {
                    var nav = new XPathDocument(fp).CreateNavigator();
                    var it = nav.Select("/queries/*");

                    while (it.MoveNext()) {
                        var idx = it.Current.Name;

                        var sqlNode = it.Current.SelectSingleNode("sql");
                        var idxSql = sqlNode.Value;

                        var ndIdx = doc.SelectSingleNode("/Indexes/Index[@Name='" + idx + "']");
                        if (ndIdx != null) {

                            addDataview("search_" + idx, idxSql, eng);

                            // save the dataview name to the master config file
                            var att = ndIdx.Attributes["DataviewName"];
                            if (att == null) {
                                att = doc.CreateAttribute("DataviewName");
                                ndIdx.Attributes.Append(att);
                            }
                            att.Value = "search_" + idx;


                            var resolvers = it.Current.Select("./*");
                            while (resolvers.MoveNext()) {
                                var nd = resolvers.Current;
                                if (nd.Name != "sql") {

                                    var res = nd.Name;

                                    var resSql = ("" + nd.Value).Trim();

                                    if (!String.IsNullOrEmpty(resSql)) {

                                        addDataview("search_" + idx + "__" + res, resSql, eng);

                                        var resTitleCase = res.Replace("_", " ").ToTitleCase().Replace(" ", "_");

                                        var ndRes = doc.SelectSingleNode("/Indexes/Index[@Name='" + idx + "']/Resolvers/Resolver[@Name='" + resTitleCase + "']");
                                        var attRes = ndRes.Attributes["DataviewName"];
                                        if (attRes == null) {
                                            attRes = doc.CreateAttribute("DataviewName");
                                            ndRes.Attributes.Append(attRes);
                                        }
                                        attRes.Value = "search_" + idx + "__" + res;
                                    }

                                }
                            }
                        }
                    }
                }
            }

            doc.Save(masterConfig);
        }

        private void addDataview(string dvName, string sql, string engine) {

            sql = ("" + sql).Trim();
            if (String.IsNullOrEmpty(sql)) {
                return;
            }

            var dsInfo = AdminProxy.GetDataViewDefinition(dvName);
            var dv = dsInfo.Tables["sys_dataview_sql"];
            var r = dv.Select("database_engine_tag = '" + engine + "'");
            DataRow dvRow = null;
            if (r.Length == 0) {
                dvRow = dv.NewRow();
                dv.Rows.Add(dvRow);
            } else {
                dvRow = r[0];
            }
            dvRow["sql_statement"] = sql;
            dvRow["database_engine_tag"] = engine;
            //dvRow["dataview_name"] = dvName;

            var cat = "";
            if (dvName.StartsWith("search_accession")) {
                cat = "Accessions";
            } else if (dvName.StartsWith("search_inventory")) {
                cat = "Inventory";
            } else if (dvName.StartsWith("search_order")) {
                cat = "Orders";
            }

            AdminProxy.CreateDataViewDefinition(dvName, true, "Search Engine", cat, 0, false, null, null, null, null, dsInfo);

        }

        private void btnRefreshLog_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void cmiIndexVerify_Click(object sender, EventArgs e) {
            if (lvIndexes.SelectedItems.Count > 0) {
                var output = new List<string>();
                using (new AutoCursor(this)) {
                    var indexes = new List<string>();
                    foreach (ListViewItem lvi in lvIndexes.SelectedItems) {
                        indexes.Add(lvi.Tag + "");
                    }

                    MainFormUpdateStatus(getDisplayMember("verify{start}", "Verifying {0} index(es)", indexes.Count.ToString("###,##0")), true);
                    output = AdminProxy.VerifySearchEngineIndexes(indexes);

                }

                if (output.Count == 0) {
                    output.Add(getDisplayMember("verify{none}", "Selected index(es) are valid."));
                    MainFormUpdateStatus(output[0], true);
                } else {
                    MainFormUpdateStatus(getDisplayMember("verify{many}", "{0} index(es) are invalid", output.Count.ToString("###,##0")), true);
                }

                var f = new frmMessageBox();
                f.txtMessage.Text = String.Join("\n", output.ToArray());
                f.btnYes.Visible = false;
                f.btnNo.Text = "OK";
                f.ShowDialog(this);

            }

        }

        private void cmiIndexReload_Click(object sender, EventArgs e) {
            if (lvIndexes.SelectedItems.Count > 0) {
                using (new AutoCursor(this)) {
                    var output = new List<string>();
                    var indexes = new List<string>();
                    foreach (ListViewItem lvi in lvIndexes.SelectedItems) {
                        indexes.Add(lvi.Tag + "");
                    }

                    MainFormUpdateStatus(getDisplayMember("reload{start}", "Reloading {0} index(es)", indexes.Count.ToString("###,##0")), true);
                    AdminProxy.ReloadSearchEngineIndexes(indexes);

                    var f = new frmMessageBox();
                    f.txtMessage.Text = String.Join("\n", output.ToArray());
                    f.ShowDialog(this);

                }
            }
        }

        private void cmiIndexRebuildAll_Click(object sender, EventArgs e) {
            AdminProxy.RebuildAllSearchEngineIndexes();
            MainFormUpdateStatus(getDisplayMember("rebuild{all}", "Rebuilding all search engine indexes..."), true);
        }
        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmSearchEngineIndexes", resourceName, null, defaultValue, substitutes);
        }
    }
}
