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
    public partial class frmGeography : frmBase {
        public frmGeography() {
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
            if (lvGeographies.SelectedItems.Count == 0) {
                MessageBox.Show(getDisplayMember("ok{geography}", "Please select a geography."));
                return;
            }
            var it = lvGeographies.SelectedItems[0];
            GeographyID = Toolkit.ToInt32(it.Tag, -1);
            GeographyText = it.SubItems[0].Text + ", " + it.SubItems[1].Text + ", " + it.SubItems[2].Text + ", " + it.SubItems[3].Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        public int GeographyID;
        public string GeographyText { get; private set; }

        private void btnGo_Click(object sender, EventArgs e) {
            var ds = _proxy.SearchGeographies(txtSearch.Text);
            var dt = ds.Tables["search_geographies"];
            lvGeographies.Items.Clear();
            foreach (DataRow dr in dt.Rows) {
                var lvi = new ListViewItem();
                lvi.Tag = dr["geography_id"];
                lvi.Text = dr["adm1"].ToString();
                lvi.SubItems.Add(dr["country_code"].ToString());
                lvi.SubItems.Add(dr["country_name"].ToString());
                lvi.SubItems.Add(dr["subcontinent"].ToString());
                lvi.SubItems.Add(dr["continent"].ToString());
                lvGeographies.Items.Add(lvi);
            }
            lblCount.Text = "Found " + lvGeographies.Items.Count + " item(s)";
            syncGui();
        }

        private void syncGui() {
            Sync(false, delegate() {
                btnGo.Enabled = txtSearch.Text.Length > 0;
                btnOK.Enabled = lvGeographies.SelectedItems.Count == 1;
            });
        }

        mdiParent _parent;
        AdminHostProxy _proxy;

        private void frmGeography_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvGeographies);
            syncGui();
        }

        public DialogResult ShowDialog(mdiParent parent, AdminHostProxy proxy) {
            _parent = parent;
            _proxy = proxy;
            return this.ShowDialog(parent);
        }

        private void lvGeographys_MouseDoubleClick(object sender, MouseEventArgs e) {
            btnOK.PerformClick();
        }

        private bool _activated;
        private void frmGeography_Activated(object sender, EventArgs e) {
            if (!_activated) {
                _activated = true;
                txtSearch.Focus();

            }
        }

        private void lvGeographies_SelectedIndexChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) {
            syncGui();
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmGeography", resourceName, null, defaultValue, substitutes);
        }
    }
}
