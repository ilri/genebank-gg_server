using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Interface.Dataviews;
using System.Diagnostics;
using GrinGlobal.Admin.ChildForms;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmDataviewParameters : frmBase {
        public frmDataviewParameters() {
            InitializeComponent();  tellBaseComponents(components);
            Parameters = new List<IDataviewParameter>();
        }

        public List<IDataviewParameter> Parameters;

        private void btnOK_Click(object sender, EventArgs e) {

            for(var i=0;i<dgvParameters.Rows.Count;i++){
                var row = dgvParameters.Rows[i];
                Parameters[i].Value = row.Cells[2].Value;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void frmDataviewParameters_Load(object sender, EventArgs e) {
            dgvParameters.Rows.Clear();
            foreach (var p in Parameters) {
                var values = new object[] { p.Name, p.TypeName, p.Value };
                dgvParameters.Rows.Add(values);
            }
        }

        private void dgvParameters_CellEndEdit(object sender, DataGridViewCellEventArgs e) {

        }

        private void dgvParameters_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter && dgvParameters.CurrentCell.RowIndex == dgvParameters.Rows.Count - 1) {
                btnOK.PerformClick();
            }
        }

        private void dgvParameters_KeyDown(object sender, KeyEventArgs e) {
        }

        public void ResetActivated() {
            _activated = false;
        }
        private bool _activated;
        private void frmDataviewParameters_Activated(object sender, EventArgs e) {
            Debug.WriteLine("activated form=" + _activated);
            if (!_activated) {
                _activated = true;
                if (dgvParameters.Rows.Count > 0 && dgvParameters.Rows[0].Cells.Count > 2) {
                    dgvParameters.CurrentCell = dgvParameters.Rows[0].Cells[2];
                    dgvParameters.BeginEdit(true);
                }
            }
        }

        private void dgvParameters_EditModeChanged(object sender, EventArgs e) {
        }

        private void dgvParameters_CellEnter(object sender, DataGridViewCellEventArgs e) {
            dgvParameters.BeginEdit(true);
        }
    }
}
