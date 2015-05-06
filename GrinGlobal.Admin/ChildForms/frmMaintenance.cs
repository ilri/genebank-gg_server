using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using GrinGlobal.InstallHelper;
using GrinGlobal.Core;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmMaintenance : GrinGlobal.Admin.ChildForms.frmBase {
        public frmMaintenance() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public override void RefreshData() {
            // do nothing yet
            initHooksForMdiParent(null, null, null);
        }

        private void frmMaintenance_Load(object sender, EventArgs e) {
            syncGui();
        }

        private void syncGui() {
            //Sync(false, delegate() {


            //});
        }

        private void rdoCodeValue_CheckedChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void rdoTraits_CheckedChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void lv_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                jumpToForm();
            }
        }

        private void jumpToForm() {
            if (lv.SelectedItems.Count > 0) {
                var todo = lv.SelectedItems[0].Tag + string.Empty;
                MainFormSelectChildTreeNode(todo);
            }
        }

        private void lv_MouseDoubleClick(object sender, MouseEventArgs e) {
            jumpToForm();
        }
    }
}
