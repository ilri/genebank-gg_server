using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;
using System.Text;

namespace GrinGlobal.Web
{
    public partial class cropmarkerview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindData(Toolkit.ToInt32(Request.QueryString["markerid"], 0), Toolkit.ToInt32(Request.QueryString["cropid"], 0));
            }
        }

        private void bindData(int markerID, int cropID)
        {
            DataTable dt = null;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                if (markerID > 0)
                {
                    dt = sd.GetData("web_crop_marker_data", ":markerid=" + markerID, 0, 0).Tables["web_crop_marker_data"];

                    if (dt.Rows.Count > 0)
                    {
                        gvData.DataSource = dt;
                        gvData.DataBind();

                        using (DataManager dm = sd.BeginProcessing(true, true))
                        {
                            string mName = dm.ReadValue(@"select name from genetic_marker where genetic_marker_id = :markerid", new DataParameters(":markerid", markerID, DbType.Int32)).ToString();
                            gvData.HeaderRow.Cells[gvData.HeaderRow.Cells.Count - 1].Text = mName;
                        }
                    }
                }
                else
                {
//                    using (DataManager dm = sd.BeginProcessing(true))
//                    {
//                        DataTable dtIDs = dm.Read(@"select genetic_marker_id, name from genetic_marker where crop_id = :cropid order by genetic_marker_id", new DataParameters(":cropid", cropID, DbType.Int32));

//                        if (dtIDs.Rows.Count > 0)
//                        {
//                            var ids = new List<string>();
//                            var names = new List<String>();
//                            int cnt = dtIDs.Rows.Count ;

//                            for (int i = 0; i < cnt - 1; i++)
//                            {
//                                ids.Add(dtIDs.Rows[i][0].ToString());
//                                names.Add(" ' ' as " + dtIDs.Rows[i][1].ToString());
//                            }

//                            StringBuilder sb = new StringBuilder();
//                            string sSelect = @" Select 
//	                                c.name as crop, 
//	                                m.name as method_name, 
//	                                a.accession_number_part1 as acp,
//	                                a.accession_number_part2 as acNumber, 
//	                                a.accession_number_part3 as acs,
//	                                i.inventory_number_part1 as ivp, 
//	                                i.inventory_number_part2 as ivNumber, 
//	                                i.inventory_number_part3 as ivs, 
//	                                i.form_type_code as ivType,";

//                            string sFrom = @" From
//	                                genetic_observation_data gobs 
//	                                join genetic_annotation ga 
//		                                on gobs.genetic_annotation_id = ga.genetic_annotation_id
//	                                join genetic_marker gm 
//		                                on ga.genetic_marker_id = gm.genetic_marker_id
//	                                join method m 
//		                                on ga.method_id = m.method_id
//	                                join inventory i  
//		                                on gobs.inventory_id = i.inventory_id
//	                                join accession a 
//		                                on i.accession_id = a.accession_id
//	                                join crop c 
//		                                on gm.crop_id = c.crop_id
//                                where 
//	                                gm.genetic_marker_id =";

//                            string sOrder = @" order by 
//	                                m.name, 
//	                                a.accession_number_part1, 
//	                                a.accession_number_part2, 
//	                                a.accession_number_part3";

//                            for (int i = 0; i < cnt - 1; i++)
//                            {
//                                //var namesi = names;
//                                var namesi = names.Select(item => (string)item.Clone()).ToList();
//                                namesi[i] = namesi[i].Replace("' '", "gobs.value");
//                                string s = string.Join(",", namesi.ToArray());
//                                if (sb.Length > 0) sb.Append(" union ");
//                                sb.Append(sSelect).Append(s).Append(sFrom).Append(ids[i]);
//                            }

//                            sb.Append(sOrder);
//                            dt = dm.Read(sb.ToString());

//                            gvData.DataSource = dt;
//                            gvData.DataBind();

//                            if ( cnt > 6) gvData.Font.Size = FontUnit.Smaller;
//                        }
//                    }

                    if (cropID > 0)
                    {
                        dt = sd.GetData("web_crop_marker_alldata", ":cropid=" + cropID, 0, 0).Tables["web_crop_marker_alldata"];

                        if (dt.Rows.Count > 0)
                        {
                            var dt2 = dt.Transform(new string[] { "crop", "method_name", "acp", "acNumber", "acs", "ivp", "ivNumber", "ivType" }, "name", "name", "value");

                            gvData.DataSource = dt2;
                            gvData.DataBind();

                            if (gvData.HeaderRow.Cells.Count > 22)
                                gvData.Font.Size = FontUnit.XXSmall;
                            else
                                if (gvData.HeaderRow.Cells.Count > 15)
                                    gvData.Font.Size = FontUnit.Smaller;

                        }
                    }

                }

            }
        }

    }
}
