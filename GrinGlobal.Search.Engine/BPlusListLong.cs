using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GrinGlobal.Search.Engine {
    /// <summary>
    /// A list of longs that can be stored as a value in a B+ tree
    /// </summary>
	public class BPlusListLong : IPersistable<BPlusListLong> {

        /// <summary>
        /// 
        /// </summary>
		public BPlusListLong() {
			_value = new List<long>();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
		public BPlusListLong(long[] items)
			: this(items.ToList()) {
		}

        /// <summary>
        /// Initializes the object and adds given firstItem as the only item in the Value parameter (a list of longs)
        /// </summary>
        /// <param name="firstItem"></param>
		public BPlusListLong(long firstItem)
			: this() {
			_value.Add(firstItem);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
		public BPlusListLong(List<long> list)
			: this() {
			_value.AddRange(list);
		}

		private List<long> _value;
		public List<long> Value {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}

		#region IPersistable Members

		public void Read(BinaryReader rdr) {
			// first read count, then read that many longs
			_value.Clear();
			int count = rdr.ReadInt32();
			for (int i = 0; i < count; i++) {
				_value.Add(rdr.ReadInt64());
			}
		}

		public void Write(BinaryWriter wtr) {
			wtr.Write(_value.Count);
			foreach (long l in _value) {
				wtr.Write(l);
			}
		}

        public void Update(BPlusListLong newValue, GrinGlobal.Interface.UpdateMode updateMode) {
			switch (updateMode) {
                case GrinGlobal.Interface.UpdateMode.Add:
					_value.AddRange(newValue.Value);
					break;
                case GrinGlobal.Interface.UpdateMode.Subtract:
					foreach (long l in newValue.Value) {
						if (_value.Contains(l)) {
							_value.Remove(l);
						}
					}
					break;
                case GrinGlobal.Interface.UpdateMode.Replace:
					_value = newValue.Value;
					break;
				default:
					throw new NotImplementedException();
			}
		}


		#endregion

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			foreach (long l in _value) {
				sb.Append(l.ToString()).Append(", ");
			}
			if (sb.Length > 2) {
				sb.Remove(sb.Length - 2, 2);
			}
			return sb.ToString();
		}

		#region IPersistable<BPlusListLong> Members


		#endregion
	}
}
