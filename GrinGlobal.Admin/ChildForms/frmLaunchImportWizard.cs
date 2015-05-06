using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using GrinGlobal.InstallHelper;
using GrinGlobal.Core;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmLaunchImportWizard : GrinGlobal.Admin.ChildForms.frmBase {
        public frmLaunchImportWizard() {
            InitializeComponent();
        }

        public override void RefreshData() {
            launchWizard();
        }

        private void btnLaunchWizard_Click(object sender, EventArgs e) {
            launchWizard();
        }

        private void launchWizard(){
            //var path = Utility.GetGrinGlobalApplicationInstallPath("GRIN-Global Import Tool", null);
            var path = Utility.ResolveFilePath("./GrinGlobal.Import.exe", false);
            if (String.IsNullOrEmpty(path)) {
                var curPath = Utility.ResolveDirectoryPath("./", false);
                MessageBox.Show(this, getDisplayMember("launchWizard{body}", "The GRIN-Global Import Wizard could not be found.\nGrinGlobal.Import.exe must exist in the same folder as GrinGlobal.Admin.exe\n({0}).", curPath), 
                    getDisplayMember("launchWizard{title}", "Import Tool Not Found"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                //var args = "/import " + (rdoCodes.Checked ? "codes" : rdoTraits.Checked ? "traits" : "");
                var args = "/import " + AdminProxy.Connection.Serialize();



                //var obj = new Shell32.IShellWindows

                //var shell = new Shell32.ShellClass();



                //shell.ShellExecute(path, args, null, null, null);
                Debug.WriteLine(args);
                Process.Start(path, args);
            }

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmLaunchImportWizard", resourceName, null, defaultValue, substitutes);
        }
    }
}
