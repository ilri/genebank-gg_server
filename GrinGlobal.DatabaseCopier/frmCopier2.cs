using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.DatabaseInspector;
using GrinGlobal.Core;
using System.IO;
using GrinGlobal.DatabaseInspector.SqlServer;
using GrinGlobal.DatabaseInspector.Oracle;
using GrinGlobal.DatabaseInspector.MySql;
using GrinGlobal.DatabaseInspector.PostgreSql;
//using GrinGlobal.Sql;


namespace GrinGlobal.DatabaseCopier {
	public partial class frmCopier2 : Form {
		public frmCopier2() {
			InitializeComponent();
		}

		private void frmCopier2_Load(object sender, EventArgs e) {

			cboFromConnectionString.SelectedIndex = 1;
			cboToConnectionString.SelectedIndex = 1;

			_fromCSVFilename = @"C:\projects\GrinGlobal\db_scripts\new_db_schema.csv";
            _fromXMLFilename = @"C:\projects\GrinGlobal\GrinGlobal.Database.Setup\original_data\__schema.xml";
			txtToFolderName.Text = @"E:\test_db_creator\";

			_tables = new List<TableInfo>();
			syncGui();

		}

		private List<TableInfo> _tables;
		private string _fromXMLFilename;
		private string _fromCSVFilename;

		#region BackgroundWorker / GUI synchronization

		private bool _refillTables;
		private BackgroundWorker _worker;
		private delegate void BackgroundCallback(object sender, DoWorkEventArgs args);

		private void showProgress(string message, int offset, int total) {
			_worker.ReportProgress(0, new ProgressEventArgs(message, offset, total));
		}
		private void showProgress(string message) {
			_worker.ReportProgress(0, new ProgressEventArgs(message, 0, 0));
		}
		void c_OnProgress(object sender, ProgressEventArgs pea) {
			_worker.ReportProgress(0, pea);
		}
		private void backgroundProgress(object sender, ProgressChangedEventArgs e) {
			ProgressEventArgs pea = (ProgressEventArgs)e.UserState;
			if (pea.Message.StartsWith("SCHEMA:")) {
				string schema = pea.Message.Substring("SCHEMA:".Length);
				frmPreview fp = new frmPreview();
				fp.txtPreview.Text = schema;
				fp.txtPreview.SelectionStart = 0;
				fp.txtPreview.SelectionLength = 0;
				fp.Show(this);
			} else {
				statusText.Text = pea.Message;
				if (pea.TotalItems > 0) {
					statusProgress.Value = (int)(((double)pea.ItemOffset / (double)pea.TotalItems) * (double)100);
				}
			}
			//} else if (pea.Message.StartsWith("PERCENT:")) {
			//    } else if (pea.Message.StartsWith("TICK:")){
			//        updateTimer();
			//} else if (pea.Message.StartsWith("Copying data for")) {
			//    lbProgress.Items.Add(pea.Message);
			//    lbProgress.SelectedIndex = lbProgress.Items.Count - 1;
			//    _tableCount++;
			//    lblTotalProgress.Text = "Table " + _tableCount + " of " + this.lvTablesTo.CheckedItems.Count;
			//} else {
			//    lbProgress.Items.Add(pea.Message);
			//    lbProgress.SelectedIndex = lbProgress.Items.Count - 1;
			//}
		}
		private void backgroundDone(object sender, RunWorkerCompletedEventArgs e) {
			statusText.Text = "Done";
			statusProgress.Value = 100;
			if (e.Error != null) {
				string txt = "Fatal error occured: " + e.Error.Message + ".";
				MessageBox.Show(txt);
			} else {
				if (_refillTables){
					// table list changed.
					lvTableList.Items.Clear();
					foreach (TableInfo ti in _tables) {
						ListViewItem lvi = new ListViewItem(ti.TableName);
						lvi.Checked = ti.IsSelected || chkSelectAllTables.Checked;
						lvTableList.Items.Add(lvi);
					}
					if (_tables.Count > 0) {
						btnActionScriptGo.Enabled = true;
						btnActionWriteDataGo.Enabled = true;
						btnActionWriteSchemaGo.Enabled = true;
						btnPopupTableList.Enabled = true;
					} else {
						btnActionScriptGo.Enabled = false;
						btnActionWriteDataGo.Enabled = false;
						btnActionWriteSchemaGo.Enabled = false;
						btnPopupTableList.Enabled = false;
					}
				}
			}
			_refillTables = false;
			Cursor = Cursors.Default;
		}

		private void background(BackgroundCallback callback) {
			statusText.Text = "-";
			statusProgress.Value = 0;
			_worker = new BackgroundWorker();
			_worker.WorkerReportsProgress = true;
			_worker.DoWork += new DoWorkEventHandler(callback);
			_worker.ProgressChanged += new ProgressChangedEventHandler(backgroundProgress);
			_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundDone);
			_worker.RunWorkerAsync();
			Cursor = Cursors.WaitCursor;
		}

		#endregion BackgroundWorker / GUI synchronization



		private void syncGui() {
			syncFromGroupBox();
			syncToGroupBox();
			syncActionGroupBox();
		}

		private void syncFromGroupBox() {
			gbFromFile.Top = gbFromDatabase.Top;
			gbFromFile.Left = gbFromDatabase.Left;

			if (rbFromIsDatabase.Checked) {
				gbFromDatabase.Visible = true;
				gbFromFile.Visible = false;
			} else {
				gbFromDatabase.Visible = false;
				gbFromFile.Visible = true;
			}

			if (rbFromIsCSV.Checked) {
				txtFromFileName.Text = _fromCSVFilename;
			} else if (rbFromIsXml.Checked) {
				txtFromFileName.Text = _fromXMLFilename;
			}

			syncToGroupBox();
		}

		private void syncToGroupBox() {
			gbToFile.Top = gbToDatabase.Top;
			gbToFile.Left = gbToDatabase.Left;
			gbToDatabase.Visible = rbToIsDatabase.Checked;
			gbToFile.Visible = !gbToDatabase.Visible;

			chkWriteSchemaCreateTables.Enabled =
				chkWriteSchemaCreateSelfReferentialConstraints.Enabled =
				chkWriteSchemaCreateIndexes.Enabled =
				chkWriteSchemaCreateForeignConstraints.Enabled =
				rbToIsDatabase.Checked;

		}

		private void syncFromFilename() {
			if (rbFromIsCSV.Checked) {
				_fromCSVFilename = txtFromFileName.Text;
			} else if (rbFromIsXml.Checked) {
				_fromXMLFilename = txtFromFileName.Text;
			}
		}

		private void rbFromIsDatabase_CheckedChanged(object sender, EventArgs e) {
			syncFromGroupBox();
		}

		private void rbFromIsCSV_CheckedChanged(object sender, EventArgs e) {
			syncFromGroupBox();
		}

		private void rbFromIsXml_CheckedChanged(object sender, EventArgs e) {
			syncFromGroupBox();
		}

		private void btnFromFilenamePrompt_Click(object sender, EventArgs e) {
			openFileDialog1.FileName = txtFromFileName.Text;
			if (rbFromIsCSV.Checked) {
				openFileDialog1.Filter = "Comma Separated Value Files (*.csv)|*.csv|All Files (*.*)|*.*";
			} else if (rbFromIsXml.Checked) {
				openFileDialog1.Filter = "Xml Files (*.xml)|*.xml|All Files (*.*)|*.*";
			}
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				txtFromFileName.Text = openFileDialog1.FileName;
			}
			syncFromFilename();
		}

		private void chkSelectAllTables_Click(object sender, EventArgs e) {
			foreach (ListViewItem lvi in lvTableList.Items) {
				lvi.Checked = chkSelectAllTables.Checked;
			}
		}


		private void btnListTables_Click(object sender, EventArgs e) {

			Creator c = null;

			if (rbFromIsDatabase.Checked) {
				// load from database
				c = getFromCreator();
				background((sentBy, args) => {
                    List<string> tablesToLoad = null;
                    //tablesToLoad = new List<string>(new string[] { "cooperator", "sys_lang", "accession" });
					_tables = c.LoadTableInfo(txtFromDatabaseName.Text, tablesToLoad, false, int.MaxValue);
                    if (c is OracleCreator) {
                        foreach (TableInfo ti in _tables) {
                            if (String.IsNullOrEmpty(ti.SchemaName)) {
                                ti.SchemaName = txtFromDatabaseName.Text;
                            }
                        }
                    }
					_refillTables = true;

                    //// since we lazy-evaluate the ReferencesTable property (by passing 0 to the LoadTableInfo() method above),
                    //// it may change the _tables collection.  So we make a copy of it to avoid a 'can't modify enumeration while looping on it' type of exception
                    //var tempTableList = new List<TableInfo>(_tables);
                    //foreach (TableInfo ti in tempTableList) {
                    //    foreach (ConstraintInfo ci in ti.Constraints) {
                    //        MessageBox.Show(ci.ConstraintName + " references " + ci.ReferencesTable.ToString());
                    //    }
                    //}

				});
			} else if (rbFromIsCSV.Checked) {
				string csvFilename = Toolkit.ResolveFilePath(_fromCSVFilename, false);
				c = getToCreator(false);
				background((sentBy, args) => {
					// load from CSV
					_tables = c.LoadTableInfo(csvFilename, true);
                    if (c is OracleCreator) {
                        foreach (TableInfo ti in _tables) {
                            if (String.IsNullOrEmpty(ti.SchemaName)) {
                                ti.SchemaName = txtToDatabaseName.Text;
                            }
                        }
                    }
					_refillTables = true;
				});
			} else if (rbFromIsXml.Checked) {
				string xmlFilename = Toolkit.ResolveFilePath(_fromXMLFilename, false);
				string folder = new FileInfo(xmlFilename).DirectoryName;
				c = getToCreator(false);
				background((sentBy, args) => {
					// load from xml
					_tables = c.LoadTableInfo(folder);
                    if (c is OracleCreator) {
                        foreach (TableInfo ti in _tables) {
                            if (String.IsNullOrEmpty(ti.SchemaName)) {
                                ti.SchemaName = txtToDatabaseName.Text;
                            }
                        }
                    }
					_refillTables = true;
				});
			} 
		}

		private void txtFromFileName_TextChanged(object sender, EventArgs e) {
			syncFromFilename();
		}

		private void rbToIsDatabase_CheckedChanged(object sender, EventArgs e) {
			syncToGroupBox();
		}

		private void rbToIsXml_CheckedChanged(object sender, EventArgs e) {
			syncToGroupBox();
		}

		private void btnWriteSchemaGo_Click(object sender, EventArgs e) {
			MessageBox.Show("TODO: implement write schema go button click");
		}

		private void syncActionGroupBox() {

			gbActionWriteSchema.Top = gbActionScript.Top;
			gbActionWriteSchema.Left = gbActionScript.Left;

			gbActionWriteData.Top = gbActionScript.Top;
			gbActionWriteData.Left = gbActionScript.Left;

			gbActionSearch.Top = gbActionScript.Top;
			gbActionSearch.Left = gbActionScript.Left;

			if (rbActionScript.Checked) {
				gbActionScript.Visible = true;
				gbActionWriteSchema.Visible = false;
				gbActionWriteData.Visible = false;
				gbActionSearch.Visible = false;
			} else if (rbActionWriteSchema.Checked) {
				gbActionScript.Visible = false;
				gbActionWriteSchema.Visible = true;
				gbActionWriteData.Visible = false;
				gbActionSearch.Visible = false;
			} else if (rbActionWriteData.Checked) {
				gbActionScript.Visible = false;
				gbActionWriteSchema.Visible = false;
				gbActionWriteData.Visible = true;
				gbActionSearch.Visible = false;
			} else if (rbActionGenerateSearch.Checked) {
				gbActionScript.Visible = false;
				gbActionWriteSchema.Visible = false;
				gbActionWriteData.Visible = false;
				gbActionSearch.Visible = true;
			}

			syncToGroupBox();


		}

		private void rbActionScript_CheckedChanged(object sender, EventArgs e) {
			syncActionGroupBox();
		}

		private void btnActionWriteDataGo_Click(object sender, EventArgs e) {



			if (rbActionWriteDatabaseData.Checked) {
				if (rbToIsDatabase.Checked && (rbFromIsCSV.Checked || rbFromIsDatabase.Checked)) {
					MessageBox.Show("Copying data is only allowed from a database to xml file(s), or from xml file(s) to a database.\n");
					return;
				}

				Creator c = rbFromIsDatabase.Checked ? getFromCreator() : getToCreator(true);
                if (c == null) {
                    return;
                }
				// first make sure checkboxes line up with our 'selected' tables
				selectTables();

				background((sentBy, args) => {

					if (rbFromIsDatabase.Checked) {
						// copy from database to xml
						string toFolder = Toolkit.ResolveDirectoryPath(txtToFolderName.Text, true);
						c.CopyDataFromDatabase(toFolder, txtFromDatabaseName.Text, _tables);
					} else {
						// copy from xml to database
						string fromFolder = new FileInfo(Toolkit.ResolveFilePath(txtFromFileName.Text, false)).DirectoryName;
						c.CopyDataToDatabase(fromFolder, txtToDatabaseName.Text, _tables, false, false);
					}

				});
			} else if (rbActionWriteMappingData.Checked) {

                MessageBox.Show("Dataview data cannot be saved to file.  It must be written directly to a target database.");
                return;
                //Creator c = getToCreator(true);

                //int selectedCount = selectTables();
                //background((sentBy, args) => {

                //    if (rbToIsDatabase.Checked) {
                //        int i = 0;
                //        foreach (TableInfo ti in _tables) {
                //            if (ti.IsSelected) {
                //                i++;
                //                showProgress("Mapping dataview for table " + ti.TableName + "... ", i, selectedCount);
                //                c.CreateDataviewMappings(ti, _tables);
                //            }
                //        }
                //    } else {

                //        // copy sys_* table info to file



                //    }
//				});

			} else {
				throw new NotImplementedException("Change code here.  btnActionWriteDataGo_Click");
			}
		}


		private Creator getFromCreator() {
			string providerName = rbFromDatabaseMySQL.Checked ? "mysql" :
				rbFromDatabaseOracle.Checked ? "oracle" :
                rbFromDatabasePostgreSQL.Checked ? "postgresql" : 
				"sqlserver";
			DataConnectionSpec dcs = new DataConnectionSpec { ConnectionString = cboFromConnectionString.Text, ProviderName = providerName };
			Creator c = Creator.GetInstance(dcs);
			c.OnProgress += new ProgressEventHandler(c_OnProgress);
			return c;
		}

		private Creator getToCreator(bool checkObjectType) {
			string providerName = rbToDatabaseMySQL.Checked ? "mysql" :
				rbToDatabaseOracle.Checked ? "oracle" :
                rbToDatabasePostgreSQL.Checked ? "postgresql" :
				"sqlserver";
			DataConnectionSpec dcs = new DataConnectionSpec { ConnectionString = cboToConnectionString.Text, ProviderName = providerName };
			Creator c = Creator.GetInstance(dcs);
			c.OnProgress += new ProgressEventHandler(c_OnProgress);

			if (checkObjectType) {
				if (_tables != null && _tables.Count > 0) {
					// make sure the table objects we have are of the appropriate database type

					bool ok = false;
					switch (providerName.ToLower()) {
						case "mysql":
							ok = (_tables[0] is MySqlTableInfo);
							break;
						case "oracle":
                        case "oracleclient":
                        case "ora":
							ok = (_tables[0] is OracleTableInfo);
							break;
                        case "mssql":
                        case "sql":
                        case "sqlclient":
						case "sqlserver":
							ok = (_tables[0] is SqlServerTableInfo);
							break;
                        case "pgsql":
                        case "postgresql":
                            ok = (_tables[0] is PostgreSqlTableInfo);
                            break;
                        default:
                            ok = false;
                            break;
					}

					if (!ok) {
						MessageBox.Show("The table objects used to fill the list are of a different type than your destination database.\nPlease have the From: database type match the To: (even if you're pulling from CSV or XML)");
						return null;
					}
				}
			}



			return c;
		}


		private void rbActionWriteSchema_CheckedChanged(object sender, EventArgs e) {
			syncActionGroupBox();
		}

		private void rbActionWriteData_CheckedChanged(object sender, EventArgs e) {
			syncActionGroupBox();
		}

		private void btnActionScriptGo_Click(object sender, EventArgs e) {


			if ((chkScriptMigrationInserts.Checked && !rbFromIsCSV.Checked) || txtToMigrateFromDatabaseName.Text == "" || txtToMigrateToDatabaseName.Text == "") {
				MessageBox.Show("When generating migration scripts, you must specify a csv file as the input source.\nFrom and To Migration database names must also be given.");
				return;
			}


			Creator c = getToCreator(true);
			if (c == null) {
				return;
			}

            var updatedByCooperatorID = c.GetSystemCooperatorID();

			// first make sure checkboxes line up with our 'selected' tables
			selectTables();

			// make sure we import things in the proper order :-)
			_tables = TableInfoComparer.SortByDependencies(_tables);

			background((sentBy, args) => {

				bool usingMySqlIsam = chkScriptUseMyISAMDuringMigration.Checked;


				StringBuilder sb = new StringBuilder();

				Dictionary<string, string> originalEngines = new Dictionary<string, string>();

				if (usingMySqlIsam) {
					// If we use mysql and the ISAM engine during import,
					// we reorder things a bit to try to speed up the process.
					// Since MYISAM disregards constraints, we apply them before
					// importing data so we don't have to rebuild them after import.
					// Adding constraints after data exists causes a rebuild of the corresponding
					// index after each constraint is applied (so 7 indexes = 7 temp table
					// creations, meaning #rows * 7 writes * 2 tables (temp and real) total writes.

					// Then, after all the data has been written and the constraints applied,
					// we convert each table to the InnoDB engine

					// force the engine for each table to be MyISAM during import.
					// But remember the original so we can switch it to the correct one after import.
					foreach (TableInfo ti in _tables) {
						originalEngines.Add(ti.TableName, ti.Engine);
						ti.Engine = "MyISAM";
						ti.SchemaName = txtToMigrateToDatabaseName.Text;
					}
					//sb.AppendLine("/***********************************************/");
					//sb.AppendLine("/******** Turn Off Unique Checks       *********/");
					//sb.AppendLine("/***********************************************/");
					//sb.AppendLine("SET UNIQUE_CHECKS=0;");

					if (chkScriptCreateTables.Checked) {
						sb.Append(c.ScriptTables(_tables));
					}
					if (chkScriptCreateIndexes.Checked) {
						sb.Append(c.ScriptIndexes(_tables));
					}
					if (chkScriptCreateMigrationTables.Checked) {
						sb.Append(c.ScriptMigrationTables(_tables));
					}

					if (chkScriptMigrationInserts.Checked) {
						//foreach (TableInfo ti in _tables) {
						//    if (ti.IsSelected) {
						//        sb.AppendLine(String.Format("select concat(now(), ' Disabling index updates on {0}.{1} ...') as Action;", txtToMigrateToDatabaseName.Text, ti.TableName));
						//        sb.AppendLine(String.Format("ALTER TABLE {0}.{1} DISABLE KEYS;\r\n", txtToMigrateToDatabaseName.Text, ti.TableName));
						//    }
						//}
						string folder = Directory.GetParent(txtFromFileName.Text).FullName;
                        sb.Append(c.ScriptMigration(txtFromFileName.Text, txtToMigrateFromDatabaseName.Text, txtToMigrateToDatabaseName.Text, _tables, updatedByCooperatorID));
						//foreach (TableInfo ti in _tables) {
						//    if (ti.IsSelected) {
						//        string fileName = ti.SchemaName + "_" + ti.TableName + ".txt";
						//        if (ti.TableName.Contains("_citation_map")){
						//            // citation_map tables are special.
						//            // they should all load citation.
						//            sb.AppendLine(String.Format("select concat(now(), ' mass loading data from {2} into {0}.{1} ...') as Action;", txtToMigrateToDatabaseName.Text, ti.TableName, fileName));
						//            sb.AppendLine(String.Format("LOAD DATA INFILE {2} INTO TABLE {0}.{1} {3};", txtToMigrateToDatabaseName.Text, ti.TableName, fileName, Creator.INFILE_OPTIONS));
						//        } else {
						//            sb.AppendLine(String.Format("select concat(now(), ' mass loading data from {2} into {0}.{1} ...') as Action;", txtToMigrateToDatabaseName.Text, ti.TableName, fileName));
						//            sb.AppendLine(String.Format("LOAD DATA INFILE {2} INTO TABLE {0}.{1} {3};", txtToMigrateToDatabaseName.Text, ti.TableName, fileName, Creator.INFILE_OPTIONS));
						//        }
						//    }
						//}
					}

					//sb.AppendLine("/***********************************************/");
					//sb.AppendLine("/******** Turn On Unique Checks       **********/");
					//sb.AppendLine("/***********************************************/");
					//sb.AppendLine("SET UNIQUE_CHECKS=1;");



					sb.AppendLine("/***********************************************/");
					sb.AppendLine("/******** Converting tables to InnoDB **********/");
					sb.AppendLine("/***********************************************/");

					// HACK: this should be in the Creator class...

					// convert all tables to the appropriate engine
					foreach (TableInfo ti in _tables) {

						if (ti.Engine.ToLower() != originalEngines[ti.TableName].ToLower()) {
							if (ti.IsSelected) {
								ti.Engine = originalEngines[ti.TableName];

								// this will convert the table to the appropriate engine
								sb.AppendLine(c.ScriptMigrationTableChangeEngine(txtToMigrateToDatabaseName.Text.ToLower(), ti));
							}
						}

					}

					// create constraints
					sb.AppendLine("/***********************************************/");
					sb.AppendLine("/************ Adding Constraints ***************/");
					sb.AppendLine("/***********************************************/");
					foreach (TableInfo ti in _tables) {

						// and create all constraints in one pass
						if (ti.IsSelected) {
							sb.AppendLine(c.ScriptMigrationTableCreateConstraints(txtToMigrateToDatabaseName.Text.ToLower(), ti));
						}

					}


					if (chkScriptDropMigrationTables.Checked) {
						sb.Append(c.ScriptMigrationTablesDrop(txtToMigrateToDatabaseName.Text, _tables));
					}

				} else {

					foreach (TableInfo ti in _tables) {
						ti.SchemaName = txtToMigrateToDatabaseName.Text;
					}

					if (chkScriptCreateTables.Checked) {
						sb.Append(c.ScriptTables(_tables));
					}
					if (chkScriptCreateIndexes.Checked) {
						sb.Append(c.ScriptIndexes(_tables));
					}
					if (chkScriptCreateMigrationTables.Checked) {
						sb.Append(c.ScriptMigrationTables(_tables));
					}
					if (chkScriptMigrationInserts.Checked) {
						sb.Append(c.ScriptMigration(txtFromFileName.Text, txtToMigrateFromDatabaseName.Text, txtToMigrateToDatabaseName.Text, _tables, updatedByCooperatorID));
					}
					if (chkScriptCreateConstraints.Checked) {
						sb.Append(c.ScriptConstraints(_tables));
					}
					if (chkScriptDropMigrationTables.Checked) {
						sb.Append(c.ScriptMigrationTablesDrop(txtToMigrateToDatabaseName.Text, _tables));
					}
				}


				string ret = "SCHEMA: " + sb.ToString();
				showProgress(ret);
			});


		}

		private int selectTables() {
			int selectedCount = 0;
			foreach(ListViewItem lvi in lvTableList.Items){
				TableInfo ti = _tables.Find(t => {
					return t.TableName.ToLower() == lvi.Text.ToLower();
				});
				if (ti != null){
					ti.IsSelected = lvi.Checked;
					if (ti.IsSelected) {
						selectedCount++;
					}
				}
			}
			return selectedCount;
		}

		private void btnActionWriteSchemaGo_Click(object sender, EventArgs e) {
			Creator c = getToCreator(true);

            if (c == null) {
                return;
            }

			// first make sure checkboxes line up with our 'selected' tables
			selectTables();

            var schemaName = txtToDatabaseName.Text;

			background((sentBy, args) => {
				if (rbToIsDatabase.Checked) {

                    try {
                        var output = c.DropDatabase(_tables[0].SchemaName, "SQLExpress");
                    } catch {
                        // eat all errors with dropping a database
                    }

                    try {
                        var output = c.CreateDatabase(_tables[0].SchemaName, "SQLExpress");
                    } catch (Exception ex) {
                        MessageBox.Show("Error creating database " + _tables[0].SchemaName + ": " + ex.Message);
                        throw;
                    }

					if (chkWriteSchemaCreateTables.Checked) {
						c.CreateTables(_tables, schemaName);
					}
					if (chkWriteSchemaCreateIndexes.Checked) {
						c.CreateIndexes(_tables);
					}
					if (chkWriteSchemaCreateForeignConstraints.Checked || chkWriteSchemaCreateSelfReferentialConstraints.Checked) {
						c.CreateConstraints(_tables, chkWriteSchemaCreateForeignConstraints.Checked, chkWriteSchemaCreateSelfReferentialConstraints.Checked);
					}
				} else {
					string fileName = Toolkit.ResolveFilePath(txtToFolderName.Text + @"\__schema.xml", true);
					c.SaveTableInfoToXml(fileName, _tables);
				}
			});
		}

		private void chkScriptUseMyISAMEngine_CheckedChanged(object sender, EventArgs e) {

		}

		private void rbActionGenerateSearch_CheckedChanged(object sender, EventArgs e) {
			syncActionGroupBox();
		}

		private void btnActionSearchGo_Click(object sender, EventArgs e) {

			Creator c = getToCreator(true);
			if (c == null) {
				return;
			}

			// first make sure checkboxes line up with our 'selected' tables
			selectTables();

			DataConnectionSpec dcs = new DataConnectionSpec { ConnectionString = cboActionGenerateSearch.Text, ProviderName = "MySQL.Data" };
			
			background((sentBy, args) => {


				string output = c.GenerateIndexSelectFromTableInfo(_tables, dcs);
				StringBuilder sb = new StringBuilder();
				foreach (TableInfo ti in _tables) {
					string pkName = ti.PrimaryKey == null ? null : ti.PrimaryKey.Name.ToLower();
					if (!String.IsNullOrEmpty(pkName)) {
						// not all tables are indexed for searching -- only certain ones.
						// for ones that are not, just add a comment
						try {
//							sb.AppendLine(ti.GenerateIndexSelectScript(sit));
						} catch {
							sb.AppendLine("\r\n####### Table '" + ti.TableName + "' is not indexed for searching ######\r\n");
						}
					}
				}

				showProgress("SCHEMA:" + output);
			});


		}

		private void btnPopupTableList_Click(object sender, EventArgs e) {

			// HACK: just show the table names in a popup editor
			background((sentBy, args) => {
				StringBuilder sb = new StringBuilder();
				foreach (TableInfo ti in _tables) {
					sb.AppendLine(ti.TableName);
				}
				showProgress("SCHEMA:" + sb.ToString());
			});



		}

		private void btnToFolderNamePrompt_Click(object sender, EventArgs e) {
			folderBrowserDialog1.ShowNewFolderButton = true;
			if (Directory.Exists(txtToFolderName.Text)) {
				folderBrowserDialog1.SelectedPath = txtToFolderName.Text;
			}
			if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK){
				txtToFolderName.Text = folderBrowserDialog1.SelectedPath;
			}
		}

        private void rbFromDatabaseSQLServer_CheckedChanged(object sender, EventArgs e) {
            cboFromConnectionString.SelectedIndex = 1;
        }

        private void rbFromDatabasePostgreSQL_CheckedChanged(object sender, EventArgs e) {
            cboFromConnectionString.SelectedIndex = 3;

        }

        private void rbFromDatabaseOracle_CheckedChanged(object sender, EventArgs e) {
            cboFromConnectionString.SelectedIndex = 5;

        }
        private void rbFromDatabaseMySQL_CheckedChanged(object sender, EventArgs e) {
            cboFromConnectionString.SelectedIndex = 7;
        }

        private void rbToDatabaseSQLServer_CheckedChanged(object sender, EventArgs e) {
            cboToConnectionString.SelectedIndex = 1;
        }

        private void rbToDatabasePostgreSQL_CheckedChanged(object sender, EventArgs e) {
            cboToConnectionString.SelectedIndex = 3;

        }

        private void rbToDatabaseOracle_CheckedChanged(object sender, EventArgs e) {
            cboToConnectionString.SelectedIndex = 5;

        }

        private void rbToDatabaseMySQL_CheckedChanged(object sender, EventArgs e) {
            cboToConnectionString.SelectedIndex = 7;

        }

        private void button1_Click(object sender, EventArgs e) {
            var sb = new StringBuilder();
            var c = getFromCreator();
            foreach (var t in _tables) {
                sb.AppendLine("/***************************************************************/");
                sb.AppendLine("/****************     <" + t.TableName + ">    *****************/");
                sb.AppendLine("/***************************************************************/");
                sb.AppendLine("insert into GRINGLOBAL." + t.TableName + " (");
                var sbSelect = new StringBuilder();
                foreach (var f in t.Fields) {
                    sb.Append(f.Name + ", ");
                    sbSelect.Append(f.Name + ", ");
                }
                sb.Length -= 2;
                sbSelect.Length -= 2;
                sb.AppendLine(")").AppendLine("   select " + sbSelect.ToString()).AppendLine(" from PROD." + t.TableName + ")");
                sb.AppendLine("/");
                sb.AppendLine();
                sb.AppendLine();
            }
            var form = new frmPreview();
            form.txtPreview.Text = sb.ToString();
            form.ShowDialog(this);
        }


	}
}

/*

----- DEV -----
Data Source=mw25pi-grin-t1.visitor.iastate.edu;Database=dev2;user id=;password=
Data Source=mw25pi-grin-t1.visitor.iastate.edu, 1435;Database=devtestsql;User Id=;password=
Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(PORT=1521)(HOST=129.186.187.178))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=;password=
----- BETA -----
Data Source=mw25pi-grin-t1.visitor.iastate.edu;Database=dev3;user id=root;password=
----- PRODUCTION -----
Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.100.146.29)(PORT=1521))(CONNECT_DATA=(SID=NPGS)));User Id=;Password=
Driver={Microsoft ODBC for Oracle};Server=npgs.ars-grin.gov;Uid=;Pwd=
*/