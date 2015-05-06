using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using GrinGlobal.Business;
using GrinGlobal.Core;
using GrinGlobal.InstallHelper;

namespace GrinGlobal.Import {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            if (Debugger.IsAttached) {

                //MessageBox.Show("connecting to sql server...");
                //args = new string[2] {
                //    "/import", 
                //    "AAEAAAD/////AQAAAAAAAAAMAgAAAE1HcmluR2xvYmFsLkJ1c2luZXNzLCBWZXJzaW9uPTIuMC4xNDg2LjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAUBAAAAIkdyaW5HbG9iYWwuQnVzaW5lc3MuQ29ubmVjdGlvbkluZm8OAAAACExhc3RVc2VkGERhdGFiYXNlRW5naW5lU2VydmVyTmFtZRpEYXRhYmFzZUVuZ2luZVByb3ZpZGVyTmFtZRpEYXRhYmFzZUVuZ2luZURhdGFiYXNlTmFtZRZEYXRhYmFzZUVuZ2luZVVzZXJOYW1lFkRhdGFiYXNlRW5naW5lUGFzc3dvcmQeRGF0YWJhc2VFbmdpbmVSZW1lbWJlclBhc3N3b3JkEkdyaW5HbG9iYWxVc2VyTmFtZRJHcmluR2xvYmFsUGFzc3dvcmQaR3Jpbkdsb2JhbFJlbWVtYmVyUGFzc3dvcmQUR3Jpbkdsb2JhbExvZ2luVG9rZW4NVXNlV2ViU2VydmljZQ1HcmluR2xvYmFsVXJsGFVzZVdpbmRvd3NBdXRoZW50aWNhdGlvbgABAQEBAQABAQABAAEADQEBAQECAAAAtIk70aw/zUgGAwAAABRsb2NhbGhvc3Rcc3FsZXhwcmVzcwYEAAAACXNxbHNlcnZlcgYFAAAACmdyaW5nbG9iYWwGBgAAAAAKAAYHAAAADWFkbWluaXN0cmF0b3IGCAAAAA1hZG1pbmlzdHJhdG9yAQYJAAAAgAFRcTR5Y0lNU0NGd1NVSDRVanl2cVFwY0Q2QlRGdWZXZ1gzL1kzV2p4Z28vaDJRWmhNUlAwUEhCaUZlTTJJcFU2OU1XaUpKV0JDcXlQTWllMTZGdlBIblEwNnpsNFUvWXZKbFJoMzNxZFM2Rld2MjhJTEpMUXFCRTJUTXlNUEdXagAJBgAAAAEL"
                //};

                //MessageBox.Show("connecting to mysql...");
                //args = new string[2]{
                //"/import",  
                //"AAEAAAD/////AQAAAAAAAAAMAgAAAE1HcmluR2xvYmFsLkJ1c2luZXNzLCBWZXJzaW9uPTIuMC4xNjI5LjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAUBAAAAIkdyaW5HbG9iYWwuQnVzaW5lc3MuQ29ubmVjdGlvbkluZm8OAAAACExhc3RVc2VkGERhdGFiYXNlRW5naW5lU2VydmVyTmFtZRpEYXRhYmFzZUVuZ2luZVByb3ZpZGVyTmFtZRpEYXRhYmFzZUVuZ2luZURhdGFiYXNlTmFtZRZEYXRhYmFzZUVuZ2luZVVzZXJOYW1lFkRhdGFiYXNlRW5naW5lUGFzc3dvcmQeRGF0YWJhc2VFbmdpbmVSZW1lbWJlclBhc3N3b3JkEkdyaW5HbG9iYWxVc2VyTmFtZRJHcmluR2xvYmFsUGFzc3dvcmQaR3Jpbkdsb2JhbFJlbWVtYmVyUGFzc3dvcmQUR3Jpbkdsb2JhbExvZ2luVG9rZW4NVXNlV2ViU2VydmljZQ1HcmluR2xvYmFsVXJsGFVzZVdpbmRvd3NBdXRoZW50aWNhdGlvbgABAQEBAQABAQABAAEADQEBAQECAAAAB5PVuL1UzUgGAwAAABMxOTIuMTY4LjU2LjEwMTozMzA2BgQAAAAFbXlzcWwGBQAAAApncmluZ2xvYmFsBgYAAAAHZ2dfdXNlcgYHAAAAEWdnX3VzZXJfcGFzc3cwcmQhAQYIAAAADWFkbWluaXN0cmF0b3IGCQAAAA1hZG1pbmlzdHJhdG9yAQYKAAAAgAFRcTR5Y0lNU0NGd1NVSDRVanl2cVFwY0Q2QlRGdWZXZ1gzL1kzV2p4Z28vdkVOVHh2cWtnbjEyRkNabnFFOGJnQVRRbWZNdkYzSkc1S1c1WDNBbzFDV3dienNibFhKT0xjUGROeXBsMTRLRVR4L3hVdy8zSUNXRTc5M1h3V0ZPbAAGCwAAAAAACw=="};
            }


            var import = false;
            var connInfo = "";
            var password = "";
            var userName = "";
            var url = "";
            var done = false;
            for (var i = 0; i < args.Length; i++) {
                switch (args[i].ToLower()) {
                    case "/import":
                    case "-import":
                    case "--import":
                    case "/i":
                    case "-i":
                    case "--i":
                        import = true;
                        if (i < args.Length - 1) {
                            var nextArg = args[++i];
                            if (nextArg.Contains("-")) {
                                // not connection info.
                                i--;
                            } else {
                                if (nextArg.ToLower() != "rem") {
                                    connInfo = nextArg;
                                }
                            }
                        }
                        break;
                    case "rem":
                        done = true;
                        break;
                    default:
                        if (args[i].Contains("=")){
                            var args2 = args[i].Split('=');
                            if (args2.Length == 2){
                                switch(args2[0].ToLower()){
                                    case "--user":
                                    case "-user":
                                    case "/user":
                                    case "-u":
                                    case "/u":
                                        import = true;
                                        userName = args2[1];
                                        break;
                                    case "-password":
                                    case "/password":
                                    case "--password":
                                        password = args2[1];
                                        break;
                                    case "-url":
                                    case "--url":
                                    case "/url":
                                        url = args2[1];
                                        break;
                                }
                            }
                        }
                        break;
                }
                if (done) {
                    // there's a REM, so ignore processing remaining arguments
                    break;
                }
            }


            //MessageBox.Show("Running as admin? " + Toolkit.IsProcessElevated());

            frmImport f = null;

            if (import || Debugger.IsAttached) {
                f = new frmImport();
                if (!String.IsNullOrEmpty(connInfo)) {
                    f.HostProxy = new HostProxy(ConnectionInfo.Deserialize(connInfo));
                } else if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(url)) {
                    f.HostProxy = new HostProxy(userName, password, url);
                }

                if (f.HostProxy == null) {
                    // no login info given, prompt for it...
                    var flogin = new frmLogin();
                    if (DialogResult.OK == flogin.ShowDialog()) {
                        f.HostProxy = flogin.HostProxy;
                    }
                }
            }

            if (f == null) {
                MessageBox.Show(getDisplayMember("Main{nothing}", "Nothing to do"));
            } else if (f.HostProxy == null){
                MessageBox.Show(getDisplayMember("Main{badlogin}", "Login credentials are missing or invalid."));
            } else {
                Application.Run(f);
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "ImportWizard", "Program", resourceName, null, defaultValue, substitutes);
        }

    }
}
