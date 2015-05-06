using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;
using System.Text.RegularExpressions;

namespace GrinGlobal.Web
{
    public partial class disclaimer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = Toolkit.ToInt32(Request.QueryString["id"], 0);
                if (id != 0)
                {
                    bindData(id);
                    pnlGGDisclaimer.Visible = false;
                }
                else
                {
                    lblHeading.Text = "GRIN-Global Software Disclaimer";
                    pnlGGDisclaimer.Visible = true;
                }
            }
        }

        private void bindData(int iprID) 
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken())) 
            {
                DataTable dt = sd.GetData("web_accessiondetail_ipr_disclaimer", ":accessioniprid=" + iprID, 0, 0).Tables["web_accessiondetail_ipr_disclaimer"];
                if (dt.Rows.Count > 0)
                {
                    string description = dt.Rows[0]["description"].ToString();
                    Regex regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);

                    MatchCollection mactches = regx.Matches(description);

                    foreach (Match match in mactches)
                    {
                        string matchURL = match.Value;
                        if (matchURL.Substring(matchURL.Length - 1, 1) == ")") matchURL = matchURL.Substring(0, matchURL.Length - 1);
                        if (matchURL.Substring(matchURL.Length - 2, 2) == ").") matchURL = matchURL.Substring(0, matchURL.Length - 2);
                        description = description.Replace(match.Value, "<a href='" + matchURL + "'>" + match.Value + "</a>");
                    }
                    lblDescription.Text = description;
                }
            }
        }
    }
}
