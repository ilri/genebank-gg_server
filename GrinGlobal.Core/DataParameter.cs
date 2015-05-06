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
	public class DataParameter {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public DataParameter(string key, object value)
            : this(key, value, ParameterDirection.Input, true, DataParameter.DeriveDbType(value), DataParameter.DeriveDbPseudoType(value)) {
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public DataParameter(string key, object value, Type type) :
            this(key, value, ParameterDirection.Input, true, DataParameter.MapDbType(type), DataParameter.MapDbPseudoType(type)) {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="direction"></param>
		public DataParameter(string key, object value, ParameterDirection direction)
			: this(key, value, direction, true, DataParameter.DeriveDbType(value), DataParameter.DeriveDbPseudoType(value)) {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="direction"></param>
		/// <param name="treatNullAsDBNull"></param>
		/// <param name="type"></param>
		public DataParameter(string key, object value, ParameterDirection direction, bool treatNullAsDBNull, DbType type, DbPseudoType pseudoType) {
			_key = key;
			_value = value;
			_direction = direction;
			_treatNullAsDBNull = treatNullAsDBNull;
			_dbType = type;
            _dbPseudoType = pseudoType;
			Size = null;
			castValueToProperType();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="direction"></param>
		/// <param name="treatNullAsDBNull"></param>
		public DataParameter(string key, object value, ParameterDirection direction, bool treatNullAsDBNull)
			: this(key, value, direction, treatNullAsDBNull, DataParameter.DeriveDbType(value), DataParameter.DeriveDbPseudoType(value)) {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="type"></param>
		public DataParameter(string key, object value, DbType type)
			: this(key, value, ParameterDirection.Input, true, type, DeriveDbPseudoType(value)) {

		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public DataParameter(string key, object value, DbType type, DbPseudoType pseudoType)
            : this(key, value, ParameterDirection.Input, true, type, pseudoType) {
        }

        /// <summary>
        /// Creates a new dataparameter with DbType.Xml and given pseudotype.  DbType.Xml is used as a placeholder only and is ignored by DataManager.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="pseudoType"></param>
        public DataParameter(string key, object value, DbPseudoType pseudoType)
        :this(key, value, ParameterDirection.Input, true, DbType.Xml, pseudoType ) {

        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="type"></param>
		/// <param name="direction"></param>
		public DataParameter(string key, object value, DbType type, ParameterDirection direction)
            : this(key, value, direction, true, type, DataParameter.DeriveDbPseudoType(value)) {
		}

		/// <summary>
		/// Gets the Key name without the symbol (e.g. '@id' or ':id' or '?id' is returned as 'id')
		/// </summary>
		public string CleanKey {
			get {
				return (_key == null ? null : _key.Replace(":", "").Replace("?", "").Replace("@", ""));
			}
		}

		private string _key;
		/// <summary>
		/// 
		/// </summary>
		public string Key {
			get { return _key; }
			set { _key = value; }
		}

		private object _value;
		/// <summary>
		/// 
		/// </summary>
		public object Value {
			get { return (_value == null && _treatNullAsDBNull ? DBNull.Value : _value); }
			set {
				if (_value != value) {
					_value = value;
					castValueToProperType();
				}
			}
		}

		public int? Precision { get; set; }
		public int? Scale { get; set; }

		public int? Size { get; set; }

		private DbType _dbType;
		public DbType DbType {
			get {
				return _dbType;
			}
			set {
				if (_dbType != value) {
					_dbType = value;
					castValueToProperType();
				}
			}
		}

		private ParameterDirection _direction;
		/// <summary>
		/// 
		/// </summary>
		public ParameterDirection Direction {
			get { return _direction; }
			set { _direction = value; }
		}

        private DbPseudoType _dbPseudoType;
        public DbPseudoType DbPseudoType {
            get {
                return _dbPseudoType;
            }
            set {
                _dbPseudoType = value;
                if (_dbPseudoType != DbPseudoType.Unknown) {
                    // if they're giving us a valid pseudotype,
                    // change the dbtype so it is processed properly.
                    _dbType = DbType.Xml;
                }
            }
        }

		private bool _treatNullAsDBNull;
		/// <summary>
		/// 
		/// </summary>
		public bool TreatNullAsDBNull {
			get { return _treatNullAsDBNull; }
			set { _treatNullAsDBNull = value; }
		}

		public override string ToString() {
			//return "key=" + _key + ", value=" + _value + ", dbtype=" + _dbType + ", dir=" + _direction + ", DBNullAsNull=" + _treatNullAsDBNull;
            if (_value is List<int>) {
                return _key + "=" + Toolkit.Join(((List<int>)_value).ToArray(), ",", "0", null, null);
            } else if (_value is int[]){
                return _key + "=" + Toolkit.Join(((int[])_value), ",", "0", null, null);
            } else if (_value is List<string>){
                return _key + "=" + Toolkit.Join(((List<string>)_value).ToArray(), "','", "", "'", "'");
            } else if (_value is string[]) {
                return _key + "=" + Toolkit.Join(((string[])_value), "','", "", "'", "'");
            } else {
                return _key + "=" + _value;
            }
		}

		private void castValueToProperType() {
			try {
				if (_value == DBNull.Value || _value == null) {
					// if it's DBNull or null, leave it alone
					return;
				}

                // HACK: added pseudo types (i.e. not values we pass as parameters to the database, but values we want to allow and substitute into the sql statement itself)
                if (_dbPseudoType == DbPseudoType.IntegerCollection) {
                    _dbType = DbType.Xml;
                } else if (_dbPseudoType == DbPseudoType.DecimalCollection) {
                    _dbType = DbType.Xml;
                    //} else if (_dbPseudoType == DbPseudoType.StringReplacement) {
                //    _dbType = DbType.Xml;
                }

				switch (_dbType) {
					case DbType.AnsiString:
					case DbType.AnsiStringFixedLength:
					case DbType.String:
					case DbType.StringFixedLength:
						_value = _value.ToString();
						break;
					case DbType.Binary:
						throw new NotImplementedException(getDisplayMember("castValueToProperType{binary}", "Parameter casting not implemented for DbType.Binary."));
					case DbType.Boolean:
						if (!(_value is bool)) {
							_value = Toolkit.ToBoolean(_value, null);
						}
						break;
					case DbType.Byte:
						throw new NotImplementedException(getDisplayMember("castValueToProperType{byte}", "Parameter casting not implemented for DbType.Byte."));
					case DbType.Date:
						//if (!(_value is DateTime)) {
						//    _value = Toolkit.ToDateTime(_value, null);
						//}
						//if (_value != null) {
						//    _value = ((DateTime)_value).ToString("yyyy-MM-dd");
						//}
						//break;
					case DbType.DateTime:
					case DbType.DateTime2:
						if (!(_value is DateTime)) {
							_value = Toolkit.ToDateTime(_value, null);
						}
						break;
					case DbType.DateTimeOffset:
						throw new NotImplementedException(getDisplayMember("castValueToProperType{datetimeoffset}", "Parameter casting not implemented for DbType.DateTimeOffset."));
					case DbType.Currency:
					case DbType.Decimal:
						if (!(_value is decimal)) {
							_value = Toolkit.ToDecimal(_value, null);
						}
						break;
					case DbType.Double:
						if (!(_value is double)) {
							_value = Toolkit.ToDouble(_value, null);
						}
						break;
					case DbType.Guid:
						if (!(_value is Guid)) {
							_value = Toolkit.ToGuid(_value, null);
						}
						break;
					case DbType.Int16:
						if (!(_value is short)) {
							_value = Toolkit.ToInt16(_value.ToString(), null);
						}
						break;
					case DbType.Int32:
						if (!(_value is int)) {
							_value = Toolkit.ToInt32(_value, null);
						}
						break;
					case DbType.Int64:
						if (!(_value is long)) {
							_value = Toolkit.ToInt64(_value, null);
						}
						break;
					case DbType.SByte:
						throw new NotImplementedException(getDisplayMember("castValueToProperType{sbyte}", "Parameter casting not implemented for DbType.SByte."));
					case DbType.Single:
						if (!(_value is float)) {
							_value = Toolkit.ToFloat(_value, null);
						}
						break;
					case DbType.Time:
						throw new NotImplementedException(getDisplayMember("castValueToProperType{time}", "Parameter casting not implemented for DbType.Time."));
					case DbType.UInt16:
						if (!(_value is UInt16)) {
							ushort us;
                            if (!UInt16.TryParse(_value.ToString(), out us)) {
                                throw new InvalidOperationException(getDisplayMember("castValueToProperType{uint16}", "Parameter with value = {0} could not be cast to UInt16.", (_value + "")));
                            } else {
                                _value = us;
                            }
						}
						break;
					case DbType.UInt32:
						if (!(_value is UInt32)) {
							uint ui;
                            if (!UInt32.TryParse(_value.ToString(), out ui)) {
                                throw new InvalidOperationException(getDisplayMember("castValueToProperType{uint32}", "Parameter with value = {0} could not be cast to UInt32.", (_value + "")));
                            } else {
                                _value = ui;
                            }
						}
						break;
					case DbType.UInt64:
						ulong ul;
						if (!(_value is UInt64)) {
							if (!UInt64.TryParse(_value.ToString(), out ul)) {
								throw new InvalidOperationException(getDisplayMember("castValueToProperType{uint64}", "Parameter with value = {0} could not be cast to UInt64.", (_value + "")));
							}
						}
						break;
					case DbType.Object:
						throw new NotImplementedException(getDisplayMember("castValueToProperType{object}", "Parameter casting not implemented for DbType.Object."));
					case DbType.VarNumeric:
						throw new NotImplementedException(getDisplayMember("castValueToProperType{varnumeric}", "Parameter casting not implemented for DbType.VarNumeric."));
					case DbType.Xml:
                        switch (_dbPseudoType) {
                            case DbPseudoType.Unknown:
                                throw new NotImplementedException(getDisplayMember("castValueToProperType{xml,unknown}", "If DbType.Xml is given, DbPseudoType must also be specified."));
                            case DbPseudoType.IntegerCollection:
                                if (_value is List<int>) {
                                    _value = (_value as List<int>).ToArray();
                                } else if (_value is int[]) {
                                    _value = (_value as int[]);
                                } else if (_value is string) {
                                    _value = Toolkit.ToIntList(_value as string).ToArray();
                                } else {
                                    throw new InvalidOperationException(getDisplayMember("castValueToProperType{xml,intcollection}", "Parameter with value = {0} could not be cast to int[].", (_value + "")));
                                }
                                break;
                            case DbPseudoType.DecimalCollection:
                                if (_value is List<decimal>) {
                                    _value = (_value as List<decimal>).ToArray();
                                } else if (_value is decimal[]) {
                                    _value = (_value as decimal[]);
                                } else if (_value is string) {
                                    _value = Toolkit.ToDecimalList(_value as string).ToArray();
                                } else {
                                    throw new InvalidOperationException(getDisplayMember("castValueToProperType{xml,decimalcollection}", "Parameter with value = {0} could not be cast to decimal[].", (_value + "")));
                                }
                                break;
                            case DbPseudoType.StringReplacement:
                                if (_value == null) {
                                    _value = "";
                                } else {
                                    _value = _value.ToString();
                                }

                                break;
                            case DbPseudoType.StringCollection:
                                if (_value is List<string>) {
                                    _value = (_value as List<string>).ToArray();
                                } else if (_value is string[]) {
                                    _value = (_value as string[]);
                                } else if (_value is string) {
                                    _value = new string[] { _value as string };
                                } else {
                                    throw new InvalidOperationException(getDisplayMember("castValueToProperType{xml,stringcollection}", "Parameter with value = {0} could not be cast to string[].", (_value + "")));
                                }
                                break;
                            default:
                                break;
                        }
                        break;
				}
			} catch (NotImplementedException) {
				// do nothing, we already know why
				throw;
			} catch (Exception) {
				// probably couldn't convert properly.
				throw;
			}
		}

		public static DbType MapDbType(string typeName) {
			switch(typeName.ToUpper()){
				case "STRING":
				case "SYSTEM.STRING":
					return MapDbType(typeof(string));
				case "DATE":
				case "TIME":
				case "DATETIME":
				case "DATETIME2":
				case "SYSTEM.DATETIME":
					return MapDbType(typeof(DateTime));
				case "INT":
				case "INT32":
				case "INTEGER":
				case "INTEGER32":
					return MapDbType(typeof(int));
				case "UINT":
				case "UINT32":
				case "UINTEGER":
				case "UINTEGER32":
				case "UNSIGNEDINT":
				case "UNSIGNEDINTEGER":
				case "UNSIGNEDINTEGER32":
					return MapDbType(typeof(uint));
				case "SHORT":
				case "INT16":
				case "INTEGER16":
					return MapDbType(typeof(Int16));
				case "USHORT":
				case "UINT16":
				case "UINTEGER16":
				case "UNSIGNEDSHORT":
				case "UNSIGNEDINT16":
				case "UNSIGNEDINTEGER16":
					return MapDbType(typeof(ushort));
				case "FLOAT":
				case "SINGLE":
					return MapDbType(typeof(float));
				case "DOUBLE":
					return MapDbType(typeof(double));
				case "DECIMAL":
					return MapDbType(typeof(Decimal));
				case "LONG":
				case "INT64":
				case "INTEGER64":
					return MapDbType(typeof(Int64));
				case "ULONG":
				case "UINT64":
				case "UINTEGER64":
				case "UNSIGNEDLONG":
				case "UNSIGNEDINT64":
				case "UNSIGNEDINTEGER64":
					return MapDbType(typeof(UInt64));
				case "BYTE":
					return MapDbType(typeof(byte));
				case "SBYTE":
					return MapDbType(typeof(SByte));
                case "INT32[]":
                case "SYSTEM.INT32[]":
                case "LIST<INT>":
                case "INTEGERCOLLECTION":
                    return DbType.Xml;
                case "DECIMAL[]":
                case "SYSTEM.DECIMAL[]":
                case "LIST<DECIMAL>":
                case "DECIMALCOLLECTION":
                    return DbType.Xml;
                case "STRINGREPLACEMENT":
                    return DbType.Xml;
                case "STRING[]":
                case "SYSTEM.STRING[]":
                case "LIST<STRING>":
                case "STRINGCOLLECTION":
                    return DbType.Xml;
				default:
					return MapDbType(typeof(string));
			}
		}

        public static DbPseudoType MapDbPseudoType(string typeName) {
            switch (typeName.ToUpper()) {
                case "INT[]":
                case "INT32[]":
                case "SYSTEM.INT32[]":
                case "INTEGERCOLLECTION":
                    return DbPseudoType.IntegerCollection;
                case "DECIMAL[]":
                case "SYSTEM.DECIMAL[]":
                case "LIST<DECIMAL>":
                case "DECIMALCOLLECTION":
                    return DbPseudoType.DecimalCollection;
                case "STRINGREPLACEMENT":
                    return DbPseudoType.StringReplacement;
                case "STRING[]":
                case "SYSTEM.STRING[]":
                case "LIST<STRING>":
                case "STRINGCOLLECTION":
                    return DbPseudoType.StringCollection;
                default:
                    return DbPseudoType.Unknown;
            }
        }


		public static DbType MapDbType(Type t) {
			if (t == typeof(int)){
				return DbType.Int32;
			} else if (t == typeof(uint)){
				return DbType.UInt32;
			} else if (t == typeof(string)){
				return DbType.String;
			} else if (t == typeof(DateTime)){
				return DbType.DateTime2;
			} else if (t == typeof(short)){
				return DbType.Int16;
			} else if (t == typeof(ushort)){
				return DbType.UInt16;
			} else if (t == typeof(decimal)){
				return DbType.Decimal;
			} else if (t == typeof(float)) {
				return DbType.Single;
			} else if (t == typeof(double)) {
				return DbType.Double;
			} else if (t == typeof(byte)){
				return DbType.Byte;
			} else if (t == typeof(sbyte)){
				return DbType.SByte;
			} else if (t == typeof(long)){
				return DbType.Int64;
			} else if (t == typeof(ulong)){
				return DbType.UInt64;
            } else if (t == typeof(int[]) || t == typeof(List<int>)){
                return DbType.Xml;
			} else if (t == typeof(string[]) || t == typeof(List<string>)){
                return DbType.Xml;
            } else {
				// punt!
				return DbType.String;
			}
		}

        public static DbPseudoType MapDbPseudoType(Type t) {
            if (t == typeof(int[]) || t == typeof(List<int>)) {
                return DbPseudoType.IntegerCollection;
            } else if (t == typeof(decimal[]) || t == typeof(List<decimal>)) {
                return DbPseudoType.DecimalCollection;
            } else if (t == typeof(string[]) || t == typeof(List<string>)) {
                return DbPseudoType.StringCollection;
            } else if (t == typeof(string)) {
                return DbPseudoType.StringReplacement;
            } else {
                return DbPseudoType.Unknown;
            }
        }


		public static DbType DeriveDbType(object value) {
			if (value == null || value == DBNull.Value) {
				// pfffft who knows? assume String
				return DbType.String;
			} else {

				if (value is string) {
					return DbType.String;
				} else if (value is DateTime) {
					return DbType.DateTime2;
				} else if (value is int) {
					return DbType.Int32;
				} else if (value is uint){
					return DbType.UInt32;
				} else if (value is decimal) {
					return DbType.Decimal;
				} else if (value is double) {
					return DbType.Double;
				} else if (value is long) {
					return DbType.Int64;
				} else if (value is ulong){
					return DbType.UInt64;
				} else if (value is short) {
					return DbType.Int16;
				} else if (value is ushort){
					return DbType.UInt16;
				} else if (value is float) {
					return DbType.Single;
				} else if (value is bool){
					return DbType.Boolean;
				} else if (value is Guid) {
					return DbType.Guid;
                } else if (value is int[] || value is List<int>){
                    return DbType.Xml;
                } else if (value is string[] || value is List<string>) {
                    return DbType.Xml;
                } else {
					throw new NotImplementedException(getDisplayMember("DeriveDbType", "DbType not mapped in DataParameter.DeriveDbType for value ='{0}' of type '{1}'", (value + ""), value.GetType().FullName));
				}
			}
		}

        public static DbPseudoType DeriveDbPseudoType(object value) {
            if (value == null || value == DBNull.Value) {
                // pfffft who knows?
                return DbPseudoType.Unknown;

            } else {
                if (value is int[] || value is List<int>) {
                    return DbPseudoType.IntegerCollection;
                } else if (value is decimal[] || value is List<decimal>) {
                    return DbPseudoType.DecimalCollection;
                } else if (value is string[] || value is List<string>){
                    return DbPseudoType.StringCollection;
                } else if (value is string) {
                    return DbPseudoType.StringReplacement;
                } else {
                    return DbPseudoType.Unknown;
                }
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "Core", "DataParameter", resourceName, null, defaultValue, substitutes);
        }


	}
}
