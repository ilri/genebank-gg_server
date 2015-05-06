using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Admin {
    internal class LangInfo {
        public int ID;
        public string Name;
        public string Text;
        public string TooltipText;
        public string DatabaseName;

        public LangInfo Clone() {
            return new LangInfo { ID = this.ID, Name = this.Name, Text = this.Text, TooltipText = this.TooltipText, DatabaseName = this.DatabaseName };
        }

        public static List<LangInfo> Clone(List<LangInfo> langs) {
            var li = new List<LangInfo>();
            foreach (var lang in langs) {
                li.Add(lang.Clone());
            }
            return li;
        }
    }

    internal class ComponentInfo {
        public string Name;
        public List<LangInfo> Languages;
        public int Offset;
    }

    internal class FormInfo {
        public string AppName;
        public string FormName;
        public string FormText;
        public List<ComponentInfo> Components;
        public FormInfo() {
            Components = new List<ComponentInfo>();
        }

        public string ToString(int offset) {
            if (Components.Count < 1) {
                return null;
            } else {


                // file format:
                // ---------------------------
                // appname
                // form name
                // resource name
                // offset (for combobox entries)
                // language text
                // language description


                // output the name of the form itself
                var output = new StringBuilder(
                    AppName + "\t"       // app name
                    + FormName + "\t"    // form name
                    + FormName + "\t"    // resource name
                    + "0\t"              // sort order
                    + "".PadLeft(offset * 2, '\t')
                    + FormText + "\t"    // english text
                    + "\r\n");               // english description

                foreach (ComponentInfo ci in Components) {
                    output.Append(AppName)
                        .Append("\t")
                        .Append(FormName)
                        .Append("\t")
                        .Append(ci.Name)
                        .Append("\t")
                        .Append(ci.Offset);
                    foreach (LangInfo li in ci.Languages) {

                        if (li.Name == "English") {
                            output.Append("\t")
                                .Append((li.Text + string.Empty).Replace("\r", "\\r").Replace("\n", "\\n"));
                            output.Append("\t")
                                .Append((li.TooltipText + string.Empty).Replace("\r", "\\r").Replace("\n", "\\n"));

                        } else {
                            output.Append("\t\t");
                        }
                    }
                    output.AppendLine();
                }

                return output.ToString();
            }
        }

    }
}
