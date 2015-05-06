using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using GrinGlobal.Admin.PopupForms;
using System.IO;
using System.Xml.XPath;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmWebApplication : GrinGlobal.Admin.ChildForms.frmBase {
        public frmWebApplication() {
            InitializeComponent();  tellBaseComponents(components);
        }

        private string _configFilePath;
        public override void RefreshData() {

            //lv.MultiSelect = !Modal;

            this.Text = "Web Application - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
            var tags = rememberSelectedTags(lv);

            // this form has multiple uses, as does the underlying ListTableMappings method.
            // it can:
            // List all TableMappings (0,[0])
            // List permission info for a single permission (37,[0])
            // List TableMappings that are NOT in a list (0, [2,3,4,5,6])


            var gringlobalWebFolder = AdminProxy.Connection.WebAppPhysicalPath; // Toolkit.GetIISPhysicalPath("gringlobal");

            if (String.IsNullOrEmpty(gringlobalWebFolder)) {
                MessageBox.Show(this, getDisplayMember("RefreshData{webappnotinstalled_body}", "The GRIN-Global Web Application does not appear to be installed."),
                    getDisplayMember("RefreshData{webappnotinstalled_title}", "GRIN-Global Web Application Not Installed"));
                this.Close();
                MainFormSelectParentTreeNode();
                return;
            }

            _configFilePath = Toolkit.ResolveFilePath(gringlobalWebFolder + @"\web.config", false);

            lv.Items.Clear();
            lvConnectionStrings.Items.Clear();

            var dt = new DataTable();
            dt.Columns.Add("setting_name", typeof(string));
            dt.Columns.Add("setting_description", typeof(string));

            if (File.Exists(_configFilePath)) {
                var nav = new XPathDocument(_configFilePath).CreateNavigator();
                var it = nav.Select("/configuration/appSettings/add");
                while (it.MoveNext()) {

                    var lvi = new ListViewItem(it.Current.GetAttribute("key", ""), 0);
                    lvi.Tag = lvi.Text;
                    dt.Rows.Add(new object[] { lvi.Text, lvi.Text });
                    var val = it.Current.GetAttribute("value", "");
                    lvi.SubItems.Add(val);
                    lvi.ToolTipText = val;
                    lv.Items.Add(lvi);

                }

                var it2 = nav.Select("/configuration/connectionStrings/add");
                while (it2.MoveNext()) {

                    var lvi = new ListViewItem(it2.Current.GetAttribute("name", ""), 1);
                    dt.Rows.Add(new object[] { lvi.Text, lvi.Text + " (Connection String)" });
                    lvi.Tag = lvi.Text;
                    lvi.SubItems.Add(it2.Current.GetAttribute("providerName", ""));
                    var connStr = it2.Current.GetAttribute("connectionString", "");
                    lvi.SubItems.Add(connStr);
                    lvi.ToolTipText = connStr;
                    lvConnectionStrings.Items.Add(lvi);

                }
            
            }

            selectRememberedTags(lv, tags);

            initHooksForMdiParent(dt, "setting_description", "setting_name");

            MainFormUpdateStatus(getDisplayMember("RefreshData{done}", "Refreshed Application Settings"), false);
        }

        private void showAppSettingProperties() {
            if (lv.SelectedItems.Count == 1) {
                MainFormSelectChildTreeNode(lv.SelectedItems[0].Tag.ToString());
            }
        }

        private void defaultPermissionMenuItem_Click(object sender, EventArgs e) {
            showAppSettingProperties();
        }

        private void refreshPermissionMenuItem_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void deletePermissionMenuItem_Click(object sender, EventArgs e) {
            promptToDeleteAppSettings();
        }

        private void promptToDeleteAppSettings() {
            if (lv.SelectedItems.Count > 0) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToDeleteAppSettings{start_body}", "Are you sure you want to delete application setting(s)?"),
                    getDisplayMember("promptToDeleteAppSettings{start_title}", "Delete Application Setting(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lv.SelectedItems) {
                        AdminProxy.DeleteApplicationSetting(lvi.Tag.ToString(), _configFilePath);
                    }
                    MainFormUpdateStatus(getDisplayMember("promptToDeleteAppSettings{done}", "Deleted {0} Application Settings", lv.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
                }
            }
        }

        private void ctxMenuNodePermission_Opening(object sender, CancelEventArgs e) {
            cmiAppSettingsDelete.Enabled = lv.SelectedItems.Count > 0;
            cmiAppSettingsProperties.Enabled = lv.SelectedItems.Count == 1;
        }

        private void lv_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void lv_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDeleteAppSettings();
            }
        }

        private void lv_MouseDoubleClick(object sender, MouseEventArgs e) {
            showAppSettingProperties();
        }

        private void lv_SelectedIndexChanged_1(object sender, EventArgs e) {

        }

        private void frmTableMappings_Load(object sender, EventArgs e) {
            MakeListViewSortable(lv);
        }

        private void newMenuItem_Click(object sender, EventArgs e) {
            var f = new frmAppSetting();
            f.ConnectionStringMode = false;
            MainFormPopupNewItemForm(f);
        }

        private void btnClearAllCaches_Click(object sender, EventArgs e) {
            using (new AutoCursor(this)) {
                if (this.AdminProxy.ClearCache(null)) {
                    MessageBox.Show(this, getDisplayMember("clearallcaches{start_body}", "All caches in the Web Application have been cleared"), 
                        getDisplayMember("clearallcaches{start_title}", "Web Caches Cleared"));
                } else {
                    MessageBox.Show(this, getDisplayMember("clearallcaches{failed_body}", "An unknown error occurred when clearing the Web Application caches."), 
                        getDisplayMember("clearallcaches{failed_title}", "Failed Clearing Web Caches"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e) {
            showConnectionStringProperties();

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e) {
            RefreshData();

        }

        private void promptToDeleteConnectionStrings() {
            if (lvConnectionStrings.SelectedItems.Count > 0) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToDeleteConnectionStrings{start_body}", "Are you sure you want to delete connection string(s)?"),
                    getDisplayMember("promptToDeleteConnectionString{start_title}", "Delete Connection String(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lvConnectionStrings.SelectedItems) {
                        AdminProxy.DeleteConnectionString(lvi.Tag.ToString(), _configFilePath);
                    }
                    MainFormUpdateStatus(getDisplayMember("promptToDeleteConnectionStrings{done}", "Deleted {0} connection string(s)", lvConnectionStrings.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            var f = new frmAppSetting();
            f.ConnectionStringMode = true;
            MainFormPopupNewItemForm(f);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            promptToDeleteConnectionStrings();

        }

        private void showConnectionStringProperties() {
            if (lvConnectionStrings.SelectedItems.Count == 1) {
                MainFormSelectChildTreeNode(lvConnectionStrings.SelectedItems[0].Tag.ToString());
            }
        }
        private void lvConnectionStrings_MouseDoubleClick(object sender, MouseEventArgs e) {
            showConnectionStringProperties();

        }

        private void cmiAppSettingsExportList_Click(object sender, EventArgs e) {
            ExportListView(lv);

        }

        private void cmiConnectionStringsExportList_Click(object sender, EventArgs e) {
            ExportListView(lvConnectionStrings);

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmWebApplication", resourceName, null, defaultValue, substitutes);
        }
    }
}
