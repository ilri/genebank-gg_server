using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrinGlobal.DatabaseCopier {
    public partial class frmProgress : Form {
        public frmProgress() {
            InitializeComponent();
        }

        private void frmProgress_Load(object sender, EventArgs e) {
            timer1.Start();
        }

        private void btnDone_Click(object sender, EventArgs e) {
            Close();
        }

        Random _ran = new Random();
        private void timer1_Tick(object sender, EventArgs e) {
            var nextVal = (progressBar1.Value + _ran.Next(1, 5));
            if (nextVal >= progressBar1.Maximum - 5){
                if (nextVal > progressBar1.Maximum) {
                    nextVal = progressBar1.Minimum;
                } else {
                    nextVal = progressBar1.Maximum;
                }
            }
            progressBar1.Value = nextVal;
        }

        private void frmProgress_FormClosing(object sender, FormClosingEventArgs e) {
            if (timer1 != null) {
                timer1.Stop();
            }
        }
    }
}
