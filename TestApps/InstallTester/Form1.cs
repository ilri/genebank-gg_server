using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;

namespace InstallTester {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            try {
                var dbeu = DatabaseEngineUtil.CreateInstance("sqlserver", Toolkit.ResolveFilePath("~/gghelper.exe", false), textBox1.Text);
                dbeu.StopService();
                dbeu.StartService();
                MessageBox.Show("SQL Server instance '" + textBox1.Text + "' restarted.");
            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message + "\n" + (ex.InnerException == null ? "" : ex.InnerException.Message));
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            try {
                var dbeu = DatabaseEngineUtil.CreateInstance("sqlserver", Toolkit.ResolveFilePath("~/gghelper.exe", false), textBox1.Text);
                MessageBox.Show("Info:\nServiceName=" + dbeu.ServiceName + "\nBase Directory=" + dbeu.BaseDirectory + "\nBinDirectory=" + dbeu.BinDirectory + "\nDataDirectory=" + dbeu.DataDirectory + "\nEngineName=" + dbeu.EngineName + "\nFriendlyName=" + dbeu.FriendlyName + "\nFullPathToHelperExe=" + dbeu.FullPathToHelperExe + "\nInstallParameter=" + dbeu.InstallParameter + "\nSuperUserName=" + dbeu.SuperUserName);
            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message + "\n" + (ex.InnerException == null ? "" : ex.InnerException.Message));
            }

        }
    }
}
