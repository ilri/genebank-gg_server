using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GrinGlobal.InstallHelper;
using System.Diagnostics;

namespace GrinGlobal.Search.Engine.Tester {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

//            Application.Run(new frmDatabaseLoginPrompt());

            if (Debugger.IsAttached) {
             args = new string[] { "/autostart" };
             // args = new string[] { "/recreateallenabled" };
            }

            Form1 f = new Form1();

            if (args.Length > 0) {
                for (var i = 0; i < args.Length; i++) {
                    if (args[i].ToLower() == "/autostart" || args[i].ToLower() == "--autostart") {
                        f.AutoStart = true;
                    } else if (args[i].ToLower() == "/recreateallenabled" || args[i].ToLower() == "--recreateallenabled") {
                        f.AutoStart = true;
                        f.RecreateAllEnabled = true;
                    }
                }
            }

			Application.Run(f);
		}
	}
}
