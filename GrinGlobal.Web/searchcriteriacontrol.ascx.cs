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
    public partial class searchcriteriacontrol : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //lblChoose.Text = Site1.DisplayText("lblChooseN", "Choose Query") + " " + sequence + ":";
            //btnClear.Text = Site1.DisplayText("btnClearN", "Clear Criteria") + " " + sequence;

            if (!IsPostBack && loadData)
            {
                bindData();
            }
        }

        private bool divAdvToggleOn;
        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string groupname = ddlItem.SelectedItem.Text;
            string tblname = ddlItem.SelectedValue;

            if (tblname.IndexOf("@") == -1)
            {
                using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
                {
                    var dt = sd.GetData("web_searchcriteria_item_value", ":groupname=" + groupname + ";:langid=" + sd.LanguageID, 0, 0).Tables["web_searchcriteria_item_value"];
                    //Only title with lang_id = 1 for now
                    //var dt = sd.GetData("web_searchcriteria_item_value", ":groupname=" + groupname + ";:langid=" + 1, 0, 0).Tables["web_searchcriteria_item_value"];
                    lstValues.DataSource = dt;
                    lstValues.DataBind();
                }
                lstValues.Visible = true;
                txtValue.Text = "";
                txtValue.Visible = false;
                pnlPIRange.Visible = false;

                ListItem li = new ListItem("Not Equal To", "!=");
                if(ddlOperator.Items.Contains(li))
                    ddlOperator.Items.Remove(li);

                ddlOperator.Visible = true;
                pnlLocation.Visible = false;
            }
            else
            {
                if (tblname == "@site.site_id")
                {
                    using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
                    {
                        var dt = sd.GetData("web_lookup_site_list", "", 0, 0).Tables["web_lookup_site_list"];
                        lstValues.DataSource = dt;
                        lstValues.DataBind();
                    }
                    lstValues.Visible = true;
                    txtValue.Text = "";
                    txtValue.Visible = false;
                    pnlPIRange.Visible = false;

                    ListItem li = new ListItem("Not Equal To", "!=");
                    if (ddlOperator.Items.Contains(li))
                        ddlOperator.Items.Remove(li);

                    ddlOperator.Visible = true;
                    pnlLocation.Visible = false;

                }
                else if (tblname == "@accession.id")
                {

                    using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
                    {
                        using (DataManager dm = sd.BeginProcessing(true, true))
                        {
                            var dt2 = dm.Read(@"select distinct accession_number_part1 as value from accession order by  accession_number_part1 asc");
                            ddlIdentifier.DataSource = dt2;
                            ddlIdentifier.DataBind();
                            var item = ddlIdentifier.Items.FindByValue("PI");
                            ddlIdentifier.SelectedIndex = ddlIdentifier.Items.IndexOf(item);
                        }
                    }

                    lstValues.Visible = false ;
                    txtValue.Text = "";
                    txtValue.Visible = false;
                    pnlPIRange.Visible = true;
                    ddlOperator.Visible = false;
                    pnlLocation.Visible = false;
                }
                else if (tblname == "@accession.location")
                {
                    using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
                    {
                        var dt = sd.GetData("web_lookup_country_list_ids", ":languageid=" + sd.LanguageID, 0, 0).Tables["web_lookup_country_list_ids"];
                        ddlCountry.DataSource = dt;
                        ddlCountry.DataBind();
                        ddlCountry.Items.Insert(0, new ListItem("-- Select a country --", ""));
                        ddlCountry.SelectedIndex = 0;
                        ddlState.Items.Clear();
                     }
                    lstValues.Visible = false;
                    txtValue.Text = "";
                    txtValue.Visible = false;
                    pnlPIRange.Visible = false;
                    ddlOperator.Visible = false;
                    pnlLocation.Visible = true;
                }
                else if (tblname == "@inventory.availability_status_code")
                {
                    lstValues.Visible = true;
                    lstValues.Items.Clear();
                    lstValues.Items.Add(new ListItem("Available", "AVAIL"));
                    lstValues.Items.Add(new ListItem("Not Available", "NOT AVAIL"));

                    txtValue.Text = "";
                    txtValue.Visible = false;
                    pnlPIRange.Visible = false;

                    ListItem li = new ListItem("Not Equal To", "!=");
                    if (ddlOperator.Items.Contains(li))
                        ddlOperator.Items.Remove(li);

                    ddlOperator.Visible = true;
                    pnlLocation.Visible = false;

                }
                else if (tblname == "@accession_inv_group.accession_inv_group_id")
                {
                    using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
                    {
                        var dt = sd.GetData("web_lookup_accession_group", "", 0, 0).Tables["web_lookup_accession_group"];
                        lstValues.DataSource = dt;
                        lstValues.DataBind();
                    }
                    lstValues.Visible = true;
                    txtValue.Text = "";
                    txtValue.Visible = false;
                    pnlPIRange.Visible = false;

                    ListItem li = new ListItem("Not Equal To", "!=");
                    if (ddlOperator.Items.Contains(li))
                        ddlOperator.Items.Remove(li);

                    ddlOperator.Visible = true;
                    pnlLocation.Visible = false;

                }
                else
                {
                    txtValue.Visible = true;
                    lstValues.Items.Clear();
                    lstValues.Visible = false;
                    pnlPIRange.Visible = false;

                    ListItem li = new ListItem("Not Equal To", "!=");
                    if (!ddlOperator.Items.Contains(li))
                        ddlOperator.Items.Add(li);

                    ddlOperator.Visible = true;
                    pnlLocation.Visible = false;
                }
            }
            divAdvToggleOn = true;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearCriteria();
        }

        public string SearchCriteria(string op)
        {
            if (ddlItem.SelectedValue =="@accession.id")
            {
                string search = " @accession.accession_number_part1='" + ddlIdentifier.SelectedValue + "'"; 
                int from = Toolkit.ToInt32(txtIDFrom.Text.Trim(), 0);
                int to = Toolkit.ToInt32(txtIDTo.Text.Trim(), 0);

                if (!string.IsNullOrEmpty(txtIDFrom.Text))
                {
                    if (!string.IsNullOrEmpty(txtIDTo.Text))
                        search += " and @accession.accession_number_part2 >= " + from;
                    else
                        search += " and @accession.accession_number_part2 = " + from;
                }

                if (!string.IsNullOrEmpty(txtIDTo.Text))
                {
                    if (!string.IsNullOrEmpty(txtIDFrom.Text))
                        search += " and @accession.accession_number_part2 <= " + to;
                    else
                        search += " and @accession.accession_number_part2 = " + to;
                }

                return search;
            }
            else if (ddlItem.SelectedValue == "@accession.location")
            {
                string search = "";
                string geoid = "";
                string sql = "";
                if (ddlState.SelectedIndex <= 0)
                //geoid = ddlCountry.SelectedValue.Split(':')[0];
                {
                    string selected = ddlCountry.SelectedValue;
                    string inState = "";
                    if (selected != "")
                    {
                        DataTable dt = Utils.GetStateList(selected.Split(':')[1]);
                        foreach(DataRow dr in dt.Rows) 
                        {
                            inState += dr["gid"] + ",";
                        }
                        inState += ddlCountry.SelectedValue.Split(':')[0];
                        //search = " @accession_source.geography_id in (" + inState + ")";
                        search = " (@accession_source.geography_id in (" + inState + ") and @accession_source.is_origin='Y') " + op;   //6/27/2013 SE works OK for this query
                        //sql = "select accession_id from accession_source where is_origin = 'Y' and geography_id in (" + inState + ")";
                    }
                }
                else
                {
                    geoid = ddlState.SelectedValue;
                    if (!string.IsNullOrEmpty(geoid))
                        //search = " @accession_source.geography_id=" + geoid;
                        search = " (@accession_source.geography_id=" + geoid + "  and @accession_source.is_origin='Y') " + op;    //6/27/2013 SE works OK for this query
                        //sql = "select accession_id from accession_source where is_origin = 'Y' and geography_id = " + geoid;
                }
                /*
                if (sql != "")
                {
                    using (var sd = UserManager.GetSecureData(true))
                    {
                        using (var dm = DataManager.Create(sd.DataConnectionSpec))
                        {
                            DataTable dtID = dm.Read(sql);
                            List<string> IDs = new List<string>();
                            if (dtID != null)
                            {
                                foreach (DataRow dr in dtID.Rows)
                                {
                                    IDs.Add(dr[0].ToString());
                                }
                                if (IDs.Count > 0)
                                    search = " @accession.accession_id in (" + String.Join(",", IDs.ToArray()) + ")";
                                else
                                    search = "";
                            }
                            else
                                search = "";
                        }
                    }
                }
                else
                    search = "";
                */
                return search;
            }
            else if (ddlOperator.SelectedIndex == 0 || ddlItem.Items.Count == 0 || (ddlItem.Items.Count > 0 && ddlItem.SelectedIndex == 0))
                return "";
            else if (ddlItem.SelectedValue == "@inventory.availability_status_code")
            {
                string condition1 = "";
                string condition2 = "";
                string search = "";

                foreach (ListItem li in lstValues.Items)
                {
                    string value = li.Value;
                    if (li.Selected)
                    {
                        if (li.Value == "AVAIL")
                            condition1 = " @inventory.availability_status_code = 'AVAIL'";

                        if (li.Value == "NOT AVAIL")
                            condition2 = " @inventory.availability_status_code not in ('AVAIL')";
                    }
                }

                if (condition1 != "" && condition2 != "") // have everything equals no constraint
                    search = "";
                else
                    search = condition1 + condition2;

                return search;
            }
            //else if (txtValue.Visible)
            //{
            //    if (txtValue.Text.Trim() != "")
            //    {
            //        return " " + ddlItem.SelectedValue  + " " + txtValue.Text.Trim();
            //    }
            //    else
            //        return "";
            //}
            //else
            //{
            //    string field = ddlItem.SelectedValue;
            //    StringBuilder sb = new StringBuilder();

            //    foreach (ListItem li in lstValues.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            if (sb.Length > 0)
            //                sb.Append(" OR @").Append(field).Append(" ").Append(li.Value);
            //            else
            //                sb.Append(" @").Append(field).Append(" ").Append(li.Value);
            //        }
            //    }
            else if (txtValue.Visible)
            {
                if (txtValue.Text.Trim() != "")
                {
                    //if (ddlOperator.SelectedValue == "like")
                    //    return " " + ddlItem.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
                    //else if (ddlOperator.SelectedValue == "=")
                    //    return " " + ddlItem.SelectedValue + " = '" + txtValue.Text.Trim() + "'";
                    //else
                    //    return " " + ddlItem.SelectedValue + " in (" + txtValue.Text.Trim() + ")";

                    string search = "";
                    string userTxt = txtValue.Text.Trim();
                    if (userTxt.IndexOf(",") < 0) userTxt = userTxt.Replace("'", "''");   //leave the case of example genus_name in ('vitis', 'Parthenocissus','Ampelocissus', 'Ampelopsis')
   
                    switch (ddlOperator.SelectedValue)
                    {
                        case "=":
                            search = " " + ddlItem.SelectedValue + " = '" + userTxt + "'";
                            break;
                        case "like":
                            search = " " + ddlItem.SelectedValue + " like '" + userTxt.Replace('*', '%') + "'";
                            break;
                        case "in":
                            search = " " + ddlItem.SelectedValue + " in (" + userTxt + ")";
                            break;
                        case "contain":
                            search = " " + ddlItem.SelectedValue + " like '%" + userTxt.Replace('*', '%') + "%'";
                            break;
                        case "!=":
                            search = " " + ddlItem.SelectedValue + " != '" + userTxt + "'";
                            break;
                        default:
                            search = " " + ddlItem.SelectedValue + " = '" + userTxt + "'";
                            break;
                    }
                    return search;
                }
                else
                    return "";
            }
            else
            {
                string field = " ";
                string fieldNull = " ";
                if (ddlItem.SelectedValue.Substring(0, 1) != "@")
                {
                    field += " @";
                    fieldNull += " @";
                }

                field += ddlItem.SelectedValue + " in (";
                fieldNull += ddlItem.SelectedValue;
                StringBuilder sb = new StringBuilder();
                bool hasNull = false;

                foreach (ListItem li in lstValues.Items)
                {
                    if (li.Selected)
                    {
                        if (li.Value.ToUpper() == "NULL")
                            hasNull = true;
                        else
                        {
                            if (sb.Length > 0)
                                sb.Append(", '").Append(li.Value).Append("'");
                            else
                                sb.Append("'").Append(li.Value).Append("'");
                        }
                    }
                }

                if (hasNull)
                {
                    if (sb.Length > 0)
                        return " (" + field + sb.Append(")").Append(" or ").Append(fieldNull).Append(" is null) ").Append(op).ToString();
                        //return field + sb.Append(")").Append(" or ").Append(fieldNull).Append(" is null").ToString();
                    else
                        //return " ("  + sb.Append(fieldNull).Append(" is null)").ToString();
                        return " (" + sb.Append(fieldNull).Append(" is null) A").Append(op).ToString();
                }
                else
                {
                    if (sb.Length > 0)
                        return field + sb.ToString() + ")";
                    else
                        return "";
                }
            }
        }

        public bool ShowControl 
        {
            get 
            {
                return divAdvToggleOn;
            }
        }

        public void bindData()
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                var dt = sd.GetData("web_searchcriteria_item_list", "", 0, 0).Tables["web_searchcriteria_item_list"];
                var drNone = dt.NewRow();
                drNone["group_name"] = " -- Select One -- ";
                drNone["name"] = "none";
                dt.Rows.InsertAt(drNone, 0);

                //foreach (DataRow dr in dt.Rows)
                //{
                //    dr["group_name"] = dr["group_name"].ToString().ToLower().Replace("_", "  ");
                //}

                bool isInternal = (Page.User.IsInRole("ALLUSERS"));
                if (isInternal)
                {
                    var drOrder = dt.NewRow();
                    drOrder["group_name"] = "order request id";
                    drOrder["name"] = "@order_request.order_request_id";
                    dt.Rows.Add(drOrder);
                }
                ddlItem.DataSource = dt;
                ddlItem.DataBind();

            }
            //lblChoose.Text = "Choose Query " + sequence + ":";
            //btnClear.Text = "Clear Criteria " + sequence;
            lblChoose.Text = Site1.DisplayText("lblChooseN", "Choose Criterion") + " " + sequence + ":";
            btnClear1.Text = Site1.DisplayText("btnClearN", "Clear Criterion") + " " + sequence;
        }

        private bool loadData;
        public bool LoadData
        {
            set
            {
                loadData = value;
            }
        }

        private int sequence;
        public int Sequence
        {
            get { return sequence; }
            set { sequence = value; }
        }

        public void ClearCriteria()
        {
            if (ddlItem.Items.Count != 0)
            {
                ddlItem.SelectedIndex = -1;
            }
            ddlOperator.SelectedIndex = 0;
            ddlOperator.Visible = true;
            lstValues.Items.Clear();
            lstValues.Visible = false;
            txtValue.Text = "";
            txtValue.Visible = false;
            txtIDFrom.Text = "";
            txtIDTo.Text = "";
            divAdvToggleOn = true;
            ddlIdentifier.Items.Clear();
            pnlPIRange.Visible = false;
            ddlCountry.Items.Clear();
            ddlState.Items.Clear();
            pnlLocation.Visible = false;
        }

        public string SearchCriteriaDisplay()
        {
            string indent = "&nbsp; &nbsp; &nbsp;";

            if (ddlItem.SelectedValue == "@accession.id")
            {
                string search = "";
                string prefix = ddlIdentifier.SelectedValue;
                string from = txtIDFrom.Text.Trim();
                string to = txtIDTo.Text.Trim();

                if (string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
                    search = indent + "'accession prefix' equal to '" + prefix + "'<br />";
                else if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
                    search = indent + "'accession identifier' equals to '" + prefix + " " + from + "'<br />";
                else if (string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                    search = indent + "'accession identifier' equals to '" + prefix + " " + to + "'<br />";
                else
                    search = indent + "'accession identifier range' between '" + prefix + " " + from + "' and '" + prefix + " " + to + "'<br />";

                return search;
            }
            else if (ddlItem.SelectedValue == "@accession.location")
            {
                string search = "";
                string country = ddlCountry.Items.Count > 0? ddlCountry.SelectedItem.Text : "";
                string state = ddlState.Items.Count > 0? ddlState.SelectedItem.Text: "";

                if (ddlCountry.SelectedIndex > 0)
                    search = indent + "'country of origin' Country equals " + country;

                if (ddlState.SelectedIndex > 0)
                    search += "; State equals " + state + "<br />";
                else
                {   
                    if (!string.IsNullOrEmpty(search))
                        search += "<br />";
                }

                return search;
            }
            else if (ddlOperator.SelectedIndex == 0 || ddlItem.Items.Count == 0 || (ddlItem.Items.Count > 0 && ddlItem.SelectedIndex == 0))
                return "";
            else if (ddlItem.SelectedValue == "@inventory.availability_status_code")
            {
                string condition1 = "";
                string condition2 = "";
                string search = "";

                foreach (ListItem li in lstValues.Items)
                {
                    string value = li.Value;
                    if (li.Selected)
                    {
                        if (li.Value == "AVAIL")
                            condition1 = "'AVAIL'";

                        if (li.Value == "NOT AVAIL")
                            condition2 = "'NOT AVAIL'";
                    }
                }

                if (condition1 != "" && condition2 != "")
                    search = indent + "'" + ddlItem.SelectedItem.ToString() + "' in (" + condition1 + ", " + condition2 + ")";
                else if (condition1 != "")
                    search = indent + "'" + ddlItem.SelectedItem.ToString() + "' in (" + condition1 + ")";
                else
                    search = indent + "'" + ddlItem.SelectedItem.ToString() + "' in (" + condition2 + ")";

                return search;
            }
            else if (txtValue.Visible)
            {
                if (txtValue.Text.Trim() != "")
                {
                    //    if (ddlOperator.SelectedValue == "like")
                    //        return indent + "'" + ddlItem.SelectedItem.ToString() + "' like '" + txtValue.Text.Trim() + "'<br />";
                    //    else if (ddlOperator.SelectedValue == "=")
                    //        return indent + "'" + ddlItem.SelectedItem.ToString() + "' = '" + txtValue.Text.Trim() + "'<br />";
                    //    else
                    //        return indent + "'" + ddlItem.SelectedItem.ToString() + "' in (" + txtValue.Text.Trim() + ")<br />";

                    string search = "";
                    switch (ddlOperator.SelectedValue)
                    {
                        case "=":
                            search = indent + "'" + ddlItem.SelectedItem.ToString() + "' equal to '" + txtValue.Text.Trim() + "'<br />";
                            break;
                        case "like":
                            search = indent + "'" + ddlItem.SelectedItem.ToString() + "' like '" + txtValue.Text.Trim() + "'<br />";
                            break;
                        case "in":
                            search = indent + "'" + ddlItem.SelectedItem.ToString() + "' in (" + txtValue.Text.Trim() + ")<br />";
                            break;
                        case "contain":
                            search = indent + "'" + ddlItem.SelectedItem.ToString() + "' contains '" + txtValue.Text.Trim() + "'<br />";
                            break;
                        case "!=":
                            search = indent + "'" + ddlItem.SelectedItem.ToString() + "' not equal to '" + txtValue.Text.Trim() + "'<br />";
                            break;
                        default:
                            search = indent + "'" + ddlItem.SelectedItem.ToString() + "' equal to '" + txtValue.Text.Trim() + "'<br />";
                            break;
                    }
                    return search;
                }
                else
                    return "";
            }
            else
            {
                string field = indent;
                //if (ddlItem.SelectedValue.Substring(0, 1) != "@")
                //    field += " @";

                field += "'" + ddlItem.SelectedItem.ToString() + "' in (";
                StringBuilder sb = new StringBuilder();

                foreach (ListItem li in lstValues.Items)
                {
                    if (li.Selected)
                    {
                        if (sb.Length > 0)
                            sb.Append(", '").Append(li.Text).Append("'");
                        else
                            sb.Append("'").Append(li.Text).Append("'");
                    }
                }

                if (sb.Length > 0)
                    return field + sb.ToString() + ")<br />";
                else
                    return "";
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = ddlCountry.SelectedValue;
            if (selected == "")
                ddlState.Items.Clear();
            else
            {
                selected = selected.Split(':')[1];

                DataTable dt = Utils.GetStateList(selected);
                if (dt.Rows.Count > 0)
                {
                    ddlState.DataValueField = "gid";
                    ddlState.DataTextField = "stateName";
                    ddlState.DataSource = dt;
                    ddlState.DataBind();
                }
            }
            divAdvToggleOn = true;
        }
     }

}