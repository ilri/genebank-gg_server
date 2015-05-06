using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.Dataviews;
using System.Diagnostics;
using GrinGlobal.Core;

namespace GrinGlobal.Business.SqlParser {
    /// <summary>
    /// Represents a join between two tables in a SQL query.  Should be able to handle any valid SQL in the FROM clause, including the case of only a single table (i.e. no 'other' table to JOIN to)
    /// </summary>
    public class Join {

        private ITable _fromTable;
        /// <summary>
        /// Gets the ITable on the left-hand side of the join clause.  Use AddTable method to alter this property.
        /// </summary>
        public ITable FromTable { get { return _fromTable; } }

        private ITable _toTable;
        /// <summary>
        /// Gets the ITable on the right-hand side of the join clause.  Use AddTable method to alter this property.  This will be null for a query with only single table in the FROM clause
        /// </summary>
        public ITable ToTable { get { return _toTable; } }
        /// <summary>
        /// Gets or sets the type of JOIN this object represents: Left, Inner, Outer, etc.  "Uknown" means it is an old-style join which has no ON clause and that comparison is done within the WHERE clause
        /// </summary>
        public JoinType JoinType { get; set; }
        private List<Conditional> _conditionals;
        /// <summary>
        /// List of Conditional objects within an ON clause.  Typical case will be exactly one conditional, such as: "sys_dataview.sys_dataview_id = sys_dataview_sql.sys_dataview_id".  However it can also handle multiple conditionals such as: "sys_dataview.sys_dataview_id = sys_dataview_lang.sys_dataview_id and sys_dataview_lang.sys_lang_id = 1"
        /// </summary>
        public List<Conditional> Conditionals { get { return _conditionals; } }
        /// <summary>
        /// Gets or sets if this represents the first join in a query.  Required to be set properly so ToString() method works correctly
        /// </summary>
        public bool IsFirstInClause;

//        public string RelationshipType { get; set; }

        /// <summary>
        /// Gets the last conditional object in the Conditionals collection.  Returns null if none exist.
        /// </summary>
        public Conditional LastConditional {
            get {
                if (_conditionals.Count == 0) {
                    return null;
                } else {
                    return _conditionals[_conditionals.Count - 1];
                }
            }
        }

        public Join() {
            _conditionals = new List<Conditional>();
        }

        public bool Combine(Join otherJoin) {
            if (_toTable == null) {
                JoinType = otherJoin.JoinType;
                _toTable = otherJoin._toTable;
                _conditionals = otherJoin._conditionals;
                return true;
            } else {
                return false;
            }
        }

        public bool IsFull {
            get {
                return ToTable != null;
            }
        }

        /// <summary>
        /// Sets either the FromTable or ToTable property, as well as the JoinType property.  May alter the existing FromTable property as needed to set relationship direction appropriately.
        /// </summary>
        /// <param name="t"></param>
        public static void AddTable(ITable t, List<Join> joins, bool errorOnMissingRelationship, bool errorOnMultipleRelationships, bool parsing) {

            //var found = false;

            if (joins == null || joins.Count == 0) {
                // very first one
                var j = new Join { _fromTable = t };
                j.IsFirstInClause = true;
                joins.Add(j);
            } else {
                var j = joins[joins.Count-1];
                if (parsing) {
                    if (j.IsFull) {
                        // last one is already full.
                        // add on a new one.
                        j = new Join();
                        joins.Add(j);
                    }
                    j._toTable = t;
                } else {
                    addJoin(t, joins, errorOnMissingRelationship, errorOnMultipleRelationships);
                }
            }
        }

        private static bool addJoin(ITable t, List<Join> joins, bool errorOnMissingRelationship, bool errorOnMultipleRelationships) {
            var possibles = determinePossibleTableRelationships(t, joins);
            if (possibles.Count == 0) {
                if (errorOnMissingRelationship) {
                    throw new MissingJoinException(getDisplayMember("addJoin{missingrelationship}", "Could not find a valid relationship between {0} and existing joins.", t.TableName), t, joins);
                } else {
                    //Debug.WriteLine("unknown join...");
                    // don't know how to join this, must be at parse time (i.e. will be taken care of later)
                    return false;
                }
            } else if (possibles.Count > 1) {
                if (errorOnMultipleRelationships) {
                    throw new MultiJoinException(getDisplayMember("addJoin{multiplerelationships}", "Multiple joins exist among {0} and existing joins.", t.TableName), t, possibles);
                } else {
                    //Debug.WriteLine("unknown join...");
                    // don't know how to join this, must be at parse time (i.e. will be taken care of later)
                    return false;
                }
            } else {
                // add on the join.
                joins.Add(possibles[0]);
                return true;
            }
        }

        private static void addLanguageConditionalIfNeeded(Join j) {
            if (j.ToTable.TableName.ToLower().EndsWith("_lang")) {
                // add the additional conditional :)
                if (j.LastConditional != null && j.LastConditional.Operator == null) {
                    j.LastConditional.Operator = "and";
                }
                j.Conditionals.Add(new Conditional { FieldA = j.ToTable.GetField("sys_lang_id"), FieldB = new Field { TableFieldName = "__LANGUAGEID__" }, Comparator = "=" });
            }
        }

        private static List<Join> determinePossibleTableRelationships(ITable tableToAdd, List<Join> existingJoins) {
            var possibleJoins = new List<Join>();
            if (tableToAdd != null) {
                // make a copy of the list, reverse it
                var joins = existingJoins.ToList();
                joins.Reverse();
                var existingTables = new List<ITable>();
                foreach (var j1 in joins) {
                    if (j1.FromTable != null) {
                        existingTables.Add(j1.FromTable);
                    }
                    if (j1.ToTable != null) {
                        existingTables.Add(j1.ToTable);
                    }
                }
                foreach (Join ej in joins) {
                    var compareToTables = new List<ITable>();
                    if (ej.ToTable != null) {
                        compareToTables.Add(ej.ToTable);
                    }
                    if (ej.FromTable != null) {
                        compareToTables.Add(ej.FromTable);
                    }

                    foreach(var compareTable in compareToTables){
                        if (tableToAdd == compareTable) {
                            // self-join... what to do?
                            break;
                        } else {

                            // first try assuming the table we're adding is a child of the 'current' table
                            associateParentAndChild(compareTable, tableToAdd, possibleJoins, existingTables);

                            // then try assuming the table we're adding is a parent of the 'current' table
                            associateParentAndChild(tableToAdd, compareTable, possibleJoins, existingTables);

                            // then try assuming they're not directly related, but share a common foreign key
                            associateChildAndChild(tableToAdd, compareTable, possibleJoins, existingTables);

                            // then try assuming one of them is a code_value table (special case since no FK is defined...)
                            associateCodedChild(tableToAdd, compareTable, possibleJoins, existingTables);
                            associateCodedChild(compareTable, tableToAdd, possibleJoins, existingTables);


                        }
                    }
                }
            }


            // we get here, no relationship could be found!
            return possibleJoins;
        }

        private static void associateParentAndChild(ITable parent, ITable child, List<Join> possibleJoins, List<ITable> existingTables) {
            // check based on B->A (they added a table which is a child of the immediate previous one, most common case)
            var pkName = parent.PrimaryKeyFieldName;
            foreach (var pfk in child.ForeignKeys) {
                //if (String.Compare(pfk.TableFieldName, pkName, true) == 0){
                if (String.Compare(pfk.ForeignKeyTableName, parent.TableName, true) == 0 && String.Compare(pfk.ForeignKeyTableFieldName, pkName, true) == 0) {

                    // since field names match exactly, we'll assume that they represent the same data
                    // and can therefore be joined upon

                    var pkField = pfk;
                    var fkField = parent.GetField(pkName);
                    if (fkField == null) {
                        fkField = parent.GetField(pfk.ForeignKeyTableFieldName);
                    }

                    if (!AlreadyReferencingTable(pkField, parent)) {

                        var j = new Join { JoinType = JoinType.Left };
                        if (existingTables.Contains(parent)) {
                            j._fromTable = parent;
                            j._toTable = child;
                        } else {
                            j._fromTable = child;
                            j._toTable = parent;
                        }

                        j.Conditionals.Add(new Conditional { FieldA = pkField, FieldB = fkField, Comparator = "=", Operator = null });
                        AddReferencingFieldProperty(parent, pkField);

                        addLanguageConditionalIfNeeded(j);
                        possibleJoins.Add(j);

                    }
                }
            }
        }

        private static void associateChildAndChild(ITable child1, ITable child2, List<Join> possibleJoins, List<ITable> existingTables) {
            var ppk = child2.GetField(child2.PrimaryKeyFieldName);
            var cpk = child1.GetField(child1.PrimaryKeyFieldName);

            foreach (var pfk in child2.ForeignKeys) {
                foreach (var cfk in child1.ForeignKeys) {
                    if (String.Compare(cfk.TableFieldName, pfk.TableFieldName, true) == 0
                        || cfk.TableFieldName.ToLower().EndsWith(pfk.TableFieldName.ToLower())
                        || pfk.TableFieldName.ToLower().EndsWith(cfk.TableFieldName.ToLower())
                        ) {

                        if (!cfk.IsAudit) {

                            // since field names match exactly or one field completely ends with another one, we'll assume that they represent the same data
                            // and can therefore be joined upon

                            var fieldA = child1.GetField(cfk.TableFieldName);
                            var fieldB = child2.GetField(pfk.TableFieldName);
                            if (!AlreadyReferencingTable(fieldA, child2) && !AlreadyReferencingTable(fieldB, child1)) {

                                var j = new Join { JoinType = JoinType.Left };
                                if (existingTables.Contains(child1)) {
                                    j._fromTable = child1;
                                    j._toTable = child2;
                                } else {
                                    j._fromTable = child2;
                                    j._toTable = child1;
                                }

                                j.Conditionals.Add(new Conditional { FieldA = fieldA, FieldB = fieldB, Comparator = "=", Operator = null });
                                AddReferencingFieldProperty(child2, fieldA);
                                AddReferencingFieldProperty(child1, fieldB);

                                addLanguageConditionalIfNeeded(j);
                                possibleJoins.Add(j);
                            }
                        }
                    //} else if (cfk.TableFieldName.ToLower().EndsWith(ppk.TableFieldName.ToLower())) {
                    //    if (!cfk.IsAudit) {

                    //        // since field names match exactly or one field completely ends with another one, we'll assume that they represent the same data
                    //        // and can therefore be joined upon

                    //        var fieldA = child1.GetField(cfk.TableFieldName);
                    //        var fieldB = child2.GetField(ppk.TableFieldName);

                    //        if (!alreadyReferencingTable(fieldA) && !alreadyReferencingTable(fieldB)) {

                    //            var j = new Join { _fromTable = child2, _toTable = child1, JoinType = JoinType.Left };

                    //            j.Conditionals.Add(new Conditional { FieldA = fieldA, FieldB = fieldB, Comparator = "=", Operator = null });
                    //            addReferencingFieldProperty(child2, fieldA);

                    //            addLanguageConditionalIfNeeded(j);
                    //            possibleJoins.Add(j);
                    //        }
                    //    }
                    } else if (pfk.TableFieldName.ToLower().EndsWith(cpk.TableFieldName.ToLower())) {
                        if (!pfk.IsAudit) {

                            // since field names match exactly or one field completely ends with another one, we'll assume that they represent the same data
                            // and can therefore be joined upon

                            var fieldA = child1.GetField(cpk.TableFieldName);
                            var fieldB = child2.GetField(pfk.TableFieldName);

                            if (!AlreadyReferencingTable(fieldA, child2) && !AlreadyReferencingTable(fieldB, child1)) {

                                var j = new Join { JoinType = JoinType.Left };
                                if (existingTables.Contains(child2)) {
                                    j._fromTable = child2;
                                    j._toTable = child1;
                                } else {
                                    j._fromTable = child1;
                                    j._toTable = child2;
                                }

                                j.Conditionals.Add(new Conditional { FieldA = fieldA, FieldB = fieldB, Comparator = "=", Operator = null });
                                AddReferencingFieldProperty(child2, fieldA);
                                AddReferencingFieldProperty(child1, fieldB);

                                addLanguageConditionalIfNeeded(j);
                                possibleJoins.Add(j);
                            }
                        }
                    }
                }
            }
        }

        private static void associateCodedChild(ITable codeTable, ITable table2, List<Join> possibleJoins, List<ITable> existingTables) {
            // special-case code for code_value table ... it doesn't have a foreign key defined for coded values!
            if (codeTable.TableName.ToLower() == "code_value") {
                foreach (var bf in table2.Mappings) {
                    if (bf.TableFieldName.ToLower().EndsWith("_code")) {
                        // assume it's a coded value we're joining by...

                        var fieldA = codeTable.GetField("value");

                        if (!AlreadyReferencingTable(bf, codeTable)) {

                            var j = new Join { JoinType = JoinType.Left };
                            if (existingTables.Contains(codeTable)) {
                                j._fromTable = null;
                                j._toTable = table2;
                            } else {
                                j._fromTable = codeTable;
                                j._toTable = table2;
                            }

                            j.Conditionals.Add(new Conditional { FieldA = fieldA, FieldB = bf, Comparator = "=", Operator = null });
                            AddReferencingFieldProperty(codeTable, bf);

                            addLanguageConditionalIfNeeded(j);
                            possibleJoins.Add(j);
                        }
                    }
                }
            }
        }

        internal static bool AlreadyReferencingTable(IField field, ITable table) {
            // debugging, seeing if we need to check this condition anymore to remain valid...
            //return false;
            var rv = field.ExtendedProperties.ContainsKey("join_parent") && ((field.ExtendedProperties["join_parent"] as string) == (String.IsNullOrEmpty(table.AliasName) ? table.TableName : table.AliasName));
            return rv;
        }

        internal static void AddReferencingFieldProperty(ITable parentTable, IField foreignKeyField) {
            if (parentTable != null && foreignKeyField != null && foreignKeyField.Table != null) {
                var alias = (String.IsNullOrEmpty(foreignKeyField.Table.AliasName) ? foreignKeyField.Table.TableName : foreignKeyField.Table.AliasName) + "." + foreignKeyField.TableFieldName;
                object existingAlias = null;
                if (parentTable.ExtendedProperties.TryGetValue("join_children", out existingAlias)) {
                    alias = existingAlias + "," + alias;
                }
                parentTable.ExtendedProperties["join_children"] = alias;

                foreignKeyField.ExtendedProperties["join_parent"] = parentTable.AliasName;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }

            var tgt = (Join)obj;

            if (IsFirstInClause != tgt.IsFirstInClause) {
                return false;
            }

            if (FromTable == null || ToTable == null || tgt.FromTable == null || tgt.FromTable == null) {
                return false;
            }

            if (String.Compare(FromTable.TableName, tgt.FromTable.TableName, true) != 0
                || JoinType != tgt.JoinType
                || _conditionals.Count == 0 || tgt.Conditionals.Count == 0
                || _conditionals.Count != tgt.Conditionals.Count) {
                return false;
            }

            for (var i = 0; i < _conditionals.Count; i++) {
                if (_conditionals[i] != tgt.Conditionals[i]) {
                    return false;
                }
            }


            // we get here, everything matches.
            return true;
        }

        public override string ToString() {

            // kicks out a query-friendly string represented by this JOIN object

            var sb = new StringBuilder();
            if (JoinType == JoinType.None || JoinType == JoinType.Unknown) {
                // table1, table2 ...
                if (FromTable != null) {
                    if (IsFirstInClause) {
                        sb.Append("    ").Append(FromTable.TableName).Append(" ").Append(FromTable.AliasName);
                    } else {
                        sb.Append(",\r\n    ").Append(FromTable.TableName).Append(" ").Append(FromTable.AliasName);
                    }
                }
                if (ToTable != null) {
                    sb.Append(",\r\n    ").Append(ToTable.TableName).Append(" ").Append(ToTable.AliasName);
                } else {
                    sb.Append("\r\n");
                }
            } else {
                // table1 inner join table2 ...
                if (IsFirstInClause) {
                    sb.Append("    ").Append(FromTable.TableName).Append(" ").Append(FromTable.AliasName);
                    object text = null;
                    if (FromTable.ExtendedProperties.TryGetValue("text", out text)) {
                        sb.Append(text as string);
                    }

                }

                if (ToTable != null) {

                    if (IsFirstInClause) {
                        sb.Append("\r\n");
                    }
                    sb.Append("    ").Append(JoinType.ToString().ToUpper()).Append(" JOIN ")
                        .Append(ToTable.TableName).Append(" ").Append(ToTable.AliasName);

                    object text = null;
                    if (ToTable.ExtendedProperties.TryGetValue("text", out text)) {
                        sb.Append(text as string);
                    }


                    sb.Append("\r\n      ON ");

                    foreach (var cond in _conditionals) {
                        sb.Append(cond.ToString());
                    }
                }

            }

            var rv = sb.ToString();
            return rv;

//            return FromTable.TableName + "." + FromField.TableFieldName + " " + JoinType + " join " + ToField.TableName + "." + ToField.TableFieldName;
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "MiddleTier", "Join", resourceName, null, defaultValue, substitutes);
        }

    }
}
