using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Configuration.Install;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using GrinGlobal.Core;
using GrinGlobal.InstallHelper;

namespace GrinGlobal.DatabaseCopier {
    class DbUtility {

        ///// <summary>
        ///// Returns the value:   HKEY_LOCAL_MACHINE\SOFTWARE\GRIN-Global
        ///// </summary>
        //public static readonly string RootRegistryKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\GRIN-Global";

        //public static bool IsWindowsXP {
        //    get {
        //        return Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1;
        //    }
        //}

        //public static bool IsWindowsVista {
        //    get {
        //        return Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 0;
        //    }
        //}

        //public static bool IsWindowsVistaOrBetter {
        //    get {
        //        return Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 0;
        //    }
        //}

        //public static bool IsWindows7 {
        //    get {
        //        return Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1;
        //    }
        //}

        //public static bool IsWindowsVistaHome {
        //    get {
        //        return WindowsVistaEdition.ToLower() == "home premium";
        //    }
        //}

        //public static bool IsWindowsVistaUltimate {
        //    get {
        //        return WindowsVistaEdition.ToLower() == "ultimate";
        //    }
        //}

        //public static bool IsWindowsVistaBusiness {
        //    get {
        //        return WindowsVistaEdition.ToLower() == "business";
        //    }
        //}


        //public static string WindowsVistaEdition {
        //    get {
        //        return Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", "") as string;
        //    }
        //}

        //public static bool IsPowershellInstalled {
        //    get {
        //        string val = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1", "PID", "") as string;
        //        return !String.IsNullOrEmpty(val);
        //    }
        //}



        //public static string GetSystem32Directory() {
        //    return Environment.GetEnvironmentVariable("SystemRoot") + @"\system32\";
        //}

        //public static bool NeedToCreatePrecache(InstallContext ctx, IDictionary state, string hostName) {

        //    string ret = null;

        //    // pull from context if possible
        //    if (ctx != null) {
        //        ret = ctx.Parameters["precache" + hostName.ToLower()];
        //        if (String.IsNullOrEmpty(ret)) {
        //            ret = ctx.Parameters["PRECACHE" + hostName.ToUpper()];
        //        }
        //    }

        //    if (state != null && String.IsNullOrEmpty(ret)) {
        //        ret = state["PRECACHE" + hostName.ToUpper()] as string;
        //    }

        //    if (ctx != null) {
        //        ctx.Parameters["precache" + hostName.ToLower()] = ret;
        //    }

        //    return ("" + ret) == "1";
        //}



        //public static string GetTargetDirectory(InstallContext ctx, IDictionary state, string appName) {

        //    string ret = null;

        //    // pull from context if possible
        //    if (ctx != null) {
        //        ret = ctx.Parameters["tgt"];
        //        if (String.IsNullOrEmpty(ret)) {
        //            ret = ctx.Parameters["TARGETDIR"];
        //        }
        //    }

        //    if (state != null && String.IsNullOrEmpty(ret)) {
        //        ret = state["TARGETDIR"] as string;
        //    }

        //    if (String.IsNullOrEmpty(ret)) {
        //        // look to the registry if we can't glean it from the context
        //        ret = Registry.GetValue(RootRegistryKey + @"\" + appName, "InstallPath", "") as string;
        //        if (String.IsNullOrEmpty(ret)) {
        //            // and finally the current directory as a last-ditch effort
        //            ret = Environment.CurrentDirectory;
        //        }
        //    }

        //    if (ctx != null) {
        //        ctx.Parameters["tgt"] = ret;
        //    }

        //    return ret;
        //}


        //public static string GetSourceDirectory(InstallContext ctx, IDictionary state) {

        //    string ret = null;

        //    if (ctx != null) {
        //        ret = ctx.Parameters["src"];
        //        if (String.IsNullOrEmpty(ret)) {
        //            ret = ctx.Parameters["SOURCEDIR"];
        //        }
        //    }


        //    if (String.IsNullOrEmpty(ret) && state != null) {
        //        ret = state["SOURCEDIR"] as string;
        //    }

        //    if (String.IsNullOrEmpty(ret)) {
        //        ret = Registry.GetValue(RootRegistryKey, "SourceDir", "") as string;
        //    }

        //    if (ctx != null) {
        //        ctx.Parameters["src"] = ret;
        //    }


        //    return ret;
        //}

        //public static string GetGrinGlobalApplicationInstallPath(string appName, string defaultIfEmpty) {
        //    string ret = Registry.GetValue(RootRegistryKey + @"\" + appName, "InstallPath", "") as string;
        //    return String.IsNullOrEmpty(ret) ? defaultIfEmpty : ret;
        //}

        //public static string GetTempDirectory() {

        //    // SQL Server installer does not play well with spaces in the path -- even if the path is in 8.3 format.
        //    // In XP the temp path may be part of the current user's Application Data path (C:\Documents and Settings\<username>\Application Data)
        //    // so we can't use the TEMP or TMP environment variables.
        //    // so we're punting and using a hardcoded path here because we have no other choice.
        //    // this will work in Vista simply because the installer (and all msi's) run with elevated privileges.
        //    return @"C:\temp\gginst\";
        //}

        //public static string GetBinDirectory(InstallContext ctx, IDictionary state) {
        //    // order of precedence:
        //    // [currentpath]\installers
        //    // [sourcedirectory]\bin

        //    StringBuilder sb = new StringBuilder();

        //    // determine the source and target dirs
        //    string binDir = (Environment.CurrentDirectory + @"\installers").Replace(@"\\", @"\");
        //    if (!Directory.Exists(binDir) || new DirectoryInfo(binDir).GetFiles().Length == 0) {
        //        sb.AppendLine(binDir);
        //        binDir = (Utility.GetSourceDirectory(ctx, state) + @"\bin").Replace(@"\\", @"\");
        //        if (!Directory.Exists(binDir) || new DirectoryInfo(binDir).GetFiles().Length == 0) {
        //            sb.AppendLine(binDir);
        //            throw new InvalidOperationException("BinDirectory not found.  Checked the following: " + sb.ToString());
        //        }
        //    }

        //    return binDir;
        //}

        //public static bool GetDatabaseWindowsAuth(InstallContext ctx, IDictionary state) {
        //    string ret = null;

        //    if (ctx != null) {
        //        if (ctx.Parameters["action"] == "uninstall") {
        //            // default to using registry entries first
        //            ret = Registry.GetValue(RootRegistryKey + @"\Database", "DatabaseWindowsAuth", "") as string;
        //            if (String.IsNullOrEmpty(ret)) {
        //                ret = Registry.GetValue(RootRegistryKey, "DatabaseWindowsAuth", "") as string;
        //            }
        //        } else {
        //            // default to using context first
        //            ret = ctx.Parameters["WINDOWSAUTH"];
        //        }
        //    }

        //    if (String.IsNullOrEmpty(ret) && state != null) {
        //        ret = state["WINDOWSAUTH"] as string;
        //    }

        //    if (String.IsNullOrEmpty(ret)) {
        //        ret = Registry.GetValue(RootRegistryKey + @"\Database", "DatabaseWindowsAuth", "") as string;
        //        if (String.IsNullOrEmpty(ret)) {
        //            ret = Registry.GetValue(RootRegistryKey, "DatabaseWindowsAuth", "") as string;
        //        }
        //    }

        //    return ret == "1";

        //}

        public static string GetDatabaseEngine(InstallContext ctx, IDictionary state, bool throwExceptionIfNotFound) {
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



//        public static string GetDatabaseInstanceName(InstallContext ctx, IDictionary state) {
//            string ret = null;

//            StringBuilder sb = new StringBuilder();

//            if (ctx != null) {

//                if (ctx.Parameters["action"] == "uninstall") {
//                    // default to using registry entries
//                    if (String.IsNullOrEmpty(ret)) {
//                        ret = Registry.GetValue(RootRegistryKey + @"\Database", "DatabaseInstanceName", "") as string;
//                        sb.AppendLine("Registry.GetValue(" + RootRegistryKey + @"\Database\DatabaseInstanceName)=" + ret);
//                    }
//                    if (String.IsNullOrEmpty(ret)) {
//                        ret = Registry.GetValue(RootRegistryKey, "DatabaseInstanceName", "") as string;
//                        sb.AppendLine("Registry.GetValue(" + RootRegistryKey + "DatabaseInstanceName)=" + ret);
//                    }
//                }


//                if (String.IsNullOrEmpty(ret)) {
//                    ret = ctx.Parameters["INSTANCENAME"];
//                    sb.AppendLine("\r\nContext.Parameters['INSTANCENAME']=" + ret);
//                    if (String.IsNullOrEmpty(ret)) {
//                        ret = ctx.Parameters["INSTANCENAME"];
//                        sb.AppendLine("Context.Parameters['INSTANCENAME']=" + ret);
//                    }
//                }
//            }

//            if (String.IsNullOrEmpty(ret) && state != null) {
//                ret = state["INSTANCENAME"] as string;
//                sb.AppendLine("state['INSTANCENAME']=" + ret);
//            }

//            if (String.IsNullOrEmpty(ret)) {
//                ret = Registry.GetValue(RootRegistryKey, "DatabaseInstanceName", "") as string;
//                sb.AppendLine("Registry.GetValue(" + RootRegistryKey + "DatabaseInstanceName)=" + ret);
//            }

//            if (String.IsNullOrEmpty(ret)) {
//                ret = Registry.GetValue(RootRegistryKey + @"\Database", "DatabaseInstanceName", "") as string;
//                sb.AppendLine("Registry.GetValue(" + RootRegistryKey + @"\Database\DatabaseInstanceName)=" + ret);
//            }

//            if (String.IsNullOrEmpty(ret)) {
//                if (ctx != null) {
//                    ctx.Parameters["instancename"] = ret;
//                }
//                if (String.IsNullOrEmpty(ret)) {
//                    ret = "SQLEXPRESS";
//                }
//            }

//            return ret;

//        }

//        public static string GetDatabaseConnectionString(InstallContext ctx, IDictionary state, string userName, string password) {
//            string ret = "" + Registry.GetValue(RootRegistryKey, "DatabaseConnectionString", "") as string;

//            ret = ret.Replace("__USERID__", userName).Replace("__PASSWORD__", password);

//            return ret;
//        }

//        public static void AddToRunOnce(string name, string fullFilePath) {
//            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce", name, fullFilePath);
//        }

//        public static bool IsProgramInstalled(string displayName) {
//            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall")) {
//                foreach (string skName in rk.GetSubKeyNames()) {
//                    using (RegistryKey sk = rk.OpenSubKey(skName)) {
//                        string displayNameInRegistry = sk.GetValue("DisplayName") as string;
//                        if (!String.IsNullOrEmpty(displayNameInRegistry)) {
//                            if (displayNameInRegistry.ToLower().Trim() == displayName.ToLower().Trim()) {
//                                return true;
//                            }
//                        }
//                    }
//                }
//            }
//            return false;
//        }

//        public static string GetUninstallCommand(string displayName, Dictionary<string, string> requiredKeyMatches) {
//            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall")) {
//                foreach (string skName in rk.GetSubKeyNames()) {
//                    using (RegistryKey sk = rk.OpenSubKey(skName)) {

//                        string displayNameInRegistry = sk.GetValue("DisplayName") as string;
//                        if (!String.IsNullOrEmpty(displayNameInRegistry)) {

//                            if (displayNameInRegistry.ToLower().Trim() == displayName.ToLower().Trim()) {

//                                string uninstallString = sk.GetValue("UninstallString") as string;
//                                if (!String.IsNullOrEmpty(uninstallString)) {

//                                    if (uninstallString.ToLower().StartsWith("msiexec.exe")) {

//                                        if (requiredKeyMatches != null) {
//                                            foreach (string key in requiredKeyMatches.Keys) {
//                                                string val = sk.GetValue(key) as string;
//                                                if (val != requiredKeyMatches[key]) {
//                                                    // we found the key, but the value associated with it is different than what we require.
//                                                    return null;
//                                                }
//                                            }
//                                        }

//                                        // for whatever reason, the uninstall path reports the /I option as the proper one for uninstallation
//                                        // as it provides the user the option to repair or uninstall.
//                                        // we not only want to uninstall but we want passive as well (no prompts).
//                                        uninstallString = uninstallString.Replace("/I{", "/passive /x{");
//                                        return uninstallString;
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            return null;
//        }

//        public static void SetSourceDirectory(string dir) {
//            Registry.SetValue(RootRegistryKey, "SourceDir", dir);
//        }

//        public static void SetDatabaseEngine(string engineName) {
//            Registry.SetValue(RootRegistryKey, "DatabaseEngine", engineName);
//        }

//        public static void SetDatabaseConnectionString(string connString) {
//            Registry.SetValue(RootRegistryKey, "DatabaseConnectionString", connString);
//        }


//        public static List<string> ListInstalledApplications() {
//            var installedApps = new List<string>();
//            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false)) {
//                string[] guids = rk.GetSubKeyNames();
//                foreach (string guid in guids) {
//                    string displayName = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + guid, "DisplayName", "") as string;
//                    if (!String.IsNullOrEmpty(displayName)) {
//                        installedApps.Add(displayName);
//                    }
//                }
//            }

//            return installedApps;
//        }





//        public static string ExtractCabFile(string cabFile, string destFolder, string pathToGguac) {

//            if (cabFile.Contains(" ") && !cabFile.StartsWith(@"""")) {
//                cabFile = @"""" + cabFile + @"""";
//            }
//            if (destFolder.Contains(" ") && !destFolder.StartsWith(@"""")) {
//                destFolder = @"""" + destFolder + @"""";
//            }
//            if (!Directory.Exists(destFolder.Replace(@"""", ""))) {
//                Directory.CreateDirectory(destFolder.Replace(@"""", ""));
//            }

//            ProcessStartInfo psi = null;

//            if (String.IsNullOrEmpty(pathToGguac)) {
//                psi = new ProcessStartInfo("expand.exe ", cabFile + " -F:* " + destFolder);
//            } else {
//                psi = new ProcessStartInfo(pathToGguac, " /wait /hide expand.exe " + cabFile + " -F:* " + destFolder);
//            }
//            psi.WindowStyle = ProcessWindowStyle.Hidden;
//            psi.UseShellExecute = false;
//            psi.RedirectStandardError = true;
//            psi.RedirectStandardOutput = true;
//            psi.CreateNoWindow = true;

////            MessageBox.Show("extracting cab file via command line: " + psi.FileName + " " + psi.Arguments);

//            Process p = Process.Start(psi);
//            string output = p.StandardOutput.ReadToEnd() + " " + p.StandardError.ReadToEnd();
//            p.WaitForExit();
//            return output;
//        }




    }
}
