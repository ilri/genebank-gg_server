using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using System.Data;
using GrinGlobal.Business;

namespace GrinGlobal.Web.feedback.admin
{
    public partial class manageprograms : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                bindData();
            }
        }

        private void bindData() {
            using (var sd = UserManager.GetSecureData(false)) {

                gvPrograms.DataSource = sd.GetData("web_feedback_admin_programs", ":feedbackid=;", 0, 0).Tables["web_feedback_admin_programs"];
                gvPrograms.DataBind();

                gvSubmissions.DataSource = sd.GetData("web_feedback_admin_pending_submissions", "", 0, 0).Tables["web_feedback_admin_pending_submissions"];
                gvSubmissions.DataBind();

                gvOrders.DataSource = sd.GetData("web_feedback_admin_pending_orders", ":feedbackid=;:inventoryid=;:orderrequestitemid=;:orderrequestid=;", 0, 0).Tables["web_feedback_admin_pending_orders"];
                gvOrders.DataBind();

            }
        }

        protected void gvSubmissions_SelectedIndexChanged(object sender, EventArgs e) {
            var groupNumber = Toolkit.ToInt32(gvSubmissions.SelectedDataKey[0], -1);
            Response.Redirect("~/feedback/participantresult.aspx?groupid=" + groupNumber);
            Response.End();

        }

        protected void gvOrders_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row != null) {
                var chk = e.Row.FindControl("chkInclude") as CheckBox;
            }
        }

        protected void btnIncludeInProgram_Click(object sender, EventArgs e) {

            using (var sd = UserManager.GetSecureData(false)) {
                var dtGroupToSave = sd.GetData("web_feedback_result_group", ":feedbackresultgroupid=-1;", 0, 0).Tables["web_feedback_result_group"];

                var needToSave = false;

                var i = -1;

                foreach (GridViewRow gvr in gvOrders.Rows) {
                    var chk = gvr.FindControl("chkInclude") as CheckBox;
                    if (chk.Checked) {
                        var feedbackID = Toolkit.ToInt32(gvOrders.DataKeys[gvr.RowIndex][0], -1);
                        var orderRequestID = Toolkit.ToInt32(gvOrders.DataKeys[gvr.RowIndex][1], -1);
                        var cooperatorID = Toolkit.ToInt32(gvOrders.DataKeys[gvr.RowIndex][2], -1);

                        // get all reports for the current program
                        var dtToWrite = sd.GetData("web_feedback_admin_write_result_group", ":feedbackid=" + feedbackID + ";:orderrequestid=" + orderRequestID + ";:cooperatorid=" + cooperatorID, 0, 0).Tables["web_feedback_admin_write_result_group"];

                        foreach (DataRow drWrite in dtToWrite.Rows) {

                            var reportID = Toolkit.ToInt32(drWrite["feedback_report_id"], -1);

                            var drOutput = dtGroupToSave.NewRow();
                            drOutput["feedback_result_group_id"] = i--;
                            drOutput["feedback_report_id"] = reportID;
                            drOutput["feedback_id"] = feedbackID;
                            drOutput["participant_cooperator_id"] = cooperatorID;
                            drOutput["order_request_id"] = orderRequestID;


                            // TODO: write due_date properly.. ???
                            var dueInterval = Toolkit.ToInt32(drWrite["due_interval"], 1);
                            var intervalType = drWrite["interval_type_code"].ToString();
                            var intervalLength = drWrite["interval_length_code"].ToString();

                            DateTime fromDate = DateTime.UtcNow;
                            switch (intervalType.ToUpper()) {
                                case "CUSTOMDATE":
                                    fromDate = Toolkit.ToDateTime(drWrite["custom_due_date"], fromDate);
                                    break;
                                case "INITIALPLANTDATE":
                                    // TODO: wtf?
                                    break;
                                case "ORDERDATE":
                                    fromDate = Toolkit.ToDateTime(drWrite["ordered_date"], fromDate);
                                    break;
                                case "TRIALSTARTDATE":
                                    fromDate = Toolkit.ToDateTime(drWrite["start_date"], fromDate);
                                    break;
                                case "":
                                default:
                                    break;
                            }
                            // drOutput["due_date"] = 

                            DateTime dueDate = fromDate;

                            switch (intervalLength.ToUpper()) {
                                case "DAYS":
                                    dueDate = fromDate.AddDays(dueInterval);
                                    break;
                                case "WEEKS":
                                    dueDate = fromDate.AddDays(dueInterval * 7);
                                    break;
                                case "MONTHS":
                                    dueDate = fromDate.AddMonths(dueInterval);
                                    break;
                                case "YEARS":
                                    dueDate = fromDate.AddYears(dueInterval);
                                    break;
                                default:
                                    // TODO: wtf?
                                    break;
                            }

                            drOutput["due_date"] = dueDate;


                            dtGroupToSave.Rows.Add(drOutput);
                            needToSave = true;
                        }
                    }
                }


                if (needToSave) {
                    // save the group-level data
                    var saved = sd.SaveData(dtGroupToSave.DataSet, true, null).Tables["web_feedback_result_group"];


                    // now that we've saved it (and have proper result group id's), we need to send/queue up email as needed
                    // now, we know the due date and if we should send email.
                    // queue that stuff up as needed.
                    foreach (DataRow drSaved in saved.Rows) {
                        var reportID = Toolkit.ToInt32(drSaved["feedback_report_id"], -1);
                        var resultGroupID = Toolkit.ToInt32(drSaved["NewPrimaryKeyID"], -1);

                        // need to re-pull the data so we get all fields filled in. should always return exactly 1 row.
                        var dtResultGroup = sd.GetData("web_feedback_result_group", ":feedbackresultgroupid=" + resultGroupID, 0, 0).Tables["web_feedback_result_group"];
                        foreach (DataRow drResultGroup in dtResultGroup.Rows) {
                            var dueDate = Toolkit.ToDateTime(drResultGroup["due_date"], DateTime.UtcNow);
                            var fromEmail = Toolkit.GetSetting("EmailFrom", "noreply@grin-global.org");
                            var toEmail = sd.WebUserName;

                            if (drResultGroup["is_notified_initially"].ToString().ToUpper() == "Y") {
                                var subject = "initial email";
                                var body = drSaved["initial_email_text"].ToString();

                                var attachPaths = new List<string>();
                                var dtAttach = sd.GetData("web_feedback_report_attachments", ":feedbackreportid=" + reportID + ";:feedbackreportattachid=;", 0, 0).Tables["web_feedback_report_attachments"];
                                foreach (DataRow drAttach in dtAttach.Rows) {
                                    attachPaths.Add(drAttach["virtual_path"].ToString());
                                }

                                EmailQueue.QueueEmail(fromEmail, toEmail, null, null, null, subject, body, true, DateTime.UtcNow, attachPaths.ToArray(), sd.CooperatorID, "feedback_result_group|initial", resultGroupID, false);
                            }


                            if (drResultGroup["is_notified_30days_prior"].ToString().ToUpper() == "Y") {
                                var subject = "30 days prior email";
                                var body = drResultGroup["prior30_email_text"].ToString();

                                EmailQueue.QueueEmail(fromEmail, toEmail, null, null, null, subject, body, true, dueDate.AddDays(-30), null, sd.CooperatorID, "feedback_result_group|prior30", resultGroupID, false);
                            }

                            if (drResultGroup["is_notified_15days_prior"].ToString().ToUpper() == "Y") {
                                var subject = "15 days prior email";
                                var body = drResultGroup["prior15_email_text"].ToString();

                                EmailQueue.QueueEmail(fromEmail, toEmail, null, null, null, subject, body, true, dueDate.AddDays(-15), null, sd.CooperatorID, "feedback_result_group|prior15", resultGroupID, false);
                            }

                            EmailQueue.SendQueuedEmailNow();

                        }
                    }





                    // get an empty DataTable to add the new result-level data to
                    var dtResultToSave = sd.GetData("web_feedback_result", ":feedbackresultid=;", 0, 0).Tables["web_feedback_result"];

                    // ok, we created the feedback_result_group record(s), one for each report within the program.
                    // now using the new feedback_result_group_id's, create the feedback_result records to go with each one
                    i = -10000;
                    foreach(DataRow drSaved in saved.Rows){
                        var feedbackID = drSaved["feedback_id"];
                        var orderRequestID = drSaved["order_request_id"];
                        var cooperatorID = drSaved["participant_cooperator_id"];
                        var reportID = drSaved["feedback_report_id"];

                        // get the inventory_id's we're supposed to add to the feedback_result table
                        var dtToWrite = sd.GetData("web_feedback_admin_write_result", ":feedbackid=" + feedbackID + ";:orderrequestid=" + orderRequestID + ";:cooperatorid=" + cooperatorID + ";:feedbackreportid=" + reportID, 0, 0).Tables["web_feedback_admin_write_result"];
                        foreach (DataRow drToWrite in dtToWrite.Rows) {
                            var drRes = dtResultToSave.NewRow();
                            drRes["feedback_result_id"] = i--;
                            drRes["feedback_result_group_id"] = drSaved["NewPrimarykeyID"];
                            drRes["inventory_id"] = drToWrite["inventory_id"];
                            // TODO: write due_date properly.. ???
                            // drOutput["due_date"] = 
                            dtResultToSave.Rows.Add(drRes);
                        }
                    }

                    // write to the feedback_result table
                    sd.SaveData(dtResultToSave.DataSet, true, null);

                }
                bindData();

            }
        }
    }
}
