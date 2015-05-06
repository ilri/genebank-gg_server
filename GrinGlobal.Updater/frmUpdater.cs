using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Management;
using System.Web.Services.Protocols;
using GrinGlobal.InstallHelper;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using GrinGlobal.Core;

namespace GrinGlobal.Updater {
    public partial class frmUpdater : Form {


        #region BackgroundWorker / GUI synchronization

        private BackgroundWorker _worker;
        private delegate void BackgroundCallback(object sender, DoWorkEventArgs args);

        void c_OnProgress(object sender, ProgressEventArgs pea) {
            _worker.ReportProgress(0, pea);
        }
        private void backgroundProgress(object sender, ProgressChangedEventArgs e) {
            ProgressEventArgs pea = (ProgressEventArgs)e.UserState;
            statusText.Text = pea.Message;
        }
        private void backgroundDone(object sender, RunWorkerCompletedEventArgs e) {
            if (!Thread.CurrentThread.IsBackground) {
                if (!statusText.Text.EndsWith("Done")) {
                    statusText.Text += "Done";
                }
            }

            bool promptOnError = true;
            int index = _backgroundHandles.IndexOf(sender.GetHashCode());
            if (index > -1) {
                promptOnError = _backgroundPromptOnError[index];
                _backgroundPromptOnError.RemoveAt(index);
            }

            if (e.Error != null && promptOnError) {
                string txt = getDisplayMember("backgroundDone{failed}", "Fatal error occured: {0}.", e.Error.Message);
                if (!Thread.CurrentThread.IsBackground) {
                    MessageBox.Show(txt);
                }
            }
            _backgroundHandles.Remove(sender.GetHashCode());

            if (!Thread.CurrentThread.IsBackground) {
                Cursor = Cursors.Default;
            }

        }

        private int background(string statusToShow, bool promptOnError, bool showAsBusy, BackgroundCallback callback) {
            statusText.Text = statusToShow;
            Application.DoEvents();
            _worker = new BackgroundWorker();
            _backgroundHandles.Add(_worker.GetHashCode());
            _backgroundPromptOnError.Add(promptOnError);
            _worker.WorkerReportsProgress = true;
            _worker.DoWork += new DoWorkEventHandler(callback);
            _worker.ProgressChanged += new ProgressChangedEventHandler(backgroundProgress);
            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundDone);
            _worker.RunWorkerAsync();
            if (showAsBusy) {
                Cursor = Cursors.WaitCursor;
            }
            return _worker.GetHashCode();
        }

        List<bool> _backgroundPromptOnError = new List<bool>();
        List<int> _backgroundHandles = new List<int>();

        #endregion BackgroundWorker / GUI synchronization

        private class FileDownloadPair {
            internal FileDownloadInfo Installed;
            internal FileDownloadInfo Latest;
        }

        internal GuiExtended GetGui() {
            var obj = normalizeAndSaveURI(false, cboSourceServer.Text, false);
            if (obj is GuiExtended) {
                return ((GuiExtended)obj);
            } else {
                return null;
            }
        }

        private string getDefaultURL() {
            // use value from registry, if any
            var ret = Utility.GetRegSetting("LastURL", null);
            if (String.IsNullOrEmpty(ret)) {

                try {
                    // if we're executing from the source dir and offline_install.xml exists there, use it...
                    var sourceDir = Utility.GetSourceDirectory(null, null, "Updater");
                    if (!String.IsNullOrEmpty(sourceDir) && Utility.IsWindowsFilePath(sourceDir)) {
                        var localPath = Utility.ResolveFilePath(sourceDir + @"\offline_install.xml", false);
                        if (File.Exists(localPath)) {
                            ret = localPath;
                            saveUrlToFile(ret);
                        }
                    }

                    if (String.IsNullOrEmpty(ret)) {
                        // otherwise use the web.config url (or fall back to the hardcoded one if web.config doesn't have one)
                        ret = Utility.GetSetting("DefaultDownloadURL", "http://distribution.grin-global.org/gringlobal/gui.asmx");
                    }
                } catch (Exception exIgnore) {
                    // if device is not ready (i.e. original source was CD and it's no longer attached, network cannot be reached, etc) just ignore and assume test1 server as the default.
                    Debug.WriteLine(exIgnore.Message);
                }

            }
            return ret;
        }

        private string _autoDownloadUrl;
        public string AutoDownloadUrl {
            get { return _autoDownloadUrl; }
            set {
                if (("" + value).ToLower() == "last") {
                    // use the last-connected-to url, which is stored 'somewhere'...
                    _autoDownloadUrl = getDefaultURL();
                } else {
                    _autoDownloadUrl = value;
                }

            }
        }
        public string AutoDownloadGroup;
        public List<string> AutoDownloadFiles;

        public bool RefreshGroups;

        public frmUpdater() {
            InitializeComponent();
        }

        public bool IsAutoDownloading() {
            return !String.IsNullOrEmpty(AutoDownloadUrl) && !String.IsNullOrEmpty(AutoDownloadGroup);
        }

        private void Form1_Load(object sender, EventArgs e) {

            lblClientRequirements.Text = "";
            lblServerRequirements.Text = "";


            var lastUrl = loadUrlsFromFile();

            _installedApplications = Utility.ListInstalledApplications();

//            this.Text = Utility.GetInstalledVersion("GRIN-Global Updater");
            this.Text = Utility.GetApplicationVersion("GRIN-Global Updater");

            if (IsAutoDownloading()) {
                var splash = new frmQueryingServer();
                try {



//                    Utility.ActivateApplication(this.Handle);
                    // auto-download specified files
                    // moved to AutoDownloadUrl property
                    //if (AutoDownloadUrl.ToLower() == "last") {
                    //    // use the last-connected-to url, which is always the default one in the combo box.
                    //    AutoDownloadUrl = Utility.GetRegSetting("LastURL", lastUrl);
                    //}

                    splash.Show(AutoDownloadUrl);
                    splash.Refresh();
                    Application.DoEvents();

                    Utility.ActivateApplication(splash.Handle);

                    cboSourceServer.Text = AutoDownloadUrl;

                    var componentType = "Optional Data";

                    var allFiles = GetLatestFileInfo(this.AutoDownloadGroup, componentType, "Checking server for files...", true, cboSourceServer.Text);

                    var fileNames = new List<string>();
                    var filesToDownload = new List<FileDownloadInfo>();
                    for (int i = 0; i < allFiles.Count; i++) {
                        var fdi = allFiles[i];
                        // only add in those files which the user specified
                        if (AutoDownloadFiles == null || AutoDownloadFiles.Count == 0) {
                            // none specified, pull all.
                            filesToDownload.Add(fdi);
                        } else {
                            // at least one specified, filter as needed
                            foreach (var s in AutoDownloadFiles) {
                                if (fdi.DisplayName.Trim().ToLower() == s.Trim().ToLower()
                                        || fdi.FileName.Trim().ToLower() == s.Trim().ToLower()) {
                                    fileNames.Add(fdi.FileName);
                                    filesToDownload.Add(fdi);
                                }
                            }
                        }
                    }


                    //MessageBox.Show("files to download: " + String.Join(", ", fileNames.ToArray()));


                    var fileDic = new SortedDictionary<string, List<FileDownloadInfo>>();
                    fileDic.Add(AutoDownloadGroup, filesToDownload);
                    splash.Close();

                    var success = downloadAndOptionallyInstall(componentType, null, fileDic, false, null, false, false, false);

                    if (success) {
                        var sb = new StringBuilder();
                        foreach (var f in filesToDownload) {
                            sb.Append("|" + f.FullFilePath);
                        }
                        Console.WriteLine("FILES: " + sb.ToString());
                    } else {
                        Console.WriteLine("ERROR: Download failed or was cancelled.");
                    }
                } catch (Exception ex) {
                    Console.WriteLine("ERROR: Download failed. Reason: " + ex.Message);
                    MessageBox.Show(getDisplayMember("formload{failed}", "ERROR: Download failed. Reason: {0}\n\nAutoDownloadURL={1}\nAutoDownloadGroup={2}\nAutoDownloadFiles={3}", 
                        ex.Message,  AutoDownloadUrl , AutoDownloadGroup ,
                        String.Join(", ", AutoDownloadFiles.ToArray())));
                } finally {
                    splash.Close();
                    Application.Exit();
                }

                // hack so form_activate code is skipped
                _activated = true;
            } else if (RefreshGroups) {
                cboSourceServer.Text = AutoDownloadUrl;
            }


        }

        private void refreshInstalledAppsList() {
            _installedApplications = Utility.ListInstalledApplications();
        }

        SortedDictionary<string, List<FileDownloadInfo>> _serverFilesToDownload = null;
        SortedDictionary<string, List<FileDownloadInfo>> _clientFilesToDownload = null;
        
        private void btnCheckForServerUpdates_Click(object sender, EventArgs e) {
            refreshInstalledAppsList();
            _serverFilesToDownload = null;
            try {
                this.Cursor = Cursors.WaitCursor;
                btnDownloadServer.Enabled = false;
                bool? haveLatest = syncGridView(dgvServerComponents, "GRIN-Global Server Full", "Server", out _serverFilesToDownload);
                if (haveLatest == false){
                    // MessageBox.Show(this, "You have the latest version of GRIN-Global Server available.");
                } else if (haveLatest == null) {
                    // don't show anything, they've already displayed an informational messagebox saying the server could not be contacted.
                } else {
                    btnDownloadServer.Enabled = true;
                }
                refreshServerRequirements();
            } catch (UriFormatException ufe) {
                MessageBox.Show(getDisplayMember("ServerCheck{baduri}", "The specified url appears to be invalid:\n{0}", ufe.Message));
            } catch (SoapException se) {
                MessageBox.Show(getDisplayMember("ServerCheck{badsoap}", "The server did not understand your request.\n\nDetails:\n{0}", se.Message));
            } catch (WebException we) {
                MessageBox.Show(we.Message);
            } finally {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnCheckForClientUpdates_Click(object sender, EventArgs e) {
            refreshInstalledAppsList();
            _clientFilesToDownload = null;
            try {
                this.Cursor = Cursors.WaitCursor;
                btnDownloadClient.Enabled = false;
                bool? haveLatest = syncGridView(dgvClientComponents, "GRIN-Global Client", "Client", out _clientFilesToDownload);
                if (haveLatest == false) {
                    // MessageBox.Show(this, "You have the latest version of GRIN-Global Client available.");
                } else if (haveLatest == null) {
                    // don't show anything, they've already displayed an informational messagebox saying the server could not be contacted.
                } else {
                    btnDownloadClient.Enabled = true;
                }
                refreshClientRequirements();

            } catch (UriFormatException ufe) {
                MessageBox.Show(getDisplayMember("ClientCheck{baduri}", "The specified url appears to be invalid:\n{0}", ufe.Message));
            } catch (SoapException se) {
                MessageBox.Show(getDisplayMember("ClientCheck{badsoap}", "The server did not understand your request.\n\nDetails:\n{0}", se.Message));
            } catch (WebException we) {
                MessageBox.Show(we.Message);
            } finally {
                this.Cursor = Cursors.Default;
            }
        }

        private string loadUrlsFromFile() {

            lock (_locker) {
                //string defaultURL = Utility.GetSetting("DefaultDownloadURL", "http://test.grin-global.org/gringlobal/gui.asmx");
                string defaultURL = getDefaultURL();

                // if we're installing from a local directory, the offline_install.xml file will reside in same folder as the setup exe file...
                // so we should default to it as the source instead of assuming test1 server.
                try {
                    var sourceDir = Utility.GetSourceDirectory(null, null, "Updater");
                    if (!String.IsNullOrEmpty(sourceDir) && Utility.IsWindowsFilePath(sourceDir)) {
                        var localPath = Utility.ResolveFilePath(sourceDir + @"\offline_install.xml", false);
                        if (File.Exists(localPath)) {
                            defaultURL = localPath;
                            saveUrlToFile(defaultURL);
                        }
                    }
                } catch (Exception exIgnore) {
                    // if device is not ready (i.e. original source was CD and it's no longer attached, network cannot be reached, etc) just ignore and assume test1 server as the default.
                    Debug.WriteLine(exIgnore.Message);
                }

                string lastURL = null;
                string urlFile = Utility.ResolveFilePath(@"*Application Data*\GRIN-Global\Updater\webserviceurl.txt", true);

                cboSourceServer.Items.Clear();
                var selectedIndex = -1;

                var foundTestServer = false;

                if (!File.Exists(urlFile)) {
                    lastURL = defaultURL;
                    cboSourceServer.Items.Add(lastURL);
                    foundTestServer = defaultURL.ToLower().Contains("distribution.grin-global.org");
                }
                else
                {
                    var lastSavedUrlDate = DateTime.MinValue;
                    var allLines = File.ReadAllLines(urlFile);
                    for (var i = 0; i < allLines.Length; i++) {
                        try {
                            var line = allLines[i];
                            if (line.ToLower().Contains("distribution.grin-global.org"))
                            {
                                foundTestServer = true;
                            }
                            string[] slots = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (slots != null && slots.Length > 0) {
                                if (!slots[0].StartsWith("#") && !slots[0].StartsWith(";") && !slots[0].StartsWith("//") && !slots[0].StartsWith("--")) {
                                    if (slots.Length == 1) {
                                        if (lastURL == null) {
                                            lastURL = slots[0];
                                        }
                                        cboSourceServer.Items.Add(slots[0]);
                                    } else {
                                        cboSourceServer.Items.Add(slots[1]);
                                        if (slots.Length >= 3) {
                                            var dt = Utility.ToDateTime(slots[2], DateTime.MinValue);
                                            if (dt > lastSavedUrlDate) {
                                                selectedIndex = i;
                                                lastSavedUrlDate = dt;
                                                lastURL = slots[1];
                                            }
                                        }
                                    }

                                }
                            }
                        } catch (Exception ex) {
                            // eat all errors
                            Debug.WriteLine(ex.Message);
                        }
                    }

                    if (cboSourceServer.Items.Count == 0) {
                        // add in test as default one if none are specified
                        lastURL = defaultURL;
                        cboSourceServer.Items.Add(lastURL);
                        foundTestServer = lastURL.ToLower().Contains("distribution.grin-global.org");
                    }

                }

                if (!foundTestServer)
                {
                    cboSourceServer.Items.Add("http://distribution.grin-global.org/gringlobal/gui.asmx");
                }

                if (selectedIndex < 0) {
                    // auto-select last one if none are selected
                    selectedIndex = cboSourceServer.Items.Count - 1;
                }
                cboSourceServer.SelectedIndex = selectedIndex;

                return lastURL;
            }
        }

        private object _locker = new object();

        /// <summary>
        /// Writes the given url to the webserviceurl.txt file as well as the LastURL registry setting.
        /// </summary>
        /// <param name="url"></param>
        private void saveUrlToFile(string url) {
            lock (_locker) {
                string urlFile = Utility.ResolveFilePath(@"*Application Data*\GRIN-Global\Updater\webserviceurl.txt", true);

                if (!File.Exists(urlFile)) {
                    using (var sw = File.CreateText(urlFile)) {
                        // use the name tab name format, same as curator tool so the same file can be used in both places (we ignore the first slot anyway)
                        sw.WriteLine(url + "\t" + url + "\t" + DateTime.UtcNow);
                    }
                    return;
                }


                string[] urls = File.ReadAllLines(urlFile);
                using (var sw2 = File.CreateText(urlFile)) {
                    bool found = false;
                    var allUrls = new List<string>();
                    foreach (var line in urls) {
                        // not the same url, just spit it back out to file (add on a datetime stamp if need be
                        string[] parts = line.Split('\t');

                        if (parts.Length == 0) {
                            // invalid line, skip it
                        } else {

                            var p0 = parts[0].Trim();
                            if (p0.StartsWith("#") || p0.StartsWith("//") || p0.StartsWith("--") || p0 == "") {
                                // commented or empty line
                                sw2.WriteLine(line);
                            } else {
                                var testUrl = parts[(parts.Length >= 2 ? 1 : 0)];
                                var lower = testUrl.ToLower();

                                if (lower == url.ToLower()) {
                                    found = true;
                                }


                                if (allUrls.Contains(lower)) {
                                    // already in the file, skip it
                                } else {
                                    switch (parts.Length) {
                                        case 1:
                                            // no friendly name or timestamp, add them in the past
                                            sw2.WriteLine(parts[0] + '\t' + parts[0] + '\t' + DateTime.UtcNow.AddHours(-1));
                                            break;
                                        case 2:
                                            // no tiemstamp, add it in the past
                                            sw2.WriteLine(parts[0] + '\t' + parts[1] + '\t' + DateTime.UtcNow.AddHours(-1));
                                            break;
                                        default:
                                            if (lower == url.ToLower()) {
                                                // selected one, mark it as most recent
                                                sw2.WriteLine(parts[0] + '\t' + parts[1] + '\t' + DateTime.UtcNow);
                                            } else {
                                                // not selected one, just spit it back out
                                                sw2.WriteLine(line);
                                            }
                                            break;
                                    }
                                    // remember it for next iteration
                                    allUrls.Add(lower);
                                }
                            }
                        }
                    }
                    if (!found) {
                        // url didn't exist in the file, append it (and timestamp it)
                        sw2.WriteLine(url + "\t" + url + "\t" + DateTime.UtcNow);
                    }
                }

                // save it to the registry so others can see the last url we connected to.
                Utility.SetRegSetting("LastURL", url, false);
            }

        }

        public bool _autoFormatDownloadServerUrl = Utility.GetRegSetting("AutoFormatURL", 1) == 1;
        public bool _isMirrorServer = Utility.GetRegSetting("IsMirrorServer", 1) == 1;
        public bool _mirrorCD = Utility.GetRegSetting("MirrorCD", 0) == 1;


        private object normalizeAndSaveURI(bool addIfValid, string url, bool showException){

            string newUrl = url.ToLower();
            //MessageBox.Show("normalizing original url of '" + newUrl + "'... (IsWindowsFilePath=" + Utility.IsWindowsFilePath(newUrl) + ")");
            if (Utility.IsWindowsFilePath(newUrl)) {
                // is a local drive or UNC path.
                //if (!newUrl.ToLower().EndsWith("ggfiles.xml")) {
                //    newUrl += @"\ggfiles.xml";
                //}
                var localFile = Utility.ResolveFilePath(newUrl, false);
                if (File.Exists(localFile)) {
                    // always save to file so we know which url was used last...
                    saveUrlToFile(url);

                    return localFile;

                } else {
                    return null;
                }
            } else {
                // assume it's an http url or server and reformat it properly if needed
                return reformatAndSaveUrl(addIfValid, url, showException);
            }

        }

        private GuiExtended reformatAndSaveUrl(bool addIfValid, string url, bool showException) {

            GuiExtended gui = new GuiExtended();
            gui.WebRequestCreatedCallback = delegate(HttpWebRequest req) {
                Utility.InitProxySettings(req);
            };
            string newUrl = url.ToLower();

            if (_autoFormatDownloadServerUrl) {
                if (!newUrl.StartsWith("http://") & !newUrl.StartsWith("https://")) {
                    newUrl = "http://" + newUrl;
                }
                if (!newUrl.Contains("gui.asmx")) {
                    if (newUrl.EndsWith("/")) {
                        newUrl += "gui.asmx";
                    } else {
                        newUrl += "/gui.asmx";
                    }
                }
                if (!newUrl.Contains("/gringlobal")) {
                    newUrl = newUrl.Replace("/gui.asmx", "/gringlobal/gui.asmx");
                }
            }

            try {
                gui.Url = newUrl;
                if (addIfValid) {
                    gui.GetVersion();
                    // it's a valid url, assign it back out to the combobox so they can see how we tweaked it
                    if (!cboSourceServer.Items.Contains(newUrl)) {
                        cboSourceServer.Items.Add(newUrl);
                        cboSourceServer.SelectedIndex = cboSourceServer.Items.Count - 1;
                    }

                    // always save to file so we know which url was used last...
                    saveUrlToFile(url);
                }
                return gui;
            } catch (UriFormatException ufe){
                Debug.WriteLine(ufe.Message);
                if (showException) {
                    MessageBox.Show(getDisplayMember("reformatAndSaveUrl{baduri}", "The url '{0}' is formatted improperly.\nPlease modify it and try again.\n\nError detail: {1}", newUrl, ufe.Message));
                    cboSourceServer.Focus();
                    cboSourceServer.Select();
                    return null;
                } else {
                    return gui;
                }
            } catch (Exception ex) {
                if (showException) {
                    Debug.WriteLine(ex.Message);
                    MessageBox.Show(getDisplayMember("reformatAndSaveUrl{failed}", "The url '{0}' does not point to a valid GRIN-Global server.\nPlease modify it and try again.\n\nError detail: {1}", newUrl , ex.Message));
                    cboSourceServer.Focus();
                    cboSourceServer.Select();
                    return null;
                } else {
                    return gui;
                }
            }

        }

        private DataSet GetLocalFileInfo(string groupName, string pathToXmlFile) {
            DataSet ds = new DataSet();
            var rootFolder = Path.GetDirectoryName(pathToXmlFile); //.Replace("ggfiles.xml", "");
            ds.ExtendedProperties.Add("url", rootFolder);
            var dt = new DataTable("file_info");
            dt.Columns.Add("display_name", typeof(string));
            dt.Columns.Add("file_version", typeof(string));
            dt.Columns.Add("file_name", typeof(string));
            dt.Columns.Add("virtual_file_path", typeof(string));
            dt.Columns.Add("virtual_webapp_file_path", typeof(string));

            ds.Tables.Add(dt);

            var nav = new XPathDocument(pathToXmlFile).CreateNavigator();
            var webAppPath = nav.SelectSingleNode("/Groups/@WebRelativePath").Value;
            if (string.IsNullOrEmpty(webAppPath)) {
                webAppPath = "~/uploads/installers/latest/";
            }
            var it = nav.Select("/Groups/Group[@Name='" + groupName + "']/File");
            while (it.MoveNext()) {

                var relativePath = it.Current.GetAttribute("RelativePath", "");
                var fullPath = Utility.ResolveFilePath(rootFolder + @"\" + relativePath.Replace("~", ""), false);

                if (!File.Exists(fullPath)) {
                    throw new FileNotFoundException(getDisplayMember("GetLocalFileInfo{nofile}", "File '{0}' could not be found", fullPath ));
                }

                var dr = dt.NewRow();
                dr["display_name"] = it.Current.GetAttribute("DisplayName", "");

                dr["file_name"] = new FileInfo(fullPath).Name;

                dr["virtual_file_path"] = relativePath;
                dr["virtual_webapp_file_path"] = relativePath.Replace("~/", webAppPath);

                var version = ""; // it.Current.GetAttribute("Version", "");
                if (relativePath.ToLower().EndsWith(".msi")) {
                    version = Utility.GetProductVersion(fullPath);
                } else {
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fullPath);
                    version = fvi.ProductVersion;
                }

                dr["file_version"] = version;
                dt.Rows.Add(dr);
            }

            return ds;

        }


        public List<FileDownloadInfo> GetLatestFileInfo(string groupName, string componentType, string statusToShow, bool promptOnError, string url) {

            var sbMissing = new StringBuilder();
            var sbOtherError = new StringBuilder();
            if (url == null) {
                url = cboSourceServer.Text;
            }

            List<FileDownloadInfo> files = new List<FileDownloadInfo>();

            object source = normalizeAndSaveURI(promptOnError, url, true);
            if (source is GuiExtended) {
                if (source == null) {
                    return new List<FileDownloadInfo>();
                }
            }

            int handle = background(statusToShow, promptOnError, false, (sentBy, args) => {
                if (source is GuiExtended) {
                    var gui = source as GuiExtended;
                    DataSet ds = gui.GetFileInfo(false, groupName, null, true, true);

                    // spin through file_info table, add / update grid as needed
                    // first, get a list of all files that we're going to inspect
                    foreach (DataRow row in ds.Tables["file_info"].Rows) {
                        var fdi = new FileDownloadInfo();
                        fdi.DisplayName = row["display_name"].ToString();
                        fdi.InstalledVersion = "???";
                        fdi.LatestVersion = row["file_version"].ToString();
                        fdi.FileName = row["file_name"].ToString();
                        fdi.AppRelativeUrl = row["virtual_file_path"].ToString();
                        fdi.AbsoluteUrl = gui.Url.Replace("/gui.asmx", fdi.AppRelativeUrl.Replace("~", ""));
                        fdi.Status = "-";

                        long fileSizeInBytes = Utility.ToInt64(row["file_size"], 0);
                        try {
                            var req = WebRequest.Create(fdi.AbsoluteUrl) as HttpWebRequest;
                            Utility.InitProxySettings(req);
                            req.Method = "HEAD";
                            var resp = req.GetResponse();
                            fdi.SizeInBytes = resp.ContentLength;
                            resp.Close();
                            files.Add(fdi);
                        } catch (WebException wex) {
                            var resp = wex.Response as HttpWebResponse;
                            if (resp != null && resp.StatusCode == HttpStatusCode.NotFound) {
                                sbMissing.AppendLine("    " + fdi.AppRelativeUrl);
                            } else {
                                sbOtherError.AppendLine(" - " + fdi.FileName + " " + wex.Message);
                            }
                        }


                    }

                } else if (source == null){
                    // nothing to do, file not found.
                    Debug.WriteLine("normalizeAndSaveURI() returned null for url=" + url);
                } else {
                    DataSet ds = GetLocalFileInfo(groupName, source as string);

                // spin through file_info table, add / update grid as needed
                    // first, get a list of all files that we're going to inspect
                    foreach (DataRow row in ds.Tables["file_info"].Rows) {
                        var fdi = new FileDownloadInfo();
                        fdi.DisplayName = row["display_name"].ToString();
                        fdi.InstalledVersion = "???";
                        fdi.LatestVersion = row["file_version"].ToString();
                        fdi.FileName = row["file_name"].ToString();

                        // since the offline_install.xml file states the relative path is ~/ (which is correct outside an HttpContext),
                        // we munge it a bit here so mirror server code copy works (i.e. we mark the AppRelativeUrl with the HttpContext-friendly
                        // relative path, and give the AbsoluteUrl the non-HttpContext-friendly path)
                        fdi.AppRelativeUrl = row["virtual_webapp_file_path"].ToString();

                        var actualRelativeUrl = row["virtual_file_path"].ToString().Replace("~", "");

                        fdi.AbsoluteUrl = Utility.ResolveFilePath(Path.GetDirectoryName(source as string) + actualRelativeUrl, false);
                        fdi.Status = "-";

                        if (File.Exists(fdi.AbsoluteUrl)) {
                            fdi.SizeInBytes = new FileInfo(fdi.AbsoluteUrl).Length;
                        }
                        files.Add(fdi);

                    }

                }

                if (sbMissing.Length > 0 || sbOtherError.Length > 0) {
                    // do not process, there were errors getting info on one or more files
                } else {


                    // now, we know all the files that are associated with the current grid.
                    // determine which ones are msi/exe pairs and which ones are not
                    foreach (var fdi in files) {
                        if (fdi.FileName.ToLower().EndsWith(".exe")) {
                            string noExtension = fdi.FileName.ToLower().Replace(".exe", "");
                            foreach (var sub in files) {
                                if (sub.FileName.ToLower().Replace(".msi", "") == noExtension) {
                                    sub.Parent = fdi;
                                    fdi.Child = sub;
                                    break;
                                }
                            }
                        }


                    }

                    foreach (var fdi in files) {

                        // now see if we need to download it
                        if (fdi.Child == null) {
                            // if this has a child, it's probably a .exe with a .msi.
                            // let the .msi handle the versioning.
                            // otherwise, default to needing to download then back off from there.
                            fdi.NeedToDownload = true;
                        }

                        // see if it exists in the download cache already (so we don't force them to redownload something)
                        string filePath = ((FileDownloadInfo.DownloadedCacheFolder(componentType) + @"\" + fdi.DisplayName + @"\").Replace("~", "").Replace("//", @"\").Replace(@"\\", @"\") + @"\").Replace(@"\\", @"\");
                        string downloadedFileName = filePath + fdi.FileName;

                        if (File.Exists(downloadedFileName)) {
                            // see if the length matches exactly, as it may be only partially downloaded.
                            using (var fs = new FileStream(downloadedFileName, FileMode.Open, FileAccess.Read)) {
                                if (fs.Length == fdi.SizeInBytes) {
                                    fdi.NeedToDownload = false;
                                }
                            }

                            // if version info doesn't match, we need to delete the locally cached version
                            // and mark the new one for download.
                            string prodVersion = "";
                            try {
                                prodVersion = Utility.GetProductVersion(downloadedFileName);
                            } catch {
                                // eat all errors when getting prodVersion, the msi might be incomplete and not able to be opened properly.
                            }
                            if (!String.IsNullOrEmpty(prodVersion) && fdi.LatestVersion != prodVersion) {
                                fdi.NeedToDownload = true;
                                if (File.Exists(downloadedFileName)) {
                                    File.Delete(downloadedFileName);
                                }
                                if (File.Exists(downloadedFileName + ".version")) {
                                    File.Delete(downloadedFileName + ".version");
                                }
                                if (fdi.Parent != null) {
                                    fdi.Parent.NeedToDownload = true;
                                    if (File.Exists(filePath + fdi.Parent.FileName)) {
                                        File.Delete(filePath + fdi.Parent.FileName);
                                    }
                                    if (File.Exists(filePath + fdi.Parent.FileName + ".version")) {
                                        File.Delete(filePath + fdi.Parent.FileName + ".version");
                                    }
                                }
                                if (fdi.Child != null) {
                                    fdi.Child.NeedToDownload = true;
                                    if (File.Exists(filePath + fdi.Child.FileName)) {
                                        File.Delete(filePath + fdi.Child.FileName);
                                    }
                                    if (File.Exists(filePath + fdi.Child.FileName + ".version")) {
                                        File.Delete(filePath + fdi.Child.FileName + ".version");
                                    }
                                }
                            }

                        }


                        if (fdi.Child != null) {

                            // nothing to do, let child handle things


                        } else if (fdi.AbsoluteUrl.ToLower().EndsWith(".msi")) {

                            // if it's an msi, see if it's already installed.
                            // and if so, check its version.


                            FileDownloadInfo appInfo = null;
                            if (_installedApplications.TryGetValue(fdi.DisplayName, out appInfo)) {
                                fdi.InstalledVersion = appInfo.InstalledVersion;
                            } else {
                                fdi.InstalledVersion = "(not installed)";
                            }

                            fdi.Status = "Current";

                            if (fdi.IsInstalledOlderThanLatest()) {

                                if (fdi.NeedToDownload) {
                                    fdi.Status = "Outdated";
                                } else {
                                    fdi.Status = "Downloaded";
                                }

                                // we know we need to install this one -- but we might have to download it first if it's not in the download cache...
                                if (fdi.Parent != null) {
                                    // if an .exe is associated with this msi,
                                    // do not mark this msi as needing to be installed as the .exe will do it for us.
                                    fdi.Parent.NeedToInstall = true;
                                    fdi.Parent.InstalledVersion = fdi.InstalledVersion;
                                    fdi.Parent.NeedToDownload = String.IsNullOrEmpty(fdi.Parent.FullFilePath) || !File.Exists(fdi.Parent.FullFilePath);
                                    fdi.Parent.Status = fdi.Status;
                                    fdi.NeedToInstall = false;
                                } else {
                                    // no .exe associated with this msi, so we need to install it
                                    fdi.NeedToInstall = true;
                                }

                            } else {
                                // latest version is installed. 
                                fdi.NeedToInstall = false;
                                fdi.NeedToDownload = false;

                                // also mark parent, if any, as not needing to be downloaded or installed.
                                if (fdi.Parent != null) {
                                    fdi.Parent.NeedToDownload = false;
                                    fdi.Parent.NeedToInstall = false;
                                }
                                if (fdi.Child != null) {
                                    fdi.Child.NeedToDownload = false;
                                    fdi.Child.NeedToInstall = false;
                                }
                            }


                        } else {

                            // this is a non-msi file (probably .exe) that does not have a dependent .msi file.
                            // we have no good way to check versioning
                            // so we just always install it

                            fdi.NeedToInstall = true;


                        }
                    }

                }

            });

            while (_backgroundHandles.Contains(handle)) {
                // wait for background to finish before returning
                // (we do this mainly so the request isn't on the main thread and the gui doesn't freeze up --
                //  logically it's the same as a synchronous request since we're doing a cheesy spin wait here)
                Thread.Sleep(500);
                Application.DoEvents();
            }

            if ((sbMissing.Length > 0 || sbOtherError.Length > 0) && promptOnError) {
                var output = "";
                if (sbMissing.Length > 0) {
                    output += getDisplayMember("GetLatestFileInfo{filenotfound}", "The following files were not found on the server at the appropriate path:\r\n\r\n{0}\r\n", sbMissing.ToString());
                }
                if (sbOtherError.Length > 0) {
                    output += getDisplayMember("GetLatestFileInfo{othererror}", "\r\nError requesting information about one or more files:\r\n{0}\r\n", sbMissing.ToString());
                }
                output += getDisplayMember("GetLatestFileInfo{contactadmin}", "Please contact the server administrator.");
                MessageBox.Show(this, output, getDisplayMember("GetLatestFileInfo{title}", "File Information Request(s) Failed"));
            }
            return files;


        }

        private bool? syncGridView(DataGridView dgv, string groupName, string componentType, out SortedDictionary<string, List<FileDownloadInfo>> filesToDownloadAndInstall) {

            filesToDownloadAndInstall = new SortedDictionary<string, List<FileDownloadInfo>>();

            dgv.Rows.Clear();

            List<FileDownloadInfo> allFiles = GetLatestFileInfo(groupName, componentType, "Checking for new version of GRIN-Global " + componentType + "...", true, cboSourceServer.Text);

            if (allFiles.Count == 0) {
                return null;
            }

            // we want to keep all files in the mix to download

            // but come install time, we want to weed out those that are dependent
            // and only spin through the non-dependent ones

            try {
                decimal fs2 = 0.0M;
                foreach (var fdi in allFiles) {

                    List<FileDownloadInfo> fileList = null;
                    if (!filesToDownloadAndInstall.TryGetValue(fdi.DisplayName, out fileList)) {
                        fileList = new List<FileDownloadInfo>();
                        filesToDownloadAndInstall.Add(fdi.DisplayName, fileList);
                    }
                    fileList.Add(fdi);


                    // first, find the gridview row with this same display name (multiple files can be tied to the same display name)
                    DataGridViewRow dgvr = null;
                    foreach (DataGridViewRow r in dgv.Rows) {
                        if (r.Cells["col" + componentType + "DisplayName"].Value.ToString().ToLower() == fdi.DisplayName.ToLower()) {
                            dgvr = r;
                            break;
                        }
                    }

                    if (dgvr == null) {
                        dgvr = new DataGridViewRow();
                        int rowIndex = dgv.Rows.Add(dgvr);
                        dgvr = dgv.Rows[rowIndex];
                        dgvr.Cells["col" + componentType + "Check"].Value = "True";
                        dgvr.Cells["col" + componentType + "DisplayName"].Value = fdi.DisplayName;
                        dgvr.Cells["col" + componentType + "InstalledVersion"].Value = fdi.InstalledVersion;
                        dgvr.Cells["col" + componentType + "LatestVersion"].Value = fdi.LatestVersion;
                        dgvr.Cells["col" + componentType + "Size"].Value = fdi.SizeInMB.ToString("###,###,##0.00");
                        dgvr.Cells["col" + componentType + "Status"].Value = fdi.Status;
                        dgvr.Cells["col" + componentType + "Action"].Value = "";
                        fs2 = Utility.ToDecimal(dgvr.Cells["col" + componentType + "Size"].Value, 0.0M);
                    }
                    else
                    {
                        fs2 = Utility.ToDecimal(dgvr.Cells["col" + componentType + "Size"].Value, 0.0M) + fdi.SizeInMB;
                    }


                    if (fdi.Child != null) {

                        // nothing to do, let child handle things

                    } else if (fdi.AbsoluteUrl.ToLower().EndsWith(".msi")) {

                        fdi.Status = "Current";
                        dgvr.Cells["col" + componentType + "InstalledVersion"].Value = fdi.InstalledVersion;

                        FileDownloadInfo fdiInstalled = null;
                        if (_installedApplications.TryGetValue(fdi.DisplayName, out fdiInstalled)) {
                            dgvr.Cells["col" + componentType + "Action"].Tag = new FileDownloadPair { Installed = fdiInstalled, Latest = fdi };
                            dgvr.Cells["col" + componentType + "Action"].Value = "Uninstall";
                        } else {
                            dgvr.Cells["col" + componentType + "Action"].Tag = null;
                            dgvr.Cells["col" + componentType + "Action"].Value = "";
                        }


                    }

                    var stat = dgvr.Cells["col" + componentType + "Status"].Value as string;
                    if (stat == "-") {
                        dgvr.Cells["col" + componentType + "Status"].Value = fdi.Status;
                        stat = fdi.Status;
                    }
                    if (stat == "Outdated") {
                        dgv.Rows[dgv.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Red;
                    } else if (stat == "Downloaded") {
                        dgv.Rows[dgv.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Red;
                    }

                    var installedVersion = dgvr.Cells["col" + componentType + "InstalledVersion"].Value as string;
                    if (installedVersion == "???") {
                        dgvr.Cells["col" + componentType + "InstalledVersion"].Value = fdi.InstalledVersion;
                    }

                    if (fdi.Parent != null) {
                        dgvr.Cells["col" + componentType + "LatestVersion"].Value = fdi.LatestVersion;
                    }


                    // add to the filesize...
                    //var fs2 = Utility.ToDecimal(dgvr.Cells["col" + componentType + "Size"].Value, 0.0M) + fdi.SizeInMB;
                    dgvr.Cells["col" + componentType + "Size"].Value = fs2.ToString("###,###,##0.00");

                }

                foreach (DataGridViewRow dgvr in dgv.Rows) {
                    if ((dgvr.Cells["col" + componentType + "Status"].Value as string) == "Current") {
                        (dgvr.Cells[0] as DataGridViewCheckBoxCell).Value = "False";
                    }
                }

                foreach (var fdi in allFiles) {
                    if (fdi.NeedToInstall) {
                        return true;
                    }
                }
            } catch (Exception ex) {
                // any exceptions that happen here are due to the app shutting down in th emiddle of processing a call.
                // ignore them ocmpletely.
                Debug.WriteLine(ex.Message);
            }

            return false;

        }

        private class ServerRequirements {
            internal bool AtLeastOneSelected;
            internal bool NeedDatabaseConnection;
            internal bool PromptBeforeDroppingDatabase;
            internal bool NeedToInstallDatabaseEngine;
            internal bool NeedToInstallDatabase;
            internal bool NeedIIS;
            internal bool NeedToInstallIIS;
            internal bool NeedToInstallWebApplication;
            internal bool NeedToAddIISComponents;
            internal bool NeedToInstallSearchEngine;
        }

        private class ClientRequirements {
            internal bool AtLeastOneSelected;
            internal bool NeedCrystalReports;
            internal bool NeedToInstallCrystalReports;
        }

        private bool installIIS(frmSplash f, bool onlyAddComponents) {



            try {
                if (!(Utility.IsWindowsVista || Utility.IsWindowsXP || Utility.IsWindows7)) {
                    // we only auto-install IIS on XP, Vista, and Windows 7.
                    var dr = MessageBox.Show(this, getDisplayMember("installIIS{notpreferredos_body}", "Before the web application can be installed properly, the Microsoft web server (IIS) must be installed.\nUpdater was unable to detect IIS on your system.\nWould you like to continue installation anyway?"), getDisplayMember("installIIS{notpreferredos_title}", "Microsoft Web Server (IIS) Not Detected"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes) {
                    } else {
                        MessageBox.Show(this, getDisplayMember("installIIS{notpreferredos|no_body}", "Please refer to your Windows documentation for the IIS installation procedure."),
                            getDisplayMember("installIIS{notpreferredos|no_title}", "Please Install IIS Manually"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                } else {
                    if (onlyAddComponents) {
                        var dr = MessageBox.Show(this,
                            getDisplayMember("installIIS{addcomponents_body}", "Before the web application can be installed properly, some additional components to the Microsoft web server (IIS) must be installed.\n\nInstalling these components may require your Windows CD.\n\nWould you like to install it now?"),
                            getDisplayMember("installIIS{addcomponents_title}", "Microsoft Web Server Missing Some Components"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (dr == DialogResult.Yes) {
                            try {
                                Utility.InstallIIS(this);
                            } catch (Exception ex) {
                                dr = MessageBox.Show(this,
                                    getDisplayMember("installIIS{componentsfailed_body}", "Error installing IIS Components:\n{0}\n\nWould you like to continue anyway?", ex.Message),
                                    getDisplayMember("installIIS{componentsfailed_title}", "IIS Component Installation Failed"), MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                                if (dr == DialogResult.Yes) {
                                    // let them keep going!
                                } else {
                                    return false;
                                }
                            }
                        } else {
                            return false;
                        }
                    } else {
                        var dr = MessageBox.Show(this,
                            getDisplayMember("installIIS{install_body}", "Before the web application can be installed properly, the Microsoft web server (IIS) must be installed.\n\nInstalling Microsoft web server may require your Windows CD.\n\nWould you like to install it now?"),
                            getDisplayMember("installIIS{install_title}", "Microsoft Web Server Not Installed"),
                            MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (dr == DialogResult.Yes) {
                            try {
                                Utility.InstallIIS(this);
                            } catch (Exception ex) {
                                dr = MessageBox.Show(this,
                                    getDisplayMember("installIIS{installfailed_body}", "Error installing IIS:\n{0}\n\nWould you like to continue anyway?", ex.Message),
                                    getDisplayMember("installIIS{installfailed_title}", "IIS Installation Failed"),
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                                if (dr == DialogResult.Yes) {
                                    // keep on truckin'
                                } else {
                                    return false;
                                }
                            }
                        } else {
                            return false;
                        }
                    }
                }
            } finally {
                f.Close();
                Application.DoEvents();
            }

            // check iis install exists again (i.e.make sure it worked)
            if (!Utility.IsIISInstalled()) {
                if (DialogResult.Yes == MessageBox.Show(this, 
                        getDisplayMember("installIIS{finalcheck_body}", "IIS was not successfully installed.\nWould you like to continue anyway?"),
                        getDisplayMember("installIIS{finalcheck_title}", "IIS Not Installed"), 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information)){
                    // never stop the beat baby
                } else {
                    return false;
                }
            }

            return true;

        }

        private void btnDownloadServer_Click(object sender, EventArgs e) {

            var controlShiftIsDown = ((Control.ModifierKeys & Keys.Control) == Keys.Control) && ((Control.ModifierKeys & Keys.Shift) == Keys.Shift);

            var reqs = refreshServerRequirements();

            // make sure there's enough space on the drive the temp folder will be created on
            try {
                var tempPath = Utility.GetTempDirectory(2000);
            } catch (Exception ex) {
                MessageBox.Show(this, getDisplayMember("DownloadServer{nospace_body}", "{0}\nYou must create more free space on this drive before continuing.", ex.Message), 
                    getDisplayMember("DownloadServer{nospace_title}", "More Drive Space Required"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // prompt them for database ENGINE information (not database login information)
            if (reqs.NeedDatabaseConnection && reqs.NeedToInstallDatabaseEngine) {
                var enginePrompt = new frmDatabaseEnginePrompt2();
                enginePrompt.AllowRemote = !reqs.NeedToInstallDatabase;
                if (DialogResult.OK != enginePrompt.ShowDialog(this, false)) {
                    return;
                }
            }


            // Install IIS or its subcomponents as needed
            var f1 = new frmSplash();
            if (reqs.NeedToInstallIIS || reqs.NeedToAddIISComponents) {
                if (!installIIS(f1, reqs.NeedToAddIISComponents)) {
                    return;
                }
            }

            // Verify IIS install
            if (reqs.NeedIIS) {

                if (!Utility.IsIISInstalled()) {
                    MessageBox.Show(this, 
                        getDisplayMember("DownloadServer{iisfailed_body}", "The Microsoft Web Server (IIS) could not be automatically installed.\n\nPlease refer to your Windows documentation for assistance."), 
                        getDisplayMember("DownloadServer{iisfailed_title}", "Microsoft Web Server Not Installed"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

#if CHECK_IIS_ASPNET
                var f2 = new frmSplash();
                var f3 = new frmSplash();
                try {
                    // make sure .net is registered with iis.
                    f2.Show("Checking .NET registration in IIS...", false, this);
                    f2.Refresh();
                    Application.DoEvents();
                    if (!Utility.IsAspNet20Registered()) {
                        f2.Close();
                        Application.DoEvents();
                        f3.Show("Registering .NET in IIS...", false, this);
                        f3.Refresh();
                        Application.DoEvents();
                        Utility.RegisterAspNet20InIIS();
                    }
                } finally {
                    f2.Close();
                    Application.DoEvents();
                    f3.Close();
                    Application.DoEvents();
                }

                if (!Utility.IsAspNet20Registered()) {
                    MessageBox.Show(this, "ASP.NET 2.0 could not be automatically registered in IIS.", "ASP.NET Not Registered", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
#else
                var f3 = new frmSplash();
                try {
                    Application.DoEvents();
                    f3.Show(getDisplayMember("DownloadServer{registering.net}", "Registering .NET in IIS..."), false, this);
                    f3.Refresh();
                    Application.DoEvents();
                    Utility.RegisterAspNet20InIIS();
                } finally {
                    f3.Close();
                    Application.DoEvents();
                }
#endif

            }


            // Tell them they're going to lose their existing data
            if (reqs.PromptBeforeDroppingDatabase) {
                var dr = MessageBox.Show(this, 
                    getDisplayMember("DownloadServer{losedata_body}", "Installing a newer version of GRIN-Global Database will cause existing data to be lost forever.\nAre you sure you want to continue?"), 
                    getDisplayMember("DownloadServer{losedata_title}", "Delete Existing Data?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr != DialogResult.Yes) {
                    return;
                }
            }


            // Prompt for database login information
            var password = "";
            var useWindowsAuth = false;
            if (reqs.NeedDatabaseConnection) {
                var fconn = new frmDatabaseLoginPrompt();
                if (DialogResult.OK != fconn.ShowDialog(this, true, true, false, null, null, false, false)) {
                    return;
                } else {
                    // values written to registry, all we really need is password and using windows auth
                    password = fconn.Password;
                    useWindowsAuth = fconn.UseWindowsAuthentication;
                }
            }


            // Prompt them to use Defaults or Custom install mode
            bool silentInstall = false;
            bool includeOptional = false;
            var instMode = new frmInstallMode();
            DialogResult resultInstMode;

            if (controlShiftIsDown)
            {
                resultInstMode = instMode.ShowDialog(this, reqs.NeedToInstallDatabase || reqs.NeedToInstallSearchEngine);
            } else {
                resultInstMode = instMode.ShowDialog(this); //, reqs.NeedToInstallDatabase || reqs.NeedToInstallSearchEngine);
            }
            if (resultInstMode == DialogResult.Cancel){
                return;
            } else {
                silentInstall = instMode.rdoDefault.Checked;
                includeOptional = instMode.rdoDefault.Checked && instMode.chkPrepopulate.Enabled && instMode.chkPrepopulate.Checked;
            }


            // Shutdown search engine if need be (so search engine uninstall / install works properly)
            if (reqs.NeedToInstallSearchEngine) {
                shutdownSearchEngine();

                tweakSearchServiceAsNeeded();
            }


            // copy out mirrored files if they're reinstalling the web app
            var tempDir = Utility.ResolveDirectoryPath(Utility.GetTempDirectory(500) + @"\mirror_installers", true);
            var srcDir = "";
            if (reqs.NeedToInstallWebApplication) {
                try {
                    srcDir = Utility.GetIISPhysicalPath("gringlobal");
                    if (_isMirrorServer && Directory.Exists(srcDir)) {
                        srcDir = Utility.ResolveFilePath(srcDir + @"\uploads\installers", true);
                        if (Directory.Exists(srcDir)) {
                            var f4 = new frmSplash();
                            try {
                                Application.DoEvents();
                                f4.Show(getDisplayMember("DownloadServer{relocating}", "Relocating installer files while reinstalling web application..."), false, this);
                                f4.Refresh();
                                Application.DoEvents();
                                Directory.Move(srcDir, tempDir);
                                //Utility.CopyDirectory(srcDir, tempDir, false);
                            } finally {
                                f4.Close();
                                Application.DoEvents();
                            }
                            
                        }
                    }
                } catch {
                    // no iis folder, ignore
                }
            }




            // Finally we can download and install stuff
            downloadAndOptionallyInstall("server", dgvServerComponents, _serverFilesToDownload, true, password, useWindowsAuth, silentInstall, includeOptional);


            
            // copy from mirror folder back to web app if needed
            if (_isMirrorServer) {

                if (_mirrorCD) {

                    // download the CD file, copy to local web app
                    var files = GetLatestFileInfo("GRIN-Global CD", "CD", "Looking for GRIN-Global CD...", true, null);
                    var sd = new SortedDictionary<string, List<FileDownloadInfo>>();
                    sd.Add("CD", files);

                    downloadAndOptionallyInstall("cd", null, sd, false, null, false, false, false);

                }

                // since we may have just installed the web app for the first time, we need to recheck the physical path for gringlobal
                try {
                    tempDir = Utility.ResolveDirectoryPath(Utility.GetTempDirectory(500) + @"\mirror_installers", true);
                    srcDir = Utility.GetIISPhysicalPath("gringlobal");
                    srcDir = Utility.ResolveFilePath(srcDir + @"\uploads\installers", true);

                } catch {
                    // and IIS maynot be installed at all yet, which would cause this to error out, which we should ignore.
                }
                if (Directory.Exists(tempDir) && Directory.Exists(srcDir)) {

                    if (reqs.NeedToInstallWebApplication) {
                        var fse3 = new frmSplash();
                        try {
                            Application.DoEvents();
                            fse3.Show(getDisplayMember("DownloadServer{mirroring}", "Mirroring files in local web server..."), false, this);
                            fse3.Refresh();
                            Application.DoEvents();
                            Utility.CopyDirectory(tempDir, srcDir, true);

                        } catch (Exception ex3) {
                            MessageBox.Show(getDisplayMember("DownloadServer{mirror_failed}", "Error mirroring files in local web server: {0}", ex3.Message));
                        } finally {
                            fse3.Close();
                            Application.DoEvents();
                        }
                    }
                }
            }

            // Make sure search engine is started if we installed it
            if (reqs.NeedToInstallSearchEngine) {
                var fse2 = new frmSplash();
                try {
                    var sc = new ServiceController("ggse");
                    if (sc != null) {
                        fse2.Show(getDisplayMember("DownloadServer{startingse}", "Starting search engine..."), false, this);
                        Application.DoEvents();
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                    }
                } catch (Exception ex) {
                    Debug.WriteLine("Could not start search engine: " + ex.Message);
                } finally {
                    fse2.Close();
                    Application.DoEvents();
                }
            }


            // this is handled on a per-file basis immediately after install completes successfully.
            // since 2 may install and 1 may fail, doing it at the end isn't the most reliable way
            //if (_isMirrorServer) {
            //    copyServerFilesForDownload();
            //}


            // check for updates now that we're done installing the new ones
            btnCheckForServerUpdates.PerformClick();
        }

        private void tweakSearchServiceAsNeeded() {
            if (Utility.GetInstalledVersion("GRIN-Global Search Engine").Contains("???")) {
                // search engine is not installed already.  This means for the install to work, we must make sure the search engine service (ggse) does not already exist.
                if (Utility.IsServiceInstalled("ggse")) {
                    // even though search engine is not installed, a service named "ggse" exists.  manually delete it.
                    var fse2 = new frmSplash();
                    try {
                        fse2.Show(getDisplayMember("tweakSearchServiceAsNeeded{remove}", "Removing orphaned search engine service..."), false, this);
                        var p = Process.Start("sc.exe", "delete ggse");
                        while (!p.HasExited) {
                            Application.DoEvents();
                            Thread.Sleep(50);
                        }
                    } catch (Exception ex) {
                        EventLog.WriteEntry("GRIN-Global Updater", "Could not remove orphaned search engine service: " + ex.Message);
                    } finally {
                        fse2.Close();
                        Application.DoEvents();
                    }
                }
            } else {
                // search engine is installed already.  Launching the installer will launch the previous install's uninstall action.
                // if a "ggse" service does not exist that uninstaller will fail.
                // so we create an orphaned one if needed so the uninstaller works properly (i.e. the orphaned one is immediately removed by the uninstaller)
                if (!Utility.IsServiceInstalled("ggse")) {

                    var fse2 = new frmSplash();
                    try {
                        fse2.Show(getDisplayMember("tweakSearchServiceAsNeeded{uninstall}", "Preparing to uninstall previous search engine service..."), false, this);
                        var p = Process.Start("sc.exe", "create ggse binPath= %WINDIR%\notepad.exe");
                        while (!p.HasExited) {
                            Application.DoEvents();
                            Thread.Sleep(50);
                        }
                    } catch (Exception ex) {
                        EventLog.WriteEntry("GRIN-Global Updater", "Could not create orphaned search engine service: " + ex.Message);
                    } finally {
                        fse2.Close();
                        Application.DoEvents();
                    }
                }
            }
        }


        private ServerRequirements getServerRequirements() {

            var reqs = new ServerRequirements();

            foreach (DataGridViewRow dgvr in dgvServerComponents.Rows) {
                var checkCell = dgvr.Cells["colServerCheck"] as DataGridViewCheckBoxCell;
                string val = checkCell.Value.ToString().ToLower();
                if (val == "true") {
                    var displayName = dgvr.Cells["colServerDisplayName"].Value.ToString().ToLower();

                    reqs.AtLeastOneSelected = true;

                    if (displayName.Contains("database")) {
                        reqs.NeedDatabaseConnection = true;
                        reqs.NeedToInstallDatabase = true;
                        reqs.NeedToInstallDatabaseEngine = !Utility.IsAnyDatabaseEngineInstalled;
                    }

                    if (displayName.Contains("search engine")) {
                        reqs.NeedToInstallSearchEngine = true;
                        reqs.NeedDatabaseConnection = true;
                    }

                    if (displayName.Contains("web application")) {
                        reqs.NeedIIS = true;
                        reqs.NeedDatabaseConnection = true;
                        reqs.NeedToInstallWebApplication = true;
                        if (!Utility.IsIISInstalled()) {
                            reqs.NeedToInstallIIS = true;
                        } else if (!Utility.AreAllGrinGlobalRequiredIISComponentsInstalled() && !Utility.IsWindowsXP) {
                            // IIS 5.x (i.e. Windows XP) always has required components by default is IIS is installed
                            // aka only Vista and Windows 7 support componentized IIS installs.
                            reqs.NeedToAddIISComponents = true;
                        }
                    }

                    if (displayName.Contains("database")) {
                        var installedVersion = dgvr.Cells["colServerInstalledVersion"].Value.ToString().ToLower();
                        if (!installedVersion.Contains("(not installed)")) {
                            reqs.PromptBeforeDroppingDatabase = true;
                        }
                    }

                }
            }



            return reqs;

        }

        private ClientRequirements getClientRequirements() {

            var reqs = new ClientRequirements();

            foreach (DataGridViewRow dgvr in dgvClientComponents.Rows) {
                var checkCell = dgvr.Cells["colClientCheck"] as DataGridViewCheckBoxCell;
                string val = checkCell.Value.ToString().ToLower();
                if (val == "true") {
                    var displayName = dgvr.Cells["colClientDisplayName"].Value.ToString().ToLower();

                    reqs.AtLeastOneSelected = true;

                    if (displayName.Contains("curator")) {
                        reqs.NeedCrystalReports = true;
                        reqs.NeedToInstallCrystalReports = true;
                        foreach(string key in Utility.ListInstalledApplications().Keys){
                            if (key.ToLower().Contains("crystal reports basic")) {
                                reqs.NeedToInstallCrystalReports = false;
                                break;
                            }
                        }
                    }
                }
            }

            return reqs;

        }

        private void btnDownloadClient_Click(object sender, EventArgs e) {

            var instMode = new frmInstallMode();
            var result = instMode.ShowDialog(this); //, false);
            if (result == DialogResult.Cancel) {
                return;
            } else {
                downloadAndOptionallyInstall("client", dgvClientComponents, _clientFilesToDownload, true, null, true, instMode.rdoDefault.Checked, false);

                // copy cd if we're a mirror server
                if (_isMirrorServer) {
                    if (_mirrorCD) {
                        // download the CD file
                        var files = GetLatestFileInfo("GRIN-Global CD", "CD", "Looking for GRIN-Global CD...", true, null);
                        var sd = new SortedDictionary<string, List<FileDownloadInfo>>();
                        sd.Add("CD", files);

                        downloadAndOptionallyInstall("cd", null, sd, false, null, false, false, false);
                    }
                }

                // check for updates now that we're done installing the new ones
                btnCheckForClientUpdates.PerformClick();
            }
        }


        private bool downloadAndOptionallyInstall(string componentType, DataGridView dgv, SortedDictionary<string, List<FileDownloadInfo>> files, bool performInstallation, string dbPassword, bool dbUsesWindowsAuthentication, bool silentInstall, bool includeOptionalData) {
            var fp = new frmProgress();

            var sd2 = new SortedDictionary<string, List<FileDownloadInfo>>();
            if (dgv != null) {
                for (int i = 0; i < dgv.Rows.Count; i++) {

                    string name = dgv.Rows[i].Cells["col" + componentType + "DisplayName"].Value as string;
                    if (files.ContainsKey(name)) {
                        if (dgv.Rows[i].Cells["col" + componentType + "Check"].Value.ToString().ToLower() != "true") {
                            // exclude unchecked ones
                            files.Remove(name);
                        } else {
                            // exclude ones that are installed and current
                            string status = dgv.Rows[i].Cells["col" + componentType + "Status"].Value as string;
                            if (status == "Current") {
                                files.Remove(name);
                            }
                        }
                    }
                }
            }


            //this.WindowState = FormWindowState.Minimized;
            //Application.DoEvents();

            var dr = fp.ShowDialog(this, componentType, files, false, Utility.GetRegSetting("IsMirrorServer", 1) == 1, performInstallation, dbPassword, dbUsesWindowsAuthentication, silentInstall, includeOptionalData);

            if (this.WindowState == FormWindowState.Minimized) {
                this.WindowState = FormWindowState.Normal;
            }
            Utility.ActivateApplication(this.Handle);
            Application.DoEvents();

            // refresh our installed list
            _installedApplications = Utility.ListInstalledApplications();

            // Fixes #5 (gridview rows showing up as wrong colors sometimes after install completion)
            if (dgv != null) {
                dgv.Refresh();
                Application.DoEvents();
            }

            if (dr == DialogResult.Cancel) {
                MessageBox.Show(getDisplayMember("downloadAndOptionallyInstall{cancel}", "Download / Install was cancelled before completion."));
            }

            return dr == DialogResult.OK;

        }


        private SortedDictionary<string, FileDownloadInfo> _installedApplications;

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
//            MessageBox.Show(Utility.GetInstalledVersion("GRIN-Global Updater"));
            //MessageBox.Show(this, Utility.GetApplicationVersion("GRIN-Global Updater"), "GRIN-Global Updater");
            var br = "---------------------------------------------------------------\r\n";
            var msg = new frmMessageBox();
            msg.btnYes.Visible = false;
            msg.Width = 480;
            msg.Height = 336;
            msg.linkLabel1.Visible = true;
            msg.btnNo.Text = "OK";
            msg.Text = "About GRIN-Global Updater";
            msg.txtMessage.Font = new Font("Courier-New", 10.0f, FontStyle.Regular);
            var info = Utility.GetApplicationVersion("GRIN-Global Updater") + br;
            msg.txtMessage.Text = Utility.GetApplicationVersion("GRIN-Global Updater") + "\r\n" + br + "\r\n";
            msg.ShowDialog(this);
        }

        private void checkForNewUpdaterToolStripMenuItem_Click(object sender, EventArgs e) {
            checkForNewUpdater(true, cboSourceServer.Text, (Control.ModifierKeys & Keys.Shift) == Keys.Shift);
        }

        delegate void updaterCheckedCallback(SortedDictionary<string, List<FileDownloadInfo>> sd, bool promptIfNotNewer, string latestVersion, string errorMessage);

        private void updaterChecked(SortedDictionary<string, List<FileDownloadInfo>> sd, bool promptIfNotNewer, string latestVersion, string errorMessage){

            if (!String.IsNullOrEmpty(errorMessage)) {
                // error occurred
                if (promptIfNotNewer) {
                    // promptIfNotNewer basically means we're not auto-cheking
                    MessageBox.Show(getDisplayMember("updaterChecked{failed}", "Error checking for newer version of GRIN-Global Updater:\n{0}", errorMessage));
                }
            } else {
                if (sd == null || sd.Keys.Count == 0 || sd[sd.Keys.First()].Count == 0) {
                    // no newer versions available.
                    if (promptIfNotNewer) {
                        MessageBox.Show(getDisplayMember("updaterChecked{latest}", "{0}\n\nYou are currently running the latest version.", Utility.GetApplicationVersion("GRIN-Global Updater")));
                    }
                } else {
                    // new version is available. prompt as needed.
                    var dr = MessageBox.Show(this, 
                        
                        getDisplayMember("updaterChecked{newer_body}", "A new version of GRIN-Global Updater is available ({0}).\nDo you want to download and install it now?", latestVersion ), 
                        getDisplayMember("updaterChecked{newer_title}", "New Version Available"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes) {
                        var f = new frmProgress();
                        if (f.ShowDialog(this, "Updater", sd, true, Utility.GetRegSetting("IsMirrorServer", 1) == 1, true, null, false, false, false) == DialogResult.OK) {
                            this.Close();
                            Application.Exit();
                        }
                    }
                }
            }
        }

        private void checkForNewUpdater(bool promptIfNotNewer, string url, bool forceDownloadNewer){

            string latestVersion = "";
            SortedDictionary<string, List<FileDownloadInfo>> sd = new SortedDictionary<string, List<FileDownloadInfo>>();

            try {
                List<FileDownloadInfo> files = GetLatestFileInfo("GRIN-Global Updater", "Updater", "Checking for new version of GRIN-Global Updater...", promptIfNotNewer, url);

                if (files.Count == 0) {
                    // something went wrong, ignore as they've already been prompted.
                    return;
                }

                List<FileDownloadInfo> list = null;

                foreach (var fdi in files) {
                    if (!sd.TryGetValue(fdi.DisplayName, out list)) {
                        list = new List<FileDownloadInfo>();
                        sd.Add(fdi.DisplayName, list);
                    }

                    if (fdi.NeedToInstall || forceDownloadNewer) {
                        fdi.NeedToInstall = true;
                        if (!fdi.NeedToDownload) {
                            fdi.NeedToDownload = forceDownloadNewer;
                        }
                        list.Add(fdi);
                        latestVersion = fdi.LatestVersion;
                        if (fdi.Child != null) {
                            latestVersion = fdi.Child.LatestVersion;
                        }
                    } else if (fdi.NeedToDownload || forceDownloadNewer) {
                        fdi.NeedToDownload = true;
                        list.Add(fdi);

                        //if (Utility.IsWindowsFilePath(url)) {
                        // the bootstrapper exe for local installs just point to relative file path (""), 
                        // so we must manually download it here
                        // in other words, don't break out, but keep processing the out of date files so
                        // the .msi is copied to the proper directory so when the bootstrapper exe runs, it
                        // will find the msi in the current directory.
                        //    list.Add(fdi);
                        //}
                    }
                }

                // successful check, tell UI thread to prompt as needed
                BeginInvoke(new updaterCheckedCallback(updaterChecked), sd, promptIfNotNewer, latestVersion, null);

            } catch (Exception ex) {
                // unsuccessful check, tell UI thread to show error as needed
                BeginInvoke(new updaterCheckedCallback(updaterChecked), null, promptIfNotNewer, latestVersion, ex.Message, forceDownloadNewer);
            }

        }

        bool _activated = false;
        private void frmUpdater_Activated(object sender, EventArgs e) {
            if (!_activated) {
                _activated = true;
//                Utility.ActivateApplication(this.Handle);
                string updaterDownloadCache = FileDownloadInfo.DownloadedCacheFolder("Updater");
                foreach (string f in Utility.GetAllFiles(updaterDownloadCache, true)) {
                    try {
                        // delete the file, always always always re-download an updater
                        // so we're absolutely sure we have the latest copy.
                        File.Delete(f);
                    } catch {
                    }
                }

                var url = cboSourceServer.Text;

                if (RefreshGroups) {
                    btnCheckForServerUpdates.PerformClick();
                    btnCheckForClientUpdates.PerformClick();
                }

                if (Utility.GetRegSetting("AutoCheckUpdaterVersion", 1) == 1 && !IsAutoDownloading()){
                    background("Checking for new updater...", false, false, (sentBy, args) => {
                        checkForNewUpdater(false, url, false);
                    });
                }

            }
            

        }

        private void copyServerFilesForDownload() {

            // first, find appropriate folder under local iis installation....
            string physPath = Utility.GetIISPhysicalPath("gringlobal");
            string installCache = FileDownloadInfo.InstalledCacheFolder("server");
            foreach (string key in _serverFilesToDownload.Keys) {
                foreach (var fdi in _serverFilesToDownload[key]) {

                    string sourceFilePath = (installCache + @"\" + key).Replace("~", "").Replace("//", @"\").Replace(@"\\", @"\") + @"\" + fdi.FileName;
                    string destFilePath = Utility.ResolveFilePath(physPath + fdi.AppRelativeUrl.Replace("~", "").Replace("//", @"\").Replace(@"\\", @"\"), true);
                    // copy the file from install cache to appropriate place in local server 
                    if (File.Exists(destFilePath)) {
                        File.Delete(destFilePath);
                    }
                    File.Copy(sourceFilePath, destFilePath);
                    // update the database to include the links to it
                    MessageBox.Show("TODO: update file links for download server");
                }
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e) {
            showOptions(false);
        }

        private void cboSourceServer_SelectedIndexChanged(object sender, EventArgs e) {
            clearGridViews();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            if (MessageBox.Show(this, 
                getDisplayMember("deleteDownloadedContent{body}", "You are about to delete all downloaded content.\nDo you want to continue?"), 
                getDisplayMember("deleteDownloadedContent{title}", "Confirm Delete"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                string rootCacheFolder = FileDownloadInfo.DownloadedCacheFolder(null);
                try {
                    Utility.EmptyDirectory(rootCacheFolder);
                } catch {
                    // eat all errors when deleting
                }
                //foreach (string d in Directory.GetDirectories(rootCacheFolder)) {
                //    Directory.Delete(d, true);
                //}
                //foreach (string f in Directory.GetFiles(rootCacheFolder)) {
                //    File.Delete(f);
                //}
            }

            clearGridViews();
        }

        private void clearGridViews(){

            dgvClientComponents.Rows.Clear();
            btnDownloadClient.Enabled = false;

            dgvServerComponents.Rows.Clear();
            btnDownloadServer.Enabled = false;

        }

        private void btnLocal_Click(object sender, EventArgs e) {
            openFileDialog1.Filter = "GRIN-Global Offline Installer|*.xml";
            openFileDialog1.Title = "Locate the GRIN-Global Offline Installer File (*.xml)";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                cboSourceServer.Text = openFileDialog1.FileName;
            }
        }

        private void runUninstall(string uninstallString) {



            ProcessStartInfo psi = new ProcessStartInfo();

            var pos = uninstallString.IndexOf(" ");
            if (pos > -1) {
                psi.FileName = uninstallString.Substring(0, pos);
                if (pos < uninstallString.Length) {
                    psi.Arguments = uninstallString.Substring(pos + 1);
                }
            } else {
                psi.FileName = uninstallString;
            }

            Process p = Process.Start(psi);
            Application.DoEvents();
            Thread.Sleep(1000);
            MessageBox.Show(this, getDisplayMember("runUninstall{body}", "Click OK when the uninstall has completed."), 
                getDisplayMember("runUninstall{title}", "Wait For Completion"));
        }

        private void shutdownSearchEngine() {
            var fse1 = new frmSplash();
            try {
                var sc = new ServiceController("ggse");
                if (sc != null) {
                    fse1.Show(getDisplayMember("shutdownSearchEngine{stopping}", "Stopping search engine..."), false, this);
                    Application.DoEvents();
                    sc.Stop();
                }
            } catch (Exception ex) {
                Debug.WriteLine("Could not stop search engine: " + ex.Message);
            } finally {
                fse1.Close();
                Application.DoEvents();
            }
        }

        private void dgvServerComponents_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex > -1)
            {
                var cell = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell is DataGridViewLinkCell)
                {
                    var fdp = cell.Tag as FileDownloadPair;
                    if (fdp.Installed.DisplayName == "GRIN-Global Search Engine")
                    {
                        // shut down service before doing the uninstall
                        shutdownSearchEngine();

                        tweakSearchServiceAsNeeded();

                    }
                    frmProgress.UninstallExistingMsi(Utility.ResolveFilePath(Utility.GetTempDirectory(100) + @"\" + fdp.Latest.FileName, true), fdp.Installed, this, false, false, null, false);

                    btnCheckForServerUpdates.PerformClick();

                }
                else if (cell is DataGridViewCheckBoxCell)
                {
                    // HACK: CellContentClick event fires before checkbox is toggled.
                    //       Bounce focus around to refresh everything correctly.
                    ((DataGridView)sender).CurrentCell = ((DataGridView)sender).Rows[e.RowIndex].Cells[1];
                    ((DataGridView)sender).CurrentCell = ((DataGridView)sender).Rows[e.RowIndex].Cells[0];
                }
            }
        }

        private ServerRequirements refreshServerRequirements() {
            var reqs = getServerRequirements();
            var items = new List<string>();
            if (reqs.NeedToInstallDatabaseEngine) {
                items.Add("Database engine");
            }
            if (reqs.NeedToInstallIIS) {
                items.Add("Web server (IIS)");
            } else if (reqs.NeedToAddIISComponents){
                items.Add("Web server components (IIS)");
            }

            if (items.Count == 0) {
                lblServerRequirements.Text = "";
            } else {
                lblServerRequirements.Text = "The following software may also be required: " + String.Join(", ", items.ToArray());
            }

            if (reqs.AtLeastOneSelected) {
                btnDownloadServer.Enabled = true;
            } else {
                btnDownloadServer.Enabled = false;
            }

            return reqs;

        }

        private ClientRequirements refreshClientRequirements() {
            var reqs = getClientRequirements();
            var items = new List<string>();
            if (reqs.NeedToInstallCrystalReports) {
                items.Add("Crystal Reports");
            }

            if (items.Count == 0) {
                lblClientRequirements.Text = "";
            } else {
                lblClientRequirements.Text = "The following software may also be required: " + String.Join(", ", items.ToArray());
            }

            if (reqs.AtLeastOneSelected) {
                btnDownloadClient.Enabled = true;
            } else {
                btnDownloadClient.Enabled = false;
            }


            return reqs;

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

        }

        private void dgvServerComponents_CellValueChanged(object sender, DataGridViewCellEventArgs e) {

        }

        private void dgvServerComponents_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex > -1 && e.RowIndex < dgvServerComponents.Rows.Count) {
                var cell = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell is DataGridViewCheckBoxCell) {
                    refreshServerRequirements();
                }
            }

        }

        private void dgvClientComponents_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex > -1)
            {
                var cell = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell is DataGridViewLinkCell)
                {
                    var fdp = cell.Tag as FileDownloadPair;
                    frmProgress.UninstallExistingMsi(Utility.ResolveFilePath(Utility.GetTempDirectory(100) + @"\" + fdp.Latest.FileName, true), fdp.Installed, this, false, false, null, false);

                    btnCheckForClientUpdates.PerformClick();

                }
                else if (cell is DataGridViewCheckBoxCell)
                {
                    // HACK: CellContentClick event fires before checkbox is toggled.
                    //       Bounce focus around to refresh everything correctly.
                    ((DataGridView)sender).CurrentCell = ((DataGridView)sender).Rows[e.RowIndex].Cells[1];
                    ((DataGridView)sender).CurrentCell = ((DataGridView)sender).Rows[e.RowIndex].Cells[0];
                }
            }
        }

        private void dgvClientComponents_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex > -1 && e.RowIndex < ((DataGridView)sender).Rows.Count) {
                var cell = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell is DataGridViewCheckBoxCell) {
                    refreshClientRequirements();
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            var fde = new frmDatabaseEnginePrompt2();
            fde.ShowDialog(this, false);
        }

        private void mnuToolsDownloadCD_Click(object sender, EventArgs e) {
            showOptions(true);
        }

        private void showOptions(bool defaultTabIsInstallCD) {

            var f = new frmOptions();
            using (new AutoCursor(this)) {
                if (defaultTabIsInstallCD) {
                    f.SelectInstallCDTab();
                }
                for (var i = 0; i < cboSourceServer.Items.Count; i++) {
                    f.AllUrls.Add(cboSourceServer.Items[i].ToString());
                }
                f.RefreshData();
            }
            f.ShowDialog(this);
            _autoFormatDownloadServerUrl = Utility.GetRegSetting("AutoFormatURL", 1) == 1;
            _isMirrorServer = Utility.GetRegSetting("IsMirrorServer", 1) == 1;
            _mirrorCD = Utility.GetRegSetting("MirrorCD", 0) == 1;
        }


        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "Updater", "frmUpdater", resourceName, null, defaultValue, substitutes);
        }
    }
}
