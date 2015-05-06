using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using GrinGlobal.InstallHelper;
using System.Diagnostics;


namespace GrinGlobal.Updater {
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer {
        public ProjectInstaller() {
            InitializeComponent();
        }

        private void ProjectInstaller_AfterInstall(object sender, InstallEventArgs e) {
            string targetDir = Utility.GetTargetDirectory(this.Context, e.SavedState, "Updater");

            var gguacPath = Utility.ResolveFilePath(targetDir + @"\gguac.exe", false);
            var utility64CabPath = Utility.ResolveFilePath(targetDir + @"\utility64.cab", false);
            var utility64ExePath = Utility.ResolveFilePath(targetDir + @"\ggutil64.exe", false);

            var tempPath = Utility.GetTempDirectory(100);

            if (!File.Exists(utility64ExePath)) {
                if (File.Exists(utility64CabPath)) {
                    // wipe out any existing utility64.exe file in the temp folder
                    var extracted = Utility.ResolveFilePath(tempPath + @"\ggutil64.exe", false);
                    if (File.Exists(extracted)) {
                        File.Delete(extracted);
                    }
                    var extracted2 = Utility.ResolveFilePath(tempPath + @"\ggutil64.pdb", false);
                    if (File.Exists(extracted2)) {
                        File.Delete(extracted2);
                    }
                    // extract it from our cab
                    var cabOutput = Utility.ExtractCabFile(utility64CabPath, tempPath, gguacPath);
                    // move it to the final target path (we can't do this up front because expand.exe tells us "can't expand cab file over itself" for some reason.
                    if (File.Exists(extracted)) {
                        File.Move(extracted, utility64ExePath);
                    }
                }
            }

        }

        private void ProjectInstaller_BeforeUninstall(object sender, InstallEventArgs e) {

            try {
                try {
                    // wipe out the "last url" so it is recalculated if they reinstall from a different location later
                    // 2010-05-07 Brock Weaver brock@circaware.com
                    // removed this so Updater doesn't seem 'forgetful' of where it last connected.
                    // Utility.SetRegSetting("LastURL", "", false);
                } catch {
                }

                var targetDir = Utility.GetTargetDirectory(this.Context, e.SavedState, "Updater");
                var utility64ExePath = Utility.ResolveFilePath(targetDir + @"\ggutil64.exe", false);

                try {
                    if (File.Exists(utility64ExePath)) {
                        File.Delete(utility64ExePath);
                    }
                } catch (Exception ex) {
                    //Context.LogMessage("Could not delete " + utility64ExePath + ": " + ex.Message);
                    EventLog.WriteEntry("GRIN-Global Updater", "Could not delete " + utility64ExePath + ": " + ex.Message, EventLogEntryType.Error);
                }
            } catch {
            }

        }

    }
}
