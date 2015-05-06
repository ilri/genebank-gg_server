using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GrinGlobal.Core;
using System.Data;
using GrinGlobal.Business;
using System.Diagnostics;

namespace GrinGlobal.Web {
    public class Cart {

        /// <summary>
        /// Gets the cart for the current user, loading from session or database as needed.  If one does not exist, a new, empty one will be created.
        /// </summary>
        public static Cart Current {
            get {

                Cart cart = null;

                // first, try to pull cart from session
                if (HttpContext.Current != null && HttpContext.Current.Session != null) {
                    cart = HttpContext.Current.Session["cart"] as Cart;
                    if (cart != null) {
                        return cart;
                    } else {
                        // we get here, it's not in session.
                        // we need to create a new one and fill it with data
                        // from the database (sys_user_cart / sys_user_cart_item tables)
                        // if we can
                        cart = new Cart();
                        HttpContext.Current.Session["cart"] = cart;
                    }
                }

                return cart;

            }
        }

        private Cart() {
            _accessions = new List<CartItem>();

            // default user data is never saved to the database, so never try to fill it from there
            if (!UserManager.IsAnonymousUser(UserManager.GetUserName())) {
                Fill();
            }
        }

        List<CartItem> _accessions;

        /// <summary>
        /// Gets the array of accessions the current user has in their cart.  Alter this array by calling AddAccession and RemoveAccession.
        /// </summary>
        public CartItem[] Accessions {
            get {
                return _accessions.ToArray();
            }

        }

        /// <summary>
        /// Empties all items from the cart and saves.
        /// </summary>
        public void Empty() {
            _accessions.Clear();
            _changed = true;
            Save();
        }

        /// <summary>
        /// Pulls accession id from given DataRow and fieldName and returns the associated CartItem if found.  Returns null otherwise.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public CartItem FindByAccessionID(DataRow dr, string fieldName) {
            int id = Toolkit.ToInt32(dr[fieldName], -1);
            if (id < 0) {
                return null;
            } else {
                return FindByAccessionID(id);
            }
        }

        /// <summary>
        /// Returns the CartItem for the given accessionID.  Returns null if not found.
        /// </summary>
        /// <param name="accessionID"></param>
        /// <returns></returns>
        public CartItem FindByAccessionID(int accessionID) {
            foreach (CartItem ci in _accessions) {
                if (ci.ItemID == accessionID) {
                    return ci;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all ItemID values contained within Accessions array
        /// </summary>
        public List<int> AccessionIDs {
            get {
                List<int> ret = new List<int>();
                foreach (CartItem ci in _accessions) {
                    ret.Add(ci.ItemID);
                }
                return ret;
            }
        }

        /// <summary>
        /// Gets the expiration date for the cart.  Can not set directly, must call Save(int) overload to set a new ExpirationDate.
        /// </summary>
        public DateTime ExpirationDate {
            get;
            private set;
        }

        private bool _changed;
        /// <summary>
        /// Returns true if the Cart or any of the items in the Cart have changed since the last call to Fill() or Save().  Returns false otherwise.
        /// </summary>
        public bool HasChanged {
            get {
                if (_changed) {
                    return true;
                } else {
                    // we haven't changed, but maybe one of our sub items has...
                    foreach (CartItem item in _accessions) {
                        if (item.HasChanged) {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Adds given ids to the Accessions array (or updates quantity as appropriate).  Returns # of new items added (as opposed to current items' quantities being updated)
        /// </summary>
        /// <param name="id"></param>
        public int AddAccession(int id, string distributionFormCode) {
            if (String.IsNullOrEmpty(distributionFormCode)) {
                var dt = ListDistributionTypes(id);
                if (dt.Rows.Count > 0) {
                    distributionFormCode = dt.Rows[0]["value"].ToString();
                }
                else {
                    return 0; // Only add avaialable accession to cart
                }
            }
            CartItem item = new CartItem { TypeName = "accession", ItemID = id, Quantity = 0, DistributionFormCode = distributionFormCode };
            foreach (CartItem it in _accessions) {
                // Only handle one material type per accession one cart
                //if (it.ItemID == id && distributionFormCode == it.DistributionFormCode) {
                if (it.ItemID == id) {
                    item = it;
                }
            }
            item.Quantity++;
            _changed = true;
            if (item.Quantity == 1) {
                _accessions.Add(item);
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Adds given ids to the Accessions array (or updates quantity as appropriate).   Returns # of new items added (as opposed to current items' quantities being updated)
        /// </summary>
        /// <param name="ids"></param>
        public int AddAccessions(List<int> ids) {
            int added = 0;
            foreach (int id in ids) {
                added += AddAccession(id, null);
            }
            return added;
        }

        /// <summary>
        /// Removes given id from Accessions array.  Optionally doesn't remove the item, just decrements the quantity for it.  If quantity reaches 0, accession is still removed.  Returns # of items removed (as opposed to current items' quantities being updated)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="justDecrementQuantityByOne"></param>
        public int RemoveAccession(int id, bool justDecrementQuantityByOne) {
            for(int i=0;i<_accessions.Count;i++){
                CartItem it = _accessions[i];
                if (it.ItemID == id) {
                    _changed = true;
                    if (justDecrementQuantityByOne) {
                        it.Quantity--;
                        if (it.Quantity == 0) {
                            _accessions.RemoveAt(i);
                            return 1;
                        }
                    } else {
                        _accessions.RemoveAt(i);
                        return 1;
                    }
                    return 0;
                }
            }
            return 0;
        }

        /// <summary>
        /// Removes given list of ids from Accession array.  Optionally doesn't remove the item, just decrements the quantity for it.  If quantity reaches 0, accession is still removed.  Returns # of items removed (as opposed to current items' quantities being updated)
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="justDecrementQuantityByOne"></param>
        public int RemoveAccessions(List<int> ids, bool justDecrementQuantityByOne) {
            int removedItems = 0;
            foreach (int id in ids) {
                removedItems += RemoveAccession(id, justDecrementQuantityByOne);
            }
            return removedItems;
        }

        public DataTable ListDistributionTypes(int accessionID) {
            using (SecureData sd = UserManager.GetSecureData(true)) {
                return sd.GetData("web_lookup_distribution_type", ":accessionid=" + accessionID + ";:langid=" + sd.LanguageID, 0, 0).Tables["web_lookup_distribution_type"];

//                using (var dm = sd.BeginProcessing(true)) {
//                    var dt = dm.Read(@"
//select
//    distinct
//	cv.value as value,
//	coalesce(cvl.description, cv.value) as display_text
//from
//	inventory i
//	left join code_value cv
//		on cv.group_name = 'GERMPLASM_FORM'
//		and cv.value = i.form_type_code
//	left join code_value_lang cvl
//		on cv.code_value_id = cvl.code_value_id
//		and cvl.sys_lang_id = :langid
//where
//	i.accession_id = :accessionid
//	and i.is_distributable = 'Y'
//	and i.is_available = 'Y'
//", new DataParameters(":accessionid", accessionID, DbType.Int32, ":langid", sd.LanguageID, DbType.Int32));
    
//                    return dt;

            }
        }

        /// <summary>
        /// Populates the Cart object with values from the database.
        /// </summary>
        public void Fill() {

            _accessions.Clear();

            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken())) {
                using (DataManager dm = sd.BeginProcessing(true)) {

                    // first, delete all expired carts and their items (regardless of user!)...
                    DateTime now = DateTime.UtcNow;
                    int wuserid = sd.WebUserID;

                    dm.Write(@"
delete from 
    web_user_cart_item
where
    web_user_cart_id in
    (select web_user_cart_id from web_user_cart where cart_type_code = 'order items' and expiration_date < :now)
", new DataParameters(":now", now, DbType.DateTime2));

                    dm.Write(@"
delete from 
    web_user_cart 
where 
    cart_type_code = 'order items' and expiration_date < :now
", new DataParameters(":now", now, DbType.DateTime2));

                    // now get the expiration date for the current user's cart
                    this.ExpirationDate = Toolkit.ToDateTime(dm.ReadValue(@"
select
    expiration_date
from
    web_user_cart
where
    cart_type_code = 'order items' and web_user_id = :wuserid
", new DataParameters(":wuserid", wuserid, DbType.Int32)), DateTime.UtcNow.AddDays(Toolkit.GetSetting("CartExpirationDays", 14)));

                    // get the items for the cart
                    DataTable dt = dm.Read(@"
select 
    wuci.accession_id,
    wuci.quantity,
    wuci.form_type_code as form_code
from 
    web_user_cart wuc inner join web_user_cart_item wuci
        on wuc.web_user_cart_id = wuci.web_user_cart_id
where 
    wuc.cart_type_code = 'order items'       
    and wuc.web_user_id = :wuserid
    and wuc.expiration_date > :now
", new DataParameters(
     ":wuserid", wuserid, DbType.Int32,
     ":now", DateTime.UtcNow, DbType.DateTime2
     ));

                    foreach (DataRow dr in dt.Rows) {
                        int val = Toolkit.ToInt32(dr["accession_id"], 0);
                        if (val > 0) {
                            int qty = Toolkit.ToInt32(dr["quantity"], 0);
                            _accessions.Add(new CartItem { TypeName = "accession", ItemID = val, Quantity = qty, DistributionFormCode = dr["form_code"].ToString() });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves cart contents to the database.  Marks expiration date as the value of the CartExpirationDays appSetting in web.config.  If that does not exist, default is 14 days.
        /// </summary>
        public void Save() {
            Save(Toolkit.GetSetting("CartExpirationDays", 14));
        }

        /// <summary>
        /// Saves cart contents to the database.  Marks expiration date as being in the future by # days = daysUntilExpiration
        /// </summary>
        /// <param name="daysUntilExpiration"></param>
        public void Save(int daysUntilExpiration) {

            DateTime expirationDate = DateTime.UtcNow.AddDays(daysUntilExpiration);

            this.ExpirationDate = expirationDate;

            if (UserManager.IsAnonymousUser(UserManager.GetUserName())) {
                // we can't save the default user to the database.
                // just make sure it's stored in session and we're good.
                // (it's already in session, but let's overwrite that reference to be safe)
                if (HttpContext.Current != null && HttpContext.Current.Session != null) {
                    HttpContext.Current.Session["cart"] = this;
                }
                _changed = false;
                return;
            }

            if (!HasChanged) {
                // nothing has changed, we don't need to write to the database.
                return;
            }

            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true))) {

                using (DataManager dm = sd.BeginProcessing(true)) {

                    // remember the time for later (when we delete records we didn't touch)
                    DateTime cutoff = DateTime.UtcNow;
                    int wuserid = sd.WebUserID;
                    string cart_type_code = "order items";  // TODO: retrive the correct code value lang when it is there

                    // write the cart record first
                    int cartID = Toolkit.ToInt32(dm.ReadValue(@"
select 
    web_user_cart_id 
from 
    web_user_cart
where
    cart_type_code = 'order items' and  
    web_user_id = :wuserid
", new DataParameters(":wuserid", wuserid, DbType.Int32)), -1);

                    if (cartID < 0) {

                        // need to insert a new record
                        cartID = dm.Write(@"
insert into web_user_cart
(web_user_id, cart_type_code, expiration_date, created_date, created_by, owned_date, owned_by)
values
(:wuserid, :carttypecode, :expireDate, :createddate, :createdby, :owneddate, :ownedby)
", true, "web_user_cart_id", new DataParameters(
     ":wuserid", wuserid, DbType.Int32,
     ":carttypecode", cart_type_code, DbType.String,
     ":expireDate", expirationDate, DbType.DateTime2,
     ":createddate", DateTime.UtcNow, DbType.DateTime2,
     ":createdby", wuserid, DbType.Int32,
     ":owneddate", DateTime.UtcNow, DbType.DateTime2,
     ":ownedby", wuserid, DbType.Int32));

                    } else {
                        // need to update the cart record (just expiration date and audit info for right now)
                        dm.Write(@"
update web_user_cart
set    
    expiration_date = :expireDate,
    modified_date = :now,
    modified_by   = :modifiedby
where
    cart_type_code = 'order items' and     
    web_user_id = :wuserid
", new DataParameters(
         ":expireDate", expirationDate, DbType.DateTime2,
         ":now", DateTime.UtcNow, DbType.DateTime2,
         ":wuserid", wuserid, DbType.Int32,
         ":modifiedby", wuserid, DbType.Int32
         ));
                        
                    }

                    // save all the accessions
                    saveItems(_accessions, cartID, sd, dm);

                    // delete any records we didn't just create or update
                    dm.Write(@"
delete from
    web_user_cart_item
where
    web_user_cart_id = :cartid
    and ((created_date < :now1 and modified_date is null)
         or (modified_date < :now2))
", new DataParameters(":cartid", cartID, DbType.Int32, ":now1", cutoff, DbType.DateTime2, ":now2", cutoff, DbType.DateTime2));


                    // ok, db contains latest info for this cart and all its items.  mark us as not changed
                    _changed = false;

                }
            }

        }

        private void saveItems(List<CartItem> items, int cartID, SecureData sd, DataManager dm) {
            // now spin through and write all the accession data
            foreach (CartItem item in items) {
                item.Save(cartID, sd, dm);
            }
        }

        /// <summary>
        /// Populates the Cart object with values from the database, using login token for query.
        /// </summary>
        public void FillDB(string loginToken)
        {
            _accessions = new List<CartItem>();

            using (SecureData sd = new SecureData(true, loginToken, null))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {

                    // first, delete all expired carts and their items (regardless of user!)...
                    DateTime now = DateTime.UtcNow;
                    int wuserid = sd.WebUserID;

                    dm.Write(@"
delete from 
    web_user_cart_item
where
    web_user_cart_id in
    (select web_user_cart_id from web_user_cart where cart_type_code = 'order items' and expiration_date < :now)
", new DataParameters(":now", now, DbType.DateTime2));

                    dm.Write(@"
delete from 
    web_user_cart 
where 
    cart_type_code = 'order items' and expiration_date < :now
", new DataParameters(":now", now, DbType.DateTime2));

                    // now get the expiration date for the current user's cart
                    this.ExpirationDate = Toolkit.ToDateTime(dm.ReadValue(@"
select
    expiration_date
from
    web_user_cart
where
    cart_type_code = 'order items' and web_user_id = :wuserid
", new DataParameters(":wuserid", wuserid, DbType.Int32)), DateTime.UtcNow.AddDays(Toolkit.GetSetting("CartExpirationDays", 14)));

                    // get the items for the cart
                    DataTable dt = dm.Read(@"
select 
    wuci.accession_id,
    wuci.quantity,
    wuci.form_type_code as form_code
from 
    web_user_cart wuc inner join web_user_cart_item wuci
        on wuc.web_user_cart_id = wuci.web_user_cart_id
where 
    wuc.cart_type_code = 'order items'       
    and wuc.web_user_id = :wuserid
    and wuc.expiration_date > :now
", new DataParameters(
     ":wuserid", wuserid, DbType.Int32,
     ":now", DateTime.UtcNow, DbType.DateTime2
     ));

                    foreach (DataRow dr in dt.Rows)
                    {
                        int val = Toolkit.ToInt32(dr["accession_id"], 0);
                        if (val > 0)
                        {
                            int qty = Toolkit.ToInt32(dr["quantity"], 0);
                            _accessions.Add(new CartItem { TypeName = "accession", ItemID = val, Quantity = qty, DistributionFormCode = dr["form_code"].ToString() });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves cart contents to the database using login token.  Marks expiration date as the value of the CartExpirationDays appSetting in web.config.  If that does not exist, default is 14 days.
        /// </summary>
        public void Save(string loginToken)
        {
            Save(loginToken, Toolkit.GetSetting("CartExpirationDays", 14));
        }

        /// <summary>
        /// Saves cart contents to the database using loginToken.  Marks expiration date as being in the future by # days = daysUntilExpiration
        /// </summary>
        /// <param name="daysUntilExpiration"></param>
        public void Save(string loginToken, int daysUntilExpiration)
        {

            DateTime expirationDate = DateTime.UtcNow.AddDays(daysUntilExpiration);

            this.ExpirationDate = expirationDate;

            if (!HasChanged)
            {
                // nothing has changed, we don't need to write to the database.
                return;
            }

            using (SecureData sd = new SecureData(true, loginToken, null))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    // remember the time for later (when we delete records we didn't touch)
                    DateTime cutoff = DateTime.UtcNow;
                    int wuserid = sd.WebUserID;
                    string cart_type_code = "order items";  // TODO: retrive the correct code value lang when it is there

                    // write the cart record first
                    int cartID = Toolkit.ToInt32(dm.ReadValue(@"
select 
    web_user_cart_id 
from 
    web_user_cart
where
    cart_type_code = 'order items' and  
    web_user_id = :wuserid
", new DataParameters(":wuserid", wuserid, DbType.Int32)), -1);

                    if (cartID < 0)
                    {
                        // need to insert a new record
                        cartID = dm.Write(@"
insert into web_user_cart
(web_user_id, cart_type_code, expiration_date, created_date, created_by, owned_date, owned_by)
values
(:wuserid, :carttypecode, :expireDate, :createddate, :createdby, :owneddate, :ownedby)
", true, "web_user_cart_id", new DataParameters(
     ":wuserid", wuserid, DbType.Int32,
     ":carttypecode", cart_type_code, DbType.String,
     ":expireDate", expirationDate, DbType.DateTime2,
     ":createddate", DateTime.UtcNow, DbType.DateTime2,
     ":createdby", wuserid, DbType.Int32,
     ":owneddate", DateTime.UtcNow, DbType.DateTime2,
     ":ownedby", wuserid, DbType.Int32));

                    }
                    else
                    {
                        // need to update the cart record (just expiration date and audit info for right now)
                        dm.Write(@"
update web_user_cart
set    
    expiration_date = :expireDate,
    modified_date = :now,
    modified_by   = :modifiedby
where
    cart_type_code = 'order items' and     
    web_user_id = :wuserid
", new DataParameters(
         ":expireDate", expirationDate, DbType.DateTime2,
         ":now", DateTime.UtcNow, DbType.DateTime2,
         ":wuserid", wuserid, DbType.Int32,
         ":modifiedby", wuserid, DbType.Int32
         ));
                    }

                    // save all the accessions
                    saveItems(_accessions, cartID, sd, dm);

                    // delete any records we didn't just create or update
                    dm.Write(@"
delete from
    web_user_cart_item
where
    web_user_cart_id = :cartid
    and ((created_date < :now1 and modified_date is null)
         or (modified_date < :now2))
", new DataParameters(":cartid", cartID, DbType.Int32, ":now1", cutoff, DbType.DateTime2, ":now2", cutoff, DbType.DateTime2));


                    // ok, db contains latest info for this cart and all its items.  mark us as not changed
                    _changed = false;

                }
            }
        }
    }
}
