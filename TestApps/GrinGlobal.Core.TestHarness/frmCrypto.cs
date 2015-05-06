using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GrinGlobal.Core;

namespace GrinGlobal.Core.TestHarness {
	public partial class frmCrypto : Form {
		public frmCrypto() {
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e) {
			txtCipher.Text = Crypto.EncryptText(txtPlain.Text);
		}

		private void button2_Click(object sender, EventArgs e) {
			txtPlain.Text = Crypto.DecryptText(txtCipher.Text);
		}
	}
}
