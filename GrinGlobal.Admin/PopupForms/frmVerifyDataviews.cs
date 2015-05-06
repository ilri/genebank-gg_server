using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.InstallHelper;
using System.IO;
using GrinGlobal.Core;
using System.Diagnostics;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmVerifyDataviews : GrinGlobal.Admin.ChildForms.frmBase {
        public frmVerifyDataviews() {
            InitializeComponent();  tellBaseComponents(components);
            _dataviews = new List<string>();
        }

        private List<string> _dataviews;
        public List<string> Dataviews {
            get {
                return _dataviews;
            }
        }

        public override void RefreshData() {
            // nothing to do???
            var dgv = dgvDataviews;
            dgv.Rows.Clear();
            foreach (var dvName in _dataviews) {
                dgv.Rows.Add(dvName, "Unverified", "");
            }
        }

        public int VerifyDataviews(out int totalErrors, out int totalWarnings) {

            int totalSuccesses = 0;
            totalErrors = 0;
            totalWarnings = 0;
            using (new AutoCursor(this)) {
                var dgv = dgvDataviews;
                dgv.ShowCellToolTips = true;

                // show that we started
                foreach (DataGridViewRow dgvr in dgv.Rows) {
                    dgvr.Cells[1].Value = "Pending...";
                    dgvr.Cells[1].Style.ForeColor = Color.Black;
                }
                dgv.Refresh();
                Application.DoEvents();

                foreach (DataGridViewRow dgvr in dgv.Rows) {
                    dgvr.Cells[1].Value = "Testing...";
                    dgv.CurrentCell = dgvr.Cells[1];
                    dgv.Refresh();
                    Application.DoEvents();
                    if (_done) {
                        // they're asking to close the form...
                        return totalSuccesses;
                    }
                    var dvName = dgvr.Cells[0].Value as string;
                    var error = AdminProxy.VerifyDataview(dvName, chkUnescapeWhere.Checked);
                    if (String.IsNullOrEmpty(error)) {
                        totalSuccesses++;
                        dgvr.Cells[1].Value = "Success";
                        dgvr.Cells[2].Value = "";
                        dgvr.Cells[1].Style.ForeColor = Color.Blue;
                    } else if (error.StartsWith("WARNING:")){
                        
                        totalWarnings++;
                        totalSuccesses++;

                        dgvr.Cells[0].ToolTipText = error;
                        dgvr.Cells[1].Value = error;
                        dgvr.Cells[1].ToolTipText = error;
                        dgvr.Cells[1].Style.ForeColor = Color.Black;


                        dgvr.Cells[2].ToolTipText = error;
                        dgvr.Cells[2].Value = "Edit";
                    } else {
                        totalErrors++;
                        dgvr.Cells[0].ToolTipText = error;
                        dgvr.Cells[1].Value = error;
                        dgvr.Cells[1].ToolTipText = error;
                        dgvr.Cells[1].Style.ForeColor = Color.Red;

                        dgvr.Cells[2].ToolTipText = error;
                        dgvr.Cells[2].Value = "Edit";
                    }
                }
                dgv.Cursor = Cursors.Default;
            }
            return totalSuccesses;
        }


        private bool _activated;
        private void frmVerifyDataviews_Activated(object sender, EventArgs e) {
            if (!_activated) {
                _activated = true;

            }
        }

        private void btnVerify_Click(object sender, EventArgs e) {
            try {
                btnVerify.Enabled = false;
                btnExport.Enabled = false;
                chkUnescapeWhere.Enabled = false;
                _done = false;
                var errorCount = 0;
                var warningCount = 0;
                var successCount = VerifyDataviews(out errorCount, out warningCount);

                var txt = getDisplayMember("verify{results_body}", "Dataview Verification Results:\n\nSuccessful:  {0}\nFailures:  {1} \nWarnings:  {2}", successCount.ToString(), errorCount.ToString(), warningCount.ToString());

                if (errorCount == 0 && warningCount == 0) {
                    MessageBox.Show(this, txt, getDisplayMember("verify{results_title}", "All Successful"), MessageBoxButtons.OK, MessageBoxIcon.None);
                } else if (errorCount == 0 && warningCount > 0) {
                    MessageBox.Show(this, txt, getDisplayMember("verify{results_title}", "Some Warnings, No Failures"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else {
                    MessageBox.Show(this, txt, getDisplayMember("verify{results_title}", "Some Failed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } finally {
                btnVerify.Enabled = true;
                btnExport.Enabled = true;
                chkUnescapeWhere.Enabled = true;
            }
        }

        private void dgvDataviews_CellClick(object sender, DataGridViewCellEventArgs e) {

        }

        private ListSortDirection _prevSortDir = ListSortDirection.Ascending;
        private void dgvDataviews_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == 2) {
                if (e.RowIndex > -1) {
                    // is the link cell, edit it if needed
                    var dgvr = dgvDataviews.Rows[e.RowIndex];
                    if (dgvr.Cells[2].Value.ToString() == "Edit") {
                        var node = MainFormCurrentNode();
                        var tag = dgvr.Cells[0].Value.ToString();

                        var allDataviewsNode = MainFormGetAncestorTreeNode("ndDataviews");

                        MainFormSelectDescendentTreeNode(tag, allDataviewsNode);
                        Close();
                        return;

                        //if ((string.Empty + node.Name).ToLower() == "nddataviews") {
                        //    MainFormSelectDescendentTreeNode(tag);
                        //    Close();
                        //    return;
                        //} else {
                        //    MainFormSelectDescendentTreeNode(tag, MainFormGetUncleTreeNode 
                        //    MainFormSelectSiblingTreeNode(tag);
                        //    Close();
                        //    return;
                        //}
                    }

                    MessageBox.Show(this, getDisplayMember("dataviewCellContentClick{body}", "No tree node found that corresponds to the selected item."), 
                        getDisplayMember("dataviewCellContentClick{title}", "Invalid Action"));
                } else {
                    _prevSortDir = (_prevSortDir == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending);
                    dgvDataviews.Sort(dgvDataviews.Columns[e.ColumnIndex], _prevSortDir);
                }
            }
        }

        private bool _done = false;
        private void btnDone_Click(object sender, EventArgs e) {
            if (btnVerify.Enabled) {
                Close();
            } else {
                _done = true;
            }
        }

        private void btnExport_Click(object sender, EventArgs e) {
            var tempFolder = Utility.GetTempDirectory(5);
            var tempFile = Toolkit.ResolveFilePath(tempFolder + @"\verify_dataviews_export.txt", true);
            var names = new List<string>();
            var results = new List<string>();
            var maxNameLength = 0;
            foreach (DataGridViewRow dgvr in dgvDataviews.Rows) {
                var nm = dgvr.Cells[0].Value.ToString();
                if (nm.Length > maxNameLength){
                    maxNameLength = nm.Length;
                }
                names.Add(nm);
                results.Add(dgvr.Cells[1].Value.ToString());
            }
            var sb = new StringBuilder("Dataview Name".PadRight(maxNameLength));
            sb.Append("\t");
            sb.AppendLine("Result");
            sb.Append("=".PadRight(maxNameLength, '='));
            sb.Append("\t");
            sb.AppendLine("=".PadRight(maxNameLength, '='));
            for (var i = 0; i < names.Count; i++) {
                sb.Append(names[i].PadRight(maxNameLength));
                sb.Append("\t");
                sb.AppendLine(results[i].Replace("\r\n", "\r\n" + " ".PadRight(maxNameLength) + "\t").Replace("FAILED: ", ""));
            }
            File.WriteAllText(tempFile, sb.ToString());
            Process.Start(tempFile);
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmVerifyDataviews", resourceName, null, defaultValue, substitutes);
        }
    }
}
