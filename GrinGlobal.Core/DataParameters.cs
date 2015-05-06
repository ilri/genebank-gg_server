using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using System.Data;

namespace GrinGlobal.Core {
	/// <summary>
	/// 
	/// </summary>
	[ComVisible(false)]
#if !DEBUGDATAMANAGER
		[DebuggerStepThrough()]
#endif
	public class DataParameters : System.Collections.Generic.List<DataParameter> {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterKeysAndValues"></param>
		public DataParameters(params object[] parameterKeysAndValues) {
			//if (parameterKeysAndValues.Length % 2 != 0) {
			//    
			//}
			for (int i = 0; i < parameterKeysAndValues.Length; i += 2) {

				if (i+1 == parameterKeysAndValues.Length){
					throw new InvalidOperationException(ResourceHelper.GetDisplayMember(null, "Core", "DataParameters", "DataParameters_constructor", null, "Each key must have a corresponding value.  The patterns supported match the parameter patterns of all the DataParameter constructor overloads.  At least one set of the parameters you gave do not match any of these patterns."));
				}

				DataParameter dp = new DataParameter((string)parameterKeysAndValues[i], parameterKeysAndValues[i + 1]);

				int jumpBy = 0;

				// if they specified the 3rd entry in a tuple as DbType, apply it to the current DataParameter
				if (i < parameterKeysAndValues.Length - 2) {
					if (parameterKeysAndValues[i + 2] != null){
						if (parameterKeysAndValues[i + 2].GetType() == typeof(DbType)) {
							// support the DataParameter overload with (name, value, type)
							dp.DbType = (DbType)parameterKeysAndValues[i + 2];
							jumpBy = 1;
                        } else if (parameterKeysAndValues[i+2].GetType() == typeof(DbPseudoType)){
                            dp.DbPseudoType = (DbPseudoType)parameterKeysAndValues[i + 2];
                            jumpBy = 1;
                        } else if (parameterKeysAndValues[i + 2].GetType() == typeof(ParameterDirection)) {
							// support the DataParameter overload with (name, value, direction)
							dp.Direction = (ParameterDirection)parameterKeysAndValues[i + 2];
							jumpBy = 1;
							if (i < parameterKeysAndValues.Length - 3) {
								if (parameterKeysAndValues[i + 3] != null) {
									if (parameterKeysAndValues[i + 3].GetType() == typeof(bool)) {
										// support the DataParameter overload with (name, value, direction, treatNullAsDBNull)
										dp.TreatNullAsDBNull = (bool)parameterKeysAndValues[i + 3];
										jumpBy++;
									}
									if (parameterKeysAndValues[i + 4] != null) {
										if (parameterKeysAndValues[i+4].GetType() == typeof(DbType)) {
											// support the DataParameter overload with (name, value, direction, treatNullAsDBNull, type)
											dp.DbType = (DbType)parameterKeysAndValues[i + 4];
											jumpBy++;
										}
									}
								}
							}
						}
					}
				}
				i += jumpBy;
				this.Add(dp);
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataParameters"></param>
		public DataParameters(params DataParameter[] dataParameters) {
			this.AddRange(dataParameters);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataParameters"></param>
		public DataParameters(List<DataParameter> dataParameters) {
			this.AddRange(dataParameters);
		}

		/// <summary>
		/// 
		/// </summary>
		public DataParameters() {
		}

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			foreach (DataParameter dp in this) {
				sb.Append(dp.ToString() + "; ");
			}
			if (sb.Length > 2) {
				sb.Remove(sb.Length - 2, 2);
			}
			string ret = sb.ToString();
			return ret;
		}

    }
}
