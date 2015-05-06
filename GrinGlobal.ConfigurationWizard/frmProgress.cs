using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Management;
using Microsoft.Win32;
using GrinGlobal.Core;
using System.DirectoryServices;
using GrinGlobal.InstallHelper;

namespace GrinGlobal.ConfigurationWizard {
    public partial class frmProgress : Form {
        public frmProgress() {
            InitializeComponent();
        }

        public List<ActionDetail> ActionsToDo { get; set; }

        private void updateWindows() {
            Process.Start(@"http://www.windowsupdate.com/");
        }

        private string installIIS(ActionDetail actionDetail) {

            if (Utility.IsWindowsXP) {
                return installIISOnXP(actionDetail);
            } else if (Utility.IsWindowsVistaOrBetter) {
                return installIISOnVista(actionDetail);
            //} else if (Utility.IsWindows7) {
            //    return "TODO: implement IIS install on Windows 7";
            }
            return "INVALID CONFIGURATION: OS of XP or Vista not detected; cannot install IIS";
        }

        private string installIISOnVista(ActionDetail actionDetail) {

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

            _processError = new StringBuilder();
            _processOutput = new StringBuilder();
            p.ErrorDataReceived +=new DataReceivedEventHandler(p_ErrorDataReceived);
            p.OutputDataReceived +=new DataReceivedEventHandler(p_OutputDataReceived);
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();

            while (!p.HasExited) {
                Thread.Sleep(100);
                Application.DoEvents();
            }

            string output = _processOutput.ToString() + " " + _processError.ToString();

            return output;

        }

        private string getWindowsVistaProductVersionNumber() {
            string fileName = Environment.GetEnvironmentVariable("WINDIR") + @"\regedit.exe";
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fileName);
            return fvi.ProductVersion;
        }

        private string getIisXmlForVistaBusinessOrUltimate() {
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

        private string getIisXmlForVistaHome() {
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

        private string installIISOnVistaBusinessOrUltimate(ActionDetail actionDetail) {
            return null;
        }

        private string installIISOnXP(ActionDetail actionDetail) {
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
                MessageBox.Show("Exception installing IIS: " + ex1.Message);
                throw;
            }
        }

        private const int _maxLabels = 10;

        private void initLabels() {
            for (int i = 0; i < _maxLabels; i++) {
                Label lbl = Controls["lbl" + i] as Label;
                if (i < ActionsToDo.Count) {
                    lbl.Visible = true;
                    lbl.Text = ActionsToDo[i].FriendlyName;
                } else {
                    lbl.Visible = false;
                    lbl.Text = "";
                }
            }
        }

        public string TargetDirectory;
        public string DatabasePassword;
        public string DatabaseEngine;
        public string DatabaseInstanceName;
        public bool DatabaseWindowsAuth;
        int _currentAction = -1;
        bool _cancelling = false;

        private string getVolumeName(string path) {
            if (String.IsNullOrEmpty(path) || !path.Contains(@":\")) {
                return "";
            }
            // determine volume name that contains for the installer cache dir
            // and the volume name that contains the source dir
            // that way we can detect changes (for example, when IIS is installed they switched the CD)
            DriveInfo[] dis = DriveInfo.GetDrives();
            foreach (DriveInfo di in dis) {
                if (path.ToLower().StartsWith(di.Name.ToLower())) {
                    if (di.IsReady) {
                        // if it's not ready, don't pull the volumelabel as that will throw an exception
                        return di.VolumeLabel;
                    } else {
                        // media is not inserted, but this is the drive they requested.
                        // return ZLS
                        return "";
                    }
                }
            }
            return "";

        }

        private string getWebConfigFilePath(string webApplicationName) {
            DirectoryEntry root = new DirectoryEntry(
            "IIS://localhost/W3SVC/1/Root");
            //"user",
            //"password");

            System.DirectoryServices.PropertyCollection properties = root.Properties;
            string path = (properties["path"] == null ? null : properties["path"].Value) as string;

            if (!String.IsNullOrEmpty(path)) {
                path = path + (@"\" + webApplicationName + @"\web.config");
            }

            return path;
        }

        private StringBuilder _processOutput;
        private StringBuilder _processError;


        private DatabaseEngineUtil writeDatabaseRegistryKeys(){
                // we pass __USERID__ and __PASSWORD__ so they can be replaced easily later (and we don't want to store those values in the registry anyway)
                DatabaseEngineUtil dbEngineUtil = DatabaseEngineUtil.CreateInstance(DatabaseEngine, Utility.GetTargetDirectory(null, null, "ConfigurationWizard") + @"\gguac.exe", this.DatabaseInstanceName);
                string conn = dbEngineUtil.GetDataConnectionSpec("gringlobal", "__USERID__", "__PASSWORD__").ConnectionString;
                Utility.SetDatabaseConnectionString(conn);
                Utility.SetDatabaseInstanceName(this.DatabaseInstanceName);
                Utility.SetDatabaseWindowsAuth(this.DatabaseWindowsAuth);
            return dbEngineUtil;
        }

        private void btnGo_Click(object sender, EventArgs e) {


            string binDir = Utility.GetBinDirectory(null, null);

            // clear out the log file
            string logfile = TargetDirectory + "install_log.txt";
            if (File.Exists(logfile)) {
                // rename the logfile to something else
                File.Move(logfile, logfile.Replace(".txt", "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".txt"));
//                File.Delete(logfile);
            }

            string userName = "";

            try {
                Cursor = Cursors.WaitCursor;

                userName = getUserName();


                btnGo.Enabled = false;

                // disable cancel ability for now...
                btnCancel.Enabled = false;

                // start the fake progress bar
                timer1.Start();




                // remember the engine / connectionstring for later...

                // remember which engine is in use
                Utility.SetDatabaseEngine(DatabaseEngine.ToLower());


                DatabaseEngineUtil dbEngineUtil = writeDatabaseRegistryKeys();

                Label prev = null;
                Label cur = null;

                _cancelling = false;

                string result = "Success";

                string sourceVolumeName = getVolumeName(binDir);

//                bool weInstalledSqlServer = false;

                // see if we're installing iis
                if (Toolkit.IsWindowsXP) {
                    for (int i = 0; i < ActionsToDo.Count; i++) {
                        if (ActionsToDo[i].SpecialCommand == "iis_install") {
                            MessageBox.Show("You will be prompted for your Windows XP CD.\nInstallation cannot complete successfully without it.\n\nIf the 'Welcome to Windows XP' screen appears, please close it.");
                            break;
                        }
                    }
                }

                // see if we're installing database engine
                // if we are, we need to remember to refresh dbengineutil after it has completed, then write the connection string
                // since dbengineutil relies on registry entries from the db engine to get the proper values.

                for (int i = 0; i < ActionsToDo.Count; i++) {

                    if (!_cancelling) {
                        // get volume label again, making sure the same CD is in the drive...
                        while (sourceVolumeName != getVolumeName(binDir)) {
                            if (DialogResult.Cancel == MessageBox.Show(this, "Please insert the GRIN-Global installation CD.", "Insert GRIN-Global CD", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)) {
                                _cancelling = true;
                                break;
                            }
                        }
                    }


                    if (_cancelling) {
                        _currentAction = -1;
                        StringBuilder sb = new StringBuilder();
                        for (int j = i; j < ActionsToDo.Count; j++) {
                            sb.AppendLine("*********************************************************************************");
                            sb.AppendLine("Cancelled action before it started: " + ActionsToDo[j].FriendlyName);
                        }
                        File.AppendAllText(logfile, sb.ToString());
                        break;
                    }


                    string msilog = TargetDirectory + "msilog.txt";
                    if (File.Exists(msilog)) {
                        File.Delete(msilog);
                    }

                    string logtext = "";

                    _currentAction = i;

                    ActionDetail act = ActionsToDo[i];

                    cur = Controls["lbl" + i] as Label;
                    cur.Text = "> " + cur.Text;
                    cur.Font = _boldFont;
                    prev = cur;

                    Application.DoEvents();

                    if (act.SpecialCommand == "iis_install") {
                        // special case, use windows component installer to do the work (as we can't redistribute IIS files)
                        File.AppendAllText(logfile, String.Format(@"


================================================================
Starting Action: {0}
        Command: (install iis)
           Args: -
   Working Path: -
           Time: {2}
================================================================

", act.FriendlyName, act.PrimaryArgs, DateTime.Now.ToString()));

                        result = installIIS(act);
                        if (String.IsNullOrEmpty(result) || result.Trim() == string.Empty) {
                            result = "Success";
                        }

                        File.AppendAllText(logfile, String.Format(@"

================================================================
Ending Action: {0}
       Result: {1}
         Time: {2}
================================================================

", act.FriendlyName, result, DateTime.Now.ToString()));

                    } else if (act.SpecialCommand == "windows_update") {
                        updateWindows();
                        result = "Success";
                    } else {

                        string origBinDir = binDir;
                        if (act.PrimaryExe.Contains("__TEMPBINDIR__") || act.PrimaryArgs.Contains("__TEMPBINDIR__") || act.PrimaryWorkingDirectory.Contains("__TEMPBINDIR__")) {

                            // this expects to run from the temp dir.

                            // we must first extract the exe to a temp folder, then 

                            string tempDir = Utility.GetTempDirectory();
                            if (!Directory.Exists(tempDir)) {
//                                MessageBox.Show("Creating temp directory: '" + tempDir + "'");
                                Directory.CreateDirectory(tempDir);
                            }

                            if (!String.IsNullOrEmpty(act.TempExe)) {
                                string tempToRun = act.TempExe.Replace("__BINDIR__", binDir).Replace("__TEMPBINDIR__", tempDir).Replace(@"\\", @"\");
                                string tempArgs = act.TempArgs.Replace("__BINDIR__", binDir).Replace("__TEMPBINDIR__", tempDir).Replace(@"\\", @"\");

//                                MessageBox.Show("Running temp exe/args: " + tempToRun + " " + tempArgs);

                                ProcessStartInfo psiExtract = new ProcessStartInfo(tempToRun, tempArgs);
                                psiExtract.CreateNoWindow = true;
                                Process.Start(psiExtract).WaitForExit();
                            }

                            origBinDir = binDir;
                            binDir = tempDir;

                            if (!String.IsNullOrEmpty(act.SpecialCommand)) {
                                // possibly have additional work to do...
                                if (act.SpecialCommand.ToLower().Contains("sqlserver")) {

                                    //if (Utility.IsWindowsVista || Utility.IsWindows7) {

                                    //    // need to make sure WinHttpAutoProxySvc has proper settings
                                    //    Process pWinProxy = Process.Start("sc.exe", " sdset winhttpautoproxysvc D:(A;;CCLCSWRPWPDTLOCRRC;;;SY)(A;;CCDCLCSWRPWPDTLOCRSDRCWDWO;;;BA)(A;;CCLCSWLOCRRC;;;AU)(A;;LCRP;;;IU)(A;;LCRP;;;SU)");
                                    //    pWinProxy.WaitForExit();

                                    //    // and the current user is explicitly listed as having Debug privileges

                                    //    Process pDebugPrivs = Process.Start((origBinDir + @"\ntrights.exe").Replace(@"\\", @"\"), " -u __CURRENTUSER__ +r SeDebugPrivilege".Replace("__CURRENTUSER__", userName;


                                    //}




                                    logtext += "sqlserverconfig.ini contents:\r\n------------------------------------------\r\n";
                                    logtext += prepareSqlServerConfigFile(act, tempDir, binDir);
                                }
                            }


                        }




                        // act[1] either contains a BINDIR or TEMPBINDIR variable, should never contain both.
                        // we replace both since we don't know which it will be.
                        string file = act.PrimaryExe.Replace("__BINDIR__", binDir).Replace("__TEMPBINDIR__", binDir).Replace(@"\\", @"\").Replace(@"""", @"");


                        var fi = new FileInfo(file);
                        if (!fi.Exists && file.ToLower() != "cmd.exe") {
                            // the file is not in the default installers cache. see if it's in the original installers path (sourcedir)
                            binDir = Utility.GetSourceDirectory(null, null); 
                            
                            if (!String.IsNullOrEmpty(binDir)) {
                                binDir += "bin";
                                file = act.PrimaryExe.Replace("__BINDIR__", binDir).Replace("__TEMPBINDIR__", binDir).Replace(@"\\", @"\").Replace(@"""", @"");
                            }

                            fi = new FileInfo(file);

                            while (fi != null && !fi.Exists) {
                                openFileDialog1.Title = "Locate file to " + act.FriendlyName;
                                openFileDialog1.InitialDirectory = binDir;
                                openFileDialog1.FileName = fi.Name;
                                openFileDialog1.Filter = "All Files (*.*)|*.*";
                                DialogResult dr = openFileDialog1.ShowDialog();
                                if (dr == DialogResult.OK) {
                                    file = openFileDialog1.FileName;
                                    fi = new FileInfo(file);
                                } else {
                                    result = "User cancelled.";
                                    // they cancelled out of a prompt.  close, go back to main form.
                                    new frmDone().ShowDialog(result, File.ReadAllText(logfile), this);
                                    DialogResult = DialogResult.Cancel;
                                    Close();
                                    return;
                                }
                            }

                            if (fi == null || !fi.Exists) {
                                logtext += "Could not find file '" + file + "' to launch.";
                                throw new InvalidOperationException(logtext);
                            }
                        }


                        string args = act.PrimaryArgs.Replace("__BINDIR__", binDir).Replace(@"__TEMPBINDIR__", binDir).Replace(@"\\", @"\");
                        if (args.ToLower().Contains("msiexec")) {
                            // msiexec.exe uses "" to escape embedded quotes
                            args = args.Replace("__PASSWORD__", ("" + DatabasePassword).Replace(@"""", @""""""));
                        } else {
                            // everybody else in the world uses \"
                            args = args.Replace("__PASSWORD__", ("" + DatabasePassword).Replace(@"""", @"\"""));
                        }
                        args = args.Replace("__CURRENT_USER__", userName + "");

                        string workingDir = act.PrimaryWorkingDirectory.Replace("__BINDIR__", binDir).Replace("__TEMPBINDIR__", binDir).Replace(@"\\", @"\");


                        ProcessStartInfo psi = new ProcessStartInfo(file, args);

                        if (Directory.Exists(workingDir)) {
                            psi.WorkingDirectory = workingDir;
                        } else {
                            string parentFolder = file;
                            try {
                                parentFolder = Directory.GetParent(file.Replace(@"""", "")).FullName;
                            } catch {
                                parentFolder = @".\installers";
                            }
                            psi.WorkingDirectory = parentFolder;
                        }

                        psi.WorkingDirectory = psi.WorkingDirectory.Replace(@"\\", @"\");


                        File.AppendAllText(logfile, String.Format(@"


================================================================
Starting Action: {0}
        Command: {1}
           Args: {2}
   Working Path: {3}
           Time: {4}
================================================================

", act.FriendlyName, psi.FileName, psi.Arguments, psi.WorkingDirectory, DateTime.Now.ToString()));

                        Application.DoEvents();

                        string output = "";
                        string lastAction = "";

                        try {

                            // HACK installer classes can only run on .exe or .dll's -- not web apps.
                            // So we're doing what should be done in the web app's installer class here.
                            if (act.PrimaryArgs.ToLower().Contains("gringlobal_web_setup") && act.FriendlyName.ToLower().StartsWith("install")) {
                                lastAction = "Registering .NET in IIS";
                                makeSureDotNetIsRegisteredInIIS();
                            }


                            if (act.PrimaryArgs.ToLower().Contains("gringlobal_database_setup")){

                                if (dbEngineUtil is SqlServerEngineUtil) {

                                    // make sure mixed mode is enabled (set the registry key, restart sql server)
                                    ((SqlServerEngineUtil)dbEngineUtil).EnableMixedModeIfNeeded();

                                }

                            }



                            //                    MessageBox.Show("Going to run:\n\n" + file + " " + args + "\n\nin folder:\n" + act[3]);

                            psi.CreateNoWindow = true;


                            psi.UseShellExecute = false;
                            psi.RedirectStandardError = true;
                            psi.RedirectStandardOutput = true;

                            lastAction = "Executing " + psi.FileName + " " + psi.Arguments;

//                            MessageBox.Show("Running: " + psi.FileName + " " + psi.Arguments + "\nWorking Directory: " + psi.WorkingDirectory);

                            Process p = Process.Start(psi);

                            //if (file.ToLower().Contains("msiexec.exe")) {
                            //    // some fudge time to allow msiexec to launch...
                            //    Thread.Sleep(2000);
                            //    int msiexecCount = 2;
                            //    while (msiexecCount > 1) {
                            //        // we do > 1 because one msiexec process is the Windows Installer Service
                            //        msiexecCount = Process.GetProcessesByName("msiexec").Length;
                            //        Thread.Sleep(200);
                            //        Application.DoEvents();
                            //    }
                            //} else {

                            _processOutput = new StringBuilder();
                            _processError = new StringBuilder();
                            p.OutputDataReceived +=new DataReceivedEventHandler(p_OutputDataReceived);
                            p.ErrorDataReceived +=new DataReceivedEventHandler(p_ErrorDataReceived);
                            p.BeginErrorReadLine();
                            p.BeginOutputReadLine();
                            while (!p.HasExited) {
                                Thread.Sleep(100);
                                Application.DoEvents();
                            }

                            output = _processOutput.ToString() + " " + _processError.ToString();

                            if (p.ExitCode != 0) {
                                if (output.Contains("Result error code: 3010")) {
                                    // reboot required.
                                    // write out a file to remember what they selected to install so we can re-select and auto start upon reboot.
                                    DialogResult = DialogResult.Retry;
                                    this.Close();
                                    return;
                                } else {
                                    logtext += "Install failed, return code=" + p.ExitCode + "\nOutput=" + output;
                                    throw new InvalidOperationException(logtext);
                                }
                            }


                            //}

                            //                    MessageBox.Show(file + " " + args + " (pid=" + pid + ") should be done by now.");



                            if (act.PrimaryArgs.ToLower().Contains("gringlobal_web_setup")) {
                                try {
                                    // update the config file...
                                    string configFile = getWebConfigFilePath("gringlobal");
                                    if (File.Exists(configFile)) {

                                        lastAction = "Reading configfile " + configFile;
                                        string contents = File.ReadAllText(configFile);

                                        // look up engine from registry (assumes database has been installed already)
                                        string engine = Utility.GetDatabaseEngine(null, null, true) + "";
                                        string connectionString = Utility.GetDatabaseConnectionString(null, null, "gg_user", "gg_user_passw0rd!");
                                        string appSetting = String.Format(@"<add providerName=""{0}"" name=""DataManager"" connectionString=""{1}"" />", engine, connectionString);

                                        contents = contents.Replace("<!-- __CONNECTIONSTRING__ -->", appSetting);
                                        contents = contents.Replace("<!-- __COMMENT__ -->", "<!-- TESTING ");
                                        contents = contents.Replace("<!-- __ENDCOMMENT__ -->", " -->");

                                        lastAction = "Writing configfile " + configFile;

                                        File.WriteAllText(configFile, contents);
                                    }
                                } finally {
                                }

                            } else if (DatabaseEngine.ToLower().Contains("mysql") && act.PrimaryArgs.ToLower().Contains("mysql")) {
                                // mysql needs some help after install. since we don't run the configuration wizard (no way to
                                // automate it easily w/o user interface), we need to do all the steps it would do.
                                // this means:
                                //  1. Create the my.ini file
                                //  2. Add the bin dir to the PATH
                                //  3. set root password (even if a previous install existed, stomp over its password)
                                //  4. write firewall exception
                                //  5. Install mysql as a service
                                //  6. Start the service

                                // create a new database engine util so the registry values are re-read
                                DatabaseEngineUtil engineUtil = DatabaseEngineUtil.CreateInstance(DatabaseEngine, Utility.GetTargetDirectory(null, null, "ConfigurationWizard") + @"\gguac.exe", this.DatabaseInstanceName);


                                // 1. create my.ini file
                                lastAction = "Writing my.ini file to " + engineUtil.BaseDirectory;
                                File.WriteAllText(engineUtil.BaseDirectory + "my.ini", engineUtil.GetSettings());

                                // 2. add bin dir to the PATH
                                string curPath = Environment.GetEnvironmentVariable("PATH");
                                if (!curPath.ToLower().Contains(engineUtil.BinDirectory.ToLower())) {
                                    string newPath = curPath + ";" + engineUtil.BinDirectory;
                                    lastAction = "Adding to PATH: " + engineUtil.BinDirectory;
                                    Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Machine);
                                }

                                // 3. set root password
                                string toWrite = String.Format(@"UPDATE mysql.user SET Password=PASSWORD('{0}') WHERE User='root';
FLUSH PRIVILEGES;
", DatabasePassword.Replace("'", "''"));

                                string initFile = (Environment.GetEnvironmentVariable("TEMP") + @"\reset_mysql_root.sql").Replace(@"\\", @"\");
                                lastAction = "Writing initFile to " + initFile;
                                File.WriteAllText(initFile, toWrite);


                                string mysqlDaemonExe = (engineUtil.BinDirectory + @"\mysqld.exe").Replace(@"\\", @"\");
                                string defaultsFile = (engineUtil.BaseDirectory + @"\my.ini").Replace(@"\\", @"\");


                                try {

                                    // to set root password, we use this method so we don't have to know the previous
                                    // root password, if any.  it's more convoluted but doesnt require the user to 
                                    // specify the previous root password (meaning we don't hav to prompt for it)

                                    string mysqlArgs = @" --defaults-file=""" + defaultsFile + @""" --init-file=""" + initFile + @""" ";

                                    lastAction = "Executing: " + mysqlDaemonExe + " " + mysqlArgs;
                                    Process pMysql = Process.Start(mysqlDaemonExe, mysqlArgs);
                                    // wait for 3 seconds, then kill it -- mysqld never ends
                                    Thread.Sleep(3000);
                                    pMysql.Kill();
                                    //                                    Process pMysqlend = Process.Start(engineUtil.BinDirectory + @"\mysqladmin.exe".Replace(@"\\", @"\"), " --shutdown");
                                    //                                    pMysqlend.WaitForExit();
                                    //                                    pMysql.WaitForExit();
                                } finally {
                                    // make sure the init file is deleted since it contains the password
                                    try {
                                        File.Delete(initFile);
                                    } catch {
                                    }
                                }

                                // 4. write firewall exception
                                writeFirewallException(mysqlDaemonExe, "mysql daemon", true);

                                // 5. install mysql as a service and start it
                                lastAction = "Installing mysql as a service via: " + mysqlDaemonExe + (@" --install mysql --defaults-file=""" + defaultsFile + @"""").Replace(@"\\", @"\");
                                Process pSvc = Process.Start(mysqlDaemonExe, @" --install mysql --defaults-file=""" + defaultsFile + @""" ");
                                pSvc.WaitForExit();

                                // 6. start the service
                                lastAction = "starting mysql service";
                                engineUtil.StartService();

                            }

                            // re-save registry keys if needed
                            string fname = (ActionsToDo[i].FriendlyName + "").ToLower();
                            if (fname.StartsWith("install")) {
                                if (fname.Contains("mysql") || fname.Contains("sql server") || fname.Contains("postgresql") || fname.Contains("oracle")) {
                                    // need to reload the dbEngineUtil and re-write the registry keys so they are correct, as we just
                                    // finished installing a database engine.
                                    dbEngineUtil = writeDatabaseRegistryKeys();
                                }
                            }



                            //if (act.FriendlyName.ToLower().StartsWith("install sql server")) {
                            //    weInstalledSqlServer = true;
                            //}


                        } catch (Exception ex2) {
                            result = "Failed -- " + ex2.Message + " (last action was: " + lastAction + ")";
                            _cancelling = true;
                        }

                        // if it was an MSI, it should have logged messages to the msilog.txt file.
                        // if it did, copy the contents in to our install log then delete the file.
                        if (File.Exists(msilog)) {
                            string msilogtext = File.ReadAllText(msilog);
                            logtext += "\r\n" + msilogtext;
                            File.AppendAllText(logfile, msilogtext);
                            File.Delete(msilog);
                        }

                        if (logtext.Contains("-- Installation failed.")) {
                            result = "Failed -- see log file";
                            _cancelling = true;
                        }

                        File.AppendAllText(logfile, String.Format(@"

================================================================
Ending Action: {0}
       Result: {1}
         Time: {2}
================================================================

", act.FriendlyName, result, DateTime.Now.ToString()));

                        if (act.PrimaryExe.Contains("__TEMPBINDIR__") || act.PrimaryArgs.Contains("__TEMPBINDIR__") || act.PrimaryWorkingDirectory.Contains("__TEMPBINDIR__")) {

                            // get rid of folder we extracted everything to
                            if (Directory.Exists(binDir)) {
                                Directory.Delete(binDir, true);
                            }

                            // restore binDir for next installer (temp one is special case)
                            binDir = origBinDir;
                        }


                    }

                    prev.Font = _regularFont;
                    prev.Text = prev.Text.Replace("> ", "") + " - " + (result == "Success" ? "Success" : "Failed");
                    Application.DoEvents();


                }

                _currentAction = -1;

                timer1.Stop();

                progressBar1.Value = 100;

                if (_cancelling) {
                    new frmDone().ShowDialog(result, File.ReadAllText(logfile), this);
                    DialogResult = DialogResult.Cancel;
                } else {
                    new frmDone().ShowDialog(null, File.ReadAllText(logfile), this);
                    DialogResult = DialogResult.OK;
                }
                this.Close();


                //if (_cancelling) {
                //    MessageBox.Show("Action cancelled.");
                //} else {
                //    MessageBox.Show("GRIN-Global has been successfully configured.");
                //    DialogResult = DialogResult.OK;
                //    this.Close();
                //}

            } catch (Exception ex){
                Cursor = Cursors.Default;
                string log = (File.Exists(logfile) ? File.ReadAllText(logfile) : "");
                new frmDone().ShowDialog(ex.Message, log, this);
                DialogResult = DialogResult.Cancel;
                this.Close();
            } finally {
                Cursor = Cursors.Default;
            }


        }

        void p_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            if (_processError != null) {
                _processError.AppendLine(e.Data);
            }
        }

        void p_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            if (_processOutput != null) {
                _processOutput.AppendLine(e.Data);
            }
        }

        private void writeFirewallException(string fullFilePath, string displayName, bool enabled) {
            string value = fullFilePath + ":*:" + (enabled ? "Enabled" : "Disabled") + ":" + displayName;
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\AuthorizedApplications\List", fullFilePath, value);
        }


        private string getUserName() {

            // since we may run in the context of the installer (first time only, but most frequent case), we can't just use the environment variable values...
            //if (!String.IsNullOrEmpty(Environment.UserDomainName)) {
            //    return Environment.UserDomainName + @"\" + Environment.UserName;
            //} else {
            //    return Environment.UserName;
            //}
            
            Process[] ps = Process.GetProcessesByName("explorer");
            if (ps.Length > 0) {
                return ps[0].StartInfo.EnvironmentVariables["username"];
            } else {
                // nobody logged in? hmmmm that's fishy.
                throw new InvalidOperationException("Nobody seems to be currently logged in. (no explorer processes are running)");
            }


        }

        Random _ran = new Random();

        private void timer1_Tick(object sender, EventArgs e) {
            progressBar1.Value = (progressBar1.Value + _ran.Next(1, 10)) % 100;
        }

        private Font _boldFont;
        private Font _regularFont;

        private void frmProgress_Load(object sender, EventArgs e) {
            _regularFont = new Font("Sans Serif", 8.25f, FontStyle.Regular);
            _boldFont = new Font("Sans Serif", 8.25f, FontStyle.Bold);
            initLabels();
        }

        private void btnCancel_Click(object sender, EventArgs e) {

            btnCancel.Enabled = false;
            btnGo.Enabled = false;

            if (_currentAction > -1) {
                Controls["lbl" + _currentAction].Text += "CANCELLING...";
                _cancelling = true;
            }

            // wait for currentAction to reset to -1 (meaning if we cancelled, the current install was allowed to finish)
            while (_currentAction > -1) {
                Application.DoEvents();
            }


            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private string getDotNet20Directory() {
            string dir = Environment.GetEnvironmentVariable("WINDIR") + @"\Microsoft.NET\Framework\v2.0.50727\";
            return dir.Replace(@"\\", @"\");
        }

        private void makeSureDotNetIsRegisteredInIIS() {
            string dotnet20Dir = getDotNet20Directory();
            //ProcessStartInfo psi = new ProcessStartInfo(dotnet20Dir + "aspnet_regiis.exe", " /lv");
            //psi.WorkingDirectory = dotnet20Dir;
            //psi.UseShellExecute = false;
            //psi.CreateNoWindow = true;
            //psi.RedirectStandardError = true;
            //psi.RedirectStandardOutput = true;
            //Process p = Process.Start(psi);
            //string output = p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();
            //p.WaitForExit();

            //if (output.Contains("2.0.50727")) {
            //    // we're good, asp.net 2.0 is registered in IIS
            //} else {
                // we need to register asp.net 2.0 in IIS
                ProcessStartInfo psiRegister = new ProcessStartInfo(dotnet20Dir + "aspnet_regiis.exe", " /i");
                psiRegister.UseShellExecute = false;
                psiRegister.WorkingDirectory = dotnet20Dir;
                psiRegister.CreateNoWindow = true;
                Process p2 = Process.Start(psiRegister);
                p2.WaitForExit();
            //}
        }

        private string prepareSqlServerConfigFile(ActionDetail args, string tempDir, string sourceDir) {



            // sql server needs a configuration.ini file created to install properly on Vista.
            // since this also works on XP / Windows 7, we do it every time.

            string cmd = args.SpecialCommand;

            string configContent = "";
            if (cmd.Contains("uninstall")) {

                // we're uninstalling something, use uninstall config
                configContent = getSqlServerUninstallConfig();

            } else {

                // we're installing something, use install config
                configContent = getSqlServerInstallConfig();


            }

            // replace constants as needed
            if (args.ConfigurationDictionary == null) {
                args.ConfigurationDictionary = new Dictionary<string,string>();
            }

            if (!args.ConfigurationDictionary.ContainsKey("__TEMPBINDIR__")) {
                args.ConfigurationDictionary.Add("__TEMPBINDIR__", tempDir);
            }
            if (!args.ConfigurationDictionary.ContainsKey("__BINDIR__")) {
                args.ConfigurationDictionary.Add("__BINDIR__", sourceDir);
            }
            foreach (string key in args.ConfigurationDictionary.Keys) {
//                configContent = configContent.Replace(key, args.ConfigurationDictionary[key].Replace(@"""", ""));
                configContent = configContent.Replace(key, args.ConfigurationDictionary[key]);
            }

            // write final config file to same folder (tempDir) as the sql server setup.exe
            string configFile = (tempDir + @"\sqlserverconfig.ini").Replace(@"\\", @"\");
            if (File.Exists(configFile)) {
                File.Delete(configFile);
            }
            File.WriteAllText(configFile , configContent);

            return configContent;

        }


        private string getSqlServerUninstallConfig() {

            string val = @"
;SQLSERVER2008 Configuration File
[SQLSERVER2008]

; Specify the Instance ID for the SQL Server features you have specified. SQL Server directory structure, registry structure, and service names will reflect the instance ID of the SQL Server instance. 

INSTANCEID=""__INSTANCENAME__""

; Specifies a Setup work flow, like INSTALL, UNINSTALL, or UPGRADE. This is a required parameter. 

ACTION=""Uninstall""

; Specifies features to install, uninstall, or upgrade. The list of top-level features include SQL, AS, RS, IS, and Tools. The SQL feature will install the database engine, replication, and full-text. The Tools feature will install Management Tools, Books online, Business Intelligence Development Studio, and other shared components. 

FEATURES=__UNINSTALLFEATURES__

; Displays the command line parameters usage 

HELP=""False""

; Specifies that the detailed Setup log should be piped to the console. 

INDICATEPROGRESS=""True""

; Setup will not display any user interface. 

QUIET=""False""

; Setup will display progress only without any user interaction. 

QUIETSIMPLE=""True""

; Specifies the path to the installation media folder where setup.exe is located. 

MEDIASOURCE=""__TEMPBINDIR__""

; Specify a default or named instance. MSSQLSERVER is the default instance for non-Express editions and SQLExpress for Express editions. This parameter is required when installing the SQL Server Database Engine (SQL), Analysis Services (AS), or Reporting Services (RS). 

INSTANCENAME=""__INSTANCENAME__""


";

            return val;
        }

        private string getSqlServerInstallConfig() {

            string val = @"
;SQLSERVER2008 Configuration File
[SQLSERVER2008]

; Specify the Instance ID for the SQL Server features you have specified. SQL Server directory structure, registry structure, and service names will reflect the instance ID of the SQL Server instance. 

INSTANCEID=""__INSTANCENAME__""

; Specifies a Setup work flow, like INSTALL, UNINSTALL, or UPGRADE. This is a required parameter. 

ACTION=""Install""

; Specifies features to install, uninstall, or upgrade. The list of top-level features include SQL, AS, RS, IS, and Tools. The SQL feature will install the database engine, replication, and full-text. The Tools feature will install Management Tools, Books online, Business Intelligence Development Studio, and other shared components. 

FEATURES=__INSTALLFEATURES__

; Displays the command line parameters usage 

HELP=""False""

; Specifies that the detailed Setup log should be piped to the console. 

INDICATEPROGRESS=""True""

; Setup will not display any user interface. 

QUIET=""False""

; Setup will display progress only without any user interaction. 

QUIETSIMPLE=""True""

; Specifies that Setup should install into WOW64. This command line argument is not supported on an IA64 or a 32-bit system. 

X86=""False""

; Specifies the path to the installation media folder where setup.exe is located. 

MEDIASOURCE=""__TEMPBINDIR__""

; Specify if errors can be reported to Microsoft to improve future SQL Server releases. Specify 1 or True to enable and 0 or False to disable this feature. 

ERRORREPORTING=""False""

; Specify the root installation directory for native shared components. 

INSTALLSHAREDDIR=""C:\Program Files\Microsoft SQL Server""

; Specify the installation directory. 

INSTANCEDIR=""C:\Program Files\Microsoft SQL Server""

; Specify that SQL Server feature usage data can be collected and sent to Microsoft. Specify 1 or True to enable and 0 or False to disable this feature. 

SQMREPORTING=""False""

; Specify a default or named instance. MSSQLSERVER is the default instance for non-Express editions and SQLExpress for Express editions. This parameter is required when installing the SQL Server Database Engine (SQL), Analysis Services (AS), or Reporting Services (RS). 

INSTANCENAME=""__INSTANCENAME__""

; Auto-start service after installation.  

AGTSVCSTARTUPTYPE=""Manual""

; Startup type for Integration Services. 

ISSVCSTARTUPTYPE=""Automatic""

; Account for Integration Services: Domain\User or system account. 

ISSVCACCOUNT=""NT AUTHORITY\NETWORK SERVICE""

; Controls the service startup type setting after the service has been created. 

ASSVCSTARTUPTYPE=""Automatic""

; The collation to be used by Analysis Services. 

ASCOLLATION=""Latin1_General_CI_AS""

; The location for the Analysis Services data files. 

ASDATADIR=""Data""

; The location for the Analysis Services log files. 

ASLOGDIR=""Log""

; The location for the Analysis Services backup files. 

ASBACKUPDIR=""Backup""

; The location for the Analysis Services temporary files. 

ASTEMPDIR=""Temp""

; The location for the Analysis Services configuration files. 

ASCONFIGDIR=""Config""

; Specifies whether or not the MSOLAP provider is allowed to run in process. 

ASPROVIDERMSOLAP=""1""

; Startup type for the SQL Server service. 

SQLSVCSTARTUPTYPE=""Automatic""

; Level to enable FILESTREAM feature at (0, 1, 2 or 3). 

FILESTREAMLEVEL=""0""

; Set to ""1"" to enable RANU for SQL Server Express. 

ENABLERANU=""True""

; Specifies a Windows collation or an SQL collation to use for the Database Engine. 

SQLCOLLATION=""SQL_Latin1_General_CP1_CI_AS""

; Account for SQL Server service: Domain\User or system account. 

SQLSVCACCOUNT=""NT AUTHORITY\NETWORK SERVICE""

; Windows account(s) to provision as SQL Server system administrators. 

SQLSYSADMINACCOUNTS=""BUILTIN\Administrators""

; The default is Windows Authentication. Use ""SQL"" for Mixed Mode Authentication. 

__SECURITYMODE__

; Provision current user as a Database Engine system administrator for SQL Server 2008 Express. 

ADDCURRENTUSERASSQLADMIN=""True""

; Specify 0 to disable or 1 to enable the TCP/IP protocol. 

TCPENABLED=""0""

; Specify 0 to disable or 1 to enable the Named Pipes protocol. 

NPENABLED=""0""

; Startup type for Browser Service. 

BROWSERSVCSTARTUPTYPE=""Disabled""

; Specifies how the startup mode of the report server NT service.  When 
; Manual - Service startup is manual mode (default).
; Automatic - Service startup is automatic mode.
; Disabled - Service is disabled 

RSSVCSTARTUPTYPE=""Automatic""

; Specifies which mode report server is installed in.  
; Default value: “FilesOnly”  

RSINSTALLMODE=""FilesOnlyMode""
";

            return val;
        }


    }
}
