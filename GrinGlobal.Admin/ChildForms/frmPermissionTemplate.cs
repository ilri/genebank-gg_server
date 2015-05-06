using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmPermissionTemplate : GrinGlobal.Admin.ChildForms.frmBase {
        public frmPermissionTemplate() {
            InitializeComponent();  tellBaseComponents(components);
        }


        public override void RefreshData() {
            this.Text = "Permission Template - " + MainFormCurrentNodeText("") + " - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

            var ds = AdminProxy.ListPermissionTemplates(ID < 0 ? int.MaxValue : ID);

            // fill permissions tab
            lvPermissions.Items.Clear();

            foreach (DataRow dr in ds.Tables["list_permission_templates"].Rows) {

                txtName.Text = dr["template_name"].ToString();
                txtDescription.Text = dr["description"].ToString();

                DataRow[] drPerms = ds.Tables["permissions_by_template"].Select("sys_perm_template_id = " + dr["sys_perm_template_id"]);
                if (drPerms != null && drPerms.Length > 0){
                    foreach(DataRow drp in drPerms){
                        var lvi = new ListViewItem(drp["perm_name"].ToString());
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
                        addPermItem(lvi, drp["create_permission_text"].ToString());
                        addPermItem(lvi, drp["read_permission_text"].ToString());
                        addPermItem(lvi, drp["update_permission_text"].ToString());
                        addPermItem(lvi, drp["delete_permission_text"].ToString());

                        // get restriction info...
                        var drFields = ds.Tables["template_perm_field_info"].Select("sys_permission_id = " + drp["sys_permission_id"]);
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
                        lvPermissions.Items.Add(lvi);
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
                if (DialogResult.Yes != MessageBox.Show(this, "You are about to permanently remove these permission(s) from this template.\nDo you want to continue?", "Remove Permissions?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    return;
                }
            }

            List<int> ids = new List<int>();
            foreach (ListViewItem lvi in lvPermissions.SelectedItems) {
                ids.Add(Toolkit.ToInt32(lvi.Tag, -1));
            }
            AdminProxy.RemovePermissionsFromTemplate(ID, ids);
            RefreshData();
        }

        private void addPermissionToolStripItem_Click(object sender, EventArgs e) {
            popupAddPermission();

        }

        private void popupAddPermission() {

            if (ID < 1) {
                if (DialogResult.Yes == MessageBox.Show(this, "You must save the permission template before adding permissions to it.\nDo you want to save it now?", "Save Permission Template?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
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
                AdminProxy.AddPermissionsToTemplate(ID, fp.SelectedIDs);
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
            ID = AdminProxy.SavePermissionTemplate(ID, txtName.Text, txtDescription.Text);

            RefreshData();
            MainFormUpdateStatus(getDisplayMember("save{done}", "Saved permission template {0}", txtName.Text), true);

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
        }

        private void lvPermissions_DragEnter(object sender, DragEventArgs e) {
            showCanDrop(e, "ndPermissions");
        }

        private void lvPermissions_DragDrop(object sender, DragEventArgs e) {
            var ddo = getDragDropObject(e);
            AdminProxy.AddPermissionsToTemplate(ID, ddo.IDList);
            RefreshData();
        }

        private void txtName_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtDescription_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void cmiPermissionsExportList_Click(object sender, EventArgs e) {
            ExportListView(lvPermissions);

        }

    }
}
