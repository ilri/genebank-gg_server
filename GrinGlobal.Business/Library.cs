using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Security;

namespace GrinGlobal.Business {
	internal class Library {
		/// <summary>
		/// Returns a new InvalidOperationException with the given message
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		internal static InvalidOperationException CreateBusinessException(string message) {
			var ioe = new InvalidOperationException(message);
            return ioe;
		}

        /// <summary>
        /// Returns a new InvalidOperationException with the given message and given inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        /// <returns></returns>
        internal static InvalidOperationException CreateBusinessException(string message, Exception inner) {
            return new InvalidOperationException(message, inner);
        }

		/// <summary>
		/// Creates but does NOT throw a new InvalidDataException stating the user does not have requested access to a table
		/// </summary>
		/// <param name="table"></param>
		/// <param name="permission"></param>
		/// <returns></returns>
		internal static SecurityException DataPermissionException(string userName, string table, Permission permission, string[] primaryKeyFieldValues) {
			string pks = primaryKeyFieldValues == null ? "" : " for primary key(s) = " + String.Join(", ", primaryKeyFieldValues);
			return new SecurityException("Access denied.  User '" + userName + "' does not have permission to " + permission.Action.ToString().ToLower() + " data in table '" + table + "' " + pks);
		}

		internal static void ThrowTablePermissionException(string p) {
			throw new NotImplementedException();
		}

        /// <summary>
        /// Concatenates the given dictionary keys and values into a string.  Example output:  "key1=value1; key2=value2; key3=value3"
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
		internal static string DataToString(IDictionary data) {
			StringBuilder sb = new StringBuilder();
			foreach (object item in data.Keys) {
				sb.Append(item.ToString() + "=" + data[item].ToString() + "; ");
			}
			if (sb.Length > 2) {
				sb.Remove(sb.Length - 2, 2);
			}
			string ret = sb.ToString();
			return ret;
		}



	}
}
