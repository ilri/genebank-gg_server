using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace GrinGlobal.Web
{
    public class Utils
    {
        [DebuggerStepThrough()]
        public static SecureData GetSecureData(bool useAnonymousIfEmptyLoginToken)
        {
            return new SecureData(false, UserManager.GetLoginToken(useAnonymousIfEmptyLoginToken));
        }

        public static DataTable GetCodeValue(string groupName, string firstText)
        {
            using (SecureData sd = GetSecureData(true))
            {
                return sd.GetData("web_lookup_code_value", ":first=" + firstText + ";:groupname=" + groupName + ";:languageid=" + sd.LanguageID, 0, 0).Tables["web_lookup_code_value"];

                //                using (DataManager dm = sd.BeginProcessing(true))
                //                {
                //                    return dm.Read(@"
                //                        select '' as Value, :firstT as Text 
                //                        union select cv.value as Value,  coalesce(cvl.title, cv.value) as Text from code_value cv 
                //                        left join code_value_lang cvl on cv.code_value_id = cvl.code_value_id 
                //                        where cv.group_name = :groupname
                //                        and cvl.sys_lang_id = :languageid
                //                        order by value
                //                        ", new DataParameters(":groupname", groupName, DbType.String, ":firstT", firstText, DbType.String, ":languageid", sd.LanguageID, DbType.Int32));
                //                }

            }
        }

        public static int GetGeographyID(string countryCode)
        {
            using (SecureData sd = GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    return Toolkit.ToInt32(dm.ReadValue(@"select geography_id from geography where country_code = :country_code order by geography_id
                                ", new DataParameters(":country_code", countryCode, DbType.String)), -1);
                }
            }
        }

        public static DataTable GetCountryList()
        {
            using (SecureData sd = GetSecureData(true))
            {
                return sd.GetData("web_lookup_country_list", ":languageid=" + (sd.LanguageID < 0 ? 1 : sd.LanguageID), 0, 0).Tables["web_lookup_country_list"];

                //                using (DataManager dm = sd.BeginProcessing(true))
                //                {
                //                    return dm.Read(@"
                //                        select distinct g.country_code as countryCode, cvl.title as countryName from geography g join code_value cv 
                //                        on g.country_code = cv.value join code_value_lang cvl on cv.code_value_id = cvl.code_value_id 
                //                        where cv.group_name = 'geography_country_code'
                //                        and cvl.sys_lang_id = :languageid
                //                        order by countryname
                //                        ", new DataParameters(":languageid", sd.LanguageID < 0 ? 1 : sd.LanguageID
                //                        ));
                //                }
            }
        }

        public static DataTable GetStateList(string countryCode)
        {
            using (SecureData sd = GetSecureData(true))
            {
                return sd.GetData("web_lookup_state_list", ":countrycode=" + countryCode, 0, 0).Tables["web_lookup_state_list"];

                //                using (DataManager dm = sd.BeginProcessing(true))
                //                {
                //                    return dm.Read(@"
                //                    select 0 as gid, '' as statename union select distinct geography_id as gid, adm1 as stateName from geography 
                //                    where country_code = :countryCode and adm1 is not null
                //                    order by stateName",
                //                    new DataParameters(":countryCode", countryCode
                //                    ));
                //                }
            }
        }

        public static string GetWebAppResource(string appName, string formName)
        {
            using (SecureData sd = GetSecureData(true))
            {
                string key = "WebPvAppResource" + sd.LanguageID;
                var cm = CacheManager.Get(key);
                if (cm.Keys.Count == 0)
                {
                    var dt = sd.GetData("web_lookup_app_resource", ":appname=" + appName + ";:formname=" + formName + ";:langid=" + sd.LanguageID, 0, 0).Tables["web_lookup_app_resource"];

                    StringBuilder sb = new StringBuilder();
                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            sb.Append(";").Append(dr["app_resource_name"].ToString()).Append("=").Append(dr["display_member"].ToString());
                        }
                    }

                    string sText = sb.ToString();
                    if (sText.Length > 0) sText = sText.Substring(1);
                    cm[key] = sText;
                    return sText;
                }
                else
                    return cm[key].ToString();
            }
        }

        public static int GetLanguageID()
        {
            using (SecureData sd = GetSecureData(true))
            {
                return (sd.LanguageID < 0 ? 1 : sd.LanguageID);
            }
        }

        public static void ExportToExcel(HttpContext ctx, GridView gv, string fName, string title)
        {
            ctx.Response.Clear();
            ctx.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.csv", fName));
            ctx.Response.Charset = "";
            ctx.Response.ContentType = "application/vnd.csv";

            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sWriter = new System.IO.StringWriter(sb);

            if (!string.IsNullOrEmpty(title))
            {
                sWriter.WriteLine(title);
                sWriter.WriteLine(",");
            }

            //for (int k = 0; k < gv.Columns.Count; k++)   // Gridview could be AutoGenerateColumns = True
            for (int k = 0; k < gv.HeaderRow.Cells.Count; k++)
            {
                sWriter.Write(gv.HeaderRow.Cells[k].Text + ",");
                //sWriter.Write(gv.Columns[k].HeaderText + ",");
            }
            sWriter.WriteLine(",");

            string sData;
            for (int i = 0; i < gv.Rows.Count; i++)
            {
                for (int j = 0; j < gv.HeaderRow.Cells.Count; j++)
                {
                    sData = (gv.Rows[i].Cells[j].Text.ToString());
                    if (sData == "&nbsp;") sData = "";
                    sData = "\"" + sData + "\"" + ",";
                    sWriter.Write(sData);
                }
                sWriter.WriteLine();
            }

            sWriter.Close();
            ctx.Response.Write(sb.ToString());
            ctx.Response.End();
        }

        public static string DisplayDate(string date, string dc)
        {
            if (!String.IsNullOrEmpty(date))
            {
                DateTime dt = Toolkit.ToDateTime(date);
                string fmt = "";
                string prefix = "";

                if (!String.IsNullOrEmpty(dc))
                {
                    if (dc.IndexOf(' ') > 0)
                    {
                        prefix = dc.Split(' ')[0];
                        fmt = dc.Split(' ')[1];
                    }
                    else
                        fmt = dc;
                }
                else
                    fmt = "dd-MMM-yyyy";

                switch (fmt)
                {
                    case "MM/dd/yyyy":
                    case "dd/MM/yyyy":
                        fmt = "dd-MMM-yyyy";
                        break;
                    case "MM/yyyy":
                        fmt = "MMM-yyyy";
                        break;
                    default:
                        break;
                }

                string value = (prefix == "" ? "" : prefix + " ") + dt.ToString(fmt);
                return value == "" ? "" : value + ".";
            }
            else
                return "";
        }

        public static string Sanitize(string inputString)
        {
            string outputString;
            if (inputString == "" || inputString == null)
            {
                outputString = "";
            }
            else
            {
                outputString = Regex.Replace(inputString, "update ", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "delete ", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "insert ", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "select into", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "create ", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "exec ", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "exec\\(", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "execute ", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "declare ", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "<", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, ">", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "@", "", RegexOptions.IgnoreCase);

            }
            return outputString;
        }

        public static string Sanitize2(string inputString)
        {
            string outputString;
            if (inputString == "" || inputString == null)
            {
                outputString = "";
            }
            else
            {
                outputString = Regex.Replace(inputString, "update ", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "delete ", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "insert ", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "select into", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "create ", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "exec ", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "exec\\(", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "execute ", "", RegexOptions.IgnoreCase);
                outputString = Regex.Replace(outputString, "declare ", "", RegexOptions.IgnoreCase);
                //outputString = Regex.Replace(outputString, "<", "", RegexOptions.IgnoreCase);
                //outputString = Regex.Replace(outputString, ">", "", RegexOptions.IgnoreCase);

            }
            return outputString;
        }

    }
}
