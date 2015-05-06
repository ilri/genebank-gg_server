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
using GrinGlobal.InstallHelper;
using System.Diagnostics;
using System.Net;
using System.Xml;
using System.Xml.XPath;
using GrinGlobal.Core;

namespace GrinGlobal.Updater {
    public partial class frmOptions : Form {
        public frmOptions() {
            InitializeComponent();
            AllUrls = new List<string>();
        }

        public void SelectInstallCDTab() {
            tabControl1.SelectedTab = tpInstallerCD;
        }
        public List<string> AllUrls;

        private void btnDelete_Click(object sender, EventArgs e) {
            decimal mb = 0;
            foreach (DataGridViewRow dgvr in dataGridView1.Rows) {
                var cell = (dgvr.Cells[0] as DataGridViewCheckBoxCell);
                if (cell.Value.ToString().ToLower() == cell.TrueValue.ToString().ToLower()) {
                    mb += Utility.ToDecimal(dgvr.Cells["colFileSize"].Value, 0.0M);
                }
            }
            if (MessageBox.Show(this, 
                getDisplayMember("ConfirmDelete{body}", "You are about to delete {0} MB of downloaded content.\nDo you want to continue?", mb.ToString("###,###,##0.00")),
                getDisplayMember("ConfirmDelete{title}", "Confirm Delete"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                foreach (DataGridViewRow dgvr in dataGridView1.Rows) {
                    var cell = (dgvr.Cells[0] as DataGridViewCheckBoxCell);
                    if (cell.Value.ToString().ToLower() == cell.TrueValue.ToString().ToLower()) {
                        string filePath = dgvr.Cells["colFullPath"].Value.ToString();
                        if (File.Exists(filePath)) {
                            try {
                                File.Delete(filePath);
                            } catch (Exception ex) {
                                MessageBox.Show(getDisplayMember("ConfirmatDelete{failed}",
                                    "Could not delete file '{0}': {1}", filePath, ex.Message));
                            }
                        }
                    }
                }
                refreshGrid();
            }

        }

        public void RefreshData() {
            chkAutoFormatUrl.Checked = Utility.GetRegSetting("AutoFormatURL", 1) == 1;
            chkIsMirrorServer.Checked = Utility.GetRegSetting("IsMirrorServer", 1) == 1;
            chkMirrorCD.Checked = Utility.GetRegSetting("MirrorCD", 0) == 1;
            chkAutoCheckUpdaterVersion.Checked = Utility.GetRegSetting("AutoCheckUpdaterVersion", 1) == 1;

            chkHttpProxy.Checked = Utility.GetRegSetting("ProxyEnabled", 0) == 1;
            chkUseDefaultProxy.Checked = Utility.GetRegSetting("ProxyUseDefault", 1) == 1;
            chkUseDefaultProxyCredentials.Checked = Utility.GetRegSetting("ProxyUseDefaultCredentials", 1) == 1;
            chkProxyBypassOnLocal.Checked = Utility.GetRegSetting("ProxyBypassOnLocal", 1) == 1;
            chkProxySuppress417Errors.Checked = Utility.GetRegSetting("ProxyExpect100Continue", 0) == 0;
            txtProxyServerName.Text = Utility.GetRegSetting("ProxyServerName", "");
            var port = Utility.GetRegSetting("ProxyPort", 0);
            if (port > 0) {
                txtProxyPort.Text = port.ToString();
            } else {
                txtProxyPort.Text = "";
            }
            txtProxyPassword.Text = Utility.GetRegSetting("ProxyPassword", "");
            txtProxyUsername.Text = Utility.GetRegSetting("ProxyUsername", "");
            txtProxyDomain.Text = Utility.GetRegSetting("ProxyDomain", "");

            lnkOpenFolder.Text = FileDownloadInfo.DownloadedCacheFolder("");
            refreshGrid();
            syncGui();
            refreshSystemInfo();
        }

        private void frmOptions_Load(object sender, EventArgs e) {
        }

        private void refreshGrid() {
            dataGridView1.Rows.Clear();
            foreach (string f in Utility.GetAllFiles(FileDownloadInfo.DownloadedCacheFolder(""), true)) {
                var fi = new FileInfo(f);
                var idx = dataGridView1.Rows.Add(true, fi.Name, (((decimal)fi.Length) / 1024.0M / 1024.0M).ToString("###,###,##0.00"), fi.FullName);
                var lnk = dataGridView1.Rows[idx].Cells[1] as DataGridViewLinkCell;
                lnk.Tag = fi.FullName;
                lnk.ToolTipText = "Show " + fi.Name + " in Windows Explorer";
            }
            btnDelete.Enabled = dataGridView1.Rows.Count > 0;
        }

        private void refreshSystemInfo() {

            var sb = new StringBuilder(Utility.GetSystemInfo());

            // add on Web app settings information
            sb.AppendLine();
            sb.AppendLine("GRIN-Global Web Application Configuration");
            sb.AppendLine("---------------------------------------");
            try {
                var webConfigFile = Utility.ResolveFilePath(Utility.GetIISPhysicalPath("gringlobal") + @"\web.config", false);
                if (File.Exists(webConfigFile)) {
                    var nav = new XPathDocument(webConfigFile).CreateNavigator();
                    var it = nav.Select("/configuration/appSettings/add");
                    sb.AppendLine();
                    sb.AppendLine("Application settings...");
                    while (it.MoveNext()) {
                        var key = it.Current.GetAttribute("key", "");
                        var value = it.Current.GetAttribute("value", "");
                        if (key.ToLower().Contains("password")) {
                            sb.AppendLine(key + "= (hidden)");
                        } else {
                            sb.AppendLine(key + "=" + value);
                        }
                    }
                    it = nav.Select("/configuration/connectionStrings/add");
                    sb.AppendLine();
                    sb.AppendLine("Connection Strings...");
                    while (it.MoveNext()) {
                        sb.AppendLine("provider=" + it.Current.GetAttribute("providerName", "") + ", connectionString=" + it.Current.GetAttribute("connectionString", ""));
                    }
                } else {
                    sb.AppendLine("(web.config file not found at " + webConfigFile + ")");
                }
            } catch (Exception exweb) {
                sb.AppendLine("********* Error: " + exweb.Message);
            }
            sb.AppendLine();



            // add on Curator Tool-specific information
            sb.AppendLine();
            sb.AppendLine("GRIN-Global Curator Tool URLs");
            sb.AppendLine("---------------------------------------");

            var ctUrlFile = Utility.ResolveFilePath(@"*ApplicationData*\GRIN-Global\Curator Tool\WebServiceURL.txt", false);
            try {
                if (File.Exists(ctUrlFile)) {
                    var lines = File.ReadAllLines(ctUrlFile);
                    foreach (var line in lines) {
                        sb.AppendLine(line);
                    }
                } else {
                    sb.AppendLine("(WebServiceUrl.txt file not found at " + ctUrlFile + ")");
                }
            } catch (Exception exCT) {
                sb.AppendLine("********* Error: " + exCT.Message);
            }
            sb.AppendLine();


            // add on Updater-specific registry settings
            sb.AppendLine();
            sb.AppendLine("GRIN-Global Updater specific settings");
            sb.AppendLine("---------------------------------------");
            sb.AppendLine("LastURL=" + Utility.GetRegSetting("LastURL", ""));
            sb.AppendLine("AutoFormatURL=" + Utility.GetRegSetting("AutoFormatURL", 1));
            sb.AppendLine("AutoCheckUpdaterVersion=" + Utility.GetRegSetting("AutoCheckUpdaterVersion", 1));
            sb.AppendLine("IsMirrorServer=" + Utility.GetRegSetting("IsMirrorServer", 1));
            sb.AppendLine("MirrorCD=" + Utility.GetRegSetting("MirrorCD", 0));

            sb.AppendLine("ProxyEnabled=" + Utility.GetRegSetting("ProxyEnabled", 0));
            sb.AppendLine("ProxyUseDefault=" + Utility.GetRegSetting("ProxyUseDefault", 1));
            sb.AppendLine("ProxyUseDefaultCredentials=" + Utility.GetRegSetting("ProxyUseDefaultCredentials", 1));
            sb.AppendLine("ProxyBypassOnLocal=" + Utility.GetRegSetting("ProxyBypassOnLocal", 1));
            sb.AppendLine("ProxyServerName=" + Utility.GetRegSetting("ProxyServerName", ""));
            sb.AppendLine("ProxyPort=" + Utility.GetRegSetting("ProxyPort", 0));
            sb.AppendLine("ProxyUserName=" + Utility.GetRegSetting("ProxyUsername", ""));
            sb.AppendLine("ProxyPassword=" + (string.IsNullOrEmpty(Utility.GetRegSetting("ProxyPassword", "")) ? "(Not set)" : "**********"));
            sb.AppendLine("ProxyDomain=" + Utility.GetRegSetting("ProxyDomain", ""));
            sb.AppendLine("ProxyExpect100Continue=" + Utility.GetRegSetting("ProxyExpect100Continue", 0));

            sb.AppendLine();
            sb.AppendLine("All URL's touched by Updater...");
            if (AllUrls == null | AllUrls.Count == 0) {
                sb.AppendLine("(none found)");
            } else {
                foreach (string s in AllUrls) {
                    sb.AppendLine(s);
                }
            }
            sb.AppendLine();

            txtSystemInfo.Text = sb.ToString();

        }

        private void btnDone_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void saveRegSettings() {
            Utility.SetRegSetting("AutoFormatURL", chkAutoFormatUrl.Checked ? 1 : 0, false);
            Utility.SetRegSetting("AutoCheckUpdaterVersion", chkAutoCheckUpdaterVersion.Checked ? 1 : 0, false);
            Utility.SetRegSetting("IsMirrorServer", chkIsMirrorServer.Checked ? 1 : 0, false);
            Utility.SetRegSetting("MirrorCD", chkMirrorCD.Checked ? 1 : 0, false);

            Utility.SetRegSetting("ProxyEnabled", chkHttpProxy.Checked ? 1 : 0, false);
            Utility.SetRegSetting("ProxyUseDefault", chkUseDefaultProxy.Checked ? 1 : 0, false);
            Utility.SetRegSetting("ProxyUseDefaultCredentials", chkUseDefaultProxyCredentials.Checked ? 1 : 0, false);
            Utility.SetRegSetting("ProxyBypassOnLocal", chkProxyBypassOnLocal.Checked ? 1 : 0, false);
            Utility.SetRegSetting("ProxyServerName", txtProxyServerName.Text, false);
            Utility.SetRegSetting("ProxyPort", Utility.ToInt32(txtProxyPort.Text, 0), false);
            Utility.SetRegSetting("ProxyUsername", txtProxyUsername.Text, false);
            Utility.SetRegSetting("ProxyPassword", txtProxyPassword.Text, false);
            Utility.SetRegSetting("ProxyDomain", txtProxyDomain.Text, false);
            Utility.SetRegSetting("ProxyExpect100Continue", chkProxySuppress417Errors.Checked ? 0 : 1, false);

        }

        private void frmOptions_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                this.Close();
            }
        }

        //private void btnDownloadPrerequisitesCD_Click(object sender, EventArgs e) {
        //    frmUpdater fu = (frmUpdater)this.Owner;
        //    var files = fu.GetLatestFileInfo("GRIN-Global Prerequisites", "Prerequisites", "Looking for GRIN-Global Prerequisites...", true, null);
        //    var sd = new SortedDictionary<string, List<FileDownloadInfo>>();
        //    sd.Add("Prerequisites", files);
        //    if (files.Count > 0){
        //        new frmProgress().ShowDialog(this, "Prerequisites", sd, false, chkIsMirrorServer.Checked, true, null, false, false, false);
        //    } else {
        //        var url = Utility.GetRegSetting("LastURL", "");
        //        if (Utility.IsWindowsFilePath(url)) {
        //            MessageBox.Show("You are currently pointing at a local file path.\nTo download the prerequisites CD, you must connect to a web server.");
        //        } else {
        //            var uri = new Uri(url);
        //            MessageBox.Show("The GRIN-Global Prerequisites cab file could be found on the server.\nPlease inform the administrator of " + uri.Host + ".");
        //        }
        //    }
        //}

        private void lnkOpenFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("explorer.exe", @"""" + FileDownloadInfo.DownloadedCacheFolder("") + @"""");
        }

        private void btnDownloadInstallerCD_Click(object sender, EventArgs e) {
            frmUpdater fu = (frmUpdater)this.Owner;


            var groupName = "GRIN-Global Full CD";
            var componentType = "Full CD";
            var description = "Looking for GRIN-Global Full CD...";

            if (rdoGGOnly.Checked) {
                groupName = "GRIN-Global CD";
                componentType = "CD";
                description = "Looking for GRIN-Global CD...";
            } else if (rdoPrerequisites.Checked) {
                groupName = "GRIN-Global Prerequisites";
                componentType = "Prerequisites";
                description = "Looking for GRIN-Global Prerequisites CD...";
            }

            List<FileDownloadInfo> files = null;
            var sd = new SortedDictionary<string, List<FileDownloadInfo>>();

            using (new AutoCursor(this)) {
                files = fu.GetLatestFileInfo(groupName, componentType, description, true, null);
                sd.Add("CD", files);
            }

            if (files.Count > 0) {
                new frmProgress().ShowDialog(this, componentType, sd, false, chkIsMirrorServer.Checked, true, null, false, false, false);
            } else {
                var url = Utility.GetRegSetting("LastURL", "");
                if (Utility.IsWindowsFilePath(url)) {
                    MessageBox.Show(getDisplayMember("DownloadCD{local}", 
                        "You are currently pointing at a local file path.\nNavigate to the following folder in Windows Explorer to see the contents of the GRIN-Global CD:\n\n{0}", Path.GetDirectoryName(url)));
                } else {
                    var uri = new Uri(url);
                    MessageBox.Show(getDisplayMember("DownloadCD{filenotfound}", "The {0} cab file could not be found on the server.\nPlease inform the administrator of {1}.", groupName , uri.Host));
                }
            }

        }

        private void chkUseDefaultProxyCredentials_CheckedChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void chkUseDefaultProxy_CheckedChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void chkHttpProxy_CheckedChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void syncGui(){
            if (chkHttpProxy.Checked) {
                chkUseDefaultProxy.Enabled = true;
                btnTestConnection.Enabled = true;
            } else {
                chkUseDefaultProxy.Enabled = false;
                btnTestConnection.Enabled = false;
            }

            if (chkUseDefaultProxy.Enabled) {
                if (chkUseDefaultProxy.Checked) {
                    gbCustomProxy.Enabled = false;
                } else {
                    gbCustomProxy.Enabled = true;
                }
            } else {
                gbCustomProxy.Enabled = false;
            }

            if (chkUseDefaultProxyCredentials.Checked) {
                lblProxyUsername.Enabled = false;
                txtProxyUsername.Enabled = false;
                lblProxyPassword.Enabled = false;
                txtProxyPassword.Enabled = false;
                lblProxyDomain.Enabled = false;
                txtProxyDomain.Enabled = false;
            } else {
                lblProxyUsername.Enabled = true;
                txtProxyUsername.Enabled = true;
                lblProxyPassword.Enabled = true;
                txtProxyPassword.Enabled = true;
                lblProxyDomain.Enabled = true;
                txtProxyDomain.Enabled = true;
            }
        
        }

        private void frmOptions_FormClosing(object sender, FormClosingEventArgs e) {
            saveRegSettings();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell is DataGridViewLinkCell && cell.Tag != null) {
                Process.Start("explorer.exe", @"/select,""" + cell.Tag + @"""");
            }
        }

        private void btnTestProxy_Click(object sender, EventArgs e) {

            using (new AutoCursor(this)) {
                frmUpdater fu = (frmUpdater)this.Owner;
                GuiExtended gui = null;
                var url = string.Empty;
                try {
                    gui = fu.GetGui();
                    if (gui == null){
                        MessageBox.Show(this,
                            getDisplayMember("TestProxy{invalidurl_body}", "A valid URL could not be determined, or Updater is configured to use a local file resource.  Please check the value specified in \"Download from Server:\" on the main form."),
                            getDisplayMember("TestProxy{invalidurl_title}", "Invalid URL"));
                        return;
                    }
                    gui.WebRequestCreatedCallback = delegate(HttpWebRequest req) {
                        Utility.InitProxySettings(req, chkHttpProxy.Checked, chkUseDefaultProxy.Checked, txtProxyServerName.Text, Utility.ToInt32(txtProxyPort.Text, 0), chkProxyBypassOnLocal.Checked, chkUseDefaultProxyCredentials.Checked, txtProxyUsername.Text, txtProxyPassword.Text, txtProxyDomain.Text, !chkProxySuppress417Errors.Checked);
                    };

                    url = gui.Url;
                    var version = gui.GetVersion();
                    MessageBox.Show(this, 
                        getDisplayMember("TestProxy{success_body}", "Successfully connected to {0} at \n {1}", version, gui.Url), 
                        getDisplayMember("TestProxy{success_title}", "Success - HTTP Proxy Configuration"));
                } catch (Exception ex) {
                    MessageBox.Show(this, 
                        getDisplayMember("TestProxy{failed_body}", "Failed to reach the GRIN-Global web service at \n{0}\n\nError: {1}", url, ex.Message), 
                        getDisplayMember("TestProxy{failed_title}", "Failed - HTTP Proxy Configuration"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void chkIsMirrorServer_CheckedChanged(object sender, EventArgs e) {
            chkMirrorCD.Enabled = chkIsMirrorServer.Checked;
            txtMirrorCD.Enabled = chkIsMirrorServer.Checked;
            txtMirrorServer.Enabled = chkIsMirrorServer.Checked;
            Utility.SetRegSetting("IsMirrorServer", chkIsMirrorServer.Checked ? 1 : 0, false);
        }

        private void chkMirrorCD_CheckedChanged(object sender, EventArgs e) {
            txtMirrorCD.Enabled = chkIsMirrorServer.Checked && chkMirrorCD.Checked;
            Utility.SetRegSetting("MirrorCD", chkMirrorCD.Checked ? 1 : 0, false);
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "Updater", "frmOptions", resourceName, null, defaultValue, substitutes);
        }
    }
}
