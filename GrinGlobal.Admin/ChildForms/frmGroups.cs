using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmGroups : GrinGlobal.Admin.ChildForms.frmBase {
        public frmGroups() {
            InitializeComponent();  tellBaseComponents(components);
        }


        public List<int> AssignedIDs = new List<int>();
        public List<int> SelectedIDs = new List<int>();


        public override void RefreshData() {

            //lvPerms.MultiSelect = !Modal;

            this.Text = "Groups - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
            var tags = rememberSelectedTags(lvGroups);

            // this form has multiple uses, as does the underlying ListPermissions method.
            // it can:
            // List all permissions (0,[0])
            // List permission info for a single permission (37,[0])
            // List permissions that are NOT in a list (0, [2,3,4,5,6])


            var ds = AdminProxy.ListGroups(ID);

            initHooksForMdiParent(ds.Tables["list_groups"], "group_name", "sys_group_id");
            lvGroups.Items.Clear();
            foreach (DataRow dr in ds.Tables["list_groups"].Rows) {
                var lvi = new ListViewItem(dr["group_name"].ToString(), 0);
                lvi.Tag = Toolkit.ToInt32(dr["sys_group_id"], -1);
                lvi.SubItems.Add(dr["description"].ToString());
                DataRow[] drPerms = ds.Tables["permissions_by_group"].Select("sys_group_id = " + dr["sys_group_id"]);
                var perms = new List<string>();
                foreach (DataRow drp in drPerms) {
                    perms.Add(drp["permission_name"].ToString());
                }
                if (perms.Count == 0) {
                    lvi.SubItems.Add("(None)");
                } else {
                    lvi.SubItems.Add("(" + perms.Count + ") - " + Toolkit.Join(perms.ToArray(), ", ", "-"));
                }

                DataRow[] drUsers = ds.Tables["users_by_group"].Select("sys_group_id = " + dr["sys_group_id"]);
                var users = new List<string>();
                foreach (DataRow dru in drUsers) {
                    users.Add(dru["user_name"].ToString());
                }

                if (users.Count == 0) {
                    lvi.SubItems.Add("(None)");
                } else {
                    lvi.SubItems.Add("(" + users.Count + ") - " + Toolkit.Join(users.ToArray(), ", ", "-"));
                }
                
                lvi.Tag = dr["sys_group_id"];
                lvGroups.Items.Add(lvi);
            }

            selectRememberedTags(lvGroups, tags);

            MainFormUpdateStatus(getDisplayMember("RefreshData{done}", "Refreshed Groups"), false);
        }

        private void showProperties() {
            if (lvGroups.SelectedItems.Count == 1) {
                MainFormSelectChildTreeNode(lvGroups.SelectedItems[0].Tag.ToString());
            }
        }

        private void btnOK_Click(object sender, EventArgs e) {
            if (lvGroups.SelectedItems.Count == 0) {
                MessageBox.Show(getDisplayMember("ok{click}", "You must select at least one group to add."));
            } else {
                SelectedIDs.Clear();
                foreach (ListViewItem lvi in lvGroups.SelectedItems) {
                    SelectedIDs.Add(Toolkit.ToInt32(lvi.Tag, -1));
                }
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();

        }

        private void frmGroups_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvGroups);
            if (!Modal) {
                btnOK.Visible = false;
                btnCancel.Visible = false;
                lvGroups.Dock = DockStyle.Fill;
            }
        }

        private void defaultPermissionTemplateMenuItem_Click(object sender, EventArgs e) {
            showProperties();

        }

        private void refreshPermissionTemplateMenuItem_Click(object sender, EventArgs e) {
            RefreshData();

        }

        private void deletePermissionTemplateMenuItem_Click(object sender, EventArgs e) {
            promptToDelete();

        }

        private void promptToDelete() {
            if (lvGroups.SelectedItems.Count > 0) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToDelete{start_body}", "Are you sure you want to delete group(s)?\n\nThis will not delete any permissions or users or alter any user-specific permissions in any way."), 
                    getDisplayMember("promptToDelete{start_title}", "Delete Group(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lvGroups.SelectedItems) {
                        AdminProxy.DeleteGroup(Toolkit.ToInt32(lvi.Tag, -1));
                    }
                    MainFormUpdateStatus(getDisplayMember("promptToDelete{done}", "Deleted {0} group(s)", lvGroups.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
                }
            }
        }

        private void ctxMenuNodePermission_Opening(object sender, CancelEventArgs e) {
            cmiDelete.Enabled = lvGroups.SelectedItems.Count > 0;
            cmiProperties.Enabled = lvGroups.SelectedItems.Count == 1;

        }

        private void newPermissionTemplateMenuItem_Click(object sender, EventArgs e) {
            MainFormPopupNewItemForm(new frmGroup());
        }

        private void lvGroups_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (Modal) {
                btnOK.PerformClick();
            } else {
                showProperties();
            }

        }

        private void lvGroups_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDelete();
            }

        }

        private void lvGroups_KeyPress(object sender, KeyPressEventArgs e) {
            if ((Keys)e.KeyChar == Keys.Enter) {
                btnOK.PerformClick();
            }

        }

        private void lvGroups_ItemDrag(object sender, ItemDragEventArgs e) {
            startDrag(sender);
        }

        private void cmiGroupExportList_Click(object sender, EventArgs e) {
            ExportListView(lvGroups);

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmGroups", resourceName, null, defaultValue, substitutes);
        }
    }
}
