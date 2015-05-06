using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Admin.ChildForms;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmSearchEngineCalculationFormatHelper : GrinGlobal.Admin.ChildForms.frmBase {
        public frmSearchEngineCalculationFormatHelper() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public override void RefreshData() {

            lbFields.Items.Clear();
            
            if (DataTable != null && DataTable.Rows.Count > 0) {
                var list = new List<string>();
                var drs = DataTable.Select("index_name = '" + IndexName + "'");
                foreach (DataRow dr in drs) {
                    list.Add(dr["field_name"].ToString());
                }
                lbFields.Items.AddRange(list.ToArray());
            } else {
            }
            MarkClean();

        }

        public string IndexName;

        public string Calculation;
        public string Format { get; private set; }

        private void btnAdd_Click(object sender, EventArgs e) {
            Format = txtFormat.Text;
            if (rdoSqlStatementCalculation.Checked) {
                Calculation = txtCalculationSql.Text;
            } else {
                Calculation = txtCalculationFreeForm.Text;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void rdoSortableDate_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void syncGUI() {
            if (rdoFreeFormFormat.Checked) {
                txtFormat.Enabled = true;
            } else {
                txtFormat.Enabled = false;
                if (rdoSortableDate.Checked) {
                    txtFormat.Text = "yyyy/MM/dd";
                }
            }

            if (rdoFieldCalculation.Checked) {
                txtCalculationSql.Visible = false;
                txtCalculationFreeForm.Visible = true;
                lbFields.Visible = true;
            } else if (rdoSqlStatementCalculation.Checked) {
                txtCalculationSql.Visible = true;
                txtCalculationFreeForm.Visible = false;
                if (!txtCalculationSql.Text.ToLower().StartsWith("sql:")) {
                    txtCalculationSql.Text = "sql:" + txtCalculationSql.Text;
                }
            } else {
                txtCalculationSql.Visible = false;
                txtCalculationFreeForm.Visible = true;
                lbFields.Visible = false;
            }
            CheckDirty();
        }

        private void frmSearchEngineCalculationFormatHelper_Load(object sender, EventArgs e) {
            txtFormat.Text = Format;
            txtCalculationSql.Left = txtCalculationFreeForm.Left;
        }

        private void rdoSqlStatementCalculation_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void rdoCalcFreeForm_CheckedChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void rdoFieldCalculation_CheckedChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtFormat_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void lbFields_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void txtCalculationFreeForm_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }
    }
}
