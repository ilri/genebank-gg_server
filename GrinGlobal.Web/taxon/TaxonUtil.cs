using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrinGlobal.Web.taxon
{
    public class TaxonUtil
    {
        public static string ItalicTaxon(string taxon)
        {
            string s = taxon;
            s = s.Replace(" subsp. ", "</i> subsp. <i>");
            s = s.Replace(" ssp. ", "</i> subsp. <i>");
            s = s.Replace(" var. ", "</i> var. <i>");
            s = s.Replace(" subvar. ", "</i> subvar. <i>");
            s = s.Replace(" f. ", "</i> f. <i>");
            s = s.Replace(" cv. ", "</i> cv. <i>");
            s = s.Replace(" race. ", "</i> race. <i>");
            s = s.Replace(" sect. ", "</i> sect. <i>");
            s = s.Replace(" unranked ", "</i> [unranked] <i>");
            s = s.Replace(" gr. ", "</i> gr. <i>");
            s = s.Replace(" prol. ", "</i> prol. <i>");
            s = s.Replace(" nothosubsp. ", "</i> nothosubsp. <i>");
            s = s.Replace(" nothossp. ", "</i> nothossp. <i>");
            s = s.Replace(" nothovar. ", "</i> nothovar. <i>");
            s = s.Replace(" aggregate ", "</i> aggr. <i>");


            s = s.Replace(" x ", " <sup>x</sup>");

            return s;
        }

        public static string RemoveTaxon(string name)
        {
            string s = name;
            s = s.Replace(" subsp. ", " ");
            s = s.Replace(" ssp. ", " ");
            s = s.Replace(" var. ", " ");
            s = s.Replace(" subvar. ", " ");
            s = s.Replace(" f. ", " ");
            s = s.Replace(" cv. ", " ");
            s = s.Replace(" race. ", " ");
            s = s.Replace(" sect. ", " ");
            s = s.Replace(" unranked ", " ");
            s = s.Replace(" gr. ", " ");
            s = s.Replace(" prol. ", " ");
            s = s.Replace(" nothosubsp. ", " ");
            s = s.Replace(" nothossp. ", " ");
            s = s.Replace(" nothovar. ", " ");
            s = s.Replace(" aggregate ", " ");

            s = s.Replace(" x ", " ");

            return s;
        }
    }
}
