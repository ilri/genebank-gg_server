using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace FieldMapperTest {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
			gvData.AutoGenerateColumns = false;
		}
		private void btnLoad_Click(object sender, EventArgs e) {

			try {

				this.Cursor = Cursors.WaitCursor;

				gvData.Columns.Clear();
				gvData.DataSource = null;

				//string dataview = "GETINVENTORY";
				//string prms = ":tabname=Amaranth;:groupname=Ama.2005field.G;:cno=75227";
                return;

//				new ggWCFSvc().ClearCache(false, "brock", "passw0rd!", null);

				string dataview = "brock_inventory";

                return;

//				DataSet ds = gui.GetData(false, "brock", "passw0rd!", rs, prms, 0, 1000);
                DataSet ds = null;
                DataTable dt = ds.Tables[dataview];
				foreach (DataColumn dc in dt.Columns) {
					DataGridViewColumn dgvc = null;
					if (dc.ExtendedProperties.Count == 0) {
						dgvc = new DataGridViewTextBoxColumn();
						dgvc.HeaderText = dc.Caption;
						dgvc.ReadOnly = false;
						dgvc.DataPropertyName = dc.ColumnName;
					} else {
						string guiHint = dc.ExtendedProperties["gui_hint"].ToString();


						// HACK so I don't have to change the data in sec_table and possibly mess up curator tool
						if (dc.ColumnName.Contains("_display_member")) {
							guiHint = "LARGE_SINGLE_SELECT_CONTROL";
						}

						switch (guiHint) {
							case "TEXT_CONTROL":
								dgvc = initTextBoxColumn(dc);
								break;
							case "TOGGLE_CONTROL":
								dgvc = initCheckBoxColumn(dc);
								break;
							case "SMALL_SINGLE_SELECT_CONTROL":

								// it's a small list according the db, so we go pull it up front
								string fkrs = dc.ExtendedProperties["foreign_key_dataview_name"].ToString();
								string fkrsprm = dc.ExtendedProperties["foreign_key_dataview_param"].ToString();

								// go pull the data for the given dataview...
                                //DataTable dtFK = gui.GetData(true, "brock", "passw0rd!", fkrs, fkrsprm, 0, 0).Tables[fkrs];
                                //dgvc = initDropDownColumn(dc, dtFK);


								break;
							case "LARGE_SINGLE_SELECT_CONTROL":

								// this is a large list. 
								// we don't want to show it -- we want to show the corresponding lookup data
								// we'll use naming convention to find the corresponding field, if any...

								// set this one to invisible
								dgvc = initButtonColumn(dc, gvData);
								break;
							case "DATE_CONTROL":
								dgvc = initDateColumn(dc);
								break;
							default:
								dgvc = initTextBoxColumn(dc);
								break;
						}
						dgvc.HeaderText = dc.ExtendedProperties["friendly_field_name"].ToString();
						dgvc.ReadOnly = dc.ReadOnly || dc.ExtendedProperties["is_readonly"].ToString() == "Y";
						// hide all primary key fields
						//dgvc.Visible = dc.ExtendedProperties["is_primary_key"].ToString() != "Y";


						// HACK!
						if (dc.ColumnName.Contains("_display_member")) {
							dc.ReadOnly = false;
							// find the corresponding id column, mark it as not readonly
							string pkColumn = dc.ColumnName.Replace("_display_member", "");
							foreach (DataColumn dc2 in dt.Columns) {
								if (dc2.ColumnName == pkColumn) {
									dc2.ReadOnly = false;
								}
							}
						}


					}

					if (dgvc.ReadOnly) {
						dgvc.DefaultCellStyle.ForeColor = Color.Gray;
					}

					// remember the datacolumn for later, as it contains stuff we might need to get
					dgvc.Tag = dc;

					gvData.Columns.Add(dgvc);
				}
				gvData.AlternatingRowsDefaultCellStyle.BackColor = Color.LemonChiffon;
				gvData.DataError += new DataGridViewDataErrorEventHandler(gvData_DataError);
				gvData.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(gvData_DataBindingComplete);
				gvData.CellClick += new DataGridViewCellEventHandler(gvData_CellClick);
				//			gvData.AutoResizeColumns();
                gvData.DataSource = ds.Tables[dataview];
			} finally {
				this.Cursor = Cursors.Default;
			}
		}

		void gvData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {
		}

		void gvData_CellClick(object sender, DataGridViewCellEventArgs e) {
			DataGridViewColumn dgvc = gvData.Columns[e.ColumnIndex];

			if (dgvc is DataGridViewButtonColumn) {
				// they clicked a button column

				// pull DataColumn from the tag
				DataColumn dc = (DataColumn)dgvc.Tag;

				// from that, determine the associated key
				DataColumn dcPK = dc.Table.Columns[dc.ColumnName.Replace("_display_member", "")];

				// load the search form, copy over dataview info, show it modally
				frmSearch fs = new frmSearch();
				fs.DataViewName = (string)dcPK.ExtendedProperties["foreign_key_dataview_name"];
				fs.DataViewParams = (string)dcPK.ExtendedProperties["foreign_key_dataview_param"];
				fs.ForeignKeyFieldName = dcPK.ColumnName;
				fs.DefaultValue = (string)gvData[e.ColumnIndex, e.RowIndex].Value.ToString();
				DialogResult dr = fs.ShowDialog(this);
				if (dr == DialogResult.OK) {

					// we know what to set the _display_member column to (it was the column that was clicked)
					// however, we have to figure out which column int he gridview the actual foreign key id is held by
					gvData.EditMode = DataGridViewEditMode.EditProgrammatically;
					gvData.BeginEdit(false);
					for(int i=0;i<gvData.Columns.Count;i++){
						DataColumn col = (DataColumn)gvData.Columns[i].Tag;
						if (col.ColumnName == dcPK.ColumnName) {
							DataGridViewTextBoxCell tb = (DataGridViewTextBoxCell)gvData[i, e.RowIndex];
							tb.Value = Toolkit.ToInt32(fs.ForeignKeyID, 0);
							break;
						}
					}

					DataGridViewButtonCell btn = (DataGridViewButtonCell)gvData[e.ColumnIndex, e.RowIndex];
					btn.Value = fs.ForeignKeyDisplayValue;
					gvData.EndEdit();

				}

//				MessageBox.Show("This is where a search form would pop up, passing it " + gvData[e.ColumnIndex, e.RowIndex].Value.ToString() + " and the dataview/params of " + fkrs + "/" + fkrsparam);

			}

		}

		void gvData_DataError(object sender, DataGridViewDataErrorEventArgs e) {
			DataGridViewColumn dgvc = gvData.Columns[e.ColumnIndex];
			if (dgvc is DataGridViewCheckBoxColumn) {
				// formatting error(s). fix and ignore.
				string val = gvData[e.ColumnIndex, e.RowIndex].Value.ToString();
				switch(val.ToUpper()){
					case "Y":
					case "X":
						gvData[e.ColumnIndex, e.RowIndex].Value = "Y";
						break;
					default:
						gvData[e.ColumnIndex, e.RowIndex].Value = "N";
						break;
				}
				e.Cancel = true;
			}
		}

		private DataGridViewTextBoxColumn initTextBoxColumn(DataColumn dc) {
			DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
			col.DataPropertyName = dc.ColumnName;
			col.ValueType = dc.DataType;
			if (dc.ExtendedProperties.Count > 0) {
				col.MaxInputLength = Toolkit.ToInt32(dc.ExtendedProperties["max_length"], -1);

				// never show primary keys
				if (dc.ExtendedProperties["is_primary_key"].ToString() == "Y") {
					col.Visible = false;
				}
			}
			return col;
		}

		private DataGridViewCheckBoxColumn initCheckBoxColumn(DataColumn dc) {
			DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
			col.DataPropertyName = dc.ColumnName;
			col.ValueType = dc.DataType;
			col.TrueValue = "Y";
			col.FalseValue = "N";
			return col;
		}

		private DataGridViewComboBoxColumn initDropDownColumn(DataColumn dc, DataTable lookupData) {
			DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
			col.DataPropertyName = dc.ColumnName;
			col.ValueType = dc.DataType;
			if (lookupData != null) {
				col.ValueMember = lookupData.Columns["value_member"].ColumnName;
				col.DisplayMember = lookupData.Columns["display_member"].ColumnName;
			}
			col.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
			col.DataSource = lookupData;
			return col;
		}

		private DataGridViewButtonColumn initButtonColumn(DataColumn dc, DataGridView dgv ) {
			DataGridViewButtonColumn col = new DataGridViewButtonColumn();
			col.DataPropertyName = dc.ColumnName;
			col.ValueType = dc.DataType;
			col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			return col;
		}

		private CalendarColumn initDateColumn(DataColumn dc) {
			CalendarColumn col = new CalendarColumn();
			col.DataPropertyName = dc.ColumnName;
			col.ValueType = dc.DataType;
			return col;
		}

		private void Form1_Load(object sender, EventArgs e) {

		}

		private void gvData_CellContentClick(object sender, DataGridViewCellEventArgs e) {

		}

        private void btnEncrypt_Click(object sender, EventArgs e) {
            if (txtPlain.Text.Length > 0) {
                txtCipher.Text = Crypto.EncryptText(txtPlain.Text);
            } else if (txtCipher.Text.Length > 0) {
                txtPlain.Text = Crypto.DecryptText(txtCipher.Text);
            }
        }

        void copyDirectory(string srcPath, string destPath, bool recurse) {
            if (!Directory.Exists(destPath)) {
                Directory.CreateDirectory(destPath);
            }
            foreach (string f in Directory.GetFiles(srcPath)) {
                File.Copy(f, destPath + @"\" + new FileInfo(f).Name, true);
            }
            if (recurse) {
                foreach (string d in Directory.GetDirectories(srcPath)) {
                    string newPath = destPath + @"\" + new DirectoryInfo(d).Name;
                    copyDirectory(d, newPath, true);
                }
            }
        }


        private void button1_Click(object sender, EventArgs e) {


            localhost.GUI gui = new FieldMapperTest.localhost.GUI();


            DataSet ds = gui.GetData(false, "admin1", "admin1", ":select * from image", null, 0, 0);
            foreach (DataRow dr in ds.Tables["Table0"].Rows) {
                // try to retrieve the image file
                try {
                    byte[] bytes = gui.DownloadImage(dr["virtual_path"].ToString());
                    Image img = Image.FromStream(new MemoryStream(bytes, false));
                    pictureBox1.Image = img;
                } catch (Exception ex) {
                    Debug.WriteLine(ex.Message);
                }
            }

            string uid = "admin1";
            string pw = "admin1";

            Image img2 = Image.FromStream(new MemoryStream(gui.DownloadImage("~/uploads/images/test2.gif"), false));
            pictureBox1.Image = img2;

            string filename = "~/uploads/images/hi/how/are/you/test2.gif";

            using (MemoryStream ms = new MemoryStream()) {
                img2.Save(ms, ImageFormat.Gif);
                gui.UploadImage(uid, pw, filename, ms.ToArray(), true, false);
                gui.UploadImage(uid, pw, filename, ms.ToArray(), true, false);
            }

            gui.DeleteImage(uid, pw, filename);

            //copyDirectory(@"C:\projects\GrinGlobal\Documentation\Standalone_Mode_Install", @"C:\projects\__gg", true);

        }

	}
}
