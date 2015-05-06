using System;
using System.Xml;
using System.Text;

namespace GrinGlobal.Core.Xml
{
	/// <summary>
	/// Represents all attributes not already defined within an object
	/// </summary>
	public class Attribute
	{
		/// <summary>
		/// Creates a new Attribute with null name and null value
		/// </summary>
		public Attribute() {
		}


		/// <summary>
		/// Creates a new Attribute with given name and value
		/// </summary>
		/// <param name="name">Name of Attribute</param>
		/// <param name="val">Value of Attribute</param>
		public Attribute(string name, string val) {
			_name = name;
			_value = val;
		}

		/// <summary>
		/// Creates a new object and copies the .Name and .Value properties and returns it
		/// </summary>
		/// <returns></returns>
		public Attribute Clone(){
			Attribute att = new Attribute(this.Name, this.Value);
			return att;
		}

		private string _name;
		private string _value;

		/// <summary>
		/// Gets or sets the Name of this attribute
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
		/// Gets or sets the Value of this attribute
		/// </summary>
		public string Value {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}

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
