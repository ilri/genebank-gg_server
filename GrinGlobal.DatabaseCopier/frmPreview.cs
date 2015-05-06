using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrinGlobal.DatabaseCopier {
	public partial class frmPreview : Form {
		public frmPreview() {
			InitializeComponent();
		}

		private void btnClose_Click(object sender, EventArgs e) {
			Close();
		}

		private void btnCopyToClipboard_Click(object sender, EventArgs e) {
			Clipboard.SetText(txtPreview.Text);
		}

		private void txtPreview_KeyPress(object sender, KeyPressEventArgs e) {
		}

		private void txtPreview_KeyUp(object sender, KeyEventArgs e) {
			if (e.Control){

				if (e.KeyCode == Keys.C) {
					Clipboard.SetText(txtPreview.SelectedText);
				} else if (e.KeyCode == Keys.A) {
					txtPreview.SelectionStart = 0;
					txtPreview.SelectionLength = txtPreview.Text.Length;
				}
			}
		}
	}
}
