using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Admin.ChildForms;
using System.IO;
using GrinGlobal.Core;
using GrinGlobal.InstallHelper;
using System.Diagnostics;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmLogViewer : frmBase {
        public frmLogViewer() {
            InitializeComponent();
        }

        private void btnDone_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void lv_SelectedIndexChanged(object sender, EventArgs e) {
            if (lv.SelectedItems.Count > 0){
                txtMessage.Text = lv.SelectedItems[0].SubItems[2].Text;
            } else {
                txtMessage.Text = "(choose an item above to view more information about it)";
            }
        }

        private DataTable _data;
        public DialogResult ShowDialog(IWin32Window owner, DataTable data) {
            _data = data;
            return ShowDialog(owner);
        }

        private void frmLogViewer_Load(object sender, EventArgs e) {
            MakeListViewSortable(lv);
            RefreshData();
        }

        private StringBuilder _log;

        public override void RefreshData() {
            lv.Items.Clear();
            txtMessage.Text = "";
            _log = new StringBuilder();
            if (_data != null) {
                var listItems = new List<ListViewItem>();
                foreach (DataRow dr in _data.Rows) {
                    var lvi = new ListViewItem(((DateTime)dr["date"]).ToString("yyyy/MM/dd h:mm:ss tt"));
                    lvi.SubItems.Add(dr["type"].ToString());
                    lvi.SubItems.Add(dr["message"].ToString());
                    lvi.ToolTipText = dr["message"].ToString();
                    _log.AppendLine(String.Format("{0} - {1} - {2}", lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text).Replace("\r", "\\r").Replace("\n", "\\n"));
                    listItems.Add(lvi);
                }
                lv.Items.AddRange(listItems.ToArray());
            }
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e) {
            selectAllIfNeeded(sender as TextBox, e);
        }

        private void btnShowInNotepad_Click(object sender, EventArgs e) {
            var tempFile = Toolkit.ResolveFilePath(Utility.GetTempDirectory() + @"\search_engine_log.txt", true);
            File.WriteAllText(tempFile, _log.ToString());
            Process.Start(tempFile);
        }
    }
}
