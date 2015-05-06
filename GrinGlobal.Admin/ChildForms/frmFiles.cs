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

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmFiles : GrinGlobal.Admin.ChildForms.frmBase {
        public frmFiles() {
            InitializeComponent();  tellBaseComponents(components);

        }

        private int _selectedFieldID;
        public bool ShowInGroupOnly;
        public int GroupID;

        public List<int> AssignedIDs = new List<int>();
        public List<int> SelectedIDs = new List<int>();


        public int GetNextFieldID(bool markNextAsCurrent) {
            if (lv.SelectedItems.Count > 0) {
                for (var i = 0; i < lv.Items.Count; i++) {
                    var id = Toolkit.ToInt32(lv.Items[i].Tag, -1);
                    if (id == _selectedFieldID) {
                        if (i == lv.Items.Count - 1) {
                            return -1;
                        } else {
                            var nextID = Toolkit.ToInt32(lv.Items[i + 1].Tag, -1);
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
            if (lv.SelectedItems.Count > 0) {
                for (var i = 0; i < lv.Items.Count; i++) {
                    var id = Toolkit.ToInt32(lv.Items[i].Tag, -1);
                    if (id == _selectedFieldID) {
                        if (i == -1) {
                            return -1;
                        } else {
                            var prevID = Toolkit.ToInt32(lv.Items[i - 1].Tag, -1);
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

        public override void RefreshData() {

            DataTable dt = null;
            if (GroupID > -1) {
                dt = AdminProxy.ListFilesByGroup(GroupID, ShowInGroupOnly).Tables["files_by_group"];
                if (ShowInGroupOnly) {
                    this.Text = "Files - In " + MainFormCurrentNodeText("") + " - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
                } else {
                    this.Text = "Files - Not In Group '" + MainFormCurrentNodeText("") + "' - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
                }
                btnAddToGroup.Visible = true;
                btnCancel.Text = "&Cancel";
                this.AcceptButton = btnAddToGroup;
            } else {
                dt = AdminProxy.ListFiles(ID).Tables["list_files"];
                if (dt.Rows.Count > 0){
                    this.Text = "Files - " + dt.Rows[0]["display_name"] + " - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
                } else {
                    this.Text = "Files - (not found) - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
                }
                btnAddToGroup.Visible = false;
                btnCancel.Text = "&Done";
                this.AcceptButton = btnCancel;
            }

            lv.Items.Clear();
            foreach (DataRow fld in dt.Rows) {
                var lvi = new ListViewItem(fld["display_name"].ToString());
                lvi.Tag = Toolkit.ToInt32(fld["sys_file_id"], -1);

                lvi.SubItems.Add(fld["file_name"].ToString());
                lvi.SubItems.Add(fld["file_version"].ToString());
                lvi.SubItems.Add((Toolkit.ToDecimal(fld["file_size"], 0.0M) / 1024.0M / 1024.0M).ToString("###,###,##0.00"));

                lvi.SubItems.Add(fld["is_enabled"].ToString());
                lvi.SubItems.Add(fld["virtual_file_path"].ToString());

                var lastTouched = fld["modified_date"] == DBNull.Value ? fld["created_date"] : fld["modified_date"];
                lvi.SubItems.Add(((DateTime)lastTouched).ToString());

                lv.Items.Add(lvi);
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            save(!Modal);
        }

        private void save(bool showParent){
            //ID = AdminProxy.Save(Toolkit.ToInt32(ID, -1),
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
            //    MainFormUpdateStatus("Saved  " + txtName.Text);
            //    DialogResult = DialogResult.OK;
            //    this.Close();
            //} else {
            //    RefreshData();
            //    MainFormUpdateStatus("Saved  " + txtName.Text);
            //    DialogResult = DialogResult.OK;
            //    this.Close();
            //}
        }

        private void btnCancel_Click(object sender, EventArgs e) {

        }

        private void showProperties(int permFieldID) {
            //fpf.ID = permFieldID;
            //fpf.ID = ID;
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
            //        if (DialogResult.No == MessageBox.Show(this, "You are about to permanently delete restriction(s) from this .\nDo you want to continue?", "Delete Restriction(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
            //            return;
            //        }
            //    }

            //    foreach (ListViewItem lvi in lvTableFields.SelectedItems) {
            //        //AdminProxy.DeleteField(Toolkit.ToInt32(lvi.Tag, -1));
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

        private void lv_SelectedIndexChanged(object sender, EventArgs e) {
            if (lv.SelectedItems.Count > 0) {
                _selectedFieldID = Toolkit.ToInt32(lv.SelectedItems[0].Tag, -1);
                refreshGUI(_selectedFieldID);
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            MainFormSelectParentTreeNode();
        }

        private void btnAddNew_Click(object sender, EventArgs e) {
            //var f = new frmField();
            //f.TableID = this.ID;
            //MainFormPopupNewItemForm(f);
        }

        private void defaultPermissionMenuItem_Click(object sender, EventArgs e) {
            showProperties();
        }

        private void showProperties() {
            if (lv.SelectedIndices.Count == 1) {
                var f = new frmFile();
                f.ID = Toolkit.ToInt32(lv.SelectedItems[0].Tag, -1);
                if (MainFormPopupForm(f, this, false) == DialogResult.OK) {
                    RefreshData();
                }
            }
        }

        private void ctxMenuFieldMapping_Opening(object sender, CancelEventArgs e) {
            cmiProperties.Enabled = lv.SelectedIndices.Count == 1;
            cmiDelete.Enabled = lv.SelectedIndices.Count > 0;


            int toEnable = 0;
            int toDisable = 0;
            foreach (ListViewItem lvi in lv.SelectedItems) {
                if (lvi.SubItems[4].Text == "Y") {
                    toDisable++;
                } else {
                    toEnable++;
                }
            }

            cmiEnable.Enabled = toEnable > 0;
            cmiDisable.Enabled = toDisable > 0;
        }

        private void newMenuItem_Click(object sender, EventArgs e) {
            //var f = new frmField();
            //f.TableID = this.ID;
            //MainFormPopupNewItemForm(f);
        }

        private void lv_MouseDoubleClick(object sender, MouseEventArgs e) {
            showProperties();

        }

        private void promptToDelete() {
            if (lv.SelectedItems.Count > 0) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToDelete{prompt_body}", "Are you sure you want to delete the file(s)?\nThe file(s) will be permanently deleted."), 
                    getDisplayMember("promptToDelete{prompt_title}", "Delete file(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lv.SelectedItems) {
                        AdminProxy.DeleteFile(Toolkit.ToInt32(lvi.Tag, -1));
                    }
                    MainFormUpdateStatus(getDisplayMember("promptToDelete{done}", "Deleted {0} file(s)", lv.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
                }
            }
        }

        private void lv_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDelete();
            }

        }

        private void refreshPermissionMenuItem_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void lv_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                showProperties();
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e) {
            MainFormSelectParentTreeNode();
        }

        private void removeMenuItem_Click(object sender, EventArgs e) {
            promptToDelete();
        }

        private void frmFileGroup_Load(object sender, EventArgs e) {
            MakeListViewSortable(lv);
        }

        private void btnOK_Click(object sender, EventArgs e) {
            if (lv.SelectedItems.Count == 0) {
                MessageBox.Show(getDisplayMember("ok{pickone}", "You must select one or more files to add."));
            } else {
                SelectedIDs.Clear();
                foreach (ListViewItem lvi in lv.SelectedItems) {
                    SelectedIDs.Add(Toolkit.ToInt32(lvi.Tag, -1));
                }
                DialogResult = DialogResult.OK;
                this.Close();
            }

        }

        private void btnCancel_Click_2(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnNewFile_Click(object sender, EventArgs e) {
            if (DialogResult.OK == MainFormPopupForm(new frmFile(), this, true)) {
                RefreshData();
            }
            // var f = new frmFile();
            //f.ID = 0;
            //var result = f.ShowDialog(this);
            //if (result == DialogResult.OK) {
            //    RefreshData();
            //} else {
            //    // nothing special to do
            //}
        }

        private void disableMenuItem_Click(object sender, EventArgs e) {

        }

        private void disableMenuItem_Click_1(object sender, EventArgs e) {
            promptToDelete();
        }

        private void deleteMenuItem_Click(object sender, EventArgs e) {
            promptToDelete();
        }

        private void lv_MouseDoubleClick_1(object sender, MouseEventArgs e) {
            if (Modal) {
                btnAddToGroup.PerformClick();
            } else {
                showProperties();
            }

        }

        private void lv_KeyUp_1(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDelete();
            }

        }

        private void lv_KeyPress_1(object sender, KeyPressEventArgs e) {
            if ((Keys)e.KeyChar == Keys.Enter) {
                btnAddToGroup.PerformClick();
            }

        }

        private void lv_ItemDrag(object sender, ItemDragEventArgs e) {
            //startDrag(sender);

        }

        private void cmFilesExportList_Click(object sender, EventArgs e) {
            ExportListView(lv);

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmFiles", resourceName, null, defaultValue, substitutes);
        }
    }
}
