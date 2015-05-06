using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;

namespace FieldMapperTest {
	public partial class frmSearch : Form {
		public frmSearch() {
			InitializeComponent();
			DialogResult = DialogResult.Cancel;
			gvSearch.ColumnAdded += new DataGridViewColumnEventHandler(gvSearch_ColumnAdded);
			gvSearch.AutoGenerateColumns = true;
		}

		public string DataViewName { get; set; }
		public string DataViewParams { get; set; }
		public string ForeignKeyFieldName { get; set; }
		public string ForeignKeyID { get; set; }
		public string ForeignKeyDisplayValue { get; set; }

		public string DefaultValue { get; set; }

		private void btnSearch_Click(object sender, EventArgs e) {
			try {
				this.Cursor = Cursors.WaitCursor;
//				ggWebSvc.GUI gui = new FieldMapperTest.ggWebSvc.GUI();

				Dictionary<string, string> dic = Toolkit.ParsePairs<string>(DataViewParams);
				dic[":pinumber"] = txtSearch.Text + "%";
				string prms = Toolkit.ConcatPairs(dic);

                //DataSet ds = gui.GetData(false, "brock", "passw0rd!", DataViewName, prms, 0, 1000);
                //gvSearch.DataSource = ds.Tables[DataViewName];
			} finally {
				this.Cursor = Cursors.Default;
			}
		}

		void gvSearch_ColumnAdded(object sender, DataGridViewColumnEventArgs e) {
			// hide the key field
			if (e.Column.Name == ForeignKeyFieldName) {
				e.Column.Visible = false;
			}
		}

		private void gvSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
			// remember the ID so the caller can access it
			DataGridViewColumn dgvc = gvSearch.Columns[ForeignKeyFieldName];
			int colIndex = (dgvc == null ? 0 : dgvc.Index);
			ForeignKeyID = gvSearch[colIndex, e.RowIndex].Value.ToString();
			ForeignKeyDisplayValue = gvSearch[1, e.RowIndex].Value.ToString();
			DialogResult = DialogResult.OK;
			Close();
		}

		private void gvSearch_CellContentClick(object sender, DataGridViewCellEventArgs e) {

		}

		private void frmSearch_Load(object sender, EventArgs e) {
			txtSearch.Text = this.DefaultValue;
		}
	}
}
