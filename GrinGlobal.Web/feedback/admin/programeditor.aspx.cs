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
    public partial class newprogram : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                var feedbackID = Toolkit.ToInt32(Request["id"], -1);
                ViewState["feedbackID"] = feedbackID;
                bindFeedback(feedbackID);
                bindReports(feedbackID);
                if (feedbackID < 0) {
                    hlNewReport.NavigateUrl = "javascript:alert('You must save the program first');";
                } else {
                    hlNewReport.NavigateUrl = "~/feedback/admin/reporteditor.aspx?id=-1&programid=" + feedbackID;
                }
            }
        }


        private DataTable getDataviewData(string dvName, int feedbackID, int limit) {
            using (var sd = new SecureData(false, UserManager.GetLoginToken(true))) {
                return sd.GetData(dvName, ":feedbackreportid=;:feedbackattachid=;:feedbackinventoryid=;:cooperatorid=" + sd.CooperatorID + ";:feedbackid=" + feedbackID, 0, limit).Tables[dvName];
            }
        }

        private void bindReports(int id) {
            var dt = getDataviewData("web_feedback_program_reports", id, 0);
            gvReports.DataSource = dt;
            gvReports.DataBind();
            if (dt.Rows.Count > 0) {
                gvReports.Rows[dt.Rows.Count - 1].FindControl("btnDown").Visible = false;
            }
        }

        private void bindFeedback(int id) {

            gvInventorySearch.DataSource = null;
            gvInventorySearch.DataBind();


            var dt = getDataviewData("web_feedback_program_details", id, 0);
            if (dt.Rows.Count > 0) {
                var dr = dt.Rows[0];

                txtTitle.Text = dr["title"].ToString();
                litHeading.Text = "Manage " + dr["title"].ToString();
                txtStartDate.Text = Toolkit.ToDateTime(dr["start_date"], DateTime.UtcNow).ToString("MM/dd/yyyy");
                txtEndDate.Text = Toolkit.ToDateTime(dr["end_date"], DateTime.UtcNow).ToString("MM/dd/yyyy");
                //calStartDate.SelectedDate = Toolkit.ToDateTime(dr["start_date"], DateTime.UtcNow);
                //calEndDate.SelectedDate = Toolkit.ToDateTime(dr["end_date"], DateTime.UtcNow);
                chkByInventory.Checked = dr["is_restricted_by_inventory"].ToString().ToUpper() == "Y";

                gvInventory.DataSource = getDataviewData("web_feedback_inventory", id, 0);
                gvInventory.DataBind();

            } else {
                gvInventory.DataSource = null;
                gvInventory.DataBind();
            }
        }

        protected void chkByInventory_CheckedChanged(object sender, EventArgs e) {
        }

        protected void btnInventorySearch_Click(object sender, EventArgs e) {
            using (var sd = UserManager.GetSecureData(false)) {

                var dt = sd.GetData("web_feedback_program_inventory_search", ":inventorynumber=%" + txtInventory.Text + "%", 0, 50).Tables["web_feedback_program_inventory_search"];

                gvInventorySearch.DataSource = dt;
                gvInventorySearch.DataBind();
            }
        }

        protected void lnkAddInventory_Click(object sender, EventArgs e) {
            pnlChooseInventory.Visible = true;
        }

        protected void gvInventorySearch_SelectedIndexChanging(object sender, GridViewSelectEventArgs e) {

            var feedbackID = Toolkit.ToInt32(ViewState["feedbackID"], -1);
            var inventoryID = Toolkit.ToInt32(gvInventorySearch.DataKeys[e.NewSelectedIndex][0], -1);

            using (var sd = UserManager.GetSecureData(false)) {
                var dt = sd.GetData("web_feedback_inventory", ":feedbackinventoryid=;:feedbackid=" + feedbackID, 0, 0).Tables["web_feedback_inventory"];
                var drs = dt.Select("inventory_id = " + inventoryID);
                if (drs.Length == 0){
                    var dr = dt.NewRow();
                    dr["feedback_inventory_id"] = -1;
                    dr["feedback_id"] = feedbackID;
                    dr["inventory_id"] = inventoryID;
                    dt.Rows.Add(dr);

                    sd.SaveData(dt.DataSet, true, null);


                    // also make sure the 'active for inventory lots' is saved as true
                    var dtProgram = getDataviewData("web_feedback_program_details", feedbackID, 0);
                    if (dtProgram.Rows.Count > 0) {
                        dtProgram.Rows[0]["is_restricted_by_inventory"] = "Y";
                        sd.SaveData(dtProgram.DataSet, true, null);
                    }



                }
            }

            pnlChooseInventory.Visible = false;

            bindFeedback(feedbackID);

            Master.ShowMessage("Inventory lot added.");

        }

        protected void gvInventory_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            var feedbackID = Toolkit.ToInt32(ViewState["feedbackID"], -1);
            var feedbackInventoryID = Toolkit.ToInt32(gvInventory.DataKeys[e.RowIndex][0], -1);

            using (var sd = UserManager.GetSecureData(false)) {
                var dt = sd.GetData("web_feedback_inventory", ":feedbackinventoryid=" + feedbackInventoryID + ";:feedbackid=" + feedbackID, 0, 0).Tables["web_feedback_inventory"];
                if (dt.Rows.Count > 0){
                    dt.Rows[0].Delete();
                    sd.SaveData(dt.DataSet, true, null);
                }
            }

            bindFeedback(feedbackID);
            Master.ShowMessage("Inventory lot removed.");

        }

        protected void btnInventorySearchCancel_Click(object sender, EventArgs e) {
            pnlChooseInventory.Visible = false;
            gvInventorySearch.DataSource = null;
            gvInventorySearch.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e) {
            var feedbackID = Toolkit.ToInt32(ViewState["feedbackID"], -1);
            using (var sd = UserManager.GetSecureData(false)) {
                var dt = sd.GetData("web_feedback_program_details", ":feedbackid=" + feedbackID, 0, 0).Tables["web_feedback_program_details"];
                if (dt.Rows.Count == 0) {
                    var dr = dt.NewRow();
                    dr["feedback_id"] = -1;
                    dr["title"] = txtTitle.Text;
                    dr["start_date"] = Toolkit.ToDateTime(txtStartDate.Text, DateTime.UtcNow);
                    dr["end_date"] = Toolkit.ToDateTime(txtEndDate.Text, DateTime.UtcNow);
                    dr["is_restricted_by_inventory"] = chkByInventory.Checked ? "Y" : "N";
                    dt.Rows.Add(dr);
                } else {
                    var dr = dt.Rows[0];
                    dr["title"] = txtTitle.Text;
                    dr["start_date"] = Toolkit.ToDateTime(txtStartDate.Text, DateTime.UtcNow);
                    dr["end_date"] = Toolkit.ToDateTime(txtEndDate.Text, DateTime.UtcNow);
                    dr["is_restricted_by_inventory"] = chkByInventory.Checked ? "Y" : "N";
                }

                sd.SaveData(dt.DataSet, true, null);
            }

            Master.FlashMessage("Program changes saved.", "~/feedback/admin/manageprograms.aspx", null);

        }

        protected void gvReports_RowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName == "UP") {
                var feedbackID = Toolkit.ToInt32(ViewState["feedbackID"], -1);
                var reportID = Toolkit.ToInt32(e.CommandArgument, -1);

                using (var sd = UserManager.GetSecureData(false)) {
                    // grab all fields
                    var dt = sd.GetData("web_feedback_program_reports", ":feedbackid=" + feedbackID, 0, 0).Tables["web_feedback_program_reports"];
                    for (var i = 0; i < dt.Rows.Count; i++) {
                        var dr = dt.Rows[i];
                        dr["sort_order"] = i;
                        if (Toolkit.ToInt32(dr["feedback_report_id"], -2) == reportID) {
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

                bindReports(feedbackID);


            } else if (e.CommandName == "DOWN") {
                var feedbackID = Toolkit.ToInt32(ViewState["feedbackID"], -1);
                var reportID = Toolkit.ToInt32(e.CommandArgument, -1);

                using (var sd = UserManager.GetSecureData(false)) {
                    // grab all fields
                    var dt = sd.GetData("web_feedback_program_reports", ":feedbackid=" + feedbackID, 0, 0).Tables["web_feedback_program_reports"];
                    for (var i = 0; i < dt.Rows.Count; i++) {
                        var dr = dt.Rows[i];
                        dr["sort_order"] = i;
                        if (Toolkit.ToInt32(dr["feedback_report_id"], -2) == reportID) {
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

                bindReports(feedbackID);
            }

        }

        protected void gvReports_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row != null && e.Row.RowIndex > -1) {
                if (e.Row.RowIndex == 0) {
                    var btnUp = e.Row.FindControl("btnUp");
                    btnUp.Visible = false;
                }
            }
        }
    }
}
