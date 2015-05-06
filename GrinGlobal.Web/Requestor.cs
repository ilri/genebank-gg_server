using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using GrinGlobal.Core;
using GrinGlobal.Business;


namespace GrinGlobal.Web
{
    public class Requestor
    {
        /// <summary>
        /// Gets the requestor for the current user, loading from session (for guest useror database as needed.
        /// </summary>

        int _webcoopid;
        string _title;
        string _firstname;
        string _lastname;
        string _organization;
        string _addr1;
        string _addr2;
        string _addr3;
        string _city;
        string _postalIndex;
        int _geographyid;
        string _phone;
        string _altphone;
        string _fax;
        string _email;
        string _note;
        int _webuserid;
        string _state;
        string _country;

        string _addr1_s;
        string _addr2_s;
        string _addr3_s;
        string _city_s;
        string _postalIndex_s;
        int _geographyid_s;
        string _state_s;
        string _country_s;

        string _carrier;
        string _carrierAcct;

        bool _emailorder = true;
        bool _emailshipping = true;
        bool _emailnews;

        public static Requestor Current
        {
            get
            {
                Requestor requestor = null;

                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    requestor = HttpContext.Current.Session["requestor"] as Requestor;
                    if (requestor != null)
                    {
                        return requestor;
                    }
                    else
                    {
                        requestor = new Requestor();
                        HttpContext.Current.Session["requestor"] = requestor;
                    }
                }

                return requestor;
            }
        }

        public Requestor(int webcoopID, string title, string firstname, string lastname, string organization, string addr1, string addr2, string addr3, string city, string postalIndex, int geographyid, string phone, string altphone, string fax, string email, string note, string state, string country,
                        string addr1_s, string addr2_s, string addr3_s, string city_s, string postalIndex_s, int geographyid_s,  string state_s, string country_s, string carrier, string carrierAcct, bool emailorder, bool emailshiping, bool emailnews)
        {
            _webcoopid = webcoopID;
            _title = title;
            _firstname = firstname;
            _lastname = lastname;
            _organization = organization;
            _addr1 = addr1;
            _addr2 = addr2;
            _addr3 = addr3;
            _city = city;
            _postalIndex = postalIndex;
            _geographyid = geographyid;
            _phone = phone;
            _altphone = altphone;
            _fax = fax;
            _email = email;
            _note = note;
            _state = state;
            _country = country;

            _addr1_s = addr1_s;
            _addr2_s = addr2_s;
            _addr3_s = addr3_s;
            _city_s = city_s;
            _postalIndex_s = postalIndex_s;
            _geographyid_s = geographyid_s;
            _state_s = state_s;
            _country_s = country_s;

            _carrier = carrier;
            _carrierAcct = carrierAcct;

            _emailorder = emailorder;
            _emailshipping = emailshiping;
            _emailnews = emailnews;
        }

        private Requestor() {
            if (!UserManager.IsAnonymousUser(UserManager.GetUserName())) {
                using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
                {
                    using (DataManager dm = sd.BeginProcessing(true, true))
                    {
                        DataTable dt = dm.Read(@"
                        select wc.*, g.adm1 as state, cvl.title as country 
                        from 
                            web_user wu join web_cooperator wc
                                on wu.web_cooperator_id = wc.web_cooperator_id
                            left join geography g
                                on wc.geography_id = g.geography_id
                            join code_value cv 
                                on g.country_code = cv.value 
                            join code_value_lang cvl 
                                on cv.code_value_id = cvl.code_value_id
                        where 
                            wu.web_user_id = :webuserid and cvl.sys_lang_id = :langid
                        ", new DataParameters(
                             ":webuserid", sd.WebUserID, DbType.Int32, ":langid", sd.LanguageID, DbType.Int32 
                         ));

                        foreach (DataRow dr in dt.Rows)
                        {
                            _webcoopid = int.Parse(dr["web_cooperator_id"].ToString());
                            _title = dr["title"].ToString();
                            _firstname = dr["first_name"].ToString();
                            _lastname = dr["last_name"].ToString();
                            _organization = dr["organization"].ToString();
                            _addr1 = dr["address_line1"].ToString();
                            _addr2 = dr["address_line2"].ToString();
                            _addr3 = dr["address_line3"].ToString();
                            _city = dr["city"].ToString();
                            _postalIndex = dr["postal_index"].ToString();
                            _geographyid = Toolkit.ToInt32(dr["geography_id"].ToString(), 0);
                            _phone = dr["primary_phone"].ToString();
                            _altphone = dr["secondary_phone"].ToString();
                            _fax = dr["fax"].ToString();
                            _email = dr["email"].ToString();
                            _note = dr["note"].ToString();
                            _state = dr["state"].ToString();
                            _country = dr["country"].ToString();
                        }
                        _webuserid = sd.WebUserID;

                        DataTable dt2 = UserManager.GetDefaultShippingAddress();
                        foreach (DataRow dr in dt2.Rows)
                        {
                            _addr1_s = dr["address_line1"].ToString();
                            _addr2_s = dr["address_line2"].ToString();
                            _addr3_s = dr["address_line3"].ToString();
                            _city_s = dr["city"].ToString();
                            _state_s = dr["state_name"].ToString();
                            _postalIndex_s = dr["postal_index"].ToString();
                            _country_s = dr["country_name"].ToString();
                            _geographyid_s = Toolkit.ToInt32(dr["geography_id"].ToString(), 0);
                        }

                        _carrier = "";
                        _carrierAcct = "";
                    }
                }
            }
        }
        public int WebCoopid
        {
            get { return _webcoopid; }
            set { _webcoopid = value; }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public string Firstname
        {
            get {return _firstname;}
            set {_firstname = value;}
        }
        public string Lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }
        public string Organization
        {
            get { return _organization; }
            set { _organization = value; }
        }
        public string Addr1
        {
            get { return _addr1; }
            set { _addr1 = value; }
        }
        public string Addr2
        {
            get { return _addr2; }
            set { _addr2 = value; }
        }
        public string Addr3
        {
            get { return _addr3; }
            set { _addr3 = value; }
        }
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }
        public string PostalIndex
        {
            get { return _postalIndex; }
            set { _postalIndex = value; }
        }
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        public string AltPhone
        {
            get { return _altphone; }
            set { _altphone = value; }
        }
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }
        public int Webuserid
        {
            get { return _webuserid; }
            set { _webuserid = value; }
        }
        public int Geographyid
        {
            get { return _geographyid; }
            set { _geographyid = value; }
        }
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }
        public string Addr1_s
        {
            get { return _addr1_s; }
            set { _addr1_s = value; }
        }
        public string Addr2_s
        {
            get { return _addr2_s; }
            set { _addr2_s = value; }
        }
        public string Addr3_s
        {
            get { return _addr3_s; }
            set { _addr3_s = value; }
        }
        public string City_s
        {
            get { return _city_s; }
            set { _city_s = value; }
        }
        public string PostalIndex_s
        {
            get { return _postalIndex_s; }
            set { _postalIndex_s = value; }
        }
        public int Geographyid_s
        {
            get { return _geographyid_s; }
            set { _geographyid_s = value; }
        }
        public string State_s
        {
            get { return _state_s; }
            set { _state_s = value; }
        }
        public string Country_s
        {
            get { return _country_s; }
            set { _country_s = value; }
        }
        public string Carrier
        {
            get { return _carrier; }
            set { _carrier = value; }
        }
        public string CarrierAcct
        {
            get { return _carrierAcct; }
            set { _carrierAcct = value; }
        }
        public bool EmailOrder
        {
            get { return _emailorder; }
            set { _emailorder = value; }
        }
        public bool EmailShipping
        {
            get { return _emailshipping; }
            set { _emailshipping = value; }
        }
        public bool EmailNews
        {
            get { return _emailnews; }
            set { _emailnews = value; }
        }

    }
}
