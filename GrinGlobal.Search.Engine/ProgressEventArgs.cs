using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GrinGlobal.Search.Engine {
	public class ProgressEventArgs : EventArgs {

		public string Message { get; set; }
		public EventLogEntryType LogType { get; set; }
		public bool WriteToEventLog { get; set; }

		public ProgressEventArgs(string message, EventLogEntryType logType, bool writeToEventLog)
			: base() {
			Message = message;
			LogType = logType;
			WriteToEventLog = writeToEventLog;
		}
	}
}
