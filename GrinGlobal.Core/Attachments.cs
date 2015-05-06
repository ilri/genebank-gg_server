using System;
using System.Collections;
using System.Xml;
using System.Text;
using System.IO;

namespace GrinGlobal.Core {
	/// <summary>
	/// Summary description for Attachments.
	/// </summary>
	[Serializable]
	public class Attachments : CollectionBase {
		/// <summary>
		/// A collection of email attachments
		/// </summary>
		public Attachments() {
		}

		/// <summary>
		/// Adds the given attachment to this collection
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public int Add(Attachment data) {
			return (List.Add(data));
		}

		/// <summary>
		/// Gets the Attachment object at the given index
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Attachment this[int index] {
			get { return ((Attachment)List[index]); }
			set { List[index] = value; }
		}

		/// <summary>
		/// Removes the given attachment object from this collection
		/// </summary>
		/// <param name="data"></param>
		public void Remove(Attachments data) {
			List.Remove(data);
		}

		/// <summary>
		/// Converts the attachment to an xml string
		/// </summary>
		/// <returns></returns>
		public string ToXml() {
			StringWriter sw = new StringWriter();
			XmlTextWriter xtw = new XmlTextWriter(sw);
			xtw.WriteStartElement("Attachments");

			foreach (Attachment data in List) {
				xtw.WriteRaw(data.ToXml());
			}

			xtw.WriteEndElement();
			return sw.ToString();
		}

	}

	/// <summary>
	/// A file to attach to an email
	/// </summary>
	public class Attachment {

        /// <summary>
		/// Creates an attachment pulling in the given file
		/// </summary>
		/// <param name="pathToFile"></param>
		public Attachment(string pathToFile) {
			_fullName = pathToFile;
			_name = new FileInfo(pathToFile).Name;
		}

		private string _fullName;
		/// <summary>
		/// Gets or sets the FullName of the Attachment (full path to the file to attach)
		/// </summary>
		public string FullName {
			get { return _fullName; }
			set { _fullName = value; }
		}

		private string _name;
		/// <summary>
		/// Gets or sets the Name of the Attachment (only name of file to attach, not full path)
		/// </summary>
		public string Name {
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// Gets the bytes from the file pointed to by FullName
		/// </summary>
		/// <returns></returns>
		public byte[] GetBytes() {
            byte[] data;
            using (FileStream fs = new FileStream(_fullName, FileMode.Open, FileAccess.Read)) {
                data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);
            }
			return data;
		}

		/// <summary>
		/// Returns the attachment as a Base64 string suitable for attaching to an email
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			byte[] bytes = this.GetBytes();
			return Convert.ToBase64String(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// Returns an xml representation of an Attachment
		/// </summary>
		/// <returns></returns>
		public string ToXml() {
			StringWriter sw = new StringWriter();
			XmlTextWriter xtw = new XmlTextWriter(sw);
			xtw.WriteStartElement("Attachment");
			xtw.WriteAttributeString("Name", _fullName);
			byte[] data = this.GetBytes();
			xtw.WriteBase64(data, 0, data.Length);
			xtw.WriteEndElement();
			return sw.ToString();
		}
	}
}
