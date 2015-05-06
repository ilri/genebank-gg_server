using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmGroup : GrinGlobal.Admin.ChildForms.frmBase {
        public frmGroup() {
            InitializeComponent();  tellBaseComponents(components);

            PrimaryTabControl = tcPermsUsers;
        }

        public override void RefreshData() {
            this.Text = "Group - " + MainFormCurrentNodeText("") + " - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

            var ds = AdminProxy.ListGroups(ID < 0 ? int.MaxValue : ID);

            // fill permissions tab
            lvPermissions.Items.Clear();

            foreach (DataRow dr in ds.Tables["list_groups"].Rows) {

                txtName.Text = dr["group_name"].ToString();
                txtTag.Text = dr["group_tag"].ToString();

                if (AdminProxy.IsReservedGroupTagValue(txtTag.Text)) {
                    txtTag.Enabled = false;
                } else {
                    txtTag.Enabled = true;
                }

                txtDescription.Text = dr["description"].ToString();

                DataRow[] drPerms = ds.Tables["permissions_by_group"].Select("sys_group_id = " + dr["sys_group_id"]);
                if (drPerms != null && drPerms.Length > 0){
                    foreach(DataRow drp in drPerms){
                        var lvi = new ListViewItem(drp["title"].ToString(), 0);
                        lvi.Tag = Toolkit.ToInt32(drp["sys_permission_id"], -1);
                        lvi.UseItemStyleForSubItems = false;
                        string dv = drp["dataview_name"].ToString();
                        string tbl = drp["table_name"].ToString();
                        if (String.IsNullOrEmpty(dv) && String.IsNullOrEmpty(tbl)) {
                            lvi.SubItems.Add("-- any resource --");
                        } else {
                            if (!String.IsNullOrEmpty(dv)) {
                                lvi.SubItems.Add(dv + " (dataview)");
                            } else {
                                lvi.SubItems.Add(tbl);
                            }
                        }
                        lvi.SubItems.Add(drp["perm_is_enabled"].ToString().ToUpper());
                        addPermItem(lvi, drp["create_permission_text"].ToString());
                        addPermItem(lvi, drp["read_permission_text"].ToString());
                        addPermItem(lvi, drp["update_permission_text"].ToString());
                        addPermItem(lvi, drp["delete_permission_text"].ToString());

                        // get restriction info...
                        var drFields = ds.Tables["group_perm_field_info"].Select("sys_permission_id = " + drp["sys_permission_id"]);
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
                                    } else if (Toolkit.ToInt32(drField["sys_table_field_id"], 1) > -1) {
                                        sb.Append(drField["table_name"] + "." + drField["table_field_name"] + " " + drField["compare_operator"] + " " + drField["compare_value"] + " ");
                                    } else {
                                        sb.Append("INVALID -- must map a field from either Data View or Table!!!");
                                    }
                                }
                            }
                            lvi.SubItems.Add(sb.ToString());
                        }
                        lvPermissions.Items.Add(lvi);
                    }
                }


                lvUsers.Items.Clear();
                var dtUsers = ds.Tables["users_by_group"];
                if (dtUsers != null) {
                    var drUsers = dtUsers.Select("sys_group_id = " + dr["sys_group_id"]);

                    foreach (DataRow drUser in drUsers) {
                        var lvi = new ListViewItem(new string[] { 
                        drUser["user_name"].ToString(), 
                        drUser["full_name"].ToString(), 
                        drUser["is_enabled"].ToString() });
                        lvi.ImageIndex = 1;
                        lvi.Tag = drUser["sys_user_id"];
                        lvUsers.Items.Add(lvi);
                    }
                }



            }

            MarkClean();

        }




        private void editPermissionToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lvPermissions.SelectedItems.Count == 1) {
                MainFormSelectCousinTreeNode("ndPermissions", lvPermissions.SelectedItems[0].Tag.ToString());
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            removeSelectedPermissions(true);
        }

        private void removeSelectedPermissions(bool prompt) {

            if (lvPermissions.SelectedItems.Count == 0) {
                return;
            }

            if (prompt) {
                if (DialogResult.Yes != MessageBox.Show(this, getDisplayMember("promptToDelete{prompt_body}", "You are about to permanently remove these permission(s) from this group.\nDo you want to continue?"),
                    getDisplayMember("promptToDelete{prompt_title}", "Remove Permissions?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    return;
                }
            }

            List<int> ids = new List<int>();
            foreach (ListViewItem lvi in lvPermissions.SelectedItems) {
                ids.Add(Toolkit.ToInt32(lvi.Tag, -1));
            }
            AdminProxy.RemovePermissionsFromGroup(ID, ids);
            RefreshData();
        }

        private void addPermissionToolStripItem_Click(object sender, EventArgs e) {
            popupAddPermission();

        }

        private void popupAddPermission() {

            if (ID < 1) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("popupAddPermission{prompt_body}", "You must save the group before adding permissions to it.\nDo you want to save it now?"), 
                    getDisplayMember("popupAddPermission{prompt_title}", "Save Group?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    save(false);
                } else {
                    return;
                }
            }
            
            frmPermissions fp = new frmPermissions();
            foreach (ListViewItem lvi in lvPermissions.Items) {
                fp.AssignedIDs.Add(Toolkit.ToInt32(lvi.Tag, -1));
            }
            if (DialogResult.OK == MainFormPopupForm(fp, this, true)) {
                // add selected permission to user, refresh
                AdminProxy.AddPermissionsToGroup(ID, fp.SelectedIDs);
                RefreshData();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            if (Modal) {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            } else {
                MainFormSelectParentTreeNode();
            }

        }

        private void btnAddPermission_Click(object sender, EventArgs e) {

            popupAddPermission();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            save(true);
        }

        private void save(bool closeAfterSave){

            var groupIDForTag = AdminProxy.GetGroupIDForTag(txtTag.Text);
            //if (String.IsNullOrEmpty(txtTag.Text)){
            //    MessageBox.Show(this, "You must specify a tag for the group.", "Tag Required");
            //    txtTag.Focus();
            //    return;
            //} else if (groupIDForTag > -1 && groupIDForTag != ID) {
            //    MessageBox.Show(this, "You must specify a unique tag for the group.\n" + txtTag.Text + " is already taken.", "Unique Tag Required");
            //    txtTag.Focus();
            //    txtTag.SelectAll();
            //    return;
            //}

            if (AdminProxy.IsReservedGroupTagValue(txtTag.Text) && txtTag.Enabled) {
                MessageBox.Show(this, getDisplayMember("save{reservedtag_body}", "{0} is a reserved tag name and cannot be used.\nPlease specify a different value.", txtTag.Text ), 
                    getDisplayMember("save{reservedtag_body}", "Reserved Tag"));
                return;
            }

            ID = AdminProxy.SaveGroup(ID, txtTag.Text, txtName.Text, txtDescription.Text);

            RefreshData();
            MainFormUpdateStatus(getDisplayMember("save{done}", "Saved group {0}", txtName.Text), true);

            if (closeAfterSave) {
                if (Modal) {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                } else {
                    MainFormSelectParentTreeNode();
                }
            }
        
        }

        private void lvPermissions_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lvPermissions.SelectedItems.Count == 1) {
                MainFormSelectCousinTreeNode("ndPermissions", lvPermissions.SelectedItems[0].Tag.ToString());
            }
        }

        private void lvPermissions_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                removeSelectedPermissions(true);
            }
        }

        private void lvPermissions_KeyPress(object sender, KeyPressEventArgs e) {

        }

        private void frmPermissionTemplate_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvPermissions);
            MakeListViewSortable(lvUsers);
        }

        private void defaultUserMenuItem_Click(object sender, EventArgs e) {

        }

        private void addUserMenuItem_Click(object sender, EventArgs e) {
            popupAddUser();

        }

        private void popupAddUser() {
            if (ID < 1) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("popupAddUser{prompt_body}", "You must save the group before adding users to it.\nDo you want to save it now?"), 
                    getDisplayMember("popupAddUser{prompt_title}", "Save Group?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    save(false);
                } else {
                    return;
                }
            }

            var fu = new frmUsers();
            foreach (ListViewItem lvi in lvUsers.Items) {
                fu.AssignedIDs.Add(Toolkit.ToInt32(lvi.Tag, -1));
            }
            if (DialogResult.OK == MainFormPopupForm(fu, this, true)) {
                // add selected permission to user, refresh
                AdminProxy.AddUsersToGroup(ID, fu.SelectedIDs);
                RefreshData();
            }

        }

        private void removeUserMenuItem_Click(object sender, EventArgs e) {
            removeSelectedUsers(true);
        }

        private void removeSelectedUsers(bool prompt) {
            if (lvUsers.SelectedItems.Count == 0) {
                return;
            }

            if (prompt) {
                if (DialogResult.Yes != MessageBox.Show(this, getDisplayMember("removeSelectedUsers{prompt_body}", "You are about to permanently remove these user(s) from this group.\nDo you want to continue?"), 
                    getDisplayMember("removeSelectedUsers{prompt_title}", "Remove Users?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    return;
                }
            }

            List<int> ids = new List<int>();
            foreach (ListViewItem lvi in lvUsers.SelectedItems) {
                ids.Add(Toolkit.ToInt32(lvi.Tag, -1));
            }
            try {
                AdminProxy.RemoveUsersFromGroup(ID, ids);
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, getDisplayMember("removeSelectedUsers{failed_title}", "Error Removing user(s)"));
            }
            RefreshData();
        }

        private void lvUsers_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                removeSelectedUsers(true);
            }

        }

        private void lvUsers_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lvUsers.SelectedItems.Count == 1) {
                MainFormSelectCousinTreeNode("ndUsers", lvUsers.SelectedItems[0].Tag.ToString());
            }
        }

        private void ctxMenuUsers_Opening(object sender, CancelEventArgs e) {
            cmiUserProperties.Enabled = lvUsers.SelectedItems.Count == 1;
            cmiUserRemove.Enabled = lvUsers.SelectedItems.Count > 0;
        }

        private void ctxMenuPermissions_Opening(object sender, CancelEventArgs e) {
            cmiPermissionProperties.Enabled = lvPermissions.SelectedItems.Count == 1;

            cmiPermissionRemove.Enabled = lvPermissions.SelectedItems.Count > 0;

        }

        private void lvPermissions_ItemDrag(object sender, ItemDragEventArgs e) {
            startDrag(sender, "ndPermissions");
        }

        private void lvUsers_ItemDrag(object sender, ItemDragEventArgs e) {
            startDrag(sender, "ndUsers");
        }

        private void lvUsers_DragEnter(object sender, DragEventArgs e) {

            showCanDrop(e, "ndUsers");


        }

        private void lvPermissions_DragEnter(object sender, DragEventArgs e) {
            showCanDrop(e, "ndPermissions", "ndPermissionTemplates");

        }

        private void lvPermissions_DragDrop(object sender, DragEventArgs e) {
            var ddo = getDragDropObject(e);
            AdminProxy.AddPermissionsToGroup(ID, ddo.IDList);
            RefreshData();
        }

        private void lvUsers_DragDrop(object sender, DragEventArgs e) {
            var ddo = getDragDropObject(e);
            AdminProxy.AddUsersToGroup(ID, ddo.IDList);
            RefreshData();
        }

        private void txtName_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtCode_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtDescription_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void btnAddPermission_Click_1(object sender, EventArgs e) {
            popupAddPermission();
        }

        private void btnAddUser_Click(object sender, EventArgs e) {
            popupAddUser();
        }

        private void cmiPermissionsExportList_Click(object sender, EventArgs e) {
            ExportListView(lvPermissions);

        }

        private void cmiUsersExportList_Click(object sender, EventArgs e) {
            ExportListView(lvUsers);

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmGroup", resourceName, null, defaultValue, substitutes);
        }
    }
}
