using System;
using System.Xml;
using System.IO;
using System.Text;

namespace GrinGlobal.Core.Xml
{
	/// <summary>
	/// Summary description for Document.
	/// </summary>
	public class Document : IDisposable
	{
		/// <summary>
		/// Creates a new Document with .Root node == 'Node'
		/// </summary>
		public Document()
		{
			_root = new Node();
            PreserveWhiteSpace = true;
		}

		/// <summary>
		/// Creates a new document with Root node of given rootNodeName
		/// </summary>
		/// <param name="rootNodeName"></param>
		public Document(string rootNodeName) {
			_root = new Node(rootNodeName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		public virtual void Load(string url){
#if TRACK_TIME
			using(HighPrecisionTimer tmr = new HighPrecisionTimer("Document.Load(String url)", true)){
#endif
				FileStream fs = new FileStream(url, FileMode.OpenOrCreate);
				load(new XmlTextReader(new StreamReader(fs)));
				fs.Close();
#if TRACK_TIME
			}
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		public virtual void LoadXml(Stream input){
#if TRACK_TIME
			using(HighPrecisionTimer tmr = new HighPrecisionTimer("Document.LoadXml(Stream input)", true)){
#endif
				load(new XmlTextReader(input));
#if TRACK_TIME
			}
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xml"></param>
		public virtual void LoadXml(string xml){
#if TRACK_TIME
			using(HighPrecisionTimer tmr = new HighPrecisionTimer("Document.LoadXml(String xml)", true)){
#endif
				load(new XmlTextReader(new StringReader(xml)));
#if TRACK_TIME
			}
#endif
		}

		private void load(XmlTextReader xtr){
#if TRACK_TIME
			using(HighPrecisionTimer tmr = new HighPrecisionTimer("Document.load", true)){
#endif
			// create the root node
				_root = new Node(true);

				// move reader to first element
				while (xtr.Read() && xtr.NodeType != XmlNodeType.Element);

				// deserialize into our generic node.
				_root.Deserialize(xtr);

				// close the reader
				xtr.Close();
#if TRACK_TIME
			}
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		protected Node _root;
		/// <summary>
		/// 
		/// </summary>
		public Node Root {
			get { return _root; }
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void Close(){
		}


        public bool PreserveWhiteSpace { get; set; }


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string ToXml() {
			StringWriter sw = new StringWriter();
			XmlTextWriter xtw = new XmlTextWriter(sw);
			xtw.Formatting = Formatting.Indented;
			xtw.IndentChar = '\t';
			xtw.Indentation = 1;
			xtw.QuoteChar = '\'';
			_root.Serialize(xtw, PreserveWhiteSpace);
			string output = sw.ToString();
			return output;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		public void Save(string path) {
			path = Path.GetFullPath(path);
			string output = ToXml();
			File.WriteAllText(path, output);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			StringWriter sw = new StringWriter();
			XmlTextWriter xtw = new XmlTextWriter(sw);
			xtw.Formatting = Formatting.Indented;
			xtw.IndentChar = '\t';
			xtw.Indentation = 1;
			xtw.QuoteChar = '\'';
			_root.Serialize(xtw, PreserveWhiteSpace);
			string output = sw.ToString();
			return output;
		}

		#region IDisposable Members
		/// <summary>
		/// 
		/// </summary>
		public virtual void Dispose() {
			Close();
		}

		#endregion
	}
}
