using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.Dataviews;

namespace GrinGlobal.Business.SqlParser {
    public class Conditional {
        /// <summary>
        /// Gets or sets the left-hand field that is part of this conditional, dubbed "A" for convenience
        /// </summary>
        public IField FieldA;
        /// <summary>
        /// Gets or sets the right-hand field that is part of this conditional, dubbed "B" for convenience
        /// </summary>
        public IField FieldB;
        /// <summary>
        /// Gets or sets the comparator sandwiched between the "A" and "B" fields.  Examples include "like", "=", etc.  See SqlBase.SQL_COMPARATORS.
        /// </summary>
        public string Comparator;
        /// <summary>
        /// Gets or sets the operator between this conditional and the previous one.  If this is the first conditional in a sequence, this value should be null.  An operator is defined as: "and", "or", "not" etc.  See SqlBase.SQL_LOGICAL_OPERATORS.
        /// </summary>
        public string Operator;

    
    
    
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }

            var tgt = obj as Conditional;


            if (Comparator != tgt.Comparator || Operator != tgt.Operator) {
                return false;
            }

            if (FieldA != tgt.FieldA || FieldB != tgt.FieldB) {
                return false;
            }

            return true;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            //sb.Append(" ");

            sb.Append(" ");

            if (FieldA.Table != null && !String.IsNullOrEmpty(FieldA.Table.AliasName)) {
                sb.Append(FieldA.Table.AliasName).Append(".");
            } else {
                if (!String.IsNullOrEmpty(FieldA.TableName)) {
                    sb.Append(FieldA.TableName).Append(".");
                }
            }
            sb.Append(FieldA.TableFieldName).Append(" ");
            object txt = null;
            if (FieldA.ExtendedProperties.TryGetValue("text", out txt)) {
                // sb.Append(txt as string);
            }

            sb.Append(Comparator).Append(" ");

            if (FieldB.Table != null && !String.IsNullOrEmpty(FieldB.Table.AliasName)) {
                sb.Append(FieldB.Table.AliasName).Append(".");
            } else {
                if (!String.IsNullOrEmpty(FieldB.TableName)) {
                    sb.Append(FieldB.TableName).Append(".");
                }
            }
            sb.Append(FieldB.TableFieldName).Append(" ");

            txt = null;
            if (FieldB.ExtendedProperties.TryGetValue("text", out txt)) {
                // sb.Append(txt as string);
            }


            if (!String.IsNullOrEmpty(Operator)) {
                sb.Append("\r\n          ").Append(Operator).Append(" ");
            } else {
                sb.Append("\r\n");
            }


            var rv = sb.ToString();
            return rv;
        }
    }
}
