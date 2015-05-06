using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace GrinGlobal.Core.DataManagers {
    public class SqlServerEngineUtil : DatabaseEngineUtil {

        internal SqlServerEngineUtil(string fullPathToUACExe, string preferredInstanceName)
            : base("sqlserver",
                Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\100", "VerSpecificRootDir", "") + @"Tools\Binn\",
                Toolkit.GetRegSetting(getSqlServerBaseRegistryKey(preferredInstanceName), "SQLPath", ""),
                Toolkit.GetRegSetting(getSqlServerBaseRegistryKey(preferredInstanceName), "SQLDataRoot", "") + @"\DATA",
                GetServiceName(preferredInstanceName),
                "SQL Server 2008 Express",
                "sa",
                fullPathToUACExe, 
                1433) {
            _instanceName = preferredInstanceName;
        }

        private string _instanceName;
        public string InstanceName {
            get {
                return _instanceName;
            }
            set {
                _instanceName = value;
                BinDirectory = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\100", "VerSpecificRootDir", "") + @"Tools\Binn\";
                BaseDirectory = Toolkit.GetRegSetting(getSqlServerBaseRegistryKey(_instanceName), "SQLPath", "");
                DataDirectory = Toolkit.GetRegSetting(getSqlServerBaseRegistryKey(_instanceName), "SQLDataRoot", "") + @"\DATA";
                ServiceName = GetServiceName(_instanceName);
            }
        }

        //private string _instanceName;
        //public string InstanceName {
        //    get {
        //        return _instanceName;
        //    }
        //    set {
        //        _instanceName = value;
        //    }
        //}

        public void RefreshProperties() {
            this.ServiceName = GetServiceName(_instanceName);
        }

        private static string replaceMsSqlVersion(string value, string putIn) {
            var dotpos = value.IndexOf(".");
            if (dotpos > -1) {
                return putIn + Toolkit.Cut(value, dotpos + 1);
            } else {
                return value;
            }
        }

        public static string GetServiceName(string instanceName) {
            var fullInstanceName = GetFullInstanceName(instanceName);

            string prefix = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\Services\SQL Server", "LName", "");
            //var serviceName = ("" + fullInstanceName).Replace("MSSQL10.", prefix);
            var serviceName = replaceMsSqlVersion("" + fullInstanceName, prefix);
            return serviceName;
        }

        /// <summary>
        /// Returns the full instance name, which includes the MSSQL10. prefix
        /// </summary>
        /// <param name="preferredInstance"></param>
        /// <param name="returnFullName"></param>
        /// <returns></returns>
        public static string GetFullInstanceName(string instanceName) {
            if (String.IsNullOrEmpty(instanceName) || instanceName.Trim().Length == 0) {
                instanceName = "SQLEXPRESS";
            }
            instanceName = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", instanceName, "");
            return instanceName;
        }

        /// <summary>
        /// Returns the instance name without the "MSSQL10." prefix
        /// </summary>
        /// <param name="preferredInstance"></param>
        /// <returns></returns>
        public static string GetPartialInstanceName(string instanceName) {

            var fullInstanceName = GetFullInstanceName(instanceName);
            fullInstanceName = replaceMsSqlVersion("" + fullInstanceName, "");
            //fullInstanceName = ("" + fullInstanceName).Replace("MSSQL10.", "");
            return fullInstanceName;
        }

        private static string getSqlServerBaseRegistryKey(string instanceName) {

            var fullInstanceName = GetFullInstanceName(instanceName);
            string baseKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\" + fullInstanceName + @"\Setup";
            return baseKey;


        }


        public bool IsMixedModeEnabled(string instanceName) {
            _instanceName = instanceName;
            var fullInstanceName = GetFullInstanceName(instanceName);
            int val = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\" + fullInstanceName + @"\MSSQLServer", "LoginMode", 0);

            return val == 2;
        }

        public void EnableUser(string superUserPassword, string databaseName, string usernameToChange) {

            var rv = ExecuteSql(superUserPassword, databaseName, "ALTER LOGIN [" + usernameToChange + "] ENABLE");
            if (!String.IsNullOrEmpty(rv) && rv.Trim().Length > 0) {
                throw new InvalidOperationException(getDisplayMember("Sqlserver_EnableUser", "Error enabling user {0}: {1}", usernameToChange, rv));
            }

        }

        public void DisableUser(string superUserPassword, string databaseName, string usernameToChange) {

            var rv = ExecuteSql(superUserPassword, databaseName, "ALTER LOGIN [" + usernameToChange + "] DISABLE");
            if (!String.IsNullOrEmpty(rv) && rv.Trim().Length > 0) {
                throw new InvalidOperationException(getDisplayMember("Sqlserver_DisableUser", "Error disabling user {0}: {1}", usernameToChange, rv));
            }

        }

        public void ChangePassword(string superUserPassword, string databaseName, string targetUsername, string oldPassword, string newPassword) {
            //var rv = ExecuteSql(superUserPassword, databaseName, String.Format("EXEC sp_password '{0}', '{1}', '{2}'", oldPassword.Replace("'", "''"), newPassword.Replace("'", "''"), targetUsername));

            var rv = ExecuteSql(superUserPassword, databaseName, String.Format("ALTER LOGIN [{0}] WITH PASSWORD=N'{1}'", targetUsername, newPassword.Replace("'", "''")));
            if (!String.IsNullOrEmpty(rv) && rv.Trim().Length > 0) {
                throw new InvalidOperationException(getDisplayMember("Sqlserver_ChangePassword", "Error changing password for user {0}: {1}", targetUsername, rv));
            }
            
        }

        public void EnableMixedModeIfNeeded(string instanceName) {

            _instanceName = instanceName;

            if (!IsMixedModeEnabled(_instanceName)) {

                if (String.IsNullOrEmpty(ServiceName)) {
                    ServiceName = GetServiceName(_instanceName);
                }

                // shut down sql server
                this.StopService();

                // change the registry key

                var fullInstanceName = GetFullInstanceName(_instanceName);

                // 1 = windows authentication only
                // 2 = mixed mode (windows authentication or sql server authentication)
                Toolkit.SaveRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\" + fullInstanceName + @"\MSSQLServer", "LoginMode", 2, true);

                // start up sql server
                this.StartService();

            }
        }

        private string getExeName() {
            string exe = (BinDirectory + @"\sqlcmd.exe").Replace(@"\\", @"\");

            // HACK: SQL Server doesn't report its bin path nicely in 64 bit OS, so this hack let's us get around that (i.e. it reports it in the 32 bit folder but actually resides in 64 bit folder when installed from WPI)
            if (!File.Exists(exe)) {
                exe = exe.Replace("Program Files (x86)", "Program Files");
            }
            return exe;
        }

        public override string ServerName {
            get {
                return base.ServerName;
            }
            set {
                //if (value != null) {
                //    // HACK: to get around the 'Cannot generate SSPI context' or 'local security authority could not be contacted' issues, 
                //    //       we explicitly resolve 'localhost' to '(local)' so domain membership / connectivity doesn't bite us when we try to restart
                //    //       the local sql server service
                //    //       http://blogs.msdn.com/sql_protocols/archive/2005/10/19/482782.aspx
                //    //       http://sqldbpool.wordpress.com/2008/09/05/%E2%80%9Ccannot-generate-sspi-context%E2%80%9D-error-message-more-comments-for-sql-server/
                //    value = value.Replace("localhost", "(local)");
                //}
                base.ServerName = value;
            }
        }

        protected override void getArgs(string userName, string databaseName, string inputFileName, out string exe, out string args) {

            exe = getExeName();

            var instance = GetPartialInstanceName(_instanceName);
            var server = ServerName;
            if (server.Contains(@"\")) {
                var s = server.Split('\\');
                server = s[0];
                instance = s[1];
            }

            var dbSwitch = "master ";
            if (!String.IsNullOrEmpty(databaseName)) {
                dbSwitch = databaseName + " ";
            }
            
            if (UseWindowsAuthentication) {
                // using windows auth, ignore username and pass -E for integrated security connection
                if (String.IsNullOrEmpty(instance)) {
                    args = String.Format(@" -E -S {0} -d {2} ", server, "", dbSwitch);
                } else {
                    args = String.Format(@" -E -S {0}\{1} -d {2} ", server, instance, dbSwitch);
                }
            } else {
                if (String.IsNullOrEmpty(instance)) {
                    args = String.Format(@" -U ""{0}"" -S {1} -d {3} ", ("" + userName).Replace(@"""", @"\"""), server, "", dbSwitch);
                } else {
                    args = String.Format(@" -U ""{0}"" -S {1}\{2} -d {3} ", ("" + userName).Replace(@"""", @"\"""), server, instance, dbSwitch);
                }
            }
            if (!String.IsNullOrEmpty(inputFileName)) {
                args += @" -i """ + inputFileName.Replace(@"""", @"\""") + @"""";
            }
        }

        public override string ExecuteSql(string userName, string password, string databaseName, string sql) {
            string exe = null, args = null;

            if (String.IsNullOrEmpty(password)) {
                // empty password, assume they're doing integrated authority.
                // we tell getArgs this by passing a null userName.
                userName = null;
                UseWindowsAuthentication = true;
            }

            getArgs(userName, databaseName, null, out exe, out args);
            args += @" -Q """ + ("" + sql).Replace(@"""", @"""""") + @"""";


            if (!File.Exists(exe)) {
                // db engine probably not installed locally.
                // use a 'normal' .net db connection to run the sql
                //EventLog.WriteEntry("GRIN-Global Database", Toolkit.Cut("Using .NET connection to execute sql: " + sql, 0, 8000), EventLogEntryType.Information);
                using (var dm = DataManager.Create(GetDataConnectionSpec(databaseName, userName, password))) {
                    dm.Write(sql);
                    return "";
                }
            } else {
                // db engine is installed locally, use the command line program to do the heavy lifting
                return exec(exe, args, password);
            }

        }

        public override string ExecuteSqlFile(string userName, string password, string databaseName, string inputFileName) {
            if (UseWindowsAuthentication || (userName == SuperUserName && String.IsNullOrEmpty(password))) {
                // assume windows authentication (by passing null username/password
                return base.ExecuteSqlFile(null, null, databaseName, inputFileName);
            } else {
                return base.ExecuteSqlFile(userName, password, databaseName, inputFileName);
            }
        }

        public override string TestLogin(string userName, string password) {
            //if (!File.Exists(getExeName())) {
                using (var dm = DataManager.Create(this.GetDataConnectionSpec(null, userName, password))) {
                    return dm.TestLogin();
                }
            //} else {
            //    string ver = GetVersion(userName, password).ToLower();
            //    if (ver.Contains("failed") || ver.Contains("error") || ver.Contains("exception") || ver.Contains("sspi") || ver.Contains("local security authority")) {
            //        return ver;
            //    } else if (ver.Trim() == "password:"){
            //        return "error: no password specified";
            //    } else {
            //        return null;
            //    }
            //}
        }
        public override string  GetVersion(string userName, string password){
            return ExecuteSql(userName, password, null, "select @@VERSION;");
        }

        public override DataConnectionSpec GetDataConnectionSpec(string databaseName, string userName, string password) {
            DataConnectionSpec dcs = new DataConnectionSpec { 
                ProviderName = EngineName, 
                DatabaseName = databaseName, 
                InstanceName = _instanceName, 
                ServerName = ServerName, 
                Port = Port,
                UseWindowsAuthentication = UseWindowsAuthentication,
                UserName = userName,
                Password = password
            };
            return dcs;

//            dcs.ProviderName = EngineName;

//            string initialCatalog = (String.IsNullOrEmpty(databaseName) ? "" : "Initial Catalog=" + databaseName + ";");

//            var instanceName = GetPartialInstanceName(_instanceName);

//            string separator = null;
//            if (String.IsNullOrEmpty(instanceName)) {
//                // default instance, no separator
//                separator = "";
//            } else {
//                if (Toolkit.ToInt32(instanceName, -1) > -1) {
//                    // using port, need a comma
//                    separator = ", ";
//                } else {
//                    // using instance name, need a backslash
//                    separator = @"\";
//                }
//            }

//            dcs.UseWindowsAuthentication = UseWindowsAuthentication;
//            if (UseWindowsAuthentication) {
//                // assume integrated security -- either null user or specified SA and empty password.
////                dcs.ConnectionString = String.Format(@"Data Source={0}" + separator + @"{1};{2}Integrated Security=SSPI;", ServerName, instanceName, initialCatalog);
//                dcs.ConnectionString = String.Format(@"Data Source={0}" + separator + @"{1};{2}Trusted_Connection=True;", ServerName, instanceName, initialCatalog);
//            } else {
//                dcs.ConnectionString = String.Format(@"Data Source={0}" + separator + @"{1};{2}User Id='{3}';Password='{4}';", ServerName, instanceName, initialCatalog, ("" + userName).Replace("'", "''"), ("" + password).Replace("'", "''"));
//            }
//            return dcs;
        }


        protected override string checkForError(string sqlOutput) {
            if (!String.IsNullOrEmpty(sqlOutput)) {
                string output = sqlOutput.ToLower();
                if (output.StartsWith("msg ") || output.Contains("error") || output.Contains("failed") || output.Contains("failure") || output.Contains("invalid") || output.Contains("denied")) {
                    throw new InvalidOperationException(getDisplayMember("Sqlserver_checkForError", "SQL Returned error: {0}", sqlOutput));
                }
            }

            return sqlOutput;
        }

        public override string DropUser(string superUserPassword, string databaseName, string userName, string serverName) {

            if (UseWindowsAuthentication) {
                var sql = String.Format(@"
drop user {0};
drop login [{1}\{0}];
", userName, serverName);

                return ExecuteSql(superUserPassword, databaseName, sql);
            } else {
                var sql = String.Format(@"
drop user {0};
drop login {0};
", userName);

                return ExecuteSql(superUserPassword, databaseName, sql);
            }
        }

        public override string CreateUser(string superUserPassword, string databaseName, string userName, string serverName, string userPassword, bool readOnlyRights, bool usingWindowsAuthentication) {

            var sql = "";
            var sqlUserName = userName.Replace(" ", "");

            if (usingWindowsAuthentication) {


                sql = String.Format(@"
create login [{1}\{0}] from windows;
create user {2} for login [{1}\{0}];
exec sp_addrolemember 'db_datareader', '{0}';
exec sp_addrolemember 'db_datawriter', '{0}';
", userName, serverName, sqlUserName);

                if (readOnlyRights) {
                    sql += String.Format(@"
exec sp_MSforeachtable 'grant select on {1}.? to {0};';
", sqlUserName, databaseName);
                } else {
                    sql += String.Format(@"
exec sp_MSforeachtable 'grant select, insert, update, delete on {1}.? to {0};';
", sqlUserName, databaseName);
                }

            } else {

                sql = String.Format(@"
create login {0} WITH PASSWORD = '{1}';
create user {2} for login {0};
exec sp_addrolemember 'db_datareader', '{0}';
exec sp_addrolemember 'db_datawriter', '{0}';
", userName, ("" + userPassword).Replace("'", "''"), sqlUserName);

                if (readOnlyRights) {
                    sql += String.Format(@"
exec sp_MSforeachtable 'grant select on {1}.? to {0};';
", sqlUserName, databaseName);
                } else {
                    sql += String.Format(@"
exec sp_MSforeachtable 'grant select, insert, update, delete on {1}.? to {0};';
", sqlUserName, databaseName);
                }

            }

            return ExecuteSql(superUserPassword, databaseName, sql);

        
        }

        public override string GrantRightsToTable(string superUserPassword, string databaseName, string userName, string serverName, string tableName, bool readOnlyRights) {
            throw new NotImplementedException();
        }

        public override string DropDatabase(string superUserPassword, string databaseName) {
            var sql = "ALTER DATABASE " + databaseName + " SET OFFLINE WITH ROLLBACK IMMEDIATE; DROP DATABASE " + databaseName + ";";
            var output = ExecuteSql(superUserPassword, null, sql);
            return output;
        }

        public override List<string> ListDatabases(string superUserPassword) {
            var output = ExecuteSql(superUserPassword, null, "sp_databases");
            var ret = new List<string>();
            // TODO: parse database list
            return ret;
        }

    }
}
