using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using System.IO;
using System.Xml.XPath;
using GrinGlobal.InstallHelper;
using System.Xml;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmAppSetting : GrinGlobal.Admin.ChildForms.frmBase {
        public frmAppSetting() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public bool ConnectionStringMode;
        private string _settingName;
        private string _settingValue;
        private string _settingProvider;

        public DialogResult ShowDialog(IWin32Window owner, bool connectionStringMode, string settingName, string settingValue, string settingProvider) {

            ConnectionStringMode = connectionStringMode;
            _settingName = settingName;
            _settingValue = settingValue;
            _settingProvider = settingProvider;

            return ShowDialog(owner);
        }

        private string _configFilePath;

        public override void RefreshData() {
            
            var settingName = "" + MainFormCurrentNode().Tag;


            //_configFilePath = Toolkit.ResolveFilePath(Toolkit.GetIISPhysicalPath("gringlobal") + @"\web.config", false);
            _configFilePath = Toolkit.ResolveFilePath(AdminProxy.Connection.WebAppPhysicalPath + @"\web.config", false);

            if (File.Exists(_configFilePath)) {
                bool found = false;
                var nav = new XPathDocument(_configFilePath).CreateNavigator();
                var it = nav.Select("/configuration/appSettings/add");
                while (it.MoveNext()) {

                    if (it.Current.GetAttribute("key", "") == settingName){
                        _settingName = settingName;
                        _settingValue = it.Current.GetAttribute("value", "");
                        ConnectionStringMode = false;
                        found = true;
                        break;
                    }

                }

                if (!found) {
                    var it2 = nav.Select("/configuration/connectionStrings/add");
                    while (it2.MoveNext()) {

                        if (it2.Current.GetAttribute("name", "") == settingName) {
                            _settingName = settingName;
                            _settingValue = it2.Current.GetAttribute("connectionString", "");
                            _settingProvider = it2.Current.GetAttribute("providerName", "");
                            ConnectionStringMode = true;
                            break;
                        }
                    }
                }
            }

            MarkClean();
            MainFormUpdateStatus(getDisplayMember("RefreshData{loaded}", "Loaded Application Setting '{0}' ", settingName), false);

        }

        private void frmSetting_Load(object sender, EventArgs e) {

        }

        private bool _activated;
        private void frmSetting_Activated(object sender, EventArgs e) {
            if (!_activated) {
                _activated = true;
                txtName.Text = _settingName;
                txtName.ReadOnly = true;
                txtValue.Text = _settingValue;

                if (ConnectionStringMode) {
                    lblValue.Text = "Connection String:";
                    ddlProvider.SelectedIndex = ddlProvider.FindString(_settingProvider);
                    btnConnectionStringCreator.Visible = true;
                } else {
                    ddlProvider.Visible = false;
                    lblProvider.Visible = false;
                    btnConnectionStringCreator.Visible = false;
                }

            }
        }

        private void btnOK_Click(object sender, EventArgs e) {
            if (ConnectionStringMode){
                if (!String.IsNullOrEmpty(_settingName) && _settingName != txtName.Text) {
                    // they changed the name.  Remove the old one from the document before adding the new one.
                    this.AdminProxy.DeleteConnectionString(_settingName, _configFilePath);
                }

                this.AdminProxy.SaveConnectionString(txtName.Text, txtValue.Text, ddlProvider.Text, _configFilePath);
            } else {
                if (!String.IsNullOrEmpty(_settingName) && _settingName != txtName.Text){
                    // they changed the name.  Remove the old one from the document before adding the new one.
                    AdminProxy.DeleteApplicationSetting(_settingName, _configFilePath);
                }

                AdminProxy.SaveApplicationSetting(txtName.Text, txtValue.Text, _configFilePath);
            }

            DialogResult = DialogResult.OK;
            if (!Modal) {
                MainFormSelectParentTreeNode();
            } else {
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            if (!Modal) {
                MainFormSelectParentTreeNode();
            } else {
                Close();
            }
        }

        private void frmSetting_FormClosing(object sender, FormClosingEventArgs e) {

        }

        private void frmSetting_FormClosed(object sender, FormClosedEventArgs e) {
        }

        private void button1_Click(object sender, EventArgs e) {

            var dsc = new DataConnectionSpec(ddlProvider.Text, null, null, txtValue.Text);

            var f = new frmDatabaseLoginPrompt();
            var util = DatabaseEngineUtil.CreateInstance(dsc.ProviderName, Toolkit.ResolveFilePath(@".\gguac.exe", false), "SQLExpress");

            var dr = f.ShowDialog(this, util, false, false, false, null, null, false, false);
            if (dr == DialogResult.OK) {
                txtValue.Text = f.txtConnectionstring.Text;
                ddlProvider.SelectedIndex = ddlProvider.FindString(f.DatabaseEngineUtil.EngineName);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtValue_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlProvider_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmAppSetting", resourceName, null, defaultValue, substitutes);
        }
    }
}
