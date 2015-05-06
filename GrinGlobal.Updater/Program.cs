using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using GrinGlobal.InstallHelper;
using GrinGlobal.Core;

namespace GrinGlobal.Updater {
    static class Program {

        static Mutex __mutex = new Mutex(true, "GrinGlobal.Updater.exe");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {




            // valid command line:
            // [/conn [last|url_to_connect_to]] [/download group file [file] [file] [file]] [/refresh]


            string connectToUrl = null;
            string group = null;
            bool refresh = false;
            var files = new List<string>();
            int i = 0;
            while (i < args.Length) {
                switch (args[i].ToLower()) {
                    case "rem":
                    case "#":
                    case ";":
                        // skip all the rest
                        i = args.Length;
                        break;
                    case "/conn":
                    case "-conn":
                    case "--conn":
                    case "/connection":
                    case "-connection":
                    case "--connection":
                    case "/c":
                    case "-c":
                    case "--c":
                        if (i < args.Length - 1) {
                            connectToUrl = args[i + 1];
                            i++;
                        }
                        break;
                    case "/download":
                    case "-download":
                    case "--download":
                    case "/d":
                    case "-d":
                    case "--d":
                        if (i < args.Length - 1) {
                            i++;
                            group = args[i];

                            while (i < args.Length - 1) {
                                i++;
                                files.Add(args[i]);
                            }

                        }
                        break;
                    case "/refresh":
                    case "-refresh":
                    case "--refresh":
                    case "/r":
                    case "-r":
                    case "--r":
                        refresh = true;
                        break;
                }
                i++;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var f = new frmUpdater();
            if (!String.IsNullOrEmpty(connectToUrl)) {
                f.AutoDownloadUrl = Utility.IsWindowsFilePath(connectToUrl) ? Utility.ResolveFilePath(connectToUrl, false) : connectToUrl;
            }
            f.AutoDownloadGroup = group;
            f.AutoDownloadFiles = files;
            f.RefreshGroups = refresh;

            if (f.IsAutoDownloading()){
                // allow multiple instance of updater to run if it is just auto-downloading...
                //MessageBox.Show("Attempting to download following files in Group='" + f.AutoDownloadGroup + "' from URL='" + f.AutoDownloadUrl + "': " + String.Join(", ", f.AutoDownloadFiles.ToArray()));
                f.StartPosition = FormStartPosition.CenterScreen;
                f.ShowInTaskbar = true;
                Application.Run(f);
            } else {
                // only allow a single instance of updater to run if it is not auto-downloading...
                if (__mutex.WaitOne(TimeSpan.Zero, true)) {
                    try {
                        Application.Run(f);
                    } finally {
                        if (__mutex != null) {
                            __mutex.ReleaseMutex();
                        }
                    }
                } else {
                    MessageBox.Show(getDisplayMember("Main", "Only one instance of GRIN-Global Updater can be run at a time."));
                }
            }

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "Updater", "Program", resourceName, null, defaultValue, substitutes);
        }

    }
}
