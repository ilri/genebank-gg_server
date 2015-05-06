using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core.Xml;
using GrinGlobal.Core;
using System.Xml.XPath;
using System.Data;
using System.Xml;

namespace GrinGlobal.Search.Engine {
	public class Field {

		internal Field() {
            SubFields = new List<Field>();
            Ordinal = -1;
		}

        //internal static Field FromXPathNavigator(XPathNavigator nav) {
        //    Field ret = new Field();
        //    ret.Name = nav.GetAttribute("Name", "");
        //    ret.IsPrimaryKey = Toolkit.ToBoolean(nav.GetAttribute("IsPrimaryKey", ""), false);
        //    ret.StoredInIndex = Toolkit.ToBoolean(nav.GetAttribute("StoredInIndex", ""), false);
        //    ret.Searchable = Toolkit.ToBoolean(nav.GetAttribute("Searchable", ""), false);
        //    ret.Format = nav.GetAttribute("Format", "");
        //    ret.Ordinal = Toolkit.ToInt32(nav.GetAttribute("Ordinal", ""), 0);
        //    ret.DataType = Type.GetType(nav.GetAttribute("Type", ""));
        //    ret.IsBoolean = Toolkit.ToBoolean(nav.GetAttribute("IsBoolean", ""), false);
        //    ret.TrueValue = nav.GetAttribute("TrueValue", "");
        //    ret.IsCreatedDate = Toolkit.ToBoolean(nav.GetAttribute("IsCreatedDate", ""), false);
        //    ret.IsModifiedDate = Toolkit.ToBoolean(nav.GetAttribute("IsModifiedDate", ""), false);

        //    ret.ForeignKeyTable = nav.GetAttribute("ForeignKeyTable", "");
        //    ret.ForeignKeyField = nav.GetAttribute("ForeignKeyField", "");

        //    ret.Calculation = nav.GetAttribute("Calculation", "") + "";

        //    XPathNodeIterator it = nav.SelectChildren("Field", "");
        //    while(it.MoveNext()){
        //        ret.SubFields.Add(Field.FromXPathNavigator(it.Current));
        //    }

        //    return ret;
        //}


        internal static Field FromXmlNode(XmlNode nd, Field dbField) {
            //if (dbField == null) {
            //    return FromXPathNavigator(nav);
            //}

            Field ret = new Field();
            ret.Name = Toolkit.GetAttValue(nd, "Name", "");
            if (String.IsNullOrEmpty(ret.Name)) {
                ret.Name = dbField.Name;
            }
            ret.IsPrimaryKey = Toolkit.ToBoolean(Toolkit.GetAttValue(nd, "IsPrimaryKey", ""), dbField.IsPrimaryKey);
            ret.StoredInIndex = Toolkit.ToBoolean(Toolkit.GetAttValue(nd, "StoredInIndex", ""), dbField.StoredInIndex);
            ret.Searchable = Toolkit.ToBoolean(Toolkit.GetAttValue(nd, "Searchable", ""), dbField.Searchable);
            ret.Format = Toolkit.Coalesce(Toolkit.GetAttValue(nd, "Format", ""), dbField.Format) as string;

            // Ordinal is the only thing that we completely ignore from the global config file and dbField always wins out on.
            ret.Ordinal = dbField.Ordinal;

            string typeName = Toolkit.GetAttValue(nd, "Type", "");
            if (String.IsNullOrEmpty(typeName)) {
                ret.DataType = dbField.DataType;
            } else {
                ret.DataType = Type.GetType(typeName);
            }
            ret.IsBoolean = Toolkit.ToBoolean(Toolkit.GetAttValue(nd, "IsBoolean", ""), dbField.IsBoolean);
            ret.TrueValue = Toolkit.GetAttValue(nd, "TrueValue", "");
            if (String.IsNullOrEmpty(ret.TrueValue)) {
                ret.TrueValue = dbField.TrueValue;
            }
            //ret.IsCreatedDate = Toolkit.ToBoolean(nav.GetAttribute("IsCreatedDate", ""), dbField.IsCreatedDate);
            //ret.IsModifiedDate = Toolkit.ToBoolean(nav.GetAttribute("IsModifiedDate", ""), dbField.IsModifiedDate);

            ret.ForeignKeyTable = Toolkit.GetAttValue(nd, "ForeignKeyTable", "");
            ret.ForeignKeyField = Toolkit.GetAttValue(nd, "ForeignKeyField", "");

            ret.Calculation = Toolkit.GetAttValue(nd, "Calculation", "");

            var children = nd.SelectNodes("Field");
            foreach(XmlNode child in children){
                ret.SubFields.Add(Field.FromXmlNode(child, dbField));
            }

            return ret;
        }


        internal static Field FromXPathNavigator(XPathNavigator nav, Field dbField) {
            //if (dbField == null) {
            //    return FromXPathNavigator(nav);
            //}

            Field ret = new Field();
            ret.Name = nav.GetAttribute("Name", "");
            if (String.IsNullOrEmpty(ret.Name)) {
                ret.Name = dbField.Name;
            }
            ret.IsPrimaryKey = Toolkit.ToBoolean(nav.GetAttribute("IsPrimaryKey", ""), dbField.IsPrimaryKey);
            ret.StoredInIndex = Toolkit.ToBoolean(nav.GetAttribute("StoredInIndex", ""), dbField.StoredInIndex);
            ret.Searchable = Toolkit.ToBoolean(nav.GetAttribute("Searchable", ""), dbField.Searchable);
            ret.Format = Toolkit.Coalesce(nav.GetAttribute("Format", ""), dbField.Format) as string;

            // Ordinal is the only thing that we completely ignore from the global config file and dbField always wins out on.
            ret.Ordinal = dbField.Ordinal;

            string typeName = nav.GetAttribute("Type", "");
            if (String.IsNullOrEmpty(typeName)) {
                ret.DataType = dbField.DataType;
            } else {
                ret.DataType = Type.GetType(typeName);
            }
            ret.IsBoolean = Toolkit.ToBoolean(nav.GetAttribute("IsBoolean", ""), dbField.IsBoolean);
            ret.TrueValue = nav.GetAttribute("TrueValue", "");
            if (String.IsNullOrEmpty(ret.TrueValue)) {
                ret.TrueValue = dbField.TrueValue;
            }
            ret.IsCreatedDate = Toolkit.ToBoolean(nav.GetAttribute("IsCreatedDate", ""), dbField.IsCreatedDate);
            ret.IsModifiedDate = Toolkit.ToBoolean(nav.GetAttribute("IsModifiedDate", ""), dbField.IsModifiedDate);

            ret.ForeignKeyTable = nav.GetAttribute("ForeignKeyTable", "");
            ret.ForeignKeyField = nav.GetAttribute("ForeignKeyField", "");

            ret.Calculation = nav.GetAttribute("Calculation", "");

            XPathNodeIterator it = nav.SelectChildren("Field", "");
            while (it.MoveNext()) {
                ret.SubFields.Add(Field.FromXPathNavigator(it.Current, dbField));
            }

            return ret;
        }


        internal static Field LoadFromSettings(Dictionary<string, string> dic, string categoryName, string fieldName) {
            var f = new Field { Name = fieldName };

            // - fields are stored in settings like this (number is the ordinal of the field)
            // "Index-" + index name + "-Fields--" + field ordinal = field name
            // Index-accession-Fields--0 = "accession-prefix"
            // Index-accession-Fields--1 = "accession-number"
            // Index-accession-Fields--2 = "accession-suffix"
            // ...

            // - per-field settings are stored like this (e.g. accession-prefix field settings):
            // "Index-" + index name + "-Fields-" + field name + "--" + property name = property value
            // Index-accession-Fields-accesion-prefix--ordinal = 0
            // Index-accession-Fields-accesion-prefix--is-boolean = N
            // Index-accession-Fields-accesion-prefix--calculation = null
            // Index-accession-Fields-accesion-prefix--type = String
            // Index-accession-Fields-accesion-prefix--foreign-key-field = null
            // ...

            var props = Lib.GetAllSettingsByCategory(dic, categoryName + "-Fields-" + fieldName.ToLower());

            f.Calculation = props["Calculation"];
            f.DataType = Type.GetType(props["Type"]);
            f.ForeignKeyField = props["ForeignKeyField"];
            f.ForeignKeyTable = props["ForeignKeyTable"];
            f.Format = props["Format"];
            f.IsBoolean = Toolkit.ToBoolean(props["IsBoolean"], false);
            //f.IsCalculated = Toolkit.ToBoolean(props["IsCalculated"], false);
            f.IsCreatedDate = Toolkit.ToBoolean(props["IsCreatedDate"], false);
            f.IsModifiedDate = Toolkit.ToBoolean(props["IsModifiedDate"], false);
            f.IsPrimaryKey = Toolkit.ToBoolean(props["IsPrimaryKey"], false);
            f.Ordinal = Toolkit.ToInt32(props["Ordinal"], 0);
            f.Searchable = Toolkit.ToBoolean(props["Searchable"], false);
            f.StoredInIndex = Toolkit.ToBoolean(props["StoredInIndex"], false);
            f.TrueValue = props["TrueValue"];
            //f.Value = props["Value"];

            return f;

        }

        internal Node ToXmlNode() {
            Node nd = new Node("Field");
            nd.Attributes.SetValue("Ordinal", Ordinal.ToString());
            nd.Attributes.SetValue("Name", Name);
            if (Value != null) {
                nd.Attributes.SetValue("Value", Value.ToString());
            }
            nd.Attributes.SetValue("IsPrimaryKey", IsPrimaryKey.ToString());
            nd.Attributes.SetValue("StoredInIndex", StoredInIndex.ToString());
            nd.Attributes.SetValue("Searchable", Searchable.ToString());
            nd.Attributes.SetValue("Format", Format);
            if (DataType != null) {
                nd.Attributes.SetValue("Type", DataType.ToString());
            }

            nd.Attributes.SetValue("IsBoolean", IsBoolean.ToString());
            nd.Attributes.SetValue("TrueValue", TrueValue);

            nd.Attributes.SetValue("IsCreatedDate", IsCreatedDate.ToString());
            nd.Attributes.SetValue("IsModifedDate", IsModifiedDate.ToString());

            nd.Attributes.SetValue("ForeignKeyTable", ForeignKeyTable);
            nd.Attributes.SetValue("ForeignKeyField", ForeignKeyField);


            nd.Attributes.SetValue("Calculation", Calculation);
            foreach (Field sf in this.SubFields) {
                nd.Nodes.Add(sf.ToXmlNode());
            }

            return nd;
        }

        internal void ToSettings(string indexName, Dictionary<string, string> settings, bool isSubField) {

            var category = "Index-" + indexName.ToLower() + "-Fields-" + this.Name.ToLower();

            Lib.AddSettingAsNeeded(settings, category, "IsSubField", isSubField.ToString(), true);

            Lib.AddSettingAsNeeded(settings, category, "Name", this.Name, true);

            Lib.AddSettingAsNeeded(settings, category, "Ordinal", Ordinal.ToString(), true);


            Lib.AddSettingAsNeeded(settings, category, "Value", ("" + Value).ToString(), true);

            Lib.AddSettingAsNeeded(settings, category, "IsPrimaryKey", IsPrimaryKey.ToString(), true);

            Lib.AddSettingAsNeeded(settings, category, "StoredInIndex", StoredInIndex.ToString(), true);

            Lib.AddSettingAsNeeded(settings, category, "Searchable", Searchable.ToString(), true);

            Lib.AddSettingAsNeeded(settings, category, "Format", Format.ToString(), true);

            Lib.AddSettingAsNeeded(settings, category, "Type", DataType.ToString(), true);

            Lib.AddSettingAsNeeded(settings, category, "IsBoolean", IsBoolean.ToString(), true);

            Lib.AddSettingAsNeeded(settings, category, "TrueValue", TrueValue.ToString(), true);

            Lib.AddSettingAsNeeded(settings, category, "IsCreatedDate", IsCreatedDate.ToString(), true);

            Lib.AddSettingAsNeeded(settings, category, "IsModifiedDate", IsModifiedDate.ToString(), true);

            Lib.AddSettingAsNeeded(settings, category, "ForeignKeyTable", ForeignKeyTable.ToString(), true);

            Lib.AddSettingAsNeeded(settings, category, "ForeignKeyField", ForeignKeyField.ToString(), true);


            Lib.AddSettingAsNeeded(settings, category, "Calculation", Calculation.ToString(), true);
            foreach (Field sf in this.SubFields) {
                sf.ToSettings(indexName, settings, true);
            }

        }

        internal static Field FillObjectFromDataSet(DataRow dr) {
            var f = new Field {
                Name = dr["field_name"].ToString(),
                Ordinal = Toolkit.ToInt32(dr["ordinal"], -1),
                // Value = dr["value"],
                IsPrimaryKey = Toolkit.ToBoolean(dr["is_primary_key"], false),
                StoredInIndex = Toolkit.ToBoolean(dr["is_stored_in_index"], false),
                Searchable = Toolkit.ToBoolean(dr["is_searchable"], false),
                Format = dr["format"].ToString(),
                DataType = Type.GetType(dr["type"].ToString()),
                IsBoolean = Toolkit.ToBoolean(dr["is_boolean"], false),
                TrueValue = dr["true_value"].ToString(),
                ForeignKeyTable = dr["foreign_key_table"].ToString(),
                ForeignKeyField = dr["foreign_key_field"].ToString(),
                Calculation = dr["calculation"].ToString()
            };

            return f;
                
        }

        internal void FillDataSet(DataSet ds, string indexName, bool isSubField) {

            var dtField = ds.Tables["index_field"];

            var drFields = dtField.Select("index_name = '" + indexName + "' and field_name = '" + Name + "'");
            var drField = dtField.NewRow();
            if (drFields.Length == 0) {
                dtField.Rows.Add(drField);
            } else {
                drField = drFields[0];
            }


            drField["index_name"] = indexName;
            drField["field_name"] = Name.ToString();
            drField["ordinal"] = Ordinal.ToString();
            //drField["value"] = ("" + Value).ToString();
            drField["is_primary_key"] = IsPrimaryKey.ToString();
            drField["is_stored_in_index"] = StoredInIndex.ToString();
            drField["is_searchable"] = Searchable.ToString();
            drField["format"] = Format.ToString();
            drField["type"] = DataType.ToString();
            drField["is_boolean"] = IsBoolean.ToString();
            drField["true_value"] = TrueValue.ToString();
            drField["foreign_key_table"] = ForeignKeyTable.ToString();
            drField["foreign_key_field"] = ForeignKeyField.ToString();
            drField["calculation"] = Calculation.ToString();

            foreach (Field sf in this.SubFields) {
                sf.FillDataSet(ds, indexName, true);
            }

        }



		internal static Field FromDefinitionFileNode(Node nd) {
			Field f = new Field {
				Name = nd.Attributes.GetValue("Name"),
                IsPrimaryKey = Toolkit.ToBoolean(nd.Attributes.GetValue("IsPrimaryKey", ""), false),
                StoredInIndex = Toolkit.ToBoolean(nd.Attributes.GetValue("StoredInIndex"), false),
				Searchable = Toolkit.ToBoolean(nd.Attributes.GetValue("Searchable"), false),
				Format = nd.Attributes.GetValue("Format", ""),
				Ordinal = Toolkit.ToInt32(nd.Attributes.GetValue("Ordinal"), 0),
				DataType = Type.GetType(nd.Attributes.GetValue("Type")),
                IsBoolean = Toolkit.ToBoolean(nd.Attributes.GetValue("IsBoolean", "false"), false),
                TrueValue = nd.Attributes.GetValue("TrueValue", ""),
                IsCreatedDate = Toolkit.ToBoolean(nd.Attributes.GetValue("IsCreatedDate", "false"), false),
                IsModifiedDate = Toolkit.ToBoolean(nd.Attributes.GetValue("IsModifiedDate", "false"), false),
                ForeignKeyTable = nd.Attributes.GetValue("ForeignKeyTable", ""),
                ForeignKeyField = nd.Attributes.GetValue("ForeignKeyField", ""),
                Calculation = nd.Attributes.GetValue("Calculation", "")
            };

            foreach(Node child in nd.Nodes){
                if (child.NodeName == "Field"){
                    f.SubFields.Add(Field.FromDefinitionFileNode(child));
                }
            }

			return f;
		}

		public string Name { get; set; }
        public bool IsPrimaryKey { get; set; }
		public bool StoredInIndex { get; set; }
		public string Format { get; set; }
		public bool Searchable { get; set; }
		public int Ordinal { get; set; }
		public Type DataType { get; set; }
        public bool IsBoolean { get; set; }
        public string TrueValue { get; set; }
        public bool IsCreatedDate { get; set; }
        public bool IsModifiedDate { get; set; }

        public string ForeignKeyTable { get; set; }
        public string ForeignKeyField { get; set; }

        public string Calculation { get; set; }
        public bool IsCalculated {
            get {
                return !String.IsNullOrEmpty(this.Calculation);
            }
        }

        public List<Field> SubFields { get; private set; }

		public object Value { get; set; }

		internal Field Clone() {
			Field f = new Field();
			f.Name = this.Name;
            f.IsPrimaryKey = this.IsPrimaryKey;
			f.Value = this.Value;
			f.Ordinal = this.Ordinal;
			f.DataType = this.DataType;
			f.StoredInIndex = this.StoredInIndex;
			f.Searchable = this.Searchable;
			f.Format = this.Format;
            f.IsBoolean = this.IsBoolean;
            f.TrueValue = this.TrueValue;
            f.IsCreatedDate = this.IsCreatedDate;
            f.IsModifiedDate = this.IsModifiedDate;
            f.ForeignKeyTable = this.ForeignKeyTable;
            f.ForeignKeyField = this.ForeignKeyField;
            f.Calculation = this.Calculation;
            foreach (Field sf in this.SubFields) {
                f.SubFields.Add(sf.Clone());
            }
			return f;
		}

		public override string ToString() {
			return "Ordinal=" + Ordinal + ", Name=" + Name + ", Value=" + Value + ", IsPrimaryKey=" + IsPrimaryKey + ", StoredInIndex=" + StoredInIndex + ", Searchable=" + Searchable + ", Format=" + Format + ", DataType=" + DataType + ", IsBoolean=" + IsBoolean + ", TrueValue=" + TrueValue + ", IsCreatedDate=" + IsCreatedDate + ", IsModifiedDate=" + IsModifiedDate + ", Calculation=" + Calculation + ", FK Table=" + ForeignKeyTable + ", FK Field=" + ForeignKeyField;
		}
	}
}
