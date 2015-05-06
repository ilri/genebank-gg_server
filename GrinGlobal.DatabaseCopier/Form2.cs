using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrinGlobal.DatabaseCopier {
	public partial class Form2 : Form {
		public Form2() {
			InitializeComponent();

			TreeNode tn = treeView1.Nodes.Add("Root");

			addRandomNodes(tn, 10);
			foreach (TreeNode tn1 in tn.Nodes) {
				addRandomNodes(tn1, 10);
			}
		}

		Random rnd = new Random();

		private void addRandomNodes(TreeNode parent, int count) {
			for (int i=0; i < count; i++) {
				parent.Nodes.Add("Node " + rnd.Next(1, count));
			}
		}


		private void button1_Click(object sender, EventArgs e) {
			TreeNode root = treeView1.Nodes[0];

		}

		private bool CheckNode(TreeNode tn) {
			return tn.Text == "Hi";
		}

		private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {

			MessageBox.Show("node exists? " + treeView1.IsDuplicateNode(e.Node));


			//e.Node.Siblings().Where<TreeNode>(CheckNode);

			//e.Node.Siblings().First(delegate(TreeNode tn) {
			//    if (tn.Text == e.Node.Text) {
			//        MessageBox.Show("found it");
			//        return true;
			//    }
			//    return false;
			//});



			//foreach (TreeNode tn in e.Node.Siblings()){
			//    if (tn.Text == e.Node.Text) {
			//        // found one at the same depth with
			//        MessageBox.Show("Found node w/ same text as clicked node! Index = " + tn.Index);
			//    }
			//}
		}
	}
}
