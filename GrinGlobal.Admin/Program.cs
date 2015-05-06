using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using GrinGlobal.Admin.PopupForms;
using GrinGlobal.Core;
using GrinGlobal.InstallHelper;
using System.Runtime.InteropServices;
using GrinGlobal.Business;
using GrinGlobal.Business.SearchSvc;
using System.Text;
using System.Data;

namespace GrinGlobal.Admin {
    static class Program {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {

            //MessageBox.Show(Toolkit.IsProcessElevated().ToString());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //// Add the event handler for handling UI thread exceptions to the event.
            //Application.ThreadException += new ThreadExceptionEventHandler(Form1_UIThreadException);

            //// Set the unhandled exception mode to force all Windows Forms errors to go through
            //// our handler.
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            //// Add the event handler for handling non-UI thread exceptions to the event. 
            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

//            Application.Run(new frmMain());


            if (Debugger.IsAttached){
                // args = new string[]{"/password", "sqlserver", @"Data Source=localhost\sqlexpress;initial catalog=gringlobal;trusted_connection=yes;", "gringlobal", "administrator", "administrator" };
                // args = new string[] { "/password", "sqlserver", @"Data Source=localhost\sqlexpress;initial catalog=gringlobal;trusted_connection=yes;", "gringlobal", "guest", "guest" };

                //args = new string[] { "/webpassword", "sqlserver", @"Data Source=localhost\sqlexpress;initial catalog=gringlobal;trusted_connection=yes;", "gringlobal", "administrator", "administrator" };
                // args = new string[] { "/webpassword", "sqlserver", @"Data Source=localhost\sqlexpress;initial catalog=gringlobal;trusted_connection=yes;", "gringlobal", "guest", "guest" };
                
                // args = new string[] { "/pingsearchengine", "pipe", "net.pipe://localhost/searchhost" };
                // args = new string[] { "/pingsearchengine", "tcp", "tcp://localhost:2012/searchhost" };
                //args = new string[] { "/querysearchengine", "pipe", "net.pipe://localhost/searchhost", "zea" };
            }

            //try {
            if (args.Length > 0) {
                var fn = args[0];

                if (fn == "/help" || fn == "--help" || fn == "/?") {
                    showHelp();
                    return;
                }  else if ((args.Length == 6 && (fn == "/password" || fn == "--password"))) {
                    //  /password providername dbconn dbname username newpassword
                    setPassword(args[1], args[2], args[3], args[4], args[5]);

                    return;
                } else if ((args.Length == 6 && (fn == "/webpassword" || fn == "--webpassword"))) {
                    //  /password providername dbconn dbname username newpassword
                    setWebPassword(args[1], args[2], args[3], args[4], args[5]);

                    return;
                } else if (args.Length == 3 && (fn == "/pingsearchengine" || fn == "--pingsearchengine")){
                    pingSearchEngine(args[1], args[2]);
                    return;
                } else if (args.Length == 4 && (fn == "/querysearchengine" || fn == "--querysearchengine")) {
                    querySearchEngine(args[1], args[2], args[3]);
                    return;
                } else {


                    // D:\downloads\2010_11_02_import_taxonomy_with_sites.dataview
                    if (Utility.IsWindowsFilePath(fn)) {
                        //mdiParent.Current.DataviewToImport = fn;
                        if (!mdiParent.Current.TellOtherAdminToolToImport(fn)) {
                            // didn't find another AT instance / bombed trying to.
                            // just load it locally.
                            mdiParent.Current.DataviewToImport = fn;
                        } else {
                            // found it, told it, assume they got it working.
                            return;
                        }
                    } else if (args.Length > 1) {
                        if (String.Compare(fn, "/display", true) == 0) {
                            mdiParent.Current.DefaultNodeToDisplay = args[1];
                        }
                    }
                }
            }



            Application.Run(mdiParent.Current);

            //} catch (Exception ex) {
            //    Debug.Fail(ex.Message);
            //}
        }

        // Handle the UI exceptions by showing a dialog box, and asking the user whether
        // or not they want to abort execution.
        private static void Form1_UIThreadException(object sender, ThreadExceptionEventArgs t) {
            DialogResult result = DialogResult.Cancel;
            try {
                result = ShowThreadExceptionDialog("Windows Forms Error", t.Exception);
            } catch {
                try {
                    MessageBox.Show(getDisplayMember("uithread{failed_body}", "Fatal Windows Forms Error"),
                        getDisplayMember("uithread{failed_title}", "Fatal Windows Forms Error"), MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                } finally {
                    Application.Exit();
                }
            }

            // Exits the program when the user clicks Abort.
            if (result == DialogResult.Abort)
                Application.Exit();
        }

        // Handle the UI exceptions by showing a dialog box, and asking the user whether
        // or not they want to abort execution.
        // NOTE: This exception cannot be kept from terminating the application - it can only 
        // log the event, and inform the user about it. 
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            try {
                Exception ex = (Exception)e.ExceptionObject;
                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                    "with the following information:\n\n";

                // Since we can't prevent the app from terminating, log this to the event log.
                if (!EventLog.SourceExists("ThreadException")) {
                    EventLog.CreateEventSource("ThreadException", "Application");
                }

                // Create an EventLog instance and assign its source.
                EventLog myLog = new EventLog();
                myLog.Source = "ThreadException";
                myLog.WriteEntry(errorMsg + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
            } catch (Exception exc) {
                try {
                    MessageBox.Show(getDisplayMember("unhandled{failed_body}", "Fatal Non-UI Error"),
                        getDisplayMember("unhandled{failed_title}", "Fatal Non-UI Error. Could not write the error to the event log. Reason: {0}", exc.Message), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                } finally {
                    Application.Exit();
                }
            }
        }

        // Creates the error message and displays it.
        private static DialogResult ShowThreadExceptionDialog(string title, Exception e) {
            string errorMsg = getDisplayMember("showthread{failed_body}", "An application error occurred. Please contact the adminstrator " +
                "with the following information:\n\n{0}{1}\n\nStack Trace:\n", e.Message, e.StackTrace);
            return MessageBox.Show(errorMsg, title, MessageBoxButtons.AbortRetryIgnore,
                MessageBoxIcon.Stop);
        }

        private static void showHelp() {
            var f = new frmMessageBox ();
            f.btnYes.Visible = false;
            f.btnNo.Text = "OK";
            f.txtMessage.Text = @"
usage
===========================
  GrinGlobal.Admin.exe (no parameters)
    Display the GUI

  GrinGlobal.Admin.exe dataview_filepath
    Display the GUI, begin the dataview import process for given filename

  GrinGlobal.Admin.exe /?|/help|--help
    Display this message
  
  GrinGlobal.Admin.exe /display nodename
    Load the GUI, display the given node

  GrinGlobal.Admin.exe /password provider conn dbname username newpassword
    Set the given user's password on the given database connection
    This is used to forcefully reset password(s) if they are forgotten

  GrinGlobal.Admin.exe /webpassword provider conn dbname username newpassword
    Set the given web user's password on the given database connection
    This is used to forcefully reset password(s) if they are forgotten

  GrinGlobal.Admin.exe /pingsearchengine type url
    Ping search engine to test connectivity.

  GrinGlobal.Admin.exe /querysearchengine type url query
    Query search engine to test serialization.

  -------------------------
  Parameter definitions
  -------------------------
    provider: Database provider to use. 
                Valid values: SqlServer, MySql, PostgreSql, Oracle.
    conn:     Connection string to use.
                e.g.: Data Source=localhost\SqlExpress;Trusted_Connection=True;Initial Catalog=gringlobal
    dbname:   Database name to connect to.
                e.g.: gringlobal
              NOTE: for Oracle, this must be the schema name. e.g. GRINGLOBAL

    type:     Type of connection to search engine. Valid values:
                http or pipe or tcp or basic (basic is non-WCF TCP, all others are WCF)

    url:      Url for search engine. e.g.:
                http://localhost:2011/searchhost
                net.pipe://localhost/searchhost
                net.tcp://localhost:2011/searchhost
                tcp://localhost:2011/searchhost

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


";

            f.ShowInTaskbar = true;
            f.Icon = mdiParent.Current.Icon;

            Application.Run(f);

        }

        private static void setPassword(string providerName, string connectionString, string databaseName, string userName, string newPassword) {

            DataConnectionSpec dsc = new DataConnectionSpec(providerName, connectionString);
            if (String.IsNullOrEmpty(dsc.DatabaseName)) {
                dsc.DatabaseName = databaseName;
            }

            var resp = SecureData.SetPasswordForcefully(userName, newPassword, dsc);
            if (String.IsNullOrEmpty(resp)) {
                Console.WriteLine("Password for '" + userName + "' has been changed.");
            } else {
                Console.WriteLine("Could not change password for '" + userName + "': " + resp);
            }
        }

        private static void setWebPassword(string providerName, string connectionString, string databaseName, string userName, string newPassword) {

            DataConnectionSpec dsc = new DataConnectionSpec(providerName, connectionString);
            if (String.IsNullOrEmpty(dsc.DatabaseName)) {
                dsc.DatabaseName = databaseName;
            }

            var resp = SecureData.SetWebPasswordForcefully(userName, newPassword, dsc);
            if (String.IsNullOrEmpty(resp)) {
                Console.WriteLine("Password for web user '" + userName + "' has been changed.");
            } else {
                Console.WriteLine("Could not change password for web user '" + userName + "': " + resp);
            }
        }

        private static void pingSearchEngine(string type, string url) {
            try {
                using (var c = new ClientSearchEngineRequest(type, url)) {
                    if (c.Ping()) {
                        Console.WriteLine("Successfully pinged search engine");
                    } else {
                        Console.WriteLine("Search engine ping returned false, but no exception was thrown.");
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine("Error pinging search engine: " + ex.ToString());
            }
        }


        private static void querySearchEngine(string type, string url, string query) {
            try {
                using (var c = new ClientSearchEngineRequest(type, url)) {
                    var ds = c.Search(query, true, true, null, "accession", 0, 0, null, null);
                    var dtQuery = ds.Tables["SearchQuery"];
                    if (dtQuery != null && dtQuery.Rows.Count > 0) {
                        var drQuery = dtQuery.Rows[0];
                        Console.WriteLine("Original query:    " + drQuery["searchString"]);
                        Console.WriteLine("Ignore Case:       " + drQuery["ignoreCase"]);
                        Console.WriteLine("Auto-AND Literals: " + drQuery["autoAndConsecutiveLiterals"]);
                        Console.WriteLine("Indexes:           " + drQuery["indexes"]);
                        Console.WriteLine("Resolver:          " + drQuery["resolverName"]);
                        Console.WriteLine("Parsed query:      " + drQuery["parsedSearchString"]);
                        Console.WriteLine("Total found:       " + drQuery["totalFound"]);
                    } else {
                        Console.WriteLine("-- No SearchQuery table in result --");
                    }
                    var dtResults = ds.Tables["SearchResult"];
                    if (dtResults == null || dtResults.Rows.Count == 0) {
                        Console.WriteLine("0 results found.");
                    } else {
                        Console.WriteLine("Found " + dtResults.Rows.Count + " results:");
                        var sb = new StringBuilder();
                        foreach (DataRow dr in dtResults.Rows) {
                            sb.Append(dr["ID"]).Append(", ");
                        }
                        Console.WriteLine(sb.ToString());
                    }
                }

            } catch (Exception ex) {
                Console.WriteLine("Error querying search engine: " + ex.ToString());
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "Program", resourceName, null, defaultValue, substitutes);
        }
    }
}
