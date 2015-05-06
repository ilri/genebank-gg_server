using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using GrinGlobal.Business;
using System.Data;
using GrinGlobal.Core;


namespace GrinGlobal.Web.taxon
{
    public partial class cwrgeneticcontrol : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bindData();
        }

        private string cropid;
        public string Crop
        {
            get { return cropid; }
            set { cropid = value; }
        }

        private int cropcount;
        public int CropCount
        {
            get { return cropcount; }
            set { cropcount = value; }
        }

        private bool primary;
        public bool Primary
        {
            get { return primary; }
            set { primary = value; }
        }

        private bool secondary;
        public bool Secondary
        {
            get { return secondary; }
            set { secondary = value; }
        }

        private bool tertiary;
        public bool Tertiary
        {
            get { return tertiary; }
            set { tertiary = value; }
        }

        private bool gratfstock;
        public bool Gratfstock
        {
            get { return gratfstock; }
            set { gratfstock = value; }
        }

        private bool hasdata;
        public bool HasData
        {
            get { return hasdata; }
            set { hasdata = value; }
        }

        private string sqlbase;
        public string SqlBase
        {
            set { sqlbase = value; }
        }

        private void bindData()
        {
            using (var sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    try
                    {
                        string sql = "";
                        DataTable dt = null;

//                        // crop
//                        var dt = dm.Read(@"
//                                            select alternate_crop_name, crop_genepool_reviewers, crop_id from taxonomy_crop_map where crop_id = :cropid and crop_genepool_reviewers is not null",
//                        new DataParameters(":cropid", cropid));

//                        if (dt.Rows.Count > 0)
//                        {
//                            DataRow dr = dt.Rows[0];
//                            lblCrop.Text = "Crop:  " + dr["alternate_crop_name"].ToString();

//                            lblCompiled.Text = "(" + dr["crop_genepool_reviewers"].ToString() + ")";
//                        }

//                        // crop taxon
//                        if (cropcount > 1)
//                            lblCropTaxon.Text = "Crop taxa:";
//                        else
//                            lblCropTaxon.Text = "Crop taxon:";

//                        rptTaxon.DataSource = sd.GetData("web_taxonomycwr_croptaxon", ":cropid=" + cropid, 0, 0).Tables["web_taxonomycwr_croptaxon"];
//                        rptTaxon.DataBind();


                        // genepool
                        if (Primary)
                        {
                            sql = sqlbase + " and tcm.crop_id = " + Toolkit.ToInt32(cropid, 0) + " and tcm.is_primary_genepool  = 'Y' order by t.name";

                            dt = dm.Read(sql);
                            if (dt.Rows.Count > 0)
                            {
                                hasdata = true;

                                rptPrimary.DataSource = dt;
                                rptPrimary.DataBind();
                            }
                        }

                        if (Secondary)
                        {
                            sql = sqlbase + " and tcm.crop_id = " + Toolkit.ToInt32(cropid, 0) + " and tcm.is_secondary_genepool = 'Y' order by t.name";

                            dt = dm.Read(sql);
                            if (dt.Rows.Count > 0)
                            {
                                hasdata = true;

                                rptSecondary.DataSource = dt;
                                rptSecondary.DataBind();
                            }
                        }

                        if (Tertiary)
                        {
                            sql = sqlbase + " and tcm.crop_id = " + Toolkit.ToInt32(cropid, 0) + " and tcm.is_tertiary_genepool = 'Y' order by t.name";

                            dt = dm.Read(sql);
                            if (dt.Rows.Count > 0)
                            {
                                hasdata = true;

                                rptTertiary.DataSource = dt;
                                rptTertiary.DataBind();
                            }
                        }

                        if (Gratfstock)
                        {
                            sql = sqlbase + " and tcm.crop_id = " + Toolkit.ToInt32(cropid, 0) + " and tcm.is_graftstock_genepool  = 'Y' order by t.name";

                            dt = dm.Read(sql);
                            if (dt.Rows.Count > 0)
                            {
                                hasdata = true;

                                rptGraftstock.DataSource = dt;
                                rptGraftstock.DataBind();
                            }
                        }

                        if (hasdata)  // only show when there is some cwr data
                        {
                            // crop
                            dt = dm.Read(@"
                                            select alternate_crop_name, crop_genepool_reviewers, crop_id from taxonomy_crop_map where crop_id = :cropid and crop_genepool_reviewers is not null",
                            new DataParameters(":cropid", cropid));

                            if (dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.Rows[0];
                                lblCrop.Text = "Crop:  " + dr["alternate_crop_name"].ToString();

                                lblCompiled.Text = "(" + dr["crop_genepool_reviewers"].ToString() + ")";
                            }

                            // crop taxon
                            if (cropcount > 1)
                                lblCropTaxon.Text = "Crop taxa:";
                            else
                                lblCropTaxon.Text = "Crop taxon:";

                            rptTaxon.DataSource = sd.GetData("web_taxonomycwr_croptaxon", ":cropid=" + cropid, 0, 0).Tables["web_taxonomycwr_croptaxon"];
                            rptTaxon.DataBind();
                            pnlControl.Visible = true;
                        }
                        else
                            pnlControl.Visible = false;
                    }
                    catch (Exception e)
                    {
                        string err = e.Message;
                        pnlControl.Visible = false;
                    }
                }
            }
        }
    }
}