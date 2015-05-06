using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmCodeGroups : GrinGlobal.Admin.ChildForms.frmBase {
        public frmCodeGroups() {
            InitializeComponent();  tellBaseComponents(components);
        }


        public List<int> AssignedIDs = new List<int>();
        public List<int> SelectedIDs = new List<int>();


        public override void RefreshData() {

            //lvPerms.MultiSelect = !Modal;

            this.Text = "Code Groups - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
            var tags = rememberSelectedTags(lvGroups);
             
            // this form has multiple uses, as does the underlying ListPermissions method.
            // it can:
            // List all permissions (0,[0])
            // List permission info for a single permission (37,[0])
            // List permissions that are NOT in a list (0, [2,3,4,5,6])


            var dt = AdminProxy.ListCodeGroups(null).Tables["list_code_groups"];

            initHooksForMdiParent(dt, "group_name", "group_name");
            lvGroups.Items.Clear();
            foreach (DataRow dr in dt.Rows) {
                var lvi = new ListViewItem(dr["group_name"].ToString(), 0);
                lvi.ImageIndex = 0;
                lvi.SubItems.Add(dr["reference_count"].ToString());
                lvi.SubItems.Add(dr["value_count"].ToString());
                lvi.SubItems.Add((Toolkit.ToDateTime(dr["last_touched"], DateTime.MinValue)).ToLocalTime().ToString());
                lvi.Tag = dr["group_name"].ToString();
                lvGroups.Items.Add(lvi);
            }

            selectRememberedTags(lvGroups, tags);

            MainFormUpdateStatus(getDisplayMember("RefreshData{refreshed}", "Refreshed Code Groups"), false);
        }

        private void showProperties() {
            if (lvGroups.SelectedItems.Count == 1) {
                MainFormSelectChildTreeNode(lvGroups.SelectedItems[0].Tag.ToString());
            }
        }

        private void btnOK_Click(object sender, EventArgs e) {
            if (lvGroups.SelectedItems.Count == 0) {
                MessageBox.Show(getDisplayMember("ok", "You must select at least one group to add."));
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
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToDelete{delete_body}", "Deleting the code group(s) will cause all dataviews and tables that are referencing them to appear as free-form textboxes in the Curator Tool.\r\nAre you sure you want to delete code group(s)"), 
                    getDisplayMember("promptToDelete{delete_title}", "Delete Code Group(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    var delcount = 0;
                    foreach (ListViewItem lvi in lvGroups.SelectedItems) {
                        var cg = lvi.Tag as string;
                        if (Array.IndexOf(frmCodeGroup.RESERVED_CODE_GROUPS, cg) > -1) {
                            MessageBox.Show(this, 
                                getDisplayMember("promptToDelete{required_body}", "Code group {0} cannot be deleted because it is required by the system.", cg ), 
                                getDisplayMember("promptToDelete{required_title}", "Code Group Required"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        } else {
                            AdminProxy.DeleteCodeGroup(lvi.Tag as string);
                            delcount++;
                        }
                    }
                    MainFormUpdateStatus(getDisplayMember("promptToDelete{deleted}", "Deleted {0} code group(s).", delcount.ToString("###,###,##0")), true);
                    RefreshData();
                }
            }
        }

        private void ctxMenuNodePermission_Opening(object sender, CancelEventArgs e) {
            cmiDelete.Enabled = lvGroups.SelectedItems.Count > 0;
            cmiProperties.Enabled = lvGroups.SelectedItems.Count == 1;

        }

        private void newPermissionTemplateMenuItem_Click(object sender, EventArgs e) {
            MainFormPopupNewItemForm(new frmCodeGroup());
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
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmCodeGroups", resourceName, null, defaultValue, substitutes);
        }
    }
}
