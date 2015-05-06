using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;

namespace GrinGlobal.DatabaseInspector {
	public class Options {

		private Dictionary<string, object> _dict;

		private Options(Options copyFrom) {
			_dict = new Dictionary<string, object>();
			if (copyFrom != null) {
				foreach (string key in copyFrom.Keys) {
					this[key] = copyFrom[key];
				}
			}
		}

		public Options(params object[] keyValPairs) {
			_dict = new Dictionary<string, object>();
			if (keyValPairs == null) {
				return;
			} else if (keyValPairs.Length % 2 != 0) {
				throw new InvalidOperationException(getDisplayMember("Options_constructor", "Parameters passed to Options constructor must be of even length (i.e. key/value pair)"));
			} else {
				for (int i=0; i < keyValPairs.Length; i += 2) {
					this[keyValPairs[i].ToString()] = keyValPairs[i + 1];
				}
			}
		}

		public Dictionary<string, object>.KeyCollection Keys {
			get {
				return _dict.Keys;
			}
		}

		public static Options operator +(Options opt1, Options opt2) {
			Options output = new Options(opt1);
			if (opt2 != null) {
				foreach (string key in opt2.Keys) {
					output[key] = opt2[key];
				}
			}
			return output;
		}

		public object this[string key] {
			get {
				if (!_dict.ContainsKey(key)) {
					return null;
				} else {
					return _dict[key];
				}
			}
			set {
				if (!_dict.ContainsKey(key)) {
					_dict.Add(key, value);
				} else {
					_dict[key] = value;
				}
			}
		}
        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "Options", resourceName, null, defaultValue, substitutes);
        }
    }
}
