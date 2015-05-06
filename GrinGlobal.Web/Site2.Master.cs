using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using GrinGlobal.Core;
using GrinGlobal.Business;
using System.Data;
using System.Diagnostics;
using System.Web.UI.HtmlControls;

namespace GrinGlobal.Web {
    public delegate void OnLanguageChanged2(object sender, EventArgs e);

	public partial class Site2 : System.Web.UI.MasterPage {

        public event OnLanguageChanged2 LanguageChanged;

        protected global::System.Web.UI.WebControls.Menu mnuMainLoggedIn;
        protected global::System.Web.UI.WebControls.Menu mnuMainAnon;

        public string AppRoot {
            get {
                var root = Page.ResolveUrl("~/");
                if (Toolkit.IsNonWindowsOS) {
                    root = "/gringlobal/";
                }
                return root;
            }
        }

        public string Theme {
            get {
                var theme = Request.QueryString["theme"];
                if (String.IsNullOrEmpty(theme)) {
                    if (Toolkit.GetSetting("AllowCookies", true)) {
                        // not on query string, look at cookie
                        var cookie = Request.Cookies["theme"];
                        if (cookie != null) {
                            theme = cookie.Value;
                        }
                    }
                } else {
                    // changed via query string. update cookie.
                    if (Toolkit.GetSetting("AllowCookies", true)) {
                        var cookie = new HttpCookie("theme", theme);
                        cookie.Expires = DateTime.UtcNow.AddYears(10);
                        Response.Cookies.Add(cookie);
                    }
                }
                if (String.IsNullOrEmpty(theme)) {
                    //theme = "default";
                    theme = Toolkit.GetSetting("DefaultWebTheme", "theme1");  // use theme1 for RC1
                }
                return theme;
            }
        }

        private CacheManager _cm;

        protected void Page_Load(object sender, EventArgs e) {

            try {
                Page.SetTitle(null);

                lnkTheme.Href = AppRoot + "styles/default.aspx?theme=" + Theme;


                loginStatus.LogoutText = "Logout " + UserManager.GetUserName();

                this.ShowFlashMessage(lblMessage);
                this.ShowFlashError(lblError);

                _cm = fillLanguageValueCache();

                this.PreRender += new EventHandler(Site1_PreRender);
                this.mnuMain.MenuItemDataBound += new MenuEventHandler(menuItemDataBound);
                if (this.mnuMainAnon != null) {
                    this.mnuMainAnon.MenuItemDataBound += new MenuEventHandler(menuItemDataBound);
                }

                if (this.mnuMainLoggedIn != null) {
                    this.mnuMainLoggedIn.MenuItemDataBound += new MenuEventHandler(menuItemDataBound);
                }

                if (!IsPostBack) {
                    bindLanguageDropdown();
                }
            } catch (Exception ex) {
                if (Request.AppRelativeCurrentExecutionFilePath.ToLower().Contains("/error.aspx")) {
                    // eat the error, we're already on the error page!
                } else {
                    throw;
                }
            }
        }

         void bindLanguageDropdown()
        {
            using (var sd = UserManager.GetSecureData(true))
            {
                if (UserManager.IsAnonymousUser(sd.WebUserName))
                {
                    // let an anonymous user change their language
                    ddlLanguage.Visible = true;
                    ddlLanguage.DataSource = UserManager.ListLanguages();
                    ddlLanguage.DataBind();
                    var item = ddlLanguage.Items.FindByValue(sd.LanguageID.ToString());
                    ddlLanguage.SelectedIndex = ddlLanguage.Items.IndexOf(item);
                    lblChoose.Visible = true;
                    pnlRegister.Visible = true;
                }
                else
                {
                    // logged in user must go to preferences page since we need to save it to the database,
                    // so hide language dropdown
                    ddlLanguage.Visible = false;
                    lblChoose.Visible = false;
                    pnlRegister.Visible = false;
                }
            }
        }

        void menuItemDataBound(object sender, MenuEventArgs e) {
             try {
                 languageText(e.Item, _cm);
             } catch (Exception ex) {
                 Debug.WriteLine(ex.Message);
             }
        }

        void Site1_PreRender(object sender, EventArgs e) {
            // loop through all server controls, replace with language-specific values

            fillLanguageValues(this.Page.Controls, _cm);

        }

        private static CacheManager fillLanguageValueCache() {
            using (var sd = UserManager.GetAdminData(true)) {
                var cm = CacheManager.Get("AppResource" + sd.LanguageID);
                if (cm.Keys.Count == 0) {
                    var dt = sd.ListAppResources("WebTool", sd.LanguageID).Tables["list_app_resources"];
                    foreach (DataRow dr in dt.Rows) {
                        // TODO: does not handle multi-valued items yet!!!!
                        cm[(dr["form_name"] + "--" + dr["app_resource_name"]).ToLower()] = dr["display_member"];
                        cm[(dr["form_name"] + "--" + dr["app_resource_name"] + "-title").ToLower()] = dr["description"];
                    }
                }
                return cm;
            }
        }

        /// <summary>
        /// Pulls record from app_resource table which matches current page and given control, or any page and given control.  aka "~/[pagename]--[controlname]" OR "Any--[controlname]".  So put "Any" in app_resource.form_name field if it should apply to the control on any page (e.g. menu items)
        /// </summary>
        /// <param name="controlName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string DisplayText(string controlID, string defaultValue) {
            var cm = fillLanguageValueCache();
            return getDisplayTextFromLanguageCache(cm, controlID, defaultValue);
        }

        /// <summary>
        /// Pulls record from app_resource table which matches current page and given control, or any page and given control.  aka "~/[pagename]--[controlname]-Description" OR "Any--[controlname]-Description".  So put "Any" in app_resource.form_name field if it should apply to the control on any page (e.g. menu items)
        /// </summary>
        /// <param name="controlName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string DescriptionText(string controlID, string defaultValue) {
            var cm = fillLanguageValueCache();
            return getDescriptionTextFromLanguageCache(cm, controlID, defaultValue);
        }

        private void languageText(MenuItem mi, CacheManager cm) {
            mi.Text = getDisplayTextFromLanguageCache(cm, mi.NavigateUrl, mi.Text);
            mi.ToolTip = getDescriptionTextFromLanguageCache(cm, mi.NavigateUrl, mi.ToolTip);
        }

        private void languageText(Control c, CacheManager cm) {

            // TODO: does not handle multi-valued items yet!!!!
            if (c is Label) {
                var lbl = c as Label;
                lbl.Text = getDisplayTextFromLanguageCache(cm, c.ID, lbl.Text);
                lbl.ToolTip = getDescriptionTextFromLanguageCache(cm, c.ID, lbl.ToolTip);
            } else if (c is TextBox) {
                var txt = c as TextBox;
                txt.Text = getDisplayTextFromLanguageCache(cm, c.ID, txt.Text);
                txt.ToolTip = getDescriptionTextFromLanguageCache(cm, c.ID, txt.ToolTip);
            } else if (c is RadioButton) {
                var rb = c as RadioButton;
                rb.Text = getDisplayTextFromLanguageCache(cm, c.ID, rb.Text);
                rb.ToolTip = getDescriptionTextFromLanguageCache(cm, c.ID, rb.ToolTip);
            } else if (c is CheckBox) {
                var cb = c as CheckBox;
                cb.Text = getDisplayTextFromLanguageCache(cm, c.ID, cb.Text);
                cb.ToolTip = getDescriptionTextFromLanguageCache(cm, c.ID, cb.ToolTip);
            } else if (c is Button) {
                var btn = c as Button;
                btn.Text = getDisplayTextFromLanguageCache(cm, c.ID, btn.Text);
                btn.ToolTip = getDescriptionTextFromLanguageCache(cm, c.ID, btn.ToolTip);
            } else if (c is HyperLink) {
                var hl = c as HyperLink;
                hl.Text = getDisplayTextFromLanguageCache(cm, hl.NavigateUrl, hl.Text);
                hl.ToolTip = getDescriptionTextFromLanguageCache(cm, hl.NavigateUrl, hl.ToolTip);
            } else if (c is LiteralControl) {
                var lc = c as LiteralControl;
                lc.Text = getDisplayTextFromLanguageCache(cm, c.ID, lc.Text);
            } else if (c is Literal) {
                var lc = c as Literal;
                lc.Text = getDisplayTextFromLanguageCache(cm, c.ID, lc.Text);
            } else if (c is LoginStatus){
                var ls = c as LoginStatus;
                ls.LogoutText = getDisplayTextFromLanguageCache(cm, ls.ID, ls.LogoutText);
                ls.LoginText = getDisplayTextFromLanguageCache(cm, c.ID, ls.LoginText);
            } else if (c is LinkButton) {
                var btn = c as LinkButton;
                btn.Text = getDisplayTextFromLanguageCache(cm, c.ID, btn.Text);
                btn.ToolTip = getDescriptionTextFromLanguageCache(cm, c.ID, btn.ToolTip);
            } else if (c is ImageButton) {
                var btn = c as ImageButton;
                btn.AlternateText = getDisplayTextFromLanguageCache(cm, c.ID, btn.AlternateText);
                btn.ToolTip = getDescriptionTextFromLanguageCache(cm, c.ID, btn.ToolTip);
            } else if (c is SiteMapPath) {
                var smp = c as SiteMapPath;
                smp.SkipLinkText = getDisplayTextFromLanguageCache(cm, c.ID, smp.SkipLinkText);
                smp.ToolTip = getDescriptionTextFromLanguageCache(cm, c.ID, smp.ToolTip);
            } else if (c is Menu){
                // recursively spin through menu items...
                foreach (MenuItem mi in (c as Menu).Items) {
                    mi.Text = getDisplayTextFromLanguageCache(cm, mi.NavigateUrl, mi.Text);
                    mi.ToolTip = getDescriptionTextFromLanguageCache(cm, mi.NavigateUrl, mi.ToolTip);
                }
            } else if (c is SiteMapNodeItem){
                var item = c as SiteMapNodeItem;
                item.ToolTip = getDescriptionTextFromLanguageCache(cm, c.ID, item.ToolTip);
            } else if (c is HtmlHead
                || c is HtmlLink
                || c is HtmlTitle
                || c is ContentPlaceHolder
                || c is Menu
                || c is DropDownList
                || c is Panel
                || c is HtmlForm
                ) {
                //Debug.WriteLine("Ignoring language text on control type " + c.GetType());
            } else {
                Debug.WriteLine("fillLanguageValues is missing control type " + c.GetType());
            }

        }

        private static string getDisplayTextFromLanguageCache(CacheManager cm, string id, string defaultValue){
            var ctx = HttpContext.Current;
            if (ctx != null && ctx.Request != null) {
                var key = (ctx.Request.AppRelativeCurrentExecutionFilePath + "--");
                return (cm[(key + id).ToLower()] ?? cm[("any--" + id).ToLower()] ?? defaultValue).ToString();
            } else {
                return defaultValue;
            }
        }

        private static string getDescriptionTextFromLanguageCache(CacheManager cm, string id, string defaultValue) {
            var ctx = HttpContext.Current;
            if (ctx != null && ctx.Request != null) {
                var key = (ctx.Request.AppRelativeCurrentExecutionFilePath + "--");
                return (cm[(key + id + "-title").ToLower()] ?? cm[("any--" + id + "-title").ToLower()] ?? defaultValue).ToString();
            } else {
                return defaultValue;
            }
        }

        void fillLanguageValues(ControlCollection controls, CacheManager cm) {

            foreach (Control c in controls) {

                languageText(c, cm);

                fillLanguageValues(c.Controls, cm);
            }
        }




        public CartControl CartControl {
            get {
                return this.ggCart;
            }
        }

        private void hideMenuItemsAsNeeded() {
            hideBasedOnRole("Admins", "/admin/");
            hideBasedOnRole("FeedbackOwners", "/feedback/admin/");
            hideBasedOnRole("FeedbackSubmitters", "/feedback/");
        }

        private void hideBasedOnRole(string roleName, string partialUrl){
            if (!Page.User.IsInRole(roleName)){
                if (mnuMainLoggedIn != null) {
                    for (int i = 0; i < mnuMainLoggedIn.Items.Count; i++) {
                        if (mnuMainLoggedIn.Items[i].NavigateUrl.ToLower().Contains(partialUrl)) {
                            mnuMainLoggedIn.Items.RemoveAt(i);
                            i--;
                        }
                    }
                }
                if (mnuMainAnon != null) {
                    for (int i = 0; i < mnuMainAnon.Items.Count; i++) {
                        if (mnuMainAnon.Items[i].NavigateUrl.ToLower().Contains(partialUrl)) {
                            mnuMainAnon.Items.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }

        private void selectMenuItem() {
            string name = Path.GetFileName(Request.FilePath);

            

        }


        protected void Page_PreRender(object sender, EventArgs e) {

            if (String.IsNullOrEmpty(Page.Form.DefaultFocus)) {
                if (this.Controls.Count > 0) {
                    // default to focusing on first textbox / checkbox / dropdownlist / in the form
                    setDefaultFocus(Page.Form.Controls);
                }
            }
        }

        private bool setDefaultFocus(ControlCollection controls){

            foreach (Control ctl in controls) {
                if (ctl is TextBox || ctl is CheckBox || ctl is DropDownList || ctl is RadioButton || ctl is FileUpload) {
                    if (ctl.Visible) {
                        Page.Form.DefaultFocus = ctl.ClientID;
                        return true;
                    }
                } else if (ctl.HasControls()) {
                    if (setDefaultFocus(ctl.Controls)) {
                        return true;
                    }
                }
            }
            return false;
        }


		protected void loginStatus_LoggingOut(object sender, LoginCancelEventArgs e) {
            UserManager.Logout();
		}


        protected override void AddedControl(Control control, int index) {
            // This is necessary because Safari and Chrome browsers don't display the Menu control correctly.
            if (Request.ServerVariables["http_user_agent"].IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) != -1) {
                this.Page.ClientTarget = "uplevel";
            }

            base.AddedControl(control, index);
        }

        protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e) {
            using (var sd = UserManager.GetSecureData(true)) {
                sd.LanguageID = Toolkit.ToInt32(ddlLanguage.SelectedValue, 1);
                if (Toolkit.GetSetting("AllowCookies", true)) {
                    var cookie = new HttpCookie("language", sd.LanguageID.ToString());
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                HttpContext.Current.Session["anonymous_login"] = sd.RegenerateLoginToken();
                _cm = fillLanguageValueCache();
                if (LanguageChanged != null) {
                    LanguageChanged(ddlLanguage, e);
                }
            }
        }

        public string PageName
        {
            get
            {
                string name = Page.Request.AppRelativeCurrentExecutionFilePath;
                int pos = name.IndexOf("/");
                name = name.Substring(pos + 1, name.Length - pos -1);
                return name;
            }
        }

        public string GeneBankName
        {
            get
            {
                return Toolkit.GetSetting("GeneBankName", "Your Gene Bank Name Here");
            }
        }
	}
}
