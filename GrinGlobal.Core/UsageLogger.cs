using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Threading;

namespace GrinGlobal.Core {
	public class UsageLogger {

		public static string __fileName;
		public static string UsageLogFileName {
			get {
				if (__fileName == null) {
					__fileName = Toolkit.GetSetting("UsageLogFileName", @".\usage.log");
				}
				return __fileName;
			}
			set {
				__fileName = value;
			}
		}

		public static void Log(string message) {
			try {
				if (!String.IsNullOrEmpty(UsageLogFileName)) {
					using (StreamWriter sw = new StreamWriter(File.Open(Toolkit.ResolveDirectoryPath(UsageLogFileName, true), FileMode.OpenOrCreate,FileAccess.Write))) {
						sw.WriteLine(DateTime.Now.ToUniversalTime().ToString() + "\t" + Thread.CurrentThread.ManagedThreadId + "\t" + message.Replace("\t", "\\t"));
					}
				}
			} catch {
				// eat all errors writing to usage log -- if we don't gather it, it's not essential
				// to execution of the application.
			}
		}

		public static void Clear() {
			if (!String.IsNullOrEmpty(UsageLogFileName)) {
				File.Delete(UsageLogFileName);
			}
		}

		public static string GetAllLogData() {
			if (!String.IsNullOrEmpty(UsageLogFileName)) {
				return File.ReadAllText(UsageLogFileName);
			} else {
				return null;
			}
		}

	}
}
