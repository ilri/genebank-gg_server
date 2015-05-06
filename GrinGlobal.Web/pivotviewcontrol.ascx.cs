using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using GrinGlobal.Core;
using System.Text;

namespace GrinGlobal.Web {
    public partial class PivotControl : System.Web.UI.UserControl {

        public event OnLanguageChanged LanguageChanged;

        public PivotControl() : base(){

            PageSize = 10;
            PageIndex = 0;

            AllowGrouping = true;
            AllowSorting = true;
            AllowFiltering = true;
            AllowPivoting = true;

            AllowPaging = true;
            AllowExporting = true;

            AllowColumnMovement = true;
            AllowColumnHiding = true;
            AllowRowHiding = true;

            AllowFilteringAutoComplete = true;

            AutoParseQueryString = false;

            ShowCheckBoxes = true;

            EmptyDataText = "No data was found matching your criteria.";

            LanguageID = 1;

            this.Init += new EventHandler(PivotControl_Init);
        }

        public void PivotControl_LanguageChanged(object sender, EventArgs e) {
            if (LanguageChanged != null) {
                LanguageChanged(sender, e);
            }
        }

        private ITemplate _itemTemplate = null;
        private ITemplate _alternatingItemTemplate = null;
        private ITemplate _headerTemplate = null;
        private ITemplate _footerTemplate = null;
        //private object _dataSource;

        //public string Query { get; set; }
        //public int Limit { get; set; }
        //public bool IgnoreCase { get; set; }
        //public bool MatchAll { get; set; }
        //public bool AutoParseQueryString { get; set; }

        public int PageSize {
            get { return Toolkit.ToInt32(ViewState["pagesize"], 0); }
            set {
                ViewState["pagesize"] = value;
            }
        }
        public int PageIndex {
            get {
                return Toolkit.ToInt32(ViewState["pageindex"], 0);
            }
            set {
                ViewState["pageindex"] = value;
            }
        }

        public bool AllowPivoting {
            get {
                return Toolkit.ToBoolean(ViewState["allowpivoting"], false);
            }
            set {
                ViewState["allowpivoting"] = value;
            }
        }

        public bool AllowPaging {
            get {
                return Toolkit.ToBoolean(ViewState["allowpaging"], false);
            }
            set {
                ViewState["allowpaging"] = value;
            }
        }

        public bool AllowSorting {
            get {
                return Toolkit.ToBoolean(ViewState["allowsorting"], false);
            }
            set {
                ViewState["allowsorting"] = value;
            }
        }

        public bool AllowColumnMovement {
            get {
                return Toolkit.ToBoolean(ViewState["allowcolumnmovement"], false);
            }
            set {
                ViewState["allowcolumnmovement"] = value;
            }
        }

        public bool AllowColumnHiding {
            get {
                return Toolkit.ToBoolean(ViewState["allowcolumnhiding"], false);
            }
            set {
                ViewState["allowcolumnhiding"] = value;
            }
        }

        public bool AllowExporting {
            get {
                return Toolkit.ToBoolean(ViewState["allowexporting"], false);
            }
            set {
                ViewState["allowexporting"] = value;
            }
        }

        public bool AllowFiltering {
            get {
                return Toolkit.ToBoolean(ViewState["allowfiltering"], false);
            }
            set {
                ViewState["allowfiltering"] = value;
            }
        }

        public bool AllowRowHiding {
            get {
                return Toolkit.ToBoolean(ViewState["allowrowhiding"], false);
            }
            set {
                ViewState["allowrowhiding"] = value;
            }
        }

        public bool AllowGrouping {
            get {
                return Toolkit.ToBoolean(ViewState["allowgrouping"], false);
            }
            set {
                ViewState["allowgrouping"] = value;
            }
        }

        public bool AllowFilteringAutoComplete {
            get {
                return Toolkit.ToBoolean(ViewState["allowfilteringautocomplete"], false);
            }
            set {
                ViewState["allowfilteringautocomplete"] = value;
            }
        }

        public string EmptyDataText {
            get {
                return ViewState["emptydatatext"] as string;
            }
            set {
                ViewState["emptydatatext"] = value;
            }
        }

        public int Limit {
            get {
                return Toolkit.ToInt32(ViewState["limit"], 0);
            }
            set {
                ViewState["limit"] = value;
            }
        }

        public int SkipCount {
            get {
                return Toolkit.ToInt32(ViewState["skipcount"], 0);
            }
            set {
                ViewState["skipcount"] = value;
            }
        }

        public bool AutoParseQueryString {
            get {
                return Toolkit.ToBoolean(ViewState["autoparsequerystring"], false);
            }
            set {
                ViewState["autoparsequerystring"] = value;
            }
        }

        public bool ShowCheckBoxes {
            get {
                return Toolkit.ToBoolean(ViewState["showcheckboxes"], false);
            }
            set {
                ViewState["showcheckboxes"] = value;
            }
        }

        public int[] PrimaryKeys {
            get {
                return ViewState["primarykeys"] as int[];
            }
            private set {
                ViewState["primarykeys"] = value;
            }
        }
        public int LanguageID
        {
            get { return Toolkit.ToInt32(ViewState["languageid"], 0); }
            set
            {
                ViewState["languageid"] = value;
            }
        }

        void PivotControl_Init(object sender, EventArgs e) {

            // jquery core is already included in our site1.master.
            // jquery ui is really large (189k) so we didn't want to put it there.
            // this will register a script include so if this control is used multiple times on a page the script is only downloaded once.
            if (Toolkit.IsNonWindowsOS) {
                Page.ClientScript.RegisterClientScriptInclude("jquery.ui", Page.ResolveClientUrl("/gringlobal/scripts/jquery-ui-1.7.2.custom.min.js"));
                Page.ClientScript.RegisterClientScriptInclude("jquery.ui.autocomplete", Page.ResolveClientUrl("/gringlobal/scripts/jquery.autocomplete.js"));
                Page.ClientScript.RegisterClientScriptInclude("pivoter", Page.ResolveClientUrl("/gringlobal/scripts/pivoter.js"));
            } else {
                ScriptManager.RegisterClientScriptInclude(this, typeof(string), "jquery.ui", Page.ResolveClientUrl("~/scripts/jquery-ui-1.7.2.custom.min.js"));
                ScriptManager.RegisterClientScriptInclude(this, typeof(string), "jquery.ui.autocomplete", Page.ResolveClientUrl("~/scripts/jquery.autocomplete.js"));
                ScriptManager.RegisterClientScriptInclude(this, typeof(string), "pivoter", Page.ResolveClientUrl("~/scripts/pivoter.js"));
            }

            ((Site1)this.Page.Master).LanguageChanged += new OnLanguageChanged(PivotControl_LanguageChanged);


            if (IsPostBack) {
                string idList = Request.Params["pivotHiddenPKList"];

                if (!String.IsNullOrEmpty(idList)) {
                    PrimaryKeys = Toolkit.ToIntList(idList).ToArray();
                } else {
                    PrimaryKeys = new int[] { };
                }
            } else {
                PrimaryKeys = new int[]{};

                if (AutoParseQueryString) {
                    Limit = Toolkit.ToInt32(Request.QueryString["lim"], Limit);
                    SkipCount = Toolkit.ToInt32(Request.QueryString["skip"], SkipCount);

                    string pg = Request.QueryString["ps"];
                    if (pg == "all" || pg == "0" || pg == "-1" || pg == "unlimited") {
                        pg = "2000000000";
                    }
                    PageSize = Toolkit.ToInt32(pg, PageSize);
                    PageIndex = Toolkit.ToInt32(Request.QueryString["pg"], PageIndex);

                }
            }
        }

        #region DataBinding / Templating

        //[TemplateContainer(typeof(SimpleTemplateItem))]
        //public ITemplate ItemTemplate {
        //    get;
        //    set;
        //}

        //[TemplateContainer(typeof(SimpleTemplateItem))]
        //public ITemplate AlternatingItemTemplate {
        //    get;
        //    set;
        //}

        //[TemplateContainer(typeof(SimpleTemplateItem))]
        //public ITemplate HeaderTemplate {
        //    get;
        //    set;
        //}

        //[TemplateContainer(typeof(SimpleTemplateItem))]
        //public ITemplate FooterTemplate {
        //    get;
        //    set;
        //}

        public string PrimaryKeyName {
            get {
                return ViewState["primarykeyname"] as string;
            }
            set {
                ViewState["primarykeyname"] = value;
            }
        }

        public string AlternateKeyName {
            get {
                return ViewState["alternatekeyname"] as string;
            }
            set {
                ViewState["alternatekeyname"] = value;
            }
        }

        private object _dataSource;
        public object DataSource {
            get {
                return _dataSource;
            }
            set {
                _dataSource = value; 
            }
        }

        public string AllLabel {
            get {
                return ViewState["alllabel"] as string;
            }
            set {
                ViewState["alllabel"] = value;
            }
        }

        private string dataSourceAsJavascript {
            get {
                if (DataSource != null){
                    if (DataSource is DataTable) {
                        // convert to json

                        string allLabel = Utils.GetWebAppResource("WebTool", "pv");
                        LanguageID = Utils.GetLanguageID();

                        DataTable dt = DataSource as DataTable;
                        return "pivotView.initData(" + dt.ToJson(PrimaryKeyName, AlternateKeyName, _headerTemplate, _itemTemplate, _alternatingItemTemplate, _footerTemplate) + ", initPivotView" + ",'" + (LanguageID + allLabel) + "');";

                    } else {
                        throw new NotSupportedException("DataSource must be a DataTable object");
                    }
                } else {
                    // nothing to bind, just signify loading is done
                    return "initPivotView();";
                }
            }
        }

        public override void DataBind() {

            // yeah lots to do here.
            // need to let the page author define Item Templates... maybe?  Since grouping is all client side this will be tricky at best.
            litData.Text = dataSourceAsJavascript;

            litPivoter.Text = String.Format(@"
            pivotView.changePageSize({0});
            pivotView.changePageTo({1});
            pivotView.setOptions({{
                allowPivoting : {2},
                allowSorting : {3},
                allowPaging : {4},
                allowColumnMovement : {5},
                allowRowHiding : {6},
                allowColumnHiding : {7},
                allowExporting : {8},
                allowFiltering : {9},
                allowFilteringAutoComplete : {10},
                allowGrouping : {11},
                showCheckBoxes : {12},
                root : '{13}',
                selector : '#{14}',
                emptyDataText : '{15}',
                languageID: '{16}'
            }});
            pivotView.refresh();
", 
                 this.PageSize, 
                 this.PageIndex, 
                 this.AllowPivoting.ToString().ToLower(), 
                 this.AllowSorting.ToString().ToLower(), 
                 this.AllowPaging.ToString().ToLower(), 
                 this.AllowColumnMovement.ToString().ToLower(), 
                 this.AllowRowHiding.ToString().ToLower(),
                 this.AllowColumnHiding.ToString().ToLower(),
                 this.AllowExporting.ToString().ToLower(),
                 this.AllowFiltering.ToString().ToLower(),
                 this.AllowFilteringAutoComplete.ToString().ToLower(),
                 this.AllowGrouping.ToString().ToLower(),
                 this.ShowCheckBoxes.ToString().ToLower(),
                 Page.ResolveUrl("~/"),
                 this.ClientID,
                 ("" + this.EmptyDataText).Replace("'", @"\'"),
                 this.LanguageID
             );

            // base.DataBind();
        }

        #endregion DataBinding / Templating

    }
}