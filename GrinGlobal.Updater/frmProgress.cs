using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Management;
using System.Diagnostics;
using System.Threading;
using GrinGlobal.InstallHelper;
using GrinGlobal.Core;

namespace GrinGlobal.Updater {
    public partial class frmProgress : Form {
        public frmProgress() {
            InitializeComponent();
        }

        private bool _cancel;

        private void btnCancel_Click(object sender, EventArgs e) {
            _cancel = true;
        }

        private string _componentType;
        private SortedDictionary<string, List<FileDownloadInfo>> _files;

//        public string PathToBatFileToReinstallUpdater { get; set; }

        private bool _installingNewUpdater;
        private bool _isMirrorServer;
        private bool _performInstallation;

        private string _dbPassword;
        private bool _dbUsesWindowsAuthentication;
        private bool _silentInstall;
        private bool _includeOptionalData;

        public DialogResult ShowDialog(IWin32Window owner, string componentType, SortedDictionary<string, List<FileDownloadInfo>> files, bool installingNewUpdater, bool isMirrorServer, bool performInstallation, string dbPassword, bool dbUsesWindowsAuthentication, bool silentInstall, bool includeOptionalData) {
            _componentType = componentType;
            _files = files;
            _installingNewUpdater = installingNewUpdater;
            _isMirrorServer = isMirrorServer;
            _performInstallation = performInstallation;

            _dbPassword = dbPassword;
            _dbUsesWindowsAuthentication = dbUsesWindowsAuthentication;
            _silentInstall = silentInstall;
            _includeOptionalData = includeOptionalData;

            if (String.IsNullOrEmpty(_componentType) || _componentType.ToLower().Contains("optional")) {
                this.ShowInTaskbar = true;
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            //Utility.ActivateApplication(this.Handle);

            return this.ShowDialog(owner);
        }

        private void frmProgress_Load(object sender, EventArgs e) {
        }


        private void downloadFileFromWebServer(FileDownloadInfo downloadInfo, ref long totalBytesRead, ref long totalBytes) {
            HttpWebRequest req = HttpWebRequest.Create(downloadInfo.AbsoluteUrl) as HttpWebRequest;
            Utility.InitProxySettings(req);

            if (File.Exists(downloadInfo.FullFilePath)) {
                if (File.Exists(downloadInfo.FullFilePath + ".version")) {

                    var version = File.ReadAllText(downloadInfo.FullFilePath + ".version");

                    if (version == downloadInfo.LatestVersion) {

                        // this file is already partially downloaded.
                        // first, send a request to get the total length
                        var headReq = WebRequest.Create(downloadInfo.AbsoluteUrl) as HttpWebRequest;
                        Utility.InitProxySettings(headReq);
                        headReq.Method = "HEAD";
                        var headResp = headReq.GetResponse();

                        long contentLength = headResp.ContentLength;
                        headResp.Close();

                        // then determine how much we've already gotten locally
                        // so we can request only the remaining bytes we don't already have.
                        // (can't trust the FileInfo().Length property, need to actually open the file to get its length)
                        long fileLength = 0;
                        using (var fs = new FileStream(downloadInfo.FullFilePath, FileMode.Open, FileAccess.Read)) {
                            fileLength = fs.Length;
                        }

                        if (fileLength == contentLength) {
                            // this is already downloaded, no need to download it again.
                            totalBytesRead += fileLength;
                            downloadInfo.NeedToDownload = false;
                        } else if (fileLength < contentLength) {
                            // this must be a partial file download.
                            totalBytesRead += fileLength;
                            req.AddRange((int)fileLength, (int)contentLength - 1);
                        } else {
                            // has to be the wrong version of the file since it's larger.
                            // delete it
                            File.Delete(downloadInfo.FullFilePath);

                        }
                    } else {
                        // old version of the file -- whether it's partially downloaded or not makes no difference, we don't want that file data.
                        File.Delete(downloadInfo.FullFilePath);

                    }
                } else {
                    // no version info found -- assume it's a bogus file and start over
                    File.Delete(downloadInfo.FullFilePath);
                }
            }

            // code above may have flipped this flag so we need to check it again
            // before we actually try to download the file contents
            if (downloadInfo.NeedToDownload) {

                pbFile.Value = 0;
                pbFile.Maximum = (int)downloadInfo.SizeInKB;
                lblCurrentAction.Text = "0.0% downloaded";
                //lblOverallDownload.Text = "0.0% of all files downloaded";
                //lblOverallInstall.Text = "0.0% installed";

                Application.DoEvents();


                var resp = req.GetResponse();


                using (var fs = new FileStream(downloadInfo.FullFilePath, FileMode.Append, FileAccess.Write)) {
                    using (var stm = resp.GetResponseStream()) {
                        byte[] buf = new byte[4096];
                        int read = 0;
                        long totalFileRead = fs.Length;
                        File.WriteAllText(downloadInfo.FullFilePath + ".version", downloadInfo.LatestVersion);
                        while ((read = stm.Read(buf, 0, buf.Length)) > 0) {
                            fs.Write(buf, 0, read);
                            totalFileRead += read;
                            totalBytesRead += read;

                            decimal percentFileDone = ((decimal)totalFileRead / (decimal)downloadInfo.SizeInBytes) * 100.0M;
                            lblCurrentAction.Text = percentFileDone.ToString("##0.00") + "% downloaded";
                            int val = (int)(totalFileRead / 1024);
                            pbFile.Value = (val > pbFile.Maximum ? pbFile.Maximum : val);

                            decimal percentTotalDone = ((decimal)totalBytesRead / (decimal)totalBytes) * 100.0M; //50.0M;
                            lblOverallDownload.Text = percentTotalDone.ToString("##0.00") + "% of all files downloaded";
                            int overallVal = (int)(totalBytesRead / 1024);
                            pbOverallDownload.Value = (overallVal > pbOverallDownload.Maximum ? pbOverallDownload.Maximum : overallVal);
                            Application.DoEvents();

                            if (_cancel) {
                                break;
                            }
                        }
                    }
                }
            }

        }

        private void runBatchFile(string batchFile) {
            var psi = new ProcessStartInfo(batchFile, null);
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }

        private bool _activatedBefore;
        private void frmProgress_Activated(object sender, EventArgs e) {

            if (_activatedBefore) {
                return;

            }

            _activatedBefore = true;
            this.DialogResult = DialogResult.Cancel;
            Application.DoEvents();

            _cancel = false;

            var files = _files;

            string downloadCache = FileDownloadInfo.DownloadedCacheFolder(_componentType);
            string installCache = FileDownloadInfo.InstalledCacheFolder(_componentType);


            // determine how many files we have, how many bytes we have to downloaded
            int totalCount = 0;
            long totalBytes = 0;
            string url = null;
            foreach (var key in files.Keys) {
                totalCount += files[key].Count;
                foreach (var di in files[key]) {
                    totalBytes += di.SizeInBytes;
                    if (url == null) {
                        url = di.AbsoluteUrl.Replace(di.AppRelativeUrl.Replace("~", ""), "");
                    }
                }
            }

            int currentCount = 1;
            long totalBytesRead = 0;
            lblStatus.Text = "Initializing...";
            lblOverallDownload.Text = "Initializing...";
            lblOverallInstall.Text = "Initializing...";
            lblURL.Text = url;
            pbOverallDownload.Maximum = (int)totalBytes / 1024;
            Application.DoEvents();


            // for each row in the datagrid
            foreach (var key in files.Keys) {
                var filesToDownload = files[key];

                string filePath = (downloadCache + @"\" + key).Replace("~", "").Replace("//", @"\").Replace(@"\\", @"\") + @"\";

                // If the .msi is marked as needing downloading, the associated setup.exe will
                // also be marked as needing downloading.
                // If there is no setup.exe with the .msi, we still want to download the msi.

                // for each file associated with each row in the datagrid
                foreach (var downloadInfo in filesToDownload) {

                    downloadInfo.FullFilePath = filePath + downloadInfo.FileName;
                    Utility.ResolveFilePath(downloadInfo.FullFilePath, true);

                    lblStatus.Text = "Downloading " + downloadInfo.FileName + " (" + currentCount + " of " + totalCount + ")...";

                    if (downloadInfo.NeedToDownload) {

                        if (Utility.IsWindowsFilePath(downloadInfo.AbsoluteUrl)) {
                            // simply copy file from source path to destination path
                            if (File.Exists(downloadInfo.FullFilePath)) {
                                File.Delete(downloadInfo.FullFilePath);
                            }
                            File.Copy(downloadInfo.AbsoluteUrl, downloadInfo.FullFilePath);
                            // and the flag file so if they connect to a web server later we know which version the cached one is
                            File.WriteAllText(downloadInfo.FullFilePath + ".version", downloadInfo.LatestVersion);
                            totalBytesRead += downloadInfo.SizeInBytes;
                        } else {
                            downloadFileFromWebServer(downloadInfo, ref totalBytesRead, ref totalBytes);
                        }
                    } else {
                        totalBytesRead += downloadInfo.SizeInBytes;
                    }


                    // add to the progress bar the bytes for this file (in case download was skipped)
                    decimal percentTotalDone = ((decimal)totalBytesRead / (decimal)totalBytes) * 100.0M; //50.0M;
                    lblOverallDownload.Text = percentTotalDone.ToString("##0.00") + "% of all files downloaded";
                    int overallVal = (int)(totalBytesRead / 1024);
                    pbOverallDownload.Value = (overallVal > pbOverallDownload.Maximum ? pbOverallDownload.Maximum : overallVal);

                    currentCount++;

                    Application.DoEvents();

                    if (_cancel) {
                        break;
                    }
                }
            }

            bool installFailed = false;
            string failedInstallName = "";

            if (!_cancel) {

                lblCurrentAction.Text = "100% downloaded";
                pbFile.Value = pbFile.Maximum;

                lblOverallDownload.Text = "100% of all files downloaded";
                pbOverallDownload.Value = pbOverallDownload.Maximum;

                this.WindowState = FormWindowState.Minimized;
                Application.DoEvents();

                if (_performInstallation) {
                    btnCancel.Enabled = false;
                    Text = "Installing...";

                    if (this.Owner != null) {
                        this.Owner.WindowState = FormWindowState.Minimized;
                    }
                    Utility.ActivateApplication(this.Handle);

                    Application.DoEvents();

                    int totalInstallCount = 0;
                    int installCount = 0;
                    foreach (var key in files.Keys) {
                        foreach (var di in files[key]) {
                            if (di.NeedToInstall) {
                                totalInstallCount++;
                            }
                        }
                    }

                    pbOverallInstall.Maximum = totalInstallCount;

                    // for each row in the datagrid
                    foreach (var key in files.Keys) {
                        var filesToDownload = files[key];

                        string filePath = (downloadCache + @"\" + key).Replace("~", "").Replace("//", @"\").Replace(@"\\", @"\") + @"\";

                        // If the .msi is marked as needing downloading, the associated setup.exe will
                        // also be marked as needing downloading.
                        // If there is no setup.exe with the .msi, we still want to download the msi.

                        var installed = new List<string>();
                        var installedInfo = new List<FileDownloadInfo>();

                        // for each file associated with the current row in the datagrid
                        foreach (var downloadInfo in filesToDownload) {
                            if (downloadInfo.NeedToInstall) {
                                lblStatus.Text = "Installing " + downloadInfo.FileName;
                                lblCurrentAction.Text = "Installing " + downloadInfo.FileName;
                                this.Text = lblCurrentAction.Text;
                                lblOverallInstall.Text = "Installing...";
                                string logFileName = null;
                                Application.DoEvents();
                                if (downloadInfo.FileName.ToLower().EndsWith(".exe")) {
                                    // install it!!!
                                    if (_installingNewUpdater) {
                                        // an exe locks files when it's running -- meaning if we try to update the updater while it's running,
                                        // we're guaranteed it will fail.

                                        copyForMirrorIfNeeded(filePath, downloadInfo);
                                        var batFilePath = createBatchFileToUpdateUpdater(downloadInfo, null);
                                        runBatchFile(batFilePath);

                                        this.DialogResult = DialogResult.OK;
                                        this.Close();
                                        return;
                                    } else {
                                        string msiName = installViaSetupExe(filePath + downloadInfo.FileName, downloadInfo, out logFileName);
                                    }
                                    installed.Add(downloadInfo.FileName.ToLower().Replace(".exe", ""));

                                } else if (downloadInfo.FileName.ToLower().EndsWith(".msi")) {
                                    string file = downloadInfo.FileName.ToLower().Replace(".msi", "");
                                    if (!installed.Contains(file)) {
                                        // this is a free msi -- no .exe is associated with it.

                                        if (_installingNewUpdater) {
                                            // an exe locks files when it's running -- meaning if we try to update the updater while it's running,
                                            // we're guaranteed it will fail.

                                            copyForMirrorIfNeeded(filePath, downloadInfo);

                                            var batFilePath = createBatchFileToUpdateUpdater(null, downloadInfo);

                                            runBatchFile(batFilePath);

                                            this.DialogResult = DialogResult.OK;
                                            this.Close();
                                            return;
                                        } else {

                                            // 'normal' msi install
                                            installViaMsiExec(filePath + downloadInfo.FileName, downloadInfo, out logFileName);
                                        }
                                        //                                installedInfo.Add(downloadInfo);
                                    } else {
                                        // the exe installed it, just skip this msi
                                    }
                                } else if (downloadInfo.FileName.ToLower().EndsWith(".zip")) {

                                    // this is a .zip file, just open it in explorer
                                    // TODO: auto-unzip? distribute a .cab instead???

                                    Process.Start("explorer.exe", @"/select,""" + downloadInfo.FullFilePath + @"""");

                                } else if (downloadInfo.FileName.ToLower().EndsWith(".cab")){

                                    // auto-extract to a temp folder or prompt them to extract it somewhere?

                                    var fCab = new frmExpand();
                                    fCab.txtExpandTo.Text = Utility.GetTempDirectory(1500) + @"\" + downloadInfo.DisplayName;
                                    fCab.ShowDialog(this);

                                    var splash = new frmSplash();

                                    try {
                                        if (fCab.rdoExpand.Checked) {
                                            // expand cab file, launch
                                            var dest = Utility.ResolveDirectoryPath(fCab.txtExpandTo.Text, true);
                                            if (fCab.chkEmptyDirectory.Checked) {
                                                splash.ChangeText(getDisplayMember("activated{emptying}", "Emptying directory {0} ...", dest));
                                                Utility.EmptyDirectory(dest);
                                            }
                                            splash.ChangeText(getDisplayMember("activated{extracting}", "Extracting {0}...", downloadInfo.FileName));
                                            Utility.ExtractCabFile(downloadInfo.FullFilePath, dest, null);
                                            if (fCab.chkLaunchSetup.Checked) {
                                                var setupBat = Utility.ResolveFilePath(dest + @"\Setup.bat", false);
                                                if (File.Exists(setupBat)) {
                                                    // launch setup.bat...
                                                    Process.Start(dest + @"\Setup.bat");
                                                } else {
                                                    Process.Start("explorer.exe", @"""" + dest + @"""");
                                                }
                                            } else {
                                                Process.Start("explorer.exe", @"""" + dest + @"""");
                                            }
                                        } else {
                                            Process.Start("explorer.exe", @"/select,""" + downloadInfo.FullFilePath + @"""");
                                        }

                                    } finally {

                                        splash.Close();

                                    }
                                } else {
                                    // we don't know what this is, just display it in explorer

                                    Process.Start("explorer.exe", @"/select,""" + filePath + @"""");

                                }

                                if (downloadInfo.NeedToInstall && downloadInfo.TriedToInstall) {
                                    if (!File.Exists(logFileName)) {
                                        failedInstallName = downloadInfo.DisplayName;
                                        installFailed = true;
                                        break;
                                    } else {
                                        string logText = File.ReadAllText(logFileName);
                                        if (logText.Contains("-- Installation failed.")) {
                                            installFailed = true;
                                            failedInstallName = downloadInfo.DisplayName;
                                            break;
                                        }
                                    }
                                }


                                installCount++;
                                pbOverallInstall.Value = installCount / totalInstallCount;
                                lblOverallInstall.Text = (((decimal)installCount / (decimal)totalInstallCount) * 100.0M).ToString("##0.00") + "% installed";
                                Application.DoEvents();
                                if (_cancel) {
                                    break;
                                }
                            }
                        }

                        if (!installFailed) {
                            foreach (var di in filesToDownload) {
                                if (di.TriedToInstall) {
                                    // move from downloadcache to installercache
                                    string curFilePath = filePath + di.FileName;
                                    string newFilePath = curFilePath.Replace(downloadCache, installCache);
                                    if (File.Exists(newFilePath)) {
                                        File.Delete(newFilePath);
                                    }
                                    Utility.ResolveDirectoryPath(Directory.GetParent(newFilePath).FullName, true);
                                    File.Move(curFilePath, newFilePath);

                                    try {
                                        if (File.Exists(curFilePath + ".version")) {
                                            File.Delete(curFilePath + ".version");
                                        }
                                    } catch { 
                                        // ignore all errors, it will be overwritten as needed later anyway
                                    }

                                    if (_cancel) {
                                        break;
                                    }
                                }
                            }
                        }
                        if (_cancel || installFailed) {
                            break;
                        }
                    }


                    if (this.Owner != null) {
                        this.Owner.WindowState = FormWindowState.Normal;
                    }
                    this.WindowState = FormWindowState.Normal;
                    Application.DoEvents();
                    Utility.ActivateApplication(this.Handle);

                    if (installFailed) {
                        MessageBox.Show(getDisplayMember("activated", "Install of {0} failed.\n\nAny remaining installs have been cancelled.", failedInstallName));
                    }

                } else {
                    // no installing done, just downloading.
                    // if this is a mirror server, be sure to copy to the mirror temp dir
                    if (_isMirrorServer) {
                        foreach (var key in files.Keys) {
                            string filePath = (downloadCache + @"\" + key).Replace("~", "").Replace("//", @"\").Replace(@"\\", @"\") + @"\";
                            var filesToDownload = files[key];
                            foreach (var fdi in filesToDownload) {
                                copyForMirrorIfNeeded(filePath, fdi);
                            }
                        }
                    }
                }
            }

            this.DialogResult = (_cancel ? DialogResult.Cancel : DialogResult.OK);
        }


        private void installViaMsiExec(string msiPath, FileDownloadInfo di, out string logFileName) {


            if (di.InstalledVersion != "(not installed)") {
                // uninstall previous version first!
                UninstallExistingMsi(di.FullFilePath, di, this, true, !String.IsNullOrEmpty(_dbPassword) || _dbUsesWindowsAuthentication, _dbPassword, _dbUsesWindowsAuthentication);
            }


            //// if an existing version is installed, uninstall it first!!!
            //if (di.Child != null && (di.Child.ProductGuid != Guid.Empty || !String.IsNullOrEmpty(di.Child.UninstallString))) {
            //    // uninstall the corresponding msi
            //    uninstallExistingMsi(msiPath, di.Child);
            //}

            logFileName = msiPath + ".install_log";
            var p = new Process();

            var optionalData = "";
            var passive = "";
            if (_silentInstall) {

                passive = @" /passive ";

                if (_includeOptionalData) {
                    optionalData = @" OPTIONALDATA=""TRUE"" ";
                } else {
                    optionalData = @" OPTIONALDATA=""FALSE"" ";
                }
            }

            var encPassword = Utility.EncryptText(_dbPassword);

            p.StartInfo = new ProcessStartInfo("msiexec.exe", @" PASSWORD=""" + encPassword + @""" USEWINDOWSAUTH=""" + _dbUsesWindowsAuthentication.ToString().ToLower() + @""" AUTOLOGIN=""TRUE"" " + passive + optionalData + @" /L """ + msiPath + @""" /i """ + di.FileName + @""" ");
            p.StartInfo.WorkingDirectory = Directory.GetParent(msiPath).FullName;
            p.Start();

            if (_installingNewUpdater) {
                // an exe locks files when it's running -- meaning if we try to update the updater while it's running,
                // we're guaranteed it will fail. jump out here
                // so the exe dies off (caller is responsible for that)
                copyForMirrorIfNeeded(msiPath, di);
                return;
            }

            p.WaitForExit();

            waitForMsiToFinish(this, di.FileName);

            di.TriedToInstall = true;
            if (di.Parent != null) {
                di.Parent.TriedToInstall = true;
            }

            copyForMirrorIfNeeded(msiPath, di);

        }

        public static void UninstallExistingMsi(string exeOrMsiFullPath, FileDownloadInfo fdi, Form parentForm, bool silentMode, bool autoLogin, string password, bool useWindowsAuth) {

            var splash = new frmSplash();
            try {
                var p = new Process();

                if (fdi.ProductGuid == Guid.Empty && String.IsNullOrEmpty(fdi.UninstallString)) {
                    // this FileDownloadInfo represents the 'new' installer file, and it was not looked up from the registry.
                    var apps = Utility.ListInstalledApplications();
                    apps.TryGetValue(fdi.DisplayName, out fdi);
                    // now, fdi represents the currently installed version info, meaning the uninstall info, if any, should be present.
                }

                if (fdi.ProductGuid != Guid.Empty) {
                    splash.ChangeText(getDisplayMember("UninstallExistingMsi{start}", "Uninstalling version {0} of {1}, please be patient...", fdi.InstalledVersion, fdi.DisplayName));
                    //string logFileName = exeOrMsiFullPath + ".uninstall_log";
                    // "B" format in following line means:
                    //      32 digits separated by hyphens, enclosed in brackets: 
                    //      {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx} 
                    //p.StartInfo = new ProcessStartInfo("msiexec.exe", " " + (silentMode ? " /passive " : "") + (autoLogin ? @" AUTOLOGIN=""TRUE"" " : "") + @" USEWINDOWSAUTH=""" + useWindowsAuth.ToString().ToUpper() + @""" PASSWORD=""" + password + @""" /L*v """ + logFileName + @""" /X" + fdi.ProductGuid.ToString("B"));

                    var encPassword = Utility.EncryptText(password);

                    p.StartInfo = new ProcessStartInfo("msiexec.exe", " " + (silentMode ? " /passive " : "") + (autoLogin ? @" AUTOLOGIN=""TRUE"" " : "") + @" USEWINDOWSAUTH=""" + useWindowsAuth.ToString().ToUpper() + @""" PASSWORD=""" + encPassword + @""" /X" + fdi.ProductGuid.ToString("B"));
                    p.Start();
                    p.WaitForExit();
                    waitForMsiToFinish(parentForm, "/X" + fdi.ProductGuid.ToString("B"));

                    //                waitForMsiToFinish(parentForm, fdi.FileName ?? fdi.ProductGuid.ToString("B"));
                } else if (!String.IsNullOrEmpty(fdi.UninstallString)) {
                    splash.ChangeText(getDisplayMember("UninstallExistingMsi{start}", "Uninstalling version {0} of {1}, please be patient...", fdi.InstalledVersion, fdi.DisplayName));
                    p.StartInfo = new ProcessStartInfo("cmd.exe", " /k " + fdi.UninstallString);
                    p.Start();
                    p.WaitForExit();
                }
            } finally {
                splash.Close();
            }


        }


        private string createBatchFileToUpdateUpdater(FileDownloadInfo fdiExe, FileDownloadInfo fdiMsi) {
            string folderPath = Utility.ResolveDirectoryPath(@"*Application Data*\GRIN-Global\Updater\Reinstall", true);
            string batfilePath = Utility.ResolveFilePath(folderPath + @"\reinstall.bat", true);

            string guid = "";
            string newVersionCommand = "";
            string newMsiPath = "";
            if (fdiExe != null && fdiExe.Child != null) {
                newMsiPath = fdiExe.Child.FullFilePath;
                if (fdiExe.Child.ProductGuid == Guid.Empty) {
                    var apps = Utility.ListInstalledApplications();
                    apps.TryGetValue(fdiExe.Child.DisplayName, out fdiMsi);
                    if (fdiMsi != null) {
                        guid = fdiMsi.ProductGuid.ToString("B");
                    }
                } else {
                    guid = fdiExe.Child.ProductGuid.ToString("B");
                }
            } else if (fdiMsi != null) {
                newMsiPath = fdiMsi.FullFilePath;
                if (fdiMsi.ProductGuid == Guid.Empty) {
                    var apps = Utility.ListInstalledApplications();
                    apps.TryGetValue(fdiMsi.DisplayName, out fdiMsi);
                    if (fdiMsi != null) {
                        guid = fdiMsi.ProductGuid.ToString("B");
                    }
                } else {
                    guid = fdiMsi.ProductGuid.ToString("B");
                }
            }

            newVersionCommand = @"start /wait msiexec.exe /passive /i """ + newMsiPath + @"""";

            var skipUninstall = false;
            if (String.IsNullOrEmpty(guid)) {
                //throw new InvalidOperationException("Could not locate existing Updater installation guid.\nPlease manually reinstall GRIN-Global Updater.");
                skipUninstall = true;
            } else if (String.IsNullOrEmpty(newVersionCommand)){
                throw new InvalidOperationException(getDisplayMember("createBatchFileToUpdateUpdater{noversion}", "Could not determine command to run for new Updater installation.\nPlease manually reinstall GRIN-Global Updater."));
            }

            string todo = String.Format(@"
@ECHO OFF

REM HACK: need to give existing updater process a bit to shutdown
REM       no 'good' way to do this in native batch file across all
REM       platforms, so we fudge it with pinging localhost for a spell

ping localhost -n 3 >NUL 2>&1

cd /d ""{0}""

REM possibly uninstall existing version
{1}echo uninstalling existing version ...
{1}start /wait msiexec.exe /X{2} /passive

REM pause for a bit using the ping trick again...
REM pause
{1}echo waiting for processing to complete ...
{1}ping localhost -n 3 >NUL 2>&1

IF EXIST ""{5}"" GOTO DOINSTALL
echo Could not find msi file {5} to install!
pause
exit

:DOINSTALL
REM install new version
echo Installing new version ...
REM echo {3} ...
{3}

REM pause for a bit using the ping trick again...
REM pause
echo Waiting for processing to complete ...
ping localhost -n 3 >NUL 2>&1

echo Launching new version of Updater ...
REM at: {4}
start ""GRIN-Global Updater"" ""{4}"" /conn last /refresh
", folderPath, skipUninstall ? "REM " : "", guid, newVersionCommand, Application.ExecutablePath, newMsiPath);


            File.WriteAllText(batfilePath, todo);

            return batfilePath;
        }

        private string installViaSetupExe(string setupExePath, FileDownloadInfo di, out string logFileName) {

            if (di.InstalledVersion != "(not installed)"){
                // uninstall previous version first!
                UninstallExistingMsi(di.FullFilePath, di, this, true, !String.IsNullOrEmpty(_dbPassword) || _dbUsesWindowsAuthentication, _dbPassword, _dbUsesWindowsAuthentication);
            }

            string url = "";

            logFileName = setupExePath + ".install_log";
            var p = new Process();

            var optionalData = "";
            var passive = "";
            if (_silentInstall) {

                passive = @" /passive ";

                if (_includeOptionalData) {
                    optionalData = @" OPTIONALDATA=""TRUE"" ";
                } else {
                    optionalData = @" OPTIONALDATA=""FALSE"" ";
                }
            }

            var encPassword = Utility.EncryptText(_dbPassword);

            p.StartInfo = new ProcessStartInfo(setupExePath, url + @" PASSWORD=""" + encPassword + @""" USEWINDOWSAUTH=""" + _dbUsesWindowsAuthentication.ToString().ToLower() + @"""  AUTOLOGIN=""TRUE"" " + passive + optionalData + @" /L*v """ + setupExePath.Replace(@"\", @"\\") + @".install_log"" ");
            p.Start();

            if (_installingNewUpdater) {
                // an exe locks files when it's running -- meaning if we try to update the updater while it's running,
                // we're guaranteed it will fail.
                copyForMirrorIfNeeded(Directory.GetParent(setupExePath).FullName, di);
                return null;
            }


            p.WaitForExit();

            // ASSUMPTION: all setup.exe are renamed to coincide with their corresponding msi file...
            string msiName = di.FileName.ToLower().Replace(".exe", ".msi");
            waitForMsiToFinish(this, msiName);
            di.TriedToInstall = true;
            if (di.Child != null) {
                di.Child.TriedToInstall = true;
            }

            copyForMirrorIfNeeded(Directory.GetParent(setupExePath).FullName, di);

            return msiName;

        }

        private void copyForMirrorIfNeeded(string sourceFolder, FileDownloadInfo di) {
            // copy from installercache to somewhere under localhost/gringlobal/uploads folder
            if (_isMirrorServer) {
                // note we copy the parent and child as well, if either exists
                // if we install the setup.exe, we get the corresponding .msi
                // if we install the .msi, we get the corresponding .exe.
                
                if (di.Parent != null){
                    copyForMirror(sourceFolder, di.Parent);
                }
                
                copyForMirror(sourceFolder, di);

                if (di.Child != null){
                    copyForMirror(sourceFolder, di.Child);
                }
            }
        }

        private void copyForMirror(string sourceFolder, FileDownloadInfo di){

            // we copy directly to the web app uploads folder if possible.
            // if the web app doesn't exist, we copy it to the mirror_installers temp folder
            // so later when the web app is installed, it'll slurp the files up into the uploads folder.

            var destPath = Utility.ResolveDirectoryPath(Utility.GetTempDirectory(500) + @"\mirror_installers", true);
            string actualDestPath = "";
            try {
                var ggWebAppPath = Utility.GetIISPhysicalPath("gringlobal");
                if (!String.IsNullOrEmpty(ggWebAppPath)) {
                    // copying directly to the web server folder
                    destPath = ggWebAppPath;
                    actualDestPath = Utility.ResolveFilePath(destPath + di.AppRelativeUrl.Replace("~/", @"\").Replace(@"\\", @"\"), true);
                } else {
                    // copying to the mirrors_installers cache folder for later
                    actualDestPath = Utility.ResolveFilePath(destPath + di.AppRelativeUrl.Replace("~/uploads/installers/", @"\").Replace(@"\\", @"\"), true);
                }
            } catch {
                // copying to the mirrors_installers cache folder for later
                actualDestPath = Utility.ResolveFilePath(destPath + di.AppRelativeUrl.Replace("~/uploads/installers/", @"\").Replace(@"\\", @"\"), true);
            }

            var actualSourcePath = Utility.ResolveFilePath(sourceFolder + @"\" + di.FileName, false);
            if (File.Exists(actualSourcePath)) {
                if (File.Exists(actualDestPath)) {
                    File.Delete(actualDestPath);
                }
                File.Copy(actualSourcePath, actualDestPath);

                //using (GUISvc.GUI gui = new GrinGlobal.Updater.GUISvc.GUI()) {
                //    // TODO: add the file mapping if needed...
                //}

            }
        }

        private static void waitForMsiToFinish(Form parentForm, string msiName) {

            // this method assumes at least one msiexec.exe has already been launched and finished

            msiName = msiName.ToLower();

            var msiprocesses = new List<Process>();

            // HACK: check for up to 10 seconds before we give up inspecting the currently running processes for information 
            //       (this is ugly, but apparently necessary as sometimes it'll zoom right by if we dont build this delay in)
            var finished = DateTime.Now.AddSeconds(10).Ticks;
            bool done = false;
//            var timedOut = false;
            while (!done) {
                msiprocesses = new List<Process>();
                foreach (var any in Process.GetProcesses()) {
                    if (any.ProcessName.Contains("msiexec")) {
                        msiprocesses.Add(any);
                    }
                }

                if (msiprocesses.Count == 0) {
                    // no more msiexec.exes are running.
                    done = true;
                    //MessageBox.Show("No msi processes!");
                } else if (msiprocesses.Count == 1) {
                    // only one msiexec.exe process was found, make sure it has the /V
                    using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(string.Format("SELECT CommandLine FROM Win32_Process WHERE ProcessId = {0}", msiprocesses[0].Id))) {
                        foreach (ManagementObject managementObject in managementObjectSearcher.Get()) {
                            string commandLine = managementObject["CommandLine"] as string;
                            if (!String.IsNullOrEmpty(commandLine)) {
                                if (commandLine.ToLower().Contains("/v")) {
                                    // this is the /V (service) process. since there are no other msiexec's running, assume the install is complete.
                                    // MessageBox.Show("One msi process, its commandline is '" + commandLine + "'");
                                    done = true;
                                }
                            }
                        }
                    }
                } else {
                    // more than one msiexec.exe process is running.
                    // this means the install is not done yet
                    done = false;
                }


                    //    using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(string.Format("SELECT CommandLine FROM Win32_Process WHERE ProcessId = {0}", any.Id))) {
                    //        foreach (ManagementObject managementObject in managementObjectSearcher.Get()) {
                    //            string commandLine = managementObject["CommandLine"] as string;
                    //            if (!String.IsNullOrEmpty(commandLine)) {
                    //                if (commandLine.ToLower().Contains(msiName)) {
                    //                    //string[] userAndDomain = new String[2];
                    //                    ////Invoke the method and populate the o var with the user name and domain
                    //                    //managementObject.InvokeMethod("GetOwner", userAndDomain as object[]);
                    //                    //Debug.Write("user=" + userAndDomain[0] + ", domain=" + userAndDomain[1]);
                    //                    msiprocesses.Add(any);
                    //                } else if (("" + any.StartInfo.UserName).ToLower() == ("" + Environment.UserName).ToLower()) {
                    //                    // if it's msiexec and belongs to the current user, add it to the list to wait for it to complete
                    //                    msiprocesses.Add(any);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    if (msiprocesses.Count > 0) {
                    //        // we found it, stop checking
                    //        done = true;
                    //        break;
                    //    }
                    //}
                //}

                //if (!done) {
                //    if (DateTime.Now.Ticks > finished) {
                //        done = true;
                //        timedOut = msiprocesses.Count < 2;
                //    }
                    //if (!parentForm.IsDisposed) {
                    //    done = true;
                    //}
                //}

                if (!done) {
                    Application.DoEvents();
                    Thread.Sleep(250);
//                    MessageBox.Show("scan for proper msiexec process failed");
                }
            }

            //// if we found a process, we must wait until it's done (but be sure to continue updating the window)
            //if (msiprocesses.Count > 0) {
            //    foreach (var p in msiprocesses) {
            //        while (!p.HasExited) {
            //            Thread.Sleep(250);
            //            Application.DoEvents();
            //        }
            //    }
            //} else {
            //    // never found it -- maybe it crashed hard or they killed it off before our checking found it.  
            //    // Either way, prevent us from flying by it w/o some kind of feedback from the user.
            //    try {
            //        MessageBox.Show(parentForm, "Click OK to continue after the current installation has completed.", "Waiting For Install To Complete");
            //    } catch {
            //        // if they exited the app before the timer ran out, this will bomb.  Ignore.
            //    }
            //}
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "frmProgress", resourceName, null, defaultValue, substitutes);
        }
    }
}
