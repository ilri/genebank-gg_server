using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using GrinGlobal.Business;
using System.Data;
using System.Text;

namespace GrinGlobal.Web
{
    public partial class Search : System.Web.UI.Page
    {

        public void PivotView_LanguageChanged(object sender, EventArgs e)
        {
            doSearch("");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindDropDowns();

                q.Text = Request.QueryString["q"] + Request.QueryString["q2"];
                string aq = "";
                if (Request.QueryString["aq"] != null) aq = Request.QueryString["aq"].ToString();

                lim.SelectedIndex = lim.Items.IndexOf(lim.Items.FindByValue(Toolkit.ToInt32(Request.QueryString["lim"], 500).ToString()));
                if (lim.SelectedIndex == -1)
                {
                    lim.SelectedIndex = lim.Items.IndexOf(lim.Items.FindByValue("500"));  // could not find their choice, use default of 500
                }

                string qsIC = ("" + Request.QueryString["ic"]).ToLower();
                ic.Checked = qsIC == "true" || qsIC == "on" || qsIC == "1" || qsIC == "yes" || qsIC == "";

                string qsMA = ("" + Request.QueryString["ma"]).ToLower();
                ma.Checked = qsMA == "true" || qsMA == "on" || qsMA == "1" || qsMA == "yes" || qsMA == "";

                string dvName = ("" + Request.QueryString["view"]);
                if (!String.IsNullOrEmpty(dvName))
                {
                    if (!dvName.ToLower().StartsWith("web_search_"))
                    {
                        dvName = "web_search_" + dvName;
                    }
                }
                view.SelectedIndex = view.Items.IndexOf(view.Items.FindByValue(dvName));
                if (view.SelectedIndex == -1)
                {
                    view.SelectedIndex = 0;  // they didn't specify a dataview, use first one as default
                }

                ggPivotView.PageSize = Toolkit.ToInt32(Request.QueryString["ps"], ggPivotView.PageSize);

                ggPivotView.PageIndex = Toolkit.ToInt32(Request.QueryString["pg"], ggPivotView.PageIndex);

                if (!String.IsNullOrEmpty(q.Text) || !String.IsNullOrEmpty(aq))
                {
                    // this is a url-based search (i.e. search.aspx?q=B73&rs=overview
                    doSearch(aq);
                }

                if (UserManager.IsAnonymousUser(UserManager.GetUserName()))
                {
                    btnAddToFavorite.Visible = false;
                    //btnAddToOrder.Visible = false;   //Laura possible later
                }
                else
                {
                    btnAddToFavorite.Visible = true;
                }
            }
        }

        private void bindDropDowns()
        {
            pnlSearchDown.Visible = false;
            pnlSearch.Visible = true;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                var dt = sd.GetData("web_search_dataviewlist", ":langid=" + sd.LanguageID, 0, 0).Tables["web_search_dataviewlist"];

                //                    using (DataManager dm = sd.BeginProcessing(true)) {
                //                        DataTable dt = dm.Read(@"
                //select
                //    dv.dataview_name,
                //    coalesce(dvl.title, dv.dataview_name, '') as title
                //from
                //    sys_dataview dv left join sys_dataview_lang dvl
                //        on dv.sys_dataview_id = dvl.sys_dataview_id
                //        and dvl.sys_lang_id = :langid
                //where
                //    dv.category_name = 'web_search'
                //order by
                //    dv.category_sort_order, 2
                //", new DataParameters(":langid", sd.LanguageID, DbType.Int32));

                view.DataSource = dt;
                view.DataBind();

                searchItem1.LoadData = true;
                searchItemLL.LoadData = true;
                //                    }
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            doSearch("");
        }

        void doSearch(string advanceQry)
        {
            try
            {
                using (HighPrecisionTimer hpt = new HighPrecisionTimer("timer", true))
                {
                    List<DataTable> tables = new List<DataTable>();

                    int limit = Toolkit.ToInt32(lim.SelectedValue, 0);

                    string query = "";
                    if (ml.Checked)
                    {
                        query = q2.Text;
                    }
                    else
                    {
                        query = q.Text;
                    }

                    q.Text = q2.Text = query;

                    string sDv = "";
                    string advancedQuery = "";
                    if (advanceQry == "")
                        advancedQuery = getAdvancedQuery();
                    else
                        advancedQuery = advanceQry;
                    var searchString = query;
                    query = query + advancedQuery;
                    string op = ma.Checked ? " and " : " or ";
                    if (query.Length >= op.Length)
                    {
                        if (query.Substring(0, op.Length) == op)
                            query = query.Substring(op.Length, query.Length - op.Length);
                    }

                    //if (ed.Checked && query != "")
                    //    query = query + " AND (@inventory.is_distributable = 'Y' and @inventory.is_available = 'Y')";

                    if (query == "")
                    {
                        Master.ShowError(Page.GetDisplayMember("Search", "doSearch{noCriteria}", "Please enter search string or other search criteria and try again."));
                    }
                    else
                    {
                        var searchIndexes = Toolkit.GetSetting("WebSearchableIndexes", "accession, accession_name, taxonomy_species, inventory, accession_source, cooperator");

                        DataSet ds = null;
                        using (var sd = new SecureData(true, UserManager.GetLoginToken(true)))
                        {
                            //ds = sd.Search(query, ic.Checked, ma.Checked, searchIndexes, "accession", 0, limit, 0, 0, view.SelectedValue, "passthru=always;OrMultipleLines=" + (ml.Checked ? "true" : "false"), null, null);
                            if (exp.Checked)
                                sDv = "web_search_overview_export";
                            else
                                sDv = view.SelectedValue;

                            //if (ed.Checked)
                            //    ds = sd.Search(query, ic.Checked, ma.Checked, searchIndexes, "accession", 0, 0, 0, 0, "", "passthru=nonindexedorcomparison;OrMultipleLines=" + (ml.Checked ? "true" : "false"), null, null);
                            //else
                            //    ds = sd.Search(query, ic.Checked, ma.Checked, searchIndexes, "accession", 0, limit, 0, 0, sDv, "passthru=nonindexedorcomparison;OrMultipleLines=" + (ml.Checked ? "true" : "false"), null, null);
                            query = query + " and @accession.is_web_visible = 'Y' ";
                            query = Utils.Sanitize2(query);

                            ds = sd.Search(query, ic.Checked, ma.Checked, searchIndexes, "accession", 0, limit, 0, 0, sDv, "passthru=nonindexedorcomparison;OrMultipleLines=" + (ml.Checked ? "true" : "false"), null, null);

                            var dtError = ds.Tables["ExceptionTable"];
                            if (dtError != null && dtError.Rows.Count > 0)
                            {
                                if (dtError.Rows[0]["Message"].ToString().ToLower().StartsWith("bad search string"))
                                {
                                    Master.ShowError(Page.GetDisplayMember("Search", "doSearch{invalidquery}", "Invalid query.  Please alter your criteria and try again."));
                                }
                                else
                                {
                                    var type = dtError.Rows[0]["ExceptionType"].ToString().ToLower();
                                    if (type.Contains("endpointnotfoundexception"))
                                    {
                                        throw new InvalidOperationException(dtError.Rows[0]["Message"].ToString());
                                    }
                                    else
                                    {
                                        Master.ShowError(dtError.Rows[0]["Message"].ToString());
                                    }
                                }
                            }
                            /*
                            else
                            {
                                if (ed.Checked)
                                {
                                    List<int> IDs = new List<int>();
                                    int id;
                                    if (ds != null)
                                    {
                                        foreach (DataRow dr in ds.Tables["SearchResult"].Rows)
                                        {
                                            if (int.TryParse(dr[0].ToString(), out id))
                                                IDs.Add(id);
                                        }
                                    }

                                    DataTable dtID = null;
                                    List<int> IDa = new List<int>();
                                    using (var dm = DataManager.Create(sd.DataConnectionSpec))
                                    {
                                        dtID = dm.Read("select distinct accession_id from inventory where is_distributable =  'Y' and is_available = 'Y'");
                                        if (dtID != null)
                                        {
                                            foreach (DataRow dr in dtID.Rows)
                                            {
                                                if (int.TryParse(dr[0].ToString(), out id))
                                                    IDa.Add(id);
                                            }
                                        }
                                    }

                                    List<int> IDavail = IDs.Intersect(IDa).ToList();

                                    if (limit < IDavail.Count)
                                        IDavail = IDavail.GetRange(0, limit);

                                    ds = sd.GetData(sDv, ":idlist=" + string.Join(",", IDavail.Select(n => n.ToString()).ToArray()), 0, 0, "");
                                }
                            } */
                        }
                        hpt.Stop();

                        //TimeSpan ts = TimeSpan.FromMilliseconds(hpt.ElapsedMilliseconds);
                        //string elapsed = ts.Seconds.ToString("00") + "." + ts.Milliseconds.ToString("000");

                        // lblTimer.Text = "Search time: " + elapsed + " seconds";

                        DataTable dt = ds.Tables[sDv];
                        if (exp.Checked)
                        {
                            pnlSearchResults.Visible = false;
                            if (dt == null)
                                Master.ShowError(Page.GetDisplayMember("Search", "doSearch{errorout}", "Query limit exceeded capacity.  Please reduce your return accessions number and try again."));
                            else
                            {
                                Toolkit.OutputData(dt, Response, "csv", false, true);
                                //exportToExcel(dt);
                            }
                        }
                        else
                        {
                            pnlSearchResults.Visible = true;
                            //if (rs.SelectedValue.Contains("obs")) {
                            //    // special case: obs data is stored row-wise,
                            //    //               we need it column-wise
                            //    // need to pivot by the 'trait_value' column

                            //    dt = dt.transform(new string[] { "accession_id", "pi_number" }, "trait_name", null, "trait_value");

                            //}

                            if (dt == null || dt.Rows.Count == 0)
                            {
                                pnlSearchResultsHeader.Visible = false;
                                ggPivotView.DataSource = dt;
                                ggPivotView.DataBind();
                            }
                            else
                            {
                                pnlSearchResultsHeader.Visible = true;
                                ggPivotView.DataSource = dt;
                                ggPivotView.DataBind();
                            }
                            var display = "<b>Query Criteria:</b><br /> " + "&nbsp; &nbsp; &nbsp;Search String: " + searchString + "<br />" + getAdvancedQueryDisplay();
                            Master.ShowMore(display);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pnlSearch.Visible = false;
                pnlSearchDown.Visible = true;
                if (Page.User.IsInRole("Admins"))
                {
                    pnlError.Visible = true;
                    SearchError = ex.ToString();
                }
                else
                    pnlError.Visible = false;
            }
        }

        public string SearchError;
        DataManager _dm;
        SecureData _sd;

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
                    _dm = sd.BeginProcessing(true, true);
                    _sd = sd;
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

            doSearch("");


            string msg = "";
            string msgRed = "";
            if (itemsAdded == 0)
            {
            }
            else if (itemsAdded == 1)
            {
                msg = Page.DisplayText("add1Item", "Added 1 item to your cart.");
            }
            else
            {
                msg = Page.DisplayText("addMultipleItems", "Added {0} items to your cart.", itemsAdded.ToString());
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
                        msg += Page.GetDisplayMember("Search", _dm.DataConnectionSpec, _sd.LanguageID, "oneItemAlreadyInCart", "  One item was already in your cart.");
                    }
                    else
                    {
                        msg += Page.GetDisplayMember("Search", _dm.DataConnectionSpec, _sd.LanguageID, "allItemsAlreadyInCart", "  All available items were already in your cart.");
                    }
                }
                else
                {
                    if (itemsProcessed > 1)
                    {
                        msg += Page.GetDisplayMember("Search", _dm.DataConnectionSpec, _sd.LanguageID, "someItemsAlreadyInCart", "  {0} items were already in your cart.", itemsProcessed.ToString());
                    }
                    else
                    {
                        msg += Page.GetDisplayMember("Search", _dm.DataConnectionSpec, _sd.LanguageID, "oneItemAlreadyInCart", "  One item was already in your cart.");
                    }
                }
            }

            if (itemsNotAvail == 1)
                msgRed = Page.GetDisplayMember("Search", _dm.DataConnectionSpec, _sd.LanguageID, "oneItemNotAvailable", "  One item is not available to be put into your cart.");
            else if (itemsNotAvail > 1)
                msgRed = Page.GetDisplayMember("Search", _dm.DataConnectionSpec, _sd.LanguageID, "someItemsNotAvailable", "  {0} items not available to be put into your cart.", itemsNotAvail.ToString());

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

            doSearch("");


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

        protected bool divAdvToggleOn;
        protected bool divLatLongToggleOn;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            divAdvToggleOn = divAdvToggleOn || searchItem1.ShowControl || searchItem2.ShowControl || searchItem3.ShowControl || searchItem4.ShowControl || searchItem5.ShowControl || searchItemLL.ShowControl;
            divLatLongToggleOn = divLatLongToggleOn || searchItemLL.ShowControl;

        }

        private string getAdvancedQuery()
        {
            string s = "";
            string op = ma.Checked ? " and " : " or ";
            
            StringBuilder sb = new StringBuilder();

            if (ck1.Checked)
            {
                sb.Append(" (@inventory.is_distributable =  'Y' and @inventory.is_available = 'Y') ");
            }

            // in case SE works
            //if (ck2.Checked)
            //{
            //    if (sb.Length != 0) sb.Append(op);
            //    sb.Append(" @accession_inv_attach.category_code = 'IMAGE' ");
            //}

            //if (ck3.Checked)
            //{
            //    if (sb.Length != 0) sb.Append(op);
            //    sb.Append("  (@accession_inv_attach.category_code = 'LINK' and @accession_inv_attach.title like '%NCBI %') ");
            //}

            // handle it here when SE won't work
            if (ck2.Checked && ck3.Checked)
            {
                string sql = @"select distinct accession_id from inventory where 
                    accession_id in (select distinct accession_id from inventory i join accession_inv_attach aia on i.inventory_id = aia.inventory_id where aia.category_code = 'IMAGE')
                    and accession_id in (select distinct accession_id from inventory i join accession_inv_attach aia on i.inventory_id = aia.inventory_id where aia.category_code = 'LINK' and aia.title like '%NCBI %')";

                string sg = UserManager.GetAccessionIds(sql);
                if (sb.Length > 0) s += op;
                s += sg;
                if (sb.Length > 0) sb.Append(op);
            }
            else if (ck2.Checked)
            {
                if (sb.Length > 0) sb.Append(op);
                sb.Append(" @accession_inv_attach.category_code = 'IMAGE' ");
            }
            else if (ck3.Checked)
            {
                if (sb.Length > 0) sb.Append(op);
                sb.Append("  (@accession_inv_attach.category_code = 'LINK' and @accession_inv_attach.title like '%NCBI %') ");
            }

            if (ck4.Checked)
            {
                //string sg = "";
                //string sql = "select distinct(i.accession_id) from inventory i join genetic_observation_data god on i.inventory_id = god.inventory_id";
                //sg = UserManager.GetAccessionIds(sql);
                //if (s.Length > 0 || sb.Length > 0) s += op;
                //s += sg;
                if (sb.Length > 0) sb.Append(op);
                sb.Append("  @genetic_observation_data.genetic_observation_data_id > 0 ");
            }

            s = sb.ToString() + s;

            string sa = searchItem1.SearchCriteria(op) + searchItem2.SearchCriteria(op) + searchItem3.SearchCriteria(op) + searchItem4.SearchCriteria(op) + searchItem5.SearchCriteria(op) + searchItemLL.SearchCriteria(op);
            if (s.Length > 0 || sa.Length > 0) s += op + sa;
            if (s.Length > 3 && s.Substring(s.Length - 4, 4).ToUpper() == "AND ")
               s = s.Substring(0, s.Length - 4);
            if (s.Length > 2 && s.Substring(s.Length - 3, 3).ToUpper() == "OR ")
                s = s.Substring(0, s.Length - 3);

            return s;
        }

        protected void btnMore_Click(object sender, EventArgs e)
        {
            divAdvToggleOn = true;

            if (!pnl2.Visible)
            {
                searchItem2.Sequence = 2;
                searchItem2.bindData();
                pnl2.Visible = true;
            }
            else if (!pnl3.Visible)
            {
                searchItem3.Sequence = 3;
                searchItem3.bindData();
                pnl3.Visible = true;
            }
            else if (!pnl4.Visible)
            {
                searchItem4.Sequence = 4;
                searchItem4.bindData();
                pnl4.Visible = true;
            }
            else if (!pnl5.Visible)
            {
                searchItem5.Sequence = 5;
                searchItem5.bindData();
                pnl5.Visible = true;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            divAdvToggleOn = true;

            searchItem1.ClearCriteria();

            searchItem2.ClearCriteria();
            pnl2.Visible = false;

            searchItem3.ClearCriteria();
            pnl3.Visible = false;

            searchItem4.ClearCriteria();
            pnl4.Visible = false;

            searchItem5.ClearCriteria();
            pnl5.Visible = false;

            searchItemLL.ClearCriteria();
            searchItemLL.ShowControl = false;
        }

        private void exportToExcel(DataTable dt)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.csv", "result"));
            Response.Charset = "";
            Response.ContentType = "application/vnd.csv";

            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sWriter = new System.IO.StringWriter(sb);

            string sData;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sData = (dt.Rows[i][j].ToString());
                    if (sData == "&nbsp;") sData = "";
                    sData = "\"" + sData + "\"" + ",";
                    sWriter.Write(sData);
                }
                sWriter.WriteLine();
            }

            sWriter.Close();
            Response.Write(sb.ToString());
            Response.End();
        }

        private string getAdvancedQueryDisplay()
        {
            string s = "";
            StringBuilder sb = new StringBuilder();
            if (ck1.Checked) sb.Append(" Exclude unavilable,");
            if (ck2.Checked) sb.Append(" With images,");
            if (ck3.Checked) sb.Append(" With NCBI link,");
            if (ck4.Checked) sb.Append(" With genomic data");

            if (sb.Length > 0)
            {
                s += "&nbsp; &nbsp; &nbsp; Acessions are:" + sb.ToString();
                if (s.Substring(s.Length - 1, 1) == ",") s = s.Substring(0, s.Length - 1);
                s += "<br />";
            }

            return s + searchItem1.SearchCriteriaDisplay() + searchItem2.SearchCriteriaDisplay() + searchItem3.SearchCriteriaDisplay() + searchItem4.SearchCriteriaDisplay() + searchItem5.SearchCriteriaDisplay() + searchItemLL.SearchCriteriaDisplay();
        }

    }
}
