using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;
using GrinGlobal.Core;
using GrinGlobal.Core.DataManagers;
using GrinGlobal.DatabaseInspector;
using System.Text;
using GrinGlobal.DatabaseInspector.PostgreSql;
using System.Data;
using GrinGlobal.InstallHelper;
using System.Net;

using System.Threading;

namespace GrinGlobal.DatabaseCopier {
    [RunInstaller(true)]
    public partial class DatabaseInstaller : Installer {
        public DatabaseInstaller() {
            InitializeComponent();

        }

        //private void setSuperUserPassword(IDictionary savedState, string newPassword) {
        //    savedState["PASSWORD"] = Crypto.EncryptText(newPassword);
        //}

        private frmProgress _frmProgress;

        private const int SLEEP_TIME = 10;

        private object _threadSync = new object();
        private void waitForUserInput(BackgroundWorker worker, object state, int maxWaitMilliseconds) {

            // block current thread until user has responded to the input
            if (worker != null) {
                worker.ReportProgress(0, state);
                // THIS CAUSES ISSUES!  Some threading problem exists, do NOT remove timeout...
                // Toolkit.BlockThread();
                Toolkit.BlockThread(maxWaitMilliseconds);
            }



        }

        private bool _workerDone;
        private void progressDone() {
            try {
                _workerDone = true;
                if (_frmProgress != null) {
                    _frmProgress.Cursor = Cursors.Default;
                    _frmProgress.Close();
                    _frmProgress = null;
                }
            } finally {
                // make sure everything continues to process so we will eventually exit at some point
                Toolkit.UnblockBlockedThread();
            }
        }

        void c_OnProgress(object sender, ProgressEventArgs pea) {
            worker_ProgressChanged(sender, new ProgressChangedEventArgs(0, pea.Message)); 
        }


        //private bool promptForSuperUserPassword(string engine, string instanceName, ref string superUserPassword, ref bool windowsAuth) {

        //    superUserPassword = null;
        //    frmLogin login = new frmLogin();
        //    DialogResult dr = login.ShowDialog(engine, instanceName, windowsAuth);
        //    if (dr == DialogResult.OK) {
        //        superUserPassword = login.SuperUserPassword;
        //        windowsAuth = login.UseWindowsAuthentication;
        //        return true;
        //    } else {
        //        return false;
        //    }
        //    //}

        //}

        private DialogResult promptForDatabaseLoginInfo(BackgroundWorker worker, string engine, string instanceName, bool windowsAuth, string gguacPath, ref DatabaseEngineUtil dbEngineUtil, ref string superUserPassword, bool allowSkip) {

            if (worker != null) {

                var args = new WorkerArguments();
                args.Properties["prompt"] = "dbEngineUtil";
                args.Properties["engine"] = engine;
                args.Properties["instanceName"] = instanceName;
                args.Properties["windowsAuth"] = windowsAuth;
                args.Properties["gguacPath"] = gguacPath;
                args.Properties["dbEngineUtil"] = dbEngineUtil;
                args.Properties["superUserPassword"] = superUserPassword;
                args.Properties["dialogresult"] = null;
                args.Properties["allowSkip"] = allowSkip;
                waitForUserInput(worker, args, int.MaxValue);  // wait forever?

                dbEngineUtil = args.Properties["dbEngineUtil"] as DatabaseEngineUtil;
                superUserPassword = args.Properties["superUserPassword"] as string;

                var dr = (DialogResult)args.Properties["dialogresult"];
                return dr;

            } else {


                var f = new frmDatabaseLoginPrompt();
                var autoLogin = Utility.GetAutoLogin(Context, null);  // enable auto login only if it's a passive install
                //MessageBox.Show("autologin: " + enableAutoLogin);
                //throw new InvalidOperationException("unconditionally bombing out");
                var dr = f.ShowDialog(
                        Toolkit.GetWindowHandle("GRIN-Global Database"), 
                        dbEngineUtil, 
                        true,
                        true,
                        false, 
                        superUserPassword, 
                        dbEngineUtil == null ? null : (bool?)dbEngineUtil.UseWindowsAuthentication,
                        autoLogin,
                        allowSkip);
                if (dr == DialogResult.OK) {
                    superUserPassword = f.txtPassword.Text;
                    dbEngineUtil = f.DatabaseEngineUtil;
                }
                return dr;
            }
        }

        private DialogResult showMessageBox(BackgroundWorker worker, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon) {
            if (worker == null) {
                try {
                    return MessageBox.Show(_frmProgress, text, caption, buttons, icon);
                } catch {
                    // if the form is disposing, showing a messagebox with it as a owner will fail.
                    return MessageBox.Show(text, caption, buttons, icon);
                }
            } else {
                var args = new WorkerArguments();
                args.Properties["prompt"] = "messagebox";
                args.Properties["text"] = text;
                args.Properties["caption"] = caption;
                args.Properties["buttons"] = buttons;
                args.Properties["icon"] = icon;
                waitForUserInput(worker, args, int.MaxValue);  // wait forever?
                object result = null;
                if (args.Properties.TryGetValue("dialogresult", out result)) {
                    return (DialogResult)result;
                } else {
                    return DialogResult.OK;
                }
            }
        }


        private BackgroundWorker initWorker() {
            var worker = new BackgroundWorker();
            _workerDone = false;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            return worker;
        }

        private void waitForWorkerToComplete(BackgroundWorker worker) {
            // do a cheesy spin wait here to make sure gui thread displays updates to user...
            while (worker.IsBusy && !_workerDone) {
                Thread.Sleep(SLEEP_TIME);
                Application.DoEvents();
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            progressDone();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            bool unblockThread = true;
            try {
                if (e.UserState is string) {
                    // background thread should not be blocked, so don't unblock it in the finally {}
                    unblockThread = false;
                    updateProgressForm(e.UserState as string, true);
                } else if (e.UserState is ApplicationException) {
                    var msg = Toolkit.Cut((e.UserState as ApplicationException).Message, 0, 7990);
                    EventLog.WriteEntry("GRIN-Global Database", msg, EventLogEntryType.Error);
                    updateProgressForm(msg, false);
                    showMessageBox(null, msg, getDisplayMember("ApplicationException", "Application Exception Occurred"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (e.UserState is Exception) {
                    var msg = Toolkit.Cut((e.UserState as Exception).Message, 0, 7990);
                    EventLog.WriteEntry("GRIN-Global Database", msg, EventLogEntryType.Error);
                    updateProgressForm(msg, false);
                    showMessageBox(null, msg, getDisplayMember("Exception", "Exception Occurred"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (e.UserState is WorkerArguments) {
                    var args = e.UserState as WorkerArguments;
                    var promptType = ("" + args.Properties["prompt"]).ToLower();
                    switch(promptType){
                        case "dbengineutil":
                            var engine = args.Properties["engine"] as string;
                            var instanceName = args.Properties["instanceName"] as string;
                            var windowsAuth = (bool)args.Properties["windowsAuth"];
                            var gguacPath = args.Properties["gguacPath"] as string;
                            var dbEngineUtil = args.Properties["dbEngineUtil"] as DatabaseEngineUtil;
                            var superUserPassword = args.Properties["superUserPassword"] as string;
                            var allowSkip = Toolkit.ToBoolean(args.Properties["allowSkip"], false);
                            args.Properties["dialogresult"] = promptForDatabaseLoginInfo(null, engine, instanceName, windowsAuth, gguacPath, ref dbEngineUtil, ref superUserPassword, allowSkip);
                            args.Properties["dbEngineUtil"] = dbEngineUtil;
                            args.Properties["superUserPassword"] = superUserPassword;
                            break;
                        case "messagebox":
                            args.Properties["dialogresult"] = showMessageBox(null, args.Properties["text"] as string, args.Properties["caption"] as string, (MessageBoxButtons)args.Properties["buttons"], (MessageBoxIcon)args.Properties["icon"]);
                            break;
                        default:
                            EventLog.WriteEntry("GRIN-Global Database", "Undefined 'prompt' type in background worker: " + promptType, EventLogEntryType.Error);
                            throw new InvalidOperationException(getDisplayMember("worker_ProgressChanged", "Undefined 'prompt' type in background worker: {0}", promptType));
                    }
                }
            } finally {
                if (unblockThread) {
                    Toolkit.UnblockBlockedThread();
                }
            }
        }

        private string _dbLogFile;
        private void updateProgressForm(string text, bool showMessageBoxOnError) {
            try {
                var msg = DateTime.Now.ToString() + " - " + text + "\r\n";
                _frmProgress.txtProgress.AppendText(msg);
                //_frmProgress.txtProgress.SelectionStart = _frmProgress.txtProgress.Text.Length;
                //_frmProgress.txtProgress.ScrollToCaret();
                if (_dbLogFile == null) {
                    _dbLogFile = Toolkit.ResolveFilePath(Utility.GetTargetDirectory(Context, null, "Database") + @"\db_install_log.txt", true);
                }
                File.AppendAllText(_dbLogFile, msg);
                _frmProgress.Refresh();
                Application.DoEvents();
            } catch {
                // window may be gone by the time we get here.  Ignore it if it is since this is just informational for the user anyway
            }
            if (showMessageBoxOnError) {
                if (text.ToLower().Contains("error")) {
                    //Context.LogMessage(text);
                    EventLog.WriteEntry("GRIN-Global Database", "Error message to user: " + text, EventLogEntryType.Error);
                    MessageBox.Show(text, getDisplayMember("updateProgressForm", "Error"));
                    throw new Exception(text);
                }
            }
        }

        private void initProgressForm() {
            if (_frmProgress == null) {
                _frmProgress = new frmProgress();
                _frmProgress.btnDone.Visible = false;
                _frmProgress.Cursor = Cursors.WaitCursor;
                _frmProgress.Show();
                updateProgressForm("Initializing, please be patient...", false);
                Toolkit.ActivateApplication(_frmProgress.Handle);
                Application.DoEvents();
            }
        }

        private void initContextVariables(BackgroundWorker worker, StringBuilder sb, InstallEventArgs e, ref string engine, ref string instanceName, ref bool windowsAuth, ref DatabaseEngineUtil dbEngineUtil, ref string targetDir, ref string sourceDir, ref string gguacPath, ref string dataDestinationFolder, ref string superUserPassword, bool allowSkip) {
            // init variables
            if (engine == null) {
                engine = Utility.GetDatabaseEngine(this.Context, e.SavedState, false, "");
            }
            sb.AppendLine("engine=" + engine);
            if (instanceName == null) {
                instanceName = Utility.GetDatabaseInstanceName(this.Context, e.SavedState, "");
            }
            sb.AppendLine("instance=" + instanceName);
            if (targetDir == null) {
                targetDir = Utility.GetTargetDirectory(this.Context, e.SavedState, "Database");
            }
            sb.AppendLine("targetdir=" + targetDir);
            if (sourceDir == null) {
                sourceDir = Utility.GetSourceDirectory(this.Context, e.SavedState, null);
            }
            sb.AppendLine("sourcedir=" + sourceDir);
            if (gguacPath == null) {
                gguacPath = Toolkit.ResolveFilePath(targetDir + @"\gguac.exe", false);
            }
            sb.AppendLine("gguacPath=" + gguacPath);
            if (dataDestinationFolder == null) {
                dataDestinationFolder = (Utility.GetTempDirectory(1500) + @"\data").Replace(@"\\", @"\");
            }
            sb.AppendLine("data temp folder=" + dataDestinationFolder);
            if (superUserPassword == null) {
                superUserPassword = Utility.GetSuperUserPassword(Context, e.SavedState);
            }
            sb.AppendLine("superuser db password specified? " + (String.IsNullOrEmpty(superUserPassword) ? "Not yet" : "Yes"));
            windowsAuth = Utility.GetDatabaseWindowsAuth(Context, e.SavedState);
            sb.AppendLine("windowsauth=" + windowsAuth.ToString());

            var useRegSettings = Utility.GetUseRegSettings(Context, e.SavedState);
            if (dbEngineUtil == null) {
                if (!DatabaseEngineUtil.IsValidEngine(engine)) {
                    // default to sql server, windows auth on, SQLExpress instance
                    engine = "sqlserver";
                    windowsAuth = true;
                    instanceName = "SQLExpress";
                }

                // prompt the user to tell us which engine and connection information they want us to use in case they want to change it
                var resp = promptForDatabaseLoginInfo(worker, engine, instanceName, windowsAuth,  gguacPath, ref dbEngineUtil, ref superUserPassword, allowSkip);
                if (resp == DialogResult.OK){
                    engine = dbEngineUtil.EngineName;
                    sb.AppendLine("engine=" + engine);
                    if (dbEngineUtil is SqlServerEngineUtil) {
                        instanceName = (dbEngineUtil as SqlServerEngineUtil).InstanceName;
                        sb.AppendLine("instanceName=" + instanceName);
                    }
                    windowsAuth = dbEngineUtil.UseWindowsAuthentication;
                    sb.AppendLine("windowsauth=" + windowsAuth);
                    //setSuperUserPassword(e.SavedState, superUserPassword);
                    sb.AppendLine("superuser db password specified? " + (String.IsNullOrEmpty(superUserPassword) ? "Not yet" : "Yes"));

                } else if (resp == DialogResult.Ignore){
                    dbEngineUtil = null;
                } else {
                    throw new InvalidOperationException(getDisplayMember("initContextVariables{usercancel}", "User cancelled out of database connection dialog"));
                }

            }

        }

        /// <summary>
        /// inner class used for negotiating between main and background threads at start and end of background thread execution
        /// </summary>
        private class WorkerArguments {
            public Exception Exception;
            public InstallEventArgs InstallEventArgs;
            public Dictionary<string, object> Properties;
            public WorkerArguments() {
                Properties = new Dictionary<string, object>();
            }
        }

        private void DatabaseInstaller_AfterInstall(object sender, InstallEventArgs e) {

            try {
                initProgressForm();

                int installerWindowHandle = Toolkit.GetWindowHandle("GRIN-Global Database");
                if (installerWindowHandle > 0) {
                    Toolkit.MinimizeWindow(installerWindowHandle);
                }

                using (var worker = initWorker()) {
                    var wa = new WorkerArguments { InstallEventArgs = e };
                    worker.DoWork += new DoWorkEventHandler(installworker_DoWork);
                    try {
                        worker.RunWorkerAsync(wa);
                        waitForWorkerToComplete(worker);
                    } catch (Exception ex) {
                        MessageBox.Show(getDisplayMember("afterInstall{exceptionbody}", "Exception running worker async: {0}", ex.Message), getDisplayMember("afterInstall{exceptiontitle}", "Worker Launch Exception"));
                    } finally {
                        if (installerWindowHandle > 0) {
                            Toolkit.RestoreWindow(installerWindowHandle);
                        }

                        // exceptions thrown in the background worker thread won't cause us to bomb here.
                        // so the code running in background worker context needs to tell us if it encountered an exception.
                        // we rethrow it on the primary process thread here if needed.
                        if (wa.Exception != null) {
                            throw wa.Exception;
                        }
                    }
                }
            } catch (Exception ex){
                EventLog.WriteEntry("GRIN-Global Database", Toolkit.Cut("Exception in DatabaseInstaller_AfterInstall: " + ex.Message, 0, 8000), EventLogEntryType.Error);
                throw;
            } finally {
                _workerDone = true;
            }

        }

        void installworker_DoWork(object sender, DoWorkEventArgs dwea) {
            StringBuilder sb = new StringBuilder();
            var wa = dwea.Argument as WorkerArguments;
            var worker = sender as BackgroundWorker;
            var e = wa.InstallEventArgs;

            try {
                doInstall(sender as BackgroundWorker, sb, (dwea.Argument as WorkerArguments));
            } finally {
                progressDone();
            }
        }

        void doInstall(BackgroundWorker worker, StringBuilder sb, WorkerArguments wa) {

            var e = wa.InstallEventArgs;

            // showMessageBox("Running in background = " + Thread.CurrentThread.IsBackground, "Background thread?", MessageBoxButtons.OK, MessageBoxIcon.Information);

            try {

                worker.ReportProgress(0, getDisplayMember("doInstall{verifying}", "Verifying database credentials..."));

                // init vars
                string engine = null;
                string instanceName = null;
                bool windowsAuth = false;
                DatabaseEngineUtil dbEngineUtil = null;
                string targetDir = null;
                string sourceDir = null;
                string gguacPath = null;
                string dataDestinationFolder = null;
                string superUserPassword = null;
                initContextVariables(worker, sb, e, ref engine, ref instanceName, ref windowsAuth, ref dbEngineUtil, ref targetDir, ref sourceDir, ref gguacPath, ref dataDestinationFolder, ref superUserPassword, false);


                // prepare the x64-specific exe in case we need to use it to look up registry keys that can only be retrieved from a 64-bit process on a 64-bit OS
                worker.ReportProgress(0, getDisplayMember("doInstall{verifying}", "Verifying database credentials..."));
                worker.ReportProgress(0, getDisplayMember("doInstall{extracting}", "Extracting utilities..."));
                extractUtility64File(targetDir, gguacPath);

                // drop existing db, if any
                bool uninstalledSuccessfully = true;
                try {
                    // uninstall previous version if needed
                    uninstalledSuccessfully = doUninstall(worker, sb, dbEngineUtil, false, ref superUserPassword, e);
                } catch {
                    // eat any errors from uninstall
                }

                if (!uninstalledSuccessfully) {
                    throw new ApplicationException(getDisplayMember("doInstall{uninstallfailed}", "Uninstall of previous version of GRIN-Global database failed.\nPlease uninstall it manually by going to Add/Remove Programs.")); 
                }



                // prepare to talk to the database engine

                //determineSuperUserPassword(dbEngineUtil, instanceName, ref superUserPassword);
                string connectionString = dbEngineUtil.GetDataConnectionSpec("gringlobal", "__USERID__", "__PASSWORD__").ConnectionString;
                //Utility.SaveDatabaseSettings(dbEngineUtil.EngineName, dbEngineUtil.UseWindowsAuthentication, instanceName, connectionString);


                // make sure no previous data is pulled in to current data load
                Toolkit.EmptyDirectory(dataDestinationFolder);

                // extract system data that was included in the msi
                extractSystemDataFile(worker, sb, targetDir, gguacPath, dataDestinationFolder);

                // 2010-10-14 Brock Weaver brock@circaware.com
                // disabled, no longer necessary for testing...
                // prompt for additional data
                promptForAndExtractAdditionalData(worker, sb, dataDestinationFolder, gguacPath);


                // create and populate database
                worker.ReportProgress(0, getDisplayMember("doInstall{starting}", "Starting install to database engine={0}, service={1}, connection string={2} ...", dbEngineUtil.EngineName, dbEngineUtil.ServiceName, connectionString));
                sb.AppendLine("Creating gringlobal database");
                createDatabase(worker, dbEngineUtil, targetDir, superUserPassword);

                Creator c = null;
                List<TableInfo> tables = null;

                // create tables / fill with data / create users / create indexes / create constraints
                populateDatabase(worker, sb, dbEngineUtil, ref c, ref tables, dataDestinationFolder, targetDir, superUserPassword);

                // create sequences / triggers as needed
                createDependentDatabaseObjects(worker, sb, dbEngineUtil, c, tables, targetDir, superUserPassword);

                // perform any clean up
                finalizeDatabaseInstall(worker, sb, dbEngineUtil, c, tables, superUserPassword, targetDir);

                // showMessageBox(worker, "Database created successfully.", "Database Created", MessageBoxButtons.OK, MessageBoxIcon.Information);


            } catch (Exception ex) {

                try {
                    sb.AppendLine("Current Directory: " + Environment.CurrentDirectory);
                    sb.AppendLine("Current Username: " + DomainUser.Current);
                    //showMessageBox(worker, "Error during install: " + ex.Message, "Installation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error); // + "\n\nHistory: " + sb.ToString(), "Exception during install", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    var msg = "Exception: " + ex.Message + "\n\nHistory: " + sb.ToString() + "\nException stack: " + ex.ToString(true);
                    EventLog.WriteEntry("GRIN-Global Database", Toolkit.Cut(msg, 0, 8000), EventLogEntryType.Error);
                } catch { 
                    // eat all errors writing to the event log
                }


                var exWrapper = new ApplicationException("Exception: " + ex.Message, ex);

                //try {
                //    string[] actions = sb.ToString().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                //    foreach (string act in actions) {
                //        this.Context.LogMessage(act);
                //    }
                //} catch {
                //}
                //this.Rollback(e.SavedState);
                waitForUserInput(worker, exWrapper, int.MaxValue); // wait forever?
                wa.Exception = ex;
                return;
            }


        }

        private void DatabaseInstaller_BeforeUninstall(object sender, InstallEventArgs e) {

            try {
                initProgressForm();


                // HACK: try to give MSI form a moment to focus before we show our dialog...
                Thread.Sleep(1000);
                Application.DoEvents();
                Thread.Sleep(500);

                int installerWindowHandle = Toolkit.GetWindowHandle("GRIN-Global Database");
                if (installerWindowHandle > 0) {
                    Toolkit.MinimizeWindow(installerWindowHandle);
                }


                using (var worker = initWorker()) {
                    var wa = new WorkerArguments { InstallEventArgs = e };
                    worker.DoWork += new DoWorkEventHandler(uninstallworker_DoWork);
                    try {
                        worker.RunWorkerAsync(wa);
                        waitForWorkerToComplete(worker);
                    } finally {
                        try {
                            if (installerWindowHandle > 0) {
                                Toolkit.RestoreWindow(installerWindowHandle);
                            }
                        } catch { }
                        // exceptions thrown in the background worker thread won't cause us to bomb here.
                        // so the code running in background worker context needs to tell us if it encountered an exception.
                        // we rethrow it on the primary process thread here if needed.
                        if (wa.Exception != null) {
                            throw wa.Exception;
                        }
                    }
                }
            } catch (Exception ex) {
                EventLog.WriteEntry("GRIN-Global Database", Toolkit.Cut("Exception in DatabaseInstaller_BeforeUninstall: " + ex.Message, 0, 8000), EventLogEntryType.Error);
                throw;
            } finally {
                _workerDone = true;
            }
            
        }

        void uninstallworker_DoWork(object sender, DoWorkEventArgs dwea) {
            StringBuilder sb = new StringBuilder();
            string superUserPassword = null;
            try {
                // uninstall previous version if needed
                doUninstall(sender as BackgroundWorker, sb, null, true, ref superUserPassword, (dwea.Argument as WorkerArguments).InstallEventArgs);
            } catch (Exception ex) {
                // eat any errors from uninstall
            } finally {
                progressDone();
            }

        }

        private bool IsUpgrade {
            get {
                return !string.IsNullOrEmpty(this.Context.Parameters["oldproductcode"]);
            }
        }


        private bool doUninstall(BackgroundWorker worker, StringBuilder sb, DatabaseEngineUtil dbEngineUtil, bool atUninstallTime, ref string superUserPassword, InstallEventArgs e) {
            //showState(e.SavedState);

            try {

                //if (atUninstallTime && !IsUpgrade) {
                //    var dr1 = showMessageBox(worker, "Uninstalling the GRIN-Global Database software does not require you to delete the corresponding database and its data.\nDo you want to permanently delete all GRIN-Global data?\n\nAnswering Yes or No will still uninstall this software.", "Delete GRIN-Global Data?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                //    switch (dr1) {
                //        case DialogResult.Yes:
                //            // just continue with the uninstall as normal
                //            break;
                //        case DialogResult.No:
                //            // skip all the database removal, let them keep their data as they asked
                //            return true;
                //        case DialogResult.Cancel:
                //        default:
                //            throw new InvalidOperationException("User cancelled out of delete data prompt.");
                //    }
                //}

                worker.ReportProgress(0, getDisplayMember("doUninstall{verifying}", "Verifying database credentials..."));
                // init vars
                string engine = null;
                string instanceName = null;
                bool windowsAuth = false;
                string targetDir = null;
                string sourceDir = null;
                string gguacPath = null;
                string dataDestinationFolder = null;
                initContextVariables(worker, sb, e, ref engine, ref instanceName, ref windowsAuth, ref dbEngineUtil, ref targetDir, ref sourceDir, ref gguacPath, ref dataDestinationFolder, ref superUserPassword, atUninstallTime);

                // bounce db engine to make sure all existing connections are severed
                //restartDatabaseEngine(worker, sb, dbEngineUtil, engine, instanceName, windowsAuth, superUserPassword, e);


                if (dbEngineUtil == null && atUninstallTime) {
                    // user chose to 'skip' database stuff.
                    // so, we don't try to do any database user or data removal
                    EventLog.WriteEntry("GRIN-Global Database", "User chose to skip removing the database at uninstall time.", EventLogEntryType.Information);
                    return true;
                }

                string output = "";

                if (dbEngineUtil is PostgreSqlEngineUtil)
                {
                    // Kill session
                    string killSessionSql = "SELECT pg_terminate_backend(pg_stat_activity.procpid) FROM pg_stat_activity WHERE pg_stat_activity.datname = 'gringlobal';";
                    dbEngineUtil.ExecuteSql(superUserPassword, "postgres", killSessionSql);
                    // Drop database
                    dropDatabase(worker, sb, dbEngineUtil, targetDir, superUserPassword);
                    // Drop users
                    dropDatabaseUsers(worker, sb, dbEngineUtil, superUserPassword);
                }
                else
                {
                    dropDatabaseUsers(worker, sb, dbEngineUtil, superUserPassword);

                    // launch database-aware scripts for dropping database
                    try
                    {
                        output = dropDatabase(worker, sb, dbEngineUtil, targetDir, superUserPassword);
                    }
                    catch (Exception exDrop)
                    {
                        if (atUninstallTime)
                        {
                            //Context.LogMessage("Exception dropping database: " + exDrop.Message);
                            EventLog.WriteEntry("GRIN-Global Database", "Exception dropping database: " + exDrop.Message, EventLogEntryType.Error);
                            //this.Rollback(e.SavedState);
                            throw;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                // bounce db engine to make sure all existing connections are severed
                // restartDatabaseEngine(worker, sb, dbEngineUtil, engine, instanceName, windowsAuth, superUserPassword, e);

                // delete the utility64 file, if any
                if (atUninstallTime) {
                    try {
                        var utility64exe = Toolkit.ResolveFilePath(targetDir + @"\ggutil64.exe", false);
                        if (File.Exists(utility64exe)) {
                            File.Delete(utility64exe);
                        }
                    } catch {
                        // eat any errors
                    }
                }

            } catch (Exception ex2) {
                // Context.LogMessage("Exception: " + ex2.Message);
                if (atUninstallTime) {
                    //this.Rollback(e.SavedState);
                    // MessageBox.Show("The following error occurred when attempting to uninstall GRIN-Global Database: " + ex2.Message + "\n\nHowever, uninstallation will continue.");
                    // throw;
                    EventLog.WriteEntry("GRIN-Global Database", "Error uninstalling database: " + ex2.Message, EventLogEntryType.Error);
                    waitForUserInput(worker, new ApplicationException(getDisplayMember("doUninstall{atuninstalltime}", "The following error occurred when attempting to uninstall GRIN-Global Database: {0}\n\nHowever, uninstallation will continue.", ex2.Message), ex2), int.MaxValue); // wait forever?
                    return false;
                } else {
                    waitForUserInput(worker, new ApplicationException(ex2.Message, ex2), int.MaxValue);  // wait forever?
                    return false;
                }
            }

            return true;

        }

        private void extractUtility64File(string targetDir, string gguacPath) {
            var utility64CabPath = Toolkit.ResolveFilePath(targetDir + @"\utility64.cab", false);
            var utility64ExePath = Toolkit.ResolveFilePath(targetDir + @"\ggutil64.exe", false);

            var tempPath = Utility.GetTempDirectory(15);

            if (!File.Exists(utility64ExePath)) {
                if (File.Exists(utility64CabPath)) {
                    // wipe out any existing utility64.exe file in the temp folder
                    var extracted = Toolkit.ResolveFilePath(tempPath + @"\ggutil64.exe", false);
                    if (File.Exists(extracted)) {
                        File.Delete(extracted);
                    }
                    var extracted2 = Toolkit.ResolveFilePath(tempPath + @"\ggutil64.pdb", false);
                    if (File.Exists(extracted2)) {
                        File.Delete(extracted2);
                    }
                    // extract it from our cab
                    var cabOutput = Utility.ExtractCabFile(utility64CabPath, tempPath, gguacPath);
                    // move it to the final target path (we can't do this up front because expand.exe tells us "can't expand cab file over itself" for some reason.
                    if (File.Exists(extracted)) {
                        File.Move(extracted, utility64ExePath);
                    }
                }
            }
        }

        private void finalizeDatabaseInstall(BackgroundWorker worker, StringBuilder sb, DatabaseEngineUtil dbEngineUtil, Creator c, List<TableInfo> tables, string superUserPassword, string targetDir) {
            // each database engine has its own nuances after dumping a bunch of data into it...
            if (dbEngineUtil is MySqlEngineUtil) {
                // rebuild the indexes
                sb.AppendLine("Rebuilding indexes");
                worker.ReportProgress(0, getDisplayMember("finalizeDatabaseInstall{rebuilding}", "Rebuilding indexes..."));
                c.RebuildIndexes(tables);

                if ((dbEngineUtil.Version + "").Trim().StartsWith("5.")) {

                    // mysql versions < 6.0 do not support unique indexes with null columns properly.
                    // so we fudge this by creating triggers to throw errors when data violates the unique index
                    // http://www.brokenbuild.com/blog/2006/08/15/mysql-triggers-how-do-you-abort-an-insert-update-or-delete-with-a-trigger/

                    worker.ReportProgress(0, getDisplayMember("finalizeDatabaseInstall{triggers}", "Creating triggers to compensate for nullable unique indexes..."));
                    dbEngineUtil.ExecuteSqlFile(superUserPassword, "gringlobal", Toolkit.ResolveFilePath(targetDir + @"\mysql_create_triggers.sql", false)); 

                }

            } else if (dbEngineUtil is SqlServerEngineUtil) {

                // SQL Server doesn't need to do anything here...

            } else if (dbEngineUtil is OracleEngineUtil) {

                // Oracle doesn't need to do anything here...

            } else if (dbEngineUtil is PostgreSqlEngineUtil) {

                // Postgresql doesn't need to do anything here...
            }

        }

        private void populateDatabase(BackgroundWorker worker, StringBuilder sb, DatabaseEngineUtil dbEngineUtil, ref Creator c, ref List<TableInfo> tables, string dataDestinationFolder, string targetDir, string superUserPassword) {

            sb.AppendLine("Getting instance of creator");
            // read schema info from the unzipped files

            if (dbEngineUtil is OracleEngineUtil) {
                // The built-in oracleconnection class in .NET uses OCI (Oracle Callable Interface) to do the actual work.
                // OCI does not allow logins to use AS SYSDBA or AS SYSOPER.
                // Since all the work we're doing will be within the gringlobal user's table space, we can login as them and do it instead.
                // (we initially create the database using the sql_plus.exe itself, which does support AS SYSDBA or AS SYSOPER)
                c = Creator.GetInstance(dbEngineUtil.GetDataConnectionSpec("gringlobal", "gringlobal", "TempPA55"));
            } else {
                c = Creator.GetInstance(dbEngineUtil.GetDataConnectionSpec("gringlobal", dbEngineUtil.SuperUserName, superUserPassword));
            }
            c.Worker = worker;
            c.OnProgress += new ProgressEventHandler(c_OnProgress);

            sb.AppendLine("Reading schema information");
            worker.ReportProgress(0, getDisplayMember("populateDatabase{readingschema}", "Reading schema information..."));
            // __schema.xml is in the installation path...
            tables = c.LoadTableInfo(dataDestinationFolder);

            sb.AppendLine("Creating tables");
            worker.ReportProgress(0, getDisplayMember("populateDatabase{creatingtables}", "Creating tables..."));

            //showMessageBox(worker, c.DataConnectionSpec.ToString(), "Connection Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            c.CreateTables(tables, "gringlobal");
            sb.AppendLine("Loading data");
            worker.ReportProgress(0, getDisplayMember("populateDatabase{loadingdata}", "Loading data..."));
            // actual data files are in the data destination folder created when we extracted the cab files...
            c.CopyDataToDatabase(dataDestinationFolder, "gringlobal", tables, false, false);


            // create users / assign rights
            createDatabaseUsers(worker, sb, dbEngineUtil, c, tables, targetDir, superUserPassword);


            //// reload table info, CopyDataToDatabase empties out the collection
            //tables = c.LoadTableInfo(dataDestinationFolder);

            sb.AppendLine("Creating indexes...");
            worker.ReportProgress(0, getDisplayMember("populateDatabase{creatingindexes}", "Creating indexes..."));
            c.CreateIndexes(tables);


            // 2010-09-18 Brock Weaver brock@circaware.com
            // HACK: since this installer code does not support pulling in portions of a table, we need to make sure the only data that
            //       exists is what the user chose.  This problem only occurs in tables which have two FKs, each of which are in different
            //       cab files (i.e. citation_map point at both taxonomy tables and accession tables, but those are in different cabs. Putting
            //       citation_map in either cab doesn't solve the problem.)
            //       The solution we're going to use is hardcode here those certain tables that straddle cab files and remove records as needed
            //       after the data was imported and indexes created (just above) but before constraints are applied (just after this method call)
            scrubTablesThatPointAtMissingData(worker, dataDestinationFolder, "gringlobal", c.DataConnectionSpec);


            sb.AppendLine("Creating foreign key constraints");
            worker.ReportProgress(0, getDisplayMember("populateDatabase{creatingforeignkeys}", "Creating foreign key constraints..."));
            c.CreateConstraints(tables, true, true);
        }

        private void scrubTablesThatPointAtMissingData(BackgroundWorker worker, string dataDestinationFolder, string databaseName, DataConnectionSpec dcs){


            // delete data from tables that straddle cab files that reference data the user chose not to install.  
            // case in point: the citation_map table...
            //  taxonomy_genus <-- citation_map --> accession

            using (var dm = DataManager.Create(dcs)) {

                var files = Directory.GetFiles(dataDestinationFolder, "*.txt");

//                if (!files.Contains(@"\taxonomy.txt")) {
//                    // user did not install taxonomy data.  remove almost all the cooperators so their base install is less cluttered.
//                    worker.ReportProgress(0, "Removing unnecessary cooperator information...");

//                    // rip out all cooperators that we don't absolutely require.
//                    // NOTE: current_cooperator_id makes this...interesting.
//                    var coopSubselect = @"
//        select coalesce(cooperator_id,-1) from sys_user
//
//        UNION
//        select coalesce(cooperator_id,-1) from cooperator where last_name = 'SYSTEM'
//
//        UNION
//        select coalesce(cooperator_id,-1) from cooperator_map
//
//        UNION
//        select coalesce(created_by,-1) from geography
//        UNION
//        select coalesce(modified_by,-1) from geography
//        UNION
//        select coalesce(owned_by,-1) from geography
//
//        UNION
//        select coalesce(created_by,-1) from geography_lang
//        UNION
//        select coalesce(modified_by,-1) from geography_lang
//        UNION
//        select coalesce(owned_by,-1) from geography_lang
//
//        UNION
//        select coalesce(created_by,-1) from geography_region_map
//        UNION
//        select coalesce(modified_by,-1) from geography_region_map
//        UNION
//        select coalesce(owned_by,-1) from geography_region_map
//
//        UNION
//        select coalesce(created_by,-1) from site
//        UNION
//        select coalesce(modified_by,-1) from site
//        UNION
//        select coalesce(owned_by,-1) from site
//
//        UNION
//        select coalesce(created_by,-1) from region
//        UNION
//        select coalesce(modified_by,-1) from region
//        UNION
//        select coalesce(owned_by,-1) from region
//
//        UNION
//        select coalesce(created_by,-1) from region_lang
//        UNION
//        select coalesce(modified_by,-1) from region_lang
//        UNION
//        select coalesce(owned_by,-1) from region_lang
//
//        UNION
//        select coalesce(created_by,-1) from code_value
//        UNION
//        select coalesce(modified_by,-1) from code_value
//        UNION
//        select coalesce(owned_by,-1) from code_value
//
//        UNION
//        select coalesce(created_by,-1) from code_value_lang
//        UNION
//        select coalesce(modified_by,-1) from code_value_lang
//        UNION
//        select coalesce(owned_by,-1) from code_value_lang
//
//        UNION
//        select coalesce(created_by,-1) from cooperator_group
//        UNION
//        select coalesce(modified_by,-1) from cooperator_group
//        UNION
//        select coalesce(owned_by,-1) from cooperator_group
//
//        UNION
//        select coalesce(created_by,-1) from cooperator_map
//        UNION
//        select coalesce(modified_by,-1) from cooperator_map
//        UNION
//        select coalesce(owned_by,-1) from cooperator_map
//
//";

//                    dm.Write(String.Format(@"
//delete from 
//    cooperator
//where
//    cooperator_id not in ({0})
//    and current_cooperator_id not in ({0})
//", coopSubselect));

//                }

                if (!files.Contains(@"\accession.txt")){
                    // user did not install accession data. remove records that point at accession data.
                    worker.ReportProgress(0, getDisplayMember("scrubTablesThatPointAtMissingData{removingaccession}", "Removing unnecessary accession citation information..."));
//                    dm.Write(@"
//delete from 
//    citation 
//where 
//    (accession_id is not null and accession_id not in (select accession_id from accession))
//    or (accession_ipr_id is not null and accession_ipr_id not in (select accession_ipr_id from accession_ipr))
//    or (accession_pedigree_id is not null and accession_pedigree_id not in (select accession_pedigree_id from accession_pedigree))
//");
                }

                if (!files.Contains(@"\genetic_marker.txt")) {
                    worker.ReportProgress(0, getDisplayMember("scrubTablesThatPointAtMissingData{removinggenetic}", "Removing unnecessary genetic citation information..."));
                    dm.Write(@"delete from citation where genetic_marker_id is not null and genetic_marker_id not in (select genetic_marker_id from genetic_marker)");
                }

                if (!files.Contains(@"\method.txt")) {
                    worker.ReportProgress(0, getDisplayMember("scrubTablesThatPointAtMissingData{removingmethod}", "Removing unnecessary method citation information..."));
                    dm.Write(@"delete from citation where method_id is not null and method_id not in (select method_id from method)");
                }

                if (!files.Contains(@"\order_request.txt")) {
                    // user did not install order data.  remove records that point at order data.
                    worker.ReportProgress(0, getDisplayMember("scrubTablesThatPointAtMissingData{removingaccession_annotation}", "Removing unnecessary accession_inv_annotation citation information..."));
                    dm.Write(@"delete from accession_inv_annotation where order_request_id is not null and order_request_id not in (select order_request_id from order_request)");

                    worker.ReportProgress(0, getDisplayMember("scrubTablesThatPointAtMissingData{removinginventory_viability}", "Removing unnecessary inventory_viability citation information..."));
                    dm.Write(@"delete from inventory_viability_data where order_request_item_id is not null and order_request_item_id not in (select order_request_item_id from order_request_item)");

                }


            }
        }

        private void promptForAndExtractAdditionalData(BackgroundWorker worker, StringBuilder sb, string dataDestinationFolder, string gguacPath) {

            // HACK: try to give MSI form a moment to focus before we show our dialog...
            Thread.Sleep(1000);
            Application.DoEvents();
            Thread.Sleep(500);

            // we may need to prompt user to see what data they want to download from the server and copy into the database
            var fid = new frmInstallData();

            DialogResult result = DialogResult.Cancel;
            var autoIncludeOptionalData = Utility.GetParameter("optionaldata", null, this.Context, null);
            if (("" + autoIncludeOptionalData).ToUpper() == "TRUE" || ("" + autoIncludeOptionalData).ToUpper() == "1") {
                // command line tells us to include optional data, don't prompt for it
                worker.ReportProgress(0, getDisplayMember("promptForAndExtractAdditionalData{autodownloading}", "Auto-downloading optional data..."));
                sb.AppendLine("Auto-downloading optional data...");
                fid.DownloadAllFiles();
                result = DialogResult.OK;
            } else if (("" + autoIncludeOptionalData).ToUpper() == "FALSE" || ("" + autoIncludeOptionalData).ToUpper() == "0"){
                worker.ReportProgress(0, getDisplayMember("promptForAndExtractAdditionalData{autoskipping}", "Auto-skipping download of optional data..."));
                sb.AppendLine("Auto-skipping download of optional data...");
                result = DialogResult.OK;
            } else {
                worker.ReportProgress(0, getDisplayMember("promptForAndExtractAdditionalData{prompting}", "Prompting for optional data..."));
                sb.AppendLine("Prompting for optional data...");
                result = fid.ShowDialog();
            }

            if (result == DialogResult.OK) {
                if (fid.DataFiles == null || fid.DataFiles.Count == 0) {
                    worker.ReportProgress(0, getDisplayMember("promptForAndExtractAdditionalData{nonselected}", "User did not select any optional data files."));
                    sb.AppendLine("User did not select any optional data files.");
                    Application.DoEvents();
                } else {
                    var splash = new frmSplash();
                    try {
                        var extracting = getDisplayMember("promptForAndExtractAdditionalData{extracting}", "Extracting optional data files...");
                        worker.ReportProgress(0, extracting);
                        sb.AppendLine("Extracting optional data files...");
                        splash.Show(extracting, false, null);
                        foreach (var s in fid.DataFiles) {
                            sb.AppendLine("Extracting files from " + s + " to folder " + dataDestinationFolder);
                            // extract each cab file
                            //                        string fn = Path.GetFileNameWithoutExtension(s).Replace("_", " ");

                            worker.ReportProgress(0, getDisplayMember("promptForAndExtractAdditionalData{extractingfile}", "Extracting {0} to {1}...", s, dataDestinationFolder));
                            splash.ChangeText(getDisplayMember("promptForAndExtractAdditionalData{extractingfilesplash}", "Extracting {0}\n\nPlease wait while data is extracted...", s));
                            Application.DoEvents();
                            Utility.ExtractCabFile(s, dataDestinationFolder, gguacPath);

                            // we expanded the cab file, now delete it since we don't need it anymore (and want the next install to re-request data from the server and ignore the cache)
                            try {
                                var moveTo = s.Replace(@"\downloaded\", @"\installed\");
                                if (File.Exists(moveTo)) {
                                    File.Delete(moveTo);
                                }
                                File.Move(s, moveTo);
                            } catch {
                                try {
                                    // move failed, try to delete it
                                    File.Delete(s);
                                } catch {
                                    // ultimately ignore all file movement errors
                                }
                            }
                        }
                    } finally {
                        splash.Close();
                        Application.DoEvents();
                    }
                }
            } else {
                EventLog.WriteEntry("GRIN-Global Database", "User cancelled out of optional data selection dialog", EventLogEntryType.Error);
                //Context.LogMessage("User cancelled out of optional data selection dialog");
                //this.Rollback(e.SavedState);
                throw new InvalidOperationException(getDisplayMember("promptForAndExtractAdditionalData{usercancel}", "User cancelled out of optional data selection dialog."));
            }

            if (_frmProgress != null) {
                Toolkit.ActivateApplication(_frmProgress.Handle);
            }
        }

        private void extractSystemDataFile(BackgroundWorker worker, StringBuilder sb, string targetDir, string gguacPath, string dataDestinationFolder) {

            //                string helperPath = (targetDir + @"\gguac.exe").Replace(@"\\", @"\");
            worker.ReportProgress(0, getDisplayMember("extractSystemDataFile", "Extracting system data file..."));
            sb.AppendLine("Extracting system data file...");
            var sourceCab = (targetDir + @"\system_data.cab").Replace(@"\\", @"\");
            var extractOutput = Utility.ExtractCabFile(sourceCab, dataDestinationFolder, gguacPath);



            // copy schema file to data dest folder
            string tgtXml = (dataDestinationFolder + @"\__schema.xml").Replace(@"\\", @"\");
            if (File.Exists(tgtXml)) {
                File.Delete(tgtXml);
            }
            File.Copy((targetDir + @"\__schema.xml").Replace(@"\\", @"\"), tgtXml);
        }

        private void createDatabase(BackgroundWorker worker, DatabaseEngineUtil dbEngineUtil, string targetDir, string superUserPassword) {
            // special case:
            // this is the first time we try to login to the database after the db engine is installed.
            // if we get user login problems, prompt them whether to bail or continue after they've taken some action.
            bool done = false;
            string output = null;
            while (!done) {
                try {


                    done = true;

                    output = dbEngineUtil.CreateDatabase(superUserPassword, "gringlobal");

                } catch (Exception exCreate) {

                    if (exCreate.Message.ToLower().Contains("local security authority cannot be contacted")) {
                        // special case: using windows login for sql server.
                        //               computer is part of a domain
                        //               computer can't authenticate with domain (i.e. laptop not VPN'd into network with the necessary domain controller)
                        // http://sqldbpool.wordpress.com/2008/09/05/%E2%80%9Ccannot-generate-sspi-context%E2%80%9D-error-message-more-comments-for-sql-server/
                        // http://blogs.msdn.com/sql_protocols/archive/2005/10/19/482782.aspx
                        EventLog.WriteEntry("GRIN-Global Database", "Exception on first pass of creating database: " + exCreate.Message, EventLogEntryType.Error);
                        var dr = showMessageBox(worker, getDisplayMember("createDatabase{windowsauthfailedbody}", "Windows login could not be authenticated.\nIf your computer is part of a Windows domain, please do either one of the following and try again:\n\nA) VPN into the network that hosts your Windows domain\nB) Disconnect from all networks\n\nAlso, you can try enabling Mixed Mode in SQL Server and logging in with the SQL Server 'sa' account.\n\nDo you want to try again?"), getDisplayMember("createDatabase{windowsauthfailedtitle}", "Could not verify login to an authority"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dr == DialogResult.Yes) {
                            done = false;
                        }

                    }

                    if (done) {
                        //Context.LogMessage("Exception creating database: " + exCreate.Message);
                        EventLog.WriteEntry("GRIN-Global Database", "Exception creating database: " + exCreate.Message, EventLogEntryType.Error);
                        showMessageBox(worker, getDisplayMember("createDatabase{exceptionbody}", "Exception creating database: {0}", exCreate.Message), getDisplayMember("createDatabase{exceptiontitle}", "Failed Creating Database"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //this.Rollback(e.SavedState);
                        throw;
                    }
                }
            }

        }

        private string createDependentDatabaseObjects(BackgroundWorker worker, StringBuilder sb, DatabaseEngineUtil dbEngineUtil, Creator c, List<TableInfo> tables, string targetDir, string superUserPassword) {

            if (dbEngineUtil is MySqlEngineUtil){

                // MySQL has nothing to do here
            } else if (dbEngineUtil is SqlServerEngineUtil){

                // SQL Server has nothing to do here

            } else if (dbEngineUtil is OracleEngineUtil) {

                // Oracle needs to create triggers and sequences...

                // get all the sequences up to snuff so new inserts work properly (i.e. make sure sequence id's are past the last one currently in each table)
                worker.ReportProgress(0, getDisplayMember("createDependentDatabaseObjects{sync}", "Synchronizing sequences with table data..."));

                var oraUtil = dbEngineUtil as OracleEngineUtil;

                foreach (var t in tables) {
                    worker.ReportProgress(0, getDisplayMember("createDependentDatabaseObjects{synctable}", "Synchronizing sequence for table {0}...", t.TableName));
                    var ret = oraUtil.RestartSequenceForTable(superUserPassword, "gringlobal", "TempPA55", t.TableName, t.PrimaryKey.Name);
                }
            } else if (dbEngineUtil is PostgreSqlEngineUtil) {

                // PostgreSQL needs to create triggers and sequences...
                
                // get all the sequences up to snuff so new inserts work properly (i.e. make sure sequence id's are past the last one currently in each table)
                worker.ReportProgress(0, getDisplayMember("createDependentDatabaseObjects{sync}", "Synchronizing sequences with table data..."));

                var pgUtil = dbEngineUtil as PostgreSqlEngineUtil;

                foreach (var t in tables) {
                    worker.ReportProgress(0, getDisplayMember("createDependentDatabaseObjects{synctable}", "Synchronizing sequence for table {0}...", t.TableName));
                    pgUtil.RestartSequenceForTable(superUserPassword, "gringlobal", null, t.TableName, t.PrimaryKey.Name);
                }
            }

            return "";
        }

        private string createDatabaseUsers(BackgroundWorker worker, StringBuilder sb, DatabaseEngineUtil dbEngineUtil, Creator c, List<TableInfo> tables, string targetDir, string superUserPassword) {
            // execute sql to create users

            worker.ReportProgress(0, getDisplayMember("createDatabaseUsers{create}", "Creating new gringlobal users..."));
            sb.AppendLine("Creating users");
            string output = null;
            try {
                //string userPassword = "gg_user_PA55w0rd!";
                //string searchPassword = "gg_search_PA55w0rd!";
                string userPassword = "PA55w0rd!";
                string searchPassword = "sPA55w0rd!";
                if (dbEngineUtil is SqlServerEngineUtil)
                {
                    worker.ReportProgress(0, getDisplayMember("createDatabaseUsers{grant}", "Granting rights to all gringlobal tables for users..."));
                    sb.AppendLine("Granting rights");

                    var machineName = Toolkit.Cut(Dns.GetHostName(), 0, 15);

                    // SQL Server, Integrated authority -- XP
                    output += dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "NETWORK SERVICE", machineName, null, false, true);
                    output += dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "ASPNET", machineName, null, false, true);

                    // SQL Server, Integrated authority -- Vista
                    output += dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "NETWORK SERVICE", "NT AUTHORITY", null, false, true);
                    output += dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "SYSTEM", "NT AUTHORITY", null, false, true);

                    // SQL Server, Integrated authority -- Windows 7
                    output += dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "DEFAULTAPPPOOL", "IIS AppPool", null, false, true);

                    // mixed mode support
                    output += dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "gg_user", machineName, userPassword, false, false);
                    output += dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "gg_search", machineName, searchPassword, true, false);

                } else if (dbEngineUtil is MySqlEngineUtil) {

                    // MySQL 
                    worker.ReportProgress(0, getDisplayMember("createDatabaseUsers{grant}", "Granting rights to all gringlobal tables for users..."));
                    sb.AppendLine("Granting rights");
                    output += dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "gg_user", "%", userPassword, false, false);
                    output += dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "gg_search", "%", searchPassword, true, false);

                } else if (dbEngineUtil is OracleEngineUtil) {


                    var res = dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "gg_user", null, userPassword, false, false);
                    output += res;
                    //showMessageBox(worker, res, "Create user gg_user result", MessageBoxButtons.OK, MessageBoxIcon.None);


                    res = dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "gg_search", null, searchPassword, true, false);
                    output += res;
                    //showMessageBox(worker, res, "Create user gg_search result", MessageBoxButtons.OK, MessageBoxIcon.None);


                    // also oracle requires us to enumerate each table to grant rights...
                    sb.AppendLine("Granting rights");
                    foreach (var ti in tables) {
                        worker.ReportProgress(0, getDisplayMember("createDatabaseUsers{granttable}", "Granting rights to {0} for necessary users...", ti.TableName));
                        //dbEngineUtil.GrantRightsToTable(superUserPassword, "gringlobal", "gringlobal", null, ti.TableName, false);
                        var grtt = dbEngineUtil.GrantRightsToTable(superUserPassword, "gringlobal", "gg_user", null, ti.TableName, false);
                        output += grtt;
                        grtt = dbEngineUtil.GrantRightsToTable(superUserPassword, "gringlobal", "gg_search", null, ti.TableName, true);
                        output += grtt;
                    }
                } else if (dbEngineUtil is PostgreSqlEngineUtil) {

                    // PostgreSQL
                    output += dbEngineUtil.CreateUser(superUserPassword, null, "gg_user", "localhost", userPassword, false, false);
                    output += dbEngineUtil.CreateUser(superUserPassword, null, "gg_search", "localhost", searchPassword, true, false);

                    // postgresql requires us to enumerate each table to grant rights...
                    sb.AppendLine("Granting rights");
                    foreach (var ti in tables) {
                        worker.ReportProgress(0, getDisplayMember("createDatabaseUsers{grantrightstable}", "Granting rights to {0} for necessary users...", ti.TableName));
                        
                        dbEngineUtil.GrantRightsToTable(superUserPassword, "gringlobal", "gg_user", null, ti.TableName, false);
                        dbEngineUtil.GrantRightsToTable(superUserPassword, "gringlobal", "gg_search", null, ti.TableName, true);

                    }
                } else if (dbEngineUtil is SqliteEngineUtil){
                    // sqlite has no concept of users, just ignore...
                } else {
                    throw new NotImplementedException(getDisplayMember("createDatabaseUsers{notimplemented}", null, "createDatabaseUsers is not implemented for {0}", dbEngineUtil.EngineName));
                }

            } catch (Exception exCreateUsers) {
                //Context.LogMessage("Exception creating users: " + exCreateUsers.Message);
                EventLog.WriteEntry("GRIN-Global Database", "Exception creating users: " + exCreateUsers.Message, EventLogEntryType.Error);
                showMessageBox(worker, getDisplayMember("createDatabaseUsers{exceptionbody}", "Exception creating users: {0}", exCreateUsers.Message), getDisplayMember("createDatabaseUsers{exceptiontitle}", "Creating Users Failed"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                throw;
            }


            return output;
        }

        private void restartDatabaseEngine(BackgroundWorker worker, StringBuilder sb, DatabaseEngineUtil dbEngineUtil, string engine, string instanceName, bool windowsAuth, string superUserPassword, InstallEventArgs e) {
            bool restartDone = false;
            while (!restartDone) {
                if (dbEngineUtil == null) {
                    dbEngineUtil = DatabaseEngineUtil.CreateInstance(engine, Utility.GetTargetDirectory(Context, e.SavedState, "Database") + @"\gguac.exe", instanceName);
                    dbEngineUtil.UseWindowsAuthentication = windowsAuth;
                }

                if (dbEngineUtil is SqlServerEngineUtil) {
                    var sqlserverUtil = (dbEngineUtil as SqlServerEngineUtil);
                    sqlserverUtil.InstanceName = instanceName;
                    sqlserverUtil.RefreshProperties();
                }
                var conn = dbEngineUtil.GetDataConnectionSpec("gringlobal", dbEngineUtil.SuperUserName, superUserPassword).ConnectionString;
                var maskedPasswordConnString = (String.IsNullOrEmpty(superUserPassword) ? conn : conn.Replace(superUserPassword, "*".PadLeft(superUserPassword.Length, '*')));
                worker.ReportProgress(0, getDisplayMember("restartDatabaseEngine", "Attempting to restart database engine (service name={0}, connectionstring={1}) to close all existing connections...", dbEngineUtil.ServiceName, maskedPasswordConnString));
                try {
                    dbEngineUtil.StopService();
                    dbEngineUtil.StartService();
                    restartDone = true;
                } catch (Exception exReboot) {
                    //Context.LogMessage("Could not restart db engine before dropping databases: " + exReboot.Message);
                    EventLog.WriteEntry("GRIN-Global Database", "Could not restart db engine before dropping databases: " + exReboot.Message, EventLogEntryType.Error);
                    if (DialogResult.Yes != showMessageBox(worker, getDisplayMember("restartDatabaseEngine{exceptionbody}",  "Unable to restart the database engine. Error:\n{0}\n\nInstallation cannot proceed until the database engine is restarted.\nDo you want to try again?", exReboot.Message), getDisplayMember("restartDatabaseEngine{exceptiontitle}", "Restart Database Engine Again?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                        restartDone = true;
                        throw;
                    } else {
                        worker.ReportProgress(0, getDisplayMember("restartDatabaseEngine", "Attempting to restart database engine (service name={0}) to close all existing connections...", dbEngineUtil.ServiceName));
                    }
                }
            }

        }

        private string dropDatabase(BackgroundWorker worker, StringBuilder sb, DatabaseEngineUtil dbEngineUtil, string targetDir, string superUserPassword) {

            string output = "";

            try {
                worker.ReportProgress(0, getDisplayMember("dropDatabase{start}", "Dropping any existing gringlobal databases..."));
                sb.AppendLine("Dropping databases");

                //var existingDatabases = dbEngineUtil.ListDatabases(superUserPassword);

                //foreach (var db in existingDatabases) {

                //}


                if (dbEngineUtil is SqlServerEngineUtil) {

                    // there should be no gringlobal*.mdf data files now.
                    // if there are, physically move them out of the way (tack on date/time plus our little extension).
                    string dataDir = dbEngineUtil.DataDirectory;
                    if (Directory.Exists(dataDir)) {

                        // first, try to drop all databases that have 'gringlobal' in their name
                        string[] files = Directory.GetFiles(dataDir);

                        worker.ReportProgress(0, getDisplayMember("dropDatabase{scan}", "Scanning data directory={0} for existing gringlobal database files, found {1} to inspect...", dataDir, files.Length.ToString()));

                        foreach (string f in files) {
                            if (f.ToLower().Contains("gringlobal") && !f.ToLower().EndsWith(".ggbackup")) {

                                if (f.ToLower().EndsWith(".mdf")) {

                                    // try to drop it first (eat any failures)
                                    string lastDB = Path.GetFileNameWithoutExtension(f.ToLower());
                                    string lastSql = null;
                                    try {
                                        worker.ReportProgress(0, getDisplayMember("dropDatabase{dropping}", "Dropping database '{0}'...", lastDB));
                                        //lastSql = "ALTER DATABASE " + lastDB + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE " + lastDB + ";";
                                        lastSql = "ALTER DATABASE " + lastDB + " SET OFFLINE WITH ROLLBACK IMMEDIATE; DROP DATABASE " + lastDB + ";";
                                        //lastSql = "DROP DATABASE " + lastDB + ";";
                                        dbEngineUtil.DropDatabase(superUserPassword, lastDB);
                                        //dbEngineUtil.ExecuteSql(superUserPassword, null, lastSql);
                                        //dbEngineUtil.ExecuteSql(_superUserPassword, false, "DROP DATABASE " + lastDB);
                                    } catch (Exception exDB) {
                                        EventLog.WriteEntry("GRIN-Global Database", "Error while trying to drop database '" + lastDB + "': " + exDB.Message, EventLogEntryType.Error);
                                        //Context.LogMessage("Could not drop database '" + lastDB + ": " + lastSql + "\n" + exDB.Message);
                                        // showMessageBox(worker, "Could not drop existing database named '" + lastDB + "':\n" + lastSql + "\nPlease close any applications that may have connections open against it (e.g. SQL Server Management Studio) and try running this installation again.", "Unable to Drop Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        // throw;
                                    }
                                }
                            }
                        }

                        // next, try to rename any files that have 'gringlobal' in their name (in case db file exists but isn't attached in sql server).
                        // ignore ones ending with '.ggbackup' because those are ones we moved at some previous time.
                        files = Directory.GetFiles(dataDir);
                        foreach (string f in files) {
                            try {
                                if (f.ToLower().Contains("gringlobal") && !f.ToLower().EndsWith(".ggbackup")) {
                                    // if the drop database failed (e.g. db was not attached), we still want to rename it so our next install goes smoothly
                                    worker.ReportProgress(0, getDisplayMember("dropDatabase{rename}", "Renaming old database file '{0}'...", f));
                                    File.Move(f, f + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".ggbackup");
                                }
                            } catch {
                                // eat all errors during a drop or file move
                            }
                        }
                    }

                } else if (dbEngineUtil is OracleEngineUtil){

                    // Oracle user == our concept of database.
                    // drop 'GRINGLOBAL' user to get rid of database.

                    dbEngineUtil.DropDatabase(superUserPassword, "gringlobal");

                } else {
                    // MySQL / PostgreSQL / sqlite
                    try {
                        worker.ReportProgress(0, getDisplayMember("dropDatabase{dropping}", "Dropping database gringlobal..."));
                        output = dbEngineUtil.DropDatabase(superUserPassword, "gringlobal");
                    } catch {
                        // eat all errors during a database drop
                    }
                }

            } catch (Exception ex){
                // ignore all exceptions when dropping databases
                Debug.Write(ex.Message);
                //Context.LogMessage("Exception dropping database: " + ex.Message);
                EventLog.WriteEntry("GRIN-Global Database", "Exception dropping database: " + ex.Message, EventLogEntryType.Error);
                worker.ReportProgress(0, "Failed to drop database: " + ex.Message);
                //waitForUserInput();
            } finally {
                worker.ReportProgress(0, getDisplayMember("dropDatabase{completed}", "Dropping database(s) completed."));
            }

            return output;

        }

        private void dropDatabaseUsers(BackgroundWorker worker, StringBuilder sb, DatabaseEngineUtil dbEngineUtil, string superUserPassword) {
            worker.ReportProgress(0, getDisplayMember("dropDatabaseUsers{start}", "Dropping any existing gringlobal users..."));
            sb.AppendLine("Dropping users");

            try {

                var machineName = Toolkit.Cut(Dns.GetHostName(), 0, 15);

                if (dbEngineUtil is SqlServerEngineUtil){
                    if (dbEngineUtil.UseWindowsAuthentication) {
                        //// SQL Server, Integrated authority -- XP
                        //dbEngineUtil.DropUser(superUserPassword, "NETWORK SERVICE", machineName);
                        //dbEngineUtil.DropUser(superUserPassword, "ASPNET", machineName);

                        //// SQL Server, Integrated authority -- Vista
                        //dbEngineUtil.DropUser(superUserPassword, "NETWORK SERVICE", "NT AUTHORITY");
                        //// SQL Server, Integrated authority -- Windows 7
                        //dbEngineUtil.DropUser(superUserPassword, "DEFAULTAPPPOOL", "IIS AppPool");
                    } else {
                        // SQL Server mixed mode
                        try {
                            dbEngineUtil.DropUser(superUserPassword, "gringlobal", "gg_user", "localhost");
                            worker.ReportProgress(0, getDisplayMember("dropDatabaseUsers{gg_user}", "Dropped user gg_user"));
                        } catch {
                        }
                        try {
                            dbEngineUtil.DropUser(superUserPassword, "gringlobal", "gg_search", "localhost");
                            worker.ReportProgress(0, getDisplayMember("dropDatabaseUsers{gg_search}", "Dropped user gg_search"));
                        } catch {
                        }
                    }

                } else if (dbEngineUtil is MySqlEngineUtil){
                    // MySQL 
                    try {
                        dbEngineUtil.DropUser(superUserPassword, "gringlobal", "gg_user", "%");
                            worker.ReportProgress(0, getDisplayMember("dropDatabaseUsers{gg_user}", "Dropped user gg_user"));
                    } catch {
                    }
                    try {
                        dbEngineUtil.DropUser(superUserPassword, "gringlobal", "gg_search", "%");
                            worker.ReportProgress(0, getDisplayMember("dropDatabaseUsers{gg_user}", "Dropped user gg_search"));
                    } catch {
                    }
                } else if (dbEngineUtil is OracleEngineUtil){
                    try {
                        dbEngineUtil.DropUser(superUserPassword, "gringlobal", "gg_user", "localhost");
                            worker.ReportProgress(0, getDisplayMember("dropDatabaseUsers{gg_user}", "Dropped user gg_user"));
                    } catch {
                    }
                    try {
                        dbEngineUtil.DropUser(superUserPassword, "gringlobal", "gg_search", "localhost");
                            worker.ReportProgress(0, getDisplayMember("dropDatabaseUsers{gg_user}", "Dropped user gg_search"));
                    } catch {
                    }

                } else if (dbEngineUtil is SqliteEngineUtil){
                    // sqlite knows nothing of users
                } else if (dbEngineUtil is PostgreSqlEngineUtil) {
                    // PostgreSQL
                    try {
                        dbEngineUtil.DropUser(superUserPassword, "postgres", "gg_user", "localhost");
                            worker.ReportProgress(0, getDisplayMember("dropDatabaseUsers{gg_user}", "Dropped user gg_user"));
                    } catch {
                    }
                    try {
                        dbEngineUtil.DropUser(superUserPassword, "postgres", "gg_search", "localhost");
                            worker.ReportProgress(0, getDisplayMember("dropDatabaseUsers{gg_user}", "Dropped user gg_search"));
                    } catch {
                    }
                } else {
                    throw new NotImplementedException(getDisplayMember("dropDatabaseUsers{notimplemented}", "Dropping users for {0} is not implemented in DatabaseInstaller.dropDatabaseUsers", dbEngineUtil.EngineName));
                }

            } catch (Exception exDropUsers) {
                Debug.WriteLine(exDropUsers.Message);
                //Context.LogMessage("Exception dropping users: " + exDropUsers.Message);
                //EventLog.WriteEntry("GRIN-Global Database", "Exception dropping users: " + exDropUsers.Message, EventLogEntryType.Error);
                // worker.ReportProgress(0, exDropUsers);
                // waitForUserInput();
            } finally {
                worker.ReportProgress(0, getDisplayMember("dropDatabaseUsers{done}", "Dropping user(s) complete."));
            }
        }



        //        private string _defaultMyIniContents = @"
        //# MySQL Server Instance Configuration File
        //# ----------------------------------------------------------------------
        //# Generated by the MySQL Server Instance Configuration Wizard
        //#
        //#
        //# Installation Instructions
        //# ----------------------------------------------------------------------
        //#
        //# On Linux you can copy this file to /etc/my.cnf to set global options,
        //# mysql-data-dir/my.cnf to set server-specific options
        //# (@localstatedir@ for this installation) or to
        //# ~/.my.cnf to set user-specific options.
        //#
        //# On Windows you should keep this file in the installation directory 
        //# of your server (e.g. C:\Program Files\MySQL\MySQL Server X.Y). To
        //# make sure the server reads the config file use the startup option 
        //# ""--defaults-file"". 
        //#
        //# To run run the server from the command line, execute this in a 
        //# command line shell, e.g.
        //# mysqld --defaults-file=""C:\Program Files\MySQL\MySQL Server X.Y\my.ini""
        //#
        //# To install the server as a Windows service manually, execute this in a 
        //# command line shell, e.g.
        //# mysqld --install MySQLXY --defaults-file=""C:\Program Files\MySQL\MySQL Server X.Y\my.ini""
        //#
        //# And then execute this in a command line shell to start the server, e.g.
        //# net start MySQLXY
        //#
        //#
        //# Guildlines for editing this file
        //# ----------------------------------------------------------------------
        //#
        //# In this file, you can use all long options that the program supports.
        //# If you want to know the options a program supports, start the program
        //# with the ""--help"" option.
        //#
        //# More detailed information about the individual options can also be
        //# found in the manual.
        //#
        //#
        //# CLIENT SECTION
        //# ----------------------------------------------------------------------
        //#
        //# The following options will be read by MySQL client applications.
        //# Note that only client applications shipped by MySQL are guaranteed
        //# to read this section. If you want your own MySQL client program to
        //# honor these values, you need to specify it as an option during the
        //# MySQL client library initialization.
        //#
        //[client]
        //
        //port=3306
        //
        //[mysql]
        //
        //default-character-set=utf8
        //
        //
        //# SERVER SECTION
        //# ----------------------------------------------------------------------
        //#
        //# The following options will be read by the MySQL Server. Make sure that
        //# you have installed the server correctly (see above) so it reads this 
        //# file.
        //#
        //[mysqld]
        //
        //# The TCP/IP Port the MySQL Server will listen on
        //port=3306
        //
        //
        //#Path to installation directory. All paths are usually resolved relative to this.
        //#basedir=""C:/Program Files/MySQL/MySQL Server 5.1/""
        //basedir=""__BASEDIR__""
        //
        //
        //#Path to the database root
        //datadir=""__DATADIR__""
        //#datadir=""C:/Documents and Settings/All Users/Application Data/MySQL/MySQL Server 5.1/Data/""
        //
        //# The default character set that will be used when a new schema or table is
        //# created and no character set is defined
        //default-character-set=utf8
        //
        //# The default storage engine that will be used when create new tables when
        //default-storage-engine=INNODB
        //
        //# Set the SQL mode to strict
        //sql-mode=""STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION""
        //
        //# The maximum amount of concurrent sessions the MySQL server will
        //# allow. One of these connections will be reserved for a user with
        //# SUPER privileges to allow the administrator to login even if the
        //# connection limit has been reached.
        //max_connections=100
        //
        //# Query cache is used to cache SELECT results and later return them
        //# without actual executing the same query once again. Having the query
        //# cache enabled may result in significant speed improvements, if your
        //# have a lot of identical queries and rarely changing tables. See the
        //# ""Qcache_lowmem_prunes"" status variable to check if the current value
        //# is high enough for your load.
        //# Note: In case your tables change very often or if your queries are
        //# textually different every time, the query cache may result in a
        //# slowdown instead of a performance improvement.
        //query_cache_size=8M
        //
        //# The number of open tables for all threads. Increasing this value
        //# increases the number of file descriptors that mysqld requires.
        //# Therefore you have to make sure to set the amount of open files
        //# allowed to at least 4096 in the variable ""open-files-limit"" in
        //# section [mysqld_safe]
        //table_cache=256
        //
        //# Maximum size for internal (in-memory) temporary tables. If a table
        //# grows larger than this value, it is automatically converted to disk
        //# based table This limitation is for a single table. There can be many
        //# of them.
        //tmp_table_size=5M
        //
        //
        //# How many threads we should keep in a cache for reuse. When a client
        //# disconnects, the client's threads are put in the cache if there aren't
        //# more than thread_cache_size threads from before.  This greatly reduces
        //# the amount of thread creations needed if you have a lot of new
        //# connections. (Normally this doesn't give a notable performance
        //# improvement if you have a good thread implementation.)
        //thread_cache_size=8
        //
        //#*** MyISAM Specific options
        //
        //# The maximum size of the temporary file MySQL is allowed to use while
        //# recreating the index (during REPAIR, ALTER TABLE or LOAD DATA INFILE.
        //# If the file-size would be bigger than this, the index will be created
        //# through the key cache (which is slower).
        //myisam_max_sort_file_size=100G
        //
        //# If the temporary file used for fast index creation would be bigger
        //# than using the key cache by the amount specified here, then prefer the
        //# key cache method.  This is mainly used to force long character keys in
        //# large tables to use the slower key cache method to create the index.
        //myisam_sort_buffer_size=8M
        //
        //# Size of the Key Buffer, used to cache index blocks for MyISAM tables.
        //# Do not set it larger than 30% of your available memory, as some memory
        //# is also required by the OS to cache rows. Even if you're not using
        //# MyISAM tables, you should still set it to 8-64M as it will also be
        //# used for internal temporary disk tables.
        //key_buffer_size=8M
        //
        //# Size of the buffer used for doing full table scans of MyISAM tables.
        //# Allocated per thread, if a full scan is needed.
        //read_buffer_size=64K
        //read_rnd_buffer_size=185K
        //
        //# This buffer is allocated when MySQL needs to rebuild the index in
        //# REPAIR, OPTIMZE, ALTER table statements as well as in LOAD DATA INFILE
        //# into an empty table. It is allocated per thread so be careful with
        //# large settings.
        //sort_buffer_size=139K
        //
        //
        //#*** INNODB Specific options ***
        //
        //
        //# Use this option if you have a MySQL server with InnoDB support enabled
        //# but you do not plan to use it. This will save memory and disk space
        //# and speed up some things.
        //#skip-innodb
        //
        //# Additional memory pool that is used by InnoDB to store metadata
        //# information.  If InnoDB requires more memory for this purpose it will
        //# start to allocate it from the OS.  As this is fast enough on most
        //# recent operating systems, you normally do not need to change this
        //# value. SHOW INNODB STATUS will display the current amount used.
        //innodb_additional_mem_pool_size=2M
        //
        //# If set to 1, InnoDB will flush (fsync) the transaction logs to the
        //# disk at each commit, which offers full ACID behavior. If you are
        //# willing to compromise this safety, and you are running small
        //# transactions, you may set this to 0 or 2 to reduce disk I/O to the
        //# logs. Value 0 means that the log is only written to the log file and
        //# the log file flushed to disk approximately once per second. Value 2
        //# means the log is written to the log file at each commit, but the log
        //# file is only flushed to disk approximately once per second.
        //innodb_flush_log_at_trx_commit=1
        //
        //# The size of the buffer InnoDB uses for buffering log data. As soon as
        //# it is full, InnoDB will have to flush it to disk. As it is flushed
        //# once per second anyway, it does not make sense to have it very large
        //# (even with long transactions).
        //innodb_log_buffer_size=1M
        //
        //# InnoDB, unlike MyISAM, uses a buffer pool to cache both indexes and
        //# row data. The bigger you set this the less disk I/O is needed to
        //# access data in tables. On a dedicated database server you may set this
        //# parameter up to 80% of the machine physical memory size. Do not set it
        //# too large, though, because competition of the physical memory may
        //# cause paging in the operating system.  Note that on 32bit systems you
        //# might be limited to 2-3.5G of user level memory per process, so do not
        //# set it too high.
        //innodb_buffer_pool_size=8M
        //
        //# Size of each log file in a log group. You should set the combined size
        //# of log files to about 25%-100% of your buffer pool size to avoid
        //# unneeded buffer pool flush activity on log file overwrite. However,
        //# note that a larger logfile size will increase the time needed for the
        //# recovery process.
        //innodb_log_file_size=10M
        //
        //# Number of threads allowed inside the InnoDB kernel. The optimal value
        //# depends highly on the application, hardware as well as the OS
        //# scheduler properties. A too high value may lead to thread thrashing.
        //innodb_thread_concurrency=8
        //
        //# Each table should be in a separate file
        //innodb_file_per_table
        //
        //";

        private string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "Core", "DatabaseInstaller", resourceName, null, defaultValue, substitutes);
        }

    }
}
