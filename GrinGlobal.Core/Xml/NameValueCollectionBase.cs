using System;
using System.Collections;
using System.Xml;

namespace GrinGlobal.Core.Xml {
	/// <summary>
	/// Summary description for NameValueCollectionBase.
	/// </summary>
	public abstract class NameValueCollectionBase : CollectionBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public virtual INameValue this[int index]{
			get { return (INameValue)List[index]; }
			set { List[index] = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public virtual INameValue this[string name]{
			get {
				int idx = findIndex(name);
				if (idx > -1){
					return (INameValue)List[idx];
				} else {
					return null;
				}
			}
			set {
				int idx = findIndex(name);
				if (idx > -1){
					List[idx] = value;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public virtual string GetName(string name){
			INameValue nv = this[name];
			if (nv == null){
				return null;
			} else {
				return nv.Name;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public virtual string GetValue(string name){
			return GetValue(name, null);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public virtual string GetValue(string name, string defaultValue){
			INameValue nv = this[name];
			if (nv == null){
				return defaultValue;
			} else {
				return nv.Value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		protected int findIndex(string name){
			for(int i=0;i<List.Count;i++){
				if (((INameValue)List[i]).Name == name){
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nv"></param>
		/// <returns></returns>
		public virtual int Add(INameValue nv){
			return List.Add(nv);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nv"></param>
		public virtual void Remove(INameValue nv){
			List.Remove(nv);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public virtual void Remove(string name){
			INameValue inv = this[name];
			if (inv != null){
				List.Remove(inv);
			}
		}
	}
}
