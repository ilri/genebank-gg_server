using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace GrinGlobal.Web {
    public partial class TrustedSiteGenerator : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            string regEntry = String.Format(@"Windows Registry Editor Version 5.00

[HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Domains\{0}]
""http""=dword:00000002

[HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Zones\2]
""2200""=dword:00000000
", Request.Url.Host);

            string url = "~/uploads/exports/" + Path.GetRandomFileName() + ".reg";
            string tempFile = Server.MapPath(url);
            File.WriteAllText(tempFile, regEntry);

            Response.Clear();
            Response.Redirect(url);

        }
    }
}
