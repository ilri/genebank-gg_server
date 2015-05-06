using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using GrinGlobal.Business;
using GrinGlobal.Core;

namespace GrinGlobal.Web
{
    public partial class PopUpHelp : System.Web.UI.Page
    {
       protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int helpId = 0;
                if (this.Request.QueryString["id"] != null)
                    helpId = Int32.Parse(Request.QueryString["id"].ToString());

                string helpTag = "";
                if (this.Request.QueryString["tag"] != null)
                    helpTag = Request.QueryString["tag"].ToString();

                string helpHeading = "";
                if (this.Request.QueryString["heading"] != null)
                    helpHeading = Request.QueryString["heading"].ToString();

                if (helpId > 0)
                    DisplayHelp(helpId);
                else if (!string.IsNullOrEmpty(helpTag))
                    DisplayHelpTag(helpTag);
                else if (!string.IsNullOrEmpty(helpHeading))
                    DisplayHelpHeading(helpHeading);
                else
                    lblTitle.Text = Site1.DisplayText("noHelp", "Help content is not available now.");  
            }
        }

        private void DisplayHelp(int id)
        {
           using (SecureData sd = UserManager.GetSecureData(true))
           {
               using (DataManager dm = sd.BeginProcessing(true, true))
               {
                   DataTable dt = dm.Read(@"
                            select 
                                web_help_tag, virtual_path, heading, title, help_text_html
                            from 
                                web_help
                            where 
                                web_help_id = :helpid 
                                and sys_lang_id = :syslangid
                            ", new DataParameters(
                           ":helpid", id,
                           ":syslangid", sd.LanguageID
                           ));


                   if (dt.Rows.Count > 0)
                   {
                       string tag = dt.Rows[0].ItemArray[0].ToString();
                       string vPath = dt.Rows[0].ItemArray[1].ToString();
                       string heading = dt.Rows[0].ItemArray[2].ToString();
                       string title = dt.Rows[0].ItemArray[3].ToString();
                       string text = dt.Rows[0].ItemArray[4].ToString();

                       string root = Toolkit.GetSetting("WebHelpURL", "~/help/");

                       if (!string.IsNullOrEmpty(vPath))
                           Response.Redirect(vPath);
                       else if (!string.IsNullOrEmpty(tag))
                           Response.Redirect(root + tag);
                       else if (!string.IsNullOrEmpty(text))
                       {
                           lblHeading.Text = heading;
                           lblTitle.Text = title;
                           lblHelp.Text = text;
                       }
                       else
                           lblTitle.Text = Site1.DisplayText("noHelp", "Help content is not available now.");
                   }
                   else
                   {
                       lblTitle.Text = Site1.DisplayText("noHelp", "Help content is not available now.");
                   }
               }
           }
        }

        private void DisplayHelpTag(string tag)
        {
            tag = Utils.Sanitize(tag);
            if (tag.ToLower() == "about")
            {
                string root = Toolkit.GetSetting("WikiURL", "http://www.grin-global.org/index.php/Main_Page");
                Response.Redirect(root);
            }
            else
            {
                string root = Toolkit.GetSetting("WebHelpURL", "~/help/");
                Response.Redirect(root + tag);
            }
        }

        private void DisplayHelpHeading(string heading)
        {
            heading = Utils.Sanitize(heading);
            using (SecureData sd = UserManager.GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    DataTable dt = dm.Read(@"
                            select 
                                web_help_tag, virtual_path, heading, title, help_text_html
                            from 
                                web_help
                            where 
                                heading = :heading 
                                and sys_lang_id = :syslangid
                            ", new DataParameters(
                            ":heading", heading,
                            ":syslangid", sd.LanguageID
                            ));


                    if (dt.Rows.Count > 0)
                    {
                        string tag = dt.Rows[0].ItemArray[0].ToString();
                        string vPath = dt.Rows[0].ItemArray[1].ToString();
                        string title = dt.Rows[0].ItemArray[3].ToString();
                        string text = dt.Rows[0].ItemArray[4].ToString();

                        string root = Toolkit.GetSetting("WebHelpURL", "~/help/");

                        if (!string.IsNullOrEmpty(vPath))
                            Response.Redirect(vPath);
                        else if (!string.IsNullOrEmpty(tag))
                            Response.Redirect(root + tag);
                        else if (!string.IsNullOrEmpty(text))
                        {
                            lblHeading.Text = heading;
                            lblTitle.Text = title;
                            lblHelp.Text = text;
                        }
                        else
                            lblTitle.Text = Site1.DisplayText("noHelp", "Help content is not available now.");
                    }
                    else
                    {
                        lblTitle.Text = Site1.DisplayText("noHelp", "Help content is not available now.");
                    }
                }
            }
        }


    }
}
