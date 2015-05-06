using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GRINGlobal.Client.Common
{
    public class WebServices
    {
        private GrinGlobalGUIWebServices.GUI _GUIWebServices;
        private string _username = "";
        private string _password = "";
        private string _passwordClearText = "";
        private string _site = "";

        public string Url
        {
            get
            {
                return _GUIWebServices.Url;
            }
            set
            {
                _GUIWebServices.Url = value;
            }
        }

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public string Password_ClearText
        {
            get
            {
                return _passwordClearText;
            }
            set
            {
                _passwordClearText = value;
            }
        }

        public string Site
        {
            get
            {
                return _site;
            }
        }
        
        public WebServices(string webServiceURL, string webServiceUsername, string webServicePasswordEncrypted, string webServicePasswordClearText, string site)
        {
            _GUIWebServices = new GrinGlobalGUIWebServices.GUI();
            if(!string.IsNullOrEmpty(webServiceURL)) _GUIWebServices.Url = webServiceURL;
            _username = webServiceUsername;
            _password = webServicePasswordEncrypted;
            _passwordClearText = webServicePasswordClearText;
            _site = site;
        }

        public DataSet GetData(string dataviewName, string delimitedParameterList, int offset, int limit)
        {
            DataSet dataViewParams;
            DataSet returnDataSet;
            string fullParamList;

            try
            {
                dataViewParams = _GUIWebServices.GetData(true, _username, _password, "get_dataview_parameters", ":dataview=" + dataviewName, 0, 0, null);
                if (dataViewParams.Tables.Contains("get_dataview_parameters") &&
                    dataViewParams.Tables["get_dataview_parameters"].Rows.Count > 0)
                {
                    fullParamList = "";
                    string[] paramKeyValueList = delimitedParameterList.Split(new char[] { ';' });
                    foreach (DataRow dr in dataViewParams.Tables["get_dataview_parameters"].Rows)
                    {
                        string paramName = dr["param_name"].ToString();
                        fullParamList += paramName + "=; ";
                        foreach (string paramKeyValue in paramKeyValueList)
                        {
                            if (paramKeyValue.Contains(paramName))
                            {
                                fullParamList = fullParamList.Replace(paramName + "=; ", paramName + "=" + paramKeyValue.Substring(paramKeyValue.IndexOf('=') + 1).Trim() + "; ");
                            }
                        }
                    }
                }
                else
                {
                    if (delimitedParameterList.Length > 0)
                    {
                        fullParamList = delimitedParameterList;
                    }
                    else
                    {
                        fullParamList = ":accessionid=; :inventoryid=; :orderrequestid=; :cooperatorid=; :createddate=; :modifieddate=; :startpkey=; :stoppkey=; :displaymember=; :valuemember=; :dataview=; :tablename=; pkfieldname=; :seclangid=; :name=;";
                    }
                }
                returnDataSet = _GUIWebServices.GetData(true, _username, _password, dataviewName, fullParamList, offset, limit, null);
                if (returnDataSet.Tables.Contains(dataviewName))
                {
                    ApplyColumnConstraints(returnDataSet.Tables[dataviewName]);
                }
                return returnDataSet;
            }
            catch(Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        // new get data for ST
        public DataSet GetData2(string dataviewName, string delimitedParameterList, int offset, int limit) {
            //DataSet dataViewParams;
            DataSet returnDataSet;
            List<string> dvParamList = new List<string>();
            string fullParamList = "";

            try {

                dvParamList = DataviewParameters(dataviewName);
                if (dvParamList.Count > 0) {
                    fullParamList = string.Join("=; ", dvParamList.ToArray()) + "=; ";
                    string[] paramKeyValueList = delimitedParameterList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    if (paramKeyValueList.Length > 0) {

                        // place each input parameter into the dataview parameter list
                        foreach (string item in paramKeyValueList) {
                            string pkv = item.Trim();
                            int eqPos = pkv.IndexOf('=');
                            if (eqPos < 2 || eqPos + 1 == pkv.Length) continue;
                            string underParamName = pkv.Substring(0, eqPos);
                            string inParamName = underParamName.Replace("_", "");

                            if (dvParamList.Contains(inParamName)) {
                                fullParamList = fullParamList.Replace(inParamName + "=; ", inParamName + "=" + pkv.Substring(pkv.IndexOf('=') + 1).Trim() + "; ");

                                // given parameter is not accepted by the dataview, lets try to convert it
                            } else {
                                string pKeyName = DataviewName2PKName(dataviewName);
                                string newParamName = ":" + pKeyName.Replace("_", "");
                                if (dvParamList.Contains(newParamName)) {
                                    //MessageBox.Show("DEBUG: GetData no replace for dataview: " + dataviewName + "\r\nlist: " + delimitedParameterList);
                                    string resolveTable = pKeyName.Replace("_id", "");  // assumes all PKs follow the form tablename_id
                                    string queryTable = underParamName.Substring(1, underParamName.Length - 4);
                                    string query = "@" + queryTable + "." + underParamName.Substring(1) + " IN (" + pkv.Substring(pkv.IndexOf('=') + 1) + ")";
                                    //MessageBox.Show("DEBUG: GetData resolve: " + resolveTable + " eqpos: " + eqPos.ToString() + " pkvlen: " + pkv.Length.ToString() + "\r\ninput: " + pkv + "\r\nquery: " + query);

                                    DataSet dsConvertResults;
                                    dsConvertResults = _GUIWebServices.Search(true, _username, _password, query, true, true, "accession", resolveTable, offset, limit, "ignorecache=false");
                                    if (dsConvertResults != null && dsConvertResults.Tables.Contains("SearchResult")) {
                                        DataRow[] basicQuerySearchResults = dsConvertResults.Tables["SearchResult"].Select("ID IS NOT NULL");
                                        if (basicQuerySearchResults.Length > 0) {
                                            string pkeyList = newParamName + "=";
                                            for (int i = 0; i < basicQuerySearchResults.Length; i++) {
                                                pkeyList += basicQuerySearchResults[i]["ID"].ToString() + ",";
                                            }
                                            fullParamList = fullParamList.Replace(newParamName + "=; ", pkeyList.Trim() + "; ");
                                        }
                                    }
                                }
                            }
                        }
                    }

                } else {
                    if (delimitedParameterList.Length > 0) {
                        fullParamList = delimitedParameterList;
                    } else {
                        fullParamList = ":accessionid=; :inventoryid=; :orderrequestid=; :cooperatorid=; :createddate=; :modifieddate=; :startpkey=; :stoppkey=; :displaymember=; :valuemember=; :dataview=; :tablename=; pkfieldname=; :seclangid=; :name=;";
                    }
                }

                returnDataSet = _GUIWebServices.GetData(true, _username, _password, dataviewName, fullParamList, offset, limit, null);
                if (returnDataSet.Tables.Contains(dataviewName)) {
                    ApplyColumnConstraints(returnDataSet.Tables[dataviewName]);
                }
                return returnDataSet;
            }
            catch (Exception err) {
                return BuildExceptionDataSet(err);
            }
        }

        private Dictionary<string, List<string>> _dvparams;

        public List<string> DataviewParameters(string dataviewName) {
            if (_dvparams == null) _dvparams = new Dictionary<string, List<string>>();
            if (_dvparams.ContainsKey(dataviewName)) {
                //MessageBox.Show("DEBUG: dvn2pkn  remembered dvname: " + dataviewName + " params: " + string.Join("=; ", _dvparams[dataviewName].ToArray()) + "=; ");
                return _dvparams[dataviewName];
            }

            List<string> parameters = new List<string>();

            // read dataview to get parameter list
            DataSet dataViewParams;
            dataViewParams = _GUIWebServices.GetData(true, _username, _password, "get_dataview_parameters", ":dataview=" + dataviewName, 0, 0, null);
            if (dataViewParams.Tables.Contains("get_dataview_parameters") &&
                dataViewParams.Tables["get_dataview_parameters"].Rows.Count > 0) {
                foreach (DataRow dr in dataViewParams.Tables["get_dataview_parameters"].Rows) {
                    string paramName = dr["param_name"].ToString();
                    parameters.Add(paramName);
                }
            } else {
                return parameters;
            }

            if (parameters.Count > 0) _dvparams.Add(dataviewName, parameters);
            return parameters;
        }

        private Dictionary<string, string> _dvpk;

        public string DataviewName2PKName(string dataviewName) {
            if (_dvpk == null) _dvpk = new Dictionary<string, string>();
            if (_dvpk.ContainsKey(dataviewName)) {
                //MessageBox.Show("DEBUG: dvn2pkn  remembered dvname: " + dataviewName + " pkname: " + _dvpk[dataviewName]);
                return _dvpk[dataviewName];
            }

            DataSet dataViewPK;
            dataViewPK = _GUIWebServices.GetData(true, _username, _password, "sys_dataview_primary_key", ":dataviewname=" + dataviewName, 0, 0, null);
            if (dataViewPK.Tables.Contains("sys_dataview_primary_key") && dataViewPK.Tables["sys_dataview_primary_key"].Rows.Count > 0) {
                // lets have a look
                foreach (DataRow dr in dataViewPK.Tables["sys_dataview_primary_key"].Rows) {
                    string pKeyName = dr["field_name"].ToString();
                    //MessageBox.Show("DEBUG: dvn2pkn dvname: " + dataviewName + " pkname: " + pKeyName);
                    _dvpk.Add(dataviewName, pKeyName);
                    return pKeyName;
                }
            }

            // fail back to simple procedure
            return dataviewName.Replace("get_", "") + "_id";
        }


        public DataSet SaveData(DataSet modifiedDataSet)
        {
            try
            {
                return _GUIWebServices.SaveData(true, _username, _password, modifiedDataSet, null);
            }
            catch (Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        private DataSet BuildExceptionDataSet(Exception err)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("ExceptionTable");
            dt.Columns.Add("ExceptionIndex", typeof(Int32));
            dt.Columns.Add("ExceptionType", typeof(string));
            dt.Columns.Add("Data", typeof(string));
            dt.Columns.Add("Message", typeof(string));
            dt.Columns.Add("Source", typeof(string));
            dt.Columns.Add("StackTrace", typeof(string));
            dt.Columns.Add("InnerException", typeof(string));
            DataRow dr = dt.NewRow();
            dr["ExceptionIndex"] = 1;
            dr["ExceptionType"] = err.GetType().FullName;
            dr["Data"] = err.Data.ToString();
            dr["Message"] = err.Message;
            dr["Source"] = err.Source;
            dr["StackTrace"] = err.StackTrace;
            if(err.InnerException != null && err.InnerException.Message != null) dr["InnerException"] = err.InnerException.Message;
            dt.Rows.Add(dr);
            ds.Tables.Add(dt);
            return ds;
        }

        public DataSet Search(string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int offset, int limit, string searchOptions)
        {
            try
            {
                return _GUIWebServices.Search(true, _username, _password, query, ignoreCase, andTermsTogether, indexList, resolverName, offset, limit, searchOptions);
            }
            catch (Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        public DataSet ValidateLogin()
        {
            try
            {
                return _GUIWebServices.ValidateLogin(true, _username, _password);
            }
            catch (Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        public DataSet ChangeLanguage(int newLanguage)
        {
            try
            {
                return _GUIWebServices.ChangeLanguage(true, _username, _password, newLanguage);
            }
            catch (Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        public DataSet ChangePassword(string newPassword)
        {
            try
            {
                return _GUIWebServices.ChangePassword(true, _username, _password, _username, newPassword);
            }
            catch (Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        public DataSet ChangeOwnership(DataSet ownedDataset, int newCNO, bool includeChildren)
        {
            try
            {
                return _GUIWebServices.TransferOwnership(true, _username, _password, ownedDataset, newCNO, includeChildren);
            }
            catch (Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        public string GetVersion()
        {
            try
            {
                return _GUIWebServices.GetVersion();
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        public string UploadImage(string destinationFilePath, byte[] imageBytes, bool createThumbnail, bool overWriteIfExists)
        {
            try
            {
                return _GUIWebServices.UploadImage(_username, _password, destinationFilePath, imageBytes, createThumbnail, overWriteIfExists);
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        public byte[] DownloadImage(string remoteFilePath)
        {
            try
            {
                return _GUIWebServices.DownloadImage(_username, _password, remoteFilePath);
            }
            catch (Exception err)
            {
                return new byte[0];
            }
        }

        public void ApplyColumnConstraints(DataTable dataviewTable)
        {
            if (dataviewTable != null &&
                dataviewTable.Columns != null &&
                dataviewTable.Columns.Count > 0)
            {
                foreach (DataColumn dc in dataviewTable.Columns)
                {
                    if (dc.ExtendedProperties.Contains("is_primary_key") &&
                        dc.ExtendedProperties["is_primary_key"].ToString() == "Y")
                    {
                        if (dc.DataType == typeof(int))
                        {
                            dc.AllowDBNull = false;
                            dc.AutoIncrement = true;
                            dc.AutoIncrementSeed = -1;
                            dc.AutoIncrementStep = -1;
                            dataviewTable.PrimaryKey = new DataColumn[1] { dc };
                        }
                        else
                        {
                            dc.AllowDBNull = false;
                            dataviewTable.PrimaryKey = new DataColumn[1] { dc };
                        }
                    }
                }
            }
        }
    }
}
