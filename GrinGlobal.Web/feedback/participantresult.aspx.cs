using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using System.Data;
using GrinGlobal.Business;
using System.IO;

namespace GrinGlobal.Web.feedback {
    public partial class participantresult : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                var groupID = Toolkit.ToInt32(Request["groupid"], -1);
                ViewState["groupID"] = groupID;
                if (groupID > -1) {
                    bindDropDown(groupID);
                    bindData();
                } else {
                    Response.Redirect("~/feedback/participantdefault.aspx");
                }
            }
        }

        private void bindDropDown(int groupID) {


            using (var sd = UserManager.GetSecureData(false)) {

                var dt = sd.GetData("web_feedback_result_accessions", ":groupid=" + groupID, 0, 0).Tables["web_feedback_result_accessions"];
                ddlAccession.DataSource = dt;
                ddlAccession.DataBind();

                if (dt.Rows.Count > 0) {
                    ViewState["previousResultAndInventory"] = ddlAccession.SelectedValue;
                }

            }
        }

        private SecureData _sd;

        private bool _hasBeenSubmitted;

        private void bindData(){

            using(_sd = UserManager.GetSecureData(false)){

                var resultID = getResultAndInventoryIDs(false)[0];

                var dt = _sd.GetData("web_feedback_result_field", ":feedbackresultid=" + resultID, 0, 0).Tables["web_feedback_result_field"];
                _hasBeenSubmitted = false;
                if (dt.Rows.Count > 0) {
                    var dr = dt.Rows[0];
                    _hasBeenSubmitted = Toolkit.ToDateTime(dr["submitted_date"], DateTime.MinValue) > DateTime.MinValue;
                    litReport.Text = dr["report_title"].ToString();
                    litProgram.Text = dr["program_title"].ToString();
                }

                if (_hasBeenSubmitted) {
                    btnSubmit.Visible = false;
                    btnSaveForLater.Visible = false;
                    lblHasBeenSubmitted.Visible = true;
                }

                rptFields.DataSource = dt;
                rptFields.DataBind();

                var dtTraits = _sd.GetData("web_feedback_result_trait_obs", ":feedbackresultid=" + resultID, 0, 0).Tables["web_feedback_result_trait_obs"];

                rptTraits.DataSource = dtTraits;
                rptTraits.DataBind();

                bindAttachments(resultID, null, _sd);

            }
        }

        private void bindAttachments(int resultID, int? resultAttachID, SecureData sd) {
            var dtAttach = sd.GetData("web_feedback_result_attachments", ":feedbackresultid=" + resultID + ";:feedbackresultattachid=" + resultAttachID, 0, 0).Tables["web_feedback_result_attachments"];
            gvAttachments.DataSource = dtAttach;
            gvAttachments.DataBind();
        }

        protected void rptFields_ItemDataBound(object sender, RepeaterItemEventArgs e) {
            if (e.Item.DataItem != null){
                var dr = (e.Item.DataItem as DataRowView).Row;

                (e.Item.FindControl("hidFieldResultID") as HiddenField).Value = dr["feedback_result_field_id"].ToString();
                (e.Item.FindControl("hidFieldFormID") as HiddenField).Value = dr["fff_id"].ToString();

                var title = dr["title"].ToString();
                var lblTitle = e.Item.FindControl("lblTitle") as Label;
                lblTitle.Text = title;

                var desc = dr["description"].ToString();
                var lblDesc = e.Item.FindControl("lblDescription") as Label;
                lblDesc.Text = desc;

                var hidField = e.Item.FindControl("hidFieldType") as HiddenField;
                var hidLookup = e.Item.FindControl("hidLookupID") as HiddenField;

                var txtField = e.Item.FindControl("txtField") as TextBox;
                var calField = e.Item.FindControl("calField") as Calendar;
                var btnField = e.Item.FindControl("btnField") as Button;
                var ddlField = e.Item.FindControl("ddlField") as DropDownList;
                var chkField = e.Item.FindControl("chkField") as CheckBox;
                var revFieldInteger = e.Item.FindControl("revFieldInteger") as RegularExpressionValidator;
                var revFieldDecimal = e.Item.FindControl("revFieldDecimal") as RegularExpressionValidator;
                var reqField = e.Item.FindControl("reqField") as RequiredFieldValidator;
                var lblRequired = e.Item.FindControl("lblRequired") as Label;
                var litField = e.Item.FindControl("litField") as Literal;

                reqField.Text = lblTitle.Text + " is required";
                reqField.ErrorMessage = lblTitle.Text + " is required";

                if (_hasBeenSubmitted) {
                    txtField.Enabled = false;
                    calField.Enabled = false;
                    btnField.Enabled = false;
                    ddlField.Enabled = false;
                    chkField.Enabled = false;
                }
                
                var defaultValue = dr["default_value"].ToString();
                var actualValue = dr["string_value"].ToString();

                txtField.Visible = false;
                calField.Visible = false;
                btnField.Visible = false;
                ddlField.Visible = false;
                chkField.Visible = false;
                litField.Visible = false;

                var guiHint = dr["gui_hint"].ToString();
                var groupName = dr["group_name"].ToString();
                var lookupDataview = dr["foreign_key_dataview_name"].ToString();

                var isrequired = dr["is_required"].ToString().ToUpper() == "Y";
                var isreadonly = dr["is_readonly"].ToString().ToUpper() == "Y" || _hasBeenSubmitted;

                var type = dr["field_type_code"].ToString();
                switch (guiHint.ToUpper()) {
                    case "DATE_CONTROL":
                        calField.Visible = true;
                        hidField.Value = "CALENDAR";
                        calField.SelectedDate = Toolkit.ToDateTime(defaultValue, DateTime.UtcNow);
                        if (isrequired) {
                            reqField.ControlToValidate = calField.ID;
                            reqField.Enabled = true;
                            reqField.Visible = true;
                            lblRequired.Visible = true;
                        }

                        if (isreadonly) {
                            txtField.ReadOnly = true;
                            txtField.Enabled = false;
                        }
                        break;

                    case "LARGE_SINGLE_SELECT_CONTROL":
                        hidField.Value = "LOOKUP";
                        txtField.Visible = true;
                        txtField.ReadOnly = true;
                        txtField.Width = 500;
                        btnField.Visible = true;
                        btnField.Text = "...";
                        btnField.CommandName = "lookup";
                        btnField.CommandArgument = lookupDataview;

                        var id = Toolkit.ToInt32(Toolkit.Coalesce(actualValue, defaultValue), -1);
                        hidLookup.Value = id.ToString();
                        txtField.Text = getLookupText(lookupDataview, id);

                        if (isrequired) {
                            reqField.ControlToValidate = txtField.ID;
                            reqField.Enabled = true;
                            reqField.Visible = true;
                            lblRequired.Visible = true;
                        }

                        if (isreadonly) {
                            btnField.Enabled = false;
                        }
                        // remember given lookupDataview so we can present them with a popup to choose from
                        
                        break;
                    case "SMALL_SINGLE_SELECT_CONTROL":
                        hidField.Value = "DROPDOWN";
                        ddlField.Visible = true;
                        // using given group name, find list of items to display
                        Master.FillCodeGroupDropDown(ddlField, groupName, _sd);
                        // select appropriate item
                        ddlField.SelectedIndex = ddlField.Items.IndexOf(ddlField.Items.FindByValue(Toolkit.Coalesce(actualValue, defaultValue) as string));

                        if (isrequired) {
                            reqField.ControlToValidate = ddlField.ID;
                            reqField.Enabled = true;
                            reqField.Visible = true;
                            lblRequired.Visible = true;
                        }
                        if (isreadonly) {
                            ddlField.Enabled = false;
                        }

                        break;
                    case "INTEGER_CONTROL":
                        hidField.Value = "TEXT";
                        txtField.Visible = true;
                        txtField.Text = actualValue;
                        revFieldInteger.Enabled = true;
                        revFieldInteger.ControlToValidate = txtField.ID;
                        revFieldInteger.Visible = true;

                        if (isrequired) {
                            reqField.ControlToValidate = txtField.ID;
                            reqField.Enabled = true;
                            reqField.Visible = true;
                            lblRequired.Visible = true;
                        }

                        if (isreadonly) {
                            txtField.ReadOnly = true;
                            txtField.Enabled = false;
                        }
                        break;

                    case "DECIMAL_CONTROL":
                        hidField.Value = "TEXT";
                        txtField.Visible = true;
                        txtField.Text = actualValue;
                        revFieldDecimal.Enabled = true;
                        revFieldDecimal.ControlToValidate = txtField.ID;
                        revFieldDecimal.Visible = true;

                        if (isrequired) {
                            reqField.ControlToValidate = txtField.ID;
                            reqField.Enabled = true;
                            reqField.Visible = true;
                            lblRequired.Visible = true;
                        }

                        if (isreadonly) {
                            txtField.ReadOnly = true;
                            txtField.Enabled = false;
                        }
                        break;

                    case "BOOLEAN_CONTROL":
                        hidField.Value = "BOOL";
                        chkField.Visible = true;
                        chkField.Checked = Toolkit.Coalesce(actualValue, defaultValue) as string == "Y";
                        chkField.Text = lblDesc.Text;
                        lblDesc.Visible = false;

                        if (isrequired) {
                            reqField.ControlToValidate = chkField.ID;
                            reqField.Enabled = true;
                            reqField.Visible = true;
                            lblRequired.Visible = true;
                        }
                        if (isreadonly) {
                            chkField.Enabled = false;
                        }

                        break;

                    case "LONGTEXT_CONTROL":
                        hidField.Value = "TEXT";
                        txtField.Visible = true;
                        txtField.Text = actualValue;
                        txtField.TextMode = TextBoxMode.MultiLine;
                        txtField.Rows = 6;
                        txtField.Columns = 60;

                        if (isrequired) {
                            reqField.ControlToValidate = txtField.ID;
                            reqField.Enabled = true;
                            reqField.Visible = true;
                            lblRequired.Visible = true;
                        }
                        if (isreadonly) {
                            txtField.ReadOnly = true;
                            txtField.Enabled = false;
                        }

                        break;

                    case "LITERAL_CONTROL":
                        hidField.Value = "LITERAL";
                        litField.Text = "<p>" + defaultValue + "</p>";
                        litField.Visible = true;
                        break;

                    case "HR_CONTROL":
                        hidField.Value = "LITERAL";
                        lblTitle.Visible = false;
                        litField.Text = "<hr />";
                        litField.Visible = true;
                        break;
                    case "H1_CONTROL":
                        hidField.Value = "LITERAL";
                        lblTitle.Visible = false;
                        litField.Text = "<h1>" + defaultValue + "</h2>";
                        litField.Visible = true;
                        break;
                    case "H2_CONTROL":
                        hidField.Value = "LITERAL";
                        lblTitle.Visible = false;
                        litField.Text = "<h2>" + defaultValue + "</h2>";
                        litField.Visible = true;
                        break;
                    case "BR_CONTROL":
                        hidField.Value = "LITERAL";
                        lblTitle.Visible = false;
                        litField.Text = "<br />";
                        litField.Visible = true;
                        break;

                    case "TEXT_CONTROL":
                    default:
                        hidField.Value = "TEXT";
                        txtField.Visible = true;
                        txtField.Text = actualValue;

                        if (isrequired) {
                            reqField.ControlToValidate = txtField.ID;
                            reqField.Enabled = true;
                            reqField.Visible = true;
                            lblRequired.Visible = true;
                        }
                        if (isreadonly) {
                            txtField.ReadOnly = true;
                            txtField.Enabled = false;
                        }

                        break;
                }

            }
        }

        private string getLookupText(string lookupDataview, int id) {
            var dt = _sd.GetData(lookupDataview, ":id=" + id + ";:txt=;", 0, 1).Tables[lookupDataview];
            if (dt.Rows.Count > 0) {
                return dt.Rows[0]["display_member"].ToString();
            } else {
                return "";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e) {

            using(var sd = UserManager.GetSecureData(false)){
                using (var dm = DataManager.Create(sd.DataConnectionSpec)){

                    var resultID = saveData(false);

                    // mark it as submitted
                    var dt = dm.Read("select feedback_result_group_id from feedback_result where feedback_result_id = :frid", new DataParameters(":frid", resultID, DbType.Int32));
                    if (dt.Rows.Count > 0) {
                        var dr = dt.Rows[0];

                        dm.Write(@"
update
    feedback_result_group
set
    modified_date = :now1,
    modified_by = :who1,
    submitted_date = :now2
where
    feedback_result_group_id = :frid
", new DataParameters(":now1", DateTime.UtcNow, DbType.DateTime2,
         ":who1", sd.CooperatorID, DbType.Int32,
         ":now2", DateTime.UtcNow, DbType.DateTime2,
         ":frid", Toolkit.ToInt32(dr["feedback_result_group_id"],-1), DbType.Int32));
                    }

                    // cancel any reminder emails
                    EmailQueue.DequeueEmail("feedback_result|%", resultID);


                }
            }

            Master.FlashMessage("Your data has been submitted for review.  Thank you!", "~/feedback/participantdefault.aspx", null);
        }

        protected void btnSaveForLater_Click(object sender, EventArgs e) {
            saveData(false);
            Master.FlashMessage("Your data has been saved.  Please remember to submit it at a later date!", "~/feedback/participantdefault.aspx", null);
        }

        protected void rptFields_ItemCommand(object source, RepeaterCommandEventArgs e) {
            if (e.CommandName == "lookup") {
                // show a popup with lookup of available items so they can pick one
                var pnl = e.Item.FindControl("pnlLookup") as Panel;
                ViewState["itemIndex"] = e.Item.ItemIndex;
                ViewState["itemLookup"] = e.CommandArgument;
                pnl.Visible = true;
            }
        }

        protected void btnLookup_Click(object sender, EventArgs e) {
            var itemIndex = Toolkit.ToInt32(ViewState["itemIndex"], -1);
            var row = rptFields.Items[itemIndex];
            var txt = ((TextBox)row.FindControl("txtLookup")).Text;
            var weblookup = ViewState["itemLookup"] as string;
            if (string.IsNullOrEmpty(weblookup)) {
                throw new InvalidOperationException("No lookup dataview found. Check ViewState['itemLookup'].");
            } else {
                var ddl = row.FindControl("ddlItems") as DropDownList;
                using (var sd = UserManager.GetSecureData(false)) {
                    var dt = sd.GetData(weblookup, ":id=;:txt=%" + txt + "%", 0, 100).Tables[weblookup];
                    ddl.DataSource = dt;
                    ddl.DataBind();
                }
            }
        }

        protected void btnLookupCancel_Click(object sender, EventArgs e) {
            var itemIndex = Toolkit.ToInt32(ViewState["itemIndex"], -1);
            var row = rptFields.Items[itemIndex];
            row.FindControl("pnlLookup").Visible = false;
        }

        protected void btnLookupSelected_Click(object sender, EventArgs e) {
            var itemIndex = Toolkit.ToInt32(ViewState["itemIndex"], -1);
            var row = rptFields.Items[itemIndex];
            var ddl = row.FindControl("ddlItems") as DropDownList;
            var itemID = Toolkit.ToInt32(ddl.SelectedValue, -1);
            var itemText = ddl.Items.FindByValue(itemID.ToString()).Text;

            var hidLookupID = row.FindControl("hidLookupID") as HiddenField;
            hidLookupID.Value = itemID.ToString();
            var txt = row.FindControl("txtField") as TextBox;
            txt.Text = itemText;

            var pnl = row.FindControl("pnlLookup") as Panel;
            pnl.Visible = false;
            ViewState["itemIndex"] = -1;
        }

        private int[] getResultAndInventoryIDs(bool usePreviousValueIfPossible) {
            string val = null;

            if (usePreviousValueIfPossible) {
                val = ViewState["previousResultAndInventory"] as string;
            }

            if (String.IsNullOrEmpty(val)) {
                val = ddlAccession.SelectedValue;
            }

            var resAndInv = val.Split('|');

            var rv = new int[2];

            rv[0] = Toolkit.ToInt32(resAndInv[0], -1);
            rv[1] = -1;

            if (resAndInv.Length > 1) {
                rv[1] = Toolkit.ToInt32(resAndInv[1], -1);
            }

            return rv;
        }

        private int saveData(bool usePreviousValueIfPossible) {

            var ri = getResultAndInventoryIDs(usePreviousValueIfPossible);
            var resultID = ri[0];
            var inventoryID = ri[1];

            using (var sd = UserManager.GetSecureData(false)) {

                var groupID = Toolkit.ToInt32(ViewState["groupID"], -1);
                var dtGroup = sd.GetData("web_feedback_result_group", ":feedbackresultgroupid=" + groupID, 0, 0).Tables["web_feedback_result_group"];
                if (dtGroup.Rows.Count > 0) {
                    var drGroup = dtGroup.Rows[0];
                    var date = Toolkit.ToDateTime(drGroup["started_date"], DateTime.MinValue);
                    if (date == DateTime.MinValue) {
                        drGroup["started_date"] = DateTime.UtcNow;
                        sd.SaveData(dtGroup.DataSet, true, null);
                    }
                }

                var dtField = sd.GetData("web_feedback_result_field", ":feedbackresultid=" + resultID, 0, 0).Tables["web_feedback_result_field"];

                // grab existing field data so we can stitch it together...
                // save field data
                foreach (RepeaterItem item in rptFields.Items) {
                    var txt = item.FindControl("txtField") as TextBox;
                    var ddl = item.FindControl("ddlField") as DropDownList;
                    var btn = item.FindControl("btnField") as Button;
                    var cal = item.FindControl("calField") as Calendar;
                    var chk = item.FindControl("chkField") as CheckBox;
                    var type = item.FindControl("hidFieldType") as HiddenField;
                    var hidLookup = item.FindControl("hidLookupID") as HiddenField;

                    var fieldFormID = Toolkit.ToInt32((item.FindControl("hidFieldFormID") as HiddenField).Value, -200000000);
                    var fieldResultID = Toolkit.ToInt32((item.FindControl("hidFieldResultID") as HiddenField).Value, -200000000);

                    string dataToSave = null;

                    if (type.Value.ToUpper() == "LITERAL") {
                        // nothing to do, this is a literal field (meaning it's for display purposes only)
                    } else {

                        switch (type.Value.ToUpper()) {
                            case "TEXT":
                                // use textbox data
                                dataToSave = txt.Text;
                                break;
                            case "LOOKUP":
                                // use hidden id from lookup result
                                dataToSave = hidLookup.Value;
                                break;
                            case "DROPDOWN":
                                // use dropdown data
                                dataToSave = ddl.SelectedValue;
                                break;
                            case "CALENDAR":
                                // use calendar data
                                dataToSave = cal.SelectedDate.ToString();
                                break;
                            case "BOOL":
                                dataToSave = chk.Checked ? "Y" : "N";
                                break;
                            case "":
                            default:
                                throw new NotImplementedException("missing type=" + type.Value.ToUpper());
                        }

                        var drs = dtField.Select("feedback_result_field_id = " + Math.Abs(fieldResultID));
                        if (drs.Length == 0) {
                            var dr = dtField.NewRow();
                            // HACK: we add - 1 billion here so we don't accidentally overlap with the bogus ids emitted from web_feedback_result_field dataview
                            // for records that do not have a valid feedback_result_field_id...
                            dr["feedback_result_field_id"] = -100000000 + dtField.Rows.Count;
                            dr["feedback_form_field_id"] = fieldFormID;
                            dr["feedback_result_id"] = resultID;
                            dr["fff_id"] = -1;
                            dr["started_date"] = DateTime.UtcNow;
                            dr["string_value"] = dataToSave;
                            dtField.Rows.Add(dr);
                        } else {
                            var dr = drs[0];
                            dr["feedback_form_field_id"] = fieldFormID;
                            dr["feedback_result_id"] = resultID;
                            dr["string_value"] = dataToSave;
                        }


                    }


                }

                sd.SaveData(dtField.DataSet, true, null);


                // save trait data

                var dtTrait = sd.GetData("web_feedback_result_trait_obs", ":feedbackresultid=" + resultID, 0, 0).Tables["web_feedback_result_trait_obs"];

                foreach (RepeaterItem item in rptTraits.Items) {
                    var txt = item.FindControl("txtTrait") as TextBox;
                    var ddl = item.FindControl("ddlTrait") as DropDownList;
                    var btn = item.FindControl("btnTrait") as Button;
                    var cal = item.FindControl("calTrait") as Calendar;
                    var type = item.FindControl("hidTraitType") as HiddenField;

                    var formTraitID = Toolkit.ToInt32((item.FindControl("hidFormTraitID") as HiddenField).Value, -200000000);
                    var cropTraitID = Toolkit.ToInt32((item.FindControl("hidCropTraitID") as HiddenField).Value, -300000000);
                    var traitResultID = Toolkit.ToInt32((item.FindControl("hidTraitResultID") as HiddenField).Value, -400000000);

                    string dataToSave = null;

                    switch (type.Value.ToUpper()) {
                        case "TEXT":
                            // use textbox data
                            dataToSave = txt.Text;
                            break;
                        case "DROPDOWN":
                            // use dropdown data
                            dataToSave = ddl.SelectedValue;
                            break;
                        case "CALENDAR":
                            // use calendar data
                            dataToSave = cal.SelectedDate.ToString();
                            break;
                        case "":
                        default:
                            throw new NotImplementedException("missing type=" + type.Value.ToUpper());
                    }

                    var drs = dtTrait.Select("feedback_result_trait_obs_id = " + Math.Abs(traitResultID));
                    if (drs.Length == 0) {
                        var dr = dtTrait.NewRow();
                        // HACK: we add -1 billion here so we don't accidentally overlap with the bogus ids emitted from web_feedback_result_field dataview
                        // for records that do not have a valid feedback_result_field_id...
                        dr["feedback_result_trait_obs_id"] = -100000000 + dtTrait.Rows.Count;
                        dr["crop_trait_id"] = cropTraitID;
                        dr["feedback_result_id"] = resultID;
                        dr["fft_id"] = -1;
                        dr["is_archived"] = "N";
                        dr["inventory_id"] = inventoryID;
                        dr["string_value"] = dataToSave;
                        dtTrait.Rows.Add(dr);
                    } else {
                        var dr = drs[0];
                        dr["crop_trait_id"] = cropTraitID;
                        dr["feedback_result_id"] = resultID;
                        dr["inventory_id"] = inventoryID;
                        dr["string_value"] = dataToSave;
                    }
                }

                sd.SaveData(dtTrait.DataSet, true, null);


            }

            return resultID;
        }

        protected void rptTraits_ItemDataBound(object sender, RepeaterItemEventArgs e) {
            if (e.Item.DataItem != null) {
                var dr = (e.Item.DataItem as DataRowView).Row;

                (e.Item.FindControl("hidTraitResultID") as HiddenField).Value = dr["feedback_result_trait_obs_id"].ToString();
                (e.Item.FindControl("hidFormTraitID") as HiddenField).Value = dr["fft_id"].ToString();
                (e.Item.FindControl("hidCropTraitID") as HiddenField).Value = dr["fft_crop_trait_id"].ToString();

                var title = dr["title"].ToString();
                var lblTitle = e.Item.FindControl("lblTitle") as Label;
                lblTitle.Text = title;

                var desc = dr["description"].ToString();
                var lblDesc = e.Item.FindControl("lblDescription") as Label;
                lblDesc.Text = desc;

                var txtTrait = e.Item.FindControl("txtTrait") as TextBox;
                var ddlTrait = e.Item.FindControl("ddlTrait") as DropDownList;

                var hidTraitType = e.Item.FindControl("hidTraitType") as HiddenField;

                var stringValue = dr["string_value"].ToString();
                var numericValue = Toolkit.ToDecimal(dr["numeric_value"], decimal.MinValue);
                var cropTraitCodeID = Toolkit.ToInt32(dr["crop_trait_code_id"], -1);
                var cropTraitID = Toolkit.ToInt32(dr["crop_trait_id"], -1);
                var isCoded = dr["is_coded"].ToString().ToUpper() == "Y";

                if (_hasBeenSubmitted) {
                    txtTrait.Enabled = false;
                    ddlTrait.Enabled = false;
                }


                txtTrait.Visible = false;
                ddlTrait.Visible = false;

                if (isCoded) {
                    // coded value, string/numeric value
                    ddlTrait.Visible = true;

                    var dtCropTraitCodes = _sd.GetData("web_list_crop_trait_code", ":croptraitid=" + cropTraitID + ";:croptraitcodeid=;", 0, 0).Tables["web_list_crop_trait_code"];

                    ddlTrait.DataValueField = "crop_trait_code_id";
                    ddlTrait.DataTextField = "title";
                    ddlTrait.DataSource = dtCropTraitCodes;
                    ddlTrait.DataBind();

                    hidTraitType.Value = "DROPDOWN";

                    ddlTrait.SelectedIndex = ddlTrait.Items.IndexOf(ddlTrait.Items.FindByValue(cropTraitCodeID.ToString()));

                } else {
                    // not a coded value.
                    txtTrait.Visible = true;
                    if (numericValue > decimal.MinValue) {
                        txtTrait.Text = numericValue.ToString();
                    } else {
                        txtTrait.Text = stringValue.ToString();
                    }

                    hidTraitType.Value = "TEXT";

                }


            }

        }

        protected void ddlAccession_SelectedIndexChanged(object sender, EventArgs e) {
            // save anything they might have changed (using the previous selectedvalue if possible)
            saveData(true);

            // remember the current selected value (in case they switch it later)
            ViewState["previousResultAndInventory"] = ddlAccession.SelectedValue;

            // bind to the proper accession now
            bindData();
        }

        protected void gvAttachments_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            var resAndInv = getResultAndInventoryIDs(false);
            var resultID = resAndInv[0];
            var attachID = Toolkit.ToInt32(gvAttachments.DataKeys[e.RowIndex][0], -1);

            using (var sd = UserManager.GetSecureData(false)) {

                var dt = sd.GetData("web_feedback_result_attachments", ":feedbackresultid=" + resultID + ";:feedbackresultattachid=" + attachID, 0, 0).Tables["web_feedback_result_attachments"];
                if (dt.Rows.Count > 0) {
                    var dr = dt.Rows[0];
                    var vpath = dr["virtual_path"].ToString();
                    var path = Toolkit.ResolveFilePath(vpath, false);
                    if (File.Exists(path)) {
                        File.Delete(path);
                    }
                    dt.Rows[0].Delete();
                    sd.SaveData(dt.DataSet, true, null);
                }

                Master.ShowMessage("Attachment deleted.");
                bindAttachments(resultID, null, sd);
            
            }


        }

        protected void lnkAddAttachment_Click(object sender, EventArgs e) {

        }

        protected void btnUploadAttachment_Click(object sender, EventArgs e) {
            var resAndInv = getResultAndInventoryIDs(false);
            var resultID = resAndInv[0];

            var filename = Request.Files[0].FileName;

            if (String.IsNullOrEmpty(filename)) {
                Master.ShowError("No file was given.");
            } else {

                var vpath = "~/uploads/feedback/resultattachments/" + resultID + "/" + filename;
                var path = Toolkit.ResolveFilePath(vpath, true);
                Request.Files[0].SaveAs(path);

                var title = txtAttachmentTitle.Text;
                if (String.IsNullOrEmpty(title)) {
                    title = filename;
                }

                using (var sd = UserManager.GetSecureData(false)) {

                    var dt = sd.GetData("web_feedback_result_attachments", ":feedbackresultid=" + resultID + ";:feedbackresultattachid=;", 0, 0).Tables["web_feedback_result_attachments"];
                    var dr = dt.NewRow();
                    dr["feedback_result_attach_id"] = -1;
                    dr["feedback_result_id"] = resultID;
                    dr["virtual_path"] = vpath;
                    dr["content_type"] = Request.Files[0].ContentType;
                    dr["title"] = title;
                    dr["is_web_visible"] = "Y";
                    dt.Rows.Add(dr);

                    sd.SaveData(dt.DataSet, true, null);

                    bindAttachments(resultID, null, sd);
                    Master.ShowMessage("Attachment saved.");

                }
            }


        }

        protected void btnCancelAttachment_Click(object sender, EventArgs e) {

        }
    }
}
