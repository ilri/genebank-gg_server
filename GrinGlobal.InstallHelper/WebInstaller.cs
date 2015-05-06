using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;
using System.Security.AccessControl;
using GrinGlobal.Core;
using System.Net;

using GrinGlobal.InstallHelper;
using GrinGlobal.Core.DataManagers;

namespace GrinGlobal.Web.Installer {
    [RunInstaller(true)]
    public partial class WebInstaller : System.Configuration.Install.Installer {
        public WebInstaller() {

            // Project Installer actions can not be called on a website -- only on an executable or a dll.
            // This dll serves no purpose other than to provide an entry point for the MSI installer

            InitializeComponent();
        }

        //private void WebInstaller_BeforeInstall(object sender, InstallEventArgs e) {
        //    ProcessStartInfo psiASPNET = new ProcessStartInfo("aspnet_regiis.exe", " /i");
        //    try {
        //        psiASPNET.UseShellExecute = true;
        //        string windir = Environment.GetEnvironmentVariable("WINDIR");
        //        psiASPNET.WorkingDirectory = windir + @"\Microsoft.NET\Framework\v2.0.50727\";
        //        Process p2 = Process.Start(psiASPNET);
        //        p2.WaitForExit();
        //        //                stateSaver["ASPNET_REGIIS_EXIT_CODE"] = p2.ExitCode;
        //    } catch (Exception ex2) {
        //        Context.LogMessage("Exception registering asp.net with IIS: " + ex2.Message);
        //        throw;
        //    }

        //}

        private void WebInstaller_AfterInstall(object sender, InstallEventArgs e) {
        }

        public override void Install(IDictionary stateSaver){
         	base.Install(stateSaver);

            var splash = new frmSplash();
            try {
                string targetDir = Utility.GetTargetDirectory(Context, stateSaver, "Web Application");

                string gguacPath = Toolkit.ResolveFilePath(targetDir + @"\gguac.exe", false);

                //MessageBox.Show("config file=" + configFile);


                splash.ChangeText(getDisplayMember("Install{extracting}", "Extracting bundled files..."));

                var utility64CabPath = Toolkit.ResolveFilePath(targetDir + @"\utility64.cab", false);
                var utility64ExePath = Toolkit.ResolveFilePath(targetDir + @"\ggutil64.exe", false);

                var tempPath = Utility.GetTempDirectory(100);

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


                // update the config file...
                string configFile = Toolkit.ResolveFilePath(targetDir + @"\web.config", false);

                if (File.Exists(configFile)) {

                    splash.ChangeText(getDisplayMember("Install{verifying}", "Verifying database connection..."));
                    string superUserPassword = Utility.GetSuperUserPassword(Context, stateSaver);
                    bool? useWindowsAuth = Utility.GetUseWindowsAuthentication(Context, stateSaver);
                    DatabaseEngineUtil dbEngineUtil = promptForDatabaseConnectionInfo(splash, ref superUserPassword, ref useWindowsAuth);
                    createDatabaseUser(splash, dbEngineUtil, superUserPassword, "gg_user", "PA55w0rd!");

                    string connectionString = null;
                    if (useWindowsAuth == true) {
                        connectionString = dbEngineUtil.GetDataConnectionSpec("gringlobal", null, null).ConnectionString;
                    } else {
                        connectionString = dbEngineUtil.GetDataConnectionSpec("gringlobal", "gg_user", "PA55w0rd!").ConnectionString;
                    }
                    EventLog.WriteEntry("GRIN-Global Web Application", "Database connection string=" + connectionString, EventLogEntryType.Information);


                    splash.ChangeText(getDisplayMember("Install{writingconfig}", "Writing configuration file..."));

                    string contents = File.ReadAllText(configFile);

                    string appSetting = @"<add providerName=""__ENGINE__"" name=""DataManager"" connectionString=""__CONNECTION_STRING__"" />".Replace("__ENGINE__", dbEngineUtil.EngineName).Replace("__CONNECTION_STRING__", connectionString);

                    contents = contents.Replace("<!-- __CONNECTIONSTRING__ -->", appSetting);
                    contents = contents.Replace("<!-- __COMMENT__ -->", "<!-- TESTING ");
                    contents = contents.Replace("<!-- __ENDCOMMENT__ -->", " -->");

                    File.WriteAllText(configFile, contents);

                }


                // give ASPNET user full control to the ~/uploads folder
                splash.ChangeText(getDisplayMember("Install{settingperms}", "Setting permissions on uploads folder..."));
                assignFolderPermissions(Utility.ResolveDirectoryPath(targetDir + @"\uploads", true));

            } catch (Exception ex){
                MessageBox.Show(getDisplayMember("Install{failed}", "Error: {0}", ex.Message));
                throw;
            } finally {
                splash.Close();
            }
        }

        private void assignFolderPermissions(string folder) {
            // Give write access to uploads folder to IIS user
            //
            var security = Directory.GetAccessControl(folder);
            try {
                // ASPNET for XP (IIS 5)
                EventLog.WriteEntry("GRIN-Global Web Application", "Assigning user ASPNET rights to " + folder, EventLogEntryType.Information);
                var access = new FileSystemAccessRule("ASPNET", FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
                security.AddAccessRule(access);
                Directory.SetAccessControl(folder, security);
            } catch (Exception) {
            }

            try {
                // NETWORK SERVICE for Server / Vista (IIS 6/7)
                EventLog.WriteEntry("GRIN-Global Web Application", "Assigning user Network Service rights to " + folder, EventLogEntryType.Information);
                var access = new FileSystemAccessRule("Network Service", FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
                security.AddAccessRule(access);
                Directory.SetAccessControl(folder, security);
            } catch (Exception) {
            }

            try {
                // ApplicationPoolIdentity for Win 7 (IIS 7.5)
                EventLog.WriteEntry("GRIN-Global Web Application", "Assigning user ApplicationPoolIdentity rights to " + folder, EventLogEntryType.Information);
                var access = new FileSystemAccessRule("ApplicationPoolIdentity", FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
                security.AddAccessRule(access);
                Directory.SetAccessControl(folder, security);
            } catch (Exception) {
            }

            // Fixes #7 -- ~/gringlobal/uploads not getting proper rights to allow uploads to work
            try {
                // Alternate config for Server / Vista (IIS 6/7) / Windows 7 (IIS 7.5)
                EventLog.WriteEntry("GRIN-Global Web Application", "Assigning user IIS_IUSRS rights to " + folder, EventLogEntryType.Information);
                var access = new FileSystemAccessRule("IIS_IUSRS", FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
                security.AddAccessRule(access);
                Directory.SetAccessControl(folder, security);
            } catch (Exception) {
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
                var netBIOSName = Toolkit.Cut(dnsName, 0, 15);

                var output = "";
                if (dbEngineUtil is SqlServerEngineUtil) {

                    // SQL Server and using Windows Authentication...

                    // XP
                    output = dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "NETWORK SERVICE", netBIOSName, null, false, true);
                    EventLog.WriteEntry("GRIN-Global Web Application", "Created db user NETWORK SERVICE allowed from machine " + netBIOSName + "... Result: " + output, EventLogEntryType.Information);

                    output = dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "ASPNET", netBIOSName, null, false, true);
                    EventLog.WriteEntry("GRIN-Global Web Application", "Created db user ASPNET allowed from machine " + netBIOSName + "... Result: " + output, EventLogEntryType.Information);

                    // Vista
                    output = dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "NETWORK SERVICE", "NT AUTHORITY", null, false, true);
                    EventLog.WriteEntry("GRIN-Global Web Application", "Created db user NETWORK SERVICE allowed from machine NT AUTHORITY" + "... Result: " + output, EventLogEntryType.Information);

                    output = dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "SYSTEM", "NT AUTHORITY", null, false, true);
                    EventLog.WriteEntry("GRIN-Global Web Application", "Created db user SYSTEM allowed from machine NT AUTHORITY" + "... Result: " + output, EventLogEntryType.Information);

                    // Windows 7
                    output = dbEngineUtil.CreateUser(superUserPassword, "gringlobal", "DEFAULTAPPPOOL", "IIS AppPool", null, false, true);
                    EventLog.WriteEntry("GRIN-Global Web Application", "Created db user DEFAULTAPPPOOL allowed from machine " + netBIOSName + "... Result: " + output, EventLogEntryType.Information);

                    // SQL Server mixed mode...
                    output = dbEngineUtil.CreateUser(superUserPassword, "gringlobal", userName, netBIOSName, userPassword, false, false);
                    EventLog.WriteEntry("GRIN-Global Web Application", "Created db user " + userName + " allowed from machine " + netBIOSName + "... Result: " + output, EventLogEntryType.Information);

                } else if (dbEngineUtil is MySqlEngineUtil){
                    output = dbEngineUtil.CreateUser(superUserPassword, "gringlobal", userName, "%", userPassword, false, false);
                    EventLog.WriteEntry("GRIN-Global Web Application", "Created db user " + userName + " allowed from machine % ... Result: " + output, EventLogEntryType.Information);
                } else if (dbEngineUtil is OracleEngineUtil) {
                    output = dbEngineUtil.CreateUser(superUserPassword, "gringlobal", userName, null, userPassword, false, false);
                    EventLog.WriteEntry("GRIN-Global Web Application", "Created db user " + userName + " allowed from machine [null] ... Result: " + output, EventLogEntryType.Information);
                } else if (dbEngineUtil is PostgreSqlEngineUtil) {
                    output = dbEngineUtil.CreateUser(superUserPassword, "gringlobal", userName, dnsName, userPassword, false, false);
                    EventLog.WriteEntry("GRIN-Global Web Application", "Created db user " + userName + " allowed from machine " + dnsName + "... Result: " + output, EventLogEntryType.Information);
                }

            } catch (Exception exCreateUsers) {
                //Context.LogMessage("Exception creating users: " + exCreateUsers.Message);
                EventLog.WriteEntry("GRIN-Global Web Application", "Exception creating user(s): " + exCreateUsers.Message, EventLogEntryType.Error);
                //MessageBox.Show("could not create db user: " + exCreateUsers.Message);

            }
            
        }

        public override void Uninstall(IDictionary savedState) {

            var targetDir = Utility.GetTargetDirectory(Context, savedState, "Web Application");
            var utility64ExePath = Toolkit.ResolveFilePath(targetDir + @"\ggutil64.exe", false);
            try {
                if (File.Exists(utility64ExePath)) {
                    File.Delete(utility64ExePath);
                }
            } catch (Exception ex) {
                //Context.LogMessage("Exception deleting utility64: " + ex.Message);
                EventLog.WriteEntry("GRIN-Global Web Application", "Could not delete " + utility64ExePath + ": " + ex.Message, EventLogEntryType.Error);
            }

            base.Uninstall(savedState);

        }

        private void WebInstaller_BeforeInstall(object sender, InstallEventArgs e) {
            //MessageBox.Show("targetvdir=" + Utility.GetTargetVirtualDirectory(Context, e.SavedState, "GRIN-Global Web Application") + 
            //"\n\ntargetdir=" + Utility.GetTargetDirectory(Context, e.SavedState, "GRIN-Global Web Application"));

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "MiddleTier", "WebInstaller", resourceName, null, defaultValue, substitutes);
        }
    }
}
