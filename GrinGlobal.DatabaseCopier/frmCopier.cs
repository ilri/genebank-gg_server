using System;
using System.Windows.Forms;

using GrinGlobal.Core;
using GrinGlobal.DatabaseInspector;
using System.Collections.Generic;

using System.ComponentModel;
using System.IO;
using System.Data;
using System.Text;
using GrinGlobal.DatabaseCopier.GUIRef;
using System.Net;
using System.Web.Services.Protocols;
using System.Diagnostics;

namespace GrinGlobal.DatabaseCopier {
	public partial class frmCopier : Form {
		public frmCopier() {
			InitializeComponent();
		}

		List<string> _tableNames = new List<string>();

		int? COMMAND_TIMEOUT = 600000;

		HighPrecisionTimer _timer;
		double _prevSeconds;

		#region backgroundworker
		void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			_timer.Stop();
			updateTimer();
			_clockTimer.Stop();
			if (e.Error != null) {
				string txt = "Fatal error occured: " + e.Error.Message + ".";
				lbProgress.Items.Add(txt);
				lbProgress.SelectedIndex = lbProgress.Items.Count - 1;
				lblStatus.Text = txt;
				MessageBox.Show(txt);
			} else {
				TimeSpan ts = TimeSpan.FromSeconds(_timer.ElapsedSeconds);
				string elapsed = ts.Hours + "h " + ts.Minutes.ToString("00") + "m " + ts.Seconds.ToString("00") + "s " + ts.Milliseconds.ToString("000") + "ms ";
				lbProgress.Items.Add("Operation completed in " + elapsed);
				lbProgress.SelectedIndex = lbProgress.Items.Count - 1;
			}
			Cursor = Cursors.Default;
		}

		int _tableCount = 0;

		void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			ProgressEventArgs pea = (ProgressEventArgs)e.UserState;
			if (pea.Message.StartsWith("SCHEMA:")) {
				string schema = pea.Message.Substring("SCHEMA:".Length);
				frmPreview fp = new frmPreview();
				fp.txtPreview.Text = schema;
				fp.txtPreview.SelectionStart = 0;
				fp.txtPreview.SelectionLength = 0;
				fp.ShowDialog(this);
			} else if (pea.Message.StartsWith("PERCENT:")) {
				lblStatus.Text = pea.Message.Substring("PERCENT:".Length);
			//} else if (pea.Message.StartsWith("TICK:")){
			//    updateTimer();
			} else if (pea.Message.StartsWith("Copying data for")){
				lbProgress.Items.Add(pea.Message);
				lbProgress.SelectedIndex = lbProgress.Items.Count - 1;
				_tableCount++;
				lblTotalProgress.Text = "Table " + _tableCount + " of " + this.lvTablesTo.CheckedItems.Count;

			} else {
				lbProgress.Items.Add(pea.Message);
				lbProgress.SelectedIndex = lbProgress.Items.Count - 1;
			}
		}

		void c_OnProgress(object sender, ProgressEventArgs pea) {
			backgroundWorker1.ReportProgress(0, pea);
		}

		private void updateTimer() {
			if (_timer == null || _timer.ElapsedMilliseconds == 0) {
				lblTotalTime.Text = "00:00:00.000";
				return;
			}
			TimeSpan ts = TimeSpan.FromSeconds(_timer.ElapsedSeconds - _prevSeconds);
//			_prevSeconds = _timer.ElapsedSeconds;
			lblTotalTime.Text = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00") + "." + ts.Milliseconds.ToString("000");
		}

		Timer _clockTimer;

		private void clearWorker() {
			_tableCount = 0;
			lbProgress.Items.Clear();
			lblStatus.Text = "-";
			if (_clockTimer != null) {
				_clockTimer.Dispose();
			}
			_clockTimer = new Timer();
			_clockTimer.Interval = 1000;
			_clockTimer.Tick += new EventHandler(_clockTimer_Tick);
			_prevSeconds = 0.0;
			_clockTimer.Start();
			updateTimer();

			backgroundWorker1 = new BackgroundWorker();
			backgroundWorker1.WorkerReportsProgress = true;
			Cursor = Cursors.WaitCursor;
		}

		void _clockTimer_Tick(object sender, EventArgs e) {
			updateTimer();
		}
		#endregion

		#region From Database To Files
		private void btnCopySchemaFrom_Click(object sender, EventArgs e) {

			clearWorker();

			List<string> tableNames = new List<string>();
			for (int i=0; i < lvFromTables.Items.Count; i++) {
				ListViewItem lvi = lvFromTables.Items[i];
				if (lvi.Checked) {
					tableNames.Add(lvi.Text);
				}
			}

			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT });
			string schemaName = txtFromDatabaseOwner.Text;
			string dest = txtDestinationFolder.Text;
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt) {
				_timer = new HighPrecisionTimer("timer", true);
				c.OnProgress += new ProgressEventHandler(c_OnProgress);
				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("Getting list of tables...", 0, 0));
				List<TableInfo> tables = c.LoadTableInfo(schemaName, tableNames, true);
				c.SaveTableInfoToXml(dest, tables);
			});
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();

		}

		private string getFromProviderName() {
			if (rbFromMySql.Checked){
				return "MySql.Data";
			} else if (rbFromSqlServer.Checked){
				return "SqlServer";
			} else if (rbFromOracle.Checked) {
				return "Oracle";
			} else {
				return "sqlserver" ;
			}
		}


		private void btnCopyDataFrom_Click(object sender, EventArgs e) {

			clearWorker();

			List<string> tableNames = new List<string>();
			for (int i=0; i < lvFromTables.Items.Count; i++) {
				ListViewItem lvi = lvFromTables.Items[i];
				if (lvi.Checked) {
					tableNames.Add(lvi.Text);
				}
			}

			DataConnectionSpec dcs = new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT };
			if (dcs.ConnectionString.Contains("Driver=")) {
				// assume odbc
				dcs.ProviderName = "ODBC";
			}

			Creator c = Creator.GetInstance(dcs);
			string schemaName = txtFromDatabaseOwner.Text;
			string dest = txtDestinationFolder.Text;
			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt2) {

				_timer = new HighPrecisionTimer("timer", true);
				c.OnProgress += new ProgressEventHandler(c_OnProgress);

				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("Gathering schema information from database...", 0, 0));

				List<TableInfo> tables = c.LoadTableInfo(schemaName, tableNames, false);

				c.CopyDataFromDatabase(dest, schemaName, tables);

			});
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();
		}

		private void btnGetTableListFrom_Click(object sender, EventArgs e) {
			clearWorker();
			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT });
			string schemaName = txtFromDatabaseOwner.Text;
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt) {
				_timer = new HighPrecisionTimer("timer", true);
				c.OnProgress += new ProgressEventHandler(c_OnProgress);
				_tableNames = c.ListTableNames(schemaName);
			});

			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();
		}
		#endregion From Database To Files

		#region From Files to Database

		private string getToProviderName() {
			if (rbToMySql.Checked) {
				return "MySql.Data";
			} else if (rbToSqlServer.Checked) {
				return "SqlServer";
			} else if (rbToOracle.Checked) {
				return "Oracle";
			} else {
				return "sqlserver";
			}
		}

		private void btnCopySchemaTo_Click(object sender, EventArgs e) {

			clearWorker();

			List<string> tableNames = new List<string>();
			foreach (ListViewItem lvi in lvTablesTo.CheckedItems) {
				tableNames.Add(lvi.Text);
			}

			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt2) {
				_timer = new HighPrecisionTimer("timer", true);
				Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = txtConnectionStringTo.Text, ProviderName = getToProviderName(), CommandTimeout = COMMAND_TIMEOUT });
				c.OnProgress += new ProgressEventHandler(c_OnProgress);
				List<TableInfo> tables = c.LoadTableInfo(txtSourceFolder.Text);
				c.CreateTables(tables);
				c.CreateIndexes(tables);
				c.CreateConstraints(tables, true, true);

			});
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();

		}

		//private void btnToRefresh_Click(object sender, EventArgs e) {

		//    clearWorker();

		//    backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
		//    backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt) {
		//        _timer = new HighPrecisionTimer("timer", true);
		//        Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName() });
		//        c.OnProgress += new ProgressEventHandler(c_OnProgress);
		//        _allTables = c.ReadSchemaFromFiles(txtSourceFolder.Text);
		//    });
		//    backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
		//    backgroundWorker1.RunWorkerAsync();

		//}

		private void btnFolderTo_Click(object sender, EventArgs e) {
			folderBrowserDialog1.SelectedPath = txtDestinationFolder.Text;
			DialogResult dr = folderBrowserDialog1.ShowDialog();
			if (dr == DialogResult.OK) {
				txtDestinationFolder.Text = folderBrowserDialog1.SelectedPath;
			}

		}

		private void btnFolderFrom_Click(object sender, EventArgs e) {
			folderBrowserDialog1.SelectedPath = txtSourceFolder.Text;
			DialogResult dr = folderBrowserDialog1.ShowDialog();
			if (dr == DialogResult.OK) {
				txtSourceFolder.Text = folderBrowserDialog1.SelectedPath;
			}

		}

		private void btnCopyDataTo_Click(object sender, EventArgs e) {
			clearWorker();

			List<string> tableNames = new List<string>();
			foreach (ListViewItem lvi in lvTablesTo.CheckedItems) {
				tableNames.Add(lvi.Text);
			}


			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt) {
				_timer = new HighPrecisionTimer("timer", true);
				Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = txtConnectionStringTo.Text, ProviderName = getToProviderName(), CommandTimeout = COMMAND_TIMEOUT });
				c.OnProgress += new ProgressEventHandler(c_OnProgress);

				List<TableInfo> tables = c.LoadTableInfo(txtSourceFolder.Text);
				c.CopyDataToDatabase(txtSourceFolder.Text, null, tables);
			});
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();

		}
		#endregion From Files to Database

		private void btnListTables_Click(object sender, EventArgs e) {

			DataConnectionSpec dcs = new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT };
			if (dcs.ConnectionString.Contains("Driver=")) {
				// assume odbc
				dcs.ProviderName = "ODBC";
			}
			Creator c = Creator.GetInstance(dcs);
			lvFromTables.Items.Clear();
			List<string> names = c.ListTableNames(txtFromDatabaseOwner.Text);
			foreach (string nm in names) {
				ListViewItem lvi = new ListViewItem(nm);
				lvi.Checked = chkFromList.Checked;
				lvFromTables.Items.Add(lvi);
			}
			toggleCopyFrom(names.Count > 0);
		}

		private void toggleCopyFrom(bool enabled) {
			btnCopyDataFrom.Enabled = btnPreviewFrom.Enabled = btnCopySchemaFrom.Enabled = chkCompressOnCopy.Enabled = enabled;
		}

		private void chkFromList_CheckedChanged(object sender, EventArgs e) {
			foreach (ListViewItem lvi in lvFromTables.Items) {
				lvi.Checked = chkFromList.Checked;
			}
		}

		private void toggleCopyTo(bool enabled) {
			btnCopyDataTo.Enabled = btnPreviewTo.Enabled = btnCopySchemaTo.Enabled = enabled;
		}

		private void btnRefreshListTo_Click(object sender, EventArgs e) {
			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = txtConnectionStringTo.Text, ProviderName = getToProviderName(), CommandTimeout = COMMAND_TIMEOUT });
			lvTablesTo.Items.Clear();
			List<TableInfo> tableList = c.LoadTableInfo(txtSourceFolder.Text);
			foreach (TableInfo ti in tableList) {
				ListViewItem lvi = new ListViewItem(ti.TableName);
				lvi.Checked = chkToList.Checked;
				lvTablesTo.Items.Add(lvi);
			}
			toggleCopyTo(tableList.Count > 0);

		}

		private string getImportToProviderName() {
			if (rbImportMySql.Checked) {
				return "MySql.Data";
			} else if (rbImportSqlServer.Checked) {
				return "SqlServer";
			} else if (rbImportOracle.Checked) {
				return "Oracle";
			} else {
				return "sqlserver";
			}
		}


		private void btnLoadImport_Click(object sender, EventArgs e) {
			string[] lines = File.ReadAllLines(txtImportFromFile.Text);
			//DataTable dt = Toolkit.ParseCSV(lines);
			//dgvImportTo.DataSource = dt;

		}

		private void btnFileImport_Click(object sender, EventArgs e) {
			openFileDialog1.InitialDirectory = txtImportFromFile.Text;
			DialogResult dr = openFileDialog1.ShowDialog();
			if (dr == DialogResult.OK) {
				txtImportFromFile.Text = openFileDialog1.FileName;
			}
		}


		private void btnImportData_Click(object sender, EventArgs e) {
			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = txtImportToConnectionString.Text, ProviderName = getImportToProviderName(), CommandTimeout = COMMAND_TIMEOUT });

			// an import is really a csv parse, dump the data to a dataset in xml,
			// then look up table info and do a normal 'copy' of data

			string tgtDir = Directory.GetParent(txtImportFromFile.Text).FullName;

			DataTable dt = dgvImportTo.DataSource as DataTable;
			if (dt.DataSet == null) {
				DataSet ds = new DataSet();
				ds.Tables.Add(dt);
			}
			dt.TableName = ddlImportToTable.Text;
			dt.DataSet.WriteXml(tgtDir + "\\" + ddlImportToTable.Text + "_0.xml", XmlWriteMode.WriteSchema);


			List<string> tableNames = c.ListTableNames(null);
			List<TableInfo> tables = c.LoadTableInfo(tgtDir);
			c.CopyDataToDatabase(tgtDir, null, tables);


		}

		private void btnImportToViewTables_Click(object sender, EventArgs e) {
			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = txtImportToConnectionString.Text, ProviderName = getImportToProviderName(), CommandTimeout = COMMAND_TIMEOUT });
			ddlImportToTable.DataSource = c.ListTableNames(null);

		}

		private void chkToList_CheckedChanged(object sender, EventArgs e) {
			foreach (ListViewItem lvi in lvTablesTo.Items) {
				lvi.Checked = chkToList.Checked;
			}
		}

		private void btnPreviewFrom_Click(object sender, EventArgs e) {

			clearWorker();

			List<string> tableNames = new List<string>();
			for (int i=0; i < lvFromTables.Items.Count; i++) {
				ListViewItem lvi = lvFromTables.Items[i];
				if (lvi.Checked) {
					tableNames.Add(lvi.Text);
				}
			}


			string schemaName = txtFromDatabaseOwner.Text;
			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt) {
				_timer = new HighPrecisionTimer("timer", true);
				Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT });
				c.OnProgress += new ProgressEventHandler(c_OnProgress);
				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("Getting list of tables...", 0, 0));
				List<TableInfo> tables = c.LoadTableInfo(schemaName, tableNames, false);
				string schema = c.ScriptTables(tables);
				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("SCHEMA:" + schema, 0, 0));
			});
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();

		}

		private void btnPreviewTo_Click(object sender, EventArgs e) {
			clearWorker();

			List<string> tableNames = new List<string>();
			foreach (ListViewItem lvi in lvTablesTo.CheckedItems) {
				tableNames.Add(lvi.Text);
			}


			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt2) {
				_timer = new HighPrecisionTimer("timer", true);
				Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = txtConnectionStringTo.Text, ProviderName = getToProviderName(), CommandTimeout = COMMAND_TIMEOUT });
				c.OnProgress += new ProgressEventHandler(c_OnProgress);

				List<TableInfo> tables = c.LoadTableInfo(txtSourceFolder.Text);
				string sql = c.ScriptTables(tables);
				sql += c.ScriptIndexes(tables);
				sql += c.ScriptConstraints(tables);
				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("SCHEMA:" + sql, 0, 0));

			});
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();

		}


		private void button1_Click(object sender, EventArgs e) {

//            clearWorker();

//            List<string> tableNames = new List<string>();
//            foreach (ListViewItem lvi in lvTablesTo.CheckedItems) {
//                tableNames.Add(lvi.Text);
//            }


//            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
//            backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt2) {
//                _timer = new HighPrecisionTimer("timer", true);
//                Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = txtConnectionStringTo.Text, ProviderName = getToProviderName(), CommandTimeout = COMMAND_TIMEOUT });
//                c.OnProgress += new ProgressEventHandler(c_OnProgress);
//                List<TableInfo> tables = c.LoadTableInfo(
//                    c.SortBy(txtSourceFolder.Text, tableNames);
//                tables.Sort(new TableInfoComparer());

//                using(StreamWriter sw = new StreamWriter(new FileStream("./tables.csv", FileMode.Create, FileAccess.Write))){
//                    Toolkit.OutputCSVHeader(new string[]{"Current Table Name", "Current Field Name", "Current Field Type", "New Table Name", "New Field Name", "New Field Type", "Comments"}, sw);
//                    foreach(TableInfo ti in tables){
//                        foreach(FieldInfo fi in ti.Fields){
//                            Toolkit.OutputCSV(new string[]{ti.TableName, fi.Name, fi.FieldType, ti.TableName, fi.Name, fi.FieldType, null}, sw, false);
//                        }
//                    }
//                }

////				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("SCHEMA:" + schema, 0, 0));

//            });
//            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
//            backgroundWorker1.RunWorkerAsync();

		}

		private void btnSelect_Click(object sender, EventArgs e) {
			//clearWorker();

			//List<string> tableNames = new List<string>();
			//foreach (ListViewItem lvi in lvFromTables.CheckedItems) {
			//    tableNames.Add(lvi.Text);
			//}


			//Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT });
			//string schemaName = txtFromDatabaseOwner.Text;
			//backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			//backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt2) {
			//    _timer = new HighPrecisionTimer("timer", true);
			//    c.OnProgress += new ProgressEventHandler(c_OnProgress);

			//    string insertSelect = c.GenerateIndexSelectFromDatabase(schemaName, tableNames);
			//    backgroundWorker1.ReportProgress(0, new ProgressEventArgs("SCHEMA:" + insertSelect, 0, 0));

			//});
			//backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			//backgroundWorker1.RunWorkerAsync();

		}

		private void btnCopyIndexesAndConstraintsToDatabase_Click(object sender, EventArgs e) {
			clearWorker();

			List<string> tableNames = new List<string>();
			foreach (ListViewItem lvi in lvTablesTo.CheckedItems) {
				tableNames.Add(lvi.Text);
			}


			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = txtConnectionStringTo.Text, ProviderName = getToProviderName(), CommandTimeout = COMMAND_TIMEOUT });
			string src = txtSourceFolder.Text;
			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt2) {
				_timer = new HighPrecisionTimer("timer", true);
				c.OnProgress += new ProgressEventHandler(c_OnProgress);
				List<TableInfo> tables = c.LoadTableInfo(txtSourceFolder.Text);
				c.CreateIndexes(tables);
				c.CreateConstraints(tables,true, true);
			});
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();


		}



		private void lbProgress_DoubleClick(object sender, EventArgs e) {
			if (lbProgress.SelectedItem != null) {
				frmPreview fp = new frmPreview();
				fp.txtPreview.Text = lbProgress.SelectedItem.ToString();
				fp.ShowDialog();
			}
		}

		private void btnCopySelfConstraints_Click(object sender, EventArgs e) {
			clearWorker();

			List<string> tableNames = new List<string>();
			foreach (ListViewItem lvi in lvTablesTo.CheckedItems) {
				tableNames.Add(lvi.Text);
			}


			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = txtConnectionStringTo.Text, ProviderName = getToProviderName(), CommandTimeout = COMMAND_TIMEOUT });
			string src = txtSourceFolder.Text;
			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt2) {
				_timer = new HighPrecisionTimer("timer", true);
				c.OnProgress += new ProgressEventHandler(c_OnProgress);
				List<TableInfo> tables = c.LoadTableInfo(txtSourceFolder.Text, tableNames);
				c.CreateConstraints(tables, false, true);
			});
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();

		}

		private void btnPreviewList_Click(object sender, EventArgs e) {
			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT });
			lvFromTables.Items.Clear();
			List<string> names = c.ListTableNames(txtFromDatabaseOwner.Text);

			frmPreview fp = new frmPreview();
			fp.txtPreview.Text = "DESCRIBE " + String.Join(";\r\nDESCRIBE ", names.ToArray()) + ";";
			fp.ShowDialog(this);
		}

		private void toggleCopyFromCSV(bool enabled) {
			btnFromCsvCopySchemaToFile.Enabled = btnFromCsvPreviewSchema.Enabled = btnFromCSVMigrationInserts.Enabled = btnFromCSVPreviewSelfConstraints.Enabled = enabled;
		}

		private void btnFromCSVListTables_Click(object sender, EventArgs e) {
			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT });
			lvFromCsvTableList.Items.Clear();
			List<TableInfo> tables = c.LoadTableInfo(txtCSVFromFile.Text, true);
			foreach (TableInfo ti in tables) {
				ListViewItem lvi = new ListViewItem(ti.TableName);
				lvi.Checked = chkFromCSVSelectAll.Checked;
				lvFromCsvTableList.Items.Add(lvi);
			}
			toggleCopyFromCSV(tables.Count > 0);
		}

		private void btnCSVFromOpen_Click(object sender, EventArgs e) {
			openFileDialog1.FileName = txtCSVFromFile.Text;
			DialogResult dr = openFileDialog1.ShowDialog();
			if (dr == DialogResult.OK) {
				txtCSVFromFile.Text = openFileDialog1.FileName;
			}

		}

		private void btnFromCsvFolderPrompt_Click(object sender, EventArgs e) {
			folderBrowserDialog1.SelectedPath = txtFromCsvToFolder.Text;
			DialogResult dr = folderBrowserDialog1.ShowDialog();
			if (dr == DialogResult.OK) {
				txtFromCsvToFolder.Text = folderBrowserDialog1.SelectedPath;
			}

		}

		private void chkFromCSVSelectAll_CheckedChanged(object sender, EventArgs e) {
			foreach (ListViewItem lvi in lvFromCsvTableList.Items) {
				lvi.Checked = chkFromCSVSelectAll.Checked;
			}
		}

		private void btnFromCsvCopySchemaToFile_Click(object sender, EventArgs e) {
			clearWorker();

			List<string> tableNames = new List<string>();
			for (int i=0; i < lvFromCsvTableList.Items.Count; i++) {
				ListViewItem lvi = lvFromCsvTableList.Items[i];
				if (lvi.Checked) {
					tableNames.Add(lvi.Text);
				}
			}

			string fileName = txtFromCsvToFolder.Text + "\\__schema.xml";

			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT });
			string fromCSVFile = txtCSVFromFile.Text;
			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt) {
				_timer = new HighPrecisionTimer("timer", true);
				c.OnProgress += new ProgressEventHandler(c_OnProgress);
				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("Getting list of tables...", 0, 0));
				List<TableInfo> tables = c.LoadTableInfo(fromCSVFile, true);
				foreach (TableInfo ti in tables) {
					if (tableNames.Contains(ti.TableName)) {
						ti.IsSelected = true;
					}
				}
				c.SaveTableInfoToXml(txtFromCsvToFolder.Text, tables);
			});
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();

		}

		private void btnFromCsvPreviewSchema_Click(object sender, EventArgs e) {
			clearWorker();

			List<string> tableNames = new List<string>();
			for (int i=0; i < lvFromCsvTableList.Items.Count; i++) {
				ListViewItem lvi = lvFromCsvTableList.Items[i];
				if (lvi.Checked) {
					tableNames.Add(lvi.Text);
				}
			}

			string fileName = txtCSVFromFile.Text;
			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT });

			bool createMapTables = chkFromCSVCreateMapTables.Checked;

			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt) {
				_timer = new HighPrecisionTimer("timer", true);
				c.OnProgress += new ProgressEventHandler(c_OnProgress);
				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("Getting list of tables...", 0, 0));
				List<TableInfo> tables = c.LoadTableInfo(fileName, true);
				foreach (TableInfo ti in tables) {
					if (tableNames.Contains(ti.TableName)) {
						ti.IsSelected = true;
					}
				}
				string schema = c.ScriptTables(tables);
				if (createMapTables) {
					schema += c.ScriptMigrationTables(tables);
				}
				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("SCHEMA:" + schema, 0, 0));
			});
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();

		}

		private void btnCreateFromScratch_Click(object sender, EventArgs e) {
			GUI gui = new GUI();

			// just to test connectivity to web service
			gui.ClearCache(false, "brock", "passw0rd!", null);

			// read in huge xml file to save
			DataSet ds = new DataSet();
			ds.ReadXml(@"D:\projects\GrinGlobal\db_scripts\savebigdataset.xml");

			// and save it
			try {
//				gui.Url = "http://mw25pi-grin-t1.visitor.iastate.edu/gringlobal_mysql/gui.asmx";
				gui.Timeout = 600000;
				gui.SaveDataSet(false, "brock", "passw0rd!", ds);
			} catch (WebException we) {
				MessageBox.Show("Web Exception during save: " + we.Message);
			} catch (SoapException se) {
				MessageBox.Show("Soap Exception during save: " + se.Message);

			}

		}

//		private void createOldDbSchemaDump() {
		//    clearWorker();

		//    Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT });
		//    string dest = txtDestinationFolder.Text;
		//    string schemaName = txtFromDatabaseOwner.Text;
		//    backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
		//    backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt) {
		//        _timer = new HighPrecisionTimer("timer", true);
		//        c.OnProgress += new ProgressEventHandler(c_OnProgress);
		//        backgroundWorker1.ReportProgress(0, new ProgressEventArgs("Getting list of tables...", 0, 0));
		//        List<string> tableNames = c.ListTableNames(txtFromDatabaseOwner.Text);
		//        List<TableInfo> tables = c
					
		//            c.GetTablesFromDatabase(schemaName, tableNames, false);

		//        StringBuilder sb = new StringBuilder();
		//        foreach (TableInfo ti in tables) {
		//            foreach (FieldInfo fi in ti.Fields) {
		//                sb.Append(ti.TableName)
		//                    .Append(".")
		//                    .Append(fi.Name)
		//                    .Append("\t");
		//                if (fi.IsAutoIncrement) {
		//                    sb.Append("Auto Increment ");
		//                }
		//                if (fi.IsNullable) {
		//                    sb.Append("Nullable ");
		//                }
		//                sb.Append("\t");
		//                if (fi.DataType == typeof(string)) {
		//                    sb.Append(fi.MaxLength.ToString());
		//                }
		//                sb.AppendLine();
		//            }
		//            sb.AppendLine();
		//        }

		//        string schema = sb.ToString();

		//        backgroundWorker1.ReportProgress(0, new ProgressEventArgs("SCHEMA:" + schema, 0, 0));
		//    });
		//    backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
		//    backgroundWorker1.RunWorkerAsync();

		//}

		private void frmCopier_Load(object sender, EventArgs e) {
			cboConnectionStringFrom.SelectedIndex = 1;
			gbSourceIsFile.Top = gbSourceIsDatabase.Top;
			gbSourceIsFile.Left = gbSourceIsDatabase.Left;
			toggleSourceGroupBox();
		}

		private void btnFromCSVMigrationInserts_Click(object sender, EventArgs e) {
			clearWorker();

			List<string> tableNames = new List<string>();
			for (int i=0; i < lvFromCsvTableList.Items.Count; i++) {
				ListViewItem lvi = lvFromCsvTableList.Items[i];
				if (lvi.Checked) {
					tableNames.Add(lvi.Text);
				}
			}

			string fileName = txtCSVFromFile.Text;
			string fromDBName = txtFromCSVFromDBName.Text;
			string toDBName = txtFromCSVToDBName.Text;

			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT });
			string fromCSVFile = txtCSVFromFile.Text;
			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt) {
				_timer = new HighPrecisionTimer("timer", true);
				c.OnProgress += new ProgressEventHandler(c_OnProgress);
				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("Getting list of tables...", 0, 0));
				List<TableInfo> tables = c.LoadTableInfo(fileName, true);
				tables = c.SortByName(tables);
				c.ScriptMigration(fromCSVFile, fromDBName, toDBName, tables);
			});
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();

		}

		private void btnFromCSVPreviewSelfConstraints_Click(object sender, EventArgs e) {
			clearWorker();

			List<string> tableNames = new List<string>();
			for (int i=0; i < lvFromCsvTableList.Items.Count; i++) {
				ListViewItem lvi = lvFromCsvTableList.Items[i];
				if (lvi.Checked) {
					tableNames.Add(lvi.Text);
				}
			}

			string fileName = txtCSVFromFile.Text;
			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT });
			string toDBName = txtFromCSVToDBName.Text;

			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt) {
				_timer = new HighPrecisionTimer("timer", true);
				c.OnProgress += new ProgressEventHandler(c_OnProgress);
				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("Getting list of tables...", 0, 0));
				List<TableInfo> tables = c.LoadTableInfo(fileName, true);
				foreach (TableInfo ti in tables) {
					if (tableNames.Contains(ti.TableName)) {
						ti.IsSelected = true;
					}
				}
				string schema = c.ScriptConstraints(tables);
				schema += "\r\n\r\n" + c.ScriptMigrationTablesDrop(toDBName, tables);
				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("SCHEMA:" + schema, 0, 0));
			});
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();

		}

		private void btnCopyFromDBOracleProductionWebService_Click(object sender, EventArgs e) {

			clearWorker();
			// first get all 'old' table names we're interested in
			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT });

			string fileName = txtCSVFromFile.Text;
			string toFolder = Toolkit.ResolveDirectoryPath(txtFromCsvToFolder.Text, true);

			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt) {
				_timer = new HighPrecisionTimer("timer", true);
				c.OnProgress += new ProgressEventHandler(c_OnProgress);
				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("Getting list of tables...", 0, 0));
				List<TableInfo> tables = c.LoadTableInfo(fileName, false);


				NetworkCredential cred = new NetworkCredential("nc7bw", "sunpasswd", null);
				CredentialCache credCache = new CredentialCache();
				credCache.Add(new Uri("https://sun.ars-grin.gov:8082"), "Basic", cred);


				WebClient wc = new WebClient();
				wc.Credentials = credCache;
				foreach (TableInfo ti in tables) {
					backgroundWorker1.ReportProgress(0, new ProgressEventArgs("Downloading " + ti.TableName + "...", 0, 0));
					try {
						wc.DownloadFile("https://sun.ars-grin.gov:8082/npgs/dbmuqs.csvtab?in_owner=PROD&in_table=" + ti.TableName.ToUpper(), toFolder + @"\" + ti.TableName + ".txt");
					} catch (WebException we) {
						Debug.WriteLine(we.Message);
					}
				}
			});
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();


		}

		private void button2_Click(object sender, EventArgs e) {
			clearWorker();

			List<string> tableNames = new List<string>();
			for (int i=0; i < lvFromCsvTableList.Items.Count; i++) {
				ListViewItem lvi = lvFromCsvTableList.Items[i];
				if (lvi.Checked) {
					tableNames.Add(lvi.Text);
				}
			}

			string fileName = txtCSVFromFile.Text;
			Creator c = Creator.GetInstance(new DataConnectionSpec { ConnectionString = cboConnectionStringFrom.Text, ProviderName = getFromProviderName(), CommandTimeout = COMMAND_TIMEOUT });

			bool createMapTables = chkFromCSVCreateMapTables.Checked;

			backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
			backgroundWorker1.DoWork += new DoWorkEventHandler(delegate(object sender2, DoWorkEventArgs evt) {
				_timer = new HighPrecisionTimer("timer", true);
				c.OnProgress += new ProgressEventHandler(c_OnProgress);
				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("Getting list of tables...", 0, 0));
				List<TableInfo> tables = c.LoadTableInfo(fileName, true);
				foreach (TableInfo ti in tables) {
					if (tableNames.Contains(ti.TableName)) {
						ti.IsSelected = true;
					}
				}
				string schema = c.ScriptIndexes(tables);
				backgroundWorker1.ReportProgress(0, new ProgressEventArgs("SCHEMA:" + schema, 0, 0));
			});
			backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
			backgroundWorker1.RunWorkerAsync();

		}

		private void toggleSourceGroupBox() {
			gbSourceIsDatabase.Visible = rbSourceIsDatabase.Checked;
			gbSourceIsFile.Visible = !gbSourceIsDatabase.Visible;
		}

		private void rbSourceIsDatabase_CheckedChanged(object sender, EventArgs e) {
			toggleSourceGroupBox();
		}

		private void rbSourceIsCSV_CheckedChanged(object sender, EventArgs e) {
			toggleSourceGroupBox();

		}

		private void rbSourceIsFolder_CheckedChanged(object sender, EventArgs e) {
			toggleSourceGroupBox();

		}

		private void btnFillTableList_Click(object sender, EventArgs e) {

		}

	}
}
