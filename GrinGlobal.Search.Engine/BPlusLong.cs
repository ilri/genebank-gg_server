using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GrinGlobal.Core;

namespace GrinGlobal.Search.Engine {
    /// <summary>
    /// A long that can be used as a search keyword or value in the B+ Tree
    /// </summary>
	public class BPlusLong : ISearchable<BPlusLong> {

        /// <summary>
        /// 
        /// </summary>
		public BPlusLong() {
		}

        /// <summary>
        /// Initializes a new object with the given value
        /// </summary>
        /// <param name="value"></param>
		public BPlusLong(long value)
			: this() {
			_value = value;
		}

		private long _value;
        /// <summary>
        /// Gets or sets the long values associated with this object
        /// </summary>
		public long Value {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}
        /// <summary>
        /// Reads a long value from the given BinaryReader
        /// </summary>
        /// <param name="rdr"></param>
		public void Read(BinaryReader rdr) {
			_value = rdr.ReadInt64();
		}

        /// <summary>
        /// Writes a long value to the given BinaryWriter
        /// </summary>
        /// <param name="wtr"></param>
		public void Write(BinaryWriter wtr) {
			wtr.Write(_value);
		}

        /// <summary>
        /// Returns the string representation of the Value property
        /// </summary>
        /// <returns></returns>
		public override string ToString() {
			return _value.ToString();
		}

        /// <summary>
        /// Returns -1 if current object's Value is less than the given bplusValue object's Value.  0 if they match.  +1 otherwise.  matchMode and ignoreCase are ignored and only exist to satisfy interface requirements.
        /// </summary>
        /// <param name="bplusValue"></param>
        /// <param name="matchMode"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
		public int CompareTo(BPlusLong bplusValue, KeywordMatchMode matchMode, bool ignoreCase) {
			long value = bplusValue.Value;
			if (_value < value) {
				return -1;
			} else if (_value > value) {
				return 1;
			} else {
				return 0;
			}
		}

		BPlusLong ISearchable<BPlusLong>.Parse(string searchTerm) {
			_value = long.Parse(searchTerm);
			return this;
		}

        /// <summary>
        /// Updates the object with the new value as specified by the updateMode parameter
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="updateMode"></param>
        public void Update(BPlusLong newValue, GrinGlobal.Interface.UpdateMode updateMode) {
			switch (updateMode) {
                case GrinGlobal.Interface.UpdateMode.Add:
					_value += newValue.Value;
					break;
                case GrinGlobal.Interface.UpdateMode.Subtract:
					_value -= newValue.Value;
					break;
                case GrinGlobal.Interface.UpdateMode.Replace:
					_value = newValue.Value;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		public int CompareTo(BPlusLong other) {
			return CompareTo(other, KeywordMatchMode.ExactMatch, false);
		}


        #region ISearchable<BPlusLong> Members

        public object RootValue {
            get {
                return _value;
            }
            set {
                _value = Toolkit.ToInt64(value, 0);
            }
        }

        #endregion
    }
}
