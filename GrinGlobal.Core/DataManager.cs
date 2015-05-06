using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Diagnostics;

using GrinGlobal.Core.DataManagers;
using System.Text.RegularExpressions;

namespace GrinGlobal.Core {

	public enum DataVendor {
		Unknown,
		SqlServer,
		Oracle,
		MySql,
		PostgreSql,
		OleDb,
		ODBC,
        Sqlite
//		Sybase,
	}


	/// <summary>
	/// One-liner database agnostic sql and stored proc execution
	/// </summary>
	[ComVisible(false)]
//#if !DEBUGDATAMANAGER
//    [DebuggerStepThrough()]
//#endif
    public abstract class DataManager : IDisposable {

        #region Constructors / Factory

		/// <summary>
		/// Returns the Vendor for the default config setting ("DataManager")
		/// </summary>
		/// <returns></returns>
		public static DataVendor GetVendor() {
			return DataManager.GetVendor(DEFAULT_NAME);
		}

		/// <summary>
		/// Returns the Vendor for the given config setting name
		/// </summary>
		/// <returns></returns>
		public static DataVendor GetVendor(string connectionStringConfigSettingName) {
			using (DataManager dm = DataManager.Create(connectionStringConfigSettingName)) {
				return dm.DataConnectionSpec.Vendor;
			}
		}

		public static string GetProviderName(DataConnectionSpec dcs) {
			if (!String.IsNullOrEmpty(dcs.ProviderName)) {
				return dcs.ProviderName;
			} else {
				if (dcs.Vendor == DataVendor.Unknown) {
					// we don't know provider name or vendor.
					// just create one and pull its vendor name to kick back the provider name :)
					using (DataManager dm = DataManager.Create(dcs)) {
						dcs.Vendor = dm.DataConnectionSpec.Vendor;
					}
				}
				switch (dcs.Vendor) {
					case DataVendor.SqlServer:
						dcs.ProviderName = "sqlserver";
						break;
					case DataVendor.MySql:
						dcs.ProviderName = "mysql";
						break;
					case DataVendor.Unknown:
					default:
						dcs.ProviderName = "oledb";
						break;
					case DataVendor.Oracle:
						dcs.ProviderName = "oracle";
						break;
					case DataVendor.PostgreSql:
						dcs.ProviderName = "postgresql";
						break;
				}
			}
			return dcs.ProviderName;
		}

		/// <summary>
		/// Returns the Vendor for the given DataConnectionSpec
		/// </summary>
		/// <returns></returns>
        public static DataVendor GetVendor(DataConnectionSpec dcs) {
            if (dcs == null) {
                throw new InvalidOperationException(getDisplayMember("GetVendor", "DataConnectionSpec must be provided when calling GetVendor(DataConnectionSpec)"));
            } else if (dcs.Vendor == DataVendor.Unknown) {
                using (DataManager dm = DataManager.Create(dcs)) {
                    dcs.Vendor = dm.DataConnectionSpec.Vendor;
                }
            }
            return dcs.Vendor;
        }

		/// <summary>
		/// Creates a DataManager object based on the "DataManager" key stored in the connectionStrings section in the app's config file.
		/// </summary>
		/// <returns></returns>
		public static DataManager Create() {
			return Create(DEFAULT_NAME);
		}

        /// <summary>
        /// Creates a DataManager object based on the "DataManagerWeb" key stored in the connectionStrings section in the app's config file.
        /// </summary>
        /// <returns></returns>
        public static DataManager CreateWeb()
        {
            return Create(DEFAULT_NAME_WEB);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dcs"></param>
		/// <returns></returns>
		public static DataManager Create(DataConnectionSpec dcs) {
            if (dcs == null) {
                return Create();
            } else {
                DataManager dm = __create(dcs);
                return dm;
            }
		}

        /// <summary>
        /// Simply verify connection to the database by doing a dummy select ("select 1")
        /// </summary>
        /// <returns></returns>
        public virtual string TestLogin() {
            try {
                ReadValue("select 1").ToString();
                return null;
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                return ex.Message;
            }
        }

		/// <summary>
		/// If given parent is null, creates a new DataManager object based on the "DataManager" key stored in the connectionStrings section of the app's config file.
		/// If given parent is not null, simply returns the parent.
		/// </summary>
		/// <param param name="parent">DataManager to inherit a transaction context from, if any</param>
		/// <returns></returns>
		public static DataManager Create(DataManager parent) {

            string settingsName = DEFAULT_NAME;
            if (parent != null && parent.DataConnectionSpec != null && !String.IsNullOrEmpty(parent.DataConnectionSpec.ConnectionStringSettingName)) {
                settingsName = parent.DataConnectionSpec.ConnectionStringSettingName;
            }

            return Create(new DataConnectionSpec {
                ConnectionStringSettingName = settingsName,
                Parent = parent
            });
		}

		/// <summary>
		/// Creates a DataManager object based on the connection string stored in the app's config file under the given connectionStringConfigSettingsName.
		/// </summary>
		/// <param name="connectionStringConfigSettingName">Name of setting in connectionStrings section of the config file from which to pull connection string information.</param>
		/// <returns></returns>
		public static DataManager Create(string connectionStringConfigSettingName) {
			return Create(new DataConnectionSpec { ConnectionStringSettingName = connectionStringConfigSettingName });
		}

		/// <summary>
		/// Creates a DataManager object based on the connection string stored in the app's config file under the given connectionStringConfigSettingsName.
		/// </summary>
		/// <param name="connectionStringConfigSettingName">Name of setting in connectionStrings section of the config file from which to pull connection string information.</param>
		/// <param name="parent"></param>
		/// <returns></returns>
		public static DataManager Create(string connectionStringConfigSettingName, DataManager parent) {
			return Create(new DataConnectionSpec { ConnectionStringSettingName = connectionStringConfigSettingName, Parent = parent });
		}

        private static DataManager __create(DataConnectionSpec dcs) {
            DataManager dm = null;

            string csName = (String.IsNullOrEmpty(dcs.ConnectionStringSettingName) ? DEFAULT_NAME : dcs.ConnectionStringSettingName);

            if (dcs.Parent == null) {
                // no parent specified, just create it as the config setting says to
                dm = __createNewInstance(dcs);
                if (dcs.CommandTimeout == null) {
                    dcs.CommandTimeout = Toolkit.GetSetting(csName + "CommandTimeout", 30);
                }
            } else {
                if (dcs.Parent.DataConnectionSpec.ConnectionString == dcs.ConnectionString) {
                    // parent is exact same connection as the one we would use.
                    // just piggyback on it.
                    // NOTE: totally ignores username/password!
                    if (dcs.Parent.IsInTran) {
                        dcs.Parent.BeginTran();
                    }
                    return dcs.Parent;
                } else {
                    // parent is different, so they must have passed it to get transaction support.
                    // associate the new object with the parent
                    dm = __createNewInstance(dcs);
                    if (dcs.CommandTimeout == null) {
                        dcs.CommandTimeout = Toolkit.GetSetting(csName + "CommandTimeout", 30);
                    }
                    dcs.Parent.addChild(dm);
                    if (dcs.Parent.IsInTran) {
                        // TODO: is this a good decision? auto-enlisting a child transaction if a parent has one?
                        //       yeah, sure why not -- as long as we also auto-commit
                        dm.BeginTran(true);
                    }
                }
            }
            //dcs.UserName = userName;
            //dcs.Password = password;
            return dm;
        }

        private static DataManager __createNewInstance(DataConnectionSpec dcs){

            if (dcs == null) {
                throw new InvalidOperationException(getDisplayMember("__createNewInstance", "No DataConnectionSpec provided.  You must either explicitly set the DataConnectionSpec property or provide the ConnectionStringConfigSettingName from which to create one."));
            }

            
            // A minimal DataConnectionSpec has at least one of the following:
            // * ConnectionStringSettingName is set AND config file has relevant info
            // * ConnectionString AND ProviderName are set
            // * ConnectionString AND Vendor are set



            if (!String.IsNullOrEmpty(dcs.ConnectionStringSettingName)) {
                // pull info from connection string setting
                ConnectionStringSettings css = initConnString(dcs.ConnectionStringSettingName);
                if (css == null) {
                    throw new InvalidOperationException(getDisplayMember("__createNewInstance{css}", "The connectionstring setting '{0}' is not specified in the connectionStrings section of the configuration file.", dcs.ConnectionStringSettingName));
                }
                if (String.IsNullOrEmpty(dcs.ConnectionString)) {
                    dcs.ConnectionString = css.ConnectionString;
                }
                if (String.IsNullOrEmpty(dcs.ProviderName)) {
                    dcs.ProviderName = css.ProviderName;
                }
                if (String.IsNullOrEmpty(dcs.Name)) {
                    dcs.Name = css.Name;
                }
            }

            if (String.IsNullOrEmpty(dcs.ProviderName)) {
                throw new InvalidOperationException(getDisplayMember("__creatNewInstance{provider}", "No DataConnectionSpec.ProviderName is specified or could be derived from given values.  Minimal values are: (1) ConnectionStringSettingName AND config file connectionString, (2) ConnectionString AND ProviderName, (3) ConnectionString AND Vendor.  Valid values for ProviderName are: sqlserver, mysql, oracle, postgresql, oledb, odbc."));
            } 


            switch (dcs.ProviderName.ToLower()) {
                case "system.data.oledb":
                case "oledb":
                case "ole":
                    return new OleDbDataManager(dcs);
                case "system.data.odbc":
                case "odbc":
                    return new OdbcDataManager(dcs);
                case "system.data.sqlclient":
                case "mssql":
                case "sql":
                case "sqlserver":
                case "sqlclient":
                    return new SqlDataManager(dcs);
                case "system.data.oracleclient":
                case "oracle":
                case "oracleclient":
                case "ora":
                    return new OracleDataManager(dcs);

                default:
                    // try to load using DynamicDataManager
                    return new DynamicDataManager(dcs);

            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="css"></param>
		protected internal DataManager(DataConnectionSpec dcs, string userNameMoniker, string passwordMoniker, DataVendor vendor) {
            dcs.Vendor = vendor;
			dcs.UserNameMoniker = userNameMoniker;
			dcs.PasswordMoniker = passwordMoniker;
            DataConnectionSpec = dcs;

            // HACK: reassign the connectionstring property since UserNameMoniker and PasswordMoniker are now set
            //       this will cause the username/password to be properly parsed out.
            dcs.ConnectionString = dcs.ConnectionString;

            mungeConnectionSpec(dcs);
            _children = new List<DataManager>();
        }
        #endregion



        //public static DataConnectionSpec CreateDataConnectionSpec(string providerName, string serverName, string databaseName, string userName, string password, bool useWindowsAuthentication) {

        //    // fill a DCS far enough so it can determine which instance of datamanger to create
        //    var dcs = new DataConnectionSpec();
        //    dcs.ProviderName = providerName;
        //    dcs.UserName = userName;
        //    dcs.Password = password;
        //    dcs.UseWindowsAuthentication = useWindowsAuthentication;

        //    using (var dm = DataManager.__createNewInstance(dcs)) {
        //        // then tell that instance to fill the connectionstring
        //        dm.initConnectionString(dcs, serverName, databaseName);
        //    }
        //    return dcs;

        //}


        #region Properties / Abstracts

		private const string DEFAULT_NAME = "DataManager";
        private const string DEFAULT_NAME_WEB = "DataManagerWeb";

 		/// <summary>
		/// 
		/// </summary>
		protected internal IDbConnection _conn;

		private int _offset;
		public int Offset {
			get {
				return _offset;
			}
			set { _offset = value; }
		}

		private int _limit;
		public int Limit {
			get {
				return _limit;
			}
			set {
				_limit = value;
			}
		}

		public DataConnectionSpec DataConnectionSpec { get; private set; }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="userNameMoniker"></param>
		/// <param name="passwordMoniker"></param>
		protected internal void setMonikers(string userNameMoniker, string passwordMoniker) {
            DataConnectionSpec.UserNameMoniker = userNameMoniker;
            DataConnectionSpec.PasswordMoniker = passwordMoniker;
		}


        protected int findMatchingParen(List<string> list, int initialOffset) {
            int parenCounter = 0;
            int i = initialOffset;
            if (i < list.Count && list[i] == "(") {
                while (i < list.Count) {
                    if (list[i] == "(") {
                        parenCounter++;
                    } else if (list[i] == ")") {
                        parenCounter--;
                        if (parenCounter == 0) {
                            break;
                        }
                    }
                    i++;
                }
            }

            if (i < list.Count) {
                return i;
            } else {
                return -1;
            }
        }



		/// <summary>
		/// 
		/// </summary>
		/// <param name="connString"></param>
		/// <returns></returns>
		protected internal abstract IDbConnection createConn(string connString);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		protected internal abstract IDbCommand createCommand(string text);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		protected internal abstract IDbDataParameter createParam(IDbCommand cmd, string name, object val, DbType dbType);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
        protected internal abstract DbDataAdapter createAdapter(IDbCommand cmd);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		protected internal abstract void enableReturnId(IDbCommand cmd, string nameOfSequenceOrPrimaryKey, ref DataParameters dps);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		protected internal abstract void mungeSql(IDbCommand cmd);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="prm"></param>
		protected internal abstract void mungeParameter(IDbCommand cmd, IDbDataParameter prm);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		protected internal abstract object executeInsert(IDbCommand cmd);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="limit"></param>
		protected internal abstract void applyLimit(IDbCommand cmd, int limit, int offset);

		/// <summary>
		/// Use this as a cross-db safe way of concatenating field names and hard coded values within a sql statement. Think "field1 + ' - ' + field2 as summary" in sql server.
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public abstract string Concatenate(string[] values);

		/// <summary>
		/// Use this as a way to expand a list of integers into an IN() clause for the given parameterName.  Note the list is not submitted as a parameter, but rather as part of the sql statement itself (which is why we require it to be an integer list -- no sql injection possible)
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="parameterName"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public static string ExpandIntegerList(string sql, string parameterName, IEnumerable<int> values) {
			StringBuilder sb = new StringBuilder();
			if (values != null) {
				foreach (int val in values) {
					sb.Append(val.ToString())
						.Append(", ");
				}
                if (sb.Length > 2) {
                    sb.Length -= 2;
                }
			}
			if (sb.Length == 0) {
				sb.Append("NULL");
			}
			string newsql = sql.Replace(parameterName, sb.ToString());
			return newsql;
		}

        #endregion

        #region Inits

		private static ConnectionStringSettings initConnString(string connectionStringConfigSettingName) {
            return System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringConfigSettingName];
        }

        //protected abstract void initConnectionString(DataConnectionSpec dcs, string serverName, string databaseName);

        protected abstract void mungeConnectionSpec(DataConnectionSpec dcs);

        protected string replaceNameValuePair(string source, string name, string value) {
            string ret = source;
            if (!String.IsNullOrEmpty(value)) {
                if (!String.IsNullOrEmpty(name)) {
                    ret = Toolkit.RemoveNameValuePair(ret, name);
                    if (!ret.EndsWith(";")) {
                        ret += ";";
                    }
                    ret += name + "=" + value + ";";
                }
            }
            return ret;
        }

        private IDbConnection initConn() {
            if (_conn == null) {
				string connString = DataConnectionSpec.ConnectionString;
                connString = replaceNameValuePair(connString, DataConnectionSpec.UserNameMoniker, DataConnectionSpec.UserName);
                connString = replaceNameValuePair(connString, DataConnectionSpec.PasswordMoniker, DataConnectionSpec.Password);
                _conn = createConn(connString);
            }
            if (_conn.State != ConnectionState.Open) {
                _conn.Open();
            }
            return _conn;
        }

		private IDbCommand initCommand(DataCommand cmd, bool returnId, string nameOfSequenceOrPrimaryKey) {

			// first thing we do is trim it and yank off any trailing ;
			if (!String.IsNullOrEmpty(cmd.ProcOrSql)) {
				cmd.ProcOrSql = cmd.ProcOrSql.Trim();
				if (cmd.ProcOrSql.EndsWith(";")) {
					cmd.ProcOrSql = Toolkit.Cut(cmd.ProcOrSql, 0, -1);
				}
			}



			if (cmd.NativeCommand == null || cmd.ProcOrSql != cmd.NativeCommand.CommandText) {
				cmd.NativeCommand = createCommand(cmd.ProcOrSql);
				cmd.NativeCommand.Parameters.Clear();

				// proc names usually don't have spaces, valid sql must have spaces.
				// so if we find whitespace and first char isn't '[', we assume it's sql.  If we don't we assume it's a proc.
				if ((cmd.NativeCommand.CommandText.Contains(" ") || cmd.NativeCommand.CommandText.Contains("\t") || cmd.NativeCommand.CommandText.Contains("\n")) && !cmd.NativeCommand.CommandText.StartsWith("[")) {
					cmd.NativeCommand.CommandType = CommandType.Text;
				} else {
					cmd.NativeCommand.CommandType = CommandType.StoredProcedure;
				}

				// allow them to override the command timeout
                string csName = String.IsNullOrEmpty(DataConnectionSpec.ConnectionStringSettingName) ? DEFAULT_NAME : DataConnectionSpec.ConnectionStringSettingName;
                cmd.NativeCommand.CommandTimeout = Toolkit.ToInt32(DataConnectionSpec.CommandTimeout, Toolkit.GetSetting(csName + "CommandTimeout", 30));

			}

			if (cmd.Timeout != null) {
				cmd.NativeCommand.CommandTimeout = (int)cmd.Timeout;
			}


			if (returnId) {

				// ok, this is where things get really hairy...
				// Insert into (and returning auto-generated id) varies widely based on the DBMS.

				// MS SQL Server expects:
				// insert into employee (name) values ('Brock'); select scope_identity();

				// MySQL expects:
				// insert into employee (name) values ('Brock'); select last_insert_id();

				// Oracle expects:
				// insert into employee (name) values ('Brock') returning emp_id into Return_val;

				// PostgreSQL expects:
				// insert into employee (name) values ('Brock') returning emp_id;
				//  OR
				// insert into employee (emp_id, name) values (DEFAULT, 'Brock') returning emp_id;

				// To make this work properly, we're going to assume some things:
				// 1) Caller always passes us the insert statement w/o an ending semicolon (which is invalid on some DBMS's)
				// 2) Caller uses the simplest insert statement available -- does not specify any DEFAULT or NEXT_VAL or anything.
				// 3) Caller always explicitly lists all fields that are to be updated.  Some DBMS's do not work properly otherwise.

				DataParameters dps = cmd.DataParameters;
				enableReturnId(cmd.NativeCommand, nameOfSequenceOrPrimaryKey, ref dps);

			}

            // Munge sql to account for differences between various DBMS's
            // basically, this allows us to use CONCAT, TRIM, etc regardless of DBMS
            mungeSql(cmd.NativeCommand);
            
            
            // apply the limit only if they're doing a select and they specified a valid limit
			// we can't do it on stored procs or insert / update.
			if ((_limit > 0 || _offset > 0) && cmd.NativeCommand.CommandText.Trim().ToUpper().StartsWith("SELECT")) {
				applyLimit(cmd.NativeCommand, _limit, _offset);
			}



			if (cmd.DataParameters != null && cmd.DataParameters.Count > 0){
				if (cmd.NativeCommand.Parameters != null && cmd.NativeCommand.Parameters.Count == 0) {
					// first time through, create and append
					foreach (DataParameter dp in cmd.DataParameters) {

                        if (dp.DbType == DbType.Xml) {
                            if (dp.DbPseudoType == DbPseudoType.IntegerCollection) {
                                // do not add as a parameter, replace param key with int collection value (i.e. "1,2,3,4")
                                int[] val = null;
                                if (dp.Value == null || dp.Value == DBNull.Value) {
                                    val = new int[0];
                                } else if (dp.Value is int[]) {
                                    val = dp.Value as int[];
                                } else if (dp.Value is List<int>) {
                                    val = (dp.Value as List<int>).ToArray();
                                } else if (dp.Value is string) {
                                    val = Toolkit.ToIntList(dp.Value as string).ToArray();
                                }
                                cmd.NativeCommand.CommandText = cmd.NativeCommand.CommandText.Replace(dp.Key, Toolkit.Join(val, ",", "-1", null, null));
                                continue;
                            } else if (dp.DbPseudoType == DbPseudoType.DecimalCollection) {
                                // do not add as a parameter, replace param key with int collection value (i.e. "1,2,3,4")
                                decimal[] val = null;
                                if (dp.Value == null || dp.Value == DBNull.Value) {
                                    val = new decimal[0];
                                } else if (dp.Value is decimal[]) {
                                    val = dp.Value as decimal[];
                                } else if (dp.Value is List<decimal>) {
                                    val = (dp.Value as List<decimal>).ToArray();
                                } else if (dp.Value is string) {
                                    val = Toolkit.ToDecimalList(dp.Value as string).ToArray();
                                }
                                cmd.NativeCommand.CommandText = cmd.NativeCommand.CommandText.Replace(dp.Key, Toolkit.Join(val, ",", "0", null, null));
                                continue;
                            } else if (dp.DbPseudoType == DbPseudoType.StringReplacement) {
                                // do not add as a parameter, replace param key with string value
                                string val = null;
                                if (dp.Value == null) {
                                    val = "";
                                } else {
                                    val = dp.Value.ToString();
                                }
                                
                                // prevent an entire class of sql injection attacks by forcing no comments...
                                val = val.Replace("--", "").Replace("/*", "").Replace("*/", "");

                                cmd.NativeCommand.CommandText = cmd.NativeCommand.CommandText.Replace(dp.Key, val);
                                continue;
                            } else if (dp.DbPseudoType == DbPseudoType.StringCollection) {
                                string[] val = null;
                                if (dp.Value == null || dp.Value == DBNull.Value) {
                                    val = new string[0];
                                } else if (dp.Value is string[]) {
                                    val = dp.Value as string[];
                                } else if (dp.Value is List<string>) {
                                    val = (dp.Value as List<string>).ToArray();
                                } else if (dp.Value is string) {
                                    // parse dp.Value into array of strings
                                    // dp.Value may look like:
                                    // * null
                                    // * ''
                                    // * HI REALLY
                                    // * Hi, Really
                                    // * 'HI', 'REALLY'
                                    // * How're, you
                                    var values = ((string)dp.Value).SplitRetain(new char[] { ' ', '\t', '\r', '\n', ',', ';' }, true, false, false);
                                    val = new string[values.Count];
                                    for(var i=0;i<values.Count;i++){
                                        val[i] = values[i];
                                        if ((val[i].StartsWith("'") && val[i].EndsWith("'")) || (val[i].StartsWith(@"""") && val[i].EndsWith(@""""))) {
                                            // remove beginning and ending delimiters (' or ")
                                            val[i] = Toolkit.Cut(val[i], 1, -1);
                                        }

                                        // HACK: does every DB engine use double &apos; to signify escaping within a string???
                                        val[i] = val[i].Replace("'", "''");
                                    }
                                }
                                if (val.Length == 0 || (val.Length == 1 && val[0].ToLower() == "null")) {
                                    cmd.NativeCommand.CommandText = cmd.NativeCommand.CommandText.Replace(dp.Key, "null");
                                } else {
                                    cmd.NativeCommand.CommandText = cmd.NativeCommand.CommandText.Replace(dp.Key, Toolkit.Join(val, "','", "", "'", "'"));
                                }
                                continue;
                            }
                        }


						IDbDataParameter prm = createParam(cmd.NativeCommand, dp.Key, dp.Value, dp.DbType);
						prm.Direction = dp.Direction;

						//// WOW be careful here...
						if (dp.Size != null) {
							prm.Size = (int)dp.Size;
						}
						if (dp.Scale != null) {
							prm.Scale = (byte)dp.Scale;
						}
						if (dp.Precision != null) {
							prm.Precision = (byte)dp.Precision;
						}

						mungeParameter(cmd.NativeCommand, prm);

						cmd.NativeCommand.Parameters.Add(prm);
					}
				} else {
					// parameters are already on the collection. just set their values.
					for(int i=0;i<cmd.NativeCommand.Parameters.Count;i++){
						((IDataParameter)cmd.NativeCommand.Parameters[i]).Value = cmd.DataParameters[i].Value;
					}
				}
			}

			if (cmd.NativeCommand.Connection == null) {

                // HACK: postgresql (namely the Npgsql driver) resets the command timeout after connection is established.
                //       so we remember it and reset it after connecting.  this should have no performance impact on others.
                int timeoutBeforeConnection = cmd.NativeCommand.CommandTimeout;

				cmd.NativeCommand.Connection = initConn();

                // HACK: see above
                cmd.NativeCommand.CommandTimeout = timeoutBeforeConnection;


				if (cmd.IsPrepared) {
					cmd.NativeCommand.Prepare();
				}
			}
			if (cmd.NativeCommand.Transaction != _tran) {
				cmd.NativeCommand.Transaction = _tran;
			}



			return cmd.NativeCommand;

		}


        #endregion

        #region Read (static)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public static DataTable ExecRead(DataCommand cmd) {
			return ExecRead(cmd.ProcOrSql, cmd.DataParameters);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public static DataTable ExecRead(DataCommand cmd, string tableName) {
			return ExecRead(cmd.ProcOrSql, tableName, cmd.DataParameters);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <returns></returns>
		public static DataTable ExecRead(string procOrSql) {
            return ExecRead(procOrSql, (DataParameters)null);
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="dps"></param>
		/// <returns></returns>
		public static DataTable ExecRead(string procOrSql, DataParameters dps) {
            using (DataManager dm = DataManager.Create()) {
                return dm.Read(procOrSql, dps);
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public static DataTable ExecRead(string procOrSql, string tableName) {
			using (DataManager dm = DataManager.Create()) {
				return dm.Read(procOrSql, tableName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="tableName"></param>
		/// <param name="dps"></param>
		/// <returns></returns>
		public static DataTable ExecRead(string procOrSql, string tableName, DataParameters dps) {
			using (DataManager dm = DataManager.Create()) {
				return dm.Read(procOrSql, tableName, dps);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="ds"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public static DataSet ExecRead(string procOrSql, DataSet ds, string tableName) {
			using (DataManager dm = DataManager.Create()) {
				return dm.Read(procOrSql, ds, tableName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="ds"></param>
		/// <param name="tableName"></param>
		/// <param name="dps"></param>
		/// <returns></returns>
		public static DataTable ExecRead(string procOrSql, DataSet ds, string tableName, DataParameters dps) {
			using (DataManager dm = DataManager.Create()) {
				return dm.Read(procOrSql, tableName, dps);
			}
		}

        #endregion

        #region Read (instance)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public DataTable Read(DataCommand cmd) {
			return Read(cmd.ProcOrSql, cmd.DataParameters);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataTable Read(DataCommand cmd, string tableName) {
			return Read(cmd.ProcOrSql, tableName, cmd.DataParameters);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <returns></returns>
		public DataTable Read(string procOrSql) {
            return Read(procOrSql, null, null, (DataParameters)null).Tables[0];
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataTable Read(string procOrSql, string tableName) {
			return Read(procOrSql, null, tableName, (DataParameters)null).Tables[tableName];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="dps"></param>
		/// <returns></returns>
		public DataTable Read(string procOrSql, DataParameters dps) {
			return Read(procOrSql, null, null, dps).Tables[0];
        }

  		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="tableName"></param>
		/// <param name="dps"></param>
		/// <returns></returns>
		public DataTable Read(string procOrSql, string tableName, DataParameters dps) {
			return Read(procOrSql, null, tableName, dps).Tables[tableName];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="ds"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataSet Read(string procOrSql, DataSet ds, string tableName) {
			return Read(procOrSql, ds, tableName, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="ds"></param>
		/// <param name="tableName"></param>
		/// <param name="dps"></param>
		/// <returns></returns>
		public DataSet Read(string procOrSql, DataSet ds, string tableName, DataParameters dps) {

			// create a new DataSet if needed
            if (ds == null) {
                ds = new DataSet();
            }

			// create a unique tableName if needed
			if (String.IsNullOrEmpty(tableName)) {
				int i = ds.Tables.Count;
				tableName = "Table" + i;
				while (ds.Tables.Contains(tableName) && i < 1000) {
					tableName = "Table" + (++i);
				}
			}

			// ... and finally pull the data and shove it into the appropriate DataTable in our DataSet
            var cmd = new DataCommand(procOrSql, dps);
			using (IDbCommand icmd = initCommand(cmd, false, null)) {
                DbDataAdapter adpt = createAdapter(icmd);
#if SHOW_SQL
                Debug.WriteLine(cmd.NativeCommand.CommandText + " -- " + (dps == null ? "" : dps.ToString()));
#endif
				try {
					adpt.Fill(ds, tableName);
				} catch (Exception e) {
					addSqlInfo(e, cmd);
					throw;
				}
            }

            return ds;
        }

        #endregion

		private void addSqlInfo(Exception e, DataCommand cmd) {
			if (e != null && e.Data != null && cmd != null && !e.Data.Contains("SqlStatement")) {
                var sql = cmd.NativeCommand == null ? cmd.ProcOrSql : cmd.NativeCommand.CommandText;
                if (!e.Data.Contains("SqlParameters")) {
                    var prms = (cmd.DataParameters == null ? "(None)" : cmd.DataParameters.ToString());
                    Debug.WriteLine("Error in SQL: " + sql + " -- Paramaters:" + prms);
                    e.Data.Add("SqlParameters", prms);
                } else {
                    Debug.WriteLine("Error in SQL: " + sql + " -- Parameters:(None)");
                }
				e.Data.Add("SqlStatement", sql);
			}
		}


        #region ReadValue (static)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public static object ExecReadValue(DataCommand cmd) {
			return ExecReadValue(cmd.ProcOrSql, cmd.DataParameters);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <returns></returns>
		public static object ExecReadValue(string procOrSql) {
            return ExecReadValue(procOrSql, (DataParameters)null);
        }
		///// <summary>
		///// 
		///// </summary>
		///// <param name="procOrSql"></param>
		///// <param name="dataParameters"></param>
		///// <returns></returns>
		//public static object ExecReadValue(string procOrSql, params object[] dataParameters) {
		//    return ExecReadValue(procOrSql, new DataParameters(dataParameters));
		//}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="dps"></param>
		/// <returns></returns>
		public static object ExecReadValue(string procOrSql, DataParameters dps) {
            using (DataManager dm = DataManager.Create()) {
                return dm.ReadValue(procOrSql, dps);
            }
        }

        #endregion

        #region ReadValue (instance)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public object ReadValue(DataCommand cmd) {
			return ReadValue(cmd.ProcOrSql, cmd.DataParameters);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <returns></returns>
		public object ReadValue(string procOrSql) {
            return ReadValue(procOrSql, (DataParameters)null);
        }
		///// <summary>
		///// 
		///// </summary>
		///// <param name="procOrSql"></param>
		///// <param name="dataParameters"></param>
		///// <returns></returns>
		//public object ReadValue(string procOrSql, params object[] dataParameters) {
		//    return ReadValue(procOrSql, new DataParameters(dataParameters));
		//}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="dps"></param>
		/// <returns></returns>
		public object ReadValue(string procOrSql, DataParameters dps) {
            var cmd = new DataCommand(procOrSql, dps);
            using (IDbCommand icmd = initCommand(cmd, false, null)) {
				try {
#if SHOW_SQL
                    Debug.WriteLine(cmd.NativeCommand.CommandText + " -- " + (dps == null ? "" : dps.ToString()));
#endif
                    object ret = icmd.ExecuteScalar();
					return (ret == null ? DBNull.Value : ret);
				} catch (Exception e) {
					addSqlInfo(e, cmd);
					throw;
				}
            }
        }
        #endregion


        #region Stream (instance)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public IDataReader Stream(DataCommand cmd) {
			using (IDbCommand icmd = initCommand(cmd, false, null)) {
#if SHOW_SQL
                Debug.WriteLine(icmd.CommandText + " -- " + (cmd.DataParameters == null ? "" : cmd.DataParameters.ToString()));
#endif

                try {
                    return icmd.ExecuteReader(CommandBehavior.CloseConnection);
                } catch (Exception e) {
                    addSqlInfo(e, cmd);
                    throw;
                }

			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <returns></returns>
		public IDataReader Stream(string procOrSql) {
            return Stream(procOrSql, (DataParameters)null);
        }
		///// <summary>
		///// 
		///// </summary>
		///// <param name="procOrSql"></param>
		///// <param name="dataParameters"></param>
		///// <returns></returns>
		//public IDataReader Stream(string procOrSql, params object[] dataParameters) {
		//    return Stream(procOrSql, new DataParameters(dataParameters));
		//}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="dps"></param>
		/// <returns></returns>
		public IDataReader Stream(string procOrSql, DataParameters dps) {
			return Stream(new DataCommand(procOrSql, dps));
        }
        #endregion

        #region Write (static)


		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public static int ExecWrite(DataCommand cmd) {
			return ExecWrite(cmd.ProcOrSql, cmd.DataParameters);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <returns></returns>
		public static int ExecWrite(string procOrSql) {
            return ExecWrite(procOrSql, (DataParameters)null);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="dps"></param>
		/// <returns></returns>
		public static int ExecWrite(string procOrSql, DataParameters dps) {
            using (DataManager dm = DataManager.Create()) {
                return dm.Write(new DataCommand(procOrSql, dps), false, null);
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="returnId"></param>
		/// <param name="dps"></param>
		/// <returns></returns>
		public static int ExecWrite(string procOrSql, bool returnId, string nameOfSequenceOrPrimaryKey, DataParameters dps) {
			using (DataManager dm = DataManager.Create()) {
				return dm.Write(new DataCommand(procOrSql, dps), returnId, nameOfSequenceOrPrimaryKey);
			}
		}

        #endregion

        #region Write (instance)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public int Write(DataCommand cmd) {
			return Write(cmd, false, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <returns></returns>
		public int Write(string procOrSql) {
			return Write(new DataCommand(procOrSql), false, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="dps"></param>
		/// <returns></returns>
		public int Write(string procOrSql, DataParameters dps) {
			return Write(new DataCommand(procOrSql, dps), false, null);
		}

		public int Write(DataCommand cmd, bool returnId, string nameOfSequenceOrPrimaryKeyField) {
			using (IDbCommand native = initCommand(cmd, returnId, nameOfSequenceOrPrimaryKeyField)) {
				try {
#if SHOW_SQL
                    Debug.WriteLine(cmd.NativeCommand.CommandText + " -- " + (cmd.DataParameters == null ? "" : cmd.DataParameters.ToString()));
#endif
					if (returnId) {
						object ret = executeInsert(cmd.NativeCommand);
						if (ret == DBNull.Value || ret == null) {
							throw new InvalidOperationException(getDisplayMember("Write", "Error writing to the database.  DBNull.Value was returned for the id of the record, which usually means no data was written during an insert)."));
						}
						int retVal = Convert.ToInt32(ret);
						return retVal;
					} else {
						int retVal2 = cmd.NativeCommand.ExecuteNonQuery();
						return retVal2;
					}
				} catch (Exception e) {
					addSqlInfo(e, cmd);
					throw;
				}
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="procOrSql"></param>
		/// <param name="returnId"></param>
		/// <param name="nameOfSequenceOrPrimaryKeyField"></param>
		/// <param name="dps"></param>
		/// <returns></returns>
		public int Write(string procOrSql, bool returnId, string nameOfSequenceOrPrimaryKeyField, DataParameters dps) {
			return Write(new DataCommand(procOrSql, dps), returnId, nameOfSequenceOrPrimaryKeyField);
		}
        #endregion Write (instance)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public virtual string ScrubValue(string value) {
			return value.Replace("'", @"\'").Replace(@"""", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
		}



		#region Transaction

		private DataManager _parent;
		private List<DataManager> _children;

		private void addChild(DataManager dm) {
			// this object has dm as a child, and dm's parent is this object
			dm._parent = this;
			_children.Add(dm);
		}

		/// <summary>
		/// Begins a transaction, or if a transaction is already started, increments TranDepth. If DataManager is being disposed and a transaction exists, it will be automatically rolled back.
		/// </summary>
		public void BeginTran() {
			BeginTran(false);
		}

		/// <summary>
		/// Begins a transaction, or if a transaction is already started, increments TranDepth.  If DataManger is being disposed and a transaction exists, autoCommit specifies whether to Commit() or Rollback() the transaction.
		/// </summary>
		/// <param name="autoCommit"></param>
		public void BeginTran(bool autoCommit) {
			_autoCommit = autoCommit;
			if (_conn == null) {
				initConn();
			}
			if (_tran == null){
				_tran = _conn.BeginTransaction();
			}
			_tranDepth++;
		}

		private IDbTransaction _tran;
		private int _tranDepth;
		private bool _autoCommit;

		/// <summary>
		/// Returns the number of times BeginTran() has been called on this object since the last Rollback() or Commit().
		/// </summary>
		public int TranDepth {
			get {
				return _tranDepth;
			}
		}

		/// <summary>
		/// Returns the number of times BeginTran() has been called for all active transactions in the entire stack of DataManagers since the last Rollback() or Commit().
		/// </summary>
		public int TranDepthTotal {
			get {
				int td = recursiveTranDepth(this, null);
				return td;
			}
		}

		/// <summary>
		/// Returns the number of times BeginTran() has been called for all active transactions from the root DataManager to this object since the last Rollback() or Commit().
		/// </summary>
		/// <returns></returns>
		private int TranDepthRelative {
			get {
				int td = recursiveTranDepth(this, this);
				return td;
			}
		}

		private int recursiveTranDepth(DataManager parentPointer, DataManager stopAt) {

			DataManager temp = this;
			if (parentPointer != null) {
				while (temp._parent != null) {
					temp = temp._parent;
				}
			}

			int td = _tranDepth;
			if (temp != stopAt) {
				foreach (DataManager dm in temp._children) {
					td += dm.recursiveTranDepth(null, stopAt);
				}
			}
			return td;
		}

		public bool IsInTran {
			get {
				return _tran != null;
			}
		}

		/// <summary>
		/// Rolls back a transaction immediately
		/// </summary>
		public void Rollback() {
			// roll back this object
			if (_tran != null) {
				_tran.Rollback();
				_tran.Dispose();
				_tran = null;
				_tranDepth = 0;
			}
			// roll back all children
			foreach (DataManager dm in _children) {
				if (dm.IsInTran) {
					dm.Rollback();
				}
			}
			// roll back the parent
			if (_parent != null && _parent.IsInTran) {
				_parent.Rollback();
			}
		}

		/// <summary>
		/// Commits the current transaction
		/// </summary>
		public void Commit() {
			if (_tran != null) {
				if (_tranDepth == 1) {
					if (_parent == null || _parent.TranDepth == 0) {
						// tell all children to commit, as this object
						// is the root since parent doesn't exist or has no transactions active

						// first decrement the counter so children see parent as being committed
						// (even though it's not actually committed yet)
						_tranDepth = 0;
						foreach (DataManager dm in _children) {
							dm.Commit();
						}
						// then commit ourselves
						_tran.Commit();
						_tran.Dispose();
						_tran = null;
					}
				} else {
					// we have more than one tran on this object.
					// just decrement the counter since we're faking nested transactions
					// on a single database connection anyway
					_tranDepth--;
				}
			}
		}

		private bool isOpen {
			get {
				return _conn != null && _conn.State == ConnectionState.Open;
			}
		}
		private void close() {
			if (_conn != null && _tran == null) {
				_conn.Close();
				_conn = null;
				foreach (DataManager dm in _children) {
					dm.Dispose();
				}
				if (_parent != null) {
					_parent.Dispose();
				}
			}
		}


		#endregion Transaction

		protected internal abstract void clearPool(IDbConnection conn);

		public void ClearPool() {
			if (_conn != null) {
				clearPool(_conn);
			}
		}

		#region IDisposable Members
		/// <summary>
		/// Safely closes the connection object and sets it to null
		/// </summary>
		public void Dispose() {

			// If a transaction exists, commit or rollback as needed
			if (_tran != null) {
				// either we should auto commit or a nested transaction (i.e. 2 datamanagers or more deep) should be marked as being done -- successful or not makes no difference
				// as root datamanager will determine that
				if (_autoCommit || _tranDepth > 1) {
					Commit();
				} else {
					Rollback();
				}
			}

			// close this connection and all its children, if needed
			// (i.e. we're not in a transaction
			close();

			// 2007-10-31 by not setting this to null,
			//            we let re-entrant calls (i.e. past the Dispose() call)
			//            work successfully.  think logging in a final { } clause.
//            _connStringSetting = null;
        }

        #endregion IDisposable Members

        protected static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "Core", "DataManager", resourceName, null, defaultValue, substitutes);
        }
    }
}
