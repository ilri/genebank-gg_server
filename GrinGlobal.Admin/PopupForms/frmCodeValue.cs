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
    public partial class frmCodeValue : frmBase {
        public frmCodeValue() {
            InitializeComponent();  tellBaseComponents(components);
            GroupName = "";
            MakeListViewSortable(lvReferencedBy);
        }

        private void initDropDowns() {

        }

        public string GroupName;

        private DataTable _dtLangs;

        private List<int> _rtlRows;

        private int _totalReferenced;
        public int TotalReferenced {
            get {
                return _totalReferenced;
            }
        }

        public override void RefreshData() {

            initDropDowns();

            dgvLanguages.AutoGenerateColumns = false;

            var filled = false;
            _totalReferenced = 0;

            var ds = AdminProxy.ListCodeValues(GroupName, ID, null);

            _dtLangs = ds.Tables["list_code_value_langs"];
            dgvLanguages.DataSource = _dtLangs;
            _rtlRows = new List<int>();
            for (var i = 0; i < _dtLangs.Rows.Count; i++) {
                var drLang = _dtLangs.Rows[i];
                if (drLang["script_direction"].ToString().ToUpper() == "RTL") {
                    _rtlRows.Add(i);
                }
            }

            if (ID > -1) {
                var dt = ds.Tables["list_code_values"];
                if (dt.Rows.Count > 0) {
                    var dr = dt.Rows[0];

                    txtValue.Text = dr["value_member"].ToString();

                    var dtRef = AdminProxy.ListTablesAndDataviewsByCodeGroup(GroupName).Tables["list_tables_and_dataviews_by_code_group"];
                    lvReferencedBy.Items.Clear();
                    foreach (DataRow drR in dtRef.Rows) {
                        var tbl = drR["table_name"].ToString();
                        var fld = drR["field_name"].ToString();
                        if (!String.IsNullOrEmpty(tbl)) {
                            var lvi = new ListViewItem(tbl);
                            lvi.Tag = tbl + "." + fld;
                            lvi.SubItems.Add(fld);

                            // todo: lookup # of rows with that value
                            var count = AdminProxy.GetCodeValueUsageCount(txtValue.Text, tbl, fld);
                            _totalReferenced += count;
                            lvi.SubItems.Add(count.ToString("###,###,##0"));

                            lvReferencedBy.Items.Add(lvi);
                        }
                    }

                    filled = true;

                }
            }

            if (!filled) {
                txtValue.Text = "";
            }

            MarkClean();

            syncGui();

        }

        private void chkPK_CheckedChanged(object sender, EventArgs e) {
            syncGui();
        }

        private bool _syncing;
        private void syncGui() {
            Sync(true, delegate() {
            });
            CheckDirty();
        }

        private void frmTableMappingField_Load(object sender, EventArgs e) {
            syncGui();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (save()) {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private List<string> _touchedTables;
        public List<string> TouchedTables {
            get {
                return _touchedTables;
            }
        }

        private bool save(){

            // if they changed the value itself, show them counts of all records which depend on it
            // and make sure they want to do that

            if (IsControlDirty(txtValue)) {
                if (_totalReferenced > 0) {
                    if (DialogResult.No == MessageBox.Show(this, getDisplayMember("save{noreferences_body}", "{0} rows in the database reference the original value.\r\nAll these rows will be updated to reflect the new value.\r\nDo you wish to continue?", _totalReferenced.ToString("###,###,##0")), 
                        getDisplayMember("save{noreferences_title}", "Update Referencing Rows?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                        return false;
                    }
                }

                var ds = AdminProxy.ListCodeValues(GroupName, null, null);
                var drs = ds.Tables["list_code_values"].Select("value_member = '" + txtValue.Text.Replace("'", "''") + "'");

                // first make sure the group/value doesn't already exist...
                if (drs.Length > 0) {
                    MessageBox.Show(this, getDisplayMember("save{groupexists_body}", "The value '{0}' already exists in group '{1}'.\r\nPlease specify a different value.", txtValue.Text , GroupName), 
                        getDisplayMember("save{groupexists_title}", "Value Already Exists"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtValue.Focus();
                    txtValue.SelectAll();
                    return false;
                }

            }

            if (_touchedTables == null) {
                _touchedTables = new List<string>();
            }

            ID = AdminProxy.SaveCodeValue(ID,
                GroupName,
                txtValue.Text,
                _dtLangs, _touchedTables);

            if (Modal && Owner != null) {
                var f = (frmCodeGroup)Owner;
                f.RefreshData();
            }
            MainFormUpdateStatus(getDisplayMember("save{done}", "Saved code value {0}", txtValue.Text), true);
            return true;

        }

        private void txtValue_TextChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            if (IsDirty()) {
                var dr = MessageBox.Show(this, getDisplayMember("cancel{body}", "Your changes will be permanently lost.\nDo you want to cancel anyway?"), 
                    getDisplayMember("cancel{title}", "Cancel Changes?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
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

        private void btnNext_Click(object sender, EventArgs e) {
            if (IsDirty()) {
                if (chkAutoSave.Checked) {
                    if (save()) {
                        if (Owner != null) {
                            var f2 = (frmCodeGroup)Owner;
                            f2.RefreshData();
                        }
                    } else {
                        return;
                    }
                } else {
                    var dr = MessageBox.Show(this, getDisplayMember("next{save_body}", "Do you want to save your changes?"), 
                        getDisplayMember("next{save_title}", "Save Changes?"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (dr) {
                        case DialogResult.Yes:
                            if (!save()) {
                                return;
                            }
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                        default:
                            return;
                    }
                }
            }


            var f = (frmCodeGroup)this.Owner;
            var newID = f.GetNextValueID(true);
            if (newID > -1) {
                ID = newID;
                RefreshData();
            }

        }

        private void btnPrevious_Click(object sender, EventArgs e) {
            if (IsDirty()) {
                if (chkAutoSave.Checked) {
                    if (save()){
                        if (Owner != null) {
                            var f2 = (frmCodeGroup)Owner;
                            f2.RefreshData();
                        }
                    } else {
                        return;
                    }
                } else {
                    var dr = MessageBox.Show(this,  getDisplayMember("previous{body}", "Do you want to save your changes?"), 
                        getDisplayMember("previous{title}", "Save Changes?"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (dr) {
                        case DialogResult.Yes:
                            if (!save()){
                                return;
                            }
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                        default:
                            return;
                    }
                }
            }


            var f = (frmCodeGroup)this.Owner;
            var newID = f.GetPreviousValueID(true);
            if (newID > -1) {
                ID = newID;
                RefreshData();
            }

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
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmCodeValue", resourceName, null, defaultValue, substitutes);
        }
    }
}
