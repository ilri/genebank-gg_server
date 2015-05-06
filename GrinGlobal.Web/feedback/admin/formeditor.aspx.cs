using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using GrinGlobal.Business;
using System.Data;
using System.Diagnostics;

namespace GrinGlobal.Web.feedback.admin {
    public partial class formbuilder : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                ViewState["programID"] = Request["programid"];
                ViewState["reportID"] = Request["reportid"];

                bindDropDowns();

                var formID = Toolkit.ToInt32(Request["id"], -1);
                if (formID > -1) {
                    ViewState["formID"] = formID;
                    pnlMustSaveFirst.Visible = false;
                    pnlFieldsAndTraits.Visible = true;
                    bindData(formID, null, null);
                } else {
                    pnlMustSaveFirst.Visible = true;
                }
            }
        }

        private void bindDropDowns() {
            using (var sd = UserManager.GetSecureData(false)) {
                Master.FillCodeGroupDropDown(ddlFieldGroups, sd);
                Master.FillWebLookupDropDown(ddlFieldLookups, sd);

                var dtCrop = sd.GetData("web_descriptorbrowse_crop", null, 0, 0).Tables["web_descriptorbrowse_crop"];
                var drNone = dtCrop.NewRow();
                drNone["name"] = " -- Select One -- ";
                drNone["crop_id"] = "-1";
                dtCrop.Rows.InsertAt(drNone, 0);

                ddlCrop.DataSource = dtCrop;
                ddlCrop.DataBind();
            }
        }

        private void bindData(int formID, int? fieldID, int? traitID) {
            using (var sd = UserManager.GetSecureData(false)) {

                //Master.FillCodeGroupDropDown(ddlFieldType, "FIELD_TYPE", _sd);

                var dt = sd.GetData("web_feedback_form", ":feedbackformid=" + formID, 0, 0).Tables["web_feedback_form"];
                if (dt.Rows.Count > 0) {
                    txtFormName.Text = dt.Rows[0]["title"].ToString();
                }

                gvFields.DataSource = getFields(sd, formID, fieldID);
                gvFields.DataBind();
                if (gvFields.Rows.Count > 0) {
                    var gvr = gvFields.Rows[gvFields.Rows.Count - 1];
                    var btn = gvr.FindControl("btnDown");
                    btn.Visible = false;
                }

                gvTraits.DataSource = getTraits(sd, formID, traitID);
                gvTraits.DataBind();
                if (gvTraits.Rows.Count > 0) {
                    var gvr = gvTraits.Rows[gvTraits.Rows.Count - 1];
                    var btn = gvr.FindControl("btnDown");
                    btn.Visible = false;
                }

            }
        }

        private void bindFields(int? fieldID) {
            using (var sd = UserManager.GetSecureData(false)) {
                var formID = (int)ViewState["formID"];
                gvFields.DataSource = getFields(sd, formID, fieldID);
                gvFields.DataBind();
                if (gvFields.Rows.Count > 0) {
                    var gvr = gvFields.Rows[gvFields.Rows.Count - 1];
                    var btn = gvr.FindControl("btnDown");
                    btn.Visible = false;
                }
            }
        }

        private void bindTraits(int? traitID) {
            using (var sd = UserManager.GetSecureData(false)) {
                var formID = (int)ViewState["formID"];
                gvTraits.DataSource = getTraits(sd, formID, traitID);
                gvTraits.DataBind();
                if (gvTraits.Rows.Count > 0) {
                    var gvr = gvTraits.Rows[gvTraits.Rows.Count - 1];
                    var btn = gvr.FindControl("btnDown");
                    btn.Visible = false;
                }
            }
        }

        private DataTable getFields(SecureData sd, int formID, int? fieldID) {
            return sd.GetData("web_feedback_form_field_list", ":feedbackformid=" + formID + ";:feedbackformfieldid=" + fieldID, 0, 0).Tables["web_feedback_form_field_list"];
        }

        private DataTable getTraits(SecureData sd, int formID, int? traitID) {
            return sd.GetData("web_feedback_form_trait_list", ":feedbackformid=" + formID + ";:feedbackformtraitid=" + traitID, 0, 0).Tables["web_feedback_form_trait_list"];
        }

        protected void gvFields_RowEditing(object sender, GridViewEditEventArgs e) {
            using (var sd = UserManager.GetSecureData(false)) {
                var formID = (int)ViewState["formID"];

                gvFields.EditIndex = e.NewEditIndex;
                gvFields.DataSource = getFields(sd, formID, null);
                gvFields.DataBind();
                if (gvFields.Rows.Count > 0) {
                    var gvr = gvFields.Rows[gvFields.Rows.Count - 1];
                    var btn = gvr.FindControl("btnDown");
                    btn.Visible = false;
                }
            }
        }

        protected void gvFields_RowDeleted(object sender, GridViewDeletedEventArgs e) {
        }

        protected void gvFields_RowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName == "UP") {
                var formID = Toolkit.ToInt32(ViewState["formID"], -1);
                var fieldID = Toolkit.ToInt32(e.CommandArgument, -1);

                using (var sd = UserManager.GetSecureData(false)) {
                    // grab all fields
                    var dt = sd.GetData("web_feedback_form_field_list", ":feedbackformfieldid=;:feedbackformid=" + formID, 0, 0).Tables["web_feedback_form_field_list"];
                    for (var i = 0; i < dt.Rows.Count; i++) {
                        var dr = dt.Rows[i];
                        dr["sort_order"] = i;
                        if (Toolkit.ToInt32(dr["feedback_form_field_id"], -2) == fieldID) {
                            // find the index of our field
                            if (i > 0) {
                                // swap it and the previous one
                                var offset = Toolkit.ToInt32(dr["sort_order"], 1);
                                var prevOffset = Toolkit.ToInt32(dt.Rows[i - 1]["sort_order"], 0);
                                if (offset == prevOffset) {
                                    offset++;
                                }
                                dt.Rows[i - 1]["sort_order"] = offset;
                                dr["sort_order"] = prevOffset;
                            }
                        }
                    }

                    // write back to database
                    sd.SaveData(dt.DataSet, true, null);

                }

                bindData(formID, null, null);


            } else if (e.CommandName == "DOWN") {
                var formID = Toolkit.ToInt32(ViewState["formID"], -1);
                var fieldID = Toolkit.ToInt32(e.CommandArgument, -1);

                using (var sd = UserManager.GetSecureData(false)) {
                    // grab all fields
                    var dt = sd.GetData("web_feedback_form_field_list", ":feedbackformfieldid=;:feedbackformid=" + formID, 0, 0).Tables["web_feedback_form_field_list"];
                    for (var i = 0; i < dt.Rows.Count; i++) {
                        var dr = dt.Rows[i];
                        dr["sort_order"] = i;
                        if (Toolkit.ToInt32(dr["feedback_form_field_id"], -2) == fieldID) {
                            // find the index of our field
                            if (i < dt.Rows.Count-1) {
                                // swap it and the next one
                                var offset = Toolkit.ToInt32(dr["sort_order"], 1);
                                var nextOffset = Toolkit.ToInt32(dt.Rows[i + 1]["sort_order"], 0);
                                if (offset == nextOffset) {
                                    offset--;
                                }
                                dt.Rows[i + 1]["sort_order"] = offset;
                                dr["sort_order"] = nextOffset;
                                i++;
                            }
                        }
                    }

                    // write back to database
                    sd.SaveData(dt.DataSet, true, null);

                }

                bindData(formID, null, null);
            }
        }

        protected void gvFields_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row != null && e.Row.RowIndex > -1) {
                if (e.Row.RowIndex == 0) {
                    var btnUp = e.Row.FindControl("btnUp");
                    btnUp.Visible = false;
                }

                if (e.Row.RowIndex == gvFields.EditIndex) {
                    var dr = ((DataRowView)e.Row.DataItem).Row as DataRow;
                    var ddlDisplayAs = e.Row.FindControl("ddlFieldDisplayAs") as DropDownList;
                    if (ddlDisplayAs != null) {
                        ddlDisplayAs.SelectedValue = dr["gui_hint"] as string;
                    }

                    var chkReadOnly = e.Row.FindControl("chkReadOnly") as CheckBox;
                    chkReadOnly.Checked = dr["is_readonly"].ToString().ToUpper() == "Y";

                    var chkRequired = e.Row.FindControl("chkRequired") as CheckBox;
                    chkRequired.Checked = dr["is_required"].ToString().ToUpper() == "Y";

                    var txtDefault = e.Row.FindControl("txtDefaultValue") as TextBox;
                    txtDefault.Text = dr["default_value"].ToString();

                    var txtCategories = e.Row.FindControl("txtCategories") as TextBox;
                    txtCategories.Text = dr["category_tag"].ToString();

                    using (var sd = UserManager.GetSecureData(false)) {
                        var ddlLookups = e.Row.FindControl("ddlFieldLookups") as DropDownList;
                        Master.FillWebLookupDropDown(ddlLookups, sd);
                        ddlLookups.SelectedValue = dr["foreign_key_dataview_name"].ToString();

                        var ddlGroups = e.Row.FindControl("ddlFieldGroups") as DropDownList;
                        Master.FillCodeGroupDropDown(ddlGroups, sd);
                        ddlGroups.SelectedValue = dr["group_name"].ToString();
                    }

                }
            }
        }

        protected void gvTraits_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
            gvTraits.EditIndex = -1;
            bindTraits(null);
        }

        protected void gvFields_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
            gvFields.EditIndex = -1;
            bindFields(null);
        }

        protected void gvFields_RowCreated(object sender, GridViewRowEventArgs e) {

        }

        protected void btnAddField_Click(object sender, EventArgs e) {
            using (var sd = UserManager.GetSecureData(false)) {

                var formID = (int)ViewState["formID"];

                var dt = getFields(sd, formID, null);

                var dr = dt.NewRow();
                dr["feedback_form_field_id"] = -1;
                dr["feedback_form_id"] = formID;
                dr["title"] = txtFieldName.Text;
                dr["gui_hint"] = ddlFieldDisplayAs.SelectedValue;
                dr["field_type_code"] = "STRING";
                dr["foreign_key_dataview_name"] = ddlFieldLookups.SelectedValue;
                dr["group_name"] = ddlFieldGroups.SelectedValue;
                if (dt.Rows.Count > 0) {
                    dr["sort_order"] = Toolkit.ToInt32(dt.Rows[dt.Rows.Count - 1]["sort_order"], 0) + 1;
                } else {
                    dr["sort_order"] = 0;
                }
                dr["category_tag"] = txtFieldTags.Text;
                dr["is_readonly"] = chkIsReadOnly.Checked ? "Y" : "N";
                dr["is_required"] = chkIsRequired.Checked ? "Y" : "N";
                dr["default_value"] = txtDefaultValue.Text;
                dt.Rows.Add(dr);

                sd.SaveData(dt.DataSet, true, null);
                
                txtFieldName.Text = "";
                ddlFieldDisplayAs.SelectedIndex = 0;
                ddlFieldGroups.SelectedIndex = 0;
                ddlFieldLookups.SelectedIndex = 0;

                Master.ShowMessage("Field successfully added.");

            }

            bindFields(null);

        }

        protected void gvFields_RowUpdated(object sender, GridViewUpdatedEventArgs e) {
        }

        protected void gvFields_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            using (var sd = UserManager.GetSecureData(false)) {
                var formID = (int)ViewState["formID"];
                var dt = getFields(sd, formID, Toolkit.ToInt32(gvFields.DataKeys[e.RowIndex][0], -1));
                if (dt.Rows.Count > 0) {
                    dt.Rows[0].Delete();
                    sd.SaveData(dt.DataSet, true, null);
                }
                gvFields.EditIndex = -1;
                gvFields.DataSource = getFields(sd, formID, null);
                gvFields.DataBind();
                if (gvFields.Rows.Count > 0) {
                    var gvr = gvFields.Rows[gvFields.Rows.Count - 1];
                    var btn = gvr.FindControl("btnDown");
                    btn.Visible = false;
                }
                Master.ShowMessage("Field successfully deleted.");
            }
        }

        protected void gvFields_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            using (var sd = UserManager.GetSecureData(false)) {

                var formID = (int)ViewState["formID"];
                var fieldID = Toolkit.ToInt32(gvFields.DataKeys[e.RowIndex][0], -1);
                var dt = getFields(sd, formID, fieldID);

                if (dt.Rows.Count > 0) {

                    var gvr = gvFields.Rows[e.RowIndex];
                    var dr = dt.Rows[0];
                    dr["feedback_form_id"] = formID;
                    dr["title"] = ((TextBox)gvr.FindControl("txtFieldName")).Text;
                    dr["gui_hint"] = ((DropDownList)gvr.FindControl("ddlFieldDisplayAs")).SelectedValue;
                    dr["group_name"] = ((DropDownList)gvr.FindControl("ddlFieldGroups")).SelectedValue;
                    dr["foreign_key_dataview_name"] = ((DropDownList)gvr.FindControl("ddlFieldLookups")).SelectedValue;
                    dr["field_type_code"] = "STRING";
                    dr["is_readonly"] = ((CheckBox)gvr.FindControl("chkReadOnly")).Checked ? "Y" : "N";
                    dr["is_required"] = ((CheckBox)gvr.FindControl("chkRequired")).Checked ? "Y" : "N";
                    dr["default_value"] = ((TextBox)gvr.FindControl("txtDefaultValue")).Text;
                    dr["category_tag"] = ((TextBox)gvr.FindControl("txtCategories")).Text;


                    sd.SaveData(dt.DataSet, true, null);
                    Master.ShowMessage("Field successfully saved.");
                }

            }
            gvFields.EditIndex = -1;

            bindFields(null);

        }

        protected void btnSaveForm_Click(object sender, EventArgs e) {
            using (var sd = UserManager.GetSecureData(false)) {
                var formID = Toolkit.ToInt32(ViewState["formID"], -1);
                var dt = sd.GetData("web_feedback_form", ":feedbackformid=" + formID, 0, 0).Tables["web_feedback_form"];
                if (dt.Rows.Count == 0) {
                    var dr = dt.NewRow();
                    dr["feedback_form_id"] = -1;
                    dr["title"] = txtFormName.Text;
                    dt.Rows.Add(dr);
                    var saved = sd.SaveData(dt.DataSet, true, null).Tables[dt.TableName];
                    if (saved.Rows.Count > 0) {
                        ViewState["formID"] = Toolkit.ToInt32(saved.Rows[0]["NewPrimaryKeyID"], -1);
                    }
                    pnlFieldsAndTraits.Visible = true;

                } else {
                    var dr = dt.Rows[0];
                    dr["title"] = txtFormName.Text;
                    sd.SaveData(dt.DataSet, true, null);
                }
                Master.ShowMessage("Feedback form successfully saved.");
            }
        }

        private void fillTraits(int cropID, DropDownList ddl) {
            using (var sd = UserManager.GetSecureData(true)) {
                //var dt = sd.GetData("web_descriptorbrowse_trait", ":langid=" + sd.LanguageID + ";:cat=" + null + ";:cropid=" + cropID, 0, 0).Tables["web_descriptorbrowse_trait"];
                var dt = sd.GetData("web_descriptorbrowse_trait", ":langid=" + sd.LanguageID + ";:cat=" + null + ";:cropid=" + cropID, 0, 0).Tables["web_descriptorbrowse_trait"];

                ddl.DataSource = dt;
                ddl.DataBind();
            }
        }

        protected void ddlCrop_SelectedIndexChanged(object sender, EventArgs e) {
            fillTraits(Toolkit.ToInt32(ddlCrop.SelectedValue, -1), ddlCropTrait);

        }

        protected void btnAddTrait_Click(object sender, EventArgs e) {
            using (var sd = UserManager.GetSecureData(false)) {

                var formID = (int)ViewState["formID"];

                var dt = getTraits(sd, formID, null);

                var customTitle = String.IsNullOrEmpty(txtTitle.Text) ? (object)DBNull.Value : (object)txtTitle.Text;
                var customDescription = String.IsNullOrEmpty(txtDescription.Text) ? (object)DBNull.Value : (object)txtDescription.Text;

                var dr = dt.NewRow();
                dr["feedback_form_trait_id"] = -1;
                dr["feedback_form_id"] = formID;
                dr["crop_trait_id"] = Toolkit.ToInt32(ddlCropTrait.SelectedValue, -1);
                dr["custom_title"] = txtTitle.Text;
                dr["custom_description"] = txtDescription.Text;
                if (dt.Rows.Count > 0) {
                    dr["sort_order"] = Toolkit.ToInt32(dt.Rows[dt.Rows.Count - 1]["sort_order"], 0) + 1;
                } else {
                    dr["sort_order"] = 0;
                }
                dt.Rows.Add(dr);

                sd.SaveData(dt.DataSet, true, null);

                ddlCrop.SelectedIndex = 0;
                ddlCropTrait.DataSource = null;
                ddlCropTrait.DataBind();
                ddlCropTrait.Items.Clear();
                ddlCropTrait.SelectedIndex = -1;
                txtTitle.Text = "";
                txtDescription.Text = "";

                Master.ShowMessage("Trait successfully added.");

            }

            bindTraits(null);

        }

        protected void gvTraits_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            using (var sd = UserManager.GetSecureData(false)) {

                var formID = (int)ViewState["formID"];
                var traitID = Toolkit.ToInt32(gvTraits.DataKeys[e.RowIndex][0], -1);
                var dt = getTraits(sd, formID, traitID);

                if (dt.Rows.Count > 0) {

                    var gvr = gvTraits.Rows[e.RowIndex];
                    var dr = dt.Rows[0];

                    dr["feedback_form_id"] = formID;
                    
                    var ddl = ((DropDownList)gvr.FindControl("ddlCropTrait"));
                    dr["crop_trait_id"] = Toolkit.ToInt32(ddl.SelectedValue, -1);

                    dr["custom_title"] = ((TextBox)gvr.FindControl("txtCustomTitle")).Text;
                    dr["custom_description"] = ((TextBox)gvr.FindControl("txtCustomDescription")).Text;

                    sd.SaveData(dt.DataSet, true, null);
                    Master.ShowMessage("Trait successfully saved.");
                }

            }
            gvTraits.EditIndex = -1;

            bindTraits(null);

        }

        protected void gvTraits_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            using (var sd = UserManager.GetSecureData(false)) {
                var formID = (int)ViewState["formID"];
                var dt = getTraits(sd, formID, Toolkit.ToInt32(gvTraits.DataKeys[e.RowIndex][0], -1));
                if (dt.Rows.Count > 0) {
                    dt.Rows[0].Delete();
                    sd.SaveData(dt.DataSet, true, null);
                }
                gvTraits.EditIndex = -1;
                gvTraits.DataSource = getTraits(sd, formID, null);
                gvTraits.DataBind();
                if (gvTraits.Rows.Count > 0) {
                    var gvr = gvTraits.Rows[gvTraits.Rows.Count - 1];
                    var btn = gvr.FindControl("btnDown");
                    btn.Visible = false;
                }
                Master.ShowMessage("Trait successfully deleted.");
            }

        }

        protected void gvTraits_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row != null && e.Row.RowIndex > -1) {
                if (e.Row.RowIndex == 0) {
                    var btnUp = e.Row.FindControl("btnUp");
                    btnUp.Visible = false;
                }

                if (e.Row.RowIndex == gvTraits.EditIndex) {
                    var dr = ((DataRowView)e.Row.DataItem).Row as DataRow;
                    var ddlCropTrait = e.Row.FindControl("ddlCropTrait") as DropDownList;
                    fillTraits(Toolkit.ToInt32(dr["crop_id"], -1), ddlCropTrait);
                    ddlCropTrait.SelectedValue = dr["crop_trait_id"] as string;
                }
            }

        }

        protected void gvTraits_RowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName == "UP") {
                var formID = Toolkit.ToInt32(ViewState["formID"], -1);
                var traitID = Toolkit.ToInt32(e.CommandArgument, -1);

                using (var sd = UserManager.GetSecureData(false)) {
                    // grab all fields
                    var dt = sd.GetData("web_feedback_form_trait_list", ":feedbackformtraitid=;:feedbackformid=" + formID, 0, 0).Tables["web_feedback_form_trait_list"];
                    for (var i = 0; i < dt.Rows.Count; i++) {
                        var dr = dt.Rows[i];
                        dr["sort_order"] = i;
                        if (Toolkit.ToInt32(dr["feedback_form_trait_id"], -2) == traitID) {
                            // find the index of our field
                            if (i > 0) {
                                // swap it and the previous one
                                var offset = Toolkit.ToInt32(dr["sort_order"], 1);
                                var prevOffset = Toolkit.ToInt32(dt.Rows[i - 1]["sort_order"], 0);
                                if (offset == prevOffset) {
                                    offset++;
                                }
                                dt.Rows[i - 1]["sort_order"] = offset;
                                dr["sort_order"] = prevOffset;
                            }
                        }
                    }

                    // write back to database
                    sd.SaveData(dt.DataSet, true, null);

                }

                bindData(formID, null, null);


            } else if (e.CommandName == "DOWN") {
                var formID = Toolkit.ToInt32(ViewState["formID"], -1);
                var fieldID = Toolkit.ToInt32(e.CommandArgument, -1);

                using (var sd = UserManager.GetSecureData(false)) {
                    // grab all fields
                    var dt = sd.GetData("web_feedback_form_trait_list", ":feedbackformtraitid=;:feedbackformid=" + formID, 0, 0).Tables["web_feedback_form_trait_list"];
                    for (var i = 0; i < dt.Rows.Count; i++) {
                        var dr = dt.Rows[i];
                        dr["sort_order"] = i;
                        if (Toolkit.ToInt32(dr["feedback_form_trait_id"], -2) == fieldID) {
                            // find the index of our field
                            if (i < dt.Rows.Count - 1) {
                                // swap it and the next one
                                var offset = Toolkit.ToInt32(dr["sort_order"], 1);
                                var nextOffset = Toolkit.ToInt32(dt.Rows[i + 1]["sort_order"], 0);
                                if (offset == nextOffset) {
                                    offset--;
                                }
                                dt.Rows[i + 1]["sort_order"] = offset;
                                dr["sort_order"] = nextOffset;
                                i++;
                            }
                        }
                    }

                    // write back to database
                    sd.SaveData(dt.DataSet, true, null);

                }

                bindData(formID, null, null);
            }

        }

        protected void gvTraits_RowEditing(object sender, GridViewEditEventArgs e) {
            using (var sd = UserManager.GetSecureData(false)) {
                var formID = (int)ViewState["formID"];

                gvTraits.EditIndex = e.NewEditIndex;
                gvTraits.DataSource = getTraits(sd, formID, null);
                gvTraits.DataBind();
                if (gvTraits.Rows.Count > 0) {
                    var gvr = gvTraits.Rows[gvTraits.Rows.Count - 1];
                    var btn = gvr.FindControl("btnDown");
                    btn.Visible = false;
                }
            }

        }

    }
}
