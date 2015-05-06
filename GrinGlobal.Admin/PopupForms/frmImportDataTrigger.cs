using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using GrinGlobal.Interface.DataTriggers;
using GrinGlobal.InstallHelper;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmImportDataTrigger : GrinGlobal.Admin.ChildForms.frmBase {
        public frmImportDataTrigger() {
            InitializeComponent();
            MakeListViewSortable(lv);
        }

        private class Resource {
            public int ID;
            public string Name;
        }

        private class TagInfo {
            public List<Resource> Dataviews;
            public List<Resource> Tables;
            public string VirtualPath;
            public string AssemblyName;
            public string ClassName;
            public string Title;
            public string Description;
        }

        public override void RefreshData() {
            // nothing to do
        }

        private void frmImportDataTriggers_Load(object sender, EventArgs e) {
            showImport();
        }


        private string getVirtualFolder() {
            return "~/bin/";
            //return "~/uploads/datatriggers/";
        }

        private string _physicalFolder;
        private string getPhysicalFolder() {
            if (String.IsNullOrEmpty(_physicalFolder)) {
                var webAppRoot = AdminProxy.Connection.WebAppPhysicalPath; // Toolkit.GetIISPhysicalPath("gringlobal");
                if (Debugger.IsAttached) {
                    // HACK: for debugging, assume we're on localhost2600, meaning the IIS lookup to "Default Web Site" will be wrong.
                    if (!Environment.CurrentDirectory.ToLower().Contains(webAppRoot.ToLower())) {
                        webAppRoot = @"C:\projects\GrinGlobal\GrinGlobal.Web\";
                    }
                }
                var physPath = webAppRoot + getVirtualFolder().Replace("~/", @"\");
                _physicalFolder = Toolkit.ResolveDirectoryPath(physPath, false);
            }
            return _physicalFolder;
        }


        private void showImport() {
            ofdDataTrigger.Multiselect = false;
            ofdDataTrigger.Title = "Select .NET Assembly Containing the GRIN-Global Data Trigger Class(es)";
            ofdDataTrigger.CheckFileExists = true;
            ofdDataTrigger.CheckPathExists = true;
            ofdDataTrigger.Filter = ".NET Assembly files (*.dll)|*.dll|All Files (*.*)|*.*";
            if (DialogResult.OK == ofdDataTrigger.ShowDialog(this)) {


                // file is not in the triggers path the website is expecting it to be.
                // copy it there now.
                var fname = Path.GetFileName(ofdDataTrigger.FileName);
//                if (!ofdDataTrigger.FileName.ToLower().Contains(getPhysicalFolder().ToLower())) {
//                    var newPath = Toolkit.ResolveFilePath(getPhysicalFolder() + @"\" + fname, true);
//                    //if (File.Exists(newPath)) {
//                    //    // TODO: prompt before deletion of file web app is supposed to have control over??
//                    //    try {
//                    //        File.Delete(newPath);
//                    //    } catch {
//                    //    }
//                    //}
//                    try {
//                        // remove any alternate data streams in case it was downloaded from the internet (otherwise ASPNET won't have access to it)
//                        if (File.Exists(ofdDataTrigger.FileName + ".old")) {
//                            File.Delete(ofdDataTrigger.FileName + ".old");
//                        }
//                        File.Move(ofdDataTrigger.FileName, ofdDataTrigger.FileName + ".old");
//                        if (File.Exists(ofdDataTrigger.FileName + ".bat")) {
//                            File.Delete(ofdDataTrigger.FileName + ".bat");
//                        }
//                        File.WriteAllText(ofdDataTrigger.FileName + ".bat", String.Format(@"
//@ECHO OFF
//type ""{0}"" > ""{1}""
//", ofdDataTrigger.FileName + ".old", ofdDataTrigger.FileName));


//                        using (var splash = new frmSplash()) {
//                            splash.ChangeText("Stopping web server...");


//                            var psi2 = new ProcessStartInfo();
//                            psi2.FileName = "iisreset.exe";
//                            psi2.Arguments = "/stop";
//                            psi2.CreateNoWindow = true;
//                            psi2.WindowStyle = ProcessWindowStyle.Hidden;
//                            psi2.UseShellExecute = false;
//                            var p2 = Process.Start(psi2);
//                            p2.WaitForExit();


//                            splash.ChangeText("Copying in data trigger file...");

//                            var psi = new ProcessStartInfo();
//                            psi.FileName = ofdDataTrigger.FileName + ".bat";
//                            psi.CreateNoWindow = true;
//                            psi.WindowStyle = ProcessWindowStyle.Hidden;
//                            psi.UseShellExecute = false;
//                            var p = Process.Start(psi);
//                            p.WaitForExit();


//                            File.Copy(ofdDataTrigger.FileName, newPath, true);

//                            if (File.Exists(ofdDataTrigger.FileName + ".bat")) {
//                                File.Delete(ofdDataTrigger.FileName + ".bat");
//                            }

//                            splash.ChangeText("Starting web server...");

//                            var psi3 = new ProcessStartInfo();
//                            psi3.FileName = "iisreset.exe";
//                            psi3.Arguments = "/start";
//                            psi3.CreateNoWindow = true;
//                            psi3.WindowStyle = ProcessWindowStyle.Hidden;
//                            psi3.UseShellExecute = false;
//                            var p3 = Process.Start(psi3);
//                            p3.WaitForExit();
//                        }

//                        fname = newPath;

//                    } catch (Exception ex){
//                        MessageBox.Show(this, "Could not load assembly: " + ex.Message, "Invalid Assembly File", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
//                        DialogResult = DialogResult.Cancel;
//                        this.Close();
//                        return;
//                    }
//                }
                if (!inspectAssemblyFile(fname)) {
                    MessageBox.Show(this, getDisplayMember("showImport{invalid_body}", "No valid Data Trigger classes were found in assembly '{0}'.", fname), 
                        getDisplayMember("showImport{invalid_title}", "Invalid Assembly File"), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    DialogResult = DialogResult.Cancel;
                    this.Close();
                }

            }
        }

        private void ofdDataTrigger_FileOk(object sender, CancelEventArgs e) {

        }


        private AppDomain _alternateDomain;
        /// <summary>
        /// This inspects the file at txtFilePath.Text and sets the lblAssemblyName appropriately and repopulates the ddlClass dropdown.
        /// </summary>
        private bool inspectAssemblyFile(string assemblyPath) {

            lv.Items.Clear();

            if (!File.Exists(assemblyPath)) {
                // not a valid file specified, just bomb out of the remaining processing
                return false;
            }


            var fileName = new FileInfo(assemblyPath).Name;

            var virtualPath = getVirtualFolder() + fileName;

            //// newpath now contains the final resting place for the file.
            //// reflect on it, see what assembly(ies) and valid class(es) it contains
            //if (_alternateDomain != null) {
            //    AppDomain.Unload(_alternateDomain);
            //    _alternateDomain = null;
            //}

            //var curSetup = AppDomain.CurrentDomain.SetupInformation;

            //var altSetup = new AppDomainSetup();
            //altSetup.ActivationArguments = curSetup.ActivationArguments;
            //altSetup.ApplicationBase = new FileInfo(assemblyPath).DirectoryName;
            //altSetup.ApplicationName = "TestLoadDomain";
            //altSetup.PrivateBinPath = altSetup.ApplicationBase;

            //_alternateDomain = AppDomain.CreateDomain("FriendlyDomainName", AppDomain.CurrentDomain.Evidence, altSetup);
            //foreach (Assembly asm1 in AppDomain.CurrentDomain.GetAssemblies()) {
            //    if (!asm1.FullName.Contains("vshost") && !asm1.FullName.Contains("GrinGlobal.Admin") && !asm1.FullName.Contains("VisualStudio")) {
            //        try {
            //            _alternateDomain.Load(asm1.FullName);
            //            Debug.WriteLine(asm1.FullName);
            //        } catch (Exception ex) {
            //            Debug.WriteLine("Could not load " + asm1.FullName + ": " + ex.Message);
            //        }
            //    }
            //}
            //var asname = AssemblyName.GetAssemblyName(assemblyPath);
            //var asm = _alternateDomain.Load(asname);

            //var assemblyBytes = File.ReadAllBytes(assemblyPath);
            //asm = _alternateDomain.Load(assemblyBytes);

            var asm = Assembly.LoadFrom(assemblyPath);

            //var asm = Assembly.LoadFrom(assemblyPath);
            var assemblyName = asm.FullName.Split(',')[0];



            var exportedTypes = asm.GetExportedTypes();
            Type[] interfaces = new Type[] { 
                typeof(GrinGlobal.Interface.DataTriggers.IDataviewSaveDataTrigger), 
                typeof(GrinGlobal.Interface.DataTriggers.IDataviewReadDataTrigger),
                typeof(GrinGlobal.Interface.DataTriggers.ITableSaveDataTrigger), 
                typeof(GrinGlobal.Interface.DataTriggers.ITableReadDataTrigger) };

            foreach (var t in exportedTypes) {
                foreach (var iface in interfaces) {
                    if (ContainsInterface(t, iface)) {
                        // add to the list
                        var lvi = inspectClass(t, fileName, virtualPath);
                        if (lvi != null) {
                            lv.Items.Add(lvi);
                        }
                        break;
                    }
                }
            }
            return lv.Items.Count > 0;
        }

        private ListViewItem inspectClass(Type t, string fileName, string virtualPath) {
            var lvi = new ListViewItem();
            var ti = new TagInfo { Dataviews = new List<Resource>(), Tables = new List<Resource>(), VirtualPath = virtualPath, AssemblyName = fileName, ClassName = t.FullName };

            lvi.Tag = ti;

            //var o = Activator.CreateInstance(_alternateDomain, fileName, t.FullName).Unwrap();
            var o = Activator.CreateInstance(t);

            var resourceNames = "(Any)";
            string singleResource = null;
            var rez = o as IDataResource;
            if (rez != null && rez.ResourceNames != null && rez.ResourceNames.Length > 0) {
                resourceNames = String.Join(", ", rez.ResourceNames);
                singleResource = rez.ResourceNames[0];
            }

            var dvText = "-";
            var tblText = "-";

            if (o is IDataviewReadDataTrigger || o is IDataviewSaveDataTrigger) {
                // dataview trigger
                dvText = resourceNames;
                foreach (var r in rez.ResourceNames) {
                    ti.Dataviews.Add(new Resource { ID = base.getDataviewID(r), Name = r });
                }
            }
            if (o is ITableReadDataTrigger || o is ITableSaveDataTrigger){
                // assume table trigger
                tblText = resourceNames;
                foreach (var r in rez.ResourceNames) {
                    ti.Tables.Add(new Resource { ID = base.getTableID(r), Name = r });
                }
            }

            lvi.Text = dvText;
            lvi.SubItems.Add(tblText);

            // class name
            lvi.SubItems.Add(t.Name);

            var desc = o as IDataTriggerDescription;
            if (desc != null) {
                // lvi.SubItems.Add(desc.GetTitle("en-US"));
                ti.Title = desc.GetTitle("en-US");
                ti.Description = desc.GetDescription("en-US");
                lvi.SubItems.Add(ti.Description);
            }


            return lvi;

        }

        private bool ContainsInterface(Type type, Type iface) {
            var typeInterfaces = type.GetInterfaces();
            foreach (var ti in typeInterfaces) {
                if (ti == iface) {
                    return true;
                }
            }
            var subtypes = type.GetNestedTypes();
            foreach (var st in subtypes) {
                if (ContainsInterface(st, iface)) {
                    return true;
                }
            }
            return false;

        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnImport_Click(object sender, EventArgs e) {

            var popup = new StringBuilder();

            var fname = Path.GetFileName(ofdDataTrigger.FileName);

            // remove any alternate data streams in case it was downloaded from the internet (otherwise ASPNET won't have access to it)
            // we do this by using the type orig_file > new_file hack
            if (File.Exists(ofdDataTrigger.FileName + ".old")) {
                File.Delete(ofdDataTrigger.FileName + ".old");
            }
            File.Move(ofdDataTrigger.FileName, ofdDataTrigger.FileName + ".old");
            if (File.Exists(ofdDataTrigger.FileName + ".bat")) {
                File.Delete(ofdDataTrigger.FileName + ".bat");
            }
            File.WriteAllText(ofdDataTrigger.FileName + ".bat", String.Format(@"
@ECHO OFF
type ""{0}"" > ""{1}""
", ofdDataTrigger.FileName + ".old", ofdDataTrigger.FileName));
            var psi = new ProcessStartInfo();
            psi.FileName = ofdDataTrigger.FileName + ".bat";
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            var p = Process.Start(psi);
            p.WaitForExit();



            if (!ofdDataTrigger.FileName.ToLower().Contains(getPhysicalFolder().ToLower())) {
                // file is not in the triggers path the website is expecting it to be.
                // copy it there now.

                var newPath = Toolkit.ResolveFilePath(getPhysicalFolder() + @"\" + fname, true);
                try {

                    using (var splash = new frmSplash()) {

                        // stop IIS (to make sure no processes are holding onto it
                        splash.ChangeText(getDisplayMember("import{stoppingiis}", "Stopping web server..."));


                        var psi2 = new ProcessStartInfo();
                        psi2.FileName = "iisreset.exe";
                        psi2.Arguments = "/stop";
                        psi2.CreateNoWindow = true;
                        psi2.WindowStyle = ProcessWindowStyle.Hidden;
                        psi2.UseShellExecute = false;
                        var p2 = Process.Start(psi2);
                        p2.WaitForExit();


                        // copy the file in
                        splash.ChangeText(getDisplayMember("import{copyingtriggerfile}", "Copying in data trigger file..."));

                        File.Copy(ofdDataTrigger.FileName, newPath, true);

                        if (File.Exists(ofdDataTrigger.FileName + ".bat")) {
                            File.Delete(ofdDataTrigger.FileName + ".bat");
                        }

                        splash.ChangeText(getDisplayMember("import{startingiis}", "Starting web server..."));

                        var psi3 = new ProcessStartInfo();
                        psi3.FileName = "iisreset.exe";
                        psi3.Arguments = "/start";
                        psi3.CreateNoWindow = true;
                        psi3.WindowStyle = ProcessWindowStyle.Hidden;
                        psi3.UseShellExecute = false;
                        var p3 = Process.Start(psi3);
                        p3.WaitForExit();
                    }

                    fname = newPath;

                } catch (Exception ex) {
                    MessageBox.Show(this, getDisplayMember("import{failed_body}", "Could not load assembly: {0}", ex.Message),
                        getDisplayMember("import{failed_title}", "Invalid Assembly File"), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }
            }

            // write database entries (safe save - by trigger name, dataview name/ tablename
            //

            foreach (ListViewItem lvi in lv.Items) {
                var ti = lvi.Tag as TagInfo;

                // add mappings for ones tied to a dataview
                foreach (var r in ti.Dataviews) {
                    if (r.ID > -1) {
                        AdminProxy.SaveTrigger(-1, r.ID, -1, ti.VirtualPath, ti.AssemblyName, ti.ClassName, ti.Title, ti.Description, true, false);
                    } else {
                        popup.Append("Could not locate dataview '" + r.Name + "' to map data trigger '" + ti.ClassName + "'.");
                    }
                }

                // add mappings for ones tied to a table
                foreach (var r in ti.Tables) {
                    if (r.ID > -1) {
                        AdminProxy.SaveTrigger(-1, -1, r.ID, ti.VirtualPath, ti.AssemblyName, ti.ClassName, ti.Title, ti.Description, true, false);
                    } else {
                        popup.Append("Could not locate table '" + r.Name + "' to map data trigger '" + ti.ClassName + "'.");
                    }
                }

                // add mapping for ones tied to neither
                if (ti.Tables.Count == 0 && ti.Dataviews.Count == 0) {
                    AdminProxy.SaveTrigger(-1, -1, -1, ti.VirtualPath, ti.AssemblyName, ti.ClassName, ti.Title, ti.Description, true, false);
                }
            }

            if (popup.Length > 0) {
                var f = new frmMessageBox();
                f.txtMessage.Text = popup.ToString();
                f.btnNo.Text = "OK";
                f.btnYes.Visible = false;
                f.ShowDialog(this);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmImportDataTrigger", resourceName, null, defaultValue, substitutes);
        }
    }
}
