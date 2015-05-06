using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using GrinGlobal.InstallHelper;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmPermissions : GrinGlobal.Admin.ChildForms.frmBase {
        public frmPermissions() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public override void RefreshData() {

            //lvPerms.MultiSelect = !Modal;

            this.Text = "Permissions - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
            var tags = rememberSelectedTags(lvPerms);

            // this form has multiple uses, as does the underlying ListPermissions method.
            // it can:
            // List all permissions (0,[0])
            // List permission info for a single permission (37,[0])
            // List permissions that are NOT in a list (0, [2,3,4,5,6])

            // var ds = AdminProxy.ListPermissions(-1, AssignedIDs, AssignedIDs.Count > 0);
            var ds = AdminProxy.ListPermissions(-1, AssignedIDs, false);

            var dt = ds.Tables["list_permissions"];

            initHooksForMdiParent(dt, "display_member", "sys_permission_id");
            lvPerms.Items.Clear();
            foreach (DataRow dr in DataTable.Rows) {

                var lvi = new ListViewItem(dr["display_member"].ToString(), 0);


                string dv = dr["dataview_name"].ToString();
                string tbl = dr["table_name"].ToString();
                if (String.IsNullOrEmpty(dv) && String.IsNullOrEmpty(tbl)) {
                    lvi.SubItems.Add("-- any resource --");
                } else {
                    if (!String.IsNullOrEmpty(dv)) {
                        lvi.SubItems.Add(dv + " (dataview)");
                    } else {
                        lvi.SubItems.Add(tbl);
                    }
                }
                lvi.SubItems.Add(dr["is_enabled"].ToString());

                lvi.UseItemStyleForSubItems = false;

                addPermItem(lvi, dr["create_permission_text"].ToString());
                addPermItem(lvi, dr["read_permission_text"].ToString());
                addPermItem(lvi, dr["update_permission_text"].ToString());
                addPermItem(lvi, dr["delete_permission_text"].ToString());

                // get restriction info...
                var drFields = ds.Tables["list_permission_fields"].Select("sys_permission_id = " + dr["sys_permission_id"]);
                if (drFields == null || drFields.Length == 0) {
                    lvi.SubItems.Add("-");
                } else {
                    // pull perm field info into english-like display...
                    var sb = new StringBuilder();
                    foreach (var drField in drFields) {
                        if (sb.Length > 0) {
                            sb.Append(" and ");
                        }
                        if (drField["compare_mode"].ToString() == "parent") {
                            sb.Append(drField["parent_table_name"] + "." + drField["parent_table_field_name"] + " " + drField["parent_compare_operator"] + " " + drField["parent_compare_value"] + " (for all children)");
                        } else {
                            if (Toolkit.ToInt32(drField["sys_dataview_field_id"], -1) > -1) {
                                sb.Append(drField["dataview_name"] + "." + drField["dataview_field_name"] + " " + drField["compare_operator"] + " " + drField["compare_value"] + " (dataview)");
                            } else if (Toolkit.ToInt32(drField["sys_table_field_id"], -1) > -1) {
                                sb.Append(drField["table_name"] + "." + drField["table_field_name"] + " " + drField["compare_operator"] + " " + drField["compare_value"] + " ");
                            } else {
                                sb.Append("INVALID -- must map a field from either Data View or Table!!!");
                            }
                        }
                    }
                    lvi.SubItems.Add(sb.ToString());
                }


                lvi.ImageIndex = 0;
                lvi.Tag = dr["sys_permission_id"];
                lvPerms.Items.Add(lvi);
            }

            selectRememberedTags(lvPerms, tags);

            MainFormUpdateStatus(getDisplayMember("RefreshData{done}", "Refreshed Permissions"), false);
        }

        private void showProperties() {
            if (lvPerms.SelectedItems.Count == 1) {
                MainFormSelectChildTreeNode(lvPerms.SelectedItems[0].Tag.ToString());
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
            if (lvPerms.SelectedItems.Count > 0) {

                var total = 0;
                var fmb = new frmMessageBox();
                fmb.Text = "Delete Permission(s)?";
                fmb.btnYes.Text = "&Delete";
                fmb.btnNo.Text = "&Cancel";

                var list = new List<string>();
                foreach (ListViewItem lvi in lvPerms.SelectedItems) {
                    list.Add(lvi.Text);
                }


                fmb.txtMessage.Text = "Are you sure you want to delete the following permission(s)?\r\n" + String.Join("\r\n", list.ToArray());
                if (DialogResult.Yes == fmb.ShowDialog(this)) {
                    foreach (ListViewItem lvi in lvPerms.SelectedItems) {
                        var id = Toolkit.ToInt32(lvi.Tag, -1);
                        try {
                            AdminProxy.DeletePermission(id, false);
                            total++;
                        } catch (Exception ex) {
                            if (ex.Message.Contains("are referencing")) {
                                fmb.txtMessage.Text = ex.Message + "\r\n\r\nDo you want to remove the reference(s) and continue deleting " + lvi.Text + "?";
                                fmb.Text = "Remove References and Continue Delete?";
                                fmb.btnYes.Text = "Continue &Deleting";
                                fmb.btnNo.Text = "&Cancel";
                                if (DialogResult.Yes == fmb.ShowDialog(this)) {
                                    AdminProxy.DeletePermission(id, true);
                                    total++;
                                } else {
                                    // nothing to do
                                }
                            } else {
                                throw;
                            }
                        }
                    }
                    MainFormRefreshData();
                    MainFormUpdateStatus(getDisplayMember("promptToDelete{done}", "Deleted {0} permission(s)", total.ToString("###,##0")), true);
                }



                //if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to delete permission(s)?", "Delete Permission(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                //    foreach (ListViewItem lvi in lvPerms.SelectedItems) {
                //        var id = Toolkit.ToInt32(lvi.Tag, -1);
                //        try {
                //            AdminProxy.DeletePermission(id, false);
                //        } catch (InvalidOperationException ioe) {
                //        }
                //    }
                //    MainFormUpdateStatus("Deleted " + lvPerms.SelectedItems.Count + " permissions.", true);
                //    RefreshData();
                //}
            }
        }

        private void disablePermissionMenuItem_Click(object sender, EventArgs e) {
            if (lvPerms.SelectedItems.Count > 0) {
//                if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to disable permission(s)?", "Disable Permission(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lvPerms.SelectedItems) {
                        AdminProxy.DisablePermission(Toolkit.ToInt32(lvi.Tag, -1));
                    }
                    MainFormUpdateStatus(getDisplayMember("disablePermission{done}", "Disabled {0} permission(s)", lvPerms.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
//                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            if (lvPerms.SelectedItems.Count > 0) {
//                if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to enable permission(s)?", "Enable Permission(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lvPerms.SelectedItems) {
                        AdminProxy.EnablePermission(Toolkit.ToInt32(lvi.Tag, -1));
                    }
                    MainFormUpdateStatus(getDisplayMember("enable{done}", "Enabled {0} permission(s)", lvPerms.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
//                }
            }
        }

        private void ctxMenuNodePermission_Opening(object sender, CancelEventArgs e) {
            cmiDelete.Enabled = lvPerms.SelectedItems.Count > 0;
            cmiProperties.Enabled = lvPerms.SelectedItems.Count == 1;

            int toEnable = 0;
            int toDisable = 0;
            foreach (ListViewItem lvi in lvPerms.SelectedItems) {
                if (lvi.SubItems[2].Text == "Y") {
                    toDisable++;
                } else {
                    toEnable++;
                }
            }

            cmiEnable.Visible = toEnable > 0;
            cmiDisable.Visible = toDisable > 0;
        }

        private void lvPerms_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void lvPerms_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (Modal) {
                btnOK.PerformClick();
            } else {
                showProperties();
            }
        }

        private void frmPermissions_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvPerms);
            if (!Modal) {
                btnOK.Visible = false;
                btnCancel.Visible = false;
                lvPerms.Dock = DockStyle.Fill;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        public List<int> AssignedIDs = new List<int>();
        public List<int> SelectedIDs = new List<int>();

        private void btnOK_Click(object sender, EventArgs e) {
            if (lvPerms.SelectedItems.Count == 0) {
                MessageBox.Show(getDisplayMember("ok{atleastonepermission}", "You must select one or more permissions to add."));
            } else {
                SelectedIDs.Clear();
                foreach (ListViewItem lvi in lvPerms.SelectedItems) {
                    SelectedIDs.Add(Toolkit.ToInt32(lvi.Tag, -1));
                }
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void lvPerms_ItemDrag(object sender, ItemDragEventArgs e) {
            startDrag(sender);
        }

        private void lvPerms_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDelete();
            }
        }

        private void lvPerms_KeyPress(object sender, KeyPressEventArgs e) {
            if ((Keys)e.KeyChar == Keys.Enter) {
                btnOK.PerformClick();
            }
        }

        private void newPermissionMenuItem_Click(object sender, EventArgs e) {
            MainFormPopupNewItemForm(new frmPermission());
        }

        private void cmiPermissionExportList_Click(object sender, EventArgs e) {
            ExportListView(lvPerms);

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmPermissions", resourceName, null, defaultValue, substitutes);
        }
    }
}
