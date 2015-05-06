using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;
using System.Data;
using GrinGlobal.Core;

namespace GrinGlobal.AdditionalDataTriggers
{
    public class TaxonomyCommonNameDataTrigger : TableDataTriggerAdapter
    {

        public override void TableRowSaving(ISaveDataTriggerArgs args)
        {
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update)
            {
                var h = args.Helper;

                // make sure 1 and only 1 of gensu/species is filled

                if ((h.IsValueEmpty("taxonomy_genus_id") && h.IsValueEmpty("taxonomy_species_id")) || (!h.IsValueEmpty("taxonomy_genus_id") && !h.IsValueEmpty("taxonomy_species_id")))
                {
                    args.Cancel("Either you have neither or both genus and species filled in. One and only one column is allowed.");
                    return;
                }

                //
                // create simplified name
                var cname = h.GetValue("name", "", true).ToString().ToUpper();
                string cname2 = cname.Replace("-","").Replace("'","").Replace(" ","");
                cname2 = cname2.Replace("&Aacute;","A");
                cname2 = cname2.Replace("&aacute;","a");
                cname2 = cname2.Replace("&Eacute;","E");
                cname2 = cname2.Replace("&eacute;","e");
                cname2 = cname2.Replace("&Iacute;","I");
                cname2 = cname2.Replace("&iacute;","i");
                cname2 = cname2.Replace("&Oacute;","O");
                cname2 = cname2.Replace("&oacute;","o");
                cname2 = cname2.Replace("&Uacute;","U");
                cname2 = cname2.Replace("&uacute;","u");
                cname2 = cname2.Replace("&yacute;","y");
                cname2 = cname2.Replace("&abreve;","a");
                cname2 = cname2.Replace("&gbreve;","g");
                cname2 = cname2.Replace("&#301;","i");
                cname2 = cname2.Replace("&Ccaron;","C");
                cname2 = cname2.Replace("&ccaron;","c");
                cname2 = cname2.Replace("&Ecaron;","E");
                cname2 = cname2.Replace("&ecaron;","e");
                cname2 = cname2.Replace("&Rcaron;","R");
                cname2 = cname2.Replace("&rcaron;","r");
                cname2 = cname2.Replace("&Scaron;","S");
                cname2 = cname2.Replace("&scaron;","s");
                cname2 = cname2.Replace("&Zcaron;","Z");
                cname2 = cname2.Replace("&zcaron;","z");
                cname2 = cname2.Replace("&Ccedil;","C");
                cname2 = cname2.Replace("&ccedil;","c");
                cname2 = cname2.Replace("&Scedil;","S");
                cname2 = cname2.Replace("&scedil;","s");
                cname2 = cname2.Replace("&acirc;","a");
                cname2 = cname2.Replace("&ecirc;","e");
                cname2 = cname2.Replace("&Icirc;","I");
                cname2 = cname2.Replace("&icirc;","i");
                cname2 = cname2.Replace("&ocirc;","o");
                cname2 = cname2.Replace("&scirc;","s");
                cname2 = cname2.Replace("&ucirc;","u");
                cname2 = cname2.Replace("&agrave;","a");
                cname2 = cname2.Replace("&egrave;","e");
                cname2 = cname2.Replace("&igrave;","i");
                cname2 = cname2.Replace("&ograve;","o");
                cname2 = cname2.Replace("&Aring;","A");
                cname2 = cname2.Replace("&aring;","a");
                cname2 = cname2.Replace("&Oslash;","O");
                cname2 = cname2.Replace("&oslash;","o");
                cname2 = cname2.Replace("&aelig;","ae");
                cname2 = cname2.Replace("&oelig;","oe");
                cname2 = cname2.Replace("&szlig;","ss");
                cname2 = cname2.Replace("&atilde;","a");
                cname2 = cname2.Replace("&Ntilde;","N");
                cname2 = cname2.Replace("&ntilde;","n");
                cname2 = cname2.Replace("&otilde;","o");
                cname2 = cname2.Replace("&Auml;","A");
                cname2 = cname2.Replace("&auml;","a");
                cname2 = cname2.Replace("&euml;","e");
                cname2 = cname2.Replace("&iuml;","i");
                cname2 = cname2.Replace("&Ouml;","O");
                cname2 = cname2.Replace("&ouml;","o");
                cname2 = cname2.Replace("&Uuml;","U");
                cname2 = cname2.Replace("&uuml;","u");

                h.SetValue("simplified_name", cname2, typeof(string), false);
                //args.Cancel("new name = " + cname2);
                //return;
            }
        }

        public override string GetDescription(string ietfLanguageTag)
        {
            return "Ensure either genus or species is selected on initial insert.";
        }

        public override string GetTitle(string ietfLanguageTag)
        {
            return "Taxonomy Common Name Data Trigger";
        }
        public override string[] ResourceNames
        {
            get
            {
                return new string[] { "taxonomy_common_name" };
            }
        }
    }
}
