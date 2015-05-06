using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;

namespace GrinGlobal.Admin {
    internal class DragDropObject {
        public string FromCategoryName {
            get {
                var nd = SourceNode;
                while (nd.Parent != null && String.IsNullOrEmpty(nd.Name)) {
                    nd = nd.Parent;
                }
                return nd.Name;
            }
        }
        public List<int> IDList { get; private set; }
        public TreeNode SourceNode { get; private set; }
        public TreeNode RootNode {
            get {
                if (SourceNode.Parent == null) {
                    return SourceNode;
                } else {
                    var nd = SourceNode.Parent;
                    while (nd.Parent != null) {
                        nd = nd.Parent;
                    }
                    return nd;
                }
            }
        }

        public DragDropObject(ListView lv, TreeNode node) : this() {
            SourceNode = node;
            foreach (ListViewItem lvi in lv.SelectedItems) {
                IDList.Add(Toolkit.ToInt32(lvi.Tag, -1));
            }
        }

        public DragDropObject(TreeNode node)
            : this() {
            SourceNode = node;
            IDList.Add(Toolkit.ToInt32(node.Tag, -1));
        }

        public DragDropObject() {
            IDList = new List<int>();
        }

        public override string ToString() {
            return FromCategoryName + ": " + Toolkit.Join(IDList.ToArray(), ", ", "");
        }
    }
}
