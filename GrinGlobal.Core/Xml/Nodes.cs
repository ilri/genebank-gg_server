using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Text;

namespace GrinGlobal.Core.Xml {
	/// <summary>
	/// Represents a collection of Node objects
	/// </summary>
	public class Nodes : NodeCollectionBase 
	{
		/// <summary>
		/// Creates a new Nodes collection to house Node objects
		/// </summary>
		public Nodes()
		{
		}



		/// <summary>
		/// Gets the Node at the given index
		/// </summary>
		public new Node this[int index]{
			get { return (Node)base[index]; }
		}


		/// <summary>
		/// Reads all nodes at the current depth, and their children, from the input stream
		/// </summary>
		/// <param name="xtr">Input stream to read from</param>
		/// <returns>True always</returns>
		public override bool Deserialize(XmlTextReader xtr) {
#if TRACK_TIME
			using(HighPrecisionTimer tmr = new HighPrecisionTimer("Nodes.Deserialize", true)){
#endif
				// spin until we hit a begin element or end element
				// move to next node
				while (xtr.NodeType != XmlNodeType.Element && xtr.NodeType != XmlNodeType.EndElement){
					xtr.Read();
				}

				int depth = xtr.Depth;
				// if we're on an element, spin until we're out of elements at this same depth
				while (depth == xtr.Depth && xtr.NodeType == XmlNodeType.Element){
					Node gn = new Node();
					gn.Deserialize(xtr);
					Add(gn);

					// ignore any whitespace
					while(xtr.NodeType == XmlNodeType.Whitespace){
						xtr.Read();
					}

				}
				return true;
#if TRACK_TIME
			}
#endif
		}

		/// <summary>
		/// Writes this Node to the ouput stream
		/// </summary>
		/// <param name="xtw">stream to write to</param>
		public override void Serialize(XmlTextWriter xtw, bool preserveWhiteSpace) {
#if TRACK_TIME
			using(HighPrecisionTimer tmr = new HighPrecisionTimer("Nodes.Serialize", true)){
#endif
				foreach(Node gn in this){
					gn.Serialize(xtw, preserveWhiteSpace);
				}
#if TRACK_TIME
			}
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			StringWriter sw = new StringWriter();
			XmlTextWriter xtw = new XmlTextWriter(sw);
			this.Serialize(xtw, true);
			return sw.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Nodes Clone(){
#if TRACK_TIME
			using(HighPrecisionTimer tmr = new HighPrecisionTimer("Nodes.Clone", true)){
#endif
				Nodes gns = new Nodes();
				foreach(Node gn in this){
					gns.Add(gn.Clone());
				}
				return gns;
#if TRACK_TIME
			}
#endif
		}
	}
}
