using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace GrinGlobal.Web.feedback {
    public partial class viewparticpantorders : System.Web.UI.Page {

        public string HeaderText;

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                parseQueryString();
            }
        }


        public void PivotView_LanguageChanged(object sender, EventArgs e) {
            parseQueryString();
        }

        private void parseQueryString() {
            // there are 2 main ways to call this page:
            // 1. Specify a mode and appropriate parameters (for dataviews hardwired to this page)
            // 2. Specify a dataview name, and its required parameters

            using (var sd = UserManager.GetSecureData(true)) {
                var dt = sd.GetData("web_feedback_participantdefault_manageorders", ":cooperatorid=" + sd.CooperatorID, ggPivotView.SkipCount, ggPivotView.Limit, null).Tables["web_feedback_participantdefault_manageorders"];
                ggPivotView.DataSource = dt;
                ggPivotView.DataBind();
            }
        }
    }
}
