using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using GrinGlobal.Core;
using System.Net;
using System.Text;


namespace GrinGlobal.Web {
    [RunInstaller(true)]
    public partial class WebInstaller : Installer {
        public WebInstaller() {
            InitializeComponent();
        }

        private string getTargetDir() {
            string ret = Context.Parameters["tgt"];
            if (String.IsNullOrEmpty(ret)) {
                ret = Context.Parameters["TARGETDIR"];
            }
            return ret;
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
            try {
                string targetDir = getTargetDir();


                
                // update the config file...
                string configFile = targetDir + "web.config";
                if (File.Exists(configFile)) {

                    string contents = File.ReadAllText(configFile);

                    // look up engine from registry (assumes database has been installed already)
                    string engine = "" + Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\GRIN-Global", "DatabaseEngine", "") as string;
                    string connectionString = "" + Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\GRIN-Global", "DatabaseConnectionString", "") as string;
                    string finalConnectionString = connectionString.Replace("__USERID__", "gg_user").Replace("__PASSWORD__", "gg_user_passw0rd!");
                    string appSetting = String.Format(@"<add providerName=""{0}"" name=""DataManager"" connectionString=""{1}"" />", engine, finalConnectionString);

                    contents = contents.Replace("<!-- __CONNECTIONSTRING__ -->", appSetting);
                    contents = contents.Replace("<!-- __COMMENT__ -->", "<!-- TESTING ");
                    contents = contents.Replace("<!-- __ENDCOMMENT__ -->", " -->");

                    File.WriteAllText(configFile, contents);


                    try {
                        // need to make the ~/uploads folder writable (for images
                        // create a batch file to do that for us (thanks to cacls requiring a 'Y' to confirm)
                        string tempFile = Path.GetTempFileName() + ".bat";

                        // give ASPNET user full control to the ~/uploads folder
                        string uploadsDir = @"""" + Toolkit.ResolveDirectoryPath(targetDir + @"\uploads", true) + @"""";
                        // vista needs "IUSR", XP needs "ASPNET"
                        string userName = (Toolkit.IsWindowsVistaOrLater ? "IUSR" : "ASPNET");

                        File.WriteAllText(tempFile, String.Format("echo y| cacls.exe {0} /T /C /G {1}:F", uploadsDir, userName));

                        string output = Toolkit.ExecuteProcess(tempFile, null, null);
                    } catch { 
                        // eat problems with this for now, have to release (2009-07-23)
                    }

                    try {

                        var engineName = Utility.GetDatabaseEngine(Context, e.SavedState, false);
                        var instanceName = Utility.GetDatabaseInstanceName(Context, e.SavedState);
                        var dbEngineUtil = DatabaseEngineUtil.CreateInstance(engineName, null, instanceName);
                        string file = targetDir + engine + @"_create_users.sql";
                        string replaced = File.ReadAllText(file).Replace("__MACHINENAME__", Dns.GetHostName()).Replace("__WWWUSER__", Toolkit.IsWindowsVistaOrLater ? "NETWORKSERVICE" : "ASPNET");
                        File.WriteAllText(file, replaced);

                        dbEngineUtil.ExecuteSqlFile(null, true, file);
                        //                    MessageBox.Show("creating users result - " + output);
                    } catch (Exception exCreateUsers) {
                        Context.LogMessage("Exception creating users: " + exCreateUsers.Message);
                    }

                }
            } finally {
            }
        }


        private static string GetDatabaseEngine(InstallContext ctx, IDictionary state, bool throwExceptionIfNotFound) {
            string ret = null;

            StringBuilder sb = new StringBuilder();

            if (ctx != null) {

                if (ctx.Parameters["action"] == "uninstall") {
                    // default to using registry entries
                    if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                        ret = Registry.GetValue(Utility.RootRegistryKey + @"\Database", "DatabaseEngine", "") as string;
                        sb.AppendLine("Registry.GetValue(" + Utility.RootRegistryKey + @"\Database\DatabaseEngine)=" + ret);
                    }
                    if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                        ret = Registry.GetValue(Utility.RootRegistryKey, "DatabaseEngine", "") as string;
                        sb.AppendLine("Registry.GetValue(" + Utility.RootRegistryKey + "DatabaseEngine)=" + ret);
                    }
                }


                if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                    ret = ctx.Parameters["engine"];
                    sb.AppendLine("\r\nContext.Parameters['engine']=" + ret);
                    if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                        ret = ctx.Parameters["ENGINE"];
                        sb.AppendLine("Context.Parameters['ENGINE']=" + ret);
                    }
                }
            }

            if (!DatabaseEngineUtil.IsValidEngine(ret) && state != null){
                ret = state["ENGINE"] as string;
                sb.AppendLine("state['ENGINE']=" + ret);
            }

            if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                ret = Registry.GetValue(Utility.RootRegistryKey, "DatabaseEngine", "") as string;
                sb.AppendLine("Registry.GetValue(" + Utility.RootRegistryKey + "DatabaseEngine)=" + ret);
            }

            if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                ret = Registry.GetValue(Utility.RootRegistryKey + @"\Database", "DatabaseEngine", "") as string;
                sb.AppendLine("Registry.GetValue(" + Utility.RootRegistryKey + @"\Database\DatabaseEngine)=" + ret);
            }

            // HACK: always make it sqlserver until we have other options available...
            if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                ret = "sqlserver";
            }

            if (!DatabaseEngineUtil.IsValidEngine(ret)){
                if (throwExceptionIfNotFound) {
                    throw new InvalidOperationException(@"GetDatabaseEngine could not resolve a valid engine name.  Inspected locations/values:" + sb.ToString());
                }
            } else {
                if (ctx != null) {
                    ctx.Parameters["engine"] = ret;
                }
            }

            return ret;

        }

    }
}
