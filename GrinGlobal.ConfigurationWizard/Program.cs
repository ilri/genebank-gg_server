using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using GrinGlobal.Core;
using GrinGlobal.InstallHelper;

namespace GrinGlobal.ConfigurationWizard {
    static class Program {

        static Mutex __mutex = new Mutex(true, "GrinGlobal.ConfigurationWizard.exe");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {


            int pid = -1;

            int i = 0;
            while(i < args.Length){
                switch (args[i].ToLower()) {
                    case "/pid":
                    case "-pid":
                    case "--pid":
                        if (i < args.Length - 1) {
                            int.TryParse(args[i+1], out pid);
                            i++;
                        }
                        break;
                }
                i++;
            }


            // if we were given a pid, wait until the process for that pid is done or no longer exists.
            if (pid > -1) {
//                MessageBox.Show("Waiting for installer (pid=" + pid + ") to exit...");
                try {
                    using (Process p = Process.GetProcessById(pid)) {
                        p.WaitForExit();
                    }
                } catch (ArgumentException ae) {
                    // we get an argument exception, the process already exited.
                    // ignore and continue.
                    Debug.WriteLine(ae.Message);
                }

                DomainUser du = DomainUser.Current;

                if (DomainUser.IsSystemAccount(du)) {

                    frmInitializing fi = new frmInitializing();

                    fi.Show();
                    Application.DoEvents();

                    // ok, we are be running in the context of the installer user
                    // what we need to do is launch this app again, but this time as the currently logged-in user
                    // this situation usually happens at Configuration Wizard install time (when it is launched from the MSI)
                    try {
                        DomainUser du2 = DomainUser.CurrentLoggedIn;
                        if (Utility.IsWindowsVistaOrBetter) {
                            Toolkit.LaunchProcess((Utility.GetTargetDirectory(null, null, "ConfigurationWizard") + @"\gguac.exe").Replace(@"\\", @"\"), @" """ + Application.ExecutablePath + @"""", du2, false, false);
                        } else {
                            Toolkit.LaunchProcess(Application.ExecutablePath, null, du2, false, false);
                        }

                        return;
                    } catch (Exception exUser) {
                        MessageBox.Show("Error trying to get current logged in user or run new instance of config wizard: " + exUser.Message);
                        return;
                    } finally {
                        fi.Close();
                    }
                }





            }


            if (__mutex.WaitOne(TimeSpan.Zero, true)) {
                try {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    frmAllInOne faio = new frmAllInOne();
                    faio.txtInstallDir.Text = AppDomain.CurrentDomain.BaseDirectory;
                    Application.Run(faio);
                } finally {
                    if (__mutex != null) {
                        __mutex.ReleaseMutex();
                    }
                }
            } else {
                MessageBox.Show("Only one instance of GRIN-Global Configuration Wizard can be run at a time.");
            }


        }
    }
}
