using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace GrinGlobal.Core.DataManagers {
    public class MySqlEngineUtil : DatabaseEngineUtil {

        internal MySqlEngineUtil(string fullPathToUACExe)
            : base("mysql", null, null, null, "MySQL", "MySQL", "root", fullPathToUACExe, 3306){

            if (!Toolkit.IsNonWindowsOS) {
                try {
                    var regPaths = Toolkit.GetRegSubKeys(@"HKEY_LOCAL_MACHINE\SOFTWARE\MySQL AB");
                    foreach (var key in regPaths) {
                        if (key.ToLower().StartsWith("mysql server")) {
                            BaseDirectory = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\MySQL AB\" + key, "Location", "") as string;
                            BinDirectory = (BaseDirectory + @"\bin\").Replace(@"\\", @"\");
                            DataDirectory = (Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\MySQL AB\" + key, "DataLocation", "") as string + @"\data\").Replace(@"\\", @"\");
                            Version = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\MySQL AB\" + key, "Version", "") as string;
                        }
                    }
                } catch (Exception ex) {
                    Debug.WriteLine("Could not read reg keys for mysql: " + ex.Message);
                    // registry error occurred, ignore / continue
                }
            }
        }

        public override string CreateDatabase(string superUserPassword, string databaseName) {
            var sql = "create database " + databaseName + " CHARACTER SET utf8 COLLATE utf8_unicode_ci";
            return ExecuteSql(superUserPassword, null, sql);
        }

        private string getExeName() {
            if (Toolkit.IsNonWindowsOS) {
                return "mysql";
            } else {
                string exe = (BinDirectory + @"\mysql.exe").Replace(@"\\", @"\");
                return exe;
            }
        }

        protected override void getArgs(string userName, string databaseName, string inputFileName, out string exe, out string args) {
            exe = getExeName();

            var dbSwitch = "";
            if (!String.IsNullOrEmpty(databaseName)) {
                dbSwitch = " --database " + databaseName + " ";
            }
            if (Toolkit.IsNonWindowsOS) {
                args = String.Format(@" --user='{0}' --password='__PASSWORD__' --host={1} --port={2} --force {3} ", ("" + userName).Replace(@"'", @"\'"), ServerName, Port, dbSwitch);
            } else {
                args = String.Format(@" --user=""{0}"" --password=""__PASSWORD__"" --host={1} --port={2} --force {3} ", ("" + userName).Replace(@"""", @"\"""), ServerName, Port, dbSwitch);
            }

            if (!String.IsNullOrEmpty(inputFileName)) {
                if (inputFileName.ToLower().Contains("_triggers")) {
                    // HACK: didn't want to change getArgs signature just for the triggers file hack, should change later...
                    args = " --delimiter=// " + args;
                }
            }

        }

        public override string ExecuteSql(string userName, string password, string databaseName, string sql) {
            string exe = null, args = null;
            getArgs(userName, databaseName, null, out exe, out args);
            // mysql doesn't like password on standard in.  provide it via the command line.
            if (Toolkit.IsNonWindowsOS) {
                args = ("" + args).Replace("__PASSWORD__", ("" + password).Replace(@"'", @"\'"));
            } else {
                args = ("" + args).Replace("__PASSWORD__", ("" + password).Replace(@"""", @"\"""));
            }
            args += @" -e """ + ("" + sql).Replace(@"""", @"\""") + @"""";

            if (!File.Exists(exe) || Toolkit.IsNonWindowsOS) {
                // db engine probably not installed locally.
                // use a 'normal' .net db connection to run the sql
                var dcs = GetDataConnectionSpec(databaseName, userName, password);
                //MessageBox.Show("connstr: " + dcs.ToString());
                using (var dm = DataManager.Create(dcs)) {
                    dm.Write(sql);
                    return "";
                }
            } else {
                // db engine is installed locally, use the command line program to do the heavy lifting
                // MessageBox.Show("exe: " + exe + "\r\nargs: " + args);
                return exec(exe, args, null);
            }
        }

        public override string ExecuteSqlFile(string userName, string password, string databaseName, string inputFileName) {

            // mysql expects file redirection to handle incoming sql.
            // the redirection provided by .NET is slow for large sql files, so we make a temporary batch file
            // and execute it.
            
            string exe = null, args = null;
            getArgs(userName, databaseName, inputFileName, out exe, out args);

            if (Toolkit.IsNonWindowsOS) {
                args = args.Replace("__PASSWORD__", password.Replace(@"'", @"\'"));
            } else {
                args = args.Replace("__PASSWORD__", password.Replace(@"""", @"\"""));
            }

            string batFileName = System.Environment.GetEnvironmentVariable("temp") + @"\temp.bat";
            string outputFileName = inputFileName + ".log";
            string writeToBatFile =
                exe +
                args +
                @" < """ + inputFileName + @"""" +
                @" 2> """ + outputFileName + @"""";

            Debug.WriteLine("writing to bat file: " + writeToBatFile);

            // create a batch file to do the indirection (performance issues doing it the 'right' .NET way)
            File.WriteAllText(batFileName, writeToBatFile);
            ProcessStartInfo psi = new ProcessStartInfo(batFileName);
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.CreateNoWindow = true;
            Process p = Process.Start(psi);
            string output = p.StandardOutput.ReadToEnd() + " " + p.StandardError.ReadToEnd();
            p.WaitForExit();
            return output;

        }

        public override string TestLogin(string userName, string password) {
            //if (!File.Exists(getExeName())) {
                using (var dm = DataManager.Create(this.GetDataConnectionSpec(null, userName, password))) {
                    return dm.TestLogin();
                }
            //} else {
            //    string ver = GetVersion(userName, password).ToLower();
            //    if (ver.ToLower().Contains("error")) {
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
                UserName = userName,
                Password = password,
            };
            return dcs;
        }


        protected override string checkForError(string sqlOutput) {
            if (!String.IsNullOrEmpty(sqlOutput)) {
                string output = sqlOutput.ToLower();
                if (output.Contains("error") || output.Contains("failed") || output.Contains("failure") || output.Contains("invalid") || output.Contains("denied")) {
                    throw new InvalidOperationException(getDisplayMember("Mysql_checkForError", "SQL Returned error: {0}", sqlOutput));
                }
            }

            return sqlOutput;
        }

        public override string GetSettings() {

            string defaultIniContents = @"
# MySQL Server Instance Configuration File
# ----------------------------------------------------------------------
# Generated by the GRIN-Global Configuration Wizard at
# __TIMESTAMP__
#
# Installation Instructions
# ----------------------------------------------------------------------
#
# On Windows you should keep this file in the installation directory 
# of your server (e.g. C:\Program Files\MySQL\MySQL Server X.Y). To
# make sure the server reads the config file use the startup option 
# ""--defaults-file"". 
#
# To run run the server from the command line, execute this in a 
# command line shell, e.g.
# mysqld --defaults-file=""C:\Program Files\MySQL\MySQL Server X.Y\my.ini""
#
# To install the server as a Windows service manually, execute this in a 
# command line shell, e.g.
# mysqld --install MySQLXY --defaults-file=""C:\Program Files\MySQL\MySQL Server X.Y\my.ini""
#
# And then execute this in a command line shell to start the server, e.g.
# net start MySQLXY
#
#
# Guildlines for editing this file
# ----------------------------------------------------------------------
#
# In this file, you can use all long options that the program supports.
# If you want to know the options a program supports, start the program
# with the ""--help"" option.
#
# More detailed information about the individual options can also be
# found in the manual.
#
#
# CLIENT SECTION
# ----------------------------------------------------------------------
#
# The following options will be read by MySQL client applications.
# Note that only client applications shipped by MySQL are guaranteed
# to read this section. If you want your own MySQL client program to
# honor these values, you need to specify it as an option during the
# MySQL client library initialization.
#
[client]

port=3306

[mysql]

default-character-set=utf8


# SERVER SECTION
# ----------------------------------------------------------------------
#
# The following options will be read by the MySQL Server. Make sure that
# you have installed the server correctly (see above) so it reads this 
# file.
#
[mysqld]

# The TCP/IP Port the MySQL Server will listen on
port=3306


#Path to installation directory. All paths are usually resolved relative to this.
#basedir=""C:/Program Files/MySQL/MySQL Server 5.1/""
basedir=""__BASEDIR__""


#Path to the database root
datadir=""__DATADIR__""
#datadir=""C:/Documents and Settings/All Users/Application Data/MySQL/MySQL Server 5.1/Data/""

# The default character set that will be used when a new schema or table is
# created and no character set is defined
default-character-set=utf8

# The default storage engine that will be used when create new tables when
default-storage-engine=INNODB

# Set the SQL mode to strict
sql-mode=""STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION""

# The maximum amount of concurrent sessions the MySQL server will
# allow. One of these connections will be reserved for a user with
# SUPER privileges to allow the administrator to login even if the
# connection limit has been reached.
max_connections=100

# Query cache is used to cache SELECT results and later return them
# without actual executing the same query once again. Having the query
# cache enabled may result in significant speed improvements, if your
# have a lot of identical queries and rarely changing tables. See the
# ""Qcache_lowmem_prunes"" status variable to check if the current value
# is high enough for your load.
# Note: In case your tables change very often or if your queries are
# textually different every time, the query cache may result in a
# slowdown instead of a performance improvement.
query_cache_size=8M

# The number of open tables for all threads. Increasing this value
# increases the number of file descriptors that mysqld requires.
# Therefore you have to make sure to set the amount of open files
# allowed to at least 4096 in the variable ""open-files-limit"" in
# section [mysqld_safe]
table_cache=256

# Maximum size for internal (in-memory) temporary tables. If a table
# grows larger than this value, it is automatically converted to disk
# based table This limitation is for a single table. There can be many
# of them.
tmp_table_size=5M


# How many threads we should keep in a cache for reuse. When a client
# disconnects, the client's threads are put in the cache if there aren't
# more than thread_cache_size threads from before.  This greatly reduces
# the amount of thread creations needed if you have a lot of new
# connections. (Normally this doesn't give a notable performance
# improvement if you have a good thread implementation.)
thread_cache_size=8

#*** MyISAM Specific options

# The maximum size of the temporary file MySQL is allowed to use while
# recreating the index (during REPAIR, ALTER TABLE or LOAD DATA INFILE.
# If the file-size would be bigger than this, the index will be created
# through the key cache (which is slower).
myisam_max_sort_file_size=100G

# If the temporary file used for fast index creation would be bigger
# than using the key cache by the amount specified here, then prefer the
# key cache method.  This is mainly used to force long character keys in
# large tables to use the slower key cache method to create the index.
myisam_sort_buffer_size=8M

# Size of the Key Buffer, used to cache index blocks for MyISAM tables.
# Do not set it larger than 30% of your available memory, as some memory
# is also required by the OS to cache rows. Even if you're not using
# MyISAM tables, you should still set it to 8-64M as it will also be
# used for internal temporary disk tables.
key_buffer_size=8M

# Size of the buffer used for doing full table scans of MyISAM tables.
# Allocated per thread, if a full scan is needed.
read_buffer_size=64K
read_rnd_buffer_size=185K

# This buffer is allocated when MySQL needs to rebuild the index in
# REPAIR, OPTIMZE, ALTER table statements as well as in LOAD DATA INFILE
# into an empty table. It is allocated per thread so be careful with
# large settings.
sort_buffer_size=139K


#*** INNODB Specific options ***


# Use this option if you have a MySQL server with InnoDB support enabled
# but you do not plan to use it. This will save memory and disk space
# and speed up some things.
#skip-innodb

# Additional memory pool that is used by InnoDB to store metadata
# information.  If InnoDB requires more memory for this purpose it will
# start to allocate it from the OS.  As this is fast enough on most
# recent operating systems, you normally do not need to change this
# value. SHOW INNODB STATUS will display the current amount used.
innodb_additional_mem_pool_size=2M

# If set to 1, InnoDB will flush (fsync) the transaction logs to the
# disk at each commit, which offers full ACID behavior. If you are
# willing to compromise this safety, and you are running small
# transactions, you may set this to 0 or 2 to reduce disk I/O to the
# logs. Value 0 means that the log is only written to the log file and
# the log file flushed to disk approximately once per second. Value 2
# means the log is written to the log file at each commit, but the log
# file is only flushed to disk approximately once per second.
innodb_flush_log_at_trx_commit=1

# The size of the buffer InnoDB uses for buffering log data. As soon as
# it is full, InnoDB will have to flush it to disk. As it is flushed
# once per second anyway, it does not make sense to have it very large
# (even with long transactions).
innodb_log_buffer_size=1M

# InnoDB, unlike MyISAM, uses a buffer pool to cache both indexes and
# row data. The bigger you set this the less disk I/O is needed to
# access data in tables. On a dedicated database server you may set this
# parameter up to 80% of the machine physical memory size. Do not set it
# too large, though, because competition of the physical memory may
# cause paging in the operating system.  Note that on 32bit systems you
# might be limited to 2-3.5G of user level memory per process, so do not
# set it too high.
innodb_buffer_pool_size=8M

# Size of each log file in a log group. You should set the combined size
# of log files to about 25%-100% of your buffer pool size to avoid
# unneeded buffer pool flush activity on log file overwrite. However,
# note that a larger logfile size will increase the time needed for the
# recovery process.
innodb_log_file_size=10M

# Number of threads allowed inside the InnoDB kernel. The optimal value
# depends highly on the application, hardware as well as the OS
# scheduler properties. A too high value may lead to thread thrashing.
innodb_thread_concurrency=8

# Each table should be in a separate file
innodb_file_per_table

";
            string ret = defaultIniContents.Replace("__BINDIR__", this.BinDirectory).Replace("__BASEDIR__", this.BaseDirectory).Replace("__DATADIR__", this.DataDirectory).Replace("__TIMESTAMP__", DateTime.Now.ToString());
            
            // NOTE: my.ini must contain / instead of \ for path separators in the bin and data directory paths
            ret = ret.Replace(@"\", "/");

            return ret;
        }

        public override string DropUser(string superUserPassword, string databaseName, string userName, string serverName) {
            var sql = String.Format(@"
drop user '{0}'@'{1}';
", userName, serverName);

            return ExecuteSql(superUserPassword, databaseName, sql);
        }

        public override string CreateUser(string superUserPassword, string databaseName, string userName, string serverName, string userPassword, bool readOnlyRights, bool usingWindowsAuthentication) {
            var sql = String.Format(@"
create user '{0}'@'{1}' identified by '{2}';
", userName, serverName, userPassword);

            if (readOnlyRights) {
                sql += String.Format(@"
grant select on {2}.* to '{0}'@'{1}';
", userName, serverName, databaseName);
            } else {
                sql += String.Format(@"
grant select, insert, update, delete on {2}.* to '{0}'@'{1}';
", userName, serverName, databaseName);
            }

            return ExecuteSql(superUserPassword, databaseName, sql);
        }

        public override string GrantRightsToTable(string superUserPassword, string databaseName, string userName, string serverName, string tableName, bool readOnlyRights) {
            var sql = "";
            if (readOnlyRights) {
                sql += String.Format(@"
grant select on {2}.* to '{0}'@'{1}';
", userName, serverName, databaseName);
            } else {
                sql += String.Format(@"
grant select, insert, update, delete on {2}.* to '{0}'@'{1}';
", userName, serverName, databaseName);
            }

            return ExecuteSql(superUserPassword, databaseName, sql);
        }

        public override List<string> ListDatabases(string superUserPassword) {
            var output = ExecuteSql(superUserPassword, null, "show databases;");
            var ret = new List<string>();
            // TODO: parse database list
            return ret;
        }

    }
}
