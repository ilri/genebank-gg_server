using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using GrinGlobal.Admin.PopupForms;
using GrinGlobal.InstallHelper;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmTableMappings : GrinGlobal.Admin.ChildForms.frmBase {
        public frmTableMappings() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public override void RefreshData() {

            //lvMappings.MultiSelect = !Modal;

            this.Text = "Table Mappings - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
            var tags = rememberSelectedTags(lvMappings);

            // this form has multiple uses, as does the underlying ListTableMappings method.
            // it can:
            // List all TableMappings (0,[0])
            // List permission info for a single permission (37,[0])
            // List TableMappings that are NOT in a list (0, [2,3,4,5,6])

            var ds = AdminProxy.ListTables(-1);

            var dt = ds.Tables["list_tables"];

            initHooksForMdiParent(dt, "table_name", "sys_table_id");
            lvMappings.Items.Clear();
            foreach (DataRow dr in DataTable.Rows) {
                var lvi = new ListViewItem(dr["table_name"].ToString(), 0);
                lvi.SubItems.Add(dr["is_enabled"].ToString());
                lvi.SubItems.Add(dr["database_area_code"].ToString());
                var lastTouched = dr["modified_date"] == DBNull.Value ? dr["created_date"] : dr["modified_date"];
                lvi.SubItems.Add(((DateTime)lastTouched).ToString());
                lvi.UseItemStyleForSubItems = false;

                lvi.Tag = dr["sys_table_id"].ToString();

                lvMappings.Items.Add(lvi);

            }

            selectRememberedTags(lvMappings, tags);

            MainFormUpdateStatus(getDisplayMember("RefreshData{done}", "Refreshed Table Mappings"), false);
        }

        private void showProperties() {
            if (lvMappings.SelectedItems.Count == 1) {
                MainFormSelectChildTreeNode(lvMappings.SelectedItems[0].Tag.ToString());
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
            if (lvMappings.SelectedItems.Count > 0) {


                var total = 0;

                var fmb = new frmMessageBox();
                fmb.btnYes.Text = "&Delete";
                fmb.btnNo.Text = "&Cancel";
                fmb.Text = "Delete Table Mapping(s)?";
                var sb = new StringBuilder();
                sb.AppendLine("Are you sure you want to delete table mappings for the following tables?\r\n");
                foreach (ListViewItem lvi in lvMappings.SelectedItems) {
                    sb.AppendLine("  " + lvi.Text);
                }
                fmb.txtMessage.Text = sb.ToString();

                if (DialogResult.Yes == fmb.ShowDialog(this)){
                //if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to delete table mapping(s)?", "Delete Table Mapping(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lvMappings.SelectedItems) {
                        try {
                            AdminProxy.DeleteTableMapping(Toolkit.ToInt32(lvi.Tag, -1), false);
                            total++;
                        } catch (Exception ex) {
                            if (ex.Message.Contains("delete the table mapping")) {
                                fmb.btnYes.Text = "Continue &Deleting";
                                fmb.Text = "Remove References and Continue Delete?";
                                fmb.txtMessage.Text = ex.Message + "\r\n\r\nDo you want to remove the reference(s) and continue deleting " + lvi.Text + "?";
                                if (DialogResult.Yes == fmb.ShowDialog(this)) {
                                //if (DialogResult.Yes == MessageBox.Show(this, ex.Message + "\r\n\r\nDo you want to remove the reference(s) and continue deleting " + lvi.Text + "?", "Remove References and Continue Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                                    AdminProxy.DeleteTableMapping(Toolkit.ToInt32(lvi.Tag, -1), true);
                                    total++;
                                } else {
                                    // nothing to do
                                }
                            } else {
                                throw;
                            }
                        }
                    }

                    MainFormUpdateStatus(getDisplayMember("promptToDelete{done}", "Deleted {0} table mapping(s)", total.ToString("###,##0")), true);
                    RefreshData();
                }
            }
        }

        private void disablePermissionMenuItem_Click(object sender, EventArgs e) {
            if (lvMappings.SelectedItems.Count > 0) {
//                if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to disable permission(s)?", "Disable Permission(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lvMappings.SelectedItems) {
                        AdminProxy.DisableTableMapping(Toolkit.ToInt32(lvi.Tag, -1));
                    }
                    MainFormUpdateStatus(getDisplayMember("disableMapping{done}", "Disabled {0} table mapping(s)", lvMappings.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
//                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            if (lvMappings.SelectedItems.Count > 0) {
//                if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to enable permission(s)?", "Enable Permission(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lvMappings.SelectedItems) {
                        AdminProxy.EnableTableMapping(Toolkit.ToInt32(lvi.Tag, -1));
                    }
                    MainFormUpdateStatus(getDisplayMember("enable{done}", "Enabled {0} table mapping(s)", lvMappings.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
//                }
            }
        }

        private void ctxMenuNodePermission_Opening(object sender, CancelEventArgs e) {
            cmiDelete.Enabled = lvMappings.SelectedItems.Count > 0;
            cmiProperties.Enabled = lvMappings.SelectedItems.Count == 1;
            cmiRebuild.Enabled = lvMappings.SelectedItems.Count > 0;

            int toEnable = 0;
            int toDisable = 0;
            foreach (ListViewItem lvi in lvMappings.SelectedItems) {
                if (lvi.SubItems[1].Text == "Y") {
                    toDisable++;
                } else {
                    toEnable++;
                }
            }

            cmiEnable.Visible = toEnable > 0;
            cmiDisable.Visible = toDisable > 0;
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

        private void newPermissionMenuItem_Click(object sender, EventArgs e) {
            MainFormPopupNewItemForm(new frmTableMapping());
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e) {
            MainFormPopupNewItemForm(new frmTableMappingInspectSchema());
        }

        private void lvMappings_MouseDoubleClick(object sender, MouseEventArgs e) {
            showProperties();
        }

        private void lvMappings_SelectedIndexChanged_1(object sender, EventArgs e) {

        }

        private void frmTableMappings_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvMappings);
        }

        private void cmiRebuild_Click(object sender, EventArgs e) {
            var f = new frmTableMappingInspectSchema();

            foreach (ListViewItem lvi in lvMappings.SelectedItems) {
                f.TableNames.Add(lvi.Text);
            }
            if (f.TableNames.Count > 0) {
                if (DialogResult.OK == MainFormPopupForm(f, this, false)) {
                }
            }

        }

        private void cmiTableExportList_Click(object sender, EventArgs e) {
            ExportListView(lvMappings);

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmTableMappings", resourceName, null, defaultValue, substitutes);
        }
    }
}
