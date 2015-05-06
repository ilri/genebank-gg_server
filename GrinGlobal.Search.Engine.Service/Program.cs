using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using GrinGlobal.Core;
using System.ServiceModel;

namespace GrinGlobal.Search.Engine.Service {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main() {


            //var f = new frmInstallIndexes();
            //f.HelperPath = (Environment.CurrentDirectory + @"\gguac.exe").Replace(@"\\", @"\");
            //f.ShowDialog();
            


			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[] { new SearchService() };
			ServiceBase.Run(ServicesToRun);
		}
	}
}
