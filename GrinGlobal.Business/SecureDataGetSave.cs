using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;
using System.Data;
using GrinGlobal.Interface.DataTriggers;
using GrinGlobal.Business.DataTriggers;
using GrinGlobal.Interface.Dataviews;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;

namespace GrinGlobal.Business {
    public partial class SecureData {

        #region Permissions

        #region Permission Checks

        #region Create

        //		[DebuggerStepThrough()]
        public bool CanCreate(DataManager dm, string tableName, DataRow drInput, DataRow drExisting) {
            Permission[] perms = getPermissions(dm, this.SysUserID, tableName, null, drInput, drExisting);
            return hasPermission("C", perms);
        }

        //		[DebuggerStepThrough()]
        public bool CanCreate(DataManager dm, DataRow drInput, DataRow drExisting, IDataview dv) {
            Permission[] perms = getPermissions(dm, this.SysUserID, drInput, drExisting, dv);
            return hasPermission("C", perms);
        }

        #endregion Create

        #region Read

        //[DebuggerStepThrough()]
        public bool CanRead(DataManager dm, string tableName, DataRow drInput, DataRow drExisting) {
            Permission[] perms = getPermissions(dm, this.SysUserID, tableName, null, drInput, drExisting);
            return hasPermission("R", perms);
        }

        //		[DebuggerStepThrough()]
        public bool CanRead(DataManager dm, DataRow drInput, DataRow drExisting, IDataview dv) {
            Permission[] perms = getPermissions(dm, this.SysUserID, drInput, drExisting, dv);
            return hasPermission("R", perms);
        }

        #endregion Read

        #region Update
        //[DebuggerStepThrough()]
        public bool CanUpdate(DataManager dm, string tableName, DataRow drInput, DataRow drExisting) {
            Permission[] perms = getPermissions(dm, this.SysUserID, tableName, null, drInput, drExisting);
            return hasPermission("U", perms);
        }

        //		[DebuggerStepThrough()]
        public bool CanUpdate(DataManager dm, DataRow drInput, DataRow drExisting, IDataview dv) {
            Permission[] perms = getPermissions(dm, this.SysUserID, drInput, drExisting, dv);
            return hasPermission("U", perms);
        }

        #endregion Update

        #region Delete

        //[DebuggerStepThrough()]
        public bool CanDelete(DataManager dm, string tableName, DataRow drInput, DataRow drExisting) {
            Permission[] perms = getPermissions(dm, this.SysUserID, tableName, null, drInput, drExisting);
            return hasPermission("D", perms);
        }


        //		[DebuggerStepThrough()]
        public bool CanDelete(DataManager dm, DataRow drInput, DataRow drExisting, IDataview dv) {
            Permission[] perms = getPermissions(dm, this.SysUserID, drInput, drExisting, dv);
            return hasPermission("D", perms);
        }

        #endregion Delete

        /// <summary>
        /// Returns the cooperator.cooperator_id for sys_user.user_name = 'SYSTEM'.  If not found (it always should be), returns -1.
        /// </summary>
        /// <returns></returns>
        public int GetCooperatorIDForSystem() {
            return getCooperatorIDForUser("SYSTEM");
        }

        /// <summary>
        /// Returns the cooperator.cooperator_id for sys_user.user_name = given user name.  If not found, returns -1.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private int getCooperatorIDForUser(string userName) {
            var cm = CacheManager.Get("GenericSettings");
            var cooperatorID = Toolkit.ToInt32(cm["CooperatorIDFor" + userName], -1);
            if (cooperatorID < 0) {
                using (var dm = BeginProcessing(true)) {
                    try {
                        cooperatorID = Toolkit.ToInt32(dm.ReadValue("select c.cooperator_id from cooperator c inner join sys_user su on c.cooperator_id = su.cooperator_id where su.user_name = :username", new DataParameters(":username", userName)), -1);
                        cm["CooperatorIDFor" + userName] = cooperatorID;
                    } catch {
                        // assume left to right if the lookup fails for some reason
                        cooperatorID = -1;
                    }
                }
            }
            return cooperatorID;
        }

        /// <summary>
        /// Returns the cooperator.cooperator_id for sys_user.user_name = 'guest' (or whatever value is in the 'AnonymousUserName' app setting).  Returns -1 if not found.
        /// </summary>
        /// <returns></returns>
        public int GetCooperatorIDForGuestWebUser() {
            return getCooperatorIDForUser(Toolkit.GetSetting("AnonymousUserName", "guest"));
        }

        /// <summary>
        /// Returns the cooperator.cooperator_id for the given web_cooperator_id.  Returns -1 if not found.
        /// </summary>
        /// <param name="webCooperatorID"></param>
        /// <returns></returns>
        private int getCooperatorIDForUser(int webCooperatorID) {
            var cm = CacheManager.Get("GenericSettings");
            var cooperatorID = Toolkit.ToInt32(cm["CooperatorIDForWebCoop" + webCooperatorID], -1);
            if (cooperatorID < 0) {
                using (var dm = BeginProcessing(true)) {
                    try {
                        cooperatorID = Toolkit.ToInt32(dm.ReadValue("select c.cooperator_id from cooperator c where c.web_cooperator_id = :webcoopid", new DataParameters(":webcoopid", webCooperatorID, DbType.Int32)), -1);
                        cm["CooperatorIDForWebCoop" + webCooperatorID] = cooperatorID;
                    } catch {
                        // assume left to right if the lookup fails for some reason
                        cooperatorID = -1;
                    }
                }
            }
            return cooperatorID;
        }

        /// <summary>
        /// Returns sys_user.sys_user_id for the 'guest' user name (or whatever user name is stored in the 'AnonymousUserName' config setting).  Returns -1 if not found.
        /// </summary>
        /// <returns></returns>
        protected int GetSysUserIDForGuestWebUser() {
            return getSysUserIDForUser(Toolkit.GetSetting("AnonymousUserName", "guest"));
        }

        /// <summary>
        /// Returns sys_user.sys_user_id for the given user name.  Returns -1 if not found.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        protected int getSysUserIDForUser(string userName) {
            var cm = CacheManager.Get("GenericSettings");
            var sysUserID = Toolkit.ToInt32(cm["SysUserIDFor" + userName], -1);
            if (sysUserID < 0) {
                using (var dm = BeginProcessing(true)) {
                    try {
                        sysUserID = Toolkit.ToInt32(dm.ReadValue("select sys_user_id from sys_user su where su.user_name = :username", new DataParameters(":username", userName)), -1);
                        cm["SysUserIDFor" + userName] = sysUserID;
                    } catch {
                        // assume left to right if the lookup fails for some reason
                        sysUserID = -1;
                    }
                }
            }
            return sysUserID;
        }

        /// <summary>
        /// Returns sys_user.sys_user_id for the given cooperator.web_cooperator_id.  Returns -1 if not found.
        /// </summary>
        /// <param name="webCooperatorID"></param>
        /// <returns></returns>
        protected int getSysUserIDForUser(int webCooperatorID) {
            var cm = CacheManager.Get("GenericSettings");
            var sysUserID = Toolkit.ToInt32(cm["SysUserIDForWebCoop" + webCooperatorID], -1);
            if (sysUserID < 0) {
                using (var dm = BeginProcessing(true)) {
                    try {
                        sysUserID = Toolkit.ToInt32(dm.ReadValue("select sys_user_id from sys_user su inner join cooperator c on su.cooperator_id = c.cooperator_id where c.web_cooperator_id = :webcoopid", new DataParameters(":webcoopid", webCooperatorID, DbType.Int32)), -1);
                        cm["SysUserIDForWebCoop" + webCooperatorID] = sysUserID;
                    } catch {
                        // assume left to right if the lookup fails for some reason
                        sysUserID = -1;
                    }
                }
            }
            return sysUserID;
        }

        /// <summary>
        /// Returns sys_user.user_name for the given cooperator.web_cooperator_id.  Returns -1 if not found.
        /// </summary>
        /// <param name="webCooperatorID"></param>
        /// <returns></returns>
        protected string getSysUserNameForUser(int webCooperatorID) {
            var cm = CacheManager.Get("GenericSettings");
            var systemUserName = cm["SysUserNameForWebCoop" + webCooperatorID] as string;
            if (String.IsNullOrEmpty(systemUserName)) {
                using (var dm = BeginProcessing(true)) {
                    try {
                        systemUserName = (string)dm.ReadValue("select user_name from sys_user su inner join cooperator c on su.cooperator_id = c.cooperator_id where c.web_cooperator_id = :webcoopid", new DataParameters(":webcoopid", webCooperatorID, DbType.Int32));
                        cm["SysUserNameForWebCoop" + webCooperatorID] = systemUserName + string.Empty;
                    } catch {
                        systemUserName = "";
                    }
                }
            }
            return systemUserName;
        }

        /// <summary>
        /// Returns sys_group.sys_group_id for given groupTag.  Returns -1 if not found.
        /// </summary>
        /// <param name="groupTag"></param>
        /// <returns></returns>
        public int GetSysGroupIDForTag(string groupTag) {
            var cm = CacheManager.Get("GenericSettings");
            var sysGroupID = Toolkit.ToInt32(cm["SysGroupIDFor" + groupTag], -1);
            if (sysGroupID < 0) {
                using (var dm = BeginProcessing(true)) {
                    try {
                        sysGroupID = Toolkit.ToInt32(dm.ReadValue("select sys_group_id from sys_group sg where sg.group_tag = :tag", new DataParameters(":tag", groupTag)), -1);
                        cm["SysGroupIDFor" + groupTag] = sysGroupID;
                    } catch {
                        // assume left to right if the lookup fails for some reason
                        sysGroupID = -1;
                    }
                }
            }
            return sysGroupID;
        }


        /// <summary>
        /// Returns all sys_group.group_name values associated with the given system user_name or sys_user_id.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<string> GetRoles(string username, int userID) {
            List<string> roles = CacheManager.Get("roles")[username + @"/-\" + userID] as List<string>;

            if (roles != null) {
                // use the cached roles
                return roles;
            } else {
                // create an empty list to fill
                roles = new List<string>();
            }
            DataSet dsReturn = createReturnDataSet();
            try {


                using (DataManager dm = BeginProcessing(true)) {
                    // To keep things simple on the database administration side,
                    // we implement all roles as Permissions that simply are all inherit.
                    // This allows a generic approach and is easy to implement.

                    if (String.IsNullOrEmpty(username) && userID < 0) {
                        // default to current user if neither username nor userID are specified
                        userID = _sysUserID;
                    }

                    DataTable dt2 = dm.Read(@"
select
    sg.sys_group_id,
    sg.group_tag,
    coalesce(sgl.title, sg.group_tag) as group_name
from
    sys_group sg inner join sys_group_user_map sgum 
        on sg.sys_group_id = sgum.sys_group_id
    inner join sys_user su
        on su.sys_user_id = sgum.sys_user_id
    left join sys_group_lang sgl
        on sgl.sys_group_id = sg.sys_group_id 
        and sgl.sys_lang_id = :langid
where
    (su.user_name = :username or su.sys_user_id = :userid)
    and sg.is_enabled = 'Y'
", new DataParameters(
     ":username", username, ":userid", userID, DbType.Int32,
     ":langid", LanguageID, DbType.Int32));

                    foreach (DataRow dr2 in dt2.Rows) {
                        roles.Add(dr2["group_tag"].ToString().ToLower());
                    }

                    //                    DataTable dt = dm.Read(@"
                    //select
                    //    sr.role_name as role_name
                    //from
                    //sys_user_role sur inner join sys_role sr
                    //    on sur.sys_role_id = sr.sys_role_id
                    //inner join sys_user su
                    //    on su.sys_user_id = sur.sys_user_id
                    //where
                    //    su.user_name = :username
                    //    and sur.is_enabled = 'Y'
                    //    and sr.is_enabled = 'Y'
                    //", new DataParameters(":username", username));
                    //                    foreach (DataRow dr in dt.Rows) {
                    //                        roles.Add(dr["role_name"].ToString().ToLower());
                    //                    }

                    // remember this users' roles in the cache
                    CacheManager.Get("roles")[username + @"/-\" + userID] = roles;
                    return roles;
                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, _languageID);
            }
            return roles;
        }

        /// <summary>
        /// Returns true if current system user is a member of the 'ADMINS' group as defined by sys_group / sys_group_user_map / sys_user tables.
        /// </summary>
        /// <returns></returns>
        public bool IsAdministrator() {
            return IsInRole("admins");
        }

        /// <summary>
        /// Returns true if current system user is a member of the given group as defined by sys_group / sys_group_user_map / sys_user tables.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool IsInRole(string roleName) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    // put code specific to the method here

                    return 0 < Toolkit.ToInt32((dm.ReadValue(@"
select
    count(sg.sys_group_id) as sys_group_id_count
from
    sys_group sg inner join sys_group_user_map sgum 
        on sg.sys_group_id = sgum.sys_group_id
where
    sgum.sys_user_id = :userid
    and sg.group_tag = :name
    and sg.is_enabled = 'Y'
", new DataParameters(":userid", _sysUserID, DbType.Int32, ":name", roleName))), -1);

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, _languageID);
            }
            return false;
        }

        /// <summary>
        /// Pass "C", "R", "U", "D", or any permutation thereof to determine what permissions the user has based on the given crudPerms.
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="permissionMask">A sequence of the characters 'C', 'R', 'U', 'D' in any order</param>
        /// <param name="dm"></param>
        /// <param name="suppressExceptions"></param>
        /// <returns></returns>
        private bool hasPermission(string permissionMask, Permission[] crudPerms) {
            if (String.IsNullOrEmpty(permissionMask)) {
                throw Library.CreateBusinessException(getDisplayMember("hasPermission{nomask}", "No permission was passed to GrinGlobal.Business.SecureData.hasPermission().  'C', 'R', 'U', 'D', or any combination thereof must be given."));
            }

            for (int i = 0; i < permissionMask.Length; i++) {
                bool denied = false;
                Permission current = null;
                switch (permissionMask[i]) {
                    case 'C':
                        current = crudPerms[(int)PermissionAction.Create];
                        break;
                    case 'R':
                        current = crudPerms[(int)PermissionAction.Read];
                        break;
                    case 'U':
                        current = crudPerms[(int)PermissionAction.Update];
                        break;
                    case 'D':
                        current = crudPerms[(int)PermissionAction.Delete];
                        break;
                    default:
                        break;
                }
                denied = current.Level == PermissionLevel.Deny;

                if (denied) {
                    return false;
                }
            }
            return true;

        }

        /// <summary>
        /// Returns all sys_group.group_name values associated with the given web user_name.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<string> GetWebUserRoles(string username)
        {
            List<string> roles = CacheManager.Get("roles")[username] as List<string>;

            if (roles != null)
            {
                // use the cached roles
                return roles;
            }
            else
            {
                // create an empty list to fill
                roles = new List<string>();
            }
            DataSet dsReturn = createReturnDataSet();
            try
            {
                using (DataManager dm = BeginProcessing(true))
                {
                    DataTable dt2 = dm.Read(@"
select
    sg.sys_group_id,
    sg.group_tag,
    coalesce(sgl.title, sg.group_tag) as group_name
from
    sys_group sg inner join sys_group_user_map sgum 
        on sg.sys_group_id = sgum.sys_group_id
    inner join sys_user su
        on su.sys_user_id = sgum.sys_user_id
    left join sys_group_lang sgl
        on sgl.sys_group_id = sg.sys_group_id 
        and sgl.sys_lang_id = :langid
    join cooperator c
		on c.cooperator_id = su.cooperator_id 
	join web_cooperator wc
		on c.web_cooperator_id = wc.web_cooperator_id
	join web_user wu
		on wc.web_cooperator_id = wu.web_cooperator_id
where
    (wu.user_name = :username)
    and sg.is_enabled = 'Y'
", new DataParameters(
     ":username", username,
     ":langid", LanguageID, DbType.Int32));

                    foreach (DataRow dr2 in dt2.Rows)
                    {
                        roles.Add(dr2["group_tag"].ToString().ToLower());
                    }

                    CacheManager.Get("roles")[username] = roles;
                    return roles;
                }
            }
            catch (Exception ex)
            {
                if (LogException(ex, dsReturn))
                {
                    throw ex;
                }
            }
            finally
            {
                FinishProcessing(dsReturn, _languageID);
            }
            return roles;
        }


        #endregion Permission Checks

        /// <summary>
        /// Calculates 'new' permission using given permission and level.  Makes no assumptions otherwise (only inspects passed parameters).  See method comments for more information.
        /// </summary>
        /// <param name="perm"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private Permission calcPermissionLevel(Permission perm, string level) {

            //// this method assumes nothing about ordering of permissions.
            //// it simply assigns the permission level based on the current permissions and the given level.

            //// PermissionLevel precedence:
            //// Varies > Deny > Approve > Inherit

            //// the PermissionLevel enum is written such that the int value determines precedence.
            //// that means we can just replace the current level if it's < the new one.
            //// HOWEVER! If the current value is 'Inherit' and the new one is 'Varies', we can assume DENY.
            //PermissionLevel newLevel = Permission.ParseLevel(level);
            //if (perm.Level < newLevel) {
            //    if (perm.Level == PermissionLevel.Inherit && newLevel == PermissionLevel.VariesByRow) {
            //        perm.Level = PermissionLevel.Deny;
            //    } else {
            //        perm.Level = newLevel;
            //    }
            //}



            // 2009-09-18 Brock Weaver brock@circaware.com
            // due to how we want to administer permissions (i.e. make as few as possible)
            // we are introducing a CSS-like inheritance scheme.  That is -- a more specific permission overrides
            // a less specific permission (just like applying a style attribute to a specific HTML tag overrides the styles from the class attribute)

            // Applying an explicit Allow or Deny makes the permission that value.
            // Applying a VariesByRow permission is the same as applying a Deny IF current permission is Inherit (otherwise ignored)
            // Applying an Inherit permission changes nothing.

            PermissionLevel newLevel = Permission.ParseLevel(level);
            if (newLevel == PermissionLevel.Deny || newLevel == PermissionLevel.Allow) {
                perm.Level = newLevel;
            } else if (newLevel == PermissionLevel.VariesByRow && perm.Level == PermissionLevel.Inherit) {
                perm.Level = PermissionLevel.Deny;
            }

            return perm;

        }

        /// <summary>
        /// Returns the permissions that apply to the given drInput row.  If drInput is null, returns Deny for everything.
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="userID"></param>
        /// <param name="drInput"></param>
        /// <param name="drExisting"></param>
        /// <param name="fmdv"></param>
        /// <returns></returns>
        protected Permission[] getPermissions(DataManager dm, int userID, DataRow drInput, DataRow drExisting, IDataview fmdv) {

            if (drInput == null) {
                // no row, bomb out
                return Permission.CRUDPermissions(PermissionLevel.Deny);
            }

            return getPermissions(dm, userID, fmdv.PrimaryKeyTableName, fmdv.DataViewName, drInput, drExisting);
            //throw Library.CreateBusinessException("DataView '" + fmrs.DataViewName + "' does not have a primary key defined and is therefore not updatable.");
            //throw Library.CreateBusinessException("Table '" + dr.Table.TableName + "' not found in mappings for dataview '" + fmrs.DataViewName + "' or no primary key is defined for that table in the mappings.  Try re-running ~/admins/mappings.aspx for table '" + dr.Table.TableName + "'.");
            //} else {
            //    var drv = dr.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;
            //    return getPermissions(dm, fmrs.TableNames, fmrs.DataViewName, new int[] { Toolkit.ToInt32(dr[pkName, drv], -1) }, dr);
            //}


        }

        /// <summary>
        /// Using the given parameters, inspects the data in sys_perm / sys_user_perm_map / sys_group / sys_group_user_map to determine permissions. If config setting "DisableSecurity" is true, returns Allow for everything regardless of all other settings.
        /// Priority is always in the following order (MOST SIGNIFICANT LAST):
        /// 1. group-level, no table, no dataview
        /// 2. group-level, dataview but no table
        /// 3. group-level, table but no dataview
        /// 4. group-level, dataview and table
        /// 5. user-level,  no table, no dataview
        /// 6. user-level,  dataview but no table
        /// 7. user-level,  table but no dataview
        /// 8. user-level,  dataview and table
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="userID"></param>
        /// <param name="tableName"></param>
        /// <param name="dataviewName"></param>
        /// <param name="drInput"></param>
        /// <param name="drExisting"></param>
        /// <returns></returns>
        protected Permission[] getPermissions(DataManager dm, int userID, string tableName, string dataviewName, DataRow drInput, DataRow drExisting) {

            // ****************************************************************************************************************
            // ******** Please be aware that any changes to this method will require significant regression testing ***********
            // ****************************************************************************************************************

            // This method represents the heart of the GRIN-Global security mechanism.  

            // The SecurityDataTrigger essentially is just an entry point for eventually calling into this method to do the dirty work.
            // Since the Admin Tool calls this to display existing permissions outside the context of an actual data transaction, I decided
            // to put the code in SecureDataGetSave.cs instead of the SecurityDataTrigger.cs itself.


            // HACK: make sure we always get all security records!!!!
            var origLimit = dm.Limit;
            try {

                dm.Limit = 0;

                if (Toolkit.GetSetting("DisableSecurity", false) || isAdministrator(userID, dm)) {
                    // User is an admin or security is disabled. let them do whatever they want
                    return Permission.CRUDPermissions(PermissionLevel.Allow);
                }



                // pull from cache if possible
                // HACK: assumes first field is pk field if no PrimaryKeyNames exist in the associated DataTable -- and therefore unique!!!
                string inputKey = null;
                if (drInput != null) {
                    var drv = drInput.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;
                    string pkName = drInput.Table.PrimaryKeyName();
                    if (String.IsNullOrEmpty(pkName)) {
                        pkName = drInput.Table.Columns[0].ColumnName;
                    }
                    inputKey = drInput[pkName, drv].ToString();
                }

                var ikValue = Toolkit.ToInt32(inputKey, -1);
                if (ikValue < 0) {
                    // map all 'new' records to 0 so cache is hit as needed
                    inputKey = "-1";
                }

                string cacheKey = userID + ":" + dataviewName + ":" + tableName + ":" + inputKey;
                var cm = CacheManager.Get("PermissionAssignment");
                Permission[] perms = cm[cacheKey] as Permission[];
                if (perms != null) {
                    // use cached version
                    return perms;
                }



                bool cacheable = true;


                // default to inheriting everything so db-specific settings override them.
                // all non-specified ones will be automatically turned into Deny at the end of this method.
                Permission[] crudPerms = Permission.CRUDPermissions(PermissionLevel.Inherit);

                // this sql only pulls back permissions that:
                // - are for the specified table and user id
                // - have enabled = 'Y'

                // the sorting here is very important!
                // an Allow permission can override a Deny if it is 'closer'
                // that is, if Deny is at the 'apply to any dataview' level,
                // an Allow at the 'apply to this specific dataview' can override it since
                // the dataview is explicitly named.
                // same goes for the table level, but it has higher priority
                // than the dataview.  This is why we do * 1,000,000 in the first column below, 
                // so priority is always in the following order (most significant last):
                //
                // 1. group-level, no table, no dataview
                // 2. group-level, dataview but no table
                // 3. group-level, table but no dataview
                // 4. group-level, dataview and table
                // 5. user-level,  no table, no dataview
                // 6. user-level,  dataview but no table
                // 7. user-level,  table but no dataview
                // 8. user-level,  dataview and table

                string parentTable = null;
                string foreignKeyField = null;
                parentTable = getParentTable(tableName, true, ref foreignKeyField, dm);

                var groupSql = @"

select 
    1 as perm_calc_type,
    coalesce(sp.sys_table_id, 0) as table_rank,
    coalesce(sp.sys_dataview_id, 0) as dataview_rank,
    sp.sys_permission_id
      ,sp.sys_dataview_id
      ,sp.sys_table_id
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
      ,sp.owned_by
from
	sys_user su
	inner join sys_group_user_map sgum
		on sgum.sys_user_id = su.sys_user_id
    inner join sys_group_permission_map sgpm
        on sgum.sys_group_id = sgpm.sys_group_id
	inner join sys_permission sp
		on sgpm.sys_permission_id = sp.sys_permission_id
    inner join sys_permission_field spf
        on sp.sys_permission_id = spf.sys_permission_id
    left join sys_permission_lang spl
        on sp.sys_permission_id = spl.sys_permission_id
        and spl.sys_lang_id = :langid1
    left join sys_dataview sdv
        on sp.sys_dataview_id = sdv.sys_dataview_id
    left join sys_table st
        on sp.sys_table_id = st.sys_table_id
where
	coalesce(su.is_enabled, 'N') = 'Y'
    and coalesce(sp.is_enabled, 'N') = 'Y'
	and st.table_name in (:tablenames1)
    and spf.compare_mode = 'parent'
	and su.sys_user_id = :userid1

UNION ALL

select 
    2 as perm_calc_type,
    coalesce(sp.sys_table_id, 0) as table_rank,
    coalesce(sp.sys_dataview_id, 0) as dataview_rank,
    sp.sys_permission_id
      ,sp.sys_dataview_id
      ,sp.sys_table_id
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
      ,sp.owned_by
from
	sys_user su
	inner join sys_group_user_map sgum
		on sgum.sys_user_id = su.sys_user_id
    inner join sys_group_permission_map sgpm
        on sgum.sys_group_id = sgpm.sys_group_id
	inner join sys_permission sp
		on sgpm.sys_permission_id = sp.sys_permission_id
    left join sys_permission_lang spl
        on sp.sys_permission_id = spl.sys_permission_id
        and spl.sys_lang_id = :langid2
    left join sys_dataview sdv
        on sp.sys_dataview_id = sdv.sys_dataview_id
    left join sys_table st
        on sp.sys_table_id = st.sys_table_id
where
	coalesce(su.is_enabled, 'N') = 'Y'
    and coalesce(sp.is_enabled, 'N') = 'Y'
	and (
        sdv.dataview_name = :dataview2
        or 
        st.table_name in (:tablenames2)
        or
        (sp.sys_dataview_id is null and sp.sys_table_id is null)
        )
	and su.sys_user_id = :userid2
";

                var userSql = @"
select 
    3,
    coalesce(sp.sys_table_id, 0) as table_rank,
    coalesce(sp.sys_dataview_id, 0) as dataview_rank,
    sp.sys_permission_id
      ,sp.sys_dataview_id
      ,sp.sys_table_id
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
      ,sp.owned_by
from
	sys_user su
	inner join sys_user_permission_map sup
		on sup.sys_user_id = su.sys_user_id
	inner join sys_permission sp
		on sup.sys_permission_id = sp.sys_permission_id
    inner join sys_permission_field spf
        on sp.sys_permission_id = spf.sys_permission_id
    left join sys_permission_lang spl
        on sp.sys_permission_id = spl.sys_permission_id
        and spl.sys_lang_id = :langid3
    left join sys_dataview sdv
        on sp.sys_dataview_id = sdv.sys_dataview_id
    left join sys_table st
        on sp.sys_table_id = st.sys_table_id
where
	coalesce(su.is_enabled, 'N') = 'Y'
    and coalesce(sp.is_enabled, 'N') = 'Y'
    and coalesce(sup.is_enabled, 'N') = 'Y'
    and spf.compare_mode = 'parent'
	and st.table_name in (:tablenames3)
	and su.sys_user_id = :userid3

UNION ALL

select 
    4,
    coalesce(sp.sys_table_id, 0) as table_rank,
    coalesce(sp.sys_dataview_id, 0) as dataview_rank,
    sp.sys_permission_id
      ,sp.sys_dataview_id
      ,sp.sys_table_id
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
      ,sp.owned_by
from
	sys_user su
	inner join sys_user_permission_map sup
		on sup.sys_user_id = su.sys_user_id
	inner join sys_permission sp
		on sup.sys_permission_id = sp.sys_permission_id
    left join sys_permission_lang spl
        on sp.sys_permission_id = spl.sys_permission_id
        and spl.sys_lang_id = :langid4
    left join sys_dataview sdv
        on sp.sys_dataview_id = sdv.sys_dataview_id
    left join sys_table st
        on sp.sys_table_id = st.sys_table_id
where
	coalesce(su.is_enabled, 'N') = 'Y'
    and coalesce(sp.is_enabled, 'N') = 'Y'
    and coalesce(sup.is_enabled, 'N') = 'Y'
	and (
        sdv.dataview_name = :dataview4
        or 
        st.table_name in (:tablenames4)
        or
        (sp.sys_dataview_id is null and sp.sys_table_id is null)
        )
	and su.sys_user_id = :userid4
order by 
    1, 2, 3
";
                if (String.IsNullOrEmpty(dataviewName)) {
                    // that mess is just to make sure we match on no dataviews
                    dataviewName = ")&*%   $ ^(&*1";
                }

                if (String.IsNullOrEmpty(tableName)) {
                    // that mess is just to make sure we match on no tables
                    tableName = "'__)(  *&  ^ & )*(1'";
                } else {
                    tableName = "'" + tableName.Replace("'", "''") + "'";
                }

                if (String.IsNullOrEmpty(parentTable)) {
                    // that mess is just to make sure we match on no tables
                    parentTable = "'__)(  *&  ^ & )*(1'";
                } else {
                    parentTable = "'" + parentTable.Replace("'", "''") + "'";
                }

#if DISABLE_SECURITY_GROUPS

                DataTable dtPerms = dm.Read(userSql,
 new DataParameters(
     ":dataview", dataviewName,
     ":tablenames", tableName, DbPseudoType.StringReplacement,
     ":userid", userID, DbType.Int32));
#else
                var sql = groupSql + " UNION ALL " + userSql;

                // HACK: horrible, horrible, horrible, horrible hack to support oracle's
                //       insistence that a UNION clause with CLOBs in the same locations in the two queries
                //       are different types and therefore cannot occupy the same field.
                if (dm.DataConnectionSpec.Vendor == DataVendor.Oracle) {
                    sql = sql.Replace("sp.description", "to_char(sp.description)");
                }


                DataTable dtPerms = dm.Read(sql,
     new DataParameters(
         ":langid1", LanguageID, DbType.Int32,
                    //     ":dataview1", dataviewName,   
         ":tablenames1", parentTable, DbPseudoType.StringReplacement,
         ":userid1", userID, DbType.Int32,

         ":langid2", LanguageID, DbType.Int32,
         ":dataview2", dataviewName,
         ":tablenames2", tableName, DbPseudoType.StringReplacement,
         ":userid2", userID, DbType.Int32,

         ":langid3", LanguageID, DbType.Int32,
                    //     ":dataview3", dataviewName,
         ":tablenames3", parentTable, DbPseudoType.StringReplacement,
         ":userid3", userID, DbType.Int32,

         ":langid4", LanguageID, DbType.Int32,
         ":dataview4", dataviewName,
         ":tablenames4", tableName, DbPseudoType.StringReplacement,
         ":userid4", userID, DbType.Int32
         ));
#endif




                foreach (DataRow drPerms in dtPerms.Rows) {
                    // however, to restrict further we have to look into the sys_permission_field to
                    // get the field(s) we must restrict by and their value(s) or field(s) to further look up

                    var permID = Toolkit.ToInt32(drPerms["sys_permission_id"], -1);
                    DataTable dtField = null;

                    var calcType = Toolkit.ToInt32(drPerms["perm_calc_type"], -1);
                    if (calcType == 2 || calcType == 4) {

                        // this permission applies directly to the given table.
                        dtField = dm.Read(@"
select
    coalesce(stdv.table_name, st.table_name) as table_name,
    coalesce(sdvf.field_name, stf.field_name) as field_name,
    stparent.table_name as parent_table_name,
    stfparent.field_name as parent_table_field_name,
    stfparentpk.field_name as parent_pk_table_field_name,
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
      ,spf.owned_by
from
	sys_permission_field spf
    
    /* pull in field info for current dataview, if any */
    left join sys_dataview_field sdvf
        on spf.sys_dataview_field_id = sdvf.sys_dataview_field_id
    left join sys_table_field stfdv
        on sdvf.sys_table_field_id = stfdv.sys_table_field_id
    left join sys_table stdv
        on stfdv.sys_table_id = stdv.sys_table_id

    /* pull in field info for current table, if any */
    left join sys_table_field stf
        on spf.sys_table_field_id = stf.sys_table_field_id
    left join sys_table st
        on stf.sys_table_id = st.sys_table_id

    /* pull in field info for parent table, if any */
    left join sys_table_field stfparent
        on spf.parent_table_field_id = stfparent.sys_table_field_id
    left join sys_table stparent
        on stparent.sys_table_id = stfparent.sys_table_id

    /* pull in PK field info for parent table, if any */
    left join sys_table_field stfparentpk
        on stfparent.sys_table_id = stfparentpk.sys_table_id
          and stfparentpk.is_primary_key = 'Y'

where
	spf.sys_permission_id = :secpermid
", new DataParameters(":secpermid", permID, DbType.Int32));


                    } else {


                        // this permission is actually defined on the parent table.
                        // if it has field restrictions defined that say "for all children", we need to restrict by it.

                        // determine the relationship field between the current table and the parent table.



                        dtField = dm.Read(@"
select
    :tablename as table_name,
    :fkfieldname as field_name,
    stparent.table_name as parent_table_name,
    stfparent.field_name as parent_table_field_name,
    stfparentpk.field_name as parent_pk_table_field_name,
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
      ,spf.owned_by
from
	sys_permission_field spf
    
    /* pull in field info for current dataview, if any */
    left join sys_dataview_field sdvf
        on spf.sys_dataview_field_id = sdvf.sys_dataview_field_id
    left join sys_table_field stfdv
        on sdvf.sys_table_field_id = stfdv.sys_table_field_id
    left join sys_table stdv
        on stfdv.sys_table_id = stdv.sys_table_id

    /* pull in field info for current table, if any */
    left join sys_table_field stf
        on spf.sys_table_field_id = stf.sys_table_field_id
    left join sys_table st
        on stf.sys_table_id = st.sys_table_id

    /* pull in field info for parent table, if any */
    left join sys_table_field stfparent
        on spf.parent_table_field_id = stfparent.sys_table_field_id
    left join sys_table stparent
        on stparent.sys_table_id = stfparent.sys_table_id

    /* pull in PK field info for parent table, if any */
    left join sys_table_field stfparentpk
        on stfparent.sys_table_id = stfparentpk.sys_table_id
          and stfparentpk.is_primary_key = 'Y'

where
	spf.sys_permission_id = :secpermid
", new DataParameters(
     ":tablename", tableName, DbType.String,
     ":fkfieldname", foreignKeyField, DbType.String,
     ":secpermid", permID, DbType.Int32));
                    }



                    // if there are no rows, that just means there is no restriction below the table level.
                    if (dtField.Rows.Count == 0) {

                        // no need to inspect the primary key value since permission is by the table only


                        // since the sql carefully orders things for us (least specific first, getting more specific as we go), 
                        // we can just march right down the line and override permissions as they come out of the table
                        calcPermissionLevel(crudPerms[(int)PermissionAction.Create], drPerms["create_permission"].ToString());
                        calcPermissionLevel(crudPerms[(int)PermissionAction.Read], drPerms["read_permission"].ToString());
                        calcPermissionLevel(crudPerms[(int)PermissionAction.Update], drPerms["update_permission"].ToString());
                        calcPermissionLevel(crudPerms[(int)PermissionAction.Delete], drPerms["delete_permission"].ToString());
                    } else {

                        // since this permission has a row-level restriction, we can't cache it.
                        // this permission will be recalculated every time.
                        cacheable = false;

                        if (drInput == null) {
                            // we need a row to tell us what their access level is.
                            // since we don't have one, we'll return Varies.
                            // Varies will be interpreted by the middle tier as Denied,
                            // but can still be viewed in admin tool(s) as a different level so
                            // it's obvious the field-level stuff is working.

                            var variesCreate = calcPermissionLevel(crudPerms[(int)PermissionAction.Create].Clone(), "V");
                            var actualCreate = calcPermissionLevel(crudPerms[(int)PermissionAction.Create], drPerms["create_permission"].ToString());
                            if (actualCreate.Level != variesCreate.Level) {
                                actualCreate.Level = PermissionLevel.VariesByRow;
                            }

                            var variesRead = calcPermissionLevel(crudPerms[(int)PermissionAction.Read].Clone(), "V");
                            var actualRead = calcPermissionLevel(crudPerms[(int)PermissionAction.Read], drPerms["read_permission"].ToString());
                            if (actualRead.Level != variesRead.Level) {
                                actualRead.Level = PermissionLevel.VariesByRow;
                            }

                            var variesUpdate = calcPermissionLevel(crudPerms[(int)PermissionAction.Update].Clone(), "V");
                            var actualUpdate = calcPermissionLevel(crudPerms[(int)PermissionAction.Update], drPerms["update_permission"].ToString());
                            if (actualUpdate.Level != variesUpdate.Level) {
                                actualUpdate.Level = PermissionLevel.VariesByRow;
                            }

                            var variesDelete = calcPermissionLevel(crudPerms[(int)PermissionAction.Delete].Clone(), "V");
                            var actualDelete = calcPermissionLevel(crudPerms[(int)PermissionAction.Delete], drPerms["delete_permission"].ToString());
                            if (actualDelete.Level != variesDelete.Level) {
                                actualDelete.Level = PermissionLevel.VariesByRow;
                            }

                        } else {

                            var drv = drInput.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

                            // this permission has restrictions below the table level.  i.e. field level.

                            // make a copy of the perms in case we need to revert to them...
                            var crudPermsBeforeFields = Permission.Clone(crudPerms);

                            foreach (DataRow drField in dtField.Rows) {

                                // ok, we're restricting by either a hardcoded value (i.e. some_field = 37)
                                // or by a value we have to determine (i.e. crop_type = id for maize)

                                string fieldName = drField["field_name"].ToString();


                                if (String.IsNullOrEmpty(fieldName)) {
                                    var pkName = drInput.Table.PrimaryKeyName();
                                    if (!String.IsNullOrEmpty(pkName)) {
                                        fieldName = pkName;
                                    }
                                }


                                object fieldValue = null;

                                if (drInput.Table.Columns.Contains(fieldName)) {
                                    fieldValue = drInput[fieldName, drv];
                                } else {
                                    if (drExisting != null) {
                                        if (drExisting.Table.Columns.Contains(fieldName)) {
                                            fieldValue = drExisting[fieldName];
                                        }
                                    } else if (fieldName.ToLower() == "owned_by") {
                                        // existing is null, this means it's a new row.
                                        // field name is owned_by.  It doesn't exist on this row.
                                        fieldValue = CooperatorID.ToString();

                                        //// fill it by default with the cooperator id.
                                        //// subsequent processing will override as necessary to possibly reassign it to the owner_parent value as needed.
                                        ////drInput.Table.Columns.Add("owned_by", typeof(int));
                                        ////drInput["owned_by"] = CooperatorID;
                                    }
                                }

                                // HACK: new rows should always get owned_by set to current cooperator (or at least check security against it that way)?
                                if (fieldValue == DBNull.Value && drInput.RowState == DataRowState.Added && fieldName.ToLower() == "owned_by") {
                                    fieldValue = CooperatorID.ToString();
                                }

                                string compareMode = drField["compare_mode"].ToString();
                                if (compareMode != "parent") {
                                    // comparing against current table (i.e. permission is not defined on parent table)
                                    string fieldType = drField["field_type"].ToString();
                                    string compareOperator = drField["compare_operator"].ToString();
                                    string compareValue = drField["compare_value"].ToString();

                                    // we're comparing against a static value.
                                    if (compareValues(fieldValue, fieldType, compareOperator, compareValue)) {
                                        // this field value does fall in the proper range.
                                        // this means that this permission can now be applied.

                                        calcPermissionLevel(crudPerms[(int)PermissionAction.Create], drPerms["create_permission"].ToString());
                                        calcPermissionLevel(crudPerms[(int)PermissionAction.Read], drPerms["read_permission"].ToString());
                                        calcPermissionLevel(crudPerms[(int)PermissionAction.Update], drPerms["update_permission"].ToString());
                                        calcPermissionLevel(crudPerms[(int)PermissionAction.Delete], drPerms["delete_permission"].ToString());

                                    } else {
                                        // this field value does NOT fall in the right range.
                                        // this means this permission is NOT applied.
                                        crudPerms = crudPermsBeforeFields;
//break;
                                    }
                                } else {
                                    // we're comparing against the parent's value.
                                    string parentTableName = drField["parent_table_name"].ToString();
                                    if (parentTableName == tableName.Replace("'", "")) {
                                        // this restriction is at the parent level, but it's the same as the current table. ignore.
                                    } else {
                                        string parentRestrictByTableFieldName = drField["parent_table_field_name"].ToString();
                                        string parentPrimaryKeyTableFieldName = drField["parent_pk_table_field_name"].ToString();
                                        string parentFieldType = drField["parent_field_type"].ToString();
                                        string parentCompareOperator = drField["parent_compare_operator"].ToString();
                                        string parentCompareValue = drField["parent_compare_value"].ToString().Replace("__CURRENTCOOPERATORID__", this.CooperatorID.ToString()).Replace("__CURRENTLANGUAGEID__", this.LanguageID.ToString());

                                        // first, grab the appropriate value from the parent table
                                        string parentSql = "select count(*) from " + parentTableName + " where " + parentRestrictByTableFieldName + " " + parentCompareOperator + " :value and " + parentPrimaryKeyTableFieldName + " = " + fieldValue;
                                        var dps = new DataParameters(":value", parentCompareValue);
                                        if (parentCompareOperator.ToLower() == "in") {
                                            if (parentFieldType.ToUpper().StartsWith("INT")) {
                                                if (parentCompareValue.ToLower().Contains("select")) {
                                                    // allows for putting in subselects (possible future enhancement of doing folder-level perms in CT)
                                                    dps[0].DbPseudoType = DbPseudoType.StringReplacement;
                                                } else {
                                                    // allows for specifying a series of integers
                                                    dps[0].DbPseudoType = DbPseudoType.IntegerCollection;
                                                }
                                            } else {
                                                // allows for specifying a series of strings (which must be properly formatted, of course)
                                                dps[0].DbPseudoType = DbPseudoType.StringCollection;
                                            }
                                            parentSql = parentSql.Replace(":value", "(:value)");
                                        }

                                        var count = Toolkit.ToInt32(dm.ReadValue(parentSql, dps), 0);

                                        if (count > 0) {
                                            // this field value does fall in the proper range.
                                            // this means that this permission can now be applied.

                                            calcPermissionLevel(crudPerms[(int)PermissionAction.Create], drPerms["create_permission"].ToString());
                                            calcPermissionLevel(crudPerms[(int)PermissionAction.Read], drPerms["read_permission"].ToString());
                                            calcPermissionLevel(crudPerms[(int)PermissionAction.Update], drPerms["update_permission"].ToString());
                                            calcPermissionLevel(crudPerms[(int)PermissionAction.Delete], drPerms["delete_permission"].ToString());
                                        } else {
                                            // this parent field value does NOT fall in the right range.
                                            // this means the permission is NOT applied.
                                            crudPerms = crudPermsBeforeFields;
//break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // all checks are done.
                // convert all Inherit privileges to Deny.
                // this means if someone is not explicitly given the rights, they are denied
                foreach (var perm in crudPerms) {
                    if (perm.Level == PermissionLevel.Inherit) {
                        perm.Level = PermissionLevel.Deny;
                    }
                }

                // cache for future use
                if (cacheable && cacheKey != null) {
                    cm[cacheKey] = crudPerms;
                }

                return crudPerms;

            } catch (Exception ex) {
                // if any errors occur, we just throw back a Deny for everything.  
                // options:
                // we could throw an exception here, but it could be accidentally eaten elsewhere.
                // Permissions could be ignored as well, but they're specifically asking for them.
                // which to do? rethrow or return Deny?
                Debug.WriteLine(ex.Message);
                //                return Permission.CRUDPermissions(PermissionLevel.Deny);

                throw;
            } finally {
                dm.Limit = origLimit;
            }
        }

        /// <summary>
        /// Returns true if the given parameters result in a comparison that is true, false otherwise.  Interprets "__CURRENTCOOPERATORID__" and "__CURRENTLANGUAGEID__" magic monikers correctly.  If comparison is undefined or cannot otherwise be executed, an exception is thrown.
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <param name="typeName"></param>
        /// <param name="compareBy"></param>
        /// <param name="compareValue"></param>
        /// <returns></returns>
        private bool compareValues(object fieldValue, string typeName, string compareBy, object compareValue) {


            if (compareValue != null && compareValue is string) {
                compareValue = (compareValue as string).Trim().ToUpper().Replace("__CURRENTCOOPERATORID__", this.CooperatorID.ToString()).Replace("__CURRENTLANGUAGEID__", this.LanguageID.ToString());
            }

            switch (typeName.ToLower()) {
                case "string":
                default:
                    switch (compareBy) {
                        case "<":
                        case "less than":
                        case "lt":
                            return String.Compare(fieldValue.ToString(), compareValue.ToString(), true) < 0;
                        case ">":
                        case "greater than":
                        case "gt":
                            return String.Compare(fieldValue.ToString(), compareValue.ToString(), true) > 0;
                        case "=":
                        case "==":
                        case "eq":
                        case "equal":
                        case "equals":
                            return String.Compare(fieldValue.ToString(), compareValue.ToString(), true) == 0;
                        case "!=":
                        case "<>":
                        case "neq":
                        case "notequal":
                        case "not equal":
                        case "notequals":
                        case "not equals":
                        case "notequalto":
                        case "not equal to":
                            return String.Compare(fieldValue.ToString(), compareValue.ToString(), true) != 0;
                        case "contains":
                        case "contains()":
                            return fieldValue.ToString().ToLower().Contains(compareValue.ToString().ToLower());
                        case "like":
                            Regex re = new Regex((compareValue.ToString()).Replace("%", ".*"), RegexOptions.IgnoreCase);
                            Match m = re.Match(fieldValue.ToString());
                            return m.Success;
                        case "!like":
                        case "not like":
                        case "dislike":
                            // assumes val2 contains % somewhere on which to wildcard
                            Regex re2 = new Regex((compareValue.ToString()).Replace("%", ".*"), RegexOptions.IgnoreCase);
                            Match m2 = re2.Match(fieldValue.ToString());
                            return !m2.Success;
                        case "in":
                            // do compase insensitive comparison of string list
                            var tgtVal = fieldValue.ToString();
                            var validValues = compareValue.ToString().SplitRetain(new char[] { ',', ' ', '\t', '\r', '\n' }, true, false, false);
                            foreach (var s in validValues) {
                                if (String.Compare(tgtVal, s, true) == 0) {
                                    return true;
                                }
                            }
                            return false;
                    }
                    break;
                case "date":
                case "datetime2":
                case "datetime":
                case "time":
                    switch (compareBy) {
                        case "<":
                        case "less than":
                        case "lt":
                            return Toolkit.ToDateTime(fieldValue) < Toolkit.ToDateTime(compareValue);
                        case ">":
                        case "greater than":
                        case "gt":
                            return Toolkit.ToDateTime(fieldValue) > Toolkit.ToDateTime(compareValue);
                        case "=":
                        case "==":
                        case "eq":
                        case "equal":
                        case "equals":
                            return Toolkit.ToDateTime(fieldValue) == Toolkit.ToDateTime(compareValue);
                        case "!=":
                        case "<>":
                        case "neq":
                        case "notequal":
                        case "not equal":
                        case "notequals":
                        case "not equals":
                        case "notequalto":
                        case "not equal to":
                            return Toolkit.ToDateTime(fieldValue) != Toolkit.ToDateTime(compareValue);
                    }
                    break;
                case "int":
                case "int32":
                case "integer":
                    switch (compareBy) {
                        case "<":
                        case "less than":
                        case "lt":
                            return Toolkit.ToInt32(fieldValue, int.MinValue) < Toolkit.ToInt32(compareValue, int.MinValue);
                        case ">":
                        case "greater than":
                        case "gt":
                            return Toolkit.ToInt32(fieldValue, int.MinValue) > Toolkit.ToInt32(compareValue, int.MinValue);
                        case "=":
                        case "==":
                        case "eq":
                        case "equal":
                        case "equals":
                            return Toolkit.ToInt32(fieldValue, int.MinValue) == Toolkit.ToInt32(compareValue, int.MinValue);
                        case "!=":
                        case "<>":
                        case "neq":
                        case "notequal":
                        case "not equal":
                        case "notequals":
                        case "not equals":
                        case "notequalto":
                        case "not equal to":
                            return Toolkit.ToInt32(fieldValue, int.MinValue) != Toolkit.ToInt32(compareValue, int.MinValue);
                        case "in":
                            var validValues = Toolkit.ToIntList(compareValue.ToString());
                            return validValues.Contains(Toolkit.ToInt32(fieldValue, int.MinValue));
                    }
                    break;
                case "long":
                case "int64":
                    switch (compareBy) {
                        case "<":
                        case "less than":
                        case "lt":
                            return Toolkit.ToInt64(fieldValue, long.MinValue) < Toolkit.ToInt64(compareValue, long.MinValue);
                        case ">":
                        case "greater than":
                        case "gt":
                            return Toolkit.ToInt64(fieldValue, long.MinValue) > Toolkit.ToInt64(compareValue, long.MinValue);
                        case "=":
                        case "==":
                        case "eq":
                        case "equal":
                        case "equals":
                            return Toolkit.ToInt64(fieldValue, long.MinValue) == Toolkit.ToInt64(compareValue, long.MinValue);
                        case "!=":
                        case "<>":
                        case "neq":
                        case "notequal":
                        case "not equal":
                        case "notequals":
                        case "not equals":
                        case "notequalto":
                        case "not equal to":
                            return Toolkit.ToInt64(fieldValue, long.MinValue) != Toolkit.ToInt64(compareValue, long.MinValue);
                        case "in":
                            var validValues = Toolkit.ToLongList(compareValue.ToString());
                            return validValues.Contains(Toolkit.ToInt64(fieldValue, long.MinValue));
                    }
                    break;
                case "float":
                    switch (compareBy) {
                        case "<":
                        case "less than":
                        case "lt":
                            return Toolkit.ToFloat(fieldValue, float.MinValue) < Toolkit.ToFloat(compareValue, float.MinValue);
                        case ">":
                        case "greater than":
                        case "gt":
                            return Toolkit.ToFloat(fieldValue, float.MinValue) > Toolkit.ToFloat(compareValue, float.MinValue);
                        case "=":
                        case "==":
                        case "eq":
                        case "equal":
                        case "equals":
                            return Toolkit.ToFloat(fieldValue, float.MinValue) == Toolkit.ToFloat(compareValue, float.MinValue);
                        case "!=":
                        case "<>":
                        case "neq":
                        case "notequal":
                        case "not equal":
                        case "notequals":
                        case "not equals":
                        case "notequalto":
                        case "not equal to":
                            return Toolkit.ToFloat(fieldValue, float.MinValue) != Toolkit.ToFloat(compareValue, float.MinValue);
                    }
                    break;
                case "decimal":
                    switch (compareBy) {
                        case "<":
                        case "less than":
                        case "lt":
                            return Toolkit.ToDecimal(fieldValue, decimal.MinValue) < Toolkit.ToDecimal(compareValue, decimal.MinValue);
                        case ">":
                        case "greater than":
                        case "gt":
                            return Toolkit.ToDecimal(fieldValue, decimal.MinValue) > Toolkit.ToDecimal(compareValue, decimal.MinValue);
                        case "=":
                        case "==":
                        case "eq":
                        case "equal":
                        case "equals":
                            return Toolkit.ToDecimal(fieldValue, decimal.MinValue) == Toolkit.ToDecimal(compareValue, decimal.MinValue);
                        case "!=":
                        case "<>":
                        case "neq":
                        case "notequal":
                        case "not equal":
                        case "notequals":
                        case "not equals":
                        case "notequalto":
                        case "not equal to":
                            return Toolkit.ToDecimal(fieldValue, decimal.MinValue) != Toolkit.ToDecimal(compareValue, decimal.MinValue);
                    }
                    break;
                case "double":
                    switch (compareBy) {
                        case "<":
                        case "less than":
                        case "lt":
                            return Toolkit.ToDouble(fieldValue, double.MinValue) < Toolkit.ToDouble(compareValue, double.MinValue);
                        case ">":
                        case "greater than":
                        case "gt":
                            return Toolkit.ToDouble(fieldValue, double.MinValue) > Toolkit.ToDouble(compareValue, double.MinValue);
                        case "=":
                        case "==":
                        case "eq":
                        case "equal":
                        case "equals":
                            return Toolkit.ToDouble(fieldValue, double.MinValue) == Toolkit.ToDouble(compareValue, double.MinValue);
                        case "!=":
                        case "<>":
                        case "neq":
                        case "notequal":
                        case "not equal":
                        case "notequals":
                        case "not equals":
                        case "notequalto":
                        case "not equal to":
                            return Toolkit.ToDouble(fieldValue, double.MinValue) != Toolkit.ToDouble(compareValue, double.MinValue);
                    }
                    break;
            }

            // we get here, the operator is not supported for the given type.
            throw Library.CreateBusinessException(getDisplayMember("compareValues{unsupported}", "Comparing values of type {0} with operator {1} is not supported.", typeName, compareBy));

        }



        #endregion Permissions

        #region Save Data

        #region Trigger Processing

        private delegate void DataViewTriggerDelegate(IDataviewSaveDataTrigger trigger, ISaveDataTriggerArgs args, int saveCount, int failedCount, TriggerMode mode);
        /// <summary>
        /// Processes dataview triggers that are async.  Currently none are configured as asynchronous, and this functionality should be deprecated.
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="args"></param>
        /// <param name="saveCount"></param>
        /// <param name="failedCount"></param>
        /// <param name="mode"></param>
        private void ProcessDataViewTriggerAsync(IDataviewSaveDataTrigger trigger, ISaveDataTriggerArgs args, int saveCount, int failedCount, TriggerMode mode) {
            switch (mode) {
                case TriggerMode.Saving:
                    trigger.DataViewSaving(args);
                    break;
                case TriggerMode.RowSaving:
                    trigger.DataViewRowSaving(args);
                    break;
                case TriggerMode.RowSaveFailed:
                    trigger.DataViewRowSaveFailed(args);
                    break;
                case TriggerMode.RowSaved:
                    trigger.DataViewRowSaved(args);
                    break;
                case TriggerMode.Saved:
                    trigger.DataViewSaved(args, saveCount, failedCount);
                    break;
                default:
                    throw Library.CreateBusinessException(getDisplayMember("ProcessDataView{default}", "mode not specified properly in call to ProcessDataViewTriggerAsync"));
            }
        }
        private delegate void TableTriggerDelegate(ITableSaveDataTrigger trigger, ISaveDataTriggerArgs args, int saveCount, int failedCount, TriggerMode mode);
        /// <summary>
        /// Processes table triggers that are async.  Currently none are configured as asynchronous, and this functionality should be deprecated.
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="args"></param>
        /// <param name="saveCount"></param>
        /// <param name="failedCount"></param>
        /// <param name="mode"></param>
        private void ProcessTableTriggerAsync(ITableSaveDataTrigger trigger, ISaveDataTriggerArgs args, int saveCount, int failedCount, TriggerMode mode) {
            switch (mode) {
                case TriggerMode.Saving:
                    trigger.TableSaving(args);
                    break;
                case TriggerMode.RowSaving:
                    trigger.TableRowSaving(args);
                    break;
                case TriggerMode.RowSaved:
                    trigger.TableRowSaved(args);
                    break;
                case TriggerMode.Saved:
                    trigger.TableSaved(args, saveCount, failedCount);
                    break;
                default:
                    throw Library.CreateBusinessException(getDisplayMember("ProcessTableTriggerAsync", "mode not specified properly in call to ProcessTableTriggerAsync"));
            }
        }

        /// <summary>
        /// Processes the "Saving" mode for a trigger at the dataview level.  Called once per Dataview definition.
        /// </summary>
        /// <param name="fmdv"></param>
        /// <param name="dt"></param>
        /// <param name="dm"></param>
        /// <param name="dsReturn"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private List<ISaveDataTriggerArgs> dataviewTriggerDataViewSaving(IDataview fmdv, DataTable dt, DataManager dm, DataSet dsReturn, SaveOptions options) {

            // apply dataview-level pre-table save triggers
            List<ISaveDataTriggerArgs> dvTriggerArgs = new List<ISaveDataTriggerArgs>();
            if (fmdv != null) {
                for (int i = 0; i < fmdv.SaveDataTriggers.Count; i++) {
                    IDataviewSaveDataTrigger dvTrigger = fmdv.SaveDataTriggers[i];
                    var args = new SaveDataTriggerArgs {
                        SecureData = this,
                        DataManager = dm,
                        Dataview = fmdv,
                        Table = null,
                        SaveMode = SaveMode.Unknown,
                        DataTable = dt,
                        Helper = new DataTriggerHelper(null, null, null),
                        SaveOptions = options.Clone()
                    };
                    dvTriggerArgs.Add(args);
                    try {
                        if (!dvTrigger.IsAsynchronous) {
                            dvTrigger.DataViewSaving(args);
                        } else {
                            DataViewTriggerDelegate rfd = new DataViewTriggerDelegate(ProcessDataViewTriggerAsync);
                            rfd.BeginInvoke(dvTrigger, args.Clone(), 0, 0, TriggerMode.Saving, null, null);
                        }
                    } catch (Exception ex) {
                        LogException(ex, dsReturn);
                        if (!args.IsCancelled) {
                            args.Cancel(ex.Message);
                        }
                    }
                    if (args.IsCancelled) {
                        // cancel all saves
                        throw Library.CreateBusinessException(args.CancelReason);
                    }
                }
            }
            return dvTriggerArgs;
        }

        /// <summary>
        /// Processes the "Saving" mode for a trigger at the table level.  Called once per Table definition.
        /// </summary>
        /// <param name="fmdv"></param>
        /// <param name="dt"></param>
        /// <param name="dm"></param>
        /// <param name="dsReturn"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private List<List<ISaveDataTriggerArgs>> tableTriggerTableSaving(IDataview fmdv, DataTable dt, DataManager dm, DataSet dsReturn, SaveOptions options) {

            // apply all table-level pre-table save triggers
            List<List<ISaveDataTriggerArgs>> allTableTriggerArgs = new List<List<ISaveDataTriggerArgs>>();
            if (fmdv == null || fmdv.Tables.Count == 0) {

                // this dataview represents exactly one table (i.e. auto-generated dataview)

                var fmt = Table.Map(dt.TableName, dm, LanguageID, false) as ITable;
                if (fmt == null) {
                    throw Library.CreateBusinessException(getDisplayMember("tableTriggerTableSaving", "Nothing defined in table mappings (sys_table, sys_table_field) for table name = '{0}'", dt.TableName));
                } else {
                    //fmdv.Tables.Add(fmt);
                    List<ISaveDataTriggerArgs> tableArgs = new List<ISaveDataTriggerArgs>();
                    allTableTriggerArgs.Add(tableArgs);
                    for (int i = 0; i < fmt.SaveDataTriggers.Count; i++) {
                        ITableSaveDataTrigger trigger = fmt.SaveDataTriggers[i];
                        var args = new SaveDataTriggerArgs {
                            SecureData = this,
                            DataManager = dm,
                            Dataview = fmdv,
                            Table = fmt,
                            SaveMode = SaveMode.Unknown,
                            DataTable = dt,
                            Helper = new DataTriggerHelper(null, null, fmt.AliasName),
                            SaveOptions = options.Clone()
                        };
                        tableArgs.Add(args);
                        try {
                            if (!trigger.IsAsynchronous) {
                                trigger.TableSaving(args);
                            } else {
                                TableTriggerDelegate tfd = new TableTriggerDelegate(ProcessTableTriggerAsync);
                                tfd.BeginInvoke(trigger, args.Clone(), 0, 0, TriggerMode.Saving, null, null);
                            }
                        } catch (Exception ex) {
                            LogException(ex, dsReturn);
                            if (!args.IsCancelled) {
                                args.Cancel(ex.Message);
                            }
                        }
                        if (args.IsCancelled) {
                            // cancel all remaining saves
                            throw Library.CreateBusinessException(args.CancelReason);
                        }
                    }

                }
            } else {

                // this dataview represents at least 1 pre-mapped table, possibly more.
                // To make sure we generate arguments for all the parent-child tables
                // as well as the many-to-many *_map tables, we concatenate them into one big list
                // then create an argument object for each one

                var allTables = fmdv.Tables.ToList();

                //foreach (IFieldMappingTable fmt in fmdv.Tables) {
                foreach (ITable fmt in allTables) {
                    List<ISaveDataTriggerArgs> tableArgs = new List<ISaveDataTriggerArgs>();
                    allTableTriggerArgs.Add(tableArgs);

                    for (int i = 0; i < fmt.SaveDataTriggers.Count; i++) {
                        ITableSaveDataTrigger trigger = fmt.SaveDataTriggers[i];
                        var args = new SaveDataTriggerArgs {
                            SecureData = this,
                            DataManager = dm,
                            Dataview = fmdv,
                            Table = fmt,
                            SaveMode = SaveMode.Unknown,
                            DataTable = dt,
                            Helper = new DataTriggerHelper(null, null, fmt.AliasName),
                            SaveOptions = options.Clone()
                        };
                        tableArgs.Add(args);
                        try {
                            if (!trigger.IsAsynchronous) {
                                trigger.TableSaving(args);
                            } else {
                                TableTriggerDelegate tfd = new TableTriggerDelegate(ProcessTableTriggerAsync);
                                tfd.BeginInvoke(trigger, args.Clone(), 0, 0, TriggerMode.Saving, null, null);
                            }
                        } catch (Exception ex) {
                            LogException(ex, dsReturn);
                            if (!args.IsCancelled) {
                                args.Cancel(ex.Message);
                            }
                        }
                        if (args.IsCancelled) {
                            // cancel all saves
                            throw Library.CreateBusinessException(args.CancelReason);
                        }
                    }
                }
            }
            return allTableTriggerArgs;
        }

        /// <summary>
        /// Processes the "RowSaving" mode for a trigger at the dataview level.  Called once for each row in the DataTable object.
        /// </summary>
        /// <param name="fmdv"></param>
        /// <param name="dvTriggerArgs"></param>
        /// <param name="saveMode"></param>
        /// <param name="dr"></param>
        /// <param name="dsReturn"></param>
        private void dataviewTriggerRowSaving(IDataview fmdv, List<ISaveDataTriggerArgs> dvTriggerArgs, SaveMode saveMode, DataRow dr, DataSet dsReturn) {
            // apply dataview-level pre-row save trigger(s)
            if (fmdv != null) {
                for (int i = 0; i < fmdv.SaveDataTriggers.Count; i++) {
                    IDataviewSaveDataTrigger dvTrigger = fmdv.SaveDataTriggers[i];
                    var args = dvTriggerArgs[i];
                    args.Uncancel();
                    args.SaveMode = saveMode;
                    args.RowToSave = dr;
                    args.Helper.SavingRow = dr;
                    args.Helper.DatabaseRow = null;
                    try {
                        if (!dvTrigger.IsAsynchronous) {
                            dvTrigger.DataViewRowSaving(args);
                        } else {
                            DataViewTriggerDelegate rfd = new DataViewTriggerDelegate(ProcessDataViewTriggerAsync);
                            rfd.BeginInvoke(dvTrigger, args.Clone(), 0, 0, TriggerMode.RowSaving, null, null);
                        }
                    } catch (Exception ex) {
                        LogException(ex, dsReturn);
                        if (!args.IsCancelled) {
                            args.Cancel(ex.Message);
                        }
                    }
                    if (args.IsCancelled) {
                        // cancel all saves for this row
                        throw Library.CreateBusinessException(args.CancelReason);
                    }
                }
            }
        }

        /// <summary>
        /// Processes the "RowSaving" mode for a trigger at the table level.  Called once for each row in the DataTable object AND each Table object defined within the dataview.
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="tableTriggerArgs"></param>
        /// <param name="dr"></param>
        /// <param name="dm"></param>
        /// <param name="saveMode"></param>
        /// <param name="dsReturn"></param>
        private void tableTriggerRowSaving(ITable fmt, List<ISaveDataTriggerArgs> tableTriggerArgs, DataRow dr, DataManager dm, SaveMode saveMode, DataSet dsReturn) {

            // apply table-level pre-row save trigger(s)
            // (remember the args list for the post-save trigger(s) since we might pull data from the db)
            if (fmt != null) {

                DataRow rowInDatabase = null;

                if (saveMode != SaveMode.Insert) {
                    if (fmt.SaveDataTriggers.Count > 0) {
                        DataCommand select = createSelectCommand(fmt, dr, dm);
                        if (select != null) {
                            DataTable dtCurrent = dm.Read(select);
                            if (dtCurrent.Rows.Count > 0) {
                                rowInDatabase = dtCurrent.Rows[0];
                            }
                        }
                    }
                }

                for (int i = 0; i < fmt.SaveDataTriggers.Count; i++) {

                    var args = tableTriggerArgs[i];
                    args.Uncancel();
                    args.SaveMode = saveMode;
                    args.RowToSave = dr;

                    args.OriginalPrimaryKeyID = 0;
                    args.NewPrimaryKeyID = 0;
                    if (!String.IsNullOrEmpty(fmt.PrimaryKeyDataViewFieldName)) {
                        if (saveMode == SaveMode.Delete) {
                            args.OriginalPrimaryKeyID = Toolkit.ToInt32(dr[fmt.PrimaryKeyDataViewFieldName, DataRowVersion.Original], -1);
                            args.NewPrimaryKeyID = 0;
                        } else if (saveMode == SaveMode.Update) {
                            args.OriginalPrimaryKeyID = Toolkit.ToInt32(dr[fmt.PrimaryKeyDataViewFieldName, DataRowVersion.Original], -1);
                            args.NewPrimaryKeyID = Toolkit.ToInt32(dr[fmt.PrimaryKeyDataViewFieldName, DataRowVersion.Current], -1);
                        }
                    }

                    args.RowInDatabase = rowInDatabase;


                    args.Helper.SavingRow = args.RowToSave;
                    args.Helper.DatabaseRow = args.RowInDatabase;

                    ITableSaveDataTrigger trigger = fmt.SaveDataTriggers[i];
                    //try {
                    if (!trigger.IsAsynchronous) {
                        trigger.TableRowSaving(args);
                    } else {
                        TableTriggerDelegate tfd = new TableTriggerDelegate(ProcessTableTriggerAsync);
                        tfd.BeginInvoke(trigger, args.Clone(), 0, 0, TriggerMode.RowSaving, null, null);
                    }
                    //} catch (Exception ex) {
                    //    LogException(ex, dsReturn);
                    //    if (!args.IsCancelled) {
                    //        args.Cancel(ex.Message, CancelAction.Stop);
                    //    }
                    //}
                    if (args.IsCancelled) {
                        // TODO: do we want to continue processing subsequent triggers
                        // if current trigger cancelled it?
                        throw Library.CreateBusinessException(args.CancelReason);
                    }
                }
            }
        }


        /// <summary>
        /// Processes the "RowSaved" mode for a trigger at the table level.  Called once for each row in the DataTable object AND each Table object defined within the dataview.
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="tableTriggerArgs"></param>
        /// <param name="dr"></param>
        /// <param name="dm"></param>
        /// <param name="saveMode"></param>
        /// <param name="newIDOrAffectedCount"></param>
        /// <param name="dsReturn"></param>
        private void tableTriggerAfterRowSave(ITable fmt, List<ISaveDataTriggerArgs> tableTriggerArgs, DataRow dr, DataManager dm, SaveMode saveMode, int newIDOrAffectedCount, DataSet dsReturn) {
            // apply table-level post-save row trigger(s)
            // notice we do this whether the save worked or not!

            //for (int i = fmt.SaveDataTriggers.Count - 1; i > -1; i--) {
            // KFE process in forward order rather than reverse
            for (int i = 0; i < fmt.SaveDataTriggers.Count; i++) {
                var args = tableTriggerArgs[i];

                // remember the new pk value if needed
                if (saveMode == SaveMode.Insert) {
                    args.NewPrimaryKeyID = newIDOrAffectedCount;
                }

                bool successfulSave = newIDOrAffectedCount != 0;

                //                args.Uncancel();
                ITableSaveDataTrigger trigger = fmt.SaveDataTriggers[i];
                // try {
                if (!trigger.IsAsynchronous) {
                    if (successfulSave) {
                        trigger.TableRowSaved(args);
                    } else {
                        trigger.TableRowSaveFailed(args);
                    }
                } else {
                    TableTriggerDelegate tfd = new TableTriggerDelegate(ProcessTableTriggerAsync);
                    tfd.BeginInvoke(trigger, args.Clone(), 0, 0, (successfulSave ? TriggerMode.RowSaved : TriggerMode.RowSaveFailed), null, null);
                }
                //} catch (Exception ex) {
                //    LogException(ex, dsReturn);
                //    if (!args.IsCancelled) {
                //        args.Cancel(ex.Message);
                //    }
                //}
                if (args.IsCancelled) {
                    // we don't throw an error here -- we handle that in doSave()
                    break;
                }
            }

        }

        /// <summary>
        /// Processes the "RowSaved" mode for a trigger at the dataview level.  Called once for each row in the DataTable object.
        /// </summary>
        /// <param name="fmdv"></param>
        /// <param name="successfulSave"></param>
        /// <param name="dvTriggerArgs"></param>
        /// <param name="dsReturn"></param>
        private void dataviewTriggerAfterRowSave(IDataview fmdv, bool? successfulSave, List<ISaveDataTriggerArgs> dvTriggerArgs, DataSet dsReturn) {
            // apply dataview-level post-save row trigger(s)
            if (fmdv != null) {
                for (int i = fmdv.SaveDataTriggers.Count - 1; i > -1; i--) {
                    IDataviewSaveDataTrigger dvTrigger = fmdv.SaveDataTriggers[i];
                    var args = dvTriggerArgs[i];
                    //                args.Uncancel();
                    try {
                        if (!dvTrigger.IsAsynchronous) {
                            dvTrigger.DataViewRowSaved(args);
                        } else {
                            DataViewTriggerDelegate rfd = new DataViewTriggerDelegate(ProcessDataViewTriggerAsync);
                            rfd.BeginInvoke(dvTrigger, args.Clone(), 0, 0, (successfulSave == true ? TriggerMode.RowSaved : TriggerMode.RowSaveFailed), null, null);
                        }
                    } catch (Exception ex) {
                        LogException(ex, dsReturn);
                        if (!args.IsCancelled) {
                            args.Cancel(ex.Message);
                        }
                    }
                    if (args.IsCancelled) {
                        // cancel all subsequent triggers on this row. the catch in saveData() will let other rows continue to process.
                        throw Library.CreateBusinessException(args.CancelReason);
                    }
                }
            }
        }

        /// <summary>
        /// Processes the "Saved" mode for a trigger at the table level.  Called once for each Table object defined on the dataview.
        /// </summary>
        /// <param name="fmdv"></param>
        /// <param name="allTableTriggerArgs"></param>
        /// <param name="savedCount"></param>
        /// <param name="failedCount"></param>
        /// <param name="dsReturn"></param>
        private void tableTriggerTableSaved(IDataview fmdv, List<List<ISaveDataTriggerArgs>> allTableTriggerArgs, int savedCount, int failedCount, DataSet dsReturn) {
            // apply table-level post-save table trigger(s) (in reverse order since we're essentially bubbling up the stack)
            if (fmdv != null) {
                for (int i = fmdv.Tables.Count - 1; i > -1; i--) {
                    var fmt = fmdv.Tables[i];
                    var singleTableTriggerArgs = allTableTriggerArgs[i];

                    for (int j = fmt.SaveDataTriggers.Count - 1; j > -1; j--) {
                        ITableSaveDataTrigger trigger = fmt.SaveDataTriggers[j];
                        var args = singleTableTriggerArgs[j];
                        args.SaveMode = SaveMode.Unknown;
                        args.RowInDatabase = null;
                        args.RowToSave = null;
                        args.Helper.SavingRow = null;
                        args.Helper.DatabaseRow = null;

                        //                    args.Uncancel();

                        try {
                            if (!trigger.IsAsynchronous) {
                                trigger.TableSaved(args, savedCount, failedCount);
                            } else {
                                TableTriggerDelegate tfd = new TableTriggerDelegate(ProcessTableTriggerAsync);
                                tfd.BeginInvoke(trigger, args.Clone(), savedCount, failedCount, TriggerMode.Saved, null, null);
                            }
                        } catch (Exception ex) {
                            LogException(ex, dsReturn);
                            if (!args.IsCancelled) {
                                args.Cancel(ex.Message);
                            }
                        }
                        if (args.IsCancelled) {
                            throw Library.CreateBusinessException(args.CancelReason);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Processes the "Saved" mode for a trigger at the dataview level.  Called once for each dataview.
        /// </summary>
        /// <param name="fmdv"></param>
        /// <param name="dvTriggerArgs"></param>
        /// <param name="savedCount"></param>
        /// <param name="failedCount"></param>
        /// <param name="dsReturn"></param>
        private void dataviewTriggerDataViewSaved(IDataview fmdv, List<ISaveDataTriggerArgs> dvTriggerArgs, int savedCount, int failedCount, DataSet dsReturn) {
            // apply dataview-level post-table save triggers (in reverse order since we're essentially bubbling up the stack)
            if (fmdv != null) {
                for (int i = fmdv.SaveDataTriggers.Count - 1; i > -1; i--) {
                    IDataviewSaveDataTrigger dvTrigger = fmdv.SaveDataTriggers[i];
                    var args = dvTriggerArgs[i];
                    args.RowToSave = null;
                    args.RowInDatabase = null;
                    args.SaveMode = SaveMode.Unknown;
                    args.Helper.SavingRow = null;
                    args.Helper.DatabaseRow = null;
                    args.Helper.AliasName = null;
                    //                args.Uncancel();
                    try {
                        if (!dvTrigger.IsAsynchronous) {
                            dvTrigger.DataViewSaved(args, savedCount, failedCount);
                        } else {
                            DataViewTriggerDelegate rfd = new DataViewTriggerDelegate(ProcessDataViewTriggerAsync);
                            rfd.BeginInvoke(dvTrigger, args.Clone(), savedCount, failedCount, TriggerMode.Saved, null, null);
                        }
                    } catch (Exception ex) {
                        LogException(ex, dsReturn);
                        if (!args.IsCancelled) {
                            args.Cancel(ex.Message);
                        }
                    }
                    if (args.IsCancelled) {
                        // cancel all saves
                        throw Library.CreateBusinessException(args.CancelReason);
                    }
                }
            }
        }

        #endregion Trigger Processing

        #region Save Response Processing
        /// <summary>
        /// Creates a schema copy of the given DataTable and adds it to the given DataSet if needed.  Appends DataColumns (ExceptionIndex, ExceptionMessage, NewPrimaryKeyID, OriginalPrimaryKeyID, SavedAction, SavedStatus, OriginalRowIndex, TableName, AliasName) as needed so the caller knows what happened to each row and why.
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="dsReturn"></param>
        /// <returns></returns>
        private DataTable cloneTableForReturn(DataTable dtSource, DataSet dsReturn) {

            if (dsReturn.Tables.Contains(dtSource.TableName)) {
                return dsReturn.Tables[dtSource.TableName];
            } else {

                // if something fails, we create a clone table to return
                // the data in question so the caller can decide what to do
                DataTable dtClone = dtSource.Clone();
                // add on fields to enable mapping to exception table
                if (!dtClone.Columns.Contains("ExceptionIndex")) {
                    DataColumn ei = new DataColumn("ExceptionIndex", typeof(int));
                    ei.AllowDBNull = true;
                    dtClone.Columns.Add(ei);
                }
                if (!dtClone.Columns.Contains("ExceptionMessage")) {
                    DataColumn em = new DataColumn("ExceptionMessage", typeof(string));
                    em.AllowDBNull = true;
                    dtClone.Columns.Add(em);
                }
                if (!dtClone.Columns.Contains("NewPrimaryKeyID")) {
                    DataColumn npk = new DataColumn("NewPrimaryKeyID", typeof(int));
                    npk.AllowDBNull = true;
                    dtClone.Columns.Add(npk);
                }
                if (!dtClone.Columns.Contains("OriginalPrimaryKeyID")) {
                    DataColumn npk = new DataColumn("OriginalPrimaryKeyID", typeof(int));
                    npk.AllowDBNull = true;
                    dtClone.Columns.Add(npk);
                }
                if (!dtClone.Columns.Contains("SavedAction")) {
                    DataColumn sa = new DataColumn("SavedAction", typeof(string));
                    sa.AllowDBNull = true;
                    dtClone.Columns.Add(sa);
                }
                if (!dtClone.Columns.Contains("SavedStatus")) {
                    DataColumn stat = new DataColumn("SavedStatus", typeof(string));
                    stat.AllowDBNull = false;
                    dtClone.Columns.Add(stat);
                }
                if (!dtClone.Columns.Contains("OriginalRowIndex")) {
                    DataColumn npk = new DataColumn("OriginalRowIndex", typeof(int));
                    npk.AllowDBNull = true;
                    dtClone.Columns.Add(npk);
                }
                if (!dtClone.Columns.Contains("TableName")) {
                    DataColumn stat = new DataColumn("TableName", typeof(string));
                    stat.AllowDBNull = true;
                    dtClone.Columns.Add(stat);
                }
                if (!dtClone.Columns.Contains("AliasName")) {
                    DataColumn stat = new DataColumn("AliasName", typeof(string));
                    stat.AllowDBNull = true;
                    dtClone.Columns.Add(stat);
                }
                dsReturn.Tables.Add(dtClone);

                return dtClone;

            }


        }

        /// <summary>
        /// Copies given DataRow object to the given dtClone table as needed so caller knows that row saved successfully.
        /// </summary>
        /// <param name="primaryKeyFieldName"></param>
        /// <param name="dtClone"></param>
        /// <param name="dr"></param>
        /// <param name="saveMode"></param>
        /// <param name="newID"></param>
        /// <param name="dsReturn"></param>
        /// <param name="originalRowIndex"></param>
        /// <param name="tableName"></param>
        /// <param name="aliasName"></param>
        /// <param name="saveOptions"></param>
        private void copySavedRowToReturnTable(string primaryKeyFieldName, DataTable dtClone, DataRow dr, SaveMode saveMode, int newID, DataSet dsReturn, int originalRowIndex, string tableName, string aliasName, SaveOptions saveOptions) {

            if (saveOptions.UseUniqueKeys && dr.Table.Rows.Count > 50000) {
                // don't queue up successful rows if it's a huge request.
                // return;
            }

            DataRow drClone = dtClone.NewRow();


            DataRowVersion drv = DataRowVersion.Current;
            if (saveMode == SaveMode.Delete) {
                // delete doesn't give us access to the 'current' values
                // so just spit back originals (since we deleted them anyway, it doesn't matter)
                drv = DataRowVersion.Original;
            }

            for (int i = 0; i < dr.Table.Columns.Count; i++) {
                var col = dr.Table.Columns[i].ColumnName;
                if (drClone.Table.Columns.Contains(col)) {
                    drClone[col] = dr[col, drv];
                }
            }

            drClone["ExceptionIndex"] = -1;
            drClone["ExceptionMessage"] = DBNull.Value;

            switch (saveMode) {
                case SaveMode.Insert:
                    // remember the new pk so they can map it on their end
                    drClone["NewPrimaryKeyID"] = newID;
                    if (String.IsNullOrEmpty(primaryKeyFieldName)) {
                        drClone["OriginalPrimaryKeyID"] = DBNull.Value;
                    } else {
                        drClone["OriginalPrimaryKeyID"] = dr[primaryKeyFieldName, drv];
                    }
                    break;
                case SaveMode.Update:
                    // copy across pk from original field to our NewPrimaryKeyID field (for consistency)
                    if (String.IsNullOrEmpty(primaryKeyFieldName)) {
                        // no primary key found. 
                        drClone["NewPrimaryKeyID"] = DBNull.Value;
                        drClone["OriginalPrimaryKeyID"] = DBNull.Value;
                    } else {
                        // map original to new as it was an update
                        drClone["NewPrimaryKeyID"] = dr[primaryKeyFieldName, drv];
                        drClone["OriginalPrimaryKeyID"] = dr[primaryKeyFieldName, drv];
                    }
                    break;
                case SaveMode.Delete:
                    // do nothing special
                    drClone["NewPrimaryKeyID"] = DBNull.Value;
                    drClone["OriginalPrimaryKeyID"] = dr[primaryKeyFieldName, drv];
                    break;
                default:
                    // should never get here thanks to the cmd != null check above
                    throw Library.CreateBusinessException(getDisplayMember("copySavedRowToReturnTable{default}", "SecureData.copyRowToClone() hit an invalid case.  Please change the code."));
            }

            drClone["SavedAction"] = saveMode.ToString();
            drClone["SavedStatus"] = "Success";
            drClone["OriginalRowIndex"] = originalRowIndex;
            drClone["TableName"] = tableName;
            drClone["AliasName"] = aliasName;
            dtClone.Rows.Add(drClone);
            drClone.AcceptChanges();
        }

        /// <summary>
        /// Copies given DataRow object to the given dtClone table as needed so caller knows that the row failed to be saved and why.
        /// </summary>
        /// <param name="primaryKeyFieldName"></param>
        /// <param name="dtClone"></param>
        /// <param name="dr"></param>
        /// <param name="ex"></param>
        /// <param name="saveMode"></param>
        /// <param name="dsReturn"></param>
        /// <param name="originalRowIndex"></param>
        /// <param name="tableName"></param>
        /// <param name="aliasName"></param>
        private void copyFailedRowToReturnTable(string primaryKeyFieldName, DataTable dtClone, DataRow dr, Exception ex, SaveMode saveMode, DataSet dsReturn, int originalRowIndex, string tableName, string aliasName) {

            if (alreadyLoggedException(ex)) {
                return;
            }

            int exceptionIndex = AddExceptionToTable(ex, true, dsReturn);

            DataRowVersion drv = DataRowVersion.Current;

            if (saveMode == SaveMode.Delete) {
                drv = DataRowVersion.Original;
            }

            DataRow drClone = dtClone.NewRow();
            for (int i = 0; i < dr.Table.Columns.Count; i++) {
                var col = dr.Table.Columns[i].ColumnName;
                if (drClone.Table.Columns.Contains(col)) {
                    drClone[col] = dr[col, drv];
                }
            }
            drClone["ExceptionIndex"] = exceptionIndex;
            drClone["ExceptionMessage"] = ex.Message;
            if (String.IsNullOrEmpty(primaryKeyFieldName)) {
                drClone["OriginalPrimaryKeyID"] = DBNull.Value;
                drClone["NewPrimaryKeyID"] = DBNull.Value;
            } else {
                drClone["OriginalPrimaryKeyID"] = dr[primaryKeyFieldName, drv];
                drClone["NewPrimaryKeyID"] = dr[primaryKeyFieldName, drv];
            }
            drClone["SavedAction"] = saveMode.ToString();
            drClone["SavedStatus"] = "Failure";
            drClone["OriginalRowIndex"] = originalRowIndex;
            drClone["TableName"] = tableName;
            drClone["AliasName"] = aliasName;
            dtClone.Rows.Add(drClone);
            drClone.AcceptChanges();

        }
        #endregion Save Response Processing

        #region Save Request Processing


        /// <summary>
        /// Writes all changed / added / deleted rows to the respective sql tables.  if safeUpdatesAndDeletes is false, only the PK is used in the where clause for an update or delete. Otherwise, all changed fields are part of the where clause for updates and created/modified fields are used for deletes (in addition to the pk, of course)
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="safeUpdates"></param>
        /// <returns></returns>
        public DataSet SaveData(DataSet ds, bool safeUpdatesAndDeletes, string options, BackgroundWorker worker) {
            return saveData(ds, safeUpdatesAndDeletes, null, null, options, worker);
        }


        /// <summary>
        /// Writes all changed / added / deleted rows to the respective sql tables.  if safeUpdatesAndDeletes is false, only the PK is used in the where clause for an update or delete. Otherwise, all changed fields are part of the where clause for updates and created/modified fields are used for deletes (in addition to the pk, of course)
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="safeUpdates"></param>
        /// <returns></returns>
        public DataSet SaveData(DataSet ds, bool safeUpdatesAndDeletes, string options) {
            return saveData(ds, safeUpdatesAndDeletes, null, null, options, null);
        }

        private DataSet saveData(DataSet ds, bool safeUpdatesAndDeletes, DataSet dsReturn, DataManager dm, string options, BackgroundWorker worker) {



            // *******************************************************************************************************************
            // Warning!
            // *******************************************************************************************************************
            // This method is the entry point to one of the most complex parts of GRIN-Global.  
            // This is where the 'magic' for saving data via nothing more than a dataview definition takes place.
            // It performs the following (either directly or indirectly via a method it calls):
            //
            //  1) Loads the proper dataview definition(s) based on dataview and table mappings records
            //  2) Initializes and calls out to associated data triggers as needed
            //  3) Determines which table(s) need to be written to
            //  4) Determines which row(s) need to be changed and how
            //  5) Generates and executes the appropriate SQL statement(s) for each row (as each row may update multiple tables)
            //  6) Handles packaging up results of row saves to the return DataSet
            //
            // Please take care when editing this method, as there are quite a few edge cases due to the large number of ways
            // data ultimately needs to be saved (think of multiplexing insert/update/delete across the various relationships of
            // single table, parent-child, many-to-many, multiple levels of parent-child, etc)
            // 
            // Having said that, I've tried to keep it as straightforward as possible and comment things clearly where it may
            // be questionable as to why things are handled the way they are.
            //
            // *******************************************************************************************************************









            // set up the options provided

            // Import Wizard changes the processing behavior of the middle tier by opting-in to certain functionality, and that is represented by saveOptions.
            var saveOptions = new SaveOptions(options);

            // only used by Import Wizard, allows us to display progress as things are being processed.
            saveOptions.BackgroundWorker = worker;

            if (saveOptions.OwnerID < 0 || !IsAdministrator()) {
                // only Admins get the ability to set owner id through saveData
                saveOptions.OwnerID = this.CooperatorID;
            }

            // By default use the current user's LanguageID, but the client can override it via saveOptions with a specific language (think of the Language dropdown on first form in Import Wizard)
            var altLanguageID = saveOptions.AltLanguageID ?? LanguageID;

            try {

                if (dsReturn == null) {
                    dsReturn = createReturnDataSet();
                }

                bool createdDataManager = false;
                try {
                    if (dm == null) {
                        // we need to remember if we created the datamanager so we know if we
                        // should cleanup when we exit -- some methods have reentrant calls to
                        // the saveData method and we need to respect that situation so
                        // we don't rollback or commit a transaction too soon.
                        dm = BeginProcessing(true);
                        createdDataManager = true;
                    }






                    DataTable dtException = GetExceptionTable(dsReturn);


                    foreach (DataTable dt in ds.Tables) {
                        // don't process the administrative datatables ('ExceptionTable' and 'MappingTable') table that we intrinsically create during a read
                        // (and they may have subsequently passed back the same DataSet)
                        if (dt.TableName != "ExceptionTable" && dt.TableName.ToLower() != "validate_login") {

                            if (dt.Rows.Count > 0) {

                                // A mapping table is assumed to exist
                                // It tells us which columns should apply to which table(s)
                                // e.g. if a DataTable actually maps to more than one database table
                                // (in the case of a join), the mapping table tells us how to resolve
                                //  each column to the proper database table.
                                // If no rows exist for the given DataTable name, just assume all map
                                // directly to the database table with the same name as the DataTable

                                var fmdv = Dataview.Map(dt.TableName, _languageID, dm) as IDataview;

                                DataTable dtClone = cloneTableForReturn(dt, dsReturn);

                                // see http://www.eggheadcafe.com/articles/20030205.asp for info as to why we do the following...
                                dtClone.BeginLoadData();

                                int savedCount = 0;
                                int failedCount = 0;

                                // triggers can be applied at both the dataview level and the table level.
                                // here's the precedence:
                                //  (assuming 1 DataTable represents 3 database tables and there are 5 DataRows in the DataTable object)
                                // - dataview.DataViewSaving  (1 call,   1 DataTable object)
                                // - table.TableSaving        (3 calls,  3 db tables)
                                // - dataview.RowSaving       (5 calls,  5 rows in DataTable object)
                                // - table.RowSaving          (15 calls, 3 db tables x 5 rows in DataTable object)
                                // - table.RowSaved           (15 calls, 3 db tables x 5 rows in DataTable object)
                                // - dataview.RowSaved        (5 calls,  5 rows in DataTable object)
                                // - table.TableSaved         (3 calls,  3 db tables)
                                // - dataview.DataViewSaved   (1 call,   1 DataTable object)

                                // create an argument object for each dataview trigger
                                List<ISaveDataTriggerArgs> dvTriggerArgs = dataviewTriggerDataViewSaving(fmdv, dt, dm, dsReturn, saveOptions);

                                // create an argument object for each unique table and table-level trigger (meaning if we have 2 tables and 3 triggers per table, 
                                // we'll end up with a grand total of 6 argument objects)
                                List<List<ISaveDataTriggerArgs>> allTableTriggerArgs = tableTriggerTableSaving(fmdv, dt, dm, dsReturn, saveOptions);

                                for (var rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++) {
                                    var dr = dt.Rows[rowIndex];

                                    // one DataTable may actually map to multiple database tables,
                                    // so this means one row may map to multiple table mappings as well.
                                    // we have to spin through all these (typically there will be only 1)
                                    // and run a command for each mapping.
                                    // NOTE: we're not using db transactions.  at least, not yet.

                                    // we only map rows that have changed (added, modified, or removed) -- others we ignore
                                    if (dr.RowState != DataRowState.Unchanged) {

                                        SaveMode saveMode;
                                        SaveMode origSaveMode;
                                        switch (dr.RowState) {
                                            case DataRowState.Added:
                                                saveMode = SaveMode.Insert;
                                                break;
                                            case DataRowState.Modified:
                                                saveMode = SaveMode.Update;
                                                break;
                                            case DataRowState.Deleted:
                                                saveMode = SaveMode.Delete;
                                                break;
                                            default:
                                                saveMode = SaveMode.None;
                                                break;
                                        }

                                        origSaveMode = saveMode;

                                        try {

                                            // apply dataview trigger(s) before row save (will throw an exception if save is cancelled)
                                            dataviewTriggerRowSaving(fmdv, dvTriggerArgs, saveMode, dr, dsReturn);

                                            bool? saveSuccessful = true;

                                            for (int i = 0; i < fmdv.Tables.Count; i++) {
                                                var fmt = fmdv.Tables[i];

                                                // testing...
                                                saveMode = origSaveMode;

                                                if (!String.IsNullOrEmpty(fmdv.PrimaryKeyTableName)) {
                                                    if (fmdv.PrimaryKeyTableName.ToLower() != fmt.TableName.ToLower()) {
                                                        switch (saveMode) {
                                                            case SaveMode.Delete:
                                                            case SaveMode.Insert:
                                                            case SaveMode.Update:
                                                                // do not allow deleting/inserting into non-pk table
                                                                continue;

                                                            //case SaveMode.Update
                                                            // allow update only if the primary key is defined on both 
                                                            // the table and the dataview

                                                            //                                                                break;
                                                            case SaveMode.None:
                                                            case SaveMode.Unknown:
                                                            default:
                                                                break;
                                                        }
                                                    }
                                                }

                                                // call down into the save code which eventually issues the SQL to the database...
                                                if (!saveAndProcessResult(allTableTriggerArgs[i], fmdv, fmt, dr, dm, ref saveMode, dtClone, dsReturn, saveOptions, rowIndex, ref failedCount, ref savedCount, ref saveSuccessful, i)) {
                                                    break;
                                                }
                                            }

                                            // apply dataview trigger(s) after row save
                                            dataviewTriggerAfterRowSave(fmdv, saveSuccessful, dvTriggerArgs, dsReturn);

                                        } catch (Exception ex) {
                                            if (!alreadyLoggedException(ex)) {
                                                // update / insert / delete failed.  log why, throw that back to the caller
                                                var pkName = (fmdv == null ? null : fmdv.PrimaryKeyNames == null ? null : fmdv.PrimaryKeyNames.Count == 0 ? null : fmdv.PrimaryKeyNames[0]);
                                                copyFailedRowToReturnTable(pkName, dtClone, dr, ex, saveMode, dsReturn, rowIndex, fmdv.DataViewName, "");
                                            }

                                            // after we've logged stuff to our cloned table,
                                            // we may need to throw it
                                            if (LogException(ex, dsReturn)) {
                                                throw;
                                            }
                                        }
                                    }
                                    if (saveOptions.BackgroundWorker != null && saveOptions.RowProgressInterval > 0) {
                                        if (rowIndex % saveOptions.RowProgressInterval == 0 && rowIndex > 0) {
                                            var pct = ((decimal)rowIndex / (decimal)dt.Rows.Count) * 100.0M;
                                            var progress = (int)pct;
                                            worker.ReportProgress(progress, rowIndex);
                                            if (worker.CancellationPending) {
                                                throw Library.CreateBusinessException(getDisplayMember("saveData{usercancelled}", "Save cancelled by user"));
                                            }
                                        }
                                    }

                                    //if (saveOptions.UseUniqueKeys && rowIndex % 1000 == 0) {

                                    //}
                                }

                                // apply table trigger(s) after table save
                                tableTriggerTableSaved(fmdv, allTableTriggerArgs, savedCount, failedCount, dsReturn);

                                // apply dataview trigger(s) after dataview save
                                dataviewTriggerDataViewSaved(fmdv, dvTriggerArgs, savedCount, failedCount, dsReturn);

                                // see http://www.eggheadcafe.com/articles/20030205.asp for info as to why we do the following...
                                dtClone.EndLoadData();

                            }

                            //dt.Clear();
                            //dt.Dispose();
                        }
                    }
                    //ds.Dispose();
                } finally {
                    if (createdDataManager) {
                        // we created it, we need to clean it up
                        if (dm != null) {
                            dm.Dispose();
                        }
                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, altLanguageID);
            }
            return dsReturn;
        }

        /// <summary>
        /// Originally more complex, right now it just calls down into doSave() and always returns true.
        /// </summary>
        /// <param name="singleTableTriggerArgs"></param>
        /// <param name="fmdv"></param>
        /// <param name="fmt"></param>
        /// <param name="dr"></param>
        /// <param name="dm"></param>
        /// <param name="saveMode"></param>
        /// <param name="dtClone"></param>
        /// <param name="dsReturn"></param>
        /// <param name="saveOptions"></param>
        /// <param name="originalRowIndex"></param>
        /// <param name="failedCount"></param>
        /// <param name="savedCount"></param>
        /// <param name="saveSuccessful"></param>
        /// <param name="tableOffset"></param>
        /// <returns></returns>
        private bool saveAndProcessResult(List<ISaveDataTriggerArgs> singleTableTriggerArgs, IDataview fmdv, ITable fmt, DataRow dr, DataManager dm, ref SaveMode saveMode, DataTable dtClone, DataSet dsReturn, SaveOptions saveOptions, int originalRowIndex, ref int failedCount, ref int savedCount, ref bool? saveSuccessful, int tableOffset) {


            saveSuccessful = doSave(singleTableTriggerArgs, fmdv, fmt, dr, dm, ref saveMode, dtClone, dsReturn, saveOptions, originalRowIndex, ref failedCount, ref savedCount);

            return true;

        }

        /// <summary>
        /// Based on inputs, determines what kind of command to run (update/insert/delete/none), fires off row-level pre-triggers, runs command, fires off row-level post-triggers, and returns whether it was successful, failed, or didn't execute at all.  Fills the dtClone table appropriately.
        /// </summary>
        /// <param name="singleTableTriggerArgs"></param>
        /// <param name="fmdv"></param>
        /// <param name="fmt"></param>
        /// <param name="dr"></param>
        /// <param name="dm"></param>
        /// <param name="saveMode"></param>
        /// <param name="dtClone"></param>
        /// <param name="dsReturn"></param>
        /// <param name="saveOptions"></param>
        /// <param name="originalRowIndex"></param>
        /// <param name="failedCount"></param>
        /// <param name="savedCount"></param>
        /// <returns></returns>
        private bool? doSave(List<ISaveDataTriggerArgs> singleTableTriggerArgs, IDataview fmdv, ITable fmt, DataRow dr, DataManager dm, ref SaveMode saveMode, DataTable dtClone, DataSet dsReturn, SaveOptions saveOptions, int originalRowIndex, ref int failedCount, ref int savedCount) {
            try {

                // saveMode defaults to that specified by dr.RowState.
                // However, dr.RowState cannot always be trusted (as in the case of the Import Wizard, RowState is *always* Added), 
                // so this method determines the proper saveMode 
                determineSaveMode(fmdv, fmt, dr, dm, ref saveMode, saveOptions);

                if (saveMode == SaveMode.Unknown || saveMode == SaveMode.None) {
                    // nothing to execute -- did not succeed or fail.
                    if (saveOptions.UseUniqueKeys) {
                        // even though we didn't need to write anything, the caller is saying to UseUniqueKeys meaning they didn't give us
                        // primary key values.
                        // the pk value may not be a column in the table, and child data in this same row may need it (as part of their unique key).
                        // so add the field to the table if it doesn't exist.
                        var pkf = fmt.GetField(fmt.PrimaryKeyFieldName);
                        if (!dr.Table.Columns.Contains(pkf.AliasedTableFieldName)) {
                            var dcPK = new DataColumn(pkf.AliasedTableFieldName, pkf.DataType);
                            dcPK.AllowDBNull = true;
                            //addJoinChildrenExtendedProperties(pkf, dcPK);
                            dcPK.ExtendedProperties["table_alias_name"] = pkf.Table.AliasName.ToLower();
                            dcPK.ExtendedProperties["table_field_name"] = pkf.TableFieldName.ToLower();

                            dr.Table.Columns.Add(dcPK);
                        }
                        //dr[pkf.AliasedTableFieldName] = DBNull.Value;
                    }

                    return null;

                } else {

                    // ok, we probably have something to do.
                    // tell the trigger we're going to save a row.
                    // apply table trigger(s) before row save (no try/catch in this guy, relies on doSave() to handle it)
                    tableTriggerRowSaving(fmt, singleTableTriggerArgs, dr, dm, saveMode, dsReturn);

                    // call determinSaveMode again after the triggers to pull in any changed values / added fields -- only necessary when using alternate key for uniqueness
                    if (saveOptions.UseUniqueKeys) {
                        determineSaveMode(fmdv, fmt, dr, dm, ref saveMode, saveOptions);
                    }

                    // now that the trigger may have altered the datarow values, we need to regen the save command
                    var cmd = initSaveCommand(fmdv, fmt, dr, dm, ref saveMode, saveOptions);


                    if (cmd == null) {

                        // will not run any SQL.
                        // but we need to add in any pk columns if we're doing IW processing so subsequent ones match up properly.

                        if (saveOptions.UseUniqueKeys) {
                            // the pk value may not be a column in the table, and child data in this same row may need it (as part of their unique key).
                            // so add the field to the table if it doesn't exist.
                            var pkf = fmt.GetField(fmt.PrimaryKeyFieldName);
                            if (!dr.Table.Columns.Contains(pkf.AliasedTableFieldName)) {
                                var dcPK = new DataColumn(pkf.AliasedTableFieldName, pkf.DataType);
                                dcPK.AllowDBNull = true;
                                //addJoinChildrenExtendedProperties(pkf, dcPK);
                                dcPK.ExtendedProperties["table_alias_name"] = pkf.Table.AliasName.ToLower();
                                dcPK.ExtendedProperties["table_field_name"] = pkf.TableFieldName.ToLower();
                                dr.Table.Columns.Add(dcPK);
                            }
                            //dr[pkf.AliasedTableFieldName] = DBNull.Value;

                            //// also output the saved row for IW...
                            //copySavedRowToReturnTable(fmt.PrimaryKeyDataViewFieldName, dtClone, dr, saveMode, 0, dsReturn, originalRowIndex, fmt.TableName, fmt.AliasName);

                        }

                        return null;

                    } else {
                        // run the command (save to db)
                        int affected = 0;

                        Exception ex = null;

                        if (saveMode == SaveMode.Insert) {
                            // return id of field to add
                            try {
                                affected = dm.Write(cmd, true, fmt.PrimaryKeyFieldName);
                                if (saveOptions.UseUniqueKeys) {
                                    // the pk value may not be a column in the table, and child data in this same row may need it (as part of their unique key).
                                    // so add the field to the table if it doesn't exist.
                                    var pkf = fmt.GetField(fmt.PrimaryKeyFieldName);
                                    if (!dr.Table.Columns.Contains(pkf.AliasedTableFieldName)) {
                                        var dcPK = new DataColumn(pkf.AliasedTableFieldName, pkf.DataType);
                                        //addJoinChildrenExtendedProperties(pkf, dcPK);
                                        dcPK.ExtendedProperties["table_alias_name"] = pkf.Table.AliasName.ToLower();
                                        dcPK.ExtendedProperties["table_field_name"] = pkf.TableFieldName.ToLower();
                                        dr.Table.Columns.Add(dcPK);
                                    }
                                    dr[pkf.AliasedTableFieldName] = affected;
                                }
                            } catch (Exception exInsert) {
                                ex = exInsert;
                            }
                        } else {
                            // return total rows affected
                            try {
                                affected = dm.Write(cmd);
                            } catch (SqlException exSqlServer) {
                                // HACK HACK HACK Sql server doesn't allow deleting from a table with > 253 foreign keys pointing at it
                                //                (aka cooperator table).  Eat this unfriendly error and throw a nicer one.
                                if (exSqlServer.ErrorCode == -2146232060 && fmt.TableName.ToLower() == "cooperator") {
                                    try {
                                        throw Library.CreateBusinessException(getDisplayMember("doSave{cannotdelete}", "It is not possible to delete data from the {0} table.", fmt.TableName));
                                    } catch (Exception exSqlHack) {
                                        ex = exSqlHack;
                                    }
                                } else {
                                    ex = exSqlServer;
                                }
                            } catch (Exception exUpdate) {
                                ex = exUpdate;
                            }
                        }

                        foreach (var args in singleTableTriggerArgs) {
                            args.Exception = ex;
                        }

                        // apply table trigger(s) after row save (no try/catch in this guy, relies on doSave() to handle it)
                        tableTriggerAfterRowSave(fmt, singleTableTriggerArgs, dr, dm, saveMode, affected, dsReturn);


                        if (affected == 0) {
                            // we didn't update/insert/delete anything
                            failedCount++;
                            if (ex == null) {
                                throw Library.CreateBusinessException(getDisplayMember("doSave{modified}", "{0} failed.  Data has been modified or deleted since your last refresh.  Your changes can not be saved until your view is refreshed.\n\nDetail: No records affected for sql={1}", saveMode.ToString() ,cmd.ToString()));
                            } else {
                                throw Library.CreateBusinessException(getDisplayMember("doSave{failed}", "{0} failed.  {1}", saveMode.ToString(), ex.Message), ex);
                            }

                        } else {
                            // everything worked!  yay.
                            savedCount++;

                            // only return the row if:
                            // it was inserted (so they can get the primary key)
                            // it failed (so they can get the reason why it failed)
                            //
                            // updates and deletes do not need to be sent back

                            copySavedRowToReturnTable(fmt.PrimaryKeyDataViewFieldName, dtClone, dr, saveMode, affected, dsReturn, originalRowIndex, fmt.TableName, fmt.AliasName, saveOptions);
                            return true;
                        }
                    }
                }

            } catch (Exception ex) {

                // update / insert / delete failed.  copy to return table if it's not already logged
                if (!alreadyLoggedException(ex)) {
                    copyFailedRowToReturnTable(fmt.PrimaryKeyDataViewFieldName, dtClone, dr, ex, saveMode, dsReturn, originalRowIndex, fmt.TableName, fmt.AliasName);
                }

                // after we've logged stuff to our cloned table,
                // we may need to throw it
                if (LogException(ex, dsReturn)) {
                    throw;
                }

                return false;
            }
        }

        /// <summary>
        /// Generates a database engine-agnostic SELECT command for the given inputs
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="dr"></param>
        /// <param name="dm"></param>
        /// <returns></returns>
        private DataCommand createSelectCommand(ITable fmt, DataRow dr, DataManager dm) {

            #region scrub inputs / initialization
            if (fmt.Mappings.Count < 1) {
                return null;
            }
            StringBuilder keys = new StringBuilder();
            DataParameters dps = new DataParameters();
            DataRowVersion drv = dr.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;
            var i = 0;
            #endregion scrub inputs / initialization

            #region process fields
            foreach (Field fm in fmt.Mappings) {
                if (!String.IsNullOrEmpty(fm.DataviewFieldName) && dr.Table.Columns.Contains(fm.DataviewFieldName)) {
                    // only map fields that are part of this dataview and appear in the datatable
                    object val = dr[fm.DataviewFieldName, drv];
                    DbType dbType = DataParameter.MapDbType(fm.DataType);
                    DbPseudoType dbPseudoType = DataParameter.MapDbPseudoType(fm.DataType);

                    // where consists of primary key(s)
                    if (fm.IsPrimaryKey) {
                        keys.Append(generateWhereFieldAndParameter(fm, val, dm, ref i, dps));
                        keys.Append(" and ");
                        //keys.Append(fm.TableFieldName).Append(" = ").Append(":WHERE__" + i).Append(" and ");
                        //dps.Add(new DataParameter(":WHERE__" + i, val, dbType, dbPseudoType));
                    } else if (fmt.UniqueKeyFields.Contains(fm.DataviewFieldName)) {

                        keys.Append(generateWhereFieldAndParameter(fm, val, dm, ref i, dps));
                        keys.Append(" and ");

                        //i++;
                        //if (fm.IsNullable) {
                        //    keys.Append("(" + fm.TableFieldName + " = :WHERE__" + i + " OR :WHERENULL__" + i + " IS NULL)").Append(" and ");
                        //    dps.Add(new DataParameter(":WHERE__" + i, val, dbType, dbPseudoType));
                        //    dps.Add(new DataParameter(":WHERENULL__" + i, val, dbType, dbPseudoType));
                        //} else {
                        //    keys.Append(fm.TableFieldName + " = :WHERE__" + i).Append(" and ");
                        //    dps.Add(new DataParameter(":WHERE__" + i, val, dbType, dbPseudoType));
                        //}
                    }
                }
            }
            #endregion process fields

            #region scrub after field processing
            if (dps.Count == 0) {
                // no need to run this command
                return null;
            }
            #endregion scrub after field processing

            #region finalize sql statement
            if (keys.Length > 5) {
                keys.Remove(keys.Length - 5, 5);
            }

            string sql = "select * from " + fmt.TableName + " where " + keys.ToString();
            DataCommand cmd = new DataCommand(sql, dps);
            return cmd;
            #endregion finalize sql statement

        }

        /// <summary>
        /// Generates a database engine-agnostic DELETE command for the given inputs
        /// </summary>
        /// <param name="fmdv"></param>
        /// <param name="fmt"></param>
        /// <param name="dr"></param>
        /// <param name="dm"></param>
        /// <param name="saveOptions"></param>
        /// <returns></returns>
        private DataCommand createDeleteCommand(IDataview fmdv, ITable fmt, DataRow dr, DataManager dm, SaveOptions saveOptions) {

            #region scrub inputs / initialization
            if (fmt.Mappings.Count < 1) {
                return null;
            }
            StringBuilder keys = new StringBuilder();
            DataParameters dps = new DataParameters();
            bool foundPrimaryKey = false;
            //bool isMainTableForDataView = false;

            var i = 0;
            #endregion scrub inputs / initialization

            #region process fields
            foreach (Field fm in fmt.Mappings) {

                if (fmdv == null) {
                    // this is a direct table mapping.
                    // copy over dataviewfieldname to tablefieldname.
                    fm.DataviewName = fm.TableName;
                    fm.DataviewFieldName = fm.TableFieldName;
                }

                if (String.IsNullOrEmpty(fm.DataviewFieldName)) {
                    // this field is not mapped in the dataview.
                    // also, this is a dataview and not just a table mapping (for dataview imports specifically)
                    // skip it.
                    continue;
                }

                if (dr.Table.Columns.Contains(fm.DataviewFieldName)) {


                    //if (fmdv == null || fmdv.PrimaryKeyNames.Contains(fm.DataViewFieldName)) {
                    //    isMainTableForDataView = true;
                    //}

                    object original = dr[fm.DataviewFieldName, DataRowVersion.Original];

                    // where consists of primary key(s) AND the modified field (so we don't
                    // delete a record that has been updated since last time we read it)
                    if (fm.IsPrimaryKey) {

                        keys.Append(generateWhereFieldAndParameter(fm, original, dm, ref i, dps));
                        keys.Append(" and ");
                        //var whereParam = ":WHERE__" + i.ToString().PadLeft(3, '0');
                        //i++;

                        //keys.Append(fm.TableFieldName).Append(" = ").Append(whereParam).Append(" and ");
                        //dps.Add(new DataParameter(whereParam, original, dbType));

                        foundPrimaryKey = true;

                    } else if (fm.IsModifiedDate && fmt.AuditsModified) {
                        if (saveOptions.SafeUpdatesAndDeletes) {
                            keys.Append(generateWhereFieldAndParameter(fm, original, dm, ref i, dps));
                            keys.Append(" and ");

                            //string whereParam1 = null;
                            //string whereParam2 = null;
                            //keys.Append(createNullableWhereField(fm.TableFieldName, out whereParam1, out whereParam2, ref i)).Append(" and ");
                            //dps.Add(new DataParameter(whereParam1, original, DbType.DateTime2));
                            //dps.Add(new DataParameter(whereParam2, original, DbType.DateTime2));
                        }
                    }
                }
            }
            #endregion process fields

            #region scrub after field processing
            if (dps.Count == 0 || !foundPrimaryKey) {
                // no need to run this command -- either no pks specified or this table isn't the main one for the dataview.
                return null;
            }
            #endregion scrub after field processing

            #region finalize sql statement
            if (keys.Length > 5) {
                keys.Remove(keys.Length - 5, 5);
            }

            string sql = "delete from " + fmt.TableName + " where " + keys.ToString();
            DataCommand cmd = new DataCommand(sql, dps);
            return cmd;
            #endregion finalize sql statement

        }

        /// <summary>
        /// Returns the proper where clause text for given parameters.  Auto-appends a new DataParameter object to the given dps collection. Does not increment offset (isn't ref)
        /// </summary>
        /// <param name="f"></param>
        /// <param name="value"></param>
        /// <param name="dm"></param>
        /// <param name="offset"></param>
        /// <param name="dps"></param>
        /// <returns></returns>
        private string generateWhereFieldAndParameter(IField f, object value, DataManager dm, ref int offset, DataParameters dps) {

            // Should kick out something like this for non-nullable fields:
            // text_field = :WHERE__000
            // date_field = :WHERE__003
            // int_field = :WHERE__001
            // decimal_field = :WHERE__002
            //
            // And something like this for nullable fields:
            // coalesce(text_field, '') = coalesce(:WHERE__000, '')
            // coalesce(date_field, '1899-01-01') = coalesce(:WHERE__003, '1899-01-01')
            // coalesce(int_field, 0) = coalesce(:WHERE__001, 0)
            // coalesce(decimal_field, 0.0) = coalesce(:WHERE__002, 0.0)

            var output = "";

            DbType dbType = DbType.String;
            if (f.DataType != null) {
                dbType = DataParameter.MapDbType(f.DataType);
            } else {
                dbType = DataParameter.DeriveDbType(value);
            }

            DbPseudoType dbPseudoType = DataParameter.MapDbPseudoType(f.DataType);

            var prmName = ":WHERE__" + offset.ToString().PadLeft(3, '0');
            if (!f.IsNullable) {
                output = f.TableFieldName + " = " + prmName;
                dps.Add(new DataParameter(prmName, value, dbType, dbPseudoType));
            } else {

                // this incorrectly matches data with NULL when it shouldn't...
                //whereFields.Add("(" + f.TableFieldName + " = :__WHERE" + offset + " OR :__WHERENULL" + offset + " IS NULL)");
                //whereParams.Add(new DataParameter(":__WHERE" + offset, dr[f.DataviewFieldName, drv], f.DataType));
                //whereParams.Add(new DataParameter(":__WHERENULL" + offset, dr[f.DataviewFieldName, drv], f.DataType));

                string emptyValue = null;
                switch (f.DataTypeString) {
                    case "STRING":
                    default:
                        if (dm.DataConnectionSpec.Vendor == DataVendor.Oracle) {
                            // HACK: oracle treats zero-length string the same as NULL, so we need to pass it something
                            // the user can never enter manaully instead of zero-length string -- such as a backspace character.
                            emptyValue = "'\b'";
                        } else {
                            emptyValue = "''";
                        }
                        break;
                    case "INTEGER":
                        emptyValue = "0";
                        break;
                    case "DATETIME":
                        if (dm.DataConnectionSpec.Vendor == DataVendor.Oracle) {
                            // HACK: oracle doesn't like the same format everybody else does for dates
                            emptyValue = "TO_DATE('1899-01-01', 'YYYY-MM-DD')";
                        } else {
                            emptyValue = "'1899-01-01'";
                        }
                        break;
                    case "DECIMAL":
                        emptyValue = "0.0";
                        break;
                }

                if (dm.DataConnectionSpec.Vendor == DataVendor.Oracle && f.DataTypeString == "STRING")
                {
                    output = "coalesce(dbms_lob.substr(" + f.TableFieldName + ",32767,1), " + emptyValue + ") = coalesce(" + prmName + ", " + emptyValue + ")";
                }
                else
                {
                    output = "coalesce(" + f.TableFieldName + ", " + emptyValue + ") = coalesce(" + prmName + ", " + emptyValue + ")";
                }
                dps.Add(new DataParameter(prmName, value, dbType, dbPseudoType));

            }
            offset++;
            return output;
        }

        /// <summary>
        /// Generates a database engine-agnostic UPDATE command for the given inputs
        /// </summary>
        /// <param name="fmdv"></param>
        /// <param name="fmt"></param>
        /// <param name="dr"></param>
        /// <param name="cooperatorID"></param>
        /// <param name="dm"></param>
        /// <param name="saveOptions"></param>
        /// <returns></returns>
        private DataCommand createUpdateCommand(IDataview fmdv, ITable fmt, DataRow dr, int cooperatorID, DataManager dm, SaveOptions saveOptions) {

            #region scrub inputs / initialization
            if (fmt.Mappings.Count < 1) {
                return null;
            }

            StringBuilder setClause = new StringBuilder();
            StringBuilder whereClause = new StringBuilder();
            DataParameters dpSet = new DataParameters();
            DataParameters dpWhere = new DataParameters();

            bool changingData = false;
            bool foundModifiedBy = false;
            bool foundModifiedAt = false;
            bool foundPrimaryKey = false;

            var i = 0;
            #endregion scrub inputs / initialization

            #region process fields
            for (var j = 0; j < fmt.Mappings.Count; j++) {
                var fm = fmt.Mappings[j];

                if (fmdv == null) {
                    // this is a direct table mapping.
                    // copy over dataviewfieldname to tablefieldname.
                    fm.DataviewName = fm.TableName;
                    fm.DataviewFieldName = fm.TableFieldName;
                } else {
                    // this is mapped via a dataview.
                    // use the dataview settings (i.e. readonly) as they may override the table settings.
                    fm = Toolkit.Coalesce(fmdv.GetField(fm.TableFieldID, fmt.AliasName), fm) as IField;
                }



                bool addField = false;
                var fieldName = string.Empty;
                if (!String.IsNullOrEmpty(fm.DataviewFieldName)) {
                    if (dr.Table.Columns.Contains(fm.DataviewFieldName)) {
                        // valid dataview field, add it to the command
                        addField = true;
                        fieldName = fm.DataviewFieldName;
                    } else if (dr.Table.Columns.Contains(fm.AliasedTableFieldName)) {
                        addField = true;
                        fieldName = fm.AliasedTableFieldName;
                    }
                } else if (String.IsNullOrEmpty(fm.DataviewFieldName)) {

                    var idx = dr.Table.Columns.IndexOf(fm.AliasedTableFieldName);
                    if (idx > -1) {
                        fieldName = fm.AliasedTableFieldName;
                        if (idx >= fmdv.Mappings.Count) {
                            addField = true;
                        }
                    } else if (dr.Table.Columns.Contains(fm.TableFieldName) && fmt.AliasName == fm.Table.AliasName) {
                        fieldName = fm.TableFieldName;

                        idx = dr.Table.Columns.IndexOf(fm.TableFieldName);
                        if (idx >= fmdv.Mappings.Count) {
                            addField = true;
                        }
                    }
                }
                //} else if (String.IsNullOrEmpty(fm.DataviewFieldName) 
                //    && (dr.Table.Columns.Contains(fm.TableFieldName) || dr.Table.Columns.Contains(fmt.AliasName + "." + fm.TableFieldName)) 
                //    && fmt.AliasName == fm.Table.AliasName){
                //    var idx1 = dr.Table.Columns.IndexOf(fm.TableFieldName);
                //    if (idx1 >= fmdv.Mappings.Count){
                //        // this field was tacked on by a trigger.
                //        // this means somebody needs to save it.
                //        addField = true;
                //        fieldName = fm.TableFieldName;
                //    } else if (idx1 == -1) {
                //        idx1 = dr.Table.Columns.IndexOf(fmt.AliasName + "." + fm.TableFieldName);
                //        if (idx1 >= fmdv.Mappings.Count){
                //            addField = true;
                //            fieldName = fmt.AliasName + "." + fm.TableFieldName;
                //        }
                //    }
                //}

                if (addField) {
                    // this field is mapped in the dataview or was added by a trigger.

                    object original = null;
                    string originalToStr = null;
                    object current = null;
                    string currentToStr = null;

                    if (dr.RowState == DataRowState.Added) {

                        // we may run into this case when the saveMode was overridden from Insert to Update
                        // because a unique key was detected in the data (and the option "useuniquekeys" was true)
                        // i.e. the user 'added' the row in the CT but the middle tier changed it to be an update
                        // so it 'does the right thing' and updates the row represented by the unique key instead of trying
                        // to create a new one and bomb on the duplicate key error.
                        // if that's the case


                        // there will be no 'original' value here, so just ignore it
                        original = DBNull.Value;
                        originalToStr = string.Empty;


                    } else {
                        original = dr[fieldName, DataRowVersion.Original];

                        if (original == DBNull.Value) {
                            original = fm.DefaultValue.ToString() == "{DBNull.Value}" ? DBNull.Value : fm.DefaultValue;
                        }


                        originalToStr = original.ToString();

                    }

                    current = dr[fieldName, DataRowVersion.Current];
                    currentToStr = current.ToString();

                    DbType dbType = DbType.String;
                    if (fm.DataType != null) {
                        dbType = DataParameter.MapDbType(fm.DataType);
                    } else {
                        dbType = DataParameter.DeriveDbType(original);
                    }

                    if (fm.IsPrimaryKey) {

                        if (originalToStr != currentToStr && saveOptions.SafeUpdatesAndDeletes) {
                            if (!saveOptions.UseUniqueKeys) {
                                // they're trying to update the primary key.   BOMB BOMB BOMB only bad stuff can happen if we allow that.
                                throw Library.CreateBusinessException(getDisplayMember("createUpdateCommand{noupdatepk}", 
                                    "{0}.{1} changed values from '{2}' to '{3}'.  This field is a primary key ({4}.{5}) and cannot be changed.", 
                                    fm.DataviewName, fm.DataviewFieldName,originalToStr, currentToStr, fm.TableName, fm.TableFieldName )  );
                            } else {
                                // they're opting in to using unique key as an alternative, so assume the changed value stored in the datarow is valid -- but don't update it, use it as the key
                            }
                        }
                        // add to where clause
                        whereClause.Append(generateWhereFieldAndParameter(fm, current, dm, ref i, dpWhere));
                        whereClause.Append(" and ");
                        
                        //whereClause.Append(fm.TableFieldName).Append(" = :").Append(fm.TableFieldName).Append(" and ");
                        //dpWhere.Add(new DataParameter(":" + fm.TableFieldName, current, dbType));

                        foundPrimaryKey = true;
                    } else if (!fm.IsReadOnly) {

                        // we are allowed to update this data (and it's not an audit field)

                        if (fm.GuiHint == "TOGGLE_CONTROL" || fm.TableFieldName.ToLower().StartsWith("is_")) {
                            // this is a boolean field.
                            if (saveOptions.BoolDefaultIsFalse) {
                                if ((current + string.Empty) == string.Empty) {
                                    current = "N";
                                    currentToStr = "N";
                                }
                            }
                        }


                        // add to both where and set clause, but only if it has changed
                        // (we add to where clause so if data we're going to update has changed since last read,
                        //  it is not updated)
                        if (originalToStr != currentToStr) {

                            if (saveOptions.SafeUpdatesAndDeletes) {

                                whereClause.Append(generateWhereFieldAndParameter(fm, original, dm, ref i, dpWhere));
                                whereClause.Append(" and ");

                                //string whereParam1 = null;
                                //string whereParam2 = null;
                                //whereClause.Append(createNullableWhereField(fm.TableFieldName, out whereParam1, out whereParam2, ref i)).Append(" and ");
                                //dpWhere.Add(new DataParameter(whereParam1, original, dbType));
                                //dpWhere.Add(new DataParameter(whereParam2, original, dbType));
                            }

                            setClause.Append(fm.TableFieldName).Append(" = :").Append(fm.TableFieldName).Append(", ");
                            dpSet.Add(new DataParameter(":" + fm.TableFieldName, current, dbType));

                            changingData = true;

                        }
                    } else {
                        // field is read only.
                        // if they've changed a value, throw an exception
                        // but only if safeUpdates is enabled! (i.e. for dataview import this check needs to be skipped)
                        if (saveOptions.SafeUpdatesAndDeletes) {
                            var changed = false;
                            if (current is DateTime) {
                                changed = Toolkit.ToDateTime(current) != Toolkit.ToDateTime(original);
                            } else if (current is string) {
                                changed = ((string)current) != ((string)original);
                            } else if (current is Int32) {
                                changed = Toolkit.ToInt32(current, 0) != Toolkit.ToInt32(original, 0);
                            } else if (current is Int64) {
                                changed = Toolkit.ToInt64(current, 0) != Toolkit.ToInt64(original, 0);
                            } else if (current is decimal) {
                                changed = Toolkit.ToDecimal(current, 0) != Toolkit.ToDecimal(original, 0);
                            } else if (current is float) {
                                changed = Toolkit.ToFloat(current, 0.0f) != Toolkit.ToFloat(original, 0.0f);
                            } else if (current is DBNull) {
                                changed = ((DBNull)current) != ((DBNull)original);
                            } else {
                                changed = current != original;
                            }
                            if (changed) {
                                throw Library.CreateBusinessException(getDisplayMember("createUpdateCommand{readonlyfield}", "Update failed.  Can not update read-only field named '{0}' (dataview field name: {1}).", fm.FriendlyFieldName, fm.DataviewFieldName));
                            }
                        }
                    }
                }
                if (fm.IsModifiedDate && fmt.AuditsModified) {

                    // special case -- we use modified to match as part of key (so we don't have
                    //  concurrent update problems) and we also alter the modified field upon update

                    //// add to where cluase
                    //whereClause.Append(fm.DatabaseFieldName).Append(" = :").Append(fm.DatabaseFieldName).Append("__WHERE__ and ");
                    //dpWhere.Add(new DataParameter(":" + fm.DatabaseFieldName + "__WHERE__", current));

                    // add to set clause!!!
                    setClause.Append(fm.TableFieldName).Append(" = :").Append(fm.TableFieldName).Append(", ");
                    dpSet.Add(new DataParameter(":" + fm.TableFieldName, DateTime.Now.ToUniversalTime(), DbType.DateTime2));

                    foundModifiedAt = true;

                } else if (fm.IsModifiedBy && fmt.AuditsModified) {

                    // add current user cooperatorID as last modified by (set clause)
                    setClause.Append(fm.TableFieldName).Append(" = :").Append(fm.TableFieldName).Append(", ");
                    dpSet.Add(new DataParameter(":" + fm.TableFieldName, cooperatorID, DbType.Int32));

                    foundModifiedBy = true;

                }
            }
            #endregion process fields

            #region scrub after field processing
            if (!changingData || !foundPrimaryKey) {
                if (!saveOptions.UseUniqueKeys) {
                    // no need to run this command, as no data fields are being updated (or they didn't give us the primary key
                    return null;
                } else {
                    // during import, we need to run the command even if there is no net change or pk value given.
                    // this is so output to the IW looks correct (import_accession will not display the Taxonomy row, but it will show the Accession row.
                    // while processing will work fine without this clause, it won't emit the Taxonomy row so the IW can't display which Taxon the Accession row is associated with.
                }
            }
            #endregion scrub after field processing

            #region apply audits as needed
            if (fmt.AuditsModified && (!foundModifiedBy || !foundModifiedAt)) {
                // look up their mappings based on the actual table name
                // 2010-01-08 brock@circaware.com
                // This no longer works because we don't have tables mapped as dataviews anymore (which was a silly restriction in the first place)
                // 
                var tblMap = Table.Map(fmt.TableName, dm.DataConnectionSpec, LanguageID, false); // brock
                if (tblMap != null) {
                    foreach (var tblFld in tblMap.Mappings) {
                        if (tblFld.IsModifiedBy) {
                            if (!foundModifiedBy) {
                                // add user cooperatorID as last modified by
                                setClause.Append(tblFld.TableFieldName).Append(" = :").Append(tblFld.TableFieldName).Append(", ");
                                dpSet.Add(new DataParameter(":" + tblFld.TableFieldName, cooperatorID, DbType.Int32));
                                foundModifiedBy = true;
                            }
                        } else if (tblFld.IsModifiedDate) {
                            if (!foundModifiedAt) {
                                // add to set clause!!!
                                setClause.Append(tblFld.TableFieldName).Append(" = :").Append(tblFld.TableFieldName).Append(", ");
                                dpSet.Add(new DataParameter(":" + tblFld.TableFieldName, DateTime.Now.ToUniversalTime(), DbType.DateTime2));
                                foundModifiedAt = true;
                            }
                        }
                    }
                }
            }
            #endregion apply audits as needed

            #region finalize sql statement
            if (setClause.Length > 2) {
                setClause.Remove(setClause.Length - 2, 2);
            }
            if (whereClause.Length > 5) {
                whereClause.Remove(whereClause.Length - 5, 5);
            }



            DataParameters dps = new DataParameters();
            dps.AddRange(dpSet);
            dps.AddRange(dpWhere);

            string sql = "update " + fmt.TableName + " set " + setClause.ToString() + " where " + whereClause.ToString();
            DataCommand cmd = new DataCommand(sql, dps);
            return cmd;
            #endregion finalize sql statement

        }

        /// <summary>
        /// Generates a database engine-agnostic INSERT command for the given inputs
        /// </summary>
        /// <param name="fmdv"></param>
        /// <param name="fmt"></param>
        /// <param name="dr"></param>
        /// <param name="cooperatorID"></param>
        /// <param name="dm"></param>
        /// <param name="saveOptions"></param>
        /// <returns></returns>
        private DataCommand createInsertCommand(IDataview fmdv, ITable fmt, DataRow dr, int cooperatorID, DataManager dm, SaveOptions saveOptions) {

            #region scrub inputs / initialization
            if (fmt.Mappings.Count < 1) {
                return null;
            }

            if (saveOptions.InsertOnlyLanguageData && !fmt.TableName.ToLower().EndsWith("_lang")) {
                // client specified option of inserting data only if it is going to a language-based table.
                // this table is not, so we don't need to investigate any further.
                return null;
            }


            StringBuilder cols = new StringBuilder();
            StringBuilder prms = new StringBuilder();

            DataParameters dps = new DataParameters();

            bool foundCreatedBy = false;
            bool foundCreatedDate = false;

            bool foundOwnedBy = false;
            bool foundOwnedDate = false;

            bool foundFieldToSave = false;

            var missingRequiredFields = new List<IField>();
            var providedOptionalFields = new List<IField>();

            string[] ukFields = fmt.UniqueKeyFields.Split(',', ' ');


            cols.Append(" (");

            //            bool isMainTableForDataView = false;

            #endregion scrub inputs / initialization

            #region determine ownership

            // ownership is assigned one of these values (most significant last):
            // 1. current user's cooperator_id
            // 2. owner of 'parent row', if any.  i.e. sys_table_relationship contains a 'OWNER_PARENT' record for the current table.
            // 3. caller-specified owner (saveOptions.OwnerID).  caller must be member of 'ADMINS' group for this to be used.

            var ownedByCooperatorID = cooperatorID;
            string ownerTableName = null;
            IField fkToOwnerParent = null;
            DataParameter ownedByDataParameter = null;

            // look up owner parent, if any
            // if one is found, use its cooperatorID instead of ours
            DataSet ds = new DataSet();
            listRelatedTables(0, fmt.TableID, 0, new string[] { "OWNER_PARENT" }, dm, ds);
            var dt = ds.Tables["list_related_tables"];
            if (dt.Rows.Count > 0) {
                // ok, we know there is a owner parent defined for this table.
                // pull the owner record
                ownerTableName = dt.Rows[0]["table_name"].ToString();
            }
            #endregion determine ownership

            #region process fields
            for (var j = 0; j < fmt.Mappings.Count; j++) {
                var fm = fmt.Mappings[j];

                // if this is a standalone fieldmappingtable, just use dataviewfieldname = tablefieldname.
                if (fmdv == null) {
                    // this is a direct table mapping.
                    // copy over dataviewfieldname to tablefieldname.
                    fm.DataviewName = fm.TableName;
                    fm.DataviewFieldName = fm.TableFieldName;
                } else {
                    fm = Toolkit.Coalesce(fmdv.GetField(fm.TableFieldID, fmt.AliasName), fm) as IField;
                }


                if (!String.IsNullOrEmpty(ownerTableName) && fm.TableFieldName.ToLower() == ownerTableName + "_id") {
                    fkToOwnerParent = fm;
                    if (saveOptions.UseUniqueKeys) {
                        fkToOwnerParent.DataviewFieldName = fm.AliasedTableFieldName;// fm.TableFieldName;
                    } else {
                        fkToOwnerParent.DataviewFieldName = fm.TableFieldName;
                    }
                }




                bool addField = false;
                var fieldName = string.Empty;

                if (!String.IsNullOrEmpty(fm.DataviewFieldName)) {
                    if (dr.Table.Columns.Contains(fm.DataviewFieldName)) {
                        // valid dataview field, add it to the command
                        addField = true;
                        fieldName = fm.DataviewFieldName;
                    } else if (dr.Table.Columns.Contains(fm.AliasedTableFieldName)) {
                        addField = true;
                        fieldName = fm.AliasedTableFieldName;
                    } else {
                        foreach (DataColumn dc in dr.Table.Columns) {
                            if (dc.ColumnName.ToLower().EndsWith("." + fm.DataviewFieldName)) {
                                addField = true;
                                fieldName = dc.ColumnName;
                                break;
                            }
                        }
                    }
                } else if (String.IsNullOrEmpty(fm.DataviewFieldName)) {


                    var idx = dr.Table.Columns.IndexOf(fm.AliasedTableFieldName);
                    if (idx > -1) {
                        // found exact alias name in column list. use it.
                        fieldName = fm.AliasedTableFieldName;
                        if (idx >= fmdv.Mappings.Count) {
                            addField = true;
                        }
                    } else if (dr.Table.Columns.Contains(fm.TableFieldName) && fmt.AliasName == fm.Table.AliasName) {
                        // field name matches and same table alias. use it.
                        fieldName = fm.TableFieldName;

                        idx = dr.Table.Columns.IndexOf(fm.TableFieldName);
                        if (idx >= fmdv.Mappings.Count) {
                            addField = true;
                        }
                    } else {
                        // last ditch effort. match on partial tablefieldname or join_children...
                        foreach (DataColumn dc in dr.Table.Columns) {
                            //if (dc.ColumnName.ToLower().EndsWith("." + fm.TableFieldName)) {
                            //    // column name ends with same field name, assume close enough.
                            //    addField = true;
                            //    fieldName = dc.ColumnName;
                            //    break;
                            //} else {
                            var jclist = getJoinChildren(dc);
                            if (jclist.Contains(fm.AliasedTableFieldName)) {
                                if (dc.DataType == fm.DataType) {
                                    addField = true;
                                    fieldName = dc.ColumnName;
                                }
                            }
                            //}
                        }

                        if (String.IsNullOrEmpty(fieldName)) {
                            var f = getFieldSaveName(fm, dr, saveOptions);
                            if (!String.IsNullOrEmpty(f)){
                                addField = true;
                                fieldName = f;
                            }
                        }

                    }
                }

                if (fm.IsNullable) {
                    if (!fm.IsAudit) {
                        if (!String.IsNullOrEmpty(fieldName)) {
                            //                            if (dr[fieldName] != DBNull.Value) {
                            providedOptionalFields.Add(fm);
                            //                            }
                        }
                    }
                } else {
                    // this field is required, so we make sure we know if it's filled or not
                    missingRequiredFields.Add(fm);
                }

                if (fm.IsAutoIncrement || fm.IsAudit || fm.IsReadOnly) {
                    // auto-increment fields, audit fields, never have to be specified
                    if (!ukFields.Contains(fm.TableFieldName)) {
                        missingRequiredFields.Remove(fm);
                    }
                }



                if (addField) {


                    object current = dr[fieldName, DataRowVersion.Current];
                    DbType dbType = DbType.String;
                    if (fm.DataType != null) {
                        dbType = DataParameter.MapDbType(fm.DataType);
                    } else {
                        dbType = DataParameter.DeriveDbType(current);
                    }

                    if (fm.IsPrimaryKey && !fm.IsAutoIncrement) {
                        // allow them to insert the pk value for non-autoincrement fields
                        cols.Append(fm.TableFieldName).Append(", ");
                        prms.Append(":").Append(fm.TableFieldName).Append(", ");
                        dps.Add(new DataParameter(":" + fm.TableFieldName, current, dbType));

                        if (current != null && current != DBNull.Value) {
                            missingRequiredFields.Remove(fm);
                        }

                    } else if (!fm.IsReadOnly || ukFields.Contains(fm.TableFieldName)) {

                        if (saveOptions.BoolDefaultIsFalse) {
                            if (fm.GuiHint == "TOGGLE_CONTROL" || fm.TableFieldName.ToLower().StartsWith("is_")) {
                                if ((current + string.Empty) == string.Empty) {
                                    current = "N";
                                }
                            }
                        }

                        var defaultValue = fm.DefaultValue;
                        if (defaultValue as string == "{DBNull.Value}") {
                            defaultValue = DBNull.Value;
                        }
                        if (current != null) {

                            cols.Append(fm.TableFieldName).Append(", ");
                            prms.Append(":").Append(fm.TableFieldName).Append(", ");
                            dps.Add(new DataParameter(":" + fm.TableFieldName, current, dbType));

                            if (current != DBNull.Value) {
                                missingRequiredFields.Remove(fm);
                            }

                            //if (providedOptionalFields.Contains(fm)) {
                                if (!saveOptions.UseUniqueKeys || current.ToString() != defaultValue.ToString() || ukFields.Contains(fm.TableFieldName)) {
                                    foundFieldToSave = true;
                                }
                            //}
                        }

                    }

                } else if (fm.DefaultValue != null && fm.DefaultValue != DBNull.Value) {

                    // this is a field in the table, but the dataview does not have a field to represent it.
                    // it also has a default value associated with it, so add it to the list of fields to insert 

                    DbType dbTypeDefault = DataParameter.DeriveDbType(fm.DefaultValue);
                    if (dbTypeDefault == DbType.String && fm.DefaultValue.ToString() == "{DBNull.Value}") {
                        // should be dbnull. just ignore.
                    } else {
                        // add default value for field that is not part of the dataview
                        cols.Append(fm.TableFieldName).Append(", ");
                        prms.Append(":").Append(fm.TableFieldName).Append(", ");
                        dps.Add(new DataParameter(":" + fm.TableFieldName, fm.DefaultValue, dbTypeDefault));
                        //foundFieldToSave = true;

                        if (fm.DefaultValue != DBNull.Value) {
                            missingRequiredFields.Remove(fm);
                        }

                    }

                }

                // check for created date/by
                if (fmt.AuditsCreated) {
                    if (fm.IsCreatedDate) {
                        // apply create time stamp automatically
                        cols.Append(fm.TableFieldName).Append(", ");
                        prms.Append(":").Append(fm.TableFieldName).Append(", ");
                        dps.Add(new DataParameter(":" + fm.TableFieldName, DateTime.Now.ToUniversalTime(), DbType.DateTime2));
                        foundCreatedDate = true;
                    } else if (fm.IsCreatedBy) {
                        // apply created by user automatically
                        cols.Append(fm.TableFieldName).Append(", ");
                        prms.Append(":").Append(fm.TableFieldName).Append(", ");
                        dps.Add(new DataParameter(":" + fm.TableFieldName, cooperatorID, DbType.Int32));
                        foundCreatedBy = true;
                    }
                }

                // check for owned date/by
                if (fmt.AuditsOwned) {
                    if (fm.IsOwnedDate) {
                        // apply owned time stamp automatically
                        cols.Append(fm.TableFieldName).Append(", ");
                        prms.Append(":").Append(fm.TableFieldName).Append(", ");
                        dps.Add(new DataParameter(":" + fm.TableFieldName, DateTime.Now.ToUniversalTime(), DbType.DateTime2));
                        foundOwnedDate = true;
                    } else if (fm.IsOwnedBy) {
                        // apply owned by user automatically
                        cols.Append(fm.TableFieldName).Append(", ");
                        prms.Append(":").Append(fm.TableFieldName).Append(", ");

                        if (saveOptions.OwnerID != cooperatorID) {
                            // caller gave us a specific owner id, and they must be an administrator (otherwise the option would not be set to something different than our own cooperator id)
                            // that overrides whatever we've calculated.
                            ownedByDataParameter = new DataParameter(":" + fm.TableFieldName, saveOptions.OwnerID, DbType.Int32);
                        } else {
                            ownedByDataParameter = new DataParameter(":" + fm.TableFieldName, cooperatorID, DbType.Int32);
                        }

                        dps.Add(ownedByDataParameter);
                        foundOwnedBy = true;
                    }
                }
            }
            #endregion process fields

            #region scrub after field processing
            if (!foundFieldToSave) {
                // no field we need to save. skip!
                return null;
            } else {
                if (dps.Count == 0 || missingRequiredFields.Count > 0) {

                    if (missingRequiredFields.Count > 0) {

                        var writableMissing = new List<IField>();
                        foreach (var mrf in missingRequiredFields) {
                            if (!mrf.IsReadOnly && !mrf.IsReadOnlyOnInsert){
                                writableMissing.Add(mrf);
                            }
                        }


                        if (writableMissing.Count > 0) {
                            if (saveOptions.OnlyIfAllRequiredFieldsExist) {
                                // we're missing required fields, but the caller said to save only if they're all there.
                                // if they didn't give us any values for optional fields, assume we shouldn't save to this table.
                                if (areAllNullInInsertCommand(providedOptionalFields, dr, saveOptions)) {
                                    // all the provided optional fields that are nullable contain a null value.
                                    // this means don't use fields with NULL values as a reason to try to create 
                                    // a record for this table (since it's missing some required fields)
                                    return null;
                                }
                            }



                            var rf = new List<String>();
                            foreach (var mf in missingRequiredFields) {
                                rf.Add(mf.FriendlyFieldName + " (" + mf.TableName + " - " + mf.AliasedTableFieldName + ")" );
                            }

                            throw Library.CreateBusinessException(getDisplayMember("createInsertCommand{missingvalues}", "Cannot insert into table '{0}' because the following fields are missing values: {1}", fmt.TableName, String.Join(", ", rf.ToArray())));
                        }
                    } else {
                        // no need to run this insert, no data to write.
                        return null;
                    }
                }
            }
            #endregion scrub after field processing

            #region apply audits as needed
            // we're inserting -- some inserting requires created by and when (based on sys_table.audits_created). 
            // if it's not in the dataview passed to us, look it up
            if (fmt.AuditsCreated && (!foundCreatedBy || !foundCreatedDate)) {
                //			if (!foundCreatedBy || !foundCreatedDate) {
                // look up their mappings based on the actual table name

                var fmt2 = Dataview.MapFirst(fmt.TableName, _languageID, dm);
                if (fmt2 != null) {
                    foreach (var fm2 in fmt2.Mappings) {
                        if (fm2.IsCreatedBy) {
                            if (!foundCreatedBy) {
                                cols.Append(fm2.TableFieldName).Append(", ");
                                prms.Append(":").Append(fm2.TableFieldName).Append(", ");
                                dps.Add(new DataParameter(":" + fm2.TableFieldName, cooperatorID, DbType.Int32));
                                foundCreatedBy = true;
                            }
                        } else if (fm2.IsCreatedDate) {
                            if (!foundCreatedDate) {
                                // apply create time stamp automatically
                                cols.Append(fm2.TableFieldName).Append(", ");
                                prms.Append(":").Append(fm2.TableFieldName).Append(", ");
                                dps.Add(new DataParameter(":" + fm2.TableFieldName, DateTime.Now.ToUniversalTime(), DbType.DateTime2));
                                foundCreatedDate = true;
                            }

                        }
                    }
                }

                if (!foundCreatedBy || !foundCreatedDate) {
                    throw Library.CreateBusinessException(getDisplayMember("createInsertCommand{missingcreateaudit}", "Could not resolve created at or created by in sys_dataview_field or sys_table_field for table = {0}", fmt.TableName));
                }

            }

            // we're inserting -- some inserting requires owned by and when (based on sys_table.audits_owned). 
            // if it's not in the dataview passed to us, look it up
            if (fmt.AuditsOwned && (!foundOwnedBy || !foundOwnedDate)) {
                //			if (!foundOwnedBy || !foundOwnedDate) {
                // look up their mappings based on the actual table name

                var fmt3 = Dataview.MapFirst(fmt.TableName, _languageID, dm);
                if (fmt3 != null) {
                    foreach (var fm3 in fmt3.Mappings) {
                        if (fm3.IsOwnedBy) {
                            if (!foundOwnedBy) {
                                cols.Append(fm3.TableFieldName).Append(", ");
                                prms.Append(":").Append(fm3.TableFieldName).Append(", ");
                                dps.Add(new DataParameter(":" + fm3.TableFieldName, cooperatorID, DbType.Int32));
                                foundOwnedBy = true;
                            }
                        } else if (fm3.IsOwnedDate) {
                            if (!foundOwnedDate) {
                                // apply create time stamp automatically
                                cols.Append(fm3.TableFieldName).Append(", ");
                                prms.Append(":").Append(fm3.TableFieldName).Append(", ");
                                dps.Add(new DataParameter(":" + fm3.TableFieldName, DateTime.Now.ToUniversalTime(), DbType.DateTime2));
                                foundOwnedDate = true;
                            }

                        }
                    }
                }

                if (!foundOwnedBy || !foundOwnedDate) {
                    throw Library.CreateBusinessException(getDisplayMember("createInsertCommand{missingownedaudit}", "Could not resolve owned date or owned by in sys_dataview_field or sys_table_field for table = {0}", fmt.TableName));
                }

            }


            // if there is a parent owner defined, and options didn't override the owner to be someone other than the current user
            if (saveOptions.OwnerID == CooperatorID) {
                if (ownedByDataParameter != null && fkToOwnerParent != null && !String.IsNullOrEmpty(ownerTableName)) {
                    // use owner of parent record as owner for this record (default to current user as owner)

                    // if the record does not contain the fk_id, we need to add it here!
                    if (!dr.Table.Columns.Contains(fkToOwnerParent.DataviewFieldName)) {
                        var dcFK = new DataColumn(fkToOwnerParent.DataviewFieldName, fkToOwnerParent.DataType);
                        dcFK.ExtendedProperties["table_alias_name"] = fkToOwnerParent.Table.AliasName.ToLower();
                        dcFK.ExtendedProperties["table_field_name"] = fkToOwnerParent.TableFieldName.ToLower();
                        dr.Table.Columns.Add(dcFK);
                    }

                    var fkValue = Toolkit.ToInt32(dr[fkToOwnerParent.DataviewFieldName, DataRowVersion.Current], -1);
                    if (fkValue > -1) {
                        ownedByDataParameter.Value = Toolkit.ToInt32(dm.ReadValue("select owned_by from " + ownerTableName + " where " + ownerTableName + "_id = " + fkValue), ownedByCooperatorID);
                    }
                }
            }
            #endregion apply audits as needed

            #region finalize sql statement

            if (cols.Length > 2) {
                cols.Remove(cols.Length - 2, 2);
            }
            if (prms.Length > 2) {
                prms.Remove(prms.Length - 2, 2);
            }

            cols.Append(") values (");
            prms.Append(")");


            string sql = "insert into " + fmt.TableName + cols.ToString() + prms.ToString();
            DataCommand cmd = new DataCommand(sql, dps);
            return cmd;
            #endregion finalize sql statement

        }

        /// <summary>
        /// Returns true if given DataColumn does not have a "join_children" key in ExtendedProperties dictionary
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        private bool missingJoinChildren(DataColumn dc) {
            return !dc.ExtendedProperties.ContainsKey("join_children");
        }

        /// <summary>
        /// Parses the "join_children" ExtendedProperty on the given DataColumn and returns a string array of the results.  Returns empty array if none exist.
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        private string[] getJoinChildren(DataColumn dc) {
            var rv = new string[0];
            if (dc.ExtendedProperties.ContainsKey("join_children")) {
                // if this field says one of its join children matches the exact alias, assume this is the right one.
                var jclist = dc.ExtendedProperties["join_children"] as string;
                if (!String.IsNullOrEmpty(jclist)) {
                    rv = jclist.Split(',');
                }
            }
            return rv;
        }

        /// <summary>
        /// Returns true if all values in dr are DBNull.Value for given list of providedOptionalFields.  Called only by createInsertCommand.
        /// </summary>
        /// <param name="providedOptionalFields"></param>
        /// <param name="dr"></param>
        /// <param name="saveOptions"></param>
        /// <returns></returns>
        private bool areAllNullInInsertCommand(List<IField> providedOptionalFields, DataRow dr, SaveOptions saveOptions) {
            var allAreNull = true;
            foreach (var pof in providedOptionalFields) {
                if (pof.IsNullable) {

                    var fn = getFieldSaveName(pof, dr, saveOptions);

                    if (!String.IsNullOrEmpty(fn) && dr[fn] != DBNull.Value) {
                        allAreNull = false;
                        break;
                    }
                }
            }
            return allAreNull;
        }

        /// <summary>
        /// Returns the ColumnName of the DataColumn in dr.Table which matches the given fld objec, based on fully aliased table field name.  Null if not found.
        /// </summary>
        /// <param name="fld"></param>
        /// <param name="dr"></param>
        /// <param name="saveOptions"></param>
        /// <returns></returns>
        private string getFieldSaveName(IField fld, DataRow dr, SaveOptions saveOptions) {
            // memoization...
            string fn = null;
            if (!saveOptions.CachedFieldNames.TryGetValue(fld, out fn)) {
                fn = fld.DataviewFieldName;
                if (String.IsNullOrEmpty(fn)) {
                    fn = fld.AliasedTableFieldName;
                    if (!dr.Table.Columns.Contains(fn)) {
                        fn = fld.TableFieldName;
                        if (!dr.Table.Columns.Contains(fn)) {
                            // what now?
                            fn = null;
                            //throw Library.CreateBusinessException("Could not determine field name...");
                        } else {
                            // this column has the same base name as the one we're looking for.  Make sure it matches on alias name as well...
                            var dc = dr.Table.Columns[fn];
                            if (fld.Table != null && dc.ExtendedProperties.ContainsKey("table_alias_name") && dc.ExtendedProperties["table_alias_name"].ToString() != fld.Table.AliasName) {
                                fn = null;
                            }
                        }
                    }
                }
                saveOptions.CachedFieldNames[fld] = fn;
            }
            return saveOptions.CachedFieldNames[fld];
        }

        /// <summary>
        /// Sets the saveMode parameter appropriately based on database values if saveOptions.UseUniqueKeys is specified.
        /// </summary>
        /// <param name="fmdv"></param>
        /// <param name="fmt"></param>
        /// <param name="dr"></param>
        /// <param name="dm"></param>
        /// <param name="saveMode"></param>
        /// <param name="saveOptions"></param>
        private void determineSaveMode(IDataview fmdv, ITable fmt, DataRow dr, DataManager dm, ref SaveMode saveMode, SaveOptions saveOptions) {
            // if the useUniqueKeys option is enabled, we may override saveMode
            // Note this is currently only true when called from Import Wizard.
            if (saveMode == SaveMode.Insert || saveMode == SaveMode.Update) {
                if (saveOptions.UseUniqueKeys) {

                    determinePrimaryKeyValues(fmdv, fmt, dr, dm, ref saveMode, saveOptions);

                }
            }

        }

        /// <summary>
        /// Depending on given saveMode, calls createInsertCommand, createDeleteCommand, or createUpdateCommand as needed to generate a DataCommand object.  If saveMode requires no DataCommand (i.e. is Unknown or None), returns null.
        /// </summary>
        /// <param name="fmdv"></param>
        /// <param name="fmt"></param>
        /// <param name="dr"></param>
        /// <param name="dm"></param>
        /// <param name="saveMode"></param>
        /// <param name="saveOptions"></param>
        /// <returns></returns>
        private DataCommand initSaveCommand(IDataview fmdv, ITable fmt, DataRow dr, DataManager dm, ref SaveMode saveMode, SaveOptions saveOptions) {

            switch (saveMode) {
                case SaveMode.Insert:
                    return createInsertCommand(fmdv, fmt, dr, _cooperatorID, dm, saveOptions);
                case SaveMode.Delete:
                    return createDeleteCommand(fmdv, fmt, dr, dm, saveOptions);
                case SaveMode.Update:
                    return createUpdateCommand(fmdv, fmt, dr, _cooperatorID, dm, saveOptions);
                default:
                    // nothing to do
                    return null;
            }
        }

        #endregion Save Request Processing

        #endregion Save Data

        #region ResolveUniqueKeys
        public DataSet ResolveUniqueKeys(DataSet ds, string options) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    // put code specific to the method here

                    foreach (DataTable dt in ds.Tables) {

                        var dtlow = dt.TableName.ToLower();
                        if (dtlow == "exceptiontable" || dtlow == "validate_login") {
                            // ignore, boilerplate table, no need to inspect
                        } else {

                            var saveOptions = new SaveOptions(options);
                            var dv = Dataview.Map(dt.TableName, saveOptions.AltLanguageID ?? _languageID, dm);


                            // create an empty return table, add it to the output
                            var clone = dt.Clone();
                            dsReturn.Tables.Add(clone);

                            var saveMode = SaveMode.Unknown;

                            // using ITable definition, spin through all the rows 
                            // and kick out primary key id values as needed
                            foreach (DataRow dr in dt.Rows) {
                                foreach (Table t in dv.Tables) {

                                    determinePrimaryKeyValues(dv, t, dr, dm, ref saveMode, saveOptions);

                                    // copy the altered row to the output
                                    clone.Rows.Add(dr.ItemArray);

                                }

                            }


                        }

                    }


                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, _languageID);
            }
            return dsReturn;
        }

        //private int _selectPKHits = 0;
        //private int _selectPKMisses = 0;


        /// <summary>
        /// Copies any join_children defined in the ConfigurationOptions on the fld object into the join_children ExtendedProperty on the dc object.
        /// </summary>
        /// <param name="fld"></param>
        /// <param name="dc"></param>
        private void addJoinChildrenExtendedProperties(IField fld, DataColumn dc){
            if (!String.IsNullOrEmpty(fld.ConfigurationOptions) && !dc.ExtendedProperties.ContainsKey("join_children")) {
                var kvs = Toolkit.ParsePairs<string>(fld.ConfigurationOptions);
                var fn = string.Empty;
                if (kvs.TryGetValue("join_children", out fn)) {
                    dc.ExtendedProperties["join_children"] = fn;
                }
            }
        }

        /// <summary>
        /// Using the unqiue key fields defined on the given fmt parameter, looks up the primary key from the database using unique key field values.  Only called within the context of UseUniqueKeys saveOption (aka Import Wizard)
        /// </summary>
        /// <param name="fmdv"></param>
        /// <param name="fmt"></param>
        /// <param name="dr"></param>
        /// <param name="dm"></param>
        /// <param name="saveMode"></param>
        /// <param name="saveOptions"></param>
        private void determinePrimaryKeyValues(IDataview fmdv, ITable fmt, DataRow dr, DataManager dm, ref SaveMode saveMode, SaveOptions saveOptions) {

            var drv = dr.RowState == DataRowState.Modified ? DataRowVersion.Original : DataRowVersion.Current;

            // ignore the rowstate value, determine the alt unique key fields and use them to determine
            // if save mode should be update or insert.
            var uniqueKeys = ("" + fmt.UniqueKeyFields).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            var missingUniqueKeys = uniqueKeys.ToList();
            if (uniqueKeys.Length == 0) {

                throw Library.CreateBusinessException(getDisplayMember("determinPrimaryKeyValues{noukdefined}", "No unique key is defined for table '{0}'.  This is required when saving data via a dataview without this table's primary key field included in it.", fmt.TableName));

            } else {

                var uniqueKeyFields = new List<IField>();

                for (var i = 0; i < uniqueKeys.Length; i++) {
                    var ukLower = uniqueKeys[i].ToLower();
                    foreach (var f in fmdv.Mappings) {
                        if (f.Table.AliasName.ToLower() == fmt.AliasName.ToLower()) {
                            var dvf = DataTriggerHelper.GetDataviewFieldName(dr.Table, f.Table.AliasName, f.TableFieldName);
                            if (!String.IsNullOrEmpty(dvf)) {
                                if (ukLower == f.TableFieldName.ToLower() || ukLower == dvf.ToLower()) {
                                    uniqueKeyFields.Add(f);
                                    missingUniqueKeys.Remove(uniqueKeys[i]);
                                }
                            }
                        }
                    }
                }

                if (missingUniqueKeys.Count > 0) {
                    // some fields may have been added to the datatable which are not part of the field mapping dataview
                    // (i.e. the pk will be added on a previous table in this fmdv that is part of the alternate key for this table.
                    // so we spin through the fields on the datarow itself to see...

                    appendMissingKeysIfPossible(uniqueKeyFields, missingUniqueKeys, dr, fmdv, fmt, saveOptions);

                }

                if (uniqueKeys.Length != uniqueKeyFields.Count) {
                    // not all alternate key fields are given. ignore.
                    saveMode = SaveMode.None;
                } else {
                    // all alternate key fields for this table are in the dataview, so it is safe to use



                    // so, we need to:
                    // 1) pull data by the alt key
                    // 2) None found, set savemode = Insert
                    // 3) Found something, remember its id, set savemode = Update



                    var pkField = fmt.GetField(fmt.PrimaryKeyFieldName);
                    if (pkField != null) {

                        object pkValue = null;


                        // determine the command we *might* run against the database
                        var cmd = generatePrimaryKeyValueSelectCommand(uniqueKeyFields, fmt, dr, drv, dm, saveOptions);
                        string idCacheKeyName = Crypto.HashText(cmd.ToString());


                        // see if the id for that is already cached
                        var idCacheName = (String.IsNullOrEmpty(fmt.AliasName) ? fmt.TableName : fmt.AliasName) + ":idCache";
                        Dictionary<string, object> idCache = null;
                        object idCacheHolder = null;
                        if (saveOptions.CachedTempData.TryGetValue(idCacheName, out idCacheHolder)) {
                            // the id cache exists, see if this particular key does as well...
                            idCache = idCacheHolder as Dictionary<string, object>;
                            idCache.TryGetValue(idCacheKeyName, out pkValue);
                        } else {
                            // id cache doesn't exist, create it
                            idCache = new Dictionary<string, object>();
                            saveOptions.CachedTempData[idCacheName] = idCache;
                        }


                        if (pkValue == null) {
                            // not cached, pull from database
                            var pk = dm.ReadValue(cmd);

                            if (pkField.DataType == typeof(int)) {
                                pkValue = Toolkit.ToInt32(pk, null);
                            } else if (pkField.DataType == typeof(string)) {
                                pkValue = pk as string;
                            } else {
                                throw Library.CreateBusinessException(getDisplayMember("determinePrimaryKeyValues{undefinedpktype}", "Primary key field '{0}' on table '{1}' has a type of '{2}', and that is not defined in GrinGlobal.Business.SecureData.determinePrimaryKeyValues().", pkField.TableFieldName, pkField.TableName, pkField.DataTypeString));
                            }
                        } else {
                            // Debug.WriteLine("id cache hit!");
                        }





                        if (pkValue != null) {

                            if (idCache.Keys.Count < 1000){
                                // haven't met our per-table id cache limit yet, add it
                                idCache[idCacheKeyName] = pkValue;
                            }



                            // found a row to update!!!!
                            if (!String.IsNullOrEmpty(pkField.DataviewFieldName)) {

                                var dc = dr.Table.Columns[pkField.DataviewFieldName];

                                // since neither the pk for this table nor the fk for the related table may be defined in the dataview,
                                // we store which table was joined to this one in the configuration_options on all fields in this table. so we just need to pluck any of them,
                                // copy their foreign_key_table_alias config option over to this pk.
                                // (note the heavy lifting is done by the dataview editor to set the configuration_options field appropriately in the first place)
                                addJoinChildrenExtendedProperties(uniqueKeyFields[0], dc);

                                if (dc.ReadOnly) {
                                    // temporarily disable readonly so we can put the id in
                                    dc.ReadOnly = false;
                                    dr[pkField.DataviewFieldName] = pkValue;
                                    dc.ReadOnly = true;
                                } else {
                                    dr[pkField.DataviewFieldName] = pkValue;
                                }
                            } else {
                                // that particular pk is not in our dataview table.
                                // so, let's add it! :)
                                DataColumn dc = null;
                                var newFieldName = pkField.AliasedTableFieldName;
                                if (!dr.Table.Columns.Contains(newFieldName)) {
                                    dc = new DataColumn(newFieldName, pkValue.GetType());
                                    dc.ExtendedProperties["table_alias_name"] = String.IsNullOrEmpty(pkField.Table.AliasName) ? pkField.Table.TableName : pkField.Table.AliasName;
                                    dc.ExtendedProperties["table_field_name"] = pkField.TableFieldName;
                                    dr.Table.Columns.Add(dc);
                                    //} else {
                                    //    dc = dr.Table.Columns[newFieldName];
                                    //    if (dc.ExtendedProperties["table_alias_name"].ToString().ToLower() != pkField.Table.AliasName.ToLower()) {
                                    //        newFieldName = pkField.AliasedTableFieldName;
                                    //        dc = new DataColumn(newFieldName, pkValue.GetType());
                                    //        dc.ExtendedProperties["table_alias_name"] = pkField.Table.AliasName;
                                    //        dc.ExtendedProperties["auto_appended"] = true;
                                    //        dr.Table.Columns.Add(dc);
                                    //    }
                                } else {
                                    dc = dr.Table.Columns[newFieldName];
                                }


                                // since neither the pk for this table nor the fk for the related table may be defined in the dataview,
                                // we store which table was joined to this one in the configuration_options on all fields in this table. so we just need to pluck any of them,
                                // copy their foreign_key_table_alias config option over to this pk.
                                // (note the heavy lifting is done by the dataview editor to set the configuration_options field appropriately in the first place)
                                addJoinChildrenExtendedProperties(uniqueKeyFields[0], dc);

                                if (dc.ReadOnly) {
                                    // temporarily disable readonly so we can put the id in
                                    dc.ReadOnly = false;
                                    dr[newFieldName] = pkValue;
                                    dc.ReadOnly = true;
                                } else {
                                    dr[newFieldName] = pkValue;
                                }

                            }

                            saveMode = SaveMode.Update;
                            // since the pk and fk are not required to be in the dataview, we need to check all other tables 'above' this one
                            // to see if any of them contain configurationoptions saying an optional fk exists and we wrote a record for it already.
                            // (think taxonomy_species.priority1_site_id and site.site_id -- need to set the value properly...)
                            determineParentValue(fmdv, fmt, dr, drv, saveOptions);

                        } else {

                            // no row found, mark it as insert if at least something is specified
                            var allNull = true;
                            foreach (var f in uniqueKeyFields) {
                                if (dr[f.DataviewFieldName] != DBNull.Value) {
                                    allNull = false;
                                    break;
                                }
                            }

                            if (allNull) {
                                // they gave us nada.  do nada.
                                saveMode = SaveMode.None;
                            } else {
                                saveMode = SaveMode.Insert;
                                // since the pk and fk are not required to be in the dataview, we need to check all other tables 'above' this one
                                // to see if any of them contain configurationoptions saying an optional fk exists and we wrote a record for it already.
                                // (think taxonomy_species.priority1_site_id and site.site_id -- need to set the value properly...)
                                determineParentValue(fmdv, fmt, dr, drv, saveOptions);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates a SELECT command with appropriate parameters to pull the primary key value given the unique key fields.  Does not run the command, only generates it.  Uses caching.
        /// </summary>
        /// <param name="uniqueKeyFields"></param>
        /// <param name="fmt"></param>
        /// <param name="dr"></param>
        /// <param name="drv"></param>
        /// <param name="dm"></param>
        /// <param name="saveOptions"></param>
        /// <returns></returns>
        private DataCommand generatePrimaryKeyValueSelectCommand(List<IField> uniqueKeyFields, ITable fmt, DataRow dr, DataRowVersion drv, DataManager dm, SaveOptions saveOptions) {

            DataCommand cmd = null;

            // memoization...
            object cacheValue = null;
            var cacheKey = (String.IsNullOrEmpty(fmt.AliasName) ? fmt.TableName : fmt.AliasName) + ":gPKVSC:";

            if (saveOptions.CachedTempData.TryGetValue(cacheKey, out cacheValue)) {
                cmd = cacheValue as DataCommand;
            } else {

                var whereFields = new List<string>();
                var whereParams = new DataParameters();
                var offset = 0;
                foreach (var f in uniqueKeyFields) {
                    whereFields.Add(generateWhereFieldAndParameter(f, dr[f.DataviewFieldName, drv], dm, ref offset, whereParams));
                }

                var whereText = String.Join(" AND ", whereFields.ToArray());

                var altSql = String.Format(@"
select 
    {0}
from
    {1}
where
    {2}", fmt.PrimaryKeyFieldName, fmt.TableName, whereText);

                cmd = new DataCommand(altSql, whereParams);

                saveOptions.CachedTempData[cacheKey] = cmd;

            }


            for(var i=0;i<cmd.DataParameters.Count;i++){
                cmd.DataParameters[i].Value = dr[uniqueKeyFields[i].DataviewFieldName, drv];
            }

            return cmd;

        }

        //private int _appendMissingKeysIfPossibleHits = 0;
        //private int _appendMissingKeysIfPossibleMisses = 0;

        /// <summary>
        /// Given a list of missingUniqueKeys, appends new DataColumn objects to the given DataRow (dr) if possible.
        /// </summary>
        /// <param name="uniqueKeyFields"></param>
        /// <param name="missingUniqueKeys"></param>
        /// <param name="dr"></param>
        /// <param name="fmdv"></param>
        /// <param name="fmt"></param>
        /// <param name="saveOptions"></param>
        private void appendMissingKeysIfPossible(List<IField> uniqueKeyFields, List<string> missingUniqueKeys, DataRow dr, IDataview fmdv, ITable fmt, SaveOptions saveOptions) {
            // memoization
#if APMK_CACHING
            var cacheKey = fmdv.DataViewName + "-" + fmt.AliasName + ":aMKIPM:";
            object cacheValue = null;
            if (saveOptions.CachedTempData.TryGetValue(cacheKey, out cacheValue)){


                //_appendMissingKeysIfPossibleHits++;
                //Debug.WriteLine("appendMissingKeysIfPossibleHits=" + _appendMissingKeysIfPossibleHits); 

                // set sys_lang_id if need be, everything else is already cached...
                if (Toolkit.ToBoolean(dr.Table.ExtendedProperties["added_sys_lang_id"], false)) {
                    dr["sys_lang_id"] = saveOptions.AltLanguageID ?? LanguageID;
                }
            } else {
#endif
            //_appendMissingKeysIfPossibleMisses++;
            //    Debug.WriteLine("appendMissingKeysIfPossibleMisses=" + _appendMissingKeysIfPossibleMisses); 
                foreach (var mak in missingUniqueKeys) {
                    var found = false;
                    var makLower = mak.ToLower();
                    var aliasLower = fmt.AliasName.ToLower();
                    var aliasMakLower = aliasLower + "." + makLower;
                    foreach (DataColumn dc in dr.Table.Columns) {
                        if (String.Compare(dc.ColumnName, makLower, true) == 0 || dc.ColumnName.ToLower() == aliasMakLower) {
                            // example case... import_accession_with_pedigree  ap.accession_id
                            var dvf = fmt.GetField(mak);
                            dvf.DataviewFieldName = dc.ColumnName;
                            if (dr[dvf.DataviewFieldName] != DBNull.Value || dvf.IsNullable) {
                                uniqueKeyFields.Add(dvf);
                                if (Toolkit.ToBoolean(dr.Table.ExtendedProperties["added_sys_lang_id"], false)) {
                                    dr["sys_lang_id"] = saveOptions.AltLanguageID ?? LanguageID;
                                }
                                found = true;
                                break;
                            }
                        }
                    }

                    if (!found) {
                        // could not find by the exact alias name. check any that end with same table field name.
                        foreach (DataColumn dc in dr.Table.Columns) {
                            if (String.Compare(dc.ColumnName, mak, true) == 0 || dc.ColumnName.ToLower().EndsWith("." + makLower)) {

                                // possibly might match.  check join children.
                                var joined = getJoinChildren(dc);
                                if (joined.Contains(aliasMakLower)) {
                                    // example case... import_cooperator   g.geography_id
                                    var dvf = fmt.GetField(mak);
                                    dvf.DataviewFieldName = dc.ColumnName;
                                    if (dr[dvf.DataviewFieldName] != DBNull.Value || dvf.IsNullable) {
                                        uniqueKeyFields.Add(dvf);
                                        if (Toolkit.ToBoolean(dr.Table.ExtendedProperties["added_sys_lang_id"], false)) {
                                            dr["sys_lang_id"] = saveOptions.AltLanguageID ?? LanguageID;
                                        }
                                        found = true;
                                        break;
                                    }
                                } else if (missingJoinChildren(dc)){
                                    // no join_children extendedproperty.  assume it's the one to use?
                                    var dvf = fmt.GetField(mak);
                                    dvf.DataviewFieldName = dc.ColumnName;
                                    if (dr[dvf.DataviewFieldName] != DBNull.Value || dvf.IsNullable) {
                                        uniqueKeyFields.Add(dvf);
                                        if (Toolkit.ToBoolean(dr.Table.ExtendedProperties["added_sys_lang_id"], false)) {
                                            dr["sys_lang_id"] = saveOptions.AltLanguageID ?? LanguageID;
                                        }
                                        found = true;
                                        break;
                                    }
                                }

                            }
                        }
                    }

                    // HACK! include current language if the missing field is sys_lang_id.
                    if (!found && (mak.ToLower().EndsWith(".sys_lang_id") || mak.ToLower() == "sys_lang_id")) {
                        var langField = fmt.GetField(mak);
                        langField.DataviewFieldName = mak;
                        uniqueKeyFields.Add(langField);
                        if (!dr.Table.Columns.Contains("sys_lang_id")) {
                            dr.Table.Columns.Add("sys_lang_id", typeof(int));
                            dr.Table.ExtendedProperties["added_sys_lang_id"] = true;
                        }
                        dr["sys_lang_id"] = saveOptions.AltLanguageID ?? this.LanguageID;
                    }
                }
#if APMK_CACHING
                // just set a flag saying we already did the processing...
                saveOptions.CachedTempData[cacheKey] = true;
            }
#endif
        }

        //private int _dpvHits = 0;
        //private int _dpvMisses = 0;
        /// <summary>
        /// In dataviews with multiple related tables joined in, there is a hierachy defined. (X before Y before Z).  If table Y needs a pk value from its parent (table X), this method determines which field that is and copies its value to the appropriate table Y field in the DataRow.
        /// </summary>
        /// <param name="fmdv"></param>
        /// <param name="fmt"></param>
        /// <param name="dr"></param>
        /// <param name="drv"></param>
        /// <param name="saveOptions"></param>
        private void determineParentValue(IDataview fmdv, ITable fmt, DataRow dr, DataRowVersion drv, SaveOptions saveOptions) {

            List<string> parentColumnNames = null;
            List<string> tableFieldNames = null;

            // memoization...
            var cacheKey = fmdv.DataViewName + "-" + (String.IsNullOrEmpty(fmt.AliasName) ? fmt.TableName : fmt.AliasName) + ":dPV:";
            object cacheValue = null;
            if (saveOptions.CachedTempData.TryGetValue(cacheKey, out cacheValue)){

                parentColumnNames = cacheValue as List<string>;
                tableFieldNames = saveOptions.CachedTempData[cacheKey + ":tf"] as List<string>;

                //Debug.WriteLine("dpvHits=" + _dpvHits++);

            } else {


                parentColumnNames = new List<string>();
                tableFieldNames = new List<string>();

                //Debug.WriteLine("dpvmisses=" + _dpvMisses++);
                var tfnames = string.Empty;
                foreach (IField f in fmdv.Mappings) {
                    var pkLower = f.Table.PrimaryKeyFieldName.ToLower();
                    var pkAliasLower = (String.IsNullOrEmpty(f.Table.AliasName) ? f.Table.TableName : f.Table.AliasName).ToLower() + "." + pkLower;
                    var kvs = Toolkit.ParsePairs<string>(f.ConfigurationOptions);
                    if (kvs.TryGetValue("join_children", out tfnames)) {

                        var tflist = tfnames.Split(',');

                        foreach (var tfn in tflist) {


                            var arr = tfn.ToLower().Split('.');
                            if (arr.Length == 2) {
                                var childTableName = arr[0].ToLower();
                                if (childTableName == fmt.TableName.ToLower() || childTableName == fmt.AliasName.ToLower()) {
                                    // this guy points at our table.
                                    // pull its pk value from the datarow and set it to the given field.
                                    object parentVal = null;
                                    Type parentType = null;
                                    var found = false;
                                    foreach (DataColumn dcp in dr.Table.Columns) {
                                        if (dcp.ColumnName.ToLower() == pkLower || dcp.ColumnName.ToLower() == pkAliasLower) {
                                            if (!parentColumnNames.Contains(dcp.ColumnName)) {
                                                parentColumnNames.Add(dcp.ColumnName);
                                            }
                                            parentVal = dr[dcp.ColumnName, drv];
                                            parentType = dr.Table.Columns[dcp.ColumnName].DataType;
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (found) {
                                        if (!dr.Table.Columns.Contains(tfn)) {
                                            var dcNew = new DataColumn(tfn, parentType);
                                            dcNew.ExtendedProperties["table_alias_name"] = arr[0];
                                            dcNew.ExtendedProperties["table_field_name"] = tfn;
                                            dr.Table.Columns.Add(dcNew);
                                        }
                                        if (!tableFieldNames.Contains(tfn)) {
                                            tableFieldNames.Add(tfn);
                                        }
                                        dr[tfn] = parentVal;
                                    }
                                }
                            }
                        }
                    }
                }

                saveOptions.CachedTempData[cacheKey] = parentColumnNames;
                saveOptions.CachedTempData[cacheKey + ":tf"] = tableFieldNames;
            }

            for (var i = 0; i < parentColumnNames.Count; i++) {
                dr[tableFieldNames[i]] = dr[parentColumnNames[i], drv];
            }

        }

        #endregion ResolveUniqueKeys


        #region Get Data

        /// <summary>
        /// Runs SQL associated with the given dataview and either returns its results in a DataSet or (if streamWriter parameter is not null) writes the results to a stream
        /// </summary>
        /// <param name="dic">A dictionary of parameters to pass to the dataview</param>
        /// <param name="dataviewNameOrSql">Name of the dataview (sys_dataview.dataview_name)</param>
        /// <param name="offset">Number of records to skip.  Typically 0.</param>
        /// <param name="limit">Number of records to return.  If &lt; 1, returns all.</param>
        /// <param name="streamWriter">The object to stream results to instead of returning them as a dataset.  Useful for emitting large amounts of data (such as via StreamData.aspx) without tying up too many resources on the server</param>
        /// <param name="streamFormat">The format of the data to write to the stream.  Valid values are "csv", "tab", "json". "csv" is the assumed default if any other value is given. Ignored if streamWriter parameter is null</param>
        /// <param name="prettyColumns">True to emit column names as the language-specific title, False to emit column names as they are returned from the SQL statement.  Applicable only to streamed data (i.e. ignored if streamWriter parameter is null)</param>
        /// <param name="dsReturn">DataSet containing results of running the SQL associated with the given dataview.  Will be null if streamWriter parameter is not null (and data is therefore streamed to that object instead)</param>
        /// <param name="getOptions">Options affecting the output of the data.  See the GrinGlobal.Interface.ReadOptions class for more information.</param>
        /// <returns></returns>
        private DataSet getData(Dictionary<string, object> dic, string dataviewNameOrSql, int offset, int limit, StreamWriter streamWriter, string streamFormat, bool prettyColumns, DataSet dsReturn, ReadOptions getOptions) {

            Debug.WriteLine(dataviewNameOrSql);
            if (dsReturn == null) {
                dsReturn = createReturnDataSet();
            }

            IDataview fmdv = null;

            // caller may override the LanguageID, remember that preference for later in this method
            var altLanguageID = getOptions.AltLanguageID ?? LanguageID;

            try {
                using (DataManager dm = BeginProcessing(true)) {

                    #region scrub inputs / initialization
                    string sql = null;
                    DataParameters dps = new DataParameters();
                    string tableName = null;


                    IReadDataTriggerArgs readArgs = null;



                    // scrub method parameters

                    if (String.IsNullOrEmpty(dataviewNameOrSql)) {
                        throw Library.CreateBusinessException(getDisplayMember("getData{missingdvname}", "A dataview name is required.  Null or zero-length string is not allowed."));
                    }

                    // get sql / titles / params / etc from dataview and table mappings (in sys_* tables)
                    fmdv = Dataview.Fill(dataviewNameOrSql.ToLower(), altLanguageID, dm) as IDataview;

                    if (fmdv == null) {
                        throw Library.CreateBusinessException(getDisplayMember("getData{undefineddv}", "No dataview named '{0}' exists in the sys_dataview table.  Please verify both your dataview name and the contents of the sys_dataview table.", dataviewNameOrSql ));
                    }

                    readArgs = new ReadDataTriggerArgs {
                        FieldMappingDataView = fmdv,
                        DataManager = dm,
                        SecureData = this
                    };

                    // remember the sql for this dataview / engine
                    sql = fmdv.SqlStatementForCurrentEngine;
                    tableName = fmdv.DataViewName;

                    // inspect given parameters, make note of which ones are missing, bomb if there are any missing
                    StringBuilder sb = new StringBuilder();
                    foreach (DataviewParameter rp in fmdv.Parameters) {
                        if (!dic.ContainsKey(rp.Name)) {
                            sb.Append(rp.Name).Append("\t").Append(rp.TypeName.ToString()).Append("\r\n");
                        } else {

                            dps.Add(new DataParameter(rp.Name, dic[rp.Name], DataParameter.MapDbType(rp.TypeName), DataParameter.MapDbPseudoType(rp.TypeName)));

                        }
                    }
                    if (sb.Length > 0) {
                        throw Library.CreateBusinessException(getDisplayMember("getData{missingparameters}", "The following sql parameters were not passed when querying for dataview '{0}':\r\n" + "Name\tType\r\n{1}",fmdv.DataViewName, sb.ToString()));
                    }

                    // scrub sql for this dataview / engine
                    if (String.IsNullOrEmpty(fmdv.SqlStatementForCurrentEngine)) {
                        throw Library.CreateBusinessException(getDisplayMember("getData{missingsql}", "Could not get data for dataviewName={0}: no sql statement found (usually means this dataview is not properly mapped through sys_dataview -> sys_dataview_field -> sys_dataview_table_field -> sys_table_field -> sys_table).", dataviewNameOrSql.ToUpper() ));
                    }


                    // make sure we serve up the current language info
                    sql = sql.Replace("__LANGUAGEID__", altLanguageID.ToString());

                    // pull only the records the caller wants to see (both will typically be 0, meaning pull all)
                    dm.Limit = limit;
                    dm.Offset = offset;

                    // unescape where clause as needed
                    if (getOptions.UnescapeWhereClause) {
                        sql = sql.Replace("/*_", "").Replace("_*/", "");
                    }

                    #endregion scrub inputs / initialization

                    #region process triggers and sql

                    if (streamWriter == null) {
                        // do not stream the data, just read it into the dataset

                        // if args and triggers exist, we need to do the additional processing...
                        // HACK: bypass triggers for lookup tables since they are so large
                        //                        if (readArgs == null || tableName.ToLower().EndsWith("_lookup")) {
                        if (readArgs == null) { // || tableName.ToLower().EndsWith("_lookup")) {

                            dm.Read(sql, dsReturn, tableName, dps);

                        } else {

                            #region call 'before' triggers

                            // run 'before' dataview trigger(s)
                            bool returnZeroRows = false;
                            foreach (IDataviewReadDataTrigger trigger in readArgs.FieldMappingDataView.ReadDataTriggers) {
                                readArgs.Uncancel();
                                trigger.DataViewReading(readArgs);
                                if (readArgs.IsCancelled) {
                                    // set flags as needed -- once they go to true, they stay true during this request
                                    returnZeroRows = true;
                                    break;
                                }
                            }

                            // run 'before' table trigger(s)
                            foreach (var tbl in readArgs.FieldMappingDataView.Tables) {
                                foreach (var rt in tbl.ReadDataTriggers) {
                                    readArgs.Uncancel();
                                    rt.TableReading(readArgs);
                                    if (readArgs.IsCancelled) {
                                        returnZeroRows = true;
                                        break;
                                    }
                                }
                            }
                            #endregion call 'before' triggers

                            #region run sql as needed
                            if (returnZeroRows) {
                                // essentially this means we know we won't give them any data, but we need to return the schema.
                                // so we limit the rows to 1 because it'll be thrown out anyway
                                dm.Limit = 1;
                                dm.Offset = 0;
                            }

                            // run the sql
                            dm.Read(sql, dsReturn, tableName, dps);

                            // return limit / offset back to normal values
                            dm.Limit = limit;
                            dm.Offset = offset;

                            // store the reseults in the readArgs so we can pass that to the trigger(s)
                            readArgs.DataTable = dsReturn.Tables[tableName];

                            // make sure datetime columns are configured to ignore timezones and daylight saving time offsets...
                            foreach (DataColumn dc in dsReturn.Tables[tableName].Columns)
                            {
                                if (dc.DataType == typeof(DateTime))
                                {
                                    dc.DateTimeMode = DataSetDateTime.Unspecified;
                                }
                            }

                            #endregion run sql as needed

                            #region call row triggers
                            if (!returnZeroRows) {
                                for (var i = 0; i < readArgs.DataTable.Rows.Count; i++) {

                                    // run 'row' table trigger(s)
                                    readArgs.DataRow = readArgs.DataTable.Rows[i];
                                    var removedRow = false;
                                    foreach (var tbl in readArgs.FieldMappingDataView.Tables) {
                                        foreach (var rt in tbl.ReadDataTriggers) {
                                            readArgs.DataRow = readArgs.DataTable.Rows[i];
                                            readArgs.Uncancel();
                                            rt.TableRowRead(readArgs);
                                            if (readArgs.IsCancelled) {
                                                readArgs.DataTable.Rows[i].Delete();
                                                removedRow = true;
                                                break;
                                            }
                                        }
                                        if (removedRow) {
                                            break;
                                        }
                                    }

                                    // run 'row' dataview trigger(s)
                                    if (!removedRow) {
                                        foreach (IDataviewReadDataTrigger trigger in readArgs.FieldMappingDataView.ReadDataTriggers) {
                                            readArgs.Uncancel();
                                            trigger.DataViewRowRead(readArgs);
                                            if (readArgs.IsCancelled) {
                                                readArgs.DataTable.Rows[i].Delete();
                                                break;
                                            }
                                        }
                                    }
                                }



                                // if any changes were made to the DataTable object, commit them now
                                readArgs.DataTable.AcceptChanges();

                                readArgs.DataRow = null;
                            }
                            #endregion run 'row' triggers

                            #region call after triggers
                            // run 'after' table trigger(s)
                            if (!returnZeroRows) {
                                foreach (var tbl in readArgs.FieldMappingDataView.Tables) {
                                    foreach (var rt in tbl.ReadDataTriggers) {
                                        readArgs.Uncancel();
                                        rt.TableRead(readArgs);
                                        if (readArgs.IsCancelled) {
                                            // remove all rows, but still return a DataTable object
                                            returnZeroRows = true;
                                            break;
                                        }
                                    }
                                    if (returnZeroRows) {
                                        break;
                                    }
                                }
                            }

                            // run 'after' dataview trigger(s)
                            if (!returnZeroRows) {
                                foreach (IDataviewReadDataTrigger trigger in readArgs.FieldMappingDataView.ReadDataTriggers) {
                                    readArgs.Uncancel();
                                    trigger.DataViewRead(readArgs);
                                    if (readArgs.IsCancelled) {
                                        // remove all rows, but still return a DataTable object
                                        returnZeroRows = true;
                                        break;
                                    }
                                }
                            }
                            #endregion call after triggers

                            #region clear table or transform rows as needed
                            if (returnZeroRows) {
                                readArgs.DataTable.Clear();
                                readArgs.DataTable.AcceptChanges();

                            } else {

                                // transform if needed
                                // a transform is also called a transpose, pivot, tilt, flip, etc.
                                // basically it means taking row-wise data and making it column-wise based on unique values 
                                // of one of the columns.
                                // these by definition are not updatable yet, but useful for displaying on web pages
                                // in certain situations
                                if (fmdv != null && fmdv.TransformByFields.Length > 0) {
                                    if (String.IsNullOrEmpty(fmdv.TransformFieldForNames) || String.IsNullOrEmpty(fmdv.TransformFieldForValues)) {
                                        throw Library.CreateBusinessException(getDisplayMember("getData{badtransform}", "DataView {0} has transformByFields defined ({1}) but is missing at least one of the following: transformFieldForValues={2}, transformFieldForNames={3}",
                                            fmdv.DataViewName,
                                            String.Join(",", fmdv.TransformByFields),
                                            fmdv.TransformFieldForValues,
                                            fmdv.TransformFieldForNames));
                                    }

                                    var dt = dsReturn.Tables[tableName].Transform(fmdv.TransformByFields, fmdv.TransformFieldForNames, fmdv.TransformFieldForCaptions, fmdv.TransformFieldForValues);
                                    dsReturn.Tables.Remove(tableName);
                                    dsReturn.Tables.Add(dt);
                                }
                            }
                            #endregion clear table or transform rows as needed
                        }
                    } else {
                        // stream data to caller

                        // TODO: security for streaming.  Not used yet, so low priority

                        if (streamFormat.ToLower() == "dataset") {
                            var ret = dm.Read(sql, dsReturn, tableName, dps);
                            if (prettyColumns) {
                                FinishProcessing(ret, altLanguageID);
                            }
                            ret.WriteXml(streamWriter.BaseStream, XmlWriteMode.WriteSchema);

                        } else {

                            using (IDataReader idr = dm.Stream(sql, dps)) {

                                #region determine columns
                                // write out header row
                                string[] colNames = new string[idr.FieldCount];
                                for (int i = 0; i < colNames.Length; i++) {
                                    colNames[i] = idr.GetName(i);
                                    if (prettyColumns) {
                                        foreach (var map in fmdv.Mappings) {
                                            if (colNames[i].ToLower() == map.DataviewFieldName.ToLower()) {
                                                colNames[i] = map.FriendlyFieldName;
                                            }
                                        }
                                    }
                                }
                                #endregion determine columns

                                #region stream formatted output
                                // write out data rows
                                object[] values = new object[idr.FieldCount];
                                Type[] types = null;

                                switch (streamFormat.ToLower()) {
                                    case "csv":
                                    default:
                                        // output a CSV-formatted stream
                                        Toolkit.OutputCSVHeader(colNames, streamWriter);
                                        while (idr.Read()) {
                                            idr.GetValues(values);
                                            if (types == null) {
                                                types = new Type[values.Length];
                                                for (var j = 0; j < types.Length; j++) {
                                                    types[j] = idr.GetFieldType(j);
                                                }
                                            } else {
                                                for (var j = 0; j < values.Length; j++) {
                                                    if (types[j] == typeof(DateTime)) {
                                                        // notice we emit datetime data in the same value as it is stored,
                                                        // which should always be in UTC (GMT, whatever) so the client
                                                        // will need to convert it to local time as needed.
                                                        values[j] = ((DateTime)values[j]).ToString("yyyy/MM/dd hh:mm:ss tt");
                                                    }
                                                }
                                            }
                                            Toolkit.OutputCSV(values, streamWriter, false);
                                        }
                                        break;
                                    case "tab":
                                        // output a tab-delimited stream
                                        Toolkit.OutputTabbedHeader(colNames, streamWriter);
                                        while (idr.Read()) {
                                            idr.GetValues(values);
                                            if (types == null) {
                                                types = new Type[values.Length];
                                                for (var j = 0; j < types.Length; j++) {
                                                    types[j] = idr.GetFieldType(j);
                                                }
                                            } else {
                                                for (var j = 0; j < values.Length; j++) {
                                                    if (types[j] == typeof(DateTime)) {
                                                        // notice we emit datetime data in the same value as it is stored,
                                                        // which should always be in UTC (GMT, whatever) so the client
                                                        // will need to convert it to local time as needed.
                                                        values[j] = ((DateTime)values[j]).ToString("yyyy/MM/dd hh:mm:ss tt");
                                                    }
                                                }
                                            }
                                            Toolkit.OutputTabbedData(values, streamWriter, false);
                                        }
                                        break;
                                    case "json":
                                        // output a JSON-formatted stream
                                        streamWriter.WriteLine("{ \"" + dataviewNameOrSql + "\" : { ");
                                        streamWriter.WriteLine(" columns : [");
                                        for (var i = 0; i < colNames.Length; i++) {
                                            if (i > 0) {
                                                streamWriter.Write(@", """ + colNames[i].Replace("\"", "\\\"") + @"""");
                                            } else {
                                                streamWriter.Write(@"""" + colNames[i].Replace("\"", "\\\"") + @"""");
                                            }
                                        }
                                        streamWriter.WriteLine("], rows : [ ");
                                        while (idr.Read()) {
                                            idr.GetValues(values);
                                            if (types == null) {
                                                streamWriter.WriteLine(", ");
                                                types = new Type[values.Length];
                                                for (var j = 0; j < types.Length; j++) {
                                                    types[j] = idr.GetFieldType(j);
                                                }
                                            } else {
                                                for (var j = 0; j < values.Length; j++) {
                                                    if (types[j] == typeof(DateTime)) {
                                                        // notice we emit datetime data in the same value as it is stored,
                                                        // which should always be in UTC (GMT, whatever) so the client
                                                        // will need to convert it to local time as needed.
                                                        values[j] = ((DateTime)values[j]).ToString("yyyy/MM/dd hh:mm:ss tt");
                                                    }
                                                }
                                            }

                                            Toolkit.OutputJsonData(colNames, values, streamWriter);
                                        }
                                        streamWriter.WriteLine(" ] } } ");
                                        break;
                                }
                                #endregion stream formatted output

                            }
                        }
                    }
                    #endregion process triggers and sql

                }
            } catch (Exception ex) {
                if (fmdv != null)
                {
                    ex.Data.Add("DataviewName", fmdv.DataViewName);
                    
                    var sb2 = new StringBuilder();
                    foreach (var p in fmdv.Parameters)
                    {
                        sb2.Append(p.Name + ", ");
                    }
                    ex.Data.Add("ExpectedDataviewParameters", sb2.ToString());

                    var sb3 = new StringBuilder();
                    foreach (var k in dic.Keys)
                    {
                        sb3.Append(k + "=" + dic[k] + ";");
                    }
                    ex.Data.Add("SuppliedDataviewParameters", sb3.ToString());

                }
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, altLanguageID);
            }
            return dsReturn;


        }

        public DataSet GetDataParameterTemplate(string dataviewName) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {

                    dm.Read(@"
select
	srp.param_name as param_name,
	'' as param_value,
	srp.param_type as param_type
from
	sys_dataview sr 
	inner join sys_dataview_param srp 
		on sr.sys_dataview_id = srp.sys_dataview_id
where
	sr.dataview_name = :dataview
order by
	srp.sort_order
", dsReturn, "dv_param_info", new DataParameters(":dataview", dataviewName));
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, _languageID);
            }
            return dsReturn;
        }

        public DataSet GetAllLookupTableStats() {
            DataSet dsReturn = createReturnDataSet();
            try {

                var rv = new DataTable("get_lookup_table_stats");
                rv.Columns.Add("dataview_name", typeof(string));
                rv.Columns.Add("title", typeof(string));
                rv.Columns.Add("description", typeof(string));
                rv.Columns.Add("pk_field_name", typeof(string));
                rv.Columns.Add("table_name", typeof(string));
                rv.Columns.Add("min_pk", typeof(int));
                rv.Columns.Add("max_pk", typeof(int));
                rv.Columns.Add("row_count", typeof(int));
                rv.Columns.Add("max_modified_date", typeof(DateTime));
                rv.Columns.Add("max_created_date", typeof(DateTime));
                rv.Columns.Add("last_touched_date", typeof(DateTime));
                dsReturn.Tables.Add(rv);

                using (DataManager dm = BeginProcessing(true)) {

                    var sql = String.Format(@"
SELECT
    dv.dataview_name,
    sdl.title,
    sdl.description, 
    stf.field_name as pk_field_name,
    st.table_name as table_name
FROM
    sys_dataview dv left join sys_dataview_lang sdl
        on dv.sys_dataview_id = sdl.sys_dataview_id
        and sdl.sys_lang_id = {0}
    inner join sys_dataview_field sdvf
	    on sdvf.sys_dataview_id = dv.sys_dataview_id
	    and sdvf.is_primary_key = 'Y'
    inner join sys_table_field stf
	    on sdvf.sys_table_field_id = stf.sys_table_field_id
    inner join sys_table st
	    on stf.sys_table_id = st.sys_table_id
WHERE
    dv.category_code = 'Lookups' AND dv.is_enabled = 'Y'
order by
    dataview_name
", LanguageID);


                    var dt = dm.Read(sql);

                    foreach (DataRow dr in dt.Rows) {
                        var row = rv.NewRow();
                        row["dataview_name"] = dr["dataview_name"];
                        row["title"] = dr["title"];
                        row["description"] = dr["description"];
                        row["table_name"] = dr["table_name"];
                        row["pk_field_name"] = dr["pk_field_name"];

                        var dtStat = getLookupTableStats(dr["table_name"].ToString(), dm, null);
                        if (dtStat.Rows.Count > 0) {
                            var stat = dtStat.Rows[0];
                            row["min_pk"] = Toolkit.ToInt32(stat["min_pk"], 0);
                            row["max_pk"] = Toolkit.ToInt32(stat["max_pk"], 0);
                            row["row_count"] = Toolkit.ToInt32(stat["row_count"], 0);
                            row["max_modified_date"] = stat["max_modified_date"];
                            row["max_created_date"] = stat["max_created_date"];
                            row["last_touched_date"] = stat["last_touched_date"];
                        }
                        rv.Rows.Add(row);
                    }

                }
                rv.AcceptChanges();

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, _languageID);
            }
            return dsReturn;

        }

        public DataSet GetLookupTableStats(string tableName) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {
                    // NOTE: we use FieldMappingTable so we can both prevent sql injection
                    //       and easily lookup the primary key field name

                    getLookupTableStats(tableName, dm, dsReturn);

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, _languageID);
            }
            return dsReturn;

        }

        /// <summary>
        /// Returns the min/max primary key vaues / max modified / created dates for the given table name.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dm"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        private DataTable getLookupTableStats(string tableName, DataManager dm, DataSet ds) {
            var fmt = Table.Map(tableName, dm.DataConnectionSpec, LanguageID, false); // brock
            if (fmt == null || fmt.TableID < 0) {
                throw new InvalidOperationException(getDisplayMember("getLookupTableStats{notableid}", "Table '{0}' is not a valid table name, or is not mapped.  Use the GRIN-Global Admin Tool to add a Table Mapping for this table if it is a valid table in the database schema.", tableName));
            } else {

                if (String.IsNullOrEmpty(fmt.PrimaryKeyFieldName)) {
                    throw new InvalidOperationException(getDisplayMember("getLookupTableStats{noprimarykey}", "Table '{0}' is mapped, but there is no primary key defined in the mapping.  Specify a field as the Primary Key for this table by using the Table Mapping functionality in the GRIN-Global Admin Tool.", tableName));
                } else {
                    var sql = String.Format(@"
select
    '{0}' as table_name,
    '{1}' as pk_field_name,
    min({1}) as min_pk,
    max({1}) as max_pk,
    count({1}) as row_count,
    max(modified_date) as max_modified_date,
    max(created_date) as max_created_date,
    max(coalesce(modified_date, created_date)) as last_touched_date
from
    {0}
", fmt.TableName, fmt.PrimaryKeyFieldName);

                    if (ds == null) {
                        return dm.Read(sql);
                    } else {
                        dm.Read(sql, ds, "get_lookup_table_stats", null);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Using the given dataviewName and parameters, pull data from the GG database
        /// </summary>
        /// <param name="dataviewNameOrSql"></param>
        /// <param name="delimitedParameterList"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataSet GetData(string dataviewNameOrSql, string delimitedParameterList, int offset, int limit) {
            return GetData(dataviewNameOrSql, delimitedParameterList, offset, limit, null);
        }
        /// <summary>
        /// Using the given dataviewName and parameters, pull data from the GG database
        /// </summary>
        /// <param name="dataviewNameOrSql"></param>
        /// <param name="delimitedParameterList"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public DataSet GetData(string dataviewNameOrSql, string delimitedParameterList, int offset, int limit, string options) {
            Dictionary<string, object> dic = Toolkit.ParsePairs<object>(delimitedParameterList);
            return GetData(dataviewNameOrSql, dic, offset, limit, options);
        }


        /// <summary>
        /// Runs given SQL and parameters and puts into a DataTable named "get_data".  Accessible only by members of the Administrators group (group_tag='ADMINS').  This method is required by the Admin Tool and should not be used by any other application.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataSet GetData(string sql, List<IDataviewParameter> parameters, string dataviewName, int languageID, int offset, int limit) {
            var dsReturn = createReturnDataSet();
            try {
                checkUserHasAdminEnabled();
                using (var dm = BeginProcessing(true)) {
                    var dps = new DataParameters();
                    foreach (var p in parameters) {
                        if (!String.IsNullOrEmpty(p.Name)) {
                            var dbType = DataParameter.MapDbType(p.TypeName);
                            var pseudoType = DataParameter.MapDbPseudoType(p.TypeName);
                            dps.Add(new DataParameter(p.Name, p.Value, dbType, pseudoType));
                        }
                    }

                    // make sure we serve up the current language info
                    sql = sql.Replace("__LANGUAGEID__", languageID.ToString());

                    if (limit > 0) {
                        dm.Limit = limit;
                    }
                    if (offset > 0) {
                        dm.Offset = offset;
                    }

                    dm.Read(sql, dsReturn, dataviewName, dps);
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, languageID);
            }

            return dsReturn;

        }
        /// <summary>
        /// Using the given dataviewName and parameters, pull data from the GG database
        /// </summary>
        /// <param name="dataviewNameOrSql"></param>
        /// <param name="dic"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public DataSet GetData(string dataviewNameOrSql, Dictionary<string, object> dic, int offset, int limit, string options) {
            return getData(dic, dataviewNameOrSql, offset, limit, null, null, false, createReturnDataSet(), new ReadOptions(options));
        }

        /// <summary>
        /// Using the given dataviewName and parameters, stream data from the GG database directly onto the HttpResponse stream.
        /// </summary>
        /// <param name="dataviewNameOrSql"></param>
        /// <param name="delimitedParameterList"></param>
        /// <param name="prettyColumns"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="format"></param>
        /// <param name="stm"></param>
        /// <param name="options"></param>
        public void StreamData(string dataviewNameOrSql, string delimitedParameterList, bool prettyColumns, int offset, int limit, string format, Stream stm, string options) {

            Dictionary<string, object> dic = Toolkit.ParsePairs<object>(delimitedParameterList);
            using (StreamWriter sw = new StreamWriter(stm)) {
                getData(dic, dataviewNameOrSql, offset, limit, sw, format, prettyColumns, createReturnDataSet(), new ReadOptions(options));
            }

        }

        #endregion Get Data

    }
}
