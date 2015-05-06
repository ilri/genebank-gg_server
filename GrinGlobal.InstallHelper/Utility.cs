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
using System.Windows.Forms;
using System.Configuration.Install;
using System.Collections;
using System.DirectoryServices;

using GrinGlobal.Core;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Net;
using System.Web.Services.Protocols;

namespace GrinGlobal.InstallHelper {
    public class Utility {

        public static readonly string RootRegistryKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\GRIN-Global";

        public static Dictionary<string, FileDownloadInfo> ListInstalledGrinGlobalApps() {

            var ret = new Dictionary<string, FileDownloadInfo>();

            var apps = Utility.ListInstalledApplications();
            foreach (var key in apps.Keys) {
                if (key.ToLower().StartsWith("grin-global")) {
                    ret.Add(key, apps[key]);
                }
            }

            return ret;
        }


        public static Dictionary<string, string> GetGrinGlobalApplicationInstallPaths() {
            var ret = new Dictionary<string, string>();

            using (var rk = Registry.LocalMachine.OpenSubKey(RootRegistryKey.Replace(@"HKEY_LOCAL_MACHINE\", ""))) {
                if (rk != null) {
                    var names = rk.GetSubKeyNames();
                    if (names != null && names.Length > 0){
                        foreach (var nm in names) {
                            if (!ret.ContainsKey(nm)) {
                                var path = Toolkit.GetRegSetting(RootRegistryKey + @"\" + nm, "InstallPath", "");
                                if (!String.IsNullOrEmpty(path)) {
                                    ret.Add(nm, path);
                                }
                                path = Toolkit.GetRegSetting(RootRegistryKey + @"\" + nm, "InstallDir", "");
                                if (!String.IsNullOrEmpty(path)) {
                                    ret.Add(nm, path);
                                }
                            }
                        }
                    }
                }
            }
            if (Utility.Is64BitOperatingSystem) {
                using (var rk = Registry.LocalMachine.OpenSubKey(RootRegistryKey.Replace(@"HKEY_LOCAL_MACHINE\", "").Replace(@"\SOFTWARE\", @"\SOFTWARE\Wow6432Node\"))) {
                    if (rk != null) {
                        var names = rk.GetSubKeyNames();
                        if (names != null && names.Length > 0) {
                            foreach (var nm in names) {
                                if (!ret.ContainsKey(nm)) {
                                    var path = Toolkit.GetRegSetting(RootRegistryKey + @"\" + nm, "InstallPath", "");
                                    if (!String.IsNullOrEmpty(path)) {
                                        ret.Add(nm, path);
                                    }
                                    path = Toolkit.GetRegSetting(RootRegistryKey + @"\" + nm, "InstallDir", "");
                                    if (!String.IsNullOrEmpty(path)) {
                                        ret.Add(nm, path);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }


        public static string GetGrinGlobalApplicationInstallPath(string appName, string defaultIfEmpty) {
            string ret = Toolkit.GetRegSetting(RootRegistryKey + @"\" + appName, "InstallPath", "") as string;
            if (String.IsNullOrEmpty(ret)) {
                ret = Toolkit.GetRegSetting(RootRegistryKey + @"\" + appName, "InstallDir", "") as string;
            }
            return String.IsNullOrEmpty(ret) ? defaultIfEmpty : ret;
        }

        /// <summary>
        /// Using expand.exe, extracts all files from the given cabFile to the destFolder.  If given, the exe to cause a UAC prompt is run to make sure the user has appropriate rights.
        /// </summary>
        /// <param name="cabFile"></param>
        /// <param name="destFolder"></param>
        /// <param name="pathToHelperExe"></param>
        /// <returns></returns>
        public static string ExtractCabFile(string cabFile, string destFolder, string fullPathToUACExe) {

            if (cabFile.Contains(" ") && !cabFile.StartsWith(@"""")) {
                cabFile = @"""" + cabFile + @"""";
            }
            if (destFolder.Contains(" ") && !destFolder.StartsWith(@"""")) {
                destFolder = @"""" + destFolder + @"""";
            }
            if (!Directory.Exists(destFolder.Replace(@"""", ""))) {
                Directory.CreateDirectory(destFolder.Replace(@"""", ""));
            }

            ProcessStartInfo psi = null;

            if (String.IsNullOrEmpty(fullPathToUACExe)) {
                psi = new ProcessStartInfo("expand.exe", " " + cabFile + " -F:* " + destFolder);
            } else {
                psi = new ProcessStartInfo(fullPathToUACExe, " /wait /hide expand.exe " + cabFile + " -F:* " + destFolder);
            }
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;

//            MessageBox.Show("extracting cab file via command line: " + psi.FileName + " " + psi.Arguments);

            Process p = Process.Start(psi);
            string output = p.StandardOutput.ReadToEnd() + " " + p.StandardError.ReadToEnd();
            p.WaitForExit();
            return output;
        }




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

        public static string OperatingSystemVersion {
            get {
                return Environment.OSVersion.VersionString;
            }
        }

        public static bool IsWindowsVistaHome {
            get {
                var edition = WindowsVistaEdition.ToLower();
                return !String.IsNullOrEmpty(edition) && edition.Contains("home") && edition.Contains("premium");
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
                return Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", "") as string;
            }
        }

        public static bool IsPowershellInstalled {
            get {
                string val = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1", "PID", "") as string;
                return !String.IsNullOrEmpty(val);
            }
        }

        /// <summary>
        /// Returns the system32 directory with trailing backslash.  Typically "C:\Windows\system32\".
        /// </summary>
        /// <returns></returns>
        public static string GetSystem32Directory() {
            return Environment.GetEnvironmentVariable("SystemRoot") + @"\system32\";
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
                    throw new InvalidOperationException(getDisplayMember("resolvePath{mismatchedasterisk}", "Badly formed path (missing corresponding * delimiter: '{0}'.", path));
                }

                string special = path.Substring(front + 1, back - 1).Replace(" ", "");
                special = Environment.GetFolderPath((Environment.SpecialFolder)Enum.Parse(typeof(Environment.SpecialFolder), special));

                //// yuck. Environment.GetFolderPath doesn't always report the correct user when
                //// we've launched a process as another user via pinvoke. Probably not
                //// loading the app domain properly or something, but I don't have time to
                //// research.  total hack here!
                //if (!special.Contains(@"\" + DomainUser.Current.UserName + @"\")) {
                //    // it didn't resolve the current user properly.  Let's fudge it.
                //    string docsAndSettings = (IsWindowsVistaOrLater ? @"\Users\" : @"\Documents and Settings\");
                //    int pos1Start = special.IndexOf(docsAndSettings);
                //    int pos1End = pos1Start + docsAndSettings.Length;
                //    int pos2 = special.IndexOf(@"\", pos1End + 1);
                //    special = Cut(special, 0, pos1End) + DomainUser.Current.UserName + Cut(special, pos2);
                //}


                //#if DEBUG
                //                Logger.LogText("cur user=" + DomainUser.Current + "; special folder=" + special, false, true);
                //#endif

                path = path.Substring(0, front) + special + path.Substring(back + 1);
            }

            if (path.IndexOf("~") == 0) {
                path = AppDomain.CurrentDomain.BaseDirectory + path.Substring(1);
            }

            // resolve folders to base directory if not absolute
            if (path.IndexOf(@":" + Path.DirectorySeparatorChar) == -1 && path.IndexOf(Path.DirectorySeparatorChar) != 0) {
                path = AppDomain.CurrentDomain.BaseDirectory + path;
                FileInfo fi = new FileInfo(path);
                path = fi.Directory + Path.DirectorySeparatorChar.ToString() + fi.Name;
            }

            // to make sure the correct version of path separators exist, replace both '/' and '\' with the os-specific equivalent.
            path = path.Replace(@"\", Path.DirectorySeparatorChar.ToString()).Replace("/", Path.DirectorySeparatorChar.ToString());

            // resolve any relativity in the middle of the path
            path = Path.GetFullPath(path);

            //Console.WriteLine("resolved path=" + path);

            return path;
        }


        public static string Run(string fullName, string arguments, bool hideWindow, bool waitForExitAndReturnOutput) {
            fullName = Utility.ResolveFilePath(fullName, true);
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(fullName, arguments);
            if (hideWindow) {
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
            }

            if (waitForExitAndReturnOutput) {
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.UseShellExecute = false;
            }
            p.StartInfo.WorkingDirectory = Directory.GetParent(fullName).FullName;

            if (!waitForExitAndReturnOutput) {
                p.Start();

            } else {
                __processError = new StringBuilder();
                __processOutput = new StringBuilder();
                p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
                p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);

                p.Start();

                p.BeginErrorReadLine();
                p.BeginOutputReadLine();

                while (!p.HasExited) {
                    Thread.Sleep(100);
                    Application.DoEvents();
                }
                string output = __processOutput.ToString() + " " + __processError.ToString();
                //if (String.IsNullOrEmpty(output)){
                //    output += __processError.ToString();
                //}
                return output;
            }

            //if (waitForExitAndReturnOutput) {
            //    // to avoid a deadlock, we have to read to end of standard output before waiting for exit!
            //    string output = p.StandardOutput.ReadToEnd();
            //    if (String.IsNullOrEmpty(output)) {
            //        output = p.StandardError.ReadToEnd();
            //    }
            //    p.WaitForExit();
            //    return output;
            //}

            return null;

        }



        public static bool ActivateApplication(IntPtr windowHandle) {
            try {
                return SetForegroundWindow(windowHandle);
            } catch (Exception ex) {
                // Windows C API may be different on 64-bit, or Vista, or whatever -- this is a non-essential function so just eat errors
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private static string HIVE = @"HKEY_LOCAL_MACHINE\SOFTWARE\GRIN-Global\Updater";
        public static int GetRegSetting(string name, int defaultValue) {
            return Utility.ToInt32(Toolkit.GetRegSetting(HIVE, name, defaultValue), defaultValue);
        }
        public static void SetRegSetting(string name, int value, bool forceWriteTo64BitRegistry) {
            Toolkit.SaveRegSetting(HIVE, name, value, forceWriteTo64BitRegistry);
        }

        public static string GetSetting(string name, string defaultValue) {
            return Toolkit.GetSetting(name, defaultValue);
        }

        public static string GetRegSetting(string name, string defaultValue) {
            return Toolkit.GetRegSetting(HIVE, name, defaultValue) as string;
        }
        public static void SetRegSetting(string name, string value, bool forceWriteTo64BitRegistry) {
            Toolkit.SaveRegSetting(HIVE, name, value, forceWriteTo64BitRegistry);
        }

        public static bool Is64BitOperatingSystem {
            get {
                return Toolkit.Is64BitOperatingSystem();
            }
        }

        public static bool Is64BitProcess {
            get {
                return Toolkit.Is64BitProcess();
            }
        }

        public static string GetIISPhysicalPath(string appName) {

            return Toolkit.GetIISPhysicalPath(appName);
        }

        public static string GetProcessNameByPort(int port) {
            var pid = GetProcessIDByPort(port);
            if (pid > -1) {
                var p = Process.GetProcessById(pid);
                if (p != null) {
                    return p.ProcessName;
                }
            }

            return null;
        }

        public static int GetProcessIDByPort(int port) {
            var allPorts = Utility.Run(Utility.GetSystem32Directory() + "netstat.exe", "-p tcp -ano", true, true);
            var lines = allPorts.Split(new string[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines) {
                var tabs = line.Split(new string[]{" "}, StringSplitOptions.RemoveEmptyEntries);
                for(var i=0;i<tabs.Length;i++){
                    var tab = tabs[i];
                    if (tab.Trim().EndsWith(":" + port)) {
                        return Toolkit.ToInt32(tabs[tabs.Length - 1].Trim(), -1);
                    }
                }
            }
            return -1;
        }

        public static bool IsAspNet20Registered() {
            string aspNetRegIIS = @"%WINDIR%\microsoft.net\framework" + (Utility.Is64BitOperatingSystem ? "64" : "") + @"\v2.0.50727\aspnet_regiis.exe";
            string s = Utility.Run(aspNetRegIIS, "-lv", true, true);

            if (Utility.IsWindowsXP) {
                // XP reports .net even if it's not properly mapped in IIS.  If it is, it will report it with "Valid (Root)".  Otherwise, it's just "Valid".
                // INSTALLED IN IIS:
                // 2.0.50727.0    Valid (Root)     C:\Windows\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll
                //
                // NOT INSTALLED IN IIS:
                // 2.0.50727.0    Valid            C:\Windows\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll
                //  -- OR --
                // Cannot find any installed version.
                return s.Contains("2.0.50727.0") && s.ToLower().Contains("valid (root)");

            } else {
                // Vista and Windows 7 properly report .NET (32-bit has only second line, 64-bit will have both)..
                // INSTALLED IN IIS: 
                // 2.0.50727.0             C:\Windows\Microsoft.NET\Framework64\v2.0.50727\aspnet_isapi.dll
                // 2.0.50727.0             C:\Windows\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll
                //
                // NOT INSTALLED IN IIS:
                // Cannot find any installed version.
                // (NOTE: that's assuming no other versions of .NET exist.  We can't assume that, so we explicitly check for 2.0.50727.0.
                return s.Contains("2.0.50727.0");

            }
        }
        public static void RegisterAspNet20InIIS() {
            //if (!s.ToLower().Contains("2.0.50727")) {
                // don't actually install it (-i) as that may mess up their existing scriptmaps for other applications. 
                // simply register it (-r) so the aspnet_regiis -sn call in the MSI won't fail.
            string aspNetRegIIS = @"%WINDIR%\microsoft.net\framework" + (Utility.Is64BitOperatingSystem ? "64" : "") + @"\v2.0.50727\aspnet_regiis.exe";
            Utility.Run(aspNetRegIIS, "-i", true, true);
            Utility.Run(aspNetRegIIS, "-ga ASPNET", true, true);
        }

        public static string GetDomainName() {
            try {
                var props = IPGlobalProperties.GetIPGlobalProperties();
                var domain = props.DomainName;
                return domain;
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public static string GetMachineName() {
            return Environment.MachineName;
        }

        public static bool IsRegisteredOnDomain() {
            return !String.IsNullOrEmpty(GetDomainName());
        }

        public static bool CanReachDomainController() {

            if (!IsRegisteredOnDomain()) {
                return false;
            } else {
                var sysdir = GetSystem32Directory();
                var dc = GetDomainName();

                var logonServer = Environment.GetEnvironmentVariable("LOGONSERVER").Replace(@"\", "");
                var machineName = GetMachineName();

                if (String.Compare(machineName, logonServer, true) == 0) {
                    // exactly the same, assume off domain and can therefore reach the domain controller (which is ourself)
                    return true;
                } else {
                    // TODO: fix this!
                    var output = Utility.Run(Toolkit.ResolveFilePath(sysdir + @"\nltest.exe", false), "/dsgetdc:" + dc, true, true);
                    if (String.IsNullOrEmpty(output)) {
                        return false;
                    } else {
                        if (output.ToLower().Contains("failed:")) {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }

        public static string InstallIIS(IWin32Window owner) {

            var splash = new frmSplash();
            try {
                if (Utility.IsWindowsXP) {
                    splash.Show("Installing IIS\nIf prompted, insert your Windows CD.\nThis process might take a few minutes, please be patient.", false, owner);
                    return installIISOnXP();
                } else if (Utility.IsWindowsVistaOrBetter) {
                    splash.Show("Installing IIS...\nThis process might take a few minutes, please be patient.", false, owner);
                    return installIISOnVista();
                    //} else if (Utility.IsWindows7) {
                    //    return "TODO: implement IIS install on Windows 7";
                }
            } finally {
                splash.Close();
            }
            return "INVALID CONFIGURATION: OS of XP or Vista not detected; cannot install IIS";
        }

        private static string installIISOnVista() {

            ProcessStartInfo psi = null;
            string tempPath = Environment.GetEnvironmentVariable("TEMP");
            string xml = null;


            // OLD WAY
            //if (Utility.IsWindowsVistaHome) {
            //    xml = getIisXmlForVistaHome();
            //} else {
            //    xml = getIisXmlForVistaBusinessOrUltimate();
            //}
            //xml = xml.Replace("__PRODUCTVERSION__", getWindowsVistaProductVersionNumber());
            //xml = xml.Replace("__ARCHITECTURE__", (Toolkit.Is64BitOperatingSystem() ? "amd64" : "x86"));

            //string xmlFilePath = tempPath + @"\unattended_iis_install.xml";
            //if (File.Exists(xmlFilePath)) {
            //    File.Delete(xmlFilePath);
            //}
            //File.WriteAllText(xmlFilePath, xml);

            //psi = new ProcessStartInfo("cmd.exe", "/c start /wait pkgmgr.exe /n:unattended_iis_install.xml /l:unattended_iis_install_log.txt");

            //psi.WorkingDirectory = tempPath;
            //psi.UseShellExecute = false;
            //psi.CreateNoWindow = true;
            //psi.WindowStyle = ProcessWindowStyle.Hidden;
            //psi.RedirectStandardError = true;
            //psi.RedirectStandardOutput = true;

            // NEW WAY
            EventLog.WriteEntry("GRIN-Global", "Windows Edition=" + Utility.WindowsVistaEdition + ", IsWindowsVistaHome=" + Utility.IsWindowsVistaHome, EventLogEntryType.Information);

            var logFilePath = Toolkit.ResolveFilePath(tempPath + @"\unattended_iis_install_log.txt", false);

            if (Utility.IsWindowsVistaHome) {

                psi = new ProcessStartInfo("cmd.exe", @"/c start /wait pkgmgr.exe /iu:IIS-WebServerRole;IIS-WebServer;IIS-CommonHttpFeatures;IIS-StaticContent;IIS-DefaultDocument;IIS-DirectoryBrowsing;IIS-HttpErrors;IIS-HttpRedirect;IIS-ApplicationDevelopment;IIS-ASPNET;IIS-NetFxExtensibility;IIS-ASP;IIS-CGI;IIS-ISAPIExtensions;IIS-ISAPIFilter;IIS-ServerSideIncludes;IIS-HealthAndDiagnostics;IIS-HttpLogging;IIS-LoggingLibraries;IIS-RequestMonitor;IIS-HttpTracing;IIS-CustomLogging;IIS-Security;IIS-BasicAuthentication;IIS-URLAuthorization;IIS-RequestFiltering;IIS-IPSecurity;IIS-Performance;IIS-HttpCompressionStatic;IIS-HttpCompressionDynamic;IIS-WebServerManagementTools;IIS-ManagementConsole;IIS-ManagementScriptingTools;IIS-ManagementService;IIS-IIS6ManagementCompatibility;IIS-Metabase;IIS-WMICompatibility;IIS-LegacyScripts;IIS-LegacySnapIn;WAS-WindowsActivationService;WAS-ProcessModel;WAS-NetFxEnvironment;WAS-ConfigurationAPI /l:unattended_iis_install_log.txt");

                psi.WorkingDirectory = tempPath;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.RedirectStandardError = true;
                psi.RedirectStandardOutput = true;


            } else {
                //xml = getIisXmlForVistaBusinessOrUltimate();
                //xml = xml.Replace("__PRODUCTVERSION__", getWindowsVistaProductVersionNumber());
                //xml = xml.Replace("__ARCHITECTURE__", (Toolkit.Is64BitOperatingSystem() ? "amd64" : "x86"));

                //string xmlFilePath = Toolkit.ResolveFilePath(tempPath + @"\unattended_iis_install.xml", false);
                //if (File.Exists(xmlFilePath)) {
                //    File.Delete(xmlFilePath);
                //}
                //File.WriteAllText(xmlFilePath, xml);
                //psi = new ProcessStartInfo("cmd.exe", @"/c start /wait pkgmgr.exe /n:unattended_iis_install.xml /l:unattended_iis_install_log.txt");

                // 2009-12-20 switched to no-xml-file version of command line as this was more reliable
                //            Note it is STILL different than the Home Premium install command line arguments.
                psi = new ProcessStartInfo("cmd.exe", @"/c start /wait pkgmgr.exe /iu:IIS-WebServerRole;IIS-WebServer;IIS-CommonHttpFeatures;IIS-StaticContent;IIS-DefaultDocument;IIS-DirectoryBrowsing;IIS-HttpErrors;IIS-HttpRedirect;IIS-ApplicationDevelopment;IIS-ASPNET;IIS-NetFxExtensibility;IIS-ASP;IIS-CGI;IIS-ISAPIExtensions;IIS-ISAPIFilter;IIS-ServerSideIncludes;IIS-HealthAndDiagnostics;IIS-HttpLogging;IIS-LoggingLibraries;IIS-RequestMonitor;IIS-HttpTracing;IIS-CustomLogging;IIS-ODBCLogging;IIS-Security;IIS-BasicAuthentication;IIS-WindowsAuthentication;IIS-DigestAuthentication;IIS-ClientCertificateMappingAuthentication;IIS-IISCertificateMappingAuthentication;IIS-URLAuthorization;IIS-RequestFiltering;IIS-IPSecurity;IIS-Performance;IIS-HttpCompressionStatic;IIS-HttpCompressionDynamic;IIS-WebServerManagementTools;IIS-ManagementConsole;IIS-ManagementScriptingTools;IIS-ManagementService;IIS-IIS6ManagementCompatibility;IIS-Metabase;IIS-WMICompatibility;IIS-LegacyScripts;IIS-LegacySnapIn;WAS-WindowsActivationService;WAS-ProcessModel;WAS-NetFxEnvironment;WAS-ConfigurationAPI /l:unattended_iis_install_log.txt");

                psi.WorkingDirectory = tempPath;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.RedirectStandardError = true;
                psi.RedirectStandardOutput = true;


            }

            EventLog.WriteEntry("GRIN-Global", "Installing IIS via: " + psi.FileName + " " + psi.Arguments, EventLogEntryType.Information);

            string output = null;

            if (Utility.Is64BitOperatingSystem && !Utility.Is64BitProcess) {

                // IIS install won't work on a 64-bit system if we shell out to pkgmgr.exe from this 32-bit process, as that will cause the 32-bit version of
                // pkgmgr.exe to be called, which will definitely fail:
                // http://social.msdn.microsoft.com/Forums/en/windowscompatibility/thread/796396fa-977d-4709-8ef3-a125c6ed68ca

                var util64 = Utility.GetPathToUtility64Exe();
                if (!File.Exists(util64)) {
                    throw new InvalidOperationException(getDisplayMember("installIISOnVista", "Could not locate ggutil64.exe to launch IIS installation."));
                } else {
                    output = Utility.Run(util64, @" /workingdir """ + psi.WorkingDirectory + @""" /launch """ + psi.FileName + @""" " + psi.Arguments, true, true);
                }

            } else {
                Process p = Process.Start(psi);

                __processError = new StringBuilder();
                __processOutput = new StringBuilder();
                p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
                p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                p.BeginErrorReadLine();
                p.BeginOutputReadLine();

                while (!p.HasExited) {
                    Thread.Sleep(100);
                    Application.DoEvents();
                }

                output = __processOutput.ToString() + " " + __processError.ToString();
            }

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

        public static  void WriteFirewallException(string fullFilePath, string displayName, bool enabled) {
            string value = fullFilePath + ":*:" + (enabled ? "Enabled" : "Disabled") + ":" + displayName;
            Toolkit.SaveRegSetting(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\AuthorizedApplications\List", fullFilePath, value, true);
        }


        private static StringBuilder __processOutput;
        private static StringBuilder __processError;

        private static string getWindowsVistaProductVersionNumber() {
            string fileName = Environment.GetEnvironmentVariable("WINDIR") + @"\regedit.exe";
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fileName);
            // see http://learn.iis.net/page.aspx/133/using-unattend-setup-to-install-iis-7/
            return fvi.ProductVersion;
//            return fvi.FileVersion;
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
         processorArchitecture=""__ARCHITECTURE__""
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
        processorArchitecture=""__ARCHITECTURE__""
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
            string windowsDrive = Environment.GetEnvironmentVariable("SYSTEMDRIVE");

            // create an inf file 
            using (StreamWriter sw = File.CreateText(fileName)) {
                sw.WriteLine(String.Format(@"[Components]
iis_common = on
iis_inetmgr = on
iis_www = on
iis_ftp = off
iis_htmla = off

[InternetServer]
PathWWWRoot={0}\Inetpub\Wwwroot
", windowsDrive));
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

        public static int IISMajorVersion {
            get {
                return Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\InetStp", "MajorVersion", 0);
            }
        }

        public static string W3SvcCommandLine {
            get {
                return Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W3SVC", "ImagePath", "");
            }
        }

        public static bool IsIISInstalled() {
            try {
                // even if IIS is installed, we have to know if the w3svc is installed as well...
                var majorVersion = IISMajorVersion;
                var w3svc = W3SvcCommandLine;
                return majorVersion >= 5 && !String.IsNullOrEmpty(w3svc);
            } catch {
                return false;
            }
        }

        private static Dictionary<string, string> __iisComponentMap;
        
        private static Dictionary<string, string> GetIISComponentMap {
            get {
                if (__iisComponentMap == null) {
                    __iisComponentMap = new Dictionary<string, string>();
                    __iisComponentMap.Add("web server", "W3SVC");
                    __iisComponentMap.Add("static content", "StaticContent");
                    __iisComponentMap.Add("default document", "DefaultDocument");
                    __iisComponentMap.Add("directory browsing", "DirectoryBrowse");
                    __iisComponentMap.Add("http errors", "HttpErrors");
                    __iisComponentMap.Add("http redirection", "HttpRedirect");
                    __iisComponentMap.Add("asp.net", "ASPNET");
                    __iisComponentMap.Add(".net extensibility", "NetFxExtensibility");
                    __iisComponentMap.Add("asp", "ASP");
                    __iisComponentMap.Add("cgi", "CGI");
                    __iisComponentMap.Add("isapi extensions", "ISAPIExtensions");
                    __iisComponentMap.Add("isapi filters", "ISAPIFilter");
                    __iisComponentMap.Add("server-side includes", "ServerSideInclude");
                    __iisComponentMap.Add("http logging", "HttpLogging");
                    __iisComponentMap.Add("logging tools", "LoggingLibraries");
                    __iisComponentMap.Add("request monitor", "RequestMonitor");
                    __iisComponentMap.Add("tracing", "HttpTracing");
                    __iisComponentMap.Add("custom logging", "CustomLogging");
                    __iisComponentMap.Add("odbc logging", "ODBCLogging");
                    __iisComponentMap.Add("basic authentication", "BasicAuthentication");
                    __iisComponentMap.Add("windows authentication", "WindowsAuthentication");
                    __iisComponentMap.Add("digest authentication", "DigestAuthentication");
                    __iisComponentMap.Add("client certificate mapping authentication", "ClientCertificateMappingAuthentication");
                    __iisComponentMap.Add("iis client certificate mapping authentication", "IISClientCertificateMappingAuthentication");
                    __iisComponentMap.Add("url authorization", "Authorization");
                    __iisComponentMap.Add("request filtering", "RequestFiltering");
                    __iisComponentMap.Add("ip and domain restrictions", "IPSecurity");
                    __iisComponentMap.Add("static content compression", "HttpCompressionStatic");
                    __iisComponentMap.Add("dynamic content compression", "HttpCompressionDynamic");
                    __iisComponentMap.Add("iis management console", "ManagementConsole");
                    __iisComponentMap.Add("iis management scripts and tools", "ManagementScriptingTools");
                    __iisComponentMap.Add("management service", "AdminService");
                    __iisComponentMap.Add("iis metabase compatibility", "Metabase");
                    __iisComponentMap.Add("iis 6 wmi compatibility", "WMICompatibility");
                    __iisComponentMap.Add("iis 6 scripting tools", "LegacyScripts");
                    __iisComponentMap.Add("iis 6 management console", "LegacySnapin");
                    __iisComponentMap.Add("ftp server", "FTPServer");
                    __iisComponentMap.Add("ftp management snap-in", "LegacySnapin");
                    __iisComponentMap.Add("process model", "ProcessModel");
                    __iisComponentMap.Add(".net environment", "NetFxEnvironment");
                    __iisComponentMap.Add("configuration apis", "WASConfigurationAPI");

                }

                return __iisComponentMap;
            }

        }


        public static bool AreAllGrinGlobalRequiredIISComponentsInstalled() {
            var checklist = new string[]{
                "Static Content",
                "Default Document",
                "Directory Browsing",
                "Http Errors",
                "Http Redirection",
                //"Application Development",
                "ASP.NET",
                ".NET Extensibility",
                "ASP",
                "CGI",
                "ISAPI Extensions",
                "ISAPI Filters",
                "Server-Side Includes",
                //"Health And Diagnostics",
                "Http Logging",
                "Logging Tools",
                "Request Monitor",
                "Tracing",
                "Custom Logging",
                //"Security",
                "Basic Authentication",
                "URL Authorization",
                "Request Filtering",
                "IP and Domain Restrictions",
                //"Performance",
                "Static Content Compression",
                "Dynamic Content Compression",
                //"Web Server Management Tools",
                "IIS Management Console",
                "IIS Management Scripts and Tools",
                "Management Service",
                //"IIS6 Management Compatibility",
                "IIS Metabase Compatibility",
                "IIS 6 WMI Compatibility",
                "IIS 6 Scripting Tools",
                "IIS 6 Management Console",
                //"Windows Activation Service",
                "Process Model",
                ".NET Environment",
                "Configuration APIs"
            };

            return DetectInstalledIISComponents(new List<string>(checklist)).Count == checklist.Length;

            /*
            CommonHttpFeatures;
             * IIS-StaticContent;
             * IIS-DefaultDocument;
             * IIS-DirectoryBrowsing;
             * IIS-HttpErrors;
             * IIS-HttpRedirect;
             * IIS-ApplicationDevelopment;
             * IIS-ASPNET;
             * IIS-NetFxExtensibility;
             * IIS-ASP;
             * IIS-CGI;
             * IIS-ISAPIExtensions;
             * IIS-ISAPIFilter;
             * IIS-ServerSideIncludes;
             * IIS-HealthAndDiagnostics;
             * IIS-HttpLogging;
             * IIS-LoggingLibraries;
             * IIS-RequestMonitor;
             * IIS-HttpTracing;
             * IIS-CustomLogging;
             * IIS-Security;
             * IIS-BasicAuthentication;
             * IIS-URLAuthorization;
             * IIS-RequestFiltering;
             * IIS-IPSecurity;
             * IIS-Performance;
             * IIS-HttpCompressionStatic;
             * IIS-HttpCompressionDynamic;
             * IIS-WebServerManagementTools;
             * IIS-ManagementConsole;
             * IIS-ManagementScriptingTools;
             * IIS-ManagementService;
             * IIS-IIS6ManagementCompatibility;
             * IIS-Metabase;
             * IIS-WMICompatibility;
             * IIS-LegacyScripts;
             * IIS-LegacySnapIn;
             * WAS-WindowsActivationService;
             * WAS-ProcessModel;
             * WAS-NetFxEnvironment;
             * WAS-ConfigurationAPI        
             */
        }

        /// <summary>
        /// Given a list of the display names of IIS components to check for, returns the list of components that are currently installed.  null or a list of 0 as a parameter returns all installed components.
        /// </summary>
        /// <param name="componentDisplayNames"></param>
        /// <returns></returns>
        public static List<string> DetectInstalledIISComponents(List<string> componentDisplayNames) {

            // see the following link for the component -> registry key mapping:
            // http://learn.iis.net/page.aspx/135/discover-installed-components/
            //
            /* Root registry key path:  HKEY_LOCAL_MACHINE\Software\Microsoft\InetStp\Components\  
             * 
             * Display Name                                     Registry Key
             * ===============================================  =========================
             * Web Server                                       W3SVC
             * 
             *  ---- Common HTTP Features ----
             * Static Content                                   StaticContent
             * Default Document                                 DefaultDocument
             * Directory Browsing                               DirectoryBrowse
             * HTTP Errors                                      HttpErrors
             * HTTP Redirection                                 HttpRedirect
             * 
             * ---- Application Development Features ----
             * ASP.NET                                          ASPNET
             * .NET Extensibility                               NetFxExtensibility
             * ASP                                              ASP
             * CGI                                              CGI
             * ISAPI Extensions                                 ISAPIExtensions
             * ISAPI Filters                                    ISAPIFilter
             * Server-Side Includes                             ServerSideInclude
             * 
             * ---- Health and Diagnostics ----
             * HTTP Logging                                     HttpLogging
             * Logging Tools                                    LoggingLibraries
             * Request Monitor                                  RequestMonitor
             * Tracing                                          HttpTracing
             * Custom Logging                                   CustomLogging
             * ODBC Logging                                     ODBCLogging
             * 
             * ---- Security ----
             * Basic Authentication                             BasicAuthentication
             * Windows Authentication                           WindowsAuthentication
             * Digest Authentication                            DigestAuthentication
             * Client Certificate Mapping Authentication        ClientCertificateMappingAuthentication
             * IIS Client Certificate Mapping Authentication    IISClientCertificateMappingAuthentication
             * URL Authorization                                Authorization
             * Request Filtering                                RequestFiltering
             * IP and Domain Restrictions                       IPSecurity
             * 
             * ---- Performance Features ----
             * Static Content Compression                       HttpCompressionStatic
             * Dynamic Content Compression                      HttpCompressionDynamic
             * 
             * ---- Management Tools ----
             * IIS Management Console                           ManagementConsole
             * IIS Management Scripts and Tools                 ManagementScriptingTools
             * Management Service                               AdminService
             * ---- IIS 6 Management Compatibility ----
             * IIS Metabase Compatibility                       Metabase
             * IIS 6 WMI Compatibility                          WMICompatibility
             * IIS 6 Scripting Tools                            LegacyScripts
             * IIS 6 Management Console                         LegacySnapin
             * 
             * ---- FTP Publishing Service ----
             * FTP Server                                       FTPServer
             * FTP Management snap-in                           LegacySnapin
             * 
             * ---- Windows Process Activation Service ----
             * Process Model                                    ProcessModel
             * .NET Environment                                 NetFxEnvironment
             * Configuration APIs                               WASConfigurationAPI
             */


            if (componentDisplayNames == null || componentDisplayNames.Count == 0) {
                // default to all if they didn't specify any
                componentDisplayNames = GetIISComponentMap.Keys.ToList();
            }

            var output = new List<string>();
            var registryPath = @"HKEY_LOCAL_MACHINE\Software\Microsoft\InetStp\Components\";

            foreach (var c in componentDisplayNames) {
                string keyName = null;
                if (GetIISComponentMap.TryGetValue(("" + c).ToLower(), out keyName)) {
                    var installed = Toolkit.GetRegSetting(registryPath, keyName, 0);
                    if (installed == 1) {
                        output.Add(c);
                    }
                }
            }

            return output;

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
                ret = Utility.GetGrinGlobalApplicationInstallPath(appName, null);
//                ret = Toolkit.GetRegSetting(RootRegistryKey + @"\" + appName, "InstallPath", "") as string;
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


        public static string GetSuperUserPassword(InstallContext ctx, IDictionary savedState) {
            string ret = ctx.Parameters["password"];
            if (String.IsNullOrEmpty(ret)) {
                ret = ctx.Parameters["PASSWORD"];
                if (String.IsNullOrEmpty(ret) && savedState != null) {
                    ret = savedState["PASSWORD"] as string;
                }
            }

            if (!String.IsNullOrEmpty(ret)) {
                // might need to be decrypted!
                try {
                    ret = Crypto.DecryptText(ret);
                } catch {
                    // decryption failed, means it wasn't encrypted in the first place :)
                }
            }




            return ret;
        }

        public static string EncryptText(string plainText) {
            return Crypto.EncryptText(plainText);
        }

        public static string DecryptText(string cipherText) {
            return Crypto.DecryptText(cipherText);
        }

        public static bool GetUseWindowsAuthentication(InstallContext ctx, IDictionary savedState) {
            string ret = ctx.Parameters["usewindowsauth"];
            if (String.IsNullOrEmpty(ret)) {
                ret = ctx.Parameters["USEWINDOWSAUTH"];
                if (String.IsNullOrEmpty(ret) && savedState != null) {
                    ret = savedState["USEWINDOWSAUTH"] as string;
                }
            }
            return ret == "1" || ret == "TRUE" || ret == "True" || ret == "true" || ret == "Y" || ret == "y";
        }


        public static string GetParameter(string parameterName, string appName, InstallContext ctx, IDictionary state) {

            string ret = null;

            if (ctx != null) {
                ret = ctx.Parameters[parameterName];
                if (String.IsNullOrEmpty(ret)) {
                    ret = ctx.Parameters[parameterName.ToUpper()];
                }
            }


            if (String.IsNullOrEmpty(ret) && state != null) {
                ret = state[parameterName] as string;
                if (String.IsNullOrEmpty(ret)) {
                    ret = state[parameterName.ToUpper()] as string;
                }
            }

            if (String.IsNullOrEmpty(ret)) {
                ret = Toolkit.GetRegSetting(RootRegistryKey + @"\" + appName, parameterName, "") as string;
            }

            if (ctx != null) {
                ctx.Parameters[parameterName] = ret;
            }

            return ret;
        }

        public static string GetSourceDirectory(InstallContext ctx, IDictionary state, string appName) {

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
                ret = Toolkit.GetRegSetting(RootRegistryKey + @"\" + appName, "SourceDir", "") as string;
            }

            if (ctx != null) {
                ctx.Parameters["src"] = ret;
            }
            return ret;
        }


        public static void ExtractUtility64File(string targetDir, string gguacPath) {
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


        public static string GetProductVersion(string fileName) {
            Type type = Type.GetTypeFromProgID("WindowsInstaller.Installer");
            WindowsInstaller.Installer installer = null;
            WindowsInstaller.Database db = null;
            WindowsInstaller.View dv = null;
            try {
                installer = (WindowsInstaller.Installer)Activator.CreateInstance(type);
                db = installer.OpenDatabase(fileName, 0);
                dv = db.OpenView("SELECT `Value` FROM `Property` WHERE `Property`='ProductVersion'");
                WindowsInstaller.Record record = null;
                dv.Execute(record);
                record = dv.Fetch();
                string productVersion = record.get_StringData(1).ToString();
                return productVersion;
            } finally {

                // since the installer objects are COM objects,
                // we need to be extra careful to clean up after ourselves

                if (db != null) {
                    Marshal.FinalReleaseComObject(db);
                    db = null;
                }
                if (dv != null) {
                    dv.Close();
                    Marshal.FinalReleaseComObject(dv);
                    dv = null;
                }

                // DO NOT REMOVE THIS!!!
                // the OpenDatabase call leaves a file descriptor open against the .msi file.
                // The only way I could find to clean this up properly was to force garbage collection
                // since there is no Close() or Dispose() or CloseDatabase() or whatever in the installer API.
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
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

        public static int ToInt32(string value, int defaultValue) {
            int ret;
            if (int.TryParse(value, out ret)) {
                return ret;
            } else {
                return defaultValue;
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

        public static string GetApplicationVersion(string appNameToDisplay) {
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
            return appNameToDisplay + " v " + an.Version.ToString();
        }

        public static SortedDictionary<string, FileDownloadInfo> ListInstalledApplications() {
            var installedApps = new SortedDictionary<string, FileDownloadInfo>();
            fillInstalledApps(installedApps, true, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            fillInstalledApps(installedApps, true, @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            return installedApps;
        }

        private static void fillInstalledApps(SortedDictionary<string, FileDownloadInfo> apps, bool allUsers, string regPath) {
            using (RegistryKey rk = (allUsers ? Registry.LocalMachine.OpenSubKey(regPath) : Registry.CurrentUser.OpenSubKey(regPath))) {
                if (rk != null){
                    string[] guids = rk.GetSubKeyNames();
                    foreach (string guid in guids) {
                        if (!String.IsNullOrEmpty(guid)) {
                            using (var subkey = rk.OpenSubKey(guid)) {
                                if (subkey != null) {
                                    var displayName = subkey.GetValue("DisplayName", "") as string;
                                    var displayVersion = subkey.GetValue("DisplayVersion", "") as string;
                                    var uninstallString = subkey.GetValue("UninstallString", "") as string;
                                    if (!String.IsNullOrEmpty(displayName) && !apps.ContainsKey(displayName)) {
                                        Guid productGuid = Guid.Empty;
                                        if (guid.StartsWith("{") && guid.Contains("-") && guid.Length >= 38 && guid[37] == '}') {
                                            productGuid = new Guid(guid.Substring(0, 38));
                                        }
                                        apps.Add(displayName, new FileDownloadInfo { DisplayName = displayName, InstalledVersion = displayVersion, UninstallString = uninstallString, ProductGuid = productGuid });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static bool IsServiceInstalled(string serviceName) {
            return Toolkit.IsServiceInstalled(serviceName);
        }

        public static string GetInstalledVersion(string installedAppName) {
            var sd = ListInstalledApplications();
            FileDownloadInfo fdi = null;
            if (sd.TryGetValue(installedAppName, out fdi)) {
                return installedAppName + " v " + fdi.InstalledVersion;
            } else {
                return installedAppName + " v ???";
            }
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
            if (Directory.Exists(directoryName)) {
                foreach (string dir in Directory.GetDirectories(directoryName)) {
                    Directory.Delete(dir, true);
                }
                foreach (string fil in Directory.GetFiles(directoryName)) {
                    File.Delete(fil);
                }
            }
        }



        public static bool IsAnyDatabaseEngineInstalled {
            get {
                return IsMySQLInstalled || IsSqlServer2008Installed || IsOracleExpressInstalled || IsPostgreSQLInstalled;
            }
        }

        public static bool IsMySQLToolsInstalled {
            get {
                try {
                    var subkeys = Toolkit.GetRegSubKeys(@"HKEY_LOCAL_MACHINE\SOFTWARE\MySQL AB");
                    if (subkeys == null || subkeys.Length == 0) {
                        return false;
                    } else {
                        foreach (var sk in subkeys) {
                            if (sk.ToLower().StartsWith("mysql tools")) {
                                var toolsDir = (string)Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\MySQL AB\" + sk, "Location", "");
                                if (String.IsNullOrEmpty(toolsDir)) {
                                    return false;
                                } else {
                                    return File.Exists((toolsDir + @"\MySQLAdministrator.exe").Replace(@"\\", @"\"));
                                }
                            }
                        }
                        return false;
                    }
                } catch {
                    return false;
                }
            }
        }

        public static bool IsMySQLInstalled {
            get {
                try {
                    var subkeys = Toolkit.GetRegSubKeys(@"HKEY_LOCAL_MACHINE\SOFTWARE\MySQL AB");
                    if (subkeys == null || subkeys.Length == 0) {
                        return false;
                    } else {
                        foreach (var sk in subkeys) {
                            if (sk.ToLower().StartsWith("mysql server")) {
                                string baseDir = (string)Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\MySQL AB\" + sk, "Location", "");
                                if (String.IsNullOrEmpty(baseDir)) {
                                    return false;
                                } else {
                                    return File.Exists((baseDir + @"\bin\mysqld.exe").Replace(@"\\", @"\"));
                                }
                            }
                        }
                        return false;
                    }
                } catch {
                    return false;
                }
            }
        }

        //public static bool IsSqlServerExpressToolsInstalled {
        //    get {
        //        string val = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\100\Tools\Setup", "FeatureList", "") as string;
        //        return (String.IsNullOrEmpty(val) ? false : val.ToLower().Contains("ssms"));
        //    }
        //}

        public static bool IsSqlServer2008Installed {
            get {
                try {
                    var ver = (string)Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\100", "VerSpecificRootDir", "");
                    if (!String.IsNullOrEmpty(ver)) {
                        if (ver.Contains(@"\100\")) {
                            // is sql server 2008. see if SQLExpress instance is installed.
                            return true;
                            //var instanceNames = (string[])Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server", "InstalledInstances", new string[]{});
                            //if (instanceNames != null && instanceNames.Length > 0){
                            //    foreach (var s in instanceNames) {
                            //        if (s.ToLower().Contains("sqlexpress")) {
                            //            return true;
                            //        }
                            //    }
                            //}
                        }
                    }



                    return false;

                    //string curVer = (string)Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\" + DatabaseInstanceName + @"\MSSQLServer\CurrentVersion", "CurrentVersion", "");
                    //return !String.IsNullOrEmpty(curVer) && curVer.StartsWith("10.");
                } catch {
                    return false;
                }
            }
        }
        public static bool IsOracleExpressToolsInstalled {
            get {
                return false;
            }
        }

        public static bool IsOracleExpressInstalled {
            get {
                try {
                    string curVer = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\ORACLE\KEY_XE", "VERSION", "") as string;
                    return !String.IsNullOrEmpty(curVer);
                } catch {
                    return false;
                }
            }
        }

        public static bool IsPostgreSQLToolsInstalled {
            get {
                return false;
            }
        }

        public static bool IsPostgreSQLInstalled {
            get {
                try {
                    //string productCode = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\PostgreSQL\Services\pgsql-8.3", "Product Code", "") as string;
                    //return !String.IsNullOrEmpty(productCode);
                    var subkeys = Toolkit.GetRegSubKeys(@"HKEY_LOCAL_MACHINE\SOFTWARE\PostgreSQL\Services");
                    if (subkeys.Length > 0) {
                        return true;
                    } else {
                        return false;
                    }
                } catch {
                    return false;
                }
            }
        }


        public static string GetDatabaseInstanceName(InstallContext ctx, IDictionary state, string defaultInstanceName) {
            string ret = null;

            StringBuilder sb = new StringBuilder();

            if (ctx != null) {

                if (ctx.Parameters["action"] == "uninstall") {
                    // default to using registry entries
                    if (ret == null) {
                        ret = Toolkit.GetRegSetting(RootRegistryKey, "DatabaseInstanceName", (string)null) as string;
                        sb.AppendLine("Toolkit.GetRegSetting(" + RootRegistryKey + "DatabaseInstanceName)=" + ret);
                    }
                }


                if (ret == null) {
                    if (ctx.Parameters.ContainsKey("INSTANCENAME")) {
                        ret = ctx.Parameters["INSTANCENAME"];
                    //} else {
                    //    MessageBox.Show("No 'INSTANCENAME' parameter found in context");
                    }
                    sb.AppendLine("\r\nContext.Parameters['INSTANCENAME']=" + ret);
                }
            }

            if (ret == null && state != null) {
                if (state.Contains("INSTANCENAME")) {
                    ret = state["INSTANCENAME"] as string;
                }
                sb.AppendLine("state['INSTANCENAME']=" + ret);
            }

            if (ret == null) {
                ret = Toolkit.GetRegSetting(RootRegistryKey, "DatabaseInstanceName", (string)null) as string;
                sb.AppendLine("Toolkit.GetRegSetting(" + RootRegistryKey + "DatabaseInstanceName)=" + ret);
            }

            if (ret == null) {
                ret = defaultInstanceName;
                if (ctx != null) {
                    ctx.Parameters["instancename"] = ret;
                }
            }

            return ret;

        }

        public static string GetDatabaseServerName(InstallContext ctx, IDictionary state, string defaultServerName) {
            string ret = null;

            StringBuilder sb = new StringBuilder();

            if (ctx != null) {

                if (ctx.Parameters["action"] == "uninstall") {
                    // default to using registry entries
                    if (String.IsNullOrEmpty(ret)) {
                        ret = Toolkit.GetRegSetting(RootRegistryKey, "DatabaseServerName", "") as string;
                        sb.AppendLine("Toolkit.GetRegSetting(" + RootRegistryKey + "DatabaseServerName)=" + ret);
                    }
                }


                if (String.IsNullOrEmpty(ret)) {
                    ret = ctx.Parameters["SERVERNAME"];
                    sb.AppendLine("\r\nContext.Parameters['SERVERNAME']=" + ret);
                }
            }

            if (String.IsNullOrEmpty(ret) && state != null) {
                ret = state["SERVERNAME"] as string;
                sb.AppendLine("state['SERVERNAME']=" + ret);
            }

            if (String.IsNullOrEmpty(ret)) {
                ret = Toolkit.GetRegSetting(RootRegistryKey, "DatabaseServerName", "") as string;
                sb.AppendLine("Toolkit.GetRegSetting(" + RootRegistryKey + "DatabaseServerName)=" + ret);
            }

            if (String.IsNullOrEmpty(ret)) {
                ret = defaultServerName;
                if (ctx != null) {
                    ctx.Parameters["servername"] = ret;
                }
            }

            return ret;

        }

        public static string GetDatabaseLocation(InstallContext ctx, IDictionary state, string defaultLocation) {
            string ret = null;

            StringBuilder sb = new StringBuilder();

            if (ctx != null) {

                if (ctx.Parameters["action"] == "uninstall") {
                    // default to using registry entries
                    if (String.IsNullOrEmpty(ret)) {
                        ret = Toolkit.GetRegSetting(RootRegistryKey, "DatabaseLocation", "") as string;
                        sb.AppendLine("Toolkit.GetRegSetting(" + RootRegistryKey + "DatabaseLocation)=" + ret);
                    }
                }


                if (String.IsNullOrEmpty(ret)) {
                    ret = ctx.Parameters["LOCATION"];
                    sb.AppendLine("\r\nContext.Parameters['LOCATION']=" + ret);
                }
            }

            if (String.IsNullOrEmpty(ret) && state != null) {
                ret = state["LOCATION"] as string;
                sb.AppendLine("state['LOCATION']=" + ret);
            }

            if (String.IsNullOrEmpty(ret)) {
                ret = Toolkit.GetRegSetting(RootRegistryKey, "DatabaseLocation", "") as string;
                sb.AppendLine("Toolkit.GetRegSetting(" + RootRegistryKey + "DatabaseLocation)=" + ret);
            }

            if (String.IsNullOrEmpty(ret)) {
                ret = defaultLocation;
                if (ctx != null) {
                    ctx.Parameters["location"] = ret;
                }
            }

            return ret;

        }

        public static int GetDatabasePort(InstallContext ctx, IDictionary state, int defaultPort) {
            int ret = 0;

            StringBuilder sb = new StringBuilder();

            if (ctx != null) {

                if (ctx.Parameters["action"] == "uninstall") {
                    // default to using registry entries
                    if (ret < 1) {
                        ret = Toolkit.GetRegSetting(RootRegistryKey, "DatabasePort", 0);
                        sb.AppendLine("Toolkit.GetRegSetting(" + RootRegistryKey + "DatabasePort)=" + ret);
                    }
                }


                if (ret < 1) {
                    ret = Toolkit.ToInt32(ctx.Parameters["PORT"], 0);
                    sb.AppendLine("\r\nContext.Parameters['PORT']=" + ret);
                }
            }

            if (ret < 1 && state != null) {
                ret = Toolkit.ToInt32(state["PORT"], 0);
                sb.AppendLine("state['PORT']=" + ret);
            }

            if (ret < 1) {
                ret = Toolkit.GetRegSetting(RootRegistryKey, "DatabasePort", 0);
                sb.AppendLine("Toolkit.GetRegSetting(" + RootRegistryKey + "DatabasePort)=" + ret);
            }

            if (ret < 1) {
                ret = defaultPort;
                if (ctx != null) {
                    ctx.Parameters["port"] = ret.ToString();
                }
            }

            return ret;

        }

        public static string GetTempDirectory(int minSizeInMB) {

            return Toolkit.GetTempDirectory(minSizeInMB);

        }

        public static bool GetUseRegSettings(InstallContext ctx, IDictionary state) {
            string ret = null;

            if (ctx != null) {
                // default to using context first
                ret = ctx.Parameters["useregsettings"];
                if (String.IsNullOrEmpty(ret)) {
                    ret = ctx.Parameters["USEREGSETTINGS"];
                }
            }

            if (String.IsNullOrEmpty(ret) && state != null) {
                ret = state["useregsettings"] as string;
                if (String.IsNullOrEmpty(ret)) {
                    ret = state["USEREGSETTINGS"] as string;
                }
            }

            return ret == "TRUE";

        }

        public static bool GetAutoLogin(InstallContext ctx, IDictionary state) {
            string ret = null;

            if (ctx != null) {
                // default to using context first
                ret = ctx.Parameters["autologin"];
                if (String.IsNullOrEmpty(ret)) {
                    ret = ctx.Parameters["AUTOLOGIN"];
                }
            }

            if (String.IsNullOrEmpty(ret) && state != null) {
                ret = state["autologin"] as string;
                if (String.IsNullOrEmpty(ret)) {
                    ret = state["AUTOLOGIN"] as string;
                }
            }

            return ("" + ret).ToUpper() == "TRUE";

        }

        public static bool GetDatabaseWindowsAuth(InstallContext ctx, IDictionary state) {
            string ret = null;

            if (ctx != null) {
                if (ctx.Parameters["action"] == "uninstall") {
                    // default to using registry entries first
                    ret = Toolkit.GetRegSetting(RootRegistryKey, "DatabaseWindowsAuth", "") as string;
                } else {
                    // default to using context first
                    ret = ctx.Parameters["WINDOWSAUTH"];
                }
            }

            if (String.IsNullOrEmpty(ret) && state != null) {
                ret = state["WINDOWSAUTH"] as string;
            }

            if (String.IsNullOrEmpty(ret)) {
                ret = Toolkit.GetRegSetting(RootRegistryKey, "DatabaseWindowsAuth", "") as string;
            }

            return ret == "1";

        }


        public static string GetDatabaseConnectionString(InstallContext ctx, IDictionary state, string userName, string password) {
            var ret = "" + Toolkit.GetRegSetting(RootRegistryKey, "DatabaseConnectionString", "") as string;
            ret = ret.Replace("__USERID__", userName).Replace("__PASSWORD__", password);
            return ret;
        }

        public static void AddToRunOnce(string name, string fullFilePath) {
            Toolkit.SaveRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce", name, fullFilePath, true);
        }

        //public static bool IsProgramInstalled(string displayName) {
        //    using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall")) {
        //        foreach (string skName in rk.GetSubKeyNames()) {
        //            using (RegistryKey sk = rk.OpenSubKey(skName)) {
        //                string displayNameInRegistry = sk.GetValue("DisplayName") as string;
        //                if (!String.IsNullOrEmpty(displayNameInRegistry)) {
        //                    if (displayNameInRegistry.ToLower().Trim() == displayName.ToLower().Trim()) {
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}

        public static string GetUninstallCommand(string displayName, Dictionary<string, string> requiredKeyMatches) {
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall")) {
                if (rk != null){
                    foreach (string skName in rk.GetSubKeyNames()) {
                        using (RegistryKey sk = rk.OpenSubKey(skName)) {
                            if (sk != null){
                                string displayNameInRegistry = sk.GetValue("DisplayName") as string;
                                if (!String.IsNullOrEmpty(displayNameInRegistry)) {

                                    if (displayNameInRegistry.ToLower().Trim() == displayName.ToLower().Trim()) {

                                        string uninstallString = sk.GetValue("UninstallString") as string;
                                        if (!String.IsNullOrEmpty(uninstallString)) {

                                            if (uninstallString.ToLower().StartsWith("msiexec.exe")) {

                                                if (requiredKeyMatches != null) {
                                                    foreach (string key in requiredKeyMatches.Keys) {
                                                        string val = sk.GetValue(key) as string;
                                                        if (val != requiredKeyMatches[key]) {
                                                            // we found the key, but the value associated with it is different than what we require.
                                                            return null;
                                                        }
                                                    }
                                                }

                                                // for whatever reason, the uninstall path reports the /I option as the proper one for uninstallation
                                                // as it provides the user the option to repair or uninstall.
                                                // we not only want to uninstall but we want passive as well (no prompts).
                                                uninstallString = uninstallString.Replace("/I{", "/passive /x{");
                                                return uninstallString;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }


        public static string GetDatabaseEngine(InstallContext ctx, IDictionary state, bool throwExceptionIfNotFound, string defaultEngineName) {
            string ret = null;

            StringBuilder sb = new StringBuilder();

            if (ctx != null) {

                if (ctx.Parameters["action"] == "uninstall") {
                    // default to using registry entries
                    if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                        ret = Toolkit.GetRegSetting(Utility.RootRegistryKey, "DatabaseEngine", "") as string;
                        sb.AppendLine("Toolkit.GetRegSetting(" + Utility.RootRegistryKey + "DatabaseEngine)=" + ret);
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
                ret = Toolkit.GetRegSetting(Utility.RootRegistryKey, "DatabaseEngine", "") as string;
                sb.AppendLine("Toolkit.GetRegSetting(" + Utility.RootRegistryKey + "DatabaseEngine)=" + ret);
            }

            // HACK: always make it sqlserver until we have other options available...
            if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                ret = defaultEngineName;
            }

            if (!DatabaseEngineUtil.IsValidEngine(ret)) {
                if (throwExceptionIfNotFound) {
                    throw new InvalidOperationException(getDisplayMember("GetDatabaseEngine", @"GetDatabaseEngine could not resolve a valid engine name.  Inspected locations/values: {0}", sb.ToString()));
                }
            } else {
                if (ctx != null) {
                    ctx.Parameters["engine"] = ret;
                }
            }

            return ret;

        }

        public static string GetPathToUtility64Exe() {
            return Toolkit.GetPathToUtility64Exe();
        }

        public static string GetPathToUACExe() {
            return Toolkit.GetPathToUACExe();
        }

        public static string GetSystemInfo() {
            var sb = new StringBuilder();
            sb.AppendLine().AppendLine("General Information").AppendLine("--------------------------------------");

            sb.Append("Operating System Version: ");
            try {
                sb.AppendLine(Utility.OperatingSystemVersion);
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Operating System Edition Name: ");
            try {
                sb.AppendLine(Utility.WindowsVistaEdition);
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Operating System Is 64-bit: ");
            try {
                sb.AppendLine(Utility.Is64BitOperatingSystem.ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Current Process Is 64-bit: ");
            try {
                sb.AppendLine(Utility.Is64BitProcess.ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Is Windows 7: ");
            try {
                sb.AppendLine(Utility.IsWindows7.ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Is Windows Vista: ");
            try {
                sb.AppendLine(Utility.IsWindowsVista.ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Is Windows XP: ");
            try {
                sb.AppendLine(Utility.IsWindowsXP.ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Domain Name: ");
            try {
                sb.AppendLine(Utility.GetDomainName());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Machine Name: ");
            try {
                sb.AppendLine(Utility.GetMachineName());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Is Registered on an Active Directory Domain? ");
            try {
                sb.AppendLine(Utility.IsRegisteredOnDomain().ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Can Reach Domain Controller? ");
            try {
                sb.AppendLine(Utility.CanReachDomainController().ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Temporary Directory: ");
            try {
                sb.AppendLine(Utility.GetTempDirectory(2000));
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("System32 Directory: ");
            try {
                sb.AppendLine(Utility.GetSystem32Directory());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Path to ggutil64.exe: ");
            try {
                sb.AppendLine(Utility.GetPathToUtility64Exe());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Path to gguac.exe: ");
            try {
                sb.AppendLine(Utility.GetPathToUACExe());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.AppendLine().AppendLine("Database Information").AppendLine("--------------------------------------");

            sb.Append("Is Any Database Engine Installed? ");
            try {
                sb.AppendLine(Utility.IsAnyDatabaseEngineInstalled.ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Is SQL Server Express 2008 Installed? ");
            try {
                sb.AppendLine(Utility.IsSqlServer2008Installed.ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Is MySQL Installed? ");
            try {
                sb.AppendLine(Utility.IsMySQLInstalled.ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Is Oracle XE Installed? ");
            try {
                sb.AppendLine(Utility.IsOracleExpressInstalled.ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Is PostgreSQL Installed? ");
            try {
                sb.AppendLine(Utility.IsPostgreSQLInstalled.ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Selected Database Engine Name: ");
            try {
                sb.AppendLine(Utility.GetDatabaseEngine(null, null, false, ""));
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Selected Database Instance Name: ");
            try {
                sb.AppendLine(Utility.GetDatabaseInstanceName(null, null, ""));
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Selected Database Connection String: ");
            try {
                sb.AppendLine(Utility.GetDatabaseConnectionString(null, null, "??", "??"));
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Is Database Login Using Windows Authentication? ");
            try {
                sb.AppendLine(Utility.GetDatabaseWindowsAuth(null, null).ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }


            sb.AppendLine().AppendLine("IIS Information").AppendLine("--------------------------------------");
            sb.Append("Is IIS Installed? ");
            try {
                sb.AppendLine(Utility.IsIISInstalled().ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Installed IIS Components: ");
            try {
                sb.AppendLine(String.Join(", ", Utility.DetectInstalledIISComponents(null).ToArray()));
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("IIS Major Version: ");
            try {
                sb.AppendLine(Utility.IISMajorVersion.ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("W3SVC Command Line: ");
            try {
                sb.AppendLine(Utility.W3SvcCommandLine);
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Is ASP.NET 2.0 Registered? ");
            try {
                sb.AppendLine(Utility.IsAspNet20Registered().ToString());
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
            }

            sb.Append("Physical Path to http://localhost/gringlobal: ");
            try {
                sb.AppendLine(Utility.GetIISPhysicalPath("gringlobal"));
            } catch (Exception ex) {
                if (Utility.IsIISInstalled()) {
                    sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
                } else {
                    sb.AppendLine("n/a, IIS is not installed");
                }
            }
            sb.Append("Process listening on port 80: ");
            try {
                sb.AppendLine(Utility.GetProcessNameByPort(80));
            } catch (Exception ex) {
                sb.AppendLine("********* Error: " + ex.Message);
            }

            sb.AppendLine().AppendLine("GRIN-Global Installed Applications").AppendLine("--------------------------------------");
            try {
                var apps = ListInstalledGrinGlobalApps();
                foreach (var key in apps.Keys) {
                    try {
                        var fdi = apps[key];
                        sb.Append(fdi.DisplayName).Append(" v ").Append(fdi.InstalledVersion);
                        sb.AppendLine(" : " + GetGrinGlobalApplicationInstallPath(key.Replace("GRIN-Global ", ""), "(could not determine)"));
                        sb.AppendLine();

                    } catch (Exception ex) {
                        sb.AppendLine("********* Error retrieving property, bailing out: " + ex.Message);
                    }
                }
            } catch (Exception ex) {
                sb.AppendLine("********* Error retrieving list of GRIN-Global applications, bailing out: " + ex.Message);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Returns true if given urlOrPath is a UNC path (starts with \\), a drive letter pattern (e.g. starts with "A:\"), a relative file path (\, .\, ~\), or a file url (starts with "file://").  Returns false otherwise.
        /// </summary>
        /// <param name="urlOrPath"></param>
        /// <returns></returns>
        public static bool IsWindowsFilePath(string urlOrPath) {
            var reLocalDrive = new Regex(@"[a-zA-Z]:\\", RegexOptions.None);
            if ((reLocalDrive.Match(urlOrPath).Success)
                || urlOrPath.StartsWith(@"\\")
                || urlOrPath.StartsWith(@"\")
                || urlOrPath.StartsWith(@".\")
                || urlOrPath.StartsWith(@"~\")
                || urlOrPath.ToLower().StartsWith(@"file://")) {
                // is a local drive, UNC path, or file-based url
                return true;
            } else {
                return false;
            }
        }

        public static void SaveDatabaseEngine(string engineName) {
            // remember which db engine we're using
            Toolkit.SaveRegSetting(Utility.RootRegistryKey, "DatabaseEngine", ("" + engineName).ToLower(), false);
        }

        public static void SaveDatabaseServer(string serverName) {
            // remember the server name
            Toolkit.SaveRegSetting(Utility.RootRegistryKey, "DatabaseServerName", ("" + serverName), false);
        }

        public static void SaveDatabaseInstanceName(string instanceName) {
            // remember the instance name
            Toolkit.SaveRegSetting(Utility.RootRegistryKey, "DatabaseInstanceName", instanceName, false);
        }

        public static void SaveDatabasePort(int port) {
            // remember the port number
            Toolkit.SaveRegSetting(Utility.RootRegistryKey, "DatabasePort", port, false);
        }

        public static void SaveDatabaseLocation(string location) {
            // remember the location (local, remote, unknown)
            Toolkit.SaveRegSetting(Utility.RootRegistryKey, "DatabaseLocation", location, false);
        }

        public static void SaveDatabaseWindowsAuth(bool usingWindowsAuthentication) {
            // remember if we're using windows auth
            Toolkit.SaveRegSetting(Utility.RootRegistryKey, "DatabaseWindowsAuth", usingWindowsAuthentication ? "1" : "0", false);
        }

        public static void SaveDatabaseSettings(string engineName, bool usingWindowsAuthentication, string serverName, int port, string instanceName, string connectionString, string location) {
            // remember which db engine we're using
            SaveDatabaseEngine(engineName);

            // remember if we're using windows auth
            SaveDatabaseWindowsAuth(usingWindowsAuthentication);

            // remember connectionstring
            Toolkit.SaveRegSetting(Utility.RootRegistryKey, "DatabaseConnectionString", connectionString, false);

            // remember the instance name
            SaveDatabaseInstanceName(instanceName);

            // remember the server name
            SaveDatabaseServer(serverName);

            // remember the port number
            SaveDatabasePort(port);

            // remember the server location
            SaveDatabaseLocation(location);

        }


        /// <summary>
        /// Given two versions (in format of 1.234.555.080), returns true if version A is greater than version B
        /// </summary>
        /// <param name="versionA"></param>
        /// <param name="versionB"></param>
        /// <returns></returns>
        public static bool IsVersionAGreater(string versionA, string versionB) {
            return Toolkit.IsVersionAGreater(versionA, versionB);
        }

        /// <summary>
        /// Returns programmatic codes for all supported providers. e.g.: "postgresql" is one of the values returned.
        /// </summary>
        /// <returns></returns>
        public static string[] ListProviderCodes() {
            return new string[] { "sqlserver", "mysql", "postgresql", "oracle" };
        }

        /// <summary>
        /// Returns friendly names for all supported providers.  e.g.: "MySQL 5.1 or later *" is one of the values returned.
        /// </summary>
        /// <returns></returns>
        public static string[] ListProviderNames() {
            return new string[]{
                "SQL Server 2008 or later",
                "MySQL 5.1 or later *",
                "PostgreSQL 8.3 or later *",
                "Oracle XE 10g or later *"
            };
        }

        /// <summary>
        /// Recursively copies all sub directories and files in 'source' to 'destination'.  Optionally only if the source file is newer than the destination file.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="onlyIfNewer"></param>
        public static void CopyDirectory(string source, string destination, bool onlyIfNewer) {
            if (Directory.Exists(source)) {
                // make sure destination is empty, create if needed.
                //if (Directory.Exists(destination)) {
                    // Toolkit.EmptyDirectory(destination);
                //}

                // copy all files at source level
                Toolkit.ResolveDirectoryPath(destination, true);
                Utility.CopyFiles(source, destination, onlyIfNewer);
                
                // then recursively all files in sub directories
                foreach (var d in TraverseDirectories(source)) {
                    var subdest = Toolkit.ResolveDirectoryPath(d.Replace(source, destination), true);
                    Utility.CopyFiles(d, subdest, onlyIfNewer);
                }

            }
        }

        /// <summary>
        /// Copies all files in given source directory to destination directory.  Optionally only if a source file is newer than the destination file.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="onlyIfNewer"></param>
        public static void CopyFiles(string source, string destination, bool onlyIfNewer){
            foreach (var f in Directory.GetFiles(source)) {
                var tgtFile = Toolkit.ResolveFilePath(f.Replace(source, destination), true);
                if (File.Exists(tgtFile)) {

                    if (onlyIfNewer) {
                        var srcFileInfo = new FileInfo(f);
                        var tgtFileInfo = new FileInfo(tgtFile);
                        if (srcFileInfo.LastWriteTimeUtc > tgtFileInfo.LastWriteTimeUtc) {
                            // source file is newer than the target, so copy it
                            File.Delete(tgtFile);
                            File.Copy(f, tgtFile);
                        } else {
                            // target is newer / same age, so do not copy the source over top of it
                            Debug.WriteLine("skipping file copy!");
                        }
                    } else {
                        // always copy over the file, so delete the target
                        File.Delete(tgtFile);
                        File.Copy(f, tgtFile);
                    }
                } else {
                    File.Copy(f, tgtFile);
                }
            }
        }

        public static IEnumerable<string> TraverseDirectories(string sourceDirectory) {
            foreach (var d in Directory.GetDirectories(sourceDirectory)) {
                yield return d;
                foreach (var d2 in TraverseDirectories(d)) {
                    yield return d2;
                }
            }
        }

        /// <summary>
        /// Given a provider code, returns the text (e.g.: "sqlserver" => "SQL Server 2008 or later").  Given provider text, returns the code (e.g.: "MySQL 5.1 or later *" => "mysql")
        /// </summary>
        /// <param name="codeNameOrFriendlyName"></param>
        /// <returns></returns>
        public static string ConvertProviderText(string codeNameOrFriendlyName) {
            var txt = ("" + codeNameOrFriendlyName).ToLower();
            // first try to convert from friendly text to code
            if (txt.Contains("sql server")) {
                return "sqlserver";
            } else if (txt.Contains("postgresql ")) {
                return "postgresql";
            } else if (txt.Contains("mysql ")) {
                return "mysql";
            } else if (txt.Contains("oracle ")) {
                return "oracle";
            } else {
                // convert from code to friendly text
                switch(txt){
                    case "sqlserver":
                        return "SQL Server 2008 or later";
                    case "mysql":
                        return "MySQL 5.1 or later *";
                    case "postgresql":
                        return "PostgreSQL 8.3 or later *";
                    case "oracle":
                        return "Oracle XE 10g or later *";
                    default:
                        throw new InvalidOperationException(getDisplayMember("ConvertProviderText", "Could not map provider={0} to a valid alternative in GrinGlobal.InstallHelper.Utility.ConvertProviderName().", codeNameOrFriendlyName));
                }
            }
        }

        //public static void InitProxySettings(SoapHttpClientProtocol soapClient) {
        //    InitProxySettings(soapClient, Utility.GetRegSetting("ProxyEnabled", 0) == 1,
        //        Utility.GetRegSetting("ProxyUseDefault", 1) == 1,
        //        Utility.GetRegSetting("ProxyServerName", ""),
        //        Utility.GetRegSetting("ProxyPort", 0),
        //        Utility.GetRegSetting("ProxyBypassOnLocal", 1) == 1,
        //        Utility.GetRegSetting("ProxyUseDefaultCredentials", 1) == 1,
        //        Utility.GetRegSetting("ProxyUserName", ""),
        //        Utility.GetRegSetting("ProxyPassword", ""),
        //        Utility.GetRegSetting("ProxyDomain", ""),
        //        Utility.GetRegSetting("ProxyExpect100Continue", 0) == 1);

        //}

        public static void InitProxySettings(HttpWebRequest req) {

            InitProxySettings(req, Utility.GetRegSetting("ProxyEnabled", 0) == 1,
                Utility.GetRegSetting("ProxyUseDefault", 1) == 1,
                Utility.GetRegSetting("ProxyServerName", ""),
                Utility.GetRegSetting("ProxyPort", 0),
                Utility.GetRegSetting("ProxyBypassOnLocal", 1) == 1,
                Utility.GetRegSetting("ProxyUseDefaultCredentials", 1) == 1,
                Utility.GetRegSetting("ProxyUserName", ""),
                Utility.GetRegSetting("ProxyPassword", ""),
                Utility.GetRegSetting("ProxyDomain", ""),
                Utility.GetRegSetting("ProxyExpect100Continue", 0) == 1);

        }

        //public static void InitProxySettings(SoapHttpClientProtocol soapClient, bool useProxy, bool useDefaultProxy, string proxyServerName, int proxyPort, bool proxyBypassOnLocal, bool proxyDefaultCredentials,
        //        string proxyUserName, string proxyPassword, string proxyDomain, bool proxyExpect100Continue) {

        //    // HACK: Due to how the SoapHttpClientProtocol object uses object pooling, setting the static ServicePointManager.Expect100Continue flag does not work after the first soap request is made (changing it is ignored since it's essentially cached in the object)
        //    //       Since that class uses a protected method (GetWebRequest) to initialize the HttpWebRequest it actually sends, we need to pluck that out and tweak the settings as needed before it is actually sent.
        //    //       We do this with some reflection trickery instead of trying to derive a class from SoapHttpClientProtocol (which is even messier to do overall)
        //    var mi = typeof(SoapHttpClientProtocol).GetMethod("GetWebRequest", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Uri) }, null);
        //    var req = mi.Invoke(soapClient, new object[] { new Uri(soapClient.Url) }) as HttpWebRequest;

        //    InitProxySettings(req, useProxy, useDefaultProxy, proxyServerName, proxyPort, proxyBypassOnLocal, proxyDefaultCredentials, proxyUserName, proxyPassword, proxyDomain, proxyExpect100Continue);

        //}

        public static void InitProxySettings(HttpWebRequest req, bool useProxy, bool useDefaultProxy, string proxyServerName, int proxyPort, bool proxyBypassOnLocal, bool proxyDefaultCredentials, 
                string proxyUserName, string proxyPassword, string proxyDomain, bool proxyExpect100Continue) {
            if (useProxy) {
                if (useDefaultProxy) {
                    req.Proxy = WebRequest.DefaultWebProxy;
                } else {
                    Uri uri = null;
                    if (!String.IsNullOrEmpty(proxyServerName)) {
                        // if they gave us a server starting with http:// or https://, assume it's the full enchilada and don't munge it at all
                        if (proxyServerName.StartsWith("http://") || proxyServerName.StartsWith("https://")) {
                            uri = new Uri(proxyServerName);
                        } else {
                            // construct a uri from proxy server name and port
                            uri = new Uri("http://" + proxyServerName + (proxyPort > 0 ? ":" + proxyPort : "") + "/");
                        }
                    }
                    req.Proxy = new WebProxy(uri, proxyBypassOnLocal);
                }

                if (proxyDefaultCredentials) {
                    req.Proxy.Credentials = CredentialCache.DefaultCredentials;
                } else {
                    if (!String.IsNullOrEmpty(proxyUserName)) {
                        req.Proxy.Credentials = new NetworkCredential(proxyUserName, proxyPassword, proxyDomain);
                    }
                }
            } else {
                // use no proxy (overwrite the default of using IE settings)
                req.Proxy = null;
            }

            if (req.Proxy != null) {
                if (req.ServicePoint.Expect100Continue != proxyExpect100Continue) {
                    // HACK: support older proxy servers that don't understand the Expect 100 http header
                    req.ServicePoint.Expect100Continue = proxyExpect100Continue;
                }
            }

        }


        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "Core", "Utility", resourceName, null, defaultValue, substitutes);
        }
    }
}
