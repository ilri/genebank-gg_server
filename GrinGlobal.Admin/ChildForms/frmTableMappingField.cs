using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using GrinGlobal.Admin.PopupForms;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmTableMappingField : frmBase {
        public frmTableMappingField() {
            InitializeComponent();  tellBaseComponents(components);
            TableID = -1;
        }

        private void initDropDowns() {
            initDataViewDropDown(ddlLookupPicker, true, "(None)", -1, null, "Lookup Table");
            //            ddlDataView.SelectedIndex = 0;

            initCodeGroupDropDown(ddlCodeGroup, "(None)");

        }

        public int TableID;

        private DataTable _dtLangs;

        private List<int> _rtlRows;

        public override void RefreshData() {

            initDropDowns();
            // this form has multiple uses, as does the underlying ListTableMappings method.
            // it can:
            // List all TableMappings (0,[0])
            // List permission info for a single permission (37,[0])
            // List TableMappings that are NOT in a list (0, [2,3,4,5,6])

            dgvLanguages.AutoGenerateColumns = false;

            var filled = false;
            if (ID > -1) {
                var ds = AdminProxy.ListTableFields(TableID, ID, false);
                var dt = ds.Tables["list_table_fields"];
                if (dt.Rows.Count > 0) {
                    var dr = dt.Rows[0];
                    TableID = Toolkit.ToInt32(dr["sys_table_id"], -1);
                    txtFieldName.Text = dr["field_name"].ToString();
                    _previousDefaultValue = txtDefaultValue.Text = dr["default_value"].ToString();
                    if (txtDefaultValue.Text == "{DBNull.Value}") {
                        chkDefaultIsNull.Checked = true;
                    }
                    chkAutoIncrement.Checked = dr["is_autoincrement"].ToString().ToUpper() == "Y";
                    chkForeignKey.Checked = dr["is_foreign_key"].ToString().ToUpper() == "Y";
                    // stored as nullable, displaying required -- so it's just the opposite
                    chkRequired.Checked = !(dr["is_nullable"].ToString().ToUpper() == "Y");
                    chkPK.Checked = dr["is_primary_key"].ToString().ToUpper() == "Y";
                    chkReadOnly.Checked = dr["is_readonly"].ToString().ToUpper() == "Y";

                    txtMinLength.Text = dr["min_length"].ToString();
                    _previousMaxLength = txtMaxLength.Text = dr["max_length"].ToString();
                    if (txtMaxLength.Text == "32767") {
                        chkUnlimitedLength.Checked = true;
                    }
                    txtPrecision.Text = dr["numeric_precision"].ToString();
                    txtScale.Text = dr["numeric_scale"].ToString();

                    _foreignKeyTableFieldID = Toolkit.ToInt32(dr["foreign_key_table_field_id"], -1);
                    lblForeignKeyField.Text = dr["foreign_key_table_field_name"].ToString();
                    ddlLookupPicker.SelectedIndex = ddlLookupPicker.FindString(dr["foreign_key_dataview_name"].ToString());
                    ddlGuiHint.SelectedIndex = ddlGuiHint.FindString(convertFieldGuiHint(dr["gui_hint"].ToString()));
                    ddlCodeGroup.SelectedIndex = ddlCodeGroup.FindString(dr["group_name"].ToString());
                    syncPurpose(dr["field_purpose"].ToString());
                    syncType(dr["field_type"].ToString());
                    syncGuiHint(dr["gui_hint"].ToString());

                    var f = ((frmTableMapping)this.Owner);
                    if (f.GetPreviousFieldID(false) > -1) {
                        btnPrevious.Enabled = true;
                    } else {
                        btnPrevious.Enabled = false;
                    }

                    if (f.GetNextFieldID(false) > -1) {
                        btnNext.Enabled = true;
                    } else {
                        btnNext.Enabled = false;
                    }

                    _dtLangs = ds.Tables["list_table_field_langs"];
                    dgvLanguages.DataSource = _dtLangs;
                    _rtlRows = new List<int>();
                    for (var i = 0; i < _dtLangs.Rows.Count; i++) {
                        var drLang = _dtLangs.Rows[i];
                        if (drLang["script_direction"].ToString().ToUpper() == "RTL") {
                            _rtlRows.Add(i);
                        }
                    }

                    filled = true;

                    MarkClean();
                }
            }

            if (!filled) {
                txtFieldName.Text = "";
                txtDefaultValue.Text = "";
                chkAutoIncrement.Checked = false;
                chkForeignKey.Checked = false;
                chkRequired.Checked = true;
                chkPK.Checked = false;
                chkReadOnly.Checked = false;

                ddlCodeGroup.SelectedIndex = ddlCodeGroup.FindString("(None)");
                syncPurpose("Editable Data");
                syncType("Text");
                syncGuiHint("Textbox (free-form)");

            }

        }

        private void syncPurpose(string text) {
            ddlPurpose.SelectedIndex = ddlPurpose.FindString(convertFieldPurpose(text));
        }

        private void syncType(string text) {
            ddlType.SelectedIndex = ddlType.FindString(convertFieldType(text));
        }

        private void syncGuiHint(string text) {
            ddlGuiHint.SelectedIndex = ddlGuiHint.FindString(convertFieldGuiHint(text));
        }

        private void chkPK_CheckedChanged(object sender, EventArgs e) {
            syncGui();
        }

        private bool _syncing;
        private void syncGui() {
            if (!_syncing) {
                try {
                    _syncing = true;
                    if (chkPK.Checked) {
                        ddlPurpose.SelectedIndex = ddlPurpose.FindString("Primary Key");
                        if (String.IsNullOrEmpty(ddlGuiHint.Text)) {
                            ddlGuiHint.SelectedIndex = ddlGuiHint.FindString("Lookup Picker");
                        }
                        if (String.IsNullOrEmpty(ddlType.Text)) {
                            ddlType.SelectedIndex = ddlType.FindString("Integer");
                        }
                        ddlPurpose.Enabled = false;
                        chkRequired.Enabled = false;
                        chkRequired.Checked = true;
                    } else {
                        ddlPurpose.Enabled = true;
                        chkRequired.Enabled = true;
                    }

                    if (chkAutoIncrement.Checked) {
                        chkReadOnly.Checked = true;
                        chkReadOnly.Enabled = false;
                    } else {
                        chkReadOnly.Enabled = true;
                    }

                    if (chkForeignKey.Checked) {
                        gbForeignKey.Enabled = true;
                    } else {
                        gbForeignKey.Enabled = false;
                    }

                    var purpose = ddlPurpose.Text.ToLower();
                    if (ddlPurpose.Enabled) {
                        if (purpose.Contains("date")) {
                            ddlType.SelectedIndex = ddlType.FindString("Date / Time");
                        } else if (purpose.Contains("by")) {
                            ddlType.SelectedIndex = ddlType.FindString("Integer");
                        }
                    }

                    txtPrecision.Top = txtMinLength.Top;
                    txtScale.Top = txtMaxLength.Top;

                    var type = ddlType.Text.ToLower();
                    if (type.Contains("decimal")) {
                        lblMaxOrScale.Visible = true;
                        lblMinOrPrecision.Visible = true;
                        txtMaxLength.Visible = false;
                        txtMinLength.Visible = false;
                        chkUnlimitedLength.Visible = false;
                        txtScale.Visible = true;
                        txtPrecision.Visible = true;

                        lblMaxOrScale.Text = "Scale:";
                        lblMinOrPrecision.Text = "Precision:";
                        //ddlGuiHint.SelectedIndex = ddlGuiHint.FindString("Textbox (decimals");
                    } else if (type.Contains("text")) {
                        lblMaxOrScale.Visible = true;
                        lblMinOrPrecision.Visible = true;
                        txtMaxLength.Visible = true;
                        txtMinLength.Visible = true;
                        chkUnlimitedLength.Visible = true;

                        txtMaxLength.Enabled = true;
                        if (chkUnlimitedLength.Checked) {
                            if (txtMaxLength.Text != "32767") {
                                _previousMaxLength = txtMaxLength.Text;
                            }
                            txtMaxLength.Text = "32767";
                            txtMaxLength.Enabled = false;
                        } else {
                            if (!String.IsNullOrEmpty(_previousMaxLength)) {
                                txtMaxLength.Text = _previousMaxLength;
                            }
                            txtMaxLength.Enabled = true;
                        }


                        txtScale.Visible = false;
                        txtPrecision.Visible = false;
                        lblMaxOrScale.Text = "Max Length:";
                        lblMinOrPrecision.Text = "Min Length:";
                        //ddlGuiHint.SelectedIndex = ddlGuiHint.FindString("Textbox (free-form)");
                    } else {

                        if (type.Contains("date")) {
                            //ddlGuiHint.SelectedIndex = ddlGuiHint.FindString("Date / Time");
                        } else if (type.Contains("integer")) {
                            //var guiHintText = ddlGuiHint.Text.ToLower();
                            //if (guiHint.Contains("checkbox") || guiHint.Contains("date") || guiHint.Contains("free-form") || String.IsNullOrEmpty(guiHint)) {
                            //    //ddlGuiHint.SelectedIndex = ddlGuiHint.FindString("Lookup Picker");
                            //}
                        }

                        lblMaxOrScale.Visible = false;
                        lblMinOrPrecision.Visible = false;
                        txtMaxLength.Visible = false;
                        txtMinLength.Visible = false;
                        chkUnlimitedLength.Visible = false;
                        txtScale.Visible = false;
                        txtPrecision.Visible = false;
                    }

                    if (!chkRequired.Checked) {
                        //chkDefaultIsNull.Enabled = true;
                        chkDefaultIsNull.Text = "Empty Default Value";
                    } else {
                        //chkDefaultIsNull.Enabled = false;
                        //chkDefaultIsNull.Checked = false;
                        chkDefaultIsNull.Text = "Empty (prevent saving unspecified value)";
                        txtDefaultValue.Text = _previousDefaultValue + "";
                        // txtDefaultValue.Enabled = true;
                    }

                    if (chkDefaultIsNull.Checked) {
                        if (txtDefaultValue.Text != "{DBNull.Value}") {
                            _previousDefaultValue = txtDefaultValue.Text;
                        }
                        txtDefaultValue.Text = "{DBNull.Value}";
                        txtDefaultValue.Enabled = false;
                    } else {
                        txtDefaultValue.Text = _previousDefaultValue + "";
                        txtDefaultValue.Enabled = true;
                    }


                    ddlLookupPicker.Left = ddlCodeGroup.Left;
                    ddlLookupPicker.Top = ddlCodeGroup.Top;

                    lblLookupPicker.Top = lblCodeGroup.Top;
                    lblLookupPicker.Left = lblCodeGroup.Left;

                    var guiHint = convertFieldGuiHint(ddlGuiHint.Text);
                    switch (guiHint) {
                        case "SMALL_SINGLE_SELECT_CONTROL":
                            ddlCodeGroup.Visible = true;
                            lblCodeGroup.Visible = true;
                            ddlLookupPicker.Visible = false;
                            lblLookupPicker.Visible = false;
                            break;
                        case "LARGE_SINGLE_SELECT_CONTROL":
                            ddlCodeGroup.Visible = false;
                            lblCodeGroup.Visible = false;
                            ddlLookupPicker.Visible = true;
                            lblLookupPicker.Visible = true;
                            break;
                        default:
                            ddlCodeGroup.Visible = false;
                            lblCodeGroup.Visible = false;
                            ddlLookupPicker.Visible = false;
                            lblLookupPicker.Visible = false;
                            break;
                    }


                } finally {
                    _syncing = false;
                }
            }
            CheckDirty();
        }
        private string _previousDefaultValue;
        private string _previousMaxLength;

        private void ddlPurpose_SelectedIndexChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void ddlType_SelectedIndexChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void chkForeignKey_CheckedChanged(object sender, EventArgs e) {
            syncGui();
        }

        private int _foreignKeyTableFieldID;
        private void btnForeignKeyField_Click(object sender, EventArgs e) {
            var f = new frmTableFieldChooser { ID = _foreignKeyTableFieldID, ShowFieldDropDown = true };
            if (MainFormPopupForm(f, this, false) == DialogResult.OK) {
                _foreignKeyTableFieldID = f.ID;
                lblForeignKeyField.Text = f.ddlTable.Text + "." + f.ddlField.Text;
                MarkDirty();
                syncGui();
            }
        }

        private void frmTableMappingField_Load(object sender, EventArgs e) {
            syncGui();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void save(){

            string codeGroup = null;
            if (ddlCodeGroup.SelectedIndex > 0){
                codeGroup = ddlCodeGroup.Text;
            }

            ID = AdminProxy.SaveTableFieldMapping(Toolkit.ToInt32(ID, -1),
                TableID,
                txtFieldName.Text,
                convertFieldPurpose(ddlPurpose.Text),
                convertFieldType(ddlType.Text),
                txtDefaultValue.Text,
                chkPK.Checked,
                chkForeignKey.Checked,
                _foreignKeyTableFieldID,
                (ddlLookupPicker.Text == "(None)" ? null: ddlLookupPicker.Text),

                // stored as nullable, displaying required -- so it's just the opposite
                !chkRequired.Checked,

                convertFieldGuiHint(ddlGuiHint.Text),
                chkReadOnly.Checked,
                (ddlType.Text == "Text" ? Toolkit.ToInt32(txtMinLength.Text, 0) : 0),
                (ddlType.Text == "Text" ? Toolkit.ToInt32(txtMaxLength.Text, 0) : 0),
                (ddlType.Text == "Integer" ? Toolkit.ToInt32(txtPrecision.Text, 0) : 0),
                (ddlType.Text == "Integer" ? Toolkit.ToInt32(txtScale.Text, 0) : 0),
                chkAutoIncrement.Checked,
                codeGroup, _dtLangs);
            MainFormUpdateStatus(getDisplayMember("save{done}", "Saved table field mapping {0}", txtFieldName.Text), true);

        }

        private void chkAutoIncrement_CheckedChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void txtMaxOrScale_TextChanged(object sender, EventArgs e) {
            var max = Toolkit.ToInt32(txtMaxLength.Text, 100);
            if (txtDefaultValue.Text.Length > max) {
                txtDefaultValue.Text = Toolkit.Cut(txtDefaultValue.Text, 0, max);
            }
            if (max < 0) {
                max = 32767;
            }
            txtDefaultValue.MaxLength = max;
            CheckDirty();

        }

        private void txtFieldName_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void label7_Click(object sender, EventArgs e) {

        }

        private void chkNullable_CheckedChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void chkReadOnly_CheckedChanged(object sender, EventArgs e) {
            syncGui();

        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void label3_Click(object sender, EventArgs e) {

        }

        private void txtDefaultValue_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlGuiHint_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();
            syncGui();
        }

        private void label4_Click(object sender, EventArgs e) {

        }

        private void label5_Click(object sender, EventArgs e) {

        }

        private void ddlCodeGroup_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void btnCancel_Click(object sender, EventArgs e) {
            if (IsDirty()) {
                var dr = MessageBox.Show(this, getDisplayMember("cancel{start_body}", "Your changes will be permanently lost.\nDo you want to cancel anyway?"),
                    getDisplayMember("cancel{start_title}", "Cancel Changes?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                switch (dr) {
                    case DialogResult.Yes:
                        // keep going...
                        break;
                    case DialogResult.No:
                    default:
                        return;
                }
            }

            DialogResult = DialogResult.Cancel;
            Close();

        }

        private void chkDefaultIsNull_CheckedChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void chkUnlimitedLength_CheckedChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void btnNext_Click(object sender, EventArgs e) {
            if (IsDirty()) {
                if (chkAutoSave.Checked) {
                    save();
                } else {
                    var dr = MessageBox.Show(this, getDisplayMember("next{start_body}", "Do you want to save your changes?"),
                        getDisplayMember("next{start_title}", "Save Changes?"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (dr) {
                        case DialogResult.Yes:
                            save();
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                        default:
                            return;
                    }
                }
            }


            var f = (frmTableMapping)this.Owner;
            ID = f.GetNextFieldID(true);
            RefreshData();

        }

        private void btnPrevious_Click(object sender, EventArgs e) {
            if (IsDirty()) {
                if (chkAutoSave.Checked) {
                    save();
                } else {
                    var dr = MessageBox.Show(this, getDisplayMember("previous{start_body}", "Do you want to save your changes?"),
                        getDisplayMember("previous{start_title}", "Save Changes?"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (dr) {
                        case DialogResult.Yes:
                            save();
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                        default:
                            return;
                    }
                }
            }


            var f = (frmTableMapping)this.Owner;
            ID = f.GetPreviousFieldID(true);
            RefreshData();

        }

        private void lblForeignKeyField_Click(object sender, EventArgs e) {

        }

        private void ddlForeignKeyDataview_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtPrecision_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtScale_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtMinLength_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void dgvLanguages_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            CheckDirty();
        }

        private void dgvLanguages_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
        }

        private void dgvLanguages_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            if (e.ColumnIndex > -1 && e.RowIndex > -1) {
                if (_rtlRows.Contains(e.RowIndex)){
                    e.PaintBackground(e.CellBounds, true);
                    TextRenderer.DrawText(e.Graphics, e.FormattedValue.ToString(), e.CellStyle.Font, e.CellBounds, e.CellStyle.ForeColor, TextFormatFlags.RightToLeft | TextFormatFlags.Right);
                    e.Handled = true;
                }
            }

        }

        private void dgvLanguages_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
            if (dgvLanguages.CurrentCell.ColumnIndex > -1 && dgvLanguages.CurrentCell.RowIndex > -1) {
                if (e.Control != null) {
                    if (_rtlRows.Contains(dgvLanguages.CurrentCell.RowIndex)){
                        e.Control.RightToLeft = RightToLeft.Yes;
                    }
                }
            }
        }

        private const char RLE_CHAR = (char)0x202B;// RLE embedding Unicode control character

        private void dgvLanguages_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e) {
            if (e.ColumnIndex > -1 && e.RowIndex > -1) {
                if (_rtlRows.Contains(e.RowIndex)){
                    if (e.ToolTipText != string.Empty) {
                        e.ToolTipText = RLE_CHAR + e.ToolTipText;
                    }
                }
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmTableMappingField", resourceName, null, defaultValue, substitutes);
        }
    }
}
