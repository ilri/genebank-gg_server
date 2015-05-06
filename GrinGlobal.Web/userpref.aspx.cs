using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using GrinGlobal.Business;
using System.Data;

// page to hold infor of preference, saved results, language choice etc.

namespace GrinGlobal.Web
{
    public partial class UserPref : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (UserManager.IsAnonymousUser(UserManager.GetUserName()))
                Response.Redirect("~/login.aspx?action=menu");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                populateLanguage();
                bindData();
            }
        }

        private void bindData()
        {
            cbEmail.Checked = Toolkit.ToBoolean(UserManager.GetUserPref("EmailOrder"), false);
            cbShipping.Checked = Toolkit.ToBoolean(UserManager.GetUserPref("EmailShipping"), false);
            cbEmailNews.Checked = Toolkit.ToBoolean(UserManager.GetUserPref("EmailNews"), false);

            SecureData sd = new SecureData(false, UserManager.GetLoginToken());

            // TODO: since login token decides the sd.languageid, so after changing the language id, the effect will only take place after logout then login.
            // TODO: so either take out the language id from logintoken, or find way to recreate the token, or ask user logout?
            ddlLang.SelectedIndex = ddlLang.Items.IndexOf(ddlLang.Items.FindByValue(sd.LanguageID.ToString()));
            // only after language part is done, then user's language setting can make the displaying different
        }

        private void populateLanguage()
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                //DataTable dt = sd.ListLanguages(0).Tables["list_languages"];  // can't directly use secure data funciton, blocked by checkUserHasAdminEnabled();
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    DataTable dt = dm.Read(@"select * from sys_lang");
                    foreach (DataRow dr in dt.Rows)
                    {
                        ddlLang.Items.Add(new ListItem(dr["title"].ToString(), dr["sys_lang_id"].ToString()));
                    }
                }
            }
        }

        protected void cbEmail_CheckedChanged(object sender, EventArgs e)
        {
            ViewState["hasChange"] = true;
        }

        protected void cbShipping_CheckedChanged(object sender, EventArgs e)
        {
            ViewState["hasChange"] = true;
        }

        protected void cbEmailNews_CheckedChanged(object sender, EventArgs e)
        {
            ViewState["hasChange"] = true;
        }

        protected void ddlLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["hasChange"] = true;
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if ((ViewState["hasChange"] != null) && Toolkit.ToBoolean(ViewState["hasChange"].ToString(), false))
            {
                if (Toolkit.ToBoolean(UserManager.GetUserPref("EmailOrder"), false) != cbEmail.Checked)
                    UserManager.SaveUserPref("EmailOrder", cbEmail.Checked.ToString());
                if (Toolkit.ToBoolean(UserManager.GetUserPref("EmailShipping"), false) != cbShipping.Checked)
                    UserManager.SaveUserPref("EmailShipping", cbShipping.Checked.ToString());
                if (Toolkit.ToBoolean(UserManager.GetUserPref("EmailNews"), false) != cbEmailNews.Checked)
                    UserManager.SaveUserPref("EmailNews", cbEmailNews.Checked.ToString());

                SecureData sd = new SecureData(false, UserManager.GetLoginToken());
                if (ddlLang.SelectedValue != sd.LanguageID.ToString())
                    UserManager.SaveUserLanguage(Toolkit.ToInt32(ddlLang.SelectedValue, 0));
            }
        }
    }
}
