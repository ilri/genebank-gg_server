using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using GrinGlobal.Business.SearchSvc;
using GrinGlobal.Search.Engine.Service.SearchSvc;
using GrinGlobal.Core;

namespace GrinGlobal.Business {
    /// <summary>
    /// Proxy class for interacting with the search engine class.  Currently only used by pivotajax.aspx.
    /// </summary>
	public class SearchAdmin {

        public static string WCFBindingName {
            get {
                // return Toolkit.GetSetting("SearchEngineBindingName", "BasicHttpBinding_ISearchHost");
                return Toolkit.GetSetting("SearchEngineBindingName", "NetNamedPipeBinding_ISearchHost");
//                return Toolkit.GetSetting("SearchEngineBindingName", "NetTcpBinding_ISearchHost");
            }
        }

        private string _bindingName;
		public SearchAdmin() {
            _bindingName = WCFBindingName;
		}

		public bool Ping() {
            using (SearchHostClient client = new SearchHostClient(_bindingName)) {
                return client.Ping();
			}
		}

		public List<string> ListIndexes(bool enabledOnly) {
            using (SearchHostClient client = new SearchHostClient(_bindingName)) {
                return client.ListIndexes(enabledOnly);
			}
		}

		public void RebuildIndexes(List<string> indexNames, bool onlyIfEnabled, bool rotateInOnComplete){
            using (SearchHostClient client = new SearchHostClient(_bindingName)) {
				client.RebuildIndexes(indexNames, onlyIfEnabled, rotateInOnComplete);
			}
		}

		public void DisableIndexes(List<string> indexNames){
            using (SearchHostClient client = new SearchHostClient(_bindingName)) {
				client.DisableIndexes(indexNames);
			}
		}

		public void EnableIndexes(List<string> indexNames){
            using (SearchHostClient client = new SearchHostClient(_bindingName)) {
				client.EnableIndexes(indexNames);
			}
		}

		public void ReloadIndexes(List<string> indexNames){
            using (SearchHostClient client = new SearchHostClient(_bindingName)) {
				client.ReloadIndexes(indexNames);
			}
		}

		public void RotateIndexes(List<string> indexNames) {
            using (SearchHostClient client = new SearchHostClient(_bindingName)) {
				client.RotateIndexes(indexNames);
			}
		}

		public List<string> VerifyIndexes(List<string> indexNames, bool checkLeaves){
            using (SearchHostClient client = new SearchHostClient(_bindingName)) {
				return client.VerifyIndexes(indexNames, checkLeaves);
			}
		}

		public string GetLatestMessages(int maxMessageCount) {
            using (SearchHostClient client = new SearchHostClient(_bindingName)) {
				return client.GetLatestMessages(maxMessageCount);
			}
		}

		public List<IndexReport> ReportOnIndexes() {
            using (SearchHostClient client = new SearchHostClient(_bindingName)) {
				return client.ReportOnIndexes();
			}
		}

		public void RestartSearchEngine(string localAdminUserName, string localAdminPassword) {

			Toolkit.RunAs(new DomainUser(null, localAdminUserName, localAdminPassword), delegate() {
				System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController("ggse");
				sc.Stop();
				sc.Start();
				return null;
			});

			//// this takes some doing!  we need to start a service
			//DomainUser du = new DomainUser(null, localAdminUserName, localAdminPassword);
			//string toRun = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\net.exe";
			//string output = Toolkit.ExecuteProcess(toRun, "stop ggse", du);
			//output = Toolkit.ExecuteProcess(toRun, "start ggse", du);
		}
	}
}
