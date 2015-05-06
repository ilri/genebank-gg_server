using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace GrinGlobal.Core.DataManagers {
    public class OracleEngineUtil : DatabaseEngineUtil {

        internal OracleEngineUtil(string fullPathToUACExe, string sid)
                    : base("oracle", 
                    getOracleBasePath() + @"\BIN", 
                    getOracleBasePath(), 
                    getOracleBasePath(), 
                    "OracleServiceXE",
                    "Oracle Express", 
                    "SYS", 
                    fullPathToUACExe, 
                    1521) {
            try {
                SID = sid;
                Version = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\ORACLE\KEY_XE", "VERSION", "") as string;
            } catch (Exception ex) {
                Debug.WriteLine("error reading reg keys for oracle: " + ex.Message);
            }
        }

        private static string getOracleBasePath() {
            try {
                //return Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\ORACLE\KEY_XE", "ORACLE_HOME", "") as string;
                string basePath = "";
                // First try to get the ORACLE_HOME assuming the Oracle XE database is installed...
                basePath = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\ORACLE\KEY_XE", "ORACLE_HOME", "");
                // If Oracle XE is not installed try to get the ORACLE_HOME KEY straight from the HKEY_LOCAL_MACHINE\SOFTWARE\ORACLE hive (this *might* be true if only one instance is installed - but newer Oracle version no longer do it this way)...
                if (string.IsNullOrEmpty(basePath))
                {
                    basePath = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\ORACLE", "ORACLE_HOME", "");
                }
                // If Oracle XE is not installed, the Oracle install is a *newer* version, or there is more than one Oracle database instance installed...
                if (string.IsNullOrEmpty(basePath))
                {
                    string [] oracleSubKeys = {};
                    // Each install of Oracle will have a separate KEY in the HKEY_LOCAL_MACHINE\SOFTWARE\ORACLE hive (that includes the word 'home')...
                    oracleSubKeys = Toolkit.GetRegSubKeys(@"HKEY_LOCAL_MACHINE\SOFTWARE\ORACLE");
                    foreach(string oracleKey in oracleSubKeys)
                    {
                        // Find the first HOME_ORACLE in the HKEY_LOCAL_MACHINE\SOFTWARE\ORACLE hive...
                        if (string.IsNullOrEmpty(basePath) && oracleKey.ToUpper().Contains("HOME"))
                        {
                            // This will return an empty string if this KEY does not have an ORACLE_HOME entry...
                            basePath = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\ORACLE\" + oracleKey, "ORACLE_HOME", "");
                        }
                    }
                }
                return basePath;
            } catch (Exception ex) {
                Debug.WriteLine("error reading oracle base path reg key: " + ex.Message);
                return "";
            }
        }

        private string getExeName() {
            string exe = (BinDirectory + @"\sqlplus.exe").Replace(@"\\", @"\");
            if (String.IsNullOrEmpty(getOracleBasePath())) {
                // assume it's on the path variable
                return "sqlplus";
            }
            return exe;
        }
        public override string DropDatabase(string superUserPassword, string databaseName) {
            return ExecuteSql(superUserPassword, null, "DROP USER " + databaseName + " CASCADE;");
        }
        public override string CreateDatabase(string superUserPassword, string databaseName) {
            // in Oracle XE, only 1 database is allowed.
            // however, each user created gets their own schema.
            // so we simply create a user named the database.
            //var sql = "CREATE USER " + databaseName + @" IDENTIFIED BY TempPA55 DEFAULT TABLESPACE users TEMPORARY TABLESPACE temp QUOTA UNLIMITED ON users;";
            var sql = "CREATE USER " + databaseName + @" IDENTIFIED BY TempPA55 DEFAULT TABLESPACE users TEMPORARY TABLESPACE temp;";
            var output = ExecuteSql(superUserPassword, null, sql);
            // logInfo("Created user " + databaseName + " results: " + output);

            // this user is the same as the database name, so allow him to do anything...
            var output2 = ExecuteSql(superUserPassword, null, @"ALTER DATABASE CHARACTER SET UTF16;");
            output += output2;
            // logInfo("Changed character set to utf16 results: " + output2);

            var output3 = ExecuteSql(superUserPassword, null, @"GRANT unlimited tablespace TO " + databaseName + ";");
            output += output3;
            // logInfo("granting user " + databaseName + " unlimited tablespace " + databaseName + " results: " + output3);

            // this user is the same as the database name, so allow him to do anything...
            //var output4 = ExecuteSql(superUserPassword, null, @"GRANT CREATE session, CREATE any table, ALTER any table, DROP any table, CREATE any sequence, CREATE any trigger, CREATE any index TO " + databaseName + ";");
            var output4 = ExecuteSql(superUserPassword, null, @"GRANT connect, resource TO " + databaseName + ";");
            output += output4;
            // logInfo("granting user " + databaseName + " all basic rights to " + databaseName + " results: " + output4);

            
            return output;
        }

        protected void logInfo(string msg) {
            EventLog.WriteEntry("OracleEngineUtil", msg, EventLogEntryType.Information);
        }

        protected override void getArgs(string userName, string databaseName, string inputFileName, out string exe, out string args) {
            exe = getExeName();

            if (userName.ToLower() == "sys") {
                args = @" -L -S SYS AS SYSDBA ";
            } else {
                args = String.Format(@" -L -S {0} ", userName.Replace(@"""", @"\"""));
            }
            if (!String.IsNullOrEmpty(inputFileName)) {
                args += @" @""" + inputFileName.Replace(@"""", @"\""") + @"""";
            }
        }

        private string _tempFile = Toolkit.ResolveFilePath(Toolkit.GetTempDirectory(1500) + @"\sqlplus_temp.sql", true);

        public override string ExecuteSql(string userName, string password, string databaseName, string sql) {
            string exe = null, args = null;
            
            try {
                if (File.Exists(_tempFile)){
                    File.Delete(_tempFile);
                }
            } catch { }

            File.WriteAllText(_tempFile, sql + "\r\nEXIT;");
            getArgs(userName, databaseName , _tempFile, out exe, out args);

            if (!File.Exists(exe)) {
                // db engine probably not installed locally.
                // use a 'normal' .net db connection to run the sql
                using (var dm = DataManager.Create(GetDataConnectionSpec(databaseName, userName, password))) {
                    dm.Write(sql);
                    return "";
                }
            } else {
                // db engine is installed locally, use the command line program to do the heavy lifting
                return exec(exe, args, password);
            }
        }

        public override string TestLogin(string userName, string password) {
            if (!File.Exists(getExeName())) {
                // use conn string as last resort because OCI doesn't support AS SYSDBA logins, which we'll need to create users / tables / indexes / etc.
                using (var dm = DataManager.Create(this.GetDataConnectionSpec(null, userName, password))) {
                    return dm.TestLogin();
                }
            } else {
                string ver = GetVersion(userName, password);
                if (ver.ToLower().Contains("logon denied")) {
                    return ver;
                } else {
                    return null;
                }
            }
        }
        public override string GetVersion(string userName, string password) {
            return ExecuteSql(userName, password, null, "select * from v$version;");
        }

        public override string GrantRightsToTable(string superUserPassword, string databaseName, string userName, string serverName, string tableName, bool readOnlyRights) {
            var output = ExecuteSql(superUserPassword, databaseName, "GRANT SELECT, UPDATE, INSERT, DELETE ON " + databaseName + "." + tableName + " TO " + userName + ";");
            output += ExecuteSql(superUserPassword, databaseName, "GRANT SELECT, UPDATE ON " + databaseName + ".sq_" + tableName + " TO " + userName + ";");
            return output;
        }

        public override DataConnectionSpec GetDataConnectionSpec(string databaseName, string userName, string password) {
            DataConnectionSpec dcs = new DataConnectionSpec {
                ProviderName = EngineName,
                DatabaseName = databaseName,
                InstanceName = "",
                SID = this.SID,
                ServerName = ServerName,
                ServiceName = ServiceName,
                Port = Port,
                UseWindowsAuthentication = UseWindowsAuthentication,
                UserName = userName,
                Password = password,
            };
            dcs.ConnectionString = dcs.RecalculateConnectionString();
            return dcs;
        }


        protected override string checkForError(string sqlOutput) {
            if (!String.IsNullOrEmpty(sqlOutput)) {
                string output = sqlOutput.ToLower();
                if (output.Contains("error") || output.Contains("failed") || output.Contains("failure") || output.Contains("invalid") || output.Contains("denied")) {
                    throw new InvalidOperationException(getDisplayMember("Oracle_checkForError", "SQL Returned error: {0}", sqlOutput));
                }
            }

            return sqlOutput;
        }

        public override string DropUser(string superUserPassword, string databaseName, string userName, string serverName) {

            if (("" + databaseName).ToLower() != ("" + userName).ToLower()) {
                // only drop the user if their name is not the same as the database name
                var sql = "DROP USER " + userName + " CASCADE;";
                var output = ExecuteSql(superUserPassword, null, sql);
                // logInfo("Dropped user " + userName + " results: " + output);
                return output;
            }
            return null;
        }

        private string assignDefaultSchema(string superUserPassword, string databaseName, string userName) {

            if (("" + databaseName).ToLower() == ("" + userName).ToLower()) {
                // user is same as database -- no need to change the default schema as they will be the same
                return "";
            } else {
                // we need to change the user's default schema on every login
                var sql = String.Format(@"CREATE OR REPLACE TRIGGER {0}.TG_AFTER_LOGIN AFTER LOGON ON {0}.SCHEMA 
BEGIN 
    DBMS_APPLICATION_INFO.set_module(USER, 'Initialized'); 
    EXECUTE IMMEDIATE 'ALTER SESSION SET current_schema={1}'; 
END;
/", userName.ToUpper(), databaseName.ToUpper());
                return ExecuteSql(superUserPassword, databaseName, sql);
            }
        }

        public override string CreateUser(string superUserPassword, string databaseName, string userName, string serverName, string userPassword, bool readOnlyRights, bool usingWindowsAuthentication) {


            // In Oracle XE a 'user' is our schema.
            // So here we don't want to create a new user, we simply want to give GRINGLOBAL the ability to connect.


            // HACK: if they're creating a user that is different than the database name, create a new user with their default tablespace being the database name.
            if (("" + databaseName).ToLower() != ("" + userName).ToLower()) {
                var sql = "CREATE USER " + userName + @" IDENTIFIED BY TempPA55 DEFAULT TABLESPACE users;";
                var output = ExecuteSql(superUserPassword, null, sql);

                // logInfo("Created user " + userName + " with default tablespace " + databaseName + " results: " + output);

                // since this is not the same user as the schema name, we only grant the ability to login (table rights are done separately)
                var output2 = ExecuteSql(superUserPassword, null, "GRANT CREATE Session TO " + userName + ";");
                output += output2;
                // logInfo("Granted user " + userName + " login rights results: " + output2);

                var output3 = ExecuteSql(superUserPassword, null, "ALTER USER " + userName + @" IDENTIFIED BY """ + userPassword + @""";");
                output += output3;
                // logInfo("changed user " + userName + " password results: " + output3);

                // create a trigger to auto-switch the user's schema after login to the databaseName given.
                var output4 = assignDefaultSchema(superUserPassword, databaseName, userName);
                output += output4;
                // logInfo("Assigned user " + userName + " default schema to " + databaseName + " results: " + output3);

                return output;
            
            } else {

                // this user is the same as the database name, so he is already created. just change his password.
                var output = ExecuteSql(superUserPassword, null, "ALTER USER " + userName + @" IDENTIFIED BY """ + userPassword + @""";");
                // logInfo("changed user " + userName + " password results: " + output);
                return output;

            }


        }

        public string RestartSequenceForTable(string superUserPassword, string userName, string userPassword, string tableName, string pkFieldName) {
            var dsc = this.GetDataConnectionSpec(userName, userName, userPassword);
            int currentMaxID = 0;
            try {
                using (var dm = DataManager.Create(dsc)) {
                    currentMaxID = Toolkit.ToInt32(dm.ReadValue(String.Format("select max({0}) from {1}", pkFieldName, tableName)), 0);
                }
            } catch (Exception ex) {
                // if the pk field does not follow our naming convention or it is not an int we just ignore!
                return String.Format("Error running 'select max({0}_id) from {0}': {1}", tableName, ex.Message);
            }

            if (currentMaxID < 1) {
                return "Success - zero records, did not drop then create sequence";
            } else {

                var sql = String.Format(@"DROP SEQUENCE {0}.SQ_{1};", userName.ToUpper(), tableName.ToUpper());

                var output = ExecuteSql(superUserPassword, userName, sql);

                sql = String.Format(@"CREATE SEQUENCE {0}.SQ_{1} MINVALUE 1 START WITH {2} INCREMENT BY 1 NOCACHE;", userName.ToUpper(), tableName.ToUpper(), (currentMaxID + 1));

                output += ExecuteSql(superUserPassword, userName, sql);

                return output;
            }

        }


        public override List<string> ListDatabases(string superUserPassword) {
            var output = ExecuteSql(superUserPassword, null, "select * from user_tablespaces;");
            var ret = new List<string>();
            // TODO: parse database list
            return ret;
        }
    }
}
