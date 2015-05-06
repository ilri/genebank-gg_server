using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GrinGlobal.Interface.Dataviews;

namespace GrinGlobal.Interface.DataTriggers {
    /// <summary>
    /// Options passed by a client when calling SaveData().  No options specified = default CT processing.  This class was introduced primarily to support Import Wizard functionality, yet remain backwardly compatible with existing CT behavior.
    /// </summary>
    public class SaveOptions {

        /// <summary>
        /// If true, primary key values are looked up from the database using the alternate key fields defined by the table mappings.  The primary key values are then added to the DataRow and commands are then processed like normal.  Default is false.
        /// </summary>
        public bool UseUniqueKeys;

        /// <summary>
        /// If true, will default boolean fields to false ("N").  Default is false (user must specify explicit values for boolean fields or an exception is raised)
        /// </summary>
        public bool BoolDefaultIsFalse;
        /// <summary>
        /// Causes an Insert/Update to occur only if all required fields have non-DBNull.Value values.  i.e. won't cause failures because some optional fields are specified but required ones are not.  Default is false.
        /// </summary>
        public bool OnlyIfAllRequiredFieldsExist;
        /// <summary>
        /// Overrides the default LanguageID for a user when writing data to the database.  Default is null.
        /// </summary>
        public int? AltLanguageID;
        /// <summary>
        /// If true, causes the middle tier to check not only the modified date, but also individual field values to make sure data hasn't changed since the client last refreshed.  i.e. don't overwrite data that has changed since last time I saw it.  Default is true.
        /// </summary>
        public bool SafeUpdatesAndDeletes;
        /// <summary>
        /// If true, causes the SearchEngineDataTrigger to skip its processing.  Used primarily by Import Wizard since it performs so much data change, it's easier to just regenerate the indexes in full afterwards.  Default is false.
        /// </summary>
        public bool SkipSearchEngineUpdates;
        /// <summary>
        /// If true, prevents Update/Insert from being called on all tables that do not end with "_lang".  Useful for importing dataview language data, but not changing the dataview definition itself (i.e. if an additional dataview field is specified in the input that does not exist in the dataview definition, it is ignored).  Default is false.
        /// </summary>
        public bool InsertOnlyLanguageData;
        /// <summary>
        /// If BackgroundWorker is not null, progress will be reported back to the caller not on every row save, but after RowProgressInterval rows have been saved.  If BackgroundWorker is null, this is ignored.  Default is 100.
        /// </summary>
        public int RowProgressInterval;
        /// <summary>
        /// The cooperator_id to assign as the owner of the records saved instead of the current user's cooperator_id.  If user is not a member of 'ADMINS' group, this is forced to match the current user's cooperator_id.  Default is current user's cooperator_id.
        /// </summary>
        public int OwnerID;
        /// <summary>
        /// The object used to notify the caller of progress.  Unavailable from a web service call.  Only used by Import Wizard so far.  Default is null.
        /// </summary>
        public BackgroundWorker BackgroundWorker;
        /// <summary>
        /// A request-local cache of field names that have already been resolved to minimize lookups.
        /// </summary>
        public Dictionary<IField, string> CachedFieldNames;
        /// <summary>
        /// A request-local cache of temp data that has already been resolved to minimize lookups.
        /// </summary>
        public Dictionary<string, object> CachedTempData;

        /// <summary>
        /// Creates a deep copy of this object.
        /// </summary>
        /// <returns></returns>
        public SaveOptions Clone() {
            var rv = new SaveOptions(null);
            rv.UseUniqueKeys = this.UseUniqueKeys;
            rv.BoolDefaultIsFalse = this.BoolDefaultIsFalse;
            rv.OnlyIfAllRequiredFieldsExist = this.OnlyIfAllRequiredFieldsExist;
            rv.AltLanguageID = this.AltLanguageID;
            rv.SafeUpdatesAndDeletes = this.SafeUpdatesAndDeletes;
            rv.SkipSearchEngineUpdates = this.SkipSearchEngineUpdates;
            rv.InsertOnlyLanguageData = this.InsertOnlyLanguageData;
            rv.RowProgressInterval = this.RowProgressInterval;
            rv.OwnerID = this.OwnerID;
            rv.BackgroundWorker = this.BackgroundWorker;
            //foreach(IField f in this.CachedFieldNames.Keys){
            //    rv.CachedFieldNames[f] = this.CachedFieldNames[f];
            //}
            return rv;
        }

        /// <summary>
        /// Initializes a new instance of SaveOptions, parsing out the given options into appropriate properties.
        /// </summary>
        /// <param name="options"></param>
        public SaveOptions(string options) {

            AltLanguageID = null;
            SafeUpdatesAndDeletes = true;
            RowProgressInterval = 100;
            OwnerID = -1;
            CachedFieldNames = new Dictionary<IField, string>();
            CachedTempData = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(options)) {
                var dic = Util.ParsePairs<string>(options);
                foreach (var key in dic.Keys) {
                    switch (key.ToLower()) {
                        case "useuniquekeys":
                            UseUniqueKeys = Util.ToBoolean(dic[key], false);
                            break;
                        case "booldefaultfalse":
                            BoolDefaultIsFalse = Util.ToBoolean(dic[key], false);
                            break;
                        case "onlyifallrequiredfieldsexist":
                            OnlyIfAllRequiredFieldsExist = Util.ToBoolean(dic[key], false);
                            break;
                        case "altlanguageid":
                            AltLanguageID = Util.ToInt32(dic[key], null);
                            break;
                        case "safeupdatesanddeletes":
                            SafeUpdatesAndDeletes = Util.ToBoolean(dic[key], true);
                            break;
                        case "skipsearchengineupdates":
                            SkipSearchEngineUpdates = Util.ToBoolean(dic[key], false);
                            break;
                        case "insertonlylanguagedata":
                            InsertOnlyLanguageData = Util.ToBoolean(dic[key], false);
                            break;
                        case "rowprogressinterval":
                            RowProgressInterval = Util.ToInt32(dic[key], RowProgressInterval);
                            break;
                        case "ownerid":
                            OwnerID = Util.ToInt32(dic[key], OwnerID);
                            break;
                        default:
                            // TODO: add more options here!
                            break;
                    }
                }
            }
        }
    }
}
