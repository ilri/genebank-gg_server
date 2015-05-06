using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using GrinGlobal.Business;
using System.Data;
using System.IO;

namespace GrinGlobal.Web.feedback.admin
{
    public partial class reportnew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                bindDropDowns();
                var id = Toolkit.ToInt32(Request["id"], -1);
                ViewState["reportID"] = id;
                var programID = Toolkit.ToInt32(Request["programid"], -1);
                if (programID < 0) {
                    Response.Redirect("~/feedback/admin/manageprograms.aspx");
                } else {
                    ViewState["programID"] = programID;
                    bindData(id, true);
                }
            }
        }

        private void bindDropDowns() { 
            using (var sd = UserManager.GetSecureData(false)){
                Master.FillCodeGroupDropDown(ddlInterval, "INTERVAL_LENGTH", sd);
                Master.FillCodeGroupDropDown(ddlStartDate, "INTERVAL_TYPE", sd);
            }
        }

        private DataTable getDataviewData(string dvName, int reportID, int limit) {
            using (var sd = new SecureData(false, UserManager.GetLoginToken(true))) {
                return sd.GetData(dvName, ":feedbackreportid=" + reportID + ";:feedbackreportattachid=;:feedbackinventoryid=;:cooperatorid=" + sd.CooperatorID, 0, limit).Tables[dvName];
            }
        }

        private void bindAttachments(int id) {
            var dt = getDataviewData("web_feedback_report_attachments", id, 0);
            gvAttachments.DataSource = dt;
            gvAttachments.DataBind();
        }


        private void bindData(int id, bool fillFormList) {

            using (var sd = new SecureData(false, UserManager.GetLoginToken(true))) {

                bindAttachments(id);

                if (fillFormList) {
                    var dtForms = sd.GetData("web_feedback_form", ":feedbackformid=;", 0, 0).Tables["web_feedback_form"];
                    ddlForms.DataSource = dtForms;
                    ddlForms.DataBind();
                }

                var dt = sd.GetData("web_feedback_reports", ":cooperatorid=" + sd.CooperatorID + ";:feedbackid=;:feedbackreportid=" + id, 0, 0).Tables["web_feedback_reports"];

                if (dt.Rows.Count > 0) {
                    var dr = dt.Rows[0];
                    txtTitle.Text = dr["title"].ToString();
                    litHeading.Text = "Manage " + dr["title"].ToString();
                    ddlDue.SelectedIndex = ddlDue.Items.IndexOf(ddlDue.Items.FindByValue(dr["due_interval"].ToString()));
                    ddlInterval.SelectedIndex = ddlInterval.Items.IndexOf(ddlInterval.Items.FindByValue(dr["interval_length_code"].ToString()));
                    ddlStartDate.SelectedIndex = ddlStartDate.Items.IndexOf(ddlStartDate.Items.FindByValue(dr["interval_type_code"].ToString()));

                    txtCustomDate.Text = Toolkit.ToDateTime(dr["custom_due_date"], DateTime.UtcNow).ToString("MM/dd/yyyy");

                    if (ddlStartDate.SelectedValue.ToString() == "CUSTOMDATE") {
                        txtCustomDate.Visible = true;
                    } else {
                        txtCustomDate.Visible = false;
                    }


                    chkIsObservationData.Checked = dr["is_observation_data"].ToString().ToUpper() == "Y";
                    ddlYesNo.SelectedIndex = dr["is_web_visible"].ToString().ToUpper() == "Y" ? 0 : 1;
                    ddlForms.SelectedIndex = ddlForms.Items.IndexOf(ddlForms.Items.FindByValue(dr["feedback_form_id"].ToString()));

                    chkSendInitialEmail.Checked = dr["is_notified_initially"].ToString().ToUpper() == "Y";
                    txtInitialEmail.Text = dr["initial_email_text"].ToString();
                    txtInitialEmailSubject.Text = dr["initial_email_subject"].ToString();

                    chkSend30DayNotice.Checked = dr["is_notified_30days_prior"].ToString().ToUpper() == "Y";
                    txt30DayEmail.Text = dr["prior30_email_text"].ToString();
                    txt30DayEmailSubject.Text = dr["prior30_email_subject"].ToString();

                    chkSend15DayNotice.Checked = dr["is_notified_15days_prior"].ToString().ToUpper() == "Y";
                    txt15DayEmail.Text = dr["prior15_email_text"].ToString();
                    txt15DayEmailSubject.Text = dr["prior15_email_subject"].ToString();



                } else {
                    ddlYesNo.SelectedIndex = 0;
                    ddlForms.SelectedIndex = 0;
                }

            }

        }

        private DataTable getDataViewDataForCooperator(string dvName, int reportID, int limit) {
            using (var sd = new SecureData(false, UserManager.GetLoginToken(true))) {
                return sd.GetData(dvName, ":cooperatorid=" + sd.CooperatorID + ";:feedbackreportid=" + reportID, 0, limit).Tables[dvName];
            }
        }

        protected void btnSave_Click(object sender, EventArgs e) {
            using (var sd = UserManager.GetSecureData(false)) {
                var id = (int)ViewState["reportID"];
                var programID = (int)ViewState["programID"];
                var dt = sd.GetData("web_feedback_reports", ":cooperatorid=" + sd.CooperatorID + ";:feedbackreportid=;:feedbackid=" + programID, 0, 0).Tables["web_feedback_reports"];

                var drs = dt.Select("feedback_report_id = " + id);

                if (drs.Length == 0) {
                    var dr = dt.NewRow();
                    dr["feedback_id"] = programID;
                    dr["feedback_report_id"] = -1;
                    dr["feedback_form_id"] = Toolkit.Coalesce(Toolkit.ToInt32(ddlForms.SelectedValue, null), DBNull.Value);
                    dr["title"] = txtTitle.Text;
                    dr["is_observation_data"] = chkIsObservationData.Checked ? "Y" : "N";
                    dr["is_web_visible"] = ddlYesNo.SelectedValue;
                    dr["due_interval"] = Toolkit.ToInt32(ddlDue.SelectedValue, 0);
                    dr["interval_length_code"] = ddlInterval.SelectedValue;
                    dr["interval_type_code"] = ddlStartDate.SelectedValue;
                    if (ddlStartDate.SelectedValue.ToString() == "CUSTOMDATE") {
                        dr["custom_due_date"] = Toolkit.Coalesce(Toolkit.ToDateTime(txtCustomDate.Text, null), DBNull.Value);
                    } else {
                        dr["custom_due_date"] = DBNull.Value;
                    }
                    dr["is_notified_initially"] = chkSendInitialEmail.Checked ? "Y" : "N";
                    dr["is_notified_30days_prior"] = chkSend30DayNotice.Checked ? "Y" : "N";
                    dr["is_notified_15days_prior"] = chkSend15DayNotice.Checked ? "Y" : "N";
                    dr["initial_email_text"] = txtInitialEmail.Text;
                    dr["prior15_email_text"] = txt15DayEmail.Text;
                    dr["prior30_email_text"] = txt30DayEmail.Text;
                    dr["initial_email_subject"] = txtInitialEmailSubject.Text;
                    dr["prior15_email_subject"] = txt15DayEmailSubject.Text;
                    dr["prior30_email_subject"] = txt30DayEmailSubject.Text;

                    if (dt.Rows.Count == 0) {
                        dr["sort_order"] = Toolkit.ToInt32(dt.Rows[dt.Rows.Count-1]["sort_order"], 0);
                    } else {
                        dr["sort_order"] = 0;
                    }
                    dt.Rows.Add(dr);
                    sd.SaveData(dt.DataSet, true, null);
                } else {
                    var dr = drs[0];
                    dr["feedback_id"] = programID;
                    dr["feedback_form_id"] = Toolkit.Coalesce(Toolkit.ToInt32(ddlForms.SelectedValue, null), DBNull.Value);
                    dr["title"] = txtTitle.Text;
                    dr["is_observation_data"] = chkIsObservationData.Checked ? "Y" : "N";
                    dr["is_web_visible"] = ddlYesNo.SelectedValue;
                    dr["due_interval"] = Toolkit.ToInt32(ddlDue.SelectedValue, 0);
                    dr["interval_length_code"] = ddlInterval.SelectedValue;
                    dr["interval_type_code"] = ddlStartDate.SelectedValue;
                    if (ddlStartDate.SelectedValue.ToString() == "CUSTOMDATE") {
                        dr["custom_due_date"] = Toolkit.ToDateTime(txtCustomDate.Text, null);
                    } else {
                        dr["custom_due_date"] = DBNull.Value;
                    }

                    dr["is_notified_initially"] = chkSendInitialEmail.Checked ? "Y" : "N";
                    dr["is_notified_30days_prior"] = chkSend30DayNotice.Checked ? "Y" : "N";
                    dr["is_notified_15days_prior"] = chkSend15DayNotice.Checked ? "Y" : "N";
                    dr["initial_email_text"] = txtInitialEmail.Text;
                    dr["prior15_email_text"] = txt15DayEmail.Text;
                    dr["prior30_email_text"] = txt30DayEmail.Text;
                    dr["initial_email_subject"] = txtInitialEmailSubject.Text;
                    dr["prior15_email_subject"] = txt15DayEmailSubject.Text;
                    dr["prior30_email_subject"] = txt30DayEmailSubject.Text;

                    sd.SaveData(dt.DataSet, true, null);
                }
                Master.ShowMessage("Your changes have been saved.");
                bindData(id, false);
            }
        }

        protected void lnkEditForm_Click(object sender, EventArgs e) {
            Response.Redirect("~/feedback/admin/formeditor.aspx?id=" + ddlForms.SelectedValue + "&reportid=" + ViewState["reportID"] + "&programid=" + ViewState["programID"]);
        }

        protected void lnkAddNewForm_Click(object sender, EventArgs e) {
            Response.Redirect("~/feedback/admin/formeditor.aspx?id=-1&reportid=" + ViewState["reportID"] + "&programid=" + ViewState["programID"]);
        }

        protected void gvAttachments_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            var reportID = Toolkit.ToInt32(ViewState["reportID"], -1);
            var attachID = Toolkit.ToInt32(gvAttachments.DataKeys[e.RowIndex][0], -1);

            using (var sd = UserManager.GetSecureData(false)) {

                var dt = sd.GetData("web_feedback_report_attachments", ":feedbackreportid=" + reportID + ";:feedbackreportattachid=" + attachID, 0, 0).Tables["web_feedback_report_attachments"];
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
            }

            Master.ShowMessage("Attachment deleted.");
            bindAttachments(reportID);

        }

        protected void lnkAddAttachment_Click(object sender, EventArgs e) {
        }

        protected void btnUploadAttachment_Click(object sender, EventArgs e) {
            var reportID = Toolkit.ToInt32(ViewState["reportID"], -1);

            var filename = Request.Files[0].FileName;

            if (String.IsNullOrEmpty(filename)) {
                Master.ShowError("No file was given.");
            } else {
                var vpath = "~/uploads/feedback/reportattachments/" + reportID + "/" + filename;
                var path = Toolkit.ResolveFilePath(vpath, true);
                Request.Files[0].SaveAs(path);

                var title = txtAttachmentTitle.Text;
                if (String.IsNullOrEmpty(title)) {
                    title = filename;
                }

                using (var sd = UserManager.GetSecureData(false)) {

                    var dt = sd.GetData("web_feedback_report_attachments", ":feedbackreportid=" + reportID + ";:feedbackreportattachid=;", 0, 0).Tables["web_feedback_report_attachments"];
                    var dr = dt.NewRow();
                    dr["feedback_report_attach_id"] = -1;
                    dr["feedback_report_id"] = reportID;
                    dr["virtual_path"] = vpath;
                    dr["content_type"] = Request.Files[0].ContentType;
                    dr["title"] = title;
                    dr["is_web_visible"] = "Y";
                    dt.Rows.Add(dr);

                    sd.SaveData(dt.DataSet, true, null);
                }
                bindAttachments(reportID);
                Master.ShowMessage("Attachment saved.");
            }

        }

        protected void btnCancelAttachment_Click(object sender, EventArgs e) {

        }

        protected void ddlStartDate_SelectedIndexChanged(object sender, EventArgs e) {
            var sel = ddlStartDate.SelectedValue as string;
            txtCustomDate.Visible = sel == "CUSTOMDATE";
        }

    }
}
