using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Search.Engine;
using GrinGlobal.Core;

namespace GrinGlobal.Search.Engine.AdminTool {
	public partial class frmSearch : Form {
		public frmSearch() {
			InitializeComponent();
		}

		private List<int> performSearch(string searchString, bool ignoreCase, bool autoAndConsecutiveLiterals, List<string> indexNames, string resolverName) {
			if (chkUseService.Checked) {
				GrinGlobalSearch.SearchHostClient client = new GrinGlobal.Search.Engine.AdminTool.GrinGlobalSearch.SearchHostClient();
				return client.Search(searchString, ignoreCase, autoAndConsecutiveLiterals, indexNames, resolverName, 0, 0);
//				return null;
			} else {
				return _indexer.Search(searchString, ignoreCase, autoAndConsecutiveLiterals, indexNames, resolverName, 0, 0).ToList();
			}
		}

		private void btnGo_Click(object sender, EventArgs e) {
			// perform the search on each of the checked indexes

			try {
				this.Cursor = Cursors.WaitCursor;

				List<string> indexNames = new List<string>();
				foreach(string name in clbIndexes.CheckedItems){
					indexNames.Add(name);
				}

				StringBuilder sb = new StringBuilder();
				int count = 0;
				using (HighPrecisionTimer timer = new HighPrecisionTimer("timer", true)) {
					foreach (int accessionID in performSearch(txtSearch.Text, chkIgnoreCase.Checked, chkMatchAll.Checked, indexNames, "Accessions")) {
						sb.AppendLine(accessionID.ToString());
						count++;
					}
					timer.Stop();
					txtAccessions.Text = sb.ToString();
					lblAccessions.Text = "Accessions - " + count.ToString("G");
					lblAccessionsTime.Text = "Search Time: " + timer.ElapsedMilliseconds.ToString("##,###.000") + " ms";
				}

				sb = new StringBuilder();
				count = 0;
				using (HighPrecisionTimer timer = new HighPrecisionTimer("timer", true)) {
					foreach (int inventoryID in performSearch(txtSearch.Text, chkIgnoreCase.Checked, chkMatchAll.Checked, indexNames, "Inventory")) {
						sb.AppendLine(inventoryID.ToString());
						count++;
					}
					timer.Stop();
					txtInventory.Text = sb.ToString();
					lblInventory.Text = "Inventory - " + count.ToString("G");
					lblInventoryTime.Text = "Search Time: " + timer.ElapsedMilliseconds.ToString("##,###.000") + " ms";
				}

				sb = new StringBuilder();
				count = 0;
				using (HighPrecisionTimer timer = new HighPrecisionTimer("timer", true)) {
					foreach (int orderID in performSearch(txtSearch.Text, chkIgnoreCase.Checked, chkMatchAll.Checked, indexNames, "Orders")) {
						sb.AppendLine(orderID.ToString());
						count++;
					}
					txtOrders.Text = sb.ToString();
					lblOrders.Text = "Orders - " + count.ToString("G");
					lblOrdersTime.Text = "Search Time: " + timer.ElapsedMilliseconds.ToString("##,###.000") + " ms";
				}

			} catch (Exception ex){
				MessageBox.Show(ex.Message);
			} finally {
				this.Cursor = Cursors.Default;
			}

		}

		private Indexer<BPlusString, BPlusListLong> _indexer;

		private void frmSearch_Load(object sender, EventArgs e) {

			_indexer = Indexer<BPlusString, BPlusListLong>.LoadConfigFromFile(null);

			_indexer.LoadAllIndexes();

			clbIndexes.Items.Clear();
			foreach (string key in _indexer.GetIndexNames()) {
				clbIndexes.Items.Add(_indexer.GetIndex(key).Name, true);
			}

		}

		private void chkCheckAll_CheckedChanged(object sender, EventArgs e) {
			for (int i=0; i < clbIndexes.Items.Count; i++) {
				clbIndexes.SetItemChecked(i, chkCheckAll.Checked);
			}
		}
	}
}
