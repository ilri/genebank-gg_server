using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace GrinGlobal.Core.DataManagers {
    public class PostgreSqlEngineUtil : DatabaseEngineUtil {

        internal PostgreSqlEngineUtil(string fullPathToUACExe)
            : base("postgresql", null, null, null, null, null, null, fullPathToUACExe, 5432) {

            try {
                var regPaths = Toolkit.GetRegSubKeys(@"HKEY_LOCAL_MACHINE\SOFTWARE\PostgreSQL\Installations");
                foreach (var key in regPaths) {
                    if (key.ToLower().StartsWith("postgresql")) {

                        BaseDirectory = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\PostgreSQL\Installations\" + key, "Base Directory", "") as string;
                        BinDirectory = BaseDirectory + @"\bin";
                        ServiceName = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\PostgreSQL\Installations\" + key, "Service ID", "") as string;
                        Version = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\PostgreSQL\Installations\" + key, "Version", "") as string;

                        DataDirectory = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\PostgreSQL\Services\" + key, "Data Directory", "") as string;
                        SuperUserName = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\PostgreSQL\Services\" + key, "Database Superuser", "") as string;
                        FriendlyName = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\PostgreSQL\Services\" + key, "Display Name", "") as string;

                    }
                }
            } catch (Exception ex) {
                Debug.WriteLine("error reading registry keys: " + ex.Message);
            }


        }

        public override string CreateDatabase(string superUserPassword, string databaseName) {
            var sql = "create database " + databaseName + " encoding = 'UTF8';";
            return ExecuteSql(SuperUserName, superUserPassword, null, sql);
        }

        private string getExeName() {
            string exe = (BinDirectory + @"\psql.exe").Replace(@"\\", @"\");
            if (String.IsNullOrEmpty(BaseDirectory)) {
                // assume it's on the path variable
                exe = "psql";
            }
            return exe;
        }

        private string _outputFile = Toolkit.ResolveFilePath(Toolkit.GetTempDirectory(1500) + @"\pgsql_output.txt", true);
        private string _batFile = Toolkit.ResolveFilePath(Toolkit.GetTempDirectory(1500) + @"\pgsql_input.bat", true);

        protected override void getArgs(string userName, string databaseName, string inputFileName, out string exe, out string args) {
            exe = getExeName();
            //args = "";
            var dbSwitch = "";
            if (!String.IsNullOrEmpty(databaseName)) {
                dbSwitch = " --dbname " + databaseName + " ";
            }
            args = String.Format(@" --host {0} --port {1} --username ""{2}"" {3} ", ServerName, Port, userName.Replace(@"""", @"\"""), dbSwitch);
            if (!String.IsNullOrEmpty(inputFileName)) {
                args += @" --file """ + inputFileName + @"""";
            }
        }

        public override string ExecuteSql(string userName, string password, string databaseName, string sql) {
            string exe = null, args = null;
            getArgs(userName, databaseName, null, out exe, out args);
            args += @" --command """ + sql.Replace(@"""", @"""""") + @"""";

            if (!File.Exists(exe)) {
                // db engine probably not installed locally.
                // use a 'normal' .net db connection to run the sql
                using (var dm = DataManager.Create(GetDataConnectionSpec(databaseName, userName, password))) {
                    dm.Write(sql);
                    return "";
                }
            } else {
                // db engine is installed locally, use the command line program to do the heavy lifting
                //EventLog.WriteEntry("GRIN-Global postgresql util", exe + " " + args, EventLogEntryType.Information);
                return exec(exe, args, databaseName, userName, password);
            }
        }

        protected string exec(string exe, string args, string databaseName, string username, string password) {

            // psql doesn't play nicely with standard output, so we tell it to always output to a file (via the --output switch in getArgs)

            var pgpassFile = Toolkit.ResolveFilePath(@"%APPDATA%\postgresql\pgpass.conf", true);
            File.WriteAllText(pgpassFile, "*:*:*:*:" + password); // String.Format(@"{0}:{1}:{2}:{3}:{4}", ServerName, "*" /*Port.ToString()*/, String.IsNullOrEmpty(databaseName) ? "postgres" : databaseName, username, password));

            ProcessStartInfo psi = new ProcessStartInfo(exe, args);
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            Process p = null;

            p = Process.Start(psi);
            string output = p.StandardOutput.ReadToEnd() + " " + p.StandardError.ReadToEnd();
            p.WaitForExit();
            p.Dispose();
            try {
                File.Delete(pgpassFile);
            } catch {
            }
            return output;

            //if (File.Exists(_outputFile)) {
            //    using (var sr = new StreamReader(new FileStream(_outputFile, FileMode.Open, FileAccess.Read, FileShare.Read))) {
            //        var output = sr.ReadToEnd();
            //        return output;
            //    }
            //} else {
            //    return null;
            //}
        }


        public override string TestLogin(string userName, string password) {
            //if (!File.Exists(getExeName())) {
                using (var dm = DataManager.Create(this.GetDataConnectionSpec(null, userName, password))) {
                    return dm.TestLogin();
                }
            //} else {
            //    string ver = GetVersion(userName, password).ToLower();
            //    if (ver.ToLower().Contains("authentication failed")) {
            //        return ver;
            //    } else {
            //        return null;
            //    }
            //}
        }
        public override string GetVersion(string userName, string password) {
            return ExecuteSql(userName, password, null, "select version();");
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
                PasswordMoniker = "password",
                UserNameMoniker = "User Id",
                UserName = userName,
                Password = password,
            };
            return dcs;
        }

        protected override string checkForError(string sqlOutput) {
            if (!String.IsNullOrEmpty(sqlOutput)) {
                string output = sqlOutput.ToLower();
                if (output.Contains("error") || output.Contains("failed") || output.Contains("failure") || output.Contains("invalid") || output.Contains("denied")) {
                    throw new InvalidOperationException(getDisplayMember("Postgresql_checkForError", "SQL Returned error: {0}", sqlOutput));
                }
            }

            return sqlOutput;
        }

        public override string DropUser(string superUserPassword, string databaseName, string userName, string serverName) {
            var sql = String.Format(@"drop user {0};", userName);

            return ExecuteSql(superUserPassword, databaseName, sql);
        }

        public override string CreateUser(string superUserPassword, string databaseName, string userName, string serverName, string userPassword, bool readOnlyRights, bool usingWindowsAuthentication) {
            var sql = String.Format(@"create user {0} with PASSWORD '{1}';", userName, userPassword);

            return ExecuteSql(superUserPassword, databaseName, sql);

        }

        public override string GrantRightsToTable(string superUserPassword, string databaseName, string userName, string serverName, string tableName, bool readOnlyRights) {
            var sql = "";
            if (readOnlyRights) {
                sql = String.Format(@"GRANT SELECT ON TABLE {1} to {0};", userName, tableName);
                return ExecuteSql(superUserPassword, databaseName, sql);

            } else {
                sql = String.Format(@"GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE {1} to {0};", userName, tableName);
                var output = ExecuteSql(superUserPassword, databaseName, sql);

                // also allow them rights to the corresponding sequence as well (for generating new id's on insert)
                sql = String.Format(@"GRANT SELECT, UPDATE ON SEQUENCE {1} to {0};", userName, "sq_" + tableName);
                output += ExecuteSql(superUserPassword, databaseName, sql);
                return output;

            }


        }

        public string RestartSequenceForTable(string superUserPassword, string databaseName, string serverName, string tableName, string pkFieldName) {
            var dsc = this.GetDataConnectionSpec(databaseName, this.SuperUserName, superUserPassword);
            int currentMaxID = 0;
            try {
                using(var dm = DataManager.Create(dsc)){
                    currentMaxID = Toolkit.ToInt32(dm.ReadValue(String.Format("select max({0}) from {1}", pkFieldName, tableName)), 0);
                }
            } catch (Exception ex){
                // if the pk field does not follow our naming convention or it is not an int we just ignore!
                return "Error: " + ex.Message;
            }

            var sql = String.Format(@"ALTER SEQUENCE sq_{0} RESTART WITH {1};", tableName, (currentMaxID + 1).ToString());
            return ExecuteSql(superUserPassword, databaseName, sql);

        }

        public override List<string> ListDatabases(string superUserPassword) {
            var output = ExecuteSql(superUserPassword, null, "select datname from pg_database;");
            var ret = new List<string>();
            // TODO: parse database list
            return ret;
        }
    }
}
