using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Web.UI.HtmlControls;
using System.Data;



namespace GrinGlobal.Web {
    public partial class ImageTester : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            DataTable dt = null;

            var accessionID = Toolkit.ToInt32(Request.QueryString["accessionid"], -1); // 390399

            if (accessionID < 0) {

                // HACK: just for testing, remove later!
                dt = new DataTable();
                dt.Columns.Add("thumbnail_virtual_path", typeof(string));
                dt.Columns.Add("title", typeof(string));
                dt.Columns.Add("virtual_path", typeof(string));
                dt.Rows.Add(new object[] { "~/uploads/images/PI_501110_93ncei01_SD_pk.jpg", "some title here", "~/uploads/images/PI_501110_93ncei01_SD_pk_thumbnail.jpg"});
                dt.Rows.Add(new object[] { @"~\uploads\images\PI_501111_93ncei01_SD_pk.jpg", "title 2 here", @"~\uploads\images/PI_501111_93ncei01_SD_pk_thumbnail.jpg" });

            } else {
                using (var sd = new SecureData(false, UserManager.GetLoginToken())) {
                    dt = sd.GetData("web_accessiondetail_inventory_image", ":accessionid=" + accessionID, 0, 0).Tables["web_accessiondetail_inventory_image"];
                }
            }

            if (dt.Rows.Count > 0)
            {
                imagePreviewer.DataSource = dt;
                imagePreviewer.DataBind();
            }
            else
            {
                imagePreviewer.Visible = false;
            }

        }
    }
}
