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
    public partial class frmWebCooperator : frmBase {
        public frmWebCooperator() {
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
            if (lvWebLogins.SelectedItems.Count == 0) {
                MessageBox.Show(getDisplayMember("ok{nowebcooperator}", "Please select a web login."));
                return;
            }
            WebCooperatorID = Toolkit.ToInt32(lvWebLogins.SelectedItems[0].Tag, -1);
            DialogResult = DialogResult.OK;
            Close();
        }

        public int WebCooperatorID;

        private void btnGo_Click(object sender, EventArgs e) {
            using (new AutoCursor(this)) {
                var ds = _proxy.SearchWebCooperators(txtSearch.Text);
                var dt = ds.Tables["search_web_cooperators"];
                lvWebLogins.Items.Clear();
                foreach (DataRow dr in dt.Rows) {
                    var lvi = new ListViewItem();
                    lvi.Tag = dr["web_cooperator_id"];
                    lvi.Text = dr["user_name"].ToString();
                    lvi.SubItems.Add(dr["last_name"].ToString());
                    lvi.SubItems.Add(dr["first_name"].ToString());
                    lvi.SubItems.Add(dr["organization"].ToString());
                    lvWebLogins.Items.Add(lvi);
                }
                lblCount.Text = "Found " + lvWebLogins.Items.Count + " web login(s)";
            }
        }

        mdiParent _parent;
        AdminHostProxy _proxy;

        private void frmCooperator_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvWebLogins);
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
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmWebCooperator", resourceName, null, defaultValue, substitutes);
        }
    }
}
