using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using GrinGlobal.Admin.PopupForms;
using GrinGlobal.Business;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmConnections : GrinGlobal.Admin.ChildForms.frmBase {
        public frmConnections() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public override void RefreshData() {

            this.Text = "Connections";
            var tags = rememberSelectedTags(lvConnections);

            //initHooksForMdiParent(dt, "table_name", "sys_table_id");
            lvConnections.Items.Clear();
            var conns = MainFormListAllConnections();
            foreach(var ci in conns){
                var lvi = new ListViewItem(ci.DatabaseEngineServerName);
                lvi.ImageIndex = 0;
                lvi.SubItems.Add(ci.DatabaseEngineProviderName);
                lvi.Tag = ci;
                lvConnections.Items.Add(lvi);
            }

            selectRememberedTags(lvConnections, tags);

            MainFormUpdateStatus(getDisplayMember("RefreshData{refreshed}", "Refreshed Connections"), false);
        }

        private void showProperties() {
            if (lvConnections.SelectedItems.Count == 1) {
                MainFormSelectConnection(lvConnections.SelectedItems[0].Tag as ConnectionInfo);
            }
        }

        private void defaultPermissionMenuItem_Click(object sender, EventArgs e) {
            showProperties();
        }

        private void refreshPermissionMenuItem_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void deletePermissionMenuItem_Click(object sender, EventArgs e) {
            promptToDelete();
        }

        private void promptToDelete() {
            if (lvConnections.SelectedItems.Count > 0) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToDelete{body}", "Are you sure you want to delete connection(s)?"), 
                    getDisplayMember("promptToDelete{title}", "Delete Connection(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lvConnections.SelectedItems) {
                        var ci = lvi.Tag as ConnectionInfo;
                        if (ci != null) {
                            MainFormDeleteConnection(ci, false);
                        }
                    }
                    RefreshData();
                    MainFormUpdateStatus(getDisplayMember("promptToDelete{deleted}", "Deleted {0} connection(s).", lvConnections.SelectedItems.Count.ToString("###,##0")), true);
                }
            }
        }

        private void ctxMenuNodePermission_Opening(object sender, CancelEventArgs e) {
            cmiDelete.Enabled = lvConnections.SelectedItems.Count > 0;
            cmiProperties.Enabled = lvConnections.SelectedItems.Count == 1;
        }

        private void lvMappings_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void lvMappings_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDelete();
            }
        }
        private void toolStripMenuItem1_Click_1(object sender, EventArgs e) {
            MainFormPopupNewItemForm(new frmLogin());
        }

        private void lvMappings_MouseDoubleClick(object sender, MouseEventArgs e) {
            showProperties();
        }

        private void lvMappings_SelectedIndexChanged_1(object sender, EventArgs e) {

        }

        private void frmTableMappings_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvConnections);
        }

        private void cmiConnectionNew_Click(object sender, EventArgs e) {
            if (MainFormPromptForNewConnection()) {
                RefreshData();
            }
        }

        private void cmiConnectionExportList_Click(object sender, EventArgs e) {
            ExportListView(lvConnections);
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmConnections", resourceName, null, defaultValue, substitutes);
        }
    }
}
