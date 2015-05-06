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
    public partial class searchcriteriacontrol2 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack & loadData) pnlLatLong.Visible = true;
         }

        private bool divLatLongToggleOn;
        public bool ShowControl 
        {
            get 
            {
                return divLatLongToggleOn;
            }
            set
            {
                divLatLongToggleOn = value;
            }
        }

        private bool loadData;
        public bool LoadData
        {
            set
            {
                loadData = value;
            }
        }

        public string SearchCriteria(string op)
        {
            // latitude/longitude
            bool bLatFrom;  // true means no data entry
            bool bLatTo;
            bool bLongFrom;
            bool bLongTo;

            float latFrom = 0;
            float latTo = 0;
            float longFrom = 0;
            float longTo = 0;

            float temp;

            StringBuilder sbCriteria = new StringBuilder(); 

            if (rblDegree.SelectedIndex == 0)
            {
                bLatFrom = (txtLAFD1.Text == "") && (txtLAFD2.Text == "") && (txtLAFD3.Text == "");
                bLatTo = (txtLATD1.Text == "") && (txtLATD2.Text == "") && (txtLATD3.Text == "");

                bLongFrom = (txtLGFD1.Text == "") && (txtLGFD2.Text == "") && (txtLGFD3.Text == "");
                bLongTo = (txtLGTD1.Text == "") && (txtLGTD2.Text == "") && (txtLGTD3.Text == "");

                if (!bLatFrom)
                {
                    latFrom = Toolkit.ToInt32(txtLAFD1.Text, 0) + Toolkit.ToFloat(txtLAFD2.Text, 0) / 60 + Toolkit.ToFloat(txtLAFD3.Text, 0) / 3600;
                    if (ddlLAF.SelectedIndex == 1) latFrom = latFrom * -1;
                }
                if (!bLatTo)
                {
                    latTo = Toolkit.ToInt32(txtLATD1.Text, 0) + Toolkit.ToFloat(txtLATD2.Text, 0) / 60 + Toolkit.ToFloat(txtLATD3.Text, 0) / 3600;
                    if (ddlLAT.SelectedIndex == 1) latTo = latTo * -1;
                }

                if (!bLongFrom)
                {
                    longFrom = Toolkit.ToInt32(txtLGFD1.Text, 0) + Toolkit.ToFloat(txtLGFD2.Text, 0) / 60 + Toolkit.ToFloat(txtLGFD3.Text, 0) / 3600;
                    if (ddlLGF.SelectedIndex == 1) longFrom = longFrom * -1;
                }
                if (!bLongTo)
                {
                    longTo = Toolkit.ToInt32(txtLGTD1.Text, 0) + Toolkit.ToFloat(txtLGTD2.Text, 0) / 60 + Toolkit.ToFloat(txtLGTD3.Text, 0) / 3600;
                    if (ddlLGT.SelectedIndex == 1) longTo = longTo * -1;
                }
            }
            else
            {
                bLatFrom = (txtLAFDC.Text  == "");
                bLatTo = (txtLATDC.Text == "");

                bLongFrom = (txtLGFDC.Text == "");
                bLongTo = (txtLGTDC.Text == "");

                latFrom = Toolkit.ToFloat(txtLAFDC.Text, 0);
                latTo = Toolkit.ToFloat(txtLATDC.Text, 0);

                longFrom = Toolkit.ToFloat(txtLGFDC.Text, 0);
                longTo = Toolkit.ToFloat(txtLGTDC.Text, 0);
            }

            if (latFrom != latTo)
            {
                if (!bLatFrom)
                {
                    if (!bLatTo)
                    {
                        if (latFrom > latTo)
                        {
                            temp = latFrom;
                            latFrom = latTo;
                            latTo = temp;
                        }
                        sbCriteria.Append(" (@accession_source.latitude >= " + latFrom + " and @accession_source.latitude <= " + latTo + ") ");
                    }
                    else
                        sbCriteria.Append(" (@accession_source.latitude >= " + latFrom + ") ");
                }
                else
                {
                    sbCriteria.Append(" (@accession_source.latitude <= " + latTo + ") ");
                }
            }
            else
            {
                 if (!bLatFrom) sbCriteria.Append(" (@accession_source.latitude = " + latFrom + ") "); // from and to are the same entered number, make it 'equal' to this special number
            }

            if (longFrom != longTo)
            {
                if (!bLongFrom)
                {
                    if (!bLongTo)
                    {
                        if (longFrom > longTo)
                        {
                            temp = longFrom;
                            longFrom = longTo;
                            longTo = temp;
                        }
                        if (sbCriteria.Length > 0)
                            sbCriteria.Append(op).Append(" (@accession_source.longitude >= " + longFrom + " and @accession_source.longitude <= " + longTo + ") ");
                        else
                            sbCriteria.Append(" (@accession_source.longitude >= " + longFrom + " and @accession_source.longitude <= " + longTo + ") ");
                    }
                    else
                    {
                        if (sbCriteria.Length > 0)
                            sbCriteria.Append(op).Append(" (@accession_source.longitude >= " + longFrom + ") ");
                        else
                            sbCriteria.Append(" (@accession_source.longitude >= " + longFrom + ") ");
                    }
                }
                else
                {
                    if (sbCriteria.Length > 0)
                        sbCriteria.Append(op).Append(" (@accession_source.longitude <= " + longTo + ") ");
                    else
                        sbCriteria.Append(" (@accession_source.longitude <= " + longTo + ") ");
                }
            }
            else
            {
                if (!bLongFrom) 
                {
                    if (sbCriteria.Length > 0)
                        sbCriteria.Append(op).Append(" (@accession_source.longitude = " + longFrom + ") "); // from and to are the same entered number, make it 'equal' to this special number
                    else
                        sbCriteria.Append(" (@accession_source.longitude = " + longFrom + ") ");
                }
            }

            // elevation
            bool bEleFrom;  // true means no data entry
            bool bEleTo;
 
            int eleFrom = 0;
            int eleTo = 0;

            int tempE;

            bEleFrom = (txtEleF.Text  == "");
            bEleTo = (txtEleT.Text == "");


            eleFrom = Toolkit.ToInt32(txtEleF.Text, 0);
            eleTo = Toolkit.ToInt32(txtEleT.Text, 0);

            if (eleFrom != eleTo)
            {
                if (!bEleFrom)
                {
                    if (!bEleTo)
                    {
                        if (eleFrom > eleTo)
                        {
                            tempE = eleFrom;
                            eleFrom = eleTo;
                            eleTo = tempE;
                        }
                        if (sbCriteria.Length > 0)
                            sbCriteria.Append(op).Append(" (@accession_source.elevation_meters >= " + eleFrom + " and @accession_source.elevation_meters <= " + eleTo + ") ");
                        else
                            sbCriteria.Append(" (@accession_source.elevation_meters >= " + eleFrom + " and @accession_source.elevation_meters <= " + eleTo + ") ");
                    }
                    else
                    {   
                        if (sbCriteria.Length > 0)
                            sbCriteria.Append(op).Append(" (@accession_source.elevation_meters >= " + eleFrom + ") ");
                        else
                            sbCriteria.Append(" (@accession_source.elevation_meters >= " + eleFrom + ") ");
                    }
                }
                else
                {
                    if (sbCriteria.Length > 0)
                        sbCriteria.Append(op).Append(" (@accession_source.elevation_meters <= " + eleTo + ") ");
                    else
                        sbCriteria.Append(" (@accession_source.elevation_meters <= " + eleTo + ") ");
                }
            }
            else
            {
                if (!bEleFrom) 
                {
                    if (sbCriteria.Length > 0)
                        sbCriteria.Append(op).Append(" (@accession_source.elevation_meters = " + eleFrom + ") "); // from and to are the same entered number, make it 'equal' to this special number
                    else
                        sbCriteria.Append(" (@accession_source.elevation_meters = " + eleFrom + ") ");
                }
            }

            // collecting date

            bool bDateFrom;  // true means no data entry, or invalid date
            bool bDateTo;

            DateTime dateFrom = DateTime.MinValue;
            DateTime dateTo = DateTime.MinValue;

            DateTime tempD;

            bDateFrom = (txtDateF.Text == "" || (Toolkit.ToDateTime(txtDateF.Text) == DateTime.MinValue));
            bDateTo = (txtDateT.Text == "" || (Toolkit.ToDateTime(txtDateT.Text) == DateTime.MinValue));

            dateFrom = Toolkit.ToDateTime(txtDateF.Text);
            dateTo = Toolkit.ToDateTime(txtDateT.Text);

            if (dateFrom != dateTo)
            {
                if (!bDateFrom)
                {
                    if (!bDateTo)
                    {
                        if (dateFrom > dateTo)
                        {
                            tempD = dateFrom;
                            dateFrom = dateTo;
                            dateTo = tempD;
                        }
                        // add @accession_source.source_type_code = 'COLLECTED'?
                        if (sbCriteria.Length > 0)
                            sbCriteria.Append(op).Append(" (@accession_source.source_date >= '" + dateFrom + "' and @accession_source.source_date <= '" + dateTo + "') ");
                        else
                            sbCriteria.Append(" (@accession_source.source_date >= '" + dateFrom + "' and @accession_source.source_date <= '" + dateTo + "') ");
                    }
                    else
                    {
                        if (sbCriteria.Length > 0)
                            sbCriteria.Append(op).Append(" (@accession_source.source_date >= '" + dateFrom + "') ");
                        else
                            sbCriteria.Append(" (@accession_source.source_date >= '" + dateFrom + "') ");
                    }
                }
                else
                {
                    if (sbCriteria.Length > 0)
                        sbCriteria.Append(op).Append(" (@accession_source.source_date <= '" + dateTo + "') ");
                    else
                        sbCriteria.Append(" (@accession_source.source_date <= '" + dateTo + "') ");
                }
            }
            else
            {
                if (!bDateFrom)
                {
                    if (sbCriteria.Length > 0)
                        sbCriteria.Append(op).Append(" (@accession_source.source_date = '" + dateFrom + "') "); // from and to are the same entered number, make it 'equal' to this special number
                    else
                        sbCriteria.Append(" (@accession_source.source_date = '" + dateFrom + "') ");
                }
            }

            // collecting note

            string note = txtNote.Text;
            note = Utils.Sanitize(note);
            if (note != "")
            {
                if (ddlOperator.SelectedItem.Text == "Contains")
                {
                    if (sbCriteria.Length > 0)
                        sbCriteria.Append(op).Append(" ( (@accession_source.collector_verbatim_locality like '%" + note + "%') or (@accession_source.environment_description like '%" + note + "%') or (@accession_source.note like '%" + note + "%') )");
                    else
                        sbCriteria.Append(" ( (@accession_source.collector_verbatim_locality like '%" + note + "%') or (@accession_source.environment_description like '%" + note + "%') or (@accession_source.note like '%" + note + "%') )");
                }
                else
                {
                    if (sbCriteria.Length > 0)
                        sbCriteria.Append(op).Append(" ( (@accession_source.collector_verbatim_locality = '" + note + "') or (@accession_source.environment_description = '" + note + "') or (@accession_source.note = '" + note + "') )");
                    else
                        sbCriteria.Append(" ( (@accession_source.collector_verbatim_locality = '" + note + "') or (@accession_source.environment_description = '" + note + "') or (@accession_source.note = '" + note + "') )");
                }
            }

            return sbCriteria.ToString();
        }

        public string SearchCriteriaDisplay()
        {
            string indent = "&nbsp; &nbsp; &nbsp;";

            // latitude and longitude search display
            bool bLatFrom;  // true means no data entry
            bool bLatTo;
            bool bLongFrom;
            bool bLongTo;

            float latFrom = 0;
            float latTo = 0;
            float longFrom = 0;
            float longTo = 0;

            float temp;

            StringBuilder sbCriteria = new StringBuilder();
            
            if (rblDegree.SelectedIndex == 0)
            {
                bLatFrom = (txtLAFD1.Text == "") && (txtLAFD2.Text == "") && (txtLAFD3.Text == "");
                bLatTo = (txtLATD1.Text == "") && (txtLATD2.Text == "") && (txtLATD3.Text == "");

                bLongFrom = (txtLGFD1.Text == "") && (txtLGFD2.Text == "") && (txtLGFD3.Text == "");
                bLongTo = (txtLGTD1.Text == "") && (txtLGTD2.Text == "") && (txtLGTD3.Text == "");

                if (!bLatFrom)
                {
                    latFrom = Toolkit.ToInt32(txtLAFD1.Text, 0) + Toolkit.ToFloat(txtLAFD2.Text, 0) / 60 + (Toolkit.ToFloat(txtLAFD3.Text, 0) / 3600);
                    if (ddlLAF.SelectedIndex == 1) latFrom = latFrom * -1;
                }
                if (!bLatTo)
                {
                    latTo = Toolkit.ToInt32(txtLATD1.Text, 0) + Toolkit.ToFloat(txtLATD2.Text, 0) / 60 + Toolkit.ToFloat(txtLATD3.Text, 0) / 3600;
                    if (ddlLAT.SelectedIndex == 1) latTo = latTo * -1;
                }

                if (!bLongFrom)
                {
                    longFrom = Toolkit.ToInt32(txtLGFD1.Text, 0) + Toolkit.ToFloat(txtLGFD2.Text, 0) / 60 + Toolkit.ToFloat(txtLGFD3.Text, 0) / 3600;
                    if (ddlLGF.SelectedIndex == 1) longFrom = longFrom * -1;
                }
                if (!bLongTo)
                {
                    longTo = Toolkit.ToInt32(txtLGTD1.Text, 0) + Toolkit.ToFloat(txtLGTD2.Text, 0) / 60 + Toolkit.ToFloat(txtLGTD3.Text, 0) / 3600;
                    if (ddlLGT.SelectedIndex == 1) longTo = longTo * -1;
                }

            }
            else
            {
                bLatFrom = (txtLAFDC.Text == "");
                bLatTo = (txtLATDC.Text == "");

                bLongFrom = (txtLGFDC.Text == "");
                bLongTo = (txtLGTDC.Text == "");

                latFrom = Toolkit.ToFloat(txtLAFDC.Text, 0);
                latTo = Toolkit.ToFloat(txtLATDC.Text, 0);

                longFrom = Toolkit.ToFloat(txtLGFDC.Text, 0);
                longTo = Toolkit.ToFloat(txtLGTDC.Text, 0);
            }

            if (latFrom != latTo)
            {
                if (!bLatFrom)
                {
                    if (!bLatTo)
                    {
                        if (latFrom > latTo)
                        {
                            temp = latFrom;
                            latFrom = latTo;
                            latTo = temp;
                        }
                        sbCriteria.Append(" latitude between " + latFrom + " and " + latTo + "; ");
                    }
                    else
                        sbCriteria.Append(" latitude is greater than " + latFrom + "; ");
                }
                else
                {
                    sbCriteria.Append(" latitude is less than " + latTo + "; ");
                }
            }
            else
            {
                if (!bLatFrom) sbCriteria.Append(" latitude is " + latFrom + "; ");  // from and to are the same entered number, make it 'equal' to this special number
            }

            if (longFrom != longTo)
            {
                if (!bLongFrom)
                {
                    if (!bLongTo)
                    {
                        if (longFrom > longTo)
                        {
                            temp = longFrom;
                            longFrom = longTo;
                            longTo = temp;
                        }
                        sbCriteria.Append(" longitude between " + longFrom + " and " + longTo + "; ");
                    }
                    else
                        sbCriteria.Append(" longitude is greater than " + longFrom + "; ");
                }
                else
                {
                    sbCriteria.Append(" longitude is less than " + longTo + "; ");
                }
            }
            else
            {
                if (!bLongFrom) sbCriteria.Append(" longitude is " + longFrom + "; "); // from and to are the same entered number, make it 'equal' to this special number
            }

            // elevation search display
            bool bEleFrom;  // true means no data entry
            bool bEleTo;

            int eleFrom = 0;
            int eleTo = 0;

            int tempE;

            bEleFrom = (txtEleF.Text == "");
            bEleTo = (txtEleT.Text == "");


            eleFrom = Toolkit.ToInt32(txtEleF.Text, 0);
            eleTo = Toolkit.ToInt32(txtEleT.Text, 0);

            if (eleFrom != eleTo)
            {
                if (!bEleFrom)
                {
                    if (!bEleTo)
                    {
                        if (eleFrom > eleTo)
                        {
                            tempE = eleFrom;
                            eleFrom = eleTo;
                            eleTo = tempE;
                        }
                        sbCriteria.Append(" elevation between " + eleFrom + "m and " + eleTo + "m; ");
                    }
                    else
                        sbCriteria.Append(" elevation is greater than " + eleFrom + "m; ");
                }
                else
                        sbCriteria.Append(" elevation is less than " + eleTo + "m; ");
            }
            else
            {
                if (!bEleFrom) sbCriteria.Append(" elevation is " + eleFrom + "m; ");
            }

            // Collecting date

            bool bDateFrom;  // true means no data entry or invalid date entry
            bool bDateTo;

            DateTime dateFrom = DateTime.MinValue;
            DateTime dateTo = DateTime.MinValue;

            DateTime tempD;

            bDateFrom = (txtDateF.Text == "" || (Toolkit.ToDateTime(txtDateF.Text) == DateTime.MinValue));
            bDateTo = (txtDateT.Text == "" || (Toolkit.ToDateTime(txtDateT.Text) == DateTime.MinValue));

            dateFrom = Toolkit.ToDateTime(txtDateF.Text);
            dateTo = Toolkit.ToDateTime(txtDateT.Text);

            if (dateFrom != dateTo)
            {
                if (!bDateFrom)
                {
                    if (!bDateTo)
                    {
                        if (dateFrom > dateTo)
                        {
                            tempD = dateFrom;
                            dateFrom = dateTo;
                            dateTo = tempD;
                        }
                        sbCriteria.Append(" collecting date between " + dateFrom.ToShortDateString() + " and " + dateTo.ToShortDateString() + "; ");
                    }
                    else
                        sbCriteria.Append(" collecting date is later than " + dateFrom.ToShortDateString() + "; ");
                }
                else
                    sbCriteria.Append(" collecting date is earlier than " + dateTo.ToShortDateString() + "; ");
            }
            else
            {
                if (!bDateFrom) sbCriteria.Append(" collecting date is " + dateFrom.ToShortDateString() + "; ");
            }

            // collecting note

            string note = txtNote.Text;
            if (note != "")
            {
                if (ddlOperator.SelectedItem.Text == "Contains")
                    sbCriteria.Append(" collecting note contains '" + note + "'; ");
                 else
                    sbCriteria.Append(" collecting note equals '" + note + "'; ");
             }

            if (sbCriteria.Length > 0)
                return indent + "collecting site: " + sbCriteria.ToString();
            else
                return "";
        }

        protected void rblDegree_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblDegree.SelectedIndex == 0)
            {
                txtLAFDC.Text = "";
                txtLAFDC.Visible = false;
                txtLATDC.Text = "";
                txtLATDC.Visible = false;

                txtLGFDC.Text = "";
                txtLGFDC.Visible = false;
                txtLGTDC.Text = "";
                txtLGTDC.Visible = false;

                txtLAFD1.Text = "";
                txtLAFD2.Text = "";
                txtLAFD3.Text = "";
                ddlLAF.SelectedIndex = 0;
                ddlLAF.Visible = true;
                pnlLatF.Visible = true;

                txtLATD1.Text = "";
                txtLATD2.Text = "";
                txtLATD3.Text = "";
                ddlLAT.SelectedIndex = 0;
                ddlLAT.Visible = true;
                pnlLatT.Visible = true;

                txtLGFD1.Text = "";
                txtLGFD2.Text = "";
                txtLGFD3.Text = "";
                ddlLGF.SelectedIndex = 0;
                ddlLGF.Visible = true;
                pnlLogF.Visible = true;

                txtLGTD1.Text = "";
                txtLGTD2.Text = "";
                txtLGTD3.Text = "";
                ddlLGT.SelectedIndex = 0;
                ddlLGT.Visible = true;
                pnlLogT.Visible = true;
            }
            else
            {
                txtLAFDC.Text = "";
                txtLAFDC.Visible = true;
                txtLATDC.Text = "";
                txtLATDC.Visible = true;

                txtLGFDC.Text = "";
                txtLGFDC.Visible = true;
                txtLGTDC.Text = "";
                txtLGTDC.Visible = true;

                txtLAFD1.Text = "";
                txtLAFD2.Text = "";
                txtLAFD3.Text = "";
                ddlLAF.SelectedIndex = 0;
                ddlLAF.Visible = false;
                pnlLatF.Visible = false;

                txtLATD1.Text = "";
                txtLATD2.Text = "";
                txtLATD3.Text = "";
                ddlLAT.SelectedIndex = 0;
                ddlLAT.Visible = false;
                pnlLatT.Visible = false;

                txtLGFD1.Text = "";
                txtLGFD2.Text = "";
                txtLGFD3.Text = "";
                ddlLGF.SelectedIndex = 0;
                ddlLGF.Visible = false;
                pnlLogF.Visible = false;

                txtLGTD1.Text = "";
                txtLGTD2.Text = "";
                txtLGTD3.Text = "";
                ddlLGT.SelectedIndex = 0;
                ddlLGT.Visible = false;
                pnlLogT.Visible = false;
            }
            divLatLongToggleOn = true;
        }

        public void ClearCriteria()
        {
            if (rblDegree.SelectedIndex == 0)
            {
                txtLAFD1.Text = "";
                txtLAFD2.Text = "";
                txtLAFD3.Text = "";
                ddlLAF.SelectedIndex = 0;
                ddlLAF.Visible = true;
                pnlLatF.Visible = true;

                txtLATD1.Text = "";
                txtLATD2.Text = "";
                txtLATD3.Text = "";
                ddlLAT.SelectedIndex = 0;
                ddlLAT.Visible = true;
                pnlLatT.Visible = true;

                txtLGFD1.Text = "";
                txtLGFD2.Text = "";
                txtLGFD3.Text = "";
                ddlLGF.SelectedIndex = 0;
                ddlLGF.Visible = true;
                pnlLogF.Visible = true;

                txtLGTD1.Text = "";
                txtLGTD2.Text = "";
                txtLGTD3.Text = "";
                ddlLGT.SelectedIndex = 0;
                ddlLGT.Visible = true;
                pnlLogT.Visible = true;
            }
            else
            {
                txtLAFDC.Text = "";
                txtLAFDC.Visible = true;
                txtLATDC.Text = "";
                txtLATDC.Visible = true;

                txtLGFDC.Text = "";
                txtLGFDC.Visible = true;
                txtLGTDC.Text = "";
                txtLGTDC.Visible = true;
            }

            txtEleF.Text = "";
            txtEleT.Text = "";

            txtDateF.Text = "";
            txtDateT.Text = "";

            ddlOperator.SelectedIndex = 0;
            txtNote.Text = "";
            divLatLongToggleOn = true;
        }

        protected void btnClearLL_Click(object sender, EventArgs e)
        {
            ClearCriteria(); 
        }

     }
}