using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GrinGlobal.Core {
	[DebuggerStepThrough()]
	public class DataConnectionSpec {
		public string ConnectionString { get; set; }
		public string ConnectionStringSettingName { get; set; }
		public string ProviderName { get; set; }
		public string Name { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public DataManager Parent { get; set; }
		public DataVendor Vendor { get; set; }
		public int? CommandTimeout { get; set; }

		public DataConnectionSpec() {
		}

		public DataConnectionSpec(string connectionStringSettingName) {
			ConnectionStringSettingName = connectionStringSettingName;
		}

		/// <summary>
		/// Makes a deep copy of the current DataConnectionSpec EXCEPT DataManager represented by Parent property is not cloned
		/// </summary>
		/// <returns></returns>
		public DataConnectionSpec Clone() {
			DataConnectionSpec dsc = new DataConnectionSpec();
			dsc.ConnectionString = ConnectionString;
			dsc.ConnectionStringSettingName = ConnectionStringSettingName;
			dsc.ProviderName = ProviderName;
			dsc.Name = Name;
			dsc.UserName = UserName;
			dsc.Password = Password;
			dsc.Parent = Parent;
			dsc.Vendor = Vendor;
			dsc.CommandTimeout = CommandTimeout;
			return dsc;
		}

		/// <summary>
		/// Makes a deep copy of the current DataConnectionSpec but uses the given DataManager for the Parent property
		/// </summary>
		/// <param name="newParent"></param>
		/// <returns></returns>
		public DataConnectionSpec Clone(DataManager newParent) {
			DataConnectionSpec dsc = Clone();
			dsc.Parent = newParent;
			return dsc;
		}

	}
}