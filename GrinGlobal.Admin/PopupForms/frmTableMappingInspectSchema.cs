using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Admin.ChildForms;
using GrinGlobal.Core;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmTableMappingInspectSchema : frmBase {
        public frmTableMappingInspectSchema() {
            InitializeComponent();  tellBaseComponents(components);
            TableNames = new List<string>();
        }

        public List<string> TableNames;

        private void frmTableMappingInspectSchema_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvMissingTableMappings);
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnGenerateDefaults_Click(object sender, EventArgs e) {
            using (new AutoCursor(this)) {
                var tablesToRecreate = new List<string>();
                foreach (ListViewItem lvi in lvMissingTableMappings.CheckedItems) {
                    tablesToRecreate.Add(lvi.Tag as string);
                }
                AdminProxy.RecreateTableMappings(tablesToRecreate);
                DialogResult = DialogResult.OK;
                this.Close();
                AdminProxy.ClearCache(null);
                MainFormRefreshData();
                if (tablesToRecreate.Count == 1) {
                    MainFormUpdateStatus(getDisplayMember("rebuilt{one}", "Rebuilt mappings for {0}", tablesToRecreate[0]), true);
                } else {
                    MainFormUpdateStatus(getDisplayMember("rebuilt{many}", "Rebuilt mappings for {0} tables", tablesToRecreate.Count.ToString("###,##0")), true);
                }
            }
        }

        public override void RefreshData() {
            lblStatus.Text = "Inspecting database schema for unmapped tables...";
            Application.DoEvents();

            var mapped = AdminProxy.ListTables(ID).Tables["list_tables"];
            var unmapped = AdminProxy.ListUnmappedTables().Tables["list_unmappedtables"];

            lvMissingTableMappings.Items.Clear();

            if (TableNames.Count > 0) {
                foreach (string tblName in TableNames) {
                    var lvi = new ListViewItem(tblName, 0);
                    lvi.Checked = true;
                    lvi.Tag = tblName;
                    lvMissingTableMappings.Items.Add(lvi);
                }
            } else {
                foreach (DataRow dr2 in unmapped.Rows) {
                    var tbl = dr2["table_name"].ToString();
                    var lvi = new ListViewItem(tbl, 0);
                    lvi.Checked = !lvi.Text.EndsWith("_temp") || TableNames.Contains(tbl);
                    lvi.Tag = tbl;
                    lvMissingTableMappings.Items.Add(lvi);
                }

                if (chkShowAllTables.Checked) {
                    foreach (DataRow dr in mapped.Rows) {
                        var lvi = new ListViewItem(dr["table_name"].ToString());
                        lvi.Checked = false;
                        lvi.Tag = dr["table_name"].ToString();
                        lvMissingTableMappings.Items.Add(lvi);
                    }
                }
            }

            if (lvMissingTableMappings.Items.Count == 0) {
                lblStatus.Text = "All tables in the database schema are already mapped.";
                btnGenerateDefaults.Enabled = false;
            } else {
                if (TableNames.Count > 0) {
                    lblStatus.Text = "Rebuild mappings for table(s)...";
                } else {
                    if (chkShowAllTables.Checked) {
                        lblStatus.Text = "Listing all tables in the database schema.";
                    } else {
                        lblStatus.Text = "The following tables are not currently mapped.";
                    }
                }
                btnGenerateDefaults.Enabled = true;
            }
            
        }

        private void chkShowAllTables_CheckedChanged(object sender, EventArgs e) {
            ID = -1;
            RefreshData();
        }

        private bool _activated;
        private void frmTableMappingInspectSchema_Activated(object sender, EventArgs e) {
            if (!_activated) {
                _activated = true;
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e) {
            foreach (ListViewItem lvi in lvMissingTableMappings.Items) {
                lvi.Checked = chkSelectAll.Checked;
            }
        }


        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmTableMappingInspectSchema", resourceName, null, defaultValue, substitutes);
        }
    }
}
