using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.Data;
using System.Data.Common;

using System.Runtime.InteropServices;

namespace GrinGlobal.Core {
	/// <summary>
	/// 
	/// </summary>
	[ComVisible(false)]
#if !DEBUGDATAMANAGER
	[DebuggerStepThrough()]
#endif
	public class DataCommand : IDisposable {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sqlOrProc"></param>
		/// <param name="dps"></param>
		public DataCommand(string procOrSql, DataParameters dps){
			_procOrSql = procOrSql;
            _dps = dps ?? new DataParameters();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sqlOrProc"></param>
		public DataCommand(string procOrSql) : this(procOrSql, null) {
		}

		private string _procOrSql;
		/// <summary>
		/// 
		/// </summary>
		public string ProcOrSql {
			[DebuggerStepThrough()]
			get { return _procOrSql; }
			[DebuggerStepThrough()]
			set { _procOrSql = value; }
		}

		private IDbCommand _nativeCommand;
		/// <summary>
		/// 
		/// </summary>
		internal IDbCommand NativeCommand {
			get {
				return _nativeCommand;
			}
			set {
				_nativeCommand = value;
			}
		}

		private int? _timeout;
		public int? Timeout {
			get { return _timeout; }
			set { _timeout = value; }
		}

		private bool _isPrepared;
		/// <summary>
		/// Flags DataManager to prepare this command for re-use
		/// </summary>
		public bool IsPrepared {
			get {
				return _isPrepared;
			}
			set {
				_isPrepared = value;
			}
		}

		private DataParameters _dps;
		/// <summary>
		/// 
		/// </summary>
		public DataParameters DataParameters {
			[DebuggerStepThrough()]
			get { return _dps; }
			[DebuggerStepThrough()]
			set {
				_dps = value;
			}
		}

		[DebuggerStepThrough()]
		public override string ToString() {
			return _procOrSql + " -- params= " + _dps.ToString();
		}


		#region IDisposable Members

		public void Dispose() {

			if (_nativeCommand != null) {
				_nativeCommand.Dispose();
			}
			_nativeCommand = null;

			if (_dps != null && _dps.Count > 0) {
				_dps.Clear();
			}
			_dps = null;
		}

		#endregion
	}
}
