using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using GrinGlobal.InstallHelper;
using Microsoft.Win32;
using GrinGlobal.Core;

namespace GrinGlobal.DatabaseCopier {
    public partial class frmInstallData : Form {
        public frmInstallData() {
            InitializeComponent();
        }

        private void frmInstallData_Load(object sender, EventArgs e) {
            DataFiles = new List<string>();
            treeView1.ExpandAll();
            treeView1.SelectedNode = treeView1.Nodes[0];
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e) {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e) {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e) {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {
            string tag = e.Node.Tag == null ? "" : e.Node.Tag.ToString();
            switch (tag) {
                case "Taxonomy Data":
                    lblDescription.Text = "Taxonomy data.  Roughly 7 MB.";
                    break;
                case "Accession Data":
                    lblDescription.Text = "Example Accession and Inventory data.  Requires Taxonomy data.  Roughly 13 MB.";
                    break;
                case "Order Data":
                    lblDescription.Text = "Example Order data.  Requires Accession / Inventory and Taxonomy data.  Roughly 12 MB.";
                    break;
                case "Observation Data":
                    lblDescription.Text = "Example Observation and Evaluation data.  Requires Accession / Inventory and Taxonomy data.  Roughly 6 MB.";
                    break;
                default:
                    lblDescription.Text = "Select an item on the left for more information.";
                    break;
            }
        }

        public List<string> DataFiles;

        private IEnumerable<TreeNode> getAllNodes(TreeNodeCollection col) {
            foreach (TreeNode node in col) {
                yield return node;
                foreach (TreeNode child in getAllNodes(node.Nodes)) {
                    yield return child;
                }
            }
        }

        /// <summary>
        /// Downloads the files and fills the DataFiles property (which is also the return value of the method)
        /// </summary>
        public List<string> DownloadAllFiles() {
            var filesToDownload = determineFilesToDownload(true);
            // get location of GrinGlobal.Updater.exe
            var updaterPath = Utility.GetGrinGlobalApplicationInstallPath("Updater", @"<registry key not found>");
            return performDownloads(updaterPath, filesToDownload.ToArray());
        }

        /// <summary>
        /// Downloads files and adds the local path to each one to the DataFiles property.  Throws exception if download fails for any reason
        /// </summary>
        /// <param name="updaterPath"></param>
        /// <param name="fileNames"></param>
        private List<string> performDownloads(string updaterPath, string[] fileNames) {
            string args = @"/conn last /download ""GRIN-Global Data Files"" """ + String.Join(@""" """, fileNames) + @"""";
            // MessageBox.Show(args);

            var output = Utility.Run((updaterPath + @"\GrinGlobal.Updater.exe").Replace(@"\\", @"\"), args, false, true);
            if (!output.StartsWith("FILES:")) {
                // failure
                throw new InvalidOperationException(getDisplayMember("performDownloads{download}", "Error downloading data files: {0}", output.Replace("ERROR:", "")));
            } else {
                // success
                var paths = output.Trim().Split('|');
                if (paths.Length < 2) {
                    // no valid file paths given.
                    if (paths.Length == 1) {
                        throw new InvalidOperationException(getDisplayMember("performDownloads{paths}", "Error downloading data:\n{0}", paths[0]));
                    } else {
                        throw new InvalidOperationException(getDisplayMember("performDownlaods{unknown}", "Unknown error occurred when downloading data"));
                    }
                } else {
                    // fill list of datafiles with the local path to the files we downloaded.
                    DataFiles = new List<string>();
                    for (var i = 1; i < paths.Length; i++) {
                        var newPath = paths[i].Trim();
                        // MessageBox.Show("Filename: '" + newPath + "'");
                        DataFiles.Add(newPath);
                    }
                    return DataFiles;
                }
            }

        }

        private void btnContinue_Click(object sender, EventArgs e) {
            var updaterPath = "<not filled yet>";
            try {

                this.Cursor = Cursors.WaitCursor;
                this.Enabled = false;

                // get location of GrinGlobal.Updater.exe
                updaterPath = Utility.GetGrinGlobalApplicationInstallPath("Updater", @"<registry key not found>");

                // remember files to download
                var filesToDownload = determineFilesToDownload(false);

                if (filesToDownload.Count == 0) {
                    // MessageBox.Show("No files to download, continuing...");
                    DialogResult = DialogResult.OK;
                    Close();
                } else {

                    performDownloads(updaterPath, filesToDownload.ToArray());

                    DialogResult = DialogResult.OK;
                    Close();

                }


            } catch (Exception ex) {
                MessageBox.Show(getDisplayMember("Continue{failed}", "{0}\nPath to Updater: {1}", ex.Message, updaterPath));
            } finally {
                this.Cursor = Cursors.Default;
                this.Enabled = true;
            }

        }

        private void recurseToggleChildren(TreeNode parent, bool chk) {
            foreach (TreeNode child in parent.Nodes) {
                child.Checked = chk;
                recurseToggleChildren(child, chk);
            }
        }

        bool _syncing;
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e) {
            if (!_syncing) {
                try {
                    _syncing = true;
                    if (!e.Node.Checked) {
                        // auto-uncheck all children as their parent isn't checked
                        recurseToggleChildren(e.Node, e.Node.Checked);
                    } else {
                        // auto-check all ancestors as they have a descendent checked
                        var nd = e.Node;
                        while (nd.Parent != null) {
                            nd.Parent.Checked = nd.Checked;
                            nd = nd.Parent;
                        }
                    }
                } finally {
                    _syncing = false;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmInstallData_FormClosing(object sender, FormClosingEventArgs e) {
        }

        private List<string> determineFilesToDownload(bool forceDownload) {
            var sel = new List<string>();
            foreach (TreeNode nd in getAllNodes(treeView1.Nodes)) {
                if (nd.Checked || forceDownload) {
                    sel.Add((nd.Tag ?? "").ToString());
                }
            }
            return sel;
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseCopier", "frmInstallData", resourceName, null, defaultValue, substitutes);
        }
    }
}
