using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;

namespace GrinGlobal.Web
{
    public partial class crop : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bindData(Toolkit.ToInt32(Request.QueryString["id"], 0));
            }
        }

        private void bindData(int coopID)
        {
            DataTable dt = null;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                dt = sd.GetData("web_crop", ":cropid=" + coopID, 0, 0).Tables["web_crop"];
                dvCrop.DataSource = dt;
                dvCrop.DataBind();

                if (dt.Rows.Count > 0)
                {
                    if ((dt.Rows[0]["note"] == DBNull.Value))
                        dvCrop.FindControl("tr_note").Visible = false;
                }

                string lnk = "";

                lnk = "<a href=cropdetail.aspx?type=descriptor&id=" + coopID + ">List of Descriptors</a>";

                lnk += "&nbsp;&nbsp;&nbsp;&nbsp;<a href=cropdetail.aspx?type=marker&id=" + coopID + ">List of Genetic Markers</a>";

                lnk += "&nbsp;&nbsp;&nbsp;&nbsp;<a href=cropdetail.aspx?type=species&id=" + coopID + ">List of Species</a>";

                lnk += "&nbsp;&nbsp;&nbsp;&nbsp;<a href=cropdetail.aspx?type=citation&id=" + coopID + ">List of Citations </a>(containing accessions in crop)<br /><br />";

                // Laura to do later
                //lnk += "&nbsp;&nbsp;&nbsp;<a href=cropdetail.aspx?type=frequency&id=" + coopID + ">Descriptor Report </a>(with frequency of observation) <br /><br />";

                lblLinks.Text = lnk;
            }

            bindAttachment(coopID);
        }

        private void bindAttachment(int id)
        {
            DataTable dt = null;

            using (var sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                dt = sd.GetData("web_crop_attach", ":cropid=" + id + ";:categorycode='IMAGE'", 0, 0).Tables["web_crop_attach"];

                if (dt.Rows.Count > 0)
                {
                    plImage.Visible = true;
                    string imgPath = dt.Rows[0]["virtual_path"].ToString();
                    image1.ImageUrl = imgPath;
                    if (!String.IsNullOrEmpty(dt.Rows[0]["note"].ToString().Trim()))
                        lblimg1.Text =  dt.Rows[0]["note"].ToString() + "<br/>";

                    if (dt.Rows.Count > 1)
                    {
                        plImage.Visible = true;
                        lblimg1.Text = lblimg1.Text + "<br/>";
                        image2.ImageUrl = dt.Rows[1]["virtual_path"].ToString();
                        if (!String.IsNullOrEmpty(dt.Rows[1]["note"].ToString().Trim()))
                            lblimg2.Text = "<br/>" + dt.Rows[1]["note"].ToString();
                    }
                }

                dt = sd.GetData("web_crop_attach", ":cropid=" + id + ";:categorycode='LINK'", 0, 0).Tables["web_crop_attach"];
                if (dt.Rows.Count > 0)
                {
                    plLink.Visible = true;
                    rptLink.DataSource = dt;
                    rptLink.DataBind();
                }
            }
        }

        private string cleanImagePath(string imgPath)
        {
            string s = imgPath.Substring(0, 1);
            string sUpload = "";
            if (imgPath.Length > 8) sUpload = imgPath.Substring(0, 8);
            if (s != "~" && s != "/" && sUpload != "uploads/")
            {
                string safePath = Toolkit.GetSetting("SafeImagePath", "http://www.ars-grin.gov");
                bool isSafe = false;
                var safePaths = safePath.Split(';');
                for (var i = 0; i < safePaths.Length; i++)
                {
                    if (imgPath.IndexOf(safePaths[i].Trim()) >= 0)
                    {
                        isSafe = true;
                        break;
                    }
                }
                if (!isSafe)
                    imgPath = "";
            }
            return imgPath;
        }
    }
}
