using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GrinGlobal.Search.Engine {
	public class BPlusTreeIndexNode<TKey, TValue>
		: BPlusTreeNode<TKey, TValue> 
		where TKey : ISearchable<TKey>, new() 
		where TValue : IPersistable<TValue>, new() {

		public List<long> ChildLocations { get; internal set; }

		public long LastChildLocation {
			get {
				if (ChildLocations.Count == 0) {
					return -1;
				} else {
					return ChildLocations[ChildLocations.Count - 1];
				}
			}
		}

		public long MedianChildLocation {
			get {
				if (ChildLocations.Count > this.Tree.Median - 1) {
					// -1 because median is not 0-based, but our list is
					return ChildLocations[this.Tree.Median - 1];
				} else {
					return -1;
				}
			}
		}

		protected internal override void write(BinaryWriter wtr) {
			wtr.Write((short)ChildLocations.Count);
			foreach (long loc in ChildLocations) {
				wtr.Write(loc);
			}
		}

        protected override void ChildIsMoving(BPlusTreeNode<TKey, TValue> child, long newLocation) {
            Tree.Lock(delegate() {
                for (short i = 0; i < ChildLocations.Count; i++) {
                    if (ChildLocations[i] == child.Location) {
                        ChildLocations[i] = newLocation;
                        if (this.Write()) {
                            BPlusTree<TKey, TValue>.LogInfo("ChildIsMoving method relocated parent node=" + this.ToString());
                        }
                        break;
                    }
                }
            });
		}

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
				.Append(" ^ ")
				.Append(Parent == null ? "  -  " : Parent.Location.ToString().PadRight(5));
			for (int i=0; i < Keywords.Count; i++) {
				sb.Append(ChildLocations[i].ToString())
					.Append(" | ")
					.Append(Keywords[i])
					.Append(" | ");
			}
			if (ChildLocations.Count > Keywords.Count) {
				sb.Append(LastChildLocation.ToString());
			} else {
				sb.Append(" - ");
			}
			return sb.ToString();
		}

		protected internal BPlusTreeIndexNode(BPlusTree<TKey, TValue> tree, BPlusTreeIndexNode<TKey, TValue> parent, BinaryReader rdr)
			: base(tree, parent) {

			IsLeaf = false;
			ChildLocations = new List<long>();

			if (rdr != null) {
				// index nodes store only children locations (which are always longs)
				short childCount = rdr.ReadInt16();
				for (int i=0; i < childCount; i++) {
					ChildLocations.Add(rdr.ReadInt64());
				}
			}

		}

		protected internal override void performSearch(TKey keyword, KeywordMatchMode matchMode, bool ignoreCase, bool returnLeafIfNotFound, List<KeywordMatch<TKey, TValue>> matches) {

            try {
                BPlusTree<TKey, TValue>.LogInfo("Begin performSearch node=" + this.ToString() + " keyword=" + keyword.ToString() + ", returnLeafIfNotFound=" + returnLeafIfNotFound);
                int childIndex = ChildLocations.Count - 1;

                // given a keyword, drill until the containing leaf node is found (return null when not found)
                if (matchMode == KeywordMatchMode.Contains || matchMode == KeywordMatchMode.EndsWith) {
                    // Contains and EndsWith must check all leaf nodes since the data is ordered alphabetically
                    // left-to-right.  So we can skip checking keywords because we know we need to end up at the leftmost leaf node
                    childIndex = 0;
                } else {
                    for (short i = 0; i < Keywords.Count; i++) {
                        int compared = keyword.CompareTo(Keywords[i], matchMode, ignoreCase);
                        if (compared < 0) {
                            // given keyword is smaller than this entry.
                            // follow the left child.
                            childIndex = i;
                            break;
                        } else if (compared == 0) {
                            if (matchMode == KeywordMatchMode.StartsWith) {
                                // special case!  startswith doesn't mean there aren't more values to the left.
                                // so, we go left instead of right.
                                childIndex = i;
                            } else {
                                // given keyword is == this entry.
                                // follow right child.
                                childIndex = i + 1;
                            }
                            break;
                        } else {
                            // keep checking...
                        }
                    }
                }



                // either we found the proper index, or we ran out of keywords and the rightmost index is the proper index.
                // read in that child, split it if needed (if we're inserting), and search down that child
                BPlusTreeNode<TKey, TValue> child = BPlusTreeNode<TKey, TValue>.Read(this.Tree, this, ChildLocations[childIndex]);

                if (!returnLeafIfNotFound) {
                    // just continue the search down the child. if the node is full or not makes no difference -- they don't want us
                    // to return the leaf if it's not found anyway.
                    child.performSearch(keyword, matchMode, ignoreCase, returnLeafIfNotFound, matches);

                } else {
                    if (!child.IsFull) {
                        // just continue the search down the child. we might have to perform an insert somewhere down the line,
                        // but there's still room in this particular node so we shouldn't split it.
                        child.performSearch(keyword, matchMode, ignoreCase, returnLeafIfNotFound, matches);

                    } else {

                        // child node is full and they want us to return a node even if not found -- they're inserting.
                        // split the child, then continue the search down the appropriate one

                        // NOTE: this does pre-emptive node splits -- meaning we split on the way down instead of
                        //       splitting on the way back up.  this is common practice to help increase concurrency.
                        int newKeywordIndex = 0;
                        BPlusTreeNode<TKey, TValue> newChild = this.SplitChild(child, out newKeywordIndex);

                        int compared = keyword.CompareTo(Keywords[newKeywordIndex], matchMode, ignoreCase);
                        if (compared < 0) {
                            // new keyword is less than the keyword that was just copied/pushed into the parent (current) node.
                            // that new keyword belongs in the original child (left child)
                            child.performSearch(keyword, matchMode, ignoreCase, returnLeafIfNotFound, matches);
                        } else {
                            // keep checking...
                            newChild.performSearch(keyword, matchMode, ignoreCase, returnLeafIfNotFound, matches);
                        }
                    }
                }
            } finally {
                BPlusTree<TKey, TValue>.LogInfo("End performSearch node=" + this.ToString() + " keyword=" + keyword.ToString() + ", returnLeafIfNotFound=" + returnLeafIfNotFound);
            }
		}


        // overload for when the caller does not care about the keyword index
        internal BPlusTreeNode<TKey, TValue> SplitChild(BPlusTreeNode<TKey, TValue> originalChild) {
            int ki = 0;
            return SplitChild(originalChild, out ki);
        }


		internal BPlusTreeNode<TKey, TValue> SplitChild(BPlusTreeNode<TKey, TValue> originalChild, out int keywordIndex) {

			// Split the current node into two nodes:
			//   a. Left node contains all pointers/keywords to the left of the median
			//   b. Right node contains all pointers/keywords to the right of the median
			//   c. Parent node gets median keyword pushed into it
			//
			// Disk-wise: the originalChild is always written back to its original position (since we're splitting it, it's guaranteed to get smaller)
			//            the newChild is placed into either an available abandoned node or tacked on the end of the file.
			//            the parent node is written back to its original position IF IT FITS.  Since we're coyping a keyword
			//               up into the parent node, it may cause it to outgrow its allocated size in the file.  If this occurs,
			//               the call to this.Write() implicitly calls relocate().  relocate() will then either write the parent to
			//               an abandoned node or tack it onto the end of the file.



			// first, determine the offset of the current originalchild in this (the parent)
			keywordIndex = 0;

            try {
                BPlusTree<TKey, TValue>.LogInfo("Begin splitting node=" + this.ToString() + ", child=" + originalChild.ToString());

                if (ChildLocations.Count > 0) {
                    // TODO: troubleshoot this -- after several thousand splits, originalChild.Location is no longer found in the ChildLocations array!!!
                    //if (LastChildLocation == originalChild.Location) {
                    //    // rightmost child, on a full node.
                    //} else {
                    while (keywordIndex < ChildLocations.Count && ChildLocations[keywordIndex] != originalChild.Location) {
                        keywordIndex++;
                    }
                    //}
                }

                // assign the median keyword to the parent's Keywords list (inserts just before the current one)
                //if (keywordIndex > this.Keywords.Count) {
                //    // rightmost keyword, add to the end of the list
                //    this.Keywords.Add(originalChild.MedianKeyword);
                //} else {
                // not rightmost keyword, insert it at the proper position
                this.Keywords.Insert(keywordIndex, originalChild.MedianKeyword);
                //}

                if (originalChild.IsLeaf) {

                    // since this is a leaf, we will copy up the median keyword to the parent (as opposed to push up for index nodes)

                    // create a leaf node
                    BPlusTreeLeafNode<TKey, TValue> origNode = (BPlusTreeLeafNode<TKey, TValue>)originalChild;
                    BPlusTreeLeafNode<TKey, TValue> newNode = new BPlusTreeLeafNode<TKey, TValue>(this.Tree, (BPlusTreeIndexNode<TKey, TValue>)this, null);

                    // copy right half of keywords + values from originalChild to newChild
                    newNode.Keywords.AddRange(origNode.Keywords.Skip(this.Tree.Median - 1));
                    newNode.Values.AddRange(origNode.Values.Skip(this.Tree.Median - 1));

                    // remove right half of keywords + values from originalChild
                    origNode.Keywords = origNode.Keywords.Take(this.Tree.Median - 1).ToList();
                    origNode.Values = origNode.Values.Take(this.Tree.Median - 1).ToList();

                    var nextKeywordIndex = keywordIndex + 1;
                    Tree.Lock(delegate() {
                        // write out nodes so sibling locations can be set properly

                        // note: orig node should never relocate on write since we're making it smaller (moving 1/2 of its keywords to a new node)
                        if (origNode.Write()){
                            BPlusTree<TKey, TValue>.LogInfo("orig leaf node relocated during split child: " + origNode.ToString());
                        }
                        if (newNode.Write()){
                            BPlusTree<TKey, TValue>.LogInfo("new leaf node relocated during split child: " + newNode.ToString());
                        }

                        // adjust sibling locations (we always add the new child to the right of the original one)
                        newNode.RightSiblingLocation = origNode.RightSiblingLocation;
                        //				newNode.LeftSiblingLocation = origNode.Location;

                        origNode.RightSiblingLocation = newNode.Location;

                        ChildLocations.Insert(nextKeywordIndex, newNode.Location);

                        // write them out again so we save sibling locations
                        if (origNode.Write()){
                            BPlusTree<TKey, TValue>.LogInfo("orig leaf node relocated on 2nd write during split child: " + origNode.ToString());
                        }
                        if (newNode.Write()){
                            BPlusTree<TKey, TValue>.LogInfo("new leaf node relocated on 2nd write during split child: " + newNode.ToString());
                        }

                        // and write out the parent since it changed as well
                        if (Write()){
                            BPlusTree<TKey, TValue>.LogInfo("parent of leaf node relocated during split child: " + this.ToString());
                        }

                    });

                    return newNode;

                } else {

                    // since this is an index node, we push up the keyword to the parent (as opposed to copy up for leaves)
                    // this means the median keyword will be removed from the original child but not moved to the new child

                    // create an index node
                    BPlusTreeIndexNode<TKey, TValue> origNode = (BPlusTreeIndexNode<TKey, TValue>)originalChild;
                    BPlusTreeIndexNode<TKey, TValue> newNode = new BPlusTreeIndexNode<TKey, TValue>(this.Tree, (BPlusTreeIndexNode<TKey, TValue>)this, null);

                    // copy right half of keywords + childlocations from originalChild to newChild
                    // (since this is an index node, skip the median keyword
                    newNode.Keywords.AddRange(origNode.Keywords.Skip(this.Tree.Median));
                    newNode.ChildLocations.AddRange(origNode.ChildLocations.Skip(this.Tree.Median));

                    // remove right half of keywords + childlocations from originalChild
                    // (note we stop one shy of the median keyword -- this is because the median keyword needs to be "pushed up"
                    //  on an index node split.  However, we don't drop the corresponding ChildLocation)
                    origNode.Keywords = origNode.Keywords.Take(this.Tree.Median - 1).ToList();
                    origNode.ChildLocations = origNode.ChildLocations.Take(this.Tree.Median).ToList();

                    var ki = keywordIndex;
                    Tree.Lock(delegate() {
                        if (origNode.Write()) {
                            BPlusTree<TKey, TValue>.LogInfo("orig index node relocated during split child: " + origNode.ToString());
                        }
                        if (newNode.Write()) {
                            BPlusTree<TKey, TValue>.LogInfo("new index node relocated during split child: " + newNode.ToString());
                        }

                        // original node may have relocated on write...
                        ChildLocations[ki] = origNode.Location;

                        ChildLocations.Insert(ki + 1, newNode.Location);

                        // and write out the parent since it changed as well
                        if (Write()) {
                            BPlusTree<TKey, TValue>.LogInfo("parent of index node relocated during split child: " + this.ToString());
                        }
                    });

                    return newNode;

                }
            } finally {
                BPlusTree<TKey, TValue>.LogInfo("End splitting node=" + this.ToString() + ", child=" + originalChild.ToString());
            }
		}
	}
}
