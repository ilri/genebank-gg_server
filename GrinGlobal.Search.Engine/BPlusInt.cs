using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GrinGlobal.Core;

namespace GrinGlobal.Search.Engine {
	public class BPlusInt : ISearchable<BPlusInt> {

		public BPlusInt() {
		}

		public BPlusInt(int value)
			: this() {
			_value = value;
		}

		#region ISearchable<int> Members

		private int _value;
		public int Value {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}

		#endregion


		#region IPersistable Members

		public void Read(BinaryReader rdr) {
			_value = rdr.ReadInt32();
		}

		public void Write(BinaryWriter wtr) {
			wtr.Write(_value);
		}

		#endregion

		public override string ToString() {
			return _value.ToString();
		}

		#region ISearchable<BPlusInt> Members

		public int CompareTo(BPlusInt bplusValue, KeywordMatchMode matchMode, bool ignoreCase) {
			int value = bplusValue.Value;
			if (_value < value) {
				return -1;
			} else if (_value > value) {
				return 1;
			} else {
				return 0;
			}
		}

		BPlusInt ISearchable<BPlusInt>.Parse(string searchTerm) {
			_value = int.Parse(searchTerm);
			return this;
		}

		#endregion

		#region IPersistable<BPlusInt> Members

        public void Update(BPlusInt newValue, GrinGlobal.Interface.UpdateMode updateMode) {
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

		#endregion

		#region IComparable<BPlusInt> Members

		public int CompareTo(BPlusInt other) {
			return this.CompareTo(other, KeywordMatchMode.ExactMatch, false);
		}

		#endregion



        #region ISearchable<BPlusInt> Members

        public object RootValue {
            get {
                return _value;
            }
            set {
                _value = Toolkit.ToInt32(value, 0);
            }
        }

        #endregion
    }
}
