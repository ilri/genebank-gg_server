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

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmPermissionField : frmBase {
        public frmPermissionField() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public int DataViewID;
        public int TableID;
//        public int ParentTableFieldID;

        protected int ResourceID {
            get {
                if (DataViewID > -1) {
                    return DataViewID;
                } else if (TableID > -1){
                    return TableID;
                } else {
                    return -1;
                }
            }
        }
        protected string ResourceType {
            get {
                if (DataViewID > -1) {
                    return "dataview";
                } else if (TableID > -1) {
                    return "table";
                } else {
                    return null;
                }
            }
        }
        public int PermissionID;
        public override void RefreshData() {
            this.Text = (ID < 0) ? "New Restriction" : "Restriction for " + MainFormCurrentNodeText("Permission");

            Sync(true, delegate() {
                var dt = AdminProxy.ListPermissionFields(PermissionID, (ID < 0 ? int.MaxValue : ID)).Tables["list_permission_fields"];

                if (dt.Rows.Count > 0) {
                    var dr = dt.Rows[0];

                    PermissionID = Toolkit.ToInt32(dr["sys_permission_id"], -1);

                    int dvID = Toolkit.ToInt32(dr["sys_dataview_id"], -1);
                    int tblID = Toolkit.ToInt32(dr["sys_table_id"], -1);

                    // if neither is specified, leave them alone so the user can change to them if need be
                    if (dvID != -1 || tblID != -1) {
                        DataViewID = dvID;
                        TableID = tblID;
                    }

                    int parentTableID = Toolkit.ToInt32(dr["parent_table_id"].ToString(), -1);
                    //ParentTableFieldID = Toolkit.ToInt32(dr["parent_table_field_id"].ToString(), -1);

                    string compareMode = dr["compare_mode"].ToString();

                    if (compareMode == "parent") {
                        rdoChildValue.Checked = true;
                    } else {
                        rdoFieldValue.Checked = true;
                    }

                    initDropDowns();
                    
                    if (DataViewID > -1) {
                        ddlResource.SelectedIndex = Toolkit.Max(-1, ddlResource.FindString(dr["dataview_name"].ToString()));
                        syncResourceFieldDropDown();
                        ddlResourceField.SelectedIndex = Toolkit.Max(-1, ddlResourceField.FindString(dr["dataview_field_name"].ToString()));

                    } else if (TableID > -1) {

                            ddlResource.SelectedIndex = Toolkit.Max(-1, ddlResource.FindString(dr["table_name"].ToString()));
                            syncResourceFieldDropDown();
                            ddlResourceField.SelectedIndex = Toolkit.Max(-1, ddlResourceField.FindString(dr["table_field_name"].ToString()));

                    } else {
                    }


                    string verbiage = getVerbiageForSpecialCommand(dr["compare_value"].ToString());
                    if (!String.IsNullOrEmpty(verbiage)) {
                        cboValue.SelectedIndex = cboValue.FindString(verbiage);
                        if (cboValue.SelectedIndex < 0) {
                            cboValue.Text = verbiage;
                        }
                    }
                    ddlCompare.SelectedIndex = ddlCompare.FindString(dr["compare_operator"].ToString());

                    verbiage = getVerbiageForSpecialCommand(dr["parent_compare_value"].ToString());
                    if (!String.IsNullOrEmpty(verbiage)) {
                        cboParentValue.SelectedIndex = cboParentValue.FindString(verbiage);
                        if (cboParentValue.SelectedIndex < 0) {
                            cboParentValue.Text = verbiage;
                        }
                    }
                    ddlParentCompare.SelectedIndex = ddlParentCompare.FindString(dr["parent_compare_operator"].ToString());


                } else {
                    initDropDowns();

                }


            });

            MarkClean();

            syncGUI();
        }

        private void initDropDowns() {
            
            if (ResourceType == "dataview") {

                initDataViewDropDown(ddlResource, true, null, ResourceID);
                ddlResource.SelectedIndex = 0;

                initDataViewFieldDropDown(ddlResourceField, ResourceID, true, null);
                ddlResourceField.SelectedIndex = 0;

                initTableDropDown(ddlParentTable, true, null, DataViewID, -1);
            } else if (ResourceType == "table") {

                initTableDropDown(ddlResource, true, null, ResourceID);
                ddlResource.SelectedIndex = 0;

                initTableFieldDropDown(ddlResourceField, ResourceID, true, null);

                initTableDropDown(ddlParentTable, true, null, ResourceID);

            } else {
                // what to do if neither???
            }


            if (ddlParentTable.Items.Count > 0) {
                ddlParentTable.SelectedIndex = 0;
            }

            int parentTableID = Toolkit.ToInt32(ddlParentTable.SelectedValue, -1);
            if (parentTableID > -1) {
                initTableFieldDropDown(ddlParentTableField, parentTableID, true, null);
                if (ddlParentTableField.Items.Count == 0) {
                    lblParentType.Text = "No parent is defined for " + ResourceType + " " + ddlResource.Text;
                } else {
                    for(int i=0;i<ddlParentTableField.Items.Count;i++){
                        DataRowView drv = ddlParentTableField.Items[i] as DataRowView;
                        int fieldID = Toolkit.ToInt32(drv["sys_table_field_id"], -1);
                        //if (ParentTableFieldID == fieldID) {
                        //    ddlParentTableField.SelectedIndex = i;
                        //    break;
                        //}
                    }
                }
            } else {
                lblParentType.Text = "No parent is defined for " + ResourceType + " " + ddlResource.Text;
            }


            initCompareDropdown(ddlCompare, "integer", "in");
            initCompareDropdown(ddlParentCompare, "integer", "in");

        }

        private void syncGUI() {
            if (rdoFieldValue.Checked) {
                ddlResource.Enabled = true;
                ddlResourceField.Enabled = true;
                ddlCompare.Enabled = true;
                cboValue.Enabled = true;

                ddlParentTable.Enabled = false;
                ddlParentTableField.Enabled = false;
                ddlParentCompare.Enabled = false;
                cboParentValue.Enabled = false;
            } else {
                ddlResource.Enabled = false;
                ddlResourceField.Enabled = false;
                ddlCompare.Enabled = false;
                cboValue.Enabled = false;

                ddlParentTable.Enabled = true;
                ddlParentTableField.Enabled = true;
                ddlParentCompare.Enabled = true;
                cboParentValue.Enabled = true;

            }
            CheckDirty();

        }


        private void btnOK_Click(object sender, EventArgs e) {

            if (rdoFieldValue.Checked) {
                if (ddlResourceField.SelectedIndex < 1) {
                    if (ResourceType == "dataview") {
                        MessageBox.Show(getDisplayMember("ok{nodvfield}", "You must select a valid Data View Field to restrict by."));
                    } else if (ResourceType == "table") {
                        MessageBox.Show(getDisplayMember("ok{notblfield}", "You must select a valid Table Field to restrict by."));
                    }
                    ddlResourceField.Focus();
                    return;
                }
            } else {
                if (ddlParentTableField.SelectedIndex < 1) {
                    MessageBox.Show(getDisplayMember("ok{noparenttable}", "You must select a valid Parent Table Field to restrict by."));
                    ddlParentTableField.Focus();
                    return;
                }
            }


            if (!checkSpecialValues(cboValue, lblType)) {
                return;
            }



            int dataviewFieldID = -1;
            int tableFieldID = -1;
            if (ResourceType == "dataview") {
                dataviewFieldID = Toolkit.ToInt32(ddlResourceField.SelectedValue, -1);
            } else if (ResourceType == "table") {
                tableFieldID = Toolkit.ToInt32(ddlResourceField.SelectedValue, -1);
            }

            string parentType = lblParentType.Text.ToLower().StartsWith("no parent") ? null : lblParentType.Text.Replace("(", "").Replace(")", "");

            AdminProxy.SavePermissionField(
                ID,
                PermissionID,
                dataviewFieldID,
                tableFieldID,
                lblType.Text.Replace("(", "").Replace(")", ""),
                ddlCompare.Text,
                getSpecialCommandFromVerbiage(cboValue.Text),
                Toolkit.ToInt32(ddlParentTableField.SelectedValue, -1),
                parentType,
                ddlParentCompare.Text,
                getSpecialCommandFromVerbiage(cboParentValue.Text),
                (rdoFieldValue.Checked ? "current" : "parent")
                );

            if (Modal) {
                DialogResult = DialogResult.OK;
                this.Close();
            } else {
                MainFormSelectParentTreeNode();
            }
        }

        private bool checkSpecialValues(ComboBox cbo, Label lbl) {
            // special commands don't have to pass type checking as they'll be replaced @ permission calculation time for a request by the middle tier

            var text = getSpecialCommandFromVerbiage(cbo.Text);
            if (!SPECIAL_COMMAND_VERBIAGE.Contains(text)) {

                switch (lblType.Text.ToLower()) {
                    case "string":
                        if (cbo.Text.Trim().Length == 0) {
                            MessageBox.Show(getDisplayMember("checkSpecialValues{compare}", "A value to compare against must be specified."));
                            cbo.Focus();
                            cbo.SelectAll();
                            return false;
                        }
                        break;
                    case "integer":
                        if (Toolkit.ToInt32(cbo.Text, int.MinValue + 1) == int.MinValue + 1) {
                            MessageBox.Show(getDisplayMember("checkSpecialValues{notint}", "A valid integer value must be specified."));
                            cbo.Focus();
                            cbo.SelectAll();
                            return false;
                        }
                        break;
                    case "datetime":
                        if (Toolkit.ToDateTime(cbo.Text, DateTime.MinValue.AddHours(2)) == DateTime.MinValue.AddHours(2)) {
                            MessageBox.Show(getDisplayMember("checkSpecialValues{notdate}", "A valid date value must be specified."));
                            cbo.Focus();
                            cbo.SelectAll();
                            return false;
                        }
                        break;
                    case "decimal":
                    case "float":
                    case "double":
                        if (Toolkit.ToDecimal(cbo.Text, decimal.MinValue + 1) == decimal.MinValue + 1) {
                            MessageBox.Show(getDisplayMember("chkSpecialValues{notdecimal}", "A valid decimal value must be specified."));
                            cbo.Focus();
                            cbo.SelectAll();
                            return false;
                        }
                        break;
                }
            }

            return true;

        }

        private void frmPermissionField_Load(object sender, EventArgs e) {
            
            cboValue.Items.Clear();
            cboValue.Items.AddRange(SPECIAL_COMMAND_VERBIAGE.ToArray());

            cboParentValue.Items.Clear();
            cboParentValue.Items.AddRange(SPECIAL_COMMAND_VERBIAGE.ToArray());

        }

        private void initCompareDropdown(ComboBox ddl, string type, string defaultValue) {

            ddl.Items.Clear();
            switch (type.Replace("(", "").Replace(")", "")) {
                case "string":
                    ddl.Items.Add("=");
                    ddl.Items.Add("<>");
                    ddl.Items.Add("like");
                    ddl.Items.Add("not like");
                    ddl.Items.Add("in");
                    break;
                case "int":
                case "integer":
                default:
                    ddl.Items.Add(">");
                    ddl.Items.Add(">=");
                    ddl.Items.Add("=");
                    ddl.Items.Add("<=");
                    ddl.Items.Add("<");
                    ddl.Items.Add("<>");
                    ddl.Items.Add("in");
                    break;
                case "datetime":
                    ddl.Items.Add(">");
                    ddl.Items.Add(">=");
                    ddl.Items.Add("=");
                    ddl.Items.Add("<=");
                    ddl.Items.Add("<");
                    ddl.Items.Add("<>");
                    break;
                case "decimal":
                case "float":
                case "numeric":
                case "double":
                    ddl.Items.Add(">");
                    ddl.Items.Add(">=");
                    ddl.Items.Add("=");
                    ddl.Items.Add("<=");
                    ddl.Items.Add("<");
                    ddl.Items.Add("<>");
                    break;
            }
            ddl.SelectedIndex = ddl.FindString(String.IsNullOrEmpty(defaultValue) ? "=" : defaultValue);
        }

        private void syncResourceFieldDropDown() {
            if (ResourceType == "dataview") {
                initDataViewFieldDropDown(ddlResourceField, Toolkit.ToInt32(ddlResource.SelectedValue, -1), true, "-- Pick One --");
            } else if (ResourceType == "table") {
                _tableFields = initTableFieldDropDown(ddlResourceField, Toolkit.ToInt32(ddlResource.SelectedValue, -1), true, "-- Pick One --");
            }
        }

        private void ddlResource_SelectedIndexChanged(object sender, EventArgs e) {
//            Sync(delegate() {
                syncResourceFieldDropDown();
                CheckDirty();
            //            });

        }

        private DataTable _tableFields;

        private void ddlResourceField_SelectedIndexChanged(object sender, EventArgs e) {
//            Sync(delegate() {
                int resID = Toolkit.ToInt32(ddlResource.SelectedValue, -1);
                int resFieldID = Toolkit.ToInt32(ddlResourceField.SelectedValue, -1);
                if (ResourceType == "dataview") {
                    var dt = AdminProxy.ListDataViewFields(resID, resFieldID).Tables["list_dataview_fields"];

                    if (dt.Rows.Count > 0) {
                        lblType.Text = "(" + dt.Rows[0]["field_type"].ToString() + ")";
                        initCompareDropdown(ddlCompare, lblType.Text.ToLower(), ddlCompare.Text);
                    }
                } else if (ResourceType == "table") {
                    var dt = AdminProxy.ListTableFields(resID, resFieldID, false).Tables["list_table_fields"];

                    if (dt.Rows.Count > 0) {
                        lblType.Text = "(" + dt.Rows[0]["field_type"].ToString() + ")";
                        initCompareDropdown(ddlCompare, lblType.Text.ToLower(), ddlCompare.Text);
                    }
                }
                CheckDirty();
            //           });

        }
        private void rdoFieldValue_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void forceResourceFieldToForeignKey() {
            if (_tableFields != null) {
                // force the field value section to using the primary key field name
                for (var i = 0; i < _tableFields.Rows.Count; i++) {
                    if (_tableFields.Rows[i]["is_foreign_key"].ToString() == "Y" && _tableFields.Rows[i]["table_field_name"].ToString() == ddlParentTable.Text + "_id") {
                        ddlResourceField.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void forceResourceFieldToPrimaryKey() {
            if (_tableFields != null) {
                // force the field value section to using the primary key field name
                for (var i = 0; i < _tableFields.Rows.Count; i++) {
                    if (_tableFields.Rows[i]["is_primary_key"].ToString() == "Y" && _tableFields.Rows[i]["table_field_name"].ToString() == ddlParentTable.Text + "_id") {
                        ddlResourceField.SelectedIndex = i;
                        break;
                    }
                }
            }
        }


        private void rdoResolveValue_CheckedChanged(object sender, EventArgs e) {
            if (rdoChildValue.Checked) {
                forceResourceFieldToForeignKey();
            }
            syncGUI();
        }

        private void ddlParentTable_SelectedIndexChanged(object sender, EventArgs e) {
//            Sync(delegate() {
            initTableFieldDropDown(ddlParentTableField, Toolkit.ToInt32(ddlParentTable.SelectedValue, -1), true, "-- Pick One --");
                //forceResourceFieldToForeignKey();
                forceResourceFieldToPrimaryKey();
                CheckDirty();
            //            });

        }

        private void ddlParentTableField_SelectedIndexChanged(object sender, EventArgs e) {
//            Sync(delegate() {
            var dt = AdminProxy.ListTableFields(Toolkit.ToInt32(ddlParentTable.SelectedValue, -1), Toolkit.ToInt32(ddlParentTableField.SelectedValue, -1), false).Tables["list_table_fields"];

                if (dt.Rows.Count > 0) {
                    lblParentType.Text = "(" + dt.Rows[0]["field_type"].ToString() + ")";
                }
                initCompareDropdown(ddlParentCompare, lblParentType.Text.ToLower(), ddlParentCompare.Text);
                CheckDirty();
            //            });
        }

        private void ddlCompare_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void cboValue_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlParentCompare_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void cboParentValue_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void cboValue_TextChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private void cboParentValue_TextChanged(object sender, EventArgs e) {
            CheckDirty();
        }
        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmPermissionField", resourceName, null, defaultValue, substitutes);
        }
    }
}
