using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;
using System.ServiceProcess;

namespace GrinGlobal.Updater {
    public class SqlServerEngineUtil { //: DatabaseEngineUtil {

        internal SqlServerEngineUtil(string fullPathToGGUACExe, string preferredInstanceName) {
            //: base("sqlserver",
            //    (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\100", "VerSpecificRootDir", "") + @"Tools\Binn\",
            //    (string)Registry.GetValue(getSqlServerBaseRegistryKey(preferredInstanceName), "SQLPath", ""),
            //    (string)Registry.GetValue(getSqlServerBaseRegistryKey(preferredInstanceName), "SQLDataRoot", "") + @"\DATA",
            //    getServiceName(preferredInstanceName),
            //    "SQL Server 2008 Express",
            //    "sa",
            //    fullPathToGGUACExe) {
            _preferredInstanceName = String.IsNullOrEmpty(preferredInstanceName) ? "SQLEXPRESS" : preferredInstanceName;
        }

        private string _preferredInstanceName;

        private static string getServiceName(string preferredInstanceName) {
            string instanceName = getInstanceName(preferredInstanceName, true);

            string prefix = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\Services\SQL Server", "LName", "") as string;
            instanceName = ("" + instanceName).Replace("MSSQL10.", prefix);
            return instanceName;
        }

        /// <summary>
        /// If returnFullName is true, returns the "MSSQL10." prefix as part of response.  Otherwise "MSSQL10." is removed from the response.
        /// </summary>
        /// <param name="preferredInstance"></param>
        /// <param name="returnFullName"></param>
        /// <returns></returns>
        private static string getInstanceName(string preferredInstance, bool returnFullName) {
            if (String.IsNullOrEmpty(preferredInstance)) {
                preferredInstance = "SQLEXPRESS";
            }
            string instanceName = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", preferredInstance, "") as string;
            if (!returnFullName) {
                instanceName = ("" + instanceName).Replace("MSSQL10.", "");
            }
            return instanceName;
        }

        private static string getSqlServerBaseRegistryKey(string preferredInstanceName) {

            string instanceName = getInstanceName(preferredInstanceName, true);

            string baseKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\" + instanceName + @"\Setup";

            return baseKey;


        }


        private bool isMixedModeEnabled() {
            string instanceName = getInstanceName(_preferredInstanceName, true);
            int loginMode = 0;
            string val = Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\" + instanceName + @"\MSSQLServer", "LoginMode", "0") as string;
            int.TryParse(val, out loginMode);

            return loginMode == 2;
        }

        public void EnableMixedModeIfNeeded() {


            if (!isMixedModeEnabled()) {

                // shut down sql server
                this.StopService();

                // change the registry key

                // 1 = windows authentication only
                // 2 = mixed mode (windows authentication or sql server authentication)
                string instanceName = getInstanceName(_preferredInstanceName, true);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\" + instanceName + @"\MSSQLServer", "LoginMode", 2, RegistryValueKind.DWord);

                // start up sql server
                this.StartService();

            }
        }

        public virtual void StartService() {
            using (ServiceController sc = new ServiceController(getServiceName(_preferredInstanceName))) {
                sc.Refresh();
                if (sc.Status != ServiceControllerStatus.Running && sc.Status != ServiceControllerStatus.StartPending) {
                    sc.Start();
                }
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30.0f));
            }
        }

        public virtual void StopService() {
            using (ServiceController sc = new ServiceController(getServiceName(_preferredInstanceName))) {
                sc.Refresh();
                if (sc.Status != ServiceControllerStatus.Stopped && sc.Status != ServiceControllerStatus.StopPending) {
                    sc.Stop();
                }
                sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30.0f));
            }
        }

    }
}
