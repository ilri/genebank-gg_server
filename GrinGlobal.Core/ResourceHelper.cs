using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;
using System.Data;
using System.IO;
using System.Xml.XPath;
using System.Threading;

namespace GrinGlobal.Core {
    public class ResourceHelper {

        private static bool _loadedFromDatabase;
        private static bool _loadedFromFile;

        public static void Initialize(DataConnectionSpec dcs) {
            init(dcs);
        }

        private static void init(DataConnectionSpec dcs) {

            if (_dtResources_a == null) {
                _dtResources_a = new DataTable();
                _dtResources_a.Columns.Add("ietf_tag", typeof(string));
                _dtResources_a.Columns.Add("app_name", typeof(string));
                _dtResources_a.Columns.Add("form_name", typeof(string));
                _dtResources_a.Columns.Add("resource_name", typeof(string));
                _dtResources_a.Columns.Add("value_member", typeof(string));
                _dtResources_a.Columns.Add("display_member", typeof(string));
                _dtResources_a.Columns.Add("description", typeof(string));
                _dtResources_a.Columns.Add("sort_order", typeof(int));
            }

            if (dcs != null && !_loadedFromDatabase) {
                try {
                    using (var dm = DataManager.Create(dcs)) {
                        var dt = dm.Read(@"
select 
    ar.app_name,
    ar.form_name,
    ar.app_resource_name,
    ar.description,
    ar.display_member,
    ar.value_member,
    ar.sort_order,
    sl.ietf_tag
from 
    app_resource ar 
    left join sys_lang sl 
        on ar.sys_lang_id = sl.sys_lang_id
");
                        foreach (DataRow dr in dt.Rows) {
                            _dtResources_a.Rows.Add(Toolkit.Coalesce(dr["ietf_tag"].ToString(), "en-US"), dr["app_name"], dr["form_name"], dr["app_resource_name"], dr["value_member"], dr["display_member"], dr["description"], dr["sort_order"]);
                        }
                        _loadedFromDatabase = true;
                    }
                } catch {
                    // eat all errors loading resources.
                }
            }

            if (!_loadedFromFile) {
                var path = Toolkit.ResolveFilePath("./resources.xml", false);
                if (File.Exists(path)) {
                    XPathNavigator nav = new XPathDocument(path).CreateNavigator();
                    var apps = nav.Select("/apps/app");
                    while (apps.MoveNext()) {
                        var app = apps.Current;
                        var appName = app.GetAttribute("Name", "");
                        var forms = app.Select("form");
                        while(forms.MoveNext()){
                            var form = forms.Current;
                            var formName = form.GetAttribute("Name", "");

                            var resources = form.Select("resource");
                            while(resources.MoveNext()){
                                var resource = resources.Current;
                                var resourceName = resource.GetAttribute("Name", "");
                                var sortOrder = Toolkit.ToInt32(resource.GetAttribute("SortOrder", ""), 0);

                                var languages = resource.Select("language");
                                while(languages.MoveNext()){
                                    var lang = languages.Current;
                                    var langName = lang.GetAttribute("Name", "");
                                    var value = lang.GetAttribute("ValueMember", "");
                                    var display = lang.GetAttribute("DisplayMember", "");
                                    var description = lang.GetAttribute("Description", "");
                                    _dtResources_a.Rows.Add(langName, appName, formName, resourceName, value, display, description, sortOrder);
                                }
                            }
                        }
                    }
                    _loadedFromFile = true;
                }
            }
        }

        private static DataTable _dtResources;
        private static DataTable _dtResources_a;

        public static string GetDisplayMember(DataConnectionSpec dcs, string appName, string formName, string resourceName, string language, string defaultValue, params string[] substitutes) {
            var rv = Get(dcs, appName, formName, resourceName, language, defaultValue, substitutes);
            return rv.DisplayMember;
        }

        /// <summary>
        /// Gets the resource text for the given inputs.  If language is null or empty, defaults to the current thread's CultureInfo.Name value (e.g. en-US).  NEVER returns null.
        /// </summary>
        /// <param name="dcs"></param>
        /// <param name="appName"></param>
        /// <param name="formName"></param>
        /// <param name="resourceName"></param>
        /// <param name="language"></param>
        /// <param name="defaultValue"></param>
        /// <param name="substitutes"></param>
        /// <returns></returns>
        public static ResourceValue Get(DataConnectionSpec dcs, string appName, string formName, string resourceName, string language, string defaultValue, params string[] substitutes) {
            if (String.IsNullOrEmpty(language)) {
                // no language given, default to the current thread's culture (.Name returns stuff like "en-US")
                language = Thread.CurrentThread.CurrentCulture.Name;
            }
            init(dcs);
            var rvs = GetList(dcs, appName, formName, resourceName, language, substitutes);
            if (rvs.Count > 0) {
                return rvs[0];
            } else {
                var val = String.Format(defaultValue, substitutes);
                var rv = new ResourceValue { 
                    ValueMember = val, 
                    DisplayMember = val,
                    Description = val,
                    SortOrder = 0
                };
                return rv;
            }
        }

        public static List<ResourceValue> GetList(DataConnectionSpec dcs, string appName, string formName, string resourceName, string language, params string[] substitutes) {

            var rvs = new List<ResourceValue>();
            
            init(dcs);
            var drs = _dtResources_a.Select("app_name = '" + appName + "' and (form_name = '" + formName + "' or form_name = 'all') and resource_name = '" + resourceName + "' and ietf_tag = '" + language + "'", "sort_order asc");
            if (drs != null && drs.Length > 0) {
                foreach (DataRow dr in drs) {
                    var rv = new ResourceValue();
                    rv.ValueMember = String.Format(dr["value_member"].ToString(), substitutes);
                    rv.DisplayMember = String.Format(dr["display_member"].ToString(), substitutes);
                    rv.Description = String.Format(dr["description"].ToString(), substitutes);
                    rv.SortOrder = Toolkit.ToInt32(dr["sort_order"], 0);
                    rvs.Add(rv);
                }
            }
            return rvs;
        }

        public static string GetDisplayMember(DataConnectionSpec dcs, string appName, string formName, string resourceName, int languageId, string defaultValue, params string[] substitutes)
        {
            var rv = Get(dcs, appName, formName, resourceName, languageId, defaultValue, substitutes);
            return rv.DisplayMember;
        }

        public static ResourceValue Get(DataConnectionSpec dcs, string appName, string formName, string resourceName, int languageId, string defaultValue, params string[] substitutes)
        {
            if (languageId == 0)
            {
                string language = Thread.CurrentThread.CurrentCulture.Name;
                return Get(null, appName, formName, resourceName, language, defaultValue, substitutes);
            }
            else
            {
                init(dcs, languageId, appName);
                var rvs = GetList(dcs, appName, formName, resourceName, languageId, substitutes);
                if (rvs.Count > 0)
                {
                    return rvs[0];
                }
                else
                {
                    var val = String.Format(defaultValue, substitutes);
                    var rv = new ResourceValue
                    {
                        ValueMember = val,
                        DisplayMember = val,
                        Description = val,
                        SortOrder = 0
                    };
                    return rv;
                }
            }
        }

        public static List<ResourceValue> GetList(DataConnectionSpec dcs, string appName, string formName, string resourceName, int languageId, params string[] substitutes)
        {

            var rvs = new List<ResourceValue>();

            init(dcs, languageId, appName);
            var drs = _dtResources.Select("app_name = '" + appName + "' and (form_name = '" + formName + "' or form_name = 'all') and resource_name = '" + resourceName + "' and sys_lang_id = '" + languageId + "'", "sort_order asc");
            if (drs != null && drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    var rv = new ResourceValue();
                    rv.ValueMember = String.Format(dr["value_member"].ToString(), substitutes);
                    rv.DisplayMember = String.Format(dr["display_member"].ToString(), substitutes);
                    rv.Description = String.Format(dr["description"].ToString(), substitutes);
                    rv.SortOrder = Toolkit.ToInt32(dr["sort_order"], 0);
                    rvs.Add(rv);
                }
            }
            return rvs;
        }

        private static void init(DataConnectionSpec dcs, int languageId, string appName)
        {

            if (_dtResources == null)
            {
                _dtResources = new DataTable();
                _dtResources.Columns.Add("sys_lang_id", typeof(int));
                _dtResources.Columns.Add("app_name", typeof(string));
                _dtResources.Columns.Add("form_name", typeof(string));
                _dtResources.Columns.Add("resource_name", typeof(string));
                _dtResources.Columns.Add("value_member", typeof(string));
                _dtResources.Columns.Add("display_member", typeof(string));
                _dtResources.Columns.Add("description", typeof(string));
                _dtResources.Columns.Add("sort_order", typeof(int));
            }

            if (dcs != null && !_loadedFromDatabase)
            {
                try
                {
                    using (var dm = DataManager.Create(dcs))
                    {
                        var dt = dm.Read(@"
select 
    ar.app_name,
    ar.form_name,
    ar.app_resource_name,
    ar.description,
    ar.display_member,
    ar.value_member,
    ar.sort_order,
    ar.sys_lang_id
from 
    app_resource ar
where app_name = :appname and sys_lang_id = :syslangid",
new DataParameters(":appname", appName, DbType.String, 
    ":syslangid", languageId, DbType.Int32
));
                        foreach (DataRow dr in dt.Rows)
                        {
                            _dtResources.Rows.Add(dr["sys_lang_id"], dr["app_name"], dr["form_name"], dr["app_resource_name"], dr["value_member"], dr["display_member"], dr["description"], dr["sort_order"]);
                        }
                        _loadedFromDatabase = true;
                    }
                }
                catch
                {
                }
            }
        }
    }
}
