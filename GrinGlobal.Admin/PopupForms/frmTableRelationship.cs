using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Admin.ChildForms;
using GrinGlobal.Core;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmTableRelationship : frmBase {
        public frmTableRelationship() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public override void RefreshData() {
            initDropDowns();
            MarkClean();
        }

        private void btnOK_Click(object sender, EventArgs e) {

            var relType = "PARENT";

            if (chkOwner.Checked) {
                relType = "OWNER_PARENT";
            }

            if (String.Compare(txtChildTableName.Text, ddlParentTable.Text, true) == 0) {
                relType = "SELF";
            }

            AdminProxy.SaveTableRelationship(ID, 
                Toolkit.ToInt32(ddlChildField.SelectedValue, -1),
                relType,
                Toolkit.ToInt32(ddlParentField.SelectedValue, -1));

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public string FromTableName;
        public int FromTableID;

        private void frmChooseTableField_Load(object sender, EventArgs e) {

        }

        private void initDropDowns() {
            // ID is table id, look it up from the database
            var defaultFromFieldText = "(None)";
            var defaultRelationshipType = "PARENT";
            var defaultToTableText = "(None)";
            var defaultToFieldText = "(None)";

            if (ID > -1) {
                var ds = AdminProxy.ListTableRelationships(FromTableID, ID);
                var dt = ds.Tables["list_table_relationships"];
                if (dt.Rows.Count > 0) {
                    var dr = dt.Rows[0];
                    defaultFromFieldText = dr["field_name"].ToString();
                    defaultRelationshipType = dr["relationship_type_tag"].ToString();
                    defaultToTableText = dr["table_name2"].ToString();
                    defaultToFieldText = dr["field_name2"].ToString();
                }
            }

            // init From
            txtChildTableName.Text = FromTableName;
            initTableFieldDropDown(ddlChildField, FromTableID, true, "(None)", false, chkChildOnlyForeignKeys.Checked);
            ddlChildField.SelectedIndex = ddlChildField.FindString(defaultFromFieldText);

            // init Relationship
            if (defaultRelationshipType.ToUpper().Contains("OWNER")) {
                chkOwner.Checked = true;
            }

            // init To
            initTableDropDown(ddlParentTable, true, "(None)", -1);
            ddlParentTable.SelectedIndex = ddlParentTable.FindString(defaultToTableText);
            initTableFieldDropDown(ddlParentField, Toolkit.ToInt32(ddlParentTable.SelectedValue, -1), true, "(None)", chkParentOnlyPrimaryKeys.Checked, false);
            ddlParentField.SelectedIndex = ddlParentField.FindString(defaultToFieldText);

            syncGui();
        }

        private void ddlField_SelectedIndexChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void syncGui() {
            CheckDirty();
            if (ddlChildField.Text == "(None)"
                || ddlParentTable.Text == "(None)"
                || ddlParentField.Text == "(None)"){
                btnSave.Enabled = false;
            }
        }

        private void ddlToTable_SelectedIndexChanged(object sender, EventArgs e) {
            initTableFieldDropDown(ddlParentField, Toolkit.ToInt32(ddlParentTable.SelectedValue, -1), true, "(None)", chkParentOnlyPrimaryKeys.Checked, false);
            syncGui();
        }

        private void ddlToField_SelectedIndexChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void ddlRelationshipType_SelectedIndexChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void chkFromOnlyKeys_CheckedChanged(object sender, EventArgs e) {
            var val = (int)ddlChildField.SelectedValue;
            initTableFieldDropDown(ddlChildField, FromTableID, true, "(None)", false, chkChildOnlyForeignKeys.Checked);
            selectItemByValue(ddlChildField, val);
            syncGui();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            var val = (int)ddlParentField.SelectedValue;
            initTableFieldDropDown(ddlParentField, Toolkit.ToInt32(ddlParentTable.SelectedValue, -1), true, "(None)", chkParentOnlyPrimaryKeys.Checked, false);
            selectItemByValue(ddlParentField, val);
            syncGui();
        }

        private void chkOwner_CheckedChanged(object sender, EventArgs e) {
            CheckDirty();

        }
    }
}
