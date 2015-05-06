using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;
using System.Data;
using System.Globalization;
using System.Threading;

namespace GrinGlobal.AdditionalDataTriggers {
    public class CooperatorDataTrigger : ITableSaveDataTrigger {
        #region ITableSaveFilter Members

        public void TableSaving(ISaveDataTriggerArgs args) {
            // pre-table save trigger
        }

        public void TableRowSaving(ISaveDataTriggerArgs args) {
            // pre-row save trigger

            if (args.SaveMode == SaveMode.Update || args.SaveMode == SaveMode.Insert) {

                // init helper with the row we want to save and the row in the database (which may be null on an insert)
                // The Helper class simply makes sure we don't try to pull values from fields that are not in the current DataRow object.
                var fh = args.Helper;

                // set the current site, userid, created date (at time of creating record for insert...not commit...rest is commit)
                // TODO: userid / created date / modified date done automatically by middle tier -- but not site_code... which I have no idea how to auto-determine...



                // make sure either last name or organization has been provided
                if (fh.IsValueEmpty("last_name") && fh.IsValueEmpty("organization")) {
                    args.Cancel("Either last name or organization must be specified");
                    return;
                }

                if (fh.AllFieldsExist("first_name", "last_name")) {
                    if (!fh.IsValueEmpty("first_name") && fh.IsValueEmpty("last_name")) {
                        args.Cancel("If first name is specified, last name must also be specified.");
                        return;
                    }
                }


                // set the new cooperator_id (cno)
                // cannot do this until after the row is written for cross-db-engine compatibility
                // as sql server / mysql do not use a sequence concept and instead use an 
                // identity / autoincrement concept which requires row to be inserted first
                // before the value can be read.  See the TableRowSaved() method in this class.
                fh.SetValue("first_name", fh.ToTitleCase("first_name"), typeof(string), false);
                fh.SetValue("last_name", fh.ToTitleCase("last_name"), typeof(string), false);


                // On new row...set "active" flag to X
                if (args.SaveMode == SaveMode.Insert) {
                    // new cooperator, make them active
                    fh.SetValue("status_code", "ACTIVE", typeof(string), true);
                } else if (args.SaveMode == SaveMode.Update) {
                    // marking current_cooperator_id same as pk value, make them active
                    if (fh.AllFieldsExist("cooperator_id", "current_cooperator_id")) {
                        if (fh.GetValue("cooperator_id", -1, true) == fh.GetValue("current_cooperator_id", -2, true)) {
                            fh.SetValue("status_code", "ACTIVE", typeof(string), true);
                        }
                    }
                }

                // FK ensures this, no longer needed.
                //// validate geography_id...make sure we have a good geo (state/country)
                //if (!fh.IsValueEmpty("geography_id")) {
                //    var dtGeo = args.ReadData("select geography_id from geography where geography_id = :geoid", ":geoid", fh.GetValue("geography_id", -1, true), DbType.Int32);
                //    if (dtGeo.Rows.Count == 0 || DataTriggerHelper.IsValueEmpty(dtGeo.Rows[0], args.Table.AliasName, "geography_id")) {
                //        args.Cancel("Invalid value for geography.");
                //        return;
                //    }
                //}

                // TODO: validate codes: requestor category (CAT), ARS region, discipline

                // TODO: if have a category (cat) - Must supply a valid country

                // upcase ORGID
                fh.SetValueIfFieldExistsAndIsEmpty("organization_abbrev", fh.GetValue("organization_abbrev", "", true).ToString().ToUpper());

                // initcap city
                fh.SetValueIfFieldExistsAndIsEmpty("city", fh.ToTitleCase("city").ToString().ToUpper());

                // upper zip
                fh.SetValueIfFieldExistsAndIsEmpty("postal_index", fh.GetValue("postal_index", "", true).ToString().ToUpper());

                // lower email
                fh.SetValueIfFieldExistsAndIsEmpty("email", fh.GetValue("email", "", true).ToString().ToLower());

                // validate address (if address line 2 but no address line 1, etc, format it all properly...put line 2 into line 1, etc)
                // i.e. throw out missing address lines, move subsequent ones up as far as possible

                // pull 2->1 if we need to
                if (fh.AllFieldsExist("address_line1", "address_line2") && fh.IsValueEmpty("address_line1")) {
                    fh.SetValue("address_line1", fh.GetValue("address_line2", DBNull.Value, true), typeof(string), false);
                    fh.SetValue("address_line2", DBNull.Value, typeof(string), false);
                }

                // pull 3->2 is we need to
                if (fh.AllFieldsExist("address_line2", "address_line3") && fh.IsValueEmpty("address_line2")) {
                    fh.SetValue("address_line2", fh.GetValue("address_line3", DBNull.Value, true), typeof(string), false);
                    fh.SetValue("address_line3", DBNull.Value, typeof(string), false);
                }

                // again, pull 2->1 if we need to (after doing moves above)
                if (fh.AllFieldsExist("address_line1", "address_line2") && fh.IsValueEmpty("address_line1")) {
                    fh.SetValue("address_line1", fh.GetValue("address_line2", DBNull.Value, true), typeof(string), false);
                    fh.SetValue("address_line2", DBNull.Value, typeof(string), false);
                }

                if (fh.AllFieldsExist("sys_lang_id") && fh.IsValueEmpty("sys_lang_id"))
                {
                    fh.SetValue("sys_lang_id", 1, typeof(int), false);
                }

                //// Do a duplicate check ... coop unique index will take care of this but we want to warn them with a better message and a chance to recover (forms)
                //var dtCoop = args.ReadData("select cooperator_id from cooperator where coalesce(full_name,'') = coalesce(:fn, '')", ":fn", fh.GetValue("full_name"));
                //if (args.SaveMode == SaveMode.Insert && dtCoop.Rows.Count > 0) {
                //    args.Cancel("A cooperator with the same full name (" + fh.GetValue("full_name") + ") already exists.");
                //    return;
                //} else if (args.SaveMode == SaveMode.Update && dtCoop.Rows.Count > 0 && ((int)dtCoop.Rows[0]["cooperator_id"])!= (int)fh.GetValue("cooperator_id")) {
                //    args.Cancel("A different cooperator with the same full name (" + fh.GetValue("full_name") + ") already exists.");
                //    return;
                //}

                // TODO: other specifics...not relevant...if city = Beltsville, Hyattsville, DC... set arsregion to BA
                // TODO: if no country provided - make it United States
            }
        }

        public void TableRowSaved(ISaveDataTriggerArgs args) {
            // post-row save trigger

            if (args.SaveMode == SaveMode.Insert) {
                if (args.Helper.IsValueEmpty("current_cooperator_id")) {
                    // make current_cooperator_id field match the cooperator_id field
                    args.WriteData(@"
update 
    cooperator 
set 
    current_cooperator_id = :coopid1 
where 
    cooperator_id = :coopid2
", ":coopid1", args.NewPrimaryKeyID, ":coopid2", args.NewPrimaryKeyID);
                }

            }



        }

        public void TableRowSaveFailed(ISaveDataTriggerArgs args) {
        }


        public void TableSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount) {
            // post-table save trigger
        }

        #endregion

        #region IAsyncFilter Members

        public bool IsAsynchronous {
            get { return false; }
        }

        public object Clone() {
            return new CooperatorDataTrigger();
        }

        #endregion


        /* email comments...
         * 
         * 
         *  
INSERT
set the current site, userid, created date (at time of creating record for insert...not commit...rest is commit)
make sure either last name or organization has been provided
if you have a first name, you have to have a last
set the new cooperator_id (cno)
initcap the first and last name unless already mixed case
set the initials based on first name (Scott Tiger Smith...initials are first+middle or S.T.)
build the coop field
On new row...set "active" flag to X
validate geography_id...make sure we have a good geo (state/country)
validate codes: requestor category (CAT), ARS region, discipline
if have a category (cat) - Must supply a valid country
upcase ORGID
initcap city
upper zip
lower email
validate address (if address line 2 but no address line 1, etc, format it all properly...put line 2 into line 1, etc)
Do a duplicate check ... coop unique index will take care of this but we want to warn them with a better message and a chance to recover (forms)
other specifics...not relevant...if city = Beltsville, Hyattsville, DC... set arsregion to BA
                                            if no country provided - make it United States
 
-----------------------------------------------------
HISTORY (not much happening here)
 
Easy way is to make sure cno and validcno are different.  If they are, the cooperator is historical.  Keep following validcno until cno=validcno...that is the "ACTIVE" cooperator (most current)
 
This has a "hand-held" way that performs several functions...forms only.  Get a query and start marking the ones you are interested in.  Set what you want to be your master cooperator and then indicate what you want to do with the other "marked" cooperators (note...this utility exists to perform many or multiple updates at once...usually cooperator cleanup type stuff when coop gets real messy...not used often...typical is above...just put a different cno into validcno):
 
security is enforced...you have to own the coops you are trying to change and if changing the cno in other tables, you have to own those rows also (see re-link below)
make it historical
re-link it (move through entire database and change the marked cno to the master cno...when complete...delete the marked cno).
delete it
there is also cooperator members and cooperator groups...but there are not many rules in those...straight forward stuff.
 
Let me know if you need more or any other clarification.
 
Gorm
         * 
         * 
         * */


        #region IDataTriggerDescription Members

        public string GetDescription(string ietfLanguageTag) {
            return "Performs various checks on data to ensure appropriate values were given.  Also updates current_cooperator_id to point at new record at initial insert.";
        }

        public string GetTitle(string ietfLanguageTag) {
            return "Cooperator Data Trigger";
        }

        #endregion

        #region IDataResource Members

        public string[] ResourceNames {
            get { return new string[] { "cooperator" }; }
        }

        #endregion
    }
}
