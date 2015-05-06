using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;

namespace GrinGlobal.InstallHelper {
    public partial class frmDatabaseNotFound : Form {
        public frmDatabaseNotFound() {
            InitializeComponent();
        }

        int _installerWindowHandle;
        public DialogResult ShowDialog(int installerWindowHandle) {
            _installerWindowHandle = installerWindowHandle;
            return this.ShowDialog();
        }



        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnPrompt_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }

        bool _activated;
        private void frmDatabaseNotFound_Activated(object sender, EventArgs e) {
            if (!_activated) {
                _activated = true;
                Toolkit.ActivateApplication(this.Handle);
                this.Focus();
                if (_installerWindowHandle > 0) {
                    Toolkit.MinimizeWindow(_installerWindowHandle);
                }
            }
        }
    }
}
