using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Admin.ChildForms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GrinGlobal.Core;
using System.Threading;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmImportDataviews : frmBase {
        public frmImportDataviews() {
            InitializeComponent();  tellBaseComponents(components);
            Sync(false, delegate() {
                chkSelectAll.Checked = true;
            });
            ImportFiles = new List<string>();
            ImportedDataviews = new List<string>();
        }

        private void frmTableMappingInspectSchema_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvDataviews);
            lblServerName.Text = AdminProxy.Connection.ServerName;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e) {
            ImportedDataviews = new List<string>();
            using (new AutoCursor(this)) {
                var dvNames = new List<string>();
                var dsList = new List<DataSet>();
                foreach (ListViewItem lvi in lvDataviews.Items) {
                    if (lvi.Checked) {
                        dvNames.Add(lvi.Text);
                        var ds = lvi.Tag as DataSet;
                        if (!dsList.Contains(ds)) {
                            dsList.Add(ds);
                        }
                    }
                }

                foreach (var ds in dsList) {

                    AdminProxy.UpconvertDataviewForImport(ds);

                    // only save the sql for the selected engines, ignore others by setting sql_statement to empty string
                    foreach (DataRow dr in ds.Tables["sys_dataview_sql"].Rows) {
                        var engine = dr["database_engine_tag"].ToString().Trim().ToLower();

                        switch (engine) {
                            case "sqlserver":
                                if (!chkSQLServer.Checked) {
                                    dr["sql_statement"] = "";
                                }
                                break;
                            case "mysql":
                                if (!chkMySQL.Checked) {
                                    dr["sql_statement"] = "";
                                }
                                break;
                            case "postgresql":
                                if (!chkPostgreSQL.Checked) {
                                    dr["sql_statement"] = "";
                                }
                                break;
                            case "oracle":
                                if (!chkOracle.Checked) {
                                    dr["sql_statement"] = "";
                                }
                                break;
                        }
                    }
                    ds.AcceptChanges();
                    bool errored = false;
                    Exception exception = null;
                    btnImport.Enabled = false;

                    var importLanguage = chkIncludeLanguage.Checked;
                    var useTableMappings = chkUseAsTableMappings.Checked && chkUseAsTableMappings.Enabled;
                    var importFieldsAndProperties = chkFieldsAndParameters.Checked;
                    if (!importFieldsAndProperties) {
                        importLanguage = false;
                        useTableMappings = false;
                    }

                    var done = false;
                    processInBackground(
                        (worker) => {
                            try {
                                worker.ReportProgress(0, getDisplayMember("importing{start}", "Importing..."));
                                var dsImported = AdminProxy.ImportDataViewDefinitions(ds, dvNames, false, importFieldsAndProperties, importLanguage, useTableMappings);
                                foreach(DataRow drImp in dsImported.Tables["imported"].Rows){
                                    ImportedDataviews.Add(drImp["dataview_name"].ToString());
                                }
                            } catch (Exception ex) {
                                exception = ex;
                                errored = true;
                            }
                        },
                        (BackgroundWorker worker2, ProgressChangedEventArgs e2) => {
                            this.Cursor = Cursors.WaitCursor;
                            statProgress.Value = e2.ProgressPercentage;
                            statLabel.Text = e2.UserState + "";
                            Refresh();
                        },
                        (worker3, e3) => {
                            try {
                                this.Cursor = Cursors.Default;
                                btnImport.Enabled = true;
                                if (!errored) {
                                    DialogResult = DialogResult.OK;
                                } else {
                                    if (exception.Message.Contains("sys_table")) {
                                        var res = MessageBox.Show(this, getDisplayMember("import{error_body}", "Error during import: {0}\n\nIgnoring this error will cause the field(s) with missing mappings to be read-only and you will need to fix the mappings manually.\n\nWould you like to ignore the missing mappings and continue?", exception.Message), getDisplayMember("import{error_title}", "Error Importing Dataview(s)"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        if (res == DialogResult.Yes) {
                                            try {
                                                var dsImported = AdminProxy.ImportDataViewDefinitions(ds, dvNames, true, importFieldsAndProperties, importLanguage, useTableMappings);
                                                foreach (DataRow drImp in dsImported.Tables["imported"].Rows) {
                                                    ImportedDataviews.Add(drImp["dataview_name"].ToString());
                                                }
                                                DialogResult = DialogResult.OK;
                                            } catch (Exception ex2ndTry) {
                                                MessageBox.Show(this, getDisplayMember("import{error2_body}", "Error during import: {0}", ex2ndTry.Message), getDisplayMember("import{error2_title}", "Error Importing Dataview(s)"));
                                            }
                                        }
                                    }
                                }
                            } finally {
                                done = true;
                                this.Close();
                            }
                        }
                    );

                    while (!done) {
                        Thread.Sleep(50);
                        Application.DoEvents();
                    }

                }
            }
        }

        public List<string> ImportFiles;
        public List<string> ImportedDataviews;

        public override void RefreshData() {

            Sync(true, delegate() {
                lvDataviews.Items.Clear();

                foreach (string file in ImportFiles) {
                    if (File.Exists(file)) {

                        DataSet ds = null;

                        var bin = new BinaryFormatter();
                        using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read)) {
                            ds = bin.Deserialize(fs) as DataSet;
                        }

                        AdminProxy.UpconvertDataviewForImport(ds);


                        foreach (DataRow dr in ds.Tables["sys_dataview"].Rows) {
                            var lvi = new ListViewItem(dr["dataview_name"].ToString());
                            lvi.Checked = chkSelectAll.Checked;

                            lvi.SubItems.Add(((DateTime)Toolkit.Coalesce(dr["modified_date"], dr["created_date"])).ToLocalTime().ToString());
                            if (dr.Table.Columns.Contains("category_code")) {
                                lvi.SubItems.Add(dr["category_code"].ToString());
                            } else {
                                lvi.SubItems.Add("");
                            }

                            if (dr.Table.Columns.Contains("database_area_code")) {
                                lvi.SubItems.Add(dr["database_area_code"].ToString());
                            } else {
                                lvi.SubItems.Add("");
                            }

                            var langRows = ds.Tables["sys_dataview_lang"].Select("sys_dataview_id = " + dr["sys_dataview_id"] + " and sys_lang_id = " + (AdminProxy == null ? 1 : AdminProxy.LanguageID));
                            if (langRows.Length > 0) {
                                if (langRows[0].Table.Columns.Contains("title")) {
                                    lvi.SubItems.Add(langRows[0]["title"].ToString());
                                } else {
                                    lvi.SubItems.Add("");
                                }
                                if (langRows[0].Table.Columns.Contains("description")) {
                                    lvi.SubItems.Add(langRows[0]["description"].ToString());
                                } else {
                                    lvi.SubItems.Add("");
                                }
                            }
                            lvi.Tag = ds;
                            lvDataviews.Items.Add(lvi);
                        }

                        //count++;
                        //proxy.ImportDataViewDefinitions(ds, dvNames);
                        //        }
                        //    }
                        //}

                    }
                }

                var allEngines = ("" + Toolkit.GetRegSetting(mdiParent.HIVE, "DataviewShowAllEngines", "false")).ToLower() == "true";
                chkOracle.Checked = chkOracle.Visible = allEngines;
                chkPostgreSQL.Checked = chkPostgreSQL.Visible = allEngines;
                chkSQLServer.Checked = chkSQLServer.Visible = allEngines;
                chkMySQL.Checked = chkMySQL.Visible = allEngines;

                var engine = AdminProxy.Connection.DatabaseEngineProviderName.ToLower();
                switch (engine) {
                    case "sqlserver":
                        chkSQLServer.Checked = chkSQLServer.Visible = true;
                        break;
                    case "mysql":
                        chkMySQL.Checked = chkMySQL.Visible = true;
                        break;
                    case "postgresql":
                        chkPostgreSQL.Checked = chkPostgreSQL.Visible = true;
                        break;
                    case "oracle":
                        chkOracle.Checked = chkOracle.Visible = true;
                        break;
                }
            });


        }

        private void chkShowAllTables_CheckedChanged(object sender, EventArgs e) {
            RefreshData();
        }

        private void chkIncludeLanguage_CheckedChanged(object sender, EventArgs e) {
            chkUseAsTableMappings.Enabled = chkIncludeLanguage.Checked;
        }

        private void lvDataviews_AfterLabelEdit(object sender, LabelEditEventArgs e) {
            if (e.Item > -1 && e.Item < lvDataviews.Items.Count){
                var lvi = lvDataviews.Items[e.Item];
                var oldName = lvi.Text;
                var newName = e.Label;

                if (oldName != newName) {
                    var ds = lvi.Tag as DataSet;
                    if (ds != null) {
                        changeValue(ds, "dataview_name", oldName, newName);
                        return;
                    }
                }
            }
            e.CancelEdit = true;
        }

        private void changeValue(DataSet ds, string columnName, string oldValue, string newValue) {
            foreach (DataTable dt in ds.Tables) {
                if (dt.Columns.Contains(columnName)) {
                    dt.Columns[columnName].ReadOnly = false;
                    foreach (DataRow dr in dt.Rows) {
                        if (dr[columnName].ToString() == oldValue) {
                            dr[columnName] = newValue;
                        }
                    }
                }
            }
        }

        private void frmImportDataviews_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
        }

        private void frmImportDataviews_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.F2) {
                if (lvDataviews.SelectedItems.Count >= 1) {
                    lvDataviews.SelectedItems[0].Focused = true;
                    lvDataviews.SelectedItems[0].BeginEdit();
                } else if (lvDataviews.Items.Count > 0) {
                    lvDataviews.Items[0].Focused = true;
                    lvDataviews.Items[0].BeginEdit();
                }
            }
        }


        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmImportDataviews", resourceName, null, defaultValue, substitutes);
        }

        private void chkFieldsAndParameters_CheckedChanged(object sender, EventArgs e) {
            gbLanguage.Enabled = chkFieldsAndParameters.Checked;
        }
    }
}
