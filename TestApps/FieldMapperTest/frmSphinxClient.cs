using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using GrinGlobal.Sql;
using GrinGlobal.Business;

namespace FieldMapperTest {
	public partial class frmSphinxClient : Form {
		public frmSphinxClient() {
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e) {

			try {
//                Cursor = Cursors.WaitCursor;

//                SearchClient sc = new SearchClient("localhost", 3312);
//                SearchQuery sq = new SearchQuery(txtSearch.Text);

//                sq.Mode = (MatchingMode)ddlMode.SelectedIndex;
//                //sq.SelectList = "*";
////				sq.SelectList = "'acc' as TableName";
//                //if (chkACC.Checked && chkIV.Checked && chkORD.Checked && chkCOOP.Checked) {
//                    sq.Index = "*";
//                //} else {
//                //    sq.Index = String.Empty;
//                //    if (chkACC.Checked) {
//                //        sq.Index += "acc ";
//                //    }
//                //    if (chkIV.Checked) {
//                //        sq.Index += "iv ";
//                //    }
//                //    if (chkORD.Checked) {
//                //        sq.Index += "ord ";
//                //    }
//                //    if (chkCOOP.Checked) {
//                //        sq.Index += "coop ";
//                //    }

//                //    if (sq.Index == String.Empty) {
//                //        sq.Index = "*";
//                //    }
//                //}
//                sq.Index = sq.Index.Trim();

////				sq.Index = "ord_coop";

//                List<SearchResult> results = sc.Execute(sq);

//                lbResults.Items.Clear();

//                foreach (SearchResult sr in results) {
//                    if (!String.IsNullOrEmpty(sr.Error)) {
//                        MessageBox.Show(sr.Error);
//                    } else {
//                        foreach (SearchMatch sm in sr.Matches) {
//                            lbResults.Items.Add("ID=" + sm.ID + ", weight=" + sm.Weight + ", IDType=" + sm.IDType.ToString() + ", fkid=" + sm.ForeignKeys);
//                        }
//                    }
//                }
			} finally {
				Cursor = Cursors.Default;
			}

		}

		private void groupBox1_Enter(object sender, EventArgs e) {

		}

		private void frmSphinxClient_Load(object sender, EventArgs e) {
			ddlMode.SelectedIndex = ddlMode.Items.Count-1;
		}
	}
}
