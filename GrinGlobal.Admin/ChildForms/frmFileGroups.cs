using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using GrinGlobal.Admin.PopupForms;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmFileGroups : GrinGlobal.Admin.ChildForms.frmBase {
        public frmFileGroups() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public override void RefreshData() {

            //lv.MultiSelect = !Modal;

            this.Text = "File Groups - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
            var tags = rememberSelectedTags(lv);

            // this form has multiple uses, as does the underlying ListTableMappings method.
            // it can:
            // List all TableMappings (0,[0])
            // List permission info for a single permission (37,[0])
            // List TableMappings that are NOT in a list (0, [2,3,4,5,6])

            var ds = AdminProxy.ListFileGroups(-1);

            var dt = ds.Tables["list_file_groups"];

            initHooksForMdiParent(dt, "group_name", "sys_file_group_id");
            lv.Items.Clear();
            foreach (DataRow dr in DataTable.Rows) {
                var lvi = new ListViewItem(dr["group_name"].ToString(), 0);
                lvi.SubItems.Add(dr["version_name"].ToString());
                lvi.SubItems.Add(dr["is_enabled"].ToString());
                var sizeInMB = Toolkit.ToDecimal(dr["total_size"], 0.0M) / 1024.0M / 1024.0M;
                lvi.SubItems.Add(sizeInMB.ToString("###,###,##0.00"));
                var lastTouched = dr["modified_date"] == DBNull.Value ? dr["created_date"] : dr["modified_date"];
                lvi.SubItems.Add(((DateTime)lastTouched).ToString());
                lvi.UseItemStyleForSubItems = false;

                lvi.Tag = dr["sys_file_group_id"].ToString();

                lv.Items.Add(lvi);

            }

            selectRememberedTags(lv, tags);

            MainFormUpdateStatus(getDisplayMember("RefreshData{done}", "Refreshed file groups"), false);
        }

        private void showProperties() {
            if (lv.SelectedItems.Count == 1) {
                MainFormSelectChildTreeNode(lv.SelectedItems[0].Tag.ToString());
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
            if (lv.SelectedItems.Count > 0) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToDelete{prompt_body}", "Are you sure you want to delete file group(s)?\nAny associated files will not be deleted."), 
                    getDisplayMember("promptToRemove{prompt_title}", "Delete File Group(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lv.SelectedItems) {
                        AdminProxy.DeleteFileGroup(Toolkit.ToInt32(lvi.Tag, -1));
                    }
                    MainFormUpdateStatus(getDisplayMember("promptToDelete{done}", "Deleted {0} file group(s)", lv.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
                }
            }
        }

        private void disablePermissionMenuItem_Click(object sender, EventArgs e) {
            if (lv.SelectedItems.Count > 0) {
//                if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to disable permission(s)?", "Disable Permission(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lv.SelectedItems) {
                        AdminProxy.DisableFileGroup(Toolkit.ToInt32(lvi.Tag, -1));
                    }
                    MainFormUpdateStatus(getDisplayMember("disableFileGroup{done}", "Disabled {0} file group(s)", lv.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
//                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            if (lv.SelectedItems.Count > 0) {
//                if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to enable permission(s)?", "Enable Permission(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lv.SelectedItems) {
                        AdminProxy.EnableFileGroup(Toolkit.ToInt32(lvi.Tag, -1));
                    }
                    MainFormUpdateStatus(getDisplayMember("enableFileGroup{done}", "Enabled {0} file group(s)", lv.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
//                }
            }
        }

        private void ctxMenuNodePermission_Opening(object sender, CancelEventArgs e) {
            cmiDelete.Enabled = lv.SelectedItems.Count > 0;
            cmiProperties.Enabled = lv.SelectedItems.Count == 1;

            int toEnable = 0;
            int toDisable = 0;
            foreach (ListViewItem lvi in lv.SelectedItems) {
                if (lvi.SubItems[2].Text == "Y") {
                    toDisable++;
                } else {
                    toEnable++;
                }
            }

            cmiEnable.Visible = toEnable > 0;
            cmiDisable.Visible = toDisable > 0;
        }

        private void lv_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void lv_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDelete();
            }
        }

        private void lv_MouseDoubleClick(object sender, MouseEventArgs e) {
            showProperties();
        }

        private void lv_SelectedIndexChanged_1(object sender, EventArgs e) {

        }

        private void frmTableMappings_Load(object sender, EventArgs e) {
            MakeListViewSortable(lv);
        }

        private void newMenuItem_Click(object sender, EventArgs e) {
            MainFormPopupNewItemForm(new frmFileGroup());

        }

        private void cmiFileGroupsExportList_Click(object sender, EventArgs e) {
            ExportListView(lv);

        }
        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmFileGroups", resourceName, null, defaultValue, substitutes);
        }
    }
}
