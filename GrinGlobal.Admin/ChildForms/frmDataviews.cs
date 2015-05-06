using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using GrinGlobal.Admin.PopupForms;
using GrinGlobal.InstallHelper;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmDataviews : GrinGlobal.Admin.ChildForms.frmBase {
        public frmDataviews() {
            InitializeComponent();  tellBaseComponents(components);
        }


        public List<int> AssignedIDs = new List<int>();
        public List<int> SelectedIDs = new List<int>();
        public string CategoryName;

        public override void RefreshData() {

            //lvPerms.MultiSelect = !Modal;

            this.Text = "Dataviews - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
            var tags = rememberSelectedTags(lvDataviews);

            // this form has multiple uses, as does the underlying ListPermissions method.
            // it can:
            // List all permissions (0,[0])
            // List permission info for a single permission (37,[0])
            // List permissions that are NOT in a list (0, [2,3,4,5,6])

            var specificCategory = this.CategoryName != null;

            var ds = AdminProxy.ListDataViews(-1, CategoryName, specificCategory, null, false);

            initHooksForMdiParent(ds.Tables["list_dataviews"], "dataview_name", "dataview_name", (specificCategory ? null : "category_code"));
            lvDataviews.Items.Clear();
            foreach (DataRow dr in ds.Tables["list_dataviews"].Rows) {
                var lvi = new ListViewItem(dr["dataview_name"].ToString(), 0);
                lvi.Tag = dr["dataview_name"].ToString();
                lvi.SubItems.Add(dr["title"].ToString());
                lvi.SubItems.Add(dr["category_name"].ToString());
                lvi.SubItems.Add(dr["database_area"].ToString());
                lvi.SubItems.Add((Toolkit.ToDateTime(dr["last_touched_date"], DateTime.MinValue)).ToLocalTime().ToString());
                lvi.SubItems.Add(dr["description"].ToString());
                lvDataviews.Items.Add(lvi);
            }

            selectRememberedTags(lvDataviews, tags);

            MainFormUpdateStatus(getDisplayMember("RefreshData{refreshed}", "Refreshed Dataviews"), false);
        }

        private void showProperties() {
            if (lvDataviews.SelectedItems.Count == 1) {
                MainFormSelectDescendentTreeNode(lvDataviews.SelectedItems[0].Tag.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();

        }

        private void frmDataviews_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvDataviews);
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
            if (lvDataviews.SelectedItems.Count > 0) {
                var total = 0;
                var fmb = new frmMessageBox();
                fmb.Text = "Delete Dataview(s)?";
                fmb.btnYes.Text = "&Delete";
                fmb.btnNo.Text = "&Cancel";

                var list = new List<string>();
                foreach (ListViewItem lvi in lvDataviews.SelectedItems) {
                    list.Add(lvi.Text);
                }


                fmb.txtMessage.Text = "Are you sure you want to delete the following dataview(s)?\r\n" + String.Join("\r\n", list.ToArray());
                if (DialogResult.Yes == fmb.ShowDialog(this)) {
                    foreach (ListViewItem lvi in lvDataviews.SelectedItems) {
                        try {
                            AdminProxy.DeleteDataViewDefinition(lvi.Tag + string.Empty, false);
                            total++;
                        } catch (Exception ex) {
                            if (ex.Message.Contains("following are referencing")) {
                                fmb.txtMessage.Text = ex.Message + "\r\n\r\nDo you want to remove the reference(s) and continue deleting " + lvi.Tag + "?";
                                fmb.Text = "Remove References and Continue Delete?";
                                fmb.btnYes.Text = "Continue &Deleting";
                                fmb.btnNo.Text = "&Cancel";
                                if (DialogResult.Yes == fmb.ShowDialog(this)) {
                                    AdminProxy.DeleteDataViewDefinition(lvi.Tag + string.Empty, true);
                                    total++;
                                } else {
                                    // nothing to do
                                }
                            } else if (ex.Message.Contains("permission(s) exist")){
                                fmb.txtMessage.Text = ex.Message + "\r\n\r\nDo you want to remove the permission(s) and continue deleting " + lvi.Tag + "?";
                                fmb.Text = "Remove Permissions and Continue Delete?";
                                fmb.btnYes.Text = "Continue &Deleting";
                                fmb.btnNo.Text = "&Cancel";
                                if (DialogResult.Yes == fmb.ShowDialog(this)) {
                                    AdminProxy.DeleteDataViewDefinition(lvi.Tag + string.Empty, true);
                                    total++;
                                } else {
                                    // nothing to do
                                }
                            } else if (ex.Message.Contains("required by the system")){
                                MessageBox.Show(this, getDisplayMember("promptToDelete{systemrequired_body}", "{0}\r\n\r\nYou must edit the dataview and remove it from the System category before it can be deleted.", ex.Message),
                                    getDisplayMember("promptToDelete{systemrequired_title}", "System-Required Dataview"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            } else {
                                throw;
                            }
                        }
                    }
                    MainFormRefreshData();
                    MainFormUpdateStatus(getDisplayMember("promptToDelete{deleted}", "Deleted {0} dataview(s).", total.ToString("###,##0")), true);
                }
            }
        }

        private void ctxMenuNodePermission_Opening(object sender, CancelEventArgs e) {
            cmiDelete.Enabled = lvDataviews.SelectedItems.Count > 0;
            cmiProperties.Enabled = lvDataviews.SelectedItems.Count == 1;
            cmiExport.Enabled = lvDataviews.SelectedItems.Count > 0;
            cmiVerify.Enabled = lvDataviews.SelectedItems.Count > 0;
            cmiCopyTo.Enabled = lvDataviews.SelectedItems.Count == 1;
            cmiRename.Enabled = lvDataviews.SelectedItems.Count == 1;
        }

        private void lvDataviews_MouseDoubleClick(object sender, MouseEventArgs e) {
            showProperties();

        }

        private void lvDataviews_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDelete();
            } else if (e.KeyCode == Keys.F2) {
                promptToRename();
            }
        }

        private void lvDataviews_KeyPress(object sender, KeyPressEventArgs e) {
        }

        private void cmiExport_Click(object sender, EventArgs e) {
            if (lvDataviews.SelectedItems.Count > 0) {

                sfdExport.FileName = DateTime.Now.ToString("yyyy_MM_dd_");
                if (lvDataviews.SelectedItems.Count == 1) {
                    sfdExport.FileName += lvDataviews.SelectedItems[0].Text;
                } else {
                    sfdExport.FileName += lvDataviews.SelectedItems.Count; 
                }
                sfdExport.OverwritePrompt = true;
                sfdExport.SupportMultiDottedExtensions = true;

                if (DialogResult.OK == sfdExport.ShowDialog(this)) {

                    var dvs = new List<string>();
                    var splash = new frmSplash();
                    foreach (ListViewItem lvi in lvDataviews.SelectedItems) {
                        dvs.Add(lvi.Tag.ToString());
                    }

                    splash.Show("Exporting " + dvs.Count + " dataview(s)...", false, this);

                    processInBackground(
                    (worker) => {
                        var ds = AdminProxy.ExportDataViewDefinitions(dvs);

                        BinaryFormatter bin = new BinaryFormatter();
                        using (StreamWriter sw = new StreamWriter(sfdExport.FileName)) {
                            ds.RemotingFormat = SerializationFormat.Binary;
                            bin.Serialize(sw.BaseStream, ds);
                        }
                    }, 
                    (worker2, e2) => {
                        splash.ChangeText(getDisplayMember("export{working}", "Still working..."));
                    }, 
                    (worker3, e3) => {
                        splash.Close();
                        MainFormUpdateStatus(getDisplayMember("export{exported}", "Exported {0} dataview(s)", dvs.Count.ToString("###,##0")), true);
                    });

                }
            }


        }

        private void cmiImport_Click(object sender, EventArgs e) {
            if (DialogResult.OK == ofdImport.ShowDialog(this)) {

                var fid = new frmImportDataviews();

                // show them a list of all the dataview names, let them select a subset
                foreach (var f in ofdImport.FileNames) {
                    fid.ImportFiles.Add(f);
                }

                if (DialogResult.OK == MainFormPopupForm(fid, this, false)) {
                    RefreshData();
                    MainFormUpdateStatus(getDisplayMember("import{imported}", "Imported {0} dataview(s)", fid.ImportedDataviews.Count.ToString("###,##0")), true);
                }
            }
        }

        private void cmiCopyTo_Click(object sender, EventArgs e) {
            var f = new frmCopyDataviewTo();
            f.Copying = true;
            var origName = lvDataviews.SelectedItems[0].Tag + string.Empty;
            f.OriginalDataviewName = origName;
            f.txtNewDataviewName.Text = origName;
            if (DialogResult.OK == MainFormPopupForm(f, this, false)) {
                var newName = f.txtNewDataviewName.Text.Trim();
                AdminProxy.CopyDataViewDefinition(origName, newName);
                MainFormRefreshData();
                MainFormUpdateStatus(getDisplayMember("copyTo{done}", "Copied {0} to {1}", origName , newName), true);
            }
        }

        private void cmiNew_Click(object sender, EventArgs e) {
            MainFormPopupNewItemForm(new frmDataview());
            //MainFormPopupNewItemForm(new frmDataview2());
        }

        private void cmiVerify_Click(object sender, EventArgs e) {
            var f = new frmVerifyDataviews();
            foreach(ListViewItem lvi in lvDataviews.SelectedItems){
                f.Dataviews.Add(lvi.Tag.ToString());
            }
            if (f.Dataviews.Count > 0) {
                MainFormPopupForm(f, this, false);
            }
        }

        private void cmiDataviewRename_Click(object sender, EventArgs e) {
            promptToRename();
        }

        private void promptToRename(){
            var f = new frmCopyDataviewTo();
            f.Copying = false;
            var origName = lvDataviews.SelectedItems[0].Tag + string.Empty;
            f.OriginalDataviewName = origName;
            f.txtNewDataviewName.Text = origName;
            if (DialogResult.OK == MainFormPopupForm(f, this, false)) {
                var newName = f.txtNewDataviewName.Text.Trim();
                AdminProxy.RenameDataViewDefinition(origName, newName);
                MainFormRefreshData();
                MainFormUpdateStatus(getDisplayMember("promptToRename{done}", "Renamed {0} to {1}", origName, newName), true);
            }
        }

        private void lvDataviews_MouseDown(object sender, MouseEventArgs e) {
        }

        private void lvDataviews_DragDrop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent("FileDrop")) {

                var fid = new frmImportDataviews();

                // show them a list of all the dataview names, let them select a subset
                string[] files = e.Data.GetData("FileDrop") as string[];
                foreach (string f in files) {
                    if (f.ToLower().EndsWith(".dat") || f.ToLower().EndsWith(".dataview")) {
                        fid.ImportFiles.Add(f);
                    }
                }

                if (fid.ImportFiles.Count > 0) {
                    if (DialogResult.OK == MainFormPopupForm(fid, this, false)) {
                        RefreshData();
                        MainFormUpdateStatus(getDisplayMember("dataviewsDragDrop{imported}", "Imported {0} dataview(s)", fid.ImportedDataviews.Count.ToString("###,##0")), true);
                    }
                }
            }
        }

        private void lvDataviews_DragOver(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent("FileDrop")) {

                var fid = new frmImportDataviews();

                // show them a list of all the dataview names, let them select a subset
                string[] files = e.Data.GetData("FileDrop") as string[];
                foreach (string f in files) {
                    if (f.ToLower().EndsWith(".dat") || f.ToLower().EndsWith(".dataview")) {
                        fid.ImportFiles.Add(f);
                    }
                }

                if (fid.ImportFiles.Count > 0) {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void lvDataviews_DragEnter(object sender, DragEventArgs e) {
            var obj = e.Data.GetData(typeof(File));
        }

        private void cmiDataviewExportList_Click(object sender, EventArgs e) {
            ExportListView(lvDataviews);
        }

        private void cmiDataviewShowDependencies_Click(object sender, EventArgs e) {
            if (lvDataviews.SelectedItems.Count == 1){

                var dvName = lvDataviews.SelectedItems[0].Tag.ToString();
                var dt = AdminProxy.GetDataViewDependencies(dvName).Tables["dataview_dependencies"];


                var msg = new frmMessageBox();
                msg.btnYes.Visible = false;
                msg.Text = "Dependencies for " + dvName;
                msg.btnNo.Text = "OK";

                if (dt.Rows.Count == 0) {
                    msg.txtMessage.Text = "(None)";
                } else {
                    var txt = "";
                    foreach (DataRow dr in dt.Rows) {
                        txt += dr["display_member"].ToString() + "\r\n";
                    }
                    msg.txtMessage.Text = txt;
                }

                msg.ShowDialog(this);
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmDataviews", resourceName, null, defaultValue, substitutes);
        }
    }
}
