using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;

namespace GrinGlobal.Core {
	/// <summary>
	/// A class for easily blocking the calling thread until caller-determined criterion are met.
	/// </summary>
	public class ThreadBlocker {

		private object _lock;

		private static ThreadBlocker __singleton = new ThreadBlocker();

		/// <summary>
		/// A global instance of the ThreadBlocker.  Defaults to infinite timeout on the Block() call.
		/// </summary>
		public static ThreadBlocker Singleton {
			get {
				return __singleton;
			}
		}

		/// <summary>
		/// Creates an instance of a ThreadBlocker with an infinite timeout on the Block() call (Block never returns until Unblock() is called)
		/// </summary>
		public ThreadBlocker() {
			_lock = new object();
			_maxWaitTime = Timeout.Infinite;
		}

		/// <summary>
		/// Creates an instance of a ThreadBlocker with maxWaitTime milliseconds before the Block() call expires and returns if Unblock() was not called in time.
		/// </summary>
		/// <param name="maxWaitTime"></param>
		public ThreadBlocker(int maxWaitTime) : this() {
			_maxWaitTime = maxWaitTime;
		}

		private int _maxWaitTime;

		/// <summary>
		/// Number of milliseconds to wait until Block() returns.  -1 is default and means never return from Block() until Unblock() has been called.
		/// </summary>
		public int MaxWaitTime {
			get { return _maxWaitTime; }
			set { _maxWaitTime = value; }
		}

		/// <summary>
		/// Notifies the thread which called Block() that it is free to continue processing.
		/// </summary>
		public void Unblock() {
			lock (_lock) {
				Monitor.Pulse(_lock);
			}
		}

		/// <summary>
		/// Blocks the calling thread until the callback returns true.  Callback will be called when given maxWaitTime milliseconds expire or immediately when Unblock() is called by another thread.
		/// </summary>
		/// <param name="callback">Method to call when we're checking whether to return control to the caller.  Should return true to cause this method to exit, false for it to continue looping.</param>
		/// <param name="maxWaitTime"></param>
		public void BlockingLoop(Toolkit.BoolCallback callback, int maxWaitTime) {
			lock (_lock) {
				while (!callback()) {
					Monitor.Wait(_lock, maxWaitTime);
				}
			}
		}

		/// <summary>
		/// Blocks the calling thread until the callback returns true.  Callback will be called when MaxWaitTime milliseconds expire or immediately when Unblock() is called by another thread.
		/// </summary>
		/// <param name="callback">Method to call when we're checking whether to return control to the caller.  Should return true to cause this method to exit, false for it to continue looping.</param>
		public void BlockingLoop(Toolkit.BoolCallback callback) {
			BlockingLoop(callback, _maxWaitTime);
		}

		/// <summary>
		/// Blocks the calling thread until Unblock() is called by another thread or ThreadBlocker.MaxWaitTime milliseconds, whichever comes first.
		/// </summary>
		public void Block() {
			Block(_maxWaitTime);
		}

		/// <summary>
		/// Blocks the calling thread until Unblock() is called by another thread or MaxWaitTime milliseconds, whichever comes first.
		/// </summary>
		/// <param name="maxWaitTime">Number of milliseconds to wait before timing out.  Pass -1 (== Timeout.Infinity) to wait forever.</param>
		public void Block(int maxWaitTime) {
			lock (_lock) {
				Monitor.Wait(_lock, maxWaitTime);
			}
		}
	}
}