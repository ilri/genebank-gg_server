using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace GrinGlobal.Interface {

    /// <summary>
    /// This object is used to communicate updates to the search engine contents.  It represents a single row in an index.
    /// </summary>
    [DataContract(Namespace = "http://www.grin-global.org")]
    [KnownType(typeof(System.DBNull))]
    [KnownType(typeof(UpdateMode))]
    public class UpdateRow {

        /// <summary>
        /// Gets or sets the primary key for the current row.  Must always be a signed 32-bit integer
        /// </summary>
        [DataMember]
        public int ID;

        /// <summary>
        /// Gets or sets the mode of update: Add, Replace, or Subtract.
        /// </summary>
        [DataMember]
        public UpdateMode Mode;

        /// <summary>
        /// Gets or sets the value(s) of field(s) in the row.
        /// </summary>
        [DataMember]
        public List<FieldValue> Values;

        public static UpdateRow FromXml(string xml) {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            if (doc.DocumentElement.Name == "row") {
                var nd = doc.DocumentElement;
                var row = new UpdateRow();
                row.Values = new List<FieldValue>();
                row.ID = Util.ToInt32(nd.Attributes["id"].Value, -1);
                row.Mode = (UpdateMode)Enum.Parse(typeof(UpdateMode), nd.Attributes["mode"].Value, true);
                foreach (XmlNode sub in nd.ChildNodes) {
                    if (sub.NodeType == XmlNodeType.Element && sub.Name == "fields") {
                        foreach (XmlNode fld in sub.ChildNodes) {
                            if (fld.NodeType == XmlNodeType.Element && fld.Name == "field") {
                                var val = new FieldValue();
                                val.FieldName = fld.Attributes["name"].Value;
                                foreach (XmlNode values in fld.ChildNodes) {
                                    if (values.NodeType == XmlNodeType.Element) {
                                        if (values.Name == "originalValue") {
                                            val.OriginalValue = getTypedValue(values.Value);
                                        } else if (values.Name == "newValue") {
                                            val.NewValue = getTypedValue(values.Value);
                                        }
                                    }
                                }
                                row.Values.Add(val);
                            }
                        }
                    }
                }
                return row;
            } else {
                return null;
            }

        }


        public static List<UpdateRow> RowsFromXml(string xml) {
            var rows = new List<UpdateRow>();

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            foreach (XmlNode nd in doc.DocumentElement.ChildNodes) {
                if (nd.NodeType == XmlNodeType.Element) {
                    if (nd.Name == "row") {
                        var row = new UpdateRow();
                        row.Values = new List<FieldValue>();
                        row.ID = Util.ToInt32(nd.Attributes["id"].Value, -1);
                        row.Mode = (UpdateMode)Enum.Parse(typeof(UpdateMode), nd.Attributes["mode"].Value, true);
                        foreach (XmlNode sub in nd.ChildNodes) {
                            if (sub.NodeType == XmlNodeType.Element && sub.Name == "fields") {
                                foreach (XmlNode fld in sub.ChildNodes) {
                                    if (fld.NodeType == XmlNodeType.Element && fld.Name == "field") {
                                        var val = new FieldValue();
                                        val.FieldName = fld.Attributes["name"].Value;
                                        foreach (XmlNode values in fld.ChildNodes) {
                                            if (values.NodeType == XmlNodeType.Element) {
                                                if (values.Name == "originalValue") {
                                                    val.OriginalValue = getTypedValue(values.Value);
                                                } else if (values.Name == "newValue") {
                                                    val.NewValue = getTypedValue(values.Value);
                                                }
                                            }
                                        }
                                        row.Values.Add(val);
                                    }
                                }
                            }
                        }
                        rows.Add(row);
                    }
                }
            }

            return rows;
        }

        private static object getTypedValue(string value) {
            var dt = Util.ToDateTime(value, (DateTime?)null);
            if (dt != null) {
                return (DateTime)dt;
            } else {
                var b = Util.ToBoolean(value, (bool?)null);
                if (b != null) {
                    return (bool)b;
                } else {
                    var i = Util.ToInt32(value, (int?)null);
                    if (i != null) {
                        return (int)i;
                    } else {
                        var d = Util.ToDecimal(value, (decimal?)null);
                        if (d != null) {
                            return (decimal)d;
                        } else {
                            return value;
                        }
                    }
                }
            }
        }

        public static string ToXml(List<UpdateRow> rows) {
            using (var sw = new StringWriter()) {
                using (var xw = new XmlTextWriter(sw)) {
                    xw.WriteStartElement("rows");
                    if (rows != null) {
                        foreach (UpdateRow r in rows) {
                            r.ToXml(xw);
                        }
                    }
                    // /rows
                    xw.WriteEndElement();
                    return sw.ToString();
                }
            }
        }

        public string ToXml() {
            using (var sw = new StringWriter()) {
                using (var xw = new XmlTextWriter(sw)) {
                    this.ToXml(xw);
                    return sw.ToString();
                }
            }
        }
        public void ToXml(XmlTextWriter xw) {
            xw.WriteStartElement("row");

            xw.WriteAttributeString("id", ID.ToString());
            xw.WriteAttributeString("mode", Mode.ToString());

            xw.WriteStartElement("fields");
            if (Values != null) {
                foreach (FieldValue val in Values) {
                    val.ToXml(xw);
                }
            }
            // /fields
            xw.WriteEndElement();

            // /row
            xw.WriteEndElement();
        }
    }
}
