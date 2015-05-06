using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;

namespace GrinGlobal.Web.taxon
{
    public partial class taxonomyliterature : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["tid"] != null)
                    bindCitation(Toolkit.ToInt32(Request.QueryString["tid"], 0));
             }
        }

        private void bindCitation(int id)
        {

            using (SecureData sd = UserManager.GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    rptReference.DataSource = sd.GetData("web_taxonomycwr_reference", ":taxonomyid=" + id, 0, 0).Tables["web_taxonomycwr_reference"];
                    rptReference.DataBind();
                }
            }
        }
    }
}
