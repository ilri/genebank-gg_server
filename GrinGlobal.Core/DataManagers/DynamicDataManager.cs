using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Diagnostics;
using System.Runtime;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GrinGlobal.Core.DataManagers {
#if !DEBUGDATAMANAGER
		[DebuggerStepThrough()]
#endif
	internal class DynamicDataManager : DataManager {

		private static string __text =
@"When using a DynamicDataManager (by specifying a connection string setting with a ProviderName value that 
is not 'sqlserver', 'oledb', 'odbc', or 'oracle'), you must provide the following:
--------------------------------------------------------------------------------------------------------------
connectionStrings node in config file (or DataConnectionSpec object):

ProviderName                     <filename of assembly -- must reside somewhere in a path that .NET probes
                                  see http://msdn.microsoft.com/en-us/library/yx7xezcf.aspx for probing info>
                                 Can also be the DynamicDataManager recognized values of:
                                   * mysql           -- implies MySql.Data.dll
                                   * postgresql      -- implies npgsql.dll
                                   * sqlite          -- implies System.Data.SQLite.dll
                                   * oracle3rdparty  -- (not yet implemented)
                                   * sybase          -- (not yet implemented)
                                 Defaults to 'mysql' if not specified

--------------------------------------------------------------------------------------------------------------
appSettings node in config file (optional if ProviderName is valid and defaults are valid):

appSetting key                   appSetting value
==============================   =============================================================================
<ProviderName>:MapAs              one of the following: 'mysql', 'postgresql', 'sybase', 'oracle', 'sqlite'
<ProviderName>:Connection         <fully qualified name of Connection class>
                                     mysql default:  MySQL.Data.MySqlClient.MySqlConnection
                                     pgsql default:  Npgsql.NpgsqlConnection
                                     sqlite default:  System.Data.SQLite.SQLiteConnection
                                     oracle default: <not yet implemented, use OracleDataManager>
                                     sybase default: <not yet implemented>
<ProviderName>:Command            <fully qualified name of Command class>
                                     mysql default:  MySQL.Data.MySqlClient.MySqlCommand
                                     pgsql default:  Npgsql.NpgsqlCommand
                                     sqlite default:  System.Data.SQLite.SQLiteCommand
                                     oracle default: <not yet implemented, use OracleDataManager>
                                     sybase default: <not yet implemented>
<ProviderName>:Parameter          <fully qualified name of Parameter class>
                                     mysql default:  MySQL.Data.MySqlClient.MySqlParameter
                                     pgsql default:  Npgsql.NpgsqlParameter
                                     sqlite default:  System.Data.SQLite.SQLiteParameter
                                     oracle default: <not yet implemented, use OracleDataManager>
                                     sybase default: <not yet implemented>
<ProviderName>:DataAdapter        <fully qualified name of DataAdapter class>
                                     mysql default:  MySQL.Data.MySqlClient.MySqlDataAdapter
                                     pgsql default:  Npgsql.NpgsqlDataAdapter
                                     sqlite default:  System.Data.SQLite.SQLiteDataAdapter
                                     oracle default: <not yet implemented, use OracleDataManager>
                                     sybase default: <not yet implemented>
";

		private string _mapAs;
		private string _providerName;
		private string _assemblyName;

        private static Dictionary<string, string> __validAssemblyNames = new Dictionary<string, string>();

		internal DynamicDataManager(DataConnectionSpec dcs)
			: base(dcs, null, null, DataVendor.Unknown) {

			_providerName = dcs.ProviderName;

            _mapAs = Toolkit.GetSetting(_providerName + ":MapAs", "");

            if (String.IsNullOrEmpty(_providerName) && String.IsNullOrEmpty(_mapAs)) {
                throw new InvalidOperationException(getDisplayMember("Dyn_constructor{provider}", "Both ProviderName and MapAs are empty.\n{0}", __text));
            }

            if (String.IsNullOrEmpty(_mapAs)){
                _mapAs = defaultMapAs(_providerName);
            }


            if (String.IsNullOrEmpty(_mapAs)) {
                throw new InvalidOperationException(getDisplayMember("Dyn_constructor{mapas}", "MapAs not defined properly.\n{0}", __text));
            }
            _mapAs = _mapAs.ToLower();

            if (!String.IsNullOrEmpty(_providerName)) {
                _assemblyName = _providerName.ToLower().EndsWith(".dll") ? Toolkit.Cut(_providerName, 0, -4) : _providerName;
            }

            try {
                if (!__validAssemblyNames.ContainsKey(_assemblyName)) {
                    // try to activate an instance of a command just to see if the assembly can be resolved
                    var cmd = createCommand("select * from notable");
                    // we get here, no exception.  Means the assembly was properly probed and loaded and contains a valid command class.
                    // remember that so we don't have to check it again during the lifetime of the application.
                    __validAssemblyNames.Add(_assemblyName, _assemblyName);
                } else {
                    // it has already been tested, pull the actual name from the map (in case the given _assemblyName is a synonym)
                    _assemblyName = __validAssemblyNames[_assemblyName];
                }
            } catch {
                // the one they specified wasn't a valid dll name.
                // see if it can be mapped to a valid default dll name.
                string synonym = _assemblyName;
                _assemblyName = defaultAssemblyName(_mapAs);
                try {
                    var cmd = createCommand("select * from faketable");
                    // we get here, the default name worked.
                    // map both the default and the synonym to the default.
                    __validAssemblyNames.Add(_assemblyName, _assemblyName);
                    __validAssemblyNames.Add(synonym, _assemblyName);
                } catch {
                    throw new InvalidOperationException(getDisplayMember("Dyn_constructor{dll}", "Provider name '{0}.dll' cannot be resolved to an assembly on the .net probing path.\n{1}", _providerName, __text));
                }
            }





            if (String.IsNullOrEmpty(_assemblyName)) {
                throw new InvalidOperationException(getDisplayMember("Dyn_constructor{assembly}", "ProviderName not defined properly.\n{0}", __text));
            }

			switch (_mapAs) {
				case "mysql":
				case "postgresql":
				case "oracle":
                case "sqlite":
				case "sybase":
					// MapAs is defined ok
					// set the monikers for later
					// (so we can kludge in user-specific userid and password on a per-user basis)
					string userMoniker = Toolkit.GetSetting(_providerName + ":UserNameMoniker", defaultUserNameMoniker());
					string passwordMoniker = Toolkit.GetSetting(_providerName + ":PasswordMoniker", defaultPasswordMoniker());
					setMonikers(userMoniker, passwordMoniker);
					break;
//					throw new NotImplementedException("Sybase support not yet added!!!");
				default:
                    throw new InvalidOperationException(getDisplayMember("Dyn_constructor{mapas2}", "MapAs not defined properly.\n{0}", __text));
			}

			switch (_mapAs) {
				case "mysql":
					DataConnectionSpec.Vendor = DataVendor.MySql;
					break;
                case "postgresql":
                    DataConnectionSpec.Vendor = DataVendor.PostgreSql;
					break;
                case "sqlite":
                    DataConnectionSpec.Vendor = DataVendor.Sqlite;
                    break;
				case "oracle":
                    DataConnectionSpec.Vendor = DataVendor.Oracle;
					break;
				//case "sybase":
				//    _vendor = DataVendor.Sybase;
				//    break;
			}

		}

        protected override void mungeConnectionSpec(DataConnectionSpec dcs) {
            // nothing to do yet
        }



		protected internal override void clearPool(IDbConnection conn) {
			switch (_mapAs) {
				case "mysql":
					//Assembly asm = Assembly.Load(_namespaceName + ".MySqlConnection");
					//Type mm = asm.GetType(_namespaceName + ".MySqlConnection");//class name should be added with assembly like (assembly.class)
					//mm.InvokeMember("ClearPool",BindingFlags.Default | BindingFlags.InvokeMethod, null, null, new object [] {});
					break;
                case "postgresql":
					//Assembly asm = Assembly.Load(_namespaceName + ".PgSqlConnection");
					//Type mm = asm.GetType(_namespaceName + ".MySqlConnection");//class name should be added with assembly like (assembly.class)
					//mm.InvokeMember("ClearPool", BindingFlags.Default | BindingFlags.InvokeMethod, null, null, new object[] { });
					break;
                case "sqlite":
                    // do nothing
                    break;
				case "oracle":
					//Assembly asm = Assembly.Load(_namespaceName + ".OracleConnection");
					//Type mm = asm.GetType(_namespaceName + ".OracleConnection");//class name should be added with assembly like (assembly.class)
					//mm.InvokeMember("ClearPool", BindingFlags.Default | BindingFlags.InvokeMethod, null, null, new object[] { });
					break;
				default:
					// do nothing
					break;
			}
		}

        private string defaultMapAs(string providerName){
            string pn = "";
            if (String.IsNullOrEmpty(providerName)) {
                pn = "mysql";
            } else {
                pn = providerName.ToLower();
            }

            if (pn.Contains("mysql")){
                return "mysql";
            } else if (pn.Contains("pgsql") || pn.Contains("postgre")){
                return "postgresql";
            } else if (pn.Contains("oracle") || pn.Contains("ora")){
                return "oracle";
            } else if (pn.Contains("sybase")){
                return "sybase";
            } else if (pn.Contains("sqlite")){
                return "sqlite";
            }
            return pn;
        }
        private string defaultAssemblyName(string mapAs) {
            switch (mapAs) {
                case "mysql":
                    return "MySql.Data";
                    //return "mysql.data";
                case "postgresql":
                    return "npgsql";
                case "oracle":
                    return "System.Data.OracleClient";
                case "sqlite":
                    return "System.Data.SQLite";
                default:
                    return null;
            }
        }
        private string defaultCommandClass() {
            switch (_mapAs) {
                case "mysql":
                    return "MySql.Data.MySqlClient.MySqlCommand";
                case "postgresql":
                    return "Npgsql.NpgsqlCommand";
                case "oracle":
                    return "System.Data.OracleClient.OracleCommand";
                case "sqlite":
                    return "System.Data.SQLite.SQLiteCommand";
                default:
                    return null;
            }
        }

        private string defaultConnectionClass() {
            switch (_mapAs) {
                case "mysql":
                    return "MySql.Data.MySqlClient.MySqlConnection";
                case "postgresql":
                    return "Npgsql.NpgsqlConnection";
                case "oracle":
                    return "System.Data.OracleClient.OracleConnection";
                case "sqlite":
                    return "System.Data.SQLite.SQLiteConnection";
                default:
                    return null;
            }
        }
        private string defaultParameterClass() {
            switch (_mapAs) {
                case "mysql":
                    return "MySql.Data.MySqlClient.MySqlParameter";
                case "postgresql":
                    return "Npgsql.NpgsqlParameter";
                case "oracle":
                    return "System.Data.OracleClient.OracleParameter";
                case "sqlite":
                    return "System.Data.SQLite.SQLiteParameter";
                default:
                    return null;
            }
        }

        private string defaultDataAdapterClass() {
            switch (_mapAs) {
                case "mysql":
                    return "MySql.Data.MySqlClient.MySqlDataAdapter";
                case "postgresql":
                    return "Npgsql.NpgsqlDataAdapter";
                case "oracle":
                    return "System.Data.OracleClient.OracleDataAdapter";
                case "sqlite":
                    return "System.Data.SQLite.SQLiteDataAdapter";
                default:
                    return null;
            }
        }
        private string defaultUserNameMoniker() {
            switch (_mapAs) {
                case "mysql":
                    return "User Id";
                case "postgresql":
                    return "User Id";
                case "sqlite":
                    return null;
                case "oracle":
                    return "User Id";
                default:
                    return null;
            }
        }
        private string defaultPasswordMoniker() {
            switch (_mapAs) {
                case "mysql":
                    return "Password";
                case "postgresql":
                    return "Password";
                case "oracle":
                    return "Password";
                case "sqlite":
                    return "Password";
                default:
                    return null;
            }
        }

		protected internal override IDbCommand createCommand(string text) {
			string cmd = Toolkit.GetSetting(_providerName + ":Command", (string)defaultCommandClass());
			if (String.IsNullOrEmpty(cmd)){
				throw new InvalidOperationException(getDisplayMember("Dyn_createCommand", "Command object not defined properly.\n{0}", __text));
			}
			IDbCommand idb = (IDbCommand)Activator.CreateInstance(_assemblyName, cmd).Unwrap();
			idb.CommandText = text;
			return idb;
		}

		protected internal override IDbDataParameter createParam(IDbCommand cmd, string name, object val, DbType dbType) {
			string prm = Toolkit.GetSetting(_providerName + ":Parameter", (string)defaultParameterClass());
			if (String.IsNullOrEmpty(prm)) {
				throw new InvalidOperationException(getDisplayMember("Dyn_createParam", "Parameter object not defined properly.\n{0}", __text));
			}
			IDbDataParameter idp = (IDbDataParameter)Activator.CreateInstance(_assemblyName, prm).Unwrap();


			// different DBMS's use different parameter delimiters.  
			// look up the delimiter from the config settings and replace as needed
			// in the sql
			switch (_mapAs) {
				case "mysql":
					// mysql uses '?'
					string newName = name.Replace('@', '?').Replace(':', '?');
					cmd.CommandText = cmd.CommandText.Replace(name, newName);
					name = newName;
                    if (dbType == DbType.DateTime2) {
                        idp.DbType = DbType.DateTime;
                    } else {
                        idp.DbType = dbType;
                    }
					break;
                case "postgresql":
					// pgsql uses :
					string newName2 = name.Replace('@', ':').Replace('?', ':');
					cmd.CommandText = cmd.CommandText.Replace(name, newName2);
					name = newName2;
                    if (dbType == DbType.DateTime2) {
                        idp.DbType = DbType.DateTime;
                    } else {
                        idp.DbType = dbType;
                    }
                    break;
				case "oracle":
					// oracle uses :
					string newName3 = name.Replace('@', ':').Replace('?', ':');
					cmd.CommandText = cmd.CommandText.Replace(name, newName3);
					name = newName3;
					break;
                case "sqlite":
                    // sqlite uses ':' for named parameters and '?' for positional parameters.
                    // we're going to assume they MUST give us named parameters.
                    string newName4 = name.Replace('@', ':').Replace('?', ':');
                    cmd.CommandText = cmd.CommandText.Replace(name, newName4);
                    name = newName4;
                    break;
				case "sybase":
					// sybase uses @
					//string newName4 = name.Replace(':', '@').Replace('?', '@');
					//cmd.CommandText = cmd.CommandText.Replace(name, newName4);
					//name = newName4;
					//break;
					throw new NotImplementedException(getDisplayMember("Dyn_createParam", "Sybase support not added yet!"));
			}

			idp.ParameterName = name;
			idp.Value = val;
			return idp;
		}

		protected internal override DbDataAdapter createAdapter(IDbCommand cmd) {
			string adpt = Toolkit.GetSetting(_providerName + ":DataAdapter", defaultDataAdapterClass());
			if (String.IsNullOrEmpty(adpt)) {
				throw new InvalidOperationException(getDisplayMember("Dyn_createAdapter", "DataAdapter object not defined properly.\n{0}", __text));
			}
			DbDataAdapter dda = (DbDataAdapter)Activator.CreateInstance(_assemblyName, adpt).Unwrap();
			dda.SelectCommand = (DbCommand)cmd;
			return dda;
		}

		protected internal override IDbConnection createConn(string connString) {
			string cnn = Toolkit.GetSetting(_providerName + ":Connection", defaultConnectionClass());
			if (String.IsNullOrEmpty(cnn)) {
				throw new InvalidOperationException(getDisplayMember("Dyn_createConn{cnn}", "Connection object not defined properly.\n{0}", __text));
			}

			if (String.IsNullOrEmpty(DataConnectionSpec.UserNameMoniker) && !String.IsNullOrEmpty(this.DataConnectionSpec.UserName)) {
				throw new InvalidOperationException(getDisplayMember("Dyn_createConn{usermoniker}", "{0}:UserNameMoniker must be specified. This must be the value that needs to be put into your connection string to point at the user name.  e.g. 'User ID' or 'UserName'.",_providerName));
			}

            if (String.IsNullOrEmpty(DataConnectionSpec.PasswordMoniker) && !String.IsNullOrEmpty(this.DataConnectionSpec.Password)) {
				throw new InvalidOperationException(getDisplayMember("Dyn_createConn{passwordmoniker}", "{0}:PasswordMoniker must be specified. This must be the value that needs to be put into your connection string to point at the password.  e.g. 'Pwd' or 'Password'.", _providerName));
			}

			IDbConnection idc = (IDbConnection)Activator.CreateInstance(_assemblyName, cnn).Unwrap();
			idc.ConnectionString = connString;
			return idc;
		}

		public override string ScrubValue(string value) {
			switch (_mapAs) {
				case "mysql":
					return value.Replace("'", @"\'").Replace(@"""", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
                case "postgresql":
					return value.Replace("'", @"\'").Replace(@"""", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
				case "oracle":
					return value.Replace("'", @"\'").Replace(@"""", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
                case "sqlite":
                    return value.Replace("'", @"\'").Replace(@"""", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
                case "sybase":
				default:
					throw new NotImplementedException(getDisplayMember("Dyn_ScrubValue", "Sybase support is not yet implemented."));
			}
		}

		protected internal override void applyLimit(IDbCommand cmd, int limit, int offset) {
			switch (_mapAs) {
				case "mysql":
					if (limit > 0) {
                        if (!cmd.CommandText.ToLower().Trim().EndsWith("limit " + limit)) {
                            cmd.CommandText += " LIMIT " + limit;
                        }
					}
					if (offset > 0) {
						cmd.CommandText += " OFFSET " + offset;
					}
					break;
                case "postgresql":
                    if (limit > 0) {
                        if (!cmd.CommandText.ToLower().Trim().EndsWith("limit " + limit)) {
                            cmd.CommandText += " LIMIT " + limit;
                        }
                    }
                    if (offset > 0) {
						cmd.CommandText += " OFFSET " + offset;
					}
					break;
                case "sqlite":
                    if (limit > 0) {
                        cmd.CommandText += " LIMIT " + limit;
                    }
                    if (offset > 0) {
                        cmd.CommandText += " OFFSET " + offset;
                    }
                    break;
                case "oracle":

					string origSql = cmd.CommandText;
					string lowerSql = origSql.ToLower();

					string output = null;

					// oracle uses "where rownum > {offset} and rownum < {limit}" to do limits, so we need to inject the where...

					int iEndPos = lowerSql.Length;
					int iStartPos = -1;
					if (lowerSql.Contains("where")) {
						iStartPos = lowerSql.LastIndexOf("where") + 5;
					}
					if (lowerSql.Contains("order by")) {
						iEndPos = lowerSql.LastIndexOf("order by");
					}
					if (lowerSql.Contains("group by")) {
						iEndPos = lowerSql.LastIndexOf("group by");
					}

					// they may have multiple OR, AND, etc statements in where clause.
					// we wrap everything in () so we don't accidentally muck up operator precedence.
					StringBuilder sb = new StringBuilder();
					if (iStartPos == -1) {
						// no where clause. inject one.
						sb.Append(Toolkit.Cut(origSql, 0, iEndPos))
							.Append(" WHERE ");
					} else {
						// where clause. insert everything before it
						sb.Append(Toolkit.Cut(origSql, 0, iStartPos));
					}

					if (offset > 0){
						sb.Append(" ROWNUM > ").Append(offset);
					}
					if (limit > 0){
						if (offset > 0){
							sb.Append(" AND ");
						}
						sb.Append(" ROWNUM < ").Append(limit).Append(" ");
					}

					if (iStartPos == -1){
						sb.Append(Toolkit.Cut(origSql, iEndPos));
					} else {
						// where clause exists, tack the original where clause on the end
						sb.Append(" AND (")
							.Append(Toolkit.Cut(origSql, iStartPos, iEndPos - iStartPos))
							.Append(") ")
							.Append(Toolkit.Cut(origSql, iEndPos));

					}

					output = sb.ToString();

					cmd.CommandText = output;
					break;
				case "sybase":
					//if (dps == null) {
					//    dps = new DataParameters();
					//}
					//DataParameter dp2 = dps.Find(new Predicate<DataParameter>(delegate(DataParameter test) {
					//    return test.Key == "@ggcreturnid";
					//}));
					//if (dp2 == null) {
					//    dp2 = new DataParameter("@ggcreturnid", 0, DbType.Int32, ParameterDirection.Output);
					//    dps.Add(dp2);
					//    cmd.CommandText += "\r\nselect @@IDENTITY as @ggcreturnid\r\n";
					//} else {
					//    dp2.Value = 0;
					//}
					//break;
					throw new NotImplementedException(getDisplayMember("Dyn_applyLimit", "Sybase support not added yet!"));
			}
		}

		public override string Concatenate(string[] values) {
			StringBuilder sb = new StringBuilder();
			switch (_mapAs) {
				case "mysql":
					sb.Append("concat(");
					foreach (string v in values) {
						sb.Append("coalesce(")
							.Append(v)
							.Append(",''), ");
					}
					if (sb.Length > 2) {
						sb.Remove(sb.Length - 2, 2);
						sb.Append(")");
					}
					break;
                case "sqlite":
                    foreach (string v in values) {
                        sb.Append("coalesce(")
                            .Append(v)
                            .Append(",'') || ");
                    }
                    if (sb.Length > 4) {
                        sb.Remove(sb.Length - 4, 4);
                        sb.Append(")");
                    }
                    break;
                case "oracle":
					foreach (string v in values) {
						sb.Append("coalesce(")
							.Append(v)
							.Append(",'') || ");
					}
					if (sb.Length > 4) {
						sb.Remove(sb.Length - 4, 4);
					}
					break;
                case "postgresql":
                    sb.Append("concat(");
                    foreach (string v in values) {
                        sb.Append("coalesce(")
                            .Append(v)
                            .Append(",''), ");
                    }
                    if (sb.Length > 2) {
                        sb.Remove(sb.Length - 2, 2);
                        sb.Append(")");
                    }
                    break;
				default:
                    throw new NotImplementedException(getDisplayMember("Dyn_Concatenate", "Concatenate not implemented for vendor = {0}", DataConnectionSpec.Vendor.ToString()));
			}
			return sb.ToString();
		}

		protected internal override void mungeParameter(IDbCommand cmd, IDbDataParameter prm) {
			switch (_mapAs) {
                case "sqlite":
                    if (prm.DbType == DbType.DateTime2 || prm.DbType == DbType.DateTime) {
                        // sqlite doesn't understand dates natively, so we're going to assume they're stored as strings in universal coordinated time format (same as CURRENT_TIMESTAMP format sqlite uses internally)
                        if (prm.Value is DateTime){
                            prm.Value = ((DateTime)prm.Value).ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss"); //.ToString("o");
                        }
                    }
                    break;
				default:
					// do nothing
					break;
			}
		}


        private void postgresMungeSql(IDbCommand cmd) {


            if (String.IsNullOrEmpty(cmd.CommandText)) {
                return;
            }

            string lc = cmd.CommandText.ToLower();
//            if (!lc.Contains("concat") && !lc.Contains("left") && !lc.Contains("convert") && !lc.Contains("len")) {
            if (!lc.Contains("concat") && !lc.Contains("left") && !lc.Contains("convert") && !lc.Contains("len") && !lc.Contains("right")) {
                // no need to inspect further, don't need to munge CONCAT() / LEFT() / CONVERT()
                cmd.CommandText = Regex.Replace(cmd.CommandText, @"\sLIKE\s", " ILIKE ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                return;
            }


            List<string> parsed = cmd.CommandText.SplitRetain(new char[] { ' ', '(', ')', '\r', '\n', '\t', ',' }, true, true, false);


            // so "select * from concat(coalesce(col1,''), trim('  hi '))"
            // should come out as:
            //    { "select", " ", "*", " ", "from", " ", 
            //      "concat", "(", "coalesce", "(", "col1", ",", 
            //      "''", ")", ",", " ", "trim", "(", "'  hi '", 
            //      ")", ")" }


            StringBuilder sb = new StringBuilder();

            Stack<int> concatMatchEnds = new Stack<int>();
            Stack<int> concatParens = new Stack<int>();

            Stack<int> convertMatchEnds = new Stack<int>();
            Stack<int> convertParens = new Stack<int>();

            Stack<int> leftMatchEnds = new Stack<int>();
            Stack<int> leftParens = new Stack<int>();

            Stack<int> rightMatchEnds = new Stack<int>();
            Stack<int> rightParens = new Stack<int>();
            Stack<string> rightValues = new Stack<string>();

            Stack<int> lenMatchEnds = new Stack<int>();
            Stack<int> lenParens = new Stack<int>();

            int parenDepth = 0;

            StringBuilder sbConvertValue = new StringBuilder();
            string convertType = null;

            for (int i = 0; i < parsed.Count; i++) {

                string s = parsed[i];
                string lower = s.ToLower();

                if (s == "(") {
                    parenDepth++;
                }

                if (lower.StartsWith("'") || lower.StartsWith(@"""")) {
                    // quoted value, ignore / add to sb
                    if (rightValues.Count < rightParens.Count) {
                        rightValues.Push(s);
                    }
                    sb.Append(s);
                } else if (lower == "concat") {
                    // convert CONCAT('hi', 'there') into 'hi' + 'there'
                    // skip until parsed[i] = '(' (only happens if whitespace is between concat and (, such as "concat   ("
                    int parenPos = i;
                    while (parenPos < parsed.Count && parsed[parenPos] != "(") {
                        parenPos++;
                    }
                    if (parenPos < parsed.Count) {
                        int concat = findMatchingParen(parsed, parenPos);
                        if (i < concat) {
                            concatMatchEnds.Push(concat);
                            concatParens.Push(parenDepth + 1);
                        }
                    }
                } else if (lower == "left" && i < parsed.Count - 1 && parsed[i + 1] == "(") {
                    // convert left('asdf', 37) into substr('asdf', 0, 37)
                    int left = findMatchingParen(parsed, i + 1);
                    if (i < left) {
                        leftMatchEnds.Push(left);
                        leftParens.Push(parenDepth + 1);
                        sb.Append("substring");
                    }
                } else if (lower == "right" && i < parsed.Count - 1 && parsed[i + 1] == "(") {
                    // convert right('asdf', 37) into substr('asdf', 0, 37)
                    int right = findMatchingParen(parsed, i + 1);
                    if (i < right) {
                        rightMatchEnds.Push(right);
                        rightParens.Push(parenDepth + 1);
                        sb.Append("substring");
                    }
                } else if (lower == "convert") {
                    int convert = findMatchingParen(parsed, i + 1);
                    if (i < convert) {
                        convertMatchEnds.Push(convert);
                        convertParens.Push(parenDepth + 1);
                        sb.Append("cast");
                    }
                } else if (lower == "len") {
                    int lenPos = findMatchingParen(parsed, i + 1);
                    if (i < lenPos) {
                        lenMatchEnds.Push(lenPos);
                        lenParens.Push(parenDepth + 1);
                        sb.Append("char_length");
                    }
                } else {

                    bool next = false;
                    if (concatMatchEnds.Count > 0) {
                        int concatMatch = concatMatchEnds.Peek();
                        int concatDepth = concatParens.Peek();
                        if (i < concatMatch && parenDepth == concatDepth) {
                            // we're inside a concat, but not too deep to be in another sub function.
                            if (parsed[i] == ",") {
                                sb.Append(" || ");
                            } else {
                                sb.Append(s);
                            }
                            next = true;
                        } else if (i == concatMatch) {
                            concatMatchEnds.Pop();
                            concatParens.Pop();
                            sb.Append(")");
                            next = true;
                        }
                    }
                    if (!next) {
                        if (leftMatchEnds.Count > 0) {

                            // left('asdf', 3) => substring('asdf', 1, 3)
                            // i.e. left(val, x) => substring(val, 1, x)

                            int leftMatch = leftMatchEnds.Peek();
                            int leftDepth = leftParens.Peek();
                            if (i < leftMatch && parenDepth == leftDepth) {
                                if (parsed[i] == ",") {
                                    sb.Append(", 1, ");
                                } else {
                                    sb.Append(s);
                                }
                                next = true;
                            } else if (i == leftMatch) {
                                leftMatchEnds.Pop();
                                leftParens.Pop();
                                sb.Append(")");
                                next = true;
                            }
                        }
                    }

                    if (!next) {
                        if (rightMatchEnds.Count > 0) {

                            // right('asdf', 3) =>  substring('asdf', 2)
                            // i.e. right(val, x) => substring(val, len(val) - x)

                            int rightMatch = rightMatchEnds.Peek();
                            int rightDepth = rightParens.Peek();
                            if (i < rightMatch && parenDepth == rightDepth) {
                                if (parsed[i] == ",") {
                                    sb.Append(s);
                                } else {
                                    var size = Toolkit.ToInt32(s, -1);
                                    if (size > -1) {
                                        sb.Append("char_length(");
                                        var rightVal = "''";
                                        if (rightValues.Count > 0){
                                            rightVal = rightValues.Pop();
                                        }
                                        sb.Append(rightVal)
                                            .Append(") - ")
                                            .Append((size - 1).ToString());
                                    } else {
                                        sb.Append(s);
                                    }
                                    //rightValues.Push(s);
                                    //var rightVal = rightValues.Peek();
                                    //sb.Append(", len(" + rightVal + ") - ");
                                    //sb.Append(s);
                                }
                                next = true;
                            } else if (i == rightMatch) {
                                rightMatchEnds.Pop();
                                rightParens.Pop();
                                sb.Append(")");
                                next = true;
                            }
                        }
                    }

                    if (!next) {
                        if (convertMatchEnds.Count > 0) {
                            int convertMatch = convertMatchEnds.Peek();
                            int convertDepth = convertParens.Peek();
                            // convert(type, value) => cast(value as type)
                            if (i < convertMatch && parenDepth == convertDepth) {
                                if (parsed[i] != "(" && parsed[i] != ")") {
                                    if (convertType == null && parsed[i].Trim().Length > 0) {
                                        // we're on the type
                                        convertType = parsed[i];
                                        switch (convertType.ToLower()) {
                                            case "datetime":
                                            case "datetime2":
                                                convertType = "timestamp";
                                                break;
                                            case "nvarchar":
                                                convertType = "varchar";
                                                break;
                                        }
                                    } else if (parsed[i] == ",") {
                                        // we're on the separator
                                    } else {
                                        // must be on the value
                                        sbConvertValue.Append(parsed[i]);
                                    }
                                } else {
                                    sb.Append(s);
                                }
                                next = true;
                            } else if (i == convertMatch) {
                                convertMatchEnds.Pop();
                                convertParens.Pop();
                                sb.Append(sbConvertValue.ToString()).Append(" as ").Append(convertType);
                                convertType = null;
                                sbConvertValue = new StringBuilder();
                                sb.Append(")");
                                next = true;
                            }
                        }
                    }

                    if (!next) {
                        if (lenMatchEnds.Count > 0) {
                            int lenMatch = lenMatchEnds.Peek();
                            int lenDepth = lenParens.Peek();
                            if (i < lenMatch && parenDepth == lenDepth) {
                                if (parsed[i] == ",") {
                                    sb.Append(", 1, ");
                                } else {
                                    sb.Append(s);
                                }
                                next = true;
                            } else if (i == lenMatch) {
                                lenMatchEnds.Pop();
                                lenParens.Pop();
                                sb.Append(")");
                                next = true;
                            }
                        }
                    }

                    if (!next) {
                        // no text found that needs munging, just copy it over
                        sb.Append(s);
                    }
                }

                if (s == ")") {
                    parenDepth--;
                }

            }

            var sql = sb.ToString();
            sql = Regex.Replace(sql, @"\sLIKE\s", " ILIKE ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            cmd.CommandText = sql;


        }

        private void sqliteMungeSql(IDbCommand cmd) {


            if (String.IsNullOrEmpty(cmd.CommandText)) {
                return;
            }

            string lc = cmd.CommandText.ToLower();
            if (!lc.Contains("concat") && !lc.Contains("left") && !lc.Contains("convert")) {
                // no need to inspect further, don't need to munge CONCAT() / LEFT() / CONVERT()
                return;
            }


            List<string> parsed = cmd.CommandText.SplitRetain(new char[] { ' ', '(', ')', '\r', '\n', '\t', ',' }, true, true, false);


            // so "select * from concat(coalesce(col1,''), trim('  hi '))"
            // should come out as:
            //    { "select", " ", "*", " ", "from", " ", 
            //      "concat", "(", "coalesce", "(", "col1", ",", 
            //      "''", ")", ",", " ", "trim", "(", "'  hi '", 
            //      ")", ")" }


            StringBuilder sb = new StringBuilder();

            Stack<int> concatMatchEnds = new Stack<int>();
            Stack<int> concatParens = new Stack<int>();

            Stack<int> convertMatchEnds = new Stack<int>();
            Stack<int> convertParens = new Stack<int>();

            Stack<int> leftMatchEnds = new Stack<int>();
            Stack<int> leftParens = new Stack<int>();

            int parenDepth = 0;

            StringBuilder sbConvertValue = new StringBuilder();
            string convertType = null;

            for (int i = 0; i < parsed.Count; i++) {

                string s = parsed[i];
                string lower = s.ToLower();

                if (s == "(") {
                    parenDepth++;
                }

                if (lower.StartsWith("'") || lower.StartsWith(@"""")) {
                    // quoted value, ignore / add to sb
                    sb.Append(s);
                } else if (lower == "concat") {
                    // convert CONCAT('hi', 'there') into 'hi' + 'there'
                    // skip until parsed[i] = '(' (only happens if whitespace is between concat and (, such as "concat   ("
                    int parenPos = i;
                    while (parenPos < parsed.Count && parsed[parenPos] != "(") {
                        parenPos++;
                    }
                    if (parenPos < parsed.Count) {
                        int concat = findMatchingParen(parsed, parenPos);
                        if (i < concat) {
                            concatMatchEnds.Push(concat);
                            concatParens.Push(parenDepth + 1);
                        }
                    }
                } else if (lower == "left" && i < parsed.Count - 1 && parsed[i + 1] == "(") {
                    // convert left('asdf', 37) into substr('asdf', 0, 37)
                    int left = findMatchingParen(parsed, i + 1);
                    if (i < left) {
                        leftMatchEnds.Push(left);
                        leftParens.Push(parenDepth + 1);
                        sb.Append("substring");
                    }
                } else if (lower == "convert") {
                    int convert = findMatchingParen(parsed, i + 1);
                    if (i < convert) {
                        convertMatchEnds.Push(convert);
                        convertParens.Push(parenDepth + 1);
                        sb.Append("cast");
                    }
                } else {

                    bool next = false;
                    if (concatMatchEnds.Count > 0) {
                        int concatMatch = concatMatchEnds.Peek();
                        int concatDepth = concatParens.Peek();
                        if (i < concatMatch && parenDepth == concatDepth) {
                            // we're inside a concat, but not too deep to be in another sub function.
                            if (parsed[i] == ",") {
                                sb.Append(" || ");
                            } else {
                                sb.Append(s);
                            }
                            next = true;
                        } else if (i == concatMatch) {
                            concatMatchEnds.Pop();
                            concatParens.Pop();
                            sb.Append(")");
                            next = true;
                        }
                    }
                    if (!next) {
                        if (leftMatchEnds.Count > 0) {
                            int leftMatch = leftMatchEnds.Peek();
                            int leftDepth = leftParens.Peek();
                            if (i < leftMatch && parenDepth == leftDepth) {
                                if (parsed[i] == ",") {
                                    sb.Append(", 1, ");
                                } else {
                                    sb.Append(s);
                                }
                                next = true;
                            } else if (i == leftMatch) {
                                leftMatchEnds.Pop();
                                leftParens.Pop();
                                sb.Append(")");
                                next = true;
                            }
                        }
                    }

                    if (!next) {
                        if (convertMatchEnds.Count > 0) {
                            int convertMatch = convertMatchEnds.Peek();
                            int convertDepth = convertParens.Peek();
                            // convert(type, value) => cast(value as type)
                            if (i < convertMatch && parenDepth == convertDepth) {
                                if (parsed[i] != "(" && parsed[i] != ")") {
                                    if (convertType == null && parsed[i].Trim().Length > 0) {
                                        // we're on the type
                                        convertType = parsed[i];
                                        switch (convertType.ToLower()) {
                                            case "datetime":
                                            case "datetime2":
                                                convertType = "timestamp";
                                                break;
                                        }
                                    } else if (parsed[i] == ",") {
                                        // we're on the separator
                                    } else {
                                        // must be on the value
                                        sbConvertValue.Append(parsed[i]);
                                    }
                                } else {
                                    sb.Append(s);
                                }
                                next = true;
                            } else if (i == convertMatch) {
                                convertMatchEnds.Pop();
                                convertParens.Pop();
                                sb.Append(sbConvertValue.ToString()).Append(" as ").Append(convertType);
                                convertType = null;
                                sbConvertValue = new StringBuilder();
                                sb.Append(")");
                                next = true;
                            }
                        }
                    }

                    if (!next) {
                        // no text found that needs munging, just copy it over
                        sb.Append(s);
                    }
                }

                if (s == ")") {
                    parenDepth--;
                }

            }

            cmd.CommandText = sb.ToString();


        }


        private void mysqlMungeSql(IDbCommand cmd) {


            if (String.IsNullOrEmpty(cmd.CommandText)) {
                return;
            }

            string lc = cmd.CommandText.ToLower();
            if (!lc.Contains("convert") && !lc.Contains("len")) {
                // no need to inspect further, don't need to munge CONCAT() / LEFT() / CONVERT()
                return;
            }


            List<string> parsed = cmd.CommandText.SplitRetain(new char[] { ' ', '(', ')', '\r', '\n', '\t', ',' }, true, true, false);


            // so "select * from concat(coalesce(col1,''), trim('  hi '))"
            // should come out as:
            //    { "select", " ", "*", " ", "from", " ", 
            //      "concat", "(", "coalesce", "(", "col1", ",", 
            //      "''", ")", ",", " ", "trim", "(", "'  hi '", 
            //      ")", ")" }


            StringBuilder sb = new StringBuilder();

            Stack<int> convertMatchEnds = new Stack<int>();
            Stack<int> convertParens = new Stack<int>();

            Stack<int> lenMatchEnds = new Stack<int>();
            Stack<int> lenParens = new Stack<int>();

            int parenDepth = 0;

            StringBuilder sbConvertValue = new StringBuilder();
            string convertType = null;

            for (int i = 0; i < parsed.Count; i++) {

                string s = parsed[i];
                string lower = s.ToLower();

                if (s == "(") {
                    parenDepth++;
                }

                if (lower.StartsWith("'") || lower.StartsWith(@"""")) {
                    // quoted value, ignore / add to sb
                    sb.Append(s);
                //} else if (lower == "convert") {
                //    int convert = findMatchingParen(parsed, i + 1);
                //    if (i < convert) {
                //        convertMatchEnds.Push(convert);
                //        convertParens.Push(parenDepth + 1);
                //        sb.Append("cast");
                //    }
                } else if (lower == "len") {
                    int lenPos = findMatchingParen(parsed, i + 1);
                    if (i < lenPos) {
                        lenMatchEnds.Push(lenPos);
                        lenParens.Push(parenDepth + 1);
                        sb.Append("length");
                    }
                } else {

                    bool next = false;
                    if (!next) {
                        if (convertMatchEnds.Count > 0) {
                            int convertMatch = convertMatchEnds.Peek();
                            int convertDepth = convertParens.Peek();
                            // convert(type, value) => cast(value as type)
                            if (i < convertMatch && parenDepth == convertDepth) {
                                if (parsed[i] != "(" && parsed[i] != ")") {
                                    if (convertType == null && parsed[i].Trim().Length > 0) {
                                        // we're on the type
                                        convertType = parsed[i];
                                        switch (convertType.ToLower()) {
                                            case "datetime":
                                            case "datetime2":
                                                convertType = "timestamp";
                                                break;
                                            case "nvarchar":
                                                convertType = "varchar";
                                                break;
                                        }
                                    } else if (parsed[i] == ",") {
                                        // we're on the separator
                                    } else {
                                        // must be on the value
                                        sbConvertValue.Append(parsed[i]);
                                    }
                                } else {
                                    sb.Append(s);
                                }
                                next = true;
                            } else if (i == convertMatch) {
                                convertMatchEnds.Pop();
                                convertParens.Pop();
                                sb.Append(sbConvertValue.ToString()).Append(" as ").Append(convertType);
                                convertType = null;
                                sbConvertValue = new StringBuilder();
                                sb.Append(")");
                                next = true;
                            }
                        }
                    }

                    if (!next) {
                        if (lenMatchEnds.Count > 0) {
                            int lenMatch = lenMatchEnds.Peek();
                            int lenDepth = lenParens.Peek();
                            if (i < lenMatch && parenDepth == lenDepth) {
                                if (parsed[i] == ",") {
                                    sb.Append(", 1, ");
                                } else {
                                    sb.Append(s);
                                }
                                next = true;
                            } else if (i == lenMatch) {
                                lenMatchEnds.Pop();
                                lenParens.Pop();
                                sb.Append(")");
                                next = true;
                            }
                        }
                    }

                    if (!next) {
                        // no text found that needs munging, just copy it over
                        sb.Append(s);
                    }
                }

                if (s == ")") {
                    parenDepth--;
                }

            }

            var sql = sb.ToString();
            cmd.CommandText = sql;


        }

		protected internal override void mungeSql(IDbCommand cmd) {
			// do nothing -- yet

            switch (_mapAs) {
                case "postgresql":
                    postgresMungeSql(cmd);
                    break;
                case "sqlite":
                    sqliteMungeSql(cmd);
                    break;
                case "mysql":
                    mysqlMungeSql(cmd);
                    break;
            }

		}

		protected internal override void enableReturnId(IDbCommand cmd, string nameOfSequenceOrPrimaryKey, ref DataParameters dps) {
			if (cmd.CommandType == CommandType.Text) {
				switch (_mapAs) {
					case "mysql":
						if (!cmd.CommandText.Contains("; SELECT LAST_INSERT_ID();")) {
							cmd.CommandText += "; SELECT LAST_INSERT_ID();";
						}
						break;
                    case "sqlite":
                        if (!cmd.CommandText.Contains("; SELECT LAST_INSERT_ROWID();")) {
                            cmd.CommandText += "; SELECT LAST_INSERT_ROWID();";
                        }
                        break;
                    case "postgresql":
						if (!cmd.CommandText.Contains(" returning " + nameOfSequenceOrPrimaryKey + ";")) {
							cmd.CommandText += " returning " + nameOfSequenceOrPrimaryKey + ";";
						}
						break;
					case "oracle":
						if (dps == null) {
							dps = new DataParameters();
						}
						DataParameter dp = dps.Find(test => {
							return test.Key == ":ggcreturnid";
						});
						if (dp == null) {
							dp = new DataParameter(":ggcreturnid", 0, DbType.Int32, ParameterDirection.Output);
							dps.Add(dp);
						} else {
							dp.Value = 0;
						}
						if (!cmd.CommandText.Contains(" returning " + nameOfSequenceOrPrimaryKey + " into :ggcreturnid")) {
							cmd.CommandText += " returning " + nameOfSequenceOrPrimaryKey + " into :ggcreturnid";
						}
						break;
					case "sybase":
						//if (dps == null) {
						//    dps = new DataParameters();
						//}
						//DataParameter dp2 = dps.Find(new Predicate<DataParameter>(delegate(DataParameter test) {
						//    return test.Key == "@ggcreturnid";
						//}));
						//if (dp2 == null) {
						//    dp2 = new DataParameter("@ggcreturnid", 0, DbType.Int32, ParameterDirection.Output);
						//    dps.Add(dp2);
						//    cmd.CommandText += "\r\nselect @@IDENTITY as @ggcreturnid\r\n";
						//} else {
						//    dp2.Value = 0;
						//}
						//break;
						throw new NotImplementedException(getDisplayMember("Dyn_enableReturnId", "Sybase support not added yet!"));
				}
			}
		}

		protected internal override object executeInsert(IDbCommand cmd) {
			object ret = null;
			switch (_mapAs) {
				case "mysql":
					ret = cmd.ExecuteScalar();
					break;
                case "sqlite":
                    ret = cmd.ExecuteScalar();
                    break;
                case "postgresql":
					ret = cmd.ExecuteScalar();
					break;
				case "oracle":
					cmd.ExecuteNonQuery();
					string prmName = cmd.Parameters.Contains("ggcreturnid") ? "ggcreturnid" : ":ggcreturnid";
					ret = ((IDataParameter)cmd.Parameters[prmName]).Value;
					break;
				case "sybase":
					//cmd.ExecuteNonQuery();
					//string prmName2 = cmd.Parameters.Contains("ggcreturnid") ? "ggcreturnid" : "@ggcreturnid";
					//ret = ((IDataParameter)cmd.Parameters[prmName2]).Value;
					//break;
					throw new NotImplementedException(getDisplayMember("Dyn_executeInsert", "Sybase support not added yet!"));
			}
			return ret;
		}

    }
}
