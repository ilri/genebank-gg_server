using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using GrinGlobal.Business;
using System.Text.RegularExpressions;

namespace GrinGlobal.Web.help
{
    public partial class abouttaxonomy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
            if (Request.QueryString["chapter"] != null)
            {
                string chapter = Request.QueryString["chapter"].ToString();
                switch (chapter)
                {
                    case "summ":
                        DisplaySummary(sender, e);
                        break;
                    case "intro":
                        DisplayIntroduction(sender, e);  
                        break;
                    case "hist":
                        DisplayHistory(sender, e);  
                        break;
                    case "scope":
                        DisplayScope(sender, e);  
                        break;
                    case "scient":
                        DisplayScientificName(sender, e); 
                        break;
                    case "common":
                        DisplayCommonName(sender, e); 
                        break;
                    case "econ":
                        DisplayEconomicImportance(sender, e); 
                        break;
                    case "distrib":
                        DisplayDistribution(sender, e); 
                        break;
                    case "liter":
                        DisplayReference(sender, e);  
                        break;
                    case "spec":
                        DisplaySpecial(sender, e); 
                        break;
                    case "basis":
                        DisplayDecisionBasis(sender, e);
                        break;
                    case "concl":
                        DisplayRemarks(sender, e); 
                        break;
                    case "acknowl":
                        DisplayAcknowledgements(sender, e); 
                        break;
                    case "refer":
                        DisplayCited(sender, e); 
                        break;
                    case "symb":
                        DisplaySymbols(sender, e);  
                        break;
                    default:
                        displayDefault();
                        break;

                }
            }
            else
                displayDefault();
        }

        private void displayDefault()
        {
            pnlIndex.Visible = true;
            pnlSumm.Visible = false;
            pnlIntro.Visible = false;
            pnlHist.Visible = false;
            pnlScope.Visible = false;
            pnlScient.Visible = false;
            pnlCommon.Visible = false;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = false;
            pnlSpec.Visible = false;
            pnlBasis.Visible = false;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = false;
            pnlSymb.Visible = false;
        }

        public string accnt, gncnt, acgncnt, igncnt, acigncnt, spcnt, acspcnt, acctaxcnt;
        protected void DisplaySummary(object sender, EventArgs e)  
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    int cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(a.accession_id) from accession a join cooperator c on a.owned_by = c.cooperator_id join site s on c.site_id = s.site_id where s.site_short_name not in ('INACTIVE','NSSB')"), 0);
                    accnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_genus_id) from taxonomy_genus where genus_name not like 'Unident%' and (subgenus_name is null and section_name is null and series_name is null)"), 0);
                    gncnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_genus_id) from taxonomy_genus where genus_name not like 'Unident%' and (subgenus_name is null and section_name is null and series_name is null) and (qualifying_code not like '%=%' or qualifying_code is null)"), 0);
                    acgncnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_genus_id) from taxonomy_genus where genus_name not like 'Unident%' and (subgenus_name is not null or section_name is not null or series_name is not null)"), 0);
                    igncnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_genus_id) from taxonomy_genus where genus_name not like 'Unident%' and (subgenus_name is not null or section_name is not null or series_name is not null) and (qualifying_code not like '%=%' or qualifying_code is null)"), 0); 
                    acigncnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_species_id) from taxonomy_species where species_name not in ('sp.','hybrid')"), 0);
                    spcnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_species_id) from taxonomy_species where taxonomy_species_id = current_taxonomy_species_id and species_name not in ('sp.','hybrid')"), 0); 
                    acspcnt = string.Format("{0:n0}", cnt1);
                }
            }

            pnlIndex.Visible = false;
            pnlSumm.Visible = true;
            pnlIntro.Visible = false;
            pnlHist.Visible = false;
            pnlScope.Visible = false;
            pnlScient.Visible = false;
            pnlCommon.Visible = false;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = false;
            pnlSpec.Visible = false;
            pnlBasis.Visible = false;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = false;
            pnlSymb.Visible = false;
        }

        protected void DisplayIntroduction(object sender, EventArgs e)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    int cnt = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(a.accession_id) from accession a join cooperator c on a.owned_by = c.cooperator_id join site s on c.site_id = s.site_id where s.site_short_name not in ('INACTIVE','NSSB')"), 0);
                    accnt = string.Format("{0:n0}", cnt);
                }
            }
            pnlIndex.Visible = false;
            pnlSumm.Visible = false;
            pnlIntro.Visible = true;
            pnlHist.Visible = false;
            pnlScope.Visible = false;
            pnlScient.Visible = false;
            pnlCommon.Visible = false;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = false;
            pnlSpec.Visible = false;
            pnlBasis.Visible = false;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = false;
            pnlSymb.Visible = false;
        }

        protected void DisplayHistory(object sender, EventArgs e)
        {
            pnlIndex.Visible = false;
            pnlSumm.Visible = false;
            pnlIntro.Visible = false;
            pnlHist.Visible = true;
            pnlScope.Visible = false;
            pnlScient.Visible = false;
            pnlCommon.Visible = false;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = false;
            pnlSpec.Visible = false;
            pnlBasis.Visible = false;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = false;
            pnlSymb.Visible = false;
        }

        protected void DisplayScope(object sender, EventArgs e)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    int cnt = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(distinct a.taxonomy_species_id) from accession a join cooperator c on a.owned_by = c.cooperator_id join site s on c.site_id = s.site_id where s.site_short_name not in ('INACTIVE','NSSB')"), 0);
                    acctaxcnt = string.Format("{0:n0}", cnt);
                }
            }
            pnlIndex.Visible = false;
            pnlSumm.Visible = false;
            pnlIntro.Visible = false;
            pnlHist.Visible = false;
            pnlScope.Visible = true;
            pnlScient.Visible = false;
            pnlCommon.Visible = false;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = false;
            pnlSpec.Visible = false;
            pnlBasis.Visible = false;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = false;
            pnlSymb.Visible = false;
        }

        public string syngncnt, infragencnt, infrafamcnt, synspcnt, bispcnt, trispcnt, quadspcnt;
        protected void DisplayScientificName(object sender, EventArgs e)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    int cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_genus_id) from taxonomy_genus where genus_name not like 'Unident%' and (subgenus_name is null and section_name is null and series_name is null) and (qualifying_code not like '%=%' or qualifying_code is null)"), 0);
                    acgncnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_genus_id) from taxonomy_genus where genus_name not like 'Unident%' and (subgenus_name is null and section_name is null and series_name is null) and qualifying_code like '%=%'"), 0);
                    syngncnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(distinct genus_name) from taxonomy_genus where (subgenus_name is not null or section_name is not null or series_name is not null) and (qualifying_code not like '%=%' or qualifying_code is null)"), 0);
                    infragencnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(distinct family_name) from taxonomy_family where (subfamily_name is not null or tribe_name is not null or subtribe_name is not null) and taxonomy_family_id = current_taxonomy_family_id"), 0);
                    infrafamcnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_species_id) from taxonomy_species where taxonomy_species_id = current_taxonomy_species_id and species_name not in ('sp.','hybrid')"), 0);
                    acspcnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_species_id) from taxonomy_species where taxonomy_species_id != current_taxonomy_species_id and species_name not in ('sp.','hybrid')"), 0);
                    synspcnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_species_id) from taxonomy_species where species_name not in ('sp.','hybrid') and name not like '% % % %'"), 0);
                    bispcnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_species_id) from taxonomy_species where species_name not in ('sp.','hybrid') and (subspecies_name is not null or variety_name is not null or subvariety_name is not null or forma_name is not null)"), 0);
                    trispcnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_species_id) from taxonomy_species where species_name not in ('sp.','hybrid') and subspecies_name is not null and (variety_name is not null or subvariety_name is not null or forma_name is not null)"), 0);
                    quadspcnt = string.Format("{0:n0}", cnt1);
                }
            }

            pnlIndex.Visible = false;
            pnlSumm.Visible = false;
            pnlIntro.Visible = false;
            pnlHist.Visible = false;
            pnlScope.Visible = false;
            pnlScient.Visible = true;
            pnlCommon.Visible = false;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = false;
            pnlSpec.Visible = false;
            pnlBasis.Visible = false;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = false;
            pnlSymb.Visible = false;
        }

        public string cncnt, taxcncnt, langcncnt, gncncnt;
        protected void DisplayCommonName(object sender, EventArgs e)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    int cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_common_name_id) from taxonomy_common_name where taxonomy_species_id is not null"), 0);
                    cncnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(distinct taxonomy_species_id) from taxonomy_common_name where taxonomy_species_id is not null"), 0);
                    taxcncnt = string.Format("{0:n0}", cnt1);
                    
                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_common_name_id) from taxonomy_common_name where language_description not like 'English%' and taxonomy_species_id is not null"), 0);
                    langcncnt = string.Format("{0:n0}", cnt1);
                    
                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_common_name_id) from taxonomy_common_name where taxonomy_genus_id is not null"), 0);
                    gncncnt = string.Format("{0:n0}", cnt1);
                }
            }

            pnlIndex.Visible = false;
            pnlSumm.Visible = false;
            pnlIntro.Visible = false;
            pnlHist.Visible = false;
            pnlScope.Visible = false;
            pnlScient.Visible = false;
            pnlCommon.Visible = true;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = false;
            pnlSpec.Visible = false;
            pnlBasis.Visible = false;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = false;
            pnlSymb.Visible = false;
        }

        public string econcnt, taxeconcnt;
        protected void DisplayEconomicImportance(object sender, EventArgs e)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    int cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_use_id) from taxonomy_use where economic_usage_code not in ('CPC','FWT','FWE')"), 0);
                    econcnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(distinct taxonomy_species_id) from taxonomy_use where economic_usage_code not in ('CPC','FWT','FWE')"), 0);
                    taxeconcnt = string.Format("{0:n0}", cnt1);
                }
            }

            pnlIndex.Visible = false;
            pnlSumm.Visible = false;
            pnlIntro.Visible = false;
            pnlHist.Visible = false;
            pnlScope.Visible = false;
            pnlScient.Visible = false;
            pnlCommon.Visible = false;
            pnlEcon.Visible = true;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = false;
            pnlSpec.Visible = false;
            pnlBasis.Visible = false;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = false;
            pnlSymb.Visible = false;
        }

        public string distcnt, taxdistcnt;
        protected void DisplayDistribution(object sender, EventArgs e)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    int cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(taxonomy_geography_map_id) from taxonomy_geography_map"), 0);
                    distcnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(distinct taxonomy_species_id) from taxonomy_geography_map"), 0);
                    taxdistcnt = string.Format("{0:n0}", cnt1);
                }
            }

            pnlIndex.Visible = false;
            pnlSumm.Visible = false;
            pnlIntro.Visible = false;
            pnlHist.Visible = false;
            pnlScope.Visible = false;
            pnlScient.Visible = false;
            pnlCommon.Visible = false;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = true;
            pnlLiter.Visible = false;
            pnlSpec.Visible = false;
            pnlBasis.Visible = false;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = false;
            pnlSymb.Visible = false;
        }

        public string litcnt, tcitcnt, actcitcnt, syntcitcnt, gcitcnt;
        protected void DisplayReference(object sender, EventArgs e)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    int cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(literature_id) from literature"), 0);
                    litcnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"
                    select count(distinct cit.citation_id) from  citation cit 
                    join literature l on cit.literature_id = l.literature_id
                    where cit.taxonomy_species_id is not null   
                    and l.abbreviation not in ('PEAS','ISTA','AH 505','AOSA Hb 25','World Econ Pl')"), 0);
                    tcitcnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"
                    select count(distinct cit.taxonomy_species_id) from taxonomy_species ts  
                    join citation cit on ts.taxonomy_species_id = cit.taxonomy_species_id  
                    join literature l on cit.literature_id = l.literature_id
                    where cit.taxonomy_species_id is not null
                    and  ts.taxonomy_species_id = ts.current_taxonomy_species_id
                    and l.abbreviation not in ('PEAS','ISTA','AH 505','AOSA Hb 25','World Econ Pl')"), 0);
                    actcitcnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(distinct cit.taxonomy_species_id) from taxonomy_species ts  
                    join citation cit on ts.taxonomy_species_id = cit.taxonomy_species_id  
                    join literature l on cit.literature_id = l.literature_id
                    where cit.taxonomy_species_id is not null
                    and  ts.taxonomy_species_id != ts.current_taxonomy_species_id
                    and l.abbreviation not in ('PEAS','ISTA','AH 505','AOSA Hb 25','World Econ Pl')"), 0);
                    syntcitcnt = string.Format("{0:n0}", cnt1);

                    cnt1 = Toolkit.ToInt32(dm.ReadValue(@"  
                    select count(citation_id) from citation where taxonomy_genus_id is not null"), 0);
                    gcitcnt = string.Format("{0:n0}", cnt1);
                }
            }

            pnlIndex.Visible = false;
            pnlSumm.Visible = false;
            pnlIntro.Visible = false;
            pnlHist.Visible = false;
            pnlScope.Visible = false;
            pnlScient.Visible = false;
            pnlCommon.Visible = false;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = true;
            pnlSpec.Visible = false;
            pnlBasis.Visible = false;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = false;
            pnlSymb.Visible = false;
        }

        protected void DisplaySpecial(object sender, EventArgs e)
        {
            pnlIndex.Visible = false;
            pnlSumm.Visible = false;
            pnlIntro.Visible = false;
            pnlHist.Visible = false;
            pnlScope.Visible = false;
            pnlScient.Visible = false;
            pnlCommon.Visible = false;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = false;
            pnlSpec.Visible = true;
            pnlBasis.Visible = false;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = false;
            pnlSymb.Visible = false;
        }

        protected void DisplayDecisionBasis(object sender, EventArgs e)
        {
            pnlIndex.Visible = false;
            pnlSumm.Visible = false;
            pnlIntro.Visible = false;
            pnlHist.Visible = false;
            pnlScope.Visible = false;
            pnlScient.Visible = false;
            pnlCommon.Visible = false;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = false;
            pnlSpec.Visible = false;
            pnlBasis.Visible = true;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = false;
            pnlSymb.Visible = false;
        }

        protected void DisplayRemarks(object sender, EventArgs e)
        {
            pnlIndex.Visible = false;
            pnlSumm.Visible = false;
            pnlIntro.Visible = false;
            pnlHist.Visible = false;
            pnlScope.Visible = false;
            pnlScient.Visible = false;
            pnlCommon.Visible = false;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = false;
            pnlSpec.Visible = false;
            pnlBasis.Visible = false;
            pnlConcl.Visible = true;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = false;
            pnlSymb.Visible = false;
        }

        protected void DisplayAcknowledgements(object sender, EventArgs e)
        {
            pnlIndex.Visible = false;
            pnlSumm.Visible = false;
            pnlIntro.Visible = false;
            pnlHist.Visible = false;
            pnlScope.Visible = false;
            pnlScient.Visible = false;
            pnlCommon.Visible = false;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = false;
            pnlSpec.Visible = false;
            pnlBasis.Visible = false;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = true;
            pnlRefer.Visible = false;
            pnlSymb.Visible = false;
        }

        protected void DisplayCited(object sender, EventArgs e)
        {
            pnlIndex.Visible = false;
            pnlSumm.Visible = false;
            pnlIntro.Visible = false;
            pnlHist.Visible = false;
            pnlScope.Visible = false;
            pnlScient.Visible = false;
            pnlCommon.Visible = false;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = false;
            pnlSpec.Visible = false;
            pnlBasis.Visible = false;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = true;
            pnlSymb.Visible = false;
        }

        protected void DisplaySymbols(object sender, EventArgs e)
        {
            pnlIndex.Visible = false;
            pnlSumm.Visible = false;
            pnlIntro.Visible = false;
            pnlHist.Visible = false;
            pnlScope.Visible = false;
            pnlScient.Visible = false;
            pnlCommon.Visible = false;
            pnlEcon.Visible = false;
            pnlDistrib.Visible = false;
            pnlLiter.Visible = false;
            pnlSpec.Visible = false;
            pnlBasis.Visible = false;
            pnlConcl.Visible = false;
            pnlAcknowl.Visible = false;
            pnlRefer.Visible = false;
            pnlSymb.Visible = true;
        }

        protected void btnSummPre_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnSummCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnSummNext_Click(object sender, EventArgs e)
        {
            DisplayIntroduction(sender, e);
        }

        protected void btnIntroPre_Click(object sender, EventArgs e)
        {
            DisplaySummary(sender, e);
        }

        protected void btnIntroCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnIntroNext_Click(object sender, EventArgs e)
        {
            DisplayHistory(sender, e);
        }

        protected void btnHistPre_Click(object sender, EventArgs e)
        {
            DisplayIntroduction(sender, e);
        }

        protected void btnHistCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnHistNext_Click(object sender, EventArgs e)
        {
            DisplayScope(sender, e);
        }

        protected void btnScopePre_Click(object sender, EventArgs e)
        {
            DisplayHistory(sender, e);
        }

        protected void btnScopeCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnScopeNext_Click(object sender, EventArgs e)
        {
            DisplayScientificName(sender, e);
        }

        protected void btnScientPre_Click(object sender, EventArgs e)
        {
            DisplayScope(sender, e);
        }

        protected void btnScientCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnScientNext_Click(object sender, EventArgs e)
        {
            DisplayCommonName(sender, e);
        }

        protected void btnCommonPre_Click(object sender, EventArgs e)
        {
            DisplayScientificName(sender, e);
        }

        protected void btnCommonCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnCommonNext_Click(object sender, EventArgs e)
        {
            DisplayEconomicImportance(sender, e);
        }

        protected void btnEconPre_Click(object sender, EventArgs e)
        {
            DisplayCommonName(sender, e);
        }

        protected void btnEconCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnEconNext_Click(object sender, EventArgs e)
        {
            DisplayDistribution(sender, e);
        }

        protected void btnDistribPre_Click(object sender, EventArgs e)
        {
            DisplayEconomicImportance(sender, e);
        }

        protected void btnDistribCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnDistribNext_Click(object sender, EventArgs e)
        {
            DisplayReference(sender, e);
        }

        protected void btnLiterPre_Click(object sender, EventArgs e)
        {
            DisplayDistribution(sender, e);
        }

        protected void btnLiterCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnLiterNext_Click(object sender, EventArgs e)
        {
            DisplaySpecial(sender, e);
        }

        protected void btnSpecPre_Click(object sender, EventArgs e)
        {
            DisplayReference(sender, e);
        }

        protected void btnSpecCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnSpecNext_Click(object sender, EventArgs e)
        {
            DisplayDecisionBasis(sender, e);
        }

        protected void btnBasisPre_Click(object sender, EventArgs e)
        {
            DisplaySpecial(sender, e);
        }

        protected void btnBasisCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnBasisNext_Click(object sender, EventArgs e)
        {
            DisplayRemarks(sender, e);
        }

        protected void btnConclPre_Click(object sender, EventArgs e)
        {
            DisplayDecisionBasis(sender, e);
        }

        protected void btnConclCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnConclNext_Click(object sender, EventArgs e)
        {
            DisplayAcknowledgements(sender, e);
        }

        protected void btnAcknowlPre_Click(object sender, EventArgs e)
        {
            DisplayRemarks(sender, e);
        }

        protected void btnAcknowlCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnAcknowlNext_Click(object sender, EventArgs e)
        {
            DisplayCited(sender, e);
        }

        protected void btnReferPre_Click(object sender, EventArgs e)
        {
            DisplayAcknowledgements(sender, e);
        }

        protected void btnReferCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }

        protected void btnReferNext_Click(object sender, EventArgs e)
        {
            DisplaySymbols(sender, e);
        }

        protected void btnSymbPre_Click(object sender, EventArgs e)
        {
            DisplayCited(sender, e);
        }

        protected void btnSymbCont_Click(object sender, EventArgs e)
        {
            displayDefault();
        }
    }
}
