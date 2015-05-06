using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using System.Data;
using System.IO;
using System.Xml;

namespace GrinGlobal.Core {
	/// <summary>
	/// 
	/// </summary>
	[XmlRoot("SerializableDictionary")]
	[Serializable]
	public class SerializableDictionary
		: Dictionary<string, object>, IXmlSerializable {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyValuePairs"></param>
		public SerializableDictionary(object[] keyValuePairs) {
			if (keyValuePairs != null && keyValuePairs.Length > 0) {
				if (keyValuePairs.Length % 2 != 0) {
					throw new ArgumentException(ResourceHelper.GetDisplayMember(null, "Core", "SerializableDictionary", "constructor", null, "You must pass an even number of items in the keyValuePairs constructor overload for SerializableDictionary."));
				} else {
					for (int i = 0; i < keyValuePairs.Length; i += 2) {
						string key = (string)keyValuePairs[i];
						object val = keyValuePairs[i + 1];
						this.Add(key, val);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SerializableDictionary() {
		}

        public static SerializableDictionary Load(string fileName) {
            var fn = Toolkit.ResolveFilePath(fileName, false);
            var dic = new SerializableDictionary();
            if (File.Exists(fn)) {
                using (var xr = new XmlTextReader(fn)) {
                    xr.ReadStartElement();
                    dic.ReadXml(xr);
                }
            }
            return dic;
        }

        public void Save(string fileName) {
            var fn = Toolkit.ResolveFilePath(fileName, false);
            using (var xw = new XmlTextWriter(fn, UnicodeEncoding.Unicode)) {
                xw.WriteStartDocument();
                xw.WriteStartElement("SerializableDictionary");
                this.WriteXml(xw);
                xw.WriteEndElement();
            }
        }

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<string, object> kvp in this) {
				sb.Append(kvp.Key + "=" + kvp.Value.ToString() + "\n");
			}
			string temp = sb.ToString();
			return temp;
		} 

		#region IXmlSerializable Members
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public System.Xml.Schema.XmlSchema GetSchema() {
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		public void ReadXml(System.Xml.XmlReader reader) {
			XmlSerializer keySerializer = new XmlSerializer(typeof(string));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(object));

            if (reader.Name == "SerializableDictionary") {
                bool wasEmpty = reader.IsEmptyElement;
                // skips the "SerializableDictionary" element start
                reader.Read();
                if (wasEmpty) {
                    return;
                }
            }

			//// skips the "SerializableDictionaryItems" element start
			//wasEmpty = reader.IsEmptyElement;
			//reader.Read();
			//if (wasEmpty) {
			//    return;
			//}

			while (reader.NodeType != System.Xml.XmlNodeType.EndElement) {
				reader.ReadStartElement("item");

				reader.ReadStartElement("key");
				string key = (string)keySerializer.Deserialize(reader);
				reader.ReadEndElement();

				reader.ReadStartElement("value");
				object value = (object)valueSerializer.Deserialize(reader);
				reader.ReadEndElement();

				this.Add(key, value);

				reader.ReadEndElement();
				reader.MoveToContent();
			}
			// skip the "SerializableDictionaryItems" element end
			//		reader.ReadEndElement();
			// skip the "SerializableDictionary" element end
			reader.ReadEndElement();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		public void WriteXml(System.Xml.XmlWriter writer) {
			XmlSerializer keySerializer = new XmlSerializer(typeof(string));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(object));

			//		writer.WriteStartElement("SerializableDictionaryItems");

			foreach (string key in this.Keys) {
				writer.WriteStartElement("item");

				writer.WriteStartElement("key");
				keySerializer.Serialize(writer, key);
				writer.WriteEndElement();

				writer.WriteStartElement("value");
				object value = this[key];
				if (value == DBNull.Value) {
					// DBNull can't be serialized properly.  bleh.
					value = null;
				}
				valueSerializer.Serialize(writer, value);
				writer.WriteEndElement();

				writer.WriteEndElement();
			}

			//		writer.WriteEndElement();
		}

		/// <summary>
		/// Converts the given DataRow to a SerializableDictionary object (suitable for returning via a web service)
		/// </summary>
		/// <param name="dr"></param>
		/// <returns></returns>
		public static SerializableDictionary Convert(DataRow dr) {
			SerializableDictionary sd = new SerializableDictionary();
			DataTable dt = dr.Table;
			for (int j = 0; j < dt.Columns.Count; j++) {
				sd.Add(dt.Columns[j].ColumnName, dr[j]);
			}
			return sd;
		}

		/// <summary>
		/// Converts the given DataTable to a SerializableDictionary[] suitable for returning via a web service
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static SerializableDictionary[] Convert(DataTable dt) {
			List<SerializableDictionary> dics = new List<SerializableDictionary>();
			for (int i = 0; i < dt.Rows.Count; i++) {
				SerializableDictionary sd = SerializableDictionary.Convert(dt.Rows[i]);
				dics.Add(sd);
			}
			return dics.ToArray();
		}
		#endregion
	}

}