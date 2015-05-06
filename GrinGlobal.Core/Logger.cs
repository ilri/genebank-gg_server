using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Configuration;

namespace GrinGlobal.Core
{
	/// <summary>
	/// Represents a class for logging to a file (only if "LogFile" app setting is specified as the file name) or the event log (only if "EventLogSourceName" is specified as the Source name) or a database if "LogToDatabase" points to a valid connectionString app setting.  Always logs to the Debugger's output window if a Debugger is attached.
	/// </summary>
	public class Logger
	{

		private static object __lockTarget = new object();

		private static string __logFilePath;
//		private static DomainUser __lastUser;

		/// <summary>
		/// Gets the path to the log file as determined by the "LogFile" app setting.
		/// </summary>
		public static string LogFile {
			get {
				__logFilePath = Toolkit.ResolveFilePath(Toolkit.GetSetting("LogFile", (string)null), true);
				return __logFilePath;
			}
			set {
				__logFilePath = value;
			}
		}

		private static string __eventLogSourceName;

		/// <summary>
		/// Gets or sets the name to use in the "Source" field when writing to event log.  If not specified, nothing is written to event log.
		/// </summary>
		public static string EventLogSourceName {
			get {
				if (__eventLogSourceName == null) {
					__eventLogSourceName = Toolkit.GetSetting("EventLogSourceName", (string)null);
				}
				return __eventLogSourceName;
			}
			set {
				__eventLogSourceName = value;
			}
		}

		private static string __logToDatabase;
		public static string LogToDatabase {
			get {
				if (String.IsNullOrEmpty(__logToDatabase)) {
					__logToDatabase = Toolkit.GetSetting("LogToDatabase", (string)null);
				}
				return __logToDatabase;
			}
		}

		/// <summary>
		/// Will call Initialize upon first access to this class.  See Initialize for details as to what it does.
		/// </summary>
		static Logger() {
			__logFilePath = null;
			__eventLogSourceName = null;
			__logToDatabase = null;
			Logger.Initialize();
		}

		/// <summary>
		/// Gets if the log file should be cleared upon first access.
		/// </summary>
		public static bool ClearLogOnStart {
			get {
				return Toolkit.GetSetting("ClearLogOnStart", (bool)false);
			}
		}

		/// <summary>
		/// Initializes the log file.  If ClearLogOnStart is true in the config settings, the log file is deleted before anything is logged to it.
		/// </summary>
		public static void Initialize() {
			if (Logger.ClearLogOnStart) {
				Logger.Clear();
			}
		}

		/// <summary>
		/// Deletes the log file and clears database table as needed.  Next log entry will auto-create it.
		/// </summary>
		public static void Clear() {
			string file = Logger.LogFile;
			if (file != null && File.Exists(file)) {
				File.Delete(file);
			}
			string db = Logger.LogToDatabase;
			if (!String.IsNullOrEmpty(db)) {
				using (DataManager dm = DataManager.Create(db)) {
					dm.Write("delete from db_log");
				}
			}
		}

		/// <summary>
		/// If expression is true, logs given text to the file specified by the "LogFile" configuration setting and the event log with source name from "EventLogSourceName" config setting.  If "LogFile" or "EventLogSourceName" settings are null or missing, text is not logged to the respective sources.  Text is always sent to Debug.WriteLine.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="text"></param>
		public static void LogTextIf(bool expression, string text) {
			if (expression) {
				LogText(text);
			}
		}

		/// <summary>
		/// Logs given text to the file specified by the "LogFile" configuration setting and the event log with source name from "EventLogSourceName" config setting.  If "LogFile" or "EventLogSourceName" settings are null or missing, text is not logged to the respective sources.  Text is always sent to Debug.WriteLine.
		/// </summary>
		/// <param name="text"></param>
		public static void LogText(string text) {
			LogText(text, null);
		}

		/// <summary>
		/// Logs exception's Message to the file specified by the "LogFile" configuration setting and the event log with source name from "EventLogSourceName" config setting.  If "LogFile" or "EventLogSourceName" settings are null or missing, text is not logged to the respective sources.  Text is always sent to Debug.WriteLine.
		/// </summary>
		/// <param name="ex"></param>
		public static void LogText(Exception ex) {
			LogText(null, ex);
		}

		/// <summary>
		/// Logs given text and exception's Message to the file specified by the "LogFile" configuration setting and the event log with source name from "EventLogSourceName" config setting.  If "LogFile" or "EventLogSourceName" settings are null or missing, text is not logged to the respective sources.  Text is always sent to Debug.WriteLine.
		/// </summary>
		/// <param name="ex"></param>
		public static void LogText(string text, Exception ex) {
			LogText(text, ex, true, true, true);
		}

		private static string generateTextToLog(string text, Exception ex) {
			StringBuilder sb = new StringBuilder();
			sb.Append(Thread.CurrentThread.ManagedThreadId.ToString())
				.Append(" - ")
				.Append(Toolkit.GetApplicationVersion())
				.Append(" - ");
			if (ex != null) {
				sb.Append("Exception: ")
					.Append(ex.Message);
				if (ex.Data != null && ex.Data.Keys.Count > 0) {
					sb.Append("; Exception.Data info: ");
					StringBuilder sb2 = new StringBuilder();
					foreach(string key in ex.Data.Keys){
						sb2.Insert(0, key + "=" + ex.Data[key] + "; ");
					}
					sb.Append(sb2.ToString());
				}
			}
			sb.Append(text);
			return sb.ToString();
		}

		/// <summary>
		/// Logs to log file specified in config settings ("Logger.log" in current directory if not present) and event log source name specified in config settings ('Logger' if not specified).  Also logs to database if LogToDatabase config setting is given.
		/// </summary>
		/// <param name="text"></param>
		public static void LogTextForcefully(string text) {
			LogTextForcefully(text, null);
		}
		/// <summary>
		/// Logs to log file specified in config settings ("Logger.log" in current directory if not present) and event log source name specified in config settings ('Logger' if not specified).  Also logs to database if LogToDatabase config setting is given.
		/// </summary>
		/// <param name="ex"></param>
		public static void LogTextForcefully(Exception ex) {
			LogTextForcefully(null, ex);
		}

		/// <summary>
		/// Logs to log file specified in config settings ("Logger.log" in current directory if not present) and event log source name specified in config settings ('Logger' if not specified).  Also logs to database if LogToDatabase config setting is given.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="ex"></param>
		public static void LogTextForcefully(string text, Exception ex) {

			DateTime timestamp = DateTime.Now.ToUniversalTime();
			string textWithoutTimestamp = generateTextToLog(text, ex);
			string textWithTimestamp = timestamp.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " - " + textWithoutTimestamp;
			string eventSourceName = EventLogSourceName;
			string logfileName = LogFile;
			if (String.IsNullOrEmpty(logfileName)){
				logfileName = Toolkit.ResolveFilePath("~/Logger.log", true);
			}
			System.Console.WriteLine(textWithTimestamp);
			System.Diagnostics.Debug.WriteLine(textWithTimestamp);
			logToFile(textWithTimestamp, logfileName);


			//if (String.IsNullOrEmpty(eventSourceName)) {
			//    eventSourceName = "GrinGlobal.Core.Logger";
			//}
			//logToEventLog(textWithoutTimestamp, eventSourceName);

			if (!String.IsNullOrEmpty(eventSourceName)) {
				logToEventLog(textWithoutTimestamp, eventSourceName);
			}

			// we can't fake a connection string, so we simply suppress logging to DB, even when "forcefully" logging
			if (!String.IsNullOrEmpty(LogToDatabase)) {
				logToDatabase(textWithoutTimestamp, timestamp);
			}
		}

		/// <summary>
		/// If "LogFile" config setting is specified, writes given text to that file.  Also writes to event log if "EventLogSourceName" config setting is specified.  Text is always sent to Debug.WriteLine or attached Debugger, if any.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="writeToLogFileIfConfigEnabledIt">optionally write to the log file.</param>
		/// <param name="writeToEventLogIfConfigEnabledIt">optionally write to event log as well.  Note only valid if "EventLogSourceName" config setting is specified.</param>
		public static void LogText(string text, bool writeToLogFileIfConfigEnabledIt, bool writeToEventLogIfConfigEnabledIt, bool writeToDatabaseIfConfigEnabledIt) {
			LogText(text, null, writeToLogFileIfConfigEnabledIt, writeToEventLogIfConfigEnabledIt, writeToDatabaseIfConfigEnabledIt);
		}

		/// <summary>
		/// If "LogFile" config setting is specified, writes given text to that file.  Also writes to event log if "EventLogSourceName" config setting is specified.  Text is always sent to Debug.WriteLine or attached Debugger, if any.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="ex"></param>
		/// <param name="writeToLogFileIfConfigEnabledIt">optionally write to the log file.</param>
		/// <param name="writeToEventLogIfConfigEnabledIt">optionally write to event log as well.  Note only valid if "EventLogSourceName" config setting is specified.</param>
		public static void LogText(string text, Exception ex, bool writeToLogFileIfConfigEnabledIt, bool writeToEventLogIfConfigEnabledIt, bool writeToDatabaseIfConfigEnabledIt) {
			DateTime timestamp = DateTime.Now.ToUniversalTime();
			string textWithoutTimestamp = generateTextToLog(text, ex);
			string textWithTimestamp = timestamp.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " - " + textWithoutTimestamp;

			System.Diagnostics.Debug.WriteLine(textWithTimestamp);
			System.Console.WriteLine(textWithTimestamp);

			if (writeToLogFileIfConfigEnabledIt) {
				string logFileName = null;
				try {
					logFileName = Logger.LogFile;
				} catch (Exception ex2) {
					// eat all errors during writing
					string msg = "Error resolving logfile name: " + ex2.Message + "\n Original text to log: " + textWithTimestamp + "\n";
					if (System.Diagnostics.Debugger.IsAttached) {
						System.Diagnostics.Debugger.Log(1, "Logger", msg);
					} else {
						// couldn't log to the log file.  try to log to the event log, just for the heck of it
						try {
							System.Diagnostics.EventLog.WriteEntry("Logger", Toolkit.Cut(msg, 0, 8000));
						} catch {
							// eat all exceptions when writing to event log
						}
					}
				}

				logToFile(textWithTimestamp, logFileName);

			}

			if (!String.IsNullOrEmpty(EventLogSourceName) && writeToEventLogIfConfigEnabledIt) {
				logToEventLog(textWithoutTimestamp, EventLogSourceName);
			}

			if (!String.IsNullOrEmpty(LogToDatabase) && writeToDatabaseIfConfigEnabledIt) {
				logToDatabase(textWithoutTimestamp, timestamp);
			}

		}

		private static void logToEventLog(string tolog, string srcName){
			try {
				lock (__lockTarget) {
					System.Diagnostics.EventLog.WriteEntry(srcName, Toolkit.Cut(tolog, 0, 8000));
				}
			} catch {
				// ignore errors writing to event log
			}
		}

		private static void logToDatabase(string tolog, DateTime timestamp) {
			try {
				lock (__lockTarget) {
					using (DataManager dm = DataManager.Create(LogToDatabase)) {
						dm.Write("insert into db_log (content, logged_at) values (:tolog, :logged_at)", new DataParameters(":tolog", Toolkit.Cut(tolog,0,8000), ":logged_at", timestamp));
					}
				}
			} catch {
				// ignore errors writing to log database table
			}
		}

		private static void logToFile(string tolog, string fileName) {
			lock(__lockTarget){
				StreamWriter sw = null;
				try
				{
					// log to debug output if a debugger is attached.
#if !DEBUG
					if (fileName == null || fileName.Length == 0) {
						return;
					}
#else
					if (fileName == null || fileName.Length == 0) {
						// not logging to file. write to output window if a debugger is attached.
						if (System.Diagnostics.Debugger.IsAttached){
							System.Diagnostics.Debugger.Log(1, "GrinGlobal.Core.Logger", tolog + "\n");
						}
						return;
					}
#endif
					sw = new StreamWriter(fileName, true);
					// write to the log file
					sw.WriteLine(tolog);

				} catch (Exception e) {

					// eat all errors during writing
					if (System.Diagnostics.Debugger.IsAttached) {
						System.Diagnostics.Debugger.Log(1, "GrinGlobal.Core.Logger", "Error during logging to file log: " + e.ToString() + "; Original log: " + tolog + "\n");
					} else {
						// couldn't log to the log file.  try to log to the event log, just for the heck of it
						try {
							System.Diagnostics.EventLog.WriteEntry("GrinGlobal.Core.Logger", "Error writing text '" + tolog + "' to log file '" + fileName + "': " + e.ToString());
						} catch {
							// eat all exceptions when writing to event log
						}
					}
				} finally {
					try {
						if (sw != null) {
							sw.Close();
						}
					} catch (Exception e2) {
						// eat all errors upon closing
						if (System.Diagnostics.Debugger.IsAttached) {
							System.Diagnostics.Debugger.Log(1, "GrinGlobal.Core.Utilties.Logger", "Error closing logfile: " + e2.ToString() + "; Original log: " + tolog + "\n");
						}
					}
				}
			}
		}

		/// <summary>
		/// Automatically logs any unhandled exceptions that occur in the application.  Applications should still have try/catch in their Main() method as good practice.
		/// </summary>
		public static void LogUnhandledExceptions() {
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
			LogText("Unhandled exception in app '" + AppDomain.CurrentDomain.SetupInformation.ApplicationName + "': " + e.ExceptionObject);
		}
	}
}
