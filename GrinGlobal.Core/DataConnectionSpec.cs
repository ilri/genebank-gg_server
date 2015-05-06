using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace GrinGlobal.Core {
//#if !DEBUGDATAMANAGER
//    [DebuggerStepThrough()]
//#endif
	public class DataConnectionSpec {
        private string _conn;
		public string ConnectionString { 
            get {
                if (String.IsNullOrEmpty(_conn)) {
                    _conn = RecalculateConnectionString();
                }
                return _conn;
            }
            set {
                _conn = value;

                if (!String.IsNullOrEmpty(_conn)) {
                    if (!String.IsNullOrEmpty(UserNameMoniker)) {
                        UserName = Toolkit.GetValueFromNameValuePair(_conn, UserNameMoniker, UserName);
                        if (!String.IsNullOrEmpty(UserName)) {
                            UserName = UserName.Replace("'", "");
                        }
                    }
                    if (!String.IsNullOrEmpty(PasswordMoniker)) {
                        Password = Toolkit.GetValueFromNameValuePair(_conn, PasswordMoniker, Password);
                        if (!String.IsNullOrEmpty(Password)) {
                            Password = Password.Replace("'", "");
                        }
                    }
                    if (String.IsNullOrEmpty(DatabaseName)) {
                        var db = Toolkit.GetValueFromNameValuePair(_conn, "Database", null);
                        if (String.IsNullOrEmpty(db)) {
                            db = Toolkit.GetValueFromNameValuePair(_conn, "Initial Catalog", null);
                            if (String.IsNullOrEmpty(db)) {
                            }
                        }
                        DatabaseName = db;
                    }
                    if (String.IsNullOrEmpty(ServerName)) {
                        var server = Toolkit.GetValueFromNameValuePair(_conn, "Server", null);
                        if (String.IsNullOrEmpty(server)) {
                            server = Toolkit.GetValueFromNameValuePair(_conn, "Data Source", null);
                        }
                        ServerName = server;
                    }
                }

            }
        }

		public string ConnectionStringSettingName { get; set; }
		public string ProviderName { get; set; }
		public string Name { get; set; }
        public bool UseWindowsAuthentication { get; set; }

        public override string ToString() {
            return "Conn=" + ConnectionString + "; Provider=" + ProviderName + "; Server=" + ServerName + "; Port=" + Port + "; Instance=" + InstanceName + "; Name=" + Name + "; UseWindowsAuthentication=" + UseWindowsAuthentication + "; UserName=" + UserName + "; UserNameMoniker=" + UserNameMoniker + "; Password=" + Password + "; PasswordMoniker=" + PasswordMoniker + "; Vendor=" + this.Vendor + "; EngineName=" + this.EngineName + "; CommandTimeout=" + this.CommandTimeout + "; IsChild=" + (this.Parent == null ? "No" : "Yes");
        }

        public string UserName {
            get;
            set;
        }

        public string UserNameMoniker { get; internal set; }

        public string Password {
            get;
            set;
        }

        public string PasswordMoniker { get; internal set; }

        public string DatabaseName { get; set; }
        private string _serverName;
        public string ServerName {
            get {
                return _serverName;
            }
            set {
                var arr = (string.Empty + value).Split('\\');
                if (arr.Length > 1) {
                    _serverName = arr[0];
                    if (arr.Length > 1) {
                        InstanceName = arr[1];
                    }
                } else {
                    arr = (string.Empty + value).Split(':');
                    _serverName = arr[0];
                    if (arr.Length > 1) {
                        Port = Toolkit.ToInt32(arr[1], 0);
                    }
                }
            }
        }
        public string ServiceName { get; set; }
        public int Port { get; set; }
        public string InstanceName { get; set; }
        public string SID { get; set; }

		public DataManager Parent { get; set; }

		public DataVendor Vendor { get; set; }

        /// <summary>
        /// Gets the name of the database engine used.  Driven by the Vendor property.  Returns "mysql", "sqlserver", "oracle", or "postgresql".  If Vendor property is Unknown or OleDb or ODBC, returns "unknown".
        /// </summary>
        public string EngineName {
            get {
                switch (Vendor) {
                    case DataVendor.SqlServer:
                        return "sqlserver";
                    case DataVendor.Oracle:
                        return "oracle";
                    case DataVendor.MySql:
                        return "mysql";
                    case DataVendor.PostgreSql:
                        return "postgresql";
                    case DataVendor.Unknown:
                    case DataVendor.OleDb:
                    case DataVendor.ODBC:
                    default:
                        return "unknown";
                }
            }
        }

		public int? CommandTimeout { get; set; }

		public DataConnectionSpec() {
		}

        public string RecalculateConnectionString() {
            var rv = "";
            if (!String.IsNullOrEmpty(ProviderName)) {
                switch (ProviderName.ToLower()) {
                    case "postgresql":
                        rv = String.Format(@"Server={0};Port={1};{2}User Id='{3}';Password='{4}';", ServerName, Port, (String.IsNullOrEmpty(DatabaseName) ? "" : "Database=" + DatabaseName + ";"), ("" + UserName).Replace("'", "''"), ("" + Password).Replace("'", "''"));
                        break;

                    case "mysql":
                        rv = String.Format(@"Server={0};Port={1};{2}User ID={3};Password={4};Pooling=true;Connection Timeout=10;Protocol=socket;", ServerName, Port, (String.IsNullOrEmpty(DatabaseName) ? "" : "Database=" + DatabaseName + ";"), UserName, Password);
                        break;
                    case "sqlite":
                        rv = String.Format(@"Data Source={0};Version=3';", DatabaseName);
                        break;
                    case "sqlserver":

                        string initialCatalog = (String.IsNullOrEmpty(DatabaseName) ? "" : "Initial Catalog=" + DatabaseName + ";");

                        string separator = null;
                        if (String.IsNullOrEmpty(InstanceName)) {
                            // default instance, no separator
                            separator = "";
                        } else {
                            if (Toolkit.ToInt32(InstanceName, -1) > -1) {
                                // using port, need a comma
                                separator = ", ";
                            } else {
                                // using instance name, need a backslash
                                separator = @"\";
                            }
                        }

                        if (UseWindowsAuthentication) {
                            // assume integrated security -- either null user or specified SA and empty password.
                            //                dcs.ConnectionString = String.Format(@"Data Source={0}" + separator + @"{1};{2}Integrated Security=SSPI;", ServerName, instanceName, initialCatalog);
                            rv = String.Format(@"Data Source={0}" + separator + @"{1};{2}Trusted_Connection=True;", ServerName, InstanceName, initialCatalog);
                        } else {
                            rv = String.Format(@"Data Source={0}" + separator + @"{1};{2}User Id='{3}';Password='{4}';", ServerName, InstanceName, initialCatalog, ("" + UserName).Replace("'", "''"), ("" + Password).Replace("'", "''"));
                        }
                        break;

                    case "oracle":
                        var dbPriv = "";
                        //if (("" + userName).ToUpper() == "SYS" || ("" + userName).ToUpper() == "SYSTEM") {
                        //    dbPriv = "DBA Privilege=SYSDBA;";
                        //}
                        //dcs.ConnectionString = String.Format(@"Data Source=XE;User Id='{2}';Password='{3}';{4}", ServerName, Port, ("" + userName).Replace("'", "''"), ("" + password).Replace("'", "''"), dbPriv);

                        var sid = String.IsNullOrEmpty(SID) ? "NO_SID_PROVIDED" : SID;

                        rv = String.Format(@"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={5})));User Id='{2}';Password='{3}';{4}", ServerName, Port, ("" + UserName).Replace("'", "''"), ("" + Password).Replace("'", "''"), dbPriv, sid);
                        break;

                    default:
                        throw new NotImplementedException(getDisplayMember("ConnectionString{get}", "DataConnectionSpec does not define a connection string for providerName={0}", ProviderName));
                }
            }
            return rv;
        }

		public DataConnectionSpec(string connectionStringSettingName) {
			ConnectionStringSettingName = connectionStringSettingName;
		}

        public DataConnectionSpec(string providerName, string connectionString) {
            ProviderName = providerName;
            if (!String.IsNullOrEmpty(providerName)) {
                switch (providerName.ToLower()) {
                    case "oracle":
                        Vendor = DataVendor.Oracle;
                        UserNameMoniker = "User Id";
                        PasswordMoniker = "Password";
                        Port = 1521;
                        break;
                    case "postgresql":
                        Vendor = DataVendor.PostgreSql;
                        UserNameMoniker = "User Id";
                        PasswordMoniker = "Password";
                        Port = 5432;
                        break;
                    case "sqlserver":
                        Vendor = DataVendor.SqlServer;
                        UserNameMoniker = "User Id";
                        PasswordMoniker = "Password";
                        Port = 1443;
                        break;
                    case "mysql":
                        Vendor = DataVendor.MySql;
                        UserNameMoniker = "User Id";
                        PasswordMoniker = "Password";
                        Port = 3306;
                        break;
                }
            }
            ConnectionString = connectionString;
        }


        public DataConnectionSpec(string providerName, string userName, string password, string connectionString) : this(providerName, connectionString) {
            UserName = userName;
            Password = password;
        }

        public DataConnectionSpec(string providerName, string userName, string password) 
            : this(providerName, userName, password, null){

        }

		/// <summary>
		/// Makes a deep copy of the current DataConnectionSpec EXCEPT DataManager represented by Parent property is not cloned
		/// </summary>
		/// <returns></returns>
		public DataConnectionSpec Clone() {
			DataConnectionSpec dsc = new DataConnectionSpec();
			dsc.ConnectionString = ConnectionString;
			dsc.ConnectionStringSettingName = ConnectionStringSettingName;
			dsc.ProviderName = ProviderName;
			dsc.Name = Name;
			dsc.UserName = UserName;
			dsc.Password = Password;
			dsc.Parent = Parent;
			dsc.Vendor = Vendor;
			dsc.CommandTimeout = CommandTimeout;
            dsc.UseWindowsAuthentication = UseWindowsAuthentication;
            dsc.PasswordMoniker = PasswordMoniker;
            dsc.UserNameMoniker = UserNameMoniker;
            dsc.ServerName = ServerName;
            dsc.Port = Port;
            dsc.InstanceName = InstanceName;
            dsc.DatabaseName = DatabaseName;
			return dsc;
		}

		/// <summary>
		/// Makes a deep copy of the current DataConnectionSpec but uses the given DataManager for the Parent property
		/// </summary>
		/// <param name="newParent"></param>
		/// <returns></returns>
		public DataConnectionSpec Clone(DataManager newParent) {
			DataConnectionSpec dsc = Clone();
			dsc.Parent = newParent;
			return dsc;
		}

        public static DataConnectionSpec Parse(string providerName, string connectionString) {
            var rv = new DataConnectionSpec { ProviderName = providerName, ConnectionString = connectionString };

            var dic = Toolkit.ParsePairs<string>(connectionString);
            foreach (var key in dic.Keys) {
                switch (key.ToLower()) {
                    case "server":
                    case "data source":
                        // property setter will parse as needed
                        rv.ServerName = dic[key];
                        break;
                    case "integrated security":
                    case "trusted_connection":
                    case "trusted connection":
                        rv.UseWindowsAuthentication = dic[key].ToLower() == "true" || dic[key].ToLower() == "sspi";
                        break;
                    case "port":
                        rv.Port = Toolkit.ToInt32(dic[key], 0);
                        break;
                    case "database":
                        rv.DatabaseName = dic[key];
                        break;
                    case "user id":
                    case "uid":
                        rv.UserName = dic[key];
                        rv.UserNameMoniker = key;
                        break;
                    case "password":
                    case "pwd":
                        rv.Password = dic[key];
                        rv.PasswordMoniker = key;
                        break;
                    case "service_name":
                        rv.InstanceName = dic[key];
                        break;
                }
            }
            return rv;
        }

        //public static DataConnectionSpec Parse(string providerName, string serverName, string databaseName, string userName, string password, bool useWindowsAuthentication) {
        //    var dcs = new DataConnectionSpec { ProviderName = providerName, ServerName = serverName, DatabaseName = databaseName, UserName = userName, Password = password, UseWindowsAuthentication = useWindowsAuthentication };
        //    return dcs;
        //}

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "Core", "DataConnectionSpec", resourceName, null, defaultValue, substitutes);
        }

	}
}