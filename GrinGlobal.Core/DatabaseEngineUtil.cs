using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.ServiceProcess;

using GrinGlobal.Core.DataManagers;
using System.Threading;

namespace GrinGlobal.Core {

    public abstract class DatabaseEngineUtil {

        public static bool IsValidEngine(string engine) {
            if (String.IsNullOrEmpty(engine)) {
                return false; 
            } else {
                switch (engine.ToLower().Trim()) {
                    case "mysql":
                    case "postgresql":
                    case "sqlserver":
                    case "oracle":
                        return true;
                    default:
                        return false;
                }
            }
        }

        private string _engine;
        public string EngineName {
            get {
                return _engine;
            }
            set {
                _engine = ("" + value).ToLower();
            }
        }
        public virtual string ServerName { get; set; }
        public virtual string SID { get; set; }

        public int Port { get; set; }

        public DatabaseEngineUtil(string engineName, string binDirectory, string baseDirectory, string dataDirectory, string serviceName, string friendlyName, string superUserName, string fullPathToUACExe, int port) {
            this.EngineName = engineName;
            this.BinDirectory = binDirectory;
            this.BaseDirectory = baseDirectory;
            this.DataDirectory = dataDirectory;
            this.ServiceName = serviceName;
            this.FriendlyName = friendlyName;
            this.SuperUserName = superUserName;
            this.FullPathToUACExe = fullPathToUACExe;
            this.Port = port;
            this.ServerName = "localhost";
            this.SID = "";
        }

        public static DatabaseEngineUtil CreateInstance(string engine, string fullPathToUACExe, string preferredInstanceName) {
            if (String.IsNullOrEmpty(engine)) {
                return null;
            } else {
                switch (engine.ToLower().Trim()) {
                    case "mysql":
                        return new MySqlEngineUtil(fullPathToUACExe);
                    case "pgsql":
                    case "postgresql":
                        return new PostgreSqlEngineUtil(fullPathToUACExe);
                    case "sql":
                    case "sqlclient":
                    case "sqlserver":
                    case "mssql":
                        return new SqlServerEngineUtil(fullPathToUACExe, preferredInstanceName);
                    case "ora":
                    case "oracle":
                    case "oracleclient":
                        return new OracleEngineUtil(fullPathToUACExe, preferredInstanceName);
                    case "sqlite":
                    case "sqlite3":
                        return new SqliteEngineUtil(fullPathToUACExe);
                    default:
                        throw new InvalidOperationException( getDisplayMember("CreateInstance", "Database engine '{0}' is not a valid engine type.  Must be 'mysql', 'sqlserver', 'oracle', or 'postgresql'.", engine));
                }
            }
        }

        public virtual string BinDirectory { get; protected set; }

        public virtual string BaseDirectory { get; protected set; }

        public virtual string DataDirectory { get; protected set; }

        public virtual string Version { get; protected set; }

        public virtual string FriendlyName { get; protected set; }

        public virtual string InstallParameter {
            get {
                return @" ENGINE=""" + EngineName + @""" ";
            }
        }

        public virtual string SuperUserName { get; protected set; }

        public virtual string FullPathToUACExe { get; protected set; }

        public virtual string FullPathToUtility64Exe { get { return this.FullPathToUACExe.Replace(@"gguac.exe", "ggutil64.exe"); } }

        public virtual string ServiceName { get; protected set; }

        public virtual void StartService() {
            using (ServiceController sc = new ServiceController(ServiceName)) {
                sc.Refresh();
                if (sc.Status != ServiceControllerStatus.Running && sc.Status != ServiceControllerStatus.StartPending) {
                    sc.Start();
                }
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30.0f));
            }
        }

        public virtual void StopService() {
            using (ServiceController sc = new ServiceController(ServiceName)) {
                sc.Refresh();
                if (sc.Status != ServiceControllerStatus.Stopped && sc.Status != ServiceControllerStatus.StopPending) {
                    sc.Stop();
                }
                sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30.0f));
            }
        }

        protected abstract void getArgs(string userName, string databaseName, string inputFileName, out string exe, out string args);

        /// <summary>
        /// Calls given exe with given args.  If standardInText is null, standard input is not redirected.  If standardInText is non-null (including zero-length), standard input is redirected.  To pass an empty password as standard in, you must pass null for this parameter if the exe would normally prompt the user for it.
        /// </summary>
        /// <param name="exe"></param>
        /// <param name="args"></param>
        /// <param name="standardInText"></param>
        /// <returns></returns>
        protected virtual string exec(string exe, string args, string standardInText) {

            //if (Toolkit.IsWindowsVistaOrLater) {
            //    // run all sql as administrator on vista / windows 7
            //    if (exe.Contains(" ") && !exe.StartsWith(@"""")) {
            //        exe = @"""" + exe + @"""";
            //    }
            //    args = " /wait /hide " + exe + " " + args;
            //    exe = this.fullPathToUACExe.Replace(@"\\", @"\");
            //}

            ProcessStartInfo psi = new ProcessStartInfo(exe, args);
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
#if DEBUGDBENGINEUTIL
            psi.CreateNoWindow = false;
            psi.WindowStyle = ProcessWindowStyle.Normal;
#else
            psi.CreateNoWindow = true;
#endif

            Process p = null;

            //EventLog.WriteEntry("GRIN-Global Database", Toolkit.Cut("Running " + psi.FileName + " " + psi.Arguments + "\n" + standardInText, 0, 8000), EventLogEntryType.Information);

            if (standardInText == null) {
                p = Process.Start(psi);
            } else {
                psi.RedirectStandardInput = true;
                p = Process.Start(psi);
                p.StandardInput.WriteLine(standardInText);
            }

            string output = p.StandardOutput.ReadToEnd() + " " + p.StandardError.ReadToEnd();
            //EventLog.WriteEntry("GRIN-Global Database", Toolkit.Cut("Result: " + output + "...", 0, 8000), EventLogEntryType.Information);
            return output;
        }

        public virtual string CreateDatabase(string superUserPassword, string databaseName) {
            var sql = "create database " + databaseName + ";";
            return ExecuteSql(SuperUserName, superUserPassword, null, sql);
        }

        public virtual string DropDatabase(string superUserPassword, string databaseName) {
            var sql = "drop database " + databaseName + ";";
            return ExecuteSql(SuperUserName, superUserPassword, null, sql);
        }

        public virtual List<string> ListDatabases(string superUserPassword) {
            throw new NotImplementedException(getDisplayMember("ListDatabases", "ListDatabases is not implemented for this type of DatabaseEngineUtil."));
        }

        public virtual string ExecuteSql(string superUserPassword, string databaseName, string sql) {
            return ExecuteSql(SuperUserName, superUserPassword, databaseName, sql);
        }
        public abstract string ExecuteSql(string userName, string password, string databaseName, string sql);

        public virtual string ExecuteSqlFile(string superUserPassword, string databaseName, string inputFileName) {
            return ExecuteSqlFile(SuperUserName, superUserPassword, databaseName, inputFileName);
        }

        public abstract string GrantRightsToTable(string superUserPassword, string databaseName, string userName, string serverName, string tableName, bool readOnlyRights);

        public virtual string ExecuteSqlFile(string userName, string password, string databaseName, string inputFileName) {

            string exe = null, args = null;
            getArgs(userName, databaseName, inputFileName, out exe, out args);

            try {
                return checkForError(exec(exe, args, password));
            } catch (Exception ex) {
                throw new InvalidOperationException(ex.Message + "\nCommand line: " + exe + " " + args + "\nRan as user: " + Environment.UserDomainName + @"\" + Environment.UserName);
            }

        }

        protected abstract string checkForError(string sqlOutput);

        public virtual string TestLogin(string superUserPassword) {
            return TestLogin(SuperUserName, superUserPassword);
        }
        public abstract string TestLogin(string userName, string password);

        public abstract string GetVersion(string userName, string password);

        public bool UseWindowsAuthentication { get; set; }

        public virtual string GetSettings() {
            throw new NotImplementedException();
        }

        public abstract DataConnectionSpec GetDataConnectionSpec(string databaseName, string userName, string password);


        public abstract string DropUser(string superUserPassword, string databaseName, string userName, string serverName);

        public abstract string CreateUser(string superUserPassword, string databaseName, string userName, string serverName, string userPassword, bool readOnlyRights, bool usingWindowsAuthentication);

        protected static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "Core", "DatabaseEngineUtil", resourceName, null, defaultValue, substitutes);
        }
    }
}
