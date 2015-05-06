using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace GrinGlobal.Core.DataManagers {
    public class SqliteEngineUtil : DatabaseEngineUtil {

        internal SqliteEngineUtil(string fullPathToUACExe)
            : base("sqlite", "~/", "~/", "~/", null, "SQLite 3 or greater", null, fullPathToUACExe, 0){
        }

        private string getExeName() {
            string exe = @"""" + BinDirectory + @"sqlite3.exe"" ";
            return exe;
        }

        protected override void getArgs(string userName, string databaseName, string inputFileName, out string exe, out string args) {
            exe = getExeName();
            var dbSwitch = "";
            if (!String.IsNullOrEmpty(databaseName)) {
                dbSwitch = databaseName + ".db";
            }
            args = String.Format(@" {0} ", dbSwitch);
        }

        public override string ExecuteSql(string userName, string password, string databaseName, string sql) {
            string exe = null, args = null;
            getArgs(userName, databaseName, null, out exe, out args);
            args += @" """ + sql.Replace(@"""", @"\""") + @"""";

            if (!File.Exists(exe)) {
                // db engine probably not installed locally.
                // use a 'normal' .net db connection to run the sql
                var dbSwitch = "";
                if (!String.IsNullOrEmpty(databaseName)) {
                    dbSwitch = databaseName + ".db";
                }
                using (var dm = DataManager.Create(GetDataConnectionSpec(dbSwitch, userName, password))) {
                    dm.Write(sql);
                    return "";
                }
            } else {
                // db engine is installed locally, use the command line program to do the heavy lifting
                return exec(exe, args, null);
            }
        }

        public override string TestLogin(string userName, string password) {
            if (!File.Exists(getExeName())) {
                using (var dm = DataManager.Create(this.GetDataConnectionSpec(null, userName, password))) {
                    return dm.TestLogin();
                }
            } else {
                string ver = GetVersion(userName, password).ToLower();
                if (ver.ToLower().Contains("error")) {
                    return ver;
                } else {
                    return null;
                }
            }
        }
        public override string GetVersion(string userName, string password) {
            return ExecuteSql(userName, password, null, "select sqlite_version();");
        }

        public override DataConnectionSpec GetDataConnectionSpec(string databaseName, string userName, string password) {
            DataConnectionSpec dcs = new DataConnectionSpec {
                ProviderName = EngineName,
                DatabaseName = databaseName, 
                InstanceName = "",
                ServerName = ServerName,
                ServiceName = ServiceName,
                Port = Port,
                UseWindowsAuthentication = UseWindowsAuthentication,
                UserName = userName, 
                Password = password, 
            };
            return dcs;
        }


        protected override string checkForError(string sqlOutput) {
            if (!String.IsNullOrEmpty(sqlOutput)) {
                string output = sqlOutput.ToLower();
                if (output.Contains("error") || output.Contains("failed") || output.Contains("failure") || output.Contains("invalid") || output.Contains("denied")) {
                    throw new InvalidOperationException(getDisplayMember("Sqlite_checkForError", "SQL Returned error: {0}", sqlOutput));
                }
            }

            return sqlOutput;
        }

        public override string DropUser(string superUserPassword, string databaseName, string userName, string serverName) {
            // sqlite has no concept of users
            return null;
        }

        public override string CreateUser(string superUserPassword, string databaseName, string userName, string serverName, string userPassword, bool readOnlyRights, bool usingWindowsAuthentication) {
            // sqlite has no concept of users
            return null;
        }

        public override string GrantRightsToTable(string superUserPassword, string databaseName, string userName, string serverName, string tableName, bool readOnlyRights) {
            // sqlite has no concept of rights
            return null;
        }

        public override List<string> ListDatabases(string superUserPassword) {
            // sqlite has no concept of multiple databases
            return new List<string>();
        }

        public override string DropDatabase(string superUserPassword, string databaseName) {
            // sqlite has no concept of multiple databases, so dropping one makes no sense
            return "";
        }

        public override string CreateDatabase(string superUserPassword, string databaseName) {
            // sqlite has no concept of multiple databases, so creating one makes no sense
            return "";
        }
    }
}
