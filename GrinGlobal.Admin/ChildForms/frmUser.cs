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
    public partial class frmUser : GrinGlobal.Admin.ChildForms.frmBase {


        private string _password = null;
        private int _webCooperatorID = -1;
        private int _cooperatorID = -1;
        private int _currentCooperatorID = -1;

        private int _geographyID = -1;

        public frmUser() {
            InitializeComponent();  tellBaseComponents(components);
            PrimaryTabControl = tcUser;
            SecondaryTabControl = tcCooperator;
        }

        private void fillCooperatorGUI(DataRow dr) {

            if (dr == null) {
                var ds = AdminProxy.GetCooperatorInfo(_cooperatorID);
                var dt = ds.Tables["cooperator_info"];
                if (dt != null) {
                    if (dt.Rows.Count > 0) {
                        dr = dt.Rows[0];
                    }
                }
            }

            if (dr == null) {

                toggleCooperatorControls(false);

            } else {

                toggleCooperatorControls(true);

                _cooperatorID = Toolkit.ToInt32(dr["cooperator_id"], -1);
                _currentCooperatorID = Toolkit.ToInt32(dr["current_cooperator_id"], -1);
                // cooperator tabs follow...

                var fn = dr["current_cooperator_full_name"] + "";

                // general tab
                txtFirstName.Text = dr["first_name"].ToString();
                //txtInitials.Text = dr["initials"].ToString();
                txtLastName.Text = dr["last_name"].ToString();

                txtTitle.Text = dr["title"].ToString();
                txtFullName.Text = dr["full_name"].ToString();

                txtJob.Text = dr["job"].ToString();
                txtOrganization.Text = dr["organization"].ToString();
                txtOrganizationAbbreviation.Text = dr["organization_abbrev"].ToString();
                ddlDisciplineCode.SelectedIndex = getSelectedIndex(ddlDisciplineCode, dr["discipline_code"].ToString());
                //txtDiscipline.Text = dr["discipline_code"].ToString();

                chkIsActive.Checked = dr["status_code"].ToString().ToUpper() == "ACTIVE";
                ddlLanguage.SelectedIndex = ddlLanguage.FindString(dr["language_name"].ToString());

                lblCurrentCooperator.Text = "Current Cooperator is " + (fn.Trim() == "" ? "(none)" : fn);

                // web login tab
                txtWebUserName.Text = dr["web_user_name"].ToString();
                txtWebFirstName.Text = dr["web_first_name"].ToString();
                txtWebLastName.Text = dr["web_last_name"].ToString();

                // contact info tab
                txtAddressLine1.Text = dr["address_line1"].ToString();
                txtAddressLine2.Text = dr["address_line2"].ToString();
                txtAddressLine3.Text = dr["address_line3"].ToString();
                txtCity.Text = dr["city"].ToString();
                txtPostalIndex.Text = dr["postal_index"].ToString();
                txtEmail.Text = dr["email"].ToString();

                txtPrimaryPhone.Text = dr["primary_phone"].ToString();
                txtSecondaryPhone.Text = dr["secondary_phone"].ToString();
                txtFax.Text = dr["fax"].ToString();

                // geographic tab
                ddlSiteCode.SelectedIndex = getSelectedIndex(ddlSiteCode, dr["site_id"].ToString());
                ddlCategoryCode.SelectedIndex = getSelectedIndex(ddlCategoryCode, dr["category_code"].ToString());
                ddlOrganizationRegionCode.SelectedIndex = getSelectedIndex(ddlOrganizationRegionCode, dr["organization_region_code"].ToString());
                txtGeography.Text = dr["geography_description"].ToString();
                _geographyID = Toolkit.ToInt32(dr["geography_id"], -1);
                //txtRegionCode.Text = dr["organization_abbrev"].ToString();
                //txtCategoryCode.Text = dr["category_code"].ToString();

                // notes tab
                txtNote.Text = dr["note"].ToString();
            }

        }

        public override void RefreshData() {

            Sync(true, delegate() {
                initDropDowns();

                refreshData(true, true, true);
                MarkClean();
            });
        }



        private void refreshData(bool generalTab, bool permissionsTab, bool groupsTab){
            this.Text = "User " + MainFormCurrentNodeText("") + " - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

            var ds = AdminProxy.GetUserInfo(ID);

            if (generalTab) {
                
                // fill general user tab (includes Web Login tab too!)

                var dt = ds.Tables["user_info"];

                var enableSave = true;
                if (dt.Rows.Count > 0) {
                    var dr = dt.Rows[0];

                    _password = null;

                    // header info for user
                    txtUserName.Text = dr["user_name"].ToString();
                    chkEnabled.Checked = dr["is_enabled"].ToString().ToUpper() == "Y";
                    lblUserIsDisabled.Visible = !chkEnabled.Checked;

                    enableSave = dr["password_is_set"].ToString().ToUpper() != "Y";
                    lblPasswordNotSet.Visible = enableSave;

                    _webCooperatorID = Toolkit.ToInt32(dr["web_cooperator_id"], -1);
                    txtWebUserName.Text = dr["web_user_name"].ToString();

                    fillCooperatorGUI(dr);

                } else {
                    ddlLanguage.SelectedIndex = getSelectedIndex(ddlLanguage, AdminProxy.LanguageID.ToString());
                }
                var enabled = lblCurrentCooperator.Text.Replace("Current Cooperator is ", "").Trim() != "";
                toggleCooperatorControls(enabled);


                btnSave.Enabled = enableSave;
                btnChangePassword.Enabled = txtUserName.Text.Length > 0;


            }

            if (permissionsTab) {
                
                // fill permissions tab
                lvPermissions.Items.Clear();

                foreach (DataRow dr in ds.Tables["user_perm_info"].Rows) {
                    var lvi = new ListViewItem(dr["group_name"].ToString());
                    lvi.Tag = dr["sys_permission_id"];
                    lvi.UseItemStyleForSubItems = false;
                    lvi.SubItems.Add(dr["title"].ToString());
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
                    addPermItem(lvi, dr["create_permission_text"].ToString());
                    addPermItem(lvi, dr["read_permission_text"].ToString());
                    addPermItem(lvi, dr["update_permission_text"].ToString());
                    addPermItem(lvi, dr["delete_permission_text"].ToString());

                    // get restriction info...
                    var drFields = ds.Tables["user_perm_field_info"].Select("sys_permission_id = " + dr["sys_permission_id"]);
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

                    if (!String.IsNullOrEmpty(dr["group_name"].ToString())) {
                        for(var i=0;i<lvi.SubItems.Count;i++){
                            lvi.SubItems[i].BackColor = Color.LightGray;
                        }
                    }


                    lvPermissions.Items.Add(lvi);
                }

                refreshEffectivePermissions();
            }

            if (groupsTab) {

                lvGroups.Items.Clear();
                foreach (DataRow dr in ds.Tables["groups_by_user"].Rows) {
                    var lvi = new ListViewItem(dr["group_name"].ToString());
                    lvi.Tag = Toolkit.ToInt32(dr["sys_group_id"], -1);
                    lvi.SubItems.Add(dr["description"].ToString());
                    lvi.Tag = dr["sys_group_id"];
                    lvGroups.Items.Add(lvi);
                }

            
            }
            MarkClean();
        }

        private void initDropDowns() {

            initDataViewDropDown(ddlDataView, false, "-- Any Data View --", -1);
            initTableDropDown(ddlTable, false, "-- Any Table --", -1);

            initSiteDropDown(ddlSiteCode, true, null);

            initLanguageDropDown(ddlLanguage, true, null);

            initCodeValueDropDown(ddlOrganizationRegionCode, "ORGANIZATION_REGION", null);
            initCodeValueDropDown(ddlCategoryCode, "COOPERATOR_CATEGORY", null);
            initCodeValueDropDown(ddlDisciplineCode, "COOPERATOR_DISCIPLINE", null);

//            initGeographyDropDown(ddlGeography, true, null);

        }

        private void toggleCooperatorControls(bool enabled) {
            foreach (Control ctl in tpCoopGeneral.Controls) {
                if (ctl != btnSelectCooperator && ctl != lblCurrentCooperator) {
                    ctl.Enabled = enabled;
                }
            }
        }

        private void frmUser_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvPermissions);
        }

        private void lvPermissions_MouseClick(object sender, MouseEventArgs e) {
            if (lvPermissions.SelectedIndices.Count > 0) {
                if (e.Button == MouseButtons.Right) {
                    cmPermissions.Show(lvPermissions, e.Location);
                }
            }
        }

        private void btnAddPermission_Click(object sender, EventArgs e) {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            removeSelectedPermissions(true);
        }

        private void removeSelectedPermissions(bool prompt) {

            if (lvPermissions.SelectedItems.Count == 0) {
                return;
            }

            if (prompt) {
                if (DialogResult.Yes != MessageBox.Show(this, getDisplayMember("removePermissions{start_body}", "You are about to permanently remove these permission(s) from this user.\nDo you want to continue?"), 
                    getDisplayMember("removePermissions{start_title}", "Remove Permissions?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    return;
                }
            }

            List<int> ids = new List<int>();
            foreach (ListViewItem lvi in lvPermissions.SelectedItems) {
                ids.Add(Toolkit.ToInt32(lvi.Tag, -1));
            }
            AdminProxy.RemovePermissionsFromUser(ID, ids);
            refreshData(false, true, false);
            MainFormUpdateStatus(getDisplayMember("removePermissions{done}", "Removed permission(s) from user"), true);
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            if (Modal) {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            } else {
                MainFormSelectParentTreeNode();
            }
        }

        private void ddlTable_SelectedIndexChanged(object sender, EventArgs e) {
            Sync(true, delegate() {
                if (ddlDataView.Items.Count > 0) {
                    ddlDataView.SelectedIndex = 0;
                }
                refreshEffectivePermissions();
            });
        }

        private void ddlDataView_SelectedIndexChanged(object sender, EventArgs e) {
            Sync(true, delegate() {
                if (ddlTable.Items.Count > 0) {
                    ddlTable.SelectedIndex = 0;
                }
                refreshEffectivePermissions();
            });
        }

        private void displayEffectivePermission(Label lbl, string text) {
            string permCode = ((text + "D").Substring(0, 1));
            switch(permCode){
                case "D":
                lbl.ForeColor = Color.Red;
                lbl.Text = "DENIED";
                    break;
                case "A":
                    lbl.ForeColor = Color.Black;
                    lbl.Text = "Allow";
                    break;
                case "V":
                    lbl.ForeColor = Color.Blue;
                    lbl.Text = "Varies by row";
                    break;
                case "I":
                default:
                    // should never ever happen
                    lbl.ForeColor = Color.Gray;
                    lbl.Text = "!!!";
                    break;
            }
        }

        private void refreshEffectivePermissions() {
            string dv = ddlDataView.SelectedValue.ToString();
            if (dv.StartsWith("--")){
                dv = null;
            } 
            string tbl = ddlTable.SelectedValue.ToString();
            if (tbl.StartsWith("--")){
                tbl = null;
            }
            var dt = AdminProxy.ListEffectivePermissions(ID, dv, tbl).Tables["effective_permissions"];

            if (dt.Rows.Count > 0) {
                var dr = dt.Rows[0];
                displayEffectivePermission(lblCreate, dr["create_permission"].ToString());
                displayEffectivePermission(lblRead, dr["read_permission"].ToString());
                displayEffectivePermission(lblUpdate, dr["update_permission"].ToString());
                displayEffectivePermission(lblDelete, dr["delete_permission"].ToString());
            }

        }

        private bool promptToSaveUserIfNeeded(string text) {
            if (ID < 1) {
                if (DialogResult.Yes == MessageBox.Show(this, text, getDisplayMember("promptToSaveUser{title}", "Save User?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    return save();
                }
            }
            return true;
        }

        private void btnAddPermission_Click_1(object sender, EventArgs e) {
            if (promptToSaveUserIfNeeded(getDisplayMember("addPermission{mustsave}", "You must save the user before adding permissions.\nDo you want to save now?"))) {
                refreshData(true, true, true);
                popupAddPermission();
            }
        }

        private void popupAddPermission(){
            frmPermissions fp = new frmPermissions();
            foreach(ListViewItem lvi in lvPermissions.Items){
                fp.AssignedIDs.Add(Toolkit.ToInt32(lvi.Tag, -1));
            }
            if (DialogResult.OK == MainFormPopupForm(fp, this, true)) {
                // add selected permission to user, refresh
                AdminProxy.AddPermissionsToUser(ID, fp.SelectedIDs);
                refreshData(false, true, false);
                MainFormUpdateStatus(getDisplayMember("popupAddPermission{done}", "Added permission to user"), true);

            }
        }

        private void lvPermissions_KeyPress(object sender, KeyPressEventArgs e) {
            //if ((Keys)e.KeyChar == Keys.Delete) {
            //    removeSelectedPermissions(true);
            //}
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            if (promptToSaveUserIfNeeded(getDisplayMember("addPermission{mustsave}", "You must save the user before adding permissions.\nDo you want to save now?"))) {
                refreshData(true, true, true);
                popupAddPermission();
            }

        }

        private void lvPermissions_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                removeSelectedPermissions(true);
            }
        }


        private void btnChangePassword_Click(object sender, EventArgs e) {
            var f = new frmSetPassword();
            if (f.ShowDialog(this, MainFormCurrentNodeText(txtUserName.Text)) == DialogResult.OK) {
                if (ID < 1) {
                    // new user, can't change it yet. remember it for when we save the user
                    _password = f.Password;
                    lblPasswordNotSet.Visible = String.IsNullOrEmpty(_password);
                    btnSave.Enabled = String.IsNullOrEmpty(_password);
                    MarkDirty();
                } else {
                    using (new AutoCursor(this)) {
                        AdminProxy.ChangePassword(ID, f.Password);
                        if (txtUserName.Text.ToLower() == AdminProxy.Connection.GrinGlobalUserName.ToLower()) {
                            AdminProxy.Connection.GrinGlobalPassword = f.Password;
                            if (AdminProxy.Connection.GrinGlobalRememberPassword) {
                                MainFormSaveConnections(AdminProxy.Connection);
                            }
                        }
                        MainFormUpdateStatus(getDisplayMember("changePassword{done}", "Changed password for user {0}", MainFormCurrentNodeText(txtUserName.Text)), true);
                    }
                }
            }

        }

        private void ctxMenuPermissions_Opening(object sender, CancelEventArgs e) {
            cmiPermissionProperties.Enabled = false;

            if (lvPermissions.SelectedItems.Count == 1) {
                if (String.IsNullOrEmpty(lvPermissions.SelectedItems[0].SubItems[0].Text)){
                    cmiPermissionProperties.Enabled = true;
                }
            }


            cmiPermissionRemove.Enabled = false;
            if (lvPermissions.SelectedItems.Count > 0) {
                if (String.IsNullOrEmpty(lvPermissions.SelectedItems[0].SubItems[0].Text)) {
                    cmiPermissionRemove.Enabled = true;
                }
            }

        }

        private void editPermissionToolStripMenuItem_Click_1(object sender, EventArgs e) {
            if (lvPermissions.SelectedItems.Count == 1) {
                MainFormSelectCousinTreeNode("ndPermissions", lvPermissions.SelectedItems[0].Tag.ToString());
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void textBox9_TextChanged(object sender, EventArgs e) {

        }

        private void textBox8_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void textBox7_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void textBox4_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void textBox5_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void textBox6_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void label15_Click(object sender, EventArgs e) {

        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void label13_Click(object sender, EventArgs e) {

        }

        private bool save() {

            if (txtUserName.Text.Trim().Length == 0) {
                tcUser.SelectedTab = tpGeneral;
                Application.DoEvents();
                MessageBox.Show(getDisplayMember("save{usernamerequired}", "You must set a username for this user before saving."));
                txtUserName.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(_password) && ID == -1) {
                tcUser.SelectedTab = tpGeneral;
                Application.DoEvents();
                MessageBox.Show(getDisplayMember("save{passwordrequired}", "You must set a password for this user before saving."));
                btnChangePassword.Focus();
                return false;
            }
            if (_cooperatorID < 1 && (txtFirstName.Text.Trim().Length == 0 || txtLastName.Text.Trim().Length == 0)) {
                tcUser.SelectedTab = tpGeneral;
                tcCooperator.SelectedTab = tpCoopGeneral;
                Application.DoEvents();
                MessageBox.Show(getDisplayMember("save{cooperatorrequired}", "You must select a cooperator or enter a new one for this user before saving.\nA new cooperator must have its First Name and Last Name specified."));
                txtFirstName.Focus();
                return false;
            }

            if (_geographyID < 1) {
                tcUser.SelectedTab = tpGeneral;
                tcCooperator.SelectedTab = tpCoopGeographic;
                Application.DoEvents();
                MessageBox.Show(getDisplayMember("save{geographyrequired}", "You must specify a geographic location for this user before saving."));
                btnGeography.Focus();
                return false;
            }

            var discCode = getSelectedValue(ddlDisciplineCode) + string.Empty;
            var catCode = getSelectedValue(ddlCategoryCode) + string.Empty;
            var orgRegionCode = getSelectedValue(ddlOrganizationRegionCode) + string.Empty; //.SelectedValue == null ? null : ((DataRowView)ddlOrganizationRegionCode.SelectedValue).Row[ddlOrganizationRegionCode.ValueMember] + string.Empty;


            var newID = -1;
            try {
                newID = AdminProxy.SaveUser(ID,
                        txtUserName.Text,
                        chkEnabled.Checked,
                        _cooperatorID,
                        _currentCooperatorID,
                        _webCooperatorID,
                        txtTitle.Text,
                        txtFirstName.Text,
                        //txtInitials.Text,
                        txtLastName.Text,
                    //                    txtFullName.Text,
                        txtJob.Text,
                        discCode,
                        txtOrganization.Text,
                        txtOrganizationAbbreviation.Text,
                        Toolkit.ToInt32(ddlLanguage.SelectedValue, -1),
                        chkIsActive.Checked,
                        txtAddressLine1.Text,
                        txtAddressLine2.Text,
                        txtAddressLine3.Text,
                        txtCity.Text,
                        txtPostalIndex.Text,
                        txtEmail.Text,
                        txtPrimaryPhone.Text,
                        txtSecondaryPhone.Text,
                        txtFax.Text,
                        Toolkit.ToInt32(ddlSiteCode.SelectedValue, -1),
                        orgRegionCode,
                        catCode,
//                        txtRegionCode.Text,
//                        txtCategoryCode.Text,
                        _geographyID,
                    //                    Toolkit.ToInt32(ddlGeography.SelectedValue, 0),
                        txtNote.Text
                    );
            } catch (InvalidOperationException ioe) {
                if (ioe.Message.Contains("duplicate")) {
                    MessageBox.Show(this, getDisplayMember("save{duplicateuser_body}", "A user with that user name already exists.\r\nYou must choose a different one."), 
                        getDisplayMember("save{duplicateuser_title}", "User Name Taken"));
                } else {
                    throw;
                }
            }

            if (newID != ID) {
                // every user is added to the all users group. always.
                var groups = AdminProxy.ListGroups(-1).Tables["list_groups"];
                var groupID = -1;
                foreach(DataRow dr in groups.Rows){
                    if (dr["group_tag"].ToString().ToLower() == "allusers"){
                        groupID = Toolkit.ToInt32(dr["sys_group_id"], -1);
                        break;
                    }
                }
                if (groupID > -1) {
                    var userIDs = new List<int>();
                    userIDs.Add(newID);
                    AdminProxy.AddUsersToGroup(groupID, userIDs);
                }
            }

            ID = newID;

            if (!String.IsNullOrEmpty(_password)) {
                AdminProxy.ChangePassword(ID, _password);
                _password = null;
            }
            return true;
        }

        private void tabUser_TabIndexChanged(object sender, EventArgs e) {
            Sync(true, delegate() {
                if (ID < 1 && tcUser.SelectedIndex > 0) {
                    if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("switchtab{save_body}", "You must save your changes before you can edit the permissions.\nDo you want to save now?"), 
                        getDisplayMember("switchtab{save_title}", "Save?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                        if (!save()) {
                            tcUser.SelectedIndex = 0;
                        } else {
                        }
                    } else {
                        tcUser.SelectedIndex = 0;
                    }
                }
            });
        }

        private void btnSelectCooperator_Click(object sender, EventArgs e) {
            MessageBox.Show("not implemented yet");
        }

        private void btnSave_Click_1(object sender, EventArgs e) {
            if (save()) {
                if (!this.Modal) {
                    this.MainFormSelectParentTreeNode();
                    MainFormUpdateStatus(getDisplayMember("saved{done}", "Saved user"), true);
                } else {
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void txtUserName_TextChanged(object sender, EventArgs e) {
            btnChangePassword.Enabled = txtUserName.Text.Length > 0;
            CheckDirty();
        }

        private void btnCancel_Click_1(object sender, EventArgs e) {
            if (Modal) {
                DialogResult = DialogResult.Cancel;
                this.Close();
            } else {
                MainFormSelectParentTreeNode();
            }
        }

        private void lvPermissions_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lvPermissions.SelectedItems.Count == 1) {
                MainFormSelectCousinTreeNode("ndPermissions", lvPermissions.SelectedItems[0].Tag.ToString());
            }
        }

        private void popupAddPermissionTemplate() {
            //frmPermissionTemplates fpt = new frmPermissionTemplates();
            //if (DialogResult.OK == MainFormPopupForm(fpt, this, true)) {
            //    // add selected permission templates to user, refresh

            //    AdminProxy.ApplyPermissionTemplatesToUser(ID, fpt.SelectedIDs); 

            //    refreshData(false, true, false);
            //    MainFormUpdateStatus("Added permissions to user", true);

            //}
        }


        private void btnApplyTemplate_Click_1(object sender, EventArgs e) {
            //if (promptToSaveUserIfNeeded("You must save the user before applying a template.\nDo you want to save now?")) {
            //    refreshData(true, true, true);
            //    popupAddPermissionTemplate();
            //}

        }

        private void lvPermissions_ItemDrag(object sender, ItemDragEventArgs e) {
            var lvi = e.Item as ListViewItem;
            if (lvi != null && lvi.BackColor != Color.LightGray) {
                startDrag(sender, "ndPermissions");
            }
        }

        private void btnSaveAsTemplate_Click(object sender, EventArgs e) {

            var ptName = "New Template " + DateTime.Today.ToString("yyyy-MM-dd");
            // create a new perm template
            int ptID = AdminProxy.SavePermissionTemplate(-1, ptName, null);

            // save current user's permissions to it
            var permIDs = new List<int>();
            foreach(ListViewItem lvi in lvPermissions.Items){
                permIDs.Add(Toolkit.ToInt32(lvi.Tag, -1));
            }
            AdminProxy.AddPermissionsToTemplate(ptID, permIDs);


            // show the permission template form so they can name it however they want to
            base.MainFormSelectCousinTreeNode("ndPermissionTemplates", ptID.ToString());
        }

        private void btnSelectCooperator_Click_1(object sender, EventArgs e) {
            var f = new frmCooperator();
            if (f.ShowDialog(this.Owner as mdiParent, this.AdminProxy) == DialogResult.OK) {
                _cooperatorID = _currentCooperatorID = f.CooperatorID;

                fillCooperatorGUI(null);
            }
            CheckDirty();

        }

        private void lvGroups_DragEnter(object sender, DragEventArgs e) {
            showCanDrop(e, "ndGroups");
        }

        private void lvGroups_DragDrop(object sender, DragEventArgs e) {
            var ddo = getDragDropObject(e);
            AdminProxy.AddUsersToGroup(ddo.IDList[0], new List<int>(new int[] { ID }));
            RefreshData();
            MainFormUpdateStatus(getDisplayMember("groupsDragDrop{addeduser}", "Added user to group"), true);
        }

        private void lvPermissions_DragEnter(object sender, DragEventArgs e) {
            showCanDrop(e, "ndPermissions");
        }

        private void lvPermissions_DragDrop(object sender, DragEventArgs e) {
            var ddo = getDragDropObject(e);
            AdminProxy.AddPermissionsToUser(ID, ddo.IDList);
            RefreshData();
            MainFormUpdateStatus(getDisplayMember("permissionsDragDrop{addpermission}", "Added permission(s) to user"), true);

        }

        private void cmiGroupProperties_Click(object sender, EventArgs e) {
            if (lvGroups.SelectedItems.Count == 1) {
                MainFormSelectCousinTreeNode("ndGroups", lvGroups.SelectedItems[0].Tag.ToString());
            }

        }

        private void cmiGroupRemove_Click(object sender, EventArgs e) {
            removeSelectedGroups(true);

        }

        private void removeSelectedGroups(bool prompt) {

            if (lvGroups.SelectedItems.Count == 0) {
                return;
            }

            if (prompt) {
                if (DialogResult.Yes != MessageBox.Show(this, getDisplayMember("removeGroups{start_body}", "You are about to permanently remove this user from these group(s).\nDo you want to continue?"), 
                    getDisplayMember("removeGroups{start_title}", "Remove From Groups?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    return;
                }
            }

            List<int> ids = new List<int>();
            ids.Add(ID);
            var count = 0;
            foreach (ListViewItem lvi in lvGroups.SelectedItems) {
                var groupID = Toolkit.ToInt32(lvi.Tag, -1);
                try {
                    AdminProxy.RemoveUsersFromGroup(groupID, ids);
                    count++;
                } catch (Exception ex) {
                    MessageBox.Show(this, ex.Message, getDisplayMember("removeGroups{failed}", "Error Removing User From Group"));
                }
            }
            refreshData(false, false, true);
            MainFormUpdateStatus(getDisplayMember("removedfromgroup{done}", "Removed user from {0} group(s)", count.ToString("###,##0")), true);

        }

        private void cmiGroupAdd_Click(object sender, EventArgs e) {
            if (promptToSaveUserIfNeeded(getDisplayMember("addToGroup{mustsave}", "You must save the user before adding it to a group.\nDo you want to save now?"))) {
                refreshData(true, true, true);
                popupAddToGroup();
            }

        }

        private void popupAddToGroup() {
            var fg = new frmGroups();
            //foreach (ListViewItem lvi in lvGroups.Items) {
            //    fg.AssignedIDs.Add(Toolkit.ToInt32(lvi.Tag, -1));
            //}
            if (DialogResult.OK == MainFormPopupForm(fg, this, false)) {
                // add selected permission to user, refresh
                var ids = new List<int>();
                ids.Add(ID);
                foreach(int groupID in fg.SelectedIDs){
                    AdminProxy.AddUsersToGroup(groupID, ids);
                }
                refreshData(false, true, true);
                MainFormUpdateStatus(getDisplayMember("popupAddToGroup{done}", "Added user to group(s)"), true);

            }
        }

        private void cmGroups_Opening(object sender, CancelEventArgs e) {
            cmiGroupProperties.Enabled = lvGroups.SelectedItems.Count == 1;
            cmiGroupRemove.Enabled = lvGroups.SelectedItems.Count > 0;

        }

        private void lvGroups_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lvGroups.SelectedItems.Count == 1) {
                MainFormSelectCousinTreeNode("ndGroups", lvGroups.SelectedItems[0].Tag.ToString());
            }

        }

        private void lvGroups_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                removeSelectedGroups(true);
            }

        }

        private void lvGroups_ItemDrag(object sender, ItemDragEventArgs e) {
            startDrag(sender, "ndGroups");
        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private void lblUserIsDisabled_Click(object sender, EventArgs e) {

        }

        private void txtTitle_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtFirstName_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtInitials_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtLastName_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtJob_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtDiscipline_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtOrganization_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtOrganizationCode_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void chkIsActive_CheckedChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtSecondaryPhone_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtFax_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtEmail_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlSiteCode_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtRegionCode_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtCategoryCode_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtGeography_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlGeography_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtNote_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void btnAddGroup_Click(object sender, EventArgs e) {
            if (promptToSaveUserIfNeeded(getDisplayMember("addToGroup{mustsave}", "You must save the user before adding it to a group.\nDo you want to save now?"))) {
                refreshData(true, true, true);
                popupAddToGroup();
            }
        }

        private void btnGeography_Click(object sender, EventArgs e) {
            var f = new frmGeography();
            if (f.ShowDialog(this.Owner as mdiParent, this.AdminProxy) == DialogResult.OK) {
                _geographyID = f.GeographyID;
                txtGeography.Text = f.GeographyText;
            }
            CheckDirty();

        }

        private void cmiPermissionsExportList_Click(object sender, EventArgs e) {
            ExportListView(lvPermissions);

        }

        private void cmiGroupsExportList_Click(object sender, EventArgs e) {
            ExportListView(lvGroups);

        }

        private void ddlDisciplineCode_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private void ddlOrganizationRegionCode_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private void ddlCategoryCode_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();
        }


        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmUser", resourceName, null, defaultValue, substitutes);
        }

        private void btnWebCooperatorSearch_Click(object sender, EventArgs e) {
            var f = new frmWebCooperator();
            if (f.ShowDialog(this.Owner as mdiParent, this.AdminProxy) == DialogResult.OK) {
                _webCooperatorID = f.WebCooperatorID;
                txtWebUserName.Text = "";
                txtWebLastName.Text = "";
                txtWebFirstName.Text = "";
                var ds = AdminProxy.GetWebCooperatorInfo(_webCooperatorID);
                var dt = ds.Tables["web_cooperator_info"];
                if (dt != null) {
                    if (dt.Rows.Count > 0) {
                        var dr = dt.Rows[0];
                        txtWebFirstName.Text = dr["web_first_name"].ToString();
                        txtWebLastName.Text = dr["web_last_name"].ToString();
                        txtWebUserName.Text = dr["web_user_name"].ToString();
                    }
                }
            }
            CheckDirty();

        }

        private void btnSetWebPassword_Click(object sender, EventArgs e) {
            var f = new frmSetPassword();
            if (f.ShowDialog(this, txtWebUserName.Text) == DialogResult.OK) {
                using (new AutoCursor(this)) {
                    AdminProxy.ChangeWebPassword(txtWebUserName.Text, f.Password);
                    MainFormUpdateStatus(getDisplayMember("changeWebPassword{done}", "Changed password for web login {0} for system user {1}", txtWebUserName.Text, MainFormCurrentNodeText(txtUserName.Text)), true);
                }
            }
        }
    }
}
