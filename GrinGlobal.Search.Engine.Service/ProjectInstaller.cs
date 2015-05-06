using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Diagnostics;
using GrinGlobal.Core;
using Microsoft.Win32;
using System.IO;
using System.ServiceProcess;
using GrinGlobal.InstallHelper;
using System.Windows.Forms;
using GrinGlobal.Search.Engine.Hosting;

using System.Threading;
using GrinGlobal.Core.DataManagers;
using System.Net;

namespace GrinGlobal.Search.Engine.Service {
	[RunInstaller(true)]
	public partial class ProjectInstaller : Installer {
		public ProjectInstaller() {
			InitializeComponent();
		}

        private void writeFirewallException(string fullFilePath, string displayName, bool enabled) {
            string value = fullFilePath + ":*:" + (enabled ? "Enabled" : "Disabled") + ":" + displayName;
            Toolkit.SaveRegSetting(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\AuthorizedApplications\List", fullFilePath, value, true);
        }

        public override void Install(IDictionary stateSaver) {

//            MessageBox.Show("Is registered on domain? " + Utility.IsRegisteredOnDomain() + "\nCan reach DC? " + Utility.CanReachDomainController());

//            EventLog.WriteEntry("GRIN-Global", "Install()", EventLogEntryType.Information);

            stopService();
            customUninstall(stateSaver);

            base.Install(stateSaver);
            
            customInstall(stateSaver);
        }

		private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e) {
//            EventLog.WriteEntry("GRIN-Global", "serviceProcessInstaller AfterInstall", EventLogEntryType.Information);
        }

        private void customInstall(IDictionary state){


            string targetDir = Utility.GetTargetDirectory(this.Context, state, "Search Engine");

            string targetDataDir = Toolkit.ResolveDirectoryPath(@"*COMMONAPPLICATIONDATA*\GRIN-Global\GRIN-Global Search Engine", true);

            int installerWindowHandle = 0;

            var splash = new frmSplash();

            try {

                string gguacPath = Toolkit.ResolveFilePath(targetDir + @"\gguac.exe", false);

                splash.ChangeText(getDisplayMember("customInstall{extracting}", "Extracting bundled files..."));

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
                        // extract it from our cab
                        var cabOutput = Utility.ExtractCabFile(utility64CabPath, tempPath, gguacPath);
                        // move it to the final target path (we can't do this up front because expand.exe tells us "can't expand cab file over itself" for some reason.
                        if (File.Exists(extracted)) {
                            File.Move(extracted, utility64ExePath);
                        }
                    }
                }


                // delete any existing index file(s)
                string indexDir = (targetDataDir + @"\indexes").Replace(@"\\", @"\");
                if (Directory.Exists(targetDataDir)) {
                    Directory.Delete(targetDataDir, true);
                }

                string helperPath = (Utility.GetTargetDirectory(this.Context, state, "Search Engine") + @"\gguac.exe").Replace(@"\\", @"\");


                // prompt user to tell us what to do -- download files or begin recreating locally...
                var f = new frmInstallIndexes();
                f.HelperPath = helperPath;

                installerWindowHandle = Toolkit.GetWindowHandle("GRIN-Global Search Engine");
                //// HACK: try to give MSI form a moment to focus before we show our dialog...
                //Thread.Sleep(500);
                //Application.DoEvents();
                //Thread.Sleep(500);

                // we no longer allow user to download indexes. always issue a recreate.
                File.WriteAllText(Toolkit.ResolveFilePath(indexDir + @"\recreate.init", true), "");


                //DialogResult result = DialogResult.Cancel;
                //var autoIncludeOptionalData = Utility.GetParameter("optionaldata", null, this.Context, null);
                //if (("" + autoIncludeOptionalData).ToUpper() == "TRUE" || ("" + autoIncludeOptionalData).ToUpper() == "1") {
                //    f.DownloadAllFiles();
                //    result = DialogResult.OK;
                //} else if (("" + autoIncludeOptionalData).ToUpper() == "FALSE" || ("" + autoIncludeOptionalData).ToUpper() == "0"){
                //    result = DialogResult.OK;
                //} else {
                //    result = f.ShowDialog(installerWindowHandle);
                //}
                
                //if (result == DialogResult.OK) {

                //    if (f.rdoCreateLocally.Checked) {
                //        // need to create locally
                //        File.WriteAllText(Toolkit.ResolveFilePath(indexDir + @"\recreate.init", true), "");
                //        // EventLog.WriteEntry("GRIN-Global Search Engine", "Should recreate indexes locally...", EventLogEntryType.Information);
                //    } else {
                //        if (f.IndexFiles != null && f.IndexFiles.Count > 0) {
                //            // downloaded files.
                //            splash.Show("Inspecting index files (" + f.IndexFiles.Count + ")...", false, null);
                //            for (var i = 0; i < f.IndexFiles.Count; i++) {
                //                var s = f.IndexFiles[i];
                //                splash.ChangeText("Extracting index files (" + (i + 1) + " of " + f.IndexFiles.Count + ")...");
                //                Utility.ExtractCabFile(s, indexDir, helperPath);
                //                // we expanded the cab file, now delete it since we don't need it anymore (and want the next install to re-request data from the server and ignore the cache)
                //                try {
                //                    var moveTo = s.Replace(@"\downloaded\", @"\installed\");
                //                    if (File.Exists(moveTo)) {
                //                        File.Delete(moveTo);
                //                    }
                //                    File.Move(s, moveTo);
                //                } catch {
                //                    try {
                //                        // move failed, try to delete it
                //                        File.Delete(s);
                //                    } catch {
                //                        // ultimately ignore all file movement errors
                //                    }
                //                }
                //            }
                //        } else {
                //            EventLog.WriteEntry("GRIN-Global Search Engine", "User chose to download indexes, but selected 0 indexes to download.  Empty indexes will be created.", EventLogEntryType.Information);
                //        }
                //    }

                //} else {
                //    // user cancelled out.
                //    //Context.LogMessage("User cancelled out of choosing to generate indexes locally or download from server");
                //    EventLog.WriteEntry("GRIN-Global Search Engine", "User cancelled out of choosing to generate indexes locally or download from server");
                //    //                    this.Rollback(state);
                //    throw new InvalidOperationException("User cancelled out of choosing to generate indexes locally or download from server.");
                //}

            } catch (Exception ex) {
                splash.Close();
                //this.Context.LogMessage("Setup failed to extract the index files: " + ex.Message);
                EventLog.WriteEntry("GRIN-Global Search Engine", "Setup failed to extract the index files: " + ex.Message);
//                this.Rollback(state);
                throw;
            }



//                // first unzip the data (note we need to run this is athe currently logged-in user for it to work)
//                Utility.ExtractCabFile((targetDir + @"\search.cab").Replace(@"\\", @"\"), indexDir, helperPath);
////                Utility.Unzip((Utility.DataDirectory + @"\gringlobal_search_small.zip").Replace(@"\\", @"\"), indexDir);




            string configFile = Toolkit.ResolveFilePath(targetDir + @"\GrinGlobal.Search.Engine.Service.exe.config", false);
            try {

                // update the config file...
                if (File.Exists(configFile)) {

                    splash.ChangeText(getDisplayMember("customInstall{verifying}", "Verifying database connection..."));
                    string superUserPassword = Utility.GetSuperUserPassword(Context, state);
                    bool? useWindowsAuthentication = Utility.GetUseWindowsAuthentication(Context, state);
                    //MessageBox.Show("pw=" + superUserPassword + ", windowsAuth=" + useWindowsAuthentication);
                    DatabaseEngineUtil dbEngineUtil = promptForDatabaseConnectionInfo(splash, ref superUserPassword, ref useWindowsAuthentication);
                    createDatabaseUser(splash, dbEngineUtil, superUserPassword, "gg_search", "gg_search_PA55w0rd!");

                    string connectionString = null;
                    if (useWindowsAuthentication == true) {
                        connectionString = dbEngineUtil.GetDataConnectionSpec("gringlobal", null, null).ConnectionString;
                    } else {
                        connectionString = dbEngineUtil.GetDataConnectionSpec("gringlobal", "gg_search", "gg_search_PA55w0rd!").ConnectionString;
                    }
                    EventLog.WriteEntry("GRIN-Global Search Engine", "Database connection string=" + connectionString, EventLogEntryType.Information);


                    splash.ChangeText(getDisplayMember("customInstall{writingconfig}", "Writing configuration file..."));

                    string contents = File.ReadAllText(configFile);

                    string appSetting = @"<add providerName=""__ENGINE__"" name=""DataManager"" connectionString=""__CONNECTION_STRING__"" />".Replace("__ENGINE__", dbEngineUtil.EngineName).Replace("__CONNECTION_STRING__", connectionString);

                    contents = contents.Replace("<!-- __CONNECTIONSTRING__ -->", appSetting);
                    contents = contents.Replace("<!-- __COMMENT__ -->", "<!-- TESTING ");
                    contents = contents.Replace("<!-- __ENDCOMMENT__ -->", " -->");

                    File.WriteAllText(configFile, contents);

                }


            } catch (Exception ex) {
                splash.Close();
                //this.Context.LogMessage("Setup failed to update the configuration file (" + configFile + "): " + ex.Message);
                EventLog.WriteEntry("GRIN-Global Search Engine", "Setup failed to update the configuration file (" + configFile + "): " + ex.Message);
//                this.Rollback(state);
                throw;
            } finally {
                splash.Close();
                if (installerWindowHandle > 0) {
                    Toolkit.RestoreWindow(installerWindowHandle);
                }
            }


		}

        private DatabaseEngineUtil promptForDatabaseConnectionInfo(frmSplash splash, ref string superUserPassword, ref bool? useWindowsAuthentication) {
            var f = new frmDatabaseLoginPrompt();
            if (DialogResult.OK == f.ShowDialog(splash, null, true, true, false, superUserPassword, useWindowsAuthentication, true, false)) {
                superUserPassword = f.txtPassword.Text;
                useWindowsAuthentication = f.UseWindowsAuthentication;
                return f.DatabaseEngineUtil;
            } else {
                throw new InvalidOperationException(getDisplayMember("promptForDatabaseConnectionInfo", "User cancelled out of database connection dialog"));
            }
        }


        private void createDatabaseUser(frmSplash splash, DatabaseEngineUtil dbEngineUtil, string superUserPassword, string userName, string userPassword) {

            try {

                splash.ChangeText(getDisplayMember("createDatabaseUser{start}", "Creating database users as needed..."));


                var dnsName = Dns.GetHostName();

                if (dbEngineUtil is SqlServerEngineUtil) {

                    var netBIOSName = Toolkit.Cut(dnsName, 0, 15);

                    // SQL Server, Integrated authority -- XP
                    dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "NETWORK SERVICE", netBIOSName, null, false, true);
                    dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "ASPNET", netBIOSName, null, false, true);

                    // SQL Server, Integrated authority -- Vista
                    dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "NETWORK SERVICE", "NT AUTHORITY", null, false, true);
                    dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "SYSTEM", "NT AUTHORITY", null, false, true);

                    // SQL Server, Integrated authority -- Windows 7
                    dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "DEFAULTAPPPOOL", "IIS AppPool", null, false, true);

                    // SQL Server mixed mode
                    dbEngineUtil.CreateUser(superUserPassword, "gringlobal", userName, netBIOSName, userPassword, false, false);
                
                } else if (dbEngineUtil is MySqlEngineUtil){
                    dbEngineUtil.CreateUser(superUserPassword, "gringlobal", userName, "*", userPassword, false, false);
                } else if (dbEngineUtil is OracleEngineUtil) {
                    dbEngineUtil.CreateUser(superUserPassword, "gringlobal", userName, null, userPassword, false, false);
                } else if (dbEngineUtil is PostgreSqlEngineUtil) {
                    dbEngineUtil.CreateUser(superUserPassword, "gringlobal", userName, dnsName, userPassword, false, false); 
                }


            } catch (Exception exCreateUsers) {
                //Context.LogMessage("Exception creating users: " + exCreateUsers.Message);
                //MessageBox.Show("could not create db user: " + exCreateUsers.Message);
                EventLog.WriteEntry("GRIN-Global Search Engine", "Exception creating users: " + exCreateUsers.Message);

            }

        }



		private void serviceProcessInstaller1_Committed(object sender, InstallEventArgs e) {
//            EventLog.WriteEntry("GRIN-Global", "serviceProcessInstaller Committed", EventLogEntryType.Information);
        }

        private bool IsUpgrade {
            get {
                return !string.IsNullOrEmpty(this.Context.Parameters["OldProductCode"]);
            }
        }

        private void customCommit(IDictionary state){

            startService();

            // Since we're running outside the context of the service, we can't easily use the config settings for doing WCF communication with it.
            // So, we're using the recreate.init file as a flag (see customInstall method) to the service it should auto-create enabled indexes when it first starts up.
            // That means we really have nothing to do here.

		}

        private void serviceProcessInstaller1_BeforeUninstall(object sender, InstallEventArgs e) {
//            EventLog.WriteEntry("GRIN-Global", "serviceProcessInstaller BeforeUninstall", EventLogEntryType.Information);
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e) {
//            EventLog.WriteEntry("GRIN-Global", "serviceInstaller AfterInstall", EventLogEntryType.Information);
        }

        private void serviceInstaller1_BeforeUninstall(object sender, InstallEventArgs e) {
//            EventLog.WriteEntry("GRIN-Global", "serviceInstaller BeforeUninstall", EventLogEntryType.Information);
        }

        private void deleteProgramFiles(IDictionary state) {
            //MessageBox.Show("manually deleting program files");
            string targetDir = Utility.GetTargetDirectory(this.Context, state, "Search Engine");

            try {

                // then delete the data files
                if (Directory.Exists(targetDir)) {
                    foreach (string s in Directory.GetFiles(targetDir)) {
                        var lower = s.ToLower();
                        if (lower.EndsWith(".exe") || lower.EndsWith(".dll") || lower.EndsWith(".config")){
                            File.Delete(s);
                        }
                    }
                }

            } catch (Exception ex) {
                //Context.LogMessage("Setup failed to delete the index or logs folder (" + targetDir + "): " + ex.Message);
                EventLog.WriteEntry("GRIN-Global Search Engine", "Setup failed to delete the index or logs folder (" + targetDir + "): " + ex.Message);
            }
        }

        private void deleteIndexesAndLogs(IDictionary state){
            //            writeFirewallException(Context.Parameters["TARGETDIR"].ToString() + "GrinGlobal.Search.Service.exe", "GRIN-Global Search Engine", false);

//            MessageBox.Show("Stopping search engine...");
            // MessageBox.Show("manually deleting indexes and logs");

            string targetDir = Utility.GetTargetDirectory(this.Context, state, "Search Engine");
            string targetDataDir = Toolkit.ResolveDirectoryPath(@"*COMMONAPPLICATIONDATA*\GRIN-Global\GRIN-Global Search Engine\", true);

            try {

                // then delete the data files
                if (Directory.Exists((targetDataDir + @"\indexes").Replace(@"\\", @"\"))) {
                    Directory.Delete((targetDataDir + @"\indexes").Replace(@"\\", @"\"), true);
                }

                // then delete the data files
                if (Directory.Exists((targetDataDir + @"\logs").Replace(@"\\", @"\"))) {
                    Directory.Delete((targetDataDir + @"\logs").Replace(@"\\", @"\"), true);
                }
            
            } catch (Exception ex) {
                //Context.LogMessage("Setup failed to delete the index or logs folder (" + (targetDataDir + @"\indexes").Replace(@"\\", @"\") + ", " + (targetDataDir + @"\logs").Replace(@"\\", @"\") + "): " + ex.Message);
                EventLog.WriteEntry("GRIN-Global Search Engine", "Setup failed to delete the index or logs folder (" + (targetDataDir + @"\indexes").Replace(@"\\", @"\") + ", " + (targetDataDir + @"\logs").Replace(@"\\", @"\") + "): " + ex.Message);
            }
        }

        private void startService() {
            try {
                //                MessageBox.Show("starting ggse service");
                // start the service (if we're uninstalling this will fail, who cares!)
                ServiceController sc = new ServiceController(serviceInstaller1.ServiceName);
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
            } catch (Exception ex) {
                //Context.LogMessage("Setup failed to start the GRIN-Global Search Engine (service=" + serviceInstaller1.ServiceName + "): " + ex.Message);
                //   EventLog.WriteEntry("GRIN-Global Search Engine", "Setup failed to start the GRIN-Global Search Engine: (service=" + serviceInstaller1.ServiceName + "): " + ex.Message);
            }
        }


        private void stopService() {
            try {
                // MessageBox.Show("stopping serviceInstaller1.ServiceName service");
                // first stop the service
                ServiceController sc = new ServiceController(serviceInstaller1.ServiceName);
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
            } catch (Exception ex) {
                // Context.LogMessage("Setup failed to stop the GRIN-Global Search Engine (service=" + serviceInstaller1.ServiceName + "): " + ex.Message);
            //    EventLog.WriteEntry("GRIN-Global Search Engine", "Setup failed to stop the GRIN-Global Search Engine (service=" + serviceInstaller1.ServiceName + "): " + ex.Message);
            }
        }

        private void serviceProcessInstaller1_BeforeInstall(object sender, InstallEventArgs e) {
//            EventLog.WriteEntry("GRIN-Global", "serviceProcessInstaller BeforeInstall", EventLogEntryType.Information);
        }

        private void serviceInstaller1_BeforeInstall(object sender, InstallEventArgs e) {
//            EventLog.WriteEntry("GRIN-Global", "serviceInstaller BeforeInstall", EventLogEntryType.Information);
        }

//        private void serviceInstaller1_AfterRollback(object sender, InstallEventArgs e) {
////            EventLog.WriteEntry("GRIN-Global", "serviceInstaller AfterRollback", EventLogEntryType.Information);

//        }

        private void serviceInstaller1_AfterUninstall(object sender, InstallEventArgs e) {
//            EventLog.WriteEntry("GRIN-Global", "serviceInstaller AfterUninstall", EventLogEntryType.Information);

        }

//        private void serviceInstaller1_BeforeRollback(object sender, InstallEventArgs e) {
////            EventLog.WriteEntry("GRIN-Global", "serviceInstaller BeforeRollback", EventLogEntryType.Information);

//        }

        private void serviceInstaller1_Committed(object sender, InstallEventArgs e) {
//            EventLog.WriteEntry("GRIN-Global", "serviceInstaller Committed", EventLogEntryType.Information);
            customCommit(e.SavedState);

        }

        private void serviceInstaller1_Committing(object sender, InstallEventArgs e) {
//            EventLog.WriteEntry("GRIN-Global", "serviceInstaller Committing", EventLogEntryType.Information);
        }

        public override void Commit(IDictionary savedState) {
//            EventLog.WriteEntry("GRIN-Global", "Commit()", EventLogEntryType.Information);
            base.Commit(savedState);
        }

//        public override void Rollback(IDictionary savedState) {
////            EventLog.WriteEntry("GRIN-Global", "Rollback()", EventLogEntryType.Information);
//            base.Rollback(savedState);
//        }

        public override void Uninstall(IDictionary savedState) {
            stopService();
            customUninstall(savedState);
            base.Uninstall(savedState);
        }

        private void customUninstall(IDictionary savedState){
//            EventLog.WriteEntry("GRIN-Global", "customUninstall()", EventLogEntryType.Information);
            deleteIndexesAndLogs(savedState);

            //if (IsUpgrade) {
            //    try {
            //        var psi = new ProcessStartInfo("sc.exe", " delete ggse");
            //        var p = Process.Start(psi);
            //        p.WaitForExit(30000);
            //    } catch (Exception ex) {
            //        MessageBox.Show("Error deleting service: " + ex.Message);
            //    }
            //}

        }

//        private void serviceProcessInstaller1_AfterRollback(object sender, InstallEventArgs e) {
////            EventLog.WriteEntry("GRIN-Global", "serviceProcessInstaller AfterRollback", EventLogEntryType.Information);
//        }

        private void serviceProcessInstaller1_AfterUninstall(object sender, InstallEventArgs e) {
//            EventLog.WriteEntry("GRIN-Global", "serviceProcessInstaller AfterUninstall", EventLogEntryType.Information);

        }

//        private void serviceProcessInstaller1_BeforeRollback(object sender, InstallEventArgs e) {
////            EventLog.WriteEntry("GRIN-Global", "serviceProcessInstaller BeforeRollback", EventLogEntryType.Information);

//        }

        private void serviceProcessInstaller1_Committing(object sender, InstallEventArgs e) {
//            EventLog.WriteEntry("GRIN-Global", "serviceProcessInstaller Committing", EventLogEntryType.Information);

        }


        /* Fresh Install event order (default VS 2008 ordering)...
         * ====================================================
         * Install()
         * serviceProcessInstaller_BeforeInstall()
         * serviceProcessInstaller_AfterInstall()
         * serviceInstaller_BeforeInstall()
         * serviceInstaller_AfterInstall()
         * Commit()
         * serviceProcessInstaller_Committing()
         * serviceProcessInstaller_Committed()
         * serviceInstaller_Committing()
         * serviceInstaller_Committed()
         * 
         * Uninstall event order (default VS 2008 ordering)...
         * ====================================================
         * Uninstall()
         * serviceInstaller_BeforeUninstall()
         * serviceInstaller_AfterUninstall()
         * serviceProcessInstaller_BeforeUninstall()
         * serviceProcessInstaller_AfterUninstall()
         * 
         * Upgrade event order (default VS 2008 ordering)...
         * ====================================================
         * Install()
         * serviceProcessInstaller_BeforeInstall()
         * serviceProcessInstaller_AfterInstall()
         * serviceInstaller_BeforeInstall()
         * serviceInstaller_AfterInstall()
         * Commit()
         * serviceProcessInstaller_Committing()
         * serviceProcessInstaller_Committed()
         * serviceInstaller_Committing()
         * serviceInstaller_Committed()
         * 
         * 
         * 
         * 
         * 
         * Rollback event order
         * ====================================================
         * Rollback()
         * serviceInstaller_BeforeRollback()
         * serviceInstaller_AfterRollback()
         * serviceProcessInstaller_BeforeRollback()
         * serviceProcessInstaller_AfterRollback()
         * 
         */

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "ProjectInstaller", resourceName, null, defaultValue, substitutes);
        }

    }
}
