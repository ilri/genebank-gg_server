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
    public partial class frmDataTriggers : GrinGlobal.Admin.ChildForms.frmBase {
        public frmDataTriggers() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public override void RefreshData() {

            //lv.MultiSelect = !Modal;

            this.Text = "Data Triggers - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
            var tags = rememberSelectedTags(lv);

            // this form has multiple uses, as does the underlying ListTableMappings method.
            // it can:
            // List all TableMappings (0,[0])
            // List permission info for a single permission (37,[0])
            // List TableMappings that are NOT in a list (0, [2,3,4,5,6])

            var ds = AdminProxy.ListTriggers(-1);

            var dt = ds.Tables["list_triggers"];

            initHooksForMdiParent(dt, "trigger_name", "sys_datatrigger_id");
            lv.Items.Clear();
            foreach (DataRow dr in DataTable.Rows) {
                var lvi = new ListViewItem(dr["dataview_name"].ToString(), 0);
                lvi.SubItems.Add(dr["table_name"].ToString());
                lvi.SubItems.Add(dr["assembly_name"].ToString());
                lvi.SubItems.Add(dr["fully_qualified_class_name"].ToString());
                lvi.SubItems.Add(dr["description"].ToString());
                lvi.SubItems.Add(dr["is_enabled"].ToString());
                // lvi.SubItems.Add(dr["is_system"].ToString());
                lvi.SubItems.Add(dr["virtual_file_path"].ToString());
                var lastTouched = dr["modified_date"] == DBNull.Value ? dr["created_date"] : dr["modified_date"];
                lvi.SubItems.Add(((DateTime)lastTouched).ToString());
                lvi.UseItemStyleForSubItems = false;

                lvi.Tag = dr["sys_datatrigger_id"].ToString();

                lv.Items.Add(lvi);

            }

            selectRememberedTags(lv, tags);

            MainFormUpdateStatus(getDisplayMember("RefreshData{refreshed}", "Refreshed Data Triggers"), false);
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
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToDelete{body}", "Are you sure you want to delete data trigger(s)?\nThe corresponding file will not be deleted."), 
                    getDisplayMember("promptToDelete{title}", "Delete Data Trigger(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lv.SelectedItems) {
                        AdminProxy.DeleteTrigger(Toolkit.ToInt32(lvi.Tag, -1));
                    }
                    MainFormUpdateStatus(getDisplayMember("promptToDelete{deleted}", "Deleted {0} Data Triggers.",  lv.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
                }
            }
        }

        private void disablePermissionMenuItem_Click(object sender, EventArgs e) {
            if (lv.SelectedItems.Count > 0) {
//                if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to disable permission(s)?", "Disable Permission(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lv.SelectedItems) {
                        AdminProxy.DisableTrigger(Toolkit.ToInt32(lvi.Tag, -1));
                    }
                    MainFormUpdateStatus(getDisplayMember("disablePermission{disabled}", "Disabled {0} Data Triggers.", lv.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
//                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            if (lv.SelectedItems.Count > 0) {
//                if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to enable permission(s)?", "Enable Permission(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lv.SelectedItems) {
                        AdminProxy.EnableTrigger(Toolkit.ToInt32(lvi.Tag, -1));
                    }
                    MainFormUpdateStatus(getDisplayMember("enableTrigger{enabled}", "Enabled {0} Data Triggers.", lv.SelectedItems.Count.ToString("###,##0")), true);
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
                if (lvi.SubItems[5].Text == "Y") {
                    toDisable++;
                } else {
                    toEnable++;
                }
            }

            cmiEnable.Enabled = toEnable > 0;
            cmiDisable.Enabled = toDisable > 0;
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

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e) {
            MainFormPopupNewItemForm(new frmDataTrigger());
        }

        private void lv_MouseDoubleClick(object sender, MouseEventArgs e) {
            showProperties();
        }

        private void lv_SelectedIndexChanged_1(object sender, EventArgs e) {

        }

        private void frmTableMappings_Load(object sender, EventArgs e) {
            MakeListViewSortable(lv);
        }

        private void lv_MouseDoubleClick_1(object sender, MouseEventArgs e) {
            showProperties();
        }

        private void toolStripMenuItem1_Click_2(object sender, EventArgs e) {
            ExportListView(lv);

        }

        private void cmiDataTriggersImport_Click(object sender, EventArgs e) {
            if (MainFormPopupForm(new frmImportDataTrigger(), this, false) == DialogResult.OK) {
                RefreshData();
            }
        }

        private void lv_KeyUp_1(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDelete();
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmDataTriggers", resourceName, null, defaultValue, substitutes);
        }
    }
}
