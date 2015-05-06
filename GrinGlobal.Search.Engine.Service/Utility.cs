using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Configuration.Install;
using System.Collections;
using System.Diagnostics;
using GrinGlobal.Core;

namespace GrinGlobal.InstallHelper {
    class DbUtility {

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

        //public static string GetGrinGlobalApplicationInstallPath(string appName, string defaultIfEmpty) {
        //    string ret = Registry.GetValue(RootRegistryKey + @"\" + appName, "InstallPath", "") as string;
        //    return String.IsNullOrEmpty(ret) ? defaultIfEmpty : ret;
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
        //    // [basedirectory]\installers
        //    // [sourcedirectory]\bin
        //    // [currentpath]\installers

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

        public static string GetDatabaseEngine(InstallContext ctx, IDictionary state, bool throwExceptionIfNotFound) {
            string ret = null;

            StringBuilder sb = new StringBuilder();

            if (ctx != null) {
                ret = ctx.Parameters["engine"];
                sb.AppendLine("\r\nContext.Parameters['engine']=" + ret);
                if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                    ret = ctx.Parameters["ENGINE"];
                    sb.AppendLine("Context.Parameters['ENGINE']=" + ret);
                }
            }

            if (!DatabaseEngineUtil.IsValidEngine(ret) && state != null) {
                ret = state["ENGINE"] as string;
                sb.AppendLine("state['ENGINE']=" + ret);
            }

            //if (!DatabaseEngineUtil.IsValidEngine(ret)) {
            //    ret = Registry.GetValue(RootRegistryKey + @"\Database", "DatabaseEngine", "") as string;
            //    sb.AppendLine("Registry.GetValue(" + RootRegistryKey + @"\Database\DatabaseEngine)=" + ret);
            //}

            if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                ret = Toolkit.GetRegSetting(Utility.RootRegistryKey + @"\Database", "DatabaseEngine", "") as string;
                sb.AppendLine("Toolkit.GetRegSetting(" + Utility.RootRegistryKey + @"\Database\DatabaseEngine)=" + ret);
            }

            if (!DatabaseEngineUtil.IsValidEngine(ret)) {
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
