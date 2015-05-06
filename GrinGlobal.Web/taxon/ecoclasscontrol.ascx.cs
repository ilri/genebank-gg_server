using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using System.Text;

namespace GrinGlobal.Web.taxon
{
    public partial class ecoclasscontrol : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && loadData)
            {
                bindData();
            }
        }

        public void bindData()
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                var dt = sd.GetData("web_lookup_taxon_use_class", "", 0, 0).Tables["web_lookup_taxon_use_class"];
                var drNone = dt.NewRow();
                drNone["code"] = "allclass";
                drNone["text"] = "All Classes";
                dt.Rows.InsertAt(drNone, 0);

                ddlClass.DataSource = dt;
                ddlClass.DataBind();

                lstSubclass.Items.Insert(0, new ListItem("all uses", ""));
                lstSubclass.Rows = 1;
            }
            lblChoose.Text = Site1.DisplayText("lblChooseCN", "Choose Class") + " " + sequence + ":";
            lblChooseSub.Text = Site1.DisplayText("lblChooseCN", "Choose Subclass") + " " + sequence + ":";

            btnClear.Text = Site1.DisplayText("btnClearCN", "Clear Class") + " " + sequence;
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearCriteria();
        }


        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code = Utils.Sanitize(ddlClass.SelectedValue);
            string value = ddlClass.SelectedItem.Text;

            if (code != "allclass")
            {
                using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
                {
                    var dt = sd.GetData("web_lookup_taxon_use_subclass", ":usagecode=" + code, 0, 0).Tables["web_lookup_taxon_use_subclass"];


                    lstSubclass.DataSource = dt;
                    lstSubclass.DataBind();

                    lstSubclass.Items.Insert(0, new ListItem("all uses", "all subclasses"));
                    lstSubclass.SelectedIndex = 0;
                    lstSubclass.Rows = 4;
                }
            }
            else
            {
                lstSubclass.Items.Clear();
                lstSubclass.Items.Insert(0, new ListItem("all uses", "all subclasses"));
                lstSubclass.Rows = 1;
            }

        }


        public void ClearCriteria()
        {
            if (ddlClass.Items.Count != 0)
            {
                ddlClass.SelectedIndex = -1;
            }
            lstSubclass.Items.Clear();
            lstSubclass.Items.Insert(0, new ListItem("all uses", "all subclasses"));
            lstSubclass.Rows = 1;

        }

        public string SearchCriteria(string opClass, string opSubclass)
        {
            string search = "";
            string useClass = "";

            useClass = Utils.Sanitize(ddlClass.SelectedValue);

            StringBuilder sb = new StringBuilder();

            if (useClass.IndexOf("allclass") < 0 && useClass != "")
            {
                if (useClass == "CITES")
                    search = opClass + " Exists (select * from taxonomy_use tu where ";
                else
                    search = opClass + " Exists (select * from taxonomy_use tu where tu.economic_usage_code = '" + useClass + "' ";

                foreach (ListItem li in lstSubclass.Items)
                {
                    if (li.Selected)
                    {

                        if (useClass == "CITES")
                        {
                            if (li.Value == "all subclasses" && opSubclass == " OR ")
                            {
                                sb.Append("tu.economic_usage_code = 'CITESI' or tu.economic_usage_code = 'CITESII' or tu.economic_usage_code = 'CITESIII'");
                                break;
                            }
                            else if (li.Value == "all subclasses")
                                continue;
                            else
                            {
                                if (sb.Length > 0)
                                    sb.Append(opSubclass).Append("tu.economic_usage_code = '").Append(Utils.Sanitize(li.Text)).Append("'");
                                else
                                    sb.Append("tu.economic_usage_code = '").Append(Utils.Sanitize(li.Text)).Append("'");
                            }
                        }
                        else
                        {
                            if (li.Value == "all subclasses" && opSubclass == " OR ")
                                break;
                            else if (li.Value == "all subclasses")
                                continue;
                            else
                            {
                                if (sb.Length > 0)
                                    sb.Append(opSubclass).Append("tu.usage_type like '%").Append(Utils.Sanitize(li.Value)).Append("%'");
                                else
                                    sb.Append("tu.usage_type like '%").Append(Utils.Sanitize(li.Value)).Append("%'");
                            }
                        }
                    }
                }

                if (sb.Length > 0)
                {
                    if (useClass == "CITES")
                        search += " (" + sb.ToString() + ") ";
                    else
                        search += " AND (" + sb.ToString() + ") ";
                }
                else if (sb.Length == 0 && useClass == "CITES")
                    search += " (tu.economic_usage_code = 'CITESI' or tu.economic_usage_code = 'CITESII' or tu.economic_usage_code = 'CITESIII') ";
                
                search += " and tu.taxonomy_species_id = t1.taxonomy_species_id) ";
            }
            else
                search = "";

            return search;
        }

        public string SearchCriteriaDisplay(string opClass, string opSubclass)
        {
            string search = "";
            string useClass = "";

            if (ddlClass.SelectedValue != "") useClass = ddlClass.SelectedItem.Text;
            opClass = (opClass == " AND ") ? " <i>&</i> " : " <i>or</i> ";
            opSubclass = (opSubclass == " AND ") ? " <i>&</i> " : " <i>or</i> ";

            StringBuilder sb = new StringBuilder();

            if (useClass.IndexOf("All Classes") < 0 && useClass != "")
            {
                if (useClass == "Rare or Endangered (CITES)")
                    search = opClass + "<b>CITES App.</b> = '";
                else
                    search = opClass + "<b>" + useClass + "</b> = '";

                foreach (ListItem li in lstSubclass.Items)
                {
                    if (li.Selected)
                    {
                        if (useClass == "Rare or Endangered (CITES)")
                        {
                            if (sb.Length > 0)
                                sb.Append(opSubclass).Append(li.Text);
                            else
                            {
                               if (li.Value == "all subclasses")
                                    sb.Append("ALL");
                               else
                                   sb.Append("'").Append(li.Text);
                            }
                        }
                        else
                        {
                            if (sb.Length > 0)
                                sb.Append(opSubclass).Append(li.Value);
                            else
                            {
                                if (li.Value == "all subclasses")
                                    sb.Append(li.Value);
                                else
                                    sb.Append("'as ").Append(li.Value);
                            }
                       }
                    }
                }

                if (sb.Length > 0) search += sb.ToString() + "'";
             }
            else
                search = "";

            return search;
        }
 
    }
}