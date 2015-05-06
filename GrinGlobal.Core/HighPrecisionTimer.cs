using System;
using System.Runtime.InteropServices;

namespace GrinGlobal.Core {
	/// <summary>
	/// Summary description for HighPrecisionTimer.
	/// </summary>
	public class HighPrecisionTimer : IDisposable {

		static HighPrecisionTimer() {
			// we want frequency per ms instead of sec, so we divide by 1000
			__frequency = (double)(HighPrecisionTimer.CurrentTickFrequency / 1000);

		}
		/// <summary>
		/// Create a timer named the given name but do not start it
		/// </summary>
		/// <param name="name"></param>
		public HighPrecisionTimer(string name)
			: this(name, false) {
		}

		/// <summary>
		/// Create a named timer and start it immediately (call Start();)
		/// </summary>
		/// <param name="name"></param>
		/// <param name="startImmediately"></param>
		public HighPrecisionTimer(string name, bool startImmediately) {
			_name = name;
			if (startImmediately) {
				Start();
			}
		}

		private static double __frequency;

		private bool _started;
		private long _begin;
		private long _end;
		private string _name;

		/// <summary>
		/// Starts the timer.  Also writes to Debug output in Debug mode.
		/// </summary>
		public void Start() {
			if (!_started) {
				// System.Diagnostics.Debug.WriteLine("Timer " + _name + " started.");
				_begin = HighPrecisionTimer.CurrentTickCount;
				_started = true;
			}
		}

		/// <summary>
		/// Gets or sets the name for this timer
		/// </summary>
		public string Name {
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// Stops and starts this timer, essentially setting ElapsedMilliseconds back to zero.
		/// </summary>
		public void Restart() {
			Stop();
			Start();
		}

		/// <summary>
		/// Stops the timer
		/// </summary>
		public double Stop() {
			if (_started) {
				_end = HighPrecisionTimer.CurrentTickCount;
				_started = false;
				// System.Diagnostics.Debug.WriteLine("Timer " + _name + " stopped - " + this.ElapsedMilliseconds + " ms");
			}
			return ElapsedMilliseconds;
		}

		public double ElapsedSeconds {
			get {
				return ElapsedMilliseconds / 1000.0f;
			}
		}

		/// <summary>
		/// Gets the number of milliseconds elapsed since the timer was started.
		/// </summary>
		public double ElapsedMilliseconds {
			get {
				if (_started) {
					// still running. read a new tick count but don't remember it.
					return (double)(HighPrecisionTimer.CurrentTickCount - _begin) / __frequency;
				} else {
					// stopped.  _end has appropriate tick count
					return (double)(_end - _begin) / __frequency;
				}
			}
		}
		#region IDisposable Members

		/// <summary>
		/// Stops the timer if it is running
		/// </summary>
		public void Dispose() {
			// make this object available to use as a using() block:
			// using(HighPerformanceTimer tmr = new HighPerformanceTimer("blah",true)){
			//   // do stuff here!
			// }
			Stop();

		}

		#endregion

		[DllImport("kernel32")]
		private static extern bool QueryPerformanceCounter(ref long count);
		/// <summary>
		/// This provides a hi-precision counter based on the current time measured in ticks (ns) since 1/1/1970.
		/// The intrinsic .NET counters are very low precision -- often a few hundred ms at a time.
		/// </summary>
		public static long CurrentTickCount {
			get {
				long ticks = 0;
                try {
                    QueryPerformanceCounter(ref ticks);
                } catch { }
				return ticks;
			}
		}

		[DllImport("kernel32")]
		private static extern bool QueryPerformanceFrequency(ref long freq);
		/// <summary>
		/// This provides the frequency at which the hi-precision counter executes for the CurrentTickCount property.
		/// </summary>
		public static long CurrentTickFrequency {
			get {
                // make sure we don't accidentally divide by zero later in case QueryPerformanceFrequency fails..
                long freq = 10;
                try {
    				QueryPerformanceFrequency(ref freq);
                } catch {
                }
				return freq;
			}
		}

	}
}
