using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using GrinGlobal.Admin.ChildForms;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmElevatePrompt : frmBase {
        public frmElevatePrompt() {
            InitializeComponent();  tellBaseComponents(components);
        }

        private void btnOK_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void frmElevatePrompt_Load(object sender, EventArgs e) {

        }
    }
}
