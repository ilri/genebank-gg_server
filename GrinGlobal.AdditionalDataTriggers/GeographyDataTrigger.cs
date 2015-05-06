using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.AdditionalDataTriggers {
    public class GeographyDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaved(ISaveDataTriggerArgs args) {
            if (args.SaveMode == SaveMode.Insert) {
                if (args.Helper.IsValueEmpty("current_geography_id")) {
                    // make current_geography_id field match the geography_id field
                    args.WriteData(@"
update 
    geography
set 
    current_geography_id = :id1
where 
    geography_id = :id2
", ":id1", args.NewPrimaryKeyID, ":id2", args.NewPrimaryKeyID);
                }

            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Updates current_geography_id to point at new record on initial insert.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Geography Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "geography" };
            }
        }
    }
}
