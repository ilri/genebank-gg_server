using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;

namespace GrinGlobal.Interface {
    /// <summary>
    /// This object represents a single field of a single row in a table in a database
    /// </summary>
    [DataContract(Namespace="http://www.grin-global.org")]
    [KnownType(typeof(System.DBNull))]
    [KnownType(typeof(System.String))]
    [KnownType(typeof(System.DateTime))]
    [KnownType(typeof(System.Int32))]
    [KnownType(typeof(System.Decimal))]
    public class FieldValue {

        /// <summary>
        /// Gets or sets the name of the field
        /// </summary>
        [DataMember]
        public string FieldName;

        /// <summary>
        /// Gets or sets the original value from the database
        /// </summary>
        [DataMember]
        public object OriginalValue;

        /// <summary>
        /// Gets or sets the new value from the database
        /// </summary>
        [DataMember]
        public object NewValue;

        public override string ToString() {
            return "FieldName=" + FieldName + ", OriginalValue=" + OriginalValue + ", NewValue=" + NewValue;
        }

        public void ToXml(XmlTextWriter xw) {
            xw.WriteStartElement("field");
            xw.WriteAttributeString("name", FieldName);

            xw.WriteStartElement("originalValue");
            xw.WriteString(OriginalValue.ToString());
            xw.WriteEndElement();

            xw.WriteStartElement("newValue");
            xw.WriteString(NewValue.ToString());
            xw.WriteEndElement();

            // /field
            xw.WriteEndElement();
        }

        public static FieldValue FromXml(XmlTextReader xtr) {
            var fv = new FieldValue();
            return fv;
        }
    }
}
