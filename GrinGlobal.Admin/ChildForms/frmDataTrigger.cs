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
using System.IO;
using System.Reflection;
using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmDataTrigger : GrinGlobal.Admin.ChildForms.frmBase {
        public frmDataTrigger() {
            InitializeComponent();  tellBaseComponents(components);

        }

        private void frmTableMapping_Load(object sender, EventArgs e) {
        }

        private string _assemblyName;
        private string _assemblyPath;

        private string getVirtualFolder() {
            return "~/bin/";
            //return "~/uploads/datatriggers/";
        }

        private string _physicalFolder;
        private string getPhysicalFolder() {
            if (String.IsNullOrEmpty(_physicalFolder)) {
                var webAppRoot = AdminProxy.Connection.WebAppPhysicalPath; //Toolkit.GetIISPhysicalPath("gringlobal");
                if (Debugger.IsAttached) {
                    // HACK: for debugging, assume we're on localhost2600, meaning the IIS lookup to "Default Web Site" will be wrong.
                    if (!Environment.CurrentDirectory.ToLower().Contains(webAppRoot.ToLower())) {
                        webAppRoot = @"C:\projects\GrinGlobal\GrinGlobal.Web\";
                    }
                }
                var physPath = webAppRoot + getVirtualFolder().Replace("~/", @"\");
                _physicalFolder = Toolkit.ResolveDirectoryPath(physPath, false);
            }
            return _physicalFolder;
        }


        public override void RefreshData() {

            var val = MainFormCurrentNodeText("");
            if (val == "Data Triggers") {
                val = "New";
            }
            this.Text = "Data Trigger - " + val + " - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

            if (ddlAssembly.Items.Count == 0) {
                initAssemblyDropDown(ddlAssembly, getPhysicalFolder(), "< Pick One >", new Type[] { typeof(IDataviewReadDataTrigger), typeof(IDataviewSaveDataTrigger), typeof(ITableReadDataTrigger), typeof(ITableSaveDataTrigger) });
            }

            Sync(true, delegate() {
                initDataViewDropDown(ddlDataview, true, "(Any)", -1);
                initTableDropDown(ddlTable, true, "(Any)", -1);
                chkEnabled.Checked = true;

                if (ID > -1) {
                    var dt = AdminProxy.ListTriggers(ID).Tables["list_triggers"];
                    if (dt.Rows.Count > 0) {

                        var dr = dt.Rows[0];

                        var path = dr["virtual_file_path"].ToString().Replace("~/", getPhysicalFolder()).Replace("/", @"\").Replace(@"\\", @"\");
                        var assemblyName = Path.GetFileName(path);

                        ddlAssembly.SelectedIndex = ddlAssembly.FindString(assemblyName);
                        if (ddlAssembly.SelectedIndex < 0) {
                            ddlAssembly.SelectedIndex = 0;
                        }
                        ddlDataview.SelectedIndex = ddlDataview.FindString(dr["dataview_name"].ToString());
                        ddlTable.SelectedIndex = ddlTable.FindString(dr["table_name"].ToString());
                        chkEnabled.Checked = dr["is_enabled"].ToString().ToUpper() == "Y";
                        if (!inspectAssemblyFile()) {
                            MessageBox.Show(this, getDisplayMember("RefreshData{body}", "Could not locate the file at {0}.  Please verify it exists or upload a new version.", path ), 
                                getDisplayMember("RefreshData{title}", "File Not Found"));
                        }
                        ddlClass.SelectedIndex = ddlClass.FindStringExact(dr["fully_qualified_class_name"].ToString());
                        txtTitle.Text = dr["title"].ToString();
                        txtDescription.Text = dr["description"].ToString();

                    }
                }

                MarkClean();
            });

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
            //fpf.DataViewID = Toolkit.ToInt32(ddlDataView.SelectedValue, 0);
            //fpf.TableID = Toolkit.ToInt32(ddlTable.SelectedValue, 0);
            //if (DialogResult.OK == MainFormPopupForm(fpf, false)) {
            //    RefreshData();
            //}
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            //if (lvTableFields.SelectedItems.Count == 1) {
            //    showProperties(Toolkit.ToInt32(lvTableFields.SelectedItems[0].Tag, 0));
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

        private void btnCancel_Click_1(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            if (Modal) {
                MainFormRefreshData();
            } else {
                MainFormSelectParentTreeNode();
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e) {
            if (ddlAssembly.SelectedIndex < 0 || ddlAssembly.Text.Contains("<") || ddlClass.SelectedIndex < 0) {
                MessageBox.Show(this, getDisplayMember("save{asmreq_body}", "Both Assembly and Class must be specified."), 
                    getDisplayMember("save{asmreq_title}", "No Class Specified"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                ddlClass.Focus();
                return;
            }

            var vPath = getVirtualFolder() + ddlAssembly.Text;
            ID = AdminProxy.SaveTrigger(ID,
                Toolkit.ToInt32(ddlDataview.SelectedValue, -1),
                Toolkit.ToInt32(ddlTable.SelectedValue, -1),
                vPath,
                _assemblyName,
                ddlClass.Text,
                txtTitle.Text,
                txtDescription.Text,
                chkEnabled.Checked,
                false);

            if (Modal) {
                MainFormRefreshData();
                DialogResult = DialogResult.OK;
                Close();
            } else {
                MainFormSelectParentTreeNode();
            }
        }

        private void btnFile_Click(object sender, EventArgs e) {
//            openFileDialog1.FileName = txtFilePath.Text;
            ofdTriggerDetail.Multiselect = false;
            ofdTriggerDetail.Title = "Select .NET Assembly Containing the GRIN-Global Trigger Class";
            ofdTriggerDetail.CheckFileExists = true;
            ofdTriggerDetail.CheckPathExists = true;
            ofdTriggerDetail.Filter = ".NET Assembly files (*.dll)|*.dll|All Files (*.*)|*.*";
            if (DialogResult.OK == ofdTriggerDetail.ShowDialog(this)) {


                // file is not in the triggers path the website is expecting it to be.
                // copy it there now.
                var fname = Path.GetFileName(ofdTriggerDetail.FileName);
                if (!ofdTriggerDetail.FileName.ToLower().Contains(getPhysicalFolder().ToLower())){
                    var newPath = Toolkit.ResolveFilePath(getPhysicalFolder() + @"\" + fname, true);
                    //if (File.Exists(newPath)) {
                    //    // TODO: prompt before deletion of file web app is supposed to have control over??
                    //    try {
                    //        File.Delete(newPath);
                    //    } catch {
                    //    }
                    //}
                    try {
                        File.Copy(ofdTriggerDetail.FileName, newPath, true);
                    } catch {
                    }
                }

                if (!ddlAssembly.Items.Contains(fname)) {
                    ddlAssembly.Items.Add(fname);
                }
                ddlAssembly.SelectedIndex = ddlAssembly.FindStringExact(fname);
                inspectAssemblyFile();

            }
        }


        /// <summary>
        /// This inspects the file at txtFilePath.Text and sets the lblAssemblyName appropriately and repopulates the ddlClass dropdown.
        /// </summary>
        private bool inspectAssemblyFile() {
            _assemblyPath = (getPhysicalFolder() + @"\" + ddlAssembly.Text).Replace(@"\\", @"\");
            var previousClass = ddlClass.Text;

            ddlClass.Items.Clear();

            if (!File.Exists(_assemblyPath)) {
                // not a valid file specified, just bomb out of the remaining processing
                return false;
            }


            var fileName = new FileInfo(_assemblyPath).Name;

            // newpath now contains the final resting place for the file.
            // reflect on it, see what assembly(ies) and valid class(es) it contains

            var asm = Assembly.LoadFrom(_assemblyPath);
            _assemblyName = asm.FullName.Split(',')[0];


            var exportedTypes = asm.GetExportedTypes();
            Type[] interfaces = null;
            if (ddlDataview.Text != "(Any)") {
                interfaces = new Type[] { typeof(GrinGlobal.Interface.DataTriggers.IDataviewSaveDataTrigger), typeof(GrinGlobal.Interface.DataTriggers.IDataviewReadDataTrigger) };
            } else if (ddlTable.Text != "(Any)") {
                interfaces = new Type[] { typeof(GrinGlobal.Interface.DataTriggers.ITableSaveDataTrigger), typeof(GrinGlobal.Interface.DataTriggers.ITableReadDataTrigger) };
            } else {
                interfaces = new Type[] {};
            }

            foreach (var t in exportedTypes) {
                foreach(var iface in interfaces){
                    if (ContainsInterface(t, iface)) {
                        if (ddlClass.FindStringExact(t.FullName) == -1) {
                            ddlClass.Items.Add(t.FullName);
                        }
                    }
                }
            }

            ddlClass.SelectedIndex = ddlClass.FindStringExact(previousClass);

            return true;
        }

        private void inspectClass() {
            var asm = Assembly.LoadFrom(_assemblyPath);
            var exportedTypes = asm.GetExportedTypes();

            txtTitle.Text = "-";
            txtDescription.Text = "-";

            var tgtClass = ddlClass.SelectedItem as string;
            if (!String.IsNullOrEmpty(tgtClass)) {
                foreach (var t in exportedTypes) {
                    if (t.FullName == tgtClass) {
                        try {
                            var o = Activator.CreateInstance(t) as IDataTriggerDescription;
                            txtTitle.Text = o.GetTitle("en-US");
                            txtDescription.Text = o.GetDescription("en-US");
                        } catch (Exception) {
                            // ignore any loading issues
                        }
                    }
                }
            }
        }

        private bool ContainsInterface(Type type, Type iface) {
            var typeInterfaces = type.GetInterfaces();
            foreach (var ti in typeInterfaces) {
                if (ti == iface) {
                    return true;
                }
            }
            var subtypes = type.GetNestedTypes();
            foreach (var st in subtypes) {
                if (ContainsInterface(st, iface)) {
                    return true;
                }
            }
            return false;

        }

        private void ddlTable_SelectedIndexChanged(object sender, EventArgs e) {
            Sync(false, delegate() {
                if (ddlTable.Text != "(Any)") {
                    // change dataview dropdown to none (can apply to exactly one or the other, not both)
                    if (ddlDataview.Items.Count > 0) {
                        ddlDataview.SelectedIndex = 0;
                        inspectAssemblyFile();
                    }
                }
                CheckDirty();
            });
        }

        private void ddlDataview_SelectedIndexChanged(object sender, EventArgs e) {
            Sync(false, delegate() {
                if (ddlDataview.Text != "(Any)") {
                    // change dataview dropdown to none (can apply to exactly one or the other, not both)
                    if (ddlTable.Items.Count > 0) {
                        ddlTable.SelectedIndex = 0;
                        inspectAssemblyFile();
                    }
                }
                CheckDirty();
            });
        }

        private void ddlAssembly_SelectedIndexChanged(object sender, EventArgs e) {
            Sync(false, delegate() {
                inspectAssemblyFile();
                CheckDirty();
            });

        }

        private void ddlClass_SelectedIndexChanged(object sender, EventArgs e) {
            Sync(false, delegate() {
                inspectClass();
                CheckDirty();
            });

        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmDataTrigger", resourceName, null, defaultValue, substitutes);
        }
    }
}
