using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;
using System.Data;
using System.Text.RegularExpressions;

namespace GrinGlobal.AdditionalDataTriggers {
    public class TaxonomyGenusDataTrigger : TableDataTriggerAdapter {

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
                        args.Cancel("Genus author " + authClean + " could not be validated.");
                    }
                }
            }
        } 
        
        public override void TableRowSaving(ISaveDataTriggerArgs args) {
                /* why have FK if it can be overridden
                if (args.SaveMode == SaveMode.Delete && args.Table != null && args.Table.TableName.ToLower() == "taxonomy_genus") {
                    // make sure to disassociate the type_taxonomy_genus_id before deleting (if needed)
                    args.WriteData(@"update taxonomy_family set type_taxonomy_genus_id = null where type_taxonomy_genus_id = :id", ":id", args.OriginalPrimaryKeyID, DbType.Int32);

                    // also remove the 'current' self-reference... May still be required in some of the other DBs
                    args.WriteData(@"update taxonomy_genus set current_taxonomy_genus_id = null where taxonomy_genus_id = :id", ":id", args.OriginalPrimaryKeyID, DbType.Int32);
                }*/

                if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update)
                {
                    var h = args.Helper;
                    /*"if instr(:NEW.genus,'-') = 0 then :NEW.genus := initcap(:NEW.genus);end if;
                 F :NEW.subgenus IS NOT NULL THEN :NEW.subgenus := initcap(:NEW.subgenus);  END IF;
    IF :NEW.section IS NOT NULL THEN :NEW.section := initcap(:NEW.section); END IF;
    IF :NEW.series IS NOT NULL THEN :NEW.series := initcap(:NEW.series); END IF;"*/
                    // 2 initcap various columns

                    string newstr = h.GetValue("genus_name", "", true).ToString().ToLower();
                    if (newstr.IndexOf("-") != -1)
                    {
                        newstr = newstr.Substring(0, 1).ToUpper() + newstr.Substring(1);
                        h.SetValue("genus_name", newstr, typeof(string), false);
                    }

                    if (h.FieldExists("subgenus_name") && !h.IsValueEmpty("subgenus_name")) 
                    {
                        newstr = h.GetValue("subgenus_name", "", true).ToString().ToLower();
                        newstr = newstr.Substring(0, 1).ToUpper() + newstr.Substring(1);
                        h.SetValue("subgenus_name", newstr, typeof(string), false);
                    }

                    bool t_section = false;
                    if (h.FieldExists("section_name") && !h.IsValueEmpty("section_name")) 
                    {
                        newstr = h.GetValue("section_name", "", true).ToString().ToLower();
                        newstr = newstr.Substring(0, 1).ToUpper() + newstr.Substring(1);
                        h.SetValue("section_name", newstr, typeof(string), false);
                        t_section = true;
                    }

                    if (h.FieldExists("subsection_name") && !h.IsValueEmpty("subsection_name")) 
                    {
                                           // 3 check subsection 
                    /*"IF :NEW.subsection IS NOT NULL THEN
        IF :NEW.section IS NULL THEN
            raise_application_error (-20000,
                'Can not enter subsection without section.');
        END IF;
        :NEW.subsection := initcap(:NEW.subsection);
    END IF;"*/
                        if (!t_section)
                        {
                            args.Cancel("Can not enter subsection without section.");
                                return;
                        }

                        newstr = h.GetValue("subsection_name", "", true).ToString().ToLower();
                        newstr = newstr.Substring(0, 1).ToUpper() + newstr.Substring(1);
                        h.SetValue("subsection_name", newstr, typeof(string), false);
                    }

                    bool t_series = false;
                    if (h.FieldExists("series_name") && !h.IsValueEmpty("series_name")) 
                    {
                        newstr = h.GetValue("series_name", "", true).ToString().ToLower();
                        newstr = newstr.Substring(0, 1).ToUpper() + newstr.Substring(1);
                        h.SetValue("series_name", newstr, typeof(string), false);
                        t_series = true;
                    }

                    if (h.FieldExists("subseries_name") && !h.IsValueEmpty("subseries_name")) 
                    {
                        if (!t_series)
                        {
                            args.Cancel("Can not enter subseries without series");
                                return;
                        }
                        newstr = h.GetValue("subseries_name", "", true).ToString().ToLower();
                        newstr = newstr.Substring(0, 1).ToUpper() + newstr.Substring(1);
                        h.SetValue("subseries_name", newstr, typeof(string), false);
                    }

                    // 6 check genus author
                    if (h.FieldExists("genus_authority") && !h.IsValueEmpty("genus_authority"))
                    {  
                        CheckAuthority(args, h.GetValue("genus_authority", "", false).ToString()); 
                    }
                 }

            }

        public override void TableRowSaved(ISaveDataTriggerArgs args) {
            if (args.SaveMode == SaveMode.Insert) {
                var h = args.Helper;

                if (h.IsValueEmpty("current_taxonomy_genus_id")) {
                    // make current_taxonomy_genus_id field match the taxonomy_genus_id field
                    args.WriteData(@"update taxonomy_genus set current_taxonomy_genus_id = :id1 where taxonomy_genus_id = :id2", ":id1", args.NewPrimaryKeyID, DbType.Int32, ":id2", args.NewPrimaryKeyID, DbType.Int32);
                }
                // 6a   handle qual will only warn, row will still be saved
                /*"IF :NEW.qual IS NOT NULL THEN
    t_code := rtrim(:NEW.qual);
    main.verify_code('PROD','GN','QUAL','%',t_code);
    :NEW.qual := t_code;
    IF :NEW.validgno = :NEW.gno AND :NEW.qual <> '~' THEN
        raise_application_error (-20000,
        'Only tilde (~) allowed for QUAL in accepted names.');
    END IF;
ELSIF :NEW.validgno <> :NEW.gno THEN
    :NEW.qual := '=';
END IF;
"*/
                if (h.FieldExists("qualifying_code"))
                {
                    string qual = h.GetValue("qualifying_code", "", true).ToString();
                    int gno = (int)h.GetValue("taxonomy_genus_id", 0, true);
                    int validgno = (int)h.GetValue("current_taxonomy_genus_id", 0, true);
                    if (validgno == 0) { validgno = gno;}

                    if (gno == validgno && !h.IsValueEmpty("qualifying_code"))
                    {
                        if (qual != "~")
                        {
                            args.Cancel("WARNING: only tilde (~) allowed for QUAL in accepted names. Redit the row and correct it.");
                            return;
                        }
                    }

                    // 6 b set qual to = if null and synonym
                    if (gno != validgno && h.IsValueEmpty("qualifying_code"))
                    {
                        args.WriteData(@"update taxonomy_genus set qualifying_code = '=' where taxonomy_genus_id = :id1", ":id1", args.NewPrimaryKeyID, DbType.Int32);
  
                    }

                    
                }

            }
        }

        public override string GetDescription(string ietfLanguageTag) {
        //    return "Updates current_taxonomy_genus_id to point at new record on initial insert. Also removes any reference from taxonomy_family.type_taxonomy_genus_id when corresponding taxonomy_genus record is deleted.";
            return "Updates current_taxonomy_genus_id to point at new record on initial insert. ";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Taxonomy Genus Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[]{"taxonomy_genus"};
            }
        }

    }
}
