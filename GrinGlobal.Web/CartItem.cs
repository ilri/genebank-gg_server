using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;
using System.Diagnostics;

namespace GrinGlobal.Web {
    public class CartItem {

        public CartItem() {
            _typeName = null;
            _itemID = 0;
            _quantity = 0;
            _distributionType = null;
            HasChanged = false;
        }

        private string _distributionType;
        public string DistributionFormCode {
            get { return _distributionType; }
            set {
                if (_distributionType != value) {
                    _distributionType = value;
                    HasChanged = true;
                }
            }
        }

        private string _typeName;
        public string TypeName {
            get {
                return _typeName;
            }
            set {
                if (_typeName != value) {
                    _typeName = value;
                    HasChanged = true;
                }
            }
        }

        private int _itemID;
        public int ItemID {
            get {
                return _itemID;
            }
            set {
                if (_itemID != value) {
                    _itemID = value;
                    HasChanged = true;
                }
            }
        }

        private int _quantity;
        public int Quantity {
            get {
                return _quantity;
            }
            set {
                if (_quantity != value) {
                    _quantity = value;
                    HasChanged = true;
                }
            }
        }

        public bool HasChanged {
            get;
            private set;
        }


        public void Save(int cartID, SecureData sd, DataManager dm)
        {
            // try to update cart item table by type_name and item_name.
            // if no records are affected, we need to insert a record.

            //could have problem if cart has two form codes rows per accession
            int wuserid = sd.WebUserID;
            int affected = dm.Write(@"
update web_user_cart_item
set
    quantity = :qty,
    form_type_code = :formcode,
    modified_date = :now,
    modified_by   = :modifiedby
where
    web_user_cart_id = :cartid
    and accession_id = :id
", new DataParameters(
":qty", this.Quantity,
":formcode", this.DistributionFormCode, DbType.String,
":now", DateTime.UtcNow, DbType.DateTime2,
":modifiedby", wuserid, DbType.Int32,
":cartid", cartID, DbType.Int32,
":id", this.ItemID, DbType.Int32));

            if (affected == 0)
            {
                dm.Write(@"
insert into web_user_cart_item
(web_user_cart_id, accession_id, quantity, form_type_code, created_date, created_by, owned_date, owned_by)
values
(:cartid, :value, :quantity, :formcode, :now, :wuserid, :now2, :wuserid2)
", new DataParameters(
":cartid", cartID, DbType.Int32,
":value", this.ItemID, DbType.Int32,
":quantity", this.Quantity,
":formcode", this.DistributionFormCode, DbType.String,
":now", DateTime.UtcNow, DbType.DateTime2,
":wuserid", wuserid, DbType.Int32,
":now2", DateTime.UtcNow, DbType.DateTime2,
":wuserid2", wuserid, DbType.Int32));
            }

            // don't forget to mark our object as unchanged now that we persisted it
            HasChanged = false;

        }

        public static string GetMaterialDescription(string formcode)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
//                using (DataManager dm = sd.BeginProcessing(true))
//                {
//                    string text = (dm.ReadValue(@"
//                            select
//	                            coalesce(cvl.description, cv.value) as description
//                            from
//	                            code_value cv
//	                        left join code_value_lang cvl
//		                        on cv.code_value_id = cvl.code_value_id
//		                        and cvl.sys_lang_id = :langid
//                            where
//	                            cv.group_name = 'GERMPLASM_FORM'
//	                            and cv.value = :formcode
//                             ", new DataParameters(":langid", sd.LanguageID, DbType.Int32
//                                 , ":formcode", formcode, DbType.String))).ToString();
//                    return text;
//                }
            
            var dt = sd.GetData("web_lookup_material_description", ":langid=" + sd.LanguageID + ";:formcode=" + formcode, 0, 0).Tables["web_lookup_material_description"];
            return dt.ToScalarValue();
            }
        }
    }
}
