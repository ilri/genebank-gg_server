using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Web;

using GrinGlobal.Search.Engine.Hosting;
using System.Threading;
using GrinGlobal.InstallHelper;
using GrinGlobal.Core;
//using GrinGlobal.Search.Engine.Tester.SearchSvc;
using System.ServiceModel.Security;

namespace GrinGlobal.Search.Engine.Tester {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private delegate void BackgroundCallback(object sender, DoWorkEventArgs args);

		#region BackgroundWorker / GUI synchronization

		private void backgroundProgress(object sender, ProgressChangedEventArgs e) {
			ProgressEventArgs pea = (ProgressEventArgs)e.UserState;
			ListViewItem lvi = new ListViewItem(DateTime.Now.ToString());
			lvi.SubItems.Add(pea.LogType.ToString());
			lvi.SubItems.Add(pea.Message);
			if (pea.LogType.ToString() == "Error") {
				lvi.ForeColor = Color.Red;
			}
            Console.WriteLine(DateTime.Now + " " + pea.Message);
			lvLog.Items.Insert(0, lvi);
		}

//		private bool backgroundCompleted = false;
		private void backgroundDone(object sender, RunWorkerCompletedEventArgs e) {
//			backgroundCompleted = true;
			if (e.Error != null) {
				string txt = "Fatal error occured: " + e.Error.Message + ".";
				MessageBox.Show(txt);
			} else {
			}

            Cursor = Cursors.Default;
            if (this.RecreateAllEnabled) {
                Console.WriteLine(DateTime.Now + " Completed");
                this.Close();
                Application.Exit();
            }

		}

		private void background(BackgroundCallback callback) {
//			backgroundCompleted = false;
			backgroundWorker1 = new BackgroundWorker();
			backgroundWorker1.WorkerReportsProgress = true;
			backgroundWorker1.DoWork += new DoWorkEventHandler(callback);
			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundProgress);
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundDone);
			backgroundWorker1.RunWorkerAsync();
			Cursor = Cursors.WaitCursor;
		}

		#endregion BackgroundWorker / GUI synchronization


        SearchHost _searcher;
        ServiceHost _wcfServer;
        TcpSearchServer _tcpServer;

		private void btnToggle_Click(object sender, EventArgs e) {

            var bindingType = Toolkit.GetSearchEngineBindingType();
            var useWCF = !String.IsNullOrEmpty(bindingType) && bindingType.ToLower() != "tcp";

            var address = Toolkit.GetSearchEngineAddress(null);

            if (useWCF) {

                if (_wcfServer == null) {
				    _itemsToAdd = new List<ProgressEventArgs>();
				    _searcher = new SearchHost(new SearchHost.ProgressEventHandler(Form1_OnProgress));
                    _wcfServer = new ServiceHost(_searcher, new Uri[] { });

                    var binding = Toolkit.GetSearchEngineBinding(null);

                    if (binding != null && address != null) {
                        _wcfServer.AddServiceEndpoint(typeof(ISearchHost), binding, address.Uri);
                        _itemsToAdd.Add(new ProgressEventArgs("WCF Server Listening on " + address.Uri, System.Diagnostics.EventLogEntryType.Information, false));
                    } else {
                        _itemsToAdd.Add(new ProgressEventArgs("Could not initiate WCF Server, no binding or address is configured.", System.Diagnostics.EventLogEntryType.Error, true));
                    }

			    }

                if (_wcfServer.State != CommunicationState.Opened) {

                    // HTTP could not register URL http://+:2011/. Your process does not have access rights to this namespace (see http://go.microsoft.com/fwlink/?LinkId=70353 for details).
                    // Solution (for Vista only):
                    //    from an admin command prompt, run the following:
                    //    netsh http add urlacl url=http://+:2011/ user=[your username]

                    _wcfServer.Open();
				    //background((sentBy, args) => {
				    //    _host.Open();
				    //});
				    btnToggle.Text = "Stop Hosting";
			    } else {
                    _searcher.Dispose();
                    _wcfServer.Close();
				    _wcfServer = null;
				    //background((sentBy, args) => {
				    //    _host.Close();
				    //    _host = null;
				    //});
				    btnToggle.Text = "Start Hosting";
			    }
            } else {

                if (_tcpServer == null) {
                    _itemsToAdd = new List<ProgressEventArgs>();
                    _searcher = new SearchHost(new SearchHost.ProgressEventHandler(Form1_OnProgress));
                    _tcpServer = new TcpSearchServer(address.Uri.Port, _searcher);
                    if (address != null) {
                        _itemsToAdd.Add(new ProgressEventArgs("Basic TCP Server Listening on " + address.Uri, System.Diagnostics.EventLogEntryType.Information, false));
                    } else {
                        _itemsToAdd.Add(new ProgressEventArgs("Could not initiate Basic TCP Server, no binding or address is configured.", System.Diagnostics.EventLogEntryType.Error, true));
                    }

                }

                if (!_tcpServer.IsListening) {

                    // HTTP could not register URL http://+:2011/. Your process does not have access rights to this namespace (see http://go.microsoft.com/fwlink/?LinkId=70353 for details).
                    // Solution (for Vista only):
                    //    from an admin command prompt, run the following:
                    //    netsh http add urlacl url=http://+:2011/ user=[your username]

                    _tcpServer.Start();
                    //background((sentBy, args) => {
                    //    _host.Open();
                    //});
                    btnToggle.Text = "Stop Hosting";
                } else {
                    _searcher.Dispose();
                    _tcpServer.Stop();
                    _tcpServer = null;
                    //background((sentBy, args) => {
                    //    _host.Close();
                    //    _host = null;
                    //});
                    btnToggle.Text = "Start Hosting";
                }
            }
		}

		List<ProgressEventArgs> _itemsToAdd;

		void Form1_OnProgress(object sender, ProgressEventArgs pea) {
			//if (!backgroundCompleted) {
			//    backgroundWorker1.ReportProgress(0, pea);
			//}

			_itemsToAdd.Add(pea); //new string[] { DateTime.Now.ToString(), pea.LogType.ToString(), pea.Message });


		}

		private void btnPing_Click(object sender, EventArgs e) {
            //string mechanism = null;
            //switch (ddlConnectUsing.Text) {
            //    case "http":
            //        mechanism = "BasicHttpBinding_ISearchHost";
            //        break;
            //    case "net.tcp":
            //        mechanism = "NetTcpBinding_ISearchHost";
            //        break;
            //    case "net.pipe":
            //        mechanism = "NetNamedPipeBinding_ISearchHost";
            //        break;
            //    default:
            //        MessageBox.Show("That one isn't implemented!");
            //        return;
            //}
            //using (SearchHostClient client = new SearchHostClient(mechanism)) {
            //    client.GetLatestMessages(1);
            //    if (client.Ping()) {
            //        MessageBox.Show("Ping worked.");
            //    }
            //}
		}

        public bool AutoStart;
        public bool RecreateAllEnabled;

		private void Form1_Load(object sender, EventArgs e) {
			ddlConnectUsing.Items.Clear();
			ddlConnectUsing.Items.Add("http");
			ddlConnectUsing.Items.Add("net.tcp");
			ddlConnectUsing.Items.Add("net.pipe");
			ddlConnectUsing.SelectedIndex = 0;
            if (AutoStart) {
                btnToggle.PerformClick();
            }

            if (RecreateAllEnabled) {
                btnToggle.Enabled = false;
                _searcher.RebuildIndexes(null, true, true);

                bool done = false;
                while (!done) {
                    Thread.Sleep(3000);
                    Application.DoEvents();
                    var dsAsXml = _searcher.GetInfo(true);
                    var ds = Toolkit.DataSetFromXml(dsAsXml);
                    done = Utility.ToInt32(ds.Tables["status"].Rows[0]["processing_indexes"], 0) == 0;
                }
                btnToggle.Enabled = true;
                btnToggle.PerformClick();
                this.Close();
            }

		}

		private void timer1_Tick(object sender, EventArgs e) {
			if (_itemsToAdd != null && _itemsToAdd.Count > 0) {
				ProgressEventArgs[] copy = new ProgressEventArgs[_itemsToAdd.Count];
				_itemsToAdd.CopyTo(copy);
				_itemsToAdd.Clear();

				foreach (ProgressEventArgs pea in copy) {
					ListViewItem lvi = new ListViewItem(DateTime.Now.ToString());
					lvi.SubItems.Add(pea.LogType.ToString());
					lvi.SubItems.Add(pea.Message);
					if (pea.LogType.ToString() == "Error") {
						lvi.ForeColor = Color.Red;
					}
					lvLog.Items.Insert(0, lvi);
				}

			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if (_searcher != null) {
                _searcher.Dispose();
            }
            if (_wcfServer != null) {
                _wcfServer.Close();
			}
            if (_tcpServer != null) {
                _tcpServer.Stop();
            }
		}

        private void button1_Click(object sender, EventArgs e) {
            var gui = new guitest.GUI();
            // gui.Url = "http://test.grin-global.org/gringlobal/gui.asmx";
            var ds = gui.GetData(true, "brock", "passw0rd!", "get_cooperators", null, 0, 0, null);
            var gg = new ggtest.gg();
            var dsToken = gg.ValidateLogin(true, "test1", "test1");
            var token = dsToken.Tables["validate_login"].Rows[0]["login_token"].ToString();
            var ds2 = gg.GetData(true, token, "sys_dataview", null, 0, 0, null);
        }

        private class TestValue {
            public TestValue(string keyword, long[] values) {
                Keyword = new BPlusString(keyword);
                List = new BPlusListLong(values == null ? new long[]{} : values);
            }
            public BPlusString Keyword;
            public BPlusListLong List;
        }

        private void btnTest_Click(object sender, EventArgs e) {

            guitest.GUI gui = new GrinGlobal.Search.Engine.Tester.guitest.GUI();
            var ds = gui.GetData(false, "admin1", "admin1", "sys_lang_lookup", ":createddate=;:displaymember=;:modifieddate=;:startpkey=;:stoppkey=;:valuemember=;", 0, 0, null);
            dataGridView1.Visible = true;
            chkDGVRightToLeft.Visible = true;
            chkMirrored.Visible = true;
            var dt = ds.Tables["sys_lang_lookup"];
            foreach(DataColumn dc in dt.Columns){
                dc.ReadOnly = false;
            }
            dataGridView1.DataSource = dt;

            //foreach (DataRow dr in ds.Tables["sys_lang_lookup"].Rows)
            //{
            //    MessageBox.Show(dr["display_member"].ToString());
            //}

            //using (var tree = BPlusTree<BPlusString, BPlusListLong>.Create("test.bpt", Encoding.Unicode, 100, 11, 1000, 10)) {
            //    // nothing to do, just create it
            //}

            //var maxWords = 20000;

            //// create a list of random words
            //var rnd = new Random();
            //var words = new List<TestValue>();
            //for (var i = 0; i < maxWords; i++) {
            //    long[] values = new long[3];
            //    values[0] = rnd.Next(0, int.MaxValue);
            //    values[1] = rnd.Next(0, int.MaxValue);
            //    values[2] = rnd.Next(0, int.MaxValue);
            //    words.Add(new TestValue("word" + values[0], values));
            //}

            //using (var tree = BPlusTree<BPlusString, BPlusListLong>.Load("test.bpt", false)) {
            //    for (var i = 0; i < words.Count; i++) {
            //        tree.Insert(words[i].Keyword, words[i].List);
            //    }
            //}

            //MessageBox.Show("done");

        }

        private void chkDGVRightToLeft_CheckedChanged(object sender, EventArgs e) {
            dataGridView1.RightToLeft = chkDGVRightToLeft.Checked ? RightToLeft.Yes : RightToLeft.No;
            this.Refresh();
        }

        private void chkMirrored_CheckedChanged(object sender, EventArgs e) {
            this.RightToLeftLayout = chkMirrored.Checked;
            this.Refresh();
            this.Hide();
            this.Show();
        }

        private void btnTest5_Click(object sender, EventArgs e) {
            var dbEngineUtil = DatabaseEngineUtil.CreateInstance("sqlserver", Toolkit.GetPathToUACExe(), "sqlexpress");
            dbEngineUtil.UseWindowsAuthentication = true;
            MessageBox.Show(dbEngineUtil.BaseDirectory + "\n" + dbEngineUtil.BinDirectory + "\n" + dbEngineUtil.DataDirectory + "\n" + dbEngineUtil.Port + "\n" + dbEngineUtil.ServerName + "\n" + dbEngineUtil.ServiceName + "\n" + dbEngineUtil.UseWindowsAuthentication);
            try {
                var output = dbEngineUtil.CreateDatabase(null, "gringlobal");
                MessageBox.Show("output=" + output);
            } catch (Exception ex) {
                MessageBox.Show("error: " + ex.Message + "\n\n" + ex.ToString());
            }
        }
    }
}
