using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.InstallHelper;
using System.IO;
using System.Diagnostics;
using GrinGlobal.Core;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmFile : GrinGlobal.Admin.ChildForms.frmBase {
        public frmFile() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public override void RefreshData() {

            this.Text = "File - New - " + AdminProxy.Connection.DatabaseEngineServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
            if (ID > -1) {

                var ds = AdminProxy.ListFiles(ID);
                var dt = ds.Tables["list_files"];
                if (dt != null) {
                    if (dt.Rows.Count > 0) {
                        var dr = dt.Rows[0];
                        var ggWebPath = AdminProxy.Connection.WebAppPhysicalPath; //Utility.GetIISPhysicalPath("gringlobal");

                        txtVirtualPath.Text = dr["virtual_file_path"].ToString();
                        txtPhysicalPath.Text = txtVirtualPath.Text.Replace("~/", ggWebPath).Replace("//", "/").Replace("/", @"\");
                        txtFileName.Text = dr["file_name"].ToString();
                        txtDisplayName.Text = dr["display_name"].ToString();
                        txtFileSize.Text = dr["file_size"].ToString();
                        txtFileVersion.Text = dr["file_version"].ToString();
                        chkAvailable.Checked = dr["is_enabled"].ToString().Trim().ToUpper() == "Y";

                        this.Text = "File - " + txtDisplayName.Text + " - " + AdminProxy.Connection.DatabaseEngineServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");


                    }
                }
                MarkClean();
            }

            if (ID == -1){
                txtVirtualPath.Text = "~/uploads/";
                txtPhysicalPath.Text = "";
                txtFileName.Text = "";
                txtDisplayName.Text = "";
                txtFileSize.Text = "";
                txtFileVersion.Text = "";
                MarkDirty();
            }

        }

        private void btnDone_Click(object sender, EventArgs e) {

            // tell database about the file, copy it to the proper directory for IIS
            ID = AdminProxy.SaveFile(ID, txtDisplayName.Text, txtFileName.Text, txtVirtualPath.Text, txtPhysicalPath.Text, chkAvailable.Checked);

            DialogResult = DialogResult.OK;
            if (!Modal) {
                MainFormRefreshData();
            }
            Close();
            
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOpenFile_Click(object sender, EventArgs e) {
            ofdPhysicalPath.Filter = "All Files (*.*)|*.*";
            var result = ofdPhysicalPath.ShowDialog(this);
            if (result == DialogResult.OK) {

                txtPhysicalPath.Text = ofdPhysicalPath.FileName;
                if (txtPhysicalPath.Text.ToLower().EndsWith(".msi")) {
                    txtFileVersion.Text = Utility.GetProductVersion(txtPhysicalPath.Text);
                } else {
                    var fvi = FileVersionInfo.GetVersionInfo(txtPhysicalPath.Text);
                    txtFileVersion.Text = fvi.FileVersion;
                }

                var fi = new FileInfo(txtPhysicalPath.Text);
                txtFileSize.Text = ((decimal)fi.Length / 1024.0M / 1024.0M).ToString("###,###,##0.00");

                txtFileName.Text = Path.GetFileName(ofdPhysicalPath.FileName);
                txtVirtualPath.Text = "~/uploads/" + txtFileName.Text;

                if (txtDisplayName.Text == "") {
                    txtDisplayName.Text = Path.GetFileNameWithoutExtension(txtPhysicalPath.Text);
                }
                chkAvailable.Checked = true;

            }
        }

        private void frmFile_Load(object sender, EventArgs e) {

        }

        private void txtPhysicalPath_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtDisplayName_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtFileName_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtVirtualPath_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void chkAvailable_CheckedChanged(object sender, EventArgs e) {
            CheckDirty();

        }
    }
}
