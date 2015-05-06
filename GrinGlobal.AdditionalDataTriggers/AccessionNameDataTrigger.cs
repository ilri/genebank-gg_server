using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.AdditionalDataTriggers
{
    public class AccessionNameDataTrigger : TableDataTriggerAdapter
    {

//        public override void TableRowSaved(ISaveDataTriggerArgs args)
        public override void TableRowSaving(ISaveDataTriggerArgs args)
        {
            if (args.SaveMode == SaveMode.Update || args.SaveMode == SaveMode.Insert)
            {
                var h = args.Helper;

                const string CategoryCode = "category_code";
                const string PlantNameRank = "plant_name_rank";

                Dictionary<string, int> RankCategory = new Dictionary<string, int>();
                RankCategory.Add("CULTIVAR", 10);
                RankCategory.Add("UNVERIFIED", 20);
                RankCategory.Add("LOCALNAME", 30);
                RankCategory.Add("CGIAR", 35);
                RankCategory.Add("INSTITUTE", 40);
                RankCategory.Add("DEVELOPER", 50);
                RankCategory.Add("DONOR", 60);
                RankCategory.Add("COLLECTOR", 70);
                RankCategory.Add("SITE", 80);
                RankCategory.Add("QUARANTINE", 90);
                RankCategory.Add("OTHER", 100);
                RankCategory.Add("DUPLICATE", 110);
                RankCategory.Add("MISIDENT", 120);
                RankCategory.Add("TRADEMARK", 130);

                if (h.FieldExists(CategoryCode) && !h.IsValueEmpty(CategoryCode))
                {
                    var catVal = h.GetValue(CategoryCode, "", false) as string;
                    if (RankCategory.ContainsKey(catVal))
                    {
                        // Set Rank
                        if (h.IsValueEmpty(PlantNameRank))
                        {
                            h.SetValue(PlantNameRank, RankCategory[catVal], typeof(int), false);
                        }
                    }
                }

            }
        }

        public override void TableRowSaved(ISaveDataTriggerArgs args)
        {
        }

        public override string GetDescription(string ietfLanguageTag)
        {
            return "Updates accession name rank.";
        }

        public override string GetTitle(string ietfLanguageTag)
        {
            return "Accession Name  Data Trigger";
        }

        public override string[] ResourceNames
        {
            get
            {
                return new string[] { "accession_name" };
            }
        }
    }
}
