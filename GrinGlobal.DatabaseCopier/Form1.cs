using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using GrinGlobal.Core;

namespace GrinGlobal.DatabaseCopier {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void createDelimitedFile(string fileName, char delim, DataTable source, bool firstLineIsColumnNames) {
			using (StreamWriter sw = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))) {
				if (firstLineIsColumnNames) {
					foreach (DataColumn dc in source.Columns) {
						sw.Write(dc.ColumnName);
						sw.Write(delim);
					}
					sw.Write("\r\n");
				}
				foreach (DataRow dr in source.Rows) {
					for (int i=0; i < dr.Table.Columns.Count; i++) {
						string val = dr[i].ToString().Replace(delim.ToString(), @"\" + delim);
						sw.Write(val);
						sw.Write(delim);
					}
					sw.Write("\r\n");
				}
			}
		}

		private void btnSave_Click(object sender, EventArgs e) {
			DialogResult dr = saveFileDialog1.ShowDialog();
			if (dr == DialogResult.OK) {
				// write to file here
				createDelimitedFile(saveFileDialog1.FileName, '\t', (DataTable)dataGridView1.DataSource, true);
			}
		}

		private void btnRun_Click(object sender, EventArgs e) {
			try {
				Cursor = Cursors.WaitCursor;
				using (DataManager dm = DataManager.Create()) {
					dm.Limit = Toolkit.ToInt32(txtLimit.Text, 0);
					DataTable dt = dm.Read(txtSql.Text);
					dataGridView1.DataSource = dt;
					lblRowCount.Text = dt.Rows.Count + " rows returned";
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			} finally {
				Cursor = Cursors.Default;
			}
		}

	}
}
