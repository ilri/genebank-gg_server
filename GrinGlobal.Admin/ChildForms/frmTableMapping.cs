using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using System.Diagnostics;
using GrinGlobal.Admin.PopupForms;
using GrinGlobal.InstallHelper;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmTableMapping : GrinGlobal.Admin.ChildForms.frmBase {
        public frmTableMapping() {
            InitializeComponent();  tellBaseComponents(components);
            PrimaryTabControl = tc;
        }

        private void frmTableMapping_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvFieldMappings);
            MakeListViewSortable(lvRelationships);
        }

        private int _selectedFieldID;

        public int GetNextFieldID(bool markNextAsCurrent) {
            if (lvFieldMappings.SelectedItems.Count > 0) {
                for (var i = 0; i < lvFieldMappings.Items.Count; i++) {
                    var id = Toolkit.ToInt32(lvFieldMappings.Items[i].Tag, -1);
                    if (id == _selectedFieldID) {
                        if (i == lvFieldMappings.Items.Count - 1) {
                            return -1;
                        } else {
                            var nextID = Toolkit.ToInt32(lvFieldMappings.Items[i + 1].Tag, -1);
                            if (markNextAsCurrent) {
                                _selectedFieldID = nextID;
                            }
                            return nextID;
                        }
                    }
                }
            }
            return -1;
        }

        public int GetPreviousFieldID(bool markPreviousAsCurrent) {
            if (lvFieldMappings.SelectedItems.Count > 0) {
                for (var i = 0; i < lvFieldMappings.Items.Count; i++) {
                    var id = Toolkit.ToInt32(lvFieldMappings.Items[i].Tag, -1);
                    if (id == _selectedFieldID) {
                        if (i == 0){
                            return -1;
                        } else {
                            var prevID = Toolkit.ToInt32(lvFieldMappings.Items[i - 1].Tag, -1);
                            if (markPreviousAsCurrent) {
                                _selectedFieldID = prevID;
                            }
                            return prevID;
                        }
                    }
                }
            }
            return -1;

        }

        private void initDropDowns() {


        }

        private string _tableName;
        public override void RefreshData() {

            this.Text = "Table Mapping - " + MainFormCurrentNodeText("") + " - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

            if (ID > -1) {
                var ds = AdminProxy.ListTableFields(ID, -1, true);
                var dt = ds.Tables["list_table_fields"];
                if (dt.Rows.Count > 0) {
                    var dr = dt.Rows[0];
                    _tableName = dr["table_name"].ToString();
                    txtDatabaseArea.Text = dr["database_area_code"].ToString();
                    chkEnabled.Checked = dr["table_is_enabled"].ToString().ToUpper() == "Y";

                    lvFieldMappings.Items.Clear();
                    foreach (DataRow fld in dt.Rows) {
                        var lvi = new ListViewItem(fld["field_name"].ToString(), 0);


                        lvi.Tag = Toolkit.ToInt32(fld["sys_table_field_id"], -1);

                        lvi.SubItems.Add(convertFieldPurpose(fld["field_purpose"].ToString().ToUpper()));

                        lvi.SubItems.Add(convertFieldType(fld["field_type"].ToString().ToUpper()));

                        lvi.SubItems.Add(convertFieldGuiHint(fld["gui_hint"].ToString().ToUpper()));


                        lvi.SubItems.Add(fld["default_value"].ToString());
                        // stored as nullable, displaying required -- meaning it's just the opposite.
                        var required = fld["is_nullable"].ToString().ToUpper() == "Y" ? "N" : "Y";
                        lvi.SubItems.Add(required);

                        if (required == "Y") {
                            // show required field icon
                            lvi.ImageIndex = 3;
                        }

                        lvi.SubItems.Add(fld["is_readonly"].ToString());
                        lvi.SubItems.Add(fld["is_foreign_key"].ToString());
                        lvi.SubItems.Add(fld["foreign_key_table_field_name"].ToString());
                        lvi.SubItems.Add(fld["foreign_key_dataview_name"].ToString());
                        lvi.SubItems.Add(fld["group_name"].ToString());


                        lvFieldMappings.Items.Add(lvi);
                    }
                }

                lvRelationships.Items.Clear();
                var dtRel = ds.Tables["list_table_relationships"];
                if (dtRel.Rows.Count > 0) {
                    foreach(DataRow drRel in dtRel.Rows){
                        var lvi = new ListViewItem(drRel["table_name"].ToString() + "." + drRel["field_name"].ToString(), 1);
                        var relType = drRel["relationship_type_tag"].ToString();
                        switch (relType) {
                            case "PARENT":
                                lvi.SubItems.Add("Parent");
                                break;
                            case "OWNER_PARENT":
                                lvi.SubItems.Add("Parent and owner");
                                break;
                            case "SELF":
                                lvi.SubItems.Add("Self-referential parent");
                                break;
                            default:
                                lvi.SubItems.Add(relType);
                                break;
                        }
                        lvi.SubItems.Add(drRel["table_name2"].ToString() + "." + drRel["field_name2"].ToString());
                        lvi.Tag = drRel["sys_table_relationship_id"];
                        lvRelationships.Items.Add(lvi);
                    }
                }

                lvIndexes.Items.Clear();
                var dtIndex = ds.Tables["list_table_indexes"];
                if (dtIndex.Rows.Count > 0) {
                    for (var i = 0; i < dtIndex.Rows.Count; i++) {
                        var drIndex = dtIndex.Rows[i];

                        var lvi = new ListViewItem(drIndex["index_name"].ToString(), 2);
                        lvi.SubItems.Add(drIndex["is_unique"].ToString().ToUpper() == "Y" ? "Y" : "N");
                        lvi.Tag = Toolkit.ToInt32(drIndex["sys_index_id"], -1);

                        var fields = new List<string>();
                        var prevName = dtIndex.Rows[i]["index_name"].ToString();
                        var curName = dtIndex.Rows[i]["index_name"].ToString();
                        while (curName == prevName){
                            fields.Add(dtIndex.Rows[i]["field_name"].ToString());
                            if (i < dtIndex.Rows.Count-1) {
                                curName = dtIndex.Rows[i + 1]["index_name"].ToString();
                            } else {
                                curName = null;
                            }

                            if (curName == prevName) {
                                i++;
                            }
                        }

                        lvi.SubItems.Add("(" + fields.Count + ") " + String.Join(", ", fields.ToArray()));
                        lvIndexes.Items.Add(lvi);
                    }
                }


            } else {

            }

            MarkClean();

        }

        private void btnSave_Click(object sender, EventArgs e) {
            save(!Modal);
        }

        private void save(bool showParent){
            //ID = AdminProxy.SaveTableMapping(Toolkit.ToInt32(ID, -1),
            //    txtName.Text,
            //    txtDescription.Text,
            //    Toolkit.ToInt32(ddlDataView.SelectedValue, -1),
            //    Toolkit.ToInt32(ddlTable.SelectedValue, -1),
            //    ddlCreate.Text,
            //    ddlRead.Text,
            //    ddlUpdate.Text,
            //    ddlDelete.Text,
            //    chkEnabled.Checked);
            //if (showParent) {
            //    MainFormSelectParentTreeNode();
            //    MainFormUpdateStatus("Saved TableMapping " + txtName.Text);
            //    DialogResult = DialogResult.OK;
            //    this.Close();
            //} else {
            //    RefreshData();
            //    MainFormUpdateStatus("Saved TableMapping " + txtName.Text);
            //    DialogResult = DialogResult.OK;
            //    this.Close();
            //}
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            if (Modal) {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            } else {
                MainFormSelectParentTreeNode();
            }
        }

        private void showProperties(int permFieldID) {
            frmTableMapping fpf = new frmTableMapping();
            //fpf.ID = permFieldID;
            //fpf.TableMappingID = ID;
            //fpf.DataViewID = Toolkit.ToInt32(ddlDataView.SelectedValue, -1);
            //fpf.TableID = Toolkit.ToInt32(ddlTable.SelectedValue, -1);
            //if (DialogResult.OK == MainFormPopupForm(fpf, false)) {
            //    RefreshData();
            //}
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            //if (lvTableFields.SelectedItems.Count == 1) {
            //    showProperties(Toolkit.ToInt32(lvTableFields.SelectedItems[0].Tag, -1));
            //}
        }

        private void lvPermFields_KeyUp(object sender, KeyEventArgs e) {
            //if (e.KeyCode == Keys.Delete) {
            //    deleteToolStripMenuItem.PerformClick();
            //} else if (e.KeyCode == Keys.Enter) {
            //    propertiesToolStripMenuItem.PerformClick();
            //}
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            deleteItems(true);
        }

        private void deleteItems(bool promptFirst) {
            //if (lvTableFields.SelectedItems.Count > 0) {
            //    if (promptFirst) {
            //        if (DialogResult.No == MessageBox.Show(this, "You are about to permanently delete restriction(s) from this TableMapping.\nDo you want to continue?", "Delete Restriction(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
            //            return;
            //        }
            //    }

            //    foreach (ListViewItem lvi in lvTableFields.SelectedItems) {
            //        //AdminProxy.DeleteTableMappingField(Toolkit.ToInt32(lvi.Tag, -1));
            //    }
            //    RefreshData();
            //}
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e) {
            //deleteToolStripMenuItem.Enabled = lvTableFields.SelectedItems.Count > 0;
            //propertiesToolStripMenuItem.Enabled = lvTableFields.SelectedItems.Count == 1;
        }

        private void lvTableFields_MouseDoubleClick(object sender, MouseEventArgs e) {
            //if (lvTableFields.SelectedItems.Count == 1) {
            //    showProperties(Toolkit.ToInt32(lvTableFields.SelectedItems[0].Tag, -1));
            //}

        }

        private void txtDescription_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            Debug.WriteLine("PreviewKeyDown: " + (e.Control ? "CTRL-" : "") + (e.Alt ? "ALT-" : "") + (e.Shift ? "SHFT-" : "") + e.KeyCode.ToString());
        }

        private void txtDescription_TextChanged(object sender, EventArgs e) {

        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e) {
            Debug.WriteLine("KeyDown: " + (e.Control ? "CTRL-" : "") + (e.Alt ? "ALT-" : "") + (e.Shift ? "SHFT-" : "") + e.KeyCode.ToString());

        }

        private void refreshGUI(int fieldMappingID) {

        }

        private void lvFieldMappings_SelectedIndexChanged(object sender, EventArgs e) {
            if (lvFieldMappings.SelectedItems.Count > 0) {
                _selectedFieldID = Toolkit.ToInt32(lvFieldMappings.SelectedItems[0].Tag, -1);
                refreshGUI(_selectedFieldID);
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            MainFormSelectParentTreeNode();
        }

        private void btnAddNew_Click(object sender, EventArgs e) {
            var f = new frmTableMappingField();
            f.TableID = this.ID;
            MainFormPopupNewItemForm(f);
        }

        private void defaultPermissionMenuItem_Click(object sender, EventArgs e) {
            showProperties();
        }

        private void showProperties() {
            if (lvFieldMappings.SelectedIndices.Count == 1) {
                var f = new frmTableMappingField();
                f.ID = Toolkit.ToInt32(lvFieldMappings.SelectedItems[0].Tag, -1);
                if (MainFormPopupForm(f, this, false) == DialogResult.OK){
                    RefreshData();
                }
            }
        }

        private void ctxMenuFieldMapping_Opening(object sender, CancelEventArgs e) {
            cmiProperties.Enabled = lvFieldMappings.SelectedIndices.Count == 1;
            cmiDelete.Enabled = lvFieldMappings.SelectedIndices.Count > 0;
        }

        private void deleteTableMappingsMenuItem_Click(object sender, EventArgs e) {
            promptToDelete();
        }

        private void newTableMappingMenuItem_Click(object sender, EventArgs e) {
            var f = new frmTableMappingField();
            f.TableID = this.ID;
            MainFormPopupNewItemForm(f);
        }

        private void lvFieldMappings_MouseDoubleClick(object sender, MouseEventArgs e) {
            showProperties();

        }

        private void promptToDelete() {
            if (lvFieldMappings.SelectedItems.Count > 0) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToDelete{start_body}", "Are you sure you want to delete field(s)?"), 
                    getDisplayMember("promptToDelete{start_title}", "Delete field(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    var total = 0;
                    try {
                        foreach (ListViewItem lvi in lvFieldMappings.SelectedItems) {
                            try {
                                AdminProxy.DeleteTableFieldMapping(Toolkit.ToInt32(lvi.Tag, -1), false);
                                total++;
                            } catch (Exception ex) {
                                if (ex.Message.Contains("Cannot delete field mapping")) {
                                    if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("delete{references_body}", "{0}\r\n\r\nDo you want to remove the reference(s) and continue deleting {1}?", ex.Message, lvi.Text),
                                        getDisplayMember("delete{references_title}", "Remove References and Continue Delete?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                                        AdminProxy.DeleteTableFieldMapping(Toolkit.ToInt32(lvi.Tag, -1), true);
                                        total++;
                                    } else {
                                        // nothing to do
                                    }
                                } else {
                                    throw;
                                }
                            }
                        }
                        MainFormUpdateStatus(getDisplayMember("promptToDelete{deleted}", "Deleted {0} fields.", lvFieldMappings.SelectedItems.Count.ToString("###,##0")), true);
                        RefreshData();
                    } catch (Exception ex) {
                        MessageBox.Show(this, ex.Message, getDisplayMember("promptToDelete{failed_title}", "Error Deleting Field"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void lvFieldMappings_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDelete();
            }

        }

        private void refreshPermissionMenuItem_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void lvFieldMappings_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                showProperties();
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e) {

            ID = AdminProxy.UpdateTableMapping(ID, txtDatabaseArea.Text, chkEnabled.Checked);
            MarkClean();
            MainFormSelectParentTreeNode();
        }

        private void cmiRelationshipNew_Click(object sender, EventArgs e) {
            var f = new frmTableRelationship();
            f.FromTableName = _tableName;
            f.FromTableID = this.ID;
            f.ID = 0;
            if (DialogResult.OK == MainFormPopupForm(f, this, true)) {
                RefreshData();
                MainFormUpdateStatus(getDisplayMember("addRelationship{done}", "Added new relationship to {0}", _tableName), true);
            }
        }

        private void cmiRelationshipDelete_Click(object sender, EventArgs e) {
            promptToDeleteRelationship();
        }

        private void cmiRelationshipRefresh_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void cmiRelationshipProperties_Click(object sender, EventArgs e) {
            showPropertiesRelationship();
        }

        private void cmRelationship_Opening(object sender, CancelEventArgs e) {
            cmiRelationshipProperties.Enabled = lvRelationships.SelectedIndices.Count == 1;
            cmiRelationshipDelete.Enabled = lvRelationships.SelectedIndices.Count > 0;

        }

        private void promptToDeleteRelationship() {
            if (lvRelationships.SelectedItems.Count > 0) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToDeleteRelationship{start_body}", "Are you sure you want to delete relationship(s)?"), 
                    getDisplayMember("promptToDeleteRelationship{start_title}", "Delete relationship(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    var count = 0;
                    foreach (ListViewItem lvi in lvRelationships.SelectedItems) {
                        AdminProxy.DeleteTableRelationship(Toolkit.ToInt32(lvi.Tag, -1));
                        count++;
                    }
                    RefreshData();
                    MainFormUpdateStatus(getDisplayMember("promptToDeleteRelationship{done}", "Deleted {0} relationship(s)", count.ToString("###,##0")), true);
                }
            }
        }

        private void showPropertiesRelationship() {
            if (lvRelationships.SelectedIndices.Count == 1) {
                var f = new frmTableRelationship();
                f.ID = Toolkit.ToInt32(lvRelationships.SelectedItems[0].Tag, -1);
                f.FromTableID = this.ID;
                f.FromTableName = _tableName;
                if (MainFormPopupForm(f, this, false) == DialogResult.OK) {
                    RefreshData();
                    MainFormUpdateStatus(getDisplayMember("saveRelationship{done}", "Saved relationship changes to {0}", _tableName), true);
                }
            }
        }

        private void lvRelationships_MouseDoubleClick(object sender, MouseEventArgs e) {
            showPropertiesRelationship();
        }

        private void lvRelationships_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDeleteRelationship();
            }

        }

        private void lvRelationships_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                showPropertiesRelationship();
            }

        }

        private void cmiRelationshipAutoCreate_Click(object sender, EventArgs e) {
            using (new AutoCursor(this)) {
                var names = new List<string>();
                //foreach(ListViewItem lvi in lvRelationships.SelectedItems){
                //    names.Add(lvi.Tag.ToString());
                //}
                MainFormUpdateStatus(getDisplayMember("createRelationships{start}", "Creating new relationship(s)..."), false);
                names.Add(_tableName);
                AdminProxy.RecreateTableRelationships(names);
                RefreshData();
                MainFormUpdateStatus(getDisplayMember("createRelationships{done}", "Created new relationship(s) for {0} table(s).", names.Count.ToString("###,##0")), true);
            }
        }

        private void cmiIndexInspect_Click(object sender, EventArgs e) {
            using (new AutoCursor(this)) {
                var names = new List<string>();
                //foreach (ListViewItem lvi in lvIndexes.SelectedItems) {
                //    names.Add(lvi.Tag.ToString());
                //}
                names.Add(_tableName);
                MainFormUpdateStatus(getDisplayMember("createIndex{start}", "Creating new index(es)..."), false);
                AdminProxy.RecreateTableIndexes(names);
                RefreshData();
                MainFormUpdateStatus(getDisplayMember("createIndex{done}", "Created new index(es) for {0} table(s)", names.Count.ToString("###,##0")), true);
            }

        }

        private void cmiIndexDelete_Click(object sender, EventArgs e) {
            promptToDeleteIndex();

        }

        private void promptToDeleteIndex() {
            if (lvIndexes.SelectedItems.Count > 0) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToDeleteIndex{start_body}", "Are you sure you want to delete index(es)?"),
                    getDisplayMember("promptToDeleteIndex{start_title}", "Delete index(es)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    try {
                        foreach (ListViewItem lvi in lvIndexes.SelectedItems) {
                            AdminProxy.DeleteTableIndex(Toolkit.ToInt32(lvi.Tag, -1));
                        }
                        MainFormUpdateStatus(getDisplayMember("promptToDelete{done}", "Deleted {0} index(es)", lvIndexes.SelectedItems.Count.ToString("###,##0")), true);
                        RefreshData();
                    } catch (Exception ex) {
                        MessageBox.Show(this, ex.Message, getDisplayMember("promptToDelete{failed_title}", "Error Deleting Index"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void cmiIndexProperties_Click(object sender, EventArgs e) {

            showPropertiesIndex();
        }

        private void showPropertiesIndex() {
            //if (lvIndexes.SelectedIndices.Count == 1) {
            //    var f = new frmTableIndex();
            //    f.ID = Toolkit.ToInt32(lvIndexes.SelectedItems[0].Tag, -1);
            //    if (MainFormPopupForm(f, this, false) == DialogResult.OK) {
            //        RefreshData();
            //        MainFormUpdateStatus("Saved index changes ", true);
            //    }
            //}
        }

        private void cmiIndexRefresh_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void cmIndex_Opening(object sender, CancelEventArgs e) {
            cmiIndexDelete.Enabled = lvIndexes.SelectedItems.Count > 0;

        }

        private void txtDatabaseArea_TextChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private void cmiFieldExportList_Click(object sender, EventArgs e) {
            ExportListView(lvFieldMappings);

        }

        private void cmiRelationshipExportList_Click(object sender, EventArgs e) {
            ExportListView(lvRelationships);

        }

        private void cmiIndexExportList_Click(object sender, EventArgs e) {
            ExportListView(lvIndexes);

        }

        private void cmiFieldShowDependencies_Click(object sender, EventArgs e) {
            if (lvFieldMappings.SelectedItems.Count == 1) {

                var fieldName = lvFieldMappings.SelectedItems[0].Text.ToString();
                var dt = AdminProxy.GetTableFieldDependencies(_tableName, fieldName).Tables["table_field_dependencies"];

                var msg = new frmMessageBox();
                msg.btnYes.Visible = false;
                msg.Text = "Dependencies for " + _tableName + "." + fieldName;
                msg.btnNo.Text = "OK";

                var txt = "";
                if (dt.Rows.Count == 0) {
                    txt = "(None)";
                } else {
                    foreach (DataRow dr in dt.Rows) {
                        txt += dr["display_member"].ToString() + "\r\n";
                    }
                }
                txt += "\r\n\r\nNote: Only dataview fields explicitly mapped to this table/field are shown above.  The SQL statement within a dataview may still use it.\r\n\r\nIf editing or removing this field, please verify all dataviews afterwards to ensure system integrity.";
                msg.txtMessage.Text = txt;

                msg.ShowDialog(this);
            }

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmTableMapping", resourceName, null, defaultValue, substitutes);
        }

    }
}
