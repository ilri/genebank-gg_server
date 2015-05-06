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
using GrinGlobal.Business.SqlParser;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmChooseJoin : frmBase {
        public frmChooseJoin(MultiJoinException mje) : this() {
            _mje = mje;
        }

        public frmChooseJoin() {
            InitializeComponent(); tellBaseComponents(components);
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public override void RefreshData() {
            if (_mje != null) {
                this.Text = "Select Preferred Relationship for adding table '" + _mje.Table.TableName + "'";
                lblSpecify.Text = "Please specify how table '" + _mje.Table.TableName + "' should be joined into the current query:";
                lvJoins.Items.Clear();
                foreach (var j in _mje.PossibleJoins) {
                    var tbl = j.ToTable == _mje.Table ? j.FromTable : j.ToTable;
                    var lvi = new ListViewItem(tbl.TableName + " (" + tbl.AliasName + ")");
                    lvi.SubItems.Add(j.ToString().Replace("\r\n", " ").Replace("\t", " ").Replace("  ", " "));
                    //var lvi = new ListViewItem(j.FromTable.TableName + " (" + j.FromTable.AliasName + ") ." + j.Conditionals[0].FieldA.DataviewFieldName);
                    //lvi.SubItems.Add(j.Conditionals[0].Operator);
                    //lvi.SubItems.Add(j.ToTable.TableName + " (" + j.ToTable.AliasName + ") ." + j.Conditionals[0].FieldB.DataviewFieldName);
                    lvi.Tag = j;
                    lvJoins.Items.Add(lvi);
                }
                if (lvJoins.Items.Count > 0) {
                    lvJoins.Items[0].Checked = true;
                }
            }
        }

        private MultiJoinException _mje;

        public Join PreferredJoin;

        private void btnOK_Click(object sender, EventArgs e) {
            if (lvJoins.CheckedItems.Count == 0) {
                MessageBox.Show(getDisplayMember("ok{please}", "Please select the preferred relationship."));
            } else {
                PreferredJoin = lvJoins.CheckedItems[0].Tag as Join;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void syncGui() {
            Sync(false, delegate() {
                btnOK.Enabled = lvJoins.CheckedItems.Count == 1;
            });
        }

        mdiParent _parent;
        AdminHostProxy _proxy;

        private void frmGeography_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvJoins);
            syncGui();
        }

        public DialogResult ShowDialog(mdiParent parent, AdminHostProxy proxy) {
            _parent = parent;
            _proxy = proxy;
            return this.ShowDialog(parent);
        }

        private void lvJoins_MouseDoubleClick(object sender, MouseEventArgs e) {
            btnOK.PerformClick();
        }

        private bool _activated;
        private void frmChooseJoin_Activated(object sender, EventArgs e) {
            if (!_activated) {
                _activated = true;

            }
        }

        private void lvJoins_SelectedIndexChanged(object sender, EventArgs e) {
            if (lvJoins.SelectedItems.Count > 0) {
                lvJoins.SelectedItems[0].Checked = true;
            }
        }

        private void lvJoins_ItemChecked(object sender, ItemCheckedEventArgs e) {
            if (e.Item.Checked) {
                for(var i=0;i<lvJoins.Items.Count;i++){
                    if (i != e.Item.Index){
                        lvJoins.Items[i].Checked = false;
                    }
                }
            }
            syncGui();
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmChooseJoin", resourceName, null, defaultValue, substitutes);
        }
    }
}
