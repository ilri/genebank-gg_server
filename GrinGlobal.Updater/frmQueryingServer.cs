using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrinGlobal.Updater {
    public partial class frmQueryingServer : Form {
        public frmQueryingServer() {
            InitializeComponent();
        }

        private void frmSplash_Load(object sender, EventArgs e) {

        }

        public void Show(string serverName) {
            lblServerName.Text = serverName;
            this.Show();
        }
    }
}
