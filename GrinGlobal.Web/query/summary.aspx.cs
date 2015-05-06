using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;

namespace GrinGlobal.Web.query
{
    public partial class summary : System.Web.UI.Page
    {
        public string cntFamily = "";
        public string cntGenus = "";
        public string cntSpecies = "";
        public string cntAccession = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            using (var sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                DataTable dt = sd.GetData("web_summary_1", "", 0, 0).Tables["web_summary_1"];

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[0].ToString() != "0") cntFamily = dr[0].ToString();
                    if (dr[1].ToString() != "0") cntGenus = dr[1].ToString();
                    if (dr[2].ToString() != "0") cntSpecies = dr[2].ToString();
                    if (dr[3].ToString() != "0") cntAccession = dr[3].ToString();
                }

            }
        }
    }
}
