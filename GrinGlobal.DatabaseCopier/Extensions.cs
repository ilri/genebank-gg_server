using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace GrinGlobal.DatabaseCopier {
	public static class Extensions {

		/// <summary>
		/// Recursively returns all descendent nodes in either breadth first or depth first fasion
		/// </summary>
		/// <param name="root"></param>
		/// <param name="depthFirst"></param>
		/// <returns></returns>
		public static IEnumerable<TreeNode> RecursiveForEach(this TreeNode root, bool depthFirst) {
			Debug.WriteLine("at node=" + root.FullPath);
			if (depthFirst) {
				foreach (TreeNode tn in root.Nodes) {
					foreach (TreeNode tnDeeper in tn.RecursiveForEach(depthFirst)) {
						yield return tnDeeper;
					}
					yield return tn;
				}
			} else {
				foreach (TreeNode tn in root.Nodes) {
					yield return tn;
					foreach (TreeNode tnWider in tn.RecursiveForEach(depthFirst)) {
						yield return tnWider;
					}
				}
			}
		}

		public static bool IsDuplicateNode(this TreeView tv, TreeNode srcNode) {
			foreach (TreeNode node in tv.Nodes[0].RecursiveForEach(false)) {
				if (node.FullPath == srcNode.FullPath && node != srcNode) {
					return true;
				}
			}
			return false;
		}

		public static IEnumerable<TreeNode> Siblings(this TreeNode node) {
			if (node.Parent == null){
				yield break;
			}

			TreeNode parent = node.Parent;
			foreach (TreeNode tn in parent.Nodes) {
				if (tn != node) {
					yield return tn;
				}
			}
		}

	}
}
