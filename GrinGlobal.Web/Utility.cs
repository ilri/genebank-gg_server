using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Management;
using System.Reflection;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;
//using System.Windows.Forms;
using System.Configuration.Install;
using System.Collections;
using GrinGlobal.Core;

namespace GrinGlobal.Web {
    class Utility {

        public static readonly string RootRegistryKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\GRIN-Global";

        public static bool IsWindowsXP {
            get {
                return Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1;
            }
        }

        public static bool IsWindowsVista {
            get {
                return Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 0;
            }
        }

        public static bool IsWindowsVistaOrBetter {
            get {
                return Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 0;
            }
        }

        public static bool IsWindows7 {
            get {
                return Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1;
            }
        }

        public static bool IsWindowsVistaHome {
            get {
                return WindowsVistaEdition.ToLower() == "home premium";
            }
        }

        public static bool IsWindowsVistaUltimate {
            get {
                return WindowsVistaEdition.ToLower() == "ultimate";
            }
        }

        public static bool IsWindowsVistaBusiness {
            get {
                return WindowsVistaEdition.ToLower() == "business";
            }
        }


        public static string WindowsVistaEdition {
            get {
                return Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", "") as string;
            }
        }

        public static bool IsPowershellInstalled {
            get {
                string val = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1", "PID", "") as string;
                return !String.IsNullOrEmpty(val);
            }
        }

        public static string GetSystem32Directory() {
            return Environment.GetEnvironmentVariable("SystemRoot") + @"\system32\";
        }

        public static string InstallIIS() {

            if (Utility.IsWindowsXP) {
                return installIISOnXP();
            } else if (Utility.IsWindowsVistaOrBetter) {
                return installIISOnVista();
                //} else if (Utility.IsWindows7) {
                //    return "TODO: implement IIS install on Windows 7";
            }
            return "INVALID CONFIGURATION: OS of XP or Vista not detected; cannot install IIS";
        }

        private static string installIISOnVista() {

            string xml = null;
            if (Utility.IsWindowsVistaHome) {
                xml = getIisXmlForVistaHome();
            } else {
                xml = getIisXmlForVistaBusinessOrUltimate();
            }


            xml = xml.Replace("__PRODUCTVERSION__", getWindowsVistaProductVersionNumber());

            string tempPath = Environment.GetEnvironmentVariable("TEMP");
            string xmlFilePath = tempPath + @"\unattended_iis_install.xml";
            if (File.Exists(xmlFilePath)) {
                File.Delete(xmlFilePath);
            }
            File.WriteAllText(xmlFilePath, xml);


            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c start /wait pkgmgr.exe /n:unattended_iis_install.xml");
            psi.WorkingDirectory = tempPath;
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;

            Process p = Process.Start(psi);

            __processError = new StringBuilder();
            __processOutput = new StringBuilder();
            p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();

            while (!p.HasExited) {
                Thread.Sleep(100);
//                Application.DoEvents();
            }

            string output = __processOutput.ToString() + " " + __processError.ToString();

            return output;

        }

        static void p_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            if (__processError != null) {
                __processError.AppendLine(e.Data);
            }
        }

        static void p_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            if (__processOutput != null) {
                __processOutput.AppendLine(e.Data);
            }
        }

        public static void WriteFirewallException(string fullFilePath, string displayName, bool enabled) {
            string value = fullFilePath + ":*:" + (enabled ? "Enabled" : "Disabled") + ":" + displayName;
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\AuthorizedApplications\List", fullFilePath, value);
        }


        private static StringBuilder __processOutput;
        private static StringBuilder __processError;

        private static string getWindowsVistaProductVersionNumber() {
            string fileName = Environment.GetEnvironmentVariable("WINDIR") + @"\regedit.exe";
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fileName);
            return fvi.ProductVersion;
        }

        private static string getIisXmlForVistaBusinessOrUltimate() {
            string unattendedXml = @"
<?xml version=""1.0"" ?>
<unattend xmlns=""urn:schemas-microsoft-com:unattend""
    xmlns:wcm=""http://schemas.microsoft.com/WMIConfig/2002/State"">
<servicing>
   <!-- Install a selectable update in a package that is in the Windows Foundation namespace -->
   <package action=""configure"">
      <assemblyIdentity
         name=""Microsoft-Windows-Foundation-Package""
         version=""__PRODUCTVERSION__""
         language=""neutral""
         processorArchitecture=""x86""
         publicKeyToken=""31bf3856ad364e35""
         versionScope=""nonSxS""
      />
    <selection name=""IIS-WebServerRole"" state=""true""/>
    <selection name=""IIS-WebServer"" state=""true""/>
    <selection name=""IIS-CommonHttpFeatures"" state=""true""/>
    <selection name=""IIS-StaticContent"" state=""true""/>
    <selection name=""IIS-DefaultDocument"" state=""true""/>
    <selection name=""IIS-DirectoryBrowsing"" state=""true""/>
    <selection name=""IIS-HttpErrors"" state=""true""/>
    <selection name=""IIS-HttpRedirect"" state=""true""/>
    <selection name=""IIS-ApplicationDevelopment"" state=""true""/>
    <selection name=""IIS-ASPNET"" state=""true""/>
    <selection name=""IIS-NetFxExtensibility"" state=""true""/>
    <selection name=""IIS-ASP"" state=""true""/>
    <selection name=""IIS-CGI"" state=""true""/>
    <selection name=""IIS-ISAPIExtensions"" state=""true""/>
    <selection name=""IIS-ISAPIFilter"" state=""true""/>
    <selection name=""IIS-ServerSideIncludes"" state=""true""/>
    <selection name=""IIS-HealthAndDiagnostics"" state=""true""/>
    <selection name=""IIS-HttpLogging"" state=""true""/>
    <selection name=""IIS-LoggingLibraries"" state=""true""/>
    <selection name=""IIS-RequestMonitor"" state=""true""/>
    <selection name=""IIS-HttpTracing"" state=""true""/>
    <selection name=""IIS-CustomLogging"" state=""true""/>
    <selection name=""IIS-ODBCLogging"" state=""true""/>
    <selection name=""IIS-Security"" state=""true""/>
    <selection name=""IIS-BasicAuthentication"" state=""true""/>
    <selection name=""IIS-WindowsAuthentication"" state=""true""/>
    <selection name=""IIS-DigestAuthentication"" state=""true""/>
    <selection name=""IIS-ClientCertificateMappingAuthentication"" state=""true""/>
    <selection name=""IIS-IISCertificateMappingAuthentication"" state=""true""/>
    <selection name=""IIS-URLAuthorization"" state=""true""/>
    <selection name=""IIS-RequestFiltering"" state=""true""/>
    <selection name=""IIS-IPSecurity"" state=""true""/>
    <selection name=""IIS-Performance"" state=""true""/>
    <selection name=""IIS-HttpCompressionStatic"" state=""true""/>
    <selection name=""IIS-HttpCompressionDynamic"" state=""true""/>
    <selection name=""IIS-WebServerManagementTools"" state=""true""/>
    <selection name=""IIS-ManagementConsole"" state=""true""/>
    <selection name=""IIS-ManagementScriptingTools"" state=""true""/>
    <selection name=""IIS-ManagementService"" state=""true""/>
    <selection name=""IIS-IIS6ManagementCompatibility"" state=""true""/>
    <selection name=""IIS-Metabase"" state=""true""/>
    <selection name=""IIS-WMICompatibility"" state=""true""/>
    <selection name=""IIS-LegacyScripts"" state=""true""/>
    <selection name=""IIS-LegacySnapIn"" state=""true""/>
    <selection name=""IIS-FTPPublishingService"" state=""true""/>
    <selection name=""IIS-FTPServer"" state=""true""/>
    <selection name=""IIS-FTPManagement"" state=""true""/>
    <selection name=""WAS-WindowsActivationService"" state=""true""/>
    <selection name=""WAS-ProcessModel"" state=""true""/>
    <selection name=""WAS-NetFxEnvironment"" state=""true""/>
    <selection name=""WAS-ConfigurationAPI"" state=""true""/>
  </package>
</servicing>
</unattend>";

            return unattendedXml;
        }

        private static string getIisXmlForVistaHome() {
            string unattendedXml = @"
<?xml version=""1.0"" ?>                                                                                                                      
<unattend xmlns=""urn:schemas-microsoft-com:unattend""
    xmlns:wcm=""http://schemas.microsoft.com/WMIConfig/2002/State"">
<servicing>
   <!-- Install a selectable update in a package that is in the Windows Foundation namespace -->
   <package action=""configure"">
      <assemblyIdentity
         name=""Microsoft-Windows-Foundation-Package""
         version=""__PRODUCTVERSION__""
         language=""neutral""
        processorArchitecture=""x86""
         publicKeyToken=""31bf3856ad364e35""
         versionScope=""nonSxS""
      />
    <selection name=""IIS-WebServerRole"" state=""true""/>
    <selection name=""IIS-WebServer"" state=""true""/>
    <selection name=""IIS-CommonHttpFeatures"" state=""true""/>
    <selection name=""IIS-StaticContent"" state=""true""/>
    <selection name=""IIS-DefaultDocument"" state=""true""/>
    <selection name=""IIS-DirectoryBrowsing"" state=""true""/>
    <selection name=""IIS-HttpErrors"" state=""true""/>
    <selection name=""IIS-HttpRedirect"" state=""true""/>
    <selection name=""IIS-ApplicationDevelopment"" state=""true""/>
    <selection name=""IIS-ASPNET"" state=""true""/>
    <selection name=""IIS-NetFxExtensibility"" state=""true""/>
    <selection name=""IIS-ASP"" state=""true""/>
    <selection name=""IIS-CGI"" state=""true""/>
    <selection name=""IIS-ISAPIExtensions"" state=""true""/>
    <selection name=""IIS-ISAPIFilter"" state=""true""/>
    <selection name=""IIS-ServerSideIncludes"" state=""true""/>
    <selection name=""IIS-HealthAndDiagnostics"" state=""true""/>
    <selection name=""IIS-HttpLogging"" state=""true""/>
    <selection name=""IIS-LoggingLibraries"" state=""true""/>
    <selection name=""IIS-RequestMonitor"" state=""true""/>
    <selection name=""IIS-HttpTracing"" state=""true""/>
    <selection name=""IIS-CustomLogging"" state=""true""/>
    <selection name=""IIS-ODBCLogging"" state=""true""/>
    <selection name=""IIS-Security"" state=""true""/>
    <selection name=""IIS-BasicAuthentication"" state=""true""/>
    <selection name=""IIS-URLAuthorization"" state=""true""/>
    <selection name=""IIS-RequestFiltering"" state=""true""/>
    <selection name=""IIS-IPSecurity"" state=""true""/>
    <selection name=""IIS-Performance"" state=""true""/>
    <selection name=""IIS-HttpCompressionStatic"" state=""true""/>
    <selection name=""IIS-HttpCompressionDynamic"" state=""true""/>
    <selection name=""IIS-WebServerManagementTools"" state=""true""/>
    <selection name=""IIS-ManagementConsole"" state=""true""/>
    <selection name=""IIS-ManagementScriptingTools"" state=""true""/>
    <selection name=""IIS-ManagementService"" state=""true""/>
    <selection name=""IIS-IIS6ManagementCompatibility"" state=""true""/>
    <selection name=""IIS-Metabase"" state=""true""/>
    <selection name=""IIS-WMICompatibility"" state=""true""/>
    <selection name=""IIS-LegacyScripts"" state=""true""/>
    <selection name=""IIS-LegacySnapIn"" state=""true""/>
    <selection name=""WAS-WindowsActivationService"" state=""true""/>
    <selection name=""WAS-ProcessModel"" state=""true""/>
    <selection name=""WAS-NetFxEnvironment"" state=""true""/>
    <selection name=""WAS-ConfigurationAPI"" state=""true""/>
  </package>
</servicing>
</unattend>";
            return unattendedXml;
        }

        private static string installIISOnVistaBusinessOrUltimate() {
            return null;
        }

        private static string installIISOnXP() {
            // http://www.microsoft.com/technet/prodtechnol/WindowsServer2003/Library/IIS/efefcb53-b86e-4cac-9b4b-fcf5f1145aa9.mspx?mfr=true
            // http://geekswithblogs.net/sdorman/archive/2007/03/01/107732.aspx

            string fileName = Environment.GetEnvironmentVariable("TEMP") + @"\iis.install.txt";

            // create an inf file 
            using (StreamWriter sw = File.CreateText(fileName)) {
                sw.WriteLine(@"[Components]
iis_common = on
iis_inetmgr = on
iis_www = on
iis_ftp = off
iis_htmla = off

[InternetServer]
PathWWWRoot=C:\Inetpub\Wwwroot
");
            }

            try {
                // run the sysocmgr against that file to install IIS
                ProcessStartInfo psiIIS = new ProcessStartInfo("sysocmgr.exe", @" /i:%windir%\inf\sysoc.inf /u:""" + fileName + @"""");
                psiIIS.UseShellExecute = false;
                psiIIS.WorkingDirectory = Utility.GetSystem32Directory();
                psiIIS.RedirectStandardOutput = true;
                psiIIS.RedirectStandardError = true;
                Process p1 = Process.Start(psiIIS);
                string output = p1.StandardOutput.ReadToEnd() + " " + p1.StandardError.ReadToEnd();
                p1.WaitForExit();
                return output;
            } catch (Exception ex1) {
                Debug.WriteLine("Exception installing IIS: " + ex1.Message);
                throw;
            }
        }

        public static bool IsIISInstalled {
            get {
                try {
                    int majorVersion = (int)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\InetStp", "MajorVersion", 0);
                    // even if IIS is installed, we have to know if the w3svc is installed as well...
                    string w3svc = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W3SVC", "ImagePath", "");

                    return majorVersion >= 5 && !String.IsNullOrEmpty(w3svc);
                } catch {
                    return false;
                }

            }
        }

        public static string GetIISRootPath() {
            try {
                string path = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\InetStp", "PathWWWRoot", "") as string;
                // even if IIS is installed, we have to know if the w3svc is installed as well...

                return path;

            } catch {
                return @"not found";
            }
        }

        public static string GetTargetDirectory(InstallContext ctx, IDictionary state, string appName) {

            string ret = null;

            // pull from context if possible
            if (ctx != null) {
                ret = ctx.Parameters["tgt"];
                if (String.IsNullOrEmpty(ret)) {
                    ret = ctx.Parameters["TARGETDIR"];
                }
            }

            if (state != null && String.IsNullOrEmpty(ret)) {
                ret = state["TARGETDIR"] as string;
            }

            if (String.IsNullOrEmpty(ret)) {
                // look to the registry if we can't glean it from the context
                ret = Registry.GetValue(RootRegistryKey + @"\" + appName, "InstallPath", "") as string;
                if (String.IsNullOrEmpty(ret)) {
                    // and finally the current directory as a last-ditch effort
                    ret = Environment.CurrentDirectory;
                }
            }
            if (ctx != null) {
                ctx.Parameters["tgt"] = ret;
            }
            return ret;
        }

        public static string GetTargetVirtualDirectory(InstallContext ctx, IDictionary state, string appName) {

            string ret = null;

            // pull from context if possible
            if (ctx != null) {
                ret = ctx.Parameters["vtgt"];
                if (String.IsNullOrEmpty(ret)) {
                    ret = ctx.Parameters["TARGETVDIR"];
                }
            }

            if (state != null && String.IsNullOrEmpty(ret)) {
                ret = state["TARGETVDIR"] as string;
            }

            if (String.IsNullOrEmpty(ret)) {
                // look to the registry if we can't glean it from the context
                ret = Registry.GetValue(RootRegistryKey + @"\" + appName, "InstallPath", "") as string;
                if (String.IsNullOrEmpty(ret)) {

                    ret = Utility.GetIISRootPath();

                    if (String.IsNullOrEmpty(ret)) {
                        // and finally the current directory as a last-ditch effort
                        ret = Environment.CurrentDirectory;
                    }
                }
            }
            if (ctx != null) {
                ctx.Parameters["vtgt"] = ret;
            }
            return ret;
        }


        public static string GetSourceDirectory(InstallContext ctx, IDictionary state) {

            string ret = null;

            if (ctx != null) {
                ret = ctx.Parameters["src"];
                if (String.IsNullOrEmpty(ret)) {
                    ret = ctx.Parameters["SOURCEDIR"];
                }
            }


            if (String.IsNullOrEmpty(ret) && state != null) {
                ret = state["SOURCEDIR"] as string;
            }

            if (String.IsNullOrEmpty(ret)) {
                ret = Registry.GetValue(RootRegistryKey, "SourceDir", "") as string;
            }

            if (ctx != null) {
                ctx.Parameters["src"] = ret;
            }
            return ret;
        }



        public static IEnumerable<string> GetAllFiles(string directory, bool recurse) {
            if (Directory.Exists(directory)) {
                if (recurse) {
                    foreach (string f in Directory.GetDirectories(directory)) {
                        foreach (string sf in GetAllFiles(f, true)) {
                            yield return sf;
                        }
                    }
                }
                foreach (string s in Directory.GetFiles(directory)) {
                    yield return s;
                }
            }
        }

        public static decimal ToDecimal(object value, decimal defaultValue) {
            if (value == DBNull.Value || value == null) {
                return defaultValue;
            } else {
                return ToDecimal(value.ToString(), defaultValue);
            }
        }


        public static decimal ToDecimal(string value, decimal defaultValue) {
            decimal ret;
            if (decimal.TryParse(value, out ret)) {
                return ret;
            } else {
                return defaultValue;
            }
        }

        public static int ToInt32(object value, int defaultValue) {
            if (value == DBNull.Value || value == null) {
                return defaultValue;
            } else if (value is string) {
                return ToInt32((string)value, defaultValue);
            } else if (!(value is int)) {
                return ToInt32(value.ToString(), defaultValue);
            } else {
                try {
                    return (int)value;
                } catch {
                    return defaultValue;
                }
            }
        }


        public static long ToInt64(string value, long defaultValue) {
            return (long)ToInt64(value, (long?)defaultValue);
        }

        public static long? ToInt64(string value, long? defaultValue) {
            long ret;
            if (long.TryParse(value, out ret)) {
                return ret;
            } else {
                return defaultValue;
            }
        }

        public static long ToInt64(object value, long defaultValue) {
            if (value == DBNull.Value || value == null) {
                return defaultValue;
            } else if (value is string) {
                return ToInt64((string)value, defaultValue);
            } else if (!(value is long)) {
                return ToInt64(value.ToString(), defaultValue);
            } else {
                try {
                    return (long)value;
                } catch {
                    return defaultValue;
                }
            }
        }

        public static DateTime ToDateTime(string value, DateTime defaultValue) {
            DateTime ret;
            if (DateTime.TryParse(value, out ret)) {
                return ret;
            } else {
                return defaultValue;
            }
        }

        public static DateTime ToDateTime(object value, DateTime defaultValue) {
            if (value == null) {
                return defaultValue;
            } else {
                return ToDateTime(value.ToString(), defaultValue);
            }
        }

        public static string GetApplicationVersion(string appName) {
            Assembly asm = Assembly.GetEntryAssembly();
            if (asm == null) {
                asm = Assembly.GetCallingAssembly();
                if (asm == null) {
                    asm = Assembly.GetExecutingAssembly();
                }
                if (asm == null) {
                    return "Unknown assembly!";
                }
            }
            AssemblyName an = asm.GetName();
            return appName + " v " + an.Version.ToString();
        }


        /// <summary>
        /// Given a path to a directory, resolves it to an absolute path.  Creates directories as needed. Environment variables are resolved (e.g. "%SystemRoot%\ -> "C:\windows\") and special folders are resolved (e.g. "*ApplicationData*\" -> "C:\Documents and Settings\[current user]\Application Data\").
        /// </summary>
        /// <param name="directoryName">Root-relative, document-relative, or absolute path to a directory.  ** delimiters are used for "special" folders like - *Application Data*</param>
        /// <param name="createDirectoriesAsNeeded">If necessary, creates all folders up to root to make sure directory can be created.</param>
        /// <returns>Absolute path (parent directories guaranteed to be created) to a directory.</returns>
        [ComVisible(true)]
        public static string ResolveDirectoryPath(string directoryName, bool createDirectoriesAsNeeded) {

            directoryName = resolvePath(directoryName);

            if (String.IsNullOrEmpty(directoryName)) {
                return String.Empty;
            }

            if (!Directory.Exists(directoryName) && createDirectoriesAsNeeded) {
                Directory.CreateDirectory(directoryName);
            }

            return directoryName;
        }

        /// <summary>
        /// Given a directory path, will make sure it is completely empty.  Does not drop then re-add directory (so permissions are not altered).
        /// </summary>
        /// <param name="directoryName"></param>
        [ComVisible(true)]
        public static void EmptyDirectory(string directoryName) {
            directoryName = ResolveDirectoryPath(directoryName, true);
            foreach (string dir in Directory.GetDirectories(directoryName)) {
                Directory.Delete(dir, true);
            }
            foreach (string fil in Directory.GetFiles(directoryName)) {
                File.Delete(fil);
            }
        }

        /// <summary>
        /// Given a path to a file, resolves it to an absolute path.  Creates directories as needed. Environment variables are resolved (e.g. "%SystemRoot%\joe.txt" -> "C:\windows\joe.txt") and special folders are resolved (e.g. "*ApplicationData*\fred.txt" -> "C:\Documents and Settings\[current user]\Application Data\fred.txt").
        /// </summary>
        /// <param name="fileName">Root-relative, document-relative, or absolute path to a file.  ** delimiters are used for "special" folders like - *Application Data*</param>
        /// <param name="createDirectoriesAsNeeded">If necessary, creates all folders up to root to make sure file can be created.</param>
        /// <returns>Absolute path (parent directories guaranteed to be created) to a file.  Does not create the file.</returns>
        [ComVisible(true)]
        public static string ResolveFilePath(string fileName, bool createDirectoriesAsNeeded) {

            fileName = resolvePath(fileName);

            if (String.IsNullOrEmpty(fileName)) {
                return String.Empty;
            }

            if (!File.Exists(fileName)) {
                FileInfo fi = new FileInfo(fileName);
                if (!Directory.Exists(fi.DirectoryName) && createDirectoriesAsNeeded) {
                    Directory.CreateDirectory(fi.DirectoryName);
                }
            }

            return fileName;
        }

        public static bool IsWindowsVistaOrLater {
            get {
                return Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 0;
            }
        }

        public static string Cut(string value, int startPosition, int length) {
            if (String.IsNullOrEmpty(value)) {
                return value;
            }

            if (length != value.Length && startPosition != 0) {
                // they specified both ends.
                // recurse to resolve front end first
                value = Cut(value, startPosition, value.Length);
                startPosition = 0;
            }

            if (length < 0) {
                // they're saying exclude last 'length' characters.
                // so Toolkit.Cut('asdf', 0, -2) = 'as'
                length = value.Length - Math.Abs(length);
                if (length < 0) {
                    // they're trying to cut off more than they have.
                    // i.e. Toolkit.Cut('asdf', 0, -8) = ''
                    // return zero-length string
                    return string.Empty;
                }
            }
            if (length > value.Length) {
                // they're saying return more than they have.
                // limit to all there is
                // ie.. Toolkit.Cut('asdf', 0, 8) = 'asdf'
                length = value.Length;
            }




            /*
            // startPosition is actually end position.
            // substract length from startPosition to get real start position
            startPosition += length;
            // and toggle length so it's now positive
            length *= -1;
             */
            if (startPosition < 0) {
                // pull from the right
                startPosition = value.Length + startPosition;
                // if they specified a negative start position less than the length, just pull from the beginning.
            }

            if (startPosition < 0) {
                startPosition = 0;
            }

            if (startPosition + length > value.Length) {
                // they said pull more than they have. just return whole string.
                return value.Substring(startPosition);
            } else if (startPosition + length < 0) {
                // they said pull negative chars. return empty string.
                return string.Empty;
            } else {
                // they said pull a valid subset of chars.
                return value.Substring(startPosition, length);
            }

        }

        public static string Cut(string value, int startPosition) {
            return Cut(value, startPosition, (value == null ? 0 : value.Length));
        }

        public static DomainUser GetDomainUser(string processName, bool ignoreSystemUsers) {
            SelectQuery selectQuery = new SelectQuery("select * from Win32_Process where name='" + processName + "'");

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery)) {
                using (ManagementObjectCollection processes = searcher.Get()) {
                    foreach (ManagementObject process in processes) {
                        using (process) {
                            //out argument to return user and domain
                            string[] s = new String[2];
                            //Invoke the GetOwner method and populate the array with the user name and domain
                            process.InvokeMethod("GetOwner", (object[])s);
                            DomainUser du = new DomainUser(s[1], s[0]);
                            if (ignoreSystemUsers && DomainUser.IsSystemAccount(du)) {
                                // keep looking
                            } else {
                                return du;
                            }
                        }
                    }
                }
            }
            return null;
        }


        private static string resolvePath(string path) {
            if (String.IsNullOrEmpty(path)) {
                // always return empty instead of null in case callers are doing string manipulation checks
                return String.Empty;
            }

            // resolve any environment variables
            path = Environment.ExpandEnvironmentVariables(path);

            // resolve any "special" folders
            if (path.Contains("*")) {
                // pluck out the ** portion
                int front = path.IndexOf("*");
                int back = path.LastIndexOf("*");
                if (front == back) {
                    throw new InvalidOperationException("Badly formed path (missing corresponding * delimiter: '" + path + "'.");
                }

                string special = path.Substring(front + 1, back - 1).Replace(" ", "");
                special = Environment.GetFolderPath((Environment.SpecialFolder)Enum.Parse(typeof(Environment.SpecialFolder), special));

                // yuck. Environment.GetFolderPath doesn't always report the correct user when
                // we've launched a process as another user via pinvoke. Probably not
                // loading the app domain properly or something, but I don't have time to
                // research.  total hack here!
                if (!special.Contains(@"\" + DomainUser.Current.UserName + @"\")) {
                    // it didn't resolve the current user properly.  Let's fudge it.
                    string docsAndSettings = (IsWindowsVistaOrLater ? @"\Users\" : @"\Documents and Settings\");
                    int pos1Start = special.IndexOf(docsAndSettings);
                    int pos1End = pos1Start + docsAndSettings.Length;
                    int pos2 = special.IndexOf(@"\", pos1End + 1);
                    special = Cut(special, 0, pos1End) + DomainUser.Current.UserName + Cut(special, pos2);
                }


                //#if DEBUG
                //                Logger.LogText("cur user=" + DomainUser.Current + "; special folder=" + special, false, true);
                //#endif

                path = path.Substring(0, front) + special + path.Substring(back + 1);
            }

            if (path.IndexOf("~") == 0) {
                path = AppDomain.CurrentDomain.BaseDirectory + path.Substring(1);
            }

            // resolve folders to base directory if not absolute
            if (path.IndexOf(@":\") == -1 && path.IndexOf(@"\") != 0) {
                path = AppDomain.CurrentDomain.BaseDirectory + path;
                FileInfo fi = new FileInfo(path);
                path = fi.Directory + @"\" + fi.Name;
            }

            // resolve any relativity in the middle of the path
            path = Path.GetFullPath(path);

            return path;
        }

        public static string Run(string fullName, string arguments, bool hideWindow, bool waitForExitAndReturnOutput) {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(fullName, arguments);
            if (hideWindow) {
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }

            if (waitForExitAndReturnOutput) {
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.UseShellExecute = false;
            }
            p.StartInfo.WorkingDirectory = Directory.GetParent(fullName).FullName;

            p.Start();

            if (waitForExitAndReturnOutput) {
                // to avoid a deadlock, we have to read to end of standard output before waiting for exit!
                string output = p.StandardOutput.ReadToEnd();
                if (String.IsNullOrEmpty(output)) {
                    output = p.StandardError.ReadToEnd();
                }
                p.WaitForExit();
                return output;
            }

            return null;

        }

        public static string GetDatabaseEngine(InstallContext ctx, IDictionary state, bool throwExceptionIfNotFound) {
            string ret = null;

            StringBuilder sb = new StringBuilder();

            if (ctx != null) {

                if (ctx.Parameters["action"] == "uninstall") {
                    // default to using registry entries
                    if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                        ret = Registry.GetValue(RootRegistryKey + @"\Database", "DatabaseEngine", "") as string;
                        sb.AppendLine("Registry.GetValue(" + RootRegistryKey + @"\Database\DatabaseEngine)=" + ret);
                    }
                    if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                        ret = Registry.GetValue(RootRegistryKey, "DatabaseEngine", "") as string;
                        sb.AppendLine("Registry.GetValue(" + RootRegistryKey + "DatabaseEngine)=" + ret);
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

            if (!DatabaseEngineUtil.IsValidEngine(ret) && state != null) {
                ret = state["ENGINE"] as string;
                sb.AppendLine("state['ENGINE']=" + ret);
            }

            if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                ret = Registry.GetValue(RootRegistryKey, "DatabaseEngine", "") as string;
                sb.AppendLine("Registry.GetValue(" + RootRegistryKey + "DatabaseEngine)=" + ret);
            }

            if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                ret = Registry.GetValue(RootRegistryKey + @"\Database", "DatabaseEngine", "") as string;
                sb.AppendLine("Registry.GetValue(" + RootRegistryKey + @"\Database\DatabaseEngine)=" + ret);
            }

            // HACK: always make it sqlserver until we have other options available...
            if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                ret = "sqlserver";
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


        public static string GetDatabaseInstanceName(InstallContext ctx, IDictionary state) {
            string ret = null;

            StringBuilder sb = new StringBuilder();

            if (ctx != null) {

                if (ctx.Parameters["action"] == "uninstall") {
                    // default to using registry entries
                    if (String.IsNullOrEmpty(ret)) {
                        ret = Registry.GetValue(RootRegistryKey + @"\Database", "DatabaseInstanceName", "") as string;
                        sb.AppendLine("Registry.GetValue(" + RootRegistryKey + @"\Database\DatabaseInstanceName)=" + ret);
                    }
                    if (String.IsNullOrEmpty(ret)) {
                        ret = Registry.GetValue(RootRegistryKey, "DatabaseInstanceName", "") as string;
                        sb.AppendLine("Registry.GetValue(" + RootRegistryKey + "DatabaseInstanceName)=" + ret);
                    }
                }


                if (String.IsNullOrEmpty(ret)) {
                    ret = ctx.Parameters["INSTANCENAME"];
                    sb.AppendLine("\r\nContext.Parameters['INSTANCENAME']=" + ret);
                    if (String.IsNullOrEmpty(ret)) {
                        ret = ctx.Parameters["INSTANCENAME"];
                        sb.AppendLine("Context.Parameters['INSTANCENAME']=" + ret);
                    }
                }
            }

            if (String.IsNullOrEmpty(ret) && state != null) {
                ret = state["INSTANCENAME"] as string;
                sb.AppendLine("state['INSTANCENAME']=" + ret);
            }

            if (String.IsNullOrEmpty(ret)) {
                ret = Registry.GetValue(RootRegistryKey, "DatabaseInstanceName", "") as string;
                sb.AppendLine("Registry.GetValue(" + RootRegistryKey + "DatabaseInstanceName)=" + ret);
            }

            if (String.IsNullOrEmpty(ret)) {
                ret = Registry.GetValue(RootRegistryKey + @"\Database", "DatabaseInstanceName", "") as string;
                sb.AppendLine("Registry.GetValue(" + RootRegistryKey + @"\Database\DatabaseInstanceName)=" + ret);
            }

            if (String.IsNullOrEmpty(ret)) {
                if (ctx != null) {
                    ctx.Parameters["instancename"] = ret;
                }
                if (String.IsNullOrEmpty(ret)) {
                    ret = "SQLEXPRESS";
                }
            }

            return ret;

        }



    }
}
