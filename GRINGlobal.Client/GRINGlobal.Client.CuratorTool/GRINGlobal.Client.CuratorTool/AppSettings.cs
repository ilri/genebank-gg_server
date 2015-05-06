using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GRINGlobal.Client.Common;

namespace GRINGlobal.Client
{
    class AppSettings
    {
        private DataTable _appSettings;
        //private WebServices _GRINGlobalWebServices;
        private SharedUtils _sharedUtils;
        private int _lang = 1;

        //public AppSettings(WebServices GRINGlobalWebServices, int lang)
        public AppSettings(SharedUtils sharedUtils, int lang)
        {
            _appSettings = new DataTable();
            //_GRINGlobalWebServices = GRINGlobalWebServices;
            _sharedUtils = sharedUtils;
            _lang = lang;
        }

        public void Load()
        {
            DataSet remoteDBUserSettings = new DataSet();
            // Get the user settings from the remote DB for the current user's CNO...
            //remoteDBUserSettings = _GRINGlobalWebServices.GetData("GET_RESOURCES", ":name=; :seclangid=" + _lang.ToString(), 0, 0);
            remoteDBUserSettings = _sharedUtils.GetWebServiceData("GET_RESOURCES", ":name=; :seclangid=" + _lang.ToString(), 0, 0);
            _appSettings.Clear();
            _appSettings = remoteDBUserSettings.Tables["GET_RESOURCES"].Copy();
            // Set the primary key for the table from the extended properties...
            System.Collections.Generic.List<DataColumn> pKeys = new System.Collections.Generic.List<DataColumn>();
            foreach (DataColumn dc in _appSettings.Columns)
            {
                // Add the column to the primary key list if it is a primary key for the resultset...
                if (dc.ExtendedProperties.Contains("is_primary_key") &&
                    dc.ExtendedProperties["is_primary_key"].ToString() == "Y")
                {
                    pKeys.Add(dc);
                }
                // Set the autoincrement property indicated (typically for the primary key)...
                if (dc.ExtendedProperties.Contains("is_autoincrement") &&
                    dc.ExtendedProperties["is_autoincrement"].ToString() == "Y")
                {
                    dc.AutoIncrement = true;
                    dc.AutoIncrementSeed = -1;
                    dc.AutoIncrementStep = -1;
                }
            }
            _appSettings.PrimaryKey = pKeys.ToArray();

            // Clean up...
            remoteDBUserSettings.Dispose();
        }

        public void UpdateControls(System.Windows.Forms.Control.ControlCollection ctrlCollection)
        {
            // If this is the client window - change the title bar...
            if (ctrlCollection.Owner.GetType() == typeof(GRINGlobal.Client.CuratorTool.GRINGlobalClientCuratorTool))
            {
                //this.Text = "GRIN-Global  v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                DataRow[] title = _appSettings.Select("app_resource_name='GRINGlobalClient'");
                if (title != null && title.Length > 0)
                {
                    ctrlCollection.Owner.Text = title[0]["display_member"] + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                }
                else
                {
                    ctrlCollection.Owner.Text = "GRIN-Global  v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                }
            }

            // Iterate through the Forms controls...
            foreach (DataRow dr in _appSettings.Rows)
            {
                System.Windows.Forms.Control[] ctrlArray = ctrlCollection.Find(dr["app_resource_name"].ToString(), true);
                if (ctrlArray != null && ctrlArray.Length > 0)
                {
                    foreach (System.Windows.Forms.Control ctrl in ctrlArray)
                    {
                        ctrl.Text = dr["display_member"].ToString();
                    }
                }
            }
        }

        public void UpdateComponents(System.ComponentModel.ComponentCollection compCollection)
        {
            // Iterate through the Forms context menus...
            foreach (System.ComponentModel.Component comp in compCollection)
            {
                if (comp is System.Windows.Forms.ContextMenuStrip)
                {
                    System.Windows.Forms.ContextMenuStrip cms = (System.Windows.Forms.ContextMenuStrip)comp;
                    foreach (DataRow dr in _appSettings.Rows)
                    {
                        System.Windows.Forms.ToolStripItem[] tsiArray = cms.Items.Find(dr["app_resource_name"].ToString(), true);
                        if (tsiArray != null && tsiArray.Length > 0)
                        {
                            foreach (System.Windows.Forms.ToolStripItem tsi in tsiArray)
                            {
                                tsi.Text = dr["display_member"].ToString();
                            }
                        }
                    }
                }
            }
        }

    }
}
