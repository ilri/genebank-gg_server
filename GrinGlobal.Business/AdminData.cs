using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;
using System.Data;
using GrinGlobal.Business.SearchSvc;
using System.IO;
using GrinGlobal.DatabaseInspector;
using System.Diagnostics;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml;
using GrinGlobal.Interface.DataTriggers;
using System.Runtime.InteropServices;
using GrinGlobal.Interface.Dataviews;

namespace GrinGlobal.Business {
    /// <summary>
    /// Used exclusively by the Admin Tool for making administrative changes to data in the sys_* tables.  Does not use dataviews at all.
    /// </summary>
    public class AdminData : SecureData {

        #region Constructors
        private AdminData(bool suppressExceptions) : base(suppressExceptions){
		}

		public AdminData(bool suppressExceptions, string loginToken) : base(suppressExceptions, loginToken) {
        }

        public AdminData(bool suppressExceptions, string loginToken, DataConnectionSpec dcs)
            : base(suppressExceptions, loginToken, dcs) {
        }
        #endregion

        #region File Group
        public DataSet CopyFileGroup(int groupID, string newVersionName) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {

                    checkUserHasAdminEnabled();

                    dm.BeginTran();
                    int newGroupID = dm.Write(@"
insert into sys_file_group
(group_name, version_name, is_enabled, created_date, created_by, owned_date, owned_by)
select
    group_name,
    :newgroupversion,
    'N',
    :now1,
    :coop1,
    :now2,
    :coop2
from
    sys_file_group
where
    sys_file_group_id = :groupid
", true, "sys_file_group_id", new DataParameters(
     ":newgroupversion", newVersionName,
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":coop1", this.CooperatorID, DbType.Int32,
     ":now2", DateTime.UtcNow, DbType.DateTime2,
     ":coop2", this.CooperatorID, DbType.Int32,
     ":groupid", groupID, DbType.Int32
     ));

                    dm.Write(@"
insert into sys_file_group_map
(sys_file_group_id, sys_file_id, created_date, created_by, owned_date, owned_by)
select
    sys_file_group_id,
    sys_file_id,
    :now1,
    :coop1,
    :now2,
    :coop2
from
    sys_file_group_map
where
    sys_file_group_id = :groupid
", new DataParameters(
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":coop1", this.CooperatorID, DbType.Int32,
     ":now2", DateTime.UtcNow, DbType.DateTime2,
     ":coop2", this.CooperatorID, DbType.Int32,
     ":groupid", newGroupID, DbType.Int32
     ));

                    dm.Commit();
                }

            } catch (Exception ex) {

                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;

        }

        public DataSet ListFilesByGroup(int groupID, bool assignedToGroup) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    dm.Read(@"
select
sf.sys_file_id
      ,sf.is_enabled
      ,sf.virtual_file_path
      ,sf.file_name
      ,sf.file_version
      ,sf.file_size
      ,sf.display_name
      ,sf.created_date
      ,sf.created_by
      ,sf.modified_date
      ,sf.modified_by
      ,sf.owned_date
      ,sf.owned_by
from
    sys_file sf 
where
    sf.sys_file_id " + (assignedToGroup ? "" : "not") + @" in (
    select sys_file_id from sys_file_group_map sfgm
    where sys_file_group_id = :groupid
)

", dsReturn, "files_by_group", new DataParameters(":groupid", groupID, DbType.Int32));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public int SaveFileGroup(int groupID, string name, string version, bool enabled) {
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (groupID < 0) {
                        groupID = dm.Write(@"
insert into sys_file_group
(group_name, version_name, is_enabled, created_date, created_by, owned_date, owned_by)
values
(:group_name, :version_name, :enabled, :created_date, :created_by, :owned_date, :owned_by)
", true, "sys_file_group_id", new DataParameters(
      ":group_name", name,
      ":version_name", version,
      ":enabled", (enabled ? "Y" : "N"),
      ":created_date", DateTime.UtcNow, DbType.DateTime2,
      ":created_by", this.CooperatorID, DbType.Int32,
      ":owned_date", DateTime.UtcNow, DbType.DateTime2,
      ":owned_by", this.CooperatorID, DbType.Int32));
                    } else {
                        dm.Write(@"
update
    sys_file_group
set
    group_name = :group_name,
    version_name = :version_name,
    is_enabled = :enabled,
    modified_date = :modified_date,
    modified_by = :modified_by
where
    sys_file_group_id = :group_id
", new DataParameters(
      ":group_name", name,
      ":version_name", version,
      ":enabled", (enabled ? "Y" : "N"),
      ":modified_date", DateTime.UtcNow, DbType.DateTime2,
      ":modified_by", this.CooperatorID, DbType.Int32,
      ":group_id", groupID, DbType.Int32));

                    }

                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);


                return groupID;

            } catch (Exception ex) {
                LogException(ex, null);
                throw ex;
            }
        }

        public DataSet ListFileGroups(int groupID, bool onlyEnabled) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    dm.Read(@"
select
    (select sum(sf.file_size) from sys_file sf where sf.sys_file_id in (select sfgm.sys_file_id from sys_file_group_map sfgm where sfgm.sys_file_group_id = sfg.sys_file_group_id)) as total_size,
    sfg.sys_file_group_id
      ,sfg.group_name
      ,sfg.version_name
      ,sfg.is_enabled
      ,sfg.created_date
      ,sfg.created_by
      ,sfg.modified_date
      ,sfg.modified_by
      ,sfg.owned_date
      ,sfg.owned_by
from
    sys_file_group sfg
where
    sfg.sys_file_group_id = coalesce(:fgid, sfg.sys_file_group_id)
    and sfg.is_enabled = coalesce(:enabled, sfg.is_enabled)
order by
    sfg.group_name, sfg.version_name
", dsReturn, "list_file_groups", new DataParameters(":fgid", (groupID > 0 ? (int?)groupID : null), DbType.Int32, ":enabled", (onlyEnabled ? "Y" : null)));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet ListFiles(int fileID, bool onlyEnabled) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    dm.Read(@"
select
    sf.sys_file_id
      ,sf.is_enabled
      ,sf.virtual_file_path
      ,sf.file_name
      ,sf.file_version
      ,sf.file_size
      ,sf.display_name
      ,sf.created_date
      ,sf.created_by
      ,sf.modified_date
      ,sf.modified_by
      ,sf.owned_date
      ,sf.owned_by
from
    sys_file sf
where
    sf.sys_file_id = coalesce(:fid, sf.sys_file_id)
    and sf.is_enabled = coalesce(:enabled, sf.is_enabled)
order by
    sf.display_name, sf.file_version
", dsReturn, "list_files", new DataParameters(":fid", (fileID > 0 ? (int?)fileID : null), DbType.Int32, ":enabled", (onlyEnabled ? "Y" : null)));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet RenameFileGroup(int groupID, string newGroupName) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {

                    checkUserHasAdminEnabled();

                    dm.Write(@"
update
    sys_file_group
set
    group_name = :newgroupname,
    modified_date = :now,
    modified_by = :coopid
where
    sys_file_group_id = :groupid
", new DataParameters(":newgroupname", newGroupName, ":now", DateTime.UtcNow, DbType.DateTime2, ":coopid", this.CooperatorID, DbType.Int32, ":groupid", groupID, DbType.Int32));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;

        }

        public DataSet ToggleFileGroupEnabled(int groupID, bool enable) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    // this requires administrative privileges
                    checkUserHasAdminEnabled();

                    // toggle table mapping is_enabled flag
                    dm.Write(@"
update
    sys_file_group
set
    is_enabled = :enabled,
    modified_date = :modifieddate,
    modified_by = :modifiedby
where
    sys_file_group_id = :fgid",
                          new DataParameters(
                              ":enabled", enable ? "Y" : "N",
                              ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
                              ":modifiedby", this.SysUserID, DbType.Int32,
                              ":fgid", groupID, DbType.Int32));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet DeleteFile(int fileID) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {


                    // this requires administrative privileges
                    checkUserHasAdminEnabled();

                    dm.BeginTran();

                    // first disassociate from all groups
                    dm.Write(@"
delete from
    sys_file_group_map
where
    sys_file_id = :fid
", new DataParameters(":fid", fileID, DbType.Int32));

                    // then lookup the virtual path
                    var vpath = (string)dm.ReadValue(@"
select 
    virtual_file_path 
from 
    sys_file 
where 
    sys_file_id = :fid
", new DataParameters(":fid", fileID, DbType.Int32));

                    // then delete the file mapping
                    dm.Write(@"
delete from
    sys_file
where
    sys_file_id = :fid
", new DataParameters(":fid", fileID, DbType.Int32));

                    // calc the physical path
                    var ggWeb = Toolkit.GetIISPhysicalPath("gringlobal");
                    var physPath = (vpath.Replace("~/", ggWeb + @"\")).Replace("/", @"\").Replace(@"\\", @"\");

                    if (File.Exists(physPath)) {
                        File.Delete(physPath);
                    }

                    // and finally commit all our changes to the database
                    dm.Commit();

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public int SaveFile(int id, string displayName, string fileName, string virtualPath, string physicalPath, bool enabled) {
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();


                    string fileVersion = "";
                    if (physicalPath.ToLower().EndsWith(".msi")) {
                        fileVersion = getProductVersion(physicalPath);
                    } else {
                        var fvi = FileVersionInfo.GetVersionInfo(physicalPath);
                        fileVersion = fvi.FileVersion;
                    }

                    var fileSize = new FileInfo(physicalPath).Length;

                    if (id < 0) {
                        id = dm.Write(@"
insert into sys_file
(is_enabled, virtual_file_path, file_name, file_version, file_size, display_name, created_date, created_by, owned_date, owned_by)
values
(:enabled, :vpath, :file_name, :file_version, :file_size, :display_name, :created_date, :created_by, :owned_date, :owned_by)
", true, "sys_file_id", new DataParameters(
      ":enabled", (enabled ? "Y" : "N"),
      ":vpath", virtualPath,
      ":file_name", fileName,
      ":file_version", fileVersion,
      ":file_size", fileSize, DbType.Int64,
      ":display_name", displayName,
      ":created_date", DateTime.UtcNow, DbType.DateTime2,
      ":created_by", this.CooperatorID, DbType.Int32,
      ":owned_date", DateTime.UtcNow, DbType.DateTime2,
      ":owned_by", this.CooperatorID, DbType.Int32));
                    } else {
                        dm.Write(@"
update
    sys_file
set
    is_enabled = :enabled,
    virtual_file_path = :vpath,
    file_name = :file_name,
    file_version = :file_version,    
    file_size = :file_size,
    display_name = :display_name,
    modified_date = :modified_date,
    modified_by = :modified_by
where
    sys_file_id = :file_id
", new DataParameters(
      ":enabled", (enabled ? "Y" : "N"),
      ":vpath", virtualPath,
      ":file_name", fileName,
      ":file_version", fileVersion,
      ":file_size", fileSize, DbType.Int64,
      ":display_name", displayName,
      ":modified_date", DateTime.UtcNow, DbType.DateTime2,
      ":modified_by", this.CooperatorID, DbType.Int32,
      ":file_id", id, DbType.Int32));

                    }

                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);


                return id;

            } catch (Exception ex) {
                LogException(ex, null);
                throw ex;
            }
        }


        public DataSet ToggleFileEnabled(int fileID, bool enable) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    // this requires administrative privileges
                    checkUserHasAdminEnabled();

                    // toggle table mapping is_enabled flag
                    dm.Write(@"
update
    sys_file
set
    is_enabled = :enabled,
    modified_date = :modifieddate,
    modified_by = :modifiedby
where
    sys_file_id = :fid",
                          new DataParameters(
                              ":enabled", enable ? "Y" : "N",
                              ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
                              ":modifiedby", this.SysUserID, DbType.Int32,
                              ":fid", fileID, DbType.Int32));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet RemoveFileFromGroup(int groupID, int fileID) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    // this requires administrative privileges
                    checkUserHasAdminEnabled();

                    // toggle table mapping is_enabled flag
                    dm.Write(@"
delete from
    sys_file_group_map
where
    sys_file_group_id = :groupid
    and sys_file_id = :fileid
", new DataParameters(":groupid", groupID, DbType.Int32, ":fileid", fileID, DbType.Int32));
                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet AddFileToGroup(int groupID, int fileID) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    // this requires administrative privileges
                    checkUserHasAdminEnabled();

                    // toggle table mapping is_enabled flag
                    dm.Write(@"
insert into sys_file_group_map 
(sys_file_group_id, sys_file_id, created_date, created_by, owned_date, owned_by)
values
(:groupid, :fileid, :now1, :coop1, :now2, :coop2)
", new DataParameters(
     ":groupid", groupID, DbType.Int32,
     ":fileid", fileID, DbType.Int32,
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":coop1", CooperatorID, DbType.Int32,
     ":now2", DateTime.UtcNow, DbType.DateTime2,
     ":coop2", CooperatorID, DbType.Int32));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet DeleteFileGroup(int groupID) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {

                    checkUserHasAdminEnabled();
                    dm.Write(@"
delete from
    sys_file_group_map
where
    sys_file_group_id = :groupid
", new DataParameters(":groupid", groupID, DbType.Int32));

                    dm.Write(@"
delete from
    sys_file_group                        
where
    sys_file_group_id = :groupid
", new DataParameters(":groupid", groupID, DbType.Int32));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;

        }
























































        public int SaveTrigger(int triggerID, int dataviewID, int tableID, string virtualFilePath, string assemblyName, string className, string title, string description, bool enabled, bool system) {
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (triggerID < 0) {
                        // try to lookup the actual trigger id based on the alternate key (dataview,table,classname)
                        triggerID = Toolkit.ToInt32(
                            dm.ReadValue("select sys_datatrigger_id from sys_datatrigger where coalesce(sys_dataview_id,-1) = :dvid and coalesce(sys_table_id, -1) = :tblid and fully_qualified_class_name = :fqcn",
                                new DataParameters(":dvid", dataviewID, DbType.Int32,
                                    ":tblid", tableID, DbType.Int32,
                                    ":fqcn", className, DbType.String)), -1);
                    }

                    if (triggerID < 0) {
                        triggerID = dm.Write(@"
insert into sys_datatrigger
(sys_dataview_id, sys_table_id, virtual_file_path, assembly_name, fully_qualified_class_name, is_enabled, is_system, sort_order, created_date, created_by, owned_date, owned_by)
values
(:sys_dataview_id, :sys_table_id, :virtual_file_path, :assembly_name, :fully_qualified_class_name, :is_enabled, :is_system, :sort_order, :created_date, :created_by, :owned_date, :owned_by)
", true, "sys_datatrigger_id", new DataParameters(
      ":sys_dataview_id", dataviewID < 0 ? null : (int?)dataviewID, DbType.Int32,
      ":sys_table_id", tableID < 0 ? null : (int?)tableID, DbType.Int32,
      ":virtual_file_path", virtualFilePath,
      ":assembly_name", assemblyName,
      ":fully_qualified_class_name", className,
      ":is_enabled", enabled ? "Y" : "N",
      ":is_system", system ? "Y" : "N",
      ":sort_order", 0, DbType.Int32,
      ":created_date", DateTime.UtcNow, DbType.DateTime2,
      ":created_by", this.CooperatorID, DbType.Int32,
      ":owned_date", DateTime.UtcNow, DbType.DateTime2,
      ":owned_by", this.CooperatorID, DbType.Int32));
                    } else {
                        dm.Write(@"
update
    sys_datatrigger
set
    sys_dataview_id = :sys_dataview_id,
    sys_table_id = :sys_table_id,
    virtual_file_path = :virtual_file_path,
    assembly_name = :assembly_name,
    fully_qualified_class_name = :fully_qualified_class_name,
    is_enabled = :is_enabled,
    is_system = :is_system,
    modified_date = :modified_date,
    modified_by = :modified_by
where
    sys_datatrigger_id = :sys_datatrigger_id
", new DataParameters(
      ":sys_dataview_id", dataviewID < 0 ? null : (int?)dataviewID, DbType.Int32,
      ":sys_table_id", tableID < 0 ? null : (int?)tableID, DbType.Int32,
      ":virtual_file_path", virtualFilePath,
      ":assembly_name", assemblyName,
      ":fully_qualified_class_name", className,
      ":is_enabled", enabled ? "Y" : "N",
      ":is_system", system ? "Y" : "N",
      ":modified_date", DateTime.UtcNow, DbType.DateTime2,
      ":modified_by", this.CooperatorID, DbType.Int32,
      ":sys_datatrigger_id", triggerID, DbType.Int32));
                    }


                    var trigLangID = Toolkit.ToInt32(dm.ReadValue(@"
select
    sys_datatrigger_lang_id
from
    sys_datatrigger_lang
where
    sys_datatrigger_id = :id
", new DataParameters(":id", triggerID, DbType.Int32)), -1);

                    if (trigLangID < 0) {
                        trigLangID = dm.Write(@"
insert into sys_datatrigger_lang 
(sys_datatrigger_id, sys_lang_id, title, description, created_date, created_by, owned_date, owned_by)
values
(:id, :langid, :title, :description, :now1, :who1, :now2, :who2)
", true, "sys_datatrigger_lang_id", new DataParameters(
     ":id", triggerID, DbType.Int32,
     ":langid", LanguageID, DbType.Int32,
     ":title", title, DbType.String,
     ":description", description, DbType.String,
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":who1", CooperatorID, DbType.Int32,
     ":now2", DateTime.UtcNow, DbType.DateTime2,
     ":who2", CooperatorID, DbType.Int32));

                    } else {

                        dm.Write(@"
update
    sys_datatrigger_lang
set
    title = :title,
    description = :description,
    modified_by = :who1,
    modified_date = :now1
where
    sys_datatrigger_lang_id = :id
", new DataParameters(
     ":title", title, DbType.String,
     ":description", description, DbType.String,
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":who1", CooperatorID, DbType.Int32,
     ":id", trigLangID, DbType.Int32));

                    }




                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);


                return triggerID;

            } catch (Exception ex) {
                LogException(ex, null);
                throw ex;
            }
        }

        public DataSet ListTriggers(int triggerID, bool onlyEnabled) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    dm.Read(@"
select
    sdt.sys_datatrigger_id
      ,sdt.sys_dataview_id
      ,sdv.dataview_name
      ,sdt.sys_table_id
      ,st.table_name
      ,coalesce(sdv.dataview_name, st.table_name) as trigger_name
      ,sdt.virtual_file_path
      ,sdt.assembly_name
      ,sdt.fully_qualified_class_name
      ,sdtl.title
      ,sdtl.description
      ,sdt.is_enabled
      ,sdt.is_system
      ,sdt.sort_order
      ,sdt.created_date
      ,sdt.created_by
      ,sdt.modified_date
      ,sdt.modified_by
      ,sdt.owned_date
      ,sdt.owned_by
from
    sys_datatrigger sdt left join sys_dataview sdv
        on sdt.sys_dataview_id = sdv.sys_dataview_id
    left join sys_table st
        on sdt.sys_table_id = st.sys_table_id
    left join sys_datatrigger_lang sdtl
        on sdt.sys_datatrigger_id = sdtl.sys_datatrigger_id
        and sdtl.sys_lang_id = :langid
where
    sdt.sys_datatrigger_id = coalesce(:trigid, sdt.sys_datatrigger_id)
    and coalesce(sdt.is_enabled, '\b') = coalesce(:enabled, sdt.is_enabled, '\b')
order by
    coalesce(sdv.dataview_name, st.table_name, '-')
", dsReturn, "list_triggers", new DataParameters(
     ":langid", LanguageID, DbType.Int32,
     ":trigid", (triggerID > 0 ? (int?)triggerID : null), DbType.Int32,
     ":enabled", (onlyEnabled ? "Y" : null)));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet ToggleTriggerEnabled(int triggerID, bool enable) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    // this requires administrative privileges
                    checkUserHasAdminEnabled();

                    // toggle table mapping is_enabled flag
                    dm.Write(@"
update
    sys_datatrigger
set
    is_enabled = :enabled,
    modified_date = :modifieddate,
    modified_by = :modifiedby
where
    sys_datatrigger_id = :trigid",
                          new DataParameters(
                              ":enabled", enable ? "Y" : "N",
                              ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
                              ":modifiedby", this.SysUserID, DbType.Int32,
                              ":trigid", triggerID, DbType.Int32));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }


        public DataSet DeleteTrigger(int triggerID) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {

                    checkUserHasAdminEnabled();
                    dm.Write(@"
delete from
    sys_datatrigger_lang
where
    sys_datatrigger_id = :trigid
", new DataParameters(":trigid", triggerID, DbType.Int32));

                    dm.Write(@"
delete from
    sys_datatrigger
where
    sys_datatrigger_id = :trigid
", new DataParameters(":trigid", triggerID, DbType.Int32));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;

        }



        public DataSet SaveSearchEngineResolver(DataSet ds, string bindingType, string bindingUrl) {
            var dsReturn = createReturnDataSet();
            try {
                using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                    c.SaveResolverSettings(ds);
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }

        public DataSet SaveSearchEngineIndexer(DataSet ds, string bindingType, string bindingUrl) {
            var dsReturn = createReturnDataSet();
            try {
                using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                    c.SaveIndexerSettings(ds);
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }
        public DataSet SaveSearchEngineIndex(DataSet ds, string bindingType, string bindingUrl) {
            var dsReturn = createReturnDataSet();
            try {
                using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                    c.SaveIndexSettings(ds);
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }
        public DataSet DisableSearchEngineResolver(string indexName, string resolverName, string bindingType, string bindingUrl) {
            var dsReturn = createReturnDataSet();
            try {
                using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                    c.DisableResolver(indexName, resolverName);
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }


        public DataSet EnableSearchEngineResolver(string indexName, string resolverName, string bindingType, string bindingUrl) {
            var dsReturn = createReturnDataSet();
            try {
                using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                    c.EnableResolver(indexName, resolverName);
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }

        public DataSet RebuildSearchEngineIndex(string indexName, string bindingType, string bindingUrl) {
            var dsReturn = createReturnDataSet();
            try {
                using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                    c.RebuildIndexes(new List<string> { indexName }, false, true);
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }

        public DataSet RebuildAllEnabledSearchEngineIndexes(string bindingType, string bindingUrl) {
            var dsReturn = createReturnDataSet();
            try {
                using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                    c.RebuildIndexes(null, true, true);
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }

        public void ReloadSearchEngineIndexes(List<string> indexNames, string bindingType, string bindingUrl) {
            using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                c.ReloadIndexes(indexNames);
            }
        }

        public List<string> VerifySearchEngineIndexes(List<string> indexNames, string bindingType, string bindingUrl) {
            using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                return c.VerifyIndexes(indexNames, false);
            }
        }

        public DataSet DefragSearchEngineIndex(string indexName, string bindingType, string bindingUrl) {
            var dsReturn = createReturnDataSet();
            try {
                using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                    c.DefragIndexes(new List<string> { indexName }, true);
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }

        public DataSet EnableSearchEngineIndex(string indexName, string bindingType, string bindingUrl) {
            var dsReturn = createReturnDataSet();
            try {
                using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                    c.EnableIndexes(new List<string> { indexName });
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }


        public DataSet DeleteSearchEngineIndexes(string indexName, string bindingType, string bindingUrl) {
            var dsReturn = createReturnDataSet();
            try {
                using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                    c.DeleteIndexes(new List<string> { indexName });
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }


        public DataSet DisableSearchEngineIndex(string indexName, string bindingType, string bindingUrl) {
            var dsReturn = createReturnDataSet();
            try {
                using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                    c.DisableIndexes(new List<string> { indexName });
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }

        public DataSet DeleteTableMapping(int tableID, bool forceDelete) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    // this requires administrative privileges
                    checkUserHasAdminEnabled();

                    var sb = new StringBuilder(getDisplayMember("DeleteTableMapping{tableheader}", "Cannot delete the table mapping.\r\n"));
                    var errored = false;

                    dm.BeginTran();

                    // make sure no dataviews are using any fields in this table
                    var dataviews = dm.Read(@"
select 
    dv.sys_dataview_id, 
    dv.dataview_name
from 
    sys_dataview dv inner join sys_dataview_field dvf
        on dv.sys_dataview_id = dvf.sys_dataview_id
    inner join sys_table_field stf
        on dvf.sys_table_field_id = stf.sys_table_field_id
where
    stf.sys_table_id = :tblid
", new DataParameters(":tblid", tableID, DbType.Int32));

                    if (dataviews.Rows.Count > 0) {
                        if (!forceDelete) {
                            sb.Append(getDisplayMember("DeleteTableMapping{referencingdataviews}", "\r\nReferencing Dataviews:\r\n * "));
                            var fields = dataviews.ListColumnValues("dataview_name", true);
                            sb.AppendLine(String.Join("\r\n * ", fields.ToArray()));
                            errored = true;
                        } else {
                            var dtFields = dm.Read(@"
select
    sys_table_field_id
from
    sys_table_field
where
    sys_table_id = :tblid
", new DataParameters(":tblid", tableID, DbType.Int32));
                            foreach (DataRow drFields in dtFields.Rows) {
                                dm.Write(@"
update 
    sys_dataview_field
set
    sys_table_field_id = null,
    is_readonly = 'Y',
    modified_date = :now,
    modified_by = :who
where
    sys_table_field_id = :tblfid
", new DataParameters(":now", DateTime.UtcNow, DbType.DateTime2,
     ":who", CooperatorID, DbType.Int32,
     ":tblfid", Toolkit.ToInt32(drFields["sys_table_field_id"], -1)));

                            }

                        }
                    }


                    // make sure no perms are using these fields
                    var perms = dm.Read(@"
select 
    sp.sys_permission_id,
    sp.permission_tag
from 
    sys_permission sp left join sys_permission_field spf
        on sp.sys_permission_id = spf.sys_permission_id
    left join sys_table_field stf
        on spf.sys_table_field_id = stf.sys_table_field_id
where
    sp.sys_table_id = :tblid
    or stf.sys_table_id = :tblid2
", new DataParameters(":tblid", tableID, DbType.Int32, ":tblid2", tableID, DbType.Int32));

                    if (perms.Rows.Count > 0) {
                        if (!forceDelete) {
                            sb.Append(getDisplayMember("DeleteTableMapping{referencingpermissions}", "\r\nReferencing Permissions:\r\n * "));
                            var fields = perms.ListColumnValues("permission_tag", true);
                            sb.AppendLine(String.Join("\r\n * ", fields.ToArray()));
                            errored = true;
                        } else {

                            foreach (DataRow dr in perms.Rows) {

                                var permID = Toolkit.ToInt32(dr["sys_permission_id"], -1);

                                // drop perm fields
                                dm.Write(@"
delete from
    sys_permission_field
where
    sys_permission_id = :permid
", new DataParameters(":permid", permID, DbType.Int32));

                                // drop perm groups
                                dm.Write(@"
delete from 
    sys_group_permission_map
where
    sys_permission_id = :permid
", new DataParameters(":permid", permID, DbType.Int32));

                                // drop perm users
                                dm.Write(@"
delete from 
    sys_user_permission_map
where
    sys_permission_id = :permid
", new DataParameters(":permid", permID, DbType.Int32));

                                // drop perm lang
                                dm.Write(@"
delete from 
    sys_permission_lang
where
    sys_permission_id = :permid
", new DataParameters(":permid", permID, DbType.Int32));

                                // drop perm
                                dm.Write(@"
delete from
    sys_permission
where
    sys_permission_id = :permid
", new DataParameters(":permid", permID, DbType.Int32));

                            }
                        }
                    }

                    // make sure no other tables are pointing to any fields in this table as a foreign key
                    var fks = dm.Read(@"
select
    stf_fk.sys_table_field_id,
    st_fk.table_name
from
    sys_table_field stf_del inner join sys_table st_del
        on stf_del.sys_table_id = st_del.sys_table_id
    inner join sys_table_field stf_fk
        on stf_fk.foreign_key_table_field_id = stf_del.sys_table_field_id
    inner join sys_table st_fk
        on st_fk.sys_table_id = stf_fk.sys_table_id
where
    stf_del.sys_table_id = :tblid

", new DataParameters(":tblid", tableID, DbType.Int32));

                    if (fks.Rows.Count > 0) {
                        if (!forceDelete) {
                            sb.Append(getDisplayMember("DeleteTableMappings{referencingtables}", "\r\nReferencing Tables:\r\n * "));
                            var tableNames = fks.ListColumnValues("table_name", true);
                            sb.AppendLine(String.Join("\r\n * ", tableNames.ToArray()));
                            errored = true;
                        } else {
                            foreach (DataRow fk in fks.Rows) {
                                dm.Write(@"
update
    sys_table_field
set
    foreign_key_table_field_id = null,
    foreign_key_dataview_name = null,
    modified_date = :now,
    modified_by = :who
where
    sys_table_field_id = :fldid
", new DataParameters(
     ":now", DateTime.UtcNow, DbType.DateTime2,
     ":who", CooperatorID, DbType.Int32,
     ":fldid", Toolkit.ToInt32(fk["sys_table_field_id"], -1), DbType.Int32));

                            }
                        }
                    }


                    if (errored) {
                        dm.Rollback();
                        throw Library.CreateBusinessException(sb.ToString());
                    }


                    // we get here, no dataviews or permissions are mapped to any fields for this table.
                    // this means we can start deleting data, but we want to do it atomically!
                    var dps = new DataParameters(":tblid", tableID, DbType.Int32);

                    // drop trigger mappings
                    dm.Write("delete from sys_datatrigger where sys_table_id = :tblid", dps);

                    // drop index mappings
                    dm.Write("delete from sys_index_field where sys_index_id in (select sys_index_id from sys_index where sys_table_id = :tblid)", dps);
                    dm.Write("delete from sys_index where sys_table_id = :tblid", dps);

                    // drop table mappings
                    dm.Write("delete from sys_table_relationship where sys_table_field_id in (select sys_table_field_id from sys_table_field where sys_table_id = :tblid)", dps);
                    dm.Write("delete from sys_table_relationship where other_table_field_id in (select sys_table_field_id from sys_table_field where sys_table_id = :tblid)", dps);
                    dm.Write("delete from sys_table_field_lang where sys_table_field_id in (select sys_table_field_id from sys_table_field where sys_table_id = :tblid)", dps);
                    dm.Write("delete from sys_search_autofield where sys_table_field_id in (select sys_table_field_id from sys_table_field where sys_table_id = :tblid)", dps);
                    dm.Write("delete from sys_search_resolver where sys_table_id = :tblid", dps);
                    dm.Write("delete from sys_table_field where sys_table_id = :tblid", dps);
                    dm.Write("delete from sys_table where sys_table_id = :tblid", dps);

                    dm.Commit();

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet ToggleTableMappingEnabled(int tableID, bool enable) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    // this requires administrative privileges
                    checkUserHasAdminEnabled();

                    // toggle table mapping is_enabled flag
                    dm.Write(@"
update
    sys_table
set
    is_enabled = :enabled,
    modified_date = :modifieddate,
    modified_by = :modifiedby
where
    sys_table_id = :tblid",
                          new DataParameters(
                              ":enabled", enable ? "Y" : "N",
                              ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
                              ":modifiedby", this.SysUserID, DbType.Int32,
                              ":tblid", tableID, DbType.Int32));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet ListUnmappedTables() {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = this.BeginProcessing(true)) {

                    var dtMapped = dm.Read(@"select * from sys_table order by table_name");

                    var unmapped = Creator.GetInstance(dm.DataConnectionSpec).ListTableNames(null);

                    var dtRet = new DataTable("list_unmappedtables");
                    dtRet.Columns.Add("table_name", typeof(string));
                    dsReturn.Tables.Add(dtRet);

                    foreach (var s in unmapped) {
                        var matches = dtMapped.Select("table_name = '" + s + "'");
                        if (matches == null || matches.Length == 0) {
                            // this table is not mapped
                            dtRet.Rows.Add(new object[] { s });
                        }
                    }


                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;

        }


        public DataSet GetGroupInfo(int groupID) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = this.BeginProcessing(true)) {

                    dm.Read(@"
select
    sys_group_id
      ,is_enabled
      ,group_tag
      ,created_date
      ,created_by
      ,modified_date
      ,modified_by
      ,owned_date
      ,owned_by
from
    sys_group
where
    sys_group_id = :groupid
", dsReturn, "group_info", new DataParameters(":groupid", groupID, DbType.Int32));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;

        }

        public DataSet MakeFileGroupAvailable(int groupID, bool markAsEnabled) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    // put code specific to the method here

                    checkUserHasAdminEnabled();

                    dm.Write(@"
update
   sys_file_group
set
    is_enabled = :enabled,
    modified_date = :now,
    modified_by = :coop
where
    sys_file_group_id = :groupid
", new DataParameters(":enabled", (markAsEnabled ? "Y" : "N"),
     ":now", DateTime.UtcNow, DbType.DateTime2,
     ":coop", this.CooperatorID, DbType.Int32,
     ":groupid", groupID, DbType.Int32));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet UpdateFileInfo(int fileID, string virtualPath, string physicalPath, string displayName, bool makeAvailable) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {

                    checkUserHasAdminEnabled();

                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(physicalPath);
                    string version = fvi.FileVersion;

                    string name = Path.GetFileName(physicalPath);

                    if (name.ToLower().EndsWith(".msi")) {
                        // get product version from the file...
                        version = getProductVersion(physicalPath);
                    }

                    long size = new FileInfo(physicalPath).Length;

                    dm.Write(@"
update sys_file
set
    is_enabled = :enabled,
    virtual_file_path = :virt,
    file_version = :fileversion,
    file_size = :filesize,
    display_name = :disp,
    modified_date = :now,
    modified_by = :coop
where
    sys_file_id = :fileid
", new DataParameters(
     ":enabled", (makeAvailable ? "Y" : "N"),
     ":virt", virtualPath,
     ":fileversion", version,
     ":filesize", size, DbType.Int64,
     ":disp", displayName,
     ":now", DateTime.UtcNow, DbType.DateTime2,
     ":coop", this.CooperatorID, DbType.Int32,
     ":fileid", fileID, DbType.Int32
     ));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet AddFileMapping(string virtualPath, string physicalPath, string installedDisplayName, bool makeAvailable) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {

                    checkUserHasAdminEnabled();

                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(physicalPath);
                    string version = fvi.FileVersion;

                    string name = Path.GetFileName(physicalPath);

                    if (name.ToLower().EndsWith(".msi")) {
                        // get product version from the file...
                        version = getProductVersion(physicalPath);
                    }

                    long size = new FileInfo(physicalPath).Length;

                    dm.Write(@"
insert into sys_file
(is_enabled, virtual_file_path, file_name, file_version, file_size, display_name, created_date, created_by, owned_date, owned_by)
values
(:enabled, :virt, :name, :fileversion, :filesize, :disp, :now1, :coop1, :now2, :coop2)
", new DataParameters(
     ":enabled", (makeAvailable ? "Y" : "N"),
     ":virt", virtualPath,
     ":name", name,
     ":fileversion", version,
     ":filesize", size, DbType.Int64,
     ":disp", installedDisplayName,
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":coop1", this.CooperatorID, DbType.Int32,
     ":now2", DateTime.UtcNow, DbType.DateTime2,
     ":coop2", this.CooperatorID, DbType.Int32
     ));

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public string DeleteFileMapping(int fileID) {
            try {
                using (DataManager dm = BeginProcessing(true)) {

                    checkUserHasAdminEnabled();

                    string virtualFilePath = dm.ReadValue("select virtual_file_path from sys_file where sys_file_id = :fileid", new DataParameters(":fileid", fileID, DbType.Int32)).ToString();
                    dm.Write(@"
delete from 
    sys_file
where
    sys_file_id = :fileid
", new DataParameters(":fileid", fileID, DbType.Int32));

                    return virtualFilePath;

                }

            } catch (Exception ex) {
                if (LogException(ex, null)) {
                    throw;
                }
            } finally {
                FinishProcessing(null, LanguageID);
            }
            return null;
        }



        #endregion File Group

        #region Administration


        public DataSet ListResources(string appName, int languageID) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    // put code specific to the method here

                    dm.Read(@"
select
    *
from
    app_resource
where
    app_name = coalesce(:appname, app_name)
    and sys_lang_id = coalesce(:langid, sys_lang_id)
order by
    app_name,
    form_name,
    app_resource_name
", dsReturn, "list_resources", new DataParameters(
     ":appname", String.IsNullOrEmpty(appName) ? null : appName,
     ":langid", (languageID < 0 ? null : (int?)languageID), DbType.Int32));


                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet ChangeWebPassword(string webUserName, string newPlaintextPassword) {

            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {

                    if (WebUserName != webUserName) {

                        checkUserHasAdminEnabled();

                        // throw Library.CreateBusinessException("You do not have rights to change the password for user '" + targetUserName + "'.");
                    }

                    // we get here, either:
                    // a) they're updating their own password (which is ok regardless of rights)
                    // b) they do have permission to update the web_user table

                    string hashedPassword = Crypto.HashText(newPlaintextPassword);
                    string doublyHashedPassword = Crypto.HashText(hashedPassword);
                    dm.Write(@"
update
	web_user
set
	password = :pw
where
	user_name = :webusername
",
                        new DataParameters(
                            new DataParameter(":pw", doublyHashedPassword),
                            new DataParameter(":webusername", webUserName)));


                    log("changed the password for web user " + webUserName + ".");
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }


        public DataSet ChangePassword(int targetUserID, string newPlaintextPassword) {

            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {

                    if (SysUserID != targetUserID) {

                        checkUserHasAdminEnabled();

                        // throw Library.CreateBusinessException("You do not have rights to change the password for user '" + targetUserName + "'.");
                    }

                    // we get here, either:
                    // a) they're updating their own password (which is ok regardless of rights)
                    // b) they do have permission to update the sys_user table

                    string hashedPassword = Crypto.HashText(newPlaintextPassword);
                    string doubleHashedPassword = Crypto.HashText(hashedPassword);
                    dm.Write(@"
update
	sys_user
set
	password = :pw,
	modified_date = :modifieddate,
	modified_by = :modifiedby
where
	sys_user_id = :userid
",
                        new DataParameters(
                            new DataParameter(":pw", doubleHashedPassword),
                            new DataParameter(":modifieddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2),
                            new DataParameter(":modifiedby", CooperatorID, DbType.Int32),
                            new DataParameter(":userid", targetUserID)));


                    log("changed the password for user " + targetUserID + ".");
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public bool IsUserEnabled(int userID) {
            bool ret = false;
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    string enabled = dm.ReadValue(@"
select 
	is_enabled
from 
	sys_user 
where 
	sys_user_id = :userid
", new DataParameters(":userid", userID, DbType.Int32)).ToString();
                    ret = enabled == "Y";

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return ret;
        }

        public DataSet EnableUser(int userID) {
            return changeUserEnabled(userID, true);
        }

        public DataSet DisableUser(int userID) {
            return changeUserEnabled(userID, false);
        }

        private DataSet changeUserEnabled(int userID, bool enabled) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    //if (!CanUpdate(dm, "sys_user", null)) {
                    //    throw Library.DataPermissionException(this.UserName, "sys_user", Permission.Update, null);
                    //}

                    checkUserHasAdminEnabled();

                    if (!enabled) {
                        if (userID == this.SysUserID) {
                            throw Library.CreateBusinessException(getDisplayMember("changeUserEnabled{notyourself}", "You cannot disable yourself."));
                        }

                        var groupID = this.GetSysGroupIDForTag("admins");
                        var dtAdmins = this.ListUsersInGroup(groupID).Tables["list_users_in_group"];
                        if (dtAdmins.Rows.Count == 1) {
                            if (userID == Toolkit.ToInt32(dtAdmins.Rows[0]["sys_user_id"], -1)) {
                                throw Library.CreateBusinessException(getDisplayMember("changeUserEnabled{notlastadmin}", "You cannot disable the last member of the ADMINS group."));
                            }
                        }
                    }


                    dm.Write(@"
update 
	sys_user
set 
	is_enabled = :isenabled,
	modified_date = :modifieddate,
	modified_by = :modifiedby
where 
	sys_user_id = :userid
",
                            new DataParameters(
                            new DataParameter(":isenabled", (enabled ? "Y" : "N")),
                            new DataParameter(":modifieddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2),
                            new DataParameter(":modifiedby", CooperatorID, DbType.Int32),
                            new DataParameter(":userid", userID, DbType.Int32)));

                    if (enabled) {
                        log("enabled user " + userID);
                    } else {
                        log("disabled user " + userID);
                    }
                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet SearchGeographies(string query) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (!String.IsNullOrEmpty(query)) {
                        if (!query.Contains("%")) {
                            // force a right-hand like if needed
                            query += "%";
                        }
                    }

                    dm.Read(@"
SELECT
  g.geography_id,
  g.current_geography_id,
  r.region_id,
  r.continent,
  r.subcontinent,
  g.country_code,
  cvl.title as country_name,
  g.adm1,
  g.adm1_type_code,
  g.adm2,
  g.adm2_type_code,
  g.adm3,
  g.adm3_type_code,
  g.adm4,
  g.adm4_type_code,
  g.changed_date,
  g.note,
  g.created_date,
  g.created_by,
  g.modified_date,
  g.modified_by,
  g.owned_date,
  g.owned_by
FROM
  geography g left join geography_region_map grm 
    on g.geography_id = grm.geography_id
    left join region r
    on grm.region_id = r.region_id
    left join code_value cv
        on g.country_code = cv.value
        and cv.group_name = 'GEOGRAPHY_COUNTRY_CODE'
    left join code_value_lang cvl
        on cv.code_value_id = cvl.code_value_id
        and cvl.sys_lang_id = :langid
where
    g.country_code like :q1 or g.adm1 like :q2 or r.continent like :q3 or r.subcontinent like :q4 or cvl.title like :q5
order by
    r.continent, r.subcontinent, g.country_code, g.adm1
", dsReturn, "search_geographies", new DataParameters(":q1", query, DbType.String,
     ":q2", query, DbType.String,
     ":q3", query, DbType.String, 
     ":q4", query, DbType.String,
     ":q5", query, DbType.String,
     ":langid", LanguageID, DbType.Int32));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet SearchWebCooperators(string query) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (!String.IsNullOrEmpty(query)) {
                        if (!query.Contains("%")) {
                            // force a right-hand like if needed
                            query += "%";
                        }
                    }

                    dm.Read(@"
select
wc.web_cooperator_id
      ,wu.user_name
      ,wc.last_name
      ,wc.title
      ,wc.first_name
      ,wc.job
      ,wc.organization
      ,wc.address_line1
      ,wc.address_line2
      ,wc.address_line3
      ,wc.city
      ,wc.postal_index
      ,wc.geography_id
      ,wc.primary_phone
      ,wc.secondary_phone
      ,wc.fax
      ,wc.email
      ,wc.category_code
      ,    TRIM(CONCAT(COALESCE(wc.last_name, ''), ', ', COALESCE(wc.first_name, ''), ', ', COALESCE(wc.organization, ''))) as full_name
      ,wc.note
      ,wu.sys_lang_id
      ,wc.created_date
      ,wc.created_by
      ,wc.modified_date
      ,wc.modified_by
      ,wc.owned_date
      ,wc.owned_by
from
    web_cooperator wc
    inner join web_user wu
        on wc.web_cooperator_id = wu.web_cooperator_id
    left join sys_lang sl
        on wu.sys_lang_id = sl.sys_lang_id
where
    wc.first_name like :q1 or wc.last_name like :q2 or 
    wu.user_name like :q3 or
        TRIM(CONCAT(COALESCE(wc.last_name, ''), ', ', COALESCE(wc.first_name, ''), ', ', COALESCE(wc.organization, ''))) like :q4
order by
    wu.user_name, wc.last_name, wc.first_name, wc.organization
", dsReturn, "search_web_cooperators", new DataParameters(":q1", query, ":q2", query, ":q3", query, ":q4", query));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }



        public DataSet SearchCooperators(string query) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (!String.IsNullOrEmpty(query)) {
                        if (!query.Contains("%")) {
                            // force a right-hand like if needed
                            query += "%";
                        }
                    }

                    dm.Read(@"
select
c.cooperator_id
      ,c.current_cooperator_id
      ,c.site_id
      ,s.site_short_name
      ,c.last_name
      ,c.title
      ,c.first_name
      ,c.job
      ,c.organization
      ,c.organization_abbrev
      ,c.address_line1
      ,c.address_line2
      ,c.address_line3
      ,c.city
      ,c.postal_index
      ,c.geography_id
      ,c.primary_phone
      ,c.secondary_phone
      ,c.fax
      ,c.email
      ,c.status_code
      ,c.category_code
      ,c.organization_region_code
      ,c.discipline_code
      ,    TRIM(CONCAT(COALESCE(c.last_name, ''), ', ', COALESCE(c.first_name, ''), ', ', COALESCE(c.organization, ''))) as full_name
      ,c.note
      ,c.sys_lang_id
      ,c.created_date
      ,c.created_by
      ,c.modified_date
      ,c.modified_by
      ,c.owned_date
      ,c.owned_by,
    TRIM(CONCAT(COALESCE(c2.last_name, ''), ', ', COALESCE(c2.first_name, ''), ', ', COALESCE(c2.organization, ''))) as current_cooperator_full_name,
    sl.title as language_name
from
    cooperator c
    left join sys_lang sl
        on c.sys_lang_id = sl.sys_lang_id
    left join site s
        on c.site_id = s.site_id
    left join cooperator c2
        on c.current_cooperator_id = c2.cooperator_id
where
    c.first_name like :q1 or c.last_name like :q2 or 
        TRIM(CONCAT(COALESCE(c.last_name, ''), ', ', COALESCE(c.first_name, ''), ', ', COALESCE(c.organization, ''))) like :q3
order by
    c.last_name, c.first_name, c.organization
", dsReturn, "search_cooperators", new DataParameters(":q1", query, ":q2", query, ":q3", query));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet GetWebCooperatorInfo(int webCooperatorID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
wc.web_cooperator_id
    ,wu.user_name as web_user_name
    , wu.web_cooperator_id
    , wc.last_name as web_last_name
    , wc.first_name as web_first_name
      ,wc.last_name
      ,wc.title
      ,wc.first_name
from
    web_cooperator wc
    left join web_user wu
        on wu.web_cooperator_id = wc.web_cooperator_id
where
    wc.web_cooperator_id = :webcooperatorid
order by
    wc.last_name, wc.first_name
", dsReturn, "web_cooperator_info", new DataParameters(":webcooperatorid", webCooperatorID, DbType.Int32));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }


        public DataSet GetCooperatorInfo(int cooperatorID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
c.cooperator_id
    ,wu.user_name as web_user_name
    , wu.web_cooperator_id
    , wc.last_name as web_last_name
    , wc.first_name as web_first_name
      ,c.current_cooperator_id
      ,c.site_id
      ,s.site_short_name
      ,c.last_name
      ,c.title
      ,c.first_name
      ,c.job
      ,c.organization
      ,c.organization_abbrev
      ,c.address_line1
      ,c.address_line2
      ,c.address_line3
      ,c.city
      ,c.postal_index
      ,c.geography_id
      , TRIM(CONCAT(COALESCE(g.adm1,''), ', ', COALESCE(g.country_code,''), ', ', COALESCE(r.subcontinent,''), ', ', COALESCE(r.continent,''))) as geography_description
      ,c.primary_phone
      ,c.secondary_phone
      ,c.fax
      ,c.email
      ,c.status_code
      ,c.category_code
      ,c.organization_region_code
      ,c.discipline_code
      ,    TRIM(CONCAT(COALESCE(c.last_name, ''), ', ', COALESCE(c.first_name, ''), ', ', COALESCE(c.organization, ''))) as full_name,
      c.note
      ,c.sys_lang_id
      ,c.created_date
      ,c.created_by
      ,c.modified_date
      ,c.modified_by
      ,c.owned_date
      ,c.owned_by,
          TRIM(CONCAT(COALESCE(c2.last_name, ''), ', ', COALESCE(c2.first_name, ''), ', ', COALESCE(c2.organization, ''))) as current_cooperator_full_name,
    sl.title as language_name
from
    cooperator c
    left join web_cooperator wc
        on c.web_cooperator_id = wc.web_cooperator_id
    left join web_user wu
        on wu.web_cooperator_id = wc.web_cooperator_id
    left join sys_lang sl
        on c.sys_lang_id = sl.sys_lang_id
    left join site s
        on c.site_id = s.site_id
    left join cooperator c2
        on c.current_cooperator_id = c2.cooperator_id
    left join geography g
        on c.geography_id = g.geography_id
    left join geography_region_map grm
        on g.geography_id = grm.geography_id
    left join region r
        on grm.region_id = r.region_id
where
    c.cooperator_id = :cooperatorid
order by
    c.last_name, c.first_name
", dsReturn, "cooperator_info", new DataParameters(":cooperatorid", cooperatorID, DbType.Int32));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet GetUserInfo(int userID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    su.sys_user_id,
    su.user_name,
    wu.web_cooperator_id as web_cooperator_id,
    wu.user_name as web_user_name,
    wc.last_name as web_last_name,
    wc.first_name as web_first_name,
    case when len(coalesce(su.password,'')) > 0 then 'Y' else 'N' end as password_is_set,
    c.cooperator_id
      ,c.current_cooperator_id
      ,c.site_id
      ,s.site_short_name
      ,c.last_name
      ,c.title
      ,c.first_name
      ,c.job
      ,c.organization
      ,c.organization_abbrev
      ,c.organization_region_code
      ,c.address_line1
      ,c.address_line2
      ,c.address_line3
      ,c.city
      ,c.postal_index
      ,c.geography_id
      ,c.secondary_organization
      ,c.secondary_organization_abbrev
      ,c.secondary_address_line1
      ,c.secondary_address_line2
      ,c.secondary_address_line3
      ,c.secondary_city
      ,c.secondary_postal_index
      ,c.secondary_geography_id
      ,c.primary_phone
      ,c.secondary_phone
      ,c.fax
      ,c.email
      ,c.status_code
      ,c.category_code
      ,c.organization_abbrev
      ,c.discipline_code
      ,c.geography_id
      , TRIM(CONCAT(COALESCE(g.adm1,''), ', ', COALESCE(g.country_code,''), ', ', COALESCE(r.subcontinent,''), ', ', COALESCE(r.continent,''))) as geography_description
      ,TRIM(CONCAT(COALESCE(c.last_name, ''), ', ', COALESCE(c.first_name, ''), ' ', COALESCE(c.organization, ''))) as full_name
      ,c.note
      ,c.sys_lang_id
      ,c.created_date
      ,c.created_by
      ,c.modified_date
      ,c.modified_by
      ,c.owned_date
      ,c.owned_by,
    TRIM(CONCAT(COALESCE(c2.last_name, ''), ', ', COALESCE(c2.first_name, ''), ' ', COALESCE(c2.organization, ''))) as current_cooperator_full_name,
    su.is_enabled,
    sl.title as language_name
from
    sys_user su left join cooperator c
        on su.cooperator_id = c.cooperator_id
    left join site s
        on c.site_id = s.site_id
    left join sys_lang sl
        on c.sys_lang_id = sl.sys_lang_id
    left join cooperator c2
        on c.current_cooperator_id = c2.cooperator_id
    left join geography g
        on c.geography_id = g.geography_id
    left join geography_region_map grm
        on g.geography_id = grm.geography_id
    left join region r
        on grm.region_id = r.region_id
    left join web_cooperator wc
        on c.web_cooperator_id = wc.web_cooperator_id
    left join web_user wu
        on wc.web_cooperator_id = wu.web_cooperator_id
where
    su.sys_user_id = :userid
order by
    su.user_name
", dsReturn, "user_info", new DataParameters(":userid", userID, DbType.Int32));

                    dm.Read(@"
select
    1,
    coalesce(sgl.title, sg.group_tag) as group_name,
    sp.sys_permission_id,
    spl.title,
    sd.sys_dataview_id,
    sd.dataview_name,
    st.sys_table_id,
    st.table_name,
    coalesce(sp.sys_table_id, 0) * 1000000 + coalesce(sp.sys_dataview_id, 0) as perm_rank,
    case sp.create_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as create_permission_text,
    case sp.read_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as read_permission_text,
    case sp.update_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as update_permission_text,
    case sp.delete_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as delete_permission_text
from
    sys_permission sp inner join sys_group_permission_map sgpm
        on sp.sys_permission_id = sgpm.sys_permission_id
    inner join sys_group_user_map sgum
        on sgpm.sys_group_id = sgum.sys_group_id
    inner join sys_group sg
        on sgum.sys_group_id = sg.sys_group_id
    left join sys_dataview sd
        on sp.sys_dataview_id = sd.sys_dataview_id
    left join sys_table st
        on sp.sys_table_id = st.sys_table_id
    left join sys_permission_lang spl
        on sp.sys_permission_id = spl.sys_permission_id
        and spl.sys_lang_id = :langid1
    left join sys_group_lang sgl
        on sg.sys_group_id = sgl.sys_group_id
        and sgl.sys_lang_id = :langid2
where
    sgum.sys_user_id = :userid
    and sp.is_enabled = 'Y'

UNION

select
    2,
    null as group_name,
    sp.sys_permission_id,
    spl.title,
    sd.sys_dataview_id,
    sd.dataview_name,
    st.sys_table_id,
    st.table_name,
    coalesce(sp.sys_table_id, 0) * 1000000 + coalesce(sp.sys_dataview_id, 0) as perm_rank,
    case sp.create_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as create_permission_text,
    case sp.read_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as read_permission_text,
    case sp.update_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as update_permission_text,
    case sp.delete_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as delete_permission_text
from
    sys_permission sp inner join sys_user_permission_map sup
        on sp.sys_permission_id = sup.sys_permission_id
    left join sys_dataview sd
        on sp.sys_dataview_id = sd.sys_dataview_id
    left join sys_table st
        on sp.sys_table_id = st.sys_table_id
    left join sys_permission_lang spl
        on sp.sys_permission_id = spl.sys_permission_id
        and spl.sys_lang_id = :langid3
where
    sup.sys_user_id = :userid
    and sup.is_enabled = 'Y'
    and sp.is_enabled = 'Y'
order by
    1,
    perm_rank,
    title
", dsReturn, "user_perm_info", new DataParameters(
     ":langid1", LanguageID, DbType.Int32,
     ":langid2", LanguageID, DbType.Int32,
     ":langid3", LanguageID, DbType.Int32,
     ":userid", userID, DbType.Int32));


                    dm.Read(@"
select
    spf.sys_permission_field_id
      ,spf.sys_permission_id
      ,spf.sys_dataview_field_id
      ,spf.sys_table_field_id
      ,spf.field_type
      ,spf.compare_operator
      ,spf.compare_value
      ,spf.parent_table_field_id
      ,spf.parent_field_type
      ,spf.parent_compare_operator
      ,spf.parent_compare_value
      ,spf.compare_mode
      ,spf.created_date
      ,spf.created_by
      ,spf.modified_date
      ,spf.modified_by
      ,spf.owned_date
      ,spf.owned_by,
    stf.sys_table_id,
    st.table_name,
    stf.field_name as table_field_name,
    sdf.sys_dataview_id,
    sd.dataview_name,
    sdf.field_name as dataview_field_name,
    stparent.table_name as parent_table_name,
    stfparent.field_name as parent_table_field_name
from
    sys_permission_field spf inner join sys_permission sp
        on spf.sys_permission_id = sp.sys_permission_id
    left join sys_user_permission_map sup
        on sp.sys_permission_id = sup.sys_permission_id
    left join sys_group_permission_map sgpm
        on sp.sys_permission_id = sgpm.sys_permission_id
    left join sys_group_user_map sgum
        on sgpm.sys_group_id = sgum.sys_group_id
    left join sys_dataview_field sdf
        on spf.sys_dataview_field_id = sdf.sys_dataview_field_id
    left join sys_dataview sd
        on sdf.sys_dataview_id = sd.sys_dataview_id
    left join sys_table_field stf
        on spf.sys_table_field_id = stf.sys_table_field_id
    left join sys_table st
        on stf.sys_table_id = st.sys_table_id
    left join sys_table_field stfparent
        on spf.parent_table_field_id = stfparent.sys_table_field_id
    left join sys_table stparent
        on stfparent.sys_table_id = stparent.sys_table_id
where
    sup.sys_user_id = :userid or sgum.sys_user_id = :userid2
", dsReturn, "user_perm_field_info", new DataParameters(":userid", userID, DbType.Int32,
     ":userid2", userID, DbType.Int32));

                    dm.Read(@"
select
    sg.sys_group_id,
    coalesce(sgl.title, sg.group_tag) as group_name,
    sgl.description
from
    sys_group sg inner join sys_group_user_map sgum
        on sg.sys_group_id = sgum.sys_group_id
    left join sys_group_lang sgl
        on sg.sys_group_id = sgl.sys_group_id
        and sgl.sys_lang_id = :langid
where
    sgum.sys_user_id = :userid
", dsReturn, "groups_by_user", new DataParameters(
     ":langid", LanguageID, DbType.Int32,
     ":userid", (userID < 0 ? null : (int?)userID), DbType.Int32));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }
        public DataSet ListUsers(int userID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    su.sys_user_id,
    su.user_name,
    wu.user_name as web_user_name,
    TRIM(CONCAT(COALESCE(c.last_name, ''), ', ', COALESCE(c.first_name, ''), ' ', COALESCE(c.organization, ''))) as full_name,
    case when 0 < (select count(sg.sys_group_id)
    from
        sys_group sg inner join sys_group_permission_map sgpm
            on sg.sys_group_id = sgpm.sys_group_id
        inner join sys_group_user_map sgum
            on sg.sys_group_id = sgum.sys_group_id
    where
        sg.group_tag = 'ADMINS'
        and sgum.sys_user_id = su.sys_user_id
    ) then 'Y' else 'N' end as is_admin,
    su.is_enabled,
    sl.title as language_name
from
    sys_user su left join cooperator c
        on su.cooperator_id = c.cooperator_id
    left join sys_lang sl
        on c.sys_lang_id = sl.sys_lang_id
    left join web_cooperator wc
        on c.web_cooperator_id = wc.web_cooperator_id
    left join web_user wu
        on wu.web_cooperator_id = wc.web_cooperator_id
where
    coalesce(:userid, su.sys_user_id) = su.sys_user_id
order by
    su.user_name
", dsReturn, "list_users", new DataParameters(":userid", userID < 0 ? null : (int?)userID, DbType.Int32));
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet ListUsersInGroup(int groupID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    su.sys_user_id,
    su.user_name,
    TRIM(CONCAT(COALESCE(c.last_name, ''), ', ', COALESCE(c.first_name, ''), ' ', COALESCE(c.organization, ''))) as full_name,
    case when 0 < (select count(sg.sys_group_id)
    from
        sys_group sg inner join sys_group_permission_map sgpm
            on sg.sys_group_id = sgpm.sys_group_id
        inner join sys_group_user_map sgum
            on sg.sys_group_id = sgum.sys_group_id
            and sgum.sys_user_id = su.sys_user_id
    where
        sg.group_tag = 'ADMINS'
    ) then 'Y' else 'N' end as is_admin,
    su.is_enabled,
    sl.title as language_name
from
    sys_user su inner join sys_group_user_map sgum
        on su.sys_user_id = sgum.sys_user_id
    left join cooperator c
        on su.cooperator_id = c.cooperator_id
    left join sys_lang sl
        on c.sys_lang_id = sl.sys_lang_id
where
    coalesce(:groupid, sgum.sys_group_id) = sgum.sys_group_id
order by
    su.user_name
", dsReturn, "list_users_in_group", new DataParameters(":groupid", groupID < 0 ? null : (int?)groupID, DbType.Int32));
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet EnablePermission(int permID) {
            return changePermissionEnabled(permID, true);
        }

        public DataSet DisablePermission(int permID) {
            return changePermissionEnabled(permID, false);
        }

        private DataSet changePermissionEnabled(int permID, bool enabled) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    //if (!CanUpdate(dm, "sys_user", null)) {
                    //    throw Library.DataPermissionException(this.UserName, "sys_user", Permission.Update, null);
                    //}

                    checkUserHasAdminEnabled();

                    dm.Write(@"
update 
	sys_permission
set 
	is_enabled = :isenabled,
	modified_date = :modifieddate,
	modified_by = :modifiedby
where 
	sys_permission_id = :permid
",
                            new DataParameters(
                            new DataParameter(":isenabled", (enabled ? "Y" : "N")),
                            new DataParameter(":modifieddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2),
                            new DataParameter(":modifiedby", CooperatorID, DbType.Int32),
                            new DataParameter(":permid", permID, DbType.Int32)));

                    if (enabled) {
                        log("enabled perm " + permID);
                    } else {
                        log("disabled perm " + permID);
                    }
                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);


            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public int SavePermission(int permID, string code, string name, string description, int dataviewID, int tableID, string createPerm, string readPerm, string updatePerm, string deletePerm, bool isEnabled) {

            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (permID < 0) {
                        permID = dm.Write(@"
insert into sys_permission
(permission_tag, sys_dataview_id, sys_table_id, create_permission, read_permission, update_permission, delete_permission, is_enabled, created_date, created_by, owned_date, owned_by)
values
(:code, :dvid, :tblid, :createperm, :readperm, :updateperm, :deleteperm, :enabled, :now1, :coop1, :now2, :coop2)
", true, "sys_permission_id", new DataParameters(
      ":code", code,
      ":dvid", dataviewID < 0 ? null : (int?)dataviewID, DbType.Int32,
      ":tblid", tableID < 0 ? null : (int?)tableID, DbType.Int32,
      ":createperm", (createPerm + "D").Substring(0, 1),
      ":readperm", (readPerm + "D").Substring(0, 1),
      ":updateperm", (updatePerm + "D").Substring(0, 1),
      ":deleteperm", (deletePerm + "D").Substring(0, 1),
      ":enabled", isEnabled ? "Y" : "N",
      ":now1", DateTime.UtcNow, DbType.DateTime2,
      ":coop1", this.CooperatorID, DbType.Int32,
      ":now2", DateTime.UtcNow, DbType.DateTime2,
      ":coop2", this.CooperatorID, DbType.Int32));
                    } else {
                        dm.Write(@"
update
    sys_permission
set
    permission_tag = :code,
    sys_dataview_id = :dvid,
    sys_table_id = :tblid,
    create_permission = :createperm,
    read_permission = :readperm,
    update_permission = :updateperm,
    delete_permission = :deleteperm,
    is_enabled = :enabled,
    modified_date = :now,
    modified_by = :coop
where
    sys_permission_id = :id
", new DataParameters(
     ":code", code,
     ":dvid", dataviewID < 0 ? null : (int?)dataviewID, DbType.Int32,
     ":tblid", tableID < 0 ? null : (int?)tableID, DbType.Int32,
     ":createperm", (createPerm + "D").Substring(0, 1),
     ":readperm", (readPerm + "D").Substring(0, 1),
     ":updateperm", (updatePerm + "D").Substring(0, 1),
     ":deleteperm", (deletePerm + "D").Substring(0, 1),
     ":enabled", isEnabled ? "Y" : "N",
     ":now", DateTime.UtcNow, DbType.DateTime2,
     ":coop", this.CooperatorID, DbType.Int32,
     ":id", permID, DbType.Int32));
                    }


                    var permLangID = Toolkit.ToInt32(dm.ReadValue("select sys_permission_lang_id from sys_permission_lang where sys_permission_id = :permid and sys_lang_id = :langid", new DataParameters(":permid", permID, DbType.Int32, ":langid", LanguageID, DbType.Int32)), -1);

                    if (permLangID < 0) {
                        permLangID = dm.Write(@"
insert into sys_permission_lang
(sys_permission_id, sys_lang_id, title, description, created_date, created_by, owned_date, owned_by)
values
(:permid, :langid, :title, :description, :now1, :coop1, :now2, :coop2)
", true, "sys_permission_lang_id", new DataParameters(
     ":permid", permID, DbType.Int32,
     ":langid", LanguageID, DbType.Int32,
      ":title", name,
      ":description", description,
      ":now1", DateTime.UtcNow, DbType.DateTime2,
      ":coop1", this.CooperatorID, DbType.Int32,
      ":now2", DateTime.UtcNow, DbType.DateTime2,
      ":coop2", this.CooperatorID, DbType.Int32));

                    } else {
                        dm.Write(@"
update
    sys_permission_lang
set
    title = :title,
    description = :description,
    modified_date = :now,
    modified_by = :coop
where
    sys_permission_lang_id = :id
", new DataParameters(
     ":title", name,
     ":description", description,
     ":now", DateTime.UtcNow, DbType.DateTime2,
     ":coop", this.CooperatorID, DbType.Int32,
     ":id", permLangID, DbType.Int32));
                    }

                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);


                return permID;

            } catch (Exception ex) {
                LogException(ex, null);
                throw ex;
            }
        }

        public DataSet DeletePermissionField(int permFieldID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Write(@"
delete from
    sys_permission_field
where
    sys_permission_field_id = :pfid
", new DataParameters(":pfid", permFieldID, DbType.Int32));

                    clearCache(null);


                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public int SavePermissionField(int permFieldID, int permID, int dataviewFieldID, int tableFieldID, string valueType, string compareOperator, string value, int parentTableFieldID, string parentValueType, string parentCompareOperator, string parentValue, string compareMode) {
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (permFieldID < 0) {
                        permID = dm.Write(@"
insert into sys_permission_field
(sys_permission_id, sys_dataview_field_id, sys_table_field_id, field_type, compare_operator, compare_value, parent_table_field_id, parent_field_type, parent_compare_operator, parent_compare_value, compare_mode, created_date, created_by, owned_date, owned_by)
values
(:permid, :dvfid, :tblfid, :valuetype, :compareoperator, :value, :parenttblfid, :parentvaluetype, :parentcompare, :parentvalue, :comparemode, :now1, :coop1, :now2, :coop2)
", true, "sys_permission_field_id", new DataParameters(
    ":permid", permID, DbType.Int32,
    ":dvfid", (dataviewFieldID < 0 ? null : (int?)dataviewFieldID), DbType.Int32,
    ":tblfid", (tableFieldID < 0 ? null : (int?)tableFieldID), DbType.Int32,
    ":valuetype", valueType,
    ":compareoperator", compareOperator,
    ":value", value,
    ":parenttblfid", (parentTableFieldID < 0 ? null : (int?)parentTableFieldID), DbType.Int32,
    ":parentvaluetype", parentValueType,
    ":parentcompare", parentCompareOperator,
    ":parentvalue", parentValue,
    ":comparemode", compareMode,
    ":now1", DateTime.UtcNow, DbType.DateTime2,
    ":coop1", this.CooperatorID, DbType.Int32,
    ":now2", DateTime.UtcNow, DbType.DateTime2,
    ":coop2", this.CooperatorID, DbType.Int32
         ));
                    } else {
                        dm.Write(@"
update
    sys_permission_field
set
    sys_permission_id = :permid,
    sys_dataview_field_id = :dvfid,
    sys_table_field_id = :tblfid,
    field_type = :valuetype,
    compare_operator = :compareoperator,
    compare_value = :value,
    parent_table_field_id = :parenttblfid,
    parent_field_type = :parentvaluetype,
    parent_compare_operator = :parentcompare,
    parent_compare_value = :parentvalue,
    compare_mode = :comparemode,
    modified_date = :now1,
    modified_by = :coop1
where
    sys_permission_field_id = :permfieldid
", new DataParameters(
    ":permid", permID, DbType.Int32,
    ":dvfid", (dataviewFieldID < 0 ? null : (int?)dataviewFieldID), DbType.Int32,
    ":tblfid", (tableFieldID < 0 ? null : (int?)tableFieldID), DbType.Int32,
    ":valuetype", valueType,
    ":compareoperator", compareOperator,
    ":value", value,
    ":parenttblfid", (parentTableFieldID < 0 ? null : (int?)parentTableFieldID), DbType.Int32,
    ":parentvaluetype", parentValueType,
    ":parentcompare", parentCompareOperator,
    ":parentvalue", parentValue,
    ":comparemode", compareMode,
    ":now1", DateTime.UtcNow, DbType.DateTime2,
    ":coop1", this.CooperatorID, DbType.Int32,
    ":permfieldid", permFieldID, DbType.Int32
     ));
                    }

                    clearCache(null);
                    return permID;

                }
            } catch (Exception ex) {
                LogException(ex, null);
                throw ex;
            }
        }


        public int SaveTableRelationship(int relationshipID, int fromFieldID, string relationshipTypeCode, int toFieldID) {
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (relationshipID < 0) {
                        relationshipID = dm.Write(@"
insert into sys_table_relationship
(sys_table_field_id, relationship_type_tag, other_table_field_id, created_date, created_by, owned_date, owned_by)
values
(:fieldID, :typeCode, :otherFieldID, :now1, :coop1, :now2, :coop2)
", true, "sys_table_field_id", new DataParameters(
    ":fieldID", fromFieldID, DbType.Int32,
    ":typeCode", relationshipTypeCode,
    ":otherFieldID", toFieldID, DbType.Int32,
    ":now1", DateTime.UtcNow, DbType.DateTime2,
    ":coop1", this.CooperatorID, DbType.Int32,
    ":now2", DateTime.UtcNow, DbType.DateTime2,
    ":coop2", this.CooperatorID, DbType.Int32
         ));
                    } else {
                        dm.Write(@"
update
    sys_table_relationship
set
    sys_table_field_id = :fieldID,
    relationship_type_tag = :typeCode,
    other_table_field_id = :otherFieldID,
    modified_date = :now1,
    modified_by = :coop1
where
    sys_table_relationship_id = :relationshipID
", new DataParameters(
    ":fieldID", fromFieldID, DbType.Int32,
    ":typeCode", relationshipTypeCode,
    ":otherFieldID", toFieldID, DbType.Int32,
    ":now1", DateTime.UtcNow, DbType.DateTime2,
    ":coop1", this.CooperatorID, DbType.Int32,
    ":relationshipID", relationshipID, DbType.Int32
     ));
                    }

                    clearCache(null);
                    return relationshipID;

                }
            } catch (Exception ex) {
                LogException(ex, null);
                throw ex;
            }
        }


        public DataSet ListPermissionFields(int permID, int permFieldID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    spf.sys_permission_field_id
      ,spf.sys_permission_id
      ,spf.sys_dataview_field_id
      ,spf.sys_table_field_id
      ,spf.field_type
      ,spf.compare_operator
      ,spf.compare_value
      ,spf.parent_table_field_id
      ,spf.parent_field_type
      ,spf.parent_compare_operator
      ,spf.parent_compare_value
      ,spf.compare_mode
      ,spf.created_date
      ,spf.created_by
      ,spf.modified_date
      ,spf.modified_by
      ,spf.owned_date
      ,spf.owned_by,
    sd.sys_dataview_id,
    sd.dataview_name,
    sdf.field_name as dataview_field_name,
    st.sys_table_id,
    st.table_name,
    stf.field_name as table_field_name,
    st2.sys_table_id as parent_table_id,
    st2.table_name as parent_table_name,
    stf2.sys_table_field_id as parent_table_field_id,
    stf2.field_name as parent_table_field_name
from
    sys_permission_field spf left join sys_dataview_field sdf
        on spf.sys_dataview_field_id = sdf.sys_dataview_field_id
    left join sys_dataview sd
        on sdf.sys_dataview_id = sd.sys_dataview_id
    left join sys_table_field stf
        on spf.sys_table_field_id = stf.sys_table_field_id
    left join sys_table st
        on stf.sys_table_id = st.sys_table_id
    left join sys_table_field stf2
        on spf.parent_table_field_id = stf2.sys_table_field_id
    left join sys_table st2
        on stf2.sys_table_id = st2.sys_table_id
where
    coalesce(:permid, spf.sys_permission_id) = spf.sys_permission_id
    and coalesce(:permfieldid, spf.sys_permission_field_id) = spf.sys_permission_field_id
order by
    sd.dataview_name, st.table_name
", dsReturn, "list_permission_fields", new DataParameters(":permid", (permID < 0 ? null : (int?)permID), DbType.Int32, ":permfieldid", (permFieldID < 0 ? null : (int?)permFieldID), DbType.Int32));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }


        public DataSet ListPermissions(int permID, List<int> excludePermIDs, bool enabledOnly) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    sp.sys_permission_id
      ,sp.sys_dataview_id
      ,sp.sys_table_id
      ,sp.permission_tag
      ,coalesce(case when spl.title = '' then null else spl.title end, sp.permission_tag) as display_member
      ,spl.title
      ,spl.description
      ,sp.is_enabled
      ,sp.create_permission
      ,sp.read_permission
      ,sp.update_permission
      ,sp.delete_permission
      ,sp.created_date
      ,sp.created_by
      ,sp.modified_date
      ,sp.modified_by
      ,sp.owned_date
      ,sp.owned_by,
    case sp.create_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as create_permission_text,
    case sp.read_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as read_permission_text,
    case sp.update_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as update_permission_text,
    case sp.delete_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as delete_permission_text,
    sd.dataview_name,
    st.table_name
from
    sys_permission sp left join sys_permission_lang spl
        on sp.sys_permission_id = spl.sys_permission_id
        and spl.sys_lang_id = :langid
    left join sys_dataview sd
        on sp.sys_dataview_id = sd.sys_dataview_id
    left join sys_table st
        on sp.sys_table_id = st.sys_table_id
where
    coalesce(:permid, sp.sys_permission_id) = sp.sys_permission_id
    and coalesce(:enabled, sp.is_enabled) = sp.is_enabled
    and sp.sys_permission_id NOT in (:excludes)
order by
    spl.title
", dsReturn, "list_permissions", new DataParameters(
     ":langid", LanguageID, DbType.Int32,
     ":permid", (permID < 0 ? null : (int?)permID), DbType.Int32,
     ":enabled", (enabledOnly ? "Y" : null),
    ":excludes", excludePermIDs, DbPseudoType.IntegerCollection));


                    dm.Read(@"
select
spf.sys_permission_field_id
      ,spf.sys_permission_id
      ,spf.sys_dataview_field_id
      ,spf.sys_table_field_id
      ,spf.field_type
      ,spf.compare_operator
      ,spf.compare_value
      ,spf.parent_table_field_id
      ,spf.parent_field_type
      ,spf.parent_compare_operator
      ,spf.parent_compare_value
      ,spf.compare_mode
      ,spf.created_date
      ,spf.created_by
      ,spf.modified_date
      ,spf.modified_by
      ,spf.owned_date
      ,spf.owned_by,
stf.sys_table_id,
    st.table_name,
    stf.field_name as table_field_name,
    sdf.sys_dataview_id,
    sd.dataview_name,
    sdf.field_name as dataview_field_name,
    stparent.table_name as parent_table_name,
    stfparent.field_name as parent_table_field_name
from
    sys_permission_field spf inner join sys_permission sp
        on spf.sys_permission_id = sp.sys_permission_id
    left join sys_dataview_field sdf
        on spf.sys_dataview_field_id = sdf.sys_dataview_field_id
    left join sys_dataview sd
        on sdf.sys_dataview_id = sd.sys_dataview_id
    left join sys_table_field stf
        on spf.sys_table_field_id = stf.sys_table_field_id
    left join sys_table st
        on stf.sys_table_id = st.sys_table_id
    left join sys_table_field stfparent
        on spf.parent_table_field_id = stfparent.sys_table_field_id
    left join sys_table stparent
        on stfparent.sys_table_id = stparent.sys_table_id
where
    coalesce(:permid, sp.sys_permission_id) = sp.sys_permission_id
    and coalesce(:enabled, sp.is_enabled) = sp.is_enabled
    and sp.sys_permission_id NOT in (:excludes)
", dsReturn, "list_permission_fields", new DataParameters(
     ":permid", (permID < 0 ? null : (int?)permID), DbType.Int32,
     ":enabled", (enabledOnly ? "Y" : null),
     ":excludes", excludePermIDs, DbPseudoType.IntegerCollection));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet DeletePermission(int permID, bool forceDelete) {
            DataSet dsReturn = createReturnDataSet();
            try {

                checkUserHasAdminEnabled();

                using (DataManager dm = BeginProcessing(true)) {

                    if (!forceDelete) {

                        var sb = new StringBuilder();
                        var dt = dm.Read(@"
select 
    concat('Group: ', coalesce(case when sgl.title = '' then null else sgl.title end, sg.group_tag, '')) as belongs_to
     
from
    sys_group sg inner join sys_group_permission_map sgpm
        on sg.sys_group_id = sgpm.sys_group_id
    left join sys_group_lang sgl
        on sg.sys_group_id = sgl.sys_group_id
        and sgl.sys_lang_id = :langid
where
    sgpm.sys_permission_id = :perm1
UNION
select 
    concat('User: ', su.user_name) as belongs_to
from
    sys_user su inner join sys_user_permission_map supm
        on su.sys_user_id = supm.sys_permission_id
where
    supm.sys_permission_id = :perm2
order by
    1
", new DataParameters(
     ":langid", LanguageID, DbType.Int32,
     ":perm1", permID, DbType.Int32,
     ":perm2", permID, DbType.Int32));

                        if (dt.Rows.Count > 0) {
                            var belongsTo = dt.ListColumnValues("belongs_to", false);
                            throw Library.CreateBusinessException(getDisplayMember("DeletePermission{referencinggroupsusers}", "The following group(s) or user(s) are referencing that permission:\r\n{0}", String.Join(", ", belongsTo.ToArray())));
                        }

                    }


                    dm.BeginTran();

                    dm.Write(@"delete from sys_group_permission_map where sys_permission_id = :permid", new DataParameters(":permid", permID, DbType.Int32));
                    dm.Write(@"delete from sys_user_permission_map where sys_permission_id = :permid", new DataParameters(":permid", permID, DbType.Int32));
                    // dm.Write(@"delete from sys_perm_template_map where sys_permission_id = :permid", new DataParameters(":permid", permID, DbType.Int32));
                    dm.Write(@"delete from sys_permission_field where sys_permission_id = :permid", new DataParameters(":permid", permID, DbType.Int32));
                    dm.Write(@"delete from sys_permission_lang where sys_permission_id = :permid", new DataParameters(":permid", permID, DbType.Int32));
                    dm.Write(@"delete from sys_permission where sys_permission_id = :permid", new DataParameters(":permid", permID, DbType.Int32));

                    dm.Commit();
                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }


        public DataSet DeleteTableFieldMapping(int fieldID, bool forceDelete) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    // see if any references are pointing to it, report them nicely if needed.
                    var dt = dm.Read(@"
select
    sd.dataview_name,
    sdf.field_name,
    sdf.sys_dataview_id,
    sdf.sys_dataview_field_id
from
    sys_dataview sd left join sys_dataview_field sdf
        on sd.sys_dataview_id = sdf.sys_dataview_id
where
    sdf.sys_table_field_id = :id"
                        , new DataParameters(":id", fieldID, DbType.Int32));

                    var sb = new StringBuilder();
                    if (dt.Rows.Count > 0) {
                        if (!forceDelete) {
                            sb.AppendLine("Dataview(s) which reference this field:");
                            foreach (DataRow dr in dt.Rows) {
                                sb.AppendLine(" - " + dr["dataview_name"] + "." + dr["field_name"]);
                            }
                            sb.AppendLine();
                        } else {
                            foreach (DataRow dr in dt.Rows) {
                                // disassociate the sys_dataview_field record from this sys_table_field record
                                dm.Write(@"
update
    sys_dataview_field
set
    gui_hint = null,
    sys_table_field_id = null,
    modified_date = :now,
    modified_by = :who
where
    sys_dataview_field_id = :dvfid
", new DataParameters(":now", DateTime.UtcNow, DbType.DateTime2,
     ":who", CooperatorID, DbType.Int32,
     ":dvfid", Toolkit.ToInt32(dr["sys_dataview_field_id"], -1), DbType.Int32));
                            }

                        }
                    }

                    dt = dm.Read(@"
select
    si.index_name,
    si.sys_index_id,
    sif.sys_index_field_id
from
    sys_index si inner join sys_index_field sif
        on si.sys_index_id = sif.sys_index_id
where
    sif.sys_table_field_id = :id
", new DataParameters(":id", fieldID, DbType.Int32));

                    if (dt.Rows.Count > 0) {
                        if (!forceDelete) {
                            sb.AppendLine("Index(es) which reference this field:");
                            foreach (DataRow dr in dt.Rows) {
                                sb.AppendLine(" - " + dr["index_name"]);
                            }
                            sb.AppendLine();
                        } else {
                            foreach (DataRow dr in dt.Rows) {
                                // the index must no longer be valid if we're dropping one of the fields in it.
                                // so remove the index mapping entirely (not a field mapping within that index mapping)
                                dm.Write(@"
delete from
    sys_index_field
where
    sys_index_id = :sid
", new DataParameters(
     ":sid", Toolkit.ToInt32(dr["sys_index_id"], -1), DbType.Int32));

                                dm.Write(@"
delete from
    sys_index
where
    sys_index_id = :sid
", new DataParameters(
":sid", Toolkit.ToInt32(dr["sys_index_id"], -1), DbType.Int32));


                            }
                        }
                    }

                    dt = dm.Read(@"
select
    stf1.sys_table_field_id as sys_table_field_id_1,
    st1.table_name as table_name1,
    stf1.field_name as field_name1,
    str.relationship_type_tag,
    stf2.sys_table_field_id as sys_table_field_id_2,
    st2.table_name as table_name2,
    stf2.field_name as field_name2
from
    sys_table_relationship str inner join sys_table_field stf1
        on str.sys_table_field_id = stf1.sys_table_field_id
    inner join sys_table st1
        on stf1.sys_table_id = st1.sys_table_id
    inner join sys_table_field stf2
        on str.other_table_field_id = stf2.sys_table_field_id
    inner join sys_table st2
        on stf2.sys_table_id = st2.sys_table_id
where
    str.sys_table_field_id = :id or str.other_table_field_id = :id2
", new DataParameters(":id", fieldID, DbType.Int32, ":id2", fieldID, DbType.Int32));

                    if (dt.Rows.Count > 0) {
                        if (!forceDelete) {
                            sb.AppendLine("Relationship(s) which reference this field:");
                            foreach (DataRow dr in dt.Rows) {
                                sb.AppendLine(" - " + dr["table_name1"] + "." + dr["field_name1"] + " -> " + dr["table_name2"] + "." + dr["field_name2"]);
                            }
                            sb.AppendLine();
                        } else {
                            foreach (DataRow dr in dt.Rows) {
                                // disassociate the sys_index_field record from this sys_table_field record
                                dm.Write(@"
delete from
    sys_table_relationship
where
    sys_table_field_id = :fid1
    or other_table_field_id = :fid2
", new DataParameters(
     ":fid1", Toolkit.ToInt32(dr["sys_table_field_id_1"], -1), DbType.Int32,
     ":fid2", Toolkit.ToInt32(dr["sys_table_field_id_2"], -1), DbType.Int32
     ));
                            }
                        }
                    }

                    if (sb.Length > 0) {
                        throw new InvalidOperationException(getDisplayMember("DeleteTableFieldMapping", "Cannot delete field mapping.\r\n\r\n{0}", sb.ToString()));
                    }



                    if (fieldID > 0) {

                        // delete any language-specific data
                        dm.Write(@"
delete from
    sys_table_field_lang
where
    sys_table_field_id = :id
", new DataParameters(":id", fieldID, DbType.Int32));

                        // delete the field itself
                        dm.Write(@"
delete from
    sys_table_field
where
    sys_table_field_id = :id
", new DataParameters(":id", fieldID, DbType.Int32));
                    }

                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet DeleteTableRelationship(int relationshipID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (relationshipID > 0) {
                        dm.Write(@"
delete from
    sys_table_relationship
where
    sys_table_relationship_id = :id
", new DataParameters(":id", relationshipID, DbType.Int32));
                    }

                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet DeleteTableIndex(int indexID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (indexID > 0) {
                        dm.Write(@"
delete from
    sys_index_field
where
    sys_index_id = :id
", new DataParameters(":id", indexID, DbType.Int32));

                        dm.Write(@"
delete from
    sys_index
where
    sys_index_id = :id
", new DataParameters(":id", indexID, DbType.Int32));
                    }

                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public int UpdateTableMapping(int tableID, string databaseArea, bool enabled) {
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Write(@"
update
    sys_table
set
    database_area_code = :dbarea,
    is_enabled = :enabled,
    modified_date = :now,
    modified_by = :coop
where
    sys_table_id = :id
", new DataParameters(":dbarea", databaseArea, ":enabled", enabled ? "Y" : "N",
 ":now", DateTime.UtcNow, DbType.DateTime2,
 ":coop", this.CooperatorID, DbType.Int32,
 ":id", tableID, DbType.Int32));

                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);


                return tableID;

            } catch (Exception ex) {
                LogException(ex, null);
                throw ex;
            }
        }

        public int SaveTableFieldMapping(int fieldID, int tableID, string fieldName, string purpose, string type, string defaultValue, bool isPrimaryKey,
            bool isForeignKey, int foreignKeyTableFieldID, string foreignKeyDataview, bool isNullable, string guiHint, bool isReadOnly,
            int minLength, int maxLength, int precision, int scale, bool autoIncrement, string groupName, DataTable languageInfo) {

            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (fieldID < 0) {
                        fieldID = dm.Write(@"
insert into sys_table_field
(sys_table_id, field_name, field_purpose, field_type, default_value, is_primary_key, is_foreign_key, foreign_key_table_field_id, foreign_key_dataview_name, is_nullable, gui_hint, is_readonly,
min_length, max_length, numeric_precision, numeric_scale, is_autoincrement, group_name, created_date, created_by, owned_date, owned_by)
values
(:tblid, :fieldname, :fieldpurpose, :fieldtype, :defaultvalue, :pk, :fk, :fktfid, :fkdvname, :nullable, :guihint, :readonly,
:minlength, :maxlength, :numericprecision, :numericscale, :autoincrement, :groupname, :now1, :coop1, :now2, :coop2)
", true, "sys_table_field_id", new DataParameters(
     ":tblid", tableID, DbType.Int32,
     ":fieldname", fieldName,
     ":fieldpurpose", purpose,
     ":fieldtype", type,
     ":defaultvalue", defaultValue,
     ":pk", isPrimaryKey ? "Y" : "N",
     ":fk", isForeignKey ? "Y" : "N",
     ":fktfid", foreignKeyTableFieldID < 0 ? null : (int?)foreignKeyTableFieldID, DbType.Int32,
     ":fkdvname", foreignKeyDataview,
     ":nullable", isNullable ? "Y" : "N",
     ":guihint", guiHint,
     ":readonly", isReadOnly ? "Y" : "N",
     ":minlength", minLength, DbType.Int32,
     ":maxlength", maxLength, DbType.Int32,
     ":numericprecision", precision, DbType.Int32,
     ":numericscale", scale, DbType.Int32,
     ":autoincrement", autoIncrement ? "Y" : "N",
     ":groupname", groupName,
      ":now1", DateTime.UtcNow, DbType.DateTime2,
      ":coop1", this.CooperatorID, DbType.Int32,
      ":now2", DateTime.UtcNow, DbType.DateTime2,
      ":coop2", this.CooperatorID, DbType.Int32));
                    } else {
                        dm.Write(@"
update
    sys_table_field
set
    sys_table_id = :tblid,
    field_name = :fieldname, 
    field_purpose = :fieldpurpose,
    field_type = :fieldtype,
    default_value = :defaultvalue,
    is_primary_key = :pk, 
    is_foreign_key = :fk,
    foreign_key_table_field_id = :fktfid, 
    foreign_key_dataview_name = :fkdvname, 
    is_nullable = :nullable, 
    gui_hint = :guihint, 
    is_readonly = :readonly,
    min_length = :minlength,
    max_length = :maxlength,
    numeric_precision = :numericprecision, 
    numeric_scale = :numericscale, 
    is_autoincrement = :autoincrement, 
    group_name = :groupName, 
    modified_date = :now,
    modified_by = :coop
where
    sys_table_field_id = :id
", new DataParameters(
     ":tblid", tableID, DbType.Int32,
     ":fieldname", fieldName,
     ":fieldpurpose", purpose,
     ":fieldtype", type,
     ":defaultvalue", defaultValue,
     ":pk", isPrimaryKey ? "Y" : "N",
     ":fk", isForeignKey ? "Y" : "N",
     ":fktfid", foreignKeyTableFieldID < 0 ? null : (int?)foreignKeyTableFieldID, DbType.Int32,
     ":fkdvname", foreignKeyDataview,
     ":nullable", isNullable ? "Y" : "N",
     ":guihint", guiHint,
     ":readonly", isReadOnly ? "Y" : "N",
     ":minlength", minLength, DbType.Int32,
     ":maxlength", maxLength, DbType.Int32,
     ":numericprecision", precision, DbType.Int32,
     ":numericscale", scale, DbType.Int32,
     ":autoincrement", autoIncrement ? "Y" : "N",
     ":groupName", String.IsNullOrEmpty(groupName) ? null : groupName,
     ":now", DateTime.UtcNow, DbType.DateTime2,
     ":coop", this.CooperatorID, DbType.Int32,
     ":id", fieldID, DbType.Int32));
                    }

                    // add any lagnuage data 
                    if (languageInfo != null && languageInfo.Rows.Count > 0) {
                        foreach (DataRow lang in languageInfo.Rows) {

                            var fieldLangID = Toolkit.ToInt32(lang["sys_table_field_lang_id"], -1);
                            var langID = Toolkit.ToInt32(lang["sys_lang_id"], -1);
                            var title = lang["title"].ToString();
                            var description = lang["description"].ToString();

                            if (title.Length > 0) {
                                if (fieldLangID < 0) {
                                    // insert
                                    dm.Write(@"
insert into
    sys_table_field_lang
(sys_table_field_id, sys_lang_id, title, description, created_by, created_date, owned_by, owned_date)
values
(:fieldid, :langid, :title, :description, :who1, :now1, :who2, :now2)
", new DataParameters(":fieldid", fieldID, DbType.Int32,
             ":langid", langID, DbType.Int32,
             ":title", title, DbType.String,
             ":description", description, DbType.String,
             ":who1", this.CooperatorID, DbType.Int32,
             ":now1", DateTime.UtcNow, DbType.DateTime2,
             ":who2", this.CooperatorID, DbType.Int32,
             ":now2", DateTime.UtcNow, DbType.DateTime2));

                                } else {
                                    // update
                                    dm.Write(@"
update
    sys_table_field_lang
set
    title = :title,
    description = :description,
    modified_by = :who1,
    modified_date = :now1
where
    sys_table_field_lang_id = :fieldlangid
", new DataParameters(":title", title, DbType.String,
             ":description", description, DbType.String,
             ":who1", this.CooperatorID, DbType.Int32,
             ":now1", DateTime.UtcNow, DbType.DateTime2,
             ":fieldlangid", fieldLangID, DbType.Int32));
                                }
                            }
                        }
                    }

                }


                // clear all caches to make sure our changes are immediately visible
                clearCache(null);


                return fieldID;

            } catch (Exception ex) {
                LogException(ex, null);
                throw ex;
            }
        }


        public int SavePermissionTemplate(int permTemplateID, string name, string description) {

            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (permTemplateID < 0) {
                        permTemplateID = dm.Write(@"
insert into sys_perm_template
(template_name, description, is_enabled, created_date, created_by, owned_date, owned_by)
values
(:name, :description, 'Y', :now1, :coop1, :now2, :coop2)
", true, "sys_perm_template_id", new DataParameters(
      ":name", name,
      ":description", description,
      ":now1", DateTime.UtcNow, DbType.DateTime2,
      ":coop1", this.CooperatorID, DbType.Int32,
      ":now2", DateTime.UtcNow, DbType.DateTime2,
      ":coop2", this.CooperatorID, DbType.Int32));
                    } else {
                        dm.Write(@"
update
    sys_perm_template
set
    template_name = :name,
    description = :description,
    modified_date = :now,
    modified_by = :coop
where
    sys_perm_template_id = :id
", new DataParameters(
     ":name", name,
     ":description", description,
     ":now", DateTime.UtcNow, DbType.DateTime2,
     ":coop", this.CooperatorID, DbType.Int32,
     ":id", permTemplateID, DbType.Int32));
                    }

                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);


                return permTemplateID;

            } catch (Exception ex) {
                LogException(ex, null);
                throw ex;
            }
        }

        public DataSet ListAppResources(string appName, int languageID) {
            var dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    dm.Read(@"
select
    sys_lang_id,
    app_name,
    form_name,
    app_resource_name,
    description,
    display_member,
    value_member,
    sort_order
from
    app_resource
where
    coalesce(app_name,'\b') = coalesce(:appname, app_name, '\b')
    and sys_lang_id = coalesce(:langid, sys_lang_id)
order by
    app_name,
    form_name,
    app_resource_name,
    sort_order
", dsReturn, "list_app_resources", new DataParameters(":appname", appName, DbType.String,
     ":langid", (languageID < 0 ? null : (int?)languageID), DbType.Int32));
                }
            } catch (Exception ex) {
                LogException(ex, null);
                throw ex;
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }


        public DataSet ListPermissionTemplates(int permTemplateID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    spt.sys_perm_template_id
      ,spt.template_name
      ,spt.description
      ,spt.is_enabled
      ,spt.created_date
      ,spt.created_by
      ,spt.modified_date
      ,spt.modified_by
      ,spt.owned_date
      ,spt.owned_by
from
    sys_perm_template spt
where
    coalesce(:permtemplateid, spt.sys_perm_template_id) = spt.sys_perm_template_id
order by
    spt.template_name
", dsReturn, "list_permission_templates", new DataParameters(":permtemplateid", (permTemplateID < 0 ? null : (int?)permTemplateID), DbType.Int32));

                    //                    dm.Read(@"
                    //select
                    //    spt.*,
                    //    sp.perm_name
                    //from
                    //    sys_perm_template spt left join sys_perm_template_map sptm
                    //        on spt.sys_perm_template_id = sptm.sys_perm_template_id
                    //    left join sys_permission sp
                    //        on sptm.sys_permission_id = sp.sys_permission_id
                    //where
                    //    coalesce(:permtemplateid, spt.sys_perm_template_id) = spt.sys_perm_template_id
                    //order by
                    //    spt.template_name
                    //", dsReturn, "permissions_by_template", new DataParameters(":permtemplateid", (permTemplateID < 0 ? null : (int?)permTemplateID)));

                    dm.Read(@"
select
    spt.sys_perm_template_id
      ,spt.template_name
      ,spt.description
      ,spt.is_enabled
      ,spt.created_date
      ,spt.created_by
      ,spt.modified_date
      ,spt.modified_by
      ,spt.owned_date
      ,spt.owned_by,
    sp.sys_permission_id,
    sp.perm_name,
    sd.sys_dataview_id,
    sd.dataview_name,
    st.sys_table_id,
    st.table_name,
    coalesce(sp.sys_table_id, 0) * 1000000 + coalesce(sp.sys_dataview_id, 0) as perm_rank,
    case sp.create_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as create_permission_text,
    case sp.read_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as read_permission_text,
    case sp.update_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as update_permission_text,
    case sp.delete_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as delete_permission_text
from
    sys_perm_template spt inner join sys_perm_template_map sptm
        on spt.sys_perm_template_id = sptm.sys_perm_template_id
    inner join sys_permission sp
        on sptm.sys_permission_id = sp.sys_permission_id
    left join sys_dataview sd
        on sp.sys_dataview_id = sd.sys_dataview_id
    left join sys_table st
        on sp.sys_table_id = st.sys_table_id
where
    coalesce(:permtemplateid, spt.sys_perm_template_id) = spt.sys_perm_template_id
    and sp.is_enabled = 'Y'
order by
    spt.template_name,
    perm_rank,
    sp.perm_name
", dsReturn, "permissions_by_template", new DataParameters(":permtemplateid", (permTemplateID < 0 ? null : (int?)permTemplateID), DbType.Int32));

                    dm.Read(@"
select
    spf.sys_permission_field_id
      ,spf.sys_permission_id
      ,spf.sys_dataview_field_id
      ,spf.sys_table_field_id
      ,spf.field_type
      ,spf.compare_operator
      ,spf.compare_value
      ,spf.parent_table_field_id
      ,spf.parent_field_type
      ,spf.parent_compare_operator
      ,spf.parent_compare_value
      ,spf.compare_mode
      ,spf.created_date
      ,spf.created_by
      ,spf.modified_date
      ,spf.modified_by
      ,spf.owned_date
      ,spf.owned_by,
    stf.sys_table_id,
    st.table_name,
    stf.field_name as table_field_name,
    sdf.sys_dataview_id,
    sd.dataview_name,
    sdf.field_name as dataview_field_name,
    stparent.table_name as parent_table_name,
    stfparent.field_name as parent_table_field_name
from
    sys_permission_field spf inner join sys_permission sp
        on spf.sys_permission_id = sp.sys_permission_id
    inner join sys_perm_template_map sptm
        on sp.sys_permission_id = sptm.sys_permission_id
    left join sys_dataview_field sdf
        on spf.sys_dataview_field_id = sdf.sys_dataview_field_id
    left join sys_dataview sd
        on sdf.sys_dataview_id = sd.sys_dataview_id
    left join sys_table_field stf
        on spf.sys_table_field_id = stf.sys_table_field_id
    left join sys_table st
        on stf.sys_table_id = st.sys_table_id
    left join sys_table_field stfparent
        on spf.parent_table_field_id = stfparent.sys_table_field_id
    left join sys_table stparent
        on stfparent.sys_table_id = stparent.sys_table_id
where
    coalesce(:permtemplateid, sptm.sys_perm_template_id) = sptm.sys_perm_template_id 
", dsReturn, "template_perm_field_info", new DataParameters(":permtemplateid", (permTemplateID < 0 ? null : (int?)permTemplateID), DbType.Int32));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet DeletePermissionTemplate(int permTemplateID) {
            DataSet dsReturn = createReturnDataSet();
            try {

                checkUserHasAdminEnabled();

                using (DataManager dm = BeginProcessing(true)) {
                    dm.BeginTran();

                    dm.Write(@"delete from sys_perm_template_map where sys_perm_template_id = :permtemplateid", new DataParameters(":permtemplateid", permTemplateID, DbType.Int32));
                    dm.Write(@"delete from sys_perm_template where sys_perm_template_id = :permtemplateid", new DataParameters(":permtemplateid", permTemplateID, DbType.Int32));

                    dm.Commit();
                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet AddPermissionsToTemplate(int permTemplateID, List<int> permIDs) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (permIDs != null) {
                        foreach (int id in permIDs) {

                            int count = Toolkit.ToInt32(dm.ReadValue("select count(*) from sys_perm_template_map where sys_perm_template_id = :permtemplateid and sys_permission_id = :permid",
                                    new DataParameters(":permtemplateid", permTemplateID, ":permid", id, DbType.Int32)), -1);

                            if (count == 0) {
                                dm.Write(@"
insert into sys_perm_template_map
(sys_perm_template_id, sys_permission_id, created_date, created_by, owned_date, owned_by)
values
(:permtemplateid, :permid, :now1, :coop1, :now2, :coop2)
", new DataParameters(":permtemplateid", permTemplateID, DbType.Int32,
         ":permid", id, DbType.Int32,
         ":now1", DateTime.UtcNow, DbType.DateTime2,
         ":coop1", this.CooperatorID, DbType.Int32,
         ":now2", DateTime.UtcNow, DbType.DateTime2,
         ":coop2", this.CooperatorID, DbType.Int32));
                            }
                        }

                        clearCache("PermissionAssignment");
                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet RemovePermissionsFromTemplate(int permTemplateID, List<int> permIDs) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (permIDs != null && permIDs.Count > 0) {
                        dm.Write(@"
delete from 
    sys_perm_template_map
where
    sys_perm_template_id = :permtemplateid
    and sys_permission_id in (:permids)
", new DataParameters(":permtemplateid", permTemplateID, DbType.Int32,
     ":permids", permIDs, DbPseudoType.IntegerCollection));

                        clearCache("PermissionAssignment");

                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }






























        public DataSet ListGroups(int groupID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    sg.sys_group_id
      , sg.group_tag
      ,coalesce(sgl.title, sg.group_tag) as group_name
      ,sgl.description
      ,sg.is_enabled
      ,sg.created_date
      ,sg.created_by
      ,sg.modified_date
      ,sg.modified_by
      ,sg.owned_date
      ,sg.owned_by
from
    sys_group sg left join sys_group_lang sgl
        on sg.sys_group_id = sgl.sys_group_id
        and sgl.sys_lang_id = :langid
where
    coalesce(:groupid, sg.sys_group_id) = sg.sys_group_id
order by
    coalesce(sgl.title, sg.group_tag)
", dsReturn, "list_groups", new DataParameters(":groupid", (groupID < 0 ? null : (int?)groupID), DbType.Int32,
     ":langid", LanguageID, DbType.Int32));

                    dm.Read(@"
select
    sg.sys_group_id
      ,coalesce(sgl.title, sg.group_tag) as group_name
      ,sgl.description
      ,sg.is_enabled
      ,sg.created_date
      ,sg.created_by
      ,sg.modified_date
      ,sg.modified_by
      ,sg.owned_date
      ,sg.owned_by,
    sp.sys_permission_id,
    sp.is_enabled as perm_is_enabled,
    spl.title,
    coalesce(spl.title, sp.permission_tag) as permission_name,
    sd.sys_dataview_id,
    sd.dataview_name,
    st.sys_table_id,
    st.table_name,
    coalesce(sp.sys_table_id, 0) * 1000000 + coalesce(sp.sys_dataview_id, 0) as perm_rank,
    case sp.create_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as create_permission_text,
    case sp.read_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as read_permission_text,
    case sp.update_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as update_permission_text,
    case sp.delete_permission when 'A' then 'Allow' when 'D' then 'Deny' else 'Inherit' end as delete_permission_text
from
    sys_group sg inner join sys_group_permission_map sgpm
        on sg.sys_group_id = sgpm.sys_group_id
    inner join sys_permission sp
        on sgpm.sys_permission_id = sp.sys_permission_id
    left join sys_permission_lang spl
        on sp.sys_permission_id = spl.sys_permission_id
        and spl.sys_lang_id = :langid1
    left join sys_dataview sd
        on sp.sys_dataview_id = sd.sys_dataview_id
    left join sys_table st
        on sp.sys_table_id = st.sys_table_id
    left join sys_group_lang sgl
        on sg.sys_group_id = sgl.sys_group_id
        and sgl.sys_lang_id = :langid2
where
    coalesce(:groupid, sg.sys_group_id) = sg.sys_group_id
order by
    2,
    perm_rank,
    spl.title
", dsReturn, "permissions_by_group", new DataParameters(
     ":langid1", LanguageID, DbType.Int32,
     ":langid2", LanguageID, DbType.Int32,
     ":groupid", (groupID < 0 ? null : (int?)groupID), DbType.Int32));

                    dm.Read(@"
select 
    su.sys_user_id,
    su.user_name,
    su.is_enabled,
    TRIM(CONCAT(COALESCE(c.last_name, ''), ', ', COALESCE(c.first_name, ''), ' ', COALESCE(c.organization, ''))) as full_name,
    sgum.sys_group_id
from 
    sys_user su inner join sys_group_user_map sgum
        on su.sys_user_id = sgum.sys_user_id
    left join cooperator c
        on su.cooperator_id = c.cooperator_id
where 
    coalesce(:groupid, sgum.sys_group_id) = sgum.sys_group_id
order by
    sgum.sys_group_id,
    su.user_name
", dsReturn, "users_by_group", new DataParameters(":groupid", (groupID < 0 ? null : (int?)groupID), DbType.Int32));

                    dm.Read(@"
select
    spf.sys_permission_field_id
      ,spf.sys_permission_id
      ,spf.sys_dataview_field_id
      ,spf.sys_table_field_id
      ,spf.field_type
      ,spf.compare_operator
      ,spf.compare_value
      ,spf.parent_table_field_id
      ,spf.parent_field_type
      ,spf.parent_compare_operator
      ,spf.parent_compare_value
      ,spf.compare_mode
      ,spf.created_date
      ,spf.created_by
      ,spf.modified_date
      ,spf.modified_by
      ,spf.owned_date
      ,spf.owned_by,
    stf.sys_table_id,
    st.table_name,
    stf.field_name as table_field_name,
    sdf.sys_dataview_id,
    sd.dataview_name,
    sdf.field_name as dataview_field_name,
    stparent.table_name as parent_table_name,
    stfparent.field_name as parent_table_field_name
from
    sys_permission_field spf inner join sys_permission sp
        on spf.sys_permission_id = sp.sys_permission_id
    inner join sys_group_permission_map sgpm
        on sp.sys_permission_id = sgpm.sys_permission_id
    left join sys_dataview_field sdf
        on spf.sys_dataview_field_id = sdf.sys_dataview_field_id
    left join sys_dataview sd
        on sdf.sys_dataview_id = sd.sys_dataview_id
    left join sys_table_field stf
        on spf.sys_table_field_id = stf.sys_table_field_id
    left join sys_table st
        on stf.sys_table_id = st.sys_table_id
    left join sys_table_field stfparent
        on spf.parent_table_field_id = stfparent.sys_table_field_id
    left join sys_table stparent
        on stfparent.sys_table_id = stparent.sys_table_id
where
    coalesce(:groupid, sgpm.sys_group_id) = sgpm.sys_group_id
", dsReturn, "group_perm_field_info", new DataParameters(":groupid", (groupID < 0 ? null : (int?)groupID), DbType.Int32));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public bool IsReservedGroupTagValue(string value) {
            if (String.Compare(value, "admins", true) == 0
                || String.Compare(value, "allusers", true) == 0
                || String.Compare(value, "ctusers", true) == 0
                || String.Compare(value, "feedbackowners", true) == 0
                || String.Compare(value, "feedbacksubmitters", true) == 0
                ) {

                return true;
            } else {
                return false;
            }
        }


        public DataSet DeleteGroup(int groupID) {
            DataSet dsReturn = createReturnDataSet();
            try {

                checkUserHasAdminEnabled();

                using (DataManager dm = BeginProcessing(true)) {

                    var dtGroup = GetGroupInfo(groupID).Tables["group_info"];

                    if (dtGroup.Rows.Count == 1) {
                        if (IsReservedGroupTagValue(dtGroup.Rows[0]["group_tag"].ToString())) {
                            if (String.Compare(dtGroup.Rows[0]["group_tag"].ToString(), "allusers", true) == 0) {
                                throw Library.CreateBusinessException(getDisplayMember("DeleteGroup{reservedallusers}", "This is a reserved group and can not be deleted, nor can users be removed from it."));
                            } else {
                                throw Library.CreateBusinessException(getDisplayMember("DeleteGroup{reserved}", "This is a reserved group and can not be deleted.  However, you may remove all users from it."));
                            }
                        }
                    }

                    dm.BeginTran();

                    dm.Write(@"delete from sys_group_permission_map where sys_group_id = :groupid", new DataParameters(":groupid", groupID, DbType.Int32));
                    dm.Write(@"delete from sys_group_user_map where sys_group_id = :groupid", new DataParameters(":groupid", groupID, DbType.Int32));
                    dm.Write(@"delete from sys_group_lang where sys_group_id = :groupid", new DataParameters(":groupid", groupID, DbType.Int32));
                    dm.Write(@"delete from sys_group where sys_group_id = :groupid", new DataParameters(":groupid", groupID, DbType.Int32));

                    dm.Commit();
                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet AddPermissionsToGroup(int groupID, List<int> permIDs) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (permIDs != null) {
                        foreach (int id in permIDs) {

                            int count = Toolkit.ToInt32(dm.ReadValue("select count(*) from sys_group_permission_map where sys_group_id = :groupid and sys_permission_id = :permid",
                                    new DataParameters(":groupid", groupID, ":permid", id, DbType.Int32)), -1);

                            if (count == 0) {
                                dm.Write(@"
insert into sys_group_permission_map
(sys_group_id, sys_permission_id, created_date, created_by, owned_date, owned_by)
values
(:groupid, :permid, :now1, :coop1, :now2, :coop2)
", new DataParameters(":groupid", groupID, DbType.Int32,
         ":permid", id, DbType.Int32,
         ":now1", DateTime.UtcNow, DbType.DateTime2,
         ":coop1", this.CooperatorID, DbType.Int32,
         ":now2", DateTime.UtcNow, DbType.DateTime2,
         ":coop2", this.CooperatorID, DbType.Int32));
                            }
                        }

                        clearCache("PermissionAssignment");
                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet RemovePermissionsFromGroup(int groupID, List<int> permIDs) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (permIDs != null && permIDs.Count > 0) {
                        dm.Write(@"
delete from 
    sys_group_permission_map
where
    sys_group_id = :groupid
    and sys_permission_id in (:permids)
", new DataParameters(":groupid", groupID, DbType.Int32,
     ":permids", permIDs, DbPseudoType.IntegerCollection));

                        clearCache("PermissionAssignment");

                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }



        public DataSet AddUsersToGroup(int groupID, List<int> userIDs) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (userIDs != null) {
                        foreach (int id in userIDs) {

                            int count = Toolkit.ToInt32(dm.ReadValue("select count(*) from sys_group_user_map where sys_group_id = :groupid and sys_user_id = :userid",
                                    new DataParameters(":groupid", groupID, ":userid", id, DbType.Int32)), -1);

                            if (count == 0) {
                                dm.Write(@"
insert into sys_group_user_map
(sys_group_id, sys_user_id, created_date, created_by, owned_date, owned_by)
values
(:groupid, :userid, :now1, :coop1, :now2, :coop2)
", new DataParameters(":groupid", groupID, DbType.Int32,
         ":userid", id, DbType.Int32,
         ":now1", DateTime.UtcNow, DbType.DateTime2,
         ":coop1", this.CooperatorID, DbType.Int32,
         ":now2", DateTime.UtcNow, DbType.DateTime2,
         ":coop2", this.CooperatorID, DbType.Int32));
                            }
                        }

                        clearCache("PermissionAssignment");
                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet RemoveUsersFromGroup(int groupID, List<int> userIDs) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    var dtGroup = GetGroupInfo(groupID).Tables["group_info"];

                    if (dtGroup.Rows.Count == 1) {
                        if (String.Compare(dtGroup.Rows[0]["group_tag"].ToString(), "allusers", true) == 0) {
                            throw Library.CreateBusinessException(getDisplayMember("RemoveUsersFromGroup{allusers}", "Users can not be removed from the 'ALLUSERS' group, as it is reserved by the system."));
                        }
                    }

                    if (userIDs != null && userIDs.Count > 0) {

                        var isAdminGroup = false;
                        if (String.Compare(dtGroup.Rows[0]["group_tag"].ToString(), "admins", true) == 0) {
                            isAdminGroup = true;
                        }

                        foreach (var uid in userIDs) {

                            if (isAdminGroup) {
                                // don't let them remove the last user from the admin group!
                                var dt = dm.Read(@"
select
    count(sys_user_id) as cnt,
    sys_user_id
from
    sys_group_user_map
where
    sys_group_id = :groupid
group by
    sys_user_id
order by 
    sys_user_id", new DataParameters(":groupid", groupID, DbType.Int32));

                                if (dt.Rows.Count > 0) {
                                    var dr = dt.Rows[0];
                                    if (Toolkit.ToInt32(dr["cnt"], 0) < 2) {
                                        var su = Toolkit.ToInt32(dr["sys_user_id"], -1);
                                        if (su == uid) {
                                            // this is the last admin user.  don't remove them.
                                            throw Library.CreateBusinessException(getDisplayMember("RemoveUsersFromGroup{lastadmin}", "The ADMINS group must contain at least one user, so the final user was not removed from it."));
                                        }
                                    }
                                }
                            }


                            dm.Write(@"
delete from 
    sys_group_user_map
where
    sys_group_id = :groupid
    and sys_user_id in (:userid)
", new DataParameters(":groupid", groupID, DbType.Int32,
":userid", uid, DbType.Int32));

                        }


                        clearCache("PermissionAssignment");

                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }



        public int SaveGroup(int groupID, string tag, string name, string description) {

            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (groupID < 0) {
                        groupID = dm.Write(@"
insert into sys_group
(group_tag, is_enabled, created_date, created_by, owned_date, owned_by)
values
(:tag, 'Y', :now1, :coop1, :now2, :coop2)
", true, "sys_group_id", new DataParameters(
     ":tag", tag,
      ":now1", DateTime.UtcNow, DbType.DateTime2,
      ":coop1", this.CooperatorID, DbType.Int32,
      ":now2", DateTime.UtcNow, DbType.DateTime2,
      ":coop2", this.CooperatorID, DbType.Int32));
                    } else {
                        dm.Write(@"
update
    sys_group
set
    group_tag = :tag,
    modified_date = :now,
    modified_by = :coop
where
    sys_group_id = :id
", new DataParameters(
     ":tag", tag,
     ":now", DateTime.UtcNow, DbType.DateTime2,
     ":coop", this.CooperatorID, DbType.Int32,
     ":id", groupID, DbType.Int32));
                    }

                    // now update the language-specific portion...
                    var groupLangID = Toolkit.ToInt32(dm.ReadValue("select sys_group_lang_id from sys_group_lang where sys_group_id = :groupid and sys_lang_id = :langid", new DataParameters(":groupid", groupID, DbType.Int32, ":langid", LanguageID, DbType.Int32)), -1);

                    if (groupLangID < 0) {
                        groupLangID = dm.Write(@"
insert into sys_group_lang
(sys_group_id, sys_lang_id, title, description, created_date, created_by, owned_date, owned_by)
values
(:groupid, :langid, :title, :description, :now1, :coop1, :now2, :coop2)
", true, "sys_group_lang_id", new DataParameters(
     ":groupid", groupID, DbType.Int32,
     ":langid", LanguageID, DbType.Int32,
     ":title", name,
     ":description", description,
      ":now1", DateTime.UtcNow, DbType.DateTime2,
      ":coop1", this.CooperatorID, DbType.Int32,
      ":now2", DateTime.UtcNow, DbType.DateTime2,
      ":coop2", this.CooperatorID, DbType.Int32));
                    } else {
                        dm.Write(@"
update
    sys_group_lang
set
    title = :title,
    description = :description,
    modified_date = :now,
    modified_by = :coop
where
    sys_group_lang_id = :id
", new DataParameters(
     ":title", name,
     ":description", description,
     ":now", DateTime.UtcNow, DbType.DateTime2,
     ":coop", this.CooperatorID, DbType.Int32,
     ":id", groupLangID, DbType.Int32));
                    }



                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);


                return groupID;

            } catch (Exception ex) {
                LogException(ex, null);
                throw ex;
            }
        }























        public DataSet ListTableFields(int tableID, int tableFieldID, bool onlyPrimaryKeyFields, bool onlyForeignKeyFields) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    st.table_name,
    st.database_area_code,
    st.is_enabled as table_is_enabled,
stf.sys_table_field_id
      ,stf.sys_table_id
      ,stf.field_name
      ,stf.field_purpose
      ,stf.field_type
      ,stf.default_value
      ,stf.is_primary_key
      ,stf.is_foreign_key
      ,stf.foreign_key_table_field_id
      ,stf.foreign_key_dataview_name
      ,stf.is_nullable
      ,stf.gui_hint
      ,stf.is_readonly
      ,stf.min_length
      ,stf.max_length
      ,stf.numeric_precision
      ,stf.numeric_scale
      ,stf.is_autoincrement
      ,stf.group_name
      ,stf.created_date
      ,stf.created_by
      ,stf.modified_date
      ,stf.modified_by
      ,stf.owned_date
      ,stf.owned_by,
    stf.field_name as table_field_name,
    case when st2.table_name is null then
        null
    else
        concat(st2.table_name, '.', stf2.field_name)
    end as foreign_key_table_field_name,
    stf.foreign_key_dataview_name
from
    sys_table_field stf inner join sys_table st 
        on stf.sys_table_id = st.sys_table_id
    left join sys_table_field stf2
        on stf2.sys_table_field_id = stf.foreign_key_table_field_id
    left join sys_table st2
        on st2.sys_table_id = stf2.sys_table_id
where
    coalesce(:tblid, st.sys_table_id) = st.sys_table_id
    and coalesce(:tblfid, stf.sys_table_field_id) = stf.sys_table_field_id
    and coalesce(stf.is_primary_key,'\b') = coalesce(:ispk, stf.is_primary_key, '\b')
    and coalesce(stf.is_foreign_key, '\b') = coalesce(:isfk, stf.is_foreign_key, '\b')
order by
    st.table_name,
    stf.field_ordinal
"
, dsReturn, "list_table_fields", new DataParameters(
            ":tblid", (tableID < 0 ? null : (int?)tableID), DbType.Int32,
            ":tblfid", (tableFieldID < 0 ? null : (int?)tableFieldID), DbType.Int32,
            ":ispk", (onlyPrimaryKeyFields ? "Y" : null),
            ":isfk", (onlyForeignKeyFields ? "Y" : null)
        ));
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }



        public DataSet ListTableFields(int tableID, int tableFieldID, bool includeRelationshipAndIndexData) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    st.table_name,
    st.database_area_code,
    st.is_enabled as table_is_enabled,
stf.sys_table_field_id
      ,stf.sys_table_id
      ,stf.field_name
      ,stf.field_purpose
      ,stf.field_type
      ,stf.default_value
      ,stf.is_primary_key
      ,stf.is_foreign_key
      ,stf.foreign_key_table_field_id
      ,stf.foreign_key_dataview_name
      ,stf.is_nullable
      ,stf.gui_hint
      ,stf.is_readonly
      ,stf.min_length
      ,stf.max_length
      ,stf.numeric_precision
      ,stf.numeric_scale
      ,stf.is_autoincrement
      ,stf.group_name
      ,stf.created_date
      ,stf.created_by
      ,stf.modified_date
      ,stf.modified_by
      ,stf.owned_date
      ,stf.owned_by,
    stf.field_name as table_field_name,
    case when st2.table_name is null then
        null
    else
        concat(st2.table_name, '.', stf2.field_name)
    end as foreign_key_table_field_name,
    stf.foreign_key_dataview_name
from
    sys_table_field stf inner join sys_table st 
        on stf.sys_table_id = st.sys_table_id
    left join sys_table_field stf2
        on stf2.sys_table_field_id = stf.foreign_key_table_field_id
    left join sys_table st2
        on st2.sys_table_id = stf2.sys_table_id
where
    coalesce(:tblid, st.sys_table_id) = st.sys_table_id
    and coalesce(:tblfid, stf.sys_table_field_id) = stf.sys_table_field_id
order by
    st.table_name,
    stf.field_ordinal
"
, dsReturn, "list_table_fields", new DataParameters(":tblid", (tableID < 0 ? null : (int?)tableID), DbType.Int32, ":tblfid", (tableFieldID < 0 ? null : (int?)tableFieldID), DbType.Int32));



                    // pull language info for given field
                    // NOTE: does NOT pull language info if a specific field is not given!!!

                    if (tableFieldID > -1) {
                        dm.Read(@"
select
    sl.sys_lang_id,
    sl.title as language_title,
    sl.script_direction,
	stfl.sys_table_field_lang_id,
	stf.sys_table_field_id,
    stf.field_ordinal,
    stfl.title,
    stfl.description
from
    sys_lang sl left join sys_table_field_lang stfl
        on sl.sys_lang_id = stfl.sys_lang_id
        and stfl.sys_table_field_id = :tblfid
    left join sys_table_field stf
        on stfl.sys_table_field_id = stf.sys_table_field_id
    left join sys_table st
        on stf.sys_table_id = st.sys_table_id
order by
	sl.title
"
    , dsReturn, "list_table_field_langs", new DataParameters(":tblfid", tableFieldID, DbType.Int32));
                    }





                    if (includeRelationshipAndIndexData) {
                        dm.Read(@"
select
    str.sys_table_relationship_id,
    st1.sys_table_id,
    st1.table_name,
    stf1.sys_table_field_id,
    stf1.field_name,
    str.relationship_type_tag,
    st2.sys_table_id as sys_table_id2,
    st2.table_name as table_name2,
    stf2.field_name as field_name2,
    stf2.sys_table_field_id as sys_table_field_id2
from
    sys_table_relationship str inner join sys_table_field stf1 
        on str.sys_table_field_id = stf1.sys_table_field_id
    inner join sys_table st1 
        on stf1.sys_table_id = st1.sys_table_id
    inner join sys_table_field stf2
        on str.other_table_field_id = stf2.sys_table_field_id
    inner join sys_table st2
        on stf2.sys_table_id = st2.sys_table_id
where
    st1.sys_table_id = coalesce(:tableid, st1.sys_table_id)
order by
    st1.table_name,
    st2.table_name,
    stf1.field_name,
    stf2.field_name
", dsReturn, "list_table_relationships", new DataParameters(
     ":tableid", (tableID < 0 ? null : (int?)tableID), DbType.Int32));
                        //     ":fieldid", (tableFieldID < 0 ? null : (int?)tableFieldID), DbType.Int32));


                        dm.Read(@"
select
    si.sys_index_id,
    si.sys_table_id,
    sif.sys_index_field_id,
    si.is_unique,
    st.table_name,
    si.index_name,
    stf.field_name
from
    sys_index si inner join sys_index_field sif
        on si.sys_index_id = sif.sys_index_id
    inner join sys_table st
        on si.sys_table_id = st.sys_table_id
    inner join sys_table_field stf
        on sif.sys_table_field_id = stf.sys_table_field_id
where
    si.sys_table_id = coalesce(:tableid, si.sys_table_id)
    and sif.sys_table_field_id = coalesce(:fieldid, sif.sys_table_field_id)
order by
    si.index_name,
    sif.sort_order
", dsReturn, "list_table_indexes", new DataParameters(
     ":tableid", (tableID < 0 ? null : (int?)tableID), DbType.Int32,
     ":fieldid", (tableFieldID < 0 ? null : (int?)tableFieldID), DbType.Int32));

                    }

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet ListTableRelationships(int fromTableID, int relationshipID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    str.sys_table_relationship_id,
    st1.sys_table_id,
    st1.table_name,
    stf1.sys_table_field_id,
    stf1.field_name,
    str.relationship_type_tag,
    st2.sys_table_id as sys_table_id2,
    st2.table_name as table_name2,
    stf2.field_name as field_name2,
    stf2.sys_table_field_id as sys_table_field_id2
from
    sys_table_relationship str inner join sys_table_field stf1 
        on str.sys_table_field_id = stf1.sys_table_field_id
    inner join sys_table st1 
        on stf1.sys_table_id = st1.sys_table_id
    inner join sys_table_field stf2
        on str.other_table_field_id = stf2.sys_table_field_id
    inner join sys_table st2
        on stf2.sys_table_id = st2.sys_table_id
where
    st1.sys_table_id = coalesce(:tableid, st1.sys_table_id)
    and str.sys_table_relationship_id = coalesce(:relid, str.sys_table_relationship_id)
order by
    st1.table_name,
    st2.table_name,
    stf1.field_name,
    stf2.field_name
", dsReturn, "list_table_relationships", new DataParameters(
     ":tableid", (fromTableID < 0 ? null : (int?)fromTableID), DbType.Int32,
     ":relid", (relationshipID < 0 ? null : (int?)relationshipID), DbType.Int32));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;

        }


        public DataSet ListDataViewFields(int dataviewID, int dataviewFieldID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    sdf.sys_dataview_field_id
      ,sdf.sys_dataview_id
      ,sdf.field_name
      ,sdf.sys_table_field_id
      ,sdf.is_readonly
      ,sdf.is_primary_key
      ,sdf.is_transform
      ,sdf.sort_order
      ,sdf.foreign_key_dataview_name
      ,sdf.group_name
      ,sdf.created_date
      ,sdf.created_by
      ,sdf.modified_date
      ,sdf.modified_by
      ,sdf.owned_date
      ,sdf.owned_by,
    sdf.field_name as dataview_field_name,
    coalesce(stf.field_type, 'NOT UPDATABLE') as field_type
from
    sys_dataview_field sdf left join sys_table_field stf
        on sdf.sys_table_field_id = stf.sys_table_field_id
where
    coalesce(:dvid, sdf.sys_dataview_id) = sdf.sys_dataview_id
    and coalesce(:dvfid, sdf.sys_dataview_field_id) = sdf.sys_dataview_field_id
order by
    sdf.field_name
", dsReturn, "list_dataview_fields", new DataParameters(":dvid", dataviewID, ":dvfid", (dataviewFieldID < 0 ? null : (int?)dataviewFieldID), DbType.Int32));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet ListDataviewCategories() {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    distinct(category_code) as category_code
from
    sys_dataview
where
    trim(coalesce(category_code, '\b')) != '\b'
order by
    category_code
", dsReturn, "list_dataview_categories");

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet ListDataviewDatabaseAreas() {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    distinct(database_area_code) as database_area_code
from
    sys_dataview
where
    trim(coalesce(database_area_code, '\b')) != '\b'
order by
    database_area_code
", dsReturn, "list_dataview_database_areas");

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public int SaveUser(int userID, string userName, bool enabled, int cooperatorID, int currentCooperatorID, int webCooperatorID, string title, string firstName, string lastName, string job,
            string discipline, string organization, string organizationAbbreviation, int languageID, bool isActive, string addressLine1, string addressLine2, string addressLine3, string city, string postalIndex, string email,
            string primaryPhone, string secondaryPhone, string fax, int siteID, string regionCode, string categoryCode, int geographyID, string note) {
            try {
                using (DataManager dm = BeginProcessing(true)) {

                    dm.BeginTran();

                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();




                    if (userID < 0) {
                        userID = dm.Write(@"
insert into
    sys_user
(user_name, password, is_enabled, cooperator_id, created_date, created_by, owned_date, owned_by)
values
(:username, :pass, :enabled, :coop, :now1, :coop1, :now2, :coop2)
", true, "sys_user_id", new DataParameters(
     ":username", userName,
     ":pass", "-",   // this will prevent a login until actual password is set, as the decryption code will fail on this used as ciphertext
     ":enabled", enabled ? "Y" : "N",
     ":coop", (cooperatorID < 0 ? null : (int?)cooperatorID), DbType.Int32,
    ":now1", DateTime.UtcNow, DbType.DateTime2,
    ":coop1", this.CooperatorID, DbType.Int32,
    ":now2", DateTime.UtcNow, DbType.DateTime2,
    ":coop2", this.CooperatorID, DbType.Int32
     ));

                    } else {
                        dm.Write(@"
update
    sys_user
set 
    user_name = :username,
    is_enabled = :enabled,
    cooperator_id = :coop,
    modified_date = :now1,
    modified_by = :coop1
where
    sys_user_id = :id
", new DataParameters(
     ":username", userName,
     ":enabled", enabled ? "Y" : "N",
     ":coop", (cooperatorID < 0 ? null : (int?)cooperatorID), DbType.Int32,
    ":now1", DateTime.UtcNow, DbType.DateTime2,
    ":coop1", this.CooperatorID, DbType.Int32,
    ":id", userID, DbType.Int32
     ));
                    }







                    if (cooperatorID < 0) {
                        cooperatorID = dm.Write(@"
INSERT INTO cooperator
           (current_cooperator_id
           ,site_id
           ,last_name
           ,title
           ,first_name
           ,job
           ,organization
           ,organization_abbrev
           ,address_line1
           ,address_line2
           ,address_line3
           ,city
           ,postal_index
           ,geography_id
           ,secondary_organization
           ,secondary_organization_abbrev
           ,secondary_address_line1
           ,secondary_address_line2
           ,secondary_address_line3
           ,secondary_city
           ,secondary_postal_index
           ,secondary_geography_id
           ,primary_phone
           ,secondary_phone
           ,fax
           ,email
           ,status_code
           ,category_code
           ,organization_region_code
           ,discipline_code
           ,note
           ,sys_lang_id
           ,web_cooperator_id
           ,created_date
           ,created_by
           ,owned_date
           ,owned_by)
     VALUES
           (
:current_cooperator_id, 
:siteid, 
:last_name, 
:title,
:first_name,
:job,
:organization,
:organization_abbrev,
:address_line1,
:address_line2,
:address_line3,
:city,
:postal_index,
:geography_id,
:secondary_organization,
:secondary_organization_abbrev,
:secondary_address_line1,
:secondary_address_line2,
:secondary_address_line3,
:secondary_city,
:secondary_postal_index,
:secondary_geography_id,
:primary_phone,
:secondary_phone,
:fax,
:email,
:status_code,
:category_code,
:organization_region_code,
:discipline_code,
:note,
:sys_lang_id,
:web_cooperator_id,
:created_date,
:created_by,
:owned_date,
:owned_by)
", true, "cooperator_id", new DataParameters(
         ":current_cooperator_id", (currentCooperatorID < 0 ? null : (int?)currentCooperatorID), DbType.Int32,
        ":siteid", (siteID < 0 ? null : (int?)siteID), DbType.Int32,
        ":last_name", lastName,
        ":title", title,
        ":first_name", firstName,
        ":job", job,
        ":organization", organization,
        ":organization_abbrev", organizationAbbreviation,
        ":address_line1", addressLine1,
        ":address_line2", addressLine2,
        ":address_line3", addressLine3,
        ":city", city,
        ":postal_index", postalIndex,
        ":geography_id", (geographyID < 0 ? null : (int?)geographyID), DbType.Int32,
        ":secondary_organization", null,
        ":secondary_organization_abbrev", null,
        ":secondary_address_line1", null,
        ":secondary_address_line2", null,
        ":secondary_address_line3", null,
        ":secondary_city", null,
        ":secondary_postal_index", null,
        ":secondary_geography_id", null, DbType.Int32,
        ":primary_phone", primaryPhone,
        ":secondary_phone", secondaryPhone,
        ":fax", fax,
        ":email", email,
        ":status_code", (isActive ? "ACTIVE" : "INACTIVE"),
        ":category_code", categoryCode,
        ":organization_region_code", regionCode,
        ":discipline_code", discipline,
        ":note", note,
        ":sys_lang_id", languageID, DbType.Int32,
        ":web_cooperator_id", (webCooperatorID < 0 ? null : (int?)webCooperatorID), DbType.Int32,
        ":created_date", DateTime.UtcNow, DbType.DateTime2,
        ":created_by", this.CooperatorID, DbType.Int32,
        ":owned_date", DateTime.UtcNow, DbType.DateTime2,
        ":owned_by", this.CooperatorID, DbType.Int32

     ));


                        // update sys_user record with new cop
                        dm.Write(@"
update
    sys_user
set
    cooperator_id = :coop1
where
    sys_user_id = :userid
", new DataParameters(":coop1", cooperatorID, DbType.Int32, ":userid", userID, DbType.Int32));



                    } else {
                        dm.Write(@"
update 
    cooperator
set
    current_cooperator_id = :current_cooperator_id,
    site_id = :siteid,
    last_name = :last_name,
    title = :title,
    first_name = :first_name,
    job = :job,
    organization = :organization,
    organization_abbrev = :organization_abbrev,
    address_line1 = :address_line1,
    address_line2 = :address_line2,
    address_line3 = :address_line3,
    city = :city,
    postal_index = :postal_index,
    geography_id = :geography_id,
    primary_phone = :primary_phone,
    secondary_phone = :secondary_phone,
    fax = :fax,
    email = :email,
    status_code = :status_code,
    category_code = :category_code,
    organization_region_code = :organization_region_code,
    discipline_code = :discipline_code,
    note = :note,
    sys_lang_id = :sys_lang_id,
    web_cooperator_id = :web_cooperator_id,
    modified_date = :modified_date,
    modified_by = :modified_by
where
    cooperator_id = :cooperator_id
", new DataParameters(
        ":current_cooperator_id", (currentCooperatorID < 0 ? null : (int?)currentCooperatorID), DbType.Int32,
        ":siteid", (siteID < 0 ? null : (int?)siteID), DbType.Int32,
        ":last_name", lastName,
        ":title", title,
        ":first_name", firstName,
        ":job", job,
        ":organization", organization,
        ":organization_abbrev", organizationAbbreviation,
        ":address_line1", addressLine1,
        ":address_line2", addressLine2,
        ":address_line3", addressLine3,
        ":city", city,
        ":postal_index", postalIndex,
        ":geography_id", (geographyID < 0 ? null : (int?)geographyID), DbType.Int32,
        ":primary_phone", primaryPhone,
        ":secondary_phone", secondaryPhone,
        ":fax", fax,
        ":email", email,
        ":status_code", (isActive ? "ACTIVE" : "INACTIVE"),
        ":category_code", categoryCode,
        ":organization_region_code", regionCode,
        ":discipline_code", discipline,
        ":note", note,
        ":sys_lang_id", languageID, DbType.Int32,
        ":web_cooperator_id", (webCooperatorID < 0 ? null : (int?)webCooperatorID), DbType.Int32,
        ":modified_date", DateTime.UtcNow, DbType.DateTime2,
        ":modified_by", this.CooperatorID, DbType.Int32,
        ":cooperator_id", cooperatorID, DbType.Int32
     ));

                    }

                    if (currentCooperatorID < 0) {
                        // update to point at itself
                        dm.Write(@"
update
    cooperator
set
    current_cooperator_id = :coop1
where
    cooperator_id = :coop2
", new DataParameters(":coop1", cooperatorID, DbType.Int32, ":coop2", cooperatorID, DbType.Int32));
                    }

                    // we get here, all is good...
                    dm.Commit();
                    clearCache(null);

                    return userID;
                }
            } catch (Exception ex) {
                LogException(ex, null);
                throw;
            }
        }

        public DataSet ListSites(string siteCode) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
SELECT
  CONCAT(s.site_short_name, ' (', site_long_name, ')') as display_text,
  s.site_id,
  s.site_short_name,
  s.site_long_name,
  s.organization_abbrev,
  s.is_internal,
  s.is_distribution_site,
  s.type_code,
  s.fao_institute_number,
  s.note,
  s.created_date,
  s.created_by,
  s.modified_date,
  s.modified_by,
  s.owned_date,
  s.owned_by
FROM
  site s
where
    coalesce(:sitecode, s.site_short_name, '\b') = coalesce(s.site_short_name, '\b')
order by
    1
", dsReturn, "list_sites", new DataParameters(":sitecode", siteCode));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }


        public DataSet ListGeographies(int geographyID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    *
from
    geography
where
    coalesce(:geographyID, geography_id) = geography_id
", dsReturn, "list_geographies", new DataParameters(":geographyID", (geographyID < 0 ? null : (int?)geographyID), DbType.Int32));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public int GetCodeValueUsageCount(string value, string tableName, string fieldName) {
            using (DataManager dm = BeginProcessing(true)) {
                // make sure user has proper permissions
                checkUserHasAdminEnabled();

                var count = Toolkit.ToInt32(dm.ReadValue(String.Format(@"
select
    count(*)
from
    {0}
where
    {0}.{1} = :val
", tableName, fieldName), new DataParameters(":val", value, DbType.String)), 0);

                return count;
            }
        }


        public DataSet ListTablesAndDataviewsByCodeGroup(string groupName) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    listTablesAndDataviewsByCodeGroup(groupName, dsReturn, dm);

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }


        private void listTablesAndDataviewsByCodeGroup(string groupName, DataSet ds, DataManager dm) {
            var gn = String.IsNullOrEmpty(groupName) ? null : groupName;

            dm.Read(@"
select
    1,
    null as dataview_name,
    st.table_name,
    stf.field_name,
    coalesce(stf.modified_date, stf.created_date) as last_touched
from
    sys_table st inner join sys_table_field stf
        on st.sys_table_id = stf.sys_table_id
where
    stf.group_name = :gn1
UNION
select
    2,
    sd.dataview_name,
    null as table_name,
    sdf.field_name,
    coalesce(sdf.modified_date, sdf.created_date) as last_touched
from
    sys_dataview sd inner join sys_dataview_field sdf
        on sd.sys_dataview_id = sdf.sys_dataview_id
where
    sdf.group_name = :gn2
order by
    1, 2, 3
", ds, "list_tables_and_dataviews_by_code_group", new DataParameters(":gn1", gn, DbType.String, ":gn2", gn, DbType.String));

        }

        public void DeleteCodeGroup(string groupName) {
            DataSet dsReturn = createReturnDataSet();
            try {

                checkUserHasAdminEnabled();

                using (DataManager dm = BeginProcessing(true)) {

                    dm.BeginTran();

                    // remap all associated dataview fields to inheriting from associated table_field (if any)
                    dm.Write(@"
update
    sys_dataview_field
set
    gui_hint = null, group_name = null
where
    group_name = :gn
", new DataParameters(":gn", groupName, DbType.String));

                    // remap all associated table fields to free-form textboxes
                    dm.Write(@"
update
    sys_table_field
set
    gui_hint = 'TEXT_CONTROL', group_name = null
where
    group_name = :gn
", new DataParameters(":gn", groupName, DbType.String));

                    // get rid of all code_value_lang records
                    dm.Write(@"
delete from 
    code_value_lang
where
    code_value_id in (select code_value_id from code_value where group_name = :gn)
", new DataParameters(":gn", groupName, DbType.String));

                    // finally get rid of all code_value records
                    dm.Write(@"
delete from 
    code_value
where
    group_name = :gn
", new DataParameters(":gn", groupName, DbType.String));

                    dm.Commit();
                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
        }

        public void DeleteCodeValue(int codeValueId, string groupName) {
            DataSet dsReturn = createReturnDataSet();
            try {

                checkUserHasAdminEnabled();

                using (DataManager dm = BeginProcessing(true)) {

                    var val = dm.ReadValue("select value from code_value where code_value_id = :cvid", new DataParameters(":cvid", codeValueId, DbType.Int32)) as string;

                    if (val == null) {
                        throw Library.CreateBusinessException(getDisplayMember("DeleteCodeValue{notfound}", "Could not find record for code_value_id = {0}", codeValueId.ToString()));
                    }

                    // first make sure nobody is referencing it
                    var dt = ListTablesAndDataviewsByCodeGroup(groupName).Tables["list_tables_and_dataviews_by_code_group"];
                    foreach (DataRow dr in dt.Rows) {
                        var tblName = dr["table_name"].ToString();
                        if (!String.IsNullOrEmpty(tblName)) {
                            var count = Toolkit.ToInt32(dm.ReadValue(String.Format(@"
select 
    count(*) 
from 
    {0} 
where 
    {1} = :val", tblName, dr["field_name"].ToString()), new DataParameters(":val", val, DbType.String)), 0);
                            if (count > 0) {
                                throw Library.CreateBusinessException(getDisplayMember("DeleteCodeValue{referencedby}", "Table/field {0}.{1} has {2} rows which reference that value.", tblName, dr["field_name"].ToString(), count.ToString("###,###,##0")));
                            }
                        }
                    }

                    dm.BeginTran();
                    dm.Write(@"
delete from 
    code_value_lang
where
    code_value_id = :cvid
", new DataParameters(":cvid", codeValueId, DbType.Int32));

                    dm.Write(@"
delete from 
    code_value
where
    code_value_id = :cvid
", new DataParameters(":cvid", codeValueId, DbType.Int32));

                    dm.Commit();
                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
        }

        public DataSet ListCodeGroups(string groupName) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    cv.group_name,
    ((select count(*) from sys_table_field where group_name = cv.group_name) 
    + (select count(*) from sys_dataview_field where group_name = cv.group_name)) as reference_count,
    count(*) as value_count,
    max(coalesce(cv.modified_date, cv.created_date)) as last_touched
from
    code_value cv
where
    coalesce(:gn, cv.group_name) = cv.group_name
group by
    cv.group_name
order by
    cv.group_name
", dsReturn, "list_code_groups", new DataParameters(":gn", String.IsNullOrEmpty(groupName) ? null : groupName));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public int SaveCodeValue(int codeValueId, string groupName, string value, DataTable dtLang, List<string> touchedTables) {
            using (DataManager dm = BeginProcessing(true)) {
                // make sure user has proper permissions
                checkUserHasAdminEnabled();

                dm.BeginTran();

                if (codeValueId < 0) {
                    codeValueId = dm.Write(@"
insert into 
    code_value
(group_name, value, created_date, created_by, owned_date, owned_by)
    values
(:gn, :val, :now1, :who1, :now2, :who2)
", true, "code_value_id", new DataParameters(
     ":gn", groupName, DbType.String,
     ":val", value, DbType.String,
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":who1", CooperatorID, DbType.Int32,
     ":now2", DateTime.UtcNow, DbType.DateTime2,
     ":who2", CooperatorID, DbType.Int32
     ));
                } else {

                    // code_value table is a special case -- there is no FK between code_value.value and any table.
                    // that means we need to handle updating the other related tables as needed if the user changes
                    // the code_value.value field itself.

                    var origValue = dm.ReadValue(@"
select 
    value
from
    code_value
where
    code_value_id = :id
    and value <> :val
", new DataParameters(":id", codeValueId, DbType.Int32,
     ":val", value, DbType.String)).ToString();

                    if (!String.IsNullOrEmpty(origValue)) {
                        // determine which table(s) currently have references to this group and update those that had the original value to the new value
                        var ds = ListTablesAndDataviewsByCodeGroup(groupName);
                        foreach (DataRow dr in ds.Tables["list_tables_and_dataviews_by_code_group"].Rows) {
                            // note we're updating all other tables before the code_value table
                            // this is ok because we're in a transaction. otherwise it would be a nightmare if something bombed.
                            var tbl = dr["table_name"].ToString();
                            var fld = dr["field_name"].ToString();
                            if (!String.IsNullOrEmpty(tbl)) {
                                dm.Write(String.Format(@"
update
    {0}
set
    {1} = :newval,
    modified_date = :now1,
    modified_by = :who1
where
    {1} = :origval
", tbl, fld), new DataParameters(
         ":newval", value, DbType.String,
         ":now1", DateTime.UtcNow, DbType.DateTime2,
         ":who1", CooperatorID, DbType.Int32,
         ":origval", origValue, DbType.String));

                                touchedTables.Add(tbl);
                            }
                        }
                    }


                    dm.Write(@"
update 
    code_value
set
    group_name = :gn,
    value = :val,
    modified_date = :now1,
    modified_by = :who1
where
    code_value_id = :id
", new DataParameters(
     ":gn", groupName, DbType.String,
     ":val", value, DbType.String,
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":who1", CooperatorID, DbType.Int32,
     ":id", codeValueId, DbType.Int32
     ));

                }

                foreach (DataRow dr in dtLang.Rows) {

                    var title = dr["title"].ToString();

                    if (title.Trim().Length == 0) {
                        // empty, remove one if it exists.
                        dm.Write(@"
delete from 
    code_value_lang
where
    code_value_id = :cvid
    and sys_lang_id = :langid
", new DataParameters(":cvid", codeValueId, DbType.Int32,
     ":langid", dr["sys_lang_id"], DbType.Int32));

                    } else {
                        var cvlid = Toolkit.ToInt32(dr["code_value_lang_id"], -1);
                        if (cvlid < 0) {
                            cvlid = dm.Write(@"
insert into
    code_value_lang
(code_value_id, sys_lang_id, title, description, created_date, created_by, owned_date, owned_by)
    values
(:cvid, :langid, :title, :description, :now1, :who1, :now2, :who2)
", true, "code_value_lang_id", new DataParameters(
             ":cvid", codeValueId, DbType.Int32,
             ":langid", dr["sys_lang_id"], DbType.Int32,
             ":title", dr["title"].ToString(), DbType.String,
             ":description", dr["description"].ToString(), DbType.String,
             ":now1", DateTime.UtcNow, DbType.DateTime2,
             ":who1", CooperatorID, DbType.Int32,
             ":now2", DateTime.UtcNow, DbType.DateTime2,
             ":who2", CooperatorID, DbType.Int32
         ));
                        } else {
                            dm.Write(@"
update
    code_value_lang
set
    code_value_id = :cvid,
    sys_lang_id = :langid,
    title = :title,
    description = :description,
    modified_date = :now1,
    modified_by = :who1
where
    code_value_lang_id = :cvlid
", new DataParameters(
         ":cvid", codeValueId, DbType.Int32,
         ":langid", dr["sys_lang_id"], DbType.Int32,
         ":title", dr["title"].ToString(), DbType.String,
         ":description", dr["description"].ToString(), DbType.String,
         ":now1", DateTime.UtcNow, DbType.DateTime2,
         ":who1", CooperatorID, DbType.Int32,
         ":cvlid", cvlid, DbType.Int32));
                        }
                    }
                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);

                dm.Commit();
                return codeValueId;
            }
        }

        public void RenameCodeGroup(string originalGroupName, string newGroupName) {
            using (DataManager dm = BeginProcessing(true)) {
                // make sure user has proper permissions
                checkUserHasAdminEnabled();

                dm.BeginTran();

                // update sys_dataview_field references
                dm.Write(@"
update
    sys_dataview_field
set
    group_name = :newgroup,
    modified_date = :now1,
    modified_by = :who1
where
    group_name = :origgroup
", new DataParameters(
     ":newgroup", newGroupName, DbType.String,
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":who1", CooperatorID, DbType.Int32,
     ":origgroup", originalGroupName, DbType.String));

                // update sys_table_field references
                dm.Write(@"
update
    sys_table_field
set
    group_name = :newgroup,
    modified_date = :now1,
    modified_by = :who1
where
    group_name = :origgroup
", new DataParameters(
     ":newgroup", newGroupName, DbType.String,
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":who1", CooperatorID, DbType.Int32,
     ":origgroup", originalGroupName, DbType.String));

                // update code_value table
                dm.Write(@"
update
    code_value
set
    group_name = :newgroup,
    modified_date = :now1,
    modified_by = :who1
where
    group_name = :origgroup
", new DataParameters(
          ":newgroup", newGroupName, DbType.String,
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":who1", CooperatorID, DbType.Int32,
     ":origgroup", originalGroupName, DbType.String));

                dm.Commit();
            }
        }


        public DataSet ListCodeValues(string groupName, int? valueID, int? languageID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    cv.group_name,
    cv.value as value_member,
    coalesce(cvl.title, cv.value) as display_member,
    cvl.title,
    cvl.description,
    case when coalesce(cvl.modified_date, cvl.created_date) > coalesce(cv.modified_date, cv.created_date) then
        coalesce(cvl.modified_date, cvl.created_date)
    else
        coalesce(cv.modified_date, cv.created_date)
    end as last_touched,
    cv.code_value_id,
    sl.title as language_title,
    sl.script_direction,
    cvl.code_value_lang_id
from
    code_value cv left join code_value_lang cvl
        on cv.code_value_id = cvl.code_value_id 
        and cvl.sys_lang_id = :langid
    left join sys_lang sl
        on sl.sys_lang_id = cvl.sys_lang_id
where
    coalesce(:gn, group_name) = group_name
    and coalesce(:cvid, cv.code_value_id) = cv.code_value_id
order by
    group_name, value
", dsReturn, "list_code_values", new DataParameters(
     ":gn", String.IsNullOrEmpty(groupName) ? null : groupName, DbType.String,
     ":cvid", valueID, DbType.Int32,
     ":langid", languageID ?? (int?)LanguageID, DbType.Int32));


                    dm.Read(@"
select
    sl.sys_lang_id,
    sl.title as language_title,
    sl.script_direction,
	cvl.code_value_lang_id,
	cvl.code_value_id,
    cvl.title,
    cvl.description
from
    sys_lang sl left join code_value_lang cvl
        on sl.sys_lang_id = cvl.sys_lang_id
        and cvl.code_value_id = :cvid
order by
	sl.title
", dsReturn, "list_code_value_langs", new DataParameters(
     ":cvid", valueID, DbType.Int32
     ));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet ListLanguages(int languageID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // any user can get a language list
                    // checkUserHasAdminEnabled();

                    dm.Read(@"
select
    *
from
    sys_lang
where
    coalesce(:languageID, sys_lang_id) = sys_lang_id
order by
    title
", dsReturn, "list_languages", new DataParameters(":languageID", (languageID < 0 ? null : (int?)languageID), DbType.Int32));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet DeleteUser(int userID) {
            DataSet dsReturn = createReturnDataSet();
            try {

                checkUserHasAdminEnabled();


                if (userID == this.SysUserID) {
                    throw Library.CreateBusinessException(getDisplayMember("DeleteUser{yourself}", "You cannot delete yourself."));
                }

                if (userID == this.getSysUserIDForUser("guest")) {
                    throw Library.CreateBusinessException(getDisplayMember("DeleteUser{guest}", "You cannot delete the guest user.  However, you may disable it."));
                }

                if (userID == this.getSysUserIDForUser("system")) {
                    throw Library.CreateBusinessException(getDisplayMember("DeleteUser{system}", "You cannot delete the System user.  However, you may disable it."));
                }

                var groupID = this.GetSysGroupIDForTag("admins");
                var dtAdmins = this.ListUsersInGroup(groupID).Tables["list_users_in_group"];
                if (dtAdmins.Rows.Count == 1) {
                    if (userID == Toolkit.ToInt32(dtAdmins.Rows[0]["sys_user_id"], -1)) {
                        throw Library.CreateBusinessException(getDisplayMember("DeleteUser{lastadmin}", "You cannot delete the last member of the ADMINS group."));
                    }
                }


                using (DataManager dm = BeginProcessing(true)) {
                    dm.BeginTran();

                    //                    dm.Write(@"delete from sys_user_role where sys_user_id = :userid", new DataParameters(":userid", userID));
                    //                    dm.Write(@"delete from sys_user_cart_item where web_user_cart_id in (select web_user_cart_id from sys_user_cart where sys_user_id = :userid)", new DataParameters(":userid", userID, DbType.Int32));
                    //                    dm.Write(@"delete from sys_user_cart where sys_user_id = :userid", new DataParameters(":userid", userID, DbType.Int32));
                    dm.Write(@"delete from app_user_gui_setting where cooperator_id in (select cooperator_id from sys_user where sys_user_id = :userid)", new DataParameters(":userid", userID, DbType.Int32));
                    dm.Write(@"delete from sys_user_permission_map where sys_user_id = :userid", new DataParameters(":userid", userID, DbType.Int32));
                    dm.Write(@"delete from sys_group_user_map where sys_user_id = :userid", new DataParameters(":userid", userID, DbType.Int32));
                    dm.Write(@"delete from sys_user where sys_user_id = :userid", new DataParameters(":userid", userID, DbType.Int32));

                    dm.Commit();
                }

                // clear all caches to make sure our changes are immediately visible
                clearCache(null);

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet CreateUser(string userName, string password, bool enabled) {

            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {

                    checkUserHasAdminEnabled();

                    // first determine if this user has proper permissions
                    //if (!CanCreate(dm, "sys_user", null)) {
                    //    throw Library.DataPermissionException(this.UserName, "sys_user", Permission.Create, null);
                    //}

                    string hashedPassword = Crypto.HashText(password);
                    string doubleHashedPassword = Crypto.HashText(hashedPassword);
                    int id = dm.Write(@"
insert into 
	sys_user 
	(user_name, password, is_enabled, created_date, created_by, owned_date, owned_by) 
values 
	(:username, :password, :isenabled, :createddate, :createdby, :owneddate, :ownedby)
",
                                    true,
                                    "id",
                                    new DataParameters(
                                        new DataParameter(":username", userName),
                                        new DataParameter(":password", doubleHashedPassword),
                                        new DataParameter(":isenabled", (enabled ? "Y" : "N")),
                                        new DataParameter(":createddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2),
                                        new DataParameter(":createdby", CooperatorID, DbType.Int32),
                                        new DataParameter(":owneddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2),
                                        new DataParameter(":ownedby", CooperatorID, DbType.Int32)
                                        ));

                    log("created user '" + userName + "', id=" + id);

                    dm.Read(@"
select * from sys_user where sys_user_id = :secuserid
", dsReturn, "sys_user", new DataParameters(":secuserid", id, DbType.Int32));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        //        private int getUserId(string userName) {
        //            return (int)_dm.ReadValue(@"
        //select 
        //	id 
        //from 
        //	sys_user 
        //where 
        //	user_name = :username
        //", new DataParameters(":username", userName));
        //        }

        public DataSet ListLookupDataViews() {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    *
from
    sys_dataview
where
    dataview_name like '%_lookup'
    or dataview_name like '%_dropdown'
order by
    dataview_name
", dsReturn, "list_lookup_dataviews", null);
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet ListDataViews(int dataviewID, string fromCategory, bool orderByCategory, string fromDatabaseArea, bool orderByDatabaseArea) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
sd.sys_dataview_id
      ,sd.dataview_name
      ,sd.is_enabled
      ,sd.is_readonly
      ,sd.category_code
      ,coalesce(cvl_cc.title, sd.category_code) as category_name
      ,sd.database_area_code
      ,coalesce(cvl_da.title, sd.database_area_code) as database_area
      ,sd.database_area_code_sort_order
      ,sd.is_transform
      ,sd.transform_field_for_names
      ,sd.transform_field_for_captions
      ,sd.transform_field_for_values
      ,sd.created_date
      ,sd.created_by
      ,sd.modified_date
      ,sd.modified_by
      ,sd.owned_date
      ,sdl.title
      ,sdl.description
      ,coalesce(sd.modified_date, sd.created_date) as last_touched_date
from
    sys_dataview sd left join sys_dataview_lang sdl
        on sd.sys_dataview_id = sdl.sys_dataview_id
        and coalesce(sdl.sys_lang_id, :langid, -1) = coalesce(:langid, sdl.sys_lang_id, -1)
    left join code_value cv_cc on cv_cc.group_name = 'DATAVIEW_CATEGORY' and cv_cc.value = sd.category_code
    left join code_value_lang cvl_cc on cv_cc.code_value_id = cvl_cc.code_value_id and cvl_cc.sys_lang_id = coalesce(:langid2, cvl_cc.sys_lang_id)
    left join code_value cv_da on cv_da.group_name = 'DATAVIEW_DATABASE_AREA' and cv_da.value = sd.database_area_code
    left join code_value_lang cvl_da on cv_da.code_value_id = cvl_da.code_value_id and cvl_da.sys_lang_id = coalesce(:langid3, cvl_da.sys_lang_id)
where
    coalesce(:dvid, sd.sys_dataview_id) = sd.sys_dataview_id
    and coalesce(:catname, sd.category_code, '\b') = coalesce(sd.category_code, '\b')
    and coalesce(:dbarea, sd.database_area_code, '\b') = coalesce(sd.database_area_code, '\b')
order by
" + (orderByCategory ? " coalesce(cvl_cc.title, sd.category_code), " : "") + @"
" + (orderByDatabaseArea ? " coalesce(cvl_da.title, sd.database_area_code), sd.database_area_code_sort_order, " : "") + @"
    sd.dataview_name
", dsReturn, "list_dataviews", new DataParameters(":dvid", dataviewID < 0 ? null : (int?)dataviewID, DbType.Int32,
     ":langid", LanguageID, DbType.Int32,
     ":langid2", LanguageID, DbType.Int32,
     ":langid3", LanguageID, DbType.Int32,
     ":catname", fromCategory, DbType.String,
     ":dbarea", fromDatabaseArea, DbType.String));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet ListTables(int dataviewID, int tableID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    st.sys_table_id,
    st.table_name,
    st.is_enabled
from
    sys_table st inner join sys_table_field stf
        on st.sys_table_id = stf.sys_table_id
    inner join sys_dataview_field sdf
        on sdf.sys_table_field_id = stf.sys_table_field_id
where
    coalesce(:tblid, st.sys_table_id) = st.sys_table_id
    and coalesce(:dvid, sdf.sys_dataview_id) = sdf.sys_dataview_id
group by
    st.sys_table_id,
    st.table_name,
    st.is_enabled
order by
    st.table_name
", dsReturn, "list_tables", new DataParameters(
     ":tblid", tableID < 0 ? null : (int?)tableID, DbType.Int32,
     ":dvid", dataviewID < 0 ? null : (int?)dataviewID, DbType.Int32)
     );
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }


        public DataSet ListTables(int tableID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    st.sys_table_id,
    st.table_name,
    st.is_enabled,
    st.is_readonly,
    st.audits_created,
    st.audits_modified,
    st.audits_owned,
    st.database_area_code,
    st.created_date,
    st.created_by,
    st.modified_date,
    st.modified_by,
    st.owned_date,
    st.owned_by
from
    sys_table st
where
    coalesce(:tblid, st.sys_table_id) = st.sys_table_id
order by
    st.table_name
", dsReturn, "list_tables", new DataParameters(":tblid", tableID < 0 ? null : (int?)tableID, DbType.Int32));
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet ListTables(string tableName) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    dm.Read(@"
select
    st.sys_table_id,
    st.table_name,
    st.is_enabled,
    st.is_readonly,
    st.audits_created,
    st.audits_modified,
    st.audits_owned,
    st.database_area_code,
    st.created_date,
    st.created_by,
    st.modified_date,
    st.modified_by,
    st.owned_date,
    st.owned_by
from
    sys_table st
where
    coalesce(:tablename, st.table_name) = st.table_name
order by
    st.table_name
", dsReturn, "list_tables", new DataParameters(":tablename", tableName, DbType.String));
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }




        public DataSet ListRelatedTables(int dataviewID, int tableID, int parentTableFieldID, string[] relationshipTypes) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    listRelatedTables(dataviewID, tableID, parentTableFieldID, relationshipTypes, dm, dsReturn);

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }



        public DataSet ApplyPermissionTemplatesToUser(int userID, List<int> permTemplateIDs) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (permTemplateIDs != null) {
                        foreach (int id in permTemplateIDs) {

                            var ds = ListPermissionTemplates(id);
                            var perms = new List<int>();
                            foreach (DataRow dr in ds.Tables["permissions_by_template"].Rows) {
                                perms.Add(Toolkit.ToInt32(dr["sys_permission_id"], -1));
                            }
                            AddPermissionsToUser(userID, perms);
                        }

                        clearCache(null);
                    }

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }


        public DataSet AddPermissionsToUser(int userID, List<int> permIDs) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (permIDs != null) {
                        foreach (int id in permIDs) {
                            int count = Toolkit.ToInt32(dm.ReadValue("select count(*) from sys_user_permission_map where sys_user_id = :userid and sys_permission_id = :permid",
                                new DataParameters(":userid", userID, DbType.Int32, ":permid", id, DbType.Int32)), -1);

                            if (count == 0) {
                                dm.Write(@"
insert into sys_user_permission_map
(sys_user_id, sys_permission_id, is_enabled, created_date, created_by, owned_date, owned_by)
values
(:userid, :permid, 'Y', :now1, :coop1, :now2, :coop2)
", new DataParameters(":userid", userID, DbType.Int32,
         ":permid", id, DbType.Int32,
         ":now1", DateTime.UtcNow, DbType.DateTime2,
         ":coop1", this.CooperatorID, DbType.Int32,
         ":now2", DateTime.UtcNow, DbType.DateTime2,
         ":coop2", this.CooperatorID, DbType.Int32));
                            }
                        }

                        clearCache("PermissionAssignment");
                    }



                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet RemovePermissionsFromUser(int userID, List<int> permIDs) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    if (permIDs != null && permIDs.Count > 0) {
                        dm.Write(@"
delete from 
    sys_user_permission_map
where
    sys_user_id = :userid 
    and sys_permission_id in (:permids)
", new DataParameters(":userid", userID, DbType.Int32,
     ":permids", permIDs, DbPseudoType.IntegerCollection));

                        clearCache("PermissionAssignment");

                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet ListEffectivePermissions(int userID, string dataviewName, string tableName) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    var perms = getPermissions(dm, userID, tableName, dataviewName, null, null);
                    var dt = dsReturn.Tables.Add("effective_permissions");
                    dt.Columns.Add("create_permission", typeof(string));
                    dt.Columns.Add("read_permission", typeof(string));
                    dt.Columns.Add("update_permission", typeof(string));
                    dt.Columns.Add("delete_permission", typeof(string));
                    DataRow dr = dt.NewRow();
                    foreach (Permission perm in perms) {
                        switch (perm.Action) {
                            case PermissionAction.Create:
                                dr["create_permission"] = perm.DatabaseValue;
                                break;
                            case PermissionAction.Read:
                                dr["read_permission"] = perm.DatabaseValue;
                                break;
                            case PermissionAction.Update:
                                dr["update_permission"] = perm.DatabaseValue;
                                break;
                            case PermissionAction.Delete:
                                dr["delete_permission"] = perm.DatabaseValue;
                                break;
                            default:
                                break;
                        }
                    }
                    dt.Rows.Add(dr);

                }


            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }
























        public DataSet ListTableNames(string schemaName) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    Creator c = Creator.GetInstance(DataConnectionSpec);

                    var names = c.ListTableNames(schemaName);
                    dsReturn.AddList(names, "list_tables");

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet RecreateTableMappings(List<string> tableNames) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    Creator c = Creator.GetInstance(DataConnectionSpec);

                    List<TableInfo> tables = new List<TableInfo>();

                    foreach (string s in tableNames) {
                        var ti = TableInfo.GetInstance(DataConnectionSpec);
                        ti.Fill(null, s, tables, false, 0);
                    }

                    foreach (TableInfo ti in tables) {
                        foreach (string s in tableNames) {
                            if (String.Compare(ti.TableName, s, true) == 0) {
                                ti.IsSelected = true;
                            }
                        }
                    }

                    tables.Sort(new TableInfoComparer());

                    // now that we have all the mappings and relationships determined,
                    // copy the info into the mappings table in the database
                    foreach (TableInfo ti in tables) {
                        if (ti.IsSelected) {
                            c.CreateTableMappings(ti, CooperatorID, tables);
                        }
                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet RecreateTableRelationships(List<string> tableNames) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    Creator c = Creator.GetInstance(DataConnectionSpec);

                    List<TableInfo> tables = new List<TableInfo>();

                    foreach (string s in tableNames) {
                        var ti = TableInfo.GetInstance(DataConnectionSpec);
                        ti.Fill(DataConnectionSpec.DatabaseName, s, tables, false, 0);
                        ti.IsSelected = true;
                    }

                    tables.Sort(new TableInfoComparer());

                    // now that we have all the mappings and relationships determined,
                    // copy the info into the mappings table in the database
                    foreach (TableInfo ti in tables) {
                        if (ti.IsSelected) {
                            c.CreateTableRelationshipMappings(ti, CooperatorID);
                        }
                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        public DataSet RecreateTableIndexes(List<string> tableNames) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    Creator c = Creator.GetInstance(DataConnectionSpec);

                    List<TableInfo> tables = new List<TableInfo>();

                    foreach (string s in tableNames) {
                        var ti = TableInfo.GetInstance(DataConnectionSpec);
                        ti.Fill(DataConnectionSpec.DatabaseName, s, tables, false, 0);
                        ti.IsSelected = true;
                    }

                    tables.Sort(new TableInfoComparer());

                    // now that we have all the mappings and relationships determined,
                    // copy the info into the mappings table in the database
                    foreach (TableInfo ti in tables) {
                        if (ti.IsSelected) {
                            c.CreateTableIndexMappings(ti, CooperatorID);
                        }
                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }


        public DataSet ListOrphanedMappings() {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    checkUserHasAdminEnabled();

                    Creator c = Creator.GetInstance(DataConnectionSpec);

                    List<TableInfo> tables = new List<TableInfo>();

                    // get the list of tables from the db engine's schema...
                    var namesInSchema = c.ListTableNames("gringlobal");

                    // and the list of tables from the mappings...
                    var dtNames = this.ListTables(-1).Tables["list_tables"];
                    var namesInMappings = dtNames.ListColumnValues("table_name", true);



                    // then find the delta and add that to our output
                    var dt = new DataTable("list_orphaned_mappings");
                    dt.Columns.Add("table_name", typeof(string));
                    dt.Columns.Add("sys_table_id", typeof(int));
                    dsReturn.Tables.Add(dt);

                    foreach (var nim in namesInMappings) {
                        var found = false;
                        foreach (var nis in namesInSchema) {
                            if (String.Compare(nis, nim, true) == 0) {
                                found = true;
                                break;
                            }
                        }
                        if (!found) {
                            // found an orphan, add it to the list
                            var tableID = Toolkit.ToInt32(dm.ReadValue("select sys_table_id from sys_table where table_name = :nm", new DataParameters(":nm", nim, DbType.String)), -1);
                            if (tableID > -1) {
                                dt.Rows.Add(nim, tableID);
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }



        public void DeleteConnectionString(string name, string configFilePath) {

            var doc = new XmlDocument();
            doc.Load(configFilePath);
            // they changed the name.  Remove the old one from the document before adding the new one.
            var oldNode = doc.SelectSingleNode("/configuration/connectionStrings/add[@name='" + name + "']");
            if (oldNode != null) {
                oldNode.ParentNode.RemoveChild(oldNode);
            }
            doc.Save(configFilePath);

        }

        public void SaveConnectionString(string name, string connectionString, string providerName, string configFilePath) {
            var doc = new XmlDocument();
            doc.Load(configFilePath);
            var addNode = doc.SelectSingleNode("/configuration/connectionStrings/add[@name='" + name + "']");

            if (addNode == null) {
                // key name doesn't exist, create and add a new 'add' node
                addNode = doc.CreateElement("add");
                var appNode = doc.SelectSingleNode("/configuration/connectionStrings");
                appNode.AppendChild(addNode);
            }

            var nm = doc.CreateAttribute("name");
            nm.Value = name;
            addNode.Attributes.SetNamedItem(nm);

            var cs = doc.CreateAttribute("connectionString");
            cs.Value = connectionString;
            addNode.Attributes.SetNamedItem(cs);

            var pn = doc.CreateAttribute("providerName");
            pn.Value = providerName;
            addNode.Attributes.SetNamedItem(pn);

            doc.Save(configFilePath);
        }

        public void DeleteApplicationSetting(string name, string configFilePath) {
            var doc = new XmlDocument();
            doc.Load(configFilePath);
            var oldNode = doc.SelectSingleNode("/configuration/appSettings/add[@key='" + name + "']");
            if (oldNode != null) {
                oldNode.ParentNode.RemoveChild(oldNode);
            }
            doc.Save(configFilePath);
        }

        public void SaveApplicationSetting(string name, string value, string configFilePath) {
            var doc = new XmlDocument();
            doc.Load(configFilePath);
            var addNode = doc.SelectSingleNode("/configuration/appSettings/add[@key='" + name + "']");

            if (addNode == null) {
                // key name doesn't exist, create and add a new 'add' node
                addNode = doc.CreateElement("add");
                var appNode = doc.SelectSingleNode("/configuration/appSettings");
                appNode.AppendChild(addNode);
            } else {
                // key name does exist, nothing special to do
            }

            var key = doc.CreateAttribute("key");
            key.Value = name;
            addNode.Attributes.SetNamedItem(key);

            var val = doc.CreateAttribute("value");
            val.Value = value;
            addNode.Attributes.SetNamedItem(val);

            doc.Save(configFilePath);
        }

        #endregion Administration


        #region DataView Management

        public DataSet GetDataViewDefinition(string dataviewName) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    //					string concatted = dm.Concatenate(new string[] { "sr.dataview_name", "' - '", "sr.description" });

                    checkUserHasAdminEnabled();

                    // pull sys_table (just so we can get unique table names to fill a dropdown possibly)
                    dm.Read(@"
select
	sys_table_id,
	table_name
from
	sys_table
order by
	table_name
", dsReturn, "sys_table");

                    var dtTableList = dsReturn.Tables["sys_table"];
                    var drNoneTable = dtTableList.NewRow();
                    drNoneTable["sys_table_id"] = 0;
                    drNoneTable["table_name"] = "(None)";
                    dtTableList.Rows.InsertAt(drNoneTable, 0);

                    //                    // pull sys_dataview
                    //                    dm.Read(@"
                    //select
                    //	sr.*,
                    //    srs.sql_statement,
                    //    srl.description,
                    //    concat(sr.dataview_name, ' - ', srl.description) as dropdown_description
                    //from
                    //	sys_dataview sr left join sys_dataview_sql srs
                    //        on sr.sys_dataview_id = srs.sys_dataview_id
                    //        and srs.database_engine_tag = :engineCode
                    //    left join sys_dataview_lang srl
                    //        on sr.sys_dataview_id = srl.sys_dataview_id
                    //        and srl.sys_lang_id = :sys_lang_id
                    //where
                    //	coalesce(sr.dataview_name, '') = coalesce(:rs, sr.dataview_name, '')
                    //    and coalesce(srs.sql_statement, '') != ''
                    //order by
                    //	dataview_name
                    //",  dsReturn , "sys_dataview", new DataParameters(":engineCode", dm.DataConnectionSpec.EngineName, ":sys_lang_id", LanguageID, ":rs", String.IsNullOrEmpty(dataviewName) ? null : dataviewName));

                    // pull sys_dataview
                    dm.Read(@"
select
sdv.sys_dataview_id
      ,sdv.dataview_name
      ,sdv.is_enabled
      ,sdv.is_readonly
      ,sdv.category_code
      ,sdv.database_area_code
      ,sdv.database_area_code_sort_order
      ,sdv.is_transform
      ,sdv.transform_field_for_names
      ,sdv.transform_field_for_captions
      ,sdv.transform_field_for_values
      ,sdv.configuration_options
      ,sdv.created_date
      ,sdv.created_by
      ,sdv.modified_date
      ,sdv.modified_by
      ,sdv.owned_date
      ,sdv.owned_by,
    sdvs.database_engine_tag,
    sdvs.sql_statement,
    sdvl.title,
    sdvl.description
from
	sys_dataview sdv left join sys_dataview_sql sdvs
        on sdv.sys_dataview_id = sdvs.sys_dataview_id
        and sdvs.database_engine_tag = :engineCode
    left join sys_dataview_lang sdvl
        on sdv.sys_dataview_id = sdvl.sys_dataview_id
        and sdvl.sys_lang_id = :sys_lang_id
where
	coalesce(sdv.dataview_name, '\b') = coalesce(:dataview, sdv.dataview_name, '\b')
order by
	dataview_name
", dsReturn, "sys_dataview", new DataParameters(":engineCode", dm.DataConnectionSpec.EngineName, ":sys_lang_id", LanguageID, DbType.Int32, ":dataview", String.IsNullOrEmpty(dataviewName) ? null : dataviewName));

                    // pull sys_dataview_sql
                    dm.Read(@"
select
    sdvs.sys_dataview_sql_id
      ,sdvs.sys_dataview_id
      ,sdvs.database_engine_tag
      ,sdvs.sql_statement
      ,sdvs.created_date
      ,sdvs.created_by
      ,sdvs.modified_date
      ,sdvs.modified_by
      ,sdvs.owned_date
      ,sdvs.owned_by
from
    sys_dataview_sql sdvs inner join sys_dataview sdv
        on sdvs.sys_dataview_id = sdv.sys_dataview_id
where
    sdv.dataview_name = :dataview
", dsReturn, "sys_dataview_sql", new DataParameters(":dataview", String.IsNullOrEmpty(dataviewName) ? null : dataviewName));

                    // if they didn't give us a specific resultname, don't bother to lookup the 
                    // rest of the info (as we don't want to pull everything from the db)
                    //if (String.IsNullOrEmpty(dataviewName) || dsReturn.Tables["sys_dataview"].Rows.Count == 0) {
                    //    return dsReturn ;
                    //}

                    // pull sys_dataview_lang
                    dm.Read(@"
select
    sdvl.sys_dataview_lang_id
      ,sdvl.sys_dataview_id
      ,sdvl.sys_lang_id
      ,sdvl.title
      ,sdvl.description
      ,sdvl.created_date
      ,sdvl.created_by
      ,sdvl.modified_date
      ,sdvl.modified_by
      ,sdvl.owned_date
      ,sdvl.owned_by
      ,sl.title as language_name
from
    sys_dataview_lang sdvl inner join sys_dataview sdv
        on sdvl.sys_dataview_id = sdv.sys_dataview_id
    inner join sys_lang sl
        on sl.sys_lang_id = sdvl.sys_lang_id
where
	coalesce(sdv.dataview_name, '\b') = coalesce(:dataview, sdv.dataview_name, '\b')
", dsReturn, "sys_dataview_lang", new DataParameters(":dataview", String.IsNullOrEmpty(dataviewName) ? null : dataviewName));


                    // pull sys_dataview_param
                    dm.Read(@"
select
sdvp.sys_dataview_param_id
      ,sdvp.sys_dataview_id
      ,sdvp.param_name
      ,sdvp.param_type
      ,sdvp.sort_order
      ,sdvp.created_date
      ,sdvp.created_by
      ,sdvp.modified_date
      ,sdvp.modified_by
      ,sdvp.owned_date
      ,sdvp.owned_by,
      null as param_value
from
	sys_dataview_param sdvp
		inner join sys_dataview sdv
		on sdv.sys_dataview_id = sdvp.sys_dataview_id
where
	coalesce(sdv.dataview_name, '\b') = coalesce(:dataview, sdv.dataview_name, '\b')
order by
	sdvp.sort_order
", dsReturn, "sys_dataview_param", new DataParameters(":dataview", String.IsNullOrEmpty(dataviewName) ? null : dataviewName));


                    // pull sys_dataview_field_lang
                    dm.Read(@"
select
	srf.field_name as dv_field_name,
	st.table_name as table_name,
	stf.field_name as table_field_name,
	srf.sort_order as dv_field_sort_order,
	sr.is_readonly as dv_readonly,
	st.is_readonly as table_readonly,
    stf.is_nullable as table_field_nullable,
	case when stf.field_purpose = 'DATA' then 'N' else 'Y' end as table_field_readonly,
	srf.is_readonly as dv_field_readonly,
    srf.is_primary_key as dv_field_primary_key,
    srf.is_transform as dv_field_transform,
	srf.sys_dataview_field_id as sys_dataview_field_id,
	srf.sys_table_field_id as dv_sys_table_field_id,
    coalesce(srf.group_name, stf.group_name) as dv_group_name,
    srf.table_alias_name as table_alias_name,
    srf.is_visible as dv_is_visible,
    srf.configuration_options as dv_options,
	coalesce(srf.foreign_key_dataview_name, stf.foreign_key_dataview_name) as foreign_key_dataview_name,
    coalesce(srf.gui_hint, stf.gui_hint) as gui_hint,
	srf.foreign_key_dataview_name as fd_dv_name_from_sys_dataview,
	stf.sys_table_field_id as sys_table_field_id
from
	sys_dataview sr
	inner join sys_dataview_field srf
		on sr.sys_dataview_id = srf.sys_dataview_id
	left join sys_table_field stf
		on srf.sys_table_field_id = stf.sys_table_field_id
	left join sys_table st
		on stf.sys_table_id = st.sys_table_id
where
	coalesce(sr.dataview_name, '\b') = coalesce(:dataview, sr.dataview_name, '\b')
order by
	srf.sort_order
", dsReturn, "dv_field_info", new DataParameters(":dataview", String.IsNullOrEmpty(dataviewName) ? null : dataviewName));


                    // pull available language(s)
                    dm.Read(@"
select
	*
from
	sys_lang
order by
	title
", dsReturn, "sys_lang");



                    // add available language(s) to the dv_field_info table as a column
                    DataTable dtRsFieldInfo = dsReturn.Tables["dv_field_info"];
                    foreach (DataRow dr in dsReturn.Tables["sys_lang"].Rows) {
                        dtRsFieldInfo.Columns.Add("friendly_name_for_" + dr["sys_lang_id"].ToString(), typeof(string));

                    }

                    // for each field, pull all the friendly table field names for all languages
                    foreach (DataRow drField in dtRsFieldInfo.Rows) {
                        DataTable dtFriendly = dm.Read(@"
select
	stfl.sys_lang_id,
	stfl.title
from
	sys_dataview_field sdf inner join sys_table_field stf
        on sdf.sys_table_field_id = stf.sys_table_field_id
    inner join sys_table_field_lang stfl
        on stf.sys_table_field_id = stfl.sys_table_field_id
where
	sdf.sys_dataview_field_id = :rsfieldid
", new DataParameters(":rsfieldid", Toolkit.ToInt32(drField["sys_dataview_field_id"], -1), DbType.Int32));

                        foreach (DataRow drFriendly in dtFriendly.Rows) {
                            if (dtRsFieldInfo.Columns.Contains("friendly_name_for_" + drFriendly["sys_lang_id"].ToString())) {
                                drField["friendly_name_for_" + drFriendly["sys_lang_id"]] = drFriendly["title"];
                            }
                        }
                    }

                    // for each field, pull all the friendly dataview field names for all languages (they override the table field ones)
                    foreach (DataRow drField in dtRsFieldInfo.Rows) {
                        DataTable dtFriendly = dm.Read(@"
select
	sys_lang_id,
	title
from
	sys_dataview_field_lang
where
	sys_dataview_field_id = :rsfieldid
", new DataParameters(":rsfieldid", Toolkit.ToInt32(drField["sys_dataview_field_id"], -1), DbType.Int32));

                        foreach (DataRow drFriendly in dtFriendly.Rows) {
                            if (dtRsFieldInfo.Columns.Contains("friendly_name_for_" + drFriendly["sys_lang_id"].ToString())) {
                                drField["friendly_name_for_" + drFriendly["sys_lang_id"]] = drFriendly["title"];
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }


        public DataSet GetTableFieldDependencies(string tableName, string fieldName) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    checkUserHasAdminEnabled();

                    var dtFk = dm.Read(@"
select
    concat('Dataview/Field: ', sd.dataview_name, '.', sdf.field_name) as display_member,
    sdf.sys_dataview_field_id as field_id,
    sd.dataview_name as dataview_name
from
    sys_table st inner join sys_table_field stf 
        on stf.sys_table_id = st.sys_table_id
    inner join sys_dataview_field sdf 
        on stf.sys_table_field_id = sdf.sys_table_field_id
    inner join sys_dataview sd
        on sdf.sys_dataview_id = sd.sys_dataview_id
where
    st.table_name = :table1 and stf.field_name = :field1
order by 
    1
", dsReturn, "table_field_dependencies", new DataParameters(":table1", tableName, ":field1", fieldName));
                    return dtFk;
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }


        private DataTable getDependentDataviewsAndTables(string fullyQualifiedDataViewName, DataManager dm) {
            var dtFk = dm.Read(@"
select
    'dataview' as source,
    sd.sys_dataview_id as source_id,
    sdf.sys_dataview_field_id as field_id,
    sd.dataview_name as name,
    concat('Dataview: ', sd.dataview_name) as display_member
from
    sys_dataview_field sdf inner join sys_dataview sd
        on sdf.sys_dataview_id = sd.sys_dataview_id
where
    sdf.foreign_key_dataview_name = :dvname1
union
select
    'table' as source,
    st.sys_table_id as source_id,
    stf.sys_table_field_id as field_id,
    st.table_name as name,
    concat('Table: ', st.table_name) as display_member
from
    sys_table_field stf inner join sys_table st
        on stf.sys_table_id = st.sys_table_id
    inner join sys_dataview_field sdf
        on stf.sys_table_field_id = sdf.sys_table_field_id
    inner join sys_dataview sd
        on sdf.sys_dataview_id = sd.sys_dataview_id
where
    stf.foreign_key_dataview_name = :dvname2
order by
    1, 2
", new DataParameters(":dvname1", fullyQualifiedDataViewName, ":dvname2", fullyQualifiedDataViewName));
            return dtFk;
        }

        private DataTable getDependentPermissions(string fullyQualifiedDataViewName, DataManager dm) {
            var dtPerms = dm.Read(@"
select 
    'permission' as source,
    sp.sys_permission_id, 
    concat('Permission: ', coalesce(sp.permission_tag,concat('(for ', sd.dataview_name, ')'))) as display_member
from 
    sys_permission sp inner join sys_dataview sd 
        on sp.sys_dataview_id = sd.sys_dataview_id 
where 
    sd.dataview_name = :dataview"
                , new DataParameters(":dataview", fullyQualifiedDataViewName));
            return dtPerms;
        }

        public DataSet GetDataViewDependencies(string fullyQualifiedDataViewName) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    checkUserHasAdminEnabled();

                    var dtRet = new DataTable("dataview_dependencies");
                    dtRet.Columns.Add("source", typeof(string));
                    dtRet.Columns.Add("display_member", typeof(string));
                    dsReturn.Tables.Add(dtRet);


                    var dtFK = getDependentDataviewsAndTables(fullyQualifiedDataViewName, dm);
                    foreach (DataRow drFK in dtFK.Rows) {
                        dtRet.Rows.Add(drFK["source"], drFK["display_member"]);
                    }

                    var dtPerms = getDependentPermissions(fullyQualifiedDataViewName, dm);
                    foreach (DataRow drPerm in dtPerms.Rows) {
                        dtRet.Rows.Add(drPerm["source"], drPerm["display_member"]);
                    }

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }

        public DataSet DeleteDataViewDefinition(string fullyQualifiedDataViewName, bool forceDelete) {

            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {

                    dm.BeginTran();
                    checkUserHasAdminEnabled();

                    // if it's a system dataview, don't let them delete it.
                    bool isSystem = Toolkit.ToBoolean(dm.ReadValue("select category_code from sys_dataview where dataview_name = :dataview", new DataParameters(":dataview", fullyQualifiedDataViewName)).ToString().ToUpper().Trim() == "SYSTEM", false);
                    if (isSystem) {
                        throw Library.CreateBusinessException(getDisplayMember("DeleteDataViewDefinition{system}", "You can not delete a dataview that is required by the system."));
                    }

                    var dtFk = getDependentDataviewsAndTables(fullyQualifiedDataViewName, dm);

                    if (dtFk.Rows.Count > 0) {
                        // at least one dataview or table is pointing at the one we're trying to delete.
                        // either clear the references or throw an exception.
                        if (!forceDelete) {
                            var refs = dtFk.ListColumnValues("display_member", true);
                            throw new InvalidOperationException(getDisplayMember("DeleteDataViewDefinition", "The following are referencing {0}, so it could not be deleted:\r\n\r\n{1}", fullyQualifiedDataViewName, String.Join("\r\n", refs.ToArray())));
                        } else {
                            foreach (DataRow dr in dtFk.Rows) {
                                if (dr["source"].ToString() == "dataview") {
                                    // remove the fkdv from sys_dataview_field and set gui_hint to free form text...
                                    dm.Write(@"
update
    sys_dataview_field
set
    gui_hint = null,
    foreign_key_dataview_name = null,
    modified_date = :now,
    modified_by = :who
where
    sys_dataview_field_id = :dvfid
", new DataParameters(":now", DateTime.UtcNow, DbType.DateTime2,
     ":who", CooperatorID, DbType.Int32,
     ":dvfid", Toolkit.ToInt32(dr["field_id"], -1), DbType.Int32));
                                } else if (dr["source"].ToString() == "table") {
                                    // remove the fkdv from sys_table_field and set gui_hint to free form text...
                                    dm.Write(@"
update
    sys_table_field
set
    gui_hint = null,
    foreign_key_dataview_name = null,
    modified_date = :now,
    modified_by = :who
where
    sys_table_field_id = :dvfid
", new DataParameters(":now", DateTime.UtcNow, DbType.DateTime2,
     ":who", CooperatorID, DbType.Int32,
     ":dvfid", Toolkit.ToInt32(dr["field_id"], -1), DbType.Int32));
                                }
                            }
                        }
                    }


                    var dtPerms = getDependentPermissions(fullyQualifiedDataViewName, dm);
                    if (dtPerms.Rows.Count > 0) {
                        if (!forceDelete) {
                            // tell user there are permission(s) directly tied to this dataview
                            var cols = new List<string>();
                            foreach (DataRow dr in dtPerms.Rows) {
                                cols.Add(dr["display_text"].ToString());
                            }
                            throw Library.CreateBusinessException(getDisplayMember("DeleteDataViewDefinition{permissions}", "{0} permission(s) exist that are specifically tied to this dataview.  You must remove the following permissions before continuing: {1}", dtPerms.Rows.Count.ToString(), Toolkit.Join(cols.ToArray(), ", ", "")));
                        } else {
                            // delete permission(s) directly tied to this dataview
                            foreach (DataRow dr in dtPerms.Rows) {
                                var permID = Toolkit.ToInt32(dr["sys_permission_id"], -1);
                                dm.Write("delete from sys_permission_field where sys_permission_id = :permid", new DataParameters(":permid", permID, DbType.Int32));
                                dm.Write("delete from sys_permission_lang where sys_permission_id = :permid", new DataParameters(":permid", permID, DbType.Int32));
                                dm.Write("delete from sys_user_permission_map where sys_permission_id = :permid", new DataParameters(":permid", permID, DbType.Int32));
                                dm.Write("delete from sys_group_permission_map where sys_permission_id = :permid", new DataParameters(":permid", permID, DbType.Int32));
                                dm.Write("delete from sys_permission where sys_permission_id = :permid", new DataParameters(":permid", permID, DbType.Int32));
                            }
                        }
                    }



                    // remove dataview friendly field mappings
                    dm.Write(@"
delete from
	sys_dataview_field_lang
where
	sys_dataview_field_id in (
		select sys_dataview_field_id from sys_dataview_field where sys_dataview_id in (
			select sys_dataview_id from sys_dataview where dataview_name = :name
		)
	)
", new DataParameters(":name", fullyQualifiedDataViewName));

                    // remove dataview field mappings
                    dm.Write(@"
delete from 
	sys_dataview_field 
where 
	sys_dataview_id in (
		select sys_dataview_id from sys_dataview where dataview_name = :name
	)", new DataParameters(":name", fullyQualifiedDataViewName));

                    // remove dataview param mappings
                    dm.Write(@"
delete from
	sys_dataview_param
where
	sys_dataview_id in (
		select sys_dataview_id from sys_dataview where dataview_name = :name
	)", new DataParameters(":name", fullyQualifiedDataViewName));

                    // remove dataview sql mappings
                    dm.Write(@"
delete from
	sys_dataview_sql
where
	sys_dataview_id in (
		select sys_dataview_id from sys_dataview where dataview_name = :name
	)", new DataParameters(":name", fullyQualifiedDataViewName));


                    // remove datatrigger lang mappings
                    dm.Write(@"
delete from 
	sys_datatrigger_lang
where 
    sys_datatrigger_id in (
	    select sys_datatrigger_id from sys_datatrigger where sys_dataview_id in (
    		select sys_dataview_id from sys_dataview where dataview_name = :name
	    )
    )", new DataParameters(":name", fullyQualifiedDataViewName));
                    
                    // remove datatrigger mappings
                    dm.Write(@"
delete from 
	sys_datatrigger
where 
	sys_dataview_id in (
		select sys_dataview_id from sys_dataview where dataview_name = :name
	)", new DataParameters(":name", fullyQualifiedDataViewName));

                    // remove dataview lang mappings
                    dm.Write(@"
delete from
	sys_dataview_lang
where
	sys_dataview_id in (
		select sys_dataview_id from sys_dataview where dataview_name = :name
	)", new DataParameters(":name", fullyQualifiedDataViewName));



                    //                // remove dataview to table mappings
                    //                sd.DataManager.Write(@"
                    //delete from 
                    //	sys_dataview_table 
                    //where 
                    //	sys_dataview_id in (
                    //		select sys_dataview_id from sys_dataview where dataview_name = :name
                    //	)", new DataParameters(":name", fullyQualifiedDataViewName));

                    // remove dataview mapping
                    dm.Write(@"
delete from 
	sys_dataview 
where 
	dataview_name = :name", new DataParameters(":name", fullyQualifiedDataViewName));


                    dm.Commit();

                }


                // clear all caches to make sure our changes are immediately visible
                clearCache(null);


            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }

        private void appendRow(DataSet target, DataRow source, string primaryKeyFieldName, int databaseID) {
            if (!target.Tables.Contains(source.Table.TableName)) {
                throw Library.CreateBusinessException(getDisplayMember("appendRow{tablenotfound}", "target dataset does not contain a table named '{0}'. Cannot append row to insert.", source.Table.TableName ));
            } else {

                // create a new, empty row, add it to the table (the RowState will show as Added)
                DataTable destTable = target.Tables[source.Table.TableName];
                DataRow clone = destTable.NewRow();

                // initialize all non-pk fields to some non-dbnull value
                // this will cause all the fields to have their original value not match their current value,
                // meaning SaveData will update every field
                foreach (DataColumn dc in clone.Table.Columns) {
                    string colName = dc.ColumnName.ToLower();
                    if (dc.DataType == typeof(string)) {
                        // \0 means null, something somebody can never type in ("\0" != "")
                        clone[colName] = "\0";
                    } else if (dc.DataType == typeof(DateTime)) {
                        // we add 1 so if they're using MinValue as a flag, the column is still marked as having changed
                        clone[colName] = DateTime.MinValue.AddDays(1);
                    } else if (dc.DataType == typeof(int)) {
                        // we add 1 so if they're using MinValue as a flag, the column is still marked as having changed
                        clone[colName] = int.MinValue + 1;
                    } else if (dc.DataType == typeof(decimal)) {
                        // we add 1 so if they're using MinValue as a flag, the column is still marked as having changed
                        clone[colName] = decimal.MinValue + 1;
                    } else if (dc.DataType == typeof(float)) {
                        // we add 1 so if they're using MinValue as a flag, the column is still marked as having changed
                        clone[colName] = float.MinValue + 1;
                    } else if (dc.DataType == typeof(double)) {
                        // we add 1 so if they're using MinValue as a flag, the column is still marked as having changed
                        clone[colName] = double.MinValue + 1;
                    }
                }

                destTable.Rows.Add(clone);

                if (databaseID > 0) {
                    // if we accept changes, then the loop below will alter the row data and the RowState will become Modified (instead of Added)
                    clone.AcceptChanges();
                }

                foreach (DataColumn dc in clone.Table.Columns) {
                    if (dc.ReadOnly) {
                        dc.ReadOnly = false;
                    }
                    if (String.Compare(dc.ColumnName, primaryKeyFieldName, true) == 0) {
                        // pk field value is special...
                        if (databaseID < 0) {
                            // we need to retain the original pk value so it can be mapped to the new pk value later...
                            clone[primaryKeyFieldName] = source[dc.ColumnName];
                        } else {
                            // use databaseID instead of source[primaryKeyFieldName] because they will probably be different
                            // (can't guarantee auto-generated id from one database matches that from another for the same data)
                            clone[primaryKeyFieldName] = databaseID;
                        }
                    } else {
                        // non-pk field value.
                        if (String.Compare(dc.ColumnName, "created_date", true) == 0 ||
                            String.Compare(dc.ColumnName, "modified_date", true) == 0 ||
                            String.Compare(dc.ColumnName, "owned_date", true) == 0) {
                            // audit date, timestamp with now so we know when it was imported
                            clone[dc.ColumnName] = DateTime.UtcNow;
                        } else if (String.Compare(dc.ColumnName, "created_by", true) == 0 ||
                            String.Compare(dc.ColumnName, "modified_by", true) == 0 ||
                            String.Compare(dc.ColumnName, "owned_by", true) == 0) {
                            // audit user, mark with current user so we know who imported it
                            clone[dc.ColumnName] = CooperatorID;
                        } else {
                            clone[dc.ColumnName] = source[dc.ColumnName];
                        }
                    }
                }

            }
        }


        private int getDataViewIDByName(DataManager dm, string dvName) {
            return Toolkit.ToInt32(dm.ReadValue("select sys_dataview_id from sys_dataview where dataview_name = :dataview", new DataParameters(":dataview", dvName)), -1);
        }

        private int getDataViewParamIDByName(DataManager dm, int dvID, string paramName) {
            return Toolkit.ToInt32(dm.ReadValue(@"
select 
    sys_dataview_param_id 
from 
    sys_dataview_param
where 
    sys_dataview_id = :rsid 
    and param_name = :paramname
", new DataParameters(":rsid", dvID, DbType.Int32, ":paramname", paramName)), -1);

        }

        private int getDataViewSqlIDByEngineName(DataManager dm, int dvID, string engineCode) {
            return Toolkit.ToInt32(dm.ReadValue(@"
select 
    sys_dataview_sql_id 
from 
    sys_dataview_sql
where 
    sys_dataview_id = :rsid 
    and database_engine_tag = :engineCode
", new DataParameters(":rsid", dvID, DbType.Int32, ":engineCode", engineCode)), -1);

        }

        private int getDataViewFieldIDByName(DataManager dm, int dvID, string fieldName) {
            return Toolkit.ToInt32(dm.ReadValue(@"
select 
    srf.sys_dataview_field_id 
from 
    sys_dataview_field srf 
where 
    srf.sys_dataview_id = :rsid 
    and srf.field_name = :fieldname
", new DataParameters(":rsid", dvID, DbType.Int32, ":fieldname", fieldName)), -1);

        }

        private int getLangIDByIsoCode(DataManager dm, string isoCode) {
            return Toolkit.ToInt32(dm.ReadValue(@"
select 
    sys_lang_id
from 
    sys_lang
where 
    iso_639_3_tag = :isocode
", new DataParameters(":isocode", isoCode)), -1);
        }

        private int getDataViewLangIDByIsoCode(DataManager dm, int dvID, string isoCode) {
            return Toolkit.ToInt32(dm.ReadValue(@"
select 
    srl.sys_dataview_lang_id
from 
    sys_dataview_lang srl left join sys_lang sl
        on srl.sys_lang_id = sl.sys_lang_id
where 
    srl.sys_dataview_id = :rsid
    and sl.iso_639_3_tag = :isocode
", new DataParameters(":rsid", dvID, DbType.Int32, ":isocode", isoCode)), -1);
        }

        private int getDataViewFieldLangIDByFieldID(DataManager dm, int rsFieldID, int secLangID) {
            return Toolkit.ToInt32(dm.ReadValue(@"
select 
    srfl.sys_dataview_field_lang_id 
from 
    sys_dataview_field_lang srfl
where 
    sys_dataview_field_id = :rsfieldid
    and sys_lang_id = :langid
", new DataParameters(":rsfieldid", rsFieldID, DbType.Int32, ":langid", secLangID)), -1);

        }

        private int getTableFieldLangIDByFieldID(DataManager dm, int tableFieldID, int sysLangID) {
            return Toolkit.ToInt32(dm.ReadValue(@"
select 
    stfl.sys_table_field_lang_id 
from 
    sys_table_field_lang stfl
where 
    sys_table_field_id = :tablefieldid
    and sys_lang_id = :langid
", new DataParameters(":tablefieldid", tableFieldID, DbType.Int32, ":langid", sysLangID)), -1);

        }


        private int getTableIDByName(DataManager dm, string tableName) {
            return Toolkit.ToInt32(dm.ReadValue("select sys_table_id from sys_table where table_name = :tablename", new DataParameters(":tablename", tableName)), -1);
        }


        private int getTableFieldIDByName(DataManager dm, string tableName, string fieldName) {
            return Toolkit.ToInt32(dm.ReadValue(@"
select
    sys_table_field_id
from
    sys_table_field stf inner join sys_table st
        on stf.sys_table_id = st.sys_table_id
where
    st.table_name = :tablename
    and stf.field_name = :fieldname
", new DataParameters(":tablename", tableName, ":fieldname", fieldName)), -1);

        }


        private List<int> getNewPrimaryKeyValues(DataTable dt) {
            var ret = new List<int>();
            foreach (DataRow dr in dt.Rows) {
                ret.Add(Toolkit.ToInt32(dr["NewPrimaryKeyID"].ToString(), -1));
            }
            return ret;
        }

        [DebuggerStepThrough()]
        private object getRowValue(DataRow dr, string columnName, object defaultValue) {
            if (dr.Table.Columns.Contains(columnName)) {
                return dr[columnName];
            } else {
                return defaultValue;
            }
        }

        /// <summary>
        /// If given columnName exists in dr, returns "Y" if the value is "Y", "N" otherwise.  If columnName does not exist in dr, returns "Y" if defaultValue is true, "N" otherwise.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [DebuggerStepThrough()]
        private string getRowToggleValue(DataRow dr, string columnName, bool defaultValue) {
            var val = getRowValue(dr, columnName, null);
            if (val == null) {
                return defaultValue ? "Y" : "N";
            } else {
                return val.ToString().ToUpper() == "Y" ? "Y" : "N";
            }
        }

        public static void UpconvertDataviewForImport(DataSet ds) {
            foreach (DataTable dt in ds.Tables) {
                if (dt.TableName.StartsWith("sec_")) {
                    dt.TableName = dt.TableName.Replace("sec_", "sys_");
                }

                foreach (DataColumn dc in dt.Columns) {
                    if (dc.ColumnName.StartsWith("sec_")) {
                        dc.ColumnName = dc.ColumnName.Replace("sec_", "sys_");
                    } else {
                        switch (dc.ColumnName) {
                            case "db_engine_code":
                                dc.ColumnName = "database_engine_tag";
                                break;
                            case "group_code":
                                dc.ColumnName = "group_tag";
                                break;
                            case "perm_code":
                                dc.ColumnName = "permission_tag";
                                break;
                            case "create_perm":
                                dc.ColumnName = "create_permission";
                                break;
                            case "read_perm":
                                dc.ColumnName = "read_permission";
                                break;
                            case "update_perm":
                                dc.ColumnName = "update_permission";
                                break;
                            case "delete_perm":
                                dc.ColumnName = "delete_permission";
                                break;
                            case "code_group_code":
                                dc.ColumnName = "group_name";
                                break;
                            case "category_sort_code":
                                dc.ColumnName = "database_area_code_sort_order";
                                break;
                            case "iso_639_3_code":
                                dc.ColumnName = "iso_639_3_tag";
                                break;
                            case "category_name":
                                dc.ColumnName = "category_code";
                                break;
                            case "database_area":
                                dc.ColumnName = "database_area_code";
                                break;
                        }
                    }
                }
            }
        }

        public virtual DataSet ImportDataViewDefinitions(DataSet dsDataViewDefinitions, List<string> dvNames, bool ignoreMissingFieldMappings, bool includeFieldsAndParameters, bool includeLanguageData, bool useInTableMappings) {

            // only people with 'admin' permission can run this...
            checkUserHasAdminEnabled();

            // 2010-06-14 Brock Weaver brock@circaware.com
            // convert schema from old-style "sec_" to new-style "sys_"
            UpconvertDataviewForImport(dsDataViewDefinitions);

            if (dvNames == null || dvNames.Count == 0) {

                dvNames = new List<string>();

                // assume all dataviews in the ds are to be imported
                foreach (DataRow dr in dsDataViewDefinitions.Tables["sys_dataview"].Rows) {
                    dvNames.Add(dr["dataview_name"].ToString());
                }

            }


            DataSet dsReturn = createReturnDataSet();
            var dtImported = new DataTable("imported");
            dtImported.Columns.Add("sys_dataview_id", typeof(int));
            dtImported.Columns.Add("dataview_name", typeof(string));
            dsReturn.Tables.Add(dtImported);
            try {

                if (!dsDataViewDefinitions.Tables.Contains("sys_dataview")) {
                    throw Library.CreateBusinessException(getDisplayMember("ImportDataviewDefinition{sys_dataview}", "sys_dataview table is missing from dataview definition dataset."));
                }
                if (!dsDataViewDefinitions.Tables.Contains("sys_dataview_sql")) {
                    throw Library.CreateBusinessException(getDisplayMember("ImportDataviewDefinition{sys_dataview_sql}", "sys_dataview_sql table is missing from dataview definition dataset."));
                }
                if (!dsDataViewDefinitions.Tables.Contains("sys_dataview_lang")) {
                    throw Library.CreateBusinessException(getDisplayMember("ImportDataviewDefinition{sys_dataview_lang}", "sys_dataview_lang table is missing from dataview definition dataset."));
                }
                if (!dsDataViewDefinitions.Tables.Contains("sys_dataview_field")) {
                    throw Library.CreateBusinessException(getDisplayMember("ImportDataviewDefinition{sys_dataview_field}", "sys_dataview_field table is missing from dataview definition dataset."));
                }
                if (!dsDataViewDefinitions.Tables.Contains("sys_dataview_field_lang")) {
                    throw Library.CreateBusinessException(getDisplayMember("ImportDataviewDefinition{sys_dataview_field_lang}", "sys_dataview_field_lang table is missing from dataview definition dataset."));
                }
                if (!dsDataViewDefinitions.Tables.Contains("sys_dataview_param")) {
                    throw Library.CreateBusinessException(getDisplayMember("ImportDataviewDefinition{sys_dataview_param}", "sys_dataview_param table is missing from dataview definition dataset."));
                }
                if (!dsDataViewDefinitions.Tables.Contains("sys_table")) {
                    throw Library.CreateBusinessException(getDisplayMember("ImportDataviewDefinition{sys_table}", "sys_table table is missing from dataview definition dataset."));
                }
                if (!dsDataViewDefinitions.Tables.Contains("sys_table_field")) {
                    throw Library.CreateBusinessException(getDisplayMember("ImportDataviewDefinition{sys_table_field}", "sys_table_field table is missing from dataview definition dataset."));
                }

                using (DataManager dm = BeginProcessing(true)) {

                    dm.BeginTran();

                    // create a new, empty dataset by cloning the given imported one.
                    // this allows us to pass the dataset to the SaveData method to have it do its magic
                    // so we don't have to handcraft the sql here (and remember to update it as schema changes are made)



                    // first, fill sys_dataview records in dataset to update / insert as needed.
                    DataTable dtSecRS = dsDataViewDefinitions.Tables["sys_dataview"];
                    foreach (DataRow drDV in dtSecRS.Rows) {

                        string dvName = drDV["dataview_name"].ToString();
                        int oldRSID = Toolkit.ToInt32(drDV["sys_dataview_id"], -1);

                        if (dvNames.Contains(dvName)) {

                            // this is one we are to import.
                            // create a new, empty dataset by cloning the given imported one.
                            // this allows us to pass the dataset to the SaveData method to have it do its magic
                            // so we dont' have to handcraft the sql here (and remmeber to change it as schema changes are applied)

                            // remember which record(s) should belong to dataview after import in each table
                            var validDVFieldIDs = new List<int>();
                            var validDVFieldLangIDs = new List<int>();
                            var validDVParamIDs = new List<int>();
                            var validDVSqlIDs = new List<int>();
                            var validDVLangIDs = new List<int>();

                            // remember detail info about why dataview could not be imported
                            var missingInfo = new StringBuilder();
                            bool missingInfoIsShowstopper = false;


                            // update/insert sys_dataview record, remember new ID
                            int newDVID = getDataViewIDByName(dm, dvName);

                            if (newDVID < 0) {

                                newDVID = dm.Write(@"
insert into sys_dataview
(dataview_name, is_enabled, is_readonly, category_code, database_area_code, database_area_code_sort_order, is_transform, transform_field_for_names, transform_field_for_captions, transform_field_for_values, configuration_options, created_date, created_by, owned_date, owned_by)
values
(:dvname, :isenabled, :isreadonly, :catname, :dbarea, :dbsort, :istransform, :tffn, :tffc, :tffv, :configoptions, :now1, :who1, :now2, :who2)
", true, "sys_dataview_id", new DataParameters(
     ":dvname", getRowValue(drDV, "dataview_name", DBNull.Value).ToString(),
     ":isenabled", getRowToggleValue(drDV, "is_enabled", false),
     ":isreadonly", getRowToggleValue(drDV, "is_readonly", true),
     ":catname", getRowValue(drDV, "category_code", DBNull.Value),
     ":dbarea", getRowValue(drDV, "database_area_code", DBNull.Value),
     ":dbsort", getRowValue(drDV, "database_area_code_sort_order", DBNull.Value),
     ":istransform", getRowToggleValue(drDV, "is_transform", false),
     ":tffn", getRowValue(drDV, "transform_field_for_names", DBNull.Value),
     ":tffc", getRowValue(drDV, "transform_field_for_captions", DBNull.Value),
     ":tffv", getRowValue(drDV, "transform_field_for_values", DBNull.Value),
     ":configoptions", getRowValue(drDV, "configuration_options", DBNull.Value),
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":who1", CooperatorID, DbType.Int32,
     ":now2", DateTime.UtcNow, DbType.DateTime2,
     ":who2", CooperatorID, DbType.Int32));



                                // when we insert a new dataview, that dataview didn't exist.
                                // even if the caller said not to include the Fields / Parameters / language stuff,
                                // we're overriding that to include it.
                                // But we mark the "use in table mappings" to false as well so we don't stomp over anything they have
                                includeFieldsAndParameters = true;
                                includeLanguageData = true;
                                useInTableMappings = false;

                            } else {

                                if (includeFieldsAndParameters) {

                                    dm.Write(@"
update
    sys_dataview
set
    dataview_name = :dvname,
    is_enabled = :isenabled,
    is_readonly = :isreadonly,
    category_code = :catname,
    database_area_code = :dbarea,
    database_area_code_sort_order = :dbareasort,
    is_transform = :istransform,
    transform_field_for_names = :tffn,
    transform_field_for_captions = :tffc,
    transform_field_for_values = :tffv,
    configuration_options = :configoptions,
    modified_date = :now1,
    modified_by = :who1
where
    sys_dataview_id = :dvid
", new DataParameters(
         ":dvname", getRowValue(drDV, "dataview_name", DBNull.Value).ToString(),
         ":isenabled", getRowToggleValue(drDV, "is_enabled", false),
         ":isreadonly", getRowToggleValue(drDV, "is_readonly", true),
         ":catname", getRowValue(drDV, "category_code", DBNull.Value),
         ":dbarea", getRowValue(drDV, "database_area_code", DBNull.Value),
         ":dbareasort", getRowValue(drDV, "database_area_code_sort_order", DBNull.Value),
         ":istransform", getRowToggleValue(drDV, "is_transform", false),
         ":tffn", getRowValue(drDV, "transform_field_for_names", DBNull.Value),
         ":tffc", getRowValue(drDV, "transform_field_for_captions", DBNull.Value),
         ":tffv", getRowValue(drDV, "transform_field_for_values", DBNull.Value),
         ":configoptions", getRowValue(drDV, "configuration_options", DBNull.Value),
         ":now1", DateTime.UtcNow, DbType.DateTime2,
         ":who1", CooperatorID, DbType.Int32,
         ":dvid", newDVID, DbType.Int32));
                                }
                            }




                            // update/insert sys_dataview_param record(s)
                            DataRow[] paramRows = dsDataViewDefinitions.Tables["sys_dataview_param"].Select("dataview_name = '" + dvName + "'");
                            if (paramRows != null) {
                                foreach (DataRow drParam in paramRows) {
                                    int newDVParamID = getDataViewParamIDByName(dm, newDVID, drParam["param_name"].ToString());

                                    if (includeFieldsAndParameters) {
                                        if (newDVParamID < 0) {
                                            newDVParamID = dm.Write(@"
insert into sys_dataview_param
(sys_dataview_id, param_name, param_type, sort_order, created_date, created_by, owned_date, owned_by)
values
(:dvid, :prmname, :prmtype, :sortorder, :now1, :who1, :now2, :who2)
", true, "sys_dataview_param_id", new DataParameters(
             ":dvid", newDVID, DbType.Int32,
             ":prmname", getRowValue(drParam, "param_name", "").ToString(),
             ":prmtype", getRowValue(drParam, "param_type", "").ToString(),
             ":sortorder", getRowValue(drParam, "sort_order", 0), DbType.Int32,
             ":now1", DateTime.UtcNow, DbType.DateTime2,
             ":who1", CooperatorID, DbType.Int32,
             ":now2", DateTime.UtcNow, DbType.DateTime2,
             ":who2", CooperatorID, DbType.Int32
             ));
                                            } else {
                                                dm.Write(@"
update 
    sys_dataview_param
set
    sys_dataview_id = :dvid,
    param_name = :prmname,
    param_type = :prmtype,
    sort_order = :sortorder,
    modified_date = :now1,
    modified_by = :who1
where
    sys_dataview_param_id = :prmid
", new DataParameters(
             ":dvid", newDVID, DbType.Int32,
             ":prmname", getRowValue(drParam, "param_name", "").ToString(),
             ":prmtype", getRowValue(drParam, "param_type", "").ToString(),
             ":sortorder", getRowValue(drParam, "sort_order", 0), DbType.Int32,
             ":now1", DateTime.UtcNow, DbType.DateTime2,
             ":who1", CooperatorID, DbType.Int32,
             ":prmid", newDVParamID, DbType.Int32));
                                        }
                                    }


                                    validDVParamIDs.Add(newDVParamID);
                                }
                            }

                            // update/insert sys_dataview_sql record(s)
                            DataRow[] sqlRows = dsDataViewDefinitions.Tables["sys_dataview_sql"].Select("dataview_name = '" + dvName + "'");
                            if (sqlRows != null) {
                                foreach (DataRow drSql in sqlRows) {

                                    int newDVSqlID = getDataViewSqlIDByEngineName(dm, newDVID, drSql["database_engine_tag"].ToString());
                                    if (!String.IsNullOrEmpty(drSql["sql_statement"].ToString().Trim())) {
                                        // only change the sql statement if it's not empty (for easily importing sql only for a given database engine)


                                        if (newDVSqlID < 0) {
                                            newDVSqlID = dm.Write(@"
insert into sys_dataview_sql
(sys_dataview_id, database_engine_tag, sql_statement, created_date, created_by, owned_date, owned_by)
values
(:dvid, :dbengine, :sqlstatement, :now1, :who1, :now2, :who2)
", true, "sys_dataview_sql_id", new DataParameters(
     ":dvid", newDVID, DbType.Int32,
     ":dbengine", getRowValue(drSql, "database_engine_tag", ""),
     ":sqlstatement", getRowValue(drSql, "sql_statement", ""),
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":who1", CooperatorID, DbType.Int32,
     ":now2", DateTime.UtcNow, DbType.DateTime2,
     ":who2", CooperatorID, DbType.Int32));
                                        } else {
                                            dm.Write(@"
update
    sys_dataview_sql
set
    sys_dataview_id = :dvid,
    database_engine_tag = :dbengine,
    sql_statement = :sqlstatement,
    modified_date = :now1,
    modified_by = :who1
where
    sys_dataview_sql_id = :id
", new DataParameters(
     ":dvid", newDVID, DbType.Int32,
     ":dbengine", getRowValue(drSql, "database_engine_tag", "").ToString(),
     ":sqlstatement", getRowValue(drSql, "sql_statement", "").ToString(),
     ":now1", DateTime.UtcNow, DbType.DateTime2,
     ":who1", CooperatorID, DbType.Int32,
     ":id", newDVSqlID, DbType.Int32));
                                        }

                                    }

                                    validDVSqlIDs.Add(newDVSqlID);

                                }
                            }

                            // insert sys_dataview_lang record(s)
                            DataRow[] dvLangRows = dsDataViewDefinitions.Tables["sys_dataview_lang"].Select("dataview_name = '" + dvName + "'");
                            if (dvLangRows != null) {
                                foreach (DataRow drLangRow in dvLangRows) {
                                    int newLangID = getLangIDByIsoCode(dm, drLangRow["iso_639_3_tag"].ToString());
                                    int newDVLangID = getDataViewLangIDByIsoCode(dm, newDVID, drLangRow["iso_639_3_tag"].ToString());

                                    // 2009-09-15 Brock Weaver brock@circaware.com
                                    // changing to just queue up informational detail instead of auto-creating the sys_lang record.
                                    // still creates the sys_dataview_lang record if sys_lang record exists though.

                                    DataSet dsSaveDVLang = dsDataViewDefinitions.Clone();
                                    drLangRow["sys_dataview_id"] = newDVID;
                                    drLangRow["sys_lang_id"] = newLangID;
                                    if (newLangID < 0) {

                                        // language not found in this db, ignore / continue
                                        missingInfo.AppendLine(getDisplayMember("ImportDataviewDefinition{sys_lang}", "sys_lang record for {0} required by dataview {1} is missing in the target database.", drLangRow["iso_639_3_tag"].ToString(), dvName));

                                    } else {

                                        if (includeLanguageData) {

                                            if (newDVLangID < 0) {
                                                newDVLangID = dm.Write(@"
insert into sys_dataview_lang
(sys_dataview_id, sys_lang_id, title, description, created_date, created_by, owned_date, owned_by)
values
(:dvid, :langid, :title, :description, :now1, :who1, :now2, :who2)
", true, "sys_dataview_lang_id", new DataParameters(
         ":dvid", newDVID, DbType.Int32,
         ":langid", newLangID, DbType.Int32,
         ":title", getRowValue(drLangRow, "title", "").ToString(),
         ":description", getRowValue(drLangRow, "description", "").ToString(),
         ":now1", DateTime.UtcNow, DbType.DateTime2,
         ":who1", CooperatorID, DbType.Int32,
         ":now2", DateTime.UtcNow, DbType.DateTime2,
         ":who2", CooperatorID, DbType.Int32
         ));
                                            } else {
                                                dm.Write(@"
update 
    sys_dataview_lang
set
    sys_dataview_id = :dvid,
    sys_lang_id = :langid,
    title = :title,
    description = :description,
    modified_date = :now1,
    modified_by = :who1
where
    sys_dataview_lang_id = :id
", new DataParameters(
         ":dvid", newDVID, DbType.Int32,
         ":langid", newLangID, DbType.Int32,
         ":title", getRowValue(drLangRow, "title", "").ToString(),
         ":description", getRowValue(drLangRow, "description", "").ToString(),
         ":now1", DateTime.UtcNow, DbType.DateTime2,
         ":who1", CooperatorID, DbType.Int32,
         ":id", newDVLangID, DbType.Int32));
                                            }
                                        }

                                        if (newDVLangID > -1) {
                                            validDVLangIDs.Add(newDVLangID);
                                        }
                                    }

                                }
                            }


                            // due to the relationship of dataview -> dv_field -> table_field -> table, we need to queue up
                            // and save sys_table stuff before table_field or dv_field

                            // spin through dataview field rows to get unique table/table_field names
                            var uniqueTableNames = new List<string>();
                            var uniqueTableFieldNames = new List<string>();
                            DataRow[] dvFieldRows = dsDataViewDefinitions.Tables["sys_dataview_field"].Select("dataview_name = '" + dvName + "'");
                            if (dvFieldRows != null && dvFieldRows.Length > 0) {
                                foreach (var drDVFieldRow in dvFieldRows) {

                                    // check for necessary sys_table rows, if any
                                    string tableName = drDVFieldRow["table_name"].ToString();
                                    var newTableFieldIDForThisRow = -1;
                                    if (!String.IsNullOrEmpty(tableName)) {
                                        int newTableID = getTableIDByName(dm, tableName);
                                        if (newTableID < 0) {
                                            // doesn't exist yet, need to insert it
                                            DataRow[] tableRows = dsDataViewDefinitions.Tables["sys_table"].Select("table_name = '" + tableName + "'");
                                            if (tableRows != null && tableRows.Length > 0) {

                                                // 2009-09-15 Brock Weaver brock@circaware.com
                                                // changing to just queue up informational detail instead of auto-creating the record...
                                                //// this sys_table record isn't mapped yet.
                                                //DataSet dsTable = dsDataViewDefinitions.Clone();
                                                //appendRow(dsTable, tableRows[0], "sys_table_id", newTableID);
                                                //saveData(dsTable, false, dsReturn);
                                                //newTableID = getTableIDByName(dm, tableName);

                                                missingInfo.AppendLine(getDisplayMember("ImportDataViewDefinition{sys_table2}", "sys_table record for {0} required by dataview {1} is missing in the target database.", tableName , dvName));
                                                missingInfoIsShowstopper = !ignoreMissingFieldMappings;
                                            }
                                        }


                                        // check for necessary sys_table_field rows, if any
                                        string tableFieldName = drDVFieldRow["table_field_name"].ToString();
                                        if (!String.IsNullOrEmpty(tableFieldName)) {

                                            newTableFieldIDForThisRow = getTableFieldIDByName(dm, tableName, tableFieldName);

                                            if (newTableFieldIDForThisRow < 0) {

                                                missingInfo.AppendLine(getDisplayMember("ImportDataViewDefinition{sys_table_field2}", "sys_table_field record for '{0}' required by dataview field '{1}.{2}' is missing in the target database.\nPlease use the GRIN-Global Admin Tool to create or edit the Table Mappings for '{3}' as needed to fix this problem.", tableFieldName, dvName, drDVFieldRow["field_name"].ToString(), tableName));
                                                missingInfoIsShowstopper = !ignoreMissingFieldMappings;

                                            } else {
                                                // if they specified to add language information to table mappings, we will do that here...
                                                if (useInTableMappings) {

                                                    DataRow[] dvFieldLangRows = dsDataViewDefinitions.Tables["sys_dataview_field_lang"].Select("dataview_name = '" + dvName + "' and field_name = '" + drDVFieldRow["field_name"].ToString().Replace("'", "''") + "'");
                                                    if (dvFieldLangRows != null) {
                                                        foreach (DataRow drDVFieldLangRow in dvFieldLangRows) {

                                                            int newSecLangID = 0;

                                                            // ok, we need to add any missing sys_lang records
                                                            string isoCode = drDVFieldLangRow["iso_639_3_tag"].ToString();
                                                            DataRow[] langRows = dsDataViewDefinitions.Tables["sys_lang"].Select("iso_639_3_tag = '" + isoCode + "'");
                                                            if (langRows != null && langRows.Length > 0) {
                                                                foreach (var drLangRow in langRows) {
                                                                    newSecLangID = getLangIDByIsoCode(dm, isoCode);
                                                                    if (newSecLangID > -1) {
                                                                        var newTableFieldLangID = getTableFieldLangIDByFieldID(dm, newTableFieldIDForThisRow, newSecLangID);
                                                                        if (newTableFieldLangID < 0) {
                                                                            newTableFieldLangID = dm.Write(@"
insert into sys_table_field_lang
(sys_table_field_id, sys_lang_id, title, description, created_date, created_by, owned_date, owned_by)
values
(:stfid, :langid, :title, :description, :now1, :who1, :now2, :who2)
", true, "sys_table_field_lang_id", new DataParameters(
                             ":stfid", newTableFieldIDForThisRow, DbType.Int32,
                             ":langid", newSecLangID, DbType.Int32,
                             ":title", getRowValue(drDVFieldLangRow, "title", ""),
                             ":description", getRowValue(drDVFieldLangRow, "description", ""),
                             ":now1", DateTime.UtcNow, DbType.DateTime2,
                             ":who1", CooperatorID, DbType.Int32,
                             ":now2", DateTime.UtcNow, DbType.DateTime2,
                             ":who2", CooperatorID, DbType.Int32
                             ));
                                                                        } else {
                                                                            dm.Write(@"
update
    sys_table_field_lang
set
    sys_table_field_id = :stfid,
    sys_lang_id = :langid,
    title = :title,
    description = :description,
    modified_date = :now1,
    modified_by = :who1
where
    sys_table_field_lang_id = :id
", new DataParameters(
                             ":stfid", newTableFieldIDForThisRow, DbType.Int32,
                             ":langid", newSecLangID, DbType.Int32,
                             ":title", getRowValue(drDVFieldLangRow, "title", ""),
                             ":description", getRowValue(drDVFieldLangRow, "description", ""),
                             ":now1", DateTime.UtcNow, DbType.DateTime2,
                             ":who1", CooperatorID, DbType.Int32,
                             ":id", newTableFieldLangID, DbType.Int32
                             ));
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }


                                    if (missingInfoIsShowstopper) {

                                        // a missing record prevents us from properly importing this dataview.
                                        // must be sys_table or sys_table_field.
                                        // bomb out, force a roll back of all dataviews, etc.
                                        throw Library.CreateBusinessException(missingInfo.ToString());

                                    } else {
                                        // we get here, all sys_table and sys_table_field records exist as needed.
                                        // we can now insert/update the sys_dataview_field record itself
                                        string dvFieldName = drDVFieldRow["field_name"].ToString();

                                        int newDVFieldID = getDataViewFieldIDByName(dm, newDVID, dvFieldName);

                                        // copy new id values into fk fields
                                        drDVFieldRow["sys_dataview_id"] = newDVID;
                                        if (newTableFieldIDForThisRow > 0) {
                                            drDVFieldRow["sys_table_field_id"] = newTableFieldIDForThisRow;

                                            // set gui_hint appropriately.
                                            // NOTE: older dataview exports won't have gui_hint in the sys_dataview_field column,
                                            //       so we skip this processing if needed.  Also, if gui_hint is same as sys_table_field's
                                            //       gui_hint, write a null so sys_table_field.gui_hint is used during lookups of the dataview definition.
                                            if (drDVFieldRow.Table.Columns.Contains("gui_hint")) {
                                                string guiHint = drDVFieldRow["gui_hint"].ToString();
                                                if (String.IsNullOrEmpty(guiHint) || guiHint.ToUpper() == "(NONE)") {
                                                    // mark as null so sys_table_field.gui_hint value is used
                                                    drDVFieldRow["gui_hint"] = DBNull.Value;
                                                } else {
                                                    var tableFieldGuiHint = dm.ReadValue(@"
select
    gui_hint
from
    sys_table_field
where
    sys_table_field_id = :id
", new DataParameters(":id", newTableFieldIDForThisRow)) as string;
                                                    if (!String.IsNullOrEmpty(tableFieldGuiHint)) {
                                                        if (String.Compare(guiHint, tableFieldGuiHint, true) == 0) {
                                                            drDVFieldRow["gui_hint"] = DBNull.Value;
                                                        }
                                                    }
                                                }
                                            }

                                        }



                                        if (includeFieldsAndParameters) {
                                            if (newDVFieldID < 0) {
                                                newDVFieldID = dm.Write(@"
insert into sys_dataview_field
(sys_dataview_id, field_name, sys_table_field_id, is_readonly, is_primary_key, is_transform, sort_order, gui_hint, foreign_key_dataview_name, group_name, table_alias_name, is_visible, configuration_options, created_date, created_by, owned_date, owned_by)
values
(:dvid, :name, :stfid, :isreadonly, :isprimarykey, :istransform, :sortorder, :guihint, :fkdv, :groupname, :aliasname, :isvisible, :configoptions, :now1, :who1, :now2, :who2)
", true, "sys_dataview_field_id", new DataParameters(
         ":dvid", newDVID, DbType.Int32,
         ":name", getRowValue(drDVFieldRow, "field_name", ""),
         ":stfid", (newTableFieldIDForThisRow > 0 ? (int?)newTableFieldIDForThisRow : null), DbType.Int32,
         ":isreadonly", getRowToggleValue(drDVFieldRow, "is_readonly", true),
         ":isprimarykey", getRowToggleValue(drDVFieldRow, "is_primary_key", false),
         ":istransform", getRowToggleValue(drDVFieldRow, "is_transform", false),
         ":sortorder", getRowValue(drDVFieldRow, "sort_order", 0), DbType.Int32,
         ":guihint", getRowValue(drDVFieldRow, "gui_hint", ""),
         ":fkdv", getRowValue(drDVFieldRow, "foreign_key_dataview_name", ""),
         ":groupname", getRowValue(drDVFieldRow, "group_name", ""),
         ":aliasname", getRowValue(drDVFieldRow, "table_alias_name", ""),
         ":isvisible", getRowToggleValue(drDVFieldRow, "is_visible", true),
         ":configoptions", getRowValue(drDVFieldRow, "configuration_options", ""),
         ":now1", DateTime.UtcNow, DbType.DateTime2,
         ":who1", CooperatorID, DbType.Int32,
         ":now2", DateTime.UtcNow, DbType.DateTime2,
         ":who2", CooperatorID, DbType.Int32
         ));
                                            } else {

                                                dm.Write(@"
update
    sys_dataview_field
set
    sys_dataview_id = :dvid,
    field_name = :name,
    sys_table_field_id = :stfid,
    is_readonly = :isreadonly,
    is_primary_key = :isprimarykey,
    is_transform = :istransform,
    sort_order = :sortorder,
    gui_hint = :guihint,
    foreign_key_dataview_name = :fkdv,
    group_name = :groupname,
    table_alias_name = :aliasname,
    is_visible = :isvisible,
    configuration_options = :configoptions,
    modified_date = :now1,
    modified_by = :who1
where
    sys_dataview_field_id = :dvfid
", new DataParameters(
         ":dvid", newDVID, DbType.Int32,
         ":name", getRowValue(drDVFieldRow, "field_name", ""),
         ":stfid", (newTableFieldIDForThisRow > 0 ? (int?)newTableFieldIDForThisRow : null), DbType.Int32,
         ":isreadonly", getRowToggleValue(drDVFieldRow, "is_readonly", true),
         ":isprimarykey", getRowToggleValue(drDVFieldRow, "is_primary_key", false),
         ":istransform", getRowToggleValue(drDVFieldRow, "is_transform", false),
         ":sortorder", getRowValue(drDVFieldRow, "sort_order", 0), DbType.Int32,
         ":guihint", getRowValue(drDVFieldRow, "gui_hint", ""),
         ":fkdv", getRowValue(drDVFieldRow, "foreign_key_dataview_name", ""),
         ":groupname", getRowValue(drDVFieldRow, "group_name", ""),
         ":aliasname", getRowValue(drDVFieldRow, "table_alias_name", ""),
         ":isvisible", getRowToggleValue(drDVFieldRow, "is_visible", true),
         ":configoptions", getRowValue(drDVFieldRow, "configuration_options", ""),
         ":now1", DateTime.UtcNow, DbType.DateTime2,
         ":who1", CooperatorID, DbType.Int32,
         ":dvfid", newDVFieldID, DbType.Int32));


                                            }
                                        }

                                        validDVFieldIDs.Add(newDVFieldID);



                                        // finally, we have everything in place.  we can now update/insert sys_dataview_field_lang records
                                        DataRow[] dvFieldLangRows = dsDataViewDefinitions.Tables["sys_dataview_field_lang"].Select("dataview_name = '" + dvName + "' and field_name = '" + drDVFieldRow["field_name"].ToString().Replace("'", "''") + "'");
                                        if (dvFieldLangRows != null) {
                                            foreach (DataRow drDVFieldLangRow in dvFieldLangRows) {

                                                int newSecLangID = 0;

                                                // ok, we need to add any missing sys_lang records
                                                string isoCode = drDVFieldLangRow["iso_639_3_tag"].ToString();
                                                DataRow[] langRows = dsDataViewDefinitions.Tables["sys_lang"].Select("iso_639_3_tag = '" + isoCode + "'");
                                                if (langRows != null && langRows.Length > 0) {
                                                    foreach (var drLangRow in langRows) {
                                                        newSecLangID = getLangIDByIsoCode(dm, isoCode);
                                                        if (newSecLangID < 0) {

                                                            // 2009-09-15 Brock Weaver brock@circaware.com
                                                            // changing to just queue up informational detail instead of auto-creating the record...
                                                            //// insert sys_lang as needed (we do this here so we have the field_id in context from which
                                                            //DataSet dsSecLang = dsDataViewDefinitions.Clone();
                                                            //appendRow(dsSecLang, drLangRow, "sys_lang_id", newSecLangID);
                                                            //dsSecLang = saveData(dsSecLang, false, dsReturn);
                                                            //if (newSecLangID < 0) {
                                                            //    newSecLangID = getLangIDByIsoCode(dm, isoCode);
                                                            //}

                                                            missingInfo.AppendLine("sys_lang record for " + isoCode + " required by dataview field " + dvName + "." + drDVFieldRow["field_name"].ToString() + " is missing in the target database.");

                                                        }
                                                    }
                                                }


                                                string dvFieldLangIsoCode = drDVFieldLangRow["iso_639_3_tag"].ToString();
                                                newSecLangID = getLangIDByIsoCode(dm, dvFieldLangIsoCode);

                                                if (newSecLangID > 0) {
                                                    int newDVFieldLangID = getDataViewFieldLangIDByFieldID(dm, newDVFieldID, newSecLangID);


                                                    // HACK: we only write dataview field lang records 
                                                    //       if the corresponding table field lang record doesn't match
                                                    var tableFieldLangTitle = dm.ReadValue(@"
select
    title
from
    sys_table_field_lang
where
    sys_table_field_id = :fieldid
    and sys_lang_id = :langid
", new DataParameters(":fieldid", newTableFieldIDForThisRow, DbType.Int32, ":langid", newSecLangID, DbType.Int32)) as string;


                                                    var writeDataviewFieldLang = false;
                                                    var dvLangTitle = getRowValue(drDVFieldLangRow, "title", null) as string;
                                                    if (!String.IsNullOrEmpty(dvLangTitle) && tableFieldLangTitle != dvLangTitle) {
                                                        writeDataviewFieldLang = true;
                                                    }

                                                    if (writeDataviewFieldLang) {

                                                        if (newDVFieldLangID < 0) {
                                                            if (includeLanguageData) {
                                                                newDVFieldLangID = dm.Write(@"
insert into sys_dataview_field_lang
(sys_dataview_field_id, sys_lang_id, title, description, created_date, created_by, owned_date, owned_by)
values
(:sdfid, :langid, :title, :description, :now1, :who1, :now2, :who2)
", true, "sys_dataview_field_lang_id", new DataParameters(
                 ":sdfid", newDVFieldID, DbType.Int32,
                 ":langid", newSecLangID, DbType.Int32,
                 ":title", getRowValue(drDVFieldLangRow, "title", ""),
                 ":description", getRowValue(drDVFieldLangRow, "description", ""),
                 ":now1", DateTime.UtcNow, DbType.DateTime2,
                 ":who1", CooperatorID, DbType.Int32,
                 ":now2", DateTime.UtcNow, DbType.DateTime2,
                 ":who2", CooperatorID, DbType.Int32
                 ));
                                                            }
                                                        } else {
                                                            if (includeLanguageData) {
                                                                dm.Write(@"
update
    sys_dataview_field_lang
set
    sys_dataview_field_id = :sdfid,
    sys_lang_id = :langid,
    title = :title,
    description = :description,
    modified_date = :now1,
    modified_by = :who1
where
    sys_dataview_field_lang_id = :id
", new DataParameters(
                 ":sdfid", newDVFieldID, DbType.Int32,
                 ":langid", newSecLangID, DbType.Int32,
                 ":title", getRowValue(drDVFieldLangRow, "title", ""),
                 ":description", getRowValue(drDVFieldLangRow, "description", ""),
                 ":now1", DateTime.UtcNow, DbType.DateTime2,
                 ":who1", CooperatorID, DbType.Int32,
                 ":id", newDVFieldLangID, DbType.Int32
                 ));
                                                            }
                                                        }
                                                        if (newDVFieldLangID > -1) {
                                                            validDVFieldLangIDs.Add(newDVFieldLangID);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            // remove previously existing ones from sys_dataview_field_lang that we did not touch (i.e. they're no longer valid)
                            // ticket #632, if skipping language data, only delete field_langs which will no longer have corresponding fields (previously deleted all)
                            if (includeLanguageData) {
                                // delete any we did not overwrite
                                dm.Write("delete from sys_dataview_field_lang where sys_dataview_field_id in (select sys_dataview_field_id from sys_dataview_field where sys_dataview_id = :dvid) and sys_dataview_field_lang_id not in (:idlist)", new DataParameters(":dvid", newDVID, DbType.Int32, ":idlist", validDVFieldLangIDs, DbPseudoType.IntegerCollection));
                            } else {
                                // delete any that belong to fields which will no longer exist after this method is done (note we use validDVFieldIDs, NOT validDVFieldLangIDs)
                                dm.Write("delete from sys_dataview_field_lang where sys_dataview_field_id in (select sys_dataview_field_id from sys_dataview_field where sys_dataview_id = :dvid) and sys_dataview_field_id not in (:idlist)", new DataParameters(":dvid", newDVID, DbType.Int32, ":idlist", validDVFieldIDs, DbPseudoType.IntegerCollection));
                            }

                            // remove previously existing ones from sys_dataview_field that we did not touch (i.e. they're no longer valid)
                            if (includeFieldsAndParameters) {
                                dm.Write("delete from sys_dataview_field where sys_dataview_id = :dvid and sys_dataview_field_id not in (:idlist)", new DataParameters(":dvid", newDVID, DbType.Int32, ":idlist", validDVFieldIDs, DbPseudoType.IntegerCollection));
                            }

                            // remove previously existing ones from sys_dataview_param that we did not touch (i.e. they're no longer valid)
                            if (includeFieldsAndParameters) {
                                dm.Write("delete from sys_dataview_param where sys_dataview_id = :dvid and sys_dataview_param_id not in (:idlist)", new DataParameters(":dvid", newDVID, DbType.Int32, ":idlist", validDVParamIDs, DbPseudoType.IntegerCollection));
                            }

                            // remove previously existing ones from sys_dataview_lang that we did not touch (i.e. they're no longer valid)
                            // ticket #632, if skipping language data, ignore sys_dataview_lang table.
                            if (includeLanguageData) {
                                dm.Write("delete from sys_dataview_lang where sys_dataview_id = :dvid and sys_dataview_lang_id not in (:idlist)", new DataParameters(":dvid", newDVID, DbType.Int32, ":idlist", validDVLangIDs, DbPseudoType.IntegerCollection));
                            }

                            // ticket #300, we essentially should never delete sql rows as there is no good way to determine if there was
                            //              no data given or it is old.  Either way, whatever was already there is just as valid as it was before.
                            //              So the sys_dataview_sql is a special case during import as we do not delete records which were not imported.
                            // remove previously existing ones from sys_dataview_sql that we did not touch (i.e. they're no longer valid)
                            // dm.Write("delete from sys_dataview_sql where sys_dataview_id = :dvid and sys_dataview_sql_id not in (:idlist)", new DataParameters(":dvid", newDVID, DbType.Int32, ":idlist", validDVSqlIDs, DbPseudoType.IntegerCollection));

                            dtImported.Rows.Add(new object[] { newDVID, dvName });
                        }
                    }


                    dm.Commit();
                }

                Dataview.ClearCache();

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;

        }

        public virtual DataSet ExportDataViewDefinitions(List<string> dvNames) {

            DataSet dsReturn = createReturnDataSet();

            try {

                checkUserHasAdminEnabled();


                string dataviews = "'" + String.Join("','", dvNames.ToArray()) + "'";

                using (DataManager dm = BeginProcessing(true)) {

                    string secDVSubselect = "select sys_dataview_id from sys_dataview where dataview_name in (" + dataviews + ")";
                    string secDVFieldSubselect = "select sys_dataview_field_id from sys_dataview_field where sys_dataview_id in (" + secDVSubselect + ")";

                    string secTableSubselect = String.Format(@"
select sys_table_id from sys_table_field where sys_table_field_id in (
  select sys_table_field_id from sys_dataview_field where sys_dataview_id in ({0})
)", secDVSubselect);

                    // pull sys_dataview
                    dm.Read(@"select * from sys_dataview where dataview_name in (" + dataviews + ") order by dataview_name", dsReturn, "sys_dataview");

                    // pull sys_datatrigger
                    //                    dm.Read(@"select * from sys_datatrigger where sys_dataview_id in (" + secRSSubselect + ")", dsReturn, "sys_datatrigger");

                    // pull sys_dataview_lang
                    dm.Read(@"
select 
sdvl.sys_dataview_lang_id
      ,sdvl.sys_dataview_id
      ,sdvl.sys_lang_id
      ,sdvl.title
      ,sdvl.description
      ,sdvl.created_date
      ,sdvl.created_by
      ,sdvl.modified_date
      ,sdvl.modified_by
      ,sdvl.owned_date
      ,sdvl.owned_by
      ,sl.iso_639_3_tag
      ,sdv.dataview_name 
from 
    sys_dataview_lang sdvl inner join sys_dataview sdv
        on sdvl.sys_dataview_id = sdv.sys_dataview_id
    inner join sys_lang sl
        on sdvl.sys_lang_id = sl.sys_lang_id
where 
    sdvl.sys_dataview_id in (" + secDVSubselect + ")", dsReturn, "sys_dataview_lang");

                    // pull sys_dataview_sql
                    dm.Read(@"
select 
    sdvs.sys_dataview_sql_id
      ,sdvs.sys_dataview_id
      ,sdvs.database_engine_tag
      ,sdvs.sql_statement
      ,sdvs.created_date
      ,sdvs.created_by
      ,sdvs.modified_date
      ,sdvs.modified_by
      ,sdvs.owned_date
      ,sdvs.owned_by
      ,sdv.dataview_name 
from 
    sys_dataview_sql sdvs inner join sys_dataview sdv
        on sdvs.sys_dataview_id = sdv.sys_dataview_id
where 
    sdvs.sys_dataview_id in (" + secDVSubselect + ")", dsReturn, "sys_dataview_sql");

                    // pull sys_dataview_param
                    dm.Read(@"
select 
sdvp.sys_dataview_param_id
      ,sdvp.sys_dataview_id
      ,sdvp.param_name
      ,sdvp.param_type
      ,sdvp.sort_order
      ,sdvp.created_date
      ,sdvp.created_by
      ,sdvp.modified_date
      ,sdvp.modified_by
      ,sdvp.owned_date
      ,sdvp.owned_by
      ,sdv.dataview_name 
from 
    sys_dataview_param sdvp inner join sys_dataview sdv
        on sdvp.sys_dataview_id = sdv.sys_dataview_id
where 
    sdvp.sys_dataview_id in (" + secDVSubselect + ")", dsReturn, "sys_dataview_param");

                    // pull sys_dataview_field
                    dm.Read(@"
select 
    sdvf.sys_dataview_field_id
      ,sdvf.sys_dataview_id
      ,sdvf.field_name
      ,sdvf.sys_table_field_id
      ,sdvf.is_readonly
      ,sdvf.is_primary_key
      ,sdvf.is_transform
      ,sdvf.sort_order
      ,sdvf.gui_hint
      ,sdvf.foreign_key_dataview_name
      ,sdvf.group_name
      ,sdvf.table_alias_name
      ,sdvf.is_visible
      ,sdvf.configuration_options
      ,sdvf.created_date
      ,sdvf.created_by
      ,sdvf.modified_date
      ,sdvf.modified_by
      ,sdvf.owned_date
      ,sdvf.owned_by
      ,sdv.dataview_name
      ,stf.field_name as table_field_name
      ,st.table_name
from 
    sys_dataview_field sdvf inner join sys_dataview sdv
        on sdvf.sys_dataview_id = sdv.sys_dataview_id
    left join sys_table_field stf
        on sdvf.sys_table_field_id = stf.sys_table_field_id
    left join sys_table st
        on stf.sys_table_id = st.sys_table_id
where 
    sdvf.sys_dataview_id in (" + secDVSubselect + ")", dsReturn, "sys_dataview_field");

                    // pull sys_dataview_field_lang
                    dm.Read(@"
select 
    sdvfl.sys_dataview_field_lang_id
      ,sdvfl.sys_dataview_field_id
      ,sdvfl.sys_lang_id
      ,sdvfl.title
      ,sdvfl.description
      ,sdvfl.created_date
      ,sdvfl.created_by
      ,sdvfl.modified_date
      ,sdvfl.modified_by
      ,sdvfl.owned_date
      ,sdvfl.owned_by
      ,sdv.dataview_name
      ,sdvf.field_name
      ,sl.iso_639_3_tag
from 
    sys_dataview_field_lang sdvfl inner join sys_dataview_field sdvf 
        on sdvfl.sys_dataview_field_id = sdvf.sys_dataview_field_id
    inner join sys_dataview sdv
        on sdvf.sys_dataview_id = sdv.sys_dataview_id
    left join sys_lang sl
        on sdvfl.sys_lang_id = sl.sys_lang_id
where 
    sdvfl.sys_dataview_field_id in (" + secDVFieldSubselect + ")", dsReturn, "sys_dataview_field_lang");

                    // pull sys_lang -- only those that are represented in sys_dataview_field_lang though.
                    dm.Read(@"
select 
    sys_lang_id
      ,iso_639_3_tag
      ,ietf_tag
      ,title
      ,description
      ,created_date
      ,created_by
      ,modified_date
      ,modified_by
      ,owned_date
      ,owned_by
from 
    sys_lang 
where 
    sys_lang_id in (select sys_lang_id from sys_dataview_field_lang where sys_dataview_field_id in (" + secDVFieldSubselect + "))", dsReturn, "sys_lang");

                    // pull sys_table_field -- resolve foreign_key_table_field_id to a name if necessary
                    dm.Read(@"
select 
stf.sys_table_field_id
      ,stf.sys_table_id
      ,stf.field_name
      ,stf.field_purpose
      ,stf.field_type
      ,stf.default_value
      ,stf.is_primary_key
      ,stf.is_foreign_key
      ,stf.foreign_key_table_field_id
      ,stf.foreign_key_dataview_name
      ,stf.is_nullable
      ,stf.gui_hint
      ,stf.is_readonly
      ,stf.min_length
      ,stf.max_length
      ,stf.numeric_precision
      ,stf.numeric_scale
      ,stf.is_autoincrement
      ,stf.group_name
      ,stf.created_date
      ,stf.created_by
      ,stf.modified_date
      ,stf.modified_by
      ,stf.owned_date
      ,stf.owned_by
      ,st2.table_name as foreign_key_table_name
      ,stf2.field_name as foreign_key_table_field_name
      ,st.table_name
from 
    sys_table_field stf inner join sys_table st
        on stf.sys_table_id = st.sys_table_id
    left join sys_table_field stf2
        on stf.foreign_key_table_field_id = stf2.sys_table_field_id
    left join sys_table st2
        on stf2.sys_table_id = st2.sys_table_id
where 
    stf.sys_table_id in (" + secTableSubselect + ")"
                       , dsReturn, "sys_table_field");



                    // pull sys_table
                    dm.Read(@"
select 
    st.sys_table_id
      ,st.table_name
      ,st.is_enabled
      ,st.is_readonly
      ,st.audits_created
      ,st.audits_modified
      ,st.audits_owned
      ,st.database_area_code
      ,st.created_date
      ,st.created_by
      ,st.modified_date
      ,st.modified_by
      ,st.owned_date
      ,st.owned_by
from 
    sys_table st
where 
    st.sys_table_id in (" + secTableSubselect + ")", dsReturn, "sys_table");



                    dsReturn.AcceptChanges();
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;

        }

        public virtual DataSet RenameDataViewDefinition(string dataviewName, string newDataViewName) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {

                    checkUserHasAdminEnabled();

                    int affected = dm.Write(@"
update
    sys_dataview
set
    dataview_name = :newdataview,
    modified_by = :modifiedby,
    modified_date = :now
where
    dataview_name = :olddataview
", new DataParameters(
     ":newdataview", newDataViewName,
     ":modifiedby", this.CooperatorID, DbType.Int32,
     ":now", DateTime.UtcNow, DbType.DateTime2,
     ":olddataview", dataviewName));

                    if (affected == 0) {
                        throw Library.CreateBusinessException(getDisplayMember("RenameDataViewDefinition{nodataviews}", "No dataviews found named '{0}')", dataviewName));
                    }

                    // one or more dataview fields may be pointing at this as well.  update those too.
                    dm.Write(@"
update
    sys_dataview_field
set
    foreign_key_dataview_name = :newdataview,
    modified_by = :modifiedby,
    modified_date = :now
where
    foreign_key_dataview_name = :olddataview
", new DataParameters(
     ":newdataview", newDataViewName,
     ":modifiedby", this.CooperatorID, DbType.Int32,
     ":now", DateTime.UtcNow, DbType.DateTime2,
     ":olddataview", dataviewName));

                    // one or more table fields may be pointing at it too.

                    dm.Write(@"
update
    sys_table_field
set
    foreign_key_dataview_name = :newdataview,
    modified_by = :modifiedby,
    modified_date = :now
where
    foreign_key_dataview_name = :olddataview
", new DataParameters(
":newdataview", newDataViewName,
":modifiedby", this.CooperatorID, DbType.Int32,
":now", DateTime.UtcNow, DbType.DateTime2,
":olddataview", dataviewName));

                    // clear caches for everything, just to make sure we don't serve up old defintions any more
                    Dataview.ClearCache();
                    Table.ClearCache();
                }


            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        private void renameDataViewInDataTable(DataTable dt, string newName) {
            if (dt != null) {
                if (dt.Columns.Contains("dataview_name")) {
                    dt.Columns["dataview_name"].ReadOnly = false;
                    foreach (DataRow dr in dt.Rows) {
                        dr["dataview_name"] = newName;
                    }
                    dt.AcceptChanges();
                }
            }
        }

        public virtual DataSet CopyDataViewDefinition(string dataviewName, string newDataViewName) {

            DataSet dsReturn = createReturnDataSet();

            try {

                // lazy man way = export dataviewName, tweak with newDataViewName, import

                checkUserHasAdminEnabled();

                DataSet dsExport = ExportDataViewDefinitions(new string[] { dataviewName }.ToList());

                using (DataManager dm = BeginProcessing(true)) {


                    renameDataViewInDataTable(dsExport.Tables["sys_dataview"], newDataViewName);
                    renameDataViewInDataTable(dsExport.Tables["sys_dataview_param"], newDataViewName);
                    renameDataViewInDataTable(dsExport.Tables["sys_dataview_sql"], newDataViewName);
                    renameDataViewInDataTable(dsExport.Tables["sys_dataview_lang"], newDataViewName);
                    renameDataViewInDataTable(dsExport.Tables["sys_dataview_field"], newDataViewName);
                    renameDataViewInDataTable(dsExport.Tables["sys_dataview_field_lang"], newDataViewName);

                    dsReturn = ImportDataViewDefinitions(dsExport, new string[] { newDataViewName }.ToList(), false, true, true, false);

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        //        public virtual DataSet CreateDataViewDefinition(string dvName, bool readOnly, bool suppressProps, bool isSystem, bool userVisible, bool webVisible, string formAssemblyName, string formFullyQualifiedName, string executableName, string categoryName, string databaseArea, int databaseAreaSortOrder, bool transform, string transformFieldForNames, string transformFieldForCaptions, string transformFieldForValues, DataSet dsInfo) {
        public virtual DataSet CreateDataViewDefinition(string dvName, bool readOnly, string categoryCode, string databaseAreaCode, int databaseAreaSortOrder, bool transform, string transformFieldForNames, string transformFieldForCaptions, string transformFieldForValues, string configurationOptions, DataSet dsInfo) {

            DataSet dsReturn = createReturnDataSet();
            try {

                checkUserHasAdminEnabled();

                //formAssemblyName = (String.IsNullOrEmpty(formAssemblyName) ? null : formAssemblyName);
                //formFullyQualifiedName = (String.IsNullOrEmpty(formFullyQualifiedName) ? null : formFullyQualifiedName);
                //executableName = (String.IsNullOrEmpty(executableName) ? null : executableName);
                databaseAreaCode = (String.IsNullOrEmpty(databaseAreaCode) ? null : databaseAreaCode);
                categoryCode = (String.IsNullOrEmpty(categoryCode) ? null : categoryCode);

                var paramIds = new List<int>();
                var fieldIds = new List<int>();
                var friendlyFieldIds = new List<int>();
                var langIds = new List<int>();
                var sqlIds = new List<int>();

                using (DataManager dm = BeginProcessing(true)) {

                    dm.BeginTran();
                    dvName = dvName.ToLower();


                    DataTable dtParam = dsInfo.Tables["sys_dataview_param"];
                    DataTable dtFieldInfo = dsInfo.Tables["dv_field_info"];
                    DataTable dtDVLangInfo = dsInfo.Tables["sys_dataview_lang"];
                    DataTable dtDVSqlInfo = dsInfo.Tables["sys_dataview_sql"];


                    // if it's a system dataview, don't let them touch it.
                    //bool isSystem = Toolkit.ToBoolean(dm.ReadValue("select is_system from sys_dataview where dataview_name = :dataview", new DataParameters(":dataview", dvName)).ToString().ToUpper() == "Y", false);
                    //if (isSystem) {
                    //    throw Library.CreateBusinessException("You can not update or delete a dataview that is required by the system.");
                    //}

                    //DataTable dt = sd.DataManager.Read(@"select * from sys_dataview where dataview_name = :name", new DataParameters(":name", dvName));
                    //if (dt.Rows.Count > 0) {
                    //    // delete the entire existing mapping(s)
                    //    DeleteCustomView(dvName);
                    //}

                    // create a new dataview record
                    int dvId = Toolkit.ToInt32(dm.ReadValue(@"
select
	sys_dataview_id
from
	sys_dataview
where
	dataview_name = :dv
", new DataParameters(":dv", dvName)), -1);


                    // new rs. create it. make it read only if they tell us to or there are no fields defined whatsoever.
                    var readOnlyDataview = readOnly || dtFieldInfo == null || dtFieldInfo.Rows.Count == 0;

                    if (dvId < 0) {


                        //                        insert into 
                        //    sys_dataview
                        //(dataview_name, is_enabled, is_readonly, is_system, is_property_suppressed, is_user_visible, is_web_visible, form_assembly_name, form_fully_qualified_name, executable_name, category_name, database_area, database_area_code_sort_order, is_transform, transform_field_for_names, transform_field_for_captions, transform_field_for_values, created_date, created_by, owned_date, owned_by)
                        //values
                        //(:name, :isenabled, :isreadonly, :issystem, :ispropertysuppressed, :isuservisible, :iswebvisible, :formassemblyname, :formfullyqualifiedname, :executablename, :categoryname, :dbarea, :dbareasort, :istransform, :transformfieldfornames, :transformfieldforcaptions, :transformfieldsforvalues, :createddate, :createdby, :owneddate, :ownedby)

                        dvId = dm.Write(@"
insert into 
	sys_dataview
(dataview_name, is_enabled, is_readonly, category_code, database_area_code, database_area_code_sort_order, is_transform, transform_field_for_names, transform_field_for_captions, transform_field_for_values, configuration_options, created_date, created_by, owned_date, owned_by)
values
(:name, :isenabled, :isreadonly, :categoryname, :dbarea, :dbareasort, :istransform, :transformfieldfornames, :transformfieldforcaptions, :transformfieldsforvalues, :configoptions, :createddate, :createdby, :owneddate, :ownedby)
", true, "sys_dataview_id", new DataParameters(
                                 ":name", dvName,
                                 ":isenabled", "Y",
                                 ":isreadonly", (readOnlyDataview ? "Y" : "N"),
                            //":issystem", (isSystem ? "Y" : "N"),
                            //":ispropertysuppressed", suppressProps ? "Y" : "N",
                            //":isuservisible", userVisible ? "Y" : "N",
                            //":iswebvisible", webVisible ? "Y" : "N",
                            //":formassemblyname", formAssemblyName,
                            //":formfullyqualifiedname", formFullyQualifiedName,
                            //":executablename", executableName,
                                 ":categoryname", categoryCode,
                                 ":dbarea", databaseAreaCode,
                                 ":dbareasort", databaseAreaSortOrder,
                                 ":istransform", transform ? "Y" : "N",
                                 ":transformfieldfornames", transformFieldForNames,
                                 ":transformfieldforcaptions", transformFieldForCaptions,
                                 ":transformfieldsforvalues", transformFieldForValues,
                                 ":configoptions", configurationOptions,
                                 ":createddate", DateTime.UtcNow, DbType.DateTime2,
                                 ":createdby", CooperatorID, DbType.Int32,
                                 ":owneddate", DateTime.UtcNow, DbType.DateTime2,
                                 ":ownedby", CooperatorID, DbType.Int32
                                 ));
                    } else {



                        // existing sys_dataview. update it.

                        dm.Write(@"
update  
	sys_dataview
set 
	is_enabled = :isenabled,
	is_readonly = :isreadonly,
    category_code = :categoryname,
    database_area_code = :dbarea,
    database_area_code_sort_order = :dbareasort,
    is_transform = :istransform,
    transform_field_for_names = :transformfieldfornames,
    transform_field_for_captions = :transformfieldforcaptions,
    transform_field_for_values = :transformfieldforvalues,
    configuration_options = :configoptions,
	modified_date = :modifieddate,
	modified_by = :modifiedby
where
	sys_dataview_id = :id
", new DataParameters(
         ":isenabled", "Y",
         ":isreadonly", (readOnlyDataview ? "Y" : "N"),
                            //":issystem", (isSystem ? "Y" : "N"),
                            //":ispropertysuppressed", suppressProps ? "Y" : "N",
                            //":isuservisible", userVisible ? "Y" : "N",
                            //":iswebvisible", webVisible ? "Y" : "N",
                            //":formassemblyname", formAssemblyName,
                            //":formfullyqualifiedname", formFullyQualifiedName,
                            //":executablename", executableName,
         ":categoryname", categoryCode,
         ":dbarea", databaseAreaCode,
         ":dbareasort", databaseAreaSortOrder,
         ":istransform", transform ? "Y" : "N",
         ":transformfieldfornames", transformFieldForNames,
         ":transformfieldforcaptions", transformFieldForCaptions,
         ":transformfieldforvalues", transformFieldForValues,
         ":configoptions", configurationOptions,
         ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
         ":modifiedby", CooperatorID, DbType.Int32,
         ":id", dvId, DbType.Int32));

                    }

                    foreach (DataRow row in dtDVLangInfo.Rows) {

                        int dvLangID = Toolkit.ToInt32(row["sys_dataview_lang_id"], -1);
                        int langID = Toolkit.ToInt32(row["sys_lang_id"], -1);
                        string title = row["title"].ToString();
                        string desc = row["description"].ToString();
                        var createddate = Toolkit.ToDateTime(row["created_date"], DateTime.UtcNow);
                        var createdby = Toolkit.ToDateTime(row["created_by"], DateTime.UtcNow);
                        if (dvLangID < 0) {
                            // insert a new dataview lang value
                            dvLangID = dm.Write(@"
insert into sys_dataview_lang
(sys_dataview_id, sys_lang_id, title, description, created_date, created_by, owned_date, owned_by)
values
(:rsid, :langid, :title, :description, :createddate, :createdby, :owneddate, :ownedby)
", true, "sys_dataview_lang_id", new DataParameters(
     ":rsid", dvId, DbType.Int32,
     ":langid", langID, DbType.Int32,
     ":title", title,
     ":description", desc,
     ":createddate", DateTime.UtcNow, DbType.DateTime2,
     ":createdby", CooperatorID, DbType.Int32,
     ":owneddate", DateTime.UtcNow, DbType.DateTime2,
     ":ownedby", CooperatorID, DbType.Int32));

                        } else {
                            // update existing dataview lang value

                            dm.Write(@"
update 
    sys_dataview_lang
set
    sys_dataview_id = :rsid,
    sys_lang_id = :langid,
    title = :title,
    description = :description,
    modified_date = :modifieddate,
    modified_by = :modifiedby
where
    sys_dataview_lang_id = :rslangid
", new DataParameters(
     ":rsid", dvId, DbType.Int32,
     ":langid", langID, DbType.Int32,
     ":title", title,
     ":description", desc,
     ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
     ":modifiedby", CooperatorID, DbType.Int32,
     ":rslangid", dvLangID, DbType.Int32));

                        }

                        langIds.Add(dvLangID);
                    }


                    foreach (DataRow rowSql in dtDVSqlInfo.Rows) {

                        int dvSqlID = Toolkit.ToInt32(rowSql["sys_dataview_sql_id"], -1);
                        string dbEngineCode = rowSql["database_engine_tag"].ToString();
                        string sql = rowSql["sql_statement"].ToString();
                        if (dvSqlID < 0) {
                            // insert a new dataview lang value
                            dvSqlID = dm.Write(@"
insert into sys_dataview_sql
(sys_dataview_id, database_engine_tag, sql_statement, created_date, created_by, owned_date, owned_by)
values
(:dataviewid, :enginecode, :sqlstatement, :createddate, :createdby, :owneddate, :ownedby)
", true, "sys_dataview_sql_id", new DataParameters(
     ":dataviewid", dvId, DbType.Int32,
     ":enginecode", dbEngineCode,
     ":sqlstatement", sql,
     ":createddate", DateTime.UtcNow, DbType.DateTime2,
     ":createdby", CooperatorID, DbType.Int32,
     ":owneddate", DateTime.UtcNow, DbType.DateTime2,
     ":ownedby", CooperatorID, DbType.Int32));

                        } else {
                            // update existing dataview lang value

                            dm.Write(@"
update 
    sys_dataview_sql
set
    sys_dataview_id = :dvid,
    database_engine_tag = :enginecode,
    sql_statement = :sqlstatement,
    modified_date = :modifieddate,
    modified_by = :modifiedby
where
    sys_dataview_sql_id = :dataviewsqlid
", new DataParameters(
     ":dvid", dvId, DbType.Int32,
     ":enginecode", dbEngineCode,
     ":sqlstatement", sql,
     ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
     ":modifiedby", CooperatorID, DbType.Int32,
     ":dataviewsqlid", dvSqlID, DbType.Int32));

                        }
                        rowSql["sys_dataview_sql_id"] = dvSqlID;
                        sqlIds.Add(dvSqlID);
                    }













                    dm.Write(@"
delete from
	sys_dataview_param
where
	sys_dataview_id = :rsid
", new DataParameters(":rsid", dvId, DbType.Int32));


                    // create the param record(s)
                    for (int i = 0; i < dtParam.Rows.Count; i++) {
                        DataRow drParam = dtParam.Rows[i];

                        string pname = drParam["param_name"].ToString();
                        string ptype = drParam["param_type"].ToString().ToUpper();


                        int dvParamId = dm.Write(@"
insert into
	sys_dataview_param
(sys_dataview_id, param_name, param_type, sort_order, created_date, created_by, owned_date, owned_by)
values
(:rsid, :paramname, :paramtype, :sortorder, :createddate, :createdby, :owneddate, :ownedby)
", true, "sys_dataview_param_id", new DataParameters(
            ":rsid", dvId, DbType.Int32,
            ":paramname", pname,
            ":paramtype", ptype,
            ":sortorder", i, DbType.Int32,
            ":createddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
            ":createdby", CooperatorID, DbType.Int32,
            ":owneddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
            ":ownedby", CooperatorID, DbType.Int32));

                    }

                    // create mappings as needed
                    if (dtFieldInfo != null && dtFieldInfo.Rows.Count > 0) {

                        // get list of all languages for each field
                        List<string> languages = new List<string>();
                        int length = "friendly_name_for_".Length;
                        foreach (DataColumn dc in dtFieldInfo.Columns) {
                            if (dc.ColumnName.ToLower().StartsWith("friendly_name_for_")) {
                                languages.Add(dc.ColumnName.Substring(length));
                            }
                        }



                        foreach (DataRow drFieldInfo in dtFieldInfo.Rows) {

                            int dvFieldId = Toolkit.ToInt32(drFieldInfo["sys_dataview_field_id"], -1);
                            int sortOrder = Toolkit.ToInt32(drFieldInfo["dv_field_sort_order"], -1);
                            string dvFieldName = drFieldInfo["dv_field_name"].ToString().ToLower();
                            string rsFieldReadOnly = drFieldInfo["dv_field_readonly"].ToString() == "Y" ? "Y" : "N";
                            string rsFieldPrimaryKey = drFieldInfo["dv_field_primary_key"].ToString() == "Y" ? "Y" : "N";
                            string rsFieldTransform = drFieldInfo["dv_field_transform"].ToString() == "Y" ? "Y" : "N";
                            string rsForeignKeyDataViewName = drFieldInfo["foreign_key_dataview_name"].ToString().ToLower();

                            // for backwards compatibility, we ignore the gui_hint field if need be
                            string guiHint = null;
                            if (dtFieldInfo.Columns.Contains("gui_hint")) {
                                guiHint = drFieldInfo["gui_hint"].ToString().ToUpper();
                                if (String.IsNullOrEmpty(guiHint)) {
                                    guiHint = null;
                                }
                            }

                            // for backwards compatibility, we ignore the table_alias_name field if need be
                            string aliasName = null;
                            if (dtFieldInfo.Columns.Contains("table_alias_name")) {
                                aliasName = drFieldInfo["table_alias_name"].ToString();
                                if (String.IsNullOrEmpty(aliasName)) {
                                    aliasName = null;
                                }
                            }

                            if (String.IsNullOrEmpty(rsForeignKeyDataViewName)) {
                                rsForeignKeyDataViewName = null;
                            }

                            string isVisible = "Y";
                            if (dtFieldInfo.Columns.Contains("dv_is_visible")) {
                                isVisible = drFieldInfo["dv_is_visible"].ToString().ToUpper();
                                if (String.IsNullOrEmpty(isVisible)) {
                                    isVisible = "Y";
                                }
                            }

                            string options = "";
                            if (dtFieldInfo.Columns.Contains("dv_options")) {
                                options = drFieldInfo["dv_options"].ToString();
                                if (String.IsNullOrEmpty(options)) {
                                    options = null;
                                }
                            }

                            string rsFieldGroupName = drFieldInfo["dv_group_name"].ToString();

                            string tblName = drFieldInfo["table_name"].ToString().ToLower();
                            string tblFieldName = drFieldInfo["table_field_name"].ToString().ToLower();

                            // create list to look up language/friendly name from
                            List<KeyValuePair<string, string>> friendlyNames = new List<KeyValuePair<string, string>>();
                            foreach (string lang in languages) {
                                string fname = drFieldInfo["friendly_name_for_" + lang].ToString();
                                if (!String.IsNullOrEmpty(fname)) {
                                    friendlyNames.Add(new KeyValuePair<string, string>(lang, fname));
                                }
                            }

                            // request to delete this mapping (and its friendly names, obviously)
                            if (dvFieldId > 0 && String.IsNullOrEmpty(dvFieldName)) {
                                dm.Write(@"
delete from
	sys_dataview_field_lang
where
	sys_dataview_field_id = :rsfieldid
", new DataParameters(":rsfieldid", dvFieldId, DbType.Int32));
                                dm.Write(@"
delete from
	sys_dataview_field
where
	sys_dataview_field_id = :rsfieldid
", new DataParameters(":rsfieldid", dvFieldId, DbType.Int32));
                            } else {

                                int? tblFieldId = Toolkit.ToInt32(dm.ReadValue(@"
select
	stf.sys_table_field_id
from
	sys_table st
		inner join sys_table_field stf
		on st.sys_table_id = stf.sys_table_id
where
	st.table_name = :tablename 
	and stf.field_name = :tablefieldname", new DataParameters(":tablename", tblName, ":tablefieldname", tblFieldName)), (int?)null);

                                //if (tblFieldId < 0) {
                                //    Library.ThrowBusinessException("Could not find table field '" + tblName + "." + tblFieldName + "' that is supposed to correspond to dataview field '" + rsFieldName + "'. Is spelling and capitalization correct?");
                                //}


                                dvFieldId = Toolkit.ToInt32(dm.ReadValue(@"
select
	sys_dataview_field_id
from
	sys_dataview_field
where
	sys_dataview_id = :rsid
	and field_name = :fname
", new DataParameters(":rsid", dvId, DbType.Int32, ":fname", dvFieldName)), -1);


                                // now that we know the sys_table_field_id, look up its gui_hint.
                                // if it matches the dataview-specific one, write a null value
                                // so changes to sys_table_field.gui_hint are properly reflected in 
                                // sys_dataview_field.gui_hint (i.e. we should write it only if it is different)
                                if (guiHint != null) {
                                    var tableFieldGuiHint = dm.ReadValue(@"
select
    gui_hint
from
    sys_table_field
where
    sys_table_field_id = :id
", new DataParameters(":id", tblFieldId, DbType.Int32)) as string;
                                    if (!String.IsNullOrEmpty(tableFieldGuiHint)) {
                                        if (String.Compare(guiHint, tableFieldGuiHint, true) == 0) {
                                            guiHint = null;
                                        }
                                    }
                                }




                                if (dvFieldId < 0) {

                                    // create dataview field mappings
                                    // 2010/10/19 Brock Weaver brock@circaware.com
                                    //            adding support for table_alias_name
                                    dvFieldId = dm.Write(@"
insert into 
	sys_dataview_field
(sys_dataview_id, field_name, sys_table_field_id, is_readonly, is_primary_key, is_transform, sort_order, foreign_key_dataview_name, gui_hint, group_name, table_alias_name, is_visible, configuration_options, created_date, created_by, owned_date, owned_by)
values
(:rsid, :fieldname, :tablefieldid, :isreadonly, :isprimarykey, :istransform, :sortorder, :fkrsname, :guihint, :groupname, :aliasname, :isvisible, :options, :createddate, :createdby, :owneddate, :ownedby)
", true, "sys_dataview_field_id", new DataParameters(
                                ":rsid", dvId, DbType.Int32,
                                ":fieldname", dvFieldName, DbType.String,
                                ":tablefieldid", tblFieldId, DbType.Int32,
                                ":isreadonly", rsFieldReadOnly, DbType.String,
                                ":isprimarykey", rsFieldPrimaryKey, DbType.String,
                                ":istransform", rsFieldTransform, DbType.String,
                                ":sortorder", sortOrder, DbType.Int32,
                                ":fkrsname", rsForeignKeyDataViewName, DbType.String,
                                ":guihint", guiHint, DbType.String,
                                ":groupname", String.IsNullOrEmpty(rsFieldGroupName) ? null : rsFieldGroupName, DbType.String,
                                ":aliasname", aliasName, DbType.String,
                                ":isvisible", isVisible, DbType.String,
                                ":options", options, DbType.String,
                                ":createddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
                                ":createdby", CooperatorID, DbType.Int32,
                                ":owneddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
                                ":ownedby", CooperatorID, DbType.Int32
                                ));

                                } else {

                                    dm.Write(@"
update
	sys_dataview_field
set
	field_name = :fname,
	sys_table_field_id = :tblfieldid,
	is_readonly = :isreadonly,
    is_primary_key = :isprimarykey,
    is_transform = :istransform,
	sort_order = :sortorder,
	foreign_key_dataview_name = :fkrsname,
    gui_hint = :guihint,
    group_name = :groupname,
    table_alias_name = :aliasname,
    is_visible = :isvisible,
    configuration_options = :options,
	modified_date = :modifieddate,
	modified_by = :modifiedby
where
	sys_dataview_field_id = :rsfieldid
", new DataParameters(
        ":fname", dvFieldName, DbType.String,
        ":tblfieldid", tblFieldId, DbType.Int32,
        ":isreadonly", rsFieldReadOnly, DbType.String,
        ":isprimarykey", rsFieldPrimaryKey, DbType.String,
        ":istransform", rsFieldTransform, DbType.String,
        ":sortorder", sortOrder, DbType.Int32,
        ":fkrsname", rsForeignKeyDataViewName, DbType.String,
        ":guihint", guiHint, DbType.String,
        ":groupname", String.IsNullOrEmpty(rsFieldGroupName) ? null : rsFieldGroupName, DbType.String,
        ":aliasname", aliasName, DbType.String,
        ":isvisible", isVisible, DbType.String,
        ":options", options, DbType.String,
        ":rsfieldid", dvFieldId, DbType.Int32,
        ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
        ":modifiedby", CooperatorID, DbType.Int32
        ));
                                }

                                fieldIds.Add(dvFieldId);

                                string dvFieldDesc = "";

                                // create friendly names if needed
                                foreach (KeyValuePair<string, string> kvp in friendlyNames) {


                                    var langID = Toolkit.ToInt32(kvp.Key, -1);

                                    // HACK: we only write dataview field lang records 
                                    //       if the corresponding table field lang record doesn't match
                                    var tableFieldLangTitle = dm.ReadValue(@"
select
    title
from
    sys_table_field_lang
where
    sys_table_field_id = :fieldid
    and sys_lang_id = :langid
", new DataParameters(":fieldid", tblFieldId, DbType.Int32, ":langid", langID, DbType.Int32)) as string;



                                    int dvFieldFriendlyId = Toolkit.ToInt32(dm.ReadValue(@"
select
	sys_dataview_field_lang_id
from
	sys_dataview_field_lang
where
	sys_dataview_field_id = :rsfieldid
	and sys_lang_id = :seclangid
", new DataParameters(":rsfieldid", dvFieldId, DbType.Int32, ":seclangid", langID, DbType.Int32)), -1);

                                    var writeDataviewFieldLang = false;
                                    if (!String.IsNullOrEmpty(kvp.Value) && tableFieldLangTitle != kvp.Value) {
                                        writeDataviewFieldLang = true;
                                    }

                                    if (writeDataviewFieldLang) {


                                        if (dvFieldFriendlyId < 0) {

                                            dvFieldFriendlyId = dm.Write(@"
insert into
	sys_dataview_field_lang
(sys_dataview_field_id, sys_lang_id, title, description, created_date, created_by, owned_date, owned_by)
values
(:rsfieldid, :seclangid, :friendlyname, :description, :createddate, :createdby, :owneddate, :ownedby)
", true, "sys_dataview_field_lang_id", new DataParameters(
            ":rsfieldid", dvFieldId, DbType.Int32,
            ":seclangid", langID, DbType.Int32,
            ":friendlyname", kvp.Value,
            ":description", dvFieldDesc,
            ":createddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
            ":createdby", CooperatorID, DbType.Int32,
            ":owneddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
            ":ownedby", CooperatorID, DbType.Int32
             ));

                                        } else {

                                            dm.Write(@"
update
	sys_dataview_field_lang
set
	title = :friendlyname,
	description = :description,
	modified_date = :modifieddate,
	modified_by = :modifiedby
where
	sys_dataview_field_lang_id = :rsfieldfriendlyid
", new DataParameters(
            ":friendlyname", kvp.Value,
            ":description", dvFieldDesc,
            ":rsfieldfriendlyid", dvFieldFriendlyId, DbType.Int32,
            ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
            ":modifiedby", CooperatorID, DbType.Int32
            ));
                                        }
                                        friendlyFieldIds.Add(dvFieldFriendlyId);
                                    }


                                }

                            }
                        }
                    }

                    // remove orphaned dv field lang recs
                    dm.Write("delete from sys_dataview_field_lang where sys_dataview_field_id in (select sys_dataview_field_id from sys_dataview_field where sys_dataview_id = :dvid) and sys_dataview_field_lang_id not in (:dvlangids)", new DataParameters(":dvid", dvId, DbType.Int32, ":dvlangids", friendlyFieldIds, DbPseudoType.IntegerCollection));

                    // remove orphaned dv field recs
                    dm.Write("delete from sys_dataview_field where sys_dataview_id = :dvid and sys_dataview_field_id not in (:dvfieldids)", new DataParameters(":dvid", dvId, DbType.Int32, ":dvfieldids", fieldIds, DbPseudoType.IntegerCollection));

                    // remove orphaned dv sql recs
                    dm.Write("delete from sys_dataview_sql where sys_dataview_id = :dvid and sys_dataview_sql_id not in (:dvsqlids)", new DataParameters(":dvid", dvId, DbType.Int32, ":dvsqlids", sqlIds, DbPseudoType.IntegerCollection));

                    // remove orphaned dv lang recs (language-specific title/description for dv itself)
                    dm.Write("delete from sys_dataview_lang where sys_dataview_id = :dvid and sys_dataview_lang_id not in (:dvlangids)", new DataParameters(":dvid", dvId, DbType.Int32, ":dvlangids", langIds, DbPseudoType.IntegerCollection));

                    // NOTE: dv param recs are deleted and recreated every time, so there's no chance of orphaned ones existing

                    var dtSqls = dtDVSqlInfo.Clone();
                    foreach (DataRow drSqlInfo in dtDVSqlInfo.Rows) {
                        dtSqls.Rows.Add(drSqlInfo.ItemArray);
                    }
                    dsReturn.Tables.Add(dtSqls);


                    // finally commit
                    dm.Commit();

                    // we just saved changes. clear the dataview cache
                    Dataview.ClearCache();
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;
        }
        #endregion DataView Management

        #region Search


        public DataSet GetSearchEngineInfoEx(bool enabledIndexesOnly, string onlyThisIndex, string onlyThisResolver, string bindingType, string bindingUrl) {
            using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                return c.GetInfoEx(enabledIndexesOnly, onlyThisIndex, onlyThisResolver);
            }
        }

        public DataSet GetSearchEngineStatus(string bindingType, string bindingUrl) {
            using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                return c.GetStatus();
            }
        }

        public DataSet GetSearchEngineLog(string bindingType, string bindingUrl) {
            DataSet dsReturn = this.createReturnDataSet();

            //// go get the results from the search engine
            string output = null;
            using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                output = c.GetLatestMessages(5000);
            }
            using (var sr = new StringReader(output)) {
                var dt = dsReturn.Tables.Add("search_engine_log");
                dt.Columns.Add("date", typeof(DateTime));
                dt.Columns.Add("type", typeof(string));
                dt.Columns.Add("message", typeof(string));
                //foreach (var row in Toolkit.ParseCSV(tr, true)) {
                foreach (var row in Toolkit.ParseTabDelimited(sr, true)) {
                    dt.Rows.Add(new object[] { Toolkit.ToDateTime(row[0], "yyyy/MM/dd HH:mm:ss", true, DateTime.Now), row[1], row[2] });
                }
            }
            return dsReturn;

        }

        public bool PingSearchEngine(string bindingType, string bindingUrl) {
            using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                return c.Ping();
            }
        }
        #endregion Search

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "MiddleTier", "AdminData", resourceName, null, defaultValue, substitutes);
        }

    }
}
