using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrinGlobal.Web {
	public partial class Default : System.Web.UI.Page 
    {
		protected void Page_Load(object sender, EventArgs e) 
        {
             //Response.Redirect("~/search.aspx");

            //string home = "~/search.aspx";
            //if (Session["InitialRun"] != null)
            //    home = GrinGlobal.Core.Toolkit.GetSetting("HomePage", "~/search.aspx");
            //else
            //    Session["InitialRun"] = false;

            string home = GrinGlobal.Core.Toolkit.GetSetting("HomePage", "~/search.aspx");

            Response.Redirect(home);
		}
	}
}
