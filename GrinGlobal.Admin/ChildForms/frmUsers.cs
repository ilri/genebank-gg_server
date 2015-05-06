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
    public partial class frmUsers : GrinGlobal.Admin.ChildForms.frmBase {
        public frmUsers() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public List<int> AssignedIDs = new List<int>();
        public List<int> SelectedIDs = new List<int>();


        private void frmUsers_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvUsers);
            if (!Modal) {
                lvUsers.Dock = DockStyle.Fill;
                btnAdd.Visible = false;
                btnCancel.Visible = false;
                btnNew.Visible = false;
            }
        }

        public override void RefreshData(){

            this.Text = "Users - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
            var tags = rememberSelectedTags(lvUsers);

            updateSplashText("listing users...");
            var dt = AdminProxy.ListUsers(-1).Tables["list_users"];

            initHooksForMdiParent(dt, "user_name", "sys_user_id");

            lvUsers.Items.Clear();
            foreach (DataRow dr in DataTable.Rows) {
                var lvi = new ListViewItem(new string[] { 
                dr["user_name"].ToString(), 
                dr["full_name"].ToString(), 
                dr["is_admin"].ToString(), 
                dr["is_enabled"].ToString(), 
                dr["language_name"].ToString(),
                dr["web_user_name"].ToString() });
                lvi.ImageIndex = 0;
                lvi.Tag = dr["sys_user_id"];
                lvUsers.Items.Add(lvi);
            }

            selectRememberedTags(lvUsers, tags);

            MainFormUpdateStatus(getDisplayMember("RefreshData{done}", "Refreshed Users"), false);
        }

        private bool _syncing;
        private void syncGUI() {
            if (!_syncing) {
                try {
                    _syncing = true;
                    // TODO: sync gui stuff here
                } finally {
                    _syncing = false;
                }
            }
        }


        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            promptToDeleteIfNeeded();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lvUsers.SelectedItems.Count > 0) {
                MainFormSelectChildTreeNode(lvUsers.SelectedItems[0].Tag.ToString());
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e) {

            cmiProperties.Enabled = lvUsers.SelectedItems.Count == 1;
            cmiChangePassword.Enabled = lvUsers.SelectedItems.Count == 1;
            cmiDelete.Enabled = lvUsers.SelectedItems.Count > 0;

            int toEnable = 0;
            int toDisable = 0;
            foreach (ListViewItem lvi in lvUsers.SelectedItems) {
                if (lvi.SubItems[3].Text == "Y") {
                    toDisable++;
                } else {
                    toEnable++;
                }
            }

            cmiEnable.Visible = toEnable > 0;
            cmiDisable.Visible = toDisable > 0;


        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            if (lvUsers.SelectedItems.Count > 0) {
//                if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to disable the selected user(s)?", "Disable User(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    using (new AutoCursor(this)) {
                        var disabledCount = 0;
                        foreach (ListViewItem lvi in lvUsers.SelectedItems) {
                            try {
                                AdminProxy.DisableUser(Toolkit.ToInt32(lvi.Tag, -1));
                                disabledCount++;
                            } catch (Exception ex) {
                                MessageBox.Show(this, ex.Message, getDisplayMember("disable{faield}", "Error Disabling User"));
                            }
                        }
                        RefreshData();
                        MainFormUpdateStatus(getDisplayMember("disable{done}", "Disabled {0} user(s)", disabledCount .ToString("###,##0")), true);
                    }
//                }
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void enableUserToolStripMenuItem_Click(object sender, EventArgs e) {
            using (new AutoCursor(this)) {
                foreach (ListViewItem lvi in lvUsers.SelectedItems) {
                    AdminProxy.EnableUser(Toolkit.ToInt32(lvi.Tag, -1));
                }
                RefreshData();
                MainFormUpdateStatus(getDisplayMember("enable{done}", "Enabled {0} user(s)", lvUsers.SelectedItems.Count.ToString("###,##0")), true);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            string username = null;
            switch (lvUsers.SelectedItems.Count) {
                case 0:
                    MessageBox.Show(getDisplayMember("changedPassword{oneuserrequired}", "Please select at least one user to change a password for."));
                    return;
                case 1:
                    username = lvUsers.SelectedItems[0].Text;
                    break;
                default:
                    username = "multiple users";
                    break;
            }
            var f = new frmSetPassword();

            if (f.ShowDialog(this, username) == DialogResult.OK){
                using (new AutoCursor(this)) {
                    foreach (ListViewItem lvi in lvUsers.SelectedItems) {
                        AdminProxy.ChangePassword(Toolkit.ToInt32(lvi.Tag, -1), f.Password);
                    }
                    MainFormUpdateStatus(getDisplayMember("changedPassword{done}", "Changed password for {0} user(s)", lvUsers.SelectedItems.Count.ToString("###,##0")), true);
                    MainFormRefreshData();
                }
            }
        }


        private void toolStripMenuItem3_Click(object sender, EventArgs e) {
            MainFormPopupNewItemForm(new frmUser());
        }

        private void lvUsers_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lvUsers.SelectedItems.Count == 1) {
                if (Modal) {
                    btnAdd.PerformClick();
                } else {
                    MainFormSelectChildTreeNode(lvUsers.SelectedItems[0].Tag.ToString());
                }
            }
        }

        private void lvUsers_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void promptToDeleteIfNeeded() {
            if (lvUsers.SelectedItems.Count > 0) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToDeleteIfNeeded{start_body}", "Are you sure you want to delete the selected user(s)?"),
                    getDisplayMember("promptToDeleteIfNeeded{start_title}", "Delete User(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    using (new AutoCursor(this)) {
                        var count = 0;
                        foreach (ListViewItem lvi in lvUsers.SelectedItems) {
                            try {
                                AdminProxy.DeleteUser(Toolkit.ToInt32(lvi.Tag, -1));
                                count++;
                            } catch (Exception ex) {
                                MessageBox.Show(this, ex.Message, getDisplayMember("promptToDeleteIfNeeded{failed}", "Error Deleting User"));
                            }
                        }
                        MainFormRefreshData();
                        MainFormUpdateStatus(getDisplayMember("promptToDeleteIfNeeded{done}", "Deleted {0} user(s)", count.ToString("###,##0")), true);
                    }
                }
            }
        }

        private void lvUsers_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDeleteIfNeeded();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            if (lvUsers.SelectedItems.Count == 0) {
                MessageBox.Show(getDisplayMember("add{atleastone}", "You must select one or more users to add."));
            } else {
                SelectedIDs.Clear();
                foreach (ListViewItem lvi in lvUsers.SelectedItems) {
                    SelectedIDs.Add(Toolkit.ToInt32(lvi.Tag, -1));
                }
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnNew_Click(object sender, EventArgs e) {
            if (DialogResult.OK == MainFormPopupForm(new frmUser(), this, true)) {
                RefreshData();
            }

        }

        private void lvUsers_ItemDrag(object sender, ItemDragEventArgs e) {
            startDrag(sender, "ndUsers");
        }

        private void cmiUsersExportList_Click(object sender, EventArgs e) {
            ExportListView(lvUsers);

        }


        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmUsers", resourceName, null, defaultValue, substitutes);
        }
    }
}
