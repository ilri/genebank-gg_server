using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using GrinGlobal.Core.Xml;
using GrinGlobal.Core;
using GrinGlobal.Admin.ChildForms;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmOptions : frmBase {

        public frmOptions() {
            InitializeComponent();  tellBaseComponents(components);
        }

        private void saveSettings() {
            //Toolkit.SaveRegSetting(mdiParent.HIVE, "GeneralAutoPopulate", chkGeneralAutoPopulate.Checked.ToString(), false);
            Toolkit.SaveRegSetting(mdiParent.HIVE, "DataviewAutoSynchronize", chkDataviewAutoSynchronize.Checked.ToString(), false);
            Toolkit.SaveRegSetting(mdiParent.HIVE, "DataviewShowAllEngines", chkDataviewShowAllEngines.Checked.ToString(), false);
            //Toolkit.SaveRegSetting(mdiParent.HIVE, "SearchEngineNotifyOnIndexProcessingCompleted", chkSearchEngineNotifyMe.Checked.ToString(), false);
        }

        public void loadSettings() {
            //chkGeneralAutoPopulate.Checked = ("" + Toolkit.GetRegSetting(mdiParent.HIVE, "GeneralAutoPopulate", "false")).ToLower() == "true";
            chkDataviewAutoSynchronize.Checked = ("" + Toolkit.GetRegSetting(mdiParent.HIVE, "DataviewAutoSynchronize", "true")).ToLower() == "true";
            chkDataviewShowAllEngines.Checked = ("" + Toolkit.GetRegSetting(mdiParent.HIVE, "DataviewShowAllEngines", "false")).ToLower() == "true";
            //chkSearchEngineNotifyMe.Checked = ("" + Toolkit.GetRegSetting(mdiParent.HIVE, "SearchEngineNotifyOnIndexProcessingCompleted", "true")).ToLower() == "true";
        }

        //public bool AutoPopulate { get { return chkGeneralAutoPopulate.Checked; } }

        private void btnOK_Click(object sender, EventArgs e) {
            saveSettings();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmOptions_Load(object sender, EventArgs e) {
            mdiParent p = this.Owner as mdiParent;
            loadSettings();
        }
    }
}
