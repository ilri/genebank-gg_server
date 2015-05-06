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
    public partial class frmFileGroup : GrinGlobal.Admin.ChildForms.frmBase {
        public frmFileGroup() {
            InitializeComponent();  tellBaseComponents(components);

        }

        private int _selectedFieldID;

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

            this.Text = "File Group - " + MainFormCurrentNodeText("") + " - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

            if (ID > 0) {

                var dtGroup = AdminProxy.ListFileGroups(ID).Tables["list_file_groups"];
                if (dtGroup != null && dtGroup.Rows.Count > 0) {
                    var drG = dtGroup.Rows[0];
                    txtName.Text = drG["group_name"].ToString();
                    txtVersion.Text = drG["version_name"].ToString();
                    chkEnabled.Checked = drG["is_enabled"].ToString().Trim().ToUpper() == "Y";

                    var dtFiles = AdminProxy.ListFilesByGroup(ID, true).Tables["files_by_group"];
                    lv.Items.Clear();
                    foreach (DataRow fld in dtFiles.Rows) {
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
            }

            MarkClean();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            save(!Modal);
        }

        private void save(bool showParent){

            ID = AdminProxy.SaveFileGroup(ID, txtName.Text, txtVersion.Text, chkEnabled.Checked);

            if (showParent) {
                MainFormSelectParentTreeNode();
            } else {
                RefreshData();
            }

            MainFormUpdateStatus(getDisplayMember("save{done}", "Saved {0}", txtName.Text), true);
            DialogResult = DialogResult.OK;
            Close();

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

        private void defaultPermissionMenuItem_Click(object sender, EventArgs e) {
            showProperties();
        }

        private void showProperties() {
            if (lv.SelectedIndices.Count == 1) {
                var f = new frmFiles();
                f.ID = Toolkit.ToInt32(lv.SelectedItems[0].Tag, -1);
                //f.ShowInGroupOnly = true;
                if (MainFormPopupForm(f, this, false) == DialogResult.OK) {
                    RefreshData();
                }
            }
        }

        private void ctxMenuFieldMapping_Opening(object sender, CancelEventArgs e) {
            cmiProperties.Enabled = lv.SelectedIndices.Count == 1;
            cmiRemove.Enabled = lv.SelectedIndices.Count > 0;


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

        private void promptToRemove() {
            if (lv.SelectedItems.Count > 0) {
                if (DialogResult.Yes == MessageBox.Show(this, getDisplayMember("promptToRemove{prompt_body}", "Are you sure you want to remove file(s) from this group?\nThe file will not be deleted."), 
                    getDisplayMember("promptToRemove{prompt_title}", "Remove file(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lv.SelectedItems) {
                        AdminProxy.RemoveFileFromGroup(ID, Toolkit.ToInt32(lvi.Tag, -1));
                    }
                    MainFormUpdateStatus(getDisplayMember("promptToRemove{done}", "Removed {0} files", lv.SelectedItems.Count.ToString("###,##0")), true);
                    RefreshData();
                }
            }
        }

        private void lv_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToRemove();
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
            promptToRemove();
        }

        private void frmFileGroup_Load(object sender, EventArgs e) {
            MakeListViewSortable(lv);
        }

        private void newMenuItem_Click_1(object sender, EventArgs e) {
            showAddFile();
        }

        private void showAddFile(){
            var f = new frmFiles();
            f.GroupID = ID;
            f.ShowInGroupOnly = false;
            var result = MainFormPopupForm(f, this, false);
            if (result == DialogResult.OK) {
                foreach (var id in f.SelectedIDs) {
                    AdminProxy.AddFileToGroup(ID, id);
                }
                RefreshData();
            }
        }

        private void enableMenuItem_Click(object sender, EventArgs e) {
            if (lv.SelectedItems.Count > 0) {
                foreach (ListViewItem lvi in lv.SelectedItems) {
                    AdminProxy.EnableFile(Toolkit.ToInt32(lvi.Tag, -1));
                }
                MainFormUpdateStatus(getDisplayMember("enable{done}", "Enabled {0} file(s)", lv.SelectedItems.Count.ToString("###,##0")), true);
                RefreshData();
            }

        }

        private void disableMenuItem_Click(object sender, EventArgs e) {
            if (lv.SelectedItems.Count > 0) {
                foreach (ListViewItem lvi in lv.SelectedItems) {
                    AdminProxy.DisableFile(Toolkit.ToInt32(lvi.Tag, -1));
                }
                MainFormUpdateStatus(getDisplayMember("disable{done}", "Disabled {0} file(s)", lv.SelectedItems.Count.ToString("###,##0")), true);
                RefreshData();
            }

        }

        private void btnAddFile_Click(object sender, EventArgs e) {
            showAddFile();
        }

        private void btnSave_Click_2(object sender, EventArgs e) {
            save(!Modal);
        }

        private void btnCancel_Click_2(object sender, EventArgs e) {
            if (Modal) {
                Close();
            } else {
                MainFormSelectParentTreeNode();
            }
        }

        private void lv_MouseDoubleClick_1(object sender, MouseEventArgs e) {
            showProperties();
        }

        private void txtName_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtVersion_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void cmiFileGroupExportList_Click(object sender, EventArgs e) {
            ExportListView(lv);

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmFileGroup", resourceName, null, defaultValue, substitutes);
        }
    }
}
