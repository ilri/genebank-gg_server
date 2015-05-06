using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.AdditionalDataTriggers {
    public class AccessionSourceDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var helper = args.Helper;

                // 2) require both lat and long if either
                if (helper.AllFieldsExist("latitude", "longitude")) {
                    //args.Cancel("both logitude and latitude exist");
                    if (!helper.IsValueEmpty("latitude") && helper.IsValueEmpty("longitude")) {
                        args.Cancel("must have logitude if latitude entered");
                    }
                    if (helper.IsValueEmpty("latitude") && !helper.IsValueEmpty("longitude")) {
                        args.Cancel("must have latitude if logitude entered");
                    }
                }

                if (helper.AllFieldsExist("geography_id") && !helper.IsValueEmpty("geography_id")) {
                    int gid = (int)helper.GetValue("geography_id", 0, true);
                    var dtGeo = args.ReadData(@"SELECT current_geography_id, country_code FROM geography WHERE geography_id = :gid", ":gid", gid, DbType.Int32);
                    if (dtGeo.Rows.Count < 1) {
                        args.Cancel("Geography could not be validated.");
                    } else {
                        int currentid = (int)dtGeo.Rows[0]["current_geography_id"];
                        string iso3 = dtGeo.Rows[0]["country_code"].ToString();

                        // Use current geo
                        if (gid != currentid) {
                            helper.SetValue("geography_id", currentid, typeof(int), false);
                            dtGeo = args.ReadData(@"SELECT current_geography_id, country_code FROM geography WHERE geography_id = :gid", ":gid", gid, DbType.Int32);
                            if (dtGeo.Rows.Count < 1) {
                                args.Cancel("Current geography could not be validated.");
                            }
                        }

                        // 1) Only allow link to valid geo
                        int num;
                        if (int.TryParse(iso3, out num)) {
                            args.Cancel("Cannot use invalid geography with a numeric contry code");
                        }
                    }
                }


            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Checks Accession Source.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "AccessionSource Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "acession_source" };
            }
        }
    }
}
