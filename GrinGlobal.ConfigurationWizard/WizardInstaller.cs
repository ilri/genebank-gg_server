using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Text;
using Microsoft.Win32;
using GrinGlobal.Core;
using GrinGlobal.InstallHelper;


namespace GrinGlobal.ConfigurationWizard {
    [RunInstaller(true)]
    public partial class WizardInstaller : Installer {

        public WizardInstaller() {
            InitializeComponent();
        }






        public void showState(IDictionary state) {
            StringBuilder sb = new StringBuilder("Context.Parameters:\n");
            foreach (string key in Context.Parameters.Keys) {
                sb.Append(key).Append("=").AppendLine(Context.Parameters[key]);
            }
            if (state != null) {
                sb.AppendLine("\nSavedState:");
                foreach (string key in state.Keys) {
                    sb.Append(key).Append("=").AppendLine(state[key].ToString());
                }
            }

            MessageBox.Show(sb.ToString());
        }


        void copyDirectory(string srcPath, string destPath, bool recurse) {
            if (!Directory.Exists(destPath)) {
//                MessageBox.Show("Creating path '" + destPath + "'");
                Directory.CreateDirectory(destPath);
            }
            foreach (string f in Directory.GetFiles(srcPath)) {
                string newFile = destPath + @"\" + new FileInfo(f).Name;
//                MessageBox.Show("Copying to file '" + newFile + "'");
                File.Copy(f, newFile, true);
            }
            if (recurse) {
                foreach (string d in Directory.GetDirectories(srcPath)) {
                    string newPath = destPath + @"\" + new DirectoryInfo(d).Name;
//                    MessageBox.Show("Copying to directory '" + newPath + "'");
                    copyDirectory(d, newPath, true);
                }
            }
        }

        void WizardInstaller_AfterInstall(object sender, InstallEventArgs e) {

  //          showState(e.SavedState);

            // copy all the binaries to the target dir so that later
            // when they re-run the configuration wizard we don't require the CD

            try {


                string sourceFolder = Utility.GetSourceDirectory(this.Context, e.SavedState);

//                MessageBox.Show("sourcedir=" + sourceFolder);


                string target = Utility.GetTargetDirectory(this.Context, e.SavedState, "ConfigurationWizard");
                string targetFolder = Directory.GetParent(target).FullName;

//                MessageBox.Show("targetdir=" + targetFolder);

                string engine =  Utility.GetDatabaseEngine(this.Context, e.SavedState, false);


                // remember source directory
                Utility.SetSourceDirectory(sourceFolder);


                //string installerCacheFolder = (targetFolder + @"\installers").Replace(@"\\", @"\");

                //// blow away the installer cache if it already exists
                //if (Directory.Exists(installerCacheFolder)) {
                //    Directory.Delete(installerCacheFolder, true);
                //}

                //// create the folder for the installer cache
                //Directory.CreateDirectory(installerCacheFolder);

                //bool runFromCD = Context.Parameters["runfromcd"] == "1";
                //if (!runFromCD) {
                //    copyDirectory((sourceFolder + @"\bin").Replace(@"\\", @"\"), installerCacheFolder, true);
                //}

                string text = @"
cd /d ""__TARGETDIR__""
gguac.exe /hide GrinGlobal.ConfigurationWizard.exe
";
                text = text.Replace("__TARGETDIR__", targetFolder).Replace(@"\\", @"\");

                File.WriteAllText((targetFolder + @"\firstrun.bat").Replace(@"\\", @"\"), text);

//                launchWizard(targetFolder);
            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
                this.Context.LogMessage("Exception in AfterInstall: " + ex.Message);
            }

        }


        void launchWizard(string targetFolder){

//            MessageBox.Show("Installer committed.  Getting pid for installer....");

            try {

                // launch the wizard exe as a new process, passing the PID of the current process.
                // the wizard exe is coded to wait for the pid to finish executing, if given a pid.
                // we do this because one MSI cannot launch another MSI.  Since the wizard exe launches 
                // the individual MSI's for each app/component, we need to wait until this one completes.
                int pid = Process.GetCurrentProcess().Id;

//                string args = "/pid " + pid;

                string targetFileName = (targetFolder + @"\GrinGlobal.ConfigurationWizard.exe").Replace(@"\\", @"\");

//                MessageBox.Show("Launching wizard at '" + targetFileName + "'...");


                DomainUser du = DomainUser.Current;

                //                MessageBox.Show("Current user: " + du.ToString());

                if (DomainUser.IsSystemAccount(du)) {

                    frmInitializing fi = new frmInitializing();

                    fi.Show();
                    Application.DoEvents();

                    // ok, we are be running in the context of the installer user
                    // what we need to do is launch this app again, but this time as the currently logged-in user
                    // this situation usually happens at Configuration Wizard install time (when it is launched from the MSI)
                    try {

                        //ProcessStartInfo psi = new ProcessStartInfo((Utility.GetTargetDirectory(this.Context, null, "ConfigurationWizard") + @"\gguac.exe").Replace(@"\\", @"\"), @" """ + targetFileName + @""" /pid " + pid);
                        //Process p = Process.Start(psi);


                        DomainUser du2 = DomainUser.CurrentLoggedIn;
                        //MessageBox.Show("Going to run new instance of config wizard as: " + du2);
                        if (Toolkit.IsWindowsVistaOrLater) {
                            Toolkit.LaunchProcess((Utility.GetTargetDirectory(this.Context, null, "ConfigurationWizard") + @"\gguac.exe").Replace(@"\\", @"\"), @" """ + targetFileName + @""" /pid " + pid, du2, false, false);
                        } else {
                            Toolkit.LaunchProcess(targetFileName, " /pid " + pid, du2, false, false);
                        }

                        return;
                    } catch (Exception exUser) {
                        MessageBox.Show("Error launching config wizard: " + exUser.Message);
                        return;
                    } finally {
                        fi.Close();
                    }
                }

                //ProcessStartInfo psi = new ProcessStartInfo(targetFileName, args);
                //psi.WorkingDirectory = targetFolder;
                //psi.UseShellExecute = false;
                //Process.Start(psi);

                // note we do not want to wait for it to end.
                // the wizard's exe knows to monitor the given pid and not start processing until the pid is gone.
            } catch (Exception ex){
                Context.LogMessage("Exception launching wizard: " + ex.Message);
            }

        }

        private void WizardInstaller_BeforeUninstall(object sender, InstallEventArgs e) {

            try {

//                showState(e.SavedState);

                // delete the installers folder (clean up after ourselves)
                string tgt = Utility.GetTargetDirectory(this.Context, e.SavedState, "ConfigurationWizard");

                string installerCacheFolder = (tgt + @"\installers").Replace(@"\\", @"\");
                if (Directory.Exists(installerCacheFolder)) {
                    Directory.Delete(installerCacheFolder, true);
                }

            } catch (Exception ex){
                Context.LogMessage("Exception in BeforeUninstall: " + ex.Message);
                // ignore any errors
            } finally {
            }
        }

        private void WizardInstaller_BeforeInstall(object sender, InstallEventArgs e) {
            try {

#if ONLY32BIT
                // 32-bit check
                if (IntPtr.Size != 4) {
                    MessageBox.Show("GRIN-Global must be installed on a 32-bit operating system.  The current operating system (" + Environment.OSVersion.Version + ") is " + (IntPtr.Size * 8) + "-bit.");

                    this.Rollback(e.SavedState);
                    throw new InvalidOperationException("Operating System (" + Environment.OSVersion.VersionString + ") is not 32-bit.");
                }
#endif

                if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1) {
                    // Windows XP
                    string sp = (Environment.OSVersion.ServicePack.ToLower().Replace("service pack", "").Trim() + "asdf").Substring(0, 1);
                    int spInt = 0;
                    int.TryParse(sp, out spInt);
//                    MessageBox.Show(Environment.OSVersion.VersionString + "\n" + Environment.OSVersion.ServicePack + "\n" + sp + "\n" + spInt.ToString());
                    if (spInt < 3) {
                        if (MessageBox.Show("You must have XP Service Pack 3 installed before installing any GRIN-Global applications.\nVisit www.windowsupdate.com to update to the latest service pack.\nWould you like to do this now?", "Need to Update Windows", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes) {
                            Process.Start("http://www.windowsupdate.com/");
                        }
                        //                    this.Rollback(e.SavedState);
                        this.Rollback(e.SavedState);
                        throw new InvalidOperationException("XP Service Pack level must be 3 or greater before installing GRIN-Global");
                    }


                } else if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 0) {
                    // Windows Vista


                } else if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1) {
                    // Windows 7
                }
            } catch (Exception ex) {
                Context.LogMessage("Exception in BeforeInstall: " + ex.Message);
                throw;
            }
        }

    }
}
