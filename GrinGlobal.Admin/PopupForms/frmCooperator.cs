using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using GrinGlobal.Admin.ChildForms;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmCooperator : frmBase {
        public frmCooperator() {
            InitializeComponent();  tellBaseComponents(components);
        }

        private void txtSearch_Leave(object sender, EventArgs e) {
            this.AcceptButton = btnOK;
        }

        private void txtSearch_Enter(object sender, EventArgs e) {
            this.AcceptButton = btnGo;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e) {
            if (lvCooperators.SelectedItems.Count == 0) {
                MessageBox.Show(getDisplayMember("ok{nocooperator}", "Please select a cooperator."));
                return;
            }
            CooperatorID = Toolkit.ToInt32(lvCooperators.SelectedItems[0].Tag, -1);
            DialogResult = DialogResult.OK;
            Close();
        }

        public int CooperatorID;

        private void btnGo_Click(object sender, EventArgs e) {
            using (new AutoCursor(this)) {
                var ds = _proxy.SearchCooperators(txtSearch.Text);
                var dt = ds.Tables["search_cooperators"];
                lvCooperators.Items.Clear();
                foreach (DataRow dr in dt.Rows) {
                    var lvi = new ListViewItem();
                    lvi.Tag = dr["cooperator_id"];
                    lvi.Text = dr["last_name"].ToString();
                    lvi.SubItems.Add(dr["first_name"].ToString());
                    lvi.SubItems.Add(dr["organization"].ToString());
                    lvi.SubItems.Add(dr["cooperator_id"].ToString() == dr["current_cooperator_id"].ToString() ? "Y" : "N");
                    lvCooperators.Items.Add(lvi);
                }
                lblCount.Text = "Found " + lvCooperators.Items.Count + " cooperator(s)";
            }
        }

        mdiParent _parent;
        AdminHostProxy _proxy;

        private void frmCooperator_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvCooperators);
        }

        public DialogResult ShowDialog(mdiParent parent, AdminHostProxy proxy) {
            _parent = parent;
            _proxy = proxy;
            return this.ShowDialog(parent);
        }

        private void lvCooperators_MouseDoubleClick(object sender, MouseEventArgs e) {
            btnOK.PerformClick();
        }

        private bool _activated;
        private void frmCooperator_Activated(object sender, EventArgs e) {
            if (!_activated) {
                _activated = true;
                txtSearch.Focus();

            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) {
            btnGo.Enabled = txtSearch.Text.Length > 0;
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmCooperator", resourceName, null, defaultValue, substitutes);
        }
    }
}
