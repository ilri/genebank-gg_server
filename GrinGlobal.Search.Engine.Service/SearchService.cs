using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using GrinGlobal.Search.Engine.Hosting;
using GrinGlobal.Core;

namespace GrinGlobal.Search.Engine.Service {
	public partial class SearchService : ServiceBase {
		public SearchService() {
			InitializeComponent();
			this.ServiceName = "GRIN-Global Search Engine";
			this.EventLog.Log = "Application";
			this.AutoLog = true;
			this.CanPauseAndContinue = true;
			this.CanStop = true;
            _bindingType = Toolkit.GetSearchEngineBindingType();
            _useWCF = !String.IsNullOrEmpty(_bindingType) && _bindingType.ToLower() != "tcp";
            _address = Toolkit.GetSearchEngineAddress(null);
		}

		ServiceHost _wcfServer;
        SearchHost _searcher;
        TcpSearchServer _tcpServer;
        bool _useWCF;
        string _bindingType;
        EndpointAddress _address;

		protected override void OnStart(string[] args) {

			try {

                if (_useWCF) {
                    if (_wcfServer != null) {
                        _wcfServer.Close();
                    }
                    SearchHost.ValidateConfig();

                    // since we want our searchhost object to last as long as the process does
                    // (keep files open), we make one instance of it and give that to the ServiceHost
                    _searcher = new SearchHost();
                    _wcfServer = new ServiceHost(_searcher);

                    
                    var binding = Toolkit.GetSearchEngineBinding(null);
                    var address = Toolkit.GetSearchEngineAddress(null);

                    if (binding != null && address != null) {
                        _wcfServer.AddServiceEndpoint(typeof(ISearchHost), binding, address.Uri);
                        _wcfServer.Open();
                    } else {
                        throw new InvalidOperationException(getDisplayMember("onstart{nowcf}", "No valid WCF end points are defined.  Check SearchEngineBindingType and SearchEngineBindingUrl configuration settings."));
                    }

                } else {

                    if (_tcpServer != null) {
                        _tcpServer.Stop();
                    }
                    SearchHost.ValidateConfig();

                    var port = _address != null && _address.Uri != null ? _address.Uri.Port : 2012;
                    _searcher = new SearchHost();
                    _tcpServer = new TcpSearchServer(port, _searcher);
                    _tcpServer.Start();

                }

				base.OnStart(args);

			} catch (Exception ex) {

				EventLog.WriteEntry("Error starting GRIN-Global Search Engine: " + ex.Message, EventLogEntryType.Error);
				throw;
			}

		}

		protected override void OnPause() {
            if (_useWCF) {
                _wcfServer.Close();
            } else {
                _tcpServer.Stop();
            }
			base.OnPause();
		}

		protected override void OnContinue() {
			base.OnContinue();
            if (_useWCF) {
                _wcfServer.Open();
            } else {
                _tcpServer.Start();
            }
		}

		protected override void OnStop() {
            if (_searcher != null) {
                _searcher.Dispose();
                _searcher = null;
            }
            if (_useWCF) {
                if (_wcfServer != null) {
                    _wcfServer.Close();
                    _wcfServer = null;
                }
            } else {
                if (_tcpServer != null) {
                    _tcpServer.Stop();
                    _tcpServer = null;
                }
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "SearchService", resourceName, null, defaultValue, substitutes);
        }

    }
}
