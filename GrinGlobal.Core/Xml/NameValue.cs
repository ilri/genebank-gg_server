using System;
using System.Xml;

namespace GrinGlobal.Core.Xml {
	/// <summary>
	/// Summary description for NameValue.
	/// </summary>
	public class NameValue : INameValue
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="val"></param>
		public NameValue(string name, string val)
		{
			_name = name;
			_value = val;
		}

		/// <summary>
		/// 
		/// </summary>
		public NameValue() : this(null,null){

		}

		private string _name;
		private string _value;

		#region INameValue Members
		/// <summary>
		/// 
		/// </summary>
		public string Name {
			get {
				return _name;
			}
			set {
				_name = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Value {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xtr"></param>
		/// <returns></returns>
		public bool Deserialize(XmlTextReader xtr){
			if (xtr.HasAttributes){
				if (xtr.MoveToNextAttribute()){
					_name = xtr.Name;
					_value = xtr.Value;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xtw"></param>
		public virtual void Serialize(XmlTextWriter xtw) {
			xtw.WriteAttributeString(_name, _value);
		}

	}
}
