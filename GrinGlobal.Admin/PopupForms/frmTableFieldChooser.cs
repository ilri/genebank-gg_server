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
    public partial class frmTableFieldChooser : frmBase {
        public frmTableFieldChooser() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public override void RefreshData() {
            initDropDowns();
            MarkClean();
        }

        private void btnOK_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            if (ShowFieldDropDown){
                ID = Toolkit.ToInt32(ddlField.SelectedValue, -1);
            } else {
                ID = Toolkit.ToInt32(ddlTable.SelectedValue, -1);
            }
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public bool ShowFieldDropDown;


        private void frmChooseTableField_Load(object sender, EventArgs e) {
            if (!ShowFieldDropDown) {
                lblField.Visible = false;
                ddlField.Visible = false;
                this.Text = "Choose Table";
            }

        }

        private void initDropDowns() {
            if (ShowFieldDropDown) {

                // ID is table id, look it up from the database
                var defaultTableText = "(None)";
                var defaultFieldText = "(None)";
                if (ID > -1) {
                    var dt = AdminProxy.ListTableFields(-1, ID, false).Tables["list_table_fields"];
                    if (dt.Rows.Count > 0) {
                        defaultTableText = dt.Rows[0]["table_name"].ToString();
                        defaultFieldText = dt.Rows[0]["field_name"].ToString();
                    }
                }

                initTableDropDown(ddlTable, true, "(None)", -1);
                ddlTable.SelectedIndex = ddlTable.FindString(defaultTableText);
                initTableFieldDropDown(ddlField, Toolkit.ToInt32(ddlTable.SelectedValue, -1), true, "(None)");
                ddlField.SelectedIndex = ddlField.FindString(defaultFieldText);
            } else {

                // ID is table id, look it up from the database
                var defaultTableText = "(None)";
                if (ID > -1) {
                    var dt = AdminProxy.ListTables(ID).Tables["list_tables"];
                    if (dt.Rows.Count > 0) {
                        defaultTableText = dt.Rows[0]["table_name"].ToString();
                    }
                }

                initTableDropDown(ddlTable, true, defaultTableText, -1);
                ddlTable.SelectedIndex = ddlTable.FindString(defaultTableText);

            }
        }

        private void ddlTable_SelectedIndexChanged(object sender, EventArgs e) {
            if (ShowFieldDropDown) {
                // refill field dropdown
                initTableFieldDropDown(ddlField, Toolkit.ToInt32(ddlTable.SelectedValue, -1), true, "(None)");
            }
            CheckDirty();

        }

        private void ddlField_SelectedIndexChanged(object sender, EventArgs e) {
            syncGui();
        }

        private void syncGui() {
            btnOK.Enabled = true;
            if (ddlTable.Text == "(None)") {
                btnOK.Enabled = false;
            }
            if (ShowFieldDropDown && ddlField.Text == "(None)") {
                btnOK.Enabled = false;
            }
            CheckDirty();
        }
    }
}
