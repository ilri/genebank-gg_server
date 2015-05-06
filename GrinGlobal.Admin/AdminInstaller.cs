using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using GrinGlobal.InstallHelper;
using GrinGlobal.Core;
using System.IO;


namespace GrinGlobal.Admin {
    [RunInstaller(true)]
    public partial class AdminInstaller : Installer {
        public AdminInstaller() {
            InitializeComponent();
        }

        private void AdminInstaller_AfterInstall(object sender, InstallEventArgs e) {

            string targetDir = Utility.GetTargetDirectory(this.Context, null, "Admin");
            string gguacPath = Toolkit.ResolveFilePath(targetDir + @"\gguac.exe", false);
            Utility.ExtractUtility64File(targetDir, gguacPath);

        }

        private void AdminInstaller_BeforeUninstall(object sender, InstallEventArgs e) {
            try {
                string targetDir = Utility.GetTargetDirectory(this.Context, e.SavedState, "Admin");
                var utility64exe = Toolkit.ResolveFilePath(targetDir + @"\ggutil64.exe", false);
                if (File.Exists(utility64exe)) {
                    File.Delete(utility64exe);
                }
            } catch {
                // eat any errors
            }

        }
    }
}
