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
using System.IO;
using System.Diagnostics;
using System.Threading;
namespace GrinGlobal.Search.Engine.AdminTool {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private Indexer<BPlusString, BPlusListLong> _indexer;

		private int _limit = 10000;

		#region BackgroundWorker / GUI synchronization

		private BackgroundWorker _worker;
		private delegate void BackgroundCallback(object sender, DoWorkEventArgs args);

		private void showProgress(string message, EventLogEntryType logType) {
			_worker.ReportProgress(0, new ProgressEventArgs(message, logType, false));
		}
		void c_OnProgress(object sender, ProgressEventArgs pea) {
			_worker.ReportProgress(0, pea);
		}

		private void backgroundProgress(object sender, ProgressChangedEventArgs e) {
			ProgressEventArgs pea = (ProgressEventArgs)e.UserState;
			//if (pea.Message.StartsWith("SCHEMA:")) {
			//    string schema = pea.Message.Substring("SCHEMA:".Length);
			//    frmPreview fp = new frmPreview();
			//    fp.txtPreview.Text = schema;
			//    fp.txtPreview.SelectionStart = 0;
			//    fp.txtPreview.SelectionLength = 0;
			//    fp.Show(this);
			//} else {
				statusText.Text = pea.Message;
			//}
		}

		private void backgroundDone(object sender, RunWorkerCompletedEventArgs e) {
			statusText.Text = "Done";
			statusProgress.Value = 100;
			_timer.Stop();
			if (e.Error != null) {
				string txt = "Fatal error occured: " + e.Error.Message + ".";
				MessageBox.Show(txt);
			} else {

				// fill combobox

//				cbIndexes.DataSource = _indexer.GetIndexNames().ToList();


				// fill treeview

				//tvIndexes.Nodes.Clear();
				//foreach (Index idx in _indexer.Indexes) {
				//    TreeNode tnIndex = new TreeNode(idx.Name);
				//    foreach (string itemKey in idx.Words.Keys) {
				//        TreeNode tnItemSet = new TreeNode(itemKey);
				//        IndexWord word = idx.Words[itemKey];

				//        foreach (IndexHit ii in word.Hits) {
				//            TreeNode tnItem = new TreeNode(ii.ID + "," + ii.FieldDefinitionOffset + "," + ii.WordOffset);
				//            for(int i=0;i<ii.Fields.Length;i++){
				//                TreeNode tnField = new TreeNode(ii.Fields[i].Name + ":  " + ii.Fields[i].Value.ToString());
				//                tnItem.Nodes.Add(tnField);
				//            }
				//            tnItemSet.Nodes.Add(tnItem);
				//        }
				//        tnIndex.Nodes.Add(tnItemSet);
				//    }
				//    tvIndexes.Nodes.Add(tnIndex);
				//}
			}
			Cursor = Cursors.Default;
		}

		HighPrecisionTimer _timer;

		private void background(BackgroundCallback callback) {
			statusText.Text = "-";
			statusProgress.Value = 0;
			_worker = new BackgroundWorker();
			_worker.WorkerReportsProgress = true;
			_worker.DoWork += new DoWorkEventHandler(callback);
			_worker.ProgressChanged += new ProgressChangedEventHandler(backgroundProgress);
			_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundDone);
			_worker.RunWorkerAsync();
			_timer = new HighPrecisionTimer("backgroundWorker", true);
			Cursor = Cursors.WaitCursor;
		}

		#endregion BackgroundWorker / GUI synchronization



		private void button1_Click(object sender, EventArgs e) {
			lbKeywords.DataSource = null;
			lvHits.Items.Clear();
			background((sentBy, args) => {
				if (_indexer != null) {
					_indexer.Dispose();
				}

				showProgress("Reading index configuration ...", EventLogEntryType.Information);
				_indexer = Indexer<BPlusString, BPlusListLong>.LoadConfigFromFile(null);
				_indexer.OnProgress += new Indexer<BPlusString, BPlusListLong>.ProgressEventHandler(indexer_OnProgress);
				showProgress("Creating all indexes ...", EventLogEntryType.Information);
//				_indexer.CreateAllIndexes();

//				_indexer.Indexes["accession_source"].Create();

			});
		}

		void indexer_OnProgress(object sender, ProgressEventArgs pea) {
			if (pea != null) {
				showProgress(pea.Message, pea.LogType);
			}
		}

		private void cbIndexes_SelectedIndexChanged(object sender, EventArgs e) {
			string name = cbIndexes.SelectedItem as string;
			if (!String.IsNullOrEmpty(name)){
				List<BPlusString> keywords = _indexer.GetIndex(name).GetAllKeywords().ToList();
				if (keywords.Count > _limit){
					lblKeywords.Text = "Keywords - showing " + _limit + " of " + keywords.Count;
					keywords.RemoveRange(_limit, keywords.Count - _limit);
				} else {
					lblKeywords.Text = "Keywords - " + keywords.Count;
				}

				lbKeywords.DataSource = keywords;
			}
		}

		private void lbKeywords_SelectedIndexChanged(object sender, EventArgs e) {

			string keyword = lbKeywords.SelectedItem.ToString();

			string name = cbIndexes.SelectedItem as string;
			if (name != null) {

				Index<BPlusString, BPlusListLong> idx = _indexer.GetIndex(name);

				SearchCommand<BPlusString, BPlusListLong> cmd = new SearchCommand<BPlusString, BPlusListLong> {
			        Keyword = new BPlusString().Parse(keyword),
			        KeywordIndex = -1,
			        FieldName = null,
			        MatchMode = KeywordMatchMode.ExactMatch,
			        IgnoreCase = false
			    };


			    // get all the hit information
			    IEnumerable<Hit> hits = idx.GetHits(cmd);

			    lvHits.Items.Clear();
			    int total = lvHits.Columns.Count;
			    for (int i=4; i < total; i++) {
			        lvHits.Columns.RemoveAt(4);
			    }

			    foreach (Field f in idx.IndexedFields) {
			        lvHits.Columns.Add(f.Name);
			    }

			    int totalHits = 0;
			    foreach (Hit h in hits) {
			        if (totalHits < _limit) {
			            ListViewItem lvi = new ListViewItem(h.PrimaryKeyID.ToString());
			            lvi.SubItems.Add(h.FieldOrdinal.ToString());
			            lvi.SubItems.Add(idx.DatabaseFields[h.FieldOrdinal].Name);
			            lvi.SubItems.Add(h.KeywordIndex.ToString());
			            //foreach(HitField hf in h.Fields){
			            //    lvi.SubItems.Add(hf.Value.ToString());
			            //}

			            lvHits.Items.Add(lvi);
			        }
			        totalHits++;
			    }

			    if (totalHits < _limit) {
			        lblHits.Text = "Hits - " + lvHits.Items.Count;
			    } else {
			        lblHits.Text = "Hits - showing " + lvHits.Items.Count + " of " + totalHits;
			    }

			    fillResolverLists(idx, hits);


			}

		}

		private void fillResolverLists(Index<BPlusString, BPlusListLong> idx, IEnumerable<Hit> hits) {
			// fill each resolver list
			try {
				List<int> accessionIDs = idx.GetResolvedIDs(hits, "Accessions");
				if (accessionIDs.Count < _limit) {
					lblAccessions.Text = "Accessions - " + accessionIDs.Count;
				} else {
					lblAccessions.Text = "Accessions - showing " + _limit + " of " + accessionIDs.Count;
					accessionIDs.RemoveRange(_limit, accessionIDs.Count - _limit);
				}
				lbAccessions.DataSource = accessionIDs;
			} catch (Exception ex) {
				if (ex.Message.Contains("'CacheEnabled' flag is false")) {
					// just eat it, we're trying to resolve something we can't.
					lbAccessions.DataSource = new string[] { "(resolver disabled)" };
				} else {
					throw;
				}
			}

			try {
				List<int> inventoryIDs = idx.GetResolvedIDs(hits, "Inventory");
				if (inventoryIDs.Count < _limit) {
					lblInventory.Text = "Inventory - " + inventoryIDs.Count;
				} else {
					lblInventory.Text = "Inventory - showing " + _limit + " of " + inventoryIDs.Count;
					inventoryIDs.RemoveRange(_limit, inventoryIDs.Count - _limit);
				}
				lbInventory.DataSource = inventoryIDs;
			} catch (Exception ex2) {
				if (ex2.Message.Contains("'CacheEnabled' flag is false")) {
					// just eat it, we're trying to resolve something we can't.
					lbInventory.DataSource = new string[] { "(resolver disabled)" };
				} else {
					throw;
				}
			}

			try {
				List<int> orderIDs = idx.GetResolvedIDs(hits, "Orders");
				if (orderIDs.Count < _limit) {
					lblOrders.Text = "Orders - " + orderIDs.Count;
				} else {
					lblOrders.Text = "Orders - showing " + _limit + " of " + orderIDs.Count;
					orderIDs.RemoveRange(_limit, orderIDs.Count - _limit);
				}
				lbOrders.DataSource = orderIDs;
			} catch (Exception ex3) {
				if (ex3.Message.Contains("'CacheEnabled' flag is false")) {
					// just eat it, we're trying to resolve something we can't.
					lbOrders.DataSource = new string[] { "(resolver disabled)" };
				} else {
					throw;
				}
			}
		}

		private void btnLoadIndexes_Click(object sender, EventArgs e) {
			background((sentBy, args) => {
				showProgress("Loading index information...", EventLogEntryType.Information);
				_indexer = Indexer<BPlusString, BPlusListLong>.LoadConfigFromFile(null);
				_indexer.LoadAllIndexes();

			});
		}

		private void writeBytes(BinaryWriter bw, string s) {
			bw.Seek(0, SeekOrigin.Begin);
			bw.Write(s);
			Debug.WriteLine("length=" + s.Length + ", Position=" + bw.BaseStream.Position);
		}

		private void checkKeywordCount(BPlusTree<BPlusString, BPlusListLong> tree, string keyword, int expectedCount) {
			int count = 0;
			foreach (BPlusString key in tree.GetAllKeywords()) {
				count++;
			}
			if (expectedCount != count) {
				MessageBox.Show("Keyword: " + keyword + "\nExpected: " + expectedCount + "\nGot: " + count);
			}
		}

//		BPlusTree _tree;
		private void debugMultithreadedInsert() {

			// give up a couple of seconds so the first call after the threadstart has a chance to process for a bit

			Thread.Sleep(2000);

//			List<KeywordMatch> kms  = _tree.Search("charactercode", KeywordMatchMode.ExactMatch, true, 10000);

			// 1.
			//_tree.Insert("aardvark", 13, 0);
		}

		private void btnBPlusTree_Click(object sender, EventArgs e) {


		}

		private void lvHits_SelectedIndexChanged(object sender, EventArgs e) {
			foreach (ListViewItem lvi in lvHits.SelectedItems) {
				int pkID = Toolkit.ToInt32(lvi.SubItems[0].Text, 0);

				List<Hit> hits = new List<Hit>();
				hits.Add(new Hit { PrimaryKeyID = pkID });


				string name = cbIndexes.SelectedItem as string;
				Index<BPlusString, BPlusListLong> idx = _indexer.GetIndex(name);
				fillResolverLists(idx, hits);

			}
		}

		private void btnCheckIndexes_Click(object sender, EventArgs e) {
			background((sentBy, args) => {
				showProgress("Loading index information...", EventLogEntryType.Information);
				_indexer = Indexer<BPlusString, BPlusListLong>.LoadConfigFromFile(null);
				_indexer.LoadAllIndexes();
				foreach (string key in _indexer.GetIndexNames()) {
                    _indexer.GetIndex(key).VerifyIntegrity(false);
				}
			});

		}

		private void btnSearch_Click(object sender, EventArgs e) {

			//GrinGlobalSearch.SearchHostClient client = new GrinGlobalSearch.SearchHostClient();
			//int[] output = client.Search("zea", true, true, new string[] { "taxonomy" }, "Accessions");

			frmSearch srch = new frmSearch();
			try {
				srch.Show(this);
			} catch (Exception ex) {
				srch.Close();
				MessageBox.Show("Could not open search window: " + ex.Message);
			}
		}

		private void createIndexesToolStripMenuItem_Click(object sender, EventArgs e) {

		}

		private void fileToolStripMenuItem_Click(object sender, EventArgs e) {

		}

		private void Form1_Load(object sender, EventArgs e) {
			_indexer = Indexer<BPlusString, BPlusListLong>.LoadConfigFromFile(null);
			refreshGUIForNewIndexes();
		}

		private void toolStripMenuItem2_Click(object sender, EventArgs e) {
			if (_indexer != null) {
				openFileDialog1.FileName = _indexer.ConfigFilePath;
			} else {
				openFileDialog1.FileName = Toolkit.ResolveDirectoryPath("~/", false);
			}
			openFileDialog1.InitialDirectory = Directory.GetParent(openFileDialog1.FileName).FullName;
			openFileDialog1.Filter = "Indexer Config Files (*.config)|*.config|Xml Files (*.xml)|*.xml|All Files (*.*)|*.*";
			DialogResult dr = openFileDialog1.ShowDialog(this);
			if (dr == DialogResult.OK) {
				if (_indexer != null) {
					_indexer.Dispose();
				}
				_indexer = Indexer<BPlusString, BPlusListLong>.LoadConfigFromFile(openFileDialog1.FileName);
				refreshGUIForNewIndexes();

			}
		}

		private void refreshGUIForNewIndexes() {

			// create index menu items
			createIndexesToolStripMenuItem.DropDownItems.Clear();
			createIndexesToolStripMenuItem.DropDownItems.Add("(All Enabled)", null, createIndexesSubmenuClick);
			foreach (string name in _indexer.GetIndexNames()) {
				createIndexesToolStripMenuItem.DropDownItems.Add(name, null, createIndexesSubmenuClick);
			}

			// verify index menu items 
			verifyIndexesMenuItem.DropDownItems.Clear();
			verifyIndexesMenuItem.DropDownItems.Add("(All Enabled)", null, verifyIndexesSubmenuClick);
			foreach (string name in _indexer.GetIndexNames()) {
				verifyIndexesMenuItem.DropDownItems.Add(name, null, verifyIndexesSubmenuClick);
				verifyIndexesMenuItem.DropDownItems[verifyIndexesMenuItem.DropDownItems.Count - 1].Enabled = _indexer.GetIndex(name).Enabled;
			}


			// load index menu items
			loadIndexToolStripMenuItem.DropDownItems.Clear();
			loadIndexToolStripMenuItem.DropDownItems.Add("(All Enabled)", null, loadIndexesSubmenuClick);
			foreach (string name in _indexer.GetIndexNames()) {
				loadIndexToolStripMenuItem.DropDownItems.Add(name, null, loadIndexesSubmenuClick);
				loadIndexToolStripMenuItem.DropDownItems[loadIndexToolStripMenuItem.DropDownItems.Count-1].Enabled = _indexer.GetIndex(name).Enabled;
			}

			// fill dropdown

		}

		private void createIndexesSubmenuClick(object sender, EventArgs e) {
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			try {
				Cursor = Cursors.WaitCursor;
				if (menu.Text.StartsWith("(All")) {
					foreach (string key in _indexer.GetIndexNames()) {
						if (_indexer.GetIndex(key).Enabled) {
							statusText.Text = "Creating index " + key + "...";
							Application.DoEvents();
							_indexer.GetIndex(key).Create(false);
						} else {
							statusText.Text = "Skipping index " + key + " (disabled)";
							Application.DoEvents();
							Thread.Sleep(250);
						}
					}
					MessageBox.Show("Indexes created.", "Index Creation Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
				} else {
					Index<BPlusString, BPlusListLong> idx = _indexer.GetIndex(menu.Text);
					if (!idx.Enabled) {
						DialogResult dr = MessageBox.Show("Index " + idx.Name + " is not enabled in the config file.  Would you like to enable it now?", "Enable Index?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
						if (dr == DialogResult.Yes) {
							idx.Enabled = true;
							_indexer.SaveConfig();
						} else {
							return;
						}
					}
					_indexer.GetIndex(menu.Text).Create(false);
					MessageBox.Show("Index " + menu.Text + " created.", "Index Creation Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			} catch (Exception ex) {
				MessageBox.Show("Error creating index " + menu.Text + ": \r\n" + ex.Message, "Index Creation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} finally {
				statusText.Text = "Index creation finished";
				Cursor = Cursors.Default;
			}
		}

		private void verifyIndexesSubmenuClick(object sender, EventArgs e) {
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			try {
				Cursor = Cursors.WaitCursor;
				if (menu.Text.StartsWith("(All")) {
					StringBuilder sb = new StringBuilder();
					int count = _indexer.GetIndexNames().Count;
					int i = 0;
					foreach (string key in _indexer.GetIndexNames()) {
						i++;
						try {
							statusProgress.Value = (int)(((decimal)(decimal)i / (decimal)count) * 100);
							if (_indexer.GetIndex(key).Enabled) {
								statusText.Text = "Verifying index " + key + "...";
								Application.DoEvents();
								_indexer.GetIndex(key).VerifyIntegrity(false);
//                            } else {
//                                statusText.Text = "Skipping index " + key + " (disabled)";
//                                Application.DoEvents();
//								Thread.Sleep(250);
							}
						} catch (Exception ex1) {
							sb.AppendLine("Invalid index " + key + ": " + ex1.Message);
						}
					}

					if (sb.Length == 0) {
						MessageBox.Show("All indexes are valid.", "Index Verification Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
					} else {
						MessageBox.Show("Only the following indexes are invalid:\n" + sb.ToString(), "Index Verification Failued", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				} else {
					if (_indexer.GetIndex(menu.Text).Enabled) {
						statusText.Text = "Verifying index " + menu.Text + "...";
						Application.DoEvents();
						_indexer.GetIndex(menu.Text).VerifyIntegrity(false);
					} else {
						statusText.Text = "Skipping index " + menu.Text + " (disabled)";
						Application.DoEvents();
						Thread.Sleep(250);
					}
					MessageBox.Show("Index " + menu.Text + " is valid.", "Index Verification Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}


			} catch (Exception ex) {
				MessageBox.Show("Error verifying index " + menu.Text + ": \r\n" + ex.Message, "Index Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} finally {
				statusText.Text = "Index verification finished";
				Cursor = Cursors.Default;
			}
		}

		private void loadIndexesSubmenuClick(object sender, EventArgs e) {
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			try {
				Cursor = Cursors.WaitCursor;
				statusText.Text = "Loading index(es)...";
				if (menu.Text.StartsWith("(All")) {
					_indexer.LoadAllIndexes();
					cbIndexes.Items.Clear();
					foreach (string key in _indexer.GetIndexNames()) {
						if (_indexer.GetIndex(key).Enabled) {
							cbIndexes.Items.Add(key);
						}
					}
				} else {
					_indexer.GetIndex(menu.Text).Load();
					if (_indexer.GetIndex(menu.Text).Enabled) {
						cbIndexes.Items.Add(menu.Text);
					}
				}
				cbIndexes.SelectedIndex = cbIndexes.Items.Count - 1;
			} catch (Exception ex) {
				MessageBox.Show("Error loading index " + menu.Text + ": \r\n" + ex.Message, "Index Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} finally {
				statusText.Text = "Index loading finished";
				Cursor = Cursors.Default;
			}
		}

		private void verifyIndexesMenuItem_Click(object sender, EventArgs e) {

		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void loadIndexToolStripMenuItem_Click(object sender, EventArgs e) {

		}

		private void allToolStripMenuItem_Click(object sender, EventArgs e) {

		}

		private void allToolStripMenuItem1_Click(object sender, EventArgs e) {

		}

		private void btnTestCreateTree_Click(object sender, EventArgs e) {

			string fileName = Toolkit.ResolveFilePath("./test.bpt", true);
			if (File.Exists(fileName)) {
				File.Delete(fileName);
			}

            //using (BPlusTree<BPlusString, BPlusListLong>  tree1 = BPlusTree<BPlusString, BPlusListLong>.Create(fileName + "1", Encoding.UTF8, 3)) {
            //    tree1.Insert("aardvark", new BPlusListLong(new long[] { 13, 47, 22 }));
            //    tree1.Insert("byproduct", new BPlusListLong(new long[] { 5 }));
            //    tree1.Insert("charactercode", new BPlusListLong(new long[] { 21 }));
            //    // this one should split the root into [ab] [c] [d]
            //    tree1.Insert("delaware", new BPlusListLong(new long[] { 32, 14 }));
            //    tree1.Insert("elephant", new BPlusListLong(new long[] { 41 }));
            //    tree1.Insert("foliage", new BPlusListLong(new long[] { 59 }));
            //}


            //using (BPlusTree<BPlusString, BPlusListLong> tree1 = BPlusTree<BPlusString, BPlusListLong>.Load(fileName + "1", true)){
            //    foreach(BPlusTreeLeafNode<BPlusString, BPlusListLong> leaf in tree1.GetAllLeaves()){
            //        Debug.WriteLine(leaf.ToString());
            //    }
            //}

            //using (BPlusTree<BPlusString, BPlusListLong> tree1 = BPlusTree<BPlusString, BPlusListLong>.Load(fileName + "1", true)) {
            //    foreach (BPlusTreeNode<BPlusString, BPlusListLong> node in tree1.TraverseBreadthFirst() ) {
            //        Debug.WriteLine(node.ToString());
            //    }
            //}

			SortedDictionary<string, List<long>> dic = new SortedDictionary<string, List<long>>();
			using (BPlusTree<BPlusString, BPlusListLong>  bulkTree = BPlusTree<BPlusString, BPlusListLong>.Create(fileName, Encoding.UTF8, 3, 10, 5, 5)) {
				using (BinaryWriter bw = new BinaryWriter(new MemoryStream())) {
					bw.Write("aardvark");
					bw.Write(3);
					bw.Write((long)13);
					bw.Write((long)47);
					bw.Write((long)22);

					bw.Write("byproduct");
					bw.Write(1);
					bw.Write((long)5);

					bw.Write("charactercode");
					bw.Write(1);
					bw.Write((long)21);

					bw.Write("delaware");
					bw.Write(2);
					bw.Write((long)32);
					bw.Write((long)14);

					bw.Write("elephant");
					bw.Write(1);
					bw.Write((long)41);

					bw.Write("foliage");
					bw.Write(1);
					bw.Write((long)59);

					bw.Write("georgia");
					bw.Write(1);
					bw.Write((long)63);

					bw.Write("homeworkism");
					bw.Write(1);
					bw.Write((long)131);

					bw.Write("indigo");
					bw.Write(1);
					bw.Write((long)217);

					bw.Write("jello");
					bw.Write(1);
					bw.Write((long)142);

					bw.Write("kangaroo");
					bw.Write(1);
					bw.Write((long)150);

					bw.Write("literal");
					bw.Write(1);
					bw.Write((long)167);

					bw.Write("mommy");
					bw.Write(1);
					bw.Write((long)169);

					bw.Write("nuance");
					bw.Write(1);
					bw.Write((long)173);

					bw.Write("omaha");
					bw.Write(1);
					bw.Write((long)180);

					bw.Write("penny");
					bw.Write(1);
					bw.Write((long)233);

					bw.Write("qwerty");
					bw.Write(1);
					bw.Write((long)250);

					bw.Write("rollin'");
					bw.Write(1);
					bw.Write((long)261);

					bw.Write("semi-professional");
					bw.Write(1);
					bw.Write((long)99);

					bw.Write("trouble");
					bw.Write(1);
					bw.Write((long)269);

					bw.Write("umpire");
					bw.Write(1);
					bw.Write((long)271);

					bw.Write("vampire");
					bw.Write(1);
					bw.Write((long)280);

					bw.Write("weaver");
					bw.Write(1);
					bw.Write((long)290);

					bw.Write("xylophone");
					bw.Write(1);
					bw.Write((long)102);

					bw.Write("yellowbelly");
					bw.Write(1);
					bw.Write((long)120);

					bw.Write("zebra");
					bw.Write(1);
					bw.Write((long)210);

                    int max = 10000;

					for (int i=1; i < max; i++) {
						bw.Write("zebra" + i.ToString().PadLeft(4, '0'));
						bw.Write(1);
						bw.Write((long)(200 + i));
					}
					bw.BaseStream.Position = 0;
					bulkTree.BulkInsert(bw.BaseStream, 1.0M);
				}
			}


            using (BPlusTree<BPlusString, BPlusListLong> tree = BPlusTree<BPlusString, BPlusListLong>.Load(fileName, true)) {
                foreach (BPlusTreeNode<BPlusString, BPlusListLong> node in tree.TraverseBreadthFirst(true)) {
                    Debug.WriteLine(node.ToString());
                }

                List<KeywordMatch<BPlusString, BPlusListLong>> matches = tree.Search("zebra", KeywordMatchMode.ExactMatch, true);
                foreach (var match in matches) {
                    Debug.WriteLine(match.ToString());
                }

            }

			using (BPlusTree<BPlusString, BPlusListLong> tree = BPlusTree<BPlusString, BPlusListLong>.Load(fileName, false)) {

				tree.Insert("abdomen", new BPlusListLong(new long[] { 1, 2, 3 }.ToList()));

//				tree.Update("abdomen", KeywordMatchMode.ExactMatch, new BPlusListLong(new long[] { 14, 98 }.ToList()), UpdateMode.Add);
//				tree.Update("abdomen", KeywordMatchMode.ExactMatch, new BPlusListLong(new long[] { 14, 98 }.ToList()), UpdateMode.Replace);
//				tree.Update("abdomen", KeywordMatchMode.ExactMatch, new BPlusListLong(new long[] { 3 }.ToList()), UpdateMode.Subtract);

				tree.Update("a", KeywordMatchMode.StartsWith, new BPlusListLong(new long[] { 3 }.ToList()), UpdateMode.Add);

				//foreach (KeywordMatch km in tree.GetAllMatches()) {
				//    MessageBox.Show(km.Keyword + "=" + km.Value.ToString());
				//}

				//foreach (BPlusString keyword in tree.GetAllKeywords()) {
				//    MessageBox.Show(keyword.Value);
				//}


				// ExactMatch tests
				List<KeywordMatch<BPlusString, BPlusListLong>> matches = tree.Search("a", KeywordMatchMode.StartsWith, false);  // should return 2 items

				foreach (string key in dic.Keys) {
					matches = tree.Search(new BPlusString(key), KeywordMatchMode.ExactMatch, false);
					if (matches.Count != 1) {
						MessageBox.Show("Invalid!  did not find value '" + key + "' in the tree!");
					}
				}




				matches = tree.Search("homeworkism", KeywordMatchMode.ExactMatch, false);

				matches = tree.Search("Elephant", KeywordMatchMode.ExactMatch, false);						// should return no items
				matches = tree.Search("CHaractercode", KeywordMatchMode.ExactMatch, true);					// should return 1 item
				matches = tree.Search("semi-professional", KeywordMatchMode.ExactMatch, true);			// should return 1 item
				matches = tree.Search("nonexistantkeyhere", KeywordMatchMode.StartsWith, true);				// should return no items

				// StartsWith tests
				matches = tree.Search("byp", KeywordMatchMode.StartsWith, false);				// should return 1 item
				matches = tree.Search("Elepha", KeywordMatchMode.StartsWith, false);				// should return no items
				matches = tree.Search("CHar", KeywordMatchMode.StartsWith, true);				// should return 1 item
				matches = tree.Search("nonexistantkeyhere", KeywordMatchMode.StartsWith, true);	// should return no items
				matches = tree.Search("semi-pro", KeywordMatchMode.StartsWith, true);			// should return 1 item

				// EndsWith tests
				matches = tree.Search("duct", KeywordMatchMode.EndsWith, false);				// should return 1 item
				matches = tree.Search("ANt", KeywordMatchMode.EndsWith, false);					// should return no items
				matches = tree.Search("CoDe", KeywordMatchMode.EndsWith, true);					// should return 1 item
				matches = tree.Search("ISM", KeywordMatchMode.EndsWith, true);					// should return 1 item
				matches = tree.Search("ssional", KeywordMatchMode.EndsWith, true);				// should return 1 item

				// Contains tests
				matches = tree.Search("pro", KeywordMatchMode.Contains, true);					// should return 2 items
				matches = tree.Search("RAc", KeywordMatchMode.Contains, true);					// should return 1 items
				matches = tree.Search("ARA", KeywordMatchMode.Contains, false);					// should return no items
				matches = tree.Search("-", KeywordMatchMode.Contains, false);					// should return 1 item
				matches = tree.Search("zebra0030", KeywordMatchMode.ExactMatch, false);					// should return 1 item
				matches = tree.Search("zebra0012", KeywordMatchMode.ExactMatch, false);					// should return 1 item
				matches = tree.Search("zebra0032", KeywordMatchMode.ExactMatch, false);					// should return 1 item
				matches = tree.Search("zebra0039", KeywordMatchMode.ExactMatch, false);					// should return 1 item
				matches = tree.Search("zebra0040", KeywordMatchMode.ExactMatch, false);					// should return 1 item
				matches = tree.Search("zebra0060", KeywordMatchMode.ExactMatch, false);					// should return 1 item
				matches = tree.Search("zebra0051", KeywordMatchMode.ExactMatch, false);					// should return 1 item
				matches = tree.Search("zebra0058", KeywordMatchMode.ExactMatch, false);					// should return 1 item
				matches = tree.Search("zebra0099", KeywordMatchMode.ExactMatch, false);					// should return 1 item
				matches = tree.Search("zebra0072", KeywordMatchMode.ExactMatch, false);					// should return 1 item
				matches = tree.Search("zebra0080", KeywordMatchMode.ExactMatch, false);					// should return 1 item
				matches = tree.Search("semi-professional", KeywordMatchMode.Contains, true);	// should return 1 item


			}

		}


	}
}
