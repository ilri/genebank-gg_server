using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.AdditionalDataTriggers {
    public class ImportAccessionDataTrigger : DataViewTriggerAdapter {
        public override void DataViewRowSaveFailed(ISaveDataTriggerArgs args) {
            // TODO: return a nice error if we couldn't find the taxonomy_species_id...
            args.Cancel("The given taxon could not be located in the database.");
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Returns a 'nice' error message if record could not be found in taxonomy_species table for given the input.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Import Accession Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "import_accession", "import_accession_with_names" };
            }
        }
    }
}
