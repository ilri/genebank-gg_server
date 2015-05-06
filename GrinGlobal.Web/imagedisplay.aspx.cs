using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using GrinGlobal.Business;
using System.Data;

namespace GrinGlobal.Web
{
    public partial class ImagaDisplay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["imgPath"] != null)
            {
                string imgPath = Request.QueryString["imgPath"];
                image1.Src = cleanImagePath(imgPath);
            }
            else if (Request.QueryString["id"] != null)
            {
                int id = Toolkit.ToInt32(Request.QueryString["id"], 0);

                string type = "";
                if (Request.QueryString["type"] != null)
                {
                    type = Request.QueryString["type"].ToString();
                }

                bindData(id, type);
            }
            else if (Request.QueryString["lnk"].ToString() != null)
            {
                string type = "";
                string link = Request.QueryString["lnk"].ToString();
                int id = Toolkit.ToInt32(link.Split(',')[0], 0);

                if (link.IndexOf("|") > 0)
                {
                    if (link.Split('|')[1] == "cta")
                        type = "cta";
                }
               
                bindData(id, type);
            }
            else
            {
                lblInformation.Visible = true;
                image1.Visible = false;
            }
        }

        private void bindData(int id, string type)
        {
            using (var sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                DataTable dt = null;
                switch (type)
                {
                    case "taxonomy":
                        dt = sd.GetData("web_image", ":inven_imageid=" + 0 + ";:taxon_imageid=" + id + ";:cta_imageid=" + 0, 0, 0).Tables["web_image"];
                        break;
                    case "cta":
                        dt = sd.GetData("web_image", ":inven_imageid=" + 0 + ";:taxon_imageid=" + 0 + ";:cta_imageid=" + id, 0, 0).Tables["web_image"];
                        break;

                    case "":
                        
                    default:
                        dt = sd.GetData("web_image", ":inven_imageid=" + id + ";:taxon_imageid=" + 0 + ";:cta_imageid=" + 0, 0, 0).Tables["web_image"];
                        break;
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string virtual_path = dr["virtual_path"].ToString();
                        string title =  dr["title"].ToString();
                        string description = dr["description"].ToString();
                        string taken_by = dr["taken_by"].ToString();
                        string coop_id = dr["cooperator_id"].ToString();
                        string taken_date = Toolkit.ToDateTime(dr["created_date"]).ToString("dd-MMM-yyyy");
                        string pi_number = dr["pi_number"].ToString();
                        string inv_number = dr["inv_number"].ToString();
                        string copyright = dr["copyright"].ToString();
                        string note = dr["note"].ToString();
                    
                        image1.Src = Resolve(virtual_path);
                        lblInformation.Visible = true;

                        if (!string.IsNullOrEmpty(note)) note = " Comment: " + note;
                        if (!string.IsNullOrEmpty(taken_by))
                            taken_by = "Taken by: <a href='cooperator.aspx?id=" + coop_id + "'  target='_blank'>" + taken_by + "</a>";

                        switch (type)
                        {
                            case "taxonomy":
                                lblInformation.Text = "Image for: " + description + "<br />"
                                + "Taken on: " + taken_date + "<br />"
                                + copyright + " " + note + "<br /><br />";

                                break;
                            case "cta":
                                lblInformation.Text = "Image for: " + description + "<br />"
                                + note + "<br /><br />";

                                break;
                            case "":
                            default:
                                string attachDate = dr["attach_date"].ToString();
                                string attachDateCode = dr["attach_date_code"].ToString();
                                taken_date = Utils.DisplayDate(attachDate, attachDateCode);
                                if (!string.IsNullOrEmpty(taken_date)) taken_date = " on " + taken_date;

                                lblInformation.Text = "Image for: " + pi_number + " - " + description + "<br />"
                                + taken_by + taken_date + "<br />"
                                + " Inventory sample: " + inv_number +  "<br />"
                                + copyright + " " + note + "<br /><br />";

                                break;
                        }
                    }
                }
            }
        }

        private string Resolve(object url)
        {
            if (url is string && !String.IsNullOrEmpty(url as string))
            {
                string path = url as string;
                if (path.ToUpper().IndexOf("HTTP://") > -1)
                {
                    return path;
                }
                else
                {
                    //path = "~/uploads/images/" + path;
                    //return Page.ResolveClientUrl(path.Replace(@"\", "/").Replace("//", "/"));

                    string rootPath = Core.Toolkit.GetSetting("WebServerURL", "");

                    if (rootPath == "")
                    {
                        path = "~/uploads/images/" + path;
                        return Page.ResolveClientUrl(path.Replace(@"\", "/").Replace("//", "/"));
                    }
                    else
                    {
                        path = (rootPath + "/uploads/images/" + path).Replace(@"\", "/").Replace("//", "/");
                        return path;
                    }
                }
            }
            else
            {
                return "";
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
