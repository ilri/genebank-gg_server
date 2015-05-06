using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using System.Data;
using System.Diagnostics;
using GrinGlobal.Business;
using System.Text;

namespace GrinGlobal.Web {
    public partial class Descriptors : System.Web.UI.Page {


        public string DefaultDisplayPanel() 
        {
            if (pnlResult.Visible){
                if (divAdvToggleOn)
                    return "panel-select";
                else
                    return "panel-results";
            } else if (pnlSelectedTraits.Visible){
                return "panel-select";
            } else if (pnlTraits.Visible){
                return "panel-descriptor";
            } else {
                return "panel-crop";
            }
        }

        protected void Page_Load(object sender, EventArgs e) 
        {
            if (!IsPostBack) 
            {
                initBind();
            }
        }

        private void initBind() 
        {
//            using (var dm = DataManager.Create()) {
//                ddlCrops.DataSource = dm.Read(@"
//select
//    crop_id,
//    name
//from
//    crop
//order by
//    name
//");
//                ddlCrops.DataBind();
//            }
            using (var sd = UserManager.GetSecureData(true))
            {
                var dt = sd.GetData("web_descriptorbrowse_crop", "", 0, 0).Tables["web_descriptorbrowse_crop"];
                var drNone = dt.NewRow();
                drNone["name"] = " -- Select One -- ";
                drNone["crop_id"] = "-1";
                dt.Rows.InsertAt(drNone, 0);
                ddlCrops.DataSource = dt;
                ddlCrops.DataBind();
            }

            searchItem1.LoadData = true;
        }

        private void showTraits(int cropID) 
        {
//            using (var sd = UserManager.GetSecureData(true)) {
//                using (var dm = DataManager.Create(sd.DataConnectionSpec)) {
//                    var dt = dm.Read(@"
//select 
//    ct.crop_id,
//    ct.crop_trait_id, 
//    ct.category_code,
//    ct.coded_name,
//    concat(ct.category_code, ' - ', coalesce(ctl.title, ct.coded_name)) as display_text
//from 
//    crop_trait ct left join crop_trait_lang ctl
//        on ct.crop_trait_id = ctl.crop_trait_id
//        and ctl.sys_lang_id = :langid
//where 
//    ct.category_code = coalesce(:cat, ct.category_code)
//    and ct.crop_id = :cropid
//order by  
//    ct.category_code,
//    coalesce(ctl.title, ct.coded_name)
//", new DataParameters(
//        ":langid", sd.LanguageID, DbType.Int32,
//        ":cat", null, DbType.String,
//        ":cropid", cropID, DbType.Int32));
          
           using (var sd = UserManager.GetSecureData(true))
           {
                   _sd = sd;
                    //var dt = sd.GetData("web_descriptorbrowse_trait", ":langid=" + sd.LanguageID + ";:cat=" + null + ";:cropid=" + cropID, 0, 0).Tables["web_descriptorbrowse_trait"];
                   var dt = sd.GetData("web_descriptorbrowse_trait_category", ":langid=" + sd.LanguageID + ";:cat=" + null + ";:cropid=" + cropID, 0, 0).Tables["web_descriptorbrowse_trait_category"];

                    pnlTraits.Visible = true;
                    pnlSelectedTraits.Visible = false;
                    pnlPassport.Visible = false;
                    divAdvToggleOn = false;
                    pnlResult.Visible = false;
                    dlListTraits.DataSource = dt;
                    dlListTraits.DataBind();
                    //chkListTraits.DataTextField = "display_text";
                    //chkListTraits.DataValueField = "crop_trait_id";
                    //chkListTraits.DataSource = dt;
                    //chkListTraits.DataBind();

                    if (cropID > 0)
                        lblCrop.Text = "<a href='crop.aspx?id=" + cropID + "'>" + ddlCrops.SelectedItem.Text + "</a>";
                    else
                        lblCrop.Text = "";
                     
           }
     }

        protected void ddlCrops_SelectedIndexChanged(object sender, EventArgs e) 
        {
            showTraits(Toolkit.ToInt32(ddlCrops.SelectedValue, -1));
        }

        private List<int> getSelectedTraits() 
        {
            var rv = new List<int>();
            foreach (DataListItem dli in dlListTraits.Items) 
            {
                var clt = dli.FindControl("chkListTraits") as CheckBoxList;
                if (clt != null) {
                    foreach (ListItem li in clt.Items) {
                        if (li.Selected) {
                            rv.Add(Toolkit.ToInt32(li.Value, -1));
                        }
                    }
                }
            }
            return rv;
        }

        protected void btnSelectTraits_Click(object sender, EventArgs e) 
        {
            var cropTraitIDs = getSelectedTraits();

            using (var sd = UserManager.GetSecureData(true))
            {
                _sd = sd;
                using (var dm = DataManager.Create(sd.DataConnectionSpec))
                {
                    _dm = dm;
                    //foreach (var ctid in cropTraitIDs) {

                    // get the list of trait names, bind to the SelectedTraits repeater
                    // for each of those, create a nested TraitValues repeater

                    //                        var dt = dm.Read(@"
                    //select
                    //    coalesce(ctl.title, ct.coded_name) as crop_trait_name,
                    //    ct.crop_trait_id
                    //    , count(i.accession_id) as accession_count
                    //from
                    //    crop_trait ct
                    //    left join crop_trait_lang ctl
                    //        on ct.crop_trait_id = ctl.crop_trait_id
                    //        and ctl.sys_lang_id = :langid1
                    //    left join crop_trait_observation cto
                    //        on ct.crop_trait_id = cto.crop_trait_id
                    //    left join inventory i
                    //        on cto.inventory_id = i.inventory_id
                    //where
                    //    ct.crop_trait_id in (:traitids)
                    //group by
                    //    ct.crop_trait_id,
                    //    coalesce(ctl.title, ct.coded_name)
                    //order by
                    //    1
                    //", new DataParameters(
                    //     ":langid1", sd.LanguageID, DbType.Int32,
                    //     ":traitids", cropTraitIDs, DbPseudoType.IntegerCollection
                    //     ));

                    var dt = sd.GetData("web_descriptorbrowse_traitselect", ":langid=" + sd.LanguageID + ";:traitids=" + Toolkit.Join(cropTraitIDs.ToArray(), ",", ""), 0, 0).Tables["web_descriptorbrowse_traitselect"];
                    pnlSelectedTraits.Visible = true;
                    pnlPassport.Visible = true;
                    divAdvToggleOn = false;
                    pnlResult.Visible = false;
                    dlSelectedTraits.ItemDataBound += new DataListItemEventHandler(dlSelectedTraits_ItemDataBound);
                    dlSelectedTraits.DataSource = dt;
                    dlSelectedTraits.DataBind();
                    //}

                    dt = sd.GetData("web_descriptorbrowse_traitcodelength", ":langid=" + sd.LanguageID + ";:traitids=" + Toolkit.Join(cropTraitIDs.ToArray(), ",", ""), 0, 0).Tables["web_descriptorbrowse_traitcodelength"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int maxLen = Toolkit.ToInt32(dt.Rows[0]["maxLen"].ToString(), 0);
                        if (maxLen > 55)
                            dlSelectedTraits.RepeatColumns = 1;
                        else if (maxLen > 30)
                            dlSelectedTraits.RepeatColumns = 2;
                        else
                            dlSelectedTraits.RepeatColumns = 3;
                    }
                }
            }
        }

        // HACK: since we can't pass the datamanger via a repeater.ItemDataBound event...
        DataManager _dm;
        SecureData _sd;

        void dlSelectedTraits_ItemDataBound(object sender, DataListItemEventArgs e) 
        {

            var lst = e.Item.FindControl("lstValues") as ListBox;
            if (lst == null) 
            {
                return;
            }

            var cropTraitID = Toolkit.ToInt32((e.Item.DataItem as DataRowView).Row["crop_trait_id"], -1);

            bindByValue(lst, cropTraitID);
            // bindByValueWithCounts(lst, cropTraitID);
        }

        private void bindByValue(ListBox lst, int cropTraitID) 
        {
//            var dt = _dm.Read(@"
//select
//	case when cto.crop_trait_code_id is not null then
//		coalesce(ctcl.title, ctc.code)
//	else
//		coalesce(string_value, convert(nvarchar, numeric_value))
//	end as crop_trait_code_text,
//    case when cto.crop_trait_code_id is not null then
//        concat('crop_trait_code_id|', convert(nvarchar, cto.crop_trait_code_id))
//    when cto.string_value is not null then
//        concat('string_value|', cto.string_value)
//    else
//        concat('numeric_value|', convert(nvarchar, cto.numeric_value))
//    end as field_name_value
//from
//    crop_trait_observation cto
//    inner join crop_trait ct
//        on cto.crop_trait_id = ct.crop_trait_id
//    left join crop_trait_lang ctl
//        on ct.crop_trait_id = ctl.crop_trait_id
//        and ctl.sys_lang_id = :langid1
//    left join crop_trait_code ctc
//        on cto.crop_trait_code_id = ctc.crop_trait_code_id
//    left join crop_trait_code_lang ctcl
//        on ctc.crop_trait_code_id = ctcl.crop_trait_code_id
//        and ctcl.sys_lang_id = :langid2
//    left join inventory i 
//        on cto.inventory_id = i.inventory_id
//    left join accession a
//        on i.accession_id = a.accession_id
//where
//    cto.crop_trait_id = :traitid
//group by
//	case when cto.crop_trait_code_id is not null then
//		coalesce(ctcl.title, ctc.code)
//	else
//		coalesce(string_value, convert(nvarchar, numeric_value))
//	end,
//    case when cto.crop_trait_code_id is not null then
//        concat('crop_trait_code_id|', convert(nvarchar, cto.crop_trait_code_id))
//    when cto.string_value is not null then
//        concat('string_value|', cto.string_value)
//    else
//        concat('numeric_value|', convert(nvarchar, cto.numeric_value))
//    end
//order by
//	1, 2
//", new DataParameters(
//":langid1", _sd.LanguageID, DbType.Int32,
//":langid2", _sd.LanguageID, DbType.Int32,
//":traitid", cropTraitID, DbType.Int32));

            var dt = _sd.GetData("web_descriptorbrowse_traitselectrange", ":langid1=" + _sd.LanguageID + ";:langid2=" + + _sd.LanguageID + ";:traitid=" + cropTraitID, 0, 0).Tables["web_descriptorbrowse_traitselectrange"];
            if (dt.Rows.Count == 0) dt = _sd.GetData("web_descriptorbrowse_traitselectrange_n", ":langid1=" + _sd.LanguageID + ";:langid2=" + +_sd.LanguageID + ";:traitid=" + cropTraitID, 0, 0).Tables["web_descriptorbrowse_traitselectrange_N"];
            lst.DataSource = dt;
            lst.DataBind();
            if (lst.Items.Count > 0)  lst.SelectedIndex = 0;
        }

        private void bindByValueWithCounts(ListBox lst, int cropTraitID)
        {
            var dt = _dm.Read(@"
select
	concat(case when cto.crop_trait_code_id is not null then
		coalesce(ctcl.title, ctc.code)
	else
		coalesce(string_value, convert(nvarchar, numeric_value))
	end, ' (__###__)') as crop_trait_code_text,
    coalesce(convert(nvarchar, cto.crop_trait_code_id), string_value, convert(nvarchar, numeric_value)) as crop_trait_code_value,
    count(a.accession_id) as accession_count
from
    crop_trait_observation cto
    inner join crop_trait ct
        on cto.crop_trait_id = ct.crop_trait_id
    left join crop_trait_lang ctl
        on ct.crop_trait_id = ctl.crop_trait_id
        and ctl.sys_lang_id = :langid1
    left join crop_trait_code ctc
        on cto.crop_trait_code_id = ctc.crop_trait_code_id
    left join crop_trait_code_lang ctcl
        on ctc.crop_trait_code_id = ctcl.crop_trait_code_id
        and ctcl.sys_lang_id = :langid2
    left join inventory i 
        on cto.inventory_id = i.inventory_id
    left join accession a
        on i.accession_id = a.accession_id
where
    cto.crop_trait_id = :traitid
group by
	concat(case when cto.crop_trait_code_id is not null then
		coalesce(ctcl.title, ctc.code)
	else
		coalesce(string_value, convert(nvarchar, numeric_value))
	end, ' (__###__)'),
    coalesce(convert(nvarchar, cto.crop_trait_code_id), string_value, convert(nvarchar, numeric_value))
order by
	1, 2, 3
", new DataParameters(
":langid1", _sd.LanguageID, DbType.Int32,
":langid2", _sd.LanguageID, DbType.Int32,
":traitid", cropTraitID, DbType.Int32));

            // HACK: we need to put a count in the text, but you can't group by a count...
            foreach (DataRow dr in dt.Rows) {
                dr["crop_trait_code_text"] = dr["crop_trait_code_text"].ToString().Replace("__###__", dr["accession_count"].ToString());
            }

            lst.DataSource = dt;
            lst.DataBind();

        }

        public void PivotView_LanguageChanged(object sender, EventArgs e) 
        {
            doSearch();
        }

        protected void btnSearch_Click(object sender, EventArgs e) 
        {
            doSearch();
        }

/*
        private void doSearch(){
            using (var sd = UserManager.GetSecureData(true)) {
//                using (var dm = DataManager.Create(sd.DataConnectionSpec))
//                {
//                    var sql = @"
//select
//	a.accession_id,
//	concat(coalesce(a.accession_number_part1,''), ' ', coalesce(convert(nvarchar, a.accession_number_part2), ''), ' ', coalesce(a.accession_number_part3,'')) as accession_number,
//	coalesce(ctl.title, ct.coded_name) as crop_trait_name,
//    coalesce(coalesce(ctcl.title, ctc.code, convert(nvarchar, cto.crop_trait_id)), string_value, convert(nvarchar, numeric_value)) as value
//from
//    crop_trait_observation cto
//    inner join crop_trait ct
//        on cto.crop_trait_id = ct.crop_trait_id
//    left join crop_trait_lang ctl
//        on ct.crop_trait_id = ctl.crop_trait_id
//        and ctl.sys_lang_id = :langid1
//    left join crop_trait_code ctc
//        on cto.crop_trait_code_id = ctc.crop_trait_code_id
//    left join crop_trait_code_lang ctcl
//        on ctc.crop_trait_code_id = ctcl.crop_trait_code_id
//        and ctcl.sys_lang_id = :langid2
//    left join inventory i 
//        on cto.inventory_id = i.inventory_id
//    left join accession a
//        on i.accession_id = a.accession_id
//where
//    {0}
//order by
//	1, 2, 3
//";

//                    var dps = new DataParameters();
//                    dps.Add(new DataParameter(":langid1", sd.LanguageID, DbType.Int32));
//                    dps.Add(new DataParameter(":langid2", sd.LanguageID, DbType.Int32));

                    var whereTemplate = " (cto.crop_trait_id = :traitid{0} and cto.{1} {2}) ";

                    var sbWhere = new StringBuilder();

                    // TODO: spin through DataList controls, grab selected values and comparison operators
                    //       create where clause and dataparameters as needed
                    var anyValueTemplate = " OR (cto.crop_trait_id in (:anytraitids))";
                    var anyValues = new List<int>();
                    for(var i=0; i < dlSelectedTraits.Items.Count; i++){
                        var it = dlSelectedTraits.Items[i];
                        var lst = it.FindControl("lstValues") as ListBox;
                        var ddlOp = it.FindControl("ddlOperator") as DropDownList;
                        var ctID = Toolkit.ToInt32(dlSelectedTraits.DataKeys[i], -1);
                        var op = ddlOp.SelectedValue;
                        var comparator = "";
                        var fieldName = "";
                        if (String.IsNullOrEmpty(op)) {
                            anyValues.Add(ctID);
                        } else {
                            var selected = new List<string>();
                            foreach (ListItem li in lst.Items) {
                                if (li.Selected) {
                                    var split = li.Value.Split('|');
                                    fieldName = split[0];
                                    selected.Add(split[1]);
                                }
                            }
                            switch(op){
                                case "EQ":
                                    comparator = " IN (:value{0}) ";
                                    break;
                                case "NEQ":
                                    comparator = " NOT IN (:value{0}) ";
                                    break;
                                case "GT":
                                    comparator = " > :value{0} ";
                                    break;
                                case "LT":
                                    comparator = " < :value{0} ";
                                    break;
                            }

                            if (sbWhere.Length > 0) {
                                sbWhere.Append(" AND ");
                            }
                            var newWhere = String.Format(whereTemplate, i, fieldName, comparator);
                            newWhere = String.Format(newWhere, i);
                            //sbWhere.Append(newWhere);

                            //dps.Add(new DataParameter(":traitid" + i, ctID, DbType.Int32));
                            newWhere = newWhere.Replace(":traitid" + i, ctID.ToString());
                            if (selected.Count == 1) {
                                if (fieldName == "crop_trait_code_id"){
                                    //dps.Add(new DataParameter(":value" + i, Toolkit.ToInt32(selected[0], -1), DbType.Int32));
                                    newWhere = newWhere.Replace(":value" + i, selected[0]);
                                } else if (fieldName == "string_value"){
                                    //dps.Add(new DataParameter(":value" + i, selected[0], DbType.String));
                                    newWhere = newWhere.Replace(":value" + i, "'" + selected[0] + "'");
                                } else if (fieldName == "numeric_value"){
                                    //dps.Add(new DataParameter(":value" + i, Toolkit.ToDecimal(selected[0], -1.0M), DbType.Decimal));
                                    newWhere = newWhere.Replace(":value" + i, selected[0]);
                                }
                            } else {
                                if (fieldName == "crop_trait_code_id"){
                                    //dps.Add(new DataParameter(":value" + i, Toolkit.ToIntList(String.Join(" ", selected.ToArray())), DbPseudoType.IntegerCollection));
                                    newWhere = newWhere.Replace(":value" + i, Toolkit.Join(selected.ToArray(), ",", ""));
                                } else if (fieldName == "string_value"){
                                    //dps.Add(new DataParameter(":value" + i, selected, DbPseudoType.StringCollection));
                                    newWhere = newWhere.Replace(":value" + i, Toolkit.Join(selected.ToArray(),  ",", "", "'"));
                                } else if (fieldName == "numeric_value"){
                                    //dps.Add(new DataParameter(":value" + i, Toolkit.ToDecimalList(String.Join(" ", selected.ToArray())), DbPseudoType.DecimalCollection));
                                    newWhere = newWhere.Replace(":value" + i, Toolkit.Join(selected.ToArray(), ",", ""));
                                }
                                //dps.Add(new DataParameter(":value" + i, selected, DbPseudoType.StringCollection));
                            }
                            sbWhere.Append(newWhere);
                        }
                    }

                    if (sbWhere.ToString() == "") {
                        if (anyValues.Count > 0) {
                            anyValueTemplate =  anyValueTemplate.Replace(":anytraitids", Toolkit.Join(anyValues.ToArray(), ",", ""));
                            sbWhere.Append(anyValueTemplate);
                            //dps.Add(new DataParameter(":anytraitids", anyValues, DbPseudoType.IntegerCollection));
                        }
                    }

                    var where = sbWhere.ToString();
                    if (where.StartsWith(" OR ")) {
                        where = Toolkit.Cut(where, 4);
                    }

                    //sql = String.Format(sql, where);

                    //var dt = dm.Read(sql, dps);
                    var dt = sd.GetData("web_descriptorbrowse_search", ":langid1=" + sd.LanguageID + ";:langid2=" + sd.LanguageID + ";:whereclause=" + where, 0, 0).Tables["web_descriptorbrowse_search"];
                    var dt2 = dt.Transform(new string[] { "accession_number", "accession_id" }, "crop_trait_name", "crop_trait_name", "value");
                    ggPivotView.DataSource = dt2;
                    ggPivotView.PrimaryKeyName = "accession_id";
                    ggPivotView.AlternateKeyName = "accession_number";
                    ggPivotView.DataBind();
                    pnlResult.Visible = true;

            }
        } 
 */
  
        protected void dlListTraits_ItemDataBound(object sender, DataListItemEventArgs e) 
        {
            var chklist = e.Item.FindControl("chkListTraits") as CheckBoxList;
            if (chklist != null) 
            {
                var categoryCode = dlListTraits.DataKeys[e.Item.ItemIndex] as string;
                if (!String.IsNullOrEmpty(categoryCode)) 
                {
                    var cropID = Toolkit.ToInt32(ddlCrops.SelectedValue, -1);
                    var dt = _sd.GetData("web_descriptorbrowse_trait", ":langid=" + _sd.LanguageID + ";:cat=" + categoryCode +";:cropid=" + cropID, 0, 0).Tables["web_descriptorbrowse_trait"];
                    chklist.DataSource = dt;
                    chklist.DataBind();

                    string categoryCode1 = categoryCode.ToLower();
                    categoryCode1 = categoryCode1.Substring(0,1).ToUpper() + categoryCode1.Substring(1, categoryCode1.Length-1);

                    var btnCheckAll = e.Item.FindControl("btnCheckAllDesc") as Button;
                    if (btnCheckAll != null)
                    {
                        btnCheckAll.Text = "Choose All " + categoryCode1 + " Descriptors";
                        btnCheckAll.CommandName = "Add";
                        btnCheckAll.CommandArgument = cropID + ";" + categoryCode;
                    }

                    var btnClearAll = e.Item.FindControl("btnClearAllDesc") as Button;
                    if (btnClearAll != null)
                    {
                        btnClearAll.Text = "Clear All " + categoryCode1 + " Descriptors";
                        btnClearAll.CommandName = "Remove";
                        btnClearAll.CommandArgument = cropID + ";" + categoryCode;
                    }
                }
            }
        }

        protected void dlListTraits_ItemCommand(object source, DataListCommandEventArgs e)
        {
            string Name = e.CommandName;
            string[] arg = e.CommandArgument.ToString().Split(';');

            using (var sd = UserManager.GetSecureData(true))
            {
                var dt = sd.GetData("web_descriptorbrowse_trait", ":langid=" + sd.LanguageID + ";:cat=" + arg[1] + ";:cropid=" + arg[0], 0, 0).Tables["web_descriptorbrowse_trait"];

                var rv = new List<int>();
                foreach (DataRow dr in dt.Rows)
                {
                    string id = dr[1].ToString();
                    foreach (DataListItem dli in dlListTraits.Items)
                    {
                        var clt = dli.FindControl("chkListTraits") as CheckBoxList;
                        if (clt != null)
                        {
                            foreach (ListItem li in clt.Items)
                            {
                                if (li.Value == id)
                                {
                                    if (Name == "Add")
                                        li.Selected = true;
                                    else
                                        li.Selected = false;
                                }
                            }
                        }
                    }
                }
            }
        }

    
       private void doSearch()
       {
            using (var sd = UserManager.GetSecureData(true)) 
            {
                using (var dm = DataManager.Create(sd.DataConnectionSpec))
                {
                    string dbEngine = dm.DataConnectionSpec.EngineName.ToUpper();
                    var sqlSelect = "";

                    if (dbEngine == "ORACLE")
                    {
                        sqlSelect = @"
                        select
	                        a.accession_id, 
	                        '<a href=""~/accessiondetail.aspx?id=' || cast(a.accession_id as varchar2(50)) || '"">' || concat(coalesce(a.accession_number_part1,'') || ' ' || coalesce(cast(a.accession_number_part2 as varchar2(50)), '') || ' ' || coalesce(a.accession_number_part3,'')) || '</a>' as accession_number,
	                        coalesce(ctl.title, ct.coded_name) as crop_trait_name,
                            coalesce(coalesce(ctcl.title, ctc.code, cast(cto.crop_trait_code_id as varchar2(50))), string_value, cast(numeric_value as varchar2(50))) as value ";
                    }
                    else if (dbEngine == "MYSQL")
                    {
                        sqlSelect = @"
                        select
	                        a.accession_id as 'Accession ID',
	                        concat('<a href=""~/accessiondetail.aspx?id=', convert(a.accession_id, char), '"">', concat(coalesce(a.accession_number_part1,''), ' ', coalesce(convert(a.accession_number_part2, char), ''), ' ', coalesce(a.accession_number_part3,'')), '</a>') as 'Plant ID',
	                        coalesce(ctl.title, ct.coded_name) as crop_trait_name,
                            coalesce(coalesce(ctcl.title, ctc.code, convert(cto.crop_trait_code_id, char)), string_value, convert(numeric_value, char)) as value ";
                    }
                    else if (dbEngine == "POSTGRESQL")
                    {
                        sqlSelect = @"
                        select
	                        a.accession_id,
	                        concat('<a href=""~/accessiondetail.aspx?id=', cast(a.accession_id as varchar), '"">', concat(coalesce(a.accession_number_part1,''), ' ', coalesce(cast(a.accession_number_part2 as varchar), ''), ' ', coalesce(a.accession_number_part3,'')), '</a>') as accession_number,
	                        coalesce(ctl.title, ct.coded_name) as crop_trait_name,
                            coalesce(coalesce(ctcl.title, ctc.code, cast(cto.crop_trait_code_id as varchar)), string_value, cast(numeric_value as varchar)) as value ";
                    }
                    else
                    {
                        sqlSelect = @"
                        select
	                        a.accession_id as 'Accession ID',
	                        '<a href=""~/accessiondetail.aspx?id=' + convert(nvarchar, a.accession_id) + '"">' + concat(coalesce(a.accession_number_part1,''), ' ', coalesce(convert(nvarchar, a.accession_number_part2), ''), ' ', coalesce(a.accession_number_part3,'')) + '</a>' as 'Plant ID',
	                        coalesce(ctl.title, ct.coded_name) as crop_trait_name,
                            coalesce(coalesce(ctcl.title, ctc.code, convert(nvarchar, cto.crop_trait_code_id)), string_value, convert(nvarchar, cast(numeric_value as float))) as value ";
                    }
                    var sql = sqlSelect + @"
from
    crop_trait_observation cto
    inner join crop_trait ct
        on cto.crop_trait_id = ct.crop_trait_id
    left join crop_trait_lang ctl
        on ct.crop_trait_id = ctl.crop_trait_id
        and ctl.sys_lang_id = :langid1
    left join crop_trait_code ctc
        on cto.crop_trait_code_id = ctc.crop_trait_code_id
    left join crop_trait_code_lang ctcl
        on ctc.crop_trait_code_id = ctcl.crop_trait_code_id
        and ctcl.sys_lang_id = :langid2
    left join inventory i 
        on cto.inventory_id = i.inventory_id
    left join accession a
        on i.accession_id = a.accession_id
where ";

//                    var sqlWhereIDs = @"
//and a.accession_id in  (
//    select  a.accession_id from accession a join inventory i on a.accession_id = i.accession_id 
//    where ";
                    var sqlWhereIDs = @" and a.accession_id in (select accession_id from inventory where ";
  
//                    var sqlWhereIDs_a = @"
//and a.accession_id in  (
//    select  a.accession_id from accession a join inventory i on a.accession_id = i.accession_id 
//    where ";
                    var sqlWhereIDs_a = @" a.accession_id in ( select accession_id from inventory where ";

                    var sbSqlID_a = new StringBuilder();
                    var sbInfor = new StringBuilder();

                    var sqlID = @" inventory_id in (
            select inventory_id   
            from crop_trait_observation cto where ";

                    var whereTemplate = " (cto.is_archived = 'N' and cto.crop_trait_id = :traitid{0} and cto.{1} {2}) ";

                    var sbWhere = new StringBuilder();
                    var sbSqlID = new StringBuilder();
                    var sbSqlStatement = new StringBuilder();

                    bool joinType = (rblMatch.SelectedValue  == "all");
                    bool hasValue = cbValue.Checked;

                    // TODO: spin through DataList controls, grab selected values and comparison operators
                    //       create where clause and dataparameters as needed
                    var anyValueTemplate = " (cto.crop_trait_id in (:anytraitids))";
                    var anyValues = new List<int>();
                    var allTraits = new List<int>();

                    for (var i = 0; i < dlSelectedTraits.Items.Count; i++)
                    {
                        var it = dlSelectedTraits.Items[i];
                        var lst = it.FindControl("lstValues") as ListBox;
                        var ddlOp = it.FindControl("ddlOperator") as DropDownList;
                        var ctID = Toolkit.ToInt32(dlSelectedTraits.DataKeys[i], -1);
                        var op = ddlOp.SelectedValue;
                        var comparator = "";
                        var fieldName = "";

                        var dtTrait = sd.GetData("web_descriptorbrowse_traitselect", ":langid=" + sd.LanguageID + ";:traitids=" + ctID, 0, 0).Tables["web_descriptorbrowse_traitselect"];
                        string traitName = dtTrait.Rows[0]["crop_trait_name"].ToString();

                        allTraits.Add(ctID);

                        if (String.IsNullOrEmpty(op))
                        {
                            anyValues.Add(ctID);
                            sbInfor.Append("&nbsp; &nbsp; &nbsp;").Append(traitName).Append(" Equal To ALL VALUES;").Append("<br />");
                        }
                        else
                        {
                            var selected = new List<string>();
                            var selectedValue = "";
                            foreach (ListItem li in lst.Items)
                            {
                                if (li.Selected)
                                {
                                    var split = li.Value.Split('|');
                                    fieldName = split[0];
                                    selected.Add(split[1].Replace("'", "''"));

                                    var splitValue = li.Text.Split('=');
                                    var traitValue = splitValue[0];

                                    selectedValue += traitValue + "; ";
                                }
                            }
                            sbInfor.Append("&nbsp; &nbsp; &nbsp;").Append(traitName).Append(" ").Append(ddlOp.SelectedItem.ToString()).Append(" ").Append(selectedValue).Append("<br />");

                            switch (op)
                            {
                                case "EQ":
                                    comparator = " IN (:value{0}) ";
                                    break;
                                case "NEQ":
                                    comparator = " NOT IN (:value{0}) ";
                                    break;
                                case "GT":
                                    comparator = " >= :value{0} ";
                                    break;
                                case "LT":
                                    comparator = " <= :value{0} ";
                                    break;
                            }

                            if (sbWhere.Length > 0)
                            {
                                sbWhere.Append(" AND ");
                            }
                            var newWhere = String.Format(whereTemplate, i, fieldName, comparator);
                            newWhere = String.Format(newWhere, i);

                            newWhere = newWhere.Replace(":traitid" + i, ctID.ToString());
                            if (selected.Count == 1)
                            {
                                if (fieldName == "crop_trait_code_id")
                                {
                                    newWhere = newWhere.Replace(":value" + i, selected[0]);
                                }
                                else if (fieldName == "string_value")
                                {
                                    newWhere = newWhere.Replace(":value" + i, "'" + selected[0] + "'");
                                }
                                else if (fieldName == "numeric_value")
                                {
                                    newWhere = newWhere.Replace(":value" + i, selected[0]);
                                }
                            }
                            else
                            {
                                if (fieldName == "crop_trait_code_id")
                                {
                                    newWhere = newWhere.Replace(":value" + i, Toolkit.Join(selected.ToArray(), ",", ""));
                                }
                                else if (fieldName == "string_value")
                                {
                                    newWhere = newWhere.Replace(":value" + i, Toolkit.Join(selected.ToArray(), ",", "", "'"));
                                }
                                else if (fieldName == "numeric_value")
                                {
                                    newWhere = newWhere.Replace(":value" + i, Toolkit.Join(selected.ToArray(), ",", ""));
                                }
                            }

                            //sbSqlID_a.Append(sqlWhereIDs_a).Append(sqlID).Append(newWhere).Append(")) ");

                            string sqlWhereIDs_aTmp = "";
                            if (sbWhere.Length > 0) 
                            {
                                sbSqlID.Append(" And ");

                                sbSqlStatement.Append(" Union ");

                                if (joinType)
                                    sqlWhereIDs_aTmp = " and " + sqlWhereIDs_a;
                                else
                                    sqlWhereIDs_aTmp = " or " + sqlWhereIDs_a;
                            }
                            else
                                sqlWhereIDs_aTmp = " and (" + sqlWhereIDs_a;

                            sbSqlID_a.Append(sqlWhereIDs_aTmp).Append(sqlID).Append(newWhere).Append(")) ");

                            sbSqlID.Append(sqlID).Append(newWhere).Append(")");

                            sbSqlStatement.Append(sql).Append(newWhere).Append(" {0} ");
                             
                            sbWhere.Append(newWhere);
                        }
                    }

                    sqlWhereIDs += sbSqlID.Append(")");
                    sqlWhereIDs_a = sbSqlID_a.ToString();
                    if (!string.IsNullOrEmpty(sqlWhereIDs_a)) sqlWhereIDs_a += ")";

                    string sqlID_trait = "";
                    List<string> traitAIDs = new List<string>();
                    List<string> AIDs = new List<string>();
                    DataTable dtTID = null;

                    List<string> AIDs_passport = new List<string>();
                    string sqlWhereIDs_a_passport = "";

                    string advancedQuery = getAdvancedQuery();

                    var sbAllTraits = new StringBuilder();  // All chosen traits need to have some observation value
                    if (hasValue)
                    {
                        string sqlFind = "select accession_id from inventory i join crop_trait_observation cto on i.inventory_id = cto.inventory_id where cto.crop_trait_id = ";
                        if (allTraits.Count > 0)
                        {
                            foreach (int traitid in allTraits)  
	                        {
                                if (sbAllTraits.Length > 0)
                                    sbAllTraits.Append(" INTERSECT ").Append(sqlFind).Append(traitid);
                                else
                                    sbAllTraits.Append(sqlFind).Append(traitid);
	                        }
                        }
                        if (sbAllTraits.Length > 0)
                            sqlWhereIDs_a += sqlWhereIDs_a + " and a.accession_id in ( " + sbAllTraits.ToString() + ")";
                    }

                    if (!String.IsNullOrEmpty(sqlWhereIDs_a))
                    {
                        sqlID_trait = "select accession_id from accession a where " + sqlWhereIDs_a.Substring(4, sqlWhereIDs_a.Length - 4);
                        dtTID = dm.Read(sqlID_trait);
                    }
                    else if (!String.IsNullOrEmpty(advancedQuery))
                    {
                        if (sbWhere.ToString() == "")
                        {
                            if (anyValues.Count > 0)
                            {
                                string anyValueTemplate1 = anyValueTemplate.Replace(":anytraitids", Toolkit.Join(anyValues.ToArray(), ",", "")) + sqlWhereIDs_a + " and cto.is_archived = 'N' order by 1, 2, 3";
                                sqlID_trait = sql + anyValueTemplate1;
                                dtTID = dm.Read(sqlID_trait, new DataParameters(":langid1", sd.LanguageID, DbType.Int32, ":langid2", sd.LanguageID, DbType.Int32));
                            }
                        }
                    }

                    if (dtTID != null)
                    {
                        foreach (DataRow dr in dtTID.Rows)
                        {
                            traitAIDs.Add(dr[0].ToString());
                        }
                    }

                    string query = "";
                    if (traitAIDs.Count > 0)
                        query = " @accession.accession_id in (" + String.Join(",", traitAIDs.ToArray()) + ")";
                    else
                        query = "";
 
                    if (!String.IsNullOrEmpty(advancedQuery) )
                    {
                        bool doJoin = true;

                        if (!String.IsNullOrEmpty(sqlWhereIDs_a))
                        {
                            if (String.IsNullOrEmpty(query))
                                doJoin = false;
                        }

                        if (doJoin)
                        {
                            var dsIDs = sd.Search(query + advancedQuery, true, true, "", "accession", 0, 20000, 0, 0, "", "passthru=nonindexedorcomparison;OrMultipleLines=false", null, null);
                            foreach (DataRow dr in dsIDs.Tables["SearchResult"].Rows)
                            {
                                AIDs.Add(dr[0].ToString());
                            }

                            if (AIDs.Count > 0)
                                sqlWhereIDs_a = " and a.accession_id in (" + String.Join(",", AIDs.ToArray()) + ") ";
                            else
                                sqlWhereIDs_a = " and 1=2 ";

                            DataSet dsIDs_passport = null;
                            if (advancedQuery.StartsWith("and"))
                                dsIDs_passport = sd.Search(advancedQuery.Substring(4, advancedQuery.Length-4), true, true, "", "accession", 0, 20000, 0, 0, "", "passthru=nonindexedorcomparison;OrMultipleLines=false", null, null);
                            else
                                dsIDs_passport = sd.Search(advancedQuery, true, true, "", "accession", 0, 20000, 0, 0, "", "passthru=nonindexedorcomparison;OrMultipleLines=false", null, null);

                            foreach (DataRow dr in dsIDs_passport.Tables["SearchResult"].Rows)
                            {
                                AIDs_passport.Add(dr[0].ToString());
                            }

                            if (AIDs_passport.Count > 0)
                                sqlWhereIDs_a_passport = " and a.accession_id in (" + String.Join(",", AIDs_passport.ToArray()) + ") ";
                            else
                                sqlWhereIDs_a_passport = " and 1=2 ";
                        }
                    }

                    //sbSqlStatement.Append(" order by 1, 2, 3 ");
                    var sqlStatement = "";
                    var sqlStatementAny = "";
                    var sqlWhereDownload = "";

                    if (sbWhere.ToString() == "")
                    {
                        if (anyValues.Count > 0)
                        {
                            anyValueTemplate = anyValueTemplate.Replace(":anytraitids", Toolkit.Join(anyValues.ToArray(), ",", "")) + sqlWhereIDs_a + " and cto.is_archived = 'N' order by 1, 2, 3";

                            sqlStatement = sql + anyValueTemplate;
                            sqlWhereDownload = anyValueTemplate.Substring(0, anyValueTemplate.Length - 16);
                        }
                    }
                    else
                    {
                        //sqlStatement = String.Format(sbSqlStatement.ToString(), sqlWhereIDs);

                        //if (anyValues.Count > 0)
                        //{
                        //    anyValueTemplate = anyValueTemplate.Replace(":anytraitids", Toolkit.Join(anyValues.ToArray(), ",", "")) + " and cto.is_archived = 'N' order by 1, 2, 3";
                        //    anyValueTemplate = anyValueTemplate.Substring(0, anyValueTemplate.Length - 16);
                        //    sqlStatementAny = sql + anyValueTemplate + sqlWhereIDs;

                        //    sqlStatement += " union " + sqlStatementAny;
                        //}

                       sqlStatement = String.Format(sbSqlStatement.ToString(), sqlWhereIDs_a);
                       sqlWhereDownload = sqlWhereIDs_a.Substring(4, sqlWhereIDs_a.Length - 4);

                        if (anyValues.Count > 0)
                        {
                            //anyValueTemplate = anyValueTemplate.Replace(":anytraitids", Toolkit.Join(anyValues.ToArray(), ",", "")) + " and cto.is_archived = 'N' order by 1, 2, 3";
                            anyValueTemplate = anyValueTemplate.Replace(":anytraitids", Toolkit.Join(allTraits.ToArray(), ",", "")) + " and cto.is_archived = 'N' order by 1, 2, 3";
                            
                            anyValueTemplate = anyValueTemplate.Substring(0, anyValueTemplate.Length - 16);
                            if (joinType) // AND
                            {
                                sqlStatementAny = sql + anyValueTemplate + sqlWhereIDs_a;
                                sqlWhereDownload = anyValueTemplate + sqlWhereIDs_a;
                            }
                            else  // OR
                            {
                                if (hasValue)  // 'All have value' overrides OR 
                                {
                                    sqlStatementAny = sql + anyValueTemplate + sqlWhereIDs_a;
                                    sqlWhereDownload = anyValueTemplate + sqlWhereIDs_a;
                                }
                                else
                                {
                                    sqlStatementAny = sql + anyValueTemplate + sqlWhereIDs_a_passport;
                                    sqlWhereDownload = anyValueTemplate + sqlWhereIDs_a_passport;
                                }

                            }

                            sqlStatement += " union " + sqlStatementAny;
                        }

                        sqlStatement += " order by 1, 2, 3 ";
                    }

                    if (sqlWhereDownload.Length > 0) // limit the downloaded traits
                    {
                        if (anyValues.Count == 0) sqlWhereDownload = anyValueTemplate.Replace(":anytraitids", Toolkit.Join(allTraits.ToArray(), ",", ""))  + " and " + sqlWhereDownload;
                    }
                    Session["sqlwhere"] = sqlWhereDownload;
                    var where = sbWhere.ToString();

                    if (dbEngine == "ORACLE" || dbEngine == "POSTGRESQL")
                    {
                        var dt = dm.Read(sqlStatement, new DataParameters(":langid1", sd.LanguageID, DbType.Int32, ":langid2", sd.LanguageID, DbType.Int32));
                        var dt2 = dt.Transform(new string[] { "accession_number", "accession_id" }, "crop_trait_name", "crop_trait_name", "value");
                        ggPivotView.DataSource = dt2;
                        ggPivotView.PrimaryKeyName = "accession_id";
                        ggPivotView.AlternateKeyName = "accession_number";
                        ggPivotView.DataBind();
                        pnlResult.Visible = true;
                        if (dt.Rows.Count > 0)
                            btnExport.Visible = true;
                        else
                            btnExport.Visible = false;  
                    }
                    else
                    {
                        var dt = dm.Read(sqlStatement, new DataParameters(":langid1", sd.LanguageID, DbType.Int32, ":langid2", sd.LanguageID, DbType.Int32));
                        var dt2 = dt.Transform(new string[] { "Plant ID", "Accession ID" }, "crop_trait_name", "crop_trait_name", "value");
                        ggPivotView.DataSource = dt2;
                        ggPivotView.PrimaryKeyName = "Accession ID";
                        ggPivotView.AlternateKeyName = "Plant ID";
                        ggPivotView.DataBind();
                        pnlResult.Visible = true;
                        if (dt.Rows.Count > 0)
                            btnExport.Visible = true;
                        else
                            btnExport.Visible = false;  
                    }

                    if (UserManager.IsAnonymousUser(UserManager.GetUserName()))
                    {
                        btnAddToFavorite.Visible = false;
                    }
                    else
                    {
                        btnAddToFavorite.Visible = true;
                    }

                    var display = "<b>Query Criteria:</b><br /> " + "&nbsp; &nbsp; &nbsp;Crop: " + ddlCrops.SelectedItem.ToString() + "<br />" + sbInfor.ToString() + "<br /> " + getOptionDisplay();
                    if (!String.IsNullOrEmpty(advancedQuery))
                        display +=  getAdvancedQueryDisplay();
                    Master.ShowMore(display);
                    if ((Page.User.IsInRole("ALLUSERS"))) btnExportFB.Visible = true;
                }
            }
        }

       protected void btnResetAll_Click(object sender, EventArgs e)
       {
           ddlCrops.SelectedIndex = -1;
           pnlTraits.Visible = false;
           pnlSelectedTraits.Visible = false;
           btnClear_Click(sender, e);
           pnlPassport.Visible = false;
           pnlResult.Visible = false;
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
                       msg += Page.GetDisplayMember("Search",  "allItemsAlreadyInCart", "  All available items were already in your cart.");
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
               msgRed = Page.GetDisplayMember("Search", "someItemsNotAvailable", "  {0} items not available to be put into your cart.", itemsNotAvail.ToString());

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
       }

       protected bool divAdvToggleOn;
       protected void Page_PreRender(object sender, EventArgs e)
       {
           divAdvToggleOn = divAdvToggleOn || searchItem1.ShowControl || searchItem2.ShowControl || searchItem3.ShowControl || searchItem4.ShowControl || searchItem5.ShowControl;
       }

       private string getAdvancedQuery()
       {
           string op = "and ";
           string s = "";

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

           string sa = searchItem1.SearchCriteria(op) + searchItem2.SearchCriteria(op) + searchItem3.SearchCriteria(op) + searchItem4.SearchCriteria(op) + searchItem5.SearchCriteria(op);
           if (s.Length > 0 || sa.Length > 0) s += op + sa;
           if (s.Length > 3 && s.Substring(s.Length - 4, 4).ToUpper() == "AND ")
               s = s.Substring(0, s.Length - 4);
           if (s.Length > 2 && s.Substring(s.Length - 3, 3).ToUpper() == "OR ")
               s = s.Substring(0, s.Length - 3);

           return s;
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
               s += "&nbsp; &nbsp; &nbsp;Acessions are:" + sb.ToString();
               if (s.Substring(s.Length - 1, 1) == ",") s = s.Substring(0, s.Length - 1);
               s += "<br />";
           }

           return s + searchItem1.SearchCriteriaDisplay() + searchItem2.SearchCriteriaDisplay() + searchItem3.SearchCriteriaDisplay() + searchItem4.SearchCriteriaDisplay() + searchItem5.SearchCriteriaDisplay();
       }

       private string getOptionDisplay()
       {
           string sResult = "";
           if (rblMatch.SelectedValue == "all")
               sResult = "&nbsp; &nbsp; &nbsp;Results match all trait conditions. <br />";
           else
               sResult = "&nbsp; &nbsp; &nbsp;Results match any trait condition. <br />";

           if (cbValue.Checked) sResult += "&nbsp; &nbsp; &nbsp;Results have observation data for all selected descriptors.<br />";

           return sResult;
       }

       protected void btnExportFB_Click(object sender, EventArgs e)
       {
           string sqlWhere = "";
           string sqlWhereAdv = getAdvancedQuery();

           if (Session["sqlwhere"] != null)
           {
               sqlWhere = Session["sqlwhere"].ToString();
           }
           else
               sqlWhere = sqlWhereAdv;

           if (!String.IsNullOrEmpty(sqlWhere))
           {
               DataTable dt1 = null;
               DataTable dt1t = null;
               DataTable dt1o = null;
               DataTable dt2 = null;
               using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
               {
                   using (DataManager dm = sd.BeginProcessing(true, true))
                   {
                       dt1 = sd.GetData("web_descriptorbrowse_trait_fieldbook1", ":where=" + sqlWhere, 0, 0).Tables["web_descriptorbrowse_trait_fieldbook1"];
                       dt1t = dt1.Transform(new string[] { "accession_id" }, "trait_name", "trait_name", "value");

                       // id list from general search if there is one
                       if (!String.IsNullOrEmpty(sqlWhereAdv))
                       {
                           if (sqlWhereAdv.StartsWith("and"))
                               dt1o = sd.Search(sqlWhereAdv.Substring(4, sqlWhereAdv.Length - 4), true, true, "", "accession", 0, 20000, 0, 0, "", "passthru=nonindexedorcomparison;OrMultipleLines=false", null, null).Tables["SearchResult"];
                           else
                               dt1o = sd.Search(sqlWhereAdv, true, true, "", "accession", 0, 20000, 0, 0, "", "passthru=nonindexedorcomparison;OrMultipleLines=false", null, null).Tables["SearchResult"];
                       }
                       else
                           dt1o = dt1;

                       if (dt1o.Rows.Count > 0)
                       {
                           List<int> idlist = new List<int>();
                           int id;

                           foreach (DataRow dr in dt1o.Rows)
                           {
                               if (int.TryParse(dr[0].ToString(), out id))
                                   idlist.Add(id);
                           }

                           dt2 = sd.GetData("web_descriptorbrowse_trait_fieldbook2", ":idlist=" + Toolkit.Join(idlist.ToArray(), ",", ""), 0, 0).Tables["web_descriptorbrowse_trait_fieldbook2"];
                           int j = dt2.Columns.Count;
                           
                           for (int i = 1; i < dt1t.Columns.Count; i++)
                           {
                               DataColumn traitCol = new DataColumn(dt1t.Columns[i].ColumnName, typeof(string));
                               dt2.Columns.Add(traitCol);
                           }

                           DataTable dtMultiple = null;
                           foreach (DataRow dr in dt2.Rows)
                           {
                               id = Toolkit.ToInt32(dr[3].ToString(), 0);

                               // first need to add some query, so to get  0) inventory (could be many?? which to put) 1) all accession name 2) all sourcehistory, replace the blank placeholder
                               dtMultiple = sd.GetData("web_fieldbook_accessionnames", ":accessionid=" + id, 0, 0).Tables["web_fieldbook_accessionnames"];
                               dt2.Columns["IDS"].ReadOnly = false;
                               dr["IDS"] = Toolkit.Join(dtMultiple, "plantname", ";", "");

                               dtMultiple = sd.GetData("web_fieldbook_srchistory", ":accessionid=" + id, 0, 0).Tables["web_fieldbook_srchistory"];

                               var sb = new StringBuilder();
                               if (dtMultiple == null || dtMultiple.Rows.Count == 0) { }
                               else
                               {
                                   dt2.Columns["HISTORY"].ReadOnly = false;
                                   string acsid = "";
                                   foreach (DataRow drs in dtMultiple.Rows)
                                   {
                                       if (drs["accession_source_id"].ToString() != acsid)
                                       {
                                            if (sb.Length > 1) sb.Append("; ");
                                           sb.Append(drs["typecode"].ToString());

                                           if (!String.IsNullOrEmpty(drs["histdate"].ToString()))
                                               sb.Append(" ").Append(drs["histdate"].ToString());

                                           if (!String.IsNullOrEmpty(drs["state"].ToString()))
                                               sb.Append(" ").Append(drs["state"].ToString()).Append(",");


                                           if (!String.IsNullOrEmpty(drs["country"].ToString()))
                                               sb.Append(" ").Append(drs["country"].ToString());

                                       }
                                       if (!String.IsNullOrEmpty(drs["fname"].ToString()))
                                           sb.Append(" by ").Append(drs["fname"].ToString()).Append(",");

                                       if (!String.IsNullOrEmpty(drs["lname"].ToString()))
                                           sb.Append(" ").Append(drs["lname"].ToString()).Append(",");

                                      if (!String.IsNullOrEmpty(drs["organization"].ToString()))
                                           sb.Append(" ").Append(drs["organization"].ToString());

                                       acsid = drs["accession_source_id"].ToString();
                                   }
                                   if (sb.Length > 1) dr["HISTORY"] = sb.ToString();
                               }

                               // get trait value if there is any
                               
                               DataRow[] foundRows = dt1t.Select("accession_id = " + id);
                               if (foundRows.Length > 0) 
                               {
                                   DataRow foundRow = foundRows[0];
                                   for (int i = 1; i < dt1t.Columns.Count; i++)
                                   {
                                       dr[j + i -1] = foundRow[i];
                                   }
                               }
 
                           }
                           gvResult.DataSource = dt2;
                           gvResult.DataBind();

                           Utils.ExportToExcel(HttpContext.Current, gvResult, "descriptor_query", "");

                       }
                   }

               }
           }
       }

     }
}
