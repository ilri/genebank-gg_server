using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Text;

namespace GrinGlobal.Core.Xml {
	/// <summary>
	/// Represents all xml attributes of a given object
	/// </summary>
	public class Attributes : NameObjectCollectionBase
	{
		/// <summary>
		/// Creates a collection of Attributes, and a list of attribute names that should be excluded when Reading
		/// </summary>
		/// <param name="excludedAtts"></param>
		public Attributes(string[] excludedAtts)
		{
			_excludedAtts = excludedAtts;

		}

		/// <summary>
		/// Adds a new Attribute to the collection
		/// </summary>
		/// <param name="name">Name of Attribute</param>
		/// <param name="val">value of Attribute</param>
		/// <returns>Index of Attribute in the Collection</returns>
		public int Add(string name, string val){
			return Add(new Attribute(name, val));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="att"></param>
		/// <returns></returns>
		public int Add(Attribute att){
			BaseAdd(att.Name, att);
			return this.Count - 1;
		}

		/// <summary>
		/// Gets or sets the Attribute at the given offset
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public virtual Attribute this[int index]{
			get { return (Attribute)BaseGet(index); }
			set { BaseSet(index, value); }
		}

		/// <summary>
		/// Gets or sets the Attribute with the given Name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public virtual Attribute this[string name]{
			get {
				return (Attribute)BaseGet(name);
//				int idx = findIndex(name);
//				if (idx > -1){
//					return (Attribute)List[idx];
//				} else {
//					return null;
//				}
			}
			set {
				BaseSet(name, value);
//				int idx = findIndex(name);
//				if (idx > -1){
//					List[idx] = value;
//				}
			}
		}

		/// <summary>
		/// Gets the Name property fro the specified attribute
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public virtual string GetName(string name){
			Attribute att = this[name];
			if (att == null){
				return null;
			} else {
				return att.Name;
			}
		}

		/// <summary>
		/// If the attribute with the given name exists, returns its value.  Otherwise returns null.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public virtual string GetValue(string name){
			return GetValue(name, null);
		}

		/// <summary>
		/// If the attribute with the given name exists and its value is not null, returns its value.  Otherwise returns defaultValue.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public virtual string GetValue(string name, string defaultValue){
			Attribute att = this[name];
			if (att == null){
				return defaultValue;
			} else {
				if (att.Value == null){
					return defaultValue;
				} else {
					return att.Value;
				}
			}
		}


//		protected int findIndex(string name){
//			for(int i=0;i<List.Count;i++){
//				if (((Attribute)List[i]).Name == name){
//					return i;
//				}
//			}
//			return -1;
//		}






		/// <summary>
		/// Reads all Attributes on the current node from the input stream 
		/// </summary>
		/// <param name="xtr">Stream to read from</param>
		/// <returns>True if the current node is not empty, false otherwise</returns>
		/// <remarks>If the current node is not empty, it moves the cursor to the next node in the stream</remarks>
		public bool Deserialize(XmlTextReader xtr) {
			if (xtr.MoveToFirstAttribute()){
				bool skip = false;
				if (_excludedAtts == null){
					// no attributes to exclude. read all.
					do{
						// may already exist, so call SetValue to update/append as needed
						SetValue(xtr.Name, xtr.Value);
					} while(xtr.MoveToNextAttribute());
				} else {
					do{
						skip = false;
						for(int i=0;i<_excludedAtts.Length;i++){
							if (_excludedAtts[i] == xtr.Name){
								skip = true;
								break;
							}
						}
						if (!skip){
							// may already exist, so call SetValue to update/append as needed
							SetValue(xtr.Name, xtr.Value);
						}
					} while(xtr.MoveToNextAttribute());
				}
				// move back to the element as if we never moved in the first place
				xtr.MoveToElement();
			}
			return xtr.IsEmptyElement;
		}

		private string[] _excludedAtts;

		/// <summary>
		/// Writes all Attributes in the collection to the output stream
		/// </summary>
		/// <param name="xtw">Stream to write to</param>
		public void Serialize(XmlTextWriter xtw) {
			foreach(string key in this.Keys){
				((Attribute)BaseGet(key)).Serialize(xtw);
			}
		}

		/// <summary>
		/// Sets the given attribute name.  If Attribute does not exist, creates a new one and appends it to the node.
		/// </summary>
		/// <param name="attName"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public bool SetName(string attName, string val){
			Attribute att = (Attribute)this[attName];
			if (att == null){
				this.Add(attName, val);
				return true;
			} else {
				att.Name = val;
				return false;
			}
		}

		/// <summary>
		/// Sets the given attribute's Value.  Creates and appends the Attribute to the node if it does not exist.
		/// </summary>
		/// <param name="attName"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public bool SetValue(string attName, string val){
			Attribute att = (Attribute)this[attName];
			if (att == null){
				this.Add(attName, val);
				return true;
			} else {
				att.Value = val;
				return false;
			}
		}
	}
}
