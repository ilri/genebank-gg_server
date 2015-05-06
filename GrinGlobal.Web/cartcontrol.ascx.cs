using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace GrinGlobal.Web {
    public partial class CartControl : System.Web.UI.UserControl {
        protected void Page_Load(object sender, EventArgs e) {

            // we don't put cart stuff here because individual pages
            // may change the contents of it.
            // we need to put it at the prerender event because
            // pages will have already done their processing by that point.
        }

        protected void Page_PreRender(object sender, EventArgs e){

            try {
                Cart c = Cart.Current;
                if (c == null) {
                    lblContents.Text = "Cart contents unknown";
                } else {
                    if (c.Accessions.Length == 0) {
                        lblContents.Text = Page.DisplayText("noItemsCart", "No items in cart");
                    } else if (c.Accessions.Length == 1) {
                        lblContents.Text = "<a href='" + AppRoot + "cartView.aspx'> " + Page.DisplayText("1ItemCart", "1 item in cart") + "</a>";
                    } else {
                        lblContents.Text = "<a href='" + AppRoot + "cartView.aspx'>" +  c.Accessions.Length + " " +  Page.DisplayText("nItemCart", "items in cart") + "</a>";
                    }
                }
            } catch (Exception ex) {
                Debug.WriteLine("Cart error: " + ex.Message);
                if (!HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.ToLower().Contains("~/error.aspx")) {
                    // never bomb if we're on error.aspx!
                    throw;
                }
            }
        }

        private string AppRoot
        {
            get
            {
                var root = Page.ResolveUrl("~/");
                if (GrinGlobal.Core.Toolkit.IsNonWindowsOS)
                {
                    root = "/gringlobal/";
                }
                return root;
            }
        }
    }
}