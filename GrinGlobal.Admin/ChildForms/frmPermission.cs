using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using System.Diagnostics;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmPermission : GrinGlobal.Admin.ChildForms.frmBase {
        public frmPermission() {
            InitializeComponent();  tellBaseComponents(components);

        }

        private void frmPermission_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvPermFields);
        }

        private void initDropDowns() {


            // load all enabled dataviews into dropdown
            //var dvs = AdminProxy.ListDataViews(null).Tables["list_data_views"];
            //foreach (DataRow dr in dvs.Rows) {
            //    ddlDataView.Items.Add(
            //}

            initDataViewDropDown(ddlDataView, true, "-- Any Data View --", -1);
//            ddlDataView.SelectedIndex = 0;

            initTableDropDown(ddlTable, true, "-- Any Table --", -1);
//            ddlTable.SelectedIndex = 0;


            //ddlCreate.SelectedIndex = 0;
            //ddlRead.SelectedIndex = 0;
            //ddlUpdate.SelectedIndex = 0;
            //ddlDelete.SelectedIndex = 0;
        }

        public override void RefreshData() {

            this.Text = "Permission " + MainFormCurrentNodeText("") + " - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

            initDropDowns();

            if (ID > -1) {
                var dt = AdminProxy.ListPermissions(ID, null, false).Tables["list_permissions"];
                if (dt.Rows.Count > 0) {
                    var dr = dt.Rows[0];
                    txtCode.Text = dr["permission_tag"].ToString();
                    txtName.Text = dr["title"].ToString();
                    txtDescription.Text = dr["description"].ToString();
                    ddlDataView.SelectedIndex = ddlDataView.FindString(dr["dataview_name"].ToString());
                    ddlTable.SelectedIndex = ddlTable.FindString(dr["table_name"].ToString());
                    ddlCreate.SelectedIndex = ddlCreate.FindString(dr["create_permission_text"].ToString());
                    ddlRead.SelectedIndex = ddlRead.FindString(dr["read_permission_text"].ToString());
                    ddlUpdate.SelectedIndex = ddlUpdate.FindString(dr["update_permission_text"].ToString());
                    ddlDelete.SelectedIndex = ddlDelete.FindString(dr["delete_permission_text"].ToString());
                    chkEnabled.Checked = dr["is_enabled"].ToString().Trim().ToUpper() == "Y";

                    var dt2 = AdminProxy.ListPermissionFields(ID, -1).Tables["list_permission_fields"];
                    lvPermFields.Items.Clear();
                    foreach (DataRow dr2 in dt2.Rows) {
                        var lvi = new ListViewItem();
                        lvi.Tag = dr2["sys_permission_field_id"];
                        lvi.UseItemStyleForSubItems = false;
                        var dv = dr2["dataview_name"].ToString();
                        var tbl = dr2["table_name"].ToString();
                        var mode = dr2["compare_mode"].ToString();
                        if (mode == "parent") {
                            // restricting by parent...
                            lvi.Text = dr2["parent_table_name"].ToString() + " (for all children)";
                            lvi.SubItems.Add(dr2["parent_table_field_name"].ToString());
                            lvi.SubItems.Add(dr2["parent_compare_operator"].ToString());
                            string verbiage = getVerbiageForSpecialCommand(dr2["parent_compare_value"].ToString());
                            lvi.SubItems.Add(verbiage);
                            lvi.SubItems.Add(dr2["parent_table_name"].ToString() + "." + dr2["parent_Table_field_name"].ToString() + " " + dr2["parent_compare_operator"] + " " + verbiage + " (for all children)");

//                            lvi.Text = dr2["parent_table_name"].ToString() + "." + dr2["parent_Table_field_name"].ToString() + " " + dr2["parent_compare_operator"] + " " + dr2["parent_compare_value"] + " (for all children)";
                        } else if (dv == "") {
                            lvi.Text = tbl;
                            lvi.SubItems.Add(dr2["table_field_name"].ToString());
                            lvi.SubItems.Add(dr2["compare_operator"].ToString());
                            string verbiage = getVerbiageForSpecialCommand(dr2["compare_value"].ToString());
                            lvi.SubItems.Add(verbiage);
                            lvi.SubItems.Add(dr2["table_name"].ToString() + "." + dr2["Table_field_name"].ToString() + " " + dr2["compare_operator"] + " " + verbiage);

//                            lvi.Text = dr2["table_name"].ToString() + "." + dr2["Table_field_name"].ToString() + " " + dr2["compare_operator"] + " " + dr2["compare_value"];
                        
                        } else {
                            lvi.Text = dv + " (dataview)";
                            lvi.SubItems.Add(dr2["dataview_field_name"].ToString());
                            lvi.SubItems.Add(dr2["compare_operator"].ToString());
                            string verbiage = getVerbiageForSpecialCommand(dr2["compare_value"].ToString());
                            lvi.SubItems.Add(verbiage);
                            lvi.SubItems.Add(dr2["dataview_field_name"].ToString() + "." + dr2["dataview_field_name"].ToString() + " " + dr2["compare_operator"] + " " + verbiage + " (dataview)");

//                            lvi.Text = dr2["dataview_field_name"].ToString() + "." + dr2["dataview_field_name"].ToString() + " " + dr2["compare_operator"] + " " + dr2["compare_value"] + " (dataview)";
                        }

                        lvPermFields.Items.Add(lvi);
                    }


                }
            } else {

                ddlDataView.SelectedIndex = 0;
                ddlTable.SelectedIndex = 0;

                ddlCreate.SelectedIndex = 0;
                ddlRead.SelectedIndex = 0;
                ddlUpdate.SelectedIndex = 0;
                ddlDelete.SelectedIndex = 0;
            }

            MarkClean();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            save(!Modal);
        }

        private void save(bool showParent){
            ID = AdminProxy.SavePermission(Toolkit.ToInt32(ID, -1),
                txtCode.Text,
                txtName.Text,
                txtDescription.Text,
                Toolkit.ToInt32(ddlDataView.SelectedValue, -1),
                Toolkit.ToInt32(ddlTable.SelectedValue, -1),
                ddlCreate.Text,
                ddlRead.Text,
                ddlUpdate.Text,
                ddlDelete.Text,
                chkEnabled.Checked);
            if (showParent) {
                MainFormSelectParentTreeNode();
                MainFormUpdateStatus(getDisplayMember("Saved permission {0}", txtName.Text), true);
                DialogResult = DialogResult.OK;
                this.Close();
            } else {
                RefreshData();
                MainFormUpdateStatus(getDisplayMember("Saved permission {0}", txtName.Text), true);
                DialogResult = DialogResult.OK;
                this.Close();
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

        private void btnAddRestriction_Click(object sender, EventArgs e) {

            if (Toolkit.ToInt32(ddlDataView.SelectedValue, -1) == -1 && Toolkit.ToInt32(ddlTable.SelectedValue, -1) == -1) {
                MessageBox.Show(this, getDisplayMember("addRestriction{noresource_body}", "Adding a restriction requires the permission to be applied to a specific Data View or Table.\nPlease select one to continue."), 
                    getDisplayMember("addRestriction{noresource_title}","Data View or Table Required"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (ID < 1) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("addRestriction{mustsave_body}", "You must save the permission before adding restrictions to it.\nDo you want to save it now?"), 
                    getDisplayMember("addRestriction{mustsave_title}", "Save Permission?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    save(false);
                } else {
                    return;
                }
            }

            showProperties(-1);
        }

        private void newRestrictionToolStripMenuItem_Click(object sender, EventArgs e) {
            btnAddRestriction.PerformClick();
        }

        private void showProperties(int permFieldID) {
            frmPermissionField fpf = new frmPermissionField();
            fpf.ID = permFieldID;
            fpf.PermissionID = ID;
            fpf.DataViewID = Toolkit.ToInt32(ddlDataView.SelectedValue, -1);
            fpf.TableID = Toolkit.ToInt32(ddlTable.SelectedValue, -1);
            if (DialogResult.OK == MainFormPopupForm(fpf, this, false)) {
                RefreshData();
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lvPermFields.SelectedItems.Count == 1) {
                showProperties(Toolkit.ToInt32(lvPermFields.SelectedItems[0].Tag, -1));
            }
        }

        private void lvPermFields_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                cmiDelete.PerformClick();
            } else if (e.KeyCode == Keys.Enter) {
                cmiProperties.PerformClick();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            deleteItems(true);
        }

        private void deleteItems(bool promptFirst) {
            if (lvPermFields.SelectedItems.Count > 0) {
                if (promptFirst) {
                    if (DialogResult.No == MessageBox.Show(this, getDisplayMember("deleteRestrictions{body}", "You are about to permanently delete restriction(s) from this permission.\nDo you want to continue?"), 
                        getDisplayMember("deleteRestrictions{title}", "Delete Restriction(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                        return;
                    }
                }

                foreach (ListViewItem lvi in lvPermFields.SelectedItems) {
                    AdminProxy.DeletePermissionField(Toolkit.ToInt32(lvi.Tag, -1));
                }
                RefreshData();
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e) {
            cmiDelete.Enabled = lvPermFields.SelectedItems.Count > 0;
            cmiProperties.Enabled = lvPermFields.SelectedItems.Count == 1;
        }

        private void lvPermFields_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lvPermFields.SelectedItems.Count == 1) {
                showProperties(Toolkit.ToInt32(lvPermFields.SelectedItems[0].Tag, -1));
            }

        }

        private void txtDescription_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            //Debug.WriteLine("PreviewKeyDown: " + (e.Control ? "CTRL-" : "") + (e.Alt ? "ALT-" : "") + (e.Shift ? "SHFT-" : "") + e.KeyCode.ToString());
        }

        private void txtDescription_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e) {
            //Debug.WriteLine("KeyDown: " + (e.Control ? "CTRL-" : "") + (e.Alt ? "ALT-" : "") + (e.Shift ? "SHFT-" : "") + e.KeyCode.ToString());

        }

        private void txtName_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlDataView_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlTable_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlCreate_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlRead_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlUpdate_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlDelete_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtCode_TextChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private void cmiPermissionExportList_Click(object sender, EventArgs e) {
            ExportListView(lvPermFields);

        }
        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmPermission", resourceName, null, defaultValue, substitutes);
        }
    }
}
