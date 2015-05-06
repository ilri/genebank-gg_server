using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using GrinGlobal.Admin.PopupForms;
using GrinGlobal.Core;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmSearchEngineIndexField : GrinGlobal.Admin.ChildForms.frmBase {
        public frmSearchEngineIndexField() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public string IndexName;
        public string FieldName;

        public override void RefreshData() {

            if (String.IsNullOrEmpty(IndexName) || DataTable == null) {
                throw new InvalidOperationException(getDisplayMember("RefreshData", "Both the IndexName and DataTable properties must be set before loading frmSearchEngineIndexField form will work properly."));
            }

            var drs = DataTable.Select("index_name = '" + IndexName + "' and field_name = '" + FieldName + "'");

            DataRow dr = null;
            if (drs != null && drs.Length > 0) {
                dr = drs[0];
            }

            if (dr != null) {

                this.Text = "Search Engine Index Field - " + dr["field_name"].ToString() + " - " + AdminProxy.Connection.DatabaseEngineServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

                txtName.Text = dr["field_name"].ToString();
                txtName.ReadOnly = true;

                txtOrdinal.Text = dr["ordinal"].ToString();
                chkPrimaryKey.Checked = dr["is_primary_key"].ToString().ToUpper() == "TRUE";
                chkStoredInIndex.Checked = dr["is_stored_in_index"].ToString().ToUpper() == "TRUE";
                txtForeignKeyTable.Text = dr["foreign_key_table"].ToString();
                txtForeignKeyField.Text = dr["foreign_key_field"].ToString();
                chkSearchable.Checked = dr["is_searchable"].ToString().ToUpper() == "TRUE";
                txtFormat.Text = dr["format"].ToString();
                txtCalculation.Text = dr["calculation"].ToString();
                chkIsBoolean.Checked = dr["is_boolean"].ToString().ToUpper() == "TRUE";
                txtTrueValue.Text = dr["true_value"].ToString();
            } else {

                this.Text = "Search Engine Index Field - (New) - " + AdminProxy.Connection.DatabaseEngineServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

                txtName.Text = "";
                txtOrdinal.Text = "(will be auto-filled)";
                chkPrimaryKey.Checked = false;
                chkStoredInIndex.Checked = false;
                txtForeignKeyTable.Text = "";
                txtForeignKeyField.Text = "";
                chkSearchable.Checked = true;
                txtFormat.Text = "";
                txtCalculation.Text = "";
                chkIsBoolean.Checked = false;
                txtTrueValue.Text = "Y";
            }

            MarkClean();
        }

        private void btnChooseForeignKey_Click(object sender, EventArgs e) {
            var f = new frmTableFieldChooser();
            f.ShowFieldDropDown = true;
            if (DialogResult.OK == MainFormPopupForm(f, this, false)) {
                txtForeignKeyField.Text = f.ddlField.Text;
                txtForeignKeyTable.Text = f.ddlTable.Text;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            CancelAndClose();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void frmSearchEngineIndexField_Load(object sender, EventArgs e) {

        }

        private void chkPrimaryKey_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void syncGUI() {
            txtCalculation.Enabled = txtFormat.Enabled = chkSearchable.Checked;
            txtTrueValue.Enabled = chkIsBoolean.Checked;
            txtForeignKeyField.Enabled = txtForeignKeyTable.Enabled = btnChooseForeignKey.Enabled = !chkPrimaryKey.Checked;

            CheckDirty();
        }

        private void chkSearchable_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void chkStoredInIndex_CheckedChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void chkIsBoolean_CheckedChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtFormat_TextChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void txtCalculation_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtTrueValue_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtForeignKeyField_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtForeignKeyTable_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtName_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void btnCalculationEditor_Click(object sender, EventArgs e) {
            var f = new frmSearchEngineCalculationFormatHelper();
            if (DialogResult.OK == f.ShowDialog(this)) {
                txtFormat.Text = f.Format;
                txtCalculation.Text = f.Calculation;
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmSearchEngineIndexField", resourceName, null, defaultValue, substitutes);
        }
    }
}
