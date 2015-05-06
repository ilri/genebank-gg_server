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
    public partial class participantdefault : System.Web.UI.Page
    {
        string _action = string.Empty;
        string _level = string.Empty;
        static string _prevPage = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            bindData();

            if (Request.QueryString["action"] != null)
            {
                _action = Request.QueryString["action"];
            }
            if (Request.QueryString["level"] != null)
            {
                _level = Request.QueryString["level"];
            }

       }

        private void bindData()
        {
            // when there's a bunch of different things to databind,
            // it's usually clearer to break them out.
            // for the mockup, let's just leave everything as DataManager.ExecRead

  //          ViewState["accession_id"] = id;

            bindParticipantHeader();
            bindParticipantReports2();
            bindParticipantAccessions();
            
        }

        private DataTable getDataViewDataForCooperator(string dvName, int limit) {

            using (var sd = new SecureData(false, UserManager.GetLoginToken(true))) {
                return sd.GetData(dvName, ":cooperatorid=" + sd.CooperatorID, 0, limit).Tables[dvName];
            }
        }

        private DataTable getDataViewData(string dvName, string dataParams, int limit)
        {
            using (var sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                return sd.GetData(dvName, dataParams, 0, limit).Tables[dvName];
            }
        }

        private void bindParticipantHeader()
        {
            var dt = getDataViewDataForCooperator("web_feedback_participantdefault_header", 1);
            if (dt.Rows.Count > 0) {
                lblWelcome.Text = lblWelcome.Text.Replace("{NAME}", dt.Rows[0]["participant_name"].ToString());
            } else {
                lblWelcome.Text = lblWelcome.Text.Replace("{NAME}", "");
            }
        }


        private void bindParticipantReports2()
        {
            // TODO: make dataview for this guy...
            var dt = getDataViewDataForCooperator("web_feedback_participantdefault_report", 0);
            gvParticipantReports2.DataSource = dt;
            gvParticipantReports2.DataBind();
        }
        private void bindParticipantAccessions() {
            // TODO: make dataview for this guy...
            var dt = getDataViewDataForCooperator("web_feedback_participantdefault_accessions", 0);
            gvAccessions.DataSource = dt;
            gvAccessions.DataBind();
        }
    }
}
