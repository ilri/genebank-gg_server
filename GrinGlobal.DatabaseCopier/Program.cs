using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GrinGlobal.Core;
using GrinGlobal.DatabaseInspector;
using System.Threading;
using GrinGlobal.InstallHelper;
using System.Diagnostics;
using System.Data;
using GrinGlobal.Core.DataManagers;
using System.Runtime.InteropServices;

namespace GrinGlobal.DatabaseCopier {
	static class Program {

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();
        static frmProgress __frmProgress;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {

            if (Debugger.IsAttached)
            {
                //args = new string[] { "/hash", "administrator" };
                //args = new string[] {"/export", @"\projects\GrinGlobal_non-svn\raw_data_files", "sqlserver", @"Data Source=localhost\sqlexpress;Initial Catalog=gringlobal;Trusted_Connection=True", "gringlobal"};
            }

            if (args.Length == 5) {
                if (args[0] == "/export" || args[0] == "--export") {
                    export(args);
                    return;
                } else if (args[0] == "/import" || args[0] == "--import") {
                    import(args);
                    return;
                }
            } else if (args.Length == 4){
                if (args[0] == "/precache" || args[0] == "--precache") {
                    precacheLookups(args);
                    return;
                }
            } else if (args.Length == 2 && (args[0] == "/encrypt" || args[0] == "--encrypt")){
                encrypt(args[1]);
                return;
            } else if (args.Length == 2 && (args[0] == "/decrypt" || args[0] == "--decrypt")){
                decrypt(args[1]);
                return;
            } else if (args.Length == 2 && (args[0] == "/hash" || args[0] == "--hash")){
                hash(args[1]);
                return;
            } else if (args.Length == 1 && (args[0] == "/prompt" || args[0] == "--prompt")) {
                // this will be shown as a forms app, so don't show the console
                try {
                    FreeConsole();
                } catch { }
                prompt();
                return;
            } else if (args.Length > 0) {
                showUsage();
                return;
            }

            // this will be shown as a forms app, so don't show the console
            try {
                FreeConsole();
            } catch { }

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
//            Application.Run(new frmZipTest());
            Application.Run(new frmCopier3());

            //Application.Run(new Form2());
            //var prompt = new frmDatabaseLoginPrompt();
            //DialogResult dr = prompt.ShowDialog(0, null, true, true);

		}

        static void encrypt(string plainText) {
            var enc = Crypto.EncryptText(plainText);
            Console.WriteLine(enc);
        }

        static void decrypt(string cipherText) {
            var dec = Crypto.DecryptText(cipherText);
            Console.WriteLine(dec);
        }

        static void hash(string inputText) {
            var hash = Crypto.HashText(inputText);
            Console.WriteLine(hash);
        }

        static void prompt() {
            var prompt = new frmDatabaseLoginPrompt();
            prompt.ShowDialog(0, null, true, true, false, null, null, false, false);
        }

        static void c_OnProgress(object sender, ProgressEventArgs pea) {
            updateProgress(pea.Message);
        }

        static void updateProgress(string message) {
            if (__frmProgress != null && __frmProgress.Visible == true) {
                __frmProgress.txtProgress.Text += message + "\r\n";
                __frmProgress.txtProgress.SelectionStart = __frmProgress.txtProgress.Text.Length;
                __frmProgress.txtProgress.ScrollToCaret();
                __frmProgress.stat.Text = message;
                __frmProgress.Refresh();
                Console.WriteLine(DateTime.Now.ToString() + " " + message);
                Application.DoEvents();
                if (message.ToLower().Contains("error")) {
                    MessageBox.Show(message);
                    throw new Exception(message);
                }
            }
        }

        static void showUsage() {
            Console.WriteLine(@"Usage for GrinGlobal.DatabaseCopier.exe");
            Console.WriteLine(@"
  GrinGlobal.DatabaseCopier.exe
      For displaying GUI version of database copier.

  GrinGlobal.DatabaseCopier.exe [/prompt|--prompt]
      For prompting the user to enter connection

  GrinGlobal.DatabaseCopier.exe [/export|--export] tgtfldr provider conn dbname
      For exporting data from database to files.

      Example export:
        GrinGlobal.DatabaseCopier.exe /export ""C:\db dump"" sqlserver ""Data Source=localhost\SqlExpress;Trusted_Connection=True;Initial Catalog=gringlobal"" gringlobal

  GrinGlobal.DatabaseCopier.exe [/import|--import] [cabfile|tgtfldr] provider conn dbname
      For importing data from files to database

      Example import:
        GrinGlobal.DatabaseCopier.exe /import ""C:\db.cab"" sqlserver ""Data Source=localhost\SqlExpress;Trusted_Connection=True;Initial Catalog=gringlobal"" gringlobal
        GrinGlobal.DatabaseCopier.exe /import ""C:\db dump"" sqlserver ""Data Source=localhost\SqlExpress;Trusted_Connection=True;Initial Catalog=gringlobal"" gringlobal

    tgtfldr:  During export: folder to write schema and data files to
              During import: folder to read schema and data files from
    provider: Database provider to use. 
                Valid values: SqlServer, MySql, PostgreSql, Oracle.
    conn:     Connection string to use.
                e.g.: Data Source=localhost\SqlExpress;Trusted_Connection=True;Initial Catalog=gringlobal
    dbname:   Database name to connect to.
                e.g.: gringlobal
              NOTE: for Oracle, this must be the schema name. e.g. GRINGLOBAL
    cabfile:  Name of cabfile to import schema and data from

    Oracle note: The Oracle .NET connector does not allow logging in with 
                 credentials that require SYSDBA or SYSOPER privileges.  A 
                 userid with non-SYSDBA privileges must be used.  By default,
                 a user named 'gringlobal' is created with a password of 
                 'TempPA55' when the GRIN-Global database is installed.  This 
                 user has the 'gringlobal' schema as its default and full 
                 privileges to all relevant tables/sequences/indexes/etc.

    Example connection strings:
      SQL Server:
         Data Source=localhost\SQLExpress;Initial Catalog=gringlobal;Trusted_Connection=True;
      MySQL:
         Data Source=localhost;Database=gringlobal;User Id=root;password=<your root password>;
      PostgreSQL:
         Server=localhost;Port=5432;Database=gringlobal;User Id=postgres;password=<your postgres password>;
      Oracle XE:
         Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id='gringlobal';Password='TempPA55';

  GrinGlobal.DatabaseCopier.exe [/encrypt|--encrypt] plaintext
      For encrypting the given plaintext.  Does not connect to any database, primarily for troubleshooting.

  GrinGlobal.DatabaseCopier.exe [/decrypt|--decrypt] ciphertext
      For decrypting the given ciphertext.  Does not connect to any database, primarily for troubleshooting.

  GrinGlobal.DatabaseCopier.exe [/hash|--hash] plaintext
      For SHA1 hashing the given plaintext.  Does not connect to any database, primarily for troubleshooting.

");
        }

        static void export(string[] args) {
            // /export "target folder here" "provider_name_here" "connection_string_here" db_name_here
            string folder = args[1].Replace("\"", "");

            string during = "Connecting to database";
            try {

                // parse the dsc into a DataConnectionSpec object
                DataConnectionSpec dsc = new DataConnectionSpec {
                    ProviderName = args[2].Replace("\"", ""),
                    ConnectionString = args[3].Replace("\"", ""),
                };

                __frmProgress = new frmProgress();
                __frmProgress.Show();
                __frmProgress.Text = "Exporting Data from " + args[4] + " Database...";


                // read table info from database
                Creator c = Creator.GetInstance(dsc);
                c.OnProgress += new ProgressEventHandler(c_OnProgress);
                updateProgress("Loading table information...");
                during = "loading table information";
                List<TableInfo> tables = c.LoadTableInfo(args[4], null, false, int.MaxValue);

                // save table schema to file
                c.SaveTableInfoToXml((folder + @"\__schema.xml").Replace(@"\\", @"\"), tables);

                // copy the data from the database
                during = "copying data";
                c.CopyDataFromDatabase(folder, args[4], tables);
                updateProgress("Done");

            } catch (Exception ex) {
                updateProgress("Error while " + during + ": " + ex.Message);
                //                MessageBox.Show("Error while " + during + ": " + ex.Message);
                throw;
            }
            __frmProgress.btnDone.Enabled = true;
            __frmProgress.Close();
            //while (__frmProgress.Visible) {
            //    Thread.Sleep(1000);
            //    Application.DoEvents();
            //}
        }

        static void import(string[] args) {
            // /import "c:\cabfile_here" "provider_name_here" "connection_string_here" db_name_here
            string folder = args[1].Replace("\"", "");

            string during = "Connecting to database";
            try {
                // parse the dsc into a DataConnectionSpec object

                DataConnectionSpec dsc = new DataConnectionSpec(args[2].Replace("\"", ""), args[3].Replace("\"", ""));
                if (String.IsNullOrEmpty(dsc.DatabaseName)) {
                    dsc.DatabaseName = args[4];
                }


                __frmProgress = new frmProgress();
                __frmProgress.Show();
                __frmProgress.Text = "Importing Data to " + args[4] + " Database...";


                updateProgress("Expanding source data files...");
                string targetDir = Toolkit.ResolveDirectoryPath(@".\", false);

                if (args[1].ToLower().EndsWith(".cab")) {
                    // they gave us a cab file. extract it, plow into local folder
                    Utility.ExtractCabFile(args[1].Replace(@"""", ""), targetDir, null);
                } else {
                    // they gave us a folder. mark that as our target dir.
                    targetDir = args[1].Replace(@"""", "");
                }
                //                        Utility.Unzip(args[1].Replace("\"", ""), targetDir);


                // read table info from the given folder\__schema.xml file
                Creator c = Creator.GetInstance(dsc);
                c.OnProgress += new ProgressEventHandler(c_OnProgress);
                during = "loading table information";
                List<TableInfo> tables = c.LoadTableInfo(targetDir);

                // create tables in the database
                during = "creating tables";
                c.CreateTables(tables, args[4]);

                // copy the data to the database
                during = "copying data";
                c.CopyDataToDatabase(targetDir, args[4].Replace("\"", ""), tables, false, false);

                // create the indexes
                during = "creating indexes";
                foreach (TableInfo ti in tables) {
                    ti.IsSelected = true;
                }
                c.CreateIndexes(tables);

                // create the constraints
                during = "creating constraints";
                foreach (TableInfo ti in tables) {
                    ti.IsSelected = true;
                }
                c.CreateConstraints(tables, true, true);

                // get all the sequences up to snuff so new inserts work properly (i.e. make sure sequence id's are past the last one currently in each table)
                var dbUtil = DatabaseEngineUtil.CreateInstance(args[2].Replace("\"", ""), null, args[3].Replace("\"", ""));

                if (dsc.ProviderName.ToLower() == "oracle") {
                    during = "restarting sequences";
                    foreach (var t in tables) {
                        updateProgress("restarting sequence for " + t.TableName + "...");
                        var ret = ((OracleEngineUtil)dbUtil).RestartSequenceForTable(dsc.Password, dsc.UserName, dsc.Password, t.TableName, t.PrimaryKey.Name);
                        updateProgress(ret);
                    }
                } else if (dsc.ProviderName.ToLower() == "postgresql") {
                    during = "restarting sequences";
                    foreach (var t in tables) {
                        updateProgress("restarting sequence for " + t.TableName + "...");
                        var ret = ((PostgreSqlEngineUtil)dbUtil).RestartSequenceForTable(dsc.Password, dsc.DatabaseName, dsc.ServerName, t.TableName, t.PrimaryKey.Name);
                        updateProgress(ret);
                    }
                } else if (dsc.ProviderName.ToLower() == "mysql") {
                    c.RebuildIndexes(tables);
                }



            } catch (Exception ex) {
                updateProgress("Error while " + during + ": " + ex.Message);
                throw;
            }
            __frmProgress.btnDone.Enabled = true;
            __frmProgress.Close();
            //while (__frmProgress.Visible) {
            //    Thread.Sleep(1000);
            //    Application.DoEvents();
            //}
        }

        private static void precacheLookups(string[] args) {
            string lastSqlRan = "";
            try {

                var providerName = args[1];
                var connectionString = args[2];
                var serverName = args[3];

                if (String.IsNullOrEmpty(providerName)) {
                    showUsage();
                    return;
                }

                if (String.IsNullOrEmpty(connectionString)) {
                    showUsage();
                    return;
                }

                if (String.IsNullOrEmpty(serverName)) {
                    showUsage();
                    return;
                }

                var dsc = new DataConnectionSpec(providerName, null, null, connectionString);

                string newDatabaseName = "GRINGlobal_" + serverName.Replace(".", "_").Replace("-", "_").Replace(":", "_");

                var dbEngineUtil = DatabaseEngineUtil.CreateInstance(providerName, Toolkit.ResolveFilePath("~/gguac.exe", false), null);

                if (dbEngineUtil is SqlServerEngineUtil) {
                    if (String.IsNullOrEmpty(dsc.Password)) {
                        (dbEngineUtil as SqlServerEngineUtil).UseWindowsAuthentication = true;
                    }
                }
                try {
                    Console.WriteLine("Dropping existing client database for server " + serverName + "...");
                    dbEngineUtil.DropDatabase(dsc.Password, newDatabaseName);
                } catch (Exception exDrop){
                    Console.WriteLine("Could not drop database: " + exDrop.Message);
                    Console.WriteLine("Continuing anyway");
                }

                Console.WriteLine("Creating new client database for server " + serverName + "...");
                dbEngineUtil.CreateDatabase(dsc.Password, newDatabaseName);

                Console.WriteLine("Getting list of tables to precache for server " + serverName + "...");
                lastSqlRan = @"
SELECT 
    dv.sys_dataview_id,
    dv.dataview_name, 
    dvs.sql_statement,
    sdl.title, 
    sdl.description, 
    stf.field_name as pk_field_name, 
    st.table_name as table_name 
FROM 
    sys_dataview dv left join sys_dataview_lang sdl 
        on dv.sys_dataview_id = sdl.sys_dataview_id 
        and sdl.sys_lang_id = 1 
    inner join sys_dataview_sql dvs
        on dv.sys_dataview_id = dvs.sys_dataview_id and dvs.database_engine_tag = 'sqlserver'
    inner join sys_dataview_field sdvf 
        on sdvf.sys_dataview_id = dv.sys_dataview_id 
        and sdvf.is_primary_key = 'Y' 
    inner join sys_table_field stf 
        on sdvf.sys_table_field_id = stf.sys_table_field_id 
    inner join sys_table st 
        on stf.sys_table_id = st.sys_table_id 
WHERE 
    dv.dataview_name like '%_lookup'
order by
    dv.dataview_name
";

                using (DataManager dm = DataManager.Create(dsc)) {

                    var dt = dm.Read(lastSqlRan, new DataParameters(":enginecode", dm.DataConnectionSpec.EngineName));

                    Console.WriteLine("Found " + dt.Rows.Count + " tables to precache.");

                    try {
                        // drop lookup status table
                        lastSqlRan = String.Format("drop table [{0}].dbo.lookup_table_status", newDatabaseName);
                        dm.Write(lastSqlRan);
                    } catch (Exception exStatus) {
                        //Console.WriteLine("Failed dropping lookup status table: " + exStatus.Message);
                        //Console.WriteLine("Continuing anyway");
                    }

                    try {

                        Console.Write("Creating lookup status table...");
                        // create the lookup status table
                        lastSqlRan = String.Format(@"
CREATE TABLE [{0}].[dbo].[lookup_table_status](
	[table_name] [varchar](255) NOT NULL,
	[pk_field_name] [varchar](255) NULL,
	[dataview_name] [varchar](255) NULL,
	[title] [varchar](255) NULL,
	[description] [varchar](255) NULL,
	[current_pk] [int] NULL,
	[min_pk] [int] NULL,
	[max_pk] [int] NULL,
	[row_count] [int] NULL,
	[auto_update] [varchar](255) NULL,
	[status] [varchar](255) NULL,
	[sync_date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[table_name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
", newDatabaseName);

                        dm.Write(lastSqlRan);

                        Console.WriteLine("Done");

                    } catch (Exception exStatus2) {
                        Console.WriteLine("Failed to create lookup status table: " + exStatus2.Message);
                        Console.WriteLine("Continuing anyway");
                    }

                    lastSqlRan = String.Format("delete from [{0}].[dbo].[lookup_table_status]", newDatabaseName);
                    dm.Write(lastSqlRan);


                    foreach (DataRow dr in dt.Rows) {

                        string lookupSql = dr["sql_statement"].ToString();
                        string newTableName = dr["dataview_name"].ToString();
                        string sourceTableName = dr["table_name"].ToString();
                        string pkFieldName = dr["pk_field_name"].ToString();
                        string title = dr["title"].ToString();
                        string description = dr["description"].ToString();

                        // grab all parameters for the dataview so we can just null them out
                        // (the way the dataview lookup queries are written this will cause all rows to be returned)
                        lastSqlRan = @"
select
    param_name
from
    sys_dataview_param
where
    sys_dataview_id = :id
";
                        var dtParams = dm.Read(lastSqlRan, new DataParameters(":id", dr["sys_dataview_id"], DbType.Int32));

                        foreach (DataRow drParam in dtParams.Rows) {
                            lookupSql = lookupSql.Replace(drParam["param_name"].ToString(), "NULL");
                        }


                        // now use that munged sql statement a to create a common table expression so we can 
                        // do a select into against it w/o marshalling everything from sql server to our process then back to sql server again
                        Console.Write(("Precaching " + newTableName + "...").PadRight(50, ' '));

                        try {
                            lastSqlRan = String.Format(@"
drop table [{0}].[dbo].[{1}];
", newDatabaseName, newTableName);
                            dm.Write(lastSqlRan);
                        } catch {
                        }

                        // create the structure
                        // (we avoid copying the rows with the common table expression as that creates a lot of memory overhead
                        //  and on boxes with 512 MB of RAM it'll bomb).
                        lastSqlRan = String.Format(@"
WITH lookuptable as ({0})
SELECT * INTO
    [{1}].[dbo].[{2}]
FROM
    lookuptable
", lookupSql, newDatabaseName, newTableName);

                        // create the table in the new database
                        dm.Write(lastSqlRan);
                        // get name of primary key for lookup table
                        // (assumes first field is PK and PK is an int)
                        lastSqlRan = String.Format("select top 1 * from [{0}].[dbo].[{1}]", newDatabaseName, newTableName);
                        var dtTemp = dm.Read(lastSqlRan);
                        var pkLookupFieldName = dtTemp.Columns[0].ColumnName;

                        // make sure the PK is set so CT works properly
                        lastSqlRan = String.Format(@"
ALTER TABLE [{0}].[dbo].[{1}] ADD CONSTRAINT
	PK_{1} PRIMARY KEY CLUSTERED 
	(
	{2}
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
", newDatabaseName, newTableName, pkLookupFieldName);

                        dm.Write(lastSqlRan);

                        var min = Toolkit.ToInt32(dm.ReadValue(String.Format("select min({2}) from [{0}].[dbo].[{1}]", newDatabaseName, newTableName, pkLookupFieldName)), 0);
                        var max = Toolkit.ToInt32(dm.ReadValue(String.Format("select max({2}) from [{0}].[dbo].[{1}]", newDatabaseName, newTableName, pkLookupFieldName)), 0);

                        var rowCount = Toolkit.ToInt32(dm.ReadValue(String.Format("select count(*) from [{0}].[dbo].[{1}]", newDatabaseName, newTableName)), 0);

                        // add to the lookup status table
                        lastSqlRan = String.Format(@"
insert into [{0}].[dbo].[lookup_table_status]
(table_name, pk_field_name, dataview_name, title, description, current_pk, min_pk, max_pk, row_count, auto_update, status, sync_date)
values
(:tbl, :pk, :dv, :title, :description, :curpk, :minpk, :maxpk, :rc, :auto, :status, getutcdate())
", newDatabaseName);

                        dm.Write(lastSqlRan, new DataParameters(":tbl", sourceTableName, ":pk", pkFieldName, ":dv", newTableName, ":title", title, ":description", (String.IsNullOrEmpty(description) ? null : description), ":curpk", max, DbType.Int32, ":minpk", min, DbType.Int32, ":maxpk", max, DbType.Int32, ":rc", rowCount, DbType.Int32, ":auto", "Y", ":status", "Completed"));


                        if (rowCount == 0) {
                            dm.Write(String.Format("drop table [{0}].[dbo].[{1}]", newDatabaseName, newTableName));
                        }

                        Console.WriteLine(rowCount + " records copied.");

                    }
                    Console.WriteLine("Successfully created and precached " + dt.Rows.Count + " lookup tables");
                }
            } catch (Exception exDropClientDB) {
                var msg = "Error precaching client data: " + exDropClientDB.Message + "\nLast SQL Ran: " + lastSqlRan;
                Console.WriteLine(msg);
            }
        }
	}
}
