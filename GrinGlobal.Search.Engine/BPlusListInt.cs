using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GrinGlobal.Search.Engine {
	public class BPlusListInt : IPersistable<BPlusListInt> {
		public BPlusListInt() {
			_value = new List<int>();
		}

		public BPlusListInt(int[] items)
			: this(items.ToList()) {
		}

		public BPlusListInt(int firstItem)
			: this() {
			_value.Add(firstItem);
		}


		public BPlusListInt(List<int> list)
			: this() {
			_value.AddRange(list);
		}

		private List<int> _value;
        /// <summary>
        /// Gets or sets the list of integers this object represents
        /// </summary>
		public List<int> Value {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}

        /// <summary>
        /// Loads this object's properties using the given BinaryReader as a datasource
        /// </summary>
        /// <param name="rdr"></param>
		public void Read(BinaryReader rdr) {
			// first read count, then read that many longs
			_value.Clear();
			int count = rdr.ReadInt32();
			for (int i = 0; i < count; i++) {
				_value.Add(rdr.ReadInt32());
			}
		}

        /// <summary>
        /// Persists this object's properties using the given BinaryWriter as a destination
        /// </summary>
        /// <param name="wtr"></param>
		public void Write(BinaryWriter wtr) {
			wtr.Write(_value.Count);
			foreach (int val in _value) {
				wtr.Write(val);
			}
		}
        /// <summary>
        /// Updates this object as specified by the given parameters
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="updateMode"></param>
        public void Update(BPlusListInt newValue, GrinGlobal.Interface.UpdateMode updateMode) {
			switch (updateMode) {
                case GrinGlobal.Interface.UpdateMode.Add:
					_value.AddRange(newValue.Value);
					break;
                case GrinGlobal.Interface.UpdateMode.Subtract:
					foreach (int val in newValue.Value) {
						if (_value.Contains(val)) {
							_value.Remove(val);
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
        /// <summary>
        /// The string representation of this object -- a comma separated list of each integer
        /// </summary>
        /// <returns></returns>
		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			foreach (int val in _value) {
				sb.Append(val.ToString()).Append(", ");
			}
			if (sb.Length > 2) {
				sb.Remove(sb.Length - 2, 2);
			}
			return sb.ToString();
		}

	}
}
