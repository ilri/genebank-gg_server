using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using GrinGlobal.Business;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace GrinGlobal.Web {
    public partial class View2 : System.Web.UI.Page {

        public string HeaderText;

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                parseQueryString();

                if (UserManager.IsAnonymousUser(UserManager.GetUserName()))
                {
                    btnAddToFavorite.Visible = false;
                }
                else
                {
                    btnAddToFavorite.Visible = true;
                }
            }
        }


        public void PivotView_LanguageChanged(object sender, EventArgs e) {
            parseQueryString();
        }

        private void parseQueryString() {
            // there are 2 main ways to call this page:
            // 1. Specify a mode and appropriate parameters (for dataviews hardwired to this page)
            // 2. Specify a dataview name, and its required parameters

            string parameters = Request.Params["params"];

            string headerDataviewName = Request.QueryString["headerdataview"] + Request.QueryString["hdv"] + Request.QueryString["headerdv"];
            if (!String.IsNullOrEmpty(headerDataviewName)) {
                using (var sd = UserManager.GetSecureData(true)) {
                    var dt = sd.GetData(headerDataviewName, parameters, 0, 0).Tables[headerDataviewName];
                    if (dt.Rows.Count > 0) {
                        HeaderText = Server.HtmlDecode(dt.Rows[0][0].ToString());
                    }
                }
            }

            var headerTitle = Request.Params["headertitle"] + Request.Params["htitle"];
            if (!String.IsNullOrEmpty(headerTitle)) {
                HeaderText = Server.HtmlDecode(headerTitle);
            }


            string format = Request.Params["format"] + string.Empty;

            switch (format.ToLower()) {
                case "xml":
                    format = "xml";
                    break;
                case "csv":
                case "comma":
                    format = "csv";
                    break;
                case "tab":
                case "txt":
                case "text":
                    format = "tab";
                    break;
                case "json":
                    format = "json";
                    break;
                case "html":
                default:
                    format = "html";
                    break;
            }

            string dataviewName = Request.Params["dataview"] + Request.Params["dv"];  // allow either 'dataview' or 'dv' in the url

            var options = "";
            var caps = (Request.Params["nocaptions"] + string.Empty).ToLower();
            var noCaptions = false;
            if (caps == "true" || caps == "1" || caps == "y") {
                // ignore captions by sending a known bogus language id
                options = "altlanguageid=-1";
                noCaptions = true;
            }
            var cpct = (Request.Params["compact"] + string.Empty).ToLower();
            var compact = cpct == "true" || cpct == "1" || cpct == "y";

            var pkName = Request.Params["pk"] + string.Empty;
            var altKeyName = Request.Params["ak"] + string.Empty;

            string errorMessage = null;
            try {
                if (!String.IsNullOrEmpty(dataviewName)) {
                    using (var sd = UserManager.GetSecureData(true)) {
                        var dt = sd.GetData(dataviewName, parameters, ggPivotView.SkipCount, ggPivotView.Limit, options).Tables[dataviewName];

                        if (format != "html") {
                            Toolkit.OutputData(dt, Response, format, noCaptions, compact);
                        } else {

                            if (!String.IsNullOrEmpty(pkName) && !String.IsNullOrEmpty(altKeyName)) {
                                ggPivotView.AllowGrouping = true;
                                ggPivotView.PrimaryKeyName = pkName;
                                ggPivotView.AlternateKeyName = altKeyName;
                            }

                            for (int i = 0; i < dt.Columns.Count; i++) {
                                var col = dt.Columns[i].ColumnName;
                                if (col.ToUpper().LastIndexOf("_ID") == col.Length - 3) {
                                    ggPivotView.PrimaryKeyName = col;
                                    break;
                                }
                            }

                            ggPivotView.DataSource = dt;
                            ggPivotView.DataBind();

                            string caption = Request.QueryString["caption"];
                            if (!String.IsNullOrEmpty(caption)) {
                                HeaderText = caption;
                            } else if (String.IsNullOrEmpty(HeaderText)) {
                                var title = dt.ExtendedProperties["Title"] as string;
                                if (!String.IsNullOrEmpty(title)) {
                                    HeaderText = title;
                                }
                            }
                            if (!String.IsNullOrEmpty(HeaderText)) {
                                HeaderText = HeaderText.Replace("__USERNAME__", UserManager.GetUserName());
                            }
                        }
                    }
                } else {
                    if (format != "html") {
                        throw new InvalidOperationException("No dataview name provided");
                    } else {
                        ggPivotView.DataSource = null;
                        ggPivotView.DataBind();
                        HeaderText = "Please specify a dataview to load by passing in 'dv' or 'dataview' on the querystring.&nbsp;&nbsp;Example:  <br /><br />view.aspx?dv=web_accessiondetail_header&amp;params=:accessionid=382733;:inventoryid=0;orderrequestid=0";
                    }
                }
            } catch (Exception ex) {
                if (format == "html") {
                    // be sure to puke hard if major problems happen and we're emitting html (not streaming raw data)
                    throw;
                } else {
                    // ASPNET doesn't respond well to Response.End() within a try/catch, so we wait until we're out of it.
                    errorMessage = ex.Message;
                }
            }
            if (format != "html") {
                if (!String.IsNullOrEmpty(errorMessage)) {
                    Response.Write("***ERROR***: " + errorMessage);
                }
                Response.End();
            }
        }

        protected void btnAddToOrder_Click(object sender, ImageClickEventArgs e)
        {
            // find all checkmarked rows, add them to the cart, save the cart.
            Cart c = Cart.Current;
            bool changed = false;
            int itemsAdded = 0;
            int itemsProcessed = 0;
            int itemsNotAvail = 0;
            string notAvailCol = "";
            for (int i = 0; i < ggPivotView.PrimaryKeys.Length; i++)
            {

                // this row needs to be added to the order.
                int accessionID = ggPivotView.PrimaryKeys[i];

                // prevent 'Not Available" item from getting into cart
                bool isAvailable = false;
                using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
                {
                    var dt = sd.GetData("web_search_overview", ":idlist=" + accessionID, 0, 0).Tables["web_search_overview"];

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["availability"].ToString().IndexOf("Add to Cart") > 0)
                        {
                            isAvailable = true;
                            break;
                        }
                    }
                }
                if (!isAvailable)
                {
                    itemsNotAvail++;
                    notAvailCol += "," + accessionID;
                }

                if (accessionID > 0 && isAvailable)
                {
                    int added = c.AddAccession(accessionID, null);
                    if (added > 0)
                    {
                        itemsAdded++;
                    }
                    else
                    {
                        itemsProcessed++;
                    }
                    changed = true;
                }
            }
            if (changed)
            {
                c.Save();
            }

            parseQueryString();


            string msg = "";
            string msgRed = "";
            if (itemsAdded == 0)
            {
            }
            else if (itemsAdded == 1)
            {
                msg = Page.GetDisplayMember("Search", "add1Item", "Added 1 item to your cart.");
            }
            else
            {
                msg = Page.GetDisplayMember("Search", "addMultipleItems", "Added {0} items to your cart.", itemsAdded.ToString());
            }

            if (itemsProcessed == 0)
            {
                // nothing processed, nothing to do
            }
            else
            {
                if (itemsAdded == 0)
                {
                    if (itemsProcessed == 1)
                    {
                        msg += Page.GetDisplayMember("Search",  "oneItemAlreadyInCart", "  One item was already in your cart.");
                    }
                    else
                    {
                        msg += Page.GetDisplayMember("Search", "allItemsAlreadyInCart", "  All available items were already in your cart.");
                    }
                }
                else
                {
                    if (itemsProcessed > 1)
                    {
                        msg += Page.GetDisplayMember("Search",  "someItemsAlreadyInCart", "  {0} items were already in your cart.", itemsProcessed.ToString());
                    }
                    else
                    {
                        msg += Page.GetDisplayMember("Search",  "oneItemAlreadyInCart", "  One item was already in your cart.");
                    }
                }
            }

            if (itemsNotAvail == 1)
                msgRed = Page.GetDisplayMember("Search",  "oneItemNotAvailable", "  One item is not available to be put into your cart.");
            else if (itemsNotAvail > 1)
                msgRed = Page.GetDisplayMember("Search",  "someItemsNotAvailable", "  {0} items not available to be put into your cart.", itemsNotAvail.ToString());

            Master.ShowMessage(msg.Trim());

            if (msgRed != "")
            {
                notAvailCol = notAvailCol.Substring(1, notAvailCol.Length - 1);
                msgRed = "<a target='_blank' href='view2.aspx?dv=web_search_overview&htitle=Item(s) not available to be put into your cart&params=:idlist=" + notAvailCol + "' title='click to see the list'><font color='red'>" + msgRed + "</font> </a>";
                Master.ShowMessageRed(msgRed.Trim());
            }

        }

        protected void btnAddToFavorite_Click(object sender, ImageClickEventArgs e)
        {
            // find all checkmarked rows, add them to the favorite.
            Favorite f = Favorite.Current;
            bool changed = false;
            int itemsAdded = 0;
            int itemsProcessed = 0;
            for (int i = 0; i < ggPivotView.PrimaryKeys.Length; i++)
            {
                // this row needs to be added to the order.
                int accessionID = ggPivotView.PrimaryKeys[i];

                if (accessionID > 0)
                {
                    int added = f.AddAccession(accessionID, null);
                    if (added > 0)
                    {
                        itemsAdded++;
                    }
                    else
                    {
                        itemsProcessed++;
                    }
                    changed = true;
                }
            }
            if (changed)
            {
                f.Save();
            }

            parseQueryString();


            string msg = "";
            if (itemsAdded == 0)
            {
            }
            else if (itemsAdded == 1)
            {
                msg = Page.GetDisplayMember("Search", "add1Item", "Added 1 item to your favorites.");
            }
            else
            {
                msg = Page.GetDisplayMember("Search", "addMultipleItems", "Added {0} items to your favorites.", itemsAdded.ToString());
            }
            if (itemsProcessed == 0)
            {
                // nothing processed, nothing to do
            }
            else
            {
                if (itemsAdded == 0)
                {
                    if (itemsProcessed == 1)
                    {
                        msg += Page.GetDisplayMember("Search", "oneItemAlreadyInFavorites", "  That item was already in your favorites.");
                    }
                    else
                    {
                        msg += Page.GetDisplayMember("Search", "allItemsAlreadyInFavorites", "  All of those items were already in your favorites.");
                    }
                }
                else
                {
                    if (itemsProcessed > 1)
                    {
                        msg += Page.GetDisplayMember("Search", "someItemsAlreadyInFavorites", "  {0} items were already in your favorites.", itemsProcessed.ToString());
                    }
                    else
                    {
                        msg += Page.GetDisplayMember("Search", "oneItemAlreadyInFavorites", "  That item was already in your favorites.");
                    }
                }
            }

            Master.ShowMessage(msg.Trim());
        }
    }
}
