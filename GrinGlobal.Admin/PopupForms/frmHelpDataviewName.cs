using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Admin.ChildForms;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmHelpDataviewCategory : GrinGlobal.Admin.ChildForms.frmBase {
        public frmHelpDataviewCategory() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public override void RefreshData() {
            // nothing to do
        }

        private void frmCopyDataviewTo_Load(object sender, EventArgs e) {
        }

        private void btnOK_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void txtHelp_KeyUp(object sender, KeyEventArgs e) {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                e.KeyCode == Keys.A) {
                txtHelp.SelectionStart = 0;
                txtHelp.SelectionLength = txtHelp.Text.Length;
            }
        }
    }
}
