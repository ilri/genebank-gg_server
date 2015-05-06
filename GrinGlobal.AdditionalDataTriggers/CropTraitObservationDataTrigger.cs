using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;
using System.Data;

namespace GrinGlobal.AdditionalDataTriggers {
    public class CropTraitObservationDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            // pre-row save trigger
 
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var helper = args.Helper;

                // Check that code agrees with trait
                if (helper.AllFieldsExist("crop_trait_id", "crop_trait_code_id")) {
                    int ctid = (int)helper.GetValue("crop_trait_id", 0, true);
                    int ctcid = (int)helper.GetValue("crop_trait_code_id", 0, true);
                    if (ctid > 0 && ctcid > 0) {
                        var dtCode = args.ReadData(@"SELECT * FROM crop_trait_code WHERE crop_trait_id = :ctid AND crop_trait_code_id = :ctcid", ":ctid", ctid, ":ctcid", ctcid);
                        if (dtCode.Rows.Count < 1) {
                            args.Cancel("Crop Trait Code must agree with Crop Trait");
                            return;
                        }
                    }
                }

                // validate data column with descriptor data column
                if (helper.AllFieldsExist("crop_trait_code_id", "string_value", "numeric_value")) 
                {
                    //    args.Cancel("I can see the three value fields - 3000 sleep");
                    
                    int ctid = (int)helper.GetValue("crop_trait_id", 0, true);
                    int ctcid = (int)helper.GetValue("crop_trait_code_id", 0, true);
                    var strg = helper.GetValue("string_value", "", true).ToString();
                    double numb = System.Convert.ToDouble(helper.GetValue("numeric_value", 0, true));

                    var dtDsc = args.ReadData(@"SELECT data_type_code,is_coded,coalesce(numeric_maximum,0) max,coalesce(numeric_minimum,0) min FROM crop_trait WHERE crop_trait_id = :ctid ", ":ctid", ctid);
                    string dtype = dtDsc.Rows[0]["data_type_code"].ToString();
                    string coded = dtDsc.Rows[0]["is_coded"].ToString();
                    double min = System.Convert.ToDouble(dtDsc.Rows[0]["min"]);
                    double max = System.Convert.ToDouble(dtDsc.Rows[0]["max"]);

                    // check for coded trait type
                    if (coded == "Y" && ctcid < 1)
                    {
                          args.Cancel("Trait is coded but no data in code value column.");
                          return;
                    }

                    // check if CHAR and empty string
                    if (dtype == "CHAR" && coded == "N" && strg == "")
                    {
                        args.Cancel("Trait is string but no data in string value column.");
                        return;
                    }

                    // check for numeric val and min/max if NUMERIC
                    if (dtype == "NUMERIC" && coded == "N" )
                    {
                        if (numb == 0)
                        {
                            args.Cancel("Trait is numeric but no data in numeric value column.");
                            return;
                        }
                        if (min > 0 && numb < min)
                        {
                            args.Cancel("Numeric value less than minimum specified for trait.");
                            return;
                        }
                        if (max > 0 && numb > max)
                        {
                            args.Cancel("Numeric value greater than specified for trait.");
                            return;
                        }
                    }

                }


                // validate taxonomy crop against crop

                // FIRST LOOK FOR A VALID COMBINATION OF THAT SPECIES AND CROP ALREADY IN THE DATABASE in taxonomy_crop_map if found continue
                // if not found look for any entry for that species in taxonomy_crop_map - a hit means that species already has a different crop and the match is potentially bad, throw error. If it is a legit
                //     new crop for that species, someone will have to add that combo to t_c_map
                // if no good or bad match found, add entry to t_c_m and continue - list will self generate
                // 
                if (helper.AllFieldsExist("inventory_id"))
                {
                  //  System.Threading.Thread.Sleep(5000);
                    int ivid = (int)helper.GetValue("inventory_id", 0, true);
                    int dno = (int)helper.GetValue("crop_trait_id", 0, true);

                    // look up taxonomy species id for accession
                    var dtTax = args.ReadData(@"select taxonomy_species_id from accession a, inventory i where i.accession_id = a.accession_id and i.inventory_id = :ivid", ":ivid", ivid);
                    int taxno = (int)dtTax.Rows[0]["taxonomy_species_id"];
                    
                    // look up crop id from trait being loaded
                    var dtCrop = args.ReadData(@"select crop_id from crop_trait where crop_trait_id = :dno", ":dno", dno);
                    int cropno = (int)dtCrop.Rows[0]["crop_id"];

                    // alternate_crop_name = string N/A separates obs entries from crop relative entries
                    // first check for a positive match between the species and crop in taxonomy_crop_map
                    var dtObsT = args.ReadData(@"select crop_id from taxonomy_crop_map where crop_id=:cropno and taxonomy_species_id=:taxno and alternate_crop_name='N/A'", ":cropno", cropno, ":taxno", taxno);
                    if (dtObsT.Rows.Count >= 1) { return; };  //  found match on species and crop so good

                    // next check for any entry as that means that species is linked to a different crop
                    //   var dtObsF = args.ReadData(@"select top 1 obid from ob2 where taxno=:taxno", ":taxno", taxno); // only need to check for ANY obs
                    var dtObsF = args.ReadData(@"select taxonomy_species_id from taxonomy_crop_map where taxonomy_species_id=:taxno and alternate_crop_name = 'N/A'", ":taxno", taxno); // only need to check for ANY obs
                   if (dtObsF.Rows.Count >= 1) 
                   {
                        args.Cancel("Accession species and crop species do not match.");
                        return;
                   }
                    
                   // must be a new combination so add to taxonomy_crop_map
                   int owned_by = (int)helper.GetValue("owned_by", 0, true);
                   int created_by = (int)helper.GetValue("created_by", 0, true);
                   DateTime owned_date = DateTime.UtcNow;
                   DateTime created_date = DateTime.UtcNow;
                   args.WriteData(@"
                       insert into taxonomy_crop_map (crop_id,taxonomy_species_id,owned_by,owned_date,created_by,created_date,alternate_crop_name,is_primary_genepool,is_secondary_genepool, 
                        is_tertiary_genepool,is_quaternary_genepool, is_graftstock_genepool) 
                        values (:cropno,:taxno,:owned_by,:owned_date,:created_by,:created_date,'N/A','N','N','N','N','N')",
                         ":cropno", cropno, DbType.String,
                         ":taxno", taxno, DbType.String,
                         ":owned_by", owned_by, DbType.Int32,
                         ":owned_date", owned_date, DbType.DateTime2,
                         ":created_by", created_by, DbType.Int32,
                         ":created_date", created_date, DbType.DateTime2
                          );
               }
            }
        }
        #region IAsyncFilter Members
        public bool IsAsynchronous
        {
            get { return false; }
        }
        public object Clone()
        {
            return new CropTraitObservationDataTrigger();
        }
        #endregion

        public override string GetDescription(string ietfLanguageTag) {
            return "Checks CropTraitObservation.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "CropTraitObservation Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "crop_trait_observation" };
            }
        }
    }
}