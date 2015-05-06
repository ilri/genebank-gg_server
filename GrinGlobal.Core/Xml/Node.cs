using System;
using System.IO;
using System.Text;
using System.Xml;

namespace GrinGlobal.Core.Xml {
	/// <summary>
	/// Represents any Xml node
	/// </summary>
	public class Node {

		/// <summary>
		/// Creates a new Generic node with given name and marks excludedAttributes that are not to be added to Attributes when read() is called
		/// </summary>
		/// <param name="nodeName">Name of node</param>
		/// <param name="excludedAttributes">Attributes that should be excluded from the Attributes collection during read()</param>
		public Node(string nodeName, string[] excludedAttributes) : this(nodeName, excludedAttributes, false) {
		}

		/// <summary>
		/// Creates a new Generic node with given name and marks excludedAttributes that are not to be added to Attributes when read() is called.  Optionally allows node to be renamed during a read() call.
		/// </summary>
		/// <param name="nodeName">Name of node</param>
		/// <param name="excludedAttributes">Attributes that should be excluded from the Attributes collection during read()</param>
		/// <param name="allowNodeRenaming"></param>
		public Node(string nodeName, string[] excludedAttributes, bool allowNodeRenaming) {
			_nodeName = nodeName;
			_allowNodeRenaming = allowNodeRenaming;
			_excludedAttributes = excludedAttributes;
			_attributes = new Attributes(excludedAttributes);
			_nodes = new Nodes();
		}

		/// <summary>
		/// Creates a new Generic node with given name 
		/// </summary>
		/// <param name="nodeName">Name of node</param>
		public Node(string nodeName) : this(nodeName, null, false) {
		}

		/// <summary>
		/// Creates a new Node with a name of "Node" 
		/// </summary>
		public Node(): this("Node", null, false) {
		}

		/// <summary>
		/// Creates a new Node with a name of "Generic" which may optionally be changed during Deserialization
		/// </summary>
		/// <param name="allowNodeRenaming">Allow name of node to change during Deserialization</param>
		public Node(bool allowNodeRenaming) : this("Generic", null, allowNodeRenaming){
		}

		private bool _allowNodeRenaming;

		/// <summary>
		/// A collection of Nodes underneath this Node
		/// </summary>
		protected Nodes _nodes;
		/// <summary>
		/// Gets any nodes contained by this node
		/// </summary>
		public Nodes Nodes {
			get { return _nodes; }
		}

		private string[] _excludedAttributes;

		private string _nodeName;
		/// <summary>
		/// Gets or sets the name of the node
		/// </summary>
		public string NodeName {
			get { return _nodeName; }
			set { _nodeName = value; }
		}

		private string _nodeValue;
		/// <summary>
		/// Gets or sets the text value of the node (#Text element)
		/// </summary>
		public string NodeValue {
			get { return _nodeValue; }
			set {
                _nodeValue = value;

                // _nodeValue = (value == null ? null : value); 
            }

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="childName"></param>
		/// <returns></returns>
		public string ChildNodeValue(string childName) {
			Node gn = _nodes[childName];
			if (gn != null) {
				return gn.NodeValue;
			}
			return null;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="childName"></param>
		/// <param name="grandChildName"></param>
		/// <returns></returns>
		public string GrandChildNodeValue(string childName, string grandChildName){
			Node gn = _nodes[childName];
			if (gn != null) {
				Node gn2 = gn.Nodes[grandChildName];
				if (gn2 != null){
					return gn2.NodeValue;
				}
			}
			return null;
		}

		private string _prefix;
		/// <summary>
		/// Gets or sets the Prefix to use for the Name and Value attributes (and only those -- all other attribute prefixes are considered part of the name.)
		/// </summary>
		public string NameAndValuePrefix {
			get { return _prefix; }
			set { _prefix = value; }
		}

		/// <summary>
		/// Writes out the beginning of the node, including the Attributes.  (i.e. &lt;NodeName name='' value='' optAtt1='' optAtt2='')
		/// </summary>
		/// <param name="xtw">Stream to write to</param>
		public void writeStartNode(XmlTextWriter xtw){
			xtw.WriteStartElement(_nodeName);
			_attributes.Serialize(xtw);
		}

		/// <summary>
		/// Writes out the ending of the node, starting with the generic child nodes, then #Text, then node close (i.e. gt;lt;child1 name='' value=''/gt;lt;/NodeNamegt;
		/// </summary>
		/// <param name="xtw"></param>
		public void writeEndNode(XmlTextWriter xtw, bool preserveWhiteSpace){
			_nodes.Serialize(xtw, preserveWhiteSpace);
            if (preserveWhiteSpace) {
                xtw.WriteString(_nodeValue);
            } else {
                xtw.WriteString(_nodeValue == null ? null : _nodeValue.Trim());
            }
			xtw.WriteEndElement();
		}

		/// <summary>
		/// Reads the next node from the input stream.  Note whitespace, comments, and CDATA is ignored.
		/// </summary>
		/// <param name="xtr">Input stream to read from</param>
		/// <returns>True, always</returns>
		public virtual bool Deserialize(XmlTextReader xtr){
#if TRACK_TIME
			using(HighPrecisionTimer tmr = new HighPrecisionTimer("Node.Deserialize", true)){
#endif

				// assumes xtr is at start of an element
				if (xtr.NodeType != XmlNodeType.Element){
					return true;
				}

				string nm = xtr.Name;

				bool skipRead = false;
				do {
					if (xtr.NodeType == XmlNodeType.EndElement && xtr.Name == nm){
						// we've processed our node.
						// move to the next node
						int depth = xtr.Depth;
						while (xtr.NodeType != XmlNodeType.Element && xtr.NodeType != XmlNodeType.None && depth == xtr.Depth){
							xtr.Read();
						}
						return true;
					}

					switch(xtr.NodeType){
						case XmlNodeType.Element:
							if (xtr.Name == nm){
								if (_allowNodeRenaming){
									_nodeName = xtr.Name;
								}
								if (Attributes.Deserialize(xtr)){
									// no more data in this guy -- he's empty.
									xtr.Read();
									return true;
								}
							} else {
								// this is a contained node. add to generic nodes.
								Node gn = new Node(_allowNodeRenaming);
								_nodes.Add(gn);
								// read it
								gn.Deserialize(xtr);
								skipRead = true;
							}
							break;
						case XmlNodeType.EndElement:
							return true;
						case XmlNodeType.Text:
							_nodeValue = xtr.Value;
							break;
							//					case XmlNodeType.Whitespace:
							//						System.Diagnostics.Debug.WriteLine("skipping whitespace");
							//						break;
					}

					if (!skipRead){
						xtr.Read();
					}
					skipRead = false;

				} while(xtr.NodeType != XmlNodeType.None);

				return true;

#if TRACK_TIME
			}
#endif
		}

		/// <summary>
		/// Writes the node to the output stream
		/// </summary>
		/// <param name="xtw">Output stream to write to</param>
		public virtual void Serialize(XmlTextWriter xtw, bool preserveWhiteSpace){
#if TRACK_TIME
			using(HighPrecisionTimer tmr = new HighPrecisionTimer("Node.Serialize", true)){
#endif
				writeStartNode(xtw);
				writeEndNode(xtw, preserveWhiteSpace);
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

        public string InnerXml() {
            using (StringWriter sw = new StringWriter()) {
                using (XmlTextWriter xtw = new XmlTextWriter(sw)) {
                    _nodes.Serialize(xtw, true);
                    xtw.WriteString(_nodeValue);
                }
                return sw.ToString();
            }
        }


		#region IAttributes Members

		/// <summary>
		/// Represents all non-required attributes
		/// </summary>
		protected Attributes _attributes;
		/// <summary>
		/// Gets the attributes associated with this node.
		/// </summary>
		public Attributes Attributes {
			get { return _attributes; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Node Clone(){
#if TRACK_TIME
			using(HighPrecisionTimer tmr = new HighPrecisionTimer("Node.Clone", true)){
#endif
				Node gn = new Node(this.NodeName, _excludedAttributes, _allowNodeRenaming);
				gn.NodeValue = this.NodeValue;
				foreach(Node gn2 in this.Nodes){
					gn.Nodes.Add(gn2.Clone());
				}
				foreach(Attribute oa in this.Attributes){
					gn.Attributes.Add(oa.Clone());
				}
				return gn;
#if TRACK_TIME
			}
#endif
		}

		#endregion

		#region INameValue Members

		/// <summary>
		/// Gets or sets the "Name" attribute
		/// </summary>
		public virtual string Name {
			get {
				return _attributes.GetValue(_prefix + "Name");
			}
			set {
				_attributes.SetValue(_prefix + "Name", value);
			}
		}

		/// <summary>
		/// Gets or sets the "Value" attribute
		/// </summary>
		public virtual string Value {
			get {
				return _attributes.GetValue("Value");
			}
			set {
				_attributes.SetValue("Value", value);
			}
		}

		#endregion
	}

}
