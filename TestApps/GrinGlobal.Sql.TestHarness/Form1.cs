using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GrinGlobal.Sql;
using GrinGlobal.Sql.GuiData;

namespace GrinGlobal.Sql.TestHarness {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e) {
			using (SecureData sec = new SecureData("brock", false)) {
				MessageBox.Show(sec.CanCreate("crop").ToString());
				MessageBox.Show(sec.CanRead("crop").ToString());
				MessageBox.Show(sec.CanUpdate("crop").ToString());
				MessageBox.Show(sec.CanDelete("crop").ToString());
			}
		}

		DataSet _ds;

		private void btnFill_Click(object sender, EventArgs e) {

			using (GenericDBMS gd = DataFactory.GetGUI("brock", "passw0rd!", true)) {
				_ds = gd.GetInventory("Zea.Ames.GH.2006.SPR");
			}
			DataTable dt = _ds.Tables["iv"];
			bindDataGrid(_ds.Tables["iv"]);
		}

		private void btnSave_Click(object sender, EventArgs e) {
			using (GenericDBMS gd = DataFactory.GetGUI("brock", "passw0rd!", true)){
				_ds = gd.SaveDataSet(_ds);
			}
			DataTable src = null;
			foreach (DataTable dt in _ds.Tables) {
				if (dt.TableName != "ExceptionTable") {
					src = dt;
					break;
				}
			}
			bindDataGrid(src);
		}

		private void btnFill2_Click(object sender, EventArgs e) {
			using (GenericDBMS gd = DataFactory.GetGUI("brock", "passw0rd!", true)) {
//				_ds = gd.GetInventoryAndAccession("Zea.Ames.GH.2006.SPR");
				Dictionary<string, object> dic = new Dictionary<string, object>();
				dic[":groupname"] = "Zea.Ames.GH.2006.SPR";
				_ds = gd.GetData("iv_and_acc", dic, 0);
			}
			bindDataGrid(_ds.Tables["iv_and_acc"]);
		}

		private void bindDataGrid(DataTable src) {
			dgView.DataSource = null;
			dgView.DataSource = src;
			if (src != null) {
				src.WriteXml("dt_data.xml");
				src.WriteXmlSchema("dt_schema.xml");
				src.DataSet.WriteXml("ds_data.xml");
				src.DataSet.WriteXmlSchema("ds_schema.xml");
			}
			foreach (DataGridViewColumn dgvc in dgView.Columns) {
				dgvc.HeaderText = src.Columns[dgvc.Name].Caption;
			}

			if (_ds.Tables["ExceptionTable"].Rows.Count > 0) {
				StringBuilder sb = new StringBuilder("Error(s) during last request:\n");
				foreach (DataRow dr in _ds.Tables["ExceptionTable"].Rows) {
					sb.Append(dr["Message"]);
				}
				MessageBox.Show(sb.ToString());
			}

		}

		private void button2_Click(object sender, EventArgs e) {

			using (GenericDBMS gd = DataFactory.GetGUI("brock", "passw0rd!", true)) {
				_ds = gd.GetViewIV("Zea.Ames.GH.2006.SPR");
			}
			bindDataGrid(_ds.Tables["viewiv"]);

		}

		private void btnCountryToggle_Click(object sender, EventArgs e) {
			using (GenericDBMS gd = DataFactory.GetGUI("brock", "passw0rd!", true)) {
				_ds = gd.ChangeLanguage("DEU");
			}
			bindDataGrid(_ds.Tables["nonexistent"]);
		}

		//private void btnSecUser_Click(object sender, EventArgs e) {
		//    SecUser su = new SecUser();
		//    MessageBox.Show(su.GenerateCreateSql());
		//}

		//private void btnInspectSecUser_Click(object sender, EventArgs e) {
		//    string[] files = GrinGlobal.Core.DatabaseGenerator.TableBase.GenerateClassFiles("prod", "sec_user");
		//    MessageBox.Show(String.Join(", ", files));
		//}

		private void btnSearch_Click(object sender, EventArgs e) {
			using (GenericDBMS gd = DataFactory.GetGUI("brock", "passw0rd!", true)) {
				_ds = gd.Search(txtSearch.Text);
			}
			bindDataGrid(_ds.Tables["query"]);

		}

		//private void btnCreateMappings_Click(object sender, EventArgs e) {
		//    try {
		//        GrinGlobal.Core.DatabaseGenerator.TableBase.DeleteAllMappings("prod");
		//        GrinGlobal.Core.DatabaseGenerator.TableBase.CreateAllMappings("prod");
		//        MessageBox.Show("All done!");
		//    } catch (Exception ex) {
		//        MessageBox.Show(ex.Message);
		//    }
		//}

		//private void btnDeleteMapping_Click(object sender, EventArgs e) {
		//    GrinGlobal.Core.DatabaseGenerator.TableBase.DeleteMapping("prod", txtMapping.Text);

		//}

		private void btnInspect_Click(object sender, EventArgs e) {

		}

		private void btnCreateMappings_Click(object sender, EventArgs e) {

		}
	}
}
