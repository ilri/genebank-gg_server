using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;
using System.Text;

namespace GrinGlobal.Web
{
    public partial class display : System.Web.UI.Page
    {
        public string HeaderText = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int methodID = Toolkit.ToInt32(Request.QueryString["methodid"], 0);

                if (methodID > 0)
                    bindMethodData(methodID);
            }
        }

        private void bindMethodData(int methodID)
        {
            HeaderText = "Query Result";

            DataTable dt = null;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                dt = sd.GetData("web_method_download", ":methodid=" + methodID, 0, 0).Tables["web_method_download"];

                dt = dt.Transform( new string[] {"crop", "method_name", "taxon", "acp", "acno", "acs"}, "coded_name", "coded_name", "observation_value");
                gv1.DataSource = dt;
                gv1.DataBind();

                //gv1.HeaderStyle.Font.Bold  =true;
                if (gv1.HeaderRow.Cells.Count > 16)
                    gv1.Font.Size = FontUnit.Smaller;
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            Utils.ExportToExcel(HttpContext.Current, gv1, "method-trait", "Query Result");
        }

    }
}
