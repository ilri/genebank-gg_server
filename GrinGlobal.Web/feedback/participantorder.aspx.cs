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
    public partial class participantorder : System.Web.UI.Page
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

        private void bindData(int orderID)
        {
            // when there's a bunch of different things to databind,
            // it's usually clearer to break them out.
            // for the mockup, let's just leave everything as DataManager.ExecRead
            bindParticipantAccessions(orderID);

            bindParticipantReports(orderID);

            bindHeader(orderID);

        }

        private DataTable getDataViewDataForCooperator(string dvName, int orderID, int limit)
        {

            using (var sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                return sd.GetData(dvName, ":cooperatorid=" + sd.CooperatorID + ";:order_request_id=" + orderID, 0, limit).Tables[dvName];
            }
        }

        private void bindParticipantAccessions(int orderID)
        {
            // TODO: make dataview for this guy...
            var dt = getDataViewDataForCooperator("web_feedback_participantorder_accessions", orderID, 0);
            gvAccessions.DataSource = dt;
            gvAccessions.DataBind();
        }


        private void bindParticipantReports(int orderID) {
            // TODO: make dataview for this guy...
            var dt = getDataViewDataForCooperator("web_feedback_participantorder_reports", orderID, 0);
            gvReports.DataSource = dt;
            gvReports.DataBind();
        }

        private void bindHeader(int orderID)
        {
            rptHeader.DataSource = getDataViewDataForCooperator("web_feedback_participantorder_header", orderID, 0);
            rptHeader.DataBind();
        }
    }
}
