using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;
using System.Data;
using System.Text.RegularExpressions;

namespace GrinGlobal.AdditionalDataTriggers {
    public class TaxonomyFamilyDataTrigger : TableDataTriggerAdapter {

        public void CheckAuthority(ISaveDataTriggerArgs args, string authorities)
        {
            //convert all author construction syntax to &
            authorities = Regex.Replace(authorities, @"\(|\)| ex | ex\. | Ex\. | ,ex | And | Et | non | sensu ", " & ");
            // removefrom evaluation
            authorities = Regex.Replace(authorities, "et al.", "");
            string[] authList = authorities.Split('&');
            foreach (string auth in authList)
            {
                string authClean = auth.Trim();
                if (!String.IsNullOrEmpty(authClean))
                {
                    var dtAuth = args.ReadData(@"SELECT * FROM taxonomy_author WHERE short_name = :auth", ":auth", authClean, DbType.String);
                    if (dtAuth.Rows.Count < 1)
                    {
                        args.Cancel("Family author " + authClean + " could not be validated.");
                    }
                }
            }
        } 

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update)
            {
                var h = args.Helper;

                // family, subfamily, tribe, subtribe set to initcap
                //    

                string family = h.GetValue("family_name", "", true).ToString().ToLower();
                family = family.Substring(0, 1).ToUpper() + family.Substring(1);
                h.SetValue("family_name", family, typeof(string), false);

               // var dt = args.ReadData(@"SELECT system_user as person");
               // string user = dt.Rows[0]["person"].ToString();
               // h.SetValue("note", user, typeof(string), false);


                if (h.FieldExists("subfamily_name") && !h.IsValueEmpty("subfamily_name")) 
                {
                    string subfamily = h.GetValue("subfamily_name", "", true).ToString().ToLower();
                    subfamily = subfamily.Substring(0, 1).ToUpper() + subfamily.Substring(1);
                    h.SetValue("subfamily_name", subfamily, typeof(string), false);
                }
                
               if (h.FieldExists("tribe_name") && !h.IsValueEmpty("tribe_name"))
               {
                    string tribe = h.GetValue("tribe_name", "", true).ToString().ToLower();
                    tribe = tribe.Substring(0, 1).ToUpper() + tribe.Substring(1);
                    h.SetValue("tribe_name", tribe, typeof(string), false);
               }

               if (h.FieldExists("subtribe_name") && !h.IsValueEmpty("subtribe_name")) 
               {
                  string subtribe = h.GetValue("subtribe_name", "", true).ToString().ToLower();
                  subtribe = subtribe.Substring(0, 1).ToUpper() + subtribe.Substring(1);
                  h.SetValue("subtribe_name", subtribe, typeof(string), false);
               }
                   
                // set alt family if conserved
                /*" SELECT DECODE (:NEW.family,'Asteraceae','Compositae','Apiaceae','Umbelliferae','Arecaceae','Palmae',Brassicaceae','Cruciferae','Clusiaceae','Guttiferae','Fabaceae','Leguminosae','Lamiaceae','Labiatae','Poaceae','Gramineae',NULL) INTO altfam FROM dual; :NEW.altfamily := altfam;  */
               switch (family)
                    {
                        case "Asteraceae":
                            h.SetValue("alternate_name", "Compositae", typeof(string), false);
                        break;
                        case "Apiaceae":
                        h.SetValue("alternate_name", "Umbelliferae", typeof(string), false);
                        break;
                        case "Arecaceae":
                        h.SetValue("alternate_name", "Palmae", typeof(string), false);
                        break;
                        case "Brassicaceae":
                        h.SetValue("alternate_name", "Cruciferae", typeof(string), false);
                        break;
                        case "Clusiaceae":
                        h.SetValue("alternate_name", "Guttiferae", typeof(string), false);
                        break;
                        case "Fabaceae":
                        h.SetValue("alternate_name", "Leguminosae", typeof(string), false);
                        break;
                        case "Lamiaceae":
                        h.SetValue("alternate_name", "Labiatae", typeof(string), false);
                        break;
                        case "Poaceae":
                        h.SetValue("alternate_name", "Gramineae", typeof(string), false);
                        break;

                    }

                    // 4 check family author
                   if (h.FieldExists("family_authority") && !h.IsValueEmpty("family_authority"))
                   {
                       CheckAuthority(args, h.GetValue("family_authority", "", false).ToString());
                   }


            }
        }

        public override void TableRowSaved(ISaveDataTriggerArgs args) {
            if (args.SaveMode == SaveMode.Insert) {
                var h = args.Helper;

                // make current_taxonomy_family_id field match the taxonomy_family_id field
                if (h.IsValueEmpty("current_taxonomy_family_id")) {
         //           args.WriteData(@"update taxonomy_family set current_taxonomy_family_id = :id1 where taxonomy_family_id = :id2", ":id1", args.NewPrimaryKeyID, DbType.Int32, ":id2", args.NewPrimaryKeyID, DbType.Int32);
                    args.WriteData(@"update taxonomy_family set current_taxonomy_family_id = taxonomy_family_id where taxonomy_family_id = :id1", ":id1", args.NewPrimaryKeyID, DbType.Int32);

                }
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Updates current_taxonomy_family_id to point at new record on initial insert.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Taxonomy Family Data Trigger";
        }
        public override string[] ResourceNames {
            get {
                return new string[] { "taxonomy_family" };
            }
        }
    }
}
