using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;


namespace GrinGlobal.WebService.TestHarness {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			_gui = new ggws.GUI();
		}

		ggws.GUI _gui;

		private void button1_Click(object sender, EventArgs e) {
			Cursor = Cursors.WaitCursor;
			for (int i=0; i < 500; i++) {
                int timeout = 90000;
                int.TryParse(ConfigurationSettings.AppSettings["WebServiceTimeout"], out timeout);
                _gui.Timeout = timeout;
				DataSet ds = _gui.GetAccessions(true, "brock", "passw0rd!", "Zea.Ames.95.GH.START");
				label1.Text = i.ToString();
				Application.DoEvents();
			}
			Cursor = Cursors.Default;
			MessageBox.Show("Done");
		}

		private void button2_Click(object sender, EventArgs e) {
			DataSet ds = _gui.GetInventory(false, "brock", "passw0rd!", "Ama.Test");
			if (ds.Tables["PROD.IV"].Rows.Count > 0) {
				string newval = new Random().Next(1,99999999).ToString();
				ds.Tables["PROD.IV"].Rows[0]["LOC2"] = newval;
				_gui.SaveDataSet(false, "brock", "passw0rd!", ds);
				MessageBox.Show("new LOC2 = " + newval + " for ivid=" + ds.Tables["PROD.IV"].Rows[0]["ivid"]);
			} else {
				MessageBox.Show("No rows found to save");
			}
		}
	}
}
