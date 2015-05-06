using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GrinGlobal.Search.Engine {
	public class BPlusTreeLeafNode<TKey, TValue>
		: BPlusTreeNode<TKey, TValue>
		where TKey : ISearchable<TKey>, new()
		where TValue : IPersistable<TValue>, new() {

//		public long LeftSiblingLocation { get; set; }
		public long RightSiblingLocation { get; set; }
		public List<TValue> Values { get; internal set; }


		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append("Root? ")
				.Append(IsRoot ? "Y" : "N")
				.Append(" Leaf? ")
				.Append(IsLeaf ? "Y" : "N")
				.Append(" Keys=")
				.Append(Keywords.Count.ToString())
				//				.Append(", ChunkSize=")
				//				.Append(_chunkSize)
				//				.Append(", CurrentSize=")
				//				.Append(CurrentSize)
				.Append(" @ ")
				.Append(Location.ToString().PadRight(5))
				.Append(" ; ")
//				.Append(LeftSiblingLocation.ToString().PadRight(5))
				.Append(" ^ ")
				.Append(Parent == null ? "  -  " : Parent.Location.ToString().PadRight(5))
				.Append(" > ")
				.Append(RightSiblingLocation.ToString().PadRight(5))
				.Append(" ; ");
			for (int i=0; i < Values.Count; i++) {
				sb.Append(Keywords[i].ToString())
					.Append("=")
					.Append(Values[i].ToString())
					.Append(", ");
			}
			if (sb.Length > 2) {
				sb.Remove(sb.Length - 2, 2);
			}
			return sb.ToString();
		}


		protected internal override void write(BinaryWriter wtr) {

			// leaves store only sibling locations and valueCount and values
			
//			wtr.Write(LeftSiblingLocation);
			wtr.Write(RightSiblingLocation);
			wtr.Write((short)Values.Count);

			foreach (TValue val in Values) {
				val.Write(wtr);
			}

		}



		protected override void movingTo(long newLocation) {

			// if this node has been written before, our left sibling has a pointer to this node on disk.
			// update that sibling to point at the new location.
			if (this.Location != 0) {
				BPlusTreeLeafNode<TKey, TValue> left = GetLeftSibling(false) as BPlusTreeLeafNode<TKey, TValue>;
				if (left != null) {
                    Tree.Lock(delegate() {
                        left.RightSiblingLocation = newLocation;
                        left.Write();
                    });
				}
			}

		}

		public BPlusTreeIndexNode<TKey, TValue> PromoteToIndexNode() {

            BPlusTreeIndexNode<TKey, TValue> indexNode = null;
            try {
                BPlusTree<TKey, TValue>.LogInfo("Begin promoting leaf node to index node: " + this.ToString());
                indexNode = new BPlusTreeIndexNode<TKey, TValue>(this.Tree, this.Parent, null);
                indexNode.Location = this.Location;
                indexNode.SlotSize = this.SlotSize;
            } finally {
                BPlusTree<TKey, TValue>.LogInfo("End promoting leaf node to index node: " + this.ToString());
            }
			return indexNode;


		}

		protected internal BPlusTreeLeafNode(BPlusTree<TKey, TValue> tree, BPlusTreeIndexNode<TKey, TValue> parent, BinaryReader rdr)
			: base(tree, parent) {

			IsLeaf = true;
//			LeftSiblingLocation = -1;
			RightSiblingLocation = -1;
			Values = new List<TValue>();

			// leaves store only sibling locations and values (keywords are handled by the base, since they're in every kind of node)
			if (rdr != null) {
//				LeftSiblingLocation = rdr.ReadInt64();
				RightSiblingLocation = rdr.ReadInt64();
				short valueCount = rdr.ReadInt16();

				for (int i=0; i < valueCount; i++) {
					TValue val = new TValue();
					val.Read(rdr);
					Values.Add(val);
				}
			}

		}

		public BPlusTreeLeafNode<TKey, TValue> GetRightSibling() {
			return GetRightSibling(false) as BPlusTreeLeafNode<TKey, TValue>;
		}

        protected internal override void performSearch(TKey keyword, KeywordMatchMode matchMode, bool ignoreCase, bool returnLeafIfNotFound, List<KeywordMatch<TKey, TValue>> matches) {

			// given a keyword, spin until the keyword is found (return null when not found)
            short lastMatchIndex = -2;
			for (short i=0; i < Keywords.Count; i++) {
				int compared = keyword.CompareTo(Keywords[i], matchMode, ignoreCase);   
				if (compared < 0) { 
					// given keyword is smaller than this entry.
					// means we're done looking in this leaf.
					// if we didn't find anything and the caller said to return a leaf even if nothing was found, append this leaf
					if (returnLeafIfNotFound && (matches.Count == 0 || matches[matches.Count-1].Node != this)) {
						// add this node as containing the hit even though it doesn't (so the value can be inserted)
						// add this keyword as matching -- so they know where at in the node to insert their new item
						matches.Add(new KeywordMatch<TKey, TValue> { Index = i, Keyword = Keywords[i], Value = default(TValue), Node = this });
					}

					return;

				} else if (compared == 0) {
					// found keyword!
					// add this node if it's not already in the list
					// we may have multiple hits, so don't return yet.
					// just add it to the list of matches.
					matches.Add(new KeywordMatch<TKey, TValue> { Index = i, Keyword = Keywords[i], Value = Values[i], Node = this });
                    lastMatchIndex = i;
				} else {
					// compared > 0
					// given keyword is larger than this entry.
					// keep checking.
				}
			}



            var inspectRightLeaf = false;
            switch (matchMode) {
                //case KeywordMatchMode.Contains:
                //case KeywordMatchMode.EndsWith:
                //    // data isn't stored in this order, so we always check the node (assuming there is one)
                //    inspectRightLeaf = true;
                //    break;
                case KeywordMatchMode.ExactMatch:
                    // nothing more to do -- if the exact match occurred in this node, it won't occur in another node.
                    break;
                case KeywordMatchMode.StartsWith:
                case KeywordMatchMode.Contains:
                case KeywordMatchMode.EndsWith:
                    if (lastMatchIndex == Keywords.Count - 1) {
                        // last keyword matched, next leaf might have info in it.
                        inspectRightLeaf = true;
                    }
                    break;
            }



            if (inspectRightLeaf){
                var nextLeaf = BPlusTreeLeafNode<TKey, TValue>.Read(this.Tree, null, this.RightSiblingLocation) as BPlusTreeLeafNode<TKey, TValue>;
                if (nextLeaf != null) {
                    // we're not on the rightmost node, inspect the next one
                    nextLeaf.performSearch(keyword, matchMode, ignoreCase, returnLeafIfNotFound, matches);
                } else {
                    // we get here, no more leaves exist.
                    // just return, the matches collection is filled properly already.
                }

            } else {

                // we get here, it was bigger than our biggest keyword
                // Since we're a leaf, it doesn't exist.
                if (returnLeafIfNotFound) {
                    // add this keyword as matching -- so they know where at in the node to insert their new item (note Index = Keywords.Count, so it'll be tacked on the end)
                    matches.Add(new KeywordMatch<TKey, TValue> { Index = Keywords.Count, Keyword = default(TKey), Value = default(TValue), Node = this });
                }
            }

		}


	}
}
