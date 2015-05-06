using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using System.Data;
using System.Text;
using GrinGlobal.Business;
using System.Web.UI.HtmlControls;

namespace GrinGlobal.Web.feedback
{
    public partial class participantaccession : System.Web.UI.Page
    {

        string _action = string.Empty;
        string _level = string.Empty;
        static string _prevPage = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            var orderID = Toolkit.ToInt32(Request["id"], -1);
            bindData(orderID);

            if (Request.QueryString["action"] != null)
            {
                _action = Request.QueryString["action"];
            }
            if (Request.QueryString["level"] != null)
            {
                _level = Request.QueryString["level"];
            }

        }

        private void bindData(int accessionID)
        {
            // when there's a bunch of different things to databind,
            // it's usually clearer to break them out.
            // for the mockup, let's just leave everything as DataManager.ExecRead
            
            bindParticipantAccessions(accessionID);

            bindAccessionSummary(accessionID);

            bindAttachments(accessionID);

            bindParticipantReports(accessionID);
        }

        private void bindAccessionSummary(int id) {
            // pull data to fill the box
            var dt = getDataViewDataForCooperator("web_feedback_participantaccession_summary", id, 1);
            rptBox.DataSource = dt;
            rptBox.DataBind();
            if (dt.Rows.Count > 0) {
                lblAccessionNumber.Text = dt.Rows[0]["pi_number"].ToString();
            }
        }

        public string UpcaseFirstLetter(object val) {
            string ret = val as string;
            if (!String.IsNullOrEmpty(ret)) {
                ret = ret.ToLower();
                ret = ret.Substring(0, 1).ToUpper() + ret.Substring(1, ret.Length - 1);
            }
            return ret;
        }


        private DataTable getDataViewDataForCooperator(string dvName, int accessionID, int limit)
        {

            using (var sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                return sd.GetData(dvName, ":cooperatorid=" + sd.CooperatorID + ";:accessionid=" + accessionID, 0, limit).Tables[dvName];
            }
        }

        private void bindParticipantAccessions(int accessionID)
        {
            var dt = getDataViewDataForCooperator("web_feedback_participantaccession_orders", accessionID, 0);
            gvOrders.DataSource = dt;
            gvOrders.DataBind();
        }

        private void bindAttachments(int accessionID) {
            var dt = getDataViewDataForCooperator("web_feedback_participantaccession_attachments", accessionID, 0);
            rptAttachments.DataSource = dt;
            rptAttachments.DataBind();
        }

        private void bindParticipantReports(int accessionID)
        {
            var dt = getDataViewDataForCooperator("web_feedback_participantaccession_reports", accessionID, 0);
            gvReports.DataSource = dt;
            gvReports.DataBind();
        }
    }
}
