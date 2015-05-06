using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using GrinGlobal.Business;

namespace GrinGlobal.Web {
    public partial class PivotAjax : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack && !IsCallback) {

                /*
                                string q = Request.QueryString["q"];
                                if (!String.IsNullOrEmpty(q)) {
                                    Query = q;
                                }
                                Limit = Toolkit.ToInt32(Request.QueryString["lim"], Limit);

                                IgnoreCase = Toolkit.ToBoolean(Request.QueryString["ic"], IgnoreCase);

                                MatchAll = Toolkit.ToBoolean(Request.QueryString["ma"], MatchAll);

                                string pg = Request.QueryString["ps"];
                                if (pg == "all" || pg == "0" || pg == "-1" || pg == "unlimited") {
                                    pg = "2000000000";
                                }
                                PageSize = Toolkit.ToInt32(pg, PageSize);

                                PageIndex = Toolkit.ToInt32(Request.QueryString["pg"], PageIndex);
                                */

















                fillDropDowns();
                ggPivotView.DataBind();
            }
        }

        private void fillDropDowns() {
            try {
                SearchAdmin sa = new SearchAdmin();

                //List<string> indexNames = sa.ListIndexes(true);
                //cblIndexes.DataSource = indexNames;
                //cblIndexes.DataBind();
                //foreach (ListItem li in cblIndexes.Items) {
                //    li.Selected = true;
                //}
                pnlSearch.Visible = true;
                pnlSearchDown.Visible = false;
            } catch {
                pnlSearch.Visible = false;
                pnlSearchDown.Visible = true;
            }
        }
    }
}
