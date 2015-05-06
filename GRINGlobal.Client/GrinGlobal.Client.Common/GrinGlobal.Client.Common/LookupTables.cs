﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace GRINGlobal.Client.Common
{
    public class LookupTables
    {
        private WebServices _webServices;
        private LocalDatabase _localData;
        private Dictionary<string, bool> _lookupTableStatus;
        private int _pageSize = 1000;
        private Dictionary<string, Dictionary<int, string>> _pkeyLUTCacheCollection = new Dictionary<string, Dictionary<int, string>>();
        private Dictionary<string, Dictionary<string, int>> _pkeyRLUTCacheCollection = new Dictionary<string, Dictionary<string, int>>();
        private Dictionary<string, string> _codevalueLUTCache = new Dictionary<string, string>();
        private bool _optimizeLUTForSpeed = false;

        private List<string> _loadingQueue = new List<string>();

        public LookupTables(WebServices webServices, LocalDatabase localData, bool optimizeLUTForSpeed)
        {
            _webServices = webServices;
            _localData = localData;
            _lookupTableStatus = new Dictionary<string, bool>();
            _optimizeLUTForSpeed = optimizeLUTForSpeed;

            // Dictionary for fast lookup...
            _pkeyLUTCacheCollection = LoadPKeyLUTCacheCollection();
            // Dictionary for fast reverse lookup...
            _pkeyRLUTCacheCollection = LoadPKeyRLUTCacheCollection();
            _codevalueLUTCache = LoadCodeValueLUTDictionary("code_value_lookup");
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        private Dictionary<string, Dictionary<int, string>> LoadPKeyLUTCacheCollection()
        {
            Dictionary<string, Dictionary<int, string>> pkeyLUTCache = new Dictionary<string,Dictionary<int,string>>();
            DataTable dt = _localData.GetData("SELECT * FROM lookup_table_status", new string[0]);

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["dataview_name"].ToString() != "code_value_lookup")
                {
                    pkeyLUTCache.Add(dr["dataview_name"].ToString(), LoadPKeyLUTDictionary(dr["dataview_name"].ToString()));
                }
            }

            return pkeyLUTCache;
        }

        private Dictionary<int, string> LoadPKeyLUTDictionary(string pkeyLUTName)
        {
            string remoteServerName = "GRINGlobal_" + _webServices.Url.ToLower().Replace("http://", "").Replace("/gringlobal/gui.asmx", "").Replace('-', '_').Replace('.', '_').Replace(':', '_');
            string destinationFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\GRIN-Global\Curator Tool\" + remoteServerName;

            Dictionary<int, string> pkeyLUTCache = new Dictionary<int, string>();
            if (File.Exists(destinationFilePath + "\\" + pkeyLUTName + "_cache.dat"))
            {
                // If the LUT has been previously cached to memory and saved to disk - load that version...
                FileStream fs = File.Open(destinationFilePath + "\\" + pkeyLUTName + "_cache.dat", FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                int count = br.ReadInt32();
                for (int n = 0; n < count; n++)
                {
                    var key = br.ReadInt32();
                    var value = br.ReadString();
                    pkeyLUTCache.Add(key, value);
                }

                br.Close();
            }
            else
            {
                //  This LUT has never been cached so if user is requesting LUT speed optimizations - load the full LUT into 
                //  memory from SQL Server Express...
                if (_optimizeLUTForSpeed)
                {
                    DataTable dt = _localData.GetData("SELECT value_member, display_member FROM " + pkeyLUTName, new string[0]);
                    foreach (DataRow dr in dt.Rows)
                    {
                        pkeyLUTCache.Add((int)dr["value_member"], dr["display_member"].ToString());
                    }
                }
                // Save the PKeyLUTDictionary to disk...
                SavePKeyLUTDictionary(pkeyLUTName, pkeyLUTCache);
            }

            return pkeyLUTCache;
        }

        private Dictionary<string, Dictionary<string, int>> LoadPKeyRLUTCacheCollection()
        {
            Dictionary<string, Dictionary<string, int>> pkeyRLUTCache = new Dictionary<string, Dictionary<string, int>>();
            DataTable dt = _localData.GetData("SELECT * FROM lookup_table_status", new string[0]);

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["dataview_name"].ToString() != "code_value_lookup")
                {
                    pkeyRLUTCache.Add(dr["dataview_name"].ToString(), LoadPKeyRLUTDictionary(dr["dataview_name"].ToString()));
                }
            }

            return pkeyRLUTCache;
        }

        private Dictionary<string, int> LoadPKeyRLUTDictionary(string pkeyLUTName)
        {
            string remoteServerName = "GRINGlobal_" + _webServices.Url.ToLower().Replace("http://", "").Replace("/gringlobal/gui.asmx", "").Replace('-', '_').Replace('.', '_').Replace(':', '_');
            string destinationFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\GRIN-Global\Curator Tool\" + remoteServerName;

            Dictionary<string, int> pkeyRLUTCache = new Dictionary<string, int>();
            if (File.Exists(destinationFilePath + "\\" + pkeyLUTName + "_RLUT_cache.dat"))
            {
                // If the LUT has been previously cached to memory and saved to disk - load that version...
                FileStream fs = File.Open(destinationFilePath + "\\" + pkeyLUTName + "_RLUT_cache.dat", FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                int count = br.ReadInt32();
                for (int n = 0; n < count; n++)
                {
                    var key = br.ReadString();
                    var value = br.ReadInt32();
                    pkeyRLUTCache.Add(key, value);
                }

                br.Close();
            }
//else
//{
//    //  This LUT has never been cached so if user is requesting LUT speed optimizations - load the full LUT into 
//    //  memory from SQL Server Express...
//    if (_optimizeLUTForSpeed)
//    {
//        DataTable dt = _localData.GetData("SELECT value_member, display_member FROM " + pkeyLUTName, new string[0]);
//        foreach (DataRow dr in dt.Rows)
//        {
//            pkeyLUTCache.Add((int)dr["value_member"], dr["display_member"].ToString());
//        }
//    }
//    // Save the PKeyLUTDictionary to disk...
//    SavePKeyLUTDictionary(pkeyLUTName, pkeyLUTCache);
//}

            return pkeyRLUTCache;
        }

        private Dictionary<string, string> LoadCodeValueLUTDictionary(string fkeyLUTName)
        {
            string remoteServerName = "GRINGlobal_" + _webServices.Url.ToLower().Replace("http://", "").Replace("/gringlobal/gui.asmx", "").Replace('-', '_').Replace('.', '_').Replace(':', '_');
            string destinationFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\GRIN-Global\Curator Tool\" + remoteServerName;

            Dictionary<string, string> codevalueLUTCache = new Dictionary<string, string>();
            if (File.Exists(destinationFilePath + "\\" + fkeyLUTName + "_cache.dat"))
            {
                // If the LUT has been previously cached to memory and saved to disk - load that version...
                FileStream fs = File.Open(destinationFilePath + "\\" + fkeyLUTName + "_cache.dat", FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                int count = br.ReadInt32();
                for (int n = 0; n < count; n++)
                {
                    var key = br.ReadString();
                    var value = br.ReadString();
                    codevalueLUTCache.Add(key, value);
                }

                br.Close();
            }
            else
            {
                //  This LUT has never been cached so load it into memory from SQL Server Express and save it to disk...
                DataTable dt = _localData.GetData("SELECT group_name, value_member, display_member FROM " + fkeyLUTName, new string[0]);
                foreach (DataRow dr in dt.Rows)
                {
                    if (codevalueLUTCache.ContainsKey(dr["group_name"].ToString() + dr["value_member"].ToString()))
                    {
                        codevalueLUTCache[dr["group_name"].ToString() + dr["value_member"].ToString()] = dr["display_member"].ToString();
                    }
                    else
                    {
                        codevalueLUTCache.Add(dr["group_name"].ToString() + dr["value_member"].ToString(), dr["display_member"].ToString());
                    }
                }
                SaveCodeValueLUTDictionary(fkeyLUTName, codevalueLUTCache);
            }

            return codevalueLUTCache;
        }

        private void UpdatePKeyLUTDictionary(string pkeyLUTName, Dictionary<int, string> pkeyLUTCache)
        {
            //  Update the pkeyLUT unless user is requesting LUT speed optimizations - then load the full LUT into 
            //  memory from SQL Server Express...
            if (_optimizeLUTForSpeed)
            {
                DataTable dt = _localData.GetData("SELECT value_member, display_member FROM " + pkeyLUTName, new string[0]);
                // Clear the dictionary...
                pkeyLUTCache.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    if (pkeyLUTCache.ContainsKey((int)dr["value_member"]))
                    {
                        pkeyLUTCache[(int)dr["value_member"]] = dr["display_member"].ToString();
                    }
                    else
                    {
                        pkeyLUTCache.Add((int)dr["value_member"], dr["display_member"].ToString());
                    }
                }
            }
            else
            {
                // Go get an updated version of each key/value pair in the dictionary from the local LUT...
                int pageSize = 1000;
                int[] pkeys = new int[pkeyLUTCache.Count];
                pkeyLUTCache.Keys.CopyTo(pkeys, 0);
                int pageStart = 0;
                int pageStop = Math.Min(pageSize, pkeyLUTCache.Count);
                while (pageStart < pkeyLUTCache.Count)
                {
                    string pkeylist = "";
                    for (int i = pageStart; i < pageStop; i++)
                    {
                        pkeylist += pkeys[i].ToString() + ",";
                    }
                    // Update the paging indexes...
                    pageStart = pageStop;
                    pageStop = Math.Min((pageStart + pageSize), pkeyLUTCache.Count);
                    // Build the param string and get the data...
//                    DataTable dt = _localData.GetData("SELECT value_member, display_member FROM " + pkeyLUTName + " WHERE value_member in (@valuemember)", new string[1] { "@valuemember=" + pkeylist.TrimEnd(',') });
                    DataTable dt = _localData.GetData("SELECT value_member, display_member FROM " + pkeyLUTName + " WHERE value_member in (" + pkeylist.TrimEnd(',') + ")", new string[0]);
                    foreach (DataRow dr in dt.Rows)
                    {
                        // For each returned row - find the dictionary key/value pair and update it...
                        if (pkeyLUTCache.ContainsKey((int)dr["value_member"]))
                        {
                            pkeyLUTCache[(int)dr["value_member"]] = dr["display_member"].ToString();
                        }
                        else
                        {
                            pkeyLUTCache.Add((int)dr["value_member"], dr["display_member"].ToString());
                        }
                    }
                }
                // Special processing for the cooperator_lookup to always load the cooperators for that user's site...
                if (pkeyLUTName.ToLower().Trim() == "cooperator_lookup")
                {
                    DataTable dt = _localData.GetData("SELECT value_member, display_member FROM " + pkeyLUTName + " WHERE account_is_enabled = 'Y' AND site = @site", new string[1] { "@site=" + _webServices.Site });
                    foreach (DataRow dr in dt.Rows)
                    {
                        // For each returned row - find the dictionary key/value pair and update it...
                        if (pkeyLUTCache.ContainsKey((int)dr["value_member"]))
                        {
                            pkeyLUTCache[(int)dr["value_member"]] = dr["display_member"].ToString();
                        }
                        else
                        {
                            pkeyLUTCache.Add((int)dr["value_member"], dr["display_member"].ToString());
                        }
                    }
                }
            }
            // Save the PKeyLUTDictionary to disk...
            SavePKeyLUTDictionary(pkeyLUTName, pkeyLUTCache);
        }

        private Dictionary<string, string> UpdateCodeValueLUTDictionary(string fkeyLUTName, Dictionary<string, string> codevalueLUTCache)
        {
            //  Reload this LUT from SQL Server Express and save it to disk...
            DataTable dt = _localData.GetData("SELECT group_name, value_member, display_member FROM " + fkeyLUTName, new string[0]);
            // Clear the dictionary...
            codevalueLUTCache.Clear();
            // Now re-add all codes from the fkeyLUTName (aka code_value_lookup) table...
            foreach (DataRow dr in dt.Rows)
            {
                if (codevalueLUTCache.ContainsKey(dr["group_name"].ToString() + dr["value_member"].ToString()))
                {
                    codevalueLUTCache[dr["group_name"].ToString() + dr["value_member"].ToString()] = dr["display_member"].ToString();
                }
                else
                {
                    codevalueLUTCache.Add(dr["group_name"].ToString() + dr["value_member"].ToString(), dr["display_member"].ToString());
                }
            }
            // Save the codevalueLUTDictionary to disk...
            SaveCodeValueLUTDictionary(fkeyLUTName, codevalueLUTCache);

            return codevalueLUTCache;
        }

        public void SaveAllLUTDictionaries()
        {
            // Save the PKey Dictionaries...
            foreach (string lutName in _pkeyLUTCacheCollection.Keys)
            {
                SavePKeyLUTDictionary(lutName, _pkeyLUTCacheCollection[lutName]);
            }
            // Save the PKey RLUT Dictionaries...
            foreach (string lutName in _pkeyRLUTCacheCollection.Keys)
            {
                SavePKeyRLUTDictionary(lutName, _pkeyRLUTCacheCollection[lutName]);
            }
            // Save the code_value Dictionary...
            SaveCodeValueLUTDictionary("code_value_lookup", _codevalueLUTCache);
        }

        private void SavePKeyLUTDictionary(string fkeyLUTName, Dictionary<int, string> dictionary)
        {
            string remoteServerName = "GRINGlobal_" + _webServices.Url.ToLower().Replace("http://", "").Replace("/gringlobal/gui.asmx", "").Replace('-', '_').Replace('.', '_').Replace(':', '_');
            string destinationFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\GRIN-Global\Curator Tool\" + remoteServerName;
            if (!Directory.Exists(destinationFilePath)) Directory.CreateDirectory(destinationFilePath);
            FileStream fs = File.Open(destinationFilePath + "\\" + fkeyLUTName + "_cache.dat", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(dictionary.Count);
            foreach (var kvp in dictionary)
            {
                bw.Write(kvp.Key);
                bw.Write(kvp.Value);
            }
            bw.Flush();
            bw.Close();
        }

        private void SavePKeyRLUTDictionary(string fkeyLUTName, Dictionary<string, int> dictionary)
        {
            string remoteServerName = "GRINGlobal_" + _webServices.Url.ToLower().Replace("http://", "").Replace("/gringlobal/gui.asmx", "").Replace('-', '_').Replace('.', '_').Replace(':', '_');
            string destinationFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\GRIN-Global\Curator Tool\" + remoteServerName;
            if (!Directory.Exists(destinationFilePath)) Directory.CreateDirectory(destinationFilePath);
            FileStream fs = File.Open(destinationFilePath + "\\" + fkeyLUTName + "_RLUT_cache.dat", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(dictionary.Count);
            foreach (var kvp in dictionary)
            {
                bw.Write(kvp.Key);
                bw.Write(kvp.Value);
            }
            bw.Flush();
            bw.Close();
        }

        private void SaveCodeValueLUTDictionary(string fkeyLUTName, Dictionary<string, string> dictionary)
        {
            string remoteServerName = "GRINGlobal_" + _webServices.Url.ToLower().Replace("http://", "").Replace("/gringlobal/gui.asmx", "").Replace('-', '_').Replace('.', '_').Replace(':', '_');
            string destinationFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\GRIN-Global\Curator Tool\" + remoteServerName;
            if (!Directory.Exists(destinationFilePath)) Directory.CreateDirectory(destinationFilePath);
            FileStream fs = File.Open(destinationFilePath + "\\" + fkeyLUTName + "_cache.dat", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(dictionary.Count);
            foreach (var kvp in dictionary)
            {
                bw.Write(kvp.Key);
                bw.Write(kvp.Value);
            }
            bw.Flush();
            bw.Close();
        }

        public bool IsUpdated(string dataviewName)
        {
            bool tableStatus = false;
            if (_lookupTableStatus.ContainsKey(dataviewName))
            {
                tableStatus = _lookupTableStatus[dataviewName];
            }
            else if (_localData.TableExists("lookup_table_status"))
            {
                DataTable localLookupTableStats = _localData.GetData("SELECT * FROM lookup_table_status WHERE dataview_name=@dataviewname", new string[1] { "@dataviewname=" + dataviewName });
                DataRow localLookupTableStatsRow = localLookupTableStats.Rows.Find(dataviewName);
                if (localLookupTableStatsRow != null &&
                    (localLookupTableStatsRow["status"].ToString().ToUpper() == "COMPLETED" ||
                    localLookupTableStatsRow["status"].ToString().ToUpper() == "UPDATED"))
                {
                    Int64 max = 1001;
                    Int64 current = 0;
                    if (Int64.TryParse(localLookupTableStatsRow["max_pk"].ToString(), out max) &&
                        Int64.TryParse(localLookupTableStatsRow["current_pk"].ToString(), out current) &&
                        current == max) //(max-current) < 1000)
                    {
                        tableStatus = true;
                    }
                }
                _lookupTableStatus.Add(dataviewName, tableStatus);
            }
            return tableStatus;
        }

        public bool IsValidFKField(DataColumn dc)
        {
            bool validFKField = false;
            if (dc.ExtendedProperties.Contains("gui_hint") && dc.ExtendedProperties["gui_hint"].ToString().ToUpper().Trim() == "LARGE_SINGLE_SELECT_CONTROL" &&
                dc.ExtendedProperties.Contains("foreign_key_dataview_name") && dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Length > 0) validFKField = true;

            return validFKField;
        }

        public bool IsValidCodeValueField(DataColumn dc)
        {
            bool validCodeValueField = false;
            if (dc.ExtendedProperties.Contains("gui_hint") && dc.ExtendedProperties["gui_hint"].ToString().ToUpper().Trim() == "SMALL_SINGLE_SELECT_CONTROL" &&
                dc.ExtendedProperties.Contains("group_name") && dc.ExtendedProperties["group_name"].ToString().Length > 0) validCodeValueField = true;

            return validCodeValueField;
        }

        public void ClearLocalLookupTable(object objDataviewName)
        {
            string dataviewName = (string)objDataviewName;
            if (_localData.TableExists("lookup_table_status"))
            {
                DataTable localLookupTableStats = _localData.GetData("SELECT * FROM lookup_table_status WHERE dataview_name=@dataviewname", new string[1] { "@dataviewname=" + dataviewName });
                DataRow localLookupTableStatsRow = localLookupTableStats.Rows.Find(dataviewName);
                if (localLookupTableStatsRow != null)
                {
                    localLookupTableStatsRow["current_pk"] = localLookupTableStatsRow["min_pk"];
                    localLookupTableStatsRow["status"] = "Empty";
                    localLookupTableStatsRow["sync_date"] = DateTime.UtcNow;
                    SaveDataPageToLocalDB(localLookupTableStats);
                }
            }

            // Drop the tabel from the local DB if it exists...
            if (_localData.TableExists(dataviewName)) _localData.Remove(dataviewName);
        }

        public void LoadTableFromDatabase(object objDataviewName)
        {
            string dataviewName = (string)objDataviewName;
            int pageSize = _pageSize;
            int pageOffset = 0;
            DataSet dataPage = new DataSet();
            DataTable dataviewTable = null;
            string unusedParameters = ":createddate=" + DateTime.Today.AddYears(1) + "; :modifieddate=" + DateTime.Today.AddYears(1) + "; :valuemember=; :displaymember;";
            string startAndStopPKeys = "";
            // Add this to the loadingQueue list (so that no more threads get started to build this table)...
            if (!_loadingQueue.Contains(dataviewName))
            {
                _loadingQueue.Add(dataviewName);
            }
            else
            {
                // There is already a thread to load this table running so bail out now...
                return;
            }

            DataTable localLookupTableStats = new DataTable("lookup_table_status");
            // If the lookup_table_status table has been built get the row for the table to be loaded (otherwise call sync to build the table)...
            if (!_localData.TableExists("lookup_table_status"))
            {
                GetSynchronizationStats();
            }
            localLookupTableStats = _localData.GetData("SELECT * FROM lookup_table_status WHERE dataview_name=@dataviewname", new string[1] { "@dataviewname=" + dataviewName });

            // Get the statistics row for this table...
            DataRow localLookupTableStatsRow = localLookupTableStats.Rows.Find(dataviewName);
            if (localLookupTableStatsRow != null)
            {
                // Change the status of the lookup table statistics to 'Loading'...
                localLookupTableStatsRow["status"] = "Loading";
                SaveDataPageToLocalDB(localLookupTableStats);
                // Begin loading the lookup table page by page...
                pageOffset = (int)localLookupTableStatsRow["current_pk"];
                for (int i = pageOffset; i <= (int)localLookupTableStatsRow["max_pk"]; i += pageSize)
                {
                    startAndStopPKeys = " :startpkey=" + i.ToString() + "; :stoppkey=" + (i + pageSize - 1).ToString();
                    try
                    {
                        dataPage = _webServices.GetData(localLookupTableStatsRow["dataview_name"].ToString(), unusedParameters + startAndStopPKeys, 0, 0);
                        if (!dataPage.Tables.Contains(localLookupTableStatsRow["dataview_name"].ToString()))
                        {
//System.Windows.Forms.MessageBox.Show("Error retrieving data for lookup table: '" + dataviewName.ToUpper() + "' --- Retrying from PKEY=" + i.ToString(), "Lookup Table Load Error");
                            i -= pageSize;
                            continue;
                        }
                        if (dataviewTable == null)
                        {
                            // Create an empty copy of the lookup table...
                            dataviewTable = dataPage.Tables[localLookupTableStatsRow["dataview_name"].ToString()].Clone();
//// Apply any column constraints (indicated in the column extended properties) to the table...
//ApplyColumnConstraints(dataviewTable);
                        }
                        dataviewTable.Rows.Clear();
                        dataviewTable.AcceptChanges();
                        dataviewTable.Load(dataPage.Tables[localLookupTableStatsRow["dataview_name"].ToString()].CreateDataReader(), LoadOption.Upsert);
                        SaveDataPageToLocalDB(dataviewTable);
                        localLookupTableStatsRow["current_pk"] = Math.Min((int)localLookupTableStatsRow["max_pk"], i + pageSize - 1);
                        localLookupTableStatsRow["status"] = "Loading";
                        SaveDataPageToLocalDB(localLookupTableStats);
                    }
                    catch (Exception err)
                    {
                        if (dataPage.Tables.Contains("ExceptionTable") &&
                            dataPage.Tables["ExceptionTable"].Rows.Count > 0)
                        {
//System.Windows.Forms.MessageBox.Show("Error retrieving data for lookup table: '" + dataviewName.ToUpper() + "' --- Aborting this background task.\n\nFull Server Error Message:\n\n" + dataPage.Tables["ExceptionTable"].Rows[0]["Message"].ToString(), "Lookup Table Load Error");
SharedUtils sharedUtils = new SharedUtils(_webServices.Url, _webServices.Username, _webServices.Password_ClearText, true);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error retrieving data for lookup table: '{0}' --- Aborting this background task.\n\nFull Server Error Message:\n\n{1}", "Lookup Table Load Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "LookupTables_LoadTableFromDatabaseMessage1";
if (sharedUtils != null && sharedUtils.IsConnected) sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}") &&
    ggMessageBox.MessageText.Contains("{1}"))
{
    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewName.ToUpper(), dataPage.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
}
else if (ggMessageBox.MessageText.Contains("{0}"))
{
    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewName.ToUpper());
}
else if (ggMessageBox.MessageText.Contains("{1}"))
{
    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataPage.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
}
ggMessageBox.ShowDialog();
                        }
                        else
                        {
//System.Windows.Forms.MessageBox.Show("Error retrieving data for lookup table: '" + dataviewName.ToUpper() + "' --- Aborting this background task.\n\nFull Internal Error Message:\n\n" + err.Message, "Lookup Table Load Error");
SharedUtils sharedUtils = new SharedUtils(_webServices.Url, _webServices.Username, _webServices.Password_ClearText, true);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error retrieving data for lookup table: '{0}' --- Aborting this background task.\n\nFull Internal Error Message:\n\n{1}", "Lookup Table Load Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "LookupTables_LoadTableFromDatabaseMessage2";
if (sharedUtils != null && sharedUtils.IsConnected) sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}") &&
    ggMessageBox.MessageText.Contains("{1}"))
{
    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewName.ToUpper(), err.Message);
}
else if (ggMessageBox.MessageText.Contains("{0}"))
{
    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewName.ToUpper());
}
else if (ggMessageBox.MessageText.Contains("{1}"))
{
    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, err.Message);
}
ggMessageBox.ShowDialog();
                        }
                        // Update the status of the LU table load on the local DB lookup_table_status table...
                        localLookupTableStatsRow["status"] = "Failed";
                        SaveDataPageToLocalDB(localLookupTableStats);
                        break;
                    }
                }
                // Looks like we were successful in loading this LU table so if this table is cached - clear it from cache...
                if (_pkeyLUTCacheCollection.ContainsKey(dataviewName))
                {
                    UpdatePKeyLUTDictionary(dataviewName, _pkeyLUTCacheCollection[dataviewName]);
                }
                else if (dataviewName.ToLower().Trim() == "code_value_lookup")
                {
                    UpdateCodeValueLUTDictionary("code_value_lookup", _codevalueLUTCache);
                }
                // Update the status of the LU table load on the local DB lookup_table_status table...
                localLookupTableStatsRow["status"] = "Completed";
                SaveDataPageToLocalDB(localLookupTableStats);
            }
            // Finished adding the lookup table - remove the name from the loadingQueue list...
            _loadingQueue.Remove(dataviewName);
            // Clear the lookup table status cache (so that it can be refreshed for each lookup table on demand)...
            _lookupTableStatus.Clear();
        }

        private void SaveDataPageToLocalDB(DataTable dt)
        {
            // If there are no rows to save bail out now...
            if (dt.Rows.Count < 1) return;
            // If this table does not exist in the local copy of the database create it now...
            if (!_localData.TableExists(dt.TableName))
            {
                _localData.CreateTable(dt, true, true);
            }

            // Save the new rows to the local copy of the database...
            try
            {
                int startPKey = -1;
                int stopPKey = -1;
                string selectSQL = "SELECT * FROM " + dt.TableName;
                if (dt.PrimaryKey.Length == 1 && dt.PrimaryKey[0].DataType == typeof(int))
                {
                    string currentSort = dt.DefaultView.Sort;
                    dt.DefaultView.Sort = dt.PrimaryKey[0].ColumnName + " ASC";
                    startPKey = (int)dt.DefaultView[0][dt.PrimaryKey[0].ColumnName];
                    stopPKey = (int)dt.DefaultView[dt.Rows.Count - 1][dt.PrimaryKey[0].ColumnName];
                    dt.DefaultView.Sort = currentSort;
                }
                if (startPKey > -1 && stopPKey > -1)
                {
                    selectSQL += " WHERE " + dt.PrimaryKey[0].ColumnName + " BETWEEN " + startPKey.ToString() + " AND " + stopPKey.ToString();
                }
                DataTable tempDT = new DataTable(dt.TableName);
                tempDT = _localData.GetData(selectSQL, new string[0]);
                // Update (or insert) any rows that need updating...
                tempDT.Load(dt.CreateDataReader(), LoadOption.Upsert);
                // Delete any rows that need to be deleted...
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow rowToDelete = null;
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        rowToDelete = tempDT.Rows.Find(dr[dt.PrimaryKey[0], DataRowVersion.Original]);
                        if (rowToDelete != null) rowToDelete.Delete();
                    }
                }
                _localData.SaveData(tempDT);
            }
            catch (Exception err)
            {
            }
        }

        public string GetCodeValueValueMember(string displayMember, string groupName, string defaultDisplayMember)
        {
            string valueMember = "!Error!";

            // Look in the memory cache for code_value (if it is not there use default)...
            if (_codevalueLUTCache.ContainsValue(displayMember))
            {
                var keys = from k in _codevalueLUTCache
                           where k.Value == displayMember && k.Key.StartsWith(groupName)
                           select k.Key;

                if (keys.Count() > 0) valueMember = keys.First().Substring(groupName.Length);
            }
            else
            {
                valueMember = defaultDisplayMember;
            }

            return valueMember;
        }

        public int GetPKeyValueMember(DataRow dr, string lookupTable, string displayMember, int defaultValueMember)
        {
            int valueMember = -1;

            // Calculate a 'likely unique displayMember' string based on other fields concatenated to the
            // supplied displayMember (these concatenated fields are any FKEY fields in the LUT that have
            // matching fields in the dr parent table)...
            List<string> uniqueFKEYS = new List<string>();
            string uniqueDisplayMember = "";
            // First get the LUT schema...
            if (dr != null)
            {
                DataTable localDBLookupTable = _localData.GetData("SELECT * FROM " + lookupTable + " WHERE value_member=@valuemember", new string[1] { "@valuemember=-1" });
                // Now iterate through each column in the LUT to see if there is a matching FKEY in the LUT and dr parent table...
                foreach (DataColumn dc in localDBLookupTable.Columns)
                {
                    if (dr.Table.Columns.Contains(dc.ColumnName))
                    {
                        uniqueFKEYS.Add(dc.ColumnName);
                        uniqueDisplayMember += dr[dc.ColumnName].ToString();
                    }
                }
            }
            uniqueDisplayMember += displayMember;


            // Look in the memory cache for a matching RLUT - the RLUT has the unique friendly name as the key
            // and the PKEY as the value 
            // (if the unique friendly name is not there look in the local DB LUT)...
            if (_pkeyRLUTCacheCollection.ContainsKey(lookupTable) &&
                _pkeyRLUTCacheCollection[lookupTable].ContainsKey(uniqueDisplayMember))
            {
                var values = from RLUT in _pkeyRLUTCacheCollection[lookupTable]
                           where RLUT.Key == uniqueDisplayMember
                           select RLUT.Value;

                if (values.Count() == 1) valueMember = values.First();
            }
            else if (!_optimizeLUTForSpeed)
            {
                if (IsUpdated(lookupTable)) valueMember = GetLocalDBValue(dr, uniqueDisplayMember, uniqueFKEYS, lookupTable, displayMember);
                if (valueMember == -1) valueMember = defaultValueMember;
            }

            return valueMember;
        }

        private int GetLocalDBValue(DataRow drSource, string uniqueDisplayMember, List<string> uniqueFKEYS, string lookupTable, string displayMember)
        {
            int valueMember = -1;
            if (_localData.TableExists(lookupTable))
            {
                DataTable localDBLookupTable = _localData.GetData("SELECT * FROM " + lookupTable + " WHERE display_member=@displaymember", new string[1] { "@displaymember=" + displayMember });
//// If no records were returned from the query, update the local LU table with (potentially) new records from the remote DB...
//if (localDBLookupTable.Rows.Count <= 0)
//{
//    // First update the table...
//    UpdateTable(lookupTable);
//    // Retry the local DB lookup...
//    localDBLookupTable = _localData.GetData("SELECT * FROM " + lookupTable + " WHERE display_member=@displaymember", new string[1] { "@displaymember=" + displayMember });
//}
                // If more than one record was returned using only the display_member to resolve 
                // we need to filter the results using FKEYS in the LUT...
                if (localDBLookupTable.Rows.Count > 1)
                {
                    string sqlQuery = "SELECT * FROM " + lookupTable + " WHERE display_member=@displaymember";
                    string[] sqlParams = new string[uniqueFKEYS.Count + 1];
                    sqlParams[0] = "@displaymember=" + displayMember;
                    for (int i = 0; i < uniqueFKEYS.Count; i++)
                    {
                        sqlQuery += " AND " + uniqueFKEYS[i] + "=@" + uniqueFKEYS[i].Replace("_", "");
                        sqlParams[i + 1] = "@" + uniqueFKEYS[i].Replace("_", "") + "=" + drSource[uniqueFKEYS[i]].ToString();
                    }

                    localDBLookupTable = _localData.GetData(sqlQuery, sqlParams);
                }

                if (localDBLookupTable.Rows.Count == 1)
                {
                    if (_pkeyRLUTCacheCollection.ContainsKey(lookupTable))
                    {
                        // If the PKey LUT Display Dictionary exists - update or insert the records found from the local DB lookup table...
                        foreach (DataRow dr in localDBLookupTable.Rows)
                        {
                            if (_pkeyRLUTCacheCollection[lookupTable].ContainsKey(uniqueDisplayMember))
                            {
                                // If the PKey exists in the dictionary - update it...
                                _pkeyRLUTCacheCollection[lookupTable][uniqueDisplayMember] = (int)dr["value_member"];
                            }
                            else
                            {
                                // If the PKey does not exist in the dictionary - add it...
                                _pkeyRLUTCacheCollection[lookupTable].Add(uniqueDisplayMember, (int)dr["value_member"]);
                            }
                        }
                    }
                    else
                    {
                        // If the PKey LUT Display Dictionary doesn't exist - create a new one and add the records found from the local DB lookup table...
                        Dictionary<string, int> newDictionary = new Dictionary<string, int>();
                        foreach (DataRow dr in localDBLookupTable.Rows)
                        {
                            newDictionary.Add(uniqueDisplayMember, (int)dr["value_member"]);
                        }
                        // Now add the new Dictionary to the PKeyLUTCacheCollection...
                        _pkeyRLUTCacheCollection.Add(lookupTable, newDictionary);
                    }

                    // Since the missing row(s) have just been added (in the code above) - populate the valueMember with the first row in the returned records...
                    valueMember = (int)localDBLookupTable.Rows[0]["value_member"];
                }
                else
                {
                    valueMember = -1;
                }
            }
            return valueMember;
        }

        public DataTable GetCodeValueDataTableByGroupName(string groupName)
        {
            // Use LINQ to create a new dictionary<string, string> by filtering for 'groupName' codes in the code_value_lookup dictionary...
            var codeValueGroupList = from k in _codevalueLUTCache
                                     where k.Key.StartsWith(groupName)
                                     select k;
            // Create a new empty DataTable...
            DataTable dt = new DataTable();
            dt.Columns.Add("value_member", typeof(string));
            dt.Columns.Add("display_member", typeof(string));
            // Iterate through the dictionary and populate the new DataTable...
            foreach (var k in codeValueGroupList)
            {
                DataRow newRow = dt.NewRow();
                newRow["value_member"] = k.Key.Replace(groupName, "");
                newRow["display_member"] = k.Value;
                dt.Rows.Add(newRow);                
            }
            dt.AcceptChanges();
            // Return the new table...
            return dt;
        }

        public string GetCodeValueDisplayMember(string valueMember, string defaultDisplayMember)
        {
            string displayMember = "!Error!";

            // Look in the memory cache for code_value (if it is not there use default)...
            if (_codevalueLUTCache.ContainsKey(valueMember)) displayMember = _codevalueLUTCache[valueMember];
            else displayMember = defaultDisplayMember;

            return displayMember;
        }

        public string GetPKeyDisplayMember(string lookupTable, int valueMember, string defaultDisplayMember)
        {
            string displayMember = "!Error!";

            // Look in the memory cache for a matching LUT and PKey (if it is not there look in the local DB LUT)...
            if (_pkeyLUTCacheCollection.ContainsKey(lookupTable) &&
                _pkeyLUTCacheCollection[lookupTable].ContainsKey(valueMember))
            {
                displayMember = _pkeyLUTCacheCollection[lookupTable][valueMember];
            }
            else if (!_optimizeLUTForSpeed)
            {
                if (IsUpdated(lookupTable)) displayMember = GetLocalDBDisplay(lookupTable, valueMember);
                if (string.IsNullOrEmpty(displayMember)) displayMember = defaultDisplayMember;
            }

            return displayMember;
        }

        private string GetLocalDBDisplay(string lookupTable, int valueMember)
        {
            string displayMember = "";
            if (_localData.TableExists(lookupTable))
            {
                DataTable localDBLookupTable = _localData.GetData("SELECT * FROM " + lookupTable + " WHERE value_member=@valuemember", new string[1] { "@valuemember=" + valueMember });

                // If no records were returned from the query, update the local LU table with (potentially) new records from the remote DB...
                if (localDBLookupTable.Rows.Count <= 0)
                {
                    // First update the table...
                    UpdateTable(lookupTable);
                    // If the lookup table was successfully updated, retry the local DB lookup...
                    if (IsUpdated(lookupTable))
                    {
                        localDBLookupTable = _localData.GetData("SELECT * FROM " + lookupTable + " WHERE value_member=@valuemember", new string[1] { "@valuemember=" + valueMember });
                    }
                }

                if (localDBLookupTable.Rows.Count == 1)
                {
                    //if (localDBLookupTable.Columns["value_member"].DataType == typeof(int))
                    //{
                        // The value_member field is an integer type so this goes in the _pkeyLUTCacheCollection (a collection of dictionary<int, string>...
                        // Check to see if the PKey LUT Display Dictionary exists...
                        if (_pkeyLUTCacheCollection.ContainsKey(lookupTable))
                        {
                            // If the PKey LUT Display Dictionary exists - update or insert the records found from the local DB lookup table...
                            foreach (DataRow dr in localDBLookupTable.Rows)
                            {
                                if (_pkeyLUTCacheCollection[lookupTable].ContainsKey((int)dr["value_member"]))
                                {
                                    // If the PKey exists in the dictionary - update it...
                                    _pkeyLUTCacheCollection[lookupTable][(int)dr["value_member"]] = dr["display_member"].ToString();
                                }
                                else
                                {
                                    // If the PKey does not exist in the dictionary - add it...
                                    _pkeyLUTCacheCollection[lookupTable].Add((int)dr["value_member"], dr["display_member"].ToString());
                                }
                            }
                        }
                        else
                        {
                            // If the PKey LUT Display Dictionary doesn't exist - create a new one and add the records found from the local DB lookup table...
                            Dictionary<int, string> newDictionary = new Dictionary<int, string>();
                            foreach (DataRow dr in localDBLookupTable.Rows)
                            {
                                newDictionary.Add((int)dr["value_member"], dr["display_member"].ToString());
                            }
                            // Now add the new Dictionary to the PKeyLUTCacheCollection...
                            _pkeyLUTCacheCollection.Add(lookupTable, newDictionary);
                        }
                        // Since the missing row(s) have just been added (in the code above) - populate the displayMember...
                        displayMember = _pkeyLUTCacheCollection[lookupTable][valueMember];
                    //}
                    //else
                    //{
                    //    // The value_member is not integer so we will put it in the _codevalueLUTCache (a dictionary<string, string>)...
                    //    // Update or insert the records found from the local DB lookup table...
                    //    foreach (DataRow dr in localDBLookupTable.Rows)
                    //    {
                    //        if (_codevalueLUTCache.ContainsKey(dr["value_member"].ToString()))
                    //        {
                    //            // If the valueMember string exists in the dictionary - update it...
                    //            _codevalueLUTCache[dr["value_member"].ToString()] = dr["display_member"].ToString();
                    //        }
                    //        else
                    //        {
                    //            // If the valueMember string does not exist in the dictionary - add it...
                    //            _codevalueLUTCache.Add(dr["value_member"].ToString(), dr["display_member"].ToString());
                    //        }
                    //    }
                    //    // Since the missing row(s) have just been added (in the code above) - populate the displayMember...
                    //    displayMember = _codevalueLUTCache[valueMember];
                    //}
                }
                else
                {
                    displayMember = "";
                }
            }
            return displayMember;
        }

        public void UpdateTable(object objDataviewName)
        {
            string dataviewName = (string)objDataviewName;
            bool updateStatus = false;
            DataTable lookupTableStatus = new DataTable("lookup_table_status");
            DataRow lookupTableStatusRow = null;

            if (_localData.TableExists("lookup_table_status"))
            {
                lookupTableStatus = _localData.GetData("SELECT * FROM lookup_table_status WHERE dataview_name=@dataviewname", new string[1] { "@dataviewname=" + dataviewName });
            }

            if (lookupTableStatus != null &&
                lookupTableStatus.Rows.Count > 0)
            {
                lookupTableStatusRow = lookupTableStatus.Rows[0];
            }

            if (lookupTableStatusRow != null &&
                (lookupTableStatusRow["status"].ToString().Trim().ToUpper() == "COMPLETED" ||
                lookupTableStatusRow["status"].ToString().Trim().ToUpper() == "UPDATED"))
            {
                // Use the last sync date for the remote database query...
                DateTime lastSyncDate = new DateTime();
                lastSyncDate = Convert.ToDateTime(lookupTableStatus.Rows[0]["sync_date"].ToString());

                if (lastSyncDate.AddSeconds(10) < DateTime.UtcNow)
                {
                    int pageSize = 1000;
                    string selectParams = ":createddate=" + lastSyncDate + "; :modifieddate=" + lastSyncDate + "; :valuemember=; :startpkey=; :stoppkey=; :displaymember;";

                    // Go get the records from the remote database that are newer than the lastSyncDate...
                    DataSet tempSync = _webServices.GetData(dataviewName, selectParams, 0, pageSize);
                    if (tempSync.Tables.Contains(dataviewName))
                    {
                        DataTable syncTable = tempSync.Tables[dataviewName].Clone();
                        //// Apply any column constraints (indicated in the column extended properties) to the table...
                        //ApplyColumnConstraints(syncTable);
                        syncTable.Rows.Clear();
                        syncTable.AcceptChanges();
                        syncTable.Load(tempSync.Tables[dataviewName].CreateDataReader(), LoadOption.Upsert);
                        if (syncTable.Rows.Count == 0)
                        {
                            lookupTableStatusRow["current_pk"] = lookupTableStatusRow["max_pk"];
                            lookupTableStatusRow["status"] = "Updated";
                            lookupTableStatusRow["sync_date"] = DateTime.UtcNow;
                            updateStatus = true;
                        }
                        else if (syncTable.Rows.Count > 0 &&
                            syncTable.Rows.Count < pageSize)
                        {
                            // Check to make sure the user wants to update this table...
                            if (lookupTableStatusRow["auto_update"].ToString().Trim().ToUpper() == "Y")
                            {
                                // Save the new lookup table rows to the local database...
                                SaveDataPageToLocalDB(syncTable);
                                // Update the dictionary if one is being used...
                                if (_pkeyLUTCacheCollection.ContainsKey(dataviewName))
                                {
                                    UpdatePKeyLUTDictionary(dataviewName, _pkeyLUTCacheCollection[dataviewName]);
                                }
                                else if (dataviewName.ToLower().Trim() == "code_value_lookup")
                                {
                                    UpdateCodeValueLUTDictionary("code_value_lookup", _codevalueLUTCache);
                                }

                                // Update the stats for this lookup table in the lookup_table_status table on the local database...
                                lookupTableStatusRow["current_pk"] = lookupTableStatusRow["max_pk"];
                                lookupTableStatusRow["status"] = "Updated";
                                lookupTableStatusRow["sync_date"] = DateTime.UtcNow;
                                updateStatus = true;
                            }
                            else
                            {
                                // If the user does not want to update this table but there are less than 1000 new records that need to be downloaded
                                // mark its status as 'PARTIAL' so that the LUT maint. dialog displays warnings properly
                                Int64 max = pageSize + 1;
                                Int64 current = 0;
                                if (Int64.TryParse(lookupTableStatusRow["max_pk"].ToString(), out max) &&
                                    Int64.TryParse(lookupTableStatusRow["current_pk"].ToString(), out current) &&
                                    (max - current) > pageSize)
                                {
                                    lookupTableStatusRow["status"] = "Partial";
                                }
                                updateStatus = false;
                            }
                        }
                        else
                        {
                            lookupTableStatusRow["status"] = "Failed";
                            lookupTableStatusRow["sync_date"] = DateTime.UtcNow;
                            updateStatus = true;
                        }
                        SaveDataPageToLocalDB(lookupTableStatus);
                    }
                    else
                    {
                        // No dataview named 'XXX' exists in the sys_dataview table.
                        // Timeout expired.  The timeout period elapsed prior to completion of the operation or the server is not responding.
                        if (tempSync.Tables.Contains("ExceptionTable") &&
                            tempSync.Tables["ExceptionTable"].Rows.Count > 0 &&
                            !tempSync.Tables["ExceptionTable"].Rows[0]["Message"].ToString().Contains("Timeout expired"))
                        {
                            // There appears to be a problem getting data for this lookup table (most likely the LU dataview is missing/broken)...
                            lookupTableStatusRow.Delete();
                            SaveDataPageToLocalDB(lookupTableStatus);
                            if (_localData.TableExists(dataviewName)) _localData.Remove(dataviewName);
                            updateStatus = true;
                        }
                    }
                }
                else
                {
                    // This table was successfully updated in the last 10 seconds so 
                    // skip the update and return successful status...
                    updateStatus = true;
                }
            }
            else
            {
//System.Windows.Forms.MessageBox.Show("WARNING!\nThe system can not perform Update for '" + dataviewName + "' until the table has been successfully downloaded", "Partially Loaded Lookup Table", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
SharedUtils sharedUtils = new SharedUtils(_webServices.Url, _webServices.Username, _webServices.Password_ClearText, true);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("WARNING!\nThe system can not perform Update for '{0}' until the table has been successfully downloaded.\n\nWould you like to do this now?", "Partially Loaded Lookup Table", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "LookupTables_LoadTableFromDatabaseMessage3";
if (sharedUtils != null && sharedUtils.IsConnected) sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewName.ToUpper());
//ggMessageBox.ShowDialog();
if (System.Windows.Forms.DialogResult.Yes == ggMessageBox.ShowDialog())
{
// Start the LUT maintenance dialog...
//LookupTableLoader ltl = new LookupTableLoader(localDBInstance, _sharedUtils);
LookupTableLoader ltl = new LookupTableLoader(_localData.ConnectionString, sharedUtils);
ltl.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
ltl.ShowDialog();
}
updateStatus = false;
            }

            // Clear the lookup table status cache (so that it can be refreshed for each lookup table on demand)...
            _lookupTableStatus.Clear();

//return updateStatus;
        }

        public DataTable LookupTableGetMatchingRows(string dataviewName, string displayMemberLikeFilter, int maxReturnRows)
        {
            DataTable filteredRows = new DataTable();
            if (_localData.TableExists(dataviewName))
            {
                if (System.Text.RegularExpressions.Regex.Match(displayMemberLikeFilter, @"\s*(?:(?:not\s+|NOT\s+)*(?:like\s+|LIKE\s+))\s*\S+\s*").Success)
                {
                    string searchOperator = "LIKE";
                    string searchValue = displayMemberLikeFilter;
                    searchOperator = System.Text.RegularExpressions.Regex.Match(displayMemberLikeFilter, @"\s*(?:(?:not\s+|NOT\s+)*(?:like\s+|LIKE\s+))\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value.Trim().ToUpper();

                    string[] splitValues = System.Text.RegularExpressions.Regex.Split(displayMemberLikeFilter, @"\s*(?:(?:not\s+|NOT\s+)*(?:like\s+|LIKE\s+))\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (splitValues != null &&
                        splitValues.Length > 0)
                    {
                        searchValue = splitValues[splitValues.Length - 1].Trim();
                    }
                    filteredRows = _localData.GetData("SELECT * FROM " + dataviewName + " WHERE display_member " + searchOperator + " @displaymember + '%'", new string[1] { "@displaymember=" + searchValue });
                }
                else
                {
                    filteredRows = _localData.GetData("SELECT * FROM " + dataviewName + " WHERE display_member like @displaymember + '%'", new string[1] { "@displaymember=" + displayMemberLikeFilter });
                }
            }
            else
            {
                string todayPlusOneYear = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd hh:mm:ss");
                string selectParameters = ":createddate=" + todayPlusOneYear + "; :modifieddate=" + todayPlusOneYear + "; :valuemember=; :startpkey=; :stoppkey=; :displaymember=" + displayMemberLikeFilter + "%";
                DataSet dataPage = _webServices.GetData(dataviewName, selectParameters, 0, maxReturnRows);
                if (dataPage.Tables.Contains(dataviewName))
                {
                    filteredRows = dataPage.Tables[dataviewName].Copy();
                }
            }
            return filteredRows;
        }

        public DataTable GetSynchronizationStats()
        {
            DataTable remoteLUTableStats = new DataTable();
            remoteLUTableStats.Columns.Add("dataview_name", typeof(string));
            remoteLUTableStats.Columns["dataview_name"].ExtendedProperties.Add("is_primary_key", "Y");
            remoteLUTableStats.Columns.Add("pk_field_name", typeof(string));
            remoteLUTableStats.Columns.Add("table_name", typeof(string));
            remoteLUTableStats.Columns.Add("title", typeof(string));
            remoteLUTableStats.Columns.Add("description", typeof(string));
            remoteLUTableStats.Columns.Add("current_pk", typeof(int));
            remoteLUTableStats.Columns.Add("min_pk", typeof(int));
            remoteLUTableStats.Columns.Add("max_pk", typeof(int));
            remoteLUTableStats.Columns.Add("row_count", typeof(int));
            remoteLUTableStats.Columns.Add("status", typeof(string));
            remoteLUTableStats.Columns.Add("sync_date", typeof(DateTime));

            DataSet remoteLUList = _webServices.GetData("get_lookup_table_list", "", 0, 0);
            DataSet remoteLUStats = new DataSet();

            // Build the local lookup_table_status table dataview by dataview (because of the way the get_lookup_table_stats dataview must be called)...
            if (remoteLUList.Tables.Contains("get_lookup_table_list"))
            {
                foreach (DataRow dr in remoteLUList.Tables["get_lookup_table_list"].Rows)
                {
                    string dataviewParams = ":tablename=" + dr["table_name"].ToString() + "; :pkfieldname=" + dr["pk_field_name"].ToString() + ";";
                    remoteLUStats = _webServices.GetData("get_lookup_table_stats", dataviewParams, 0, 0);
                    if (remoteLUStats.Tables.Contains("get_lookup_table_stats") &&
                        remoteLUStats.Tables["get_lookup_table_stats"].Rows.Count > 0)
                    {
                        DataRow newRemoteLUTableStatsRow = remoteLUTableStats.NewRow();
                        newRemoteLUTableStatsRow["dataview_name"] = dr["dataview_name"];
                        newRemoteLUTableStatsRow["pk_field_name"] = remoteLUStats.Tables["get_lookup_table_stats"].Rows[0]["pk_field_name"]; //.ToString();
                        newRemoteLUTableStatsRow["table_name"] = remoteLUStats.Tables["get_lookup_table_stats"].Rows[0]["table_name"]; //.ToString();
                        newRemoteLUTableStatsRow["title"] = dr["title"];
                        newRemoteLUTableStatsRow["description"] = dr["description"];
                        newRemoteLUTableStatsRow["current_pk"] = remoteLUStats.Tables["get_lookup_table_stats"].Rows[0]["min_pk"]; //.ToString();
                        newRemoteLUTableStatsRow["min_pk"] = remoteLUStats.Tables["get_lookup_table_stats"].Rows[0]["min_pk"]; //.ToString();
                        newRemoteLUTableStatsRow["max_pk"] = remoteLUStats.Tables["get_lookup_table_stats"].Rows[0]["max_pk"]; //.ToString();
                        newRemoteLUTableStatsRow["row_count"] = remoteLUStats.Tables["get_lookup_table_stats"].Rows[0]["row_count"]; //.ToString();
                        remoteLUTableStats.Rows.Add(newRemoteLUTableStatsRow);
                    }
                }
            }

            // Now sync the remote data with the local copy of the lookup_table_status table...
            // First get a copy of the local lookup_table_status table...
            DataTable localLUTableStats = null;
            if (_localData.TableExists("lookup_table_status"))
            {
                //localLUTableStats = _localData.GetData("SELECT * FROM lookup_table_status");
                localLUTableStats = _localData.GetData("SELECT * FROM lookup_table_status", new string[0]);
                if (!localLUTableStats.Columns.Contains("dataview_name") ||
                    !localLUTableStats.Columns.Contains("pk_field_name") ||
                    !localLUTableStats.Columns.Contains("table_name") ||
                    !localLUTableStats.Columns.Contains("title") ||
                    !localLUTableStats.Columns.Contains("description") ||
                    !localLUTableStats.Columns.Contains("current_pk") ||
                    !localLUTableStats.Columns.Contains("min_pk") ||
                    !localLUTableStats.Columns.Contains("max_pk") ||
                    !localLUTableStats.Columns.Contains("row_count") ||
                    !localLUTableStats.Columns.Contains("auto_update") ||
                    !localLUTableStats.Columns.Contains("status") ||
                    !localLUTableStats.Columns.Contains("sync_date") ||
                    (localLUTableStats.PrimaryKey.Length == 1 && localLUTableStats.PrimaryKey[0].ColumnName != "dataview_name"))
                {
                    _localData.Remove("lookup_table_status");
                    localLUTableStats = null;
                }
            }
            // If the local lookup_table_status table is not there or is out of date - (re)build it...
            if (localLUTableStats == null)
            {
                localLUTableStats = new DataTable("lookup_table_status");
                // Create the empty table...
                localLUTableStats.Columns.Add("dataview_name", typeof(string));
                localLUTableStats.Columns["dataview_name"].ExtendedProperties.Add("is_primary_key", "Y");
                localLUTableStats.Columns.Add("pk_field_name", typeof(string));
                localLUTableStats.Columns.Add("table_name", typeof(string));
                localLUTableStats.Columns.Add("title", typeof(string));
                localLUTableStats.Columns.Add("description", typeof(string));
                localLUTableStats.Columns.Add("current_pk", typeof(int));
                localLUTableStats.Columns.Add("min_pk", typeof(int));
                localLUTableStats.Columns.Add("max_pk", typeof(int));
                localLUTableStats.Columns.Add("row_count", typeof(int));
                localLUTableStats.Columns.Add("auto_update", typeof(string));
                localLUTableStats.Columns.Add("status", typeof(string));
                localLUTableStats.Columns.Add("sync_date", typeof(DateTime));
                _localData.CreateTable(localLUTableStats, false, true);
                // Go get the table from the localData (because it will have the PKey properly defined)...
                localLUTableStats = _localData.GetData("SELECT * FROM lookup_table_status", new string[0]);
            }
            // Now iterate through each row in the remote copy of the lookup_table_stats dataview...
            foreach (DataRow remoteLookupTableStatusRow in remoteLUTableStats.Rows)
            {
                DataRow localLUTableStatsRow = localLUTableStats.Rows.Find(remoteLookupTableStatusRow["dataview_name"].ToString());
                if (localLUTableStatsRow != null)
                {
                    // Update the fields returned from the remote server on the local copy of the Lookup Table Status...
                    localLUTableStatsRow["pk_field_name"] = remoteLookupTableStatusRow["pk_field_name"];
                    localLUTableStatsRow["table_name"] = remoteLookupTableStatusRow["table_name"];
                    localLUTableStatsRow["title"] = remoteLookupTableStatusRow["title"];
                    localLUTableStatsRow["description"] = remoteLookupTableStatusRow["description"];
                    int remoteINT = 0;

                    if (int.TryParse(remoteLookupTableStatusRow["min_pk"].ToString(), out remoteINT))
                    {
                        localLUTableStatsRow["min_pk"] = remoteLookupTableStatusRow["min_pk"];
                    }
                    else
                    {
                        localLUTableStatsRow["min_pk"] = remoteINT;
                    }
                    remoteINT = 0;
                    if (int.TryParse(remoteLookupTableStatusRow["max_pk"].ToString(), out remoteINT))
                    {
                        localLUTableStatsRow["max_pk"] = remoteLookupTableStatusRow["max_pk"];
                    }
                    else
                    {
                        localLUTableStatsRow["max_pk"] = remoteINT;
                    }
                    remoteINT = 0;
                    if (int.TryParse(remoteLookupTableStatusRow["row_count"].ToString(), out remoteINT))
                    {
                        localLUTableStatsRow["row_count"] = remoteLookupTableStatusRow["row_count"];
                    }
                    else
                    {
                        localLUTableStatsRow["row_count"] = remoteINT;
                    }
                    // If the locally saved current_pk value is null set it to the min_pk value...
                    if (!int.TryParse(localLUTableStatsRow["current_pk"].ToString(), out remoteINT))
                    {
                        localLUTableStatsRow["current_pk"] = localLUTableStatsRow["min_pk"];
                    }

                    // If the table was not completely loaded yet - set the status to Partial so that the Loader can pick up where it left off...
                    if (localLUTableStatsRow["status"].ToString().Trim().ToUpper() == "LOADING")
                    {
                        localLUTableStatsRow["status"] = "Partial";
                    }
                }
                else
                {
                    // No row was found in lookup_table_status for the current table being processed - so create an empty one...
                    localLUTableStatsRow = localLUTableStats.NewRow();
                    localLUTableStatsRow["dataview_name"] = remoteLookupTableStatusRow["dataview_name"];
                    localLUTableStatsRow["pk_field_name"] = remoteLookupTableStatusRow["pk_field_name"];
                    localLUTableStatsRow["table_name"] = remoteLookupTableStatusRow["table_name"];
                    localLUTableStatsRow["title"] = remoteLookupTableStatusRow["title"];
                    localLUTableStatsRow["description"] = remoteLookupTableStatusRow["description"];
                    localLUTableStatsRow["current_pk"] = remoteLookupTableStatusRow["min_pk"];
                    localLUTableStatsRow["min_pk"] = remoteLookupTableStatusRow["min_pk"];
                    localLUTableStatsRow["max_pk"] = remoteLookupTableStatusRow["max_pk"];
                    localLUTableStatsRow["row_count"] = remoteLookupTableStatusRow["row_count"];
                    localLUTableStatsRow["auto_update"] = "Y";
                    if (localLUTableStatsRow["row_count"].ToString().Trim() == "0")
                    {
                        localLUTableStatsRow["status"] = "Completed";
                    }
                    else
                    {
                        localLUTableStatsRow["status"] = "Empty";
                    }
                    localLUTableStatsRow["sync_date"] = DateTime.UtcNow;
                    localLUTableStats.Rows.Add(localLUTableStatsRow);
                }
            }
            // Now save the updates to the local copy of the lookup_table_status table...
            SaveDataPageToLocalDB(localLUTableStats);

            return localLUTableStats;
        }
    }
}
