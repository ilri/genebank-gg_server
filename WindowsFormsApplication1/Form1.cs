using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Net;
using System.IO;

namespace WindowsFormsApplication1 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            string serverName = "grin-global-dev1.agron.iastate.edu";

            var newFiles = ListLatestClientFiles(serverName);
            if (newFiles.Count > 0) {
                // new version available, download it
                var localNewFiles = DownloadFiles(serverName, newFiles);
                InstallFiles(localNewFiles);
            }

        }

        public List<string> ListLatestClientFiles(string serverName) {
            var filesToDownload = new List<string>();

            // determine which files 
            WebSvc.GUI gui = new WindowsFormsApplication1.WebSvc.GUI();
            gui.Url = "http://" + serverName + "/gringlobal/gui.asmx";
            var ds = gui.GetFileInfo(false, "GRIN-Global Client", null, true, true);
            foreach (DataRow dr in ds.Tables["file_info"].Rows) {
                // display name is the name of the installed app -- the one that shows up in Add/Remove Programs.
                string installedVersion = getInstalledVersion(dr["display_name"].ToString());
                if (!String.IsNullOrEmpty(installedVersion)) {
                    // it's currently installed. if the version doesn't match exactly, assume it's out of date
                    // we can track only the version of msi's, so just always download their associated exe's
                    if (installedVersion != dr["file_version"].ToString() || !dr["file_name"].ToString().ToLower().EndsWith(".msi")) {
                        filesToDownload.Add(dr["virtual_file_path"].ToString());
                    }
                } else {
                    // it's not installed.
                    filesToDownload.Add(dr["virtual_file_path"].ToString());
                }
            }
            return filesToDownload;
        }

        public List<string> DownloadFiles(string serverName, List<string> appRelativeUrls){

            var localFiles = new List<string>();
            // download all the files
            foreach (var appUrl in appRelativeUrls) {
                string url = "http://" + serverName + appUrl.Replace("~/", "/");
                HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;
                HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
                if (Convert.ToInt32(resp.StatusCode) > 500) {
                    throw new InvalidOperationException("File download '" + url + "' failed: " + resp.StatusCode + " - " + resp.StatusDescription);
                } else {
                    string localFolder = Environment.ExpandEnvironmentVariables(@"%APPDATA%\GRIN-Global\Client\downloaded\");
                    if (!Directory.Exists(localFolder)) {
                        Directory.CreateDirectory(localFolder);
                    }
                    string localFile = localFolder + Path.GetFileName(appUrl);
                    if (File.Exists(localFile)) {
                        File.Delete(localFile);
                    }
                    localFiles.Add(localFile);

                    using (Stream s = resp.GetResponseStream()) {
                        using (FileStream fs = new FileStream(localFile, FileMode.Create, FileAccess.Write)) {
                            int readBytes = 0;
                            var buf = new byte[4096];
                            while ((readBytes = s.Read(buf, 0, buf.Length)) > 0) {
                                fs.Write(buf, 0, readBytes);
                            }
                        }
                    }
                }
            }

            return localFiles;

        }

        public void InstallFiles(List<string> newFiles) {

            // TODO: determine if the exe or msi should be run, run it, exit
        }

        private string getInstalledVersion(string installedAppDisplayName) {

            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false)) {
                string[] guids = rk.GetSubKeyNames();
                foreach (string guid in guids) {
                    string displayName = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + guid, "DisplayName", "") as string;
                    string displayVersion = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + guid, "DisplayVersion", "") as string;
                    string uninstallString = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + guid, "UninstallString", "") as string;
                    if (!String.IsNullOrEmpty(displayName) && displayName.ToLower() == installedAppDisplayName.ToLower()) {
                        return displayVersion;
                    }
                }
            }

            return null;
        }
    }
}
