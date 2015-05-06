using System;
using System.Collections;
using System.Xml;
using System.Text;

namespace GrinGlobal.Core.Xml {
	/// <summary>
	/// Summary description for NodeCollectionBase.
	/// </summary>
	public abstract class NodeCollectionBase : CollectionBase {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public virtual Node this[int index]{
			get { 
				return (Node)List[index];
			}
			set { List[index] = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="attName"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public virtual Node this[string attName, string val] {
			get {
				int idx = findIndexByAtt(attName, val);
				if (idx > -1){
					return (Node)List[idx];
				} else {
					return null;
				}
			}
			set {
				int idx = findIndexByAtt(attName, val);
				if (idx > -1){
					List[idx] = value;
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="nodeName"></param>
		/// <returns></returns>
		public virtual Node this[string nodeName]{
			get {
					int idx = findIndex(nodeName);
					if (idx > -1){
						return (Node)List[idx];
					} else {
						return null;
					}
			}
			set {
					int idx = findIndex(nodeName);
					if (idx > -1){
						List[idx] = value;
					}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="attName"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		protected int findIndexByAtt(string attName, string val){
			for(int i=0;i< this.Count;i++){
				Node gn = (Node)List[i];
				if (gn != null){
					Attribute att = gn.Attributes[attName];
					if (att != null){
						if (att.Value == val){
							return i;
						}
					}
				}
			}
			return -1;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		protected int findIndex(string name){
			for(int i=0;i<this.Count;i++){
				Node node = (Node)List[i];
				if (node.NodeName == name){
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nodes"></param>
		/// <returns></returns>
		public virtual void AddRange(Nodes nodes) {
			foreach (Node n in nodes) {
				Add(n);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ign"></param>
		/// <returns></returns>
		public virtual int Add(Node ign){
			return List.Add(ign);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ign"></param>
		public virtual void Remove(Node ign){
			List.Remove(ign.NodeName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public virtual void Remove(string name){
			List.Remove(name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xtr"></param>
		/// <returns></returns>
		public abstract bool Deserialize(XmlTextReader xtr);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xtw"></param>
		public abstract void Serialize(XmlTextWriter xtw, bool preserveWhiteSpace);
	}
}
