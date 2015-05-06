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
    public partial class frmCodeGroup : frmBase {

        public static String[] RESERVED_CODE_GROUPS = new String[] { "DATAVIEW_CATEGORY", "DATAVIEW_DATABASE_AREA", "SYS_PERMISSION_CODE", "GEOGRAPHY_COUNTRY_CODE", "GERMPLASM_FORM" };

        public frmCodeGroup() {
            InitializeComponent();
            PrimaryTabControl = tc;
            MakeListViewSortable(lvValues);
            MakeListViewSortable(lvReferencedBy);
        }

        private int _selectedFieldID;

        public int GetNextValueID(bool markNextAsCurrent) {
            for (var i = 0; i < lvValues.Items.Count; i++) {
                var id = Toolkit.ToInt32(lvValues.Items[i].Tag, -1);
                if (id == _selectedFieldID) {
                    if (i == lvValues.Items.Count - 1) {
                        return -1;
                    } else {
                        var nextID = Toolkit.ToInt32(lvValues.Items[i + 1].Tag, -1);
                        if (markNextAsCurrent) {
                            _selectedFieldID = nextID;
                        }
                        return nextID;
                    }
                }
            }
            return -1;
        }

        public int GetPreviousValueID(bool markPreviousAsCurrent) {
            for (var i = 0; i < lvValues.Items.Count; i++) {
                var id = Toolkit.ToInt32(lvValues.Items[i].Tag, -1);
                if (id == _selectedFieldID) {
                    if (i == 0) {
                        return -1;
                    } else {
                        var prevID = Toolkit.ToInt32(lvValues.Items[i - 1].Tag, -1);
                        if (markPreviousAsCurrent) {
                            _selectedFieldID = prevID;
                        }
                        return prevID;
                    }
                }
            }
            return -1;

        }


        private string _originalGroupName;

        public override void RefreshData() {
            Sync(true, delegate() {


                var groupName = MainFormCurrentNode().Tag as string;
                if (String.IsNullOrEmpty(groupName)) {
                    groupName = _originalGroupName;
                }

                this.Text = "Code Group - " + groupName + " - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

                if (!String.IsNullOrEmpty(groupName)) {
                    // fill values tab
                    var languageID = Toolkit.ToInt32(getSelectedValue(ddlLanguage), null);
                    var dt = AdminProxy.ListCodeValues(groupName, null, languageID).Tables["list_code_values"];

                    if (dt.Rows.Count > 0) {
                        txtName.Text = dt.Rows[0]["group_name"].ToString();
                        _originalGroupName = dt.Rows[0]["group_name"].ToString();
                    }

                    if (Array.IndexOf(RESERVED_CODE_GROUPS, _originalGroupName) > -1) {
                        // don't let them edit reserved code groups
                        txtName.Enabled = false;
                    }

                    //dgvCodes.AutoGenerateColumns = false;
                    //dgvCodes.DataSource = dt;

                    var lvs = new List<ListViewItem>();
                    foreach (DataRow dr in dt.Rows) {
                        var lvi = new ListViewItem(dr["value_member"].ToString());
                        lvi.Tag = dr["code_value_id"];
                        lvi.SubItems.Add(dr["title"].ToString());
                        lvi.SubItems.Add(dr["description"].ToString());
                        lvi.SubItems.Add((Toolkit.ToDateTime(dr["last_touched"], DateTime.MinValue)).ToLocalTime().ToString());
                        lvs.Add(lvi);
                    }
                    lvValues.Items.Clear();
                    lvValues.Items.AddRange(lvs.ToArray());


                    tpValues.Text = "Values ( " + dt.Rows.Count + " ) ";

                    initHooksForMdiParent(null, null, null);


                    // fill referenced by tab
                    var dtRef = AdminProxy.ListTablesAndDataviewsByCodeGroup(groupName).Tables["list_tables_and_dataviews_by_code_group"];
                    lvReferencedBy.Items.Clear();
                    var tables = 0;
                    var dvs = 0;
                    foreach (DataRow drR in dtRef.Rows) {
                        var dv = drR["dataview_name"].ToString();
                        var lvi = new ListViewItem(dv);
                        if (!String.IsNullOrEmpty(dv)) {
                            dvs++;
                        } else {
                            tables++;
                        }
                        lvi.SubItems.Add(drR["table_name"].ToString());
                        lvi.SubItems.Add(drR["field_name"].ToString());
                        lvi.SubItems.Add((Toolkit.ToDateTime(drR["last_touched"], DateTime.MinValue)).ToLocalTime().ToString());
                        lvReferencedBy.Items.Add(lvi);
                    }
                    tpReferencedBy.Text = "Referenced By ( " + dvs + " / " + tables + " )"; 
                }

                MarkClean();

            });

            syncGui();
        }

        private void syncGui() {
            Sync(true, delegate() {
                btnSave.Enabled = IsDirty();



            });
        }

        private void frmCodeGroup_Load(object sender, EventArgs e) {
            initDropDowns();
        }

        private void initDropDowns() {
            Sync(true, delegate() {
                initLanguageDropDown(ddlLanguage, true, null);
                ddlLanguage.SelectedIndex = getSelectedIndex(ddlLanguage, AdminProxy.LanguageID.ToString());
            });
        }

        private void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e) {
            // TODO: is dirty check...
            if (!IsSyncing) {
                if (promptToSave()) {
                    RefreshData();
                }
            }
        }

        private bool promptToSave() {
            if (IsDirty()) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToSave{body}", "All your changes will be lost.\nContinue?"), 
                    getDisplayMember("promptToSave{title}", "Ignore changes?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            if (!promptToSave()) {
                return;
            }
            DialogResult = DialogResult.Cancel;
            if (!Modal) {
                MainFormSelectParentTreeNode();
            } else {
                Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {

            save();
        }

        private bool save(){
            // if name changed, tell user it might bust something...
            if (IsControlDirty(txtName)) {
                if (lvReferencedBy.Items.Count > 0) {
                    if (DialogResult.No == MessageBox.Show(this, 
                        getDisplayMember("save{referenced_body}", "This group is referenced by {0} tables and dataviews.\r\nChanging the name will update those references as well.\r\nDo you want to continue?", lvReferencedBy.Items.Count .ToString()), 
                        getDisplayMember("save{referenced_title}", "Update Group References?"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) {
                        return false;
                    }
                }

                var dt = AdminProxy.ListCodeGroups(null).Tables["list_code_groups"];
                var drs = dt.Select("group_name = '" + txtName.Text.Replace("'", "''") + "'");
                if (drs.Length > 0) {
                    MessageBox.Show(this, 
                        getDisplayMember("save{uniquegroup_body}", "The code group name must be unique.  {0} already exists.\r\nPlease specify a different one.", txtName.Text),
                        getDisplayMember("save{uniquegroup_title}", "Group Name Exists"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtName.Focus();
                    txtName.SelectAll();
                    return false;
                }


            }

            if (lvValues.Items.Count == 0) {
                if (DialogResult.No == MessageBox.Show(this, 
                    getDisplayMember("save{onevalueminimum_body}", "A code group must contain at least one value.\r\nWould you like to add one now?"), 
                    getDisplayMember("save{onevalueminimum_title}", "Value Required"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    return false;
                } else {
                    var f = new frmCodeValue();
                    f.GroupName = txtName.Text;
                    if (DialogResult.Cancel == MainFormPopupForm(f, this, false)) {
                        RefreshData();
                        return true;
                    }
                }
            }

            if (String.IsNullOrEmpty(_originalGroupName)) {
                this.Close();
                MainFormRefreshData();
                MainFormUpdateStatus(getDisplayMember("save{added}", "Added code group '{0}'", txtName.Text), true);
            } else {
                // if deleted row(s), check existing data for orphaned values...
                AdminProxy.RenameCodeGroup(_originalGroupName, txtName.Text);
                var nd = MainFormCurrentNode();
                nd.Tag = txtName.Text;
                nd.Text = txtName.Text;
                RefreshData();
                MainFormUpdateStatus(getDisplayMember("save{renamed}", "Renamed group to '{0}'", txtName.Text), true);
            }
            return true;

        }

        private void lvValues_MouseDoubleClick(object sender, MouseEventArgs e) {
            showValueForm();
        }

        private List<string> _touchedTables;

        private void showValueForm() {
            if (lvValues.SelectedItems.Count > 0) {
                var origValue = lvValues.SelectedItems[0].Text;

                var f = new frmCodeValue();
                f.GroupName = txtName.Text;
                f.ID = Toolkit.ToInt32(lvValues.SelectedItems[0].Tag, -1);

                if (DialogResult.OK == MainFormPopupForm(f, this, false)) {

                    if (_touchedTables == null) {
                        _touchedTables = new List<string>();
                    }

                    if (f.TouchedTables != null) {
                        foreach (var t in f.TouchedTables) {
                            if (!_touchedTables.Contains(t)) {
                                _touchedTables.Add(t);
                            }
                        }
                    }

                    RefreshData();
                    MainFormUpdateStatus(getDisplayMember("showValueForm{saved}", "Saved changes to code value '{0}'", origValue), true);
                }
            }
        }

        private void lvValues_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                showValueForm();
            }
        }

        private void lvValues_SelectedIndexChanged(object sender, EventArgs e) {
            if (lvValues.SelectedItems.Count > 0) {
                _selectedFieldID = Toolkit.ToInt32(lvValues.SelectedItems[0].Tag, -1);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private void cmiProperties_Click(object sender, EventArgs e) {
            showValueForm();
        }

        private void cmiRefresh_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void cmiExportList_Click(object sender, EventArgs e) {
            ExportListView(lvValues);
        }

        private void cmiReferenceExportList_Click(object sender, EventArgs e) {
            ExportListView(lvReferencedBy);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void cmiDelete_Click(object sender, EventArgs e) {
            promptToDelete();
        }

        private void promptToDelete(){
            if (lvValues.SelectedItems.Count > 0) {
                if (DialogResult.No == MessageBox.Show(this, 
                    getDisplayMember("promptToDelete{body}", "Are you sure you want to delete these values?"), 
                    getDisplayMember("promptToDelete{title}", "Delete Values?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    return;
                } else {
                    var delcount = 0;
                    var skipcount = 0;
                    var dtTables = AdminProxy.ListTablesAndDataviewsByCodeGroup(txtName.Text).Tables["list_tables_and_dataviews_by_code_group"];
                    var tables = new List<string>();
                    var fields = new List<string>();
                    foreach (DataRow dr in dtTables.Rows) {
                        var tbl = dr["table_name"].ToString();
                        if (!String.IsNullOrEmpty(tbl)) {
                            tables.Add(tbl);
                            fields.Add(dr["field_name"].ToString());
                        }
                    }
                    foreach (ListViewItem lvi in lvValues.SelectedItems) {
                        var rowcount = 0;
                        for (var i = 0; i < tables.Count; i++) {
                            rowcount += AdminProxy.GetCodeValueUsageCount(lvi.Text, tables[i], fields[i]);
                        }

                        if (rowcount > 0) {
                            skipcount++;
                            MessageBox.Show(this, 
                                getDisplayMember("promptToDelete{rowcount_body}", "Cannot delete value '{0}'.  {1} rows of data are currently using it.", lvi.Text , rowcount.ToString()),
                                getDisplayMember("promptToDelete{rowcount_title}", "Value In Use"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        } else {
                            AdminProxy.DeleteCodeValue(Toolkit.ToInt32(lvi.Tag, -1), txtName.Text);
                            delcount++;
                        }
                    }
                    if (delcount > 0) {
                        RefreshData();
                    }
                    MainFormUpdateStatus(getDisplayMember("promptToDelete{deleted}", "Deleted {0} code values, skipped {1}",delcount.ToString(), skipcount.ToString()), true);
                }
            }
        }

        private void lvValues_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDelete();
            }
        }

        private void cmiNew_Click(object sender, EventArgs e) {
            promptForNewValue();
        }

        private void promptForNewValue(){
            var f = new frmCodeValue();
            f.GroupName = txtName.Text;
            if (DialogResult.OK == MainFormPopupForm(f, this, false)) {
                _originalGroupName = txtName.Text;
                RefreshData();
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e) {
            promptForNewValue();
        }

        private void frmCodeGroup_FormClosing(object sender, FormClosingEventArgs e) {
            //if (_touchedTables != null && _touchedTables.Count > 0) {
            //    if (DialogResult.Yes == MessageBox.Show(this, 
            //        getDisplayMember("formclosing{body}", "Some search engine indexes may be out of date.\nDo you want to refresh them now?"), 
            //        getDisplayMember("formclosing{title}", "Refresh Search Engine Indexes?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
            //        foreach(var t in _touchedTables){
            //            AdminProxy.RebuildSearchEngineIndex(t);
            //        }
            //    }
            //}
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmCodeGroup", resourceName, null, defaultValue, substitutes);
        }
    }
}
