using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;

namespace GrinGlobal.Web {
    public partial class pingsearchengine : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            using (var sa = UserManager.GetSecureData(true)) {
                sa.PingSearchEngine();
            }
        }
        public string GetBindingType() {
            return Toolkit.GetSearchEngineBindingType();
        }
        public string GetBindingUrl() {
            return Toolkit.GetSearchEngineBindingUrl();
        }
    }
}
