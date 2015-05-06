using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Diagnostics;
using System.Web;
using GrinGlobal.Interface;
using System.Reflection;
using GrinGlobal.Interface.DataTriggers;


namespace GrinGlobal.Interface.Dataviews {
//	[DebuggerStepThrough()]
	public interface IDataview {


        IDataview Clone();

		List<ITable> Tables { get; }
        List<string> TableNames { get; }

		List<IDataviewSaveDataTrigger> SaveDataTriggers { get; set; }
        List<IDataviewReadDataTrigger> ReadDataTriggers { get; set; }

		ITable GetParentTable(IField fm) ;


        Dictionary<string, object> ExtendedProperties { get; }

        bool IsEnabled { get; set; }
        bool IsReadOnly { get; set; }
        bool IsTransform { get; set; }

        string Title { get; set; }
        string Description { get; set; }
        string CategoryCode { get; set; }
        string DatabaseAreaCode { get; set; }
        int DatabaseAreaSortOrder { get; set; }
        string[] TransformByFields { get; set; }
        string TransformFieldForNames { get; set; }
        string TransformFieldForCaptions { get; set; }
        string TransformFieldForValues { get; set; }
        string ConfigurationOptions { get; set; }

        string SqlStatementForCurrentEngine { get; }

        string PrimaryKeyTableName { get; set; }
        List<string> PrimaryKeyNames { get; set; }

		List<IField> Mappings { get; }

		List<IDataviewParameter> Parameters { get; }

        Dictionary<string, string> SqlStatements { get; }

        string DataViewName { get; set; }

        DataColumn[] GetPrimaryKeyColumns(DataTable dt);

        IField GetField(int tableFieldID, string aliasName);

        /// <summary>
        /// Finds value from helper.RowToSave based on dataview field mapping information and extendedproperties
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        int? FindIntValue(DataTriggerHelper helper, string columnName);
    }
}
