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

namespace GrinGlobal.Search.Engine.Service {
    public partial class frmInstallIndexes : Form {
        public frmInstallIndexes() {
            InitializeComponent();
        }

        private int _installerWindowHandle;

        public DialogResult ShowDialog(int installerWindowHandle) {
            _installerWindowHandle = installerWindowHandle;
            return ShowDialog();
        }

        public string HelperPath;

        private void frmInstallData_Load(object sender, EventArgs e) {
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
            string tag = e.Node.Tag == null ? "" : e.Node.Tag.ToString().ToLower();
            switch (tag) {
                case "taxonomy search":
                    lblDescription.Text = "Index for Taxonomy data (roughly 16 MB)";
                    break;
                case "accession search":
                    lblDescription.Text = "Index for Accession data (roughly 20 MB)";
                    break;
                case "inventory search":
                    lblDescription.Text = "Index for Inventory data (roughly 30 MB)";
                    break;
                case "observation search":
                    lblDescription.Text = "Index for Observation data (roughly 108 MB)";
                    break;
                case "cooperator search":
                    lblDescription.Text = "Index for Cooperator data (roughly 5 MB)";
                    break;
                default:
                    lblDescription.Text = "Select an item on the left for more information.";
                    break;
            }
        }

        public List<string> IndexFiles;

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
            string args = @"/conn last /download ""GRIN-Global Index Files"" """ + String.Join(@""" """, fileNames) + @"""";

            var output = Utility.Run((updaterPath + @"\GrinGlobal.Updater.exe").Replace(@"\\", @"\"), args, false, true);


            if (!output.StartsWith("FILES:")) {
                // failure
                throw new InvalidOperationException(getDisplayMember("performDownloads{nofiles}", "Error downloading index files: {0}", output.Replace("ERROR:", "")));
            } else {
                // success
                var paths = output.Trim().Split('|');
                if (paths.Length < 2) {
                    // no valid file paths given.
                    if (paths.Length == 1) {
                        throw new InvalidOperationException(getDisplayMember("performDownloads{nopath}", "Error downloading index file:\n{0}", paths[0]));
                    } else {
                        throw new InvalidOperationException(getDisplayMember("performDownloads{unknown}", "Unknown error occurred when downloading index file"));
                    }
                } else {
                    // fill list of datafiles with the local path to the files we downloaded.
                    IndexFiles = new List<string>();
                    for (var i = 1; i < paths.Length; i++) {
                        var newPath = paths[i].Trim();
                        // MessageBox.Show("Filename: '" + newPath + "'");
                        IndexFiles.Add(newPath);
                    }

                    return IndexFiles;
                }
            }
        }

 
        private void btnContinue_Click(object sender, EventArgs e) {

            if (rdoCreateLocally.Checked) {

                // mark index files as empty so code that loaded this form knows it should create indexes locally as there are none to download.
                IndexFiles = null;

                DialogResult = DialogResult.OK;
                Close();

            } else {

                var updaterPath = "<not filled yet>";
                try {
                    this.Cursor = Cursors.WaitCursor;

                    this.Enabled = false;

                    // get location of GrinGlobal.Updater.exe
                    updaterPath = Utility.GetGrinGlobalApplicationInstallPath("Updater", @"<registry key not found>");

                    // remember files to download
                    var filesToDownload = determineFilesToDownload(false);

                    if (filesToDownload.Count == 0) {

//                        MessageBox.Show("You must either select at least one index file to download or choose to create the indexes locally.");
                        if (DialogResult.OK == MessageBox.Show(this, 
                                getDisplayMember("Continue{nofiles}", "If no pre-built indexes are downloaded, you must create them yourself by going to Admin | Utilities | Search Engine page on the local web site.\nDo you want to continue anyway?"), 
                                getDisplayMember("Continue{nofilestitle}", "No Search Indexes Selected"), 
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)){

                            DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        return;

                    } else {

                        performDownloads(updaterPath, filesToDownload.ToArray());

                        DialogResult = DialogResult.OK;
                        Close();
                    }


                } catch (Exception ex) {
                    MessageBox.Show(ex.Message + "\nPath to Updater:" + updaterPath);
                    EventLog.WriteEntry("GRIN-Global Search Engine", "Error downloading or installing indexes: " + ex.Message + ".  Path to Updater: " + updaterPath, EventLogEntryType.Error);
                } finally {
                    this.Cursor = Cursors.Default;
                    this.Enabled = true;
                }
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

        private List<string> determineFilesToDownload(bool force) {
            var sel = new List<string>();
            foreach (TreeNode nd in getAllNodes(treeView1.Nodes)) {
                if (nd.Checked || force) {
                    sel.Add((nd.Tag ?? "").ToString());
                }
            }
            return sel;
        }

        private void rdoCreateLocally_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void rdoDownloadFromServer_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void syncGUI() {
            lblDescription.Enabled = treeView1.Enabled = rdoDownloadFromServer.Checked;
        }

        private bool _activated;
        private void frmInstallIndexes_Activated(object sender, EventArgs e) {
            if (!_activated) {
                _activated = true;
                Toolkit.ActivateApplication(this.Handle);
                this.Focus();
                if (_installerWindowHandle > 0) {
                    Toolkit.MinimizeWindow(_installerWindowHandle);
                }
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "frmInstallIndexes", resourceName, null, defaultValue, substitutes);
        }

    }
}
