using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

using GrinGlobal.Core;

namespace maizegdb {
	/// <summary>
	/// Summary description for maizegdb
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class maizegdb : System.Web.Services.WebService {

		//[WebMethod]
		//public DataTable TestDataTable(string acp) {
		//    using (DataManager dm = DataManager.Create()) {
		//        return dm.Read("select * from ACC where ACP = @ACP", new DataParameters("@ACP", acp));
		//    }
		//}

		//[WebMethod]
		//public DataSet TestDataSet(string acp) {
		//    using (DataManager dm = DataManager.Create()) {
		//        return dm.Read("select * from ACC where ACP = @ACP", null, "Table0", new DataParameters("@ACP", acp));
		//    }
		//}

		[WebMethod]
		public DataTable ETLNonUniqueAcids() {
			using (DataManager dm = DataManager.Create()) {
				return dm.Read(@"
SELECT 
	A.ACID,
	COUNT(A.ACID) AS ACID_COUNT
FROM 
	ACC A INNER JOIN IV I 
		ON A.ACID = I.ACID 
WHERE 
	I.IMNAME LIKE 'NC7-MAIZE%'
	AND I.STATUS = 'AVAIL'
	AND I.DISTRIBUTE = 'X'
GROUP BY 
	A.ACID
HAVING 
	COUNT(A.ACID) > 1", "ETL_NON_UNIQUE_ACIDS");
			}
		}

		[WebMethod]
		public DataSet ETLQuery() {

			/*
			 * stock_grin
Name            Null?    Type
--------------- -------- -------------
STOCK_ID                 NUMBER(10)
AUTO_NUM        NOT NULL NUMBER(10)
AC_ID           NOT NULL NUMBER(7)
AC_NO           NOT NULL NUMBER(6)
AC_P            NOT NULL VARCHAR2(4)
GENUS           NOT NULL VARCHAR2(30)
PLANT_ID        NOT NULL VARCHAR2(60)
SEARCH_ID       NOT NULL VARCHAR2(40)
SITE            NOT NULL VARCHAR2(4)
ACS                      VARCHAR2(4)
AC_IMPT                  VARCHAR2(10
AG_NAME                  VARCHAR2(20
COUNTRY                  VARCHAR2(30)
STATE                    VARCHAR2(20)
TOP_NAME                 VARCHAR2(1)
UNIFORM                  VARCHAR2(10)
                                   
                                   
stock_grin_available               
Name            Null?    Type      
--------------- -------- -------------
AC_ID           NOT NULL NUMBER(8) 
ACP             NOT NULL VARCHAR2(6)
AC_NO           NOT NULL NUMBER(8)
ACS                      VARCHAR2(4)
D_QUANT                  NUMBER(6)
			 */

			using (DataManager dm = DataManager.Create()) {
				DataSet ds = dm.Read(@"
SELECT 
	A.ACID,
	A.ACP,
	A.ACNO,
	A.ACS,
	'AVAIL' AS STATUS,
	MAX(I.DUNITS) AS DUNITS_MAX,
	MAX(I.DQUANT) AS DQUANT_MAX
FROM 
	PROD.ACC A INNER JOIN PROD.IV I
		ON A.ACID = I.ACID
WHERE
	I.IMNAME LIKE 'NC7-maize%'
	AND I.STATUS = 'AVAIL'
	AND I.DISTRIBUTE = 'X'
	AND A.ACP != 'NSL'
GROUP BY
	A.ACID,
	A.ACP,
	A.ACNO,
	A.ACS
", null, "STOCK_GRIN_AVAILABLE");


				return ds;

				// TODO: add other table data as a second query here
				//return dm.Read(@"<SQL HERE>", ds, "stock_grin");

			}
		}
	}
}