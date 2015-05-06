using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;
using GrinGlobal.Core;

namespace GrinGlobal.Search.Engine {
	public class BPlusString : ISearchable<BPlusString> {

		public BPlusString() {
		}

		public BPlusString(string value)
			: this() {
			_value = value;
		}

		private string _value;
		public string Value {
			get {
				return _value;
			}
			set {
				_value = value;
				DateTime val = DateTime.Now;
				val.ToLocalTime();
			}
		}


		#region IPersistable<string> Members

		public void Read(BinaryReader rdr) {
			_value = rdr.ReadString();
		}

		public void Write(BinaryWriter wtr) {
			wtr.Write(_value == null ? String.Empty : _value);
		}

		#endregion

		public override string ToString() {
			return _value;
		}


		#region ISearchable<BPlusString> Members

		public int CompareTo(BPlusString bplusValue, KeywordMatchMode matchMode, bool ignoreCase) {

			if (String.IsNullOrEmpty(bplusValue.Value)) {
				return -1;
			}

			switch (matchMode) {
				case KeywordMatchMode.Contains:
					// we return 1 on no match so matching continues
					if (ignoreCase) {
						return bplusValue.Value.ToLower().Contains(_value.ToLower()) ? 0 : 1;
					} else {
						return bplusValue.Value.Contains(_value) ? 0 : 1;
					}

				case KeywordMatchMode.EndsWith:
					// we return 1 on no match so matching continues
					if (ignoreCase) {
						return bplusValue.Value.ToLower().EndsWith(_value.ToLower()) ? 0 : 1;
					} else {
						return bplusValue.Value.EndsWith(_value) ? 0 : 1;
					}

				case KeywordMatchMode.ExactMatch:
					return String.Compare(_value, bplusValue.Value, ignoreCase, CultureInfo.CurrentCulture);

				case KeywordMatchMode.StartsWith:
					// we return 1 on no match so matching continues
					if (ignoreCase) {
						if (bplusValue.Value.ToLower().StartsWith(_value.ToLower())) {
							return 0;
						}
					} else {
						if (bplusValue.Value.StartsWith(_value)) {
                            return 0;
						}
					}

					// we get here, this value doesn't start with the given one.
					// keep checking -- but determine which direction to check
					// since we're doing a startswith, we need to determine which way to go
					return String.Compare(_value, bplusValue.Value, ignoreCase, CultureInfo.CurrentCulture);

				default:
					throw new NotImplementedException(getDisplayMember("CompareTo", "Missing case in switch() in BPlusString.CompareTo()"));
			}


		}

		public BPlusString Parse(string searchTerm) {
			_value = searchTerm;
			return this;
		}

		#endregion

		#region IPersistable<BPlusString> Members

        public void Update(BPlusString newValue, GrinGlobal.Interface.UpdateMode updateMode) {
			switch (updateMode) {
                case GrinGlobal.Interface.UpdateMode.Add:
					_value += newValue.Value;
					break;
                case GrinGlobal.Interface.UpdateMode.Subtract:
					_value = _value.Replace(newValue.Value, "");
					break;
                case GrinGlobal.Interface.UpdateMode.Replace:
					_value = newValue.Value;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		#endregion

		#region IComparable<BPlusString> Members

		public int CompareTo(BPlusString other) {
			int c = CompareTo(other, KeywordMatchMode.ExactMatch, false);
			return c;
		}

		#endregion

        #region ISearchable<BPlusString> Members

        public object RootValue {
            get {
                return _value;
            }
            set {
                _value = value as string;
            }
        }

        #endregion

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "BPlusString", resourceName, null, defaultValue, substitutes);
        }
    
    }
}
