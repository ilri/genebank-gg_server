using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace GrinGlobal.DatabaseInspector {
	public class ProgressEventArgs : EventArgs {

        public BackgroundWorker Worker { get; set; }
		public string Message { get; set; }

		public int TotalItems { get; set; }

		public int ItemOffset { get; set; }

		public ProgressEventArgs(string message, int itemOffset, int totalItems)
			: base() {
			Message = message;
			ItemOffset = itemOffset;
			TotalItems = totalItems;
		}
	}
}
